// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Linq;
using Generator.IO;

namespace Generator.Tables.CSharp {
	[Generator(TargetLanguage.CSharp)]
	sealed class CSharpMemorySizeInfoTableGenerator {
		readonly IdentifierConverter idConverter;
		readonly GenTypes genTypes;

		public CSharpMemorySizeInfoTableGenerator(GeneratorContext generatorContext) {
			idConverter = CSharpIdentifierConverter.Create();
			genTypes = generatorContext.Types;
		}

		public void Generate() {
			var defs = genTypes.GetObject<MemorySizeDefs>(TypeIds.MemorySizeDefs).Defs;
			var filename = CSharpConstants.GetFilename(genTypes, CSharpConstants.IcedNamespace, "MemorySizeExtensions.cs");
			var sizeToIndex = new Dictionary<uint, uint>();
			uint index = 0;
			foreach (var size in defs.Select(a => a.Size).Distinct().OrderBy(a => a))
				sizeToIndex[size] = index++;
			const int SizeBits = 5;
			const ushort IsSigned = 0x8000;
			const uint SizeMask = (1U << SizeBits) - 1;
			const int SizeShift = 0;
			const int ElemSizeShift = SizeBits;
			if (sizeToIndex.Count > SizeMask)
				throw new InvalidOperationException();
			new FileUpdater(TargetLanguage.CSharp, "MemorySizeInfoTable", filename).Generate(writer => {
				var memSizeName = genTypes[TypeIds.MemorySize].Name(idConverter);
				foreach (var def in defs) {
					byte b0 = checked((byte)def.ElementType.Value);
					ushort value = checked((ushort)((sizeToIndex[def.Size] << SizeShift) | (sizeToIndex[def.ElementSize] << ElemSizeShift)));
					if ((value & IsSigned) != 0)
						throw new InvalidOperationException();
					if (def.IsSigned)
						value |= IsSigned;
					writer.WriteLine($"0x{b0:X2}, 0x{(byte)value:X2}, 0x{(byte)(value >> 8):X2},");
				}
			});
			new FileUpdater(TargetLanguage.CSharp, "ConstData", filename).Generate(writer => {
				writer.WriteLine($"const ushort {idConverter.Constant(nameof(IsSigned))} = {IsSigned};");
				writer.WriteLine($"const uint {idConverter.Constant(nameof(SizeMask))} = {SizeMask};");
				writer.WriteLine($"const int {idConverter.Constant(nameof(SizeShift))} = {SizeShift};");
				writer.WriteLine($"const int {idConverter.Constant(nameof(ElemSizeShift))} = {ElemSizeShift};");
				writer.WriteLine("var sizes = new ushort[] {");
				using (writer.Indent()) {
					foreach (var size in sizeToIndex.Select(a => a.Key).OrderBy(a => a))
						writer.WriteLine($"{size},");
				}
				writer.WriteLine("};");
			});
		}
	}
}
