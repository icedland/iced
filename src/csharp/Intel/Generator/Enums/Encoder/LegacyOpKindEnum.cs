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

namespace Generator.Enums.Encoder {
	enum LegacyOpKind : byte {
		None,
		Aww,
		Adw,
		M,
		Mfbcd,
		Mf32,
		Mf64,
		Mf80,
		Mfi16,
		Mfi32,
		Mfi64,
		M14,
		M28,
		M98,
		M108,
		Mp,
		Ms,
		Mo,
		Mb,
		Mw,
		Md,
		Md_MPX,
		Mq,
		Mq_MPX,
		Mw2,
		Md2,
		Eb,
		Ew,
		Ed,
		Ed_MPX,
		Ew_d,
		Ew_q,
		Eq,
		Eq_MPX,
		Eww,
		Edw,
		Eqw,
		RdMb,
		RqMb,
		RdMw,
		RqMw,
		Gb,
		Gw,
		Gd,
		Gq,
		Gw_mem,
		Gd_mem,
		Gq_mem,
		Rw,
		Rd,
		Rq,
		Sw,
		Cd,
		Cq,
		Dd,
		Dq,
		Td,
		Ib,
		Ib16,
		Ib32,
		Ib64,
		Iw,
		Id,
		Id64,
		Iq,
		Ib21,
		Ib11,
		Xb,
		Xw,
		Xd,
		Xq,
		Yb,
		Yw,
		Yd,
		Yq,
		wJb,
		dJb,
		qJb,
		Jw,
		wJd,
		dJd,
		qJd,
		Jxw,
		Jxd,
		Jdisp16,
		Jdisp32,
		Ob,
		Ow,
		Od,
		Oq,
		Imm1,
		B,
		BMq,
		BMo,
		MIB,
		N,
		P,
		Q,
		RX,
		VX,
		WX,
		rDI,
		MRBX,
		ES,
		CS,
		SS,
		DS,
		FS,
		GS,
		AL,
		CL,
		AX,
		DX,
		EAX,
		RAX,
		ST,
		STi,
		r8_rb,
		r16_rw,
		r32_rd,
		r64_ro,
	}

	static class LegacyOpKindEnum {
		const string? documentation = null;

		static EnumValue[] GetValues() =>
			typeof(LegacyOpKind).GetFields().Where(a => a.IsLiteral).Select(a => new EnumValue((uint)(LegacyOpKind)a.GetValue(null)!, a.Name, CommentAttribute.GetDocumentation(a))).ToArray();

		public static readonly EnumType Instance = new EnumType(TypeIds.LegacyOpKind, documentation, GetValues(), EnumTypeFlags.NoInitialize);
	}
}
