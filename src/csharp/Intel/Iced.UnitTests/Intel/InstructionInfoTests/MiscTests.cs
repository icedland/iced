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

#if !NO_INSTR_INFO
using System;
using System.Collections.Generic;
using Iced.Intel;
using Iced.UnitTests.Intel.DecoderTests;
using Xunit;

namespace Iced.UnitTests.Intel.InstructionInfoTests {
	public sealed class MiscTests {
		[Fact]
		void IsBranchCall() {
			var jccShort = MiscTestsData.JccShort;
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

			for (int i = 0; i < IcedConstants.NumberOfCodeValues; i++) {
				var code = (Code)i;
				Instruction instr = default;
				instr.Code = code;

				Assert.Equal(jccShort.Contains(code) || jccNear.Contains(code), code.IsJccShortOrNear());
				Assert.Equal(code.IsJccShortOrNear(), instr.IsJccShortOrNear);

				Assert.Equal(jccNear.Contains(code), code.IsJccNear());
				Assert.Equal(code.IsJccNear(), instr.IsJccNear);

				Assert.Equal(jccShort.Contains(code), code.IsJccShort());
				Assert.Equal(code.IsJccShort(), instr.IsJccShort);

				Assert.Equal(jmpShort.Contains(code), code.IsJmpShort());
				Assert.Equal(code.IsJmpShort(), instr.IsJmpShort);

				Assert.Equal(jmpNear.Contains(code), code.IsJmpNear());
				Assert.Equal(code.IsJmpNear(), instr.IsJmpNear);

				Assert.Equal(jmpShort.Contains(code) || jmpNear.Contains(code), code.IsJmpShortOrNear());
				Assert.Equal(code.IsJmpShortOrNear(), instr.IsJmpShortOrNear);

				Assert.Equal(jmpFar.Contains(code), code.IsJmpFar());
				Assert.Equal(code.IsJmpFar(), instr.IsJmpFar);

				Assert.Equal(callNear.Contains(code), code.IsCallNear());
				Assert.Equal(code.IsCallNear(), instr.IsCallNear);

				Assert.Equal(callFar.Contains(code), code.IsCallFar());
				Assert.Equal(code.IsCallFar(), instr.IsCallFar);

				Assert.Equal(jmpNearIndirect.Contains(code), code.IsJmpNearIndirect());
				Assert.Equal(code.IsJmpNearIndirect(), instr.IsJmpNearIndirect);

				Assert.Equal(jmpFarIndirect.Contains(code), code.IsJmpFarIndirect());
				Assert.Equal(code.IsJmpFarIndirect(), instr.IsJmpFarIndirect);

				Assert.Equal(callNearIndirect.Contains(code), code.IsCallNearIndirect());
				Assert.Equal(code.IsCallNearIndirect(), instr.IsCallNearIndirect);

				Assert.Equal(callFarIndirect.Contains(code), code.IsCallFarIndirect());
				Assert.Equal(code.IsCallFarIndirect(), instr.IsCallFarIndirect);
			}
		}

		[Fact]
		void Verify_ProtectedMode_is_true_if_VEX_XOP_EVEX() {
			foreach (var info in DecoderTestUtils.GetDecoderTests(includeOtherTests: false, includeInvalid: false)) {
				var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(info.HexBytes), info.Options);
				decoder.Decode(out var instr);
				switch (instr.Encoding) {
				case EncodingKind.Legacy:
				case EncodingKind.D3NOW:
					break;
				case EncodingKind.VEX:
				case EncodingKind.EVEX:
				case EncodingKind.XOP:
					Assert.True(instr.IsProtectedMode);
					Assert.True(info.Code.IsProtectedMode());
					break;
				default:
					throw new InvalidOperationException();
				}
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

			for (int i = 0; i < IcedConstants.NumberOfCodeValues; i++) {
				var code = (Code)i;
				Instruction instr = default;
				instr.Code = code;

				if (!toNegatedCodeValue.TryGetValue(code, out var negatedCodeValue))
					negatedCodeValue = code;

				Assert.Equal(negatedCodeValue, code.NegateConditionCode());
				instr.NegateConditionCode();
				Assert.Equal(negatedCodeValue, instr.Code);
			}
		}

		[Fact]
		void Verify_ToShortBranch() {
			var toShortBranch = new Dictionary<Code, Code>();
			foreach (var info in MiscTestsData.JccNearInfos)
				toShortBranch.Add(info.jcc, info.jccShort);
			foreach (var info in MiscTestsData.JmpInfos)
				toShortBranch.Add(info.jmpNear, info.jmpShort);

			for (int i = 0; i < IcedConstants.NumberOfCodeValues; i++) {
				var code = (Code)i;
				Instruction instr = default;
				instr.Code = code;

				if (!toShortBranch.TryGetValue(code, out var shortCodeValue))
					shortCodeValue = code;

				Assert.Equal(shortCodeValue, code.ToShortBranch());
				instr.ToShortBranch();
				Assert.Equal(shortCodeValue, instr.Code);
			}
		}

		[Fact]
		void Verify_ToNearBranch() {
			var toNearBranch = new Dictionary<Code, Code>();
			foreach (var info in MiscTestsData.JccShortInfos)
				toNearBranch.Add(info.jcc, info.jccNear);
			foreach (var info in MiscTestsData.JmpInfos)
				toNearBranch.Add(info.jmpShort, info.jmpNear);

			for (int i = 0; i < IcedConstants.NumberOfCodeValues; i++) {
				var code = (Code)i;
				Instruction instr = default;
				instr.Code = code;

				if (!toNearBranch.TryGetValue(code, out var nearCodeValue))
					nearCodeValue = code;

				Assert.Equal(nearCodeValue, code.ToNearBranch());
				instr.ToNearBranch();
				Assert.Equal(nearCodeValue, instr.Code);
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

			for (int i = 0; i < IcedConstants.NumberOfCodeValues; i++) {
				var code = (Code)i;
				Instruction instr = default;
				instr.Code = code;

				if (!toConditionCode.TryGetValue(code, out var cc))
					cc = ConditionCode.None;

				Assert.Equal(cc, code.ConditionCode());
				Assert.Equal(cc, instr.ConditionCode);
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
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)IcedConstants.NumberOfCodeValues).Encoding());
		}

		[Fact]
		void InstructionInfoExtensions_CpuidFeatures_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)(-1)).CpuidFeatures());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)IcedConstants.NumberOfCodeValues).CpuidFeatures());
		}

		[Fact]
		void InstructionInfoExtensions_FlowControl_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)(-1)).FlowControl());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)IcedConstants.NumberOfCodeValues).FlowControl());
		}

		[Fact]
		void InstructionInfoExtensions_IsProtectedMode_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)(-1)).IsProtectedMode());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)IcedConstants.NumberOfCodeValues).IsProtectedMode());
		}

		[Fact]
		void InstructionInfoExtensions_IsPrivileged_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)(-1)).IsPrivileged());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)IcedConstants.NumberOfCodeValues).IsPrivileged());
		}

		[Fact]
		void InstructionInfoExtensions_IsStackInstruction_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)(-1)).IsStackInstruction());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)IcedConstants.NumberOfCodeValues).IsStackInstruction());
		}

		[Fact]
		void InstructionInfoExtensions_IsSaveRestoreInstruction_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)(-1)).IsSaveRestoreInstruction());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)IcedConstants.NumberOfCodeValues).IsSaveRestoreInstruction());
		}

		[Fact]
		void InstructionInfo_GetOpAccess_throws_if_invalid_input() {
			var info = Instruction.Create(Code.Nopd).GetInfo();
			Assert.Throws<ArgumentOutOfRangeException>(() => info.GetOpAccess(-1));
			Assert.Throws<ArgumentOutOfRangeException>(() => info.GetOpAccess(IcedConstants.MaxOpCount));
		}

		[Fact]
		void MemorySizeExtensions_GetInfo_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)(-1)).GetInfo());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)IcedConstants.NumberOfMemorySizes).GetInfo());
		}

		[Fact]
		void MemorySizeExtensions_GetSize_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)(-1)).GetSize());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)IcedConstants.NumberOfMemorySizes).GetSize());
		}

		[Fact]
		void MemorySizeExtensions_GetElementSize_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)(-1)).GetElementSize());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)IcedConstants.NumberOfMemorySizes).GetElementSize());
		}

		[Fact]
		void MemorySizeExtensions_GetElementType_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)(-1)).GetElementType());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)IcedConstants.NumberOfMemorySizes).GetElementType());
		}

		[Fact]
		void MemorySizeExtensions_GetElementTypeInfo_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)(-1)).GetElementTypeInfo());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)IcedConstants.NumberOfMemorySizes).GetElementTypeInfo());
		}

		[Fact]
		void MemorySizeExtensions_IsSigned_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)(-1)).IsSigned());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)IcedConstants.NumberOfMemorySizes).IsSigned());
		}

		[Fact]
		void MemorySizeExtensions_IsPacked_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)(-1)).IsPacked());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)IcedConstants.NumberOfMemorySizes).IsPacked());
		}

		[Fact]
		void MemorySizeExtensions_GetElementCount_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)(-1)).GetElementCount());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)IcedConstants.NumberOfMemorySizes).GetElementCount());
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
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Register)IcedConstants.NumberOfRegisters).GetInfo());
		}

		[Fact]
		void RegisterExtensions_GetBaseRegister_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Register)(-1)).GetBaseRegister());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Register)IcedConstants.NumberOfRegisters).GetBaseRegister());
		}

		[Fact]
		void RegisterExtensions_GetNumber_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Register)(-1)).GetNumber());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Register)IcedConstants.NumberOfRegisters).GetNumber());
		}

		[Fact]
		void RegisterExtensions_GetFullRegister_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Register)(-1)).GetFullRegister());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Register)IcedConstants.NumberOfRegisters).GetFullRegister());
		}

		[Fact]
		void RegisterExtensions_GetFullRegister32_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Register)(-1)).GetFullRegister32());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Register)IcedConstants.NumberOfRegisters).GetFullRegister32());
		}

		[Fact]
		void RegisterExtensions_GetSize_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Register)(-1)).GetSize());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Register)IcedConstants.NumberOfRegisters).GetSize());
		}
	}
}
#endif
