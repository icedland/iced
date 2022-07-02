// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86;

import java.util.ArrayList;
import java.util.function.BiConsumer;

public final class SectionFileReader {
	public static void read(String filename, SectionInfo[] sectionInfos) {
		SectionInfo currentSectionInfo = null;
		ArrayList<String> lines = FileUtils.readAllLines(filename);
		int lineNo = 0;
		for (String line : lines) {
			lineNo++;
			if (line.length() == 0 || line.charAt(0) == '#')
				continue;

			try {
				if (line.startsWith("[")) {
					if (!line.endsWith("]"))
						throw new UnsupportedOperationException("Missing ']'");
					String sectionName = line.substring(1, line.length() - 1).trim();
					currentSectionInfo = tryGetSection(sectionInfos, sectionName);
					if (currentSectionInfo == null)
						throw new UnsupportedOperationException(String.format("Unknown section name: %s", sectionName));
				}
				else {
					BiConsumer<String, String> handler = currentSectionInfo.handler;
					handler.accept(currentSectionInfo.name, line);
				}
			}
			catch (Exception ex) {
				throw new UnsupportedOperationException(String.format("Invalid line %d (%s): %s", lineNo, filename, ex.getMessage()));
			}
		}
	}

	static SectionInfo tryGetSection(SectionInfo[] sectionInfos, String sectionName) {
		for (SectionInfo info : sectionInfos) {
			if (info.name.equals(sectionName))
				return info;
		}
		return null;
	}
}
