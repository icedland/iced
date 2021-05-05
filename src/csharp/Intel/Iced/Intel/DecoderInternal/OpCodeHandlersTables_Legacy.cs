// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if DECODER
namespace Iced.Intel.DecoderInternal {
	static partial class OpCodeHandlersTables_Legacy {
		internal static readonly OpCodeHandler[] OneByteHandlers;
		internal static readonly OpCodeHandler[] TwoByteHandlers_0FXX;

		static OpCodeHandlersTables_Legacy() {
			var handlerReader = new LegacyOpCodeHandlerReader();
			var deserializer = new TableDeserializer(handlerReader, MaxIdNames, GetSerializedTables());
			deserializer.Deserialize();
			OneByteHandlers = deserializer.GetTable(OneByteHandlersIndex);
			TwoByteHandlers_0FXX = deserializer.GetTable(TwoByteHandlers_0FXXIndex);
		}
	}
}
#endif
