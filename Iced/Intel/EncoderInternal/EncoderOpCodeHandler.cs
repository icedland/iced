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
using System.Diagnostics;

namespace Iced.Intel.EncoderInternal {
	enum LegacyOpCodeTable {
		Normal					= 0,
		Table0F					= 1,
		Table0F38				= 2,
		Table0F3A				= 3,
	}

	enum VexOpCodeTable {
		Table0F					= 1,
		Table0F38				= 2,
		Table0F3A				= 3,
	}

	enum XopOpCodeTable {
		XOP8					= 1,
		XOP9					= 2,
		XOPA					= 3,
	}

	enum EvexOpCodeTable {
		Table0F					= 1,
		Table0F38				= 2,
		Table0F3A				= 3,
	}

	enum Encodable {
		Unknown,// Don't use it, it's to catch bugs in the Code table
		Any,
		Only1632,
		Only64,
	}

	[Flags]
	enum EncFlags1 : uint {
		CodeMask			= (1U << Instruction.TEST_CodeBits) - 1,
		EncodingShift		= 13,
		EncodingMask		= 7,
		Legacy				= EncodingKind.Legacy << (int)EncodingShift,
		VEX					= EncodingKind.VEX << (int)EncodingShift,
		EVEX				= EncodingKind.EVEX << (int)EncodingShift,
		XOP					= EncodingKind.XOP << (int)EncodingShift,
		D3NOW				= EncodingKind.D3NOW << (int)EncodingShift,
		OpCodeShift			= 16,
	}

	[Flags]
	enum LegacyFlags3 : uint {
		OpMask				= 0x7F,
		Op1Shift			= 7,
		Op2Shift			= 14,
		Op3Shift			= 21,
	}

	[Flags]
	enum VexFlags3 : uint {
		OpMask				= 0x3F,
		Op1Shift			= 6,
		Op2Shift			= 12,
		Op3Shift			= 18,
		Op4Shift			= 24,
	}

	[Flags]
	enum XopFlags3 : uint {
		OpMask				= 0x1F,
		Op1Shift			= 5,
		Op2Shift			= 10,
		Op3Shift			= 15,
	}

	[Flags]
	enum EvexFlags3 : uint {
		OpMask				= 0x3F,
		Op1Shift			= 6,
		Op2Shift			= 12,
		Op3Shift			= 18,
	}

	enum AllowedPrefixes : uint {
		None,
		Bnd,
		BndNotrack,
		HintTakenBnd,
		Lock,
		Rep,
		RepeRepne,
		XacquireXreleaseLock,
		Xrelease,
	}

	[Flags]
	enum LegacyFlags : uint {
		None							= 0,

		MandatoryPrefixMask				= 3,
		MandatoryPrefixShift			= 0,
		PNP								= (MandatoryPrefixByte.None << (int)MandatoryPrefixShift) | HasMandatoryPrefix,
		P66								= (MandatoryPrefixByte.P66 << (int)MandatoryPrefixShift) | HasMandatoryPrefix,
		PF3								= (MandatoryPrefixByte.PF3 << (int)MandatoryPrefixShift) | HasMandatoryPrefix,
		PF2								= (MandatoryPrefixByte.PF2 << (int)MandatoryPrefixShift) | HasMandatoryPrefix,

		OpCodeTableMask					= 3,
		OpCodeTableShift				= 2,
		Table0F							= LegacyOpCodeTable.Table0F << (int)OpCodeTableShift,
		Table0F38						= LegacyOpCodeTable.Table0F38 << (int)OpCodeTableShift,
		Table0F3A						= LegacyOpCodeTable.Table0F3A << (int)OpCodeTableShift,

		EncodableMask					= 3,
		EncodableShift					= 4,
		Encodable_Any					= Encodable.Any << (int)EncodableShift,
		Encodable_Only1632				= Encodable.Only1632 << (int)EncodableShift,
		Encodable_Only64				= Encodable.Only64 << (int)EncodableShift,

		HasGroupIndex					= 1 << 6,
		GroupShift						= 7,
		Group0							= HasGroupIndex | (0 << (int)GroupShift),
		Group1							= HasGroupIndex | (1 << (int)GroupShift),
		Group2							= HasGroupIndex | (2 << (int)GroupShift),
		Group3							= HasGroupIndex | (3 << (int)GroupShift),
		Group4							= HasGroupIndex | (4 << (int)GroupShift),
		Group5							= HasGroupIndex | (5 << (int)GroupShift),
		Group6							= HasGroupIndex | (6 << (int)GroupShift),
		Group7							= HasGroupIndex | (7 << (int)GroupShift),

		AllowedPrefixesMask				= 0xF,
		AllowedPrefixesShift			= 10,
		Bnd								= AllowedPrefixes.Bnd << (int)AllowedPrefixesShift,
		BndNotrack						= AllowedPrefixes.BndNotrack << (int)AllowedPrefixesShift,
		HintTakenBnd					= AllowedPrefixes.HintTakenBnd << (int)AllowedPrefixesShift,
		Lock							= AllowedPrefixes.Lock << (int)AllowedPrefixesShift,
		Rep								= AllowedPrefixes.Rep << (int)AllowedPrefixesShift,
		RepeRepne						= AllowedPrefixes.RepeRepne << (int)AllowedPrefixesShift,
		XacquireXreleaseLock			= AllowedPrefixes.XacquireXreleaseLock << (int)AllowedPrefixesShift,
		Xrelease						= AllowedPrefixes.Xrelease << (int)AllowedPrefixesShift,

		Fwait							= 0x00004000,
		HasMandatoryPrefix				= 0x00008000,

		Legacy_OpSizeShift				= 28,
		Legacy_OperandSizeMask			= 3,
		Legacy_OpSize16					= OperandSize.Size16 << (int)Legacy_OpSizeShift,
		Legacy_OpSize32					= OperandSize.Size32 << (int)Legacy_OpSizeShift,
		Legacy_OpSize64					= OperandSize.Size64 << (int)Legacy_OpSizeShift,
		Legacy_AddrSizeShift			= 30,
		Legacy_AddressSizeMask			= 3,
		Legacy_AddrSize16				= (uint)AddressSize.Size16 << (int)Legacy_AddrSizeShift,
		Legacy_AddrSize32				= (uint)AddressSize.Size32 << (int)Legacy_AddrSizeShift,
		Legacy_AddrSize64				= (uint)AddressSize.Size64 << (int)Legacy_AddrSizeShift,
		a16 = Legacy_AddrSize16,
		a16_o16 = a16 | o16,
		a16_o32 = a16 | o32,
		a32 = Legacy_AddrSize32,
		a32_o16 = a32 | o16,
		a32_o32 = a32 | o32,
		a64 = Legacy_AddrSize64,
		o16 = Legacy_OpSize16,
		o32 = Legacy_OpSize32,
		rexw = Legacy_OpSize64,
	}

	[Flags]
	enum VexFlags : uint {
		None							= 0,

		MandatoryPrefixMask				= 3,
		MandatoryPrefixShift			= 0,
		P66								= MandatoryPrefixByte.P66 << (int)MandatoryPrefixShift,
		PF3								= MandatoryPrefixByte.PF3 << (int)MandatoryPrefixShift,
		PF2								= MandatoryPrefixByte.PF2 << (int)MandatoryPrefixShift,

		OpCodeTableMask					= 3,
		OpCodeTableShift				= 2,
		Table0F							= VexOpCodeTable.Table0F << (int)OpCodeTableShift,
		Table0F38						= VexOpCodeTable.Table0F38 << (int)OpCodeTableShift,
		Table0F3A						= VexOpCodeTable.Table0F3A << (int)OpCodeTableShift,

		EncodableMask					= 3,
		EncodableShift					= 4,
		Encodable_Any					= Encodable.Any << (int)EncodableShift,
		Encodable_Only1632				= Encodable.Only1632 << (int)EncodableShift,
		Encodable_Only64				= Encodable.Only64 << (int)EncodableShift,

		HasGroupIndex					= 1 << 6,
		GroupShift						= 7,
		Group0							= HasGroupIndex | (0 << (int)GroupShift),
		Group1							= HasGroupIndex | (1 << (int)GroupShift),
		Group2							= HasGroupIndex | (2 << (int)GroupShift),
		Group3							= HasGroupIndex | (3 << (int)GroupShift),
		Group4							= HasGroupIndex | (4 << (int)GroupShift),
		Group5							= HasGroupIndex | (5 << (int)GroupShift),
		Group6							= HasGroupIndex | (6 << (int)GroupShift),
		Group7							= HasGroupIndex | (7 << (int)GroupShift),

		VEX_LShift						= 26,
		VEX_L128						= 0,
		VEX_L256						= 0x04000000,
		VEX_L0							= VEX_L128 | VEX_L0_L1,
		VEX_L1							= VEX_L256 | VEX_L0_L1,
		VEX_LIG							= 0x08000000,
		VEX_L0_L1						= 0x10000000,

		VEX_W0							= 0,
		VEX_W1							= 0x20000000,
		VEX_WIG							= 0x40000000,
		VEX_WIG32						= 0x80000000,

		VEX_128_W0 = VEX_L128 | VEX_W0,
		VEX_128_W1 = VEX_L128 | VEX_W1,
		VEX_128_WIG = VEX_L128 | VEX_WIG,
		VEX_256_W0 = VEX_L256 | VEX_W0,
		VEX_256_W1 = VEX_L256 | VEX_W1,
		VEX_256_WIG = VEX_L256 | VEX_WIG,
		VEX_LIG_W0 = VEX_LIG | VEX_W0,
		VEX_LIG_W1 = VEX_LIG | VEX_W1,
		VEX_L0_W0 = VEX_L0 | VEX_W0,
		VEX_L0_W1 = VEX_L0 | VEX_W1,
		VEX_L0_WIG = VEX_L0 | VEX_WIG,
		VEX_LIG_WIG = VEX_LIG | VEX_WIG,
		VEX_L1_W0 = VEX_L1 | VEX_W0,
		VEX_L1_W1 = VEX_L1 | VEX_W1,
	}

	[Flags]
	enum XopFlags : uint {
		None							= 0,

		MandatoryPrefixMask				= 3,
		MandatoryPrefixShift			= 0,
		P66								= MandatoryPrefixByte.P66 << (int)MandatoryPrefixShift,
		PF3								= MandatoryPrefixByte.PF3 << (int)MandatoryPrefixShift,
		PF2								= MandatoryPrefixByte.PF2 << (int)MandatoryPrefixShift,

		OpCodeTableMask					= 3,
		OpCodeTableShift				= 2,
		TableXOP8						= XopOpCodeTable.XOP8 << (int)OpCodeTableShift,
		TableXOP9						= XopOpCodeTable.XOP9 << (int)OpCodeTableShift,
		TableXOPA						= XopOpCodeTable.XOPA << (int)OpCodeTableShift,

		EncodableMask					= 3,
		EncodableShift					= 4,
		Encodable_Any					= Encodable.Any << (int)EncodableShift,
		Encodable_Only1632				= Encodable.Only1632 << (int)EncodableShift,
		Encodable_Only64				= Encodable.Only64 << (int)EncodableShift,

		HasGroupIndex					= 1 << 6,
		GroupShift						= 7,
		Group0							= HasGroupIndex | (0 << (int)GroupShift),
		Group1							= HasGroupIndex | (1 << (int)GroupShift),
		Group2							= HasGroupIndex | (2 << (int)GroupShift),
		Group3							= HasGroupIndex | (3 << (int)GroupShift),
		Group4							= HasGroupIndex | (4 << (int)GroupShift),
		Group5							= HasGroupIndex | (5 << (int)GroupShift),
		Group6							= HasGroupIndex | (6 << (int)GroupShift),
		Group7							= HasGroupIndex | (7 << (int)GroupShift),

		XOP_LShift						= 28,
		XOP_L128						= 0,
		XOP_L256						= 0x10000000,
		XOP_L0							= XOP_L128 | XOP_L0_L1,
		XOP_L1							= XOP_L256 | XOP_L0_L1,
		XOP_L0_L1						= 0x20000000,

		XOP_W0							= 0,
		XOP_W1							= 0x40000000,
		XOP_WIG32						= 0x80000000,

		XOP_128_W0 = XOP_L128 | XOP_W0,
		XOP_256_W0 = XOP_L256 | XOP_W0,
		XOP_L0_W0 = XOP_L0 | XOP_W0,
		XOP_L0_W1 = XOP_L0 | XOP_W1,
		XOP_128_W1 = XOP_L128 | XOP_W1,
		XOP_256_W1 = XOP_L256 | XOP_W1,
	}

	[Flags]
	enum EvexFlags : uint {
		None							= 0,

		MandatoryPrefixMask				= 3,
		MandatoryPrefixShift			= 0,
		P66								= MandatoryPrefixByte.P66 << (int)MandatoryPrefixShift,
		PF3								= MandatoryPrefixByte.PF3 << (int)MandatoryPrefixShift,
		PF2								= MandatoryPrefixByte.PF2 << (int)MandatoryPrefixShift,

		OpCodeTableMask					= 3,
		OpCodeTableShift				= 2,
		Table0F							= EvexOpCodeTable.Table0F << (int)OpCodeTableShift,
		Table0F38						= EvexOpCodeTable.Table0F38 << (int)OpCodeTableShift,
		Table0F3A						= EvexOpCodeTable.Table0F3A << (int)OpCodeTableShift,

		EncodableMask					= 3,
		EncodableShift					= 4,
		Encodable_Any					= Encodable.Any << (int)EncodableShift,
		Encodable_Only1632				= Encodable.Only1632 << (int)EncodableShift,
		Encodable_Only64				= Encodable.Only64 << (int)EncodableShift,

		HasGroupIndex					= 1 << 6,
		GroupShift						= 7,
		Group0							= HasGroupIndex | (0 << (int)GroupShift),
		Group1							= HasGroupIndex | (1 << (int)GroupShift),
		Group2							= HasGroupIndex | (2 << (int)GroupShift),
		Group3							= HasGroupIndex | (3 << (int)GroupShift),
		Group4							= HasGroupIndex | (4 << (int)GroupShift),
		Group5							= HasGroupIndex | (5 << (int)GroupShift),
		Group6							= HasGroupIndex | (6 << (int)GroupShift),
		Group7							= HasGroupIndex | (7 << (int)GroupShift),

		TupleTypeBits					= 6,
		TupleTypeMask					= (1 << (int)TupleTypeBits) - 1,
		TupleTypeShift					= 10,

		EVEX_LShift						= 21,
		EVEX_L128						= VectorLength.L128 << (int)EVEX_LShift,
		EVEX_L256						= VectorLength.L256 << (int)EVEX_LShift,
		EVEX_L512						= VectorLength.L512 << (int)EVEX_LShift,
		EVEX_LIG						= 0x00800000,

		EVEX_W0							= 0,
		EVEX_W1							= 0x01000000,
		EVEX_WIG						= 0x02000000,
		EVEX_WIG32						= 0x04000000,

		EVEX_b							= 0x08000000,
		EVEX_er							= 0x10000000,
		EVEX_sae						= 0x20000000,
		EVEX_k1							= 0x40000000,
		EVEX_z							= 0x80000000,
		er = EVEX_er,
		k1 = EVEX_k1,
		k1_b = EVEX_k1 | EVEX_b,
		k1_sae = EVEX_k1 | EVEX_sae,
		k1_sae_b = EVEX_k1 | EVEX_sae | EVEX_b,
		k1z = EVEX_k1 | EVEX_z,
		k1z_b = EVEX_k1 | EVEX_z | EVEX_b,
		k1z_er = EVEX_k1 | EVEX_z | EVEX_er,
		k1z_er_b = EVEX_k1 | EVEX_z | EVEX_er | EVEX_b,
		k1z_sae = EVEX_k1 | EVEX_z | EVEX_sae,
		k1z_sae_b = EVEX_k1 | EVEX_z | EVEX_sae | EVEX_b,
		sae = EVEX_sae,
		sae_b = EVEX_sae | EVEX_b,
		EVEX_128_W0 = EVEX_L128 | EVEX_W0,
		EVEX_128_W1 = EVEX_L128 | EVEX_W1,
		EVEX_128_WIG = EVEX_L128 | EVEX_WIG,
		EVEX_256_W0 = EVEX_L256 | EVEX_W0,
		EVEX_256_W1 = EVEX_L256 | EVEX_W1,
		EVEX_256_WIG = EVEX_L256 | EVEX_WIG,
		EVEX_512_W0 = EVEX_L512 | EVEX_W0,
		EVEX_512_W1 = EVEX_L512 | EVEX_W1,
		EVEX_512_WIG = EVEX_L512 | EVEX_WIG,
		EVEX_LIG_W0 = EVEX_LIG | EVEX_W0,
		EVEX_LIG_W1 = EVEX_LIG | EVEX_W1,
	}

	[Flags]
	enum D3nowFlags : uint {
		None							= 0,

		EncodableMask					= 3,
		EncodableShift					= 0,
		Encodable_Any					= Encodable.Any << (int)EncodableShift,
		Encodable_Only1632				= Encodable.Only1632 << (int)EncodableShift,
		Encodable_Only64				= Encodable.Only64 << (int)EncodableShift,
	}

	delegate bool TryConvertToDisp8N(Encoder encoder, in Instruction instr, OpCodeHandler handler, int displ, out sbyte compressedValue);

	[Flags]
	enum OpCodeHandlerFlags : uint {
		None					= 0,
		Fwait					= 0x00000001,
		DeclareData				= 0x00000002,
	}

	abstract class OpCodeHandler {
		internal readonly Code TEST_Code;
		internal readonly uint OpCode;
		internal readonly int GroupIndex;
		internal readonly OpCodeHandlerFlags Flags;
		internal readonly Encodable Encodable;
		internal readonly OperandSize OpSize;
		internal readonly AddressSize AddrSize;
		internal readonly TryConvertToDisp8N? TryConvertToDisp8N;
		internal readonly Op[] Operands;
		protected OpCodeHandler(Code code, uint opCode, int groupIndex, OpCodeHandlerFlags flags, Encodable encodable, OperandSize opSize, AddressSize addrSize, TryConvertToDisp8N? tryConvertToDisp8N, Op[] operands) {
			TEST_Code = code;
			OpCode = opCode;
			GroupIndex = groupIndex;
			Flags = flags;
			Encodable = encodable;
			OpSize = opSize;
			AddrSize = addrSize;
			TryConvertToDisp8N = tryConvertToDisp8N;
			Operands = operands;
		}

		protected static Code GetCode(uint dword1) => (Code)(dword1 & (uint)EncFlags1.CodeMask);
		protected static uint GetOpCode(uint dword1) => dword1 >> (int)EncFlags1.OpCodeShift;
		public abstract void Encode(Encoder encoder, in Instruction instr);
	}

	sealed class InvalidHandler : OpCodeHandler {
		internal const string ERROR_MESSAGE = "Can't encode an invalid instruction";

		public InvalidHandler(Code code) : base(code, 0, 0, OpCodeHandlerFlags.None, Encodable.Any, OperandSize.None, AddressSize.None, null, Array2.Empty<Op>()) { }

		public override void Encode(Encoder encoder, in Instruction instr) =>
			encoder.ErrorMessage = ERROR_MESSAGE;
	}

	sealed class DeclareDataHandler : OpCodeHandler {
		readonly int elemLength;

		public DeclareDataHandler(Code code)
			: base(code, 0, 0, OpCodeHandlerFlags.DeclareData, Encodable.Any, OperandSize.None, AddressSize.None, null, Array2.Empty<Op>()) {
			switch (code) {
			case Code.DeclareByte:
				elemLength = 1;
				break;
			case Code.DeclareWord:
				elemLength = 2;
				break;
			case Code.DeclareDword:
				elemLength = 4;
				break;
			case Code.DeclareQword:
				elemLength = 8;
				break;
			default:
				throw new InvalidOperationException();
			}
		}

		public override void Encode(Encoder encoder, in Instruction instr) {
			int byteLength = instr.DeclareDataCount * elemLength;
			for (int i = 0; i < byteLength; i++)
				encoder.WriteByte(instr.GetDeclareByteValue(i));
		}
	}

	sealed class LegacyHandler : OpCodeHandler {
		readonly uint tableByte1, tableByte2;
		readonly uint mandatoryPrefix;

		static int GetGroupIndex(uint dword2) {
			if ((dword2 & (uint)LegacyFlags.HasGroupIndex) == 0)
				return -1;
			return (int)((dword2 >> (int)LegacyFlags.GroupShift) & 7);
		}

		static Op[] CreateOps(uint dword3) {
			var op0 = (LegacyOpKind)(dword3 & (uint)LegacyFlags3.OpMask);
			var op1 = (LegacyOpKind)((dword3 >> (int)LegacyFlags3.Op1Shift) & (uint)LegacyFlags3.OpMask);
			var op2 = (LegacyOpKind)((dword3 >> (int)LegacyFlags3.Op2Shift) & (uint)LegacyFlags3.OpMask);
			var op3 = (LegacyOpKind)((dword3 >> (int)LegacyFlags3.Op3Shift) & (uint)LegacyFlags3.OpMask);
			if (op3 != LegacyOpKind.None) {
				Debug.Assert(op0 != LegacyOpKind.None && op1 != LegacyOpKind.None && op2 != LegacyOpKind.None);
				return new Op[] { LegacyOps.Ops[(int)op0], LegacyOps.Ops[(int)op1], LegacyOps.Ops[(int)op2], LegacyOps.Ops[(int)op3] };
			}
			if (op2 != LegacyOpKind.None) {
				Debug.Assert(op0 != LegacyOpKind.None && op1 != LegacyOpKind.None);
				return new Op[] { LegacyOps.Ops[(int)op0], LegacyOps.Ops[(int)op1], LegacyOps.Ops[(int)op2] };
			}
			if (op1 != LegacyOpKind.None) {
				Debug.Assert(op0 != LegacyOpKind.None);
				return new Op[] { LegacyOps.Ops[(int)op0], LegacyOps.Ops[(int)op1] };
			}
			if (op0 != LegacyOpKind.None)
				return new Op[] { LegacyOps.Ops[(int)op0] };
			return Array2.Empty<Op>();
		}

		static OpCodeHandlerFlags GetFlags(uint dword2) {
			var flags = OpCodeHandlerFlags.None;
			if ((dword2 & (uint)LegacyFlags.Fwait) != 0)
				flags |= OpCodeHandlerFlags.Fwait;
			return flags;
		}

		public LegacyHandler(uint dword1, uint dword2, uint dword3)
			: base(GetCode(dword1), GetOpCode(dword1), GetGroupIndex(dword2), GetFlags(dword2), (Encodable)((dword2 >> (int)LegacyFlags.EncodableShift) & (uint)LegacyFlags.EncodableMask), (OperandSize)((dword2 >> (int)LegacyFlags.Legacy_OpSizeShift) & (uint)LegacyFlags.Legacy_OperandSizeMask), (AddressSize)((dword2 >> (int)LegacyFlags.Legacy_AddrSizeShift) & (uint)LegacyFlags.Legacy_AddressSizeMask), null, CreateOps(dword3)) {
			switch ((LegacyOpCodeTable)((dword2 >> (int)LegacyFlags.OpCodeTableShift) & (uint)LegacyFlags.OpCodeTableMask)) {
			case LegacyOpCodeTable.Normal:
				tableByte1 = 0;
				tableByte2 = 0;
				break;

			case LegacyOpCodeTable.Table0F:
				tableByte1 = 0x0F;
				tableByte2 = 0;
				break;

			case LegacyOpCodeTable.Table0F38:
				tableByte1 = 0x0F;
				tableByte2 = 0x38;
				break;

			case LegacyOpCodeTable.Table0F3A:
				tableByte1 = 0x0F;
				tableByte2 = 0x3A;
				break;

			default:
				throw new InvalidOperationException();
			}

			switch ((MandatoryPrefixByte)((dword2 >> (int)LegacyFlags.MandatoryPrefixShift) & (uint)LegacyFlags.MandatoryPrefixMask)) {
			case MandatoryPrefixByte.None:	mandatoryPrefix = 0x00; break;
			case MandatoryPrefixByte.P66:	mandatoryPrefix = 0x66; break;
			case MandatoryPrefixByte.PF3:	mandatoryPrefix = 0xF3; break;
			case MandatoryPrefixByte.PF2:	mandatoryPrefix = 0xF2; break;
			default:					throw new InvalidOperationException();
			}
		}

		public override void Encode(Encoder encoder, in Instruction instr) {
			uint b;
			if ((b = mandatoryPrefix) != 0)
				encoder.WriteByte(b);

			Debug.Assert((int)EncoderFlags.B == 0x01);
			Debug.Assert((int)EncoderFlags.X == 0x02);
			Debug.Assert((int)EncoderFlags.R == 0x04);
			Debug.Assert((int)EncoderFlags.W == 0x08);
			Debug.Assert((int)EncoderFlags.REX == 0x40);
			b = (uint)encoder.EncoderFlags;
			b &= 0x4F;
			if (b != 0) {
				if ((encoder.EncoderFlags & EncoderFlags.HighLegacy8BitRegs) != 0)
					encoder.ErrorMessage = "Registers AH, CH, DH, BH can't be used if there's a REX prefix. Use AL, CL, DL, BL, SPL, BPL, SIL, DIL, R8L-R15L instead.";
				b |= 0x40;
				encoder.WriteByte(b);
			}

			if ((b = tableByte1) != 0) {
				encoder.WriteByte(b);
				if ((b = tableByte2) != 0)
					encoder.WriteByte(b);
			}
		}
	}

	sealed class VexHandler : OpCodeHandler {
		readonly VexOpCodeTable opCodeTable;
		readonly bool W1;
		readonly uint lastByte;
		readonly uint mask_W_L;
		readonly uint mask_L;

		static int GetGroupIndex(uint dword2) {
			if ((dword2 & (uint)VexFlags.HasGroupIndex) == 0)
				return -1;
			return (int)((dword2 >> (int)VexFlags.GroupShift) & 7);
		}

		static Op[] CreateOps(uint dword3) {
			var op0 = (VexOpKind)(dword3 & (uint)VexFlags3.OpMask);
			var op1 = (VexOpKind)((dword3 >> (int)VexFlags3.Op1Shift) & (uint)VexFlags3.OpMask);
			var op2 = (VexOpKind)((dword3 >> (int)VexFlags3.Op2Shift) & (uint)VexFlags3.OpMask);
			var op3 = (VexOpKind)((dword3 >> (int)VexFlags3.Op3Shift) & (uint)VexFlags3.OpMask);
			var op4 = (VexOpKind)((dword3 >> (int)VexFlags3.Op4Shift) & (uint)VexFlags3.OpMask);
			if (op4 != VexOpKind.None) {
				Debug.Assert(op0 != VexOpKind.None && op1 != VexOpKind.None && op2 != VexOpKind.None && op3 != VexOpKind.None);
				return new Op[] { VexOps.Ops[(int)op0], VexOps.Ops[(int)op1], VexOps.Ops[(int)op2], VexOps.Ops[(int)op3], VexOps.Ops[(int)op4] };
			}
			if (op3 != VexOpKind.None) {
				Debug.Assert(op0 != VexOpKind.None && op1 != VexOpKind.None && op2 != VexOpKind.None);
				return new Op[] { VexOps.Ops[(int)op0], VexOps.Ops[(int)op1], VexOps.Ops[(int)op2], VexOps.Ops[(int)op3] };
			}
			if (op2 != VexOpKind.None) {
				Debug.Assert(op0 != VexOpKind.None && op1 != VexOpKind.None);
				return new Op[] { VexOps.Ops[(int)op0], VexOps.Ops[(int)op1], VexOps.Ops[(int)op2] };
			}
			if (op1 != VexOpKind.None) {
				Debug.Assert(op0 != VexOpKind.None);
				return new Op[] { VexOps.Ops[(int)op0], VexOps.Ops[(int)op1] };
			}
			if (op0 != VexOpKind.None)
				return new Op[] { VexOps.Ops[(int)op0] };
			return Array2.Empty<Op>();
		}

		public VexHandler(uint dword1, uint dword2, uint dword3)
			: base(GetCode(dword1), GetOpCode(dword1), GetGroupIndex(dword2), OpCodeHandlerFlags.None, (Encodable)((dword2 >> (int)VexFlags.EncodableShift) & (uint)VexFlags.EncodableMask), OperandSize.None, AddressSize.None, null, CreateOps(dword3)) {
			opCodeTable = (VexOpCodeTable)((dword2 >> (int)VexFlags.OpCodeTableShift) & (uint)VexFlags.OpCodeTableMask);
			W1 = (dword2 & (uint)VexFlags.VEX_W1) != 0;
			lastByte = (dword2 >> ((int)VexFlags.VEX_LShift - 2)) & 4;
			if (W1)
				lastByte |= 0x80;
			lastByte |= (dword2 >> (int)VexFlags.MandatoryPrefixShift) & (uint)VexFlags.MandatoryPrefixMask;
			if ((dword2 & (uint)VexFlags.VEX_WIG) != 0)
				mask_W_L |= 0x80;
			if ((dword2 & (uint)VexFlags.VEX_LIG) != 0) {
				mask_W_L |= 4;
				mask_L |= 4;
			}
		}

		public override void Encode(Encoder encoder, in Instruction instr) {
			uint encoderFlags = (uint)encoder.EncoderFlags;

			Debug.Assert((int)MandatoryPrefixByte.None == 0);
			Debug.Assert((int)MandatoryPrefixByte.P66 == 1);
			Debug.Assert((int)MandatoryPrefixByte.PF3 == 2);
			Debug.Assert((int)MandatoryPrefixByte.PF2 == 3);
			uint b = lastByte;
			b |= (~encoderFlags >> ((int)EncoderFlags.VvvvvShift - 3)) & 0x78;

			if (encoder.PreventVEX2 || W1 || opCodeTable != VexOpCodeTable.Table0F || (encoderFlags & (uint)(EncoderFlags.X | EncoderFlags.B | EncoderFlags.W)) != 0) {
				encoder.WriteByte(0xC4);
				Debug.Assert((int)VexOpCodeTable.Table0F == 1);
				Debug.Assert((int)VexOpCodeTable.Table0F38 == 2);
				Debug.Assert((int)VexOpCodeTable.Table0F3A == 3);
				uint b2 = (uint)opCodeTable;
				Debug.Assert((int)EncoderFlags.B == 1);
				Debug.Assert((int)EncoderFlags.X == 2);
				Debug.Assert((int)EncoderFlags.R == 4);
				b2 |= (~encoderFlags & 7) << 5;
				encoder.WriteByte(b2);
				b |= mask_W_L & encoder.Internal_VEX_WIG_LIG;
				encoder.WriteByte(b);
			}
			else {
				encoder.WriteByte(0xC5);
				Debug.Assert((int)EncoderFlags.R == 4);
				b |= (~encoderFlags & 4) << 5;
				b |= mask_L & encoder.Internal_VEX_LIG;
				encoder.WriteByte(b);
			}
		}
	}

	sealed class XopHandler : OpCodeHandler {
		readonly uint opCodeTable;
		readonly uint lastByte;

		static int GetGroupIndex(uint dword2) {
			if ((dword2 & (uint)XopFlags.HasGroupIndex) == 0)
				return -1;
			return (int)((dword2 >> (int)XopFlags.GroupShift) & 7);
		}

		static Op[] CreateOps(uint dword3) {
			var op0 = (XopOpKind)(dword3 & (uint)XopFlags3.OpMask);
			var op1 = (XopOpKind)((dword3 >> (int)XopFlags3.Op1Shift) & (uint)XopFlags3.OpMask);
			var op2 = (XopOpKind)((dword3 >> (int)XopFlags3.Op2Shift) & (uint)XopFlags3.OpMask);
			var op3 = (XopOpKind)((dword3 >> (int)XopFlags3.Op3Shift) & (uint)XopFlags3.OpMask);
			if (op3 != XopOpKind.None) {
				Debug.Assert(op0 != XopOpKind.None && op1 != XopOpKind.None && op2 != XopOpKind.None);
				return new Op[] { XopOps.Ops[(int)op0], XopOps.Ops[(int)op1], XopOps.Ops[(int)op2], XopOps.Ops[(int)op3] };
			}
			if (op2 != XopOpKind.None) {
				Debug.Assert(op0 != XopOpKind.None && op1 != XopOpKind.None);
				return new Op[] { XopOps.Ops[(int)op0], XopOps.Ops[(int)op1], XopOps.Ops[(int)op2] };
			}
			if (op1 != XopOpKind.None) {
				Debug.Assert(op0 != XopOpKind.None);
				return new Op[] { XopOps.Ops[(int)op0], XopOps.Ops[(int)op1] };
			}
			if (op0 != XopOpKind.None)
				return new Op[] { XopOps.Ops[(int)op0] };
			return Array2.Empty<Op>();
		}

		public XopHandler(uint dword1, uint dword2, uint dword3)
			: base(GetCode(dword1), GetOpCode(dword1), GetGroupIndex(dword2), OpCodeHandlerFlags.None, (Encodable)((dword2 >> (int)XopFlags.EncodableShift) & (uint)XopFlags.EncodableMask), OperandSize.None, AddressSize.None, null, CreateOps(dword3)) {
			Debug.Assert((int)XopOpCodeTable.XOP8 == 1);
			Debug.Assert((int)XopOpCodeTable.XOP9 == 2);
			Debug.Assert((int)XopOpCodeTable.XOPA == 3);
			opCodeTable = 7 + ((dword2 >> (int)XopFlags.OpCodeTableShift) & (uint)XopFlags.OpCodeTableMask);
			Debug.Assert(opCodeTable == 8 || opCodeTable == 9 || opCodeTable == 10);
			lastByte = (dword2 >> ((int)XopFlags.XOP_LShift - 2)) & 4;
			if ((dword2 & (uint)XopFlags.XOP_W1) != 0)
				lastByte |= 0x80;
			lastByte |= (dword2 >> (int)XopFlags.MandatoryPrefixShift) & (uint)XopFlags.MandatoryPrefixMask;
		}

		public override void Encode(Encoder encoder, in Instruction instr) {
			encoder.WriteByte(0x8F);

			uint encoderFlags = (uint)encoder.EncoderFlags;
			Debug.Assert((int)MandatoryPrefixByte.None == 0);
			Debug.Assert((int)MandatoryPrefixByte.P66 == 1);
			Debug.Assert((int)MandatoryPrefixByte.PF3 == 2);
			Debug.Assert((int)MandatoryPrefixByte.PF2 == 3);

			uint b = opCodeTable;
			Debug.Assert((int)EncoderFlags.B == 1);
			Debug.Assert((int)EncoderFlags.X == 2);
			Debug.Assert((int)EncoderFlags.R == 4);
			b |= (~encoderFlags & 7) << 5;
			encoder.WriteByte(b);
			b = lastByte;
			b |= (~encoderFlags >> ((int)EncoderFlags.VvvvvShift - 3)) & 0x78;
			encoder.WriteByte(b);
		}
	}

	sealed class EvexHandler : OpCodeHandler {
		readonly EvexFlags flags;
		readonly TupleType tupleType;
		readonly EvexOpCodeTable opCodeTable;
		readonly uint p1Bits;
		readonly uint llBits;
		readonly uint mask_W;
		readonly uint mask_LL;

		static int GetGroupIndex(uint dword2) {
			if ((dword2 & (uint)EvexFlags.HasGroupIndex) == 0)
				return -1;
			return (int)((dword2 >> (int)EvexFlags.GroupShift) & 7);
		}

		static Op[] CreateOps(uint dword3) {
			var op0 = (EvexOpKind)(dword3 & (uint)EvexFlags3.OpMask);
			var op1 = (EvexOpKind)((dword3 >> (int)EvexFlags3.Op1Shift) & (uint)EvexFlags3.OpMask);
			var op2 = (EvexOpKind)((dword3 >> (int)EvexFlags3.Op2Shift) & (uint)EvexFlags3.OpMask);
			var op3 = (EvexOpKind)((dword3 >> (int)EvexFlags3.Op3Shift) & (uint)EvexFlags3.OpMask);
			if (op3 != EvexOpKind.None) {
				Debug.Assert(op0 != EvexOpKind.None && op1 != EvexOpKind.None && op2 != EvexOpKind.None);
				return new Op[] { EvexOps.Ops[(int)op0], EvexOps.Ops[(int)op1], EvexOps.Ops[(int)op2], EvexOps.Ops[(int)op3] };
			}
			if (op2 != EvexOpKind.None) {
				Debug.Assert(op0 != EvexOpKind.None && op1 != EvexOpKind.None);
				return new Op[] { EvexOps.Ops[(int)op0], EvexOps.Ops[(int)op1], EvexOps.Ops[(int)op2] };
			}
			if (op1 != EvexOpKind.None) {
				Debug.Assert(op0 != EvexOpKind.None);
				return new Op[] { EvexOps.Ops[(int)op0], EvexOps.Ops[(int)op1] };
			}
			if (op0 != EvexOpKind.None)
				return new Op[] { EvexOps.Ops[(int)op0] };
			return Array2.Empty<Op>();
		}

		static readonly TryConvertToDisp8N tryConvertToDisp8N = new TryConvertToDisp8NImpl().TryConvertToDisp8N;

		public EvexHandler(uint dword1, uint dword2, uint dword3)
			: base(GetCode(dword1), GetOpCode(dword1), GetGroupIndex(dword2), OpCodeHandlerFlags.None, (Encodable)((dword2 >> (int)EvexFlags.EncodableShift) & (uint)EvexFlags.EncodableMask), OperandSize.None, AddressSize.None, tryConvertToDisp8N, CreateOps(dword3)) {
			flags = (EvexFlags)dword2;
			tupleType = (TupleType)((dword2 >> (int)EvexFlags.TupleTypeShift) & (uint)EvexFlags.TupleTypeMask);
			opCodeTable = (EvexOpCodeTable)((dword2 >> (int)EvexFlags.OpCodeTableShift) & (uint)EvexFlags.OpCodeTableMask);
			Debug.Assert((int)MandatoryPrefixByte.None == 0);
			Debug.Assert((int)MandatoryPrefixByte.P66 == 1);
			Debug.Assert((int)MandatoryPrefixByte.PF3 == 2);
			Debug.Assert((int)MandatoryPrefixByte.PF2 == 3);
			p1Bits = 4 | ((dword2 >> (int)EvexFlags.MandatoryPrefixShift) & (uint)EvexFlags.MandatoryPrefixMask);
			if ((dword2 & (uint)EvexFlags.EVEX_W1) != 0)
				p1Bits |= 0x80;
			llBits = (dword2 >> ((int)EvexFlags.EVEX_LShift - 5)) & 0x60;
			if ((dword2 & (uint)EvexFlags.EVEX_WIG) != 0)
				mask_W |= 0x80;
			if ((dword2 & (uint)EvexFlags.EVEX_LIG) != 0)
				mask_LL |= 0x60;
		}

		sealed class TryConvertToDisp8NImpl {
			public bool TryConvertToDisp8N(Encoder encoder, in Instruction instr, OpCodeHandler handler, int displ, out sbyte compressedValue) {
				var evexHandler = (EvexHandler)handler;
				int n;
				switch (evexHandler.tupleType) {
				case TupleType.None:
					n = 1;
					break;

				case TupleType.Full_128:
					if ((encoder.EncoderFlags & EncoderFlags.b) != 0)
						n = (evexHandler.flags & EvexFlags.EVEX_W1) != 0 ? 8 : 4;
					else
						n = 16;
					break;

				case TupleType.Full_256:
					if ((encoder.EncoderFlags & EncoderFlags.b) != 0)
						n = (evexHandler.flags & EvexFlags.EVEX_W1) != 0 ? 8 : 4;
					else
						n = 32;
					break;

				case TupleType.Full_512:
					if ((encoder.EncoderFlags & EncoderFlags.b) != 0)
						n = (evexHandler.flags & EvexFlags.EVEX_W1) != 0 ? 8 : 4;
					else
						n = 64;
					break;

				case TupleType.Half_128:
					n = (encoder.EncoderFlags & EncoderFlags.b) != 0 ? 4 : 8;
					break;

				case TupleType.Half_256:
					n = (encoder.EncoderFlags & EncoderFlags.b) != 0 ? 4 : 16;
					break;

				case TupleType.Half_512:
					n = (encoder.EncoderFlags & EncoderFlags.b) != 0 ? 4 : 32;
					break;

				case TupleType.Full_Mem_128:
					n = 16;
					break;

				case TupleType.Full_Mem_256:
					n = 32;
					break;

				case TupleType.Full_Mem_512:
					n = 64;
					break;

				case TupleType.Tuple1_Scalar:
					n = (evexHandler.flags & EvexFlags.EVEX_W1) != 0 ? 8 : 4;
					break;

				case TupleType.Tuple1_Scalar_1:
					n = 1;
					break;

				case TupleType.Tuple1_Scalar_2:
					n = 2;
					break;

				case TupleType.Tuple1_Scalar_4:
					n = 4;
					break;

				case TupleType.Tuple1_Scalar_8:
					n = 8;
					break;

				case TupleType.Tuple1_Fixed:
					n = (evexHandler.flags & EvexFlags.EVEX_W1) != 0 ? 8 : 4;
					break;

				case TupleType.Tuple1_Fixed_4:
					n = 4;
					break;

				case TupleType.Tuple1_Fixed_8:
					n = 8;
					break;

				case TupleType.Tuple2:
					n = (evexHandler.flags & EvexFlags.EVEX_W1) != 0 ? 16 : 8;
					break;

				case TupleType.Tuple4:
					n = (evexHandler.flags & EvexFlags.EVEX_W1) != 0 ? 32 : 16;
					break;

				case TupleType.Tuple8:
					Debug.Assert((evexHandler.flags & EvexFlags.EVEX_W1) == 0);
					n = 32;
					break;

				case TupleType.Tuple1_4X:
					n = 16;
					break;

				case TupleType.Half_Mem_128:
					n = 8;
					break;

				case TupleType.Half_Mem_256:
					n = 16;
					break;

				case TupleType.Half_Mem_512:
					n = 32;
					break;

				case TupleType.Quarter_Mem_128:
					n = 4;
					break;

				case TupleType.Quarter_Mem_256:
					n = 8;
					break;

				case TupleType.Quarter_Mem_512:
					n = 16;
					break;

				case TupleType.Eighth_Mem_128:
					n = 2;
					break;

				case TupleType.Eighth_Mem_256:
					n = 4;
					break;

				case TupleType.Eighth_Mem_512:
					n = 8;
					break;

				case TupleType.Mem128:
					n = 16;
					break;

				case TupleType.MOVDDUP_128:
					n = 8;
					break;

				case TupleType.MOVDDUP_256:
					n = 32;
					break;

				case TupleType.MOVDDUP_512:
					n = 64;
					break;

				default:
					throw new InvalidOperationException();
				}

				int res = displ / n;
				if (res * n == displ && sbyte.MinValue <= res && res <= sbyte.MaxValue) {
					compressedValue = (sbyte)res;
					return true;
				}

				compressedValue = 0;
				return false;
			}
		}

		public override void Encode(Encoder encoder, in Instruction instr) {
			uint encoderFlags = (uint)encoder.EncoderFlags;

			encoder.WriteByte(0x62);

			Debug.Assert((int)EvexOpCodeTable.Table0F == 1);
			Debug.Assert((int)EvexOpCodeTable.Table0F38 == 2);
			Debug.Assert((int)EvexOpCodeTable.Table0F3A == 3);
			uint b = (uint)opCodeTable;
			Debug.Assert((int)EncoderFlags.B == 1);
			Debug.Assert((int)EncoderFlags.X == 2);
			Debug.Assert((int)EncoderFlags.R == 4);
			b |= (encoderFlags & 7) << 5;
			Debug.Assert((int)EncoderFlags.R2 == 0x00000200);
			b |= (encoderFlags >> (9 - 4)) & 0x10;
			b ^= ~0xFU;
			encoder.WriteByte(b);

			b = p1Bits;
			b |= (~encoderFlags >> ((int)EncoderFlags.VvvvvShift - 3)) & 0x78;
			b |= mask_W & encoder.Internal_EVEX_WIG;
			encoder.WriteByte(b);

			b = instr.InternalOpMask;
			if (b != 0 && (flags & EvexFlags.EVEX_k1) == 0)
				encoder.ErrorMessage = "The instruction doesn't support opmask registers";
			b |= (encoderFlags >> ((int)EncoderFlags.VvvvvShift + 4 - 3)) & 8;
			if (instr.SuppressAllExceptions) {
				if ((flags & EvexFlags.EVEX_sae) == 0)
					encoder.ErrorMessage = "The instruction doesn't support suppress-all-exceptions";
				b |= 0x10;
			}
			var rc = instr.RoundingControl;
			if (rc != RoundingControl.None) {
				if ((flags & EvexFlags.EVEX_er) == 0)
					encoder.ErrorMessage = "The instruction doesn't support rounding control";
				b |= 0x10;
				Debug.Assert((int)RoundingControl.RoundToNearest == 1);
				Debug.Assert((int)RoundingControl.RoundDown == 2);
				Debug.Assert((int)RoundingControl.RoundUp == 3);
				Debug.Assert((int)RoundingControl.RoundTowardZero == 4);
				b |= (uint)(rc - RoundingControl.RoundToNearest) << 5;
			}
			else if ((flags & EvexFlags.EVEX_sae) == 0 || !instr.SuppressAllExceptions)
				b |= llBits;
			if ((encoderFlags & (uint)EncoderFlags.b) != 0) {
				if ((flags & EvexFlags.EVEX_b) == 0)
					encoder.ErrorMessage = "The instruction doesn't support broadcasting";
				b |= 0x10;
			}
			if (instr.ZeroingMasking) {
				if ((flags & EvexFlags.EVEX_z) == 0)
					encoder.ErrorMessage = "The instruction doesn't support zeroing masking";
				b |= 0x80;
			}
			b ^= 8;
			b |= mask_LL & encoder.Internal_EVEX_LIG;
			encoder.WriteByte(b);
		}
	}

	sealed class D3nowHandler : OpCodeHandler {
		static readonly Op[] operands = new Op[] {
			new OpModRM_reg(Register.MM0, Register.MM7),
			new OpModRM_rm(Register.MM0, Register.MM7),
		};
		readonly uint immediate;

		public D3nowHandler(uint dword1, uint dword2, uint dword3)
			: base(GetCode(dword1), 0x0F, -1, OpCodeHandlerFlags.None, (Encodable)((dword2 >> (int)D3nowFlags.EncodableShift) & (uint)D3nowFlags.EncodableMask), OperandSize.None, AddressSize.None, null, operands) {
			immediate = GetOpCode(dword1);
			Debug.Assert(immediate <= byte.MaxValue);
		}

		public override void Encode(Encoder encoder, in Instruction instr) {
			encoder.WriteByte(0x0F);
			encoder.ImmSize = ImmSize.Size1OpCode;
			encoder.Immediate = immediate;
		}
	}
}
#endif
