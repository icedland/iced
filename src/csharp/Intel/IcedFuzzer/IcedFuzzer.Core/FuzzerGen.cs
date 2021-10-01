// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using Iced.Intel;

namespace IcedFuzzer.Core {
	enum FuzzerEncodingKind {
		Legacy,
		D3NOW,
		VEX2,
		VEX3,
		XOP,
		EVEX,
	}

	readonly struct FuzzerGenResult {
		public readonly bool IsValid;
		public FuzzerGenResult(bool isValid) => IsValid = isValid;
	}

	sealed class UsedRegs {
		readonly List<(FuzzerRegisterClass registerClass, uint regNum)> usedRegs;

		public UsedRegs() =>
			usedRegs = new List<(FuzzerRegisterClass registerClass, uint regNum)>();

		public void Clear() => usedRegs.Clear();

		public bool HasRegister(FuzzerRegisterClass registerClass, uint regNum) {
			foreach (var info in usedRegs) {
				if (info.registerClass == registerClass && info.regNum == regNum)
					return true;
			}
			return false;
		}

		public uint GetNextUnusedReg(FuzzerRegisterClass registerClass, uint startNum = 0) {
			uint maxValue = RegisterUtils.GetRegisterCount(registerClass);
			for (uint regNum = startNum; regNum < maxValue; regNum++) {
				if (!HasRegister(registerClass, regNum))
					return regNum;
			}
			throw ThrowHelpers.Unreachable;
		}

		public void Add(in RegisterInfo regInfo, RegisterFuzzerOperand regOp, uint regNum) =>
			Add(regOp.RegisterClass, regInfo.MaskOutIgnoredBits(regOp.Register, regNum));

		public void Add(FuzzerRegisterClass registerClass, uint regNum) =>
			usedRegs.Add((registerClass, regNum));
	}

	sealed class FuzzerGenContext {
		public Fuzzer Fuzzer { get; }
		public int Bitness => Fuzzer.Bitness;
		public FuzzerInstruction Instruction { get; }
		public FuzzerEncodingKind Encoding { get; }
		public UsedRegs UsedRegs { get; }
		public bool UselessPrefixes => Fuzzer.UselessPrefixes;
		uint immIndex;
		uint nonZeroImmIndex;

		public readonly WritePrefix[] WritePrefixes;
		public readonly byte[] PrefixesTmp1;
		public readonly byte[] PrefixesTmp2;

		public FuzzerGenContext(Fuzzer fuzzer, FuzzerInstruction instruction, FuzzerEncodingKind encoding) {
			Fuzzer = fuzzer;
			Instruction = instruction;
			Encoding = encoding;
			UsedRegs = new UsedRegs();

			WritePrefixes = new WritePrefix[4];
			PrefixesTmp1 = new byte[1];
			PrefixesTmp2 = new byte[2];

			uint imm = (uint)instruction.OpCode.Byte0 ^ instruction.OpCode.Byte1;
			if (instruction.GroupIndex >= 0)
				imm |= (uint)instruction.GroupIndex << 8;
			if (instruction.RmGroupIndex >= 0)
				imm |= (uint)instruction.RmGroupIndex << 8;
			immIndex = imm % MaxImmediates;
			nonZeroImmIndex = imm % MaxNonZeroImmediates;
		}

		// Use an odd number so instructions with 2 imms don't always use the same sign bit set/clear in the same operand.
		public const int MaxImmediates = 5;
		public const int MaxNonZeroImmediates = 4;

		public ulong NextUInt64() =>
			(immIndex++ % MaxImmediates) switch {
				0 => 0xE4ADC79CF883AFE8,// sign bit set
				1 => 0x6D4E27A475A30FED,// sign bit clear
				2 => 0x8000000000000000,// min signed
				3 => 0x7FFFFFFFFFFFFFFF,// max signed
				4 => 0x0000000000000000,
				_ => throw ThrowHelpers.Unreachable,
			};

		// These just return the upper bits of NextUInt64()
		public uint NextUInt32() => (uint)(NextUInt64() >> 32);
		public ushort NextUInt16() => (ushort)(NextUInt64() >> 48);
		public byte NextUInt8() => (byte)(NextUInt64() >> 56);

		public ulong NextNonZeroUInt64() =>
			(nonZeroImmIndex++ % MaxNonZeroImmediates) switch {
				0 => 0xE4ADC79CF883AFE8,// sign bit set
				1 => 0x6D4E27A475A30FED,// sign bit clear
				2 => 0x8000000000000000,// min signed
				3 => 0x7FFFFFFFFFFFFFFF,// max signed
				_ => throw ThrowHelpers.Unreachable,
			};

		// These just return the upper bits of NextNonZeroUInt64()
		public uint NextNonZeroUInt32() => (uint)(NextNonZeroUInt64() >> 32);
		public ushort NextNonZeroUInt16() => (ushort)(NextNonZeroUInt64() >> 48);
		public byte NextNonZeroUInt8() => (byte)(NextNonZeroUInt64() >> 56);
	}

	[Flags]
	enum OpHelpersFlags : uint {
		None					= 0,
		NoClearUsedRegs			= 0x00000001,
		NoInitOpMask			= 0x00000002,
		Prefix67				= 0x00000004,
	}

	static class OpHelpers {
		public static InstructionInfo InitializeInstruction(FuzzerGenContext context, OpHelpersFlags flags = OpHelpersFlags.None) {
			if ((flags & OpHelpersFlags.NoClearUsedRegs) == 0)
				context.UsedRegs.Clear();
			var info = InstructionInfo.Create(context);
			foreach (var op in context.Instruction.Operands) {
				if ((flags & OpHelpersFlags.NoInitOpMask) != 0 && op is RegisterFuzzerOperand regOp && regOp.RegLocation == FuzzerOperandRegLocation.AaaBits)
					continue;
				InitializeOperand(context, op, ref info, flags);
			}
			return info;
		}

		public static void InitializeOperand(FuzzerGenContext context, FuzzerOperand op, ref InstructionInfo info, OpHelpersFlags flags = OpHelpersFlags.None) {
			int addressSize = info.Bitness;
			if ((flags & OpHelpersFlags.Prefix67) != 0) {
				addressSize = addressSize switch {
					16 => 32,
					32 => 16,
					64 => 32,
					_ => throw ThrowHelpers.Unreachable,
				};
			}

			switch (op.Kind) {
			case FuzzerOperandKind.None:
				break;

			case FuzzerOperandKind.Immediate:
				switch (((ImmediateFuzzerOperand)op).ImmKind) {
				case FuzzerImmediateKind.Imm1:
					info.SetImmediate(1, context.NextUInt8());
					break;
				case FuzzerImmediateKind.Imm2:
					info.SetImmediate(2, context.NextUInt16());
					break;
				case FuzzerImmediateKind.Imm4:
					info.SetImmediate(4, context.NextUInt32());
					break;
				case FuzzerImmediateKind.Imm8:
					info.SetImmediate(8, context.NextUInt64());
					break;
				case FuzzerImmediateKind.Imm2_2:
					info.SetImmediate(2, context.NextUInt16());
					info.SetImmediate(2, context.NextUInt16());
					break;
				case FuzzerImmediateKind.Imm4_2:
					info.SetImmediate(4, context.NextUInt32());
					info.SetImmediate(2, context.NextUInt16());
					break;
				default:
					throw ThrowHelpers.Unreachable;
				}
				break;

			case FuzzerOperandKind.MemOffs:
				switch (addressSize) {
				case 16:
					info.SetImmediate(2, context.NextUInt16());
					break;
				case 32:
					info.SetImmediate(4, context.NextUInt32());
					break;
				case 64:
					info.SetImmediate(8, context.NextUInt64());
					break;
				default:
					throw ThrowHelpers.Unreachable;
				}
				break;

			case FuzzerOperandKind.ImpliedMem:
				break;

			case FuzzerOperandKind.Mem:
				// We don't care if it's the same regs if it's the GPRs, only if it's eg. vec regs which can be in mem ops if it's VSIB
				var memOp = (ModrmMemoryFuzzerOperand)op;
				if ((addressSize >= 32 && info.addressSize != 16) || (memOp.MustNotUseAddrSize16 && (flags & OpHelpersFlags.Prefix67) == 0)) {
					Assert.True(info.addressSize != 16);
					if (addressSize == 16) {
						Assert.False(context.Instruction.DontUsePrefix67);
						info.addressSizePrefix = 0x67;
					}
					if (memOp.IsVSIB) {
						uint vecNum = context.UsedRegs.GetNextUnusedReg(FuzzerRegisterClass.Vector);
						context.UsedRegs.Add(FuzzerRegisterClass.Vector, vecNum);
						info.SetModrmSibMemory(0x04, 0x40 + ((vecNum & 7) << 3), UsedBits.x | UsedBits.v2);// [eax+vec[n]*2] / [rax+vec[n]*2]
						info.x = (vecNum >> 3) & 1;
						info.v2 = vecNum >> 4;
					}
					else {
						// Any GPR can be used, even same ones used in different ops
						info.SetModrmSibMemory(0x04, 0x48);// [eax+ecx*2] / [rax+rcx*2]
					}
				}
				else {
					if (memOp.MustNotUseAddrSize16)
						info.IsValid = false;
					// Any GPR can be used, even same ones used in different ops
					info.SetModrmMemory(0x00);// [bx+si]
				}
				break;

			case FuzzerOperandKind.Register:
				var regOp = (RegisterFuzzerOperand)op;
				var regInfo = regOp.GetRegisterInfo(context.Bitness, info.Encoding);
				uint startNum = regOp.RegLocation == FuzzerOperandRegLocation.AaaBits && context.Instruction.RequireOpMaskRegister ? 1U : 0;
				uint regNum = context.UsedRegs.GetNextUnusedReg(regOp.RegisterClass, startNum);
				context.UsedRegs.Add(regInfo, regOp, regNum);
				info.SetRegister(regOp.RegLocation, regNum);
				break;

			default:
				throw ThrowHelpers.Unreachable;
			}
		}
	}

	abstract class FuzzerGen {
		/// <summary>
		/// Generates zero or more instructions, valid or invalid encodings
		/// </summary>
		public abstract IEnumerable<FuzzerGenResult> Generate(FuzzerGenContext context);
	}

	// Generates all prefixes, one at a time, before the mandatory prefix (if any)
	// Generates all prefixes, one at a time, after the mandatory prefix (if any)
	// Multi prefixes before the mandatory prefix:
	//	4F + any other prefix
	//	same prefix twice
	//	two prefixes from the same group, all combos, except same prefixes (already gen'd)
	//	67 + seg + seg (some combos)
	//	67 + F3/F2
	//	xacquire/xrelease + lock
	//	notrack + bnd
	sealed class PrefixesFuzzerGen : FuzzerGen {
		static readonly byte[][] oneBytePrefixes = new byte[][] {
			new byte[] { 0x26 },
			new byte[] { 0x2E },
			new byte[] { 0x36 },
			new byte[] { 0x3E },
			new byte[] { 0x64 },
			new byte[] { 0x65 },
			new byte[] { 0x66 },
			new byte[] { 0x67 },
			new byte[] { 0xF0 },
			new byte[] { 0xF2 },
			new byte[] { 0xF3 },
			new byte[] { 0x4F },
		};
		static readonly byte[][] multiBytePrefixes = new byte[][] {
			// 4F + any other prefix
			new byte[] { 0x4F, 0x26 },
			new byte[] { 0x4F, 0x2E },
			new byte[] { 0x4F, 0x36 },
			new byte[] { 0x4F, 0x3E },
			new byte[] { 0x4F, 0x64 },
			new byte[] { 0x4F, 0x65 },
			new byte[] { 0x4F, 0x66 },
			new byte[] { 0x4F, 0x67 },
			new byte[] { 0x4F, 0xF0 },
			new byte[] { 0x4F, 0xF2 },
			new byte[] { 0x4F, 0xF3 },
			new byte[] { 0x4F, 0x40 },

			// xacquire/xrelease + lock
			new byte[] { 0xF0, 0xF2 },
			new byte[] { 0xF3, 0xF0 },

			// notrack + bnd
			new byte[] { 0x3E, 0xF2 },

			// same prefix twice
			new byte[] { 0x26, 0x26 },
			new byte[] { 0x2E, 0x2E },
			new byte[] { 0x36, 0x36 },
			new byte[] { 0x3E, 0x3E },
			new byte[] { 0x64, 0x64 },
			new byte[] { 0x65, 0x65 },
			new byte[] { 0x66, 0x66 },
			new byte[] { 0x67, 0x67 },
			new byte[] { 0xF0, 0xF0 },
			new byte[] { 0xF2, 0xF2 },
			new byte[] { 0xF3, 0xF3 },
			new byte[] { 0x4F, 0x40 },

			// two prefixes from the same group, all combos, except same prefixes (already gen'd)
			new byte[] { 0xF3, 0xF2 },
			new byte[] { 0xF2, 0xF3 },
			new byte[] { 0x66, 0xF3 },
			new byte[] { 0x66, 0xF2 },
			new byte[] { 0xF3, 0x66 },
			new byte[] { 0xF2, 0x66 },

			new byte[] { 0x26, 0x2E },
			new byte[] { 0x26, 0x36 },
			new byte[] { 0x26, 0x3E },
			new byte[] { 0x26, 0x64 },
			new byte[] { 0x26, 0x65 },

			new byte[] { 0x2E, 0x26 },
			new byte[] { 0x2E, 0x36 },
			new byte[] { 0x2E, 0x3E },
			new byte[] { 0x2E, 0x64 },
			new byte[] { 0x2E, 0x65 },

			new byte[] { 0x36, 0x26 },
			new byte[] { 0x36, 0x2E },
			new byte[] { 0x36, 0x3E },
			new byte[] { 0x36, 0x64 },
			new byte[] { 0x36, 0x65 },

			new byte[] { 0x3E, 0x26 },
			new byte[] { 0x3E, 0x2E },
			new byte[] { 0x3E, 0x36 },
			new byte[] { 0x3E, 0x64 },
			new byte[] { 0x3E, 0x65 },

			new byte[] { 0x64, 0x26 },
			new byte[] { 0x64, 0x2E },
			new byte[] { 0x64, 0x36 },
			new byte[] { 0x64, 0x3E },
			new byte[] { 0x64, 0x65 },

			new byte[] { 0x65, 0x26 },
			new byte[] { 0x65, 0x2E },
			new byte[] { 0x65, 0x36 },
			new byte[] { 0x65, 0x3E },
			new byte[] { 0x65, 0x64 },

			// 67 + seg + seg (some combos)
			new byte[] { 0x67, 0x65, 0x26 },
			new byte[] { 0x65, 0x67, 0x2E },
			new byte[] { 0x65, 0x36, 0x67 },
			new byte[] { 0x65, 0x67, 0x3E },
			new byte[] { 0x67, 0x65, 0x64 },

			// 67 + F3/F2
			new byte[] { 0x67, 0xF3 },
			new byte[] { 0xF2, 0x67 },
		};
		static readonly (bool beforeMP, byte[][] allPrefixes)[] prefixTests = new (bool beforeMP, byte[][] allPrefixes)[] {
			(true, oneBytePrefixes),
			(false, oneBytePrefixes),
			(true, multiBytePrefixes),
		};

		public override IEnumerable<FuzzerGenResult> Generate(FuzzerGenContext context) {
			if (!context.UselessPrefixes)
				yield break;

			bool canUseMPREX;
			MandatoryPrefix realMP;
			switch (context.Encoding) {
			case FuzzerEncodingKind.Legacy:
			case FuzzerEncodingKind.D3NOW:
				canUseMPREX = true;
				realMP = context.Instruction.MandatoryPrefix;
				break;
			case FuzzerEncodingKind.VEX2:
			case FuzzerEncodingKind.VEX3:
			case FuzzerEncodingKind.XOP:
			case FuzzerEncodingKind.EVEX:
				canUseMPREX = false;
				realMP = MandatoryPrefix.None;
				break;
			default:
				throw ThrowHelpers.Unreachable;
			}

			bool no66 = context.Instruction.No66;
			var writePrefixes = context.WritePrefixes;
			var prefixesTmp1 = context.PrefixesTmp1;
			var prefixesTmp2 = context.PrefixesTmp2;
			bool usesLockAsExtraRegBit = context.Fuzzer.CpuDecoder == CpuDecoder.AMD && context.Instruction.AmdLockRegBit;
			foreach (var (beforeMP, allPrefixes) in prefixTests) {
				for (int i = 0; i < allPrefixes.Length; i++) {
					var prefixes = allPrefixes[i];
					var flags = OpHelpersFlags.None;
					bool isValid = true;

					byte effectiveMP = 0;
					byte rex = 0;
					bool ignoreThis = false;
					bool has66 = false;
					foreach (var prefix in prefixes) {
						if ((prefix & 0xF0) == 0x40) {
							if (context.Bitness < 64) {
								ignoreThis = true;
								break;
							}
							rex = prefix;
						}
						else {
							rex = 0;
							switch (prefix) {
							case 0x67:
								if (context.Instruction.DontUsePrefix67) {
									ignoreThis = true;
									continue;
								}
								flags |= OpHelpersFlags.Prefix67;
								break;
							case 0x66:
								if (effectiveMP == 0)
									effectiveMP = prefix;
								has66 = true;
								break;
							case 0xF2:
							case 0xF3:
								effectiveMP = prefix;
								break;
							case 0xF0:
								if (!context.Instruction.CanUseLockPrefix || !context.Instruction.IsModrmMemory)
									isValid = false;
								if (usesLockAsExtraRegBit) {
									ignoreThis = true;
									continue;
								}
								break;
							}
						}
					}
					if (ignoreThis)
						continue;

					switch (effectiveMP) {
					case 0:
						break;
					case 0x66:
						if (!canUseMPREX)
							isValid = false;
						else {
							if (realMP == MandatoryPrefix.PNP)
								continue;
							if (realMP != MandatoryPrefix.P66 && context.Instruction.DontUsePrefix66)
								continue;
						}
						break;
					case 0xF3:
						if (!canUseMPREX)
							isValid = false;
						else {
							if (beforeMP) {
								if (realMP == MandatoryPrefix.PNP || realMP == MandatoryPrefix.P66)
									continue;
								if (realMP != MandatoryPrefix.PF3 && realMP != MandatoryPrefix.PF2 && context.Instruction.DontUsePrefixF3)
									continue;
							}
							else {
								if (realMP == MandatoryPrefix.PNP || realMP == MandatoryPrefix.P66 || realMP == MandatoryPrefix.PF2)
									continue;
								if (realMP != MandatoryPrefix.PF3 && context.Instruction.DontUsePrefixF3)
									continue;
							}
						}
						break;
					case 0xF2:
						if (!canUseMPREX)
							isValid = false;
						else {
							if (beforeMP) {
								if (realMP == MandatoryPrefix.PNP || realMP == MandatoryPrefix.P66)
									continue;
								if (realMP != MandatoryPrefix.PF3 && realMP != MandatoryPrefix.PF2 && context.Instruction.DontUsePrefixF2)
									continue;
							}
							else {
								if (realMP == MandatoryPrefix.PNP || realMP == MandatoryPrefix.P66 || realMP == MandatoryPrefix.PF3)
									continue;
								if (realMP != MandatoryPrefix.PF2 && context.Instruction.DontUsePrefixF2)
									continue;
							}
						}
						break;
					default:
						throw ThrowHelpers.Unreachable;
					}

					context.UsedRegs.Clear();
					if (context.Instruction.IsXchgRegAcc)
						context.UsedRegs.Add(FuzzerRegisterClass.GPR, 0);
					var info = OpHelpers.InitializeInstruction(context, flags | OpHelpersFlags.NoClearUsedRegs);

					if ((info.UsedBits & UsedBits.w) != 0 || (info.Flags & EncodedInfoFlags.HasREX) != 0 || (beforeMP && realMP > MandatoryPrefix.PNP)) {
						// Last REX prefix wins so this one will be ignored
						rex = 0;
					}

					if (has66) {
						if (no66)
							isValid = false;

						// REX.W overrides 66
						if ((rex & 8) == 0 && (context.Instruction.OperandSize != 64 || context.Instruction.DefaultOperandSize64)) {
							int opSize = context.Bitness == 16 ? 32 : 16;
							if (context.Instruction.DontUsePrefix66 && context.Instruction.OperandSize != opSize)
								continue;
						}
					}

					if (rex != 0) {
						Assert.True(prefixes[^1] == rex);
						if (context.Bitness < 64)
							continue;
						if (!canUseMPREX)
							isValid = false;
						else {
							if (context.Instruction.DontUsePrefixREXW)
								rex &= 0xF7;
							if ((info.UsedBits & UsedBits.r) != 0)
								rex &= 0xFB;
							if ((info.UsedBits & UsedBits.x) != 0)
								rex &= 0xFD;
							if ((info.UsedBits & UsedBits.b) != 0)
								rex &= 0xFE;
							Assert.True(prefixes.Length == 1 || prefixes.Length == 2);
							var prefixesTmp = prefixes.Length == 1 ? prefixesTmp1 : prefixesTmp2;
							for (int j = 0; j < prefixes.Length; j++)
								prefixesTmp[j] = prefixes[j];
							prefixesTmp[^1] = rex;
							prefixes = prefixesTmp;
						}
					}

					if (beforeMP) {
						writePrefixes[0] = new WritePrefix(WritePrefixKind.AddressSize);
						writePrefixes[1] = new WritePrefix(WritePrefixKind.OperandSize);
						writePrefixes[2] = new WritePrefix(prefixes);
						writePrefixes[3] = new WritePrefix(WritePrefixKind.MandatoryPrefix);
					}
					else {
						writePrefixes[0] = new WritePrefix(WritePrefixKind.MandatoryPrefix);
						writePrefixes[1] = new WritePrefix(WritePrefixKind.OperandSize);
						writePrefixes[2] = new WritePrefix(WritePrefixKind.AddressSize);
						writePrefixes[3] = new WritePrefix(prefixes);
					}
					info.WritePrefixes = writePrefixes;
					context.Fuzzer.Write(info);

					if (!info.IsValid)
						isValid = false;
					yield return new FuzzerGenResult(isValid);
				}
			}
		}
	}

	// For each instr, encode it with too few bytes (1..instr_len-1 total bytes)
	sealed class NotEnoughBytesLeftFuzzerGen : FuzzerGen {
		public override IEnumerable<FuzzerGenResult> Generate(FuzzerGenContext context) {
			for (int i = 1; ; i++) {
				context.UsedRegs.Clear();
				var info = OpHelpers.InitializeInstruction(context, OpHelpersFlags.NoClearUsedRegs);
				Assert.True(info.IsValid);
				context.Fuzzer.Write(info);
				int newLen = context.Fuzzer.Writer.Length - i;
				if (newLen <= 0) {
					context.Fuzzer.Writer.Clear();
					break;
				}
				context.Fuzzer.Writer.SetLength(newLen);
				yield return new FuzzerGenResult(isValid: false);
			}
		}
	}

	// For each instruction, add enough valid/ignored prefixes to make the total length > 15 bytes
	sealed class InvalidLengthFuzzerGen : FuzzerGen {
		static int GetInstructionLength(FuzzerGenContext context) {
			Assert.True(context.Fuzzer.Writer.Length == 0);
			var info = OpHelpers.InitializeInstruction(context);
			context.Fuzzer.Write(info);
			int length = context.Fuzzer.Writer.Length;
			context.Fuzzer.Writer.Clear();
			Assert.True(length != 0);
			Assert.True(context.Fuzzer.Writer.Length == 0);
			return length;
		}

		public override IEnumerable<FuzzerGenResult> Generate(FuzzerGenContext context) {
			if (!context.UselessPrefixes)
				yield break;

			int instrLength = GetInstructionLength(context);
			const int maxLength = 15;
			int prefixes = instrLength <= maxLength ? maxLength - instrLength : 0;
			for (; prefixes <= maxLength; prefixes++) {
				context.UsedRegs.Clear();
				if (context.Instruction.IsXchgRegAcc)
					context.UsedRegs.Add(FuzzerRegisterClass.GPR, 0);
				var info = OpHelpers.InitializeInstruction(context, OpHelpersFlags.NoClearUsedRegs);
				Assert.True(info.IsValid);
				for (int i = 0; i < prefixes; i++)
					context.Fuzzer.Writer.WriteByte((byte)(0x64 ^ (i & 1)));// FS: or GS:
				context.Fuzzer.Write(info);
				int expectedLength = instrLength + prefixes;
				Assert.True(context.Fuzzer.Writer.Length == expectedLength);
				bool isValid = expectedLength <= maxLength;
				yield return new FuzzerGenResult(isValid);
			}
		}
	}

	// For each (op1,op2) that use the same register class, gen op1==op2. If one of them
	// is a modrm mem op, then the mem op must be a vsib op and the reg op must be a vec reg.
	// It doesn't gen the same gpr in a reg op and a mem op, eg. `mov eax,[eax]`, but it
	// does gen it if they're both reg ops, eg. `mov eax,eax`.
	sealed class SameRegsFuzzerGen : FuzzerGen {
		static (FuzzerRegisterClass?, Func<int, int, bool>) GetUniqueOperandRegClass(FuzzerInstruction instr) {
			int count = 0;
			Func<int, int, bool> isInvalid = (_, _) => false;
			if (instr.RequiresUniqueRegNums) {
				count++;
				// Always invalid
				isInvalid = (_, _) => true;
			}
			if (instr.RequiresUniqueDestRegNum) {
				count++;
				// Invalid if dst equals src1 or src2
				isInvalid = (op0Index, _) => op0Index == 0;
			}
			if (count > 1)
				throw ThrowHelpers.Unreachable;
			if (count == 0)
				return (null, isInvalid);

			const uint VEC_REG = 0x01;
			const uint TMM_REG = 0x02;
			uint regs = 0;
			foreach (var op in instr.RegisterOperands) {
				switch (op.RegisterClass) {
				case FuzzerRegisterClass.Vector:
					regs |= VEC_REG;
					break;
				case FuzzerRegisterClass.TMM:
					regs |= TMM_REG;
					break;
				}
			}
			var regClass = regs switch {
				VEC_REG => FuzzerRegisterClass.Vector,
				TMM_REG => FuzzerRegisterClass.TMM,
				_ => throw ThrowHelpers.Unreachable,
			};
			return (regClass, isInvalid);
		}

		public override IEnumerable<FuzzerGenResult> Generate(FuzzerGenContext context) {
			var (uniqueOpRegClass, isInvalid) = GetUniqueOperandRegClass(context.Instruction);
			var ops = context.Instruction.Operands;
			for (int op0Index = 0; op0Index < ops.Length; op0Index++) {
				var op0 = ops[op0Index];
				if (!GetRegInfo(context, op0, out var op0RegInfo))
					continue;
				for (int op1Index = op0Index + 1; op1Index < ops.Length; op1Index++) {
					var op1 = ops[op1Index];
					if (!GetRegInfo(context, op1, out var op1RegInfo))
						continue;
					if (op0RegInfo.RegClass != op1RegInfo.RegClass)
						continue;

					uint maxRegs = Math.Max(op0RegInfo.MaxRegCount, op1RegInfo.MaxRegCount);
					// If it's a GPR8, test with a REX prefix too so all GPR8 regs are tested
					uint maxRexCount = context.Bitness == 64 && (op0RegInfo.IsGPR8 || op1RegInfo.IsGPR8) ? 2U : 1;
					for (uint rexCount = 0; rexCount < maxRexCount; rexCount++) {
						for (uint regNum = 0; regNum <= maxRegs; regNum++) {
							context.UsedRegs.Clear();
							uint op0RegNum = op0RegInfo.MaskOutIgnoredBits(regNum);
							uint op1RegNum = op1RegInfo.MaskOutIgnoredBits(regNum);
							context.UsedRegs.Add(op0RegInfo.RegClass, op0RegNum);
							context.UsedRegs.Add(op1RegInfo.RegClass, op1RegNum);

							var info = InstructionInfo.Create(context);
							if (rexCount == 1) {
								if (!context.UselessPrefixes && ((op0RegInfo.IsGPR8 && op0RegNum < 4) || (op1RegInfo.IsGPR8 && op1RegNum < 4)))
									continue;
								info.Flags |= EncodedInfoFlags.HasREX;
							}
							foreach (var op in ops) {
								if (op == op0)
									op0RegInfo.InitializeOperand(ref info, regNum);
								else if (op == op1)
									op1RegInfo.InitializeOperand(ref info, regNum);
								else
									OpHelpers.InitializeOperand(context, op, ref info);
							}
							Assert.True(info.IsValid);
							context.Fuzzer.Write(info);
							bool isValid = true;
							if (!op0RegInfo.IsValidRegister(context.Instruction, regNum) || !op1RegInfo.IsValidRegister(context.Instruction, regNum))
								isValid = false;
							if (((op0RegNum == 0 && op0RegInfo.IsOpMask) || (op1RegNum == 0 && op1RegInfo.IsOpMask)) && context.Instruction.RequireOpMaskRegister)
								isValid = false;
							if (op0RegNum == op1RegNum && uniqueOpRegClass == op0RegInfo.RegClass && isInvalid(op0Index, op1Index))
								isValid = false;
							yield return new FuzzerGenResult(isValid);
						}
					}
				}
			}
		}

		readonly struct RegInfo {
			public readonly bool IsGPR8;
			public readonly FuzzerRegisterClass RegClass;
			public readonly uint MaxRegCount;

			readonly RegisterFuzzerOperand? regOp;
			readonly RegisterInfo regInfo;

			readonly ModrmMemoryFuzzerOperand? memOp;
			readonly uint regMask;

			public RegInfo(FuzzerGenContext context, RegisterFuzzerOperand regOp) {
				IsGPR8 = regOp.Register == FuzzerRegisterKind.GPR8;
				RegClass = regOp.RegisterClass;
				regInfo = regOp.GetRegisterInfo(context.Bitness, context.Encoding);
				MaxRegCount = regInfo.MaxRegCount;
				regMask = 0;
				this.regOp = regOp;
				memOp = null;
			}

			public RegInfo(FuzzerRegisterClass regClass, uint maxRegCount, ModrmMemoryFuzzerOperand memOp, uint regMask) {
				IsGPR8 = false;
				RegClass = regClass;
				regInfo = default;
				MaxRegCount = maxRegCount;
				this.regMask = regMask;
				regOp = null;
				this.memOp = memOp;
			}

			public bool IsOpMask =>
				regOp is not null && regOp.Register == FuzzerRegisterKind.K && regOp.RegLocation == FuzzerOperandRegLocation.AaaBits;

			public bool IsValidRegister(FuzzerInstruction instruction, uint regNum) {
				regNum = MaskOutIgnoredBits(regNum);
				if (regOp is not null)
					return regInfo.IsValid(instruction, regOp.Register, regNum);
				return true;
			}

			public uint MaskOutIgnoredBits(uint regNum) {
				regNum %= MaxRegCount;
				if (regOp is not null)
					return regInfo.MaskOutIgnoredBits(regOp.Register, regNum);
				if (memOp is not null)
					return regNum & regMask;
				throw ThrowHelpers.Unreachable;
			}

			public void InitializeOperand(ref InstructionInfo info, uint regNum) {
				regNum = MaskOutIgnoredBits(regNum);
				if (regOp is not null)
					info.SetRegister(regOp.RegLocation, regNum);
				else if (memOp is not null) {
					if (info.Bitness == 16)
						info.addressSizePrefix = 0x67;
					info.SetModrmSibMemory(0x04, 0x40 + ((regNum & 7) << 3), UsedBits.x | UsedBits.v2);// [eax+vec[n]*2] / [rax+vec[n]*2]
					info.x = (regNum >> 3) & 1;
					Assert.True(memOp.IsVSIB);
					info.v2 = regNum >> 4;
				}
				else
					throw ThrowHelpers.Unreachable;
			}
		}

		static bool GetRegInfo(FuzzerGenContext context, FuzzerOperand op, out RegInfo regInfo) {
			switch (op) {
			case ModrmMemoryFuzzerOperand memOp:
				if (memOp.IsVSIB) {
					// GPR base, vec reg index
					// index reg = V' X sib.iii

					uint maxRegCount, regMask;
					switch (context.Encoding) {
					case FuzzerEncodingKind.VEX2:
						// index reg: sib.iii (no X bit available)
						maxRegCount = 8;
						regMask = 7;
						break;

					case FuzzerEncodingKind.VEX3:
					case FuzzerEncodingKind.XOP:
						// index reg: X sib.iii, but if 16/32-bit mode: X can't be used
						if (context.Bitness < 64) {
							maxRegCount = 8;
							regMask = 7;
						}
						else {
							maxRegCount = 16;
							regMask = 0xF;
						}
						break;

					case FuzzerEncodingKind.EVEX:
						// index reg: V' X sib.iii, but if 16/32-bit mode: X can't be used and V' is ignored
						if (context.Bitness < 64) {
							maxRegCount = 8;
							regMask = 7;
						}
						else {
							maxRegCount = 32;
							regMask = 0x1F;
						}
						break;

					case FuzzerEncodingKind.Legacy:
					case FuzzerEncodingKind.D3NOW:
					default:
						throw ThrowHelpers.Unreachable;
					}

					regInfo = new RegInfo(FuzzerRegisterClass.Vector, maxRegCount, memOp, regMask);
					return true;
				}
				else {
					// GPR base + index. No need to test this
					break;
				}

			case RegisterFuzzerOperand regOp:
				regInfo = new RegInfo(context, regOp);
				return true;

			default:
				break;
			}

			regInfo = default;
			return false;
		}
	}

	// For each reg operand, gen each valid and invalid register. The other operands are set to valid operand values.
	// If it's mov to/from CR/DR/TR, generate all mod=00..11 values.
	// If AMD and mov to/from CR/DR/TR, generate a LOCK prefix as an extra reg bit.
	// Also gens instructions with ignored bits set to 1 (W R X B R').
	sealed class AllRegsFuzzerGen : FuzzerGen {
		static IEnumerable<(RegisterFuzzerOperand regOp, bool forceREX)> GetRegisterOperands(FuzzerInstruction instruction, int bitness, FuzzerEncodingKind encoding) {
			foreach (var regOp in instruction.RegisterOperands) {
				yield return (regOp, false);
				if (bitness == 64 && regOp.Register == FuzzerRegisterKind.GPR8 && encoding == FuzzerEncodingKind.Legacy)
					yield return (regOp, true);
			}
		}

		public override IEnumerable<FuzzerGenResult> Generate(FuzzerGenContext context) {
			bool ignoresModBits = context.Instruction.IgnoresModBits;
			uint modMax = ignoresModBits ? 3U : 0;
			uint lockTestMax = 0;
			if (ignoresModBits && context.Fuzzer.CpuDecoder == CpuDecoder.AMD)
				lockTestMax = 2;

			for (uint lockCount = 0; lockCount <= lockTestMax; lockCount++) {
				bool useLockPrefix = lockCount >= 1;
				bool useLockAndRex = lockCount >= 2;
				for (uint mod = 0; mod <= modMax; mod++) {
					foreach (var (regOp, forceREX) in GetRegisterOperands(context.Instruction, context.Bitness, context.Encoding)) {
						var regInfo = regOp.GetRegisterInfo(context.Bitness, context.Encoding);
						// xchg reg,rAX is NOP if reg==rAX
						uint startNum = context.Instruction.IsXchgRegAcc ? 1U : 0;
						for (uint i = startNum; i < regInfo.MaxRegCount + 1; i++) {
							bool setIgnoredBits;
							uint regNum;
							if (i == regInfo.MaxRegCount) {
								regNum = regOp.Register switch {
									FuzzerRegisterKind.K => 1,
									_ => startNum,
								};
								setIgnoredBits = true;
							}
							else {
								regNum = i;
								setIgnoredBits = false;
							}

							context.UsedRegs.Clear();
							context.UsedRegs.Add(regInfo, regOp, regNum);
							var info = InstructionInfo.Create(context);
							if (forceREX) {
								if (!context.UselessPrefixes && regOp.Register == FuzzerRegisterKind.GPR8 && (i & 0xF) < 4)
									continue;
								info.Flags |= EncodedInfoFlags.HasREX;
							}
							bool isValid = true;
							if (useLockPrefix && (regOp.Register == FuzzerRegisterKind.CR || regOp.Register == FuzzerRegisterKind.DR || regOp.Register == FuzzerRegisterKind.TR)) {
								Assert.True(regOp.RegLocation == FuzzerOperandRegLocation.ModrmRegBits);
								Assert.True(context.Encoding == FuzzerEncodingKind.Legacy);
								Assert.True(regNum <= 15);
								if (useLockAndRex)
									info.SetRegister(regOp.RegLocation, regNum);
								else
									info.SetRegister(regOp.RegLocation, regNum & 7);
								if ((regNum & 8) != 0) {
									if (useLockAndRex)
										isValid = false;
									info.WritePrefixes = new WritePrefix[] {
										new WritePrefix(new byte[] { 0xF0 }),
										new WritePrefix(WritePrefixKind.AddressSize),
										new WritePrefix(WritePrefixKind.OperandSize),
										new WritePrefix(WritePrefixKind.MandatoryPrefix),
									};
								}
							}
							else
								info.SetRegister(regOp.RegLocation, regNum);
							if (ignoresModBits) {
								Assert.True((info.Flags & EncodedInfoFlags.HasModrm) != 0);
								info.modrm = (info.modrm & 0x3F) | (mod << 6);
							}

							foreach (var op in context.Instruction.Operands) {
								if (op == regOp)
									continue;
								OpHelpers.InitializeOperand(context, op, ref info);
							}
							Assert.True(info.IsValid);

							if (setIgnoredBits) {
								if (!context.UselessPrefixes)
									continue;
								info.SetUnusedBits();
							}

							context.Fuzzer.Write(info);
							isValid = isValid && regInfo.IsValid(context.Instruction, regOp.Register, regNum);
							Assert.True(!setIgnoredBits || isValid, "Must be a valid instruction when testing ignored bits!");
							yield return new FuzzerGenResult(isValid);
						}
					}
				}
			}
		}
	}

	// For each modrm mem op, gen various kinds of mem ops with and without sib bytes and with and without 67 prefixes.
	// Also gens instructions with ignored bits set to 1 (W R X B R').
	sealed class AllModrmMemFuzzerGen : FuzzerGen {
		static readonly (byte modrm, byte sib, byte dispSize, bool hasSib, bool isIpRel)[]? mem16 = new (byte modrm, byte sib, byte dispSize, bool hasSib, bool isIpRel)[] {
			(0x01, 0x00, 0, false, false),	// [bx+di]
			(0x06, 0x00, 2, false, false),	// [disp16]
			(0x43, 0x00, 1, false, false),	// [bp+di+disp8]
			(0x84, 0x00, 2, false, false),	// [si+disp16]
		};
		static readonly (byte modrm, byte sib, byte dispSize, bool hasSib, bool isIpRel)[]? mem32 = new (byte modrm, byte sib, byte dispSize, bool hasSib, bool isIpRel)[] {
			(0x02, 0x00, 0, false, false),	// [edx]
			(0x05, 0x00, 4, false, false),	// [disp32]
			(0x45, 0x00, 1, false, false),	// [ebp+disp8]
			(0x86, 0x00, 4, false, false),	// [esi+disp32]
			(0x04, 0x1A, 0, true, false),	// [edx+ebx]
			(0x04, 0x62, 0, true, false),	// [edx]
			(0x04, 0xB5, 4, true, false),	// [esi*4+disp32]
			(0x04, 0xE5, 4, true, false),	// [disp32]
			(0x44, 0x5A, 1, true, false),	// [edx+ebx*2+disp8]
			(0x44, 0xA2, 1, true, false),	// [edx+disp8]
			(0x44, 0xF5, 1, true, false),	// [ebp+esi*8+disp8]
			(0x44, 0x25, 1, true, false),	// [ebp+disp8]
			(0x84, 0x9A, 4, true, false),	// [edx+ebx*4+disp32]
			(0x84, 0xE2, 4, true, false),	// [edx+disp32]
			(0x84, 0x35, 4, true, false),	// [ebp+esi+disp32]
			(0x84, 0x65, 4, true, false),	// [ebp+disp32]
		};
		static readonly (byte modrm, byte sib, byte dispSize, bool hasSib, bool isIpRel)[]? mem64 = new (byte modrm, byte sib, byte dispSize, bool hasSib, bool isIpRel)[] {
			(0x02, 0x00, 0, false, false),	// [rdx]
			(0x05, 0x00, 4, false, true),	// [rip+disp32]
			(0x45, 0x00, 1, false, false),	// [rbp+disp8]
			(0x86, 0x00, 4, false, false),	// [rsi+disp32]
			(0x04, 0x1A, 0, true, false),	// [rdx+rbx]
			(0x04, 0x62, 0, true, false),	// [rdx]
			(0x04, 0xB5, 4, true, false),	// [rsi*4+disp32]
			(0x04, 0xE5, 4, true, false),	// [disp32]
			(0x44, 0x5A, 1, true, false),	// [rdx+rbx*2+disp8]
			(0x44, 0xA2, 1, true, false),	// [rdx+disp8]
			(0x44, 0xF5, 1, true, false),	// [rbp+rsi*8+disp8]
			(0x44, 0x25, 1, true, false),	// [rbp+disp8]
			(0x84, 0x9A, 4, true, false),	// [rdx+rbx*4+disp32]
			(0x84, 0xE2, 4, true, false),	// [rdx+disp32]
			(0x84, 0x35, 4, true, false),	// [rbp+rsi+disp32]
			(0x84, 0x65, 4, true, false),	// [rbp+disp32]
		};
		static readonly (byte modrm, byte sib, byte dispSize, bool hasSib, bool isIpRel)[]? vsib = new (byte modrm, byte sib, byte dispSize, bool hasSib, bool isIpRel)[] {
			(0x02, 0x00, 0, false, false),	// [rDX]
			(0x05, 0x00, 4, false, false),	// [disp32] / [rIP+disp32]
			(0x45, 0x00, 1, false, false),	// [rBP+disp8]
			(0x86, 0x00, 4, false, false),	// [rSI+disp32]
			(0x04, 0x1A, 0, true, false),	// [rDX+vec3]
			(0x04, 0x62, 0, true, false),	// [rDX+vec4*2]
			(0x04, 0xB5, 4, true, false),	// [vec6*4+disp32]
			(0x04, 0xE5, 4, true, false),	// [vec4*8+disp32]
			(0x44, 0x5A, 1, true, false),	// [rDX+vec3*2+disp8]
			(0x44, 0xA2, 1, true, false),	// [rDX+vec4*4+disp8]
			(0x44, 0xF5, 1, true, false),	// [rBP+vec6*8+disp8]
			(0x44, 0x25, 1, true, false),	// [rBP+vec4+disp8]
			(0x84, 0x9A, 4, true, false),	// [rDX+vec3*4+disp32]
			(0x84, 0xE2, 4, true, false),	// [rDX+vec4*8+disp32]
			(0x84, 0x35, 4, true, false),	// [rBP+vec6+disp32]
			(0x84, 0x65, 4, true, false),	// [rBP+vec4*2+disp32]
		};

		static readonly ((byte modrm, byte sib, byte dispSize, bool hasSib, bool isIpRel)[] memInfo, int addrSize, bool prefix67)[] info16 = new ((byte modrm, byte sib, byte dispSize, bool hasSib, bool isIpRel)[] memInfo, int addrSize, bool prefix67)[] {
			(mem16, 16, false),
			(mem32, 32, true),
		};
		static readonly ((byte modrm, byte sib, byte dispSize, bool hasSib, bool isIpRel)[] memInfo, int addrSize, bool prefix67)[] info32 = new ((byte modrm, byte sib, byte dispSize, bool hasSib, bool isIpRel)[] memInfo, int addrSize, bool prefix67)[] {
			(mem16, 16, true),
			(mem32, 32, false),
		};
		static readonly ((byte modrm, byte sib, byte dispSize, bool hasSib, bool isIpRel)[] memInfo, int addrSize, bool prefix67)[] info64 = new ((byte modrm, byte sib, byte dispSize, bool hasSib, bool isIpRel)[] memInfo, int addrSize, bool prefix67)[] {
			(mem64, 32, true),
			(mem64, 64, false),
		};
		static readonly ((byte modrm, byte sib, byte dispSize, bool hasSib, bool isIpRel)[] memInfo, int addrSize, bool prefix67)[] infoVsib16 = new ((byte modrm, byte sib, byte dispSize, bool hasSib, bool isIpRel)[] memInfo, int addrSize, bool prefix67)[] {
			(mem16, 16, false),
			(vsib, 32, true),
		};
		static readonly ((byte modrm, byte sib, byte dispSize, bool hasSib, bool isIpRel)[] memInfo, int addrSize, bool prefix67)[] infoVsib32 = new ((byte modrm, byte sib, byte dispSize, bool hasSib, bool isIpRel)[] memInfo, int addrSize, bool prefix67)[] {
			(mem16, 16, true),
			(vsib, 32, false),
		};
		static readonly ((byte modrm, byte sib, byte dispSize, bool hasSib, bool isIpRel)[] memInfo, int addrSize, bool prefix67)[] infoVsib64 = new ((byte modrm, byte sib, byte dispSize, bool hasSib, bool isIpRel)[] memInfo, int addrSize, bool prefix67)[] {
			(vsib, 32, true),
			(vsib, 64, false),
		};

		public override IEnumerable<FuzzerGenResult> Generate(FuzzerGenContext context) {
			if (context.Instruction.ModrmMemoryOperands.Length == 0)
				yield break;
			Assert.True(context.Instruction.ModrmMemoryOperands.Length == 1);
			var memOp = context.Instruction.ModrmMemoryOperands[0];

			((byte modrm, byte sib, byte dispSize, bool hasSib, bool isIpRel)[] memInfo, int addrSize, bool prefix67)[] infos;
			if (context.Instruction.IsVsib) {
				infos = context.Bitness switch {
					16 => infoVsib16,
					32 => infoVsib32,
					64 => infoVsib64,
					_ => throw ThrowHelpers.Unreachable,
				};
			}
			else {
				infos = context.Bitness switch {
					16 => info16,
					32 => info32,
					64 => info64,
					_ => throw ThrowHelpers.Unreachable,
				};
			}

			uint baseCount, indexCount;
			if (context.Bitness == 64) {
				switch (context.Encoding) {
				case FuzzerEncodingKind.VEX2:
					// No X or B bits
					baseCount = 0;
					indexCount = 0;
					break;
				case FuzzerEncodingKind.Legacy:
				case FuzzerEncodingKind.D3NOW:
				case FuzzerEncodingKind.VEX3:
				case FuzzerEncodingKind.XOP:
					// B bit
					baseCount = 2;
					// X bit
					indexCount = 2;
					break;
				case FuzzerEncodingKind.EVEX:
					// B bit
					baseCount = 2;
					if (context.Instruction.IsVsib) {
						// V' X bits
						indexCount = 4;
					}
					else {
						// X bit
						indexCount = 2;
					}
					break;
				default:
					throw ThrowHelpers.Unreachable;
				}
			}
			else {
				baseCount = 0;
				indexCount = 0;
			}

			foreach (var (memInfo, addrSize, prefix67) in infos) {
				foreach (var mem in memInfo) {
					if (TryGen(context, memOp, mem, addrSize, prefix67, 0, 0, setIgnoredBits: false, out var result))
						yield return result;
					if (context.UselessPrefixes) {
						if (TryGen(context, memOp, mem, addrSize, prefix67, 0, 0, setIgnoredBits: true, out result))
							yield return result;
					}

					// Base reg bit: B. Start from 1 since we've already tested 0.
					for (uint @base = 1; @base < baseCount; @base++) {
						if (TryGen(context, memOp, mem, addrSize, prefix67, @base, 0, setIgnoredBits: false, out result))
							yield return result;
					}

					// Index reg bits: V' X (VSIB) or just X. Start from 1 since we've already tested 0.
					for (uint index = 1; index < indexCount; index++) {
						if (TryGen(context, memOp, mem, addrSize, prefix67, 0, index, setIgnoredBits: false, out result))
							yield return result;
					}
				}
			}
		}

		static bool TryGen(FuzzerGenContext context, ModrmMemoryFuzzerOperand memOp, in (byte modrm, byte sib, byte dispSize, bool hasSib, bool isIpRel) mem, int addrSize, bool prefix67, uint baseUpperBits, uint indexUpperBits, bool setIgnoredBits, out FuzzerGenResult result) {
			Assert.True(baseUpperBits <= 1);
			// EVEX is the only encoding using v2, see below
			Assert.True((context.Encoding != FuzzerEncodingKind.EVEX && indexUpperBits <= 1) ||
				(context.Encoding == FuzzerEncodingKind.EVEX && indexUpperBits <= 3));
			context.UsedRegs.Clear();
			var info = InstructionInfo.Create(context);
			if (info.addressSizePrefix != 0) {
				if (!prefix67) {
					result = default;
					return false;
				}
			}
			else if (prefix67) {
				if (context.Instruction.DontUsePrefix67) {
					result = default;
					return false;
				}
				info.addressSizePrefix = 0x67;
			}

			if (mem.hasSib && context.Instruction.IsVsib) {
				uint vecNum = ((uint)mem.sib >> 3) & 7;
				if (indexUpperBits >= 0)
					vecNum += indexUpperBits << 3;
				context.UsedRegs.Add(FuzzerRegisterClass.Vector, vecNum);
			}

			foreach (var op in context.Instruction.Operands) {
				if (op == memOp) {
					if (mem.hasSib)
						info.SetModrmSibMemory(mem.modrm, mem.sib, UsedBits.b | UsedBits.x | UsedBits.v2);
					else
						info.SetModrmMemory(mem.modrm, UsedBits.b | UsedBits.x | UsedBits.v2);
					info.b = baseUpperBits;
					info.x = indexUpperBits & 1;
					if (context.Encoding == FuzzerEncodingKind.EVEX && context.Instruction.IsVsib)
						info.v2 = indexUpperBits >> 1;
					else
						Assert.True(indexUpperBits <= 1);
					switch (mem.dispSize) {
					case 0: break;
					case 1: info.SetImmediate(mem.dispSize, context.NextNonZeroUInt8()); break;
					case 2: info.SetImmediate(mem.dispSize, context.NextNonZeroUInt16()); break;
					case 4: info.SetImmediate(mem.dispSize, context.NextNonZeroUInt32()); break;
					default: throw ThrowHelpers.Unreachable;
					}
				}
				else
					OpHelpers.InitializeOperand(context, op, ref info);
			}
			Assert.True(info.IsValid);

			if (setIgnoredBits)
				info.SetUnusedBits();

			context.Fuzzer.Write(info);
			bool isValid = true;
			if (memOp.MustUseSib && !mem.hasSib)
				isValid = false;
			if (memOp.MustNotUseAddrSize16 && addrSize == 16)
				isValid = false;
			if (memOp.NoRipRel && mem.isIpRel)
				isValid = false;
			result = new FuzzerGenResult(isValid);
			return true;
		}
	}

	// For each mem offs op, gen the operand with and without a 67 prefix.
	// Also gens instructions with ignored bits set to 1 (W R X B R').
	sealed class AllMemOffsFuzzerGen : FuzzerGen {
		static readonly (bool useAddrSize, int size)[] infos16 = new (bool useAddrSize, int size)[] {
			(false, 2),
			(true, 4),
		};
		static readonly (bool useAddrSize, int size)[] infos32 = new (bool useAddrSize, int size)[] {
			(false, 4),
			(true, 2),
		};
		static readonly (bool useAddrSize, int size)[] infos64 = new (bool useAddrSize, int size)[] {
			(false, 8),
			(true, 4),
		};

		public override IEnumerable<FuzzerGenResult> Generate(FuzzerGenContext context) {
			var memInfos = context.Bitness switch {
				16 => infos16,
				32 => infos32,
				64 => infos64,
				_ => throw ThrowHelpers.Unreachable,
			};
			foreach (var memOffsOp in context.Instruction.MemOffsOperands) {
				foreach (var memInfo in memInfos) {
					for (int i = 0; i < FuzzerGenContext.MaxImmediates; i++) {
						for (int j = 0; j < 2; j++) {
							bool setIgnoredBits = j == 1;
							context.UsedRegs.Clear();
							var info = InstructionInfo.Create(context);
							foreach (var op in context.Instruction.Operands) {
								if (op == memOffsOp) {
									switch (memInfo.size) {
									case 2: info.SetImmediate(2, context.NextUInt16()); break;
									case 4: info.SetImmediate(4, context.NextUInt32()); break;
									case 8: info.SetImmediate(8, context.NextUInt64()); break;
									default: throw ThrowHelpers.Unreachable;
									}
									if (memInfo.useAddrSize) {
										Assert.True(info.addressSizePrefix == 0);
										Assert.False(context.Instruction.DontUsePrefix67);
										info.addressSizePrefix = 0x67;
									}
								}
								else
									OpHelpers.InitializeOperand(context, op, ref info);
							}
							Assert.True(info.IsValid);
							if (setIgnoredBits) {
								if (!context.UselessPrefixes)
									continue;
								info.SetUnusedBits();
							}
							bool isValid = true;
							Assert.True(isValid);
							Assert.True(!setIgnoredBits || isValid, "Must be a valid instruction when testing ignored bits!");
							context.Fuzzer.Write(info);
							yield return new FuzzerGenResult(isValid);
						}
					}
				}
			}
		}
	}

	// For each instruction with an implied op, gen with and without a 67h prefix
	// Also gens instructions with ignored bits set to 1 (W R X B R').
	sealed class AllImpliedMemFuzzerGen : FuzzerGen {
		public override IEnumerable<FuzzerGenResult> Generate(FuzzerGenContext context) {
			if (context.Instruction.ImpliedMemOperands.Length == 0)
				yield break;

			for (int i = 0; i < 2; i++) {
				for (int j = 0; j < 2; j++) {
					bool setIgnoredBits = j == 1;
					var info = OpHelpers.InitializeInstruction(context);
					if (i == 1) {
						Assert.False(context.Instruction.DontUsePrefix67);
						info.addressSizePrefix = 0x67;
					}
					Assert.True(info.IsValid);
					if (setIgnoredBits) {
						if (!context.UselessPrefixes)
							continue;
						info.SetUnusedBits();
					}
					bool isValid = true;
					Assert.True(isValid);
					Assert.True(!setIgnoredBits || isValid, "Must be a valid instruction when testing ignored bits!");
					context.Fuzzer.Write(info);
					yield return new FuzzerGenResult(isValid);
				}
			}
		}
	}

	// Every instruction with an immediate is generated with imm values. If it has a reg op,
	// it's not tested since it was tested by the reg fuzzer. The only exception is if it
	// is a special pseudo op instruction in which case we generate all 256 imm values.
	// Also gens instructions with unused bits set to 1 (W R X B R') (unless it has a reg op, it has been tested already).
	sealed class AllImmediateMemFuzzerGen : FuzzerGen {
		static bool ShouldGenAllImmValues(Code code) =>
			code switch {
				Code.Cmpps_xmm_xmmm128_imm8 or Code.VEX_Vcmpps_xmm_xmm_xmmm128_imm8 or Code.VEX_Vcmpps_ymm_ymm_ymmm256_imm8 or
				Code.EVEX_Vcmpps_kr_k1_xmm_xmmm128b32_imm8 or Code.EVEX_Vcmpps_kr_k1_ymm_ymmm256b32_imm8 or
				Code.EVEX_Vcmpps_kr_k1_zmm_zmmm512b32_imm8_sae or Code.Cmppd_xmm_xmmm128_imm8 or Code.VEX_Vcmppd_xmm_xmm_xmmm128_imm8 or
				Code.VEX_Vcmppd_ymm_ymm_ymmm256_imm8 or Code.EVEX_Vcmppd_kr_k1_xmm_xmmm128b64_imm8 or Code.EVEX_Vcmppd_kr_k1_ymm_ymmm256b64_imm8 or
				Code.EVEX_Vcmppd_kr_k1_zmm_zmmm512b64_imm8_sae or Code.Cmpss_xmm_xmmm32_imm8 or Code.VEX_Vcmpss_xmm_xmm_xmmm32_imm8 or
				Code.EVEX_Vcmpss_kr_k1_xmm_xmmm32_imm8_sae or Code.Cmpsd_xmm_xmmm64_imm8 or Code.VEX_Vcmpsd_xmm_xmm_xmmm64_imm8 or
				Code.EVEX_Vcmpsd_kr_k1_xmm_xmmm64_imm8_sae or Code.Pclmulqdq_xmm_xmmm128_imm8 or Code.VEX_Vpclmulqdq_xmm_xmm_xmmm128_imm8 or
				Code.VEX_Vpclmulqdq_ymm_ymm_ymmm256_imm8 or Code.EVEX_Vpclmulqdq_xmm_xmm_xmmm128_imm8 or Code.EVEX_Vpclmulqdq_ymm_ymm_ymmm256_imm8 or
				Code.EVEX_Vpclmulqdq_zmm_zmm_zmmm512_imm8 or Code.XOP_Vpcomb_xmm_xmm_xmmm128_imm8 or Code.XOP_Vpcomw_xmm_xmm_xmmm128_imm8 or
				Code.XOP_Vpcomd_xmm_xmm_xmmm128_imm8 or Code.XOP_Vpcomq_xmm_xmm_xmmm128_imm8 or Code.XOP_Vpcomub_xmm_xmm_xmmm128_imm8 or
				Code.XOP_Vpcomuw_xmm_xmm_xmmm128_imm8 or Code.XOP_Vpcomud_xmm_xmm_xmmm128_imm8 or Code.XOP_Vpcomuq_xmm_xmm_xmmm128_imm8 or
				Code.EVEX_Vpcmpb_kr_k1_xmm_xmmm128_imm8 or Code.EVEX_Vpcmpb_kr_k1_ymm_ymmm256_imm8 or Code.EVEX_Vpcmpb_kr_k1_zmm_zmmm512_imm8 or
				Code.EVEX_Vpcmpw_kr_k1_xmm_xmmm128_imm8 or Code.EVEX_Vpcmpw_kr_k1_ymm_ymmm256_imm8 or Code.EVEX_Vpcmpw_kr_k1_zmm_zmmm512_imm8 or
				Code.EVEX_Vpcmpd_kr_k1_xmm_xmmm128b32_imm8 or Code.EVEX_Vpcmpd_kr_k1_ymm_ymmm256b32_imm8 or Code.EVEX_Vpcmpd_kr_k1_zmm_zmmm512b32_imm8 or
				Code.EVEX_Vpcmpq_kr_k1_xmm_xmmm128b64_imm8 or Code.EVEX_Vpcmpq_kr_k1_ymm_ymmm256b64_imm8 or Code.EVEX_Vpcmpq_kr_k1_zmm_zmmm512b64_imm8 or
				Code.EVEX_Vpcmpub_kr_k1_xmm_xmmm128_imm8 or Code.EVEX_Vpcmpub_kr_k1_ymm_ymmm256_imm8 or Code.EVEX_Vpcmpub_kr_k1_zmm_zmmm512_imm8 or
				Code.EVEX_Vpcmpuw_kr_k1_xmm_xmmm128_imm8 or Code.EVEX_Vpcmpuw_kr_k1_ymm_ymmm256_imm8 or Code.EVEX_Vpcmpuw_kr_k1_zmm_zmmm512_imm8 or
				Code.EVEX_Vpcmpud_kr_k1_xmm_xmmm128b32_imm8 or Code.EVEX_Vpcmpud_kr_k1_ymm_ymmm256b32_imm8 or Code.EVEX_Vpcmpud_kr_k1_zmm_zmmm512b32_imm8 or
				Code.EVEX_Vpcmpuq_kr_k1_xmm_xmmm128b64_imm8 or Code.EVEX_Vpcmpuq_kr_k1_ymm_ymmm256b64_imm8 or Code.EVEX_Vpcmpuq_kr_k1_zmm_zmmm512b64_imm8 => true,
				_ => false,
			};

		public override IEnumerable<FuzzerGenResult> Generate(FuzzerGenContext context) {
			if (context.Instruction.ImmediateOperands.Length == 0)
				yield break;
			if (ShouldGenAllImmValues(context.Instruction.Code)) {
				Assert.True(context.Instruction.ImmediateOperands.Length == 1);
				Assert.True(context.Instruction.ImmediateOperands[0].ImmKind == FuzzerImmediateKind.Imm1);
				var immOp = context.Instruction.ImmediateOperands[0];
				for (uint imm = 0; imm <= 0xFF; imm++) {
					context.UsedRegs.Clear();
					var info = InstructionInfo.Create(context);
					foreach (var op in context.Instruction.Operands) {
						if (op == immOp)
							info.SetImmediate(1, imm);
						else
							OpHelpers.InitializeOperand(context, op, ref info);
					}
					Assert.True(info.IsValid);
					context.Fuzzer.Write(info);
					yield return new FuzzerGenResult(isValid: true);
				}
			}
			else {
				// This was tested when generating reg instructions
				if (context.Instruction.RegisterOperands.Length != 0)
					yield break;

				for (int i = 0; i < FuzzerGenContext.MaxImmediates; i++) {
					for (int j = 0; j < 2; j++) {
						bool setIgnoredBits = j == 1;
						var info = OpHelpers.InitializeInstruction(context);
						Assert.True(info.IsValid);
						if (setIgnoredBits) {
							if (!context.UselessPrefixes)
								continue;
							info.SetUnusedBits();
						}
						bool isValid = true;
						Assert.True(isValid);
						Assert.True(!setIgnoredBits || isValid, "Must be a valid instruction when testing ignored bits!");
						context.Fuzzer.Write(info);
						yield return new FuzzerGenResult(isValid);
					}
				}
			}
		}
	}

	// Gens all instrs with no ops.
	// Also gens instructions with ignored bits set to 1 (W R X B R').
	sealed class AllNoOpFuzzerGen : FuzzerGen {
		public override IEnumerable<FuzzerGenResult> Generate(FuzzerGenContext context) {
			if (context.Instruction.Operands.Length != 0)
				yield break;

			for (int i = 0; i < 2; i++) {
				bool setIgnoredBits = i == 1;
				var info = InstructionInfo.Create(context);
				Assert.True(info.IsValid);
				if (setIgnoredBits) {
					if (!context.UselessPrefixes)
						continue;
					info.SetUnusedBits();
				}
				bool isValid = true;
				Assert.True(isValid);
				Assert.True(!setIgnoredBits || isValid, "Must be a valid instruction when testing ignored bits!");
				context.Fuzzer.Write(info);
				yield return new FuzzerGenResult(isValid);
			}
		}
	}

	// Gens all combinations of EVEX.bcst/z/aaa bits, and if {er} is supported, all L'L bits
	sealed class EvexAaaZBcstErFuzzerGen : FuzzerGen {
		public override IEnumerable<FuzzerGenResult> Generate(FuzzerGenContext context) {
			if (context.Encoding != FuzzerEncodingKind.EVEX)
				yield break;
			bool destOpIsMem = context.Instruction.Operands.Length > 0 && context.Instruction.Operands[0] is ModrmMemoryFuzzerOperand;
			uint maxCount = context.Instruction.CanUseRoundingControl && !context.Instruction.DontUseEvexBcstBit ? 0x7FU : 0x1F;
			for (uint bits = 0; bits <= maxCount; bits++) {
				context.UsedRegs.Clear();
				context.UsedRegs.Add(FuzzerRegisterClass.K, 0);
				uint aaa = bits & 7;
				uint bcst = (bits >> 3) & 1;
				uint z = (bits >> 4) & 1;
				uint ll = bits >> 5;
				if (bcst != 0 && context.Instruction.DontUseEvexBcstBit)
					continue;
				var info = OpHelpers.InitializeInstruction(context, OpHelpersFlags.NoClearUsedRegs | OpHelpersFlags.NoInitOpMask);
				Assert.True(info.IsValid);
				info.aaa = aaa;
				info.bcst = bcst;
				info.z = z;
				if (info.Instruction.CanUseRoundingControl && bcst != 0)
					info.l = ll;
				context.Fuzzer.Write(info);
				bool isValid = true;
				if (context.Instruction.IsModrmMemory) {
					if (bcst != 0 && !context.Instruction.CanBroadcast)
						isValid = false;
				}
				else {
					if (bcst != 0 && !context.Instruction.CanUseRoundingControl && !context.Instruction.CanSuppressAllExceptions)
						isValid = false;
				}
				if (aaa == 0 && z == 1)
					isValid = false;
				if (aaa != 0 && !context.Instruction.CanUseOpMaskRegister)
					isValid = false;
				if (z == 1 && (!context.Instruction.CanUseZeroingMasking || destOpIsMem))
					isValid = false;
				if (aaa == 0 && context.Instruction.RequireOpMaskRegister)
					isValid = false;
				yield return new FuzzerGenResult(isValid);
			}
		}
	}

	// Gens all possible values of V' vvvv bits, but only if it's an instruction that doesn't use them.
	// If the instruction already uses the V' vvvv bits, the reg fuzzer gen has already generated all values.
	sealed class InvalidV2vvvvFuzzerGen : FuzzerGen {
		static bool UsesVvvvBits(FuzzerInstruction instruction) {
			foreach (var op in instruction.Operands) {
				switch (op.Kind) {
				case FuzzerOperandKind.None:
				case FuzzerOperandKind.Immediate:
				case FuzzerOperandKind.MemOffs:
				case FuzzerOperandKind.ImpliedMem:
				case FuzzerOperandKind.Mem:
					break;

				case FuzzerOperandKind.Register:
					switch (((RegisterFuzzerOperand)op).RegLocation) {
					case FuzzerOperandRegLocation.ModrmRegBits:
					case FuzzerOperandRegLocation.ModrmRmBits:
					case FuzzerOperandRegLocation.OpCodeBits:
					case FuzzerOperandRegLocation.Is4Bits:
					case FuzzerOperandRegLocation.Is5Bits:
					case FuzzerOperandRegLocation.AaaBits:
						break;

					case FuzzerOperandRegLocation.VvvvBits:
						return true;

					default:
						throw ThrowHelpers.Unreachable;
					}
					break;

				default:
					throw ThrowHelpers.Unreachable;
				}
			}
			return false;
		}

		public override IEnumerable<FuzzerGenResult> Generate(FuzzerGenContext context) {
			if (UsesVvvvBits(context.Instruction))
				yield break;

			uint v2vvvvCount, v2vvvvMask;
			switch (context.Encoding) {
			case FuzzerEncodingKind.Legacy:
			case FuzzerEncodingKind.D3NOW:
				// No V' & vvvv bits
				yield break;

			case FuzzerEncodingKind.VEX2:
				if (context.Bitness < 64) {
					// vvvv bits
					v2vvvvCount = 7;
					v2vvvvMask = 7;
				}
				else {
					// vvvv bits
					v2vvvvCount = 0xF;
					v2vvvvMask = 0xF;
				}
				break;

			case FuzzerEncodingKind.VEX3:
			case FuzzerEncodingKind.XOP:
				// vvvv bits
				v2vvvvCount = 0xF;
				v2vvvvMask = 0xF;
				break;

			case FuzzerEncodingKind.EVEX:
				if (context.Instruction.IsVsib) {
					// vvvv bits. V' is for the index register.
					v2vvvvCount = 0xF;
					v2vvvvMask = 0xF;
				}
				else {
					// V' and vvvv bits
					v2vvvvCount = 0x1F;
					v2vvvvMask = 0x1F;
				}
				break;

			default:
				throw ThrowHelpers.Unreachable;
			}

			for (uint v2vvvv = 0; v2vvvv < v2vvvvCount; v2vvvv++) {
				var info = OpHelpers.InitializeInstruction(context);
				Assert.True(info.IsValid);
				info.vvvv = v2vvvv & 0xF;
				if (!context.Instruction.IsVsib)
					info.v2 = v2vvvv >> 4;
				context.Fuzzer.Write(info);
				bool isValid = (v2vvvv & v2vvvvMask) == 0;
				yield return new FuzzerGenResult(isValid);
			}
		}
	}

	// EVEX: Sets the reserved bits p0[3] to 1 and p1[2] to 0
	sealed class InvalidReservedEvexBitsFuzzerGen : FuzzerGen {
		public override IEnumerable<FuzzerGenResult> Generate(FuzzerGenContext context) {
			if (context.Encoding != FuzzerEncodingKind.EVEX)
				yield break;

			for (uint r = 1; r <= 1; r++) {
				var info = OpHelpers.InitializeInstruction(context);
				Assert.True(info.IsValid);
				info.EVEX_res3 = r;
				context.Fuzzer.Write(info);
				yield return new FuzzerGenResult(isValid: false);
			}
			for (uint r = 0; r <= 0; r++) {
				var info = OpHelpers.InitializeInstruction(context);
				Assert.True(info.IsValid);
				info.EVEX_res10 = r;
				context.Fuzzer.Write(info);
				yield return new FuzzerGenResult(isValid: false);
			}
		}
	}

	// For each invalid instruction with reg-only modrm, gen all possible values of reg, rm, or reg/rm bits.
	// For each invalid instruction with mem-only modrm, gen all possible values of reg bits.
	sealed class GroupInvalidFuzzerGen : FuzzerGen {
		enum GroupKind {
			None,
			Rm,
			Reg,
			RmReg,
		}

		public override IEnumerable<FuzzerGenResult> Generate(FuzzerGenContext context) {
			Assert.False(context.Instruction.IsValid);
			if (context.Instruction.OpCode.IsTwobyte)
				yield break;

			GroupKind groupKind;
			if (context.Instruction.OpCode.IsTwobyte) {
				Assert.True(context.Instruction.GroupIndex < 0 && context.Instruction.RmGroupIndex < 0);
				groupKind = GroupKind.None;
			}
			else if (context.Instruction.GroupIndex < 0 && context.Instruction.RmGroupIndex < 0) {
				// Memory op uses the rm bits
				groupKind = context.Instruction.IsModrmMemory ? GroupKind.Reg : GroupKind.RmReg;
			}
			else if (context.Instruction.GroupIndex < 0) {
				Assert.True(context.Instruction.RmGroupIndex >= 0);
				// Group index is stored in the reg bits
				groupKind = GroupKind.Reg;
			}
			else {
				Assert.True(context.Instruction.RmGroupIndex < 0);
				// rm group index is stored in the rm bits
				groupKind = context.Instruction.IsModrmMemory ? GroupKind.None : GroupKind.Rm;
			}

			var opFlags = context.Bitness == 16 ? OpHelpersFlags.Prefix67 : OpHelpersFlags.None;
			switch (groupKind) {
			case GroupKind.None:
				Assert.True(context.Instruction.IsModrmMemory || context.Instruction.OpCode.IsTwobyte);
				yield break;

			case GroupKind.Rm:
				Assert.False(context.Instruction.IsModrmMemory, "rm bits are used by the mem op");
				for (uint rm = 0; rm <= 7; rm++) {
					var info = OpHelpers.InitializeInstruction(context, opFlags);
					Assert.True(info.IsValid);
					info.SetModrmRmBits(rm);
					info.SetModrmModBits(3);
					info.InitializeXOP0to7();
					context.Fuzzer.Write(info);
					yield return new FuzzerGenResult(isValid: false);
				}
				break;

			case GroupKind.Reg:
				for (uint reg = 0; reg <= 7; reg++) {
					var info = OpHelpers.InitializeInstruction(context, opFlags);
					Assert.True(info.IsValid);
					if (context.Instruction.IsModrmMemory) {
						// Use at least 32-bit addressing since some instructions require 32-bit addressing
						// but no instruction requires 16-bit addressing.
						if (context.Bitness == 16) {
							Assert.False(context.Instruction.DontUsePrefix67);
							info.addressSizePrefix = 0x67;
						}
						info.SetModrmSibMemory(0x04, 0x48);// [eax+ecx*2] / [rax+rcx*2]
					}
					info.SetModrmRegBits(reg);
					info.InitializeXOP0to7();
					context.Fuzzer.Write(info);
					yield return new FuzzerGenResult(isValid: false);
				}
				break;

			case GroupKind.RmReg:
				Assert.False(context.Instruction.IsModrmMemory, "rm bits are used by the mem op");
				for (uint regRm = 0; regRm <= 0x3F; regRm++) {
					var info = OpHelpers.InitializeInstruction(context, opFlags);
					Assert.True(info.IsValid);
					info.SetModrmRegRmBits(regRm);
					info.SetModrmModBits(3);
					info.InitializeXOP0to7();
					context.Fuzzer.Write(info);
					yield return new FuzzerGenResult(isValid: false);
				}
				break;

			default:
				throw ThrowHelpers.Unreachable;
			}
		}
	}

	// Gen an instruction with a modrm byte and reg and mem ops. If mem op, use [base+index] sib byte. Uses small reg values.
	sealed class InvalidFuzzerGen : FuzzerGen {
		public override IEnumerable<FuzzerGenResult> Generate(FuzzerGenContext context) {
			Assert.False(context.Instruction.IsValid);

			var info = InstructionInfo.Create(context);
			if (context.Instruction.OpCode.IsOneByte) {
				if (context.Instruction.IsModrmMemory) {
					if (context.Instruction.GroupIndex < 0)
						info.SetRegister(FuzzerOperandRegLocation.ModrmRegBits, 1);
					if (context.Instruction.RmGroupIndex < 0) {
						if (info.Bitness == 16)
							info.addressSizePrefix = 0x67;
						info.SetModrmSibMemory(0x04, 0xD1); // [rcx+rdx*8]
					}
				}
				else {
					if (context.Instruction.GroupIndex < 0)
						info.SetRegister(FuzzerOperandRegLocation.ModrmRegBits, 1);
					if (context.Instruction.RmGroupIndex < 0)
						info.SetRegister(FuzzerOperandRegLocation.ModrmRmBits, 2);
				}
			}
			info.InitializeXOP0to7();
			Assert.True(info.IsValid);
			context.Fuzzer.Write(info);
			yield return new FuzzerGenResult(isValid: false);
		}
	}
}
