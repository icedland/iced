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

#if !NO_GAS_FORMATTER && !NO_FORMATTER
using System;
using Iced.Intel.FormatterInternal;

namespace Iced.Intel.GasFormatterInternal {
	static partial class InstrInfos {
		public static readonly InstrInfo[] AllInfos = ReadInfos();

		static string AddSuffix(string s, char[] ca) =>
			ca[0] == '\0' ? s : string.Intern(s + new string(ca));

		static InstrInfo[] ReadInfos() {
			var reader = new DataReader(GetSerializedInstrInfos());
			var infos = new InstrInfo[DecoderConstants.NumberOfCodeValues];
			var strings = FormatterStringsTable.GetStringsTable();

			var ca = new char[1];
			string s, s2, s3, s4;
			uint v, v2;
			int prevIndex = -1;
			for (int i = 0; i < infos.Length; i++) {
				var code = (Code)i;
				var ctorKind = (CtorKind)reader.ReadByte();
				int currentIndex;
				if (ctorKind == CtorKind.Previous) {
					currentIndex = reader.Index;
					reader.Index = prevIndex;
					ctorKind = (CtorKind)reader.ReadByte();
				}
				else {
					currentIndex = -1;
					prevIndex = reader.Index - 1;
				}
				InstrInfo instrInfo;
				switch (ctorKind) {
				case CtorKind.Normal_1:
					s = strings[reader.ReadCompressedUInt32()];
					instrInfo = new SimpleInstrInfo(code, s);
					break;

				case CtorKind.Normal_2a:
					s = strings[reader.ReadCompressedUInt32()];
					ca[0] = (char)reader.ReadByte();
					s2 = AddSuffix(s, ca);
					instrInfo = new SimpleInstrInfo(code, s, s2);
					break;

				case CtorKind.Normal_2b:
					s = strings[reader.ReadCompressedUInt32()];
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo(code, s, (InstrOpInfoFlags)v);
					break;

				case CtorKind.Normal_3:
					s = strings[reader.ReadCompressedUInt32()];
					ca[0] = (char)reader.ReadByte();
					s2 = AddSuffix(s, ca);
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo(code, s, s2, (InstrOpInfoFlags)v);
					break;

				case CtorKind.AamAad:
					s = strings[reader.ReadCompressedUInt32()];
					instrInfo = new SimpleInstrInfo_AamAad(code, s);
					break;

				case CtorKind.asz:
					v = reader.ReadCompressedUInt32();
					s = strings[reader.ReadCompressedUInt32()];
					instrInfo = new SimpleInstrInfo_as(code, (int)v, s);
					break;

				case CtorKind.bnd2_2:
					s = strings[reader.ReadCompressedUInt32()];
					ca[0] = (char)reader.ReadByte();
					s2 = AddSuffix(s, ca);
					instrInfo = new SimpleInstrInfo_bnd2(code, s, s2);
					break;

				case CtorKind.bnd2_3:
					s = strings[reader.ReadCompressedUInt32()];
					ca[0] = (char)reader.ReadByte();
					s2 = AddSuffix(s, ca);
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_bnd2(code, s, s2, (InstrOpInfoFlags)v);
					break;

				case CtorKind.DeclareData:
					s = strings[reader.ReadCompressedUInt32()];
					instrInfo = new SimpleInstrInfo_DeclareData(code, s);
					break;

				case CtorKind.er_2:
					v = reader.ReadCompressedUInt32();
					s = strings[reader.ReadCompressedUInt32()];
					instrInfo = new SimpleInstrInfo_er(code, (int)v, s);
					break;

				case CtorKind.er_4:
					v = reader.ReadCompressedUInt32();
					s = strings[reader.ReadCompressedUInt32()];
					ca[0] = (char)reader.ReadByte();
					s2 = AddSuffix(s, ca);
					v2 = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_er(code, (int)v, s, s2, (InstrOpInfoFlags)v2);
					break;

				case CtorKind.far:
					v = reader.ReadCompressedUInt32();
					s = strings[reader.ReadCompressedUInt32()];
					ca[0] = (char)reader.ReadByte();
					s2 = AddSuffix(s, ca);
					instrInfo = new SimpleInstrInfo_far(code, (int)v, s, s2);
					break;

				case CtorKind.imul:
					s = strings[reader.ReadCompressedUInt32()];
					ca[0] = (char)reader.ReadByte();
					s2 = AddSuffix(s, ca);
					instrInfo = new SimpleInstrInfo_imul(code, s, s2);
					break;

				case CtorKind.maskmovq:
					s = strings[reader.ReadCompressedUInt32()];
					instrInfo = new SimpleInstrInfo_maskmovq(code, s);
					break;

				case CtorKind.movabs:
					v = reader.ReadCompressedUInt32();
					s = strings[reader.ReadCompressedUInt32()];
					ca[0] = (char)reader.ReadByte();
					s2 = AddSuffix(s, ca);
					s3 = strings[reader.ReadCompressedUInt32()];
					ca[0] = (char)reader.ReadByte();
					s4 = AddSuffix(s3, ca);
					instrInfo = new SimpleInstrInfo_movabs(code, (int)v, s, s2, s3, s4);
					break;

				case CtorKind.nop:
					v = reader.ReadCompressedUInt32();
					s = strings[reader.ReadCompressedUInt32()];
					v2 = reader.ReadByte();
					instrInfo = new SimpleInstrInfo_nop(code, (int)v, s, (Register)v2);
					break;

				case CtorKind.OpSize:
					v = reader.ReadByte();
					s = strings[reader.ReadCompressedUInt32()];
					s2 = string.Intern(s + "w");
					s3 = string.Intern(s + "l");
					s4 = string.Intern(s + "q");
					instrInfo = new SimpleInstrInfo_OpSize(code, (CodeSize)v, s, s2, s3, s4);
					break;

				case CtorKind.OpSize2_bnd:
					s = strings[reader.ReadCompressedUInt32()];
					s2 = strings[reader.ReadCompressedUInt32()];
					s3 = strings[reader.ReadCompressedUInt32()];
					s4 = strings[reader.ReadCompressedUInt32()];
					instrInfo = new SimpleInstrInfo_OpSize2_bnd(code, s, s2, s3, s4);
					break;

				case CtorKind.OpSize3:
					v = reader.ReadCompressedUInt32();
					s = strings[reader.ReadCompressedUInt32()];
					ca[0] = (char)reader.ReadByte();
					s2 = AddSuffix(s, ca);
					instrInfo = new SimpleInstrInfo_OpSize3(code, (int)v, s, s2);
					break;

				case CtorKind.os_A:
					v = reader.ReadCompressedUInt32();
					s = strings[reader.ReadCompressedUInt32()];
					instrInfo = new SimpleInstrInfo_os(code, (int)v, s);
					break;

				case CtorKind.os_B:
					v = reader.ReadCompressedUInt32();
					s = strings[reader.ReadCompressedUInt32()];
					v2 = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_os(code, (int)v, s, (InstrOpInfoFlags)v2);
					break;

				case CtorKind.os_bnd:
					v = reader.ReadCompressedUInt32();
					s = strings[reader.ReadCompressedUInt32()];
					instrInfo = new SimpleInstrInfo_os_bnd(code, (int)v, s);
					break;

				case CtorKind.os_jcc:
					v = reader.ReadCompressedUInt32();
					s = strings[reader.ReadCompressedUInt32()];
					instrInfo = new SimpleInstrInfo_os_jcc(code, (int)v, s);
					break;

				case CtorKind.os_loop:
					v = reader.ReadCompressedUInt32();
					v2 = reader.ReadCompressedUInt32();
					s = strings[reader.ReadCompressedUInt32()];
					ca[0] = (char)reader.ReadByte();
					s2 = AddSuffix(s, ca);
					instrInfo = new SimpleInstrInfo_os_loop(code, (int)v, (int)v2, s, s2);
					break;

				case CtorKind.os_mem:
					v = reader.ReadCompressedUInt32();
					s = strings[reader.ReadCompressedUInt32()];
					ca[0] = (char)reader.ReadByte();
					s2 = AddSuffix(s, ca);
					instrInfo = new SimpleInstrInfo_os_mem(code, (int)v, s, s2);
					break;

				case CtorKind.os_mem_reg16:
					v = reader.ReadCompressedUInt32();
					s = strings[reader.ReadCompressedUInt32()];
					instrInfo = new SimpleInstrInfo_os_mem_reg16(code, (int)v, s);
					break;

				case CtorKind.os_mem2:
					v = reader.ReadCompressedUInt32();
					s = strings[reader.ReadCompressedUInt32()];
					ca[0] = (char)reader.ReadByte();
					s2 = AddSuffix(s, ca);
					instrInfo = new SimpleInstrInfo_os_mem2(code, (int)v, s, s2);
					break;

				case CtorKind.os2_3:
					v = reader.ReadCompressedUInt32();
					s = strings[reader.ReadCompressedUInt32()];
					ca[0] = (char)reader.ReadByte();
					s2 = AddSuffix(s, ca);
					instrInfo = new SimpleInstrInfo_os2(code, (int)v, s, s2);
					break;

				case CtorKind.os2_4:
					v = reader.ReadCompressedUInt32();
					s = strings[reader.ReadCompressedUInt32()];
					ca[0] = (char)reader.ReadByte();
					s2 = AddSuffix(s, ca);
					v2 = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_os2(code, (int)v, s, s2, (InstrOpInfoFlags)v2);
					break;

				case CtorKind.os2_bnd:
					v = reader.ReadCompressedUInt32();
					s = strings[reader.ReadCompressedUInt32()];
					ca[0] = (char)reader.ReadByte();
					s2 = AddSuffix(s, ca);
					instrInfo = new SimpleInstrInfo_os2_bnd(code, (int)v, s, s2);
					break;

				case CtorKind.pblendvb:
					s = strings[reader.ReadCompressedUInt32()];
					instrInfo = new SimpleInstrInfo_pblendvb(code, s);
					break;

				case CtorKind.pclmulqdq:
					s = strings[reader.ReadCompressedUInt32()];
					v = reader.ReadByte();
					instrInfo = new SimpleInstrInfo_pclmulqdq(code, s, FormatterConstants.GetPseudoOps((PseudoOpsKind)v));
					break;

				case CtorKind.pops:
					s = strings[reader.ReadCompressedUInt32()];
					v = reader.ReadByte();
					instrInfo = new SimpleInstrInfo_pops(code, s, FormatterConstants.GetPseudoOps((PseudoOpsKind)v));
					break;

				case CtorKind.Reg16:
					s = strings[reader.ReadCompressedUInt32()];
					instrInfo = new SimpleInstrInfo_Reg16(code, s);
					break;

				case CtorKind.sae:
					v = reader.ReadCompressedUInt32();
					s = strings[reader.ReadCompressedUInt32()];
					instrInfo = new SimpleInstrInfo_sae(code, (int)v, s);
					break;

				case CtorKind.sae_pops:
					v = reader.ReadCompressedUInt32();
					s = strings[reader.ReadCompressedUInt32()];
					v2 = reader.ReadByte();
					instrInfo = new SimpleInstrInfo_sae_pops(code, (int)v, s, FormatterConstants.GetPseudoOps((PseudoOpsKind)v2));
					break;

				case CtorKind.ST_STi:
					s = strings[reader.ReadCompressedUInt32()];
					instrInfo = new SimpleInstrInfo_ST_STi(code, s);
					break;

				case CtorKind.STi_ST:
					s = strings[reader.ReadCompressedUInt32()];
					instrInfo = new SimpleInstrInfo_STi_ST(code, s);
					break;

				case CtorKind.STi_ST2:
					s = strings[reader.ReadCompressedUInt32()];
					instrInfo = new SimpleInstrInfo_STi_ST2(code, s);
					break;

				case CtorKind.STIG_1a:
					s = strings[reader.ReadCompressedUInt32()];
					instrInfo = new SimpleInstrInfo_STIG1(code, s);
					break;

				case CtorKind.STIG_1b:
					s = strings[reader.ReadCompressedUInt32()];
					v = reader.ReadByte();
					if (v > 1)
						throw new InvalidOperationException();
					instrInfo = new SimpleInstrInfo_STIG1(code, s, v != 0);
					break;

				case CtorKind.xbegin:
					v = reader.ReadCompressedUInt32();
					s = strings[reader.ReadCompressedUInt32()];
					instrInfo = new SimpleInstrInfo_xbegin(code, (int)v, s);
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
