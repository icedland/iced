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
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Generator.Enums;

namespace Generator.Decoder {
	// Re-uses duplicate handlers
	sealed class DecoderTableOptimizer {
		readonly Dictionary<object, string> dupes;
		readonly Dictionary<string, (string name, object?[] handlers, int id)> referencedHandlers;

		public DecoderTableOptimizer((string name, object?[] handlers)[] handlers, string[] rootNames, IdentifierConverter idConverter, bool forceAllStatics) {
			if (rootNames.Length == 0)
				throw new InvalidOperationException();
			referencedHandlers = new Dictionary<string, (string name, object?[] handlers, int id)>();

			var nameToHandlers = new Dictionary<string, object?[]>();
			foreach (var info in handlers)
				nameToHandlers.Add(info.name, info.handlers);

			var handlerComparer = new HandlerEqualityComparer(s => {
				if (referencedHandlers.TryGetValue(s, out var info))
					return info.handlers;
				if (nameToHandlers.TryGetValue(s, out var other))
					return other;
				throw new InvalidOperationException();
			});
			var dict = new Dictionary<object, int>(handlerComparer);
			var nameRefsToCheck = new Stack<string>(rootNames);
			var referencedNames = new HashSet<string>(StringComparer.Ordinal);
			var traverser = new HandlerTraverser(obj => {
				if (IsHandler(obj)) {
					dict.TryGetValue(obj, out var count);
					count++;
					dict[obj] = count;
				}
				else if (obj is string nameRef) {
					if (referencedNames.Add(nameRef))
						nameRefsToCheck.Push(nameRef);
				}
				return (obj, false);
			});
			int currentId = 0;
			var origIds = new Dictionary<string, int>(StringComparer.Ordinal);
			for (int i = handlers.Length - 1; i >= 0; i--)
				origIds.Add(handlers[i].name, currentId++);
			while (nameRefsToCheck.Count > 0) {
				var name = nameRefsToCheck.Pop();
				var hs = nameToHandlers[name];
				nameToHandlers.Remove(name);
				if (!origIds.TryGetValue(name, out var id))
					id = currentId++;
				referencedHandlers.Add(name, (name, hs, id));
				foreach (var handler in hs)
					traverser.Visit(handler);
			}
			dupes = new Dictionary<object, string>(handlerComparer);
			foreach (var kv in dict) {
				if (forceAllStatics || kv.Value > 1) {
					var hs = (object?[])kv.Key;
					string name = CreateName(idConverter, hs, referencedHandlers);
					dupes.Add(kv.Key, name);
					referencedHandlers.Add(name, (name, hs, currentId++));
				}
			}
		}

		static string CreateName(IdentifierConverter idConverter, object?[] handlers, Dictionary<string, (string name, object?[] handlers, int id)> referencedHandlers) {
			if (handlers.Length == 0)
				throw new InvalidOperationException();
			if (!(handlers[0] is EnumValue enumValue))
				throw new InvalidOperationException();
			var baseName = enumValue.RawName;
			if (handlers.Length >= 2 && handlers[1] is EnumValue codeValue && codeValue.DeclaringType.TypeId == TypeIds.Code)
				baseName = baseName + "_" + codeValue.RawName;
			baseName = "Gen" + baseName;
			baseName = idConverter.Static(baseName);
			if (!referencedHandlers.ContainsKey(baseName))
				return baseName;
			baseName += "_";
			for (uint i = 1; ; i++) {
				var newName = baseName + i.ToString();
				if (!referencedHandlers.ContainsKey(newName))
					return newName;
			}
		}

		static bool IsHandler([NotNullWhen(true)] object? data) =>
			data is object?[] array && array[0] is IEnumValue;

		public (string name, object?[] handlers)[] Optimize() {
			var traverser = new HandlerTraverser(obj => {
				if (IsHandler(obj) && dupes.TryGetValue(obj, out var newHandler))
					return (newHandler, true);
				return (obj, false);
			});
			var result = new List<(string name, object?[] handlers)>();
			foreach (var info in referencedHandlers.OrderByDescending(a => a.Value.id)) {
				var name = info.Value.name;
				var handlers = info.Value.handlers;
				for (int i = 0; i < handlers.Length; i++)
					handlers[i] = traverser.Visit(handlers[i]);
				result.Add((name, handlers));
			}
			return result.ToArray();
		}
	}
}
