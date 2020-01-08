using System;
using static Iced.Intel.ExtendedRegisters;

namespace Iced.Intel
{
	/// <summary>
	/// Example playground - will be replaced by proper tests
	/// </summary>
	public static class ExtendedEncoderPlay {
		public static void Play() {

			var c = ExtendedEncoder.Create(64, new MyCodeWriter());

			var label1 = c.CreateLabel();
						
			c.push(rax);
			c.jne(label1);
			for (int i = 0; i < 4; i++) {
				c.mov(__dword_ptr[rax * 8 + rdx + i], eax);
			}
			c.mov(rax, rdx);
			c.syscall();
			
			c.label(label1);
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
