// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using Generator.Enums;
using Generator.Enums.Formatter.Fast;
using Generator.IO;

namespace Generator.Formatters {
	sealed class FastFmtInstructionDef {
		public readonly EnumValue Code;
		public readonly string Mnemonic;
		public readonly IEnumValue Flags;

		public FastFmtInstructionDef(EnumValue code, string mnemonic, IEnumValue flags) {
			Code = code;
			Mnemonic = mnemonic;
			Flags = flags;
		}
	}

	abstract class FastFormatterTableSerializer : IFormatterTableSerializer {
		readonly FastFmtInstructionDef[] defs;
		readonly IdentifierConverter idConverter;

		protected FastFormatterTableSerializer(FastFmtInstructionDef[] defs, IdentifierConverter idConverter) {
			this.defs = defs;
			this.idConverter = idConverter;
		}

		public abstract string GetFilename(GenTypes genTypes);

		public void Initialize(GenTypes genTypes, StringsTable stringsTable) {
			var expectedLength = genTypes[TypeIds.Code].Values.Length;
			if (defs.Length != expectedLength)
				throw new InvalidOperationException($"Found {defs.Length} elements, expected {expectedLength}");
			for (int i = 0; i < defs.Length; i++) {
				var def = defs[i];
				stringsTable.Add((uint)i, def.Mnemonic, true);
			}
		}

		protected void SerializeTable(GenTypes genTypes, ByteTableWriter writer, StringsTable stringsTable) {
			var fastFmtFlags = genTypes[TypeIds.FastFmtFlags];
			var hasVPrefixEnum = fastFmtFlags[nameof(FastFmtFlags.HasVPrefix)];
			var sameAsPrevEnum = fastFmtFlags[nameof(FastFmtFlags.SameAsPrev)];

			var flagsValues = new List<EnumValue>();
			int index = -1;
			uint prevMnemonicStringIndex = uint.MaxValue;
			foreach (var def in defs) {
				index++;
				var code = def.Code;
				if (code.Value != (uint)index)
					throw new InvalidOperationException();
				flagsValues.Clear();

				if (index != 0)
					writer.WriteLine();
				writer.WriteCommentLine(code.ToStringValue(idConverter));

				var mnemonic = def.Mnemonic;
				uint mnemonicStringIndex = stringsTable.GetIndex(mnemonic, optimize: true, out var hasVPrefix);
				var flags = def.Flags;
				if (hasVPrefix)
					flagsValues.Add(hasVPrefixEnum);
				bool isSame = false;
				if (mnemonicStringIndex == prevMnemonicStringIndex) {
					isSame = true;
					flagsValues.Add(sameAsPrevEnum);
				}

				if (def.Flags is EnumValue flags2)
					flagsValues.Add(flags2);
				else if (def.Flags is OrEnumValue flags3)
					flagsValues.AddRange(flags3.Values);
				else
					throw new InvalidOperationException();

				uint flagsValue = 0;
				foreach (var enumValue in flagsValues)
					flagsValue |= enumValue.Value;
				if (flagsValue > byte.MaxValue)
					throw new InvalidOperationException();
				writer.WriteByte((byte)flagsValue);
				string comment = flagsValues.Count switch {
					0 => "No flags set",
					1 => flagsValues[0].ToStringValue(idConverter),
					_ => new OrEnumValue(fastFmtFlags, flagsValues.ToArray()).ToStringValue(idConverter),
				};
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
