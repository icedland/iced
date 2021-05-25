// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::decoder::handlers::{OpCodeHandler, OpCodeHandlerDecodeFn};
use crate::decoder::table_de::*;
use alloc::vec::Vec;
use lazy_static::lazy_static;

pub(super) struct Tables {
	pub(super) handlers_xx: Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
	#[cfg(not(feature = "no_vex"))]
	pub(super) handlers_vex_0fxx: Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
	#[cfg(not(feature = "no_vex"))]
	pub(super) handlers_vex_0f38xx: Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
	#[cfg(not(feature = "no_vex"))]
	pub(super) handlers_vex_0f3axx: Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
	#[cfg(not(feature = "no_evex"))]
	pub(super) handlers_evex_0fxx: Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
	#[cfg(not(feature = "no_evex"))]
	pub(super) handlers_evex_0f38xx: Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
	#[cfg(not(feature = "no_evex"))]
	pub(super) handlers_evex_0f3axx: Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
	#[cfg(not(feature = "no_xop"))]
	pub(super) handlers_xop8: Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
	#[cfg(not(feature = "no_xop"))]
	pub(super) handlers_xop9: Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
	#[cfg(not(feature = "no_xop"))]
	pub(super) handlers_xopa: Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
	#[cfg(feature = "no_vex")]
	#[allow(dead_code)]
	handlers_vex_0fxx: (),
	#[cfg(feature = "no_vex")]
	#[allow(dead_code)]
	handlers_vex_0f38xx: (),
	#[cfg(feature = "no_vex")]
	#[allow(dead_code)]
	handlers_vex_0f3axx: (),
	#[cfg(feature = "no_evex")]
	#[allow(dead_code)]
	handlers_evex_0fxx: (),
	#[cfg(feature = "no_evex")]
	#[allow(dead_code)]
	handlers_evex_0f38xx: (),
	#[cfg(feature = "no_evex")]
	#[allow(dead_code)]
	handlers_evex_0f3axx: (),
	#[cfg(feature = "no_xop")]
	#[allow(dead_code)]
	handlers_xop8: (),
	#[cfg(feature = "no_xop")]
	#[allow(dead_code)]
	handlers_xop9: (),
	#[cfg(feature = "no_xop")]
	#[allow(dead_code)]
	handlers_xopa: (),
}

lazy_static! {
	pub(super) static ref TABLES: Tables = {
		let handlers_xx = read_legacy();
		#[cfg(not(feature = "no_vex"))]
		let (handlers_vex_0fxx, handlers_vex_0f38xx, handlers_vex_0f3axx) = read_vex();
		#[cfg(not(feature = "no_evex"))]
		let (handlers_evex_0fxx, handlers_evex_0f38xx, handlers_evex_0f3axx) = read_evex();
		#[cfg(not(feature = "no_xop"))]
		let (handlers_xop8, handlers_xop9, handlers_xopa) = read_xop();
		#[cfg(feature = "no_vex")]
		let (handlers_vex_0fxx, handlers_vex_0f38xx, handlers_vex_0f3axx) = ((), (), ());
		#[cfg(feature = "no_evex")]
		let (handlers_evex_0fxx, handlers_evex_0f38xx, handlers_evex_0f3axx) = ((), (), ());
		#[cfg(feature = "no_xop")]
		let (handlers_xop8, handlers_xop9, handlers_xopa) = ((), (), ());
		Tables {
			handlers_xx,
			handlers_vex_0fxx,
			handlers_vex_0f38xx,
			handlers_vex_0f3axx,
			handlers_evex_0fxx,
			handlers_evex_0f38xx,
			handlers_evex_0f3axx,
			handlers_xop8,
			handlers_xop9,
			handlers_xopa,
		}
	};
}
