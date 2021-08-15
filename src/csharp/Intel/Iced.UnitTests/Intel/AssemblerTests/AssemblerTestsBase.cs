// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if ENCODER && BLOCK_ENCODER && CODE_ASSEMBLER
using System;
using Iced.Intel;
using Iced.UnitTests.Intel.EncoderTests;
using Xunit;

namespace Iced.UnitTests.Intel.AssemblerTests {
	public abstract class AssemblerTestsBase {
		protected const int FirstLabelId = 1;
		readonly int bitness;

		protected AssemblerTestsBase(int bitness) =>
			this.bitness = bitness;

		public int Bitness => bitness;

		protected void TestAssembler(Action<Assembler> fAsm, Instruction expected, LocalOpCodeFlags flags = LocalOpCodeFlags.None,
			DecoderOptions decoderOptions = DecoderOptions.None) {
			var assembler = new Assembler(bitness);

			// Encode the instruction
			if ((flags & LocalOpCodeFlags.PreferVex) != 0)
				assembler.PreferVex = true;
			else if ((flags & LocalOpCodeFlags.PreferEvex) != 0)
				assembler.PreferVex = false;
			if ((flags & LocalOpCodeFlags.PreferShortBranch) != 0)
				assembler.PreferShortBranch = true;
			else if ((flags & LocalOpCodeFlags.PreferNearBranch) != 0)
				assembler.PreferShortBranch = false;
			fAsm(assembler);

			// Expecting only one instruction
			Assert.Equal(1, assembler.Instructions.Count);

			// Check that the instruction is the one expected
			if ((flags & LocalOpCodeFlags.Broadcast) != 0)
				expected.IsBroadcast = true;
			var asmInstr = assembler.Instructions[0];
			Assert.Equal(expected, asmInstr);

			// Encode the instruction first to get any errors
			var writer = new CodeWriterImpl();
			assembler.Assemble(writer, 0, (flags & LocalOpCodeFlags.BranchUlong) != 0 ? BlockEncoderOptions.None : BlockEncoderOptions.DontFixBranches);

			// Check decoding back against the original instruction
			var instructionAsBytes = new System.Text.StringBuilder();
			foreach (var b in writer.ToArray())
				instructionAsBytes.Append($"{b:X2} ");

			var decoder = Decoder.Create(bitness, new ByteArrayCodeReader(writer.ToArray()), decoderOptions);
			var decodedInstr = decoder.Decode();
			if ((flags & LocalOpCodeFlags.IgnoreCode) != 0)
				decodedInstr.Code = asmInstr.Code;
			if ((flags & LocalOpCodeFlags.RemoveRepRepnePrefixes) != 0) {
				decodedInstr.HasRepPrefix = false;
				decodedInstr.HasRepnePrefix = false;
			}
			if ((flags & LocalOpCodeFlags.Fwait) != 0) {
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

			if (asmInstr.Code != Code.Jmpe_disp16 && asmInstr.Code != Code.Jmpe_disp32 && (flags & LocalOpCodeFlags.Branch) != 0)
				asmInstr.NearBranch64 = 0;

			// Short branches can be fixed if the target is too far away.
			// Eg. `loopne target` => `loopne jmpt; jmp short skip; jmpt: jmp near target; skip:`
			if ((flags & LocalOpCodeFlags.BranchUlong) != 0) {
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

		protected static Instruction ApplyK1(Instruction instruction) {
			instruction.OpMask = Register.K1;
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

		[Flags]
		protected enum LocalOpCodeFlags {
			None = 0,
			Fwait = 1 << 0,
			PreferVex = 1 << 1,
			PreferEvex = 1 << 2,
			PreferShortBranch = 1 << 3,
			PreferNearBranch = 1 << 4,
			Branch = 1 << 5,
			Broadcast = 1 << 6,
			BranchUlong = 1 << 7,
			IgnoreCode = 1 << 8,
			RemoveRepRepnePrefixes = 1 << 9,
		}
	}
}
#endif
