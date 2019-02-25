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

#if !NO_ENCODER
using System;
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

		public const int Infos16_Count = 53;
		public const int Infos32_Count = 53;
		public const int Infos64_Count = 52;

		public static readonly (string hexBytes, Instruction instruction)[] Infos16 = new (string hexBytes, Instruction instruction)[Infos16_Count] {
			("0F", Instruction.Create(Code.Popw_CS, Register.CS)),
			("9B D9 30", Instruction.Create(Code.Fstenv_m14byte, new MemoryOperand(Register.BX, Register.SI, 1, 0, 0, false, Register.None))),
			("9B 64 D9 30", Instruction.Create(Code.Fstenv_m14byte, new MemoryOperand(Register.BX, Register.SI, 1, 0, 0, false, Register.FS))),
			("9B 66 D9 30", Instruction.Create(Code.Fstenv_m28byte, new MemoryOperand(Register.BX, Register.SI, 1, 0, 0, false, Register.None))),
			("9B 64 66 D9 30", Instruction.Create(Code.Fstenv_m28byte, new MemoryOperand(Register.BX, Register.SI, 1, 0, 0, false, Register.FS))),
			("9B D9 38", Instruction.Create(Code.Fstcw_m16, new MemoryOperand(Register.BX, Register.SI, 1, 0, 0, false, Register.None))),
			("9B 64 D9 38", Instruction.Create(Code.Fstcw_m16, new MemoryOperand(Register.BX, Register.SI, 1, 0, 0, false, Register.FS))),
			("9B DB E0", Instruction.Create(Code.Feni)),
			("9B DB E1", Instruction.Create(Code.Fdisi)),
			("9B DB E2", Instruction.Create(Code.Fclex)),
			("9B DB E3", Instruction.Create(Code.Finit)),
			("9B DB E4", Instruction.Create(Code.Fsetpm)),
			("9B DD 30", Instruction.Create(Code.Fsave_m94byte, new MemoryOperand(Register.BX, Register.SI, 1, 0, 0, false, Register.None))),
			("9B 64 DD 30", Instruction.Create(Code.Fsave_m94byte, new MemoryOperand(Register.BX, Register.SI, 1, 0, 0, false, Register.FS))),
			("9B 66 DD 30", Instruction.Create(Code.Fsave_m108byte, new MemoryOperand(Register.BX, Register.SI, 1, 0, 0, false, Register.None))),
			("9B 64 66 DD 30", Instruction.Create(Code.Fsave_m108byte, new MemoryOperand(Register.BX, Register.SI, 1, 0, 0, false, Register.FS))),
			("9B DD 38", Instruction.Create(Code.Fstsw_m16, new MemoryOperand(Register.BX, Register.SI, 1, 0, 0, false, Register.None))),
			("9B 64 DD 38", Instruction.Create(Code.Fstsw_m16, new MemoryOperand(Register.BX, Register.SI, 1, 0, 0, false, Register.FS))),
			("9B DF E0", Instruction.Create(Code.Fstsw_AX, Register.AX)),
			("", Instruction.CreateDeclareByte(Array.Empty<byte>())),
			("77", Instruction.CreateDeclareByte(0x77)),
			("77 A9", Instruction.CreateDeclareByte(0x77, 0xA9)),
			("77 A9 CE", Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE)),
			("77 A9 CE 9D", Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D)),
			("77 A9 CE 9D 55", Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55)),
			("77 A9 CE 9D 55 05", Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05)),
			("77 A9 CE 9D 55 05 42", Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42)),
			("77 A9 CE 9D 55 05 42 6C", Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C)),
			("77 A9 CE 9D 55 05 42 6C 86", Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86)),
			("77 A9 CE 9D 55 05 42 6C 86 32", Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32)),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE", Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE)),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE 4F", Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F)),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34", Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34)),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34 27", Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27)),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34 27 AA", Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA)),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34 27 AA 08", Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08)),
			("", Instruction.CreateDeclareWord(Array.Empty<byte>())),
			("A977", Instruction.CreateDeclareWord(0x77A9)),
			("A977 9DCE", Instruction.CreateDeclareWord(0x77A9, 0xCE9D)),
			("A977 9DCE 0555", Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505)),
			("A977 9DCE 0555 6C42", Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C)),
			("A977 9DCE 0555 6C42 3286", Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632)),
			("A977 9DCE 0555 6C42 3286 4FFE", Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F)),
			("A977 9DCE 0555 6C42 3286 4FFE 2734", Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427)),
			("A977 9DCE 0555 6C42 3286 4FFE 2734 08AA", Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xAA08)),
			("", Instruction.CreateDeclareDword(Array.Empty<byte>())),
			("9DCEA977", Instruction.CreateDeclareDword(0x77A9CE9D)),
			("9DCEA977 6C420555", Instruction.CreateDeclareDword(0x77A9CE9D, 0x5505426C)),
			("9DCEA977 6C420555 4FFE3286", Instruction.CreateDeclareDword(0x77A9CE9D, 0x5505426C, 0x8632FE4F)),
			("9DCEA977 6C420555 4FFE3286 08AA2734", Instruction.CreateDeclareDword(0x77A9CE9D, 0x5505426C, 0x8632FE4F, 0x3427AA08)),
			("", Instruction.CreateDeclareQword(Array.Empty<byte>())),
			("6C4205559DCEA977", Instruction.CreateDeclareQword(0x77A9CE9D5505426C)),
			("6C4205559DCEA977 08AA27344FFE3286", Instruction.CreateDeclareQword(0x77A9CE9D5505426C, 0x8632FE4F3427AA08)),
		};

		public static readonly (string hexBytes, Instruction instruction)[] Infos32 = new (string hexBytes, Instruction instruction)[Infos32_Count] {
			("66 0F", Instruction.Create(Code.Popw_CS, Register.CS)),
			("9B 66 D9 30", Instruction.Create(Code.Fstenv_m14byte, new MemoryOperand(Register.EAX, Register.None, 1, 0, 0, false, Register.None))),
			("9B 64 66 D9 30", Instruction.Create(Code.Fstenv_m14byte, new MemoryOperand(Register.EAX, Register.None, 1, 0, 0, false, Register.FS))),
			("9B D9 30", Instruction.Create(Code.Fstenv_m28byte, new MemoryOperand(Register.EAX, Register.None, 1, 0, 0, false, Register.None))),
			("9B 64 D9 30", Instruction.Create(Code.Fstenv_m28byte, new MemoryOperand(Register.EAX, Register.None, 1, 0, 0, false, Register.FS))),
			("9B D9 38", Instruction.Create(Code.Fstcw_m16, new MemoryOperand(Register.EAX, Register.None, 1, 0, 0, false, Register.None))),
			("9B 64 D9 38", Instruction.Create(Code.Fstcw_m16, new MemoryOperand(Register.EAX, Register.None, 1, 0, 0, false, Register.FS))),
			("9B DB E0", Instruction.Create(Code.Feni)),
			("9B DB E1", Instruction.Create(Code.Fdisi)),
			("9B DB E2", Instruction.Create(Code.Fclex)),
			("9B DB E3", Instruction.Create(Code.Finit)),
			("9B DB E4", Instruction.Create(Code.Fsetpm)),
			("9B 66 DD 30", Instruction.Create(Code.Fsave_m94byte, new MemoryOperand(Register.EAX, Register.None, 1, 0, 0, false, Register.None))),
			("9B 64 66 DD 30", Instruction.Create(Code.Fsave_m94byte, new MemoryOperand(Register.EAX, Register.None, 1, 0, 0, false, Register.FS))),
			("9B DD 30", Instruction.Create(Code.Fsave_m108byte, new MemoryOperand(Register.EAX, Register.None, 1, 0, 0, false, Register.None))),
			("9B 64 DD 30", Instruction.Create(Code.Fsave_m108byte, new MemoryOperand(Register.EAX, Register.None, 1, 0, 0, false, Register.FS))),
			("9B DD 38", Instruction.Create(Code.Fstsw_m16, new MemoryOperand(Register.EAX, Register.None, 1, 0, 0, false, Register.None))),
			("9B 64 DD 38", Instruction.Create(Code.Fstsw_m16, new MemoryOperand(Register.EAX, Register.None, 1, 0, 0, false, Register.FS))),
			("9B DF E0", Instruction.Create(Code.Fstsw_AX, Register.AX)),
			("", Instruction.CreateDeclareByte(Array.Empty<byte>())),
			("77", Instruction.CreateDeclareByte(0x77)),
			("77 A9", Instruction.CreateDeclareByte(0x77, 0xA9)),
			("77 A9 CE", Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE)),
			("77 A9 CE 9D", Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D)),
			("77 A9 CE 9D 55", Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55)),
			("77 A9 CE 9D 55 05", Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05)),
			("77 A9 CE 9D 55 05 42", Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42)),
			("77 A9 CE 9D 55 05 42 6C", Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C)),
			("77 A9 CE 9D 55 05 42 6C 86", Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86)),
			("77 A9 CE 9D 55 05 42 6C 86 32", Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32)),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE", Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE)),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE 4F", Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F)),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34", Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34)),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34 27", Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27)),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34 27 AA", Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA)),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34 27 AA 08", Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08)),
			("", Instruction.CreateDeclareWord(Array.Empty<byte>())),
			("A977", Instruction.CreateDeclareWord(0x77A9)),
			("A977 9DCE", Instruction.CreateDeclareWord(0x77A9, 0xCE9D)),
			("A977 9DCE 0555", Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505)),
			("A977 9DCE 0555 6C42", Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C)),
			("A977 9DCE 0555 6C42 3286", Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632)),
			("A977 9DCE 0555 6C42 3286 4FFE", Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F)),
			("A977 9DCE 0555 6C42 3286 4FFE 2734", Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427)),
			("A977 9DCE 0555 6C42 3286 4FFE 2734 08AA", Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xAA08)),
			("", Instruction.CreateDeclareDword(Array.Empty<byte>())),
			("9DCEA977", Instruction.CreateDeclareDword(0x77A9CE9D)),
			("9DCEA977 6C420555", Instruction.CreateDeclareDword(0x77A9CE9D, 0x5505426C)),
			("9DCEA977 6C420555 4FFE3286", Instruction.CreateDeclareDword(0x77A9CE9D, 0x5505426C, 0x8632FE4F)),
			("9DCEA977 6C420555 4FFE3286 08AA2734", Instruction.CreateDeclareDword(0x77A9CE9D, 0x5505426C, 0x8632FE4F, 0x3427AA08)),
			("", Instruction.CreateDeclareQword(Array.Empty<byte>())),
			("6C4205559DCEA977", Instruction.CreateDeclareQword(0x77A9CE9D5505426C)),
			("6C4205559DCEA977 08AA27344FFE3286", Instruction.CreateDeclareQword(0x77A9CE9D5505426C, 0x8632FE4F3427AA08)),
		};

		public static readonly (string hexBytes, Instruction instruction)[] Infos64 = new (string hexBytes, Instruction instruction)[Infos64_Count] {
			("9B 66 D9 30", Instruction.Create(Code.Fstenv_m14byte, new MemoryOperand(Register.RAX, Register.None, 1, 0, 0, false, Register.None))),
			("9B 64 66 D9 30", Instruction.Create(Code.Fstenv_m14byte, new MemoryOperand(Register.RAX, Register.None, 1, 0, 0, false, Register.FS))),
			("9B D9 30", Instruction.Create(Code.Fstenv_m28byte, new MemoryOperand(Register.RAX, Register.None, 1, 0, 0, false, Register.None))),
			("9B 64 D9 30", Instruction.Create(Code.Fstenv_m28byte, new MemoryOperand(Register.RAX, Register.None, 1, 0, 0, false, Register.FS))),
			("9B D9 38", Instruction.Create(Code.Fstcw_m16, new MemoryOperand(Register.RAX, Register.None, 1, 0, 0, false, Register.None))),
			("9B 64 D9 38", Instruction.Create(Code.Fstcw_m16, new MemoryOperand(Register.RAX, Register.None, 1, 0, 0, false, Register.FS))),
			("9B DB E0", Instruction.Create(Code.Feni)),
			("9B DB E1", Instruction.Create(Code.Fdisi)),
			("9B DB E2", Instruction.Create(Code.Fclex)),
			("9B DB E3", Instruction.Create(Code.Finit)),
			("9B DB E4", Instruction.Create(Code.Fsetpm)),
			("9B 66 DD 30", Instruction.Create(Code.Fsave_m94byte, new MemoryOperand(Register.RAX, Register.None, 1, 0, 0, false, Register.None))),
			("9B 64 66 DD 30", Instruction.Create(Code.Fsave_m94byte, new MemoryOperand(Register.RAX, Register.None, 1, 0, 0, false, Register.FS))),
			("9B DD 30", Instruction.Create(Code.Fsave_m108byte, new MemoryOperand(Register.RAX, Register.None, 1, 0, 0, false, Register.None))),
			("9B 64 DD 30", Instruction.Create(Code.Fsave_m108byte, new MemoryOperand(Register.RAX, Register.None, 1, 0, 0, false, Register.FS))),
			("9B DD 38", Instruction.Create(Code.Fstsw_m16, new MemoryOperand(Register.RAX, Register.None, 1, 0, 0, false, Register.None))),
			("9B 64 DD 38", Instruction.Create(Code.Fstsw_m16, new MemoryOperand(Register.RAX, Register.None, 1, 0, 0, false, Register.FS))),
			("9B DF E0", Instruction.Create(Code.Fstsw_AX, Register.AX)),
			("", Instruction.CreateDeclareByte(Array.Empty<byte>())),
			("77", Instruction.CreateDeclareByte(0x77)),
			("77 A9", Instruction.CreateDeclareByte(0x77, 0xA9)),
			("77 A9 CE", Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE)),
			("77 A9 CE 9D", Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D)),
			("77 A9 CE 9D 55", Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55)),
			("77 A9 CE 9D 55 05", Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05)),
			("77 A9 CE 9D 55 05 42", Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42)),
			("77 A9 CE 9D 55 05 42 6C", Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C)),
			("77 A9 CE 9D 55 05 42 6C 86", Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86)),
			("77 A9 CE 9D 55 05 42 6C 86 32", Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32)),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE", Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE)),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE 4F", Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F)),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34", Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34)),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34 27", Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27)),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34 27 AA", Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA)),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34 27 AA 08", Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08)),
			("", Instruction.CreateDeclareWord(Array.Empty<byte>())),
			("A977", Instruction.CreateDeclareWord(0x77A9)),
			("A977 9DCE", Instruction.CreateDeclareWord(0x77A9, 0xCE9D)),
			("A977 9DCE 0555", Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505)),
			("A977 9DCE 0555 6C42", Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C)),
			("A977 9DCE 0555 6C42 3286", Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632)),
			("A977 9DCE 0555 6C42 3286 4FFE", Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F)),
			("A977 9DCE 0555 6C42 3286 4FFE 2734", Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427)),
			("A977 9DCE 0555 6C42 3286 4FFE 2734 08AA", Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xAA08)),
			("", Instruction.CreateDeclareDword(Array.Empty<byte>())),
			("9DCEA977", Instruction.CreateDeclareDword(0x77A9CE9D)),
			("9DCEA977 6C420555", Instruction.CreateDeclareDword(0x77A9CE9D, 0x5505426C)),
			("9DCEA977 6C420555 4FFE3286", Instruction.CreateDeclareDword(0x77A9CE9D, 0x5505426C, 0x8632FE4F)),
			("9DCEA977 6C420555 4FFE3286 08AA2734", Instruction.CreateDeclareDword(0x77A9CE9D, 0x5505426C, 0x8632FE4F, 0x3427AA08)),
			("", Instruction.CreateDeclareQword(Array.Empty<byte>())),
			("6C4205559DCEA977", Instruction.CreateDeclareQword(0x77A9CE9D5505426C)),
			("6C4205559DCEA977 08AA27344FFE3286", Instruction.CreateDeclareQword(0x77A9CE9D5505426C, 0x8632FE4F3427AA08)),
		};
	}
}
#endif
