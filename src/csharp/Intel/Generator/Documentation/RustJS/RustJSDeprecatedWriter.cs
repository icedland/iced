// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

using Generator.Constants;
using Generator.Enums;
using Generator.IO;

namespace Generator.Documentation.RustJS {
	sealed class RustJSDeprecatedWriter : DeprecatedWriter {
		readonly IdentifierConverter idConverter;

		public RustJSDeprecatedWriter(IdentifierConverter idConverter) =>
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
			writer.WriteLine("///");
			writer.WriteLine("/// ***************************************************");
			if (newName is not null)
				writer.WriteLine($"/// DEPRECATED since {version}: Use {newName} instead");
			else
				writer.WriteLine($"/// DEPRECATED since {version}: Don't use it!");
		}
	}
}
