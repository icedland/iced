/*
Copyright (C) 2018-2019 de4dot@gmail.com

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

use crate::instruction::Instruction;
use core::slice;
use pyo3::class::iter::IterNextOutput;
use pyo3::exceptions::{PyTypeError, PyValueError};
use pyo3::gc::{PyGCProtocol, PyVisit};
use pyo3::prelude::*;
use pyo3::types::{PyByteArray, PyBytes};
use pyo3::{PyIterProtocol, PyTraverseError};

enum DecoderDataRef {
	None,
	Vec(Vec<u8>),
	PyObj(PyObject),
}

/// Decodes 16/32/64-bit x86 instructions
///
/// Args:
/// 	bitness (:class:`int`): 16, 32 or 64
/// 	data (:class:`bytes` | :class:`bytearray`): Data to decode. For best PERF, use :class:`bytes` since it's immutable and nothing gets copied.
/// 	options (:class:`int`): (default = 0) Decoder options, eg. `DecoderOptions.NO_INVALID_CHECK | DecoderOptions.AMD`
///
/// Raises:
/// 	ValueError: If `bitness` is invalid
/// 	TypeError: If `data` is not a supported type
///
/// Examples:
///
/// ```
/// use iced_x86::*;
///
/// // xchg ah,[rdx+rsi+16h]
/// // xacquire lock add dword ptr [rax],5Ah
/// // vmovdqu64 zmm18{k3}{z},zmm11
/// let bytes = b"\x86\x64\x32\x16\xF0\xF2\x83\x00\x5A\x62\xC1\xFE\xCB\x6F\xD3";
/// let mut decoder = Decoder::new(64, bytes, DecoderOptions::NOTHING);
/// decoder.set_ip(0x1234_5678);
///
/// let instr1 = decoder.decode();
/// assert_eq!(Code::Xchg_rm8_r8, instr1.code());
/// assert_eq!(Mnemonic::Xchg, instr1.mnemonic());
/// assert_eq!(4, instr1.len());
///
/// let instr2 = decoder.decode();
/// assert_eq!(Code::Add_rm32_imm8, instr2.code());
/// assert_eq!(Mnemonic::Add, instr2.mnemonic());
/// assert_eq!(5, instr2.len());
///
/// let instr3 = decoder.decode();
/// assert_eq!(Code::EVEX_Vmovdqu64_zmm_k1z_zmmm512, instr3.code());
/// assert_eq!(Mnemonic::Vmovdqu64, instr3.mnemonic());
/// assert_eq!(6, instr3.len());
/// ```
///
/// It's sometimes useful to decode some invalid instructions, eg. `lock add esi,ecx`.
/// Pass in `DecoderOptions::NO_INVALID_CHECK` to the constructor and the decoder
/// will decode some invalid encodings.
///
/// ```
/// use iced_x86::*;
///
/// // lock add esi,ecx   ; lock not allowed
/// let bytes = b"\xF0\x01\xCE";
/// let mut decoder = Decoder::new(64, bytes, DecoderOptions::NOTHING);
/// decoder.set_ip(0x1234_5678);
/// let instr = decoder.decode();
/// assert_eq!(Code::INVALID, instr.code());
///
/// // We want to decode some instructions with invalid encodings
/// let mut decoder = Decoder::new(64, bytes, DecoderOptions::NO_INVALID_CHECK);
/// decoder.set_ip(0x1234_5678);
/// let instr = decoder.decode();
/// assert_eq!(Code::Add_rm32_r32, instr.code());
/// assert!(instr.has_lock_prefix());
/// ```
#[pyclass(module = "iced_x86_py")]
#[text_signature = "(bitness, data, options, /)"]
pub struct Decoder {
	// * If the decoder ctor was called with a `bytes` object, data_ref is PyObj(`bytes` object)
	//   and the decoder holds a ref to its data.
	// * If the decoder ctor was called with a `bytearray` object, data_ref is Vec(copy of `bytearray` data)
	//   and the decoder holds a reference to this copied data.
	data_ref: DecoderDataRef,
	decoder: iced_x86::Decoder<'static>,
}

// iced_x86::Decoder has read only pointer fields which are !Send
unsafe impl Send for Decoder {}

#[pymethods]
impl Decoder {
	#[new]
	#[args(options = 0)]
	fn new(bitness: u32, data: &PyAny, options: u32) -> PyResult<Self> {
		match bitness {
			16 | 32 | 64 => {}
			_ => return Err(PyValueError::new_err("bitness must be 16, 32 or 64")),
		}

		let (data_ref, decoder_data): (DecoderDataRef, &'static [u8]) = if let Ok(bytes) = <PyBytes as PyTryFrom>::try_from(data) {
			let slice_data = bytes.as_bytes();
			let decoder_data = unsafe { slice::from_raw_parts(slice_data.as_ptr(), slice_data.len()) };
			(DecoderDataRef::PyObj(bytes.into()), decoder_data)
		} else if let Ok(bytearray) = <PyByteArray as PyTryFrom>::try_from(data) {
			//TODO: support bytearray without copying its data by getting a ref to its data every time the Decoder is used (also update the ctor args docs)
			let vec_data: Vec<_> = unsafe { bytearray.as_bytes().into() };
			let decoder_data = unsafe { slice::from_raw_parts(vec_data.as_ptr(), vec_data.len()) };
			(DecoderDataRef::Vec(vec_data), decoder_data)
		} else {
			//TODO: support memoryview
			return Err(PyTypeError::new_err("Expected one of these types: bytes, bytearray"));
		};

		let decoder = iced_x86::Decoder::new(bitness, decoder_data, options);
		Ok(Decoder { data_ref, decoder })
	}

	/// The current `IP`/`EIP`/`RIP` value, see also `position`
	///
	/// Note:
	/// 	The setter only updates the IP value, it does not change the data position, use the `position` setter to change the position.
	#[getter]
	fn ip(&self) -> u64 {
		self.decoder.ip()
	}

	#[setter]
	fn set_ip(&mut self, new_value: u64) {
		self.decoder.set_ip(new_value);
	}

	/// Gets the bitness (16, 32 or 64)
	#[getter]
	fn bitness(&self) -> u32 {
		self.decoder.bitness()
	}

	/// Gets the max value that can be written to `position`.
	///
	/// This is the size of the data that gets decoded to instructions and it's the length of the data that was passed to the constructor.
	#[getter]
	fn max_position(&self) -> usize {
		self.decoder.max_position()
	}

	/// The current data position, which is the index into the data passed to the constructor.
	///
	/// This value is always <= `max_position`. When `position` == `max_position`, it's not possible to decode more
	/// instructions and `can_decode` returns `False`.
	///
	/// Raises:
	/// 	TODO SomeError: If the new position is invalid.
	///
	/// Examples:
	///
	/// ```
	/// use iced_x86::*;
	///
	/// // nop and pause
	/// let bytes = b"\x90\xF3\x90";
	/// let mut decoder = Decoder::new(64, bytes, DecoderOptions::NOTHING);
	/// decoder.set_ip(0x1234_5678);
	///
	/// assert_eq!(0, decoder.position());
	/// assert_eq!(3, decoder.max_position());
	/// let instr = decoder.decode();
	/// assert_eq!(1, decoder.position());
	/// assert_eq!(Code::Nopd, instr.code());
	///
	/// let instr = decoder.decode();
	/// assert_eq!(3, decoder.position());
	/// assert_eq!(Code::Pause, instr.code());
	///
	/// // Start all over again
	/// decoder.set_position(0);
	/// assert_eq!(0, decoder.position());
	/// assert_eq!(Code::Nopd, decoder.decode().code());
	/// assert_eq!(Code::Pause, decoder.decode().code());
	/// assert_eq!(3, decoder.position());
	/// ```
	#[getter]
	fn position(&self) -> usize {
		self.decoder.position()
	}

	#[setter]
	fn set_position(&mut self, new_pos: usize) {
		self.decoder.set_position(new_pos);
	}

	/// Returns `True` if there's at least one more byte to decode.
	///
	/// It doesn't verify that the next instruction is valid, it only checks if there's
	/// at least one more byte to read. See also `position` and `max_position`.
	///
	/// It's not required to call this method. If this method returns `False`, then `decode_out()`
	/// and `decode()` will return an instruction whose `code` == `Code.INVALID`.
	///
	/// Examples:
	///
	/// ```
	/// use iced_x86::*;
	///
	/// // nop and an incomplete instruction
	/// let bytes = b"\x90\xF3\x0F";
	/// let mut decoder = Decoder::new(64, bytes, DecoderOptions::NOTHING);
	/// decoder.set_ip(0x1234_5678);
	///
	/// // 3 bytes left to read
	/// assert!(decoder.can_decode());
	/// let instr = decoder.decode();
	/// assert_eq!(Code::Nopd, instr.code());
	///
	/// // 2 bytes left to read
	/// assert!(decoder.can_decode());
	/// let instr = decoder.decode();
	/// // Not enough bytes left to decode a full instruction
	/// assert_eq!(Code::INVALID, instr.code());
	///
	/// // 0 bytes left to read
	/// assert!(!decoder.can_decode());
	/// ```
	#[getter]
	fn can_decode(&self) -> bool {
		self.decoder.can_decode()
	}

	/// Gets the last decoder error (a `DecoderError` enum value).
	///
	/// Unless you need to know the reason it failed, it's better to check `instruction.is_invalid` or `if not instruction:`.
	#[getter]
	fn last_error(&self) -> u32 {
		self.decoder.last_error() as u32
	}

	/// Decodes and returns the next instruction.
	///
	/// See also `decode_out(instruction)` which avoids copying the decoded instruction to the caller's return variable.
	/// See also `last_error`.
	///
	/// Examples:
	///
	/// ```
	/// use iced_x86::*;
	///
	/// // xrelease lock add [rax],ebx
	/// let bytes = b"\xF0\xF3\x01\x18";
	/// let mut decoder = Decoder::new(64, bytes, DecoderOptions::NOTHING);
	/// decoder.set_ip(0x1234_5678);
	/// let instr = decoder.decode();
	///
	/// assert_eq!(Code::Add_rm32_r32, instr.code());
	/// assert_eq!(Mnemonic::Add, instr.mnemonic());
	/// assert_eq!(4, instr.len());
	/// assert_eq!(2, instr.op_count());
	///
	/// assert_eq!(OpKind::Memory, instr.op0_kind());
	/// assert_eq!(Register::RAX, instr.memory_base());
	/// assert_eq!(Register::None, instr.memory_index());
	/// assert_eq!(1, instr.memory_index_scale());
	/// assert_eq!(0, instr.memory_displacement());
	/// assert_eq!(Register::DS, instr.memory_segment());
	/// assert_eq!(Register::None, instr.segment_prefix());
	/// assert_eq!(MemorySize::UInt32, instr.memory_size());
	///
	/// assert_eq!(OpKind::Register, instr.op1_kind());
	/// assert_eq!(Register::EBX, instr.op1_register());
	///
	/// assert!(instr.has_lock_prefix());
	/// assert!(instr.has_xrelease_prefix());
	/// ```
	#[text_signature = "($self, /)"]
	fn decode(&mut self) -> Instruction {
		Instruction { instr: self.decoder.decode() }
	}

	/// Decodes the next instruction.
	///
	/// The difference between this method and `decode()` is that this method doesn't need to copy the result to
	/// the caller's return variable (saves 32-bytes of copying).
	///
	/// See also `last_error`.
	///
	/// Args:
	/// 	instruction (:class:`Instruction`): Updated with the decoded instruction. All fields are initialized (it's an `out` argument)
	///
	/// Examples:
	///
	/// ```
	/// use iced_x86::*;
	///
	/// // xrelease lock add [rax],ebx
	/// let bytes = b"\xF0\xF3\x01\x18";
	/// let mut decoder = Decoder::new(64, bytes, DecoderOptions::NOTHING);
	/// decoder.set_ip(0x1234_5678);
	/// let mut instr = Instruction::default();
	/// decoder.decode_out(&mut instr);
	///
	/// assert_eq!(Code::Add_rm32_r32, instr.code());
	/// assert_eq!(Mnemonic::Add, instr.mnemonic());
	/// assert_eq!(4, instr.len());
	/// assert_eq!(2, instr.op_count());
	///
	/// assert_eq!(OpKind::Memory, instr.op0_kind());
	/// assert_eq!(Register::RAX, instr.memory_base());
	/// assert_eq!(Register::None, instr.memory_index());
	/// assert_eq!(1, instr.memory_index_scale());
	/// assert_eq!(0, instr.memory_displacement());
	/// assert_eq!(Register::DS, instr.memory_segment());
	/// assert_eq!(Register::None, instr.segment_prefix());
	/// assert_eq!(MemorySize::UInt32, instr.memory_size());
	///
	/// assert_eq!(OpKind::Register, instr.op1_kind());
	/// assert_eq!(Register::EBX, instr.op1_register());
	///
	/// assert!(instr.has_lock_prefix());
	/// assert!(instr.has_xrelease_prefix());
	/// ```
	#[text_signature = "($self, instruction, /)"]
	fn decode_out(&mut self, instruction: &mut Instruction) {
		self.decoder.decode_out(&mut instruction.instr)
	}

	/// Gets the offsets of the constants (memory displacement and immediate) in the decoded instruction.
	///
	/// The caller can check if there are any relocations at those addresses.
	///
	/// Args:
	/// 	instruction (:class:`Instruction`): The latest instruction that was decoded by this decoder
	///
	/// Examples:
	///
	/// ```
	/// use iced_x86::*;
	///
	/// // nop
	/// // xor dword ptr [rax-5AA5EDCCh],5Ah
	/// //                  00  01  02  03  04  05  06
	/// //                \opc\mrm\displacement___\imm
	/// let bytes = b"\x90\x83\xB3\x34\x12\x5A\xA5\x5A";
	/// let mut decoder = Decoder::new(64, bytes, DecoderOptions::NOTHING);
	/// decoder.set_ip(0x1234_5678);
	/// assert_eq!(Code::Nopd, decoder.decode().code());
	/// let instr = decoder.decode();
	/// let co = decoder.get_constant_offsets(&instr);
	///
	/// assert!(co.has_displacement());
	/// assert_eq!(2, co.displacement_offset());
	/// assert_eq!(4, co.displacement_size());
	/// assert!(co.has_immediate());
	/// assert_eq!(6, co.immediate_offset());
	/// assert_eq!(1, co.immediate_size());
	/// // It's not an instruction with two immediates (e.g. enter)
	/// assert!(!co.has_immediate2());
	/// assert_eq!(0, co.immediate_offset2());
	/// assert_eq!(0, co.immediate_size2());
	/// ```
	#[text_signature = "($self, instruction, /)"]
	fn get_constant_offsets(&self, _instruction: &Instruction) {
		//TODO:
	}
}

#[pyproto]
impl PyGCProtocol for Decoder {
	fn __traverse__(&self, visit: PyVisit) -> Result<(), PyTraverseError> {
		if let DecoderDataRef::PyObj(ref data_obj) = self.data_ref {
			visit.call(data_obj)?
		}
		Ok(())
	}

	fn __clear__(&mut self) {
		if let DecoderDataRef::PyObj(_) = self.data_ref {
			self.data_ref = DecoderDataRef::None;
		}
	}
}

#[pyproto]
impl PyIterProtocol for Decoder {
	fn __iter__(slf: PyRef<Self>) -> PyRef<Self> {
		slf
	}

	fn __next__(mut slf: PyRefMut<Self>) -> IterNextOutput<Instruction, ()> {
		if slf.decoder.can_decode() {
			IterNextOutput::Yield(slf.decode())
		} else {
			IterNextOutput::Return(())
		}
	}
}
