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

#if (!NO_GAS_FORMATTER || !NO_INTEL_FORMATTER || !NO_MASM_FORMATTER || !NO_NASM_FORMATTER) && !NO_FORMATTER
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Iced.Intel;
using Iced.UnitTests.Intel.DecoderTests;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests {
	public sealed class MiscTests {
		[Fact]
		void Make_sure_all_Code_values_are_formatted() {
			var tested = new byte[IcedConstants.NumberOfCodeValues];

			var allArgs = new (int bitness, bool isMisc)[] {
				(16, false),
				(32, false),
				(64, false),
				(16, true),
				(32, true),
				(64, true),
			};
			foreach (var args in allArgs) {
				var infos = FormatterTest.GetInstructionInfos(args.bitness, args.isMisc);
				foreach (var info in infos)
					tested[(int)info.Code] = 1;
			}
#if !NO_ENCODER
			foreach (var info in NonDecodedInstructions.GetTests())
				tested[(int)info.instruction.Code] = 1;
#endif

			var sb = new StringBuilder();
			int missing = 0;
			var codeNames = ToEnumConverter.GetCodeNames().ToArray();
			for (int i = 0; i < tested.Length; i++) {
				if (tested[i] != 1) {
					sb.Append(codeNames[i] + " ");
					missing++;
				}
			}
			Assert.Equal("Fmt: 0 ins ", $"Fmt: {missing} ins " + sb.ToString());
		}

		[Fact]
		void Test_FormatterOperandOptions_properties() {
			FormatterOperandOptions options = default;

			options.MemorySizeOptions = MemorySizeOptions.Always;
			Assert.Equal(MemorySizeOptions.Always, options.MemorySizeOptions);

			options.MemorySizeOptions = MemorySizeOptions.Minimum;
			Assert.Equal(MemorySizeOptions.Minimum, options.MemorySizeOptions);

			options.MemorySizeOptions = MemorySizeOptions.Never;
			Assert.Equal(MemorySizeOptions.Never, options.MemorySizeOptions);

			options.MemorySizeOptions = MemorySizeOptions.Default;
			Assert.Equal(MemorySizeOptions.Default, options.MemorySizeOptions);

			options.BranchSize = true;
			Assert.True(options.BranchSize);
			options.BranchSize = false;
			Assert.False(options.BranchSize);

			options.RipRelativeAddresses = true;
			Assert.True(options.RipRelativeAddresses);
			options.RipRelativeAddresses = false;
			Assert.False(options.RipRelativeAddresses);
		}

		[Fact]
		void NumberFormattingOptions_ctor_throws_if_null_options() {
			Assert.Throws<ArgumentNullException>(() => new NumberFormattingOptions(null, false, false, false));
		}

		public static IEnumerable<object[]> AllFormatters {
			get {
				var result = new List<object[]>();
				foreach (var formatter in Utils.GetAllFormatters())
					result.Add(new object[] { formatter });
				return result.ToArray();
			}
		}

		[Theory]
		[MemberData(nameof(AllFormatters))]
		void Methods_throw_if_null_input(Formatter formatter) {
			var instr = Instruction.Create(Code.Mov_rm64_r64, Register.RAX, Register.RCX);
			Assert.Throws<ArgumentNullException>(() => formatter.FormatMnemonic(instr, null));
			Assert.Throws<ArgumentNullException>(() => formatter.FormatMnemonic(instr, null, FormatMnemonicOptions.None));
			Assert.Throws<ArgumentNullException>(() => formatter.FormatOperand(instr, null, 0));
			Assert.Throws<ArgumentNullException>(() => formatter.FormatOperandSeparator(instr, null));
			Assert.Throws<ArgumentNullException>(() => formatter.FormatAllOperands(instr, null));
			Assert.Throws<ArgumentNullException>(() => formatter.Format(instr, null));
		}

		[Theory]
		[MemberData(nameof(AllFormatters))]
		void Methods_throw_if_invalid_operand_or_instructionOperand(Formatter formatter) {
			{
				var instr = Instruction.Create(Code.Mov_rm64_r64, Register.RAX, Register.RCX);
				const int numOps = 2;
				const int numInstrOps = 2;
				Assert.Equal(numOps, formatter.GetOperandCount(instr));
				Assert.Equal(numInstrOps, instr.OpCount);
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.TryGetOpAccess(instr, -1, out _));
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.TryGetOpAccess(instr, numOps, out _));
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetInstructionOperand(instr, -1));
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetInstructionOperand(instr, numOps));
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetFormatterOperand(instr, -1));
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetFormatterOperand(instr, numInstrOps));
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.FormatOperand(instr, new StringBuilderFormatterOutput(), -1));
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.FormatOperand(instr, new StringBuilderFormatterOutput(), numOps));
			}

			{
				Instruction invalid = default;
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.TryGetOpAccess(invalid, -1, out _));
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.TryGetOpAccess(invalid, 0, out _));
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetInstructionOperand(invalid, -1));
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetInstructionOperand(invalid, 0));
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetFormatterOperand(invalid, -1));
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetFormatterOperand(invalid, 0));
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.FormatOperand(invalid, new StringBuilderFormatterOutput(), -1));
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.FormatOperand(invalid, new StringBuilderFormatterOutput(), 0));
			}

			{
				var db = Instruction.CreateDeclareByte(new byte[8]);
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.TryGetOpAccess(db, -1, out _));
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetInstructionOperand(db, -1));
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetFormatterOperand(db, -1));
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.FormatOperand(db, new StringBuilderFormatterOutput(), -1));
				Assert.Equal(8, db.DeclareDataCount);
				for (int i = 0; i < db.DeclareDataCount; i++) {
					formatter.TryGetOpAccess(db, i, out _);
					formatter.GetInstructionOperand(db, i);
					formatter.FormatOperand(db, new StringBuilderFormatterOutput(), i);
				}
				for (int i = db.DeclareDataCount; i < 17; i++) {
					Assert.Throws<ArgumentOutOfRangeException>(() => formatter.TryGetOpAccess(db, i, out _));
					Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetInstructionOperand(db, i));
					Assert.Throws<ArgumentOutOfRangeException>(() => formatter.FormatOperand(db, new StringBuilderFormatterOutput(), i));
				}
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetFormatterOperand(db, 0));
			}

			{
				var dw = Instruction.CreateDeclareWord(new byte[8]);
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.TryGetOpAccess(dw, -1, out _));
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetInstructionOperand(dw, -1));
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetFormatterOperand(dw, -1));
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.FormatOperand(dw, new StringBuilderFormatterOutput(), -1));
				Assert.Equal(4, dw.DeclareDataCount);
				for (int i = 0; i < dw.DeclareDataCount; i++) {
					formatter.TryGetOpAccess(dw, i, out _);
					formatter.GetInstructionOperand(dw, i);
					formatter.FormatOperand(dw, new StringBuilderFormatterOutput(), i);
				}
				for (int i = dw.DeclareDataCount; i < 17; i++) {
					Assert.Throws<ArgumentOutOfRangeException>(() => formatter.TryGetOpAccess(dw, i, out _));
					Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetInstructionOperand(dw, i));
					Assert.Throws<ArgumentOutOfRangeException>(() => formatter.FormatOperand(dw, new StringBuilderFormatterOutput(), i));
				}
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetFormatterOperand(dw, 0));
			}

			{
				var dd = Instruction.CreateDeclareDword(new byte[8]);
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.TryGetOpAccess(dd, -1, out _));
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetInstructionOperand(dd, -1));
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetFormatterOperand(dd, -1));
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.FormatOperand(dd, new StringBuilderFormatterOutput(), -1));
				Assert.Equal(2, dd.DeclareDataCount);
				for (int i = 0; i < dd.DeclareDataCount; i++) {
					formatter.TryGetOpAccess(dd, i, out _);
					formatter.GetInstructionOperand(dd, i);
					formatter.FormatOperand(dd, new StringBuilderFormatterOutput(), i);
				}
				for (int i = dd.DeclareDataCount; i < 17; i++) {
					Assert.Throws<ArgumentOutOfRangeException>(() => formatter.TryGetOpAccess(dd, i, out _));
					Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetInstructionOperand(dd, i));
					Assert.Throws<ArgumentOutOfRangeException>(() => formatter.FormatOperand(dd, new StringBuilderFormatterOutput(), i));
				}
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetFormatterOperand(dd, 0));
			}

			{
				var dq = Instruction.CreateDeclareQword(new byte[8]);
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.TryGetOpAccess(dq, -1, out _));
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetInstructionOperand(dq, -1));
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetFormatterOperand(dq, -1));
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.FormatOperand(dq, new StringBuilderFormatterOutput(), -1));
				Assert.Equal(1, dq.DeclareDataCount);
				for (int i = 0; i < dq.DeclareDataCount; i++) {
					formatter.TryGetOpAccess(dq, i, out _);
					formatter.GetInstructionOperand(dq, i);
					formatter.FormatOperand(dq, new StringBuilderFormatterOutput(), i);
				}
				for (int i = dq.DeclareDataCount; i < 17; i++) {
					Assert.Throws<ArgumentOutOfRangeException>(() => formatter.TryGetOpAccess(dq, i, out _));
					Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetInstructionOperand(dq, i));
					Assert.Throws<ArgumentOutOfRangeException>(() => formatter.FormatOperand(dq, new StringBuilderFormatterOutput(), i));
				}
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetFormatterOperand(dq, 0));
			}
		}

		[Fact]
		void StringBuilderFormatterOutput_throws_if_invalid_input() {
			Assert.Throws<ArgumentNullException>(() => new StringBuilderFormatterOutput(null));
		}

		[Fact]
		void StringBuilderFormatterOutput_uses_input_sb() {
			var sb = new StringBuilder();
			sb.Append("Text");
			var output = new StringBuilderFormatterOutput(sb);
			output.Write("hello", FormatterOutputTextKind.Text);
			Assert.Equal("Texthello", sb.ToString());
			Assert.Equal("Texthello", output.ToString());
		}
	}
}
#endif
