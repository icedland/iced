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

#if (!NO_DECODER32 || !NO_DECODER64) && !NO_DECODER
using System.Diagnostics;

namespace Iced.Intel.DecoderInternal {
	static class OpCodeHandlers_D3NOW {
		internal static readonly Code[] CodeValues = CreateCodeValues();

		static Code[] CreateCodeValues() {
			var result = new Code[0x100];
			Debug.Assert(Code.INVALID == 0);
			result[0x0C] = Code.D3NOW_Pi2fw_mm_mmm64;
			result[0x0D] = Code.D3NOW_Pi2fd_mm_mmm64;
			result[0x1C] = Code.D3NOW_Pf2iw_mm_mmm64;
			result[0x1D] = Code.D3NOW_Pf2id_mm_mmm64;
			result[0x86] = Code.D3NOW_Pfrcpv_mm_mmm64;
			result[0x87] = Code.D3NOW_Pfrsqrtv_mm_mmm64;
			result[0x8A] = Code.D3NOW_Pfnacc_mm_mmm64;
			result[0x8E] = Code.D3NOW_Pfpnacc_mm_mmm64;
			result[0x90] = Code.D3NOW_Pfcmpge_mm_mmm64;
			result[0x94] = Code.D3NOW_Pfmin_mm_mmm64;
			result[0x96] = Code.D3NOW_Pfrcp_mm_mmm64;
			result[0x97] = Code.D3NOW_Pfrsqrt_mm_mmm64;
			result[0x9A] = Code.D3NOW_Pfsub_mm_mmm64;
			result[0x9E] = Code.D3NOW_Pfadd_mm_mmm64;
			result[0xA0] = Code.D3NOW_Pfcmpgt_mm_mmm64;
			result[0xA4] = Code.D3NOW_Pfmax_mm_mmm64;
			result[0xA6] = Code.D3NOW_Pfrcpit1_mm_mmm64;
			result[0xA7] = Code.D3NOW_Pfrsqit1_mm_mmm64;
			result[0xAA] = Code.D3NOW_Pfsubr_mm_mmm64;
			result[0xAE] = Code.D3NOW_Pfacc_mm_mmm64;
			result[0xB0] = Code.D3NOW_Pfcmpeq_mm_mmm64;
			result[0xB4] = Code.D3NOW_Pfmul_mm_mmm64;
			result[0xB6] = Code.D3NOW_Pfrcpit2_mm_mmm64;
			result[0xB7] = Code.D3NOW_Pmulhrw_mm_mmm64;
			result[0xBB] = Code.D3NOW_Pswapd_mm_mmm64;
			result[0xBF] = Code.D3NOW_Pavgusb_mm_mmm64;
			return result;
		}
	}
}
#endif
