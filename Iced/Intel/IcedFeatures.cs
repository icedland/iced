/*
    Copyright (C) 2018-2019 de4dot@gmail.com

    This file is part of Iced.

    Iced is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Iced is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Iced.  If not, see <https://www.gnu.org/licenses/>.
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
