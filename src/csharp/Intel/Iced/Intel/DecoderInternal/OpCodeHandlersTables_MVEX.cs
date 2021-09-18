// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if DECODER && MVEX
namespace Iced.Intel.DecoderInternal {
	static partial class OpCodeHandlersTables_MVEX {
		internal static readonly OpCodeHandler[] Handlers_0F;
		internal static readonly OpCodeHandler[] Handlers_0F38;
		internal static readonly OpCodeHandler[] Handlers_0F3A;

		static OpCodeHandlersTables_MVEX() {
			var handlerReader = new MvexOpCodeHandlerReader();
			var deserializer = new TableDeserializer(handlerReader, MaxIdNames, GetSerializedTables());
			deserializer.Deserialize();
			Handlers_0F = deserializer.GetTable(Handlers_0FIndex);
			Handlers_0F38 = deserializer.GetTable(Handlers_0F38Index);
			Handlers_0F3A = deserializer.GetTable(Handlers_0F3AIndex);
		}
	}
}
#endif
