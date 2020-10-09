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

using Generator.Constants.Encoder;
using Generator.Enums;
using Generator.Enums.Encoder;
using Generator.Enums.Formatter;
using Generator.Enums.Formatter.Masm;
using Generator.Enums.InstructionInfo;

namespace Generator.Tables {
	static class InstrInfoDictConstants {
		public static (string name, EnumValue value)[] OpAccessConstants(GenTypes genTypes) =>
			new (string name, EnumValue value)[] {
				("n", genTypes[TypeIds.OpAccess][nameof(OpAccess.None)]),
				("r", genTypes[TypeIds.OpAccess][nameof(OpAccess.Read)]),
				("cr", genTypes[TypeIds.OpAccess][nameof(OpAccess.CondRead)]),
				("w", genTypes[TypeIds.OpAccess][nameof(OpAccess.Write)]),
				("cw", genTypes[TypeIds.OpAccess][nameof(OpAccess.CondWrite)]),
				("rw", genTypes[TypeIds.OpAccess][nameof(OpAccess.ReadWrite)]),
				("rcw", genTypes[TypeIds.OpAccess][nameof(OpAccess.ReadCondWrite)]),
				("nma", genTypes[TypeIds.OpAccess][nameof(OpAccess.NoMemAccess)]),
			};

		public static (string value, EnumValue flags)[] MemorySizeFlagsTable(GenTypes genTypes) =>
			new (string value, EnumValue flags)[] {
				("signed", genTypes[TypeIds.MemorySizeFlags][nameof(MemorySizeFlags.Signed)]),
				("bcst", genTypes[TypeIds.MemorySizeFlags][nameof(MemorySizeFlags.Broadcast)]),
				("packed", genTypes[TypeIds.MemorySizeFlags][nameof(MemorySizeFlags.Packed)]),
			};

		public static (string value, EnumValue flags)[] RegisterFlagsTable(GenTypes genTypes) =>
			new (string value, EnumValue flags)[] {
				(RegisterConstants.SegmentRegister, genTypes[TypeIds.RegisterFlags][nameof(RegisterFlags.SegmentRegister)]),
				(RegisterConstants.GPR, genTypes[TypeIds.RegisterFlags][nameof(RegisterFlags.GPR)]),
				(RegisterConstants.GPR8, genTypes[TypeIds.RegisterFlags][nameof(RegisterFlags.GPR8)]),
				(RegisterConstants.GPR16, genTypes[TypeIds.RegisterFlags][nameof(RegisterFlags.GPR16)]),
				(RegisterConstants.GPR32, genTypes[TypeIds.RegisterFlags][nameof(RegisterFlags.GPR32)]),
				(RegisterConstants.GPR64, genTypes[TypeIds.RegisterFlags][nameof(RegisterFlags.GPR64)]),
				(RegisterConstants.XMM, genTypes[TypeIds.RegisterFlags][nameof(RegisterFlags.XMM)]),
				(RegisterConstants.YMM, genTypes[TypeIds.RegisterFlags][nameof(RegisterFlags.YMM)]),
				(RegisterConstants.ZMM, genTypes[TypeIds.RegisterFlags][nameof(RegisterFlags.ZMM)]),
				(RegisterConstants.VectorRegister, genTypes[TypeIds.RegisterFlags][nameof(RegisterFlags.VectorRegister)]),
				(RegisterConstants.IP, genTypes[TypeIds.RegisterFlags][nameof(RegisterFlags.IP)]),
				(RegisterConstants.K, genTypes[TypeIds.RegisterFlags][nameof(RegisterFlags.K)]),
				(RegisterConstants.BND, genTypes[TypeIds.RegisterFlags][nameof(RegisterFlags.BND)]),
				(RegisterConstants.CR, genTypes[TypeIds.RegisterFlags][nameof(RegisterFlags.CR)]),
				(RegisterConstants.DR, genTypes[TypeIds.RegisterFlags][nameof(RegisterFlags.DR)]),
				(RegisterConstants.TR, genTypes[TypeIds.RegisterFlags][nameof(RegisterFlags.TR)]),
				(RegisterConstants.ST, genTypes[TypeIds.RegisterFlags][nameof(RegisterFlags.ST)]),
				(RegisterConstants.MM, genTypes[TypeIds.RegisterFlags][nameof(RegisterFlags.MM)]),
				(RegisterConstants.TMM, genTypes[TypeIds.RegisterFlags][nameof(RegisterFlags.TMM)]),
			};
	}

	static class EncoderConstants {
		public static (string value, EnumValue flags)[] EncodingKindTable(GenTypes genTypes) =>
			new (string value, EnumValue flags)[] {
				(OpCodeInfoConstants.Encoding_Legacy, genTypes[TypeIds.EncodingKind][nameof(EncodingKind.Legacy)]),
				(OpCodeInfoConstants.Encoding_VEX, genTypes[TypeIds.EncodingKind][nameof(EncodingKind.VEX)]),
				(OpCodeInfoConstants.Encoding_EVEX, genTypes[TypeIds.EncodingKind][nameof(EncodingKind.EVEX)]),
				(OpCodeInfoConstants.Encoding_XOP, genTypes[TypeIds.EncodingKind][nameof(EncodingKind.XOP)]),
				(OpCodeInfoConstants.Encoding_3DNOW, genTypes[TypeIds.EncodingKind][nameof(EncodingKind.D3NOW)]),
			};
		public static (string value, EnumValue flags)[] MandatoryPrefixTable(GenTypes genTypes) =>
			new (string value, EnumValue flags)[] {
				(OpCodeInfoConstants.MandatoryPrefix_None, genTypes[TypeIds.MandatoryPrefix][nameof(MandatoryPrefix.None)]),
				(OpCodeInfoConstants.MandatoryPrefix_NP, genTypes[TypeIds.MandatoryPrefix][nameof(MandatoryPrefix.PNP)]),
				(OpCodeInfoConstants.MandatoryPrefix_66, genTypes[TypeIds.MandatoryPrefix][nameof(MandatoryPrefix.P66)]),
				(OpCodeInfoConstants.MandatoryPrefix_F3, genTypes[TypeIds.MandatoryPrefix][nameof(MandatoryPrefix.PF3)]),
				(OpCodeInfoConstants.MandatoryPrefix_F2, genTypes[TypeIds.MandatoryPrefix][nameof(MandatoryPrefix.PF2)]),
			};
		public static (string value, EnumValue flags)[] OpCodeTableKindTable(GenTypes genTypes) =>
			new (string value, EnumValue flags)[] {
				(OpCodeInfoConstants.Table_Legacy, genTypes[TypeIds.OpCodeTableKind][nameof(OpCodeTableKind.Normal)]),
				(OpCodeInfoConstants.Table_0F, genTypes[TypeIds.OpCodeTableKind][nameof(OpCodeTableKind.T0F)]),
				(OpCodeInfoConstants.Table_0F38, genTypes[TypeIds.OpCodeTableKind][nameof(OpCodeTableKind.T0F38)]),
				(OpCodeInfoConstants.Table_0F3A, genTypes[TypeIds.OpCodeTableKind][nameof(OpCodeTableKind.T0F3A)]),
				(OpCodeInfoConstants.Table_XOP8, genTypes[TypeIds.OpCodeTableKind][nameof(OpCodeTableKind.XOP8)]),
				(OpCodeInfoConstants.Table_XOP9, genTypes[TypeIds.OpCodeTableKind][nameof(OpCodeTableKind.XOP9)]),
				(OpCodeInfoConstants.Table_XOPA, genTypes[TypeIds.OpCodeTableKind][nameof(OpCodeTableKind.XOPA)]),
			};
	}

	static class MasmSymbolOptionsConstants {
		public static (string value, EnumValue flags)[] SymbolTestFlagsTable(GenTypes genTypes) =>
			new (string value, EnumValue flags)[] {
				("sym", genTypes[TypeIds.MasmSymbolTestFlags][nameof(SymbolTestFlags.Symbol)]),
				("signed", genTypes[TypeIds.MasmSymbolTestFlags][nameof(SymbolTestFlags.Signed)]),
				("symbr", genTypes[TypeIds.MasmSymbolTestFlags][nameof(SymbolTestFlags.SymbolDisplInBrackets)]),
				("displbr", genTypes[TypeIds.MasmSymbolTestFlags][nameof(SymbolTestFlags.DisplInBrackets)]),
				("rip", genTypes[TypeIds.MasmSymbolTestFlags][nameof(SymbolTestFlags.Rip)]),
				("disp0", genTypes[TypeIds.MasmSymbolTestFlags][nameof(SymbolTestFlags.ShowZeroDisplacements)]),
				("nods32", genTypes[TypeIds.MasmSymbolTestFlags][nameof(SymbolTestFlags.NoAddDsPrefix32)]),
			};
	}

	static class FormatMnemonicOptionsConstants {
		public static (string value, EnumValue flags)[] FormatMnemonicOptionsTable(GenTypes genTypes) =>
			new (string value, EnumValue flags)[] {
				("noprefixes", genTypes[TypeIds.FormatMnemonicOptions][nameof(FormatMnemonicOptions.NoPrefixes)]),
				("nomnemonic", genTypes[TypeIds.FormatMnemonicOptions][nameof(FormatMnemonicOptions.NoMnemonic)]),
			};
	}

	static class SymbolFlagsConstants {
		public static (string value, EnumValue flags)[] SymbolFlagsTable(GenTypes genTypes) =>
			new (string value, EnumValue flags)[] {
				("rel", genTypes[TypeIds.SymbolFlags][nameof(SymbolFlags.Relative)]),
				("signed", genTypes[TypeIds.SymbolFlags][nameof(SymbolFlags.Signed)]),
			};
	}
}
