// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86;

import java.io.IOException;
import java.nio.charset.StandardCharsets;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.util.ArrayList;
import java.util.List;

public final class FileUtils {
	public static ArrayList<String> readAllLines(String filename) {
		return readAllLines(filename, false);
	}

	public static ArrayList<String> readAllLines(String filename, boolean ignoreEmptyAndCommentLines) {
		ArrayList<String> result = new ArrayList<String>();
		List<String> lines;
		try {
			lines = Files.readAllLines(Paths.get(filename), StandardCharsets.UTF_8);
		}
		catch (IOException ex) {
			throw new UnsupportedOperationException(String.format("Couldn't read `%s`", filename));
		}
		for (String line : lines) {
			if (ignoreEmptyAndCommentLines && (line.length() == 0 || line.charAt(0) == '#'))
				continue;
			result.add(line);
		}
		return result;
	}
}
