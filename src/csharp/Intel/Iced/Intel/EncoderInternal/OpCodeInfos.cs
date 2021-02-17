// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if ENCODER && OPCODE_INFO
using System.Text;

namespace Iced.Intel.EncoderInternal {
	static class OpCodeInfos {
		public static readonly OpCodeInfo[] Infos = CreateInfos();

		static OpCodeInfo[] CreateInfos() {
			var infos = new OpCodeInfo[IcedConstants.CodeEnumCount];
			var encFlags1 = EncoderData.EncFlags1;
			var encFlags2 = EncoderData.EncFlags2;
			var encFlags3 = EncoderData.EncFlags3;
			var opcFlags1 = OpCodeInfoData.OpcFlags1;
			var opcFlags2 = OpCodeInfoData.OpcFlags2;
			var sb = new StringBuilder();
			for (int i = 0; i < infos.Length; i++)
				infos[i] = new OpCodeInfo((Code)i, (EncFlags1)encFlags1[i], (EncFlags2)encFlags2[i], (EncFlags3)encFlags3[i], (OpCodeInfoFlags1)opcFlags1[i], (OpCodeInfoFlags2)opcFlags2[i], sb);
			return infos;
		}
	}
}
#endif
