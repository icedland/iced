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
using System.Collections.Generic;
using System.Linq;
using Generator.Enums;
using Generator.Enums.Decoder;
using Generator.IO;

namespace Generator.Decoder.Rust {
	abstract class DecoderTableSerializer {
		public abstract string Name { get; }
		public virtual string HandlersModuleName => Name;
		protected abstract (string name, object?[] handlers)[] Handlers { get; }
		protected abstract (string origName, string newName)[] RootNames { get; }
		protected abstract string GetHandlerTypeName(EnumValue handlerType);
		protected abstract void WriteFieldsCore(FileWriter writer, object?[] handler);
		protected virtual IEnumerable<string> ExtraUseStatements => Array.Empty<string>();

		readonly IdentifierConverter idConverter;
		readonly Dictionary<string, string> nameRefToHandlerType;
		readonly Dictionary<string, object?[]> nameRefToArray;
		string? invalidNameRef;
		int unsafeCount;

		protected DecoderTableSerializer() {
			idConverter = RustIdentifierConverter.Create();
			nameRefToHandlerType = new Dictionary<string, string>(StringComparer.Ordinal);
			nameRefToArray = new Dictionary<string, object?[]>();
			invalidNameRef = null;
			unsafeCount = 0;
		}

		protected string GetInvalid() => invalidNameRef ?? throw new InvalidOperationException();
		static bool IsInvalid(EnumValue enumValue) {
			if (enumValue.DeclaringType.TypeId == TypeIds.OpCodeHandlerKind)
				return enumValue.Value == (uint)OpCodeHandlerKindEnum.Enum.Invalid;
			if (enumValue.DeclaringType.TypeId == TypeIds.VexOpCodeHandlerKind)
				return enumValue.Value == (uint)VexOpCodeHandlerKindEnum.Enum.Invalid;
			if (enumValue.DeclaringType.TypeId == TypeIds.EvexOpCodeHandlerKind)
				return enumValue.Value == (uint)EvexOpCodeHandlerKindEnum.Enum.Invalid;
			return false;
		}

		public void Serialize(FileWriter writer) {
			writer.WriteFileHeader();
			writer.WriteLine(RustConstants.AttributeNoRustFmtInner);
			writer.WriteLine();

			var uses = new List<string>(ExtraUseStatements) {
				"use super::handlers::*;",
				$"use super::handlers_{HandlersModuleName}::*;",
			};
			uses.Sort(StringComparer.Ordinal);
			foreach (var use in uses)
				writer.WriteLine(use);
			writer.WriteLine("use super::*;");

			var renamedTables = RootNames.ToDictionary(a => a.origName, a => a.newName, StringComparer.Ordinal);
			// rustc 1.27.0-1.35.0 can't compile the code: 'error[E0597]: borrowed value does not live long enough'
			// so create temporaries for everything that's not at top level.
			// rustc 1.20.0-1.26.0 and 1.36.0+ have no problem with the code.
			var infos = new DecoderTableOptimizer(Handlers, RootNames.Select(a => a.origName).ToArray(), idConverter, forceAllStatics: true).Optimize();

			foreach (var info in infos) {
				if (HandlerUtils.IsHandler(info.handlers, out var enumValue)) {
					var typeName = GetHandlerTypeName(enumValue);
					nameRefToHandlerType.Add(info.name, typeName);
					if (IsInvalid(enumValue))
						invalidNameRef = info.name;
				}
				else
					nameRefToArray.Add(info.name, info.handlers);
			}
			if (invalidNameRef is null)
				throw new InvalidOperationException();

			foreach (var info in infos) {
				var name = info.name;
				var handlers = info.handlers;
				bool isPublic;
				if (renamedTables.TryGetValue(name, out var newName)) {
					isPublic = true;
					name = newName;
				}
				else {
					isPublic = false;
					name = idConverter.Constant(name);
				}

				writer.WriteLine();

				if (HandlerUtils.IsHandler(info.handlers, out var handlerType)) {
					var typeName = GetHandlerTypeName(handlerType);
					writer.WriteLine($"static {idConverter.Constant(info.name)}: {typeName} = {typeName} {{");
					writer.Indent();
					WriteFields(writer, info.handlers);
					writer.Unindent();
					writer.WriteLine("};");
				}
				else {
					var pubStr = isPublic ? "pub(crate) " : string.Empty;
					writer.WriteLine($"{pubStr}static {name}: [&OpCodeHandler; {(info.handlers.Length >= 10 ? "0x" + info.handlers.Length.ToString("X") : info.handlers.Length.ToString())}] = [");

					writer.Indent();
					for (int i = 0; i < info.handlers.Length; i++) {
						writer.Write($"/*{i:X2}*/");
						Write(writer, info.handlers[i]);
					}
					writer.Unindent();

					writer.WriteLine("];");
				}
			}
		}

		void WriteFields(FileWriter writer, object?[] handler) {
			if (!HandlerUtils.IsHandler(handler, out var handlerType))
				throw new InvalidOperationException();
			WriteFieldsCore(writer, handler);
		}

		protected void WriteFirstFields(FileWriter writer, string handlerTypeName, bool hasModRM) {
			writer.WriteLine($"decode: {handlerTypeName}::decode,");
			writer.WriteLine($"has_modrm: {(hasModRM ? "true" : "false")},");
		}

		protected void WriteField(FileWriter writer, string fieldName, object? data) {
			writer.Write(fieldName);
			writer.Write(": ");
			Write(writer, data);
		}

		string BeginUnsafe() {
			unsafeCount++;
			if (unsafeCount == 1)
				return "unsafe { ";
			return string.Empty;
		}
		string CloseUnsafe() {
			unsafeCount--;
			if (unsafeCount < 0)
				throw new InvalidOperationException();
			if (unsafeCount == 0)
				return " }";
			return string.Empty;
		}

		void Write(FileWriter writer, object? data) {
			string typeName;
			switch (data) {
			case object?[] handler:
				if (HandlerUtils.IsHandler(handler, out var handlerEnumValue)) {
					typeName = GetHandlerTypeName(handlerEnumValue);
					writer.WriteLine($"{BeginUnsafe()}HandlerTransmuter {{ from: &{typeName} {{");
					writer.Indent();
					WriteFields(writer, handler);
					writer.Unindent();
					writer.WriteLine($"}} }}.to{CloseUnsafe()},");
				}
				else {
					writer.WriteLine("[");
					writer.Indent();
					foreach (var d in handler)
						Write(writer, d);
					writer.Unindent();
					writer.WriteLine("],");
				}
				break;

			case string handlerRef:
				var newHandlerRef = idConverter.Static(handlerRef);
				if (nameRefToHandlerType.TryGetValue(handlerRef, out _))
					writer.WriteLine($"{BeginUnsafe()}HandlerTransmuter {{ from: &{newHandlerRef} }}.to{CloseUnsafe()},");
				else
					writer.WriteLine($"&{newHandlerRef},");
				break;

			case EnumValue enumValue:
				string name;
				if (enumValue.DeclaringType.IsFlags)
					name = idConverter.Constant(enumValue.RawName);
				else
					name = enumValue.Name(idConverter);
				writer.Write($"{enumValue.DeclaringType.Name(idConverter)}::{name}");
				if (enumValue.DeclaringType.TypeId == TypeIds.Code)
					writer.Write(" as u32");
				writer.WriteLine(",");
				break;

			case OrEnumValue orEnumValue:
				for (int i = 0; i < orEnumValue.Values.Length; i++) {
					var value = orEnumValue.Values[i];
					if (i > 0)
						writer.Write(" | ");
					writer.Write($"{value.DeclaringType.Name(idConverter)}::{idConverter.Constant(value.RawName)}");
				}
				writer.WriteLine(",");
				break;

			case int value:
				writer.WriteLine($"{NumberFormatter.FormatHexUInt32WithSep((uint)value)},");
				break;

			case uint value:
				writer.WriteLine($"{NumberFormatter.FormatHexUInt32WithSep(value)},");
				break;

			case bool b:
				writer.WriteLine(b ? "true," : "false,");
				break;

			case ValueTuple<object, object> t:
				writer.WriteLine("(");
				writer.Indent();
				Write(writer, t.Item1);
				Write(writer, t.Item2);
				writer.Unindent();
				writer.WriteLine("),");
				break;

			case null:
				writer.WriteLine($"{BeginUnsafe()}HandlerTransmuter {{ from: &GEN_NULL_HANDLER }}.to{CloseUnsafe()},");
				break;

			default:
				throw new InvalidOperationException();
			}
		}

		protected object? VerifyArray(int count, object? data) {
			if (data is object?[] array && array.Length == count)
				return data;
			if (data is string nameRef && nameRefToArray.TryGetValue(nameRef, out var refArray) && refArray.Length == count)
				return data;
			throw new InvalidOperationException();
		}
	}
}
