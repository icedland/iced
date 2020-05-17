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
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Generator.Constants;
using Generator.Enums;

namespace Generator {
	interface ITypeGen {
		void Generate(GenTypes genTypes);
	}

	sealed class GenTypes {
		readonly Dictionary<TypeId, EnumType> enums;
		readonly Dictionary<TypeId, ConstantsType> constants;
		readonly Dictionary<TypeId, object> objs;

		public GeneratorOptions Options { get; }

		public GenTypes(GeneratorOptions options) {
			Options = options;
			constants = new Dictionary<TypeId, ConstantsType>();
			objs = new Dictionary<TypeId, object>();
			var assembly = GetType().Assembly;
			enums = CreateEnumsDict(assembly);
			CallTypeGens(assembly);
		}

		static Dictionary<TypeId, EnumType> CreateEnumsDict(Assembly assembly) {
			var allTypeIds = typeof(TypeIds).GetFields().Select(a => (TypeId)a.GetValue(null)!).ToHashSet();
			var enums = new Dictionary<TypeId, EnumType>();
			foreach (var type in assembly.GetTypes()) {
				var ca = type.GetCustomAttribute<EnumAttribute>(false);
				if (ca is null)
					continue;
				if (!allTypeIds.Contains(ca.TypeId))
					throw new InvalidOperationException();
				allTypeIds.Remove(ca.TypeId);
				var flags = EnumTypeFlags.None;
				if (ca.Public)
					flags |= EnumTypeFlags.Public;
				if (ca.NoInitialize)
					flags |= EnumTypeFlags.NoInitialize;
				if (ca.Flags)
					flags |= EnumTypeFlags.Flags;
				var values = type.GetFields().Where(a => a.IsLiteral).Select(a => new EnumValue(GetValue(a.GetValue(null)), a.Name, CommentAttribute.GetDocumentation(a))).ToArray();
				var enumType = new EnumType(ca.Name, ca.TypeId, ca.Documentation, values, flags);
				enums.Add(ca.TypeId, enumType);
			}
			return enums;
		}

		static uint GetValue(object? value) {
			if (value is null)
				throw new InvalidOperationException();
			return Type.GetTypeCode(value.GetType().GetEnumUnderlyingType()) switch {
				TypeCode.SByte => (uint)(sbyte)value,
				TypeCode.Byte => (byte)value,
				TypeCode.Int16 => (uint)(short)value,
				TypeCode.UInt16 => (ushort)value,
				TypeCode.Int32 => (uint)(int)value,
				TypeCode.UInt32 => (uint)value,
				_ => throw new InvalidOperationException(),
			};
		}

		public EnumType this[TypeId id] {
			get {
				if (!enums.TryGetValue(id, out var enumType))
					throw new InvalidOperationException($"Enum type {id} doesn't exist");
				return enumType;
			}
		}

		public ConstantsType GetConstantsType(TypeId id) {
			if (!constants.TryGetValue(id, out var constantsType))
				throw new InvalidOperationException($"Constants type {id} doesn't exist");
			return constantsType;
		}

		void CallTypeGens(Assembly assembly) {
			var typeGens = new List<(Type type, TypeGenAttribute ca)>();
			foreach (var type in assembly.GetTypes()) {
				var ca = type.GetCustomAttribute<TypeGenAttribute>(false);
				if (ca is null)
					continue;
				typeGens.Add((type, ca));
			}
			typeGens.Sort((a, b) => {
				int c = a.ca.Order.CompareTo(b.ca.Order);
				if (c != 0)
					return c;
				Debug.Assert(a.type.FullName is object);
				Debug.Assert(b.type.FullName is object);
				return a.type.FullName.CompareTo(b.type.FullName);
			});
			foreach (var (type, ca) in typeGens) {
				var ctor = type.GetTypeInfo().DeclaredConstructors.FirstOrDefault(c => c.GetParameters().Length == 1 && c.GetParameters()[0].ParameterType == typeof(GenTypes));
				if (ctor is null)
					throw new InvalidOperationException($"{type} needs a constructor with a {nameof(GenTypes)} argument");
				var gen = ctor.Invoke(new object[] { this });
				if (gen is null)
					throw new InvalidOperationException();
				if (gen is ITypeGen typeGen)
					typeGen.Generate(this);
			}
		}

		public void Add(EnumType enumType) => enums.Add(enumType.TypeId, enumType);
		public void Add(ConstantsType constantsType) => constants.Add(constantsType.TypeId, constantsType);

		public void AddObject(TypeId id, object obj) => objs.Add(id, obj);

		public T GetObject<T>(TypeId id) {
			if (objs.TryGetValue(id, out var obj)) {
				if (obj is T t)
					return t;
				throw new InvalidOperationException($"{id} is not a {typeof(T).FullName}");
			}
			throw new InvalidOperationException($"{id} doesn't exist");
		}
	}
}
