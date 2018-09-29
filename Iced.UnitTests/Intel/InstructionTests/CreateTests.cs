/*
    Copyright (C) 2018 de4dot@gmail.com

    This file is part of Iced.

    Iced is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Iced is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Iced.  If not, see <https://www.gnu.org/licenses/>.
*/

#if !NO_ENCODER
using System;
using System.Collections.Generic;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.InstructionTests {
	public sealed class CreateTests {
		sealed class CodeWriterImpl : CodeWriter {
			readonly List<byte> bytes = new List<byte>();
			public override void WriteByte(byte value) => bytes.Add(value);
			public byte[] ToArray() => bytes.ToArray();
		}

		[Theory]
		[MemberData(nameof(CreateTest_Data))]
		void CreateTest(int bitness, string hexBytes, Func<Instruction> create) {
			var bytes = HexUtils.ToByteArray(hexBytes);
			var decoder = Decoder.Create(bitness, new ByteArrayCodeReader(bytes));
			switch (bitness) {
			case 16: decoder.InstructionPointer = DecoderConstants.DEFAULT_IP16; break;
			case 32: decoder.InstructionPointer = DecoderConstants.DEFAULT_IP32; break;
			case 64: decoder.InstructionPointer = DecoderConstants.DEFAULT_IP64; break;
			default: throw new InvalidOperationException();
			}
			var origRip = decoder.InstructionPointer;
			decoder.Decode(out var decodedInstr);
			decodedInstr.CodeSize = 0;
			decodedInstr.ByteLength = 0;
			decodedInstr.NextIP64 = 0;

			var createdInstr = create();
			Assert.True(Instruction.TEST_BitByBitEquals(ref decodedInstr, ref createdInstr));

			var writer = new CodeWriterImpl();
			var encoder = decoder.CreateEncoder(writer);
			bool result = encoder.TryEncode(ref createdInstr, origRip, out _, out var errorMessage);
			Assert.Null(errorMessage);
			Assert.True(result);
			Assert.Equal(bytes, writer.ToArray());
		}
		public static IEnumerable<object[]> CreateTest_Data {
			get {
				yield return new object[] { 64, "90", new Func<Instruction>(() => Instruction.Create(Code.Nopd)) };
				yield return new object[] { 64, "48B9FFFFFFFFFFFFFFFF", new Func<Instruction>(() => Instruction.Create(Code.Mov_r64_imm64, Register.RCX, -1)) };
				yield return new object[] { 64, "48B9123456789ABCDE31", new Func<Instruction>(() => Instruction.Create(Code.Mov_r64_imm64, Register.RCX, 0x31DEBC9A78563412)) };
				yield return new object[] { 64, "48B9FFFFFFFF00000000", new Func<Instruction>(() => Instruction.Create(Code.Mov_r64_imm64, Register.RCX, 0xFFFFFFFFU)) };
				yield return new object[] { 64, "8FC1", new Func<Instruction>(() => Instruction.Create(Code.Pop_rm64, Register.RCX)) };
				yield return new object[] { 64, "648F847501EFCDAB", new Func<Instruction>(() => Instruction.Create(Code.Pop_rm64, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS))) };
				yield return new object[] { 64, "C6F85A", new Func<Instruction>(() => Instruction.Create(Code.Xabort_imm8, 0x5A)) };
				yield return new object[] { 64, "66685AA5", new Func<Instruction>(() => Instruction.Create(Code.Push_imm16, 0xA55A)) };
				yield return new object[] { 32, "685AA51234", new Func<Instruction>(() => Instruction.Create(Code.Pushd_imm32, 0x3412A55A)) };
				yield return new object[] { 64, "666A5A", new Func<Instruction>(() => Instruction.Create(Code.Pushw_imm8, 0x5A)) };
				yield return new object[] { 32, "6A5A", new Func<Instruction>(() => Instruction.Create(Code.Pushd_imm8, 0x5A)) };
				yield return new object[] { 64, "6A5A", new Func<Instruction>(() => Instruction.Create(Code.Pushq_imm8, 0x5A)) };
				yield return new object[] { 64, "685AA512A4", new Func<Instruction>(() => Instruction.Create(Code.Pushq_imm32, -0x5BED5AA6)) };
				yield return new object[] { 32, "66705A", new Func<Instruction>(() => Instruction.CreateBranch(Code.Jo_rel8_16, 0x4D)) };
				yield return new object[] { 32, "705A", new Func<Instruction>(() => Instruction.CreateBranch(Code.Jo_rel8_32, 0x8000004C)) };
				yield return new object[] { 64, "705A", new Func<Instruction>(() => Instruction.CreateBranch(Code.Jo_rel8_64, 0x800000000000004C)) };
				yield return new object[] { 32, "669A12345678", new Func<Instruction>(() => Instruction.CreateBranch(Code.Call_ptr1616, 0x7856, 0x3412)) };
				yield return new object[] { 32, "9A123456789ABC", new Func<Instruction>(() => Instruction.CreateBranch(Code.Call_ptr3216, 0xBC9A, 0x78563412)) };
				yield return new object[] { 64, "00D1", new Func<Instruction>(() => Instruction.Create(Code.Add_rm8_r8, Register.CL, Register.DL)) };
				yield return new object[] { 64, "64028C7501EFCDAB", new Func<Instruction>(() => Instruction.Create(Code.Add_r8_rm8, Register.CL, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS))) };
				yield return new object[] { 64, "80C15A", new Func<Instruction>(() => Instruction.Create(Code.Add_rm8_imm8, Register.CL, 0x5A)) };
				yield return new object[] { 64, "6681C15AA5", new Func<Instruction>(() => Instruction.Create(Code.Add_rm16_imm16, Register.CX, 0xA55A)) };
				yield return new object[] { 64, "81C15AA51234", new Func<Instruction>(() => Instruction.Create(Code.Add_rm32_imm32, Register.ECX, 0x3412A55A)) };
				yield return new object[] { 64, "48B904152637A55A5678", new Func<Instruction>(() => Instruction.Create(Code.Mov_r64_imm64, Register.RCX, 0x78565AA537261504)) };
				yield return new object[] { 64, "6683C15A", new Func<Instruction>(() => Instruction.Create(Code.Add_rm16_imm8, Register.CX, 0x5A)) };
				yield return new object[] { 64, "83C15A", new Func<Instruction>(() => Instruction.Create(Code.Add_rm32_imm8, Register.ECX, 0x5A)) };
				yield return new object[] { 64, "4883C15A", new Func<Instruction>(() => Instruction.Create(Code.Add_rm64_imm8, Register.RCX, 0x5A)) };
				yield return new object[] { 64, "4881C15AA51234", new Func<Instruction>(() => Instruction.Create(Code.Add_rm64_imm32, Register.RCX, 0x3412A55A)) };
				yield return new object[] { 32, "64676E", new Func<Instruction>(() => Instruction.CreateString_Reg_SegRSI(Code.Outsb_DX_m8, Register.DX, Register.SI, Register.FS)) };
				yield return new object[] { 64, "64676E", new Func<Instruction>(() => Instruction.CreateString_Reg_SegRSI(Code.Outsb_DX_m8, Register.DX, Register.ESI, Register.FS)) };
				yield return new object[] { 64, "646E", new Func<Instruction>(() => Instruction.CreateString_Reg_SegRSI(Code.Outsb_DX_m8, Register.DX, Register.RSI, Register.FS)) };
				yield return new object[] { 32, "67AE", new Func<Instruction>(() => Instruction.CreateString_Reg_ESRDI(Code.Scasb_AL_m8, Register.AL, Register.DI)) };
				yield return new object[] { 64, "67AE", new Func<Instruction>(() => Instruction.CreateString_Reg_ESRDI(Code.Scasb_AL_m8, Register.AL, Register.EDI)) };
				yield return new object[] { 64, "AE", new Func<Instruction>(() => Instruction.CreateString_Reg_ESRDI(Code.Scasb_AL_m8, Register.AL, Register.RDI)) };
				yield return new object[] { 64, "64A0123456789ABCDEF0", new Func<Instruction>(() => Instruction.CreateMemory64(Code.Mov_AL_moffs8, Register.AL, 0xF0DEBC9A78563412, Register.FS)) };
				yield return new object[] { 64, "6400947501EFCDAB", new Func<Instruction>(() => Instruction.Create(Code.Add_rm8_r8, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), Register.DL)) };
				yield return new object[] { 64, "6480847501EFCDAB5A", new Func<Instruction>(() => Instruction.Create(Code.Add_rm8_imm8, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x5A)) };
				yield return new object[] { 64, "646681847501EFCDAB5AA5", new Func<Instruction>(() => Instruction.Create(Code.Add_rm16_imm16, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0xA55A)) };
				yield return new object[] { 64, "6481847501EFCDAB5AA51234", new Func<Instruction>(() => Instruction.Create(Code.Add_rm32_imm32, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x3412A55A)) };
				yield return new object[] { 64, "646683847501EFCDAB5A", new Func<Instruction>(() => Instruction.Create(Code.Add_rm16_imm8, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x5A)) };
				yield return new object[] { 64, "6483847501EFCDAB5A", new Func<Instruction>(() => Instruction.Create(Code.Add_rm32_imm8, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x5A)) };
				yield return new object[] { 64, "644883847501EFCDAB5A", new Func<Instruction>(() => Instruction.Create(Code.Add_rm64_imm8, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x5A)) };
				yield return new object[] { 64, "644881847501EFCDAB5AA51234", new Func<Instruction>(() => Instruction.Create(Code.Add_rm64_imm32, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x3412A55A)) };
				yield return new object[] { 64, "E65A", new Func<Instruction>(() => Instruction.Create(Code.Out_imm8_AL, 0x5A, Register.AL)) };
				yield return new object[] { 64, "66C85AA5A6", new Func<Instruction>(() => Instruction.Create(Code.Enterw_imm16_imm8, 0xA55A, 0xA6)) };
				yield return new object[] { 32, "6467A6", new Func<Instruction>(() => Instruction.CreateString_SegRSI_ESRDI(Code.Cmpsb_m8_m8, Register.SI, Register.DI, Register.FS)) };
				yield return new object[] { 64, "6467A6", new Func<Instruction>(() => Instruction.CreateString_SegRSI_ESRDI(Code.Cmpsb_m8_m8, Register.ESI, Register.EDI, Register.FS)) };
				yield return new object[] { 64, "64A6", new Func<Instruction>(() => Instruction.CreateString_SegRSI_ESRDI(Code.Cmpsb_m8_m8, Register.RSI, Register.RDI, Register.FS)) };
				yield return new object[] { 32, "676C", new Func<Instruction>(() => Instruction.CreateString_ESRDI_Reg(Code.Insb_m8_DX, Register.DI, Register.DX)) };
				yield return new object[] { 32, "6467A4", new Func<Instruction>(() => Instruction.CreateString_ESRDI_SegRSI(Code.Movsb_m8_m8, Register.DI, Register.SI, Register.FS)) };
				yield return new object[] { 64, "676C", new Func<Instruction>(() => Instruction.CreateString_ESRDI_Reg(Code.Insb_m8_DX, Register.EDI, Register.DX)) };
				yield return new object[] { 64, "6467A4", new Func<Instruction>(() => Instruction.CreateString_ESRDI_SegRSI(Code.Movsb_m8_m8, Register.EDI, Register.ESI, Register.FS)) };
				yield return new object[] { 64, "6C", new Func<Instruction>(() => Instruction.CreateString_ESRDI_Reg(Code.Insb_m8_DX, Register.RDI, Register.DX)) };
				yield return new object[] { 64, "64A4", new Func<Instruction>(() => Instruction.CreateString_ESRDI_SegRSI(Code.Movsb_m8_m8, Register.RDI, Register.RSI, Register.FS)) };
				yield return new object[] { 64, "64A2123456789ABCDEF0", new Func<Instruction>(() => Instruction.CreateMemory64(Code.Mov_moffs8_AL, 0xF0DEBC9A78563412, Register.AL, Register.FS)) };
				yield return new object[] { 64, "C5E814CB", new Func<Instruction>(() => Instruction.Create(Code.VEX_Vunpcklps_xmm_xmm_xmmm128, Register.XMM1, Register.XMM2, Register.XMM3)) };
				yield return new object[] { 64, "64C5E8148C7501EFCDAB", new Func<Instruction>(() => Instruction.Create(Code.VEX_Vunpcklps_xmm_xmm_xmmm128, Register.XMM1, Register.XMM2, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS))) };
				yield return new object[] { 64, "62F1F50873D2A5", new Func<Instruction>(() => Instruction.Create(Code.EVEX_Vpsrlq_xmm_k1z_xmmm128b64_imm8, Register.XMM1, Register.XMM2, 0xA5)) };
				yield return new object[] { 64, "6669CAA55A", new Func<Instruction>(() => Instruction.Create(Code.Imul_r16_rm16_imm16, Register.CX, Register.DX, 0x5AA5)) };
				yield return new object[] { 64, "69CA5AA51234", new Func<Instruction>(() => Instruction.Create(Code.Imul_r32_rm32_imm32, Register.ECX, Register.EDX, 0x3412A55A)) };
				yield return new object[] { 64, "666BCA5A", new Func<Instruction>(() => Instruction.Create(Code.Imul_r16_rm16_imm8, Register.CX, Register.DX, 0x5A)) };
				yield return new object[] { 64, "6BCA5A", new Func<Instruction>(() => Instruction.Create(Code.Imul_r32_rm32_imm8, Register.ECX, Register.EDX, 0x5A)) };
				yield return new object[] { 64, "486BCA5A", new Func<Instruction>(() => Instruction.Create(Code.Imul_r64_rm64_imm8, Register.RCX, Register.RDX, 0x5A)) };
				yield return new object[] { 64, "4869CA5AA512A4", new Func<Instruction>(() => Instruction.Create(Code.Imul_r64_rm64_imm32, Register.RCX, Register.RDX, -0x5BED5AA6)) };
				yield return new object[] { 64, "64C4E261908C7501EFCDAB", new Func<Instruction>(() => Instruction.Create(Code.VEX_Vpgatherdd_xmm_vm32x_xmm, Register.XMM1, new MemoryOperand(Register.RBP, Register.XMM6, 2, -0x543210FF, 8, false, Register.FS), Register.XMM3)) };
				yield return new object[] { 64, "6462F1F50873947501EFCDABA5", new Func<Instruction>(() => Instruction.Create(Code.EVEX_Vpsrlq_xmm_k1z_xmmm128b64_imm8, Register.XMM1, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0xA5)) };
				yield return new object[] { 64, "6466698C7501EFCDAB5AA5", new Func<Instruction>(() => Instruction.Create(Code.Imul_r16_rm16_imm16, Register.CX, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0xA55A)) };
				yield return new object[] { 64, "64698C7501EFCDAB5AA51234", new Func<Instruction>(() => Instruction.Create(Code.Imul_r32_rm32_imm32, Register.ECX, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x3412A55A)) };
				yield return new object[] { 64, "64666B8C7501EFCDAB5A", new Func<Instruction>(() => Instruction.Create(Code.Imul_r16_rm16_imm8, Register.CX, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x5A)) };
				yield return new object[] { 64, "646B8C7501EFCDAB5A", new Func<Instruction>(() => Instruction.Create(Code.Imul_r32_rm32_imm8, Register.ECX, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x5A)) };
				yield return new object[] { 64, "64486B8C7501EFCDAB5A", new Func<Instruction>(() => Instruction.Create(Code.Imul_r64_rm64_imm8, Register.RCX, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x5A)) };
				yield return new object[] { 64, "6448698C7501EFCDAB5AA512A4", new Func<Instruction>(() => Instruction.Create(Code.Imul_r64_rm64_imm32, Register.RCX, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), -0x5BED5AA6)) };
				yield return new object[] { 64, "660F78C1A5FD", new Func<Instruction>(() => Instruction.Create(Code.Extrq_xmm_imm8_imm8, Register.XMM1, 0xA5, 0xFD)) };
				yield return new object[] { 64, "64C4E2692E9C7501EFCDAB", new Func<Instruction>(() => Instruction.Create(Code.VEX_Vmaskmovps_m128_xmm_xmm, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), Register.XMM2, Register.XMM3)) };
				yield return new object[] { 64, "64660FA4947501EFCDAB5A", new Func<Instruction>(() => Instruction.Create(Code.Shld_rm16_r16_imm8, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), Register.DX, 0x5A)) };
				yield return new object[] { 32, "64670FF7D3", new Func<Instruction>(() => Instruction.CreateMaskmov_SegRDI_Reg_Reg(Code.Maskmovq_rDI_mm_mm, Register.DI, Register.MM2, Register.MM3, Register.FS)) };
				yield return new object[] { 64, "64670FF7D3", new Func<Instruction>(() => Instruction.CreateMaskmov_SegRDI_Reg_Reg(Code.Maskmovq_rDI_mm_mm, Register.EDI, Register.MM2, Register.MM3, Register.FS)) };
				yield return new object[] { 64, "640FF7D3", new Func<Instruction>(() => Instruction.CreateMaskmov_SegRDI_Reg_Reg(Code.Maskmovq_rDI_mm_mm, Register.RDI, Register.MM2, Register.MM3, Register.FS)) };
				yield return new object[] { 64, "C4E3694ACB40", new Func<Instruction>(() => Instruction.Create(Code.VEX_Vblendvps_xmm_xmm_xmmm128_xmm, Register.XMM1, Register.XMM2, Register.XMM3, Register.XMM4)) };
				yield return new object[] { 64, "64C4E3E95C8C7501EFCDAB30", new Func<Instruction>(() => Instruction.Create(Code.VEX_Vfmaddsubps_xmm_xmm_xmm_xmmm128, Register.XMM1, Register.XMM2, Register.XMM3, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS))) };
				yield return new object[] { 64, "62F16D08C4CBA5", new Func<Instruction>(() => Instruction.Create(Code.EVEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register.XMM1, Register.XMM2, Register.EBX, 0xA5)) };
				yield return new object[] { 64, "64C4E3694A8C7501EFCDAB40", new Func<Instruction>(() => Instruction.Create(Code.VEX_Vblendvps_xmm_xmm_xmmm128_xmm, Register.XMM1, Register.XMM2, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), Register.XMM4)) };
				yield return new object[] { 64, "6462F16D08C48C7501EFCDABA5", new Func<Instruction>(() => Instruction.Create(Code.EVEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register.XMM1, Register.XMM2, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0xA5)) };
				yield return new object[] { 64, "F20F78CAA5FD", new Func<Instruction>(() => Instruction.Create(Code.Insertq_xmm_xmm_imm8_imm8, Register.XMM1, Register.XMM2, 0xA5, 0xFD)) };
				yield return new object[] { 64, "C4E36948CB40", new Func<Instruction>(() => Instruction.Create(Code.VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm8, Register.XMM1, Register.XMM2, Register.XMM3, Register.XMM4, 0x0)) };
				yield return new object[] { 64, "64C4E3E9488C7501EFCDAB31", new Func<Instruction>(() => Instruction.Create(Code.VEX_Vpermil2ps_xmm_xmm_xmm_xmmm128_imm8, Register.XMM1, Register.XMM2, Register.XMM3, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x1)) };
				yield return new object[] { 64, "64C4E369488C7501EFCDAB41", new Func<Instruction>(() => Instruction.Create(Code.VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm8, Register.XMM1, Register.XMM2, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), Register.XMM4, 0x1)) };
				yield return new object[] { 16, "0FB8 55AA", new Func<Instruction>(() => Instruction.CreateBranch(Code.Jmpe_disp16, 0xAA55)) };
				yield return new object[] { 32, "0FB8 123455AA", new Func<Instruction>(() => Instruction.CreateBranch(Code.Jmpe_disp32, 0xAA553412)) };
			}
		}
	}
}
#endif
