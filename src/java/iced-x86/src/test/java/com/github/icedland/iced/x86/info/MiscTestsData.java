// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.info;

import java.util.ArrayList;
import java.util.HashSet;

import com.github.icedland.iced.x86.CodeUtils;
import com.github.icedland.iced.x86.PathUtils;
import com.github.icedland.iced.x86.SectionFileReader;
import com.github.icedland.iced.x86.SectionInfo;
import com.github.icedland.iced.x86.ToCode;
import com.github.icedland.iced.x86.ToConditionCode;

final class MiscTestsData {
	public static final HashSet<Integer> jccShort;
	public static final HashSet<Integer> jmpNear;
	public static final HashSet<Integer> jmpFar;
	public static final HashSet<Integer> jmpShort;
	public static final HashSet<Integer> jmpNearIndirect;
	public static final HashSet<Integer> jmpFarIndirect;
	public static final HashSet<Integer> jccNear;
	public static final HashSet<Integer> callFar;
	public static final HashSet<Integer> callNear;
	public static final HashSet<Integer> callNearIndirect;
	public static final HashSet<Integer> callFarIndirect;
	public static final HashSet<Integer> jmpeNear;
	public static final HashSet<Integer> jmpeNearIndirect;
	public static final HashSet<Integer> loop;
	public static final HashSet<Integer> jrcxz;
	public static final HashSet<Integer> xbegin;
	public static final HashSet<Integer> stringInstr;
	public static final HashSet<Integer> jkccShort;
	public static final HashSet<Integer> jkccNear;
	public static final JmpInfo[] jmpInfos;
	public static final JccShortInfo[] jccShortInfos;
	public static final JccNearInfo[] jccNearInfos;
	public static final JkccShortInfo[] jkccShortInfos;
	public static final JkccNearInfo[] jkccNearInfos;
	public static final SetccInfo[] setccInfos;
	public static final CmovccInfo[] cmovccInfos;
	public static final CmpccxaddInfo[] cmpccxaddInfos;
	public static final LoopccInfo[] loopccInfos;

	static class JmpInfo {
		int jmpShort;
		int jmpNear;

		JmpInfo(int jmpShort, int jmpNear) {
			this.jmpShort = jmpShort;
			this.jmpNear = jmpNear;
		}
	}
	static class JccShortInfo {
		int jcc;
		int negated;
		int jccNear;
		int cc;

		JccShortInfo(int jcc, int negated, int jccNear, int cc) {
			this.jcc = jcc;
			this.negated = negated;
			this.jccNear = jccNear;
			this.cc = cc;
		}
	}
	static class JccNearInfo {
		int jcc;
		int negated;
		int jccShort;
		int cc;

		JccNearInfo(int jcc, int negated, int jccShort, int cc) {
			this.jcc = jcc;
			this.negated = negated;
			this.jccShort = jccShort;
			this.cc = cc;
		}
	}
	static class JkccShortInfo {
		int jkcc;
		int negated;
		int jkccNear;
		int cc;

		JkccShortInfo(int jkcc, int negated, int jkccNear, int cc) {
			this.jkcc = jkcc;
			this.negated = negated;
			this.jkccNear = jkccNear;
			this.cc = cc;
		}
	}
	static class JkccNearInfo {
		int jkcc;
		int negated;
		int jkccShort;
		int cc;

		JkccNearInfo(int jkcc, int negated, int jkccShort, int cc) {
			this.jkcc = jkcc;
			this.negated = negated;
			this.jkccShort = jkccShort;
			this.cc = cc;
		}
	}
	static class SetccInfo {
		int setcc;
		int negated;
		int cc;

		SetccInfo(int setcc, int negated, int cc) {
			this.setcc = setcc;
			this.negated = negated;
			this.cc = cc;
		}
	}
	static class CmovccInfo {
		int cmovcc;
		int negated;
		int cc;

		CmovccInfo(int cmovcc, int negated, int cc) {
			this.cmovcc = cmovcc;
			this.negated = negated;
			this.cc = cc;
		}
	}
	static class CmpccxaddInfo {
		int cmpccxadd;
		int negated;
		int cc;

		CmpccxaddInfo(int cmpccxadd, int negated, int cc) {
			this.cmpccxadd = cmpccxadd;
			this.negated = negated;
			this.cc = cc;
		}
	}
	static class LoopccInfo {
		int loopcc;
		int negated;
		int cc;

		LoopccInfo(int loopcc, int negated, int cc) {
			this.loopcc = loopcc;
			this.negated = negated;
			this.cc = cc;
		}
	}

	static {
		HashSet<Integer> tmp_jccShort = new HashSet<Integer>();
		HashSet<Integer> tmp_jmpNear = new HashSet<Integer>();
		HashSet<Integer> tmp_jmpFar = new HashSet<Integer>();
		HashSet<Integer> tmp_jmpShort = new HashSet<Integer>();
		HashSet<Integer> tmp_jmpNearIndirect = new HashSet<Integer>();
		HashSet<Integer> tmp_jmpFarIndirect = new HashSet<Integer>();
		HashSet<Integer> tmp_jccNear = new HashSet<Integer>();
		HashSet<Integer> tmp_callFar = new HashSet<Integer>();
		HashSet<Integer> tmp_callNear = new HashSet<Integer>();
		HashSet<Integer> tmp_callNearIndirect = new HashSet<Integer>();
		HashSet<Integer> tmp_callFarIndirect = new HashSet<Integer>();
		HashSet<Integer> tmp_jmpeNear = new HashSet<Integer>();
		HashSet<Integer> tmp_jmpeNearIndirect = new HashSet<Integer>();
		HashSet<Integer> tmp_loop = new HashSet<Integer>();
		HashSet<Integer> tmp_jrcxz = new HashSet<Integer>();
		HashSet<Integer> tmp_xbegin = new HashSet<Integer>();
		HashSet<Integer> tmp_stringInstr = new HashSet<Integer>();
		HashSet<Integer> tmp_jkccShort = new HashSet<Integer>();
		HashSet<Integer> tmp_jkccNear = new HashSet<Integer>();
		ArrayList<JmpInfo> tmp_jmpInfos = new ArrayList<JmpInfo>();
		ArrayList<JccShortInfo> tmp_jccShortInfos = new ArrayList<JccShortInfo>();
		ArrayList<JccNearInfo> tmp_jccNearInfos = new ArrayList<JccNearInfo>();
		ArrayList<JkccShortInfo> tmp_jkccShortInfos = new ArrayList<JkccShortInfo>();
		ArrayList<JkccNearInfo> tmp_jkccNearInfos = new ArrayList<JkccNearInfo>();
		ArrayList<SetccInfo> tmp_setccInfos = new ArrayList<SetccInfo>();
		ArrayList<CmovccInfo> tmp_cmovccInfos = new ArrayList<CmovccInfo>();
		ArrayList<CmpccxaddInfo> tmp_cmpccxaddInfos = new ArrayList<CmpccxaddInfo>();
		ArrayList<LoopccInfo> tmp_loopccInfos = new ArrayList<LoopccInfo>();

		SectionInfo[] sectionInfos = new SectionInfo[] {
			new SectionInfo(MiscSectionNames.JCC_SHORT, (_ig, line) -> addCode(tmp_jccShort, line)),
			new SectionInfo(MiscSectionNames.JMP_NEAR, (_ig, line) -> addCode(tmp_jmpNear, line)),
			new SectionInfo(MiscSectionNames.JMP_FAR, (_ig, line) -> addCode(tmp_jmpFar, line)),
			new SectionInfo(MiscSectionNames.JMP_SHORT, (_ig, line) -> addCode(tmp_jmpShort, line)),
			new SectionInfo(MiscSectionNames.JMP_NEAR_INDIRECT, (_ig, line) -> addCode(tmp_jmpNearIndirect, line)),
			new SectionInfo(MiscSectionNames.JMP_FAR_INDIRECT, (_ig, line) -> addCode(tmp_jmpFarIndirect, line)),
			new SectionInfo(MiscSectionNames.JCC_NEAR, (_ig, line) -> addCode(tmp_jccNear, line)),
			new SectionInfo(MiscSectionNames.CALL_FAR, (_ig, line) -> addCode(tmp_callFar, line)),
			new SectionInfo(MiscSectionNames.CALL_NEAR, (_ig, line) -> addCode(tmp_callNear, line)),
			new SectionInfo(MiscSectionNames.CALL_NEAR_INDIRECT, (_ig, line) -> addCode(tmp_callNearIndirect, line)),
			new SectionInfo(MiscSectionNames.CALL_FAR_INDIRECT, (_ig, line) -> addCode(tmp_callFarIndirect, line)),
			new SectionInfo(MiscSectionNames.JMPE_NEAR, (_ig, line) -> addCode(tmp_jmpeNear, line)),
			new SectionInfo(MiscSectionNames.JMPE_NEAR_INDIRECT, (_ig, line) -> addCode(tmp_jmpeNearIndirect, line)),
			new SectionInfo(MiscSectionNames.LOOP, (_ig, line) -> addCode(tmp_loop, line)),
			new SectionInfo(MiscSectionNames.JRCXZ, (_ig, line) -> addCode(tmp_jrcxz, line)),
			new SectionInfo(MiscSectionNames.XBEGIN, (_ig, line) -> addCode(tmp_xbegin, line)),
			new SectionInfo(MiscSectionNames.STRING_INSTRUCTION, (_ig, line) -> addCode(tmp_stringInstr, line)),
			new SectionInfo(MiscSectionNames.JKCC_SHORT, (_ig, line) -> addCode(tmp_jkccShort, line)),
			new SectionInfo(MiscSectionNames.JKCC_NEAR, (_ig, line) -> addCode(tmp_jkccNear, line)),
			new SectionInfo(MiscSectionNames.JMP_INFO, (_ig, line) -> addJmpInfo(tmp_jmpInfos, line)),
			new SectionInfo(MiscSectionNames.JCC_SHORT_INFO, (_ig, line) -> addJccShortInfo(tmp_jccShortInfos, line)),
			new SectionInfo(MiscSectionNames.JCC_NEAR_INFO, (_ig, line) -> addJccNearInfo(tmp_jccNearInfos, line)),
			new SectionInfo(MiscSectionNames.JKCC_SHORT_INFO, (_ig, line) -> addJkccShortInfo(tmp_jkccShortInfos, line)),
			new SectionInfo(MiscSectionNames.JKCC_NEAR_INFO, (_ig, line) -> addJkccNearInfo(tmp_jkccNearInfos, line)),
			new SectionInfo(MiscSectionNames.SETCC_INFO, (_ig, line) -> addSetccInfo(tmp_setccInfos, line)),
			new SectionInfo(MiscSectionNames.CMOVCC_INFO, (_ig, line) -> addCmovccInfo(tmp_cmovccInfos, line)),
			new SectionInfo(MiscSectionNames.CMPCCXADD_INFO, (_ig, line) -> addCmpccxaddInfo(tmp_cmpccxaddInfos, line)),
			new SectionInfo(MiscSectionNames.LOOPCC_INFO, (_ig, line) -> addLoopccInfo(tmp_loopccInfos, line)),
		};
		String filename = PathUtils.getTestTextFilename("InstructionInfo", "Misc.txt");
		SectionFileReader.read(filename, sectionInfos);

		jccShort = tmp_jccShort;
		jmpNear = tmp_jmpNear;
		jmpFar = tmp_jmpFar;
		jmpShort = tmp_jmpShort;
		jmpNearIndirect = tmp_jmpNearIndirect;
		jmpFarIndirect = tmp_jmpFarIndirect;
		jccNear = tmp_jccNear;
		callFar = tmp_callFar;
		callNear = tmp_callNear;
		callNearIndirect = tmp_callNearIndirect;
		callFarIndirect = tmp_callFarIndirect;
		jmpeNear = tmp_jmpeNear;
		jmpeNearIndirect = tmp_jmpeNearIndirect;
		loop = tmp_loop;
		jrcxz = tmp_jrcxz;
		xbegin = tmp_xbegin;
		stringInstr = tmp_stringInstr;
		jkccShort = tmp_jkccShort;
		jkccNear = tmp_jkccNear;
		jmpInfos = tmp_jmpInfos.toArray(new JmpInfo[0]);
		jccShortInfos = tmp_jccShortInfos.toArray(new JccShortInfo[0]);
		jccNearInfos = tmp_jccNearInfos.toArray(new JccNearInfo[0]);
		jkccShortInfos = tmp_jkccShortInfos.toArray(new JkccShortInfo[0]);
		jkccNearInfos = tmp_jkccNearInfos.toArray(new JkccNearInfo[0]);
		setccInfos = tmp_setccInfos.toArray(new SetccInfo[0]);
		cmovccInfos = tmp_cmovccInfos.toArray(new CmovccInfo[0]);
		cmpccxaddInfos = tmp_cmpccxaddInfos.toArray(new CmpccxaddInfo[0]);
		loopccInfos = tmp_loopccInfos.toArray(new LoopccInfo[0]);
	}

	private static void addCode(HashSet<Integer> hash, String line) {
		String code = line.trim();
		if (CodeUtils.isIgnored(code))
			return;
		if (!hash.add(ToCode.get(code)))
			throw new UnsupportedOperationException("Duplicate Code value");
	}

	private static void addJmpInfo(ArrayList<JmpInfo> infos, String line) {
		final int ELEMS = 2;
		String[] elems = line.split(",");
		if (elems.length != ELEMS)
			throw new UnsupportedOperationException(String.format("Expected %d elements, found %d", ELEMS, elems.length));
		String code1 = elems[0].trim();
		String code2 = elems[1].trim();
		if (CodeUtils.isIgnored(code1) || CodeUtils.isIgnored(code2))
			return;
		infos.add(new JmpInfo(ToCode.get(code1), ToCode.get(code2)));
	}

	private static void addJccShortInfo(ArrayList<JccShortInfo> infos, String line) {
		final int ELEMS = 4;
		String[] elems = line.split(",");
		if (elems.length != ELEMS)
			throw new UnsupportedOperationException(String.format("Expected %d elements, found %d", ELEMS, elems.length));
		String code1 = elems[0].trim();
		String code2 = elems[1].trim();
		String code3 = elems[2].trim();
		if (CodeUtils.isIgnored(code1) || CodeUtils.isIgnored(code2) || CodeUtils.isIgnored(code3))
			return;
		infos.add(new JccShortInfo(ToCode.get(code1), ToCode.get(code2), ToCode.get(code3), ToConditionCode.get(elems[3].trim())));
	}

	private static void addJccNearInfo(ArrayList<JccNearInfo> infos, String line) {
		final int ELEMS = 4;
		String[] elems = line.split(",");
		if (elems.length != ELEMS)
			throw new UnsupportedOperationException(String.format("Expected %d elements, found %d", ELEMS, elems.length));
		String code1 = elems[0].trim();
		String code2 = elems[1].trim();
		String code3 = elems[2].trim();
		if (CodeUtils.isIgnored(code1) || CodeUtils.isIgnored(code2) || CodeUtils.isIgnored(code3))
			return;
		infos.add(new JccNearInfo(ToCode.get(code1), ToCode.get(code2), ToCode.get(code3), ToConditionCode.get(elems[3].trim())));
	}

	private static void addJkccShortInfo(ArrayList<JkccShortInfo> infos, String line) {
		final int ELEMS = 4;
		String[] elems = line.split(",");
		if (elems.length != ELEMS)
			throw new UnsupportedOperationException(String.format("Expected %d elements, found %d", ELEMS, elems.length));
		String code1 = elems[0].trim();
		String code2 = elems[1].trim();
		String code3 = elems[2].trim();
		if (CodeUtils.isIgnored(code1) || CodeUtils.isIgnored(code2) || CodeUtils.isIgnored(code3))
			return;
		infos.add(new JkccShortInfo(ToCode.get(code1), ToCode.get(code2), ToCode.get(code3), ToConditionCode.get(elems[3].trim())));
	}

	private static void addJkccNearInfo(ArrayList<JkccNearInfo> infos, String line) {
		final int ELEMS = 4;
		String[] elems = line.split(",");
		if (elems.length != ELEMS)
			throw new UnsupportedOperationException(String.format("Expected %d elements, found %d", ELEMS, elems.length));
		String code1 = elems[0].trim();
		String code2 = elems[1].trim();
		String code3 = elems[2].trim();
		if (CodeUtils.isIgnored(code1) || CodeUtils.isIgnored(code2) || CodeUtils.isIgnored(code3))
			return;
		infos.add(new JkccNearInfo(ToCode.get(code1), ToCode.get(code2), ToCode.get(code3), ToConditionCode.get(elems[3].trim())));
	}

	private static void addSetccInfo(ArrayList<SetccInfo> infos, String line) {
		final int ELEMS = 3;
		String[] elems = line.split(",");
		if (elems.length != ELEMS)
			throw new UnsupportedOperationException(String.format("Expected %d elements, found %d", ELEMS, elems.length));
		String code1 = elems[0].trim();
		String code2 = elems[1].trim();
		if (CodeUtils.isIgnored(code1) || CodeUtils.isIgnored(code2))
			return;
		infos.add(new SetccInfo(ToCode.get(code1), ToCode.get(code2), ToConditionCode.get(elems[2].trim())));
	}

	private static void addCmovccInfo(ArrayList<CmovccInfo> infos, String line) {
		final int ELEMS = 3;
		String[] elems = line.split(",");
		if (elems.length != ELEMS)
			throw new UnsupportedOperationException(String.format("Expected %d elements, found %d", ELEMS, elems.length));
		String code1 = elems[0].trim();
		String code2 = elems[1].trim();
		if (CodeUtils.isIgnored(code1) || CodeUtils.isIgnored(code2))
			return;
		infos.add(new CmovccInfo(ToCode.get(code1), ToCode.get(code2), ToConditionCode.get(elems[2].trim())));
	}

	private static void addCmpccxaddInfo(ArrayList<CmpccxaddInfo> infos, String line) {
		final int ELEMS = 3;
		String[] elems = line.split(",");
		if (elems.length != ELEMS)
			throw new UnsupportedOperationException(String.format("Expected %d elements, found %d", ELEMS, elems.length));
		String code1 = elems[0].trim();
		String code2 = elems[1].trim();
		if (CodeUtils.isIgnored(code1) || CodeUtils.isIgnored(code2))
			return;
		infos.add(new CmpccxaddInfo(ToCode.get(code1), ToCode.get(code2), ToConditionCode.get(elems[2].trim())));
	}

	private static void addLoopccInfo(ArrayList<LoopccInfo> infos, String line) {
		final int ELEMS = 3;
		String[] elems = line.split(",");
		if (elems.length != ELEMS)
			throw new UnsupportedOperationException(String.format("Expected %d elements, found %d", ELEMS, elems.length));
		String code1 = elems[0].trim();
		String code2 = elems[1].trim();
		if (CodeUtils.isIgnored(code1) || CodeUtils.isIgnored(code2))
			return;
		infos.add(new LoopccInfo(ToCode.get(code1), ToCode.get(code2), ToConditionCode.get(elems[2].trim())));
	}
}
