// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if ENCODER && OPCODE_INFO
using System.Runtime.CompilerServices;
using Iced.Intel.EncoderInternal;

namespace Iced.Intel {
	/// <summary>
	/// Extensions
	/// </summary>
	public static class EncoderCodeExtensions {
		/// <summary>
		/// Gets a <see cref="OpCodeInfo"/>
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static OpCodeInfo ToOpCode(this Code code) {
			var infos = OpCodeInfos.Infos;
			if ((uint)code >= (uint)infos.Length)
				ThrowHelper.ThrowArgumentOutOfRangeException_code();
			return infos[(int)code];
		}
	}
}
#endif
