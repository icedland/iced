/*
Copyright (C) 2018-2019 de4dot@gmail.com

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System;
using Generator.Enums;
using Generator.IO;

namespace Generator.Formatters {
	sealed class FmtInstructionDef {
		public readonly EnumValue Code;
		public readonly string Mnemonic;
		public readonly EnumValue CtorKind;
		public readonly object[] Args;

		public FmtInstructionDef(EnumValue code, string mnemonic, EnumValue ctorKind, object[] args) {
			Code = code;
			Mnemonic = mnemonic;
			CtorKind = ctorKind;
			Args = args;
		}
	}

	abstract class FormatterTableSerializer : IFormatterTableSerializer {
		protected readonly FmtInstructionDef[] defs;
		protected readonly IdentifierConverter idConverter;
		readonly EnumValue previousCtorKind;

		protected FormatterTableSerializer(FmtInstructionDef[] infos, IdentifierConverter idConverter, EnumValue previousCtorKind) {
			this.defs = infos;
			this.idConverter = idConverter;
			this.previousCtorKind = previousCtorKind;
		}

		public abstract string GetFilename(GenTypes genTypes);

		public void Initialize(GenTypes genTypes, StringsTable stringsTable) {
			var expectedLength = genTypes[TypeIds.Code].Values.Length;
			if (defs.Length != expectedLength)
				throw new InvalidOperationException($"Found {defs.Length} elements, expected {expectedLength}");
			for (int i = 0; i < defs.Length; i++) {
				var def = defs[i];
				stringsTable.Add((uint)i, def.Mnemonic, true);
				foreach (var o in def.Args) {
					if (o is string s)
						stringsTable.Add((uint)i, s, false);
				}
			}
		}

		public abstract void Serialize(GenTypes genTypes, FileWriter writer, StringsTable stringsTable);

		protected void SerializeTable(FileWriter writer, StringsTable stringsTable) {
			int index = -1;
			for (int i = 0; i < defs.Length; i++) {
				var def = defs[i];
				index++;
				var ctorKind = def.CtorKind;
				var code = def.Code;
				if (code.Value != (uint)index)
					throw new InvalidOperationException();

				if (index != 0)
					writer.WriteLine();
				writer.WriteCommentLine(code.ToStringValue(idConverter));

				bool isSame = i > 0 && IsSame(defs[i - 1], def);
				if (isSame)
					ctorKind = previousCtorKind;

				uint si = stringsTable.GetIndex(def.Mnemonic, ignoreVPrefix: true, out bool hasVPrefix);
				if (ctorKind.Value > 0x7F)
					throw new InvalidOperationException();
				writer.WriteByte((byte)(ctorKind.Value | (hasVPrefix ? 0x80U : 0)));
				if (hasVPrefix)
					writer.WriteCommentLine($"'v', {ctorKind.ToStringValue(idConverter)}");
				else
					writer.WriteCommentLine($"{ctorKind.ToStringValue(idConverter)}");
				if (isSame)
					continue;

				writer.WriteCompressedUInt32(si);
				writer.WriteCommentLine($"{si} = \"{def.Mnemonic}\"");
				foreach (var arg in def.Args) {
					switch (arg) {
					case string s:
						si = stringsTable.GetIndex(s, ignoreVPrefix: true, out hasVPrefix);
						if (hasVPrefix)
							throw new InvalidOperationException();
						writer.WriteCompressedUInt32(si);
						writer.WriteCommentLine($"{si} = \"{s}\"");
						break;

					case char c:
						if ((ushort)c > byte.MaxValue)
							throw new InvalidOperationException();
						writer.WriteByte((byte)c);
						if (c == '\0')
							writer.WriteCommentLine(@"'\0'");
						else
							writer.WriteCommentLine($"'{c}'");
						break;

					case int ival:
						writer.WriteCompressedUInt32((uint)ival);
						writer.WriteCommentLine($"0x{ival:X}");
						break;

					case bool b:
						writer.WriteByte((byte)(b ? 1 : 0));
						writer.WriteCommentLine(b.ToString());
						break;

					case IEnumValue enumValue:
						var typeId = enumValue.DeclaringType.TypeId;
						if (typeId == TypeIds.GasInstrOpInfoFlags) {
							writer.WriteCompressedUInt32(enumValue.Value);
							writer.WriteCommentLine($"0x{enumValue.Value:X} = {enumValue.ToStringValue(idConverter)}");
						}
						else if (typeId == TypeIds.IntelInstrOpInfoFlags) {
							writer.WriteCompressedUInt32(enumValue.Value);
							writer.WriteCommentLine($"0x{enumValue.Value:X} = {enumValue.ToStringValue(idConverter)}");
						}
						else if (typeId == TypeIds.MasmInstrOpInfoFlags) {
							writer.WriteCompressedUInt32(enumValue.Value);
							writer.WriteCommentLine($"0x{enumValue.Value:X} = {enumValue.ToStringValue(idConverter)}");
						}
						else if (typeId == TypeIds.NasmInstrOpInfoFlags) {
							writer.WriteCompressedUInt32(enumValue.Value);
							writer.WriteCommentLine($"0x{enumValue.Value:X} = {enumValue.ToStringValue(idConverter)}");
						}
						else if (typeId == TypeIds.PseudoOpsKind) {
							if (enumValue.Value > byte.MaxValue)
								throw new InvalidOperationException();
							writer.WriteByte((byte)enumValue.Value);
							writer.WriteCommentLine(enumValue.ToStringValue(idConverter));
						}
						else if (typeId == TypeIds.CodeSize) {
							if (enumValue.Value > byte.MaxValue)
								throw new InvalidOperationException();
							writer.WriteByte((byte)enumValue.Value);
							writer.WriteCommentLine(enumValue.ToStringValue(idConverter));
						}
						else if (typeId == TypeIds.Register) {
							if (enumValue.Value > byte.MaxValue)
								throw new InvalidOperationException();
							writer.WriteByte((byte)enumValue.Value);
							writer.WriteCommentLine(enumValue.ToStringValue(idConverter));
						}
						else if (typeId == TypeIds.MemorySize) {
							if (enumValue.Value > byte.MaxValue)
								throw new InvalidOperationException();
							writer.WriteByte((byte)enumValue.Value);
							writer.WriteCommentLine(enumValue.ToStringValue(idConverter));
						}
						else if (typeId == TypeIds.NasmSignExtendInfo) {
							if (enumValue.Value > byte.MaxValue)
								throw new InvalidOperationException();
							writer.WriteByte((byte)enumValue.Value);
							writer.WriteCommentLine(enumValue.ToStringValue(idConverter));
						}
						else
							throw new InvalidOperationException();
						break;

					default:
						throw new InvalidOperationException();
					}
				}
			}
		}

		static bool IsSame(FmtInstructionDef a, FmtInstructionDef b) {
			if (a.CtorKind != b.CtorKind)
				return false;
			if (!StringComparer.Ordinal.Equals(a.Mnemonic, b.Mnemonic))
				return false;

			return IsSame(a.Args, b.Args);
		}

		static bool IsSame(object[] a, object[] b) {
			if (a.Length != b.Length)
				return false;
			for (int i = 0; i < a.Length; i++) {
				bool same;
				if (a[i] is string sa && b[i] is string sb)
					same = StringComparer.Ordinal.Equals(sa, sb);
				else
					same = a[i].Equals(b[i]);
				if (!same)
					return false;
			}
			return true;
		}
	}
}
