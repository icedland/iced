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

#if !NO_ENCODER
using System;
using System.Collections.Generic;
using System.IO;
using Iced.Intel;

namespace Iced.UnitTests.Intel.EncoderTests {
	static class OpCodeInfoTestCasesReader {
		static readonly Dictionary<string, Code> toCode = CreateToCode();
		static readonly Dictionary<string, TupleType> toTupleType = CreateToTupleType();
		static readonly Dictionary<string, OpCodeOperandKind> toOpCodeOperandKind = CreateToOpCodeOperandKind();

		static Dictionary<string, Code> CreateToCode() {
			var dict = new Dictionary<string, Code>(StringComparer.Ordinal);
			var names = Enum.GetNames(typeof(Code));
			var values = (Code[])Enum.GetValues(typeof(Code));
			if (names.Length != values.Length)
				throw new InvalidOperationException();
			for (int i = 0; i < names.Length; i++)
				dict.Add(names[i], values[i]);
			return dict;
		}

		static Dictionary<string, TupleType> CreateToTupleType() {
			var dict = new Dictionary<string, TupleType>(StringComparer.Ordinal);
			var names = Enum.GetNames(typeof(TupleType));
			var values = (TupleType[])Enum.GetValues(typeof(TupleType));
			if (names.Length != values.Length)
				throw new InvalidOperationException();
			for (int i = 0; i < names.Length; i++)
				dict.Add(names[i], values[i]);
			return dict;
		}

		static Dictionary<string, OpCodeOperandKind> CreateToOpCodeOperandKind() {
			var dict = new Dictionary<string, OpCodeOperandKind>(StringComparer.Ordinal);
			var names = Enum.GetNames(typeof(OpCodeOperandKind));
			var values = (OpCodeOperandKind[])Enum.GetValues(typeof(OpCodeOperandKind));
			if (names.Length != values.Length)
				throw new InvalidOperationException();
			for (int i = 0; i < names.Length; i++)
				dict.Add(names[i], values[i]);
			return dict;
		}

		public static IEnumerable<OpCodeInfoTestCase> ReadFile(string filename) {
			int lineNo = 0;
			foreach (var line in File.ReadLines(filename)) {
				lineNo++;
				if (line.Length == 0 || line[0] == '#')
					continue;
				OpCodeInfoTestCase testCase;
				try {
					testCase = ReadTestCase(line, lineNo);
				}
				catch (Exception ex) {
					throw new InvalidOperationException($"Error parsing opcode test case file '{filename}', line {lineNo}: {ex.Message}");
				}
				yield return testCase;
			}
		}

		static readonly char[] seps = new char[] { ',' };
		static readonly char[] optsseps = new char[] { ' ' };
		static readonly char[] opseps = new char[] { ';' };
		static OpCodeInfoTestCase ReadTestCase(string line, int lineNo) {
			var parts = line.Split(seps);
			if (parts.Length != 7)
				throw new InvalidOperationException($"Invalid number of commas ({parts.Length - 1} commas)");

			var tc = new OpCodeInfoTestCase();
			tc.LineNumber = lineNo;
			tc.IsInstruction = true;
			tc.GroupIndex = -1;

			tc.Code = ToCode(parts[0].Trim());
			tc.Encoding = ToEncoding(parts[1].Trim());
			tc.MandatoryPrefix = ToMandatoryPrefix(parts[2].Trim());
			tc.Table = ToTable(parts[3].Trim());
			tc.OpCode = ToOpCode(parts[4].Trim());
			tc.OpCodeString = parts[5].Trim();

			foreach (var part in parts[6].Split(optsseps)) {
				var key = part.Trim();
				if (key.Length == 0)
					continue;
				int index = key.IndexOf('=');
				if (index >= 0) {
					var value = key.Substring(index + 1);
					key = key.Substring(0, index);
					switch (key) {
					case "g":
						if (!uint.TryParse(value, out uint groupIndex) || groupIndex > 7)
							throw new InvalidOperationException($"Invalid group index: {value}");
						tc.GroupIndex = (int)groupIndex;
						tc.IsGroup = true;
						break;

					case "op":
						var opParts = value.Split(opseps);
						tc.OpCount = opParts.Length;
						if (opParts.Length >= 1)
							tc.Op0Kind = ToOpCodeOperandKind(opParts[0]);
						if (opParts.Length >= 2)
							tc.Op1Kind = ToOpCodeOperandKind(opParts[1]);
						if (opParts.Length >= 3)
							tc.Op2Kind = ToOpCodeOperandKind(opParts[2]);
						if (opParts.Length >= 4)
							tc.Op3Kind = ToOpCodeOperandKind(opParts[3]);
						if (opParts.Length >= 5)
							tc.Op4Kind = ToOpCodeOperandKind(opParts[4]);
						if (Iced.Intel.DecoderConstants.MaxOpCount != 5)
							throw new InvalidOperationException("Invalid MaxOpCount value");
						if (opParts.Length >= 6)
							throw new InvalidOperationException($"Invalid number of operands: '{value}'");
						break;

					case "tt":
						tc.TupleType = ToTupleType(value.Trim());
						break;

					default:
						throw new InvalidOperationException($"Invalid key: '{key}'");
					}
				}
				else {
					switch (key) {
					case "notinstr":
						tc.IsInstruction = false;
						break;

					case "16b":
						tc.Mode16 = true;
						break;

					case "32b":
						tc.Mode32 = true;
						break;

					case "64b":
						tc.Mode64 = true;
						break;

					case "fwait":
						tc.Fwait = true;
						break;

					case "o16":
						tc.OperandSize = 16;
						break;

					case "o32":
						tc.OperandSize = 32;
						break;

					case "o64":
						tc.OperandSize = 64;
						break;

					case "a16":
						tc.AddressSize = 16;
						break;

					case "a32":
						tc.AddressSize = 32;
						break;

					case "a64":
						tc.AddressSize = 64;
						break;

					case "LIG":
						tc.IsLIG = true;
						break;

					case "L0":
						tc.L = 0;
						break;

					case "L1":
						tc.L = 1;
						break;

					case "L128":
						tc.L = 0;
						break;

					case "L256":
						tc.L = 1;
						break;

					case "L512":
						tc.L = 2;
						break;

					case "WIG":
						tc.IsWIG = true;
						break;

					case "W0":
						tc.W = 0;
						break;

					case "W1":
						tc.W = 1;
						break;

					case "b":
						tc.CanBroadcast = true;
						break;

					case "er":
						tc.CanUseRoundingControl = true;
						break;

					case "sae":
						tc.CanSuppressAllExceptions = true;
						break;

					case "k":
						tc.CanUseOpMaskRegister = true;
						break;

					case "z":
						tc.CanUseZeroingMasking = true;
						break;

					case "lock":
						tc.CanUseLockPrefix = true;
						break;

					case "xacquire":
						tc.CanUseXacquirePrefix = true;
						break;

					case "xrelease":
						tc.CanUseXreleasePrefix = true;
						break;

					case "rep":
					case "repe":
						tc.CanUseRepPrefix = true;
						break;

					case "repne":
						tc.CanUseRepnePrefix = true;
						break;

					case "bnd":
						tc.CanUseBndPrefix = true;
						break;

					case "ht":
						tc.CanUseHintTakenPrefix = true;
						break;

					case "notrack":
						tc.CanUseNotrackPrefix = true;
						break;

					default:
						throw new InvalidOperationException($"Invalid key: '{key}'");
					}
				}
			}

			return tc;
		}

		static Code ToCode(string value) {
			if (!toCode.TryGetValue(value, out var code))
				throw new InvalidOperationException($"Invalid Code value: '{value}'");
			return code;
		}

		static TupleType ToTupleType(string value) {
			if (!toTupleType.TryGetValue(value, out var code))
				throw new InvalidOperationException($"Invalid TupleType value: '{value}'");
			return code;
		}

		static OpCodeOperandKind ToOpCodeOperandKind(string value) {
			if (!toOpCodeOperandKind.TryGetValue(value, out var code))
				throw new InvalidOperationException($"Invalid OpCodeOperandKind value: '{value}'");
			return code;
		}

		static EncodingKind ToEncoding(string value) {
			switch (value) {
			case "legacy":	return EncodingKind.Legacy;
			case "VEX":		return EncodingKind.VEX;
			case "EVEX":	return EncodingKind.EVEX;
			case "XOP":		return EncodingKind.XOP;
			case "3DNow!":	return EncodingKind.D3NOW;
			default: throw new InvalidOperationException($"Invalid encoding value: '{value}'");
			}
		}

		static MandatoryPrefix ToMandatoryPrefix(string value) {
			switch (value) {
			case "":		return MandatoryPrefix.None;
			case "NP":		return MandatoryPrefix.PNP;
			case "66":		return MandatoryPrefix.P66;
			case "F3":		return MandatoryPrefix.PF3;
			case "F2":		return MandatoryPrefix.PF2;
			default: throw new InvalidOperationException($"Invalid mandatory prefix value: '{value}'");
			}
		}

		static OpCodeTableKind ToTable(string value) {
			switch (value) {
			case "legacy":	return OpCodeTableKind.Normal;
			case "0F":		return OpCodeTableKind.T0F;
			case "0F38":	return OpCodeTableKind.T0F38;
			case "0F3A":	return OpCodeTableKind.T0F3A;
			case "X8":		return OpCodeTableKind.XOP8;
			case "X9":		return OpCodeTableKind.XOP9;
			case "XA":		return OpCodeTableKind.XOPA;
			default: throw new InvalidOperationException($"Invalid opcode table value: '{value}'");
			}
		}

		static uint ToOpCode(string value) {
			if (value.Length == 2 || value.Length == 4) {
				if (uint.TryParse(value, System.Globalization.NumberStyles.HexNumber, null, out uint result))
					return result;
			}
			throw new InvalidOperationException($"Invalid opcode: '{value}'");
		}
	}
}
#endif
