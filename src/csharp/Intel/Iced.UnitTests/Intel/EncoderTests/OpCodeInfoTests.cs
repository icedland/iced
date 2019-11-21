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

#if !NO_ENCODER
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
			Assert.Equal(tc.CanBroadcast, info.CanBroadcast);
			Assert.Equal(tc.CanUseRoundingControl, info.CanUseRoundingControl);
			Assert.Equal(tc.CanSuppressAllExceptions, info.CanSuppressAllExceptions);
			Assert.Equal(tc.CanUseOpMaskRegister, info.CanUseOpMaskRegister);
			Assert.Equal(tc.RequireNonZeroOpMaskRegister, info.RequireNonZeroOpMaskRegister);
			if (tc.RequireNonZeroOpMaskRegister) {
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
			Assert.Equal(tc.Table, info.Table);
			Assert.Equal(tc.MandatoryPrefix, info.MandatoryPrefix);
			Assert.Equal(tc.OpCode, info.OpCode);
			Assert.Equal(tc.IsGroup, info.IsGroup);
			Assert.Equal(tc.GroupIndex, info.GroupIndex);
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
			for (int i = 0; i < IcedConstants.NumberOfCodeValues; i++) {
				var code = (Code)i;
				Instruction instr = default;
				instr.Code = (Code)i;
				Assert.True(ReferenceEquals(code.ToOpCode(), instr.OpCode));
			}
		}

		[Fact]
		void Make_sure_all_Code_values_are_tested_exactly_once() {
			var tested = new bool[IcedConstants.NumberOfCodeValues];
			foreach (var info in OpCodeInfoTestCases.OpCodeInfoTests) {
				Assert.False(tested[(int)info.Code]);
				tested[(int)info.Code] = true;
			}
			var sb = new StringBuilder();
			for (int i = 0; i < tested.Length; i++) {
				if (!tested[i]) {
					if (sb.Length > 0)
						sb.Append(',');
					sb.Append(((Code)i).ToString());
				}
			}
			Assert.Equal(string.Empty, sb.ToString());
		}
	}
}
#endif
