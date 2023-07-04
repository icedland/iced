// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if INSTR_INFO
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Iced.Intel.InstructionInfoInternal;

namespace Iced.Intel {
	/// <summary>
	/// Instruction info options used by <see cref="InstructionInfoFactory"/>
	/// </summary>
	[Flags]
	public enum InstructionInfoOptions : uint {
		/// <summary>
		/// No option is enabled
		/// </summary>
		None						= 0,

		/// <summary>
		/// Don't include memory usage, i.e., <see cref="InstructionInfo.GetUsedMemory"/> will return an empty iterator. All
		/// registers that are used by memory operands are still returned by <see cref="InstructionInfo.GetUsedRegisters"/>.
		/// </summary>
		NoMemoryUsage				= 0x00000001,

		/// <summary>
		/// Don't include register usage, i.e., <see cref="InstructionInfo.GetUsedRegisters"/> will return an empty iterator
		/// </summary>
		NoRegisterUsage				= 0x00000002,
	}

	/// <summary>
	/// Creates <see cref="InstructionInfo"/>s.
	/// </summary>
	public sealed class InstructionInfoFactory {
		const int defaultRegisterArrayCount = 2;
		const int defaultMemoryArrayCount = 1;

		InstructionInfo info;

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
		public InstructionInfoFactory() => info = new InstructionInfo(true);

		/// <summary>
		/// Creates an <see cref="InstructionInfo"/>. The return value is only valid until this instance creates a new <see cref="InstructionInfo"/> value.
		/// </summary>
		/// <param name="instruction">Instruction</param>
		/// <returns></returns>
		public ref readonly InstructionInfo GetInfo(in Instruction instruction) =>
			ref Create(instruction, InstructionInfoOptions.None);

		/// <summary>
		/// Creates an <see cref="InstructionInfo"/>. The return value is only valid until this instance creates a new <see cref="InstructionInfo"/> value.
		/// </summary>
		/// <param name="instruction">Instruction</param>
		/// <param name="options">Options</param>
		/// <returns></returns>
		public ref readonly InstructionInfo GetInfo(in Instruction instruction, InstructionInfoOptions options) =>
			ref Create(instruction, options);

		ref readonly InstructionInfo Create(in Instruction instruction, InstructionInfoOptions options) {
			info.usedRegisters.ValidLength = 0;
			info.usedMemoryLocations.ValidLength = 0;

			var data = InstrInfoTable.Data;
			var index = (uint)instruction.Code << 1;
			var flags1 = data[(int)index];
			var flags2 = data[(int)index + 1];

			var codeSize = instruction.CodeSize;
			Static.Assert((uint)InstructionInfoOptions.NoMemoryUsage == (uint)Flags.NoMemoryUsage ? 0 : -1);
			Static.Assert((uint)InstructionInfoOptions.NoRegisterUsage == (uint)Flags.NoRegisterUsage ? 0 : -1);
			var flags = (Flags)options & (Flags.NoMemoryUsage | Flags.NoRegisterUsage);
			if (codeSize == CodeSize.Code64 || codeSize == CodeSize.Unknown)
				flags |= Flags.Is64Bit;
			if ((flags2 & ((uint)InfoFlags2.EncodingMask << (int)InfoFlags2.EncodingShift)) != ((uint)EncodingKind.Legacy << (int)InfoFlags2.EncodingShift))
				flags |= Flags.ZeroExtVecRegs;

			OpAccess op0Access;
			// If it fails, update InstrInfoTypesGen.GenerateOpInfoX()
			Static.Assert(InstrInfoConstants.OpInfo0_Count == 13 ? 0 : -1);
			var op0Info = (OpInfo0)((flags1 >> (int)InfoFlags1.OpInfo0Shift) & (uint)InfoFlags1.OpInfo0Mask);
			switch (op0Info) {
			default:
			case OpInfo0.None:
				op0Access = OpAccess.None;
				break;

			case OpInfo0.Read:
				op0Access = OpAccess.Read;
				break;

			case OpInfo0.Write:
				if (instruction.HasOpMask && instruction.MergingMasking) {
					if (instruction.Op0Kind != OpKind.Register)
						op0Access = OpAccess.CondWrite;
					else
						op0Access = OpAccess.ReadWrite;
				}
				else
					op0Access = OpAccess.Write;
				break;

			case OpInfo0.WriteVmm:
				// If it's opmask+merging ({k1}) and dest is xmm/ymm/zmm, then access is one of:
				//	k1			mem			xmm/ymm		zmm
				//	----------------------------------------
				//	all 1s		write		write		write		all bits are overwritten, upper bits in zmm (if xmm/ymm) are cleared
				//	all 0s		no access	read/write	no access	no elem is written, but xmm/ymm's upper bits (in zmm) are cleared so
				//													treat it as R lower bits + clear upper bits + W full reg
				//	else		cond-write	read/write	r-c-w		some elems are unchanged, the others are overwritten
				// If it's xmm/ymm, use RW, else use RCW. If it's mem, use CW
				if (instruction.HasOpMask && instruction.MergingMasking) {
					if (instruction.Op0Kind != OpKind.Register)
						op0Access = OpAccess.CondWrite;
					else
						op0Access = OpAccess.ReadCondWrite;
				}
				else
					op0Access = OpAccess.Write;
				break;

			case OpInfo0.WriteForce:
			case OpInfo0.WriteForceP1:
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

			case OpInfo0.ReadWriteVmm:
				// If it's opmask+merging ({k1}) and dest is xmm/ymm/zmm, then access is one of:
				//	k1			xmm/ymm		zmm
				//	-------------------------------
				//	all 1s		read/write	read/write	all bits are overwritten, upper bits in zmm (if xmm/ymm) are cleared
				//	all 0s		read/write	no access	no elem is written, but xmm/ymm's upper bits (in zmm) are cleared so
				//										treat it as R lower bits + clear upper bits + W full reg
				//	else		read/write	r-c-w		some elems are unchanged, the others are overwritten
				// If it's xmm/ymm, use RW, else use RCW
				if (instruction.HasOpMask && instruction.MergingMasking)
					op0Access = OpAccess.ReadCondWrite;
				else
					op0Access = OpAccess.ReadWrite;
				break;

			case OpInfo0.ReadCondWrite:
				op0Access = OpAccess.ReadCondWrite;
				break;

			case OpInfo0.NoMemAccess:
				op0Access = OpAccess.NoMemAccess;
				break;

			case OpInfo0.WriteMem_ReadWriteReg:
				if (instruction.Internal_Op0IsNotReg_or_Op1IsNotReg)
					op0Access = OpAccess.Write;
				else
					op0Access = OpAccess.ReadWrite;
				break;
			}

			Debug.Assert(instruction.OpCount <= IcedConstants.MaxOpCount);
			unsafe { info.opAccesses[0] = (byte)op0Access; }
			var op1Info = (OpInfo1)((flags1 >> (int)InfoFlags1.OpInfo1Shift) & (uint)InfoFlags1.OpInfo1Mask);
			unsafe { info.opAccesses[1] = (byte)OpAccesses.Op1[(int)op1Info]; }
			unsafe { info.opAccesses[2] = (byte)OpAccesses.Op2[(int)((flags1 >> (int)InfoFlags1.OpInfo2Shift) & (uint)InfoFlags1.OpInfo2Mask)]; }
			if ((flags1 & (((uint)InfoFlags1.OpInfo3Mask) << (int)InfoFlags1.OpInfo3Shift)) != 0) {
				Static.Assert((int)InstrInfoConstants.OpInfo3_Count == 2 ? 0 : -1);
				unsafe { info.opAccesses[3] = (byte)OpAccess.Read; }
			}
			else
				unsafe { info.opAccesses[3] = (byte)OpAccess.None; }
			if ((flags1 & (((uint)InfoFlags1.OpInfo4Mask) << (int)InfoFlags1.OpInfo4Shift)) != 0) {
				Static.Assert((int)InstrInfoConstants.OpInfo4_Count == 2 ? 0 : -1);
				unsafe { info.opAccesses[4] = (byte)OpAccess.Read; }
			}
			else
				unsafe { info.opAccesses[4] = (byte)OpAccess.None; }
			Static.Assert(IcedConstants.MaxOpCount == 5 ? 0 : -1);

			int opCount = instruction.OpCount;
			for (int i = 0; i < opCount; i++) {
				OpAccess access;
				unsafe { access = (OpAccess)info.opAccesses[i]; }
				if (access == OpAccess.None)
					continue;

				switch (instruction.GetOpKind(i)) {
				case OpKind.Register:
					if (access == OpAccess.NoMemAccess) {
						access = OpAccess.Read;
						unsafe { info.opAccesses[i] = (byte)OpAccess.Read; };
					}
					if ((flags & Flags.NoRegisterUsage) == 0) {
						if (i == 0 && op0Info == OpInfo0.WriteForceP1) {
							var reg = instruction.Op0Register;
							AddRegister(flags, reg, access);
							if (Register.K0 <= reg && reg <= Register.K7)
								AddRegister(flags, ((reg - Register.K0) ^ 1) + Register.K0, access);
						}
						else if (i == 1 && op1Info == OpInfo1.ReadP3) {
							var reg = instruction.Op1Register;
							if (Register.XMM0 <= reg && reg <= IcedConstants.VMM_last) {
								reg = IcedConstants.VMM_first + ((reg - IcedConstants.VMM_first) & ~3);
								for (int j = 0; j < 4; j++)
									AddRegister(flags, reg + j, access);
							}
						}
						else
							AddRegister(flags, instruction.GetOpRegister(i), access);
					}
					break;

				case OpKind.Memory:
					Static.Assert((uint)InfoFlags1.IgnoresSegment == (1U << 31) ? 0 : -1);
					Static.Assert(Register.None == 0 ? 0 : -1);
					var segReg = (Register)((uint)instruction.MemorySegment & ~(uint)((int)flags1 >> 31));
					var baseReg = instruction.MemoryBase;
					if (baseReg == Register.RIP) {
						if ((flags & Flags.NoMemoryUsage) == 0)
							AddMemory(segReg, Register.None, Register.None, 1, instruction.MemoryDisplacement64, instruction.MemorySize, access, CodeSize.Code64, 0);
						if ((flags & Flags.NoRegisterUsage) == 0 && segReg != Register.None)
							AddMemorySegmentRegister(flags, segReg, OpAccess.Read);
					}
					else if (baseReg == Register.EIP) {
						if ((flags & Flags.NoMemoryUsage) == 0)
							AddMemory(segReg, Register.None, Register.None, 1, instruction.MemoryDisplacement32, instruction.MemorySize, access, CodeSize.Code32, 0);
						if ((flags & Flags.NoRegisterUsage) == 0 && segReg != Register.None)
							AddMemorySegmentRegister(flags, segReg, OpAccess.Read);
					}
					else {
						int scale;
						Register indexReg;
						if ((flags1 & (uint)InfoFlags1.IgnoresIndexVA) != 0) {
							indexReg = instruction.MemoryIndex;
							if ((flags & Flags.NoRegisterUsage) == 0 && indexReg != Register.None)
								AddRegister(flags, indexReg, OpAccess.Read);
							indexReg = Register.None;
							scale = 1;
						}
						else {
							indexReg = instruction.MemoryIndex;
							scale = instruction.MemoryIndexScale;
						}
						if ((flags & Flags.NoMemoryUsage) == 0) {
							var addrSizeBytes = InstructionUtils.GetAddressSizeInBytes(baseReg, indexReg, instruction.MemoryDisplSize, codeSize);
							var addrSize = addrSizeBytes switch {
								2 => CodeSize.Code16,
								4 => CodeSize.Code32,
								8 => CodeSize.Code64,
								_ => CodeSize.Unknown,
							};
							int vsibSize = 0;
							if (indexReg.IsVectorRegister() && instruction.TryGetVsib64(out bool vsib64))
								vsibSize = vsib64 ? 8 : 4;
							ulong displ;
							if (addrSizeBytes == 8)
								displ = instruction.MemoryDisplacement64;
							else
								displ = instruction.MemoryDisplacement32;
							AddMemory(segReg, baseReg, indexReg, scale, displ, instruction.MemorySize, access, addrSize, vsibSize);
						}
						if ((flags & Flags.NoRegisterUsage) == 0) {
							if (segReg != Register.None)
								AddMemorySegmentRegister(flags, segReg, OpAccess.Read);
							if (baseReg != Register.None)
								AddRegister(flags, baseReg, OpAccess.Read);
							if (indexReg != Register.None)
								AddRegister(flags, indexReg, OpAccess.Read);
						}
					}
					break;
				}
			}

			var impliedAccess = (ImpliedAccess)((flags1 >> (int)InfoFlags1.ImpliedAccessShift) & (uint)InfoFlags1.ImpliedAccessMask);
			if (impliedAccess != ImpliedAccess.None)
				AddImpliedAccesses(impliedAccess, instruction, flags);

			if (instruction.HasOpMask && (flags & Flags.NoRegisterUsage) == 0)
				AddRegister(flags, instruction.OpMask, (flags1 & (uint)InfoFlags1.OpMaskReadWrite) != 0 ? OpAccess.ReadWrite : OpAccess.Read);

			return ref info;
		}

		static Register GetXSP(CodeSize codeSize, out ulong xspMask, out CodeSize addressSize) {
			if (codeSize == CodeSize.Code64 || codeSize == CodeSize.Unknown) {
				xspMask = ulong.MaxValue;
				addressSize = CodeSize.Code64;
				return Register.RSP;
			}
			if (codeSize == CodeSize.Code32) {
				xspMask = uint.MaxValue;
				addressSize = CodeSize.Code32;
				return Register.ESP;
			}
			Debug.Assert(codeSize == CodeSize.Code16);
			xspMask = ushort.MaxValue;
			addressSize = CodeSize.Code16;
			return Register.SP;
		}

		void AddImpliedAccesses(ImpliedAccess impliedAccess, in Instruction instruction, Flags flags) {
			Debug.Assert(impliedAccess != ImpliedAccess.None);
			switch (impliedAccess) {
			// GENERATOR-BEGIN: ImpliedAccessHandler
			// ⚠️This was generated by GENERATOR!🦹‍♂️
			case ImpliedAccess.None:
				break;
			case ImpliedAccess.Shift_Ib_MASK1FMOD9:
				break;
			case ImpliedAccess.Shift_Ib_MASK1FMOD11:
				break;
			case ImpliedAccess.Shift_Ib_MASK1F:
				break;
			case ImpliedAccess.Shift_Ib_MASK3F:
				break;
			case ImpliedAccess.Clear_rflags:
				CommandClearRflags(instruction, flags);
				break;
			case ImpliedAccess.t_push1x2:
				CommandPush(instruction, flags, 1, 2);
				break;
			case ImpliedAccess.t_push1x4:
				CommandPush(instruction, flags, 1, 4);
				break;
			case ImpliedAccess.t_pop1x2:
				CommandPop(instruction, flags, 1, 2);
				break;
			case ImpliedAccess.t_pop1x4:
				CommandPop(instruction, flags, 1, 4);
				break;
			case ImpliedAccess.t_RWal:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.AL, OpAccess.ReadWrite);
				}
				break;
			case ImpliedAccess.t_RWax:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.AX, OpAccess.ReadWrite);
				}
				break;
			case ImpliedAccess.t_push1x8:
				CommandPush(instruction, flags, 1, 8);
				break;
			case ImpliedAccess.t_pop1x8:
				CommandPop(instruction, flags, 1, 8);
				break;
			case ImpliedAccess.t_pusha2:
				CommandPusha(instruction, flags, 2);
				break;
			case ImpliedAccess.t_pusha4:
				CommandPusha(instruction, flags, 4);
				break;
			case ImpliedAccess.t_popa2:
				CommandPopa(instruction, flags, 2);
				break;
			case ImpliedAccess.t_popa4:
				CommandPopa(instruction, flags, 4);
				break;
			case ImpliedAccess.t_arpl:
				CommandArpl(instruction, flags);
				break;
			case ImpliedAccess.t_ins:
				CommandIns(instruction, flags);
				break;
			case ImpliedAccess.t_outs:
				CommandOuts(instruction, flags);
				break;
			case ImpliedAccess.t_lea:
				CommandLea(instruction, flags);
				break;
			case ImpliedAccess.t_gpr16:
				CommandLastGpr(instruction, flags, Register.AX);
				break;
			case ImpliedAccess.t_poprm2:
				CommandPopRm(instruction, flags, 2);
				break;
			case ImpliedAccess.t_poprm4:
				CommandPopRm(instruction, flags, 4);
				break;
			case ImpliedAccess.t_poprm8:
				CommandPopRm(instruction, flags, 8);
				break;
			case ImpliedAccess.t_Ral_Wah:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.AL, OpAccess.Read);
					AddRegister(flags, Register.AH, OpAccess.Write);
				}
				break;
			case ImpliedAccess.t_Rax_Weax:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.AX, OpAccess.Read);
					AddRegister(flags, Register.EAX, OpAccess.Write);
				}
				break;
			case ImpliedAccess.t_RWeax:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.EAX, OpAccess.ReadWrite);
				}
				break;
			case ImpliedAccess.t_Rax_Wdx:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.AX, OpAccess.Read);
					AddRegister(flags, Register.DX, OpAccess.Write);
				}
				break;
			case ImpliedAccess.t_Reax_Wedx:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.EAX, OpAccess.Read);
					AddRegister(flags, Register.EDX, OpAccess.Write);
				}
				break;
			case ImpliedAccess.t_Rrax_Wrdx:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.RAX, OpAccess.Read);
					AddRegister(flags, Register.RDX, OpAccess.Write);
				}
				break;
			case ImpliedAccess.t_push2x2:
				CommandPush(instruction, flags, 2, 2);
				break;
			case ImpliedAccess.t_push2x4:
				CommandPush(instruction, flags, 2, 4);
				break;
			case ImpliedAccess.t_Rah:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.AH, OpAccess.Read);
				}
				break;
			case ImpliedAccess.t_Wah:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.AH, OpAccess.Write);
				}
				break;
			case ImpliedAccess.t_movs:
				CommandMovs(instruction, flags);
				break;
			case ImpliedAccess.t_cmps:
				CommandCmps(instruction, flags);
				break;
			case ImpliedAccess.t_stos:
				CommandStos(instruction, flags);
				break;
			case ImpliedAccess.t_lods:
				CommandLods(instruction, flags);
				break;
			case ImpliedAccess.t_scas:
				CommandScas(instruction, flags);
				break;
			case ImpliedAccess.t_Wes:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.ES, OpAccess.Write);
				}
				break;
			case ImpliedAccess.t_Wds:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.DS, OpAccess.Write);
				}
				break;
			case ImpliedAccess.t_CWeax:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.EAX, OpAccess.CondWrite);
				}
				break;
			case ImpliedAccess.t_enter2:
				CommandEnter(instruction, flags, 2);
				break;
			case ImpliedAccess.t_enter4:
				CommandEnter(instruction, flags, 4);
				break;
			case ImpliedAccess.t_enter8:
				CommandEnter(instruction, flags, 8);
				break;
			case ImpliedAccess.t_leave2:
				CommandLeave(instruction, flags, 2);
				break;
			case ImpliedAccess.t_leave4:
				CommandLeave(instruction, flags, 4);
				break;
			case ImpliedAccess.t_leave8:
				CommandLeave(instruction, flags, 8);
				break;
			case ImpliedAccess.t_pop2x2:
				CommandPop(instruction, flags, 2, 2);
				break;
			case ImpliedAccess.t_pop2x4:
				CommandPop(instruction, flags, 2, 4);
				break;
			case ImpliedAccess.t_pop2x8:
				CommandPop(instruction, flags, 2, 8);
				break;
			case ImpliedAccess.b64_t_Wss_pop5x2_f_pop3x2:
				if ((flags & Flags.Is64Bit) != 0) {
					if ((flags & Flags.NoRegisterUsage) == 0) {
						AddRegister(flags, Register.SS, OpAccess.Write);
					}
					CommandPop(instruction, flags, 5, 2);
				}
				else {
					CommandPop(instruction, flags, 3, 2);
				}
				break;
			case ImpliedAccess.b64_t_Wss_pop5x4_f_pop3x4:
				if ((flags & Flags.Is64Bit) != 0) {
					if ((flags & Flags.NoRegisterUsage) == 0) {
						AddRegister(flags, Register.SS, OpAccess.Write);
					}
					CommandPop(instruction, flags, 5, 4);
				}
				else {
					CommandPop(instruction, flags, 3, 4);
				}
				break;
			case ImpliedAccess.t_Wss_pop5x8:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.SS, OpAccess.Write);
				}
				CommandPop(instruction, flags, 5, 8);
				break;
			case ImpliedAccess.t_Ral_Wax:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.AL, OpAccess.Read);
					AddRegister(flags, Register.AX, OpAccess.Write);
				}
				break;
			case ImpliedAccess.t_Wal:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.AL, OpAccess.Write);
				}
				break;
			case ImpliedAccess.t_RWst0:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.ST0, OpAccess.ReadWrite);
				}
				break;
			case ImpliedAccess.t_Rst0:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.ST0, OpAccess.Read);
				}
				break;
			case ImpliedAccess.t_Rst0_RWst1:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.ST0, OpAccess.Read);
					AddRegister(flags, Register.ST1, OpAccess.ReadWrite);
				}
				break;
			case ImpliedAccess.t_RCWst0:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.ST0, OpAccess.ReadCondWrite);
				}
				break;
			case ImpliedAccess.t_Rst1_RWst0:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.ST1, OpAccess.Read);
					AddRegister(flags, Register.ST0, OpAccess.ReadWrite);
				}
				break;
			case ImpliedAccess.t_Rst0_Rst1:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.ST0, OpAccess.Read);
					AddRegister(flags, Register.ST1, OpAccess.Read);
				}
				break;
			case ImpliedAccess.t_Wst0TOst7_Wmm0TOmm7:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					for (var reg = Register.ST0; reg <= Register.ST7; reg++)
						AddRegister(flags, reg, OpAccess.Write);
					for (var reg = Register.MM0; reg <= Register.MM7; reg++)
						AddRegister(flags, reg, OpAccess.Write);
				}
				break;
			case ImpliedAccess.t_Rst0TOst7_Rmm0TOmm7:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					for (var reg = Register.ST0; reg <= Register.ST7; reg++)
						AddRegister(flags, reg, OpAccess.Read);
					for (var reg = Register.MM0; reg <= Register.MM7; reg++)
						AddRegister(flags, reg, OpAccess.Read);
				}
				break;
			case ImpliedAccess.t_RWcx:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.CX, OpAccess.ReadWrite);
				}
				break;
			case ImpliedAccess.t_RWecx:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.ECX, OpAccess.ReadWrite);
				}
				break;
			case ImpliedAccess.t_RWrcx:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.RCX, OpAccess.ReadWrite);
				}
				break;
			case ImpliedAccess.t_Rcx:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.CX, OpAccess.Read);
				}
				break;
			case ImpliedAccess.t_Recx:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.ECX, OpAccess.Read);
				}
				break;
			case ImpliedAccess.t_Rrcx:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.RCX, OpAccess.Read);
				}
				break;
			case ImpliedAccess.t_Wdx_RWax:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.DX, OpAccess.Write);
					AddRegister(flags, Register.AX, OpAccess.ReadWrite);
				}
				break;
			case ImpliedAccess.t_Wedx_RWeax:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.EDX, OpAccess.Write);
					AddRegister(flags, Register.EAX, OpAccess.ReadWrite);
				}
				break;
			case ImpliedAccess.t_Wrdx_RWrax:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.RDX, OpAccess.Write);
					AddRegister(flags, Register.RAX, OpAccess.ReadWrite);
				}
				break;
			case ImpliedAccess.t_RWax_RWdx:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.AX, OpAccess.ReadWrite);
					AddRegister(flags, Register.DX, OpAccess.ReadWrite);
				}
				break;
			case ImpliedAccess.t_RWeax_RWedx:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.EAX, OpAccess.ReadWrite);
					AddRegister(flags, Register.EDX, OpAccess.ReadWrite);
				}
				break;
			case ImpliedAccess.t_RWrax_RWrdx:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.RAX, OpAccess.ReadWrite);
					AddRegister(flags, Register.RDX, OpAccess.ReadWrite);
				}
				break;
			case ImpliedAccess.t_push2x8:
				CommandPush(instruction, flags, 2, 8);
				break;
			case ImpliedAccess.t_Rcr0:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.CR0, OpAccess.Read);
				}
				break;
			case ImpliedAccess.t_RWcr0:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.CR0, OpAccess.ReadWrite);
				}
				break;
			case ImpliedAccess.t_gpr16_RWcr0:
				CommandLastGpr(instruction, flags, Register.AX);
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.CR0, OpAccess.ReadWrite);
				}
				break;
			case ImpliedAccess.t_RCWeax_b64_t_CRrcx_CRrdx_CRrbx_CWrcx_CWrdx_CWrbx_f_CRecx_CRedx_CRebx_CRds_CWecx_CWedx_CWebx:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.EAX, OpAccess.ReadCondWrite);
				}
				if ((flags & Flags.Is64Bit) != 0) {
					if ((flags & Flags.NoRegisterUsage) == 0) {
						AddRegister(flags, Register.RCX, OpAccess.CondRead);
						AddRegister(flags, Register.RDX, OpAccess.CondRead);
						AddRegister(flags, Register.RBX, OpAccess.CondRead);
						AddRegister(flags, Register.RCX, OpAccess.CondWrite);
						AddRegister(flags, Register.RDX, OpAccess.CondWrite);
						AddRegister(flags, Register.RBX, OpAccess.CondWrite);
					}
				}
				else {
					if ((flags & Flags.NoRegisterUsage) == 0) {
						AddRegister(flags, Register.ECX, OpAccess.CondRead);
						AddRegister(flags, Register.EDX, OpAccess.CondRead);
						AddRegister(flags, Register.EBX, OpAccess.CondRead);
						AddRegister(flags, Register.DS, OpAccess.CondRead);
						AddRegister(flags, Register.ECX, OpAccess.CondWrite);
						AddRegister(flags, Register.EDX, OpAccess.CondWrite);
						AddRegister(flags, Register.EBX, OpAccess.CondWrite);
					}
				}
				break;
			case ImpliedAccess.t_CWecx_CWedx_CWebx_RWeax_b64_t_CRrcx_CRrdx_CRrbx_f_CRecx_CRedx_CRebx_CRds:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.ECX, OpAccess.CondWrite);
					AddRegister(flags, Register.EDX, OpAccess.CondWrite);
					AddRegister(flags, Register.EBX, OpAccess.CondWrite);
					AddRegister(flags, Register.EAX, OpAccess.ReadWrite);
				}
				if ((flags & Flags.Is64Bit) != 0) {
					if ((flags & Flags.NoRegisterUsage) == 0) {
						AddRegister(flags, Register.RCX, OpAccess.CondRead);
						AddRegister(flags, Register.RDX, OpAccess.CondRead);
						AddRegister(flags, Register.RBX, OpAccess.CondRead);
					}
				}
				else {
					if ((flags & Flags.NoRegisterUsage) == 0) {
						AddRegister(flags, Register.ECX, OpAccess.CondRead);
						AddRegister(flags, Register.EDX, OpAccess.CondRead);
						AddRegister(flags, Register.EBX, OpAccess.CondRead);
						AddRegister(flags, Register.DS, OpAccess.CondRead);
					}
				}
				break;
			case ImpliedAccess.t_Rax_Recx_Redx_Rseg:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.AX, OpAccess.Read);
					AddRegister(flags, Register.ECX, OpAccess.Read);
					AddRegister(flags, Register.EDX, OpAccess.Read);
					AddMemorySegmentRegister(flags, GetSegDefaultDS(instruction), OpAccess.Read);
				}
				break;
			case ImpliedAccess.t_Reax_Recx_Redx_Rseg:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.EAX, OpAccess.Read);
					AddRegister(flags, Register.ECX, OpAccess.Read);
					AddRegister(flags, Register.EDX, OpAccess.Read);
					AddMemorySegmentRegister(flags, GetSegDefaultDS(instruction), OpAccess.Read);
				}
				break;
			case ImpliedAccess.t_Recx_Redx_Rrax_Rseg:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.ECX, OpAccess.Read);
					AddRegister(flags, Register.EDX, OpAccess.Read);
					AddRegister(flags, Register.RAX, OpAccess.Read);
					AddMemorySegmentRegister(flags, GetSegDefaultDS(instruction), OpAccess.Read);
				}
				break;
			case ImpliedAccess.t_Reax_Recx:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.EAX, OpAccess.Read);
					AddRegister(flags, Register.ECX, OpAccess.Read);
				}
				break;
			case ImpliedAccess.t_Recx_Weax_Wedx:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.ECX, OpAccess.Read);
					AddRegister(flags, Register.EAX, OpAccess.Write);
					AddRegister(flags, Register.EDX, OpAccess.Write);
				}
				break;
			case ImpliedAccess.t_Reax_Recx_Redx:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.EAX, OpAccess.Read);
					AddRegister(flags, Register.ECX, OpAccess.Read);
					AddRegister(flags, Register.EDX, OpAccess.Read);
				}
				break;
			case ImpliedAccess.t_Rax:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.AX, OpAccess.Read);
				}
				break;
			case ImpliedAccess.t_Reax:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.EAX, OpAccess.Read);
				}
				break;
			case ImpliedAccess.t_Rrax:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.RAX, OpAccess.Read);
				}
				break;
			case ImpliedAccess.t_Rax_Wfs_Wgs:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.AX, OpAccess.Read);
					AddRegister(flags, Register.FS, OpAccess.Write);
					AddRegister(flags, Register.GS, OpAccess.Write);
				}
				break;
			case ImpliedAccess.t_Reax_Wfs_Wgs:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.EAX, OpAccess.Read);
					AddRegister(flags, Register.FS, OpAccess.Write);
					AddRegister(flags, Register.GS, OpAccess.Write);
				}
				break;
			case ImpliedAccess.t_Rrax_Wfs_Wgs:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.RAX, OpAccess.Read);
					AddRegister(flags, Register.FS, OpAccess.Write);
					AddRegister(flags, Register.GS, OpAccess.Write);
				}
				break;
			case ImpliedAccess.t_Rax_Rfs_Rgs:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.AX, OpAccess.Read);
					AddRegister(flags, Register.FS, OpAccess.Read);
					AddRegister(flags, Register.GS, OpAccess.Read);
				}
				break;
			case ImpliedAccess.t_Reax_Rfs_Rgs:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.EAX, OpAccess.Read);
					AddRegister(flags, Register.FS, OpAccess.Read);
					AddRegister(flags, Register.GS, OpAccess.Read);
				}
				break;
			case ImpliedAccess.t_Rrax_Rfs_Rgs:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.RAX, OpAccess.Read);
					AddRegister(flags, Register.FS, OpAccess.Read);
					AddRegister(flags, Register.GS, OpAccess.Read);
				}
				break;
			case ImpliedAccess.t_Reax_Wcr0_Wdr6_Wdr7_WesTOgs_Wcr2TOcr4_Wdr0TOdr3_b64_t_WraxTOr15_f_WeaxTOedi:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.EAX, OpAccess.Read);
					AddRegister(flags, Register.CR0, OpAccess.Write);
					AddRegister(flags, Register.DR6, OpAccess.Write);
					AddRegister(flags, Register.DR7, OpAccess.Write);
					for (var reg = Register.ES; reg <= Register.GS; reg++)
						AddRegister(flags, reg, OpAccess.Write);
					for (var reg = Register.CR2; reg <= Register.CR4; reg++)
						AddRegister(flags, reg, OpAccess.Write);
					for (var reg = Register.DR0; reg <= Register.DR3; reg++)
						AddRegister(flags, reg, OpAccess.Write);
				}
				if ((flags & Flags.Is64Bit) != 0) {
					if ((flags & Flags.NoRegisterUsage) == 0) {
						for (var reg = Register.RAX; reg <= Register.R15; reg++)
							AddRegister(flags, reg, OpAccess.Write);
					}
				}
				else {
					if ((flags & Flags.NoRegisterUsage) == 0) {
						for (var reg = Register.EAX; reg <= Register.EDI; reg++)
							AddRegister(flags, reg, OpAccess.Write);
					}
				}
				break;
			case ImpliedAccess.t_Rax_Recx:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.AX, OpAccess.Read);
					AddRegister(flags, Register.ECX, OpAccess.Read);
				}
				break;
			case ImpliedAccess.t_Recx_Rrax:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.ECX, OpAccess.Read);
					AddRegister(flags, Register.RAX, OpAccess.Read);
				}
				break;
			case ImpliedAccess.t_Weax_Wecx_Wedx:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.EAX, OpAccess.Write);
					AddRegister(flags, Register.ECX, OpAccess.Write);
					AddRegister(flags, Register.EDX, OpAccess.Write);
				}
				break;
			case ImpliedAccess.t_Reax_Recx_CRebx:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.EAX, OpAccess.Read);
					AddRegister(flags, Register.ECX, OpAccess.Read);
					AddRegister(flags, Register.EBX, OpAccess.CondRead);
				}
				break;
			case ImpliedAccess.t_Rax_Rseg:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.AX, OpAccess.Read);
					AddMemorySegmentRegister(flags, GetSegDefaultDS(instruction), OpAccess.Read);
				}
				break;
			case ImpliedAccess.t_Reax_Rseg:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.EAX, OpAccess.Read);
					AddMemorySegmentRegister(flags, GetSegDefaultDS(instruction), OpAccess.Read);
				}
				break;
			case ImpliedAccess.t_Rrax_Rseg:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.RAX, OpAccess.Read);
					AddMemorySegmentRegister(flags, GetSegDefaultDS(instruction), OpAccess.Read);
				}
				break;
			case ImpliedAccess.t_Wecx_b64_t_Wr11:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.ECX, OpAccess.Write);
				}
				if ((flags & Flags.Is64Bit) != 0) {
					if ((flags & Flags.NoRegisterUsage) == 0) {
						AddRegister(flags, Register.R11, OpAccess.Write);
					}
				}
				break;
			case ImpliedAccess.t_Redi_Res:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.EDI, OpAccess.Read);
					AddRegister(flags, Register.ES, OpAccess.Read);
				}
				break;
			case ImpliedAccess.t_Recx_Wcs_Wss_b64_t_Rr11d:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.ECX, OpAccess.Read);
					AddRegister(flags, Register.CS, OpAccess.Write);
					AddRegister(flags, Register.SS, OpAccess.Write);
				}
				if ((flags & Flags.Is64Bit) != 0) {
					if ((flags & Flags.NoRegisterUsage) == 0) {
						AddRegister(flags, Register.R11D, OpAccess.Read);
					}
				}
				break;
			case ImpliedAccess.t_Rr11d_Rrcx_Wcs_Wss:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.R11D, OpAccess.Read);
					AddRegister(flags, Register.RCX, OpAccess.Read);
					AddRegister(flags, Register.CS, OpAccess.Write);
					AddRegister(flags, Register.SS, OpAccess.Write);
				}
				break;
			case ImpliedAccess.t_Weax_Wedx:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.EAX, OpAccess.Write);
					AddRegister(flags, Register.EDX, OpAccess.Write);
				}
				break;
			case ImpliedAccess.t_Wesp:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.ESP, OpAccess.Write);
				}
				break;
			case ImpliedAccess.t_Recx_Redx_Wesp_Wcs_Wss:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.ECX, OpAccess.Read);
					AddRegister(flags, Register.EDX, OpAccess.Read);
					AddRegister(flags, Register.ESP, OpAccess.Write);
					AddRegister(flags, Register.CS, OpAccess.Write);
					AddRegister(flags, Register.SS, OpAccess.Write);
				}
				break;
			case ImpliedAccess.t_Rrcx_Rrdx_Wrsp_Wcs_Wss:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.RCX, OpAccess.Read);
					AddRegister(flags, Register.RDX, OpAccess.Read);
					AddRegister(flags, Register.RSP, OpAccess.Write);
					AddRegister(flags, Register.CS, OpAccess.Write);
					AddRegister(flags, Register.SS, OpAccess.Write);
				}
				break;
			case ImpliedAccess.t_zrrm:
				CommandClearRegRegmem(instruction, flags);
				break;
			case ImpliedAccess.t_zrrrm:
				CommandClearRegRegRegmem(instruction, flags);
				break;
			case ImpliedAccess.b64_t_RWxmm0TOxmm15_f_RWxmm0TOxmm7:
				if ((flags & Flags.Is64Bit) != 0) {
					if ((flags & Flags.NoRegisterUsage) == 0) {
						for (var reg = Register.XMM0; reg <= Register.XMM15; reg++)
							AddRegister(flags, reg, OpAccess.ReadWrite);
					}
				}
				else {
					if ((flags & Flags.NoRegisterUsage) == 0) {
						for (var reg = Register.XMM0; reg <= Register.XMM7; reg++)
							AddRegister(flags, reg, OpAccess.ReadWrite);
					}
				}
				break;
			case ImpliedAccess.b64_t_Wzmm0TOzmm15_f_Wzmm0TOzmm7:
				if ((flags & Flags.Is64Bit) != 0) {
					if ((flags & Flags.NoRegisterUsage) == 0) {
						for (var reg = Register.ZMM0; reg <= Register.ZMM15; reg++)
							AddRegister(flags, reg, OpAccess.Write);
					}
				}
				else {
					if ((flags & Flags.NoRegisterUsage) == 0) {
						for (var reg = Register.ZMM0; reg <= Register.ZMM7; reg++)
							AddRegister(flags, reg, OpAccess.Write);
					}
				}
				break;
			case ImpliedAccess.t_CRecx_Wecx_Wedx_Webx_RWeax:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.ECX, OpAccess.CondRead);
					AddRegister(flags, Register.ECX, OpAccess.Write);
					AddRegister(flags, Register.EDX, OpAccess.Write);
					AddRegister(flags, Register.EBX, OpAccess.Write);
					AddRegister(flags, Register.EAX, OpAccess.ReadWrite);
				}
				break;
			case ImpliedAccess.t_CRmem_CRsi_CReax_CRes_CWeax_CWedx_RCWecx:
				if ((flags & Flags.NoMemoryUsage) == 0) {
					AddMemory(Register.ES, Register.SI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code16, 0);
				}
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.SI, OpAccess.CondRead);
					AddRegister(flags, Register.EAX, OpAccess.CondRead);
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, Register.ES, OpAccess.CondRead);
					AddRegister(flags, Register.EAX, OpAccess.CondWrite);
					AddRegister(flags, Register.EDX, OpAccess.CondWrite);
					AddRegister(flags, Register.ECX, OpAccess.ReadCondWrite);
				}
				break;
			case ImpliedAccess.t_CRmem_CReax_CResi_CRes_CWeax_CWedx_RCWecx:
				if ((flags & Flags.NoMemoryUsage) == 0) {
					AddMemory(Register.ES, Register.ESI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code32, 0);
				}
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.EAX, OpAccess.CondRead);
					AddRegister(flags, Register.ESI, OpAccess.CondRead);
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, Register.ES, OpAccess.CondRead);
					AddRegister(flags, Register.EAX, OpAccess.CondWrite);
					AddRegister(flags, Register.EDX, OpAccess.CondWrite);
					AddRegister(flags, Register.ECX, OpAccess.ReadCondWrite);
				}
				break;
			case ImpliedAccess.t_CRmem_CReax_CRrsi_CRes_CWeax_CWedx_RCWrcx:
				if ((flags & Flags.NoMemoryUsage) == 0) {
					AddMemory(Register.ES, Register.RSI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code64, 0);
				}
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.EAX, OpAccess.CondRead);
					AddRegister(flags, Register.RSI, OpAccess.CondRead);
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, Register.ES, OpAccess.CondRead);
					AddRegister(flags, Register.EAX, OpAccess.CondWrite);
					AddRegister(flags, Register.EDX, OpAccess.CondWrite);
					AddRegister(flags, Register.RCX, OpAccess.ReadCondWrite);
				}
				break;
			case ImpliedAccess.t_CRmem_CRmem_CWmem_CRsi_CRdi_CRes_CWsi_RCWax_RCWcx:
				if ((flags & Flags.NoMemoryUsage) == 0) {
					AddMemory(Register.ES, Register.SI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code16, 0);
					AddMemory(Register.ES, Register.DI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code16, 0);
					AddMemory(Register.ES, Register.DI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondWrite, CodeSize.Code16, 0);
				}
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.SI, OpAccess.CondRead);
					AddRegister(flags, Register.DI, OpAccess.CondRead);
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, Register.ES, OpAccess.CondRead);
					AddRegister(flags, Register.SI, OpAccess.CondWrite);
					AddRegister(flags, Register.AX, OpAccess.ReadCondWrite);
					AddRegister(flags, Register.CX, OpAccess.ReadCondWrite);
				}
				break;
			case ImpliedAccess.t_CRmem_CRmem_CWmem_CResi_CRedi_CRes_CWesi_RCWeax_RCWecx:
				if ((flags & Flags.NoMemoryUsage) == 0) {
					AddMemory(Register.ES, Register.ESI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code32, 0);
					AddMemory(Register.ES, Register.EDI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code32, 0);
					AddMemory(Register.ES, Register.EDI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondWrite, CodeSize.Code32, 0);
				}
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.ESI, OpAccess.CondRead);
					AddRegister(flags, Register.EDI, OpAccess.CondRead);
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, Register.ES, OpAccess.CondRead);
					AddRegister(flags, Register.ESI, OpAccess.CondWrite);
					AddRegister(flags, Register.EAX, OpAccess.ReadCondWrite);
					AddRegister(flags, Register.ECX, OpAccess.ReadCondWrite);
				}
				break;
			case ImpliedAccess.t_CRmem_CRmem_CWmem_CRrsi_CRrdi_CRes_CWrsi_RCWrax_RCWrcx:
				if ((flags & Flags.NoMemoryUsage) == 0) {
					AddMemory(Register.ES, Register.RSI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code64, 0);
					AddMemory(Register.ES, Register.RDI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code64, 0);
					AddMemory(Register.ES, Register.RDI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondWrite, CodeSize.Code64, 0);
				}
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.RSI, OpAccess.CondRead);
					AddRegister(flags, Register.RDI, OpAccess.CondRead);
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, Register.ES, OpAccess.CondRead);
					AddRegister(flags, Register.RSI, OpAccess.CondWrite);
					AddRegister(flags, Register.RAX, OpAccess.ReadCondWrite);
					AddRegister(flags, Register.RCX, OpAccess.ReadCondWrite);
				}
				break;
			case ImpliedAccess.t_Rcl_Rax:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.CL, OpAccess.Read);
					AddRegister(flags, Register.AX, OpAccess.Read);
				}
				break;
			case ImpliedAccess.t_Rcl_Reax:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.CL, OpAccess.Read);
					AddRegister(flags, Register.EAX, OpAccess.Read);
				}
				break;
			case ImpliedAccess.t_xstore2:
				CommandXstore(instruction, flags, 2);
				break;
			case ImpliedAccess.t_xstore4:
				CommandXstore(instruction, flags, 4);
				break;
			case ImpliedAccess.t_xstore8:
				CommandXstore(instruction, flags, 8);
				break;
			case ImpliedAccess.t_CRmem_CRmem_CRmem_CWmem_CRdx_CRbx_CRsi_CRdi_CRes_CWsi_CWdi_RCWcx:
				if ((flags & Flags.NoMemoryUsage) == 0) {
					AddMemory(Register.ES, Register.DX, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code16, 0);
					AddMemory(Register.ES, Register.BX, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code16, 0);
					AddMemory(Register.ES, Register.SI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code16, 0);
					AddMemory(Register.ES, Register.DI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondWrite, CodeSize.Code16, 0);
				}
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.DX, OpAccess.CondRead);
					AddRegister(flags, Register.BX, OpAccess.CondRead);
					AddRegister(flags, Register.SI, OpAccess.CondRead);
					AddRegister(flags, Register.DI, OpAccess.CondRead);
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, Register.ES, OpAccess.CondRead);
					AddRegister(flags, Register.SI, OpAccess.CondWrite);
					AddRegister(flags, Register.DI, OpAccess.CondWrite);
					AddRegister(flags, Register.CX, OpAccess.ReadCondWrite);
				}
				break;
			case ImpliedAccess.t_CRmem_CRmem_CRmem_CWmem_CRedx_CRebx_CResi_CRedi_CRes_CWesi_CWedi_RCWecx:
				if ((flags & Flags.NoMemoryUsage) == 0) {
					AddMemory(Register.ES, Register.EDX, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code32, 0);
					AddMemory(Register.ES, Register.EBX, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code32, 0);
					AddMemory(Register.ES, Register.ESI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code32, 0);
					AddMemory(Register.ES, Register.EDI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondWrite, CodeSize.Code32, 0);
				}
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.EDX, OpAccess.CondRead);
					AddRegister(flags, Register.EBX, OpAccess.CondRead);
					AddRegister(flags, Register.ESI, OpAccess.CondRead);
					AddRegister(flags, Register.EDI, OpAccess.CondRead);
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, Register.ES, OpAccess.CondRead);
					AddRegister(flags, Register.ESI, OpAccess.CondWrite);
					AddRegister(flags, Register.EDI, OpAccess.CondWrite);
					AddRegister(flags, Register.ECX, OpAccess.ReadCondWrite);
				}
				break;
			case ImpliedAccess.t_CRmem_CRmem_CRmem_CWmem_CRrdx_CRrbx_CRrsi_CRrdi_CRes_CWrsi_CWrdi_RCWrcx:
				if ((flags & Flags.NoMemoryUsage) == 0) {
					AddMemory(Register.ES, Register.RDX, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code64, 0);
					AddMemory(Register.ES, Register.RBX, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code64, 0);
					AddMemory(Register.ES, Register.RSI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code64, 0);
					AddMemory(Register.ES, Register.RDI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondWrite, CodeSize.Code64, 0);
				}
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.RDX, OpAccess.CondRead);
					AddRegister(flags, Register.RBX, OpAccess.CondRead);
					AddRegister(flags, Register.RSI, OpAccess.CondRead);
					AddRegister(flags, Register.RDI, OpAccess.CondRead);
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, Register.ES, OpAccess.CondRead);
					AddRegister(flags, Register.RSI, OpAccess.CondWrite);
					AddRegister(flags, Register.RDI, OpAccess.CondWrite);
					AddRegister(flags, Register.RCX, OpAccess.ReadCondWrite);
				}
				break;
			case ImpliedAccess.t_CRmem_CRmem_CRmem_CRmem_CWmem_CWmem_CRax_CRdx_CRbx_CRsi_CRdi_CRes_CWsi_CWdi_RCWcx:
				if ((flags & Flags.NoMemoryUsage) == 0) {
					AddMemory(Register.ES, Register.AX, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code16, 0);
					AddMemory(Register.ES, Register.DX, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code16, 0);
					AddMemory(Register.ES, Register.BX, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code16, 0);
					AddMemory(Register.ES, Register.SI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code16, 0);
					AddMemory(Register.ES, Register.AX, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondWrite, CodeSize.Code16, 0);
					AddMemory(Register.ES, Register.DI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondWrite, CodeSize.Code16, 0);
				}
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.AX, OpAccess.CondRead);
					AddRegister(flags, Register.DX, OpAccess.CondRead);
					AddRegister(flags, Register.BX, OpAccess.CondRead);
					AddRegister(flags, Register.SI, OpAccess.CondRead);
					AddRegister(flags, Register.DI, OpAccess.CondRead);
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, Register.ES, OpAccess.CondRead);
					AddRegister(flags, Register.SI, OpAccess.CondWrite);
					AddRegister(flags, Register.DI, OpAccess.CondWrite);
					AddRegister(flags, Register.CX, OpAccess.ReadCondWrite);
				}
				break;
			case ImpliedAccess.t_CRmem_CRmem_CRmem_CRmem_CWmem_CWmem_CReax_CRedx_CRebx_CResi_CRedi_CRes_CWesi_CWedi_RCWecx:
				if ((flags & Flags.NoMemoryUsage) == 0) {
					AddMemory(Register.ES, Register.EAX, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code32, 0);
					AddMemory(Register.ES, Register.EDX, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code32, 0);
					AddMemory(Register.ES, Register.EBX, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code32, 0);
					AddMemory(Register.ES, Register.ESI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code32, 0);
					AddMemory(Register.ES, Register.EAX, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondWrite, CodeSize.Code32, 0);
					AddMemory(Register.ES, Register.EDI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondWrite, CodeSize.Code32, 0);
				}
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.EAX, OpAccess.CondRead);
					AddRegister(flags, Register.EDX, OpAccess.CondRead);
					AddRegister(flags, Register.EBX, OpAccess.CondRead);
					AddRegister(flags, Register.ESI, OpAccess.CondRead);
					AddRegister(flags, Register.EDI, OpAccess.CondRead);
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, Register.ES, OpAccess.CondRead);
					AddRegister(flags, Register.ESI, OpAccess.CondWrite);
					AddRegister(flags, Register.EDI, OpAccess.CondWrite);
					AddRegister(flags, Register.ECX, OpAccess.ReadCondWrite);
				}
				break;
			case ImpliedAccess.t_CRmem_CRmem_CRmem_CRmem_CWmem_CWmem_CRrax_CRrdx_CRrbx_CRrsi_CRrdi_CRes_CWrsi_CWrdi_RCWrcx:
				if ((flags & Flags.NoMemoryUsage) == 0) {
					AddMemory(Register.ES, Register.RAX, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code64, 0);
					AddMemory(Register.ES, Register.RDX, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code64, 0);
					AddMemory(Register.ES, Register.RBX, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code64, 0);
					AddMemory(Register.ES, Register.RSI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code64, 0);
					AddMemory(Register.ES, Register.RAX, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondWrite, CodeSize.Code64, 0);
					AddMemory(Register.ES, Register.RDI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondWrite, CodeSize.Code64, 0);
				}
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.RAX, OpAccess.CondRead);
					AddRegister(flags, Register.RDX, OpAccess.CondRead);
					AddRegister(flags, Register.RBX, OpAccess.CondRead);
					AddRegister(flags, Register.RSI, OpAccess.CondRead);
					AddRegister(flags, Register.RDI, OpAccess.CondRead);
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, Register.ES, OpAccess.CondRead);
					AddRegister(flags, Register.RSI, OpAccess.CondWrite);
					AddRegister(flags, Register.RDI, OpAccess.CondWrite);
					AddRegister(flags, Register.RCX, OpAccess.ReadCondWrite);
				}
				break;
			case ImpliedAccess.t_RCWal:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.AL, OpAccess.ReadCondWrite);
				}
				break;
			case ImpliedAccess.t_RCWax:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.AX, OpAccess.ReadCondWrite);
				}
				break;
			case ImpliedAccess.t_RCWeax:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.EAX, OpAccess.ReadCondWrite);
				}
				break;
			case ImpliedAccess.t_Reax_Redx:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.EAX, OpAccess.Read);
					AddRegister(flags, Register.EDX, OpAccess.Read);
				}
				break;
			case ImpliedAccess.t_gpr8:
				CommandLastGpr(instruction, flags, Register.AL);
				break;
			case ImpliedAccess.t_gpr32_Reax_Redx:
				CommandLastGpr(instruction, flags, Register.EAX);
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.EAX, OpAccess.Read);
					AddRegister(flags, Register.EDX, OpAccess.Read);
				}
				break;
			case ImpliedAccess.t_Rmem_Rseg:
				if ((flags & Flags.NoMemoryUsage) == 0) {
					AddMemory(GetSegDefaultDS(instruction), instruction.Op0Register, Register.None, 1, 0x0, MemorySize.UInt8, OpAccess.Read, CodeSize.Unknown, 0);
				}
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddMemorySegmentRegister(flags, GetSegDefaultDS(instruction), OpAccess.Read);
				}
				break;
			case ImpliedAccess.t_RCWrax:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.RAX, OpAccess.ReadCondWrite);
				}
				break;
			case ImpliedAccess.t_Wss:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.SS, OpAccess.Write);
				}
				break;
			case ImpliedAccess.t_Wfs:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.FS, OpAccess.Write);
				}
				break;
			case ImpliedAccess.t_Wgs:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.GS, OpAccess.Write);
				}
				break;
			case ImpliedAccess.t_CRecx_CRebx_RCWeax_RCWedx:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.ECX, OpAccess.CondRead);
					AddRegister(flags, Register.EBX, OpAccess.CondRead);
					AddRegister(flags, Register.EAX, OpAccess.ReadCondWrite);
					AddRegister(flags, Register.EDX, OpAccess.ReadCondWrite);
				}
				break;
			case ImpliedAccess.t_CRrcx_CRrbx_RCWrax_RCWrdx:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.RCX, OpAccess.CondRead);
					AddRegister(flags, Register.RBX, OpAccess.CondRead);
					AddRegister(flags, Register.RAX, OpAccess.ReadCondWrite);
					AddRegister(flags, Register.RDX, OpAccess.ReadCondWrite);
				}
				break;
			case ImpliedAccess.t_Wmem_RarDI_Rseg:
				if ((flags & Flags.NoMemoryUsage) == 0) {
					AddMemory(GetSegDefaultDS(instruction), GetARDI(instruction), Register.None, 1, 0x0, instruction.MemorySize, OpAccess.Write, CodeSize.Unknown, 0);
				}
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, GetARDI(instruction), OpAccess.Read);
					AddMemorySegmentRegister(flags, GetSegDefaultDS(instruction), OpAccess.Read);
				}
				break;
			case ImpliedAccess.t_Rxmm0:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.XMM0, OpAccess.Read);
				}
				break;
			case ImpliedAccess.t_Redx:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.EDX, OpAccess.Read);
				}
				break;
			case ImpliedAccess.t_Rrdx:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.RDX, OpAccess.Read);
				}
				break;
			case ImpliedAccess.t_Wmem_Res:
				if ((flags & Flags.NoMemoryUsage) == 0) {
					AddMemory(Register.ES, instruction.Op0Register, Register.None, 1, 0x0, instruction.MemorySize, OpAccess.Write, CodeSize.Unknown, 0);
				}
				if ((flags & Flags.NoRegisterUsage) == 0) {
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, Register.ES, OpAccess.Read);
				}
				break;
			case ImpliedAccess.t_Reax_Redx_Wxmm0:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.EAX, OpAccess.Read);
					AddRegister(flags, Register.EDX, OpAccess.Read);
					AddRegister(flags, Register.XMM0, OpAccess.Write);
				}
				break;
			case ImpliedAccess.t_Rrax_Rrdx_Wxmm0:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.RAX, OpAccess.Read);
					AddRegister(flags, Register.RDX, OpAccess.Read);
					AddRegister(flags, Register.XMM0, OpAccess.Write);
				}
				break;
			case ImpliedAccess.t_Reax_Redx_Wecx:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.EAX, OpAccess.Read);
					AddRegister(flags, Register.EDX, OpAccess.Read);
					AddRegister(flags, Register.ECX, OpAccess.Write);
				}
				break;
			case ImpliedAccess.t_Rrax_Rrdx_Wecx:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.RAX, OpAccess.Read);
					AddRegister(flags, Register.RDX, OpAccess.Read);
					AddRegister(flags, Register.ECX, OpAccess.Write);
				}
				break;
			case ImpliedAccess.t_Wxmm0:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.XMM0, OpAccess.Write);
				}
				break;
			case ImpliedAccess.t_Wecx:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.ECX, OpAccess.Write);
				}
				break;
			case ImpliedAccess.t_Rmem_Rds:
				if ((flags & Flags.NoMemoryUsage) == 0) {
					AddMemory(Register.DS, instruction.Op0Register, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.Read, CodeSize.Unknown, 0);
				}
				if ((flags & Flags.NoRegisterUsage) == 0) {
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, Register.DS, OpAccess.Read);
				}
				break;
			case ImpliedAccess.t_Rrcx_Rrdx_RWrax:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.RCX, OpAccess.Read);
					AddRegister(flags, Register.RDX, OpAccess.Read);
					AddRegister(flags, Register.RAX, OpAccess.ReadWrite);
				}
				break;
			case ImpliedAccess.t_Rmem_Rrcx_Rseg_RWrax:
				if ((flags & Flags.NoMemoryUsage) == 0) {
					AddMemory(GetSegDefaultDS(instruction), Register.RCX, Register.None, 1, 0x0, MemorySize.UInt128, OpAccess.Read, CodeSize.Code64, 0);
				}
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.RCX, OpAccess.Read);
					AddMemorySegmentRegister(flags, GetSegDefaultDS(instruction), OpAccess.Read);
					AddRegister(flags, Register.RAX, OpAccess.ReadWrite);
				}
				break;
			case ImpliedAccess.t_RWrax:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.RAX, OpAccess.ReadWrite);
				}
				break;
			case ImpliedAccess.t_Rax_Recx_Redx_Weax:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.AX, OpAccess.Read);
					AddRegister(flags, Register.ECX, OpAccess.Read);
					AddRegister(flags, Register.EDX, OpAccess.Read);
					AddRegister(flags, Register.EAX, OpAccess.Write);
				}
				break;
			case ImpliedAccess.t_Recx_Redx_RWeax:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.ECX, OpAccess.Read);
					AddRegister(flags, Register.EDX, OpAccess.Read);
					AddRegister(flags, Register.EAX, OpAccess.ReadWrite);
				}
				break;
			case ImpliedAccess.t_Recx_Redx_RWrax:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.ECX, OpAccess.Read);
					AddRegister(flags, Register.EDX, OpAccess.Read);
					AddRegister(flags, Register.RAX, OpAccess.ReadWrite);
				}
				break;
			case ImpliedAccess.t_Rax_Recx_Redx:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.AX, OpAccess.Read);
					AddRegister(flags, Register.ECX, OpAccess.Read);
					AddRegister(flags, Register.EDX, OpAccess.Read);
				}
				break;
			case ImpliedAccess.t_Recx_Redx_Rrax:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.ECX, OpAccess.Read);
					AddRegister(flags, Register.EDX, OpAccess.Read);
					AddRegister(flags, Register.RAX, OpAccess.Read);
				}
				break;
			case ImpliedAccess.t_Wtmm0TOtmm7:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					for (var reg = Register.TMM0; reg <= Register.TMM7; reg++)
						AddRegister(flags, reg, OpAccess.Write);
				}
				break;
			case ImpliedAccess.t_Reax_Rebx:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.EAX, OpAccess.Read);
					AddRegister(flags, Register.EBX, OpAccess.Read);
				}
				break;
			case ImpliedAccess.t_Rebx_Weax:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.EBX, OpAccess.Read);
					AddRegister(flags, Register.EAX, OpAccess.Write);
				}
				break;
			case ImpliedAccess.t_emmiW:
				CommandEmmi(instruction, flags, OpAccess.Write);
				break;
			case ImpliedAccess.t_emmiRW:
				CommandEmmi(instruction, flags, OpAccess.ReadWrite);
				break;
			case ImpliedAccess.t_emmiR:
				CommandEmmi(instruction, flags, OpAccess.Read);
				break;
			case ImpliedAccess.t_CRrcx_CRrdx_CRr8_CRr9_RWrax:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.RCX, OpAccess.CondRead);
					AddRegister(flags, Register.RDX, OpAccess.CondRead);
					AddRegister(flags, Register.R8, OpAccess.CondRead);
					AddRegister(flags, Register.R9, OpAccess.CondRead);
					AddRegister(flags, Register.RAX, OpAccess.ReadWrite);
				}
				break;
			case ImpliedAccess.t_RWxmm0TOxmm7:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					for (var reg = Register.XMM0; reg <= Register.XMM7; reg++)
						AddRegister(flags, reg, OpAccess.ReadWrite);
				}
				break;
			case ImpliedAccess.t_Reax_Rxmm0:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.EAX, OpAccess.Read);
					AddRegister(flags, Register.XMM0, OpAccess.Read);
				}
				break;
			case ImpliedAccess.t_Wxmm1_Wxmm2_RWxmm0_Wxmm4TOxmm6:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.XMM1, OpAccess.Write);
					AddRegister(flags, Register.XMM2, OpAccess.Write);
					AddRegister(flags, Register.XMM0, OpAccess.ReadWrite);
					for (var reg = Register.XMM4; reg <= Register.XMM6; reg++)
						AddRegister(flags, reg, OpAccess.Write);
				}
				break;
			case ImpliedAccess.t_RWxmm0_RWxmm1_Wxmm2TOxmm6:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.XMM0, OpAccess.ReadWrite);
					AddRegister(flags, Register.XMM1, OpAccess.ReadWrite);
					for (var reg = Register.XMM2; reg <= Register.XMM6; reg++)
						AddRegister(flags, reg, OpAccess.Write);
				}
				break;
			case ImpliedAccess.t_pop3x8:
				CommandPop(instruction, flags, 3, 8);
				break;
			case ImpliedAccess.t_CRmem_CRmem_CWmem_CRbx_CRsi_CRdi_CRes_CWsi_RCWax_RCWcx:
				if ((flags & Flags.NoMemoryUsage) == 0) {
					AddMemory(Register.ES, Register.SI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code16, 0);
					AddMemory(Register.ES, Register.DI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code16, 0);
					AddMemory(Register.ES, Register.DI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondWrite, CodeSize.Code16, 0);
				}
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.BX, OpAccess.CondRead);
					AddRegister(flags, Register.SI, OpAccess.CondRead);
					AddRegister(flags, Register.DI, OpAccess.CondRead);
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, Register.ES, OpAccess.CondRead);
					AddRegister(flags, Register.SI, OpAccess.CondWrite);
					AddRegister(flags, Register.AX, OpAccess.ReadCondWrite);
					AddRegister(flags, Register.CX, OpAccess.ReadCondWrite);
				}
				break;
			case ImpliedAccess.t_CRmem_CRmem_CWmem_CRebx_CResi_CRedi_CRes_CWesi_RCWeax_RCWecx:
				if ((flags & Flags.NoMemoryUsage) == 0) {
					AddMemory(Register.ES, Register.ESI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code32, 0);
					AddMemory(Register.ES, Register.EDI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code32, 0);
					AddMemory(Register.ES, Register.EDI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondWrite, CodeSize.Code32, 0);
				}
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.EBX, OpAccess.CondRead);
					AddRegister(flags, Register.ESI, OpAccess.CondRead);
					AddRegister(flags, Register.EDI, OpAccess.CondRead);
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, Register.ES, OpAccess.CondRead);
					AddRegister(flags, Register.ESI, OpAccess.CondWrite);
					AddRegister(flags, Register.EAX, OpAccess.ReadCondWrite);
					AddRegister(flags, Register.ECX, OpAccess.ReadCondWrite);
				}
				break;
			case ImpliedAccess.t_CRmem_CRmem_CWmem_CRrbx_CRrsi_CRrdi_CRes_CWrsi_RCWrax_RCWrcx:
				if ((flags & Flags.NoMemoryUsage) == 0) {
					AddMemory(Register.ES, Register.RSI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code64, 0);
					AddMemory(Register.ES, Register.RDI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code64, 0);
					AddMemory(Register.ES, Register.RDI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondWrite, CodeSize.Code64, 0);
				}
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.RBX, OpAccess.CondRead);
					AddRegister(flags, Register.RSI, OpAccess.CondRead);
					AddRegister(flags, Register.RDI, OpAccess.CondRead);
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, Register.ES, OpAccess.CondRead);
					AddRegister(flags, Register.RSI, OpAccess.CondWrite);
					AddRegister(flags, Register.RAX, OpAccess.ReadCondWrite);
					AddRegister(flags, Register.RCX, OpAccess.ReadCondWrite);
				}
				break;
			case ImpliedAccess.t_CRmem_CRmem_CRmem_CWmem_CRax_CRdx_CRbx_CRsi_CRdi_CRes_CWsi_CWdi_RCWcx:
				if ((flags & Flags.NoMemoryUsage) == 0) {
					AddMemory(Register.ES, Register.DX, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code16, 0);
					AddMemory(Register.ES, Register.BX, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code16, 0);
					AddMemory(Register.ES, Register.SI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code16, 0);
					AddMemory(Register.ES, Register.DI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondWrite, CodeSize.Code16, 0);
				}
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.AX, OpAccess.CondRead);
					AddRegister(flags, Register.DX, OpAccess.CondRead);
					AddRegister(flags, Register.BX, OpAccess.CondRead);
					AddRegister(flags, Register.SI, OpAccess.CondRead);
					AddRegister(flags, Register.DI, OpAccess.CondRead);
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, Register.ES, OpAccess.CondRead);
					AddRegister(flags, Register.SI, OpAccess.CondWrite);
					AddRegister(flags, Register.DI, OpAccess.CondWrite);
					AddRegister(flags, Register.CX, OpAccess.ReadCondWrite);
				}
				break;
			case ImpliedAccess.t_CRmem_CRmem_CRmem_CWmem_CReax_CRedx_CRebx_CResi_CRedi_CRes_CWesi_CWedi_RCWecx:
				if ((flags & Flags.NoMemoryUsage) == 0) {
					AddMemory(Register.ES, Register.EDX, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code32, 0);
					AddMemory(Register.ES, Register.EBX, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code32, 0);
					AddMemory(Register.ES, Register.ESI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code32, 0);
					AddMemory(Register.ES, Register.EDI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondWrite, CodeSize.Code32, 0);
				}
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.EAX, OpAccess.CondRead);
					AddRegister(flags, Register.EDX, OpAccess.CondRead);
					AddRegister(flags, Register.EBX, OpAccess.CondRead);
					AddRegister(flags, Register.ESI, OpAccess.CondRead);
					AddRegister(flags, Register.EDI, OpAccess.CondRead);
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, Register.ES, OpAccess.CondRead);
					AddRegister(flags, Register.ESI, OpAccess.CondWrite);
					AddRegister(flags, Register.EDI, OpAccess.CondWrite);
					AddRegister(flags, Register.ECX, OpAccess.ReadCondWrite);
				}
				break;
			case ImpliedAccess.t_CRmem_CRmem_CRmem_CWmem_CRrax_CRrdx_CRrbx_CRrsi_CRrdi_CRes_CWrsi_CWrdi_RCWrcx:
				if ((flags & Flags.NoMemoryUsage) == 0) {
					AddMemory(Register.ES, Register.RDX, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code64, 0);
					AddMemory(Register.ES, Register.RBX, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code64, 0);
					AddMemory(Register.ES, Register.RSI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code64, 0);
					AddMemory(Register.ES, Register.RDI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondWrite, CodeSize.Code64, 0);
				}
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.RAX, OpAccess.CondRead);
					AddRegister(flags, Register.RDX, OpAccess.CondRead);
					AddRegister(flags, Register.RBX, OpAccess.CondRead);
					AddRegister(flags, Register.RSI, OpAccess.CondRead);
					AddRegister(flags, Register.RDI, OpAccess.CondRead);
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, Register.ES, OpAccess.CondRead);
					AddRegister(flags, Register.RSI, OpAccess.CondWrite);
					AddRegister(flags, Register.RDI, OpAccess.CondWrite);
					AddRegister(flags, Register.RCX, OpAccess.ReadCondWrite);
				}
				break;
			case ImpliedAccess.t_gpr16_Wgs:
				CommandLastGpr(instruction, flags, Register.AX);
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.GS, OpAccess.Write);
				}
				break;
			case ImpliedAccess.t_Wrsp_Wcs_Wss_pop6x8:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.RSP, OpAccess.Write);
					AddRegister(flags, Register.CS, OpAccess.Write);
					AddRegister(flags, Register.SS, OpAccess.Write);
				}
				CommandPop(instruction, flags, 6, 8);
				break;
			case ImpliedAccess.t_Rcs_Rss_Wrsp_pop6x8:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.CS, OpAccess.Read);
					AddRegister(flags, Register.SS, OpAccess.Read);
					AddRegister(flags, Register.RSP, OpAccess.Write);
				}
				CommandPop(instruction, flags, 6, 8);
				break;
			case ImpliedAccess.t_Reax_Recx_Wedx_Webx:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.EAX, OpAccess.Read);
					AddRegister(flags, Register.ECX, OpAccess.Read);
					AddRegister(flags, Register.EDX, OpAccess.Write);
					AddRegister(flags, Register.EBX, OpAccess.Write);
				}
				break;
			case ImpliedAccess.t_Reax_Recx_Redx_CRebx_CWedx_CWebx:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.EAX, OpAccess.Read);
					AddRegister(flags, Register.ECX, OpAccess.Read);
					AddRegister(flags, Register.EDX, OpAccess.Read);
					AddRegister(flags, Register.EBX, OpAccess.CondRead);
					AddRegister(flags, Register.EDX, OpAccess.CondWrite);
					AddRegister(flags, Register.EBX, OpAccess.CondWrite);
				}
				break;
			case ImpliedAccess.t_memdisplm64:
				CommandMemDispl(flags, -64);
				break;
			case ImpliedAccess.t_CRmem_CRmem_CWmem_CRsi_CRdi_CRes_CWsi_RCWcx:
				if ((flags & Flags.NoMemoryUsage) == 0) {
					AddMemory(Register.ES, Register.SI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code16, 0);
					AddMemory(Register.ES, Register.DI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code16, 0);
					AddMemory(Register.ES, Register.DI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondWrite, CodeSize.Code16, 0);
				}
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.SI, OpAccess.CondRead);
					AddRegister(flags, Register.DI, OpAccess.CondRead);
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, Register.ES, OpAccess.CondRead);
					AddRegister(flags, Register.SI, OpAccess.CondWrite);
					AddRegister(flags, Register.CX, OpAccess.ReadCondWrite);
				}
				break;
			case ImpliedAccess.t_CRmem_CRmem_CWmem_CResi_CRedi_CRes_CWesi_RCWecx:
				if ((flags & Flags.NoMemoryUsage) == 0) {
					AddMemory(Register.ES, Register.ESI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code32, 0);
					AddMemory(Register.ES, Register.EDI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code32, 0);
					AddMemory(Register.ES, Register.EDI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondWrite, CodeSize.Code32, 0);
				}
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.ESI, OpAccess.CondRead);
					AddRegister(flags, Register.EDI, OpAccess.CondRead);
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, Register.ES, OpAccess.CondRead);
					AddRegister(flags, Register.ESI, OpAccess.CondWrite);
					AddRegister(flags, Register.ECX, OpAccess.ReadCondWrite);
				}
				break;
			case ImpliedAccess.t_CRmem_CRmem_CWmem_CRrsi_CRrdi_CRes_CWrsi_RCWrcx:
				if ((flags & Flags.NoMemoryUsage) == 0) {
					AddMemory(Register.ES, Register.RSI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code64, 0);
					AddMemory(Register.ES, Register.RDI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code64, 0);
					AddMemory(Register.ES, Register.RDI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondWrite, CodeSize.Code64, 0);
				}
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.RSI, OpAccess.CondRead);
					AddRegister(flags, Register.RDI, OpAccess.CondRead);
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, Register.ES, OpAccess.CondRead);
					AddRegister(flags, Register.RSI, OpAccess.CondWrite);
					AddRegister(flags, Register.RCX, OpAccess.ReadCondWrite);
				}
				break;
			case ImpliedAccess.t_CRmem_CRmem_Rrcx_CRrsi_CRrdi_CRes_CRds_CWrcx:
				if ((flags & Flags.NoMemoryUsage) == 0) {
					AddMemory(Register.ES, Register.RDI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code64, 0);
					AddMemory(Register.DS, Register.RSI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code64, 0);
				}
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.RCX, OpAccess.Read);
					AddRegister(flags, Register.RSI, OpAccess.CondRead);
					AddRegister(flags, Register.RDI, OpAccess.CondRead);
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, Register.ES, OpAccess.CondRead);
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, Register.DS, OpAccess.CondRead);
					AddRegister(flags, Register.RCX, OpAccess.CondWrite);
				}
				break;
			case ImpliedAccess.t_CRmem_CWmem_Rrcx_CRrsi_CRrdi_CRes_CRds_CWrcx:
				if ((flags & Flags.NoMemoryUsage) == 0) {
					AddMemory(Register.DS, Register.RSI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondRead, CodeSize.Code64, 0);
					AddMemory(Register.ES, Register.RDI, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.CondWrite, CodeSize.Code64, 0);
				}
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.RCX, OpAccess.Read);
					AddRegister(flags, Register.RSI, OpAccess.CondRead);
					AddRegister(flags, Register.RDI, OpAccess.CondRead);
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, Register.ES, OpAccess.CondRead);
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, Register.DS, OpAccess.CondRead);
					AddRegister(flags, Register.RCX, OpAccess.CondWrite);
				}
				break;
			case ImpliedAccess.t_Rdl_Rrax_Weax_Wrcx_Wrdx:
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.DL, OpAccess.Read);
					AddRegister(flags, Register.RAX, OpAccess.Read);
					AddRegister(flags, Register.EAX, OpAccess.Write);
					AddRegister(flags, Register.RCX, OpAccess.Write);
					AddRegister(flags, Register.RDX, OpAccess.Write);
				}
				break;
			case ImpliedAccess.t_Rmem_Wmem_Rrcx_Rrbx_Rds_Weax:
				if ((flags & Flags.NoMemoryUsage) == 0) {
					AddMemory(Register.DS, Register.RBX, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.Read, CodeSize.Code64, 0);
					AddMemory(Register.DS, Register.RCX, Register.None, 1, 0x0, MemorySize.Unknown, OpAccess.Write, CodeSize.Code64, 0);
				}
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, Register.RCX, OpAccess.Read);
					AddRegister(flags, Register.RBX, OpAccess.Read);
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, Register.DS, OpAccess.Read);
					AddRegister(flags, Register.EAX, OpAccess.Write);
				}
				break;
			// GENERATOR-END: ImpliedAccessHandler

			default:
				throw new InvalidOperationException();
			}
		}

		static Register GetARDI(Instruction instruction) =>
			instruction.Op0Kind switch {
				OpKind.MemorySegDI => Register.DI,
				OpKind.MemorySegEDI => Register.EDI,
				_ => Register.RDI,
			};

		static Register GetSegDefaultDS(Instruction instruction) {
			var seg = instruction.SegmentPrefix;
			return seg == Register.None ? Register.DS : seg;
		}

		void CommandPush(in Instruction instruction, Flags flags, int count, uint opSize) {
			Debug.Assert(count > 0);
			var xsp = GetXSP(instruction.CodeSize, out var xspMask, out var addressSize);
			if ((flags & Flags.NoRegisterUsage) == 0) {
				if ((flags & Flags.Is64Bit) == 0)
					AddRegister(flags, Register.SS, OpAccess.Read);
				AddRegister(flags, xsp, OpAccess.ReadWrite);
			}
			if ((flags & Flags.NoMemoryUsage) == 0) {
				MemorySize memSize;
				if (opSize == 8)
					memSize = MemorySize.UInt64;
				else if (opSize == 4)
					memSize = MemorySize.UInt32;
				else {
					Debug.Assert(opSize == 2);
					memSize = MemorySize.UInt16;
				}
				ulong offset = 0UL - opSize;
				for (int i = 0; i < count; i++, offset -= opSize)
					AddMemory(Register.SS, xsp, Register.None, 1, offset & xspMask, memSize, OpAccess.Write, addressSize, 0);
			}
		}

		void CommandPop(in Instruction instruction, Flags flags, int count, uint opSize) {
			Debug.Assert(count > 0);
			var xsp = GetXSP(instruction.CodeSize, out _, out var addressSize);
			if ((flags & Flags.NoRegisterUsage) == 0) {
				if ((flags & Flags.Is64Bit) == 0)
					AddRegister(flags, Register.SS, OpAccess.Read);
				AddRegister(flags, xsp, OpAccess.ReadWrite);
			}
			if ((flags & Flags.NoMemoryUsage) == 0) {
				MemorySize memSize;
				if (opSize == 8)
					memSize = MemorySize.UInt64;
				else if (opSize == 4)
					memSize = MemorySize.UInt32;
				else {
					Debug.Assert(opSize == 2);
					memSize = MemorySize.UInt16;
				}
				ulong offset = 0;
				for (int i = 0; i < count; i++, offset += opSize)
					AddMemory(Register.SS, xsp, Register.None, 1, offset, memSize, OpAccess.Read, addressSize, 0);
			}
		}

		void CommandPopRm(in Instruction instruction, Flags flags, uint opSize) {
			var xsp = GetXSP(instruction.CodeSize, out _, out var addressSize);
			if ((flags & Flags.NoRegisterUsage) == 0) {
				if ((flags & Flags.Is64Bit) == 0)
					AddRegister(flags, Register.SS, OpAccess.Read);
				AddRegister(flags, xsp, OpAccess.ReadWrite);
			}
			if ((flags & Flags.NoMemoryUsage) == 0) {
				MemorySize memSize;
				if (opSize == 8)
					memSize = MemorySize.UInt64;
				else if (opSize == 4)
					memSize = MemorySize.UInt32;
				else {
					Debug.Assert(opSize == 2);
					memSize = MemorySize.UInt16;
				}
				if (instruction.Op0Kind == OpKind.Memory) {
					Debug.Assert(info.usedMemoryLocations.ValidLength == 1);
					if (instruction.MemoryBase == Register.RSP || instruction.MemoryBase == Register.ESP) {
						ref var mem = ref info.usedMemoryLocations.Array[0];
						var displ = mem.Displacement + opSize;
						if (instruction.MemoryBase == Register.ESP)
							displ = (uint)displ;
						info.usedMemoryLocations.Array[0] = new UsedMemory(mem.Segment, mem.Base, mem.Index, mem.Scale, displ, mem.MemorySize, mem.Access, mem.AddressSize, mem.VsibSize);
					}
				}
				AddMemory(Register.SS, xsp, Register.None, 1, 0, memSize, OpAccess.Read, addressSize, 0);
			}
		}

		void CommandPusha(in Instruction instruction, Flags flags, uint opSize) {
			var xsp = GetXSP(instruction.CodeSize, out var xspMask, out var addressSize);
			if ((flags & Flags.NoRegisterUsage) == 0) {
				if ((flags & Flags.Is64Bit) == 0)
					AddRegister(flags, Register.SS, OpAccess.Read);
				AddRegister(flags, xsp, OpAccess.ReadWrite);
			}
			long displ;
			MemorySize memSize;
			Register baseReg;
			if (opSize == 4) {
				displ = -4;
				memSize = MemorySize.UInt32;
				baseReg = Register.EAX;
			}
			else {
				Debug.Assert(opSize == 2);
				displ = -2;
				memSize = MemorySize.UInt16;
				baseReg = Register.AX;
			}
			for (int i = 0; i < 8; i++) {
				if ((flags & Flags.NoRegisterUsage) == 0)
					AddRegister(flags, baseReg + i, OpAccess.Read);
				if ((flags & Flags.NoMemoryUsage) == 0)
					AddMemory(Register.SS, xsp, Register.None, 1, (ulong)(displ * (i + 1)) & xspMask, memSize, OpAccess.Write, addressSize, 0);
			}
		}

		void CommandPopa(in Instruction instruction, Flags flags, uint opSize) {
			var xsp = GetXSP(instruction.CodeSize, out var xspMask, out var addressSize);
			if ((flags & Flags.NoRegisterUsage) == 0) {
				if ((flags & Flags.Is64Bit) == 0)
					AddRegister(flags, Register.SS, OpAccess.Read);
				AddRegister(flags, xsp, OpAccess.ReadWrite);
			}
			MemorySize memSize;
			Register baseReg;
			if (opSize == 4) {
				memSize = MemorySize.UInt32;
				baseReg = Register.EAX;
			}
			else {
				Debug.Assert(opSize == 2);
				memSize = MemorySize.UInt16;
				baseReg = Register.AX;
			}
			for (int i = 0; i < 8; i++) {
				// Ignore eSP
				if (i != 3) {
					if ((flags & Flags.NoRegisterUsage) == 0)
						AddRegister(flags, baseReg + 7 - i, OpAccess.Write);
					if ((flags & Flags.NoMemoryUsage) == 0)
						AddMemory(Register.SS, xsp, Register.None, 1, (opSize * (uint)i) & xspMask, memSize, OpAccess.Read, addressSize, 0);
				}
			}
		}

		void CommandIns(in Instruction instruction, Flags flags) {
			CodeSize addressSize;
			Register rDI, rCX;
			switch (instruction.Op0Kind) {
			case OpKind.MemoryESDI: addressSize = CodeSize.Code16; rDI = Register.DI; rCX = Register.CX; break;
			case OpKind.MemoryESEDI: addressSize = CodeSize.Code32; rDI = Register.EDI; rCX = Register.ECX; break;
			default: addressSize = CodeSize.Code64; rDI = Register.RDI; rCX = Register.RCX; break;
			}
			if (instruction.Internal_HasRepeOrRepnePrefix) {
				unsafe { info.opAccesses[0] = (byte)OpAccess.CondWrite; }
				unsafe { info.opAccesses[1] = (byte)OpAccess.CondRead; }
				if ((flags & Flags.NoMemoryUsage) == 0)
					AddMemory(Register.ES, rDI, Register.None, 1, 0, MemorySize.Unknown, OpAccess.CondWrite, addressSize, 0);
				if ((flags & Flags.NoRegisterUsage) == 0) {
					Debug.Assert(info.usedRegisters.ValidLength == 1);
					info.usedRegisters.Array[0] = new UsedRegister(Register.DX, OpAccess.CondRead);
					AddRegister(flags, rCX, OpAccess.ReadCondWrite);
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, Register.ES, OpAccess.CondRead);
					AddRegister(flags, rDI, OpAccess.CondRead);
					AddRegister(flags, rDI, OpAccess.CondWrite);
				}
			}
			else {
				if ((flags & Flags.NoMemoryUsage) == 0)
					AddMemory(Register.ES, rDI, Register.None, 1, 0, instruction.MemorySize, OpAccess.Write, addressSize, 0);
				if ((flags & Flags.NoRegisterUsage) == 0) {
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, Register.ES, OpAccess.Read);
					AddRegister(flags, rDI, OpAccess.ReadWrite);
				}
			}
		}

		void CommandOuts(in Instruction instruction, Flags flags) {
			CodeSize addressSize;
			Register rSI, rCX;
			switch (instruction.Op1Kind) {
			case OpKind.MemorySegSI: addressSize = CodeSize.Code16; rSI = Register.SI; rCX = Register.CX; break;
			case OpKind.MemorySegESI: addressSize = CodeSize.Code32; rSI = Register.ESI; rCX = Register.ECX; break;
			default: addressSize = CodeSize.Code64; rSI = Register.RSI; rCX = Register.RCX; break;
			}
			if (instruction.Internal_HasRepeOrRepnePrefix) {
				unsafe { info.opAccesses[0] = (byte)OpAccess.CondRead; }
				unsafe { info.opAccesses[1] = (byte)OpAccess.CondRead; }
				if ((flags & Flags.NoMemoryUsage) == 0)
					AddMemory(instruction.MemorySegment, rSI, Register.None, 1, 0, MemorySize.Unknown, OpAccess.CondRead, addressSize, 0);
				if ((flags & Flags.NoRegisterUsage) == 0) {
					Debug.Assert(info.usedRegisters.ValidLength == 1);
					info.usedRegisters.Array[0] = new UsedRegister(Register.DX, OpAccess.CondRead);
					AddRegister(flags, rCX, OpAccess.ReadCondWrite);
					AddMemorySegmentRegister(flags, instruction.MemorySegment, OpAccess.CondRead);
					AddRegister(flags, rSI, OpAccess.CondRead);
					AddRegister(flags, rSI, OpAccess.CondWrite);
				}
			}
			else {
				if ((flags & Flags.NoMemoryUsage) == 0)
					AddMemory(instruction.MemorySegment, rSI, Register.None, 1, 0, instruction.MemorySize, OpAccess.Read, addressSize, 0);
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddMemorySegmentRegister(flags, instruction.MemorySegment, OpAccess.Read);
					AddRegister(flags, rSI, OpAccess.ReadWrite);
				}
			}
		}

		void CommandMovs(in Instruction instruction, Flags flags) {
			CodeSize addressSize;
			Register rSI, rDI, rCX;
			switch (instruction.Op0Kind) {
			case OpKind.MemoryESDI: addressSize = CodeSize.Code16; rSI = Register.SI; rDI = Register.DI; rCX = Register.CX; break;
			case OpKind.MemoryESEDI: addressSize = CodeSize.Code32; rSI = Register.ESI; rDI = Register.EDI; rCX = Register.ECX; break;
			default: addressSize = CodeSize.Code64; rSI = Register.RSI; rDI = Register.RDI; rCX = Register.RCX; break;
			}
			if (instruction.Internal_HasRepeOrRepnePrefix) {
				unsafe { info.opAccesses[0] = (byte)OpAccess.CondWrite; }
				unsafe { info.opAccesses[1] = (byte)OpAccess.CondRead; }
				if ((flags & Flags.NoMemoryUsage) == 0) {
					AddMemory(Register.ES, rDI, Register.None, 1, 0, MemorySize.Unknown, OpAccess.CondWrite, addressSize, 0);
					AddMemory(instruction.MemorySegment, rSI, Register.None, 1, 0, MemorySize.Unknown, OpAccess.CondRead, addressSize, 0);
				}
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, rCX, OpAccess.ReadCondWrite);
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, Register.ES, OpAccess.CondRead);
					AddRegister(flags, rDI, OpAccess.CondRead);
					AddRegister(flags, rDI, OpAccess.CondWrite);
					AddMemorySegmentRegister(flags, instruction.MemorySegment, OpAccess.CondRead);
					AddRegister(flags, rSI, OpAccess.CondRead);
					AddRegister(flags, rSI, OpAccess.CondWrite);
				}
			}
			else {
				if ((flags & Flags.NoMemoryUsage) == 0) {
					AddMemory(Register.ES, rDI, Register.None, 1, 0, instruction.MemorySize, OpAccess.Write, addressSize, 0);
					AddMemory(instruction.MemorySegment, rSI, Register.None, 1, 0, instruction.MemorySize, OpAccess.Read, addressSize, 0);
				}
				if ((flags & Flags.NoRegisterUsage) == 0) {
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, Register.ES, OpAccess.Read);
					AddRegister(flags, rDI, OpAccess.ReadWrite);
					AddMemorySegmentRegister(flags, instruction.MemorySegment, OpAccess.Read);
					AddRegister(flags, rSI, OpAccess.ReadWrite);
				}
			}
		}

		void CommandCmps(in Instruction instruction, Flags flags) {
			CodeSize addressSize;
			Register rSI, rDI, rCX;
			switch (instruction.Op0Kind) {
			case OpKind.MemorySegSI: addressSize = CodeSize.Code16; rSI = Register.SI; rDI = Register.DI; rCX = Register.CX; break;
			case OpKind.MemorySegESI: addressSize = CodeSize.Code32; rSI = Register.ESI; rDI = Register.EDI; rCX = Register.ECX; break;
			default: addressSize = CodeSize.Code64; rSI = Register.RSI; rDI = Register.RDI; rCX = Register.RCX; break;
			}
			if (instruction.Internal_HasRepeOrRepnePrefix) {
				unsafe { info.opAccesses[0] = (byte)OpAccess.CondRead; }
				unsafe { info.opAccesses[1] = (byte)OpAccess.CondRead; }
				if ((flags & Flags.NoMemoryUsage) == 0) {
					AddMemory(instruction.MemorySegment, rSI, Register.None, 1, 0, MemorySize.Unknown, OpAccess.CondRead, addressSize, 0);
					AddMemory(Register.ES, rDI, Register.None, 1, 0, MemorySize.Unknown, OpAccess.CondRead, addressSize, 0);
				}
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddRegister(flags, rCX, OpAccess.ReadCondWrite);
					AddMemorySegmentRegister(flags, instruction.MemorySegment, OpAccess.CondRead);
					AddRegister(flags, rSI, OpAccess.CondRead);
					AddRegister(flags, rSI, OpAccess.CondWrite);
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, Register.ES, OpAccess.CondRead);
					AddRegister(flags, rDI, OpAccess.CondRead);
					AddRegister(flags, rDI, OpAccess.CondWrite);
				}
			}
			else {
				if ((flags & Flags.NoMemoryUsage) == 0) {
					AddMemory(instruction.MemorySegment, rSI, Register.None, 1, 0, instruction.MemorySize, OpAccess.Read, addressSize, 0);
					AddMemory(Register.ES, rDI, Register.None, 1, 0, instruction.MemorySize, OpAccess.Read, addressSize, 0);
				}
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddMemorySegmentRegister(flags, instruction.MemorySegment, OpAccess.Read);
					AddRegister(flags, rSI, OpAccess.ReadWrite);
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, Register.ES, OpAccess.Read);
					AddRegister(flags, rDI, OpAccess.ReadWrite);
				}
			}
		}

		void CommandStos(in Instruction instruction, Flags flags) {
			CodeSize addressSize;
			Register rDI, rCX;
			switch (instruction.Op0Kind) {
			case OpKind.MemoryESDI: addressSize = CodeSize.Code16; rDI = Register.DI; rCX = Register.CX; break;
			case OpKind.MemoryESEDI: addressSize = CodeSize.Code32; rDI = Register.EDI; rCX = Register.ECX; break;
			default: addressSize = CodeSize.Code64; rDI = Register.RDI; rCX = Register.RCX; break;
			}
			if (instruction.Internal_HasRepeOrRepnePrefix) {
				unsafe { info.opAccesses[0] = (byte)OpAccess.CondWrite; }
				unsafe { info.opAccesses[1] = (byte)OpAccess.CondRead; }
				if ((flags & Flags.NoMemoryUsage) == 0)
					AddMemory(Register.ES, rDI, Register.None, 1, 0, MemorySize.Unknown, OpAccess.CondWrite, addressSize, 0);
				if ((flags & Flags.NoRegisterUsage) == 0) {
					Debug.Assert(info.usedRegisters.ValidLength == 1);
					info.usedRegisters.Array[0] = new UsedRegister(info.usedRegisters.Array[0].Register, OpAccess.CondRead);
					AddRegister(flags, rCX, OpAccess.ReadCondWrite);
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, Register.ES, OpAccess.CondRead);
					AddRegister(flags, rDI, OpAccess.CondRead);
					AddRegister(flags, rDI, OpAccess.CondWrite);
				}
			}
			else {
				if ((flags & Flags.NoMemoryUsage) == 0)
					AddMemory(Register.ES, rDI, Register.None, 1, 0, instruction.MemorySize, OpAccess.Write, addressSize, 0);
				if ((flags & Flags.NoRegisterUsage) == 0) {
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, Register.ES, OpAccess.Read);
					AddRegister(flags, rDI, OpAccess.ReadWrite);
				}
			}
		}

		void CommandLods(in Instruction instruction, Flags flags) {
			CodeSize addressSize;
			Register rSI, rCX;
			switch (instruction.Op1Kind) {
			case OpKind.MemorySegSI: addressSize = CodeSize.Code16; rSI = Register.SI; rCX = Register.CX; break;
			case OpKind.MemorySegESI: addressSize = CodeSize.Code32; rSI = Register.ESI; rCX = Register.ECX; break;
			default: addressSize = CodeSize.Code64; rSI = Register.RSI; rCX = Register.RCX; break;
			}
			if (instruction.Internal_HasRepeOrRepnePrefix) {
				unsafe { info.opAccesses[0] = (byte)OpAccess.CondWrite; }
				unsafe { info.opAccesses[1] = (byte)OpAccess.CondRead; }
				if ((flags & Flags.NoMemoryUsage) == 0)
					AddMemory(instruction.MemorySegment, rSI, Register.None, 1, 0, MemorySize.Unknown, OpAccess.CondRead, addressSize, 0);
				if ((flags & Flags.NoRegisterUsage) == 0) {
					Debug.Assert(info.usedRegisters.ValidLength == 1);
					info.usedRegisters.Array[0] = new UsedRegister(info.usedRegisters.Array[0].Register, OpAccess.CondWrite);
					AddRegister(flags, rCX, OpAccess.ReadCondWrite);
					AddMemorySegmentRegister(flags, instruction.MemorySegment, OpAccess.CondRead);
					AddRegister(flags, rSI, OpAccess.CondRead);
					AddRegister(flags, rSI, OpAccess.CondWrite);
				}
			}
			else {
				if ((flags & Flags.NoMemoryUsage) == 0)
					AddMemory(instruction.MemorySegment, rSI, Register.None, 1, 0, instruction.MemorySize, OpAccess.Read, addressSize, 0);
				if ((flags & Flags.NoRegisterUsage) == 0) {
					AddMemorySegmentRegister(flags, instruction.MemorySegment, OpAccess.Read);
					AddRegister(flags, rSI, OpAccess.ReadWrite);
				}
			}
		}

		void CommandScas(in Instruction instruction, Flags flags) {
			CodeSize addressSize;
			Register rDI, rCX;
			switch (instruction.Op1Kind) {
			case OpKind.MemoryESDI: addressSize = CodeSize.Code16; rDI = Register.DI; rCX = Register.CX; break;
			case OpKind.MemoryESEDI: addressSize = CodeSize.Code32; rDI = Register.EDI; rCX = Register.ECX; break;
			default: addressSize = CodeSize.Code64; rDI = Register.RDI; rCX = Register.RCX; break;
			}
			if (instruction.Internal_HasRepeOrRepnePrefix) {
				unsafe { info.opAccesses[0] = (byte)OpAccess.CondRead; }
				unsafe { info.opAccesses[1] = (byte)OpAccess.CondRead; }
				if ((flags & Flags.NoMemoryUsage) == 0)
					AddMemory(Register.ES, rDI, Register.None, 1, 0, MemorySize.Unknown, OpAccess.CondRead, addressSize, 0);
				if ((flags & Flags.NoRegisterUsage) == 0) {
					Debug.Assert(info.usedRegisters.ValidLength == 1);
					info.usedRegisters.Array[0] = new UsedRegister(info.usedRegisters.Array[0].Register, OpAccess.CondRead);
					AddRegister(flags, rCX, OpAccess.ReadCondWrite);
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, Register.ES, OpAccess.CondRead);
					AddRegister(flags, rDI, OpAccess.CondRead);
					AddRegister(flags, rDI, OpAccess.CondWrite);
				}
			}
			else {
				if ((flags & Flags.NoMemoryUsage) == 0)
					AddMemory(Register.ES, rDI, Register.None, 1, 0, instruction.MemorySize, OpAccess.Read, addressSize, 0);
				if ((flags & Flags.NoRegisterUsage) == 0) {
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, Register.ES, OpAccess.Read);
					AddRegister(flags, rDI, OpAccess.ReadWrite);
				}
			}
		}

		void CommandXstore(in Instruction instruction, Flags flags, uint size) {
			CodeSize addressSize;
			Register rDI, rCX;
			switch (size) {
			case 2: addressSize = CodeSize.Code16; rDI = Register.DI; rCX = Register.CX; break;
			case 4: addressSize = CodeSize.Code32; rDI = Register.EDI; rCX = Register.ECX; break;
			default: addressSize = CodeSize.Code64; rDI = Register.RDI; rCX = Register.RCX; break;
			}
			if (instruction.Internal_HasRepeOrRepnePrefix) {
				if ((flags & Flags.NoMemoryUsage) == 0)
					AddMemory(Register.ES, rDI, Register.None, 1, 0, MemorySize.Unknown, OpAccess.CondWrite, addressSize, 0);
				if ((flags & Flags.NoRegisterUsage) == 0) {
					Debug.Assert(info.usedRegisters.ValidLength == 0);
					AddRegister(flags, rCX, OpAccess.ReadCondWrite);
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, Register.ES, OpAccess.CondRead);
					AddRegister(flags, rDI, OpAccess.CondRead);
					AddRegister(flags, rDI, OpAccess.CondWrite);
					AddRegister(flags, Register.EAX, OpAccess.CondWrite);
					AddRegister(flags, Register.EDX, OpAccess.CondRead);
				}
			}
			else {
				if ((flags & Flags.NoMemoryUsage) == 0)
					AddMemory(Register.ES, rDI, Register.None, 1, 0, instruction.MemorySize, OpAccess.Write, addressSize, 0);
				if ((flags & Flags.NoRegisterUsage) == 0) {
					if ((flags & Flags.Is64Bit) == 0)
						AddRegister(flags, Register.ES, OpAccess.Read);
					AddRegister(flags, rDI, OpAccess.ReadWrite);
					AddRegister(flags, Register.EAX, OpAccess.Write);
					AddRegister(flags, Register.EDX, OpAccess.Read);
				}
			}
		}

		void CommandEnter(in Instruction instruction, Flags flags, uint opSize) {
			var xsp = GetXSP(instruction.CodeSize, out var xspMask, out var addressSize);
			if ((flags & Flags.NoRegisterUsage) == 0) {
				if ((flags & Flags.Is64Bit) == 0)
					AddRegister(flags, Register.SS, OpAccess.Read);
				AddRegister(flags, xsp, OpAccess.ReadWrite);
			}

			MemorySize memSize;
			Register rSP;
			if (opSize == 8) {
				memSize = MemorySize.UInt64;
				rSP = Register.RSP;
			}
			else if (opSize == 4) {
				memSize = MemorySize.UInt32;
				rSP = Register.ESP;
			}
			else {
				Debug.Assert(opSize == 2);
				memSize = MemorySize.UInt16;
				rSP = Register.SP;
			}

			if (rSP != xsp && (flags & Flags.NoRegisterUsage) == 0)
				AddRegister(flags, rSP, OpAccess.ReadWrite);

			int nestingLevel = instruction.Immediate8_2nd & 0x1F;

			ulong xspOffset = 0;
			// push rBP
			if ((flags & Flags.NoRegisterUsage) == 0)
				AddRegister(flags, rSP + 1, OpAccess.ReadWrite);
			if ((flags & Flags.NoMemoryUsage) == 0)
				AddMemory(Register.SS, xsp, Register.None, 1, (xspOffset -= opSize) & xspMask, memSize, OpAccess.Write, addressSize, 0);

			if (nestingLevel != 0) {
				var xbp = xsp + 1;// rBP immediately follows rSP
				ulong xbpOffset = 0;
				for (int i = 1; i < nestingLevel; i++) {
					if (i == 1 && rSP + 1 != xbp && (flags & Flags.NoRegisterUsage) == 0)
						AddRegister(flags, xbp, OpAccess.ReadWrite);
					// push [xbp]
					if ((flags & Flags.NoMemoryUsage) == 0) {
						AddMemory(Register.SS, xbp, Register.None, 1, (xbpOffset -= opSize) & xspMask, memSize, OpAccess.Read, addressSize, 0);
						AddMemory(Register.SS, xsp, Register.None, 1, (xspOffset -= opSize) & xspMask, memSize, OpAccess.Write, addressSize, 0);
					}
				}
				// push frameTemp
				if ((flags & Flags.NoMemoryUsage) == 0)
					AddMemory(Register.SS, xsp, Register.None, 1, (xspOffset -= opSize) & xspMask, memSize, OpAccess.Write, addressSize, 0);
			}
		}

		void CommandLeave(in Instruction instruction, Flags flags, uint opSize) {
			var xsp = GetXSP(instruction.CodeSize, out _, out var addressSize);
			if ((flags & Flags.NoRegisterUsage) == 0) {
				if ((flags & Flags.Is64Bit) == 0)
					AddRegister(flags, Register.SS, OpAccess.Read);
				AddRegister(flags, xsp, OpAccess.Write);
			}

			if (opSize == 8) {
				if ((flags & Flags.NoMemoryUsage) == 0)
					AddMemory(Register.SS, xsp + 1, Register.None, 1, 0, MemorySize.UInt64, OpAccess.Read, addressSize, 0);
				if ((flags & Flags.NoRegisterUsage) == 0) {
					if (xsp + 1 == Register.RBP)
						AddRegister(flags, Register.RBP, OpAccess.ReadWrite);
					else {
						AddRegister(flags, xsp + 1, OpAccess.Read);
						AddRegister(flags, Register.RBP, OpAccess.Write);
					}
				}
			}
			else if (opSize == 4) {
				if ((flags & Flags.NoMemoryUsage) == 0)
					AddMemory(Register.SS, xsp + 1, Register.None, 1, 0, MemorySize.UInt32, OpAccess.Read, addressSize, 0);
				if ((flags & Flags.NoRegisterUsage) == 0) {
					if (xsp + 1 == Register.EBP)
						AddRegister(flags, Register.EBP, OpAccess.ReadWrite);
					else {
						AddRegister(flags, xsp + 1, OpAccess.Read);
						AddRegister(flags, Register.EBP, OpAccess.Write);
					}
				}
			}
			else {
				Debug.Assert(opSize == 2);
				if ((flags & Flags.NoMemoryUsage) == 0)
					AddMemory(Register.SS, xsp + 1, Register.None, 1, 0, MemorySize.UInt16, OpAccess.Read, addressSize, 0);
				if ((flags & Flags.NoRegisterUsage) == 0) {
					if (xsp + 1 == Register.BP)
						AddRegister(flags, Register.BP, OpAccess.ReadWrite);
					else {
						AddRegister(flags, xsp + 1, OpAccess.Read);
						AddRegister(flags, Register.BP, OpAccess.Write);
					}
				}
			}
		}

		void CommandClearRflags(in Instruction instruction, Flags flags) {
			if (instruction.Op0Register != instruction.Op1Register)
				return;
			if (instruction.Op0Kind != OpKind.Register || instruction.Op1Kind != OpKind.Register)
				return;
			unsafe { info.opAccesses[0] = (byte)OpAccess.Write; }
			unsafe { info.opAccesses[1] = (byte)OpAccess.None; }
			if ((flags & Flags.NoRegisterUsage) == 0) {
				Debug.Assert(info.usedRegisters.ValidLength == 2 || info.usedRegisters.ValidLength == 3);
				info.usedRegisters.ValidLength = 0;
				AddRegister(flags, instruction.Op0Register, OpAccess.Write);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static bool IsClearInstr(in Instruction instruction) {
#if MVEX
			switch (instruction.MvexRegMemConv) {
			case MvexRegMemConv.None:
			case MvexRegMemConv.RegSwizzleNone:
				return true;
			default:
				return false;
			}
#else
			return true;
#endif
		}

		void CommandClearRegRegmem(in Instruction instruction, Flags flags) {
			if (instruction.Op0Register != instruction.Op1Register)
				return;
			if (instruction.Op1Kind != OpKind.Register)
				return;
			if (!IsClearInstr(instruction))
				return;
			unsafe { info.opAccesses[0] = (byte)OpAccess.Write; }
			unsafe { info.opAccesses[1] = (byte)OpAccess.None; }
			if ((flags & Flags.NoRegisterUsage) == 0) {
				Debug.Assert(info.usedRegisters.ValidLength == 2 || info.usedRegisters.ValidLength == 3);
				info.usedRegisters.Array[0] = new UsedRegister(instruction.Op0Register, OpAccess.Write);
				info.usedRegisters.ValidLength = 1;
			}
		}

		void CommandClearRegRegRegmem(in Instruction instruction, Flags flags) {
			if (instruction.Op1Register != instruction.Op2Register)
				return;
			if (instruction.Op2Kind != OpKind.Register)
				return;
			if (!IsClearInstr(instruction))
				return;
			unsafe { info.opAccesses[1] = (byte)OpAccess.None; }
			unsafe { info.opAccesses[2] = (byte)OpAccess.None; }
			if ((flags & Flags.NoRegisterUsage) == 0) {
				Debug.Assert(info.usedRegisters.ValidLength == 3 || info.usedRegisters.ValidLength == 4);
				Debug.Assert(info.usedRegisters.Array[info.usedRegisters.ValidLength - 2].Register == instruction.Op1Register);
				Debug.Assert(info.usedRegisters.Array[info.usedRegisters.ValidLength - 1].Register == instruction.Op2Register);
				info.usedRegisters.ValidLength -= 2;
			}
		}

		void CommandArpl(in Instruction instruction, Flags flags) {
			if ((flags & Flags.NoRegisterUsage) == 0) {
				Debug.Assert(info.usedRegisters.ValidLength != 0);
				// Skip memory operand, if any
				int startIndex = instruction.Op0Kind == OpKind.Register ? 0 : info.usedRegisters.ValidLength - 1;
				for (int i = 0; i < info.usedRegisters.ValidLength; i++) {
					if (i < startIndex)
						continue;
					var regInfo = info.usedRegisters.Array[i];
					int index = TryGetGpr163264Index(regInfo.Register);
					if (index >= 4)
						index += 4;// Skip AH, CH, DH, BH
					if (index >= 0)
						info.usedRegisters.Array[i] = new UsedRegister(Register.AL + index, regInfo.Access);
				}
			}
		}

		void CommandLastGpr(in Instruction instruction, Flags flags, Register baseReg) {
			if ((flags & Flags.NoRegisterUsage) == 0) {
				int opCount = instruction.OpCount;
				int immCount = instruction.GetOpKind(opCount - 1) == OpKind.Immediate8 ? 1 : 0;
				const int N = 1;
				int opIndex = opCount - N - immCount;
				if (instruction.GetOpKind(opIndex) == OpKind.Register) {
					Debug.Assert(info.usedRegisters.ValidLength >= N);
					Debug.Assert(info.usedRegisters.Array[info.usedRegisters.ValidLength - N].Register == instruction.GetOpRegister(opIndex));
					Debug.Assert(info.usedRegisters.Array[info.usedRegisters.ValidLength - N].Access == OpAccess.Read);
					int index = TryGetGpr163264Index(instruction.GetOpRegister(opIndex));
					if (index >= 4 && baseReg == Register.AL)
						index += 4;// Skip AH, CH, DH, BH
					if (index >= 0)
						info.usedRegisters.Array[info.usedRegisters.ValidLength - N] = new UsedRegister(baseReg + index, OpAccess.Read);
				}
			}
		}

		void CommandLea(in Instruction instruction, Flags flags) {
			if ((flags & Flags.NoRegisterUsage) == 0) {
				Debug.Assert(info.usedRegisters.ValidLength >= 1);
				Debug.Assert(instruction.Op0Kind == OpKind.Register);
				var reg = instruction.Op0Register;
				// The memory operand's regs start at index 1
				for (int i = 1; i < info.usedRegisters.ValidLength; i++) {
					var regInfo = info.usedRegisters.Array[i];
					if (reg >= Register.EAX && reg <= Register.R15D) {
						if (regInfo.Register >= Register.RAX && regInfo.Register <= Register.R15) {
							var memReg = regInfo.Register - Register.RAX + Register.EAX;
							info.usedRegisters.Array[i] = new UsedRegister(memReg, regInfo.Access);
						}
					}
					else if (reg >= Register.AX && reg <= Register.R15W) {
						if (regInfo.Register >= Register.EAX && regInfo.Register <= Register.R15) {
							var memReg = ((regInfo.Register - Register.EAX) & 0xF) + Register.AX;
							info.usedRegisters.Array[i] = new UsedRegister(memReg, regInfo.Access);
						}
					}
					else {
						Debug.Assert(reg >= Register.RAX && reg <= Register.R15);
						break;
					}
				}
			}
		}

		void CommandEmmi(in Instruction instruction, Flags flags, OpAccess opAccess) {
			if ((flags & Flags.NoRegisterUsage) == 0) {
				if (instruction.Op0Kind == OpKind.Register) {
					var reg = instruction.Op0Register;
					if (reg >= Register.MM0 && reg <= Register.MM7) {
						reg = ((reg - Register.MM0) ^ 1) + Register.MM0;
						AddRegister(flags, reg, opAccess);
					}
				}
			}
		}

		void CommandMemDispl(Flags flags, int extraDispl) {
			if ((flags & Flags.NoMemoryUsage) == 0) {
				if (info.usedMemoryLocations.ValidLength == 1) {
					ref var mem = ref info.usedMemoryLocations.Array[0];
					ulong mask = mem.AddressSize switch {
						CodeSize.Code16 => ushort.MaxValue,
						CodeSize.Code32 => uint.MaxValue,
						_ => ulong.MaxValue,
					};
					var displ = (mem.Displacement + (ulong)extraDispl) & mask;
					info.usedMemoryLocations.Array[0] = new UsedMemory(mem.Segment, mem.Base, mem.Index, mem.Scale, displ, mem.MemorySize, mem.Access, mem.AddressSize, mem.VsibSize);
				}
				else
					Debug.Assert(false);
			}
		}

		static int TryGetGpr163264Index(Register register) {
			int index;
			index = register - Register.EAX;
			if ((uint)index <= 15)
				return index;
			index = register - Register.RAX;
			if ((uint)index <= 15)
				return index;
			index = register - Register.AX;
			if ((uint)index <= 15)
				return index;
			return -1;
		}

		void AddMemory(Register segReg, Register baseReg, Register indexReg, int scale, ulong displ, MemorySize memorySize, OpAccess access, CodeSize addressSize, int vsibSize) {
			if (addressSize == CodeSize.Unknown) {
				var reg = baseReg != Register.None ? baseReg : indexReg;
				if (reg.IsGPR64())
					addressSize = CodeSize.Code64;
				else if (reg.IsGPR32())
					addressSize = CodeSize.Code32;
				else if (reg.IsGPR16())
					addressSize = CodeSize.Code16;
			}
			if (access != OpAccess.NoMemAccess) {
				int arrayLength = info.usedMemoryLocations.Array.Length;
				int validLen = info.usedMemoryLocations.ValidLength;
				if (arrayLength == validLen) {
					if (arrayLength == 0)
						info.usedMemoryLocations.Array = new UsedMemory[defaultMemoryArrayCount];
					else
						Array.Resize(ref info.usedMemoryLocations.Array, arrayLength * 2);
				}
				info.usedMemoryLocations.Array[validLen] = new UsedMemory(segReg, baseReg, indexReg, scale, displ, memorySize, access, addressSize, vsibSize);
				info.usedMemoryLocations.ValidLength = validLen + 1;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		void AddMemorySegmentRegister(Flags flags, Register seg, OpAccess access) {
			Debug.Assert(Register.ES <= seg && seg <= Register.GS);
			// Ignore es,cs,ss,ds memory operand segment registers in 64-bit mode
			if ((flags & Flags.Is64Bit) == 0 || seg >= Register.FS)
				AddRegister(flags, seg, access);
		}

		void AddRegister(Flags flags, Register reg, OpAccess access) {
			Debug.Assert((flags & Flags.NoRegisterUsage) == 0, "Caller should check flags before calling this method");

			var writeReg = reg;
			if ((flags & (Flags.Is64Bit | Flags.ZeroExtVecRegs)) != 0) {
				Static.Assert(OpAccess.Write + 1 == OpAccess.CondWrite ? 0 : -1);
				Static.Assert(OpAccess.Write + 2 == OpAccess.ReadWrite ? 0 : -1);
				Static.Assert(OpAccess.Write + 3 == OpAccess.ReadCondWrite ? 0 : -1);
				if ((uint)(access - OpAccess.Write) <= 3) {
					int index;
					Static.Assert(IcedConstants.VMM_first == Register.ZMM0 ? 0 : -1);
					if ((flags & Flags.Is64Bit) != 0 && (uint)(index = reg - Register.EAX) <= (Register.R15D - Register.EAX))
						writeReg = Register.RAX + index;
					else if ((flags & Flags.ZeroExtVecRegs) != 0 && (uint)(index = reg - Register.XMM0) <= IcedConstants.VMM_last - Register.XMM0)
						writeReg = Register.ZMM0 + (index % IcedConstants.VMM_count);
					if (access != OpAccess.ReadWrite && access != OpAccess.ReadCondWrite)
						reg = writeReg;
				}
			}

			var array = info.usedRegisters.Array;
			int validLen = info.usedRegisters.ValidLength;
			int arrayLength = array.Length;
			int numRegs = writeReg == reg ? 1 : 2;
			if (validLen + numRegs > arrayLength) {
				if (arrayLength == 0) {
					// The code below that resizes the array assumes there's at least 2 new free elements, so the minimum array length is 2.
					Debug.Assert(defaultRegisterArrayCount >= 2);
					info.usedRegisters.Array = array = new UsedRegister[defaultRegisterArrayCount];
				}
				else {
					Debug.Assert(arrayLength * 2 >= arrayLength + numRegs);
					Array.Resize(ref info.usedRegisters.Array, arrayLength * 2);
					array = info.usedRegisters.Array;
				}
			}

			if (writeReg == reg) {
				array[validLen] = new UsedRegister(reg, access);
				info.usedRegisters.ValidLength = validLen + 1;
			}
			else {
				Debug.Assert(access == OpAccess.ReadWrite || access == OpAccess.ReadCondWrite);
				array[validLen] = new UsedRegister(reg, OpAccess.Read);
				validLen++;
				var lastAccess = access == OpAccess.ReadWrite ? OpAccess.Write : OpAccess.CondWrite;
				array[validLen] = new UsedRegister(writeReg, lastAccess);
				info.usedRegisters.ValidLength = validLen + 1;
			}
		}
	}
}
#endif
