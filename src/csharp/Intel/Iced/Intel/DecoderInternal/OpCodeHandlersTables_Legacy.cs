// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if DECODER
namespace Iced.Intel.DecoderInternal {
	static partial class OpCodeHandlersTables_Legacy {
		internal static readonly OpCodeHandler[] Handlers_MAP0;

		static OpCodeHandlersTables_Legacy() {
			var handlerReader = new LegacyOpCodeHandlerReader();
			var deserializer = new TableDeserializer(handlerReader, MaxIdNames, GetSerializedTables());
			deserializer.Deserialize();
			Handlers_MAP0 = deserializer.GetTable(Handlers_MAP0Index);
		}
	}
}
#endif
