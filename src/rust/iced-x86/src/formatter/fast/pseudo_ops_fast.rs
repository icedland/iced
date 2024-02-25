// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

// Keep this file in sync with pseudo_ops.rs

use crate::formatter::enums_shared::PseudoOpsKind;
use crate::formatter::fast::FastStringMnemonic;
use alloc::vec::Vec;
use lazy_static::lazy_static;

use super::mk_const_fast_str;

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
		PseudoOpsKind::vpcmpb => &pseudo_ops.vpcmpb,
		PseudoOpsKind::vpcmpw => &pseudo_ops.vpcmpw,
		PseudoOpsKind::vpcmpd => &pseudo_ops.vpcmpd,
		PseudoOpsKind::vpcmpq => &pseudo_ops.vpcmpq,
		PseudoOpsKind::vpcmpub => &pseudo_ops.vpcmpub,
		PseudoOpsKind::vpcmpuw => &pseudo_ops.vpcmpuw,
		PseudoOpsKind::vpcmpud => &pseudo_ops.vpcmpud,
		PseudoOpsKind::vpcmpuq => &pseudo_ops.vpcmpuq,
		PseudoOpsKind::vcmpph => &pseudo_ops.vcmpph,
		PseudoOpsKind::vcmpsh => &pseudo_ops.vcmpsh,
		PseudoOpsKind::vcmpps8 => &pseudo_ops.vcmpps8,
		PseudoOpsKind::vcmppd8 => &pseudo_ops.vcmppd8,
		PseudoOpsKind::vpcmpd6 => &pseudo_ops.vpcmpd6,
		PseudoOpsKind::vpcmpud6 => &pseudo_ops.vpcmpud6,
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
	vpcmpb: Vec<FastStringMnemonic>,
	vpcmpw: Vec<FastStringMnemonic>,
	vpcmpd: Vec<FastStringMnemonic>,
	vpcmpq: Vec<FastStringMnemonic>,
	vpcmpub: Vec<FastStringMnemonic>,
	vpcmpuw: Vec<FastStringMnemonic>,
	vpcmpud: Vec<FastStringMnemonic>,
	vpcmpuq: Vec<FastStringMnemonic>,
	vcmpph: Vec<FastStringMnemonic>,
	vcmpsh: Vec<FastStringMnemonic>,
	vcmpps8: Vec<FastStringMnemonic>,
	vcmppd8: Vec<FastStringMnemonic>,
	vpcmpd6: Vec<FastStringMnemonic>,
	vpcmpud6: Vec<FastStringMnemonic>,
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
		let vcmpph = create(&cc, 32, "vcmp", "ph");
		let vcmpsh = create(&cc, 32, "vcmp", "sh");
		let vcmpps8 = create(&cc, 8, "vcmp", "ps");
		let vcmppd8 = create(&cc, 8, "vcmp", "pd");

		#[rustfmt::skip]
		let cc6: [&'static str; 8] = [
			"eq",
			"lt",
			"le",
			"??",
			"neq",
			"nlt",
			"nle",
			"???",
		];
		let vpcmpd6 = create(&cc6, 8, "vpcmp", "d");
		let vpcmpud6 = create(&cc6, 8, "vpcmp", "ud");

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
			mk_const_fast_str!(FastStringMnemonic, "pclmullqlqdq"),
			mk_const_fast_str!(FastStringMnemonic, "pclmulhqlqdq"),
			mk_const_fast_str!(FastStringMnemonic, "pclmullqhqdq"),
			mk_const_fast_str!(FastStringMnemonic, "pclmulhqhqdq"),
		];
		#[rustfmt::skip]
		let vpclmulqdq = vec![
			mk_const_fast_str!(FastStringMnemonic, "vpclmullqlqdq"),
			mk_const_fast_str!(FastStringMnemonic, "vpclmulhqlqdq"),
			mk_const_fast_str!(FastStringMnemonic, "vpclmullqhqdq"),
			mk_const_fast_str!(FastStringMnemonic, "vpclmulhqhqdq"),
		];

		#[rustfmt::skip]
		let pcmpcc: [&'static str; 8] = [
			"eq",
			"lt",
			"le",
			"false",
			"neq",
			"nlt",
			"nle",
			"true",
		];
		let vpcmpb = create(&pcmpcc, 8, "vpcmp", "b");
		let vpcmpw = create(&pcmpcc, 8, "vpcmp", "w");
		let vpcmpd = create(&pcmpcc, 8, "vpcmp", "d");
		let vpcmpq = create(&pcmpcc, 8, "vpcmp", "q");
		let vpcmpub = create(&pcmpcc, 8, "vpcmp", "ub");
		let vpcmpuw = create(&pcmpcc, 8, "vpcmp", "uw");
		let vpcmpud = create(&pcmpcc, 8, "vpcmp", "ud");
		let vpcmpuq = create(&pcmpcc, 8, "vpcmp", "uq");

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
			vpcmpb,
			vpcmpw,
			vpcmpd,
			vpcmpq,
			vpcmpub,
			vpcmpuw,
			vpcmpud,
			vpcmpuq,
			vcmpph,
			vcmpsh,
			vcmpps8,
			vcmppd8,
			vpcmpd6,
			vpcmpud6,
		}
	};
}

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
		new_vec.extend(core::iter::repeat(b' ').take(FastStringMnemonic::SIZE - (new_vec.len() - 1)));
		debug_assert_eq!(new_vec.len(), 1 + FastStringMnemonic::SIZE);
		let len_data = new_vec.leak();
		strings.push(FastStringMnemonic::from_raw(len_data));
	}
	strings
}
