using Iced.Intel;
using Xunit;

using static Iced.Intel.AssemblerRegisters;

namespace Iced.UnitTests.Intel.AssemblerTests {
	public sealed partial class AssemblerTests64 {

		[Fact]
		public void TestInstructionPrefixes() {
			{
				var inst = Instruction.CreateStosd(Bitness);
				inst.HasRepPrefix = true;
				TestAssembler(c => c.rep.stosd(), inst);
			}
			
			{
				var inst = Instruction.CreateStosd(Bitness);
				inst.HasRepePrefix = true;
				TestAssembler(c => c.repe.stosd(), inst);
			}
			
			{
				var inst = Instruction.CreateStosd(Bitness);
				inst.HasRepnePrefix = true;
				TestAssembler(c => c.repne.stosd(), inst);
			}

			{
				var inst = Instruction.Create(Code.Xchg_rm64_r64, __[rdx], rax);
				inst.HasXacquirePrefix = true;
				TestAssembler( c=> c.xacquire.xchg(__[rdx], rax), inst);
			}
			
			{
				var inst = Instruction.Create(Code.Xchg_rm64_r64, __[rdx], rax);
				inst.HasLockPrefix = true;
				TestAssembler( c=> c.@lock.xchg(__[rdx], rax), inst);
			}
			
			{
				var inst = Instruction.Create(Code.Xchg_rm64_r64, __[rdx], rax);
				inst.HasXreleasePrefix = true;
				TestAssembler( c=> c.xrelease.xchg(__[rdx], rax), inst);
			}

			{
				var inst = Instruction.Create(Code.Call_m1664, __[rax]);
				inst.SegmentPrefix = Register.DS;
				TestAssembler( c=> c.notrack.call(__[rax]), inst);
			}

			{
				var inst = Instruction.Create(Code.Call_m1664, __[rax]);
				inst.SegmentPrefix = Register.DS;
				inst.HasRepnePrefix = true;
				TestAssembler( c=> c.bnd.notrack.call(__[rax]), inst);
			}
		}
		
		[Fact]
		public void TestOperandModifiers() {
			{
				var inst = Instruction.Create(Code.EVEX_Vunpcklps_xmm_k1z_xmm_xmmm128b32, xmm2, xmm6, __[rax]);
				inst.ZeroingMasking = true;
				inst.OpMask = Register.K1;
				inst.IsBroadcast = true;
				TestAssembler(c => c.vunpcklps(xmm2.k1.z, xmm6, __dword_bcst[rax]), inst, LocalOpCodeFlags.PreferEvex);
			}
			{
				var inst = Instruction.Create(Code.EVEX_Vunpcklps_xmm_k1z_xmm_xmmm128b32, xmm2, xmm6, __[rax]);
				inst.ZeroingMasking = true;
				inst.OpMask = Register.K2;
				inst.IsBroadcast = true;
				TestAssembler(c => c.vunpcklps(xmm2.k2.z, xmm6, __dword_bcst[rax]), inst, LocalOpCodeFlags.PreferEvex);
			}
			{
				var inst = Instruction.Create(Code.EVEX_Vunpcklps_xmm_k1z_xmm_xmmm128b32, xmm2, xmm6, __[rax]);
				inst.ZeroingMasking = true;
				inst.OpMask = Register.K3;
				inst.IsBroadcast = true;
				TestAssembler(c => c.vunpcklps(xmm2.k3.z, xmm6, __dword_bcst[rax]), inst, LocalOpCodeFlags.PreferEvex);
			}
			{
				var inst = Instruction.Create(Code.EVEX_Vunpcklps_xmm_k1z_xmm_xmmm128b32, xmm2, xmm6, __[rax]);
				inst.ZeroingMasking = true;
				inst.OpMask = Register.K4;
				inst.IsBroadcast = true;
				TestAssembler(c => c.vunpcklps(xmm2.k4.z, xmm6, __dword_bcst[rax]), inst, LocalOpCodeFlags.PreferEvex);
			}
			{
				var inst = Instruction.Create(Code.EVEX_Vunpcklps_xmm_k1z_xmm_xmmm128b32, xmm2, xmm6, __[rax]);
				inst.ZeroingMasking = true;
				inst.OpMask = Register.K5;
				inst.IsBroadcast = true;
				TestAssembler(c => c.vunpcklps(xmm2.k5.z, xmm6, __dword_bcst[rax]), inst, LocalOpCodeFlags.PreferEvex);
			}
			{
				var inst = Instruction.Create(Code.EVEX_Vunpcklps_xmm_k1z_xmm_xmmm128b32, xmm2, xmm6, __[rax]);
				inst.ZeroingMasking = true;
				inst.OpMask = Register.K6;
				inst.IsBroadcast = true;
				TestAssembler(c => c.vunpcklps(xmm2.k6.z, xmm6, __dword_bcst[rax]), inst, LocalOpCodeFlags.PreferEvex);
			}
			{
				var inst = Instruction.Create(Code.EVEX_Vunpcklps_xmm_k1z_xmm_xmmm128b32, xmm2, xmm6, __[rax]);
				inst.ZeroingMasking = true;
				inst.OpMask = Register.K7;
				inst.IsBroadcast = true;
				TestAssembler(c => c.vunpcklps(xmm2.k7.z, xmm6, __dword_bcst[rax]), inst, LocalOpCodeFlags.PreferEvex);
			}
			
			/* // TODO
			{
				var inst = Instruction.Create(Code.EVEX_Vaddpd_zmm_k1z_zmm_zmmm512b64_er, zmm1, zmm2, __[rax]);
				inst.IsBroadcast = true;
				inst.RoundingControl = RoundingControl.RoundDown;
				TestAssembler(c => c.vaddpd(zmm1, zmm2, __dword_bcst[rax].rd_sae), inst);
			}			
			{
				var inst = Instruction.Create(Code.EVEX_Vaddpd_zmm_k1z_zmm_zmmm512b64_er, zmm1, zmm2, __[rax]);
				inst.IsBroadcast = true;
				inst.RoundingControl = RoundingControl.RoundUp;
				TestAssembler(c => c.vaddpd(zmm1, zmm2, __dword_bcst[rax].ru_sae), inst);
			}			
			{
				var inst = Instruction.Create(Code.EVEX_Vaddpd_zmm_k1z_zmm_zmmm512b64_er, zmm1, zmm2, __[rax]);
				inst.IsBroadcast = true;
				inst.RoundingControl = RoundingControl.RoundToNearest;
				TestAssembler(c => c.vaddpd(zmm1, zmm2, __dword_bcst[rax].rn_sae), inst);
			}			
			{
				var inst = Instruction.Create(Code.EVEX_Vaddpd_zmm_k1z_zmm_zmmm512b64_er, zmm1, zmm2, __[rax]);
				inst.IsBroadcast = true;
				inst.RoundingControl = RoundingControl.RoundTowardZero;
				TestAssembler(c => c.vaddpd(zmm1, zmm2, __dword_bcst[rax].rz_sae), inst);
			}			
			*/
		}
	}
}
