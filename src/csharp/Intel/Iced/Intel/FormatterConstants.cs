// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

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
	}
}
#endif
