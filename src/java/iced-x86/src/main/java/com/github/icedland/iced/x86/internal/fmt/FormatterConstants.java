// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.internal.fmt;

/**
 * DO NOT USE: INTERNAL API
 *
 * @deprecated Not part of the public API
 */
@Deprecated
public final class FormatterConstants {
	private FormatterConstants() {
	}

	public static FormatterString[] getPseudoOps(int kind) {
		switch (kind) {
		case PseudoOpsKind.CMPPS:
			return cmpps_pseudo_ops;
		case PseudoOpsKind.VCMPPS:
			return vcmpps_pseudo_ops;
		case PseudoOpsKind.CMPPD:
			return cmppd_pseudo_ops;
		case PseudoOpsKind.VCMPPD:
			return vcmppd_pseudo_ops;
		case PseudoOpsKind.CMPSS:
			return cmpss_pseudo_ops;
		case PseudoOpsKind.VCMPSS:
			return vcmpss_pseudo_ops;
		case PseudoOpsKind.CMPSD:
			return cmpsd_pseudo_ops;
		case PseudoOpsKind.VCMPSD:
			return vcmpsd_pseudo_ops;
		case PseudoOpsKind.PCLMULQDQ:
			return pclmulqdq_pseudo_ops;
		case PseudoOpsKind.VPCLMULQDQ:
			return vpclmulqdq_pseudo_ops;
		case PseudoOpsKind.VPCOMB:
			return vpcomb_pseudo_ops;
		case PseudoOpsKind.VPCOMW:
			return vpcomw_pseudo_ops;
		case PseudoOpsKind.VPCOMD:
			return vpcomd_pseudo_ops;
		case PseudoOpsKind.VPCOMQ:
			return vpcomq_pseudo_ops;
		case PseudoOpsKind.VPCOMUB:
			return vpcomub_pseudo_ops;
		case PseudoOpsKind.VPCOMUW:
			return vpcomuw_pseudo_ops;
		case PseudoOpsKind.VPCOMUD:
			return vpcomud_pseudo_ops;
		case PseudoOpsKind.VPCOMUQ:
			return vpcomuq_pseudo_ops;
		case PseudoOpsKind.VPCMPB:
			return vpcmpb_pseudo_ops;
		case PseudoOpsKind.VPCMPW:
			return vpcmpw_pseudo_ops;
		case PseudoOpsKind.VPCMPD:
			return vpcmpd_pseudo_ops;
		case PseudoOpsKind.VPCMPQ:
			return vpcmpq_pseudo_ops;
		case PseudoOpsKind.VPCMPUB:
			return vpcmpub_pseudo_ops;
		case PseudoOpsKind.VPCMPUW:
			return vpcmpuw_pseudo_ops;
		case PseudoOpsKind.VPCMPUD:
			return vpcmpud_pseudo_ops;
		case PseudoOpsKind.VPCMPUQ:
			return vpcmpuq_pseudo_ops;
		case PseudoOpsKind.VCMPPH:
			return vcmpph_pseudo_ops;
		case PseudoOpsKind.VCMPSH:
			return vcmpsh_pseudo_ops;
		case PseudoOpsKind.VCMPPS8:
			return vcmpps8_pseudo_ops;
		case PseudoOpsKind.VCMPPD8:
			return vcmppd8_pseudo_ops;
		case PseudoOpsKind.VPCMPD6:
			return vpcmpd6_pseudo_ops;
		case PseudoOpsKind.VPCMPUD6:
			return vpcmpud6_pseudo_ops;
		default:
			throw new IllegalArgumentException("kind");
		}
	}

	static {
		String[] cc = new String[] {
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
		};
		cmpps_pseudo_ops = create(cc, 8, "cmp", "ps");
		vcmpps_pseudo_ops = create(cc, 32, "vcmp", "ps");
		cmppd_pseudo_ops = create(cc, 8, "cmp", "pd");
		vcmppd_pseudo_ops = create(cc, 32, "vcmp", "pd");
		cmpss_pseudo_ops = create(cc, 8, "cmp", "ss");
		vcmpss_pseudo_ops = create(cc, 32, "vcmp", "ss");
		cmpsd_pseudo_ops = create(cc, 8, "cmp", "sd");
		vcmpsd_pseudo_ops = create(cc, 32, "vcmp", "sd");
		vcmpph_pseudo_ops = create(cc, 32, "vcmp", "ph");
		vcmpsh_pseudo_ops = create(cc, 32, "vcmp", "sh");
		vcmpps8_pseudo_ops = create(cc, 8, "vcmp", "ps");
		vcmppd8_pseudo_ops = create(cc, 8, "vcmp", "pd");

		String[] cc6 = new String[] {
			"eq",
			"lt",
			"le",
			"??",
			"neq",
			"nlt",
			"nle",
			"???",
		};
		vpcmpd6_pseudo_ops = create(cc6, 8, "vpcmp", "d");
		vpcmpud6_pseudo_ops = create(cc6, 8, "vpcmp", "ud");

		String[] xopcc = new String[] {
			"lt",
			"le",
			"gt",
			"ge",
			"eq",
			"neq",
			"false",
			"true",
		};
		vpcomb_pseudo_ops = create(xopcc, 8, "vpcom", "b");
		vpcomw_pseudo_ops = create(xopcc, 8, "vpcom", "w");
		vpcomd_pseudo_ops = create(xopcc, 8, "vpcom", "d");
		vpcomq_pseudo_ops = create(xopcc, 8, "vpcom", "q");
		vpcomub_pseudo_ops = create(xopcc, 8, "vpcom", "ub");
		vpcomuw_pseudo_ops = create(xopcc, 8, "vpcom", "uw");
		vpcomud_pseudo_ops = create(xopcc, 8, "vpcom", "ud");
		vpcomuq_pseudo_ops = create(xopcc, 8, "vpcom", "uq");

		String[] pcmpcc = new String[] {
			"eq",
			"lt",
			"le",
			"false",
			"neq",
			"nlt",
			"nle",
			"true",
		};
		vpcmpb_pseudo_ops = create(pcmpcc, 8, "vpcmp", "b");
		vpcmpw_pseudo_ops = create(pcmpcc, 8, "vpcmp", "w");
		vpcmpd_pseudo_ops = create(pcmpcc, 8, "vpcmp", "d");
		vpcmpq_pseudo_ops = create(pcmpcc, 8, "vpcmp", "q");
		vpcmpub_pseudo_ops = create(pcmpcc, 8, "vpcmp", "ub");
		vpcmpuw_pseudo_ops = create(pcmpcc, 8, "vpcmp", "uw");
		vpcmpud_pseudo_ops = create(pcmpcc, 8, "vpcmp", "ud");
		vpcmpuq_pseudo_ops = create(pcmpcc, 8, "vpcmp", "uq");
	}

	private static FormatterString[] create(String[] cc, int size, String prefix, String suffix) {
		FormatterString[] strings = new FormatterString[size];
		for (int i = 0; i < strings.length; i++)
			strings[i] = new FormatterString(prefix + cc[i] + suffix);
		return strings;
	}

	private static final FormatterString[] cmpps_pseudo_ops;
	private static final FormatterString[] vcmpps_pseudo_ops;
	private static final FormatterString[] cmppd_pseudo_ops;
	private static final FormatterString[] vcmppd_pseudo_ops;
	private static final FormatterString[] cmpss_pseudo_ops;
	private static final FormatterString[] vcmpss_pseudo_ops;
	private static final FormatterString[] cmpsd_pseudo_ops;
	private static final FormatterString[] vcmpsd_pseudo_ops;
	private static final FormatterString[] vcmpph_pseudo_ops;
	private static final FormatterString[] vcmpsh_pseudo_ops;
	private static final FormatterString[] vcmpps8_pseudo_ops;
	private static final FormatterString[] vcmppd8_pseudo_ops;
	private static final FormatterString[] vpcmpd6_pseudo_ops;
	private static final FormatterString[] vpcmpud6_pseudo_ops;

	private static final FormatterString[] pclmulqdq_pseudo_ops = new FormatterString[] {
		new FormatterString("pclmullqlqdq"),
		new FormatterString("pclmulhqlqdq"),
		new FormatterString("pclmullqhqdq"),
		new FormatterString("pclmulhqhqdq"),
	};

	private static final FormatterString[] vpclmulqdq_pseudo_ops = new FormatterString[] {
		new FormatterString("vpclmullqlqdq"),
		new FormatterString("vpclmulhqlqdq"),
		new FormatterString("vpclmullqhqdq"),
		new FormatterString("vpclmulhqhqdq"),
	};

	private static final FormatterString[] vpcomb_pseudo_ops;
	private static final FormatterString[] vpcomw_pseudo_ops;
	private static final FormatterString[] vpcomd_pseudo_ops;
	private static final FormatterString[] vpcomq_pseudo_ops;
	private static final FormatterString[] vpcomub_pseudo_ops;
	private static final FormatterString[] vpcomuw_pseudo_ops;
	private static final FormatterString[] vpcomud_pseudo_ops;
	private static final FormatterString[] vpcomuq_pseudo_ops;

	private static final FormatterString[] vpcmpb_pseudo_ops;
	private static final FormatterString[] vpcmpw_pseudo_ops;
	private static final FormatterString[] vpcmpd_pseudo_ops;
	private static final FormatterString[] vpcmpq_pseudo_ops;
	private static final FormatterString[] vpcmpub_pseudo_ops;
	private static final FormatterString[] vpcmpuw_pseudo_ops;
	private static final FormatterString[] vpcmpud_pseudo_ops;
	private static final FormatterString[] vpcmpuq_pseudo_ops;
}
