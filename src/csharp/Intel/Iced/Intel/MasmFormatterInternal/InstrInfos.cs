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

#if !NO_MASM_FORMATTER && !NO_FORMATTER
using System;
using Iced.Intel.FormatterInternal;
using Iced.Intel.Internal;

namespace Iced.Intel.MasmFormatterInternal {
	static partial class InstrInfos {
		public static readonly InstrInfo[] AllInfos = ReadInfos();

		static string AddSuffix(string s, char[] ca) =>
			string.Intern(s + new string(ca));

		static string AddPrefix(string s, char[] ca) =>
			string.Intern(new string(ca) + s);

		static InstrInfo[] ReadInfos() {
			var reader = new DataReader(GetSerializedInstrInfos());
			var infos = new InstrInfo[IcedConstants.NumberOfCodeValues];
			var strings = FormatterStringsTable.GetStringsTable();

			var ca = new char[1];
			string s, s2, s3, s4;
			uint v, v2, v3;
			int prevIndex = -1;
			for (int i = 0; i < infos.Length; i++) {
				var code = (Code)i;
				byte f = reader.ReadByte();
				var ctorKind = (CtorKind)(f & 0x7F);
				int currentIndex;
				if (ctorKind == CtorKind.Previous) {
					currentIndex = reader.Index;
					reader.Index = prevIndex;
					ctorKind = (CtorKind)(reader.ReadByte() & 0x7F);
				}
				else {
					currentIndex = -1;
					prevIndex = reader.Index - 1;
				}
				s = strings[reader.ReadCompressedUInt32()];
				if ((f & 0x80) != 0) {
					ca[0] = 'v';
					s = AddPrefix(s, ca);
				}
				InstrInfo instrInfo;
				switch (ctorKind) {
				case CtorKind.Normal_1:
					instrInfo = new SimpleInstrInfo(code, s);
					break;

				case CtorKind.Normal_2:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo(code, s, (InstrOpInfoFlags)v);
					break;

				case CtorKind.AamAad:
					instrInfo = new SimpleInstrInfo_AamAad(code, s);
					break;

				case CtorKind.AX:
					ca[0] = (char)reader.ReadByte();
					s2 = AddSuffix(s, ca);
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_AX(code, s, s2, (InstrOpInfoFlags)v);
					break;

				case CtorKind.AY:
					ca[0] = (char)reader.ReadByte();
					s2 = AddSuffix(s, ca);
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_AY(code, s, s2, (InstrOpInfoFlags)v);
					break;

				case CtorKind.bnd_1:
					instrInfo = new SimpleInstrInfo_bnd(code, s);
					break;

				case CtorKind.bnd_2:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_bnd(code, s, (InstrOpInfoFlags)v);
					break;

				case CtorKind.DeclareData:
					instrInfo = new SimpleInstrInfo_DeclareData(code, s);
					break;

				case CtorKind.DX:
					ca[0] = (char)reader.ReadByte();
					s2 = AddSuffix(s, ca);
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_DX(code, s, s2, (InstrOpInfoFlags)v);
					break;

				case CtorKind.fword:
					ca[0] = (char)reader.ReadByte();
					s2 = AddSuffix(s, ca);
					v = reader.ReadByte();
					v2 = reader.ReadByte();
					if (v2 > 1)
						throw new InvalidOperationException();
					instrInfo = new SimpleInstrInfo_fword(code, (CodeSize)v, v2 != 0, s, s2);
					break;

				case CtorKind.Ib:
					instrInfo = new SimpleInstrInfo_Ib(code, s);
					break;

				case CtorKind.imul:
					instrInfo = new SimpleInstrInfo_imul(code, s);
					break;

				case CtorKind.invlpga:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_invlpga(code, (int)v, s);
					break;

				case CtorKind.jcc:
					instrInfo = new SimpleInstrInfo_jcc(code, s);
					break;

				case CtorKind.maskmovq:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_maskmovq(code, s, (InstrOpInfoFlags)v);
					break;

				case CtorKind.memsize:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_memsize(code, (int)v, s);
					break;

				case CtorKind.mmxmem_1:
					instrInfo = new SimpleInstrInfo_mmxmem(code, s);
					break;

				case CtorKind.mmxmem_2:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_mmxmem(code, s, (InstrOpInfoFlags)v);
					break;

				case CtorKind.monitor:
					v = reader.ReadByte();
					v2 = reader.ReadByte();
					v3 = reader.ReadByte();
					instrInfo = new SimpleInstrInfo_monitor(code, s, (Register)v, (Register)v2, (Register)v3);
					break;

				case CtorKind.mwait:
					instrInfo = new SimpleInstrInfo_mwait(code, s);
					break;

				case CtorKind.mwaitx:
					instrInfo = new SimpleInstrInfo_mwaitx(code, s);
					break;

				case CtorKind.nop:
					v = reader.ReadCompressedUInt32();
					v2 = reader.ReadByte();
					instrInfo = new SimpleInstrInfo_nop(code, (int)v, s, (Register)v2);
					break;

				case CtorKind.OpSize_1:
					v = reader.ReadByte();
					ca[0] = 'w';
					s2 = AddSuffix(s, ca);
					ca[0] = 'd';
					s3 = AddSuffix(s, ca);
					ca[0] = 'q';
					s4 = AddSuffix(s, ca);
					instrInfo = new SimpleInstrInfo_OpSize(code, (CodeSize)v, s, s2, s3, s4);
					break;

				case CtorKind.OpSize_2:
					ca[0] = (char)reader.ReadByte();
					s2 = AddSuffix(s, ca);
					v = reader.ReadByte();
					instrInfo = new SimpleInstrInfo_OpSize(code, (CodeSize)v, s, s2, s2, s2);
					break;

				case CtorKind.OpSize2:
					s2 = strings[reader.ReadCompressedUInt32()];
					s3 = strings[reader.ReadCompressedUInt32()];
					s4 = strings[reader.ReadCompressedUInt32()];
					instrInfo = new SimpleInstrInfo_OpSize2(code, s, s2, s3, s4);
					break;

				case CtorKind.OpSize2_bnd:
					s2 = strings[reader.ReadCompressedUInt32()];
					s3 = strings[reader.ReadCompressedUInt32()];
					s4 = strings[reader.ReadCompressedUInt32()];
					instrInfo = new SimpleInstrInfo_OpSize2_bnd(code, s, s2, s3, s4);
					break;

				case CtorKind.pblendvb:
					instrInfo = new SimpleInstrInfo_pblendvb(code, s);
					break;

				case CtorKind.pclmulqdq:
					v = reader.ReadByte();
					instrInfo = new SimpleInstrInfo_pclmulqdq(code, s, FormatterConstants.GetPseudoOps((PseudoOpsKind)v));
					break;

				case CtorKind.pops_2:
					v = reader.ReadByte();
					instrInfo = new SimpleInstrInfo_pops(code, s, FormatterConstants.GetPseudoOps((PseudoOpsKind)v));
					break;

				case CtorKind.pops_3:
					v = reader.ReadByte();
					v2 = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_pops(code, s, FormatterConstants.GetPseudoOps((PseudoOpsKind)v), (InstrOpInfoFlags)v2);
					break;

				case CtorKind.pushm:
					v = reader.ReadByte();
					instrInfo = new SimpleInstrInfo_pushm(code, (CodeSize)v, s);
					break;

				case CtorKind.reg:
					v = reader.ReadByte();
					instrInfo = new SimpleInstrInfo_reg(code, s, (Register)v);
					break;

				case CtorKind.Reg16:
					instrInfo = new SimpleInstrInfo_Reg16(code, s);
					break;

				case CtorKind.reverse2:
					instrInfo = new SimpleInstrInfo_reverse2(code, s);
					break;

				case CtorKind.ST_STi:
					instrInfo = new SimpleInstrInfo_ST_STi(code, s);
					break;

				case CtorKind.STi_ST:
					instrInfo = new SimpleInstrInfo_STi_ST(code, s);
					break;

				case CtorKind.STi_ST2:
					instrInfo = new SimpleInstrInfo_STi_ST2(code, s);
					break;

				case CtorKind.STIG1_1:
					instrInfo = new SimpleInstrInfo_STIG1(code, s);
					break;

				case CtorKind.STIG1_2:
					v = reader.ReadByte();
					if (v > 1)
						throw new InvalidOperationException();
					instrInfo = new SimpleInstrInfo_STIG1(code, s, v != 0);
					break;

				case CtorKind.XLAT:
					ca[0] = (char)reader.ReadByte();
					s2 = AddSuffix(s, ca);
					instrInfo = new SimpleInstrInfo_XLAT(code, s, s2);
					break;

				case CtorKind.XY:
					ca[0] = (char)reader.ReadByte();
					s2 = AddSuffix(s, ca);
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_XY(code, s, s2, (InstrOpInfoFlags)v);
					break;

				case CtorKind.YA:
					ca[0] = (char)reader.ReadByte();
					s2 = AddSuffix(s, ca);
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_YA(code, s, s2, (InstrOpInfoFlags)v);
					break;

				case CtorKind.YD:
					ca[0] = (char)reader.ReadByte();
					s2 = AddSuffix(s, ca);
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_YD(code, s, s2, (InstrOpInfoFlags)v);
					break;

				case CtorKind.YX:
					ca[0] = (char)reader.ReadByte();
					s2 = AddSuffix(s, ca);
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_YX(code, s, s2, (InstrOpInfoFlags)v);
					break;

				default:
					throw new InvalidOperationException();
				}
				infos[i] = instrInfo;
				if (currentIndex >= 0)
					reader.Index = currentIndex;
			}
			if (reader.CanRead)
				throw new InvalidOperationException();

			return infos;
		}
	}
}
#endif
