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

#if !NO_NASM_FORMATTER && !NO_FORMATTER
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

				case CtorKind.asz:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_as(code, (int)v, s);
					break;

				case CtorKind.AX:
					instrInfo = new SimpleInstrInfo_AX(code, s);
					break;

				case CtorKind.AY:
					instrInfo = new SimpleInstrInfo_AY(code, s);
					break;

				case CtorKind.bcst:
					v = reader.ReadCompressedUInt32();
					v2 = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_bcst(code, s, (InstrOpInfoFlags)v, (InstrOpInfoFlags)v2);
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
					instrInfo = new SimpleInstrInfo_DX(code, s);
					break;

				case CtorKind.er_2:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_er(code, (int)v, s);
					break;

				case CtorKind.er_3:
					v = reader.ReadCompressedUInt32();
					v2 = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_er(code, (int)v, s, (InstrOpInfoFlags)v2);
					break;

				case CtorKind.far:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_far(code, (int)v, s);
					break;

				case CtorKind.far_mem:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_far_mem(code, (int)v, s);
					break;

				case CtorKind.invlpga:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_invlpga(code, (int)v, s);
					break;

				case CtorKind.maskmovq:
					instrInfo = new SimpleInstrInfo_maskmovq(code, s);
					break;

				case CtorKind.mmxmem_1:
					instrInfo = new SimpleInstrInfo_mmxmem(code, s);
					break;

				case CtorKind.mmxmem_2:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_mmxmem(code, s, (InstrOpInfoFlags)v);
					break;

				case CtorKind.mmxmem_3:
					v = reader.ReadCompressedUInt32();
					v2 = reader.ReadByte();
					instrInfo = new SimpleInstrInfo_mmxmem(code, s, (InstrOpInfoFlags)v, (MemorySize)v2);
					break;

				case CtorKind.movabs:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_movabs(code, (int)v, s);
					break;

				case CtorKind.ms_pops:
					v2 = reader.ReadByte();
					v3 = reader.ReadCompressedUInt32();
					v4 = reader.ReadByte();
					instrInfo = new SimpleInstrInfo_ms_pops(code, s, FormatterConstants.GetPseudoOps((PseudoOpsKind)v2), (InstrOpInfoFlags)v3, (MemorySize)v4);
					break;

				case CtorKind.nop:
					v = reader.ReadCompressedUInt32();
					v2 = reader.ReadByte();
					instrInfo = new SimpleInstrInfo_nop(code, (int)v, s, (Register)v2);
					break;

				case CtorKind.OpSize:
					v = reader.ReadByte();
					ca[0] = 'w';
					s2 = AddSuffix(s, ca);
					ca[0] = 'd';
					s3 = AddSuffix(s, ca);
					ca[0] = 'q';
					s4 = AddSuffix(s, ca);
					instrInfo = new SimpleInstrInfo_OpSize(code, (CodeSize)v, s, s2, s3, s4);
					break;

				case CtorKind.OpSize2_bnd:
					s2 = strings[reader.ReadCompressedUInt32()];
					s3 = strings[reader.ReadCompressedUInt32()];
					s4 = strings[reader.ReadCompressedUInt32()];
					instrInfo = new SimpleInstrInfo_OpSize2_bnd(code, s, s2, s3, s4);
					break;

				case CtorKind.OpSize3:
					ca[0] = (char)reader.ReadByte();
					s2 = AddSuffix(s, ca);
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_OpSize3(code, (int)v, s, s2);
					break;

				case CtorKind.os_2:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_os(code, (int)v, s);
					break;

				case CtorKind.os_3:
					v = reader.ReadCompressedUInt32();
					v2 = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_os(code, (int)v, s, (InstrOpInfoFlags)v2);
					break;

				case CtorKind.os_call_2:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_os_call(code, (int)v, s);
					break;

				case CtorKind.os_call_3:
					v = reader.ReadCompressedUInt32();
					v2 = reader.ReadByte();
					if (v2 > 1)
						throw new InvalidOperationException();
					instrInfo = new SimpleInstrInfo_os_call(code, (int)v, s, v2 != 0);
					break;

				case CtorKind.os_jcc_2:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_os_jcc(code, (int)v, s);
					break;

				case CtorKind.os_jcc_3:
					v = reader.ReadCompressedUInt32();
					v2 = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_os_jcc(code, (int)v, s, (InstrOpInfoFlags)v2);
					break;

				case CtorKind.os_loop:
					v = reader.ReadCompressedUInt32();
					v2 = reader.ReadByte();
					instrInfo = new SimpleInstrInfo_os_loop(code, (int)v, (Register)v2, s);
					break;

				case CtorKind.os_mem:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_os_mem(code, (int)v, s);
					break;

				case CtorKind.os_mem_reg16:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_os_mem_reg16(code, (int)v, s);
					break;

				case CtorKind.os_mem2:
					v = reader.ReadCompressedUInt32();
					v2 = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_os_mem2(code, (int)v, s, (InstrOpInfoFlags)v2);
					break;

				case CtorKind.pblendvb_1:
					instrInfo = new SimpleInstrInfo_pblendvb(code, s);
					break;

				case CtorKind.pblendvb_2:
					v = reader.ReadByte();
					instrInfo = new SimpleInstrInfo_pblendvb(code, s, (MemorySize)v);
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

				case CtorKind.Reg16:
					instrInfo = new SimpleInstrInfo_Reg16(code, s);
					break;

				case CtorKind.reverse2:
					instrInfo = new SimpleInstrInfo_reverse2(code, s);
					break;

				case CtorKind.sae:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_sae(code, (int)v, s);
					break;

				case CtorKind.sae_pops:
					v = reader.ReadCompressedUInt32();
					v2 = reader.ReadByte();
					instrInfo = new SimpleInstrInfo_sae_pops(code, (int)v, s, FormatterConstants.GetPseudoOps((PseudoOpsKind)v2));
					break;

				case CtorKind.SEX1:
					v = reader.ReadCompressedUInt32();
					v2 = reader.ReadByte();
					instrInfo = new SimpleInstrInfo_SEX1(code, (int)v, (SignExtendInfo)v2, s);
					break;

				case CtorKind.SEX1a:
					v = reader.ReadCompressedUInt32();
					v2 = reader.ReadByte();
					instrInfo = new SimpleInstrInfo_SEX1a(code, (int)v, (SignExtendInfo)v2, s);
					break;

				case CtorKind.SEX2_2:
					v = reader.ReadByte();
					instrInfo = new SimpleInstrInfo_SEX2(code, (SignExtendInfo)v, s);
					break;

				case CtorKind.SEX2_3:
					v = reader.ReadByte();
					v2 = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_SEX2(code, (SignExtendInfo)v, s, (InstrOpInfoFlags)v2);
					break;

				case CtorKind.SEX2_4:
					v = reader.ReadByte();
					v2 = reader.ReadByte();
					v3 = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_SEX2(code, (SignExtendInfo)v, (SignExtendInfo)v2, s, (InstrOpInfoFlags)v3);
					break;

				case CtorKind.SEX3:
					v = reader.ReadByte();
					instrInfo = new SimpleInstrInfo_SEX3(code, (SignExtendInfo)v, s);
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

				case CtorKind.STIG2_2a:
					v = reader.ReadByte();
					if (v > 1)
						throw new InvalidOperationException();
					instrInfo = new SimpleInstrInfo_STIG2(code, s, v != 0);
					break;

				case CtorKind.STIG2_2b:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_STIG2(code, s, (InstrOpInfoFlags)v);
					break;

				case CtorKind.xbegin:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_xbegin(code, (int)v, s);
					break;

				case CtorKind.XLAT:
					instrInfo = new SimpleInstrInfo_XLAT(code, s);
					break;

				case CtorKind.XY:
					instrInfo = new SimpleInstrInfo_XY(code, s);
					break;

				case CtorKind.YA:
					instrInfo = new SimpleInstrInfo_YA(code, s);
					break;

				case CtorKind.YD:
					instrInfo = new SimpleInstrInfo_YD(code, s);
					break;

				case CtorKind.YX:
					instrInfo = new SimpleInstrInfo_YX(code, s);
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
