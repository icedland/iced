// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if ENCODER && OPCODE_INFO
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.EncoderTests {
	public sealed class OpCodeInfoTests {
		[Theory]
		[MemberData(nameof(TestAllOpCodeInfos_Data))]
#pragma warning disable xUnit1026 // Theory methods should use all of their parameters
		void Test_all_OpCodeInfos(int lineNo, Code code, string opCodeString, string instructionString, OpCodeInfoTestCase tc) {
#pragma warning restore xUnit1026 // Theory methods should use all of their parameters
			var info = tc.Code.ToOpCode();
			Assert.Equal(tc.Code, info.Code);
#pragma warning disable xUnit2006 // Do not use invalid string equality check
			// Show the full string without ellipses by using Equal<string>() instead of Equal()
			Assert.Equal<string>(tc.OpCodeString, info.ToOpCodeString());
			Assert.Equal<string>(tc.InstructionString, info.ToInstructionString());
#pragma warning restore xUnit2006 // Do not use invalid string equality check
			Assert.True((object)info.ToInstructionString() == info.ToString());
			Assert.Equal(tc.Mnemonic, info.Mnemonic);
			Assert.Equal(tc.Encoding, info.Encoding);
			Assert.Equal(tc.IsInstruction, info.IsInstruction);
			Assert.Equal(tc.Mode16, info.Mode16);
			Assert.Equal(tc.Mode16, info.IsAvailableInMode(16));
			Assert.Equal(tc.Mode32, info.Mode32);
			Assert.Equal(tc.Mode32, info.IsAvailableInMode(32));
			Assert.Equal(tc.Mode64, info.Mode64);
			Assert.Equal(tc.Mode64, info.IsAvailableInMode(64));
			Assert.Equal(tc.Fwait, info.Fwait);
			Assert.Equal(tc.OperandSize, info.OperandSize);
			Assert.Equal(tc.AddressSize, info.AddressSize);
			Assert.Equal(tc.L, info.L);
			Assert.Equal(tc.W, info.W);
			Assert.Equal(tc.IsLIG, info.IsLIG);
			Assert.Equal(tc.IsWIG, info.IsWIG);
			Assert.Equal(tc.IsWIG32, info.IsWIG32);
			Assert.Equal(tc.TupleType, info.TupleType);
			Assert.Equal(tc.MemorySize, info.MemorySize);
			Assert.Equal(tc.BroadcastMemorySize, info.BroadcastMemorySize);
			Assert.Equal(tc.DecoderOption, info.DecoderOption);
			Assert.Equal(tc.CanBroadcast, info.CanBroadcast);
			Assert.Equal(tc.CanUseRoundingControl, info.CanUseRoundingControl);
			Assert.Equal(tc.CanSuppressAllExceptions, info.CanSuppressAllExceptions);
			Assert.Equal(tc.CanUseOpMaskRegister, info.CanUseOpMaskRegister);
			Assert.Equal(tc.RequireOpMaskRegister, info.RequireOpMaskRegister);
			if (tc.RequireOpMaskRegister) {
				Assert.True(info.CanUseOpMaskRegister);
				Assert.False(info.CanUseZeroingMasking);
			}
			Assert.Equal(tc.CanUseZeroingMasking, info.CanUseZeroingMasking);
			Assert.Equal(tc.CanUseLockPrefix, info.CanUseLockPrefix);
			Assert.Equal(tc.CanUseXacquirePrefix, info.CanUseXacquirePrefix);
			Assert.Equal(tc.CanUseXreleasePrefix, info.CanUseXreleasePrefix);
			Assert.Equal(tc.CanUseRepPrefix, info.CanUseRepPrefix);
			Assert.Equal(tc.CanUseRepnePrefix, info.CanUseRepnePrefix);
			Assert.Equal(tc.CanUseBndPrefix, info.CanUseBndPrefix);
			Assert.Equal(tc.CanUseHintTakenPrefix, info.CanUseHintTakenPrefix);
			Assert.Equal(tc.CanUseNotrackPrefix, info.CanUseNotrackPrefix);
			Assert.Equal(tc.IgnoresRoundingControl, info.IgnoresRoundingControl);
			Assert.Equal(tc.AmdLockRegBit, info.AmdLockRegBit);
			Assert.Equal(tc.DefaultOpSize64, info.DefaultOpSize64);
			Assert.Equal(tc.ForceOpSize64, info.ForceOpSize64);
			Assert.Equal(tc.IntelForceOpSize64, info.IntelForceOpSize64);
			Assert.Equal(tc.Cpl0 && !tc.Cpl1 && !tc.Cpl2 && !tc.Cpl3, info.MustBeCpl0);
			Assert.Equal(tc.Cpl0, info.Cpl0);
			Assert.Equal(tc.Cpl1, info.Cpl1);
			Assert.Equal(tc.Cpl2, info.Cpl2);
			Assert.Equal(tc.Cpl3, info.Cpl3);
			Assert.Equal(tc.IsInputOutput, info.IsInputOutput);
			Assert.Equal(tc.IsNop, info.IsNop);
			Assert.Equal(tc.IsReservedNop, info.IsReservedNop);
			Assert.Equal(tc.IsSerializingIntel, info.IsSerializingIntel);
			Assert.Equal(tc.IsSerializingAmd, info.IsSerializingAmd);
			Assert.Equal(tc.MayRequireCpl0, info.MayRequireCpl0);
			Assert.Equal(tc.IsCetTracked, info.IsCetTracked);
			Assert.Equal(tc.IsNonTemporal, info.IsNonTemporal);
			Assert.Equal(tc.IsFpuNoWait, info.IsFpuNoWait);
			Assert.Equal(tc.IgnoresModBits, info.IgnoresModBits);
			Assert.Equal(tc.No66, info.No66);
			Assert.Equal(tc.NFx, info.NFx);
			Assert.Equal(tc.RequiresUniqueRegNums, info.RequiresUniqueRegNums);
			Assert.Equal(tc.RequiresUniqueDestRegNum, info.RequiresUniqueDestRegNum);
			Assert.Equal(tc.IsPrivileged, info.IsPrivileged);
			Assert.Equal(tc.IsSaveRestore, info.IsSaveRestore);
			Assert.Equal(tc.IsStackInstruction, info.IsStackInstruction);
			Assert.Equal(tc.IgnoresSegment, info.IgnoresSegment);
			Assert.Equal(tc.IsOpMaskReadWrite, info.IsOpMaskReadWrite);
			Assert.Equal(tc.RealMode, info.RealMode);
			Assert.Equal(tc.ProtectedMode, info.ProtectedMode);
			Assert.Equal(tc.Virtual8086Mode, info.Virtual8086Mode);
			Assert.Equal(tc.CompatibilityMode, info.CompatibilityMode);
			Assert.Equal(tc.LongMode, info.LongMode);
			Assert.Equal(tc.UseOutsideSmm, info.UseOutsideSmm);
			Assert.Equal(tc.UseInSmm, info.UseInSmm);
			Assert.Equal(tc.UseOutsideEnclaveSgx, info.UseOutsideEnclaveSgx);
			Assert.Equal(tc.UseInEnclaveSgx1, info.UseInEnclaveSgx1);
			Assert.Equal(tc.UseInEnclaveSgx2, info.UseInEnclaveSgx2);
			Assert.Equal(tc.UseOutsideVmxOp, info.UseOutsideVmxOp);
			Assert.Equal(tc.UseInVmxRootOp, info.UseInVmxRootOp);
			Assert.Equal(tc.UseInVmxNonRootOp, info.UseInVmxNonRootOp);
			Assert.Equal(tc.UseOutsideSeam, info.UseOutsideSeam);
			Assert.Equal(tc.UseInSeam, info.UseInSeam);
			Assert.Equal(tc.TdxNonRootGenUd, info.TdxNonRootGenUd);
			Assert.Equal(tc.TdxNonRootGenVe, info.TdxNonRootGenVe);
			Assert.Equal(tc.TdxNonRootMayGenEx, info.TdxNonRootMayGenEx);
			Assert.Equal(tc.IntelVmExit, info.IntelVmExit);
			Assert.Equal(tc.IntelMayVmExit, info.IntelMayVmExit);
			Assert.Equal(tc.IntelSmmVmExit, info.IntelSmmVmExit);
			Assert.Equal(tc.AmdVmExit, info.AmdVmExit);
			Assert.Equal(tc.AmdMayVmExit, info.AmdMayVmExit);
			Assert.Equal(tc.TsxAbort, info.TsxAbort);
			Assert.Equal(tc.TsxImplAbort, info.TsxImplAbort);
			Assert.Equal(tc.TsxMayAbort, info.TsxMayAbort);
			Assert.Equal(tc.IntelDecoder16, info.IntelDecoder16);
			Assert.Equal(tc.IntelDecoder32, info.IntelDecoder32);
			Assert.Equal(tc.IntelDecoder64, info.IntelDecoder64);
			Assert.Equal(tc.AmdDecoder16, info.AmdDecoder16);
			Assert.Equal(tc.AmdDecoder32, info.AmdDecoder32);
			Assert.Equal(tc.AmdDecoder64, info.AmdDecoder64);
			Assert.Equal(tc.Table, info.Table);
			Assert.Equal(tc.MandatoryPrefix, info.MandatoryPrefix);
			Assert.Equal(tc.OpCode, info.OpCode);
			Assert.Equal(tc.OpCodeLength, info.OpCodeLength);
			Assert.Equal(tc.IsGroup, info.IsGroup);
			Assert.Equal(tc.GroupIndex, info.GroupIndex);
			Assert.Equal(tc.IsRmGroup, info.IsRmGroup);
			Assert.Equal(tc.RmGroupIndex, info.RmGroupIndex);
			Assert.Equal(tc.OpCount, info.OpCount);
			Assert.Equal(tc.Op0Kind, info.Op0Kind);
			Assert.Equal(tc.Op1Kind, info.Op1Kind);
			Assert.Equal(tc.Op2Kind, info.Op2Kind);
			Assert.Equal(tc.Op3Kind, info.Op3Kind);
			Assert.Equal(tc.Op4Kind, info.Op4Kind);
			Assert.Equal(tc.Op0Kind, info.GetOpKind(0));
			Assert.Equal(tc.Op1Kind, info.GetOpKind(1));
			Assert.Equal(tc.Op2Kind, info.GetOpKind(2));
			Assert.Equal(tc.Op3Kind, info.GetOpKind(3));
			Assert.Equal(tc.Op4Kind, info.GetOpKind(4));
			Static.Assert(IcedConstants.MaxOpCount == 5 ? 0 : -1);
			for (int i = tc.OpCount; i < IcedConstants.MaxOpCount; i++)
				Assert.Equal(OpCodeOperandKind.None, info.GetOpKind(i));
#if MVEX
			Assert.Equal(tc.Mvex.EHBit, info.MvexEHBit);
			Assert.Equal(tc.Mvex.CanUseEvictionHint, info.MvexCanUseEvictionHint);
			Assert.Equal(tc.Mvex.CanUseImmRoundingControl, info.MvexCanUseImmRoundingControl);
			Assert.Equal(tc.Mvex.IgnoresOpMaskRegister, info.MvexIgnoresOpMaskRegister);
			Assert.Equal(tc.Mvex.NoSaeRc, info.MvexNoSaeRc);
			Assert.Equal(tc.Mvex.TupleTypeLutKind, info.MvexTupleTypeLutKind);
			Assert.Equal(tc.Mvex.ConversionFunc, info.MvexConversionFunc);
			Assert.Equal(tc.Mvex.ValidConversionFuncsMask, info.MvexValidConversionFuncsMask);
			Assert.Equal(tc.Mvex.ValidSwizzleFuncsMask, info.MvexValidSwizzleFuncsMask);
#endif
		}
		public static IEnumerable<object[]> TestAllOpCodeInfos_Data => OpCodeInfoTestCases.OpCodeInfoTests.Select(a => new object[] { a.LineNumber, a.Code, a.OpCodeString, a.InstructionString, a });

		[Fact]
		void GetOpKindThrowsIfInvalidInput() {
			var info = Code.Aaa.ToOpCode();
			Assert.Throws<ArgumentOutOfRangeException>(() => info.GetOpKind(int.MinValue));
			Assert.Throws<ArgumentOutOfRangeException>(() => info.GetOpKind(-1));
			info.GetOpKind(0);
			info.GetOpKind(IcedConstants.MaxOpCount - 1);
			Assert.Throws<ArgumentOutOfRangeException>(() => info.GetOpKind(IcedConstants.MaxOpCount));
			Assert.Throws<ArgumentOutOfRangeException>(() => info.GetOpKind(int.MaxValue));
		}

		[Fact]
		void Verify_Instruction_OpCodeInfo() {
			for (int i = 0; i < IcedConstants.CodeEnumCount; i++) {
				var code = (Code)i;
				Instruction instruction = default;
				instruction.Code = (Code)i;
				Assert.True(ReferenceEquals(code.ToOpCode(), instruction.OpCode));
			}
		}

		[Fact]
		void Make_sure_all_Code_values_are_tested_exactly_once() {
			var tested = new bool[IcedConstants.CodeEnumCount];
			foreach (var info in OpCodeInfoTestCases.OpCodeInfoTests) {
				Assert.False(tested[(int)info.Code]);
				tested[(int)info.Code] = true;
			}
			var sb = new StringBuilder();
			var codeNames = ToEnumConverter.GetCodeNames();
			for (int i = 0; i < tested.Length; i++) {
				if (!tested[i] && !CodeUtils.IsIgnored(codeNames[i])) {
					if (sb.Length > 0)
						sb.Append(',');
					sb.Append(codeNames[i]);
				}
			}
			Assert.Equal(string.Empty, sb.ToString());
		}
	}
}
#endif
