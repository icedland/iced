// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if GAS || INTEL || MASM || NASM || FAST_FMT
using System;
using Iced.Intel.FormatterInternal;

namespace Iced.Intel {
	static class FormatterConstants {
		public static FormatterString[] GetPseudoOps(PseudoOpsKind kind) =>
			kind switch {
				PseudoOpsKind.cmpps => cmpps_pseudo_ops,
				PseudoOpsKind.vcmpps => vcmpps_pseudo_ops,
				PseudoOpsKind.cmppd => cmppd_pseudo_ops,
				PseudoOpsKind.vcmppd => vcmppd_pseudo_ops,
				PseudoOpsKind.cmpss => cmpss_pseudo_ops,
				PseudoOpsKind.vcmpss => vcmpss_pseudo_ops,
				PseudoOpsKind.cmpsd => cmpsd_pseudo_ops,
				PseudoOpsKind.vcmpsd => vcmpsd_pseudo_ops,
				PseudoOpsKind.pclmulqdq => pclmulqdq_pseudo_ops,
				PseudoOpsKind.vpclmulqdq => vpclmulqdq_pseudo_ops,
				PseudoOpsKind.vpcomb => vpcomb_pseudo_ops,
				PseudoOpsKind.vpcomw => vpcomw_pseudo_ops,
				PseudoOpsKind.vpcomd => vpcomd_pseudo_ops,
				PseudoOpsKind.vpcomq => vpcomq_pseudo_ops,
				PseudoOpsKind.vpcomub => vpcomub_pseudo_ops,
				PseudoOpsKind.vpcomuw => vpcomuw_pseudo_ops,
				PseudoOpsKind.vpcomud => vpcomud_pseudo_ops,
				PseudoOpsKind.vpcomuq => vpcomuq_pseudo_ops,
				PseudoOpsKind.vpcmpb => vpcmpb_pseudo_ops,
				PseudoOpsKind.vpcmpw => vpcmpw_pseudo_ops,
				PseudoOpsKind.vpcmpd => vpcmpd_pseudo_ops,
				PseudoOpsKind.vpcmpq => vpcmpq_pseudo_ops,
				PseudoOpsKind.vpcmpub => vpcmpub_pseudo_ops,
				PseudoOpsKind.vpcmpuw => vpcmpuw_pseudo_ops,
				PseudoOpsKind.vpcmpud => vpcmpud_pseudo_ops,
				PseudoOpsKind.vpcmpuq => vpcmpuq_pseudo_ops,
				PseudoOpsKind.vcmpph => vcmpph_pseudo_ops,
				PseudoOpsKind.vcmpsh => vcmpsh_pseudo_ops,
				PseudoOpsKind.vcmpps8 => vcmpps8_pseudo_ops,
				PseudoOpsKind.vcmppd8 => vcmppd8_pseudo_ops,
				PseudoOpsKind.vpcmpd6 => vpcmpd6_pseudo_ops,
				PseudoOpsKind.vpcmpud6 => vpcmpud6_pseudo_ops,
				_ => throw new ArgumentOutOfRangeException(nameof(kind)),
			};

		static FormatterConstants() {
			var cc = new string[32] {
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
			cmpps_pseudo_ops = Create(cc, 8, "cmp", "ps");
			vcmpps_pseudo_ops = Create(cc, 32, "vcmp", "ps");
			cmppd_pseudo_ops = Create(cc, 8, "cmp", "pd");
			vcmppd_pseudo_ops = Create(cc, 32, "vcmp", "pd");
			cmpss_pseudo_ops = Create(cc, 8, "cmp", "ss");
			vcmpss_pseudo_ops = Create(cc, 32, "vcmp", "ss");
			cmpsd_pseudo_ops = Create(cc, 8, "cmp", "sd");
			vcmpsd_pseudo_ops = Create(cc, 32, "vcmp", "sd");
			vcmpph_pseudo_ops = Create(cc, 32, "vcmp", "ph");
			vcmpsh_pseudo_ops = Create(cc, 32, "vcmp", "sh");
			vcmpps8_pseudo_ops = Create(cc, 8, "vcmp", "ps");
			vcmppd8_pseudo_ops = Create(cc, 8, "vcmp", "pd");

			var cc6 = new string[8] {
				"eq",
				"lt",
				"le",
				"??",
				"neq",
				"nlt",
				"nle",
				"???",
			};
			vpcmpd6_pseudo_ops = Create(cc6, 8, "vpcmp", "d");
			vpcmpud6_pseudo_ops = Create(cc6, 8, "vpcmp", "ud");

			var xopcc = new string[8] {
				"lt",
				"le",
				"gt",
				"ge",
				"eq",
				"neq",
				"false",
				"true",
			};
			vpcomb_pseudo_ops = Create(xopcc, 8, "vpcom", "b");
			vpcomw_pseudo_ops = Create(xopcc, 8, "vpcom", "w");
			vpcomd_pseudo_ops = Create(xopcc, 8, "vpcom", "d");
			vpcomq_pseudo_ops = Create(xopcc, 8, "vpcom", "q");
			vpcomub_pseudo_ops = Create(xopcc, 8, "vpcom", "ub");
			vpcomuw_pseudo_ops = Create(xopcc, 8, "vpcom", "uw");
			vpcomud_pseudo_ops = Create(xopcc, 8, "vpcom", "ud");
			vpcomuq_pseudo_ops = Create(xopcc, 8, "vpcom", "uq");

			var pcmpcc = new string[8] {
				"eq",
				"lt",
				"le",
				"false",
				"neq",
				"nlt",
				"nle",
				"true",
			};
			vpcmpb_pseudo_ops = Create(pcmpcc, 8, "vpcmp", "b");
			vpcmpw_pseudo_ops = Create(pcmpcc, 8, "vpcmp", "w");
			vpcmpd_pseudo_ops = Create(pcmpcc, 8, "vpcmp", "d");
			vpcmpq_pseudo_ops = Create(pcmpcc, 8, "vpcmp", "q");
			vpcmpub_pseudo_ops = Create(pcmpcc, 8, "vpcmp", "ub");
			vpcmpuw_pseudo_ops = Create(pcmpcc, 8, "vpcmp", "uw");
			vpcmpud_pseudo_ops = Create(pcmpcc, 8, "vpcmp", "ud");
			vpcmpuq_pseudo_ops = Create(pcmpcc, 8, "vpcmp", "uq");
		}

		static FormatterString[] Create(string[] cc, int size, string prefix, string suffix) {
			var strings = new FormatterString[size];
			for (int i = 0; i < strings.Length; i++)
				strings[i] = new FormatterString(prefix + cc[i] + suffix);
			return strings;
		}

		static readonly FormatterString[] cmpps_pseudo_ops;
		static readonly FormatterString[] vcmpps_pseudo_ops;
		static readonly FormatterString[] cmppd_pseudo_ops;
		static readonly FormatterString[] vcmppd_pseudo_ops;
		static readonly FormatterString[] cmpss_pseudo_ops;
		static readonly FormatterString[] vcmpss_pseudo_ops;
		static readonly FormatterString[] cmpsd_pseudo_ops;
		static readonly FormatterString[] vcmpsd_pseudo_ops;
		static readonly FormatterString[] vcmpph_pseudo_ops;
		static readonly FormatterString[] vcmpsh_pseudo_ops;
		static readonly FormatterString[] vcmpps8_pseudo_ops;
		static readonly FormatterString[] vcmppd8_pseudo_ops;
		static readonly FormatterString[] vpcmpd6_pseudo_ops;
		static readonly FormatterString[] vpcmpud6_pseudo_ops;

		static readonly FormatterString[] pclmulqdq_pseudo_ops = new FormatterString[4] {
			new FormatterString("pclmullqlqdq"),
			new FormatterString("pclmulhqlqdq"),
			new FormatterString("pclmullqhqdq"),
			new FormatterString("pclmulhqhqdq"),
		};

		static readonly FormatterString[] vpclmulqdq_pseudo_ops = new FormatterString[4] {
			new FormatterString("vpclmullqlqdq"),
			new FormatterString("vpclmulhqlqdq"),
			new FormatterString("vpclmullqhqdq"),
			new FormatterString("vpclmulhqhqdq"),
		};

		static readonly FormatterString[] vpcomb_pseudo_ops;
		static readonly FormatterString[] vpcomw_pseudo_ops;
		static readonly FormatterString[] vpcomd_pseudo_ops;
		static readonly FormatterString[] vpcomq_pseudo_ops;
		static readonly FormatterString[] vpcomub_pseudo_ops;
		static readonly FormatterString[] vpcomuw_pseudo_ops;
		static readonly FormatterString[] vpcomud_pseudo_ops;
		static readonly FormatterString[] vpcomuq_pseudo_ops;

		static readonly FormatterString[] vpcmpb_pseudo_ops;
		static readonly FormatterString[] vpcmpw_pseudo_ops;
		static readonly FormatterString[] vpcmpd_pseudo_ops;
		static readonly FormatterString[] vpcmpq_pseudo_ops;
		static readonly FormatterString[] vpcmpub_pseudo_ops;
		static readonly FormatterString[] vpcmpuw_pseudo_ops;
		static readonly FormatterString[] vpcmpud_pseudo_ops;
		static readonly FormatterString[] vpcmpuq_pseudo_ops;
	}
}
#endif
