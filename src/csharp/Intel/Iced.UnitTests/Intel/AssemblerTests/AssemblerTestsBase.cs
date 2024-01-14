// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if ENCODER && BLOCK_ENCODER && CODE_ASSEMBLER
using System;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.AssemblerTests {
	public abstract class AssemblerTestsBase {
		protected const int FirstLabelId = 1;
		readonly int bitness;

		protected AssemblerTestsBase(int bitness) =>
			this.bitness = bitness;

		public int Bitness => bitness;

		private protected void TestAssembler(Action<Assembler> fAsm, Instruction expected, TestInstrFlags flags = TestInstrFlags.None,
			DecoderOptions decoderOptions = DecoderOptions.None) {
			var assembler = new Assembler(bitness);

			// Encode the instruction
			if ((flags & TestInstrFlags.PreferVex) != 0)
				assembler.PreferVex = true;
			else if ((flags & TestInstrFlags.PreferEvex) != 0)
				assembler.PreferVex = false;
			if ((flags & TestInstrFlags.PreferShortBranch) != 0)
				assembler.PreferShortBranch = true;
			else if ((flags & TestInstrFlags.PreferNearBranch) != 0)
				assembler.PreferShortBranch = false;
			fAsm(assembler);

			// Expecting only one instruction
			Assert.Single(assembler.Instructions);

			// Check that the instruction is the one expected
			if ((flags & TestInstrFlags.Broadcast) != 0)
				expected.IsBroadcast = true;
			var asmInstr = assembler.Instructions[0];
			Assert.Equal(expected, asmInstr);

			// Encode the instruction first to get any errors
			var writer = new CodeWriterImpl();
			assembler.Assemble(writer, 0, (flags & TestInstrFlags.BranchU64) != 0 ? BlockEncoderOptions.None : BlockEncoderOptions.DontFixBranches);

			// Check decoding back against the original instruction
			var instructionAsBytes = new System.Text.StringBuilder();
			foreach (var b in writer.ToArray())
				instructionAsBytes.Append($"{b:X2} ");

			var instrBytes = writer.ToArray();
			var decoder = Decoder.Create(bitness, new ByteArrayCodeReader(instrBytes), decoderOptions);
			Instruction decodedInstr;
			if (expected.Code == Code.Zero_bytes && instrBytes.Length == 0)
				decodedInstr = Instruction.Create(Code.Zero_bytes);
			else
				decodedInstr = decoder.Decode();
			if ((flags & TestInstrFlags.IgnoreCode) != 0)
				decodedInstr.Code = asmInstr.Code;
			if ((flags & TestInstrFlags.RemoveRepRepnePrefixes) != 0) {
				decodedInstr.HasRepPrefix = false;
				decodedInstr.HasRepnePrefix = false;
			}
			if ((flags & TestInstrFlags.Fwait) != 0) {
				Assert.Equal(decodedInstr, Instruction.Create(Code.Wait));
				decodedInstr = decoder.Decode();
				decodedInstr.Code = decodedInstr.Code switch {
					Code.Fnstenv_m14byte => Code.Fstenv_m14byte,
					Code.Fnstenv_m28byte => Code.Fstenv_m28byte,
					Code.Fnstcw_m2byte => Code.Fstcw_m2byte,
					Code.Fneni => Code.Feni,
					Code.Fndisi => Code.Fdisi,
					Code.Fnclex => Code.Fclex,
					Code.Fninit => Code.Finit,
					Code.Fnsetpm => Code.Fsetpm,
					Code.Fnsave_m94byte => Code.Fsave_m94byte,
					Code.Fnsave_m108byte => Code.Fsave_m108byte,
					Code.Fnstsw_m2byte => Code.Fstsw_m2byte,
					Code.Fnstsw_AX => Code.Fstsw_AX,
					Code.Fnstdw_AX => Code.Fstdw_AX,
					Code.Fnstsg_AX => Code.Fstsg_AX,
					_ => throw new InvalidOperationException(),
				};
			}

			if (asmInstr.Code != Code.Jmpe_disp16 && asmInstr.Code != Code.Jmpe_disp32 && (flags & TestInstrFlags.Branch) != 0)
				asmInstr.NearBranch64 = 0;

			// Short branches can be re-written if the target is too far away.
			// Eg. `loopne target` => `loopne jmpt; jmp short skip; jmpt: jmp near target; skip:`
			if ((flags & TestInstrFlags.BranchU64) != 0) {
				asmInstr.Code = asmInstr.Code.ToShortBranch();
				decodedInstr.Code = decodedInstr.Code.ToShortBranch();
				Assert.Equal(asmInstr.Code, decodedInstr.Code);

				if (decodedInstr.NearBranch64 == 4) {
					var nextDecodedInst = decoder.Decode();
					var expectedCode = Bitness switch {
						16 => Code.Jmp_rel8_16,
						32 => Code.Jmp_rel8_32,
						64 => Code.Jmp_rel8_64,
						_ => throw new InvalidOperationException(),
					};

					Assert.True(nextDecodedInst.Code == expectedCode, $"Branch ulong next decoding failed!\nExpected: {expectedCode} \nActual Decoded: {nextDecodedInst}\n");
				}
				else
					Assert.True(asmInstr == decodedInstr, $"Branch decoding offset failed!\nExpected: {asmInstr} ({instructionAsBytes})\nActual Decoded: {decodedInstr}\n");
			}
			else
				Assert.True(asmInstr == decodedInstr, $"Decoding failed!\nExpected: {asmInstr} ({instructionAsBytes})\nActual Decoded: {decodedInstr}\n");
		}

		protected unsafe void TestAssemblerDeclareData<T>(Action<Assembler> fAsm, T[] data) where T : unmanaged {
			var assembler = new Assembler(Bitness);
			var sizeOfT = sizeof(T);
			fAsm(assembler);

			var writer = new CodeWriterImpl();
			assembler.Assemble(writer, 0);
			var buffer = writer.ToArray();

			Assert.Equal(sizeOfT * data.Length, buffer.Length);
			fixed (void* pData = data) {
				for (int i = 0; i < buffer.Length; i++) {
					var expectedData = ((byte*)pData)[i];
					Assert.True(expectedData == buffer[i], $"Invalid data at offset {i}. Expecting {expectedData:x2}. Actual: {buffer[i]}");
				}
			}
		}

		protected static Label CreateAndEmitLabel(Assembler c) {
			var label = c.CreateLabel();
			c.Label(ref label);
			return label;
		}

		protected static Instruction AssignLabel(Instruction instruction, ulong value) {
			instruction.IP = value;
			return instruction;
		}

		protected static Instruction ApplyK(Instruction instruction, Register k) {
			instruction.OpMask = k;
			return instruction;
		}

		protected static void AssertInvalid(Action action) {
			var exception = Assert.Throws<InvalidOperationException>(action);
			Assert.Contains("Unable to calculate an OpCode", exception.Message);
		}
	}

	// GENERATOR-BEGIN: TestInstrFlags
	// ⚠️This was generated by GENERATOR!🦹‍♂️
	[Flags]
	enum TestInstrFlags {
		None = 0x00000000,
		Fwait = 0x00000001,
		PreferVex = 0x00000002,
		PreferEvex = 0x00000004,
		PreferShortBranch = 0x00000008,
		PreferNearBranch = 0x00000010,
		Branch = 0x00000020,
		Broadcast = 0x00000040,
		BranchU64 = 0x00000080,
		IgnoreCode = 0x00000100,
		RemoveRepRepnePrefixes = 0x00000200,
	}
	// GENERATOR-END: TestInstrFlags
}
#endif
