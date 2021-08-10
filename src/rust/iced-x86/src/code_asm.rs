// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

//! Easier creating of instructions (eg. `a.mov(eax, ecx)`) than using `Instruction::with*()` functions.
//!
//! TODO:

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
