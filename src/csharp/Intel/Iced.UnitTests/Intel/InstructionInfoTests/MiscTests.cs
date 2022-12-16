// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if INSTR_INFO
using System;
using System.Collections.Generic;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.InstructionInfoTests {
	public sealed class MiscTests {
		[Fact]
		void IsBranchCall() {
			var jccShort = MiscTestsData.JccShort;
			var jcxShort = MiscTestsData.Jrcxz;
			var jmpNear = MiscTestsData.JmpNear;
			var jmpFar = MiscTestsData.JmpFar;
			var jmpShort = MiscTestsData.JmpShort;
			var jmpNearIndirect = MiscTestsData.JmpNearIndirect;
			var jmpFarIndirect = MiscTestsData.JmpFarIndirect;
			var jccNear = MiscTestsData.JccNear;
			var callFar = MiscTestsData.CallFar;
			var callNear = MiscTestsData.CallNear;
			var callNearIndirect = MiscTestsData.CallNearIndirect;
			var callFarIndirect = MiscTestsData.CallFarIndirect;
#if MVEX
			var jkccShort = MiscTestsData.JkccShort;
			var jkccNear = MiscTestsData.JkccNear;
#endif
			var loop = MiscTestsData.Loop;

			for (int i = 0; i < IcedConstants.CodeEnumCount; i++) {
				var code = (Code)i;
				Instruction instruction = default;
				instruction.Code = code;

				Assert.Equal(jccShort.Contains(code) || jccNear.Contains(code), code.IsJccShortOrNear());
				Assert.Equal(code.IsJccShortOrNear(), instruction.IsJccShortOrNear);

				Assert.Equal(jccNear.Contains(code), code.IsJccNear());
				Assert.Equal(code.IsJccNear(), instruction.IsJccNear);

				Assert.Equal(jccShort.Contains(code), code.IsJccShort());
				Assert.Equal(code.IsJccShort(), instruction.IsJccShort);

				Assert.Equal(jcxShort.Contains(code), code.IsJcxShort());
				Assert.Equal(code.IsJcxShort(), instruction.IsJcxShort);

				Assert.Equal(jmpShort.Contains(code), code.IsJmpShort());
				Assert.Equal(code.IsJmpShort(), instruction.IsJmpShort);

				Assert.Equal(jmpNear.Contains(code), code.IsJmpNear());
				Assert.Equal(code.IsJmpNear(), instruction.IsJmpNear);

				Assert.Equal(jmpShort.Contains(code) || jmpNear.Contains(code), code.IsJmpShortOrNear());
				Assert.Equal(code.IsJmpShortOrNear(), instruction.IsJmpShortOrNear);

				Assert.Equal(jmpFar.Contains(code), code.IsJmpFar());
				Assert.Equal(code.IsJmpFar(), instruction.IsJmpFar);

				Assert.Equal(callNear.Contains(code), code.IsCallNear());
				Assert.Equal(code.IsCallNear(), instruction.IsCallNear);

				Assert.Equal(callFar.Contains(code), code.IsCallFar());
				Assert.Equal(code.IsCallFar(), instruction.IsCallFar);

				Assert.Equal(jmpNearIndirect.Contains(code), code.IsJmpNearIndirect());
				Assert.Equal(code.IsJmpNearIndirect(), instruction.IsJmpNearIndirect);

				Assert.Equal(jmpFarIndirect.Contains(code), code.IsJmpFarIndirect());
				Assert.Equal(code.IsJmpFarIndirect(), instruction.IsJmpFarIndirect);

				Assert.Equal(callNearIndirect.Contains(code), code.IsCallNearIndirect());
				Assert.Equal(code.IsCallNearIndirect(), instruction.IsCallNearIndirect);

				Assert.Equal(callFarIndirect.Contains(code), code.IsCallFarIndirect());
				Assert.Equal(code.IsCallFarIndirect(), instruction.IsCallFarIndirect);

#if MVEX
				Assert.Equal(jkccShort.Contains(code) || jkccNear.Contains(code), code.IsJkccShortOrNear());
				Assert.Equal(code.IsJkccShortOrNear(), instruction.IsJkccShortOrNear);

				Assert.Equal(jkccNear.Contains(code), code.IsJkccNear());
				Assert.Equal(code.IsJkccNear(), instruction.IsJkccNear);

				Assert.Equal(jkccShort.Contains(code), code.IsJkccShort());
				Assert.Equal(code.IsJkccShort(), instruction.IsJkccShort);
#endif
				Assert.Equal(loop.Contains(code), code.IsLoop() || code.IsLoopcc());
				Assert.Equal(code.IsLoop(), instruction.IsLoop);
				Assert.Equal(code.IsLoopcc(), instruction.IsLoopcc);
			}
		}

		[Fact]
		void Verify_NegateConditionCode() {
			var toNegatedCodeValue = new Dictionary<Code, Code>();
			foreach (var info in MiscTestsData.JccShortInfos)
				toNegatedCodeValue.Add(info.jcc, info.negated);
			foreach (var info in MiscTestsData.JccNearInfos)
				toNegatedCodeValue.Add(info.jcc, info.negated);
			foreach (var info in MiscTestsData.SetccInfos)
				toNegatedCodeValue.Add(info.setcc, info.negated);
			foreach (var info in MiscTestsData.CmovccInfos)
				toNegatedCodeValue.Add(info.cmovcc, info.negated);
			foreach (var info in MiscTestsData.CmpccxaddInfos)
				toNegatedCodeValue.Add(info.cmpccxadd, info.negated);
			foreach (var info in MiscTestsData.LoopccInfos)
				toNegatedCodeValue.Add(info.loopcc, info.negated);
			foreach (var info in MiscTestsData.JkccShortInfos)
				toNegatedCodeValue.Add(info.jkcc, info.negated);
			foreach (var info in MiscTestsData.JkccNearInfos)
				toNegatedCodeValue.Add(info.jkcc, info.negated);

			for (int i = 0; i < IcedConstants.CodeEnumCount; i++) {
				var code = (Code)i;
				Instruction instruction = default;
				instruction.Code = code;

				if (!toNegatedCodeValue.TryGetValue(code, out var negatedCodeValue))
					negatedCodeValue = code;

				Assert.Equal(negatedCodeValue, code.NegateConditionCode());
				instruction.NegateConditionCode();
				Assert.Equal(negatedCodeValue, instruction.Code);
			}
		}

		[Fact]
		void Verify_ToShortBranch() {
			var toShortBranch = new Dictionary<Code, Code>();
			foreach (var info in MiscTestsData.JccNearInfos)
				toShortBranch.Add(info.jcc, info.jccShort);
			foreach (var info in MiscTestsData.JmpInfos)
				toShortBranch.Add(info.jmpNear, info.jmpShort);
			foreach (var info in MiscTestsData.JkccNearInfos)
				toShortBranch.Add(info.jkcc, info.jkccShort);

			for (int i = 0; i < IcedConstants.CodeEnumCount; i++) {
				var code = (Code)i;
				Instruction instruction = default;
				instruction.Code = code;

				if (!toShortBranch.TryGetValue(code, out var shortCodeValue))
					shortCodeValue = code;

				Assert.Equal(shortCodeValue, code.ToShortBranch());
				instruction.ToShortBranch();
				Assert.Equal(shortCodeValue, instruction.Code);
			}
		}

		[Fact]
		void Verify_ToNearBranch() {
			var toNearBranch = new Dictionary<Code, Code>();
			foreach (var info in MiscTestsData.JccShortInfos)
				toNearBranch.Add(info.jcc, info.jccNear);
			foreach (var info in MiscTestsData.JmpInfos)
				toNearBranch.Add(info.jmpShort, info.jmpNear);
			foreach (var info in MiscTestsData.JkccShortInfos)
				toNearBranch.Add(info.jkcc, info.jkccNear);

			for (int i = 0; i < IcedConstants.CodeEnumCount; i++) {
				var code = (Code)i;
				Instruction instruction = default;
				instruction.Code = code;

				if (!toNearBranch.TryGetValue(code, out var nearCodeValue))
					nearCodeValue = code;

				Assert.Equal(nearCodeValue, code.ToNearBranch());
				instruction.ToNearBranch();
				Assert.Equal(nearCodeValue, instruction.Code);
			}
		}

		[Fact]
		void Verify_ConditionCode() {
			var toConditionCode = new Dictionary<Code, ConditionCode>();
			foreach (var info in MiscTestsData.JccShortInfos)
				toConditionCode.Add(info.jcc, info.cc);
			foreach (var info in MiscTestsData.JccNearInfos)
				toConditionCode.Add(info.jcc, info.cc);
			foreach (var info in MiscTestsData.SetccInfos)
				toConditionCode.Add(info.setcc, info.cc);
			foreach (var info in MiscTestsData.CmovccInfos)
				toConditionCode.Add(info.cmovcc, info.cc);
			foreach (var info in MiscTestsData.CmpccxaddInfos)
				toConditionCode.Add(info.cmpccxadd, info.cc);
			foreach (var info in MiscTestsData.LoopccInfos)
				toConditionCode.Add(info.loopcc, info.cc);
			foreach (var info in MiscTestsData.JkccShortInfos)
				toConditionCode.Add(info.jkcc, info.cc);
			foreach (var info in MiscTestsData.JkccNearInfos)
				toConditionCode.Add(info.jkcc, info.cc);

			for (int i = 0; i < IcedConstants.CodeEnumCount; i++) {
				var code = (Code)i;
				Instruction instruction = default;
				instruction.Code = code;

				if (!toConditionCode.TryGetValue(code, out var cc))
					cc = ConditionCode.None;

				Assert.Equal(cc, code.ConditionCode());
				Assert.Equal(cc, instruction.ConditionCode);
			}
		}

		[Fact]
		void Verify_StringInstr() {
			var stringInstr = MiscTestsData.StringInstr;

			for (int i = 0; i < IcedConstants.CodeEnumCount; i++) {
				var code = (Code)i;
				Instruction instruction = default;
				instruction.Code = code;

				Assert.Equal(stringInstr.Contains(code), code.IsStringInstruction());
				Assert.Equal(code.IsStringInstruction(), instruction.IsStringInstruction);
			}
		}

		[Fact]
		void Verify_ConditionCode_values_are_in_correct_order() {
			Static.Assert((int)ConditionCode.None == 0 ? 0 : -1);
			Static.Assert((int)ConditionCode.o == 1 ? 0 : -1);
			Static.Assert((int)ConditionCode.no == 2 ? 0 : -1);
			Static.Assert((int)ConditionCode.b == 3 ? 0 : -1);
			Static.Assert((int)ConditionCode.ae == 4 ? 0 : -1);
			Static.Assert((int)ConditionCode.e == 5 ? 0 : -1);
			Static.Assert((int)ConditionCode.ne == 6 ? 0 : -1);
			Static.Assert((int)ConditionCode.be == 7 ? 0 : -1);
			Static.Assert((int)ConditionCode.a == 8 ? 0 : -1);
			Static.Assert((int)ConditionCode.s == 9 ? 0 : -1);
			Static.Assert((int)ConditionCode.ns == 10 ? 0 : -1);
			Static.Assert((int)ConditionCode.p == 11 ? 0 : -1);
			Static.Assert((int)ConditionCode.np == 12 ? 0 : -1);
			Static.Assert((int)ConditionCode.l == 13 ? 0 : -1);
			Static.Assert((int)ConditionCode.ge == 14 ? 0 : -1);
			Static.Assert((int)ConditionCode.le == 15 ? 0 : -1);
			Static.Assert((int)ConditionCode.g == 16 ? 0 : -1);
		}

		[Fact]
		void InstructionInfoExtensions_Encoding_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)(-1)).Encoding());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)IcedConstants.CodeEnumCount).Encoding());
		}

		[Fact]
		void InstructionInfoExtensions_CpuidFeatures_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)(-1)).CpuidFeatures());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)IcedConstants.CodeEnumCount).CpuidFeatures());
		}

		[Fact]
		void InstructionInfoExtensions_FlowControl_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)(-1)).FlowControl());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)IcedConstants.CodeEnumCount).FlowControl());
		}

		[Fact]
		void InstructionInfoExtensions_IsPrivileged_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)(-1)).IsPrivileged());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)IcedConstants.CodeEnumCount).IsPrivileged());
		}

		[Fact]
		void InstructionInfoExtensions_IsStackInstruction_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)(-1)).IsStackInstruction());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)IcedConstants.CodeEnumCount).IsStackInstruction());
		}

		[Fact]
		void InstructionInfoExtensions_IsSaveRestoreInstruction_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)(-1)).IsSaveRestoreInstruction());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)IcedConstants.CodeEnumCount).IsSaveRestoreInstruction());
		}

		[Fact]
		void InstructionInfo_GetOpAccess_throws_if_invalid_input() {
			Instruction instr = default;
			instr.Code = Code.Nopd;
			var info = new InstructionInfoFactory().GetInfo(instr);
			Assert.Throws<ArgumentOutOfRangeException>(() => info.GetOpAccess(-1));
			Assert.Throws<ArgumentOutOfRangeException>(() => info.GetOpAccess(IcedConstants.MaxOpCount));
		}

		[Fact]
		void MemorySizeExtensions_GetInfo_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)(-1)).GetInfo());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)IcedConstants.MemorySizeEnumCount).GetInfo());
		}

		[Fact]
		void MemorySizeExtensions_GetSize_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)(-1)).GetSize());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)IcedConstants.MemorySizeEnumCount).GetSize());
		}

		[Fact]
		void MemorySizeExtensions_GetElementSize_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)(-1)).GetElementSize());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)IcedConstants.MemorySizeEnumCount).GetElementSize());
		}

		[Fact]
		void MemorySizeExtensions_GetElementType_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)(-1)).GetElementType());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)IcedConstants.MemorySizeEnumCount).GetElementType());
		}

		[Fact]
		void MemorySizeExtensions_GetElementTypeInfo_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)(-1)).GetElementTypeInfo());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)IcedConstants.MemorySizeEnumCount).GetElementTypeInfo());
		}

		[Fact]
		void MemorySizeExtensions_IsSigned_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)(-1)).IsSigned());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)IcedConstants.MemorySizeEnumCount).IsSigned());
		}

		[Fact]
		void MemorySizeExtensions_IsPacked_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)(-1)).IsPacked());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)IcedConstants.MemorySizeEnumCount).IsPacked());
		}

		[Fact]
		void MemorySizeExtensions_GetElementCount_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)(-1)).GetElementCount());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)IcedConstants.MemorySizeEnumCount).GetElementCount());
		}

		[Fact]
		void MemorySizeInfo_ctor_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => new MemorySizeInfo(MemorySize.Unknown, -1, 0, MemorySize.Unknown, false, false));
			Assert.Throws<ArgumentOutOfRangeException>(() => new MemorySizeInfo(MemorySize.Unknown, 0, -1, MemorySize.Unknown, false, false));
			Assert.Throws<ArgumentOutOfRangeException>(() => new MemorySizeInfo(MemorySize.Unknown, 0, 1, MemorySize.Unknown, false, false));
		}

		[Fact]
		void RegisterExtensions_GetInfo_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Register)(-1)).GetInfo());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Register)IcedConstants.RegisterEnumCount).GetInfo());
		}

		[Fact]
		void RegisterExtensions_GetBaseRegister_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Register)(-1)).GetBaseRegister());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Register)IcedConstants.RegisterEnumCount).GetBaseRegister());
		}

		[Fact]
		void RegisterExtensions_GetNumber_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Register)(-1)).GetNumber());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Register)IcedConstants.RegisterEnumCount).GetNumber());
		}

		[Fact]
		void RegisterExtensions_GetFullRegister_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Register)(-1)).GetFullRegister());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Register)IcedConstants.RegisterEnumCount).GetFullRegister());
		}

		[Fact]
		void RegisterExtensions_GetFullRegister32_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Register)(-1)).GetFullRegister32());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Register)IcedConstants.RegisterEnumCount).GetFullRegister32());
		}

		[Fact]
		void RegisterExtensions_GetSize_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Register)(-1)).GetSize());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Register)IcedConstants.RegisterEnumCount).GetSize());
		}
	}
}
#endif
