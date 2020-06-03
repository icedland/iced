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
using Generator.Tables;

namespace Generator {
	/// <summary>
	/// Can be implemented by classes with a <see cref="TypeGenAttribute"/>
	/// </summary>
	interface ITypeGen {
		void Generate(GenTypes genTypes) { }
		void OnGenerated(GenTypes genTypes) { }
	}

	/// <summary>
	/// Can be implemented by classes with a <see cref="TypeGenAttribute"/> and if so,
	/// its priority must be &lt; <see cref="TypeGenOrders.CreatedInstructions"/>
	/// </summary>
	interface ICreatedInstructions {
		void OnCreatedInstructions(GenTypes genTypes, HashSet<EnumValue> filteredCodeValues);
	}

	sealed class GenTypes {
		readonly Dictionary<TypeId, EnumType> enums;
		readonly Dictionary<TypeId, ConstantsType> constants;
		readonly Dictionary<TypeId, object> objs;
		bool canGetCodeEnum;

		public GeneratorOptions Options { get; }

		public GenTypes(GeneratorOptions options) {
			canGetCodeEnum = false;
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

			var created = new List<object>();
			int index = 0;
			CallTypeGens(created, typeGens, ref index, order => order < TypeGenOrders.PreCreateInstructions);
			canGetCodeEnum = true;
			CallTypeGens(created, typeGens, ref index, order => order < TypeGenOrders.CreatedInstructions);

			var (origCodeValues, removedCodeHash, codeHash) = FilterCode();
			AddObject(TypeIds.OrigCodeValues, origCodeValues);
			AddObject(TypeIds.RemovedCodeValues, removedCodeHash);
			for (int i = 0; i < index; i++) {
				if (created[i] is ICreatedInstructions createdInstrs)
					createdInstrs.OnCreatedInstructions(this, codeHash);
			}

			int createdCount = created.Count;
			CallTypeGens(created, typeGens, ref index, order => true);
			for (int i = createdCount; i < created.Count; i++) {
				if (created[i] is ICreatedInstructions)
					throw new InvalidOperationException($"Can't impl {nameof(ICreatedInstructions)}, update the {nameof(TypeGenAttribute)} order");
			}

			for (int i = 0; i < created.Count; i++) {
				if (created[i] is ITypeGen tg)
					tg.OnGenerated(this);
			}
		}

		void CallTypeGens(List<object> created, List<(Type type, TypeGenAttribute ca)> typeGens, ref int index, Func<double, bool> checkOrder) {
			for (; index < typeGens.Count; index++) {
				var typeGen = typeGens[index];
				if (!checkOrder(typeGen.ca.Order))
					break;
				var ctor = typeGen.type.GetTypeInfo().DeclaredConstructors.FirstOrDefault(c => c.GetParameters().Length == 1 && c.GetParameters()[0].ParameterType == typeof(GenTypes));
				if (ctor is null)
					throw new InvalidOperationException($"{typeGen.type} needs a constructor with a {nameof(GenTypes)} argument");
				var gen = ctor.Invoke(new object[] { this });
				if (gen is null)
					throw new InvalidOperationException();
				created.Add(gen);
				if (gen is ITypeGen tg)
					tg.Generate(this);
			}
		}

		(EnumValue[] origCodeValues, HashSet<EnumValue> removedCodeHash, HashSet<EnumValue> codeHash) FilterCode() {
			var defs = GetObject<InstructionDefs>(TypeIds.InstructionDefs).GetDefsPreFiltered();

			var newCodeValues = new List<EnumValue>();
			var removedCodeHash = new HashSet<EnumValue>();
			foreach (var def in defs) {
				if (ShouldInclude(def))
					newCodeValues.Add(def.OpCodeInfo.Code);
				else
					removedCodeHash.Add(def.OpCodeInfo.Code);
			}

			var code = this[TypeIds.Code];
			var origCodeValues = code.Values.ToArray();
			code.ResetValues(newCodeValues.ToArray());

			return (origCodeValues, removedCodeHash, newCodeValues.ToHashSet());
		}

		bool ShouldInclude(InstructionDef def) {
			switch (def.OpCodeInfo.Encoding) {
			case EncodingKind.Legacy:
				break;
			case EncodingKind.VEX:
				if (!Options.IncludeVEX)
					return false;
				break;
			case EncodingKind.EVEX:
				if (!Options.IncludeEVEX)
					return false;
				break;
			case EncodingKind.XOP:
				if (!Options.IncludeXOP)
					return false;
				break;
			case EncodingKind.D3NOW:
				if (!Options.Include3DNow)
					return false;
				break;
			default:
				throw new InvalidOperationException();
			}

			if (Options.IncludeCpuid.Count != 0) {
				foreach (var cpuid in def.InstrInfo.Cpuid) {
					if (Options.IncludeCpuid.Contains(cpuid.RawName))
						return true;
				}
				return false;
			}
			if (Options.ExcludeCpuid.Count != 0) {
				foreach (var cpuid in def.InstrInfo.Cpuid) {
					if (Options.ExcludeCpuid.Contains(cpuid.RawName))
						return false;
				}
			}

			return true;
		}

		public void Add(EnumType enumType) => enums.Add(enumType.TypeId, enumType);
		public void Add(ConstantsType constantsType) => constants.Add(constantsType.TypeId, constantsType);

		public void AddObject(TypeId id, object obj) => objs.Add(id, obj);

		public T GetObject<T>(TypeId id) {
			if (!canGetCodeEnum && id == TypeIds.Code)
				throw new InvalidOperationException($"Can't read Code value yet. Update the class' {nameof(TypeGenAttribute)} order");
			if (objs.TryGetValue(id, out var obj)) {
				if (obj is T t)
					return t;
				throw new InvalidOperationException($"{id} is not a {typeof(T).FullName}");
			}
			throw new InvalidOperationException($"{id} doesn't exist");
		}

		public EnumValue[] GetKeptCodeValues(params Code[] values) {
			var origCode = GetObject<EnumValue[]>(TypeIds.OrigCodeValues);
			var removed = GetObject<HashSet<EnumValue>>(TypeIds.RemovedCodeValues);
			return values.Select(a => origCode[(int)a]).Where(a => !removed.Contains(a)).ToArray();
		}
	}
}
