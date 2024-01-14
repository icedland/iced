// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if ENCODER && BLOCK_ENCODER && CODE_ASSEMBLER
using System;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.AssemblerTests {
	public class AssemblerMultiByteNopTests {
		[Fact]
		public void TestNops() {
			foreach (var bitness in new[] { 16, 32, 64 }) {
				for (int i = 0; i <= 128; i++) {
					var a = new Assembler(bitness);
					a.nop(i);
					var writer = new CodeWriterImpl();
					a.Assemble(writer, 0);
					var data = writer.ToArray();
					Assert.Equal(i, data.Length);
					var reader = new ByteArrayCodeReader(data);
					var decoder = Decoder.Create(bitness, reader);
					while (reader.CanReadByte) {
						var instr = decoder.Decode();
						switch (instr.Code) {
						case Code.Nopw:
						case Code.Nopd:
						case Code.Nopq:
						case Code.Nop_rm16:
						case Code.Nop_rm32:
						case Code.Nop_rm64:
							break;
						default:
							Assert.Fail($"Expected a NOP but got {instr.Code}");
							break;
						}
					}
				}

				{
					var a = new Assembler(bitness);
					Assert.Throws<ArgumentOutOfRangeException>(() => a.nop(-1));
				}
			}
		}
	}
}
#endif
