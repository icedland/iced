using static Iced.Intel.ExtendedRegisters;

namespace Iced.Intel
{
	/// <summary>
	/// Example playground - will be replaced by proper tests
	/// </summary>
	public static class ExtendedEncoderPlay {
		public static void Play() {

			var c = ExtendedEncoder.Create(64, new MyCodeWriter());

			c.push(rax);
			c.syscall();
			c.retnq();

			c.Encode();
		}

		public class MyCodeWriter  : CodeWriter {
			/// <summary>
			/// Writes the next byte
			/// </summary>
			/// <param name="value">Value</param>
			public override void WriteByte(byte value) {

			}
		}
	}
}
