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

#if !NO_DECODER
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Iced.Intel.DecoderInternal;

namespace Iced.Intel {
	enum OpSize : byte {
		Size16	= 0,
		Size32	= 1,
		Size64	= 2,
	}

	[Flags]
	enum StateFlags : uint {
		EncodingMask			= 7,
		HasRex					= 0x00000008,
		b						= 0x00000010,
		z						= 0x00000020,
		IsInvalid				= 0x00000040,
		W						= 0x00000080,
		NoImm					= 0x00000100,
		Addr64					= 0x00000200,
		BranchImm8				= 0x00000400,
		Xbegin					= 0x00000800,
		Lock					= 0x00001000,
		AllowLock				= 0x00002000,
	}

	/// <summary>
	/// Decodes 16/32/64-bit x86 instructions
	/// </summary>
	public sealed partial class Decoder {
		ulong instructionPointer;
		readonly CodeReader reader;
		readonly uint[] prefixes;
		readonly RegInfo2[] memRegs16;
		readonly OpCodeHandler[] handlers_XX;
		readonly OpCodeHandler[] handlers_0FXX_VEX;
		readonly OpCodeHandler[] handlers_0F38XX_VEX;
		readonly OpCodeHandler[] handlers_0F3AXX_VEX;
		readonly OpCodeHandler[] handlers_0FXX_EVEX;
		readonly OpCodeHandler[] handlers_0F38XX_EVEX;
		readonly OpCodeHandler[] handlers_0F3AXX_EVEX;
		readonly OpCodeHandler[] handlers_XOP8;
		readonly OpCodeHandler[] handlers_XOP9;
		readonly OpCodeHandler[] handlers_XOPA;
		internal State state;
		internal uint displIndex;
		internal readonly DecoderOptions options;
		internal readonly CodeSize defaultCodeSize;
		readonly OpSize defaultOperandSize, defaultInvertedOperandSize;
		readonly OpSize defaultAddressSize, defaultInvertedAddressSize;
		internal readonly bool is64Mode;

		internal struct State {
			public uint instructionLength;
			public uint modrm, mod, reg, rm;
			public uint extraRegisterBase;		// R << 3
			public uint extraIndexRegisterBase;	// X << 3
			public uint extraBaseRegisterBase;	// B << 3
			public uint vvvv;// V`vvvv. Not stored in inverted form. If 16/32-bit, bits [4:3] are cleared
			public uint aaa;
			public uint extraRegisterBaseEVEX;
			public uint extraBaseRegisterBaseEVEX;
			public uint extraIndexRegisterBaseVSIB;
			public StateFlags flags;
			public byte defaultDsSegment;
			public VectorLength vectorLength;
			public MandatoryPrefix mandatoryPrefix;
			public OpSize operandSize;
			public OpSize addressSize;
			public readonly EncodingKind Encoding => (EncodingKind)(flags & StateFlags.EncodingMask);
		}

		/// <summary>
		/// Current IP/EIP/RIP value
		/// </summary>
		[Obsolete("Use " + nameof(IP) + " instead of this property", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public ulong InstructionPointer {
			get => instructionPointer;
			set => instructionPointer = value;
		}

		/// <summary>
		/// Current IP/EIP/RIP value
		/// </summary>
		public ulong IP {
			get => instructionPointer;
			set => instructionPointer = value;
		}

		/// <summary>
		/// Gets the bitness (16, 32 or 64)
		/// </summary>
		public int Bitness { get; }

		static readonly uint[] prefixes1632 = CreatePrefixes(false);
		static readonly uint[] prefixes64 = CreatePrefixes(true);

		static uint[] CreatePrefixes(bool is64) {
			var data = new uint[0x100 / 32];
			SetBit(data, 0x26);
			SetBit(data, 0x2E);
			SetBit(data, 0x36);
			SetBit(data, 0x3E);
			SetBit(data, 0x64);
			SetBit(data, 0x65);
			SetBit(data, 0x66);
			SetBit(data, 0x67);
			SetBit(data, 0xF0);
			SetBit(data, 0xF2);
			SetBit(data, 0xF3);
			if (is64) {
				for (int i = 0x40; i <= 0x4F; i++)
					SetBit(data, i);
			}
			return data;

			static void SetBit(uint[] d, int b) => d[b / 32] |= 1U << (b & 31);
		}

		static Decoder() {
			// Initialize cctors that are used by decoder related methods. It doesn't speed up
			// decoding much, but getting instruction info is a little faster.
			_ = OpCodeHandler_Invalid.Instance;
			_ = InstructionMemorySizes.Sizes;
			_ = OpCodeHandler_D3NOW.CodeValues;
			_ = InstructionOpCounts.OpCount;
			_ = MnemonicUtils.toMnemonic;
#if !NO_INSTR_INFO
			_ = RegisterExtensions.RegisterInfos;
			_ = MemorySizeExtensions.MemorySizeInfos;
			_ = InstructionInfoInternal.InfoHandlers.Data;
			_ = InstructionInfoInternal.RflagsInfoConstants.flagsCleared;
			_ = InstructionInfoInternal.CpuidFeatureInternalData.ToCpuidFeatures;
			_ = InstructionInfoInternal.SimpleList<UsedRegister>.Empty;
			_ = InstructionInfoInternal.SimpleList<UsedMemory>.Empty;
#endif
		}

		Decoder(CodeReader reader, DecoderOptions options, OpSize defaultOpSize) {
			this.reader = reader ?? throw new ArgumentNullException(nameof(reader));
			this.options = options;
			memRegs16 = s_memRegs16;
			if (defaultOpSize == OpSize.Size64) {
				is64Mode = true;
				Bitness = 64;
				defaultCodeSize = CodeSize.Code64;
				defaultOperandSize = OpSize.Size32;
				defaultInvertedOperandSize = OpSize.Size16;
				defaultAddressSize = OpSize.Size64;
				defaultInvertedAddressSize = OpSize.Size32;
				prefixes = prefixes64;
			}
			else if (defaultOpSize == OpSize.Size32) {
				is64Mode = false;
				Bitness = 32;
				defaultCodeSize = CodeSize.Code32;
				defaultOperandSize = defaultOpSize;
				defaultInvertedOperandSize = OpSize.Size16;
				defaultAddressSize = defaultOpSize;
				defaultInvertedAddressSize = OpSize.Size16;
				prefixes = prefixes1632;
			}
			else {
				Debug.Assert(defaultOpSize == OpSize.Size16);
				is64Mode = false;
				Bitness = 16;
				defaultCodeSize = CodeSize.Code16;
				defaultOperandSize = defaultOpSize;
				defaultInvertedOperandSize = OpSize.Size32;
				defaultAddressSize = defaultOpSize;
				defaultInvertedAddressSize = OpSize.Size32;
				prefixes = prefixes1632;
			}
			handlers_XX = DecoderInternal.OpCodeHandlersTables_Legacy.OneByteHandlers;
			handlers_0FXX_VEX = DecoderInternal.OpCodeHandlersTables_VEX.TwoByteHandlers_0FXX;
			handlers_0F38XX_VEX = DecoderInternal.OpCodeHandlersTables_VEX.ThreeByteHandlers_0F38XX;
			handlers_0F3AXX_VEX = DecoderInternal.OpCodeHandlersTables_VEX.ThreeByteHandlers_0F3AXX;
			handlers_0FXX_EVEX = DecoderInternal.OpCodeHandlersTables_EVEX.TwoByteHandlers_0FXX;
			handlers_0F38XX_EVEX = DecoderInternal.OpCodeHandlersTables_EVEX.ThreeByteHandlers_0F38XX;
			handlers_0F3AXX_EVEX = DecoderInternal.OpCodeHandlersTables_EVEX.ThreeByteHandlers_0F3AXX;
			handlers_XOP8 = DecoderInternal.OpCodeHandlersTables_XOP.XOP8;
			handlers_XOP9 = DecoderInternal.OpCodeHandlersTables_XOP.XOP9;
			handlers_XOPA = DecoderInternal.OpCodeHandlersTables_XOP.XOPA;
		}

		/// <summary>
		/// Creates a decoder
		/// </summary>
		/// <param name="bitness">16, 32 or 64</param>
		/// <param name="reader">Code reader</param>
		/// <param name="options">Decoder options</param>
		/// <returns></returns>
		public static Decoder Create(int bitness, CodeReader reader, DecoderOptions options = DecoderOptions.None) {
			switch (bitness) {
			case 16: return new Decoder(reader, options, OpSize.Size16);
			case 32: return new Decoder(reader, options, OpSize.Size32);
			case 64: return new Decoder(reader, options, OpSize.Size64);
			default: throw new ArgumentOutOfRangeException(nameof(bitness));
			}
		}

		/// <summary>
		/// Creates a decoder that decodes 16-bit code
		/// </summary>
		/// <param name="reader">Code reader</param>
		/// <param name="options">Decoder options</param>
		/// <returns></returns>
		[Obsolete("Use " + nameof(Create) + " instead", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static Decoder Create16(CodeReader reader, DecoderOptions options = DecoderOptions.None) => Create(16, reader, options);

		/// <summary>
		/// Creates a decoder that decodes 32-bit code
		/// </summary>
		/// <param name="reader">Code reader</param>
		/// <param name="options">Decoder options</param>
		/// <returns></returns>
		[Obsolete("Use " + nameof(Create) + " instead", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static Decoder Create32(CodeReader reader, DecoderOptions options = DecoderOptions.None) => Create(32, reader, options);

		/// <summary>
		/// Creates a decoder that decodes 64-bit code
		/// </summary>
		/// <param name="reader">Code reader</param>
		/// <param name="options">Decoder options</param>
		/// <returns></returns>
		[Obsolete("Use " + nameof(Create) + " instead", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static Decoder Create64(CodeReader reader, DecoderOptions options = DecoderOptions.None) => Create(64, reader, options);

		internal uint ReadByte() {
			uint instrLen = state.instructionLength;
			if (instrLen < DecoderConstants.MaxInstructionLength) {
				uint b = (uint)reader.ReadByte();
				Debug.Assert(b <= byte.MaxValue || b > int.MaxValue);
				if (b <= byte.MaxValue) {
					state.instructionLength = instrLen + 1;
					return b;
				}
			}
			state.flags |= StateFlags.IsInvalid;
			return 0;
		}

		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		internal uint ReadUInt16() => ReadByte() | (ReadByte() << 8);
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		internal uint ReadUInt32() => ReadByte() | (ReadByte() << 8) | (ReadByte() << 16) | (ReadByte() << 24);

		/// <summary>
		/// Decodes the next instruction, see also <see cref="Decode(out Instruction)"/> which is faster
		/// if you already have an <see cref="Instruction"/> local, array element or field.
		/// </summary>
		/// <returns></returns>
		public Instruction Decode() {
			Decode(out var instr);
			return instr;
		}

		/// <summary>
		/// Decodes the next instruction
		/// </summary>
		/// <param name="instruction">Decoded instruction</param>
		public void Decode(out Instruction instruction) {
			instruction = default;
			// JIT32: it's 9% slower decoding instructions if we clear the whole 'state'
			// 32-bit RyuJIT: not tested
			// 64-bit RyuJIT: diff is too small to care about
#if truex
			state = default;
#else
			state.instructionLength = 0;
			state.extraRegisterBase = 0;
			state.extraIndexRegisterBase = 0;
			state.extraBaseRegisterBase = 0;
			state.flags = 0;
			state.mandatoryPrefix = 0;
			state.extraIndexRegisterBaseVSIB = 0;
#endif
			state.defaultDsSegment = (byte)Register.DS;
			state.operandSize = defaultOperandSize;
			state.addressSize = defaultAddressSize;
			uint rexPrefix = 0;
			uint b;
			for (;;) {
				b = ReadByte();
				// RyuJIT32: 2-5% faster, RyuJIT64: almost no improvement
				if (((prefixes[(int)(b / 32)] >> ((int)b & 31)) & 1) == 0)
					break;
				// Converting these prefixes to opcode handlers instead of a switch results in slightly worse perf
				// with JIT32, and about the same speed with 64-bit RyuJIT.
				switch (b) {
				case 0x26:
					if (!is64Mode || (state.defaultDsSegment != (byte)Register.FS && state.defaultDsSegment != (byte)Register.GS)) {
						instruction.SegmentPrefix = Register.ES;
						state.defaultDsSegment = (byte)Register.ES;
					}
					rexPrefix = 0;
					break;

				case 0x2E:
					if (!is64Mode || (state.defaultDsSegment != (byte)Register.FS && state.defaultDsSegment != (byte)Register.GS)) {
						instruction.SegmentPrefix = Register.CS;
						state.defaultDsSegment = (byte)Register.CS;
					}
					rexPrefix = 0;
					break;

				case 0x36:
					if (!is64Mode || (state.defaultDsSegment != (byte)Register.FS && state.defaultDsSegment != (byte)Register.GS)) {
						instruction.SegmentPrefix = Register.SS;
						state.defaultDsSegment = (byte)Register.SS;
					}
					rexPrefix = 0;
					break;

				case 0x3E:
					if (!is64Mode || (state.defaultDsSegment != (byte)Register.FS && state.defaultDsSegment != (byte)Register.GS)) {
						instruction.SegmentPrefix = Register.DS;
						state.defaultDsSegment = (byte)Register.DS;
					}
					rexPrefix = 0;
					break;

				case 0x64:
					instruction.SegmentPrefix = Register.FS;
					state.defaultDsSegment = (byte)Register.FS;
					rexPrefix = 0;
					break;

				case 0x65:
					instruction.SegmentPrefix = Register.GS;
					state.defaultDsSegment = (byte)Register.GS;
					rexPrefix = 0;
					break;

				case 0x66:
					state.operandSize = defaultInvertedOperandSize;
					rexPrefix = 0;
					if (state.mandatoryPrefix == MandatoryPrefix.None)
						state.mandatoryPrefix = MandatoryPrefix.P66;
					break;

				case 0x67:
					state.addressSize = defaultInvertedAddressSize;
					rexPrefix = 0;
					break;

				case 0xF0:
					instruction.InternalSetHasLockPrefix();
					state.flags |= StateFlags.Lock;
					rexPrefix = 0;
					break;

				case 0xF2:
					instruction.InternalSetHasRepnePrefix();
					rexPrefix = 0;
					state.mandatoryPrefix = MandatoryPrefix.PF2;
					break;

				case 0xF3:
					instruction.InternalSetHasRepePrefix();
					rexPrefix = 0;
					state.mandatoryPrefix = MandatoryPrefix.PF3;
					break;

				default:
					if (is64Mode && (b & 0xF0) == 0x40) {
						rexPrefix = b;
						break;
					}
					goto after_read_prefixes;
				}
			}
after_read_prefixes:
			if (rexPrefix != 0) {
				state.flags |= StateFlags.HasRex;
				if ((rexPrefix & 8) != 0) {
					state.operandSize = OpSize.Size64;
					state.flags |= StateFlags.W;
				}
				state.extraRegisterBase = (rexPrefix & 4) << 1;
				state.extraIndexRegisterBase = (rexPrefix & 2) << 2;
				state.extraBaseRegisterBase = (rexPrefix & 1) << 3;
			}
			DecodeTable(handlers_XX[b], ref instruction);
			var flags = state.flags;
			if ((flags & (StateFlags.IsInvalid | StateFlags.Lock)) != 0) {
				if ((flags & StateFlags.IsInvalid) != 0 ||
					((options & DecoderOptions.NoInvalidCheck) == 0 && (flags & (StateFlags.Lock | StateFlags.AllowLock)) == StateFlags.Lock)) {
					instruction = default;
					Debug.Assert(Code.INVALID == 0);
					//instruction.InternalCode = Code.INVALID;
				}
			}
			instruction.InternalCodeSize = defaultCodeSize;
			uint instrLen = state.instructionLength;
			Debug.Assert(0 <= instrLen && instrLen <= DecoderConstants.MaxInstructionLength);// Could be 0 if there were no bytes available
			instruction.InternalByteLength = instrLen;
			var ip = instructionPointer;
			ip += instrLen;
			instructionPointer = ip;
			instruction.NextIP = ip;
		}

		internal uint GetCurrentInstructionPointer32() => (uint)instructionPointer + state.instructionLength;
		internal ulong GetCurrentInstructionPointer64() => instructionPointer + state.instructionLength;

		internal void ClearMandatoryPrefix(ref Instruction instruction) {
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			switch (state.mandatoryPrefix) {
			case MandatoryPrefix.P66:
				state.operandSize = defaultOperandSize;
				break;
			case MandatoryPrefix.PF3:
				instruction.InternalClearHasRepePrefix();
				break;
			case MandatoryPrefix.PF2:
				instruction.InternalClearHasRepnePrefix();
				break;
			}
		}

		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		internal void SetXacquireRelease(ref Instruction instruction, HandlerFlags flags) {
			if ((flags & HandlerFlags.XacquireReleaseNoLock) != 0 || instruction.HasLockPrefix)
				SetXacquireReleaseCore(ref instruction, flags);
		}

		void SetXacquireReleaseCore(ref Instruction instruction, HandlerFlags flags) {
			Debug.Assert(!((flags & HandlerFlags.XacquireReleaseNoLock) == 0 && !instruction.HasLockPrefix));
			switch (state.mandatoryPrefix) {
			case MandatoryPrefix.PF2:
				if ((flags & HandlerFlags.Xacquire) != 0) {
					ClearMandatoryPrefixF2(ref instruction);
					instruction.InternalSetHasXacquirePrefix();
				}
				break;

			case MandatoryPrefix.PF3:
				if ((flags & HandlerFlags.Xrelease) != 0) {
					ClearMandatoryPrefixF3(ref instruction);
					instruction.InternalSetHasXreleasePrefix();
				}
				break;
			}
		}

		internal void ClearMandatoryPrefixF3(ref Instruction instruction) {
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			Debug.Assert(state.mandatoryPrefix == MandatoryPrefix.PF3);
			instruction.InternalClearHasRepePrefix();
		}

		internal void ClearMandatoryPrefixF2(ref Instruction instruction) {
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			Debug.Assert(state.mandatoryPrefix == MandatoryPrefix.PF2);
			instruction.InternalClearHasRepnePrefix();
		}

		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		internal void SetInvalidInstruction() => state.flags |= StateFlags.IsInvalid;

		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		internal void DecodeTable(OpCodeHandler[] table, ref Instruction instruction) => DecodeTable(table[(int)ReadByte()], ref instruction);

		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		void DecodeTable(OpCodeHandler handler, ref Instruction instruction) {
			if (handler.HasModRM) {
				uint m = ReadByte();
				state.modrm = m;
				state.mod = m >> 6;
				state.reg = (m >> 3) & 7;
				state.rm = m & 7;
			}
			handler.Decode(this, ref instruction);
		}

		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		internal void ReadModRM() {
			uint m = ReadByte();
			state.modrm = m;
			state.mod = m >> 6;
			state.reg = (m >> 3) & 7;
			state.rm = m & 7;
		}

		internal void VEX2(ref Instruction instruction) {
			if ((options & DecoderOptions.NoInvalidCheck) == 0 && ((uint)(state.flags & StateFlags.HasRex) + (uint)state.mandatoryPrefix) != 0)
				SetInvalidInstruction();

			state.flags |= (StateFlags)EncodingKind.VEX;
			uint b = state.modrm;
			if (is64Mode && (b & 0x80) == 0)
				state.extraRegisterBase = 8;
			// Bit 6 can only be 1 if it's 16/32-bit mode, so we don't need to change the mask
			state.vvvv = (~b >> 3) & 0x0F;

			Debug.Assert((int)VectorLength.L128 == 0);
			Debug.Assert((int)VectorLength.L256 == 1);
			state.vectorLength = (VectorLength)((b >> 2) & 1);

			Debug.Assert((int)MandatoryPrefix.None == 0);
			Debug.Assert((int)MandatoryPrefix.P66 == 1);
			Debug.Assert((int)MandatoryPrefix.PF3 == 2);
			Debug.Assert((int)MandatoryPrefix.PF2 == 3);
			state.mandatoryPrefix = (MandatoryPrefix)(b & 3);

			DecodeTable(handlers_0FXX_VEX, ref instruction);
		}

		internal void VEX3(ref Instruction instruction) {
			if ((options & DecoderOptions.NoInvalidCheck) == 0 && ((uint)(state.flags & StateFlags.HasRex) + (uint)state.mandatoryPrefix) != 0)
				SetInvalidInstruction();

			state.flags |= (StateFlags)EncodingKind.VEX;
			uint b1 = state.modrm;
			uint b2 = ReadByte();

			Debug.Assert((int)StateFlags.W == 0x80);
			state.flags |= (StateFlags)(b2 & 0x80);

			Debug.Assert((int)VectorLength.L128 == 0);
			Debug.Assert((int)VectorLength.L256 == 1);
			state.vectorLength = (VectorLength)((b2 >> 2) & 1);

			Debug.Assert((int)MandatoryPrefix.None == 0);
			Debug.Assert((int)MandatoryPrefix.P66 == 1);
			Debug.Assert((int)MandatoryPrefix.PF3 == 2);
			Debug.Assert((int)MandatoryPrefix.PF2 == 3);
			state.mandatoryPrefix = (MandatoryPrefix)(b2 & 3);

			if (is64Mode) {
				if ((b2 & 0x80) != 0)
					state.operandSize = OpSize.Size64;
				state.vvvv = (~b2 >> 3) & 0x0F;
				uint b1x = ~b1;
				state.extraRegisterBase = (b1x >> 4) & 8;
				state.extraIndexRegisterBase = (b1x >> 3) & 8;
				state.extraBaseRegisterBase = (b1x >> 2) & 8;
			}
			else
				state.vvvv = (~b2 >> 3) & 0x07;

			int table = (int)(b1 & 0x1F);
			if (table == 1)
				DecodeTable(handlers_0FXX_VEX, ref instruction);
			else if (table == 2)
				DecodeTable(handlers_0F38XX_VEX, ref instruction);
			else if (table == 3)
				DecodeTable(handlers_0F3AXX_VEX, ref instruction);
			else
				SetInvalidInstruction();
		}

		internal void XOP(ref Instruction instruction) {
			if ((options & DecoderOptions.NoInvalidCheck) == 0 && ((uint)(state.flags & StateFlags.HasRex) + (uint)state.mandatoryPrefix) != 0)
				SetInvalidInstruction();

			state.flags |= (StateFlags)EncodingKind.XOP;
			uint b1 = state.modrm;
			uint b2 = ReadByte();

			Debug.Assert((int)StateFlags.W == 0x80);
			state.flags |= (StateFlags)(b2 & 0x80);

			Debug.Assert((int)VectorLength.L128 == 0);
			Debug.Assert((int)VectorLength.L256 == 1);
			state.vectorLength = (VectorLength)((b2 >> 2) & 1);

			Debug.Assert((int)MandatoryPrefix.None == 0);
			Debug.Assert((int)MandatoryPrefix.P66 == 1);
			Debug.Assert((int)MandatoryPrefix.PF3 == 2);
			Debug.Assert((int)MandatoryPrefix.PF2 == 3);
			state.mandatoryPrefix = (MandatoryPrefix)(b2 & 3);

			if (is64Mode) {
				if ((b2 & 0x80) != 0)
					state.operandSize = OpSize.Size64;
				state.vvvv = (~b2 >> 3) & 0x0F;
				uint b1x = ~b1;
				state.extraRegisterBase = (b1x >> 4) & 8;
				state.extraIndexRegisterBase = (b1x >> 3) & 8;
				state.extraBaseRegisterBase = (b1x >> 2) & 8;
			}
			else
				state.vvvv = (~b2 >> 3) & 0x07;

			int table = (int)(b1 & 0x1F);
			if (table == 8)
				DecodeTable(handlers_XOP8, ref instruction);
			else if (table == 9)
				DecodeTable(handlers_XOP9, ref instruction);
			else if (table == 10)
				DecodeTable(handlers_XOPA, ref instruction);
			else
				SetInvalidInstruction();
		}

		internal void EVEX_MVEX(ref Instruction instruction) {
			if ((options & DecoderOptions.NoInvalidCheck) == 0 && ((uint)(state.flags & StateFlags.HasRex) + (uint)state.mandatoryPrefix) != 0)
				SetInvalidInstruction();

			uint p0 = state.modrm;
			uint p1 = ReadByte();
			uint p2 = ReadByte();

			if ((p1 & 4) == 0) {
				//TODO: Support deprecated MVEX instructions: https://github.com/0xd4d/iced/issues/2
				SetInvalidInstruction();
			}
			else {
				if ((p0 & 0x0C) != 0) {
					SetInvalidInstruction();
					return;
				}

				state.flags |= (StateFlags)EncodingKind.EVEX;

				Debug.Assert((int)MandatoryPrefix.None == 0);
				Debug.Assert((int)MandatoryPrefix.P66 == 1);
				Debug.Assert((int)MandatoryPrefix.PF3 == 2);
				Debug.Assert((int)MandatoryPrefix.PF2 == 3);
				state.mandatoryPrefix = (MandatoryPrefix)(p1 & 3);

				Debug.Assert((int)StateFlags.W == 0x80);
				state.flags |= (StateFlags)(p1 & 0x80);

				uint aaa = p2 & 7;
				state.aaa = aaa;
				Debug.Assert((int)StateFlags.z == 0x20);
				state.flags |= (StateFlags)(p2 >> 2) & StateFlags.z;
				if ((options & DecoderOptions.NoInvalidCheck) == 0 && aaa == 0 && (state.flags & StateFlags.z) != 0)
					SetInvalidInstruction();

				Debug.Assert((int)StateFlags.b == 0x10);
				state.flags |= (StateFlags)(p2 & 0x10);

				Debug.Assert((int)VectorLength.L128 == 0);
				Debug.Assert((int)VectorLength.L256 == 1);
				Debug.Assert((int)VectorLength.L512 == 2);
				Debug.Assert((int)VectorLength.Unknown == 3);
				state.vectorLength = (VectorLength)((p2 >> 5) & 3);

				if (is64Mode) {
					state.vvvv = (~p1 >> 3) & 0x0F;
					uint tmp = (~p2 & 8) << 1;
					state.vvvv += tmp;
					state.extraIndexRegisterBaseVSIB = tmp;
					if ((p1 & 0x80) != 0)
						state.operandSize = OpSize.Size64;
					uint p0x = ~p0;
					state.extraRegisterBase = (p0x >> 4) & 8;
					state.extraIndexRegisterBase = (p0x & 0x40) >> 3;
					state.extraBaseRegisterBaseEVEX = (p0x & 0x40) >> 2;
					state.extraBaseRegisterBase = (p0x >> 2) & 8;
					state.extraRegisterBaseEVEX = p0x & 0x10;
				}
				else
					state.vvvv = (~p1 >> 3) & 0x07;

				int table = (int)(p0 & 3);
				if (table == 1)
					DecodeTable(handlers_0FXX_EVEX, ref instruction);
				else if (table == 2)
					DecodeTable(handlers_0F38XX_EVEX, ref instruction);
				else if (table == 3)
					DecodeTable(handlers_0F3AXX_EVEX, ref instruction);
				else
					SetInvalidInstruction();
			}
		}

		// Return type is uint since caller will write to a uint field
		internal uint ReadIb() => ReadByte();

		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		internal Register ReadOpSw() {
			uint reg = state.reg;
			if (reg >= 6) {
				state.flags |= StateFlags.IsInvalid;
				return Register.None;
			}
			else
				return Register.ES + (int)reg;
		}

		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		internal void ReadOpMem(ref Instruction instruction) {
			Debug.Assert(state.Encoding != EncodingKind.EVEX);
			if (state.addressSize == OpSize.Size64)
				ReadOpMem32Or64(ref instruction, Register.RAX, Register.RAX, TupleType.None, false);
			else if (state.addressSize == OpSize.Size32)
				ReadOpMem32Or64(ref instruction, Register.EAX, Register.EAX, TupleType.None, false);
			else
				ReadOpMem16(ref instruction, TupleType.None);
		}

		// All MPX instructions in 64-bit mode force 64-bit addressing, and
		// all MPX instructions in 16/32-bit mode require 32-bit addressing
		// (see SDM Vol 1, 17.5.1 Intel MPX and Operating Modes)
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		internal void ReadOpMem_MPX(ref Instruction instruction) {
			Debug.Assert(state.Encoding != EncodingKind.EVEX);
			if (is64Mode) {
				state.addressSize = OpSize.Size64;
				ReadOpMem32Or64(ref instruction, Register.RAX, Register.RAX, TupleType.None, false);
			}
			else if (state.addressSize == OpSize.Size32)
				ReadOpMem32Or64(ref instruction, Register.EAX, Register.EAX, TupleType.None, false);
			else {
				ReadOpMem16(ref instruction, TupleType.None);
				if ((options & DecoderOptions.NoInvalidCheck) == 0)
					SetInvalidInstruction();
			}
		}

		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		internal void ReadOpMem(ref Instruction instruction, TupleType tupleType) {
			Debug.Assert(state.Encoding == EncodingKind.EVEX);
			if (state.addressSize == OpSize.Size64)
				ReadOpMem32Or64(ref instruction, Register.RAX, Register.RAX, tupleType, false);
			else if (state.addressSize == OpSize.Size32)
				ReadOpMem32Or64(ref instruction, Register.EAX, Register.EAX, tupleType, false);
			else
				ReadOpMem16(ref instruction, tupleType);
		}

		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		internal void ReadOpMem_VSIB(ref Instruction instruction, Register vsibIndex, TupleType tupleType) {
			bool isInvalid;
			if (state.addressSize == OpSize.Size64)
				isInvalid = !ReadOpMem32Or64(ref instruction, Register.RAX, vsibIndex, tupleType, true);
			else if (state.addressSize == OpSize.Size32)
				isInvalid = !ReadOpMem32Or64(ref instruction, Register.EAX, vsibIndex, tupleType, true);
			else {
				isInvalid = true;
				ReadOpMem16(ref instruction, tupleType);
			}
			if ((options & DecoderOptions.NoInvalidCheck) == 0 && isInvalid)
				SetInvalidInstruction();
		}

		readonly struct RegInfo2 {
			public readonly Register baseReg;
			public readonly Register indexReg;
			public RegInfo2(Register baseReg, Register indexReg) {
				this.baseReg = baseReg;
				this.indexReg = indexReg;
			}
			public void Deconstruct(out Register baseReg, out Register indexReg) {
				baseReg = this.baseReg;
				indexReg = this.indexReg;
			}
		}
		static readonly RegInfo2[] s_memRegs16 = new RegInfo2[8] {
			new RegInfo2(Register.BX, Register.SI),
			new RegInfo2(Register.BX, Register.DI),
			new RegInfo2(Register.BP, Register.SI),
			new RegInfo2(Register.BP, Register.DI),
			new RegInfo2(Register.SI, Register.None),
			new RegInfo2(Register.DI, Register.None),
			new RegInfo2(Register.BP, Register.None),
			new RegInfo2(Register.BX, Register.None),
		};
		void ReadOpMem16(ref Instruction instruction, TupleType tupleType) {
			Debug.Assert(state.addressSize == OpSize.Size16);
			var (baseReg, indexReg) = memRegs16[(int)state.rm];
			switch ((int)state.mod) {
			case 0:
				if (state.rm == 6) {
					instruction.InternalSetMemoryDisplSize(2);
					displIndex = state.instructionLength;
					instruction.MemoryDisplacement = ReadUInt16();
					baseReg = Register.None;
					Debug.Assert(indexReg == Register.None);
				}
				break;

			case 1:
				instruction.InternalSetMemoryDisplSize(1);
				displIndex = state.instructionLength;
				if (tupleType == TupleType.None)
					instruction.MemoryDisplacement = (ushort)(sbyte)ReadByte();
				else
					instruction.MemoryDisplacement = (ushort)(GetDisp8N(tupleType) * (uint)(sbyte)ReadByte());
				break;

			default:
				Debug.Assert(state.mod == 2);
				instruction.InternalSetMemoryDisplSize(2);
				displIndex = state.instructionLength;
				instruction.MemoryDisplacement = ReadUInt16();
				break;
			}

			instruction.InternalMemoryBase = baseReg;
			instruction.InternalMemoryIndex = indexReg;
		}

		// Returns true if the SIB byte was read
		bool ReadOpMem32Or64(ref Instruction instruction, Register baseReg, Register indexReg, TupleType tupleType, bool isVsib) {
			Debug.Assert(state.addressSize == OpSize.Size32 || state.addressSize == OpSize.Size64);
			uint sib;
			uint displSizeScale, displ;
			switch ((int)state.mod) {
			case 0:
				if (state.rm == 4) {
					sib = ReadByte();
					displSizeScale = 0;
					displ = 0;
					break;
				}
				else if (state.rm == 5) {
					if (state.addressSize == OpSize.Size64)
						instruction.InternalSetMemoryDisplSize(4);
					else
						instruction.InternalSetMemoryDisplSize(3);
					displIndex = state.instructionLength;
					instruction.MemoryDisplacement = ReadUInt32();
					if (is64Mode) {
						if (state.addressSize == OpSize.Size64)
							instruction.InternalMemoryBase = Register.RIP;
						else
							instruction.InternalMemoryBase = Register.EIP;
					}
					return false;
				}
				else {
					Debug.Assert(0 <= state.rm && state.rm <= 7 && state.rm != 4 && state.rm != 5);
					instruction.InternalMemoryBase = (int)(state.extraBaseRegisterBase + state.rm) + baseReg;
					return false;
				}

			case 1:
				if (state.rm == 4) {
					sib = ReadByte();
					displSizeScale = 1;
					displIndex = state.instructionLength;
					if (tupleType == TupleType.None)
						displ = (uint)(sbyte)ReadByte();
					else
						displ = GetDisp8N(tupleType) * (uint)(sbyte)ReadByte();
					break;
				}
				else {
					Debug.Assert(0 <= state.rm && state.rm <= 7 && state.rm != 4);
					instruction.InternalSetMemoryDisplSize(1);
					displIndex = state.instructionLength;
					if (tupleType == TupleType.None)
						instruction.MemoryDisplacement = (uint)(sbyte)ReadByte();
					else
						instruction.MemoryDisplacement = GetDisp8N(tupleType) * (uint)(sbyte)ReadByte();
					instruction.InternalMemoryBase = (int)(state.extraBaseRegisterBase + state.rm) + baseReg;
					return false;
				}

			case 2:
				if (state.rm == 4) {
					sib = ReadByte();
					displSizeScale = state.addressSize == OpSize.Size64 ? 4U : 3;
					displIndex = state.instructionLength;
					displ = ReadUInt32();
					break;
				}
				else {
					Debug.Assert(0 <= state.rm && state.rm <= 7 && state.rm != 4);
					if (state.addressSize == OpSize.Size64)
						instruction.InternalSetMemoryDisplSize(4);
					else
						instruction.InternalSetMemoryDisplSize(3);
					displIndex = state.instructionLength;
					instruction.MemoryDisplacement = ReadUInt32();
					instruction.InternalMemoryBase = (int)(state.extraBaseRegisterBase + state.rm) + baseReg;
					return false;
				}

			default:
				Debug.Fail("Not reachable");
				return false;
			}

			uint index = ((sib >> 3) & 7) + state.extraIndexRegisterBase;
			uint @base = sib & 7;

			instruction.InternalMemoryIndexScale = (int)(sib >> 6);
			if (!isVsib) {
				if (index != 4)
					instruction.InternalMemoryIndex = (int)index + indexReg;
			}
			else
				instruction.InternalMemoryIndex = (int)(index + state.extraIndexRegisterBaseVSIB) + indexReg;

			if (@base == 5 && state.mod == 0) {
				if (state.addressSize == OpSize.Size64)
					instruction.InternalSetMemoryDisplSize(4);
				else
					instruction.InternalSetMemoryDisplSize(3);
				displIndex = state.instructionLength;
				instruction.MemoryDisplacement = ReadUInt32();
			}
			else {
				instruction.InternalMemoryBase = (int)(@base + state.extraBaseRegisterBase) + baseReg;
				instruction.InternalSetMemoryDisplSize(displSizeScale);
				instruction.MemoryDisplacement = displ;
			}
			return true;
		}

		uint GetDisp8N(TupleType tupleType) {
			switch (tupleType) {
			case TupleType.None:
				return 1;

			case TupleType.Full_128:
				if ((state.flags & StateFlags.b) != 0)
					return (state.flags & StateFlags.W) != 0 ? 8U : 4;
				return 16;

			case TupleType.Full_256:
				if ((state.flags & StateFlags.b) != 0)
					return (state.flags & StateFlags.W) != 0 ? 8U : 4;
				return 32;

			case TupleType.Full_512:
				if ((state.flags & StateFlags.b) != 0)
					return (state.flags & StateFlags.W) != 0 ? 8U : 4;
				return 64;

			case TupleType.Half_128:
				return (state.flags & StateFlags.b) != 0 ? 4U : 8;

			case TupleType.Half_256:
				return (state.flags & StateFlags.b) != 0 ? 4U : 16;

			case TupleType.Half_512:
				return (state.flags & StateFlags.b) != 0 ? 4U : 32;

			case TupleType.Full_Mem_128:
				return 16;

			case TupleType.Full_Mem_256:
				return 32;

			case TupleType.Full_Mem_512:
				return 64;

			case TupleType.Tuple1_Scalar:
				return (state.flags & StateFlags.W) != 0 ? 8U : 4;

			case TupleType.Tuple1_Scalar_1:
				return 1;

			case TupleType.Tuple1_Scalar_2:
				return 2;

			case TupleType.Tuple1_Scalar_4:
				return 4;

			case TupleType.Tuple1_Scalar_8:
				return 8;

			case TupleType.Tuple1_Fixed:
				return (state.flags & StateFlags.W) != 0 ? 8U : 4;

			case TupleType.Tuple1_Fixed_4:
				return 4;

			case TupleType.Tuple1_Fixed_8:
				return 8;

			case TupleType.Tuple2:
				return (state.flags & StateFlags.W) != 0 ? 16U : 8;

			case TupleType.Tuple4:
				return (state.flags & StateFlags.W) != 0 ? 32U : 16;

			case TupleType.Tuple8:
				Debug.Assert((state.flags & StateFlags.W) == 0);
				return 32;

			case TupleType.Tuple1_4X:
				return 16;

			case TupleType.Half_Mem_128:
				return 8;

			case TupleType.Half_Mem_256:
				return 16;

			case TupleType.Half_Mem_512:
				return 32;

			case TupleType.Quarter_Mem_128:
				return 4;

			case TupleType.Quarter_Mem_256:
				return 8;

			case TupleType.Quarter_Mem_512:
				return 16;

			case TupleType.Eighth_Mem_128:
				return 2;

			case TupleType.Eighth_Mem_256:
				return 4;

			case TupleType.Eighth_Mem_512:
				return 8;

			case TupleType.Mem128:
				return 16;

			case TupleType.MOVDDUP_128:
				return 8;

			case TupleType.MOVDDUP_256:
				return 32;

			case TupleType.MOVDDUP_512:
				return 64;

			default:
				Debug.Fail($"Unreachable code");
				return 0;
			}
		}

		/// <summary>
		/// Gets the offsets of the constants (memory displacement and immediate) in the decoded instruction.
		/// The caller can check if there are any relocations at those addresses.
		/// </summary>
		/// <param name="instruction">The latest instruction that was decoded by this decoder</param>
		/// <returns></returns>
		public ConstantOffsets GetConstantOffsets(in Instruction instruction) {
			ConstantOffsets constantOffsets = default;

			int displSize = instruction.MemoryDisplSize;
			if (displSize != 0) {
				constantOffsets.DisplacementOffset = (byte)displIndex;
				if (displSize == 8 && (state.flags & StateFlags.Addr64) == 0)
					constantOffsets.DisplacementSize = 4;
				else
					constantOffsets.DisplacementSize = (byte)displSize;
			}

			if ((state.flags & StateFlags.NoImm) == 0) {
				int extraImmSub = 0;
				for (int i = instruction.OpCount - 1; i >= 0; i--) {
					switch (instruction.GetOpKind(i)) {
					case OpKind.Immediate8:
					case OpKind.Immediate8to16:
					case OpKind.Immediate8to32:
					case OpKind.Immediate8to64:
						constantOffsets.ImmediateOffset = (byte)(instruction.ByteLength - extraImmSub - 1);
						constantOffsets.ImmediateSize = 1;
						goto after_imm_loop;

					case OpKind.Immediate16:
						constantOffsets.ImmediateOffset = (byte)(instruction.ByteLength - extraImmSub - 2);
						constantOffsets.ImmediateSize = 2;
						goto after_imm_loop;

					case OpKind.Immediate32:
					case OpKind.Immediate32to64:
						constantOffsets.ImmediateOffset = (byte)(instruction.ByteLength - extraImmSub - 4);
						constantOffsets.ImmediateSize = 4;
						goto after_imm_loop;

					case OpKind.Immediate64:
						constantOffsets.ImmediateOffset = (byte)(instruction.ByteLength - extraImmSub - 8);
						constantOffsets.ImmediateSize = 8;
						goto after_imm_loop;

					case OpKind.Immediate8_2nd:
						constantOffsets.ImmediateOffset2 = (byte)(instruction.ByteLength - 1);
						constantOffsets.ImmediateSize2 = 1;
						extraImmSub = 1;
						break;

					case OpKind.NearBranch16:
						if ((state.flags & StateFlags.BranchImm8) != 0) {
							constantOffsets.ImmediateOffset = (byte)(instruction.ByteLength - 1);
							constantOffsets.ImmediateSize = 1;
						}
						else if ((state.flags & StateFlags.Xbegin) == 0) {
							constantOffsets.ImmediateOffset = (byte)(instruction.ByteLength - 2);
							constantOffsets.ImmediateSize = 2;
						}
						else {
							Debug.Assert((state.flags & StateFlags.Xbegin) != 0);
							if (state.operandSize != OpSize.Size16) {
								constantOffsets.ImmediateOffset = (byte)(instruction.ByteLength - 4);
								constantOffsets.ImmediateSize = 4;
							}
							else {
								constantOffsets.ImmediateOffset = (byte)(instruction.ByteLength - 2);
								constantOffsets.ImmediateSize = 2;
							}
						}
						break;

					case OpKind.NearBranch32:
					case OpKind.NearBranch64:
						if ((state.flags & StateFlags.BranchImm8) != 0) {
							constantOffsets.ImmediateOffset = (byte)(instruction.ByteLength - 1);
							constantOffsets.ImmediateSize = 1;
						}
						else if ((state.flags & StateFlags.Xbegin) == 0) {
							constantOffsets.ImmediateOffset = (byte)(instruction.ByteLength - 4);
							constantOffsets.ImmediateSize = 4;
						}
						else {
							Debug.Assert((state.flags & StateFlags.Xbegin) != 0);
							if (state.operandSize != OpSize.Size16) {
								constantOffsets.ImmediateOffset = (byte)(instruction.ByteLength - 4);
								constantOffsets.ImmediateSize = 4;
							}
							else {
								constantOffsets.ImmediateOffset = (byte)(instruction.ByteLength - 2);
								constantOffsets.ImmediateSize = 2;
							}
						}
						break;

					case OpKind.FarBranch16:
						constantOffsets.ImmediateOffset = (byte)(instruction.ByteLength - (2 + 2));
						constantOffsets.ImmediateSize = 2;
						constantOffsets.ImmediateOffset2 = (byte)(instruction.ByteLength - 2);
						constantOffsets.ImmediateSize2 = 2;
						break;

					case OpKind.FarBranch32:
						constantOffsets.ImmediateOffset = (byte)(instruction.ByteLength - (4 + 2));
						constantOffsets.ImmediateSize = 4;
						constantOffsets.ImmediateOffset2 = (byte)(instruction.ByteLength - 2);
						constantOffsets.ImmediateSize2 = 2;
						break;
					}
				}
			}
after_imm_loop:

			return constantOffsets;
		}

#if !NO_ENCODER
		/// <summary>
		/// Creates an encoder
		/// </summary>
		/// <param name="writer">Destination</param>
		/// <returns></returns>
		[Obsolete("Call Encoder.Create(decoder.Bitness, writer)", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public Encoder CreateEncoder(CodeWriter writer) => Encoder.Create(Bitness, writer);
#endif
	}
}
#endif
