// SPDX-License-Identifier: MIT
// Copyright wtfsck@protonmail.com
// Copyright iced contributors

using System;

using Generator.Enums.Formatter;
namespace Generator.Formatters {
	static class FormatterConstants {
		public static (string mnemonic, int imm)[] GetPseudoOps(PseudoOpsKind kind) =>
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

		static (string mnemonic, int imm)[] Create(string[] cc, int size, string prefix, string suffix) {
			var values = new (string mnemonic, int imm)[size];
			for (int i = 0; i < values.Length; i++)
				values[i] = (prefix + cc[i] + suffix, i);
			return values;
		}

		static readonly (string mnemonic, int imm)[] cmpps_pseudo_ops;
		static readonly (string mnemonic, int imm)[] vcmpps_pseudo_ops;
		static readonly (string mnemonic, int imm)[] cmppd_pseudo_ops;
		static readonly (string mnemonic, int imm)[] vcmppd_pseudo_ops;
		static readonly (string mnemonic, int imm)[] cmpss_pseudo_ops;
		static readonly (string mnemonic, int imm)[] vcmpss_pseudo_ops;
		static readonly (string mnemonic, int imm)[] cmpsd_pseudo_ops;
		static readonly (string mnemonic, int imm)[] vcmpsd_pseudo_ops;

		static readonly (string mnemonic, int imm)[] pclmulqdq_pseudo_ops = new (string mnemonic, int imm)[4] {
			("pclmullqlqdq", 0x00),
			("pclmulhqlqdq", 0x01),
			("pclmullqhqdq", 0x10),
			("pclmulhqhqdq", 0x11),
		};

		static readonly (string mnemonic, int imm)[] vpclmulqdq_pseudo_ops = new (string mnemonic, int imm)[4] {
			("vpclmullqlqdq", 0x00),
			("vpclmulhqlqdq", 0x01),
			("vpclmullqhqdq", 0x10),
			("vpclmulhqhqdq", 0x11),
		};

		static readonly (string mnemonic, int imm)[] vpcomb_pseudo_ops;
		static readonly (string mnemonic, int imm)[] vpcomw_pseudo_ops;
		static readonly (string mnemonic, int imm)[] vpcomd_pseudo_ops;
		static readonly (string mnemonic, int imm)[] vpcomq_pseudo_ops;
		static readonly (string mnemonic, int imm)[] vpcomub_pseudo_ops;
		static readonly (string mnemonic, int imm)[] vpcomuw_pseudo_ops;
		static readonly (string mnemonic, int imm)[] vpcomud_pseudo_ops;
		static readonly (string mnemonic, int imm)[] vpcomuq_pseudo_ops;
	}
}
