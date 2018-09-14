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
		internal static readonly MemorySize[] MemorySizes = CreateMemorySizes();

		static Code[] CreateCodeValues() {
			var result = new Code[0x100];
			Debug.Assert(Code.INVALID == 0);
			result[0x0C] = Code.D3NOW_Pi2fw_P_Q;
			result[0x0D] = Code.D3NOW_Pi2fd_P_Q;
			result[0x1C] = Code.D3NOW_Pf2iw_P_Q;
			result[0x1D] = Code.D3NOW_Pf2id_P_Q;
			result[0x86] = Code.D3NOW_Pfrcpv_P_Q;
			result[0x87] = Code.D3NOW_Pfrsqrtv_P_Q;
			result[0x8A] = Code.D3NOW_Pfnacc_P_Q;
			result[0x8E] = Code.D3NOW_Pfpnacc_P_Q;
			result[0x90] = Code.D3NOW_Pfcmpge_P_Q;
			result[0x94] = Code.D3NOW_Pfmin_P_Q;
			result[0x96] = Code.D3NOW_Pfrcp_P_Q;
			result[0x97] = Code.D3NOW_Pfrsqrt_P_Q;
			result[0x9A] = Code.D3NOW_Pfsub_P_Q;
			result[0x9E] = Code.D3NOW_Pfadd_P_Q;
			result[0xA0] = Code.D3NOW_Pfcmpgt_P_Q;
			result[0xA4] = Code.D3NOW_Pfmax_P_Q;
			result[0xA6] = Code.D3NOW_Pfrcpit1_P_Q;
			result[0xA7] = Code.D3NOW_Pfrsqit1_P_Q;
			result[0xAA] = Code.D3NOW_Pfsubr_P_Q;
			result[0xAE] = Code.D3NOW_Pfacc_P_Q;
			result[0xB0] = Code.D3NOW_Pfcmpeq_P_Q;
			result[0xB4] = Code.D3NOW_Pfmul_P_Q;
			result[0xB6] = Code.D3NOW_Pfrcpit2_P_Q;
			result[0xB7] = Code.D3NOW_Pmulhrw_P_Q;
			result[0xBB] = Code.D3NOW_Pswapd_P_Q;
			result[0xBF] = Code.D3NOW_Pavgusb_P_Q;
			return result;
		}

		static MemorySize[] CreateMemorySizes() {
			var result = new MemorySize[0x100];
			result[0x0C] = MemorySize.Packed64_Int16;
			result[0x0D] = MemorySize.Packed64_Int32;
			result[0x1C] = MemorySize.Packed64_Float32;
			result[0x1D] = MemorySize.Packed64_Float32;
			result[0x86] = MemorySize.Packed64_Float32;
			result[0x87] = MemorySize.Packed64_Float32;
			result[0x8A] = MemorySize.Packed64_Float32;
			result[0x8E] = MemorySize.Packed64_Float32;
			result[0x90] = MemorySize.Packed64_Float32;
			result[0x94] = MemorySize.Packed64_Float32;
			result[0x96] = MemorySize.Packed64_Float32;
			result[0x97] = MemorySize.Packed64_Float32;
			result[0x9A] = MemorySize.Packed64_Float32;
			result[0x9E] = MemorySize.Packed64_Float32;
			result[0xA0] = MemorySize.Packed64_Float32;
			result[0xA4] = MemorySize.Packed64_Float32;
			result[0xA6] = MemorySize.Packed64_Float32;
			result[0xA7] = MemorySize.Packed64_Float32;
			result[0xAA] = MemorySize.Packed64_Float32;
			result[0xAE] = MemorySize.Packed64_Float32;
			result[0xB0] = MemorySize.Packed64_Float32;
			result[0xB4] = MemorySize.Packed64_Float32;
			result[0xB6] = MemorySize.Packed64_Float32;
			result[0xB7] = MemorySize.Packed64_Int16;
			result[0xBB] = MemorySize.Packed64_UInt32;
			result[0xBF] = MemorySize.Packed64_UInt8;
			return result;
		}
	}
}
#endif
