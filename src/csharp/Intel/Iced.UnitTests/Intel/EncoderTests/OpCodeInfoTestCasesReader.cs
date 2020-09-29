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

#if ENCODER && OPCODE_INFO
using System;
using System.Collections.Generic;
using System.IO;
using Iced.Intel;

namespace Iced.UnitTests.Intel.EncoderTests {
	static class OpCodeInfoTestCasesReader {
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
				if (testCase is object)
					yield return testCase;
			}
		}

		static readonly char[] seps = new char[] { ',' };
		static readonly char[] optsseps = new char[] { ' ' };
		static readonly char[] opseps = new char[] { ';' };
		static OpCodeInfoTestCase ReadTestCase(string line, int lineNo) {
			var parts = line.Split(seps);
			if (parts.Length != 8)
				throw new InvalidOperationException($"Invalid number of commas ({parts.Length - 1} commas)");

			var tc = new OpCodeInfoTestCase();
			tc.LineNumber = lineNo;
			tc.IsInstruction = true;
			tc.GroupIndex = -1;

			var code = parts[0].Trim();
			if (CodeUtils.IsIgnored(code))
				return null;
			tc.Code = ToCode(code);
			tc.Encoding = ToEncoding(parts[1].Trim());
			tc.MandatoryPrefix = ToMandatoryPrefix(parts[2].Trim());
			tc.Table = ToTable(parts[3].Trim());
			tc.OpCode = ToOpCode(parts[4].Trim());
			tc.OpCodeString = parts[5].Trim();
			tc.InstructionString = parts[6].Trim().Replace('|', ',');

			bool gotVectorLength = false;
			bool gotW = false;
			foreach (var part in parts[7].Split(optsseps)) {
				var key = part.Trim();
				if (key.Length == 0)
					continue;
				int index = key.IndexOf('=');
				if (index >= 0) {
					var value = key.Substring(index + 1);
					key = key.Substring(0, index);
					switch (key) {
					case OpCodeInfoKeys.GroupIndex:
						if (!uint.TryParse(value, out uint groupIndex) || groupIndex > 7)
							throw new InvalidOperationException($"Invalid group index: {value}");
						tc.GroupIndex = (int)groupIndex;
						tc.IsGroup = true;
						break;

					case OpCodeInfoKeys.RmGroupIndex:
						if (!uint.TryParse(value, out uint rmGroupIndex) || rmGroupIndex > 7)
							throw new InvalidOperationException($"Invalid group index: {value}");
						tc.RmGroupIndex = (int)rmGroupIndex;
						tc.IsRmGroup = true;
						break;

					case OpCodeInfoKeys.OpCodeOperandKind:
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
						Static.Assert(IcedConstants.MaxOpCount == 5 ? 0 : -1);
						if (opParts.Length >= 6)
							throw new InvalidOperationException($"Invalid number of operands: '{value}'");
						break;

					case OpCodeInfoKeys.TupleType:
						tc.TupleType = ToTupleType(value.Trim());
						break;

					default:
						throw new InvalidOperationException($"Invalid key: '{key}'");
					}
				}
				else {
					switch (key) {
					case OpCodeInfoFlags.NotInstruction:
						tc.IsInstruction = false;
						break;

					case OpCodeInfoFlags.Bit16:
						tc.Mode16 = true;
						break;

					case OpCodeInfoFlags.Bit32:
						tc.Mode32 = true;
						break;

					case OpCodeInfoFlags.Bit64:
						tc.Mode64 = true;
						break;

					case OpCodeInfoFlags.Fwait:
						tc.Fwait = true;
						break;

					case OpCodeInfoFlags.OperandSize16:
						tc.OperandSize = 16;
						break;

					case OpCodeInfoFlags.OperandSize32:
						tc.OperandSize = 32;
						break;

					case OpCodeInfoFlags.OperandSize64:
						tc.OperandSize = 64;
						break;

					case OpCodeInfoFlags.AddressSize16:
						tc.AddressSize = 16;
						break;

					case OpCodeInfoFlags.AddressSize32:
						tc.AddressSize = 32;
						break;

					case OpCodeInfoFlags.AddressSize64:
						tc.AddressSize = 64;
						break;

					case OpCodeInfoFlags.LIG:
						tc.IsLIG = true;
						gotVectorLength = true;
						break;

					case OpCodeInfoFlags.L0:
						tc.L = 0;
						gotVectorLength = true;
						break;

					case OpCodeInfoFlags.L1:
						tc.L = 1;
						gotVectorLength = true;
						break;

					case OpCodeInfoFlags.L128:
						tc.L = 0;
						gotVectorLength = true;
						break;

					case OpCodeInfoFlags.L256:
						tc.L = 1;
						gotVectorLength = true;
						break;

					case OpCodeInfoFlags.L512:
						tc.L = 2;
						gotVectorLength = true;
						break;

					case OpCodeInfoFlags.WIG:
						tc.IsWIG = true;
						gotW = true;
						break;

					case OpCodeInfoFlags.WIG32:
						tc.W = 0;
						tc.IsWIG32 = true;
						gotW = true;
						break;

					case OpCodeInfoFlags.W0:
						tc.W = 0;
						gotW = true;
						break;

					case OpCodeInfoFlags.W1:
						tc.W = 1;
						gotW = true;
						break;

					case OpCodeInfoFlags.Broadcast:
						tc.CanBroadcast = true;
						break;

					case OpCodeInfoFlags.RoundingControl:
						tc.CanUseRoundingControl = true;
						break;

					case OpCodeInfoFlags.SuppressAllExceptions:
						tc.CanSuppressAllExceptions = true;
						break;

					case OpCodeInfoFlags.OpMaskRegister:
						tc.CanUseOpMaskRegister = true;
						break;

					case OpCodeInfoFlags.RequireOpMaskRegister:
						tc.CanUseOpMaskRegister = true;
						tc.RequireOpMaskRegister = true;
						break;

					case OpCodeInfoFlags.ZeroingMasking:
						tc.CanUseZeroingMasking = true;
						break;

					case OpCodeInfoFlags.LockPrefix:
						tc.CanUseLockPrefix = true;
						break;

					case OpCodeInfoFlags.XacquirePrefix:
						tc.CanUseXacquirePrefix = true;
						break;

					case OpCodeInfoFlags.XreleasePrefix:
						tc.CanUseXreleasePrefix = true;
						break;

					case OpCodeInfoFlags.RepPrefix:
					case OpCodeInfoFlags.RepePrefix:
						tc.CanUseRepPrefix = true;
						break;

					case OpCodeInfoFlags.RepnePrefix:
						tc.CanUseRepnePrefix = true;
						break;

					case OpCodeInfoFlags.BndPrefix:
						tc.CanUseBndPrefix = true;
						break;

					case OpCodeInfoFlags.HintTakenPrefix:
						tc.CanUseHintTakenPrefix = true;
						break;

					case OpCodeInfoFlags.NotrackPrefix:
						tc.CanUseNotrackPrefix = true;
						break;

					default:
						throw new InvalidOperationException($"Invalid key: '{key}'");
					}
				}
			}
			switch (tc.Encoding) {
			case EncodingKind.Legacy:
			case EncodingKind.D3NOW:
				break;
			case EncodingKind.VEX:
			case EncodingKind.EVEX:
			case EncodingKind.XOP:
				if (!gotVectorLength)
					throw new InvalidOperationException("Missing vector length: L0/L1/L128/L256/L512/LIG");
				if (!gotW)
					throw new InvalidOperationException("Missing W bit: W0/W1/WIG/WIG32");
				break;
			default:
				throw new InvalidOperationException();
			}

			return tc;
		}

		static Code ToCode(string value) {
			if (!ToEnumConverter.TryCode(value, out var code))
				throw new InvalidOperationException($"Invalid Code value: '{value}'");
			return code;
		}

		static TupleType ToTupleType(string value) {
			if (!ToEnumConverter.TryTupleType(value, out var code))
				throw new InvalidOperationException($"Invalid TupleType value: '{value}'");
			return code;
		}

		static OpCodeOperandKind ToOpCodeOperandKind(string value) {
			if (!ToEnumConverter.TryOpCodeOperandKind(value, out var code))
				throw new InvalidOperationException($"Invalid OpCodeOperandKind value: '{value}'");
			return code;
		}

		static EncodingKind ToEncoding(string value) {
			if (OpCodeInfoDicts.ToEncodingKind.TryGetValue(value, out var kind))
				return kind;
			throw new InvalidOperationException($"Invalid encoding value: '{value}'");
		}

		static MandatoryPrefix ToMandatoryPrefix(string value) {
			if (OpCodeInfoDicts.ToMandatoryPrefix.TryGetValue(value, out var prefix))
				return prefix;
			throw new InvalidOperationException($"Invalid mandatory prefix value: '{value}'");
		}

		static OpCodeTableKind ToTable(string value) {
			if (OpCodeInfoDicts.ToOpCodeTableKind.TryGetValue(value, out var kind))
				return kind;
			throw new InvalidOperationException($"Invalid opcode table value: '{value}'");
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
