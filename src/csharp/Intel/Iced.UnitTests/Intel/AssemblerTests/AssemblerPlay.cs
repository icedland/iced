using System;
using static Iced.Intel.AssemblerRegisters;

namespace Iced.Intel
{
	/// <summary>
	/// Example playground - will be replaced by proper tests
	/// </summary>
	public static class ExtendedEncoderPlay {
		public static void Play() {

			var c = Assembler.Create(64, new MyCodeWriter());
			c.PreferVex = false;
			var label1 = c.CreateLabel();

			var test = Instruction.Create(Code.Mov_RAX_moffs64, Register.RAX, 0x1234567890abcdf);
			
			c.push(rax);
			c.jne(label1);
			for (int i = 0; i < 4; i++) {
				c.mov(__dword_ptr[rax * 8 + rdx + i], eax);
			}
			c.mov(rax, rdx);
			c.syscall();
			
			c.Label(label1);
			
			c.stosb();
			c.stosw();
			c.stosd();
			c.rep.stosd();
			c.repe.movsd();
			c.repne.stosd();

			c.vunpcklps(xmm2.k5.z, xmm6, __dword_bcst[rax + 4]);
			
			c.ret();

			var result = c.Encode();
			foreach (var offset in result.NewInstructionOffsets) {
				Console.WriteLine(offset);
			}
		}

		public class MyCodeWriter  : CodeWriter {
			/// <summary>
			/// Writes the next byte
			/// </summary>
			/// <param name="value">Value</param>
			public override void WriteByte(byte value) {
				Console.Write($"{value:x2} ");
			}
		}
	}
}
