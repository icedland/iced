// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use alloc::borrow::Cow;
use alloc::string::String;
use core::fmt;
#[cfg(feature = "std")]
use std::error::Error;

/// iced error
#[derive(Debug, Clone)]
pub struct IcedError {
	error: Cow<'static, str>,
}

struct _TraitsCheck
where
	IcedError: fmt::Debug + Clone + fmt::Display + Send + Sync;
#[cfg(feature = "std")]
struct _TraitsCheckStd
where
	IcedError: Error;

impl IcedError {
	#[allow(dead_code)]
	pub(crate) fn new(error: &'static str) -> Self {
		Self { error: error.into() }
	}

	#[allow(dead_code)]
	pub(crate) fn with_string(error: String) -> Self {
		Self { error: error.into() }
	}
}

#[cfg(feature = "std")]
impl Error for IcedError {
	// Required since MSRV < 1.42.0
	#[allow(clippy::missing_inline_in_public_items)]
	fn description(&self) -> &str {
		&self.error
	}
}

impl fmt::Display for IcedError {
	#[allow(clippy::missing_inline_in_public_items)]
	fn fmt(&self, f: &mut fmt::Formatter<'_>) -> fmt::Result {
		write!(f, "{}", &self.error)
	}
}
