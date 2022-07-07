// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if GAS || INTEL || MASM || NASM
using System;
using System.Collections.Generic;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests {
	public sealed class MiscTests {
		[Fact]
		void Test_FormatterOperandOptions_properties() {
			FormatterOperandOptions options = default;

			options.MemorySizeOptions = MemorySizeOptions.Always;
			Assert.Equal(MemorySizeOptions.Always, options.MemorySizeOptions);

			options.MemorySizeOptions = MemorySizeOptions.Minimal;
			Assert.Equal(MemorySizeOptions.Minimal, options.MemorySizeOptions);

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
			Instruction instruction = default;
			instruction.Code = Code.Mov_rm64_r64;
			instruction.Op0Register = Register.RAX;
			instruction.Op1Register = Register.RCX;
			Assert.Throws<ArgumentNullException>(() => formatter.FormatMnemonic(instruction, null));
			Assert.Throws<ArgumentNullException>(() => formatter.FormatMnemonic(instruction, null, FormatMnemonicOptions.None));
			Assert.Throws<ArgumentNullException>(() => formatter.FormatOperand(instruction, null, 0));
			Assert.Throws<ArgumentNullException>(() => formatter.FormatOperandSeparator(instruction, null));
			Assert.Throws<ArgumentNullException>(() => formatter.FormatAllOperands(instruction, null));
			Assert.Throws<ArgumentNullException>(() => formatter.Format(instruction, null));
		}

		[Theory]
		[MemberData(nameof(AllFormatters))]
		void Methods_throw_if_invalid_operand_or_instructionOperand(Formatter formatter) {
			{
				Instruction instruction = default;
				instruction.Code = Code.Mov_rm64_r64;
				instruction.Op0Register = Register.RAX;
				instruction.Op1Register = Register.RCX;
				const int numOps = 2;
				const int numInstrOps = 2;
				Assert.Equal(numOps, formatter.GetOperandCount(instruction));
				Assert.Equal(numInstrOps, instruction.OpCount);
#if INSTR_INFO
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.TryGetOpAccess(instruction, -1, out _));
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.TryGetOpAccess(instruction, numOps, out _));
#endif
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetInstructionOperand(instruction, -1));
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetInstructionOperand(instruction, numOps));
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetFormatterOperand(instruction, -1));
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetFormatterOperand(instruction, numInstrOps));
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.FormatOperand(instruction, new StringOutput(), -1));
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.FormatOperand(instruction, new StringOutput(), numOps));
			}

			{
				Instruction invalid = default;
#if INSTR_INFO
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.TryGetOpAccess(invalid, -1, out _));
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.TryGetOpAccess(invalid, 0, out _));
#endif
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetInstructionOperand(invalid, -1));
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetInstructionOperand(invalid, 0));
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetFormatterOperand(invalid, -1));
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetFormatterOperand(invalid, 0));
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.FormatOperand(invalid, new StringOutput(), -1));
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.FormatOperand(invalid, new StringOutput(), 0));
			}

#if ENCODER
			{
				var db = Instruction.CreateDeclareByte(new byte[8]);
#if INSTR_INFO
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.TryGetOpAccess(db, -1, out _));
#endif
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetInstructionOperand(db, -1));
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetFormatterOperand(db, -1));
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.FormatOperand(db, new StringOutput(), -1));
				Assert.Equal(8, db.DeclareDataCount);
				for (int i = 0; i < db.DeclareDataCount; i++) {
#if INSTR_INFO
					formatter.TryGetOpAccess(db, i, out _);
#endif
					formatter.GetInstructionOperand(db, i);
					formatter.FormatOperand(db, new StringOutput(), i);
				}
				for (int i = db.DeclareDataCount; i < 17; i++) {
#if INSTR_INFO
					Assert.Throws<ArgumentOutOfRangeException>(() => formatter.TryGetOpAccess(db, i, out _));
#endif
					Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetInstructionOperand(db, i));
					Assert.Throws<ArgumentOutOfRangeException>(() => formatter.FormatOperand(db, new StringOutput(), i));
				}
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetFormatterOperand(db, 0));
			}
#endif

#if ENCODER
			{
				var dw = Instruction.CreateDeclareWord(new byte[8]);
#if INSTR_INFO
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.TryGetOpAccess(dw, -1, out _));
#endif
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetInstructionOperand(dw, -1));
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetFormatterOperand(dw, -1));
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.FormatOperand(dw, new StringOutput(), -1));
				Assert.Equal(4, dw.DeclareDataCount);
				for (int i = 0; i < dw.DeclareDataCount; i++) {
#if INSTR_INFO
					formatter.TryGetOpAccess(dw, i, out _);
#endif
					formatter.GetInstructionOperand(dw, i);
					formatter.FormatOperand(dw, new StringOutput(), i);
				}
				for (int i = dw.DeclareDataCount; i < 17; i++) {
#if INSTR_INFO
					Assert.Throws<ArgumentOutOfRangeException>(() => formatter.TryGetOpAccess(dw, i, out _));
#endif
					Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetInstructionOperand(dw, i));
					Assert.Throws<ArgumentOutOfRangeException>(() => formatter.FormatOperand(dw, new StringOutput(), i));
				}
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetFormatterOperand(dw, 0));
			}
#endif

#if ENCODER
			{
				var dd = Instruction.CreateDeclareDword(new byte[8]);
#if INSTR_INFO
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.TryGetOpAccess(dd, -1, out _));
#endif
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetInstructionOperand(dd, -1));
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetFormatterOperand(dd, -1));
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.FormatOperand(dd, new StringOutput(), -1));
				Assert.Equal(2, dd.DeclareDataCount);
				for (int i = 0; i < dd.DeclareDataCount; i++) {
#if INSTR_INFO
					formatter.TryGetOpAccess(dd, i, out _);
#endif
					formatter.GetInstructionOperand(dd, i);
					formatter.FormatOperand(dd, new StringOutput(), i);
				}
				for (int i = dd.DeclareDataCount; i < 17; i++) {
#if INSTR_INFO
					Assert.Throws<ArgumentOutOfRangeException>(() => formatter.TryGetOpAccess(dd, i, out _));
#endif
					Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetInstructionOperand(dd, i));
					Assert.Throws<ArgumentOutOfRangeException>(() => formatter.FormatOperand(dd, new StringOutput(), i));
				}
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetFormatterOperand(dd, 0));
			}
#endif

#if ENCODER
			{
				var dq = Instruction.CreateDeclareQword(new byte[8]);
#if INSTR_INFO
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.TryGetOpAccess(dq, -1, out _));
#endif
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetInstructionOperand(dq, -1));
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetFormatterOperand(dq, -1));
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.FormatOperand(dq, new StringOutput(), -1));
				Assert.Equal(1, dq.DeclareDataCount);
				for (int i = 0; i < dq.DeclareDataCount; i++) {
#if INSTR_INFO
					formatter.TryGetOpAccess(dq, i, out _);
#endif
					formatter.GetInstructionOperand(dq, i);
					formatter.FormatOperand(dq, new StringOutput(), i);
				}
				for (int i = dq.DeclareDataCount; i < 17; i++) {
#if INSTR_INFO
					Assert.Throws<ArgumentOutOfRangeException>(() => formatter.TryGetOpAccess(dq, i, out _));
#endif
					Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetInstructionOperand(dq, i));
					Assert.Throws<ArgumentOutOfRangeException>(() => formatter.FormatOperand(dq, new StringOutput(), i));
				}
				Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetFormatterOperand(dq, 0));
			}
#endif
		}

		[Fact]
		void StringOutput_throws_if_invalid_input() {
			Assert.Throws<ArgumentNullException>(() => new StringOutput(null));
		}

		[Fact]
		void StringOutput_uses_input_sb() {
			var sb = new System.Text.StringBuilder();
			sb.Append("Text");
			var output = new StringOutput(sb);
			output.Write("hello", FormatterTextKind.Text);
			Assert.Equal("Texthello", sb.ToString());
			Assert.Equal("Texthello", output.ToString());
		}

		[Fact]
		void Verify_default_formatter_options() {
			var options = new FormatterOptions();
			Assert.False(options.UppercasePrefixes);
			Assert.False(options.UppercaseMnemonics);
			Assert.False(options.UppercaseRegisters);
			Assert.False(options.UppercaseKeywords);
			Assert.False(options.UppercaseDecorators);
			Assert.False(options.UppercaseAll);
			Assert.Equal(0, options.FirstOperandCharIndex);
			Assert.Equal(0, options.TabSize);
			Assert.False(options.SpaceAfterOperandSeparator);
			Assert.False(options.SpaceAfterMemoryBracket);
			Assert.False(options.SpaceBetweenMemoryAddOperators);
			Assert.False(options.SpaceBetweenMemoryMulOperators);
			Assert.False(options.ScaleBeforeIndex);
			Assert.False(options.AlwaysShowScale);
			Assert.False(options.AlwaysShowSegmentRegister);
			Assert.False(options.ShowZeroDisplacements);
			Assert.Null(options.HexPrefix);
			Assert.Null(options.HexSuffix);
			Assert.Equal(4, options.HexDigitGroupSize);
			Assert.Null(options.DecimalPrefix);
			Assert.Null(options.DecimalSuffix);
			Assert.Equal(3, options.DecimalDigitGroupSize);
			Assert.Null(options.OctalPrefix);
			Assert.Null(options.OctalSuffix);
			Assert.Equal(4, options.OctalDigitGroupSize);
			Assert.Null(options.BinaryPrefix);
			Assert.Null(options.BinarySuffix);
			Assert.Equal(4, options.BinaryDigitGroupSize);
			Assert.Null(options.DigitSeparator);
			Assert.False(options.LeadingZeros);
			Assert.False(options.LeadingZeroes);
			Assert.True(options.UppercaseHex);
			Assert.True(options.SmallHexNumbersInDecimal);
			Assert.True(options.AddLeadingZeroToHexNumbers);
			Assert.Equal(NumberBase.Hexadecimal, options.NumberBase);
			Assert.True(options.BranchLeadingZeros);
			Assert.True(options.BranchLeadingZeroes);
			Assert.False(options.SignedImmediateOperands);
			Assert.True(options.SignedMemoryDisplacements);
			Assert.False(options.DisplacementLeadingZeros);
			Assert.False(options.DisplacementLeadingZeroes);
			Assert.Equal(MemorySizeOptions.Default, options.MemorySizeOptions);
			Assert.False(options.RipRelativeAddresses);
			Assert.True(options.ShowBranchSize);
			Assert.True(options.UsePseudoOps);
			Assert.False(options.ShowSymbolAddress);
			Assert.False(options.PreferST0);
			Assert.Equal(CC_b.b, options.CC_b);
			Assert.Equal(CC_ae.ae, options.CC_ae);
			Assert.Equal(CC_e.e, options.CC_e);
			Assert.Equal(CC_ne.ne, options.CC_ne);
			Assert.Equal(CC_be.be, options.CC_be);
			Assert.Equal(CC_a.a, options.CC_a);
			Assert.Equal(CC_p.p, options.CC_p);
			Assert.Equal(CC_np.np, options.CC_np);
			Assert.Equal(CC_l.l, options.CC_l);
			Assert.Equal(CC_ge.ge, options.CC_ge);
			Assert.Equal(CC_le.le, options.CC_le);
			Assert.Equal(CC_g.g, options.CC_g);
			Assert.False(options.ShowUselessPrefixes);
			Assert.False(options.GasNakedRegisters);
			Assert.False(options.GasShowMnemonicSizeSuffix);
			Assert.False(options.GasSpaceAfterMemoryOperandComma);
			Assert.True(options.MasmAddDsPrefix32);
			Assert.True(options.MasmSymbolDisplInBrackets);
			Assert.True(options.MasmDisplInBrackets);
			Assert.False(options.NasmShowSignExtendedImmediateSize);
		}

		[Fact]
		void Throws_if_invalid_CC_value() {
			Assert.Throws<ArgumentOutOfRangeException>(() => new FormatterOptions().CC_b = (CC_b)IcedConstants.CC_b_EnumCount);
			Assert.Throws<ArgumentOutOfRangeException>(() => new FormatterOptions().CC_ae = (CC_ae)IcedConstants.CC_ae_EnumCount);
			Assert.Throws<ArgumentOutOfRangeException>(() => new FormatterOptions().CC_e = (CC_e)IcedConstants.CC_e_EnumCount);
			Assert.Throws<ArgumentOutOfRangeException>(() => new FormatterOptions().CC_ne = (CC_ne)IcedConstants.CC_ne_EnumCount);
			Assert.Throws<ArgumentOutOfRangeException>(() => new FormatterOptions().CC_be = (CC_be)IcedConstants.CC_be_EnumCount);
			Assert.Throws<ArgumentOutOfRangeException>(() => new FormatterOptions().CC_a = (CC_a)IcedConstants.CC_a_EnumCount);
			Assert.Throws<ArgumentOutOfRangeException>(() => new FormatterOptions().CC_p = (CC_p)IcedConstants.CC_p_EnumCount);
			Assert.Throws<ArgumentOutOfRangeException>(() => new FormatterOptions().CC_np = (CC_np)IcedConstants.CC_np_EnumCount);
			Assert.Throws<ArgumentOutOfRangeException>(() => new FormatterOptions().CC_l = (CC_l)IcedConstants.CC_l_EnumCount);
			Assert.Throws<ArgumentOutOfRangeException>(() => new FormatterOptions().CC_ge = (CC_ge)IcedConstants.CC_ge_EnumCount);
			Assert.Throws<ArgumentOutOfRangeException>(() => new FormatterOptions().CC_le = (CC_le)IcedConstants.CC_le_EnumCount);
			Assert.Throws<ArgumentOutOfRangeException>(() => new FormatterOptions().CC_g = (CC_g)IcedConstants.CC_g_EnumCount);
		}
	}
}
#endif
