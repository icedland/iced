// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt.masm;

import com.github.icedland.iced.x86.internal.IcedConstants;
import com.github.icedland.iced.x86.internal.ResourceReader;
import com.github.icedland.iced.x86.internal.fmt.FormatterStringsTable;

final class InstrInfos {
	static final InstrInfo[] allInfos = readInfos();

	private static byte[] getSerializedInstrInfos() {
		return ResourceReader.readByteArray(InstrInfos.class.getClassLoader(), "com/github/icedland/iced/x86/fmt/masm/InstrInfos.bin");
	}

	private static String addSuffix(String s, char[] ca) {
		return (s + new String(ca)).intern();
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
		String s, s2, s3, s4;
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

			case CtorKind.AAM_AAD:
				instrInfo = new SimpleInstrInfo_AamAad(s);
				break;

			case CtorKind.AX:
				ca[0] = (char)reader.readByte();
				s2 = addSuffix(s, ca);
				instrInfo = new SimpleInstrInfo_AX(s, s2);
				break;

			case CtorKind.AY:
				ca[0] = (char)reader.readByte();
				s2 = addSuffix(s, ca);
				instrInfo = new SimpleInstrInfo_AY(s, s2);
				break;

			case CtorKind.BND:
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_bnd(s, v);
				break;

			case CtorKind.DECLARE_DATA:
				instrInfo = new SimpleInstrInfo_DeclareData(i, s);
				break;

			case CtorKind.DX:
				ca[0] = (char)reader.readByte();
				s2 = addSuffix(s, ca);
				instrInfo = new SimpleInstrInfo_DX(s, s2);
				break;

			case CtorKind.FWORD:
				ca[0] = (char)reader.readByte();
				s2 = addSuffix(s, ca);
				v = reader.readByte();
				v2 = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_fword(v, v2, s, s2);
				break;

			case CtorKind.INT3:
				instrInfo = new SimpleInstrInfo_Int3(s);
				break;

			case CtorKind.IMUL:
				instrInfo = new SimpleInstrInfo_imul(s);
				break;

			case CtorKind.INVLPGA:
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_invlpga(v, s);
				break;

			case CtorKind.CCA_1:
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_cc(v, new String[] { s });
				break;

			case CtorKind.CCA_2:
				s2 = strings[reader.readCompressedUInt32()];
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_cc(v, new String[] { s, s2 });
				break;

			case CtorKind.CCA_3:
				s2 = strings[reader.readCompressedUInt32()];
				s3 = strings[reader.readCompressedUInt32()];
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_cc(v, new String[] { s, s2, s3 });
				break;

			case CtorKind.CCB_1:
				v2 = reader.readCompressedUInt32();
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_cc(v2, new String[] { s }, v);
				break;

			case CtorKind.CCB_2:
				s2 = strings[reader.readCompressedUInt32()];
				v2 = reader.readCompressedUInt32();
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_cc(v2, new String[] { s, s2 }, v);
				break;

			case CtorKind.CCB_3:
				s2 = strings[reader.readCompressedUInt32()];
				s3 = strings[reader.readCompressedUInt32()];
				v2 = reader.readCompressedUInt32();
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_cc(v2, new String[] { s, s2, s3 }, v);
				break;

			case CtorKind.JCC_1:
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_jcc(v, new String[] { s });
				break;

			case CtorKind.JCC_2:
				s2 = strings[reader.readCompressedUInt32()];
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_jcc(v, new String[] { s, s2 });
				break;

			case CtorKind.JCC_3:
				s2 = strings[reader.readCompressedUInt32()];
				s3 = strings[reader.readCompressedUInt32()];
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_jcc(v, new String[] { s, s2, s3 });
				break;

			case CtorKind.LOOPCC1:
				s2 = strings[reader.readCompressedUInt32()];
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_cc(v, new String[] { s, s2 });
				break;

			case CtorKind.LOOPCC2:
				s2 = strings[reader.readCompressedUInt32()];
				ca[0] = (char)reader.readByte();
				v2 = reader.readCompressedUInt32();
				s3 = addSuffix(s, ca);
				s4 = addSuffix(s2, ca);
				v = reader.readByte();
				instrInfo = new SimpleInstrInfo_OpSize_cc(v, v2, new String[] { s, s2 }, new String[] { s3, s4 });
				break;

			case CtorKind.MASKMOVQ:
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_maskmovq(s, v);
				break;

			case CtorKind.MEMSIZE:
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_memsize(v, s);
				break;

			case CtorKind.MONITOR:
				v = reader.readByte();
				v2 = reader.readByte();
				v3 = reader.readByte();
				instrInfo = new SimpleInstrInfo_monitor(s, v, v2, v3);
				break;

			case CtorKind.MWAIT:
				instrInfo = new SimpleInstrInfo_mwait(s);
				break;

			case CtorKind.MWAITX:
				instrInfo = new SimpleInstrInfo_mwaitx(s);
				break;

			case CtorKind.NOP:
				v = reader.readCompressedUInt32();
				v2 = reader.readByte();
				instrInfo = new SimpleInstrInfo_nop(v, s, v2);
				break;

			case CtorKind.OP_SIZE_1:
				v = reader.readByte();
				ca[0] = 'w';
				s2 = addSuffix(s, ca);
				ca[0] = 'd';
				s3 = addSuffix(s, ca);
				ca[0] = 'q';
				s4 = addSuffix(s, ca);
				instrInfo = new SimpleInstrInfo_OpSize(v, s, s2, s3, s4);
				break;

			case CtorKind.OP_SIZE_2:
				ca[0] = (char)reader.readByte();
				s2 = addSuffix(s, ca);
				v = reader.readByte();
				instrInfo = new SimpleInstrInfo_OpSize(v, s, s2, s2, s2);
				break;

			case CtorKind.OP_SIZE2:
				s2 = strings[reader.readCompressedUInt32()];
				s3 = strings[reader.readCompressedUInt32()];
				s4 = strings[reader.readCompressedUInt32()];
				v = reader.readByte();
				if (v > 1)
					throw new UnsupportedOperationException();
				instrInfo = new SimpleInstrInfo_OpSize2(s, s2, s3, s4, v != 0);
				break;

			case CtorKind.PBLENDVB:
				instrInfo = new SimpleInstrInfo_pblendvb(s);
				break;

			case CtorKind.PCLMULQDQ:
				v = reader.readByte();
				instrInfo = new SimpleInstrInfo_pclmulqdq(s, com.github.icedland.iced.x86.internal.fmt.FormatterConstants.getPseudoOps(v));
				break;

			case CtorKind.POPS_2:
				v = reader.readByte();
				instrInfo = new SimpleInstrInfo_pops(s, com.github.icedland.iced.x86.internal.fmt.FormatterConstants.getPseudoOps(v));
				break;

			case CtorKind.POPS_3:
				v = reader.readByte();
				v2 = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_pops(s, com.github.icedland.iced.x86.internal.fmt.FormatterConstants.getPseudoOps(v), v2);
				break;

			case CtorKind.REG:
				v = reader.readByte();
				instrInfo = new SimpleInstrInfo_reg(s, v);
				break;

			case CtorKind.REG16:
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_Reg16(s, v);
				break;

			case CtorKind.REG32:
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_Reg32(s, v);
				break;

			case CtorKind.REVERSE:
				instrInfo = new SimpleInstrInfo_reverse(s);
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

			case CtorKind.XLAT:
				ca[0] = 'b';
				s2 = addSuffix(s, ca);
				instrInfo = new SimpleInstrInfo_XLAT(s, s2);
				break;

			case CtorKind.XY:
				ca[0] = (char)reader.readByte();
				s2 = addSuffix(s, ca);
				instrInfo = new SimpleInstrInfo_XY(s, s2);
				break;

			case CtorKind.YA:
				ca[0] = (char)reader.readByte();
				s2 = addSuffix(s, ca);
				instrInfo = new SimpleInstrInfo_YA(s, s2);
				break;

			case CtorKind.YD:
				ca[0] = (char)reader.readByte();
				s2 = addSuffix(s, ca);
				instrInfo = new SimpleInstrInfo_YD(s, s2);
				break;

			case CtorKind.YX:
				ca[0] = (char)reader.readByte();
				s2 = addSuffix(s, ca);
				instrInfo = new SimpleInstrInfo_YX(s, s2);
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
