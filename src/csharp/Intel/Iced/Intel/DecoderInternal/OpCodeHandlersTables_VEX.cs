// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if DECODER && !NO_VEX
namespace Iced.Intel.DecoderInternal {
	static partial class OpCodeHandlersTables_VEX {
#if MVEX
		internal static readonly OpCodeHandler[] Handlers_MAP0;
#endif
		internal static readonly OpCodeHandler[] Handlers_0F;
		internal static readonly OpCodeHandler[] Handlers_0F38;
		internal static readonly OpCodeHandler[] Handlers_0F3A;

		static OpCodeHandlersTables_VEX() {
			var handlerReader = new VexOpCodeHandlerReader();
			var deserializer = new TableDeserializer(handlerReader, MaxIdNames, GetSerializedTables());
			deserializer.Deserialize();
#if MVEX
			Handlers_MAP0 = deserializer.GetTable(Handlers_MAP0Index);
#endif
			Handlers_0F = deserializer.GetTable(Handlers_0FIndex);
			Handlers_0F38 = deserializer.GetTable(Handlers_0F38Index);
			Handlers_0F3A = deserializer.GetTable(Handlers_0F3AIndex);
		}
	}
}
#endif
