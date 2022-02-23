// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if ENCODER && OPCODE_INFO
using System;
using System.Diagnostics;
using System.Text;

namespace Iced.Intel.EncoderInternal {
	struct InstructionFormatter {
		readonly OpCodeInfo opCode;
		readonly StringBuilder sb;
		readonly int r32_count;
		readonly int r64_count;
		readonly int bnd_count;
		readonly int startOpIndex;
		int r32_index, r64_index, bnd_index;
		int k_index;
		int vec_index;
		int tmm_index;
		readonly int opCount;
		// true: k2 {k1}, false: k1 {k2}
		readonly bool opMaskIsK1;
		readonly bool noVecIndex;
		readonly bool swapVecIndex12;
		readonly bool noGprSuffix;
		readonly bool vecIndexSameAsOpIndex;

#if MVEX
		static readonly string[] ConvFnNames = new string[] {
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
#endif

		int GetKIndex() {
			k_index++;
			if (opMaskIsK1) {
				if (k_index == 1)
					return 2;
				if (k_index == 2)
					return 1;
			}
			return k_index;
		}

		int GetBndIndex() {
			if (bnd_count <= 1)
				return 0;
			bnd_index++;
			return bnd_index;
		}

		int GetVecIndex(int opIndex) {
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

		int GetTmmIndex() {
			tmm_index++;
			return tmm_index;
		}

		public InstructionFormatter(OpCodeInfo opCode, InstrStrFmtOption fmtOption, StringBuilder sb) {
			this.opCode = opCode;
			this.sb = sb;
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
			opCount = opCode.OpCount;
			opMaskIsK1 = false;
			switch (fmtOption) {
			case InstrStrFmtOption.None:
				break;
			case InstrStrFmtOption.OpMaskIsK1_or_NoGprSuffix:
				opMaskIsK1 = true;
				noGprSuffix = true;
				break;
			case InstrStrFmtOption.IncVecIndex:
				vec_index++;
				break;
			case InstrStrFmtOption.NoVecIndex:
				noVecIndex = true;
				break;
			case InstrStrFmtOption.SwapVecIndex12:
				swapVecIndex12 = true;
				break;
			case InstrStrFmtOption.SkipOp0:
				startOpIndex = 1;
				break;
			case InstrStrFmtOption.VecIndexSameAsOpIndex:
				vecIndexSameAsOpIndex = true;
				break;
			default:
				throw new InvalidOperationException();
			}
			if ((opCode.Op0Kind == OpCodeOperandKind.k_reg || opCode.Op0Kind == OpCodeOperandKind.kp1_reg) && opCode.OpCount > 2 &&
				opCode.Encoding != EncodingKind.MVEX) {
				vecIndexSameAsOpIndex = true;
			}
			for (int i = 0; i < opCode.OpCount; i++) {
				switch (opCode.GetOpKind(i)) {
				case OpCodeOperandKind.r32_reg:
				case OpCodeOperandKind.r32_reg_mem:
				case OpCodeOperandKind.r32_rm:
				case OpCodeOperandKind.r32_opcode:
				case OpCodeOperandKind.r32_vvvv:
					r32_count++;
					break;

				case OpCodeOperandKind.r64_reg:
				case OpCodeOperandKind.r64_reg_mem:
				case OpCodeOperandKind.r64_rm:
				case OpCodeOperandKind.r64_opcode:
				case OpCodeOperandKind.r64_vvvv:
					r64_count++;
					break;

				case OpCodeOperandKind.bnd_or_mem_mpx:
				case OpCodeOperandKind.bnd_reg:
					bnd_count++;
					break;

				case OpCodeOperandKind.None:
				case OpCodeOperandKind.farbr2_2:
				case OpCodeOperandKind.farbr4_2:
				case OpCodeOperandKind.mem_offs:
				case OpCodeOperandKind.mem:
				case OpCodeOperandKind.mem_mpx:
				case OpCodeOperandKind.mem_mib:
				case OpCodeOperandKind.mem_vsib32x:
				case OpCodeOperandKind.mem_vsib64x:
				case OpCodeOperandKind.mem_vsib32y:
				case OpCodeOperandKind.mem_vsib64y:
				case OpCodeOperandKind.mem_vsib32z:
				case OpCodeOperandKind.mem_vsib64z:
				case OpCodeOperandKind.r8_or_mem:
				case OpCodeOperandKind.r16_or_mem:
				case OpCodeOperandKind.r32_or_mem:
				case OpCodeOperandKind.r32_or_mem_mpx:
				case OpCodeOperandKind.r64_or_mem:
				case OpCodeOperandKind.r64_or_mem_mpx:
				case OpCodeOperandKind.mm_or_mem:
				case OpCodeOperandKind.xmm_or_mem:
				case OpCodeOperandKind.ymm_or_mem:
				case OpCodeOperandKind.zmm_or_mem:
				case OpCodeOperandKind.k_or_mem:
				case OpCodeOperandKind.r8_reg:
				case OpCodeOperandKind.r8_opcode:
				case OpCodeOperandKind.r16_reg:
				case OpCodeOperandKind.r16_reg_mem:
				case OpCodeOperandKind.r16_rm:
				case OpCodeOperandKind.r16_opcode:
				case OpCodeOperandKind.seg_reg:
				case OpCodeOperandKind.k_reg:
				case OpCodeOperandKind.kp1_reg:
				case OpCodeOperandKind.k_rm:
				case OpCodeOperandKind.k_vvvv:
				case OpCodeOperandKind.mm_reg:
				case OpCodeOperandKind.mm_rm:
				case OpCodeOperandKind.xmm_reg:
				case OpCodeOperandKind.xmm_rm:
				case OpCodeOperandKind.xmm_vvvv:
				case OpCodeOperandKind.xmmp3_vvvv:
				case OpCodeOperandKind.xmm_is4:
				case OpCodeOperandKind.xmm_is5:
				case OpCodeOperandKind.ymm_reg:
				case OpCodeOperandKind.ymm_rm:
				case OpCodeOperandKind.ymm_vvvv:
				case OpCodeOperandKind.ymm_is4:
				case OpCodeOperandKind.ymm_is5:
				case OpCodeOperandKind.zmm_reg:
				case OpCodeOperandKind.zmm_rm:
				case OpCodeOperandKind.zmm_vvvv:
				case OpCodeOperandKind.zmmp3_vvvv:
				case OpCodeOperandKind.cr_reg:
				case OpCodeOperandKind.dr_reg:
				case OpCodeOperandKind.tr_reg:
				case OpCodeOperandKind.es:
				case OpCodeOperandKind.cs:
				case OpCodeOperandKind.ss:
				case OpCodeOperandKind.ds:
				case OpCodeOperandKind.fs:
				case OpCodeOperandKind.gs:
				case OpCodeOperandKind.al:
				case OpCodeOperandKind.cl:
				case OpCodeOperandKind.ax:
				case OpCodeOperandKind.dx:
				case OpCodeOperandKind.eax:
				case OpCodeOperandKind.rax:
				case OpCodeOperandKind.st0:
				case OpCodeOperandKind.sti_opcode:
				case OpCodeOperandKind.imm4_m2z:
				case OpCodeOperandKind.imm8:
				case OpCodeOperandKind.imm8_const_1:
				case OpCodeOperandKind.imm8sex16:
				case OpCodeOperandKind.imm8sex32:
				case OpCodeOperandKind.imm8sex64:
				case OpCodeOperandKind.imm16:
				case OpCodeOperandKind.imm32:
				case OpCodeOperandKind.imm32sex64:
				case OpCodeOperandKind.imm64:
				case OpCodeOperandKind.seg_rDI:
				case OpCodeOperandKind.br16_1:
				case OpCodeOperandKind.br32_1:
				case OpCodeOperandKind.br64_1:
				case OpCodeOperandKind.br16_2:
				case OpCodeOperandKind.br32_4:
				case OpCodeOperandKind.br64_4:
				case OpCodeOperandKind.xbegin_2:
				case OpCodeOperandKind.xbegin_4:
				case OpCodeOperandKind.brdisp_2:
				case OpCodeOperandKind.brdisp_4:
				case OpCodeOperandKind.sibmem:
				case OpCodeOperandKind.tmm_reg:
				case OpCodeOperandKind.tmm_rm:
				case OpCodeOperandKind.tmm_vvvv:
					break;

				case OpCodeOperandKind.seg_rSI:
				case OpCodeOperandKind.es_rDI:
				case OpCodeOperandKind.seg_rBX_al:
					// string instructions, xlat
					opCount = 0;
					break;

				default:
					throw new InvalidOperationException();
				}
			}
		}

		MemorySize GetMemorySize(bool isBroadcast) {
			int index = (int)opCode.Code;
			if (isBroadcast)
				return (MemorySize)InstructionMemorySizes.SizesBcst[index];
			else
				return (MemorySize)InstructionMemorySizes.SizesNormal[index];
		}

		public string Format() {
			if (!opCode.IsInstruction) {
				return opCode.Code switch {
					// GENERATOR-BEGIN: InstrFmtNotInstructionString
					// ⚠️This was generated by GENERATOR!🦹‍♂️
					Code.INVALID => "<invalid>",
					Code.DeclareByte => "<db>",
					Code.DeclareWord => "<dw>",
					Code.DeclareDword => "<dd>",
					Code.DeclareQword => "<dq>",
					Code.Zero_bytes => "ZERO_BYTES",
					// GENERATOR-END: InstrFmtNotInstructionString
					_ => throw new InvalidOperationException(),
				};
			}

			sb.Length = 0;

			Write(opCode.Mnemonic.ToString(), upper: true);
			if (startOpIndex < opCount) {
				sb.Append(' ');
				int saeErIndex = opCount - 1;
				if (opCode.Encoding != EncodingKind.Legacy && opCode.GetOpKind(saeErIndex) == OpCodeOperandKind.imm8)
					saeErIndex--;
				bool addComma = false;
				for (int i = startOpIndex; i < opCount; i++) {
					if (addComma)
						WriteOpSeparator();
					addComma = true;

#if MVEX
					if (i == saeErIndex && opCode.Encoding == EncodingKind.MVEX) {
						var mvexInfo = new MvexInfo(opCode.Code);
						var convFn = mvexInfo.ConvFn;
						if (convFn != MvexConvFn.None) {
							sb.Append(ConvFnNames[(int)convFn - 1]);
							sb.Append('(');
						}
					}
#endif
					var opKind = opCode.GetOpKind(i);
					switch (opKind) {
					case OpCodeOperandKind.farbr2_2:
						sb.Append("ptr16:16");
						break;

					case OpCodeOperandKind.farbr4_2:
						sb.Append("ptr16:32");
						break;

					case OpCodeOperandKind.mem_offs:
						sb.Append("moffs");
						WriteMemorySize(GetMemorySize(isBroadcast: false));
						break;

					case OpCodeOperandKind.mem:
					case OpCodeOperandKind.mem_mpx:
						WriteMemory();
						break;

					case OpCodeOperandKind.sibmem:
						sb.Append("sibmem");
						break;

					case OpCodeOperandKind.mem_mib:
						sb.Append("mib");
						break;

					case OpCodeOperandKind.mem_vsib32x:
						sb.Append("vm32x");
						break;

					case OpCodeOperandKind.mem_vsib64x:
						sb.Append("vm64x");
						break;

					case OpCodeOperandKind.mem_vsib32y:
						sb.Append("vm32y");
						break;

					case OpCodeOperandKind.mem_vsib64y:
						sb.Append("vm64y");
						break;

					case OpCodeOperandKind.mem_vsib32z:
						if (opCode.Encoding == EncodingKind.MVEX)
							sb.Append("mvt");
						else
							sb.Append("vm32z");
						break;

					case OpCodeOperandKind.mem_vsib64z:
						sb.Append("vm64z");
						break;

					case OpCodeOperandKind.r8_or_mem:
						WriteGprMem(8);
						break;

					case OpCodeOperandKind.r16_or_mem:
						WriteGprMem(16);
						break;

					case OpCodeOperandKind.r32_or_mem:
					case OpCodeOperandKind.r32_or_mem_mpx:
						WriteGprMem(32);
						break;

					case OpCodeOperandKind.r64_or_mem:
					case OpCodeOperandKind.r64_or_mem_mpx:
						WriteGprMem(64);
						break;

					case OpCodeOperandKind.mm_or_mem:
						WriteRegMem("mm", GetVecIndex(i));
						break;

					case OpCodeOperandKind.xmm_or_mem:
						WriteRegMem("xmm", GetVecIndex(i));
						break;

					case OpCodeOperandKind.ymm_or_mem:
						WriteRegMem("ymm", GetVecIndex(i));
						break;

					case OpCodeOperandKind.zmm_or_mem:
						WriteRegMem("zmm", GetVecIndex(i));
						break;

					case OpCodeOperandKind.tmm_reg:
					case OpCodeOperandKind.tmm_rm:
					case OpCodeOperandKind.tmm_vvvv:
						WriteRegOp("tmm", GetTmmIndex());
						break;

					case OpCodeOperandKind.bnd_or_mem_mpx:
						WriteRegOp("bnd", GetBndIndex());
						sb.Append('/');
						WriteMemory();
						break;

					case OpCodeOperandKind.k_or_mem:
						WriteRegMem("k", GetKIndex());
						break;

					case OpCodeOperandKind.r8_reg:
					case OpCodeOperandKind.r8_opcode:
						WriteRegOp("r8");
						break;

					case OpCodeOperandKind.r16_reg:
					case OpCodeOperandKind.r16_reg_mem:
					case OpCodeOperandKind.r16_rm:
					case OpCodeOperandKind.r16_opcode:
						WriteRegOp("r16");
						break;

					case OpCodeOperandKind.r32_reg:
					case OpCodeOperandKind.r32_reg_mem:
					case OpCodeOperandKind.r32_rm:
					case OpCodeOperandKind.r32_opcode:
					case OpCodeOperandKind.r32_vvvv:
						WriteRegOp("r32");
						AppendGprSuffix(r32_count, ref r32_index);
						break;

					case OpCodeOperandKind.r64_reg:
					case OpCodeOperandKind.r64_reg_mem:
					case OpCodeOperandKind.r64_rm:
					case OpCodeOperandKind.r64_opcode:
					case OpCodeOperandKind.r64_vvvv:
						WriteRegOp("r64");
						AppendGprSuffix(r64_count, ref r64_index);
						break;

					case OpCodeOperandKind.seg_reg:
						sb.Append("Sreg");
						break;

					case OpCodeOperandKind.k_reg:
					case OpCodeOperandKind.k_rm:
					case OpCodeOperandKind.k_vvvv:
						WriteRegOp("k", GetKIndex());
						break;

					case OpCodeOperandKind.kp1_reg:
						WriteRegOp("k", GetKIndex());
						sb.Append("+1");
						break;

					case OpCodeOperandKind.mm_reg:
					case OpCodeOperandKind.mm_rm:
						WriteRegOp("mm", GetVecIndex(i));
						break;

					case OpCodeOperandKind.xmm_reg:
					case OpCodeOperandKind.xmm_rm:
					case OpCodeOperandKind.xmm_vvvv:
					case OpCodeOperandKind.xmm_is4:
					case OpCodeOperandKind.xmm_is5:
						WriteRegOp("xmm", GetVecIndex(i));
						break;

					case OpCodeOperandKind.xmmp3_vvvv:
						WriteRegOp("xmm", GetVecIndex(i));
						sb.Append("+3");
						break;

					case OpCodeOperandKind.ymm_reg:
					case OpCodeOperandKind.ymm_rm:
					case OpCodeOperandKind.ymm_vvvv:
					case OpCodeOperandKind.ymm_is4:
					case OpCodeOperandKind.ymm_is5:
						WriteRegOp("ymm", GetVecIndex(i));
						break;

					case OpCodeOperandKind.zmm_reg:
					case OpCodeOperandKind.zmm_rm:
					case OpCodeOperandKind.zmm_vvvv:
						WriteRegOp("zmm", GetVecIndex(i));
						break;

					case OpCodeOperandKind.zmmp3_vvvv:
						WriteRegOp("zmm", GetVecIndex(i));
						sb.Append("+3");
						break;

					case OpCodeOperandKind.bnd_reg:
						WriteRegOp("bnd", GetBndIndex());
						break;

					case OpCodeOperandKind.cr_reg:
						WriteRegOp("cr");
						break;

					case OpCodeOperandKind.dr_reg:
						WriteRegOp("dr");
						break;

					case OpCodeOperandKind.tr_reg:
						WriteRegOp("tr");
						break;

					case OpCodeOperandKind.es:
						WriteRegister("es");
						break;

					case OpCodeOperandKind.cs:
						WriteRegister("cs");
						break;

					case OpCodeOperandKind.ss:
						WriteRegister("ss");
						break;

					case OpCodeOperandKind.ds:
						WriteRegister("ds");
						break;

					case OpCodeOperandKind.fs:
						WriteRegister("fs");
						break;

					case OpCodeOperandKind.gs:
						WriteRegister("gs");
						break;

					case OpCodeOperandKind.al:
						WriteRegister("al");
						break;

					case OpCodeOperandKind.cl:
						WriteRegister("cl");
						break;

					case OpCodeOperandKind.ax:
						WriteRegister("ax");
						break;

					case OpCodeOperandKind.dx:
						WriteRegister("dx");
						break;

					case OpCodeOperandKind.eax:
						WriteRegister("eax");
						break;

					case OpCodeOperandKind.rax:
						WriteRegister("rax");
						break;

					case OpCodeOperandKind.st0:
					case OpCodeOperandKind.sti_opcode:
						WriteRegister("ST");
						if (opKind == OpCodeOperandKind.st0) {
							switch (opCode.Code) {
							case Code.Fcomi_st0_sti:
							case Code.Fcomip_st0_sti:
							case Code.Fucomi_st0_sti:
							case Code.Fucomip_st0_sti:
								break;
							default:
								sb.Append("(0)");
								break;
							}
						}
						else {
							Debug.Assert(opKind == OpCodeOperandKind.sti_opcode);
							sb.Append("(i)");
						}
						break;

					case OpCodeOperandKind.imm4_m2z:
						sb.Append("imm4");
						break;

					case OpCodeOperandKind.imm8:
					case OpCodeOperandKind.imm8sex16:
					case OpCodeOperandKind.imm8sex32:
					case OpCodeOperandKind.imm8sex64:
						sb.Append("imm8");
						break;

					case OpCodeOperandKind.imm8_const_1:
						_ = sb.Append('1');
						break;

					case OpCodeOperandKind.imm16:
						sb.Append("imm16");
						break;

					case OpCodeOperandKind.imm32:
					case OpCodeOperandKind.imm32sex64:
						sb.Append("imm32");
						break;

					case OpCodeOperandKind.imm64:
						sb.Append("imm64");
						break;

					case OpCodeOperandKind.seg_rSI:
					case OpCodeOperandKind.es_rDI:
					case OpCodeOperandKind.seg_rDI:
					case OpCodeOperandKind.seg_rBX_al:
						addComma = false;
						break;

					case OpCodeOperandKind.br16_1:
					case OpCodeOperandKind.br32_1:
					case OpCodeOperandKind.br64_1:
						sb.Append("rel8");
						break;

					case OpCodeOperandKind.br16_2:
					case OpCodeOperandKind.xbegin_2:
						sb.Append("rel16");
						break;

					case OpCodeOperandKind.br32_4:
					case OpCodeOperandKind.br64_4:
					case OpCodeOperandKind.xbegin_4:
						sb.Append("rel32");
						break;

					case OpCodeOperandKind.brdisp_2:
						sb.Append("disp16");
						break;

					case OpCodeOperandKind.brdisp_4:
						sb.Append("disp32");
						break;

					case OpCodeOperandKind.None:
					default:
						throw new InvalidOperationException();
					}

#if MVEX
					if (i == saeErIndex && opCode.Encoding == EncodingKind.MVEX) {
						var mvexInfo = new MvexInfo(opCode.Code);
						if (mvexInfo.ConvFn != MvexConvFn.None)
							sb.Append(')');
					}
#endif
					if (i == 0) {
						if (opCode.CanUseOpMaskRegister) {
							sb.Append(' ');
							WriteRegDecorator("k", GetKIndex());
							if (opCode.CanUseZeroingMasking)
								WriteDecorator("z");
						}
					}
					if (i == saeErIndex && opCode.Encoding != EncodingKind.MVEX) {
						if (opCode.CanSuppressAllExceptions)
							WriteDecorator("sae");
						if (opCode.CanUseRoundingControl)
							WriteDecorator("er");
					}
				}
			}

			switch (opCode.Code) {
			// GENERATOR-BEGIN: PrintImpliedOps
			// ⚠️This was generated by GENERATOR!🦹‍♂️
			case Code.Tpause_r32:
			case Code.Tpause_r64:
			case Code.Umwait_r32:
			case Code.Umwait_r64:
				WriteOpSeparator();
				Write("<EDX>", upper: false);
				WriteOpSeparator();
				Write("<EAX>", upper: false);
				break;
			case Code.Pblendvb_xmm_xmmm128:
			case Code.Blendvps_xmm_xmmm128:
			case Code.Blendvpd_xmm_xmmm128:
			case Code.Sha256rnds2_xmm_xmmm128:
				WriteOpSeparator();
				Write("<XMM0>", upper: true);
				break;
			case Code.Aesencwide128kl_m384:
			case Code.Aesdecwide128kl_m384:
			case Code.Aesencwide256kl_m512:
			case Code.Aesdecwide256kl_m512:
				WriteOpSeparator();
				Write("<XMM0-7>", upper: true);
				break;
			case Code.Loadiwkey_xmm_xmm:
				WriteOpSeparator();
				Write("<EAX>", upper: true);
				WriteOpSeparator();
				Write("<XMM0>", upper: true);
				break;
			case Code.Encodekey128_r32_r32:
				WriteOpSeparator();
				Write("<XMM0-2>", upper: true);
				WriteOpSeparator();
				Write("<XMM4-6>", upper: true);
				break;
			case Code.Encodekey256_r32_r32:
				WriteOpSeparator();
				Write("<XMM0-6>", upper: true);
				break;
			case Code.Hreset_imm8:
				WriteOpSeparator();
				Write("<EAX>", upper: true);
				break;
			// GENERATOR-END: PrintImpliedOps

			default:
				break;
			}

			return sb.ToString();
		}

		void WriteMemorySize(MemorySize memorySize) {
			switch (opCode.Code) {
			case Code.Fldcw_m2byte:
			case Code.Fnstcw_m2byte:
			case Code.Fstcw_m2byte:
			case Code.Fnstsw_m2byte:
			case Code.Fstsw_m2byte:
				sb.Append("2byte");
				return;
			}

			switch (memorySize) {
			case MemorySize.Bound16_WordWord:
				sb.Append("16&16");
				break;

			case MemorySize.Bound32_DwordDword:
				sb.Append("32&32");
				break;

			case MemorySize.FpuEnv14:
				sb.Append("14byte");
				break;

			case MemorySize.FpuEnv28:
				sb.Append("28byte");
				break;

			case MemorySize.FpuState94:
				sb.Append("94byte");
				break;

			case MemorySize.FpuState108:
				sb.Append("108byte");
				break;

			case MemorySize.Fxsave_512Byte:
			case MemorySize.Fxsave64_512Byte:
				sb.Append("512byte");
				break;

			case MemorySize.Xsave:
			case MemorySize.Xsave64:
				// 'm' has already been appended
				sb.Append("em");
				break;

			case MemorySize.SegPtr16:
				sb.Append("16:16");
				break;

			case MemorySize.SegPtr32:
				sb.Append("16:32");
				break;

			case MemorySize.SegPtr64:
				sb.Append("16:64");
				break;

			case MemorySize.Fword6:
				if (!IsSgdtOrSidt())
					sb.Append("16&32");
				break;

			case MemorySize.Fword10:
				if (!IsSgdtOrSidt())
					sb.Append("16&64");
				break;

			default:
				int memSize = memorySize.GetSize();
				if (memSize != 0)
					sb.Append(memSize * 8);
				break;
			}

			if (IsFpuInstruction(opCode.Code)) {
				switch (memorySize) {
				case MemorySize.Int16:
				case MemorySize.Int32:
				case MemorySize.Int64:
					sb.Append("int");
					break;

				case MemorySize.Float32:
				case MemorySize.Float64:
				case MemorySize.Float80:
					sb.Append("fp");
					break;

				case MemorySize.Bcd:
					sb.Append("bcd");
					break;
				}
			}
		}

		bool IsSgdtOrSidt() =>
			opCode.Code switch {
				Code.Sgdt_m1632_16 or Code.Sgdt_m1632 or Code.Sgdt_m1664 or Code.Sidt_m1632_16 or Code.Sidt_m1632 or Code.Sidt_m1664 => true,
				_ => false,
			};

		void WriteRegister(string register) => Write(register, upper: true);
		void WriteRegOp(string register) => Write(register, upper: false);
		void WriteRegOp(string register, int index) {
			WriteRegOp(register);
			if (index > 0)
				sb.Append(index);
		}
		void WriteDecorator(string decorator) {
			sb.Append('{');
			Write(decorator, upper: false);
			sb.Append('}');
		}
		void WriteRegDecorator(string register, int index) {
			sb.Append('{');
			Write(register, upper: false);
			sb.Append(index);
			sb.Append('}');
		}

		void AppendGprSuffix(int count, ref int index) {
			if (count <= 1 || noGprSuffix)
				return;
			sb.Append((char)('a' + index));
			index++;
		}

		void WriteOpSeparator() => sb.Append(", ");

		void Write(string s, bool upper) {
			for (int i = 0; i < s.Length; i++) {
				var c = s[i];
				c = upper ? char.ToUpperInvariant(c) : char.ToLowerInvariant(c);
				sb.Append(c);
			}
		}

		void WriteGprMem(int regSize) {
			Debug.Assert(!opCode.CanBroadcast);
			sb.Append('r');
			int memSize = GetMemorySize(isBroadcast: false).GetSize() * 8;
			if (memSize != regSize)
				sb.Append(regSize);
			sb.Append('/');
			WriteMemory();
		}

		void WriteRegMem(string register, int index) {
			WriteRegOp(register, index);
			sb.Append('/');
			WriteMemory();
		}

		void WriteMemory() {
			WriteMemory(isBroadcast: false);
			if (opCode.CanBroadcast) {
				sb.Append('/');
				WriteMemory(isBroadcast: true);
			}
		}

		void WriteMemory(bool isBroadcast) {
			var memorySize = GetMemorySize(isBroadcast);
			sb.Append('m');
#if MVEX
			if (opCode.Encoding == EncodingKind.MVEX) {
				var mvexInfo = new MvexInfo(opCode.Code);
				if (mvexInfo.EHBit == MvexEHBit.None && !mvexInfo.IgnoresEvictionHint)
					sb.Append('t');
			}
#endif
			WriteMemorySize(memorySize);
			if (isBroadcast)
				sb.Append("bcst");
		}

		static bool IsFpuInstruction(Code code) =>
			(uint)(code - Code.Fadd_m32fp) <= (uint)(Code.Fcomip_st0_sti - Code.Fadd_m32fp);
	}
}
#endif
