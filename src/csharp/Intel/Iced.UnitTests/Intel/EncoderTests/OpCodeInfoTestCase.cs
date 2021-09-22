// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if ENCODER && OPCODE_INFO
using Iced.Intel;

namespace Iced.UnitTests.Intel.EncoderTests {
#if MVEX
	struct MvexTestCase {
		public MvexEHBit EHBit;
		public bool CanUseEvictionHint;
		public bool CanUseImmRoundingControl;
		public bool IgnoresOpMaskRegister;
		public bool NoSaeRc;
		public MvexTupleTypeLutKind TupleTypeLutKind;
		public MvexConvFn ConversionFunc;
		public byte ValidConversionFuncsMask;
		public byte ValidSwizzleFuncsMask;
	}
#endif

	sealed class OpCodeInfoTestCase {
		public int LineNumber = -1;
		public Code Code = Code.INVALID;
		public Mnemonic Mnemonic = Mnemonic.INVALID;
		public string OpCodeString = string.Empty;
		public string InstructionString = string.Empty;
		public EncodingKind Encoding = EncodingKind.Legacy;
		public bool IsInstruction;
		public bool Mode16;
		public bool Mode32;
		public bool Mode64;
		public bool Fwait;
		public int OperandSize;
		public int AddressSize;
		public uint L;
		public uint W;
		public bool IsLIG;
		public bool IsWIG;
		public bool IsWIG32;
		public TupleType TupleType = TupleType.N1;
		public MemorySize MemorySize = MemorySize.Unknown;
		public MemorySize BroadcastMemorySize = MemorySize.Unknown;
		public DecoderOptions DecoderOption = DecoderOptions.None;
		public bool CanBroadcast;
		public bool CanUseRoundingControl;
		public bool CanSuppressAllExceptions;
		public bool CanUseOpMaskRegister;
		public bool RequireOpMaskRegister;
		public bool CanUseZeroingMasking;
		public bool CanUseLockPrefix;
		public bool CanUseXacquirePrefix;
		public bool CanUseXreleasePrefix;
		public bool CanUseRepPrefix;
		public bool CanUseRepnePrefix;
		public bool CanUseBndPrefix;
		public bool CanUseHintTakenPrefix;
		public bool CanUseNotrackPrefix;
		public bool IgnoresRoundingControl;
		public bool AmdLockRegBit;
		public bool DefaultOpSize64;
		public bool ForceOpSize64;
		public bool IntelForceOpSize64;
		public bool Cpl0;
		public bool Cpl1;
		public bool Cpl2;
		public bool Cpl3;
		public bool IsInputOutput;
		public bool IsNop;
		public bool IsReservedNop;
		public bool IsSerializingIntel;
		public bool IsSerializingAmd;
		public bool MayRequireCpl0;
		public bool IsCetTracked;
		public bool IsNonTemporal;
		public bool IsFpuNoWait;
		public bool IgnoresModBits;
		public bool No66;
		public bool NFx;
		public bool RequiresUniqueRegNums;
		public bool RequiresUniqueDestRegNum;
		public bool IsPrivileged;
		public bool IsSaveRestore;
		public bool IsStackInstruction;
		public bool IgnoresSegment;
		public bool IsOpMaskReadWrite;
		public bool RealMode;
		public bool ProtectedMode;
		public bool Virtual8086Mode;
		public bool CompatibilityMode;
		public bool LongMode;
		public bool UseOutsideSmm;
		public bool UseInSmm;
		public bool UseOutsideEnclaveSgx;
		public bool UseInEnclaveSgx1;
		public bool UseInEnclaveSgx2;
		public bool UseOutsideVmxOp;
		public bool UseInVmxRootOp;
		public bool UseInVmxNonRootOp;
		public bool UseOutsideSeam;
		public bool UseInSeam;
		public bool TdxNonRootGenUd;
		public bool TdxNonRootGenVe;
		public bool TdxNonRootMayGenEx;
		public bool IntelVmExit;
		public bool IntelMayVmExit;
		public bool IntelSmmVmExit;
		public bool AmdVmExit;
		public bool AmdMayVmExit;
		public bool TsxAbort;
		public bool TsxImplAbort;
		public bool TsxMayAbort;
		public bool IntelDecoder16;
		public bool IntelDecoder32;
		public bool IntelDecoder64;
		public bool AmdDecoder16;
		public bool AmdDecoder32;
		public bool AmdDecoder64;
		public OpCodeTableKind Table = OpCodeTableKind.Normal;
		public MandatoryPrefix MandatoryPrefix = MandatoryPrefix.None;
		public uint OpCode = 0;
		public int OpCodeLength = 0;
		public bool IsGroup = false;
		public int GroupIndex = -1;
		public bool IsRmGroup = false;
		public int RmGroupIndex = -1;
		public int OpCount = 0;
		public OpCodeOperandKind Op0Kind = OpCodeOperandKind.None;
		public OpCodeOperandKind Op1Kind = OpCodeOperandKind.None;
		public OpCodeOperandKind Op2Kind = OpCodeOperandKind.None;
		public OpCodeOperandKind Op3Kind = OpCodeOperandKind.None;
		public OpCodeOperandKind Op4Kind = OpCodeOperandKind.None;
#if MVEX
		public MvexTestCase Mvex;
#endif
	}
}
#endif
