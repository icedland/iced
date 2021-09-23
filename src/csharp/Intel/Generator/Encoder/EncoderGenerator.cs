// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Linq;
using Generator.Enums;
using Generator.Enums.Decoder;
using Generator.Enums.Encoder;
using Generator.IO;
using Generator.Tables;

namespace Generator.Encoder {
	abstract class EncoderGenerator {
		protected abstract void Generate(EnumType enumType);
		protected abstract void Generate(OpCodeHandlers handlers);
		protected abstract void GenerateOpCodeInfo(InstructionDef[] defs, (MvexTupleTypeLutKind ttLutKind, EnumValue[] tupleTypes)[] mvexTupleTypeData, (MvexTupleTypeLutKind ttLutKind, EnumValue[] tupleTypes)[] mvexMemorySizeData);
		protected abstract void Generate((EnumValue value, uint size)[] immSizes);
		protected abstract void GenerateInstructionFormatter((EnumValue code, string result)[] notInstrStrings);
		protected abstract void GenerateOpCodeFormatter((EnumValue code, string result)[] notInstrStrings, EnumValue[] hasModRM, EnumValue[] hasVsib);
		protected abstract void GenerateCore();
		protected abstract void GenerateInstrSwitch(EnumValue[] jccInstr, EnumValue[] simpleBranchInstr, EnumValue[] callInstr, EnumValue[] jmpInstr, EnumValue[] xbeginInstr);
		protected abstract void GenerateVsib(EnumValue[] vsib32, EnumValue[] vsib64);
		protected abstract void GenerateDecoderOptionsTable((EnumValue decOptionValue, EnumValue decoderOptions)[] values);
		protected abstract void GenerateImpliedOps((EncodingKind Encoding, InstrStrImpliedOp[] Ops, InstructionDef[] defs)[] impliedOpsInfo);

		protected readonly struct OpCodeHandlers {
			public readonly (EnumValue opCodeOperandKind, OpHandlerKind opHandlerKind, object[] args)[] Legacy;
			public readonly (EnumValue opCodeOperandKind, OpHandlerKind opHandlerKind, object[] args)[] Vex;
			public readonly (EnumValue opCodeOperandKind, OpHandlerKind opHandlerKind, object[] args)[] Xop;
			public readonly (EnumValue opCodeOperandKind, OpHandlerKind opHandlerKind, object[] args)[] Evex;
			public readonly (EnumValue opCodeOperandKind, OpHandlerKind opHandlerKind, object[] args)[] Mvex;

			public OpCodeHandlers((EnumValue opCodeOperandKind, OpHandlerKind opHandlerKind, object[] args)[] legacy,
				(EnumValue opCodeOperandKind, OpHandlerKind opHandlerKind, object[] args)[] vex,
				(EnumValue opCodeOperandKind, OpHandlerKind opHandlerKind, object[] args)[] xop,
				(EnumValue opCodeOperandKind, OpHandlerKind opHandlerKind, object[] args)[] evex,
				(EnumValue opCodeOperandKind, OpHandlerKind opHandlerKind, object[] args)[] mvex) {
				Legacy = legacy;
				Vex = vex;
				Xop = xop;
				Evex = evex;
				Mvex = mvex;
			}
		}

		protected readonly GenTypes genTypes;
		readonly EncoderTypes encoderTypes;

		protected EncoderGenerator(GenTypes genTypes) {
			this.genTypes = genTypes;
			encoderTypes = genTypes.GetObject<EncoderTypes>(TypeIds.EncoderTypes);
		}

		readonly struct ImpliedOpsKey : IEquatable<ImpliedOpsKey> {
			readonly InstructionDef def;

			public EncodingKind Encoding => def.Encoding;
			public InstrStrImpliedOp[] Ops => def.InstrStrImpliedOps;

			public ImpliedOpsKey(InstructionDef def) => this.def = def;

			public bool Equals(ImpliedOpsKey other) {
				if (def.Encoding != other.def.Encoding)
					return false;
				var a = def.InstrStrImpliedOps;
				var b = other.def.InstrStrImpliedOps;
				if (a.Length != b.Length)
					return false;
				for (int i = 0; i < a.Length; i++) {
					if (a[i].IsUpper != b[i].IsUpper)
						return false;
					if (!StringComparer.OrdinalIgnoreCase.Equals(a[i].Operand, b[i].Operand))
						return false;
				}
				return true;
			}

			public override bool Equals(object? obj) => obj is ImpliedOpsKey other && Equals(other);

			public override int GetHashCode() {
				int hc = HashCode.Combine(def.Encoding);
				foreach (var op in def.InstrStrImpliedOps)
					hc = HashCode.Combine(hc, StringComparer.OrdinalIgnoreCase.GetHashCode(op.Operand), op.IsUpper);
				return hc;
			}
		}

		public void Generate() {
			var enumTypes = new EnumType[] {
				genTypes[TypeIds.EncFlags1],
			};
			foreach (var enumType in enumTypes)
				Generate(enumType);

			Generate(new OpCodeHandlers(encoderTypes.LegacyOpHandlers, encoderTypes.VexOpHandlers, encoderTypes.XopOpHandlers,
				encoderTypes.EvexOpHandlers, encoderTypes.MvexOpHandlers));
			var defs = genTypes.GetObject<InstructionDefs>(TypeIds.InstructionDefs).Defs;
			var impliedOpsInfo = defs.Where(a => a.InstrStrImpliedOps.Length > 0).
				GroupBy(a => new ImpliedOpsKey(a), (a, b) => (a.Encoding, a.Ops, b.OrderBy(a => a.Code.Value).ToArray())).ToArray();
			GenerateImpliedOps(impliedOpsInfo);
			var tupleTypeType = genTypes[TypeIds.TupleType];
			var memorySizeType = genTypes[TypeIds.MemorySize];
			var mvexLutData = MvexLutData.GetLutData();
			var mvexTupleTypeData = mvexLutData.Select(x => (x.ttLutKind, x.data.Select(x => tupleTypeType[x.TupleType.ToString()]).ToArray())).ToArray();
			var mvexMemorySizeData = mvexLutData.Select(x => (x.ttLutKind, x.data.Select(x => memorySizeType[x.MemorySize.ToString()]).ToArray())).ToArray();
			GenerateOpCodeInfo(defs, mvexTupleTypeData, mvexMemorySizeData);
			Generate(encoderTypes.ImmSizes);
			var notInstrOpCodeStrs = defs.Where(a => (a.Flags1 & InstructionDefFlags1.NoInstruction) != 0).Select(a => (a.Code, a.OpCodeString)).ToArray();
			var notInstrInstrStrs = defs.Where(a => (a.Flags1 & InstructionDefFlags1.NoInstruction) != 0).Select(a => (a.Code, a.InstructionString)).ToArray();
			GenerateInstructionFormatter(notInstrInstrStrs);
			var opCodeOperandKind = genTypes[TypeIds.OpCodeOperandKind];
			var opDefs = genTypes.GetObject<OpCodeOperandKindDefs>(TypeIds.OpCodeOperandKindDefs).Defs;
			var hasModRM = opDefs.Where(a => a.Modrm).Select(a => a.EnumValue).ToArray();
			var hasVsib = opDefs.Where(a => a.Vsib).Select(a => a.EnumValue).ToArray();
			GenerateOpCodeFormatter(notInstrOpCodeStrs, hasModRM, hasVsib);
			GenerateCore();
			var jccInstr = defs.Where(a =>
				a.BranchKind == BranchKind.JccShort || a.BranchKind == BranchKind.JccNear ||
				a.BranchKind == BranchKind.JkccShort || a.BranchKind == BranchKind.JkccNear).Select(a => a.Code).OrderBy(a => a.Value).ToArray();
			var simpleBranchInstr = defs.Where(a => a.BranchKind == BranchKind.Loop || a.BranchKind == BranchKind.Jrcxz).Select(a => a.Code).OrderBy(a => a.Value).ToArray();
			var callInstr = defs.Where(a => a.BranchKind == BranchKind.CallNear).Select(a => a.Code).OrderBy(a => a.Value).ToArray();
			var jmpInstr = defs.Where(a => a.BranchKind == BranchKind.JmpShort || a.BranchKind == BranchKind.JmpNear).Select(a => a.Code).OrderBy(a => a.Value).ToArray();
			var xbeginInstr = defs.Where(a => a.BranchKind == BranchKind.Xbegin).Select(a => a.Code).OrderBy(a => a.Value).ToArray();
			GenerateInstrSwitch(jccInstr, simpleBranchInstr, callInstr, jmpInstr, xbeginInstr);
			var vsib32 = defs.Where(a => a.OpKindDefs.Any(b => b.Vsib32)).Select(a => a.Code).ToArray();
			var vsib64 = defs.Where(a => a.OpKindDefs.Any(b => b.Vsib64)).Select(a => a.Code).ToArray();
			GenerateVsib(vsib32, vsib64);

			var decoderOptionsType = genTypes[TypeIds.DecoderOptions];
			var decOptionValueType = genTypes[TypeIds.DecOptionValue];
			var decOptValues = decOptionValueType.Values.Select(a => (decOptionValue: a, decoderOptions: decoderOptionsType[a.RawName])).ToArray();
			GenerateDecoderOptionsTable(decOptValues);
		}

		protected struct MvexEncInfo {
			public EnumValue TupleTypeLutKind;
			public EnumValue EHBit;
			public EnumValue ConvFn;
			public byte InvalidConvFns;
			public byte InvalidSwizzleFns;
			public MvexInfoFlags1 Flags1;
			public MvexInfoFlags2 Flags2;
		}

		protected IEnumerable<(InstructionDef def, uint encFlags1, uint encFlags2, uint encFlags3, uint opcFlags1, uint opcFlags2, MvexEncInfo? mvex)> GetData(InstructionDef[] defs) {
			var encFlags1Type = genTypes[TypeIds.EncFlags1];
			var ignoreRoundingControl = encFlags1Type[nameof(InstructionDefFlags3.IgnoresRoundingControl)];
			var amdLockRegBit = encFlags1Type[nameof(InstructionDefFlags3.AmdLockRegBit)];

			var legacyOpShifts = new[] {
				(int)encFlags1Type["Legacy_Op0Shift"].Value,
				(int)encFlags1Type["Legacy_Op1Shift"].Value,
				(int)encFlags1Type["Legacy_Op2Shift"].Value,
				(int)encFlags1Type["Legacy_Op3Shift"].Value,
			};
			var vexOpShifts = new[] {
				(int)encFlags1Type["VEX_Op0Shift"].Value,
				(int)encFlags1Type["VEX_Op1Shift"].Value,
				(int)encFlags1Type["VEX_Op2Shift"].Value,
				(int)encFlags1Type["VEX_Op3Shift"].Value,
				(int)encFlags1Type["VEX_Op4Shift"].Value,
			};
			var xopOpShifts = new[] {
				(int)encFlags1Type["XOP_Op0Shift"].Value,
				(int)encFlags1Type["XOP_Op1Shift"].Value,
				(int)encFlags1Type["XOP_Op2Shift"].Value,
				(int)encFlags1Type["XOP_Op3Shift"].Value,
			};
			var evexOpShifts = new[] {
				(int)encFlags1Type["EVEX_Op0Shift"].Value,
				(int)encFlags1Type["EVEX_Op1Shift"].Value,
				(int)encFlags1Type["EVEX_Op2Shift"].Value,
				(int)encFlags1Type["EVEX_Op3Shift"].Value,
			};
			var mvexOpShifts = new[] {
				(int)encFlags1Type["MVEX_Op0Shift"].Value,
				(int)encFlags1Type["MVEX_Op1Shift"].Value,
				(int)encFlags1Type["MVEX_Op2Shift"].Value,
				(int)encFlags1Type["MVEX_Op3Shift"].Value,
			};

			var mvexConvFnType = genTypes[TypeIds.MvexConvFn];
			var ehBitType = genTypes[TypeIds.MvexEHBit];
			foreach (var def in defs) {
				uint encFlags1 = 0;

				if ((def.Flags3 & InstructionDefFlags3.IgnoresRoundingControl) != 0)
					encFlags1 |= ignoreRoundingControl.Value;
				if ((def.Flags3 & InstructionDefFlags3.AmdLockRegBit) != 0)
					encFlags1 |= amdLockRegBit.Value;

				var encFlags2 = EncFlags2.None;
				var encFlags3 = EncFlags3.None;
				var opcFlags1 = OpCodeInfoFlags1.None;
				encFlags2 |= (EncFlags2)(def.OpCode << (int)EncFlags2.OpCodeShift);
				switch (def.OpCodeLength) {
				case 1:
					if (def.OpCode > 0xFF)
						throw new InvalidOperationException();
					break;
				case 2:
					if (def.OpCode > 0xFFFF)
						throw new InvalidOperationException();
					encFlags2 |= EncFlags2.OpCodeIs2Bytes;
					break;
				default:
					throw new InvalidOperationException();
				}

				var mpByte = GetMandatoryPrefixByte(def.MandatoryPrefix);
				if ((uint)mpByte > (uint)EncFlags2.MandatoryPrefixMask)
					throw new InvalidOperationException();
				encFlags2 |= (EncFlags2)((uint)mpByte << (int)EncFlags2.MandatoryPrefixShift);
				if (def.MandatoryPrefix != MandatoryPrefix.None)
					encFlags2 |= EncFlags2.HasMandatoryPrefix;

				var lbit = GetLBit(def);
				if ((uint)lbit > (uint)EncFlags2.LBitMask)
					throw new InvalidOperationException();
				encFlags2 |= (EncFlags2)((uint)lbit << (int)EncFlags2.LBitShift);

				var wbit = GetWBit(def);
				if ((uint)wbit > (uint)EncFlags2.WBitMask)
					throw new InvalidOperationException();
				encFlags2 |= (EncFlags2)((uint)wbit << (int)EncFlags2.WBitShift);

				if (def.GroupIndex >= 0 && def.RmGroupIndex >= 0)
					throw new InvalidOperationException();
				if (def.GroupIndex >= 0) {
					if ((uint)def.GroupIndex > (uint)EncFlags2.GroupIndexMask)
						throw new InvalidOperationException();
					encFlags2 |= EncFlags2.HasGroupIndex;
					encFlags2 |= (EncFlags2)((uint)def.GroupIndex << (int)EncFlags2.GroupIndexShift);
				}
				else if (def.RmGroupIndex >= 0) {
					if ((uint)def.RmGroupIndex > (uint)EncFlags2.GroupIndexMask)
						throw new InvalidOperationException();
					encFlags3 |= EncFlags3.HasRmGroupIndex;
					encFlags2 |= (EncFlags2)((uint)def.RmGroupIndex << (int)EncFlags2.GroupIndexShift);
				}

				if ((uint)def.Encoding > (uint)EncFlags3.EncodingMask)
					throw new InvalidOperationException();
				encFlags3 |= (EncFlags3)((uint)def.Encoding << (int)EncFlags3.EncodingShift);

				if ((uint)def.OperandSize > (uint)EncFlags3.OperandSizeMask)
					throw new InvalidOperationException();
				encFlags3 |= (EncFlags3)((uint)def.OperandSize << (int)EncFlags3.OperandSizeShift);

				if ((uint)def.AddressSize > (uint)EncFlags3.AddressSizeMask)
					throw new InvalidOperationException();
				encFlags3 |= (EncFlags3)((uint)def.AddressSize << (int)EncFlags3.AddressSizeShift);

				if ((uint)def.TupleType > (uint)EncFlags3.TupleTypeMask)
					throw new InvalidOperationException();
				encFlags3 |= (EncFlags3)((uint)def.TupleType << (int)EncFlags3.TupleTypeShift);

				if ((def.Flags3 & InstructionDefFlags3.DefaultOpSize64) != 0) encFlags3 |= EncFlags3.DefaultOpSize64;
				if ((def.Flags3 & InstructionDefFlags3.ForceOpSize64) != 0) opcFlags1 |= OpCodeInfoFlags1.ForceOpSize64;
				if ((def.Flags3 & InstructionDefFlags3.IntelForceOpSize64) != 0) encFlags3 |= EncFlags3.IntelForceOpSize64;
				if ((def.Flags1 & InstructionDefFlags1.Fwait) != 0) encFlags3 |= EncFlags3.Fwait;
				if ((def.Flags1 & (InstructionDefFlags1.Bit16 | InstructionDefFlags1.Bit32)) != 0) encFlags3 |= EncFlags3.Bit16or32;
				if ((def.Flags1 & InstructionDefFlags1.Bit64) != 0) encFlags3 |= EncFlags3.Bit64;
				if ((def.Flags1 & InstructionDefFlags1.Lock) != 0) encFlags3 |= EncFlags3.Lock;
				if ((def.Flags1 & InstructionDefFlags1.Xacquire) != 0) encFlags3 |= EncFlags3.Xacquire;
				if ((def.Flags1 & InstructionDefFlags1.Xrelease) != 0) encFlags3 |= EncFlags3.Xrelease;
				if ((def.Flags1 & InstructionDefFlags1.Rep) != 0) encFlags3 |= EncFlags3.Rep;
				if ((def.Flags1 & InstructionDefFlags1.Repne) != 0) encFlags3 |= EncFlags3.Repne;
				if ((def.Flags1 & InstructionDefFlags1.Bnd) != 0) encFlags3 |= EncFlags3.Bnd;
				if ((def.Flags1 & InstructionDefFlags1.HintTaken) != 0) encFlags3 |= EncFlags3.HintTaken;
				if ((def.Flags1 & InstructionDefFlags1.Notrack) != 0) encFlags3 |= EncFlags3.Notrack;
				if ((def.Flags1 & InstructionDefFlags1.Broadcast) != 0) encFlags3 |= EncFlags3.Broadcast;
				if ((def.Flags1 & InstructionDefFlags1.RoundingControl) != 0) encFlags3 |= EncFlags3.RoundingControl;
				if ((def.Flags1 & InstructionDefFlags1.SuppressAllExceptions) != 0) encFlags3 |= EncFlags3.SuppressAllExceptions;
				if ((def.Flags1 & InstructionDefFlags1.OpMaskRegister) != 0) encFlags3 |= EncFlags3.OpMaskRegister;
				if ((def.Flags1 & InstructionDefFlags1.ZeroingMasking) != 0) encFlags3 |= EncFlags3.ZeroingMasking;
				if ((def.Flags1 & InstructionDefFlags1.RequireOpMaskRegister) != 0) encFlags3 |= EncFlags3.RequireOpMaskRegister;

				const InstructionDefFlags1 CplBits =
					InstructionDefFlags1.Cpl0 | InstructionDefFlags1.Cpl1 | InstructionDefFlags1.Cpl2 | InstructionDefFlags1.Cpl3;
				opcFlags1 |= (def.Flags1 & CplBits) switch {
					InstructionDefFlags1.Cpl0 => OpCodeInfoFlags1.Cpl0Only,
					InstructionDefFlags1.Cpl3 => OpCodeInfoFlags1.Cpl3Only,
					CplBits => OpCodeInfoFlags1.None,
					_ => throw new InvalidOperationException(),
				};
				if ((def.Flags3 & InstructionDefFlags3.InputOutput) != 0) opcFlags1 |= OpCodeInfoFlags1.InputOutput;
				if ((def.Flags3 & InstructionDefFlags3.Nop) != 0) opcFlags1 |= OpCodeInfoFlags1.Nop;
				if ((def.Flags3 & InstructionDefFlags3.ReservedNop) != 0) opcFlags1 |= OpCodeInfoFlags1.ReservedNop;
				if ((def.Flags3 & InstructionDefFlags3.SerializingIntel) != 0) opcFlags1 |= OpCodeInfoFlags1.SerializingIntel;
				if ((def.Flags3 & InstructionDefFlags3.SerializingAmd) != 0) opcFlags1 |= OpCodeInfoFlags1.SerializingAmd;
				if ((def.Flags3 & InstructionDefFlags3.MayRequireCpl0) != 0) opcFlags1 |= OpCodeInfoFlags1.MayRequireCpl0;
				if ((def.Flags3 & InstructionDefFlags3.CetTracked) != 0) opcFlags1 |= OpCodeInfoFlags1.CetTracked;
				if ((def.Flags3 & InstructionDefFlags3.NonTemporal) != 0) opcFlags1 |= OpCodeInfoFlags1.NonTemporal;
				if ((def.Flags3 & InstructionDefFlags3.FpuNoWait) != 0) opcFlags1 |= OpCodeInfoFlags1.FpuNoWait;
				if ((def.Flags1 & InstructionDefFlags1.IgnoresModBits) != 0) opcFlags1 |= OpCodeInfoFlags1.IgnoresModBits;
				if ((def.Flags1 & InstructionDefFlags1.No66) != 0) opcFlags1 |= OpCodeInfoFlags1.No66;
				if ((def.Flags1 & InstructionDefFlags1.NFx) != 0) opcFlags1 |= OpCodeInfoFlags1.NFx;
				if ((def.Flags1 & InstructionDefFlags1.RequiresUniqueRegNums) != 0) opcFlags1 |= OpCodeInfoFlags1.RequiresUniqueRegNums;
				if ((def.Flags3 & InstructionDefFlags3.RequiresUniqueDestRegNum) != 0) opcFlags1 |= OpCodeInfoFlags1.RequiresUniqueDestRegNum;
				if ((def.Flags3 & InstructionDefFlags3.Privileged) != 0) opcFlags1 |= OpCodeInfoFlags1.Privileged;
				if ((def.Flags1 & InstructionDefFlags1.SaveRestore) != 0) opcFlags1 |= OpCodeInfoFlags1.SaveRestore;
				if ((def.Flags1 & InstructionDefFlags1.StackInstruction) != 0) opcFlags1 |= OpCodeInfoFlags1.StackInstruction;
				if ((def.Flags1 & InstructionDefFlags1.IgnoresSegment) != 0) opcFlags1 |= OpCodeInfoFlags1.IgnoresSegment;
				if ((def.Flags1 & InstructionDefFlags1.OpMaskReadWrite) != 0) opcFlags1 |= OpCodeInfoFlags1.OpMaskReadWrite;
				if ((def.InstrStrFlags & InstructionStringFlags.ModRegRmString) != 0) opcFlags1 |= OpCodeInfoFlags1.ModRegRmString;

				if (def.DecoderOption.DeclaringType.TypeId != TypeIds.DecOptionValue)
					throw new InvalidOperationException();
				if (def.DecoderOption.Value > (uint)OpCodeInfoFlags1.DecOptionValueMask)
					throw new InvalidOperationException();
				opcFlags1 |= (OpCodeInfoFlags1)(def.DecoderOption.Value << (int)OpCodeInfoFlags1.DecOptionValueShift);

				var opcFlags2 = OpCodeInfoFlags2.None;
				if ((def.Flags2 & InstructionDefFlags2.RealMode) != 0) opcFlags2 |= OpCodeInfoFlags2.RealMode;
				if ((def.Flags2 & InstructionDefFlags2.ProtectedMode) != 0) opcFlags2 |= OpCodeInfoFlags2.ProtectedMode;
				if ((def.Flags2 & InstructionDefFlags2.Virtual8086Mode) != 0) opcFlags2 |= OpCodeInfoFlags2.Virtual8086Mode;
				if ((def.Flags2 & InstructionDefFlags2.CompatibilityMode) != 0) opcFlags2 |= OpCodeInfoFlags2.CompatibilityMode;
				if ((def.Flags2 & InstructionDefFlags2.UseOutsideSmm) != 0) opcFlags2 |= OpCodeInfoFlags2.UseOutsideSmm;
				if ((def.Flags2 & InstructionDefFlags2.UseInSmm) != 0) opcFlags2 |= OpCodeInfoFlags2.UseInSmm;
				if ((def.Flags2 & InstructionDefFlags2.UseOutsideEnclaveSgx) != 0) opcFlags2 |= OpCodeInfoFlags2.UseOutsideEnclaveSgx;
				if ((def.Flags2 & InstructionDefFlags2.UseInEnclaveSgx1) != 0) opcFlags2 |= OpCodeInfoFlags2.UseInEnclaveSgx1;
				if ((def.Flags2 & InstructionDefFlags2.UseInEnclaveSgx2) != 0) opcFlags2 |= OpCodeInfoFlags2.UseInEnclaveSgx2;
				if ((def.Flags2 & InstructionDefFlags2.UseOutsideVmxOp) != 0) opcFlags2 |= OpCodeInfoFlags2.UseOutsideVmxOp;
				if ((def.Flags2 & InstructionDefFlags2.UseInVmxRootOp) != 0) opcFlags2 |= OpCodeInfoFlags2.UseInVmxRootOp;
				if ((def.Flags2 & InstructionDefFlags2.UseInVmxNonRootOp) != 0) opcFlags2 |= OpCodeInfoFlags2.UseInVmxNonRootOp;
				if ((def.Flags2 & InstructionDefFlags2.UseOutsideSeam) != 0) opcFlags2 |= OpCodeInfoFlags2.UseOutsideSeam;
				if ((def.Flags2 & InstructionDefFlags2.UseInSeam) != 0) opcFlags2 |= OpCodeInfoFlags2.UseInSeam;
				if ((def.Flags2 & InstructionDefFlags2.TdxNonRootGenUd) != 0) opcFlags2 |= OpCodeInfoFlags2.TdxNonRootGenUd;
				if ((def.Flags2 & InstructionDefFlags2.TdxNonRootGenVe) != 0) opcFlags2 |= OpCodeInfoFlags2.TdxNonRootGenVe;
				if ((def.Flags2 & InstructionDefFlags2.TdxNonRootMayGenEx) != 0) opcFlags2 |= OpCodeInfoFlags2.TdxNonRootMayGenEx;
				if ((def.Flags2 & InstructionDefFlags2.IntelVmExit) != 0) opcFlags2 |= OpCodeInfoFlags2.IntelVmExit;
				if ((def.Flags2 & InstructionDefFlags2.IntelMayVmExit) != 0) opcFlags2 |= OpCodeInfoFlags2.IntelMayVmExit;
				if ((def.Flags2 & InstructionDefFlags2.IntelSmmVmExit) != 0) opcFlags2 |= OpCodeInfoFlags2.IntelSmmVmExit;
				if ((def.Flags2 & InstructionDefFlags2.AmdVmExit) != 0) opcFlags2 |= OpCodeInfoFlags2.AmdVmExit;
				if ((def.Flags2 & InstructionDefFlags2.AmdMayVmExit) != 0) opcFlags2 |= OpCodeInfoFlags2.AmdMayVmExit;
				if ((def.Flags2 & InstructionDefFlags2.TsxAbort) != 0) opcFlags2 |= OpCodeInfoFlags2.TsxAbort;
				if ((def.Flags2 & InstructionDefFlags2.TsxImplAbort) != 0) opcFlags2 |= OpCodeInfoFlags2.TsxImplAbort;
				if ((def.Flags2 & InstructionDefFlags2.TsxMayAbort) != 0) opcFlags2 |= OpCodeInfoFlags2.TsxMayAbort;
				if ((def.Flags2 & (InstructionDefFlags2.IntelDecoder16 | InstructionDefFlags2.IntelDecoder32)) != 0) opcFlags2 |= OpCodeInfoFlags2.IntelDecoder16or32;
				if ((def.Flags2 & InstructionDefFlags2.IntelDecoder64) != 0) opcFlags2 |= OpCodeInfoFlags2.IntelDecoder64;
				if ((def.Flags2 & (InstructionDefFlags2.AmdDecoder16 | InstructionDefFlags2.AmdDecoder32)) != 0) opcFlags2 |= OpCodeInfoFlags2.AmdDecoder16or32;
				if ((def.Flags2 & InstructionDefFlags2.AmdDecoder64) != 0) opcFlags2 |= OpCodeInfoFlags2.AmdDecoder64;

				if ((uint)def.InstrStrFmtOption > (uint)OpCodeInfoFlags2.InstrStrFmtOptionMask)
					throw new InvalidOperationException();
				opcFlags2 |= (OpCodeInfoFlags2)((uint)def.InstrStrFmtOption << (int)OpCodeInfoFlags2.InstrStrFmtOptionShift);

				uint tableIndex;
				switch (def.Encoding) {
				case EncodingKind.Legacy:
					for (int i = 0; i < def.OpKindDefs.Length; i++)
						encFlags1 |= encoderTypes.ToLegacy(def.OpKindDefs[i]) << legacyOpShifts[i];
					tableIndex = (uint)GetLegacyTable(def.Table);
					break;

				case EncodingKind.VEX:
					for (int i = 0; i < def.OpKindDefs.Length; i++)
						encFlags1 |= encoderTypes.ToVex(def.OpKindDefs[i]) << vexOpShifts[i];
					tableIndex = (uint)GetVexTable(def.Table);
					break;

				case EncodingKind.EVEX:
					for (int i = 0; i < def.OpKindDefs.Length; i++)
						encFlags1 |= encoderTypes.ToEvex(def.OpKindDefs[i]) << evexOpShifts[i];
					tableIndex = (uint)GetEvexTable(def.Table);
					break;

				case EncodingKind.XOP:
					for (int i = 0; i < def.OpKindDefs.Length; i++)
						encFlags1 |= encoderTypes.ToXop(def.OpKindDefs[i]) << xopOpShifts[i];
					tableIndex = (uint)GetXopTable(def.Table);
					break;

				case EncodingKind.D3NOW:
					tableIndex = 0;
					break;

				case EncodingKind.MVEX:
					for (int i = 0; i < def.OpKindDefs.Length; i++)
						encFlags1 |= encoderTypes.ToMvex(def.OpKindDefs[i]) << mvexOpShifts[i];
					tableIndex = (uint)GetMvexTable(def.Table);
					break;

				default:
					throw new InvalidOperationException();
				}
				if (tableIndex > (uint)EncFlags2.TableMask)
					throw new InvalidOperationException();
				encFlags2 |= (EncFlags2)(tableIndex << (int)EncFlags2.TableShift);

				MvexEncInfo? mvex = null;
				if (def.Encoding == EncodingKind.MVEX) {
					var mvexFlags1 = def.Mvex.Flags1;
					var mvexFlags2 = def.Mvex.Flags2;
					switch (def.NDKind) {
					case NonDestructiveOpKind.None: break;
					case NonDestructiveOpKind.NDD: mvexFlags1 |= MvexInfoFlags1.NDD; break;
					case NonDestructiveOpKind.NDS: mvexFlags1 |= MvexInfoFlags1.NDS; break;
					default: throw new InvalidOperationException();
					}
					if (def.Mvex.TupleTypeLutKind.Value > byte.MaxValue) throw new InvalidOperationException();
					if ((uint)def.Mvex.EHBit > byte.MaxValue) throw new InvalidOperationException();
					if ((uint)def.Mvex.ConvFn > byte.MaxValue) throw new InvalidOperationException();
					if ((uint)mvexFlags1 > byte.MaxValue) throw new InvalidOperationException();
					if ((uint)mvexFlags2 > byte.MaxValue) throw new InvalidOperationException();
					mvex = new MvexEncInfo {
						TupleTypeLutKind = def.Mvex.TupleTypeLutKind,
						EHBit = ehBitType[def.Mvex.EHBit.ToString()],
						ConvFn = mvexConvFnType[def.Mvex.ConvFn.ToString()],
						InvalidConvFns = (byte)~def.Mvex.ValidConvFns,
						InvalidSwizzleFns = (byte)~def.Mvex.ValidSwizzleFns,
						Flags1 = mvexFlags1,
						Flags2 = mvexFlags2,
					};
				}

				yield return (def, encFlags1, (uint)encFlags2, (uint)encFlags3, (uint)opcFlags1, (uint)opcFlags2, mvex);
			}
		}

		static MandatoryPrefixByte GetMandatoryPrefixByte(MandatoryPrefix mandatoryPrefix) =>
			mandatoryPrefix switch {
				MandatoryPrefix.None => MandatoryPrefixByte.None,
				MandatoryPrefix.PNP => MandatoryPrefixByte.None,
				MandatoryPrefix.P66 => MandatoryPrefixByte.P66,
				MandatoryPrefix.PF3 => MandatoryPrefixByte.PF3,
				MandatoryPrefix.PF2 => MandatoryPrefixByte.PF2,
				_ => throw new InvalidOperationException(),
			};

		static LegacyOpCodeTable GetLegacyTable(OpCodeTableKind table) =>
			table switch {
				OpCodeTableKind.Normal => LegacyOpCodeTable.MAP0,
				OpCodeTableKind.T0F => LegacyOpCodeTable.MAP0F,
				OpCodeTableKind.T0F38 => LegacyOpCodeTable.MAP0F38,
				OpCodeTableKind.T0F3A => LegacyOpCodeTable.MAP0F3A,
				_ => throw new InvalidOperationException(),
			};

		static VexOpCodeTable GetVexTable(OpCodeTableKind table) =>
			table switch {
				OpCodeTableKind.Normal => VexOpCodeTable.MAP0,
				OpCodeTableKind.T0F => VexOpCodeTable.MAP0F,
				OpCodeTableKind.T0F38 => VexOpCodeTable.MAP0F38,
				OpCodeTableKind.T0F3A => VexOpCodeTable.MAP0F3A,
				_ => throw new InvalidOperationException(),
			};

		static EvexOpCodeTable GetEvexTable(OpCodeTableKind table) =>
			table switch {
				OpCodeTableKind.T0F => EvexOpCodeTable.MAP0F,
				OpCodeTableKind.T0F38 => EvexOpCodeTable.MAP0F38,
				OpCodeTableKind.T0F3A => EvexOpCodeTable.MAP0F3A,
				OpCodeTableKind.MAP5 => EvexOpCodeTable.MAP5,
				OpCodeTableKind.MAP6 => EvexOpCodeTable.MAP6,
				_ => throw new InvalidOperationException(),
			};

		static XopOpCodeTable GetXopTable(OpCodeTableKind table) =>
			table switch {
				OpCodeTableKind.MAP8 => XopOpCodeTable.MAP8,
				OpCodeTableKind.MAP9 => XopOpCodeTable.MAP9,
				OpCodeTableKind.MAP10 => XopOpCodeTable.MAP10,
				_ => throw new InvalidOperationException(),
			};

		static MvexOpCodeTable GetMvexTable(OpCodeTableKind table) =>
			table switch {
				OpCodeTableKind.T0F => MvexOpCodeTable.MAP0F,
				OpCodeTableKind.T0F38 => MvexOpCodeTable.MAP0F38,
				OpCodeTableKind.T0F3A => MvexOpCodeTable.MAP0F3A,
				_ => throw new InvalidOperationException(),
			};

		static LBit GetLBit(InstructionDef def) =>
			def.LBit switch {
				OpCodeL.None => LBit.L0,
				OpCodeL.L0 => LBit.L0,
				OpCodeL.L1 => LBit.L1,
				OpCodeL.LIG => LBit.LIG,
				OpCodeL.LZ => LBit.LZ,
				OpCodeL.L128 => LBit.L128,
				OpCodeL.L256 => LBit.L256,
				OpCodeL.L512 => LBit.L512,
				_ => throw new InvalidOperationException(),
			};

		static WBit GetWBit(InstructionDef def) {
			if ((def.Flags1 & InstructionDefFlags1.WIG32) != 0)
				return WBit.WIG32;
			return def.WBit switch {
				OpCodeW.None => WBit.W0,
				OpCodeW.W0 => WBit.W0,
				OpCodeW.W1 => WBit.W1,
				OpCodeW.WIG => WBit.WIG,
				OpCodeW.WIG32 => WBit.WIG32,
				_ => throw new InvalidOperationException(),
			};
		}

		protected void WriteFlags(FileWriter writer, IdentifierConverter idConverter, InstructionDefFlags1 prefixes, (EnumValue value, InstructionDefFlags1 flag)[] flagsInfos, string orSep, string enumItemSep, bool forceConstant) {
			bool printed = false;
			foreach (var info in flagsInfos) {
				if ((prefixes & info.flag) != 0) {
					prefixes &= ~info.flag;
					if (printed)
						writer.Write(orSep);
					printed = true;
					WriteEnum(writer, idConverter, info.value, enumItemSep, forceConstant);
				}
			}
			if (!printed) {
				var value = genTypes[TypeIds.EncFlags2][nameof(EncFlags2.None)];
				WriteEnum(writer, idConverter, value, enumItemSep, forceConstant);
			}
			if (prefixes != 0)
				throw new InvalidOperationException();

			static void WriteEnum(FileWriter writer, IdentifierConverter idConverter, EnumValue value, string enumItemSep, bool forceConstant) {
				var name = forceConstant ? idConverter.Constant(value.RawName) : value.Name(idConverter);
				writer.Write($"{value.DeclaringType.Name(idConverter)}{enumItemSep}{name}");
			}
		}
	}
}
