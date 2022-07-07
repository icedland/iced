// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt;

import java.util.ArrayList;

import com.github.icedland.iced.x86.FileUtils;
import com.github.icedland.iced.x86.PathUtils;

final class FmtFileUtils {
	public static String getFormatterFilename(String filename) {
		return PathUtils.getTestTextFilename("Formatter", filename + ".txt");
	}

	public static ArrayList<String> readRawStrings(String filename) {
		return FileUtils.readAllLines(getFormatterFilename(filename), true);
	}
}
