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

#if INSTR_INFO || ENCODER
namespace Iced.Intel {
	/// <summary>
	/// <see cref="Register"/> extension methods
	/// </summary>
	public static partial class RegisterExtensions {
		/// <summary>
		/// Checks if it's a segment register (<c>ES</c>, <c>CS</c>, <c>SS</c>, <c>DS</c>, <c>FS</c>, <c>GS</c>)
		/// </summary>
		/// <param name="register">Register</param>
		/// <returns></returns>
		public static bool IsSegmentRegister(this Register register) => Register.ES <= register && register <= Register.GS;

		/// <summary>
		/// Checks if it's a general purpose register (<c>AL</c>-<c>R15L</c>, <c>AX</c>-<c>R15W</c>, <c>EAX</c>-<c>R15D</c>, <c>RAX</c>-<c>R15</c>)
		/// </summary>
		/// <param name="register">Register</param>
		/// <returns></returns>
		public static bool IsGPR(this Register register) => Register.AL <= register && register <= Register.R15;

		/// <summary>
		/// Checks if it's an 8-bit general purpose register (<c>AL</c>-<c>R15L</c>)
		/// </summary>
		/// <param name="register">Register</param>
		/// <returns></returns>
		public static bool IsGPR8(this Register register) => Register.AL <= register && register <= Register.R15L;

		/// <summary>
		/// Checks if it's a 16-bit general purpose register (<c>AX</c>-<c>R15W</c>)
		/// </summary>
		/// <param name="register">Register</param>
		/// <returns></returns>
		public static bool IsGPR16(this Register register) => Register.AX <= register && register <= Register.R15W;

		/// <summary>
		/// Checks if it's a 32-bit general purpose register (<c>EAX</c>-<c>R15D</c>)
		/// </summary>
		/// <param name="register">Register</param>
		/// <returns></returns>
		public static bool IsGPR32(this Register register) => Register.EAX <= register && register <= Register.R15D;

		/// <summary>
		/// Checks if it's a 64-bit general purpose register (<c>RAX</c>-<c>R15</c>)
		/// </summary>
		/// <param name="register">Register</param>
		/// <returns></returns>
		public static bool IsGPR64(this Register register) => Register.RAX <= register && register <= Register.R15;

		/// <summary>
		/// Checks if it's a 128-bit vector register (<c>XMM0</c>-<c>XMM31</c>)
		/// </summary>
		/// <param name="register">Register</param>
		/// <returns></returns>
		public static bool IsXMM(this Register register) => Register.XMM0 <= register && register <= IcedConstants.XMM_last;

		/// <summary>
		/// Checks if it's a 256-bit vector register (<c>YMM0</c>-<c>YMM31</c>)
		/// </summary>
		/// <param name="register">Register</param>
		/// <returns></returns>
		public static bool IsYMM(this Register register) => Register.YMM0 <= register && register <= IcedConstants.YMM_last;

		/// <summary>
		/// Checks if it's a 512-bit vector register (<c>ZMM0</c>-<c>ZMM31</c>)
		/// </summary>
		/// <param name="register">Register</param>
		/// <returns></returns>
		public static bool IsZMM(this Register register) => Register.ZMM0 <= register && register <= IcedConstants.ZMM_last;

		/// <summary>
		/// Checks if it's <c>EIP</c>/<c>RIP</c>
		/// </summary>
		/// <param name="register">Register</param>
		/// <returns></returns>
		public static bool IsIP(this Register register) => register == Register.EIP || register == Register.RIP;

		/// <summary>
		/// Checks if it's an opmask register (<c>K0</c>-<c>K7</c>)
		/// </summary>
		/// <param name="register">Register</param>
		/// <returns></returns>
		public static bool IsK(this Register register) => Register.K0 <= register && register <= Register.K7;

		/// <summary>
		/// Checks if it's a control register (<c>CR0</c>-<c>CR15</c>)
		/// </summary>
		/// <param name="register">Register</param>
		/// <returns></returns>
		public static bool IsCR(this Register register) => Register.CR0 <= register && register <= Register.CR15;

		/// <summary>
		/// Checks if it's a debug register (<c>DR0</c>-<c>DR15</c>)
		/// </summary>
		/// <param name="register">Register</param>
		/// <returns></returns>
		public static bool IsDR(this Register register) => Register.DR0 <= register && register <= Register.DR15;

		/// <summary>
		/// Checks if it's a test register (<c>TR0</c>-<c>TR7</c>)
		/// </summary>
		/// <param name="register">Register</param>
		/// <returns></returns>
		public static bool IsTR(this Register register) => Register.TR0 <= register && register <= Register.TR7;

		/// <summary>
		/// Checks if it's an FPU stack register (<c>ST0</c>-<c>ST7</c>)
		/// </summary>
		/// <param name="register">Register</param>
		/// <returns></returns>
		public static bool IsST(this Register register) => Register.ST0 <= register && register <= Register.ST7;

		/// <summary>
		/// Checks if it's a bound register (<c>BND0</c>-<c>BND3</c>)
		/// </summary>
		/// <param name="register">Register</param>
		/// <returns></returns>
		public static bool IsBND(this Register register) => Register.BND0 <= register && register <= Register.BND3;

		/// <summary>
		/// Checks if it's an MMX register (<c>MM0</c>-<c>MM7</c>)
		/// </summary>
		/// <param name="register">Register</param>
		/// <returns></returns>
		public static bool IsMM(this Register register) => Register.MM0 <= register && register <= Register.MM7;

		/// <summary>
		/// Checks if it's an <c>XMM</c>, <c>YMM</c> or <c>ZMM</c> register
		/// </summary>
		/// <param name="register">Register</param>
		/// <returns></returns>
		public static bool IsVectorRegister(this Register register) => Register.XMM0 <= register && register <= IcedConstants.VMM_last;
	}
}
#endif
