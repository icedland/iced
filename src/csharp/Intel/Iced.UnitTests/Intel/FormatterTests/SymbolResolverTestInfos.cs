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
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests {
	static class SymbolResolverTestInfos {
		public const int AllInfos_Length = 129;
		public static readonly SymbolInstructionInfo[] AllInfos = new SymbolInstructionInfo[AllInfos_Length] {
			new SymbolInstructionInfo(16, "70 00", Code.Jo_rel8_16, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(2, addressSize);
					Assert.Equal(DecoderConstants.DEFAULT_IP16 + 2, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(16, "70 00", Code.Jo_rel8_16, a => a.ShowSymbolAddress = true, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(2, addressSize);
					Assert.Equal(DecoderConstants.DEFAULT_IP16 + 2, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(16, "70 00", Code.Jo_rel8_16, a => a.ShowBranchSize = true, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(2, addressSize);
					Assert.Equal(DecoderConstants.DEFAULT_IP16 + 2, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(16, "70 00", Code.Jo_rel8_16, a => a.ShowBranchSize = false, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(2, addressSize);
					Assert.Equal(DecoderConstants.DEFAULT_IP16 + 2, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(32, "70 00", Code.Jo_rel8_32, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(4, addressSize);
					Assert.Equal(DecoderConstants.DEFAULT_IP32 + 2, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(32, "70 00", Code.Jo_rel8_32, a => a.ShowSymbolAddress = true, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(4, addressSize);
					Assert.Equal(DecoderConstants.DEFAULT_IP32 + 2, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(32, "70 00", Code.Jo_rel8_32, a => a.ShowBranchSize = true, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(4, addressSize);
					Assert.Equal(DecoderConstants.DEFAULT_IP32 + 2, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(32, "70 00", Code.Jo_rel8_32, a => a.ShowBranchSize = false, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(4, addressSize);
					Assert.Equal(DecoderConstants.DEFAULT_IP32 + 2, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "70 00", Code.Jo_rel8_64, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(DecoderConstants.DEFAULT_IP64 + 2, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "70 00", Code.Jo_rel8_64, a => a.ShowSymbolAddress = true, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(DecoderConstants.DEFAULT_IP64 + 2, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "70 00", Code.Jo_rel8_64, a => a.ShowBranchSize = true, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(DecoderConstants.DEFAULT_IP64 + 2, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "70 00", Code.Jo_rel8_64, a => a.ShowBranchSize = false, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(DecoderConstants.DEFAULT_IP64 + 2, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(16, "9A DCFE 5AA5", Code.Call_ptr1616, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(2, addressSize);
					if (operand == 0) {
						Assert.Equal(0xFEDCU, address);
						symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text));
					}
					else {
						Assert.Equal(1, operand);
						Assert.Equal(0xA55AU, address);
						symbol = new SymbolResult(address, new TextInfo("selsym", FormatterOutputTextKind.Text), SymbolFlags.Signed);
					}
					return true;
				},
			}),
			new SymbolInstructionInfo(16, "9A DCFE 5AA5", Code.Call_ptr1616, a => a.ShowSymbolAddress = true, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(2, addressSize);
					if (operand == 0) {
						Assert.Equal(0xFEDCU, address);
						symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("symbol", FormatterOutputTextKind.Text), new TextPart("more", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					}
					else {
						Assert.Equal(1, operand);
						Assert.Equal(0xA55AU, address);
						symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("selsym", FormatterOutputTextKind.Text), new TextPart("extra", FormatterOutputTextKind.Text) }));
					}
					return true;
				},
			}),
			new SymbolInstructionInfo(16, "9A DCFE 5AA5", Code.Call_ptr1616, a => a.ShowBranchSize = true, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(2, addressSize);
					if (operand == 0) {
						Assert.Equal(0xFEDCU, address);
						symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text));
						return true;
					}
					else {
						Assert.Equal(1, operand);
						Assert.Equal(0xA55AU, address);
						symbol = default;
						return false;
					}
				},
			}),
			new SymbolInstructionInfo(16, "9A DCFE 5AA5", Code.Call_ptr1616, a => a.ShowBranchSize = false, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(2, addressSize);
					if (operand == 0) {
						Assert.Equal(0xFEDCU, address);
						symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text));
						return true;
					}
					else {
						Assert.Equal(1, operand);
						Assert.Equal(0xA55AU, address);
						symbol = default;
						return false;
					}
				},
			}),
			new SymbolInstructionInfo(32, "9A 98BADCFE 5AA5", Code.Call_ptr1632, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					if (operand == 0) {
						Assert.Equal(0xFEDCBA98, address);
						Assert.Equal(4, addressSize);
						symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text));
					}
					else {
						Assert.Equal(1, operand);
						Assert.Equal(2, addressSize);
						Assert.Equal(0xA55AU, address);
						symbol = new SymbolResult(address, new TextInfo("selsym", FormatterOutputTextKind.Text), SymbolFlags.Signed);
					}
					return true;
				},
			}),
			new SymbolInstructionInfo(32, "9A 98BADCFE 5AA5", Code.Call_ptr1632, a => a.ShowSymbolAddress = true, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					if (operand == 0) {
						Assert.Equal(0xFEDCBA98, address);
						Assert.Equal(4, addressSize);
						symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("symbol", FormatterOutputTextKind.Text), new TextPart("more", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					}
					else {
						Assert.Equal(1, operand);
						Assert.Equal(2, addressSize);
						Assert.Equal(0xA55AU, address);
						symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("selsym", FormatterOutputTextKind.Text), new TextPart("extra", FormatterOutputTextKind.Text) }));
					}
					return true;
				},
			}),
			new SymbolInstructionInfo(32, "9A 98BADCFE 5AA5", Code.Call_ptr1632, a => a.ShowBranchSize = true, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					if (operand == 0) {
						Assert.Equal(0xFEDCBA98, address);
						Assert.Equal(4, addressSize);
						symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text));
						return true;
					}
					else {
						Assert.Equal(1, operand);
						Assert.Equal(2, addressSize);
						Assert.Equal(0xA55AU, address);
						symbol = default;
						return false;
					}
				},
			}),
			new SymbolInstructionInfo(32, "9A 98BADCFE 5AA5", Code.Call_ptr1632, a => a.ShowBranchSize = false, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					if (operand == 0) {
						Assert.Equal(0xFEDCBA98, address);
						Assert.Equal(4, addressSize);
						symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text));
						return true;
					}
					else {
						Assert.Equal(1, operand);
						Assert.Equal(2, addressSize);
						Assert.Equal(0xA55AU, address);
						symbol = default;
						return false;
					}
				},
			}),
			new SymbolInstructionInfo(64, "B1 A5", Code.Mov_r8_imm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(1, addressSize);
					Assert.Equal(0xA5UL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.Relative);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "B1 A5", Code.Mov_r8_imm8, a => a.ShowSymbolAddress = true, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(1, addressSize);
					Assert.Equal(0xA5UL, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Relative | SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "B1 A5", Code.Mov_r8_imm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(1, addressSize);
					Assert.Equal(0xA5UL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "B1 A5", Code.Mov_r8_imm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(1, addressSize);
					Assert.Equal(0xA5UL, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "C8 5AA5 A5", Code.Enterq_imm16_imm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					if (address == 0xA55A) {
						Assert.Equal(2, addressSize);
						symbol = default;
						return false;
					}
					else {
						Assert.Equal(1, addressSize);
						Assert.Equal(0xA5UL, address);
						symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.Relative);
						return true;
					}
				},
			}),
			new SymbolInstructionInfo(64, "C8 5AA5 A5", Code.Enterq_imm16_imm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					if (address == 0xA55A) {
						Assert.Equal(2, addressSize);
						symbol = default;
						return false;
					}
					else {
						Assert.Equal(1, addressSize);
						Assert.Equal(0xA5UL, address);
						symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Relative | SymbolFlags.Signed);
						return true;
					}
				},
			}),
			new SymbolInstructionInfo(64, "C8 5AA5 A5", Code.Enterq_imm16_imm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					if (address == 0xA55A) {
						Assert.Equal(2, addressSize);
						symbol = default;
						return false;
					}
					else {
						Assert.Equal(1, addressSize);
						Assert.Equal(0xA5UL, address);
						symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text));
						return true;
					}
				},
			}),
			new SymbolInstructionInfo(64, "C8 5AA5 A5", Code.Enterq_imm16_imm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					if (address == 0xA55A) {
						Assert.Equal(2, addressSize);
						symbol = default;
						return false;
					}
					else {
						Assert.Equal(1, addressSize);
						Assert.Equal(0xA5UL, address);
						symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
						return true;
					}
				},
			}),
			new SymbolInstructionInfo(64, "66 B9 5AA5", Code.Mov_r16_imm16, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(2, addressSize);
					Assert.Equal(0xA55AUL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.Relative);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "66 B9 5AA5", Code.Mov_r16_imm16, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(2, addressSize);
					Assert.Equal(0xA55AUL, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Relative | SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "66 B9 5AA5", Code.Mov_r16_imm16, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(2, addressSize);
					Assert.Equal(0xA55AUL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "66 B9 5AA5", Code.Mov_r16_imm16, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(2, addressSize);
					Assert.Equal(0xA55AUL, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "B9 98BADCFE", Code.Mov_r32_imm32, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(4, addressSize);
					Assert.Equal(0xFEDCBA98, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.Relative);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "B9 98BADCFE", Code.Mov_r32_imm32, a => a.ShowSymbolAddress = true, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(4, addressSize);
					Assert.Equal(0xFEDCBA98, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Relative | SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "B9 98BADCFE", Code.Mov_r32_imm32, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(4, addressSize);
					Assert.Equal(0xFEDCBA98, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "B9 98BADCFE", Code.Mov_r32_imm32, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(4, addressSize);
					Assert.Equal(0xFEDCBA98, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "48 B9 5AA5123498BADCFE", Code.Mov_r64_imm64, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(0xFEDCBA983412A55A, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.Relative);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "48 B9 5AA5123498BADCFE", Code.Mov_r64_imm64, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(0xFEDCBA983412A55A, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Relative | SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "48 B9 5AA5123498BADCFE", Code.Mov_r64_imm64, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(0xFEDCBA983412A55A, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "48 B9 5AA5123498BADCFE", Code.Mov_r64_imm64, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(0xFEDCBA983412A55A, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "CC", Code.Int3, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(1, addressSize);
					Assert.Equal(3UL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.Relative);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "CC", Code.Int3, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(1, addressSize);
					Assert.Equal(3UL, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Relative | SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "CC", Code.Int3, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(1, addressSize);
					Assert.Equal(3UL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "CC", Code.Int3, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(1, addressSize);
					Assert.Equal(3UL, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "66 83 C9 FF", Code.Or_rm16_imm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(2, addressSize);
					Assert.Equal(0xFFFFUL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.Relative);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "66 83 C9 FF", Code.Or_rm16_imm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(2, addressSize);
					Assert.Equal(0xFFFFUL, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Relative | SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "66 83 C9 FF", Code.Or_rm16_imm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(2, addressSize);
					Assert.Equal(0xFFFFUL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "66 83 C9 FF", Code.Or_rm16_imm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(2, addressSize);
					Assert.Equal(0xFFFFUL, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "83 C9 FF", Code.Or_rm32_imm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(4, addressSize);
					Assert.Equal(0xFFFFFFFFUL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.Relative);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "83 C9 FF", Code.Or_rm32_imm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(4, addressSize);
					Assert.Equal(0xFFFFFFFFUL, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Relative | SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "83 C9 FF", Code.Or_rm32_imm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(4, addressSize);
					Assert.Equal(0xFFFFFFFFUL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "83 C9 FF", Code.Or_rm32_imm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(4, addressSize);
					Assert.Equal(0xFFFFFFFFUL, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "48 83 C9 FF", Code.Or_rm64_imm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(0xFFFFFFFFFFFFFFFFUL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.Relative);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "48 83 C9 FF", Code.Or_rm64_imm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(0xFFFFFFFFFFFFFFFFUL, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Relative | SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "48 83 C9 FF", Code.Or_rm64_imm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(0xFFFFFFFFFFFFFFFFUL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "48 83 C9 FF", Code.Or_rm64_imm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(0xFFFFFFFFFFFFFFFFUL, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(16, "64 A4", Code.Movsb_m8_m8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(2, addressSize);
					Assert.Equal(0UL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(16, "64 A4", Code.Movsb_m8_m8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(2, addressSize);
					Assert.Equal(0UL, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(16, "64 A4", Code.Movsb_m8_m8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(2, addressSize);
					Assert.Equal(0UL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(16, "64 A4", Code.Movsb_m8_m8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(2, addressSize);
					Assert.Equal(0UL, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(32, "64 A4", Code.Movsb_m8_m8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(4, addressSize);
					Assert.Equal(0UL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(32, "64 A4", Code.Movsb_m8_m8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(4, addressSize);
					Assert.Equal(0UL, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(32, "64 A4", Code.Movsb_m8_m8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(4, addressSize);
					Assert.Equal(0UL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(32, "64 A4", Code.Movsb_m8_m8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(4, addressSize);
					Assert.Equal(0UL, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "64 A4", Code.Movsb_m8_m8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(0UL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "64 A4", Code.Movsb_m8_m8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(0UL, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "64 A4", Code.Movsb_m8_m8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(0UL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "64 A4", Code.Movsb_m8_m8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(0UL, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "A0 123456789ABCDEF0", Code.Mov_AL_moffs8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(0xF0DEBC9A78563412UL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "A0 123456789ABCDEF0", Code.Mov_AL_moffs8, a => a.ShowSymbolAddress = true, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(0xF0DEBC9A78563412UL, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "A0 123456789ABCDEF0", Code.Mov_AL_moffs8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(0xF0DEBC9A78563412UL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "A0 123456789ABCDEF0", Code.Mov_AL_moffs8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(0xF0DEBC9A78563412UL, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8A 0D 78563412", Code.Mov_r8_rm8, a => a.RipRelativeAddresses = false, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(DecoderConstants.DEFAULT_IP64 + 6 + 0x12345678, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.None);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "67 8A 0D 78563412", Code.Mov_r8_rm8, a => a.RipRelativeAddresses = false, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(4, addressSize);
					Assert.Equal((DecoderConstants.DEFAULT_IP64 + 7 + 0x12345678) & 0xFFFFFFFFUL, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8A 0D 78563412", Code.Mov_r8_rm8, a => a.RipRelativeAddresses = true, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(DecoderConstants.DEFAULT_IP64 + 6 + 0x12345678, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "67 8A 0D 78563412", Code.Mov_r8_rm8, a => a.RipRelativeAddresses = true, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(4, addressSize);
					Assert.Equal((DecoderConstants.DEFAULT_IP64 + 7 + 0x12345678) & 0xFFFFFFFFUL, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(16, "8A 06 FFFF", Code.Mov_r8_rm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(2, addressSize);
					Assert.Equal(0xFFFFUL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.None);
					return true;
				},
			}),
			new SymbolInstructionInfo(16, "8A 06 FFFF", Code.Mov_r8_rm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(2, addressSize);
					Assert.Equal(0xFFFFUL, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(16, "8A 06 FFFF", Code.Mov_r8_rm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(2, addressSize);
					Assert.Equal(0xFFFFUL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(16, "8A 06 FFFF", Code.Mov_r8_rm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(2, addressSize);
					Assert.Equal(0xFFFFUL, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(32, "8A 05 FFFFFFFF", Code.Mov_r8_rm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(4, addressSize);
					Assert.Equal(0xFFFFFFFFUL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.None);
					return true;
				},
			}),
			new SymbolInstructionInfo(32, "8A 05 FFFFFFFF", Code.Mov_r8_rm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(4, addressSize);
					Assert.Equal(0xFFFFFFFFUL, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(32, "8A 05 FFFFFFFF", Code.Mov_r8_rm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(4, addressSize);
					Assert.Equal(0xFFFFFFFFUL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(32, "8A 05 FFFFFFFF", Code.Mov_r8_rm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(4, addressSize);
					Assert.Equal(0xFFFFFFFFUL, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8A 04 25 FFFFFFFF", Code.Mov_r8_rm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(0xFFFFFFFFFFFFFFFFUL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.None);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8A 04 25 FFFFFFFF", Code.Mov_r8_rm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(0xFFFFFFFFFFFFFFFFUL, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8A 04 25 FFFFFFFF", Code.Mov_r8_rm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(0xFFFFFFFFFFFFFFFFUL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8A 04 25 FFFFFFFF", Code.Mov_r8_rm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(0xFFFFFFFFFFFFFFFFUL, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8A 00", Code.Mov_r8_rm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(0UL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.None);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8A 00", Code.Mov_r8_rm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(0UL, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8A 00", Code.Mov_r8_rm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(0UL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8A 00", Code.Mov_r8_rm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(0UL, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8A 40 A5", Code.Mov_r8_rm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(0xFFFFFFFFFFFFFFA5UL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.None);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8A 40 A5", Code.Mov_r8_rm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(0xFFFFFFFFFFFFFFA5UL, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8A 40 A5", Code.Mov_r8_rm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(0xFFFFFFFFFFFFFFA5UL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8A 40 A5", Code.Mov_r8_rm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(0xFFFFFFFFFFFFFFA5UL, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8A 40 A5", Code.Mov_r8_rm8, a => a.SpaceBetweenMemoryAddOperators = true, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(0xFFFFFFFFFFFFFFA5UL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.None);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8A 40 A5", Code.Mov_r8_rm8, a => a.SpaceBetweenMemoryAddOperators = true, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(0xFFFFFFFFFFFFFFA5UL, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8A 40 A5", Code.Mov_r8_rm8, a => a.SpaceBetweenMemoryAddOperators = false, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(0xFFFFFFFFFFFFFFA5UL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.None);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8A 40 A5", Code.Mov_r8_rm8, a => a.SpaceBetweenMemoryAddOperators = false, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(0xFFFFFFFFFFFFFFA5UL, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(16, "8A 80 5AA5", Code.Mov_r8_rm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(2, addressSize);
					Assert.Equal(0xA55AUL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.None);
					return true;
				},
			}),
			new SymbolInstructionInfo(16, "8A 80 5AA5", Code.Mov_r8_rm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(2, addressSize);
					Assert.Equal(0xA55AUL, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(16, "8A 80 5AA5", Code.Mov_r8_rm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(2, addressSize);
					Assert.Equal(0xA55AUL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.None);
					return true;
				},
			}),
			new SymbolInstructionInfo(16, "8A 80 5AA5", Code.Mov_r8_rm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(2, addressSize);
					Assert.Equal(0xA55AUL, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(32, "8A 80 88A9CBED", Code.Mov_r8_rm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(4, addressSize);
					Assert.Equal(0xEDCBA988UL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.None);
					return true;
				},
			}),
			new SymbolInstructionInfo(32, "8A 80 88A9CBED", Code.Mov_r8_rm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(4, addressSize);
					Assert.Equal(0xEDCBA988UL, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(32, "8A 80 88A9CBED", Code.Mov_r8_rm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(4, addressSize);
					Assert.Equal(0xEDCBA988UL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.None);
					return true;
				},
			}),
			new SymbolInstructionInfo(32, "8A 80 88A9CBED", Code.Mov_r8_rm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(4, addressSize);
					Assert.Equal(0xEDCBA988UL, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8A 80 88A9CBED", Code.Mov_r8_rm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(0xFFFFFFFFEDCBA988UL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.None);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8A 80 88A9CBED", Code.Mov_r8_rm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(0xFFFFFFFFEDCBA988UL, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8A 80 88A9CBED", Code.Mov_r8_rm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(0xFFFFFFFFEDCBA988UL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.None);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8A 80 88A9CBED", Code.Mov_r8_rm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(0xFFFFFFFFEDCBA988UL, address);
					symbol = new SymbolResult(address, new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8A 40 A5", Code.Mov_r8_rm8, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(0xFFFFFFFFFFFFFFA5UL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.None);
					return false;
				},
			}),
			new SymbolInstructionInfo(64, "8B 80 78563412", Code.Mov_r32_rm32, a => a.MemorySizeOptions = MemorySizeOptions.Always, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(0x0000000012345678UL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.None, MemorySize.UInt32);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8B 80 78563412", Code.Mov_r32_rm32, a => a.MemorySizeOptions = MemorySizeOptions.Always, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(0x0000000012345678UL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.None, MemorySize.UInt8);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8B 80 78563412", Code.Mov_r32_rm32, a => a.MemorySizeOptions = MemorySizeOptions.Never, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(0x0000000012345678UL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.None, MemorySize.UInt32);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8B 80 78563412", Code.Mov_r32_rm32, a => a.MemorySizeOptions = MemorySizeOptions.Never, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(0x0000000012345678UL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.None, MemorySize.UInt8);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8B 80 78563412", Code.Mov_r32_rm32, a => a.MemorySizeOptions = MemorySizeOptions.Default, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(0x0000000012345678UL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.None, MemorySize.UInt32);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8B 80 78563412", Code.Mov_r32_rm32, a => a.MemorySizeOptions = MemorySizeOptions.Default, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(0x0000000012345678UL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.None, MemorySize.UInt8);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8B 80 78563412", Code.Mov_r32_rm32, a => a.MemorySizeOptions = MemorySizeOptions.Minimum, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(0x0000000012345678UL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.None, MemorySize.UInt32);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8B 80 78563412", Code.Mov_r32_rm32, a => a.MemorySizeOptions = MemorySizeOptions.Minimum, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(0x0000000012345678UL, address);
					symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.None, MemorySize.UInt8);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "C7 00 78563412", Code.Mov_rm32_imm32, a => a.MemorySizeOptions = MemorySizeOptions.Always, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					if (instructionOperand == 0) {
						symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.None, MemorySize.UInt32);
						return true;
					}
					symbol = default;
					return false;
				},
			}),
			new SymbolInstructionInfo(64, "C7 00 78563412", Code.Mov_rm32_imm32, a => a.MemorySizeOptions = MemorySizeOptions.Always, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					if (instructionOperand == 0) {
						symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.None, MemorySize.UInt8);
						return true;
					}
					symbol = default;
					return false;
				},
			}),
			new SymbolInstructionInfo(64, "C7 00 78563412", Code.Mov_rm32_imm32, a => a.MemorySizeOptions = MemorySizeOptions.Never, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					if (instructionOperand == 0) {
						symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.None, MemorySize.UInt32);
						return true;
					}
					symbol = default;
					return false;
				},
			}),
			new SymbolInstructionInfo(64, "C7 00 78563412", Code.Mov_rm32_imm32, a => a.MemorySizeOptions = MemorySizeOptions.Never, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					if (instructionOperand == 0) {
						symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.None, MemorySize.UInt8);
						return true;
					}
					symbol = default;
					return false;
				},
			}),
			new SymbolInstructionInfo(64, "C7 00 78563412", Code.Mov_rm32_imm32, a => a.MemorySizeOptions = MemorySizeOptions.Default, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					if (instructionOperand == 0) {
						symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.None, MemorySize.UInt32);
						return true;
					}
					symbol = default;
					return false;
				},
			}),
			new SymbolInstructionInfo(64, "C7 00 78563412", Code.Mov_rm32_imm32, a => a.MemorySizeOptions = MemorySizeOptions.Default, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					if (instructionOperand == 0) {
						symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.None, MemorySize.UInt8);
						return true;
					}
					symbol = default;
					return false;
				},
			}),
			new SymbolInstructionInfo(64, "C7 00 78563412", Code.Mov_rm32_imm32, a => a.MemorySizeOptions = MemorySizeOptions.Minimum, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					if (instructionOperand == 0) {
						symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.None, MemorySize.UInt32);
						return true;
					}
					symbol = default;
					return false;
				},
			}),
			new SymbolInstructionInfo(64, "C7 00 78563412", Code.Mov_rm32_imm32, a => a.MemorySizeOptions = MemorySizeOptions.Minimum, new TestSymbolResolver {
				tryGetSymbol = (in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) => {
					if (instructionOperand == 0) {
						symbol = new SymbolResult(address, new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.None, MemorySize.UInt8);
						return true;
					}
					symbol = default;
					return false;
				},
			}),
		};
	}
}
#endif
