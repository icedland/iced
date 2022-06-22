// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.dec;

import java.io.IOException;
import java.nio.charset.StandardCharsets;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.util.HashSet;
import java.util.List;

import com.github.icedland.iced.x86.CodeUtils;
import com.github.icedland.iced.x86.PathUtils;
import com.github.icedland.iced.x86.ToCode;

final class CodeValueReader {
	public static HashSet<Integer> read(String name) {
		String filename = PathUtils.getTestTextFilename("Decoder", name);
		int lineNumber = 0;
		List<String> lines;
		try {
			lines = Files.readAllLines(Paths.get(filename), StandardCharsets.UTF_8);
		}
		catch (IOException ex) {
			throw new UnsupportedOperationException(String.format("Couldn't read `%s`", filename));
		}
		HashSet<Integer> hash = new HashSet<Integer>();
		for (String line : lines) {
			lineNumber++;
			if (line.length() == 0 || line.charAt(0) == '#')
				continue;
			String value = line.trim();
			if (CodeUtils.isIgnored(value))
				continue;
			Integer code = ToCode.tryGet(value);
			if (code == null)
				throw new UnsupportedOperationException(
						String.format("Error parsing Code file '%s', line %d: Invalid value: %s", filename, lineNumber, value));
			hash.add(code);
		}
		return hash;
	}
}
