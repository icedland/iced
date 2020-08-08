/*
Copyright (C) 2018-2019 de4dot@gmail.com

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

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
			if ((flags & LocalOpCodeFlags.PreferBranchShort) != 0)
				assembler.PreferBranchShort = true;
			else if ((flags & LocalOpCodeFlags.PreferBranchNear) != 0)
				assembler.PreferBranchShort = false;
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
			Assert.Equal(expectedInst, inst);

			// Special for decoding options
			var decoderOptions = DecoderOptions.None;
			switch (inst.Code) {
			case Code.Call_rm16:
			case Code.Jmp_rm16:
				if (bitness == 64)
					decoderOptions = DecoderOptions.AMD;
				break;
			case Code.Umov_rm8_r8:
			case Code.Umov_rm16_r16:
			case Code.Umov_rm32_r32:
			case Code.Umov_r8_rm8:
			case Code.Umov_r16_rm16:
			case Code.Umov_r32_rm32:
				decoderOptions = DecoderOptions.Umov;
				break;
			case Code.Xbts_r16_rm16:
			case Code.Xbts_r32_rm32:
			case Code.Ibts_rm16_r16:
			case Code.Ibts_rm32_r32:
				decoderOptions = DecoderOptions.Xbts;
				break;
			case Code.Frstpm:
			case Code.Fnstdw_AX:
			case Code.Fstdw_AX:
			case Code.Fnstsg_AX:
			case Code.Fstsg_AX:
				decoderOptions = DecoderOptions.OldFpu;
				break;
			case Code.Pcommit:
				decoderOptions = DecoderOptions.Pcommit;
				break;
			case Code.Loadall386:
				decoderOptions = DecoderOptions.Loadall386;
				break;
			case Code.Cl1invmb:
				decoderOptions = DecoderOptions.Cl1invmb;
				break;
			case Code.Mov_r32_tr:
			case Code.Mov_tr_r32:
				decoderOptions = DecoderOptions.MovTr;
				break;
			case Code.Jmpe_rm16:
			case Code.Jmpe_rm32:
			case Code.Jmpe_disp16:
			case Code.Jmpe_disp32:
				decoderOptions = DecoderOptions.Jmpe;
				break;
			case Code.ReservedNop_rm16_r16_0F0D:
			case Code.ReservedNop_rm32_r32_0F0D:
			case Code.ReservedNop_rm64_r64_0F0D:
			case Code.ReservedNop_rm16_r16_0F18:
			case Code.ReservedNop_rm32_r32_0F18:
			case Code.ReservedNop_rm64_r64_0F18:
			case Code.ReservedNop_rm16_r16_0F19:
			case Code.ReservedNop_rm32_r32_0F19:
			case Code.ReservedNop_rm64_r64_0F19:
			case Code.ReservedNop_rm16_r16_0F1A:
			case Code.ReservedNop_rm32_r32_0F1A:
			case Code.ReservedNop_rm64_r64_0F1A:
			case Code.ReservedNop_rm16_r16_0F1B:
			case Code.ReservedNop_rm32_r32_0F1B:
			case Code.ReservedNop_rm64_r64_0F1B:
			case Code.ReservedNop_rm16_r16_0F1C:
			case Code.ReservedNop_rm32_r32_0F1C:
			case Code.ReservedNop_rm64_r64_0F1C:
			case Code.ReservedNop_rm16_r16_0F1D:
			case Code.ReservedNop_rm32_r32_0F1D:
			case Code.ReservedNop_rm64_r64_0F1D:
			case Code.ReservedNop_rm16_r16_0F1E:
			case Code.ReservedNop_rm32_r32_0F1E:
			case Code.ReservedNop_rm64_r64_0F1E:
			case Code.ReservedNop_rm16_r16_0F1F:
			case Code.ReservedNop_rm32_r32_0F1F:
			case Code.ReservedNop_rm64_r64_0F1F:
				decoderOptions = DecoderOptions.ForceReservedNop;
				break;
			case Code.Bndldx_bnd_mib:
			case Code.Bndmov_bnd_bndm64:
			case Code.Bndmov_bnd_bndm128:
			case Code.Bndcl_bnd_rm32:
			case Code.Bndcl_bnd_rm64:
			case Code.Bndcu_bnd_rm32:
			case Code.Bndcu_bnd_rm64:
			case Code.Bndstx_mib_bnd:
			case Code.Bndmov_bndm64_bnd:
			case Code.Bndmov_bndm128_bnd:
			case Code.Bndmk_bnd_m32:
			case Code.Bndmk_bnd_m64:
			case Code.Bndcn_bnd_rm32:
			case Code.Bndcn_bnd_rm64:
				decoderOptions = DecoderOptions.MPX;
				break;
			case Code.Ud0:
				decoderOptions = DecoderOptions.AMD;
				break;
			case Code.Rdshr_rm32:
			case Code.Wrshr_rm32:
			case Code.Smint:
			case Code.Svdc_m80_Sreg:
			case Code.Rsdc_Sreg_m80:
			case Code.Svldt_m80:
			case Code.Rsldt_m80:
			case Code.Svts_m80:
			case Code.Rsts_m80:
			case Code.Bb0_reset:
			case Code.Bb1_reset:
			case Code.Cpu_write:
			case Code.Cpu_read:
			case Code.Paveb_mm_mmm64:
			case Code.Paddsiw_mm_mmm64:
			case Code.Pmagw_mm_mmm64:
			case Code.Pdistib_mm_m64:
			case Code.Psubsiw_mm_mmm64:
			case Code.Pmvzb_mm_m64:
			case Code.Pmulhrw_mm_mmm64:
			case Code.Pmvnzb_mm_m64:
			case Code.Pmvlzb_mm_m64:
			case Code.Pmvgezb_mm_m64:
			case Code.Pmulhriw_mm_mmm64:
			case Code.Pmachriw_mm_m64:
			case Code.Cyrix_D9D7:
			case Code.Cyrix_D9E2:
			case Code.Ftstp:
			case Code.Cyrix_D9E7:
			case Code.Frint2:
			case Code.Frichop:
			case Code.Cyrix_DED8:
			case Code.Cyrix_DEDA:
			case Code.Cyrix_DEDC:
			case Code.Cyrix_DEDD:
			case Code.Cyrix_DEDE:
			case Code.Frinear:
				decoderOptions = DecoderOptions.Cyrix;
				break;
			case Code.Dmint:
			case Code.Rdm:
				decoderOptions = DecoderOptions.Cyrix_DMI;
				break;
			case Code.Smint_0F7E:
				decoderOptions = DecoderOptions.Cyrix_SMINT_0F7E;
				break;
			case Code.Altinst:
				decoderOptions = DecoderOptions.ALTINST;
				break;
			}

			// Check decoding back against the original instruction
			var instructionAsBytes = new System.Text.StringBuilder();
			foreach (var b in writer.ToArray())
				instructionAsBytes.Append($"{b:X2} ");

			var decoder = Decoder.Create(bitness, new ByteArrayCodeReader(writer.ToArray()), decoderOptions);
			var decodedInst = decoder.Decode();
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
			case Code.XcryptEcb_16:
			case Code.XcryptEcb_32:
			case Code.XcryptEcb_64:
			case Code.XcryptCbc_16:
			case Code.XcryptCbc_32:
			case Code.XcryptCbc_64:
			case Code.XcryptCtr_16:
			case Code.XcryptCtr_32:
			case Code.XcryptCtr_64:
			case Code.XcryptCfb_16:
			case Code.XcryptCfb_32:
			case Code.XcryptCfb_64:
			case Code.XcryptOfb_16:
			case Code.XcryptOfb_32:
			case Code.XcryptOfb_64:
				// They're mandatory prefix instructions but the REP prefix isn't cleared since it's shown in disassembly
				decodedInst.HasRepPrefix = false;
				break;
			}
			if ((flags & LocalOpCodeFlags.Fwait) != 0) {
				Assert.Equal(decodedInst, Instruction.Create(Code.Wait));
				decodedInst = decoder.Decode();

				switch (decodedInst.Code) {
				case Code.Fnstenv_m14byte:
					decodedInst.Code = Code.Fstenv_m14byte;
					break;
				case Code.Fnstenv_m28byte:
					decodedInst.Code = Code.Fstenv_m28byte;
					break;
				case Code.Fnstcw_m2byte:
					decodedInst.Code = Code.Fstcw_m2byte;
					break;
				case Code.Fneni:
					decodedInst.Code = Code.Feni;
					break;
				case Code.Fndisi:
					decodedInst.Code = Code.Fdisi;
					break;
				case Code.Fnclex:
					decodedInst.Code = Code.Fclex;
					break;
				case Code.Fninit:
					decodedInst.Code = Code.Finit;
					break;
				case Code.Fnsetpm:
					decodedInst.Code = Code.Fsetpm;
					break;
				case Code.Fnsave_m94byte:
					decodedInst.Code = Code.Fsave_m94byte;
					break;
				case Code.Fnsave_m108byte:
					decodedInst.Code = Code.Fsave_m108byte;
					break;
				case Code.Fnstsw_m2byte:
					decodedInst.Code = Code.Fstsw_m2byte;
					break;
				case Code.Fnstsw_AX:
					decodedInst.Code = Code.Fstsw_AX;
					break;
				case Code.Fnstdw_AX:
					decodedInst.Code = Code.Fstdw_AX;
					break;
				case Code.Fnstsg_AX:
					decodedInst.Code = Code.Fstsg_AX;
					break;
				}
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

		protected Label CreateAndEmitLabel(Assembler c) {
			var label = c.CreateLabel();
			c.Label(ref label);
			return label;
		}

		protected Instruction CreateMemory64(Code code, AssemblerMemoryOperand dst, AssemblerRegister8 src) =>
			Instruction.CreateMemory64(code, (ulong)dst.Displacement, src, dst.Prefix);

		protected Instruction CreateMemory64(Code code, AssemblerMemoryOperand dst, AssemblerRegister16 src) =>
			Instruction.CreateMemory64(code, (ulong)dst.Displacement, src, dst.Prefix);

		protected Instruction CreateMemory64(Code code, AssemblerMemoryOperand dst, AssemblerRegister32 src) =>
			Instruction.CreateMemory64(code, (ulong)dst.Displacement, src, dst.Prefix);

		protected Instruction CreateMemory64(Code code, AssemblerMemoryOperand dst, AssemblerRegister64 src) =>
			Instruction.CreateMemory64(code, (ulong)dst.Displacement, src, dst.Prefix);

		protected Instruction CreateMemory64(Code code, AssemblerRegister8 dst, AssemblerMemoryOperand src) =>
			Instruction.CreateMemory64(code, dst, (ulong)src.Displacement, src.Prefix);

		protected Instruction CreateMemory64(Code code, AssemblerRegister16 dst, AssemblerMemoryOperand src) =>
			Instruction.CreateMemory64(code, dst, (ulong)src.Displacement, src.Prefix);

		protected Instruction CreateMemory64(Code code, AssemblerRegister32 dst, AssemblerMemoryOperand src) =>
			Instruction.CreateMemory64(code, dst, (ulong)src.Displacement, src.Prefix);

		protected Instruction CreateMemory64(Code code, AssemblerRegister64 dst, AssemblerMemoryOperand src) =>
			Instruction.CreateMemory64(code, dst, (ulong)src.Displacement, src.Prefix);

		protected Instruction AssignLabel(Instruction instruction, ulong value) {
			instruction.IP = value;
			return instruction;
		}

		protected Instruction ApplyK1(Instruction instruction) {
			instruction.OpMask = Register.K1;
			return instruction;
		}

		protected Instruction ApplyK(Instruction instruction, Register k) {
			instruction.OpMask = k;
			return instruction;
		}

		protected void AssertInvalid(Action action) {
			var exception = Assert.Throws<InvalidOperationException>(action);
			Assert.Contains("Unable to calculate an OpCode", exception.Message);
		}

		[Flags]
		protected enum LocalOpCodeFlags {
			None = 0,
			Fwait = 1 << 0,
			PreferVex = 1 << 1,
			PreferEvex = 1 << 2,
			PreferBranchShort = 1 << 3,
			PreferBranchNear = 1 << 4,
			Branch = 1 << 5,
			Broadcast = 1 << 6,
			BranchUlong = 1 << 7,
		}
	}
}
#endif
