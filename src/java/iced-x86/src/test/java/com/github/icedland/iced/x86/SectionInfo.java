// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86;

import java.util.function.BiConsumer;

public final class SectionInfo {
	public final String name;
	public final BiConsumer<String, String> handler;
	public SectionInfo(String name, BiConsumer<String, String> handler) {
		this.name = name;
		this.handler = handler;
	}
}
