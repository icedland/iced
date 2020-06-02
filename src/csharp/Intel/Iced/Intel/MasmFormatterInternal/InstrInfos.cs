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

#if MASM
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
					instrInfo = new SimpleInstrInfo(s);
					break;

				case CtorKind.Normal_2:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo(s, (InstrOpInfoFlags)v);
					break;

				case CtorKind.AamAad:
					instrInfo = new SimpleInstrInfo_AamAad(s);
					break;

				case CtorKind.AX:
					ca[0] = (char)reader.ReadByte();
					s2 = AddSuffix(s, ca);
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_AX(s, s2, (InstrOpInfoFlags)v);
					break;

				case CtorKind.AY:
					ca[0] = (char)reader.ReadByte();
					s2 = AddSuffix(s, ca);
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_AY(s, s2, (InstrOpInfoFlags)v);
					break;

				case CtorKind.bnd_1:
					instrInfo = new SimpleInstrInfo_bnd(s);
					break;

				case CtorKind.bnd_2:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_bnd(s, (InstrOpInfoFlags)v);
					break;

				case CtorKind.DeclareData:
					instrInfo = new SimpleInstrInfo_DeclareData((Code)i, s);
					break;

				case CtorKind.DX:
					ca[0] = (char)reader.ReadByte();
					s2 = AddSuffix(s, ca);
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_DX(s, s2, (InstrOpInfoFlags)v);
					break;

				case CtorKind.fword:
					ca[0] = (char)reader.ReadByte();
					s2 = AddSuffix(s, ca);
					v = reader.ReadByte();
					v2 = reader.ReadByte();
					if (v2 > 1)
						throw new InvalidOperationException();
					instrInfo = new SimpleInstrInfo_fword((CodeSize)v, v2 != 0, s, s2);
					break;

				case CtorKind.Ib:
					instrInfo = new SimpleInstrInfo_Ib(s);
					break;

				case CtorKind.imul:
					instrInfo = new SimpleInstrInfo_imul(s);
					break;

				case CtorKind.invlpga:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_invlpga((int)v, s);
					break;

				case CtorKind.CCa_1:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_cc((int)v, new[] { s });
					break;

				case CtorKind.CCa_2:
					s2 = strings[reader.ReadCompressedUInt32()];
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_cc((int)v, new[] { s, s2 });
					break;

				case CtorKind.CCa_3:
					s2 = strings[reader.ReadCompressedUInt32()];
					s3 = strings[reader.ReadCompressedUInt32()];
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_cc((int)v, new[] { s, s2, s3 });
					break;

				case CtorKind.CCb_1:
					v2 = reader.ReadCompressedUInt32();
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_cc((int)v2, new[] { s }, (InstrOpInfoFlags)v);
					break;

				case CtorKind.CCb_2:
					s2 = strings[reader.ReadCompressedUInt32()];
					v2 = reader.ReadCompressedUInt32();
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_cc((int)v2, new[] { s, s2 }, (InstrOpInfoFlags)v);
					break;

				case CtorKind.CCb_3:
					s2 = strings[reader.ReadCompressedUInt32()];
					s3 = strings[reader.ReadCompressedUInt32()];
					v2 = reader.ReadCompressedUInt32();
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_cc((int)v2, new[] { s, s2, s3 }, (InstrOpInfoFlags)v);
					break;

				case CtorKind.jcc_1:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_jcc((int)v, new[] { s });
					break;

				case CtorKind.jcc_2:
					s2 = strings[reader.ReadCompressedUInt32()];
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_jcc((int)v, new[] { s, s2 });
					break;

				case CtorKind.jcc_3:
					s2 = strings[reader.ReadCompressedUInt32()];
					s3 = strings[reader.ReadCompressedUInt32()];
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_jcc((int)v, new[] { s, s2, s3 });
					break;

				case CtorKind.Loopcc1:
					s2 = strings[reader.ReadCompressedUInt32()];
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_cc((int)v, new[] { s, s2 });
					break;

				case CtorKind.Loopcc2:
					s2 = strings[reader.ReadCompressedUInt32()];
					ca[0] = (char)reader.ReadByte();
					v2 = reader.ReadCompressedUInt32();
					s3 = AddSuffix(s, ca);
					s4 = AddSuffix(s2, ca);
					v = reader.ReadByte();
					instrInfo = new SimpleInstrInfo_OpSize_cc((CodeSize)v, (int)v2, new[] { s, s2 }, new[] { s3, s4 });
					break;

				case CtorKind.maskmovq:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_maskmovq(s, (InstrOpInfoFlags)v);
					break;

				case CtorKind.memsize:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_memsize((int)v, s);
					break;

				case CtorKind.mmxmem_1:
					instrInfo = new SimpleInstrInfo_mmxmem(s);
					break;

				case CtorKind.mmxmem_2:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_mmxmem(s, (InstrOpInfoFlags)v);
					break;

				case CtorKind.monitor:
					v = reader.ReadByte();
					v2 = reader.ReadByte();
					v3 = reader.ReadByte();
					instrInfo = new SimpleInstrInfo_monitor(s, (Register)v, (Register)v2, (Register)v3);
					break;

				case CtorKind.mwait:
					instrInfo = new SimpleInstrInfo_mwait(s);
					break;

				case CtorKind.mwaitx:
					instrInfo = new SimpleInstrInfo_mwaitx(s);
					break;

				case CtorKind.nop:
					v = reader.ReadCompressedUInt32();
					v2 = reader.ReadByte();
					instrInfo = new SimpleInstrInfo_nop((int)v, s, (Register)v2);
					break;

				case CtorKind.OpSize_1:
					v = reader.ReadByte();
					ca[0] = 'w';
					s2 = AddSuffix(s, ca);
					ca[0] = 'd';
					s3 = AddSuffix(s, ca);
					ca[0] = 'q';
					s4 = AddSuffix(s, ca);
					instrInfo = new SimpleInstrInfo_OpSize((CodeSize)v, s, s2, s3, s4);
					break;

				case CtorKind.OpSize_2:
					ca[0] = (char)reader.ReadByte();
					s2 = AddSuffix(s, ca);
					v = reader.ReadByte();
					instrInfo = new SimpleInstrInfo_OpSize((CodeSize)v, s, s2, s2, s2);
					break;

				case CtorKind.OpSize2:
					s2 = strings[reader.ReadCompressedUInt32()];
					s3 = strings[reader.ReadCompressedUInt32()];
					s4 = strings[reader.ReadCompressedUInt32()];
					instrInfo = new SimpleInstrInfo_OpSize2(s, s2, s3, s4);
					break;

				case CtorKind.OpSize2_bnd:
					s2 = strings[reader.ReadCompressedUInt32()];
					s3 = strings[reader.ReadCompressedUInt32()];
					s4 = strings[reader.ReadCompressedUInt32()];
					instrInfo = new SimpleInstrInfo_OpSize2_bnd(s, s2, s3, s4);
					break;

				case CtorKind.pblendvb:
					instrInfo = new SimpleInstrInfo_pblendvb(s);
					break;

				case CtorKind.pclmulqdq:
					v = reader.ReadByte();
					instrInfo = new SimpleInstrInfo_pclmulqdq(s, FormatterConstants.GetPseudoOps((PseudoOpsKind)v));
					break;

				case CtorKind.pops_2:
					v = reader.ReadByte();
					instrInfo = new SimpleInstrInfo_pops(s, FormatterConstants.GetPseudoOps((PseudoOpsKind)v));
					break;

				case CtorKind.pops_3:
					v = reader.ReadByte();
					v2 = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_pops(s, FormatterConstants.GetPseudoOps((PseudoOpsKind)v), (InstrOpInfoFlags)v2);
					break;

				case CtorKind.pushm:
					v = reader.ReadByte();
					instrInfo = new SimpleInstrInfo_pushm((CodeSize)v, s);
					break;

				case CtorKind.reg:
					v = reader.ReadByte();
					instrInfo = new SimpleInstrInfo_reg(s, (Register)v);
					break;

				case CtorKind.Reg16:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_Reg16(s, (InstrOpInfoFlags)v);
					break;

				case CtorKind.Reg32:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_Reg32(s, (InstrOpInfoFlags)v);
					break;

				case CtorKind.reverse2:
					instrInfo = new SimpleInstrInfo_reverse2(s);
					break;

				case CtorKind.ST_STi:
					instrInfo = new SimpleInstrInfo_ST_STi(s);
					break;

				case CtorKind.STi_ST:
					instrInfo = new SimpleInstrInfo_STi_ST(s);
					break;

				case CtorKind.STi_ST2:
					instrInfo = new SimpleInstrInfo_STi_ST2(s);
					break;

				case CtorKind.STIG1_1:
					instrInfo = new SimpleInstrInfo_STIG1(s);
					break;

				case CtorKind.STIG1_2:
					v = reader.ReadByte();
					if (v > 1)
						throw new InvalidOperationException();
					instrInfo = new SimpleInstrInfo_STIG1(s, v != 0);
					break;

				case CtorKind.XLAT:
					ca[0] = (char)reader.ReadByte();
					s2 = AddSuffix(s, ca);
					instrInfo = new SimpleInstrInfo_XLAT(s, s2);
					break;

				case CtorKind.XY:
					ca[0] = (char)reader.ReadByte();
					s2 = AddSuffix(s, ca);
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_XY(s, s2, (InstrOpInfoFlags)v);
					break;

				case CtorKind.YA:
					ca[0] = (char)reader.ReadByte();
					s2 = AddSuffix(s, ca);
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_YA(s, s2, (InstrOpInfoFlags)v);
					break;

				case CtorKind.YD:
					ca[0] = (char)reader.ReadByte();
					s2 = AddSuffix(s, ca);
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_YD(s, s2, (InstrOpInfoFlags)v);
					break;

				case CtorKind.YX:
					ca[0] = (char)reader.ReadByte();
					s2 = AddSuffix(s, ca);
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_YX(s, s2, (InstrOpInfoFlags)v);
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
