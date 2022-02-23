// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if ENCODER
using System;
using System.Diagnostics;

namespace Iced.Intel.EncoderInternal {
	delegate bool TryConvertToDisp8N(Encoder encoder, OpCodeHandler handler, in Instruction instruction, int displ, out sbyte compressedValue);

	abstract class OpCodeHandler {
		internal readonly uint OpCode;
		internal readonly bool Is2ByteOpCode;
		internal readonly int GroupIndex;
		internal readonly int RmGroupIndex;
		internal readonly bool IsSpecialInstr;
		internal readonly EncFlags3 EncFlags3;
		internal readonly CodeSize OpSize;
		internal readonly CodeSize AddrSize;
		internal readonly TryConvertToDisp8N? TryConvertToDisp8N;
		internal readonly Op[] Operands;
		protected OpCodeHandler(EncFlags2 encFlags2, EncFlags3 encFlags3, bool isSpecialInstr, TryConvertToDisp8N? tryConvertToDisp8N, Op[] operands) {
			EncFlags3 = encFlags3;
			OpCode = GetOpCode(encFlags2);
			Is2ByteOpCode = (encFlags2 & EncFlags2.OpCodeIs2Bytes) != 0;
			GroupIndex = (encFlags2 & EncFlags2.HasGroupIndex) == 0 ? -1 : (int)(((uint)encFlags2 >> (int)EncFlags2.GroupIndexShift) & 7);
			RmGroupIndex = (encFlags3 & EncFlags3.HasRmGroupIndex) == 0 ? -1 : (int)(((uint)encFlags2 >> (int)EncFlags2.GroupIndexShift) & 7);
			IsSpecialInstr = isSpecialInstr;
			OpSize = (CodeSize)(((uint)encFlags3 >> (int)EncFlags3.OperandSizeShift) & (uint)EncFlags3.OperandSizeMask);
			AddrSize = (CodeSize)(((uint)encFlags3 >> (int)EncFlags3.AddressSizeShift) & (uint)EncFlags3.AddressSizeMask);
			TryConvertToDisp8N = tryConvertToDisp8N;
			Operands = operands;
		}

		protected static uint GetOpCode(EncFlags2 encFlags2) => (ushort)((uint)encFlags2 >> (int)EncFlags2.OpCodeShift);
		public abstract void Encode(Encoder encoder, in Instruction instruction);
	}

	sealed class InvalidHandler : OpCodeHandler {
		internal const string ERROR_MESSAGE = "Can't encode an invalid instruction";

		public InvalidHandler() : base(EncFlags2.None, EncFlags3.Bit16or32 | EncFlags3.Bit64, false, null, Array2.Empty<Op>()) { }

		public override void Encode(Encoder encoder, in Instruction instruction) =>
			encoder.ErrorMessage = ERROR_MESSAGE;
	}

	sealed class DeclareDataHandler : OpCodeHandler {
		readonly int elemLength;
		readonly int maxLength;

		public DeclareDataHandler(Code code)
			: base(EncFlags2.None, EncFlags3.Bit16or32 | EncFlags3.Bit64, true, null, Array2.Empty<Op>()) {
			elemLength = code switch {
				Code.DeclareByte => 1,
				Code.DeclareWord => 2,
				Code.DeclareDword => 4,
				Code.DeclareQword => 8,
				_ => throw new InvalidOperationException(),
			};
			maxLength = 16 / elemLength;
		}

		public override void Encode(Encoder encoder, in Instruction instruction) {
			var declDataCount = instruction.DeclareDataCount;
			if (declDataCount < 1 || declDataCount > maxLength) {
				encoder.ErrorMessage = $"Invalid db/dw/dd/dq data count. Count = {declDataCount}, max count = {maxLength}";
				return;
			}
			int length = declDataCount * elemLength;
			for (int i = 0; i < length; i++)
				encoder.WriteByteInternal(instruction.GetDeclareByteValue(i));
		}
	}

	sealed class ZeroBytesHandler : OpCodeHandler {
		public ZeroBytesHandler(Code code)
			: base(EncFlags2.None, EncFlags3.Bit16or32 | EncFlags3.Bit64, true, null, Array2.Empty<Op>()) {
		}

		public override void Encode(Encoder encoder, in Instruction instruction) {
		}
	}

	sealed class LegacyHandler : OpCodeHandler {
		readonly uint tableByte1, tableByte2;
		readonly uint mandatoryPrefix;

		static Op[] CreateOps(EncFlags1 encFlags1) {
			var op0 = (int)(((uint)encFlags1 >> (int)EncFlags1.Legacy_Op0Shift) & (uint)EncFlags1.Legacy_OpMask);
			var op1 = (int)(((uint)encFlags1 >> (int)EncFlags1.Legacy_Op1Shift) & (uint)EncFlags1.Legacy_OpMask);
			var op2 = (int)(((uint)encFlags1 >> (int)EncFlags1.Legacy_Op2Shift) & (uint)EncFlags1.Legacy_OpMask);
			var op3 = (int)(((uint)encFlags1 >> (int)EncFlags1.Legacy_Op3Shift) & (uint)EncFlags1.Legacy_OpMask);
			if (op3 != 0) {
				Debug.Assert(op0 != 0 && op1 != 0 && op2 != 0);
				return new Op[] { OpHandlerData.LegacyOps[op0 - 1], OpHandlerData.LegacyOps[op1 - 1], OpHandlerData.LegacyOps[op2 - 1], OpHandlerData.LegacyOps[op3 - 1] };
			}
			if (op2 != 0) {
				Debug.Assert(op0 != 0 && op1 != 0);
				return new Op[] { OpHandlerData.LegacyOps[op0 - 1], OpHandlerData.LegacyOps[op1 - 1], OpHandlerData.LegacyOps[op2 - 1] };
			}
			if (op1 != 0) {
				Debug.Assert(op0 != 0);
				return new Op[] { OpHandlerData.LegacyOps[op0 - 1], OpHandlerData.LegacyOps[op1 - 1] };
			}
			if (op0 != 0)
				return new Op[] { OpHandlerData.LegacyOps[op0 - 1] };
			return Array2.Empty<Op>();
		}

		public LegacyHandler(EncFlags1 encFlags1, EncFlags2 encFlags2, EncFlags3 encFlags3)
			: base(encFlags2, encFlags3, false, null, CreateOps(encFlags1)) {
			switch ((LegacyOpCodeTable)(((uint)encFlags2 >> (int)EncFlags2.TableShift) & (uint)EncFlags2.TableMask)) {
			case LegacyOpCodeTable.MAP0:
				tableByte1 = 0;
				tableByte2 = 0;
				break;

			case LegacyOpCodeTable.MAP0F:
				tableByte1 = 0x0F;
				tableByte2 = 0;
				break;

			case LegacyOpCodeTable.MAP0F38:
				tableByte1 = 0x0F;
				tableByte2 = 0x38;
				break;

			case LegacyOpCodeTable.MAP0F3A:
				tableByte1 = 0x0F;
				tableByte2 = 0x3A;
				break;

			default:
				throw new InvalidOperationException();
			}

			mandatoryPrefix = (MandatoryPrefixByte)(((uint)encFlags2 >> (int)EncFlags2.MandatoryPrefixShift) & (uint)EncFlags2.MandatoryPrefixMask) switch {
				MandatoryPrefixByte.None => 0x00,
				MandatoryPrefixByte.P66 => 0x66,
				MandatoryPrefixByte.PF3 => 0xF3,
				MandatoryPrefixByte.PF2 => 0xF2,
				_ => throw new InvalidOperationException(),
			};
		}

		public override void Encode(Encoder encoder, in Instruction instruction) {
			uint b = mandatoryPrefix;
			encoder.WritePrefixes(instruction, b != 0xF3);
			if (b != 0)
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

		static Op[] CreateOps(EncFlags1 encFlags1) {
			var op0 = (int)(((uint)encFlags1 >> (int)EncFlags1.VEX_Op0Shift) & (uint)EncFlags1.VEX_OpMask);
			var op1 = (int)(((uint)encFlags1 >> (int)EncFlags1.VEX_Op1Shift) & (uint)EncFlags1.VEX_OpMask);
			var op2 = (int)(((uint)encFlags1 >> (int)EncFlags1.VEX_Op2Shift) & (uint)EncFlags1.VEX_OpMask);
			var op3 = (int)(((uint)encFlags1 >> (int)EncFlags1.VEX_Op3Shift) & (uint)EncFlags1.VEX_OpMask);
			var op4 = (int)(((uint)encFlags1 >> (int)EncFlags1.VEX_Op4Shift) & (uint)EncFlags1.VEX_OpMask);
			if (op4 != 0) {
				Debug.Assert(op0 != 0 && op1 != 0 && op2 != 0 && op3 != 0);
				return new Op[] { OpHandlerData.VexOps[op0 - 1], OpHandlerData.VexOps[op1 - 1], OpHandlerData.VexOps[op2 - 1], OpHandlerData.VexOps[op3 - 1], OpHandlerData.VexOps[op4 - 1] };
			}
			if (op3 != 0) {
				Debug.Assert(op0 != 0 && op1 != 0 && op2 != 0);
				return new Op[] { OpHandlerData.VexOps[op0 - 1], OpHandlerData.VexOps[op1 - 1], OpHandlerData.VexOps[op2 - 1], OpHandlerData.VexOps[op3 - 1] };
			}
			if (op2 != 0) {
				Debug.Assert(op0 != 0 && op1 != 0);
				return new Op[] { OpHandlerData.VexOps[op0 - 1], OpHandlerData.VexOps[op1 - 1], OpHandlerData.VexOps[op2 - 1] };
			}
			if (op1 != 0) {
				Debug.Assert(op0 != 0);
				return new Op[] { OpHandlerData.VexOps[op0 - 1], OpHandlerData.VexOps[op1 - 1] };
			}
			if (op0 != 0)
				return new Op[] { OpHandlerData.VexOps[op0 - 1] };
			return Array2.Empty<Op>();
		}

		public VexHandler(EncFlags1 encFlags1, EncFlags2 encFlags2, EncFlags3 encFlags3)
			: base(encFlags2, encFlags3, false, null, CreateOps(encFlags1)) {
			table = ((uint)encFlags2 >> (int)EncFlags2.TableShift) & (uint)EncFlags2.TableMask;
			var wbit = (WBit)(((uint)encFlags2 >> (int)EncFlags2.WBitShift) & (uint)EncFlags2.WBitMask);
			W1 = wbit == WBit.W1 ? uint.MaxValue : 0;
			var lbit = (LBit)(((uint)encFlags2 >> (int)EncFlags2.LBitShift) & (int)EncFlags2.LBitMask);
			switch (lbit) {
			case LBit.L1:
			case LBit.L256:
				lastByte = 4;
				break;
			}
			if (W1 != 0)
				lastByte |= 0x80;
			lastByte |= ((uint)encFlags2 >> (int)EncFlags2.MandatoryPrefixShift) & (uint)EncFlags2.MandatoryPrefixMask;
			if (wbit == WBit.WIG)
				mask_W_L |= 0x80;
			if (lbit == LBit.LIG) {
				mask_W_L |= 4;
				mask_L |= 4;
			}
		}

		public override void Encode(Encoder encoder, in Instruction instruction) {
			encoder.WritePrefixes(instruction);

			uint encoderFlags = (uint)encoder.EncoderFlags;

			Static.Assert((int)MandatoryPrefixByte.None == 0 ? 0 : -1);
			Static.Assert((int)MandatoryPrefixByte.P66 == 1 ? 0 : -1);
			Static.Assert((int)MandatoryPrefixByte.PF3 == 2 ? 0 : -1);
			Static.Assert((int)MandatoryPrefixByte.PF2 == 3 ? 0 : -1);
			uint b = lastByte;
			b |= (~encoderFlags >> ((int)EncoderFlags.VvvvvShift - 3)) & 0x78;

			if ((encoder.Internal_PreventVEX2 | W1 | (table - (uint)VexOpCodeTable.MAP0F) | (encoderFlags & (uint)(EncoderFlags.X | EncoderFlags.B | EncoderFlags.W))) != 0) {
				encoder.WriteByteInternal(0xC4);
				Static.Assert((int)VexOpCodeTable.MAP0F == 1 ? 0 : -1);
				Static.Assert((int)VexOpCodeTable.MAP0F38 == 2 ? 0 : -1);
				Static.Assert((int)VexOpCodeTable.MAP0F3A == 3 ? 0 : -1);
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

		static Op[] CreateOps(EncFlags1 encFlags1) {
			var op0 = (int)(((uint)encFlags1 >> (int)EncFlags1.XOP_Op0Shift) & (uint)EncFlags1.XOP_OpMask);
			var op1 = (int)(((uint)encFlags1 >> (int)EncFlags1.XOP_Op1Shift) & (uint)EncFlags1.XOP_OpMask);
			var op2 = (int)(((uint)encFlags1 >> (int)EncFlags1.XOP_Op2Shift) & (uint)EncFlags1.XOP_OpMask);
			var op3 = (int)(((uint)encFlags1 >> (int)EncFlags1.XOP_Op3Shift) & (uint)EncFlags1.XOP_OpMask);
			if (op3 != 0) {
				Debug.Assert(op0 != 0 && op1 != 0 && op2 != 0);
				return new Op[] { OpHandlerData.XopOps[op0 - 1], OpHandlerData.XopOps[op1 - 1], OpHandlerData.XopOps[op2 - 1], OpHandlerData.XopOps[op3 - 1] };
			}
			if (op2 != 0) {
				Debug.Assert(op0 != 0 && op1 != 0);
				return new Op[] { OpHandlerData.XopOps[op0 - 1], OpHandlerData.XopOps[op1 - 1], OpHandlerData.XopOps[op2 - 1] };
			}
			if (op1 != 0) {
				Debug.Assert(op0 != 0);
				return new Op[] { OpHandlerData.XopOps[op0 - 1], OpHandlerData.XopOps[op1 - 1] };
			}
			if (op0 != 0)
				return new Op[] { OpHandlerData.XopOps[op0 - 1] };
			return Array2.Empty<Op>();
		}

		public XopHandler(EncFlags1 encFlags1, EncFlags2 encFlags2, EncFlags3 encFlags3)
			: base(encFlags2, encFlags3, false, null, CreateOps(encFlags1)) {
			Static.Assert((int)XopOpCodeTable.MAP8 == 0 ? 0 : -1);
			Static.Assert((int)XopOpCodeTable.MAP9 == 1 ? 0 : -1);
			Static.Assert((int)XopOpCodeTable.MAP10 == 2 ? 0 : -1);
			table = 8 + (((uint)encFlags2 >> (int)EncFlags2.TableShift) & (uint)EncFlags2.TableMask);
			Debug.Assert(table == 8 || table == 9 || table == 10);
			switch ((LBit)(((uint)encFlags2 >> (int)EncFlags2.LBitShift) & (int)EncFlags2.LBitMask)) {
			case LBit.L1:
			case LBit.L256:
				lastByte = 4;
				break;
			}
			var wbit = (WBit)(((uint)encFlags2 >> (int)EncFlags2.WBitShift) & (uint)EncFlags2.WBitMask);
			if (wbit == WBit.W1)
				lastByte |= 0x80;
			lastByte |= ((uint)encFlags2 >> (int)EncFlags2.MandatoryPrefixShift) & (uint)EncFlags2.MandatoryPrefixMask;
		}

		public override void Encode(Encoder encoder, in Instruction instruction) {
			encoder.WritePrefixes(instruction);

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
		readonly TupleType tupleType;
		readonly uint table;
		readonly uint p1Bits;
		readonly uint llBits;
		readonly uint mask_W;
		readonly uint mask_LL;

		static Op[] CreateOps(EncFlags1 encFlags1) {
			var op0 = (int)(((uint)encFlags1 >> (int)EncFlags1.EVEX_Op0Shift) & (uint)EncFlags1.EVEX_OpMask);
			var op1 = (int)(((uint)encFlags1 >> (int)EncFlags1.EVEX_Op1Shift) & (uint)EncFlags1.EVEX_OpMask);
			var op2 = (int)(((uint)encFlags1 >> (int)EncFlags1.EVEX_Op2Shift) & (uint)EncFlags1.EVEX_OpMask);
			var op3 = (int)(((uint)encFlags1 >> (int)EncFlags1.EVEX_Op3Shift) & (uint)EncFlags1.EVEX_OpMask);
			if (op3 != 0) {
				Debug.Assert(op0 != 0 && op1 != 0 && op2 != 0);
				return new Op[] { OpHandlerData.EvexOps[op0 - 1], OpHandlerData.EvexOps[op1 - 1], OpHandlerData.EvexOps[op2 - 1], OpHandlerData.EvexOps[op3 - 1] };
			}
			if (op2 != 0) {
				Debug.Assert(op0 != 0 && op1 != 0);
				return new Op[] { OpHandlerData.EvexOps[op0 - 1], OpHandlerData.EvexOps[op1 - 1], OpHandlerData.EvexOps[op2 - 1] };
			}
			if (op1 != 0) {
				Debug.Assert(op0 != 0);
				return new Op[] { OpHandlerData.EvexOps[op0 - 1], OpHandlerData.EvexOps[op1 - 1] };
			}
			if (op0 != 0)
				return new Op[] { OpHandlerData.EvexOps[op0 - 1] };
			return Array2.Empty<Op>();
		}

		static readonly TryConvertToDisp8N tryConvertToDisp8N = TryConvertToDisp8NImpl.TryConvertToDisp8N;

		public EvexHandler(EncFlags1 encFlags1, EncFlags2 encFlags2, EncFlags3 encFlags3)
			: base(encFlags2, encFlags3, false, tryConvertToDisp8N, CreateOps(encFlags1)) {
			tupleType = (TupleType)(((uint)encFlags3 >> (int)EncFlags3.TupleTypeShift) & (uint)EncFlags3.TupleTypeMask);
			table = ((uint)encFlags2 >> (int)EncFlags2.TableShift) & (uint)EncFlags2.TableMask;
			Static.Assert((int)MandatoryPrefixByte.None == 0 ? 0 : -1);
			Static.Assert((int)MandatoryPrefixByte.P66 == 1 ? 0 : -1);
			Static.Assert((int)MandatoryPrefixByte.PF3 == 2 ? 0 : -1);
			Static.Assert((int)MandatoryPrefixByte.PF2 == 3 ? 0 : -1);
			p1Bits = 4 | (((uint)encFlags2 >> (int)EncFlags2.MandatoryPrefixShift) & (uint)EncFlags2.MandatoryPrefixMask);
			wbit = (WBit)(((uint)encFlags2 >> (int)EncFlags2.WBitShift) & (uint)EncFlags2.WBitMask);
			if (wbit == WBit.W1)
				p1Bits |= 0x80;
			switch ((LBit)(((uint)encFlags2 >> (int)EncFlags2.LBitShift) & (int)EncFlags2.LBitMask)) {
			case LBit.LIG:
				llBits = 0 << 5;
				mask_LL = 3 << 5;
				break;
			case LBit.L0:
			case LBit.LZ:
			case LBit.L128:
				llBits = 0 << 5;
				break;
			case LBit.L1:
			case LBit.L256:
				llBits = 1 << 5;
				break;
			case LBit.L512:
				llBits = 2 << 5;
				break;
			default:
				throw new InvalidOperationException();
			}
			if (wbit == WBit.WIG)
				mask_W |= 0x80;
		}

		sealed class TryConvertToDisp8NImpl {
			public static bool TryConvertToDisp8N(Encoder encoder, OpCodeHandler handler, in Instruction instruction, int displ, out sbyte compressedValue) {
				var evexHandler = (EvexHandler)handler;
				int n = (int)TupleTypeTable.GetDisp8N(evexHandler.tupleType, (encoder.EncoderFlags & EncoderFlags.Broadcast) != 0);
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
			encoder.WritePrefixes(instruction);

			uint encoderFlags = (uint)encoder.EncoderFlags;

			encoder.WriteByteInternal(0x62);

			Static.Assert((int)EvexOpCodeTable.MAP0F == 1 ? 0 : -1);
			Static.Assert((int)EvexOpCodeTable.MAP0F38 == 2 ? 0 : -1);
			Static.Assert((int)EvexOpCodeTable.MAP0F3A == 3 ? 0 : -1);
			Static.Assert((int)EvexOpCodeTable.MAP5 == 5 ? 0 : -1);
			Static.Assert((int)EvexOpCodeTable.MAP6 == 6 ? 0 : -1);
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
				if ((EncFlags3 & EncFlags3.OpMaskRegister) == 0)
					encoder.ErrorMessage = "The instruction doesn't support opmask registers";
			}
			else {
				if ((EncFlags3 & EncFlags3.RequireOpMaskRegister) != 0)
					encoder.ErrorMessage = "The instruction must use an opmask register";
			}
			b |= (encoderFlags >> ((int)EncoderFlags.VvvvvShift + 4 - 3)) & 8;
			if (instruction.SuppressAllExceptions) {
				if ((EncFlags3 & EncFlags3.SuppressAllExceptions) == 0)
					encoder.ErrorMessage = "The instruction doesn't support suppress-all-exceptions";
				b |= 0x10;
			}
			var rc = instruction.RoundingControl;
			if (rc != RoundingControl.None) {
				if ((EncFlags3 & EncFlags3.RoundingControl) == 0)
					encoder.ErrorMessage = "The instruction doesn't support rounding control";
				b |= 0x10;
				Static.Assert((int)RoundingControl.RoundToNearest == 1 ? 0 : -1);
				Static.Assert((int)RoundingControl.RoundDown == 2 ? 0 : -1);
				Static.Assert((int)RoundingControl.RoundUp == 3 ? 0 : -1);
				Static.Assert((int)RoundingControl.RoundTowardZero == 4 ? 0 : -1);
				b |= (uint)(rc - RoundingControl.RoundToNearest) << 5;
			}
			else if ((EncFlags3 & EncFlags3.SuppressAllExceptions) == 0 || !instruction.SuppressAllExceptions)
				b |= llBits;
			if ((encoderFlags & (uint)EncoderFlags.Broadcast) != 0)
				b |= 0x10;
			else if (instruction.IsBroadcast)
				encoder.ErrorMessage = "The instruction doesn't support broadcasting";
			if (instruction.ZeroingMasking) {
				if ((EncFlags3 & EncFlags3.ZeroingMasking) == 0)
					encoder.ErrorMessage = "The instruction doesn't support zeroing masking";
				b |= 0x80;
			}
			b ^= 8;
			b |= mask_LL & encoder.Internal_EVEX_LIG;
			encoder.WriteByteInternal(b);
		}
	}
#endif

#if MVEX
	sealed class MvexHandler : OpCodeHandler {
		readonly WBit wbit;
		readonly uint table;
		readonly uint p1Bits;
		readonly uint mask_W;

		static Op[] CreateOps(EncFlags1 encFlags1) {
			var op0 = (int)(((uint)encFlags1 >> (int)EncFlags1.MVEX_Op0Shift) & (uint)EncFlags1.MVEX_OpMask);
			var op1 = (int)(((uint)encFlags1 >> (int)EncFlags1.MVEX_Op1Shift) & (uint)EncFlags1.MVEX_OpMask);
			var op2 = (int)(((uint)encFlags1 >> (int)EncFlags1.MVEX_Op2Shift) & (uint)EncFlags1.MVEX_OpMask);
			var op3 = (int)(((uint)encFlags1 >> (int)EncFlags1.MVEX_Op3Shift) & (uint)EncFlags1.MVEX_OpMask);
			if (op3 != 0) {
				Debug.Assert(op0 != 0 && op1 != 0 && op2 != 0);
				return new Op[] { OpHandlerData.MvexOps[op0 - 1], OpHandlerData.MvexOps[op1 - 1], OpHandlerData.MvexOps[op2 - 1], OpHandlerData.MvexOps[op3 - 1] };
			}
			if (op2 != 0) {
				Debug.Assert(op0 != 0 && op1 != 0);
				return new Op[] { OpHandlerData.MvexOps[op0 - 1], OpHandlerData.MvexOps[op1 - 1], OpHandlerData.MvexOps[op2 - 1] };
			}
			if (op1 != 0) {
				Debug.Assert(op0 != 0);
				return new Op[] { OpHandlerData.MvexOps[op0 - 1], OpHandlerData.MvexOps[op1 - 1] };
			}
			if (op0 != 0)
				return new Op[] { OpHandlerData.MvexOps[op0 - 1] };
			return Array2.Empty<Op>();
		}

		static readonly TryConvertToDisp8N tryConvertToDisp8N = TryConvertToDisp8NImpl.TryConvertToDisp8N;

		public MvexHandler(EncFlags1 encFlags1, EncFlags2 encFlags2, EncFlags3 encFlags3)
			: base(encFlags2, encFlags3, false, tryConvertToDisp8N, CreateOps(encFlags1)) {
			table = ((uint)encFlags2 >> (int)EncFlags2.TableShift) & (uint)EncFlags2.TableMask;
			Static.Assert((int)MandatoryPrefixByte.None == 0 ? 0 : -1);
			Static.Assert((int)MandatoryPrefixByte.P66 == 1 ? 0 : -1);
			Static.Assert((int)MandatoryPrefixByte.PF3 == 2 ? 0 : -1);
			Static.Assert((int)MandatoryPrefixByte.PF2 == 3 ? 0 : -1);
			p1Bits = ((uint)encFlags2 >> (int)EncFlags2.MandatoryPrefixShift) & (uint)EncFlags2.MandatoryPrefixMask;
			wbit = (WBit)(((uint)encFlags2 >> (int)EncFlags2.WBitShift) & (uint)EncFlags2.WBitMask);
			if (wbit == WBit.W1)
				p1Bits |= 0x80;
			if (wbit == WBit.WIG)
				mask_W |= 0x80;
		}

		sealed class TryConvertToDisp8NImpl {
			public static bool TryConvertToDisp8N(Encoder encoder, OpCodeHandler handler, in Instruction instruction, int displ, out sbyte compressedValue) {
				var mvex = new MvexInfo(instruction.Code);
				int sss = ((int)instruction.MvexRegMemConv - (int)MvexRegMemConv.MemConvNone) & 7;
				var tupleType = (TupleType)MvexTupleTypeLut.Data[(int)mvex.TupleTypeLutKind * 8 + sss];

				int n = (int)TupleTypeTable.GetDisp8N(tupleType, false);
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
			encoder.WritePrefixes(instruction);

			uint encoderFlags = (uint)encoder.EncoderFlags;

			encoder.WriteByteInternal(0x62);

			Static.Assert((int)MvexOpCodeTable.MAP0F == 1 ? 0 : -1);
			Static.Assert((int)MvexOpCodeTable.MAP0F38 == 2 ? 0 : -1);
			Static.Assert((int)MvexOpCodeTable.MAP0F3A == 3 ? 0 : -1);
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
			b |= mask_W & encoder.Internal_MVEX_WIG;
			encoder.WriteByteInternal(b);

			b = instruction.InternalOpMask;
			if (b != 0) {
				if ((EncFlags3 & EncFlags3.OpMaskRegister) == 0)
					encoder.ErrorMessage = "The instruction doesn't support opmask registers";
			}
			else {
				if ((EncFlags3 & EncFlags3.RequireOpMaskRegister) != 0)
					encoder.ErrorMessage = "The instruction must use an opmask register";
			}
			b |= (encoderFlags >> ((int)EncoderFlags.VvvvvShift + 4 - 3)) & 8;
			var mvex = new MvexInfo(instruction.Code);
			var conv = instruction.MvexRegMemConv;
			// Memory ops can only be op0-op2, never op3 (imm8)
			if (instruction.Op0Kind == OpKind.Memory || instruction.Op1Kind == OpKind.Memory || instruction.Op2Kind == OpKind.Memory) {
				if (conv >= MvexRegMemConv.MemConvNone && conv <= MvexRegMemConv.MemConvSint16)
					b |= ((uint)conv - (uint)MvexRegMemConv.MemConvNone) << 4;
				else if (conv == MvexRegMemConv.None) {
					// Nothing, treat it as MvexRegMemConv.MemConvNone
				}
				else
					encoder.ErrorMessage = "Memory operands must use a valid MvexRegMemConv variant, eg. MvexRegMemConv.MemConvNone";
				if (instruction.IsMvexEvictionHint) {
					if (mvex.CanUseEvictionHint)
						b |= 0x80;
					else
						encoder.ErrorMessage = "This instruction doesn't support eviction hint (`{eh}`)";
				}
			}
			else {
				if (instruction.IsMvexEvictionHint)
					encoder.ErrorMessage = "Only memory operands can enable eviction hint (`{eh}`)";
				if (conv == MvexRegMemConv.None) {
					b |= 0x80;
					if (instruction.SuppressAllExceptions) {
						b |= 0x40;
						if ((EncFlags3 & EncFlags3.SuppressAllExceptions) == 0)
							encoder.ErrorMessage = "The instruction doesn't support suppress-all-exceptions";
					}
					var rc = instruction.RoundingControl;
					if (rc == RoundingControl.None) {
						// Nothing
					}
					else {
						if ((EncFlags3 & EncFlags3.RoundingControl) == 0)
							encoder.ErrorMessage = "The instruction doesn't support rounding control";
						else {
							Static.Assert((int)RoundingControl.RoundToNearest == 1 ? 0 : -1);
							Static.Assert((int)RoundingControl.RoundDown == 2 ? 0 : -1);
							Static.Assert((int)RoundingControl.RoundUp == 3 ? 0 : -1);
							Static.Assert((int)RoundingControl.RoundTowardZero == 4 ? 0 : -1);
							b |= ((uint)rc - (uint)RoundingControl.RoundToNearest) << 4;
						}
					}
				}
				else if (conv >= MvexRegMemConv.RegSwizzleNone && conv <= MvexRegMemConv.RegSwizzleDddd) {
					if (instruction.SuppressAllExceptions)
						encoder.ErrorMessage = "Can't use {sae} with register swizzles";
					else if (instruction.RoundingControl != RoundingControl.None)
						encoder.ErrorMessage = "Can't use rounding control with register swizzles";
					b |= (((uint)conv - (uint)MvexRegMemConv.RegSwizzleNone) & 7) << 4;
				}
				else
					encoder.ErrorMessage = "Register operands can't use memory up/down conversions";
			}
			if (mvex.EHBit == MvexEHBit.EH1)
				b |= 0x80;
			b ^= 8;
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

		public D3nowHandler(EncFlags2 encFlags2, EncFlags3 encFlags3)
			: base((EncFlags2)(((uint)encFlags2 & ~(0xFFFF << (int)EncFlags2.OpCodeShift)) | (0x000F << (int)EncFlags2.OpCodeShift)), encFlags3, false, null, operands) {
			immediate = GetOpCode(encFlags2);
			Debug.Assert(immediate <= byte.MaxValue);
		}

		public override void Encode(Encoder encoder, in Instruction instruction) {
			encoder.WritePrefixes(instruction);
			encoder.WriteByteInternal(0x0F);
			encoder.ImmSize = ImmSize.Size1OpCode;
			encoder.Immediate = immediate;
		}
	}
#endif
}
#endif
