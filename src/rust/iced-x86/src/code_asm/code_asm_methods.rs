// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

// All methods are in this file instead of code_asm.rs to prevent `cargo doc` from
// putting all generated methods (thousands of methods) ahead of non-generated fns
// such as the constructor, prefix fns, etc.

use crate::code_asm::op_state::CodeAsmOpState;
use crate::code_asm::{CodeAssembler, CodeAssemblerOptions, CodeAssemblerResult, CodeLabel, PrefixFlags};
use crate::IcedError;
use crate::{BlockEncoder, BlockEncoderOptions, Code, Instruction, InstructionBlock, MemoryOperand, Register};
use alloc::vec::Vec;

impl CodeAssembler {
	const MAX_DB_COUNT: usize = 16;
	const MAX_DW_COUNT: usize = CodeAssembler::MAX_DB_COUNT / 2;
	const MAX_DD_COUNT: usize = CodeAssembler::MAX_DB_COUNT / 4;
	const MAX_DQ_COUNT: usize = CodeAssembler::MAX_DB_COUNT / 8;

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
		self.prefix_flags |= PrefixFlags::REPNE;
		self
	}

	/// Adds an `XRELEASE` prefix to the next added instruction
	#[must_use]
	#[inline]
	pub fn xrelease(&mut self) -> &mut Self {
		self.prefix_flags |= PrefixFlags::REPE;
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

	/// Adds a `REPE`/`REPZ` prefix to the next added instruction
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

	/// Adds a `REPE`/`REPZ` prefix to the next added instruction
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::code_asm::*;
	///
	/// # fn main() -> Result<(), IcedError> {
	/// let mut a = CodeAssembler::new(64)?;
	///
	/// a.repz().cmpsb()?;
	/// a.nop()?;
	///
	/// let bytes = a.assemble(0x1234_5678)?;
	/// assert_eq!(bytes, vec![0xF3, 0xA6, 0x90]);
	/// # Ok(())
	/// # }
	/// ```
	#[must_use]
	#[inline]
	pub fn repz(&mut self) -> &mut Self {
		self.repe()
	}

	/// Adds a `REPNE`/`REPNZ` prefix to the next added instruction
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

	/// Adds a `REPNE`/`REPNZ` prefix to the next added instruction
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::code_asm::*;
	///
	/// # fn main() -> Result<(), IcedError> {
	/// let mut a = CodeAssembler::new(64)?;
	///
	/// a.repnz().scasb()?;
	/// a.nop()?;
	///
	/// let bytes = a.assemble(0x1234_5678)?;
	/// assert_eq!(bytes, vec![0xF2, 0xAE, 0x90]);
	/// # Ok(())
	/// # }
	/// ```
	#[must_use]
	#[inline]
	pub fn repnz(&mut self) -> &mut Self {
		self.repne()
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

	/// Prefer `VEX` encoding if the next instruction can be `VEX` and `EVEX` encoded
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::code_asm::*;
	///
	/// # fn main() -> Result<(), IcedError> {
	/// let mut a = CodeAssembler::new(64)?;
	///
	/// // This instruction can be VEX and EVEX encoded
	/// a.vex().vaddpd(xmm1, xmm2, xmm3)?;
	/// a.evex().vaddpd(xmm1, xmm2, xmm3)?;
	///
	/// let bytes = a.assemble(0x1234_5678)?;
	/// assert_eq!(bytes, b"\xC5\xE9\x58\xCB\x62\xF1\xED\x08\x58\xCB");
	/// # Ok(())
	/// # }
	/// ```
	#[must_use]
	#[inline]
	pub fn vex(&mut self) -> &mut Self {
		self.prefix_flags |= PrefixFlags::PREFER_VEX;
		self
	}

	/// Prefer `EVEX` encoding if the next instruction can be `VEX` and `EVEX` encoded
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::code_asm::*;
	///
	/// # fn main() -> Result<(), IcedError> {
	/// let mut a = CodeAssembler::new(64)?;
	///
	/// // This instruction can be VEX and EVEX encoded
	/// a.vex().vaddpd(xmm1, xmm2, xmm3)?;
	/// a.evex().vaddpd(xmm1, xmm2, xmm3)?;
	///
	/// let bytes = a.assemble(0x1234_5678)?;
	/// assert_eq!(bytes, b"\xC5\xE9\x58\xCB\x62\xF1\xED\x08\x58\xCB");
	/// # Ok(())
	/// # }
	/// ```
	#[must_use]
	#[inline]
	pub fn evex(&mut self) -> &mut Self {
		self.prefix_flags |= PrefixFlags::PREFER_EVEX;
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
	/// assert_eq!(a.instructions(), &[
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
	/// a.set_label(&mut label1)?;
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
	pub fn set_label(&mut self, label: &mut CodeLabel) -> Result<(), IcedError> {
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
	/// let anon = a.fwd()?; // Unfortunately, Rust forces us to create a local
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
	///
	/// # Errors
	///
	/// Fails if an error was detected
	#[inline]
	pub fn fwd(&mut self) -> Result<CodeLabel, IcedError> {
		// This method returns a `Result<T, E>` for consistency with other methods, including `bwd()`,
		// so you don't have to memorize which methods return a Result and which don't.

		if self.next_anon_label.is_empty() {
			self.next_anon_label = self.create_label();
		}
		Ok(self.next_anon_label)
	}

	#[inline]
	fn decl_data_verify_no_prefixes(&self) -> Result<(), IcedError> {
		if self.prefix_flags != 0 {
			Err(IcedError::new("db/dw/dd/dq: No prefixes are allowed"))
		} else {
			Ok(())
		}
	}

	/// Adds data
	///
	/// # Errors
	///
	/// Fails if an error was detected
	///
	/// # Arguments
	///
	/// * `data`: The data that will be added at the current position
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::code_asm::*;
	///
	/// # fn main() -> Result<(), IcedError> {
	/// let mut a = CodeAssembler::new(64)?;
	/// a.int3()?;
	/// a.db(b"\x16\x85\x10\xA0\xFA\x9E\x11\xEB\x97\x34\x3B\x7E\xB7\x2B\x92\x63\x16\x85")?;
	/// a.nop()?;
	/// let bytes = a.assemble(0x1234_5678)?;
	/// assert_eq!(bytes, b"\xCC\x16\x85\x10\xA0\xFA\x9E\x11\xEB\x97\x34\x3B\x7E\xB7\x2B\x92\x63\x16\x85\x90");
	/// # Ok(())
	/// # }
	/// ```
	#[allow(clippy::missing_inline_in_public_items)]
	pub fn db(&mut self, data: &[u8]) -> Result<(), IcedError> {
		self.decl_data_verify_no_prefixes()?;
		for bytes in data.chunks(CodeAssembler::MAX_DB_COUNT) {
			self.add_instr(Instruction::with_declare_byte(bytes)?)?;
		}

		Ok(())
	}

	/// Adds data
	///
	/// # Errors
	///
	/// Fails if an error was detected
	///
	/// # Arguments
	///
	/// * `data`: The data that will be added at the current position
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::code_asm::*;
	///
	/// # fn main() -> Result<(), IcedError> {
	/// let mut a = CodeAssembler::new(64)?;
	/// a.int3()?;
	/// a.db_i(&[0x16, -0x7B, 0x10, -0x60, -0x06, -0x62, 0x11, -0x15, -0x69, 0x34, 0x3B, 0x7E, -0x49, 0x2B, -0x6E, 0x63, 0x16, -0x7B])?;
	/// a.nop()?;
	/// let bytes = a.assemble(0x1234_5678)?;
	/// assert_eq!(bytes, b"\xCC\x16\x85\x10\xA0\xFA\x9E\x11\xEB\x97\x34\x3B\x7E\xB7\x2B\x92\x63\x16\x85\x90");
	/// # Ok(())
	/// # }
	/// ```
	#[allow(clippy::missing_inline_in_public_items)]
	pub fn db_i(&mut self, data: &[i8]) -> Result<(), IcedError> {
		self.decl_data_verify_no_prefixes()?;
		let mut tmp = [0; CodeAssembler::MAX_DB_COUNT];
		for bytes in data.chunks(CodeAssembler::MAX_DB_COUNT) {
			for (t, b) in tmp.iter_mut().zip(bytes.iter()) {
				*t = *b as u8;
			}
			self.add_instr(Instruction::with_declare_byte(&tmp[0..bytes.len()])?)?;
		}

		Ok(())
	}

	/// Adds data
	///
	/// # Errors
	///
	/// Fails if an error was detected
	///
	/// # Arguments
	///
	/// * `data`: The data that will be added at the current position
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::code_asm::*;
	///
	/// # fn main() -> Result<(), IcedError> {
	/// let mut a = CodeAssembler::new(64)?;
	/// a.int3()?;
	/// a.dw(&[0x4068, 0x7956, 0xFA9F, 0x11EB, 0x9467, 0x77FA, 0x747C, 0xD088, 0x7D7E])?;
	/// a.nop()?;
	/// let bytes = a.assemble(0x1234_5678)?;
	/// assert_eq!(bytes, b"\xCC\x68\x40\x56\x79\x9F\xFA\xEB\x11\x67\x94\xFA\x77\x7C\x74\x88\xD0\x7E\x7D\x90");
	/// # Ok(())
	/// # }
	/// ```
	#[allow(clippy::missing_inline_in_public_items)]
	pub fn dw(&mut self, data: &[u16]) -> Result<(), IcedError> {
		self.decl_data_verify_no_prefixes()?;
		for words in data.chunks(CodeAssembler::MAX_DW_COUNT) {
			self.add_instr(Instruction::with_declare_word(words)?)?;
		}

		Ok(())
	}

	/// Adds data
	///
	/// # Errors
	///
	/// Fails if an error was detected
	///
	/// # Arguments
	///
	/// * `data`: The data that will be added at the current position
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::code_asm::*;
	///
	/// # fn main() -> Result<(), IcedError> {
	/// let mut a = CodeAssembler::new(64)?;
	/// a.int3()?;
	/// a.dw_i(&[0x4068, 0x7956, -0x0561, 0x11EB, -0x6B99, 0x77FA, 0x747C, -0x2F78, 0x7D7E])?;
	/// a.nop()?;
	/// let bytes = a.assemble(0x1234_5678)?;
	/// assert_eq!(bytes, b"\xCC\x68\x40\x56\x79\x9F\xFA\xEB\x11\x67\x94\xFA\x77\x7C\x74\x88\xD0\x7E\x7D\x90");
	/// # Ok(())
	/// # }
	/// ```
	#[allow(clippy::missing_inline_in_public_items)]
	pub fn dw_i(&mut self, data: &[i16]) -> Result<(), IcedError> {
		self.decl_data_verify_no_prefixes()?;
		let mut tmp = [0; CodeAssembler::MAX_DW_COUNT];
		for words in data.chunks(CodeAssembler::MAX_DW_COUNT) {
			for (t, w) in tmp.iter_mut().zip(words.iter()) {
				*t = *w as u16;
			}
			self.add_instr(Instruction::with_declare_word(&tmp[0..words.len()])?)?;
		}

		Ok(())
	}

	/// Adds data
	///
	/// # Errors
	///
	/// Fails if an error was detected
	///
	/// # Arguments
	///
	/// * `data`: The data that will be added at the current position
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::code_asm::*;
	///
	/// # fn main() -> Result<(), IcedError> {
	/// let mut a = CodeAssembler::new(64)?;
	/// a.int3()?;
	/// a.dd(&[0x40687956, 0xFA9F11EB, 0x946777FA, 0x747CD088, 0x7D7E7C58])?;
	/// a.nop()?;
	/// let bytes = a.assemble(0x1234_5678)?;
	/// assert_eq!(bytes, b"\xCC\x56\x79\x68\x40\xEB\x11\x9F\xFA\xFA\x77\x67\x94\x88\xD0\x7C\x74\x58\x7C\x7E\x7D\x90");
	/// # Ok(())
	/// # }
	/// ```
	#[allow(clippy::missing_inline_in_public_items)]
	pub fn dd(&mut self, data: &[u32]) -> Result<(), IcedError> {
		self.decl_data_verify_no_prefixes()?;
		for dwords in data.chunks(CodeAssembler::MAX_DD_COUNT) {
			self.add_instr(Instruction::with_declare_dword(dwords)?)?;
		}

		Ok(())
	}

	/// Adds data
	///
	/// # Errors
	///
	/// Fails if an error was detected
	///
	/// # Arguments
	///
	/// * `data`: The data that will be added at the current position
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::code_asm::*;
	///
	/// # fn main() -> Result<(), IcedError> {
	/// let mut a = CodeAssembler::new(64)?;
	/// a.int3()?;
	/// a.dd_i(&[0x40687956, -0x0560EE15, -0x6B988806, 0x747CD088, 0x7D7E7C58])?;
	/// a.nop()?;
	/// let bytes = a.assemble(0x1234_5678)?;
	/// assert_eq!(bytes, b"\xCC\x56\x79\x68\x40\xEB\x11\x9F\xFA\xFA\x77\x67\x94\x88\xD0\x7C\x74\x58\x7C\x7E\x7D\x90");
	/// # Ok(())
	/// # }
	/// ```
	#[allow(clippy::missing_inline_in_public_items)]
	pub fn dd_i(&mut self, data: &[i32]) -> Result<(), IcedError> {
		self.decl_data_verify_no_prefixes()?;
		let mut tmp = [0; CodeAssembler::MAX_DD_COUNT];
		for dwords in data.chunks(CodeAssembler::MAX_DD_COUNT) {
			for (t, d) in tmp.iter_mut().zip(dwords.iter()) {
				*t = *d as u32;
			}
			self.add_instr(Instruction::with_declare_dword(&tmp[0..dwords.len()])?)?;
		}

		Ok(())
	}

	/// Adds data
	///
	/// # Errors
	///
	/// Fails if an error was detected
	///
	/// # Arguments
	///
	/// * `data`: The data that will be added at the current position
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::code_asm::*;
	///
	/// # fn main() -> Result<(), IcedError> {
	/// let mut a = CodeAssembler::new(64)?;
	/// a.int3()?;
	/// a.dd_f32(&[3.14, -1234.5678, 1e12, -3.14, 1234.5678, -1e12])?;
	/// a.nop()?;
	/// let bytes = a.assemble(0x1234_5678)?;
	/// assert_eq!(bytes, b"\xCC\xC3\xF5\x48\x40\x2B\x52\x9A\xC4\xA5\xD4\x68\x53\xC3\xF5\x48\xC0\x2B\x52\x9A\x44\xA5\xD4\x68\xD3\x90");
	/// # Ok(())
	/// # }
	/// ```
	#[allow(clippy::missing_inline_in_public_items)]
	pub fn dd_f32(&mut self, data: &[f32]) -> Result<(), IcedError> {
		self.decl_data_verify_no_prefixes()?;
		let mut tmp = [0; CodeAssembler::MAX_DD_COUNT];
		for dwords in data.chunks(CodeAssembler::MAX_DD_COUNT) {
			for (t, d) in tmp.iter_mut().zip(dwords.iter()) {
				*t = f32::to_bits(*d);
			}
			self.add_instr(Instruction::with_declare_dword(&tmp[0..dwords.len()])?)?;
		}

		Ok(())
	}

	/// Adds data
	///
	/// # Errors
	///
	/// Fails if an error was detected
	///
	/// # Arguments
	///
	/// * `data`: The data that will be added at the current position
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::code_asm::*;
	///
	/// # fn main() -> Result<(), IcedError> {
	/// let mut a = CodeAssembler::new(64)?;
	/// a.int3()?;
	/// a.dq(&[0x40687956FA9F11EB, 0x946777FA747CD088, 0x7D7E7C5814C2BA6E])?;
	/// a.nop()?;
	/// let bytes = a.assemble(0x1234_5678)?;
	/// assert_eq!(bytes, b"\xCC\xEB\x11\x9F\xFA\x56\x79\x68\x40\x88\xD0\x7C\x74\xFA\x77\x67\x94\x6E\xBA\xC2\x14\x58\x7C\x7E\x7D\x90");
	/// # Ok(())
	/// # }
	/// ```
	#[allow(clippy::missing_inline_in_public_items)]
	pub fn dq(&mut self, data: &[u64]) -> Result<(), IcedError> {
		self.decl_data_verify_no_prefixes()?;
		for qwords in data.chunks(CodeAssembler::MAX_DQ_COUNT) {
			self.add_instr(Instruction::with_declare_qword(qwords)?)?;
		}

		Ok(())
	}

	/// Adds data
	///
	/// # Errors
	///
	/// Fails if an error was detected
	///
	/// # Arguments
	///
	/// * `data`: The data that will be added at the current position
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::code_asm::*;
	///
	/// # fn main() -> Result<(), IcedError> {
	/// let mut a = CodeAssembler::new(64)?;
	/// a.int3()?;
	/// a.dq_i(&[0x40687956FA9F11EB, -0x6B9888058B832F78, 0x7D7E7C5814C2BA6E])?;
	/// a.nop()?;
	/// let bytes = a.assemble(0x1234_5678)?;
	/// assert_eq!(bytes, b"\xCC\xEB\x11\x9F\xFA\x56\x79\x68\x40\x88\xD0\x7C\x74\xFA\x77\x67\x94\x6E\xBA\xC2\x14\x58\x7C\x7E\x7D\x90");
	/// # Ok(())
	/// # }
	/// ```
	#[allow(clippy::missing_inline_in_public_items)]
	pub fn dq_i(&mut self, data: &[i64]) -> Result<(), IcedError> {
		self.decl_data_verify_no_prefixes()?;
		let mut tmp = [0; CodeAssembler::MAX_DQ_COUNT];
		for qwords in data.chunks(CodeAssembler::MAX_DQ_COUNT) {
			for (t, q) in tmp.iter_mut().zip(qwords.iter()) {
				*t = *q as u64;
			}
			self.add_instr(Instruction::with_declare_qword(&tmp[0..qwords.len()])?)?;
		}

		Ok(())
	}

	/// Adds data
	///
	/// # Errors
	///
	/// Fails if an error was detected
	///
	/// # Arguments
	///
	/// * `data`: The data that will be added at the current position
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::code_asm::*;
	///
	/// # fn main() -> Result<(), IcedError> {
	/// let mut a = CodeAssembler::new(64)?;
	/// a.int3()?;
	/// a.dq_f64(&[3.14, -1234.5678, 1e123])?;
	/// a.nop()?;
	/// let bytes = a.assemble(0x1234_5678)?;
	/// assert_eq!(bytes, b"\xCC\x1F\x85\xEB\x51\xB8\x1E\x09\x40\xAD\xFA\x5C\x6D\x45\x4A\x93\xC0\xF1\x72\xF8\xA5\x25\x34\x78\x59\x90");
	/// # Ok(())
	/// # }
	/// ```
	#[allow(clippy::missing_inline_in_public_items)]
	pub fn dq_f64(&mut self, data: &[f64]) -> Result<(), IcedError> {
		self.decl_data_verify_no_prefixes()?;
		let mut tmp = [0; CodeAssembler::MAX_DQ_COUNT];
		for qwords in data.chunks(CodeAssembler::MAX_DQ_COUNT) {
			for (t, q) in tmp.iter_mut().zip(qwords.iter()) {
				*t = f64::to_bits(*q);
			}
			self.add_instr(Instruction::with_declare_qword(&tmp[0..qwords.len()])?)?;
		}

		Ok(())
	}

	/// Adds nops, preferring long nops
	///
	/// # Errors
	///
	/// Fails if an error was detected
	///
	/// # Arguments
	///
	/// * `size`: Size in bytes of all nops
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::code_asm::*;
	///
	/// # fn main() -> Result<(), IcedError> {
	/// let mut a = CodeAssembler::new(64)?;
	/// a.nops_with_size(17)?;
	/// let bytes = a.assemble(0x1234_5678)?;
	/// assert_eq!(bytes.len(), 17);
	/// # Ok(())
	/// # }
	/// ```
	#[allow(clippy::missing_inline_in_public_items)]
	pub fn nops_with_size(&mut self, size: usize) -> Result<(), IcedError> {
		if self.prefix_flags != 0 {
			return Err(IcedError::new("No prefixes are allowed"));
		}

		const MAX_NOP_LEN: usize = 9;
		if size >= MAX_NOP_LEN {
			let bytes = self.get_nop_bytes(MAX_NOP_LEN);
			for _ in 0..size / MAX_NOP_LEN {
				self.db(bytes)?;
			}
		}
		match size % MAX_NOP_LEN {
			0 => {}
			remaining => self.db(self.get_nop_bytes(remaining))?,
		}

		Ok(())
	}

	fn get_nop_bytes(&self, size: usize) -> &'static [u8] {
		match size {
			1 => &[0x90],                   // NOP
			2 => &[0x66, 0x90],             // 66 NOP
			3 => &[0x0F, 0x1F, 0x00],       // NOP dword ptr [eax] or NOP word ptr [bx+si]
			4 => &[0x0F, 0x1F, 0x40, 0x00], // NOP dword ptr [eax + 00] or NOP word ptr [bx+si]
			5 => {
				if self.bitness() != 16 {
					&[0x0F, 0x1F, 0x44, 0x00, 0x00] // NOP dword ptr [eax + eax*1 + 00]
				} else {
					&[0x0F, 0x1F, 0x80, 0x00, 0x00] // NOP word ptr[bx + si]
				}
			}
			6 => {
				if self.bitness() != 16 {
					&[0x66, 0x0F, 0x1F, 0x44, 0x00, 0x00] // 66 NOP dword ptr [eax + eax*1 + 00]
				} else {
					&[0x66, 0x0F, 0x1F, 0x80, 0x00, 0x00] // NOP dword ptr [bx+si]
				}
			}
			7 => {
				if self.bitness() != 16 {
					&[0x0F, 0x1F, 0x80, 0x00, 0x00, 0x00, 0x00] // NOP dword ptr [eax + 00000000]
				} else {
					&[0x67, 0x66, 0x0F, 0x1F, 0x44, 0x00, 0x00] // NOP dword ptr [eax+eax]
				}
			}
			8 => {
				if self.bitness() != 16 {
					&[0x0F, 0x1F, 0x84, 0x00, 0x00, 0x00, 0x00, 0x00] // NOP dword ptr [eax + eax*1 + 00000000]
				} else {
					&[0x67, 0x0F, 0x1F, 0x80, 0x00, 0x00, 0x00, 0x00] // NOP word ptr [eax]
				}
			}
			9 => {
				if self.bitness() != 16 {
					&[0x66, 0x0F, 0x1F, 0x84, 0x00, 0x00, 0x00, 0x00, 0x00] // 66 NOP dword ptr [eax + eax*1 + 00000000]
				} else {
					&[0x67, 0x0F, 0x1F, 0x84, 0x00, 0x00, 0x00, 0x00, 0x00] // NOP word ptr [eax+eax]
				}
			}
			_ => unreachable!(),
		}
	}

	/// Adds an instruction created by the decoder or by `Instruction::with*()` methods
	///
	/// # Errors
	///
	/// Fails if an error was detected
	///
	/// # Arguments
	///
	/// * `instruction`: Instruction to add
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	/// use iced_x86::code_asm::*;
	///
	/// # fn main() -> Result<(), IcedError> {
	/// let mut a = CodeAssembler::new(64)?;
	/// a.nop()?;
	/// a.add_instruction(Instruction::with1(Code::Push_r64, Register::RCX)?)?;
	/// let bytes = a.assemble(0x1234_5678)?;
	/// assert_eq!(bytes, b"\x90\x51");
	/// # Ok(())
	/// # }
	/// ```
	#[inline]
	pub fn add_instruction(&mut self, instruction: Instruction) -> Result<(), IcedError> {
		self.add_instr(instruction)
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
	/// a.set_label(&mut label1)?;
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
		Ok(self.assemble_options(ip, BlockEncoderOptions::NONE)?.inner.code_buffer)
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
	/// * `options`: [`BlockEncoderOptions`] flags
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::BlockEncoderOptions;
	/// use iced_x86::code_asm::*;
	///
	/// # fn main() -> Result<(), IcedError> {
	/// let mut a = CodeAssembler::new(64)?;
	/// let mut label1 = a.create_label();
	/// a.push(rcx)?;
	/// // The address of this label is the next added instruction
	/// a.set_label(&mut label1)?;
	/// a.xor(rcx, rdx)?;
	/// // Target is the `xor rcx, rdx` instruction
	/// a.je(label1)?;
	/// a.nop()?;
	/// let result = a.assemble_options(0x1234_5678, BlockEncoderOptions::RETURN_NEW_INSTRUCTION_OFFSETS)?;
	/// assert_eq!(result.inner.rip, 0x1234_5678);
	/// assert_eq!(result.inner.code_buffer, b"\x51\x48\x31\xD1\x74\xFB\x90");
	/// // Get the address of the label, requires `BlockEncoderOptions::RETURN_NEW_INSTRUCTION_OFFSETS`
	/// assert_eq!(result.label_ip(&label1)?, 0x1234_5679);
	/// # Ok(())
	/// # }
	/// ```
	///
	/// [`BlockEncoderOptions`]: ../struct.BlockEncoderOptions.html
	#[inline]
	pub fn assemble_options(&mut self, ip: u64, options: u32) -> Result<CodeAssemblerResult, IcedError> {
		if self.prefix_flags != 0 {
			return Err(IcedError::new("Unused prefixes. Did you forget to add an instruction?"));
		}
		if !self.current_label.is_empty() {
			return Err(IcedError::new("Unused label. Did you forget to add an instruction?"));
		}
		if self.defined_anon_label {
			return Err(IcedError::new("Unused anonymous label. Did you forget to add an instruction?"));
		}
		if !self.next_anon_label.is_empty() {
			return Err(IcedError::new("Unused anonymous fwd() label. Did you forget to call anonymous_label()?"));
		}

		let block = InstructionBlock::new(self.instructions(), ip);
		let result = BlockEncoder::encode(self.bitness(), block, options)?;
		Ok(CodeAssemblerResult { inner: result })
	}

	/// Gets the bitness (16, 32 or 64)
	#[must_use]
	#[inline]
	pub fn bitness(&self) -> u32 {
		self.bitness
	}

	/// `true` (default value) to use `VEX` encoding intead of `EVEX` encoding if we must pick one of the encodings. See also [`vex()`] and [`evex()`]
	///
	/// [`vex()`]: #method.vex
	/// [`evex()`]: #method.evex
	#[must_use]
	#[inline]
	pub fn prefer_vex(&self) -> bool {
		(self.options & CodeAssemblerOptions::PREFER_VEX) != 0
	}

	/// `true` (default value) to use `VEX` encoding intead of `EVEX` encoding if we must pick one of the encodings. See also [`vex()`] and [`evex()`]
	///
	/// [`vex()`]: #method.vex
	/// [`evex()`]: #method.evex
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

	#[inline]
	pub(crate) fn instruction_prefer_vex(&self) -> bool {
		if (self.prefix_flags & (PrefixFlags::PREFER_VEX | PrefixFlags::PREFER_EVEX)) != 0 {
			(self.prefix_flags & PrefixFlags::PREFER_VEX) != 0
		} else {
			(self.options & CodeAssemblerOptions::PREFER_VEX) != 0
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
