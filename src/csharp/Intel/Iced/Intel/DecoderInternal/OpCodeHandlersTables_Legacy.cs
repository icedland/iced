// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if DECODER
namespace Iced.Intel.DecoderInternal {
	static partial class OpCodeHandlersTables_Legacy {
		internal static readonly OpCodeHandler[] OneByteHandlers;

		static OpCodeHandlersTables_Legacy() {
			var handlerReader = new LegacyOpCodeHandlerReader();
			var deserializer = new TableDeserializer(handlerReader, MaxIdNames, GetSerializedTables());
			deserializer.Deserialize();
			OneByteHandlers = deserializer.GetTable(OneByteHandlersIndex);
		}
	}
}
#endif
