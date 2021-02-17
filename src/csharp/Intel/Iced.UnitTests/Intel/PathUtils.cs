// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System.IO;
using System.Linq;
using System.Reflection;

namespace Iced.UnitTests.Intel {
	static class PathUtils {
		public static string GetTestTextFilename(string filename, params string[] directories) {
			var baseDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", "..", "..", "..", "..", "..", "UnitTests", "Intel");
			baseDir = Path.Combine(new string[] { baseDir }.Concat(directories).ToArray());
			return Path.GetFullPath(Path.Combine(baseDir, filename));
		}
	}
}
