/*
Copyright (C) 2018-2019 de4dot@gmail.com

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

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
				((enumValue.DeclaringType.TypeId == TypeIds.OpCodeHandlerKind && enumValue == genTypes[TypeIds.OpCodeHandlerKind][nameof(OpCodeHandlerKind.Invalid)]) ||
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
