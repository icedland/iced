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
	abstract class FormatterTableSerializer {
		protected readonly object[][] infos;
		protected readonly IdentifierConverter idConverter;
		readonly EnumValue previousCtorKind;

		protected FormatterTableSerializer(object[][] infos, IdentifierConverter idConverter, EnumValue previousCtorKind) {
			this.infos = infos;
			this.idConverter = idConverter;
			this.previousCtorKind = previousCtorKind;
		}

		public void Initialize(GenTypes genTypes, StringsTable stringsTable) {
			var expectedLength = genTypes[TypeIds.Code].Values.Length;
			if (infos.Length != expectedLength)
				throw new InvalidOperationException($"Found {infos.Length} elements, expected {expectedLength}");
			for (int i = 0; i < infos.Length; i++) {
				var info = infos[i];
				bool ignoreVPrefix = true;
				foreach (var o in info) {
					if (o is string s) {
						stringsTable.Add((uint)i, s, ignoreVPrefix);
						ignoreVPrefix = false;
					}
				}
			}
		}

		protected void SerializeTable(FileWriter writer, StringsTable stringsTable) {
			int index = -1;
			for (int i = 0; i < infos.Length; i++) {
				var info = infos[i];
				index++;
				var ctorKind = (EnumValue)info[0];
				var code = (EnumValue)info[Utils.CodeValueIndex];
				if (code.Value != (uint)index)
					throw new InvalidOperationException();

				if (index != 0)
					writer.WriteLine();
				writer.WriteCommentLine(code.ToStringValue(idConverter));

				bool isSame = i > 0 && IsSame(infos[i - 1], info);
				if (isSame)
					ctorKind = previousCtorKind;

				if ((uint)ctorKind.Value > 0x7F)
					throw new InvalidOperationException();
				uint firstStringIndex = GetFirstStringIndex(stringsTable, info, out bool hasVPrefix);
				writer.WriteByte((byte)((uint)ctorKind.Value | (hasVPrefix ? 0x80U : 0)));
				if (hasVPrefix)
					writer.WriteCommentLine($"'v', {ctorKind.ToStringValue(idConverter)}");
				else
					writer.WriteCommentLine($"{ctorKind.ToStringValue(idConverter)}");
				if (isSame)
					continue;
				uint si;
				for (int j = 2; j < info.Length; j++) {
					switch (info[j]) {
					case string s:
						if (firstStringIndex != uint.MaxValue) {
							si = firstStringIndex;
							firstStringIndex = uint.MaxValue;
						}
						else {
							si = stringsTable.GetIndex(s, ignoreVPrefix: true, out hasVPrefix);
							if (hasVPrefix)
								throw new InvalidOperationException();
						}
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
							writer.WriteCompressedUInt32((uint)enumValue.Value);
							writer.WriteCommentLine($"0x{(uint)enumValue.Value:X} = {enumValue.ToStringValue(idConverter)}");
						}
						else if (typeId == TypeIds.IntelInstrOpInfoFlags) {
							writer.WriteCompressedUInt32((uint)enumValue.Value);
							writer.WriteCommentLine($"0x{(uint)enumValue.Value:X} = {enumValue.ToStringValue(idConverter)}");
						}
						else if (typeId == TypeIds.MasmInstrOpInfoFlags) {
							writer.WriteCompressedUInt32((uint)enumValue.Value);
							writer.WriteCommentLine($"0x{(uint)enumValue.Value:X} = {enumValue.ToStringValue(idConverter)}");
						}
						else if (typeId == TypeIds.NasmInstrOpInfoFlags) {
							writer.WriteCompressedUInt32((uint)enumValue.Value);
							writer.WriteCommentLine($"0x{(uint)enumValue.Value:X} = {enumValue.ToStringValue(idConverter)}");
						}
						else if (typeId == TypeIds.PseudoOpsKind) {
							if ((uint)enumValue.Value > byte.MaxValue)
								throw new InvalidOperationException();
							writer.WriteByte((byte)enumValue.Value);
							writer.WriteCommentLine(enumValue.ToStringValue(idConverter));
						}
						else if (typeId == TypeIds.CodeSize) {
							if ((uint)enumValue.Value > byte.MaxValue)
								throw new InvalidOperationException();
							writer.WriteByte((byte)enumValue.Value);
							writer.WriteCommentLine(enumValue.ToStringValue(idConverter));
						}
						else if (typeId == TypeIds.Register) {
							if ((uint)enumValue.Value > byte.MaxValue)
								throw new InvalidOperationException();
							writer.WriteByte((byte)enumValue.Value);
							writer.WriteCommentLine(enumValue.ToStringValue(idConverter));
						}
						else if (typeId == TypeIds.MemorySize) {
							if ((uint)enumValue.Value > byte.MaxValue)
								throw new InvalidOperationException();
							writer.WriteByte((byte)enumValue.Value);
							writer.WriteCommentLine(enumValue.ToStringValue(idConverter));
						}
						else if (typeId == TypeIds.NasmSignExtendInfo) {
							if ((uint)enumValue.Value > byte.MaxValue)
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

		static uint GetFirstStringIndex(StringsTable stringsTable, object[] info, out bool hasVPrefix) {
			if (!(info[2] is string s))
				throw new InvalidOperationException();
			return stringsTable.GetIndex(s, ignoreVPrefix: true, out hasVPrefix);
		}

		static bool IsSame(object[] a, object[] b) {
			if (a.Length != b.Length)
				return false;
			for (int i = 0; i < a.Length; i++) {
				if (i == 1) {
					if (!(a[i] is EnumValue eva && eva.DeclaringType.TypeId == TypeIds.Code) ||
						!(b[i] is EnumValue evb && evb.DeclaringType.TypeId == TypeIds.Code)) {
						throw new InvalidOperationException();
					}
					continue;
				}
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
