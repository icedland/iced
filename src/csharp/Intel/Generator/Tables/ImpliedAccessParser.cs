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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Generator.Constants;
using Generator.Enums;
using Generator.Enums.InstructionInfo;

namespace Generator.Tables {
	sealed class ImpliedAccessParser {
		readonly Dictionary<string, EnumValue> toMemorySize;
		readonly Dictionary<uint, EnumValue> uintToRegister;
		readonly Dictionary<string, EnumValue> toRegisterImplAcc;

		public ImpliedAccessParser(GenTypes genTypes) {
			toMemorySize = CreateEnumDict(genTypes[TypeIds.MemorySize]);
			uintToRegister = genTypes[TypeIds.Register].Values.ToDictionary(a => a.Value, a => a);
			toRegisterImplAcc = CreateEnumDict(genTypes[TypeIds.Register], ignoreCase: true);

			uint vmmFirst = IcedConstantsType.Get_VMM_first(genTypes).Value;
			uint vmmLast = IcedConstantsType.Get_VMM_last(genTypes).Value;
			uint vmmCount = vmmLast - vmmFirst + 1;
			var tmmLast = IcedConstantsType.Get_TMM_last(genTypes);
			toRegisterImplAcc.Add("tmm_last", tmmLast);
			for (uint ri = 0; ri < vmmCount; ri++) {
				var reg = uintToRegister[vmmFirst + ri];
				toRegisterImplAcc.Add("vmm" + ri, reg);
			}
		}

		static Dictionary<string, EnumValue> CreateEnumDict(EnumType enumType, bool ignoreCase = false) =>
			enumType.Values.ToDictionary(a => a.RawName, a => a, ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);

		static bool TryParse_2_4_or_8(string value, out uint result, [NotNullWhen(false)] out string? error) {
			if (!ParserUtils.TryParseUInt32(value, out result, out error))
				return false;
			switch (result) {
			case 2:
			case 4:
			case 8:
				error = null;
				return true;
			default:
				error = $"Invalid integer, expected 2, 4 or 8: `{value}`";
				return false;
			}
		}

		static bool TryParse_2_or_4(string value, out uint result, [NotNullWhen(false)] out string? error) {
			if (!ParserUtils.TryParseUInt32(value, out result, out error))
				return false;
			switch (result) {
			case 2:
			case 4:
				error = null;
				return true;
			default:
				error = $"Invalid integer, expected 2, 4 or 8: `{value}`";
				return false;
			}
		}

		static bool TryParsePushPopArgs(string value, out uint count, out uint size, [NotNullWhen(false)] out string? error) {
			count = 0;
			size = 0;

			var parts = value.Split('x');
			if (parts.Length != 2) {
				error = $"Expected <count> x <size>, eg. 1x8: `{value}`";
				return false;
			}

			if (!ParserUtils.TryParseUInt32(parts[0], out count, out error))
				return false;
			if (!TryParse_2_4_or_8(parts[1], out size, out error))
				return false;

			error = null;
			return true;
		}

		static readonly HashSet<string> impliedAccessKeyHasValue = new HashSet<string>(StringComparer.Ordinal) {
			"cr", "cw", "crcw", "r", "w", "rw", "rcw",
			"shift-mask-1F-mod",
			"enter", "leave",
			"push", "pop", "pop-rm",
			"pusha", "popa",
			"emmi-reg",
		};
		public bool ReadImpliedAccesses(string line, [NotNullWhen(true)] out ImpliedAccesses? accesses, [NotNullWhen(false)] out string? error) {
			accesses = null;

			if (!TryReadImpliedAccesses(line, out var conds, out error))
				return false;

			if (HasSpecialAccWithOtherAcc(conds)) {
				// They change rflags bits and the runtime code needs to check if it's one of these, see GetRflagsInfo()
				error = "These can only be used with no other commands: shift-mask-1F, shift-mask-1F-mod, shift-mask-3F, zero-reg-rflags";
				return false;
			}

			var mergedConds = MergeConds(conds);
			AddReadMemRegs(mergedConds);
			RemoveDupeStatements(mergedConds);
			RemoveUselessRegs(mergedConds);
			mergedConds.Sort((a, b) => a.Kind.CompareTo(b.Kind));
			foreach (var cond in mergedConds) {
				Sort(cond.TrueStatements);
				Sort(cond.FalseStatements);
			}

			accesses = new ImpliedAccesses(mergedConds);
			return true;
		}

		void Sort(List<ImplAccStatement> stmts) => stmts.Sort(CompareStmts);

		int CompareStmts(ImplAccStatement x, ImplAccStatement y) {
			if (x.Kind != y.Kind)
				return x.Kind.CompareTo(y.Kind);

			switch (x) {
			case NoArgImplAccStatement _:
				return 0;

			case EmmiImplAccStatement emmi_a:
				var emmi_b = (EmmiImplAccStatement)y;
				return emmi_a.CompareTo(emmi_b);

			case IntArgImplAccStatement ia_a:
				var ia_b = (IntArgImplAccStatement)y;
				return ia_a.CompareTo(ia_b);

			case IntX2ArgImplAccStatement ia2_a:
				var ia2_b = (IntX2ArgImplAccStatement)y;
				return ia2_a.CompareTo(ia2_b);

			case RegisterRangeImplAccStatement range_a:
				var range_b = (RegisterRangeImplAccStatement)y;
				return range_a.CompareTo(range_b);

			case RegisterImplAccStatement reg_a:
				var reg_b = (RegisterImplAccStatement)y;
				return reg_a.CompareTo(reg_b);

			case MemoryImplAccStatement mem_a:
				var mem_b = (MemoryImplAccStatement)y;
				return mem_a.CompareTo(mem_b);

			default:
				throw new InvalidOperationException();
			}
		}

		void RemoveUselessRegs(List<ImplAccCondition> conds) {
			foreach (var cond in conds) {
				switch (cond.Kind) {
				case ImplAccConditionKind.Bit64:
					RemoveUselessSegmentReads(cond.TrueStatements);
					break;
				case ImplAccConditionKind.NotBit64:
					RemoveUselessSegmentReads(cond.FalseStatements);
					break;
				}
			}
		}

		void RemoveUselessSegmentReads(List<ImplAccStatement> stmts) {
			for (int i = stmts.Count - 1; i >= 0; i--) {
				var stmt = stmts[i];
				if (stmt is RegisterImplAccStatement reg) {
					if (!IsSeg_ES_CS_SS_DS(reg.Register))
						continue;

					if (reg.Access == OpAccess.Read || reg.Access == OpAccess.CondRead)
						stmts.RemoveAt(i);
				}
			}
		}

		static bool IsSeg_ES_CS_SS_DS(ImplAccRegister register) {
			if (register.Kind == ImplAccRegisterKind.Register) {
				Debug.Assert(register.Register is object);
				switch ((Register)register.Register.Value) {
				case Register.ES:
				case Register.CS:
				case Register.SS:
				case Register.DS:
					return true;
				}
			}
			return false;
		}

		void RemoveDupeStatements(List<ImplAccCondition> conds) {
			foreach (var cond in conds) {
				RemoveDupeStatements(cond.TrueStatements);
				RemoveDupeStatements(cond.FalseStatements);
			}
		}

		void RemoveDupeStatements(List<ImplAccStatement> stmts) {
			var unique = stmts.Distinct().ToArray();
			stmts.Clear();
			stmts.AddRange(unique);
		}

		void AddReadMemRegs(List<ImplAccCondition> conds) {
			var extra = new List<ImplAccStatement>();
			foreach (var cond in conds) {
				AddReadRegs(extra, cond.TrueStatements);
				AddReadRegs(extra, cond.FalseStatements);
			}
		}

		void AddReadRegs(List<ImplAccStatement> extra, List<ImplAccStatement> stmts) {
			extra.Clear();
			foreach (var stmt in stmts) {
				if (stmt is MemoryImplAccStatement mem) {
					OpAccess opAccess;
					switch (mem.Access) {
					case OpAccess.Read:
					case OpAccess.Write:
					case OpAccess.ReadWrite:
					case OpAccess.ReadCondWrite:
					case OpAccess.NoMemAccess:
						opAccess = OpAccess.Read;
						break;
					case OpAccess.CondRead:
					case OpAccess.CondWrite:
						opAccess = OpAccess.CondRead;
						break;
					case OpAccess.None:
					default:
						throw new InvalidOperationException();
					}
					AddReadReg(extra, mem.Segment, opAccess, true);
					AddReadReg(extra, mem.Base, opAccess, false);
					AddReadReg(extra, mem.Index, opAccess, false);
				}
			}
			stmts.AddRange(extra);
		}

		void AddReadReg(List<ImplAccStatement> extra, ImplAccRegister? reg, OpAccess opAccess, bool isMemOpSegRead) {
			if (reg is ImplAccRegister reg2) {
				switch (reg2.Kind) {
				case ImplAccRegisterKind.Register:
				case ImplAccRegisterKind.SegmentDefaultDS:
				case ImplAccRegisterKind.a_rDI:
					extra.Add(new RegisterImplAccStatement(opAccess, reg2, isMemOpSegRead));
					break;

				case ImplAccRegisterKind.Op0:
				case ImplAccRegisterKind.Op1:
				case ImplAccRegisterKind.Op2:
				case ImplAccRegisterKind.Op3:
				case ImplAccRegisterKind.Op4:
					// Added automatically by the runtime code
					break;

				default:
					throw new InvalidOperationException();
				}
			}
		}

		static readonly Dictionary<ImplAccConditionKind, ImplAccConditionKind> toPosCond = new Dictionary<ImplAccConditionKind, ImplAccConditionKind> {
			{ ImplAccConditionKind.None, ImplAccConditionKind.None },
			{ ImplAccConditionKind.Bit64, ImplAccConditionKind.Bit64 },
			{ ImplAccConditionKind.NotBit64, ImplAccConditionKind.Bit64 },
		};
		static readonly Dictionary<ImplAccConditionKind, ImplAccConditionKind> toNegCond = new Dictionary<ImplAccConditionKind, ImplAccConditionKind> {
			{ ImplAccConditionKind.None, ImplAccConditionKind.None },
			{ ImplAccConditionKind.Bit64, ImplAccConditionKind.NotBit64 },
			{ ImplAccConditionKind.NotBit64, ImplAccConditionKind.NotBit64 },
		};
		List<ImplAccCondition> MergeConds(Dictionary<ImplAccConditionKind, ImplAccCondition> conds) {
			if (toPosCond.Count != toNegCond.Count)
				throw new InvalidOperationException();

			var mergedConds = new Dictionary<ImplAccConditionKind, ImplAccCondition>();
			foreach (var kv in conds) {
				var currCond = kv.Value;

				var newKind = toPosCond[currCond.Kind];
				if (!mergedConds.TryGetValue(newKind, out var cond))
					mergedConds.Add(newKind, cond = new ImplAccCondition(newKind));

				if (currCond.Kind == newKind) {
					cond.TrueStatements.AddRange(currCond.TrueStatements);
					cond.FalseStatements.AddRange(currCond.FalseStatements);
				}
				else {
					cond.TrueStatements.AddRange(currCond.FalseStatements);
					cond.FalseStatements.AddRange(currCond.TrueStatements);
				}
			}

			var result = new List<ImplAccCondition>();
			foreach (var kv in mergedConds) {
				var currCond = kv.Value;
				if (currCond.TrueStatements.Count == 0 && currCond.FalseStatements.Count == 0)
					continue;

				if (currCond.TrueStatements.Count == 0) {
					var newKind = toNegCond[currCond.Kind];
					var newCond = new ImplAccCondition(newKind);
					newCond.TrueStatements.AddRange(currCond.FalseStatements);
					result.Add(newCond);
				}
				else
					result.Add(currCond);
			}

			return result;
		}

		static bool HasSpecialAccWithOtherAcc(Dictionary<ImplAccConditionKind, ImplAccCondition> conds) {
			int count = 0;
			bool found = false;
			foreach (var kv in conds) {
				var acc = kv.Value;
				Check(ref count, ref found, acc.TrueStatements);
				Check(ref count, ref found, acc.FalseStatements);
			}

			return found && count > 1;

			static void Check(ref int count, ref bool found, List<ImplAccStatement> stmts) {
				foreach (var stmt in stmts) {
					count++;
					switch (stmt.Kind) {
					case ImplAccStatementKind.ShiftMask1F:
					case ImplAccStatementKind.ShiftMask1FMod:
					case ImplAccStatementKind.ShiftMask3F:
					case ImplAccStatementKind.ZeroRegRflags:
						found = true;
						break;
					}
				}
			}
		}

		bool TryReadImpliedAccesses(string line, out Dictionary<ImplAccConditionKind, ImplAccCondition> conds, [NotNullWhen(false)] out string? error) {
			conds = new Dictionary<ImplAccConditionKind, ImplAccCondition>();
			foreach (var keyValue in line.Split(' ', StringSplitOptions.RemoveEmptyEntries)) {
				var (keyCond, value) = ParserUtils.GetKeyValue(keyValue);
				var (key, condStr) = GetKeyCond(keyCond);

				ImplAccConditionKind cond;
				switch (condStr) {
				case "": cond = ImplAccConditionKind.None; break;
				case "64": cond = ImplAccConditionKind.Bit64; break;
				case "!64": cond = ImplAccConditionKind.NotBit64; break;
				default:
					error = $"Unknown condition `{condStr}`";
					return false;
				}

				if (impliedAccessKeyHasValue.Contains(key)) {
					if (value == string.Empty) {
						error = $"Missing key=value: `{keyValue}`";
						return false;
					}
				}
				else {
					if (value != string.Empty) {
						error = $"`{key}` doesn't support values: `{keyValue}`";
						return false;
					}
				}

				if (!conds.TryGetValue(cond, out var implAccCond))
					conds.Add(cond, implAccCond = new ImplAccCondition(cond));
				ImplAccStatement? stmt = null;
				uint arg1, arg2;
				switch (key) {
				case "cr":
					if (!TryParseImplAccRegOrMem(implAccCond.TrueStatements, value, OpAccess.CondRead, out error))
						return false;
					break;

				case "cw":
					if (!TryParseImplAccRegOrMem(implAccCond.TrueStatements, value, OpAccess.CondWrite, out error))
						return false;
					break;

				case "crcw":
					if (!TryParseImplAccRegOrMem(implAccCond.TrueStatements, value, OpAccess.CondRead, out error))
						return false;
					if (!TryParseImplAccRegOrMem(implAccCond.TrueStatements, value, OpAccess.CondWrite, out error))
						return false;
					break;

				case "r":
					if (!TryParseImplAccRegOrMem(implAccCond.TrueStatements, value, OpAccess.Read, out error))
						return false;
					break;

				case "w":
					if (!TryParseImplAccRegOrMem(implAccCond.TrueStatements, value, OpAccess.Write, out error))
						return false;
					break;

				case "rw":
					if (!TryParseImplAccRegOrMem(implAccCond.TrueStatements, value, OpAccess.ReadWrite, out error))
						return false;
					break;

				case "rcw":
					if (!TryParseImplAccRegOrMem(implAccCond.TrueStatements, value, OpAccess.ReadCondWrite, out error))
						return false;
					break;

				case "shift-mask-1F":
					stmt = new NoArgImplAccStatement(ImplAccStatementKind.ShiftMask1F);
					break;

				case "shift-mask-3F":
					stmt = new NoArgImplAccStatement(ImplAccStatementKind.ShiftMask3F);
					break;

				case "zero-reg-rflags":
					stmt = new NoArgImplAccStatement(ImplAccStatementKind.ZeroRegRflags);
					break;

				case "zero-reg-regmem":
					stmt = new NoArgImplAccStatement(ImplAccStatementKind.ZeroRegRegmem);
					break;

				case "zero-reg-reg-regmem":
					stmt = new NoArgImplAccStatement(ImplAccStatementKind.ZeroRegRegRegmem);
					break;

				case "arpl":
					stmt = new NoArgImplAccStatement(ImplAccStatementKind.Arpl);
					break;

				case "last-gpr-8":
					stmt = new NoArgImplAccStatement(ImplAccStatementKind.LastGpr8);
					break;

				case "last-gpr-16":
					stmt = new NoArgImplAccStatement(ImplAccStatementKind.LastGpr16);
					break;

				case "last-gpr-32":
					stmt = new NoArgImplAccStatement(ImplAccStatementKind.LastGpr32);
					break;

				case "lea":
					stmt = new NoArgImplAccStatement(ImplAccStatementKind.lea);
					break;

				case "cmps":
					stmt = new NoArgImplAccStatement(ImplAccStatementKind.Cmps);
					break;

				case "ins":
					stmt = new NoArgImplAccStatement(ImplAccStatementKind.Ins);
					break;

				case "lods":
					stmt = new NoArgImplAccStatement(ImplAccStatementKind.Lods);
					break;

				case "movs":
					stmt = new NoArgImplAccStatement(ImplAccStatementKind.Movs);
					break;

				case "outs":
					stmt = new NoArgImplAccStatement(ImplAccStatementKind.Outs);
					break;

				case "scas":
					stmt = new NoArgImplAccStatement(ImplAccStatementKind.Scas);
					break;

				case "stos":
					stmt = new NoArgImplAccStatement(ImplAccStatementKind.Stos);
					break;

				case "xstore":
					stmt = new NoArgImplAccStatement(ImplAccStatementKind.Xstore);
					break;

				case "emmi-reg":
					EmmiAccess emmiAccess;
					switch (value) {
					case "r": emmiAccess = EmmiAccess.Read; break;
					case "w": emmiAccess = EmmiAccess.Write; break;
					case "rw": emmiAccess = EmmiAccess.ReadWrite; break;
					default:
						error = $"Unknown access: `{value}`";
						return false;
					}
					stmt = new EmmiImplAccStatement(emmiAccess);
					break;

				case "shift-mask-1F-mod":
					if (!ParserUtils.TryParseUInt32(value, out arg1, out error))
						return false;
					if (arg1 != 9 && arg1 != 17) {
						error = $"Invalid modulus: `{value}`";
						return false;
					}
					stmt = new IntArgImplAccStatement(ImplAccStatementKind.ShiftMask1FMod, arg1);
					break;

				case "enter":
					if (!TryParse_2_4_or_8(value, out arg1, out error))
						return false;
					stmt = new IntArgImplAccStatement(ImplAccStatementKind.Enter, arg1);
					break;

				case "leave":
					if (!TryParse_2_4_or_8(value, out arg1, out error))
						return false;
					stmt = new IntArgImplAccStatement(ImplAccStatementKind.Leave, arg1);
					break;

				case "push":
					if (!TryParsePushPopArgs(value, out arg1, out arg2, out error))
						return false;
					stmt = new IntX2ArgImplAccStatement(ImplAccStatementKind.Push, arg1, arg2);
					break;

				case "pop":
					if (!TryParsePushPopArgs(value, out arg1, out arg2, out error))
						return false;
					stmt = new IntX2ArgImplAccStatement(ImplAccStatementKind.Pop, arg1, arg2);
					break;

				case "pop-rm":
					if (!TryParse_2_4_or_8(value, out arg1, out error))
						return false;
					stmt = new IntArgImplAccStatement(ImplAccStatementKind.PopRm, arg1);
					break;

				case "pusha":
					if (!TryParse_2_or_4(value, out arg1, out error))
						return false;
					stmt = new IntArgImplAccStatement(ImplAccStatementKind.Pusha, arg1);
					break;

				case "popa":
					if (!TryParse_2_or_4(value, out arg1, out error))
						return false;
					stmt = new IntArgImplAccStatement(ImplAccStatementKind.Popa, arg1);
					break;

				default:
					error = $"Unknown key=value: `{keyValue}`";
					return false;
				}
				if (stmt is object)
					implAccCond.TrueStatements.Add(stmt);
			}

			error = null;
			return true;
		}

		bool TryParseImplAccRegOrMem(List<ImplAccStatement> statements, string value, OpAccess access, [NotNullWhen(false)] out string? error) {
			var parts = value.Split(';', StringSplitOptions.RemoveEmptyEntries);
			if (parts.Length == 0) {
				error = "Missing value";
				return false;
			}

			foreach (var part in parts) {
				ImplAccStatement? stmt;
				if (part[0] == '[') {
					if (!TryParseImplAccMem(part, access, out stmt, out error))
						return false;
					statements.Add(stmt);
				}
				else {
					if (TrySplit(part, '-', out var left, out var right)) {
						if (!TryParseRegister(left, out var regLeft, out error))
							return false;
						if (!TryParseRegister(right, out var regRight, out error))
							return false;
						if (regLeft.Register is null || regLeft.Kind != ImplAccRegisterKind.Register) {
							error = $"Must be a normal register: `{left}`";
							return false;
						}
						if (regRight.Register is null || regRight.Kind != ImplAccRegisterKind.Register) {
							error = $"Must be a normal register: `{right}`";
							return false;
						}
						if (regLeft.Register.Value > regRight.Register.Value) {
							error = $"Left reg ({left}) must be <= right reg ({right})";
							return false;
						}
						statements.Add(new RegisterRangeImplAccStatement(access, regLeft.Register, regRight.Register));
					}
					else {
						if (!TryParseImplAccReg(part, access, out stmt, out error))
							return false;
						statements.Add(stmt);
					}
				}
			}

			error = null;
			return true;
		}

		bool TryParseMemorySize(string value, out ImplAccMemorySize memorySize, [NotNullWhen(false)] out string? error) {
			memorySize = default;

			if (toMemorySize.TryGetValue(value, out var memSize)) {
				memorySize = new ImplAccMemorySize(memSize);
				error = null;
				return true;
			}

			switch (value) {
			case "default":
				memorySize = new ImplAccMemorySize(ImplAccMemorySizeKind.Default);
				break;

			default:
				error = $"Unknown memory size: `{value}`";
				return false;
			}

			error = null;
			return true;
		}

		bool TryParseRegister(string regStr, [NotNullWhen(true)] out ImplAccRegister register, [NotNullWhen(false)] out string? error) {
			register = default;

			if (toRegisterImplAcc.TryGetValue(regStr, out var regEnumValue)) {
				register = new ImplAccRegister(regEnumValue);
				error = null;
				return true;
			}

			switch (regStr) {
			case "seg": register = new ImplAccRegister(ImplAccRegisterKind.SegmentDefaultDS); break;
			case "a_rDI": register = new ImplAccRegister(ImplAccRegisterKind.a_rDI); break;
			case "op0-reg": register = new ImplAccRegister(ImplAccRegisterKind.Op0); break;
			case "op1-reg": register = new ImplAccRegister(ImplAccRegisterKind.Op1); break;
			case "op2-reg": register = new ImplAccRegister(ImplAccRegisterKind.Op2); break;
			case "op3-reg": register = new ImplAccRegister(ImplAccRegisterKind.Op3); break;
			case "op4-reg": register = new ImplAccRegister(ImplAccRegisterKind.Op4); break;
			default:
				error = $"Unknown register `{regStr}";
				return false;
			}
			error = null;
			return true;
		}

		static bool TrySplit(string value, char c, [NotNullWhen(true)] out string? left, [NotNullWhen(true)] out string? right) {
			int index = value.IndexOf(c);
			if (index < 0) {
				left = null;
				right = null;
				return false;
			}
			else {
				left = value.Substring(0, index).Trim();
				right = value.Substring(index + 1).Trim();
				return true;
			}
		}

		bool TryParseImplAccMem(string value, OpAccess access, [NotNullWhen(true)] out ImplAccStatement? stmt, [NotNullWhen(false)] out string? error) {
			stmt = null;

			if (value.Length == 0 || value[0] != '[' || value[^1] != ']') {
				error = "Memory must be inside `[` and `]`";
				return false;
			}

			value = value.Substring(1, value.Length - 2).Trim();

			if (!TrySplit(value, '=', out var left, out var memSize)) {
				error = "Missing memory size";
				return false;
			}
			value = left;
			if (!TryParseMemorySize(memSize, out var memorySize, out error))
				return false;

			ImplAccRegister? segment;
			if (TrySplit(value, ':', out var segStr, out var right)) {
				value = right;
				if (!TryParseRegister(segStr, out var seg, out error))
					return false;
				segment = seg;
			}
			else
				segment = null;

			if (!TryParseRegister(value, out var @base, out error))
				return false;

			stmt = new MemoryImplAccStatement(access, segment, @base, null, 1, memorySize);
			error = null;
			return true;
		}

		bool TryParseImplAccReg(string value, OpAccess access, [NotNullWhen(true)] out ImplAccStatement? stmt, [NotNullWhen(false)] out string? error) {
			stmt = null;

			if (!TryParseRegister(value, out var register, out error))
				return false;

			stmt = new RegisterImplAccStatement(access, register, false);
			error = null;
			return true;
		}

		static (string key, string cond) GetKeyCond(string keyCond) {
			int index = keyCond.IndexOf(';');
			if (index < 0)
				return (keyCond, string.Empty);
			var key = keyCond.Substring(0, index).Trim();
			var cond = keyCond.Substring(index + 1).Trim();
			return (key, cond);
		}
	}
}
