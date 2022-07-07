// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt.nasm;

import com.github.icedland.iced.x86.internal.IcedConstants;
import com.github.icedland.iced.x86.internal.ResourceReader;
import com.github.icedland.iced.x86.internal.fmt.FormatterStringsTable;

final class InstrInfos {
	static final InstrInfo[] allInfos = readInfos();

	private static byte[] getSerializedInstrInfos() {
		return ResourceReader.readByteArray(InstrInfos.class.getClassLoader(), "com/github/icedland/iced/x86/fmt/nasm/InstrInfos.bin");
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

			case CtorKind.ASZ:
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_as(v, s);
				break;

			case CtorKind.STRING:
				instrInfo = new SimpleInstrInfo_String(s);
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

			case CtorKind.ER_2:
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_er(v, s);
				break;

			case CtorKind.ER_3:
				v = reader.readCompressedUInt32();
				v2 = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_er(v, s, v2);
				break;

			case CtorKind.FAR:
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_far(v, s);
				break;

			case CtorKind.FAR_MEM:
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_far_mem(v, s);
				break;

			case CtorKind.INVLPGA:
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_invlpga(v, s);
				break;

			case CtorKind.MASKMOVQ:
				instrInfo = new SimpleInstrInfo_maskmovq(s);
				break;

			case CtorKind.MOVABS:
				instrInfo = new SimpleInstrInfo_movabs(s);
				break;

			case CtorKind.NOP:
				v = reader.readCompressedUInt32();
				v2 = reader.readByte();
				instrInfo = new SimpleInstrInfo_nop(v, s, v2);
				break;

			case CtorKind.OP_SIZE:
				v = reader.readByte();
				ca[0] = 'w';
				s2 = addSuffix(s, ca);
				ca[0] = 'd';
				s3 = addSuffix(s, ca);
				ca[0] = 'q';
				s4 = addSuffix(s, ca);
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

			case CtorKind.OS_2:
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_os(v, s);
				break;

			case CtorKind.OS_3:
				v = reader.readCompressedUInt32();
				v2 = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_os(v, s, v2);
				break;

			case CtorKind.OS_CALL:
				v = reader.readCompressedUInt32();
				v2 = reader.readByte();
				if (v2 > 1)
					throw new UnsupportedOperationException();
				instrInfo = new SimpleInstrInfo_os_call(v, s, v2 != 0);
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

			case CtorKind.OS_MEM:
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_os_mem(v, s);
				break;

			case CtorKind.OS_MEM_REG16:
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_os_mem_reg16(v, s);
				break;

			case CtorKind.OS_MEM2:
				v = reader.readCompressedUInt32();
				v2 = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_os_mem2(v, s, v2);
				break;

			case CtorKind.PBLENDVB:
				v = reader.readByte();
				instrInfo = new SimpleInstrInfo_pblendvb(s, v);
				break;

			case CtorKind.PCLMULQDQ:
				v = reader.readByte();
				instrInfo = new SimpleInstrInfo_pclmulqdq(s, com.github.icedland.iced.x86.internal.fmt.FormatterConstants.getPseudoOps(v));
				break;

			case CtorKind.POPS:
				v = reader.readByte();
				instrInfo = new SimpleInstrInfo_pops(s, com.github.icedland.iced.x86.internal.fmt.FormatterConstants.getPseudoOps(v));
				break;

			case CtorKind.REG16:
				instrInfo = new SimpleInstrInfo_Reg16(s);
				break;

			case CtorKind.REG32:
				instrInfo = new SimpleInstrInfo_Reg32(s);
				break;

			case CtorKind.REVERSE:
				instrInfo = new SimpleInstrInfo_reverse(s);
				break;

			case CtorKind.SAE:
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_sae(v, s);
				break;

			case CtorKind.PUSH_IMM8:
				v = reader.readCompressedUInt32();
				v2 = reader.readByte();
				instrInfo = new SimpleInstrInfo_push_imm8(v, v2, s);
				break;

			case CtorKind.PUSH_IMM:
				v = reader.readCompressedUInt32();
				v2 = reader.readByte();
				instrInfo = new SimpleInstrInfo_push_imm(v, v2, s);
				break;

			case CtorKind.SIGN_EXT_3:
				v = reader.readByte();
				v2 = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_SignExt(v, s, v2);
				break;

			case CtorKind.SIGN_EXT_4:
				v = reader.readByte();
				v2 = reader.readByte();
				v3 = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_SignExt(v, v2, s, v3);
				break;

			case CtorKind.IMUL:
				v = reader.readByte();
				instrInfo = new SimpleInstrInfo_imul(v, s);
				break;

			case CtorKind.STIG1:
				v = reader.readByte();
				if (v > 1)
					throw new UnsupportedOperationException();
				instrInfo = new SimpleInstrInfo_STIG1(s, v != 0);
				break;

			case CtorKind.STIG2_2A:
				v = reader.readByte();
				if (v > 1)
					throw new UnsupportedOperationException();
				instrInfo = new SimpleInstrInfo_STIG2(s, v != 0);
				break;

			case CtorKind.STIG2_2B:
				v = reader.readCompressedUInt32();
				instrInfo = new SimpleInstrInfo_STIG2(s, v);
				break;

			case CtorKind.XLAT:
				instrInfo = new SimpleInstrInfo_XLAT(s);
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
