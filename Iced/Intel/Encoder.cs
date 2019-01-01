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
using System.Runtime.CompilerServices;
using Iced.Intel.EncoderInternal;

namespace Iced.Intel.EncoderInternal {
	enum OperandSize : byte {
		None,
		Size16,
		Size32,
		Size64,
		// Update LegacyFlags: Legacy_OpSizeShift/Legacy_OperandSizeMask if a new value is added
	}

	enum AddressSize : byte {
		None,
		Size16,
		Size32,
		Size64,
		// Update LegacyFlags: Legacy_AddrSizeShift/Legacy_AddressSizeMask if a new value is added
	}

	enum DisplSize : byte {
		None,
		Size1,
		Size2,
		Size4,
		Size8,
		RipRelSize4_Target32,
		RipRelSize4_Target64,
	}

	enum ImmSize : byte {
		None,
		Size1,
		Size2,
		Size4,
		Size8,
		Size2_1,// enter xxxx,yy
		Size1_1,// extrq/insertq xx,yy
		Size2_2,// call16 far x:y
		Size4_2,// call32 far x:y
		RipRelSize1_Target16,
		RipRelSize1_Target32,
		RipRelSize1_Target64,
		RipRelSize2_Target16,
		RipRelSize2_Target32,
		RipRelSize2_Target64,
		RipRelSize4_Target32,
		RipRelSize4_Target64,
		SizeIbReg,
		Size1OpCode,

		Last,
	}

	[Flags]
	enum EncoderFlags : uint {
		None				= 0,
		B					= 0x00000001,
		X					= 0x00000002,
		R					= 0x00000004,
		W					= 0x00000008,

		ModRM				= 0x00000010,
		Sib					= 0x00000020,
		REX					= 0x00000040,
		P66					= 0x00000080,
		P67					= 0x00000100,

		R2					= 0x00000200,// EVEX.R'
		b					= 0x00000400,
		HighLegacy8BitRegs	= 0x00000800,
		Displ				= 0x00001000,
		PF0					= 0x00002000,

		VvvvvShift			= 27,// 5 bits
		VvvvvMask			= 0x1F,
		// Make sure all bits fit in the enum (static assert)
		VvvvvMax			= VvvvvMask << (int)VvvvvShift,
	}
}

namespace Iced.Intel {
	/// <summary>
	/// Encodes instructions decoded by the decoder or instructions created by other code
	/// </summary>
	public sealed class Encoder {
		static readonly uint[] immSizes = new uint[(int)ImmSize.Last] {
			0,
			1,
			2,
			4,
			8,
			2 + 1,
			1 + 1,
			2 + 2,
			4 + 2,
			1,
			1,
			1,
			2,
			2,
			2,
			4,
			4,
			1,
			1,
		};

		internal const string ERROR_ONLY_1632_BIT_MODE = "The instruction can only be used in 16/32-bit mode";
		internal const string ERROR_ONLY_64_BIT_MODE = "The instruction can only be used in 64-bit mode";

		readonly CodeWriter writer;
		readonly int defaultCodeSize;
		readonly OpCodeHandler[] handlers;

		ulong currentRip;
		string errorMessage;
		OpCodeHandler handler;
		uint eip;
		uint displAddr;
		uint immAddr;
		internal uint Immediate;
		// high 32 bits if it's a 64-bit immediate
		// high 32 bits if it's an IP relative immediate (jcc,call target)
		// high 32 bits if it's a 64-bit absolute address
		internal uint ImmediateHi;
		uint Displ;
		// high 32 bits if it's an IP relative mem displ (target)
		uint DisplHi;
		internal uint OpCode;
		internal EncoderFlags EncoderFlags;
		DisplSize DisplSize;
		internal ImmSize ImmSize;
		byte ModRM;
		byte Sib;

		/// <summary>
		/// Gets the bitness (16, 32 or 64)
		/// </summary>
		public int Bitness => defaultCodeSize;

		Encoder(CodeWriter writer, int defaultCodeSize) {
			Debug.Assert(defaultCodeSize == 16 || defaultCodeSize == 32 || defaultCodeSize == 64);
			this.writer = writer ?? throw new ArgumentNullException(nameof(writer));
			this.defaultCodeSize = defaultCodeSize;
			handlers = OpCodeHandlers.Handlers;
		}

		/// <summary>
		/// Creates an encoder
		/// </summary>
		/// <param name="bitness">16, 32 or 64</param>
		/// <param name="writer">Destination</param>
		/// <returns></returns>
		public static Encoder Create(int bitness, CodeWriter writer) {
			switch (bitness) {
			case 16: return Create16(writer);
			case 32: return Create32(writer);
			case 64: return Create64(writer);
			default: throw new ArgumentOutOfRangeException(nameof(bitness));
			}
		}

		/// <summary>
		/// Creates a 16-bit encoder
		/// </summary>
		/// <param name="writer">Destination</param>
		/// <returns></returns>
		public static Encoder Create16(CodeWriter writer) => new Encoder(writer, 16);

		/// <summary>
		/// Creates a 32-bit encoder
		/// </summary>
		/// <param name="writer">Destination</param>
		/// <returns></returns>
		public static Encoder Create32(CodeWriter writer) => new Encoder(writer, 32);

		/// <summary>
		/// Creates a 64-bit encoder
		/// </summary>
		/// <param name="writer">Destination</param>
		/// <returns></returns>
		public static Encoder Create64(CodeWriter writer) => new Encoder(writer, 64);

		/// <summary>
		/// Encodes an instruction and returns the size of the encoded instruction.
		/// A <see cref="EncoderException"/> is thrown if it failed to encode the instruction.
		/// </summary>
		/// <param name="instruction">Instruction to encode</param>
		/// <param name="rip">RIP of the encoded instruction</param>
		/// <returns></returns>
		public uint Encode(ref Instruction instruction, ulong rip) {
			if (!TryEncode(ref instruction, rip, out uint result, out var errorMessage))
				throw new EncoderException(errorMessage, instruction);
			return result;
		}

		/// <summary>
		/// Encodes an instruction
		/// </summary>
		/// <param name="instruction">Instruction to encode</param>
		/// <param name="rip">RIP of the encoded instruction</param>
		/// <param name="encodedLength">Updated with length of encoded instruction if successful</param>
		/// <param name="errorMessage">Set to the error message if we couldn't encode the instruction</param>
		/// <returns></returns>
		public bool TryEncode(ref Instruction instruction, ulong rip, out uint encodedLength, out string errorMessage) {
			currentRip = rip;
			eip = (uint)rip;
			this.errorMessage = null;
			EncoderFlags = EncoderFlags.None;
			DisplSize = DisplSize.None;
			ImmSize = ImmSize.None;
			ModRM = 0;

			Debug.Assert((uint)instruction.Code < (uint)handlers.Length);
			var handler = handlers[(int)instruction.Code];
			this.handler = handler;
			OpCode = handler.OpCode;
			if (handler.GroupIndex >= 0) {
				EncoderFlags |= EncoderFlags.ModRM;
				ModRM |= (byte)(handler.GroupIndex << 3);
			}

			switch (handler.Encodable) {
			case Encodable.Any:
				break;

			case Encodable.Only1632:
				if (defaultCodeSize == 64)
					ErrorMessage = ERROR_ONLY_1632_BIT_MODE;
				break;

			case Encodable.Only64:
				if (defaultCodeSize != 64)
					ErrorMessage = ERROR_ONLY_64_BIT_MODE;
				break;

			case Encodable.Unknown:
				Debug.Fail($"Missing encodable value in the encoder Code table, this is a bug. Code = {instruction.Code}");
				goto default;

			default:
				throw new InvalidOperationException();
			}

			switch (handler.OpSize) {
			case OperandSize.None:
				break;

			case OperandSize.Size16:
				if (defaultCodeSize != 16)
					EncoderFlags |= EncoderFlags.P66;
				break;

			case OperandSize.Size32:
				if (defaultCodeSize == 16)
					EncoderFlags |= EncoderFlags.P66;
				break;

			case OperandSize.Size64:
				EncoderFlags |= EncoderFlags.W;
				break;

			default:
				throw new InvalidOperationException();
			}

			switch (handler.AddrSize) {
			case AddressSize.None:
				break;

			case AddressSize.Size16:
				if (defaultCodeSize != 16)
					EncoderFlags |= EncoderFlags.P67;
				break;

			case AddressSize.Size32:
				if (defaultCodeSize != 32)
					EncoderFlags |= EncoderFlags.P67;
				break;

			case AddressSize.Size64:
				break;

			default:
				throw new InvalidOperationException();
			}

			var ops = handler.Operands;
			if (instruction.OpCount != ops.Length)
				ErrorMessage = $"Expected {ops.Length} operand(s) but the instruction has {instruction.OpCount} operand(s)";
			for (int i = 0; i < ops.Length; i++)
				ops[i].Encode(this, ref instruction, i);

			if ((handler.Flags & OpCodeHandlerFlags.Fwait) != 0)
				WriteByte(0x9B);

			WritePrefixes(ref instruction);

			handler.Encode(this, ref instruction);

			WriteOpCode();

			if ((EncoderFlags & (EncoderFlags.ModRM | EncoderFlags.Displ)) != 0)
				WriteModRM();

			WriteImmediate();

			uint instrLen = (uint)currentRip - (uint)rip;
			if (instrLen > DecoderConstants.MaxInstructionLength)
				ErrorMessage = $"Instruction length > {DecoderConstants.MaxInstructionLength} bytes";
			errorMessage = this.errorMessage;
			if (errorMessage != null) {
				encodedLength = 0;
				return false;
			}
			encodedLength = instrLen;
			return true;
		}

		internal string ErrorMessage {
			get => errorMessage;
			set {
				if (errorMessage == null)
					errorMessage = value;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal bool Verify(int operand, OpKind expected, OpKind actual) {
			if (expected == actual)
				return true;
			ErrorMessage = $"Operand {operand}: Expected: {expected}, actual: {actual}";
			return false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal bool Verify(int operand, Register expected, Register actual) {
			if (expected == actual)
				return true;
			ErrorMessage = $"Operand {operand}: Expected: {expected}, actual: {actual}";
			return false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal bool Verify(int operand, Register register, Register regLo, Register regHi) {
			if (defaultCodeSize != 64 && regHi > regLo + 7)
				regHi = regLo + 7;
			if (regLo <= register && register <= regHi)
				return true;
			ErrorMessage = $"Operand {operand}: Register {register} is not between {regLo} and {regHi} (inclusive)";
			return false;
		}

		internal void AddBranch(OpKind opKind, int immSize, ref Instruction instr, int operand) {
			if (!Verify(operand, opKind, instr.GetOpKind(operand)))
				return;

			ulong target;
			switch (immSize) {
			case 1:
				switch (opKind) {
				case OpKind.NearBranch16:
					if (defaultCodeSize != 16)
						EncoderFlags |= EncoderFlags.P66;
					ImmSize = ImmSize.RipRelSize1_Target16;
					Immediate = instr.NearBranch16;
					break;

				case OpKind.NearBranch32:
					if (defaultCodeSize == 16)
						EncoderFlags |= EncoderFlags.P66;
					ImmSize = ImmSize.RipRelSize1_Target32;
					Immediate = instr.NearBranch32;
					break;

				case OpKind.NearBranch64:
					ImmSize = ImmSize.RipRelSize1_Target64;
					target = instr.NearBranch64;
					Immediate = (uint)target;
					ImmediateHi = (uint)(target >> 32);
					break;

				default:
					throw new InvalidOperationException();
				}
				break;

			case 2:
				switch (opKind) {
				case OpKind.NearBranch16:
					if (defaultCodeSize != 16)
						EncoderFlags |= EncoderFlags.P66;
					ImmSize = ImmSize.RipRelSize2_Target16;
					Immediate = instr.NearBranch16;
					break;

				default:
					throw new InvalidOperationException();
				}
				break;

			case 4:
				switch (opKind) {
				case OpKind.NearBranch32:
					if (defaultCodeSize == 16)
						EncoderFlags |= EncoderFlags.P66;
					ImmSize = ImmSize.RipRelSize4_Target32;
					Immediate = instr.NearBranch32;
					break;

				case OpKind.NearBranch64:
					ImmSize = ImmSize.RipRelSize4_Target64;
					target = instr.NearBranch64;
					Immediate = (uint)target;
					ImmediateHi = (uint)(target >> 32);
					break;

				default:
					throw new InvalidOperationException();
				}
				break;

			default:
				throw new InvalidOperationException();
			}
		}

		internal void AddBranchX(int immSize, ref Instruction instr, int operand) {
			if (defaultCodeSize == 64) {
				if (!Verify(operand, OpKind.NearBranch64, instr.GetOpKind(operand)))
					return;

				var target = instr.NearBranch64;
				switch (immSize) {
				case 2:
					EncoderFlags |= EncoderFlags.P66;
					ImmSize = ImmSize.RipRelSize2_Target64;
					Immediate = (uint)target;
					ImmediateHi = (uint)(target >> 32);
					break;

				case 4:
					ImmSize = ImmSize.RipRelSize4_Target64;
					Immediate = (uint)target;
					ImmediateHi = (uint)(target >> 32);
					break;

				case 8:
					EncoderFlags |= EncoderFlags.W;
					ImmSize = ImmSize.RipRelSize4_Target64;
					Immediate = (uint)target;
					ImmediateHi = (uint)(target >> 32);
					break;

				default:
					throw new InvalidOperationException();
				}
			}
			else if (defaultCodeSize == 32) {
				if (!Verify(operand, OpKind.NearBranch32, instr.GetOpKind(operand)))
					return;

				switch (immSize) {
				case 2:
					EncoderFlags |= EncoderFlags.P66;
					ImmSize = ImmSize.RipRelSize2_Target32;
					Immediate = instr.NearBranch32;
					break;

				case 4:
					ImmSize = ImmSize.RipRelSize4_Target32;
					Immediate = instr.NearBranch32;
					break;

				case 8:
				default:
					throw new InvalidOperationException();
				}
			}
			else {
				Debug.Assert(defaultCodeSize == 16);
				if (!Verify(operand, OpKind.NearBranch16, instr.GetOpKind(operand)))
					return;

				switch (immSize) {
				case 2:
					ImmSize = ImmSize.RipRelSize2_Target16;
					Immediate = instr.NearBranch16;
					break;

				case 4:
					EncoderFlags |= EncoderFlags.P66;
					ImmSize = ImmSize.RipRelSize4_Target32;
					Immediate = instr.NearBranch16;
					break;

				case 8:
				default:
					throw new InvalidOperationException();
				}
			}
		}

		internal void AddBranchDisp(int displSize, ref Instruction instr, int operand) {
			Debug.Assert(displSize == 2 || displSize == 4);
			OpKind opKind;
			switch (displSize) {
			case 2:
				opKind = OpKind.NearBranch16;
				ImmSize = ImmSize.Size2;
				Immediate = instr.NearBranch16;
				break;

			case 4:
				opKind = OpKind.NearBranch32;
				ImmSize = ImmSize.Size4;
				Immediate = instr.NearBranch32;
				break;

			default:
				throw new InvalidOperationException();
			}
			if (!Verify(operand, opKind, instr.GetOpKind(operand)))
				return;
		}

		internal void AddFarBranch(ref Instruction instr, int operand, int size) {
			if (size == 2) {
				if (!Verify(operand, OpKind.FarBranch16, instr.GetOpKind(operand)))
					return;
				ImmSize = ImmSize.Size2_2;
				Immediate = instr.FarBranch16;
				ImmediateHi = instr.FarBranchSelector;
			}
			else {
				Debug.Assert(size == 4);
				if (!Verify(operand, OpKind.FarBranch32, instr.GetOpKind(operand)))
					return;
				ImmSize = ImmSize.Size4_2;
				Immediate = instr.FarBranch32;
				ImmediateHi = instr.FarBranchSelector;
			}
			if (defaultCodeSize != size * 8)
				EncoderFlags |= EncoderFlags.P66;
		}

		internal void SetAddrSize(int regSize) {
			Debug.Assert(regSize == 2 || regSize == 4 || regSize == 8);
			if (defaultCodeSize == 64) {
				if (regSize == 2) {
					ErrorMessage = $"Invalid register size: {regSize * 8}, must be 32-bit or 64-bit";
					return;
				}
				else if (regSize == 4)
					EncoderFlags |= EncoderFlags.P67;
			}
			else {
				if (regSize == 8) {
					ErrorMessage = $"Invalid register size: {regSize * 8}, must be 16-bit or 32-bit";
					return;
				}
				if (defaultCodeSize == 16) {
					if (regSize == 4)
						EncoderFlags |= EncoderFlags.P67;
				}
				else {
					if (regSize == 2)
						EncoderFlags |= EncoderFlags.P67;
				}
			}
		}

		internal void AddAbsMem(ref Instruction instr, int operand) {
			EncoderFlags |= EncoderFlags.Displ;
			var opKind = instr.GetOpKind(operand);
			if (opKind == OpKind.Memory64) {
				if (defaultCodeSize != 64) {
					ErrorMessage = $"Operand {operand}: 64-bit abs address is only available in 64-bit mode";
					return;
				}
				DisplSize = DisplSize.Size8;
				ulong addr = instr.MemoryAddress64;
				Displ = (uint)addr;
				DisplHi = (uint)(addr >> 32);
			}
			else if (opKind == OpKind.Memory) {
				if (instr.MemoryBase != Register.None || instr.MemoryIndex != Register.None) {
					ErrorMessage = $"Operand {operand}: Absolute addresses can't have base and/or index regs";
					return;
				}
				var displSize = instr.MemoryDisplSize;
				if (displSize == 2) {
					if (defaultCodeSize == 64) {
						ErrorMessage = $"Operand {operand}: 16-bit abs addresses can't be used in 64-bit mode";
						return;
					}
					if (defaultCodeSize == 32)
						EncoderFlags |= EncoderFlags.P67;
					DisplSize = DisplSize.Size2;
					Displ = instr.MemoryDisplacement;
				}
				else if (displSize == 4) {
					if (defaultCodeSize != 32)
						EncoderFlags |= EncoderFlags.P67;
					DisplSize = DisplSize.Size4;
					Displ = instr.MemoryDisplacement;
				}
				else
					ErrorMessage = $"Operand {operand}: {nameof(Instruction)}.{nameof(Instruction.MemoryDisplSize)} must be initialized to 2 (16-bit) or 4 (32-bit)";
			}
			else
				ErrorMessage = $"Operand {operand}: Expected OpKind {nameof(OpKind.Memory)} or {nameof(OpKind.Memory64)}, actual: {opKind}";
		}

		internal void AddModRMRegister(ref Instruction instr, int operand, Register regLo, Register regHi) {
			if (!Verify(operand, OpKind.Register, instr.GetOpKind(operand)))
				return;
			var reg = instr.GetOpRegister(operand);
			if (!Verify(operand, reg, regLo, regHi))
				return;
			uint regNum = (uint)(reg - regLo);
			if (regLo == Register.AL) {
				if (reg >= Register.SPL) {
					regNum -= 4;
					EncoderFlags |= EncoderFlags.REX;
				}
				else if (reg >= Register.AH)
					EncoderFlags |= EncoderFlags.HighLegacy8BitRegs;
			}
			Debug.Assert(regNum <= 31);
			ModRM |= (byte)((regNum & 7) << 3);
			EncoderFlags |= EncoderFlags.ModRM;
			Debug.Assert((int)EncoderFlags.R == 4);
			EncoderFlags |= (EncoderFlags)((regNum & 8) >> 1);
			Debug.Assert((int)EncoderFlags.R2 == 0x200);
			EncoderFlags |= (EncoderFlags)((regNum & 0x10) << (9 - 4));
		}

		internal void AddReg(ref Instruction instr, int operand, Register regLo, Register regHi) {
			if (!Verify(operand, OpKind.Register, instr.GetOpKind(operand)))
				return;
			var reg = instr.GetOpRegister(operand);
			if (!Verify(operand, reg, regLo, regHi))
				return;
			uint regNum = (uint)(reg - regLo);
			if (regLo == Register.AL) {
				if (reg >= Register.SPL) {
					regNum -= 4;
					EncoderFlags |= EncoderFlags.REX;
				}
				else if (reg >= Register.AH)
					EncoderFlags |= EncoderFlags.HighLegacy8BitRegs;
			}
			Debug.Assert(regNum <= 15);
			OpCode |= regNum & 7;
			Debug.Assert((int)EncoderFlags.B == 1);
			Debug.Assert(regNum <= 15);
			EncoderFlags |= (EncoderFlags)(regNum >> 3);// regNum <= 15, so no need to mask out anything
		}

		internal void AddRegOrMem(ref Instruction instr, int operand, Register regLo, Register regHi, bool allowMemOp, bool allowRegOp) =>
			AddRegOrMem(ref instr, operand, regLo, regHi, Register.None, Register.None, allowMemOp, allowRegOp);

		internal void AddRegOrMem(ref Instruction instr, int operand, Register regLo, Register regHi, Register vsibIndexRegLo, Register vsibIndexRegHi, bool allowMemOp, bool allowRegOp) {
			var opKind = instr.GetOpKind(operand);
			EncoderFlags |= EncoderFlags.ModRM;
			if (opKind == OpKind.Register) {
				if (!allowRegOp) {
					ErrorMessage = $"Operand {operand}: register operand is not allowed";
					return;
				}
				var reg = instr.GetOpRegister(operand);
				if (!Verify(operand, reg, regLo, regHi))
					return;
				uint regNum = (uint)(reg - regLo);
				if (regLo == Register.AL) {
					if (reg >= Register.R8L)
						regNum -= 4;
					else if (reg >= Register.SPL) {
						regNum -= 4;
						EncoderFlags |= EncoderFlags.REX;
					}
					else if (reg >= Register.AH)
						EncoderFlags |= EncoderFlags.HighLegacy8BitRegs;
				}
				ModRM |= (byte)(regNum & 7);
				ModRM |= 0xC0;
				Debug.Assert((int)EncoderFlags.B == 1);
				Debug.Assert((int)EncoderFlags.X == 2);
				EncoderFlags |= (EncoderFlags)((regNum >> 3) & 3);
				Debug.Assert(regNum <= 31);
			}
			else if (opKind == OpKind.Memory) {
				if (!allowMemOp) {
					ErrorMessage = $"Operand {operand}: memory operand is not allowed";
					return;
				}
				if (instr.MemorySize.IsBroadcast())
					EncoderFlags |= EncoderFlags.b;

				var codeSize = instr.CodeSize;
				if (codeSize == CodeSize.Unknown) {
					if (defaultCodeSize == 64)
						codeSize = CodeSize.Code64;
					else if (defaultCodeSize == 32)
						codeSize = CodeSize.Code32;
					else {
						Debug.Assert(defaultCodeSize == 16);
						codeSize = CodeSize.Code16;
					}
				}
				int addrSize = InstructionUtils.GetAddressSizeInBytes(instr.MemoryBase, instr.MemoryIndex, instr.MemoryDisplSize, codeSize) * 8;
				if (addrSize != defaultCodeSize)
					EncoderFlags |= EncoderFlags.P67;
				if (addrSize == 16) {
					if (vsibIndexRegLo != Register.None) {
						ErrorMessage = $"Operand {operand}: VSIB operands can't use 16-bit addressing. It must be 32-bit or 64-bit addressing";
						return;
					}
					AddMemOp16(ref instr, operand);
				}
				else
					AddMemOp(ref instr, operand, addrSize, vsibIndexRegLo, vsibIndexRegHi);
			}
			else
				ErrorMessage = $"Operand {operand}: Expected a register or memory operand, but opKind is {opKind}";
		}

		bool TryConvertToDisp8N(ref Instruction instr, int displ, out sbyte compressedValue) {
			var tryConvertToDisp8N = handler.TryConvertToDisp8N;
			if (tryConvertToDisp8N != null)
				return tryConvertToDisp8N(this, ref instr, handler, displ, out compressedValue);
			if (sbyte.MinValue <= displ && displ <= sbyte.MaxValue) {
				compressedValue = (sbyte)displ;
				return true;
			}
			compressedValue = 0;
			return false;
		}

		void AddMemOp16(ref Instruction instr, int operand) {
			if (defaultCodeSize == 64) {
				ErrorMessage = $"Operand {operand}: 16-bit addressing can't be used by 64-bit code";
				return;
			}
			var baseReg = instr.MemoryBase;
			var indexReg = instr.MemoryIndex;
			var displSize = instr.MemoryDisplSize;
			if (baseReg == Register.BX && indexReg == Register.SI) {
				// Nothing
			}
			else if (baseReg == Register.BX && indexReg == Register.DI)
				ModRM |= 1;
			else if (baseReg == Register.BP && indexReg == Register.SI)
				ModRM |= 2;
			else if (baseReg == Register.BP && indexReg == Register.DI)
				ModRM |= 3;
			else if (baseReg == Register.SI && indexReg == Register.None)
				ModRM |= 4;
			else if (baseReg == Register.DI && indexReg == Register.None)
				ModRM |= 5;
			else if (baseReg == Register.BP && indexReg == Register.None)
				ModRM |= 6;
			else if (baseReg == Register.BX && indexReg == Register.None)
				ModRM |= 7;
			else if (baseReg == Register.None && indexReg == Register.None) {
				ModRM |= 6;
				DisplSize = DisplSize.Size2;
				Displ = instr.MemoryDisplacement;
			}
			else {
				ErrorMessage = $"Operand {operand}: Invalid 16-bit base + index registers: base={baseReg}, index={indexReg}";
				return;
			}

			if (baseReg != Register.None || indexReg != Register.None) {
				Displ = instr.MemoryDisplacement;
				// [bp] => [bp+00]
				if (displSize == 0 && baseReg == Register.BP && indexReg == Register.None) {
					displSize = 1;
					Displ = 0;
				}
				if (displSize == 1) {
					if (TryConvertToDisp8N(ref instr, (short)Displ, out sbyte compressedValue))
						Displ = (byte)compressedValue;
					else
						displSize = 2;
				}
				if (displSize == 0) {
					// Nothing
				}
				else if (displSize == 1) {
					ModRM |= 0x40;
					DisplSize = DisplSize.Size1;
				}
				else if (displSize == 2) {
					ModRM |= 0x80;
					DisplSize = DisplSize.Size2;
				}
				else {
					ErrorMessage = $"Operand {operand}: Invalid displacement size: {displSize}, must be 0, 1, or 2";
					return;
				}
			}
		}

		void AddMemOp(ref Instruction instr, int operand, int addrSize, Register vsibIndexRegLo, Register vsibIndexRegHi) {
			Debug.Assert(addrSize == 32 || addrSize == 64);
			if (defaultCodeSize != 64 && addrSize == 64) {
				ErrorMessage = $"Operand {operand}: 64-bit addressing can only be used in 64-bit mode";
				return;
			}

			var baseReg = instr.MemoryBase;
			var indexReg = instr.MemoryIndex;
			var displSize = instr.MemoryDisplSize;
			Displ = instr.MemoryDisplacement;

			Register baseRegLo, baseRegHi;
			Register indexRegLo, indexRegHi;
			if (addrSize == 32) {
				baseRegLo = Register.EAX;
				baseRegHi = Register.R15D;
			}
			else {
				Debug.Assert(addrSize == 64);
				baseRegLo = Register.RAX;
				baseRegHi = Register.R15;
			}
			if (vsibIndexRegLo != Register.None) {
				indexRegLo = vsibIndexRegLo;
				indexRegHi = vsibIndexRegHi;
			}
			else {
				indexRegLo = baseRegLo;
				indexRegHi = baseRegHi;
			}
			if (baseReg != Register.None && baseReg != Register.RIP && baseReg != Register.EIP && !Verify(operand, baseReg, baseRegLo, baseRegHi))
				return;
			if (indexReg != Register.None && !Verify(operand, indexReg, indexRegLo, indexRegHi))
				return;

			if (displSize != 0 && displSize != 1 && displSize != 4 && displSize != 8) {
				ErrorMessage = $"Operand {operand}: Invalid displ size: {displSize}, must be 0, 1, 4, 8";
				return;
			}
			if (baseReg == Register.RIP || baseReg == Register.EIP) {
				if (indexReg != Register.None) {
					ErrorMessage = $"Operand {operand}: RIP relative addressing can't use an index register";
					return;
				}
				if (defaultCodeSize != 64) {
					ErrorMessage = $"Operand {operand}: RIP/EIP relative addressing is only available in 64-bit mode";
					return;
				}
				ModRM |= 5;
				if (baseReg == Register.RIP) {
					DisplSize = DisplSize.RipRelSize4_Target64;
					ulong target = instr.NextIP64 + (ulong)(int)Displ;
					Displ = (uint)target;
					DisplHi = (uint)(target >> 32);
				}
				else {
					DisplSize = DisplSize.RipRelSize4_Target32;
					Displ = instr.NextIP32 + Displ;
				}
				return;
			}
			var scale = instr.InternalMemoryIndexScale;
			if (baseReg == Register.None && indexReg == Register.None) {
				if (vsibIndexRegLo != Register.None) {
					ErrorMessage = $"Operand {operand}: VSIB addressing can't use an offset-only address";
					return;
				}
				if (defaultCodeSize == 64 || scale != 0) {
					ModRM |= 4;
					DisplSize = DisplSize.Size4;
					EncoderFlags |= EncoderFlags.Sib;
					Sib = (byte)(0x25 | (scale << 6));
					return;
				}
				else {
					ModRM |= 5;
					DisplSize = DisplSize.Size4;
					return;
				}
			}

			int baseNum = baseReg == Register.None ? -1 : baseReg - baseRegLo;
			int indexNum = indexReg == Register.None ? -1 : indexReg - indexRegLo;

			// [ebp] => [ebp+00]
			if (displSize == 0 && indexReg == Register.None && (baseNum & 7) == 5) {
				displSize = 1;
				Displ = 0;
			}

			if (displSize == 1) {
				if (TryConvertToDisp8N(ref instr, (short)Displ, out sbyte compressedValue))
					Displ = (byte)compressedValue;
				else
					displSize = addrSize / 8;
			}

			if (baseReg == Register.None) {
				// Tested earlier in the method
				Debug.Assert(indexReg != Register.None);
				DisplSize = DisplSize.Size4;
			}
			else if (displSize == 1) {
				ModRM |= 0x40;
				DisplSize = DisplSize.Size1;
			}
			else if (addrSize == 32 ? displSize == 4 : displSize == 8) {
				ModRM |= 0x80;
				DisplSize = DisplSize.Size4;
			}
			else if (displSize != 0)
				throw new ArgumentException($"Invalid displSize = {displSize}");

			if (indexReg == Register.None && (baseNum & 7) != 4 && scale == 0) {
				// Tested earlier in the method
				Debug.Assert(baseReg != Register.None);
				ModRM |= (byte)(baseNum & 7);
			}
			else {
				EncoderFlags |= EncoderFlags.Sib;
				Sib = (byte)(scale << 6);
				ModRM |= 4;
				if (indexReg == Register.RSP || indexReg == Register.ESP) {
					ErrorMessage = $"Operand {operand}: ESP/RSP can't be used as an index register";
					return;
				}
				if (baseNum < 0)
					Sib |= 5;
				else
					Sib |= (byte)(baseNum & 7);
				if (indexNum < 0)
					Sib |= 0x20;
				else
					Sib |= (byte)((indexNum & 7) << 3);
			}

			if (baseNum >= 0) {
				Debug.Assert((int)EncoderFlags.B == 1);
				Debug.Assert(baseNum <= 15);// No '& 1' required below
				EncoderFlags |= (EncoderFlags)(baseNum >> 3);
			}
			if (indexNum >= 0) {
				Debug.Assert((int)EncoderFlags.X == 2);
				EncoderFlags |= (EncoderFlags)((indexNum >> 2) & 2);
				EncoderFlags |= (EncoderFlags)((indexNum & 0x10) << (int)EncoderFlags.VvvvvShift);
				Debug.Assert(indexNum <= 31);
			}
		}

		static readonly byte[] segmentOverrides = new byte[6] { 0x26, 0x2E, 0x36, 0x3E, 0x64, 0x65 };
		void WritePrefixes(ref Instruction instr) {
			var seg = instr.SegmentPrefix;
			if (seg != Register.None) {
				Debug.Assert((uint)(seg - Register.ES) < (uint)segmentOverrides.Length);
				WriteByte(segmentOverrides[seg - Register.ES]);
			}
			if ((EncoderFlags & EncoderFlags.PF0) != 0 || instr.HasLockPrefix)
				WriteByte(0xF0);
			if ((EncoderFlags & EncoderFlags.P66) != 0)
				WriteByte(0x66);
			if ((EncoderFlags & EncoderFlags.P67) != 0)
				WriteByte(0x67);
			if (instr.Internal_HasRepePrefix_HasXreleasePrefix)
				WriteByte(0xF3);
			if (instr.Internal_HasRepnePrefix_HasXacquirePrefix)
				WriteByte(0xF2);
		}

		void WriteOpCode() {
			var opCode = OpCode;
			if (opCode <= 0x000000FF)
				WriteByte(opCode);
			else if (opCode <= 0x0000FFFF) {
				WriteByte(opCode >> 8);
				WriteByte(opCode);
			}
			else if (opCode <= 0x00FFFFFF) {
				WriteByte(opCode >> 16);
				WriteByte(opCode >> 8);
				WriteByte(opCode);
			}
			else {
				WriteByte(opCode >> 24);
				WriteByte(opCode >> 16);
				WriteByte(opCode >> 8);
				WriteByte(opCode);
			}
		}

		void WriteModRM() {
			Debug.Assert((EncoderFlags & (EncoderFlags.ModRM | EncoderFlags.Displ)) != 0);
			if ((EncoderFlags & EncoderFlags.ModRM) != 0) {
				WriteByte(ModRM);
				if ((EncoderFlags & EncoderFlags.Sib) != 0)
					WriteByte(Sib);
			}

			uint diff4;
			displAddr = (uint)currentRip;
			switch (DisplSize) {
			case DisplSize.None:
				break;

			case DisplSize.Size1:
				WriteByte(Displ);
				break;

			case DisplSize.Size2:
				diff4 = Displ;
				WriteByte(diff4);
				WriteByte(diff4 >> 8);
				break;

			case DisplSize.Size4:
				diff4 = Displ;
				WriteByte(diff4);
				WriteByte(diff4 >> 8);
				WriteByte(diff4 >> 16);
				WriteByte(diff4 >> 24);
				break;

			case DisplSize.Size8:
				diff4 = Displ;
				WriteByte(diff4);
				WriteByte(diff4 >> 8);
				WriteByte(diff4 >> 16);
				WriteByte(diff4 >> 24);
				diff4 = DisplHi;
				WriteByte(diff4);
				WriteByte(diff4 >> 8);
				WriteByte(diff4 >> 16);
				WriteByte(diff4 >> 24);
				break;

			case DisplSize.RipRelSize4_Target32:
				uint eip = (uint)currentRip + 4 + immSizes[(int)ImmSize];
				diff4 = Displ - eip;
				WriteByte(diff4);
				WriteByte(diff4 >> 8);
				WriteByte(diff4 >> 16);
				WriteByte(diff4 >> 24);
				break;

			case DisplSize.RipRelSize4_Target64:
				ulong rip = currentRip + 4 + immSizes[(int)ImmSize];
				long diff8 = (long)(((ulong)DisplHi << 32) | (ulong)Displ) - (long)rip;
				if (diff8 < int.MinValue || diff8 > int.MaxValue)
					ErrorMessage = $"RIP relative distance is too far away: nextIp: 0x{rip:X16} target: 0x{DisplHi:X8}{Displ:X8}, diff = {diff8}, diff must fit in an Int32";
				diff4 = (uint)diff8;
				WriteByte(diff4);
				WriteByte(diff4 >> 8);
				WriteByte(diff4 >> 16);
				WriteByte(diff4 >> 24);
				break;

			default:
				throw new InvalidOperationException();
			}
		}

		void WriteImmediate() {
			ushort ip;
			uint eip;
			ulong rip;
			short diff2;
			int diff4;
			long diff8;
			uint value;
			immAddr = (uint)currentRip;
			switch (ImmSize) {
			case ImmSize.None:
				break;

			case ImmSize.Size1:
			case ImmSize.SizeIbReg:
			case ImmSize.Size1OpCode:
				WriteByte(Immediate);
				break;

			case ImmSize.Size2:
				value = Immediate;
				WriteByte(value);
				WriteByte(value >> 8);
				break;

			case ImmSize.Size4:
				value = Immediate;
				WriteByte(value);
				WriteByte(value >> 8);
				WriteByte(value >> 16);
				WriteByte(value >> 24);
				break;

			case ImmSize.Size8:
				value = Immediate;
				WriteByte(value);
				WriteByte(value >> 8);
				WriteByte(value >> 16);
				WriteByte(value >> 24);
				value = ImmediateHi;
				WriteByte(value);
				WriteByte(value >> 8);
				WriteByte(value >> 16);
				WriteByte(value >> 24);
				break;

			case ImmSize.Size2_1:
				value = Immediate;
				WriteByte(value);
				WriteByte(value >> 8);
				WriteByte(ImmediateHi);
				break;

			case ImmSize.Size1_1:
				WriteByte(Immediate);
				WriteByte(ImmediateHi);
				break;

			case ImmSize.Size2_2:
				value = Immediate;
				WriteByte(value);
				WriteByte(value >> 8);
				value = ImmediateHi;
				WriteByte(value);
				WriteByte(value >> 8);
				break;

			case ImmSize.Size4_2:
				value = Immediate;
				WriteByte(value);
				WriteByte(value >> 8);
				WriteByte(value >> 16);
				WriteByte(value >> 24);
				value = ImmediateHi;
				WriteByte(value);
				WriteByte(value >> 8);
				break;

			case ImmSize.RipRelSize1_Target16:
				ip = (ushort)((uint)currentRip + 1);
				diff2 = (short)((short)Immediate - ip);
				if (diff2 < sbyte.MinValue || diff2 > sbyte.MaxValue)
					ErrorMessage = $"Branch distance is too far away: nextIp: 0x{ip:X4} target: 0x{(ushort)Immediate:X4}, diff = {diff2}, diff must fit in an Int8";
				WriteByte((uint)diff2);
				break;

			case ImmSize.RipRelSize1_Target32:
				eip = (uint)currentRip + 1;
				diff4 = (int)Immediate - (int)eip;
				if (diff4 < sbyte.MinValue || diff4 > sbyte.MaxValue)
					ErrorMessage = $"Branch distance is too far away: nextIp: 0x{eip:X8} target: 0x{Immediate:X8}, diff = {diff4}, diff must fit in an Int8";
				WriteByte((uint)diff4);
				break;

			case ImmSize.RipRelSize1_Target64:
				rip = currentRip + 1;
				diff8 = (long)(((ulong)ImmediateHi << 32) | (ulong)Immediate) - (long)rip;
				if (diff8 < sbyte.MinValue || diff8 > sbyte.MaxValue)
					ErrorMessage = $"Branch distance is too far away: nextIp: 0x{rip:X16} target: 0x{ImmediateHi:X8}{Immediate:X8}, diff = {diff8}, diff must fit in an Int8";
				WriteByte((uint)diff8);
				break;

			case ImmSize.RipRelSize2_Target16:
				ip = (ushort)((uint)currentRip + 2);
				value = Immediate - ip;
				WriteByte(value);
				WriteByte(value >> 8);
				break;

			case ImmSize.RipRelSize2_Target32:
				eip = (uint)currentRip + 2;
				diff4 = (int)(Immediate - eip);
				if (diff4 < short.MinValue || diff4 > short.MaxValue)
					ErrorMessage = $"Branch distance is too far away: nextIp: 0x{eip:X8} target: 0x{Immediate:X8}, diff = {diff4}, diff must fit in an Int16";
				value = (uint)diff4;
				WriteByte(value);
				WriteByte(value >> 8);
				break;

			case ImmSize.RipRelSize2_Target64:
				rip = currentRip + 2;
				diff8 = (long)(((ulong)ImmediateHi << 32) | (ulong)Immediate) - (long)rip;
				if (diff8 < short.MinValue || diff8 > short.MaxValue)
					ErrorMessage = $"Branch distance is too far away: nextIp: 0x{rip:X16} target: 0x{ImmediateHi:X8}{Immediate:X8}, diff = {diff8}, diff must fit in an Int16";
				value = (uint)diff8;
				WriteByte(value);
				WriteByte(value >> 8);
				break;

			case ImmSize.RipRelSize4_Target32:
				eip = (uint)currentRip + 4;
				value = Immediate - eip;
				WriteByte(value);
				WriteByte(value >> 8);
				WriteByte(value >> 16);
				WriteByte(value >> 24);
				break;

			case ImmSize.RipRelSize4_Target64:
				rip = currentRip + 4;
				diff8 = (long)(((ulong)ImmediateHi << 32) | (ulong)Immediate) - (long)rip;
				if (diff8 < int.MinValue || diff8 > int.MaxValue)
					ErrorMessage = $"Branch distance is too far away: nextIp: 0x{rip:X16} target: 0x{ImmediateHi:X8}{Immediate:X8}, diff = {diff8}, diff must fit in an Int32";
				value = (uint)diff8;
				WriteByte(value);
				WriteByte(value >> 8);
				WriteByte(value >> 16);
				WriteByte(value >> 24);
				break;

			case ImmSize.Last:
			default:
				throw new InvalidOperationException();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void WriteByte(uint value) {
			writer.WriteByte((byte)value);
			currentRip++;
		}

		/// <summary>
		/// Gets the offsets of the constants (memory displacement and immediate) in the encoded instruction.
		/// The caller can use this information to add relocations if needed.
		/// </summary>
		/// <returns></returns>
		public ConstantOffsets GetConstantOffsets() {
			ConstantOffsets constantOffsets = default;

			switch (DisplSize) {
			case DisplSize.None:
				break;

			case DisplSize.Size1:
				constantOffsets.DisplacementSize = 1;
				constantOffsets.DisplacementOffset = (byte)(displAddr - eip);
				break;

			case DisplSize.Size2:
				constantOffsets.DisplacementSize = 2;
				constantOffsets.DisplacementOffset = (byte)(displAddr - eip);
				break;

			case DisplSize.Size4:
			case DisplSize.RipRelSize4_Target32:
			case DisplSize.RipRelSize4_Target64:
				constantOffsets.DisplacementSize = 4;
				constantOffsets.DisplacementOffset = (byte)(displAddr - eip);
				break;

			case DisplSize.Size8:
				constantOffsets.DisplacementSize = 8;
				constantOffsets.DisplacementOffset = (byte)(displAddr - eip);
				break;

			default:
				throw new InvalidOperationException();
			}

			switch (ImmSize) {
			case ImmSize.None:
			case ImmSize.RipRelSize1_Target16:
			case ImmSize.RipRelSize1_Target32:
			case ImmSize.RipRelSize1_Target64:
			case ImmSize.RipRelSize2_Target16:
			case ImmSize.RipRelSize2_Target32:
			case ImmSize.RipRelSize2_Target64:
			case ImmSize.RipRelSize4_Target32:
			case ImmSize.RipRelSize4_Target64:
			case ImmSize.SizeIbReg:
			case ImmSize.Size1OpCode:
				break;

			case ImmSize.Size1:
				constantOffsets.ImmediateSize = 1;
				constantOffsets.ImmediateOffset = (byte)(immAddr - eip);
				break;

			case ImmSize.Size1_1:
				constantOffsets.ImmediateSize = 1;
				constantOffsets.ImmediateOffset = (byte)(immAddr - eip);
				constantOffsets.ImmediateSize2 = 1;
				constantOffsets.ImmediateOffset2 = (byte)(immAddr - eip + 1);
				break;

			case ImmSize.Size2:
				constantOffsets.ImmediateSize = 2;
				constantOffsets.ImmediateOffset = (byte)(immAddr - eip);
				break;

			case ImmSize.Size2_1:
				constantOffsets.ImmediateSize = 2;
				constantOffsets.ImmediateOffset = (byte)(immAddr - eip);
				constantOffsets.ImmediateSize2 = 1;
				constantOffsets.ImmediateOffset2 = (byte)(immAddr - eip + 2);
				break;

			case ImmSize.Size2_2:
				constantOffsets.ImmediateSize = 2;
				constantOffsets.ImmediateOffset = (byte)(immAddr - eip);
				constantOffsets.ImmediateSize2 = 2;
				constantOffsets.ImmediateOffset2 = (byte)(immAddr - eip + 2);
				break;

			case ImmSize.Size4:
				constantOffsets.ImmediateSize = 4;
				constantOffsets.ImmediateOffset = (byte)(immAddr - eip);
				break;

			case ImmSize.Size4_2:
				constantOffsets.ImmediateSize = 4;
				constantOffsets.ImmediateOffset = (byte)(immAddr - eip);
				constantOffsets.ImmediateSize2 = 2;
				constantOffsets.ImmediateOffset2 = (byte)(immAddr - eip + 4);
				break;

			case ImmSize.Size8:
				constantOffsets.ImmediateSize = 8;
				constantOffsets.ImmediateOffset = (byte)(immAddr - eip);
				break;

			case ImmSize.Last:
			default:
				throw new InvalidOperationException();
			}

			return constantOffsets;
		}
	}
}
#endif
