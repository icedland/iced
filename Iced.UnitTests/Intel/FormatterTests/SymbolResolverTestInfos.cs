/*
    Copyright (C) 2018 de4dot@gmail.com

    This file is part of Iced.

    Iced is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Iced is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Iced.  If not, see <https://www.gnu.org/licenses/>.
*/

#if (!NO_GAS_FORMATTER || !NO_INTEL_FORMATTER || !NO_MASM_FORMATTER || !NO_NASM_FORMATTER) && !NO_FORMATTER
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests {
	static class SymbolResolverTestInfos {
		public const int AllInfos_Length = 113;
		public static readonly SymbolInstructionInfo[] AllInfos = new SymbolInstructionInfo[AllInfos_Length] {
			new SymbolInstructionInfo(16, "70 00", Code.Jo_Jb16, new TestSymbolResolver {
				tryGetBranchSymbol = (ulong address, int addressSize, out SymbolResult symbol, ref bool showBranchSize, ref NumberFormattingOptions options) => {
					Assert.Equal(2, addressSize);
					Assert.Equal(DecoderConstants.DEFAULT_IP16 + 2, address);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(16, "70 00", Code.Jo_Jb16, new TestSymbolResolver {
				tryGetBranchSymbol = (ulong address, int addressSize, out SymbolResult symbol, ref bool showBranchSize, ref NumberFormattingOptions options) => {
					Assert.Equal(2, addressSize);
					Assert.Equal(DecoderConstants.DEFAULT_IP16 + 2, address);
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(16, "70 00", Code.Jo_Jb16, a => a.ShowBranchSize = true, new TestSymbolResolver {
				tryGetBranchSymbol = (ulong address, int addressSize, out SymbolResult symbol, ref bool showBranchSize, ref NumberFormattingOptions options) => {
					Assert.Equal(2, addressSize);
					Assert.Equal(DecoderConstants.DEFAULT_IP16 + 2, address);
					Assert.True(showBranchSize);
					showBranchSize = false;
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(16, "70 00", Code.Jo_Jb16, a => a.ShowBranchSize = false, new TestSymbolResolver {
				tryGetBranchSymbol = (ulong address, int addressSize, out SymbolResult symbol, ref bool showBranchSize, ref NumberFormattingOptions options) => {
					Assert.Equal(2, addressSize);
					Assert.Equal(DecoderConstants.DEFAULT_IP16 + 2, address);
					Assert.False(showBranchSize);
					showBranchSize = true;
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(32, "70 00", Code.Jo_Jb32, new TestSymbolResolver {
				tryGetBranchSymbol = (ulong address, int addressSize, out SymbolResult symbol, ref bool showBranchSize, ref NumberFormattingOptions options) => {
					Assert.Equal(4, addressSize);
					Assert.Equal(DecoderConstants.DEFAULT_IP32 + 2, address);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(32, "70 00", Code.Jo_Jb32, new TestSymbolResolver {
				tryGetBranchSymbol = (ulong address, int addressSize, out SymbolResult symbol, ref bool showBranchSize, ref NumberFormattingOptions options) => {
					Assert.Equal(4, addressSize);
					Assert.Equal(DecoderConstants.DEFAULT_IP32 + 2, address);
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(32, "70 00", Code.Jo_Jb32, a => a.ShowBranchSize = true, new TestSymbolResolver {
				tryGetBranchSymbol = (ulong address, int addressSize, out SymbolResult symbol, ref bool showBranchSize, ref NumberFormattingOptions options) => {
					Assert.Equal(4, addressSize);
					Assert.Equal(DecoderConstants.DEFAULT_IP32 + 2, address);
					Assert.True(showBranchSize);
					showBranchSize = false;
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(32, "70 00", Code.Jo_Jb32, a => a.ShowBranchSize = false, new TestSymbolResolver {
				tryGetBranchSymbol = (ulong address, int addressSize, out SymbolResult symbol, ref bool showBranchSize, ref NumberFormattingOptions options) => {
					Assert.Equal(4, addressSize);
					Assert.Equal(DecoderConstants.DEFAULT_IP32 + 2, address);
					Assert.False(showBranchSize);
					showBranchSize = true;
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "70 00", Code.Jo_Jb64, new TestSymbolResolver {
				tryGetBranchSymbol = (ulong address, int addressSize, out SymbolResult symbol, ref bool showBranchSize, ref NumberFormattingOptions options) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(DecoderConstants.DEFAULT_IP64 + 2, address);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "70 00", Code.Jo_Jb64, new TestSymbolResolver {
				tryGetBranchSymbol = (ulong address, int addressSize, out SymbolResult symbol, ref bool showBranchSize, ref NumberFormattingOptions options) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(DecoderConstants.DEFAULT_IP64 + 2, address);
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "70 00", Code.Jo_Jb64, a => a.ShowBranchSize = true, new TestSymbolResolver {
				tryGetBranchSymbol = (ulong address, int addressSize, out SymbolResult symbol, ref bool showBranchSize, ref NumberFormattingOptions options) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(DecoderConstants.DEFAULT_IP64 + 2, address);
					Assert.True(showBranchSize);
					showBranchSize = false;
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "70 00", Code.Jo_Jb64, a => a.ShowBranchSize = false, new TestSymbolResolver {
				tryGetBranchSymbol = (ulong address, int addressSize, out SymbolResult symbol, ref bool showBranchSize, ref NumberFormattingOptions options) => {
					Assert.Equal(8, addressSize);
					Assert.Equal(DecoderConstants.DEFAULT_IP64 + 2, address);
					Assert.False(showBranchSize);
					showBranchSize = true;
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(16, "9A DCFE 5AA5", Code.Call_Aww, new TestSymbolResolver {
				tryGetFarBranchSymbol = (ushort selector, uint address, int addressSize, out SymbolResult symbolSelector, out SymbolResult symbol, ref bool showBranchSize, ref NumberFormattingOptions options) => {
					Assert.Equal(2, addressSize);
					Assert.Equal(0xA55A, selector);
					Assert.Equal(0xFEDCU, address);
					symbolSelector = new SymbolResult(new TextInfo("selsym", FormatterOutputTextKind.Text), SymbolFlags.Signed);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(16, "9A DCFE 5AA5", Code.Call_Aww, new TestSymbolResolver {
				tryGetFarBranchSymbol = (ushort selector, uint address, int addressSize, out SymbolResult symbolSelector, out SymbolResult symbol, ref bool showBranchSize, ref NumberFormattingOptions options) => {
					Assert.Equal(2, addressSize);
					Assert.Equal(0xA55A, selector);
					Assert.Equal(0xFEDCU, address);
					symbolSelector = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("selsym", FormatterOutputTextKind.Text), new TextPart("extra", FormatterOutputTextKind.Text) }));
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("symbol", FormatterOutputTextKind.Text), new TextPart("more", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(16, "9A DCFE 5AA5", Code.Call_Aww, a => a.ShowBranchSize = true, new TestSymbolResolver {
				tryGetFarBranchSymbol = (ushort selector, uint address, int addressSize, out SymbolResult symbolSelector, out SymbolResult symbol, ref bool showBranchSize, ref NumberFormattingOptions options) => {
					Assert.Equal(2, addressSize);
					Assert.Equal(0xA55A, selector);
					Assert.Equal(0xFEDCU, address);
					Assert.True(showBranchSize);
					symbolSelector = default;
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(16, "9A DCFE 5AA5", Code.Call_Aww, a => a.ShowBranchSize = false, new TestSymbolResolver {
				tryGetFarBranchSymbol = (ushort selector, uint address, int addressSize, out SymbolResult symbolSelector, out SymbolResult symbol, ref bool showBranchSize, ref NumberFormattingOptions options) => {
					Assert.Equal(2, addressSize);
					Assert.Equal(0xA55A, selector);
					Assert.Equal(0xFEDCU, address);
					Assert.False(showBranchSize);
					symbolSelector = default;
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(32, "9A 98BADCFE 5AA5", Code.Call_Adw, new TestSymbolResolver {
				tryGetFarBranchSymbol = (ushort selector, uint address, int addressSize, out SymbolResult symbolSelector, out SymbolResult symbol, ref bool showBranchSize, ref NumberFormattingOptions options) => {
					Assert.Equal(4, addressSize);
					Assert.Equal(0xA55A, selector);
					Assert.Equal(0xFEDCBA98, address);
					symbolSelector = new SymbolResult(new TextInfo("selsym", FormatterOutputTextKind.Text), SymbolFlags.Signed);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(32, "9A 98BADCFE 5AA5", Code.Call_Adw, new TestSymbolResolver {
				tryGetFarBranchSymbol = (ushort selector, uint address, int addressSize, out SymbolResult symbolSelector, out SymbolResult symbol, ref bool showBranchSize, ref NumberFormattingOptions options) => {
					Assert.Equal(4, addressSize);
					Assert.Equal(0xA55A, selector);
					Assert.Equal(0xFEDCBA98, address);
					symbolSelector = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("selsym", FormatterOutputTextKind.Text), new TextPart("extra", FormatterOutputTextKind.Text) }));
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("symbol", FormatterOutputTextKind.Text), new TextPart("more", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(32, "9A 98BADCFE 5AA5", Code.Call_Adw, a => a.ShowBranchSize = true, new TestSymbolResolver {
				tryGetFarBranchSymbol = (ushort selector, uint address, int addressSize, out SymbolResult symbolSelector, out SymbolResult symbol, ref bool showBranchSize, ref NumberFormattingOptions options) => {
					Assert.Equal(4, addressSize);
					Assert.Equal(0xA55A, selector);
					Assert.Equal(0xFEDCBA98, address);
					Assert.True(showBranchSize);
					symbolSelector = default;
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(32, "9A 98BADCFE 5AA5", Code.Call_Adw, a => a.ShowBranchSize = false, new TestSymbolResolver {
				tryGetFarBranchSymbol = (ushort selector, uint address, int addressSize, out SymbolResult symbolSelector, out SymbolResult symbol, ref bool showBranchSize, ref NumberFormattingOptions options) => {
					Assert.Equal(4, addressSize);
					Assert.Equal(0xA55A, selector);
					Assert.Equal(0xFEDCBA98, address);
					Assert.False(showBranchSize);
					symbolSelector = default;
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "B1 A5", Code.Mov_CL_Ib, new TestSymbolResolver {
				tryGetImmediateSymbol = (ulong immediate, int immediateSize, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(1, immediateSize);
					Assert.Equal(0xA5UL, immediate);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "B1 A5", Code.Mov_CL_Ib, new TestSymbolResolver {
				tryGetImmediateSymbol = (ulong immediate, int immediateSize, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(1, immediateSize);
					Assert.Equal(0xA5UL, immediate);
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "B1 A5", Code.Mov_CL_Ib, new TestSymbolResolver {
				tryGetImmediateSymbol = (ulong immediate, int immediateSize, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(1, immediateSize);
					Assert.Equal(0xA5UL, immediate);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.Address);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "B1 A5", Code.Mov_CL_Ib, new TestSymbolResolver {
				tryGetImmediateSymbol = (ulong immediate, int immediateSize, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(1, immediateSize);
					Assert.Equal(0xA5UL, immediate);
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Address | SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "C8 5AA5 A5", Code.Enterq_Iw_Ib, new TestSymbolResolver {
				tryGetImmediateSymbol = (ulong immediate, int immediateSize, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					if (immediate == 0xA55A) {
						Assert.Equal(2, immediateSize);
						symbol = default;
						return false;
					}
					else {
						Assert.Equal(1, immediateSize);
						Assert.Equal(0xA5UL, immediate);
						symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text));
						return true;
					}
				},
			}),
			new SymbolInstructionInfo(64, "C8 5AA5 A5", Code.Enterq_Iw_Ib, new TestSymbolResolver {
				tryGetImmediateSymbol = (ulong immediate, int immediateSize, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					if (immediate == 0xA55A) {
						Assert.Equal(2, immediateSize);
						symbol = default;
						return false;
					}
					else {
						Assert.Equal(1, immediateSize);
						Assert.Equal(0xA5UL, immediate);
						symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
						return true;
					}
				},
			}),
			new SymbolInstructionInfo(64, "C8 5AA5 A5", Code.Enterq_Iw_Ib, new TestSymbolResolver {
				tryGetImmediateSymbol = (ulong immediate, int immediateSize, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					if (immediate == 0xA55A) {
						Assert.Equal(2, immediateSize);
						symbol = default;
						return false;
					}
					else {
						Assert.Equal(1, immediateSize);
						Assert.Equal(0xA5UL, immediate);
						symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.Address);
						return true;
					}
				},
			}),
			new SymbolInstructionInfo(64, "C8 5AA5 A5", Code.Enterq_Iw_Ib, new TestSymbolResolver {
				tryGetImmediateSymbol = (ulong immediate, int immediateSize, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					if (immediate == 0xA55A) {
						Assert.Equal(2, immediateSize);
						symbol = default;
						return false;
					}
					else {
						Assert.Equal(1, immediateSize);
						Assert.Equal(0xA5UL, immediate);
						symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Address | SymbolFlags.Signed);
						return true;
					}
				},
			}),
			new SymbolInstructionInfo(64, "66 B9 5AA5", Code.Mov_CX_Iw, new TestSymbolResolver {
				tryGetImmediateSymbol = (ulong immediate, int immediateSize, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(2, immediateSize);
					Assert.Equal(0xA55AUL, immediate);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "66 B9 5AA5", Code.Mov_CX_Iw, new TestSymbolResolver {
				tryGetImmediateSymbol = (ulong immediate, int immediateSize, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(2, immediateSize);
					Assert.Equal(0xA55AUL, immediate);
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "66 B9 5AA5", Code.Mov_CX_Iw, new TestSymbolResolver {
				tryGetImmediateSymbol = (ulong immediate, int immediateSize, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(2, immediateSize);
					Assert.Equal(0xA55AUL, immediate);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.Address);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "66 B9 5AA5", Code.Mov_CX_Iw, new TestSymbolResolver {
				tryGetImmediateSymbol = (ulong immediate, int immediateSize, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(2, immediateSize);
					Assert.Equal(0xA55AUL, immediate);
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Address | SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "B9 98BADCFE", Code.Mov_ECX_Id, new TestSymbolResolver {
				tryGetImmediateSymbol = (ulong immediate, int immediateSize, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(4, immediateSize);
					Assert.Equal(0xFEDCBA98, immediate);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "B9 98BADCFE", Code.Mov_ECX_Id, new TestSymbolResolver {
				tryGetImmediateSymbol = (ulong immediate, int immediateSize, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(4, immediateSize);
					Assert.Equal(0xFEDCBA98, immediate);
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "B9 98BADCFE", Code.Mov_ECX_Id, new TestSymbolResolver {
				tryGetImmediateSymbol = (ulong immediate, int immediateSize, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(4, immediateSize);
					Assert.Equal(0xFEDCBA98, immediate);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.Address);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "B9 98BADCFE", Code.Mov_ECX_Id, new TestSymbolResolver {
				tryGetImmediateSymbol = (ulong immediate, int immediateSize, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(4, immediateSize);
					Assert.Equal(0xFEDCBA98, immediate);
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Address | SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "48 B9 5AA5123498BADCFE", Code.Mov_RCX_Iq, new TestSymbolResolver {
				tryGetImmediateSymbol = (ulong immediate, int immediateSize, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(8, immediateSize);
					Assert.Equal(0xFEDCBA983412A55A, immediate);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "48 B9 5AA5123498BADCFE", Code.Mov_RCX_Iq, new TestSymbolResolver {
				tryGetImmediateSymbol = (ulong immediate, int immediateSize, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(8, immediateSize);
					Assert.Equal(0xFEDCBA983412A55A, immediate);
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "48 B9 5AA5123498BADCFE", Code.Mov_RCX_Iq, new TestSymbolResolver {
				tryGetImmediateSymbol = (ulong immediate, int immediateSize, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(8, immediateSize);
					Assert.Equal(0xFEDCBA983412A55A, immediate);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.Address);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "48 B9 5AA5123498BADCFE", Code.Mov_RCX_Iq, new TestSymbolResolver {
				tryGetImmediateSymbol = (ulong immediate, int immediateSize, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(8, immediateSize);
					Assert.Equal(0xFEDCBA983412A55A, immediate);
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Address | SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "CC", Code.Int3, new TestSymbolResolver {
				tryGetImmediateSymbol = (ulong immediate, int immediateSize, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(1, immediateSize);
					Assert.Equal(3UL, immediate);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "CC", Code.Int3, new TestSymbolResolver {
				tryGetImmediateSymbol = (ulong immediate, int immediateSize, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(1, immediateSize);
					Assert.Equal(3UL, immediate);
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "CC", Code.Int3, new TestSymbolResolver {
				tryGetImmediateSymbol = (ulong immediate, int immediateSize, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(1, immediateSize);
					Assert.Equal(3UL, immediate);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.Address);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "CC", Code.Int3, new TestSymbolResolver {
				tryGetImmediateSymbol = (ulong immediate, int immediateSize, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(1, immediateSize);
					Assert.Equal(3UL, immediate);
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Address | SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "66 83 C9 FF", Code.Or_Ew_Ib16, new TestSymbolResolver {
				tryGetImmediateSymbol = (ulong immediate, int immediateSize, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(2, immediateSize);
					Assert.Equal(0xFFFFUL, immediate);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "66 83 C9 FF", Code.Or_Ew_Ib16, new TestSymbolResolver {
				tryGetImmediateSymbol = (ulong immediate, int immediateSize, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(2, immediateSize);
					Assert.Equal(0xFFFFUL, immediate);
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "66 83 C9 FF", Code.Or_Ew_Ib16, new TestSymbolResolver {
				tryGetImmediateSymbol = (ulong immediate, int immediateSize, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(2, immediateSize);
					Assert.Equal(0xFFFFUL, immediate);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.Address);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "66 83 C9 FF", Code.Or_Ew_Ib16, new TestSymbolResolver {
				tryGetImmediateSymbol = (ulong immediate, int immediateSize, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(2, immediateSize);
					Assert.Equal(0xFFFFUL, immediate);
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Address | SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "83 C9 FF", Code.Or_Ed_Ib32, new TestSymbolResolver {
				tryGetImmediateSymbol = (ulong immediate, int immediateSize, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(4, immediateSize);
					Assert.Equal(0xFFFFFFFFUL, immediate);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "83 C9 FF", Code.Or_Ed_Ib32, new TestSymbolResolver {
				tryGetImmediateSymbol = (ulong immediate, int immediateSize, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(4, immediateSize);
					Assert.Equal(0xFFFFFFFFUL, immediate);
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "83 C9 FF", Code.Or_Ed_Ib32, new TestSymbolResolver {
				tryGetImmediateSymbol = (ulong immediate, int immediateSize, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(4, immediateSize);
					Assert.Equal(0xFFFFFFFFUL, immediate);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.Address);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "83 C9 FF", Code.Or_Ed_Ib32, new TestSymbolResolver {
				tryGetImmediateSymbol = (ulong immediate, int immediateSize, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(4, immediateSize);
					Assert.Equal(0xFFFFFFFFUL, immediate);
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Address | SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "48 83 C9 FF", Code.Or_Eq_Ib64, new TestSymbolResolver {
				tryGetImmediateSymbol = (ulong immediate, int immediateSize, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(8, immediateSize);
					Assert.Equal(0xFFFFFFFFFFFFFFFFUL, immediate);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "48 83 C9 FF", Code.Or_Eq_Ib64, new TestSymbolResolver {
				tryGetImmediateSymbol = (ulong immediate, int immediateSize, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(8, immediateSize);
					Assert.Equal(0xFFFFFFFFFFFFFFFFUL, immediate);
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "48 83 C9 FF", Code.Or_Eq_Ib64, new TestSymbolResolver {
				tryGetImmediateSymbol = (ulong immediate, int immediateSize, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(8, immediateSize);
					Assert.Equal(0xFFFFFFFFFFFFFFFFUL, immediate);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.Address);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "48 83 C9 FF", Code.Or_Eq_Ib64, new TestSymbolResolver {
				tryGetImmediateSymbol = (ulong immediate, int immediateSize, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(8, immediateSize);
					Assert.Equal(0xFFFFFFFFFFFFFFFFUL, immediate);
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Address | SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(16, "64 A4", Code.Movsb_Yb_Xb, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(2, displacementSize);
					Assert.Equal(0UL, displacement);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(16, "64 A4", Code.Movsb_Yb_Xb, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(2, displacementSize);
					Assert.Equal(0UL, displacement);
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(16, "64 A4", Code.Movsb_Yb_Xb, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(2, displacementSize);
					Assert.Equal(0UL, displacement);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.Address);
					return true;
				},
			}),
			new SymbolInstructionInfo(16, "64 A4", Code.Movsb_Yb_Xb, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(2, displacementSize);
					Assert.Equal(0UL, displacement);
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Address | SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(32, "64 A4", Code.Movsb_Yb_Xb, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(4, displacementSize);
					Assert.Equal(0UL, displacement);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(32, "64 A4", Code.Movsb_Yb_Xb, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(4, displacementSize);
					Assert.Equal(0UL, displacement);
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(32, "64 A4", Code.Movsb_Yb_Xb, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(4, displacementSize);
					Assert.Equal(0UL, displacement);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.Address);
					return true;
				},
			}),
			new SymbolInstructionInfo(32, "64 A4", Code.Movsb_Yb_Xb, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(4, displacementSize);
					Assert.Equal(0UL, displacement);
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Address | SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "64 A4", Code.Movsb_Yb_Xb, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(8, displacementSize);
					Assert.Equal(0UL, displacement);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "64 A4", Code.Movsb_Yb_Xb, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(8, displacementSize);
					Assert.Equal(0UL, displacement);
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "64 A4", Code.Movsb_Yb_Xb, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(8, displacementSize);
					Assert.Equal(0UL, displacement);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.Address);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "64 A4", Code.Movsb_Yb_Xb, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(8, displacementSize);
					Assert.Equal(0UL, displacement);
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Address | SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "A0 123456789ABCDEF0", Code.Mov_AL_Ob, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(8, displacementSize);
					Assert.Equal(0xF0DEBC9A78563412UL, displacement);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text));
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "A0 123456789ABCDEF0", Code.Mov_AL_Ob, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(8, displacementSize);
					Assert.Equal(0xF0DEBC9A78563412UL, displacement);
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "A0 123456789ABCDEF0", Code.Mov_AL_Ob, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(8, displacementSize);
					Assert.Equal(0xF0DEBC9A78563412UL, displacement);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.Address);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "A0 123456789ABCDEF0", Code.Mov_AL_Ob, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(8, displacementSize);
					Assert.Equal(0xF0DEBC9A78563412UL, displacement);
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Address | SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8A 0D 78563412", Code.Mov_Gb_Eb, a => a.RipRelativeAddresses = false, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(8, displacementSize);
					Assert.Equal(DecoderConstants.DEFAULT_IP64 + 6 + 0x12345678, displacement);
					Assert.False(ripRelativeAddresses);
					ripRelativeAddresses = true;
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.None);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "67 8A 0D 78563412", Code.Mov_Gb_Eb, a => a.RipRelativeAddresses = false, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(4, displacementSize);
					Assert.Equal((DecoderConstants.DEFAULT_IP64 + 7 + 0x12345678) & 0xFFFFFFFFUL, displacement);
					Assert.False(ripRelativeAddresses);
					ripRelativeAddresses = true;
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8A 0D 78563412", Code.Mov_Gb_Eb, a => a.RipRelativeAddresses = true, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(8, displacementSize);
					Assert.Equal(DecoderConstants.DEFAULT_IP64 + 6 + 0x12345678, displacement);
					Assert.True(ripRelativeAddresses);
					ripRelativeAddresses = false;
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.Address);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "67 8A 0D 78563412", Code.Mov_Gb_Eb, a => a.RipRelativeAddresses = true, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(4, displacementSize);
					Assert.Equal((DecoderConstants.DEFAULT_IP64 + 7 + 0x12345678) & 0xFFFFFFFFUL, displacement);
					Assert.True(ripRelativeAddresses);
					ripRelativeAddresses = false;
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Address | SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(16, "8A 06 FFFF", Code.Mov_Gb_Eb, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(2, displacementSize);
					Assert.Equal(0xFFFFUL, displacement);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.None);
					return true;
				},
			}),
			new SymbolInstructionInfo(16, "8A 06 FFFF", Code.Mov_Gb_Eb, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(2, displacementSize);
					Assert.Equal(0xFFFFUL, displacement);
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(16, "8A 06 FFFF", Code.Mov_Gb_Eb, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(2, displacementSize);
					Assert.Equal(0xFFFFUL, displacement);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.Address);
					return true;
				},
			}),
			new SymbolInstructionInfo(16, "8A 06 FFFF", Code.Mov_Gb_Eb, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(2, displacementSize);
					Assert.Equal(0xFFFFUL, displacement);
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Address | SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(32, "8A 05 FFFFFFFF", Code.Mov_Gb_Eb, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(4, displacementSize);
					Assert.Equal(0xFFFFFFFFUL, displacement);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.None);
					return true;
				},
			}),
			new SymbolInstructionInfo(32, "8A 05 FFFFFFFF", Code.Mov_Gb_Eb, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(4, displacementSize);
					Assert.Equal(0xFFFFFFFFUL, displacement);
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(32, "8A 05 FFFFFFFF", Code.Mov_Gb_Eb, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(4, displacementSize);
					Assert.Equal(0xFFFFFFFFUL, displacement);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.Address);
					return true;
				},
			}),
			new SymbolInstructionInfo(32, "8A 05 FFFFFFFF", Code.Mov_Gb_Eb, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(4, displacementSize);
					Assert.Equal(0xFFFFFFFFUL, displacement);
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Address | SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8A 04 25 FFFFFFFF", Code.Mov_Gb_Eb, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(8, displacementSize);
					Assert.Equal(0xFFFFFFFFFFFFFFFFUL, displacement);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.None);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8A 04 25 FFFFFFFF", Code.Mov_Gb_Eb, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(8, displacementSize);
					Assert.Equal(0xFFFFFFFFFFFFFFFFUL, displacement);
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8A 04 25 FFFFFFFF", Code.Mov_Gb_Eb, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(8, displacementSize);
					Assert.Equal(0xFFFFFFFFFFFFFFFFUL, displacement);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.Address);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8A 04 25 FFFFFFFF", Code.Mov_Gb_Eb, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(8, displacementSize);
					Assert.Equal(0xFFFFFFFFFFFFFFFFUL, displacement);
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Address | SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8A 00", Code.Mov_Gb_Eb, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(8, displacementSize);
					Assert.Equal(0UL, displacement);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.None);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8A 00", Code.Mov_Gb_Eb, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(8, displacementSize);
					Assert.Equal(0UL, displacement);
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8A 00", Code.Mov_Gb_Eb, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(8, displacementSize);
					Assert.Equal(0UL, displacement);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.Address);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8A 00", Code.Mov_Gb_Eb, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(8, displacementSize);
					Assert.Equal(0UL, displacement);
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Address | SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8A 40 A5", Code.Mov_Gb_Eb, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(8, displacementSize);
					Assert.Equal(0xFFFFFFFFFFFFFFA5UL, displacement);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.None);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8A 40 A5", Code.Mov_Gb_Eb, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(8, displacementSize);
					Assert.Equal(0xFFFFFFFFFFFFFFA5UL, displacement);
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8A 40 A5", Code.Mov_Gb_Eb, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(8, displacementSize);
					Assert.Equal(0xFFFFFFFFFFFFFFA5UL, displacement);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.Address);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8A 40 A5", Code.Mov_Gb_Eb, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(8, displacementSize);
					Assert.Equal(0xFFFFFFFFFFFFFFA5UL, displacement);
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Address | SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8A 40 A5", Code.Mov_Gb_Eb, a => a.SpacesBetweenMemoryAddOperators = true, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(8, displacementSize);
					Assert.Equal(0xFFFFFFFFFFFFFFA5UL, displacement);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.None);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8A 40 A5", Code.Mov_Gb_Eb, a => a.SpacesBetweenMemoryAddOperators = true, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(8, displacementSize);
					Assert.Equal(0xFFFFFFFFFFFFFFA5UL, displacement);
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8A 40 A5", Code.Mov_Gb_Eb, a => a.SpacesBetweenMemoryAddOperators = false, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(8, displacementSize);
					Assert.Equal(0xFFFFFFFFFFFFFFA5UL, displacement);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.None);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8A 40 A5", Code.Mov_Gb_Eb, a => a.SpacesBetweenMemoryAddOperators = false, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(8, displacementSize);
					Assert.Equal(0xFFFFFFFFFFFFFFA5UL, displacement);
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(16, "8A 80 5AA5", Code.Mov_Gb_Eb, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(2, displacementSize);
					Assert.Equal(0xA55AUL, displacement);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.None);
					return true;
				},
			}),
			new SymbolInstructionInfo(16, "8A 80 5AA5", Code.Mov_Gb_Eb, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(2, displacementSize);
					Assert.Equal(0xA55AUL, displacement);
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(16, "8A 80 5AA5", Code.Mov_Gb_Eb, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(2, displacementSize);
					Assert.Equal(0xA55AUL, displacement);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.None);
					return true;
				},
			}),
			new SymbolInstructionInfo(16, "8A 80 5AA5", Code.Mov_Gb_Eb, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(2, displacementSize);
					Assert.Equal(0xA55AUL, displacement);
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(32, "8A 80 88A9CBED", Code.Mov_Gb_Eb, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(4, displacementSize);
					Assert.Equal(0xEDCBA988UL, displacement);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.None);
					return true;
				},
			}),
			new SymbolInstructionInfo(32, "8A 80 88A9CBED", Code.Mov_Gb_Eb, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(4, displacementSize);
					Assert.Equal(0xEDCBA988UL, displacement);
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(32, "8A 80 88A9CBED", Code.Mov_Gb_Eb, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(4, displacementSize);
					Assert.Equal(0xEDCBA988UL, displacement);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.None);
					return true;
				},
			}),
			new SymbolInstructionInfo(32, "8A 80 88A9CBED", Code.Mov_Gb_Eb, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(4, displacementSize);
					Assert.Equal(0xEDCBA988UL, displacement);
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8A 80 88A9CBED", Code.Mov_Gb_Eb, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(8, displacementSize);
					Assert.Equal(0xFFFFFFFFEDCBA988UL, displacement);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.None);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8A 80 88A9CBED", Code.Mov_Gb_Eb, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(8, displacementSize);
					Assert.Equal(0xFFFFFFFFEDCBA988UL, displacement);
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8A 80 88A9CBED", Code.Mov_Gb_Eb, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(8, displacementSize);
					Assert.Equal(0xFFFFFFFFEDCBA988UL, displacement);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.None);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8A 80 88A9CBED", Code.Mov_Gb_Eb, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(8, displacementSize);
					Assert.Equal(0xFFFFFFFFEDCBA988UL, displacement);
					symbol = new SymbolResult(new TextInfo(new TextPart[] { new TextPart("sym", FormatterOutputTextKind.Text), new TextPart("next", FormatterOutputTextKind.Text) }), SymbolFlags.Signed);
					return true;
				},
			}),
			new SymbolInstructionInfo(64, "8A 40 A5", Code.Mov_Gb_Eb, new TestSymbolResolver {
				tryGetDisplSymbol = (ulong displacement, int displacementSize, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) => {
					Assert.Equal(8, displacementSize);
					Assert.Equal(0xFFFFFFFFFFFFFFA5UL, displacement);
					symbol = new SymbolResult(new TextInfo("symbol", FormatterOutputTextKind.Text), SymbolFlags.None);
					return false;
				},
			}),
		};
	}
}
#endif
