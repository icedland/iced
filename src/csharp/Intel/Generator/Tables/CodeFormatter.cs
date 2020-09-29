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

using System;
using System.Diagnostics;
using System.Text;
using Generator.Enums;
using Generator.Enums.Encoder;

namespace Generator.Tables {
	readonly struct CodeFormatter {
		readonly StringBuilder sb;
		readonly MemorySizeInfoTable memSizeTbl;
		readonly string codeMnemonic;
		readonly string? codeSuffix;
		readonly string? codeMemorySize;
		readonly string? codeMemorySizeSuffix;
		readonly EnumValue memSize;
		readonly EnumValue memSizeBcst;
		readonly InstructionDefFlags1 flags;
		readonly EncodingKind encoding;
		readonly OpCodeOperandKind[] opKinds;

		public CodeFormatter(StringBuilder sb, MemorySizeInfoTable memSizeTbl, string codeMnemonic, string? codeSuffix, string? codeMemorySize, string? codeMemorySizeSuffix, EnumValue memSize, EnumValue memSizeBcst, InstructionDefFlags1 flags, EncodingKind encoding, OpCodeOperandKind[] opKinds) {
			if (codeMnemonic == string.Empty)
				throw new ArgumentOutOfRangeException(nameof(codeMnemonic));
			this.sb = sb;
			this.memSizeTbl = memSizeTbl;
			this.codeMnemonic = codeMnemonic;
			this.codeSuffix = codeSuffix;
			this.codeMemorySize = codeMemorySize;
			this.codeMemorySizeSuffix = codeMemorySizeSuffix;
			this.memSize = memSize;
			this.memSizeBcst = memSizeBcst;
			this.flags = flags;
			this.encoding = encoding;
			this.opKinds = opKinds;
		}

		MemorySize GetMemorySize(bool isBroadcast) => (MemorySize)(isBroadcast ? memSizeBcst.Value : memSize.Value);
		int GetSizeInBytes(MemorySize memSize) => memSizeTbl.Data[(int)memSize].Size;

		public string Format() {
			sb.Clear();

			switch (encoding) {
			case EncodingKind.Legacy:
				break;
			case EncodingKind.VEX:
				sb.Append("VEX_");
				break;
			case EncodingKind.EVEX:
				sb.Append("EVEX_");
				break;
			case EncodingKind.XOP:
				sb.Append("XOP_");
				break;
			case EncodingKind.D3NOW:
				sb.Append("D3NOW_");
				break;
			default:
				throw new InvalidOperationException();
			}

			sb.Append(codeMnemonic);

			if (opKinds.Length > 0) {
				sb.Append('_');
				for (int i = 0; i < opKinds.Length; i++) {
					if (i > 0)
						sb.Append('_');
					var opKind = opKinds[i];
					switch (opKind) {
					case OpCodeOperandKind.farbr2_2:
						sb.Append("ptr1616");
						break;

					case OpCodeOperandKind.farbr4_2:
						sb.Append("ptr1632");
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
						WriteRegMem("mm");
						break;

					case OpCodeOperandKind.xmm_or_mem:
						WriteRegMem("xmm");
						break;

					case OpCodeOperandKind.ymm_or_mem:
						WriteRegMem("ymm");
						break;

					case OpCodeOperandKind.zmm_or_mem:
						WriteRegMem("zmm");
						break;

					case OpCodeOperandKind.tmm_reg:
					case OpCodeOperandKind.tmm_rm:
					case OpCodeOperandKind.tmm_vvvv:
						WriteRegOp("tmm");
						break;

					case OpCodeOperandKind.bnd_or_mem_mpx:
						WriteRegOp("bnd");
						WriteMemory();
						break;

					case OpCodeOperandKind.k_or_mem:
						WriteRegMem("k");
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
						break;

					case OpCodeOperandKind.r64_reg:
					case OpCodeOperandKind.r64_reg_mem:
					case OpCodeOperandKind.r64_rm:
					case OpCodeOperandKind.r64_opcode:
					case OpCodeOperandKind.r64_vvvv:
						WriteRegOp("r64");
						break;

					case OpCodeOperandKind.seg_reg:
						sb.Append("Sreg");
						break;

					case OpCodeOperandKind.k_reg:
					case OpCodeOperandKind.k_rm:
					case OpCodeOperandKind.k_vvvv:
						WriteRegOp("kr");
						break;

					case OpCodeOperandKind.kp1_reg:
						WriteRegOp("kp1");
						break;

					case OpCodeOperandKind.mm_reg:
					case OpCodeOperandKind.mm_rm:
						WriteRegOp("mm");
						break;

					case OpCodeOperandKind.xmm_reg:
					case OpCodeOperandKind.xmm_rm:
					case OpCodeOperandKind.xmm_vvvv:
					case OpCodeOperandKind.xmm_is4:
					case OpCodeOperandKind.xmm_is5:
						WriteRegOp("xmm");
						break;

					case OpCodeOperandKind.xmmp3_vvvv:
						WriteRegOp("xmmp3");
						break;

					case OpCodeOperandKind.ymm_reg:
					case OpCodeOperandKind.ymm_rm:
					case OpCodeOperandKind.ymm_vvvv:
					case OpCodeOperandKind.ymm_is4:
					case OpCodeOperandKind.ymm_is5:
						WriteRegOp("ymm");
						break;

					case OpCodeOperandKind.zmm_reg:
					case OpCodeOperandKind.zmm_rm:
					case OpCodeOperandKind.zmm_vvvv:
						WriteRegOp("zmm");
						break;

					case OpCodeOperandKind.zmmp3_vvvv:
						WriteRegOp("zmmp3");
						break;

					case OpCodeOperandKind.bnd_reg:
						WriteRegOp("bnd");
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
						if (opKind == OpCodeOperandKind.st0)
							sb.Append("st0");
						else {
							Debug.Assert(opKind == OpCodeOperandKind.sti_opcode);
							sb.Append("sti");
						}
						break;

					case OpCodeOperandKind.imm2_m2z:
						sb.Append("imm2");
						break;

					case OpCodeOperandKind.imm8:
					case OpCodeOperandKind.imm8sex16:
					case OpCodeOperandKind.imm8sex32:
					case OpCodeOperandKind.imm8sex64:
						sb.Append("imm8");
						break;

					case OpCodeOperandKind.imm8_const_1:
						sb.Append("1");
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
					case OpCodeOperandKind.seg_rBX_al:
						WriteMemory();
						break;

					case OpCodeOperandKind.seg_rDI:
						sb.Append("rDI");
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

					if (i == 0) {
						if ((flags & InstructionDefFlags1.OpMaskRegister) != 0) {
							sb.Append("_k1");
							if ((flags & InstructionDefFlags1.ZeroingMasking) != 0)
								sb.Append("z");
						}
					}
					if (i == opKinds.Length - 1) {
						if ((flags & InstructionDefFlags1.SuppressAllExceptions) != 0)
							sb.Append("_sae");
						if ((flags & InstructionDefFlags1.RoundingControl) != 0)
							sb.Append("_er");
					}
				}
			}

			if (codeSuffix is object) {
				sb.Append('_');
				sb.Append(codeSuffix);
			}

			return sb.ToString();
		}

		void WriteGprMem(int regSize) {
			sb.Append('r');
			int memSize = GetSizeInBytes(GetMemorySize(isBroadcast: false)) * 8;
			if (memSize != regSize)
				sb.Append(regSize);
			WriteMemory();
		}

		void WriteRegMem(string register) {
			WriteRegOp(register);
			WriteMemory();
		}

		void WriteMemory() {
			WriteMemory(isBroadcast: false);
			if ((flags & InstructionDefFlags1.Broadcast) != 0)
				WriteMemory(isBroadcast: true);
		}

		void WriteMemory(bool isBroadcast) {
			var memorySize = GetMemorySize(isBroadcast);
			sb.Append(isBroadcast ? 'b' : 'm');
			WriteMemorySize(memorySize);
		}

		void WriteMemorySize(MemorySize memorySize) {
			if (codeMemorySize is object)
				sb.Append(codeMemorySize);
			else {
				int memSize = GetSizeInBytes(memorySize);
				if (memSize != 0)
					sb.Append(memSize * 8);
			}

			if (codeMemorySizeSuffix is object)
				sb.Append(codeMemorySizeSuffix);
		}

		void WriteRegister(string register) => sb.Append(register.ToUpperInvariant());
		void WriteRegOp(string register) => sb.Append(register.ToLowerInvariant());
	}
}
