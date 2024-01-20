// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

//! Easier creating of instructions (eg. `a.mov(eax, ecx)`) than using `Instruction::with*()` functions.
//!
//! This requires the `code_asm` feature to use (not enabled by default). Add it to your `Cargo.toml`:
//!
//! ```toml
//! [dependencies.iced-x86]
//! version = "1.21.0"
//! features = ["code_asm"]
//! ```
//!
//! See [`CodeAssembler`] docs for usage.
//!
//! [`CodeAssembler`]: struct.CodeAssembler.html

pub mod asm_traits;
mod code_asm_methods;
mod fn_asm_impl;
mod fn_asm_pub;
mod mem;
mod op_state;
mod reg;
pub mod registers;
#[cfg(test)]
mod tests;

pub use crate::code_asm::mem::*;
pub use crate::code_asm::reg::*;
pub use crate::code_asm::registers::*;
pub use crate::IcedError;
use crate::{BlockEncoderResult, Instruction};
use alloc::vec::Vec;
use core::hash::{Hash, Hasher};

struct PrefixFlags;
impl PrefixFlags {
	const NONE: u8 = 0x00;
	const LOCK: u8 = 0x01;
	const REPE: u8 = 0x02;
	const REPNE: u8 = 0x04;
	const NOTRACK: u8 = 0x08;
	const PREFER_VEX: u8 = 0x10;
	const PREFER_EVEX: u8 = 0x20;
}

struct CodeAssemblerOptions;
impl CodeAssemblerOptions {
	const PREFER_VEX: u8 = 0x01;
	const PREFER_SHORT_BRANCH: u8 = 0x02;
}

/// Creates and encodes instructions. It's easier to use this struct than to call `Instruction::with*()` functions.
///
/// This requires the `code_asm` feature to use (not enabled by default). Add it to your `Cargo.toml`:
///
/// ```toml
/// [dependencies.iced-x86]
/// version = "1.21.0"
/// features = ["code_asm"]
/// ```
///
/// # Examples
///
/// ```
/// use iced_x86::code_asm::*;
///
/// # fn main() -> Result<(), IcedError> {
/// let mut a = CodeAssembler::new(64)?;
///
/// // Anytime you add something to a register (or subtract from it), you create a
/// // memory operand. You can also call word_ptr(), dword_bcst() etc to create memory
/// // operands.
/// let _ = rax; // register
/// let _ = rax + 0; // memory with no size hint
/// let _ = ptr(rax); // memory with no size hint
/// let _ = rax + rcx * 4 - 123; // memory with no size hint
/// // To create a memory operand with only a displacement or only a base register,
/// // you can call one of the memory fns:
/// let _ = qword_ptr(123); // memory with a qword size hint
/// let _ = dword_bcst(rcx); // memory (broadcast) with a dword size hint
/// // To add a segment override, call the segment methods:
/// let _ = ptr(rax).fs(); // fs:[rax]
///
/// // Each mnemonic is a method
/// a.push(rcx)?;
/// // There are a few exceptions where you must append `_<opcount>` to the mnemonic to
/// // get the instruction you need:
/// a.ret()?;
/// a.ret_1(123)?;
/// // Use byte_ptr(), word_bcst(), etc to force the arg to a memory operand and to add a
/// // size hint
/// a.xor(byte_ptr(rdx+r14*4+123), 0x10)?;
/// // Prefixes are also methods
/// a.rep().stosd()?;
/// // Sometimes, you must add an integer suffix to help the compiler:
/// a.mov(rax, 0x1234_5678_9ABC_DEF0u64)?;
///
/// // Create labels that can be referenced by code
/// let mut loop_lbl1 = a.create_label();
/// let mut after_loop1 = a.create_label();
/// a.mov(ecx, 10)?;
/// a.set_label(&mut loop_lbl1)?;
/// a.dec(ecx)?;
/// a.jp(after_loop1)?;
/// a.jne(loop_lbl1)?;
/// a.set_label(&mut after_loop1)?;
///
/// // It's possible to reference labels with RIP-relative addressing
/// let mut skip_data = a.create_label();
/// let mut data = a.create_label();
/// a.jmp(skip_data)?;
/// a.set_label(&mut data)?;
/// a.db(b"\x90\xCC\xF1\x90")?;
/// a.set_label(&mut skip_data)?;
/// a.lea(rax, ptr(data))?;
///
/// // AVX512 opmasks, {z}, {sae}, {er} and broadcasting are also supported:
/// a.vsqrtps(zmm16.k2().z(), dword_bcst(rcx))?;
/// a.vsqrtps(zmm1.k2().z(), zmm23.rd_sae())?;
/// // Sometimes, the encoder doesn't know if you want VEX or EVEX encoding.
/// // You can force EVEX globally like so:
/// a.set_prefer_vex(false);
/// a.vucomiss(xmm31, xmm15.sae())?;
/// a.vucomiss(xmm31, ptr(rcx))?;
/// // or call vex()/evex() to override the encoding option:
/// a.evex().vucomiss(xmm31, xmm15.sae())?;
/// a.vex().vucomiss(xmm15, xmm14)?;
///
/// // Encode all added instructions
/// let bytes = a.assemble(0x1234_5678)?;
/// assert_eq!(bytes.len(), 82);
/// // If you don't want to encode them, you can get all instructions by calling
/// // one of these methods:
/// let instrs = a.instructions(); // Get a reference to the internal vec
/// assert_eq!(instrs.len(), 19);
/// let instrs = a.take_instructions(); // Take ownership of the vec with all instructions
/// assert_eq!(instrs.len(), 19);
/// assert_eq!(a.instructions().len(), 0);
/// # Ok(())
/// # }
/// ```
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
		self.id.hash(state);
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

/// Result of assembling the instructions
#[derive(Debug)]
#[cfg_attr(not(feature = "exhaustive_enums"), non_exhaustive)]
pub struct CodeAssemblerResult {
	/// Inner `BlockEncoder` result
	pub inner: BlockEncoderResult,
}

impl CodeAssemblerResult {
	/// Gets the address of a label
	///
	/// # Notes
	///
	/// You should pass [`BlockEncoderOptions::RETURN_NEW_INSTRUCTION_OFFSETS`] to [`CodeAssembler::assemble_options()`] or this method will fail.
	///
	/// # Arguments
	///
	/// * `label`: The label
	///
	/// # Errors
	///
	/// Fails if the label is invalid
	///
	/// [`BlockEncoderOptions::RETURN_NEW_INSTRUCTION_OFFSETS`]: ../struct.BlockEncoderOptions.html#associatedconstant.RETURN_NEW_INSTRUCTION_OFFSETS
	/// [`CodeAssembler::assemble_options()`]: struct.CodeAssembler.html#method.assemble_options
	#[allow(clippy::missing_inline_in_public_items)]
	pub fn label_ip(&self, label: &CodeLabel) -> Result<u64, IcedError> {
		if label.is_empty() {
			return Err(IcedError::new("Invalid label. Must be created via `CodeAssembler::create_label()`."));
		}
		if !label.has_instruction_index() {
			return Err(IcedError::new(
				"The label is not associated with an instruction index. It must be emitted via `CodeAssembler::set_label()`.",
			));
		}
		let new_offset = if let Some(new_offset) = self.inner.new_instruction_offsets.get(label.instruction_index) {
			*new_offset
		} else {
			return Err(IcedError::new(
				"Invalid label instruction index or `BlockEncoderOptions::RETURN_NEW_INSTRUCTION_OFFSETS` option was not enabled when calling `assemble_options()`.",
			));
		};
		if new_offset == u32::MAX {
			Err(IcedError::new("The instruction was re-written to a longer instruction (eg. JE NEAR -> JE FAR) and there's no instruction offset. Consider using a `zero_bytes()` instruction as a label instead of a normal instruction or disable branch optimizations."))
		} else {
			Ok(self.inner.rip.wrapping_add(new_offset as u64))
		}
	}
}
