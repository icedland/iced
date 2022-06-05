// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Linq;
using Generator.IO;

namespace Generator.Tables.Java {
	[Generator(TargetLanguage.Java)]
	sealed class JavaMemorySizeInfoTableGenerator {
		readonly IdentifierConverter idConverter;
		readonly GenTypes genTypes;

		public JavaMemorySizeInfoTableGenerator(GeneratorContext generatorContext) {
			idConverter = JavaIdentifierConverter.Create();
			genTypes = generatorContext.Types;
		}

		public void Generate() {
			var defs = genTypes.GetObject<MemorySizeDefs>(TypeIds.MemorySizeDefs).Defs;
			var filename = JavaConstants.GetFilename(genTypes, JavaConstants.IcedPackage, "MemorySizeInfo.java");
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
			new FileUpdater(TargetLanguage.Java, "MemorySizeInfoTable", filename).Generate(writer => {
				var memSizeName = genTypes[TypeIds.MemorySize].Name(idConverter);
				foreach (var def in defs) {
					byte b0 = checked((byte)def.ElementType.Value);
					ushort value = checked((ushort)((sizeToIndex[def.Size] << SizeShift) | (sizeToIndex[def.ElementSize] << ElemSizeShift)));
					if ((value & IsSigned) != 0)
						throw new InvalidOperationException();
					if (def.IsSigned)
						value |= IsSigned;
					writer.WriteLine($"(byte)0x{b0:X2}, (byte)0x{(byte)value:X2}, (byte)0x{(byte)(value >> 8):X2},");
				}
			});
			new FileUpdater(TargetLanguage.Java, "ConstData", filename).Generate(writer => {
				writer.WriteLine($"final short {idConverter.Constant(nameof(IsSigned))} = {unchecked((short)IsSigned)};");
				writer.WriteLine($"final int {idConverter.Constant(nameof(SizeMask))} = {unchecked((int)SizeMask)};");
				writer.WriteLine($"final int {idConverter.Constant(nameof(SizeShift))} = {SizeShift};");
				writer.WriteLine($"final int {idConverter.Constant(nameof(ElemSizeShift))} = {ElemSizeShift};");
				writer.WriteLine("short[] sizes = new short[] {");
				using (writer.Indent()) {
					foreach (var size in sizeToIndex.Select(a => a.Key).OrderBy(a => a))
						writer.WriteLine($"{size},");
				}
				writer.WriteLine("};");
			});
		}
	}
}
