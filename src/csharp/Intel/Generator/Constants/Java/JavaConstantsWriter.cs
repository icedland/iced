// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Linq;
using System.Text;
using Generator.Documentation.Java;
using Generator.IO;

namespace Generator.Constants.Java {
	sealed class JavaConstantsWriter {
		readonly GenTypes genTypes;
		readonly IdentifierConverter idConverter;
		readonly JavaDocCommentWriter docWriter;
		readonly JavaDeprecatedWriter deprecatedWriter;

		public JavaConstantsWriter(GenTypes genTypes, IdentifierConverter idConverter, JavaDocCommentWriter docWriter, JavaDeprecatedWriter deprecatedWriter) {
			this.genTypes = genTypes;
			this.idConverter = idConverter;
			this.docWriter = docWriter;
			this.deprecatedWriter = deprecatedWriter;
		}

		public void Write(FileWriter writer, ConstantsType constantsType, string[] attributes, bool forcePublic, bool isTestFile) {
			if (forcePublic && !isTestFile)
				docWriter.WriteSummary(writer, JavaConstants.InternalDoc, constantsType.RawName, null);
			else
				docWriter.WriteSummary(writer, constantsType.Documentation.GetComment(TargetLanguage.Java), constantsType.RawName, null);
			foreach (var attr in attributes)
				writer.WriteLine(attr);
			var pub = forcePublic || constantsType.IsPublic ? "public " : string.Empty;
			writer.WriteLine($"{pub}final class {constantsType.Name(idConverter)} {{");

			using (writer.Indent()) {
				writer.WriteLine($"private {constantsType.Name(idConverter)}() {{");
				writer.WriteLine("}");
				writer.WriteLine();
				WriteVariants(writer, constantsType, forcePublic, isTestFile);
			}
			writer.WriteLine("}");
		}

		public void WriteVariants(FileWriter writer, ConstantsType constantsType, bool forcePublic, bool isTestFile) {
			var sb = new StringBuilder();
			var upperNames = Enums.EnumUtils.UppercaseTypeFields(constantsType.RawName);
			foreach (var constant in constantsType.Constants) {
				if (forcePublic && !isTestFile)
					docWriter.WriteSummary(writer, JavaConstants.InternalDoc, constantsType.RawName, null);
				else {
					var deprecMsg = deprecatedWriter.GetDeprecatedString(constant);
					docWriter.WriteSummary(writer, constant.Documentation.GetComment(TargetLanguage.Java), constantsType.RawName, deprecMsg);
				}
				if (constant.DeprecatedInfo.IsDeprecated)
					writer.WriteLine("@Deprecated");
				sb.Clear();
				sb.Append(forcePublic || constant.IsPublic ? "public " : string.Empty);
				sb.Append("static final ");
				sb.Append(GetType(constant.Kind));
				sb.Append(" ");
				sb.Append(upperNames ? constant.RawName.ToUpperInvariant() : constant.Name(idConverter));
				sb.Append(" = "); sb.Append(GetValue(constant));
				sb.Append(';');
				writer.WriteLine(sb.ToString());
			}
		}

		string GetType(ConstantKind kind) =>
			kind switch {
				ConstantKind.Char => "char",
				ConstantKind.String => "String",
				ConstantKind.Int32 or ConstantKind.UInt32 => "int",
				ConstantKind.UInt64 => "long",
				ConstantKind.Index => "int",
				ConstantKind.Register or ConstantKind.MemorySize => "int",
				_ => throw new InvalidOperationException(),
			};

		string GetValue(Constant constant) {
			switch (constant.Kind) {
			case ConstantKind.Char:
				var c = (char)constant.ValueUInt64;
				return "'" + c.ToString() + "'";

			case ConstantKind.String:
				if (constant.RefValue is string s)
					return "\"" + EscapeStringValue(s) + "\"";
				throw new InvalidOperationException();

			case ConstantKind.Int32:
			case ConstantKind.UInt32:
			case ConstantKind.Index:
				if (constant.UseHex)
					return NumberFormatter.FormatHexUInt32WithSep((uint)constant.ValueUInt64);
				return ((uint)constant.ValueUInt64).ToString();

			case ConstantKind.UInt64:
				const string suffix = "L";
				if (constant.UseHex)
					return NumberFormatter.FormatHexUInt64WithSep(constant.ValueUInt64) + suffix;
				return constant.ValueUInt64.ToString() + suffix;

			case ConstantKind.Register:
			case ConstantKind.MemorySize:
				return GetValueString(constant);

			default:
				throw new InvalidOperationException();
			}
		}

		static string EscapeStringValue(string s) => s;

		string GetValueString(Constant constant) {
			var enumType = EnumUtils.GetEnumType(genTypes, constant.Kind);
			var enumValue = enumType.Values.First(a => a.Value == constant.ValueUInt64);
			return JavaConstants.IcedPackage + "." + idConverter.ToDeclTypeAndValue(enumValue);
		}

		public void WriteFile(string package, string filename, ConstantsType constantsType, string[] attributes, bool isTestFile, string? id = null) {
			bool forcePublic =
				package.EndsWith(".internal", StringComparison.Ordinal) ||
				package.Contains(".internal.", StringComparison.Ordinal) ||
				isTestFile;
			if (id is string genId)
				new FileUpdater(TargetLanguage.Java, genId, filename).Generate(writer => WriteVariants(writer, constantsType, forcePublic, isTestFile));
			else {
				using (var writer = new FileWriter(TargetLanguage.Java, FileUtils.OpenWrite(filename))) {
					writer.WriteFileHeader();
					writer.WriteLine($"package {package};");
					writer.WriteLine();
					Write(writer, constantsType, attributes, forcePublic, isTestFile);
				}
			}
		}
	}
}
