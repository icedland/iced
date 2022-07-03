// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.asm;

import static org.junit.jupiter.api.Assertions.*;
import org.junit.jupiter.api.Test;

import com.github.icedland.iced.x86.CodeWriterImpl;

import static com.github.icedland.iced.x86.asm.AsmRegisters.*;

final class AssemblerLabelTests {
	@Test
	void multiple_labels_on_same_instruction_throws() {
		CodeAssembler c = new CodeAssembler(64);
		CodeLabel l1 = c.createLabel();
		CodeLabel l2 = c.createLabel();
		CodeLabel l3 = c.createLabel();
		c.nop();
		c.label(l1);
		c.nop();
		c.label(l2);
		assertThrows(IllegalArgumentException.class, () -> c.label(l3));
	}

	@Test
	void anonymous_labels() {
		CodeAssembler c = new CodeAssembler(64);

		CodeLabel lbl1 = c.createLabel();
		CodeLabel lbl2 = c.createLabel();
		CodeLabel lbl3 = c.createLabel();
		CodeLabel lbl4 = c.createLabel();

		c.label(lbl1);
		c.inc(eax);
		c.nop();
		c.anonymousLabel();
		c.je(c.b());
		c.nop();
		c.label(lbl2);
		c.je(c.b());
		c.nop();
		c.jmp(lbl1);
		c.nop();
		c.jmp(lbl2);
		c.nop();
		c.jmp(lbl3);
		c.nop();
		c.jmp(lbl4);
		c.nop();
		c.jne(c.f());
		c.nop();
		c.label(lbl3);
		c.jne(c.f());
		c.nop();
		c.anonymousLabel();
		c.inc(eax);
		c.nop();
		c.label(lbl4);
		c.nop();
		c.nop();

		byte[] expectedData = new byte[] {
			(byte)0xFF, (byte)0xC0, (byte)0x90, 0x74, (byte)0xFE, (byte)0x90, 0x74, (byte)0xFB, (byte)0x90, (byte)0xEB, (byte)0xF5, (byte)0x90, (byte)0xEB, (byte)0xF8, (byte)0x90, (byte)0xEB,
			0x07, (byte)0x90, (byte)0xEB, 0x0A, (byte)0x90, 0x75, 0x04, (byte)0x90, 0x75, 0x01, (byte)0x90, (byte)0xFF, (byte)0xC0, (byte)0x90, (byte)0x90, (byte)0x90,
		};
		CodeWriterImpl writer = new CodeWriterImpl();
		c.assemble(writer, 0);
		assertArrayEquals(expectedData, writer.toArray());
	}

	@Test
	void unused_anonymous_label_fails() {
		CodeAssembler c = new CodeAssembler(64);
		c.nop();
		c.anonymousLabel();
		Object result = c.assemble(new CodeWriterImpl(), 0);
		assertInstanceOf(String.class, result);
	}

	@Test
	void undeclared_forward_anon_label_fails() {
		CodeAssembler c = new CodeAssembler(64);
		c.nop();
		c.je(c.f());
		Object result = c.assemble(new CodeWriterImpl(), 0);
		assertInstanceOf(String.class, result);
	}

	@Test
	void at_most_one_anon_label_per_instruction() {
		CodeAssembler c = new CodeAssembler(64);
		c.nop();
		c.anonymousLabel();
		assertThrows(UnsupportedOperationException.class, () -> c.anonymousLabel());
	}

	@Test
	void referencing_backward_anon_label_when_not_defined_throws() {
		CodeAssembler c = new CodeAssembler(64);
		c.nop();
		assertThrows(UnsupportedOperationException.class, () -> c.je(c.b()));
	}

	@Test
	void anonymous_label_and_named_label_cant_use_same_instruction() {
		{
			CodeAssembler c = new CodeAssembler(64);
			CodeLabel lbl = c.createLabel();
			c.nop();
			c.label(lbl);
			c.anonymousLabel();
			assertThrows(UnsupportedOperationException.class, () -> c.nop());
		}
		{
			CodeAssembler c = new CodeAssembler(64);
			CodeLabel lbl = c.createLabel();
			c.nop();
			c.anonymousLabel();
			c.label(lbl);
			assertThrows(UnsupportedOperationException.class, () -> c.nop());
		}
	}
}
