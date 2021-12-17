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
	pub(crate) const fn new(error: &'static str) -> Self {
		Self { error: Cow::Borrowed(error) }
	}

	#[allow(dead_code)]
	pub(crate) const fn with_string(error: String) -> Self {
		Self { error: Cow::Owned(error) }
	}
}

#[cfg(feature = "std")]
impl Error for IcedError {}

impl fmt::Display for IcedError {
	#[inline]
	fn fmt(&self, f: &mut fmt::Formatter<'_>) -> fmt::Result {
		write!(f, "{}", &self.error)
	}
}
