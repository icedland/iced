// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

// Keep this file in sync with pseudo_ops.rs

use crate::formatter::enums_shared::PseudoOpsKind;
use crate::formatter::fast::FastStringMnemonic;
use alloc::boxed::Box;
use alloc::vec::Vec;
use lazy_static::lazy_static;
use static_assertions::const_assert;

// Copied from fast.rs since it doesn't seem to be possible to use it from this module even with #[macro_use]
macro_rules! mk_const_fast_str {
	($fast_ty:tt, $str:literal) => {{
		const STR: &str = $str;
		const_assert!(STR.len() == 1 + <$fast_ty>::SIZE);
		const_assert!(STR.as_bytes()[0] as usize <= <$fast_ty>::SIZE);
		$fast_ty { len_data: STR.as_ptr() }
	}};
}

pub(super) fn get_pseudo_ops(kind: PseudoOpsKind) -> &'static Vec<FastStringMnemonic> {
	let pseudo_ops = &*PSEUDO_OPS;
	match kind {
		PseudoOpsKind::cmpps => &pseudo_ops.cmpps,
		PseudoOpsKind::vcmpps => &pseudo_ops.vcmpps,
		PseudoOpsKind::cmppd => &pseudo_ops.cmppd,
		PseudoOpsKind::vcmppd => &pseudo_ops.vcmppd,
		PseudoOpsKind::cmpss => &pseudo_ops.cmpss,
		PseudoOpsKind::vcmpss => &pseudo_ops.vcmpss,
		PseudoOpsKind::cmpsd => &pseudo_ops.cmpsd,
		PseudoOpsKind::vcmpsd => &pseudo_ops.vcmpsd,
		PseudoOpsKind::pclmulqdq => &pseudo_ops.pclmulqdq,
		PseudoOpsKind::vpclmulqdq => &pseudo_ops.vpclmulqdq,
		PseudoOpsKind::vpcomb => &pseudo_ops.vpcomb,
		PseudoOpsKind::vpcomw => &pseudo_ops.vpcomw,
		PseudoOpsKind::vpcomd => &pseudo_ops.vpcomd,
		PseudoOpsKind::vpcomq => &pseudo_ops.vpcomq,
		PseudoOpsKind::vpcomub => &pseudo_ops.vpcomub,
		PseudoOpsKind::vpcomuw => &pseudo_ops.vpcomuw,
		PseudoOpsKind::vpcomud => &pseudo_ops.vpcomud,
		PseudoOpsKind::vpcomuq => &pseudo_ops.vpcomuq,
	}
}

struct PseudoOps {
	cmpps: Vec<FastStringMnemonic>,
	vcmpps: Vec<FastStringMnemonic>,
	cmppd: Vec<FastStringMnemonic>,
	vcmppd: Vec<FastStringMnemonic>,
	cmpss: Vec<FastStringMnemonic>,
	vcmpss: Vec<FastStringMnemonic>,
	cmpsd: Vec<FastStringMnemonic>,
	vcmpsd: Vec<FastStringMnemonic>,
	pclmulqdq: Vec<FastStringMnemonic>,
	vpclmulqdq: Vec<FastStringMnemonic>,
	vpcomb: Vec<FastStringMnemonic>,
	vpcomw: Vec<FastStringMnemonic>,
	vpcomd: Vec<FastStringMnemonic>,
	vpcomq: Vec<FastStringMnemonic>,
	vpcomub: Vec<FastStringMnemonic>,
	vpcomuw: Vec<FastStringMnemonic>,
	vpcomud: Vec<FastStringMnemonic>,
	vpcomuq: Vec<FastStringMnemonic>,
}

lazy_static! {
	static ref PSEUDO_OPS: PseudoOps = {
		#[rustfmt::skip]
		let cc: [&'static str; 32] = [
			"eq",
			"lt",
			"le",
			"unord",
			"neq",
			"nlt",
			"nle",
			"ord",
			"eq_uq",
			"nge",
			"ngt",
			"false",
			"neq_oq",
			"ge",
			"gt",
			"true",
			"eq_os",
			"lt_oq",
			"le_oq",
			"unord_s",
			"neq_us",
			"nlt_uq",
			"nle_uq",
			"ord_s",
			"eq_us",
			"nge_uq",
			"ngt_uq",
			"false_os",
			"neq_os",
			"ge_oq",
			"gt_oq",
			"true_us",
		];
		let cmpps = create(&cc, 8, "cmp", "ps");
		let vcmpps = create(&cc, 32, "vcmp", "ps");
		let cmppd = create(&cc, 8, "cmp", "pd");
		let vcmppd = create(&cc, 32, "vcmp", "pd");
		let cmpss = create(&cc, 8, "cmp", "ss");
		let vcmpss = create(&cc, 32, "vcmp", "ss");
		let cmpsd = create(&cc, 8, "cmp", "sd");
		let vcmpsd = create(&cc, 32, "vcmp", "sd");

		#[rustfmt::skip]
		let xopcc: [&'static str; 8] = [
			"lt",
			"le",
			"gt",
			"ge",
			"eq",
			"neq",
			"false",
			"true",
		];
		let vpcomb = create(&xopcc, 8, "vpcom", "b");
		let vpcomw = create(&xopcc, 8, "vpcom", "w");
		let vpcomd = create(&xopcc, 8, "vpcom", "d");
		let vpcomq = create(&xopcc, 8, "vpcom", "q");
		let vpcomub = create(&xopcc, 8, "vpcom", "ub");
		let vpcomuw = create(&xopcc, 8, "vpcom", "uw");
		let vpcomud = create(&xopcc, 8, "vpcom", "ud");
		let vpcomuq = create(&xopcc, 8, "vpcom", "uq");

		#[rustfmt::skip]
		let pclmulqdq = vec![
			mk_const_fast_str!(FastStringMnemonic, "\x0Cpclmullqlqdq        "),
			mk_const_fast_str!(FastStringMnemonic, "\x0Cpclmulhqlqdq        "),
			mk_const_fast_str!(FastStringMnemonic, "\x0Cpclmullqhqdq        "),
			mk_const_fast_str!(FastStringMnemonic, "\x0Cpclmulhqhqdq        "),
		];
		#[rustfmt::skip]
		let vpclmulqdq = vec![
			mk_const_fast_str!(FastStringMnemonic, "\x0Dvpclmullqlqdq       "),
			mk_const_fast_str!(FastStringMnemonic, "\x0Dvpclmulhqlqdq       "),
			mk_const_fast_str!(FastStringMnemonic, "\x0Dvpclmullqhqdq       "),
			mk_const_fast_str!(FastStringMnemonic, "\x0Dvpclmulhqhqdq       "),
		];

		PseudoOps {
			cmpps,
			vcmpps,
			cmppd,
			vcmppd,
			cmpss,
			vcmpss,
			cmpsd,
			vcmpsd,
			pclmulqdq,
			vpclmulqdq,
			vpcomb,
			vpcomw,
			vpcomd,
			vpcomq,
			vpcomub,
			vpcomuw,
			vpcomud,
			vpcomuq,
		}
	};
}

#[allow(clippy::char_lit_as_u8)]
fn create(cc: &[&str], size: usize, prefix: &str, suffix: &str) -> Vec<FastStringMnemonic> {
	let mut strings = Vec::with_capacity(size);
	for &cc_s in cc {
		if strings.len() == size {
			break;
		}
		let mut new_vec = Vec::with_capacity(1 + FastStringMnemonic::SIZE);
		let new_len = prefix.len() + cc_s.len() + suffix.len();
		debug_assert!(new_len <= FastStringMnemonic::SIZE);
		new_vec.push(new_len as u8);
		new_vec.extend_from_slice(prefix.as_bytes());
		new_vec.extend_from_slice(cc_s.as_bytes());
		new_vec.extend_from_slice(suffix.as_bytes());
		new_vec.extend(core::iter::repeat(' ' as u8).take(FastStringMnemonic::SIZE - (new_vec.len() - 1)));
		debug_assert_eq!(new_vec.len(), 1 + FastStringMnemonic::SIZE);
		let len_data = Box::leak(Box::new(new_vec)).as_ptr();
		strings.push(FastStringMnemonic::new(len_data));
	}
	strings
}
