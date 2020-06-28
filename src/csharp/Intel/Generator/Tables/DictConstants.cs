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
				("seg", genTypes[TypeIds.RegisterFlags][nameof(RegisterFlags.SegmentRegister)]),
				("gpr", genTypes[TypeIds.RegisterFlags][nameof(RegisterFlags.GPR)]),
				("gpr8", genTypes[TypeIds.RegisterFlags][nameof(RegisterFlags.GPR8)]),
				("gpr16", genTypes[TypeIds.RegisterFlags][nameof(RegisterFlags.GPR16)]),
				("gpr32", genTypes[TypeIds.RegisterFlags][nameof(RegisterFlags.GPR32)]),
				("gpr64", genTypes[TypeIds.RegisterFlags][nameof(RegisterFlags.GPR64)]),
				("xmm", genTypes[TypeIds.RegisterFlags][nameof(RegisterFlags.XMM)]),
				("ymm", genTypes[TypeIds.RegisterFlags][nameof(RegisterFlags.YMM)]),
				("zmm", genTypes[TypeIds.RegisterFlags][nameof(RegisterFlags.ZMM)]),
				("vec", genTypes[TypeIds.RegisterFlags][nameof(RegisterFlags.VectorRegister)]),
				("ip", genTypes[TypeIds.RegisterFlags][nameof(RegisterFlags.IP)]),
				("k", genTypes[TypeIds.RegisterFlags][nameof(RegisterFlags.K)]),
				("bnd", genTypes[TypeIds.RegisterFlags][nameof(RegisterFlags.BND)]),
				("cr", genTypes[TypeIds.RegisterFlags][nameof(RegisterFlags.CR)]),
				("dr", genTypes[TypeIds.RegisterFlags][nameof(RegisterFlags.DR)]),
				("tr", genTypes[TypeIds.RegisterFlags][nameof(RegisterFlags.TR)]),
				("st", genTypes[TypeIds.RegisterFlags][nameof(RegisterFlags.ST)]),
				("mm", genTypes[TypeIds.RegisterFlags][nameof(RegisterFlags.MM)]),
				("tmm", genTypes[TypeIds.RegisterFlags][nameof(RegisterFlags.TMM)]),
			};
	}

	static class EncoderConstants {
		public static (string value, EnumValue flags)[] EncodingKindTable(GenTypes genTypes) =>
			new (string value, EnumValue flags)[] {
				("legacy", genTypes[TypeIds.EncodingKind][nameof(EncodingKind.Legacy)]),
				("VEX", genTypes[TypeIds.EncodingKind][nameof(EncodingKind.VEX)]),
				("EVEX", genTypes[TypeIds.EncodingKind][nameof(EncodingKind.EVEX)]),
				("XOP", genTypes[TypeIds.EncodingKind][nameof(EncodingKind.XOP)]),
				("3DNow!", genTypes[TypeIds.EncodingKind][nameof(EncodingKind.D3NOW)]),
			};
		public static (string value, EnumValue flags)[] MandatoryPrefixTable(GenTypes genTypes) =>
			new (string value, EnumValue flags)[] {
				("", genTypes[TypeIds.MandatoryPrefix][nameof(MandatoryPrefix.None)]),
				("NP", genTypes[TypeIds.MandatoryPrefix][nameof(MandatoryPrefix.PNP)]),
				("66", genTypes[TypeIds.MandatoryPrefix][nameof(MandatoryPrefix.P66)]),
				("F3", genTypes[TypeIds.MandatoryPrefix][nameof(MandatoryPrefix.PF3)]),
				("F2", genTypes[TypeIds.MandatoryPrefix][nameof(MandatoryPrefix.PF2)]),
			};
		public static (string value, EnumValue flags)[] OpCodeTableKindTable(GenTypes genTypes) =>
			new (string value, EnumValue flags)[] {
				("legacy", genTypes[TypeIds.OpCodeTableKind][nameof(OpCodeTableKind.Normal)]),
				("0F", genTypes[TypeIds.OpCodeTableKind][nameof(OpCodeTableKind.T0F)]),
				("0F38", genTypes[TypeIds.OpCodeTableKind][nameof(OpCodeTableKind.T0F38)]),
				("0F3A", genTypes[TypeIds.OpCodeTableKind][nameof(OpCodeTableKind.T0F3A)]),
				("X8", genTypes[TypeIds.OpCodeTableKind][nameof(OpCodeTableKind.XOP8)]),
				("X9", genTypes[TypeIds.OpCodeTableKind][nameof(OpCodeTableKind.XOP9)]),
				("XA", genTypes[TypeIds.OpCodeTableKind][nameof(OpCodeTableKind.XOPA)]),
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
