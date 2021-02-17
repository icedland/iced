// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if INTEL
using System;
using Iced.Intel.FormatterInternal;
using Iced.Intel.Internal;

namespace Iced.Intel.IntelFormatterInternal {
	static partial class InstrInfos {
		public static readonly InstrInfo[] AllInfos = ReadInfos();

		static string AddPrefix(string s, char[] ca) =>
			string.Intern(new string(ca) + s);

		static InstrInfo[] ReadInfos() {
			var reader = new DataReader(GetSerializedInstrInfos());
			var infos = new InstrInfo[IcedConstants.CodeEnumCount];
			var strings = FormatterStringsTable.GetStringsTable();

			var ca = new char[1];
			string s, s2, s3;
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

				case CtorKind.asz:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_as((int)v, s);
					break;

				case CtorKind.StringIg0:
					instrInfo = new SimpleInstrInfo_StringIg0(s);
					break;

				case CtorKind.StringIg1:
					instrInfo = new SimpleInstrInfo_StringIg1(s);
					break;

				case CtorKind.bcst:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_bcst(s, (InstrOpInfoFlags)v);
					break;

				case CtorKind.bnd:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_bnd(s, (InstrOpInfoFlags)v);
					break;

				case CtorKind.DeclareData:
					instrInfo = new SimpleInstrInfo_DeclareData((Code)i, s);
					break;

				case CtorKind.ST_STi:
					v = reader.ReadByte();
					if (v > 1)
						throw new InvalidOperationException();
					instrInfo = new SimpleInstrInfo_ST_STi(s, v != 0);
					break;

				case CtorKind.STi_ST:
					v = reader.ReadByte();
					if (v > 1)
						throw new InvalidOperationException();
					instrInfo = new SimpleInstrInfo_STi_ST(s, v != 0);
					break;

				case CtorKind.imul:
					instrInfo = new SimpleInstrInfo_imul(s);
					break;

				case CtorKind.opmask_op:
					instrInfo = new SimpleInstrInfo_opmask_op(s);
					break;

				case CtorKind.maskmovq:
					instrInfo = new SimpleInstrInfo_maskmovq(s);
					break;

				case CtorKind.memsize:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_memsize((int)v, s);
					break;

				case CtorKind.movabs:
					instrInfo = new SimpleInstrInfo_movabs(s);
					break;

				case CtorKind.nop:
					v = reader.ReadCompressedUInt32();
					v2 = reader.ReadByte();
					instrInfo = new SimpleInstrInfo_nop((int)v, s, (Register)v2);
					break;

				case CtorKind.os2:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_os((int)v, s);
					break;

				case CtorKind.os3:
					v = reader.ReadCompressedUInt32();
					v2 = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_os((int)v, s, (InstrOpInfoFlags)v2);
					break;

				case CtorKind.os_bnd:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_os_bnd((int)v, s);
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

				case CtorKind.pclmulqdq:
					v = reader.ReadByte();
					instrInfo = new SimpleInstrInfo_pclmulqdq(s, FormatterConstants.GetPseudoOps((PseudoOpsKind)v));
					break;

				case CtorKind.pops:
					v = reader.ReadByte();
					instrInfo = new SimpleInstrInfo_pops(s, FormatterConstants.GetPseudoOps((PseudoOpsKind)v));
					break;

				case CtorKind.reg:
					v = reader.ReadByte();
					instrInfo = new SimpleInstrInfo_reg(s, (Register)v);
					break;

				case CtorKind.Reg16:
					instrInfo = new SimpleInstrInfo_Reg16(s);
					break;

				case CtorKind.Reg32:
					instrInfo = new SimpleInstrInfo_Reg32(s);
					break;

				case CtorKind.ST1_2:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_ST1(s, (InstrOpInfoFlags)v);
					break;

				case CtorKind.ST1_3:
					v = reader.ReadCompressedUInt32();
					v2 = reader.ReadByte();
					if (v2 > 1)
						throw new InvalidOperationException();
					instrInfo = new SimpleInstrInfo_ST1(s, (InstrOpInfoFlags)v, v2 != 0);
					break;

				case CtorKind.ST2:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_ST2(s, (InstrOpInfoFlags)v);
					break;

				case CtorKind.invlpga:
					v = reader.ReadCompressedUInt32();
					instrInfo = new SimpleInstrInfo_invlpga((int)v, s);
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
