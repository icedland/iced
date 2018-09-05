/*
    Copyright (C) 2018 de4dot@gmail.com

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

#if !NO_ENCODER
using System;

namespace Iced.Intel.BlockEncoderInternal {
	static class CodeUtils {
		public static Code ShortJccToNativeJcc(this Code code, int bitness) {
			Code c16, c32, c64;
			switch (code) {
			case Code.Jo_Jb16:
			case Code.Jo_Jb32:
			case Code.Jo_Jb64:
				c16 = Code.Jo_Jb16;
				c32 = Code.Jo_Jb32;
				c64 = Code.Jo_Jb64;
				break;

			case Code.Jno_Jb16:
			case Code.Jno_Jb32:
			case Code.Jno_Jb64:
				c16 = Code.Jno_Jb16;
				c32 = Code.Jno_Jb32;
				c64 = Code.Jno_Jb64;
				break;

			case Code.Jb_Jb16:
			case Code.Jb_Jb32:
			case Code.Jb_Jb64:
				c16 = Code.Jb_Jb16;
				c32 = Code.Jb_Jb32;
				c64 = Code.Jb_Jb64;
				break;

			case Code.Jae_Jb16:
			case Code.Jae_Jb32:
			case Code.Jae_Jb64:
				c16 = Code.Jae_Jb16;
				c32 = Code.Jae_Jb32;
				c64 = Code.Jae_Jb64;
				break;

			case Code.Je_Jb16:
			case Code.Je_Jb32:
			case Code.Je_Jb64:
				c16 = Code.Je_Jb16;
				c32 = Code.Je_Jb32;
				c64 = Code.Je_Jb64;
				break;

			case Code.Jne_Jb16:
			case Code.Jne_Jb32:
			case Code.Jne_Jb64:
				c16 = Code.Jne_Jb16;
				c32 = Code.Jne_Jb32;
				c64 = Code.Jne_Jb64;
				break;

			case Code.Jbe_Jb16:
			case Code.Jbe_Jb32:
			case Code.Jbe_Jb64:
				c16 = Code.Jbe_Jb16;
				c32 = Code.Jbe_Jb32;
				c64 = Code.Jbe_Jb64;
				break;

			case Code.Ja_Jb16:
			case Code.Ja_Jb32:
			case Code.Ja_Jb64:
				c16 = Code.Ja_Jb16;
				c32 = Code.Ja_Jb32;
				c64 = Code.Ja_Jb64;
				break;

			case Code.Js_Jb16:
			case Code.Js_Jb32:
			case Code.Js_Jb64:
				c16 = Code.Js_Jb16;
				c32 = Code.Js_Jb32;
				c64 = Code.Js_Jb64;
				break;

			case Code.Jns_Jb16:
			case Code.Jns_Jb32:
			case Code.Jns_Jb64:
				c16 = Code.Jns_Jb16;
				c32 = Code.Jns_Jb32;
				c64 = Code.Jns_Jb64;
				break;

			case Code.Jp_Jb16:
			case Code.Jp_Jb32:
			case Code.Jp_Jb64:
				c16 = Code.Jp_Jb16;
				c32 = Code.Jp_Jb32;
				c64 = Code.Jp_Jb64;
				break;

			case Code.Jnp_Jb16:
			case Code.Jnp_Jb32:
			case Code.Jnp_Jb64:
				c16 = Code.Jnp_Jb16;
				c32 = Code.Jnp_Jb32;
				c64 = Code.Jnp_Jb64;
				break;

			case Code.Jl_Jb16:
			case Code.Jl_Jb32:
			case Code.Jl_Jb64:
				c16 = Code.Jl_Jb16;
				c32 = Code.Jl_Jb32;
				c64 = Code.Jl_Jb64;
				break;

			case Code.Jge_Jb16:
			case Code.Jge_Jb32:
			case Code.Jge_Jb64:
				c16 = Code.Jge_Jb16;
				c32 = Code.Jge_Jb32;
				c64 = Code.Jge_Jb64;
				break;

			case Code.Jle_Jb16:
			case Code.Jle_Jb32:
			case Code.Jle_Jb64:
				c16 = Code.Jle_Jb16;
				c32 = Code.Jle_Jb32;
				c64 = Code.Jle_Jb64;
				break;

			case Code.Jg_Jb16:
			case Code.Jg_Jb32:
			case Code.Jg_Jb64:
				c16 = Code.Jg_Jb16;
				c32 = Code.Jg_Jb32;
				c64 = Code.Jg_Jb64;
				break;


			default:
				throw new ArgumentOutOfRangeException(nameof(code));
			}

			switch (bitness) {
			case 16: return c16;
			case 32: return c32;
			case 64: return c64;
			default: throw new ArgumentOutOfRangeException(nameof(bitness));
			}
		}

		public static Code ToShortBranchCode(this Code code) {
			switch (code) {
			case Code.Jo_Jb16:
			case Code.Jo_Jb32:
			case Code.Jo_Jb64:
			case Code.Jno_Jb16:
			case Code.Jno_Jb32:
			case Code.Jno_Jb64:
			case Code.Jb_Jb16:
			case Code.Jb_Jb32:
			case Code.Jb_Jb64:
			case Code.Jae_Jb16:
			case Code.Jae_Jb32:
			case Code.Jae_Jb64:
			case Code.Je_Jb16:
			case Code.Je_Jb32:
			case Code.Je_Jb64:
			case Code.Jne_Jb16:
			case Code.Jne_Jb32:
			case Code.Jne_Jb64:
			case Code.Jbe_Jb16:
			case Code.Jbe_Jb32:
			case Code.Jbe_Jb64:
			case Code.Ja_Jb16:
			case Code.Ja_Jb32:
			case Code.Ja_Jb64:

			case Code.Js_Jb16:
			case Code.Js_Jb32:
			case Code.Js_Jb64:
			case Code.Jns_Jb16:
			case Code.Jns_Jb32:
			case Code.Jns_Jb64:
			case Code.Jp_Jb16:
			case Code.Jp_Jb32:
			case Code.Jp_Jb64:
			case Code.Jnp_Jb16:
			case Code.Jnp_Jb32:
			case Code.Jnp_Jb64:
			case Code.Jl_Jb16:
			case Code.Jl_Jb32:
			case Code.Jl_Jb64:
			case Code.Jge_Jb16:
			case Code.Jge_Jb32:
			case Code.Jge_Jb64:
			case Code.Jle_Jb16:
			case Code.Jle_Jb32:
			case Code.Jle_Jb64:
			case Code.Jg_Jb16:
			case Code.Jg_Jb32:
			case Code.Jg_Jb64:
				return code;

			case Code.Jo_Jw16:	return Code.Jo_Jb16;
			case Code.Jo_Jd32:	return Code.Jo_Jb32;
			case Code.Jo_Jd64:	return Code.Jo_Jb64;
			case Code.Jno_Jw16:	return Code.Jno_Jb16;
			case Code.Jno_Jd32:	return Code.Jno_Jb32;
			case Code.Jno_Jd64:	return Code.Jno_Jb64;
			case Code.Jb_Jw16:	return Code.Jb_Jb16;
			case Code.Jb_Jd32:	return Code.Jb_Jb32;
			case Code.Jb_Jd64:	return Code.Jb_Jb64;
			case Code.Jae_Jw16:	return Code.Jae_Jb16;
			case Code.Jae_Jd32:	return Code.Jae_Jb32;
			case Code.Jae_Jd64:	return Code.Jae_Jb64;
			case Code.Je_Jw16:	return Code.Je_Jb16;
			case Code.Je_Jd32:	return Code.Je_Jb32;
			case Code.Je_Jd64:	return Code.Je_Jb64;
			case Code.Jne_Jw16:	return Code.Jne_Jb16;
			case Code.Jne_Jd32:	return Code.Jne_Jb32;
			case Code.Jne_Jd64:	return Code.Jne_Jb64;
			case Code.Jbe_Jw16:	return Code.Jbe_Jb16;
			case Code.Jbe_Jd32:	return Code.Jbe_Jb32;
			case Code.Jbe_Jd64:	return Code.Jbe_Jb64;
			case Code.Ja_Jw16:	return Code.Ja_Jb16;
			case Code.Ja_Jd32:	return Code.Ja_Jb32;
			case Code.Ja_Jd64:	return Code.Ja_Jb64;

			case Code.Js_Jw16:	return Code.Js_Jb16;
			case Code.Js_Jd32:	return Code.Js_Jb32;
			case Code.Js_Jd64:	return Code.Js_Jb64;
			case Code.Jns_Jw16:	return Code.Jns_Jb16;
			case Code.Jns_Jd32:	return Code.Jns_Jb32;
			case Code.Jns_Jd64:	return Code.Jns_Jb64;
			case Code.Jp_Jw16:	return Code.Jp_Jb16;
			case Code.Jp_Jd32:	return Code.Jp_Jb32;
			case Code.Jp_Jd64:	return Code.Jp_Jb64;
			case Code.Jnp_Jw16:	return Code.Jnp_Jb16;
			case Code.Jnp_Jd32:	return Code.Jnp_Jb32;
			case Code.Jnp_Jd64:	return Code.Jnp_Jb64;
			case Code.Jl_Jw16:	return Code.Jl_Jb16;
			case Code.Jl_Jd32:	return Code.Jl_Jb32;
			case Code.Jl_Jd64:	return Code.Jl_Jb64;
			case Code.Jge_Jw16:	return Code.Jge_Jb16;
			case Code.Jge_Jd32:	return Code.Jge_Jb32;
			case Code.Jge_Jd64:	return Code.Jge_Jb64;
			case Code.Jle_Jw16:	return Code.Jle_Jb16;
			case Code.Jle_Jd32:	return Code.Jle_Jb32;
			case Code.Jle_Jd64:	return Code.Jle_Jb64;
			case Code.Jg_Jw16:	return Code.Jg_Jb16;
			case Code.Jg_Jd32:	return Code.Jg_Jb32;
			case Code.Jg_Jd64:	return Code.Jg_Jb64;

			case Code.Jmp_Jw16:	return Code.Jmp_Jb16;
			case Code.Jmp_Jd32:	return Code.Jmp_Jb32;
			case Code.Jmp_Jd64:	return Code.Jmp_Jb64;

			case Code.Jmp_Jb16:
			case Code.Jmp_Jb32:
			case Code.Jmp_Jb64:
				return code;

			default:
				throw new ArgumentOutOfRangeException(nameof(code));
			}
		}

		public static Code ToNearBranchCode(this Code code) {
			switch (code) {
			case Code.Jo_Jb16:	return Code.Jo_Jw16;
			case Code.Jo_Jb32:	return Code.Jo_Jd32;
			case Code.Jo_Jb64:	return Code.Jo_Jd64;
			case Code.Jno_Jb16:	return Code.Jno_Jw16;
			case Code.Jno_Jb32:	return Code.Jno_Jd32;
			case Code.Jno_Jb64:	return Code.Jno_Jd64;
			case Code.Jb_Jb16:	return Code.Jb_Jw16;
			case Code.Jb_Jb32:	return Code.Jb_Jd32;
			case Code.Jb_Jb64:	return Code.Jb_Jd64;
			case Code.Jae_Jb16:	return Code.Jae_Jw16;
			case Code.Jae_Jb32:	return Code.Jae_Jd32;
			case Code.Jae_Jb64:	return Code.Jae_Jd64;
			case Code.Je_Jb16:	return Code.Je_Jw16;
			case Code.Je_Jb32:	return Code.Je_Jd32;
			case Code.Je_Jb64:	return Code.Je_Jd64;
			case Code.Jne_Jb16:	return Code.Jne_Jw16;
			case Code.Jne_Jb32:	return Code.Jne_Jd32;
			case Code.Jne_Jb64:	return Code.Jne_Jd64;
			case Code.Jbe_Jb16:	return Code.Jbe_Jw16;
			case Code.Jbe_Jb32:	return Code.Jbe_Jd32;
			case Code.Jbe_Jb64:	return Code.Jbe_Jd64;
			case Code.Ja_Jb16:	return Code.Ja_Jw16;
			case Code.Ja_Jb32:	return Code.Ja_Jd32;
			case Code.Ja_Jb64:	return Code.Ja_Jd64;

			case Code.Js_Jb16:	return Code.Js_Jw16;
			case Code.Js_Jb32:	return Code.Js_Jd32;
			case Code.Js_Jb64:	return Code.Js_Jd64;
			case Code.Jns_Jb16:	return Code.Jns_Jw16;
			case Code.Jns_Jb32:	return Code.Jns_Jd32;
			case Code.Jns_Jb64:	return Code.Jns_Jd64;
			case Code.Jp_Jb16:	return Code.Jp_Jw16;
			case Code.Jp_Jb32:	return Code.Jp_Jd32;
			case Code.Jp_Jb64:	return Code.Jp_Jd64;
			case Code.Jnp_Jb16:	return Code.Jnp_Jw16;
			case Code.Jnp_Jb32:	return Code.Jnp_Jd32;
			case Code.Jnp_Jb64:	return Code.Jnp_Jd64;
			case Code.Jl_Jb16:	return Code.Jl_Jw16;
			case Code.Jl_Jb32:	return Code.Jl_Jd32;
			case Code.Jl_Jb64:	return Code.Jl_Jd64;
			case Code.Jge_Jb16:	return Code.Jge_Jw16;
			case Code.Jge_Jb32:	return Code.Jge_Jd32;
			case Code.Jge_Jb64:	return Code.Jge_Jd64;
			case Code.Jle_Jb16:	return Code.Jle_Jw16;
			case Code.Jle_Jb32:	return Code.Jle_Jd32;
			case Code.Jle_Jb64:	return Code.Jle_Jd64;
			case Code.Jg_Jb16:	return Code.Jg_Jw16;
			case Code.Jg_Jb32:	return Code.Jg_Jd32;
			case Code.Jg_Jb64:	return Code.Jg_Jd64;

			case Code.Jo_Jw16:
			case Code.Jo_Jd32:
			case Code.Jo_Jd64:
			case Code.Jno_Jw16:
			case Code.Jno_Jd32:
			case Code.Jno_Jd64:
			case Code.Jb_Jw16:
			case Code.Jb_Jd32:
			case Code.Jb_Jd64:
			case Code.Jae_Jw16:
			case Code.Jae_Jd32:
			case Code.Jae_Jd64:
			case Code.Je_Jw16:
			case Code.Je_Jd32:
			case Code.Je_Jd64:
			case Code.Jne_Jw16:
			case Code.Jne_Jd32:
			case Code.Jne_Jd64:
			case Code.Jbe_Jw16:
			case Code.Jbe_Jd32:
			case Code.Jbe_Jd64:
			case Code.Ja_Jw16:
			case Code.Ja_Jd32:
			case Code.Ja_Jd64:

			case Code.Js_Jw16:
			case Code.Js_Jd32:
			case Code.Js_Jd64:
			case Code.Jns_Jw16:
			case Code.Jns_Jd32:
			case Code.Jns_Jd64:
			case Code.Jp_Jw16:
			case Code.Jp_Jd32:
			case Code.Jp_Jd64:
			case Code.Jnp_Jw16:
			case Code.Jnp_Jd32:
			case Code.Jnp_Jd64:
			case Code.Jl_Jw16:
			case Code.Jl_Jd32:
			case Code.Jl_Jd64:
			case Code.Jge_Jw16:
			case Code.Jge_Jd32:
			case Code.Jge_Jd64:
			case Code.Jle_Jw16:
			case Code.Jle_Jd32:
			case Code.Jle_Jd64:
			case Code.Jg_Jw16:
			case Code.Jg_Jd32:
			case Code.Jg_Jd64:
				return code;

			case Code.Jmp_Jw16:
			case Code.Jmp_Jd32:
			case Code.Jmp_Jd64:
				return code;

			case Code.Jmp_Jb16:	return Code.Jmp_Jw16;
			case Code.Jmp_Jb32:	return Code.Jmp_Jd32;
			case Code.Jmp_Jb64:	return Code.Jmp_Jd64;

			default:
				throw new ArgumentOutOfRangeException(nameof(code));
			}
		}

		public static Code ToNegatedJcc(this Code code) {
			switch (code) {
			case Code.Jo_Jb16:	return Code.Jno_Jb16;
			case Code.Jo_Jb32:	return Code.Jno_Jb32;
			case Code.Jo_Jb64:	return Code.Jno_Jb64;
			case Code.Jno_Jb16:	return Code.Jo_Jb16;
			case Code.Jno_Jb32:	return Code.Jo_Jb32;
			case Code.Jno_Jb64:	return Code.Jo_Jb64;
			case Code.Jb_Jb16:	return Code.Jae_Jb16;
			case Code.Jb_Jb32:	return Code.Jae_Jb32;
			case Code.Jb_Jb64:	return Code.Jae_Jb64;
			case Code.Jae_Jb16:	return Code.Jb_Jb16;
			case Code.Jae_Jb32:	return Code.Jb_Jb32;
			case Code.Jae_Jb64:	return Code.Jb_Jb64;
			case Code.Je_Jb16:	return Code.Jne_Jb16;
			case Code.Je_Jb32:	return Code.Jne_Jb32;
			case Code.Je_Jb64:	return Code.Jne_Jb64;
			case Code.Jne_Jb16:	return Code.Je_Jb16;
			case Code.Jne_Jb32:	return Code.Je_Jb32;
			case Code.Jne_Jb64:	return Code.Je_Jb64;
			case Code.Jbe_Jb16:	return Code.Ja_Jb16;
			case Code.Jbe_Jb32:	return Code.Ja_Jb32;
			case Code.Jbe_Jb64:	return Code.Ja_Jb64;
			case Code.Ja_Jb16:	return Code.Jbe_Jb16;
			case Code.Ja_Jb32:	return Code.Jbe_Jb32;
			case Code.Ja_Jb64:	return Code.Jbe_Jb64;

			case Code.Js_Jb16:	return Code.Jns_Jb16;
			case Code.Js_Jb32:	return Code.Jns_Jb32;
			case Code.Js_Jb64:	return Code.Jns_Jb64;
			case Code.Jns_Jb16:	return Code.Js_Jb16;
			case Code.Jns_Jb32:	return Code.Js_Jb32;
			case Code.Jns_Jb64:	return Code.Js_Jb64;
			case Code.Jp_Jb16:	return Code.Jnp_Jb16;
			case Code.Jp_Jb32:	return Code.Jnp_Jb32;
			case Code.Jp_Jb64:	return Code.Jnp_Jb64;
			case Code.Jnp_Jb16:	return Code.Jp_Jb16;
			case Code.Jnp_Jb32:	return Code.Jp_Jb32;
			case Code.Jnp_Jb64:	return Code.Jp_Jb64;
			case Code.Jl_Jb16:	return Code.Jge_Jb16;
			case Code.Jl_Jb32:	return Code.Jge_Jb32;
			case Code.Jl_Jb64:	return Code.Jge_Jb64;
			case Code.Jge_Jb16:	return Code.Jl_Jb16;
			case Code.Jge_Jb32:	return Code.Jl_Jb32;
			case Code.Jge_Jb64:	return Code.Jl_Jb64;
			case Code.Jle_Jb16:	return Code.Jg_Jb16;
			case Code.Jle_Jb32:	return Code.Jg_Jb32;
			case Code.Jle_Jb64:	return Code.Jg_Jb64;
			case Code.Jg_Jb16:	return Code.Jle_Jb16;
			case Code.Jg_Jb32:	return Code.Jle_Jb32;
			case Code.Jg_Jb64:	return Code.Jle_Jb64;

			case Code.Jo_Jw16:	return Code.Jno_Jw16;
			case Code.Jo_Jd32:	return Code.Jno_Jd32;
			case Code.Jo_Jd64:	return Code.Jno_Jd64;
			case Code.Jno_Jw16:	return Code.Jo_Jw16;
			case Code.Jno_Jd32:	return Code.Jo_Jd32;
			case Code.Jno_Jd64:	return Code.Jo_Jd64;
			case Code.Jb_Jw16:	return Code.Jae_Jw16;
			case Code.Jb_Jd32:	return Code.Jae_Jd32;
			case Code.Jb_Jd64:	return Code.Jae_Jd64;
			case Code.Jae_Jw16:	return Code.Jb_Jw16;
			case Code.Jae_Jd32:	return Code.Jb_Jd32;
			case Code.Jae_Jd64:	return Code.Jb_Jd64;
			case Code.Je_Jw16:	return Code.Jne_Jw16;
			case Code.Je_Jd32:	return Code.Jne_Jd32;
			case Code.Je_Jd64:	return Code.Jne_Jd64;
			case Code.Jne_Jw16:	return Code.Je_Jw16;
			case Code.Jne_Jd32:	return Code.Je_Jd32;
			case Code.Jne_Jd64:	return Code.Je_Jd64;
			case Code.Jbe_Jw16:	return Code.Ja_Jw16;
			case Code.Jbe_Jd32:	return Code.Ja_Jd32;
			case Code.Jbe_Jd64:	return Code.Ja_Jd64;
			case Code.Ja_Jw16:	return Code.Jbe_Jw16;
			case Code.Ja_Jd32:	return Code.Jbe_Jd32;
			case Code.Ja_Jd64:	return Code.Jbe_Jd64;

			case Code.Js_Jw16:	return Code.Jns_Jw16;
			case Code.Js_Jd32:	return Code.Jns_Jd32;
			case Code.Js_Jd64:	return Code.Jns_Jd64;
			case Code.Jns_Jw16:	return Code.Js_Jw16;
			case Code.Jns_Jd32:	return Code.Js_Jd32;
			case Code.Jns_Jd64:	return Code.Js_Jd64;
			case Code.Jp_Jw16:	return Code.Jnp_Jw16;
			case Code.Jp_Jd32:	return Code.Jnp_Jd32;
			case Code.Jp_Jd64:	return Code.Jnp_Jd64;
			case Code.Jnp_Jw16:	return Code.Jp_Jw16;
			case Code.Jnp_Jd32:	return Code.Jp_Jd32;
			case Code.Jnp_Jd64:	return Code.Jp_Jd64;
			case Code.Jl_Jw16:	return Code.Jge_Jw16;
			case Code.Jl_Jd32:	return Code.Jge_Jd32;
			case Code.Jl_Jd64:	return Code.Jge_Jd64;
			case Code.Jge_Jw16:	return Code.Jl_Jw16;
			case Code.Jge_Jd32:	return Code.Jl_Jd32;
			case Code.Jge_Jd64:	return Code.Jl_Jd64;
			case Code.Jle_Jw16:	return Code.Jg_Jw16;
			case Code.Jle_Jd32:	return Code.Jg_Jd32;
			case Code.Jle_Jd64:	return Code.Jg_Jd64;
			case Code.Jg_Jw16:	return Code.Jle_Jw16;
			case Code.Jg_Jd32:	return Code.Jle_Jd32;
			case Code.Jg_Jd64:	return Code.Jle_Jd64;

			default:
				throw new ArgumentOutOfRangeException(nameof(code));
			}
		}
	}
}
#endif
