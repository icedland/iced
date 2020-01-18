using Iced.Intel;
using Xunit;
using static Iced.Intel.AssemblerRegisters;

namespace Iced.UnitTests.Intel.AssemblerTests {
	public class AssemblerRegisterTests {

		[Fact]
		public void TestMemoryOperands() {
			{
				Assert.NotEqual(st0, st1);

				Assert.Equal(Register.ST0, st0);
				Assert.Equal(Register.ST7, st7);
				Assert.Equal(Register.ES, es);
				Assert.Equal(Register.DS, ds);
				Assert.Equal(Register.CR0, cr0);
				Assert.Equal(Register.CR15, cr15);
				Assert.Equal(Register.TR0, tr0);
				Assert.Equal(Register.TR7, tr7);
				Assert.Equal(Register.DR0, dr0);
				Assert.Equal(Register.DR7, dr7);
				Assert.Equal(Register.K1, k1);
				Assert.Equal(Register.K7, k7);
				Assert.Equal(Register.AL, al);
				Assert.Equal(Register.AX, ax);
				Assert.Equal(Register.EAX, eax);
				Assert.Equal(Register.RAX, rax);
				Assert.Equal(Register.MM1, mm1);
				Assert.Equal(Register.MM7, mm7);
				Assert.Equal(Register.XMM1, xmm1);
				Assert.Equal(Register.XMM15, xmm15);
				Assert.Equal(Register.YMM1, ymm1);
				Assert.Equal(Register.YMM15, ymm15);
				Assert.Equal(Register.ZMM1, zmm1);
				Assert.Equal(Register.ZMM15, zmm15);
			}
			
			{
				Assert.Equal(AssemblerOperandFlags.K1, zmm0.k1.Flags);
				Assert.Equal(AssemblerOperandFlags.SuppressAllExceptions, zmm0.sae.Flags);
				Assert.Equal(AssemblerOperandFlags.K2 | AssemblerOperandFlags.Zeroing, zmm0.k2.z.Flags);

				Assert.NotEqual(zmm0, zmm1);
				Assert.NotEqual(zmm0, zmm0.k1);
				Assert.NotEqual(zmm0.GetHashCode(), zmm1.GetHashCode());
				Assert.NotEqual(zmm0.GetHashCode(), zmm0.k1.GetHashCode());
			}
			
			{
				var m = __[sbyte.MinValue];
				Assert.Equal(new MemoryOperand(Register.None, Register.None, 1, sbyte.MinValue, 1), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.None, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}
			
			{
				var m = __[sbyte.MaxValue];
				Assert.Equal(new MemoryOperand(Register.None, Register.None, 1, sbyte.MaxValue, 1), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.None, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}
			
			{
				var m = __[short.MinValue];
				Assert.Equal(new MemoryOperand(Register.None, Register.None, 1, short.MinValue, 2), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.None, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}

			{
				var m = __[short.MaxValue];
				Assert.Equal(new MemoryOperand(Register.None, Register.None, 1, short.MaxValue, 2), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.None, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}

			{
				var m = __[int.MinValue];
				Assert.Equal(new MemoryOperand(Register.None, Register.None, 1, int.MinValue, 4), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.None, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}

			{
				var m = __[int.MaxValue];
				Assert.Equal(new MemoryOperand(Register.None, Register.None, 1, int.MaxValue, 4), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.None, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}

			{
				var m = __[(long)int.MaxValue + 1];
				Assert.Equal(new MemoryOperand(Register.None, Register.None, 1, unchecked((int)((long)int.MaxValue + 1)), 8), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.None, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}

			{
				var m = __[eax];
          		Assert.Equal(new MemoryOperand(Register.EAX, Register.None, 1, 0, 0), m.ToMemoryOperand(64));
           		Assert.Equal(MemoryOperandSize.None, m.Size);
           		Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}

			{
				var m = __word_ptr[eax];
          		Assert.Equal(new MemoryOperand(Register.EAX, Register.None, 1, 0, 0), m.ToMemoryOperand(64));
           		Assert.Equal(MemoryOperandSize.WordPtr, m.Size);
           		Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}

			{
				var m = __dword_ptr[eax];
          		Assert.Equal(new MemoryOperand(Register.EAX, Register.None, 1, 0, 0), m.ToMemoryOperand(64));
           		Assert.Equal(MemoryOperandSize.DwordPtr, m.Size);
           		Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}

			{
				var m = __qword_ptr[eax];
          		Assert.Equal(new MemoryOperand(Register.EAX, Register.None, 1, 0, 0), m.ToMemoryOperand(64));
           		Assert.Equal(MemoryOperandSize.QwordPtr, m.Size);
           		Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}

			{
				var m = __xmmword_ptr[eax];
				Assert.Equal(new MemoryOperand(Register.EAX, Register.None, 1, 0, 0), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.OwordPtr, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}

			{
				var m = __ymmword_ptr[eax];
				Assert.Equal(new MemoryOperand(Register.EAX, Register.None, 1, 0, 0), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.YwordPtr, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}
			
			{
				var m = __zmmword_ptr[eax];
				Assert.Equal(new MemoryOperand(Register.EAX, Register.None, 1, 0, 0), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.ZwordPtr, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}
			
			{
				var m = __[eax + edx];
				Assert.Equal(new MemoryOperand(Register.EAX, Register.EDX, 1, 0, 0), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.None, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}
			
			{
				var m = __[eax + xmm1];
				Assert.Equal(new MemoryOperand(Register.EAX, Register.XMM1, 1, 0, 0), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.None, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}

			{
				var m = __[eax + ymm1];
				Assert.Equal(new MemoryOperand(Register.EAX, Register.YMM1, 1, 0, 0), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.None, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}

			{
				var m = __[eax + zmm1];
				Assert.Equal(new MemoryOperand(Register.EAX, Register.ZMM1, 1, 0, 0), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.None, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}

			{
				var m = __[rsi + rdi * 2 + 512];
				Assert.Equal(new MemoryOperand(Register.RSI, Register.RDI, 2, 512, 2), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.None, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}

			{
				var m = __[eax + edx * 4 + 8];
				Assert.Equal(new MemoryOperand(Register.EAX, Register.EDX, 4, 8, 1), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.None, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}
			
			{
				var m = __[rsi + rdi * 2 + 124];
				Assert.Equal(new MemoryOperand(Register.RSI, Register.RDI, 2, 512, 1), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.None, m.Size);
				Assert.Equal(AssemblerOperandFlags.None, m.Flags);
			}
			
			{
				var m = __dword_bcst[rsi + rdi * 2 + 124];
				Assert.Equal(new MemoryOperand(Register.RSI, Register.RDI, 2, 512, 1), m.ToMemoryOperand(64));
				Assert.Equal(MemoryOperandSize.DwordPtr, m.Size);
				Assert.Equal(AssemblerOperandFlags.Broadcast, m.Flags);
			}

			{
				var m1 = __qword_bcst[rsi + rdi * 2 + 512];
				var m2 = __dword_bcst[rsi + rdi * 2 + 512];
				Assert.NotEqual(m1, m2);
				Assert.Equal(m1.ToMemoryOperand(64), m2.ToMemoryOperand(64));
				Assert.NotEqual(m1.GetHashCode(), m2.GetHashCode());
			}
		}
	}
}
