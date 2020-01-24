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

using System.Linq;

namespace Generator.Enums.Decoder {
	static class DecoderOptionsEnum {
		const string documentation = "Decoder options";

		internal enum DecoderOptions {
			[Comment("No option is enabled")]
			None,
			[Comment("Disable some checks for invalid encodings of instructions, eg. most instructions can't use a #(c:LOCK)# prefix so if one is found, they're decoded as #(e:Code.INVALID)# unless this option is enabled.")]
			NoInvalidCheck,
			[Comment("AMD branch decoder: allow 16-bit branch/ret instructions in 64-bit mode")]
			AmdBranches,
			[Comment("Decode opcodes #(c:0F0D)# and #(c:0F18-0F1F)# as reserved-nop instructions (eg. #(e:Code.ReservedNop_rm32_r32_0F1D)#)")]
			ForceReservedNop,
			[Comment("Decode #(c:UMOV)# instructions (eg. #(e:Code.Umov_r32_rm32)#)")]
			Umov,
			[Comment("Decode #(c:XBTS)#/#(c:IBTS)#")]
			Xbts,
			[Comment("Decode #(c:0FA6)#/#(c:0FA7)# as #(c:CMPXCHG)#")]
			Cmpxchg486A,
			[Comment("Decode some old removed FPU instructions (eg. #(c:FRSTPM)#)")]
			OldFpu,
			[Comment("Decode #(e:Code.Pcommit)#")]
			Pcommit,
			[Comment("Decode 286 #(c:LOADALL)# (#(c:0F04)# and #(c:0F05)#)")]
			Loadall286,
			[Comment("Decode #(e:Code.Loadall386)#")]
			Loadall386,
			[Comment("Decode #(e:Code.Cl1invmb)#")]
			Cl1invmb,
			[Comment("Decode #(e:Code.Mov_r32_tr)# and #(e:Code.Mov_tr_r32)#")]
			MovTr,
			[Comment("Decode #(c:JMPE)# instructions")]
			Jmpe,
			[Comment("Don't decode #(e:Code.Pause)#, decode #(e:Code.Nopd)#/etc instead")]
			NoPause,
			[Comment("Don't decode #(e:Code.Wbnoinvd)#, decode #(e:Code.Wbinvd)# instead")]
			NoWbnoinvd,
			[Comment("Don't decode #(c:LOCK MOV CR0)# as #(c:MOV CR8)# (AMD)")]
			NoLockMovCR0,
			[Comment("Don't decode #(e:Code.Tzcnt_r32_rm32)#/etc, decode #(e:Code.Bsf_r32_rm32)#/etc instead")]
			NoMPFX_0FBC,
			[Comment("Don't decode #(e:Code.Lzcnt_r32_rm32)#/etc, decode #(e:Code.Bsr_r32_rm32)#/etc instead")]
			NoMPFX_0FBD,
			[Comment("Don't decode #(e:Code.Lahf)# and #(e:Code.Sahf)# in 64-bit mode")]
			NoLahfSahf64,
		}

		static EnumValue[] GetValues() =>
			typeof(DecoderOptions).GetFields().Where(a => a.IsLiteral).Select(a => new EnumValue((uint)(DecoderOptions)a.GetValue(null)!, a.Name, CommentAttribute.GetDocumentation(a))).ToArray();

		public static readonly EnumType Instance = new EnumType(TypeIds.DecoderOptions, documentation, GetValues(), EnumTypeFlags.Public | EnumTypeFlags.Flags);
	}
}
