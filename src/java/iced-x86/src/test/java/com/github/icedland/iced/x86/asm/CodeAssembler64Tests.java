// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.asm;

import static org.junit.jupiter.api.Assertions.*;
import org.junit.jupiter.api.Test;

import static com.github.icedland.iced.x86.asm.AsmRegisters.*;
import com.github.icedland.iced.x86.BitnessUtils;
import com.github.icedland.iced.x86.Code;
import com.github.icedland.iced.x86.CodeWriterImpl;
import com.github.icedland.iced.x86.ICRegisters;
import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.MemoryOperand;
import com.github.icedland.iced.x86.Register;
import com.github.icedland.iced.x86.RoundingControl;
import com.github.icedland.iced.x86.enc.BlockEncoderOptions;

// Make sure it can be derived
final class MyAssembler extends CodeAssembler {
	public MyAssembler() {
		super(64);
	}
}

final class CodeAssembler64Tests extends CodeAssemblerTestsBase {
	private CodeAssembler64Tests() {
		super(64);
	}

	@Test
	void xlatb() {
		testAssembler(c -> c.xlatb(), Instruction.create(Code.XLAT_M8, new MemoryOperand(ICRegisters.rbx, ICRegisters.al)));
	}

	@Test
	public void xbegin_label() {
		testAssembler(c -> c.xbegin(createAndEmitLabel(c)), assignLabel(Instruction.createXbegin(getBitness(), FIRST_LABEL_ID), FIRST_LABEL_ID),
				TestInstrFlags.BRANCH);
	}

	@Test
	public void xbegin_offset() {
		testAssembler(c -> c.xbegin(12752), Instruction.createXbegin(getBitness(), 12752), TestInstrFlags.BRANCH_U64 | TestInstrFlags.IGNORE_CODE);
	}

	@Test
	void Ctor() {
		CodeAssembler c = new CodeAssembler(getBitness());
		assertEquals(getBitness(), c.getBitness());
		assertTrue(c.getPreferVex());
		assertTrue(c.getPreferShortBranch());
		assertEquals(0, c.getInstructions().size());
	}

	@Test
	void Reset_works() {
		CodeAssembler c = new CodeAssembler(getBitness());
		c.createLabel();
		c.add(rax, rcx);
		c.lock();
		c.setPreferVex(false);
		c.setPreferShortBranch(false);
		c.reset();
		assertFalse(c.getPreferVex());
		assertFalse(c.getPreferShortBranch());
		assertEquals(0, c.getInstructions().size());
		CodeWriterImpl writer = new CodeWriterImpl();
		Object result = c.assemble(writer, 0);
		assertInstanceOf(CodeAssemblerResult.class, result);
		assertArrayEquals(new byte[0], writer.toArray());
	}

	@Test
	void Invalid_bitness_throws() {
		for (int bitness : BitnessUtils.getInvalidBitnessValues())
			assertThrows(IllegalArgumentException.class, () -> new CodeAssembler(bitness));
	}

	@Test
	void Assemble_throws_if_null_writer() {
		CodeAssembler c = new CodeAssembler(getBitness());
		c.nop();
		assertThrows(NullPointerException.class, () -> c.assemble(null, 0));
	}

	@Test
	void Assemble_fails_if_error() {
		CodeAssembler c = new CodeAssembler(64);
		c.aaa();
		Object result = c.assemble(new CodeWriterImpl(), 0);
		assertInstanceOf(String.class, result);
	}

	@Test
	void TryAssemble_returns_error_string_if_it_failed() {
		CodeAssembler c = new CodeAssembler(64);
		c.aaa();
		Object result = c.assemble(new CodeWriterImpl(), 0);
		assertInstanceOf(String.class, result);
	}

	@Test
	void Test_opmask_registers() {
		testAssembler(c -> c.vmovups(zmm0.k1(), zmm1),
				applyK(Instruction.create(Code.EVEX_VMOVUPS_ZMM_K1Z_ZMMM512, ICRegisters.zmm0, ICRegisters.zmm1), Register.K1),
				TestInstrFlags.PREFER_EVEX);
		testAssembler(c -> c.vmovups(zmm0.k2(), zmm1),
				applyK(Instruction.create(Code.EVEX_VMOVUPS_ZMM_K1Z_ZMMM512, ICRegisters.zmm0, ICRegisters.zmm1), Register.K2),
				TestInstrFlags.PREFER_EVEX);
		testAssembler(c -> c.vmovups(zmm0.k3(), zmm1),
				applyK(Instruction.create(Code.EVEX_VMOVUPS_ZMM_K1Z_ZMMM512, ICRegisters.zmm0, ICRegisters.zmm1), Register.K3),
				TestInstrFlags.PREFER_EVEX);
		testAssembler(c -> c.vmovups(zmm0.k4(), zmm1),
				applyK(Instruction.create(Code.EVEX_VMOVUPS_ZMM_K1Z_ZMMM512, ICRegisters.zmm0, ICRegisters.zmm1), Register.K4),
				TestInstrFlags.PREFER_EVEX);
		testAssembler(c -> c.vmovups(zmm0.k5(), zmm1),
				applyK(Instruction.create(Code.EVEX_VMOVUPS_ZMM_K1Z_ZMMM512, ICRegisters.zmm0, ICRegisters.zmm1), Register.K5),
				TestInstrFlags.PREFER_EVEX);
		testAssembler(c -> c.vmovups(zmm0.k6(), zmm1),
				applyK(Instruction.create(Code.EVEX_VMOVUPS_ZMM_K1Z_ZMMM512, ICRegisters.zmm0, ICRegisters.zmm1), Register.K6),
				TestInstrFlags.PREFER_EVEX);
		testAssembler(c -> c.vmovups(zmm0.k7(), zmm1),
				applyK(Instruction.create(Code.EVEX_VMOVUPS_ZMM_K1Z_ZMMM512, ICRegisters.zmm0, ICRegisters.zmm1), Register.K7),
				TestInstrFlags.PREFER_EVEX);
	}

	@Test
	public void TestDeclareData_db_array() {
		testAssemblerDeclareByte(c -> c.db(new byte[0]), new byte[0]);
		testAssemblerDeclareByte(c -> c.db(new byte[] { 1 }), new byte[] { 1 });
		testAssemblerDeclareByte(c -> c.db(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }), new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 });
		testAssemblerDeclareByte(c -> c.db(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 }), new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 });
		testAssemblerDeclareByte(c -> c.db(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 }), new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 });
		testAssemblerDeclareByte(c -> c.db(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33 }), new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33 });
	}

	@Test
	public void TestDeclareData_db_array_index_length() {
		testAssemblerDeclareByte(c -> c.db(new byte[] { 97, 98, 1, 99, 100, 101 }, 2, 1), new byte[] { 1 });
		testAssemblerDeclareByte(c -> c.db(new byte[] { 97, 98, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 99, 100, 101 }, 2, 16), new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 });
		testAssemblerDeclareByte(c -> c.db(new byte[] { 97, 98, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 99, 100, 101 }, 2, 17), new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 });
		testAssemblerDeclareByte(c -> c.db(new byte[] { 97, 98, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 99, 100, 101 }, 2, 32), new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 });
		testAssemblerDeclareByte(c -> c.db(new byte[] { 97, 98, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 99, 100, 101 }, 2, 33), new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33 });
	}

	@Test
	public void TestDeclareData_db_array_throws_if_null_array() {
		CodeAssembler assembler = new CodeAssembler(getBitness());
		assertThrows(NullPointerException.class, () -> assembler.db(null));
	}

	@Test
	public void TestDeclareData_db_array_index_length_throws() {
		CodeAssembler assembler = new CodeAssembler(getBitness());
		assertThrows(NullPointerException.class, () -> assembler.db(null, 0, 0));
		assertThrows(NullPointerException.class, () -> assembler.db(null, 1, 2));
		assertThrows(IllegalArgumentException.class, () -> assembler.db(new byte[] {}, 1, 0));
		assertThrows(IllegalArgumentException.class, () -> assembler.db(new byte[] {}, 0, 1));
		assertThrows(IllegalArgumentException.class, () -> assembler.db(new byte[] { 1, 2, 3 }, -1, 0));
		assertThrows(IllegalArgumentException.class, () -> assembler.db(new byte[] { 1, 2, 3 }, 0, -1));
		assertThrows(IllegalArgumentException.class, () -> assembler.db(new byte[] { 1, 2, 3 }, -1, -1));
		assertThrows(IllegalArgumentException.class, () -> assembler.db(new byte[] { 1, 2, 3 }, 0, 4));
		assertThrows(IllegalArgumentException.class, () -> assembler.db(new byte[] { 1, 2, 3 }, 1, 3));
		assertThrows(IllegalArgumentException.class, () -> assembler.db(new byte[] { 1, 2, 3 }, 3, 1));
		assertThrows(IllegalArgumentException.class, () -> assembler.db(new byte[] { 1, 2, 3 }, 4, 0));
		assertThrows(IllegalArgumentException.class, () -> assembler.db(new byte[] { 1, 2, 3 }, 4, -1));
	}

	@Test
	public void TestManualInvalid() {
		// pop_regSegment
		assertInvalid(() -> {
			testAssembler(c -> c.pop(cs), new Instruction());
		});
	}

	@Test
	public void TestInvalidStateAssembler() {
		{
			CodeAssembler assembler = new CodeAssembler(getBitness());
			CodeWriterImpl writer = new CodeWriterImpl();
			Object result = assembler.rep().assemble(writer, 0);
			assertInstanceOf(String.class, result);
			String msg = (String)result;
			assertTrue(msg.contains("Unused prefixes"));
		}
		{
			CodeAssembler assembler = new CodeAssembler(getBitness());
			CodeLabel label = assembler.createLabel("BadLabel");
			assembler.label(label);
			CodeWriterImpl writer = new CodeWriterImpl();
			Object result = assembler.assemble(writer, 0);
			assertInstanceOf(String.class, result);
			String msg = (String)result;
			assertTrue(msg.contains("Unused label"));
		}
	}

	@Test
	public void TestLabelRIP() {
		{
			CodeAssembler c = new CodeAssembler(getBitness());
			CodeLabel label0 = c.createLabel();
			CodeLabel label1 = c.createLabel();
			c.nop();
			c.nop();
			c.nop();
			c.label(label1);
			c.nop();

			CodeWriterImpl writer = new CodeWriterImpl();
			Object result = c.assemble(writer, 0x100, BlockEncoderOptions.RETURN_NEW_INSTRUCTION_OFFSETS);
			assertInstanceOf(CodeAssemblerResult.class, result);
			CodeAssemblerResult asmRes = (CodeAssemblerResult)result;
			long label1RIP = asmRes.getLabelRIP(label1);
			assertEquals(0x103L, label1RIP);
			assertThrows(IllegalArgumentException.class, () -> asmRes.getLabelRIP(label1, 1));
			assertThrows(IllegalArgumentException.class, () -> asmRes.getLabelRIP(label0));
		}
		{
			CodeAssembler c = new CodeAssembler(getBitness());
			CodeLabel label1 = c.createLabel();
			c.nop();
			c.label(label1);
			c.nop();

			// Cannot use a label already emitted
			assertThrows(IllegalArgumentException.class, () -> c.label(label1));

			CodeWriterImpl writer = new CodeWriterImpl();
			Object result = c.assemble(writer, 0);
			assertInstanceOf(CodeAssemblerResult.class, result);
			CodeAssemblerResult asmRes = (CodeAssemblerResult)result;
			// Will throw without BlockEncoderOptions.RETURN_NEW_INSTRUCTION_OFFSETS
			assertThrows(IllegalArgumentException.class, () -> asmRes.getLabelRIP(label1));
		}
	}

	@Test
	public void TestInstructionPrefixes() {
		{
			Instruction inst = Instruction.createStosd(getBitness());
			inst.setRepPrefix(true);
			testAssembler(c -> c.rep().stosd(), inst);
		}

		{
			Instruction inst = Instruction.createStosd(getBitness());
			inst.setRepePrefix(true);
			testAssembler(c -> c.repe().stosd(), inst);
		}
		{
			Instruction inst = Instruction.createStosd(getBitness());
			inst.setRepePrefix(true);
			testAssembler(c -> c.repz().stosd(), inst);
		}

		{
			Instruction inst = Instruction.createStosd(getBitness());
			inst.setRepnePrefix(true);
			testAssembler(c -> c.repne().stosd(), inst);
		}
		{
			Instruction inst = Instruction.createStosd(getBitness());
			inst.setRepnePrefix(true);
			testAssembler(c -> c.repnz().stosd(), inst);
		}

		{
			Instruction inst = Instruction.create(Code.XCHG_RM64_R64, mem_ptr(rdx).toMemoryOperand(64), ICRegisters.rax);
			inst.setXacquirePrefix(true);
			testAssembler(c -> c.xacquire().xchg(mem_ptr(rdx), rax), inst);
		}

		{
			Instruction inst = Instruction.create(Code.XCHG_RM64_R64, mem_ptr(rdx).toMemoryOperand(64), ICRegisters.rax);
			inst.setLockPrefix(true);
			testAssembler(c -> c.lock().xchg(mem_ptr(rdx), rax), inst);
		}

		{
			Instruction inst = Instruction.create(Code.XCHG_RM64_R64, mem_ptr(rdx).toMemoryOperand(64), ICRegisters.rax);
			inst.setXreleasePrefix(true);
			testAssembler(c -> c.xrelease().xchg(mem_ptr(rdx), rax), inst);
		}

		{
			Instruction inst = Instruction.create(Code.CALL_RM64, mem_ptr(rax).toMemoryOperand(64));
			inst.setSegmentPrefix(Register.DS);
			testAssembler(c -> c.notrack().call(qword_ptr(rax)), inst);
		}

		{
			Instruction inst = Instruction.create(Code.CALL_RM64, mem_ptr(rax).toMemoryOperand(64));
			inst.setSegmentPrefix(Register.DS);
			inst.setRepnePrefix(true);
			testAssembler(c -> c.bnd().notrack().call(qword_ptr(rax)), inst);
		}
	}

	@Test
	public void TestOperandModifiers() {
		{
			Instruction inst = Instruction.create(Code.EVEX_VUNPCKLPS_XMM_K1Z_XMM_XMMM128B32, ICRegisters.xmm2, ICRegisters.xmm6,
					mem_ptr(rax).toMemoryOperand(64));
			inst.setZeroingMasking(true);
			inst.setOpMask(Register.K1);
			inst.setBroadcast(true);
			testAssembler(c -> c.vunpcklps(xmm2.k1().z(), xmm6, dword_bcst(rax)), inst, TestInstrFlags.PREFER_EVEX);
		}
		{
			Instruction inst = Instruction.create(Code.EVEX_VUNPCKLPS_XMM_K1Z_XMM_XMMM128B32, ICRegisters.xmm2, ICRegisters.xmm6,
					mem_ptr(rax).toMemoryOperand(64));
			inst.setZeroingMasking(true);
			inst.setOpMask(Register.K2);
			inst.setBroadcast(true);
			testAssembler(c -> c.vunpcklps(xmm2.k2().z(), xmm6, dword_bcst(rax)), inst, TestInstrFlags.PREFER_EVEX);
		}
		{
			Instruction inst = Instruction.create(Code.EVEX_VUNPCKLPS_XMM_K1Z_XMM_XMMM128B32, ICRegisters.xmm2, ICRegisters.xmm6,
					mem_ptr(rax).toMemoryOperand(64));
			inst.setZeroingMasking(true);
			inst.setOpMask(Register.K3);
			inst.setBroadcast(true);
			testAssembler(c -> c.vunpcklps(xmm2.k3().z(), xmm6, dword_bcst(rax)), inst, TestInstrFlags.PREFER_EVEX);
		}
		{
			Instruction inst = Instruction.create(Code.EVEX_VUNPCKLPS_XMM_K1Z_XMM_XMMM128B32, ICRegisters.xmm2, ICRegisters.xmm6,
					mem_ptr(rax).toMemoryOperand(64));
			inst.setZeroingMasking(true);
			inst.setOpMask(Register.K4);
			inst.setBroadcast(true);
			testAssembler(c -> c.vunpcklps(xmm2.k4().z(), xmm6, dword_bcst(rax)), inst, TestInstrFlags.PREFER_EVEX);
		}
		{
			Instruction inst = Instruction.create(Code.EVEX_VUNPCKLPS_XMM_K1Z_XMM_XMMM128B32, ICRegisters.xmm2, ICRegisters.xmm6,
					mem_ptr(rax).toMemoryOperand(64));
			inst.setZeroingMasking(true);
			inst.setOpMask(Register.K5);
			inst.setBroadcast(true);
			testAssembler(c -> c.vunpcklps(xmm2.k5().z(), xmm6, dword_bcst(rax)), inst, TestInstrFlags.PREFER_EVEX);
		}
		{
			Instruction inst = Instruction.create(Code.EVEX_VUNPCKLPS_XMM_K1Z_XMM_XMMM128B32, ICRegisters.xmm2, ICRegisters.xmm6,
					mem_ptr(rax).toMemoryOperand(64));
			inst.setZeroingMasking(true);
			inst.setOpMask(Register.K6);
			inst.setBroadcast(true);
			testAssembler(c -> c.vunpcklps(xmm2.k6().z(), xmm6, dword_bcst(rax)), inst, TestInstrFlags.PREFER_EVEX);
		}
		{
			Instruction inst = Instruction.create(Code.EVEX_VUNPCKLPS_XMM_K1Z_XMM_XMMM128B32, ICRegisters.xmm2, ICRegisters.xmm6,
					mem_ptr(rax).toMemoryOperand(64));
			inst.setZeroingMasking(true);
			inst.setOpMask(Register.K7);
			inst.setBroadcast(true);
			testAssembler(c -> c.vunpcklps(xmm2.k7().z(), xmm6, dword_bcst(rax)), inst, TestInstrFlags.PREFER_EVEX);
		}

		{
			Instruction inst = Instruction.create(Code.EVEX_VCVTTSS2SI_R64_XMMM32_SAE, ICRegisters.rax, ICRegisters.xmm1);
			inst.setSuppressAllExceptions(true);
			testAssembler(c -> c.vcvttss2si(rax, xmm1.sae()), inst, TestInstrFlags.PREFER_EVEX);
		}
		{
			Instruction inst = Instruction.create(Code.EVEX_VADDPD_ZMM_K1Z_ZMM_ZMMM512B64_ER, ICRegisters.zmm1, ICRegisters.zmm2, ICRegisters.zmm3);
			inst.setOpMask(Register.K1);
			inst.setRoundingControl(RoundingControl.ROUND_DOWN);
			testAssembler(c -> c.vaddpd(zmm1.k1(), zmm2, zmm3.rd_sae()), inst);
		}
		{
			Instruction inst = Instruction.create(Code.EVEX_VADDPD_ZMM_K1Z_ZMM_ZMMM512B64_ER, ICRegisters.zmm1, ICRegisters.zmm2, ICRegisters.zmm3);
			inst.setOpMask(Register.K1);
			inst.setZeroingMasking(true);
			inst.setRoundingControl(RoundingControl.ROUND_UP);
			testAssembler(c -> c.vaddpd(zmm1.k1().z(), zmm2, zmm3.ru_sae()), inst);
		}
		{
			Instruction inst = Instruction.create(Code.EVEX_VADDPD_ZMM_K1Z_ZMM_ZMMM512B64_ER, ICRegisters.zmm1, ICRegisters.zmm2, ICRegisters.zmm3);
			inst.setOpMask(Register.K2);
			inst.setRoundingControl(RoundingControl.ROUND_TO_NEAREST);
			testAssembler(c -> c.vaddpd(zmm1.k2(), zmm2, zmm3.rn_sae()), inst);
		}
		{
			Instruction inst = Instruction.create(Code.EVEX_VADDPD_ZMM_K1Z_ZMM_ZMMM512B64_ER, ICRegisters.zmm1, ICRegisters.zmm2, ICRegisters.zmm3);
			inst.setOpMask(Register.K3);
			inst.setZeroingMasking(true);
			inst.setRoundingControl(RoundingControl.ROUND_TOWARD_ZERO);
			testAssembler(c -> c.vaddpd(zmm1.k3().z(), zmm2, zmm3.rz_sae()), inst);
		}
	}

	@Test
	void TestVexEvexPrefixes() {
		CodeAssembler a = new CodeAssembler(64);

		a.setPreferVex(true);
		assertTrue(a.getPreferVex());
		a.vaddpd(xmm1, xmm2, xmm3);
		a.vex().vaddpd(xmm1, xmm2, xmm3);
		a.vaddpd(xmm1, xmm2, xmm3);
		a.evex().vaddpd(xmm1, xmm2, xmm3);
		a.vaddpd(xmm1, xmm2, xmm3);
		assertTrue(a.getPreferVex());

		a.setPreferVex(false);
		assertFalse(a.getPreferVex());
		a.vaddpd(xmm1, xmm2, xmm3);
		a.vex().vaddpd(xmm1, xmm2, xmm3);
		a.vaddpd(xmm1, xmm2, xmm3);
		a.evex().vaddpd(xmm1, xmm2, xmm3);
		a.vaddpd(xmm1, xmm2, xmm3);
		assertFalse(a.getPreferVex());

		CodeWriterImpl writer = new CodeWriterImpl();
		a.assemble(writer, 0);
		byte[] bytes = writer.toArray();
		assertArrayEquals(new byte[] {
			(byte)0xC5, (byte)0xE9, 0x58, (byte)0xCB,
			(byte)0xC5, (byte)0xE9, 0x58, (byte)0xCB,
			(byte)0xC5, (byte)0xE9, 0x58, (byte)0xCB,
			0x62, (byte)0xF1, (byte)0xED, 0x08, 0x58, (byte)0xCB,
			(byte)0xC5, (byte)0xE9, 0x58, (byte)0xCB,
			0x62, (byte)0xF1, (byte)0xED, 0x08, 0x58, (byte)0xCB,
			(byte)0xC5, (byte)0xE9, 0x58, (byte)0xCB,
			0x62, (byte)0xF1, (byte)0xED, 0x08, 0x58, (byte)0xCB,
			0x62, (byte)0xF1, (byte)0xED, 0x08, 0x58, (byte)0xCB,
			0x62, (byte)0xF1, (byte)0xED, 0x08, 0x58, (byte)0xCB,
		}, bytes);
	}

	@Test
	void Test_zero_bytes() {
		CodeAssembler a = new CodeAssembler(64);

		CodeLabel lblf = a.createLabel();
		CodeLabel lbll = a.createLabel();
		CodeLabel lbl1 = a.createLabel();
		CodeLabel lbl2 = a.createLabel();

		a.label(lblf);
		a.zero_bytes();

		a.je(lbl1);
		a.je(lbl2);
		a.label(lbl1);
		a.zero_bytes();
		a.label(lbl2);
		a.nop();
		a.lock().rep().zero_bytes();

		a.label(lbll);
		a.zero_bytes();

		CodeWriterImpl writer = new CodeWriterImpl();
		a.assemble(writer, 0);
		byte[] bytes = writer.toArray();
		assertArrayEquals(new byte[] { 0x74, 0x02, 0x74, 0x00, (byte)0x90 }, bytes);
	}
}
