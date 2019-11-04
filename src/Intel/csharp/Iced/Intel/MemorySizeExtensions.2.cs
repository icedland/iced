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

#if !NO_INSTR_INFO || !NO_ENCODER
namespace Iced.Intel {
	/// <summary>
	/// <see cref="MemorySize"/> extension methods
	/// </summary>
	public static partial class MemorySizeExtensions {
		/// <summary>
		/// Checks if <paramref name="memorySize"/> is a broadcast memory type
		/// </summary>
		/// <param name="memorySize">Memory size</param>
		/// <returns></returns>
		public static bool IsBroadcast(this MemorySize memorySize) => memorySize >= FirstBroadcastMemorySize;
		const MemorySize FirstBroadcastMemorySize = MemorySize.Broadcast64_UInt32;
	}
}
#endif
