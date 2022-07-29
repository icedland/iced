// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::ex_utils::to_js_error;
use crate::instruction::Instruction;
use crate::memory_size::{iced_to_memory_size, MemorySize};
use crate::op_access::{iced_to_op_access, OpAccess};
use crate::register::{iced_to_register, Register};
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
	/// Effective segment register (a [`Register`] enum value) or [`Register.None`] if the segment register is ignored
	///
	/// [`Register`]: struct.Register.html
	/// [`Register.None`]: enum.Register.html#variant.None
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

	/// Displacement
	#[wasm_bindgen(getter)]
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

/// Contains accessed registers and memory locations
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
	pub fn op_access(&self, operand: u32) -> Result<OpAccess, JsValue> {
		Ok(iced_to_op_access(self.0.try_op_access(operand).map_err(to_js_error)?))
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
/// [`InstructionInfo`]: struct.InstructionInfo.html
#[wasm_bindgen]
pub struct InstructionInfoFactory(iced_x86_rust::InstructionInfoFactory);

#[wasm_bindgen]
impl InstructionInfoFactory {
	/// Creates a new instance.
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
	/// [`InstructionInfo`]: struct.InstructionInfo.html
	/// [`infoOptions()`]: #method.info_options
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
	/// assert.equal(mem.displacement, 0xFFFFFFFFA55A1234n);
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
	/// [`InstructionInfo`]: struct.InstructionInfo.html
	/// [`info()`]: #method.info
	///
	/// # Arguments
	///
	/// * `instruction`: The instruction that should be analyzed
	/// * `options`: Options, see [`InstructionInfoOptions`]
	///
	/// [`InstructionInfoOptions`]: struct.InstructionInfoOptions.html
	#[wasm_bindgen(js_name = "infoOptions")]
	pub fn info_options(&mut self, instruction: &Instruction, options: u32 /*flags: InstructionInfoOptions*/) -> InstructionInfo {
		const _: () = assert!(InstructionInfoOptions::None as u32 == iced_x86_rust::InstructionInfoOptions::NONE);
		const _: () = assert!(InstructionInfoOptions::NoMemoryUsage as u32 == iced_x86_rust::InstructionInfoOptions::NO_MEMORY_USAGE);
		const _: () = assert!(InstructionInfoOptions::NoRegisterUsage as u32 == iced_x86_rust::InstructionInfoOptions::NO_REGISTER_USAGE);
		InstructionInfo((*self.0.info_options(&instruction.0, options)).clone())
	}
}
