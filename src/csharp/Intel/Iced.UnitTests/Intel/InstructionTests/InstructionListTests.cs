// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if ENCODER
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.InstructionTests {
	public sealed class InstructionListTests {
		[Fact]
		void Default_ctor_creates_empty_list() {
			var list = new InstructionList();
			Assert.Equal(0, list.Count);
		}

		static readonly byte[] testCode = new byte[] {
			0x48, 0x89, 0x5C, 0x24, 0x10, 0x48, 0x89, 0x74, 0x24, 0x18, 0x55, 0x57, 0x41, 0x56, 0x48, 0x8D,
			0xAC, 0x24, 0x00, 0xFF, 0xFF, 0xFF, 0x48, 0x81, 0xEC, 0x00, 0x02, 0x00, 0x00, 0x48, 0x8B, 0x05,
			0x18, 0x57, 0x0A, 0x00, 0x48, 0x33, 0xC4, 0x48, 0x89, 0x85, 0xF0, 0x00, 0x00, 0x00, 0x4C, 0x8B,
			0x05, 0x2F, 0x24, 0x0A, 0x00, 0x48, 0x8D, 0x05, 0x78, 0x7C, 0x04, 0x00, 0x33, 0xFF
		};
		static Instruction[] GetInstructions() {
			var decoder = Decoder.Create(64, new ByteArrayCodeReader(testCode));
			var endRip = decoder.IP + (uint)testCode.Length;
			var list = new List<Instruction>();
			while (decoder.IP < endRip)
				list.Add(decoder.Decode());
			return list.ToArray();
		}

		static void AssertEqual(IList<Instruction> expected, IList<Instruction> actual, int elems = -1) {
			if (elems == -1)
				elems = expected.Count;
			Assert.True(elems <= expected.Count);
			Assert.True(elems <= actual.Count);
			for (int i = 0; i < elems; i++) {
				var expInstr = expected[i];
				var actualInstr = actual[i];
				Assert.True(Instruction.EqualsAllBits(expInstr, actualInstr));
			}
		}

		[Fact]
		void Implements_ifaces() {
			Assert.True(typeof(IList<Instruction>).IsAssignableFrom(typeof(InstructionList)));
			Assert.True(typeof(IReadOnlyList<Instruction>).IsAssignableFrom(typeof(InstructionList)));
			Assert.True(typeof(System.Collections.IList).IsAssignableFrom(typeof(InstructionList)));
		}

		[Fact]
		void Default_ctor_has_no_capacity() {
			var list = new InstructionList();
			Assert.Equal(0, list.Count);
			Assert.Equal(0, list.Capacity);
		}

		[Theory]
		[MemberData(nameof(Ctor_array_Data))]
		void Ctor_array(Instruction[] instructions) {
			var list = new InstructionList(instructions);
			Assert.Equal(instructions.Length, list.Count);
			Assert.Equal(instructions.Length, list.Capacity);
			var listElems = new Instruction[list.Count];
			list.CopyTo(listElems);
			AssertEqual(instructions, listElems);
		}
		public static IEnumerable<object[]> Ctor_array_Data {
			get {
				yield return new object[] { Array.Empty<Instruction>() };
				yield return new object[] { new Instruction[] { Instruction.Create(Code.Nopd) } };
				yield return new object[] { GetInstructions() };
			}
		}

		[Theory]
		[MemberData(nameof(Ctor_IEnumerable_Data))]
		void Ctor_IEnumerable(Instruction[] instructions) {
			var list = new InstructionList(AsEnumerable(instructions));
			Assert.Equal(instructions.Length, list.Count);
			Assert.True(instructions.Length <= list.Capacity);
			var listElems = new Instruction[list.Count];
			list.CopyTo(listElems);
			AssertEqual(instructions, listElems);
		}
		public static IEnumerable<object[]> Ctor_IEnumerable_Data {
			get {
				yield return new object[] { Array.Empty<Instruction>() };
				yield return new object[] { new Instruction[] { Instruction.Create(Code.Nopd) } };
				yield return new object[] { GetInstructions() };
			}
		}

		[Theory]
		[MemberData(nameof(Ctor_InstructionList_Data))]
		void Ctor_InstructionList(Instruction[] instructions, InstructionList instructionList) {
			var list = new InstructionList(instructionList);
			Assert.Equal(instructions.Length, list.Count);
			Assert.Equal(instructions.Length, list.Capacity);
			var listElems = new Instruction[list.Count];
			list.CopyTo(listElems);
			AssertEqual(instructions, listElems);
		}
		public static IEnumerable<object[]> Ctor_InstructionList_Data {
			get {
				yield return Create(Array.Empty<Instruction>());
				yield return Create(new Instruction[] { Instruction.Create(Code.Nopd) });
				yield return Create(GetInstructions());

				static object[] Create(Instruction[] instructions) => new object[] { instructions, new InstructionList(instructions) };
			}
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(1234)]
		void Ctor_capacity_arg(int capacity) {
			var list = new InstructionList(capacity);
			Assert.Equal(0, list.Count);
			Assert.Equal(capacity, list.Capacity);
		}

		[Fact]
		void Ctor_InstructionList_throws_if_null() =>
			Assert.Throws<ArgumentNullException>(() => new InstructionList((InstructionList)null));

		[Fact]
		void Ctor_IEnumerable_throws_if_null() =>
			Assert.Throws<ArgumentNullException>(() => new InstructionList((IEnumerable<Instruction>)null));

		[Theory]
		[InlineData(-1)]
		[InlineData(int.MinValue)]
		void Ctor_capacity_arg_throws_if_invalid(int capacity) =>
			Assert.Throws<ArgumentOutOfRangeException>(() => new InstructionList(capacity));

		[Fact]
		void Indexer_works() {
			var instructions = GetInstructions();
			var list = new InstructionList(instructions);
			Assert.Equal(instructions.Length, list.Count);
			for (int i = 0; i < instructions.Length; i++)
				Assert.True(Instruction.EqualsAllBits(instructions[i], list[i]));
		}

		[Fact]
		void Indexer_throws_if_invalid_index() {
			InstructionList list;

			list = new InstructionList();
			Assert.Throws<IndexOutOfRangeException>(() => list[GetValue(0)]);
			Assert.Throws<IndexOutOfRangeException>(() => list[GetValue(-1)]);

			list = new InstructionList(GetInstructions());
			Assert.Throws<IndexOutOfRangeException>(() => list[GetValue(-1)]);
			Assert.Throws<IndexOutOfRangeException>(() => list[list.Count]);
			Assert.Throws<IndexOutOfRangeException>(() => list[GetValue(int.MaxValue)]);
			Assert.Throws<IndexOutOfRangeException>(() => list[GetValue(int.MinValue)]);
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		static int GetValue(int value) => value;

		[Fact]
		void Clear_works() {
			InstructionList list;

			list = new InstructionList();
			Assert.Equal(0, list.Count);
			Assert.Equal(0, list.Capacity);
			list.Clear();
			Assert.Equal(0, list.Count);
			Assert.Equal(0, list.Capacity);

			var instructions = GetInstructions();
			list = new InstructionList(instructions);
			Assert.Equal(instructions.Length, list.Count);
			Assert.Equal(instructions.Length, list.Capacity);
			list.Clear();
			Assert.Equal(0, list.Count);
			Assert.Equal(instructions.Length, list.Capacity);
		}

		[Fact]
		void Add_works() {
			var instructions = GetInstructions();
			var list = new InstructionList();
			for (int i = 0; i < instructions.Length; i++) {
				list.Add(instructions[i]);
				Assert.Equal(i + 1, list.Count);
				Assert.True(i < list.Capacity);
				var listElems = new Instruction[list.Count];
				list.CopyTo(listElems);
				AssertEqual(instructions, listElems, listElems.Length);
			}
		}

		[Fact]
		void AllocUninitializedElement_works() {
			var instructions = GetInstructions();
			var list = new InstructionList();
			for (int i = 0; i < instructions.Length; i++) {
				list.AllocUninitializedElement() = instructions[i];
				Assert.Equal(i + 1, list.Count);
				Assert.True(i < list.Capacity);
				var listElems = new Instruction[list.Count];
				list.CopyTo(listElems);
				AssertEqual(instructions, listElems, listElems.Length);
			}
		}

		[Fact]
		void Insert_works() {
			var instructions = GetInstructions();
			var list = new InstructionList();
			Assert.Equal(0, list.Count);
			Assert.Equal(0, list.Capacity);
			list.Insert(0, instructions[0]);
			Assert.Equal(1, list.Count);
			Assert.True(1 <= list.Capacity);
			list.Insert(1, instructions[1]);
			Assert.Equal(2, list.Count);
			Assert.True(2 <= list.Capacity);
			list.Insert(0, instructions[2]);
			Assert.Equal(3, list.Count);
			Assert.True(3 <= list.Capacity);
			list.Insert(1, instructions[3]);
			Assert.Equal(4, list.Count);
			Assert.True(4 <= list.Capacity);
			list.Insert(3, instructions[4]);
			Assert.Equal(5, list.Count);
			Assert.True(5 <= list.Capacity);
			list.Insert(5, instructions[5]);
			Assert.Equal(6, list.Count);
			Assert.True(6 <= list.Capacity);
			var expected = new Instruction[] {
				instructions[2],
				instructions[3],
				instructions[0],
				instructions[4],
				instructions[1],
				instructions[5],
			};
			var listElems = new Instruction[list.Count];
			list.CopyTo(listElems);
			AssertEqual(expected, listElems);
		}

		[Fact]
		void Insert_throws_if_invalid_input() {
			InstructionList list;

			list = new InstructionList();
			Assert.Throws<ArgumentOutOfRangeException>(() => list.Insert(GetValue(-1), Instruction.Create(Code.Nopd)));
			Assert.Throws<ArgumentOutOfRangeException>(() => list.Insert(GetValue(1), Instruction.Create(Code.Nopd)));

			list = new InstructionList(GetInstructions());
			Assert.Throws<ArgumentOutOfRangeException>(() => list.Insert(GetValue(-1), Instruction.Create(Code.Nopd)));
			Assert.Throws<ArgumentOutOfRangeException>(() => list.Insert(GetValue(int.MinValue), Instruction.Create(Code.Nopd)));
			Assert.Throws<ArgumentOutOfRangeException>(() => list.Insert(GetValue(int.MaxValue), Instruction.Create(Code.Nopd)));
			Assert.Throws<ArgumentOutOfRangeException>(() => list.Insert(GetValue(list.Count + 1), Instruction.Create(Code.Nopd)));
		}

		[Fact]
		void RemoveAt_works() {
			var instructions = GetInstructions();
			var list = new InstructionList(instructions);
			Assert.Equal(instructions.Length, list.Count);
			Assert.Equal(instructions.Length, list.Capacity);
			list.RemoveAt(0);
			Assert.Equal(instructions.Length - 1, list.Count);
			Assert.Equal(instructions.Length, list.Capacity);
			list.RemoveAt(list.Count - 1);
			Assert.Equal(instructions.Length - 2, list.Count);
			Assert.Equal(instructions.Length, list.Capacity);
			list.RemoveAt(4);
			Assert.Equal(instructions.Length - 3, list.Count);
			Assert.Equal(instructions.Length, list.Capacity);
			var expected = new Instruction[instructions.Length - 3];
			for (int i = 0, j = 0; i < instructions.Length; i++) {
				if (i == 0 || i == instructions.Length - 1 || i == 4 + 1)
					continue;
				expected[j++] = instructions[i];
			}
			var listElems = new Instruction[list.Count];
			list.CopyTo(listElems);
			AssertEqual(expected, listElems);
		}

		[Fact]
		void RemoveAt_throws_if_invalid_input() {
			var instructions = GetInstructions();
			InstructionList list;

			list = new InstructionList();
			Assert.Throws<ArgumentOutOfRangeException>(() => list.RemoveAt(GetValue(-1)));
			Assert.Throws<ArgumentOutOfRangeException>(() => list.RemoveAt(GetValue(0)));

			list = new InstructionList(instructions);
			Assert.Throws<ArgumentOutOfRangeException>(() => list.RemoveAt(GetValue(-1)));
			Assert.Throws<ArgumentOutOfRangeException>(() => list.RemoveAt(GetValue(int.MinValue)));
			Assert.Throws<ArgumentOutOfRangeException>(() => list.RemoveAt(GetValue(int.MaxValue)));
			Assert.Throws<ArgumentOutOfRangeException>(() => list.RemoveAt(GetValue(list.Count)));
		}

		[Theory]
		[MemberData(nameof(ForEach_works_Data))]
		void ForEach_works(Instruction[] instructions) {
			var list = new InstructionList(instructions);
			Assert.Equal(instructions.Length, list.Count);
			Assert.Equal(instructions.Length, list.Capacity);
			int index = 0;
			foreach (ref var instruction in list) {
				Assert.True(index < instructions.Length);
				Assert.True(Instruction.EqualsAllBits(instructions[index], instruction));
				index++;
			}
			Assert.Equal(instructions.Length, index);
		}
		public static IEnumerable<object[]> ForEach_works_Data {
			get {
				yield return new object[] { Array.Empty<Instruction>() };
				yield return new object[] { new Instruction[] { Instruction.Create(Code.Nopd) } };
				yield return new object[] { GetInstructions() };
			}
		}

		[Theory]
		[MemberData(nameof(ToArray_works_Data))]
		void ToArray_works(Instruction[] instructions, bool addExtraElem) {
			var list = new InstructionList(instructions);
			if (addExtraElem) {
				var instruction = Instruction.Create(Code.Nopw);
				list.Add(instruction);
				Array.Resize(ref instructions, instructions.Length + 1);
				instructions[instructions.Length - 1] = instruction;
			}
			Assert.Equal(instructions.Length, list.Count);
			Assert.True(instructions.Length <= list.Capacity);
			var array = list.ToArray();
			Assert.Equal(list.Count, array.Length);
			AssertEqual(instructions, array);
		}
		public static IEnumerable<object[]> ToArray_works_Data {
			get {
				for (int i = 0; i < 2; i++) {
					bool addExtraElem = i == 1;
					yield return new object[] { Array.Empty<Instruction>(), addExtraElem };
					yield return new object[] { new Instruction[] { Instruction.Create(Code.Nopd) }, addExtraElem };
					yield return new object[] { GetInstructions(), addExtraElem };
				}
			}
		}

		[Theory]
		[MemberData(nameof(AsReadOnly_works_Data))]
		void AsReadOnly_works(Instruction[] instructions) {
			var list = new InstructionList(instructions);
			Assert.Equal(instructions.Length, list.Count);
			Assert.Equal(instructions.Length, list.Capacity);
			var roList = list.AsReadOnly();
			Assert.IsType<ReadOnlyCollection<Instruction>>(roList);
			for (int i = 0; i < list.Count; i++)
				list[i].Code++;
			var array = roList.ToArray();
			Assert.Equal(list.Count, array.Length);
			AssertEqual(list, array);
		}
		public static IEnumerable<object[]> AsReadOnly_works_Data {
			get {
				yield return new object[] { Array.Empty<Instruction>() };
				yield return new object[] { new Instruction[] { Instruction.Create(Code.Nopd) } };
				yield return new object[] { GetInstructions() };
			}
		}

		enum InsertRangeKind {
			IEnumerable,
			Array,
			IList,
			ReadOnlyList,
			InstructionList,

			Last,
		}
		[Theory]
		[MemberData(nameof(InsertRange_works_Data))]
		void InsertRange_works(int index, InstructionList list, IEnumerable<Instruction> inserted, Instruction[] expected) {
			list.InsertRange(index, inserted);
			Assert.Equal(expected.Length, list.Count);
			Assert.True(expected.Length <= list.Capacity);
			var listElems = new Instruction[list.Count];
			list.CopyTo(listElems);
			AssertEqual(expected, listElems);
		}
		public static IEnumerable<object[]> InsertRange_works_Data {
			get {
				var i = GetInstructions();
				for (InsertRangeKind kind = default; kind < InsertRangeKind.Last; kind++) {
					yield return Create(kind, 0, Array.Empty<Instruction>(), Array.Empty<Instruction>(), Array.Empty<Instruction>());
					yield return Create(kind, 0, Array.Empty<Instruction>(), new Instruction[] { i[0] }, new Instruction[] { i[0] });
					yield return Create(kind, 0, Array.Empty<Instruction>(), i, i);

					yield return Create(kind, 0, new Instruction[] { i[1] }, Array.Empty<Instruction>(), new Instruction[] { i[1] });
					yield return Create(kind, 0, new Instruction[] { i[1] }, new Instruction[] { i[3] }, new Instruction[] { i[3], i[1] });
					yield return Create(kind, 0, new Instruction[] { i[1] }, new Instruction[] { i[3], i[5], i[7] }, new Instruction[] { i[3], i[5], i[7], i[1] });

					yield return Create(kind, 1, new Instruction[] { i[1] }, Array.Empty<Instruction>(), new Instruction[] { i[1] });
					yield return Create(kind, 1, new Instruction[] { i[1] }, new Instruction[] { i[3] }, new Instruction[] { i[1], i[3] });
					yield return Create(kind, 1, new Instruction[] { i[1] }, new Instruction[] { i[3], i[5], i[7] }, new Instruction[] { i[1], i[3], i[5], i[7] });

					yield return Create(kind, 0, new Instruction[] { i[1], i[12], i[10] }, Array.Empty<Instruction>(), new Instruction[] { i[1], i[12], i[10] });
					yield return Create(kind, 0, new Instruction[] { i[1], i[12], i[10] }, new Instruction[] { i[3] }, new Instruction[] { i[3], i[1], i[12], i[10] });
					yield return Create(kind, 0, new Instruction[] { i[1], i[12], i[10] }, new Instruction[] { i[3], i[5], i[7] }, new Instruction[] { i[3], i[5], i[7], i[1], i[12], i[10] });

					yield return Create(kind, 3, new Instruction[] { i[1], i[12], i[10] }, Array.Empty<Instruction>(), new Instruction[] { i[1], i[12], i[10] });
					yield return Create(kind, 3, new Instruction[] { i[1], i[12], i[10] }, new Instruction[] { i[3] }, new Instruction[] { i[1], i[12], i[10], i[3] });
					yield return Create(kind, 3, new Instruction[] { i[1], i[12], i[10] }, new Instruction[] { i[3], i[5], i[7] }, new Instruction[] { i[1], i[12], i[10], i[3], i[5], i[7] });

					yield return Create(kind, 1, new Instruction[] { i[1], i[12], i[10] }, Array.Empty<Instruction>(), new Instruction[] { i[1], i[12], i[10] });
					yield return Create(kind, 1, new Instruction[] { i[1], i[12], i[10] }, new Instruction[] { i[3] }, new Instruction[] { i[1], i[3], i[12], i[10] });
					yield return Create(kind, 1, new Instruction[] { i[1], i[12], i[10] }, new Instruction[] { i[3], i[5], i[7] }, new Instruction[] { i[1], i[3], i[5], i[7], i[12], i[10] });
				}

				static object[] Create(InsertRangeKind kind, int index, Instruction[] instructions, Instruction[] inserted, Instruction[] expected) {
					var list = new InstructionList(instructions);
					return new object[] { index, list, Convert(kind, inserted), expected };
				}
			}
		}
		static IEnumerable<Instruction> Convert(InsertRangeKind kind, Instruction[] array) =>
			kind switch {
				InsertRangeKind.IEnumerable => AsEnumerable(array),// IEnumerable<Instruction> code path
				InsertRangeKind.Array => array,// Instruction[] code path (none at the moment)
				InsertRangeKind.IList => new IListImpl<Instruction>(array),// IList<Instruction> code path
				InsertRangeKind.ReadOnlyList => new IReadOnlyListImpl<Instruction>(array),// IReadOnlyList<Instruction> code path
				InsertRangeKind.InstructionList => new InstructionList(array),// InstructionList code path
				_ => throw new InvalidOperationException(),
			};

		static IEnumerable<T> AsEnumerable<T>(IEnumerable<T> collection) {
			// Don't return the input, always create a new enumerable
			foreach (var elem in collection)
				yield return elem;
		}

		sealed class IListImpl<T> : IList<T> {
			readonly T[] array;
			public IListImpl(T[] array) => this.array = array;
			public int Count => array.Length;
			public T this[int index] {
				get => array[index];
				set => throw new NotImplementedException();
			}
			public bool IsReadOnly => throw new NotImplementedException();
			public int IndexOf(T item) => throw new NotImplementedException();
			public void Insert(int index, T item) => throw new NotImplementedException();
			public void RemoveAt(int index) => throw new NotImplementedException();
			public void Add(T item) => throw new NotImplementedException();
			public void Clear() => throw new NotImplementedException();
			public bool Contains(T item) => throw new NotImplementedException();
			public bool Remove(T item) => throw new NotImplementedException();
			public void CopyTo(T[] array, int arrayIndex) => throw new NotImplementedException();
			public IEnumerator<T> GetEnumerator() => throw new NotImplementedException();
			IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
		}

		sealed class IReadOnlyListImpl<T> : IReadOnlyList<T> {
			readonly T[] array;
			public IReadOnlyListImpl(T[] array) => this.array = array;
			public int Count => array.Length;
			public T this[int index] => array[index];
			public IEnumerator<T> GetEnumerator() => throw new NotImplementedException();
			IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
		}

		[Theory]
		[MemberData(nameof(AddRange_works_Data))]
		void AddRange_works(InstructionList list, IEnumerable<Instruction> inserted, Instruction[] expected) {
			list.AddRange(inserted);
			Assert.Equal(expected.Length, list.Count);
			Assert.True(expected.Length <= list.Capacity);
			var listElems = new Instruction[list.Count];
			list.CopyTo(listElems);
			AssertEqual(expected, listElems);
		}
		public static IEnumerable<object[]> AddRange_works_Data {
			get {
				var i = GetInstructions();
				for (InsertRangeKind kind = default; kind < InsertRangeKind.Last; kind++) {
					yield return Create(kind, Array.Empty<Instruction>(), Array.Empty<Instruction>(), Array.Empty<Instruction>());
					yield return Create(kind, Array.Empty<Instruction>(), new Instruction[] { i[0] }, new Instruction[] { i[0] });
					yield return Create(kind, Array.Empty<Instruction>(), i, i);

					yield return Create(kind, new Instruction[] { i[1] }, Array.Empty<Instruction>(), new Instruction[] { i[1] });
					yield return Create(kind, new Instruction[] { i[1] }, new Instruction[] { i[3] }, new Instruction[] { i[1], i[3] });
					yield return Create(kind, new Instruction[] { i[1] }, new Instruction[] { i[3], i[5], i[7] }, new Instruction[] { i[1], i[3], i[5], i[7] });

					yield return Create(kind, new Instruction[] { i[1], i[12], i[10] }, Array.Empty<Instruction>(), new Instruction[] { i[1], i[12], i[10] });
					yield return Create(kind, new Instruction[] { i[1], i[12], i[10] }, new Instruction[] { i[3] }, new Instruction[] { i[1], i[12], i[10], i[3] });
					yield return Create(kind, new Instruction[] { i[1], i[12], i[10] }, new Instruction[] { i[3], i[5], i[7] }, new Instruction[] { i[1], i[12], i[10], i[3], i[5], i[7] });
				}

				static object[] Create(InsertRangeKind kind, Instruction[] instructions, Instruction[] inserted, Instruction[] expected) {
					var list = new InstructionList(instructions);
					return new object[] { list, Convert(kind, inserted), expected };
				}
			}
		}

		[Theory]
		[MemberData(nameof(RemoveRange_works_Data))]
		void RemoveRange_works(int index, int count, InstructionList list, Instruction[] expected) {
			list.RemoveRange(index, count);
			Assert.Equal(expected.Length, list.Count);
			Assert.True(expected.Length <= list.Capacity);
			var listElems = new Instruction[list.Count];
			list.CopyTo(listElems);
			AssertEqual(expected, listElems);
		}
		public static IEnumerable<object[]> RemoveRange_works_Data {
			get {
				var i = GetInstructions();

				yield return Create(0, 0, Array.Empty<Instruction>(), Array.Empty<Instruction>());

				yield return Create(0, 0, new Instruction[] { i[0] }, new Instruction[] { i[0] });
				yield return Create(0, 1, new Instruction[] { i[0] }, Array.Empty<Instruction>());

				yield return Create(0, 0, new Instruction[] { i[0], i[1], i[2], i[3], i[4] }, new Instruction[] { i[0], i[1], i[2], i[3], i[4] });
				yield return Create(0, 1, new Instruction[] { i[0], i[1], i[2], i[3], i[4] }, new Instruction[] { i[1], i[2], i[3], i[4] });
				yield return Create(0, 3, new Instruction[] { i[0], i[1], i[2], i[3], i[4] }, new Instruction[] { i[3], i[4] });
				yield return Create(0, 5, new Instruction[] { i[0], i[1], i[2], i[3], i[4] }, Array.Empty<Instruction>());

				yield return Create(2, 0, new Instruction[] { i[0], i[1], i[2], i[3], i[4] }, new Instruction[] { i[0], i[1], i[2], i[3], i[4] });
				yield return Create(2, 1, new Instruction[] { i[0], i[1], i[2], i[3], i[4] }, new Instruction[] { i[0], i[1], i[3], i[4] });
				yield return Create(2, 2, new Instruction[] { i[0], i[1], i[2], i[3], i[4] }, new Instruction[] { i[0], i[1], i[4] });
				yield return Create(2, 3, new Instruction[] { i[0], i[1], i[2], i[3], i[4] }, new Instruction[] { i[0], i[1] });

				yield return Create(4, 1, new Instruction[] { i[0], i[1], i[2], i[3], i[4] }, new Instruction[] { i[0], i[1], i[2], i[3] });

				yield return Create(5, 0, new Instruction[] { i[0], i[1], i[2], i[3], i[4] }, new Instruction[] { i[0], i[1], i[2], i[3], i[4] });

				static object[] Create(int index, int count, Instruction[] instructions, Instruction[] expected) {
					var list = new InstructionList(instructions);
					return new object[] { index, count, list, expected };
				}
			}
		}

		[Fact]
		void InsertRange_throws_if_invalid_input() {
			var instructions = GetInstructions();
			InstructionList list;

			list = new InstructionList(Array.Empty<Instruction>());
			Assert.Throws<ArgumentOutOfRangeException>(() => list.InsertRange(GetValue(-1), instructions));
			Assert.Throws<ArgumentOutOfRangeException>(() => list.InsertRange(GetValue(1), instructions));
			Assert.Throws<ArgumentNullException>(() => list.InsertRange(GetValue(0), null));

			list = new InstructionList(instructions);
			Assert.Throws<ArgumentOutOfRangeException>(() => list.InsertRange(GetValue(-1), instructions));
			Assert.Throws<ArgumentOutOfRangeException>(() => list.InsertRange(GetValue(int.MinValue), instructions));
			Assert.Throws<ArgumentOutOfRangeException>(() => list.InsertRange(GetValue(int.MaxValue), instructions));
			Assert.Throws<ArgumentOutOfRangeException>(() => list.InsertRange(GetValue(list.Count + 1), instructions));
			Assert.Throws<ArgumentNullException>(() => list.InsertRange(GetValue(0), null));
		}

		[Fact]
		void AddRange_throws_if_invalid_input() {
			var list = new InstructionList(Array.Empty<Instruction>());
			Assert.Throws<ArgumentNullException>(() => list.AddRange(null));

			list = new InstructionList(GetInstructions());
			Assert.Throws<ArgumentNullException>(() => list.AddRange(null));
		}

		[Fact]
		void RemoveRange_throws_if_invalid_input() {
			var instructions = GetInstructions();
			InstructionList list;

			list = new InstructionList(Array.Empty<Instruction>());
			Assert.Throws<ArgumentOutOfRangeException>(() => list.RemoveRange(GetValue(-1), GetValue(0)));
			Assert.Throws<ArgumentOutOfRangeException>(() => list.RemoveRange(GetValue(0), GetValue(1)));
			Assert.Throws<ArgumentOutOfRangeException>(() => list.RemoveRange(GetValue(1), GetValue(0)));
			Assert.Throws<ArgumentOutOfRangeException>(() => list.RemoveRange(GetValue(-1), GetValue(1)));

			list = new InstructionList(instructions);
			Assert.Throws<ArgumentOutOfRangeException>(() => list.RemoveRange(GetValue(-1), 0));
			Assert.Throws<ArgumentOutOfRangeException>(() => list.RemoveRange(GetValue(1), GetValue(list.Count)));
			Assert.Throws<ArgumentOutOfRangeException>(() => list.RemoveRange(GetValue(0), GetValue(list.Count + 1)));
			Assert.Throws<ArgumentOutOfRangeException>(() => list.RemoveRange(GetValue(int.MinValue), GetValue(0)));
			Assert.Throws<ArgumentOutOfRangeException>(() => list.RemoveRange(GetValue(0), GetValue(int.MaxValue)));
			Assert.Throws<ArgumentOutOfRangeException>(() => list.RemoveRange(GetValue(int.MaxValue), GetValue(int.MaxValue)));
			Assert.Throws<ArgumentOutOfRangeException>(() => list.RemoveRange(GetValue(int.MinValue), GetValue(int.MaxValue)));
		}

		[Fact]
		void GetRange_empty() {
			var list = new InstructionList();
			var newList = list.GetRange(0, 0);
			var expected = Array.Empty<Instruction>();
			Array.Equals(expected.Length, newList.Count);
			AssertEqual(expected, newList);
		}

		[Fact]
		void GetRange_1() {
			var i = GetInstructions();
			var expected = new Instruction[] {
				i[2],
			};
			var list = new InstructionList(i);
			var newList = list.GetRange(2, 1);
			Array.Equals(expected.Length, newList.Count);
			AssertEqual(expected, newList);
		}

		[Fact]
		void GetRange_2() {
			var i = GetInstructions();
			var expected = new Instruction[] {
				i[5],
				i[6],
				i[7],
				i[8],
			};
			var list = new InstructionList(i);
			var newList = list.GetRange(5, 4);
			Array.Equals(expected.Length, newList.Count);
			AssertEqual(expected, newList);
		}

		[Fact]
		void GetRange_3() {
			var i = GetInstructions();
			var expected = new Instruction[] {
				i[0],
				i[1],
			};
			var list = new InstructionList(i);
			var newList = list.GetRange(0, 2);
			Array.Equals(expected.Length, newList.Count);
			AssertEqual(expected, newList);
		}

		[Fact]
		void GetRange_4() {
			var i = GetInstructions();
			int end = i.Length;
			var expected = new Instruction[] {
				i[end - 5],
				i[end - 4],
				i[end - 3],
				i[end - 2],
				i[end - 1],
			};
			var list = new InstructionList(i);
			var newList = list.GetRange(end - 5, 5);
			Array.Equals(expected.Length, newList.Count);
			AssertEqual(expected, newList);
		}

		[Fact]
		void CopyTo_works_1() {
			var i = GetInstructions();
			var list = new InstructionList(i);
			var array = new Instruction[5];
			array[0] = Instruction.Create(Code.Nopd);
			array[4] = Instruction.Create(Code.Nopq);
			var expected = new Instruction[5] {
				Instruction.Create(Code.Nopd),
				i[2],
				i[3],
				i[4],
				Instruction.Create(Code.Nopq),
			};
			list.CopyTo(2, array, 1, 3);
			Assert.Equal(expected, array);
		}

		[Fact]
		void CopyTo_works_2() {
			var i = GetInstructions();
			var list = new InstructionList(i);
			var array = new Instruction[i.Length];
			list.CopyTo(0, array, 0, array.Length);
			Assert.Equal(i, array);
		}

		[Theory]
		[MemberData(nameof(Contains_works_Data))]
		void Contains_works(Instruction[] data, Instruction instruction, bool expected) {
			var list = new InstructionList(data);
			var result = list.Contains(instruction);
			Assert.Equal(expected, result);
		}
		public static IEnumerable<object[]> Contains_works_Data {
			get {
				var i = GetInstructions();

				yield return new object[] { Array.Empty<Instruction>(), i[0], false };
				yield return new object[] { new Instruction[] { i[1] }, i[0], false };
				yield return new object[] { new Instruction[] { i[0] }, i[0], true };
				yield return new object[] { new Instruction[] { i[0], i[1] }, i[0], true };
				yield return new object[] { new Instruction[] { i[1], i[0] }, i[0], true };
				yield return new object[] { new Instruction[] { i[1], i[0], i[3] }, i[0], true };
			}
		}

		[Theory]
		[MemberData(nameof(IndexOf1_works_Data))]
		void IndexOf1_works(Instruction[] data, Instruction instruction, int expected) {
			var list = new InstructionList(data);
			var result = list.IndexOf(instruction);
			Assert.Equal(expected, result);
		}
		public static IEnumerable<object[]> IndexOf1_works_Data {
			get {
				var i = GetInstructions();

				yield return new object[] { Array.Empty<Instruction>(), i[0], -1 };
				yield return new object[] { new Instruction[] { i[1] }, i[0], -1 };
				yield return new object[] { new Instruction[] { i[0] }, i[0], 0 };
				yield return new object[] { new Instruction[] { i[0], i[1] }, i[0], 0 };
				yield return new object[] { new Instruction[] { i[1], i[0] }, i[0], 1 };
				yield return new object[] { new Instruction[] { i[1], i[0], i[3] }, i[0], 1 };
				yield return new object[] { new Instruction[] { i[1], i[4], i[0], i[3], i[0] }, i[0], 2 };
			}
		}

		[Theory]
		[MemberData(nameof(IndexOf2_works_Data))]
		void IndexOf2_works(Instruction[] data, Instruction instruction, int index, int expected) {
			var list = new InstructionList(data);
			var result = list.IndexOf(instruction, index);
			Assert.Equal(expected, result);
		}
		public static IEnumerable<object[]> IndexOf2_works_Data {
			get {
				var i = GetInstructions();

				yield return new object[] { Array.Empty<Instruction>(), i[0], 0, -1 };
				yield return new object[] { new Instruction[] { i[1] }, i[0], 0, -1 };
				yield return new object[] { new Instruction[] { i[0] }, i[0], 0, 0 };
				yield return new object[] { new Instruction[] { i[0] }, i[0], 1, -1 };
				yield return new object[] { new Instruction[] { i[0], i[1] }, i[0], 0, 0 };
				yield return new object[] { new Instruction[] { i[0], i[1] }, i[0], 1, -1 };
				yield return new object[] { new Instruction[] { i[1], i[0] }, i[0], 0, 1 };
				yield return new object[] { new Instruction[] { i[1], i[0] }, i[0], 1, 1 };
				yield return new object[] { new Instruction[] { i[1], i[0] }, i[0], 2, -1 };
				yield return new object[] { new Instruction[] { i[1], i[0], i[3] }, i[0], 0, 1 };
				yield return new object[] { new Instruction[] { i[1], i[0], i[3] }, i[0], 1, 1 };
				yield return new object[] { new Instruction[] { i[1], i[0], i[3] }, i[0], 2, -1 };
				yield return new object[] { new Instruction[] { i[1], i[4], i[0], i[3], i[0] }, i[0], 0, 2 };
				yield return new object[] { new Instruction[] { i[1], i[4], i[0], i[3], i[0] }, i[0], 2, 2 };
				yield return new object[] { new Instruction[] { i[1], i[4], i[0], i[3], i[0] }, i[0], 3, 4 };
				yield return new object[] { new Instruction[] { i[1], i[4], i[0], i[3], i[0] }, i[0], 4, 4 };
				yield return new object[] { new Instruction[] { i[1], i[4], i[0], i[3], i[0] }, i[0], 5, -1 };
			}
		}

		[Theory]
		[MemberData(nameof(IndexOf3_works_Data))]
		void IndexOf3_works(Instruction[] data, Instruction instruction, int index, int count, int expected) {
			var list = new InstructionList(data);
			var result = list.IndexOf(instruction, index, count);
			Assert.Equal(expected, result);
		}
		public static IEnumerable<object[]> IndexOf3_works_Data {
			get {
				var i = GetInstructions();

				yield return new object[] { Array.Empty<Instruction>(), i[0], 0, 0, -1 };
				yield return new object[] { new Instruction[] { i[1] }, i[0], 0, 1, -1 };
				yield return new object[] { new Instruction[] { i[0] }, i[0], 0, 1, 0 };
				yield return new object[] { new Instruction[] { i[0] }, i[0], 0, 0, -1 };
				yield return new object[] { new Instruction[] { i[0] }, i[0], 1, 0, -1 };
				yield return new object[] { new Instruction[] { i[0], i[1] }, i[0], 0, 1, 0 };
				yield return new object[] { new Instruction[] { i[0], i[1] }, i[0], 1, 1, -1 };
				yield return new object[] { new Instruction[] { i[1], i[0] }, i[0], 0, 2, 1 };
				yield return new object[] { new Instruction[] { i[1], i[0] }, i[0], 0, 1, -1 };
				yield return new object[] { new Instruction[] { i[1], i[0] }, i[0], 1, 1, 1 };
				yield return new object[] { new Instruction[] { i[1], i[0] }, i[0], 0, 1, -1 };
				yield return new object[] { new Instruction[] { i[1], i[0] }, i[0], 2, 0, -1 };
				yield return new object[] { new Instruction[] { i[1], i[0], i[3] }, i[0], 0, 2, 1 };
				yield return new object[] { new Instruction[] { i[1], i[0], i[3] }, i[0], 1, 1, 1 };
				yield return new object[] { new Instruction[] { i[1], i[0], i[3] }, i[0], 2, 1, -1 };
				yield return new object[] { new Instruction[] { i[1], i[4], i[0], i[3], i[0] }, i[0], 0, 5, 2 };
				yield return new object[] { new Instruction[] { i[1], i[4], i[0], i[3], i[0] }, i[0], 0, 3, 2 };
				yield return new object[] { new Instruction[] { i[1], i[4], i[0], i[3], i[0] }, i[0], 0, 2, -1 };
				yield return new object[] { new Instruction[] { i[1], i[4], i[0], i[3], i[0] }, i[0], 2, 3, 2 };
				yield return new object[] { new Instruction[] { i[1], i[4], i[0], i[3], i[0] }, i[0], 3, 2, 4 };
				yield return new object[] { new Instruction[] { i[1], i[4], i[0], i[3], i[0] }, i[0], 3, 1, -1 };
				yield return new object[] { new Instruction[] { i[1], i[4], i[0], i[3], i[0] }, i[0], 4, 1, 4 };
				yield return new object[] { new Instruction[] { i[1], i[4], i[0], i[3], i[0] }, i[0], 5, 0, -1 };
			}
		}

		[Theory]
		[MemberData(nameof(LastIndexOf1_works_Data))]
		void LastIndexOf1_works(Instruction[] data, Instruction instruction, int expected) {
			var list = new InstructionList(data);
			var result = list.LastIndexOf(instruction);
			Assert.Equal(expected, result);
		}
		public static IEnumerable<object[]> LastIndexOf1_works_Data {
			get {
				var i = GetInstructions();

				yield return new object[] { Array.Empty<Instruction>(), i[0], -1 };
				yield return new object[] { new Instruction[] { i[1] }, i[0], -1 };
				yield return new object[] { new Instruction[] { i[0] }, i[0], 0 };
				yield return new object[] { new Instruction[] { i[0], i[1] }, i[0], 0 };
				yield return new object[] { new Instruction[] { i[1], i[0] }, i[0], 1 };
				yield return new object[] { new Instruction[] { i[1], i[0], i[3] }, i[0], 1 };
				yield return new object[] { new Instruction[] { i[1], i[4], i[0], i[3], i[0] }, i[0], 4 };
			}
		}

		[Theory]
		[MemberData(nameof(LastIndexOf2_works_Data))]
		void LastIndexOf2_works(Instruction[] data, Instruction instruction, int index, int expected) {
			var list = new InstructionList(data);
			var result = list.LastIndexOf(instruction, index);
			Assert.Equal(expected, result);
		}
		public static IEnumerable<object[]> LastIndexOf2_works_Data {
			get {
				var i = GetInstructions();

				yield return new object[] { Array.Empty<Instruction>(), i[0], 0, -1 };
				yield return new object[] { new Instruction[] { i[1] }, i[0], 0, -1 };
				yield return new object[] { new Instruction[] { i[1] }, i[0], 1, -1 };
				yield return new object[] { new Instruction[] { i[0] }, i[0], 0, 0 };
				yield return new object[] { new Instruction[] { i[0] }, i[0], 1, -1 };
				yield return new object[] { new Instruction[] { i[0], i[1] }, i[0], 0, 0 };
				yield return new object[] { new Instruction[] { i[0], i[1] }, i[0], 1, -1 };
				yield return new object[] { new Instruction[] { i[1], i[0] }, i[0], 0, 1 };
				yield return new object[] { new Instruction[] { i[1], i[0] }, i[0], 1, 1 };
				yield return new object[] { new Instruction[] { i[1], i[0] }, i[0], 2, -1 };
				yield return new object[] { new Instruction[] { i[1], i[0], i[3] }, i[0], 0, 1 };
				yield return new object[] { new Instruction[] { i[1], i[0], i[3] }, i[0], 1, 1 };
				yield return new object[] { new Instruction[] { i[1], i[0], i[3] }, i[0], 2, -1 };
				yield return new object[] { new Instruction[] { i[1], i[4], i[0], i[3], i[0] }, i[0], 0, 4 };
				yield return new object[] { new Instruction[] { i[1], i[4], i[0], i[3], i[0] }, i[0], 1, 4 };
				yield return new object[] { new Instruction[] { i[1], i[4], i[0], i[3], i[0] }, i[0], 2, 4 };
				yield return new object[] { new Instruction[] { i[1], i[4], i[0], i[3], i[0] }, i[0], 3, 4 };
				yield return new object[] { new Instruction[] { i[1], i[4], i[0], i[3], i[0] }, i[0], 4, 4 };
				yield return new object[] { new Instruction[] { i[1], i[4], i[0], i[3], i[0] }, i[0], 5, -1 };
			}
		}

		[Theory]
		[MemberData(nameof(LastIndexOf3_works_Data))]
		void LastIndexOf3_works(Instruction[] data, Instruction instruction, int index, int count, int expected) {
			var list = new InstructionList(data);
			var result = list.LastIndexOf(instruction, index, count);
			Assert.Equal(expected, result);
		}
		public static IEnumerable<object[]> LastIndexOf3_works_Data {
			get {
				var i = GetInstructions();

				yield return new object[] { Array.Empty<Instruction>(), i[0], 0, 0, -1 };
				yield return new object[] { new Instruction[] { i[1] }, i[0], 0, 1, -1 };
				yield return new object[] { new Instruction[] { i[1] }, i[0], 1, 0, -1 };
				yield return new object[] { new Instruction[] { i[0] }, i[0], 0, 1, 0 };
				yield return new object[] { new Instruction[] { i[0] }, i[0], 0, 0, -1 };
				yield return new object[] { new Instruction[] { i[0] }, i[0], 1, 0, -1 };
				yield return new object[] { new Instruction[] { i[0], i[1] }, i[0], 0, 1, 0 };
				yield return new object[] { new Instruction[] { i[0], i[1] }, i[0], 1, 1, -1 };
				yield return new object[] { new Instruction[] { i[1], i[0] }, i[0], 0, 1, -1 };
				yield return new object[] { new Instruction[] { i[1], i[0] }, i[0], 0, 2, 1 };
				yield return new object[] { new Instruction[] { i[1], i[0] }, i[0], 1, 1, 1 };
				yield return new object[] { new Instruction[] { i[1], i[0] }, i[0], 2, 0, -1 };
				yield return new object[] { new Instruction[] { i[1], i[0], i[3] }, i[0], 0, 2, 1 };
				yield return new object[] { new Instruction[] { i[1], i[0], i[3] }, i[0], 0, 1, -1 };
				yield return new object[] { new Instruction[] { i[1], i[0], i[3] }, i[0], 2, 1, -1 };
				yield return new object[] { new Instruction[] { i[1], i[0], i[3] }, i[0], 1, 1, 1 };
				yield return new object[] { new Instruction[] { i[1], i[0], i[3] }, i[0], 2, 1, -1 };
				yield return new object[] { new Instruction[] { i[1], i[4], i[0], i[3], i[0] }, i[0], 0, 2, -1 };
				yield return new object[] { new Instruction[] { i[1], i[4], i[0], i[3], i[0] }, i[0], 1, 1, -1 };
				yield return new object[] { new Instruction[] { i[1], i[4], i[0], i[3], i[0] }, i[0], 3, 1, -1 };
				yield return new object[] { new Instruction[] { i[1], i[4], i[0], i[3], i[0] }, i[0], 5, 0, -1 };
				yield return new object[] { new Instruction[] { i[1], i[4], i[0], i[3], i[0] }, i[0], 0, 5, 4 };
				yield return new object[] { new Instruction[] { i[1], i[4], i[0], i[3], i[0] }, i[0], 0, 4, 2 };
			}
		}

		[Theory]
		[MemberData(nameof(Remove_works_Data))]
		void Remove_works(Instruction[] data, Instruction instruction, bool expected, Instruction[] expectedData) {
			var list = new InstructionList(data);
			var result = list.Remove(instruction);
			Assert.Equal(expected, result);
			var listElems = new Instruction[list.Count];
			list.CopyTo(listElems);
			AssertEqual(expectedData, listElems);
		}
		public static IEnumerable<object[]> Remove_works_Data {
			get {
				var i = GetInstructions();

				yield return new object[] { Array.Empty<Instruction>(), i[0], false, Array.Empty<Instruction>() };
				yield return new object[] { new Instruction[] { i[0] }, i[1], false, new Instruction[] { i[0] } };
				yield return new object[] { new Instruction[] { i[0] }, i[0], true, Array.Empty<Instruction>() };
				yield return new object[] { new Instruction[] { i[0], i[1], i[2], i[0], i[4], i[5], i[0] }, i[0], true, new Instruction[] { i[1], i[2], i[0], i[4], i[5], i[0] } };
			}
		}
	}
}
#endif
