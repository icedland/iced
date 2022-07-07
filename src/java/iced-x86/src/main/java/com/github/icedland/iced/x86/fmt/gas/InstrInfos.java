// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt.gas;

import com.github.icedland.iced.x86.internal.IcedConstants;
import com.github.icedland.iced.x86.internal.ResourceReader;
import com.github.icedland.iced.x86.internal.fmt.FormatterStringsTable;

final class InstrInfos {
	static final InstrInfo[] allInfos = readInfos();

	private static byte[] getSerializedInstrInfos() {
		return ResourceReader.readByteArray(InstrInfos.class.getClassLoader(), "com/github/icedland/iced/x86/fmt/gas/InstrInfos.bin");
	}

	private static String addSuffix(String s, char[] ca) {
		return ca[0] == '\0' ? s : (s + new String(ca).intern());
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
		String s, s2, s3, s4, s5, s6;
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

			case CtorKind.NORMAL_2A:
				ca[0] = (char)reader.readByte();
				s2 = addSuffix(s, ca);
				instrInfo = new SimpleInstrInfo(s, s2);
				break;

			case CtorKind.NORMAL_2B:
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo(s, v);
				break;

			case CtorKind.NORMAL_2C:
				ca[0] = (char)reader.readByte();
				s = addSuffix(s, ca);
				instrInfo = new SimpleInstrInfo(s);
				break;

			case CtorKind.NORMAL_3:
				ca[0] = (char)reader.readByte();
				s2 = addSuffix(s, ca);
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo(s, s2, v);
				break;

			case CtorKind.AAM_AAD:
				instrInfo = new SimpleInstrInfo_AamAad(s);
				break;

			case CtorKind.ASZ:
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_as(v, s);
				break;

			case CtorKind.BND:
				ca[0] = (char)reader.readByte();
				s2 = addSuffix(s, ca);
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_bnd(s, s2, v);
				break;

			case CtorKind.DECLARE_DATA:
				instrInfo = new SimpleInstrInfo_DeclareData(i, s);
				break;

			case CtorKind.ER_2:
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_er(v, s);
				break;

			case CtorKind.ER_4:
				ca[0] = (char)reader.readByte();
				s2 = addSuffix(s, ca);
				v = reader.readCompressedUInt32();
				v2 = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_er(v, s, s2, v2);
				break;

			case CtorKind.FAR:
				ca[0] = (char)reader.readByte();
				s2 = addSuffix(s, ca);
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_far(v, s, s2);
				break;

			case CtorKind.IMUL:
				ca[0] = (char)reader.readByte();
				s2 = addSuffix(s, ca);
				instrInfo = new SimpleInstrInfo_imul(s, s2);
				break;

			case CtorKind.MASKMOVQ:
				instrInfo = new SimpleInstrInfo_maskmovq(s);
				break;

			case CtorKind.MOVABS:
				ca[0] = (char)reader.readByte();
				s2 = addSuffix(s, ca);
				s3 = strings[reader.readCompressedUInt32()];
				s4 = addSuffix(s3, ca);
				instrInfo = new SimpleInstrInfo_movabs(s, s2, s3, s4);
				break;

			case CtorKind.NOP:
				v = reader.readCompressedUInt32();
				v2 = reader.readByte();
				instrInfo = new SimpleInstrInfo_nop(v, s, v2);
				break;

			case CtorKind.OP_SIZE:
				v = reader.readByte();
				s2 = (s + "w").intern();
				s3 = (s + "l").intern();
				s4 = (s + "q").intern();
				instrInfo = new SimpleInstrInfo_OpSize(v, s, s2, s3, s4);
				break;

			case CtorKind.OP_SIZE2_BND:
				s2 = strings[reader.readCompressedUInt32()];
				s3 = strings[reader.readCompressedUInt32()];
				s4 = strings[reader.readCompressedUInt32()];
				instrInfo = new SimpleInstrInfo_OpSize2_bnd(s, s2, s3, s4);
				break;

			case CtorKind.OP_SIZE3:
				ca[0] = (char)reader.readByte();
				s2 = addSuffix(s, ca);
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_OpSize3(v, s, s2);
				break;

			case CtorKind.OS:
				v = reader.readCompressedUInt32();
				v2 = reader.readByte();
				if (v2 > 1)
					throw new UnsupportedOperationException();
				v3 = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_os(v, s, v2 != 0, v3);
				break;

			case CtorKind.CC_1:
				ca[0] = (char)reader.readByte();
				s2 = addSuffix(s, ca);
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_cc(v, new String[] { s }, new String[] { s2 });
				break;

			case CtorKind.CC_2:
				s2 = strings[reader.readCompressedUInt32()];
				ca[0] = (char)reader.readByte();
				v = reader.readCompressedUInt32();
				s3 = addSuffix(s, ca);
				s4 = addSuffix(s2, ca);
				instrInfo = new SimpleInstrInfo_cc(v, new String[] { s, s2 }, new String[] { s3, s4 });
				break;

			case CtorKind.CC_3:
				s2 = strings[reader.readCompressedUInt32()];
				s3 = strings[reader.readCompressedUInt32()];
				ca[0] = (char)reader.readByte();
				v = reader.readCompressedUInt32();
				s4 = addSuffix(s, ca);
				s5 = addSuffix(s2, ca);
				s6 = addSuffix(s3, ca);
				instrInfo = new SimpleInstrInfo_cc(v, new String[] { s, s2, s3 }, new String[] { s4, s5, s6 });
				break;

			case CtorKind.OS_JCC_1:
				v2 = reader.readCompressedUInt32();
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_os_jcc(v, v2, new String[] { s });
				break;

			case CtorKind.OS_JCC_2:
				s2 = strings[reader.readCompressedUInt32()];
				v2 = reader.readCompressedUInt32();
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_os_jcc(v, v2, new String[] { s, s2 });
				break;

			case CtorKind.OS_JCC_3:
				s2 = strings[reader.readCompressedUInt32()];
				s3 = strings[reader.readCompressedUInt32()];
				v2 = reader.readCompressedUInt32();
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_os_jcc(v, v2, new String[] { s, s2, s3 });
				break;

			case CtorKind.OS_LOOPCC:
				s2 = strings[reader.readCompressedUInt32()];
				ca[0] = (char)reader.readByte();
				s3 = addSuffix(s, ca);
				s4 = addSuffix(s2, ca);
				v3 = reader.readCompressedUInt32();
				v = reader.readCompressedUInt32();
				v2 = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_os_loop(v, v2, v3, new String[] { s, s2 }, new String[] { s3, s4 });
				break;

			case CtorKind.OS_LOOP:
				ca[0] = (char)reader.readByte();
				s2 = addSuffix(s, ca);
				v = reader.readCompressedUInt32();
				v2 = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_os_loop(v, v2, -1, new String[] { s }, new String[] { s2 });
				break;

			case CtorKind.OS_MEM:
				ca[0] = (char)reader.readByte();
				s2 = addSuffix(s, ca);
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_os_mem(v, s, s2);
				break;

			case CtorKind.REG16:
				ca[0] = 'w';
				s2 = addSuffix(s, ca);
				instrInfo = new SimpleInstrInfo_Reg16(s, s2);
				break;

			case CtorKind.OS_MEM2:
				ca[0] = (char)reader.readByte();
				s2 = addSuffix(s, ca);
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_os_mem2(v, s, s2);
				break;

			case CtorKind.OS2_3:
				ca[0] = (char)reader.readByte();
				s2 = addSuffix(s, ca);
				v = reader.readCompressedUInt32();
				v2 = reader.readByte();
				if (v2 > 1)
					throw new UnsupportedOperationException();
				instrInfo = new SimpleInstrInfo_os2(v, s, s2, v2 != 0, InstrOpInfoFlags.NONE);
				break;

			case CtorKind.OS2_4:
				ca[0] = (char)reader.readByte();
				s2 = addSuffix(s, ca);
				v = reader.readCompressedUInt32();
				v2 = reader.readByte();
				if (v2 > 1)
					throw new UnsupportedOperationException();
				v3 = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_os2(v, s, s2, v2 != 0, v3);
				break;

			case CtorKind.PBLENDVB:
				instrInfo = new SimpleInstrInfo_pblendvb(s);
				break;

			case CtorKind.PCLMULQDQ:
				v = reader.readByte();
				instrInfo = new SimpleInstrInfo_pclmulqdq(s, com.github.icedland.iced.x86.internal.fmt.FormatterConstants.getPseudoOps(v));
				break;

			case CtorKind.POPS:
				v = reader.readByte();
				v2 = reader.readByte();
				if (v2 > 1)
					throw new UnsupportedOperationException();
				instrInfo = new SimpleInstrInfo_pops(s, com.github.icedland.iced.x86.internal.fmt.FormatterConstants.getPseudoOps(v), v2 != 0);
				break;

			case CtorKind.MEM16:
				ca[0] = (char)reader.readByte();
				s2 = addSuffix(s, ca);
				ca[0] = 'w';
				s3 = addSuffix(s, ca);
				instrInfo = new SimpleInstrInfo_mem16(s, s2, s3);
				break;

			case CtorKind.REG32:
				instrInfo = new SimpleInstrInfo_Reg32(s);
				break;

			case CtorKind.SAE:
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_sae(v, s);
				break;

			case CtorKind.ST_STI:
				instrInfo = new SimpleInstrInfo_ST_STi(s);
				break;

			case CtorKind.STI_ST:
				v = reader.readByte();
				if (v > 1)
					throw new UnsupportedOperationException();
				instrInfo = new SimpleInstrInfo_STi_ST(s, v != 0);
				break;

			case CtorKind.STIG1:
				v = reader.readByte();
				if (v > 1)
					throw new UnsupportedOperationException();
				instrInfo = new SimpleInstrInfo_STIG1(s, v != 0);
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
