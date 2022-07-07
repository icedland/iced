// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.internal.fmt;

import com.github.icedland.iced.x86.fmt.FormatterOptions;

/**
 * DO NOT USE: INTERNAL API
 *
 * @deprecated Not part of the public API
 */
@Deprecated
public final class MnemonicCC {
	public static FormatterString getMnemonicCC(FormatterOptions options, int ccIndex, FormatterString[] mnemonics) {
		int index;
		switch (ccIndex) {
		case 0: // o
			assert mnemonics.length == 1 : mnemonics.length;
			index = 0;
			break;
		case 1: // no
			assert mnemonics.length == 1 : mnemonics.length;
			index = 0;
			break;
		case 2: // b, c, nae
			assert mnemonics.length == 3 : mnemonics.length;
			index = options.getCC_b();
			break;
		case 3: // ae, nb, nc
			assert mnemonics.length == 3 : mnemonics.length;
			index = options.getCC_ae();
			break;
		case 4: // e, z
			assert mnemonics.length == 2 : mnemonics.length;
			index = options.getCC_e();
			break;
		case 5: // ne, nz
			assert mnemonics.length == 2 : mnemonics.length;
			index = options.getCC_ne();
			break;
		case 6: // be, na
			assert mnemonics.length == 2 : mnemonics.length;
			index = options.getCC_be();
			break;
		case 7: // a, nbe
			assert mnemonics.length == 2 : mnemonics.length;
			index = options.getCC_a();
			break;
		case 8: // s
			assert mnemonics.length == 1 : mnemonics.length;
			index = 0;
			break;
		case 9: // ns
			assert mnemonics.length == 1 : mnemonics.length;
			index = 0;
			break;
		case 10: // p, pe
			assert mnemonics.length == 2 : mnemonics.length;
			index = options.getCC_p();
			break;
		case 11: // np, po
			assert mnemonics.length == 2 : mnemonics.length;
			index = options.getCC_np();
			break;
		case 12: // l, nge
			assert mnemonics.length == 2 : mnemonics.length;
			index = options.getCC_l();
			break;
		case 13: // ge, nl
			assert mnemonics.length == 2 : mnemonics.length;
			index = options.getCC_ge();
			break;
		case 14: // le, ng
			assert mnemonics.length == 2 : mnemonics.length;
			index = options.getCC_le();
			break;
		case 15: // g, nle
			assert mnemonics.length == 2 : mnemonics.length;
			index = options.getCC_g();
			break;
		default:
			throw new UnsupportedOperationException();
		}
		assert Integer.compareUnsigned(index, mnemonics.length) < 0;
		return mnemonics[index];
	}
}
