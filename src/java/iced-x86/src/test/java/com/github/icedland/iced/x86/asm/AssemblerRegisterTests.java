// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.asm;

import static org.junit.jupiter.api.Assertions.*;
import org.junit.jupiter.api.Test;

import com.github.icedland.iced.x86.ICRegister;
import com.github.icedland.iced.x86.ICRegisters;
import com.github.icedland.iced.x86.MemoryOperand;
import static com.github.icedland.iced.x86.asm.AsmRegisters.*;

public class AssemblerRegisterTests {
	@Test
	public void testMemoryOperands() {
		{
			assertFalse(st0.equals(st1));

			assertEquals(ICRegisters.al, al.get());
			assertEquals(ICRegisters.cl, cl.get());
			assertEquals(ICRegisters.dl, dl.get());
			assertEquals(ICRegisters.bl, bl.get());
			assertEquals(ICRegisters.ah, ah.get());
			assertEquals(ICRegisters.ch, ch.get());
			assertEquals(ICRegisters.dh, dh.get());
			assertEquals(ICRegisters.bh, bh.get());
			assertEquals(ICRegisters.spl, spl.get());
			assertEquals(ICRegisters.bpl, bpl.get());
			assertEquals(ICRegisters.sil, sil.get());
			assertEquals(ICRegisters.dil, dil.get());
			assertEquals(ICRegisters.r8b, r8b.get());
			assertEquals(ICRegisters.r9b, r9b.get());
			assertEquals(ICRegisters.r10b, r10b.get());
			assertEquals(ICRegisters.r11b, r11b.get());
			assertEquals(ICRegisters.r12b, r12b.get());
			assertEquals(ICRegisters.r13b, r13b.get());
			assertEquals(ICRegisters.r14b, r14b.get());
			assertEquals(ICRegisters.r15b, r15b.get());
			assertEquals(ICRegisters.ax, ax.get());
			assertEquals(ICRegisters.cx, cx.get());
			assertEquals(ICRegisters.dx, dx.get());
			assertEquals(ICRegisters.bx, bx.get());
			assertEquals(ICRegisters.sp, sp.get());
			assertEquals(ICRegisters.bp, bp.get());
			assertEquals(ICRegisters.si, si.get());
			assertEquals(ICRegisters.di, di.get());
			assertEquals(ICRegisters.r8w, r8w.get());
			assertEquals(ICRegisters.r9w, r9w.get());
			assertEquals(ICRegisters.r10w, r10w.get());
			assertEquals(ICRegisters.r11w, r11w.get());
			assertEquals(ICRegisters.r12w, r12w.get());
			assertEquals(ICRegisters.r13w, r13w.get());
			assertEquals(ICRegisters.r14w, r14w.get());
			assertEquals(ICRegisters.r15w, r15w.get());
			assertEquals(ICRegisters.eax, eax.get());
			assertEquals(ICRegisters.ecx, ecx.get());
			assertEquals(ICRegisters.edx, edx.get());
			assertEquals(ICRegisters.ebx, ebx.get());
			assertEquals(ICRegisters.esp, esp.get());
			assertEquals(ICRegisters.ebp, ebp.get());
			assertEquals(ICRegisters.esi, esi.get());
			assertEquals(ICRegisters.edi, edi.get());
			assertEquals(ICRegisters.r8d, r8d.get());
			assertEquals(ICRegisters.r9d, r9d.get());
			assertEquals(ICRegisters.r10d, r10d.get());
			assertEquals(ICRegisters.r11d, r11d.get());
			assertEquals(ICRegisters.r12d, r12d.get());
			assertEquals(ICRegisters.r13d, r13d.get());
			assertEquals(ICRegisters.r14d, r14d.get());
			assertEquals(ICRegisters.r15d, r15d.get());
			assertEquals(ICRegisters.rax, rax.get());
			assertEquals(ICRegisters.rcx, rcx.get());
			assertEquals(ICRegisters.rdx, rdx.get());
			assertEquals(ICRegisters.rbx, rbx.get());
			assertEquals(ICRegisters.rsp, rsp.get());
			assertEquals(ICRegisters.rbp, rbp.get());
			assertEquals(ICRegisters.rsi, rsi.get());
			assertEquals(ICRegisters.rdi, rdi.get());
			assertEquals(ICRegisters.r8, r8.get());
			assertEquals(ICRegisters.r9, r9.get());
			assertEquals(ICRegisters.r10, r10.get());
			assertEquals(ICRegisters.r11, r11.get());
			assertEquals(ICRegisters.r12, r12.get());
			assertEquals(ICRegisters.r13, r13.get());
			assertEquals(ICRegisters.r14, r14.get());
			assertEquals(ICRegisters.r15, r15.get());
			assertEquals(ICRegisters.es, es.get());
			assertEquals(ICRegisters.cs, cs.get());
			assertEquals(ICRegisters.ss, ss.get());
			assertEquals(ICRegisters.ds, ds.get());
			assertEquals(ICRegisters.fs, fs.get());
			assertEquals(ICRegisters.gs, gs.get());
			assertEquals(ICRegisters.xmm0, xmm0.get());
			assertEquals(ICRegisters.xmm1, xmm1.get());
			assertEquals(ICRegisters.xmm2, xmm2.get());
			assertEquals(ICRegisters.xmm3, xmm3.get());
			assertEquals(ICRegisters.xmm4, xmm4.get());
			assertEquals(ICRegisters.xmm5, xmm5.get());
			assertEquals(ICRegisters.xmm6, xmm6.get());
			assertEquals(ICRegisters.xmm7, xmm7.get());
			assertEquals(ICRegisters.xmm8, xmm8.get());
			assertEquals(ICRegisters.xmm9, xmm9.get());
			assertEquals(ICRegisters.xmm10, xmm10.get());
			assertEquals(ICRegisters.xmm11, xmm11.get());
			assertEquals(ICRegisters.xmm12, xmm12.get());
			assertEquals(ICRegisters.xmm13, xmm13.get());
			assertEquals(ICRegisters.xmm14, xmm14.get());
			assertEquals(ICRegisters.xmm15, xmm15.get());
			assertEquals(ICRegisters.xmm16, xmm16.get());
			assertEquals(ICRegisters.xmm17, xmm17.get());
			assertEquals(ICRegisters.xmm18, xmm18.get());
			assertEquals(ICRegisters.xmm19, xmm19.get());
			assertEquals(ICRegisters.xmm20, xmm20.get());
			assertEquals(ICRegisters.xmm21, xmm21.get());
			assertEquals(ICRegisters.xmm22, xmm22.get());
			assertEquals(ICRegisters.xmm23, xmm23.get());
			assertEquals(ICRegisters.xmm24, xmm24.get());
			assertEquals(ICRegisters.xmm25, xmm25.get());
			assertEquals(ICRegisters.xmm26, xmm26.get());
			assertEquals(ICRegisters.xmm27, xmm27.get());
			assertEquals(ICRegisters.xmm28, xmm28.get());
			assertEquals(ICRegisters.xmm29, xmm29.get());
			assertEquals(ICRegisters.xmm30, xmm30.get());
			assertEquals(ICRegisters.xmm31, xmm31.get());
			assertEquals(ICRegisters.ymm0, ymm0.get());
			assertEquals(ICRegisters.ymm1, ymm1.get());
			assertEquals(ICRegisters.ymm2, ymm2.get());
			assertEquals(ICRegisters.ymm3, ymm3.get());
			assertEquals(ICRegisters.ymm4, ymm4.get());
			assertEquals(ICRegisters.ymm5, ymm5.get());
			assertEquals(ICRegisters.ymm6, ymm6.get());
			assertEquals(ICRegisters.ymm7, ymm7.get());
			assertEquals(ICRegisters.ymm8, ymm8.get());
			assertEquals(ICRegisters.ymm9, ymm9.get());
			assertEquals(ICRegisters.ymm10, ymm10.get());
			assertEquals(ICRegisters.ymm11, ymm11.get());
			assertEquals(ICRegisters.ymm12, ymm12.get());
			assertEquals(ICRegisters.ymm13, ymm13.get());
			assertEquals(ICRegisters.ymm14, ymm14.get());
			assertEquals(ICRegisters.ymm15, ymm15.get());
			assertEquals(ICRegisters.ymm16, ymm16.get());
			assertEquals(ICRegisters.ymm17, ymm17.get());
			assertEquals(ICRegisters.ymm18, ymm18.get());
			assertEquals(ICRegisters.ymm19, ymm19.get());
			assertEquals(ICRegisters.ymm20, ymm20.get());
			assertEquals(ICRegisters.ymm21, ymm21.get());
			assertEquals(ICRegisters.ymm22, ymm22.get());
			assertEquals(ICRegisters.ymm23, ymm23.get());
			assertEquals(ICRegisters.ymm24, ymm24.get());
			assertEquals(ICRegisters.ymm25, ymm25.get());
			assertEquals(ICRegisters.ymm26, ymm26.get());
			assertEquals(ICRegisters.ymm27, ymm27.get());
			assertEquals(ICRegisters.ymm28, ymm28.get());
			assertEquals(ICRegisters.ymm29, ymm29.get());
			assertEquals(ICRegisters.ymm30, ymm30.get());
			assertEquals(ICRegisters.ymm31, ymm31.get());
			assertEquals(ICRegisters.zmm0, zmm0.get());
			assertEquals(ICRegisters.zmm1, zmm1.get());
			assertEquals(ICRegisters.zmm2, zmm2.get());
			assertEquals(ICRegisters.zmm3, zmm3.get());
			assertEquals(ICRegisters.zmm4, zmm4.get());
			assertEquals(ICRegisters.zmm5, zmm5.get());
			assertEquals(ICRegisters.zmm6, zmm6.get());
			assertEquals(ICRegisters.zmm7, zmm7.get());
			assertEquals(ICRegisters.zmm8, zmm8.get());
			assertEquals(ICRegisters.zmm9, zmm9.get());
			assertEquals(ICRegisters.zmm10, zmm10.get());
			assertEquals(ICRegisters.zmm11, zmm11.get());
			assertEquals(ICRegisters.zmm12, zmm12.get());
			assertEquals(ICRegisters.zmm13, zmm13.get());
			assertEquals(ICRegisters.zmm14, zmm14.get());
			assertEquals(ICRegisters.zmm15, zmm15.get());
			assertEquals(ICRegisters.zmm16, zmm16.get());
			assertEquals(ICRegisters.zmm17, zmm17.get());
			assertEquals(ICRegisters.zmm18, zmm18.get());
			assertEquals(ICRegisters.zmm19, zmm19.get());
			assertEquals(ICRegisters.zmm20, zmm20.get());
			assertEquals(ICRegisters.zmm21, zmm21.get());
			assertEquals(ICRegisters.zmm22, zmm22.get());
			assertEquals(ICRegisters.zmm23, zmm23.get());
			assertEquals(ICRegisters.zmm24, zmm24.get());
			assertEquals(ICRegisters.zmm25, zmm25.get());
			assertEquals(ICRegisters.zmm26, zmm26.get());
			assertEquals(ICRegisters.zmm27, zmm27.get());
			assertEquals(ICRegisters.zmm28, zmm28.get());
			assertEquals(ICRegisters.zmm29, zmm29.get());
			assertEquals(ICRegisters.zmm30, zmm30.get());
			assertEquals(ICRegisters.zmm31, zmm31.get());
			assertEquals(ICRegisters.k0, k0.get());
			assertEquals(ICRegisters.k1, k1.get());
			assertEquals(ICRegisters.k2, k2.get());
			assertEquals(ICRegisters.k3, k3.get());
			assertEquals(ICRegisters.k4, k4.get());
			assertEquals(ICRegisters.k5, k5.get());
			assertEquals(ICRegisters.k6, k6.get());
			assertEquals(ICRegisters.k7, k7.get());
			assertEquals(ICRegisters.bnd0, bnd0.get());
			assertEquals(ICRegisters.bnd1, bnd1.get());
			assertEquals(ICRegisters.bnd2, bnd2.get());
			assertEquals(ICRegisters.bnd3, bnd3.get());
			assertEquals(ICRegisters.cr0, cr0.get());
			assertEquals(ICRegisters.cr1, cr1.get());
			assertEquals(ICRegisters.cr2, cr2.get());
			assertEquals(ICRegisters.cr3, cr3.get());
			assertEquals(ICRegisters.cr4, cr4.get());
			assertEquals(ICRegisters.cr5, cr5.get());
			assertEquals(ICRegisters.cr6, cr6.get());
			assertEquals(ICRegisters.cr7, cr7.get());
			assertEquals(ICRegisters.cr8, cr8.get());
			assertEquals(ICRegisters.cr9, cr9.get());
			assertEquals(ICRegisters.cr10, cr10.get());
			assertEquals(ICRegisters.cr11, cr11.get());
			assertEquals(ICRegisters.cr12, cr12.get());
			assertEquals(ICRegisters.cr13, cr13.get());
			assertEquals(ICRegisters.cr14, cr14.get());
			assertEquals(ICRegisters.cr15, cr15.get());
			assertEquals(ICRegisters.dr0, dr0.get());
			assertEquals(ICRegisters.dr1, dr1.get());
			assertEquals(ICRegisters.dr2, dr2.get());
			assertEquals(ICRegisters.dr3, dr3.get());
			assertEquals(ICRegisters.dr4, dr4.get());
			assertEquals(ICRegisters.dr5, dr5.get());
			assertEquals(ICRegisters.dr6, dr6.get());
			assertEquals(ICRegisters.dr7, dr7.get());
			assertEquals(ICRegisters.dr8, dr8.get());
			assertEquals(ICRegisters.dr9, dr9.get());
			assertEquals(ICRegisters.dr10, dr10.get());
			assertEquals(ICRegisters.dr11, dr11.get());
			assertEquals(ICRegisters.dr12, dr12.get());
			assertEquals(ICRegisters.dr13, dr13.get());
			assertEquals(ICRegisters.dr14, dr14.get());
			assertEquals(ICRegisters.dr15, dr15.get());
			assertEquals(ICRegisters.st0, st0.get());
			assertEquals(ICRegisters.st1, st1.get());
			assertEquals(ICRegisters.st2, st2.get());
			assertEquals(ICRegisters.st3, st3.get());
			assertEquals(ICRegisters.st4, st4.get());
			assertEquals(ICRegisters.st5, st5.get());
			assertEquals(ICRegisters.st6, st6.get());
			assertEquals(ICRegisters.st7, st7.get());
			assertEquals(ICRegisters.mm0, mm0.get());
			assertEquals(ICRegisters.mm1, mm1.get());
			assertEquals(ICRegisters.mm2, mm2.get());
			assertEquals(ICRegisters.mm3, mm3.get());
			assertEquals(ICRegisters.mm4, mm4.get());
			assertEquals(ICRegisters.mm5, mm5.get());
			assertEquals(ICRegisters.mm6, mm6.get());
			assertEquals(ICRegisters.mm7, mm7.get());
			assertEquals(ICRegisters.tr0, tr0.get());
			assertEquals(ICRegisters.tr1, tr1.get());
			assertEquals(ICRegisters.tr2, tr2.get());
			assertEquals(ICRegisters.tr3, tr3.get());
			assertEquals(ICRegisters.tr4, tr4.get());
			assertEquals(ICRegisters.tr5, tr5.get());
			assertEquals(ICRegisters.tr6, tr6.get());
			assertEquals(ICRegisters.tr7, tr7.get());
		}

		{
			assertEquals(AsmOperandFlags.K1, zmm0.k1().flags);
			assertEquals(AsmOperandFlags.K2 | AsmOperandFlags.ZEROING, zmm0.k2().z().flags);
			assertEquals(AsmOperandFlags.SUPPRESS_ALL_EXCEPTIONS, zmm0.sae().flags);
			assertEquals(AsmOperandFlags.ROUND_TO_NEAREST, zmm0.rn_sae().flags);
			assertEquals(AsmOperandFlags.ROUND_DOWN, zmm0.rd_sae().flags);
			assertEquals(AsmOperandFlags.ROUND_UP, zmm0.ru_sae().flags);
			assertEquals(AsmOperandFlags.ROUND_TOWARD_ZERO, zmm0.rz_sae().flags);

			assertFalse(zmm0.equals(zmm1));
			assertFalse(zmm0.equals(zmm0.k1()));
			assertFalse(zmm0.hashCode() == zmm1.hashCode());
			assertFalse(zmm0.hashCode() == zmm0.k1().hashCode());
		}

		{
			AsmMemoryOperand m = mem_ptr(13).cs();
			assertEquals(new MemoryOperand(ICRegister.NONE, ICRegister.NONE, 1, 13, 8, false, ICRegisters.cs), m.toMemoryOperand(64));
			assertEquals(MemoryOperandSize.NONE, m.size);
			assertEquals(AsmOperandFlags.NONE, m.flags);
		}
		{
			AsmMemoryOperand m = mem_ptr(13).ds();
			assertEquals(new MemoryOperand(ICRegister.NONE, ICRegister.NONE, 1, 13, 8, false, ICRegisters.ds), m.toMemoryOperand(64));
			assertEquals(MemoryOperandSize.NONE, m.size);
			assertEquals(AsmOperandFlags.NONE, m.flags);
		}
		{
			AsmMemoryOperand m = mem_ptr(13).es();
			assertEquals(new MemoryOperand(ICRegister.NONE, ICRegister.NONE, 1, 13, 8, false, ICRegisters.es), m.toMemoryOperand(64));
			assertEquals(MemoryOperandSize.NONE, m.size);
			assertEquals(AsmOperandFlags.NONE, m.flags);
		}
		{
			AsmMemoryOperand m = mem_ptr(13).fs();
			assertEquals(new MemoryOperand(ICRegister.NONE, ICRegister.NONE, 1, 13, 8, false, ICRegisters.fs), m.toMemoryOperand(64));
			assertEquals(MemoryOperandSize.NONE, m.size);
			assertEquals(AsmOperandFlags.NONE, m.flags);
		}
		{
			AsmMemoryOperand m = mem_ptr(13).gs();
			assertEquals(new MemoryOperand(ICRegister.NONE, ICRegister.NONE, 1, 13, 8, false, ICRegisters.gs), m.toMemoryOperand(64));
			assertEquals(MemoryOperandSize.NONE, m.size);
			assertEquals(AsmOperandFlags.NONE, m.flags);
		}
		{
			AsmMemoryOperand m = mem_ptr(13).ss();
			assertEquals(new MemoryOperand(ICRegister.NONE, ICRegister.NONE, 1, 13, 8, false, ICRegisters.ss), m.toMemoryOperand(64));
			assertEquals(MemoryOperandSize.NONE, m.size);
			assertEquals(AsmOperandFlags.NONE, m.flags);
		}
		{
			AsmMemoryOperand m = mem_ptr(-0x80);
			assertEquals(new MemoryOperand(ICRegister.NONE, ICRegister.NONE, 1, -0x80, 8), m.toMemoryOperand(64));
			assertEquals(MemoryOperandSize.NONE, m.size);
			assertEquals(AsmOperandFlags.NONE, m.flags);
		}

		{
			AsmMemoryOperand m = mem_ptr(0x7F);
			assertEquals(new MemoryOperand(ICRegister.NONE, ICRegister.NONE, 1, 0x7F, 8), m.toMemoryOperand(64));
			assertEquals(MemoryOperandSize.NONE, m.size);
			assertEquals(AsmOperandFlags.NONE, m.flags);
		}

		{
			AsmMemoryOperand m = mem_ptr(-0x8000);
			assertEquals(new MemoryOperand(ICRegister.NONE, ICRegister.NONE, 1, -0x8000, 8), m.toMemoryOperand(64));
			assertEquals(MemoryOperandSize.NONE, m.size);
			assertEquals(AsmOperandFlags.NONE, m.flags);
		}

		{
			AsmMemoryOperand m = mem_ptr(0x7FFF);
			assertEquals(new MemoryOperand(ICRegister.NONE, ICRegister.NONE, 1, 0x7FFF, 8), m.toMemoryOperand(64));
			assertEquals(MemoryOperandSize.NONE, m.size);
			assertEquals(AsmOperandFlags.NONE, m.flags);
		}

		{
			AsmMemoryOperand m = mem_ptr(-0x8000_0000);
			assertEquals(new MemoryOperand(ICRegister.NONE, ICRegister.NONE, 1, -0x8000_0000, 8), m.toMemoryOperand(64));
			assertEquals(MemoryOperandSize.NONE, m.size);
			assertEquals(AsmOperandFlags.NONE, m.flags);
		}

		{
			AsmMemoryOperand m = mem_ptr(0x7FFF_FFFF);
			assertEquals(new MemoryOperand(ICRegister.NONE, ICRegister.NONE, 1, 0x7FFF_FFFF, 8), m.toMemoryOperand(64));
			assertEquals(MemoryOperandSize.NONE, m.size);
			assertEquals(AsmOperandFlags.NONE, m.flags);
		}

		{
			AsmMemoryOperand m = mem_ptr(0x8000_0000L);
			assertEquals(new MemoryOperand(ICRegister.NONE, ICRegister.NONE, 1, 0x8000_0000L, 8), m.toMemoryOperand(64));
			assertEquals(MemoryOperandSize.NONE, m.size);
			assertEquals(AsmOperandFlags.NONE, m.flags);
		}

		{
			AsmMemoryOperand m = mem_ptr(0xFFFF_FFFF_FFFF_FFFFL);
			assertEquals(new MemoryOperand(ICRegister.NONE, ICRegister.NONE, 1, 0xFFFF_FFFF_FFFF_FFFFL, 8), m.toMemoryOperand(64));
			assertEquals(0xFFFF_FFFF_FFFF_FFFFL, m.displacement);
			assertEquals(MemoryOperandSize.NONE, m.size);
			assertEquals(AsmOperandFlags.NONE, m.flags);
		}
		{
			AsmMemoryOperand m = mem_ptr(eax);
			assertEquals(new MemoryOperand(ICRegisters.eax, ICRegister.NONE, 1, 0, 0), m.toMemoryOperand(64));
			assertEquals(MemoryOperandSize.NONE, m.size);
			assertEquals(AsmOperandFlags.NONE, m.flags);
		}

		{
			AsmMemoryOperand m = mem_ptr(eax, 1);
			assertEquals(new MemoryOperand(ICRegisters.eax, ICRegister.NONE, 1, 1, 1), m.toMemoryOperand(64));
			assertEquals(MemoryOperandSize.NONE, m.size);
			assertEquals(AsmOperandFlags.NONE, m.flags);
		}

		{
			AsmMemoryOperand m = mem_ptr(eax, -1);
			assertEquals(new MemoryOperand(ICRegisters.eax, ICRegister.NONE, 1, -1, 1), m.toMemoryOperand(64));
			assertEquals(MemoryOperandSize.NONE, m.size);
			assertEquals(AsmOperandFlags.NONE, m.flags);
		}

		{
			AsmMemoryOperand m = word_ptr(eax);
			assertEquals(new MemoryOperand(ICRegisters.eax, ICRegister.NONE, 1, 0, 0), m.toMemoryOperand(64));
			assertEquals(MemoryOperandSize.WORD, m.size);
			assertEquals(AsmOperandFlags.NONE, m.flags);
		}

		{
			AsmMemoryOperand m = dword_ptr(eax);
			assertEquals(new MemoryOperand(ICRegisters.eax, ICRegister.NONE, 1, 0, 0), m.toMemoryOperand(64));
			assertEquals(MemoryOperandSize.DWORD, m.size);
			assertEquals(AsmOperandFlags.NONE, m.flags);
		}

		{
			AsmMemoryOperand m = qword_ptr(eax);
			assertEquals(new MemoryOperand(ICRegisters.eax, ICRegister.NONE, 1, 0, 0), m.toMemoryOperand(64));
			assertEquals(MemoryOperandSize.QWORD, m.size);
			assertEquals(AsmOperandFlags.NONE, m.flags);
		}

		{
			AsmMemoryOperand m = xmmword_ptr(eax);
			assertEquals(new MemoryOperand(ICRegisters.eax, ICRegister.NONE, 1, 0, 0), m.toMemoryOperand(64));
			assertEquals(MemoryOperandSize.XWORD, m.size);
			assertEquals(AsmOperandFlags.NONE, m.flags);
		}

		{
			AsmMemoryOperand m = ymmword_ptr(eax);
			assertEquals(new MemoryOperand(ICRegisters.eax, ICRegister.NONE, 1, 0, 0), m.toMemoryOperand(64));
			assertEquals(MemoryOperandSize.YWORD, m.size);
			assertEquals(AsmOperandFlags.NONE, m.flags);
		}

		{
			AsmMemoryOperand m = zmmword_ptr(eax);
			assertEquals(new MemoryOperand(ICRegisters.eax, ICRegister.NONE, 1, 0, 0), m.toMemoryOperand(64));
			assertEquals(MemoryOperandSize.ZWORD, m.size);
			assertEquals(AsmOperandFlags.NONE, m.flags);
		}

		{
			AsmMemoryOperand m = mem_ptr(bx, si);
			assertEquals(new MemoryOperand(ICRegisters.bx, ICRegisters.si, 1, 0, 0), m.toMemoryOperand(16));
			assertEquals(MemoryOperandSize.NONE, m.size);
			assertEquals(AsmOperandFlags.NONE, m.flags);
		}

		{
			AsmMemoryOperand m = mem_ptr(eax, edx);
			assertEquals(new MemoryOperand(ICRegisters.eax, ICRegisters.edx, 1, 0, 0), m.toMemoryOperand(64));
			assertEquals(MemoryOperandSize.NONE, m.size);
			assertEquals(AsmOperandFlags.NONE, m.flags);
		}

		{
			AsmMemoryOperand m = mem_ptr(eax, xmm1);
			assertEquals(new MemoryOperand(ICRegisters.eax, ICRegisters.xmm1, 1, 0, 0), m.toMemoryOperand(64));
			assertEquals(MemoryOperandSize.NONE, m.size);
			assertEquals(AsmOperandFlags.NONE, m.flags);
		}

		{
			AsmMemoryOperand m = mem_ptr(eax, ymm1);
			assertEquals(new MemoryOperand(ICRegisters.eax, ICRegisters.ymm1, 1, 0, 0), m.toMemoryOperand(64));
			assertEquals(MemoryOperandSize.NONE, m.size);
			assertEquals(AsmOperandFlags.NONE, m.flags);
		}

		{
			AsmMemoryOperand m = mem_ptr(eax, zmm1);
			assertEquals(new MemoryOperand(ICRegisters.eax, ICRegisters.zmm1, 1, 0, 0), m.toMemoryOperand(64));
			assertEquals(MemoryOperandSize.NONE, m.size);
			assertEquals(AsmOperandFlags.NONE, m.flags);
		}

		{
			AsmMemoryOperand m = mem_ptr(rsi, rdi, 2, 124);
			assertEquals(new MemoryOperand(ICRegisters.rsi, ICRegisters.rdi, 2, 124, 1), m.toMemoryOperand(64));
			assertEquals(MemoryOperandSize.NONE, m.size);
			assertEquals(AsmOperandFlags.NONE, m.flags);
		}

		{
			AsmMemoryOperand m = mem_ptr(eax, edx, 4, 8);
			assertEquals(new MemoryOperand(ICRegisters.eax, ICRegisters.edx, 4, 8, 1), m.toMemoryOperand(64));
			assertEquals(MemoryOperandSize.NONE, m.size);
			assertEquals(AsmOperandFlags.NONE, m.flags);
		}

		{
			AsmMemoryOperand m = mem_ptr(rsi, rdi, 2, 124);
			assertEquals(new MemoryOperand(ICRegisters.rsi, ICRegisters.rdi, 2, 124, 1), m.toMemoryOperand(64));
			assertEquals(MemoryOperandSize.NONE, m.size);
			assertEquals(AsmOperandFlags.NONE, m.flags);
		}

		{
			AsmMemoryOperand m = dword_bcst(rsi, rdi, 2, 124);
			assertEquals(new MemoryOperand(ICRegisters.rsi, ICRegisters.rdi, 2, 124, 1, true, ICRegister.NONE), m.toMemoryOperand(64));
			assertEquals(MemoryOperandSize.DWORD, m.size);
			assertEquals(AsmOperandFlags.BROADCAST, m.flags);
		}

		{
			AsmMemoryOperand m = dword_bcst(rsi, rdi, 2, -124);
			assertEquals(new MemoryOperand(ICRegisters.rsi, ICRegisters.rdi, 2, -124, 1, true, ICRegister.NONE), m.toMemoryOperand(64));
			assertEquals(MemoryOperandSize.DWORD, m.size);
			assertEquals(AsmOperandFlags.BROADCAST, m.flags);
		}

		{
			AsmMemoryOperand m = dword_bcst(rsi, rdi, 2, -123);
			assertEquals(new MemoryOperand(ICRegisters.rsi, ICRegisters.rdi, 2, -123, 1, true, ICRegister.NONE), m.toMemoryOperand(64));
			assertEquals(MemoryOperandSize.DWORD, m.size);
			assertEquals(AsmOperandFlags.BROADCAST, m.flags);
		}

		{
			AsmMemoryOperand m = dword_bcst(rsi, rdi, 2, -125);
			assertEquals(new MemoryOperand(ICRegisters.rsi, ICRegisters.rdi, 2, -125, 1, true, ICRegister.NONE), m.toMemoryOperand(64));
			assertEquals(MemoryOperandSize.DWORD, m.size);
			assertEquals(AsmOperandFlags.BROADCAST, m.flags);
		}

		{
			AsmMemoryOperand m1 = qword_bcst(rsi, rdi, 2, 124);
			AsmMemoryOperand m2 = dword_bcst(rsi, rdi, 2, 124);
			assertFalse(m1.equals(m2));
			assertEquals(m1.toMemoryOperand(64), m2.toMemoryOperand(64));
			assertFalse(m1.hashCode() == m2.hashCode());
		}

		{
			CodeAssembler assembler = new CodeAssembler(64);
			CodeLabel label = assembler.createLabel("Check");
			AsmMemoryOperand m = mem_ptr(label);
			assertEquals(new MemoryOperand(ICRegisters.rip, ICRegister.NONE, 1, 1, 1), m.toMemoryOperand(64));
			assertEquals(MemoryOperandSize.NONE, m.size);
			assertEquals(AsmOperandFlags.NONE, m.flags);
		}
	}
}
