// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt.intel;

import com.github.icedland.iced.x86.internal.IcedConstants;
import com.github.icedland.iced.x86.internal.ResourceReader;
import com.github.icedland.iced.x86.internal.fmt.FormatterStringsTable;

final class InstrInfos {
	static final InstrInfo[] allInfos = readInfos();

	private static byte[] getSerializedInstrInfos() {
		return ResourceReader.readByteArray(InstrInfos.class.getClassLoader(), "com/github/icedland/iced/x86/fmt/intel/InstrInfos.bin");
	}

	private static String addPrefix(String s, char[] ca) {
		return (new String(ca) + s).intern();
	}

	@SuppressWarnings("deprecation")
	private static InstrInfo[] readInfos() {
		com.github.icedland.iced.x86.internal.DataReader reader = new com.github.icedland.iced.x86.internal.DataReader(getSerializedInstrInfos());
		InstrInfo[] infos = new InstrInfo[IcedConstants.CODE_ENUM_COUNT];
		String[] strings = FormatterStringsTable.getStringsTable();

		char[] ca = new char[1];
		String s, s2, s3;
		int v, v2, v3;
		int prevIndex = -1;
		for (int i = 0; i < infos.length; i++) {
			byte f = (byte)reader.readByte();
			byte ctorKind = (byte)(f & 0x7F);
			int currentIndex;
			if (ctorKind == CtorKind.PREVIOUS) {
				currentIndex = reader.getIndex();
				reader.setIndex(prevIndex);
				ctorKind = (byte)(reader.readByte() & 0x7F);
			}
			else {
				currentIndex = -1;
				prevIndex = reader.getIndex() - 1;
			}
			s = strings[reader.readCompressedUInt32()];
			if ((f & 0x80) != 0) {
				ca[0] = 'v';
				s = addPrefix(s, ca);
			}
			InstrInfo instrInfo;
			switch (ctorKind) {
			case CtorKind.NORMAL_1:
				instrInfo = new SimpleInstrInfo(s);
				break;

			case CtorKind.NORMAL_2:
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo(s, v);
				break;

			case CtorKind.ASZ:
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_as(v, s);
				break;

			case CtorKind.STRING_IG0:
				instrInfo = new SimpleInstrInfo_StringIg0(s);
				break;

			case CtorKind.STRING_IG1:
				instrInfo = new SimpleInstrInfo_StringIg1(s);
				break;

			case CtorKind.BCST:
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_bcst(s, v);
				break;

			case CtorKind.BND:
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_bnd(s, v);
				break;

			case CtorKind.DECLARE_DATA:
				instrInfo = new SimpleInstrInfo_DeclareData(i, s);
				break;

			case CtorKind.ST_STI:
				v = reader.readByte();
				if (v > 1)
					throw new UnsupportedOperationException();
				instrInfo = new SimpleInstrInfo_ST_STi(s, v != 0);
				break;

			case CtorKind.STI_ST:
				v = reader.readByte();
				if (v > 1)
					throw new UnsupportedOperationException();
				instrInfo = new SimpleInstrInfo_STi_ST(s, v != 0);
				break;

			case CtorKind.IMUL:
				instrInfo = new SimpleInstrInfo_imul(s);
				break;

			case CtorKind.OPMASK_OP:
				instrInfo = new SimpleInstrInfo_opmask_op(s);
				break;

			case CtorKind.MASKMOVQ:
				instrInfo = new SimpleInstrInfo_maskmovq(s);
				break;

			case CtorKind.MEMSIZE:
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_memsize(v, s);
				break;

			case CtorKind.MOVABS:
				instrInfo = new SimpleInstrInfo_movabs(s);
				break;

			case CtorKind.NOP:
				v = reader.readCompressedUInt32();
				v2 = reader.readByte();
				instrInfo = new SimpleInstrInfo_nop(v, s, v2);
				break;

			case CtorKind.OS2:
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_os(v, s);
				break;

			case CtorKind.OS3:
				v = reader.readCompressedUInt32();
				v2 = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_os(v, s, v2);
				break;

			case CtorKind.OS_BND:
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_os_bnd(v, s);
				break;

			case CtorKind.CC_1:
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_cc(v, new String[] { s });
				break;

			case CtorKind.CC_2:
				s2 = strings[reader.readCompressedUInt32()];
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_cc(v, new String[] { s, s2 });
				break;

			case CtorKind.CC_3:
				s2 = strings[reader.readCompressedUInt32()];
				s3 = strings[reader.readCompressedUInt32()];
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_cc(v, new String[] { s, s2, s3 });
				break;

			case CtorKind.OS_JCC_A_1:
				v2 = reader.readCompressedUInt32();
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_os_jcc(v, v2, new String[] { s });
				break;

			case CtorKind.OS_JCC_A_2:
				s2 = strings[reader.readCompressedUInt32()];
				v2 = reader.readCompressedUInt32();
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_os_jcc(v, v2, new String[] { s, s2 });
				break;

			case CtorKind.OS_JCC_A_3:
				s2 = strings[reader.readCompressedUInt32()];
				s3 = strings[reader.readCompressedUInt32()];
				v2 = reader.readCompressedUInt32();
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_os_jcc(v, v2, new String[] { s, s2, s3 });
				break;

			case CtorKind.OS_JCC_B_1:
				v3 = reader.readCompressedUInt32();
				v = reader.readCompressedUInt32();
				v2 = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_os_jcc(v, v3, new String[] { s }, v2);
				break;

			case CtorKind.OS_JCC_B_2:
				s2 = strings[reader.readCompressedUInt32()];
				v3 = reader.readCompressedUInt32();
				v = reader.readCompressedUInt32();
				v2 = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_os_jcc(v, v3, new String[] { s, s2 }, v2);
				break;

			case CtorKind.OS_JCC_B_3:
				s2 = strings[reader.readCompressedUInt32()];
				s3 = strings[reader.readCompressedUInt32()];
				v3 = reader.readCompressedUInt32();
				v = reader.readCompressedUInt32();
				v2 = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_os_jcc(v, v3, new String[] { s, s2, s3 }, v2);
				break;

			case CtorKind.OS_LOOPCC:
				s2 = strings[reader.readCompressedUInt32()];
				v3 = reader.readCompressedUInt32();
				v = reader.readCompressedUInt32();
				v2 = reader.readByte();
				instrInfo = new SimpleInstrInfo_os_loop(v, v3, v2, new String[] { s, s2 });
				break;

			case CtorKind.OS_LOOP:
				v = reader.readCompressedUInt32();
				v2 = reader.readByte();
				instrInfo = new SimpleInstrInfo_os_loop(v, -1, v2, new String[] { s });
				break;

			case CtorKind.PCLMULQDQ:
				v = reader.readByte();
				instrInfo = new SimpleInstrInfo_pclmulqdq(s, com.github.icedland.iced.x86.internal.fmt.FormatterConstants.getPseudoOps(v));
				break;

			case CtorKind.POPS:
				v = reader.readByte();
				instrInfo = new SimpleInstrInfo_pops(s, com.github.icedland.iced.x86.internal.fmt.FormatterConstants.getPseudoOps(v));
				break;

			case CtorKind.REG:
				v = reader.readByte();
				instrInfo = new SimpleInstrInfo_reg(s, v);
				break;

			case CtorKind.REG16:
				instrInfo = new SimpleInstrInfo_Reg16(s);
				break;

			case CtorKind.REG32:
				instrInfo = new SimpleInstrInfo_Reg32(s);
				break;

			case CtorKind.ST1_2:
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_ST1(s, v);
				break;

			case CtorKind.ST1_3:
				v = reader.readCompressedUInt32();
				v2 = reader.readByte();
				if (v2 > 1)
					throw new UnsupportedOperationException();
				instrInfo = new SimpleInstrInfo_ST1(s, v, v2 != 0);
				break;

			case CtorKind.ST2:
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_ST2(s, v);
				break;

			case CtorKind.INVLPGA:
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_invlpga(v, s);
				break;

			default:
				throw new UnsupportedOperationException();
			}
			infos[i] = instrInfo;
			if (currentIndex >= 0)
				reader.setIndex(currentIndex);
		}
		if (reader.canRead())
			throw new UnsupportedOperationException();

		return infos;
	}
}
