// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.internal;

/**
 * DO NOT USE: INTERNAL API
 *
 * @deprecated Not part of the public API
 */
@Deprecated
public final class Constants {
	// Some code store a Register in a byte so if this changes, those fields
	// should be changed to a short and this constant should be removed. Also
	// all code that returns those fields 'return blah & 0xFF' should be changed
	// to 'return blah' or possibly 'return blah & 0xFFFF'
	public static final int REG_MASK = 0xFF;
}
