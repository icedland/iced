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
using System;
using System.Diagnostics;

namespace Iced.Intel.EncoderInternal {
	delegate bool TryConvertToDisp8N(Encoder encoder, in Instruction instruction, OpCodeHandler handler, int displ, out sbyte compressedValue);

	abstract class OpCodeHandler {
		internal readonly uint OpCode;
		internal readonly int GroupIndex;
		internal readonly OpCodeHandlerFlags Flags;
		internal readonly Encodable Encodable;
		internal readonly OperandSize OpSize;
		internal readonly AddressSize AddrSize;
		internal readonly TryConvertToDisp8N? TryConvertToDisp8N;
		internal readonly Op[] Operands;
		protected OpCodeHandler(uint opCode, int groupIndex, OpCodeHandlerFlags flags, Encodable encodable, OperandSize opSize, AddressSize addrSize, TryConvertToDisp8N? tryConvertToDisp8N, Op[] operands) {
			OpCode = opCode;
			GroupIndex = groupIndex;
			Flags = flags;
			Encodable = encodable;
			OpSize = opSize;
			AddrSize = addrSize;
			TryConvertToDisp8N = tryConvertToDisp8N;
			Operands = operands;
		}

		protected static uint GetOpCode(uint dword1) => dword1 >> (int)EncFlags1.OpCodeShift;
		public abstract void Encode(Encoder encoder, in Instruction instruction);
	}

	sealed class InvalidHandler : OpCodeHandler {
		internal const string ERROR_MESSAGE = "Can't encode an invalid instruction";

		public InvalidHandler() : base(0, 0, OpCodeHandlerFlags.None, Encodable.Any, OperandSize.None, AddressSize.None, null, Array2.Empty<Op>()) { }

		public override void Encode(Encoder encoder, in Instruction instruction) =>
			encoder.ErrorMessage = ERROR_MESSAGE;
	}

	sealed class DeclareDataHandler : OpCodeHandler {
		readonly int elemLength;

		public DeclareDataHandler(Code code)
			: base(0, 0, OpCodeHandlerFlags.DeclareData, Encodable.Any, OperandSize.None, AddressSize.None, null, Array2.Empty<Op>()) {
			elemLength = code switch {
				Code.DeclareByte => 1,
				Code.DeclareWord => 2,
				Code.DeclareDword => 4,
				Code.DeclareQword => 8,
				_ => throw new InvalidOperationException(),
			};
		}

		public override void Encode(Encoder encoder, in Instruction instruction) {
			int length = instruction.DeclareDataCount * elemLength;
			for (int i = 0; i < length; i++)
				encoder.WriteByteInternal(instruction.GetDeclareByteValue(i));
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
			var op0 = (LegacyOpKind)((dword3 >> (int)LegacyFlags3.Op0Shift) & (uint)LegacyFlags3.OpMask);
			var op1 = (LegacyOpKind)((dword3 >> (int)LegacyFlags3.Op1Shift) & (uint)LegacyFlags3.OpMask);
			var op2 = (LegacyOpKind)((dword3 >> (int)LegacyFlags3.Op2Shift) & (uint)LegacyFlags3.OpMask);
			var op3 = (LegacyOpKind)((dword3 >> (int)LegacyFlags3.Op3Shift) & (uint)LegacyFlags3.OpMask);
			if (op3 != LegacyOpKind.None) {
				Debug.Assert(op0 != LegacyOpKind.None && op1 != LegacyOpKind.None && op2 != LegacyOpKind.None);
				return new Op[] { OpHandlerData.LegacyOps[(int)op0 - 1], OpHandlerData.LegacyOps[(int)op1 - 1], OpHandlerData.LegacyOps[(int)op2 - 1], OpHandlerData.LegacyOps[(int)op3 - 1] };
			}
			if (op2 != LegacyOpKind.None) {
				Debug.Assert(op0 != LegacyOpKind.None && op1 != LegacyOpKind.None);
				return new Op[] { OpHandlerData.LegacyOps[(int)op0 - 1], OpHandlerData.LegacyOps[(int)op1 - 1], OpHandlerData.LegacyOps[(int)op2 - 1] };
			}
			if (op1 != LegacyOpKind.None) {
				Debug.Assert(op0 != LegacyOpKind.None);
				return new Op[] { OpHandlerData.LegacyOps[(int)op0 - 1], OpHandlerData.LegacyOps[(int)op1 - 1] };
			}
			if (op0 != LegacyOpKind.None)
				return new Op[] { OpHandlerData.LegacyOps[(int)op0 - 1] };
			return Array2.Empty<Op>();
		}

		static OpCodeHandlerFlags GetFlags(uint dword2) {
			var flags = OpCodeHandlerFlags.None;
			if ((dword2 & (uint)LegacyFlags.Fwait) != 0)
				flags |= OpCodeHandlerFlags.Fwait;
			return flags;
		}

		public LegacyHandler(uint dword1, uint dword2, uint dword3)
			: base(GetOpCode(dword1), GetGroupIndex(dword2), GetFlags(dword2), (Encodable)((dword2 >> (int)LegacyFlags.EncodableShift) & (uint)LegacyFlags.EncodableMask), (OperandSize)((dword2 >> (int)LegacyFlags.OperandSizeShift) & (uint)LegacyFlags.OperandSizeMask), (AddressSize)((dword2 >> (int)LegacyFlags.AddressSizeShift) & (uint)LegacyFlags.AddressSizeMask), null, CreateOps(dword3)) {
			switch ((LegacyOpCodeTable)((dword2 >> (int)LegacyFlags.LegacyOpCodeTableShift) & (uint)LegacyFlags.LegacyOpCodeTableMask)) {
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

			mandatoryPrefix = (MandatoryPrefixByte)((dword2 >> (int)LegacyFlags.MandatoryPrefixByteShift) & (uint)LegacyFlags.MandatoryPrefixByteMask) switch {
				MandatoryPrefixByte.None => 0x00,
				MandatoryPrefixByte.P66 => 0x66,
				MandatoryPrefixByte.PF3 => 0xF3,
				MandatoryPrefixByte.PF2 => 0xF2,
				_ => throw new InvalidOperationException(),
			};
		}

		public override void Encode(Encoder encoder, in Instruction instruction) {
			uint b;
			if ((b = mandatoryPrefix) != 0)
				encoder.WriteByteInternal(b);

			Static.Assert((int)EncoderFlags.B == 0x01 ? 0 : -1);
			Static.Assert((int)EncoderFlags.X == 0x02 ? 0 : -1);
			Static.Assert((int)EncoderFlags.R == 0x04 ? 0 : -1);
			Static.Assert((int)EncoderFlags.W == 0x08 ? 0 : -1);
			Static.Assert((int)EncoderFlags.REX == 0x40 ? 0 : -1);
			b = (uint)encoder.EncoderFlags;
			b &= 0x4F;
			if (b != 0) {
				if ((encoder.EncoderFlags & EncoderFlags.HighLegacy8BitRegs) != 0)
					encoder.ErrorMessage = "Registers AH, CH, DH, BH can't be used if there's a REX prefix. Use AL, CL, DL, BL, SPL, BPL, SIL, DIL, R8L-R15L instead.";
				b |= 0x40;
				encoder.WriteByteInternal(b);
			}

			if ((b = tableByte1) != 0) {
				encoder.WriteByteInternal(b);
				if ((b = tableByte2) != 0)
					encoder.WriteByteInternal(b);
			}
		}
	}

#if !NO_VEX
	sealed class VexHandler : OpCodeHandler {
		readonly uint table;
		readonly uint lastByte;
		readonly uint mask_W_L;
		readonly uint mask_L;
		readonly uint W1;

		static int GetGroupIndex(uint dword2) {
			if ((dword2 & (uint)VexFlags.HasGroupIndex) == 0)
				return -1;
			return (int)((dword2 >> (int)VexFlags.GroupShift) & 7);
		}

		static Op[] CreateOps(uint dword3) {
			var op0 = (VexOpKind)((dword3 >> (int)VexFlags3.Op0Shift) & (uint)VexFlags3.OpMask);
			var op1 = (VexOpKind)((dword3 >> (int)VexFlags3.Op1Shift) & (uint)VexFlags3.OpMask);
			var op2 = (VexOpKind)((dword3 >> (int)VexFlags3.Op2Shift) & (uint)VexFlags3.OpMask);
			var op3 = (VexOpKind)((dword3 >> (int)VexFlags3.Op3Shift) & (uint)VexFlags3.OpMask);
			var op4 = (VexOpKind)((dword3 >> (int)VexFlags3.Op4Shift) & (uint)VexFlags3.OpMask);
			if (op4 != VexOpKind.None) {
				Debug.Assert(op0 != VexOpKind.None && op1 != VexOpKind.None && op2 != VexOpKind.None && op3 != VexOpKind.None);
				return new Op[] { OpHandlerData.VexOps[(int)op0 - 1], OpHandlerData.VexOps[(int)op1 - 1], OpHandlerData.VexOps[(int)op2 - 1], OpHandlerData.VexOps[(int)op3 - 1], OpHandlerData.VexOps[(int)op4 - 1] };
			}
			if (op3 != VexOpKind.None) {
				Debug.Assert(op0 != VexOpKind.None && op1 != VexOpKind.None && op2 != VexOpKind.None);
				return new Op[] { OpHandlerData.VexOps[(int)op0 - 1], OpHandlerData.VexOps[(int)op1 - 1], OpHandlerData.VexOps[(int)op2 - 1], OpHandlerData.VexOps[(int)op3 - 1] };
			}
			if (op2 != VexOpKind.None) {
				Debug.Assert(op0 != VexOpKind.None && op1 != VexOpKind.None);
				return new Op[] { OpHandlerData.VexOps[(int)op0 - 1], OpHandlerData.VexOps[(int)op1 - 1], OpHandlerData.VexOps[(int)op2 - 1] };
			}
			if (op1 != VexOpKind.None) {
				Debug.Assert(op0 != VexOpKind.None);
				return new Op[] { OpHandlerData.VexOps[(int)op0 - 1], OpHandlerData.VexOps[(int)op1 - 1] };
			}
			if (op0 != VexOpKind.None)
				return new Op[] { OpHandlerData.VexOps[(int)op0 - 1] };
			return Array2.Empty<Op>();
		}

		public VexHandler(uint dword1, uint dword2, uint dword3)
			: base(GetOpCode(dword1), GetGroupIndex(dword2), OpCodeHandlerFlags.None, (Encodable)((dword2 >> (int)VexFlags.EncodableShift) & (uint)VexFlags.EncodableMask), OperandSize.None, AddressSize.None, null, CreateOps(dword3)) {
			table = ((dword2 >> (int)VexFlags.VexOpCodeTableShift) & (uint)VexFlags.VexOpCodeTableMask);
			var wbit = (WBit)((dword2 >> (int)VexFlags.WBitShift) & (uint)VexFlags.WBitMask);
			W1 = wbit == WBit.W1 ? uint.MaxValue : 0;
			var vexFlags = (VexVectorLength)((dword2 >> (int)VexFlags.VexVectorLengthShift) & (int)VexFlags.VexVectorLengthMask);
			switch (vexFlags) {
			case VexVectorLength.LZ:
			case VexVectorLength.L0:
			case VexVectorLength.L128:
			case VexVectorLength.LIG:
				break;
			case VexVectorLength.L1:
			case VexVectorLength.L256:
				lastByte = 4;
				break;
			default:
				throw new InvalidOperationException();
			}
			if (W1 != 0)
				lastByte |= 0x80;
			lastByte |= (dword2 >> (int)VexFlags.MandatoryPrefixByteShift) & (uint)VexFlags.MandatoryPrefixByteMask;
			if (wbit == WBit.WIG)
				mask_W_L |= 0x80;
			if (vexFlags == VexVectorLength.LIG) {
				mask_W_L |= 4;
				mask_L |= 4;
			}
		}

		public override void Encode(Encoder encoder, in Instruction instruction) {
			uint encoderFlags = (uint)encoder.EncoderFlags;

			Static.Assert((int)MandatoryPrefixByte.None == 0 ? 0 : -1);
			Static.Assert((int)MandatoryPrefixByte.P66 == 1 ? 0 : -1);
			Static.Assert((int)MandatoryPrefixByte.PF3 == 2 ? 0 : -1);
			Static.Assert((int)MandatoryPrefixByte.PF2 == 3 ? 0 : -1);
			uint b = lastByte;
			b |= (~encoderFlags >> ((int)EncoderFlags.VvvvvShift - 3)) & 0x78;

			if ((encoder.Internal_PreventVEX2 | W1 | (table - (uint)VexOpCodeTable.Table0F) | (encoderFlags & (uint)(EncoderFlags.X | EncoderFlags.B | EncoderFlags.W))) != 0) {
				encoder.WriteByteInternal(0xC4);
				Static.Assert((int)VexOpCodeTable.Table0F == 1 ? 0 : -1);
				Static.Assert((int)VexOpCodeTable.Table0F38 == 2 ? 0 : -1);
				Static.Assert((int)VexOpCodeTable.Table0F3A == 3 ? 0 : -1);
				uint b2 = table;
				Static.Assert((int)EncoderFlags.B == 1 ? 0 : -1);
				Static.Assert((int)EncoderFlags.X == 2 ? 0 : -1);
				Static.Assert((int)EncoderFlags.R == 4 ? 0 : -1);
				b2 |= (~encoderFlags & 7) << 5;
				encoder.WriteByteInternal(b2);
				b |= mask_W_L & encoder.Internal_VEX_WIG_LIG;
				encoder.WriteByteInternal(b);
			}
			else {
				encoder.WriteByteInternal(0xC5);
				Static.Assert((int)EncoderFlags.R == 4 ? 0 : -1);
				b |= (~encoderFlags & 4) << 5;
				b |= mask_L & encoder.Internal_VEX_LIG;
				encoder.WriteByteInternal(b);
			}
		}
	}
#endif

#if !NO_XOP
	sealed class XopHandler : OpCodeHandler {
		readonly uint table;
		readonly uint lastByte;

		static int GetGroupIndex(uint dword2) {
			if ((dword2 & (uint)XopFlags.HasGroupIndex) == 0)
				return -1;
			return (int)((dword2 >> (int)XopFlags.GroupShift) & 7);
		}

		static Op[] CreateOps(uint dword3) {
			var op0 = (XopOpKind)((dword3 >> (int)XopFlags3.Op0Shift) & (uint)XopFlags3.OpMask);
			var op1 = (XopOpKind)((dword3 >> (int)XopFlags3.Op1Shift) & (uint)XopFlags3.OpMask);
			var op2 = (XopOpKind)((dword3 >> (int)XopFlags3.Op2Shift) & (uint)XopFlags3.OpMask);
			var op3 = (XopOpKind)((dword3 >> (int)XopFlags3.Op3Shift) & (uint)XopFlags3.OpMask);
			if (op3 != XopOpKind.None) {
				Debug.Assert(op0 != XopOpKind.None && op1 != XopOpKind.None && op2 != XopOpKind.None);
				return new Op[] { OpHandlerData.XopOps[(int)op0 - 1], OpHandlerData.XopOps[(int)op1 - 1], OpHandlerData.XopOps[(int)op2 - 1], OpHandlerData.XopOps[(int)op3 - 1] };
			}
			if (op2 != XopOpKind.None) {
				Debug.Assert(op0 != XopOpKind.None && op1 != XopOpKind.None);
				return new Op[] { OpHandlerData.XopOps[(int)op0 - 1], OpHandlerData.XopOps[(int)op1 - 1], OpHandlerData.XopOps[(int)op2 - 1] };
			}
			if (op1 != XopOpKind.None) {
				Debug.Assert(op0 != XopOpKind.None);
				return new Op[] { OpHandlerData.XopOps[(int)op0 - 1], OpHandlerData.XopOps[(int)op1 - 1] };
			}
			if (op0 != XopOpKind.None)
				return new Op[] { OpHandlerData.XopOps[(int)op0 - 1] };
			return Array2.Empty<Op>();
		}

		public XopHandler(uint dword1, uint dword2, uint dword3)
			: base(GetOpCode(dword1), GetGroupIndex(dword2), OpCodeHandlerFlags.None, (Encodable)((dword2 >> (int)XopFlags.EncodableShift) & (uint)XopFlags.EncodableMask), OperandSize.None, AddressSize.None, null, CreateOps(dword3)) {
			Static.Assert((int)XopOpCodeTable.XOP8 == 0 ? 0 : -1);
			Static.Assert((int)XopOpCodeTable.XOP9 == 1 ? 0 : -1);
			Static.Assert((int)XopOpCodeTable.XOPA == 2 ? 0 : -1);
			table = 8 + ((dword2 >> (int)XopFlags.XopOpCodeTableShift) & (uint)XopFlags.XopOpCodeTableMask);
			Debug.Assert(table == 8 || table == 9 || table == 10);
			lastByte = (dword2 >> ((int)XopFlags.XopVectorLengthShift - 2)) & 4;
			var wbit = (WBit)((dword2 >> (int)XopFlags.WBitShift) & (uint)XopFlags.WBitMask);
			if (wbit == WBit.W1)
				lastByte |= 0x80;
			lastByte |= (dword2 >> (int)XopFlags.MandatoryPrefixByteShift) & (uint)XopFlags.MandatoryPrefixByteMask;
		}

		public override void Encode(Encoder encoder, in Instruction instruction) {
			encoder.WriteByteInternal(0x8F);

			uint encoderFlags = (uint)encoder.EncoderFlags;
			Static.Assert((int)MandatoryPrefixByte.None == 0 ? 0 : -1);
			Static.Assert((int)MandatoryPrefixByte.P66 == 1 ? 0 : -1);
			Static.Assert((int)MandatoryPrefixByte.PF3 == 2 ? 0 : -1);
			Static.Assert((int)MandatoryPrefixByte.PF2 == 3 ? 0 : -1);

			uint b = table;
			Static.Assert((int)EncoderFlags.B == 1 ? 0 : -1);
			Static.Assert((int)EncoderFlags.X == 2 ? 0 : -1);
			Static.Assert((int)EncoderFlags.R == 4 ? 0 : -1);
			b |= (~encoderFlags & 7) << 5;
			encoder.WriteByteInternal(b);
			b = lastByte;
			b |= (~encoderFlags >> ((int)EncoderFlags.VvvvvShift - 3)) & 0x78;
			encoder.WriteByteInternal(b);
		}
	}
#endif

#if !NO_EVEX
	sealed class EvexHandler : OpCodeHandler {
		readonly WBit wbit;
		readonly EvexFlags flags;
		readonly TupleType tupleType;
		readonly uint table;
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
			var op0 = (EvexOpKind)((dword3 >> (int)EvexFlags3.Op0Shift) & (uint)EvexFlags3.OpMask);
			var op1 = (EvexOpKind)((dword3 >> (int)EvexFlags3.Op1Shift) & (uint)EvexFlags3.OpMask);
			var op2 = (EvexOpKind)((dword3 >> (int)EvexFlags3.Op2Shift) & (uint)EvexFlags3.OpMask);
			var op3 = (EvexOpKind)((dword3 >> (int)EvexFlags3.Op3Shift) & (uint)EvexFlags3.OpMask);
			if (op3 != EvexOpKind.None) {
				Debug.Assert(op0 != EvexOpKind.None && op1 != EvexOpKind.None && op2 != EvexOpKind.None);
				return new Op[] { OpHandlerData.EvexOps[(int)op0 - 1], OpHandlerData.EvexOps[(int)op1 - 1], OpHandlerData.EvexOps[(int)op2 - 1], OpHandlerData.EvexOps[(int)op3 - 1] };
			}
			if (op2 != EvexOpKind.None) {
				Debug.Assert(op0 != EvexOpKind.None && op1 != EvexOpKind.None);
				return new Op[] { OpHandlerData.EvexOps[(int)op0 - 1], OpHandlerData.EvexOps[(int)op1 - 1], OpHandlerData.EvexOps[(int)op2 - 1] };
			}
			if (op1 != EvexOpKind.None) {
				Debug.Assert(op0 != EvexOpKind.None);
				return new Op[] { OpHandlerData.EvexOps[(int)op0 - 1], OpHandlerData.EvexOps[(int)op1 - 1] };
			}
			if (op0 != EvexOpKind.None)
				return new Op[] { OpHandlerData.EvexOps[(int)op0 - 1] };
			return Array2.Empty<Op>();
		}

		static readonly TryConvertToDisp8N tryConvertToDisp8N = new TryConvertToDisp8NImpl().TryConvertToDisp8N;

		public EvexHandler(uint dword1, uint dword2, uint dword3)
			: base(GetOpCode(dword1), GetGroupIndex(dword2), OpCodeHandlerFlags.None, (Encodable)((dword2 >> (int)EvexFlags.EncodableShift) & (uint)EvexFlags.EncodableMask), OperandSize.None, AddressSize.None, tryConvertToDisp8N, CreateOps(dword3)) {
			flags = (EvexFlags)dword2;
			tupleType = (TupleType)((dword2 >> (int)EvexFlags.TupleTypeShift) & (uint)EvexFlags.TupleTypeMask);
			table = (dword2 >> (int)EvexFlags.EvexOpCodeTableShift) & (uint)EvexFlags.EvexOpCodeTableMask;
			Static.Assert((int)MandatoryPrefixByte.None == 0 ? 0 : -1);
			Static.Assert((int)MandatoryPrefixByte.P66 == 1 ? 0 : -1);
			Static.Assert((int)MandatoryPrefixByte.PF3 == 2 ? 0 : -1);
			Static.Assert((int)MandatoryPrefixByte.PF2 == 3 ? 0 : -1);
			p1Bits = 4 | ((dword2 >> (int)EvexFlags.MandatoryPrefixByteShift) & (uint)EvexFlags.MandatoryPrefixByteMask);
			wbit = (WBit)((dword2 >> (int)EvexFlags.WBitShift) & (uint)EvexFlags.WBitMask);
			if (wbit == WBit.W1)
				p1Bits |= 0x80;
			Static.Assert((int)EvexFlags.EvexVectorLengthMask == 3 ? 0 : -1);
			llBits = (dword2 >> ((int)EvexFlags.EvexVectorLengthShift - 5)) & 0x60;
			if (wbit == WBit.WIG)
				mask_W |= 0x80;
			if ((dword2 & (uint)EvexFlags.LIG) != 0)
				mask_LL |= 0x60;
		}

		sealed class TryConvertToDisp8NImpl {
			public bool TryConvertToDisp8N(Encoder encoder, in Instruction instruction, OpCodeHandler handler, int displ, out sbyte compressedValue) {
				var evexHandler = (EvexHandler)handler;
				int n;
				switch (evexHandler.tupleType) {
				case TupleType.None:
					n = 1;
					break;

				case TupleType.Full_128:
					if ((encoder.EncoderFlags & EncoderFlags.Broadcast) != 0)
						n = evexHandler.wbit == WBit.W1 ? 8 : 4;
					else
						n = 16;
					break;

				case TupleType.Full_256:
					if ((encoder.EncoderFlags & EncoderFlags.Broadcast) != 0)
						n = evexHandler.wbit == WBit.W1 ? 8 : 4;
					else
						n = 32;
					break;

				case TupleType.Full_512:
					if ((encoder.EncoderFlags & EncoderFlags.Broadcast) != 0)
						n = evexHandler.wbit == WBit.W1 ? 8 : 4;
					else
						n = 64;
					break;

				case TupleType.Half_128:
					n = (encoder.EncoderFlags & EncoderFlags.Broadcast) != 0 ? 4 : 8;
					break;

				case TupleType.Half_256:
					n = (encoder.EncoderFlags & EncoderFlags.Broadcast) != 0 ? 4 : 16;
					break;

				case TupleType.Half_512:
					n = (encoder.EncoderFlags & EncoderFlags.Broadcast) != 0 ? 4 : 32;
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
					n = evexHandler.wbit == WBit.W1 ? 8 : 4;
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

				case TupleType.Tuple1_Fixed_4:
					n = 4;
					break;

				case TupleType.Tuple1_Fixed_8:
					n = 8;
					break;

				case TupleType.Tuple2:
					n = evexHandler.wbit == WBit.W1 ? 16 : 8;
					break;

				case TupleType.Tuple4:
					n = evexHandler.wbit == WBit.W1 ? 32 : 16;
					break;

				case TupleType.Tuple8:
					Debug.Assert(evexHandler.wbit != WBit.W1);
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

		public override void Encode(Encoder encoder, in Instruction instruction) {
			uint encoderFlags = (uint)encoder.EncoderFlags;

			encoder.WriteByteInternal(0x62);

			Static.Assert((int)EvexOpCodeTable.Table0F == 1 ? 0 : -1);
			Static.Assert((int)EvexOpCodeTable.Table0F38 == 2 ? 0 : -1);
			Static.Assert((int)EvexOpCodeTable.Table0F3A == 3 ? 0 : -1);
			uint b = table;
			Static.Assert((int)EncoderFlags.B == 1 ? 0 : -1);
			Static.Assert((int)EncoderFlags.X == 2 ? 0 : -1);
			Static.Assert((int)EncoderFlags.R == 4 ? 0 : -1);
			b |= (encoderFlags & 7) << 5;
			Static.Assert((int)EncoderFlags.R2 == 0x00000200 ? 0 : -1);
			b |= (encoderFlags >> (9 - 4)) & 0x10;
			b ^= ~0xFU;
			encoder.WriteByteInternal(b);

			b = p1Bits;
			b |= (~encoderFlags >> ((int)EncoderFlags.VvvvvShift - 3)) & 0x78;
			b |= mask_W & encoder.Internal_EVEX_WIG;
			encoder.WriteByteInternal(b);

			b = instruction.InternalOpMask;
			if (b != 0) {
				if ((flags & EvexFlags.k1) == 0)
					encoder.ErrorMessage = "The instruction doesn't support opmask registers";
			}
			else {
				if ((flags & EvexFlags.NonZeroOpMaskRegister) != 0)
					encoder.ErrorMessage = "The instruction must use an opmask register";
			}
			b |= (encoderFlags >> ((int)EncoderFlags.VvvvvShift + 4 - 3)) & 8;
			if (instruction.SuppressAllExceptions) {
				if ((flags & EvexFlags.sae) == 0)
					encoder.ErrorMessage = "The instruction doesn't support suppress-all-exceptions";
				b |= 0x10;
			}
			var rc = instruction.RoundingControl;
			if (rc != RoundingControl.None) {
				if ((flags & EvexFlags.er) == 0)
					encoder.ErrorMessage = "The instruction doesn't support rounding control";
				b |= 0x10;
				Static.Assert((int)RoundingControl.RoundToNearest == 1 ? 0 : -1);
				Static.Assert((int)RoundingControl.RoundDown == 2 ? 0 : -1);
				Static.Assert((int)RoundingControl.RoundUp == 3 ? 0 : -1);
				Static.Assert((int)RoundingControl.RoundTowardZero == 4 ? 0 : -1);
				b |= (uint)(rc - RoundingControl.RoundToNearest) << 5;
			}
			else if ((flags & EvexFlags.sae) == 0 || !instruction.SuppressAllExceptions)
				b |= llBits;
			if ((encoderFlags & (uint)EncoderFlags.Broadcast) != 0) {
				if ((flags & EvexFlags.b) == 0)
					encoder.ErrorMessage = "The instruction doesn't support broadcasting";
				b |= 0x10;
			}
			if (instruction.ZeroingMasking) {
				if ((flags & EvexFlags.z) == 0)
					encoder.ErrorMessage = "The instruction doesn't support zeroing masking";
				b |= 0x80;
			}
			b ^= 8;
			b |= mask_LL & encoder.Internal_EVEX_LIG;
			encoder.WriteByteInternal(b);
		}
	}
#endif

#if !NO_D3NOW
	sealed class D3nowHandler : OpCodeHandler {
		static readonly Op[] operands = new Op[] {
			new OpModRM_reg(Register.MM0, Register.MM7),
			new OpModRM_rm(Register.MM0, Register.MM7),
		};
		readonly uint immediate;

		public D3nowHandler(uint dword1, uint dword2, uint dword3)
			: base(0x0F, -1, OpCodeHandlerFlags.None, (Encodable)((dword2 >> (int)D3nowFlags.EncodableShift) & (uint)D3nowFlags.EncodableMask), OperandSize.None, AddressSize.None, null, operands) {
			immediate = GetOpCode(dword1);
			Debug.Assert(immediate <= byte.MaxValue);
		}

		public override void Encode(Encoder encoder, in Instruction instruction) {
			encoder.WriteByteInternal(0x0F);
			encoder.ImmSize = ImmSize.Size1OpCode;
			encoder.Immediate = immediate;
		}
	}
#endif
}
#endif
