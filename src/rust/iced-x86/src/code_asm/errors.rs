// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use core::fmt;
#[cfg(feature = "std")]
use std::error::Error;

/// Error returned by [`CodeAssembler`].
///
/// [`CodeAssembler`]: struct.CodeAssembler.html
#[derive(Debug, Clone)]
#[allow(missing_copy_implementations)]
pub enum CodeAssemblerError {
	//TODO:
}

struct _TraitsCheck
where
	CodeAssemblerError: fmt::Debug + Clone + fmt::Display + Send + Sync;
#[cfg(feature = "std")]
struct _TraitsCheckStd
where
	CodeAssemblerError: Error;

#[cfg(feature = "std")]
impl Error for CodeAssemblerError {
	// Required since MSRV < 1.42.0
	#[allow(clippy::missing_inline_in_public_items)]
	fn description(&self) -> &str {
		"TODO:"
	}
}

impl fmt::Display for CodeAssemblerError {
	#[inline]
	fn fmt(&self, f: &mut fmt::Formatter<'_>) -> fmt::Result {
		write!(f, "TODO:")
	}
}
