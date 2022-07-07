// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if ENCODER && BLOCK_ENCODER && CODE_ASSEMBLER
using System;
using Iced.Intel;
using Xunit;
using static Iced.Intel.AssemblerRegisters;

namespace Iced.UnitTests.Intel.AssemblerTests {
	public sealed class AssemblerLabelTests {
		[Fact]
		void Multiple_labels_on_same_instruction_throws() {
			var c = new Assembler(64);
			var l1 = c.CreateLabel();
			var l2 = c.CreateLabel();
			var l3 = c.CreateLabel();
			c.nop();
			c.Label(ref l1);
			c.nop();
			c.Label(ref l2);
			Assert.Throws<ArgumentException>(() => c.Label(ref l3));
		}

		[Fact]
		void Anonymous_labels() {
			var c = new Assembler(64);

			var lbl1 = c.CreateLabel();
			var lbl2 = c.CreateLabel();
			var lbl3 = c.CreateLabel();
			var lbl4 = c.CreateLabel();

			c.Label(ref lbl1);
			c.inc(eax);
			c.nop();
			c.AnonymousLabel();
			c.je(c.@B);
			c.nop();
			c.Label(ref lbl2);
			c.je(c.@B);
			c.nop();
			c.jmp(lbl1);
			c.nop();
			c.jmp(lbl2);
			c.nop();
			c.jmp(lbl3);
			c.nop();
			c.jmp(lbl4);
			c.nop();
			c.jne(c.@F);
			c.nop();
			c.Label(ref lbl3);
			c.jne(c.@F);
			c.nop();
			c.AnonymousLabel();
			c.inc(eax);
			c.nop();
			c.Label(ref lbl4);
			c.nop();
			c.nop();

			var expectedData = new byte[] {
				0xFF, 0xC0, 0x90, 0x74, 0xFE, 0x90, 0x74, 0xFB, 0x90, 0xEB, 0xF5, 0x90, 0xEB, 0xF8, 0x90, 0xEB,
				0x07, 0x90, 0xEB, 0x0A, 0x90, 0x75, 0x04, 0x90, 0x75, 0x01, 0x90, 0xFF, 0xC0, 0x90, 0x90, 0x90,
			};
			var writer = new CodeWriterImpl();
			c.Assemble(writer, 0);
			Assert.Equal(expectedData, writer.ToArray());
		}

		[Fact]
		void Unused_anonymous_label_throws() {
			var c = new Assembler(64);
			c.nop();
			c.AnonymousLabel();
			Assert.Throws<InvalidOperationException>(() => c.Assemble(new CodeWriterImpl(), 0));
		}

		[Fact]
		void Undeclared_forward_anon_label_throws() {
			var c = new Assembler(64);
			c.nop();
			c.je(c.@F);
			Assert.Throws<InvalidOperationException>(() => c.Assemble(new CodeWriterImpl(), 0));
		}

		[Fact]
		void At_most_one_anon_label_per_instruction() {
			var c = new Assembler(64);
			c.nop();
			c.AnonymousLabel();
			Assert.Throws<InvalidOperationException>(() => c.AnonymousLabel());
		}

		[Fact]
		void Referencing_backward_anon_label_when_not_defined_throws() {
			var c = new Assembler(64);
			c.nop();
			Assert.Throws<InvalidOperationException>(() => c.je(c.@B));
		}

		[Fact]
		void Anonymous_label_and_named_label_cant_use_same_instruction() {
			{
				var c = new Assembler(64);
				var lbl = c.CreateLabel();
				c.nop();
				c.Label(ref lbl);
				c.AnonymousLabel();
				Assert.Throws<InvalidOperationException>(() => c.nop());
			}
			{
				var c = new Assembler(64);
				var lbl = c.CreateLabel();
				c.nop();
				c.AnonymousLabel();
				c.Label(ref lbl);
				Assert.Throws<InvalidOperationException>(() => c.nop());
			}
		}
	}
}
#endif
