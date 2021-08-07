// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

//! Easier creating of instructions (eg. `a.mov(eax, ecx)`) than using `Instruction::with*()` functions.
//!
//! TODO:

#![allow(clippy::unused_self, clippy::todo, clippy::unused_self)] //TODO:

mod fn_asm_impl;
mod fn_asm_pub;
mod fn_asm_traits;
mod mem;
mod op_state;
mod reg;
pub mod registers;

pub use crate::code_asm::mem::*;
use crate::code_asm::op_state::CodeAsmOpState;
pub use crate::code_asm::reg::*;
pub use crate::code_asm::registers::*;
pub use crate::IcedError;
use crate::{BlockEncoderOptions, Code, Instruction, MemoryOperand, Register};
use alloc::vec::Vec;
use core::hash::{Hash, Hasher};
use core::usize;

mod private {
	pub trait Sealed {}
}

struct PrefixFlags;
impl PrefixFlags {
	const NONE: u8 = 0x00;
	const XACQUIRE: u8 = 0x01;
	const XRELEASE: u8 = 0x02;
	const LOCK: u8 = 0x04;
	const REPE: u8 = 0x08;
	const REPNE: u8 = 0x10;
	const NOTRACK: u8 = 0x20;
}

struct CodeAssemblerOptions;
impl CodeAssemblerOptions {
	const PREFER_VEX: u8 = 0x01;
	const PREFER_SHORT_BRANCH: u8 = 0x02;
}

/// Creates and encodes instructions. It's easier to use this struct than to call `Instruction::with*()` functions.
#[allow(missing_debug_implementations)]
pub struct CodeAssembler {
	bitness: u32,
	instructions: Vec<Instruction>,
	current_label_id: u64,
	current_label: CodeLabel,
	current_anon_label: CodeLabel,
	next_anon_label: CodeLabel,
	defined_anon_label: bool,
	prefix_flags: u8,
	options: u8,
}

impl CodeAssembler {
	/// Creates a new instance
	///
	/// # Errors
	///
	/// Fails if `bitness` is invalid
	///
	/// # Arguments
	///
	/// * `bitness`: 16, 32, or 64
	#[inline]
	pub fn new(bitness: u32) -> Result<CodeAssembler, IcedError> {
		match bitness {
			16 | 32 | 64 => {}
			_ => return Err(IcedError::new("Invalid bitness")),
		}

		Ok(Self {
			bitness,
			instructions: Vec::new(),
			current_label_id: 0,
			current_label: CodeLabel::default(),
			current_anon_label: CodeLabel::default(),
			next_anon_label: CodeLabel::default(),
			defined_anon_label: false,
			prefix_flags: PrefixFlags::NONE,
			options: CodeAssemblerOptions::PREFER_VEX | CodeAssemblerOptions::PREFER_SHORT_BRANCH,
		})
	}

	/// Gets the bitness (16, 32 or 64)
	#[must_use]
	#[inline]
	pub fn bitness(&self) -> u32 {
		self.bitness
	}

	/// `true` (default value) to use `VEX` encoding intead of `EVEX` encoding if we must pick one of the encodings
	#[must_use]
	#[inline]
	pub fn prefer_vex(&self) -> bool {
		(self.options & CodeAssemblerOptions::PREFER_VEX) != 0
	}

	/// `true` (default value) to use `VEX` encoding intead of `EVEX` encoding if we must pick one of the encodings
	///
	/// # Arguments
	///
	/// * `new_value`: New value
	#[inline]
	pub fn set_prefer_vex(&mut self, new_value: bool) {
		if new_value {
			self.options |= CodeAssemblerOptions::PREFER_VEX;
		} else {
			self.options &= !CodeAssemblerOptions::PREFER_VEX;
		}
	}

	/// `true` (default value) to create short branches, `false` to create near branches.
	#[must_use]
	#[inline]
	pub fn prefer_short_branch(&self) -> bool {
		(self.options & CodeAssemblerOptions::PREFER_SHORT_BRANCH) != 0
	}

	/// `true` (default value) to create short branches, `false` to create near branches.
	///
	/// # Arguments
	///
	/// * `new_value`: New value
	#[inline]
	pub fn set_prefer_short_branch(&mut self, new_value: bool) {
		if new_value {
			self.options |= CodeAssemblerOptions::PREFER_SHORT_BRANCH;
		} else {
			self.options &= !CodeAssemblerOptions::PREFER_SHORT_BRANCH;
		}
	}

	/// Adds an `XACQUIRE` prefix to the next added instruction
	#[must_use]
	#[inline]
	pub fn xacquire(&mut self) -> &mut Self {
		self.prefix_flags |= PrefixFlags::XACQUIRE;
		self
	}

	/// Adds an `XRELEASE` prefix to the next added instruction
	#[must_use]
	#[inline]
	pub fn xrelease(&mut self) -> &mut Self {
		self.prefix_flags |= PrefixFlags::XRELEASE;
		self
	}

	/// Adds a `LOCK` prefix to the next added instruction
	#[must_use]
	#[inline]
	pub fn lock(&mut self) -> &mut Self {
		self.prefix_flags |= PrefixFlags::LOCK;
		self
	}

	/// Adds a `REP` prefix to the next added instruction
	#[must_use]
	#[inline]
	pub fn rep(&mut self) -> &mut Self {
		self.prefix_flags |= PrefixFlags::REPE;
		self
	}

	/// Adds a `REPE` prefix to the next added instruction
	#[must_use]
	#[inline]
	pub fn repe(&mut self) -> &mut Self {
		self.prefix_flags |= PrefixFlags::REPE;
		self
	}

	/// Adds a `REPNE` prefix to the next added instruction
	#[must_use]
	#[inline]
	pub fn repne(&mut self) -> &mut Self {
		self.prefix_flags |= PrefixFlags::REPNE;
		self
	}

	/// Adds a `BND` prefix to the next added instruction
	#[must_use]
	#[inline]
	pub fn bnd(&mut self) -> &mut Self {
		self.prefix_flags |= PrefixFlags::REPNE;
		self
	}

	/// Adds a `NOTRACK` prefix to the next added instruction
	#[must_use]
	#[inline]
	pub fn notrack(&mut self) -> &mut Self {
		self.prefix_flags |= PrefixFlags::NOTRACK;
		self
	}

	/// Gets all added instructions
	#[must_use]
	#[inline]
	pub fn instructions(&self) -> &[Instruction] {
		&self.instructions
	}

	/// Takes ownership of all instructions and returns them. Instruction state is also reset (see [`reset()`])
	///
	/// [`reset()`]: #method.reset
	#[must_use]
	#[inline]
	pub fn take_instructions(&mut self) -> Vec<Instruction> {
		let instrs = core::mem::take(&mut self.instructions);
		self.reset();
		instrs
	}

	/// Resets all instructions and labels
	#[inline]
	pub fn reset(&mut self) {
		self.instructions.clear();
		self.current_label_id = 0;
		self.current_label = CodeLabel::default();
		self.current_anon_label = CodeLabel::default();
		self.next_anon_label = CodeLabel::default();
		self.defined_anon_label = false;
		self.prefix_flags = PrefixFlags::NONE;
	}

	/// Creates a label that can be referenced by instructions
	#[must_use]
	#[inline]
	pub fn create_label(&mut self) -> CodeLabel {
		self.current_label_id += 1;
		CodeLabel::new(self.current_label_id)
	}

	/// Gets the current label
	#[must_use]
	#[inline]
	pub fn current_label(&self) -> CodeLabel {
		self.current_label
	}

	/// Initializes the label to the next instruction
	///
	/// # Errors
	///
	/// Fails if the label wasn't created by [`current_label()`], if this method was called
	/// multiple times for the same label, or if the next instruction already has a label.
	///
	/// [`current_label()`]: #method.current_label
	#[allow(clippy::missing_inline_in_public_items)]
	pub fn set_current_label(&mut self, label: &mut CodeLabel) -> Result<(), IcedError> {
		if label.is_empty() {
			return Err(IcedError::new("Invalid label. Must be created by current_label()"));
		}
		if label.has_instruction_index() {
			return Err(IcedError::new("Labels can't be re-used and can only be set once."));
		}
		if !self.current_label.is_empty() {
			return Err(IcedError::new("Only one label per instruction is allowed"));
		}
		label.instruction_index = self.instructions.len();
		self.current_label = *label;
		Ok(())
	}

	/// Creates an anonymous label that can be referenced by calling [`bwd()`] and [`fwd()`]
	///
	/// # Errors
	///
	/// Fails if the next instruction already has an anonymous label
	///
	/// [`bwd()`]: #method.bwd
	/// [`fwd()`]: #method.fwd
	#[allow(clippy::missing_inline_in_public_items)]
	pub fn anonymous_label(&mut self) -> Result<(), IcedError> {
		if self.defined_anon_label {
			return Err(IcedError::new("At most one anonymous label per instruction is allowed"));
		}
		self.current_anon_label = if self.next_anon_label.is_empty() { self.create_label() } else { self.next_anon_label };
		self.next_anon_label = CodeLabel::default();
		self.defined_anon_label = true;
		Ok(())
	}

	/// Gets the previously created anonymous label created by [`anonymous_label()`]
	///
	/// [`anonymous_label()`]: #method.anonymous_label
	///
	/// # Errors
	///
	/// Fails if no anonymous label has been created yet
	#[inline]
	pub fn bwd(&mut self) -> Result<CodeLabel, IcedError> {
		if self.current_anon_label.is_empty() {
			Err(IcedError::new("No anonymous label has been created yet"))
		} else {
			Ok(self.current_anon_label)
		}
	}

	/// Gets the next anonymous label created by [`anonymous_label()`]
	///
	/// [`anonymous_label()`]: #method.anonymous_label
	#[must_use]
	#[inline]
	pub fn fwd(&mut self) -> CodeLabel {
		if self.next_anon_label.is_empty() {
			self.next_anon_label = self.create_label();
		}
		self.next_anon_label
	}

	/// Adds data
	///
	/// # Arguments
	///
	/// * `data`: The data that will be added at the current position
	#[allow(clippy::missing_inline_in_public_items)]
	pub fn db(&mut self, _data: &[u8]) {
		//TODO:
	}

	/// `CALL FAR` instruction
	///
	/// Instruction | Opcode | CPUID
	/// ------------|--------|------
	/// `CALL ptr16:16` | `o16 9A cd` | `8086+`
	/// `CALL ptr16:32` | `o32 9A cp` | `386+`
	///
	/// # Errors
	///
	/// Fails if an operand is invalid (basic checks only)
	///
	/// # Arguments
	///
	/// * `selector`: Selector/segment
	/// * `offset`: Offset within the segment
	#[inline]
	pub fn call_far(&mut self, selector: u16, offset: u32) -> Result<(), IcedError> {
		let code = if self.bitness() >= 32 { Code::Call_ptr1632 } else { Code::Call_ptr1616 };
		self.add_instr(Instruction::with_far_branch(code, selector, offset)?)
	}

	/// `JMP FAR` instruction
	///
	/// Instruction | Opcode | CPUID
	/// ------------|--------|------
	/// `JMP ptr16:16` | `o16 EA cd` | `8086+`
	/// `JMP ptr16:32` | `o32 EA cp` | `386+`
	///
	/// # Errors
	///
	/// Fails if an operand is invalid (basic checks only)
	///
	/// # Arguments
	///
	/// * `selector`: Selector/segment
	/// * `offset`: Offset within the segment
	#[inline]
	pub fn jmp_far(&mut self, selector: u16, offset: u32) -> Result<(), IcedError> {
		let code = if self.bitness() >= 32 { Code::Jmp_ptr1632 } else { Code::Jmp_ptr1616 };
		self.add_instr(Instruction::with_far_branch(code, selector, offset)?)
	}

	/// `XLATB` instruction
	///
	/// Instruction | Opcode | CPUID
	/// ------------|--------|------
	/// `XLATB` | `D7` | `8086+`
	///
	/// # Errors
	///
	/// Fails if an operand is invalid (basic checks only)
	#[inline]
	pub fn xlatb(&mut self) -> Result<(), IcedError> {
		let base = match self.bitness() {
			64 => Register::RBX,
			32 => Register::EBX,
			16 => Register::BX,
			_ => unreachable!(),
		};
		self.add_instr(Instruction::with1(Code::Xlat_m8, MemoryOperand::with_base_index(base, Register::AL))?)
	}

	/// Adds nops, preferring long nops over short nops
	///
	/// # Errors
	///
	/// Fails if an error was detected
	///
	/// # Arguments
	///
	/// * `size`: Size in bytes of all nops
	#[allow(clippy::missing_inline_in_public_items)]
	pub fn nops_with_size(_size: usize) -> Result<(), IcedError> {
		//TODO:
		todo!()
	}

	pub(crate) fn add_instr_with_state(&mut self, mut instruction: Instruction, state: CodeAsmOpState) -> Result<(), IcedError> {
		if !state.is_default() {
			if state.is_broadcast() {
				instruction.set_is_broadcast(true);
			}
			if state.zeroing_masking() {
				instruction.set_zeroing_masking(true);
			}
			if state.suppress_all_exceptions() {
				instruction.set_suppress_all_exceptions(true);
			}
			instruction.set_op_mask(state.op_mask());
			instruction.set_rounding_control(state.rounding_control());
		}
		self.add_instr(instruction)
	}

	pub(crate) fn add_instr(&mut self, mut instruction: Instruction) -> Result<(), IcedError> {
		if self.prefix_flags != 0 {
			if (self.prefix_flags & PrefixFlags::XACQUIRE) != 0 {
				instruction.set_has_xacquire_prefix(true);
			}
			if (self.prefix_flags & PrefixFlags::XRELEASE) != 0 {
				instruction.set_has_xrelease_prefix(true);
			}
			if (self.prefix_flags & PrefixFlags::LOCK) != 0 {
				instruction.set_has_lock_prefix(true);
			}
			if (self.prefix_flags & PrefixFlags::REPE) != 0 {
				instruction.set_has_repe_prefix(true);
			} else if (self.prefix_flags & PrefixFlags::REPNE) != 0 {
				instruction.set_has_repne_prefix(true);
			}
			if (self.prefix_flags & PrefixFlags::NOTRACK) != 0 {
				instruction.set_segment_prefix(Register::DS);
			}
		}

		self.instructions.push(instruction);
		self.prefix_flags = PrefixFlags::NONE;
		Ok(())
	}

	/// Encodes all added instructions and returns the result
	///
	/// # Errors
	///
	/// Fails if an error was detected
	///
	/// # Arguments
	///
	/// * `ip`: Base address of all instructions
	#[inline]
	pub fn assemble(&mut self, ip: u64) -> Result<(), IcedError> {
		self.assemble_options(ip, BlockEncoderOptions::NONE)
	}

	/// Encodes all added instructions and returns the result
	///
	/// # Errors
	///
	/// Fails if an error was detected
	///
	/// # Arguments
	///
	/// * `ip`: Base address of all instructions
	/// * `options`: Encoder options
	#[allow(clippy::missing_inline_in_public_items)]
	pub fn assemble_options(&mut self, _ip: u64, _options: u32) -> Result<(), IcedError> {
		//TODO:
		todo!()
	}
}

/// A label created by [`CodeAssembler`]
///
/// [`CodeAssembler`]: struct.CodeAssembler.html
#[derive(Debug, Default, Copy, Clone)]
pub struct CodeLabel {
	id: u64,
	instruction_index: usize,
}

impl Eq for CodeLabel {}

impl PartialEq for CodeLabel {
	#[inline]
	fn eq(&self, other: &CodeLabel) -> bool {
		self.id == other.id
	}
}

impl Hash for CodeLabel {
	#[inline]
	fn hash<H: Hasher>(&self, state: &mut H) {
		state.write_u64(self.id);
	}
}

impl CodeLabel {
	#[must_use]
	#[inline]
	pub(crate) fn new(id: u64) -> Self {
		Self { id, instruction_index: usize::MAX }
	}

	#[must_use]
	#[inline]
	pub(crate) fn is_empty(&self) -> bool {
		self.id == 0
	}

	#[must_use]
	#[inline]
	pub(crate) fn has_instruction_index(&self) -> bool {
		self.instruction_index != usize::MAX
	}

	#[must_use]
	#[inline]
	pub(crate) fn id(&self) -> u64 {
		self.id
	}
}
