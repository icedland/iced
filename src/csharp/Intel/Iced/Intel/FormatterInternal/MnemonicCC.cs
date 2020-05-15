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
