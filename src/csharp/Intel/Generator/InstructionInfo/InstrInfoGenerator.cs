// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Linq;
using Generator.Constants;
using Generator.Enums;
using Generator.Enums.InstructionInfo;
using Generator.IO;
using Generator.Tables;

namespace Generator.InstructionInfo {
	abstract class InstrInfoGenerator {
		protected abstract void Generate(EnumType enumType);
		protected abstract void Generate(ConstantsType constantsType);
		protected abstract void Generate((InstructionDef def, uint dword1, uint dword2)[] infos);
		protected abstract void Generate(EnumValue[] enumValues, RflagsBits[] read, RflagsBits[] undefined, RflagsBits[] written, RflagsBits[] cleared, RflagsBits[] set, RflagsBits[] modified);
		protected abstract void Generate((EnumValue cpuidInternal, EnumValue[] cpuidFeatures)[] cpuidFeatures);
		protected abstract void GenerateImpliedAccesses(ImpliedAccessesDef[] defs);
		protected abstract void GenerateIgnoresSegmentTable((EncodingKind encoding, InstructionDef[] defs)[] defs);
		protected abstract void GenerateIgnoresIndexTable((EncodingKind encoding, InstructionDef[] defs)[] defs);
		protected abstract void GenerateTileStrideIndexTable((EncodingKind encoding, InstructionDef[] defs)[] defs);
		protected abstract void GenerateIsStringOpTable((EncodingKind encoding, InstructionDef[] defs)[] defs);
		protected abstract void GenerateFpuStackIncrementInfoTable((FpuStackInfo info, InstructionDef[] defs)[] defs);
		protected abstract void GenerateStackPointerIncrementTable((EncodingKind encoding, StackInfo info, InstructionDef[] defs)[] defs);
		protected abstract void GenerateCore();

		protected readonly GenTypes genTypes;
		protected readonly InstrInfoTypes instrInfoTypes;

		protected readonly struct FpuStackInfo : IEquatable<FpuStackInfo>, IComparable<FpuStackInfo> {
			public readonly int Increment;
			public readonly bool Conditional;
			public readonly bool WritesTop;
			public FpuStackInfo(InstructionDef def) {
				Increment = def.FpuStackIncrement;
				Conditional = (def.Flags3 & InstructionDefFlags3.IsFpuCondWriteTop) != 0;
				WritesTop = (def.Flags3 & InstructionDefFlags3.WritesFpuTop) != 0;
			}
			public override bool Equals(object? obj) => obj is FpuStackInfo info && Equals(info);
			public bool Equals(FpuStackInfo other) => Increment == other.Increment && Conditional == other.Conditional && WritesTop == other.WritesTop;
			public override int GetHashCode() => HashCode.Combine(Increment, Conditional, WritesTop);
			public int CompareTo(FpuStackInfo other) {
				int c = Increment.CompareTo(other.Increment);
				if (c != 0) return c;
				c = Conditional.CompareTo(other.Conditional);
				if (c != 0) return c;
				return WritesTop.CompareTo(other.WritesTop);
			}
		}

		protected InstrInfoGenerator(GenTypes genTypes) {
			this.genTypes = genTypes;
			instrInfoTypes = genTypes.GetObject<InstrInfoTypes>(TypeIds.InstrInfoTypes);
		}

		public void Generate() {
			var enumTypes = new List<EnumType> {
				genTypes[TypeIds.ImpliedAccess],
				genTypes[TypeIds.RflagsInfo],
				genTypes[TypeIds.InfoFlags1],
				genTypes[TypeIds.InfoFlags2],
				genTypes[TypeIds.CpuidFeatureInternal],
			};
			enumTypes.AddRange(instrInfoTypes.EnumOpInfos);
			foreach (var enumType in enumTypes)
				Generate(enumType);

			var constantsTypes = new ConstantsType[] {
				genTypes.GetConstantsType(TypeIds.InstrInfoConstants),
			};
			foreach (var constantsType in constantsTypes)
				Generate(constantsType);

			var defs = genTypes.GetObject<InstructionDefs>(TypeIds.InstructionDefs).Defs;
			GenerateImpliedAccesses(genTypes.GetObject<InstructionDefs>(TypeIds.InstructionDefs).ImpliedAccessesDefs);

			static (EncodingKind encoding, InstructionDef[] defs)[] GetDefs(IEnumerable<InstructionDef> defs) =>
				defs.GroupBy(a => a.Encoding, (a, b) => (encoding: a, b.OrderBy(a => a.Code.Value).ToArray())).OrderBy(a => a.encoding).ToArray();
			GenerateIgnoresSegmentTable(GetDefs(defs.Where(a => (a.Flags1 & InstructionDefFlags1.IgnoresSegment) != 0)));
			GenerateIgnoresIndexTable(GetDefs(defs.Where(a => (a.Flags3 & InstructionDefFlags3.IgnoresIndex) != 0)));
			GenerateTileStrideIndexTable(GetDefs(defs.Where(a => (a.Flags3 & InstructionDefFlags3.TileStrideIndex) != 0)));
			GenerateIsStringOpTable(GetDefs(defs.Where(a => (a.Flags3 & InstructionDefFlags3.IsStringOp) != 0)));

			var fpuDefs = defs.
				Where(a => a.FpuStackIncrement != 0 || (a.Flags3 & (InstructionDefFlags3.IsFpuCondWriteTop | InstructionDefFlags3.WritesFpuTop)) != 0).
				GroupBy(a => new FpuStackInfo(a), (a, b) => (info: a, b.OrderBy(a => a.Code.Value).ToArray())).
				OrderBy(a => a.info).ToArray();
			GenerateFpuStackIncrementInfoTable(fpuDefs);

			var stackDefs = defs.
				Where(a => a.StackInfo.Kind != StackInfoKind.None).
				GroupBy(a => (encoding: a.Encoding, info: a.StackInfo), (a, b) => (a.encoding, a.info, b.OrderBy(a => a.Code.Value).ToArray())).
				OrderBy(a => (a.encoding, a.info)).ToArray();
			GenerateStackPointerIncrementTable(stackDefs);

			{
				var shifts = new int[IcedConstants.MaxOpCount] {
					(int)genTypes[TypeIds.InfoFlags1]["OpInfo0Shift"].Value,
					(int)genTypes[TypeIds.InfoFlags1]["OpInfo1Shift"].Value,
					(int)genTypes[TypeIds.InfoFlags1]["OpInfo2Shift"].Value,
					(int)genTypes[TypeIds.InfoFlags1]["OpInfo3Shift"].Value,
					(int)genTypes[TypeIds.InfoFlags1]["OpInfo4Shift"].Value,
				};
				var infos = new (InstructionDef def, uint dword1, uint dword2)[defs.Length];
				for (int i = 0; i < defs.Length; i++) {
					var def = defs[i];
					uint dword1 = 0;
					uint dword2 = 0;

					for (int j = 0; j < def.OpInfoEnum.Length; j++)
						dword1 |= def.OpInfoEnum[j].Value << shifts[j];
					var rflagsInfo = def.RflagsInfo ?? throw new InvalidOperationException();
					if (rflagsInfo.Value > (uint)InfoFlags1.RflagsInfoMask)
						throw new InvalidOperationException();
					dword1 |= rflagsInfo.Value << (int)InfoFlags1.RflagsInfoShift;
					if (def.ImpliedAccessDef.EnumValue.Value > (uint)InfoFlags1.ImpliedAccessMask)
						throw new InvalidOperationException();
					dword1 |= def.ImpliedAccessDef.EnumValue.Value << (int)InfoFlags1.ImpliedAccessShift;
					// TILELOADD{,T1}, TILESTORED: the index reg is the stride indicator. The real index is tilecfg.start_row
					if ((def.Flags3 & InstructionDefFlags3.TileStrideIndex) != 0) dword1 |= (uint)InfoFlags1.IgnoresIndexVA;
					if ((def.Flags1 & InstructionDefFlags1.OpMaskReadWrite) != 0) dword1 |= (uint)InfoFlags1.OpMaskReadWrite;
					if ((def.Flags1 & InstructionDefFlags1.IgnoresSegment) != 0) dword1 |= (uint)InfoFlags1.IgnoresSegment;

					if (def.EncodingValue.Value > (uint)InfoFlags2.EncodingMask)
						throw new InvalidOperationException();
					dword2 |= def.EncodingValue.Value << (int)InfoFlags2.EncodingShift;
					if ((def.Flags1 & InstructionDefFlags1.SaveRestore) != 0) dword2 |= (uint)InfoFlags2.SaveRestore;
					if ((def.Flags1 & InstructionDefFlags1.StackInstruction) != 0) dword2 |= (uint)InfoFlags2.StackInstruction;
					if ((def.Flags3 & InstructionDefFlags3.Privileged) != 0) dword2 |= (uint)InfoFlags2.Privileged;
					if (def.ControlFlow.Value > (uint)InfoFlags2.FlowControlMask)
						throw new InvalidOperationException();
					dword2 |= def.ControlFlow.Value << (int)InfoFlags2.FlowControlShift;
					var cpuidInternal = def.CpuidInternal ?? throw new InvalidOperationException();
					if (cpuidInternal.Value > (uint)InfoFlags2.CpuidFeatureInternalMask)
						throw new InvalidOperationException();
					dword2 |= cpuidInternal.Value << (int)InfoFlags2.CpuidFeatureInternalShift;

					infos[i] = (def, dword1, dword2);
				}
				Generate(infos);
			}

			{
				var rflagsInfos = instrInfoTypes.RflagsInfos;
				var enumValues = new EnumValue[rflagsInfos.Length];
				var read = new RflagsBits[rflagsInfos.Length];
				var undefined = new RflagsBits[rflagsInfos.Length];
				var written = new RflagsBits[rflagsInfos.Length];
				var cleared = new RflagsBits[rflagsInfos.Length];
				var set = new RflagsBits[rflagsInfos.Length];
				var modified = new RflagsBits[rflagsInfos.Length];
				for (int i = 0; i < rflagsInfos.Length; i++) {
					var rflags = rflagsInfos[i].rflags;
					enumValues[i] = rflagsInfos[i].value;
					read[i] = rflags.read;
					undefined[i] = rflags.undefined;
					written[i] = rflags.written;
					cleared[i] = rflags.cleared;
					set[i] = rflags.set;
					modified[i] = rflags.undefined | rflags.written | rflags.cleared | rflags.set;
				}
				Generate(enumValues, read, undefined, written, cleared, set, modified);
			}

			Generate(instrInfoTypes.CpuidFeatures);

			GenerateCore();
		}

		protected EnumValue ToOpAccess(EnumValue opInfo) {
			if (opInfo.RawName == nameof(OpInfo.ReadP3))
				return genTypes[TypeIds.OpAccess][nameof(OpAccess.Read)];
			if (opInfo.RawName == nameof(OpInfo.WriteForceP1))
				return genTypes[TypeIds.OpAccess][nameof(OpAccess.Write)];
			return genTypes[TypeIds.OpAccess][opInfo.RawName];
		}

		protected struct StmtState {
			readonly string regBegin;
			readonly string regEnd;
			readonly string memBegin;
			readonly string memEnd;
			StmtKind stmtKind;
			FileWriter.Indenter? indenter;

			public StmtState(string regBegin, string regEnd, string memBegin, string memEnd) {
				this.regBegin = regBegin;
				this.regEnd = regEnd;
				this.memBegin = memBegin;
				this.memEnd = memEnd;
				stmtKind = StmtKind.Other;
				indenter = null;
			}

			enum StmtKind {
				Other,
				Register,
				Memory,
			}

			public void Done(FileWriter writer) => SetKind(writer, StmtKind.Other);
			public void SetKind(FileWriter writer, ImplAccStatementKind kind) => SetKind(writer, GetStmtKind(kind));

			void SetKind(FileWriter writer, StmtKind kind) {
				if (kind == stmtKind)
					return;

				indenter?.Dispose();
				indenter = null;
				switch (stmtKind) {
				case StmtKind.Other:
					break;
				case StmtKind.Register:
					writer.WriteLine(regEnd);
					break;
				case StmtKind.Memory:
					writer.WriteLine(memEnd);
					break;
				default:
					throw new InvalidOperationException();
				}

				switch (kind) {
				case StmtKind.Other:
					break;
				case StmtKind.Register:
					writer.WriteLine(regBegin);
					indenter = writer.Indent();
					break;
				case StmtKind.Memory:
					writer.WriteLine(memBegin);
					indenter = writer.Indent();
					break;
				default:
					throw new InvalidOperationException();
				}
				stmtKind = kind;
			}

			static StmtKind GetStmtKind(ImplAccStatementKind kind) =>
				kind switch {
					ImplAccStatementKind.MemoryAccess => StmtKind.Memory,
					ImplAccStatementKind.RegisterAccess or ImplAccStatementKind.RegisterRangeAccess => StmtKind.Register,
					_ => StmtKind.Other,
				};
		}

		protected static bool CouldBeNullSegIn64BitMode(ImplAccRegister register, out bool definitelyNullSeg) {
			definitelyNullSeg = false;
			switch (register.Kind) {
			case ImplAccRegisterKind.Register:
				switch ((Register)register.Register!.Value) {
				case Register.ES:
				case Register.CS:
				case Register.SS:
				case Register.DS:
					definitelyNullSeg = true;
					return true;
				}
				return false;
			case ImplAccRegisterKind.SegmentDefaultDS:
				return true;
			case ImplAccRegisterKind.a_rDI:
			case ImplAccRegisterKind.Op0:
			case ImplAccRegisterKind.Op1:
			case ImplAccRegisterKind.Op2:
			case ImplAccRegisterKind.Op3:
			case ImplAccRegisterKind.Op4:
				return false;
			default:
				throw new InvalidOperationException();
			}
		}

		protected static uint Verify_9_or_17(uint value) =>
			value switch {
				9 or 17 => value,
				_ => throw new InvalidOperationException(),
			};

		protected static uint Verify_2_4_or_8(uint value) =>
			value switch {
				2 or 4 or 8 => value,
				_ => throw new InvalidOperationException(),
			};

		protected static uint Verify_2_or_4(uint value) =>
			value switch {
				2 or 4 => value,
				_ => throw new InvalidOperationException(),
			};

		protected static EnumValue GetOpAccess(EnumType opAccessType, EmmiAccess access) =>
			access switch {
				EmmiAccess.Read => opAccessType[nameof(OpAccess.Read)],
				EmmiAccess.Write => opAccessType[nameof(OpAccess.Write)],
				EmmiAccess.ReadWrite => opAccessType[nameof(OpAccess.ReadWrite)],
				_ => throw new InvalidOperationException(),
			};
	}
}
