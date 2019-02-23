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

#if !NO_INSTR_INFO
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Iced.Intel.InstructionInfoInternal;

namespace Iced.Intel {
	/// <summary>
	/// Instruction info options
	/// </summary>
	[Flags]
	public enum InstructionInfoOptions : uint {
		/// <summary>
		/// No option is enabled
		/// </summary>
		None						= 0,

		/// <summary>
		/// Don't include memory usage, eg. <see cref="InstructionInfo.GetUsedMemory"/> will return an empty iterator. All
		/// registers that are used by memory operands are still returned by <see cref="InstructionInfo.GetUsedRegisters"/>.
		/// </summary>
		NoMemoryUsage				= 0x00000001,

		/// <summary>
		/// Don't include register usage, eg. <see cref="InstructionInfo.GetUsedRegisters"/> will return an empty iterator
		/// </summary>
		NoRegisterUsage				= 0x00000002,
	}

	/// <summary>
	/// Creates <see cref="InstructionInfo"/>s but minimizes allocations
	/// </summary>
	public sealed class InstructionInfoFactory {
		const int defaultRegisterArrayCount = 2;
		const int defaultMemoryArrayCount = 1;

		SimpleList<UsedRegister> usedRegisters;
		SimpleList<UsedMemory> usedMemoryLocations;

		[Flags]
		enum Flags : uint {
			None				= 0,
			NoMemoryUsage		= 0x00000001,
			NoRegisterUsage		= 0x00000002,
			Is64Bit				= 0x00000004,
			ZeroExtVecRegs		= 0x00000008,
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public InstructionInfoFactory() {
			usedRegisters = new SimpleList<UsedRegister>(new UsedRegister[10]);
			usedMemoryLocations = new SimpleList<UsedMemory>(new UsedMemory[8]);
		}

		/// <summary>
		/// Creates an <see cref="InstructionInfo"/>. The return value is only valid until this instance creates a new <see cref="InstructionInfo"/> value.
		/// </summary>
		/// <param name="instruction">Instruction</param>
		/// <returns></returns>
		public InstructionInfo GetInfo(ref Instruction instruction) =>
			Create(ref instruction, ref usedRegisters, ref usedMemoryLocations, InstructionInfoOptions.None);

		/// <summary>
		/// Creates an <see cref="InstructionInfo"/>. The return value is only valid until this instance creates a new <see cref="InstructionInfo"/> value.
		/// </summary>
		/// <param name="instruction">Instruction</param>
		/// <param name="options">Options</param>
		/// <returns></returns>
		public InstructionInfo GetInfo(ref Instruction instruction, InstructionInfoOptions options) =>
			Create(ref instruction, ref usedRegisters, ref usedMemoryLocations, options);

		internal static unsafe InstructionInfo Create(ref Instruction instruction, ref SimpleList<UsedRegister> usedRegisters, ref SimpleList<UsedMemory> usedMemoryLocations, InstructionInfoOptions options) {
			usedRegisters.ValidLength = 0;
			usedMemoryLocations.ValidLength = 0;

			var data = InfoHandlers.Data;
			var index = (uint)instruction.Code << 1;
			var flags1 = data[(int)index];
			var flags2 = data[(int)index + 1];

			if ((flags2 & (uint)InfoFlags2.AVX2_Check) != 0 && instruction.Op1Kind == OpKind.Register) {
				flags2 = (flags2 & ~((uint)InfoFlags2.CpuidFeatureMask << (int)InfoFlags2.CpuidFeatureShift)) |
					((uint)CpuidFeatureInternal.AVX2 << (int)InfoFlags2.CpuidFeatureShift);
			}

			var codeSize = instruction.CodeSize;
			Debug.Assert((uint)InstructionInfoOptions.NoMemoryUsage == (uint)Flags.NoMemoryUsage);
			Debug.Assert((uint)InstructionInfoOptions.NoRegisterUsage == (uint)Flags.NoRegisterUsage);
			var flags = (Flags)options & (Flags.NoMemoryUsage | Flags.NoRegisterUsage);
			if (codeSize == CodeSize.Code64 || codeSize == CodeSize.Unknown)
				flags |= Flags.Is64Bit;
			if ((flags2 & ((uint)InfoFlags2.EncodingMask << (int)InfoFlags2.EncodingShift)) != ((uint)EncodingKind.Legacy << (int)InfoFlags2.EncodingShift))
				flags |= Flags.ZeroExtVecRegs;

			OpAccess op0Access;
			switch ((OpInfo0)(flags2 & (uint)InfoFlags2.OpInfo0Mask)) {
			default:
			case OpInfo0.None:
				op0Access = OpAccess.None;
				break;

			case OpInfo0.Read:
				op0Access = OpAccess.Read;
				break;

			case OpInfo0.Write:
				if (instruction.HasOpMask && instruction.MergingMasking)
					op0Access = OpAccess.ReadWrite;
				else
					op0Access = OpAccess.Write;
				break;

			case OpInfo0.WriteForce:
				op0Access = OpAccess.Write;
				break;

			case OpInfo0.CondWrite:
				op0Access = OpAccess.CondWrite;
				break;

			case OpInfo0.CondWrite32_ReadWrite64:
				if ((flags & Flags.Is64Bit) != 0)
					op0Access = OpAccess.ReadWrite;
				else
					op0Access = OpAccess.CondWrite;
				break;

			case OpInfo0.ReadWrite:
				op0Access = OpAccess.ReadWrite;
				break;

			case OpInfo0.ReadCondWrite:
				op0Access = OpAccess.ReadCondWrite;
				break;

			case OpInfo0.NoMemAccess:
				op0Access = OpAccess.NoMemAccess;
				break;

			case OpInfo0.WriteMem_ReadWriteReg:
				if (instruction.Internal_Op0IsNotReg_or_Op0IsNotReg)
					op0Access = OpAccess.Write;
				else
					op0Access = OpAccess.ReadWrite;
				break;
			}

			Debug.Assert(instruction.OpCount <= DecoderConstants.MaxOpCount);
			var accesses = stackalloc OpAccess[DecoderConstants.MaxOpCount];
			accesses[0] = op0Access;
			var op1info = (OpInfo1)((flags2 >> (int)InfoFlags2.OpInfo1Shift) & (uint)InfoFlags2.OpInfo1Mask);
			accesses[1] = InfoHandlers.Op1Accesses[(int)op1info];
			accesses[2] = InfoHandlers.Op2Accesses[(int)((flags2 >> (int)InfoFlags2.OpInfo2Shift) & (uint)InfoFlags2.OpInfo2Mask)];
			if ((flags2 & (((uint)InfoFlags2.OpInfo3Mask) << (int)InfoFlags2.OpInfo3Shift)) != 0)
				accesses[3] = OpAccess.Read;
			if ((flags2 & (((uint)InfoFlags2.OpInfo4Mask) << (int)InfoFlags2.OpInfo4Shift)) != 0)
				accesses[4] = OpAccess.Read;
			Debug.Assert(DecoderConstants.MaxOpCount == 5);

			int opCount = instruction.OpCount;
			for (int i = 0; i < opCount; i++) {
				var access = accesses[i];
				if (access == OpAccess.None)
					continue;

				switch (instruction.GetOpKind(i)) {
				case OpKind.Register:
					if (access == OpAccess.NoMemAccess) {
						access = OpAccess.Read;
						accesses[i] = OpAccess.Read;
					}
					if ((flags & Flags.NoRegisterUsage) == 0) {
						if (i == 1 && op1info == OpInfo1.ReadP3) {
							var reg = instruction.Op1Register;
							Debug.Assert(Register.XMM0 <= reg && reg <= InstructionInfoConstants.VMM_last);
							reg = InstructionInfoConstants.VMM_first + ((reg - InstructionInfoConstants.VMM_first) & ~3);
							for (int j = 0; j < 4; j++)
								AddRegister(flags, ref usedRegisters, reg + j, access);
						}
						else
							AddRegister(flags, ref usedRegisters, instruction.GetOpRegister(i), access);
					}
					break;

				case OpKind.Memory64:
					if ((flags & Flags.NoMemoryUsage) == 0)
						AddMemory(ref usedMemoryLocations, instruction.MemorySegment, Register.None, Register.None, 1, instruction.MemoryAddress64, instruction.MemorySize, access);
					if ((flags & Flags.NoRegisterUsage) == 0)
						AddMemorySegmentRegister(flags, ref usedRegisters, instruction.MemorySegment, OpAccess.Read);
					break;

				case OpKind.Memory:
					Debug.Assert((uint)InfoFlags1.NoSegmentRead == (1U << 31));
					Debug.Assert(Register.None == 0);
					var segReg = (Register)((uint)instruction.MemorySegment & ~(uint)((int)flags1 >> 31));
					var baseReg = instruction.MemoryBase;
					if (baseReg == Register.RIP) {
						if ((flags & Flags.NoMemoryUsage) == 0)
							AddMemory(ref usedMemoryLocations, segReg, Register.None, Register.None, 1, instruction.NextIP + (ulong)(int)instruction.MemoryDisplacement, instruction.MemorySize, access);
						if ((flags & Flags.NoRegisterUsage) == 0 && segReg != Register.None)
							AddMemorySegmentRegister(flags, ref usedRegisters, segReg, OpAccess.Read);
					}
					else if (baseReg == Register.EIP) {
						if ((flags & Flags.NoMemoryUsage) == 0)
							AddMemory(ref usedMemoryLocations, segReg, Register.None, Register.None, 1, instruction.NextIP32 + instruction.MemoryDisplacement, instruction.MemorySize, access);
						if ((flags & Flags.NoRegisterUsage) == 0 && segReg != Register.None)
							AddMemorySegmentRegister(flags, ref usedRegisters, segReg, OpAccess.Read);
					}
					else {
						ulong displ;
						var indexReg = instruction.MemoryIndex;
						if (InstructionUtils.GetAddressSizeInBytes(baseReg, indexReg, instruction.MemoryDisplSize, codeSize) == 8)
							displ = (ulong)(int)instruction.MemoryDisplacement;
						else
							displ = instruction.MemoryDisplacement;
						if ((flags & Flags.NoMemoryUsage) == 0)
							AddMemory(ref usedMemoryLocations, segReg, baseReg, indexReg, instruction.MemoryIndexScale, displ, instruction.MemorySize, access);
						if ((flags & Flags.NoRegisterUsage) == 0) {
							if (segReg != Register.None)
								AddMemorySegmentRegister(flags, ref usedRegisters, segReg, OpAccess.Read);
							if (baseReg != Register.None)
								AddRegister(flags, ref usedRegisters, baseReg, OpAccess.Read);
							if (indexReg != Register.None)
								AddRegister(flags, ref usedRegisters, indexReg, OpAccess.Read);
						}
					}
					break;
				}
			}

			var rflagsInfo = (RflagsInfo)((flags1 >> (int)InfoFlags1.RflagsInfoShift) & (uint)InfoFlags1.RflagsInfoMask);
			var codeInfo = (CodeInfo)((flags1 >> (int)InfoFlags1.CodeInfoShift) & (uint)InfoFlags1.CodeInfoMask);
			if (codeInfo != CodeInfo.None)
				CodeInfoHandler(codeInfo, ref instruction, ref usedRegisters, ref usedMemoryLocations, ref rflagsInfo, flags, accesses);

			if (instruction.HasOpMask && (flags & Flags.NoRegisterUsage) == 0)
				AddRegister(flags, ref usedRegisters, instruction.OpMask, (flags2 & (uint)InfoFlags2.OpMaskRegReadWrite) != 0 ? OpAccess.ReadWrite : OpAccess.Read);

			// Inlined ctor
			InstructionInfo result;
			Debug.Assert(DecoderConstants.MaxOpCount == 5);
			Debug.Assert(usedRegisters.Array != null);
			result.usedRegisters = usedRegisters.Array;
			Debug.Assert(usedMemoryLocations.Array != null);
			result.usedMemoryLocations = usedMemoryLocations.Array;
			Debug.Assert((uint)usedRegisters.ValidLength <= ushort.MaxValue);
			result.usedRegistersLength = (ushort)usedRegisters.ValidLength;
			Debug.Assert((uint)usedMemoryLocations.ValidLength <= ushort.MaxValue);
			result.usedMemoryLocationsLength = (ushort)usedMemoryLocations.ValidLength;
			result.opMaskFlags = (ushort)((uint)accesses[0] |
				((uint)accesses[1] << (int)InstructionInfo.OpMaskFlags.Op1AccessShift) |
				((uint)accesses[2] << (int)InstructionInfo.OpMaskFlags.Op2AccessShift) |
				((uint)accesses[3] << (int)InstructionInfo.OpMaskFlags.Op3AccessShift) |
				((uint)accesses[4] << (int)InstructionInfo.OpMaskFlags.Op4AccessShift));
			Debug.Assert(((flags2 >> (int)InfoFlags2.CpuidFeatureShift) & (uint)InfoFlags2.CpuidFeatureMask) <= byte.MaxValue);
			result.cpuidFeature = (byte)((flags2 >> (int)InfoFlags2.CpuidFeatureShift) & (uint)InfoFlags2.CpuidFeatureMask);
			Debug.Assert(((flags2 >> (int)InfoFlags2.FlowControlShift) & (uint)InfoFlags2.FlowControlMask) <= byte.MaxValue);
			result.flowControl = (byte)((flags2 >> (int)InfoFlags2.FlowControlShift) & (uint)InfoFlags2.FlowControlMask);
			Debug.Assert(((flags2 >> (int)InfoFlags2.EncodingShift) & (uint)InfoFlags2.EncodingMask) <= byte.MaxValue);
			result.encoding = (byte)((flags2 >> (int)InfoFlags2.EncodingShift) & (uint)InfoFlags2.EncodingMask);
			Debug.Assert((uint)rflagsInfo <= byte.MaxValue);
			Debug.Assert((uint)rflagsInfo < (uint)RflagsInfo.Last);
			result.rflagsInfo = (byte)rflagsInfo;

			Debug.Assert((uint)InfoFlags1.SaveRestore == 0x08000000);
			Debug.Assert((uint)InfoFlags1.StackInstruction == 0x10000000);
			Debug.Assert((uint)InfoFlags1.ProtectedMode == 0x20000000);
			Debug.Assert((uint)InfoFlags1.Privileged == 0x40000000);
			Debug.Assert((uint)InstructionInfo.Flags.SaveRestore == 0x01);
			Debug.Assert((uint)InstructionInfo.Flags.StackInstruction == 0x02);
			Debug.Assert((uint)InstructionInfo.Flags.ProtectedMode == 0x04);
			Debug.Assert((uint)InstructionInfo.Flags.Privileged == 0x08);
			// Bit 4 could be set but we don't use it so we don't need to mask it out
			result.flags = (byte)(flags1 >> 27);
			return result;
		}

		static Register GetXSP(CodeSize codeSize, out ulong xspMask) {
			if (codeSize == CodeSize.Code64 || codeSize == CodeSize.Unknown) {
				xspMask = ulong.MaxValue;
				return Register.RSP;
			}
			if (codeSize == CodeSize.Code32) {
				xspMask = uint.MaxValue;
				return Register.ESP;
			}
			Debug.Assert(codeSize == CodeSize.Code16);
			xspMask = ushort.MaxValue;
			return Register.SP;
		}

		static unsafe void CodeInfoHandler(CodeInfo codeInfo, ref Instruction instruction, ref SimpleList<UsedRegister> usedRegisters, ref SimpleList<UsedMemory> usedMemoryLocations, ref RflagsInfo rflagsInfo, Flags flags, OpAccess* accesses) {
			Debug.Assert(codeInfo != CodeInfo.None);
			ulong xspMask;
			ulong displ;
			Register xsp;
			Register baseReg;
			MemorySize memSize;
			Code code;
			switch (codeInfo) {
			case CodeInfo.RW_AX:
				if ((flags & Flags.NoRegisterUsage) == 0)
					AddRegister(flags, ref usedRegisters, Register.AX, OpAccess.ReadWrite);
				break;

			case CodeInfo.RW_AL:
				if ((flags & Flags.NoRegisterUsage) == 0)
					AddRegister(flags, ref usedRegisters, Register.AL, OpAccess.ReadWrite);
				break;

			case CodeInfo.Salc:
				if ((flags & Flags.NoRegisterUsage) == 0)
					AddRegister(flags, ref usedRegisters, Register.AL, OpAccess.Write);
				break;

			case CodeInfo.R_AL_W_AH:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, ref usedRegisters, Register.AL, OpAccess.Read);
					AddRegister(flags, ref usedRegisters, Register.AH, OpAccess.Write);
				}
				break;

			case CodeInfo.R_AL_W_AX:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, ref usedRegisters, Register.AL, OpAccess.Read);
					AddRegister(flags, ref usedRegisters, Register.AX, OpAccess.Write);
				}
				break;

			case CodeInfo.Cwde:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, ref usedRegisters, Register.AX, OpAccess.Read);
					AddRegister(flags, ref usedRegisters, Register.EAX, OpAccess.Write);
				}
				break;

			case CodeInfo.Cdqe:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, ref usedRegisters, Register.EAX, OpAccess.Read);
					AddRegister(flags, ref usedRegisters, Register.RAX, OpAccess.Write);
				}
				break;

			case CodeInfo.Cwd:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, ref usedRegisters, Register.AX, OpAccess.Read);
					AddRegister(flags, ref usedRegisters, Register.DX, OpAccess.Write);
				}
				break;

			case CodeInfo.Cdq:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, ref usedRegisters, Register.EAX, OpAccess.Read);
					AddRegister(flags, ref usedRegisters, Register.EDX, OpAccess.Write);
				}
				break;

			case CodeInfo.Cqo:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, ref usedRegisters, Register.RAX, OpAccess.Read);
					AddRegister(flags, ref usedRegisters, Register.RDX, OpAccess.Write);
				}
				break;

			case CodeInfo.R_XMM0:
				if ((flags & Flags.NoRegisterUsage) == 0)
					AddRegister(flags, ref usedRegisters, Register.XMM0, OpAccess.Read);
				break;

			case CodeInfo.Push_2:
				xsp = GetXSP(instruction.CodeSize, out xspMask);
				if ((flags & Flags.NoRegisterUsage) == 0) {
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, ref usedRegisters, Register.SS, OpAccess.Read);
					AddRegister(flags, ref usedRegisters, xsp, OpAccess.ReadWrite);
				}
				if ((flags & Flags.NoMemoryUsage) == 0)
					AddMemory(ref usedMemoryLocations, Register.SS, xsp, Register.None, 1, 0xFFFF_FFFF_FFFF_FFFE & xspMask, MemorySize.UInt16, OpAccess.Write);
				break;

			case CodeInfo.Push_4:
				xsp = GetXSP(instruction.CodeSize, out xspMask);
				if ((flags & Flags.NoRegisterUsage) == 0) {
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, ref usedRegisters, Register.SS, OpAccess.Read);
					AddRegister(flags, ref usedRegisters, xsp, OpAccess.ReadWrite);
				}
				if ((flags & Flags.NoMemoryUsage) == 0)
					AddMemory(ref usedMemoryLocations, Register.SS, xsp, Register.None, 1, 0xFFFF_FFFF_FFFF_FFFC & xspMask, MemorySize.UInt32, OpAccess.Write);
				break;

			case CodeInfo.Push_8:
				xsp = GetXSP(instruction.CodeSize, out xspMask);
				if ((flags & Flags.NoRegisterUsage) == 0) {
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, ref usedRegisters, Register.SS, OpAccess.Read);
					AddRegister(flags, ref usedRegisters, xsp, OpAccess.ReadWrite);
				}
				if ((flags & Flags.NoMemoryUsage) == 0)
					AddMemory(ref usedMemoryLocations, Register.SS, xsp, Register.None, 1, 0xFFFF_FFFF_FFFF_FFF8 & xspMask, MemorySize.UInt64, OpAccess.Write);
				break;

			case CodeInfo.Push_2_2:
				xsp = GetXSP(instruction.CodeSize, out xspMask);
				if ((flags & Flags.NoRegisterUsage) == 0) {
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, ref usedRegisters, Register.SS, OpAccess.Read);
					AddRegister(flags, ref usedRegisters, xsp, OpAccess.ReadWrite);
				}
				if ((flags & Flags.NoMemoryUsage) == 0) {
					AddMemory(ref usedMemoryLocations, Register.SS, xsp, Register.None, 1, 0xFFFF_FFFF_FFFF_FFFE & xspMask, MemorySize.UInt16, OpAccess.Write);
					AddMemory(ref usedMemoryLocations, Register.SS, xsp, Register.None, 1, 0xFFFF_FFFF_FFFF_FFFC & xspMask, MemorySize.UInt16, OpAccess.Write);
				}
				break;

			case CodeInfo.Push_4_4:
				xsp = GetXSP(instruction.CodeSize, out xspMask);
				if ((flags & Flags.NoRegisterUsage) == 0) {
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, ref usedRegisters, Register.SS, OpAccess.Read);
					AddRegister(flags, ref usedRegisters, xsp, OpAccess.ReadWrite);
				}
				if ((flags & Flags.NoMemoryUsage) == 0) {
					AddMemory(ref usedMemoryLocations, Register.SS, xsp, Register.None, 1, 0xFFFF_FFFF_FFFF_FFFC & xspMask, MemorySize.UInt32, OpAccess.Write);
					AddMemory(ref usedMemoryLocations, Register.SS, xsp, Register.None, 1, 0xFFFF_FFFF_FFFF_FFF8 & xspMask, MemorySize.UInt32, OpAccess.Write);
				}
				break;

			case CodeInfo.Push_8_8:
				xsp = GetXSP(instruction.CodeSize, out xspMask);
				if ((flags & Flags.NoRegisterUsage) == 0) {
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, ref usedRegisters, Register.SS, OpAccess.Read);
					AddRegister(flags, ref usedRegisters, xsp, OpAccess.ReadWrite);
				}
				if ((flags & Flags.NoMemoryUsage) == 0) {
					AddMemory(ref usedMemoryLocations, Register.SS, xsp, Register.None, 1, 0xFFFF_FFFF_FFFF_FFF8 & xspMask, MemorySize.UInt64, OpAccess.Write);
					AddMemory(ref usedMemoryLocations, Register.SS, xsp, Register.None, 1, 0xFFFF_FFFF_FFFF_FFF0 & xspMask, MemorySize.UInt64, OpAccess.Write);
				}
				break;

			case CodeInfo.Pop_2:
				xsp = GetXSP(instruction.CodeSize, out xspMask);
				if ((flags & Flags.NoRegisterUsage) == 0) {
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, ref usedRegisters, Register.SS, OpAccess.Read);
					AddRegister(flags, ref usedRegisters, xsp, OpAccess.ReadWrite);
				}
				if ((flags & Flags.NoMemoryUsage) == 0)
					AddMemory(ref usedMemoryLocations, Register.SS, xsp, Register.None, 1, 0, MemorySize.UInt16, OpAccess.Read);
				break;

			case CodeInfo.Pop_4:
				xsp = GetXSP(instruction.CodeSize, out xspMask);
				if ((flags & Flags.NoRegisterUsage) == 0) {
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, ref usedRegisters, Register.SS, OpAccess.Read);
					AddRegister(flags, ref usedRegisters, xsp, OpAccess.ReadWrite);
				}
				if ((flags & Flags.NoMemoryUsage) == 0)
					AddMemory(ref usedMemoryLocations, Register.SS, xsp, Register.None, 1, 0, MemorySize.UInt32, OpAccess.Read);
				break;

			case CodeInfo.Pop_8:
				xsp = GetXSP(instruction.CodeSize, out xspMask);
				if ((flags & Flags.NoRegisterUsage) == 0) {
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, ref usedRegisters, Register.SS, OpAccess.Read);
					AddRegister(flags, ref usedRegisters, xsp, OpAccess.ReadWrite);
				}
				if ((flags & Flags.NoMemoryUsage) == 0)
					AddMemory(ref usedMemoryLocations, Register.SS, xsp, Register.None, 1, 0, MemorySize.UInt64, OpAccess.Read);
				break;

			case CodeInfo.Pop_2_2:
				xsp = GetXSP(instruction.CodeSize, out xspMask);
				if ((flags & Flags.NoRegisterUsage) == 0) {
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, ref usedRegisters, Register.SS, OpAccess.Read);
					AddRegister(flags, ref usedRegisters, xsp, OpAccess.ReadWrite);
				}
				if ((flags & Flags.NoMemoryUsage) == 0) {
					AddMemory(ref usedMemoryLocations, Register.SS, xsp, Register.None, 1, 0, MemorySize.UInt16, OpAccess.Read);
					AddMemory(ref usedMemoryLocations, Register.SS, xsp, Register.None, 1, 2, MemorySize.UInt16, OpAccess.Read);
				}
				break;

			case CodeInfo.Pop_4_4:
				xsp = GetXSP(instruction.CodeSize, out xspMask);
				if ((flags & Flags.NoRegisterUsage) == 0) {
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, ref usedRegisters, Register.SS, OpAccess.Read);
					AddRegister(flags, ref usedRegisters, xsp, OpAccess.ReadWrite);
				}
				if ((flags & Flags.NoMemoryUsage) == 0) {
					AddMemory(ref usedMemoryLocations, Register.SS, xsp, Register.None, 1, 0, MemorySize.UInt32, OpAccess.Read);
					AddMemory(ref usedMemoryLocations, Register.SS, xsp, Register.None, 1, 4, MemorySize.UInt32, OpAccess.Read);
				}
				break;

			case CodeInfo.Pop_8_8:
				xsp = GetXSP(instruction.CodeSize, out xspMask);
				if ((flags & Flags.NoRegisterUsage) == 0) {
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, ref usedRegisters, Register.SS, OpAccess.Read);
					AddRegister(flags, ref usedRegisters, xsp, OpAccess.ReadWrite);
				}
				if ((flags & Flags.NoMemoryUsage) == 0) {
					AddMemory(ref usedMemoryLocations, Register.SS, xsp, Register.None, 1, 0, MemorySize.UInt64, OpAccess.Read);
					AddMemory(ref usedMemoryLocations, Register.SS, xsp, Register.None, 1, 8, MemorySize.UInt64, OpAccess.Read);
				}
				break;

			case CodeInfo.Pop_Ev:
				xsp = GetXSP(instruction.CodeSize, out xspMask);
				if ((flags & Flags.NoRegisterUsage) == 0) {
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, ref usedRegisters, Register.SS, OpAccess.Read);
					AddRegister(flags, ref usedRegisters, xsp, OpAccess.ReadWrite);
				}
				if ((flags & Flags.NoMemoryUsage) == 0) {
					code = instruction.Code;
					uint size;
					if (code == Code.Pop_rm64) {
						memSize = MemorySize.UInt64;
						size = 8;
					}
					else if (code == Code.Pop_rm32) {
						memSize = MemorySize.UInt32;
						size = 4;
					}
					else {
						Debug.Assert(instruction.Code == Code.Pop_rm16);
						memSize = MemorySize.UInt16;
						size = 2;
					}
					if (instruction.Op0Kind == OpKind.Memory) {
						Debug.Assert(usedMemoryLocations.ValidLength == 1);
						if (instruction.MemoryBase == Register.RSP || instruction.MemoryBase == Register.ESP) {
							ref var mem = ref usedMemoryLocations.Array[0];
							displ = mem.Displacement + size;
							if (instruction.MemoryBase == Register.ESP)
								displ = (uint)displ;
							usedMemoryLocations.Array[0] = new UsedMemory(mem.Segment, mem.Base, mem.Index, mem.Scale, displ, mem.MemorySize, mem.Access);
						}
					}
					AddMemory(ref usedMemoryLocations, Register.SS, xsp, Register.None, 1, 0, memSize, OpAccess.Read);
				}
				break;

			case CodeInfo.Pusha:
				xsp = GetXSP(instruction.CodeSize, out xspMask);
				if ((flags & Flags.NoRegisterUsage) == 0) {
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, ref usedRegisters, Register.SS, OpAccess.Read);
					AddRegister(flags, ref usedRegisters, xsp, OpAccess.ReadWrite);
				}
				if (instruction.Code == Code.Pushad) {
					displ = 0xFFFF_FFFF_FFFF_FFFC;
					memSize = MemorySize.UInt32;
					baseReg = Register.EAX;
				}
				else {
					Debug.Assert(instruction.Code == Code.Pushaw);
					displ = 0xFFFF_FFFF_FFFF_FFFE;
					memSize = MemorySize.UInt16;
					baseReg = Register.AX;
				}
				for (int i = 0; i < 8; i++) {
					if ((flags & Flags.NoRegisterUsage) == 0)
						AddRegister(flags, ref usedRegisters, baseReg + i, OpAccess.Read);
					if ((flags & Flags.NoMemoryUsage) == 0)
						AddMemory(ref usedMemoryLocations, Register.SS, xsp, Register.None, 1, (ulong)((long)displ * (i + 1)) & xspMask, memSize, OpAccess.Write);
				}
				break;

			case CodeInfo.Popa:
				xsp = GetXSP(instruction.CodeSize, out xspMask);
				if ((flags & Flags.NoRegisterUsage) == 0) {
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, ref usedRegisters, Register.SS, OpAccess.Read);
					AddRegister(flags, ref usedRegisters, xsp, OpAccess.ReadWrite);
				}
				if (instruction.Code == Code.Popad) {
					displ = 4;
					memSize = MemorySize.UInt32;
					baseReg = Register.EAX;
				}
				else {
					Debug.Assert(instruction.Code == Code.Popaw);
					displ = 2;
					memSize = MemorySize.UInt16;
					baseReg = Register.AX;
				}
				for (int i = 0; i < 8; i++) {
					// Ignore eSP
					if (i != 3) {
						if ((flags & Flags.NoRegisterUsage) == 0)
							AddRegister(flags, ref usedRegisters, baseReg + 7 - i, OpAccess.Write);
						if ((flags & Flags.NoMemoryUsage) == 0)
							AddMemory(ref usedMemoryLocations, Register.SS, xsp, Register.None, 1, displ * (uint)i & xspMask, memSize, OpAccess.Read);
					}
				}
				break;

			case CodeInfo.Ins:
				if (instruction.Internal_HasRepeOrRepnePrefix) {
					accesses[0] = OpAccess.CondWrite;
					accesses[1] = OpAccess.CondRead;
					Debug.Assert(OpKind.MemoryESDI + 1 == OpKind.MemoryESEDI);
					Debug.Assert(OpKind.MemoryESDI + 2 == OpKind.MemoryESRDI);
					Debug.Assert(Register.DI + 16 == Register.EDI);
					Debug.Assert(Register.DI + 32 == Register.RDI);
					Debug.Assert(Register.CX + 16 == Register.ECX);
					Debug.Assert(Register.CX + 32 == Register.RCX);
					baseReg = ((instruction.Op0Kind - OpKind.MemoryESDI) << 4) + Register.DI;
					if ((flags & Flags.NoMemoryUsage) == 0)
						AddMemory(ref usedMemoryLocations, Register.ES, baseReg, Register.None, 1, 0, MemorySize.Unknown, OpAccess.CondWrite);
					if ((flags & Flags.NoRegisterUsage) == 0) {
						Debug.Assert(usedRegisters.ValidLength == 1);
						usedRegisters.Array[0] = new UsedRegister(Register.DX, OpAccess.CondRead);
						AddRegister(flags, ref usedRegisters, ((instruction.Op0Kind - OpKind.MemoryESDI) << 4) + Register.CX, OpAccess.ReadCondWrite);
						if ((flags & Flags.Is64Bit) == 0)
							AddRegister(flags, ref usedRegisters, Register.ES, OpAccess.CondRead);
						AddRegister(flags, ref usedRegisters, baseReg, OpAccess.CondRead);
						AddRegister(flags, ref usedRegisters, baseReg, OpAccess.CondWrite);
					}
				}
				else {
					Debug.Assert(OpKind.MemoryESDI + 1 == OpKind.MemoryESEDI);
					Debug.Assert(OpKind.MemoryESDI + 2 == OpKind.MemoryESRDI);
					Debug.Assert(Register.DI + 16 == Register.EDI);
					Debug.Assert(Register.DI + 32 == Register.RDI);
					baseReg = ((instruction.Op0Kind - OpKind.MemoryESDI) << 4) + Register.DI;
					if ((flags & Flags.NoMemoryUsage) == 0)
						AddMemory(ref usedMemoryLocations, Register.ES, baseReg, Register.None, 1, 0, instruction.MemorySize, OpAccess.Write);
					if ((flags & Flags.NoRegisterUsage) == 0) {
						if ((flags & Flags.Is64Bit) == 0)
							AddRegister(flags, ref usedRegisters, Register.ES, OpAccess.Read);
						AddRegister(flags, ref usedRegisters, baseReg, OpAccess.ReadWrite);
					}
				}
				break;

			case CodeInfo.Outs:
				if (instruction.Internal_HasRepeOrRepnePrefix) {
					accesses[0] = OpAccess.CondRead;
					accesses[1] = OpAccess.CondRead;
					Debug.Assert(OpKind.MemorySegSI + 1 == OpKind.MemorySegESI);
					Debug.Assert(OpKind.MemorySegSI + 2 == OpKind.MemorySegRSI);
					Debug.Assert(Register.SI + 16 == Register.ESI);
					Debug.Assert(Register.SI + 32 == Register.RSI);
					Debug.Assert(Register.CX + 16 == Register.ECX);
					Debug.Assert(Register.CX + 32 == Register.RCX);
					baseReg = ((instruction.Op1Kind - OpKind.MemorySegSI) << 4) + Register.SI;
					if ((flags & Flags.NoMemoryUsage) == 0)
						AddMemory(ref usedMemoryLocations, instruction.MemorySegment, baseReg, Register.None, 1, 0, MemorySize.Unknown, OpAccess.CondRead);
					if ((flags & Flags.NoRegisterUsage) == 0) {
						Debug.Assert(usedRegisters.ValidLength == 1);
						usedRegisters.Array[0] = new UsedRegister(Register.DX, OpAccess.CondRead);
						AddRegister(flags, ref usedRegisters, ((instruction.Op1Kind - OpKind.MemorySegSI) << 4) + Register.CX, OpAccess.ReadCondWrite);
						AddMemorySegmentRegister(flags, ref usedRegisters, instruction.MemorySegment, OpAccess.CondRead);
						AddRegister(flags, ref usedRegisters, baseReg, OpAccess.CondRead);
						AddRegister(flags, ref usedRegisters, baseReg, OpAccess.CondWrite);
					}
				}
				else {
					Debug.Assert(OpKind.MemorySegSI + 1 == OpKind.MemorySegESI);
					Debug.Assert(OpKind.MemorySegSI + 2 == OpKind.MemorySegRSI);
					Debug.Assert(Register.SI + 16 == Register.ESI);
					Debug.Assert(Register.SI + 32 == Register.RSI);
					baseReg = ((instruction.Op1Kind - OpKind.MemorySegSI) << 4) + Register.SI;
					if ((flags & Flags.NoMemoryUsage) == 0)
						AddMemory(ref usedMemoryLocations, instruction.MemorySegment, baseReg, Register.None, 1, 0, instruction.MemorySize, OpAccess.Read);
					if ((flags & Flags.NoRegisterUsage) == 0) {
						AddMemorySegmentRegister(flags, ref usedRegisters, instruction.MemorySegment, OpAccess.Read);
						AddRegister(flags, ref usedRegisters, baseReg, OpAccess.ReadWrite);
					}
				}
				break;

			case CodeInfo.Movs:
				if (instruction.Internal_HasRepeOrRepnePrefix) {
					accesses[0] = OpAccess.CondWrite;
					accesses[1] = OpAccess.CondRead;
					Debug.Assert(OpKind.MemoryESDI + 1 == OpKind.MemoryESEDI);
					Debug.Assert(OpKind.MemoryESDI + 2 == OpKind.MemoryESRDI);
					Debug.Assert(Register.DI + 16 == Register.EDI);
					Debug.Assert(Register.DI + 32 == Register.RDI);
					Debug.Assert(Register.CX + 16 == Register.ECX);
					Debug.Assert(Register.CX + 32 == Register.RCX);
					baseReg = ((instruction.Op0Kind - OpKind.MemoryESDI) << 4) + Register.DI;
					if ((flags & Flags.NoMemoryUsage) == 0)
						AddMemory(ref usedMemoryLocations, Register.ES, baseReg, Register.None, 1, 0, MemorySize.Unknown, OpAccess.CondWrite);
					if ((flags & Flags.NoRegisterUsage) == 0) {
						AddRegister(flags, ref usedRegisters, ((instruction.Op0Kind - OpKind.MemoryESDI) << 4) + Register.CX, OpAccess.ReadCondWrite);
						if ((flags & Flags.Is64Bit) == 0)
							AddRegister(flags, ref usedRegisters, Register.ES, OpAccess.CondRead);
						AddRegister(flags, ref usedRegisters, baseReg, OpAccess.CondRead);
						AddRegister(flags, ref usedRegisters, baseReg, OpAccess.CondWrite);
					}
					Debug.Assert(OpKind.MemorySegSI + 1 == OpKind.MemorySegESI);
					Debug.Assert(OpKind.MemorySegSI + 2 == OpKind.MemorySegRSI);
					Debug.Assert(Register.SI + 16 == Register.ESI);
					Debug.Assert(Register.SI + 32 == Register.RSI);
					baseReg = ((instruction.Op1Kind - OpKind.MemorySegSI) << 4) + Register.SI;
					if ((flags & Flags.NoMemoryUsage) == 0)
						AddMemory(ref usedMemoryLocations, instruction.MemorySegment, baseReg, Register.None, 1, 0, MemorySize.Unknown, OpAccess.CondRead);
					if ((flags & Flags.NoRegisterUsage) == 0) {
						AddMemorySegmentRegister(flags, ref usedRegisters, instruction.MemorySegment, OpAccess.CondRead);
						AddRegister(flags, ref usedRegisters, baseReg, OpAccess.CondRead);
						AddRegister(flags, ref usedRegisters, baseReg, OpAccess.CondWrite);
					}
				}
				else {
					Debug.Assert(OpKind.MemoryESDI + 1 == OpKind.MemoryESEDI);
					Debug.Assert(OpKind.MemoryESDI + 2 == OpKind.MemoryESRDI);
					Debug.Assert(Register.DI + 16 == Register.EDI);
					Debug.Assert(Register.DI + 32 == Register.RDI);
					baseReg = ((instruction.Op0Kind - OpKind.MemoryESDI) << 4) + Register.DI;
					if ((flags & Flags.NoMemoryUsage) == 0)
						AddMemory(ref usedMemoryLocations, Register.ES, baseReg, Register.None, 1, 0, instruction.MemorySize, OpAccess.Write);
					if ((flags & Flags.NoRegisterUsage) == 0) {
						if ((flags & Flags.Is64Bit) == 0)
							AddRegister(flags, ref usedRegisters, Register.ES, OpAccess.Read);
						AddRegister(flags, ref usedRegisters, baseReg, OpAccess.ReadWrite);
					}
					Debug.Assert(OpKind.MemorySegSI + 1 == OpKind.MemorySegESI);
					Debug.Assert(OpKind.MemorySegSI + 2 == OpKind.MemorySegRSI);
					Debug.Assert(Register.SI + 16 == Register.ESI);
					Debug.Assert(Register.SI + 32 == Register.RSI);
					baseReg = ((instruction.Op1Kind - OpKind.MemorySegSI) << 4) + Register.SI;
					if ((flags & Flags.NoMemoryUsage) == 0)
						AddMemory(ref usedMemoryLocations, instruction.MemorySegment, baseReg, Register.None, 1, 0, instruction.MemorySize, OpAccess.Read);
					if ((flags & Flags.NoRegisterUsage) == 0) {
						AddMemorySegmentRegister(flags, ref usedRegisters, instruction.MemorySegment, OpAccess.Read);
						AddRegister(flags, ref usedRegisters, baseReg, OpAccess.ReadWrite);
					}
				}
				break;

			case CodeInfo.Cmps:
				if (instruction.Internal_HasRepeOrRepnePrefix) {
					accesses[0] = OpAccess.CondRead;
					accesses[1] = OpAccess.CondRead;
					Debug.Assert(OpKind.MemorySegSI + 1 == OpKind.MemorySegESI);
					Debug.Assert(OpKind.MemorySegSI + 2 == OpKind.MemorySegRSI);
					Debug.Assert(Register.SI + 16 == Register.ESI);
					Debug.Assert(Register.SI + 32 == Register.RSI);
					Debug.Assert(Register.CX + 16 == Register.ECX);
					Debug.Assert(Register.CX + 32 == Register.RCX);
					baseReg = ((instruction.Op0Kind - OpKind.MemorySegSI) << 4) + Register.SI;
					if ((flags & Flags.NoMemoryUsage) == 0)
						AddMemory(ref usedMemoryLocations, instruction.MemorySegment, baseReg, Register.None, 1, 0, MemorySize.Unknown, OpAccess.CondRead);
					if ((flags & Flags.NoRegisterUsage) == 0) {
						AddRegister(flags, ref usedRegisters, ((instruction.Op0Kind - OpKind.MemorySegSI) << 4) + Register.CX, OpAccess.ReadCondWrite);
						AddMemorySegmentRegister(flags, ref usedRegisters, instruction.MemorySegment, OpAccess.CondRead);
						AddRegister(flags, ref usedRegisters, baseReg, OpAccess.CondRead);
						AddRegister(flags, ref usedRegisters, baseReg, OpAccess.CondWrite);
					}
					Debug.Assert(OpKind.MemoryESDI + 1 == OpKind.MemoryESEDI);
					Debug.Assert(OpKind.MemoryESDI + 2 == OpKind.MemoryESRDI);
					Debug.Assert(Register.DI + 16 == Register.EDI);
					Debug.Assert(Register.DI + 32 == Register.RDI);
					baseReg = ((instruction.Op1Kind - OpKind.MemoryESDI) << 4) + Register.DI;
					if ((flags & Flags.NoMemoryUsage) == 0)
						AddMemory(ref usedMemoryLocations, Register.ES, baseReg, Register.None, 1, 0, MemorySize.Unknown, OpAccess.CondRead);
					if ((flags & Flags.NoRegisterUsage) == 0) {
						if ((flags & Flags.Is64Bit) == 0)
							AddRegister(flags, ref usedRegisters, Register.ES, OpAccess.CondRead);
						AddRegister(flags, ref usedRegisters, baseReg, OpAccess.CondRead);
						AddRegister(flags, ref usedRegisters, baseReg, OpAccess.CondWrite);
					}
				}
				else {
					Debug.Assert(OpKind.MemorySegSI + 1 == OpKind.MemorySegESI);
					Debug.Assert(OpKind.MemorySegSI + 2 == OpKind.MemorySegRSI);
					Debug.Assert(Register.SI + 16 == Register.ESI);
					Debug.Assert(Register.SI + 32 == Register.RSI);
					baseReg = ((instruction.Op0Kind - OpKind.MemorySegSI) << 4) + Register.SI;
					if ((flags & Flags.NoMemoryUsage) == 0)
						AddMemory(ref usedMemoryLocations, instruction.MemorySegment, baseReg, Register.None, 1, 0, instruction.MemorySize, OpAccess.Read);
					if ((flags & Flags.NoRegisterUsage) == 0) {
						AddMemorySegmentRegister(flags, ref usedRegisters, instruction.MemorySegment, OpAccess.Read);
						AddRegister(flags, ref usedRegisters, baseReg, OpAccess.ReadWrite);
					}
					Debug.Assert(OpKind.MemoryESDI + 1 == OpKind.MemoryESEDI);
					Debug.Assert(OpKind.MemoryESDI + 2 == OpKind.MemoryESRDI);
					Debug.Assert(Register.DI + 16 == Register.EDI);
					Debug.Assert(Register.DI + 32 == Register.RDI);
					baseReg = ((instruction.Op1Kind - OpKind.MemoryESDI) << 4) + Register.DI;
					if ((flags & Flags.NoMemoryUsage) == 0)
						AddMemory(ref usedMemoryLocations, Register.ES, baseReg, Register.None, 1, 0, instruction.MemorySize, OpAccess.Read);
					if ((flags & Flags.NoRegisterUsage) == 0) {
						if ((flags & Flags.Is64Bit) == 0)
							AddRegister(flags, ref usedRegisters, Register.ES, OpAccess.Read);
						AddRegister(flags, ref usedRegisters, baseReg, OpAccess.ReadWrite);
					}
				}
				break;

			case CodeInfo.Stos:
				if (instruction.Internal_HasRepeOrRepnePrefix) {
					accesses[0] = OpAccess.CondWrite;
					accesses[1] = OpAccess.CondRead;
					Debug.Assert(OpKind.MemoryESDI + 1 == OpKind.MemoryESEDI);
					Debug.Assert(OpKind.MemoryESDI + 2 == OpKind.MemoryESRDI);
					Debug.Assert(Register.DI + 16 == Register.EDI);
					Debug.Assert(Register.DI + 32 == Register.RDI);
					Debug.Assert(Register.CX + 16 == Register.ECX);
					Debug.Assert(Register.CX + 32 == Register.RCX);
					baseReg = ((instruction.Op0Kind - OpKind.MemoryESDI) << 4) + Register.DI;
					if ((flags & Flags.NoMemoryUsage) == 0)
						AddMemory(ref usedMemoryLocations, Register.ES, baseReg, Register.None, 1, 0, MemorySize.Unknown, OpAccess.CondWrite);
					if ((flags & Flags.NoRegisterUsage) == 0) {
						Debug.Assert(usedRegisters.ValidLength == 1);
						usedRegisters.Array[0] = new UsedRegister(usedRegisters.Array[0].Register, OpAccess.CondRead);
						AddRegister(flags, ref usedRegisters, ((instruction.Op0Kind - OpKind.MemoryESDI) << 4) + Register.CX, OpAccess.ReadCondWrite);
						if ((flags & Flags.Is64Bit) == 0)
							AddRegister(flags, ref usedRegisters, Register.ES, OpAccess.CondRead);
						AddRegister(flags, ref usedRegisters, baseReg, OpAccess.CondRead);
						AddRegister(flags, ref usedRegisters, baseReg, OpAccess.CondWrite);
					}
				}
				else {
					Debug.Assert(OpKind.MemoryESDI + 1 == OpKind.MemoryESEDI);
					Debug.Assert(OpKind.MemoryESDI + 2 == OpKind.MemoryESRDI);
					Debug.Assert(Register.DI + 16 == Register.EDI);
					Debug.Assert(Register.DI + 32 == Register.RDI);
					baseReg = ((instruction.Op0Kind - OpKind.MemoryESDI) << 4) + Register.DI;
					if ((flags & Flags.NoMemoryUsage) == 0)
						AddMemory(ref usedMemoryLocations, Register.ES, baseReg, Register.None, 1, 0, instruction.MemorySize, OpAccess.Write);
					if ((flags & Flags.NoRegisterUsage) == 0) {
						if ((flags & Flags.Is64Bit) == 0)
							AddRegister(flags, ref usedRegisters, Register.ES, OpAccess.Read);
						AddRegister(flags, ref usedRegisters, baseReg, OpAccess.ReadWrite);
					}
				}
				break;

			case CodeInfo.Lods:
				if (instruction.Internal_HasRepeOrRepnePrefix) {
					accesses[0] = OpAccess.CondWrite;
					accesses[1] = OpAccess.CondRead;
					Debug.Assert(OpKind.MemorySegSI + 1 == OpKind.MemorySegESI);
					Debug.Assert(OpKind.MemorySegSI + 2 == OpKind.MemorySegRSI);
					Debug.Assert(Register.SI + 16 == Register.ESI);
					Debug.Assert(Register.SI + 32 == Register.RSI);
					Debug.Assert(Register.CX + 16 == Register.ECX);
					Debug.Assert(Register.CX + 32 == Register.RCX);
					baseReg = ((instruction.Op1Kind - OpKind.MemorySegSI) << 4) + Register.SI;
					if ((flags & Flags.NoMemoryUsage) == 0)
						AddMemory(ref usedMemoryLocations, instruction.MemorySegment, baseReg, Register.None, 1, 0, MemorySize.Unknown, OpAccess.CondRead);
					if ((flags & Flags.NoRegisterUsage) == 0) {
						Debug.Assert(usedRegisters.ValidLength == 1);
						usedRegisters.Array[0] = new UsedRegister(usedRegisters.Array[0].Register, OpAccess.CondWrite);
						AddRegister(flags, ref usedRegisters, ((instruction.Op1Kind - OpKind.MemorySegSI) << 4) + Register.CX, OpAccess.ReadCondWrite);
						AddMemorySegmentRegister(flags, ref usedRegisters, instruction.MemorySegment, OpAccess.CondRead);
						AddRegister(flags, ref usedRegisters, baseReg, OpAccess.CondRead);
						AddRegister(flags, ref usedRegisters, baseReg, OpAccess.CondWrite);
					}
				}
				else {
					Debug.Assert(OpKind.MemorySegSI + 1 == OpKind.MemorySegESI);
					Debug.Assert(OpKind.MemorySegSI + 2 == OpKind.MemorySegRSI);
					Debug.Assert(Register.SI + 16 == Register.ESI);
					Debug.Assert(Register.SI + 32 == Register.RSI);
					baseReg = ((instruction.Op1Kind - OpKind.MemorySegSI) << 4) + Register.SI;
					if ((flags & Flags.NoMemoryUsage) == 0)
						AddMemory(ref usedMemoryLocations, instruction.MemorySegment, baseReg, Register.None, 1, 0, instruction.MemorySize, OpAccess.Read);
					if ((flags & Flags.NoRegisterUsage) == 0) {
						AddMemorySegmentRegister(flags, ref usedRegisters, instruction.MemorySegment, OpAccess.Read);
						AddRegister(flags, ref usedRegisters, baseReg, OpAccess.ReadWrite);
					}
				}
				break;

			case CodeInfo.Scas:
				if (instruction.Internal_HasRepeOrRepnePrefix) {
					accesses[0] = OpAccess.CondRead;
					accesses[1] = OpAccess.CondRead;
					Debug.Assert(OpKind.MemoryESDI + 1 == OpKind.MemoryESEDI);
					Debug.Assert(OpKind.MemoryESDI + 2 == OpKind.MemoryESRDI);
					Debug.Assert(Register.DI + 16 == Register.EDI);
					Debug.Assert(Register.DI + 32 == Register.RDI);
					Debug.Assert(Register.CX + 16 == Register.ECX);
					Debug.Assert(Register.CX + 32 == Register.RCX);
					baseReg = ((instruction.Op1Kind - OpKind.MemoryESDI) << 4) + Register.DI;
					if ((flags & Flags.NoMemoryUsage) == 0)
						AddMemory(ref usedMemoryLocations, Register.ES, baseReg, Register.None, 1, 0, MemorySize.Unknown, OpAccess.CondRead);
					if ((flags & Flags.NoRegisterUsage) == 0) {
						Debug.Assert(usedRegisters.ValidLength == 1);
						usedRegisters.Array[0] = new UsedRegister(usedRegisters.Array[0].Register, OpAccess.CondRead);
						AddRegister(flags, ref usedRegisters, ((instruction.Op1Kind - OpKind.MemoryESDI) << 4) + Register.CX, OpAccess.ReadCondWrite);
						if ((flags & Flags.Is64Bit) == 0)
							AddRegister(flags, ref usedRegisters, Register.ES, OpAccess.CondRead);
						AddRegister(flags, ref usedRegisters, baseReg, OpAccess.CondRead);
						AddRegister(flags, ref usedRegisters, baseReg, OpAccess.CondWrite);
					}
				}
				else {
					Debug.Assert(OpKind.MemoryESDI + 1 == OpKind.MemoryESEDI);
					Debug.Assert(OpKind.MemoryESDI + 2 == OpKind.MemoryESRDI);
					Debug.Assert(Register.DI + 16 == Register.EDI);
					Debug.Assert(Register.DI + 32 == Register.RDI);
					baseReg = ((instruction.Op1Kind - OpKind.MemoryESDI) << 4) + Register.DI;
					if ((flags & Flags.NoMemoryUsage) == 0)
						AddMemory(ref usedMemoryLocations, Register.ES, baseReg, Register.None, 1, 0, instruction.MemorySize, OpAccess.Read);
					if ((flags & Flags.NoRegisterUsage) == 0) {
						if ((flags & Flags.Is64Bit) == 0)
							AddRegister(flags, ref usedRegisters, Register.ES, OpAccess.Read);
						AddRegister(flags, ref usedRegisters, baseReg, OpAccess.ReadWrite);
					}
				}
				break;

			case CodeInfo.Cmpxchg:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					code = instruction.Code;
					if (code == Code.Cmpxchg_rm64_r64)
						AddRegister(flags, ref usedRegisters, Register.RAX, OpAccess.ReadCondWrite);
					else if (code == Code.Cmpxchg_rm32_r32 || code == Code.Cmpxchg486_rm32_r32)
						AddRegister(flags, ref usedRegisters, Register.EAX, OpAccess.ReadCondWrite);
					else if (code == Code.Cmpxchg_rm16_r16 || code == Code.Cmpxchg486_rm16_r16)
						AddRegister(flags, ref usedRegisters, Register.AX, OpAccess.ReadCondWrite);
					else {
						Debug.Assert(code == Code.Cmpxchg_rm8_r8 || code == Code.Cmpxchg486_rm8_r8);
						AddRegister(flags, ref usedRegisters, Register.AL, OpAccess.ReadCondWrite);
					}
				}
				break;

			case CodeInfo.Cmpxchg8b:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					if (instruction.Code == Code.Cmpxchg16b_m128) {
						AddRegister(flags, ref usedRegisters, Register.RDX, OpAccess.ReadCondWrite);
						AddRegister(flags, ref usedRegisters, Register.RAX, OpAccess.ReadCondWrite);
						AddRegister(flags, ref usedRegisters, Register.RCX, OpAccess.CondRead);
						AddRegister(flags, ref usedRegisters, Register.RBX, OpAccess.CondRead);
					}
					else {
						Debug.Assert(instruction.Code == Code.Cmpxchg8b_m64);
						AddRegister(flags, ref usedRegisters, Register.EDX, OpAccess.ReadCondWrite);
						AddRegister(flags, ref usedRegisters, Register.EAX, OpAccess.ReadCondWrite);
						AddRegister(flags, ref usedRegisters, Register.ECX, OpAccess.CondRead);
						AddRegister(flags, ref usedRegisters, Register.EBX, OpAccess.CondRead);
					}
				}
				break;

			case CodeInfo.Cpuid:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, ref usedRegisters, Register.EAX, OpAccess.ReadWrite);
					AddRegister(flags, ref usedRegisters, Register.ECX, OpAccess.CondRead);
					AddRegister(flags, ref usedRegisters, Register.ECX, OpAccess.Write);
					AddRegister(flags, ref usedRegisters, Register.EDX, OpAccess.Write);
					AddRegister(flags, ref usedRegisters, Register.EBX, OpAccess.Write);
				}
				break;

			case CodeInfo.Div:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					code = instruction.Code;
					if (code == Code.Idiv_rm64 || code == Code.Div_rm64) {
						AddRegister(flags, ref usedRegisters, Register.RDX, OpAccess.ReadWrite);
						AddRegister(flags, ref usedRegisters, Register.RAX, OpAccess.ReadWrite);
					}
					else if (code == Code.Idiv_rm32 || code == Code.Div_rm32) {
						AddRegister(flags, ref usedRegisters, Register.EDX, OpAccess.ReadWrite);
						AddRegister(flags, ref usedRegisters, Register.EAX, OpAccess.ReadWrite);
					}
					else if (code == Code.Idiv_rm16 || code == Code.Div_rm16) {
						AddRegister(flags, ref usedRegisters, Register.DX, OpAccess.ReadWrite);
						AddRegister(flags, ref usedRegisters, Register.AX, OpAccess.ReadWrite);
					}
					else {
						Debug.Assert(code == Code.Idiv_rm8 || code == Code.Div_rm8);
						AddRegister(flags, ref usedRegisters, Register.AX, OpAccess.ReadWrite);
					}
				}
				break;

			case CodeInfo.Mul:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					code = instruction.Code;
					if (code == Code.Imul_rm64 || code == Code.Mul_rm64) {
						AddRegister(flags, ref usedRegisters, Register.RAX, OpAccess.ReadWrite);
						AddRegister(flags, ref usedRegisters, Register.RDX, OpAccess.Write);
					}
					else if (code == Code.Imul_rm32 || code == Code.Mul_rm32) {
						AddRegister(flags, ref usedRegisters, Register.EAX, OpAccess.ReadWrite);
						AddRegister(flags, ref usedRegisters, Register.EDX, OpAccess.Write);
					}
					else if (code == Code.Imul_rm16 || code == Code.Mul_rm16) {
						AddRegister(flags, ref usedRegisters, Register.AX, OpAccess.ReadWrite);
						AddRegister(flags, ref usedRegisters, Register.DX, OpAccess.Write);
					}
					else {
						Debug.Assert(code == Code.Imul_rm8 || code == Code.Mul_rm8);
						AddRegister(flags, ref usedRegisters, Register.AL, OpAccess.Read);
						AddRegister(flags, ref usedRegisters, Register.AX, OpAccess.Write);
					}
				}
				break;

			case CodeInfo.Enter:
				xsp = GetXSP(instruction.CodeSize, out xspMask);
				if ((flags & Flags.NoRegisterUsage) == 0) {
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, ref usedRegisters, Register.SS, OpAccess.Read);
					AddRegister(flags, ref usedRegisters, xsp, OpAccess.ReadWrite);
				}

				uint opSize;
				code = instruction.Code;
				Register rSP;
				if (code == Code.Enterq_imm16_imm8) {
					opSize = 8;
					memSize = MemorySize.UInt64;
					rSP = Register.RSP;
				}
				else if (code == Code.Enterd_imm16_imm8) {
					opSize = 4;
					memSize = MemorySize.UInt32;
					rSP = Register.ESP;
				}
				else {
					Debug.Assert(code == Code.Enterw_imm16_imm8);
					opSize = 2;
					memSize = MemorySize.UInt16;
					rSP = Register.SP;
				}

				if (rSP != xsp && (flags & Flags.NoRegisterUsage) == 0)
					AddRegister(flags, ref usedRegisters, rSP, OpAccess.ReadWrite);

				int nestingLevel = instruction.Immediate8_2nd & 0x1F;

				ulong xspOffset = 0;
				// push rBP
				if ((flags & Flags.NoRegisterUsage) == 0)
					AddRegister(flags, ref usedRegisters, rSP + 1, OpAccess.ReadWrite);
				if ((flags & Flags.NoMemoryUsage) == 0)
					AddMemory(ref usedMemoryLocations, Register.SS, xsp, Register.None, 1, (xspOffset -= opSize) & xspMask, memSize, OpAccess.Write);

				if (nestingLevel != 0) {
					var xbp = xsp + 1;// rBP immediately follows rSP
					ulong xbpOffset = 0;
					for (int i = 1; i < nestingLevel; i++) {
						if (i == 1 && rSP + 1 != xbp && (flags & Flags.NoRegisterUsage) == 0)
							AddRegister(flags, ref usedRegisters, xbp, OpAccess.ReadWrite);
						// push [xbp]
						if ((flags & Flags.NoMemoryUsage) == 0) {
							AddMemory(ref usedMemoryLocations, Register.SS, xbp, Register.None, 1, (xbpOffset -= opSize) & xspMask, memSize, OpAccess.Read);
							AddMemory(ref usedMemoryLocations, Register.SS, xsp, Register.None, 1, (xspOffset -= opSize) & xspMask, memSize, OpAccess.Write);
						}
					}
					// push frameTemp
					if ((flags & Flags.NoMemoryUsage) == 0)
						AddMemory(ref usedMemoryLocations, Register.SS, xsp, Register.None, 1, (xspOffset -= opSize) & xspMask, memSize, OpAccess.Write);
				}
				break;

			case CodeInfo.Leave:
				xsp = GetXSP(instruction.CodeSize, out xspMask);
				if ((flags & Flags.NoRegisterUsage) == 0) {
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, ref usedRegisters, Register.SS, OpAccess.Read);
					AddRegister(flags, ref usedRegisters, xsp, OpAccess.Write);
				}

				code = instruction.Code;
				if (code == Code.Leaveq) {
					if ((flags & Flags.NoMemoryUsage) == 0)
						AddMemory(ref usedMemoryLocations, Register.SS, xsp + 1, Register.None, 1, 0, MemorySize.UInt64, OpAccess.Read);
					if ((flags & Flags.NoRegisterUsage) == 0) {
						if (xsp + 1 == Register.RBP)
							AddRegister(flags, ref usedRegisters, Register.RBP, OpAccess.ReadWrite);
						else {
							AddRegister(flags, ref usedRegisters, xsp + 1, OpAccess.Read);
							AddRegister(flags, ref usedRegisters, Register.RBP, OpAccess.Write);
						}
					}
				}
				else if (code == Code.Leaved) {
					if ((flags & Flags.NoMemoryUsage) == 0)
						AddMemory(ref usedMemoryLocations, Register.SS, xsp + 1, Register.None, 1, 0, MemorySize.UInt32, OpAccess.Read);
					if ((flags & Flags.NoRegisterUsage) == 0) {
						if (xsp + 1 == Register.EBP)
							AddRegister(flags, ref usedRegisters, Register.EBP, OpAccess.ReadWrite);
						else {
							AddRegister(flags, ref usedRegisters, xsp + 1, OpAccess.Read);
							AddRegister(flags, ref usedRegisters, Register.EBP, OpAccess.Write);
						}
					}
				}
				else {
					Debug.Assert(code == Code.Leavew);
					if ((flags & Flags.NoMemoryUsage) == 0)
						AddMemory(ref usedMemoryLocations, Register.SS, xsp + 1, Register.None, 1, 0, MemorySize.UInt16, OpAccess.Read);
					if ((flags & Flags.NoRegisterUsage) == 0) {
						if (xsp + 1 == Register.BP)
							AddRegister(flags, ref usedRegisters, Register.BP, OpAccess.ReadWrite);
						else {
							AddRegister(flags, ref usedRegisters, xsp + 1, OpAccess.Read);
							AddRegister(flags, ref usedRegisters, Register.BP, OpAccess.Write);
						}
					}
				}
				break;

			case CodeInfo.Iret:
				xsp = GetXSP(instruction.CodeSize, out xspMask);
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, ref usedRegisters, Register.SS, (flags & Flags.Is64Bit) != 0 ? OpAccess.Write : OpAccess.Read);
					AddRegister(flags, ref usedRegisters, xsp, OpAccess.ReadWrite);
				}

				if ((flags & Flags.NoMemoryUsage) == 0) {
					code = instruction.Code;
					if (code == Code.Iretq) {
						AddMemory(ref usedMemoryLocations, Register.SS, xsp, Register.None, 1, 0 * 8, MemorySize.UInt64, OpAccess.Read);
						AddMemory(ref usedMemoryLocations, Register.SS, xsp, Register.None, 1, 1 * 8, MemorySize.UInt64, OpAccess.Read);
						AddMemory(ref usedMemoryLocations, Register.SS, xsp, Register.None, 1, 2 * 8, MemorySize.UInt64, OpAccess.Read);
						AddMemory(ref usedMemoryLocations, Register.SS, xsp, Register.None, 1, 3 * 8, MemorySize.UInt64, OpAccess.Read);
						AddMemory(ref usedMemoryLocations, Register.SS, xsp, Register.None, 1, 4 * 8, MemorySize.UInt64, OpAccess.Read);
					}
					else if (code == Code.Iretd) {
						AddMemory(ref usedMemoryLocations, Register.SS, xsp, Register.None, 1, 0 * 4, MemorySize.UInt32, OpAccess.Read);
						AddMemory(ref usedMemoryLocations, Register.SS, xsp, Register.None, 1, 1 * 4, MemorySize.UInt32, OpAccess.Read);
						AddMemory(ref usedMemoryLocations, Register.SS, xsp, Register.None, 1, 2 * 4, MemorySize.UInt32, OpAccess.Read);
						if (instruction.CodeSize == CodeSize.Code64) {
							AddMemory(ref usedMemoryLocations, Register.SS, xsp, Register.None, 1, 3 * 4, MemorySize.UInt32, OpAccess.Read);
							AddMemory(ref usedMemoryLocations, Register.SS, xsp, Register.None, 1, 4 * 4, MemorySize.UInt32, OpAccess.Read);
						}
					}
					else {
						Debug.Assert(code == Code.Iretw);
						AddMemory(ref usedMemoryLocations, Register.SS, xsp, Register.None, 1, 0 * 2, MemorySize.UInt16, OpAccess.Read);
						AddMemory(ref usedMemoryLocations, Register.SS, xsp, Register.None, 1, 1 * 2, MemorySize.UInt16, OpAccess.Read);
						AddMemory(ref usedMemoryLocations, Register.SS, xsp, Register.None, 1, 2 * 2, MemorySize.UInt16, OpAccess.Read);
						if (instruction.CodeSize == CodeSize.Code64) {
							AddMemory(ref usedMemoryLocations, Register.SS, xsp, Register.None, 1, 3 * 2, MemorySize.UInt16, OpAccess.Read);
							AddMemory(ref usedMemoryLocations, Register.SS, xsp, Register.None, 1, 4 * 2, MemorySize.UInt16, OpAccess.Read);
						}
					}
				}
				break;

			case CodeInfo.Vzeroall:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					OpAccess access;
					if (instruction.Code == Code.VEX_Vzeroupper)
						access = OpAccess.ReadWrite;
					else {
						Debug.Assert(instruction.Code == Code.VEX_Vzeroall);
						access = OpAccess.Write;
					}
					int maxVecRegs;
					if ((flags & Flags.Is64Bit) != 0)
						maxVecRegs = InstructionInfoConstants.VMM_last - InstructionInfoConstants.VMM_first + 1;
					else
						maxVecRegs = 8;
					for (int i = 0; i < maxVecRegs; i++)
						AddRegister(flags, ref usedRegisters, InstructionInfoConstants.VMM_first + i, access);
				}
				break;

			case CodeInfo.Jrcxz:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					code = instruction.Code;
					if (code == Code.Jrcxz_rel8_64 || code == Code.Jrcxz_rel8_16)
						AddRegister(flags, ref usedRegisters, Register.RCX, OpAccess.Read);
					else if (code == Code.Jecxz_rel8_64 || code == Code.Jecxz_rel8_32 || code == Code.Jecxz_rel8_16)
						AddRegister(flags, ref usedRegisters, Register.ECX, OpAccess.Read);
					else {
						Debug.Assert(code == Code.Jcxz_rel8_32 || code == Code.Jcxz_rel8_16);
						AddRegister(flags, ref usedRegisters, Register.CX, OpAccess.Read);
					}
				}
				break;

			case CodeInfo.Loop:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					code = instruction.Code;
					if (code == Code.Loopne_rel8_64_RCX || code == Code.Loope_rel8_64_RCX || code == Code.Loop_rel8_64_RCX ||
						code == Code.Loopne_rel8_16_RCX || code == Code.Loope_rel8_16_RCX || code == Code.Loop_rel8_16_RCX)
						AddRegister(flags, ref usedRegisters, Register.RCX, OpAccess.ReadWrite);
					else if (code == Code.Loopne_rel8_16_ECX || code == Code.Loopne_rel8_32_ECX || code == Code.Loopne_rel8_64_ECX ||
						code == Code.Loope_rel8_16_ECX || code == Code.Loope_rel8_32_ECX || code == Code.Loope_rel8_64_ECX ||
						code == Code.Loop_rel8_16_ECX || code == Code.Loop_rel8_32_ECX || code == Code.Loop_rel8_64_ECX)
						AddRegister(flags, ref usedRegisters, Register.ECX, OpAccess.ReadWrite);
					else {
						Debug.Assert(code == Code.Loopne_rel8_16_CX || code == Code.Loopne_rel8_32_CX ||
									code == Code.Loope_rel8_16_CX || code == Code.Loope_rel8_32_CX ||
									code == Code.Loop_rel8_16_CX || code == Code.Loop_rel8_32_CX);
						AddRegister(flags, ref usedRegisters, Register.CX, OpAccess.ReadWrite);
					}
				}
				break;

			case CodeInfo.Lahf:
				if ((flags & Flags.NoRegisterUsage) == 0)
					AddRegister(flags, ref usedRegisters, Register.AH, instruction.Code == Code.Sahf ? OpAccess.Read : OpAccess.Write);
				break;

			case CodeInfo.Lds:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					code = instruction.Code;
					if (Code.Lfs_r16_m32 <= code && code <= Code.Lfs_r64_m80)
						AddRegister(flags, ref usedRegisters, Register.FS, OpAccess.Write);
					else if (Code.Lgs_r16_m32 <= code && code <= Code.Lgs_r64_m80)
						AddRegister(flags, ref usedRegisters, Register.GS, OpAccess.Write);
					else if (Code.Lss_r16_m32 <= code && code <= Code.Lss_r64_m80)
						AddRegister(flags, ref usedRegisters, Register.SS, OpAccess.Write);
					else if (Code.Lds_r16_m32 <= code && code <= Code.Lds_r32_m48)
						AddRegister(flags, ref usedRegisters, Register.DS, OpAccess.Write);
					else {
						Debug.Assert(Code.Les_r16_m32 <= code && code <= Code.Les_r32_m48);
						AddRegister(flags, ref usedRegisters, Register.ES, OpAccess.Write);
					}
				}
				break;

			case CodeInfo.Maskmovq:
				switch (instruction.Op0Kind) {
				case OpKind.MemorySegDI:
					baseReg = Register.DI;
					break;

				case OpKind.MemorySegEDI:
					baseReg = Register.EDI;
					break;

				default:
					Debug.Assert(instruction.Op0Kind == OpKind.MemorySegRDI);
					baseReg = Register.RDI;
					break;
				}
				if ((flags & Flags.NoMemoryUsage) == 0)
					AddMemory(ref usedMemoryLocations, instruction.MemorySegment, baseReg, Register.None, 1, 0, instruction.MemorySize, OpAccess.Write);
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddMemorySegmentRegister(flags, ref usedRegisters, instruction.MemorySegment, OpAccess.Read);
					AddRegister(flags, ref usedRegisters, baseReg, OpAccess.Read);
				}
				break;

			case CodeInfo.Monitor:
				code = instruction.Code;
				if (code == Code.Monitorq || code == Code.Monitorxq)
					baseReg = Register.RAX;
				else if (code == Code.Monitord || code == Code.Monitorxd)
					baseReg = Register.EAX;
				else {
					Debug.Assert(code == Code.Monitorw || code == Code.Monitorxw);
					baseReg = Register.AX;
				}
				var seg = instruction.SegmentPrefix;
				if (seg == Register.None)
					seg = Register.DS;
				if ((flags & Flags.NoMemoryUsage) == 0)
					AddMemory(ref usedMemoryLocations, seg, baseReg, Register.None, 1, 0, MemorySize.Unknown, OpAccess.Read);
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddMemorySegmentRegister(flags, ref usedRegisters, seg, OpAccess.Read);
					AddRegister(flags, ref usedRegisters, baseReg, OpAccess.Read);
					if ((flags & Flags.Is64Bit) != 0) {
						AddRegister(flags, ref usedRegisters, Register.RCX, OpAccess.Read);
						AddRegister(flags, ref usedRegisters, Register.RDX, OpAccess.Read);
					}
					else {
						AddRegister(flags, ref usedRegisters, Register.ECX, OpAccess.Read);
						AddRegister(flags, ref usedRegisters, Register.EDX, OpAccess.Read);
					}
				}
				break;

			case CodeInfo.Mwait:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					if ((flags & Flags.Is64Bit) != 0) {
						AddRegister(flags, ref usedRegisters, Register.RAX, OpAccess.Read);
						AddRegister(flags, ref usedRegisters, Register.RCX, OpAccess.Read);
					}
					else {
						AddRegister(flags, ref usedRegisters, Register.EAX, OpAccess.Read);
						AddRegister(flags, ref usedRegisters, Register.ECX, OpAccess.Read);
					}
				}
				break;

			case CodeInfo.Mwaitx:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					if ((flags & Flags.Is64Bit) != 0) {
						AddRegister(flags, ref usedRegisters, Register.RAX, OpAccess.Read);
						AddRegister(flags, ref usedRegisters, Register.RCX, OpAccess.Read);
						AddRegister(flags, ref usedRegisters, Register.RBX, OpAccess.CondRead);
					}
					else {
						AddRegister(flags, ref usedRegisters, Register.EAX, OpAccess.Read);
						AddRegister(flags, ref usedRegisters, Register.ECX, OpAccess.Read);
						AddRegister(flags, ref usedRegisters, Register.EBX, OpAccess.CondRead);
					}
				}
				break;

			case CodeInfo.Mulx:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					if (instruction.Code == Code.VEX_Mulx_r32_r32_rm32)
						AddRegister(flags, ref usedRegisters, Register.EDX, OpAccess.Read);
					else {
						Debug.Assert(instruction.Code == Code.VEX_Mulx_r64_r64_rm64);
						AddRegister(flags, ref usedRegisters, Register.RDX, OpAccess.Read);
					}
				}
				break;

			case CodeInfo.PcmpXstrY:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					code = instruction.Code;
					if (code == Code.Pcmpestrm_xmm_xmmm128_imm8 || code == Code.VEX_Vpcmpestrm_xmm_xmmm128_imm8 ||
						code == Code.Pcmpestri_xmm_xmmm128_imm8 || code == Code.VEX_Vpcmpestri_xmm_xmmm128_imm8) {
						AddRegister(flags, ref usedRegisters, Register.EAX, OpAccess.Read);
						AddRegister(flags, ref usedRegisters, Register.EDX, OpAccess.Read);
					}
					else if (code == Code.Pcmpestrm64_xmm_xmmm128_imm8 || code == Code.VEX_Vpcmpestrm64_xmm_xmmm128_imm8 ||
						code == Code.Pcmpestri64_xmm_xmmm128_imm8 || code == Code.VEX_Vpcmpestri64_xmm_xmmm128_imm8) {
						AddRegister(flags, ref usedRegisters, Register.RAX, OpAccess.Read);
						AddRegister(flags, ref usedRegisters, Register.RDX, OpAccess.Read);
					}

					if (code == Code.Pcmpestrm_xmm_xmmm128_imm8 || code == Code.VEX_Vpcmpestrm_xmm_xmmm128_imm8 ||
						code == Code.Pcmpestrm64_xmm_xmmm128_imm8 || code == Code.VEX_Vpcmpestrm64_xmm_xmmm128_imm8 ||
						code == Code.Pcmpistrm_xmm_xmmm128_imm8 || code == Code.VEX_Vpcmpistrm_xmm_xmmm128_imm8) {
						AddRegister(flags, ref usedRegisters, Register.XMM0, OpAccess.Write);
					}
					else {
						Debug.Assert(code == Code.Pcmpestri_xmm_xmmm128_imm8 || code == Code.VEX_Vpcmpestri_xmm_xmmm128_imm8 ||
									code == Code.Pcmpestri64_xmm_xmmm128_imm8 || code == Code.VEX_Vpcmpestri64_xmm_xmmm128_imm8 ||
									code == Code.Pcmpistri_xmm_xmmm128_imm8 || code == Code.VEX_Vpcmpistri_xmm_xmmm128_imm8);
						AddRegister(flags, ref usedRegisters, Register.ECX, OpAccess.Write);
					}
				}
				break;

			case CodeInfo.Shift_Ib_MASK1FMOD9:
				if ((instruction.Immediate8 & 0x1F) % 9 == 0)
					rflagsInfo = RflagsInfo.None;
				break;

			case CodeInfo.Shift_Ib_MASK1FMOD11:
				if ((instruction.Immediate8 & 0x1F) % 17 == 0)
					rflagsInfo = RflagsInfo.None;
				break;

			case CodeInfo.Shift_Ib_MASK1F:
				if ((instruction.Immediate8 & 0x1F) == 0)
					rflagsInfo = RflagsInfo.None;
				break;

			case CodeInfo.Shift_Ib_MASK3F:
				if ((instruction.Immediate8 & 0x3F) == 0)
					rflagsInfo = RflagsInfo.None;
				break;

			case CodeInfo.R_EAX_EDX:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, ref usedRegisters, Register.EAX, OpAccess.Read);
					AddRegister(flags, ref usedRegisters, Register.EDX, OpAccess.Read);
				}
				break;

			case CodeInfo.R_ECX_W_EAX_EDX:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, ref usedRegisters, Register.EAX, OpAccess.Write);
					AddRegister(flags, ref usedRegisters, Register.ECX, OpAccess.Read);
					AddRegister(flags, ref usedRegisters, Register.EDX, OpAccess.Write);
				}
				break;

			case CodeInfo.R_EAX_ECX_EDX:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, ref usedRegisters, Register.EAX, OpAccess.Read);
					AddRegister(flags, ref usedRegisters, Register.ECX, OpAccess.Read);
					AddRegister(flags, ref usedRegisters, Register.EDX, OpAccess.Read);
				}
				break;

			case CodeInfo.W_EAX_EDX:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, ref usedRegisters, Register.EAX, OpAccess.Write);
					AddRegister(flags, ref usedRegisters, Register.EDX, OpAccess.Write);
				}
				break;

			case CodeInfo.W_EAX_ECX_EDX:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, ref usedRegisters, Register.EAX, OpAccess.Write);
					AddRegister(flags, ref usedRegisters, Register.ECX, OpAccess.Write);
					AddRegister(flags, ref usedRegisters, Register.EDX, OpAccess.Write);
				}
				break;

			case CodeInfo.Pconfig:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, ref usedRegisters, Register.EAX, OpAccess.ReadWrite);
					baseReg = (flags & Flags.Is64Bit) != 0 ? Register.RAX : Register.EAX;
					AddRegister(flags, ref usedRegisters, baseReg + 1, OpAccess.CondRead);
					AddRegister(flags, ref usedRegisters, baseReg + 2, OpAccess.CondRead);
					AddRegister(flags, ref usedRegisters, baseReg + 3, OpAccess.CondRead);
				}
				break;

			case CodeInfo.Syscall:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					code = instruction.Code;
					if (code == Code.Syscall) {
						AddRegister(flags, ref usedRegisters, Register.ECX, OpAccess.Write);
						if ((flags & Flags.Is64Bit) != 0)
							AddRegister(flags, ref usedRegisters, Register.R11, OpAccess.Write);
					}
					else if (code == Code.Sysenter)
						AddRegister(flags, ref usedRegisters, (flags & Flags.Is64Bit) != 0 ? Register.RSP : Register.ESP, OpAccess.Write);
					else if (code == Code.Sysretq) {
						AddRegister(flags, ref usedRegisters, Register.RCX, OpAccess.Read);
						AddRegister(flags, ref usedRegisters, Register.R11, OpAccess.Read);
					}
					else if (code == Code.Sysexitq) {
						AddRegister(flags, ref usedRegisters, Register.RCX, OpAccess.Read);
						AddRegister(flags, ref usedRegisters, Register.RDX, OpAccess.Read);
						AddRegister(flags, ref usedRegisters, Register.RSP, OpAccess.Write);
					}
					else if (code == Code.Sysretd) {
						AddRegister(flags, ref usedRegisters, Register.ECX, OpAccess.Read);
						if ((flags & Flags.Is64Bit) != 0)
							AddRegister(flags, ref usedRegisters, Register.R11, OpAccess.Read);
					}
					else {
						Debug.Assert(code == Code.Sysexitd);
						AddRegister(flags, ref usedRegisters, Register.ECX, OpAccess.Read);
						AddRegister(flags, ref usedRegisters, Register.EDX, OpAccess.Read);
						AddRegister(flags, ref usedRegisters, (flags & Flags.Is64Bit) != 0 ? Register.RSP : Register.ESP, OpAccess.Write);
					}
				}
				break;

			case CodeInfo.Encls:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					baseReg = (flags & Flags.Is64Bit) != 0 ? Register.RAX : Register.EAX;
					AddRegister(flags, ref usedRegisters, Register.EAX, OpAccess.Read);
					// rcx/ecx
					AddRegister(flags, ref usedRegisters, baseReg + 1, OpAccess.CondRead);
					AddRegister(flags, ref usedRegisters, baseReg + 1, OpAccess.CondWrite);
					// rdx/edx
					AddRegister(flags, ref usedRegisters, baseReg + 2, OpAccess.CondRead);
					AddRegister(flags, ref usedRegisters, baseReg + 2, OpAccess.CondWrite);
					// rbx/ebx
					AddRegister(flags, ref usedRegisters, baseReg + 3, OpAccess.CondRead);
					AddRegister(flags, ref usedRegisters, baseReg + 3, OpAccess.CondWrite);
				}
				break;

			case CodeInfo.Vmfunc:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, ref usedRegisters, Register.EAX, OpAccess.Read);
					AddRegister(flags, ref usedRegisters, Register.ECX, OpAccess.Read);
				}
				break;

			case CodeInfo.Vmload:
				code = instruction.Code;
				if (code == Code.Vmloadq || code == Code.Vmsaveq || code == Code.Vmrunq)
					baseReg = Register.RAX;
				else if (code == Code.Vmloadd || code == Code.Vmsaved || code == Code.Vmrund)
					baseReg = Register.EAX;
				else {
					Debug.Assert(code == Code.Vmloadw || code == Code.Vmsavew || code == Code.Vmrunw);
					baseReg = Register.AX;
				}
				if ((flags & Flags.NoRegisterUsage) == 0)
					AddRegister(flags, ref usedRegisters, baseReg, OpAccess.Read);
				break;

			case CodeInfo.R_CR0:
				if ((flags & Flags.NoRegisterUsage) == 0)
					AddRegister(flags, ref usedRegisters, Register.CR0, OpAccess.Read);
				break;

			case CodeInfo.RW_CR0:
				if ((flags & Flags.NoRegisterUsage) == 0)
					AddRegister(flags, ref usedRegisters, Register.CR0, OpAccess.ReadWrite);
				break;

			case CodeInfo.RW_ST0:
				if ((flags & Flags.NoRegisterUsage) == 0)
					AddRegister(flags, ref usedRegisters, Register.ST0, OpAccess.ReadWrite);
				break;

			case CodeInfo.R_ST0:
				if ((flags & Flags.NoRegisterUsage) == 0)
					AddRegister(flags, ref usedRegisters, Register.ST0, OpAccess.Read);
				break;

			case CodeInfo.W_ST0:
				if ((flags & Flags.NoRegisterUsage) == 0)
					AddRegister(flags, ref usedRegisters, Register.ST0, OpAccess.Write);
				break;

			case CodeInfo.R_ST0_ST1:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, ref usedRegisters, Register.ST0, OpAccess.Read);
					AddRegister(flags, ref usedRegisters, Register.ST1, OpAccess.Read);
				}
				break;

			case CodeInfo.R_ST0_R_ST1:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, ref usedRegisters, Register.ST0, OpAccess.Read);
					AddRegister(flags, ref usedRegisters, Register.ST1, OpAccess.Read);
				}
				break;

			case CodeInfo.R_ST0_RW_ST1:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, ref usedRegisters, Register.ST0, OpAccess.Read);
					AddRegister(flags, ref usedRegisters, Register.ST1, OpAccess.ReadWrite);
				}
				break;

			case CodeInfo.RW_ST0_R_ST1:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, ref usedRegisters, Register.ST0, OpAccess.ReadWrite);
					AddRegister(flags, ref usedRegisters, Register.ST1, OpAccess.Read);
				}
				break;

			case CodeInfo.Clzero:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					code = instruction.Code;
					if (code == Code.Clzeroq)
						baseReg = Register.RAX;
					else if (code == Code.Clzerod)
						baseReg = Register.EAX;
					else {
						Debug.Assert(code == Code.Clzerow);
						baseReg = Register.AX;
					}
					AddRegister(flags, ref usedRegisters, baseReg, OpAccess.Read);
					if ((flags & Flags.NoRegisterUsage) == 0) {
						baseReg = instruction.SegmentPrefix;
						if (baseReg == Register.None)
							baseReg = Register.DS;
						AddMemorySegmentRegister(flags, ref usedRegisters, baseReg, OpAccess.Read);
					}
				}
				break;

			case CodeInfo.Invlpga:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					code = instruction.Code;
					if (code == Code.Invlpgaq)
						baseReg = Register.RAX;
					else if (code == Code.Invlpgad)
						baseReg = Register.EAX;
					else {
						Debug.Assert(code == Code.Invlpgaw);
						baseReg = Register.AX;
					}
					AddRegister(flags, ref usedRegisters, baseReg, OpAccess.Read);
					AddRegister(flags, ref usedRegisters, Register.ECX, OpAccess.Read);
				}
				break;

			case CodeInfo.Llwpcb:
				if ((flags & (Flags.NoRegisterUsage | Flags.Is64Bit)) == 0)
					AddRegister(flags, ref usedRegisters, Register.DS, OpAccess.Read);
				if ((flags & Flags.NoMemoryUsage) == 0)
					AddMemory(ref usedMemoryLocations, Register.DS, instruction.Op0Register, Register.None, 1, 0, MemorySize.Unknown, OpAccess.Read);
				break;

			case CodeInfo.Loadall386:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, ref usedRegisters, Register.ES, OpAccess.Read);
					AddRegister(flags, ref usedRegisters, Register.EDI, OpAccess.Read);
				}
				break;

			case CodeInfo.Xbts:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					code = instruction.Code;
					if (code == Code.Xbts_r32_rm32 || code == Code.Ibts_rm32_r32)
						AddRegister(flags, ref usedRegisters, Register.EAX, OpAccess.Read);
					else {
						Debug.Assert(code == Code.Xbts_r16_rm16 || code == Code.Ibts_rm16_r16);
						AddRegister(flags, ref usedRegisters, Register.AX, OpAccess.Read);
					}
					AddRegister(flags, ref usedRegisters, Register.CL, OpAccess.Read);
				}
				break;

			case CodeInfo.Umonitor:
				baseReg = instruction.SegmentPrefix;
				if (baseReg == Register.None)
					baseReg = Register.DS;
				if ((flags & Flags.NoRegisterUsage) == 0)
					AddMemorySegmentRegister(flags, ref usedRegisters, baseReg, OpAccess.Read);
				if ((flags & Flags.NoMemoryUsage) == 0)
					AddMemory(ref usedMemoryLocations, baseReg, instruction.Op0Register, Register.None, 1, 0, MemorySize.UInt8, OpAccess.Read);
				break;

			case CodeInfo.Movdir64b:
				if ((flags & Flags.Is64Bit) == 0 && (flags & Flags.NoRegisterUsage) == 0)
					AddRegister(flags, ref usedRegisters, Register.ES, OpAccess.Read);
				if ((flags & Flags.NoMemoryUsage) == 0)
					AddMemory(ref usedMemoryLocations, Register.ES, instruction.Op0Register, Register.None, 1, 0, MemorySize.UInt512, OpAccess.Write);
				break;

			case CodeInfo.Clear_rflags:
				if (instruction.Op0Register != instruction.Op1Register)
					break;
				if (instruction.Op0Kind != OpKind.Register || instruction.Op1Kind != OpKind.Register)
					break;
				accesses[0] = OpAccess.Write;
				accesses[1] = OpAccess.None;
				rflagsInfo = RflagsInfo.C_cos_S_pz_U_a;
				if ((flags & Flags.NoRegisterUsage) == 0) {
					Debug.Assert(usedRegisters.ValidLength == 2 || usedRegisters.ValidLength == 3);
					usedRegisters.ValidLength = 0;
					AddRegister(flags, ref usedRegisters, instruction.Op0Register, OpAccess.Write);
				}
				break;

			case CodeInfo.Clear_reg_regmem:
				if (instruction.Op0Register != instruction.Op1Register)
					break;
				if (instruction.Op1Kind != OpKind.Register)
					break;
				accesses[0] = OpAccess.Write;
				accesses[1] = OpAccess.None;
				if ((flags & Flags.NoRegisterUsage) == 0) {
					Debug.Assert(usedRegisters.ValidLength == 2 || usedRegisters.ValidLength == 3);
					usedRegisters.Array[0] = new UsedRegister(instruction.Op0Register, OpAccess.Write);
					usedRegisters.ValidLength = 1;
				}
				break;

			case CodeInfo.Clear_reg_reg_regmem:
				if (instruction.Op1Register != instruction.Op2Register)
					break;
				if (instruction.Op2Kind != OpKind.Register)
					break;
				accesses[1] = OpAccess.None;
				accesses[2] = OpAccess.None;
				if ((flags & Flags.NoRegisterUsage) == 0) {
					Debug.Assert(usedRegisters.ValidLength == 3 || usedRegisters.ValidLength == 4);
					Debug.Assert(usedRegisters.Array[usedRegisters.ValidLength - 2].Register == instruction.Op1Register);
					Debug.Assert(usedRegisters.Array[usedRegisters.ValidLength - 1].Register == instruction.Op2Register);
					usedRegisters.ValidLength -= 2;
				}
				break;

			case CodeInfo.Montmul:
				Debug.Assert(Code.Montmul_16 + 1 == Code.Montmul_32);
				Debug.Assert(Code.Montmul_16 + 2 == Code.Montmul_64);
				Debug.Assert(Register.AX + 16 == Register.EAX);
				Debug.Assert(Register.AX + 32 == Register.RAX);
				if (instruction.Internal_HasRepeOrRepnePrefix) {
					baseReg = (Register)((instruction.Code - Code.Montmul_16) << 4);
					if ((flags & Flags.NoMemoryUsage) == 0)
						AddMemory(ref usedMemoryLocations, Register.ES, Register.SI + (int)baseReg, Register.None, 1, 0, MemorySize.Unknown, OpAccess.CondRead);
					if ((flags & Flags.NoRegisterUsage) == 0) {
						Debug.Assert(usedRegisters.ValidLength == 0);
						AddRegister(flags, ref usedRegisters, baseReg == 0 ? Register.ECX : Register.CX + (int)baseReg, OpAccess.ReadCondWrite);
						if ((flags & Flags.Is64Bit) == 0)
							AddRegister(flags, ref usedRegisters, Register.ES, OpAccess.CondRead);
						AddRegister(flags, ref usedRegisters, Register.SI + (int)baseReg, OpAccess.CondRead);
						AddRegister(flags, ref usedRegisters, Register.EAX, OpAccess.CondRead);
						AddRegister(flags, ref usedRegisters, Register.EAX, OpAccess.CondWrite);
						AddRegister(flags, ref usedRegisters, Register.EDX, OpAccess.CondWrite);
					}
				}
				else {
					baseReg = (Register)((instruction.Code - Code.Montmul_16) << 4);
					if ((flags & Flags.NoMemoryUsage) == 0)
						AddMemory(ref usedMemoryLocations, Register.ES, Register.SI + (int)baseReg, Register.None, 1, 0, instruction.MemorySize, OpAccess.Read);
					if ((flags & Flags.NoRegisterUsage) == 0) {
						if ((flags & Flags.Is64Bit) == 0)
							AddRegister(flags, ref usedRegisters, Register.ES, OpAccess.Read);
						AddRegister(flags, ref usedRegisters, Register.SI + (int)baseReg, OpAccess.Read);
						AddRegister(flags, ref usedRegisters, Register.EAX, OpAccess.ReadWrite);
						AddRegister(flags, ref usedRegisters, Register.EDX, OpAccess.Write);
					}
				}
				break;

			case CodeInfo.Xsha:
				Debug.Assert(Code.Xsha1_16 + 1 == Code.Xsha1_32);
				Debug.Assert(Code.Xsha1_16 + 2 == Code.Xsha1_64);
				Debug.Assert(Code.Xsha1_16 + 3 == Code.Xsha256_16);
				Debug.Assert(Code.Xsha1_16 + 4 == Code.Xsha256_32);
				Debug.Assert(Code.Xsha1_16 + 5 == Code.Xsha256_64);
				Debug.Assert(Register.AX + 16 == Register.EAX);
				Debug.Assert(Register.AX + 32 == Register.RAX);
				if (instruction.Internal_HasRepeOrRepnePrefix) {
					baseReg = (Register)(((instruction.Code - Code.Xsha1_16) % 3) << 4);
					if ((flags & Flags.NoMemoryUsage) == 0) {
						AddMemory(ref usedMemoryLocations, Register.ES, Register.SI + (int)baseReg, Register.None, 1, 0, MemorySize.Unknown, OpAccess.CondRead);
						AddMemory(ref usedMemoryLocations, Register.ES, Register.DI + (int)baseReg, Register.None, 1, 0, MemorySize.Unknown, OpAccess.CondRead);
						AddMemory(ref usedMemoryLocations, Register.ES, Register.DI + (int)baseReg, Register.None, 1, 0, MemorySize.Unknown, OpAccess.CondWrite);
					}
					if ((flags & Flags.NoRegisterUsage) == 0) {
						Debug.Assert(usedRegisters.ValidLength == 0);
						AddRegister(flags, ref usedRegisters, Register.CX + (int)baseReg, OpAccess.ReadCondWrite);
						if ((flags & Flags.Is64Bit) == 0)
							AddRegister(flags, ref usedRegisters, Register.ES, OpAccess.CondRead);
						AddRegister(flags, ref usedRegisters, Register.SI + (int)baseReg, OpAccess.CondRead);
						AddRegister(flags, ref usedRegisters, Register.SI + (int)baseReg, OpAccess.CondWrite);
						AddRegister(flags, ref usedRegisters, Register.DI + (int)baseReg, OpAccess.CondRead);
						AddRegister(flags, ref usedRegisters, Register.DI + (int)baseReg, OpAccess.CondWrite);
						AddRegister(flags, ref usedRegisters, Register.AX + (int)baseReg, OpAccess.CondRead);
						AddRegister(flags, ref usedRegisters, Register.AX + (int)baseReg, OpAccess.CondWrite);
					}
				}
				else {
					baseReg = (Register)(((instruction.Code - Code.Xsha1_16) % 3) << 4);
					if ((flags & Flags.NoMemoryUsage) == 0) {
						AddMemory(ref usedMemoryLocations, Register.ES, Register.SI + (int)baseReg, Register.None, 1, 0, instruction.MemorySize, OpAccess.Read);
						AddMemory(ref usedMemoryLocations, Register.ES, Register.DI + (int)baseReg, Register.None, 1, 0, instruction.MemorySize, OpAccess.ReadWrite);
					}
					if ((flags & Flags.NoRegisterUsage) == 0) {
						if ((flags & Flags.Is64Bit) == 0)
							AddRegister(flags, ref usedRegisters, Register.ES, OpAccess.Read);
						AddRegister(flags, ref usedRegisters, Register.SI + (int)baseReg, OpAccess.ReadWrite);
						AddRegister(flags, ref usedRegisters, Register.DI + (int)baseReg, OpAccess.ReadWrite);
					}
				}
				break;

			case CodeInfo.Xcrypt:
				Debug.Assert(Code.XcryptEcb_16 + 1 == Code.XcryptEcb_32);
				Debug.Assert(Code.XcryptEcb_16 + 2 == Code.XcryptEcb_64);
				Debug.Assert(Code.XcryptEcb_16 + 3 == Code.XcryptCbc_16);
				Debug.Assert(Code.XcryptEcb_16 + 4 == Code.XcryptCbc_32);
				Debug.Assert(Code.XcryptEcb_16 + 5 == Code.XcryptCbc_64);
				Debug.Assert(Code.XcryptEcb_16 + 6 == Code.XcryptCtr_16);
				Debug.Assert(Code.XcryptEcb_16 + 7 == Code.XcryptCtr_32);
				Debug.Assert(Code.XcryptEcb_16 + 8 == Code.XcryptCtr_64);
				Debug.Assert(Code.XcryptEcb_16 + 9 == Code.XcryptCfb_16);
				Debug.Assert(Code.XcryptEcb_16 + 10 == Code.XcryptCfb_32);
				Debug.Assert(Code.XcryptEcb_16 + 11 == Code.XcryptCfb_64);
				Debug.Assert(Code.XcryptEcb_16 + 12 == Code.XcryptOfb_16);
				Debug.Assert(Code.XcryptEcb_16 + 13 == Code.XcryptOfb_32);
				Debug.Assert(Code.XcryptEcb_16 + 14 == Code.XcryptOfb_64);
				Debug.Assert(Register.AX + 16 == Register.EAX);
				Debug.Assert(Register.AX + 32 == Register.RAX);
				if (instruction.Internal_HasRepeOrRepnePrefix) {
					Debug.Assert(OpKind.MemoryESDI + 1 == OpKind.MemoryESEDI);
					Debug.Assert(OpKind.MemoryESDI + 2 == OpKind.MemoryESRDI);
					Debug.Assert(Register.DI + 16 == Register.EDI);
					Debug.Assert(Register.DI + 32 == Register.RDI);
					Debug.Assert(Register.CX + 16 == Register.ECX);
					Debug.Assert(Register.CX + 32 == Register.RCX);
					baseReg = (Register)(((instruction.Code - Code.XcryptEcb_16) % 3) << 4);
					if ((flags & Flags.NoMemoryUsage) == 0) {
						// Check if not XcryptEcb
						if (instruction.Code >= Code.XcryptCbc_16) {
							AddMemory(ref usedMemoryLocations, Register.ES, Register.AX + (int)baseReg, Register.None, 1, 0, MemorySize.Unknown, OpAccess.CondRead);
							AddMemory(ref usedMemoryLocations, Register.ES, Register.AX + (int)baseReg, Register.None, 1, 0, MemorySize.Unknown, OpAccess.CondWrite);
						}
						AddMemory(ref usedMemoryLocations, Register.ES, Register.DX + (int)baseReg, Register.None, 1, 0, MemorySize.Unknown, OpAccess.CondRead);
						AddMemory(ref usedMemoryLocations, Register.ES, Register.BX + (int)baseReg, Register.None, 1, 0, MemorySize.Unknown, OpAccess.CondRead);
						AddMemory(ref usedMemoryLocations, Register.ES, Register.SI + (int)baseReg, Register.None, 1, 0, MemorySize.Unknown, OpAccess.CondRead);
						AddMemory(ref usedMemoryLocations, Register.ES, Register.DI + (int)baseReg, Register.None, 1, 0, MemorySize.Unknown, OpAccess.CondWrite);
					}
					if ((flags & Flags.NoRegisterUsage) == 0) {
						Debug.Assert(usedRegisters.ValidLength == 0);
						AddRegister(flags, ref usedRegisters, Register.CX + (int)baseReg, OpAccess.ReadCondWrite);
						if ((flags & Flags.Is64Bit) == 0)
							AddRegister(flags, ref usedRegisters, Register.ES, OpAccess.CondRead);
						// Check if not XcryptEcb
						if (instruction.Code >= Code.XcryptCbc_16) {
							AddRegister(flags, ref usedRegisters, Register.AX + (int)baseReg, OpAccess.CondRead);
							AddRegister(flags, ref usedRegisters, Register.AX + (int)baseReg, OpAccess.CondWrite);
						}
						AddRegister(flags, ref usedRegisters, Register.DX + (int)baseReg, OpAccess.CondRead);
						AddRegister(flags, ref usedRegisters, Register.BX + (int)baseReg, OpAccess.CondRead);
						AddRegister(flags, ref usedRegisters, Register.SI + (int)baseReg, OpAccess.CondRead);
						AddRegister(flags, ref usedRegisters, Register.SI + (int)baseReg, OpAccess.CondWrite);
						AddRegister(flags, ref usedRegisters, Register.DI + (int)baseReg, OpAccess.CondRead);
						AddRegister(flags, ref usedRegisters, Register.DI + (int)baseReg, OpAccess.CondWrite);
					}
				}
				else {
					Debug.Assert(OpKind.MemoryESDI + 1 == OpKind.MemoryESEDI);
					Debug.Assert(OpKind.MemoryESDI + 2 == OpKind.MemoryESRDI);
					Debug.Assert(Register.DI + 16 == Register.EDI);
					Debug.Assert(Register.DI + 32 == Register.RDI);
					baseReg = (Register)(((instruction.Code - Code.XcryptEcb_16) % 3) << 4);
					if ((flags & Flags.NoMemoryUsage) == 0) {
						// Check if not XcryptEcb
						if (instruction.Code >= Code.XcryptCbc_16)
							AddMemory(ref usedMemoryLocations, Register.ES, Register.AX + (int)baseReg, Register.None, 1, 0, instruction.MemorySize, OpAccess.ReadWrite);
						AddMemory(ref usedMemoryLocations, Register.ES, Register.DX + (int)baseReg, Register.None, 1, 0, instruction.MemorySize, OpAccess.Read);
						AddMemory(ref usedMemoryLocations, Register.ES, Register.BX + (int)baseReg, Register.None, 1, 0, instruction.MemorySize, OpAccess.Read);
						AddMemory(ref usedMemoryLocations, Register.ES, Register.SI + (int)baseReg, Register.None, 1, 0, instruction.MemorySize, OpAccess.Read);
						AddMemory(ref usedMemoryLocations, Register.ES, Register.DI + (int)baseReg, Register.None, 1, 0, instruction.MemorySize, OpAccess.Write);
					}
					if ((flags & Flags.NoRegisterUsage) == 0) {
						if ((flags & Flags.Is64Bit) == 0)
							AddRegister(flags, ref usedRegisters, Register.ES, OpAccess.Read);
						// Check if not XcryptEcb
						if (instruction.Code >= Code.XcryptCbc_16)
							AddRegister(flags, ref usedRegisters, Register.AX + (int)baseReg, OpAccess.ReadWrite);
						AddRegister(flags, ref usedRegisters, Register.DX + (int)baseReg, OpAccess.Read);
						AddRegister(flags, ref usedRegisters, Register.BX + (int)baseReg, OpAccess.Read);
						AddRegister(flags, ref usedRegisters, Register.SI + (int)baseReg, OpAccess.ReadWrite);
						AddRegister(flags, ref usedRegisters, Register.DI + (int)baseReg, OpAccess.ReadWrite);
					}
				}
				break;

			case CodeInfo.Xstore:
				Debug.Assert(Code.Xstore_16 + 1 == Code.Xstore_32);
				Debug.Assert(Code.Xstore_16 + 2 == Code.Xstore_64);
				Debug.Assert(Register.AX + 16 == Register.EAX);
				Debug.Assert(Register.AX + 32 == Register.RAX);
				if (instruction.Internal_HasRepeOrRepnePrefix) {
					baseReg = (Register)((instruction.Code - Code.Xstore_16) << 4);
					if ((flags & Flags.NoMemoryUsage) == 0)
						AddMemory(ref usedMemoryLocations, Register.ES, Register.DI + (int)baseReg, Register.None, 1, 0, MemorySize.Unknown, OpAccess.CondWrite);
					if ((flags & Flags.NoRegisterUsage) == 0) {
						Debug.Assert(usedRegisters.ValidLength == 0);
						AddRegister(flags, ref usedRegisters, Register.CX + (int)baseReg, OpAccess.ReadCondWrite);
						if ((flags & Flags.Is64Bit) == 0)
							AddRegister(flags, ref usedRegisters, Register.ES, OpAccess.CondRead);
						AddRegister(flags, ref usedRegisters, Register.DI + (int)baseReg, OpAccess.CondRead);
						AddRegister(flags, ref usedRegisters, Register.DI + (int)baseReg, OpAccess.CondWrite);
						AddRegister(flags, ref usedRegisters, Register.EAX, OpAccess.CondWrite);
						AddRegister(flags, ref usedRegisters, Register.EDX, OpAccess.CondRead);
					}
				}
				else {
					baseReg = (Register)((instruction.Code - Code.Xstore_16) << 4);
					if ((flags & Flags.NoMemoryUsage) == 0)
						AddMemory(ref usedMemoryLocations, Register.ES, Register.DI + (int)baseReg, Register.None, 1, 0, instruction.MemorySize, OpAccess.Write);
					if ((flags & Flags.NoRegisterUsage) == 0) {
						if ((flags & Flags.Is64Bit) == 0)
							AddRegister(flags, ref usedRegisters, Register.ES, OpAccess.Read);
						AddRegister(flags, ref usedRegisters, Register.DI + (int)baseReg, OpAccess.ReadWrite);
						AddRegister(flags, ref usedRegisters, Register.EAX, OpAccess.Write);
						AddRegister(flags, ref usedRegisters, Register.EDX, OpAccess.Read);
					}
				}
				break;

			case CodeInfo.None:
			default:
				throw new InvalidOperationException();
			}
		}

		static void AddMemory(ref SimpleList<UsedMemory> usedMemoryLocations, Register segReg, Register baseReg, Register indexReg, int scale, ulong displ, MemorySize memorySize, OpAccess access) {
			if (access != OpAccess.NoMemAccess) {
				int arrayLength = usedMemoryLocations.Array.Length;
				int validLen = usedMemoryLocations.ValidLength;
				if (arrayLength == validLen) {
					if (arrayLength == 0)
						usedMemoryLocations.Array = new UsedMemory[defaultMemoryArrayCount];
					else
						Array.Resize(ref usedMemoryLocations.Array, arrayLength * 2);
				}
				usedMemoryLocations.Array[validLen] = new UsedMemory(segReg, baseReg, indexReg, scale, displ, memorySize, access);
				usedMemoryLocations.ValidLength = validLen + 1;
			}
		}

		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		static void AddMemorySegmentRegister(Flags flags, ref SimpleList<UsedRegister> regs, Register seg, OpAccess access) {
			Debug.Assert(Register.ES <= seg && seg <= Register.GS);
			// Ignore es,cs,ss,ds memory operand segment registers in 64-bit mode
			if ((flags & Flags.Is64Bit) == 0 || seg >= Register.FS)
				AddRegister(flags, ref regs, seg, access);
		}

		static void AddRegister(Flags flags, ref SimpleList<UsedRegister> regs, Register reg, OpAccess access) {
			Debug.Assert((flags & Flags.NoRegisterUsage) == 0, "Caller should check flags before calling this method");

			var writeReg = reg;
			if ((flags & (Flags.Is64Bit | Flags.ZeroExtVecRegs)) != 0) {
				Debug.Assert(OpAccess.Write + 1 == OpAccess.CondWrite);
				Debug.Assert(OpAccess.Write + 2 == OpAccess.ReadWrite);
				Debug.Assert(OpAccess.Write + 3 == OpAccess.ReadCondWrite);
				if ((uint)(access - OpAccess.Write) <= 3) {
					int index;
					Debug.Assert(InstructionInfoConstants.VMM_first == Register.ZMM0);
					const uint VecRegCount = InstructionInfoConstants.VMM_last - InstructionInfoConstants.VMM_first + 1;
					Debug.Assert((VecRegCount & (VecRegCount - 1)) == 0);// Verify that it's a power of 2
					if ((flags & Flags.Is64Bit) != 0 && (uint)(index = reg - Register.EAX) <= (Register.R15D - Register.EAX))
						writeReg = Register.RAX + index;
					else if ((flags & Flags.ZeroExtVecRegs) != 0 && (uint)(index = reg - Register.XMM0) <= InstructionInfoConstants.VMM_last - Register.XMM0)
						writeReg = Register.ZMM0 + (index & ((int)VecRegCount - 1));
					if (access != OpAccess.ReadWrite && access != OpAccess.ReadCondWrite)
						reg = writeReg;
				}
			}

			var array = regs.Array;
			int validLen = regs.ValidLength;
			int arrayLength = array.Length;
			int numRegs = writeReg == reg ? 1 : 2;
			if (validLen + numRegs > arrayLength) {
				if (arrayLength == 0) {
					// The code below that resizes the array assumes there's at least 2 new free elements, so the minimum array length is 2.
					Debug.Assert(defaultRegisterArrayCount >= 2);
					regs.Array = array = new UsedRegister[defaultRegisterArrayCount];
				}
				else {
					Debug.Assert(arrayLength * 2 >= arrayLength + numRegs);
					Array.Resize(ref regs.Array, arrayLength * 2);
					array = regs.Array;
				}
			}

			if (writeReg == reg) {
				array[validLen] = new UsedRegister(reg, access);
				regs.ValidLength = validLen + 1;
			}
			else {
				Debug.Assert(access == OpAccess.ReadWrite || access == OpAccess.ReadCondWrite);
				array[validLen] = new UsedRegister(reg, OpAccess.Read);
				validLen++;
				var lastAccess = access == OpAccess.ReadWrite ? OpAccess.Write : OpAccess.CondWrite;
				array[validLen] = new UsedRegister(writeReg, lastAccess);
				regs.ValidLength = validLen + 1;
			}
		}
	}
}
#endif
