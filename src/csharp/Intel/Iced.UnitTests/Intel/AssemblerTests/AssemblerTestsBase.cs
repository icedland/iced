// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if ENCODER && BLOCK_ENCODER && CODE_ASSEMBLER
using System;
using Iced.Intel;
using Iced.UnitTests.Intel.EncoderTests;
using Xunit;

namespace Iced.UnitTests.Intel.AssemblerTests {
	public abstract class AssemblerTestsBase {
		readonly int bitness;

		protected AssemblerTestsBase(int bitness) =>
			this.bitness = bitness;

		public int Bitness => bitness;

		protected void TestAssembler(Action<Assembler> fAsm, Instruction expectedInst, LocalOpCodeFlags flags = LocalOpCodeFlags.None) {
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

			// Encode the instruction first to get any errors
			var writer = new CodeWriterImpl();
			assembler.Assemble(writer, 0, (flags & LocalOpCodeFlags.BranchUlong) != 0 ? BlockEncoderOptions.None : BlockEncoderOptions.DontFixBranches);

			// Check that the instruction is the one expected
			if ((flags & LocalOpCodeFlags.Broadcast) != 0)
				expectedInst.IsBroadcast = true;
			var inst = assembler.Instructions[0];
			if ((flags & LocalOpCodeFlags.IgnoreCode) != 0)
				expectedInst.Code = inst.Code;
			Assert.Equal(expectedInst, inst);

			// Special for decoding options
			var opCode = inst.Code.ToOpCode();
			var decoderOptions = opCode.DecoderOption;
			switch (bitness) {
			case 16:
				if (!opCode.IntelDecoder16 && opCode.AmdDecoder16)
					decoderOptions |= DecoderOptions.AMD;
				break;
			case 32:
				if (!opCode.IntelDecoder32 && opCode.AmdDecoder32)
					decoderOptions |= DecoderOptions.AMD;
				break;
			case 64:
				if (!opCode.IntelDecoder64 && opCode.AmdDecoder64)
					decoderOptions |= DecoderOptions.AMD;
				break;
			default:
				throw new InvalidOperationException();
			}
			if (opCode.IsReservedNop)
				decoderOptions |= DecoderOptions.ForceReservedNop;

			// Check decoding back against the original instruction
			var instructionAsBytes = new System.Text.StringBuilder();
			foreach (var b in writer.ToArray())
				instructionAsBytes.Append($"{b:X2} ");

			var decoder = Decoder.Create(bitness, new ByteArrayCodeReader(writer.ToArray()), decoderOptions);
			var decodedInst = decoder.Decode();
			if ((flags & LocalOpCodeFlags.IgnoreCode) != 0)
				decodedInst.Code = inst.Code;
			switch (inst.Code) {
			case Code.Montmul_16:
			case Code.Montmul_32:
			case Code.Montmul_64:
			case Code.Xsha1_16:
			case Code.Xsha1_32:
			case Code.Xsha1_64:
			case Code.Xsha256_16:
			case Code.Xsha256_32:
			case Code.Xsha256_64:
			case Code.Xcryptecb_16:
			case Code.Xcryptecb_32:
			case Code.Xcryptecb_64:
			case Code.Xcryptcbc_16:
			case Code.Xcryptcbc_32:
			case Code.Xcryptcbc_64:
			case Code.Xcryptctr_16:
			case Code.Xcryptctr_32:
			case Code.Xcryptctr_64:
			case Code.Xcryptcfb_16:
			case Code.Xcryptcfb_32:
			case Code.Xcryptcfb_64:
			case Code.Xcryptofb_16:
			case Code.Xcryptofb_32:
			case Code.Xcryptofb_64:
			case Code.Ccs_hash_16:
			case Code.Ccs_hash_32:
			case Code.Ccs_hash_64:
			case Code.Ccs_encrypt_16:
			case Code.Ccs_encrypt_32:
			case Code.Ccs_encrypt_64:
				// They're mandatory prefix instructions but the REP prefix isn't cleared since it's shown in disassembly
				decodedInst.HasRepPrefix = false;
				break;
			}
			if ((flags & LocalOpCodeFlags.Fwait) != 0) {
				Assert.Equal(decodedInst, Instruction.Create(Code.Wait));
				decodedInst = decoder.Decode();
				decodedInst.Code = decodedInst.Code switch {
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
					_ => decodedInst.Code,
				};
			}

			// Reset IP to 0 when matching against decode
			if (inst.Code != Code.Jmpe_disp16 && inst.Code != Code.Jmpe_disp32 && (flags & LocalOpCodeFlags.Branch) != 0) {
				// Check if it's a label ID, if so, replace it with the IP
				if (inst.NearBranch64 == 1)
					inst.NearBranch64 = 0;
			}

			// Special case for branch via ulong. An instruction like `loopne 000031D0h`
			// could have been encoded with a sequence like:
			//
			// 0:  e0 02                   loopne 0x4
			// 2:  eb 05                   jmp    0x9
			// 4:  e9 c7 31 00 00          jmp    0x31d0
			if ((flags & LocalOpCodeFlags.BranchUlong) != 0) {
				Assert.Equal(inst.Code.ToShortBranch(), decodedInst.Code.ToShortBranch());

				if (decodedInst.NearBranch64 == 4) {
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
					Assert.True(inst.NearBranch64 == decodedInst.NearBranch64, $"Branch decoding offset failed!\nExpected: {inst} ({instructionAsBytes})\nActual Decoded: {decodedInst}\n");
			}
			else
				Assert.True(inst == decodedInst, $"Decoding failed!\nExpected: {inst} ({instructionAsBytes})\nActual Decoded: {decodedInst}\n");
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
		}
	}
}
#endif
