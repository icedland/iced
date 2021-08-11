// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

// All methods are in this file instead of code_asm.rs to prevent `cargo doc` from
// putting all generated methods (thousands of methods) ahead of non-generated fns
// such as the constructor, prefix fns, etc.

#![allow(clippy::unused_self, clippy::todo, clippy::unused_self)] //TODO:

use crate::code_asm::op_state::CodeAsmOpState;
use crate::code_asm::{CodeAssembler, CodeAssemblerOptions, CodeLabel, PrefixFlags};
use crate::IcedError;
use crate::{BlockEncoder, BlockEncoderOptions, Code, Instruction, InstructionBlock, MemoryOperand, Register};
use alloc::vec::Vec;
use core::usize;

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
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::code_asm::*;
	///
	/// # fn main() -> Result<(), IcedError> {
	/// let mut a = CodeAssembler::new(64)?;
	///
	/// a.rep().stosq()?;
	/// a.nop()?;
	///
	/// let bytes = a.assemble(0x1234_5678)?;
	/// assert_eq!(bytes, vec![0xF3, 0x48, 0xAB, 0x90]);
	/// # Ok(())
	/// # }
	/// ```
	#[must_use]
	#[inline]
	pub fn rep(&mut self) -> &mut Self {
		self.prefix_flags |= PrefixFlags::REPE;
		self
	}

	/// Adds a `REPE` prefix to the next added instruction
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::code_asm::*;
	///
	/// # fn main() -> Result<(), IcedError> {
	/// let mut a = CodeAssembler::new(64)?;
	///
	/// a.repe().cmpsb()?;
	/// a.nop()?;
	///
	/// let bytes = a.assemble(0x1234_5678)?;
	/// assert_eq!(bytes, vec![0xF3, 0xA6, 0x90]);
	/// # Ok(())
	/// # }
	/// ```
	#[must_use]
	#[inline]
	pub fn repe(&mut self) -> &mut Self {
		self.prefix_flags |= PrefixFlags::REPE;
		self
	}

	/// Adds a `REPNE` prefix to the next added instruction
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::code_asm::*;
	///
	/// # fn main() -> Result<(), IcedError> {
	/// let mut a = CodeAssembler::new(64)?;
	///
	/// a.repne().scasb()?;
	/// a.nop()?;
	///
	/// let bytes = a.assemble(0x1234_5678)?;
	/// assert_eq!(bytes, vec![0xF2, 0xAE, 0x90]);
	/// # Ok(())
	/// # }
	/// ```
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

	/// Gets all added instructions, see also [`take_instructions()`] and [`assemble()`]
	///
	/// [`take_instructions()`]: #method.take_instructions
	/// [`assemble()`]: #method.assemble
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	/// use iced_x86::code_asm::*;
	///
	/// # fn main() -> Result<(), IcedError> {
	/// let mut a = CodeAssembler::new(64)?;
	/// a.push(rcx)?;
	/// a.xor(rcx, rdx)?;
	/// assert_eq!(a.instructions(), vec![
	///     Instruction::with1(Code::Push_r64, Register::RCX)?,
	///     Instruction::with2(Code::Xor_rm64_r64, Register::RCX, Register::RDX)?,
	/// ]);
	/// # Ok(())
	/// # }
	/// ```
	#[must_use]
	#[inline]
	pub fn instructions(&self) -> &[Instruction] {
		&self.instructions
	}

	/// Takes ownership of all instructions and returns them. Instruction state is also reset (see [`reset()`])
	///
	/// [`reset()`]: #method.reset
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	/// use iced_x86::code_asm::*;
	///
	/// # fn main() -> Result<(), IcedError> {
	/// let mut a = CodeAssembler::new(64)?;
	/// a.push(rcx)?;
	/// a.xor(rcx, rdx)?;
	/// assert_eq!(a.instructions().len(), 2);
	/// let instrs = a.take_instructions();
	/// assert_eq!(a.instructions().len(), 0);
	/// assert_eq!(instrs.len(), 2);
	/// assert_eq!(instrs, vec![
	///     Instruction::with1(Code::Push_r64, Register::RCX)?,
	///     Instruction::with2(Code::Xor_rm64_r64, Register::RCX, Register::RDX)?,
	/// ]);
	/// # Ok(())
	/// # }
	/// ```
	#[must_use]
	#[inline]
	pub fn take_instructions(&mut self) -> Vec<Instruction> {
		let instrs = core::mem::take(&mut self.instructions);
		self.reset();
		instrs
	}

	/// Resets all instructions and labels so this instance can be re-used
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::code_asm::*;
	///
	/// # fn main() -> Result<(), IcedError> {
	/// let mut a = CodeAssembler::new(64)?;
	/// a.push(rcx)?;
	/// a.xor(rcx, rdx)?;
	/// assert_eq!(a.instructions().len(), 2);
	/// a.reset();
	/// assert_eq!(a.instructions().len(), 0);
	/// # Ok(())
	/// # }
	/// ```
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
	/// Fails if the label wasn't created by [`create_label()`], if this method was called
	/// multiple times for the same label, or if the next instruction already has a label.
	///
	/// [`create_label()`]: #method.create_label
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::code_asm::*;
	///
	/// # fn main() -> Result<(), IcedError> {
	/// let mut a = CodeAssembler::new(64)?;
	/// let mut label1 = a.create_label();
	/// a.push(rcx)?;
	/// // The address of this label is the next added instruction
	/// a.set_current_label(&mut label1)?;
	/// a.xor(rcx, rdx)?;
	/// // Target is the `xor rcx, rdx` instruction
	/// a.je(label1)?;
	/// a.nop()?;
	/// let bytes = a.assemble(0x1234_5678)?;
	/// assert_eq!(bytes, b"\x51\x48\x31\xD1\x74\xFB\x90");
	/// # Ok(())
	/// # }
	/// ```
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
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::code_asm::*;
	///
	/// # fn main() -> Result<(), IcedError> {
	/// let mut a = CodeAssembler::new(64)?;
	/// a.push(rcx)?;
	/// // The address of this label is the next added instruction
	/// a.anonymous_label()?;
	/// a.xor(rcx, rdx)?;
	/// // Target is the `xor rcx, rdx` instruction
	/// let anon = a.bwd()?; // Unfortunately, Rust forces us to create a local
	/// a.je(anon)?;
	/// // Target is the `sub eax, eax` instruction
	/// let anon = a.fwd(); // Unfortunately, Rust forces us to create a local
	/// a.js(anon)?;
	/// a.nop()?;
	/// // Create the label referenced by `fwd()` above
	/// a.anonymous_label()?;
	/// a.sub(eax, eax)?;
	/// let bytes = a.assemble(0x1234_5678)?;
	/// assert_eq!(bytes, b"\x51\x48\x31\xD1\x74\xFB\x78\x01\x90\x29\xC0");
	/// # Ok(())
	/// # }
	/// ```
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

	/// Gets the next anonymous label created by a future call to [`anonymous_label()`]
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
		if !self.current_label.is_empty() && self.defined_anon_label {
			return Err(IcedError::new("You can't create both an anonymous label and a normal label"));
		}
		if !self.current_label.is_empty() {
			instruction.set_ip(self.current_label.id());
		} else if self.defined_anon_label {
			instruction.set_ip(self.current_anon_label.id());
		}

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
		self.current_label = CodeLabel::default();
		self.defined_anon_label = false;
		self.prefix_flags = PrefixFlags::NONE;
		Ok(())
	}

	/// Encodes all added instructions and returns the result
	///
	/// # Errors
	///
	/// Fails if an error was detected (eg. an invalid instruction operand)
	///
	/// # Arguments
	///
	/// * `ip`: Base address of all instructions
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::code_asm::*;
	///
	/// # fn main() -> Result<(), IcedError> {
	/// let mut a = CodeAssembler::new(64)?;
	/// let mut label1 = a.create_label();
	/// a.push(rcx)?;
	/// // The address of this label is the next added instruction
	/// a.set_current_label(&mut label1)?;
	/// a.xor(rcx, rdx)?;
	/// // Target is the `xor rcx, rdx` instruction
	/// a.je(label1)?;
	/// a.nop()?;
	/// let bytes = a.assemble(0x1234_5678)?;
	/// assert_eq!(bytes, b"\x51\x48\x31\xD1\x74\xFB\x90");
	/// # Ok(())
	/// # }
	/// ```
	#[inline]
	pub fn assemble(&mut self, ip: u64) -> Result<Vec<u8>, IcedError> {
		if self.prefix_flags != 0 {
			return Err(IcedError::new("Unused prefixes. Did you forget to add an instruction?"));
		}
		if self.defined_anon_label {
			return Err(IcedError::new("Unused anonymous label. Did you forget to add an instruction?"));
		}
		if !self.next_anon_label.is_empty() {
			return Err(IcedError::new("Unused anonymous fwd() label. Did you forget to call anonymous_label()?"));
		}

		let options = BlockEncoderOptions::NONE;
		let block = InstructionBlock::new(self.instructions(), ip);
		let result = BlockEncoder::encode(self.bitness(), block, options)?;
		Ok(result.code_buffer)
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
}
