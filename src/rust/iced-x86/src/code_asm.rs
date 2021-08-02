// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

//! TODO:

mod errors;

pub use crate::code_asm::errors::*;

mod private {
	pub trait Sealed {}
}

/// TODO:
#[allow(missing_debug_implementations, missing_copy_implementations)] //TODO:
pub struct CodeAssembler;

impl crate::code_asm::private::Sealed for CodeAssembler {}
