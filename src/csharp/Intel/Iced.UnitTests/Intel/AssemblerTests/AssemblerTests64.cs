using System;
using Iced.Intel;
using Iced.UnitTests.Intel.EncoderTests;
using Xunit;

using static Iced.Intel.AssemblerRegisters;

namespace Iced.UnitTests.Intel.AssemblerTests {
	public sealed partial class AssemblerTests64 {

		[Fact]
		public void TestManualInvalid() {
			// pop_regSegment
			AssertInvalid(() => {
				TestAssembler(c => c.pop(cs), default);
			});

			// push_regSegment
			// create a none register which won't match with any entries
			AssertInvalid(() => {
				TestAssembler(c => c.push(new AssemblerRegisterSegment()), default);
			});
		}

		[Fact]
		public void TestInvalidStateAssembler() {
			{
				var writer = new CodeWriterImpl();
				var assembler = Assembler.Create(Bitness, writer);
				var ex = Assert.Throws<InvalidOperationException>(() => assembler.rep.Encode());
				Assert.Contains("Unused prefixes", ex.Message);
			}
			{
				var writer = new CodeWriterImpl();
				var assembler = Assembler.Create(Bitness, writer);
				var label = assembler.CreateLabel(("BadLabel"));
				assembler.Label(ref label);
				var ex = Assert.Throws<InvalidOperationException>(() => assembler.Encode());
				Assert.Contains("Unused label", ex.Message);
			}
		}
		
		[Fact]
		public void TestLabelRIP() {
			{
				var writer = new CodeWriterImpl();
				var c = Assembler.Create(Bitness, writer);
				var label1 = c.CreateLabel();
				c.nop();
				c.nop();
				c.nop();
				c.Label(ref label1);		
				c.nop();
				var result = c.Encode(0x100, BlockEncoderOptions.ReturnNewInstructionOffsets);
				var label1RIP = result.GetLabelRIP(label1);
				Assert.Equal((ulong)0x103, label1RIP);
			}
			{
				var writer = new CodeWriterImpl();
				var c = Assembler.Create(Bitness, writer);
				var label1 = c.CreateLabel();
				c.nop();
				c.Label(ref label1);				
				c.nop();
				
				// Cannot use a label not created via CreateLabel
				var emptyLabel = new Label();
				Assert.Throws<ArgumentException>(() => c.Label(ref emptyLabel));
				
				// Cannot use a label already emitted
				Assert.Throws<ArgumentException>(() => c.Label(ref label1));
				
				var result = c.Encode();
				// Will throw without BlockEncoderOptions.ReturnNewInstructionOffsets
				Assert.Throws<ArgumentOutOfRangeException>(() => result.GetLabelRIP(label1));
			}
		}

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
				var inst = Instruction.Create(Code.Xchg_rm64_r64, __[rdx].ToMemoryOperand(64), rax);
				inst.HasXacquirePrefix = true;
				TestAssembler( c=> c.xacquire.xchg(__[rdx], rax), inst);
			}
			
			{
				var inst = Instruction.Create(Code.Xchg_rm64_r64, __[rdx].ToMemoryOperand(64), rax);
				inst.HasLockPrefix = true;
				TestAssembler( c=> c.@lock.xchg(__[rdx], rax), inst);
			}
			
			{
				var inst = Instruction.Create(Code.Xchg_rm64_r64, __[rdx].ToMemoryOperand(64), rax);
				inst.HasXreleasePrefix = true;
				TestAssembler( c=> c.xrelease.xchg(__[rdx], rax), inst);
			}

			{
				var inst = Instruction.Create(Code.Call_rm64, __[rax].ToMemoryOperand(64));
				inst.SegmentPrefix = Register.DS;
				TestAssembler( c=> c.notrack.call(__qword_ptr[rax]), inst);
			}

			{
				var inst = Instruction.Create(Code.Call_rm64, __[rax].ToMemoryOperand(64));
				inst.SegmentPrefix = Register.DS;
				inst.HasRepnePrefix = true;
				TestAssembler( c=> c.bnd.notrack.call(__qword_ptr[rax]), inst);
			}
		}
		
		[Fact]
		public void TestOperandModifiers() {
			{
				var inst = Instruction.Create(Code.EVEX_Vunpcklps_xmm_k1z_xmm_xmmm128b32, xmm2, xmm6, __[rax].ToMemoryOperand(64));
				inst.ZeroingMasking = true;
				inst.OpMask = Register.K1;
				inst.IsBroadcast = true;
				TestAssembler(c => c.vunpcklps(xmm2.k1.z, xmm6, __dword_bcst[rax]), inst, LocalOpCodeFlags.PreferEvex);
			}
			{
				var inst = Instruction.Create(Code.EVEX_Vunpcklps_xmm_k1z_xmm_xmmm128b32, xmm2, xmm6, __[rax].ToMemoryOperand(64));
				inst.ZeroingMasking = true;
				inst.OpMask = Register.K2;
				inst.IsBroadcast = true;
				TestAssembler(c => c.vunpcklps(xmm2.k2.z, xmm6, __dword_bcst[rax]), inst, LocalOpCodeFlags.PreferEvex);
			}
			{
				var inst = Instruction.Create(Code.EVEX_Vunpcklps_xmm_k1z_xmm_xmmm128b32, xmm2, xmm6, __[rax].ToMemoryOperand(64));
				inst.ZeroingMasking = true;
				inst.OpMask = Register.K3;
				inst.IsBroadcast = true;
				TestAssembler(c => c.vunpcklps(xmm2.k3.z, xmm6, __dword_bcst[rax]), inst, LocalOpCodeFlags.PreferEvex);
			}
			{
				var inst = Instruction.Create(Code.EVEX_Vunpcklps_xmm_k1z_xmm_xmmm128b32, xmm2, xmm6, __[rax].ToMemoryOperand(64));
				inst.ZeroingMasking = true;
				inst.OpMask = Register.K4;
				inst.IsBroadcast = true;
				TestAssembler(c => c.vunpcklps(xmm2.k4.z, xmm6, __dword_bcst[rax]), inst, LocalOpCodeFlags.PreferEvex);
			}
			{
				var inst = Instruction.Create(Code.EVEX_Vunpcklps_xmm_k1z_xmm_xmmm128b32, xmm2, xmm6, __[rax].ToMemoryOperand(64));
				inst.ZeroingMasking = true;
				inst.OpMask = Register.K5;
				inst.IsBroadcast = true;
				TestAssembler(c => c.vunpcklps(xmm2.k5.z, xmm6, __dword_bcst[rax]), inst, LocalOpCodeFlags.PreferEvex);
			}
			{
				var inst = Instruction.Create(Code.EVEX_Vunpcklps_xmm_k1z_xmm_xmmm128b32, xmm2, xmm6, __[rax].ToMemoryOperand(64));
				inst.ZeroingMasking = true;
				inst.OpMask = Register.K6;
				inst.IsBroadcast = true;
				TestAssembler(c => c.vunpcklps(xmm2.k6.z, xmm6, __dword_bcst[rax]), inst, LocalOpCodeFlags.PreferEvex);
			}
			{
				var inst = Instruction.Create(Code.EVEX_Vunpcklps_xmm_k1z_xmm_xmmm128b32, xmm2, xmm6, __[rax].ToMemoryOperand(64));
				inst.ZeroingMasking = true;
				inst.OpMask = Register.K7;
				inst.IsBroadcast = true;
				TestAssembler(c => c.vunpcklps(xmm2.k7.z, xmm6, __dword_bcst[rax]), inst, LocalOpCodeFlags.PreferEvex);
			}
			
			{
				var inst = Instruction.Create(Code.EVEX_Vcvttss2si_r64_xmmm32_sae, rax, xmm1);
				inst.SuppressAllExceptions = true;
				TestAssembler(c => c.vcvttss2si(rax, xmm1.sae), inst, LocalOpCodeFlags.PreferEvex);
			}			
			{
				var inst = Instruction.Create(Code.EVEX_Vaddpd_zmm_k1z_zmm_zmmm512b64_er, zmm1, zmm2, zmm3);
				inst.OpMask = Register.K1;
				inst.RoundingControl = RoundingControl.RoundDown;
				TestAssembler(c => c.vaddpd(zmm1.k1, zmm2, zmm3.rd_sae), inst);
			}			
			{
				var inst = Instruction.Create(Code.EVEX_Vaddpd_zmm_k1z_zmm_zmmm512b64_er, zmm1, zmm2, zmm3);
				inst.OpMask = Register.K1;
				inst.ZeroingMasking = true;
				inst.RoundingControl = RoundingControl.RoundUp;
				TestAssembler(c => c.vaddpd(zmm1.k1.z, zmm2, zmm3.ru_sae), inst);
			}			
			{
				var inst = Instruction.Create(Code.EVEX_Vaddpd_zmm_k1z_zmm_zmmm512b64_er, zmm1, zmm2, zmm3);
				inst.OpMask = Register.K2;
				inst.RoundingControl = RoundingControl.RoundToNearest;
				TestAssembler(c => c.vaddpd(zmm1.k2, zmm2, zmm3.rn_sae), inst);
			}			
			{
				var inst = Instruction.Create(Code.EVEX_Vaddpd_zmm_k1z_zmm_zmmm512b64_er, zmm1, zmm2, zmm3);
				inst.OpMask = Register.K3;
				inst.ZeroingMasking = true;
				inst.RoundingControl = RoundingControl.RoundTowardZero;
				TestAssembler(c => c.vaddpd(zmm1.k3.z, zmm2, zmm3.rz_sae), inst);
			}			
		}
	}
}
