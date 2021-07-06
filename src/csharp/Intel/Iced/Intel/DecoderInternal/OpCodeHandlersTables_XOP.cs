// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if DECODER && !NO_XOP
namespace Iced.Intel.DecoderInternal {
	static partial class OpCodeHandlersTables_XOP {
		internal static readonly OpCodeHandler[] Handlers_MAP8;
		internal static readonly OpCodeHandler[] Handlers_MAP9;
		internal static readonly OpCodeHandler[] Handlers_MAP10;

		static OpCodeHandlersTables_XOP() {
			var handlerReader = new VexOpCodeHandlerReader();
			var deserializer = new TableDeserializer(handlerReader, MaxIdNames, GetSerializedTables());
			deserializer.Deserialize();
			Handlers_MAP8 = deserializer.GetTable(Handlers_MAP8Index);
			Handlers_MAP9 = deserializer.GetTable(Handlers_MAP9Index);
			Handlers_MAP10 = deserializer.GetTable(Handlers_MAP10Index);
		}
	}
}
#endif
