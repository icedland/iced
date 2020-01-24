/*
Copyright (C) 2018-2019 de4dot@gmail.com

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System;

using Generator.Enums.Formatter;
namespace Generator.Formatters {
	static class FormatterConstants {
		public const string InvalidMnemonicName = "(bad)";
		
		public static string[] GetPseudoOps(PseudoOpsKind kind) {
			switch (kind) {
			case PseudoOpsKind.cmpps:		return cmpps_pseudo_ops;
			case PseudoOpsKind.vcmpps:		return vcmpps_pseudo_ops;
			case PseudoOpsKind.cmppd:		return cmppd_pseudo_ops;
			case PseudoOpsKind.vcmppd:		return vcmppd_pseudo_ops;
			case PseudoOpsKind.cmpss:		return cmpss_pseudo_ops;
			case PseudoOpsKind.vcmpss:		return vcmpss_pseudo_ops;
			case PseudoOpsKind.cmpsd:		return cmpsd_pseudo_ops;
			case PseudoOpsKind.vcmpsd:		return vcmpsd_pseudo_ops;
			case PseudoOpsKind.pclmulqdq:	return pclmulqdq_pseudo_ops;
			case PseudoOpsKind.vpclmulqdq:	return vpclmulqdq_pseudo_ops;
			case PseudoOpsKind.vpcomb:		return vpcomb_pseudo_ops;
			case PseudoOpsKind.vpcomw:		return vpcomw_pseudo_ops;
			case PseudoOpsKind.vpcomd:		return vpcomd_pseudo_ops;
			case PseudoOpsKind.vpcomq:		return vpcomq_pseudo_ops;
			case PseudoOpsKind.vpcomub:		return vpcomub_pseudo_ops;
			case PseudoOpsKind.vpcomuw:		return vpcomuw_pseudo_ops;
			case PseudoOpsKind.vpcomud:		return vpcomud_pseudo_ops;
			case PseudoOpsKind.vpcomuq:		return vpcomuq_pseudo_ops;
			default:						throw new ArgumentOutOfRangeException(nameof(kind));
			}
		}

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

		static string[] Create(string[] cc, int size, string prefix, string suffix) {
			var strings = new string[size];
			for (int i = 0; i < strings.Length; i++)
				strings[i] = prefix + cc[i] + suffix;
			return strings;
		}

		static readonly string[] cmpps_pseudo_ops;
		static readonly string[] vcmpps_pseudo_ops;
		static readonly string[] cmppd_pseudo_ops;
		static readonly string[] vcmppd_pseudo_ops;
		static readonly string[] cmpss_pseudo_ops;
		static readonly string[] vcmpss_pseudo_ops;
		static readonly string[] cmpsd_pseudo_ops;
		static readonly string[] vcmpsd_pseudo_ops;

		static readonly string[] pclmulqdq_pseudo_ops = new string[4] {
			"pclmullqlqdq",
			"pclmulhqlqdq",
			"pclmullqhqdq",
			"pclmulhqhqdq",
		};

		static readonly string[] vpclmulqdq_pseudo_ops = new string[4] {
			"vpclmullqlqdq",
			"vpclmulhqlqdq",
			"vpclmullqhqdq",
			"vpclmulhqhqdq",
		};

		static readonly string[] vpcomb_pseudo_ops;
		static readonly string[] vpcomw_pseudo_ops;
		static readonly string[] vpcomd_pseudo_ops;
		static readonly string[] vpcomq_pseudo_ops;
		static readonly string[] vpcomub_pseudo_ops;
		static readonly string[] vpcomuw_pseudo_ops;
		static readonly string[] vpcomud_pseudo_ops;
		static readonly string[] vpcomuq_pseudo_ops;
	}
}
