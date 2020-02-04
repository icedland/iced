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
		public static readonly (string name, EnumValue value)[] OpAccessConstants = new (string name, EnumValue value)[] {
			("n", OpAccessEnum.Instance[nameof(OpAccess.None)]),
			("r", OpAccessEnum.Instance[nameof(OpAccess.Read)]),
			("cr", OpAccessEnum.Instance[nameof(OpAccess.CondRead)]),
			("w", OpAccessEnum.Instance[nameof(OpAccess.Write)]),
			("cw", OpAccessEnum.Instance[nameof(OpAccess.CondWrite)]),
			("rw", OpAccessEnum.Instance[nameof(OpAccess.ReadWrite)]),
			("rcw", OpAccessEnum.Instance[nameof(OpAccess.ReadCondWrite)]),
			("nma", OpAccessEnum.Instance[nameof(OpAccess.NoMemAccess)]),
		};

		public static readonly (string value, EnumValue flags)[] MemorySizeFlagsTable = new (string value, EnumValue flags)[] {
			("signed", MemorySizeFlagsEnum.Instance[nameof(MemorySizeFlags.Signed)]),
			("bcst", MemorySizeFlagsEnum.Instance[nameof(MemorySizeFlags.Broadcast)]),
			("packed", MemorySizeFlagsEnum.Instance[nameof(MemorySizeFlags.Packed)]),
		};

		public static readonly (string value, EnumValue flags)[] RegisterFlagsTable = new (string value, EnumValue flags)[] {
			("seg", RegisterFlagsEnum.Instance[nameof(RegisterFlags.SegmentRegister)]),
			("gpr", RegisterFlagsEnum.Instance[nameof(RegisterFlags.GPR)]),
			("gpr8", RegisterFlagsEnum.Instance[nameof(RegisterFlags.GPR8)]),
			("gpr16", RegisterFlagsEnum.Instance[nameof(RegisterFlags.GPR16)]),
			("gpr32", RegisterFlagsEnum.Instance[nameof(RegisterFlags.GPR32)]),
			("gpr64", RegisterFlagsEnum.Instance[nameof(RegisterFlags.GPR64)]),
			("xmm", RegisterFlagsEnum.Instance[nameof(RegisterFlags.XMM)]),
			("ymm", RegisterFlagsEnum.Instance[nameof(RegisterFlags.YMM)]),
			("zmm", RegisterFlagsEnum.Instance[nameof(RegisterFlags.ZMM)]),
			("vec", RegisterFlagsEnum.Instance[nameof(RegisterFlags.VectorRegister)]),
			("ip", RegisterFlagsEnum.Instance[nameof(RegisterFlags.IP)]),
			("k", RegisterFlagsEnum.Instance[nameof(RegisterFlags.K)]),
			("bnd", RegisterFlagsEnum.Instance[nameof(RegisterFlags.BND)]),
			("cr", RegisterFlagsEnum.Instance[nameof(RegisterFlags.CR)]),
			("dr", RegisterFlagsEnum.Instance[nameof(RegisterFlags.DR)]),
			("tr", RegisterFlagsEnum.Instance[nameof(RegisterFlags.TR)]),
			("st", RegisterFlagsEnum.Instance[nameof(RegisterFlags.ST)]),
			("mm", RegisterFlagsEnum.Instance[nameof(RegisterFlags.MM)]),
		};
	}

	static class EncoderConstants {
		public static readonly (string value, EnumValue flags)[] EncodingKindTable = new (string value, EnumValue flags)[] {
			("legacy", EncodingKindEnum.Instance[nameof(EncodingKind.Legacy)]),
			("VEX", EncodingKindEnum.Instance[nameof(EncodingKind.VEX)]),
			("EVEX", EncodingKindEnum.Instance[nameof(EncodingKind.EVEX)]),
			("XOP", EncodingKindEnum.Instance[nameof(EncodingKind.XOP)]),
			("3DNow!", EncodingKindEnum.Instance[nameof(EncodingKind.D3NOW)]),
		};
		public static readonly (string value, EnumValue flags)[] MandatoryPrefixTable = new (string value, EnumValue flags)[] {
			("", MandatoryPrefixEnum.Instance[nameof(MandatoryPrefix.None)]),
			("NP", MandatoryPrefixEnum.Instance[nameof(MandatoryPrefix.PNP)]),
			("66", MandatoryPrefixEnum.Instance[nameof(MandatoryPrefix.P66)]),
			("F3", MandatoryPrefixEnum.Instance[nameof(MandatoryPrefix.PF3)]),
			("F2", MandatoryPrefixEnum.Instance[nameof(MandatoryPrefix.PF2)]),
		};
		public static readonly (string value, EnumValue flags)[] OpCodeTableKindTable = new (string value, EnumValue flags)[] {
			("legacy", OpCodeTableKindEnum.Instance[nameof(OpCodeTableKind.Normal)]),
			("0F", OpCodeTableKindEnum.Instance[nameof(OpCodeTableKind.T0F)]),
			("0F38", OpCodeTableKindEnum.Instance[nameof(OpCodeTableKind.T0F38)]),
			("0F3A", OpCodeTableKindEnum.Instance[nameof(OpCodeTableKind.T0F3A)]),
			("X8", OpCodeTableKindEnum.Instance[nameof(OpCodeTableKind.XOP8)]),
			("X9", OpCodeTableKindEnum.Instance[nameof(OpCodeTableKind.XOP9)]),
			("XA", OpCodeTableKindEnum.Instance[nameof(OpCodeTableKind.XOPA)]),
		};
	}

	static class MasmSymbolOptionsConstants {
		public static readonly (string value, EnumValue flags)[] SymbolTestFlagsTable = new (string value, EnumValue flags)[] {
			("sym", SymbolTestFlagsEnum.Instance[nameof(SymbolTestFlags.Symbol)]),
			("signed", SymbolTestFlagsEnum.Instance[nameof(SymbolTestFlags.Signed)]),
			("symbr", SymbolTestFlagsEnum.Instance[nameof(SymbolTestFlags.SymbolDisplInBrackets)]),
			("displbr", SymbolTestFlagsEnum.Instance[nameof(SymbolTestFlags.DisplInBrackets)]),
			("rip", SymbolTestFlagsEnum.Instance[nameof(SymbolTestFlags.Rip)]),
			("disp0", SymbolTestFlagsEnum.Instance[nameof(SymbolTestFlags.ShowZeroDisplacements)]),
			("nods32", SymbolTestFlagsEnum.Instance[nameof(SymbolTestFlags.NoAddDsPrefix32)]),
		};
	}

	static class FormatMnemonicOptionsConstants {
		public static readonly (string value, EnumValue flags)[] FormatMnemonicOptionsTable = new (string value, EnumValue flags)[] {
			("noprefixes", FormatMnemonicOptionsEnum.Instance[nameof(FormatMnemonicOptions.NoPrefixes)]),
			("nomnemonic", FormatMnemonicOptionsEnum.Instance[nameof(FormatMnemonicOptions.NoMnemonic)]),
		};
	}
}
