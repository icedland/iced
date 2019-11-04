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

#if !NO_ENCODER
namespace Iced.Intel {
	/// <summary>
	/// Mandatory prefix
	/// </summary>
	public enum MandatoryPrefix {
		/// <summary>
		/// No mandatory prefix (legacy and 3DNow! tables only)
		/// </summary>
		None,

		/// <summary>
		/// Empty mandatory prefix (no 66, F3 or F2 prefix)
		/// </summary>
		PNP,

		/// <summary>
		/// 66 prefix
		/// </summary>
		P66,

		/// <summary>
		/// F3 prefix
		/// </summary>
		PF3,

		/// <summary>
		/// F2 prefix
		/// </summary>
		PF2,
	}
}
#endif
