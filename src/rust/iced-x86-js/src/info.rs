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
pub struct UsedRegister(iced_x86_rust::UsedRegister);

#[wasm_bindgen]
impl UsedRegister {
	/// Gets the register (a [`Register`] enum value)
	///
	/// [`Register`]: struct.Register.html
	#[wasm_bindgen(getter)]
	pub fn register(&self) -> Register {
		iced_to_register(self.0.register())
	}

	/// Gets the register access (an [`OpAccess`] enum value)
	///
	/// [`OpAccess`]: enum.OpAccess.html
	#[wasm_bindgen(getter)]
	pub fn access(&self) -> OpAccess {
		iced_to_op_access(self.0.access())
	}
}

/// A memory location used by an instruction
#[wasm_bindgen]
pub struct UsedMemory(iced_x86_rust::UsedMemory);

#[wasm_bindgen]
impl UsedMemory {
	/// Effective segment register (a [`Register`] enum value)
	///
	/// [`Register`]: struct.Register.html
	#[wasm_bindgen(getter)]
	pub fn segment(&self) -> Register {
		iced_to_register(self.0.segment())
	}

	/// Base register (a [`Register`] enum value) or [`Register.None`] if none
	///
	/// [`Register`]: struct.Register.html
	/// [`Register.None`]: enum.Register.html#variant.None
	#[wasm_bindgen(getter)]
	pub fn base(&self) -> Register {
		iced_to_register(self.0.base())
	}

	/// Index register (a [`Register`] enum value) or [`Register.None`] if none
	///
	/// [`Register`]: struct.Register.html
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

	/// Displacement (low 32 bits).
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "displacementLo")]
	#[cfg(not(feature = "bigint"))]
	pub fn displacement_lo(&self) -> u32 {
		self.0.displacement() as u32
	}

	/// Displacement (high 32 bits).
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "displacementHi")]
	#[cfg(not(feature = "bigint"))]
	pub fn displacement_hi(&self) -> u32 {
		(self.0.displacement() >> 32) as u32
	}

	/// Displacement
	#[wasm_bindgen(getter)]
	#[cfg(feature = "bigint")]
	pub fn displacement(&self) -> u64 {
		self.0.displacement()
	}

	/// Size of location (a [`MemorySize`] enum value)
	///
	/// [`MemorySize`]: enum.MemorySize.html
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "memorySize")]
	pub fn memory_size(&self) -> MemorySize {
		iced_to_memory_size(self.0.memory_size())
	}

	/// Memory access (an [`OpAccess`] enum value)
	///
	/// [`OpAccess`]: enum.OpAccess.html
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
pub struct InstructionInfo(iced_x86_rust::InstructionInfo);

#[wasm_bindgen]
impl InstructionInfo {
	/// Gets all accessed registers (an array of [`UsedRegister`] classes).
	/// This method doesn't return all accessed registers if [`isSaveRestoreInstruction`] is `true`.
	///
	/// Some instructions have a `r16`/`r32` operand but only use the low 8 bits of the register. In that case
	/// this method returns the 8-bit register even if it's `SPL`, `BPL`, `SIL`, `DIL` and the
	/// instruction was decoded in 16 or 32-bit mode. This is more accurate than returning the `r16`/`r32`
	/// register. Example instructions that do this: `PINSRB`, `ARPL`
	///
	/// [`UsedRegister`]: struct.UsedRegister.html
	/// [`isSaveRestoreInstruction`]: #method.is_save_restore_instruction
	#[wasm_bindgen(js_name = "usedRegisters")]
	pub fn used_registers(&self) -> js_sys::Array {
		//TODO: https://github.com/rustwasm/wasm-bindgen/issues/111
		self.0.used_registers().iter().map(|&r| JsValue::from(UsedRegister(r))).collect()
	}

	/// Gets all accessed memory locations (an array of [`UsedMemory`] classes).
	///
	/// [`UsedMemory`]: struct.UsedMemory.html
	#[wasm_bindgen(js_name = "usedMemory")]
	pub fn used_memory(&self) -> js_sys::Array {
		//TODO: https://github.com/rustwasm/wasm-bindgen/issues/111
		self.0.used_memory().iter().map(|&m| JsValue::from(UsedMemory(m))).collect()
	}

	/// `true` if it's a privileged instruction (all CPL=0 instructions (except `VMCALL`) and IOPL instructions `IN`, `INS`, `OUT`, `OUTS`, `CLI`, `STI`)
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

	/// Instruction encoding (an [`EncodingKind`] enum value), eg. Legacy, 3DNow!, VEX, EVEX, XOP
	///
	/// [`EncodingKind`]: enum.EncodingKind.html
	#[wasm_bindgen(getter)]
	pub fn encoding(&self) -> EncodingKind {
		iced_to_encoding_kind(self.0.encoding())
	}

	/// Gets the CPU or CPUID feature flags (an array of [`CpuidFeature`] values)
	///
	/// [`CpuidFeature`]: enum.CpuidFeature.html
	#[wasm_bindgen(js_name = "cpuidFeatures")]
	pub fn cpuid_features(&self) -> Vec<i32> {
		// It's not possible to return a Vec<CpuidFeature>
		self.0.cpuid_features().iter().map(|&a| a as i32).collect()
	}

	/// Control flow info (a [`FlowControl`] enum value)
	///
	/// [`FlowControl`]: enum.FlowControl.html
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "flowControl")]
	pub fn flow_control(&self) -> FlowControl {
		iced_to_flow_control(self.0.flow_control())
	}

	/// Operand #0 access (an [`OpAccess`] enum value)
	///
	/// [`OpAccess`]: enum.OpAccess.html
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "op0Access")]
	pub fn op0_access(&self) -> OpAccess {
		iced_to_op_access(self.0.op0_access())
	}

	/// Operand #1 access (an [`OpAccess`] enum value)
	///
	/// [`OpAccess`]: enum.OpAccess.html
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "op1Access")]
	pub fn op1_access(&self) -> OpAccess {
		iced_to_op_access(self.0.op1_access())
	}

	/// Operand #2 access (an [`OpAccess`] enum value)
	///
	/// [`OpAccess`]: enum.OpAccess.html
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "op2Access")]
	pub fn op2_access(&self) -> OpAccess {
		iced_to_op_access(self.0.op2_access())
	}

	/// Operand #3 access (an [`OpAccess`] enum value)
	///
	/// [`OpAccess`]: enum.OpAccess.html
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "op3Access")]
	pub fn op3_access(&self) -> OpAccess {
		iced_to_op_access(self.0.op3_access())
	}

	/// Operand #4 access (an [`OpAccess`] enum value)
	///
	/// [`OpAccess`]: enum.OpAccess.html
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "op4Access")]
	pub fn op4_access(&self) -> OpAccess {
		iced_to_op_access(self.0.op4_access())
	}

	/// Gets operand access (an [`OpAccess`] enum value)
	///
	/// [`OpAccess`]: enum.OpAccess.html
	///
	/// # Throws
	///
	/// Throws if `operand` is invalid
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

	/// All flags that are modified by the CPU. This is `rflagsWritten + rflagsCleared + rflagsSet + rflagsUndefined`.
	/// This method returns a [`RflagsBits`] value.
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
pub struct InstructionInfoFactory(iced_x86_rust::InstructionInfoFactory);

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
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Decoder, DecoderOptions, Instruction, InstructionInfoFactory } = require("iced-x86");
	///
	/// // add [rdi+r12*8-5AA5EDCCh],esi
	/// const bytes = new Uint8Array([0x42, 0x01, 0xB4, 0xE7, 0x34, 0x12, 0x5A, 0xA5]);
	/// const decoder = new Decoder(64, bytes, DecoderOptions.None);
	///
	/// const infoFactory = new InstructionInfoFactory();
	/// const instr = new Instruction();
	/// while (decoder.canDecode) {
	///     // Decode the next instruction, overwriting `instr`
	///     decoder.decodeOut(instr);
	///
	///     // There's also infoOptions() if you only need reg usage or only mem usage.
	///     // info() returns both.
	///     const info = infoFactory.info(instr);
	///     for (const memInfo of info.usedMemory()) {
	///         // Do something here with the `UsedMemory` instance...
	///         // ...
	///
	///         // Free wasm memory
	///         memInfo.free();
	///     }
	///     for (const regInfo of info.usedRegisters()) {
	///         // Do something here with the `UsedRegister` instance...
	///         // ...
	///
	///         // Free wasm memory
	///         regInfo.free();
	///     }
	/// }
	///
	/// // Free wasm memory
	/// decoder.free();
	/// instr.free();
	/// infoFactory.free();
	/// ```
	#[wasm_bindgen(constructor)]
	#[allow(clippy::new_without_default)]
	pub fn new() -> Self {
		InstructionInfoFactory(iced_x86_rust::InstructionInfoFactory::new())
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
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Decoder, DecoderOptions, InstructionInfoFactory, MemorySize, OpAccess, Register } = require("iced-x86");
	///
	/// // add [rdi+r12*8-5AA5EDCCh],esi
	/// const bytes = new Uint8Array([0x42, 0x01, 0xB4, 0xE7, 0x34, 0x12, 0x5A, 0xA5]);
	/// const decoder = new Decoder(64, bytes, DecoderOptions.None);
	/// const infoFactory = new InstructionInfoFactory();
	///
	/// const instr = decoder.decode();
	/// const info = infoFactory.info(instr);
	///
	/// const usedMem = info.usedMemory();
	/// assert.equal(usedMem.length, 1);
	/// const mem = usedMem[0];
	/// assert.equal(mem.segment, Register.DS);
	/// assert.equal(mem.base, Register.RDI);
	/// assert.equal(mem.index, Register.R12);
	/// assert.equal(mem.scale, 8);
	/// assert.equal(mem.displacementLo, 0xA55A1234);
	/// assert.equal(mem.displacementHi, 0xFFFFFFFF);
	/// assert.equal(mem.memorySize, MemorySize.UInt32);
	/// assert.equal(mem.access, OpAccess.ReadWrite);
	///
	/// const usedRegs = info.usedRegisters();
	/// assert.equal(usedRegs.length, 3);
	/// assert.equal(usedRegs[0].register, Register.RDI);
	/// assert.equal(usedRegs[0].access, OpAccess.Read);
	/// assert.equal(usedRegs[1].register, Register.R12);
	/// assert.equal(usedRegs[1].access, OpAccess.Read);
	/// assert.equal(usedRegs[2].register, Register.ESI);
	/// assert.equal(usedRegs[2].access, OpAccess.Read);
	///
	/// // Free wasm memory
	/// decoder.free();
	/// instr.free();
	/// infoFactory.free();
	/// usedMem.forEach(m => m.free());
	/// usedRegs.forEach(r => r.free());
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
	pub fn info_options(&mut self, instruction: &Instruction, options: u32 /*flags: InstructionInfoOptions*/) -> InstructionInfo {
		const_assert_eq!(iced_x86_rust::InstructionInfoOptions::NONE, InstructionInfoOptions::None as u32);
		const_assert_eq!(iced_x86_rust::InstructionInfoOptions::NO_MEMORY_USAGE, InstructionInfoOptions::NoMemoryUsage as u32);
		const_assert_eq!(iced_x86_rust::InstructionInfoOptions::NO_REGISTER_USAGE, InstructionInfoOptions::NoRegisterUsage as u32);
		InstructionInfo((*self.0.info_options(&instruction.0, options)).clone())
	}
}
