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

#if !NO_INSTR_INFO
using System;
using System.Collections.Generic;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.InstructionInfoTests {
	public sealed class RegisterExtensionsTests {
		[Fact]
		void Verify_RegisterInfos() {
			var infos = RegisterExtensions.RegisterInfos;
			for (int i = 0; i < infos.Length; i++)
				Assert.Equal((Register)i, infos[i].Register);
		}

		[Theory]
		[InlineData((Register)(-1))]
		[InlineData((Register)IcedConstants.NumberOfRegisters)]
		void GetInfo_throws_if_invalid_value(Register register) {
			Assert.Throws<ArgumentOutOfRangeException>(() => register.GetInfo());
			Assert.Throws<ArgumentOutOfRangeException>(() => register.GetBaseRegister());
			Assert.Throws<ArgumentOutOfRangeException>(() => register.GetNumber());
			Assert.Throws<ArgumentOutOfRangeException>(() => register.GetFullRegister());
			Assert.Throws<ArgumentOutOfRangeException>(() => register.GetFullRegister32());
			Assert.Throws<ArgumentOutOfRangeException>(() => register.GetSize());
		}

		[Flags]
		enum RegisterFlags {
			None				= 0,
			SegmentRegister		= 1,
			GPR					= 2,
			GPR8				= 4,
			GPR16				= 8,
			GPR32				= 0x10,
			GPR64				= 0x20,
			XMM					= 0x40,
			YMM					= 0x80,
			ZMM					= 0x100,
			VectorRegister		= 0x200,
		}

		[Theory]
		[MemberData(nameof(VerifyRegisterProperties_Data))]
		void VerifyRegisterProperties(Register register, int number, Register baseRegister, Register fullRegister, Register fullRegister32, int size, RegisterFlags flags) {
			var info = register.GetInfo();
			Assert.Equal(register, info.Register);
			Assert.Equal(baseRegister, info.Base);
			Assert.Equal(number, info.Number);
			Assert.Equal(fullRegister, info.FullRegister);
			Assert.Equal(fullRegister32, info.FullRegister32);
			Assert.Equal(size, info.Size);

			Assert.Equal(baseRegister, register.GetBaseRegister());
			Assert.Equal(number, register.GetNumber());
			Assert.Equal(fullRegister, register.GetFullRegister());
			Assert.Equal(fullRegister32, register.GetFullRegister32());
			Assert.Equal(size, register.GetSize());

			Assert.Equal((flags & RegisterFlags.SegmentRegister) != 0, register.IsSegmentRegister());
			Assert.Equal((flags & RegisterFlags.GPR) != 0, register.IsGPR());
			Assert.Equal((flags & RegisterFlags.GPR8) != 0, register.IsGPR8());
			Assert.Equal((flags & RegisterFlags.GPR16) != 0, register.IsGPR16());
			Assert.Equal((flags & RegisterFlags.GPR32) != 0, register.IsGPR32());
			Assert.Equal((flags & RegisterFlags.GPR64) != 0, register.IsGPR64());
			Assert.Equal((flags & RegisterFlags.XMM) != 0, register.IsXMM());
			Assert.Equal((flags & RegisterFlags.YMM) != 0, register.IsYMM());
			Assert.Equal((flags & RegisterFlags.ZMM) != 0, register.IsZMM());
			Assert.Equal((flags & RegisterFlags.VectorRegister) != 0, register.IsVectorRegister());
		}
		public static IEnumerable<object[]> VerifyRegisterProperties_Data {
			get {
				var res = new object[IcedConstants.NumberOfRegisters][] {
					new object[] { Register.None, 0, Register.None, Register.None, Register.None, 0, RegisterFlags.None },

					new object[] { Register.AL, 0, Register.AL, Register.RAX, Register.EAX, 1, RegisterFlags.GPR | RegisterFlags.GPR8 },
					new object[] { Register.CL, 1, Register.AL, Register.RCX, Register.ECX, 1, RegisterFlags.GPR | RegisterFlags.GPR8 },
					new object[] { Register.DL, 2, Register.AL, Register.RDX, Register.EDX, 1, RegisterFlags.GPR | RegisterFlags.GPR8 },
					new object[] { Register.BL, 3, Register.AL, Register.RBX, Register.EBX, 1, RegisterFlags.GPR | RegisterFlags.GPR8 },
					new object[] { Register.AH, 4, Register.AL, Register.RAX, Register.EAX, 1, RegisterFlags.GPR | RegisterFlags.GPR8 },
					new object[] { Register.CH, 5, Register.AL, Register.RCX, Register.ECX, 1, RegisterFlags.GPR | RegisterFlags.GPR8 },
					new object[] { Register.DH, 6, Register.AL, Register.RDX, Register.EDX, 1, RegisterFlags.GPR | RegisterFlags.GPR8 },
					new object[] { Register.BH, 7, Register.AL, Register.RBX, Register.EBX, 1, RegisterFlags.GPR | RegisterFlags.GPR8 },
					new object[] { Register.SPL, 8, Register.AL, Register.RSP, Register.ESP, 1, RegisterFlags.GPR | RegisterFlags.GPR8 },
					new object[] { Register.BPL, 9, Register.AL, Register.RBP, Register.EBP, 1, RegisterFlags.GPR | RegisterFlags.GPR8 },
					new object[] { Register.SIL, 10, Register.AL, Register.RSI, Register.ESI, 1, RegisterFlags.GPR | RegisterFlags.GPR8 },
					new object[] { Register.DIL, 11, Register.AL, Register.RDI, Register.EDI, 1, RegisterFlags.GPR | RegisterFlags.GPR8 },
					new object[] { Register.R8L, 12, Register.AL, Register.R8, Register.R8D, 1, RegisterFlags.GPR | RegisterFlags.GPR8 },
					new object[] { Register.R9L, 13, Register.AL, Register.R9, Register.R9D, 1, RegisterFlags.GPR | RegisterFlags.GPR8 },
					new object[] { Register.R10L, 14, Register.AL, Register.R10, Register.R10D, 1, RegisterFlags.GPR | RegisterFlags.GPR8 },
					new object[] { Register.R11L, 15, Register.AL, Register.R11, Register.R11D, 1, RegisterFlags.GPR | RegisterFlags.GPR8 },
					new object[] { Register.R12L, 16, Register.AL, Register.R12, Register.R12D, 1, RegisterFlags.GPR | RegisterFlags.GPR8 },
					new object[] { Register.R13L, 17, Register.AL, Register.R13, Register.R13D, 1, RegisterFlags.GPR | RegisterFlags.GPR8 },
					new object[] { Register.R14L, 18, Register.AL, Register.R14, Register.R14D, 1, RegisterFlags.GPR | RegisterFlags.GPR8 },
					new object[] { Register.R15L, 19, Register.AL, Register.R15, Register.R15D, 1, RegisterFlags.GPR | RegisterFlags.GPR8 },

					new object[] { Register.AX, 0, Register.AX, Register.RAX, Register.EAX, 2, RegisterFlags.GPR | RegisterFlags.GPR16 },
					new object[] { Register.CX, 1, Register.AX, Register.RCX, Register.ECX, 2, RegisterFlags.GPR | RegisterFlags.GPR16 },
					new object[] { Register.DX, 2, Register.AX, Register.RDX, Register.EDX, 2, RegisterFlags.GPR | RegisterFlags.GPR16 },
					new object[] { Register.BX, 3, Register.AX, Register.RBX, Register.EBX, 2, RegisterFlags.GPR | RegisterFlags.GPR16 },
					new object[] { Register.SP, 4, Register.AX, Register.RSP, Register.ESP, 2, RegisterFlags.GPR | RegisterFlags.GPR16 },
					new object[] { Register.BP, 5, Register.AX, Register.RBP, Register.EBP, 2, RegisterFlags.GPR | RegisterFlags.GPR16 },
					new object[] { Register.SI, 6, Register.AX, Register.RSI, Register.ESI, 2, RegisterFlags.GPR | RegisterFlags.GPR16 },
					new object[] { Register.DI, 7, Register.AX, Register.RDI, Register.EDI, 2, RegisterFlags.GPR | RegisterFlags.GPR16 },
					new object[] { Register.R8W, 8, Register.AX, Register.R8, Register.R8D, 2, RegisterFlags.GPR | RegisterFlags.GPR16 },
					new object[] { Register.R9W, 9, Register.AX, Register.R9, Register.R9D, 2, RegisterFlags.GPR | RegisterFlags.GPR16 },
					new object[] { Register.R10W, 10, Register.AX, Register.R10, Register.R10D, 2, RegisterFlags.GPR | RegisterFlags.GPR16 },
					new object[] { Register.R11W, 11, Register.AX, Register.R11, Register.R11D, 2, RegisterFlags.GPR | RegisterFlags.GPR16 },
					new object[] { Register.R12W, 12, Register.AX, Register.R12, Register.R12D, 2, RegisterFlags.GPR | RegisterFlags.GPR16 },
					new object[] { Register.R13W, 13, Register.AX, Register.R13, Register.R13D, 2, RegisterFlags.GPR | RegisterFlags.GPR16 },
					new object[] { Register.R14W, 14, Register.AX, Register.R14, Register.R14D, 2, RegisterFlags.GPR | RegisterFlags.GPR16 },
					new object[] { Register.R15W, 15, Register.AX, Register.R15, Register.R15D, 2, RegisterFlags.GPR | RegisterFlags.GPR16 },

					new object[] { Register.EAX, 0, Register.EAX, Register.RAX, Register.EAX, 4, RegisterFlags.GPR | RegisterFlags.GPR32 },
					new object[] { Register.ECX, 1, Register.EAX, Register.RCX, Register.ECX, 4, RegisterFlags.GPR | RegisterFlags.GPR32 },
					new object[] { Register.EDX, 2, Register.EAX, Register.RDX, Register.EDX, 4, RegisterFlags.GPR | RegisterFlags.GPR32 },
					new object[] { Register.EBX, 3, Register.EAX, Register.RBX, Register.EBX, 4, RegisterFlags.GPR | RegisterFlags.GPR32 },
					new object[] { Register.ESP, 4, Register.EAX, Register.RSP, Register.ESP, 4, RegisterFlags.GPR | RegisterFlags.GPR32 },
					new object[] { Register.EBP, 5, Register.EAX, Register.RBP, Register.EBP, 4, RegisterFlags.GPR | RegisterFlags.GPR32 },
					new object[] { Register.ESI, 6, Register.EAX, Register.RSI, Register.ESI, 4, RegisterFlags.GPR | RegisterFlags.GPR32 },
					new object[] { Register.EDI, 7, Register.EAX, Register.RDI, Register.EDI, 4, RegisterFlags.GPR | RegisterFlags.GPR32 },
					new object[] { Register.R8D, 8, Register.EAX, Register.R8, Register.R8D, 4, RegisterFlags.GPR | RegisterFlags.GPR32 },
					new object[] { Register.R9D, 9, Register.EAX, Register.R9, Register.R9D, 4, RegisterFlags.GPR | RegisterFlags.GPR32 },
					new object[] { Register.R10D, 10, Register.EAX, Register.R10, Register.R10D, 4, RegisterFlags.GPR | RegisterFlags.GPR32 },
					new object[] { Register.R11D, 11, Register.EAX, Register.R11, Register.R11D, 4, RegisterFlags.GPR | RegisterFlags.GPR32 },
					new object[] { Register.R12D, 12, Register.EAX, Register.R12, Register.R12D, 4, RegisterFlags.GPR | RegisterFlags.GPR32 },
					new object[] { Register.R13D, 13, Register.EAX, Register.R13, Register.R13D, 4, RegisterFlags.GPR | RegisterFlags.GPR32 },
					new object[] { Register.R14D, 14, Register.EAX, Register.R14, Register.R14D, 4, RegisterFlags.GPR | RegisterFlags.GPR32 },
					new object[] { Register.R15D, 15, Register.EAX, Register.R15, Register.R15D, 4, RegisterFlags.GPR | RegisterFlags.GPR32 },

					new object[] { Register.RAX, 0, Register.RAX, Register.RAX, Register.EAX, 8, RegisterFlags.GPR | RegisterFlags.GPR64 },
					new object[] { Register.RCX, 1, Register.RAX, Register.RCX, Register.ECX, 8, RegisterFlags.GPR | RegisterFlags.GPR64 },
					new object[] { Register.RDX, 2, Register.RAX, Register.RDX, Register.EDX, 8, RegisterFlags.GPR | RegisterFlags.GPR64 },
					new object[] { Register.RBX, 3, Register.RAX, Register.RBX, Register.EBX, 8, RegisterFlags.GPR | RegisterFlags.GPR64 },
					new object[] { Register.RSP, 4, Register.RAX, Register.RSP, Register.ESP, 8, RegisterFlags.GPR | RegisterFlags.GPR64 },
					new object[] { Register.RBP, 5, Register.RAX, Register.RBP, Register.EBP, 8, RegisterFlags.GPR | RegisterFlags.GPR64 },
					new object[] { Register.RSI, 6, Register.RAX, Register.RSI, Register.ESI, 8, RegisterFlags.GPR | RegisterFlags.GPR64 },
					new object[] { Register.RDI, 7, Register.RAX, Register.RDI, Register.EDI, 8, RegisterFlags.GPR | RegisterFlags.GPR64 },
					new object[] { Register.R8, 8, Register.RAX, Register.R8, Register.R8D, 8, RegisterFlags.GPR | RegisterFlags.GPR64 },
					new object[] { Register.R9, 9, Register.RAX, Register.R9, Register.R9D, 8, RegisterFlags.GPR | RegisterFlags.GPR64 },
					new object[] { Register.R10, 10, Register.RAX, Register.R10, Register.R10D, 8, RegisterFlags.GPR | RegisterFlags.GPR64 },
					new object[] { Register.R11, 11, Register.RAX, Register.R11, Register.R11D, 8, RegisterFlags.GPR | RegisterFlags.GPR64 },
					new object[] { Register.R12, 12, Register.RAX, Register.R12, Register.R12D, 8, RegisterFlags.GPR | RegisterFlags.GPR64 },
					new object[] { Register.R13, 13, Register.RAX, Register.R13, Register.R13D, 8, RegisterFlags.GPR | RegisterFlags.GPR64 },
					new object[] { Register.R14, 14, Register.RAX, Register.R14, Register.R14D, 8, RegisterFlags.GPR | RegisterFlags.GPR64 },
					new object[] { Register.R15, 15, Register.RAX, Register.R15, Register.R15D, 8, RegisterFlags.GPR | RegisterFlags.GPR64 },

					new object[] { Register.EIP, 0, Register.EIP, Register.RIP, Register.RIP, 4, RegisterFlags.None },
					new object[] { Register.RIP, 1, Register.EIP, Register.RIP, Register.RIP, 8, RegisterFlags.None },

					new object[] { Register.ES, 0, Register.ES, Register.ES, Register.ES, 2, RegisterFlags.SegmentRegister },
					new object[] { Register.CS, 1, Register.ES, Register.CS, Register.CS, 2, RegisterFlags.SegmentRegister },
					new object[] { Register.SS, 2, Register.ES, Register.SS, Register.SS, 2, RegisterFlags.SegmentRegister },
					new object[] { Register.DS, 3, Register.ES, Register.DS, Register.DS, 2, RegisterFlags.SegmentRegister },
					new object[] { Register.FS, 4, Register.ES, Register.FS, Register.FS, 2, RegisterFlags.SegmentRegister },
					new object[] { Register.GS, 5, Register.ES, Register.GS, Register.GS, 2, RegisterFlags.SegmentRegister },

					new object[] { Register.XMM0, 0, Register.XMM0, Register.ZMM0, Register.ZMM0, 16, RegisterFlags.VectorRegister | RegisterFlags.XMM },
					new object[] { Register.XMM1, 1, Register.XMM0, Register.ZMM1, Register.ZMM1, 16, RegisterFlags.VectorRegister | RegisterFlags.XMM },
					new object[] { Register.XMM2, 2, Register.XMM0, Register.ZMM2, Register.ZMM2, 16, RegisterFlags.VectorRegister | RegisterFlags.XMM },
					new object[] { Register.XMM3, 3, Register.XMM0, Register.ZMM3, Register.ZMM3, 16, RegisterFlags.VectorRegister | RegisterFlags.XMM },
					new object[] { Register.XMM4, 4, Register.XMM0, Register.ZMM4, Register.ZMM4, 16, RegisterFlags.VectorRegister | RegisterFlags.XMM },
					new object[] { Register.XMM5, 5, Register.XMM0, Register.ZMM5, Register.ZMM5, 16, RegisterFlags.VectorRegister | RegisterFlags.XMM },
					new object[] { Register.XMM6, 6, Register.XMM0, Register.ZMM6, Register.ZMM6, 16, RegisterFlags.VectorRegister | RegisterFlags.XMM },
					new object[] { Register.XMM7, 7, Register.XMM0, Register.ZMM7, Register.ZMM7, 16, RegisterFlags.VectorRegister | RegisterFlags.XMM },
					new object[] { Register.XMM8, 8, Register.XMM0, Register.ZMM8, Register.ZMM8, 16, RegisterFlags.VectorRegister | RegisterFlags.XMM },
					new object[] { Register.XMM9, 9, Register.XMM0, Register.ZMM9, Register.ZMM9, 16, RegisterFlags.VectorRegister | RegisterFlags.XMM },
					new object[] { Register.XMM10, 10, Register.XMM0, Register.ZMM10, Register.ZMM10, 16, RegisterFlags.VectorRegister | RegisterFlags.XMM },
					new object[] { Register.XMM11, 11, Register.XMM0, Register.ZMM11, Register.ZMM11, 16, RegisterFlags.VectorRegister | RegisterFlags.XMM },
					new object[] { Register.XMM12, 12, Register.XMM0, Register.ZMM12, Register.ZMM12, 16, RegisterFlags.VectorRegister | RegisterFlags.XMM },
					new object[] { Register.XMM13, 13, Register.XMM0, Register.ZMM13, Register.ZMM13, 16, RegisterFlags.VectorRegister | RegisterFlags.XMM },
					new object[] { Register.XMM14, 14, Register.XMM0, Register.ZMM14, Register.ZMM14, 16, RegisterFlags.VectorRegister | RegisterFlags.XMM },
					new object[] { Register.XMM15, 15, Register.XMM0, Register.ZMM15, Register.ZMM15, 16, RegisterFlags.VectorRegister | RegisterFlags.XMM },
					new object[] { Register.XMM16, 16, Register.XMM0, Register.ZMM16, Register.ZMM16, 16, RegisterFlags.VectorRegister | RegisterFlags.XMM },
					new object[] { Register.XMM17, 17, Register.XMM0, Register.ZMM17, Register.ZMM17, 16, RegisterFlags.VectorRegister | RegisterFlags.XMM },
					new object[] { Register.XMM18, 18, Register.XMM0, Register.ZMM18, Register.ZMM18, 16, RegisterFlags.VectorRegister | RegisterFlags.XMM },
					new object[] { Register.XMM19, 19, Register.XMM0, Register.ZMM19, Register.ZMM19, 16, RegisterFlags.VectorRegister | RegisterFlags.XMM },
					new object[] { Register.XMM20, 20, Register.XMM0, Register.ZMM20, Register.ZMM20, 16, RegisterFlags.VectorRegister | RegisterFlags.XMM },
					new object[] { Register.XMM21, 21, Register.XMM0, Register.ZMM21, Register.ZMM21, 16, RegisterFlags.VectorRegister | RegisterFlags.XMM },
					new object[] { Register.XMM22, 22, Register.XMM0, Register.ZMM22, Register.ZMM22, 16, RegisterFlags.VectorRegister | RegisterFlags.XMM },
					new object[] { Register.XMM23, 23, Register.XMM0, Register.ZMM23, Register.ZMM23, 16, RegisterFlags.VectorRegister | RegisterFlags.XMM },
					new object[] { Register.XMM24, 24, Register.XMM0, Register.ZMM24, Register.ZMM24, 16, RegisterFlags.VectorRegister | RegisterFlags.XMM },
					new object[] { Register.XMM25, 25, Register.XMM0, Register.ZMM25, Register.ZMM25, 16, RegisterFlags.VectorRegister | RegisterFlags.XMM },
					new object[] { Register.XMM26, 26, Register.XMM0, Register.ZMM26, Register.ZMM26, 16, RegisterFlags.VectorRegister | RegisterFlags.XMM },
					new object[] { Register.XMM27, 27, Register.XMM0, Register.ZMM27, Register.ZMM27, 16, RegisterFlags.VectorRegister | RegisterFlags.XMM },
					new object[] { Register.XMM28, 28, Register.XMM0, Register.ZMM28, Register.ZMM28, 16, RegisterFlags.VectorRegister | RegisterFlags.XMM },
					new object[] { Register.XMM29, 29, Register.XMM0, Register.ZMM29, Register.ZMM29, 16, RegisterFlags.VectorRegister | RegisterFlags.XMM },
					new object[] { Register.XMM30, 30, Register.XMM0, Register.ZMM30, Register.ZMM30, 16, RegisterFlags.VectorRegister | RegisterFlags.XMM },
					new object[] { Register.XMM31, 31, Register.XMM0, Register.ZMM31, Register.ZMM31, 16, RegisterFlags.VectorRegister | RegisterFlags.XMM },

					new object[] { Register.YMM0, 0, Register.YMM0, Register.ZMM0, Register.ZMM0, 32, RegisterFlags.VectorRegister | RegisterFlags.YMM },
					new object[] { Register.YMM1, 1, Register.YMM0, Register.ZMM1, Register.ZMM1, 32, RegisterFlags.VectorRegister | RegisterFlags.YMM },
					new object[] { Register.YMM2, 2, Register.YMM0, Register.ZMM2, Register.ZMM2, 32, RegisterFlags.VectorRegister | RegisterFlags.YMM },
					new object[] { Register.YMM3, 3, Register.YMM0, Register.ZMM3, Register.ZMM3, 32, RegisterFlags.VectorRegister | RegisterFlags.YMM },
					new object[] { Register.YMM4, 4, Register.YMM0, Register.ZMM4, Register.ZMM4, 32, RegisterFlags.VectorRegister | RegisterFlags.YMM },
					new object[] { Register.YMM5, 5, Register.YMM0, Register.ZMM5, Register.ZMM5, 32, RegisterFlags.VectorRegister | RegisterFlags.YMM },
					new object[] { Register.YMM6, 6, Register.YMM0, Register.ZMM6, Register.ZMM6, 32, RegisterFlags.VectorRegister | RegisterFlags.YMM },
					new object[] { Register.YMM7, 7, Register.YMM0, Register.ZMM7, Register.ZMM7, 32, RegisterFlags.VectorRegister | RegisterFlags.YMM },
					new object[] { Register.YMM8, 8, Register.YMM0, Register.ZMM8, Register.ZMM8, 32, RegisterFlags.VectorRegister | RegisterFlags.YMM },
					new object[] { Register.YMM9, 9, Register.YMM0, Register.ZMM9, Register.ZMM9, 32, RegisterFlags.VectorRegister | RegisterFlags.YMM },
					new object[] { Register.YMM10, 10, Register.YMM0, Register.ZMM10, Register.ZMM10, 32, RegisterFlags.VectorRegister | RegisterFlags.YMM },
					new object[] { Register.YMM11, 11, Register.YMM0, Register.ZMM11, Register.ZMM11, 32, RegisterFlags.VectorRegister | RegisterFlags.YMM },
					new object[] { Register.YMM12, 12, Register.YMM0, Register.ZMM12, Register.ZMM12, 32, RegisterFlags.VectorRegister | RegisterFlags.YMM },
					new object[] { Register.YMM13, 13, Register.YMM0, Register.ZMM13, Register.ZMM13, 32, RegisterFlags.VectorRegister | RegisterFlags.YMM },
					new object[] { Register.YMM14, 14, Register.YMM0, Register.ZMM14, Register.ZMM14, 32, RegisterFlags.VectorRegister | RegisterFlags.YMM },
					new object[] { Register.YMM15, 15, Register.YMM0, Register.ZMM15, Register.ZMM15, 32, RegisterFlags.VectorRegister | RegisterFlags.YMM },
					new object[] { Register.YMM16, 16, Register.YMM0, Register.ZMM16, Register.ZMM16, 32, RegisterFlags.VectorRegister | RegisterFlags.YMM },
					new object[] { Register.YMM17, 17, Register.YMM0, Register.ZMM17, Register.ZMM17, 32, RegisterFlags.VectorRegister | RegisterFlags.YMM },
					new object[] { Register.YMM18, 18, Register.YMM0, Register.ZMM18, Register.ZMM18, 32, RegisterFlags.VectorRegister | RegisterFlags.YMM },
					new object[] { Register.YMM19, 19, Register.YMM0, Register.ZMM19, Register.ZMM19, 32, RegisterFlags.VectorRegister | RegisterFlags.YMM },
					new object[] { Register.YMM20, 20, Register.YMM0, Register.ZMM20, Register.ZMM20, 32, RegisterFlags.VectorRegister | RegisterFlags.YMM },
					new object[] { Register.YMM21, 21, Register.YMM0, Register.ZMM21, Register.ZMM21, 32, RegisterFlags.VectorRegister | RegisterFlags.YMM },
					new object[] { Register.YMM22, 22, Register.YMM0, Register.ZMM22, Register.ZMM22, 32, RegisterFlags.VectorRegister | RegisterFlags.YMM },
					new object[] { Register.YMM23, 23, Register.YMM0, Register.ZMM23, Register.ZMM23, 32, RegisterFlags.VectorRegister | RegisterFlags.YMM },
					new object[] { Register.YMM24, 24, Register.YMM0, Register.ZMM24, Register.ZMM24, 32, RegisterFlags.VectorRegister | RegisterFlags.YMM },
					new object[] { Register.YMM25, 25, Register.YMM0, Register.ZMM25, Register.ZMM25, 32, RegisterFlags.VectorRegister | RegisterFlags.YMM },
					new object[] { Register.YMM26, 26, Register.YMM0, Register.ZMM26, Register.ZMM26, 32, RegisterFlags.VectorRegister | RegisterFlags.YMM },
					new object[] { Register.YMM27, 27, Register.YMM0, Register.ZMM27, Register.ZMM27, 32, RegisterFlags.VectorRegister | RegisterFlags.YMM },
					new object[] { Register.YMM28, 28, Register.YMM0, Register.ZMM28, Register.ZMM28, 32, RegisterFlags.VectorRegister | RegisterFlags.YMM },
					new object[] { Register.YMM29, 29, Register.YMM0, Register.ZMM29, Register.ZMM29, 32, RegisterFlags.VectorRegister | RegisterFlags.YMM },
					new object[] { Register.YMM30, 30, Register.YMM0, Register.ZMM30, Register.ZMM30, 32, RegisterFlags.VectorRegister | RegisterFlags.YMM },
					new object[] { Register.YMM31, 31, Register.YMM0, Register.ZMM31, Register.ZMM31, 32, RegisterFlags.VectorRegister | RegisterFlags.YMM },

					new object[] { Register.ZMM0, 0, Register.ZMM0, Register.ZMM0, Register.ZMM0, 64, RegisterFlags.VectorRegister | RegisterFlags.ZMM },
					new object[] { Register.ZMM1, 1, Register.ZMM0, Register.ZMM1, Register.ZMM1, 64, RegisterFlags.VectorRegister | RegisterFlags.ZMM },
					new object[] { Register.ZMM2, 2, Register.ZMM0, Register.ZMM2, Register.ZMM2, 64, RegisterFlags.VectorRegister | RegisterFlags.ZMM },
					new object[] { Register.ZMM3, 3, Register.ZMM0, Register.ZMM3, Register.ZMM3, 64, RegisterFlags.VectorRegister | RegisterFlags.ZMM },
					new object[] { Register.ZMM4, 4, Register.ZMM0, Register.ZMM4, Register.ZMM4, 64, RegisterFlags.VectorRegister | RegisterFlags.ZMM },
					new object[] { Register.ZMM5, 5, Register.ZMM0, Register.ZMM5, Register.ZMM5, 64, RegisterFlags.VectorRegister | RegisterFlags.ZMM },
					new object[] { Register.ZMM6, 6, Register.ZMM0, Register.ZMM6, Register.ZMM6, 64, RegisterFlags.VectorRegister | RegisterFlags.ZMM },
					new object[] { Register.ZMM7, 7, Register.ZMM0, Register.ZMM7, Register.ZMM7, 64, RegisterFlags.VectorRegister | RegisterFlags.ZMM },
					new object[] { Register.ZMM8, 8, Register.ZMM0, Register.ZMM8, Register.ZMM8, 64, RegisterFlags.VectorRegister | RegisterFlags.ZMM },
					new object[] { Register.ZMM9, 9, Register.ZMM0, Register.ZMM9, Register.ZMM9, 64, RegisterFlags.VectorRegister | RegisterFlags.ZMM },
					new object[] { Register.ZMM10, 10, Register.ZMM0, Register.ZMM10, Register.ZMM10, 64, RegisterFlags.VectorRegister | RegisterFlags.ZMM },
					new object[] { Register.ZMM11, 11, Register.ZMM0, Register.ZMM11, Register.ZMM11, 64, RegisterFlags.VectorRegister | RegisterFlags.ZMM },
					new object[] { Register.ZMM12, 12, Register.ZMM0, Register.ZMM12, Register.ZMM12, 64, RegisterFlags.VectorRegister | RegisterFlags.ZMM },
					new object[] { Register.ZMM13, 13, Register.ZMM0, Register.ZMM13, Register.ZMM13, 64, RegisterFlags.VectorRegister | RegisterFlags.ZMM },
					new object[] { Register.ZMM14, 14, Register.ZMM0, Register.ZMM14, Register.ZMM14, 64, RegisterFlags.VectorRegister | RegisterFlags.ZMM },
					new object[] { Register.ZMM15, 15, Register.ZMM0, Register.ZMM15, Register.ZMM15, 64, RegisterFlags.VectorRegister | RegisterFlags.ZMM },
					new object[] { Register.ZMM16, 16, Register.ZMM0, Register.ZMM16, Register.ZMM16, 64, RegisterFlags.VectorRegister | RegisterFlags.ZMM },
					new object[] { Register.ZMM17, 17, Register.ZMM0, Register.ZMM17, Register.ZMM17, 64, RegisterFlags.VectorRegister | RegisterFlags.ZMM },
					new object[] { Register.ZMM18, 18, Register.ZMM0, Register.ZMM18, Register.ZMM18, 64, RegisterFlags.VectorRegister | RegisterFlags.ZMM },
					new object[] { Register.ZMM19, 19, Register.ZMM0, Register.ZMM19, Register.ZMM19, 64, RegisterFlags.VectorRegister | RegisterFlags.ZMM },
					new object[] { Register.ZMM20, 20, Register.ZMM0, Register.ZMM20, Register.ZMM20, 64, RegisterFlags.VectorRegister | RegisterFlags.ZMM },
					new object[] { Register.ZMM21, 21, Register.ZMM0, Register.ZMM21, Register.ZMM21, 64, RegisterFlags.VectorRegister | RegisterFlags.ZMM },
					new object[] { Register.ZMM22, 22, Register.ZMM0, Register.ZMM22, Register.ZMM22, 64, RegisterFlags.VectorRegister | RegisterFlags.ZMM },
					new object[] { Register.ZMM23, 23, Register.ZMM0, Register.ZMM23, Register.ZMM23, 64, RegisterFlags.VectorRegister | RegisterFlags.ZMM },
					new object[] { Register.ZMM24, 24, Register.ZMM0, Register.ZMM24, Register.ZMM24, 64, RegisterFlags.VectorRegister | RegisterFlags.ZMM },
					new object[] { Register.ZMM25, 25, Register.ZMM0, Register.ZMM25, Register.ZMM25, 64, RegisterFlags.VectorRegister | RegisterFlags.ZMM },
					new object[] { Register.ZMM26, 26, Register.ZMM0, Register.ZMM26, Register.ZMM26, 64, RegisterFlags.VectorRegister | RegisterFlags.ZMM },
					new object[] { Register.ZMM27, 27, Register.ZMM0, Register.ZMM27, Register.ZMM27, 64, RegisterFlags.VectorRegister | RegisterFlags.ZMM },
					new object[] { Register.ZMM28, 28, Register.ZMM0, Register.ZMM28, Register.ZMM28, 64, RegisterFlags.VectorRegister | RegisterFlags.ZMM },
					new object[] { Register.ZMM29, 29, Register.ZMM0, Register.ZMM29, Register.ZMM29, 64, RegisterFlags.VectorRegister | RegisterFlags.ZMM },
					new object[] { Register.ZMM30, 30, Register.ZMM0, Register.ZMM30, Register.ZMM30, 64, RegisterFlags.VectorRegister | RegisterFlags.ZMM },
					new object[] { Register.ZMM31, 31, Register.ZMM0, Register.ZMM31, Register.ZMM31, 64, RegisterFlags.VectorRegister | RegisterFlags.ZMM },

					new object[] { Register.K0, 0, Register.K0, Register.K0, Register.K0, 8, RegisterFlags.None },
					new object[] { Register.K1, 1, Register.K0, Register.K1, Register.K1, 8, RegisterFlags.None },
					new object[] { Register.K2, 2, Register.K0, Register.K2, Register.K2, 8, RegisterFlags.None },
					new object[] { Register.K3, 3, Register.K0, Register.K3, Register.K3, 8, RegisterFlags.None },
					new object[] { Register.K4, 4, Register.K0, Register.K4, Register.K4, 8, RegisterFlags.None },
					new object[] { Register.K5, 5, Register.K0, Register.K5, Register.K5, 8, RegisterFlags.None },
					new object[] { Register.K6, 6, Register.K0, Register.K6, Register.K6, 8, RegisterFlags.None },
					new object[] { Register.K7, 7, Register.K0, Register.K7, Register.K7, 8, RegisterFlags.None },

					new object[] { Register.BND0, 0, Register.BND0, Register.BND0, Register.BND0, 16, RegisterFlags.None },
					new object[] { Register.BND1, 1, Register.BND0, Register.BND1, Register.BND1, 16, RegisterFlags.None },
					new object[] { Register.BND2, 2, Register.BND0, Register.BND2, Register.BND2, 16, RegisterFlags.None },
					new object[] { Register.BND3, 3, Register.BND0, Register.BND3, Register.BND3, 16, RegisterFlags.None },

					new object[] { Register.CR0, 0, Register.CR0, Register.CR0, Register.CR0, 8, RegisterFlags.None },
					new object[] { Register.CR1, 1, Register.CR0, Register.CR1, Register.CR1, 8, RegisterFlags.None },
					new object[] { Register.CR2, 2, Register.CR0, Register.CR2, Register.CR2, 8, RegisterFlags.None },
					new object[] { Register.CR3, 3, Register.CR0, Register.CR3, Register.CR3, 8, RegisterFlags.None },
					new object[] { Register.CR4, 4, Register.CR0, Register.CR4, Register.CR4, 8, RegisterFlags.None },
					new object[] { Register.CR5, 5, Register.CR0, Register.CR5, Register.CR5, 8, RegisterFlags.None },
					new object[] { Register.CR6, 6, Register.CR0, Register.CR6, Register.CR6, 8, RegisterFlags.None },
					new object[] { Register.CR7, 7, Register.CR0, Register.CR7, Register.CR7, 8, RegisterFlags.None },
					new object[] { Register.CR8, 8, Register.CR0, Register.CR8, Register.CR8, 8, RegisterFlags.None },
					new object[] { Register.CR9, 9, Register.CR0, Register.CR9, Register.CR9, 8, RegisterFlags.None },
					new object[] { Register.CR10, 10, Register.CR0, Register.CR10, Register.CR10, 8, RegisterFlags.None },
					new object[] { Register.CR11, 11, Register.CR0, Register.CR11, Register.CR11, 8, RegisterFlags.None },
					new object[] { Register.CR12, 12, Register.CR0, Register.CR12, Register.CR12, 8, RegisterFlags.None },
					new object[] { Register.CR13, 13, Register.CR0, Register.CR13, Register.CR13, 8, RegisterFlags.None },
					new object[] { Register.CR14, 14, Register.CR0, Register.CR14, Register.CR14, 8, RegisterFlags.None },
					new object[] { Register.CR15, 15, Register.CR0, Register.CR15, Register.CR15, 8, RegisterFlags.None },

					new object[] { Register.DR0, 0, Register.DR0, Register.DR0, Register.DR0, 8, RegisterFlags.None },
					new object[] { Register.DR1, 1, Register.DR0, Register.DR1, Register.DR1, 8, RegisterFlags.None },
					new object[] { Register.DR2, 2, Register.DR0, Register.DR2, Register.DR2, 8, RegisterFlags.None },
					new object[] { Register.DR3, 3, Register.DR0, Register.DR3, Register.DR3, 8, RegisterFlags.None },
					new object[] { Register.DR4, 4, Register.DR0, Register.DR4, Register.DR4, 8, RegisterFlags.None },
					new object[] { Register.DR5, 5, Register.DR0, Register.DR5, Register.DR5, 8, RegisterFlags.None },
					new object[] { Register.DR6, 6, Register.DR0, Register.DR6, Register.DR6, 8, RegisterFlags.None },
					new object[] { Register.DR7, 7, Register.DR0, Register.DR7, Register.DR7, 8, RegisterFlags.None },
					new object[] { Register.DR8, 8, Register.DR0, Register.DR8, Register.DR8, 8, RegisterFlags.None },
					new object[] { Register.DR9, 9, Register.DR0, Register.DR9, Register.DR9, 8, RegisterFlags.None },
					new object[] { Register.DR10, 10, Register.DR0, Register.DR10, Register.DR10, 8, RegisterFlags.None },
					new object[] { Register.DR11, 11, Register.DR0, Register.DR11, Register.DR11, 8, RegisterFlags.None },
					new object[] { Register.DR12, 12, Register.DR0, Register.DR12, Register.DR12, 8, RegisterFlags.None },
					new object[] { Register.DR13, 13, Register.DR0, Register.DR13, Register.DR13, 8, RegisterFlags.None },
					new object[] { Register.DR14, 14, Register.DR0, Register.DR14, Register.DR14, 8, RegisterFlags.None },
					new object[] { Register.DR15, 15, Register.DR0, Register.DR15, Register.DR15, 8, RegisterFlags.None },

					new object[] { Register.ST0, 0, Register.ST0, Register.ST0, Register.ST0, 10, RegisterFlags.None },
					new object[] { Register.ST1, 1, Register.ST0, Register.ST1, Register.ST1, 10, RegisterFlags.None },
					new object[] { Register.ST2, 2, Register.ST0, Register.ST2, Register.ST2, 10, RegisterFlags.None },
					new object[] { Register.ST3, 3, Register.ST0, Register.ST3, Register.ST3, 10, RegisterFlags.None },
					new object[] { Register.ST4, 4, Register.ST0, Register.ST4, Register.ST4, 10, RegisterFlags.None },
					new object[] { Register.ST5, 5, Register.ST0, Register.ST5, Register.ST5, 10, RegisterFlags.None },
					new object[] { Register.ST6, 6, Register.ST0, Register.ST6, Register.ST6, 10, RegisterFlags.None },
					new object[] { Register.ST7, 7, Register.ST0, Register.ST7, Register.ST7, 10, RegisterFlags.None },

					new object[] { Register.MM0, 0, Register.MM0, Register.MM0, Register.MM0, 8, RegisterFlags.None },
					new object[] { Register.MM1, 1, Register.MM0, Register.MM1, Register.MM1, 8, RegisterFlags.None },
					new object[] { Register.MM2, 2, Register.MM0, Register.MM2, Register.MM2, 8, RegisterFlags.None },
					new object[] { Register.MM3, 3, Register.MM0, Register.MM3, Register.MM3, 8, RegisterFlags.None },
					new object[] { Register.MM4, 4, Register.MM0, Register.MM4, Register.MM4, 8, RegisterFlags.None },
					new object[] { Register.MM5, 5, Register.MM0, Register.MM5, Register.MM5, 8, RegisterFlags.None },
					new object[] { Register.MM6, 6, Register.MM0, Register.MM6, Register.MM6, 8, RegisterFlags.None },
					new object[] { Register.MM7, 7, Register.MM0, Register.MM7, Register.MM7, 8, RegisterFlags.None },

					new object[] { Register.TR0, 0, Register.TR0, Register.TR0, Register.TR0, 4, RegisterFlags.None },
					new object[] { Register.TR1, 1, Register.TR0, Register.TR1, Register.TR1, 4, RegisterFlags.None },
					new object[] { Register.TR2, 2, Register.TR0, Register.TR2, Register.TR2, 4, RegisterFlags.None },
					new object[] { Register.TR3, 3, Register.TR0, Register.TR3, Register.TR3, 4, RegisterFlags.None },
					new object[] { Register.TR4, 4, Register.TR0, Register.TR4, Register.TR4, 4, RegisterFlags.None },
					new object[] { Register.TR5, 5, Register.TR0, Register.TR5, Register.TR5, 4, RegisterFlags.None },
					new object[] { Register.TR6, 6, Register.TR0, Register.TR6, Register.TR6, 4, RegisterFlags.None },
					new object[] { Register.TR7, 7, Register.TR0, Register.TR7, Register.TR7, 4, RegisterFlags.None },
				};
				for (int i = 0; i < res.Length; i++)
					Assert.Equal((Register)i, (Register)res[i][0]);
				return res;
			}
		}
	}
}
#endif
