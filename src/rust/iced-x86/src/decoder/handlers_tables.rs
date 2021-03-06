// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::decoder::handlers::{OpCodeHandler, OpCodeHandlerDecodeFn};
use crate::decoder::table_de::*;
use alloc::vec::Vec;
use lazy_static::lazy_static;

pub(super) struct Tables {
	#[cfg(not(feature = "no_evex"))]
	pub(super) invalid_map: Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
	#[cfg(feature = "no_evex")]
	#[allow(dead_code)]
	invalid_map: (),

	pub(super) handlers_map0: Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
	#[cfg(not(feature = "no_vex"))]
	pub(super) handlers_vex_0f: Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
	#[cfg(not(feature = "no_vex"))]
	pub(super) handlers_vex_0f38: Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
	#[cfg(not(feature = "no_vex"))]
	pub(super) handlers_vex_0f3a: Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
	#[cfg(not(feature = "no_evex"))]
	pub(super) handlers_evex_0f: Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
	#[cfg(not(feature = "no_evex"))]
	pub(super) handlers_evex_0f38: Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
	#[cfg(not(feature = "no_evex"))]
	pub(super) handlers_evex_0f3a: Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
	#[cfg(not(feature = "no_evex"))]
	pub(super) handlers_evex_map5: Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
	#[cfg(not(feature = "no_evex"))]
	pub(super) handlers_evex_map6: Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
	#[cfg(not(feature = "no_xop"))]
	pub(super) handlers_xop_map8: Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
	#[cfg(not(feature = "no_xop"))]
	pub(super) handlers_xop_map9: Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
	#[cfg(not(feature = "no_xop"))]
	pub(super) handlers_xop_map10: Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
	#[cfg(feature = "no_vex")]
	#[allow(dead_code)]
	handlers_vex_0f: (),
	#[cfg(feature = "no_vex")]
	#[allow(dead_code)]
	handlers_vex_0f38: (),
	#[cfg(feature = "no_vex")]
	#[allow(dead_code)]
	handlers_vex_0f3a: (),
	#[cfg(feature = "no_evex")]
	#[allow(dead_code)]
	handlers_evex_0f: (),
	#[cfg(feature = "no_evex")]
	#[allow(dead_code)]
	handlers_evex_0f38: (),
	#[cfg(feature = "no_evex")]
	#[allow(dead_code)]
	handlers_evex_0f3a: (),
	#[cfg(feature = "no_evex")]
	#[allow(dead_code)]
	handlers_evex_map5: (),
	#[cfg(feature = "no_evex")]
	#[allow(dead_code)]
	handlers_evex_map6: (),
	#[cfg(feature = "no_xop")]
	#[allow(dead_code)]
	handlers_xop_map8: (),
	#[cfg(feature = "no_xop")]
	#[allow(dead_code)]
	handlers_xop_map9: (),
	#[cfg(feature = "no_xop")]
	#[allow(dead_code)]
	handlers_xop_map10: (),
}

lazy_static! {
	pub(super) static ref TABLES: Tables = {
		let handlers_map0 = read_legacy();
		#[cfg(not(feature = "no_vex"))]
		let (handlers_vex_0f, handlers_vex_0f38, handlers_vex_0f3a) = read_vex();
		#[cfg(not(feature = "no_evex"))]
		let (handlers_evex_0f, handlers_evex_0f38, handlers_evex_0f3a, handlers_evex_map5, handlers_evex_map6) = read_evex();
		#[cfg(not(feature = "no_xop"))]
		let (handlers_xop_map8, handlers_xop_map9, handlers_xop_map10) = read_xop();
		#[cfg(feature = "no_vex")]
		let (handlers_vex_0f, handlers_vex_0f38, handlers_vex_0f3a) = ((), (), ());
		#[cfg(feature = "no_evex")]
		let (handlers_evex_0f, handlers_evex_0f38, handlers_evex_0f3a, handlers_evex_map5, handlers_evex_map6) = ((), (), (), (), ());
		#[cfg(feature = "no_xop")]
		let (handlers_xop_map8, handlers_xop_map9, handlers_xop_map10) = ((), (), ());

		#[cfg(not(feature = "no_evex"))]
		let invalid_map = core::iter::repeat(super::handlers::get_invalid_handler()).take(0x100).collect();
		#[cfg(feature = "no_evex")]
		let invalid_map = ();
		Tables {
			invalid_map,
			handlers_map0,
			handlers_vex_0f,
			handlers_vex_0f38,
			handlers_vex_0f3a,
			handlers_evex_0f,
			handlers_evex_0f38,
			handlers_evex_0f3a,
			handlers_evex_map5,
			handlers_evex_map6,
			handlers_xop_map8,
			handlers_xop_map9,
			handlers_xop_map10,
		}
	};
}
