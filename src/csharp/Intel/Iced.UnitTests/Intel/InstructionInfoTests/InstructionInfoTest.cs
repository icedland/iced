// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if INSTR_INFO
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
			x.Access == y.Access &&
			x.AddressSize == y.AddressSize &&
			x.VsibSize == y.VsibSize;

		public int GetHashCode(UsedMemory obj) {
			int hc = 0;
			hc ^= (int)obj.Segment;
			hc ^= (int)obj.Base << 8;
			hc ^= (int)obj.Index << 16;
			hc ^= obj.Scale << 28;
			hc ^= obj.Displacement.GetHashCode();
			hc ^= (int)obj.MemorySize << 12;
			hc ^= (int)obj.Access << 24;
			hc ^= (int)obj.AddressSize << 3;
			hc ^= (int)obj.VsibSize << 11;
			return hc;
		}
	}

	public abstract class InstructionInfoTest {
		protected void TestInstructionInfo(int bitness, string hexBytes, Code code, DecoderOptions options, int lineNo, InstructionInfoTestCase testCase) {
			var codeBytes = HexUtils.ToByteArray(hexBytes);
			Instruction instruction;
			if (testCase.IsSpecial) {
				if (bitness == 16 && code == Code.Popw_CS && hexBytes == "0F") {
					instruction = default;
					instruction.Code = Code.Popw_CS;
					instruction.Op0Kind = OpKind.Register;
					instruction.Op0Register = Register.CS;
					instruction.CodeSize = CodeSize.Code16;
					instruction.Length = 1;
				}
				else if (code <= Code.DeclareQword) {
					instruction = default;
					instruction.Code = code;
					instruction.DeclareDataCount = 1;
					Assert.Equal(64, bitness);
					instruction.CodeSize = CodeSize.Code64;
					switch (code) {
					case Code.DeclareByte:
						Assert.Equal("66", hexBytes);
						instruction.SetDeclareByteValue(0, 0x66);
						break;
					case Code.DeclareWord:
						Assert.Equal("6644", hexBytes);
						instruction.SetDeclareWordValue(0, 0x4466);
						break;
					case Code.DeclareDword:
						Assert.Equal("664422EE", hexBytes);
						instruction.SetDeclareDwordValue(0, 0xEE224466);
						break;
					case Code.DeclareQword:
						Assert.Equal("664422EE12345678", hexBytes);
						instruction.SetDeclareQwordValue(0, 0x78563412EE224466);
						break;
					default: throw new InvalidOperationException();
					}
				}
				else if (code == Code.Zero_bytes) {
					instruction = default;
					instruction.Code = code;
					Assert.Equal(64, bitness);
					instruction.CodeSize = CodeSize.Code64;
					Assert.Equal("", hexBytes);
				}
				else {
					var decoder = CreateDecoder(bitness, codeBytes, testCase.IP, options);
					instruction = decoder.Decode();
					if (codeBytes.Length > 1 && codeBytes[0] == 0x9B && instruction.Length == 1) {
						instruction = decoder.Decode();
						instruction.Code = instruction.Code switch {
							Code.Fnstenv_m14byte => Code.Fstenv_m14byte,
							Code.Fnstenv_m28byte => Code.Fstenv_m28byte,
							Code.Fnstcw_m2byte => Code.Fstcw_m2byte,
							Code.Fneni => Code.Feni,
							Code.Fndisi => Code.Fdisi,
							Code.Fnclex => Code.Fclex,
							Code.Fninit => Code.Finit,
							Code.Fnsetpm => Code.Fsetpm,
							Code.Fnsave_m94byte => Code.Fsave_m94byte,
							Code.Fnsave_m108byte => Code.Fsave_m108byte,
							Code.Fnstsw_m2byte => Code.Fstsw_m2byte,
							Code.Fnstsw_AX => Code.Fstsw_AX,
							Code.Fnstdw_AX => Code.Fstdw_AX,
							Code.Fnstsg_AX => Code.Fstsg_AX,
							_ => throw new InvalidOperationException(),
						};
					}
					else
						throw new InvalidOperationException();
				}
			}
			else {
				var decoder = CreateDecoder(bitness, codeBytes, testCase.IP, options);
				instruction = decoder.Decode();
			}
			Assert.Equal(code, instruction.Code);

			Assert.Equal(testCase.StackPointerIncrement, instruction.StackPointerIncrement);

			var info = new InstructionInfoFactory().GetInfo(instruction);
			Assert.Equal(testCase.Op0Access, info.Op0Access);
			Assert.Equal(testCase.Op1Access, info.Op1Access);
			Assert.Equal(testCase.Op2Access, info.Op2Access);
			Assert.Equal(testCase.Op3Access, info.Op3Access);
			Assert.Equal(testCase.Op4Access, info.Op4Access);
			var fpuInfo = instruction.GetFpuStackIncrementInfo();
			Assert.Equal(testCase.FpuTopIncrement, fpuInfo.Increment);
			Assert.Equal(testCase.FpuConditionalTop, fpuInfo.Conditional);
			Assert.Equal(testCase.FpuWritesTop, fpuInfo.WritesTop);
			Assert.Equal(
				new HashSet<UsedMemory>(testCase.UsedMemory, UsedMemoryEqualityComparer.Instance),
				new HashSet<UsedMemory>(info.GetUsedMemory(), UsedMemoryEqualityComparer.Instance));
			Assert.Equal(
				new HashSet<UsedRegister>(GetUsedRegisters(testCase.UsedRegisters), UsedRegisterEqualityComparer.Instance),
				new HashSet<UsedRegister>(GetUsedRegisters(info.GetUsedRegisters()), UsedRegisterEqualityComparer.Instance));

			Static.Assert(IcedConstants.MaxOpCount == 5 ? 0 : -1);
			Debug.Assert(instruction.OpCount <= IcedConstants.MaxOpCount);
			for (int i = 0; i < instruction.OpCount; i++) {
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
			for (int i = instruction.OpCount; i < IcedConstants.MaxOpCount; i++)
				Assert.Equal(OpAccess.None, info.GetOpAccess(i));

			var info2 = new InstructionInfoFactory().GetInfo(instruction, InstructionInfoOptions.None);
			CheckEqual(ref info, ref info2, hasRegs2: true, hasMem2: true);
			info2 = new InstructionInfoFactory().GetInfo(instruction, InstructionInfoOptions.NoMemoryUsage);
			CheckEqual(ref info, ref info2, hasRegs2: true, hasMem2: false);
			info2 = new InstructionInfoFactory().GetInfo(instruction, InstructionInfoOptions.NoRegisterUsage);
			CheckEqual(ref info, ref info2, hasRegs2: false, hasMem2: true);
			info2 = new InstructionInfoFactory().GetInfo(instruction, InstructionInfoOptions.NoRegisterUsage | InstructionInfoOptions.NoMemoryUsage);
			CheckEqual(ref info, ref info2, hasRegs2: false, hasMem2: false);

			Assert.Equal(testCase.Encoding, instruction.Code.Encoding());
#if ENCODER && OPCODE_INFO
			Assert.Equal(code.ToOpCode().Encoding, testCase.Encoding);
#endif
			Assert.Equal(testCase.CpuidFeatures, instruction.Code.CpuidFeatures());
			Assert.Equal(testCase.FlowControl, instruction.Code.FlowControl());
			Assert.Equal(testCase.IsPrivileged, instruction.Code.IsPrivileged());
			Assert.Equal(testCase.IsStackInstruction, instruction.Code.IsStackInstruction());
			Assert.Equal(testCase.IsSaveRestoreInstruction, instruction.Code.IsSaveRestoreInstruction());

			Assert.Equal(testCase.Encoding, instruction.Encoding);
#if MVEX
			if (instruction.Encoding == EncodingKind.MVEX)
				Assert.True(IcedConstants.IsMvex(instruction.Code));
			else
				Assert.False(IcedConstants.IsMvex(instruction.Code));
#endif
			Assert.Equal(testCase.CpuidFeatures, instruction.CpuidFeatures);
			Assert.Equal(testCase.FlowControl, instruction.FlowControl);
			Assert.Equal(testCase.IsPrivileged, instruction.IsPrivileged);
			Assert.Equal(testCase.IsStackInstruction, instruction.IsStackInstruction);
			Assert.Equal(testCase.IsSaveRestoreInstruction, instruction.IsSaveRestoreInstruction);
			Assert.Equal(testCase.RflagsRead, instruction.RflagsRead);
			Assert.Equal(testCase.RflagsWritten, instruction.RflagsWritten);
			Assert.Equal(testCase.RflagsCleared, instruction.RflagsCleared);
			Assert.Equal(testCase.RflagsSet, instruction.RflagsSet);
			Assert.Equal(testCase.RflagsUndefined, instruction.RflagsUndefined);
			Assert.Equal(testCase.RflagsWritten | testCase.RflagsCleared | testCase.RflagsSet | testCase.RflagsUndefined, instruction.RflagsModified);

			Assert.Equal(RflagsBits.None, instruction.RflagsWritten & (instruction.RflagsCleared | instruction.RflagsSet | instruction.RflagsUndefined));
			Assert.Equal(RflagsBits.None, instruction.RflagsCleared & (instruction.RflagsWritten | instruction.RflagsSet | instruction.RflagsUndefined));
			Assert.Equal(RflagsBits.None, instruction.RflagsSet & (instruction.RflagsWritten | instruction.RflagsCleared | instruction.RflagsUndefined));
			Assert.Equal(RflagsBits.None, instruction.RflagsUndefined & (instruction.RflagsWritten | instruction.RflagsCleared | instruction.RflagsSet));
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
			Assert.Equal(info1.Op0Access, info2.Op0Access);
			Assert.Equal(info1.Op1Access, info2.Op1Access);
			Assert.Equal(info1.Op2Access, info2.Op2Access);
			Assert.Equal(info1.Op3Access, info2.Op3Access);
			Assert.Equal(info1.Op4Access, info2.Op4Access);
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
				else if (Register.YMM0 <= reg && reg <= IcedConstants.YMM_last) {
					index = reg - Register.YMM0;
					if (hash.Contains(Register.ZMM0 + index))
						continue;
				}
				else if (Register.XMM0 <= reg && reg <= IcedConstants.XMM_last) {
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
		static readonly (Register rl, Register rh, Register rx)[] lowRegs = new (Register rl, Register rh, Register rx)[4] {
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

			if (Register.ZMM0 <= reg && reg <= IcedConstants.ZMM_last)
				return 4;
			if (Register.YMM0 <= reg && reg <= IcedConstants.YMM_last)
				return 5;
			if (Register.XMM0 <= reg && reg <= IcedConstants.XMM_last)
				return 6;

			return -1;
		}

		Decoder CreateDecoder(int bitness, byte[] codeBytes, ulong ip, DecoderOptions options) {
			var codeReader = new ByteArrayCodeReader(codeBytes);
			var decoder = Decoder.Create(bitness, codeReader, options);
			decoder.IP = ip;
			Assert.Equal(bitness, decoder.Bitness);
			return decoder;
		}

		static protected IEnumerable<object[]> GetTestCases(int bitness) =>
			InstructionInfoTestReader.GetTestCases(bitness, bitness);
	}
}
#endif
