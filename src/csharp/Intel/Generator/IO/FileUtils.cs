// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System.IO;
using System.Text;

namespace Generator.IO {
	static class FileUtils {
		public static readonly Encoding FileEncoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);

		public static StreamWriter OpenWrite(string filename) =>
			new(filename, append: false, FileEncoding);
	}
}
