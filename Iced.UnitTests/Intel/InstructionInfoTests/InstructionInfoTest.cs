/*
    Copyright (C) 2018 de4dot@gmail.com

    This file is part of Iced.

    Iced is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Iced is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Iced.  If not, see <https://www.gnu.org/licenses/>.
*/

#if !NO_INSTR_INFO
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.InstructionInfoTests {
	sealed class UsedRegisterEqualityComparer : IEqualityComparer<UsedRegister> {
		public static readonly UsedRegisterEqualityComparer Instance = new UsedRegisterEqualityComparer();
		UsedRegisterEqualityComparer() { }

		public bool Equals(UsedRegister x, UsedRegister y) =>
			x.Register == y.Register && x.Access == y.Access;

		public int GetHashCode(UsedRegister obj) =>
			(int)obj.Register ^ (int)obj.Access;
	}

	sealed class UsedMemoryEqualityComparer : IEqualityComparer<UsedMemory> {
		public static readonly UsedMemoryEqualityComparer Instance = new UsedMemoryEqualityComparer();
		UsedMemoryEqualityComparer() { }

		public bool Equals(UsedMemory x, UsedMemory y) =>
			x.Segment == y.Segment &&
			x.Base == y.Base &&
			x.Index == y.Index &&
			x.Scale == y.Scale &&
			x.Displacement == y.Displacement &&
			x.MemorySize == y.MemorySize &&
			x.Access == y.Access;

		public int GetHashCode(UsedMemory obj) {
			int hc = 0;
			hc ^= (int)obj.Segment;
			hc ^= (int)obj.Base << 8;
			hc ^= (int)obj.Index << 16;
			hc ^= obj.Scale << 28;
			hc ^= obj.Displacement.GetHashCode();
			hc ^= (int)obj.MemorySize << 12;
			hc ^= (int)obj.Access << 24;
			return hc;
		}
	}

	public abstract class InstructionInfoTest {
		protected void TestInstructionInfo(int bitness, string hexBytes, Code code, int lineNo, InstructionInfoTestCase testCase) {
			var decoder = CreateDecoder(bitness, hexBytes);
			var instr = decoder.Decode();
			Assert.Equal(code, instr.Code);

			Assert.Equal(testCase.StackPointerIncrement, instr.StackPointerIncrement);

			var info = instr.GetInfo();
			Assert.Equal(testCase.Encoding, info.Encoding);
			Assert.Equal(testCase.CpuidFeature, info.CpuidFeature);
			Assert.Equal(testCase.RflagsRead, info.RflagsRead);
			Assert.Equal(testCase.RflagsUndefined, info.RflagsUndefined);
			Assert.Equal(testCase.RflagsWritten, info.RflagsWritten);
			Assert.Equal(testCase.RflagsCleared, info.RflagsCleared);
			Assert.Equal(testCase.RflagsSet, info.RflagsSet);
			Assert.Equal(testCase.Privileged, info.Privileged);
			Assert.Equal(testCase.ProtectedMode, info.ProtectedMode);
			Assert.Equal(testCase.StackInstruction, info.StackInstruction);
			Assert.Equal(testCase.SaveRestoreInstruction, info.SaveRestoreInstruction);
			Assert.Equal(testCase.FlowControl, info.FlowControl);
			Assert.Equal(testCase.Op0Access, info.Op0Access);
			Assert.Equal(testCase.Op1Access, info.Op1Access);
			Assert.Equal(testCase.Op2Access, info.Op2Access);
			Assert.Equal(testCase.Op3Access, info.Op3Access);
			Assert.Equal(testCase.Op4Access, info.Op4Access);
			Assert.Equal(
				new HashSet<UsedMemory>(testCase.UsedMemory, UsedMemoryEqualityComparer.Instance),
				new HashSet<UsedMemory>(info.GetUsedMemory(), UsedMemoryEqualityComparer.Instance));
			Assert.Equal(
				new HashSet<UsedRegister>(GetUsedRegisters(testCase.UsedRegisters), UsedRegisterEqualityComparer.Instance),
				new HashSet<UsedRegister>(GetUsedRegisters(info.GetUsedRegisters()), UsedRegisterEqualityComparer.Instance));
			Assert.Equal(info.GetUsedMemory(), instr.GetUsedMemory(), UsedMemoryEqualityComparer.Instance);
			Assert.Equal(info.GetUsedRegisters(), instr.GetUsedRegisters(), UsedRegisterEqualityComparer.Instance);

			Debug.Assert(Iced.Intel.DecoderConstants.MaxOpCount == 5);
			Debug.Assert(instr.OpCount <= Iced.Intel.DecoderConstants.MaxOpCount);
			for (int i = 0; i < instr.OpCount; i++) {
				switch (i) {
				case 0:
					Assert.Equal(testCase.Op0Access, info.GetOpAccess(i));
					break;

				case 1:
					Assert.Equal(testCase.Op1Access, info.GetOpAccess(i));
					break;

				case 2:
					Assert.Equal(testCase.Op2Access, info.GetOpAccess(i));
					break;

				case 3:
					Assert.Equal(testCase.Op3Access, info.GetOpAccess(i));
					break;

				case 4:
					Assert.Equal(testCase.Op4Access, info.GetOpAccess(i));
					break;

				default:
					throw new InvalidOperationException();
				}
			}
			for (int i = instr.OpCount; i < Iced.Intel.DecoderConstants.MaxOpCount; i++)
				Assert.Equal(OpAccess.None, info.GetOpAccess(i));

			Assert.Equal(RflagsBits.None, info.RflagsWritten & (info.RflagsCleared | info.RflagsSet | info.RflagsUndefined));
			Assert.Equal(RflagsBits.None, info.RflagsCleared & (info.RflagsWritten | info.RflagsSet | info.RflagsUndefined));
			Assert.Equal(RflagsBits.None, info.RflagsSet & (info.RflagsWritten | info.RflagsCleared | info.RflagsUndefined));
			Assert.Equal(RflagsBits.None, info.RflagsUndefined & (info.RflagsWritten | info.RflagsCleared | info.RflagsSet));
			Assert.Equal(info.RflagsWritten | info.RflagsCleared | info.RflagsSet | info.RflagsUndefined, info.RflagsModified);

			var info2 = new InstructionInfoFactory().GetInfo(ref instr);
			CheckEqual(ref info, ref info2, hasRegs2: true, hasMem2: true);
			info2 = new InstructionInfoFactory().GetInfo(ref instr, InstructionInfoOptions.NoMemoryUsage);
			CheckEqual(ref info, ref info2, hasRegs2: true, hasMem2: false);
			info2 = new InstructionInfoFactory().GetInfo(ref instr, InstructionInfoOptions.NoRegisterUsage);
			CheckEqual(ref info, ref info2, hasRegs2: false, hasMem2: true);
			info2 = new InstructionInfoFactory().GetInfo(ref instr, InstructionInfoOptions.NoRegisterUsage | InstructionInfoOptions.NoMemoryUsage);
			CheckEqual(ref info, ref info2, hasRegs2: false, hasMem2: false);

			Assert.Equal(info.Encoding, instr.Code.Encoding());
			var cf = instr.Code.CpuidFeature();
			if (cf == CpuidFeature.AVX && instr.Op1Kind == OpKind.Register && (code == Code.VEX_Vbroadcastss_xmm_xmmm32 || code == Code.VEX_Vbroadcastss_ymm_xmmm32 || code == Code.VEX_Vbroadcastsd_ymm_xmmm64))
				cf = CpuidFeature.AVX2;
			Assert.Equal(info.CpuidFeature, cf);
			Assert.Equal(info.FlowControl, instr.Code.FlowControl());
			Assert.Equal(info.ProtectedMode, instr.Code.ProtectedMode());
			Assert.Equal(info.Privileged, instr.Code.Privileged());
			Assert.Equal(info.StackInstruction, instr.Code.StackInstruction());
			Assert.Equal(info.SaveRestoreInstruction, instr.Code.SaveRestoreInstruction());

			Assert.Equal(info.Encoding, instr.Encoding);
			Assert.Equal(info.CpuidFeature, instr.CpuidFeature);
			Assert.Equal(info.FlowControl, instr.FlowControl);
			Assert.Equal(info.ProtectedMode, instr.ProtectedMode);
			Assert.Equal(info.Privileged, instr.Privileged);
			Assert.Equal(info.StackInstruction, instr.StackInstruction);
			Assert.Equal(info.SaveRestoreInstruction, instr.SaveRestoreInstruction);
			Assert.Equal(info.RflagsRead, instr.RflagsRead);
			Assert.Equal(info.RflagsWritten, instr.RflagsWritten);
			Assert.Equal(info.RflagsCleared, instr.RflagsCleared);
			Assert.Equal(info.RflagsSet, instr.RflagsSet);
			Assert.Equal(info.RflagsUndefined, instr.RflagsUndefined);
			Assert.Equal(info.RflagsModified, instr.RflagsModified);
		}

		void CheckEqual(ref InstructionInfo info1, ref InstructionInfo info2, bool hasRegs2, bool hasMem2) {
			if (hasRegs2)
				Assert.Equal(info1.GetUsedRegisters(), info2.GetUsedRegisters(), UsedRegisterEqualityComparer.Instance);
			else
				Assert.Empty(info2.GetUsedRegisters());
			if (hasMem2)
				Assert.Equal(info1.GetUsedMemory(), info2.GetUsedMemory(), UsedMemoryEqualityComparer.Instance);
			else
				Assert.Empty(info2.GetUsedMemory());
			Assert.Equal(info1.ProtectedMode, info2.ProtectedMode);
			Assert.Equal(info1.Privileged, info2.Privileged);
			Assert.Equal(info1.StackInstruction, info2.StackInstruction);
			Assert.Equal(info1.SaveRestoreInstruction, info2.SaveRestoreInstruction);
			Assert.Equal(info1.Encoding, info2.Encoding);
			Assert.Equal(info1.CpuidFeature, info2.CpuidFeature);
			Assert.Equal(info1.FlowControl, info2.FlowControl);
			Assert.Equal(info1.Op0Access, info2.Op0Access);
			Assert.Equal(info1.Op1Access, info2.Op1Access);
			Assert.Equal(info1.Op2Access, info2.Op2Access);
			Assert.Equal(info1.Op3Access, info2.Op3Access);
			Assert.Equal(info1.Op4Access, info2.Op4Access);
			Assert.Equal(info1.RflagsRead, info2.RflagsRead);
			Assert.Equal(info1.RflagsWritten, info2.RflagsWritten);
			Assert.Equal(info1.RflagsCleared, info2.RflagsCleared);
			Assert.Equal(info1.RflagsSet, info2.RflagsSet);
			Assert.Equal(info1.RflagsUndefined, info2.RflagsUndefined);
			Assert.Equal(info1.RflagsModified, info2.RflagsModified);
		}

		IEnumerable<UsedRegister> GetUsedRegisters(IEnumerable<UsedRegister> usedRegisterIterator) {
			var read = new List<Register>();
			var write = new List<Register>();
			var condRead = new List<Register>();
			var condWrite = new List<Register>();

			foreach (var info in usedRegisterIterator) {
				switch (info.Access) {
				case OpAccess.Read:
					read.Add(info.Register);
					break;

				case OpAccess.CondRead:
					condRead.Add(info.Register);
					break;

				case OpAccess.Write:
					write.Add(info.Register);
					break;

				case OpAccess.CondWrite:
					condWrite.Add(info.Register);
					break;

				case OpAccess.ReadWrite:
					read.Add(info.Register);
					write.Add(info.Register);
					break;

				case OpAccess.ReadCondWrite:
					read.Add(info.Register);
					condWrite.Add(info.Register);
					break;

				case OpAccess.None:
				case OpAccess.NoMemAccess:
				default:
					throw new InvalidOperationException();
				}
			}

			foreach (var reg in GetRegisters(read))
				yield return new UsedRegister(reg, OpAccess.Read);
			foreach (var reg in GetRegisters(write))
				yield return new UsedRegister(reg, OpAccess.Write);
			foreach (var reg in GetRegisters(condRead))
				yield return new UsedRegister(reg, OpAccess.CondRead);
			foreach (var reg in GetRegisters(condWrite))
				yield return new UsedRegister(reg, OpAccess.CondWrite);
		}

		IEnumerable<Register> GetRegisters(List<Register> regs) {
			if (regs.Count <= 1)
				return regs;

			regs.Sort(RegisterSorter);

			var hash = new HashSet<Register>();
			int index;
			foreach (var reg in regs) {
				if (Register.EAX <= reg && reg <= Register.R15D) {
					index = reg - Register.EAX;
					if (hash.Contains(Register.RAX + index))
						continue;
				}
				else if (Register.AX <= reg && reg <= Register.R15W) {
					index = reg - Register.AX;
					if (hash.Contains(Register.RAX + index))
						continue;
					if (hash.Contains(Register.EAX + index))
						continue;
				}
				else if (Register.AL <= reg && reg <= Register.R15L) {
					index = reg - Register.AL;
					if (Register.AH <= reg && reg <= Register.BH)
						index -= 4;
					if (hash.Contains(Register.RAX + index))
						continue;
					if (hash.Contains(Register.EAX + index))
						continue;
					if (hash.Contains(Register.AX + index))
						continue;
				}
				else if (Register.YMM0 <= reg && reg <= Register.YMM0 + InstructionInfoConstants.VMM_count - 1) {
					index = reg - Register.YMM0;
					if (hash.Contains(Register.ZMM0 + index))
						continue;
				}
				else if (Register.XMM0 <= reg && reg <= Register.XMM0 + InstructionInfoConstants.VMM_count - 1) {
					index = reg - Register.XMM0;
					if (hash.Contains(Register.ZMM0 + index))
						continue;
					if (hash.Contains(Register.YMM0 + index))
						continue;
				}
				hash.Add(reg);
			}

			foreach (var info in lowRegs) {
				if (hash.Contains(info.rl) && hash.Contains(info.rh)) {
					hash.Remove(info.rl);
					hash.Remove(info.rh);
					hash.Add(info.rx);
				}
			}

			return hash;
		}
		static readonly (Register rl, Register rh, Register rx)[] lowRegs = new(Register rl, Register rh, Register rx)[4] {
			(Register.AL, Register.AH, Register.AX),
			(Register.CL, Register.CH, Register.CX),
			(Register.DL, Register.DH, Register.DX),
			(Register.BL, Register.BH, Register.BX),
		};

		static int RegisterSorter(Register x, Register y) {
			int c = GetRegisterGroupOrder(x) - GetRegisterGroupOrder(y);
			if (c != 0)
				return c;
			return x - y;
		}

		static int GetRegisterGroupOrder(Register reg) {
			if (Register.RAX <= reg && reg <= Register.R15)
				return 0;
			if (Register.EAX <= reg && reg <= Register.R15D)
				return 1;
			if (Register.AX <= reg && reg <= Register.R15W)
				return 2;
			if (Register.AL <= reg && reg <= Register.R15L)
				return 3;

			if (Register.ZMM0 <= reg && reg <= Register.ZMM0 + InstructionInfoConstants.VMM_count - 1)
				return 4;
			if (Register.YMM0 <= reg && reg <= Register.YMM0 + InstructionInfoConstants.VMM_count - 1)
				return 5;
			if (Register.XMM0 <= reg && reg <= Register.XMM0 + InstructionInfoConstants.VMM_count - 1)
				return 6;

			return -1;
		}

		Decoder CreateDecoder(int codeSize, string hexBytes) {
			var codeReader = new ByteArrayCodeReader(hexBytes);
			var decoder = Decoder.Create(codeSize, codeReader);

			switch (codeSize) {
			case 16:
				decoder.InstructionPointer = DecoderConstants.DEFAULT_IP16;
				break;

			case 32:
				decoder.InstructionPointer = DecoderConstants.DEFAULT_IP32;
				break;

			case 64:
				decoder.InstructionPointer = DecoderConstants.DEFAULT_IP64;
				break;

			default:
				throw new ArgumentOutOfRangeException(nameof(codeSize));
			}

			Assert.Equal(codeSize, decoder.Bitness);
			return decoder;
		}

		static Dictionary<string, Code> CreateEnumDictCode() {
			var names = Enum.GetNames(typeof(Code));
			var dict = new Dictionary<string, Code>(names.Length, StringComparer.Ordinal);
			foreach (Code value in Enum.GetValues(typeof(Code))) {
				Debug.Assert(names[(int)value] == value.ToString());
				dict[names[(int)value]] = value;
			}
			return dict;
		}

		static Dictionary<string, EncodingKind> CreateEnumDictEncodingKind() {
			var names = Enum.GetNames(typeof(EncodingKind));
			var dict = new Dictionary<string, EncodingKind>(names.Length, StringComparer.Ordinal);
			foreach (EncodingKind value in Enum.GetValues(typeof(EncodingKind))) {
				Debug.Assert(names[(int)value] == value.ToString());
				dict[names[(int)value]] = value;
			}
			return dict;
		}

		static Dictionary<string, CpuidFeature> CreateEnumDictCpuidFeature() {
			var names = Enum.GetNames(typeof(CpuidFeature));
			var dict = new Dictionary<string, CpuidFeature>(names.Length, StringComparer.Ordinal);
			foreach (CpuidFeature value in Enum.GetValues(typeof(CpuidFeature))) {
				Debug.Assert(names[(int)value] == value.ToString());
				dict[names[(int)value]] = value;
			}
			return dict;
		}

		static Dictionary<string, FlowControl> CreateEnumDictFlowControl() {
			var names = Enum.GetNames(typeof(FlowControl));
			var dict = new Dictionary<string, FlowControl>(names.Length, StringComparer.Ordinal);
			foreach (FlowControl value in Enum.GetValues(typeof(FlowControl))) {
				Debug.Assert(names[(int)value] == value.ToString());
				dict[names[(int)value]] = value;
			}
			return dict;
		}

		static Dictionary<string, MemorySize> CreateEnumDictMemorySize() {
			var names = Enum.GetNames(typeof(MemorySize));
			var dict = new Dictionary<string, MemorySize>(names.Length, StringComparer.Ordinal);
			foreach (MemorySize value in Enum.GetValues(typeof(MemorySize))) {
				Debug.Assert(names[(int)value] == value.ToString());
				dict[names[(int)value]] = value;
			}
			return dict;
		}

		static Dictionary<string, Register> CreateEnumDictRegister() {
			var names = Enum.GetNames(typeof(Register));
			var dict = new Dictionary<string, Register>(names.Length, StringComparer.OrdinalIgnoreCase);
			foreach (Register value in Enum.GetValues(typeof(Register))) {
				Debug.Assert(names[(int)value] == value.ToString());
				dict[names[(int)value]] = value;
			}
			return dict;
		}

		const string VMM_prefix = "VMM";
		static readonly char[] commaSeparator = new char[] { ',' };
		static readonly char[] spaceSeparator = new char[] { ' ' };
		static readonly char[] semicolonSeparator = new char[] { ';' };
		static readonly char[] plusSeparator = new char[] { '+' };
		static protected IEnumerable<object[]> GetTestCases(int bitness, string className) =>
			GetTestCases(bitness, bitness, className);
		static protected IEnumerable<object[]> GetTestCases(int bitness, int stackAddressSize, string className) {
			var toCode = CreateEnumDictCode();
			var toEncoding = CreateEnumDictEncodingKind();
			var toCpuidFeature = CreateEnumDictCpuidFeature();
			var toFlowControl = CreateEnumDictFlowControl();
			var toMemorySize = CreateEnumDictMemorySize();
			var toRegister = CreateEnumDictRegister();
			var toAccess = new Dictionary<string, OpAccess>(StringComparer.Ordinal) {
				{ "n", OpAccess.None },
				{ "r", OpAccess.Read },
				{ "cr", OpAccess.CondRead },
				{ "w", OpAccess.Write },
				{ "cw", OpAccess.CondWrite },
				{ "rw", OpAccess.ReadWrite },
				{ "rcw", OpAccess.ReadCondWrite },
				{ "nma", OpAccess.NoMemAccess },
			};
			Assert.Equal(Enum.GetNames(typeof(OpAccess)).Length, toAccess.Count);

			// XSP = SP/ESP/RSP depending on stack address size, XBP = BP/EBP/RBP depending on stack address size
			const string XSP = "XSP";
			const string XBP = "XBP";
			switch (stackAddressSize) {
			case 16:
				toRegister.Add(XSP, Register.SP);
				toRegister.Add(XBP, Register.BP);
				break;
			case 32:
				toRegister.Add(XSP, Register.ESP);
				toRegister.Add(XBP, Register.EBP);
				break;
			case 64:
				toRegister.Add(XSP, Register.RSP);
				toRegister.Add(XBP, Register.RBP);
				break;
			default:
				throw new InvalidOperationException();
			}

			for (int i = 0; i < (InstructionInfoConstants.VMM_last - InstructionInfoConstants.VMM_first + 1); i++)
				toRegister.Add(VMM_prefix + i.ToString(), InstructionInfoConstants.VMM_first + i);

			var filename = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Intel", "InstructionInfoTests", className + ".txt");
			Debug.Assert(File.Exists(filename));
			int lineNo = 0;
			foreach (var line in File.ReadLines(filename)) {
				lineNo++;
				if (line.Length == 0 || line.StartsWith("#"))
					continue;

				const int ELEMS = 5;
				var elems = line.Split(commaSeparator, ELEMS);
				if (elems.Length != ELEMS)
					throw new Exception($"Invalid line, line {lineNo}: '{line}' ({filename})");

				var hexBytes = elems[0].Trim();
				var codeString = elems[1].Trim();
				var encodingString = elems[2].Trim();
				var cpuidFeatureString = elems[3].Trim();

				var testCase = new InstructionInfoTestCase();

				if (!toCode.TryGetValue(codeString, out var code))
					throw new Exception($"Invalid {nameof(Code)} value, line {lineNo}: '{codeString}' ({filename})");
				if (!toEncoding.TryGetValue(encodingString, out testCase.Encoding))
					throw new Exception($"Invalid {nameof(EncodingKind)} value, line {lineNo}: '{encodingString}' ({filename})");
				if (!toCpuidFeature.TryGetValue(cpuidFeatureString, out testCase.CpuidFeature))
					throw new Exception($"Invalid {nameof(CpuidFeature)} value, line {lineNo}: '{cpuidFeatureString}' ({filename})");

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
					case "pm":
						if (value != string.Empty)
							throw new Exception($"Invalid key-value value, line {lineNo}: '{keyValue}' ({filename})");
						testCase.ProtectedMode = true;
						break;

					case "priv":
						if (value != string.Empty)
							throw new Exception($"Invalid key-value value, line {lineNo}: '{keyValue}' ({filename})");
						testCase.Privileged = true;
						break;

					case "saverestore":
						if (value != string.Empty)
							throw new Exception($"Invalid key-value value, line {lineNo}: '{keyValue}' ({filename})");
						testCase.SaveRestoreInstruction = true;
						break;

					case "stack":
						if (!int.TryParse(value, out testCase.StackPointerIncrement))
							throw new Exception($"Invalid key-value value, line {lineNo}: '{keyValue}' ({filename})");
						testCase.StackInstruction = true;
						break;

					case "fr":
						if (!ParseRflags(value, ref testCase.RflagsRead))
							throw new Exception($"Invalid key-value value, line {lineNo}: '{keyValue}' ({filename})");
						break;

					case "fu":
						if (!ParseRflags(value, ref testCase.RflagsUndefined))
							throw new Exception($"Invalid key-value value, line {lineNo}: '{keyValue}' ({filename})");
						break;

					case "fw":
						if (!ParseRflags(value, ref testCase.RflagsWritten))
							throw new Exception($"Invalid key-value value, line {lineNo}: '{keyValue}' ({filename})");
						break;

					case "fc":
						if (!ParseRflags(value, ref testCase.RflagsCleared))
							throw new Exception($"Invalid key-value value, line {lineNo}: '{keyValue}' ({filename})");
						break;

					case "fs":
						if (!ParseRflags(value, ref testCase.RflagsSet))
							throw new Exception($"Invalid key-value value, line {lineNo}: '{keyValue}' ({filename})");
						break;

					case "flow":
						if (!toFlowControl.TryGetValue(value, out testCase.FlowControl))
							throw new Exception($"Invalid key-value value, line {lineNo}: '{keyValue}' ({filename})");
						break;

					case "op0":
						if (!toAccess.TryGetValue(value, out testCase.Op0Access))
							throw new Exception($"Invalid key-value value, line {lineNo}: '{keyValue}' ({filename})");
						break;

					case "op1":
						if (!toAccess.TryGetValue(value, out testCase.Op1Access))
							throw new Exception($"Invalid key-value value, line {lineNo}: '{keyValue}' ({filename})");
						break;

					case "op2":
						if (!toAccess.TryGetValue(value, out testCase.Op2Access))
							throw new Exception($"Invalid key-value value, line {lineNo}: '{keyValue}' ({filename})");
						break;

					case "op3":
						if (!toAccess.TryGetValue(value, out testCase.Op3Access))
							throw new Exception($"Invalid key-value value, line {lineNo}: '{keyValue}' ({filename})");
						break;

					case "op4":
						if (!toAccess.TryGetValue(value, out testCase.Op4Access))
							throw new Exception($"Invalid key-value value, line {lineNo}: '{keyValue}' ({filename})");
						break;

					case "r":
						if (!AddRegisters(toRegister, value, OpAccess.Read, testCase, lineNo, filename))
							throw new Exception($"Invalid key-value value, line {lineNo}: '{keyValue}' ({filename})");
						break;

					case "cr":
						if (!AddRegisters(toRegister, value, OpAccess.CondRead, testCase, lineNo, filename))
							throw new Exception($"Invalid key-value value, line {lineNo}: '{keyValue}' ({filename})");
						break;

					case "w":
						if (!AddRegisters(toRegister, value, OpAccess.Write, testCase, lineNo, filename))
							throw new Exception($"Invalid key-value value, line {lineNo}: '{keyValue}' ({filename})");
						break;

					case "cw":
						if (!AddRegisters(toRegister, value, OpAccess.CondWrite, testCase, lineNo, filename))
							throw new Exception($"Invalid key-value value, line {lineNo}: '{keyValue}' ({filename})");
						break;

					case "rw":
						if (!AddRegisters(toRegister, value, OpAccess.ReadWrite, testCase, lineNo, filename))
							throw new Exception($"Invalid key-value value, line {lineNo}: '{keyValue}' ({filename})");
						break;

					case "rcw":
						if (!AddRegisters(toRegister, value, OpAccess.ReadCondWrite, testCase, lineNo, filename))
							throw new Exception($"Invalid key-value value, line {lineNo}: '{keyValue}' ({filename})");
						break;

					case "rm":
						if (!AddMemory(bitness, toMemorySize, toRegister, value, OpAccess.Read, testCase))
							throw new Exception($"Invalid key-value value, line {lineNo}: '{keyValue}' ({filename})");
						break;

					case "crm":
						if (!AddMemory(bitness, toMemorySize, toRegister, value, OpAccess.CondRead, testCase))
							throw new Exception($"Invalid key-value value, line {lineNo}: '{keyValue}' ({filename})");
						break;

					case "rwm":
						if (!AddMemory(bitness, toMemorySize, toRegister, value, OpAccess.ReadWrite, testCase))
							throw new Exception($"Invalid key-value value, line {lineNo}: '{keyValue}' ({filename})");
						break;

					case "rcwm":
						if (!AddMemory(bitness, toMemorySize, toRegister, value, OpAccess.ReadCondWrite, testCase))
							throw new Exception($"Invalid key-value value, line {lineNo}: '{keyValue}' ({filename})");
						break;

					case "wm":
						if (!AddMemory(bitness, toMemorySize, toRegister, value, OpAccess.Write, testCase))
							throw new Exception($"Invalid key-value value, line {lineNo}: '{keyValue}' ({filename})");
						break;

					case "cwm":
						if (!AddMemory(bitness, toMemorySize, toRegister, value, OpAccess.CondWrite, testCase))
							throw new Exception($"Invalid key-value value, line {lineNo}: '{keyValue}' ({filename})");
						break;

					default:
						throw new Exception($"Invalid key-value value, line {lineNo}: '{keyValue}' ({filename})");
					}
				}

				yield return new object[4] { hexBytes, code, lineNo, testCase };
			}
		}

		static bool AddMemory(int bitness, Dictionary<string, MemorySize> toMemorySize, Dictionary<string, Register> toRegister, string value, OpAccess access, InstructionInfoTestCase testCase) {
			var elems = value.Split(semicolonSeparator);
			if (elems.Length != 2)
				return false;
			var expr = elems[0].Trim();
			if (!toMemorySize.TryGetValue(elems[1].Trim(), out var memorySize))
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

		static bool AddRegisters(Dictionary<string, Register> toRegister, string value, OpAccess access, InstructionInfoTestCase testCase, int lineNo, string filename) {
			foreach (var tmp in value.Split(semicolonSeparator, StringSplitOptions.RemoveEmptyEntries)) {
				var regString = tmp.Trim();
				if (!toRegister.TryGetValue(regString, out var reg))
					return false;

				if (testCase.Encoding != EncodingKind.Legacy && testCase.Encoding != EncodingKind.D3NOW) {
					switch (access) {
					case OpAccess.None:
					case OpAccess.Read:
					case OpAccess.NoMemAccess:
						break;

					case OpAccess.Write:
					case OpAccess.CondWrite:
					case OpAccess.ReadWrite:
					case OpAccess.ReadCondWrite:
						if (Register.XMM0 <= reg && reg <= InstructionInfoConstants.VMM_last && !regString.StartsWith(VMM_prefix, StringComparison.OrdinalIgnoreCase))
							throw new Exception($"Register {regString} is written ({access}) but {VMM_prefix} pseudo register should be used instead, line {lineNo} ({filename})");
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
				case 'a':
					rflags |= RflagsBits.AF;
					break;

				case 'c':
					rflags |= RflagsBits.CF;
					break;

				case 'o':
					rflags |= RflagsBits.OF;
					break;

				case 'p':
					rflags |= RflagsBits.PF;
					break;

				case 's':
					rflags |= RflagsBits.SF;
					break;

				case 'z':
					rflags |= RflagsBits.ZF;
					break;

				case 'i':
					rflags |= RflagsBits.IF;
					break;

				case 'd':
					rflags |= RflagsBits.DF;
					break;

				case 'A':
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
