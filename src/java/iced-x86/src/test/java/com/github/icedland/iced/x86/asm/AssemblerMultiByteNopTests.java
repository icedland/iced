// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.asm;

import static org.junit.jupiter.api.Assertions.*;
import org.junit.jupiter.api.Test;

import com.github.icedland.iced.x86.Code;
import com.github.icedland.iced.x86.CodeWriterImpl;
import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.dec.ByteArrayCodeReader;
import com.github.icedland.iced.x86.dec.Decoder;

public class AssemblerMultiByteNopTests {
	@Test
	public void testNops() {
		for (int bitness : new int[] { 16, 32, 64 }) {
			for (int i = 0; i <= 128; i++) {
				CodeAssembler a = new CodeAssembler(bitness);
				a.nop(i);
				CodeWriterImpl writer = new CodeWriterImpl();
				a.assemble(writer, 0);
				byte[] data = writer.toArray();
				assertEquals(i, data.length);
				ByteArrayCodeReader reader = new ByteArrayCodeReader(data);
				Decoder decoder = new Decoder(bitness, reader);
				while (reader.canReadByte()) {
					Instruction instr = decoder.decode();
					switch (instr.getCode()) {
					case Code.NOPW:
					case Code.NOPD:
					case Code.NOPQ:
					case Code.NOP_RM16:
					case Code.NOP_RM32:
					case Code.NOP_RM64:
						break;
					default:
						assertTrue(false, String.format("Expected a NOP but got Code %d", instr.getCode()));
						break;
					}
				}
			}

			{
				CodeAssembler a = new CodeAssembler(bitness);
				assertThrows(IllegalArgumentException.class, () -> a.nop(-1));
			}
		}
	}
}
