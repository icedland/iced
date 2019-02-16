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

using System.Diagnostics;

namespace Iced.Intel {
	/// <summary>
	/// A register
	/// </summary>
	public enum Register {
#pragma warning disable 1591 // Missing XML comment for publicly visible type or member
		None,

		AL,
		CL,
		DL,
		BL,
		AH,
		CH,
		DH,
		BH,
		SPL,
		BPL,
		SIL,
		DIL,
		R8L,
		R9L,
		R10L,
		R11L,
		R12L,
		R13L,
		R14L,
		R15L,

		AX,
		CX,
		DX,
		BX,
		SP,
		BP,
		SI,
		DI,
		R8W,
		R9W,
		R10W,
		R11W,
		R12W,
		R13W,
		R14W,
		R15W,

		EAX,
		ECX,
		EDX,
		EBX,
		ESP,
		EBP,
		ESI,
		EDI,
		R8D,
		R9D,
		R10D,
		R11D,
		R12D,
		R13D,
		R14D,
		R15D,

		RAX,
		RCX,
		RDX,
		RBX,
		RSP,
		RBP,
		RSI,
		RDI,
		R8,
		R9,
		R10,
		R11,
		R12,
		R13,
		R14,
		R15,

		EIP,
		RIP,

		ES,
		CS,
		SS,
		DS,
		FS,
		GS,

		XMM0,
		XMM1,
		XMM2,
		XMM3,
		XMM4,
		XMM5,
		XMM6,
		XMM7,
		XMM8,
		XMM9,
		XMM10,
		XMM11,
		XMM12,
		XMM13,
		XMM14,
		XMM15,
		XMM16,
		XMM17,
		XMM18,
		XMM19,
		XMM20,
		XMM21,
		XMM22,
		XMM23,
		XMM24,
		XMM25,
		XMM26,
		XMM27,
		XMM28,
		XMM29,
		XMM30,
		XMM31,

		YMM0,
		YMM1,
		YMM2,
		YMM3,
		YMM4,
		YMM5,
		YMM6,
		YMM7,
		YMM8,
		YMM9,
		YMM10,
		YMM11,
		YMM12,
		YMM13,
		YMM14,
		YMM15,
		YMM16,
		YMM17,
		YMM18,
		YMM19,
		YMM20,
		YMM21,
		YMM22,
		YMM23,
		YMM24,
		YMM25,
		YMM26,
		YMM27,
		YMM28,
		YMM29,
		YMM30,
		YMM31,

		ZMM0,
		ZMM1,
		ZMM2,
		ZMM3,
		ZMM4,
		ZMM5,
		ZMM6,
		ZMM7,
		ZMM8,
		ZMM9,
		ZMM10,
		ZMM11,
		ZMM12,
		ZMM13,
		ZMM14,
		ZMM15,
		ZMM16,
		ZMM17,
		ZMM18,
		ZMM19,
		ZMM20,
		ZMM21,
		ZMM22,
		ZMM23,
		ZMM24,
		ZMM25,
		ZMM26,
		ZMM27,
		ZMM28,
		ZMM29,
		ZMM30,
		ZMM31,

		K0,
		K1,
		K2,
		K3,
		K4,
		K5,
		K6,
		K7,

		BND0,
		BND1,
		BND2,
		BND3,

		CR0,
		CR1,
		CR2,
		CR3,
		CR4,
		CR5,
		CR6,
		CR7,
		CR8,
		CR9,
		CR10,
		CR11,
		CR12,
		CR13,
		CR14,
		CR15,

		DR0,
		DR1,
		DR2,
		DR3,
		DR4,
		DR5,
		DR6,
		DR7,
		DR8,
		DR9,
		DR10,
		DR11,
		DR12,
		DR13,
		DR14,
		DR15,

		ST0,
		ST1,
		ST2,
		ST3,
		ST4,
		ST5,
		ST6,
		ST7,

		MM0,
		MM1,
		MM2,
		MM3,
		MM4,
		MM5,
		MM6,
		MM7,

		TR0,
		TR1,
		TR2,
		TR3,
		TR4,
		TR5,
		TR6,
		TR7,
#pragma warning restore 1591 // Missing XML comment for publicly visible type or member
	}

#if !NO_INSTR_INFO
	/// <summary>
	/// <see cref="Register"/> extension methods
	/// </summary>
	public static class RegisterExtensions {
		internal static readonly RegisterInfo[] RegisterInfos = new RegisterInfo[DecoderConstants.NumberOfRegisters] {
			new RegisterInfo(Register.None, Register.None, Register.None, 0),

			new RegisterInfo(Register.AL, Register.AL, Register.RAX, 1),
			new RegisterInfo(Register.CL, Register.AL, Register.RCX, 1),
			new RegisterInfo(Register.DL, Register.AL, Register.RDX, 1),
			new RegisterInfo(Register.BL, Register.AL, Register.RBX, 1),
			new RegisterInfo(Register.AH, Register.AL, Register.RAX, 1),
			new RegisterInfo(Register.CH, Register.AL, Register.RCX, 1),
			new RegisterInfo(Register.DH, Register.AL, Register.RDX, 1),
			new RegisterInfo(Register.BH, Register.AL, Register.RBX, 1),
			new RegisterInfo(Register.SPL, Register.AL, Register.RSP, 1),
			new RegisterInfo(Register.BPL, Register.AL, Register.RBP, 1),
			new RegisterInfo(Register.SIL, Register.AL, Register.RSI, 1),
			new RegisterInfo(Register.DIL, Register.AL, Register.RDI, 1),
			new RegisterInfo(Register.R8L, Register.AL, Register.R8, 1),
			new RegisterInfo(Register.R9L, Register.AL, Register.R9, 1),
			new RegisterInfo(Register.R10L, Register.AL, Register.R10, 1),
			new RegisterInfo(Register.R11L, Register.AL, Register.R11, 1),
			new RegisterInfo(Register.R12L, Register.AL, Register.R12, 1),
			new RegisterInfo(Register.R13L, Register.AL, Register.R13, 1),
			new RegisterInfo(Register.R14L, Register.AL, Register.R14, 1),
			new RegisterInfo(Register.R15L, Register.AL, Register.R15, 1),

			new RegisterInfo(Register.AX, Register.AX, Register.RAX, 2),
			new RegisterInfo(Register.CX, Register.AX, Register.RCX, 2),
			new RegisterInfo(Register.DX, Register.AX, Register.RDX, 2),
			new RegisterInfo(Register.BX, Register.AX, Register.RBX, 2),
			new RegisterInfo(Register.SP, Register.AX, Register.RSP, 2),
			new RegisterInfo(Register.BP, Register.AX, Register.RBP, 2),
			new RegisterInfo(Register.SI, Register.AX, Register.RSI, 2),
			new RegisterInfo(Register.DI, Register.AX, Register.RDI, 2),
			new RegisterInfo(Register.R8W, Register.AX, Register.R8, 2),
			new RegisterInfo(Register.R9W, Register.AX, Register.R9, 2),
			new RegisterInfo(Register.R10W, Register.AX, Register.R10, 2),
			new RegisterInfo(Register.R11W, Register.AX, Register.R11, 2),
			new RegisterInfo(Register.R12W, Register.AX, Register.R12, 2),
			new RegisterInfo(Register.R13W, Register.AX, Register.R13, 2),
			new RegisterInfo(Register.R14W, Register.AX, Register.R14, 2),
			new RegisterInfo(Register.R15W, Register.AX, Register.R15, 2),

			new RegisterInfo(Register.EAX, Register.EAX, Register.RAX, 4),
			new RegisterInfo(Register.ECX, Register.EAX, Register.RCX, 4),
			new RegisterInfo(Register.EDX, Register.EAX, Register.RDX, 4),
			new RegisterInfo(Register.EBX, Register.EAX, Register.RBX, 4),
			new RegisterInfo(Register.ESP, Register.EAX, Register.RSP, 4),
			new RegisterInfo(Register.EBP, Register.EAX, Register.RBP, 4),
			new RegisterInfo(Register.ESI, Register.EAX, Register.RSI, 4),
			new RegisterInfo(Register.EDI, Register.EAX, Register.RDI, 4),
			new RegisterInfo(Register.R8D, Register.EAX, Register.R8, 4),
			new RegisterInfo(Register.R9D, Register.EAX, Register.R9, 4),
			new RegisterInfo(Register.R10D, Register.EAX, Register.R10, 4),
			new RegisterInfo(Register.R11D, Register.EAX, Register.R11, 4),
			new RegisterInfo(Register.R12D, Register.EAX, Register.R12, 4),
			new RegisterInfo(Register.R13D, Register.EAX, Register.R13, 4),
			new RegisterInfo(Register.R14D, Register.EAX, Register.R14, 4),
			new RegisterInfo(Register.R15D, Register.EAX, Register.R15, 4),

			new RegisterInfo(Register.RAX, Register.RAX, Register.RAX, 8),
			new RegisterInfo(Register.RCX, Register.RAX, Register.RCX, 8),
			new RegisterInfo(Register.RDX, Register.RAX, Register.RDX, 8),
			new RegisterInfo(Register.RBX, Register.RAX, Register.RBX, 8),
			new RegisterInfo(Register.RSP, Register.RAX, Register.RSP, 8),
			new RegisterInfo(Register.RBP, Register.RAX, Register.RBP, 8),
			new RegisterInfo(Register.RSI, Register.RAX, Register.RSI, 8),
			new RegisterInfo(Register.RDI, Register.RAX, Register.RDI, 8),
			new RegisterInfo(Register.R8, Register.RAX, Register.R8, 8),
			new RegisterInfo(Register.R9, Register.RAX, Register.R9, 8),
			new RegisterInfo(Register.R10, Register.RAX, Register.R10, 8),
			new RegisterInfo(Register.R11, Register.RAX, Register.R11, 8),
			new RegisterInfo(Register.R12, Register.RAX, Register.R12, 8),
			new RegisterInfo(Register.R13, Register.RAX, Register.R13, 8),
			new RegisterInfo(Register.R14, Register.RAX, Register.R14, 8),
			new RegisterInfo(Register.R15, Register.RAX, Register.R15, 8),

			new RegisterInfo(Register.EIP, Register.EIP, Register.RIP, 4),
			new RegisterInfo(Register.RIP, Register.EIP, Register.RIP, 8),

			new RegisterInfo(Register.ES, Register.ES, Register.ES, 2),
			new RegisterInfo(Register.CS, Register.ES, Register.CS, 2),
			new RegisterInfo(Register.SS, Register.ES, Register.SS, 2),
			new RegisterInfo(Register.DS, Register.ES, Register.DS, 2),
			new RegisterInfo(Register.FS, Register.ES, Register.FS, 2),
			new RegisterInfo(Register.GS, Register.ES, Register.GS, 2),

			new RegisterInfo(Register.XMM0, Register.XMM0, Register.ZMM0, 16),
			new RegisterInfo(Register.XMM1, Register.XMM0, Register.ZMM1, 16),
			new RegisterInfo(Register.XMM2, Register.XMM0, Register.ZMM2, 16),
			new RegisterInfo(Register.XMM3, Register.XMM0, Register.ZMM3, 16),
			new RegisterInfo(Register.XMM4, Register.XMM0, Register.ZMM4, 16),
			new RegisterInfo(Register.XMM5, Register.XMM0, Register.ZMM5, 16),
			new RegisterInfo(Register.XMM6, Register.XMM0, Register.ZMM6, 16),
			new RegisterInfo(Register.XMM7, Register.XMM0, Register.ZMM7, 16),
			new RegisterInfo(Register.XMM8, Register.XMM0, Register.ZMM8, 16),
			new RegisterInfo(Register.XMM9, Register.XMM0, Register.ZMM9, 16),
			new RegisterInfo(Register.XMM10, Register.XMM0, Register.ZMM10, 16),
			new RegisterInfo(Register.XMM11, Register.XMM0, Register.ZMM11, 16),
			new RegisterInfo(Register.XMM12, Register.XMM0, Register.ZMM12, 16),
			new RegisterInfo(Register.XMM13, Register.XMM0, Register.ZMM13, 16),
			new RegisterInfo(Register.XMM14, Register.XMM0, Register.ZMM14, 16),
			new RegisterInfo(Register.XMM15, Register.XMM0, Register.ZMM15, 16),
			new RegisterInfo(Register.XMM16, Register.XMM0, Register.ZMM16, 16),
			new RegisterInfo(Register.XMM17, Register.XMM0, Register.ZMM17, 16),
			new RegisterInfo(Register.XMM18, Register.XMM0, Register.ZMM18, 16),
			new RegisterInfo(Register.XMM19, Register.XMM0, Register.ZMM19, 16),
			new RegisterInfo(Register.XMM20, Register.XMM0, Register.ZMM20, 16),
			new RegisterInfo(Register.XMM21, Register.XMM0, Register.ZMM21, 16),
			new RegisterInfo(Register.XMM22, Register.XMM0, Register.ZMM22, 16),
			new RegisterInfo(Register.XMM23, Register.XMM0, Register.ZMM23, 16),
			new RegisterInfo(Register.XMM24, Register.XMM0, Register.ZMM24, 16),
			new RegisterInfo(Register.XMM25, Register.XMM0, Register.ZMM25, 16),
			new RegisterInfo(Register.XMM26, Register.XMM0, Register.ZMM26, 16),
			new RegisterInfo(Register.XMM27, Register.XMM0, Register.ZMM27, 16),
			new RegisterInfo(Register.XMM28, Register.XMM0, Register.ZMM28, 16),
			new RegisterInfo(Register.XMM29, Register.XMM0, Register.ZMM29, 16),
			new RegisterInfo(Register.XMM30, Register.XMM0, Register.ZMM30, 16),
			new RegisterInfo(Register.XMM31, Register.XMM0, Register.ZMM31, 16),

			new RegisterInfo(Register.YMM0, Register.YMM0, Register.ZMM0, 32),
			new RegisterInfo(Register.YMM1, Register.YMM0, Register.ZMM1, 32),
			new RegisterInfo(Register.YMM2, Register.YMM0, Register.ZMM2, 32),
			new RegisterInfo(Register.YMM3, Register.YMM0, Register.ZMM3, 32),
			new RegisterInfo(Register.YMM4, Register.YMM0, Register.ZMM4, 32),
			new RegisterInfo(Register.YMM5, Register.YMM0, Register.ZMM5, 32),
			new RegisterInfo(Register.YMM6, Register.YMM0, Register.ZMM6, 32),
			new RegisterInfo(Register.YMM7, Register.YMM0, Register.ZMM7, 32),
			new RegisterInfo(Register.YMM8, Register.YMM0, Register.ZMM8, 32),
			new RegisterInfo(Register.YMM9, Register.YMM0, Register.ZMM9, 32),
			new RegisterInfo(Register.YMM10, Register.YMM0, Register.ZMM10, 32),
			new RegisterInfo(Register.YMM11, Register.YMM0, Register.ZMM11, 32),
			new RegisterInfo(Register.YMM12, Register.YMM0, Register.ZMM12, 32),
			new RegisterInfo(Register.YMM13, Register.YMM0, Register.ZMM13, 32),
			new RegisterInfo(Register.YMM14, Register.YMM0, Register.ZMM14, 32),
			new RegisterInfo(Register.YMM15, Register.YMM0, Register.ZMM15, 32),
			new RegisterInfo(Register.YMM16, Register.YMM0, Register.ZMM16, 32),
			new RegisterInfo(Register.YMM17, Register.YMM0, Register.ZMM17, 32),
			new RegisterInfo(Register.YMM18, Register.YMM0, Register.ZMM18, 32),
			new RegisterInfo(Register.YMM19, Register.YMM0, Register.ZMM19, 32),
			new RegisterInfo(Register.YMM20, Register.YMM0, Register.ZMM20, 32),
			new RegisterInfo(Register.YMM21, Register.YMM0, Register.ZMM21, 32),
			new RegisterInfo(Register.YMM22, Register.YMM0, Register.ZMM22, 32),
			new RegisterInfo(Register.YMM23, Register.YMM0, Register.ZMM23, 32),
			new RegisterInfo(Register.YMM24, Register.YMM0, Register.ZMM24, 32),
			new RegisterInfo(Register.YMM25, Register.YMM0, Register.ZMM25, 32),
			new RegisterInfo(Register.YMM26, Register.YMM0, Register.ZMM26, 32),
			new RegisterInfo(Register.YMM27, Register.YMM0, Register.ZMM27, 32),
			new RegisterInfo(Register.YMM28, Register.YMM0, Register.ZMM28, 32),
			new RegisterInfo(Register.YMM29, Register.YMM0, Register.ZMM29, 32),
			new RegisterInfo(Register.YMM30, Register.YMM0, Register.ZMM30, 32),
			new RegisterInfo(Register.YMM31, Register.YMM0, Register.ZMM31, 32),

			new RegisterInfo(Register.ZMM0, Register.ZMM0, Register.ZMM0, 64),
			new RegisterInfo(Register.ZMM1, Register.ZMM0, Register.ZMM1, 64),
			new RegisterInfo(Register.ZMM2, Register.ZMM0, Register.ZMM2, 64),
			new RegisterInfo(Register.ZMM3, Register.ZMM0, Register.ZMM3, 64),
			new RegisterInfo(Register.ZMM4, Register.ZMM0, Register.ZMM4, 64),
			new RegisterInfo(Register.ZMM5, Register.ZMM0, Register.ZMM5, 64),
			new RegisterInfo(Register.ZMM6, Register.ZMM0, Register.ZMM6, 64),
			new RegisterInfo(Register.ZMM7, Register.ZMM0, Register.ZMM7, 64),
			new RegisterInfo(Register.ZMM8, Register.ZMM0, Register.ZMM8, 64),
			new RegisterInfo(Register.ZMM9, Register.ZMM0, Register.ZMM9, 64),
			new RegisterInfo(Register.ZMM10, Register.ZMM0, Register.ZMM10, 64),
			new RegisterInfo(Register.ZMM11, Register.ZMM0, Register.ZMM11, 64),
			new RegisterInfo(Register.ZMM12, Register.ZMM0, Register.ZMM12, 64),
			new RegisterInfo(Register.ZMM13, Register.ZMM0, Register.ZMM13, 64),
			new RegisterInfo(Register.ZMM14, Register.ZMM0, Register.ZMM14, 64),
			new RegisterInfo(Register.ZMM15, Register.ZMM0, Register.ZMM15, 64),
			new RegisterInfo(Register.ZMM16, Register.ZMM0, Register.ZMM16, 64),
			new RegisterInfo(Register.ZMM17, Register.ZMM0, Register.ZMM17, 64),
			new RegisterInfo(Register.ZMM18, Register.ZMM0, Register.ZMM18, 64),
			new RegisterInfo(Register.ZMM19, Register.ZMM0, Register.ZMM19, 64),
			new RegisterInfo(Register.ZMM20, Register.ZMM0, Register.ZMM20, 64),
			new RegisterInfo(Register.ZMM21, Register.ZMM0, Register.ZMM21, 64),
			new RegisterInfo(Register.ZMM22, Register.ZMM0, Register.ZMM22, 64),
			new RegisterInfo(Register.ZMM23, Register.ZMM0, Register.ZMM23, 64),
			new RegisterInfo(Register.ZMM24, Register.ZMM0, Register.ZMM24, 64),
			new RegisterInfo(Register.ZMM25, Register.ZMM0, Register.ZMM25, 64),
			new RegisterInfo(Register.ZMM26, Register.ZMM0, Register.ZMM26, 64),
			new RegisterInfo(Register.ZMM27, Register.ZMM0, Register.ZMM27, 64),
			new RegisterInfo(Register.ZMM28, Register.ZMM0, Register.ZMM28, 64),
			new RegisterInfo(Register.ZMM29, Register.ZMM0, Register.ZMM29, 64),
			new RegisterInfo(Register.ZMM30, Register.ZMM0, Register.ZMM30, 64),
			new RegisterInfo(Register.ZMM31, Register.ZMM0, Register.ZMM31, 64),

			new RegisterInfo(Register.K0, Register.K0, Register.K0, 8),
			new RegisterInfo(Register.K1, Register.K0, Register.K1, 8),
			new RegisterInfo(Register.K2, Register.K0, Register.K2, 8),
			new RegisterInfo(Register.K3, Register.K0, Register.K3, 8),
			new RegisterInfo(Register.K4, Register.K0, Register.K4, 8),
			new RegisterInfo(Register.K5, Register.K0, Register.K5, 8),
			new RegisterInfo(Register.K6, Register.K0, Register.K6, 8),
			new RegisterInfo(Register.K7, Register.K0, Register.K7, 8),

			new RegisterInfo(Register.BND0, Register.BND0, Register.BND0, 16),
			new RegisterInfo(Register.BND1, Register.BND0, Register.BND1, 16),
			new RegisterInfo(Register.BND2, Register.BND0, Register.BND2, 16),
			new RegisterInfo(Register.BND3, Register.BND0, Register.BND3, 16),

			new RegisterInfo(Register.CR0, Register.CR0, Register.CR0, 8),
			new RegisterInfo(Register.CR1, Register.CR0, Register.CR1, 8),
			new RegisterInfo(Register.CR2, Register.CR0, Register.CR2, 8),
			new RegisterInfo(Register.CR3, Register.CR0, Register.CR3, 8),
			new RegisterInfo(Register.CR4, Register.CR0, Register.CR4, 8),
			new RegisterInfo(Register.CR5, Register.CR0, Register.CR5, 8),
			new RegisterInfo(Register.CR6, Register.CR0, Register.CR6, 8),
			new RegisterInfo(Register.CR7, Register.CR0, Register.CR7, 8),
			new RegisterInfo(Register.CR8, Register.CR0, Register.CR8, 8),
			new RegisterInfo(Register.CR9, Register.CR0, Register.CR9, 8),
			new RegisterInfo(Register.CR10, Register.CR0, Register.CR10, 8),
			new RegisterInfo(Register.CR11, Register.CR0, Register.CR11, 8),
			new RegisterInfo(Register.CR12, Register.CR0, Register.CR12, 8),
			new RegisterInfo(Register.CR13, Register.CR0, Register.CR13, 8),
			new RegisterInfo(Register.CR14, Register.CR0, Register.CR14, 8),
			new RegisterInfo(Register.CR15, Register.CR0, Register.CR15, 8),

			new RegisterInfo(Register.DR0, Register.DR0, Register.DR0, 8),
			new RegisterInfo(Register.DR1, Register.DR0, Register.DR1, 8),
			new RegisterInfo(Register.DR2, Register.DR0, Register.DR2, 8),
			new RegisterInfo(Register.DR3, Register.DR0, Register.DR3, 8),
			new RegisterInfo(Register.DR4, Register.DR0, Register.DR4, 8),
			new RegisterInfo(Register.DR5, Register.DR0, Register.DR5, 8),
			new RegisterInfo(Register.DR6, Register.DR0, Register.DR6, 8),
			new RegisterInfo(Register.DR7, Register.DR0, Register.DR7, 8),
			new RegisterInfo(Register.DR8, Register.DR0, Register.DR8, 8),
			new RegisterInfo(Register.DR9, Register.DR0, Register.DR9, 8),
			new RegisterInfo(Register.DR10, Register.DR0, Register.DR10, 8),
			new RegisterInfo(Register.DR11, Register.DR0, Register.DR11, 8),
			new RegisterInfo(Register.DR12, Register.DR0, Register.DR12, 8),
			new RegisterInfo(Register.DR13, Register.DR0, Register.DR13, 8),
			new RegisterInfo(Register.DR14, Register.DR0, Register.DR14, 8),
			new RegisterInfo(Register.DR15, Register.DR0, Register.DR15, 8),

			new RegisterInfo(Register.ST0, Register.ST0, Register.ST0, 10),
			new RegisterInfo(Register.ST1, Register.ST0, Register.ST1, 10),
			new RegisterInfo(Register.ST2, Register.ST0, Register.ST2, 10),
			new RegisterInfo(Register.ST3, Register.ST0, Register.ST3, 10),
			new RegisterInfo(Register.ST4, Register.ST0, Register.ST4, 10),
			new RegisterInfo(Register.ST5, Register.ST0, Register.ST5, 10),
			new RegisterInfo(Register.ST6, Register.ST0, Register.ST6, 10),
			new RegisterInfo(Register.ST7, Register.ST0, Register.ST7, 10),

			new RegisterInfo(Register.MM0, Register.MM0, Register.MM0, 8),
			new RegisterInfo(Register.MM1, Register.MM0, Register.MM1, 8),
			new RegisterInfo(Register.MM2, Register.MM0, Register.MM2, 8),
			new RegisterInfo(Register.MM3, Register.MM0, Register.MM3, 8),
			new RegisterInfo(Register.MM4, Register.MM0, Register.MM4, 8),
			new RegisterInfo(Register.MM5, Register.MM0, Register.MM5, 8),
			new RegisterInfo(Register.MM6, Register.MM0, Register.MM6, 8),
			new RegisterInfo(Register.MM7, Register.MM0, Register.MM7, 8),

			new RegisterInfo(Register.TR0, Register.TR0, Register.TR0, 4),
			new RegisterInfo(Register.TR1, Register.TR0, Register.TR1, 4),
			new RegisterInfo(Register.TR2, Register.TR0, Register.TR2, 4),
			new RegisterInfo(Register.TR3, Register.TR0, Register.TR3, 4),
			new RegisterInfo(Register.TR4, Register.TR0, Register.TR4, 4),
			new RegisterInfo(Register.TR5, Register.TR0, Register.TR5, 4),
			new RegisterInfo(Register.TR6, Register.TR0, Register.TR6, 4),
			new RegisterInfo(Register.TR7, Register.TR0, Register.TR7, 4),
		};

		/// <summary>
		/// Gets register info
		/// </summary>
		/// <param name="register">Register</param>
		/// <returns></returns>
		public static RegisterInfo GetInfo(this Register register) {
			var infos = RegisterInfos;
			if ((uint)register >= (uint)infos.Length)
				ThrowHelper.ThrowArgumentOutOfRangeException_register();
			return infos[(int)register];
		}

		/// <summary>
		/// Gets the base register, eg. AL, AX, EAX, RAX, MM0, XMM0, YMM0, ZMM0, ES
		/// </summary>
		/// <param name="register">Register</param>
		/// <returns></returns>
		public static Register GetBaseRegister(this Register register) => register.GetInfo().Base;

		/// <summary>
		/// The register number (index) relative to <see cref="GetBaseRegister(Register)"/>, eg. 0-15, or 0-31, or if 8-bit GPR, 0-19
		/// </summary>
		/// <param name="register">Register</param>
		/// <returns></returns>
		public static int GetNumber(this Register register) => register.GetInfo().Number;

		/// <summary>
		/// Gets the full register that this one is a part of, eg. CL/CH/CX/ECX/RCX -> RCX, XMM11/YMM11/ZMM11 -> ZMM11
		/// </summary>
		/// <param name="register">Register</param>
		/// <returns></returns>
		public static Register GetFullRegister(this Register register) => register.GetInfo().FullRegister;

		/// <summary>
		/// Gets the full register that this one is a part of, except if it's a GPR in which case the 32-bit register is returned,
		/// eg. CL/CH/CX/ECX/RCX -> ECX, XMM11/YMM11/ZMM11 -> ZMM11
		/// </summary>
		/// <param name="register">Register</param>
		/// <returns></returns>
		public static Register GetFullRegister32(this Register register) => register.GetInfo().FullRegister32;

		/// <summary>
		/// Gets the size of the register in bytes
		/// </summary>
		/// <param name="register">Register</param>
		/// <returns></returns>
		public static int GetSize(this Register register) => register.GetInfo().Size;

		/// <summary>
		/// Checks if it's a segment register (ES, CS, SS, DS, FS, GS)
		/// </summary>
		/// <param name="register">Register</param>
		/// <returns></returns>
		public static bool IsSegmentRegister(this Register register) => Register.ES <= register && register <= Register.GS;

		/// <summary>
		/// Checks if it's a general purpose register (AL-R15L, AX-R15W, EAX-R15D, RAX-R15)
		/// </summary>
		/// <param name="register">Register</param>
		/// <returns></returns>
		public static bool IsGPR(this Register register) => Register.AL <= register && register <= Register.R15;

		/// <summary>
		/// Checks if it's an 8-bit general purpose register (AL-R15L)
		/// </summary>
		/// <param name="register">Register</param>
		/// <returns></returns>
		public static bool IsGPR8(this Register register) => Register.AL <= register && register <= Register.R15L;

		/// <summary>
		/// Checks if it's a 16-bit general purpose register (AX-R15W)
		/// </summary>
		/// <param name="register">Register</param>
		/// <returns></returns>
		public static bool IsGPR16(this Register register) => Register.AX <= register && register <= Register.R15W;

		/// <summary>
		/// Checks if it's a 32-bit general purpose register (EAX-R15D)
		/// </summary>
		/// <param name="register">Register</param>
		/// <returns></returns>
		public static bool IsGPR32(this Register register) => Register.EAX <= register && register <= Register.R15D;

		/// <summary>
		/// Checks if it's a 64-bit general purpose register (RAX-R15)
		/// </summary>
		/// <param name="register">Register</param>
		/// <returns></returns>
		public static bool IsGPR64(this Register register) => Register.RAX <= register && register <= Register.R15;

		/// <summary>
		/// Checks if it's a 128-bit vector register (XMM0-XMM31)
		/// </summary>
		/// <param name="register">Register</param>
		/// <returns></returns>
		public static bool IsXMM(this Register register) => Register.XMM0 <= register && register <= Register.XMM0 + InstructionInfoConstants.VMM_count - 1;

		/// <summary>
		/// Checks if it's a 256-bit vector register (YMM0-YMM31)
		/// </summary>
		/// <param name="register">Register</param>
		/// <returns></returns>
		public static bool IsYMM(this Register register) => Register.YMM0 <= register && register <= Register.YMM0 + InstructionInfoConstants.VMM_count - 1;

		/// <summary>
		/// Checks if it's a 512-bit vector register (ZMM0-ZMM31)
		/// </summary>
		/// <param name="register">Register</param>
		/// <returns></returns>
		public static bool IsZMM(this Register register) => Register.ZMM0 <= register && register <= Register.ZMM0 + InstructionInfoConstants.VMM_count - 1;

		/// <summary>
		/// Checks if it's an XMM, YMM or ZMM register
		/// </summary>
		/// <param name="register">Register</param>
		/// <returns></returns>
		public static bool IsVectorRegister(this Register register) => Register.XMM0 <= register && register <= Register.ZMM0 + InstructionInfoConstants.VMM_count - 1;
	}

	/// <summary>
	/// Register info
	/// </summary>
	public readonly struct RegisterInfo {
		readonly byte baseRegister;
		readonly byte fullRegister;
		readonly byte size;
		readonly byte register;

		/// <summary>
		/// Gets the register
		/// </summary>
		public Register Register => (Register)register;

		/// <summary>
		/// Gets the base register, eg. AL, AX, EAX, RAX, MM0, XMM0, YMM0, ZMM0, ES
		/// </summary>
		public Register Base => (Register)baseRegister;

		/// <summary>
		/// The register number (index) relative to <see cref="Base"/>, eg. 0-15, or 0-31, or if 8-bit GPR, 0-19
		/// </summary>
		public int Number => register - baseRegister;

		/// <summary>
		/// The full register that this one is a part of, eg. CL/CH/CX/ECX/RCX -> RCX, XMM11/YMM11/ZMM11 -> ZMM11
		/// </summary>
		public Register FullRegister => (Register)fullRegister;

		/// <summary>
		/// Gets the full register that this one is a part of, except if it's a GPR in which case the 32-bit register is returned,
		/// eg. CL/CH/CX/ECX/RCX -> ECX, XMM11/YMM11/ZMM11 -> ZMM11
		/// </summary>
		public Register FullRegister32 {
			get {
				var fullRegister = (Register)this.fullRegister;
				if (fullRegister.IsGPR()) {
					Debug.Assert(Register.RAX <= fullRegister && fullRegister <= Register.R15);
					return fullRegister - Register.RAX + Register.EAX;
				}
				return fullRegister;
			}
		}

		/// <summary>
		/// Size of the register in bytes
		/// </summary>
		public int Size => size;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="register">Register</param>
		/// <param name="baseRegister">Base register, eg. AL, AX, EAX, RAX, XMM0, YMM0, ZMM0, ES</param>
		/// <param name="fullRegister">Full register, eg. RAX, ZMM0, ES</param>
		/// <param name="size">Size of register in bytes</param>
		public RegisterInfo(Register register, Register baseRegister, Register fullRegister, int size) {
			Debug.Assert(baseRegister <= register);
			Debug.Assert((uint)register <= byte.MaxValue);
			this.register = (byte)register;
			Debug.Assert((uint)baseRegister <= byte.MaxValue);
			this.baseRegister = (byte)baseRegister;
			Debug.Assert((uint)fullRegister <= byte.MaxValue);
			this.fullRegister = (byte)fullRegister;
			Debug.Assert((uint)size <= byte.MaxValue);
			this.size = (byte)size;
		}
	}
#endif
}
