// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

using Generator.Constants;
using Generator.Enums;
using Generator.IO;

namespace Generator.Documentation.Rust {
	sealed class RustDeprecatedWriter : DeprecatedWriter {
		readonly IdentifierConverter idConverter;

		public RustDeprecatedWriter(IdentifierConverter idConverter) =>
			this.idConverter = idConverter;

		public override void WriteDeprecated(FileWriter writer, EnumValue value) {
			if (value.DeprecatedInfo.IsDeprecated) {
				if (value.DeprecatedInfo.NewName is not null) {
					var newValue = value.DeclaringType[value.DeprecatedInfo.NewName];
					WriteDeprecated(writer, value.DeprecatedInfo.VersionStr, newValue.Name(idConverter));
				}
				else
					WriteDeprecated(writer, value.DeprecatedInfo.VersionStr, null);
			}
		}

		public override void WriteDeprecated(FileWriter writer, Constant value) {
			if (value.DeprecatedInfo.IsDeprecated) {
				if (value.DeprecatedInfo.NewName is not null) {
					var newValue = value.DeclaringType[value.DeprecatedInfo.NewName];
					WriteDeprecated(writer, value.DeprecatedInfo.VersionStr, newValue.Name(idConverter));
				}
				else
					WriteDeprecated(writer, value.DeprecatedInfo.VersionStr, null);
			}
		}

		static void WriteDeprecated(FileWriter writer, string version, string? newName) {
			if (newName is not null)
				writer.WriteLine($"#[deprecated(since = \"{version}\", note = \"Use {newName} instead\")]");
			else
				writer.WriteLine($"#[deprecated(since = \"{version}\", note = \"Don't use it!\")]");
		}
	}
}
