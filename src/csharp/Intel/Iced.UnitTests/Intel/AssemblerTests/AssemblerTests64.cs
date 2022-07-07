// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if ENCODER && BLOCK_ENCODER && CODE_ASSEMBLER
using System;
using Iced.Intel;
using Xunit;
using static Iced.Intel.AssemblerRegisters;

namespace Iced.UnitTests.Intel.AssemblerTests {
	// Make sure it can be derived
	sealed class MyAssembler : Assembler {
		public MyAssembler() : base(64) { }
	}

	public sealed partial class AssemblerTests64 {
		[Fact]
		void xlatb() {
			TestAssembler(c => c.xlatb(), Instruction.Create(Code.Xlat_m8, new MemoryOperand(Register.RBX, Register.AL)));
		}

		[Fact]
		public void xbegin_label() {
			TestAssembler(c => c.xbegin(CreateAndEmitLabel(c)), AssignLabel(Instruction.CreateXbegin(Bitness, FirstLabelId), FirstLabelId), TestInstrFlags.Branch);
		}

		[Fact]
		public void xbegin_offset() {
			TestAssembler(c => c.xbegin(12752), Instruction.CreateXbegin(Bitness, 12752), TestInstrFlags.BranchU64 | TestInstrFlags.IgnoreCode);
		}

		[Fact]
		void Ctor() {
			var c = new Assembler(Bitness);
			Assert.Equal(Bitness, c.Bitness);
			Assert.True(c.PreferVex);
			Assert.True(c.PreferShortBranch);
			Assert.Empty(c.Instructions);
			Assert.True(c.CurrentLabel.IsEmpty);
		}

		[Fact]
		void Reset_works() {
			var c = new Assembler(Bitness);
			c.CreateLabel();
			c.add(rax, rcx);
			_ = c.@lock;
			c.PreferVex = false;
			c.PreferShortBranch = false;
			c.Reset();
			Assert.False(c.PreferVex);
			Assert.False(c.PreferShortBranch);
			Assert.Empty(c.Instructions);
			Assert.True(c.CurrentLabel.IsEmpty);
			var writer = new CodeWriterImpl();
			var result = c.Assemble(writer, 0);
			Assert.Single(result.Result);
			Assert.Empty(writer.ToArray());
		}

		[Fact]
		void Invalid_bitness_throws() {
			foreach (var bitness in BitnessUtils.GetInvalidBitnessValues())
				Assert.Throws<ArgumentOutOfRangeException>(() => new Assembler(bitness));
		}

		[Fact]
		void Assemble_throws_if_null_writer() {
			var c = new Assembler(Bitness);
			c.nop();
			Assert.Throws<ArgumentNullException>(() => c.Assemble(null, 0));
		}

		[Fact]
		void TryAssemble_throws_if_null_writer() {
			var c = new Assembler(Bitness);
			c.nop();
			Assert.Throws<ArgumentNullException>(() => c.TryAssemble(null, 0, out _, out _));
		}

		[Fact]
		void Assemble_throws_if_error() {
			var c = new Assembler(64);
			c.aaa();
			Assert.Throws<InvalidOperationException>(() => c.Assemble(new CodeWriterImpl(), 0));
		}

		[Fact]
		void TryAssemble_returns_error_string_if_it_failed() {
			var c = new Assembler(64);
			c.aaa();
			bool b = c.TryAssemble(new CodeWriterImpl(), 0, out var errorMessage, out var result);
			Assert.False(b);
			Assert.NotNull(errorMessage);
			Assert.NotNull(result.Result);
			Assert.Empty(result.Result);
		}

		[Fact]
		void Test_opmask_registers() {
			TestAssembler(c => c.vmovups(zmm0.k1, zmm1), ApplyK(Instruction.Create(Code.EVEX_Vmovups_zmm_k1z_zmmm512, zmm0, zmm1), Register.K1), TestInstrFlags.PreferEvex);
			TestAssembler(c => c.vmovups(zmm0.k2, zmm1), ApplyK(Instruction.Create(Code.EVEX_Vmovups_zmm_k1z_zmmm512, zmm0, zmm1), Register.K2), TestInstrFlags.PreferEvex);
			TestAssembler(c => c.vmovups(zmm0.k3, zmm1), ApplyK(Instruction.Create(Code.EVEX_Vmovups_zmm_k1z_zmmm512, zmm0, zmm1), Register.K3), TestInstrFlags.PreferEvex);
			TestAssembler(c => c.vmovups(zmm0.k4, zmm1), ApplyK(Instruction.Create(Code.EVEX_Vmovups_zmm_k1z_zmmm512, zmm0, zmm1), Register.K4), TestInstrFlags.PreferEvex);
			TestAssembler(c => c.vmovups(zmm0.k5, zmm1), ApplyK(Instruction.Create(Code.EVEX_Vmovups_zmm_k1z_zmmm512, zmm0, zmm1), Register.K5), TestInstrFlags.PreferEvex);
			TestAssembler(c => c.vmovups(zmm0.k6, zmm1), ApplyK(Instruction.Create(Code.EVEX_Vmovups_zmm_k1z_zmmm512, zmm0, zmm1), Register.K6), TestInstrFlags.PreferEvex);
			TestAssembler(c => c.vmovups(zmm0.k7, zmm1), ApplyK(Instruction.Create(Code.EVEX_Vmovups_zmm_k1z_zmmm512, zmm0, zmm1), Register.K7), TestInstrFlags.PreferEvex);
		}

		[Fact]
		public void TestDeclareData_db_array() {
			TestAssemblerDeclareData(c => c.db(Array.Empty<byte>()), Array.Empty<byte>());
			TestAssemblerDeclareData(c => c.db(new byte[] { 1 }), new byte[] { 1 });
			TestAssemblerDeclareData(c => c.db(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }), new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 });
			TestAssemblerDeclareData(c => c.db(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 }), new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 });
			TestAssemblerDeclareData(c => c.db(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 }), new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 });
			TestAssemblerDeclareData(c => c.db(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33 }), new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33 });
		}

#if HAS_SPAN
		[Fact]
		public void TestDeclareData_db_span() {
			TestAssemblerDeclareData(c => c.db(Array.Empty<byte>().AsSpan()), Array.Empty<byte>());
			TestAssemblerDeclareData(c => c.db(new byte[] { 1 }.AsSpan()), new byte[] { 1 });
			TestAssemblerDeclareData(c => c.db(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }.AsSpan()), new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 });
			TestAssemblerDeclareData(c => c.db(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 }.AsSpan()), new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 });
			TestAssemblerDeclareData(c => c.db(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 }.AsSpan()), new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 });
			TestAssemblerDeclareData(c => c.db(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33 }.AsSpan()), new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33 });
		}
#endif

		[Fact]
		public void TestDeclareData_db_array_index_length() {
			TestAssemblerDeclareData(c => c.db(new byte[] { 97, 98, 1, 99, 100, 101 }, 2, 1), new byte[] { 1 });
			TestAssemblerDeclareData(c => c.db(new byte[] { 97, 98, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 99, 100, 101 }, 2, 16), new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 });
			TestAssemblerDeclareData(c => c.db(new byte[] { 97, 98, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 99, 100, 101 }, 2, 17), new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 });
			TestAssemblerDeclareData(c => c.db(new byte[] { 97, 98, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 99, 100, 101 }, 2, 32), new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 });
			TestAssemblerDeclareData(c => c.db(new byte[] { 97, 98, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 99, 100, 101 }, 2, 33), new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33 });
		}

		[Fact]
		public void TestDeclareData_db_array_throws_if_null_array() {
			var assembler = new Assembler(Bitness);
			Assert.Throws<ArgumentNullException>(() => assembler.db(null));
		}

		[Fact]
		public void TestDeclareData_db_array_index_length_throws() {
			var assembler = new Assembler(Bitness);
			Assert.Throws<ArgumentNullException>(() => assembler.db(null, 0, 0));
			Assert.Throws<ArgumentNullException>(() => assembler.db(null, 1, 2));
			Assert.Throws<ArgumentOutOfRangeException>(() => assembler.db(new byte[] { }, 1, 0));
			Assert.Throws<ArgumentOutOfRangeException>(() => assembler.db(new byte[] { }, 0, 1));
			Assert.Throws<ArgumentOutOfRangeException>(() => assembler.db(new byte[] { 1, 2, 3 }, -1, 0));
			Assert.Throws<ArgumentOutOfRangeException>(() => assembler.db(new byte[] { 1, 2, 3 }, 0, -1));
			Assert.Throws<ArgumentOutOfRangeException>(() => assembler.db(new byte[] { 1, 2, 3 }, -1, -1));
			Assert.Throws<ArgumentOutOfRangeException>(() => assembler.db(new byte[] { 1, 2, 3 }, 0, 4));
			Assert.Throws<ArgumentOutOfRangeException>(() => assembler.db(new byte[] { 1, 2, 3 }, 1, 3));
			Assert.Throws<ArgumentOutOfRangeException>(() => assembler.db(new byte[] { 1, 2, 3 }, 3, 1));
			Assert.Throws<ArgumentOutOfRangeException>(() => assembler.db(new byte[] { 1, 2, 3 }, 4, 0));
			Assert.Throws<ArgumentOutOfRangeException>(() => assembler.db(new byte[] { 1, 2, 3 }, 4, -1));
		}

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
				var assembler = new Assembler(Bitness);
				var writer = new CodeWriterImpl();
				var ex = Assert.Throws<InvalidOperationException>(() => assembler.rep.Assemble(writer, 0));
				Assert.Contains("Unused prefixes", ex.Message);
			}
			{
				var assembler = new Assembler(Bitness);
				var label = assembler.CreateLabel("BadLabel");
				assembler.Label(ref label);
				var writer = new CodeWriterImpl();
				var ex = Assert.Throws<InvalidOperationException>(() => assembler.Assemble(writer, 0));
				Assert.Contains("Unused label", ex.Message);
			}
		}

		[Fact]
		public void TestLabelRIP() {
			{
				var c = new Assembler(Bitness);
				var label0 = c.CreateLabel();
				var label1 = c.CreateLabel();
				c.nop();
				c.nop();
				c.nop();
				c.Label(ref label1);
				c.nop();

				var writer = new CodeWriterImpl();
				var result = c.Assemble(writer, 0x100, BlockEncoderOptions.ReturnNewInstructionOffsets);
				var label1RIP = result.GetLabelRIP(label1);
				Assert.Equal((ulong)0x103, label1RIP);
				Assert.Throws<ArgumentOutOfRangeException>(() => result.GetLabelRIP(label1, 1));
				Assert.Throws<ArgumentException>(() => result.GetLabelRIP(label0));
			}
			{
				var c = new Assembler(Bitness);
				var label1 = c.CreateLabel();
				c.nop();
				c.Label(ref label1);
				c.nop();

				// Cannot use a label not created via CreateLabel
				var emptyLabel = new Label();
				Assert.Throws<ArgumentException>(() => c.Label(ref emptyLabel));

				// Cannot use a label already emitted
				Assert.Throws<ArgumentException>(() => c.Label(ref label1));

				var writer = new CodeWriterImpl();
				var result = c.Assemble(writer, 0);
				// Will throw without BlockEncoderOptions.ReturnNewInstructionOffsets
				Assert.Throws<ArgumentOutOfRangeException>(() => result.GetLabelRIP(label1));
				Assert.Throws<ArgumentOutOfRangeException>(() => default(AssemblerResult).GetLabelRIP(label1));
				Assert.Throws<ArgumentException>(() => result.GetLabelRIP(new Label()));
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
				inst.HasRepePrefix = true;
				TestAssembler(c => c.repz.stosd(), inst);
			}

			{
				var inst = Instruction.CreateStosd(Bitness);
				inst.HasRepnePrefix = true;
				TestAssembler(c => c.repne.stosd(), inst);
			}
			{
				var inst = Instruction.CreateStosd(Bitness);
				inst.HasRepnePrefix = true;
				TestAssembler(c => c.repnz.stosd(), inst);
			}

			{
				var inst = Instruction.Create(Code.Xchg_rm64_r64, __[rdx].ToMemoryOperand(64), rax);
				inst.HasXacquirePrefix = true;
				TestAssembler(c => c.xacquire.xchg(__[rdx], rax), inst);
			}

			{
				var inst = Instruction.Create(Code.Xchg_rm64_r64, __[rdx].ToMemoryOperand(64), rax);
				inst.HasLockPrefix = true;
				TestAssembler(c => c.@lock.xchg(__[rdx], rax), inst);
			}

			{
				var inst = Instruction.Create(Code.Xchg_rm64_r64, __[rdx].ToMemoryOperand(64), rax);
				inst.HasXreleasePrefix = true;
				TestAssembler(c => c.xrelease.xchg(__[rdx], rax), inst);
			}

			{
				var inst = Instruction.Create(Code.Call_rm64, __[rax].ToMemoryOperand(64));
				inst.SegmentPrefix = Register.DS;
				TestAssembler(c => c.notrack.call(__qword_ptr[rax]), inst);
			}

			{
				var inst = Instruction.Create(Code.Call_rm64, __[rax].ToMemoryOperand(64));
				inst.SegmentPrefix = Register.DS;
				inst.HasRepnePrefix = true;
				TestAssembler(c => c.bnd.notrack.call(__qword_ptr[rax]), inst);
			}
		}

		[Fact]
		public void TestOperandModifiers() {
			{
				var inst = Instruction.Create(Code.EVEX_Vunpcklps_xmm_k1z_xmm_xmmm128b32, xmm2, xmm6, __[rax].ToMemoryOperand(64));
				inst.ZeroingMasking = true;
				inst.OpMask = Register.K1;
				inst.IsBroadcast = true;
				TestAssembler(c => c.vunpcklps(xmm2.k1.z, xmm6, __dword_bcst[rax]), inst, TestInstrFlags.PreferEvex);
			}
			{
				var inst = Instruction.Create(Code.EVEX_Vunpcklps_xmm_k1z_xmm_xmmm128b32, xmm2, xmm6, __[rax].ToMemoryOperand(64));
				inst.ZeroingMasking = true;
				inst.OpMask = Register.K2;
				inst.IsBroadcast = true;
				TestAssembler(c => c.vunpcklps(xmm2.k2.z, xmm6, __dword_bcst[rax]), inst, TestInstrFlags.PreferEvex);
			}
			{
				var inst = Instruction.Create(Code.EVEX_Vunpcklps_xmm_k1z_xmm_xmmm128b32, xmm2, xmm6, __[rax].ToMemoryOperand(64));
				inst.ZeroingMasking = true;
				inst.OpMask = Register.K3;
				inst.IsBroadcast = true;
				TestAssembler(c => c.vunpcklps(xmm2.k3.z, xmm6, __dword_bcst[rax]), inst, TestInstrFlags.PreferEvex);
			}
			{
				var inst = Instruction.Create(Code.EVEX_Vunpcklps_xmm_k1z_xmm_xmmm128b32, xmm2, xmm6, __[rax].ToMemoryOperand(64));
				inst.ZeroingMasking = true;
				inst.OpMask = Register.K4;
				inst.IsBroadcast = true;
				TestAssembler(c => c.vunpcklps(xmm2.k4.z, xmm6, __dword_bcst[rax]), inst, TestInstrFlags.PreferEvex);
			}
			{
				var inst = Instruction.Create(Code.EVEX_Vunpcklps_xmm_k1z_xmm_xmmm128b32, xmm2, xmm6, __[rax].ToMemoryOperand(64));
				inst.ZeroingMasking = true;
				inst.OpMask = Register.K5;
				inst.IsBroadcast = true;
				TestAssembler(c => c.vunpcklps(xmm2.k5.z, xmm6, __dword_bcst[rax]), inst, TestInstrFlags.PreferEvex);
			}
			{
				var inst = Instruction.Create(Code.EVEX_Vunpcklps_xmm_k1z_xmm_xmmm128b32, xmm2, xmm6, __[rax].ToMemoryOperand(64));
				inst.ZeroingMasking = true;
				inst.OpMask = Register.K6;
				inst.IsBroadcast = true;
				TestAssembler(c => c.vunpcklps(xmm2.k6.z, xmm6, __dword_bcst[rax]), inst, TestInstrFlags.PreferEvex);
			}
			{
				var inst = Instruction.Create(Code.EVEX_Vunpcklps_xmm_k1z_xmm_xmmm128b32, xmm2, xmm6, __[rax].ToMemoryOperand(64));
				inst.ZeroingMasking = true;
				inst.OpMask = Register.K7;
				inst.IsBroadcast = true;
				TestAssembler(c => c.vunpcklps(xmm2.k7.z, xmm6, __dword_bcst[rax]), inst, TestInstrFlags.PreferEvex);
			}

			{
				var inst = Instruction.Create(Code.EVEX_Vcvttss2si_r64_xmmm32_sae, rax, xmm1);
				inst.SuppressAllExceptions = true;
				TestAssembler(c => c.vcvttss2si(rax, xmm1.sae), inst, TestInstrFlags.PreferEvex);
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

		[Fact]
		void TestVexEvexPrefixes() {
			var a = new Assembler(64);

			a.PreferVex = true;
			Assert.True(a.PreferVex);
			a.vaddpd(xmm1, xmm2, xmm3);
			a.vex.vaddpd(xmm1, xmm2, xmm3);
			a.vaddpd(xmm1, xmm2, xmm3);
			a.evex.vaddpd(xmm1, xmm2, xmm3);
			a.vaddpd(xmm1, xmm2, xmm3);
			Assert.True(a.PreferVex);

			a.PreferVex = false;
			Assert.False(a.PreferVex);
			a.vaddpd(xmm1, xmm2, xmm3);
			a.vex.vaddpd(xmm1, xmm2, xmm3);
			a.vaddpd(xmm1, xmm2, xmm3);
			a.evex.vaddpd(xmm1, xmm2, xmm3);
			a.vaddpd(xmm1, xmm2, xmm3);
			Assert.False(a.PreferVex);

			var writer = new CodeWriterImpl();
			a.Assemble(writer, 0);
			var bytes = writer.ToArray();
			Assert.Equal(new byte[] {
				0xC5, 0xE9, 0x58, 0xCB,
				0xC5, 0xE9, 0x58, 0xCB,
				0xC5, 0xE9, 0x58, 0xCB,
				0x62, 0xF1, 0xED, 0x08, 0x58, 0xCB,
				0xC5, 0xE9, 0x58, 0xCB,
				0x62, 0xF1, 0xED, 0x08, 0x58, 0xCB,
				0xC5, 0xE9, 0x58, 0xCB,
				0x62, 0xF1, 0xED, 0x08, 0x58, 0xCB,
				0x62, 0xF1, 0xED, 0x08, 0x58, 0xCB,
				0x62, 0xF1, 0xED, 0x08, 0x58, 0xCB,
			}, bytes);
		}

		[Fact]
		void Test_zero_bytes() {
			var a = new Assembler(64);

			var lblf = a.CreateLabel();
			var lbll = a.CreateLabel();
			var lbl1 = a.CreateLabel();
			var lbl2 = a.CreateLabel();

			a.Label(ref lblf);
			a.zero_bytes();

			a.je(lbl1);
			a.je(lbl2);
			a.Label(ref lbl1);
			a.zero_bytes();
			a.Label(ref lbl2);
			a.nop();
			a.@lock.rep.zero_bytes();

			a.Label(ref lbll);
			a.zero_bytes();

			var writer = new CodeWriterImpl();
			a.Assemble(writer, 0);
			var bytes = writer.ToArray();
			Assert.Equal(new byte[] { 0x74, 0x02, 0x74, 0x00, 0x90 }, bytes);
		}
	}
}
#endif
