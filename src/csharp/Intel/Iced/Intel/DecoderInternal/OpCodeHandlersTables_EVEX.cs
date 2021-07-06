// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if DECODER && !NO_EVEX
namespace Iced.Intel.DecoderInternal {
	static partial class OpCodeHandlersTables_EVEX {
		internal static readonly OpCodeHandler[] Handlers_0F;
		internal static readonly OpCodeHandler[] Handlers_0F38;
		internal static readonly OpCodeHandler[] Handlers_0F3A;
		internal static readonly OpCodeHandler[] Handlers_MAP5;
		internal static readonly OpCodeHandler[] Handlers_MAP6;

		static OpCodeHandlersTables_EVEX() {
			var handlerReader = new EvexOpCodeHandlerReader();
			var deserializer = new TableDeserializer(handlerReader, MaxIdNames, GetSerializedTables());
			deserializer.Deserialize();
			Handlers_0F = deserializer.GetTable(Handlers_0FIndex);
			Handlers_0F38 = deserializer.GetTable(Handlers_0F38Index);
			Handlers_0F3A = deserializer.GetTable(Handlers_0F3AIndex);
			Handlers_MAP5 = deserializer.GetTable(Handlers_MAP5Index);
			Handlers_MAP6 = deserializer.GetTable(Handlers_MAP6Index);
		}
	}
}
#endif
