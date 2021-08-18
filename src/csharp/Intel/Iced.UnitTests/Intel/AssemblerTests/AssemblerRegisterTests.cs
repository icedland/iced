// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if ENCODER && BLOCK_ENCODER && CODE_ASSEMBLER
using Iced.Intel;
using Xunit;
using static Iced.Intel.AssemblerRegisters;

namespace Iced.UnitTests.Intel.AssemblerTests {
	public class AssemblerRegisterTests {
		[Fact]
		public void TestMemoryOperands() {
			{
				Assert.NotEqual(st0, st1);

				Assert.Equal(Register.AL, al);
				Assert.Equal(Register.CL, cl);
				Assert.Equal(Register.DL, dl);
				Assert.Equal(Register.BL, bl);
				Assert.Equal(Register.AH, ah);
				Assert.Equal(Register.CH, ch);
				Assert.Equal(Register.DH, dh);
				Assert.Equal(Register.BH, bh);
				Assert.Equal(Register.SPL, spl);
				Assert.Equal(Register.BPL, bpl);
				Assert.Equal(Register.SIL, sil);
				Assert.Equal(Register.DIL, dil);
				Assert.Equal(Register.R8L, r8b);
				Assert.Equal(Register.R9L, r9b);
				Assert.Equal(Register.R10L, r10b);
				Assert.Equal(Register.R11L, r11b);
				Assert.Equal(Register.R12L, r12b);
				Assert.Equal(Register.R13L, r13b);
				Assert.Equal(Register.R14L, r14b);
				Assert.Equal(Register.R15L, r15b);
				Assert.Equal(Register.AX, ax);
				Assert.Equal(Register.CX, cx);
				Assert.Equal(Register.DX, dx);
				Assert.Equal(Register.BX, bx);
				Assert.Equal(Register.SP, sp);
				Assert.Equal(Register.BP, bp);
				Assert.Equal(Register.SI, si);
				Assert.Equal(Register.DI, di);
				Assert.Equal(Register.R8W, r8w);
				Assert.Equal(Register.R9W, r9w);
				Assert.Equal(Register.R10W, r10w);
				Assert.Equal(Register.R11W, r11w);
				Assert.Equal(Register.R12W, r12w);
				Assert.Equal(Register.R13W, r13w);
				Assert.Equal(Register.R14W, r14w);
				Assert.Equal(Register.R15W, r15w);
				Assert.Equal(Register.EAX, eax);
				Assert.Equal(Register.ECX, ecx);
				Assert.Equal(Register.EDX, edx);
				Assert.Equal(Register.EBX, ebx);
				Assert.Equal(Register.ESP, esp);
				Assert.Equal(Register.EBP, ebp);
				Assert.Equal(Register.ESI, esi);
				Assert.Equal(Register.EDI, edi);
				Assert.Equal(Register.R8D, r8d);
				Assert.Equal(Register.R9D, r9d);
				Assert.Equal(Register.R10D, r10d);
				Assert.Equal(Register.R11D, r11d);
				Assert.Equal(Register.R12D, r12d);
				Assert.Equal(Register.R13D, r13d);
				Assert.Equal(Register.R14D, r14d);
				Assert.Equal(Register.R15D, r15d);
				Assert.Equal(Register.RAX, rax);
				Assert.Equal(Register.RCX, rcx);
				Assert.Equal(Register.RDX, rdx);
				Assert.Equal(Register.RBX, rbx);
				Assert.Equal(Register.RSP, rsp);
				Assert.Equal(Register.RBP, rbp);
				Assert.Equal(Register.RSI, rsi);
				Assert.Equal(Register.RDI, rdi);
				Assert.Equal(Register.R8, r8);
				Assert.Equal(Register.R9, r9);
				Assert.Equal(Register.R10, r10);
				Assert.Equal(Register.R11, r11);
				Assert.Equal(Register.R12, r12);
				Assert.Equal(Register.R13, r13);
				Assert.Equal(Register.R14, r14);
				Assert.Equal(Register.R15, r15);
				Assert.Equal(Register.ES, es);
				Assert.Equal(Register.CS, cs);
				Assert.Equal(Register.SS, ss);
				Assert.Equal(Register.DS, ds);
				Assert.Equal(Register.FS, fs);
				Assert.Equal(Register.GS, gs);
				Assert.Equal(Register.XMM0, xmm0);
				Assert.Equal(Register.XMM1, xmm1);
				Assert.Equal(Register.XMM2, xmm2);
				Assert.Equal(Register.XMM3, xmm3);
				Assert.Equal(Register.XMM4, xmm4);
				Assert.Equal(Register.XMM5, xmm5);
				Assert.Equal(Register.XMM6, xmm6);
				Assert.Equal(Register.XMM7, xmm7);
				Assert.Equal(Register.XMM8, xmm8);
				Assert.Equal(Register.XMM9, xmm9);
				Assert.Equal(Register.XMM10, xmm10);
				Assert.Equal(Register.XMM11, xmm11);
				Assert.Equal(Register.XMM12, xmm12);
				Assert.Equal(Register.XMM13, xmm13);
				Assert.Equal(Register.XMM14, xmm14);
				Assert.Equal(Register.XMM15, xmm15);
				Assert.Equal(Register.XMM16, xmm16);
				Assert.Equal(Register.XMM17, xmm17);
				Assert.Equal(Register.XMM18, xmm18);
				Assert.Equal(Register.XMM19, xmm19);
				Assert.Equal(Register.XMM20, xmm20);
				Assert.Equal(Register.XMM21, xmm21);
				Assert.Equal(Register.XMM22, xmm22);
				Assert.Equal(Register.XMM23, xmm23);
				Assert.Equal(Register.XMM24, xmm24);
				Assert.Equal(Register.XMM25, xmm25);
				Assert.Equal(Register.XMM26, xmm26);
				Assert.Equal(Register.XMM27, xmm27);
				Assert.Equal(Register.XMM28, xmm28);
				Assert.Equal(Register.XMM29, xmm29);
				Assert.Equal(Register.XMM30, xmm30);
				Assert.Equal(Register.XMM31, xmm31);
				Assert.Equal(Register.YMM0, ymm0);
				Assert.Equal(Register.YMM1, ymm1);
				Assert.Equal(Register.YMM2, ymm2);
				Assert.Equal(Register.YMM3, ymm3);
				Assert.Equal(Register.YMM4, ymm4);
				Assert.Equal(Register.YMM5, ymm5);
				Assert.Equal(Register.YMM6, ymm6);
				Assert.Equal(Register.YMM7, ymm7);
				Assert.Equal(Register.YMM8, ymm8);
				Assert.Equal(Register.YMM9, ymm9);
				Assert.Equal(Register.YMM10, ymm10);
				Assert.Equal(Register.YMM11, ymm11);
				Assert.Equal(Register.YMM12, ymm12);
				Assert.Equal(Register.YMM13, ymm13);
				Assert.Equal(Register.YMM14, ymm14);
				Assert.Equal(Register.YMM15, ymm15);
				Assert.Equal(Register.YMM16, ymm16);
				Assert.Equal(Register.YMM17, ymm17);
				Assert.Equal(Register.YMM18, ymm18);
				Assert.Equal(Register.YMM19, ymm19);
				Assert.Equal(Register.YMM20, ymm20);
				Assert.Equal(Register.YMM21, ymm21);
				Assert.Equal(Register.YMM22, ymm22);
				Assert.Equal(Register.YMM23, ymm23);
				Assert.Equal(Register.YMM24, ymm24);
				Assert.Equal(Register.YMM25, ymm25);
				Assert.Equal(Register.YMM26, ymm26);
				Assert.Equal(Register.YMM27, ymm27);
				Assert.Equal(Register.YMM28, ymm28);
				Assert.Equal(Register.YMM29, ymm29);
				Assert.Equal(Register.YMM30, ymm30);
				Assert.Equal(Register.YMM31, ymm31);
				Assert.Equal(Register.ZMM0, zmm0);
				Assert.Equal(Register.ZMM1, zmm1);
				Assert.Equal(Register.ZMM2, zmm2);
				Assert.Equal(Register.ZMM3, zmm3);
				Assert.Equal(Register.ZMM4, zmm4);
				Assert.Equal(Register.ZMM5, zmm5);
				Assert.Equal(Register.ZMM6, zmm6);
				Assert.Equal(Register.ZMM7, zmm7);
				Assert.Equal(Register.ZMM8, zmm8);
				Assert.Equal(Register.ZMM9, zmm9);
				Assert.Equal(Register.ZMM10, zmm10);
				Assert.Equal(Register.ZMM11, zmm11);
				Assert.Equal(Register.ZMM12, zmm12);
				Assert.Equal(Register.ZMM13, zmm13);
				Assert.Equal(Register.ZMM14, zmm14);
				Assert.Equal(Register.ZMM15, zmm15);
				Assert.Equal(Register.ZMM16, zmm16);
				Assert.Equal(Register.ZMM17, zmm17);
				Assert.Equal(Register.ZMM18, zmm18);
				Assert.Equal(Register.ZMM19, zmm19);
				Assert.Equal(Register.ZMM20, zmm20);
				Assert.Equal(Register.ZMM21, zmm21);
				Assert.Equal(Register.ZMM22, zmm22);
				Assert.Equal(Register.ZMM23, zmm23);
				Assert.Equal(Register.ZMM24, zmm24);
				Assert.Equal(Register.ZMM25, zmm25);
				Assert.Equal(Register.ZMM26, zmm26);
				Assert.Equal(Register.ZMM27, zmm27);
				Assert.Equal(Register.ZMM28, zmm28);
				Assert.Equal(Register.ZMM29, zmm29);
				Assert.Equal(Register.ZMM30, zmm30);
				Assert.Equal(Register.ZMM31, zmm31);
				Assert.Equal(Register.K0, k0);
				Assert.Equal(Register.K1, k1);
				Assert.Equal(Register.K2, k2);
				Assert.Equal(Register.K3, k3);
				Assert.Equal(Register.K4, k4);
				Assert.Equal(Register.K5, k5);
				Assert.Equal(Register.K6, k6);
				Assert.Equal(Register.K7, k7);
				Assert.Equal(Register.BND0, bnd0);
				Assert.Equal(Register.BND1, bnd1);
				Assert.Equal(Register.BND2, bnd2);
				Assert.Equal(Register.BND3, bnd3);
				Assert.Equal(Register.CR0, cr0);
				Assert.Equal(Register.CR1, cr1);
				Assert.Equal(Register.CR2, cr2);
				Assert.Equal(Register.CR3, cr3);
				Assert.Equal(Register.CR4, cr4);
				Assert.Equal(Register.CR5, cr5);
				Assert.Equal(Register.CR6, cr6);
				Assert.Equal(Register.CR7, cr7);
				Assert.Equal(Register.CR8, cr8);
				Assert.Equal(Register.CR9, cr9);
				Assert.Equal(Register.CR10, cr10);
				Assert.Equal(Register.CR11, cr11);
				Assert.Equal(Register.CR12, cr12);
				Assert.Equal(Register.CR13, cr13);
				Assert.Equal(Register.CR14, cr14);
				Assert.Equal(Register.CR15, cr15);
				Assert.Equal(Register.DR0, dr0);
				Assert.Equal(Register.DR1, dr1);
				Assert.Equal(Register.DR2, dr2);
				Assert.Equal(Register.DR3, dr3);
				Assert.Equal(Register.DR4, dr4);
				Assert.Equal(Register.DR5, dr5);
				Assert.Equal(Register.DR6, dr6);
				Assert.Equal(Register.DR7, dr7);
				Assert.Equal(Register.DR8, dr8);
				Assert.Equal(Register.DR9, dr9);
				Assert.Equal(Register.DR10, dr10);
				Assert.Equal(Register.DR11, dr11);
				Assert.Equal(Register.DR12, dr12);
				Assert.Equal(Register.DR13, dr13);
				Assert.Equal(Register.DR14, dr14);
				Assert.Equal(Register.DR15, dr15);
				Assert.Equal(Register.ST0, st0);
				Assert.Equal(Register.ST1, st1);
				Assert.Equal(Register.ST2, st2);
				Assert.Equal(Register.ST3, st3);
				Assert.Equal(Register.ST4, st4);
				Assert.Equal(Register.ST5, st5);
				Assert.Equal(Register.ST6, st6);
				Assert.Equal(Register.ST7, st7);
				Assert.Equal(Register.MM0, mm0);
				Assert.Equal(Register.MM1, mm1);
				Assert.Equal(Register.MM2, mm2);
				Assert.Equal(Register.MM3, mm3);
				Assert.Equal(Register.MM4, mm4);
				Assert.Equal(Register.MM5, mm5);
				Assert.Equal(Register.MM6, mm6);
				Assert.Equal(Register.MM7, mm7);
				Assert.Equal(Register.TR0, tr0);
				Assert.Equal(Register.TR1, tr1);
				Assert.Equal(Register.TR2, tr2);
				Assert.Equal(Register.TR3, tr3);
				Assert.Equal(Register.TR4, tr4);
				Assert.Equal(Register.TR5, tr5);
				Assert.Equal(Register.TR6, tr6);
				Assert.Equal(Register.TR7, tr7);
			}

			{
				Assert.Equal(AssemblerOperandFlags.K1, zmm0.k1.Flags);
				Assert.Equal(AssemblerOperandFlags.K2 | AssemblerOperandFlags.Zeroing, zmm0.k2.z.Flags);
				Assert.Equal(AssemblerOperandFlags.SuppressAllExceptions, zmm0.sae.Flags);
				Assert.Equal(AssemblerOperandFlags.RoundToNearest, zmm0.rn_sae.Flags);
				Assert.Equal(AssemblerOperandFlags.RoundDown, zmm0.rd_sae.Flags);
				Assert.Equal(AssemblerOperandFlags.RoundUp, zmm0.ru_sae.Flags);
				Assert.Equal(AssemblerOperandFlags.RoundTowardZero, zmm0.rz_sae.Flags);

				Assert.NotEqual(zmm0, zmm1);
				Assert.NotEqual(zmm0, zmm0.k1);
				Assert.NotEqual(zmm0.GetHashCode(), zmm1.GetHashCode());
				Assert.NotEqual(zmm0.GetHashCode(), zmm0.k1.GetHashCode());
			}

			{
				var m = __.cs[13];
				Assert.Equal(new MemoryOperand(Register.None, Register.None, 1, 13, 8, false, Register.CS), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.None, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}
			{
				var m = __.ds[13];
				Assert.Equal(new MemoryOperand(Register.None, Register.None, 1, 13, 8, false, Register.DS), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.None, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}
			{
				var m = __.es[13];
				Assert.Equal(new MemoryOperand(Register.None, Register.None, 1, 13, 8, false, Register.ES), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.None, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}
			{
				var m = __.fs[13];
				Assert.Equal(new MemoryOperand(Register.None, Register.None, 1, 13, 8, false, Register.FS), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.None, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}
			{
				var m = __.gs[13];
				Assert.Equal(new MemoryOperand(Register.None, Register.None, 1, 13, 8, false, Register.GS), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.None, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}
			{
				var m = __.ss[13];
				Assert.Equal(new MemoryOperand(Register.None, Register.None, 1, 13, 8, false, Register.SS), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.None, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}
			{
				var m = __[sbyte.MinValue];
				Assert.Equal(new MemoryOperand(Register.None, Register.None, 1, sbyte.MinValue, 8), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.None, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}

			{
				var m = __[sbyte.MaxValue];
				Assert.Equal(new MemoryOperand(Register.None, Register.None, 1, sbyte.MaxValue, 8), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.None, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}

			{
				var m = __[short.MinValue];
				Assert.Equal(new MemoryOperand(Register.None, Register.None, 1, short.MinValue, 8), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.None, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}

			{
				var m = __[short.MaxValue];
				Assert.Equal(new MemoryOperand(Register.None, Register.None, 1, short.MaxValue, 8), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.None, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}

			{
				var m = __[int.MinValue];
				Assert.Equal(new MemoryOperand(Register.None, Register.None, 1, int.MinValue, 8), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.None, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}

			{
				var m = __[int.MaxValue];
				Assert.Equal(new MemoryOperand(Register.None, Register.None, 1, int.MaxValue, 8), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.None, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}

			{
				var m = __[(long)int.MaxValue + 1];
				Assert.Equal(new MemoryOperand(Register.None, Register.None, 1, (long)int.MaxValue + 1, 8), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.None, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}

			{
				var m = __[ulong.MaxValue];
				Assert.Equal(new MemoryOperand(Register.None, Register.None, 1, unchecked((long)ulong.MaxValue), 8), m.ToMemoryOperand(64));
				Assert.Equal(ulong.MaxValue, (ulong)m.Displacement);
				Assert.Equal(MemoryOperandSize.None, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}
			{
				var m = __[eax];
				Assert.Equal(new MemoryOperand(Register.EAX, Register.None, 1, 0, 0), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.None, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}

			{
				var m = __[eax + 1];
				Assert.Equal(new MemoryOperand(Register.EAX, Register.None, 1, 1, 1), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.None, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}

			{
				var m = __[eax - 1];
				Assert.Equal(new MemoryOperand(Register.EAX, Register.None, 1, -1, 1), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.None, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}

			{
				var m = __word_ptr[eax];
				Assert.Equal(new MemoryOperand(Register.EAX, Register.None, 1, 0, 0), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.Word, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}

			{
				var m = __dword_ptr[eax];
				Assert.Equal(new MemoryOperand(Register.EAX, Register.None, 1, 0, 0), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.Dword, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}

			{
				var m = __qword_ptr[eax];
				Assert.Equal(new MemoryOperand(Register.EAX, Register.None, 1, 0, 0), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.Qword, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}

			{
				var m = __xmmword_ptr[eax];
				Assert.Equal(new MemoryOperand(Register.EAX, Register.None, 1, 0, 0), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.Xword, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}

			{
				var m = __ymmword_ptr[eax];
				Assert.Equal(new MemoryOperand(Register.EAX, Register.None, 1, 0, 0), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.Yword, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}

			{
				var m = __zmmword_ptr[eax];
				Assert.Equal(new MemoryOperand(Register.EAX, Register.None, 1, 0, 0), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.Zword, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}

			{
				var m = __[bx + si];
				Assert.Equal(new MemoryOperand(Register.BX, Register.SI, 1, 0, 0), m.ToMemoryOperand(16));
				Assert.Equal(MemoryOperandSize.None, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}

			{
				var m = __[eax + edx];
				Assert.Equal(new MemoryOperand(Register.EAX, Register.EDX, 1, 0, 0), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.None, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}

			{
				var m = __[eax + xmm1];
				Assert.Equal(new MemoryOperand(Register.EAX, Register.XMM1, 1, 0, 0), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.None, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}

			{
				var m = __[eax + ymm1];
				Assert.Equal(new MemoryOperand(Register.EAX, Register.YMM1, 1, 0, 0), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.None, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}

			{
				var m = __[eax + zmm1];
				Assert.Equal(new MemoryOperand(Register.EAX, Register.ZMM1, 1, 0, 0), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.None, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}

			{
				var m = __[rsi + rdi * 2 + 124];
				Assert.Equal(new MemoryOperand(Register.RSI, Register.RDI, 2, 124, 1), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.None, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}

			{
				var m = __[eax + edx * 4 + 8];
				Assert.Equal(new MemoryOperand(Register.EAX, Register.EDX, 4, 8, 1), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.None, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}

			{
				var m = __[rsi + rdi * 2 + 124];
				Assert.Equal(new MemoryOperand(Register.RSI, Register.RDI, 2, 124, 1), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.None, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}

			{
				var m = __dword_bcst[rsi + rdi * 2 + 124];
				Assert.Equal(new MemoryOperand(Register.RSI, Register.RDI, 2, 124, 1, true, Register.None), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.Dword, m.Size);
				Assert.Equal(AssemblerOperandFlags.Broadcast, m.Flags);
			}

			{
				var m = __dword_bcst[rsi + rdi * 2 - 124];
				Assert.Equal(new MemoryOperand(Register.RSI, Register.RDI, 2, -124, 1, true, Register.None), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.Dword, m.Size);
				Assert.Equal(AssemblerOperandFlags.Broadcast, m.Flags);
			}

			{
				var m = __dword_bcst[(rsi + rdi * 2 - 124) + 1];
				Assert.Equal(new MemoryOperand(Register.RSI, Register.RDI, 2, -123, 1, true, Register.None), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.Dword, m.Size);
				Assert.Equal(AssemblerOperandFlags.Broadcast, m.Flags);
			}

			{
				var m = __dword_bcst[(rsi + rdi * 2 - 124) - 1];
				Assert.Equal(new MemoryOperand(Register.RSI, Register.RDI, 2, -125, 1, true, Register.None), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.Dword, m.Size);
				Assert.Equal(AssemblerOperandFlags.Broadcast, m.Flags);
			}

			{
				var m1 = __qword_bcst[rsi + rdi * 2 + 124];
				var m2 = __dword_bcst[rsi + rdi * 2 + 124];
				Assert.NotEqual(m1, m2);
				Assert.Equal(m1.ToMemoryOperand(64), m2.ToMemoryOperand(64));
				Assert.NotEqual(m1.GetHashCode(), m2.GetHashCode());
			}

			{
				var assembler = new Assembler(64);
				var label = assembler.CreateLabel("Check");
				var m = __[label];
				Assert.Equal(new MemoryOperand(Register.RIP, Register.None, 1, 1, 1), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.None, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}
		}
	}
}
#endif
