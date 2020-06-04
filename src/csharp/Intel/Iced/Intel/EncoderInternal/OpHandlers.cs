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
using System.Diagnostics;

namespace Iced.Intel.EncoderInternal {
	abstract class Op {
		public abstract void Encode(Encoder encoder, in Instruction instruction, int operand);

		/// <summary>
		/// If this is an immediate operand, it returns the <see cref="OpKind"/> value, else it returns -1
		/// </summary>
		/// <returns></returns>
		public virtual OpKind GetImmediateOpKind() => (OpKind)(-1);

		/// <summary>
		/// If this is a near branch operand, it returns the <see cref="OpKind"/> value, else it returns -1
		/// </summary>
		/// <returns></returns>
		public virtual OpKind GetNearBranchOpKind() => (OpKind)(-1);

		/// <summary>
		/// If this is a far branch operand, it returns the <see cref="OpKind"/> value, else it returns -1
		/// </summary>
		/// <returns></returns>
		public virtual OpKind GetFarBranchOpKind() => (OpKind)(-1);
	}

	sealed class OpModRM_rm_mem_only : Op {
		public override void Encode(Encoder encoder, in Instruction instruction, int operand) =>
			encoder.AddRegOrMem(instruction, operand, Register.None, Register.None, allowMemOp: true, allowRegOp: false);
	}

	sealed class OpModRM_rm : Op {
		readonly Register regLo;
		readonly Register regHi;

		public OpModRM_rm(Register regLo, Register regHi) {
			this.regLo = regLo;
			this.regHi = regHi;
		}

		public override void Encode(Encoder encoder, in Instruction instruction, int operand) =>
			encoder.AddRegOrMem(instruction, operand, regLo, regHi, allowMemOp: true, allowRegOp: true);
	}

	sealed class OpRegEmbed8: Op {
		readonly Register regLo;
		readonly Register regHi;

		public OpRegEmbed8(Register regLo, Register regHi) {
			this.regLo = regLo;
			this.regHi = regHi;
		}

		public override void Encode(Encoder encoder, in Instruction instruction, int operand) =>
			encoder.AddReg(instruction, operand, regLo, regHi);
	}

	sealed class OpModRM_rm_reg_only : Op {
		readonly Register regLo;
		readonly Register regHi;

		public OpModRM_rm_reg_only(Register regLo, Register regHi) {
			this.regLo = regLo;
			this.regHi = regHi;
		}

		public override void Encode(Encoder encoder, in Instruction instruction, int operand) =>
			encoder.AddRegOrMem(instruction, operand, regLo, regHi, allowMemOp: false, allowRegOp: true);
	}

	sealed class OpModRM_reg : Op {
		readonly Register regLo;
		readonly Register regHi;

		public OpModRM_reg(Register regLo, Register regHi) {
			this.regLo = regLo;
			this.regHi = regHi;
		}

		public override void Encode(Encoder encoder, in Instruction instruction, int operand) =>
			encoder.AddModRMRegister(instruction, operand, regLo, regHi);
	}

	sealed class OpModRM_reg_mem : Op {
		readonly Register regLo;
		readonly Register regHi;

		public OpModRM_reg_mem(Register regLo, Register regHi) {
			this.regLo = regLo;
			this.regHi = regHi;
		}

		public override void Encode(Encoder encoder, in Instruction instruction, int operand) {
			encoder.AddModRMRegister(instruction, operand, regLo, regHi);
			encoder.EncoderFlags |= EncoderFlags.RegIsMemory;
		}
	}

	sealed class OpModRM_regF0 : Op {
		readonly Register regLo;
		readonly Register regHi;

		public OpModRM_regF0(Register regLo, Register regHi) {
			this.regLo = regLo;
			this.regHi = regHi;
		}

		public override void Encode(Encoder encoder, in Instruction instruction, int operand) {
			if (encoder.Bitness != 64 && instruction.GetOpKind(operand) == OpKind.Register && instruction.GetOpRegister(operand) >= regLo + 8 && instruction.GetOpRegister(operand) <= regLo + 15) {
				encoder.EncoderFlags |= EncoderFlags.PF0;
				encoder.AddModRMRegister(instruction, operand, regLo + 8, regLo + 15);
			}
			else
				encoder.AddModRMRegister(instruction, operand, regLo, regHi);
		}
	}

	sealed class OpReg : Op {
		readonly Register register;

		public OpReg(Register register) => this.register = register;

		public override void Encode(Encoder encoder, in Instruction instruction, int operand) {
			encoder.Verify(operand, OpKind.Register, instruction.GetOpKind(operand));
			encoder.Verify(operand, register, instruction.GetOpRegister(operand));
		}
	}

	sealed class OpRegSTi : Op {
		public override void Encode(Encoder encoder, in Instruction instruction, int operand) {
			if (!encoder.Verify(operand, OpKind.Register, instruction.GetOpKind(operand)))
				return;
			var reg = instruction.GetOpRegister(operand);
			if (!encoder.Verify(operand, reg, Register.ST0, Register.ST7))
				return;
			Debug.Assert((encoder.OpCode & 7) == 0);
			encoder.OpCode |= (uint)(reg - Register.ST0);
		}
	}

	sealed class OprDI : Op {
		static int GetRegSize(OpKind opKind) {
			if (opKind == OpKind.MemorySegRDI)
				return 8;
			if (opKind == OpKind.MemorySegEDI)
				return 4;
			if (opKind == OpKind.MemorySegDI)
				return 2;
			return 0;
		}

		public override void Encode(Encoder encoder, in Instruction instruction, int operand) {
			var regSize = GetRegSize(instruction.GetOpKind(operand));
			if (regSize == 0) {
				encoder.ErrorMessage = $"Operand {operand}: expected OpKind = {nameof(OpKind.MemorySegDI)}, {nameof(OpKind.MemorySegEDI)} or {nameof(OpKind.MemorySegRDI)}";
				return;
			}
			encoder.SetAddrSize(regSize);
		}
	}

	sealed class OpIb : Op {
		readonly OpKind opKind;

		public OpIb(OpKind opKind) => this.opKind = opKind;

		public override void Encode(Encoder encoder, in Instruction instruction, int operand) {
			var opImmKind = instruction.GetOpKind(operand);
			if (!encoder.Verify(operand, opKind, opImmKind))
				return;
			encoder.ImmSize = ImmSize.Size1;
			encoder.Immediate = instruction.Immediate8;
		}

		public override OpKind GetImmediateOpKind() => opKind;
	}

	sealed class OpIw : Op {
		public override void Encode(Encoder encoder, in Instruction instruction, int operand) {
			if (!encoder.Verify(operand, OpKind.Immediate16, instruction.GetOpKind(operand)))
				return;
			encoder.ImmSize = ImmSize.Size2;
			encoder.Immediate = instruction.Immediate16;
		}

		public override OpKind GetImmediateOpKind() => OpKind.Immediate16;
	}

	sealed class OpId : Op {
		readonly OpKind opKind;

		public OpId(OpKind opKind) => this.opKind = opKind;

		public override void Encode(Encoder encoder, in Instruction instruction, int operand) {
			var opImmKind = instruction.GetOpKind(operand);
			if (!encoder.Verify(operand, opKind, opImmKind))
				return;
			encoder.ImmSize = ImmSize.Size4;
			encoder.Immediate = instruction.Immediate32;
		}

		public override OpKind GetImmediateOpKind() => opKind;
	}

	sealed class OpIq : Op {
		public override void Encode(Encoder encoder, in Instruction instruction, int operand) {
			if (!encoder.Verify(operand, OpKind.Immediate64, instruction.GetOpKind(operand)))
				return;
			encoder.ImmSize = ImmSize.Size8;
			ulong imm = instruction.Immediate64;
			encoder.Immediate = (uint)imm;
			encoder.ImmediateHi = (uint)(imm >> 32);
		}

		public override OpKind GetImmediateOpKind() => OpKind.Immediate64;
	}

	sealed class OpIb21 : Op {
		public override void Encode(Encoder encoder, in Instruction instruction, int operand) {
			if (!encoder.Verify(operand, OpKind.Immediate8_2nd, instruction.GetOpKind(operand)))
				return;
			Debug.Assert(encoder.ImmSize == ImmSize.Size2);
			encoder.ImmSize = ImmSize.Size2_1;
			encoder.ImmediateHi = instruction.Immediate8_2nd;
		}

		public override OpKind GetImmediateOpKind() => OpKind.Immediate8_2nd;
	}

	sealed class OpIb11 : Op {
		public override void Encode(Encoder encoder, in Instruction instruction, int operand) {
			if (!encoder.Verify(operand, OpKind.Immediate8_2nd, instruction.GetOpKind(operand)))
				return;
			Debug.Assert(encoder.ImmSize == ImmSize.Size1);
			encoder.ImmSize = ImmSize.Size1_1;
			encoder.ImmediateHi = instruction.Immediate8_2nd;
		}

		public override OpKind GetImmediateOpKind() => OpKind.Immediate8_2nd;
	}

	sealed class OpI2 : Op {
		public override void Encode(Encoder encoder, in Instruction instruction, int operand) {
			var opImmKind = instruction.GetOpKind(operand);
			if (!encoder.Verify(operand, OpKind.Immediate8, opImmKind))
				return;
			Debug.Assert(encoder.ImmSize == ImmSize.SizeIbReg);
			Debug.Assert((encoder.Immediate & 3) == 0);
			if (instruction.Immediate8 > 3) {
				encoder.ErrorMessage = $"Operand {operand}: Immediate value must be 0-3, but value is 0x{instruction.Immediate8:X2}";
				return;
			}
			encoder.ImmSize = ImmSize.Size1;
			encoder.Immediate |= instruction.Immediate8;
		}

		public override OpKind GetImmediateOpKind() => OpKind.Immediate8;
	}

	sealed class OpX : Op {
		internal static int GetXRegSize(OpKind opKind) {
			if (opKind == OpKind.MemorySegRSI)
				return 8;
			if (opKind == OpKind.MemorySegESI)
				return 4;
			if (opKind == OpKind.MemorySegSI)
				return 2;
			return 0;
		}

		internal static int GetYRegSize(OpKind opKind) {
			if (opKind == OpKind.MemoryESRDI)
				return 8;
			if (opKind == OpKind.MemoryESEDI)
				return 4;
			if (opKind == OpKind.MemoryESDI)
				return 2;
			return 0;
		}

		public override void Encode(Encoder encoder, in Instruction instruction, int operand) {
			var regXSize = GetXRegSize(instruction.GetOpKind(operand));
			if (regXSize == 0) {
				encoder.ErrorMessage = $"Operand {operand}: expected OpKind = {nameof(OpKind.MemorySegSI)}, {nameof(OpKind.MemorySegESI)} or {nameof(OpKind.MemorySegRSI)}";
				return;
			}
			switch (instruction.Code) {
			case Code.Movsb_m8_m8:
			case Code.Movsw_m16_m16:
			case Code.Movsd_m32_m32:
			case Code.Movsq_m64_m64:
				var regYSize = GetYRegSize(instruction.Op0Kind);
				if (regXSize != regYSize) {
					encoder.ErrorMessage = $"Same sized register must be used: reg #1 size = {regYSize * 8}, reg #2 size = {regXSize * 8}";
					return;
				}
				break;
			}
			encoder.SetAddrSize(regXSize);
		}
	}

	sealed class OpY : Op {
		public override void Encode(Encoder encoder, in Instruction instruction, int operand) {
			var regYSize = OpX.GetYRegSize(instruction.GetOpKind(operand));
			if (regYSize == 0) {
				encoder.ErrorMessage = $"Operand {operand}: expected OpKind = {nameof(OpKind.MemoryESDI)}, {nameof(OpKind.MemoryESEDI)} or {nameof(OpKind.MemoryESRDI)}";
				return;
			}
			switch (instruction.Code) {
			case Code.Cmpsb_m8_m8:
			case Code.Cmpsw_m16_m16:
			case Code.Cmpsd_m32_m32:
			case Code.Cmpsq_m64_m64:
				var regXSize = OpX.GetXRegSize(instruction.Op0Kind);
				if (regXSize != regYSize) {
					encoder.ErrorMessage = $"Same sized register must be used: reg #1 size = {regXSize * 8}, reg #2 size = {regYSize * 8}";
					return;
				}
				break;
			}
			encoder.SetAddrSize(regYSize);
		}
	}

	sealed class OpMRBX : Op {
		public override void Encode(Encoder encoder, in Instruction instruction, int operand) {
			if (!encoder.Verify(operand, OpKind.Memory, instruction.GetOpKind(operand)))
				return;
			var baseReg = instruction.MemoryBase;
			if (instruction.MemoryDisplSize != 0 || instruction.MemoryIndex != Register.AL || (baseReg != Register.BX && baseReg != Register.EBX && baseReg != Register.RBX)) {
				encoder.ErrorMessage = $"Operand {operand}: Operand must be [bx+al], [ebx+al], or [rbx+al]";
				return;
			}
			int regSize;
			if (baseReg == Register.RBX)
				regSize = 8;
			else if (baseReg == Register.EBX)
				regSize = 4;
			else {
				Debug.Assert(baseReg == Register.BX);
				regSize = 2;
			}
			encoder.SetAddrSize(regSize);
		}
	}

	sealed class OpJ : Op {
		readonly OpKind opKind;
		readonly int immSize;

		public OpJ(OpKind opKind, int immSize) {
			this.opKind = opKind;
			this.immSize = immSize;
		}

		public override void Encode(Encoder encoder, in Instruction instruction, int operand) =>
			encoder.AddBranch(opKind, immSize, instruction, operand);

		public override OpKind GetNearBranchOpKind() => opKind;
	}

	sealed class OpJx : Op {
		readonly int immSize;

		public OpJx(int immSize) => this.immSize = immSize;

		public override void Encode(Encoder encoder, in Instruction instruction, int operand) =>
			encoder.AddBranchX(immSize, instruction, operand);

		public override OpKind GetNearBranchOpKind() {
			// xbegin is special and doesn't mask the target IP. We need to know the code size to return the correct value.
			// Instruction.CreateXbegin() should be used to create the instruction and this method should never be called.
			Debug.Fail("Call Instruction.CreateXbegin()");
			return base.GetNearBranchOpKind();
		}
	}

	sealed class OpJdisp : Op {
		readonly int displSize;

		public OpJdisp(int displSize) => this.displSize = displSize;

		public override void Encode(Encoder encoder, in Instruction instruction, int operand) =>
			encoder.AddBranchDisp(displSize, instruction, operand);

		public override OpKind GetNearBranchOpKind() => displSize == 2 ? OpKind.NearBranch16 : OpKind.NearBranch32;
	}

	sealed class OpA : Op {
		readonly int size;

		public OpA(int size) {
			Debug.Assert(size == 2 || size == 4);
			this.size = size;
		}

		public override void Encode(Encoder encoder, in Instruction instruction, int operand) =>
			encoder.AddFarBranch(instruction, operand, size);

		public override OpKind GetFarBranchOpKind() {
			Debug.Assert(size == 2 || size == 4);
			return size == 2 ? OpKind.FarBranch16 : OpKind.FarBranch32;
		}
	}

	sealed class OpO : Op {
		public override void Encode(Encoder encoder, in Instruction instruction, int operand) =>
			encoder.AddAbsMem(instruction, operand);
	}

	sealed class OpImm : Op {
		readonly byte value;

		public OpImm(byte value) => this.value = value;

		public override void Encode(Encoder encoder, in Instruction instruction, int operand) {
			if (!encoder.Verify(operand, OpKind.Immediate8, instruction.GetOpKind(operand)))
				return;
			if (instruction.Immediate8 != value) {
				encoder.ErrorMessage = $"Operand {operand}: Expected 0x{value:X2}, actual: 0x{instruction.Immediate8:X2}";
				return;
			}
		}

		public override OpKind GetImmediateOpKind() => OpKind.Immediate8;
	}

	sealed class OpHx : Op {
		readonly Register regLo;
		readonly Register regHi;

		public OpHx(Register regLo, Register regHi) {
			this.regLo = regLo;
			this.regHi = regHi;
		}

		public override void Encode(Encoder encoder, in Instruction instruction, int operand) {
			if (!encoder.Verify(operand, OpKind.Register, instruction.GetOpKind(operand)))
				return;
			var reg = instruction.GetOpRegister(operand);
			if (!encoder.Verify(operand, reg, regLo, regHi))
				return;
			encoder.EncoderFlags |= (EncoderFlags)((uint)(reg - regLo) << (int)EncoderFlags.VvvvvShift);
		}
	}

#if !NO_VEX || !NO_EVEX
	sealed class OpVMx : Op {
		readonly Register vsibIndexRegLo;
		readonly Register vsibIndexRegHi;

		public OpVMx(Register regLo, Register regHi) {
			vsibIndexRegLo = regLo;
			vsibIndexRegHi = regHi;
		}

		public override void Encode(Encoder encoder, in Instruction instruction, int operand) =>
			encoder.AddRegOrMem(instruction, operand, Register.None, Register.None, vsibIndexRegLo, vsibIndexRegHi, allowMemOp: true, allowRegOp: false);
	}
#endif

#if !NO_VEX || !NO_XOP
	sealed class OpIs4x : Op {
		readonly Register regLo;
		readonly Register regHi;

		public OpIs4x(Register regLo, Register regHi) {
			this.regLo = regLo;
			this.regHi = regHi;
		}

		public override void Encode(Encoder encoder, in Instruction instruction, int operand) {
			if (!encoder.Verify(operand, OpKind.Register, instruction.GetOpKind(operand)))
				return;
			var reg = instruction.GetOpRegister(operand);
			if (!encoder.Verify(operand, reg, regLo, regHi))
				return;
			encoder.ImmSize = ImmSize.SizeIbReg;
			encoder.Immediate = (uint)(reg - regLo) << 4;
		}
	}
#endif
}
#endif
