// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Diagnostics.CodeAnalysis;
using Generator.Enums;
using Generator.Enums.Decoder;

namespace Generator.Decoder {
	static class DecoderTableUtils {
		public static bool IsInvalid(GenTypes genTypes, object?[] handler) {
			var data = handler[0];
			bool isInvalid =
				data is IEnumValue enumValue &&
				((enumValue.DeclaringType.TypeId == TypeIds.LegacyOpCodeHandlerKind && enumValue == genTypes[TypeIds.LegacyOpCodeHandlerKind][nameof(LegacyOpCodeHandlerKind.Invalid)]) ||
				(enumValue.DeclaringType.TypeId == TypeIds.VexOpCodeHandlerKind && enumValue == genTypes[TypeIds.VexOpCodeHandlerKind][nameof(VexOpCodeHandlerKind.Invalid)]) ||
				(enumValue.DeclaringType.TypeId == TypeIds.EvexOpCodeHandlerKind && enumValue == genTypes[TypeIds.EvexOpCodeHandlerKind][nameof(EvexOpCodeHandlerKind.Invalid)]));
			if (isInvalid && handler.Length != 1)
				throw new InvalidOperationException();
			return isInvalid;
		}

		public static bool IsHandler(object?[] handlers) => IsHandler(handlers, out _);
		public static bool IsHandler(object?[] handlers, [NotNullWhen(true)] out EnumValue? enumValue) {
			enumValue = handlers[0] as EnumValue;
			return enumValue is not null;
		}
	}
}
