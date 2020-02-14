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

#if ENCODER
using System.Collections.Generic;
using Iced.Intel;

namespace Iced.UnitTests.Intel.DecoderTests {
	public static class NonDecodedInstructions {
		public static IEnumerable<(int bitness, string hexBytes, Instruction instruction)> GetTests() {
			foreach (var info in Infos16)
				yield return (16, info.hexBytes, info.instruction);
			foreach (var info in Infos32)
				yield return (32, info.hexBytes, info.instruction);
			foreach (var info in Infos64)
				yield return (64, info.hexBytes, info.instruction);
		}

		public const int Infos16_Count = 49;
		public const int Infos32_Count = 49;
		public const int Infos64_Count = 48;

		static Instruction C16(Instruction instruction) {
			instruction.CodeSize = CodeSize.Code16;
			return instruction;
		}

		static Instruction C32(Instruction instruction) {
			instruction.CodeSize = CodeSize.Code32;
			return instruction;
		}

		static Instruction C64(Instruction instruction) {
			instruction.CodeSize = CodeSize.Code64;
			return instruction;
		}

		public static readonly (string hexBytes, Instruction instruction)[] Infos16 = new (string hexBytes, Instruction instruction)[Infos16_Count] {
			("0F", C16(Instruction.Create(Code.Popw_CS, Register.CS))),
			("9B D9 30", C16(Instruction.Create(Code.Fstenv_m14byte, new MemoryOperand(Register.BX, Register.SI, 1, 0, 0, false, Register.None)))),
			("9B 64 D9 30", C16(Instruction.Create(Code.Fstenv_m14byte, new MemoryOperand(Register.BX, Register.SI, 1, 0, 0, false, Register.FS)))),
			("9B 66 D9 30", C16(Instruction.Create(Code.Fstenv_m28byte, new MemoryOperand(Register.BX, Register.SI, 1, 0, 0, false, Register.None)))),
			("9B 64 66 D9 30", C16(Instruction.Create(Code.Fstenv_m28byte, new MemoryOperand(Register.BX, Register.SI, 1, 0, 0, false, Register.FS)))),
			("9B D9 38", C16(Instruction.Create(Code.Fstcw_m2byte, new MemoryOperand(Register.BX, Register.SI, 1, 0, 0, false, Register.None)))),
			("9B 64 D9 38", C16(Instruction.Create(Code.Fstcw_m2byte, new MemoryOperand(Register.BX, Register.SI, 1, 0, 0, false, Register.FS)))),
			("9B DB E0", C16(Instruction.Create(Code.Feni))),
			("9B DB E1", C16(Instruction.Create(Code.Fdisi))),
			("9B DB E2", C16(Instruction.Create(Code.Fclex))),
			("9B DB E3", C16(Instruction.Create(Code.Finit))),
			("9B DB E4", C16(Instruction.Create(Code.Fsetpm))),
			("9B DD 30", C16(Instruction.Create(Code.Fsave_m94byte, new MemoryOperand(Register.BX, Register.SI, 1, 0, 0, false, Register.None)))),
			("9B 64 DD 30", C16(Instruction.Create(Code.Fsave_m94byte, new MemoryOperand(Register.BX, Register.SI, 1, 0, 0, false, Register.FS)))),
			("9B 66 DD 30", C16(Instruction.Create(Code.Fsave_m108byte, new MemoryOperand(Register.BX, Register.SI, 1, 0, 0, false, Register.None)))),
			("9B 64 66 DD 30", C16(Instruction.Create(Code.Fsave_m108byte, new MemoryOperand(Register.BX, Register.SI, 1, 0, 0, false, Register.FS)))),
			("9B DD 38", C16(Instruction.Create(Code.Fstsw_m2byte, new MemoryOperand(Register.BX, Register.SI, 1, 0, 0, false, Register.None)))),
			("9B 64 DD 38", C16(Instruction.Create(Code.Fstsw_m2byte, new MemoryOperand(Register.BX, Register.SI, 1, 0, 0, false, Register.FS)))),
			("9B DF E0", C16(Instruction.Create(Code.Fstsw_AX, Register.AX))),
			("77", C16(Instruction.CreateDeclareByte(0x77))),
			("77 A9", C16(Instruction.CreateDeclareByte(0x77, 0xA9))),
			("77 A9 CE", C16(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE))),
			("77 A9 CE 9D", C16(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D))),
			("77 A9 CE 9D 55", C16(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55))),
			("77 A9 CE 9D 55 05", C16(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05))),
			("77 A9 CE 9D 55 05 42", C16(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42))),
			("77 A9 CE 9D 55 05 42 6C", C16(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C))),
			("77 A9 CE 9D 55 05 42 6C 86", C16(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86))),
			("77 A9 CE 9D 55 05 42 6C 86 32", C16(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32))),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE", C16(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE))),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE 4F", C16(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F))),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34", C16(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34))),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34 27", C16(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27))),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34 27 AA", C16(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA))),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34 27 AA 08", C16(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08))),
			("A977", C16(Instruction.CreateDeclareWord(0x77A9))),
			("A977 9DCE", C16(Instruction.CreateDeclareWord(0x77A9, 0xCE9D))),
			("A977 9DCE 0555", C16(Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505))),
			("A977 9DCE 0555 6C42", C16(Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C))),
			("A977 9DCE 0555 6C42 3286", C16(Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632))),
			("A977 9DCE 0555 6C42 3286 4FFE", C16(Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F))),
			("A977 9DCE 0555 6C42 3286 4FFE 2734", C16(Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427))),
			("A977 9DCE 0555 6C42 3286 4FFE 2734 08AA", C16(Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xAA08))),
			("9DCEA977", C16(Instruction.CreateDeclareDword(0x77A9CE9D))),
			("9DCEA977 6C420555", C16(Instruction.CreateDeclareDword(0x77A9CE9D, 0x5505426C))),
			("9DCEA977 6C420555 4FFE3286", C16(Instruction.CreateDeclareDword(0x77A9CE9D, 0x5505426C, 0x8632FE4F))),
			("9DCEA977 6C420555 4FFE3286 08AA2734", C16(Instruction.CreateDeclareDword(0x77A9CE9D, 0x5505426C, 0x8632FE4F, 0x3427AA08))),
			("6C4205559DCEA977", C16(Instruction.CreateDeclareQword(0x77A9CE9D5505426C))),
			("6C4205559DCEA977 08AA27344FFE3286", C16(Instruction.CreateDeclareQword(0x77A9CE9D5505426C, 0x8632FE4F3427AA08))),
		};

		public static readonly (string hexBytes, Instruction instruction)[] Infos32 = new (string hexBytes, Instruction instruction)[Infos32_Count] {
			("66 0F", C32(Instruction.Create(Code.Popw_CS, Register.CS))),
			("9B 66 D9 30", C32(Instruction.Create(Code.Fstenv_m14byte, new MemoryOperand(Register.EAX, Register.None, 1, 0, 0, false, Register.None)))),
			("9B 64 66 D9 30", C32(Instruction.Create(Code.Fstenv_m14byte, new MemoryOperand(Register.EAX, Register.None, 1, 0, 0, false, Register.FS)))),
			("9B D9 30", C32(Instruction.Create(Code.Fstenv_m28byte, new MemoryOperand(Register.EAX, Register.None, 1, 0, 0, false, Register.None)))),
			("9B 64 D9 30", C32(Instruction.Create(Code.Fstenv_m28byte, new MemoryOperand(Register.EAX, Register.None, 1, 0, 0, false, Register.FS)))),
			("9B D9 38", C32(Instruction.Create(Code.Fstcw_m2byte, new MemoryOperand(Register.EAX, Register.None, 1, 0, 0, false, Register.None)))),
			("9B 64 D9 38", C32(Instruction.Create(Code.Fstcw_m2byte, new MemoryOperand(Register.EAX, Register.None, 1, 0, 0, false, Register.FS)))),
			("9B DB E0", C32(Instruction.Create(Code.Feni))),
			("9B DB E1", C32(Instruction.Create(Code.Fdisi))),
			("9B DB E2", C32(Instruction.Create(Code.Fclex))),
			("9B DB E3", C32(Instruction.Create(Code.Finit))),
			("9B DB E4", C32(Instruction.Create(Code.Fsetpm))),
			("9B 66 DD 30", C32(Instruction.Create(Code.Fsave_m94byte, new MemoryOperand(Register.EAX, Register.None, 1, 0, 0, false, Register.None)))),
			("9B 64 66 DD 30", C32(Instruction.Create(Code.Fsave_m94byte, new MemoryOperand(Register.EAX, Register.None, 1, 0, 0, false, Register.FS)))),
			("9B DD 30", C32(Instruction.Create(Code.Fsave_m108byte, new MemoryOperand(Register.EAX, Register.None, 1, 0, 0, false, Register.None)))),
			("9B 64 DD 30", C32(Instruction.Create(Code.Fsave_m108byte, new MemoryOperand(Register.EAX, Register.None, 1, 0, 0, false, Register.FS)))),
			("9B DD 38", C32(Instruction.Create(Code.Fstsw_m2byte, new MemoryOperand(Register.EAX, Register.None, 1, 0, 0, false, Register.None)))),
			("9B 64 DD 38", C32(Instruction.Create(Code.Fstsw_m2byte, new MemoryOperand(Register.EAX, Register.None, 1, 0, 0, false, Register.FS)))),
			("9B DF E0", C32(Instruction.Create(Code.Fstsw_AX, Register.AX))),
			("77", C32(Instruction.CreateDeclareByte(0x77))),
			("77 A9", C32(Instruction.CreateDeclareByte(0x77, 0xA9))),
			("77 A9 CE", C32(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE))),
			("77 A9 CE 9D", C32(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D))),
			("77 A9 CE 9D 55", C32(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55))),
			("77 A9 CE 9D 55 05", C32(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05))),
			("77 A9 CE 9D 55 05 42", C32(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42))),
			("77 A9 CE 9D 55 05 42 6C", C32(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C))),
			("77 A9 CE 9D 55 05 42 6C 86", C32(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86))),
			("77 A9 CE 9D 55 05 42 6C 86 32", C32(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32))),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE", C32(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE))),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE 4F", C32(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F))),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34", C32(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34))),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34 27", C32(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27))),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34 27 AA", C32(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA))),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34 27 AA 08", C32(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08))),
			("A977", C32(Instruction.CreateDeclareWord(0x77A9))),
			("A977 9DCE", C32(Instruction.CreateDeclareWord(0x77A9, 0xCE9D))),
			("A977 9DCE 0555", C32(Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505))),
			("A977 9DCE 0555 6C42", C32(Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C))),
			("A977 9DCE 0555 6C42 3286", C32(Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632))),
			("A977 9DCE 0555 6C42 3286 4FFE", C32(Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F))),
			("A977 9DCE 0555 6C42 3286 4FFE 2734", C32(Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427))),
			("A977 9DCE 0555 6C42 3286 4FFE 2734 08AA", C32(Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xAA08))),
			("9DCEA977", C32(Instruction.CreateDeclareDword(0x77A9CE9D))),
			("9DCEA977 6C420555", C32(Instruction.CreateDeclareDword(0x77A9CE9D, 0x5505426C))),
			("9DCEA977 6C420555 4FFE3286", C32(Instruction.CreateDeclareDword(0x77A9CE9D, 0x5505426C, 0x8632FE4F))),
			("9DCEA977 6C420555 4FFE3286 08AA2734", C32(Instruction.CreateDeclareDword(0x77A9CE9D, 0x5505426C, 0x8632FE4F, 0x3427AA08))),
			("6C4205559DCEA977", C32(Instruction.CreateDeclareQword(0x77A9CE9D5505426C))),
			("6C4205559DCEA977 08AA27344FFE3286", C32(Instruction.CreateDeclareQword(0x77A9CE9D5505426C, 0x8632FE4F3427AA08))),
		};

		public static readonly (string hexBytes, Instruction instruction)[] Infos64 = new (string hexBytes, Instruction instruction)[Infos64_Count] {
			("9B 66 D9 30", C64(Instruction.Create(Code.Fstenv_m14byte, new MemoryOperand(Register.RAX, Register.None, 1, 0, 0, false, Register.None)))),
			("9B 64 66 D9 30", C64(Instruction.Create(Code.Fstenv_m14byte, new MemoryOperand(Register.RAX, Register.None, 1, 0, 0, false, Register.FS)))),
			("9B D9 30", C64(Instruction.Create(Code.Fstenv_m28byte, new MemoryOperand(Register.RAX, Register.None, 1, 0, 0, false, Register.None)))),
			("9B 64 D9 30", C64(Instruction.Create(Code.Fstenv_m28byte, new MemoryOperand(Register.RAX, Register.None, 1, 0, 0, false, Register.FS)))),
			("9B D9 38", C64(Instruction.Create(Code.Fstcw_m2byte, new MemoryOperand(Register.RAX, Register.None, 1, 0, 0, false, Register.None)))),
			("9B 64 D9 38", C64(Instruction.Create(Code.Fstcw_m2byte, new MemoryOperand(Register.RAX, Register.None, 1, 0, 0, false, Register.FS)))),
			("9B DB E0", C64(Instruction.Create(Code.Feni))),
			("9B DB E1", C64(Instruction.Create(Code.Fdisi))),
			("9B DB E2", C64(Instruction.Create(Code.Fclex))),
			("9B DB E3", C64(Instruction.Create(Code.Finit))),
			("9B DB E4", C64(Instruction.Create(Code.Fsetpm))),
			("9B 66 DD 30", C64(Instruction.Create(Code.Fsave_m94byte, new MemoryOperand(Register.RAX, Register.None, 1, 0, 0, false, Register.None)))),
			("9B 64 66 DD 30", C64(Instruction.Create(Code.Fsave_m94byte, new MemoryOperand(Register.RAX, Register.None, 1, 0, 0, false, Register.FS)))),
			("9B DD 30", C64(Instruction.Create(Code.Fsave_m108byte, new MemoryOperand(Register.RAX, Register.None, 1, 0, 0, false, Register.None)))),
			("9B 64 DD 30", C64(Instruction.Create(Code.Fsave_m108byte, new MemoryOperand(Register.RAX, Register.None, 1, 0, 0, false, Register.FS)))),
			("9B DD 38", C64(Instruction.Create(Code.Fstsw_m2byte, new MemoryOperand(Register.RAX, Register.None, 1, 0, 0, false, Register.None)))),
			("9B 64 DD 38", C64(Instruction.Create(Code.Fstsw_m2byte, new MemoryOperand(Register.RAX, Register.None, 1, 0, 0, false, Register.FS)))),
			("9B DF E0", C64(Instruction.Create(Code.Fstsw_AX, Register.AX))),
			("77", C64(Instruction.CreateDeclareByte(0x77))),
			("77 A9", C64(Instruction.CreateDeclareByte(0x77, 0xA9))),
			("77 A9 CE", C64(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE))),
			("77 A9 CE 9D", C64(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D))),
			("77 A9 CE 9D 55", C64(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55))),
			("77 A9 CE 9D 55 05", C64(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05))),
			("77 A9 CE 9D 55 05 42", C64(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42))),
			("77 A9 CE 9D 55 05 42 6C", C64(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C))),
			("77 A9 CE 9D 55 05 42 6C 86", C64(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86))),
			("77 A9 CE 9D 55 05 42 6C 86 32", C64(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32))),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE", C64(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE))),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE 4F", C64(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F))),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34", C64(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34))),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34 27", C64(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27))),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34 27 AA", C64(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA))),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34 27 AA 08", C64(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08))),
			("A977", C64(Instruction.CreateDeclareWord(0x77A9))),
			("A977 9DCE", C64(Instruction.CreateDeclareWord(0x77A9, 0xCE9D))),
			("A977 9DCE 0555", C64(Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505))),
			("A977 9DCE 0555 6C42", C64(Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C))),
			("A977 9DCE 0555 6C42 3286", C64(Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632))),
			("A977 9DCE 0555 6C42 3286 4FFE", C64(Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F))),
			("A977 9DCE 0555 6C42 3286 4FFE 2734", C64(Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427))),
			("A977 9DCE 0555 6C42 3286 4FFE 2734 08AA", C64(Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xAA08))),
			("9DCEA977", C64(Instruction.CreateDeclareDword(0x77A9CE9D))),
			("9DCEA977 6C420555", C64(Instruction.CreateDeclareDword(0x77A9CE9D, 0x5505426C))),
			("9DCEA977 6C420555 4FFE3286", C64(Instruction.CreateDeclareDword(0x77A9CE9D, 0x5505426C, 0x8632FE4F))),
			("9DCEA977 6C420555 4FFE3286 08AA2734", C64(Instruction.CreateDeclareDword(0x77A9CE9D, 0x5505426C, 0x8632FE4F, 0x3427AA08))),
			("6C4205559DCEA977", C64(Instruction.CreateDeclareQword(0x77A9CE9D5505426C))),
			("6C4205559DCEA977 08AA27344FFE3286", C64(Instruction.CreateDeclareQword(0x77A9CE9D5505426C, 0x8632FE4F3427AA08))),
		};
	}
}
#endif
