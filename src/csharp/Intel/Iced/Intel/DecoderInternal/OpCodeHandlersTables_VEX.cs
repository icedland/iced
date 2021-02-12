// SPDX-License-Identifier: MIT
// Copyright wtfsck@protonmail.com
// Copyright iced contributors

#if DECODER && !NO_VEX
namespace Iced.Intel.DecoderInternal {
	static partial class OpCodeHandlersTables_VEX {
		internal static readonly OpCodeHandler[] ThreeByteHandlers_0F38XX;
		internal static readonly OpCodeHandler[] ThreeByteHandlers_0F3AXX;
		internal static readonly OpCodeHandler[] TwoByteHandlers_0FXX;

		static OpCodeHandlersTables_VEX() {
			var handlerReader = new VexOpCodeHandlerReader();
			var deserializer = new TableDeserializer(handlerReader, MaxIdNames, GetSerializedTables());
			deserializer.Deserialize();
			ThreeByteHandlers_0F38XX = deserializer.GetTable(ThreeByteHandlers_0F38XXIndex);
			ThreeByteHandlers_0F3AXX = deserializer.GetTable(ThreeByteHandlers_0F3AXXIndex);
			TwoByteHandlers_0FXX = deserializer.GetTable(TwoByteHandlers_0FXXIndex);
		}
	}
}
#endif
