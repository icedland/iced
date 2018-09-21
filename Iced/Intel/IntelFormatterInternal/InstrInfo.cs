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

#if !NO_INTEL_FORMATTER && !NO_FORMATTER
using System;
using System.Diagnostics;

namespace Iced.Intel.IntelFormatterInternal {
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
	}

	enum SizeOverride {
		None,
		Size16,
		Size32,
		Size64,
	}

	enum BranchSizeInfo {
		None,
		Short,
	}

	[Flags]
	enum InstrOpInfoFlags : ushort {
		None						= 0,

		// show no mem size
		MemSize_Nothing				= 1,

		// AlwaysShowMemorySize is disabled: always show memory size
		ShowNoMemSize_ForceSize		= 2,

		SizeOverrideMask			= 3,
		OpSizeShift					= 2,
		OpSize16					= SizeOverride.Size16 << (int)OpSizeShift,
		OpSize32					= SizeOverride.Size32 << (int)OpSizeShift,
		OpSize64					= SizeOverride.Size64 << (int)OpSizeShift,
		AddrSizeShift				= 4,
		AddrSize16					= SizeOverride.Size16 << (int)AddrSizeShift,
		AddrSize32					= SizeOverride.Size32 << (int)AddrSizeShift,
		AddrSize64					= SizeOverride.Size64 << (int)AddrSizeShift,
		BranchSizeInfoShift			= 6,
		BranchSizeInfoMask			= 1,
		BranchSizeInfo_Short		= BranchSizeInfo.Short << (int)BranchSizeInfoShift,

		IgnoreOpMask				= 0x00000080,
		FarMnemonic					= 0x00000100,
		JccNotTaken					= 0x00000200,
		JccTaken					= 0x00000400,
		BndPrefix					= 0x00000800,
		IgnoreIndexReg				= 0x00001000,
		IgnorePrefixSegment			= 0x00002000,
		ForceMemSizeDwordOrQword	= 0x00004000,
	}

	struct InstrOpInfo {
		internal const int TEST_RegisterBits = 8;

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
			default: throw new ArgumentOutOfRangeException(nameof(operand));
			}
		}

		public InstrOpInfo(string mnemonic, ref Instruction instr, InstrOpInfoFlags flags) {
			Debug.Assert(DecoderConstants.MaxOpCount == 5);
			Mnemonic = mnemonic;
			Flags = flags;
			OpCount = (byte)instr.OpCount;
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
		}
	}

	abstract class InstrInfo {
		internal readonly Code TEST_Code;
		protected InstrInfo(Code code) => TEST_Code = code;

		public abstract void GetOpInfo(IntelFormatterOptions options, ref Instruction instr, out InstrOpInfo info);

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

		public override void GetOpInfo(IntelFormatterOptions options, ref Instruction instr, out InstrOpInfo info) =>
			info = new InstrOpInfo(mnemonic, ref instr, flags);
	}

	sealed class SimpleInstrInfo_memsize : InstrInfo {
		readonly int codeSize;
		readonly string mnemonic;

		public SimpleInstrInfo_memsize(Code code, int codeSize, string mnemonic)
			: base(code) {
			this.codeSize = codeSize;
			this.mnemonic = mnemonic;
		}

		public override void GetOpInfo(IntelFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			int instrCodeSize = GetCodeSize(instr.CodeSize);
			var flags = instrCodeSize == 0 || (instrCodeSize & codeSize) != 0 ? InstrOpInfoFlags.MemSize_Nothing : InstrOpInfoFlags.ShowNoMemSize_ForceSize;
			info = new InstrOpInfo(mnemonic, ref instr, flags);
		}
	}

	sealed class SimpleInstrInfo_YA : InstrInfo {
		readonly string mnemonic;

		public SimpleInstrInfo_YA(Code code, string mnemonic) : base(code) => this.mnemonic = mnemonic;

		public override void GetOpInfo(IntelFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			info = default;
			info.Mnemonic = mnemonic;
			info.OpCount = 1;
			info.Op0Kind = (InstrOpKind)instr.Op0Kind;
		}
	}

	sealed class SimpleInstrInfo_AX : InstrInfo {
		readonly string mnemonic;

		public SimpleInstrInfo_AX(Code code, string mnemonic) : base(code) => this.mnemonic = mnemonic;

		public override void GetOpInfo(IntelFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			info = default;
			info.Mnemonic = mnemonic;
			info.OpCount = 1;
			info.Op0Kind = (InstrOpKind)instr.Op1Kind;
		}
	}

	sealed class SimpleInstrInfo_AY : InstrInfo {
		readonly string mnemonic;

		public SimpleInstrInfo_AY(Code code, string mnemonic) : base(code) => this.mnemonic = mnemonic;

		public override void GetOpInfo(IntelFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			info = default;
			info.Mnemonic = mnemonic;
			info.OpCount = 1;
			info.Op0Kind = (InstrOpKind)instr.Op1Kind;
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

		public override void GetOpInfo(IntelFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			int instrCodeSize = GetCodeSize(instr.CodeSize);
			if (instrCodeSize == 0 || (instrCodeSize & codeSize) != 0)
				info = new InstrOpInfo(mnemonic, ref instr, InstrOpInfoFlags.None);
			else {
				info = default;
				info.Mnemonic = "xchg";
				info.OpCount = 2;
				info.Op0Kind = InstrOpKind.Register;
				info.Op1Kind = InstrOpKind.Register;
				Debug.Assert(InstrOpInfo.TEST_RegisterBits == 8);
				info.Op0Register = (byte)register;
				Debug.Assert(InstrOpInfo.TEST_RegisterBits == 8);
				info.Op1Register = (byte)register;
			}
		}
	}

	sealed class SimpleInstrInfo_ST1 : InstrInfo {
		readonly string mnemonic;
		readonly InstrOpInfoFlags flags;

		public SimpleInstrInfo_ST1(Code code, string mnemonic, InstrOpInfoFlags flags)
			: base(code) {
			this.mnemonic = mnemonic;
			this.flags = flags;
		}

		public override void GetOpInfo(IntelFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			info = new InstrOpInfo(mnemonic, ref instr, flags);
			Debug.Assert(instr.OpCount == 1);
			info.OpCount = 2;
			info.Op1Kind = info.Op0Kind;
			info.Op1Register = info.Op0Register;
			info.Op0Kind = InstrOpKind.Register;
			Debug.Assert(InstrOpInfo.TEST_RegisterBits == 8);
			info.Op0Register = (byte)Registers.Register_ST;
		}
	}

	sealed class SimpleInstrInfo_ST2 : InstrInfo {
		readonly string mnemonic;
		readonly InstrOpInfoFlags flags;

		public SimpleInstrInfo_ST2(Code code, string mnemonic, InstrOpInfoFlags flags)
			: base(code) {
			this.mnemonic = mnemonic;
			this.flags = flags;
		}

		public override void GetOpInfo(IntelFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			info = new InstrOpInfo(mnemonic, ref instr, flags);
			Debug.Assert(instr.OpCount == 1);
			info.OpCount = 2;
			info.Op1Kind = InstrOpKind.Register;
			Debug.Assert(InstrOpInfo.TEST_RegisterBits == 8);
			info.Op1Register = (byte)Registers.Register_ST;
		}
	}

	sealed class SimpleInstrInfo_maskmovq : InstrInfo {
		readonly string mnemonic;
		readonly InstrOpInfoFlags flags;

		public SimpleInstrInfo_maskmovq(Code code, string mnemonic, InstrOpInfoFlags flags)
			: base(code) {
			this.mnemonic = mnemonic;
			this.flags = flags | InstrOpInfoFlags.IgnorePrefixSegment;
		}

		public override void GetOpInfo(IntelFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			Debug.Assert(instr.OpCount == 3);

			var opKind = instr.Op0Kind;
			OpKind shortFormOpKind;
			switch (instr.CodeSize) {
			case CodeSize.Unknown:
				shortFormOpKind = opKind;
				break;
			case CodeSize.Code16:
				shortFormOpKind = OpKind.MemorySegDI;
				break;
			case CodeSize.Code32:
				shortFormOpKind = OpKind.MemorySegEDI;
				break;
			case CodeSize.Code64:
				shortFormOpKind = OpKind.MemorySegRDI;
				break;
			default:
				throw new InvalidOperationException();
			}

			var flags = this.flags;
			if (opKind != shortFormOpKind) {
				if (opKind == OpKind.MemorySegDI)
					flags |= InstrOpInfoFlags.AddrSize16;
				else if (opKind == OpKind.MemorySegEDI)
					flags |= InstrOpInfoFlags.AddrSize32;
				else if (opKind == OpKind.MemorySegRDI)
					flags |= InstrOpInfoFlags.AddrSize64;
			}
			info = default;
			info.Flags = flags;
			info.Mnemonic = mnemonic;
			info.OpCount = 2;
			info.Op0Kind = (InstrOpKind)instr.Op1Kind;
			Debug.Assert(InstrOpInfo.TEST_RegisterBits == 8);
			info.Op0Register = (byte)instr.Op1Register;
			info.Op1Kind = (InstrOpKind)instr.Op2Kind;
			Debug.Assert(InstrOpInfo.TEST_RegisterBits == 8);
			info.Op1Register = (byte)instr.Op2Register;
			var segReg = instr.PrefixSegment;
			if (segReg != Register.None) {
				info.OpCount = 3;
				info.Op2Kind = InstrOpKind.Register;
				Debug.Assert(InstrOpInfo.TEST_RegisterBits == 8);
				info.Op2Register = (byte)segReg;
			}
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

		public override void GetOpInfo(IntelFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
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

	sealed class SimpleInstrInfo_os_bnd : InstrInfo {
		readonly int codeSize;
		readonly string mnemonic;
		readonly InstrOpInfoFlags flags;

		public SimpleInstrInfo_os_bnd(Code code, int codeSize, string mnemonic) : this(code, codeSize, mnemonic, InstrOpInfoFlags.None) { }

		public SimpleInstrInfo_os_bnd(Code code, int codeSize, string mnemonic, InstrOpInfoFlags flags)
			: base(code) {
			this.codeSize = codeSize;
			this.mnemonic = mnemonic;
			this.flags = flags;
		}

		public override void GetOpInfo(IntelFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			var flags = this.flags;
			if (instr.HasPrefixRepne)
				flags |= InstrOpInfoFlags.BndPrefix;
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

	sealed class SimpleInstrInfo_as : InstrInfo {
		readonly int codeSize;
		readonly string mnemonic;
		readonly InstrOpInfoFlags flags;

		public SimpleInstrInfo_as(Code code, int codeSize, string mnemonic) : this(code, codeSize, mnemonic, InstrOpInfoFlags.None) { }

		public SimpleInstrInfo_as(Code code, int codeSize, string mnemonic, InstrOpInfoFlags flags)
			: base(code) {
			this.codeSize = codeSize;
			this.mnemonic = mnemonic;
			this.flags = flags;
		}

		public override void GetOpInfo(IntelFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			var flags = this.flags;
			int instrCodeSize = GetCodeSize(instr.CodeSize);
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

	sealed class SimpleInstrInfo_os_mem : InstrInfo {
		readonly int codeSize;
		readonly string mnemonic;

		public SimpleInstrInfo_os_mem(Code code, int codeSize, string mnemonic)
			: base(code) {
			this.codeSize = codeSize;
			this.mnemonic = mnemonic;
		}

		public override void GetOpInfo(IntelFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
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

		public override void GetOpInfo(IntelFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
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
			var prefixSeg = instr.PrefixSegment;
			if (prefixSeg == Register.CS)
				flags |= InstrOpInfoFlags.IgnorePrefixSegment | InstrOpInfoFlags.JccNotTaken;
			else if (prefixSeg == Register.DS)
				flags |= InstrOpInfoFlags.IgnorePrefixSegment | InstrOpInfoFlags.JccTaken;
			if (instr.HasPrefixRepne)
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

		public override void GetOpInfo(IntelFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
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
			if (instrCodeSize != 0 && instrCodeSize != codeSize) {
				if (codeSize == 16)
					flags |= InstrOpInfoFlags.OpSize16;
				else if (codeSize == 32)
					flags |= InstrOpInfoFlags.OpSize32;
				else
					flags |= InstrOpInfoFlags.OpSize64;
			}
			if (expectedReg != reg) {
				if (reg == Register.CX)
					flags |= InstrOpInfoFlags.AddrSize16;
				else if (reg == Register.ECX)
					flags |= InstrOpInfoFlags.AddrSize32;
				else
					flags |= InstrOpInfoFlags.AddrSize64;
			}
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

		public override void GetOpInfo(IntelFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
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
			if (instrCodeSize != memSize) {
				if (memSize == 16)
					flags |= InstrOpInfoFlags.AddrSize16;
				else if (memSize == 32)
					flags |= InstrOpInfoFlags.AddrSize32;
				else
					flags |= InstrOpInfoFlags.AddrSize64;
			}
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

		public override void GetOpInfo(IntelFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			var flags = InstrOpInfoFlags.None;
			int instrCodeSize = GetCodeSize(instr.CodeSize);
			if (instrCodeSize == 64)
				instrCodeSize = 32;
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

	sealed class SimpleInstrInfo_nop0F1F : InstrInfo {
		readonly string mnemonic;
		readonly InstrOpInfoFlags flags;
		readonly Register reg;

		public SimpleInstrInfo_nop0F1F(Code code, Register reg, string mnemonic, InstrOpInfoFlags flags)
			: base(code) {
			this.mnemonic = mnemonic;
			this.flags = flags;
			this.reg = reg;
		}

		public override void GetOpInfo(IntelFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			Debug.Assert(instr.OpCount == 1, "Instruction is fixed, remove this class");
			info = new InstrOpInfo(mnemonic, ref instr, flags);
			info.OpCount++;
			info.Op1Kind = InstrOpKind.Register;
			Debug.Assert(InstrOpInfo.TEST_RegisterBits == 8);
			info.Op1Register = (byte)reg;
		}
	}

	sealed class SimpleInstrInfo_k1 : InstrInfo {
		readonly string mnemonic;
		readonly InstrOpInfoFlags flags;

		public SimpleInstrInfo_k1(Code code, string mnemonic) : this(code, mnemonic, InstrOpInfoFlags.None) { }

		public SimpleInstrInfo_k1(Code code, string mnemonic, InstrOpInfoFlags flags)
			: base(code) {
			this.mnemonic = mnemonic;
			this.flags = flags;
		}

		public override void GetOpInfo(IntelFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			info = new InstrOpInfo(mnemonic, ref instr, flags);
			Debug.Assert(instr.OpCount == 1);
			var kreg = instr.OpMask;
			if (kreg != Register.None) {
				info.OpCount++;
				info.Op1Kind = InstrOpKind.Register;
				Debug.Assert(InstrOpInfo.TEST_RegisterBits == 8);
				info.Op1Register = (byte)kreg;
				info.Flags |= InstrOpInfoFlags.IgnoreOpMask;
			}
		}
	}

	sealed class SimpleInstrInfo_k2 : InstrInfo {
		readonly string mnemonic;
		readonly InstrOpInfoFlags flags;

		public SimpleInstrInfo_k2(Code code, string mnemonic) : this(code, mnemonic, InstrOpInfoFlags.None) { }

		public SimpleInstrInfo_k2(Code code, string mnemonic, InstrOpInfoFlags flags)
			: base(code) {
			this.mnemonic = mnemonic;
			this.flags = flags;
		}

		public override void GetOpInfo(IntelFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			info = new InstrOpInfo(mnemonic, ref instr, flags);
			Debug.Assert(instr.OpCount == 2);
			var kreg = instr.OpMask;
			if (kreg != Register.None) {
				info.OpCount++;
				info.Op2Kind = info.Op1Kind;
				info.Op2Register = info.Op1Register;
				info.Op1Kind = InstrOpKind.Register;
				Debug.Assert(InstrOpInfo.TEST_RegisterBits == 8);
				info.Op1Register = (byte)kreg;
				info.Flags |= InstrOpInfoFlags.IgnoreOpMask;
			}
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

		public override void GetOpInfo(IntelFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			var flags = this.flags;
			if (instr.HasPrefixRepne)
				flags |= InstrOpInfoFlags.BndPrefix;
			info = new InstrOpInfo(mnemonic, ref instr, flags);
		}
	}

	sealed class SimpleInstrInfo_fpu_ST_STi : InstrInfo {
		readonly string mnemonic;

		public SimpleInstrInfo_fpu_ST_STi(Code code, string mnemonic)
			: base(code) {
			this.mnemonic = mnemonic;
		}

		public override void GetOpInfo(IntelFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			const InstrOpInfoFlags flags = 0;
			if (options.UsePseudoOps && (instr.Op0Register == Register.ST1 || instr.Op1Register == Register.ST1)) {
				info = default;
				info.Mnemonic = mnemonic;
			}
			else {
				info = new InstrOpInfo(mnemonic, ref instr, flags);
				Debug.Assert(info.Op0Register == (int)Register.ST0);
				Debug.Assert(InstrOpInfo.TEST_RegisterBits == 8);
				info.Op0Register = (byte)Registers.Register_ST;
			}
		}
	}

	sealed class SimpleInstrInfo_fpu_STi_ST : InstrInfo {
		readonly string mnemonic;

		public SimpleInstrInfo_fpu_STi_ST(Code code, string mnemonic)
			: base(code) {
			this.mnemonic = mnemonic;
		}

		public override void GetOpInfo(IntelFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			const InstrOpInfoFlags flags = 0;
			if (options.UsePseudoOps && (instr.Op0Register == Register.ST1 || instr.Op1Register == Register.ST1)) {
				info = default;
				info.Mnemonic = mnemonic;
			}
			else {
				info = new InstrOpInfo(mnemonic, ref instr, flags);
				Debug.Assert(info.Op1Register == (int)Register.ST0);
				Debug.Assert(InstrOpInfo.TEST_RegisterBits == 8);
				info.Op1Register = (byte)Registers.Register_ST;
			}
		}
	}

	sealed class SimpleInstrInfo_ST_STi : InstrInfo {
		readonly string mnemonic;
		readonly InstrOpInfoFlags flags;

		public SimpleInstrInfo_ST_STi(Code code, string mnemonic) : this(code, mnemonic, InstrOpInfoFlags.None) { }

		public SimpleInstrInfo_ST_STi(Code code, string mnemonic, InstrOpInfoFlags flags)
			: base(code) {
			this.mnemonic = mnemonic;
			this.flags = flags;
		}

		public override void GetOpInfo(IntelFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			info = new InstrOpInfo(mnemonic, ref instr, flags);
			Debug.Assert(info.Op0Register == (int)Register.ST0);
			Debug.Assert(InstrOpInfo.TEST_RegisterBits == 8);
			info.Op0Register = (byte)Registers.Register_ST;
		}
	}

	sealed class SimpleInstrInfo_STi_ST : InstrInfo {
		readonly string mnemonic;
		readonly InstrOpInfoFlags flags;

		public SimpleInstrInfo_STi_ST(Code code, string mnemonic) : this(code, mnemonic, InstrOpInfoFlags.None) { }

		public SimpleInstrInfo_STi_ST(Code code, string mnemonic, InstrOpInfoFlags flags)
			: base(code) {
			this.mnemonic = mnemonic;
			this.flags = flags;
		}

		public override void GetOpInfo(IntelFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			info = new InstrOpInfo(mnemonic, ref instr, flags);
			Debug.Assert(info.Op1Register == (int)Register.ST0);
			Debug.Assert(InstrOpInfo.TEST_RegisterBits == 8);
			info.Op1Register = (byte)Registers.Register_ST;
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

		public override void GetOpInfo(IntelFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			info = new InstrOpInfo(mnemonic, ref instr, flags);
			int imm = instr.Immediate8;
			if (options.UsePseudoOps && (uint)imm < (uint)pseudo_ops.Length) {
				info.OpCount--;
				info.Mnemonic = pseudo_ops[imm];
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

		public override void GetOpInfo(IntelFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
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
					info.OpCount--;
				}
			}
		}
	}

	sealed class SimpleInstrInfo_imul : InstrInfo {
		readonly string mnemonic;
		readonly InstrOpInfoFlags flags;

		public SimpleInstrInfo_imul(Code code, string mnemonic) : this(code, mnemonic, InstrOpInfoFlags.None) { }

		public SimpleInstrInfo_imul(Code code, string mnemonic, InstrOpInfoFlags flags)
			: base(code) {
			this.mnemonic = mnemonic;
			this.flags = flags;
		}

		public override void GetOpInfo(IntelFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			info = new InstrOpInfo(mnemonic, ref instr, flags);
			Debug.Assert(info.OpCount == 3);
			if (options.UsePseudoOps && info.Op0Kind == InstrOpKind.Register && info.Op1Kind == InstrOpKind.Register && info.Op0Register == info.Op1Register) {
				info.OpCount--;
				info.Op1Kind = info.Op2Kind;
			}
		}
	}

	sealed class SimpleInstrInfo_Reg16 : InstrInfo {
		readonly string mnemonic;

		public SimpleInstrInfo_Reg16(Code code, string mnemonic)
			: base(code) {
			this.mnemonic = mnemonic;
		}

		public override void GetOpInfo(IntelFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			const InstrOpInfoFlags flags = InstrOpInfoFlags.None;
			info = new InstrOpInfo(mnemonic, ref instr, flags);
			if (Register.EAX <= (Register)info.Op0Register && (Register)info.Op0Register <= Register.R15D) {
				Debug.Assert(InstrOpInfo.TEST_RegisterBits == 8);
				info.Op0Register = (byte)((Register)info.Op0Register - Register.EAX + Register.AX);
			}
			if (Register.EAX <= (Register)info.Op1Register && (Register)info.Op1Register <= Register.R15D) {
				Debug.Assert(InstrOpInfo.TEST_RegisterBits == 8);
				info.Op1Register = (byte)((Register)info.Op1Register - Register.EAX + Register.AX);
			}
		}
	}

	sealed class SimpleInstrInfo_reg : InstrInfo {
		readonly string mnemonic;
		readonly Register register;
		readonly InstrOpInfoFlags flags;

		public SimpleInstrInfo_reg(Code code, string mnemonic, Register register) : this(code, mnemonic, register, InstrOpInfoFlags.None) { }

		public SimpleInstrInfo_reg(Code code, string mnemonic, Register register, InstrOpInfoFlags flags)
			: base(code) {
			this.mnemonic = mnemonic;
			this.register = register;
			this.flags = flags;
		}

		public override void GetOpInfo(IntelFormatterOptions options, ref Instruction instr, out InstrOpInfo info) {
			info = new InstrOpInfo(mnemonic, ref instr, flags);
			info.OpCount = 1;
			info.Op0Kind = InstrOpKind.Register;
			Debug.Assert(InstrOpInfo.TEST_RegisterBits == 8);
			info.Op0Register = (byte)register;
		}
	}
}
#endif
