// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.info;

import java.util.Locale;

import com.github.icedland.iced.x86.Code;
import com.github.icedland.iced.x86.EncodingKind;
import com.github.icedland.iced.x86.MemorySize;
import com.github.icedland.iced.x86.MvexConvFn;
import com.github.icedland.iced.x86.MvexEHBit;
import com.github.icedland.iced.x86.internal.InstructionMemorySizes;
import com.github.icedland.iced.x86.internal.MvexInfo;
import com.github.icedland.iced.x86.internal.enc.InstrStrFmtOption;

final class InstructionFormatter {
	private final OpCodeInfo opCode;
	private final StringBuilder sb;
	private final String[] mnemonics;
	private int r32_count;
	private int r64_count;
	private int bnd_count;
	private int startOpIndex;
	private int r32_index, r64_index, bnd_index;
	private int k_index;
	private int vec_index;
	private int tmm_index;
	private int opCount;
	// true: k2 {k1}, false: k1 {k2}
	private boolean opMaskIsK1;
	private boolean noVecIndex;
	private boolean swapVecIndex12;
	private boolean noGprSuffix;
	private boolean vecIndexSameAsOpIndex;

	private static final String[] convFnNames = new String[] {
		"Sf32",
		"Sf64",
		"Si32",
		"Si64",
		"Uf32",
		"Uf64",
		"Ui32",
		"Ui64",
		"Df32",
		"Df64",
		"Di32",
		"Di64",
	};

	private int getKIndex() {
		k_index++;
		if (opMaskIsK1) {
			if (k_index == 1)
				return 2;
			if (k_index == 2)
				return 1;
		}
		return k_index;
	}

	private int getBndIndex() {
		if (bnd_count <= 1)
			return 0;
		bnd_index++;
		return bnd_index;
	}

	private int getVecIndex(int opIndex) {
		if (noVecIndex)
			return 0;
		if (vecIndexSameAsOpIndex)
			return opIndex + 1;
		vec_index++;
		if (swapVecIndex12) {
			if (vec_index == 1)
				return 2;
			if (vec_index == 2)
				return 1;
		}
		return vec_index;
	}

	private int getTmmIndex() {
		tmm_index++;
		return tmm_index;
	}

	InstructionFormatter(OpCodeInfo opCode, int fmtOption, StringBuilder sb, String[] mnemonics) {
		this.opCode = opCode;
		this.sb = sb;
		this.mnemonics = mnemonics;
		noVecIndex = false;
		swapVecIndex12 = false;
		noGprSuffix = false;
		vecIndexSameAsOpIndex = false;
		startOpIndex = 0;
		bnd_count = 0;
		r32_count = 0;
		r64_count = 0;
		r32_index = 0;
		r64_index = 0;
		k_index = 0;
		vec_index = 0;
		tmm_index = 0;
		bnd_index = 0;
		opCount = opCode.getOpCount();
		opMaskIsK1 = false;
		switch (fmtOption) {
		case InstrStrFmtOption.NONE:
			break;
		case InstrStrFmtOption.OP_MASK_IS_K1_OR_NO_GPR_SUFFIX:
			opMaskIsK1 = true;
			noGprSuffix = true;
			break;
		case InstrStrFmtOption.INC_VEC_INDEX:
			vec_index++;
			break;
		case InstrStrFmtOption.NO_VEC_INDEX:
			noVecIndex = true;
			break;
		case InstrStrFmtOption.SWAP_VEC_INDEX12:
			swapVecIndex12 = true;
			break;
		case InstrStrFmtOption.SKIP_OP0:
			startOpIndex = 1;
			break;
		case InstrStrFmtOption.VEC_INDEX_SAME_AS_OP_INDEX:
			vecIndexSameAsOpIndex = true;
			break;
		default:
			throw new UnsupportedOperationException();
		}
		if ((opCode.getOp0Kind() == OpCodeOperandKind.K_REG || opCode.getOp0Kind() == OpCodeOperandKind.KP1_REG) && opCode.getOpCount() > 2 &&
				opCode.getEncoding() != EncodingKind.MVEX) {
			vecIndexSameAsOpIndex = true;
		}
		for (int i = 0; i < opCode.getOpCount(); i++) {
			switch (opCode.getOpKind(i)) {
			case OpCodeOperandKind.R32_REG:
			case OpCodeOperandKind.R32_REG_MEM:
			case OpCodeOperandKind.R32_RM:
			case OpCodeOperandKind.R32_OPCODE:
			case OpCodeOperandKind.R32_VVVV:
				r32_count++;
				break;

			case OpCodeOperandKind.R64_REG:
			case OpCodeOperandKind.R64_REG_MEM:
			case OpCodeOperandKind.R64_RM:
			case OpCodeOperandKind.R64_OPCODE:
			case OpCodeOperandKind.R64_VVVV:
				r64_count++;
				break;

			case OpCodeOperandKind.BND_OR_MEM_MPX:
			case OpCodeOperandKind.BND_REG:
				bnd_count++;
				break;

			case OpCodeOperandKind.NONE:
			case OpCodeOperandKind.FARBR2_2:
			case OpCodeOperandKind.FARBR4_2:
			case OpCodeOperandKind.MEM_OFFS:
			case OpCodeOperandKind.MEM:
			case OpCodeOperandKind.MEM_MPX:
			case OpCodeOperandKind.MEM_MIB:
			case OpCodeOperandKind.MEM_VSIB32X:
			case OpCodeOperandKind.MEM_VSIB64X:
			case OpCodeOperandKind.MEM_VSIB32Y:
			case OpCodeOperandKind.MEM_VSIB64Y:
			case OpCodeOperandKind.MEM_VSIB32Z:
			case OpCodeOperandKind.MEM_VSIB64Z:
			case OpCodeOperandKind.R8_OR_MEM:
			case OpCodeOperandKind.R16_OR_MEM:
			case OpCodeOperandKind.R32_OR_MEM:
			case OpCodeOperandKind.R32_OR_MEM_MPX:
			case OpCodeOperandKind.R64_OR_MEM:
			case OpCodeOperandKind.R64_OR_MEM_MPX:
			case OpCodeOperandKind.MM_OR_MEM:
			case OpCodeOperandKind.XMM_OR_MEM:
			case OpCodeOperandKind.YMM_OR_MEM:
			case OpCodeOperandKind.ZMM_OR_MEM:
			case OpCodeOperandKind.K_OR_MEM:
			case OpCodeOperandKind.R8_REG:
			case OpCodeOperandKind.R8_OPCODE:
			case OpCodeOperandKind.R16_REG:
			case OpCodeOperandKind.R16_REG_MEM:
			case OpCodeOperandKind.R16_RM:
			case OpCodeOperandKind.R16_OPCODE:
			case OpCodeOperandKind.SEG_REG:
			case OpCodeOperandKind.K_REG:
			case OpCodeOperandKind.KP1_REG:
			case OpCodeOperandKind.K_RM:
			case OpCodeOperandKind.K_VVVV:
			case OpCodeOperandKind.MM_REG:
			case OpCodeOperandKind.MM_RM:
			case OpCodeOperandKind.XMM_REG:
			case OpCodeOperandKind.XMM_RM:
			case OpCodeOperandKind.XMM_VVVV:
			case OpCodeOperandKind.XMMP3_VVVV:
			case OpCodeOperandKind.XMM_IS4:
			case OpCodeOperandKind.XMM_IS5:
			case OpCodeOperandKind.YMM_REG:
			case OpCodeOperandKind.YMM_RM:
			case OpCodeOperandKind.YMM_VVVV:
			case OpCodeOperandKind.YMM_IS4:
			case OpCodeOperandKind.YMM_IS5:
			case OpCodeOperandKind.ZMM_REG:
			case OpCodeOperandKind.ZMM_RM:
			case OpCodeOperandKind.ZMM_VVVV:
			case OpCodeOperandKind.ZMMP3_VVVV:
			case OpCodeOperandKind.CR_REG:
			case OpCodeOperandKind.DR_REG:
			case OpCodeOperandKind.TR_REG:
			case OpCodeOperandKind.ES:
			case OpCodeOperandKind.CS:
			case OpCodeOperandKind.SS:
			case OpCodeOperandKind.DS:
			case OpCodeOperandKind.FS:
			case OpCodeOperandKind.GS:
			case OpCodeOperandKind.AL:
			case OpCodeOperandKind.CL:
			case OpCodeOperandKind.AX:
			case OpCodeOperandKind.DX:
			case OpCodeOperandKind.EAX:
			case OpCodeOperandKind.RAX:
			case OpCodeOperandKind.ST0:
			case OpCodeOperandKind.STI_OPCODE:
			case OpCodeOperandKind.IMM4_M2Z:
			case OpCodeOperandKind.IMM8:
			case OpCodeOperandKind.IMM8_CONST_1:
			case OpCodeOperandKind.IMM8SEX16:
			case OpCodeOperandKind.IMM8SEX32:
			case OpCodeOperandKind.IMM8SEX64:
			case OpCodeOperandKind.IMM16:
			case OpCodeOperandKind.IMM32:
			case OpCodeOperandKind.IMM32SEX64:
			case OpCodeOperandKind.IMM64:
			case OpCodeOperandKind.SEG_RDI:
			case OpCodeOperandKind.BR16_1:
			case OpCodeOperandKind.BR32_1:
			case OpCodeOperandKind.BR64_1:
			case OpCodeOperandKind.BR16_2:
			case OpCodeOperandKind.BR32_4:
			case OpCodeOperandKind.BR64_4:
			case OpCodeOperandKind.XBEGIN_2:
			case OpCodeOperandKind.XBEGIN_4:
			case OpCodeOperandKind.BRDISP_2:
			case OpCodeOperandKind.BRDISP_4:
			case OpCodeOperandKind.SIBMEM:
			case OpCodeOperandKind.TMM_REG:
			case OpCodeOperandKind.TMM_RM:
			case OpCodeOperandKind.TMM_VVVV:
				break;

			case OpCodeOperandKind.SEG_RSI:
			case OpCodeOperandKind.ES_RDI:
			case OpCodeOperandKind.SEG_RBX_AL:
				// String instructions, xlat
				opCount = 0;
				break;

			default:
				throw new UnsupportedOperationException();
			}
		}
	}

	private int getMemorySize(boolean isBroadcast) {
		int index = opCode.getCode();
		if (isBroadcast)
			return InstructionMemorySizes.sizesBcst[index] & 0xFF;
		else
			return InstructionMemorySizes.sizesNormal[index] & 0xFF;
	}

	public String format() {
		if (!opCode.isInstruction()) {
			switch (opCode.getCode()) {
			// GENERATOR-BEGIN: InstrFmtNotInstructionString
			// ⚠️This was generated by GENERATOR!🦹‍♂️
			case Code.INVALID:
				return "<invalid>";
			case Code.DECLAREBYTE:
				return "<db>";
			case Code.DECLAREWORD:
				return "<dw>";
			case Code.DECLAREDWORD:
				return "<dd>";
			case Code.DECLAREQWORD:
				return "<dq>";
			case Code.ZERO_BYTES:
				return "ZERO_BYTES";
			// GENERATOR-END: InstrFmtNotInstructionString
			default:
				throw new UnsupportedOperationException();
			}
		}

		sb.setLength(0);

		write(mnemonics[opCode.getMnemonic()], true);
		if (startOpIndex < opCount) {
			sb.append(' ');
			int saeErIndex = opCount - 1;
			if (opCode.getEncoding() != EncodingKind.LEGACY && opCode.getOpKind(saeErIndex) == OpCodeOperandKind.IMM8)
				saeErIndex--;
			boolean addComma = false;
			for (int i = startOpIndex; i < opCount; i++) {
				if (addComma)
					writeOpSeparator();
				addComma = true;

				if (i == saeErIndex && opCode.getEncoding() == EncodingKind.MVEX) {
					int convFn = MvexInfo.getConvFn(opCode.getCode());
					if (convFn != MvexConvFn.NONE) {
						sb.append(convFnNames[convFn - 1]);
						sb.append('(');
					}
				}
				int opKind = opCode.getOpKind(i);
				switch (opKind) {
				case OpCodeOperandKind.FARBR2_2:
					sb.append("ptr16:16");
					break;

				case OpCodeOperandKind.FARBR4_2:
					sb.append("ptr16:32");
					break;

				case OpCodeOperandKind.MEM_OFFS:
					sb.append("moffs");
					writeMemorySize(getMemorySize(false));
					break;

				case OpCodeOperandKind.MEM:
				case OpCodeOperandKind.MEM_MPX:
					writeMemory();
					break;

				case OpCodeOperandKind.SIBMEM:
					sb.append("sibmem");
					break;

				case OpCodeOperandKind.MEM_MIB:
					sb.append("mib");
					break;

				case OpCodeOperandKind.MEM_VSIB32X:
					sb.append("vm32x");
					break;

				case OpCodeOperandKind.MEM_VSIB64X:
					sb.append("vm64x");
					break;

				case OpCodeOperandKind.MEM_VSIB32Y:
					sb.append("vm32y");
					break;

				case OpCodeOperandKind.MEM_VSIB64Y:
					sb.append("vm64y");
					break;

				case OpCodeOperandKind.MEM_VSIB32Z:
					if (opCode.getEncoding() == EncodingKind.MVEX)
						sb.append("mvt");
					else
						sb.append("vm32z");
					break;

				case OpCodeOperandKind.MEM_VSIB64Z:
					sb.append("vm64z");
					break;

				case OpCodeOperandKind.R8_OR_MEM:
					writeGprMem(8);
					break;

				case OpCodeOperandKind.R16_OR_MEM:
					writeGprMem(16);
					break;

				case OpCodeOperandKind.R32_OR_MEM:
				case OpCodeOperandKind.R32_OR_MEM_MPX:
					writeGprMem(32);
					break;

				case OpCodeOperandKind.R64_OR_MEM:
				case OpCodeOperandKind.R64_OR_MEM_MPX:
					writeGprMem(64);
					break;

				case OpCodeOperandKind.MM_OR_MEM:
					writeRegMem("mm", getVecIndex(i));
					break;

				case OpCodeOperandKind.XMM_OR_MEM:
					writeRegMem("xmm", getVecIndex(i));
					break;

				case OpCodeOperandKind.YMM_OR_MEM:
					writeRegMem("ymm", getVecIndex(i));
					break;

				case OpCodeOperandKind.ZMM_OR_MEM:
					writeRegMem("zmm", getVecIndex(i));
					break;

				case OpCodeOperandKind.TMM_REG:
				case OpCodeOperandKind.TMM_RM:
				case OpCodeOperandKind.TMM_VVVV:
					writeRegOp("tmm", getTmmIndex());
					break;

				case OpCodeOperandKind.BND_OR_MEM_MPX:
					writeRegOp("bnd", getBndIndex());
					sb.append('/');
					writeMemory();
					break;

				case OpCodeOperandKind.K_OR_MEM:
					writeRegMem("k", getKIndex());
					break;

				case OpCodeOperandKind.R8_REG:
				case OpCodeOperandKind.R8_OPCODE:
					writeRegOp("r8");
					break;

				case OpCodeOperandKind.R16_REG:
				case OpCodeOperandKind.R16_REG_MEM:
				case OpCodeOperandKind.R16_RM:
				case OpCodeOperandKind.R16_OPCODE:
					writeRegOp("r16");
					break;

				case OpCodeOperandKind.R32_REG:
				case OpCodeOperandKind.R32_REG_MEM:
				case OpCodeOperandKind.R32_RM:
				case OpCodeOperandKind.R32_OPCODE:
				case OpCodeOperandKind.R32_VVVV:
					writeRegOp("r32");
					r32_index = appendGprSuffix(r32_count, r32_index);
					break;

				case OpCodeOperandKind.R64_REG:
				case OpCodeOperandKind.R64_REG_MEM:
				case OpCodeOperandKind.R64_RM:
				case OpCodeOperandKind.R64_OPCODE:
				case OpCodeOperandKind.R64_VVVV:
					writeRegOp("r64");
					r64_index = appendGprSuffix(r64_count, r64_index);
					break;

				case OpCodeOperandKind.SEG_REG:
					sb.append("Sreg");
					break;

				case OpCodeOperandKind.K_REG:
				case OpCodeOperandKind.K_RM:
				case OpCodeOperandKind.K_VVVV:
					writeRegOp("k", getKIndex());
					break;

				case OpCodeOperandKind.KP1_REG:
					writeRegOp("k", getKIndex());
					sb.append("+1");
					break;

				case OpCodeOperandKind.MM_REG:
				case OpCodeOperandKind.MM_RM:
					writeRegOp("mm", getVecIndex(i));
					break;

				case OpCodeOperandKind.XMM_REG:
				case OpCodeOperandKind.XMM_RM:
				case OpCodeOperandKind.XMM_VVVV:
				case OpCodeOperandKind.XMM_IS4:
				case OpCodeOperandKind.XMM_IS5:
					writeRegOp("xmm", getVecIndex(i));
					break;

				case OpCodeOperandKind.XMMP3_VVVV:
					writeRegOp("xmm", getVecIndex(i));
					sb.append("+3");
					break;

				case OpCodeOperandKind.YMM_REG:
				case OpCodeOperandKind.YMM_RM:
				case OpCodeOperandKind.YMM_VVVV:
				case OpCodeOperandKind.YMM_IS4:
				case OpCodeOperandKind.YMM_IS5:
					writeRegOp("ymm", getVecIndex(i));
					break;

				case OpCodeOperandKind.ZMM_REG:
				case OpCodeOperandKind.ZMM_RM:
				case OpCodeOperandKind.ZMM_VVVV:
					writeRegOp("zmm", getVecIndex(i));
					break;

				case OpCodeOperandKind.ZMMP3_VVVV:
					writeRegOp("zmm", getVecIndex(i));
					sb.append("+3");
					break;

				case OpCodeOperandKind.BND_REG:
					writeRegOp("bnd", getBndIndex());
					break;

				case OpCodeOperandKind.CR_REG:
					writeRegOp("cr");
					break;

				case OpCodeOperandKind.DR_REG:
					writeRegOp("dr");
					break;

				case OpCodeOperandKind.TR_REG:
					writeRegOp("tr");
					break;

				case OpCodeOperandKind.ES:
					writeRegister("es");
					break;

				case OpCodeOperandKind.CS:
					writeRegister("cs");
					break;

				case OpCodeOperandKind.SS:
					writeRegister("ss");
					break;

				case OpCodeOperandKind.DS:
					writeRegister("ds");
					break;

				case OpCodeOperandKind.FS:
					writeRegister("fs");
					break;

				case OpCodeOperandKind.GS:
					writeRegister("gs");
					break;

				case OpCodeOperandKind.AL:
					writeRegister("al");
					break;

				case OpCodeOperandKind.CL:
					writeRegister("cl");
					break;

				case OpCodeOperandKind.AX:
					writeRegister("ax");
					break;

				case OpCodeOperandKind.DX:
					writeRegister("dx");
					break;

				case OpCodeOperandKind.EAX:
					writeRegister("eax");
					break;

				case OpCodeOperandKind.RAX:
					writeRegister("rax");
					break;

				case OpCodeOperandKind.ST0:
				case OpCodeOperandKind.STI_OPCODE:
					writeRegister("ST");
					if (opKind == OpCodeOperandKind.ST0) {
						switch (opCode.getCode()) {
						case Code.FCOMI_ST0_STI:
						case Code.FCOMIP_ST0_STI:
						case Code.FUCOMI_ST0_STI:
						case Code.FUCOMIP_ST0_STI:
							break;
						default:
							sb.append("(0)");
							break;
						}
					}
					else {
						assert opKind == OpCodeOperandKind.STI_OPCODE : opKind;
						sb.append("(i)");
					}
					break;

				case OpCodeOperandKind.IMM4_M2Z:
					sb.append("imm4");
					break;

				case OpCodeOperandKind.IMM8:
				case OpCodeOperandKind.IMM8SEX16:
				case OpCodeOperandKind.IMM8SEX32:
				case OpCodeOperandKind.IMM8SEX64:
					sb.append("imm8");
					break;

				case OpCodeOperandKind.IMM8_CONST_1:
					sb.append('1');
					break;

				case OpCodeOperandKind.IMM16:
					sb.append("imm16");
					break;

				case OpCodeOperandKind.IMM32:
				case OpCodeOperandKind.IMM32SEX64:
					sb.append("imm32");
					break;

				case OpCodeOperandKind.IMM64:
					sb.append("imm64");
					break;

				case OpCodeOperandKind.SEG_RSI:
				case OpCodeOperandKind.ES_RDI:
				case OpCodeOperandKind.SEG_RDI:
				case OpCodeOperandKind.SEG_RBX_AL:
					addComma = false;
					break;

				case OpCodeOperandKind.BR16_1:
				case OpCodeOperandKind.BR32_1:
				case OpCodeOperandKind.BR64_1:
					sb.append("rel8");
					break;

				case OpCodeOperandKind.BR16_2:
				case OpCodeOperandKind.XBEGIN_2:
					sb.append("rel16");
					break;

				case OpCodeOperandKind.BR32_4:
				case OpCodeOperandKind.BR64_4:
				case OpCodeOperandKind.XBEGIN_4:
					sb.append("rel32");
					break;

				case OpCodeOperandKind.BRDISP_2:
					sb.append("disp16");
					break;

				case OpCodeOperandKind.BRDISP_4:
					sb.append("disp32");
					break;

				case OpCodeOperandKind.NONE:
				default:
					throw new UnsupportedOperationException();
				}

				if (i == saeErIndex && opCode.getEncoding() == EncodingKind.MVEX) {
					if (MvexInfo.getConvFn(opCode.getCode()) != MvexConvFn.NONE)
						sb.append(')');
				}
				if (i == 0) {
					if (opCode.canUseOpMaskRegister()) {
						sb.append(' ');
						writeRegDecorator("k", getKIndex());
						if (opCode.canUseZeroingMasking())
							writeDecorator("z");
					}
				}
				if (i == saeErIndex && opCode.getEncoding() != EncodingKind.MVEX) {
					if (opCode.canSuppressAllExceptions())
						writeDecorator("sae");
					if (opCode.canUseRoundingControl())
						writeDecorator("er");
				}
			}
		}

		switch (opCode.getCode()) {
		// GENERATOR-BEGIN: PrintImpliedOps
		// ⚠️This was generated by GENERATOR!🦹‍♂️
		case Code.TPAUSE_R32:
		case Code.TPAUSE_R64:
		case Code.UMWAIT_R32:
		case Code.UMWAIT_R64:
			writeOpSeparator();
			write("<EDX>", false);
			writeOpSeparator();
			write("<EAX>", false);
			break;
		case Code.PBLENDVB_XMM_XMMM128:
		case Code.BLENDVPS_XMM_XMMM128:
		case Code.BLENDVPD_XMM_XMMM128:
		case Code.SHA256RNDS2_XMM_XMMM128:
			writeOpSeparator();
			write("<XMM0>", true);
			break;
		case Code.AESENCWIDE128KL_M384:
		case Code.AESDECWIDE128KL_M384:
		case Code.AESENCWIDE256KL_M512:
		case Code.AESDECWIDE256KL_M512:
			writeOpSeparator();
			write("<XMM0-7>", true);
			break;
		case Code.LOADIWKEY_XMM_XMM:
			writeOpSeparator();
			write("<EAX>", true);
			writeOpSeparator();
			write("<XMM0>", true);
			break;
		case Code.ENCODEKEY128_R32_R32:
			writeOpSeparator();
			write("<XMM0-2>", true);
			writeOpSeparator();
			write("<XMM4-6>", true);
			break;
		case Code.ENCODEKEY256_R32_R32:
			writeOpSeparator();
			write("<XMM0-6>", true);
			break;
		case Code.HRESET_IMM8:
			writeOpSeparator();
			write("<EAX>", true);
			break;
		// GENERATOR-END: PrintImpliedOps

		default:
			break;
		}

		return sb.toString();
	}

	private void writeMemorySize(int memorySize) {
		switch (opCode.getCode()) {
		case Code.FLDCW_M2BYTE:
		case Code.FNSTCW_M2BYTE:
		case Code.FSTCW_M2BYTE:
		case Code.FNSTSW_M2BYTE:
		case Code.FSTSW_M2BYTE:
			sb.append("2byte");
			return;
		}

		switch (memorySize) {
		case MemorySize.BOUND16_WORD_WORD:
			sb.append("16&16");
			break;

		case MemorySize.BOUND32_DWORD_DWORD:
			sb.append("32&32");
			break;

		case MemorySize.FPU_ENV14:
			sb.append("14byte");
			break;

		case MemorySize.FPU_ENV28:
			sb.append("28byte");
			break;

		case MemorySize.FPU_STATE94:
			sb.append("94byte");
			break;

		case MemorySize.FPU_STATE108:
			sb.append("108byte");
			break;

		case MemorySize.FXSAVE_512BYTE:
		case MemorySize.FXSAVE64_512BYTE:
			sb.append("512byte");
			break;

		case MemorySize.XSAVE:
		case MemorySize.XSAVE64:
			// 'm' has already been appended
			sb.append("em");
			break;

		case MemorySize.SEG_PTR16:
			sb.append("16:16");
			break;

		case MemorySize.SEG_PTR32:
			sb.append("16:32");
			break;

		case MemorySize.SEG_PTR64:
			sb.append("16:64");
			break;

		case MemorySize.FWORD6:
			if (!isSgdtOrSidt())
				sb.append("16&32");
			break;

		case MemorySize.FWORD10:
			if (!isSgdtOrSidt())
				sb.append("16&64");
			break;

		default:
			int memSize = MemorySize.getSize(memorySize);
			if (memSize != 0)
				sb.append(memSize * 8);
			break;
		}

		if (isFpuInstruction(opCode.getCode())) {
			switch (memorySize) {
			case MemorySize.INT16:
			case MemorySize.INT32:
			case MemorySize.INT64:
				sb.append("int");
				break;

			case MemorySize.FLOAT32:
			case MemorySize.FLOAT64:
			case MemorySize.FLOAT80:
				sb.append("fp");
				break;

			case MemorySize.BCD:
				sb.append("bcd");
				break;
			}
		}
	}

	private boolean isSgdtOrSidt() {
		switch (opCode.getCode()) {
		case Code.SGDT_M1632_16:
		case Code.SGDT_M1632:
		case Code.SGDT_M1664:
		case Code.SIDT_M1632_16:
		case Code.SIDT_M1632:
		case Code.SIDT_M1664:
			return true;
		default:
			return false;
		}
	}

	private void writeRegister(String register) {
		write(register, true);
	}

	private void writeRegOp(String register) {
		write(register, false);
	}

	private void writeRegOp(String register, int index) {
		writeRegOp(register);
		if (index > 0)
			sb.append(index);
	}

	private void writeDecorator(String decorator) {
		sb.append('{');
		write(decorator, false);
		sb.append('}');
	}

	private void writeRegDecorator(String register, int index) {
		sb.append('{');
		write(register, false);
		sb.append(index);
		sb.append('}');
	}

	private int appendGprSuffix(int count, int index) {
		if (count <= 1 || noGprSuffix)
			return index;
		sb.append((char)('a' + index));
		index++;
		return index;
	}

	private void writeOpSeparator() {
		sb.append(", ");
	}

	private void write(String s, boolean upper) {
		String fixed = upper ? s.toUpperCase(Locale.ROOT) : s.toLowerCase(Locale.ROOT);
		sb.append(fixed);
	}

	private void writeGprMem(int regSize) {
		assert !opCode.canBroadcast();
		sb.append('r');
		int memSize = MemorySize.getSize(getMemorySize(false)) * 8;
		if (memSize != regSize)
			sb.append(regSize);
		sb.append('/');
		writeMemory();
	}

	private void writeRegMem(String register, int index) {
		writeRegOp(register, index);
		sb.append('/');
		writeMemory();
	}

	private void writeMemory() {
		writeMemory(false);
		if (opCode.canBroadcast()) {
			sb.append('/');
			writeMemory(true);
		}
	}

	private void writeMemory(boolean isBroadcast) {
		int memorySize = getMemorySize(isBroadcast);
		sb.append('m');
		if (opCode.getEncoding() == EncodingKind.MVEX) {
			if (MvexInfo.getEHBit(opCode.getCode()) == MvexEHBit.NONE && !MvexInfo.getIgnoresEvictionHint(opCode.getCode()))
				sb.append('t');
		}
		writeMemorySize(memorySize);
		if (isBroadcast)
			sb.append("bcst");
	}

	private static boolean isFpuInstruction(int code) {
		return Integer.compareUnsigned(code - Code.FADD_M32FP, Code.FCOMIP_ST0_STI - Code.FADD_M32FP) <= 0;
	}
}
