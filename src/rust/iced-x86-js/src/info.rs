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

use super::encoding_kind::{iced_to_encoding_kind, EncodingKind};
use super::flow_control::{iced_to_flow_control, FlowControl};
use super::instruction::Instruction;
use super::memory_size::{iced_to_memory_size, MemorySize};
use super::op_access::{iced_to_op_access, OpAccess};
use super::register::{iced_to_register, Register};
use wasm_bindgen::prelude::*;

/// A register used by an instruction
#[wasm_bindgen]
pub struct UsedRegister(iced_x86::UsedRegister);

#[wasm_bindgen]
impl UsedRegister {
	/// Gets the register
	#[wasm_bindgen(getter)]
	pub fn register(&self) -> Register {
		iced_to_register(self.0.register())
	}

	/// Gets the register access
	#[wasm_bindgen(getter)]
	pub fn access(&self) -> OpAccess {
		iced_to_op_access(self.0.access())
	}
}

/// A memory location used by an instruction
#[wasm_bindgen]
pub struct UsedMemory(iced_x86::UsedMemory);

#[wasm_bindgen]
impl UsedMemory {
	/// Effective segment register
	#[wasm_bindgen(getter)]
	pub fn segment(&self) -> Register {
		iced_to_register(self.0.segment())
	}

	/// Base register or [`Register.None`] if none
	///
	/// [`Register.None`]: enum.Register.html#variant.None
	#[wasm_bindgen(getter)]
	pub fn base(&self) -> Register {
		iced_to_register(self.0.base())
	}

	/// Index register or [`Register.None`] if none
	///
	/// [`Register.None`]: enum.Register.html#variant.None
	#[wasm_bindgen(getter)]
	pub fn index(&self) -> Register {
		iced_to_register(self.0.index())
	}

	/// Index scale (1, 2, 4 or 8)
	#[wasm_bindgen(getter)]
	pub fn scale(&self) -> u32 {
		self.0.scale()
	}

	/// Displacement
	#[wasm_bindgen(getter)]
	pub fn displacement(&self) -> u64 {
		self.0.displacement()
	}

	/// Size of location
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "memorySize")]
	pub fn memory_size(&self) -> MemorySize {
		iced_to_memory_size(self.0.memory_size())
	}

	/// Memory access
	#[wasm_bindgen(getter)]
	pub fn access(&self) -> OpAccess {
		iced_to_op_access(self.0.access())
	}
}

/// Contains information about an instruction, eg. read/written registers, read/written `RFLAGS` bits, `CPUID` feature bit, etc.
/// Created by an [`InstructionInfoFactory`].
///
/// [`InstructionInfoFactory`]: struct.InstructionInfoFactory.html
#[wasm_bindgen]
pub struct InstructionInfo(iced_x86::InstructionInfo);

#[wasm_bindgen]
impl InstructionInfo {
	/// Gets all accessed registers. This method doesn't return all accessed registers if [`isSaveRestoreInstruction`] is `true`.
	///
	/// [`isSaveRestoreInstruction`]: #method.is_save_restore_instruction
	#[wasm_bindgen(js_name = "usedRegisters")]
	pub fn used_registers(&self) -> js_sys::Array {
		//TODO: https://github.com/rustwasm/wasm-bindgen/issues/111
		self.0.used_registers().iter().map(|&r| JsValue::from(UsedRegister(r))).collect()
	}

	/// Gets all accessed memory locations
	#[wasm_bindgen(js_name = "usedMemory")]
	pub fn used_memory(&self) -> js_sys::Array {
		//TODO: https://github.com/rustwasm/wasm-bindgen/issues/111
		self.0.used_memory().iter().map(|&m| JsValue::from(UsedMemory(m))).collect()
	}

	/// `true` if the instruction isn't available in real mode or virtual 8086 mode
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "isProtectedMode")]
	pub fn is_protected_mode(&self) -> bool {
		self.0.is_protected_mode()
	}

	/// `true` if this is a privileged instruction
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "isPrivileged")]
	pub fn is_privileged(&self) -> bool {
		self.0.is_privileged()
	}

	/// `true` if this is an instruction that implicitly uses the stack pointer (`SP`/`ESP`/`RSP`), eg. `CALL`, `PUSH`, `POP`, `RET`, etc.
	/// See also [`Instruction.stackPointerIncrement`]
	///
	/// [`Instruction.stackPointerIncrement`]: struct.Instruction.html#method.stack_pointer_increment
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "isStackInstruction")]
	pub fn is_stack_instruction(&self) -> bool {
		self.0.is_stack_instruction()
	}

	/// `true` if it's an instruction that saves or restores too many registers (eg. `FXRSTOR`, `XSAVE`, etc).
	/// [`usedRegisters()`] won't return all accessed registers.
	///
	/// [`usedRegisters()`]: #method.used_registers
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "isSaveRestoreInstruction")]
	pub fn is_save_restore_instruction(&self) -> bool {
		self.0.is_save_restore_instruction()
	}

	/// Instruction encoding, eg. legacy, VEX, EVEX, ...
	#[wasm_bindgen(getter)]
	pub fn encoding(&self) -> EncodingKind {
		iced_to_encoding_kind(self.0.encoding())
	}

	/// Gets the CPU or CPUID feature flags (an array of `CpuidFeature`)
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "cpuidFeatures")]
	pub fn cpuid_features(&self) -> Vec<i32> {
		// It's not possible to return a Vec<CpuidFeature>
		self.0.cpuid_features().iter().map(|&a| a as i32).collect()
	}

	/// Flow control info
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "flowControl")]
	pub fn flow_control(&self) -> FlowControl {
		iced_to_flow_control(self.0.flow_control())
	}

	/// Operand #0 access
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "op0Access")]
	pub fn op0_access(&self) -> OpAccess {
		iced_to_op_access(self.0.op0_access())
	}

	/// Operand #1 access
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "op1Access")]
	pub fn op1_access(&self) -> OpAccess {
		iced_to_op_access(self.0.op1_access())
	}

	/// Operand #2 access
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "op2Access")]
	pub fn op2_access(&self) -> OpAccess {
		iced_to_op_access(self.0.op2_access())
	}

	/// Operand #3 access
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "op3Access")]
	pub fn op3_access(&self) -> OpAccess {
		iced_to_op_access(self.0.op3_access())
	}

	/// Operand #4 access
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "op4Access")]
	pub fn op4_access(&self) -> OpAccess {
		iced_to_op_access(self.0.op4_access())
	}

	/// Gets operand access
	///
	/// # Panics
	///
	/// Panics if `operand` is invalid
	///
	/// # Arguments
	///
	/// * `operand`: Operand number, 0-4
	#[wasm_bindgen(js_name = "opAccess")]
	pub fn op_access(&self, operand: u32) -> OpAccess {
		iced_to_op_access(self.0.op_access(operand))
	}

	/// All flags that are read by the CPU when executing the instruction.
	/// This method returns a [`RflagsBits`] value. See also [`rflagsModified`].
	///
	/// [`RflagsBits`]: enum.RflagsBits.html
	/// [`rflagsModified`]: #method.rflags_modified
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "rflagsRead")]
	pub fn rflags_read(&self) -> u32 {
		self.0.rflags_read()
	}

	/// All flags that are written by the CPU, except those flags that are known to be undefined, always set or always cleared.
	/// This method returns a [`RflagsBits`] value. See also [`rflagsModified`].
	///
	/// [`RflagsBits`]: enum.RflagsBits.html
	/// [`rflagsModified`]: #method.rflags_modified
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "rflagsWritten")]
	pub fn rflags_written(&self) -> u32 {
		self.0.rflags_written()
	}

	/// All flags that are always cleared by the CPU.
	/// This method returns a [`RflagsBits`] value. See also [`rflagsModified`].
	///
	/// [`RflagsBits`]: enum.RflagsBits.html
	/// [`rflagsModified`]: #method.rflags_modified
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "rflagsCleared")]
	pub fn rflags_cleared(&self) -> u32 {
		self.0.rflags_cleared()
	}

	/// All flags that are always set by the CPU.
	/// This method returns a [`RflagsBits`] value. See also [`rflagsModified`].
	///
	/// [`RflagsBits`]: enum.RflagsBits.html
	/// [`rflagsModified`]: #method.rflags_modified
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "rflagsSet")]
	pub fn rflags_set(&self) -> u32 {
		self.0.rflags_set()
	}

	/// All flags that are undefined after executing the instruction.
	/// This method returns a [`RflagsBits`] value. See also [`rflagsModified`].
	///
	/// [`RflagsBits`]: enum.RflagsBits.html
	/// [`rflagsModified`]: #method.rflags_modified
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "rflagsUndefined")]
	pub fn rflags_undefined(&self) -> u32 {
		self.0.rflags_undefined()
	}

	/// All flags that are modified by the CPU. This is `rflagsWritten() + rflagsCleared() + rflagsSet() + rflagsUndefined()`. This method returns a [`RflagsBits`] value.
	///
	/// [`RflagsBits`]: enum.RflagsBits.html
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "rflagsModified")]
	pub fn rflags_modified(&self) -> u32 {
		self.0.rflags_modified()
	}
}

/// Instruction info options used by [`InstructionInfoFactory`]
///
/// [`InstructionInfoFactory`]: struct.InstructionInfoFactory.html
#[wasm_bindgen]
pub enum InstructionInfoOptions {
	/// No option is enabled
	None = 0,
	/// Don't include memory usage, i.e., [`InstructionInfo.usedMemory()`] will return an empty vector. All
	/// registers that are used by memory operands are still returned by [`InstructionInfo.usedRegisters()`].
	///
	/// [`InstructionInfo.usedMemory()`]: struct.InstructionInfo.html#method.used_memory
	/// [`InstructionInfo.usedRegisters()`]: struct.InstructionInfo.html#method.used_registers
	NoMemoryUsage = 0x0000_0001,
	/// Don't include register usage, i.e., [`InstructionInfo.usedRegisters()`] will return an empty vector
	///
	/// [`InstructionInfo.usedRegisters()`]: struct.InstructionInfo.html#method.used_registers
	NoRegisterUsage = 0x0000_0002,
}

/// Creates [`InstructionInfo`]s.
///
/// If you don't need to know register and memory usage, it's faster to call [`Instruction`] and
/// [`Code`] methods such as [`Instruction.flowControl`] instead of getting that info from this struct.
///
/// [`InstructionInfo`]: struct.InstructionInfo.html
/// [`Instruction`]: struct.Instruction.html
/// [`Code`]: enum.Code.html
/// [`Instruction.flowControl`]: struct.Instruction.html#method.flow_control
#[wasm_bindgen]
pub struct InstructionInfoFactory(iced_x86::InstructionInfoFactory);

#[wasm_bindgen]
impl InstructionInfoFactory {
	/// Creates a new instance.
	///
	/// If you don't need to know register and memory usage, it's faster to call [`Instruction`] and
	/// [`Code`] methods such as [`Instruction.flowControl`] instead of getting that info from this struct.
	///
	/// [`Instruction`]: struct.Instruction.html
	/// [`Code`]: enum.Code.html
	/// [`Instruction.flowControl`]: struct.Instruction.html#method.flow_control
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	///
	/// // add [rdi+r12*8-5AA5EDCCh],esi
	/// let bytes = b"\x42\x01\xB4\xE7\x34\x12\x5A\xA5";
	/// let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
	///
	/// // This allocates two vectors but they get re-used every time you call info() and infoOptions().
	/// let mut info_factory = InstructionInfoFactory::new();
	///
	/// for instr in &mut decoder {
	///     // There's also info_options() if you only need reg usage or only mem usage.
	///     // info() returns both.
	///     let info = info_factory.info(&instr);
	///     for mem_info in info.used_memory().iter() {
	///         println!("{:?}", mem_info);
	///     }
	///     for reg_info in info.used_registers().iter() {
	///         println!("{:?}", reg_info);
	///     }
	/// }
	/// ```
	#[wasm_bindgen(constructor)]
	#[allow(clippy::new_without_default)]
	pub fn new() -> Self {
		InstructionInfoFactory(iced_x86::InstructionInfoFactory::new())
	}

	/// Creates a new [`InstructionInfo`], see also [`infoOptions()`] if you only need register usage
	/// but not memory usage or vice versa.
	///
	/// If you don't need to know register and memory usage, it's faster to call [`Instruction`] and
	/// [`Code`] methods such as [`Instruction.flowControl`] instead of getting that info from this struct.
	///
	/// [`InstructionInfo`]: struct.InstructionInfo.html
	/// [`infoOptions()`]: #method.info_options
	/// [`Instruction`]: struct.Instruction.html
	/// [`Code`]: enum.Code.html
	/// [`Instruction.flowControl`]: struct.Instruction.html#method.flow_control
	///
	/// # Arguments
	///
	/// * `instruction`: The instruction that should be analyzed
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	///
	/// // add [rdi+r12*8-5AA5EDCCh],esi
	/// let bytes = b"\x42\x01\xB4\xE7\x34\x12\x5A\xA5";
	/// let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
	/// let mut info_factory = InstructionInfoFactory::new();
	///
	/// let instr = decoder.decode();
	/// let info = info_factory.info(&instr);
	///
	/// assert_eq!(1, info.used_memory().len());
	/// let mem = info.used_memory()[0];
	/// assert_eq!(Register::DS, mem.segment());
	/// assert_eq!(Register::RDI, mem.base());
	/// assert_eq!(Register::R12, mem.index());
	/// assert_eq!(8, mem.scale());
	/// assert_eq!(0xFFFFFFFFA55A1234, mem.displacement());
	/// assert_eq!(MemorySize::UInt32, mem.memory_size());
	/// assert_eq!(OpAccess::ReadWrite, mem.access());
	///
	/// let regs = info.used_registers();
	/// assert_eq!(3, regs.len());
	/// assert_eq!(Register::RDI, regs[0].register());
	/// assert_eq!(OpAccess::Read, regs[0].access());
	/// assert_eq!(Register::R12, regs[1].register());
	/// assert_eq!(OpAccess::Read, regs[1].access());
	/// assert_eq!(Register::ESI, regs[2].register());
	/// assert_eq!(OpAccess::Read, regs[2].access());
	/// ```
	pub fn info(&mut self, instruction: &Instruction) -> InstructionInfo {
		InstructionInfo((*self.0.info(&instruction.0)).clone())
	}

	/// Creates a new [`InstructionInfo`], see also [`info()`].
	///
	/// If you don't need to know register and memory usage, it's faster to call [`Instruction`] and
	/// [`Code`] methods such as [`Instruction.flowControl`] instead of getting that info from this struct.
	///
	/// [`InstructionInfo`]: struct.InstructionInfo.html
	/// [`info()`]: #method.info
	/// [`Instruction`]: struct.Instruction.html
	/// [`Code`]: enum.Code.html
	/// [`Instruction.flowControl`]: struct.Instruction.html#method.flow_control
	///
	/// # Arguments
	///
	/// * `instruction`: The instruction that should be analyzed
	/// * `options`: Options, see [`InstructionInfoOptions`]
	///
	/// [`InstructionInfoOptions`]: struct.InstructionInfoOptions.html
	#[wasm_bindgen(js_name = "infoOptions")]
	pub fn info_options(&mut self, instruction: &Instruction, options: InstructionInfoOptions) -> InstructionInfo {
		const_assert_eq!(iced_x86::InstructionInfoOptions::NONE, InstructionInfoOptions::None as u32);
		const_assert_eq!(iced_x86::InstructionInfoOptions::NO_MEMORY_USAGE, InstructionInfoOptions::NoMemoryUsage as u32);
		const_assert_eq!(iced_x86::InstructionInfoOptions::NO_REGISTER_USAGE, InstructionInfoOptions::NoRegisterUsage as u32);
		InstructionInfo((*self.0.info_options(&instruction.0, options as u32)).clone())
	}
}
