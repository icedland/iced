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
			case Code.Jo_rel8_16:
			case Code.Jo_rel8_32:
			case Code.Jo_rel8_64:
				c16 = Code.Jo_rel8_16;
				c32 = Code.Jo_rel8_32;
				c64 = Code.Jo_rel8_64;
				break;

			case Code.Jno_rel8_16:
			case Code.Jno_rel8_32:
			case Code.Jno_rel8_64:
				c16 = Code.Jno_rel8_16;
				c32 = Code.Jno_rel8_32;
				c64 = Code.Jno_rel8_64;
				break;

			case Code.Jb_rel8_16:
			case Code.Jb_rel8_32:
			case Code.Jb_rel8_64:
				c16 = Code.Jb_rel8_16;
				c32 = Code.Jb_rel8_32;
				c64 = Code.Jb_rel8_64;
				break;

			case Code.Jae_rel8_16:
			case Code.Jae_rel8_32:
			case Code.Jae_rel8_64:
				c16 = Code.Jae_rel8_16;
				c32 = Code.Jae_rel8_32;
				c64 = Code.Jae_rel8_64;
				break;

			case Code.Je_rel8_16:
			case Code.Je_rel8_32:
			case Code.Je_rel8_64:
				c16 = Code.Je_rel8_16;
				c32 = Code.Je_rel8_32;
				c64 = Code.Je_rel8_64;
				break;

			case Code.Jne_rel8_16:
			case Code.Jne_rel8_32:
			case Code.Jne_rel8_64:
				c16 = Code.Jne_rel8_16;
				c32 = Code.Jne_rel8_32;
				c64 = Code.Jne_rel8_64;
				break;

			case Code.Jbe_rel8_16:
			case Code.Jbe_rel8_32:
			case Code.Jbe_rel8_64:
				c16 = Code.Jbe_rel8_16;
				c32 = Code.Jbe_rel8_32;
				c64 = Code.Jbe_rel8_64;
				break;

			case Code.Ja_rel8_16:
			case Code.Ja_rel8_32:
			case Code.Ja_rel8_64:
				c16 = Code.Ja_rel8_16;
				c32 = Code.Ja_rel8_32;
				c64 = Code.Ja_rel8_64;
				break;

			case Code.Js_rel8_16:
			case Code.Js_rel8_32:
			case Code.Js_rel8_64:
				c16 = Code.Js_rel8_16;
				c32 = Code.Js_rel8_32;
				c64 = Code.Js_rel8_64;
				break;

			case Code.Jns_rel8_16:
			case Code.Jns_rel8_32:
			case Code.Jns_rel8_64:
				c16 = Code.Jns_rel8_16;
				c32 = Code.Jns_rel8_32;
				c64 = Code.Jns_rel8_64;
				break;

			case Code.Jp_rel8_16:
			case Code.Jp_rel8_32:
			case Code.Jp_rel8_64:
				c16 = Code.Jp_rel8_16;
				c32 = Code.Jp_rel8_32;
				c64 = Code.Jp_rel8_64;
				break;

			case Code.Jnp_rel8_16:
			case Code.Jnp_rel8_32:
			case Code.Jnp_rel8_64:
				c16 = Code.Jnp_rel8_16;
				c32 = Code.Jnp_rel8_32;
				c64 = Code.Jnp_rel8_64;
				break;

			case Code.Jl_rel8_16:
			case Code.Jl_rel8_32:
			case Code.Jl_rel8_64:
				c16 = Code.Jl_rel8_16;
				c32 = Code.Jl_rel8_32;
				c64 = Code.Jl_rel8_64;
				break;

			case Code.Jge_rel8_16:
			case Code.Jge_rel8_32:
			case Code.Jge_rel8_64:
				c16 = Code.Jge_rel8_16;
				c32 = Code.Jge_rel8_32;
				c64 = Code.Jge_rel8_64;
				break;

			case Code.Jle_rel8_16:
			case Code.Jle_rel8_32:
			case Code.Jle_rel8_64:
				c16 = Code.Jle_rel8_16;
				c32 = Code.Jle_rel8_32;
				c64 = Code.Jle_rel8_64;
				break;

			case Code.Jg_rel8_16:
			case Code.Jg_rel8_32:
			case Code.Jg_rel8_64:
				c16 = Code.Jg_rel8_16;
				c32 = Code.Jg_rel8_32;
				c64 = Code.Jg_rel8_64;
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
			case Code.Jo_rel8_16:
			case Code.Jo_rel8_32:
			case Code.Jo_rel8_64:
			case Code.Jno_rel8_16:
			case Code.Jno_rel8_32:
			case Code.Jno_rel8_64:
			case Code.Jb_rel8_16:
			case Code.Jb_rel8_32:
			case Code.Jb_rel8_64:
			case Code.Jae_rel8_16:
			case Code.Jae_rel8_32:
			case Code.Jae_rel8_64:
			case Code.Je_rel8_16:
			case Code.Je_rel8_32:
			case Code.Je_rel8_64:
			case Code.Jne_rel8_16:
			case Code.Jne_rel8_32:
			case Code.Jne_rel8_64:
			case Code.Jbe_rel8_16:
			case Code.Jbe_rel8_32:
			case Code.Jbe_rel8_64:
			case Code.Ja_rel8_16:
			case Code.Ja_rel8_32:
			case Code.Ja_rel8_64:

			case Code.Js_rel8_16:
			case Code.Js_rel8_32:
			case Code.Js_rel8_64:
			case Code.Jns_rel8_16:
			case Code.Jns_rel8_32:
			case Code.Jns_rel8_64:
			case Code.Jp_rel8_16:
			case Code.Jp_rel8_32:
			case Code.Jp_rel8_64:
			case Code.Jnp_rel8_16:
			case Code.Jnp_rel8_32:
			case Code.Jnp_rel8_64:
			case Code.Jl_rel8_16:
			case Code.Jl_rel8_32:
			case Code.Jl_rel8_64:
			case Code.Jge_rel8_16:
			case Code.Jge_rel8_32:
			case Code.Jge_rel8_64:
			case Code.Jle_rel8_16:
			case Code.Jle_rel8_32:
			case Code.Jle_rel8_64:
			case Code.Jg_rel8_16:
			case Code.Jg_rel8_32:
			case Code.Jg_rel8_64:
				return code;

			case Code.Jo_rel16:		return Code.Jo_rel8_16;
			case Code.Jo_rel32_32:	return Code.Jo_rel8_32;
			case Code.Jo_rel32_64:	return Code.Jo_rel8_64;
			case Code.Jno_rel16:	return Code.Jno_rel8_16;
			case Code.Jno_rel32_32:	return Code.Jno_rel8_32;
			case Code.Jno_rel32_64:	return Code.Jno_rel8_64;
			case Code.Jb_rel16:		return Code.Jb_rel8_16;
			case Code.Jb_rel32_32:	return Code.Jb_rel8_32;
			case Code.Jb_rel32_64:	return Code.Jb_rel8_64;
			case Code.Jae_rel16:	return Code.Jae_rel8_16;
			case Code.Jae_rel32_32:	return Code.Jae_rel8_32;
			case Code.Jae_rel32_64:	return Code.Jae_rel8_64;
			case Code.Je_rel16:		return Code.Je_rel8_16;
			case Code.Je_rel32_32:	return Code.Je_rel8_32;
			case Code.Je_rel32_64:	return Code.Je_rel8_64;
			case Code.Jne_rel16:	return Code.Jne_rel8_16;
			case Code.Jne_rel32_32:	return Code.Jne_rel8_32;
			case Code.Jne_rel32_64:	return Code.Jne_rel8_64;
			case Code.Jbe_rel16:	return Code.Jbe_rel8_16;
			case Code.Jbe_rel32_32:	return Code.Jbe_rel8_32;
			case Code.Jbe_rel32_64:	return Code.Jbe_rel8_64;
			case Code.Ja_rel16:		return Code.Ja_rel8_16;
			case Code.Ja_rel32_32:	return Code.Ja_rel8_32;
			case Code.Ja_rel32_64:	return Code.Ja_rel8_64;

			case Code.Js_rel16:		return Code.Js_rel8_16;
			case Code.Js_rel32_32:	return Code.Js_rel8_32;
			case Code.Js_rel32_64:	return Code.Js_rel8_64;
			case Code.Jns_rel16:	return Code.Jns_rel8_16;
			case Code.Jns_rel32_32:	return Code.Jns_rel8_32;
			case Code.Jns_rel32_64:	return Code.Jns_rel8_64;
			case Code.Jp_rel16:		return Code.Jp_rel8_16;
			case Code.Jp_rel32_32:	return Code.Jp_rel8_32;
			case Code.Jp_rel32_64:	return Code.Jp_rel8_64;
			case Code.Jnp_rel16:	return Code.Jnp_rel8_16;
			case Code.Jnp_rel32_32:	return Code.Jnp_rel8_32;
			case Code.Jnp_rel32_64:	return Code.Jnp_rel8_64;
			case Code.Jl_rel16:		return Code.Jl_rel8_16;
			case Code.Jl_rel32_32:	return Code.Jl_rel8_32;
			case Code.Jl_rel32_64:	return Code.Jl_rel8_64;
			case Code.Jge_rel16:	return Code.Jge_rel8_16;
			case Code.Jge_rel32_32:	return Code.Jge_rel8_32;
			case Code.Jge_rel32_64:	return Code.Jge_rel8_64;
			case Code.Jle_rel16:	return Code.Jle_rel8_16;
			case Code.Jle_rel32_32:	return Code.Jle_rel8_32;
			case Code.Jle_rel32_64:	return Code.Jle_rel8_64;
			case Code.Jg_rel16:		return Code.Jg_rel8_16;
			case Code.Jg_rel32_32:	return Code.Jg_rel8_32;
			case Code.Jg_rel32_64:	return Code.Jg_rel8_64;

			case Code.Jmp_rel16:	return Code.Jmp_rel8_16;
			case Code.Jmp_rel32_32:	return Code.Jmp_rel8_32;
			case Code.Jmp_rel32_64:	return Code.Jmp_rel8_64;

			case Code.Jmp_rel8_16:
			case Code.Jmp_rel8_32:
			case Code.Jmp_rel8_64:
				return code;

			default:
				throw new ArgumentOutOfRangeException(nameof(code));
			}
		}

		public static Code ToNearBranchCode(this Code code) {
			switch (code) {
			case Code.Jo_rel8_16:	return Code.Jo_rel16;
			case Code.Jo_rel8_32:	return Code.Jo_rel32_32;
			case Code.Jo_rel8_64:	return Code.Jo_rel32_64;
			case Code.Jno_rel8_16:	return Code.Jno_rel16;
			case Code.Jno_rel8_32:	return Code.Jno_rel32_32;
			case Code.Jno_rel8_64:	return Code.Jno_rel32_64;
			case Code.Jb_rel8_16:	return Code.Jb_rel16;
			case Code.Jb_rel8_32:	return Code.Jb_rel32_32;
			case Code.Jb_rel8_64:	return Code.Jb_rel32_64;
			case Code.Jae_rel8_16:	return Code.Jae_rel16;
			case Code.Jae_rel8_32:	return Code.Jae_rel32_32;
			case Code.Jae_rel8_64:	return Code.Jae_rel32_64;
			case Code.Je_rel8_16:	return Code.Je_rel16;
			case Code.Je_rel8_32:	return Code.Je_rel32_32;
			case Code.Je_rel8_64:	return Code.Je_rel32_64;
			case Code.Jne_rel8_16:	return Code.Jne_rel16;
			case Code.Jne_rel8_32:	return Code.Jne_rel32_32;
			case Code.Jne_rel8_64:	return Code.Jne_rel32_64;
			case Code.Jbe_rel8_16:	return Code.Jbe_rel16;
			case Code.Jbe_rel8_32:	return Code.Jbe_rel32_32;
			case Code.Jbe_rel8_64:	return Code.Jbe_rel32_64;
			case Code.Ja_rel8_16:	return Code.Ja_rel16;
			case Code.Ja_rel8_32:	return Code.Ja_rel32_32;
			case Code.Ja_rel8_64:	return Code.Ja_rel32_64;

			case Code.Js_rel8_16:	return Code.Js_rel16;
			case Code.Js_rel8_32:	return Code.Js_rel32_32;
			case Code.Js_rel8_64:	return Code.Js_rel32_64;
			case Code.Jns_rel8_16:	return Code.Jns_rel16;
			case Code.Jns_rel8_32:	return Code.Jns_rel32_32;
			case Code.Jns_rel8_64:	return Code.Jns_rel32_64;
			case Code.Jp_rel8_16:	return Code.Jp_rel16;
			case Code.Jp_rel8_32:	return Code.Jp_rel32_32;
			case Code.Jp_rel8_64:	return Code.Jp_rel32_64;
			case Code.Jnp_rel8_16:	return Code.Jnp_rel16;
			case Code.Jnp_rel8_32:	return Code.Jnp_rel32_32;
			case Code.Jnp_rel8_64:	return Code.Jnp_rel32_64;
			case Code.Jl_rel8_16:	return Code.Jl_rel16;
			case Code.Jl_rel8_32:	return Code.Jl_rel32_32;
			case Code.Jl_rel8_64:	return Code.Jl_rel32_64;
			case Code.Jge_rel8_16:	return Code.Jge_rel16;
			case Code.Jge_rel8_32:	return Code.Jge_rel32_32;
			case Code.Jge_rel8_64:	return Code.Jge_rel32_64;
			case Code.Jle_rel8_16:	return Code.Jle_rel16;
			case Code.Jle_rel8_32:	return Code.Jle_rel32_32;
			case Code.Jle_rel8_64:	return Code.Jle_rel32_64;
			case Code.Jg_rel8_16:	return Code.Jg_rel16;
			case Code.Jg_rel8_32:	return Code.Jg_rel32_32;
			case Code.Jg_rel8_64:	return Code.Jg_rel32_64;

			case Code.Jo_rel16:
			case Code.Jo_rel32_32:
			case Code.Jo_rel32_64:
			case Code.Jno_rel16:
			case Code.Jno_rel32_32:
			case Code.Jno_rel32_64:
			case Code.Jb_rel16:
			case Code.Jb_rel32_32:
			case Code.Jb_rel32_64:
			case Code.Jae_rel16:
			case Code.Jae_rel32_32:
			case Code.Jae_rel32_64:
			case Code.Je_rel16:
			case Code.Je_rel32_32:
			case Code.Je_rel32_64:
			case Code.Jne_rel16:
			case Code.Jne_rel32_32:
			case Code.Jne_rel32_64:
			case Code.Jbe_rel16:
			case Code.Jbe_rel32_32:
			case Code.Jbe_rel32_64:
			case Code.Ja_rel16:
			case Code.Ja_rel32_32:
			case Code.Ja_rel32_64:

			case Code.Js_rel16:
			case Code.Js_rel32_32:
			case Code.Js_rel32_64:
			case Code.Jns_rel16:
			case Code.Jns_rel32_32:
			case Code.Jns_rel32_64:
			case Code.Jp_rel16:
			case Code.Jp_rel32_32:
			case Code.Jp_rel32_64:
			case Code.Jnp_rel16:
			case Code.Jnp_rel32_32:
			case Code.Jnp_rel32_64:
			case Code.Jl_rel16:
			case Code.Jl_rel32_32:
			case Code.Jl_rel32_64:
			case Code.Jge_rel16:
			case Code.Jge_rel32_32:
			case Code.Jge_rel32_64:
			case Code.Jle_rel16:
			case Code.Jle_rel32_32:
			case Code.Jle_rel32_64:
			case Code.Jg_rel16:
			case Code.Jg_rel32_32:
			case Code.Jg_rel32_64:
				return code;

			case Code.Jmp_rel16:
			case Code.Jmp_rel32_32:
			case Code.Jmp_rel32_64:
				return code;

			case Code.Jmp_rel8_16:	return Code.Jmp_rel16;
			case Code.Jmp_rel8_32:	return Code.Jmp_rel32_32;
			case Code.Jmp_rel8_64:	return Code.Jmp_rel32_64;

			default:
				throw new ArgumentOutOfRangeException(nameof(code));
			}
		}

		public static Code ToNegatedJcc(this Code code) {
			switch (code) {
			case Code.Jo_rel8_16:	return Code.Jno_rel8_16;
			case Code.Jo_rel8_32:	return Code.Jno_rel8_32;
			case Code.Jo_rel8_64:	return Code.Jno_rel8_64;
			case Code.Jno_rel8_16:	return Code.Jo_rel8_16;
			case Code.Jno_rel8_32:	return Code.Jo_rel8_32;
			case Code.Jno_rel8_64:	return Code.Jo_rel8_64;
			case Code.Jb_rel8_16:	return Code.Jae_rel8_16;
			case Code.Jb_rel8_32:	return Code.Jae_rel8_32;
			case Code.Jb_rel8_64:	return Code.Jae_rel8_64;
			case Code.Jae_rel8_16:	return Code.Jb_rel8_16;
			case Code.Jae_rel8_32:	return Code.Jb_rel8_32;
			case Code.Jae_rel8_64:	return Code.Jb_rel8_64;
			case Code.Je_rel8_16:	return Code.Jne_rel8_16;
			case Code.Je_rel8_32:	return Code.Jne_rel8_32;
			case Code.Je_rel8_64:	return Code.Jne_rel8_64;
			case Code.Jne_rel8_16:	return Code.Je_rel8_16;
			case Code.Jne_rel8_32:	return Code.Je_rel8_32;
			case Code.Jne_rel8_64:	return Code.Je_rel8_64;
			case Code.Jbe_rel8_16:	return Code.Ja_rel8_16;
			case Code.Jbe_rel8_32:	return Code.Ja_rel8_32;
			case Code.Jbe_rel8_64:	return Code.Ja_rel8_64;
			case Code.Ja_rel8_16:	return Code.Jbe_rel8_16;
			case Code.Ja_rel8_32:	return Code.Jbe_rel8_32;
			case Code.Ja_rel8_64:	return Code.Jbe_rel8_64;

			case Code.Js_rel8_16:	return Code.Jns_rel8_16;
			case Code.Js_rel8_32:	return Code.Jns_rel8_32;
			case Code.Js_rel8_64:	return Code.Jns_rel8_64;
			case Code.Jns_rel8_16:	return Code.Js_rel8_16;
			case Code.Jns_rel8_32:	return Code.Js_rel8_32;
			case Code.Jns_rel8_64:	return Code.Js_rel8_64;
			case Code.Jp_rel8_16:	return Code.Jnp_rel8_16;
			case Code.Jp_rel8_32:	return Code.Jnp_rel8_32;
			case Code.Jp_rel8_64:	return Code.Jnp_rel8_64;
			case Code.Jnp_rel8_16:	return Code.Jp_rel8_16;
			case Code.Jnp_rel8_32:	return Code.Jp_rel8_32;
			case Code.Jnp_rel8_64:	return Code.Jp_rel8_64;
			case Code.Jl_rel8_16:	return Code.Jge_rel8_16;
			case Code.Jl_rel8_32:	return Code.Jge_rel8_32;
			case Code.Jl_rel8_64:	return Code.Jge_rel8_64;
			case Code.Jge_rel8_16:	return Code.Jl_rel8_16;
			case Code.Jge_rel8_32:	return Code.Jl_rel8_32;
			case Code.Jge_rel8_64:	return Code.Jl_rel8_64;
			case Code.Jle_rel8_16:	return Code.Jg_rel8_16;
			case Code.Jle_rel8_32:	return Code.Jg_rel8_32;
			case Code.Jle_rel8_64:	return Code.Jg_rel8_64;
			case Code.Jg_rel8_16:	return Code.Jle_rel8_16;
			case Code.Jg_rel8_32:	return Code.Jle_rel8_32;
			case Code.Jg_rel8_64:	return Code.Jle_rel8_64;

			case Code.Jo_rel16:		return Code.Jno_rel16;
			case Code.Jo_rel32_32:	return Code.Jno_rel32_32;
			case Code.Jo_rel32_64:	return Code.Jno_rel32_64;
			case Code.Jno_rel16:	return Code.Jo_rel16;
			case Code.Jno_rel32_32:	return Code.Jo_rel32_32;
			case Code.Jno_rel32_64:	return Code.Jo_rel32_64;
			case Code.Jb_rel16:		return Code.Jae_rel16;
			case Code.Jb_rel32_32:	return Code.Jae_rel32_32;
			case Code.Jb_rel32_64:	return Code.Jae_rel32_64;
			case Code.Jae_rel16:	return Code.Jb_rel16;
			case Code.Jae_rel32_32:	return Code.Jb_rel32_32;
			case Code.Jae_rel32_64:	return Code.Jb_rel32_64;
			case Code.Je_rel16:		return Code.Jne_rel16;
			case Code.Je_rel32_32:	return Code.Jne_rel32_32;
			case Code.Je_rel32_64:	return Code.Jne_rel32_64;
			case Code.Jne_rel16:	return Code.Je_rel16;
			case Code.Jne_rel32_32:	return Code.Je_rel32_32;
			case Code.Jne_rel32_64:	return Code.Je_rel32_64;
			case Code.Jbe_rel16:	return Code.Ja_rel16;
			case Code.Jbe_rel32_32:	return Code.Ja_rel32_32;
			case Code.Jbe_rel32_64:	return Code.Ja_rel32_64;
			case Code.Ja_rel16:		return Code.Jbe_rel16;
			case Code.Ja_rel32_32:	return Code.Jbe_rel32_32;
			case Code.Ja_rel32_64:	return Code.Jbe_rel32_64;

			case Code.Js_rel16:		return Code.Jns_rel16;
			case Code.Js_rel32_32:	return Code.Jns_rel32_32;
			case Code.Js_rel32_64:	return Code.Jns_rel32_64;
			case Code.Jns_rel16:	return Code.Js_rel16;
			case Code.Jns_rel32_32:	return Code.Js_rel32_32;
			case Code.Jns_rel32_64:	return Code.Js_rel32_64;
			case Code.Jp_rel16:		return Code.Jnp_rel16;
			case Code.Jp_rel32_32:	return Code.Jnp_rel32_32;
			case Code.Jp_rel32_64:	return Code.Jnp_rel32_64;
			case Code.Jnp_rel16:	return Code.Jp_rel16;
			case Code.Jnp_rel32_32:	return Code.Jp_rel32_32;
			case Code.Jnp_rel32_64:	return Code.Jp_rel32_64;
			case Code.Jl_rel16:		return Code.Jge_rel16;
			case Code.Jl_rel32_32:	return Code.Jge_rel32_32;
			case Code.Jl_rel32_64:	return Code.Jge_rel32_64;
			case Code.Jge_rel16:	return Code.Jl_rel16;
			case Code.Jge_rel32_32:	return Code.Jl_rel32_32;
			case Code.Jge_rel32_64:	return Code.Jl_rel32_64;
			case Code.Jle_rel16:	return Code.Jg_rel16;
			case Code.Jle_rel32_32:	return Code.Jg_rel32_32;
			case Code.Jle_rel32_64:	return Code.Jg_rel32_64;
			case Code.Jg_rel16:		return Code.Jle_rel16;
			case Code.Jg_rel32_32:	return Code.Jle_rel32_32;
			case Code.Jg_rel32_64:	return Code.Jle_rel32_64;

			default:
				throw new ArgumentOutOfRangeException(nameof(code));
			}
		}
	}
}
#endif
