// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::constant_offsets::ConstantOffsets;
use crate::instruction::Instruction;
use crate::utils::to_value_error;
use core::slice;
use pyo3::exceptions::PyTypeError;
use pyo3::gc::PyVisit;
use pyo3::prelude::*;
use pyo3::types::{PyByteArray, PyBytes};
use pyo3::PyTraverseError;

enum DecoderDataRef {
	None,
	Vec(#[allow(dead_code)] Vec<u8>),
	#[allow(dead_code)]
	PyObj(PyObject),
}

/// Decodes 16/32/64-bit x86 instructions
///
/// Args:
///     bitness (int): 16, 32 or 64
///     data (bytes, bytearray): Data to decode. For best PERF, use :class:`bytes` since it's immutable and nothing gets copied.
///     options (:class:`DecoderOptions`): (default = :class:`DecoderOptions.NONE`) Decoder options, eg. :class:`DecoderOptions.NO_INVALID_CHECK` | :class:`DecoderOptions.AMD`
///     ip (int): (``u64``) (default = ``0``) ``RIP`` value
///
/// Raises:
///     ValueError: If `bitness` is invalid
///     TypeError: If `data` is not a supported type
///
/// Examples:
///
/// .. testcode::
///
///     from iced_x86 import *
///
///     data = b"\x86\x64\x32\x16\xF0\xF2\x83\x00\x5A\x62\xC1\xFE\xCB\x6F\xD3"
///     decoder = Decoder(64, data, ip=0x1234_5678)
///
///     # The decoder is iterable
///     for instr in decoder:
///         print(f"Decoded: IP=0x{instr.ip:X}: {instr}")
///
/// Output:
///
/// .. testoutput::
///
///     Decoded: IP=0x12345678: xchg ah,[rdx+rsi+16h]
///     Decoded: IP=0x1234567C: xacquire lock add dword ptr [rax],5Ah
///     Decoded: IP=0x12345681: vmovdqu64 zmm18{k3}{z},zmm11
///
/// .. testcode::
///
///     from iced_x86 import *
///
///     # xchg ah,[rdx+rsi+16h]
///     # xacquire lock add dword ptr [rax],5Ah
///     # vmovdqu64 zmm18{k3}{z},zmm11
///     data = b"\x86\x64\x32\x16\xF0\xF2\x83\x00\x5A\x62\xC1\xFE\xCB\x6F\xD3"
///     decoder = Decoder(64, data, ip=0x1234_5678)
///
///     instr1 = decoder.decode()
///     assert instr1.code == Code.XCHG_RM8_R8
///     assert instr1.mnemonic == Mnemonic.XCHG
///     assert instr1.len == 4
///
///     instr2 = decoder.decode()
///     assert instr2.code == Code.ADD_RM32_IMM8
///     assert instr2.mnemonic == Mnemonic.ADD
///     assert instr2.len == 5
///
///     instr3 = decoder.decode()
///     assert instr3.code == Code.EVEX_VMOVDQU64_ZMM_K1Z_ZMMM512
///     assert instr3.mnemonic == Mnemonic.VMOVDQU64
///     assert instr3.len == 6
///
/// It's sometimes useful to decode some invalid instructions, eg. ``lock add esi,ecx``.
/// Pass in :class:`DecoderOptions.NO_INVALID_CHECK` to the constructor and the decoder
/// will decode some invalid encodings.
///
/// .. testcode::
///
///     from iced_x86 import *
///
///     # lock add esi,ecx   # lock not allowed
///     data = b"\xF0\x01\xCE"
///     decoder = Decoder(64, data, ip=0x1234_5678)
///     instr = decoder.decode()
///     assert instr.code == Code.INVALID
///
///     # We want to decode some instructions with invalid encodings
///     decoder = Decoder(64, data, DecoderOptions.NO_INVALID_CHECK, 0x1234_5678)
///     instr = decoder.decode()
///     assert instr.code == Code.ADD_RM32_R32
///     assert instr.has_lock_prefix
#[pyclass(module = "iced_x86._iced_x86_py")]
pub(crate) struct Decoder {
	// * If the decoder ctor was called with a `bytes` object, data_ref is PyObj(`bytes` object)
	//   and the decoder holds a ref to its data.
	// * If the decoder ctor was called with a `bytearray` object, data_ref is Vec(copy of `bytearray` data)
	//   and the decoder holds a reference to this copied data.
	data_ref: DecoderDataRef,
	decoder: iced_x86::Decoder<'static>,
}

#[pymethods]
impl Decoder {
	#[new]
	#[pyo3(text_signature = "(bitness, data, options = 0, ip = 0)")]
	#[pyo3(signature = (bitness, data, options = 0, ip = 0))]
	fn new(bitness: u32, data: &Bound<'_, PyAny>, options: u32, ip: u64) -> PyResult<Self> {
		// #[pyo3(signature = (...))] line assumption
		const _: () = assert!(iced_x86::DecoderOptions::NONE == 0);

		let (data_ref, decoder_data): (DecoderDataRef, &'static [u8]) = if let Ok(bytes) = data.downcast::<PyBytes>() {
			//TODO: try to use a reference to the original data like we did with PyO3 0.20 and earlier, see previous commit
			let vec_data: Vec<_> = bytes.as_bytes().into();
			let decoder_data = unsafe { slice::from_raw_parts(vec_data.as_ptr(), vec_data.len()) };
			(DecoderDataRef::Vec(vec_data), decoder_data)
		} else if let Ok(bytearray) = data.downcast::<PyByteArray>() {
			//TODO: support bytearray without copying its data by getting a ref to its data every time the Decoder is used (also update the ctor args docs)
			let vec_data: Vec<_> = unsafe { bytearray.as_bytes().into() };
			let decoder_data = unsafe { slice::from_raw_parts(vec_data.as_ptr(), vec_data.len()) };
			(DecoderDataRef::Vec(vec_data), decoder_data)
		} else {
			//TODO: support memoryview (also update docs and get_temporary_byte_array_ref and the message below)
			return Err(PyTypeError::new_err("Expected one of these types: bytes, bytearray"));
		};

		let decoder = iced_x86::Decoder::try_with_ip(bitness, decoder_data, ip, options).map_err(to_value_error)?;
		Ok(Decoder { data_ref, decoder })
	}

	/// int: (``u64``) The current ``IP``/``EIP``/``RIP`` value, see also :class:`Decoder.position`
	///
	/// Note:
	///     The setter only updates the IP value, it does not change the data position, use the :class:`Decoder.position` setter to change the position.
	#[getter]
	fn ip(&self) -> u64 {
		self.decoder.ip()
	}

	#[setter]
	fn set_ip(&mut self, new_value: u64) {
		self.decoder.set_ip(new_value);
	}

	/// int: Gets the bitness (16, 32 or 64)
	#[getter]
	fn bitness(&self) -> u32 {
		self.decoder.bitness()
	}

	/// int: (``usize``) Gets the max value that can be written to :class:`Decoder.position`.
	///
	/// This is the size of the data that gets decoded to instructions and it's the length of the data that was passed to the constructor.
	#[getter]
	fn max_position(&self) -> usize {
		self.decoder.max_position()
	}

	/// int: (``usize``) The current data position, which is the index into the data passed to the constructor.
	///
	/// This value is always <= :class:`Decoder.max_position`. When :class:`Decoder.position` == :class:`Decoder.max_position`, it's not possible to decode more
	/// instructions and :class:`Decoder.can_decode` returns ``False``.
	///
	/// Raises:
	///     ValueError: If the new position is invalid.
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     # nop and pause
	///     data = b"\x90\xF3\x90"
	///     decoder = Decoder(64, data, ip=0x1234_5678)
	///
	///     assert decoder.position == 0
	///     assert decoder.max_position == 3
	///     instr = decoder.decode()
	///     assert decoder.position == 1
	///     assert instr.code == Code.NOPD
	///
	///     instr = decoder.decode()
	///     assert decoder.position == 3
	///     assert instr.code == Code.PAUSE
	///
	///     # Start all over again
	///     decoder.position = 0
	///     decoder.ip = 0x1234_5678
	///     assert decoder.position == 0
	///     assert decoder.decode().code == Code.NOPD
	///     assert decoder.decode().code == Code.PAUSE
	///     assert decoder.position == 3
	#[getter]
	fn position(&self) -> usize {
		self.decoder.position()
	}

	#[setter]
	fn set_position(&mut self, new_value: usize) -> PyResult<()> {
		self.decoder.set_position(new_value).map_err(to_value_error)
	}

	/// bool: Returns ``True`` if there's at least one more byte to decode.
	///
	/// It doesn't verify that the next instruction is valid, it only checks if there's
	/// at least one more byte to read. See also :class:`Decoder.position` and :class:`Decoder.max_position`.
	///
	/// It's not required to call this method. If this method returns ``False``, then :class:`Decoder.decode_out`
	/// and :class:`Decoder.decode` will return an instruction whose :class:`Instruction.code` == :class:`Code.INVALID`.
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     # nop and an incomplete instruction
	///     data = b"\x90\xF3\x0F"
	///     decoder = Decoder(64, data, ip=0x1234_5678)
	///
	///     # 3 bytes left to read
	///     assert decoder.can_decode
	///     instr = decoder.decode()
	///     assert instr.code == Code.NOPD
	///
	///     # 2 bytes left to read
	///     assert decoder.can_decode
	///     instr = decoder.decode()
	///     # Not enough bytes left to decode a full instruction
	///     assert decoder.last_error == DecoderError.NO_MORE_BYTES
	///     assert instr.code == Code.INVALID
	///     assert not instr
	///     assert instr.is_invalid
	///
	///     # 0 bytes left to read
	///     assert not decoder.can_decode
	#[getter]
	fn can_decode(&self) -> bool {
		self.decoder.can_decode()
	}

	/// :class:`DecoderError`: Gets the last decoder error (a :class:`DecoderError` enum value).
	///
	/// Unless you need to know the reason it failed, it's better to check :class:`Instruction.is_invalid` or ``if not instruction:``.
	#[getter]
	fn last_error(&self) -> u32 {
		self.decoder.last_error() as u32
	}

	/// Decodes and returns the next instruction.
	///
	/// See also :class:`Decoder.decode_out` which avoids copying the decoded instruction to the caller's return variable.
	/// See also :class:`Decoder.last_error`.
	///
	/// Returns:
	///     Instruction: The next instruction
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     # xrelease lock add [rax],ebx
	///     data = b"\xF0\xF3\x01\x18"
	///     decoder = Decoder(64, data, ip=0x1234_5678)
	///     instr = decoder.decode()
	///
	///     assert instr.code == Code.ADD_RM32_R32
	///     assert instr.mnemonic == Mnemonic.ADD
	///     assert instr.len == 4
	///     assert instr.op_count == 2
	///
	///     assert instr.op0_kind == OpKind.MEMORY
	///     assert instr.memory_base == Register.RAX
	///     assert instr.memory_index == Register.NONE
	///     assert instr.memory_index_scale == 1
	///     assert instr.memory_displacement == 0
	///     assert instr.memory_segment == Register.DS
	///     assert instr.segment_prefix == Register.NONE
	///     assert instr.memory_size == MemorySize.UINT32
	///
	///     assert instr.op1_kind == OpKind.REGISTER
	///     assert instr.op1_register == Register.EBX
	///
	///     assert instr.has_lock_prefix
	///     assert instr.has_xrelease_prefix
	#[pyo3(text_signature = "($self)")]
	fn decode(&mut self) -> Instruction {
		Instruction { instr: self.decoder.decode() }
	}

	/// Decodes the next instruction.
	///
	/// The difference between this method and :class:`Decoder.decode` is that this method doesn't need to
	/// allocate a new instruction since it overwrites the input instruction.
	///
	/// See also :class:`Decoder.last_error`.
	///
	/// Args:
	///     instruction (:class:`Instruction`): Updated with the decoded instruction.
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     # xrelease lock add [rax],ebx
	///     data = b"\xF0\xF3\x01\x18"
	///     decoder = Decoder(64, data, ip=0x1234_5678)
	///     instr = Instruction()
	///     decoder.decode_out(instr)
	///
	///     assert instr.code == Code.ADD_RM32_R32
	///     assert instr.mnemonic == Mnemonic.ADD
	///     assert instr.len == 4
	///     assert instr.op_count == 2
	///
	///     assert instr.op0_kind == OpKind.MEMORY
	///     assert instr.memory_base == Register.RAX
	///     assert instr.memory_index == Register.NONE
	///     assert instr.memory_index_scale == 1
	///     assert instr.memory_displacement == 0
	///     assert instr.memory_segment == Register.DS
	///     assert instr.segment_prefix == Register.NONE
	///     assert instr.memory_size == MemorySize.UINT32
	///
	///     assert instr.op1_kind == OpKind.REGISTER
	///     assert instr.op1_register == Register.EBX
	///
	///     assert instr.has_lock_prefix
	///     assert instr.has_xrelease_prefix
	#[pyo3(text_signature = "($self, instruction)")]
	fn decode_out(&mut self, instruction: &mut Instruction) {
		self.decoder.decode_out(&mut instruction.instr)
	}

	/// Gets the offsets of the constants (memory displacement and immediate) in the decoded instruction.
	///
	/// The caller can check if there are any relocations at those addresses.
	///
	/// Args:
	///     instruction (:class:`Instruction`): The latest instruction that was decoded by this decoder
	///
	/// Returns:
	///     ConstantOffsets: Offsets and sizes of immediates
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     # nop
	///     # xor dword ptr [rax-5AA5EDCCh],5Ah
	///     #              00  01  02  03  04  05  06
	///     #            \opc\mrm\displacement___\imm
	///     data = b"\x90\x83\xB3\x34\x12\x5A\xA5\x5A"
	///     decoder = Decoder(64, data, ip=0x1234_5678)
	///     assert decoder.decode().code == Code.NOPD
	///     instr = decoder.decode()
	///     co = decoder.get_constant_offsets(instr)
	///
	///     assert co.has_displacement
	///     assert co.displacement_offset == 2
	///     assert co.displacement_size == 4
	///     assert co.has_immediate
	///     assert co.immediate_offset == 6
	///     assert co.immediate_size == 1
	///     # It's not an instruction with two immediates (e.g. enter)
	///     assert not co.has_immediate2
	///     assert co.immediate_offset2 == 0
	///     assert co.immediate_size2 == 0
	#[pyo3(text_signature = "($self, instruction)")]
	fn get_constant_offsets(&self, instruction: &Instruction) -> ConstantOffsets {
		ConstantOffsets { offsets: self.decoder.get_constant_offsets(&instruction.instr) }
	}

	fn __traverse__(&self, visit: PyVisit<'_>) -> Result<(), PyTraverseError> {
		if let DecoderDataRef::PyObj(ref data_obj) = self.data_ref {
			visit.call(data_obj)?
		}
		Ok(())
	}

	fn __clear__(&mut self) {
		if let DecoderDataRef::PyObj(_) = self.data_ref {
			self.decoder = iced_x86::Decoder::new(64, b"", iced_x86::DecoderOptions::NONE);
			self.data_ref = DecoderDataRef::None;
		}
	}

	fn __iter__(slf: PyRef<'_, Self>) -> PyRef<'_, Self> {
		slf
	}

	fn __next__(mut slf: PyRefMut<'_, Self>) -> Option<Instruction> {
		if slf.can_decode() {
			Some(slf.decode())
		} else {
			None
		}
	}
}
