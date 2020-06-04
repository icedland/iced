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

#if INSTR_INFO
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Iced.Intel;

namespace Iced.UnitTests.Intel.InstructionInfoTests {
	sealed class InstructionInfoTestReader {
		static readonly char[] commaSeparator = new char[] { ',' };
		static readonly char[] spaceSeparator = new char[] { ' ' };
		static readonly char[] semicolonSeparator = new char[] { ';' };
		static readonly char[] plusSeparator = new char[] { '+' };

		public static IEnumerable<object[]> GetTestCases(int bitness, int stackAddressSize) {
			var toRegister = ToEnumConverter.CloneRegisterDict();
			switch (stackAddressSize) {
			case 16:
				toRegister.Add(MiscInstrInfoTestConstants.XSP, Register.SP);
				toRegister.Add(MiscInstrInfoTestConstants.XBP, Register.BP);
				break;
			case 32:
				toRegister.Add(MiscInstrInfoTestConstants.XSP, Register.ESP);
				toRegister.Add(MiscInstrInfoTestConstants.XBP, Register.EBP);
				break;
			case 64:
				toRegister.Add(MiscInstrInfoTestConstants.XSP, Register.RSP);
				toRegister.Add(MiscInstrInfoTestConstants.XBP, Register.RBP);
				break;
			default:
				throw new InvalidOperationException();
			}

			for (int i = 0; i < IcedConstants.VMM_count; i++)
				toRegister.Add(MiscInstrInfoTestConstants.VMM_prefix + i.ToString(), IcedConstants.VMM_first + i);

			var filename = PathUtils.GetTestTextFilename($"InstructionInfoTest_{bitness}.txt", "InstructionInfo");
			Debug.Assert(File.Exists(filename));
			int lineNo = 0;
			foreach (var line in File.ReadLines(filename)) {
				lineNo++;
				if (line.Length == 0 || line.StartsWith("#"))
					continue;

				(string hexBytes, Code code, DecoderOptions options, InstructionInfoTestCase testCase) info;
				try {
					info = ParseLine(line, bitness, toRegister);
				}
				catch (Exception ex) {
					throw new Exception($"Invalid line {lineNo} ({filename}): {ex.Message}");
				}
				if (info.testCase is object)
					yield return new object[5] { info.hexBytes, info.code, info.options, lineNo, info.testCase };
			}
		}

		static (string hexBytes, Code code, DecoderOptions options, InstructionInfoTestCase testCase) ParseLine(string line, int bitness, Dictionary<string, Register> toRegister) {
			Static.Assert(MiscInstrInfoTestConstants.InstrInfoElemsPerLine == 5 ? 0 : -1);
			var elems = line.Split(commaSeparator, MiscInstrInfoTestConstants.InstrInfoElemsPerLine);
			if (elems.Length != MiscInstrInfoTestConstants.InstrInfoElemsPerLine)
				throw new Exception($"Expected {MiscInstrInfoTestConstants.InstrInfoElemsPerLine - 1} commas");

			var testCase = new InstructionInfoTestCase();

			var hexBytes = ToHexBytes(elems[0].Trim());
			var codeStr = elems[1].Trim();
			if (CodeUtils.IsIgnored(codeStr))
				return default;
			var code = ToEnumConverter.GetCode(codeStr);
			testCase.Encoding = ToEnumConverter.GetEncodingKind(elems[2].Trim());
			var cpuidFeatureStrings = elems[3].Trim().Split(new[] { ';' });

			var cpuidFeatures = new CpuidFeature[cpuidFeatureStrings.Length];
			testCase.CpuidFeatures = cpuidFeatures;
			for (int i = 0; i < cpuidFeatures.Length; i++)
				cpuidFeatures[i] = ToEnumConverter.GetCpuidFeature(cpuidFeatureStrings[i]);

			var options = DecoderOptions.None;
			foreach (var keyValue in elems[4].Split(spaceSeparator, StringSplitOptions.RemoveEmptyEntries)) {
				string key, value;
				int index = keyValue.IndexOf('=');
				if (index >= 0) {
					key = keyValue.Substring(0, index);
					value = keyValue.Substring(index + 1);
				}
				else {
					key = keyValue;
					value = string.Empty;
				}

				switch (key) {
				case InstructionInfoKeys.IsProtectedMode:
					if (value != string.Empty)
						throw new Exception($"Invalid key-value value, '{keyValue}'");
					testCase.IsProtectedMode = true;
					break;

				case InstructionInfoKeys.IsPrivileged:
					if (value != string.Empty)
						throw new Exception($"Invalid key-value value, '{keyValue}'");
					testCase.IsPrivileged = true;
					break;

				case InstructionInfoKeys.IsSaveRestoreInstruction:
					if (value != string.Empty)
						throw new Exception($"Invalid key-value value, '{keyValue}'");
					testCase.IsSaveRestoreInstruction = true;
					break;

				case InstructionInfoKeys.IsStackInstruction:
					if (!int.TryParse(value, out testCase.StackPointerIncrement))
						throw new Exception($"Invalid key-value value, '{keyValue}'");
					testCase.IsStackInstruction = true;
					break;

				case InstructionInfoKeys.IsSpecial:
					if (value != string.Empty)
						throw new Exception($"Invalid key-value value, '{keyValue}'");
					testCase.IsSpecial = true;
					break;

				case InstructionInfoKeys.RflagsRead:
					if (!ParseRflags(value, ref testCase.RflagsRead))
						throw new Exception($"Invalid key-value value, '{keyValue}'");
					break;

				case InstructionInfoKeys.RflagsUndefined:
					if (!ParseRflags(value, ref testCase.RflagsUndefined))
						throw new Exception($"Invalid key-value value, '{keyValue}'");
					break;

				case InstructionInfoKeys.RflagsWritten:
					if (!ParseRflags(value, ref testCase.RflagsWritten))
						throw new Exception($"Invalid key-value value, '{keyValue}'");
					break;

				case InstructionInfoKeys.RflagsCleared:
					if (!ParseRflags(value, ref testCase.RflagsCleared))
						throw new Exception($"Invalid key-value value, '{keyValue}'");
					break;

				case InstructionInfoKeys.RflagsSet:
					if (!ParseRflags(value, ref testCase.RflagsSet))
						throw new Exception($"Invalid key-value value, '{keyValue}'");
					break;

				case InstructionInfoKeys.FlowControl:
					if (!ToEnumConverter.TryFlowControl(value, out testCase.FlowControl))
						throw new Exception($"Invalid key-value value, '{keyValue}'");
					break;

				case InstructionInfoKeys.Op0Access:
					if (!InstructionInfoDicts.ToAccess.TryGetValue(value, out testCase.Op0Access))
						throw new Exception($"Invalid key-value value, '{keyValue}'");
					break;

				case InstructionInfoKeys.Op1Access:
					if (!InstructionInfoDicts.ToAccess.TryGetValue(value, out testCase.Op1Access))
						throw new Exception($"Invalid key-value value, '{keyValue}'");
					break;

				case InstructionInfoKeys.Op2Access:
					if (!InstructionInfoDicts.ToAccess.TryGetValue(value, out testCase.Op2Access))
						throw new Exception($"Invalid key-value value, '{keyValue}'");
					break;

				case InstructionInfoKeys.Op3Access:
					if (!InstructionInfoDicts.ToAccess.TryGetValue(value, out testCase.Op3Access))
						throw new Exception($"Invalid key-value value, '{keyValue}'");
					break;

				case InstructionInfoKeys.Op4Access:
					if (!InstructionInfoDicts.ToAccess.TryGetValue(value, out testCase.Op4Access))
						throw new Exception($"Invalid key-value value, '{keyValue}'");
					break;

				case InstructionInfoKeys.ReadRegister:
					if (!AddRegisters(toRegister, value, OpAccess.Read, testCase))
						throw new Exception($"Invalid key-value value, '{keyValue}'");
					break;

				case InstructionInfoKeys.CondReadRegister:
					if (!AddRegisters(toRegister, value, OpAccess.CondRead, testCase))
						throw new Exception($"Invalid key-value value, '{keyValue}'");
					break;

				case InstructionInfoKeys.WriteRegister:
					if (!AddRegisters(toRegister, value, OpAccess.Write, testCase))
						throw new Exception($"Invalid key-value value, '{keyValue}'");
					break;

				case InstructionInfoKeys.CondWriteRegister:
					if (!AddRegisters(toRegister, value, OpAccess.CondWrite, testCase))
						throw new Exception($"Invalid key-value value, '{keyValue}'");
					break;

				case InstructionInfoKeys.ReadWriteRegister:
					if (!AddRegisters(toRegister, value, OpAccess.ReadWrite, testCase))
						throw new Exception($"Invalid key-value value, '{keyValue}'");
					break;

				case InstructionInfoKeys.ReadCondWriteRegister:
					if (!AddRegisters(toRegister, value, OpAccess.ReadCondWrite, testCase))
						throw new Exception($"Invalid key-value value, '{keyValue}'");
					break;

				case InstructionInfoKeys.ReadMemory:
					if (!AddMemory(bitness, toRegister, value, OpAccess.Read, testCase))
						throw new Exception($"Invalid key-value value, '{keyValue}'");
					break;

				case InstructionInfoKeys.CondReadMemory:
					if (!AddMemory(bitness, toRegister, value, OpAccess.CondRead, testCase))
						throw new Exception($"Invalid key-value value, '{keyValue}'");
					break;

				case InstructionInfoKeys.ReadWriteMemory:
					if (!AddMemory(bitness, toRegister, value, OpAccess.ReadWrite, testCase))
						throw new Exception($"Invalid key-value value, '{keyValue}'");
					break;

				case InstructionInfoKeys.ReadCondWriteMemory:
					if (!AddMemory(bitness, toRegister, value, OpAccess.ReadCondWrite, testCase))
						throw new Exception($"Invalid key-value value, '{keyValue}'");
					break;

				case InstructionInfoKeys.WriteMemory:
					if (!AddMemory(bitness, toRegister, value, OpAccess.Write, testCase))
						throw new Exception($"Invalid key-value value, '{keyValue}'");
					break;

				case InstructionInfoKeys.CondWriteMemory:
					if (!AddMemory(bitness, toRegister, value, OpAccess.CondWrite, testCase))
						throw new Exception($"Invalid key-value value, '{keyValue}'");
					break;

				case InstructionInfoKeys.DecoderOptions:
					if (!TryParseDecoderOptions(value.Split(semicolonSeparator), ref options))
						throw new Exception($"Invalid key-value value, '{keyValue}'");
					break;

				default:
					throw new Exception($"Invalid key-value value, '{keyValue}'");
				}
			}

			return (hexBytes, code, options, testCase);
		}

		static string ToHexBytes(string value) {
			try {
				HexUtils.ToByteArray(value);
			}
			catch {
				throw new InvalidOperationException($"Invalid hex bytes: '{value}'");
			}
			return value;
		}

		static bool TryParseDecoderOptions(string[] stringOptions, ref DecoderOptions options) {
			foreach (var opt in stringOptions) {
				if (!ToEnumConverter.TryDecoderOptions(opt.Trim(), out var decOpts))
					return false;
				options |= decOpts;
			}
			return true;
		}

		static bool AddMemory(int bitness, Dictionary<string, Register> toRegister, string value, OpAccess access, InstructionInfoTestCase testCase) {
			var elems = value.Split(semicolonSeparator);
			if (elems.Length != 2)
				return false;
			var expr = elems[0].Trim();
			if (!ToEnumConverter.TryMemorySize(elems[1].Trim(), out var memorySize))
				return false;

			if (!TryParseMemExpr(toRegister, expr, out var segReg, out var baseReg, out var indexReg, out int scale, out ulong displ))
				return false;

			switch (bitness) {
			case 16:
				if (!(short.MinValue <= (long)displ && (long)displ <= short.MaxValue) && displ > ushort.MaxValue)
					return false;
				displ = (ushort)displ;
				break;

			case 32:
				if (!(int.MinValue <= (long)displ && (long)displ <= int.MaxValue) && displ > uint.MaxValue)
					return false;
				displ = (uint)displ;
				break;

			case 64:
				break;

			default:
				throw new InvalidOperationException();
			}

			if (access != OpAccess.NoMemAccess)
				testCase.UsedMemory.Add(new UsedMemory(segReg, baseReg, indexReg, scale, displ, memorySize, access));

			return true;
		}

		static bool TryParseMemExpr(Dictionary<string, Register> toRegister, string value, out Register segReg, out Register baseReg, out Register indexReg, out int scale, out ulong displ) {
			segReg = Register.None;
			baseReg = Register.None;
			indexReg = Register.None;
			scale = 1;
			displ = 0;

			bool hasBase = false;
			foreach (var tmp in value.Split(plusSeparator)) {
				var s = tmp;
				bool isIndex = hasBase;
				int segIndex = s.IndexOf(":");
				if (segIndex >= 0) {
					var segRegString = s.Substring(0, segIndex);
					s = s.Substring(segIndex + 1);
					if (!toRegister.TryGetValue(segRegString, out segReg))
						return false;
					if (!(Register.ES <= segReg && segReg <= Register.GS))
						return false;
				}
				if (s.IndexOf('*') >= 0) {
					if (s.EndsWith("*1"))
						scale = 1;
					else if (s.EndsWith("*2"))
						scale = 2;
					else if (s.EndsWith("*4"))
						scale = 4;
					else if (s.EndsWith("*8"))
						scale = 8;
					else
						return false;
					s = s.Substring(0, s.Length - 2);
					isIndex = true;
				}
				if (toRegister.TryGetValue(s, out var reg)) {
					if (isIndex)
						indexReg = reg;
					else {
						baseReg = reg;
						hasBase = true;
					}
				}
				else {
					var numString = s;
					if (numString.StartsWith("0x")) {
						numString = numString.Substring(2);
						if (!ulong.TryParse(numString, NumberStyles.HexNumber, null, out displ))
							return false;
					}
					else {
						if (!ulong.TryParse(numString, out displ))
							return false;
					}
				}
			}

			return segReg != Register.None;
		}

		static bool AddRegisters(Dictionary<string, Register> toRegister, string value, OpAccess access, InstructionInfoTestCase testCase) {
			foreach (var tmp in value.Split(semicolonSeparator, StringSplitOptions.RemoveEmptyEntries)) {
				var regString = tmp.Trim();
				if (!toRegister.TryGetValue(regString, out var reg))
					return false;

				if (testCase.Encoding != EncodingKind.Legacy && testCase.Encoding != EncodingKind.D3NOW) {
					switch (access) {
					case OpAccess.None:
					case OpAccess.Read:
					case OpAccess.NoMemAccess:
					case OpAccess.CondRead:
						break;

					case OpAccess.Write:
					case OpAccess.CondWrite:
					case OpAccess.ReadWrite:
					case OpAccess.ReadCondWrite:
						if (Register.XMM0 <= reg && reg <= IcedConstants.VMM_last && !regString.StartsWith(MiscInstrInfoTestConstants.VMM_prefix, StringComparison.OrdinalIgnoreCase))
							throw new Exception($"Register {regString} is written ({access}) but {MiscInstrInfoTestConstants.VMM_prefix} pseudo register should be used instead");
						break;

					default:
						throw new InvalidOperationException();
					}
				}
				testCase.UsedRegisters.Add(new UsedRegister(reg, access));
			}
			return true;
		}

		static bool ParseRflags(string value, ref RflagsBits rflags) {
			foreach (var c in value) {
				switch (c) {
				case RflagsBitsConstants.AF:
					rflags |= RflagsBits.AF;
					break;

				case RflagsBitsConstants.CF:
					rflags |= RflagsBits.CF;
					break;

				case RflagsBitsConstants.OF:
					rflags |= RflagsBits.OF;
					break;

				case RflagsBitsConstants.PF:
					rflags |= RflagsBits.PF;
					break;

				case RflagsBitsConstants.SF:
					rflags |= RflagsBits.SF;
					break;

				case RflagsBitsConstants.ZF:
					rflags |= RflagsBits.ZF;
					break;

				case RflagsBitsConstants.IF:
					rflags |= RflagsBits.IF;
					break;

				case RflagsBitsConstants.DF:
					rflags |= RflagsBits.DF;
					break;

				case RflagsBitsConstants.AC:
					rflags |= RflagsBits.AC;
					break;

				default:
					return false;
				}
			}

			return true;
		}
	}
}
#endif
