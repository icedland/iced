// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86;

import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.ArrayList;
import java.util.Arrays;

public final class PathUtils {
	private static Path rootDir = getIcedRootDir();

	private static Path getIcedRootDir() {
		final String ENV_ROOT_NAME = "ICED_REPO_ROOT_DIR";
		String envRootDir = System.getenv(ENV_ROOT_NAME);
		Path rootDir = null;
		if (envRootDir != null)
			rootDir = Paths.get(envRootDir).toAbsolutePath();
		else {
			// This code assumes the dir is "<repo-root>/src/java/iced-x86"
			String userDir = System.getProperty("user.dir");
			if (userDir != null) {
				Path path = Paths.get(userDir);
				rootDir = path.toAbsolutePath().getParent().getParent().getParent();
			}
		}

		if (rootDir != null && rootDir.resolve("LICENSE.txt").toFile().exists())
			return rootDir;

		throw new UnsupportedOperationException(String.format("Couldn't find the iced repo root dir. Try setting %s env var to the root of the repo", ENV_ROOT_NAME));
	}

	public static String getTestTextFilename(String... items) {
		ArrayList<String> dirs = new ArrayList<String>();
		dirs.addAll(Arrays.asList("src", "UnitTests", "Intel"));
		dirs.addAll(Arrays.asList(items));
		Path path = Paths.get(rootDir.toString(), dirs.toArray(new String[dirs.size()]));
		String result = path.toAbsolutePath().normalize().toString();
		return result;
	}
}
