// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

mod mvex_data;
mod mvex_info;
pub(crate) mod mvex_memsz_lut;
#[cfg(any(feature = "decoder", feature = "encoder"))]
pub(crate) mod mvex_tt_lut;

use crate::iced_constants::IcedConstants;
use crate::Code;

pub(crate) fn get_mvex_info(code: Code) -> &'static self::mvex_info::MvexInfo {
	debug_assert!(IcedConstants::is_mvex(code));
	&self::mvex_data::MVEX_INFO[code as usize - IcedConstants::MVEX_START as usize]
}
