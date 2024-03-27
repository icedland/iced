// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::constant_offsets::ConstantOffsets;
use crate::instruction::Instruction;
use crate::utils::to_value_error;
use pyo3::prelude::*;
use pyo3::types::PyBytes;

/// Encodes instructions decoded by the decoder or instructions created by other code.
///
/// See also :class:`BlockEncoder` which can encode any number of instructions.
///
/// Args:
///     bitness (int): 16, 32 or 64
///     capacity (int): (default = 0) Initial capacity of the byte buffer
///
/// Raises:
///     ValueError: If `bitness` is invalid
///
/// Examples:
///
/// .. testcode::
///
///     from iced_x86 import *
///
///     # xchg ah,[rdx+rsi+16h]
///     data = b"\x86\x64\x32\x16"
///     decoder = Decoder(64, data, ip=0x1234_5678)
///     instr = decoder.decode()
///
///     encoder = Encoder(64)
///     try:
///         instr_len = encoder.encode(instr, 0x5555_5555)
///         assert instr_len == 4
///     except ValueError as ex:
///         print(f"Failed to encode the instruction: {ex}")
///         raise
///
///     # We're done, take ownership of the buffer
///     buffer = encoder.take_buffer()
///     assert buffer == b"\x86\x64\x32\x16"
#[pyclass(module = "iced_x86._iced_x86_py")]
pub(crate) struct Encoder {
	encoder: iced_x86::Encoder,
}

#[pymethods]
impl Encoder {
	#[new]
	#[pyo3(text_signature = "(bitness, capacity = 0)")]
	#[pyo3(signature = (bitness, capacity = 0))]
	fn new(bitness: u32, capacity: usize) -> PyResult<Self> {
		let encoder = iced_x86::Encoder::try_with_capacity(bitness, capacity).map_err(to_value_error)?;
		Ok(Self { encoder })
	}

	/// Encodes an instruction and returns the size of the encoded instruction
	///
	/// Args:
	///     `instruction` (Instruction): Instruction to encode
	///     `rip` (int): (``u64``) ``RIP`` of the encoded instruction
	///
	/// Returns:
	///     int: Size of the encoded instruction
	///
	/// Raises:
	///     ValueError: If it failed to encode the instruction (eg. a target branch / RIP-rel operand is too far away)
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     # je short $+4
	///     data = b"\x75\x02"
	///     decoder = Decoder(64, data, ip=0x1234_5678)
	///     instr = decoder.decode()
	///
	///     encoder = Encoder(64)
	///     try:
	///         # Use a different IP (orig rip + 0x10)
	///         instr_len = encoder.encode(instr, 0x1234_5688)
	///         assert instr_len == 2
	///     except ValueError as ex:
	///         print(f"Failed to encode the instruction: {ex}")
	///         raise
	///
	///     # We're done, take ownership of the buffer
	///     buffer = encoder.take_buffer()
	///     assert buffer == b"\x75\xF2"
	#[pyo3(text_signature = "($self, instruction, rip)")]
	fn encode(&mut self, instruction: &Instruction, rip: u64) -> PyResult<usize> {
		self.encoder.encode(&instruction.instr, rip).map_err(to_value_error)
	}

	/// Writes a byte to the output buffer
	///
	/// Args:
	///     `value` (int): (``u8``) Value to write
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     # je short $+4
	///     data = b"\x75\x02"
	///     decoder = Decoder(64, data, ip=0x1234_5678)
	///     instr = decoder.decode()
	///
	///     encoder = Encoder(64)
	///     # Add a random byte
	///     encoder.write_u8(0x90)
	///
	///     try:
	///         # Use a different IP (orig rip + 0x10)
	///         instr_len = encoder.encode(instr, 0x1234_5688)
	///         assert instr_len == 2
	///     except ValueError as ex:
	///         print(f"Failed to encode the instruction: {ex}")
	///         raise
	///
	///     # Add a random byte
	///     encoder.write_u8(0x90)
	///
	///     # We're done, take ownership of the buffer
	///     buffer = encoder.take_buffer()
	///     assert buffer == b"\x90\x75\xF2\x90"
	#[pyo3(text_signature = "($self, value)")]
	fn write_u8(&mut self, value: u8) {
		self.encoder.write_u8(value)
	}

	/// Returns the buffer and initializes the internal buffer to an empty array.
	///
	/// Should be called when you've encoded all instructions and need the raw instruction bytes.
	///
	/// Returns:
	///     bytes: The encoded instructions
	#[pyo3(text_signature = "($self)")]
	fn take_buffer<'py>(&mut self, py: Python<'py>) -> Bound<'py, PyBytes> {
		let buffer = self.encoder.take_buffer();
		PyBytes::new_bound(py, &buffer)
	}

	/// Gets the offsets of the constants (memory displacement and immediate) in the encoded instruction.
	///
	/// The caller can use this information to add relocations if needed.
	///
	/// Returns:
	///     ConstantOffsets: Offsets and sizes of immediates
	#[pyo3(text_signature = "($self)")]
	fn get_constant_offsets(&self) -> ConstantOffsets {
		ConstantOffsets { offsets: self.encoder.get_constant_offsets() }
	}

	/// bool: Disables 2-byte VEX encoding and encodes all VEX instructions with the 3-byte VEX encoding
	#[getter]
	fn prevent_vex2(&self) -> bool {
		self.encoder.prevent_vex2()
	}

	#[setter]
	fn set_prevent_vex2(&mut self, new_value: bool) {
		self.encoder.set_prevent_vex2(new_value)
	}

	/// int: (``u8``) Value of the ``VEX.W`` bit to use if it's an instruction that ignores the bit. Default is 0.
	#[getter]
	fn vex_wig(&self) -> u32 {
		self.encoder.vex_wig()
	}

	#[setter]
	fn set_vex_wig(&mut self, new_value: u32) {
		self.encoder.set_vex_wig(new_value)
	}

	/// int: (``u8``) Value of the ``VEX.L`` bit to use if it's an instruction that ignores the bit. Default is 0.
	#[getter]
	fn vex_lig(&self) -> u32 {
		self.encoder.vex_lig()
	}

	#[setter]
	fn set_vex_lig(&mut self, new_value: u32) {
		self.encoder.set_vex_lig(new_value)
	}

	/// int: (``u8``) Value of the ``EVEX.W`` bit to use if it's an instruction that ignores the bit. Default is 0.
	#[getter]
	fn evex_wig(&self) -> u32 {
		self.encoder.evex_wig()
	}

	#[setter]
	fn set_evex_wig(&mut self, new_value: u32) {
		self.encoder.set_evex_wig(new_value)
	}

	/// int: (``u8``) Value of the ``EVEX.L'L`` bits to use if it's an instruction that ignores the bits. Default is 0.
	#[getter]
	fn evex_lig(&self) -> u32 {
		self.encoder.evex_lig()
	}

	#[setter]
	fn set_evex_lig(&mut self, new_value: u32) {
		self.encoder.set_evex_lig(new_value)
	}

	/// int: (``u8``) Value of the ``MVEX.W`` bit to use if it's an instruction that ignores the bit. Default is 0.
	#[getter]
	fn mvex_wig(&self) -> u32 {
		self.encoder.mvex_wig()
	}

	#[setter]
	fn set_mvex_wig(&mut self, new_value: u32) {
		self.encoder.set_mvex_wig(new_value)
	}

	/// int: Gets the bitness (16, 32 or 64)
	#[getter]
	fn bitness(&self) -> u32 {
		self.encoder.bitness()
	}
}
