// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if DECODER && !NO_XOP
namespace Iced.Intel.DecoderInternal {
	static partial class OpCodeHandlersTables_XOP {
		internal static readonly OpCodeHandler[] XOP8;
		internal static readonly OpCodeHandler[] XOP9;
		internal static readonly OpCodeHandler[] XOPA;

		static OpCodeHandlersTables_XOP() {
			var handlerReader = new VexOpCodeHandlerReader();
			var deserializer = new TableDeserializer(handlerReader, MaxIdNames, GetSerializedTables());
			deserializer.Deserialize();
			XOP8 = deserializer.GetTable(XOP8Index);
			XOP9 = deserializer.GetTable(XOP9Index);
			XOPA = deserializer.GetTable(XOPAIndex);
		}
	}
}
#endif
