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

#if (!NO_DECODER32 || !NO_DECODER64) && !NO_DECODER
using System;

namespace Iced.Intel {
	/// <summary>
	/// Decoder options
	/// </summary>
	[Flags]
	public enum DecoderOptions : uint {
		/// <summary>
		/// No option is enabled
		/// </summary>
		None						= 0,

		/// <summary>
		/// AMD decoder: allow 16-bit branch/ret instructions in 64-bit mode
		/// </summary>
		AMD							= 0x00000001,

		/// <summary>
		/// Decode opcodes 0F0D and 0F18-0F1F as reserved-nop instructions (eg. <see cref="Code.ReservedNop_rm32_r32_0F1D"/>)
		/// </summary>
		ForceReservedNop			= 0x00000002,

		/// <summary>
		/// Decode <see cref="Code.Cflsh"/>
		/// </summary>
		Cflsh						= 0x00000004,

		/// <summary>
		/// Decode umov instructions (eg. <see cref="Code.Umov_r32_rm32"/>)
		/// </summary>
		Umov						= 0x00000008,

		/// <summary>
		/// Decode <see cref="Code.Rdecr"/> and <see cref="Code.Wrecr"/>
		/// </summary>
		Ecr							= 0x00000010,

		/// <summary>
		/// Decode xbts/ibts
		/// </summary>
		Xbts						= 0x00000020,

		/// <summary>
		/// Decode 0FA6/0FA7 as cmpxchg
		/// </summary>
		Cmpxchg486A					= 0x00000040,

		/// <summary>
		/// Decode <see cref="Code.Zalloc_m256"/>
		/// </summary>
		Zalloc						= 0x00000080,

		/// <summary>
		/// Decode some old removed FPU instructions (eg. frstpm)
		/// </summary>
		OldFpu						= 0x00000100,

		/// <summary>
		/// Decode <see cref="Code.Pcommit"/>
		/// </summary>
		Pcommit						= 0x00000200,

		/// <summary>
		/// Decode 286 loadall (0F04 and 0F05)
		/// </summary>
		Loadall286					= 0x00000400,

		/// <summary>
		/// Decode <see cref="Code.Loadall386"/>
		/// </summary>
		Loadall386					= 0x00000800,

		/// <summary>
		/// Decode <see cref="Code.Cl1invmb"/>
		/// </summary>
		Cl1invmb					= 0x00001000,

		/// <summary>
		/// Decode <see cref="Code.Mov_r32_tr"/> and <see cref="Code.Mov_tr_r32"/>
		/// </summary>
		MovTr						= 0x00002000,

		/// <summary>
		/// Don't decode <see cref="Code.Pause"/>, decode <see cref="Code.Nopd"/>/etc instead
		/// </summary>
		NoPause						= 0x00004000,

		/// <summary>
		/// Don't decode <see cref="Code.Wbnoinvd"/>, decode <see cref="Code.Wbinvd"/> instead
		/// </summary>
		NoWbnoinvd					= 0x00008000,

		/// <summary>
		/// Don't decode LOCK MOV CR0 as MOV CR8
		/// </summary>
		NoLockMovCR0				= 0x00010000,

		/// <summary>
		/// Don't decode <see cref="Code.Popcnt_r32_rm32"/>/etc, decode eg. <see cref="Code.Jmpe_disp32"/>/etc instead
		/// </summary>
		NoMPFX_0FB8					= 0x00020000,

		/// <summary>
		/// Don't decode <see cref="Code.Tzcnt_r32_rm32"/>/etc, decode <see cref="Code.Bsf_r32_rm32"/>/etc instead
		/// </summary>
		NoMPFX_0FBC					= 0x00040000,

		/// <summary>
		/// Don't decode <see cref="Code.Lzcnt_r32_rm32"/>/etc, decode <see cref="Code.Bsr_r32_rm32"/>/etc instead
		/// </summary>
		NoMPFX_0FBD					= 0x00080000,

		/// <summary>
		/// Don't decode <see cref="Code.Lahf"/> and <see cref="Code.Sahf"/> in 64-bit mode
		/// </summary>
		NoLahfSahf64				= 0x00100000,
	}
}
#endif
