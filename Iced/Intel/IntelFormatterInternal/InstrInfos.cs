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

#if !NO_INTEL_FORMATTER && !NO_FORMATTER
using System;
using Iced.Intel.FormatterInternal;
using Iced.Intel.Internal;

namespace Iced.Intel.IntelFormatterInternal {
	static partial class InstrInfos {
		public static readonly InstrInfo[] AllInfos = ReadInfos();

		static InstrInfo[] ReadInfos() {
			var reader = new DataReader(GetSerializedInstrInfos());
			var infos = new InstrInfo[DecoderConstants.NumberOfCodeValues];
			var strings = FormatterStringsTable.GetStringsTable();

			string s;
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

				case CtorKind.Normal_2:
					s = strings[reader.ReadCompressedUInt32()];
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo(code, s, (InstrOpInfoFlags)v);
					break;

				case CtorKind.asz:
					v = reader.ReadCompressedUInt32();
					s = strings[reader.ReadCompressedUInt32()];
					instrInfo = new SimpleInstrInfo_as(code, (int)v, s);
					break;

				case CtorKind.AX:
					s = strings[reader.ReadCompressedUInt32()];
					instrInfo = new SimpleInstrInfo_AX(code, s);
					break;

				case CtorKind.AY:
					s = strings[reader.ReadCompressedUInt32()];
					instrInfo = new SimpleInstrInfo_AY(code, s);
					break;

				case CtorKind.bcst:
					s = strings[reader.ReadCompressedUInt32()];
					v = reader.ReadCompressedUInt32();
					v2 = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_bcst(code, s, (InstrOpInfoFlags)v, (InstrOpInfoFlags)v2);
					break;

				case CtorKind.bnd_1:
					s = strings[reader.ReadCompressedUInt32()];
					instrInfo = new SimpleInstrInfo_bnd(code, s);
					break;

				case CtorKind.bnd_2:
					s = strings[reader.ReadCompressedUInt32()];
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_bnd(code, s, (InstrOpInfoFlags)v);
					break;

				case CtorKind.DeclareData:
					s = strings[reader.ReadCompressedUInt32()];
					instrInfo = new SimpleInstrInfo_DeclareData(code, s);
					break;

				case CtorKind.fpu_ST_STi:
					s = strings[reader.ReadCompressedUInt32()];
					instrInfo = new SimpleInstrInfo_fpu_ST_STi(code, s);
					break;

				case CtorKind.fpu_STi_ST:
					s = strings[reader.ReadCompressedUInt32()];
					instrInfo = new SimpleInstrInfo_fpu_STi_ST(code, s);
					break;

				case CtorKind.imul:
					s = strings[reader.ReadCompressedUInt32()];
					instrInfo = new SimpleInstrInfo_imul(code, s);
					break;

				case CtorKind.k1:
					s = strings[reader.ReadCompressedUInt32()];
					instrInfo = new SimpleInstrInfo_k1(code, s);
					break;

				case CtorKind.k2:
					s = strings[reader.ReadCompressedUInt32()];
					instrInfo = new SimpleInstrInfo_k2(code, s);
					break;

				case CtorKind.maskmovq:
					s = strings[reader.ReadCompressedUInt32()];
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_maskmovq(code, s, (InstrOpInfoFlags)v);
					break;

				case CtorKind.memsize:
					v = reader.ReadCompressedUInt32();
					s = strings[reader.ReadCompressedUInt32()];
					instrInfo = new SimpleInstrInfo_memsize(code, (int)v, s);
					break;

				case CtorKind.movabs:
					v = reader.ReadCompressedUInt32();
					s = strings[reader.ReadCompressedUInt32()];
					instrInfo = new SimpleInstrInfo_movabs(code, (int)v, s);
					break;

				case CtorKind.nop:
					v = reader.ReadCompressedUInt32();
					s = strings[reader.ReadCompressedUInt32()];
					v2 = reader.ReadByte();
					instrInfo = new SimpleInstrInfo_nop(code, (int)v, s, (Register)v2);
					break;

				case CtorKind.nop0F1F:
					v = reader.ReadByte();
					s = strings[reader.ReadCompressedUInt32()];
					v2 = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_nop0F1F(code, (Register)v, s, (InstrOpInfoFlags)v2);
					break;

				case CtorKind.os2:
					v = reader.ReadCompressedUInt32();
					s = strings[reader.ReadCompressedUInt32()];
					instrInfo = new SimpleInstrInfo_os(code, (int)v, s);
					break;

				case CtorKind.os3:
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

				case CtorKind.os_jcc_2:
					v = reader.ReadCompressedUInt32();
					s = strings[reader.ReadCompressedUInt32()];
					instrInfo = new SimpleInstrInfo_os_jcc(code, (int)v, s);
					break;

				case CtorKind.os_jcc_3:
					v = reader.ReadCompressedUInt32();
					s = strings[reader.ReadCompressedUInt32()];
					v2 = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_os_jcc(code, (int)v, s, (InstrOpInfoFlags)v2);
					break;

				case CtorKind.os_loop:
					v = reader.ReadCompressedUInt32();
					v2 = reader.ReadByte();
					s = strings[reader.ReadCompressedUInt32()];
					instrInfo = new SimpleInstrInfo_os_loop(code, (int)v, (Register)v2, s);
					break;

				case CtorKind.os_mem:
					v = reader.ReadCompressedUInt32();
					s = strings[reader.ReadCompressedUInt32()];
					instrInfo = new SimpleInstrInfo_os_mem(code, (int)v, s);
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

				case CtorKind.reg:
					s = strings[reader.ReadCompressedUInt32()];
					v = reader.ReadByte();
					instrInfo = new SimpleInstrInfo_reg(code, s, (Register)v);
					break;

				case CtorKind.Reg16:
					s = strings[reader.ReadCompressedUInt32()];
					instrInfo = new SimpleInstrInfo_Reg16(code, s);
					break;

				case CtorKind.ST_STi:
					s = strings[reader.ReadCompressedUInt32()];
					instrInfo = new SimpleInstrInfo_ST_STi(code, s);
					break;

				case CtorKind.ST1_2:
					s = strings[reader.ReadCompressedUInt32()];
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_ST1(code, s, (InstrOpInfoFlags)v);
					break;

				case CtorKind.ST1_3:
					s = strings[reader.ReadCompressedUInt32()];
					v = reader.ReadCompressedUInt32();
					v2 = reader.ReadByte();
					if (v2 > 1)
						throw new InvalidOperationException();
					instrInfo = new SimpleInstrInfo_ST1(code, s, (InstrOpInfoFlags)v, v2 != 0);
					break;

				case CtorKind.ST2:
					s = strings[reader.ReadCompressedUInt32()];
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_ST2(code, s, (InstrOpInfoFlags)v);
					break;

				case CtorKind.STi_ST:
					s = strings[reader.ReadCompressedUInt32()];
					instrInfo = new SimpleInstrInfo_STi_ST(code, s);
					break;

				case CtorKind.xbegin:
					v = reader.ReadCompressedUInt32();
					s = strings[reader.ReadCompressedUInt32()];
					instrInfo = new SimpleInstrInfo_xbegin(code, (int)v, s);
					break;

				case CtorKind.YA:
					s = strings[reader.ReadCompressedUInt32()];
					instrInfo = new SimpleInstrInfo_YA(code, s);
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
