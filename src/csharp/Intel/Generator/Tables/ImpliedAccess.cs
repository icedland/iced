// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using Generator.Enums;
using Generator.Enums.InstructionInfo;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Generator.Tables {
	sealed class ImpliedAccessesDef {
		public readonly ImpliedAccesses ImpliedAccesses;
		public readonly EnumValue EnumValue;
		public ImpliedAccessesDef(ImpliedAccesses impliedAccesses, EnumValue enumValue) {
			ImpliedAccesses = impliedAccesses;
			EnumValue = enumValue;
		}
	}

	sealed class ImpliedAccesses : IEquatable<ImpliedAccesses?> {
		public readonly List<ImplAccCondition> Conditions;
		public ImpliedAccesses() => Conditions = new List<ImplAccCondition>();
		public ImpliedAccesses(List<ImplAccCondition> conditions) => Conditions = conditions;

		public override bool Equals(object? obj) => Equals(obj as ImpliedAccesses);

		public bool Equals(ImpliedAccesses? other) {
			if (other is null)
				return false;
			if (Conditions.Count != other.Conditions.Count)
				return false;
			for (int i = 0; i < Conditions.Count; i++) {
				if (!Conditions[i].Equals(other.Conditions[i]))
					return false;
			}
			return true;
		}

		public override int GetHashCode() {
			int hc = 0;
			foreach (var cond in Conditions)
				hc = HashCode.Combine(hc, cond);
			return hc;
		}
	}

	enum ImplAccConditionKind {
		// No condition
		None,
		// 64-bit code only
		Bit64,
		// Not 64-bit code (16 or 32-bit code only)
		NotBit64,
	}

	sealed class ImplAccCondition : IEquatable<ImplAccCondition?> {
		public readonly ImplAccConditionKind Kind;
		public readonly List<ImplAccStatement> TrueStatements = new();
		public readonly List<ImplAccStatement> FalseStatements = new();
		public ImplAccCondition(ImplAccConditionKind kind) => Kind = kind;

		public override bool Equals(object? obj) => Equals(obj as ImplAccCondition);

		public bool Equals(ImplAccCondition? other) =>
			other is not null && Kind == other.Kind && Equals(TrueStatements, other.TrueStatements) && Equals(FalseStatements, other.FalseStatements);

		static bool Equals(List<ImplAccStatement> a, List<ImplAccStatement> b) {
			if (a.Count != b.Count)
				return false;
			for (int i = 0; i < a.Count; i++) {
				if (!a[i].Equals(b[i]))
					return false;
			}
			return true;
		}

		public override int GetHashCode() {
			int hc = HashCode.Combine(Kind);
			foreach (var stmt in TrueStatements)
				hc = HashCode.Combine(hc, stmt);
			foreach (var stmt in FalseStatements)
				hc = HashCode.Combine(hc, stmt);
			return hc;
		}
	}

	enum ImplAccRegisterKind {
		/// <summary>A Register enum value</summary>
		Register,
		/// <summary>Use segment prefix, or DS if no segment prefix</summary>
		SegmentDefaultDS,
		/// <summary>DI, EDI, or RDI depending on address size</summary>
		a_rDI,
		/// <summary>Use op #0's register prop</summary>
		Op0,
		/// <summary>Use op #1's register prop</summary>
		Op1,
		/// <summary>Use op #2's register prop</summary>
		Op2,
		/// <summary>Use op #3's register prop</summary>
		Op3,
		/// <summary>Use op #4's register prop</summary>
		Op4,
	}

	readonly struct ImplAccRegister : IEquatable<ImplAccRegister>, IComparable<ImplAccRegister> {
		public readonly ImplAccRegisterKind Kind;
		public readonly EnumValue? Register;
		public ImplAccRegister(EnumValue register) {
			Kind = ImplAccRegisterKind.Register;
			Register = register;
		}
		public ImplAccRegister(ImplAccRegisterKind kind) {
			if (kind == ImplAccRegisterKind.Register)
				throw new ArgumentOutOfRangeException(nameof(kind));
			Kind = kind;
			Register = null;
		}
		public static bool operator ==(ImplAccRegister left, ImplAccRegister right) => left.Equals(right);
		public static bool operator !=(ImplAccRegister left, ImplAccRegister right) => !left.Equals(right);
		public bool Equals(ImplAccRegister other) => Kind == other.Kind && Register == other.Register;
		public override bool Equals(object? obj) => obj is ImplAccRegister other && Equals(other);
		public override int GetHashCode() => HashCode.Combine(Kind, Register);
		public int CompareTo(ImplAccRegister other) {
			int c;
			c = Kind.CompareTo(other.Kind);
			if (c != 0) return c;
			if (Kind == ImplAccRegisterKind.Register) {
				if (Register is EnumValue r1 && other.Register is EnumValue r2)
					return r1.Value.CompareTo(r2.Value);
				throw new InvalidOperationException();
			}
			else {
				if (Register is not null || other.Register is not null)
					throw new InvalidOperationException();
				return 0;
			}
		}
	}

	enum ImplAccMemorySizeKind {
		/// <summary>A MemorySize enum value</summary>
		MemorySize,
		/// <summary>Use the instruction's memory size property</summary>
		Default,
	}

	readonly struct ImplAccMemorySize : IEquatable<ImplAccMemorySize>, IComparable<ImplAccMemorySize> {
		public readonly ImplAccMemorySizeKind Kind;
		public readonly EnumValue? MemorySize;
		public ImplAccMemorySize(EnumValue memorySize) {
			Kind = ImplAccMemorySizeKind.MemorySize;
			MemorySize = memorySize;
		}
		public ImplAccMemorySize(ImplAccMemorySizeKind kind) {
			if (kind == ImplAccMemorySizeKind.MemorySize)
				throw new ArgumentOutOfRangeException(nameof(kind));
			Kind = kind;
			MemorySize = null;
		}
		public static bool operator ==(ImplAccMemorySize left, ImplAccMemorySize right) => left.Equals(right);
		public static bool operator !=(ImplAccMemorySize left, ImplAccMemorySize right) => !left.Equals(right);
		public bool Equals(ImplAccMemorySize other) => Kind == other.Kind && MemorySize == other.MemorySize;
		public override bool Equals(object? obj) => obj is ImplAccMemorySize size && Equals(size);
		public override int GetHashCode() => HashCode.Combine(Kind, MemorySize);
		public int CompareTo(ImplAccMemorySize other) {
			if (Kind != other.Kind)
				return Kind.CompareTo(other.Kind);
			if (Kind == ImplAccMemorySizeKind.MemorySize) {
				if (MemorySize is EnumValue m1 && other.MemorySize is EnumValue m2)
					return m1.Value.CompareTo(m2.Value);
				throw new InvalidOperationException();
			}
			else {
				if (MemorySize is not null && other.MemorySize is not null)
					throw new InvalidOperationException();
				return 0;
			}
		}
	}

	enum ImplAccStatementKind {
		// These check an internal array and won't work if other statements add more registers. Put them first so they're sorted first.
		Arpl,
		LastGpr8,
		LastGpr16,
		LastGpr32,

		MemoryAccess,
		RegisterAccess,
		RegisterRangeAccess,
		ShiftMask,
		ShiftMask1FMod,
		ZeroRegRflags,
		ZeroRegRegmem,
		ZeroRegRegRegmem,
		EmmiReg,
		Enter,
		Leave,
		Push,
		Pop,
		PopRm,
		Pusha,
		Popa,
		lea,
		Cmps,
		Ins,
		Lods,
		Movs,
		Outs,
		Scas,
		Stos,
		Xstore,
		MemDispl,
	}

	abstract class ImplAccStatement : IEquatable<ImplAccStatement> {
		public abstract ImplAccStatementKind Kind { get; }
		public abstract bool Equals([AllowNull] ImplAccStatement other);
		public sealed override bool Equals(object? obj) => obj is ImplAccStatement other && Equals(other);
		public abstract override int GetHashCode();
	}

	sealed class NoArgImplAccStatement : ImplAccStatement {
		public override ImplAccStatementKind Kind { get; }
		public NoArgImplAccStatement(ImplAccStatementKind kind) => Kind = kind;
		public override bool Equals([AllowNull] ImplAccStatement obj) => obj is NoArgImplAccStatement other && Kind == other.Kind;
		public override int GetHashCode() => HashCode.Combine(Kind);
	}

	enum EmmiAccess {
		Read,
		Write,
		ReadWrite,
	}

	sealed class EmmiImplAccStatement : ImplAccStatement, IComparable<EmmiImplAccStatement> {
		public override ImplAccStatementKind Kind => ImplAccStatementKind.EmmiReg;
		public readonly EmmiAccess Access;
		public EmmiImplAccStatement(EmmiAccess access) => Access = access;
		public override bool Equals([AllowNull] ImplAccStatement obj) => obj is EmmiImplAccStatement other && Access == other.Access;
		public override int GetHashCode() => HashCode.Combine(Access);
		public int CompareTo([AllowNull] EmmiImplAccStatement other) {
			if (other is null)
				return 1;
			return Access.CompareTo(other.Access);
		}
	}

	sealed class IntArgImplAccStatement : ImplAccStatement, IComparable<IntArgImplAccStatement> {
		public override ImplAccStatementKind Kind { get; }
		public readonly uint Arg;
		public IntArgImplAccStatement(ImplAccStatementKind kind, uint arg) {
			Kind = kind;
			Arg = arg;
		}
		public override bool Equals([AllowNull] ImplAccStatement obj) => obj is IntArgImplAccStatement other && Kind == other.Kind && Arg == other.Arg;
		public override int GetHashCode() => HashCode.Combine(Kind, Arg);
		public int CompareTo([AllowNull] IntArgImplAccStatement other) {
			if (other is null)
				return 1;
			return Arg.CompareTo(other.Arg);
		}
	}

	sealed class IntX2ArgImplAccStatement : ImplAccStatement, IComparable<IntX2ArgImplAccStatement> {
		public override ImplAccStatementKind Kind { get; }
		public readonly uint Arg1;
		public readonly uint Arg2;
		public IntX2ArgImplAccStatement(ImplAccStatementKind kind, uint arg1, uint arg2) {
			Kind = kind;
			Arg1 = arg1;
			Arg2 = arg2;
		}
		public override bool Equals([AllowNull] ImplAccStatement obj) => obj is IntX2ArgImplAccStatement other && Kind == other.Kind && Arg1 == other.Arg1 && Arg2 == other.Arg2;
		public override int GetHashCode() => HashCode.Combine(Kind, Arg1, Arg2);
		public int CompareTo([AllowNull] IntX2ArgImplAccStatement other) {
			if (other is null)
				return 1;
			int c;
			c = Kind.CompareTo(other.Kind);
			if (c != 0) return c;
			c = Arg1.CompareTo(other.Arg1);
			if (c != 0) return c;
			return Arg2.CompareTo(other.Arg2);
		}
	}

	sealed class RegisterRangeImplAccStatement : ImplAccStatement, IComparable<RegisterRangeImplAccStatement> {
		public override ImplAccStatementKind Kind => ImplAccStatementKind.RegisterRangeAccess;
		public readonly OpAccess Access;
		public readonly EnumValue RegisterFirst;
		public readonly EnumValue RegisterLast;
		public RegisterRangeImplAccStatement(OpAccess access, EnumValue registerFirst, EnumValue registerLast) {
			Access = access;
			RegisterFirst = registerFirst;
			RegisterLast = registerLast;
		}
		public override bool Equals([AllowNull] ImplAccStatement obj) =>
			obj is RegisterRangeImplAccStatement other && Access == other.Access && RegisterFirst == other.RegisterFirst && RegisterLast == other.RegisterLast;
		public override int GetHashCode() => HashCode.Combine(Access, RegisterFirst, RegisterLast);
		public int CompareTo([AllowNull] RegisterRangeImplAccStatement other) {
			if (other is null)
				return 1;
			int c;
			c = Access.CompareTo(other.Access);
			if (c != 0) return c;
			c = RegisterFirst.Value.CompareTo(other.RegisterFirst.Value);
			if (c != 0) return c;
			return RegisterLast.Value.CompareTo(other.RegisterLast.Value);
		}
	}

	sealed class RegisterImplAccStatement : ImplAccStatement, IComparable<RegisterImplAccStatement> {
		public override ImplAccStatementKind Kind => ImplAccStatementKind.RegisterAccess;
		public readonly OpAccess Access;
		public readonly ImplAccRegister Register;
		public readonly bool IsMemOpSegRead;
		public RegisterImplAccStatement(OpAccess access, ImplAccRegister register, bool isMemOpSegRead) {
			IsMemOpSegRead = register.Kind == ImplAccRegisterKind.SegmentDefaultDS;
			if (isMemOpSegRead) {
				if (register.Kind == ImplAccRegisterKind.Register) {
					switch ((Register)register.Register!.Value) {
					case Enums.Register.ES:
					case Enums.Register.CS:
					case Enums.Register.SS:
					case Enums.Register.DS:
						IsMemOpSegRead = true;
						break;
					}
				}
			}
			Access = access;
			Register = register;
		}
		public override bool Equals([AllowNull] ImplAccStatement obj) =>
			obj is RegisterImplAccStatement other && Access == other.Access && Register == other.Register && IsMemOpSegRead == other.IsMemOpSegRead;
		public override int GetHashCode() => HashCode.Combine(Access, Register, IsMemOpSegRead);
		public int CompareTo([AllowNull] RegisterImplAccStatement other) {
			if (other is null)
				return 1;
			int c;
			c = Access.CompareTo(other.Access);
			if (c != 0) return c;
			c = IsMemOpSegRead.CompareTo(other.IsMemOpSegRead);
			if (c != 0) return c;
			return Register.CompareTo(other.Register);
		}
	}

	sealed class MemoryImplAccStatement : ImplAccStatement, IComparable<MemoryImplAccStatement> {
		public override ImplAccStatementKind Kind => ImplAccStatementKind.MemoryAccess;
		public readonly OpAccess Access;
		public readonly ImplAccRegister? Segment;
		public readonly ImplAccRegister? Base;
		public readonly ImplAccRegister? Index;
		public readonly int Scale;
		public ulong Displacement => 0;
		public readonly ImplAccMemorySize MemorySize;
		public readonly CodeSize AddressSize;
		public readonly uint VsibSize;
		public MemoryImplAccStatement(OpAccess access, ImplAccRegister? segment, ImplAccRegister? @base, ImplAccRegister? index, int scale, ImplAccMemorySize memorySize, CodeSize addressSize, uint vsibSize) {
			if (vsibSize != 0 && vsibSize != 4 && vsibSize != 8)
				throw new ArgumentOutOfRangeException(nameof(vsibSize));
			Access = access;
			Segment = segment;
			Base = @base;
			Index = index;
			Scale = scale;
			MemorySize = memorySize;
			AddressSize = addressSize;
			VsibSize = vsibSize;
		}
		public override bool Equals([AllowNull] ImplAccStatement obj) =>
			obj is MemoryImplAccStatement other &&
			Access == other.Access && Segment == other.Segment &&
			Base == other.Base && Index == other.Index && Scale == other.Scale &&
			MemorySize == other.MemorySize &&
			AddressSize == other.AddressSize &&
			VsibSize == other.VsibSize;
		public override int GetHashCode() => HashCode.Combine(Access, Segment, Base, Index, Scale, MemorySize, AddressSize, VsibSize);
		public int CompareTo([AllowNull] MemoryImplAccStatement other) {
			if (other is null)
				return 1;
			int c;
			c = Access.CompareTo(other.Access);
			if (c != 0) return c;
			c = CompareTo(Segment, other.Segment);
			if (c != 0) return c;
			c = CompareTo(Base, other.Base);
			if (c != 0) return c;
			c = CompareTo(Index, other.Index);
			if (c != 0) return c;
			c = Scale.CompareTo(other.Scale);
			if (c != 0) return c;
			c = MemorySize.CompareTo(other.MemorySize);
			if (c != 0) return c;
			c = AddressSize.CompareTo(other.AddressSize);
			if (c != 0) return c;
			return VsibSize.CompareTo(other.VsibSize);
		}

		static int CompareTo(ImplAccRegister? a, ImplAccRegister? b) {
			if (a == b)
				return 0;
			if (a is null)
				return -1;
			if (b is null)
				return 1;
			return a.GetValueOrDefault().CompareTo(b.GetValueOrDefault());
		}
	}

	sealed class ImpliedAccessEnumFactory {
		readonly Dictionary<ImpliedAccesses, ImpliedAccessesDef> toDef;
		readonly List<ImpliedAccessesDef> hardCodedDefs;
		readonly List<ImpliedAccessesDef> otherDefs;
		readonly ImpliedAccessesDef noneDef;
		readonly HashSet<string> usedEnumNames;
		readonly StringBuilder sb;

		public ImpliedAccessEnumFactory() {
			toDef = new Dictionary<ImpliedAccesses, ImpliedAccessesDef>();
			hardCodedDefs = new List<ImpliedAccessesDef>();
			otherDefs = new List<ImpliedAccessesDef>();
			usedEnumNames = new HashSet<string>(StringComparer.Ordinal);
			sb = new StringBuilder();

			noneDef = AddHardCodedValue("None", new ImpliedAccesses());
			// The order is important. The code assumes this order, see Instruction.Info.cs
			AddHardCodedValue("Shift_Ib_MASK1FMOD9", new IntArgImplAccStatement(ImplAccStatementKind.ShiftMask1FMod, 9));
			AddHardCodedValue("Shift_Ib_MASK1FMOD11", new IntArgImplAccStatement(ImplAccStatementKind.ShiftMask1FMod, 17));
			AddHardCodedValue("Shift_Ib_MASK1F", new IntArgImplAccStatement(ImplAccStatementKind.ShiftMask, 0x1F));
			AddHardCodedValue("Shift_Ib_MASK3F", new IntArgImplAccStatement(ImplAccStatementKind.ShiftMask, 0x3F));
			AddHardCodedValue("Clear_rflags", new NoArgImplAccStatement(ImplAccStatementKind.ZeroRegRflags));
		}

		void AddHardCodedValue(string enumValueName, ImplAccStatement impAcc) {
			var cond = new ImplAccCondition(ImplAccConditionKind.None);
			cond.TrueStatements.Add(impAcc);
			var accesses = new ImpliedAccesses(new List<ImplAccCondition> { cond });
			AddHardCodedValue(enumValueName, accesses);
		}

		ImpliedAccessesDef AddHardCodedValue(string enumValueName, ImpliedAccesses accesses) {
			usedEnumNames.Add(enumValueName);
			var enumValue = new EnumValue(0, enumValueName, default);
			var def = new ImpliedAccessesDef(accesses, enumValue);
			hardCodedDefs.Add(def);
			toDef.Add(accesses, def);
			return def;
		}

		public ImpliedAccessesDef Add(ImpliedAccesses? accesses) {
			if (accesses is null)
				return noneDef;
			if (toDef.TryGetValue(accesses, out var def))
				return def;

			var name = GetEnumName(accesses);
			var enumValue = new EnumValue(0, name, default);
			def = new ImpliedAccessesDef(accesses, enumValue);
			toDef.Add(accesses, def);
			otherDefs.Add(def);
			return def;
		}

		string GetEnumName(ImpliedAccesses accesses) {
			var name = CreateName(sb, accesses);
			if (name.Length == 0)
				throw new InvalidOperationException();

			const int MaxNameLength = 100;
			if (name.Length > MaxNameLength)
				name = name[0..MaxNameLength] + "_etc";

			for (int i = 0; ; i++) {
				var newName = i == 0 ? name : name + "_" + i.ToString();
				if (usedEnumNames.Add(newName))
					return newName;
			}

			throw new InvalidOperationException();
		}

		static string CreateName(StringBuilder sb, ImpliedAccesses accesses) {
			sb.Clear();
			foreach (var cond in accesses.Conditions) {
				if (sb.Length > 0)
					sb.Append('_');
				switch (cond.Kind) {
				case ImplAccConditionKind.None: break;
				case ImplAccConditionKind.Bit64: sb.Append("b64"); break;
				case ImplAccConditionKind.NotBit64: sb.Append("n64"); break;
				default: throw new InvalidOperationException();
				}
				CreateName(sb, "t", cond.TrueStatements);
				CreateName(sb, "f", cond.FalseStatements);
			}
			return sb.ToString();
		}

		static void CreateName(StringBuilder sb, string name, List<ImplAccStatement> stmts) {
			if (stmts.Count == 0)
				return;
			if (sb.Length > 0)
				sb.Append('_');
			sb.Append(name);
			foreach (var stmt in stmts) {
				sb.Append('_');
				IntArgImplAccStatement arg1;
				IntX2ArgImplAccStatement arg2;
				switch (stmt.Kind) {
				case ImplAccStatementKind.MemoryAccess:
					var mem = (MemoryImplAccStatement)stmt;
					sb.Append(GetAccess(mem.Access));
					sb.Append("mem");
					break;
				case ImplAccStatementKind.RegisterAccess:
					var reg = (RegisterImplAccStatement)stmt;
					sb.Append(GetAccess(reg.Access));
					sb.Append(GetRegister(reg.Register));
					break;
				case ImplAccStatementKind.RegisterRangeAccess:
					var rreg = (RegisterRangeImplAccStatement)stmt;
					sb.Append(GetAccess(rreg.Access));
					sb.Append(rreg.RegisterFirst.RawName.ToLowerInvariant());
					sb.Append("TO");
					sb.Append(rreg.RegisterLast.RawName.ToLowerInvariant());
					break;
				case ImplAccStatementKind.ShiftMask:
					arg1 = (IntArgImplAccStatement)stmt;
					sb.Append($"sm{arg1.Arg:X}");
					break;
				case ImplAccStatementKind.ZeroRegRflags:
					sb.Append("zrfl");
					break;
				case ImplAccStatementKind.ZeroRegRegmem:
					sb.Append("zrrm");
					break;
				case ImplAccStatementKind.ZeroRegRegRegmem:
					sb.Append("zrrrm");
					break;
				case ImplAccStatementKind.Arpl:
					sb.Append("arpl");
					break;
				case ImplAccStatementKind.LastGpr8:
					sb.Append("gpr8");
					break;
				case ImplAccStatementKind.LastGpr16:
					sb.Append("gpr16");
					break;
				case ImplAccStatementKind.LastGpr32:
					sb.Append("gpr32");
					break;
				case ImplAccStatementKind.lea:
					sb.Append("lea");
					break;
				case ImplAccStatementKind.Cmps:
					sb.Append("cmps");
					break;
				case ImplAccStatementKind.Ins:
					sb.Append("ins");
					break;
				case ImplAccStatementKind.Lods:
					sb.Append("lods");
					break;
				case ImplAccStatementKind.Movs:
					sb.Append("movs");
					break;
				case ImplAccStatementKind.Outs:
					sb.Append("outs");
					break;
				case ImplAccStatementKind.Scas:
					sb.Append("scas");
					break;
				case ImplAccStatementKind.Stos:
					sb.Append("stos");
					break;
				case ImplAccStatementKind.Xstore:
					arg1 = (IntArgImplAccStatement)stmt;
					sb.Append($"xstore{arg1.Arg}");
					break;
				case ImplAccStatementKind.MemDispl:
					arg1 = (IntArgImplAccStatement)stmt;
					if ((int)arg1.Arg < 0)
						sb.Append($"memdisplm{-(int)arg1.Arg}");
					else
						sb.Append($"memdisplp{(int)arg1.Arg}");
					break;
				case ImplAccStatementKind.ShiftMask1FMod:
					arg1 = (IntArgImplAccStatement)stmt;
					sb.Append($"sm1Fm{arg1.Arg}");
					break;
				case ImplAccStatementKind.Enter:
					arg1 = (IntArgImplAccStatement)stmt;
					sb.Append($"enter{arg1.Arg}");
					break;
				case ImplAccStatementKind.Leave:
					arg1 = (IntArgImplAccStatement)stmt;
					sb.Append($"leave{arg1.Arg}");
					break;
				case ImplAccStatementKind.PopRm:
					arg1 = (IntArgImplAccStatement)stmt;
					sb.Append($"poprm{arg1.Arg}");
					break;
				case ImplAccStatementKind.Pusha:
					arg1 = (IntArgImplAccStatement)stmt;
					sb.Append($"pusha{arg1.Arg}");
					break;
				case ImplAccStatementKind.Popa:
					arg1 = (IntArgImplAccStatement)stmt;
					sb.Append($"popa{arg1.Arg}");
					break;
				case ImplAccStatementKind.Push:
					arg2 = (IntX2ArgImplAccStatement)stmt;
					sb.Append($"push{arg2.Arg1}x{arg2.Arg2}");
					break;
				case ImplAccStatementKind.Pop:
					arg2 = (IntX2ArgImplAccStatement)stmt;
					sb.Append($"pop{arg2.Arg1}x{arg2.Arg2}");
					break;
				case ImplAccStatementKind.EmmiReg:
					var emmi = (EmmiImplAccStatement)stmt;
					sb.Append("emmi");
					switch (emmi.Access) {
					case EmmiAccess.Read: sb.Append('R'); break;
					case EmmiAccess.Write: sb.Append('W'); break;
					case EmmiAccess.ReadWrite: sb.Append("RW"); break;
					default: throw new InvalidOperationException();
					}
					break;
				default:
					throw new InvalidOperationException();
				}
			}
		}

		static string GetRegister(ImplAccRegister register) =>
			register.Kind switch {
				ImplAccRegisterKind.Register => register.Register!.RawName.ToLowerInvariant(),
				ImplAccRegisterKind.SegmentDefaultDS => "seg",
				ImplAccRegisterKind.a_rDI => "arDI",
				ImplAccRegisterKind.Op0 => "op0reg",
				ImplAccRegisterKind.Op1 => "op1reg",
				ImplAccRegisterKind.Op2 => "op2reg",
				ImplAccRegisterKind.Op3 => "op3reg",
				ImplAccRegisterKind.Op4 => "op4reg",
				_ => throw new InvalidOperationException(),
			};

		static string GetAccess(OpAccess access) =>
			access switch {
				OpAccess.None => string.Empty,
				OpAccess.Read => "R",
				OpAccess.CondRead => "CR",
				OpAccess.Write => "W",
				OpAccess.CondWrite => "CW",
				OpAccess.ReadWrite => "RW",
				OpAccess.ReadCondWrite => "RCW",
				OpAccess.NoMemAccess => "NMA",
				_ => throw new InvalidOperationException(),
			};

		public (EnumType type, ImpliedAccessesDef[] defs) CreateEnum() {
			var values = new List<ImpliedAccessesDef>(hardCodedDefs.Count + otherDefs.Count);
			values.AddRange(hardCodedDefs);
			values.AddRange(otherDefs);
			return (new EnumType(TypeIds.ImpliedAccess, default, values.Select(a => a.EnumValue).ToArray(), EnumTypeFlags.None), values.ToArray());
		}
	}
}
