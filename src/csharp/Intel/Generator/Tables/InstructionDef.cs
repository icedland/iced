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

using System;
using Generator.InstructionInfo;
using Generator.Enums;
using Generator.Enums.InstructionInfo;
using Generator.Enums.Encoder;
using System.Diagnostics;
using Generator.Formatters;
using Generator.Enums.Formatter;

namespace Generator.Tables {
	[Flags]
	enum InstructionDefFlags1 : uint {
		None					= 0,
		Bit16					= 0x00000001,
		Bit32					= 0x00000002,
		Bit64					= 0x00000004,
		CPL0					= 0x00000008,//TODO: Add to OpCodeInfo
		CPL1					= 0x00000010,//TODO: Add to OpCodeInfo
		CPL2					= 0x00000020,//TODO: Add to OpCodeInfo
		CPL3					= 0x00000040,//TODO: Add to OpCodeInfo
		/// <summary>
		/// It reads/writes too many registers
		/// </summary>
		SaveRestore				= 0x00000080,
		/// <summary>
		/// It's an instruction that implicitly uses the stack register, eg. <c>CALL</c>, <c>POP</c>, etc
		/// </summary>
		StackInstruction		= 0x00000100,
		/// <summary>
		/// The instruction doesn't read the segment register
		/// </summary>
		IgnoreSegment			= 0x00000200,
		/// <summary>
		/// The opmask register is read and written (instead of just read). This also implies that it can't be <c>K0</c>.
		/// </summary>
		OpMaskReadWrite			= 0x00000400,
		/// <summary>
		/// <c>W</c> bit is ignored in 16/32-bit mode (but not 64-bit mode)
		/// </summary>
		WIG32					= 0x00000800,
		Lock					= 0x00001000,
		Xacquire				= 0x00002000,
		Xrelease				= 0x00004000,
		Rep						= 0x00008000,
		Repne					= 0x00010000,
		Bnd						= 0x00020000,
		HintTaken				= 0x00040000,
		Notrack					= 0x00080000,
		Fwait					= 0x00100000,
		/// <summary>
		/// It's one of: INVALID, db, dw, dd, dq
		/// </summary>
		NoInstruction			= 0x00200000,
		Broadcast				= 0x00400000,
		RoundingControl			= 0x00800000,
		SuppressAllExceptions	= 0x01000000,
		OpMaskRegister			= 0x02000000,
		ZeroingMasking			= 0x04000000,
		/// <summary>
		/// Opmask can't be <c>K0</c>
		/// </summary>
		RequireOpMaskRegister	= 0x08000000,
		/// <summary>
		/// The mod bits are ignored and it's assumed modrm[7:6] == 11b
		/// </summary>
		IgnoresModBits			= 0x10000000,//TODO: Add to OpCodeInfo
		/// <summary>
		/// The index reg's reg-num (vsib op) (if any) and register ops' reg-num must be unique, eg. <c>MNEMONIC XMM1,YMM1,[RAX+ZMM1*2]</c>
		/// is invalid. Registers = <c>XMM</c>/<c>YMM</c>/<c>ZMM</c>/<c>TMM</c>.
		/// </summary>
		RequireUniqueRegNums	= 0x20000000,//TODO: Add to OpCodeInfo
		/// <summary>
		/// 66H prefix is not allowed (it will #UD)
		/// </summary>
		No66					= 0x40000000,//TODO: Add to OpCodeInfo
	}

	[Flags]
	enum InstructionDefFlags2 : uint {
		None					= 0,
		/// <summary>
		/// Decoded by the 16-bit Intel decoder
		/// </summary>
		IntelDecoder16			= 0x00000001,//TODO: Add to OpCodeInfo
		/// <summary>
		/// Decoded by the 32-bit Intel decoder
		/// </summary>
		IntelDecoder32			= 0x00000002,//TODO: Add to OpCodeInfo
		/// <summary>
		/// Decoded by the 64-bit Intel decoder
		/// </summary>
		IntelDecoder64			= 0x00000004,//TODO: Add to OpCodeInfo
		/// <summary>
		/// Decoded by the 16-bit AMD decoder
		/// </summary>
		AmdDecoder16			= 0x00000008,//TODO: Add to OpCodeInfo
		/// <summary>
		/// Decoded by the 32-bit AMD decoder
		/// </summary>
		AmdDecoder32			= 0x00000010,//TODO: Add to OpCodeInfo
		/// <summary>
		/// Decoded by the 64-bit AMD decoder
		/// </summary>
		AmdDecoder64			= 0x00000020,//TODO: Add to OpCodeInfo
		/// <summary>
		/// Intel decoder forces 64-bit operand size
		/// </summary>
		IntelForceOpSize64		= 0x00000040,//TODO: Add to OpCodeInfo
		/// <summary>
		/// Default operand size is 64 in 64-bit mode
		/// </summary>
		DefaultOpSize64			= 0x00000080,//TODO: Add to OpCodeInfo
		/// <summary>
		/// The operand size is always 64 in 64-bit mode
		/// </summary>
		ForceOpSize64			= 0x00000100,//TODO: Add to OpCodeInfo
		ProtectedMode			= 0x00000200,
	}

	[Flags]
	enum InstructionStringFlags : uint {
		/// <summary>
		/// No special code is needed to format the instruction string
		/// </summary>
		None					= 0,
		/// <summary>
		/// Set if the opmask is `{k1}` even if the first operand is also a `k` reg, eg. `xxx k2 {k1}, xmm`
		/// </summary>
		OpMaskIsK1				= 0x01,//TODO: Add to OpCodeInfo
		/// <summary>
		/// Increment the vector index which causes the first vector register number to be `2` instead of `1`, eg. `xxx eax, xmm2`
		/// </summary>
		IncVecIndex				= 0x02,//TODO: Add to OpCodeInfo
		/// <summary>
		/// Don't print the vector index (usually set if it's an MMX instruction), eg. `xxx mm, mm/m64`
		/// </summary>
		NoVecIndex				= 0x04,//TODO: Add to OpCodeInfo
		/// <summary>
		/// The first operand should use index `2` and the next operand index `1`, eg. `xxx xmm2, xmm1`
		/// </summary>
		SwapVecIndex12			= 0x08,//TODO: Add to OpCodeInfo
		/// <summary>
		/// If it's an FPU instruction with ops `st(0), st(i)`, don't print the first operand (`st(0)`), eg. `FCOM ST(i)`
		/// </summary>
		FpuSkipOp0				= 0x10,//TODO: Add to OpCodeInfo
		/// <summary>
		/// The modrm info is part of the string, eg. `!(11):000:bbb`
		/// </summary>
		ModRegRmString			= 0x20,//TODO: Add to OpCodeInfo
	}

	enum BranchKind {
		None,
		JccShort,
		JccNear,
		JmpShort,
		JmpNear,
		JmpFar,
		JmpNearIndirect,
		JmpFarIndirect,
		CallNear,
		CallFar,
		CallNearIndirect,
		CallFarIndirect,
		JmpeNear,
		JmpeNearIndirect,
		Loop,
		Jrcxz,
		Xbegin,
	}

	[DebuggerDisplay("{OpCodeString,nq} | {InstructionString,nq}")]
	sealed class InstructionDef {
		public readonly string OpCodeString;
		public readonly string InstructionString;
		public int OpCount => OpKinds.Length;
		public readonly EnumValue Code;
		public readonly EnumValue Mnemonic;
		public readonly EnumValue Memory;
		public readonly EnumValue MemoryBroadcast;
		public readonly EnumValue DecoderOption;//TODO: Add to OpCodeInfo
		public readonly EnumValue EncodingValue;
		public EncodingKind Encoding => (EncodingKind)EncodingValue.Value;
		public readonly InstructionDefFlags1 Flags1;
		public readonly InstructionDefFlags2 Flags2;
		public readonly InstructionStringFlags IStringFlags;

		public readonly CodeSize OperandSize;
		public readonly CodeSize AddressSize;
		public readonly MandatoryPrefix MandatoryPrefix;
		public readonly OpCodeTableKind Table;
		public readonly OpCodeL LBit;
		public readonly OpCodeW WBit;
		public readonly uint OpCode;
		public readonly int GroupIndex;
		public readonly int RmGroupIndex;
		public readonly TupleType TupleType;
		public readonly OpCodeOperandKind[] OpKinds;

		public readonly PseudoOpsKind? PseudoOp;
		public readonly CodeInfo CodeInfo;
		public readonly EnumValue ControlFlow;
		public readonly ConditionCode ConditionCode;
		public readonly BranchKind BranchKind;//TODO: Add to OpCodeInfo
		public readonly RflagsBits RflagsRead;
		public readonly RflagsBits RflagsUndefined;
		public readonly RflagsBits RflagsWritten;
		public readonly RflagsBits RflagsCleared;
		public readonly RflagsBits RflagsSet;
		public EnumValue? RflagsInfo { get; internal set; }
		public readonly EnumValue[] Cpuid;
		public EnumValue? CpuidInternal { get; internal set; }
		public readonly OpInfo[] OpInfo;
		public readonly EnumValue[] OpInfoEnum;

		public readonly FastFmtInstructionDef Fast;
		public readonly FmtInstructionDef Gas;
		public readonly FmtInstructionDef Intel;
		public readonly FmtInstructionDef Masm;
		public readonly FmtInstructionDef Nasm;

		public bool IsPrivileged => (Flags1 & InstructionDefFlags1.CPL3) == 0;

		public InstructionDef(EnumValue code, string opCodeString, string instructionString, EnumValue mnemonic, EnumValue mem, EnumValue bcst,
			EnumValue decoderOption, InstructionDefFlags1 flags1, InstructionDefFlags2 flags2, InstructionStringFlags istringFlags,
			MandatoryPrefix mandatoryPrefix, OpCodeTableKind table, OpCodeL lBit, OpCodeW wBit, uint opCode, int groupIndex, int rmGroupIndex, CodeSize operandSize, CodeSize addressSize, TupleType tupleType, OpCodeOperandKind[] opKinds,
			PseudoOpsKind? pseudoOp, CodeInfo codeInfo, EnumValue encoding, EnumValue flowControl, ConditionCode conditionCode, BranchKind branchKind, RflagsBits read, RflagsBits undefined, RflagsBits written, RflagsBits cleared, RflagsBits set, EnumValue[] cpuid, OpInfo[] opInfo,
			FastFmtInstructionDef fast, FmtInstructionDef gas, FmtInstructionDef intel, FmtInstructionDef masm, FmtInstructionDef nasm) {
			Code = code;
			OpCodeString = opCodeString;
			InstructionString = instructionString;
			Mnemonic = mnemonic;
			Memory = mem;
			MemoryBroadcast = bcst;
			DecoderOption = decoderOption;
			EncodingValue = encoding;
			Flags1 = flags1;
			Flags2 = flags2;
			IStringFlags = istringFlags;

			MandatoryPrefix = mandatoryPrefix;
			Table = table;
			LBit = lBit;
			WBit = wBit;
			OpCode = opCode;
			GroupIndex = groupIndex;
			RmGroupIndex = rmGroupIndex;
			TupleType = tupleType;
			OperandSize = operandSize;
			AddressSize = addressSize;
			OpKinds = opKinds;

			PseudoOp = pseudoOp;
			CodeInfo = codeInfo;
			ControlFlow = flowControl;
			ConditionCode = conditionCode;
			BranchKind = branchKind;
			RflagsRead = read;
			RflagsUndefined = undefined;
			RflagsWritten = written;
			RflagsCleared = cleared;
			RflagsSet = set;
			RflagsInfo = null;
			Cpuid = cpuid;
			CpuidInternal = null;
			OpInfo = opInfo;
			OpInfoEnum = new EnumValue[opInfo.Length];

			Fast = fast;
			Gas = gas;
			Intel = intel;
			Masm = masm;
			Nasm = nasm;
		}
	}
}
