// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Iced.Intel;

namespace IcedFuzzer.Core {
	public sealed class EncodingTables {
		public readonly List<FuzzerOpCode> Legacy = new List<FuzzerOpCode>();
		public readonly List<FuzzerOpCode> D3now = new List<FuzzerOpCode>();
		public readonly List<FuzzerOpCode> VEX = new List<FuzzerOpCode>();
		public readonly List<FuzzerOpCode> XOP = new List<FuzzerOpCode>();
		public readonly List<FuzzerOpCode> EVEX = new List<FuzzerOpCode>();
		public readonly List<FuzzerOpCode> MVEX = new List<FuzzerOpCode>();

		public IEnumerable<(EncodingKind encoding, List<FuzzerOpCode> opCodes)> GetOpCodeGroups() {
			yield return (EncodingKind.Legacy, Legacy);
			yield return (EncodingKind.D3NOW, D3now);
			yield return (EncodingKind.VEX, VEX);
			yield return (EncodingKind.XOP, XOP);
			yield return (EncodingKind.EVEX, EVEX);
			yield return (EncodingKind.MVEX, MVEX);
		}
	}

	public enum InstrGenFlags : uint {
		None				= 0,
		UnusedTables		= 0x00000001,
		NoVEX				= 0x00000002,
		NoXOP				= 0x00000004,
		NoEVEX				= 0x00000008,
		No3DNow				= 0x00000010,
		NoMVEX				= 0x00000020,
	}

	public static class InstrGen {
		readonly struct OpCodeKey : IEquatable<OpCodeKey> {
			public readonly FuzzerOpCodeTable Table;
			public readonly bool IsModrmMemory;

			public OpCodeKey(FuzzerOpCodeTable table, bool isModrmMemory) {
				Table = table;
				IsModrmMemory = isModrmMemory;
			}

			public override bool Equals(object? obj) => obj is OpCodeKey key && Equals(key);
			public bool Equals(OpCodeKey other) => Table == other.Table;
			public override int GetHashCode() => Table.GetHashCode() ^ (int)(IsModrmMemory ? 0 : uint.MaxValue);
		}

		sealed class FuzzerInstructions {
			public readonly List<FuzzerInstruction> Instructions;
			public readonly bool IsModrmMemory;
			public int HasModrmCount;
			public int HasNoModrmCount;
			public bool NotUsed;
			public FuzzerInstructions(bool isModrmMemory) {
				IsModrmMemory = isModrmMemory;
				Instructions = new List<FuzzerInstruction>();
				NotUsed = false;
			}
			public void Add(bool hasModrm, FuzzerInstruction instruction) {
				Instructions.Add(instruction);
				if (hasModrm || instruction.OpCode.IsTwobyte)
					HasModrmCount++;
				else
					HasNoModrmCount++;
			}
			public void VerifyModrmCounts() {
				if (HasModrmCount != 0 || HasNoModrmCount != 0)
					Assert.True(HasModrmCount == 0 || HasNoModrmCount == 0);
			}
		}

		static FuzzerInstructions[] CreateOpCodes(bool isModrmMemory) {
			var opCodes = new FuzzerInstructions[0x100];
			for (int j = 0; j < opCodes.Length; j++)
				opCodes[j] = new FuzzerInstructions(isModrmMemory);
			return opCodes;
		}

		static bool IsRegOpCodeInstruction(FuzzerInstruction instr) {
			foreach (var operand in instr.RegisterOperands) {
				if (operand.RegLocation != FuzzerOperandRegLocation.OpCodeBits)
					continue;
				if (operand.Register != FuzzerRegisterKind.ST) {
					Assert.True((instr.OpCode.Byte0 & 7) == 0);
					return true;
				}
			}
			return false;
		}

		public static EncodingTables Create(int bitness, OpCodeInfo[] opCodes, InstrGenFlags genFlags) {
			var encodingTables = new EncodingTables();

			var legacy = new Dictionary<OpCodeKey, FuzzerInstructions[]>();
			for (int i = 0; i < 2; i++) {
				bool isModrmMemory = i != 0;
				var maxTable = (genFlags & InstrGenFlags.UnusedTables) != 0 ? OpCodeTableIndexes.LegacyTable_Max : OpCodeTableIndexes.LegacyTable_MaxUsed;
				for (int table = 0; table <= maxTable; table++)
					legacy.Add(new OpCodeKey(new FuzzerOpCodeTable(EncodingKind.Legacy, table), isModrmMemory), CreateOpCodes(isModrmMemory));
			}

			var d3now = new Dictionary<OpCodeKey, FuzzerInstructions[]>();
			if ((genFlags & InstrGenFlags.No3DNow) == 0) {
				for (int i = 0; i < 2; i++) {
					bool isModrmMemory = i != 0;
					d3now.Add(new OpCodeKey(new FuzzerOpCodeTable(EncodingKind.D3NOW, OpCodeTableIndexes.D3nowTable), isModrmMemory), CreateOpCodes(isModrmMemory));
				}
			}

			var vex = new Dictionary<OpCodeKey, FuzzerInstructions[]>();
			if ((genFlags & InstrGenFlags.NoVEX) == 0) {
				for (int i = 0; i < 2; i++) {
					bool isModrmMemory = i != 0;
					for (int table = 0; table < 0x20; table++) {
						bool usedTable = table == 1 || table == 2 || table == 3 || (table == 0 && (genFlags & InstrGenFlags.NoMVEX) == 0);
						if (usedTable || (genFlags & InstrGenFlags.UnusedTables) != 0)
							vex.Add(new OpCodeKey(new FuzzerOpCodeTable(EncodingKind.VEX, table), isModrmMemory), CreateOpCodes(isModrmMemory));
					}
				}
			}

			var xop = new Dictionary<OpCodeKey, FuzzerInstructions[]>();
			if ((genFlags & InstrGenFlags.NoXOP) == 0) {
				for (int i = 0; i < 2; i++) {
					bool isModrmMemory = i != 0;
					for (int table = 0; table < 0x20; table++) {
						bool usedTable = table == 8 || table == 9 || table == 10;
						if (usedTable || (genFlags & InstrGenFlags.UnusedTables) != 0)
							xop.Add(new OpCodeKey(new FuzzerOpCodeTable(EncodingKind.XOP, table), isModrmMemory), CreateOpCodes(isModrmMemory));
					}
				}
			}

			var evex = new Dictionary<OpCodeKey, FuzzerInstructions[]>();
			if ((genFlags & InstrGenFlags.NoEVEX) == 0) {
				for (int i = 0; i < 2; i++) {
					bool isModrmMemory = i != 0;
					for (int table = 0; table < 8; table++) {
						bool usedTable = table == 1 || table == 2 || table == 3 || table == 5 || table == 6;
						if (usedTable || (genFlags & InstrGenFlags.UnusedTables) != 0)
							evex.Add(new OpCodeKey(new FuzzerOpCodeTable(EncodingKind.EVEX, table), isModrmMemory), CreateOpCodes(isModrmMemory));
					}
				}
			}

			if ((genFlags & InstrGenFlags.NoMVEX) == 0) {
				throw new NotImplementedException();
			}

			foreach (var (hasModrm, instr) in GetInstructions(bitness, opCodes)) {
				OpCodeKey key;
				byte byteOpCode = instr.GetByteOpCode();
				switch (instr.Table.Encoding) {
				case EncodingKind.Legacy:
					key = new OpCodeKey(instr.Table, instr.IsModrmMemory);
					var legacyInstrs = legacy[key];
					legacyInstrs[byteOpCode].Add(hasModrm, instr);
					if (instr.OpCode.IsOneByte) {
						if (IsRegOpCodeInstruction(instr)) {
							for (int i = 1; i < 8; i++)
								legacyInstrs[byteOpCode + i].NotUsed = true;
							var other = legacy[new OpCodeKey(instr.Table, !instr.IsModrmMemory)];
							for (int i = 0; i < 8; i++)
								other[byteOpCode + i].NotUsed = true;
						}
					}
					break;

				case EncodingKind.VEX:
					Assert.True((genFlags & InstrGenFlags.NoVEX) == 0);
					key = new OpCodeKey(instr.Table, instr.IsModrmMemory);
					var vexInstrs = vex[key];
					vexInstrs[byteOpCode].Add(hasModrm, instr);
					break;

				case EncodingKind.XOP:
					Assert.True((genFlags & InstrGenFlags.NoXOP) == 0);
					key = new OpCodeKey(instr.Table, instr.IsModrmMemory);
					var xopInstrs = xop[key];
					xopInstrs[byteOpCode].Add(hasModrm, instr);
					break;

				case EncodingKind.EVEX:
					Assert.True((genFlags & InstrGenFlags.NoEVEX) == 0);
					key = new OpCodeKey(instr.Table, instr.IsModrmMemory);
					var evexInstrs = evex[key];
					evexInstrs[byteOpCode].Add(hasModrm, instr);
					break;

				case EncodingKind.D3NOW:
					Assert.True((genFlags & InstrGenFlags.No3DNow) == 0);
					key = new OpCodeKey(instr.Table, instr.IsModrmMemory);
					var d3nowInstrs = d3now[key];
					d3nowInstrs[byteOpCode].Add(hasModrm, instr);
					break;

				case EncodingKind.MVEX:
					throw new NotImplementedException();

				default:
					throw ThrowHelpers.Unreachable;
				}
			}

			var allDicts = new[] {
				legacy,
				d3now,
				vex,
				xop,
				evex,
			};
			// If it's an instruction without a modrm byte, then don't create an invalid
			// instruction with a modrm byte (eg. opcode 04 = add al,imm8 has no modrm byte
			// so we can't create an invalid instruction with opcode 04 with a modrm byte).
			foreach (var dict in allDicts) {
				foreach (var kv in dict) {
					var key = kv.Key;
					var regKey = new OpCodeKey(key.Table, isModrmMemory: false);
					var memKey = new OpCodeKey(key.Table, isModrmMemory: true);
					var reg = dict[regKey];
					var mem = dict[memKey];
					Assert.True(reg.Length == mem.Length);
					for (int i = 0; i < reg.Length; i++) {
						var regi = reg[i];
						var memi = mem[i];
						regi.VerifyModrmCounts();
						memi.VerifyModrmCounts();
						if (regi.HasNoModrmCount != 0) {
							Assert.True(memi.HasModrmCount == 0 && memi.HasNoModrmCount == 0);
							memi.NotUsed = true;
						}
					}
				}
			}

			var flags = new LegacyFlags[0x40];
			foreach (var kv in legacy) {
				var key = kv.Key;
				Assert.True(kv.Value.Length == 0x100);
				for (int opCode = 0; opCode < kv.Value.Length; opCode++) {
					// Some instructions encode the operand in the low 3 bits (eg. xchg rAX,reg). Ignore those
					// opcodes if the low 3 bits = 1..7.
					if (kv.Value[opCode].NotUsed) {
						Assert.True(kv.Value[opCode].Instructions.Count == 0);
						continue;
					}
					var fuzzerOpCode = new FuzzerOpCode(key.Table, (byte)opCode, key.IsModrmMemory);
					fuzzerOpCode.Instructions.AddRange(GetLegacyInstructions(bitness, genFlags, flags, key.Table, (byte)opCode, kv.Value[opCode].Instructions, kv.Value[opCode].IsModrmMemory));
					if (fuzzerOpCode.Instructions.Count != 0)
						encodingTables.Legacy.Add(fuzzerOpCode);
				}
			}

			foreach (var kv in d3now) {
				var key = kv.Key;
				Assert.True(kv.Value.Length == 0x100);
				for (int opCode = 0; opCode < kv.Value.Length; opCode++) {
					if (kv.Value[opCode].NotUsed) {
						Assert.True(kv.Value[opCode].Instructions.Count == 0);
						continue;
					}
					var fuzzerOpCode = new FuzzerOpCode(key.Table, (byte)opCode, key.IsModrmMemory);
					fuzzerOpCode.Instructions.AddRange(Get3DNowInstructions((byte)opCode, kv.Value[opCode].Instructions, kv.Value[opCode].IsModrmMemory));
					if (fuzzerOpCode.Instructions.Count != 0)
						encodingTables.D3now.Add(fuzzerOpCode);
				}
			}

			foreach (var kv in vex) {
				var key = kv.Key;
				Assert.True(kv.Value.Length == 0x100);
				for (int opCode = 0; opCode < kv.Value.Length; opCode++) {
					if (kv.Value[opCode].NotUsed) {
						Assert.True(kv.Value[opCode].Instructions.Count == 0);
						continue;
					}
					var fuzzerOpCode = new FuzzerOpCode(key.Table, (byte)opCode, key.IsModrmMemory);
					fuzzerOpCode.Instructions.AddRange(GetVecInstructions(key.Table, (byte)opCode, kv.Value[opCode].Instructions, kv.Value[opCode].IsModrmMemory));
					if (fuzzerOpCode.Instructions.Count != 0)
						encodingTables.VEX.Add(fuzzerOpCode);
				}
			}

			foreach (var kv in xop) {
				var key = kv.Key;
				Assert.True(kv.Value.Length == 0x100);
				for (int opCode = 0; opCode < kv.Value.Length; opCode++) {
					if (kv.Value[opCode].NotUsed) {
						Assert.True(kv.Value[opCode].Instructions.Count == 0);
						continue;
					}
					var fuzzerOpCode = new FuzzerOpCode(key.Table, (byte)opCode, key.IsModrmMemory);
					fuzzerOpCode.Instructions.AddRange(GetVecInstructions(key.Table, (byte)opCode, kv.Value[opCode].Instructions, kv.Value[opCode].IsModrmMemory));
					if (fuzzerOpCode.Instructions.Count != 0)
						encodingTables.XOP.Add(fuzzerOpCode);
				}
			}

			foreach (var kv in evex) {
				var key = kv.Key;
				Assert.True(kv.Value.Length == 0x100);
				for (int opCode = 0; opCode < kv.Value.Length; opCode++) {
					if (kv.Value[opCode].NotUsed) {
						Assert.True(kv.Value[opCode].Instructions.Count == 0);
						continue;
					}
					var fuzzerOpCode = new FuzzerOpCode(key.Table, (byte)opCode, key.IsModrmMemory);
					fuzzerOpCode.Instructions.AddRange(GetVecInstructions(key.Table, (byte)opCode, kv.Value[opCode].Instructions, kv.Value[opCode].IsModrmMemory));
					if (fuzzerOpCode.Instructions.Count != 0)
						encodingTables.EVEX.Add(fuzzerOpCode);
				}
			}

			// Verify that all input Code values are used. Reserved-nops aren't guaranteed to be used
			// since other instructions can override them.
			const bool filterOutReservednop = true;
			var hash = opCodes.Where(a => !filterOutReservednop || !a.IsReservedNop).Select(a => a.Code).ToHashSet();
			foreach (var (_, fuzzerOpCodes) in encodingTables.GetOpCodeGroups()) {
				foreach (var fuzzerOpCode in fuzzerOpCodes) {
					foreach (var instr in fuzzerOpCode.Instructions)
						hash.Remove(instr.Code);
				}
			}
			Assert.True(hash.Count == 0, "Missing Code values: " + string.Join(", ", hash.ToArray()));

			return encodingTables;
		}

		static int GetRegGroupIndex(FuzzerInstruction instr) {
			Assert.True(instr.GroupIndex < 0 || instr.RmGroupIndex < 0);
			Assert.True(!instr.IsModrmMemory);
			if (instr.OpCode.IsOneByte) {
				if (instr.GroupIndex >= 0)
					return instr.GroupIndex << 3;
				if (instr.RmGroupIndex >= 0)
					return instr.RmGroupIndex;
				throw ThrowHelpers.Unreachable;
			}
			else if (instr.OpCode.IsTwobyte) {
				uint modrm = instr.OpCode.Byte1;
				Assert.True(modrm >= 0xC0);
				return (int)(modrm - 0xC0);
			}
			else
				throw ThrowHelpers.Unreachable;
		}

		[Flags]
		enum LegacyFlags : byte {
			None = 1 << MandatoryPrefix.None,
			PNP = 1 << MandatoryPrefix.PNP,
			P66 = 1 << MandatoryPrefix.P66,
			PF3 = 1 << MandatoryPrefix.PF3,
			PF2 = 1 << MandatoryPrefix.PF2,
			NP_OpSize_1632 = 0x40,
			Legacy_OpSize_1632 = 0x80,
		}

		enum InvalidInstructionKind {
			Full,
			Group,
			TwoByte,
		}

		readonly struct LegacyInfo {
			public readonly List<FuzzerInstruction>[] Instructions;
			public bool HasNone => (Flags & LegacyFlags.None) != 0;
			public bool HasNP => (Flags & LegacyFlags.PNP) != 0;
			public bool Has66 => (Flags & LegacyFlags.P66) != 0;
			public bool HasF3 => (Flags & LegacyFlags.PF3) != 0;
			public bool HasF2 => (Flags & LegacyFlags.PF2) != 0;
			public bool NP_OpSize_1632 => (Flags & LegacyFlags.NP_OpSize_1632) != 0;
			public bool Legacy_OpSize_1632 => (Flags & LegacyFlags.Legacy_OpSize_1632) != 0;
			public bool Has(MandatoryPrefix prefix) =>
				prefix switch {
					MandatoryPrefix.None => HasNone,
					MandatoryPrefix.PNP => HasNP,
					MandatoryPrefix.P66 => Has66,
					MandatoryPrefix.PF3 => HasF3,
					MandatoryPrefix.PF2 => HasF2,
					_ => throw ThrowHelpers.Unreachable,
				};
			public List<FuzzerInstruction> NoPrefix => Instructions[(int)MandatoryPrefix.None];
			public List<FuzzerInstruction> PNP => Instructions[(int)MandatoryPrefix.PNP];
			public List<FuzzerInstruction> P66 => Instructions[(int)MandatoryPrefix.P66];
			public List<FuzzerInstruction> PF3 => Instructions[(int)MandatoryPrefix.PF3];
			public List<FuzzerInstruction> PF2 => Instructions[(int)MandatoryPrefix.PF2];
			public readonly LegacyFlags Flags;
			public LegacyInfo(LegacyFlags flags) {
				Flags = flags;
				// 5 = no mandatory prefix, NP, 66, F3, F2
				var instrs = new List<FuzzerInstruction>[5];
				for (int i = 0; i < instrs.Length; i++)
					instrs[i] = new List<FuzzerInstruction>();
				Instructions = instrs;
			}

			public void ClearAll() {
				foreach (var instrs in Instructions)
					instrs.Clear();
			}

			public void SetInstructionFlags(MandatoryPrefix prefix, FuzzerInstructionFlags flags) {
				foreach (var instr in Instructions[(int)prefix])
					instr.Flags |= flags;
			}

			public bool IsInvalidMandatoryPrefixInstructions(InvalidInstructionKind kind) {
				for (int i = 0; i < Instructions.Length; i++) {
					if (!IsInvalidMandatoryPrefixInstructions((MandatoryPrefix)i, kind))
						return false;
				}
				return true;
			}

			public bool IsInvalidMandatoryPrefixInstructions(MandatoryPrefix prefix, InvalidInstructionKind kind) {
				var instrs = Instructions[(int)prefix];
				if (prefix == MandatoryPrefix.None) {
					if (instrs.Count != 0)
						return false;
				}
				else {
					if (instrs.Count != 1)
						return false;
					var instr = instrs[0];
					if (instr.IsValid)
						return false;
					InvalidInstructionKind itsKind;
					if (instr.OpCode.IsOneByte && instr.GroupIndex < 0)
						itsKind = InvalidInstructionKind.Full;
					else if (instr.GroupIndex >= 0)
						itsKind = InvalidInstructionKind.Group;
					else if (instr.OpCode.IsTwobyte)
						itsKind = InvalidInstructionKind.TwoByte;
					else
						throw ThrowHelpers.Unreachable;
					if (kind != itsKind)
						return false;
				}

				return true;
			}

			public bool IsEmpty() {
				foreach (var instr in Instructions) {
					if (instr.Count != 0)
						return false;
				}
				return true;
			}

			public bool IsEmpty(MandatoryPrefix prefix) => Instructions[(int)prefix].Count == 0;

			public bool IsInvalidOrEmpty(MandatoryPrefix prefix) =>
				Instructions[(int)prefix].Count == 0 ||
				(Instructions[(int)prefix].Count == 1 && !Instructions[(int)prefix][0].IsValid);
		}

		readonly struct LegacyKey : IEquatable<LegacyKey> {
			readonly MandatoryPrefix mandatoryPrefix;
			readonly int operandSize;
			readonly int addressSize;
			readonly OpCode opCode;
			readonly int groupIndex;

			public LegacyKey(MandatoryPrefix mandatoryPrefix, int operandSize, int addressSize, OpCode opCode, int groupIndex) {
				this.mandatoryPrefix = mandatoryPrefix;
				this.operandSize = operandSize;
				this.addressSize = addressSize;
				this.opCode = opCode;
				this.groupIndex = groupIndex;
			}

			public static bool operator ==(LegacyKey left, LegacyKey right) => left.Equals(right);
			public static bool operator !=(LegacyKey left, LegacyKey right) => !left.Equals(right);
			public override bool Equals(object? obj) => obj is LegacyKey key && Equals(key);
			public bool Equals(LegacyKey other) =>
				mandatoryPrefix == other.mandatoryPrefix && operandSize == other.operandSize && addressSize == other.addressSize &&
				opCode == other.opCode && groupIndex == other.groupIndex;
			public override int GetHashCode() => (int)mandatoryPrefix ^ (operandSize << 8) ^ (addressSize << 16) ^ opCode.GetHashCode() ^ (groupIndex << 12);
		}

		static bool HasSTiOp(FuzzerInstruction instruction) {
			foreach (var operand in instruction.RegisterOperands) {
				if (operand.RegLocation == FuzzerOperandRegLocation.OpCodeBits && operand.Register == FuzzerRegisterKind.ST)
					return true;
			}
			return false;
		}

		sealed class ReservednopInfo {
			public FuzzerInstruction? Instr16;
			public FuzzerInstruction? Instr32;
			public FuzzerInstruction? Instr64;
			public List<FuzzerInstruction> GetInstructions() {
				Assert.False(Instr16 is null || Instr32 is null);
				var instrs = new List<FuzzerInstruction>(Instr64 is not null ? 3 : 2) {
					Instr16,
					Instr32
				};
				if (Instr64 is not null)
					instrs.Add(Instr64);
				return instrs;
			}
		}

		static int GetRealGroupIndex(FuzzerInstruction instr) {
			if (instr.OpCode.IsOneByte)
				return instr.GroupIndex;
			if (instr.OpCode.IsTwobyte)
				return (instr.OpCode.Byte1 >> 3) & 7;
			throw ThrowHelpers.Unreachable;
		}

		static IEnumerable<FuzzerInstruction> GetLegacyInstructions(int bitness, InstrGenFlags genFlags, LegacyFlags[] flags, FuzzerOpCodeTable table, byte opCode, List<FuzzerInstruction> instructions, bool isModrmMemory) {
			Assert.True(table.Encoding == EncodingKind.Legacy);
			Assert.True(flags.Length == 0x40);

			bool xopCheck = false;
			// Check if it's a prefix or a disabled VEX/XOP/EVEX/MVEX/3DNow instruction
			if (table.TableIndex == 0) {
				switch (opCode) {
				case 0x0F:
					Assert.True(instructions.Count == 0);
					yield break;

				case 0x26: // es
				case 0x2E: // cs
				case 0x36: // ss
				case 0x3E: // ds
				case 0x64: // fs
				case 0x65: // gs
				case 0x66: // opsize
				case 0x67: // addrsize
				case 0xF0: // lock
				case 0xF2: // repne
				case 0xF3: // rep/repe
					Assert.True(instructions.Count == 0);
					yield break;

				case 0x62:
					if ((genFlags & InstrGenFlags.NoEVEX) == 0) {
						// EVEX is present. In 16/32-bit mode, only reg form (modrm >= C0h) is EVEX prefix
						if (bitness == 64 || !isModrmMemory) {
							Assert.True(instructions.Count == 0);
							yield break;
						}
					}
					break;

				case 0x8F:
					// XOP uses reg=1-7 so we need special checks below
					xopCheck = (genFlags & InstrGenFlags.NoXOP) == 0;
					break;

				case 0xC4:
				case 0xC5:
					if ((genFlags & InstrGenFlags.NoVEX) == 0) {
						// VEX is present. In 16/32-bit mode, only reg form (modrm >= C0h) is VEX2/VEX3 prefix
						if (bitness == 64 || !isModrmMemory) {
							Assert.True(instructions.Count == 0);
							yield break;
						}
					}
					break;

				default:
					// Check if REX
					if (bitness == 64 && (opCode & 0xF0) == 0x40) {
						Assert.True(instructions.Count == 0);
						yield break;
					}
					break;
				}
			}
			else if (table.TableIndex == 1) {
				switch (opCode) {
				case 0x0F:
					if ((genFlags & InstrGenFlags.No3DNow) == 0) {
						// 3DNow! is present
						Assert.True(instructions.Count == 0);
						yield break;
					}
					break;

				case 0x38:
				case 0x39:
				case 0x3A:
				case 0x3B:
				case 0x3C:
				case 0x3D:
				case 0x3E:
				case 0x3F:
					Assert.True(instructions.Count == 0);
					yield break;
				}
			}

			for (int i = 0; i < flags.Length; i++)
				flags[i] = 0;

			var allInstructions = new List<List<FuzzerInstruction>>(2);
			for (int i = 0; i < 2; i++)
				allInstructions.Add(new List<FuzzerInstruction>());

			// One mandatory prefix could be a group and another one a normal instruction,
			// eg. NP Vmread_rm64_r64 (normal), 66 Extrq_xmm_imm8_imm8 (group)
			const int INDEX_NORMAL = 0;
			const int INDEX_GROUP = 1;
			var instrHash = new HashSet<LegacyKey>();
			ReservednopInfo? resNop = null;
			foreach (var instruction in instructions) {
				Assert.True(instruction.RmGroupIndex < 0, "Not supported");
				if (instruction.IsReservedNop) {
					if (resNop is null)
						resNop = new ReservednopInfo();
					// We assume they don't use mandatory prefixes and only use OperandSize (16,32,64)
					Assert.True(instruction.MandatoryPrefix == MandatoryPrefix.None);
					switch (instruction.OperandSize) {
					case 16:
						Assert.False(resNop.Instr16 is not null);
						resNop.Instr16 = instruction;
						break;
					case 32:
						Assert.False(resNop.Instr32 is not null);
						resNop.Instr32 = instruction;
						break;
					case 64:
						Assert.False(resNop.Instr64 is not null);
						resNop.Instr64 = instruction;
						break;
					default:
						throw ThrowHelpers.Unreachable;
					}
					continue;
				}

				var key = new LegacyKey(instruction.MandatoryPrefix, instruction.OperandSize, instruction.AddressSize, instruction.OpCode, instruction.GroupIndex);
				if (!instrHash.Add(key)) {
					if (!IsNopXchgDupe(instructions, instruction.Code))
						Assert.Fail($"Dupe instruction: {instruction.Code}");
				}

				if (GetRealGroupIndex(instruction) >= 0)
					allInstructions[INDEX_GROUP].Add(instruction);
				else
					allInstructions[INDEX_NORMAL].Add(instruction);

				InitializeLegacyFlags(flags, instruction, instruction.OpCode, instruction.GroupIndex);
			}
			if (resNop is not null) {
				// We assume none of them were removed by the user. What's the point of removing just one of them instead of all of them?
				Assert.False(resNop.Instr16 is null || resNop.Instr32 is null || (bitness == 64 && resNop.Instr64 is null));
			}
			{
				bool removeNormal = false;
				bool removeGroup = false;
				if (allInstructions[INDEX_GROUP].Count != 0 && allInstructions[INDEX_NORMAL].Count == 0)
					removeNormal = true;
				if (allInstructions[INDEX_GROUP].Count == 0 && !removeNormal)
					removeGroup = true;
				var group = allInstructions[INDEX_GROUP];
				var normal = allInstructions[INDEX_NORMAL];
				allInstructions.Clear();
				if (!removeNormal)
					allInstructions.Add(normal);
				if (!removeGroup)
					allInstructions.Add(group);
				Assert.True(allInstructions.Count != 0);
			}

			for (int k = 0; k < allInstructions.Count; k++) {
				var instrs = allInstructions[k];
				if (k == 1) {
					// Currently only happens with 0F 78. Prevent the 'group' code from creating the
					// same invalid prefix instruction created by the 'non-group' code.
					LegacyFlags prefixFlags = 0;
					foreach (var ins in allInstructions) {
						foreach (var i in ins)
							prefixFlags |= (LegacyFlags)(1 << (int)i.MandatoryPrefix);
					}
					prefixFlags ^= LegacyFlags.PNP | LegacyFlags.P66 | LegacyFlags.PF3 | LegacyFlags.PF2;
					for (int l = 0; l < flags.Length; l++)
						flags[l] |= prefixFlags;
				}

				// Need to special case the group opcodes
				if (instrs.Any(a => GetRealGroupIndex(a) >= 0)) {
					Assert.True(instrs.All(a => GetRealGroupIndex(a) >= 0));
					if (isModrmMemory) {
						// Up to 8 instructions with mem ops

						var groupInfos = new LegacyInfo[8];
						for (int groupIndex = 0; groupIndex < groupInfos.Length; groupIndex++)
							groupInfos[groupIndex] = new LegacyInfo(flags[groupIndex << 3]);

						var groupInstrs = new List<FuzzerInstruction>[groupInfos.Length];
						for (int i = 0; i < groupInstrs.Length; i++)
							groupInstrs[i] = new List<FuzzerInstruction>();
						foreach (var instr in instrs)
							groupInstrs[instr.GroupIndex].Add(instr);

						for (int groupIndex = 0; groupIndex < groupInfos.Length; groupIndex++)
							AddLegacy(bitness, table, prefix => (new OpCode(opCode), groupIndex), isModrmMemory, ref groupInfos[groupIndex], groupInstrs[groupIndex]);

						// If there's a reserved-nop, replace all invalid instructions with a reserved-nop instruction
						if (resNop is not null) {
							for (int groupIndex = 0; groupIndex < groupInfos.Length; groupIndex++)
								InitializeResNop(bitness, flags, table, prefix => (new OpCode(opCode), groupIndex), isModrmMemory, ref groupInfos[groupIndex], groupIndex << 3, resNop, InvalidInstructionKind.Group, 0);
						}

						if (xopCheck) {
							// Make sure there are no instructions in the XOP range
							for (int i = 1; i < 8; i++) {
								Assert.True(groupInfos[i].IsInvalidMandatoryPrefixInstructions(InvalidInstructionKind.Group));
								groupInfos[i].ClearAll();
							}
						}

						foreach (var info in groupInfos) {
							foreach (var infoInstrs in info.Instructions) {
								foreach (var instr in infoInstrs) {
									// If there's a reserved nop, it must be used
									Assert.False(resNop is not null && !instr.IsValid);
									yield return instr;
								}
							}
						}
					}
					else {
						// 8 instructions with a reg operand or 64 instructions with no ops (one instruction per modrm byte C0-FFh)
						// or a mix of that.

						var infos = new LegacyInfo[64];
						for (int index = 0; index < infos.Length; index++)
							infos[index] = new LegacyInfo(flags[index]);

						var groupInstrs = new List<FuzzerInstruction>[infos.Length];
						for (int i = 0; i < groupInstrs.Length; i++)
							groupInstrs[i] = new List<FuzzerInstruction>();
						var rmGroups = new bool[8 * 5];
						foreach (var instr in instrs) {
							int index = GetRegGroupIndex(instr);
							if (instr.GroupIndex >= 0 || HasSTiOp(instr))
								rmGroups[GetMpGroupIndex(instr.MandatoryPrefix, GetRealGroupIndex(instr))] = true;
							groupInstrs[index].Add(instr);
						}

						static (OpCode opCode, int groupIndex) GetOpCodeAndGroup(bool[] rmGroups, int index, byte opCode, MandatoryPrefix prefix) {
							OpCode newOpCode;
							int groupIndex;
							if ((index & 7) == 0 && rmGroups[GetMpGroupIndex(prefix, index >> 3)]) {
								newOpCode = new OpCode(opCode);
								groupIndex = index >> 3;
							}
							else {
								newOpCode = new OpCode(opCode, (byte)(0xC0 | index));
								groupIndex = -1;
							}
							return (newOpCode, groupIndex);
						}

						for (int index = 0; index < infos.Length; index++) {
							(OpCode opCode, int groupIndex) GetOpCode(MandatoryPrefix prefix) => GetOpCodeAndGroup(rmGroups, index, opCode, prefix);
							AddLegacy(bitness, table, GetOpCode, isModrmMemory, ref infos[index], groupInstrs[index]);
						}

						static bool IsInvalidGroup(LegacyInfo[] infos, int index) {
							if ((index & 7) != 0)
								return false;
							if (infos[index].IsInvalidMandatoryPrefixInstructions(InvalidInstructionKind.Group))
								return true;
							for (int i = 0; i < 8; i++) {
								if (!infos[index + i].IsInvalidMandatoryPrefixInstructions(InvalidInstructionKind.TwoByte))
									return false;
							}
							return true;
						}
						static bool IsInvalidGroupPrefix(LegacyInfo[] infos, int index, MandatoryPrefix prefix) {
							if ((index & 7) != 0)
								return false;
							if (infos[index].IsInvalidMandatoryPrefixInstructions(prefix, InvalidInstructionKind.Group))
								return true;
							for (int i = 0; i < 8; i++) {
								if (!infos[index + i].IsInvalidMandatoryPrefixInstructions(prefix, InvalidInstructionKind.TwoByte))
									return false;
							}
							return true;
						}
						static bool IsResNopGroup(LegacyInfo[] infos, int index, MandatoryPrefix[] prefixes) {
							if ((index & 7) != 0)
								return false;
							foreach (var prefix in prefixes) {
								if (!infos[index].IsInvalidMandatoryPrefixInstructions(prefix, InvalidInstructionKind.Group))
									return false;
							}
							return true;
						}

						// Convert 8 consecutive invalid instructions to an invalid instruction with modrm.reg=N
						for (int groupIndex = 0; groupIndex < 8; groupIndex++) {
							int index = groupIndex << 3;
							if (!IsInvalidGroup(infos, index))
								continue;
							for (int i = 1; i < 8; i++)
								infos[index + i].ClearAll();
							foreach (var prefix in allMandatoryPrefixes)
								rmGroups[GetMpGroupIndex(prefix, groupIndex)] = true;
							infos[index] = new LegacyInfo(flags[index]);
							AddLegacy(bitness, table, prefix => (new OpCode(opCode), groupIndex), isModrmMemory, ref infos[index], groupInstrs[index]);
						}
						for (int groupIndex = 0; groupIndex < 8; groupIndex++) {
							int index = groupIndex << 3;
							foreach (var prefix in mandatoryPrefixes) {
								if (!IsInvalidGroupPrefix(infos, index, prefix))
									continue;
								for (int i = 0; i < 8; i++)
									infos[index + i].Instructions[(int)prefix].Clear();
								rmGroups[GetMpGroupIndex(prefix, groupIndex)] = true;
								AddLegacyMP(table, prefix => (new OpCode(opCode), groupIndex), isModrmMemory, ref infos[index], prefix);
								UpdateInstructionFlags(bitness, ref infos[index]);
							}
						}

						// If there's a reserved-nop, replace all invalid instructions with a reserved-nop instruction
						if (resNop is not null) {
							// Convert 8 consecutive invalid instructions to a reserved nop with modrm.reg=N
							for (int groupIndex = 0; groupIndex < 8; groupIndex++) {
								int index = groupIndex << 3;
								if (!IsResNopGroup(infos, index, allMandatoryPrefixes))
									continue;
								for (int i = 1; i < 8; i++)
									infos[index + i].ClearAll();
								foreach (var prefix in allMandatoryPrefixes)
									rmGroups[GetMpGroupIndex(prefix, groupIndex)] = true;
								InitializeResNop(bitness, flags, table, prefix => (new OpCode(opCode), groupIndex), isModrmMemory, ref infos[index], groupIndex << 3, resNop, InvalidInstructionKind.Group, 0);
							}

							// Convert 8 consecutive invalid instructions to a reserved nop with modrm.reg=N and a mandatory prefix (NP,66,F3,F2)
							for (int groupIndex = 0; groupIndex < 8; groupIndex++) {
								int index = groupIndex << 3;
								foreach (var prefix in mandatoryPrefixes) {
									if (!IsResNopGroup(infos, index, new[] { prefix }))
										continue;

									for (int i = 1; i < 8; i++)
										infos[index + i].Instructions[(int)prefix].Clear();
									rmGroups[GetMpGroupIndex(prefix, groupIndex)] = true;

									var ignoredPrefixes = LegacyFlags.PNP | LegacyFlags.P66 | LegacyFlags.PF3 | LegacyFlags.PF2;
									ignoredPrefixes &= ~(LegacyFlags)(1 << (int)prefix);

									InitializeResNop(bitness, flags, table, prefix => (new OpCode(opCode), groupIndex), isModrmMemory, ref infos[index], groupIndex << 3, resNop, InvalidInstructionKind.Group, ignoredPrefixes);
								}
							}

							for (int index = 0; index < infos.Length; index++) {
								LegacyFlags ignoredPrefixes = 0;
								if ((index & 7) != 0) {
									int groupIndex = index >> 3;
									if (rmGroups[GetMpGroupIndex(MandatoryPrefix.PNP, groupIndex)])
										ignoredPrefixes |= LegacyFlags.PNP;
									if (rmGroups[GetMpGroupIndex(MandatoryPrefix.P66, groupIndex)])
										ignoredPrefixes |= LegacyFlags.P66;
									if (rmGroups[GetMpGroupIndex(MandatoryPrefix.PF3, groupIndex)])
										ignoredPrefixes |= LegacyFlags.PF3;
									if (rmGroups[GetMpGroupIndex(MandatoryPrefix.PF2, groupIndex)])
										ignoredPrefixes |= LegacyFlags.PF2;
								}

								(OpCode opCode, int groupIndex) GetOpCode(MandatoryPrefix prefix) => GetOpCodeAndGroup(rmGroups, index, opCode, prefix);
								InitializeResNop(bitness, flags, table, GetOpCode, isModrmMemory, ref infos[index], index, resNop, InvalidInstructionKind.TwoByte, ignoredPrefixes);
							}
						}

						if (xopCheck) {
							// Make sure there are no instructions in the XOP range
							for (int i = 1; i < 8; i++) {
								Assert.True(infos[i << 3].IsInvalidMandatoryPrefixInstructions(InvalidInstructionKind.Group));
								infos[i << 3].ClearAll();
							}
						}

						foreach (var prefix in allMandatoryPrefixes) {
							for (int i = 0; i < 8; i++) {
								if (!rmGroups[GetMpGroupIndex(prefix, i)])
									continue;
								int index = i << 3;
								for (int j = 1; j < 8; j++) {
									var info = infos[index + j];
									Assert.True(info.IsEmpty(prefix), "At least one no-ops instruction is in the same range as a group instruction");
									info.Instructions[(int)prefix].Clear();
								}
							}
						}

						foreach (var info in infos) {
							foreach (var infoInstrs in info.Instructions) {
								foreach (var instr in infoInstrs) {
									// If there's a reserved nop, it must be used
									Assert.False(resNop is not null && !instr.IsValid);
									yield return instr;
								}
							}
						}
					}
				}
				else {
					// Non-group instructions

					if (xopCheck && !instructions.Any(a => GetRealGroupIndex(a) >= 0)) {
						// Seems like `pop r/m` was removed
						Assert.True(instrs.Count == 0);
						// XOP only uses reg=1-7 (reg=4 sets XOP.B=1 and uses XOP tables 0-7, reg=0 is pop rm)
						yield return FuzzerInstruction.CreateInvalidLegacy(table, new OpCode(opCode), 0, isModrmMemory, MandatoryPrefix.None);
						yield break;
					}

					var info = new LegacyInfo(flags[0]);
					AddLegacy(bitness, table, prefix => (new OpCode(opCode), -1), isModrmMemory, ref info, instrs);

					// If there's a reserved-nop, replace all invalid instructions with a reserved-nop instruction
					if (resNop is not null)
						InitializeResNop(bitness, flags, table, prefix => (new OpCode(opCode), -1), isModrmMemory, ref info, 0, resNop, InvalidInstructionKind.Full, 0);

					foreach (var infoInstrs in info.Instructions) {
						foreach (var instr in infoInstrs) {
							// If there's a reserved nop, it must be used
							Assert.False(resNop is not null && !instr.IsValid);
							yield return instr;
						}
					}
				}
			}
		}

		static void InitializeResNop(int bitness, LegacyFlags[] flags, FuzzerOpCodeTable table, Func<MandatoryPrefix, (OpCode opCode, int groupIndex)> getOpCode, bool isModrmMemory, ref LegacyInfo info, int flagsIndex, ReservednopInfo resNop, InvalidInstructionKind invalidKind, LegacyFlags ignoredPrefixes) {
			var resNopInstrs = resNop.GetInstructions();
			if (ignoredPrefixes == 0 && info.IsInvalidMandatoryPrefixInstructions(invalidKind)) {
				var (opCode, groupIndex) = getOpCode(MandatoryPrefix.None);
				foreach (var instr in resNopInstrs)
					InitializeLegacyFlags(flags, instr, opCode, groupIndex);
				Assert.True(info.Instructions[(int)MandatoryPrefix.None].Count == 0);
				info = new LegacyInfo(flags[flagsIndex]);
				resNopInstrs = resNopInstrs.Select(instr => instr.WithGroup(opCode, groupIndex)).ToList();
				AddLegacy(bitness, table, getOpCode, isModrmMemory, ref info, resNopInstrs);
			}
			else
				AddResNop(bitness, getOpCode, isModrmMemory, ref info, resNopInstrs, ignoredPrefixes);
		}

		static void InitializeLegacyFlags(LegacyFlags[] flags, FuzzerInstruction instruction, OpCode opCode, int groupIndex) {
			LegacyFlags flag;
			// Only a few instructions are NFx: rdrand, rdseed, movbe. bsf/bsr are similar but they allow garbage F2 (F3 is lzcnt/tzcnt).
			if (instruction.NFx)
				flag = LegacyFlags.PNP | LegacyFlags.P66;
			else
				flag = (LegacyFlags)(1 << (int)instruction.MandatoryPrefix);
			// Some instructions use only REX.W (no 66h), eg. sysretd/sysretq
			if (instruction.OperandSize == 16 || instruction.OperandSize == 32) {
				if (instruction.MandatoryPrefix == MandatoryPrefix.None)
					flag |= LegacyFlags.Legacy_OpSize_1632;
				if (instruction.MandatoryPrefix == MandatoryPrefix.PNP)
					flag |= LegacyFlags.NP_OpSize_1632;
			}
			int realGroupIndex = groupIndex;
			if (opCode.IsTwobyte)
				realGroupIndex = (opCode.Byte1 >> 3) & 7;
			if (realGroupIndex < 0) {
				for (int i = 0; i < 64; i++)
					flags[i] |= flag;
			}
			else if (groupIndex >= 0 || HasSTiOp(instruction)) {
				int index = realGroupIndex << 3;
				for (int i = 0; i < 8; i++) {
					flags[index + i] |= flag;
				}
			}
			else {
				Assert.False(opCode.IsOneByte);
				int index = opCode.Byte1 - 0xC0;
				flags[index] |= flag;
			}
		}

		static void AddResNop(int bitness, Func<MandatoryPrefix, (OpCode opCode, int groupIndex)> getOpCode, bool isModrmMemory, ref LegacyInfo info, List<FuzzerInstruction> instrs, LegacyFlags ignoredPrefixes) {
			if (info.Instructions[(int)MandatoryPrefix.None].Count != 0)
				return;
			bool hasNP = !info.IsInvalidOrEmpty(MandatoryPrefix.PNP) || (ignoredPrefixes & LegacyFlags.PNP) != 0;
			bool has66 = !info.IsInvalidOrEmpty(MandatoryPrefix.P66) || (ignoredPrefixes & LegacyFlags.P66) != 0;
			bool hasF3 = !info.IsInvalidOrEmpty(MandatoryPrefix.PF3) || (ignoredPrefixes & LegacyFlags.PF3) != 0;
			bool hasF2 = !info.IsInvalidOrEmpty(MandatoryPrefix.PF2) || (ignoredPrefixes & LegacyFlags.PF2) != 0;
			if (hasNP && has66 && hasF3 && hasF2)
				return;
			if (!hasNP && !has66 && !hasF3 && !hasF2)
				return;

			// If F3/F2 are unused, we can use an opsize prefix (66), eg. 66 F3 ... is used by some instrs, eg. POPCNT, TZCNT, LZCNT, CRC32
			var mpInfos = new List<(bool usesPrefix, MandatoryPrefix mandatoryPrefix)> {
				(hasF3, MandatoryPrefix.PF3),
				(hasF2, MandatoryPrefix.PF2),
			};
			if (!hasNP && !has66) {
				// Both NP and 66 are free so we can use opsize
				info.Instructions[(int)MandatoryPrefix.PNP].Clear();
				info.Instructions[(int)MandatoryPrefix.P66].Clear();
				mpInfos.Add((false, MandatoryPrefix.None));
			}
			else if (!hasNP)
				mpInfos.Add((hasNP, MandatoryPrefix.PNP));
			else if (!has66)
				mpInfos.Add((has66, MandatoryPrefix.P66));

			foreach (var (usesPrefix, mandatoryPrefix) in mpInfos) {
				if (!usesPrefix) {
					Assert.True(info.IsInvalidOrEmpty(mandatoryPrefix));
					info.Instructions[(int)mandatoryPrefix].Clear();
					foreach (var instr in instrs) {
						// If NP, we know 66 is used and it can't use a 66 operand size prefix
						if (mandatoryPrefix == MandatoryPrefix.PNP) {
							Assert.True(has66);
							if (bitness == 16) {
								if (instr.OperandSize != 16)
									continue;// Can't use a 66 prefix
							}
							else {
								// REX.W can select 64-bit operand size
								if (instr.OperandSize == 16)
									continue;// Can't use a 66 prefix
							}
						}
						// If 66, we know NP is used and we must always use a 66 operand size prefix
						if (mandatoryPrefix == MandatoryPrefix.P66) {
							Assert.True(hasNP);
							if (bitness == 16) {
								if (instr.OperandSize == 16)
									continue;// Must use a 66 prefix
							}
							else {
								// REX.W can select 64-bit operand size
								if (instr.OperandSize == 32)
									continue;// Must use a 66 prefix
							}
						}

						var (opCode, groupIndex) = getOpCode(mandatoryPrefix);
						var finstr = FuzzerInstruction.CreateValid(instr.Code, isModrmMemory, 0, 0, mandatoryPrefix, groupIndex, opCode);
						if (has66)
							finstr.Flags |= FuzzerInstructionFlags.DontUsePrefix66;
						// F3/F2 are reserved nops or other valid instructions
						finstr.Flags |= FuzzerInstructionFlags.DontUsePrefixF3 | FuzzerInstructionFlags.DontUsePrefixF2;
						info.Instructions[(int)mandatoryPrefix].Add(finstr);
					}
					Assert.True(info.Instructions[(int)mandatoryPrefix].Count != 0);
				}
			}

			UpdateInstructionFlags(bitness, ref info);
		}

		static void AddLegacyMP(FuzzerOpCodeTable table, Func<MandatoryPrefix, (OpCode opCode, int groupIndex)> getOpCode, bool isModrmMemory, ref LegacyInfo info, MandatoryPrefix prefix) {
			// Don't add a 66 prefix if NP uses opsize
			if (prefix == MandatoryPrefix.P66 && info.NP_OpSize_1632)
				return;
			if (info.Has(prefix))
				return;

			var (opCode, groupIndex) = getOpCode(prefix);
			info.Instructions[(int)prefix].Add(FuzzerInstruction.CreateInvalidLegacy(table, opCode, groupIndex, isModrmMemory, prefix));
		}

		static void AddLegacy(int bitness, FuzzerOpCodeTable table, Func<MandatoryPrefix, (OpCode opCode, int groupIndex)> getOpCode, bool isModrmMemory, ref LegacyInfo info, List<FuzzerInstruction> instrs) {
			bool isXchgAcc = false;
			bool isNop = false;
			foreach (var instr in instrs) {
				if (instr.IsXchgRegAcc)
					isXchgAcc = true;
				else if (instr.IsNop)
					isNop = true;
				int keyIndex = (int)instr.MandatoryPrefix;
				info.Instructions[keyIndex].Add(instr);
			}

			if (!info.HasNone) {
				// Here if there's no instruction or if all instructions use a mandatory prefix

				foreach (var prefix in mandatoryPrefixes)
					AddLegacyMP(table, getOpCode, isModrmMemory, ref info, prefix);
			}
			else {
				// Here if there's 1+ normal legacy instructions and 0+ instructions with a mandatory prefix

				// If there's a legacy instruction, NP instructions can't be used since they both use the same opcode without a prefix
				if (info.HasNP) {
					var (opCode, groupIndex) = getOpCode(MandatoryPrefix.None);
					Assert.Fail($"OpCode {opCode:X2} (g={groupIndex}) table {table.TableIndex}: Legacy + NP instruction");
				}
				if (info.Has66 && info.Legacy_OpSize_1632) {
					var (opCode, groupIndex) = getOpCode(MandatoryPrefix.None);
					Assert.Fail($"OpCode {opCode:X2} (g={groupIndex}) table {table.TableIndex}: o16/32/64 legacy + 66 mandatory prefix instruction");
				}

				if (info.Has66)
					info.SetInstructionFlags(MandatoryPrefix.None, FuzzerInstructionFlags.DontUsePrefix66);
				if (info.HasF3)
					info.SetInstructionFlags(MandatoryPrefix.None, FuzzerInstructionFlags.DontUsePrefixF3);
				if (info.HasF2)
					info.SetInstructionFlags(MandatoryPrefix.None, FuzzerInstructionFlags.DontUsePrefixF2);

				// F3 90 = pause, but F3 xchg reg,rAX is an ignored prefix
				if (isNop && isXchgAcc) {
					foreach (var instr in info.Instructions[(int)MandatoryPrefix.None]) {
						if (instr.IsXchgRegAcc)
							instr.Flags &= ~(FuzzerInstructionFlags.DontUsePrefixF3 | FuzzerInstructionFlags.DontUsePrefixF2);
					}
				}
			}

			UpdateInstructionFlags(bitness, ref info);
		}

		static void UpdateInstructionFlags(int bitness, ref LegacyInfo info) {
			// If an instruction uses REX.W, 66 or 67, mark the prefix as used (not ignored)
			for (int i = 0; i < info.Instructions.Length; i++) {
				var prefix = (MandatoryPrefix)i;
				var fflags = FuzzerInstructionFlags.None;
				foreach (var instr in info.Instructions[i]) {
					if (instr.OperandSize == 64 && !instr.DefaultOperandSize64)
						fflags |= FuzzerInstructionFlags.DontUsePrefixREXW;
					if (bitness == 16) {
						if (instr.OperandSize == 32)
							fflags |= FuzzerInstructionFlags.DontUsePrefix66;
					}
					else {
						if (instr.OperandSize == 16)
							fflags |= FuzzerInstructionFlags.DontUsePrefix66;
					}
					if (instr.AddressSize != 0 && instr.AddressSize != bitness)
						fflags |= FuzzerInstructionFlags.DontUsePrefix67;
					if (instr.RequiresAddressSize32 && instr.AddressSize == 32)
						fflags |= FuzzerInstructionFlags.DontUsePrefix67;
				}
				info.SetInstructionFlags(prefix, fflags);
				if ((fflags & FuzzerInstructionFlags.DontUsePrefixREXW) == 0) {
					// No code uses REX.W. o32/o64 instruction can use REX.W (ignored) but o16 instruction can't use it
					if (info.Instructions[i].Count > 1) {
						foreach (var instr in info.Instructions[i]) {
							if (instr.OperandSize == 16)
								instr.Flags |= FuzzerInstructionFlags.DontUsePrefixREXW;
						}
					}
				}
			}
		}

		static int GetMpGroupIndex(MandatoryPrefix mandatoryPrefix, int groupIndex) {
			Assert.True(groupIndex >= 0);
			return groupIndex + (int)mandatoryPrefix * 8;
		}

		static readonly (Code code1, Code code2)[] nopXchgDupes = new (Code, Code)[] {
			(Code.Nopw, Code.Xchg_r16_AX),
			(Code.Nopd, Code.Xchg_r32_EAX),
			(Code.Nopq, Code.Xchg_r64_RAX),
			(Code.Xchg_r16_AX, Code.Nopw),
			(Code.Xchg_r32_EAX, Code.Nopd),
			(Code.Xchg_r64_RAX, Code.Nopq),
		};
		static bool IsNopXchgDupe(List<FuzzerInstruction> instructions, Code code) {
			foreach (var info in nopXchgDupes) {
				if (code == info.code1)
					return instructions.Any(a => a.Code == info.code2);
			}
			return false;
		}

		static IEnumerable<FuzzerInstruction> Get3DNowInstructions(byte opCode, List<FuzzerInstruction> instructions, bool isModrmMemory) {
			switch (instructions.Count) {
			case 0:
				yield return FuzzerInstruction.CreateInvalid3dnow(new OpCode(opCode), isModrmMemory);
				break;

			case 1:
				var instr = instructions[0];
				Assert.True(GetRealGroupIndex(instr) < 0);
				yield return instr;
				break;

			default:
				Assert.Fail($"Dupe instruction {instructions[0].Code} vs {instructions[1].Code}");
				throw ThrowHelpers.Unreachable;
			}
		}

		static readonly MandatoryPrefix[] allMandatoryPrefixes = (MandatoryPrefix[])Enum.GetValues(typeof(MandatoryPrefix));
		static readonly MandatoryPrefix[] mandatoryPrefixes = new MandatoryPrefix[4] {
			MandatoryPrefix.PNP, MandatoryPrefix.P66, MandatoryPrefix.PF3, MandatoryPrefix.PF2
		};
		// Returns all combinations of mandatory prefix, W, L. If it's a group, also modrm.reg=0-7, modrm.rm=0-7 and/or modrm=C0-FFh.
		static IEnumerable<FuzzerInstruction> GetVecInstructions(FuzzerOpCodeTable table, byte opCode, List<FuzzerInstruction> instructions, bool isModrmMemory) {
			Assert.True(table.Encoding == EncodingKind.VEX || table.Encoding == EncodingKind.XOP ||
				table.Encoding == EncodingKind.EVEX || table.Encoding == EncodingKind.MVEX);
			const int L_er_or_sae = 2;

			Func<uint, uint, int> getKeyIndex;
			int l_max;
			switch (table.Encoding) {
			case EncodingKind.VEX:
			case EncodingKind.XOP:
				l_max = 1;
				getKeyIndex = (uint w, uint l) => {
					Assert.True(w <= 1 && l <= 1);
					return (int)l + (int)w * 2;
				};
				break;
			case EncodingKind.EVEX:
				l_max = 3;
				getKeyIndex = (uint w, uint l) => {
					Assert.True(w <= 1 && l <= 3);
					return (int)l + (int)w * 4;
				};
				break;
			case EncodingKind.MVEX:
				throw new NotImplementedException();
			default:
				throw ThrowHelpers.Unreachable;
			}
			// 2 W values * 2or4 L values
			int totalInstrs = 2 * (l_max + 1);

			var prefixInstructions = new (MandatoryPrefix prefix, List<FuzzerInstruction> instructions)[4];
			for (int i = 0; i < mandatoryPrefixes.Length; i++)
				prefixInstructions[i] = (mandatoryPrefixes[i], new List<FuzzerInstruction>());
			foreach (var instruction in instructions) {
				int prefixIndex = instruction.MandatoryPrefix switch {
					MandatoryPrefix.PNP => 0,
					MandatoryPrefix.P66 => 1,
					MandatoryPrefix.PF3 => 2,
					MandatoryPrefix.PF2 => 3,
					_ => throw ThrowHelpers.Unreachable,
				};
				prefixInstructions[prefixIndex].instructions.Add(instruction);
			}

			foreach (var (prefix, prefixInstrs) in prefixInstructions) {
				// Need to special case the group opcodes
				if (prefixInstrs.Any(a => GetRealGroupIndex(a) >= 0 || a.RmGroupIndex >= 0)) {
					Assert.True(prefixInstrs.All(a => GetRealGroupIndex(a) >= 0 || a.RmGroupIndex >= 0));

					if (isModrmMemory) {
						// Up to 8 instructions with mem ops

						var groupInstrs = new FuzzerInstruction?[8][];
						for (int groupIndex = 0; groupIndex < groupInstrs.Length; groupIndex++)
							groupInstrs[groupIndex] = new FuzzerInstruction?[totalInstrs];

						// Add all valid instructions, verifying that there are no two instructions with the same key (prefix, W, L) in the same group (reg bits)
						foreach (var instr in prefixInstrs) {
							Assert.True(instr.RmGroupIndex < 0, "Mem with rm-group index???");
							var group = groupInstrs[GetRealGroupIndex(instr)];
							int keyIndex = getKeyIndex(instr.W, instr.L);
							if (group[keyIndex] is not null)
								Assert.Fail($"Dupe instruction {group[keyIndex]!.Code} vs {instr.Code}");
							group[keyIndex] = instr;
						}

						// For all remaining slots in all groups, create an invalid instruction
						for (int groupIndex = 0; groupIndex < groupInstrs.Length; groupIndex++) {
							var group = groupInstrs[groupIndex];
							for (uint w = 0; w <= 1; w++) {
								for (uint l = 0; l <= l_max; l++) {
									int keyIndex = getKeyIndex(w, l);
									if (group[keyIndex] is null)
										group[keyIndex] = FuzzerInstruction.CreateInvalidVec(table, new OpCode(opCode), groupIndex, -1, isModrmMemory, prefix, w, l, FuzzerInstructionFlags.None);
								}
							}
						}

						foreach (var group in groupInstrs) {
							foreach (var instr in group) {
								Assert.True(instr is not null);
								yield return instr;
							}
						}
					}
					else {
						// 8 instructions with a reg operand or 64 instructions with no ops (one instruction per modrm byte C0-FFh)
						// or a mix of that.

						var groupInstrs = new FuzzerInstruction?[64][];
						for (int groupIndex = 0; groupIndex < groupInstrs.Length; groupIndex++)
							groupInstrs[groupIndex] = new FuzzerInstruction?[totalInstrs];

						// Add all valid instructions, verifying that there are no two instructions with the same key (prefix, W, L) with the same modrm byte.
						// It doesn't verify that there are no two instructions where one uses reg ops with reg=1 (modrm C8-CF) and another instruction
						// with no ops and modrm C9h. The code after this loop checks that.
						foreach (var instr in prefixInstrs) {
							int groupIndex = GetRegGroupIndex(instr);
							var group = groupInstrs[groupIndex];
							int keyIndex = getKeyIndex(instr.W, instr.L);
							if (group[keyIndex] is not null)
								Assert.Fail($"Dupe instruction {group[keyIndex]!.Code} vs {instr.Code}");
							group[keyIndex] = instr;
						}

						if (table.Encoding == EncodingKind.EVEX) {
							for (int groupIndex = 0; groupIndex < 0x40; groupIndex++) {
								for (uint w = 0; w <= 1; w++) {
									var group = groupInstrs[groupIndex];
									if (UsesSaeOrRc(group[getKeyIndex(w, L_er_or_sae)])) {
										// This instruction uses {er} or {sae} so make sure the other instructions don't try to test EVEX.b
										for (uint l = 0; l <= l_max; l++) {
											if (l == L_er_or_sae)
												continue;
											if (group[getKeyIndex(w, l)] is FuzzerInstruction instr)
												instr.Flags |= FuzzerInstructionFlags.DontUseEvexBcstBit;
										}
									}
								}
							}
						}

						// For all remaining slots in all groups, create an invalid instruction
						for (uint w = 0; w <= 1; w++) {
							int keyIndexL2 = table.Encoding == EncodingKind.EVEX ? getKeyIndex(w, L_er_or_sae) : -1;
							for (uint l = 0; l <= l_max; l++) {
								int keyIndex = getKeyIndex(w, l);
								if (HasRmGroupIndex(prefixInstrs, w, l)) {
									for (int rmGroupIndex = 0; rmGroupIndex < 8; rmGroupIndex++) {
										if (IsRmGroupIndexInstruction(groupInstrs, rmGroupIndex, keyIndex, out var instr)) {
											var group = groupInstrs[rmGroupIndex];
											// Verify that reg=0..7 is only used by this instruction.
											for (int i = 1; i < 0x40; i += 8) {
												group = groupInstrs[rmGroupIndex + i];
												if (group[keyIndex] is not null)
													Assert.Fail($"Dupe instruction {group[keyIndex]!.Code} vs {instr.Code}");
											}
										}
										else {
											int numNulls = 0;
											for (int i = 0; i < 0x40; i += 8) {
												var group = groupInstrs[rmGroupIndex + i];
												if (group[keyIndex] is null)
													numNulls++;
											}
											if (numNulls == 8) {
												var group = groupInstrs[rmGroupIndex];
												var flags = table.Encoding == EncodingKind.EVEX && UsesSaeOrRc(group[keyIndexL2]) ? FuzzerInstructionFlags.DontUseEvexBcstBit : FuzzerInstructionFlags.None;
												group[keyIndex] = FuzzerInstruction.CreateInvalidVec(table, new OpCode(opCode), -1, rmGroupIndex, isModrmMemory, prefix, w, l, flags);
											}
											else {
												for (int i = 0; i < 0x40; i += 8) {
													var group = groupInstrs[rmGroupIndex + i];
													if (group[keyIndex] is null) {
														var flags = table.Encoding == EncodingKind.EVEX && UsesSaeOrRc(group[keyIndexL2]) ? FuzzerInstructionFlags.DontUseEvexBcstBit : FuzzerInstructionFlags.None;
														var newOpCode = new OpCode(opCode, (byte)(0xC0 | rmGroupIndex + i));
														group[keyIndex] = FuzzerInstruction.CreateInvalidVec(table, newOpCode, -1, -1, isModrmMemory, prefix, w, l, flags);
													}
												}
											}
										}
									}
								}
								else {
									for (int groupIndex = 0; groupIndex < 0x40; groupIndex += 8) {
										if (IsGroupRmInstruction(groupInstrs, groupIndex, keyIndex, out var instr)) {
											var group = groupInstrs[groupIndex];
											// Verify that rm=0..7 is only used by this instruction.
											for (int i = 1; i < 8; i++) {
												group = groupInstrs[groupIndex + i];
												if (group[keyIndex] is not null)
													Assert.Fail($"Dupe instruction {group[keyIndex]!.Code} vs {instr.Code}");
											}
										}
										else {
											int numNulls = 0;
											if ((groupIndex & 7) == 0) {
												for (int i = 0; i < 8; i++) {
													var group = groupInstrs[groupIndex + i];
													if (group[keyIndex] is null)
														numNulls++;
												}
											}
											if (numNulls == 8) {
												var group = groupInstrs[groupIndex];
												var flags = table.Encoding == EncodingKind.EVEX && UsesSaeOrRc(group[keyIndexL2]) ? FuzzerInstructionFlags.DontUseEvexBcstBit : FuzzerInstructionFlags.None;
												group[keyIndex] = FuzzerInstruction.CreateInvalidVec(table, new OpCode(opCode), groupIndex >> 3, -1, isModrmMemory, prefix, w, l, flags);
											}
											else {
												for (int i = 0; i < 8; i++) {
													var group = groupInstrs[groupIndex + i];
													if (group[keyIndex] is null) {
														var flags = table.Encoding == EncodingKind.EVEX && UsesSaeOrRc(group[keyIndexL2]) ? FuzzerInstructionFlags.DontUseEvexBcstBit : FuzzerInstructionFlags.None;
														var newOpCode = new OpCode(opCode, (byte)(0xC0 | groupIndex + i));
														group[keyIndex] = FuzzerInstruction.CreateInvalidVec(table, newOpCode, -1, -1, isModrmMemory, prefix, w, l, flags);
													}
												}
											}
										}
									}
								}
							}
						}

						foreach (var group in groupInstrs) {
							foreach (var instr in group) {
								if (instr is not null)
									yield return instr;
							}
						}
					}
				}
				else {
					// Non-group instructions

					var instrs = new FuzzerInstruction?[totalInstrs];

					// Add all valid instructions, verifying that there are no two instructions with the same key (prefix, W, L)
					foreach (var instr in prefixInstrs) {
						int keyIndex = getKeyIndex(instr.W, instr.L);
						if (instrs[keyIndex] is not null)
							Assert.Fail($"Dupe instruction {instrs[keyIndex]!.Code} vs {instr.Code}");
						instrs[keyIndex] = instr;
					}

					if (table.Encoding == EncodingKind.EVEX) {
						for (uint w = 0; w <= 1; w++) {
							if (UsesSaeOrRc(instrs[getKeyIndex(w, L_er_or_sae)])) {
								// This instruction uses {er} or {sae} so make sure the other instructions don't try to test EVEX.b
								for (uint l = 0; l <= l_max; l++) {
									if (l == L_er_or_sae)
										continue;
									if (instrs[getKeyIndex(w, l)] is FuzzerInstruction instr)
										instr.Flags |= FuzzerInstructionFlags.DontUseEvexBcstBit;
								}
							}
						}
					}

					// For all remaining slots, create an invalid instruction
					for (uint w = 0; w <= 1; w++) {
						var flags = table.Encoding == EncodingKind.EVEX && UsesSaeOrRc(instrs[getKeyIndex(w, L_er_or_sae)]) ? FuzzerInstructionFlags.DontUseEvexBcstBit : FuzzerInstructionFlags.None;
						for (uint l = 0; l <= l_max; l++) {
							int keyIndex = getKeyIndex(w, l);
							if (instrs[keyIndex] is null)
								instrs[keyIndex] = FuzzerInstruction.CreateInvalidVec(table, new OpCode(opCode), -1, -1, isModrmMemory, prefix, w, l, flags);
						}
					}

					foreach (var instr in instrs) {
						Assert.True(instr is not null);
						yield return instr;
					}
				}
			}
		}

		static bool HasRmGroupIndex(List<FuzzerInstruction> instrs, uint w, uint l) {
			bool hasGroupIndex = false;
			bool hasRmGroupIndex = false;
			foreach (var instr in instrs) {
				if (instr.W == w && instr.L == l) {
					hasGroupIndex |= instr.GroupIndex >= 0;
					hasRmGroupIndex |= instr.RmGroupIndex >= 0;
				}
			}
			// Not supported. If one instruction has a RmGroupIndex, we assume it uses the reg
			// bits for something, because if reg was fixed, it would be a 2-byte opcode.
			// We assume all reg bits are used. In that case, there can be no normal group instructions
			// since they use the reg bits as the group index.
			Assert.False(hasGroupIndex && hasRmGroupIndex);
			return hasRmGroupIndex;
		}

		static bool UsesSaeOrRc(FuzzerInstruction? instr) =>
			instr is FuzzerInstruction instrL2 &&
			!instr.IsModrmMemory &&
			(instrL2.Flags & (FuzzerInstructionFlags.CanSuppressAllExceptions | FuzzerInstructionFlags.CanUseRoundingControl)) != 0;

		// Checks if the instruction uses the `rm` bits of the modrm byte to encode an operand, and if so returns it.
		static bool IsGroupRmInstruction(FuzzerInstruction?[][] groupInstrs, int groupIndex, int keyIndex, [NotNullWhen(true)] out FuzzerInstruction? instr) {
			// Must have rm=0
			Assert.True((groupIndex & 7) == 0);
			instr = null;
			var group = groupInstrs[groupIndex];
			if (group[keyIndex] is not FuzzerInstruction finstr)
				return false;
			if (finstr.RmGroupIndex >= 0)
				return false;
			Assert.True(GetRealGroupIndex(finstr) >= 0);
			// 2-byte opcode if it has no ops, eg. `swapgs`
			if (finstr.OpCode.IsTwobyte)
				return false;

			instr = finstr;
			return true;
		}

		static bool IsRmGroupIndexInstruction(FuzzerInstruction?[][] groupInstrs, int rmGroupIndex, int keyIndex, [NotNullWhen(true)] out FuzzerInstruction? instr) {
			instr = null;
			var group = groupInstrs[rmGroupIndex];
			if (group[keyIndex] is not FuzzerInstruction finstr)
				return false;
			if (finstr.RmGroupIndex < 0)
				return false;
			Assert.True(UsesModrmRegFieldInOperand(finstr), "If rm is hard coded and modrm.reg too, it should be a 2-byte opcode");

			instr = finstr;
			return true;
		}

		static bool UsesModrmRegFieldInOperand(FuzzerInstruction instr) {
			foreach (var regOpoperand in instr.RegisterOperands) {
				if (regOpoperand.RegLocation == FuzzerOperandRegLocation.ModrmRegBits)
					return true;
			}
			return false;
		}

		static bool IgnoresModRmRegBits(Code code) =>
			code switch {
				Code.Seto_rm8 or Code.Setno_rm8 or Code.Setb_rm8 or Code.Setae_rm8 or Code.Sete_rm8 or Code.Setne_rm8 or Code.Setbe_rm8 or
				Code.Seta_rm8 or Code.Sets_rm8 or Code.Setns_rm8 or Code.Setp_rm8 or Code.Setnp_rm8 or Code.Setl_rm8 or Code.Setge_rm8 or
				Code.Setle_rm8 or Code.Setg_rm8 => true,
				_ => false,
			};

		static bool IgnoresModRmLow3Bits(Code code) =>
			code switch {
				Code.Montmul_16 or Code.Montmul_32 or Code.Montmul_64 or
				Code.Xsha1_16 or Code.Xsha1_32 or Code.Xsha1_64 or
				Code.Xsha256_16 or Code.Xsha256_32 or Code.Xsha256_64 or
				Code.Xsha512_16 or Code.Xsha512_32 or Code.Xsha512_64 or
				Code.Xsha512_alt_16 or Code.Xsha512_alt_32 or Code.Xsha512_alt_64 or
				Code.Xstore_16 or Code.Xstore_32 or Code.Xstore_64 or
				Code.Xcryptecb_16 or Code.Xcryptecb_32 or Code.Xcryptecb_64 or
				Code.Xcryptcbc_16 or Code.Xcryptcbc_32 or Code.Xcryptcbc_64 or
				Code.Xcryptctr_16 or Code.Xcryptctr_32 or Code.Xcryptctr_64 or
				Code.Xcryptcfb_16 or Code.Xcryptcfb_32 or Code.Xcryptcfb_64 or
				Code.Xcryptofb_16 or Code.Xcryptofb_32 or Code.Xcryptofb_64 or
				Code.Xstore_alt_16 or Code.Xstore_alt_32 or Code.Xstore_alt_64 or
				Code.Ccs_hash_16 or Code.Ccs_hash_32 or Code.Ccs_hash_64 or
				Code.Via_undoc_F30FA6F0_16 or Code.Via_undoc_F30FA6F0_32 or Code.Via_undoc_F30FA6F0_64 or
				Code.Via_undoc_F30FA6F8_16 or Code.Via_undoc_F30FA6F8_32 or Code.Via_undoc_F30FA6F8_64 or
				Code.Ccs_encrypt_16 or Code.Ccs_encrypt_32 or Code.Ccs_encrypt_64 => true,
				_ => false,
			};

		static IEnumerable<(bool hasModrm, FuzzerInstruction)> GetInstructions(int bitness, OpCodeInfo[] opCodes) {
			// Split up instructions with a reg/mem (modrm) operand into two instructions,
			// one with reg only ops and the other one with reg+mem ops, eg. `add r16,rm16`
			// becomes `add r16,m16` and `add r16,r16`.
			foreach (var opCode in opCodes) {
				if (IgnoresModRmRegBits(opCode.Code)) {
					Assert.True(opCode.GroupIndex == -1);
					for (int i = 0; i < 8; i++) {
						foreach (var info in GetInstructions(bitness, opCode, opCode.MandatoryPrefix, i))
							yield return info;
					}
				}
				else if (IgnoresModRmLow3Bits(opCode.Code)) {
					for (int i = 0; i < 8; i++) {
						Assert.True((opCode.OpCode & 7) == 0);
						var realOpCode = OpCode.CreateFromUInt32(opCode.OpCode + (uint)i, opCode.OpCodeLength);
						foreach (var info in GetInstructions(bitness, opCode, opCode.MandatoryPrefix, opCode.GroupIndex, realOpCode))
							yield return info;
					}
				}
				else {
					foreach (var info in GetInstructions(bitness, opCode, opCode.MandatoryPrefix, opCode.GroupIndex))
						yield return info;
				}
			}
		}

		static IEnumerable<(bool hasModrm, FuzzerInstruction)> GetInstructions(int bitness, OpCodeInfo opCode, MandatoryPrefix mandatoryPrefix, int groupIndex, OpCode? realOpCode = null) {
			var (hasModrm, kind) = HasModRmWithRegAndMemOps(opCode);
			foreach (var (w, l) in GetLW(bitness, opCode)) {
				if (kind == ModrmMemoryKind.Mem || kind == ModrmMemoryKind.RegOrMem)
					yield return (hasModrm, FuzzerInstruction.CreateValid(opCode.Code, isModrmMemory: true, w, l, mandatoryPrefix, groupIndex, realOpCode));
				if (kind == ModrmMemoryKind.Other || kind == ModrmMemoryKind.RegOrMem)
					yield return (hasModrm, FuzzerInstruction.CreateValid(opCode.Code, isModrmMemory: false, w, l, mandatoryPrefix, groupIndex, realOpCode));
			}
		}

		static IEnumerable<(uint w, uint l)> GetLW(int bitness, OpCodeInfo opCode) {
			uint w_lo, w_hi;
			uint l_lo, l_hi;
			switch (opCode.Encoding) {
			case EncodingKind.Legacy:
			case EncodingKind.D3NOW:
				(w_lo, w_hi) = (0, 1);
				(l_lo, l_hi) = (0, 1);
				break;
			case EncodingKind.VEX:
			case EncodingKind.XOP:
				if (opCode.IsLIG)
					(l_lo, l_hi) = (0, 2);
				else
					(l_lo, l_hi) = (opCode.L, opCode.L + 1);
				if (opCode.IsWIG || (bitness != 64 && opCode.IsWIG32))
					(w_lo, w_hi) = (0, 2);
				else
					(w_lo, w_hi) = (opCode.W, opCode.W + 1);
				break;
			case EncodingKind.EVEX:
				if (opCode.IsLIG)
					(l_lo, l_hi) = (0, 3);// They're not really LIG (eg. L=3)
				else
					(l_lo, l_hi) = (opCode.L, opCode.L + 1);
				if (opCode.IsWIG || (bitness != 64 && opCode.IsWIG32))
					(w_lo, w_hi) = (0, 2);
				else
					(w_lo, w_hi) = (opCode.W, opCode.W + 1);
				break;
			case EncodingKind.MVEX:
				throw new NotImplementedException();
			default:
				throw ThrowHelpers.Unreachable;
			}
			for (uint w = w_lo; w < w_hi; w++) {
				for (uint l = l_lo; l < l_hi; l++)
					yield return (w, l);
			}
		}

		enum ModrmMemoryKind {
			Other,
			RegOrMem,
			Mem,
		}

		static (bool hasModrm, ModrmMemoryKind kind) HasModRmWithRegAndMemOps(OpCodeInfo opCode) {
			bool hasModrm = false;
			for (int i = 0; i < opCode.OpCount; i++) {
				switch (opCode.GetOpKind(i)) {
				case OpCodeOperandKind.r8_or_mem:
				case OpCodeOperandKind.r16_or_mem:
				case OpCodeOperandKind.r32_or_mem:
				case OpCodeOperandKind.r32_or_mem_mpx:
				case OpCodeOperandKind.r64_or_mem:
				case OpCodeOperandKind.r64_or_mem_mpx:
				case OpCodeOperandKind.mm_or_mem:
				case OpCodeOperandKind.xmm_or_mem:
				case OpCodeOperandKind.ymm_or_mem:
				case OpCodeOperandKind.zmm_or_mem:
				case OpCodeOperandKind.bnd_or_mem_mpx:
				case OpCodeOperandKind.k_or_mem:
					return (true, ModrmMemoryKind.RegOrMem);

				case OpCodeOperandKind.mem:
				case OpCodeOperandKind.sibmem:
				case OpCodeOperandKind.mem_mpx:
				case OpCodeOperandKind.mem_mib:
				case OpCodeOperandKind.mem_vsib32x:
				case OpCodeOperandKind.mem_vsib64x:
				case OpCodeOperandKind.mem_vsib32y:
				case OpCodeOperandKind.mem_vsib64y:
				case OpCodeOperandKind.mem_vsib32z:
				case OpCodeOperandKind.mem_vsib64z:
					return (true, ModrmMemoryKind.Mem);

				case OpCodeOperandKind.r8_reg:
				case OpCodeOperandKind.r16_reg:
				case OpCodeOperandKind.r16_reg_mem:
				case OpCodeOperandKind.r16_rm:
				case OpCodeOperandKind.r32_reg:
				case OpCodeOperandKind.r32_reg_mem:
				case OpCodeOperandKind.r32_rm:
				case OpCodeOperandKind.r64_reg:
				case OpCodeOperandKind.r64_reg_mem:
				case OpCodeOperandKind.r64_rm:
				case OpCodeOperandKind.seg_reg:
				case OpCodeOperandKind.k_reg:
				case OpCodeOperandKind.kp1_reg:
				case OpCodeOperandKind.k_rm:
				case OpCodeOperandKind.mm_reg:
				case OpCodeOperandKind.mm_rm:
				case OpCodeOperandKind.xmm_reg:
				case OpCodeOperandKind.xmm_rm:
				case OpCodeOperandKind.ymm_reg:
				case OpCodeOperandKind.ymm_rm:
				case OpCodeOperandKind.zmm_reg:
				case OpCodeOperandKind.zmm_rm:
				case OpCodeOperandKind.bnd_reg:
				case OpCodeOperandKind.tmm_reg:
				case OpCodeOperandKind.tmm_rm:
					hasModrm = true;
					break;

				case OpCodeOperandKind.cr_reg:
				case OpCodeOperandKind.dr_reg:
				case OpCodeOperandKind.tr_reg:
					// They do use a modrm byte but the mod bits are ignored
					return (false, ModrmMemoryKind.Other);

				case OpCodeOperandKind.None:
				case OpCodeOperandKind.farbr2_2:
				case OpCodeOperandKind.farbr4_2:
				case OpCodeOperandKind.mem_offs:// no modrm memory op
				case OpCodeOperandKind.r8_opcode:
				case OpCodeOperandKind.r16_opcode:
				case OpCodeOperandKind.r32_opcode:
				case OpCodeOperandKind.r32_vvvv:
				case OpCodeOperandKind.r64_opcode:
				case OpCodeOperandKind.r64_vvvv:
				case OpCodeOperandKind.k_vvvv:
				case OpCodeOperandKind.xmm_vvvv:
				case OpCodeOperandKind.xmmp3_vvvv:
				case OpCodeOperandKind.xmm_is4:
				case OpCodeOperandKind.xmm_is5:
				case OpCodeOperandKind.ymm_vvvv:
				case OpCodeOperandKind.ymm_is4:
				case OpCodeOperandKind.ymm_is5:
				case OpCodeOperandKind.zmm_vvvv:
				case OpCodeOperandKind.zmmp3_vvvv:
				case OpCodeOperandKind.es:
				case OpCodeOperandKind.cs:
				case OpCodeOperandKind.ss:
				case OpCodeOperandKind.ds:
				case OpCodeOperandKind.fs:
				case OpCodeOperandKind.gs:
				case OpCodeOperandKind.al:
				case OpCodeOperandKind.cl:
				case OpCodeOperandKind.ax:
				case OpCodeOperandKind.dx:
				case OpCodeOperandKind.eax:
				case OpCodeOperandKind.rax:
				case OpCodeOperandKind.st0:
				case OpCodeOperandKind.sti_opcode:
				case OpCodeOperandKind.imm4_m2z:
				case OpCodeOperandKind.imm8:
				case OpCodeOperandKind.imm8_const_1:
				case OpCodeOperandKind.imm8sex16:
				case OpCodeOperandKind.imm8sex32:
				case OpCodeOperandKind.imm8sex64:
				case OpCodeOperandKind.imm16:
				case OpCodeOperandKind.imm32:
				case OpCodeOperandKind.imm32sex64:
				case OpCodeOperandKind.imm64:
				case OpCodeOperandKind.seg_rSI:
				case OpCodeOperandKind.es_rDI:
				case OpCodeOperandKind.seg_rDI:
				case OpCodeOperandKind.seg_rBX_al:
				case OpCodeOperandKind.br16_1:
				case OpCodeOperandKind.br32_1:
				case OpCodeOperandKind.br64_1:
				case OpCodeOperandKind.br16_2:
				case OpCodeOperandKind.br32_4:
				case OpCodeOperandKind.br64_4:
				case OpCodeOperandKind.xbegin_2:
				case OpCodeOperandKind.xbegin_4:
				case OpCodeOperandKind.brdisp_2:
				case OpCodeOperandKind.brdisp_4:
				case OpCodeOperandKind.tmm_vvvv:
					break;

				default:
					throw ThrowHelpers.Unreachable;
				}
			}
			return (hasModrm, ModrmMemoryKind.Other);
		}
	}
}
