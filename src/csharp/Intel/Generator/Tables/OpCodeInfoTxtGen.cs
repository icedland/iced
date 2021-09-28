// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Linq;
using Generator.Constants.Encoder;
using Generator.Enums;
using Generator.Enums.Encoder;
using Generator.IO;

namespace Generator.Tables {
	[Generator(TargetLanguage.Other)]
	sealed class OpCodeInfoTxtGen {
		readonly GenTypes genTypes;

		public OpCodeInfoTxtGen(GeneratorContext generatorContext) =>
			genTypes = generatorContext.Types;

		public void Generate() {
			var filename = genTypes.Dirs.GetUnitTestFilename("Encoder", "OpCodeInfos.txt");
			using (var writer = new FileWriter(TargetLanguage.Other, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();
				foreach (var def in genTypes.GetObject<InstructionDefs>(TypeIds.InstructionDefs).Defs)
					Write(writer, def);
			}
		}

		static void Write(FileWriter writer, InstructionDef def) {
			const string sepNoSpace = ",";
			const string sep = sepNoSpace + " ";

			writer.Write(def.Code.RawName);

			writer.Write(sep);
			writer.Write(def.Mnemonic.RawName);

			writer.Write(sep);
			writer.Write(def.Memory.RawName);

			writer.Write(sep);
			writer.Write(def.MemoryBroadcast.RawName);

			var encoding = def.Encoding switch {
				EncodingKind.Legacy => OpCodeInfoConstants.Encoding_Legacy,
				EncodingKind.VEX => OpCodeInfoConstants.Encoding_VEX,
				EncodingKind.EVEX => OpCodeInfoConstants.Encoding_EVEX,
				EncodingKind.XOP => OpCodeInfoConstants.Encoding_XOP,
				EncodingKind.D3NOW => OpCodeInfoConstants.Encoding_3DNOW,
				EncodingKind.MVEX => OpCodeInfoConstants.Encoding_MVEX,
				_ => throw new InvalidOperationException(),
			};
			writer.Write(sep);
			writer.Write(encoding);

			var mp = def.MandatoryPrefix switch {
				MandatoryPrefix.None => OpCodeInfoConstants.MandatoryPrefix_None,
				MandatoryPrefix.PNP => OpCodeInfoConstants.MandatoryPrefix_NP,
				MandatoryPrefix.P66 => OpCodeInfoConstants.MandatoryPrefix_66,
				MandatoryPrefix.PF3 => OpCodeInfoConstants.MandatoryPrefix_F3,
				MandatoryPrefix.PF2 => OpCodeInfoConstants.MandatoryPrefix_F2,
				_ => throw new InvalidOperationException(),
			};
			writer.Write(sep);
			writer.Write(mp);

			var table = def.Table switch {
				OpCodeTableKind.Normal => OpCodeInfoConstants.Table_Legacy,
				OpCodeTableKind.T0F => OpCodeInfoConstants.Table_0F,
				OpCodeTableKind.T0F38 => OpCodeInfoConstants.Table_0F38,
				OpCodeTableKind.T0F3A => OpCodeInfoConstants.Table_0F3A,
				OpCodeTableKind.MAP5 => OpCodeInfoConstants.Table_MAP5,
				OpCodeTableKind.MAP6 => OpCodeInfoConstants.Table_MAP6,
				OpCodeTableKind.MAP8 => OpCodeInfoConstants.Table_MAP8,
				OpCodeTableKind.MAP9 => OpCodeInfoConstants.Table_MAP9,
				OpCodeTableKind.MAP10 => OpCodeInfoConstants.Table_MAP10,
				_ => throw new InvalidOperationException(),
			};
			writer.Write(sep);
			writer.Write(table);

			writer.Write(sep);
			switch (def.OpCodeLength) {
			case 1:
				writer.Write(def.OpCode.ToString("X2"));
				break;
			case 2:
				writer.Write(def.OpCode.ToString("X4"));
				break;
			default:
				throw new InvalidOperationException();
			}

			writer.Write(sep);
			writer.Write(def.OpCodeString);

			writer.Write(sep);
			writer.Write(def.InstructionString.Replace(',', '|'));

			writer.Write(sepNoSpace);

			void W(string prop) {
				writer.Write(" ");
				writer.Write(prop);
			}
			void WK(string key) {
				writer.Write(" ");
				writer.Write(key);
				writer.Write("=");
			}

			if (def.DecoderOption.Value != 0) {
				WK(OpCodeInfoKeywordKeys.DecoderOption);
				writer.Write(def.DecoderOption.RawName);
			}

			if (def.GroupIndex >= 0) {
				WK(OpCodeInfoKeywordKeys.GroupIndex);
				writer.Write(def.GroupIndex.ToString());
			}
			if (def.RmGroupIndex >= 0) {
				WK(OpCodeInfoKeywordKeys.RmGroupIndex);
				writer.Write(def.RmGroupIndex.ToString());
			}

			if ((def.Flags1 & InstructionDefFlags1.NoInstruction) != 0) W(OpCodeInfoKeywords.NoInstruction);
			if ((def.Flags1 & InstructionDefFlags1.Bit16) != 0) W(OpCodeInfoKeywords.Bit16);
			if ((def.Flags1 & (InstructionDefFlags1.Bit16 | InstructionDefFlags1.Bit32)) != 0) W(OpCodeInfoKeywords.Bit32);
			if ((def.Flags1 & InstructionDefFlags1.Bit64) != 0) W(OpCodeInfoKeywords.Bit64);
			if ((def.Flags1 & InstructionDefFlags1.Cpl0) != 0) W(OpCodeInfoKeywords.Cpl0);
			if ((def.Flags1 & InstructionDefFlags1.Cpl1) != 0) W(OpCodeInfoKeywords.Cpl1);
			if ((def.Flags1 & InstructionDefFlags1.Cpl2) != 0) W(OpCodeInfoKeywords.Cpl2);
			if ((def.Flags1 & InstructionDefFlags1.Cpl3) != 0) W(OpCodeInfoKeywords.Cpl3);
			if ((def.Flags1 & InstructionDefFlags1.Fwait) != 0) W(OpCodeInfoKeywords.Fwait);

			switch (def.OperandSize) {
			case CodeSize.Unknown: break;
			case CodeSize.Code16: W(OpCodeInfoKeywords.OperandSize16); break;
			case CodeSize.Code32: W(OpCodeInfoKeywords.OperandSize32); break;
			case CodeSize.Code64: W(OpCodeInfoKeywords.OperandSize64); break;
			default: throw new InvalidOperationException();
			}
			switch (def.AddressSize) {
			case CodeSize.Unknown: break;
			case CodeSize.Code16: W(OpCodeInfoKeywords.AddressSize16); break;
			case CodeSize.Code32: W(OpCodeInfoKeywords.AddressSize32); break;
			case CodeSize.Code64: W(OpCodeInfoKeywords.AddressSize64); break;
			default: throw new InvalidOperationException();
			}

			switch (def.LBit) {
			case OpCodeL.None: break;
			case OpCodeL.L0:
			case OpCodeL.LZ: W(OpCodeInfoKeywords.L0); break;
			case OpCodeL.L1: W(OpCodeInfoKeywords.L1); break;
			case OpCodeL.LIG: W(OpCodeInfoKeywords.LIG); break;
			case OpCodeL.L128: W(OpCodeInfoKeywords.L128); break;
			case OpCodeL.L256: W(OpCodeInfoKeywords.L256); break;
			case OpCodeL.L512: W(OpCodeInfoKeywords.L512); break;
			default: throw new InvalidOperationException();
			}

			if ((def.Flags1 & InstructionDefFlags1.WIG32) != 0)
				W(OpCodeInfoKeywords.WIG32);
			else {
				switch (def.WBit) {
				case OpCodeW.None: break;
				case OpCodeW.W0: W(OpCodeInfoKeywords.W0); break;
				case OpCodeW.W1: W(OpCodeInfoKeywords.W1); break;
				case OpCodeW.WIG: W(OpCodeInfoKeywords.WIG); break;
				case OpCodeW.WIG32:
				default:
					throw new InvalidOperationException();
				}
			}

			switch (def.Mvex.EHBit) {
			case MvexEHBit.None: break;
			case MvexEHBit.EH0: W(OpCodeInfoKeywords.EH0); break;
			case MvexEHBit.EH1: W(OpCodeInfoKeywords.EH1); break;
			default: throw new InvalidOperationException();
			}

			if (def.OpCount > 0) {
				WK(OpCodeInfoKeywordKeys.OpCodeOperandKind);
				for (int i = 0; i < def.OpCount; i++) {
					if (i > 0)
						writer.Write(";");
					writer.Write(def.OpKindDefs[i].EnumValue.RawName);
				}
			}

			if ((def.Flags1 & InstructionDefFlags1.SaveRestore) != 0) W(OpCodeInfoKeywords.SaveRestore);
			if ((def.Flags1 & InstructionDefFlags1.StackInstruction) != 0) W(OpCodeInfoKeywords.StackInstruction);
			if ((def.Flags1 & InstructionDefFlags1.IgnoresSegment) != 0) W(OpCodeInfoKeywords.IgnoresSegment);
			if ((def.Flags1 & InstructionDefFlags1.OpMaskReadWrite) != 0) W(OpCodeInfoKeywords.OpMaskReadWrite);
			if ((def.Flags1 & InstructionDefFlags1.Lock) != 0) W(OpCodeInfoKeywords.Lock);
			if ((def.Flags1 & InstructionDefFlags1.Xacquire) != 0) W(OpCodeInfoKeywords.Xacquire);
			if ((def.Flags1 & InstructionDefFlags1.Xrelease) != 0) W(OpCodeInfoKeywords.Xrelease);
			switch (def.Flags1 & (InstructionDefFlags1.Rep | InstructionDefFlags1.Repne)) {
			case InstructionDefFlags1.Rep | InstructionDefFlags1.Repne:
				W(OpCodeInfoKeywords.Repe);
				W(OpCodeInfoKeywords.Repne);
				break;
			case InstructionDefFlags1.Rep:
				W(OpCodeInfoKeywords.Rep);
				break;
			case InstructionDefFlags1.Repne:
				W(OpCodeInfoKeywords.Repne);
				break;
			default:
				break;
			}
			if ((def.Flags1 & InstructionDefFlags1.Bnd) != 0) W(OpCodeInfoKeywords.Bnd);
			if ((def.Flags1 & InstructionDefFlags1.HintTaken) != 0) W(OpCodeInfoKeywords.HintTaken);
			if ((def.Flags1 & InstructionDefFlags1.Notrack) != 0) W(OpCodeInfoKeywords.Notrack);

			bool addTupleType = def.Encoding switch {
				EncodingKind.EVEX or EncodingKind.MVEX => def.OpKindDefs.Any(x => x.Memory),
				_ => false,
			};
			if (addTupleType) {
				WK(OpCodeInfoKeywordKeys.TupleType);
				writer.Write(def.TupleType.ToString());
			}

			if (def.Encoding == EncodingKind.MVEX) {
				WK(OpCodeInfoKeywordKeys.MVEX);
				ref readonly var mvex = ref def.Mvex;
				var value = $"{mvex.TupleTypeLutKind.RawName};{mvex.ConvFn};0x{mvex.ValidConvFns:X};0x{mvex.ValidSwizzleFns:X}";
				writer.Write(value);
			}

			if ((def.Flags1 & InstructionDefFlags1.Broadcast) != 0) W(OpCodeInfoKeywords.Broadcast);
			if ((def.Flags1 & InstructionDefFlags1.RoundingControl) != 0) W(OpCodeInfoKeywords.RoundingControl);
			if ((def.Flags1 & InstructionDefFlags1.SuppressAllExceptions) != 0) W(OpCodeInfoKeywords.SuppressAllExceptions);
			if ((def.Flags1 & InstructionDefFlags1.OpMaskRegister) != 0) W(OpCodeInfoKeywords.OpMaskRegister);
			if ((def.Flags1 & InstructionDefFlags1.ZeroingMasking) != 0) W(OpCodeInfoKeywords.ZeroingMasking);
			if ((def.Flags1 & InstructionDefFlags1.RequireOpMaskRegister) != 0) W(OpCodeInfoKeywords.RequireOpMaskRegister);
			if ((def.Flags1 & InstructionDefFlags1.IgnoresModBits) != 0) W(OpCodeInfoKeywords.IgnoresModBits);
			if ((def.Flags1 & InstructionDefFlags1.RequiresUniqueRegNums) != 0) W(OpCodeInfoKeywords.RequiresUniqueRegNums);
			if ((def.Flags1 & InstructionDefFlags1.No66) != 0) W(OpCodeInfoKeywords.No66);
			if ((def.Flags1 & InstructionDefFlags1.NFx) != 0) W(OpCodeInfoKeywords.NFx);

			if ((def.Flags2 & InstructionDefFlags2.IntelDecoder16) != 0) W(OpCodeInfoKeywords.IntelDecoder16);
			if ((def.Flags2 & (InstructionDefFlags2.IntelDecoder16 | InstructionDefFlags2.IntelDecoder32)) != 0) W(OpCodeInfoKeywords.IntelDecoder32);
			if ((def.Flags2 & InstructionDefFlags2.IntelDecoder64) != 0) W(OpCodeInfoKeywords.IntelDecoder64);
			if ((def.Flags2 & InstructionDefFlags2.AmdDecoder16) != 0) W(OpCodeInfoKeywords.AmdDecoder16);
			if ((def.Flags2 & (InstructionDefFlags2.AmdDecoder16 | InstructionDefFlags2.AmdDecoder32)) != 0) W(OpCodeInfoKeywords.AmdDecoder32);
			if ((def.Flags2 & InstructionDefFlags2.AmdDecoder64) != 0) W(OpCodeInfoKeywords.AmdDecoder64);
			if ((def.Flags2 & InstructionDefFlags2.RealMode) != 0) W(OpCodeInfoKeywords.RealMode);
			if ((def.Flags2 & InstructionDefFlags2.ProtectedMode) != 0) W(OpCodeInfoKeywords.ProtectedMode);
			if ((def.Flags2 & InstructionDefFlags2.Virtual8086Mode) != 0) W(OpCodeInfoKeywords.Virtual8086Mode);
			if ((def.Flags2 & InstructionDefFlags2.CompatibilityMode) != 0) W(OpCodeInfoKeywords.CompatibilityMode);
			if ((def.Flags2 & InstructionDefFlags2.LongMode) != 0) W(OpCodeInfoKeywords.LongMode);
			if ((def.Flags2 & InstructionDefFlags2.UseOutsideSmm) != 0) W(OpCodeInfoKeywords.UseOutsideSmm);
			if ((def.Flags2 & InstructionDefFlags2.UseInSmm) != 0) W(OpCodeInfoKeywords.UseInSmm);
			if ((def.Flags2 & InstructionDefFlags2.UseOutsideEnclaveSgx) != 0) W(OpCodeInfoKeywords.UseOutsideEnclaveSgx);
			if ((def.Flags2 & InstructionDefFlags2.UseInEnclaveSgx1) != 0) W(OpCodeInfoKeywords.UseInEnclaveSgx1);
			if ((def.Flags2 & InstructionDefFlags2.UseInEnclaveSgx2) != 0) W(OpCodeInfoKeywords.UseInEnclaveSgx2);
			if ((def.Flags2 & InstructionDefFlags2.UseOutsideVmxOp) != 0) W(OpCodeInfoKeywords.UseOutsideVmxOp);
			if ((def.Flags2 & InstructionDefFlags2.UseInVmxRootOp) != 0) W(OpCodeInfoKeywords.UseInVmxRootOp);
			if ((def.Flags2 & InstructionDefFlags2.UseInVmxNonRootOp) != 0) W(OpCodeInfoKeywords.UseInVmxNonRootOp);
			if ((def.Flags2 & InstructionDefFlags2.UseOutsideSeam) != 0) W(OpCodeInfoKeywords.UseOutsideSeam);
			if ((def.Flags2 & InstructionDefFlags2.UseInSeam) != 0) W(OpCodeInfoKeywords.UseInSeam);
			if ((def.Flags2 & InstructionDefFlags2.TdxNonRootGenUd) != 0) W(OpCodeInfoKeywords.TdxNonRootGenUd);
			if ((def.Flags2 & InstructionDefFlags2.TdxNonRootGenVe) != 0) W(OpCodeInfoKeywords.TdxNonRootGenVe);
			if ((def.Flags2 & InstructionDefFlags2.TdxNonRootMayGenEx) != 0) W(OpCodeInfoKeywords.TdxNonRootMayGenEx);
			if ((def.Flags2 & InstructionDefFlags2.IntelVmExit) != 0) W(OpCodeInfoKeywords.IntelVmExit);
			if ((def.Flags2 & InstructionDefFlags2.IntelMayVmExit) != 0) W(OpCodeInfoKeywords.IntelMayVmExit);
			if ((def.Flags2 & InstructionDefFlags2.IntelSmmVmExit) != 0) W(OpCodeInfoKeywords.IntelSmmVmExit);
			if ((def.Flags2 & InstructionDefFlags2.AmdVmExit) != 0) W(OpCodeInfoKeywords.AmdVmExit);
			if ((def.Flags2 & InstructionDefFlags2.AmdMayVmExit) != 0) W(OpCodeInfoKeywords.AmdMayVmExit);
			if ((def.Flags2 & InstructionDefFlags2.TsxAbort) != 0) W(OpCodeInfoKeywords.TsxAbort);
			if ((def.Flags2 & InstructionDefFlags2.TsxImplAbort) != 0) W(OpCodeInfoKeywords.TsxImplAbort);
			if ((def.Flags2 & InstructionDefFlags2.TsxMayAbort) != 0) W(OpCodeInfoKeywords.TsxMayAbort);

			if ((def.Flags3 & InstructionDefFlags3.DefaultOpSize64) != 0) W(OpCodeInfoKeywords.DefaultOpSize64);
			if ((def.Flags3 & InstructionDefFlags3.ForceOpSize64) != 0) W(OpCodeInfoKeywords.ForceOpSize64);
			if ((def.Flags3 & InstructionDefFlags3.IntelForceOpSize64) != 0) W(OpCodeInfoKeywords.IntelForceOpSize64);
			if ((def.Flags3 & InstructionDefFlags3.InputOutput) != 0) W(OpCodeInfoKeywords.InputOutput);
			if ((def.Flags3 & InstructionDefFlags3.Nop) != 0) W(OpCodeInfoKeywords.Nop);
			if ((def.Flags3 & InstructionDefFlags3.ReservedNop) != 0) W(OpCodeInfoKeywords.ReservedNop);
			if ((def.Flags3 & InstructionDefFlags3.IgnoresRoundingControl) != 0) W(OpCodeInfoKeywords.IgnoresRoundingControl);
			if ((def.Flags3 & InstructionDefFlags3.SerializingIntel) != 0) W(OpCodeInfoKeywords.SerializingIntel);
			if ((def.Flags3 & InstructionDefFlags3.SerializingAmd) != 0) W(OpCodeInfoKeywords.SerializingAmd);
			if ((def.Flags3 & InstructionDefFlags3.MayRequireCpl0) != 0) W(OpCodeInfoKeywords.MayRequireCpl0);
			if ((def.Flags3 & InstructionDefFlags3.AmdLockRegBit) != 0) W(OpCodeInfoKeywords.AmdLockRegBit);
			if ((def.Flags3 & InstructionDefFlags3.CetTracked) != 0) W(OpCodeInfoKeywords.CetTracked);
			if ((def.Flags3 & InstructionDefFlags3.NonTemporal) != 0) W(OpCodeInfoKeywords.NonTemporal);
			if ((def.Flags3 & InstructionDefFlags3.FpuNoWait) != 0) W(OpCodeInfoKeywords.FpuNoWait);
			if ((def.Flags3 & InstructionDefFlags3.Privileged) != 0) W(OpCodeInfoKeywords.Privileged);
			if ((def.Flags3 & InstructionDefFlags3.RequiresUniqueDestRegNum) != 0) W(OpCodeInfoKeywords.RequiresUniqueDestRegNum);

			if ((def.Mvex.Flags1 & MvexInfoFlags1.EvictionHint) != 0) W(OpCodeInfoKeywords.EvictionHint);
			if ((def.Mvex.Flags1 & MvexInfoFlags1.IgnoresOpMaskRegister) != 0) W(OpCodeInfoKeywords.IgnoresOpMaskRegister);
			if ((def.Mvex.Flags1 & MvexInfoFlags1.ImmRoundingControl) != 0) W(OpCodeInfoKeywords.ImmRoundingControl);
			if ((def.Mvex.Flags2 & MvexInfoFlags2.NoSaeRoundingControl) != 0) W(OpCodeInfoKeywords.NoSaeRoundingControl);

			writer.WriteLine();
		}
	}
}
