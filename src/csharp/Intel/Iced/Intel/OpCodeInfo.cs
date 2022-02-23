// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if ENCODER && OPCODE_INFO
using System;
using System.Diagnostics;
using System.Text;
using Iced.Intel.EncoderInternal;

namespace Iced.Intel {
	/// <summary>
	/// Opcode info
	/// </summary>
	public sealed class OpCodeInfo {
#pragma warning disable CS0649
		readonly string toOpCodeStringValue;
		readonly string toInstructionStringValue;
		readonly EncFlags2 encFlags2;
		readonly EncFlags3 encFlags3;
		readonly OpCodeInfoFlags1 opcFlags1;
		readonly OpCodeInfoFlags2 opcFlags2;
		readonly ushort code;
		readonly byte encoding;
		readonly byte operandSize;
		readonly byte addressSize;
		readonly byte l;
		readonly byte tupleType;
		readonly byte table;
		readonly byte mandatoryPrefix;
		readonly sbyte groupIndex;
		readonly sbyte rmGroupIndex;
		readonly byte op0Kind;
		readonly byte op1Kind;
		readonly byte op2Kind;
		readonly byte op3Kind;
		readonly byte op4Kind;
		readonly Flags flags;
#pragma warning restore CS0649

		[Flags]
		enum Flags : uint {
			None					= 0,
			IgnoresRoundingControl	= 0x00000001,
			AmdLockRegBit			= 0x00000002,
			LIG						= 0x00000004,
			W						= 0x00000008,
			WIG						= 0x00000010,
			WIG32					= 0x00000020,
			CPL0					= 0x00000040,
			CPL1					= 0x00000080,
			CPL2					= 0x00000100,
			CPL3					= 0x00000200,
		}

		internal OpCodeInfo(Code code, EncFlags1 encFlags1, EncFlags2 encFlags2, EncFlags3 encFlags3, OpCodeInfoFlags1 opcFlags1, OpCodeInfoFlags2 opcFlags2, StringBuilder sb) {
			Debug.Assert((uint)code < (uint)IcedConstants.CodeEnumCount);
			Debug.Assert((uint)code <= ushort.MaxValue);
			this.code = (ushort)code;
			this.encFlags2 = encFlags2;
			this.encFlags3 = encFlags3;
			this.opcFlags1 = opcFlags1;
			this.opcFlags2 = opcFlags2;

			if ((encFlags1 & EncFlags1.IgnoresRoundingControl) != 0)
				flags |= Flags.IgnoresRoundingControl;
			if ((encFlags1 & EncFlags1.AmdLockRegBit) != 0)
				flags |= Flags.AmdLockRegBit;
			flags |= (opcFlags1 & (OpCodeInfoFlags1.Cpl0Only | OpCodeInfoFlags1.Cpl3Only)) switch {
				OpCodeInfoFlags1.Cpl0Only => Flags.CPL0,
				OpCodeInfoFlags1.Cpl3Only => Flags.CPL3,
				_ => Flags.CPL0 | Flags.CPL1 | Flags.CPL2 | Flags.CPL3,
			};

			encoding = (byte)(((uint)encFlags3 >> (int)EncFlags3.EncodingShift) & (uint)EncFlags3.EncodingMask);
			mandatoryPrefix = (MandatoryPrefixByte)(((uint)encFlags2 >> (int)EncFlags2.MandatoryPrefixShift) & (uint)EncFlags2.MandatoryPrefixMask) switch {
				MandatoryPrefixByte.None => (byte)((encFlags2 & EncFlags2.HasMandatoryPrefix) != 0 ? MandatoryPrefix.PNP : MandatoryPrefix.None),
				MandatoryPrefixByte.P66 => (byte)MandatoryPrefix.P66,
				MandatoryPrefixByte.PF3 => (byte)MandatoryPrefix.PF3,
				MandatoryPrefixByte.PF2 => (byte)MandatoryPrefix.PF2,
				_ => throw new InvalidOperationException(),
			};
			operandSize = (CodeSize)(((uint)encFlags3 >> (int)EncFlags3.OperandSizeShift) & (uint)EncFlags3.OperandSizeMask) switch {
				CodeSize.Unknown => 0,
				CodeSize.Code16 => 16,
				CodeSize.Code32 => 32,
				CodeSize.Code64 => 64,
				_ => throw new InvalidOperationException(),
			};
			addressSize = (CodeSize)(((uint)encFlags3 >> (int)EncFlags3.AddressSizeShift) & (uint)EncFlags3.AddressSizeMask) switch {
				CodeSize.Unknown => 0,
				CodeSize.Code16 => 16,
				CodeSize.Code32 => 32,
				CodeSize.Code64 => 64,
				_ => throw new InvalidOperationException(),
			};
			groupIndex = (sbyte)((encFlags2 & EncFlags2.HasGroupIndex) == 0 ? -1 : (int)(((uint)encFlags2 >> (int)EncFlags2.GroupIndexShift) & 7));
			rmGroupIndex = (sbyte)((encFlags3 & EncFlags3.HasRmGroupIndex) == 0 ? -1 : (int)(((uint)encFlags2 >> (int)EncFlags2.GroupIndexShift) & 7));
			tupleType = (byte)(((uint)encFlags3 >> (int)EncFlags3.TupleTypeShift) & (uint)EncFlags3.TupleTypeMask);

			LKind lkind;
#if !NO_VEX || !NO_EVEX || !NO_XOP
			switch ((LBit)(((uint)encFlags2 >> (int)EncFlags2.LBitShift) & (int)EncFlags2.LBitMask)) {
			case LBit.LZ:
				lkind = LKind.LZ;
				l = 0;
				break;
			case LBit.L0:
				lkind = LKind.L0;
				l = 0;
				break;
			case LBit.L1:
				lkind = LKind.L0;
				l = 1;
				break;
			case LBit.L128:
				lkind = LKind.L128;
				l = 0;
				break;
			case LBit.L256:
				lkind = LKind.L128;
				l = 1;
				break;
			case LBit.L512:
				lkind = LKind.L128;
				l = 2;
				break;
			case LBit.LIG:
				lkind = LKind.None;
				l = 0;
				flags |= Flags.LIG;
				break;
			default:
				throw new InvalidOperationException();
			}
#else
			lkind = LKind.LZ;
#endif

#if !NO_VEX || !NO_EVEX || !NO_XOP
			switch ((WBit)(((uint)encFlags2 >> (int)EncFlags2.WBitShift) & (uint)EncFlags2.WBitMask)) {
			case WBit.W0:
				break;
			case WBit.W1:
				flags |= Flags.W;
				break;
			case WBit.WIG:
				flags |= Flags.WIG;
				break;
			case WBit.WIG32:
				flags |= Flags.WIG32;
				break;
			default:
				throw new InvalidOperationException();
			}
#endif

			string? toOpCodeStringValue = null;
			string? toInstructionStringValue = null;
#if HAS_SPAN
			ReadOnlySpan<byte> opKinds;
#else
			byte[] opKinds;
#endif
			switch ((EncodingKind)encoding) {
			case EncodingKind.Legacy:
				opKinds = OpCodeOperandKinds.LegacyOpKinds;
				op0Kind = opKinds[(int)(((uint)encFlags1 >> (int)EncFlags1.Legacy_Op0Shift) & (uint)EncFlags1.Legacy_OpMask)];
				op1Kind = opKinds[(int)(((uint)encFlags1 >> (int)EncFlags1.Legacy_Op1Shift) & (uint)EncFlags1.Legacy_OpMask)];
				op2Kind = opKinds[(int)(((uint)encFlags1 >> (int)EncFlags1.Legacy_Op2Shift) & (uint)EncFlags1.Legacy_OpMask)];
				op3Kind = opKinds[(int)(((uint)encFlags1 >> (int)EncFlags1.Legacy_Op3Shift) & (uint)EncFlags1.Legacy_OpMask)];

				table = (LegacyOpCodeTable)(((uint)encFlags2 >> (int)EncFlags2.TableShift) & (uint)EncFlags2.TableMask) switch {
					LegacyOpCodeTable.MAP0 => (byte)OpCodeTableKind.Normal,
					LegacyOpCodeTable.MAP0F => (byte)OpCodeTableKind.T0F,
					LegacyOpCodeTable.MAP0F38 => (byte)OpCodeTableKind.T0F38,
					LegacyOpCodeTable.MAP0F3A => (byte)OpCodeTableKind.T0F3A,
					_ => throw new InvalidOperationException(),
				};
				break;

			case EncodingKind.VEX:
#if !NO_VEX
				opKinds = OpCodeOperandKinds.VexOpKinds;
				op0Kind = opKinds[(int)(((uint)encFlags1 >> (int)EncFlags1.VEX_Op0Shift) & (uint)EncFlags1.VEX_OpMask)];
				op1Kind = opKinds[(int)(((uint)encFlags1 >> (int)EncFlags1.VEX_Op1Shift) & (uint)EncFlags1.VEX_OpMask)];
				op2Kind = opKinds[(int)(((uint)encFlags1 >> (int)EncFlags1.VEX_Op2Shift) & (uint)EncFlags1.VEX_OpMask)];
				op3Kind = opKinds[(int)(((uint)encFlags1 >> (int)EncFlags1.VEX_Op3Shift) & (uint)EncFlags1.VEX_OpMask)];
				op4Kind = opKinds[(int)(((uint)encFlags1 >> (int)EncFlags1.VEX_Op4Shift) & (uint)EncFlags1.VEX_OpMask)];

				table = (VexOpCodeTable)(((uint)encFlags2 >> (int)EncFlags2.TableShift) & (uint)EncFlags2.TableMask) switch {
					VexOpCodeTable.MAP0 => (byte)OpCodeTableKind.Normal,
					VexOpCodeTable.MAP0F => (byte)OpCodeTableKind.T0F,
					VexOpCodeTable.MAP0F38 => (byte)OpCodeTableKind.T0F38,
					VexOpCodeTable.MAP0F3A => (byte)OpCodeTableKind.T0F3A,
					_ => throw new InvalidOperationException(),
				};
				break;
#else
				op4Kind = (byte)OpCodeOperandKind.None;
				toOpCodeStringValue = string.Empty;
				toInstructionStringValue = string.Empty;
				break;
#endif

			case EncodingKind.EVEX:
#if !NO_EVEX
				opKinds = OpCodeOperandKinds.EvexOpKinds;
				op0Kind = opKinds[(int)(((uint)encFlags1 >> (int)EncFlags1.EVEX_Op0Shift) & (uint)EncFlags1.EVEX_OpMask)];
				op1Kind = opKinds[(int)(((uint)encFlags1 >> (int)EncFlags1.EVEX_Op1Shift) & (uint)EncFlags1.EVEX_OpMask)];
				op2Kind = opKinds[(int)(((uint)encFlags1 >> (int)EncFlags1.EVEX_Op2Shift) & (uint)EncFlags1.EVEX_OpMask)];
				op3Kind = opKinds[(int)(((uint)encFlags1 >> (int)EncFlags1.EVEX_Op3Shift) & (uint)EncFlags1.EVEX_OpMask)];

				table = (EvexOpCodeTable)(((uint)encFlags2 >> (int)EncFlags2.TableShift) & (uint)EncFlags2.TableMask) switch {
					EvexOpCodeTable.MAP0F => (byte)OpCodeTableKind.T0F,
					EvexOpCodeTable.MAP0F38 => (byte)OpCodeTableKind.T0F38,
					EvexOpCodeTable.MAP0F3A => (byte)OpCodeTableKind.T0F3A,
					EvexOpCodeTable.MAP5 => (byte)OpCodeTableKind.MAP5,
					EvexOpCodeTable.MAP6 => (byte)OpCodeTableKind.MAP6,
					_ => throw new InvalidOperationException(),
				};
				break;
#else
				toOpCodeStringValue = string.Empty;
				toInstructionStringValue = string.Empty;
				break;
#endif

			case EncodingKind.XOP:
#if !NO_XOP
				opKinds = OpCodeOperandKinds.XopOpKinds;
				op0Kind = opKinds[(int)(((uint)encFlags1 >> (int)EncFlags1.XOP_Op0Shift) & (uint)EncFlags1.XOP_OpMask)];
				op1Kind = opKinds[(int)(((uint)encFlags1 >> (int)EncFlags1.XOP_Op1Shift) & (uint)EncFlags1.XOP_OpMask)];
				op2Kind = opKinds[(int)(((uint)encFlags1 >> (int)EncFlags1.XOP_Op2Shift) & (uint)EncFlags1.XOP_OpMask)];
				op3Kind = opKinds[(int)(((uint)encFlags1 >> (int)EncFlags1.XOP_Op3Shift) & (uint)EncFlags1.XOP_OpMask)];

				table = (XopOpCodeTable)(((uint)encFlags2 >> (int)EncFlags2.TableShift) & (uint)EncFlags2.TableMask) switch {
					XopOpCodeTable.MAP8 => (byte)OpCodeTableKind.MAP8,
					XopOpCodeTable.MAP9 => (byte)OpCodeTableKind.MAP9,
					XopOpCodeTable.MAP10 => (byte)OpCodeTableKind.MAP10,
					_ => throw new InvalidOperationException(),
				};
				break;
#else
				toOpCodeStringValue = string.Empty;
				toInstructionStringValue = string.Empty;
				break;
#endif

			case EncodingKind.D3NOW:
#if !NO_D3NOW
				op0Kind = (byte)OpCodeOperandKind.mm_reg;
				op1Kind = (byte)OpCodeOperandKind.mm_or_mem;
				table = (byte)OpCodeTableKind.T0F;
				break;
#else
				toOpCodeStringValue = string.Empty;
				toInstructionStringValue = string.Empty;
				break;
#endif

			case EncodingKind.MVEX:
#if MVEX
				opKinds = OpCodeOperandKinds.MvexOpKinds;
				op0Kind = opKinds[(int)(((uint)encFlags1 >> (int)EncFlags1.MVEX_Op0Shift) & (uint)EncFlags1.MVEX_OpMask)];
				op1Kind = opKinds[(int)(((uint)encFlags1 >> (int)EncFlags1.MVEX_Op1Shift) & (uint)EncFlags1.MVEX_OpMask)];
				op2Kind = opKinds[(int)(((uint)encFlags1 >> (int)EncFlags1.MVEX_Op2Shift) & (uint)EncFlags1.MVEX_OpMask)];
				op3Kind = opKinds[(int)(((uint)encFlags1 >> (int)EncFlags1.MVEX_Op3Shift) & (uint)EncFlags1.MVEX_OpMask)];

				table = (MvexOpCodeTable)(((uint)encFlags2 >> (int)EncFlags2.TableShift) & (uint)EncFlags2.TableMask) switch {
					MvexOpCodeTable.MAP0F => (byte)OpCodeTableKind.T0F,
					MvexOpCodeTable.MAP0F38 => (byte)OpCodeTableKind.T0F38,
					MvexOpCodeTable.MAP0F3A => (byte)OpCodeTableKind.T0F3A,
					_ => throw new InvalidOperationException(),
				};
				break;
#else
				toOpCodeStringValue = string.Empty;
				toInstructionStringValue = string.Empty;
				break;
#endif

			default:
				throw new InvalidOperationException();
			}

			this.toOpCodeStringValue = toOpCodeStringValue ?? new OpCodeFormatter(this, sb, lkind, (opcFlags1 & OpCodeInfoFlags1.ModRegRmString) != 0).Format();
			var fmtOption = (InstrStrFmtOption)(((uint)opcFlags2 >> (int)OpCodeInfoFlags2.InstrStrFmtOptionShift) & (uint)OpCodeInfoFlags2.InstrStrFmtOptionMask);
			this.toInstructionStringValue = toInstructionStringValue ?? new InstructionFormatter(this, fmtOption, sb).Format();
		}

		/// <summary>
		/// Gets the code
		/// </summary>
		public Code Code => (Code)code;

		/// <summary>
		/// Gets the mnemonic
		/// </summary>
		public Mnemonic Mnemonic => Code.Mnemonic();

		/// <summary>
		/// Gets the encoding
		/// </summary>
		public EncodingKind Encoding => (EncodingKind)encoding;

		/// <summary>
		/// <see langword="true"/> if it's an instruction, <see langword="false"/> if it's eg. <see cref="Code.INVALID"/>, <c>db</c>, <c>dw</c>, <c>dd</c>, <c>dq</c>, <c>zero_bytes</c>
		/// </summary>
		public bool IsInstruction => !(code <= (ushort)Code.DeclareQword || code == (ushort)Code.Zero_bytes);

		/// <summary>
		/// <see langword="true"/> if it's an instruction available in 16-bit mode
		/// </summary>
		public bool Mode16 => (encFlags3 & EncFlags3.Bit16or32) != 0;

		/// <summary>
		/// <see langword="true"/> if it's an instruction available in 32-bit mode
		/// </summary>
		public bool Mode32 => (encFlags3 & EncFlags3.Bit16or32) != 0;

		/// <summary>
		/// <see langword="true"/> if it's an instruction available in 64-bit mode
		/// </summary>
		public bool Mode64 => (encFlags3 & EncFlags3.Bit64) != 0;

		/// <summary>
		/// <see langword="true"/> if an <c>FWAIT</c> (<c>9B</c>) instruction is added before the instruction
		/// </summary>
		public bool Fwait => (encFlags3 & EncFlags3.Fwait) != 0;

		/// <summary>
		/// (Legacy encoding) Gets the required operand size (16,32,64) or 0
		/// </summary>
		public int OperandSize => operandSize;

		/// <summary>
		/// (Legacy encoding) Gets the required address size (16,32,64) or 0
		/// </summary>
		public int AddressSize => addressSize;

		/// <summary>
		/// (VEX/XOP/EVEX) <c>L</c> / <c>L'L</c> value or default value if <see cref="IsLIG"/> is <see langword="true"/>
		/// </summary>
		public uint L => l;

		/// <summary>
		/// (VEX/XOP/EVEX/MVEX) <c>W</c> value or default value if <see cref="IsWIG"/> or <see cref="IsWIG32"/> is <see langword="true"/>
		/// </summary>
		public uint W => (flags & Flags.W) != 0 ? 1U : 0;

		/// <summary>
		/// (VEX/XOP/EVEX) <see langword="true"/> if the <c>L</c> / <c>L'L</c> fields are ignored.
		/// <br/>
		/// EVEX: if reg-only ops and <c>{er}</c> (<c>EVEX.b</c> is set), <c>L'L</c> is the rounding control and not ignored.
		/// </summary>
		public bool IsLIG => (flags & Flags.LIG) != 0;

		/// <summary>
		/// (VEX/XOP/EVEX/MVEX) <see langword="true"/> if the <c>W</c> field is ignored in 16/32/64-bit modes
		/// </summary>
		public bool IsWIG => (flags & Flags.WIG) != 0;

		/// <summary>
		/// (VEX/XOP/EVEX/MVEX) <see langword="true"/> if the <c>W</c> field is ignored in 16/32-bit modes (but not 64-bit mode)
		/// </summary>
		public bool IsWIG32 => (flags & Flags.WIG32) != 0;

		/// <summary>
		/// (EVEX/MVEX) Gets the tuple type
		/// </summary>
		public TupleType TupleType => (TupleType)tupleType;

#if MVEX
		/// <summary>
		/// (MVEX) Gets the <c>EH</c> bit that's required to encode this instruction
		/// </summary>
		public MvexEHBit MvexEHBit => Encoding == EncodingKind.MVEX ? new MvexInfo(Code).EHBit : MvexEHBit.None;

		/// <summary>
		/// (MVEX) <see langword="true"/> if the instruction supports eviction hint (if it has a memory operand)
		/// </summary>
		public bool MvexCanUseEvictionHint => Encoding == EncodingKind.MVEX && new MvexInfo(Code).CanUseEvictionHint;

		/// <summary>
		/// (MVEX) <see langword="true"/> if the instruction's rounding control bits are stored in <c>imm8[1:0]</c>
		/// </summary>
		public bool MvexCanUseImmRoundingControl => Encoding == EncodingKind.MVEX && new MvexInfo(Code).CanUseImmRoundingControl;

		/// <summary>
		/// (MVEX) <see langword="true"/> if the instruction ignores op mask registers (eg. <c>{k1}</c>)
		/// </summary>
		public bool MvexIgnoresOpMaskRegister => Encoding == EncodingKind.MVEX && new MvexInfo(Code).IgnoresOpMaskRegister;

		/// <summary>
		/// (MVEX) <see langword="true"/> if the instruction must have <c>MVEX.SSS=000</c> if <c>MVEX.EH=1</c>
		/// </summary>
		public bool MvexNoSaeRc => Encoding == EncodingKind.MVEX && new MvexInfo(Code).NoSaeRc;

		/// <summary>
		/// (MVEX) Gets the tuple type / conv lut kind
		/// </summary>
		public MvexTupleTypeLutKind MvexTupleTypeLutKind => Encoding == EncodingKind.MVEX ? new MvexInfo(Code).TupleTypeLutKind : MvexTupleTypeLutKind.Int32;

		/// <summary>
		/// (MVEX) Gets the conversion function, eg. <c>Sf32</c>
		/// </summary>
		public MvexConvFn MvexConversionFunc => Encoding == EncodingKind.MVEX ? new MvexInfo(Code).ConvFn : MvexConvFn.None;

		/// <summary>
		/// (MVEX) Gets flags indicating which conversion functions are valid (bit 0 == func 0)
		/// </summary>
		public byte MvexValidConversionFuncsMask => (byte)(Encoding == EncodingKind.MVEX ? ~new MvexInfo(Code).InvalidConvFns : 0);

		/// <summary>
		/// (MVEX) Gets flags indicating which swizzle functions are valid (bit 0 == func 0)
		/// </summary>
		public byte MvexValidSwizzleFuncsMask => (byte)(Encoding == EncodingKind.MVEX ? ~new MvexInfo(Code).InvalidSwizzleFns : 0);
#endif

		/// <summary>
		/// If it has a memory operand, gets the <see cref="MemorySize"/> (non-broadcast memory type)
		/// </summary>
		public MemorySize MemorySize => (MemorySize)InstructionMemorySizes.SizesNormal[(int)code];

		/// <summary>
		/// If it has a memory operand, gets the <see cref="MemorySize"/> (broadcast memory type)
		/// </summary>
		public MemorySize BroadcastMemorySize => (MemorySize)InstructionMemorySizes.SizesBcst[(int)code];

		/// <summary>
		/// (EVEX) <see langword="true"/> if the instruction supports broadcasting (<c>EVEX.b</c> bit) (if it has a memory operand)
		/// </summary>
		public bool CanBroadcast => (encFlags3 & EncFlags3.Broadcast) != 0;

		/// <summary>
		/// (EVEX/MVEX) <see langword="true"/> if the instruction supports rounding control
		/// </summary>
		public bool CanUseRoundingControl => (encFlags3 & EncFlags3.RoundingControl) != 0;

		/// <summary>
		/// (EVEX/MVEX) <see langword="true"/> if the instruction supports suppress all exceptions
		/// </summary>
		public bool CanSuppressAllExceptions => (encFlags3 & EncFlags3.SuppressAllExceptions) != 0;

		/// <summary>
		/// (EVEX/MVEX) <see langword="true"/> if an opmask register can be used
		/// </summary>
		public bool CanUseOpMaskRegister => (encFlags3 & EncFlags3.OpMaskRegister) != 0;

		/// <summary>
		/// (EVEX/MVEX) <see langword="true"/> if a non-zero opmask register must be used
		/// </summary>
		public bool RequireOpMaskRegister => (encFlags3 & EncFlags3.RequireOpMaskRegister) != 0;

		/// <summary>
		/// (EVEX) <see langword="true"/> if the instruction supports zeroing masking (if one of the opmask registers <c>K1</c>-<c>K7</c> is used and destination operand is not a memory operand)
		/// </summary>
		public bool CanUseZeroingMasking => (encFlags3 & EncFlags3.ZeroingMasking) != 0;

		/// <summary>
		/// <see langword="true"/> if the <c>LOCK</c> (<c>F0</c>) prefix can be used
		/// </summary>
		public bool CanUseLockPrefix => (encFlags3 & EncFlags3.Lock) != 0;

		/// <summary>
		/// <see langword="true"/> if the <c>XACQUIRE</c> (<c>F2</c>) prefix can be used
		/// </summary>
		public bool CanUseXacquirePrefix => (encFlags3 & EncFlags3.Xacquire) != 0;

		/// <summary>
		/// <see langword="true"/> if the <c>XRELEASE</c> (<c>F3</c>) prefix can be used
		/// </summary>
		public bool CanUseXreleasePrefix => (encFlags3 & EncFlags3.Xrelease) != 0;

		/// <summary>
		/// <see langword="true"/> if the <c>REP</c> / <c>REPE</c> (<c>F3</c>) prefixes can be used
		/// </summary>
		public bool CanUseRepPrefix => (encFlags3 & EncFlags3.Rep) != 0;

		/// <summary>
		/// <see langword="true"/> if the <c>REPNE</c> (<c>F2</c>) prefix can be used
		/// </summary>
		public bool CanUseRepnePrefix => (encFlags3 & EncFlags3.Repne) != 0;

		/// <summary>
		/// <see langword="true"/> if the <c>BND</c> (<c>F2</c>) prefix can be used
		/// </summary>
		public bool CanUseBndPrefix => (encFlags3 & EncFlags3.Bnd) != 0;

		/// <summary>
		/// <see langword="true"/> if the <c>HINT-TAKEN</c> (<c>3E</c>) and <c>HINT-NOT-TAKEN</c> (<c>2E</c>) prefixes can be used
		/// </summary>
		public bool CanUseHintTakenPrefix => (encFlags3 & EncFlags3.HintTaken) != 0;

		/// <summary>
		/// <see langword="true"/> if the <c>NOTRACK</c> (<c>3E</c>) prefix can be used
		/// </summary>
		public bool CanUseNotrackPrefix => (encFlags3 & EncFlags3.Notrack) != 0;

		/// <summary>
		/// <see langword="true"/> if rounding control is ignored (#UD is not generated)
		/// </summary>
		public bool IgnoresRoundingControl => (flags & Flags.IgnoresRoundingControl) != 0;

		/// <summary>
		/// (AMD) <see langword="true"/> if the <c>LOCK</c> prefix can be used as an extra register bit (bit 3) to access registers 8-15 without a <c>REX</c> prefix (eg. in 32-bit mode)
		/// </summary>
		public bool AmdLockRegBit => (flags & Flags.AmdLockRegBit) != 0;

		/// <summary>
		/// <see langword="true"/> if the default operand size is 64 in 64-bit mode. A <c>66</c> prefix can switch to 16-bit operand size.
		/// </summary>
		public bool DefaultOpSize64 => (encFlags3 & EncFlags3.DefaultOpSize64) != 0;

		/// <summary>
		/// <see langword="true"/> if the operand size is always 64 in 64-bit mode. A <c>66</c> prefix is ignored.
		/// </summary>
		public bool ForceOpSize64 => (opcFlags1 & OpCodeInfoFlags1.ForceOpSize64) != 0;

		/// <summary>
		/// <see langword="true"/> if the Intel decoder forces 64-bit operand size. A <c>66</c> prefix is ignored.
		/// </summary>
		public bool IntelForceOpSize64 => (encFlags3 & EncFlags3.IntelForceOpSize64) != 0;

		/// <summary>
		/// <see langword="true"/> if it can only be executed when CPL=0
		/// </summary>
		public bool MustBeCpl0 => (flags & (Flags.CPL0 | Flags.CPL1 | Flags.CPL2 | Flags.CPL3)) == Flags.CPL0;

		/// <summary>
		/// <see langword="true"/> if it can be executed when CPL=0
		/// </summary>
		public bool Cpl0 => (flags & Flags.CPL0) != 0;

		/// <summary>
		/// <see langword="true"/> if it can be executed when CPL=1
		/// </summary>
		public bool Cpl1 => (flags & Flags.CPL1) != 0;

		/// <summary>
		/// <see langword="true"/> if it can be executed when CPL=2
		/// </summary>
		public bool Cpl2 => (flags & Flags.CPL2) != 0;

		/// <summary>
		/// <see langword="true"/> if it can be executed when CPL=3
		/// </summary>
		public bool Cpl3 => (flags & Flags.CPL3) != 0;

		/// <summary>
		/// <see langword="true"/> if the instruction accesses the I/O address space (eg. <c>IN</c>, <c>OUT</c>, <c>INS</c>, <c>OUTS</c>)
		/// </summary>
		public bool IsInputOutput => (opcFlags1 & OpCodeInfoFlags1.InputOutput) != 0;

		/// <summary>
		/// <see langword="true"/> if it's one of the many nop instructions (does not include FPU nop instructions, eg. <c>FNOP</c>)
		/// </summary>
		public bool IsNop => (opcFlags1 & OpCodeInfoFlags1.Nop) != 0;

		/// <summary>
		/// <see langword="true"/> if it's one of the many reserved nop instructions (eg. <c>0F0D</c>, <c>0F18-0F1F</c>)
		/// </summary>
		public bool IsReservedNop => (opcFlags1 & OpCodeInfoFlags1.ReservedNop) != 0;

		/// <summary>
		/// <see langword="true"/> if it's a serializing instruction (Intel CPUs)
		/// </summary>
		public bool IsSerializingIntel => (opcFlags1 & OpCodeInfoFlags1.SerializingIntel) != 0;

		/// <summary>
		/// <see langword="true"/> if it's a serializing instruction (AMD CPUs)
		/// </summary>
		public bool IsSerializingAmd => (opcFlags1 & OpCodeInfoFlags1.SerializingAmd) != 0;

		/// <summary>
		/// <see langword="true"/> if the instruction requires either CPL=0 or CPL&lt;=3 depending on some CPU option (eg. <c>CR4.TSD</c>, <c>CR4.PCE</c>, <c>CR4.UMIP</c>)
		/// </summary>
		public bool MayRequireCpl0 => (opcFlags1 & OpCodeInfoFlags1.MayRequireCpl0) != 0;

		/// <summary>
		/// <see langword="true"/> if it's a tracked <c>JMP</c>/<c>CALL</c> indirect instruction (CET)
		/// </summary>
		public bool IsCetTracked => (opcFlags1 & OpCodeInfoFlags1.CetTracked) != 0;

		/// <summary>
		/// <see langword="true"/> if it's a non-temporal hint memory access (eg. <c>MOVNTDQ</c>)
		/// </summary>
		public bool IsNonTemporal => (opcFlags1 & OpCodeInfoFlags1.NonTemporal) != 0;

		/// <summary>
		/// <see langword="true"/> if it's a no-wait FPU instruction, eg. <c>FNINIT</c>
		/// </summary>
		public bool IsFpuNoWait => (opcFlags1 & OpCodeInfoFlags1.FpuNoWait) != 0;

		/// <summary>
		/// <see langword="true"/> if the mod bits are ignored and it's assumed <c>modrm[7:6] == 11b</c>
		/// </summary>
		public bool IgnoresModBits => (opcFlags1 & OpCodeInfoFlags1.IgnoresModBits) != 0;

		/// <summary>
		/// <see langword="true"/> if the <c>66</c> prefix is not allowed (it will #UD)
		/// </summary>
		public bool No66 => (opcFlags1 & OpCodeInfoFlags1.No66) != 0;

		/// <summary>
		/// <see langword="true"/> if the <c>F2</c>/<c>F3</c> prefixes aren't allowed
		/// </summary>
		public bool NFx => (opcFlags1 & OpCodeInfoFlags1.NFx) != 0;

		/// <summary>
		/// <see langword="true"/> if the index reg's reg-num (vsib op) (if any) and register ops' reg-nums must be unique,
		/// eg. <c>MNEMONIC XMM1,YMM1,[RAX+ZMM1*2]</c> is invalid. Registers = <c>XMM</c>/<c>YMM</c>/<c>ZMM</c>/<c>TMM</c>.
		/// </summary>
		public bool RequiresUniqueRegNums => (opcFlags1 & OpCodeInfoFlags1.RequiresUniqueRegNums) != 0;

		/// <summary>
		/// <see langword="true"/> if the destination register's reg-num must not be present in any other operand, eg. <c>MNEMONIC XMM1,YMM1,[RAX+ZMM1*2]</c>
		/// is invalid. Registers = <c>XMM</c>/<c>YMM</c>/<c>ZMM</c>/<c>TMM</c>.
		/// </summary>
		public bool RequiresUniqueDestRegNum => (opcFlags1 & OpCodeInfoFlags1.RequiresUniqueDestRegNum) != 0;

		/// <summary>
		/// <see langword="true"/> if it's a privileged instruction (all CPL=0 instructions (except <c>VMCALL</c>) and IOPL instructions <c>IN</c>, <c>INS</c>, <c>OUT</c>, <c>OUTS</c>, <c>CLI</c>, <c>STI</c>)
		/// </summary>
		public bool IsPrivileged => (opcFlags1 & OpCodeInfoFlags1.Privileged) != 0;

		/// <summary>
		/// <see langword="true"/> if it reads/writes too many registers
		/// </summary>
		public bool IsSaveRestore => (opcFlags1 & OpCodeInfoFlags1.SaveRestore) != 0;

		/// <summary>
		/// <see langword="true"/> if it's an instruction that implicitly uses the stack register, eg. <c>CALL</c>, <c>POP</c>, etc
		/// </summary>
		public bool IsStackInstruction => (opcFlags1 & OpCodeInfoFlags1.StackInstruction) != 0;

		/// <summary>
		/// <see langword="true"/> if the instruction doesn't read the segment register if it uses a memory operand
		/// </summary>
		public bool IgnoresSegment => (opcFlags1 & OpCodeInfoFlags1.IgnoresSegment) != 0;

		/// <summary>
		/// <see langword="true"/> if the opmask register is read and written (instead of just read). This also implies that it can't be <c>K0</c>.
		/// </summary>
		public bool IsOpMaskReadWrite => (opcFlags1 & OpCodeInfoFlags1.OpMaskReadWrite) != 0;

		/// <summary>
		/// <see langword="true"/> if it can be executed in real mode
		/// </summary>
		public bool RealMode => (opcFlags2 & OpCodeInfoFlags2.RealMode) != 0;

		/// <summary>
		/// <see langword="true"/> if it can be executed in protected mode
		/// </summary>
		public bool ProtectedMode => (opcFlags2 & OpCodeInfoFlags2.ProtectedMode) != 0;

		/// <summary>
		/// <see langword="true"/> if it can be executed in virtual 8086 mode
		/// </summary>
		public bool Virtual8086Mode => (opcFlags2 & OpCodeInfoFlags2.Virtual8086Mode) != 0;

		/// <summary>
		/// <see langword="true"/> if it can be executed in compatibility mode
		/// </summary>
		public bool CompatibilityMode => (opcFlags2 & OpCodeInfoFlags2.CompatibilityMode) != 0;

		/// <summary>
		/// <see langword="true"/> if it can be executed in 64-bit mode
		/// </summary>
		public bool LongMode => (encFlags3 & EncFlags3.Bit64) != 0;

		/// <summary>
		/// <see langword="true"/> if it can be used outside SMM
		/// </summary>
		public bool UseOutsideSmm => (opcFlags2 & OpCodeInfoFlags2.UseOutsideSmm) != 0;

		/// <summary>
		/// <see langword="true"/> if it can be used in SMM
		/// </summary>
		public bool UseInSmm => (opcFlags2 & OpCodeInfoFlags2.UseInSmm) != 0;

		/// <summary>
		/// <see langword="true"/> if it can be used outside an enclave (SGX)
		/// </summary>
		public bool UseOutsideEnclaveSgx => (opcFlags2 & OpCodeInfoFlags2.UseOutsideEnclaveSgx) != 0;

		/// <summary>
		/// <see langword="true"/> if it can be used inside an enclave (SGX1)
		/// </summary>
		public bool UseInEnclaveSgx1 => (opcFlags2 & OpCodeInfoFlags2.UseInEnclaveSgx1) != 0;

		/// <summary>
		/// <see langword="true"/> if it can be used inside an enclave (SGX2)
		/// </summary>
		public bool UseInEnclaveSgx2 => (opcFlags2 & OpCodeInfoFlags2.UseInEnclaveSgx2) != 0;

		/// <summary>
		/// <see langword="true"/> if it can be used outside VMX operation
		/// </summary>
		public bool UseOutsideVmxOp => (opcFlags2 & OpCodeInfoFlags2.UseOutsideVmxOp) != 0;

		/// <summary>
		/// <see langword="true"/> if it can be used in VMX root operation
		/// </summary>
		public bool UseInVmxRootOp => (opcFlags2 & OpCodeInfoFlags2.UseInVmxRootOp) != 0;

		/// <summary>
		/// <see langword="true"/> if it can be used in VMX non-root operation
		/// </summary>
		public bool UseInVmxNonRootOp => (opcFlags2 & OpCodeInfoFlags2.UseInVmxNonRootOp) != 0;

		/// <summary>
		/// <see langword="true"/> if it can be used outside SEAM
		/// </summary>
		public bool UseOutsideSeam => (opcFlags2 & OpCodeInfoFlags2.UseOutsideSeam) != 0;

		/// <summary>
		/// <see langword="true"/> if it can be used in SEAM
		/// </summary>
		public bool UseInSeam => (opcFlags2 & OpCodeInfoFlags2.UseInSeam) != 0;

		/// <summary>
		/// <see langword="true"/> if #UD is generated in TDX non-root operation
		/// </summary>
		public bool TdxNonRootGenUd => (opcFlags2 & OpCodeInfoFlags2.TdxNonRootGenUd) != 0;

		/// <summary>
		/// <see langword="true"/> if #VE is generated in TDX non-root operation
		/// </summary>
		public bool TdxNonRootGenVe => (opcFlags2 & OpCodeInfoFlags2.TdxNonRootGenVe) != 0;

		/// <summary>
		/// <see langword="true"/> if an exception (eg. #GP(0), #VE) may be generated in TDX non-root operation
		/// </summary>
		public bool TdxNonRootMayGenEx => (opcFlags2 & OpCodeInfoFlags2.TdxNonRootMayGenEx) != 0;

		/// <summary>
		/// (Intel VMX) <see langword="true"/> if it causes a VM exit in VMX non-root operation
		/// </summary>
		public bool IntelVmExit => (opcFlags2 & OpCodeInfoFlags2.IntelVmExit) != 0;

		/// <summary>
		/// (Intel VMX) <see langword="true"/> if it may cause a VM exit in VMX non-root operation
		/// </summary>
		public bool IntelMayVmExit => (opcFlags2 & OpCodeInfoFlags2.IntelMayVmExit) != 0;

		/// <summary>
		/// (Intel VMX) <see langword="true"/> if it causes an SMM VM exit in VMX root operation (if dual-monitor treatment is activated)
		/// </summary>
		public bool IntelSmmVmExit => (opcFlags2 & OpCodeInfoFlags2.IntelSmmVmExit) != 0;

		/// <summary>
		/// (AMD SVM) <see langword="true"/> if it causes a #VMEXIT in guest mode
		/// </summary>
		public bool AmdVmExit => (opcFlags2 & OpCodeInfoFlags2.AmdVmExit) != 0;

		/// <summary>
		/// (AMD SVM) <see langword="true"/> if it may cause a #VMEXIT in guest mode
		/// </summary>
		public bool AmdMayVmExit => (opcFlags2 & OpCodeInfoFlags2.AmdMayVmExit) != 0;

		/// <summary>
		/// <see langword="true"/> if it causes a TSX abort inside a TSX transaction
		/// </summary>
		public bool TsxAbort => (opcFlags2 & OpCodeInfoFlags2.TsxAbort) != 0;

		/// <summary>
		/// <see langword="true"/> if it causes a TSX abort inside a TSX transaction depending on the implementation
		/// </summary>
		public bool TsxImplAbort => (opcFlags2 & OpCodeInfoFlags2.TsxImplAbort) != 0;

		/// <summary>
		/// <see langword="true"/> if it may cause a TSX abort inside a TSX transaction depending on some condition
		/// </summary>
		public bool TsxMayAbort => (opcFlags2 & OpCodeInfoFlags2.TsxMayAbort) != 0;

		/// <summary>
		/// <see langword="true"/> if it's decoded by iced's 16-bit Intel decoder
		/// </summary>
		public bool IntelDecoder16 => (opcFlags2 & OpCodeInfoFlags2.IntelDecoder16or32) != 0;

		/// <summary>
		/// <see langword="true"/> if it's decoded by iced's 32-bit Intel decoder
		/// </summary>
		public bool IntelDecoder32 => (opcFlags2 & OpCodeInfoFlags2.IntelDecoder16or32) != 0;

		/// <summary>
		/// <see langword="true"/> if it's decoded by iced's 64-bit Intel decoder
		/// </summary>
		public bool IntelDecoder64 => (opcFlags2 & OpCodeInfoFlags2.IntelDecoder64) != 0;

		/// <summary>
		/// <see langword="true"/> if it's decoded by iced's 16-bit AMD decoder
		/// </summary>
		public bool AmdDecoder16 => (opcFlags2 & OpCodeInfoFlags2.AmdDecoder16or32) != 0;

		/// <summary>
		/// <see langword="true"/> if it's decoded by iced's 32-bit AMD decoder
		/// </summary>
		public bool AmdDecoder32 => (opcFlags2 & OpCodeInfoFlags2.AmdDecoder16or32) != 0;

		/// <summary>
		/// <see langword="true"/> if it's decoded by iced's 64-bit AMD decoder
		/// </summary>
		public bool AmdDecoder64 => (opcFlags2 & OpCodeInfoFlags2.AmdDecoder64) != 0;

#if DECODER
		static readonly DecoderOptions[] toDecoderOptions = new DecoderOptions[] {
			// GENERATOR-BEGIN: ToDecoderOptionsTable
			// ⚠️This was generated by GENERATOR!🦹‍♂️
			DecoderOptions.None,
			DecoderOptions.ALTINST,
			DecoderOptions.Cl1invmb,
			DecoderOptions.Cmpxchg486A,
			DecoderOptions.Cyrix,
			DecoderOptions.Cyrix_DMI,
			DecoderOptions.Cyrix_SMINT_0F7E,
			DecoderOptions.Jmpe,
			DecoderOptions.Loadall286,
			DecoderOptions.Loadall386,
			DecoderOptions.MovTr,
			DecoderOptions.MPX,
			DecoderOptions.OldFpu,
			DecoderOptions.Pcommit,
			DecoderOptions.Umov,
			DecoderOptions.Xbts,
			DecoderOptions.Udbg,
			DecoderOptions.KNC,
			// GENERATOR-END: ToDecoderOptionsTable
		};

		/// <summary>
		/// Gets the decoder option that's needed to decode the instruction or <see cref="DecoderOptions.None"/>
		/// </summary>
		public DecoderOptions DecoderOption =>
			toDecoderOptions[(int)(((uint)opcFlags1 >> (int)OpCodeInfoFlags1.DecOptionValueShift) & (uint)OpCodeInfoFlags1.DecOptionValueMask)];
#endif

		/// <summary>
		/// Gets the opcode table
		/// </summary>
		public OpCodeTableKind Table => (OpCodeTableKind)table;

		/// <summary>
		/// Gets the mandatory prefix
		/// </summary>
		public MandatoryPrefix MandatoryPrefix => (MandatoryPrefix)mandatoryPrefix;

		/// <summary>
		/// Gets the opcode byte(s). The low byte(s) of this value is the opcode. The length is in <see cref="OpCodeLength"/>.
		/// It doesn't include the table value, see <see cref="Table"/>.
		/// <br/>
		/// Example values: <c>0xDFC0</c> (<see cref="Code.Ffreep_sti"/>), <c>0x01D8</c> (<see cref="Code.Vmrunw"/>), <c>0x2A</c> (<see cref="Code.Sub_r8_rm8"/>, <see cref="Code.Cvtpi2ps_xmm_mmm64"/>, etc).
		/// </summary>
		public uint OpCode => (ushort)((uint)encFlags2 >> (int)EncFlags2.OpCodeShift);

		/// <summary>
		/// Gets the length of the opcode bytes (<see cref="OpCode"/>). The low bytes is the opcode value.
		/// </summary>
		public int OpCodeLength => (encFlags2 & EncFlags2.OpCodeIs2Bytes) != 0 ? 2 : 1;

		/// <summary>
		/// <see langword="true"/> if it's part of a group
		/// </summary>
		public bool IsGroup => GroupIndex >= 0;

		/// <summary>
		/// Group index (0-7) or -1. If it's 0-7, it's stored in the <c>reg</c> field of the <c>modrm</c> byte.
		/// </summary>
		public int GroupIndex => groupIndex;

		/// <summary>
		/// <see langword="true"/> if it's part of a modrm.rm group
		/// </summary>
		public bool IsRmGroup => RmGroupIndex >= 0;

		/// <summary>
		/// modrm.rm group index (0-7) or -1. If it's 0-7, it's stored in the <c>rm</c> field of the <c>modrm</c> byte.
		/// </summary>
		public int RmGroupIndex => rmGroupIndex;

		/// <summary>
		/// Gets the number of operands
		/// </summary>
		public int OpCount => InstructionOpCounts.OpCount[code];

		/// <summary>
		/// Gets operand #0's opkind
		/// </summary>
		public OpCodeOperandKind Op0Kind => (OpCodeOperandKind)op0Kind;

		/// <summary>
		/// Gets operand #1's opkind
		/// </summary>
		public OpCodeOperandKind Op1Kind => (OpCodeOperandKind)op1Kind;

		/// <summary>
		/// Gets operand #2's opkind
		/// </summary>
		public OpCodeOperandKind Op2Kind => (OpCodeOperandKind)op2Kind;

		/// <summary>
		/// Gets operand #3's opkind
		/// </summary>
		public OpCodeOperandKind Op3Kind => (OpCodeOperandKind)op3Kind;

		/// <summary>
		/// Gets operand #4's opkind
		/// </summary>
		public OpCodeOperandKind Op4Kind => (OpCodeOperandKind)op4Kind;

		/// <summary>
		/// Gets an operand's opkind
		/// </summary>
		/// <param name="operand">Operand number, 0-4</param>
		/// <returns></returns>
		public OpCodeOperandKind GetOpKind(int operand) =>
			operand switch {
				0 => Op0Kind,
				1 => Op1Kind,
				2 => Op2Kind,
				3 => Op3Kind,
				4 => Op4Kind,
				_ => throw new ArgumentOutOfRangeException(nameof(operand)),
			};

		/// <summary>
		/// Checks if the instruction is available in 16-bit mode, 32-bit mode or 64-bit mode
		/// </summary>
		/// <param name="bitness">16, 32 or 64</param>
		/// <returns></returns>
		public bool IsAvailableInMode(int bitness) =>
			bitness switch {
				16 => Mode16,
				32 => Mode32,
				64 => Mode64,
				_ => throw new ArgumentOutOfRangeException(nameof(bitness)),
			};

		/// <summary>
		/// Gets the opcode string, eg. <c>VEX.128.66.0F38.W0 78 /r</c>, see also <see cref="ToInstructionString()"/>
		/// </summary>
		/// <returns></returns>
		public string ToOpCodeString() => toOpCodeStringValue;

		/// <summary>
		/// Gets the instruction string, eg. <c>VPBROADCASTB xmm1, xmm2/m8</c>, see also <see cref="ToOpCodeString()"/>
		/// </summary>
		/// <returns></returns>
		public string ToInstructionString() => toInstructionStringValue;

		/// <summary>
		/// Gets the instruction string, eg. <c>VPBROADCASTB xmm1, xmm2/m8</c>, see also <see cref="ToOpCodeString()"/>
		/// </summary>
		/// <returns></returns>
		public override string ToString() => ToInstructionString();
	}
}
#endif
