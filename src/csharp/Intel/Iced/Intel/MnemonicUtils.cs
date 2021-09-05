// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Iced.Intel {
	/// <summary>
	/// Extension methods
	/// </summary>
	public static class MnemonicUtils {
		/// <summary>
		/// Gets the mnemonic
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Mnemonic Mnemonic(this Code code) {
			Debug.Assert((uint)code < (uint)MnemonicUtilsData.toMnemonic.Length);
			return (Mnemonic)MnemonicUtilsData.toMnemonic[(int)code];
		}
	}
}
