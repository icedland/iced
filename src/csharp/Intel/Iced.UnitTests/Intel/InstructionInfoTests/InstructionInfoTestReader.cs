// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

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
				if (line.Length == 0 || line[0] == '#')
					continue;

				(string hexBytes, Code code, DecoderOptions options, InstructionInfoTestCase testCase) info;
				try {
					info = ParseLine(line, bitness, toRegister);
				}
				catch (Exception ex) {
					throw new Exception($"Invalid line {lineNo} ({filename}): {ex.Message}");
				}
				if (info.testCase is not null)
					yield return new object[5] { info.hexBytes, info.code, info.options, lineNo, info.testCase };
			}
		}

		static (string hexBytes, Code code, DecoderOptions options, InstructionInfoTestCase testCase) ParseLine(string line, int bitness, Dictionary<string, Register> toRegister) {
			Static.Assert(MiscInstrInfoTestConstants.InstrInfoElemsPerLine == 5 ? 0 : -1);
			var elems = line.Split(commaSeparator, MiscInstrInfoTestConstants.InstrInfoElemsPerLine);
			if (elems.Length != MiscInstrInfoTestConstants.InstrInfoElemsPerLine)
				throw new Exception($"Expected {MiscInstrInfoTestConstants.InstrInfoElemsPerLine - 1} commas");

			var testCase = new InstructionInfoTestCase();
			testCase.IP = bitness switch {
				16 => DecoderConstants.DEFAULT_IP16,
				32 => DecoderConstants.DEFAULT_IP32,
				64 => DecoderConstants.DEFAULT_IP64,
				_ => throw new InvalidOperationException(),
			};

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

				case InstructionInfoKeys.FpuTopIncrement:
					testCase.FpuTopIncrement = NumberConverter.ToInt32(value);
					testCase.FpuWritesTop = true;
					break;

				case InstructionInfoKeys.FpuConditionalTop:
					if (value != string.Empty)
						throw new Exception($"Invalid key-value value, '{keyValue}'");
					testCase.FpuConditionalTop = true;
					break;

				case InstructionInfoKeys.FpuWritesTop:
					if (value != string.Empty)
						throw new Exception($"Invalid key-value value, '{keyValue}'");
					testCase.FpuWritesTop = true;
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

			if (!TryParseMemExpr(toRegister, expr, bitness, out var segReg, out var baseReg, out var indexReg, out int scale, out ulong displ, out var addressSize, out var vsibSize))
				return false;

			switch (addressSize) {
			case CodeSize.Code16:
				if (!(short.MinValue <= (long)displ && (long)displ <= short.MaxValue) && displ > ushort.MaxValue)
					return false;
				displ = (ushort)displ;
				break;

			case CodeSize.Code32:
				if (!(int.MinValue <= (long)displ && (long)displ <= int.MaxValue) && displ > uint.MaxValue)
					return false;
				displ = (uint)displ;
				break;

			case CodeSize.Code64:
				break;

			default:
				throw new InvalidOperationException();
			}

			if (access != OpAccess.NoMemAccess)
				testCase.UsedMemory.Add(new UsedMemory(segReg, baseReg, indexReg, scale, displ, memorySize, access, addressSize, vsibSize));

			return true;
		}

		static bool TryParseMemExpr(Dictionary<string, Register> toRegister, string value, int bitness, out Register segReg, out Register baseReg, out Register indexReg, out int scale, out ulong displ, out CodeSize addressSize, out int vsibSize) {
			segReg = Register.None;
			baseReg = Register.None;
			indexReg = Register.None;
			scale = 1;
			displ = 0;
			addressSize = CodeSize.Unknown;
			vsibSize = 0;

			var memArgs = value.Split('|');
			value = memArgs[0];
			for (int i = 1; i < memArgs.Length; i++) {
				var option = memArgs[i];
				switch (option) {
				case MiscInstrInfoTestConstants.MemSizeOption_Addr16: addressSize = CodeSize.Code16; break;
				case MiscInstrInfoTestConstants.MemSizeOption_Addr32: addressSize = CodeSize.Code32; break;
				case MiscInstrInfoTestConstants.MemSizeOption_Addr64: addressSize = CodeSize.Code64; break;
				case MiscInstrInfoTestConstants.MemSizeOption_Vsib32: vsibSize = 4; break;
				case MiscInstrInfoTestConstants.MemSizeOption_Vsib64: vsibSize = 8; break;
				default: return false;
				}
			}

			bool hasBase = false;
			foreach (var tmp in value.Split(plusSeparator)) {
				var s = tmp;
				bool isIndex = hasBase;
				int segIndex = s.IndexOf(":", StringComparison.Ordinal);
				if (segIndex >= 0) {
					var segRegString = s.Substring(0, segIndex);
					s = s.Substring(segIndex + 1);
					if (!toRegister.TryGetValue(segRegString, out segReg))
						return false;
					if (!(Register.ES <= segReg && segReg <= Register.GS))
						return false;
				}
				if (s.IndexOf('*') >= 0) {
					if (s.EndsWith("*1", StringComparison.Ordinal))
						scale = 1;
					else if (s.EndsWith("*2", StringComparison.Ordinal))
						scale = 2;
					else if (s.EndsWith("*4", StringComparison.Ordinal))
						scale = 4;
					else if (s.EndsWith("*8", StringComparison.Ordinal))
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
					if (numString.StartsWith("0x", StringComparison.Ordinal)) {
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

			if (addressSize == CodeSize.Unknown) {
				var reg = baseReg != Register.None ? baseReg : indexReg;
				if (reg.IsGPR16())
					addressSize = CodeSize.Code16;
				else if (reg.IsGPR32())
					addressSize = CodeSize.Code32;
				else if (reg.IsGPR64())
					addressSize = CodeSize.Code64;
			}
			if (addressSize == CodeSize.Unknown) {
				addressSize = bitness switch {
					16 => CodeSize.Code16,
					32 => CodeSize.Code32,
					64 => CodeSize.Code64,
					_ => throw new InvalidOperationException(),
				};
			}
			if (vsibSize == 0 && indexReg.IsVectorRegister())
				return false;
			if (vsibSize != 0 && !indexReg.IsVectorRegister())
				return false;

			return segReg != Register.None;
		}

		static bool TrySplit(string value, char sep, out string left, out string right) {
			int index = value.IndexOf(sep);
			if (index >= 0) {
				left = value.Substring(0, index).Trim();
				right = value.Substring(index + 1).Trim();
				return true;
			}
			else {
				left = null;
				right = null;
				return false;
			}
		}

		static bool TryGetRegister(Dictionary<string, Register> toRegister, string regString, EncodingKind encoding, OpAccess access, out Register register) {
			if (!toRegister.TryGetValue(regString, out register))
				return false;

			if (encoding != EncodingKind.Legacy && encoding != EncodingKind.D3NOW) {
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
					if (Register.XMM0 <= register && register <= IcedConstants.VMM_last && !regString.StartsWith(MiscInstrInfoTestConstants.VMM_prefix, StringComparison.OrdinalIgnoreCase))
						throw new Exception($"Register {regString} is written ({access}) but {MiscInstrInfoTestConstants.VMM_prefix} pseudo register should be used instead");
					break;

				default:
					throw new InvalidOperationException();
				}
			}

			return true;
		}

		static bool AddRegisters(Dictionary<string, Register> toRegister, string value, OpAccess access, InstructionInfoTestCase testCase) {
			foreach (var tmp in value.Split(semicolonSeparator, StringSplitOptions.RemoveEmptyEntries)) {
				var regString = tmp.Trim();
				if (TrySplit(regString, '-', out var firstRegStr, out var lastRegStr)) {
					if (!TryGetRegister(toRegister, firstRegStr, testCase.Encoding, access, out var firstReg))
						return false;
					if (!TryGetRegister(toRegister, lastRegStr, testCase.Encoding, access, out var lastReg))
						return false;
					if (lastReg < firstReg)
						throw new Exception($"Invalid register range: {regString}");
					for (var reg = firstReg; reg <= lastReg; reg++)
						testCase.UsedRegisters.Add(new UsedRegister(reg, access));
				}
				else {
					if (!TryGetRegister(toRegister, regString, testCase.Encoding, access, out var register))
						return false;
					testCase.UsedRegisters.Add(new UsedRegister(register, access));
				}
			}
			return true;
		}

		static bool ParseRflags(string value, ref RflagsBits rflags) {
			foreach (var c in value) {
				switch (c) {
				case RflagsBitsConstants.AF: rflags |= RflagsBits.AF; break;
				case RflagsBitsConstants.CF: rflags |= RflagsBits.CF; break;
				case RflagsBitsConstants.OF: rflags |= RflagsBits.OF; break;
				case RflagsBitsConstants.PF: rflags |= RflagsBits.PF; break;
				case RflagsBitsConstants.SF: rflags |= RflagsBits.SF; break;
				case RflagsBitsConstants.ZF: rflags |= RflagsBits.ZF; break;
				case RflagsBitsConstants.IF: rflags |= RflagsBits.IF; break;
				case RflagsBitsConstants.DF: rflags |= RflagsBits.DF; break;
				case RflagsBitsConstants.AC: rflags |= RflagsBits.AC; break;
				case RflagsBitsConstants.C0: rflags |= RflagsBits.C0; break;
				case RflagsBitsConstants.C1: rflags |= RflagsBits.C1; break;
				case RflagsBitsConstants.C2: rflags |= RflagsBits.C2; break;
				case RflagsBitsConstants.C3: rflags |= RflagsBits.C3; break;
				case RflagsBitsConstants.UIF: rflags |= RflagsBits.UIF; break;
				default: return false;
				}
			}

			return true;
		}
	}
}
#endif
