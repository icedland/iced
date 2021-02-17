// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if GAS || INTEL || MASM || NASM
using System;
using System.Diagnostics;

namespace Iced.Intel.FormatterInternal {
	static class MnemonicCC {
		public static FormatterString GetMnemonicCC(FormatterOptions options, int ccIndex, FormatterString[] mnemonics) {
			int index;
			switch (ccIndex) {
			case 0: // o
				Debug.Assert(mnemonics.Length == 1);
				index = 0;
				break;
			case 1: // no
				Debug.Assert(mnemonics.Length == 1);
				index = 0;
				break;
			case 2: // b, c, nae
				Debug.Assert(mnemonics.Length == 3);
				index = (int)options.CC_b;
				break;
			case 3: // ae, nb, nc
				Debug.Assert(mnemonics.Length == 3);
				index = (int)options.CC_ae;
				break;
			case 4: // e, z
				Debug.Assert(mnemonics.Length == 2);
				index = (int)options.CC_e;
				break;
			case 5: // ne, nz
				Debug.Assert(mnemonics.Length == 2);
				index = (int)options.CC_ne;
				break;
			case 6: // be, na
				Debug.Assert(mnemonics.Length == 2);
				index = (int)options.CC_be;
				break;
			case 7: // a, nbe
				Debug.Assert(mnemonics.Length == 2);
				index = (int)options.CC_a;
				break;
			case 8: // s
				Debug.Assert(mnemonics.Length == 1);
				index = 0;
				break;
			case 9: // ns
				Debug.Assert(mnemonics.Length == 1);
				index = 0;
				break;
			case 10: // p, pe
				Debug.Assert(mnemonics.Length == 2);
				index = (int)options.CC_p;
				break;
			case 11: // np, po
				Debug.Assert(mnemonics.Length == 2);
				index = (int)options.CC_np;
				break;
			case 12: // l, nge
				Debug.Assert(mnemonics.Length == 2);
				index = (int)options.CC_l;
				break;
			case 13: // ge, nl
				Debug.Assert(mnemonics.Length == 2);
				index = (int)options.CC_ge;
				break;
			case 14: // le, ng
				Debug.Assert(mnemonics.Length == 2);
				index = (int)options.CC_le;
				break;
			case 15: // g, nle
				Debug.Assert(mnemonics.Length == 2);
				index = (int)options.CC_g;
				break;
			default:
				throw new InvalidOperationException();
			}
			Debug.Assert((uint)index < (uint)mnemonics.Length);
			return mnemonics[index];
		}
	}
}
#endif
