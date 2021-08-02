// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

//! TODO:

mod errors;
mod mem;
mod op_state;
mod reg;
pub mod registers;

pub use crate::code_asm::errors::*;
pub use crate::code_asm::mem::*;
pub use crate::code_asm::reg::*;
pub use crate::code_asm::registers::*;

mod private {
	pub trait Sealed {}
}

/// TODO:
#[allow(missing_debug_implementations, missing_copy_implementations)] //TODO:
pub struct CodeAssembler;

impl crate::code_asm::private::Sealed for CodeAssembler {}
