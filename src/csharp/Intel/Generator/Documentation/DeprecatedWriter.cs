// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

using Generator.Constants;
using Generator.Enums;
using Generator.IO;

namespace Generator.Documentation {
	abstract class DeprecatedWriter {
		public abstract void WriteDeprecated(FileWriter writer, EnumValue value);
		public abstract void WriteDeprecated(FileWriter writer, Constant value);
	}
}
