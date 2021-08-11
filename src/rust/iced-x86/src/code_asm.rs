// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

//! Easier creating of instructions (eg. `a.mov(eax, ecx)`) than using `Instruction::with*()` functions.

mod code_asm_methods;
mod fn_asm_impl;
mod fn_asm_pub;
mod fn_asm_traits;
mod mem;
mod op_state;
mod reg;
pub mod registers;

pub use crate::code_asm::mem::*;
pub use crate::code_asm::reg::*;
pub use crate::code_asm::registers::*;
pub use crate::IcedError;
use crate::Instruction;
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
///
/// # Examples
///
/// ```
/// use iced_x86::code_asm::*;
///
/// # fn main() -> Result<(), IcedError> {
/// let mut a = CodeAssembler::new(64)?;
///
/// // Anytime you add something to a register (or subtract from it), you create a memory operand.
/// // You can also call word_ptr(), dword_bcst() etc to create memory operands.
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
/// // There are a few exceptions where you must append `_<opcount>` to the mnemonic to get the
/// // instruction you need:
/// a.ret()?;
/// a.ret_1(123)?;
/// // Use byte_ptr(), word_bcst(), etc to force the arg to a memory operand and to add a size hint
/// a.xor(byte_ptr(rdx+r14*4+123), 0x10)?;
/// // Prefixes are also methods
/// a.rep().stosd()?;
///
/// // Create labels that can be referenced by code
/// let mut loop_lbl1 = a.create_label();
/// let mut after_loop1 = a.create_label();
/// a.mov(ecx, 10)?;
/// a.set_current_label(&mut loop_lbl1)?;
/// a.dec(ecx)?;
/// a.jp(after_loop1)?;
/// a.jne(loop_lbl1)?;
/// a.set_current_label(&mut after_loop1)?;
///
/// // AVX512 opmasks, {z}, {sae}, {er} and broadcasting are also supported:
/// a.vsqrtps(zmm16.k2().z(), dword_bcst(rcx))?;
/// a.vsqrtps(zmm1.k2().z(), zmm23.rd_sae())?;
/// // Sometimes, the encoder doesn't know if you want VEX or EVEX encoding.
/// // You can force EVEX like so:
/// a.set_prefer_vex(false);
/// a.vucomiss(xmm31, xmm15.sae())?;
/// a.vucomiss(xmm31, ptr(rcx))?;
///
/// // Encode all added instructions
/// let bytes = a.assemble(0x1234_5678)?;
/// assert_eq!(bytes.len(), 48);
/// // If you don't want to encode them, you can get all instructions by calling
/// // one of these methods:
/// let instrs = a.instructions(); // Get a reference to the internal vec
/// assert_eq!(instrs.len(), 13);
/// let instrs = a.take_instructions(); // Take ownership of the vec with all instructions
/// assert_eq!(instrs.len(), 13);
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
