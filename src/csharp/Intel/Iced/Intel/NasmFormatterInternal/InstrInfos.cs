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

#if NASM
using System;
using Iced.Intel.FormatterInternal;
using Iced.Intel.Internal;

namespace Iced.Intel.NasmFormatterInternal {
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
			uint v, v2, v3, v4;
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

				case CtorKind.asz:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_as((int)v, s);
					break;

				case CtorKind.AX:
					instrInfo = new SimpleInstrInfo_AX(s);
					break;

				case CtorKind.AY:
					instrInfo = new SimpleInstrInfo_AY(s);
					break;

				case CtorKind.bcst:
					v = reader.ReadCompressedUInt32();
					v2 = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_bcst(s, (InstrOpInfoFlags)v, (InstrOpInfoFlags)v2);
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
					instrInfo = new SimpleInstrInfo_DX(s);
					break;

				case CtorKind.er_2:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_er((int)v, s);
					break;

				case CtorKind.er_3:
					v = reader.ReadCompressedUInt32();
					v2 = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_er((int)v, s, (InstrOpInfoFlags)v2);
					break;

				case CtorKind.far:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_far((int)v, s);
					break;

				case CtorKind.far_mem:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_far_mem((int)v, s);
					break;

				case CtorKind.invlpga:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_invlpga((int)v, s);
					break;

				case CtorKind.maskmovq:
					instrInfo = new SimpleInstrInfo_maskmovq(s);
					break;

				case CtorKind.mmxmem_1:
					instrInfo = new SimpleInstrInfo_mmxmem(s);
					break;

				case CtorKind.mmxmem_2:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_mmxmem(s, (InstrOpInfoFlags)v);
					break;

				case CtorKind.mmxmem_3:
					v = reader.ReadCompressedUInt32();
					v2 = reader.ReadByte();
					instrInfo = new SimpleInstrInfo_mmxmem(s, (InstrOpInfoFlags)v, (MemorySize)v2);
					break;

				case CtorKind.movabs:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_movabs((int)v, s);
					break;

				case CtorKind.ms_pops:
					v2 = reader.ReadByte();
					v3 = reader.ReadCompressedUInt32();
					v4 = reader.ReadByte();
					instrInfo = new SimpleInstrInfo_ms_pops(s, FormatterConstants.GetPseudoOps((PseudoOpsKind)v2), (InstrOpInfoFlags)v3, (MemorySize)v4);
					break;

				case CtorKind.nop:
					v = reader.ReadCompressedUInt32();
					v2 = reader.ReadByte();
					instrInfo = new SimpleInstrInfo_nop((int)v, s, (Register)v2);
					break;

				case CtorKind.OpSize:
					v = reader.ReadByte();
					ca[0] = 'w';
					s2 = AddSuffix(s, ca);
					ca[0] = 'd';
					s3 = AddSuffix(s, ca);
					ca[0] = 'q';
					s4 = AddSuffix(s, ca);
					instrInfo = new SimpleInstrInfo_OpSize((CodeSize)v, s, s2, s3, s4);
					break;

				case CtorKind.OpSize2_bnd:
					s2 = strings[reader.ReadCompressedUInt32()];
					s3 = strings[reader.ReadCompressedUInt32()];
					s4 = strings[reader.ReadCompressedUInt32()];
					instrInfo = new SimpleInstrInfo_OpSize2_bnd(s, s2, s3, s4);
					break;

				case CtorKind.OpSize3:
					ca[0] = (char)reader.ReadByte();
					s2 = AddSuffix(s, ca);
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_OpSize3((int)v, s, s2);
					break;

				case CtorKind.os_2:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_os((int)v, s);
					break;

				case CtorKind.os_3:
					v = reader.ReadCompressedUInt32();
					v2 = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_os((int)v, s, (InstrOpInfoFlags)v2);
					break;

				case CtorKind.os_call_2:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_os_call((int)v, s);
					break;

				case CtorKind.os_call_3:
					v = reader.ReadCompressedUInt32();
					v2 = reader.ReadByte();
					if (v2 > 1)
						throw new InvalidOperationException();
					instrInfo = new SimpleInstrInfo_os_call((int)v, s, v2 != 0);
					break;

				case CtorKind.CC_1:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_cc((int)v, new[] { s });
					break;

				case CtorKind.CC_2:
					s2 = strings[reader.ReadCompressedUInt32()];
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_cc((int)v, new[] { s, s2 });
					break;

				case CtorKind.CC_3:
					s2 = strings[reader.ReadCompressedUInt32()];
					s3 = strings[reader.ReadCompressedUInt32()];
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_cc((int)v, new[] { s, s2, s3 });
					break;

				case CtorKind.os_jcc_a_1:
					v2 = reader.ReadCompressedUInt32();
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_os_jcc((int)v, (int)v2, new[] { s });
					break;

				case CtorKind.os_jcc_a_2:
					s2 = strings[reader.ReadCompressedUInt32()];
					v2 = reader.ReadCompressedUInt32();
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_os_jcc((int)v, (int)v2, new[] { s, s2 });
					break;

				case CtorKind.os_jcc_a_3:
					s2 = strings[reader.ReadCompressedUInt32()];
					s3 = strings[reader.ReadCompressedUInt32()];
					v2 = reader.ReadCompressedUInt32();
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_os_jcc((int)v, (int)v2, new[] { s, s2, s3 });
					break;

				case CtorKind.os_jcc_b_1:
					v3 = reader.ReadCompressedUInt32();
					v = reader.ReadCompressedUInt32();
					v2 = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_os_jcc((int)v, (int)v3, new[] { s }, (InstrOpInfoFlags)v2);
					break;

				case CtorKind.os_jcc_b_2:
					s2 = strings[reader.ReadCompressedUInt32()];
					v3 = reader.ReadCompressedUInt32();
					v = reader.ReadCompressedUInt32();
					v2 = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_os_jcc((int)v, (int)v3, new[] { s, s2 }, (InstrOpInfoFlags)v2);
					break;

				case CtorKind.os_jcc_b_3:
					s2 = strings[reader.ReadCompressedUInt32()];
					s3 = strings[reader.ReadCompressedUInt32()];
					v3 = reader.ReadCompressedUInt32();
					v = reader.ReadCompressedUInt32();
					v2 = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_os_jcc((int)v, (int)v3, new[] { s, s2, s3 }, (InstrOpInfoFlags)v2);
					break;

				case CtorKind.os_loopcc:
					s2 = strings[reader.ReadCompressedUInt32()];
					v3 = reader.ReadCompressedUInt32();
					v = reader.ReadCompressedUInt32();
					v2 = reader.ReadByte();
					instrInfo = new SimpleInstrInfo_os_loop((int)v, (int)v3, (Register)v2, new[] { s, s2 });
					break;

				case CtorKind.os_loop:
					v = reader.ReadCompressedUInt32();
					v2 = reader.ReadByte();
					instrInfo = new SimpleInstrInfo_os_loop((int)v, -1, (Register)v2, new[] { s });
					break;

				case CtorKind.os_mem:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_os_mem((int)v, s);
					break;

				case CtorKind.os_mem_reg16:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_os_mem_reg16((int)v, s);
					break;

				case CtorKind.os_mem2:
					v = reader.ReadCompressedUInt32();
					v2 = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_os_mem2((int)v, s, (InstrOpInfoFlags)v2);
					break;

				case CtorKind.pblendvb_1:
					instrInfo = new SimpleInstrInfo_pblendvb(s);
					break;

				case CtorKind.pblendvb_2:
					v = reader.ReadByte();
					instrInfo = new SimpleInstrInfo_pblendvb(s, (MemorySize)v);
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

				case CtorKind.Reg16:
					instrInfo = new SimpleInstrInfo_Reg16(s);
					break;

				case CtorKind.Reg32:
					instrInfo = new SimpleInstrInfo_Reg32(s);
					break;

				case CtorKind.reverse2:
					instrInfo = new SimpleInstrInfo_reverse2(s);
					break;

				case CtorKind.sae:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_sae((int)v, s);
					break;

				case CtorKind.sae_pops:
					v = reader.ReadCompressedUInt32();
					v2 = reader.ReadByte();
					instrInfo = new SimpleInstrInfo_sae_pops((int)v, s, FormatterConstants.GetPseudoOps((PseudoOpsKind)v2));
					break;

				case CtorKind.SEX1:
					v = reader.ReadCompressedUInt32();
					v2 = reader.ReadByte();
					instrInfo = new SimpleInstrInfo_SEX1((int)v, (SignExtendInfo)v2, s);
					break;

				case CtorKind.SEX1a:
					v = reader.ReadCompressedUInt32();
					v2 = reader.ReadByte();
					instrInfo = new SimpleInstrInfo_SEX1a((int)v, (SignExtendInfo)v2, s);
					break;

				case CtorKind.SEX2_2:
					v = reader.ReadByte();
					instrInfo = new SimpleInstrInfo_SEX2((SignExtendInfo)v, s);
					break;

				case CtorKind.SEX2_3:
					v = reader.ReadByte();
					v2 = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_SEX2((SignExtendInfo)v, s, (InstrOpInfoFlags)v2);
					break;

				case CtorKind.SEX2_4:
					v = reader.ReadByte();
					v2 = reader.ReadByte();
					v3 = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_SEX2((SignExtendInfo)v, (SignExtendInfo)v2, s, (InstrOpInfoFlags)v3);
					break;

				case CtorKind.SEX3:
					v = reader.ReadByte();
					instrInfo = new SimpleInstrInfo_SEX3((SignExtendInfo)v, s);
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

				case CtorKind.STIG2_2a:
					v = reader.ReadByte();
					if (v > 1)
						throw new InvalidOperationException();
					instrInfo = new SimpleInstrInfo_STIG2(s, v != 0);
					break;

				case CtorKind.STIG2_2b:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_STIG2(s, (InstrOpInfoFlags)v);
					break;

				case CtorKind.XLAT:
					instrInfo = new SimpleInstrInfo_XLAT(s);
					break;

				case CtorKind.XY:
					instrInfo = new SimpleInstrInfo_XY(s);
					break;

				case CtorKind.YA:
					instrInfo = new SimpleInstrInfo_YA(s);
					break;

				case CtorKind.YD:
					instrInfo = new SimpleInstrInfo_YD(s);
					break;

				case CtorKind.YX:
					instrInfo = new SimpleInstrInfo_YX(s);
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
