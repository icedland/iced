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

#if !NO_NASM_FORMATTER && !NO_FORMATTER
using System;
using System.Diagnostics;

namespace Iced.Intel.NasmFormatterInternal {
	enum InstrOpKind : byte {
		Register = OpKind.Register,
		NearBranch16 = OpKind.NearBranch16,
		NearBranch32 = OpKind.NearBranch32,
		NearBranch64 = OpKind.NearBranch64,
		FarBranch16 = OpKind.FarBranch16,
		FarBranch32 = OpKind.FarBranch32,
		Immediate8 = OpKind.Immediate8,
		Immediate8_2nd = OpKind.Immediate8_2nd,
		Immediate16 = OpKind.Immediate16,
		Immediate32 = OpKind.Immediate32,
		Immediate64 = OpKind.Immediate64,
		Immediate8to16 = OpKind.Immediate8to16,
		Immediate8to32 = OpKind.Immediate8to32,
		Immediate8to64 = OpKind.Immediate8to64,
		Immediate32to64 = OpKind.Immediate32to64,
		MemorySegSI = OpKind.MemorySegSI,
		MemorySegESI = OpKind.MemorySegESI,
		MemorySegRSI = OpKind.MemorySegRSI,
		MemorySegDI = OpKind.MemorySegDI,
		MemorySegEDI = OpKind.MemorySegEDI,
		MemorySegRDI = OpKind.MemorySegRDI,
		MemoryESDI = OpKind.MemoryESDI,
		MemoryESEDI = OpKind.MemoryESEDI,
		MemoryESRDI = OpKind.MemoryESRDI,
		Memory64 = OpKind.Memory64,
		Memory = OpKind.Memory,

		// Extra opkinds
		Sae,
		RnSae,
		RdSae,
		RuSae,
		RzSae,
		DeclareByte,
		DeclareWord,
		DeclareDword,
		DeclareQword,
	}

	enum MemorySizeInfo {
		None,
		Word,
		Dword,
		Qword,
	}

	enum FarMemorySizeInfo {
		None,
		Word,
		Dword,
	}

	struct InstrOpInfo {
		internal const int TEST_RegisterBits = 8;
		internal const int TEST_MemorySizeBits = (int)InstrOpInfoFlags.MemorySizeBits;

		public string Mnemonic;
		public InstrOpInfoFlags Flags;
		public byte OpCount;
		public InstrOpKind Op0Kind;
		public InstrOpKind Op1Kind;
		public InstrOpKind Op2Kind;
		public InstrOpKind Op3Kind;
		public InstrOpKind Op4Kind;
		public byte Op0Register;
		public byte Op1Register;
		public byte Op2Register;
		public byte Op3Register;
		public byte Op4Register;
		public sbyte Op0Index;
		public sbyte Op1Index;
		public sbyte Op2Index;
		public sbyte Op3Index;
		public sbyte Op4Index;

		public MemorySize MemorySize {
			get => (MemorySize)(((uint)Flags >> (int)InstrOpInfoFlags.MemorySizeShift) & (uint)InstrOpInfoFlags.MemorySizeMask);
			set => Flags = (InstrOpInfoFlags)(((uint)Flags & ~((uint)InstrOpInfoFlags.MemorySizeMask << (int)InstrOpInfoFlags.MemorySizeShift)) |
				(((uint)value & (uint)InstrOpInfoFlags.MemorySizeMask) << (int)InstrOpInfoFlags.MemorySizeShift));
		}

		public Register GetOpRegister(int operand) {
			switch (operand) {
			case 0: return (Register)Op0Register;
			case 1: return (Register)Op1Register;
			case 2: return (Register)Op2Register;
			case 3: return (Register)Op3Register;
			case 4: return (Register)Op4Register;
			default: throw new ArgumentOutOfRangeException(nameof(operand));
			}
		}

		public InstrOpKind GetOpKind(int operand) {
			switch (operand) {
			case 0: return Op0Kind;
			case 1: return Op1Kind;
			case 2: return Op2Kind;
			case 3: return Op3Kind;
			case 4: return Op4Kind;
			default:
				Debug.Assert(Op0Kind == InstrOpKind.DeclareByte || Op0Kind == InstrOpKind.DeclareWord || Op0Kind == InstrOpKind.DeclareDword || Op0Kind == InstrOpKind.DeclareQword);
				return Op0Kind;
			}
		}

		public int GetInstructionIndex(int operand) {
			int instructionOperand;
			switch (operand) {
			case 0: instructionOperand = Op0Index; break;
			case 1: instructionOperand = Op1Index; break;
			case 2: instructionOperand = Op2Index; break;
			case 3: instructionOperand = Op3Index; break;
			case 4: instructionOperand = Op4Index; break;
			default:
				Debug.Assert(Op0Kind == InstrOpKind.DeclareByte || Op0Kind == InstrOpKind.DeclareWord || Op0Kind == InstrOpKind.DeclareDword || Op0Kind == InstrOpKind.DeclareQword);
				instructionOperand = -1;
				break;
			}
			return instructionOperand < 0 ? -1 : instructionOperand;
		}

#if !NO_INSTR_INFO
		public bool TryGetOpAccess(int operand, out OpAccess access) {
			int instructionOperand;
			switch (operand) {
			case 0: instructionOperand = Op0Index; break;
			case 1: instructionOperand = Op1Index; break;
			case 2: instructionOperand = Op2Index; break;
			case 3: instructionOperand = Op3Index; break;
			case 4: instructionOperand = Op4Index; break;
			default:
				Debug.Assert(Op0Kind == InstrOpKind.DeclareByte || Op0Kind == InstrOpKind.DeclareWord || Op0Kind == InstrOpKind.DeclareDword || Op0Kind == InstrOpKind.DeclareQword);
				instructionOperand = Op0Index;
				break;
			}
			if (instructionOperand < InstrInfo.OpAccess_INVALID) {
				access = (OpAccess)(-instructionOperand - 2);
				return true;
			}
			access = OpAccess.None;
			return false;
		}
#endif

		public int GetOperandIndex(int instructionOperand) {
			int index;
			if (instructionOperand == Op0Index)
				index = 0;
			else if (instructionOperand == Op1Index)
				index = 1;
			else if (instructionOperand == Op2Index)
				index = 2;
			else if (instructionOperand == Op3Index)
				index = 3;
			else if (instructionOperand == Op4Index)
				index = 4;
			else
				index = -1;
			return index < OpCount ? index : -1;
		}

		public InstrOpInfo(string mnemonic, ref Instruction instr, InstrOpInfoFlags flags) {
			Debug.Assert(DecoderConstants.MaxOpCount == 5);
			Mnemonic = mnemonic;
			Flags = flags | (InstrOpInfoFlags)((uint)instr.MemorySize << (int)InstrOpInfoFlags.MemorySizeShift);
			Op0Kind = (InstrOpKind)instr.Op0Kind;
			Op1Kind = (InstrOpKind)instr.Op1Kind;
			Op2Kind = (InstrOpKind)instr.Op2Kind;
			Op3Kind = (InstrOpKind)instr.Op3Kind;
			Op4Kind = (InstrOpKind)instr.Op4Kind;
			Debug.Assert(TEST_RegisterBits == 8);
			Op0Register = (byte)instr.Op0Register;
			Debug.Assert(TEST_RegisterBits == 8);
			Op1Register = (byte)instr.Op1Register;
			Debug.Assert(TEST_RegisterBits == 8);
			Op2Register = (byte)instr.Op2Register;
			Debug.Assert(TEST_RegisterBits == 8);
			Op3Register = (byte)instr.Op3Register;
			Debug.Assert(TEST_RegisterBits == 8);
			Op4Register = (byte)instr.Op4Register;
			int opCount = instr.OpCount;
			OpCount = (byte)opCount;
			switch (opCount) {
			case 0:
				Op0Index = InstrInfo.OpAccess_INVALID;
				Op1Index = InstrInfo.OpAccess_INVALID;
				Op2Index = InstrInfo.OpAccess_INVALID;
				Op3Index = InstrInfo.OpAccess_INVALID;
				Op4Index = InstrInfo.OpAccess_INVALID;
				break;

			case 1:
				Op0Index = 0;
				Op1Index = InstrInfo.OpAccess_INVALID;
				Op2Index = InstrInfo.OpAccess_INVALID;
				Op3Index = InstrInfo.OpAccess_INVALID;
				Op4Index = InstrInfo.OpAccess_INVALID;
				break;

			case 2:
				Op0Index = 0;
				Op1Index = 1;
				Op2Index = InstrInfo.OpAccess_INVALID;
				Op3Index = InstrInfo.OpAccess_INVALID;
				Op4Index = InstrInfo.OpAccess_INVALID;
				break;

			case 3:
				Op0Index = 0;
				Op1Index = 1;
				Op2Index = 2;
				Op3Index = InstrInfo.OpAccess_INVALID;
				Op4Index = InstrInfo.OpAccess_INVALID;
				break;

			case 4:
				Op0Index = 0;
				Op1Index = 1;
				Op2Index = 2;
				Op3Index = 3;
				Op4Index = InstrInfo.OpAccess_INVALID;
				break;

			case 5:
				Op0Index = 0;
				Op1Index = 1;
				Op2Index = 2;
				Op3Index = 3;
				Op4Index = 4;
				break;

			default:
				throw new InvalidOperationException();
			}
		}
	}

	abstract class InstrInfo {
		public const int OpAccess_INVALID = -1;
#if !NO_INSTR_INFO
		public const int OpAccess_None = -(int)(OpAccess.None + 2);
		public const int OpAccess_Read = -(int)(OpAccess.Read + 2);
		public const int OpAccess_CondRead = -(int)(OpAccess.CondRead + 2);
		public const int OpAccess_Write = -(int)(OpAccess.Write + 2);
		public const int OpAccess_CondWrite = -(int)(OpAccess.CondWrite + 2);
		public const int OpAccess_ReadWrite = -(int)(OpAccess.ReadWrite + 2);
		public const int OpAccess_ReadCondWrite = -(int)(OpAccess.ReadCondWrite + 2);
		public const int OpAccess_NoMemAccess = -(int)(OpAccess.NoMemAccess + 2);
#else
		public const int OpAccess_None = OpAccess_INVALID;
		public const int OpAccess_Read = OpAccess_INVALID;
		public const int OpAccess_CondRead = OpAccess_INVALID;
		public const int OpAccess_Write = OpAccess_INVALID;
		public const int OpAccess_CondWrite = OpAccess_INVALID;
		public const int OpAccess_ReadWrite = OpAccess_INVALID;
		public const int OpAccess_ReadCondWrite = OpAccess_INVALID;
		public const int OpAccess_NoMemAccess = OpAccess_INVALID;
#endif

		internal readonly Code TEST_Code;
		protected InstrInfo(Code code) => TEST_Code = code;

		public abstract void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info);

		protected static int GetCodeSize(CodeSize codeSize) {
			switch (codeSize) {
			case CodeSize.Code16:	return 16;
			case CodeSize.Code32:	return 32;
			case CodeSize.Code64:	return 64;
			default:
			case CodeSize.Unknown:	return 0;
			}
		}
	}

	sealed class SimpleInstrInfo : InstrInfo {
		readonly string mnemonic;
		readonly InstrOpInfoFlags flags;

		public SimpleInstrInfo(Code code, string mnemonic) : this(code, mnemonic, InstrOpInfoFlags.None) { }

		public SimpleInstrInfo(Code code, string mnemonic, InstrOpInfoFlags flags)
			: base(code) {
			this.mnemonic = mnemonic;
			this.flags = flags;
		}

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) =>
			info = new InstrOpInfo(mnemonic, ref instr, flags);
	}

	sealed class SimpleInstrInfo_mmxmem : InstrInfo {
		readonly string mnemonic;
		readonly InstrOpInfoFlags flags;
		readonly MemorySize memSize;

		public SimpleInstrInfo_mmxmem(Code code, string mnemonic) : this(code, mnemonic, InstrOpInfoFlags.None, MemorySize.Unknown) { }
		public SimpleInstrInfo_mmxmem(Code code, string mnemonic, InstrOpInfoFlags flags) : this(code, mnemonic, flags, MemorySize.Unknown) { }

		public SimpleInstrInfo_mmxmem(Code code, string mnemonic, InstrOpInfoFlags flags, MemorySize memSize)
			: base(code) {
			this.mnemonic = mnemonic;
			this.flags = flags;
			this.memSize = memSize;
		}

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			info = new InstrOpInfo(mnemonic, ref instr, flags);
			if (memSize != MemorySize.Unknown)
				info.MemorySize = memSize;
		}
	}

	sealed class SimpleInstrInfo_SEX1 : InstrInfo {
		readonly int codeSize;
		readonly SignExtendInfo sexInfo;
		readonly string mnemonic;

		public SimpleInstrInfo_SEX1(Code code, int codeSize, SignExtendInfo sexInfo, string mnemonic)
			: base(code) {
			this.codeSize = codeSize;
			this.sexInfo = sexInfo;
			this.mnemonic = mnemonic;
		}

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			var flags = (InstrOpInfoFlags)((int)sexInfo << (int)InstrOpInfoFlags.SignExtendInfoShift);

			int instrCodeSize = GetCodeSize(instr.CodeSize);
			if (codeSize != 0 && instrCodeSize != 0 && instrCodeSize != codeSize) {
				if (codeSize == 16)
					flags |= InstrOpInfoFlags.OpSize16;
				else if (codeSize == 32)
					flags |= InstrOpInfoFlags.OpSize32;
				else
					flags |= InstrOpInfoFlags.OpSize64;
			}

			info = new InstrOpInfo(mnemonic, ref instr, flags);
		}
	}

	sealed class SimpleInstrInfo_SEX1a : InstrInfo {
		readonly int codeSize;
		readonly SignExtendInfo sexInfo;
		readonly string mnemonic;

		public SimpleInstrInfo_SEX1a(Code code, int codeSize, SignExtendInfo sexInfo, string mnemonic)
			: base(code) {
			this.codeSize = codeSize;
			this.sexInfo = sexInfo;
			this.mnemonic = mnemonic;
		}

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			var flags = InstrOpInfoFlags.None;

			bool signExtend = true;
			int instrCodeSize = GetCodeSize(instr.CodeSize);
			if (codeSize != 0 && instrCodeSize != 0 && instrCodeSize != codeSize) {
				if (instrCodeSize == 64)
					flags |= InstrOpInfoFlags.OpSize16;
			}
			else if (codeSize == 16 && instrCodeSize == 16)
				signExtend = false;

			if (signExtend)
				flags |= (InstrOpInfoFlags)((int)sexInfo << (int)InstrOpInfoFlags.SignExtendInfoShift);

			info = new InstrOpInfo(mnemonic, ref instr, flags);
		}
	}

	sealed class SimpleInstrInfo_SEX2 : InstrInfo {
		readonly SignExtendInfo sexInfoReg;
		readonly SignExtendInfo sexInfoMem;
		readonly string mnemonic;
		readonly InstrOpInfoFlags flags;

		public SimpleInstrInfo_SEX2(Code code, SignExtendInfo sexInfo, string mnemonic) : this(code, sexInfo, sexInfo, mnemonic, InstrOpInfoFlags.None) { }
		public SimpleInstrInfo_SEX2(Code code, SignExtendInfo sexInfo, string mnemonic, InstrOpInfoFlags flags) : this(code, sexInfo, sexInfo, mnemonic, flags) { }

		public SimpleInstrInfo_SEX2(Code code, SignExtendInfo sexInfoReg, SignExtendInfo sexInfoMem, string mnemonic, InstrOpInfoFlags flags)
			: base(code) {
			this.sexInfoReg = sexInfoReg;
			this.sexInfoMem = sexInfoMem;
			this.mnemonic = mnemonic;
			this.flags = flags;
		}

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			Debug.Assert(instr.OpCount == 2);
			var sexInfo = instr.Op0Kind == OpKind.Memory || instr.Op1Kind == OpKind.Memory ? sexInfoMem : sexInfoReg;
			var flags = this.flags;
			flags |= (InstrOpInfoFlags)((int)sexInfo << (int)InstrOpInfoFlags.SignExtendInfoShift);
			info = new InstrOpInfo(mnemonic, ref instr, flags);
		}
	}

	sealed class SimpleInstrInfo_SEX3 : InstrInfo {
		readonly SignExtendInfo sexInfo;
		readonly string mnemonic;

		public SimpleInstrInfo_SEX3(Code code, SignExtendInfo sexInfo, string mnemonic)
			: base(code) {
			this.sexInfo = sexInfo;
			this.mnemonic = mnemonic;
		}

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			var flags = (InstrOpInfoFlags)((int)sexInfo << (int)InstrOpInfoFlags.SignExtendInfoShift);
			info = new InstrOpInfo(mnemonic, ref instr, flags);
			Debug.Assert(info.OpCount == 3);
			if (options.UsePseudoOps && info.Op0Kind == InstrOpKind.Register && info.Op1Kind == InstrOpKind.Register && info.Op0Register == info.Op1Register) {
				info.OpCount--;
				info.Op1Kind = info.Op2Kind;
				info.Op1Index = 2;
				info.Op2Index = OpAccess_INVALID;
			}
		}
	}

	sealed class SimpleInstrInfo_AamAad : InstrInfo {
		readonly string mnemonic;

		public SimpleInstrInfo_AamAad(Code code, string mnemonic) : base(code) => this.mnemonic = mnemonic;

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			if (instr.Immediate8 == 10) {
				info = default;
				info.Mnemonic = mnemonic;
			}
			else
				info = new InstrOpInfo(mnemonic, ref instr, InstrOpInfoFlags.None);
		}
	}

	static class StringUtils {
		public static InstrOpInfoFlags GetAddressSizeFlags(OpKind opKind) {
			switch (opKind) {
			case OpKind.MemorySegSI:
			case OpKind.MemorySegDI:
			case OpKind.MemoryESDI:
				return InstrOpInfoFlags.AddrSize16;

			case OpKind.MemorySegESI:
			case OpKind.MemorySegEDI:
			case OpKind.MemoryESEDI:
				return InstrOpInfoFlags.AddrSize32;

			case OpKind.MemorySegRSI:
			case OpKind.MemorySegRDI:
			case OpKind.MemoryESRDI:
				return InstrOpInfoFlags.AddrSize64;

			case OpKind.Register:
			case OpKind.NearBranch16:
			case OpKind.NearBranch32:
			case OpKind.NearBranch64:
			case OpKind.FarBranch16:
			case OpKind.FarBranch32:
			case OpKind.Immediate8:
			case OpKind.Immediate8_2nd:
			case OpKind.Immediate16:
			case OpKind.Immediate32:
			case OpKind.Immediate64:
			case OpKind.Memory64:
			case OpKind.Memory:
			default:
				return 0;
			}
		}
	}

	sealed class SimpleInstrInfo_YD : InstrInfo {
		readonly string mnemonic;

		public SimpleInstrInfo_YD(Code code, string mnemonic) : base(code) => this.mnemonic = mnemonic;

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			var opKind = instr.Op0Kind;
			OpKind shortFormOpKind;
			switch (instr.CodeSize) {
			case CodeSize.Unknown:
				shortFormOpKind = opKind;
				break;
			case CodeSize.Code16:
				shortFormOpKind = OpKind.MemoryESDI;
				break;
			case CodeSize.Code32:
				shortFormOpKind = OpKind.MemoryESEDI;
				break;
			case CodeSize.Code64:
				shortFormOpKind = OpKind.MemoryESRDI;
				break;
			default:
				throw new InvalidOperationException();
			}

			InstrOpInfoFlags flags = 0;
			if (opKind != shortFormOpKind)
				flags |= StringUtils.GetAddressSizeFlags(opKind);
			info = default;
			info.Flags = flags;
			info.Mnemonic = mnemonic;
		}
	}

	sealed class SimpleInstrInfo_DX : InstrInfo {
		readonly string mnemonic;

		public SimpleInstrInfo_DX(Code code, string mnemonic) : base(code) => this.mnemonic = mnemonic;

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			var opKind = instr.Op1Kind;
			OpKind shortFormOpKind;
			switch (instr.CodeSize) {
			case CodeSize.Unknown:
				shortFormOpKind = opKind;
				break;
			case CodeSize.Code16:
				shortFormOpKind = OpKind.MemorySegSI;
				break;
			case CodeSize.Code32:
				shortFormOpKind = OpKind.MemorySegESI;
				break;
			case CodeSize.Code64:
				shortFormOpKind = OpKind.MemorySegRSI;
				break;
			default:
				throw new InvalidOperationException();
			}

			InstrOpInfoFlags flags = 0;
			if (opKind != shortFormOpKind)
				flags |= StringUtils.GetAddressSizeFlags(opKind);
			info = default;
			info.Flags = flags;
			info.Mnemonic = mnemonic;
		}
	}

	sealed class SimpleInstrInfo_YX : InstrInfo {
		readonly string mnemonic;

		public SimpleInstrInfo_YX(Code code, string mnemonic) : base(code) => this.mnemonic = mnemonic;

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			var opKind = instr.Op0Kind;
			OpKind shortFormOpKind;
			switch (instr.CodeSize) {
			case CodeSize.Unknown:
				shortFormOpKind = opKind;
				break;
			case CodeSize.Code16:
				shortFormOpKind = OpKind.MemoryESDI;
				break;
			case CodeSize.Code32:
				shortFormOpKind = OpKind.MemoryESEDI;
				break;
			case CodeSize.Code64:
				shortFormOpKind = OpKind.MemoryESRDI;
				break;
			default:
				throw new InvalidOperationException();
			}

			InstrOpInfoFlags flags = 0;
			if (opKind != shortFormOpKind)
				flags |= StringUtils.GetAddressSizeFlags(opKind);
			info = default;
			info.Flags = flags;
			info.Mnemonic = mnemonic;
		}
	}

	sealed class SimpleInstrInfo_XY : InstrInfo {
		readonly string mnemonic;

		public SimpleInstrInfo_XY(Code code, string mnemonic) : base(code) => this.mnemonic = mnemonic;

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			var opKind = instr.Op1Kind;
			OpKind shortFormOpKind;
			switch (instr.CodeSize) {
			case CodeSize.Unknown:
				shortFormOpKind = opKind;
				break;
			case CodeSize.Code16:
				shortFormOpKind = OpKind.MemoryESDI;
				break;
			case CodeSize.Code32:
				shortFormOpKind = OpKind.MemoryESEDI;
				break;
			case CodeSize.Code64:
				shortFormOpKind = OpKind.MemoryESRDI;
				break;
			default:
				throw new InvalidOperationException();
			}

			InstrOpInfoFlags flags = 0;
			if (opKind != shortFormOpKind)
				flags |= StringUtils.GetAddressSizeFlags(opKind);
			info = default;
			info.Flags = flags;
			info.Mnemonic = mnemonic;
		}
	}

	sealed class SimpleInstrInfo_YA : InstrInfo {
		readonly string mnemonic;

		public SimpleInstrInfo_YA(Code code, string mnemonic) : base(code) => this.mnemonic = mnemonic;

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			var opKind = instr.Op0Kind;
			OpKind shortFormOpKind;
			switch (instr.CodeSize) {
			case CodeSize.Unknown:
				shortFormOpKind = opKind;
				break;
			case CodeSize.Code16:
				shortFormOpKind = OpKind.MemoryESDI;
				break;
			case CodeSize.Code32:
				shortFormOpKind = OpKind.MemoryESEDI;
				break;
			case CodeSize.Code64:
				shortFormOpKind = OpKind.MemoryESRDI;
				break;
			default:
				throw new InvalidOperationException();
			}

			InstrOpInfoFlags flags = 0;
			if (opKind != shortFormOpKind)
				flags |= StringUtils.GetAddressSizeFlags(opKind);
			info = default;
			info.Flags = flags;
			info.Mnemonic = mnemonic;
		}
	}

	sealed class SimpleInstrInfo_AX : InstrInfo {
		readonly string mnemonic;

		public SimpleInstrInfo_AX(Code code, string mnemonic) : base(code) => this.mnemonic = mnemonic;

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			var opKind = instr.Op1Kind;
			OpKind shortFormOpKind;
			switch (instr.CodeSize) {
			case CodeSize.Unknown:
				shortFormOpKind = opKind;
				break;
			case CodeSize.Code16:
				shortFormOpKind = OpKind.MemorySegSI;
				break;
			case CodeSize.Code32:
				shortFormOpKind = OpKind.MemorySegESI;
				break;
			case CodeSize.Code64:
				shortFormOpKind = OpKind.MemorySegRSI;
				break;
			default:
				throw new InvalidOperationException();
			}

			InstrOpInfoFlags flags = 0;
			if (opKind != shortFormOpKind)
				flags |= StringUtils.GetAddressSizeFlags(opKind);
			info = default;
			info.Flags = flags;
			info.Mnemonic = mnemonic;
		}
	}

	sealed class SimpleInstrInfo_AY : InstrInfo {
		readonly string mnemonic;

		public SimpleInstrInfo_AY(Code code, string mnemonic) : base(code) => this.mnemonic = mnemonic;

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			var opKind = instr.Op1Kind;
			OpKind shortFormOpKind;
			switch (instr.CodeSize) {
			case CodeSize.Unknown:
				shortFormOpKind = opKind;
				break;
			case CodeSize.Code16:
				shortFormOpKind = OpKind.MemoryESDI;
				break;
			case CodeSize.Code32:
				shortFormOpKind = OpKind.MemoryESEDI;
				break;
			case CodeSize.Code64:
				shortFormOpKind = OpKind.MemoryESRDI;
				break;
			default:
				throw new InvalidOperationException();
			}

			InstrOpInfoFlags flags = 0;
			if (opKind != shortFormOpKind)
				flags |= StringUtils.GetAddressSizeFlags(opKind);
			info = default;
			info.Flags = flags;
			info.Mnemonic = mnemonic;
		}
	}

	sealed class SimpleInstrInfo_XLAT : InstrInfo {
		readonly string mnemonic;

		public SimpleInstrInfo_XLAT(Code code, string mnemonic) : base(code) => this.mnemonic = mnemonic;

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			Register baseReg;
			switch (instr.CodeSize) {
			case CodeSize.Unknown:
				baseReg = instr.MemoryBase;
				break;
			case CodeSize.Code16:
				baseReg = Register.BX;
				break;
			case CodeSize.Code32:
				baseReg = Register.EBX;
				break;
			case CodeSize.Code64:
				baseReg = Register.RBX;
				break;
			default:
				throw new InvalidOperationException();
			}

			InstrOpInfoFlags flags = 0;
			var memBaseReg = instr.MemoryBase;
			if (memBaseReg != baseReg) {
				if (memBaseReg == Register.BX)
					flags |= InstrOpInfoFlags.AddrSize16;
				else if (memBaseReg == Register.EBX)
					flags |= InstrOpInfoFlags.AddrSize32;
				else if (memBaseReg == Register.RBX)
					flags |= InstrOpInfoFlags.AddrSize64;
			}
			info = default;
			info.Flags = flags;
			info.Mnemonic = mnemonic;
		}
	}

	sealed class SimpleInstrInfo_nop : InstrInfo {
		readonly int codeSize;
		readonly string mnemonic;
		readonly Register register;

		public SimpleInstrInfo_nop(Code code, int codeSize, string mnemonic, Register register)
			: base(code) {
			this.codeSize = codeSize;
			this.mnemonic = mnemonic;
			this.register = register;
		}

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			int instrCodeSize = GetCodeSize(instr.CodeSize);
			if (instrCodeSize == 0 || (instrCodeSize & codeSize) != 0)
				info = new InstrOpInfo(mnemonic, ref instr, InstrOpInfoFlags.None);
			else {
				info = default;
				info.Mnemonic = "xchg";
				info.OpCount = 2;
				Debug.Assert(InstrOpKind.Register == 0);
				//info.Op0Kind = InstrOpKind.Register;
				//info.Op1Kind = InstrOpKind.Register;
				Debug.Assert(InstrOpInfo.TEST_RegisterBits == 8);
				info.Op0Register = (byte)register;
				Debug.Assert(InstrOpInfo.TEST_RegisterBits == 8);
				info.Op1Register = (byte)register;
				if (instr.Op0Register == instr.Op1Register) {
					info.Op0Index = OpAccess_None;
					info.Op1Index = OpAccess_None;
				}
				else {
					info.Op0Index = OpAccess_ReadWrite;
					info.Op1Index = OpAccess_ReadWrite;
				}
			}
		}
	}

	sealed class SimpleInstrInfo_STIG1 : InstrInfo {
		readonly string mnemonic;
		readonly bool pseudoOp;

		public SimpleInstrInfo_STIG1(Code code, string mnemonic) : this(code, mnemonic, false) { }

		public SimpleInstrInfo_STIG1(Code code, string mnemonic, bool pseudoOp)
			: base(code) {
			this.mnemonic = mnemonic;
			this.pseudoOp = pseudoOp;
		}

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			info = default;
			info.Mnemonic = mnemonic;
			Debug.Assert(instr.OpCount == 2);
			Debug.Assert(instr.Op0Kind == OpKind.Register && instr.Op0Register == Register.ST0);
			if (!pseudoOp || !(options.UsePseudoOps && instr.Op1Register == Register.ST1)) {
				info.OpCount = 1;
				Debug.Assert(InstrOpKind.Register == 0);
				//info.Op0Kind = InstrOpKind.Register;
				Debug.Assert(InstrOpInfo.TEST_RegisterBits == 8);
				info.Op0Register = (byte)instr.Op1Register;
				info.Op0Index = 1;
			}
		}
	}

	sealed class SimpleInstrInfo_STIG2 : InstrInfo {
		readonly InstrOpInfoFlags flags;
		readonly string mnemonic;
		readonly bool pseudoOp;

		public SimpleInstrInfo_STIG2(Code code, string mnemonic, bool pseudoOp) : this(code, mnemonic, 0, pseudoOp) { }
		public SimpleInstrInfo_STIG2(Code code, string mnemonic, InstrOpInfoFlags flags) : this(code, mnemonic, flags, false) { }

		SimpleInstrInfo_STIG2(Code code, string mnemonic, InstrOpInfoFlags flags, bool pseudoOp)
			: base(code) {
			this.flags = flags;
			this.mnemonic = mnemonic;
			this.pseudoOp = pseudoOp;
		}

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			info = default;
			info.Flags = flags;
			info.Mnemonic = mnemonic;
			Debug.Assert(instr.OpCount == 2);
			Debug.Assert(instr.Op1Kind == OpKind.Register && instr.Op1Register == Register.ST0);
			if (!pseudoOp || !(options.UsePseudoOps && instr.Op0Register == Register.ST1)) {
				info.OpCount = 1;
				Debug.Assert(InstrOpKind.Register == 0);
				//info.Op0Kind = InstrOpKind.Register;
				Debug.Assert(InstrOpInfo.TEST_RegisterBits == 8);
				info.Op0Register = (byte)instr.Op0Register;
			}
		}
	}

	sealed class SimpleInstrInfo_as : InstrInfo {
		readonly int codeSize;
		readonly string mnemonic;

		public SimpleInstrInfo_as(Code code, int codeSize, string mnemonic)
			: base(code) {
			this.codeSize = codeSize;
			this.mnemonic = mnemonic;
		}

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			InstrOpInfoFlags flags = 0;
			var instrCodeSize = GetCodeSize(instr.CodeSize);
			if (instrCodeSize != 0 && instrCodeSize != codeSize) {
				if (codeSize == 16)
					flags |= InstrOpInfoFlags.AddrSize16;
				else if (codeSize == 32)
					flags |= InstrOpInfoFlags.AddrSize32;
				else
					flags |= InstrOpInfoFlags.AddrSize64;
			}
			info = new InstrOpInfo(mnemonic, ref instr, flags);
		}
	}

	sealed class SimpleInstrInfo_maskmovq : InstrInfo {
		readonly string mnemonic;

		public SimpleInstrInfo_maskmovq(Code code, string mnemonic) : base(code) => this.mnemonic = mnemonic;

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			Debug.Assert(instr.OpCount == 3);

			var instrCodeSize = GetCodeSize(instr.CodeSize);

			int codeSize;
			switch (instr.Op0Kind) {
			case OpKind.MemorySegDI:
				codeSize = 16;
				break;

			case OpKind.MemorySegEDI:
				codeSize = 32;
				break;

			case OpKind.MemorySegRDI:
				codeSize = 64;
				break;

			default:
				codeSize = instrCodeSize;
				break;
			}

			info = default;
			info.Mnemonic = mnemonic;
			info.OpCount = 2;
			info.Op0Kind = (InstrOpKind)instr.Op1Kind;
			info.Op0Index = 1;
			Debug.Assert(InstrOpInfo.TEST_RegisterBits == 8);
			info.Op0Register = (byte)instr.Op1Register;
			info.Op1Kind = (InstrOpKind)instr.Op2Kind;
			info.Op1Index = 2;
			Debug.Assert(InstrOpInfo.TEST_RegisterBits == 8);
			info.Op1Register = (byte)instr.Op2Register;
			if (instrCodeSize != 0 && instrCodeSize != codeSize) {
				if (codeSize == 16)
					info.Flags |= InstrOpInfoFlags.AddrSize16;
				else if (codeSize == 32)
					info.Flags |= InstrOpInfoFlags.AddrSize32;
				else
					info.Flags |= InstrOpInfoFlags.AddrSize64;
			}
		}
	}

	sealed class SimpleInstrInfo_pblendvb : InstrInfo {
		readonly string mnemonic;
		readonly MemorySize memSize;

		public SimpleInstrInfo_pblendvb(Code code, string mnemonic) : this(code, mnemonic, MemorySize.Unknown) { }

		public SimpleInstrInfo_pblendvb(Code code, string mnemonic, MemorySize memSize)
			: base(code) {
			this.mnemonic = mnemonic;
			this.memSize = memSize;
		}

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			info = default;
			Debug.Assert(instr.OpCount == 2);
			info.Mnemonic = mnemonic;
			info.OpCount = 3;
			info.Op0Kind = (InstrOpKind)instr.Op0Kind;
			Debug.Assert(InstrOpInfo.TEST_RegisterBits == 8);
			info.Op0Register = (byte)instr.Op0Register;
			info.Op1Kind = (InstrOpKind)instr.Op1Kind;
			info.Op1Index = 1;
			Debug.Assert(InstrOpInfo.TEST_RegisterBits == 8);
			info.Op1Register = (byte)instr.Op1Register;
			Debug.Assert(InstrOpKind.Register == 0);
			//info.Op2Kind = InstrOpKind.Register;
			Debug.Assert(InstrOpInfo.TEST_RegisterBits == 8);
			info.Op2Register = (byte)Register.XMM0;
			info.MemorySize = memSize;
			info.Op2Index = OpAccess_Read;
		}
	}

	sealed class SimpleInstrInfo_reverse2 : InstrInfo {
		readonly string mnemonic;

		public SimpleInstrInfo_reverse2(Code code, string mnemonic) : base(code) => this.mnemonic = mnemonic;

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			info = default;
			info.Mnemonic = mnemonic;
			Debug.Assert(instr.OpCount == 2);
			info.OpCount = 2;
			info.Op0Kind = (InstrOpKind)instr.Op1Kind;
			info.Op0Index = 1;
			Debug.Assert(InstrOpInfo.TEST_RegisterBits == 8);
			info.Op0Register = (byte)instr.Op1Register;
			info.Op1Kind = (InstrOpKind)instr.Op0Kind;
			Debug.Assert(InstrOpInfo.TEST_RegisterBits == 8);
			info.Op1Register = (byte)instr.Op0Register;
			info.MemorySize = instr.MemorySize;
		}
	}

	sealed class SimpleInstrInfo_OpSize : InstrInfo {
		readonly CodeSize codeSize;
		readonly string[] mnemonics;

		public SimpleInstrInfo_OpSize(Code code, CodeSize codeSize, string mnemonic, string mnemonic16, string mnemonic32, string mnemonic64)
			: base(code) {
			this.codeSize = codeSize;
			mnemonics = new string[4];
			mnemonics[(int)CodeSize.Unknown] = mnemonic;
			mnemonics[(int)CodeSize.Code16] = mnemonic16;
			mnemonics[(int)CodeSize.Code32] = mnemonic32;
			mnemonics[(int)CodeSize.Code64] = mnemonic64;
		}

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			string mnemonic;
			if (instr.CodeSize == codeSize)
				mnemonic = mnemonics[(int)CodeSize.Unknown];
			else
				mnemonic = mnemonics[(int)codeSize];
			info = new InstrOpInfo(mnemonic, ref instr, InstrOpInfoFlags.None);
		}
	}

	sealed class SimpleInstrInfo_OpSize2_bnd : InstrInfo {
		readonly string[] mnemonics;

		public SimpleInstrInfo_OpSize2_bnd(Code code, string mnemonic, string mnemonic16, string mnemonic32, string mnemonic64)
			: base(code) {
			mnemonics = new string[4];
			mnemonics[(int)CodeSize.Unknown] = mnemonic;
			mnemonics[(int)CodeSize.Code16] = mnemonic16;
			mnemonics[(int)CodeSize.Code32] = mnemonic32;
			mnemonics[(int)CodeSize.Code64] = mnemonic64;
		}

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			var flags = InstrOpInfoFlags.None;
			if (instr.HasRepnePrefix)
				flags |= InstrOpInfoFlags.BndPrefix;
			var mnemonic = mnemonics[(int)instr.CodeSize];
			info = new InstrOpInfo(mnemonic, ref instr, flags);
		}
	}

	sealed class SimpleInstrInfo_OpSize3 : InstrInfo {
		readonly int codeSize;
		readonly string mnemonicDefault;
		readonly string mnemonicFull;

		public SimpleInstrInfo_OpSize3(Code code, int codeSize, string mnemonicDefault, string mnemonicFull)
			: base(code) {
			this.codeSize = codeSize;
			this.mnemonicDefault = mnemonicDefault;
			this.mnemonicFull = mnemonicFull;
		}

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			var instrCodeSize = GetCodeSize(instr.CodeSize);
			string mnemonic;
			if (instrCodeSize == 0 || (instrCodeSize & codeSize) != 0)
				mnemonic = mnemonicDefault;
			else
				mnemonic = mnemonicFull;
			info = new InstrOpInfo(mnemonic, ref instr, InstrOpInfoFlags.None);
		}
	}

	sealed class SimpleInstrInfo_os : InstrInfo {
		readonly int codeSize;
		readonly string mnemonic;
		readonly InstrOpInfoFlags flags;

		public SimpleInstrInfo_os(Code code, int codeSize, string mnemonic) : this(code, codeSize, mnemonic, InstrOpInfoFlags.None) { }

		public SimpleInstrInfo_os(Code code, int codeSize, string mnemonic, InstrOpInfoFlags flags)
			: base(code) {
			this.codeSize = codeSize;
			this.mnemonic = mnemonic;
			this.flags = flags;
		}

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			var flags = this.flags;
			int instrCodeSize = GetCodeSize(instr.CodeSize);
			if (instrCodeSize != 0 && instrCodeSize != codeSize) {
				if (codeSize == 16)
					flags |= InstrOpInfoFlags.OpSize16;
				else if (codeSize == 32)
					flags |= InstrOpInfoFlags.OpSize32;
				else
					flags |= InstrOpInfoFlags.OpSize64;
			}
			info = new InstrOpInfo(mnemonic, ref instr, flags);
		}
	}

	sealed class SimpleInstrInfo_os_mem : InstrInfo {
		readonly int codeSize;
		readonly string mnemonic;

		public SimpleInstrInfo_os_mem(Code code, int codeSize, string mnemonic)
			: base(code) {
			this.codeSize = codeSize;
			this.mnemonic = mnemonic;
		}

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			var flags = InstrOpInfoFlags.None;
			int instrCodeSize = GetCodeSize(instr.CodeSize);
			bool hasMemOp = instr.Op0Kind == OpKind.Memory || instr.Op1Kind == OpKind.Memory;
			if (hasMemOp && !(instrCodeSize == 0 || (instrCodeSize != 64 && instrCodeSize == codeSize) || (instrCodeSize == 64 && codeSize == 32))) {
				if (codeSize == 16)
					flags |= InstrOpInfoFlags.OpSize16;
				else if (codeSize == 32)
					flags |= InstrOpInfoFlags.OpSize32;
				else
					flags |= InstrOpInfoFlags.OpSize64;
			}
			info = new InstrOpInfo(mnemonic, ref instr, flags);
		}
	}

	sealed class SimpleInstrInfo_os_mem2 : InstrInfo {
		readonly InstrOpInfoFlags flags;
		readonly int codeSize;
		readonly string mnemonic;

		public SimpleInstrInfo_os_mem2(Code code, int codeSize, string mnemonic, InstrOpInfoFlags flags)
			: base(code) {
			this.flags = flags;
			this.codeSize = codeSize;
			this.mnemonic = mnemonic;
		}

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			var flags = this.flags;
			int instrCodeSize = GetCodeSize(instr.CodeSize);
			if (instrCodeSize != 0 && (instrCodeSize & codeSize) == 0) {
				if (instrCodeSize != 16)
					flags |= InstrOpInfoFlags.OpSize16;
				else
					flags |= InstrOpInfoFlags.OpSize32;
			}
			info = new InstrOpInfo(mnemonic, ref instr, flags);
		}
	}

	sealed class SimpleInstrInfo_os_mem_reg16 : InstrInfo {
		readonly int codeSize;
		readonly string mnemonic;

		public SimpleInstrInfo_os_mem_reg16(Code code, int codeSize, string mnemonic)
			: base(code) {
			this.codeSize = codeSize;
			this.mnemonic = mnemonic;
		}

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			var flags = InstrOpInfoFlags.None;
			int instrCodeSize = GetCodeSize(instr.CodeSize);
			Debug.Assert(instr.OpCount == 1);
			if (instr.Op0Kind == OpKind.Memory) {
				if (!(instrCodeSize == 0 || (instrCodeSize != 64 && instrCodeSize == codeSize) || (instrCodeSize == 64 && codeSize == 32))) {
					if (codeSize == 16)
						flags |= InstrOpInfoFlags.OpSize16;
					else if (codeSize == 32)
						flags |= InstrOpInfoFlags.OpSize32;
					else
						flags |= InstrOpInfoFlags.OpSize64;
				}
			}
			info = new InstrOpInfo(mnemonic, ref instr, flags);
			if (instr.Op0Kind == OpKind.Register) {
				var reg = (Register)info.Op0Register;
				int regSize = 0;
				if (Register.AX <= reg && reg <= Register.R15W)
					regSize = 16;
				else if (Register.EAX <= reg && reg <= Register.R15D) {
					regSize = 32;
					reg = reg - Register.EAX + Register.AX;
				}
				else if (Register.RAX <= reg && reg <= Register.R15) {
					regSize = 64;
					reg = reg - Register.RAX + Register.AX;
				}
				Debug.Assert(regSize != 0);
				if (regSize != 0) {
					Debug.Assert(InstrOpInfo.TEST_RegisterBits == 8);
					info.Op0Register = (byte)reg;
					if (!((instrCodeSize != 64 && instrCodeSize == regSize) || (instrCodeSize == 64 && regSize == 32))) {
						if (codeSize == 16)
							info.Flags |= InstrOpInfoFlags.OpSize16;
						else if (codeSize == 32)
							info.Flags |= InstrOpInfoFlags.OpSize32;
						else
							info.Flags |= InstrOpInfoFlags.OpSize64;
					}
				}
			}
		}
	}

	sealed class SimpleInstrInfo_os_jcc : InstrInfo {
		readonly int codeSize;
		readonly string mnemonic;
		readonly InstrOpInfoFlags flags;

		public SimpleInstrInfo_os_jcc(Code code, int codeSize, string mnemonic) : this(code, codeSize, mnemonic, InstrOpInfoFlags.None) { }

		public SimpleInstrInfo_os_jcc(Code code, int codeSize, string mnemonic, InstrOpInfoFlags flags)
			: base(code) {
			this.codeSize = codeSize;
			this.mnemonic = mnemonic;
			this.flags = flags;
		}

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			var flags = this.flags;
			int instrCodeSize = GetCodeSize(instr.CodeSize);
			if (flags != InstrOpInfoFlags.None) {
				if (instrCodeSize != 0 && instrCodeSize != codeSize) {
					if (codeSize == 16)
						flags |= InstrOpInfoFlags.OpSize16;
					else if (codeSize == 32)
						flags |= InstrOpInfoFlags.OpSize32;
					else
						flags |= InstrOpInfoFlags.OpSize64;
				}
			}
			else {
				var branchInfo = BranchSizeInfo.Near;
				if (instrCodeSize != 0 && instrCodeSize != codeSize) {
					if (codeSize == 16)
						branchInfo = BranchSizeInfo.NearWord;
					else if (codeSize == 32)
						branchInfo = BranchSizeInfo.NearDword;
				}
				flags |= (InstrOpInfoFlags)((int)branchInfo << (int)InstrOpInfoFlags.BranchSizeInfoShift);
			}
			if (instr.HasRepnePrefix)
				flags |= InstrOpInfoFlags.BndPrefix;
			info = new InstrOpInfo(mnemonic, ref instr, flags);
		}
	}

	sealed class SimpleInstrInfo_os_loop : InstrInfo {
		readonly int codeSize;
		readonly Register reg;
		readonly string mnemonic;

		public SimpleInstrInfo_os_loop(Code code, int codeSize, Register reg, string mnemonic)
			: base(code) {
			this.codeSize = codeSize;
			this.reg = reg;
			this.mnemonic = mnemonic;
		}

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			var flags = InstrOpInfoFlags.None;
			int instrCodeSize = GetCodeSize(instr.CodeSize);
			Register expectedReg;
			switch (instrCodeSize) {
			case 0:
				expectedReg = reg;
				break;
			case 16:
				expectedReg = Register.CX;
				break;
			case 32:
				expectedReg = Register.ECX;
				break;
			case 64:
				expectedReg = Register.RCX;
				break;
			default:
				throw new InvalidOperationException();
			}
			bool addReg = expectedReg != reg;
			if (instrCodeSize != 0 && instrCodeSize != codeSize) {
				if (codeSize == 16)
					flags |= InstrOpInfoFlags.OpSize16;
				else if (codeSize == 32)
					flags |= InstrOpInfoFlags.OpSize32;
				else
					flags |= InstrOpInfoFlags.OpSize64;
			}
			info = new InstrOpInfo(mnemonic, ref instr, flags);
			if (addReg) {
				Debug.Assert(info.OpCount == 1);
				info.OpCount = 2;
				info.Op1Kind = InstrOpKind.Register;
				info.Op1Index = OpAccess_ReadWrite;
				Debug.Assert(InstrOpInfo.TEST_RegisterBits == 8);
				info.Op1Register = (byte)reg;
			}
		}
	}

	sealed class SimpleInstrInfo_os_call : InstrInfo {
		readonly int codeSize;
		readonly string mnemonic;
		readonly bool canHaveBndPrefix;

		public SimpleInstrInfo_os_call(Code code, int codeSize, string mnemonic, bool canHaveBndPrefix = false)
			: base(code) {
			this.codeSize = codeSize;
			this.mnemonic = mnemonic;
			this.canHaveBndPrefix = canHaveBndPrefix;
		}

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			var flags = InstrOpInfoFlags.None;
			if (canHaveBndPrefix && instr.HasRepnePrefix)
				flags |= InstrOpInfoFlags.BndPrefix;
			int instrCodeSize = GetCodeSize(instr.CodeSize);
			var branchInfo = BranchSizeInfo.None;
			if (instrCodeSize != 0 && instrCodeSize != codeSize) {
				if (codeSize == 16)
					branchInfo = BranchSizeInfo.Word;
				else if (codeSize == 32)
					branchInfo = BranchSizeInfo.Dword;
			}
			flags |= (InstrOpInfoFlags)((int)branchInfo << (int)InstrOpInfoFlags.BranchSizeInfoShift);
			info = new InstrOpInfo(mnemonic, ref instr, flags);
		}
	}

	sealed class SimpleInstrInfo_far : InstrInfo {
		readonly int codeSize;
		readonly string mnemonic;

		public SimpleInstrInfo_far(Code code, int codeSize, string mnemonic)
			: base(code) {
			this.codeSize = codeSize;
			this.mnemonic = mnemonic;
		}

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			var flags = InstrOpInfoFlags.None;
			int instrCodeSize = GetCodeSize(instr.CodeSize);
			var branchInfo = BranchSizeInfo.None;
			if (instrCodeSize != 0 && instrCodeSize != codeSize) {
				if (codeSize == 16)
					branchInfo = BranchSizeInfo.Word;
				else
					branchInfo = BranchSizeInfo.Dword;
			}
			flags |= (InstrOpInfoFlags)((int)branchInfo << (int)InstrOpInfoFlags.BranchSizeInfoShift);
			info = new InstrOpInfo(mnemonic, ref instr, flags);
		}
	}

	sealed class SimpleInstrInfo_far_mem : InstrInfo {
		readonly int codeSize;
		readonly string mnemonic;

		public SimpleInstrInfo_far_mem(Code code, int codeSize, string mnemonic)
			: base(code) {
			this.codeSize = codeSize;
			this.mnemonic = mnemonic;
		}

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			var flags = InstrOpInfoFlags.ShowNoMemSize_ForceSize;
			int instrCodeSize = GetCodeSize(instr.CodeSize);
			var farMemSizeInfo = FarMemorySizeInfo.None;
			if (instrCodeSize != 0 && instrCodeSize != codeSize) {
				if (codeSize == 16)
					farMemSizeInfo = FarMemorySizeInfo.Word;
				else
					farMemSizeInfo = FarMemorySizeInfo.Dword;
			}
			flags |= (InstrOpInfoFlags)((int)farMemSizeInfo << (int)InstrOpInfoFlags.FarMemorySizeInfoShift);
			info = new InstrOpInfo(mnemonic, ref instr, flags);
		}
	}

	sealed class SimpleInstrInfo_xbegin : InstrInfo {
		readonly int codeSize;
		readonly string mnemonic;

		public SimpleInstrInfo_xbegin(Code code, int codeSize, string mnemonic)
			: base(code) {
			this.codeSize = codeSize;
			this.mnemonic = mnemonic;
		}

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			var flags = InstrOpInfoFlags.None;
			var branchInfo = BranchSizeInfo.None;
			int instrCodeSize = GetCodeSize(instr.CodeSize);
			if (instrCodeSize == 0) {
				// Nothing
			}
			else if (instrCodeSize == 64) {
				if ((codeSize & 16) != 0)
					flags |= InstrOpInfoFlags.OpSize16;
			}
			else if ((instrCodeSize & codeSize) == 0) {
				if ((codeSize & 16) != 0)
					branchInfo = BranchSizeInfo.Word;
				else if ((codeSize & 32) != 0)
					branchInfo = BranchSizeInfo.Dword;
			}
			flags |= (InstrOpInfoFlags)((int)branchInfo << (int)InstrOpInfoFlags.BranchSizeInfoShift);
			info = new InstrOpInfo(mnemonic, ref instr, flags);
		}
	}

	sealed class SimpleInstrInfo_movabs : InstrInfo {
		readonly int memOpNumber;
		readonly string mnemonic;

		public SimpleInstrInfo_movabs(Code code, int memOpNumber, string mnemonic)
			: base(code) {
			this.memOpNumber = memOpNumber;
			this.mnemonic = mnemonic;
		}

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			var flags = InstrOpInfoFlags.None;
			int instrCodeSize = GetCodeSize(instr.CodeSize);
			var opKind = instr.GetOpKind(memOpNumber);
			int memSize;
			if (opKind == OpKind.Memory64)
				memSize = 64;
			else {
				Debug.Assert(opKind == OpKind.Memory);
				int displSize = instr.MemoryDisplSize;
				memSize = displSize == 2 ? 16 : 32;
			}
			if (instrCodeSize == 0)
				instrCodeSize = memSize;
			var memSizeInfo = MemorySizeInfo.None;
			if (instrCodeSize == 64) {
				if (memSize == 32)
					flags |= InstrOpInfoFlags.AddrSize32;
				else
					memSizeInfo = MemorySizeInfo.Qword;
			}
			else if (instrCodeSize != memSize) {
				Debug.Assert(memSize == 16 || memSize == 32);
				if (memSize == 16)
					memSizeInfo = MemorySizeInfo.Word;
				else
					memSizeInfo = MemorySizeInfo.Dword;
			}
			flags |= (InstrOpInfoFlags)((int)memSizeInfo << (int)InstrOpInfoFlags.MemorySizeInfoShift);
			info = new InstrOpInfo(mnemonic, ref instr, flags);
		}
	}

	sealed class SimpleInstrInfo_er : InstrInfo {
		readonly int erIndex;
		readonly string mnemonic;
		readonly InstrOpInfoFlags flags;

		public SimpleInstrInfo_er(Code code, int erIndex, string mnemonic) : this(code, erIndex, mnemonic, InstrOpInfoFlags.None) { }

		public SimpleInstrInfo_er(Code code, int erIndex, string mnemonic, InstrOpInfoFlags flags)
			: base(code) {
			this.erIndex = erIndex;
			this.mnemonic = mnemonic;
			this.flags = flags;
		}

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			info = new InstrOpInfo(mnemonic, ref instr, flags);
			var rc = instr.RoundingControl;
			if (rc != RoundingControl.None) {
				InstrOpKind rcOpKind;
				switch (rc) {
				case RoundingControl.RoundToNearest:	rcOpKind = InstrOpKind.RnSae; break;
				case RoundingControl.RoundDown:			rcOpKind = InstrOpKind.RdSae; break;
				case RoundingControl.RoundUp:			rcOpKind = InstrOpKind.RuSae; break;
				case RoundingControl.RoundTowardZero:	rcOpKind = InstrOpKind.RzSae; break;
				default:
					return;
				}
				MoveOperands(ref info, erIndex, rcOpKind);
			}
		}

		internal static void MoveOperands(ref InstrOpInfo info, int index, InstrOpKind newOpKind) {
			Debug.Assert(info.OpCount <= 4);

			switch (index) {
			case 2:
				Debug.Assert(info.OpCount < 4 || info.Op3Kind != InstrOpKind.Register);
				info.Op4Kind = info.Op3Kind;
				info.Op3Kind = info.Op2Kind;
				info.Op3Register = info.Op2Register;
				info.Op2Kind = newOpKind;
				info.Op4Index = info.Op3Index;
				info.Op3Index = info.Op2Index;
				info.Op2Index = OpAccess_None;
				info.OpCount++;
				break;

			case 3:
				Debug.Assert(info.OpCount < 4 || info.Op3Kind != InstrOpKind.Register);
				info.Op4Kind = info.Op3Kind;
				info.Op3Kind = newOpKind;
				info.Op4Index = info.Op3Index;
				info.Op3Index = OpAccess_None;
				info.OpCount++;
				break;

			default:
				throw new InvalidOperationException();
			}
		}
	}

	sealed class SimpleInstrInfo_sae : InstrInfo {
		readonly int saeIndex;
		readonly string mnemonic;

		public SimpleInstrInfo_sae(Code code, int saeIndex, string mnemonic)
			: base(code) {
			this.saeIndex = saeIndex;
			this.mnemonic = mnemonic;
		}

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			info = new InstrOpInfo(mnemonic, ref instr, InstrOpInfoFlags.None);
			if (instr.SuppressAllExceptions)
				SimpleInstrInfo_er.MoveOperands(ref info, saeIndex, InstrOpKind.Sae);
		}
	}

	sealed class SimpleInstrInfo_bcst : InstrInfo {
		readonly string mnemonic;
		readonly InstrOpInfoFlags flagsNoBroadcast;
		readonly InstrOpInfoFlags flagsBroadcast;

		public SimpleInstrInfo_bcst(Code code, string mnemonic, InstrOpInfoFlags flagsNoBroadcast, InstrOpInfoFlags flagsBroadcast)
			: base(code) {
			this.mnemonic = mnemonic;
			this.flagsNoBroadcast = flagsNoBroadcast;
			this.flagsBroadcast = flagsBroadcast;
		}

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			var memInfo = MemorySizes.AllMemorySizes[(int)instr.MemorySize];
			var flags = memInfo.bcstTo != null ? flagsBroadcast : flagsNoBroadcast;
			info = new InstrOpInfo(mnemonic, ref instr, flags);
		}
	}

	sealed class SimpleInstrInfo_bnd : InstrInfo {
		readonly string mnemonic;
		readonly InstrOpInfoFlags flags;

		public SimpleInstrInfo_bnd(Code code, string mnemonic) : this(code, mnemonic, InstrOpInfoFlags.None) { }

		public SimpleInstrInfo_bnd(Code code, string mnemonic, InstrOpInfoFlags flags)
			: base(code) {
			this.mnemonic = mnemonic;
			this.flags = flags;
		}

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			var flags = this.flags;
			if (instr.HasRepnePrefix)
				flags |= InstrOpInfoFlags.BndPrefix;
			info = new InstrOpInfo(mnemonic, ref instr, flags);
		}
	}

	sealed class SimpleInstrInfo_pops : InstrInfo {
		readonly string mnemonic;
		readonly string[] pseudo_ops;
		readonly InstrOpInfoFlags flags;

		public SimpleInstrInfo_pops(Code code, string mnemonic, string[] pseudo_ops) : this(code, mnemonic, pseudo_ops, InstrOpInfoFlags.None) { }

		public SimpleInstrInfo_pops(Code code, string mnemonic, string[] pseudo_ops, InstrOpInfoFlags flags)
			: base(code) {
			this.mnemonic = mnemonic;
			this.pseudo_ops = pseudo_ops;
			this.flags = flags;
		}

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			info = new InstrOpInfo(mnemonic, ref instr, flags);
			int imm = instr.Immediate8;
			if (options.UsePseudoOps && (uint)imm < (uint)pseudo_ops.Length) {
				info.Mnemonic = pseudo_ops[imm];
				RemoveLastOp(ref info);
			}
		}

		internal static void RemoveLastOp(ref InstrOpInfo info) {
			switch (info.OpCount) {
			case 5:
				info.Op4Index = OpAccess_INVALID;
				break;
			case 4:
				info.Op3Index = OpAccess_INVALID;
				break;
			case 3:
				info.Op2Index = OpAccess_INVALID;
				break;
			default:
				throw new InvalidOperationException();
			}
			info.OpCount--;
		}
	}

	sealed class SimpleInstrInfo_sae_pops : InstrInfo {
		readonly int saeIndex;
		readonly string mnemonic;
		readonly string[] pseudo_ops;

		public SimpleInstrInfo_sae_pops(Code code, int saeIndex, string mnemonic, string[] pseudo_ops)
			: base(code) {
			this.saeIndex = saeIndex;
			this.mnemonic = mnemonic;
			this.pseudo_ops = pseudo_ops;
		}

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			info = new InstrOpInfo(mnemonic, ref instr, InstrOpInfoFlags.None);
			if (instr.SuppressAllExceptions)
				SimpleInstrInfo_er.MoveOperands(ref info, saeIndex, InstrOpKind.Sae);
			int imm = instr.Immediate8;
			if (options.UsePseudoOps && (uint)imm < (uint)pseudo_ops.Length) {
				info.Mnemonic = pseudo_ops[imm];
				SimpleInstrInfo_pops.RemoveLastOp(ref info);
			}
		}
	}

	sealed class SimpleInstrInfo_ms_pops : InstrInfo {
		readonly string mnemonic;
		readonly string[] pseudo_ops;
		readonly InstrOpInfoFlags flags;
		readonly MemorySize memSize;

		public SimpleInstrInfo_ms_pops(Code code, string mnemonic, string[] pseudo_ops, InstrOpInfoFlags flags, MemorySize memSize)
			: base(code) {
			this.mnemonic = mnemonic;
			this.pseudo_ops = pseudo_ops;
			this.flags = flags;
			this.memSize = memSize;
		}

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			info = new InstrOpInfo(mnemonic, ref instr, flags);
			if (memSize != MemorySize.Unknown)
				info.MemorySize = memSize;
			int imm = instr.Immediate8;
			if (options.UsePseudoOps && (uint)imm < (uint)pseudo_ops.Length) {
				info.Mnemonic = pseudo_ops[imm];
				SimpleInstrInfo_pops.RemoveLastOp(ref info);
			}
		}
	}

	sealed class SimpleInstrInfo_pclmulqdq : InstrInfo {
		readonly string mnemonic;
		readonly string[] pseudo_ops;

		public SimpleInstrInfo_pclmulqdq(Code code, string mnemonic, string[] pseudo_ops)
			: base(code) {
			this.mnemonic = mnemonic;
			this.pseudo_ops = pseudo_ops;
		}

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			info = new InstrOpInfo(mnemonic, ref instr, InstrOpInfoFlags.None);
			if (options.UsePseudoOps) {
				int index;
				int imm = instr.Immediate8;
				if (imm == 0)
					index = 0;
				else if (imm == 1)
					index = 1;
				else if (imm == 0x10)
					index = 2;
				else if (imm == 0x11)
					index = 3;
				else
					index = -1;
				if (index >= 0) {
					info.Mnemonic = pseudo_ops[index];
					SimpleInstrInfo_pops.RemoveLastOp(ref info);
				}
			}
		}
	}

	sealed class SimpleInstrInfo_Reg16 : InstrInfo {
		readonly string mnemonic;

		public SimpleInstrInfo_Reg16(Code code, string mnemonic)
			: base(code) {
			this.mnemonic = mnemonic;
		}

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			const InstrOpInfoFlags flags = InstrOpInfoFlags.None;
			info = new InstrOpInfo(mnemonic, ref instr, flags);
			if (Register.EAX <= (Register)info.Op0Register && (Register)info.Op0Register <= Register.R15D)
				info.Op0Register = (byte)((Register)info.Op0Register - Register.EAX + Register.AX);
			if (Register.EAX <= (Register)info.Op1Register && (Register)info.Op1Register <= Register.R15D)
				info.Op1Register = (byte)((Register)info.Op1Register - Register.EAX + Register.AX);
		}
	}

	sealed class SimpleInstrInfo_invlpga : InstrInfo {
		readonly int codeSize;
		readonly string mnemonic;

		public SimpleInstrInfo_invlpga(Code code, int codeSize, string mnemonic)
			: base(code) {
			this.codeSize = codeSize;
			this.mnemonic = mnemonic;
		}

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			info = default;
			info.Mnemonic = mnemonic;
			info.OpCount = 2;
			info.Op0Kind = InstrOpKind.Register;
			info.Op1Kind = InstrOpKind.Register;
			Debug.Assert(InstrOpInfo.TEST_RegisterBits == 8);
			info.Op1Register = (byte)Register.ECX;
			info.Op0Index = OpAccess_Read;
			info.Op1Index = OpAccess_Read;

			switch (codeSize) {
			case 16:
				Debug.Assert(InstrOpInfo.TEST_RegisterBits == 8);
				info.Op0Register = (byte)Register.AX;
				break;

			case 32:
				Debug.Assert(InstrOpInfo.TEST_RegisterBits == 8);
				info.Op0Register = (byte)Register.EAX;
				break;

			case 64:
				Debug.Assert(InstrOpInfo.TEST_RegisterBits == 8);
				info.Op0Register = (byte)Register.RAX;
				break;

			default:
				throw new InvalidOperationException();
			}
		}
	}

	sealed class SimpleInstrInfo_DeclareData : InstrInfo {
		readonly string mnemonic;
		readonly InstrOpKind opKind;

		public SimpleInstrInfo_DeclareData(Code code, string mnemonic)
			: base(code) {
			this.mnemonic = mnemonic;
			InstrOpKind opKind;
			switch (code) {
			case Code.DeclareByte: opKind = InstrOpKind.DeclareByte; break;
			case Code.DeclareWord: opKind = InstrOpKind.DeclareWord; break;
			case Code.DeclareDword: opKind = InstrOpKind.DeclareDword; break;
			case Code.DeclareQword: opKind = InstrOpKind.DeclareQword; break;
			default: throw new InvalidOperationException();
			}
			this.opKind = opKind;
		}

		public override void GetOpInfo(NasmFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			info = new InstrOpInfo(mnemonic, ref instr, InstrOpInfoFlags.MnemonicIsDirective);
			info.OpCount = (byte)instr.DeclareDataCount;
			info.Op0Kind = opKind;
			info.Op1Kind = opKind;
			info.Op2Kind = opKind;
			info.Op3Kind = opKind;
			info.Op4Kind = opKind;
			info.Op0Index = OpAccess_Read;
			info.Op1Index = OpAccess_Read;
			info.Op2Index = OpAccess_Read;
			info.Op3Index = OpAccess_Read;
			info.Op4Index = OpAccess_Read;
		}
	}
}
#endif
