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
using System.Collections.Generic;
using Generator.Enums;
using Generator.Enums.Formatter.Fast;
using Generator.IO;

namespace Generator.Formatters {
	abstract class FastFormatterTableSerializer : IFormatterTableSerializer {
		readonly object[][] infos;
		readonly IdentifierConverter idConverter;

		protected FastFormatterTableSerializer(object[][] infos, IdentifierConverter idConverter) {
			this.infos = infos;
			this.idConverter = idConverter;
		}

		public abstract string GetFilename(GeneratorContext generatorContext);

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

		public abstract void Serialize(GenTypes genTypes, FileWriter writer, StringsTable stringsTable);

		protected void SerializeTable(GenTypes genTypes, FileWriter writer, StringsTable stringsTable) {
			var fastFmtFlags = genTypes[TypeIds.FastFmtFlags];
			var hasVPrefixEnum = fastFmtFlags[nameof(FastFmtFlags.HasVPrefix)];
			var sameAsPrevEnum = fastFmtFlags[nameof(FastFmtFlags.SameAsPrev)];

			var flagsValues = new List<EnumValue>();
			int index = -1;
			uint prevMnemonicStringIndex = uint.MaxValue;
			foreach (var info in infos) {
				if (info.Length < 2)
					throw new InvalidOperationException();
				index++;
				var code = (EnumValue)info[0];
				if (code.Value != (uint)index)
					throw new InvalidOperationException();
				flagsValues.Clear();

				if (index != 0)
					writer.WriteLine();
				writer.WriteCommentLine(code.ToStringValue(idConverter));

				var mnemonic = info[1] as string ?? throw new InvalidOperationException();
				uint mnemonicStringIndex = stringsTable.GetIndex(mnemonic, ignoreVPrefix: true, out var hasVPrefix);
				if (hasVPrefix)
					flagsValues.Add(hasVPrefixEnum);
				bool isSame = false;
				if (mnemonicStringIndex == prevMnemonicStringIndex) {
					isSame = true;
					flagsValues.Add(sameAsPrevEnum);
				}

				for (int i = 2; i < info.Length; i++) {
					switch (info[i]) {
					case IEnumValue enumValue:
						var typeId = enumValue.DeclaringType.TypeId;
						if (typeId == TypeIds.FastFmtFlags) {
							if (enumValue is EnumValue enumValue2)
								flagsValues.Add(enumValue2);
							else if (enumValue is OrEnumValue orEnumValue)
								flagsValues.AddRange(orEnumValue.Values);
							else
								throw new InvalidOperationException();
						}
						else
							throw new InvalidOperationException();
						break;

					default:
						throw new InvalidOperationException();
					}
				}

				uint flagsValue = 0;
				foreach (var enumValue in flagsValues)
					flagsValue |= enumValue.Value;
				if (flagsValue > byte.MaxValue)
					throw new InvalidOperationException();
				writer.WriteByte((byte)flagsValue);
				string comment;
				switch (flagsValues.Count) {
				case 0:
					comment = "No flags set";
					break;
				case 1:
					comment = flagsValues[0].ToStringValue(idConverter);
					break;
				default:
					comment = new OrEnumValue(fastFmtFlags, flagsValues.ToArray()).ToStringValue(idConverter);
					break;
				}
				writer.WriteCommentLine(comment);

				// We save 4KB (11,595 -> 7,435 bytes)
				if (!isSame) {
					writer.WriteCompressedUInt32(mnemonicStringIndex);
					writer.WriteCommentLine($"{mnemonicStringIndex} = \"{mnemonic}\"");
				}

				prevMnemonicStringIndex = mnemonicStringIndex;
			}
		}
	}
}
