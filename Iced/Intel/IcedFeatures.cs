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

namespace Iced.Intel {
	/// <summary>
	/// Gets the available features
	/// </summary>
	public static class IcedFeatures {
		/// <summary>
		/// true if the gas (AT&amp;T) formatter is available
		/// </summary>
		public static bool HasGasFormatter {
			get {
#if !NO_GAS_FORMATTER && !NO_FORMATTER
				return true;
#else
				return false;
#endif
			}
		}

		/// <summary>
		/// true if the Intel (xed) formatter is available
		/// </summary>
		public static bool HasIntelFormatter {
			get {
#if !NO_INTEL_FORMATTER && !NO_FORMATTER
				return true;
#else
				return false;
#endif
			}
		}

		/// <summary>
		/// true if the masm formatter is available
		/// </summary>
		public static bool HasMasmFormatter {
			get {
#if !NO_MASM_FORMATTER && !NO_FORMATTER
				return true;
#else
				return false;
#endif
			}
		}

		/// <summary>
		/// true if the nasm formatter is available
		/// </summary>
		public static bool HasNasmFormatter {
			get {
#if !NO_NASM_FORMATTER && !NO_FORMATTER
				return true;
#else
				return false;
#endif
			}
		}

		/// <summary>
		/// true if the 16/32-bit decoder is available
		/// </summary>
		public static bool HasDecoder32 {
			get {
#if !NO_DECODER32 && !NO_DECODER
				return true;
#else
				return false;
#endif
			}
		}

		/// <summary>
		/// true if the 64-bit decoder is available
		/// </summary>
		public static bool HasDecoder64 {
			get {
#if !NO_DECODER64 && !NO_DECODER
				return true;
#else
				return false;
#endif
			}
		}

		/// <summary>
		/// true if the encoder is available
		/// </summary>
		public static bool HasEncoder {
			get {
#if !NO_ENCODER
				return true;
#else
				return false;
#endif
			}
		}

		/// <summary>
		/// true if the instruction info code is available
		/// </summary>
		public static bool HasInstructionInfo {
			get {
#if !NO_INSTR_INFO
				return true;
#else
				return false;
#endif
			}
		}
	}
}
