// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System.Runtime.CompilerServices;

namespace Iced.Intel {
	/// <summary>
	/// Gets the available features
	/// </summary>
	public static class IcedFeatures {
		/// <summary>
		/// <see langword="true"/> if the gas (AT&amp;T) formatter is available
		/// </summary>
		public static bool HasGasFormatter {
			get {
#if GAS
				return true;
#else
				return false;
#endif
			}
		}

		/// <summary>
		/// <see langword="true"/> if the Intel (xed) formatter is available
		/// </summary>
		public static bool HasIntelFormatter {
			get {
#if INTEL
				return true;
#else
				return false;
#endif
			}
		}

		/// <summary>
		/// <see langword="true"/> if the masm formatter is available
		/// </summary>
		public static bool HasMasmFormatter {
			get {
#if MASM
				return true;
#else
				return false;
#endif
			}
		}

		/// <summary>
		/// <see langword="true"/> if the nasm formatter is available
		/// </summary>
		public static bool HasNasmFormatter {
			get {
#if NASM
				return true;
#else
				return false;
#endif
			}
		}

		/// <summary>
		/// <see langword="true"/> if the fast formatter is available
		/// </summary>
		public static bool HasFastFormatter {
			get {
#if FAST_FMT
				return true;
#else
				return false;
#endif
			}
		}

		/// <summary>
		/// <see langword="true"/> if the decoder is available
		/// </summary>
		public static bool HasDecoder {
			get {
#if DECODER
				return true;
#else
				return false;
#endif
			}
		}

		/// <summary>
		/// <see langword="true"/> if the encoder is available
		/// </summary>
		public static bool HasEncoder {
			get {
#if ENCODER
				return true;
#else
				return false;
#endif
			}
		}

		/// <summary>
		/// <see langword="true"/> if the block encoder is available
		/// </summary>
		public static bool HasBlockEncoder {
			get {
#if ENCODER && BLOCK_ENCODER
				return true;
#else
				return false;
#endif
			}
		}

		/// <summary>
		/// <see langword="true"/> if the opcode info is available
		/// </summary>
		public static bool HasOpCodeInfo {
			get {
#if ENCODER && OPCODE_INFO
				return true;
#else
				return false;
#endif
			}
		}

		/// <summary>
		/// <see langword="true"/> if the instruction info code is available
		/// </summary>
		public static bool HasInstructionInfo {
			get {
#if INSTR_INFO
				return true;
#else
				return false;
#endif
			}
		}

		/// <summary>
		/// Initializes some static constructors related to the decoder and instruction info. If those
		/// static constructors are initialized, the jitter generates faster code since it doesn't have
		/// to add runtime checks to see if those static constructors must be called.
		/// 
		/// This method should be called before using the decoder and instruction info classes and
		/// should *not* be called from the same method as any code that uses the decoder / instruction
		/// info classes. Eg. call this method from Main() and decode instructions / get instruction info
		/// in a method called by Main().
		/// </summary>
		public static void Initialize() {
#if DECODER
			// The decoder already initializes this stuff, but when it's called, it's a little bit too late.
			RuntimeHelpers.RunClassConstructor(typeof(Decoder).TypeHandle);
#endif
		}
	}
}
