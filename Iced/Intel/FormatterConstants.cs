/*
    Copyright (C) 2018-2019 de4dot@gmail.com

    This file is part of Iced.

    Iced is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Iced is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Iced.  If not, see <https://www.gnu.org/licenses/>.
*/

#if (!NO_GAS_FORMATTER || !NO_INTEL_FORMATTER || !NO_MASM_FORMATTER || !NO_NASM_FORMATTER) && !NO_FORMATTER
namespace Iced.Intel {
	static class FormatterConstants {
		public const string InvalidMnemonicName = "(bad)";

		public static readonly string[] cmpps_pseudo_ops = new string[8] {
			"cmpeqps",
			"cmpltps",
			"cmpleps",
			"cmpunordps",
			"cmpneqps",
			"cmpnltps",
			"cmpnleps",
			"cmpordps",
		};

		public static readonly string[] vcmpps_pseudo_ops = new string[32] {
			"vcmpeqps",
			"vcmpltps",
			"vcmpleps",
			"vcmpunordps",
			"vcmpneqps",
			"vcmpnltps",
			"vcmpnleps",
			"vcmpordps",
			"vcmpeq_uqps",
			"vcmpngeps",
			"vcmpngtps",
			"vcmpfalseps",
			"vcmpneq_oqps",
			"vcmpgeps",
			"vcmpgtps",
			"vcmptrueps",
			"vcmpeq_osps",
			"vcmplt_oqps",
			"vcmple_oqps",
			"vcmpunord_sps",
			"vcmpneq_usps",
			"vcmpnlt_uqps",
			"vcmpnle_uqps",
			"vcmpord_sps",
			"vcmpeq_usps",
			"vcmpnge_uqps",
			"vcmpngt_uqps",
			"vcmpfalse_osps",
			"vcmpneq_osps",
			"vcmpge_oqps",
			"vcmpgt_oqps",
			"vcmptrue_usps",
		};

		public static readonly string[] cmppd_pseudo_ops = new string[8] {
			"cmpeqpd",
			"cmpltpd",
			"cmplepd",
			"cmpunordpd",
			"cmpneqpd",
			"cmpnltpd",
			"cmpnlepd",
			"cmpordpd",
		};

		public static readonly string[] vcmppd_pseudo_ops = new string[32] {
			"vcmpeqpd",
			"vcmpltpd",
			"vcmplepd",
			"vcmpunordpd",
			"vcmpneqpd",
			"vcmpnltpd",
			"vcmpnlepd",
			"vcmpordpd",
			"vcmpeq_uqpd",
			"vcmpngepd",
			"vcmpngtpd",
			"vcmpfalsepd",
			"vcmpneq_oqpd",
			"vcmpgepd",
			"vcmpgtpd",
			"vcmptruepd",
			"vcmpeq_ospd",
			"vcmplt_oqpd",
			"vcmple_oqpd",
			"vcmpunord_spd",
			"vcmpneq_uspd",
			"vcmpnlt_uqpd",
			"vcmpnle_uqpd",
			"vcmpord_spd",
			"vcmpeq_uspd",
			"vcmpnge_uqpd",
			"vcmpngt_uqpd",
			"vcmpfalse_ospd",
			"vcmpneq_ospd",
			"vcmpge_oqpd",
			"vcmpgt_oqpd",
			"vcmptrue_uspd",
		};

		public static readonly string[] cmpss_pseudo_ops = new string[8] {
			"cmpeqss",
			"cmpltss",
			"cmpless",
			"cmpunordss",
			"cmpneqss",
			"cmpnltss",
			"cmpnless",
			"cmpordss",
		};

		public static readonly string[] vcmpss_pseudo_ops = new string[32] {
			"vcmpeqss",
			"vcmpltss",
			"vcmpless",
			"vcmpunordss",
			"vcmpneqss",
			"vcmpnltss",
			"vcmpnless",
			"vcmpordss",
			"vcmpeq_uqss",
			"vcmpngess",
			"vcmpngtss",
			"vcmpfalsess",
			"vcmpneq_oqss",
			"vcmpgess",
			"vcmpgtss",
			"vcmptruess",
			"vcmpeq_osss",
			"vcmplt_oqss",
			"vcmple_oqss",
			"vcmpunord_sss",
			"vcmpneq_usss",
			"vcmpnlt_uqss",
			"vcmpnle_uqss",
			"vcmpord_sss",
			"vcmpeq_usss",
			"vcmpnge_uqss",
			"vcmpngt_uqss",
			"vcmpfalse_osss",
			"vcmpneq_osss",
			"vcmpge_oqss",
			"vcmpgt_oqss",
			"vcmptrue_usss",
		};

		public static readonly string[] cmpsd_pseudo_ops = new string[8] {
			"cmpeqsd",
			"cmpltsd",
			"cmplesd",
			"cmpunordsd",
			"cmpneqsd",
			"cmpnltsd",
			"cmpnlesd",
			"cmpordsd",
		};

		public static readonly string[] vcmpsd_pseudo_ops = new string[32] {
			"vcmpeqsd",
			"vcmpltsd",
			"vcmplesd",
			"vcmpunordsd",
			"vcmpneqsd",
			"vcmpnltsd",
			"vcmpnlesd",
			"vcmpordsd",
			"vcmpeq_uqsd",
			"vcmpngesd",
			"vcmpngtsd",
			"vcmpfalsesd",
			"vcmpneq_oqsd",
			"vcmpgesd",
			"vcmpgtsd",
			"vcmptruesd",
			"vcmpeq_ossd",
			"vcmplt_oqsd",
			"vcmple_oqsd",
			"vcmpunord_ssd",
			"vcmpneq_ussd",
			"vcmpnlt_uqsd",
			"vcmpnle_uqsd",
			"vcmpord_ssd",
			"vcmpeq_ussd",
			"vcmpnge_uqsd",
			"vcmpngt_uqsd",
			"vcmpfalse_ossd",
			"vcmpneq_ossd",
			"vcmpge_oqsd",
			"vcmpgt_oqsd",
			"vcmptrue_ussd",
		};

		public static readonly string[] pclmulqdq_pseudo_ops = new string[4] {
			"pclmullqlqdq",
			"pclmulhqlqdq",
			"pclmullqhqdq",
			"pclmulhqhqdq",
		};

		public static readonly string[] vpclmulqdq_pseudo_ops = new string[4] {
			"vpclmullqlqdq",
			"vpclmulhqlqdq",
			"vpclmullqhqdq",
			"vpclmulhqhqdq",
		};

		public static readonly string[] vpcomb_pseudo_ops = new string[8] {
			"vpcomltb",
			"vpcomleb",
			"vpcomgtb",
			"vpcomgeb",
			"vpcomeqb",
			"vpcomneqb",
			"vpcomfalseb",
			"vpcomtrueb",
		};

		public static readonly string[] vpcomw_pseudo_ops = new string[8] {
			"vpcomltw",
			"vpcomlew",
			"vpcomgtw",
			"vpcomgew",
			"vpcomeqw",
			"vpcomneqw",
			"vpcomfalsew",
			"vpcomtruew",
		};

		public static readonly string[] vpcomd_pseudo_ops = new string[8] {
			"vpcomltd",
			"vpcomled",
			"vpcomgtd",
			"vpcomged",
			"vpcomeqd",
			"vpcomneqd",
			"vpcomfalsed",
			"vpcomtrued",
		};

		public static readonly string[] vpcomq_pseudo_ops = new string[8] {
			"vpcomltq",
			"vpcomleq",
			"vpcomgtq",
			"vpcomgeq",
			"vpcomeqq",
			"vpcomneqq",
			"vpcomfalseq",
			"vpcomtrueq",
		};

		public static readonly string[] vpcomub_pseudo_ops = new string[8] {
			"vpcomltub",
			"vpcomleub",
			"vpcomgtub",
			"vpcomgeub",
			"vpcomequb",
			"vpcomnequb",
			"vpcomfalseub",
			"vpcomtrueub",
		};

		public static readonly string[] vpcomuw_pseudo_ops = new string[8] {
			"vpcomltuw",
			"vpcomleuw",
			"vpcomgtuw",
			"vpcomgeuw",
			"vpcomequw",
			"vpcomnequw",
			"vpcomfalseuw",
			"vpcomtrueuw",
		};

		public static readonly string[] vpcomud_pseudo_ops = new string[8] {
			"vpcomltud",
			"vpcomleud",
			"vpcomgtud",
			"vpcomgeud",
			"vpcomequd",
			"vpcomnequd",
			"vpcomfalseud",
			"vpcomtrueud",
		};

		public static readonly string[] vpcomuq_pseudo_ops = new string[8] {
			"vpcomltuq",
			"vpcomleuq",
			"vpcomgtuq",
			"vpcomgeuq",
			"vpcomequq",
			"vpcomnequq",
			"vpcomfalseuq",
			"vpcomtrueuq",
		};
	}
}
#endif
