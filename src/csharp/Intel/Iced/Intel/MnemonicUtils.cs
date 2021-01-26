// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

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
		[System.Obsolete("Use " + nameof(Mnemonic) + " instead of this method", true)]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public static Mnemonic ToMnemonic(this Code code) => code.Mnemonic();

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
