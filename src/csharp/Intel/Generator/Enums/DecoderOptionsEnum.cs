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

namespace Generator.Enums {
	static class DecoderOptionsEnum {
		const string documentation = "Decoder options";

		static EnumValue[] GetValues() =>
			new EnumValue[] {
				new EnumValue("None", "No option is enabled"),
				new EnumValue("NoInvalidCheck", "Disable some checks for invalid encodings of instructions, eg. most instructions can't use a #(c:LOCK)# prefix so if one is found, they're decoded as #(e:Code.INVALID)# unless this option is enabled."),
				new EnumValue("AmdBranches", "AMD branch decoder: allow 16-bit branch/ret instructions in 64-bit mode"),
				new EnumValue("ForceReservedNop", "Decode opcodes #(c:0F0D)# and #(c:0F18-0F1F)# as reserved-nop instructions (eg. #(e:Code.ReservedNop_rm32_r32_0F1D)#)"),
				new EnumValue("Umov", "Decode #(c:umov)# instructions (eg. #(e:Code.Umov_r32_rm32)#)"),
				new EnumValue("Xbts", "Decode #(c:xbts)#/#(c:ibts)#"),
				new EnumValue("Cmpxchg486A", "Decode #(c:0FA6)#/#(c:0FA7)# as #(c:cmpxchg)#"),
				new EnumValue("OldFpu", "Decode some old removed FPU instructions (eg. #(c:frstpm)#)"),
				new EnumValue("Pcommit", "Decode #(e:Code.Pcommit)#"),
				new EnumValue("Loadall286", "Decode 286 #(c:loadall)# (#(c:0F04)# and #(c:0F05)#)"),
				new EnumValue("Loadall386", "Decode #(e:Code.Loadall386)#"),
				new EnumValue("Cl1invmb", "Decode #(e:Code.Cl1invmb)#"),
				new EnumValue("MovTr", "Decode #(e:Code.Mov_r32_tr)# and #(e:Code.Mov_tr_r32)#"),
				new EnumValue("Jmpe", "Decode #(c:jmpe)# instructions"),
				new EnumValue("NoPause", "Don't decode #(e:Code.Pause)#, decode #(e:Code.Nopd)#/etc instead"),
				new EnumValue("NoWbnoinvd", "Don't decode #(e:Code.Wbnoinvd)#, decode #(e:Code.Wbinvd)# instead"),
				new EnumValue("NoLockMovCR0", "Don't decode #(c:LOCK MOV CR0)# as #(c:MOV CR8)# (AMD)"),
				new EnumValue("NoMPFX_0FBC", "Don't decode #(e:Code.Tzcnt_r32_rm32)#/etc, decode #(e:Code.Bsf_r32_rm32)#/etc instead"),
				new EnumValue("NoMPFX_0FBD", "Don't decode #(e:Code.Lzcnt_r32_rm32)#/etc, decode #(e:Code.Bsr_r32_rm32)#/etc instead"),
				new EnumValue("NoLahfSahf64", "Don't decode #(e:Code.Lahf)# and #(e:Code.Sahf)# in 64-bit mode"),
			};

		public static readonly EnumType Instance = new EnumType(TypeIds.DecoderOptions, documentation, GetValues(), EnumTypeFlags.Public | EnumTypeFlags.Flags);
	}
}
