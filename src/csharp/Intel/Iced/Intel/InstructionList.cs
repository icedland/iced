// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Iced.Intel {
	/// <summary>
	/// A list of <see cref="Instruction"/>s. It's faster than <see cref="List{T}"/> and has
	/// methods to get references to the elements so no <see cref="Instruction"/> gets copied.
	/// Use 'foreach (ref var instr in list)' to use the foreach ref iterator.
	/// </summary>
	[DebuggerDisplay("Count = {" + nameof(Count) + "}")]
	[DebuggerTypeProxy(typeof(InstructionListDebugView))]
	[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
	public sealed class InstructionList : IList<Instruction>, IReadOnlyList<Instruction>, IList {
		Instruction[] elements;
		int count;

		/// <summary>
		/// Gets the number of valid elements
		/// </summary>
		public int Count => count;
		int ICollection<Instruction>.Count => count;
		int ICollection.Count => count;
		int IReadOnlyCollection<Instruction>.Count => count;

		/// <summary>
		/// Gets the size of the internal array
		/// </summary>
		public int Capacity => elements.Length;

		bool ICollection<Instruction>.IsReadOnly => false;
		bool IList.IsReadOnly => false;

		bool IList.IsFixedSize => false;
		bool ICollection.IsSynchronized => false;
		object ICollection.SyncRoot => this;

		/// <summary>
		/// Gets a reference to an element. The returned reference is valid until the internal array is resized.
		/// </summary>
		/// <param name="index">Index of element</param>
		/// <returns></returns>
		public ref Instruction this[int index] =>
			// Let the jitter validate the index. Note that this also allows code to accidentally
			// get a reference to an unused element if Count < Capacity; this code won't throw.
			// It's acceptable, this list is used for PERF, use List<Instruction> to throw.
			ref elements[index];
		Instruction IList<Instruction>.this[int index] {
			get => elements[index];
			set => elements[index] = value;
		}
		Instruction IReadOnlyList<Instruction>.this[int index] => elements[index];
		object? IList.this[int index] {
			get => elements[index];
			set {
				if (value is null)
					ThrowHelper.ThrowArgumentNullException_value();
				if (!(value is Instruction))
					ThrowHelper.ThrowArgumentException();
				elements[index] = (Instruction)value;
			}
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public InstructionList() => elements = Array2.Empty<Instruction>();

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="capacity">Initial size of the internal array</param>
		public InstructionList(int capacity) {
			if (capacity < 0)
				ThrowHelper.ThrowArgumentOutOfRangeException_capacity();
			elements = capacity == 0 ? Array2.Empty<Instruction>() : new Instruction[capacity];
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="list">List that will be copied to this instance</param>
		public InstructionList(InstructionList list) {
			if (list is null)
				ThrowHelper.ThrowArgumentNullException_list();
			int length = list.count;
			if (length == 0)
				elements = Array2.Empty<Instruction>();
			else {
				var elements = new Instruction[length];
				this.elements = elements;
				count = length;
				Array.Copy(list.elements, 0, elements, 0, length);
			}
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="collection">Collection that will be copied to this instance</param>
		public InstructionList(IEnumerable<Instruction> collection) {
			if (collection is null)
				ThrowHelper.ThrowArgumentNullException_collection();
			if (collection is ICollection<Instruction> coll) {
				int count = coll.Count;
				if (count == 0)
					elements = Array2.Empty<Instruction>();
				else {
					var elements = new Instruction[count];
					this.elements = elements;
					coll.CopyTo(elements, 0);
					this.count = count;
				}
			}
			else {
				elements = Array2.Empty<Instruction>();
				foreach (var elem in collection)
					Add(elem);
			}
		}

		void SetMinCapacity(int minCapacity) {
			var elements = this.elements;
			uint capacity = (uint)elements.Length;
			if (minCapacity <= (int)capacity)
				return;
			const uint DEFAULT_SIZE = 4;// Same as List<Instruction>
			uint newCapacity = capacity * 2;
			if (newCapacity < DEFAULT_SIZE)
				newCapacity = DEFAULT_SIZE;
			if (newCapacity < (uint)minCapacity)
				newCapacity = (uint)minCapacity;
			// See coreclr/vm/gchelpers.cpp:MaxArrayLength()
			const uint MaxArrayLength = 0x7FEFFFFF;
			Debug.Assert(MaxArrayLength <= int.MaxValue);
			if (newCapacity > MaxArrayLength)
				newCapacity = MaxArrayLength;
			var newElements = new Instruction[(int)newCapacity];
			Array.Copy(elements, 0, newElements, 0, count);
			this.elements = newElements;
		}

		/// <summary>
		/// Allocates an uninitialized element at the end of the list and returns a reference to it.
		/// The return value can be passed to eg. Decoder.Decode(out Instruction).
		/// The returned reference is valid until the internal array is resized.
		/// </summary>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]// Add() is inlined, and this method does almost the same thing
		public ref Instruction AllocUninitializedElement() {
			var count = this.count;
			var elements = this.elements;
			if (count == elements.Length) {
				SetMinCapacity(count + 1);
				elements = this.elements;
			}
			this.count = count + 1;
			return ref elements[count];
		}

		void MakeRoom(int index, int extraLength) {
			//TODO: This can be optimized to copy less data. The current code can copy the same
			//      data twice if the internal array is resized by SetMinCapacity()
			SetMinCapacity(count + extraLength);
			int copyCount = count - index;
			if (copyCount != 0) {
				var elements = this.elements;
				Array.Copy(elements, index, elements, index + extraLength, copyCount);
			}
		}

		/// <summary>
		/// Inserts an element
		/// </summary>
		/// <param name="index">Index of element</param>
		/// <param name="instruction">Instruction to add</param>
		public void Insert(int index, in Instruction instruction) {
			var count = this.count;
			if ((uint)index > (uint)count)
				ThrowHelper.ThrowArgumentOutOfRangeException_index();
			MakeRoom(index, 1);
			elements[index] = instruction;
			this.count = count + 1;
		}
		void IList<Instruction>.Insert(int index, Instruction instruction) => Insert(index, instruction);
		void IList.Insert(int index, object? value) {
			if (value is null)
				ThrowHelper.ThrowArgumentNullException_value();
			if (!(value is Instruction))
				ThrowHelper.ThrowArgumentException();
			Insert(index, (Instruction)value);
		}

		/// <summary>
		/// Removes an element from the list
		/// </summary>
		/// <param name="index">Index of element to remove</param>
		public void RemoveAt(int index) {
			var newCount = count;
			if ((uint)index >= (uint)newCount)
				ThrowHelper.ThrowArgumentOutOfRangeException_index();
			newCount--;
			count = newCount;
			int copyCount = newCount - index;
			if (copyCount != 0) {
				var elements = this.elements;
				Array.Copy(elements, index + 1, elements, index, copyCount);
			}
		}
		void IList<Instruction>.RemoveAt(int index) => RemoveAt(index);
		void IList.RemoveAt(int index) => RemoveAt(index);

		/// <summary>
		/// Adds a collection to the end of this list
		/// </summary>
		/// <param name="collection">Collection to add</param>
		public void AddRange(IEnumerable<Instruction> collection) => InsertRange(count, collection);

		/// <summary>
		/// Inserts elements
		/// </summary>
		/// <param name="index">Index of element</param>
		/// <param name="collection">Items to insert</param>
		public void InsertRange(int index, IEnumerable<Instruction> collection) {
			if ((uint)index > (uint)count)
				ThrowHelper.ThrowArgumentOutOfRangeException_index();
			if (collection is null)
				ThrowHelper.ThrowArgumentNullException_collection();
			if (collection is InstructionList list) {
				int list_count = list.count;
				if (list_count != 0) {
					MakeRoom(index, list_count);
					count += list_count;
					Array.Copy(list.elements, 0, elements, index, list_count);
				}
			}
			else if (collection is IList<Instruction> ilist) {
				int ilist_Count = ilist.Count;
				if (ilist_Count != 0) {
					MakeRoom(index, ilist_Count);
					count += ilist_Count;
					var elements = this.elements;
					for (int i = 0; i < ilist_Count; i++)
						elements[index + i] = ilist[i];
				}
			}
			else if (collection is IReadOnlyList<Instruction> roList) {
				int roList_Count = roList.Count;
				if (roList_Count != 0) {
					MakeRoom(index, roList_Count);
					count += roList_Count;
					var elements = this.elements;
					for (int i = 0; i < roList_Count; i++)
						elements[index + i] = roList[i];
				}
			}
			else {
				foreach (var instruction in collection)
					Insert(index++, instruction);
			}
		}

		/// <summary>
		/// Removes elements
		/// </summary>
		/// <param name="index">Index of element</param>
		/// <param name="count">Number of elements to remove</param>
		public void RemoveRange(int index, int count) {
			if (index < 0)
				ThrowHelper.ThrowArgumentOutOfRangeException_index();
			if (count < 0)
				ThrowHelper.ThrowArgumentOutOfRangeException_count();
			if ((uint)index + (uint)count > (uint)this.count)
				ThrowHelper.ThrowArgumentOutOfRangeException_count();
			var newCount = this.count;
			newCount -= count;
			this.count = newCount;
			int copyCount = newCount - index;
			if (copyCount != 0) {
				var elements = this.elements;
				Array.Copy(elements, index + count, elements, index, copyCount);
			}
		}

		/// <summary>
		/// Adds a new instruction to the end of the list
		/// </summary>
		/// <param name="instruction">Instruction to add</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]// Needed since it's not inlined otherwise (because of 'in') (List<Instruction>.Add() gets auto-inlined)
		public void Add(in Instruction instruction) {
			var count = this.count;
			var elements = this.elements;
			if (count == elements.Length) {
				SetMinCapacity(count + 1);
				elements = this.elements;
			}
			elements[count] = instruction;
			this.count = count + 1;
		}
		void ICollection<Instruction>.Add(Instruction instruction) => Add(instruction);
		int IList.Add(object? value) {
			if (value is null)
				ThrowHelper.ThrowArgumentNullException_value();
			if (!(value is Instruction))
				ThrowHelper.ThrowArgumentException();
			Add((Instruction)value);
			return count - 1;
		}

		/// <summary>
		/// Clears the list
		/// </summary>
		public void Clear() => count = 0;// There are no GC refs in Instruction, so we don't have to clear the elements
		void ICollection<Instruction>.Clear() => Clear();
		void IList.Clear() => Clear();

		/// <summary>
		/// Checks if <paramref name="instruction"/> exists in the list
		/// </summary>
		/// <param name="instruction">Instruction</param>
		/// <returns></returns>
		public bool Contains(in Instruction instruction) => IndexOf(instruction) >= 0;
		bool ICollection<Instruction>.Contains(Instruction instruction) => Contains(instruction);
		bool IList.Contains(object? value) {
			if (value is Instruction instruction)
				return Contains(instruction);
			return false;
		}

		/// <summary>
		/// Gets the index of <paramref name="instruction"/> or -1 if it doesn't exist in the list
		/// </summary>
		/// <param name="instruction">Instruction</param>
		/// <returns></returns>
		public int IndexOf(in Instruction instruction) {
			var elements = this.elements;
			int count = this.count;
			for (int i = 0; i < count; i++) {
				if (elements[i] == instruction)
					return i;
			}
			return -1;
		}
		int IList<Instruction>.IndexOf(Instruction instruction) => IndexOf(instruction);
		int IList.IndexOf(object? value) {
			if (value is Instruction instruction)
				return IndexOf(instruction);
			return -1;
		}

		/// <summary>
		/// Gets the index of <paramref name="instruction"/> or -1 if it doesn't exist in the list
		/// </summary>
		/// <param name="instruction">Instruction</param>
		/// <param name="index">Start index</param>
		/// <returns></returns>
		public int IndexOf(in Instruction instruction, int index) {
			int count = this.count;
			if ((uint)index > (uint)count)
				ThrowHelper.ThrowArgumentOutOfRangeException_index();
			var elements = this.elements;
			for (int i = index; i < count; i++) {
				if (elements[i] == instruction)
					return i;
			}
			return -1;
		}

		/// <summary>
		/// Gets the index of <paramref name="instruction"/> or -1 if it doesn't exist in the list
		/// </summary>
		/// <param name="instruction">Instruction</param>
		/// <param name="index">Start index</param>
		/// <param name="count">Number of instructions to check</param>
		/// <returns></returns>
		public int IndexOf(in Instruction instruction, int index, int count) {
			if (index < 0)
				ThrowHelper.ThrowArgumentOutOfRangeException_index();
			if (count < 0)
				ThrowHelper.ThrowArgumentOutOfRangeException_count();
			int end = index + count;
			if ((uint)end > (uint)this.count)
				ThrowHelper.ThrowArgumentOutOfRangeException_count();
			var elements = this.elements;
			for (int i = index; i < end; i++) {
				if (elements[i] == instruction)
					return i;
			}
			return -1;
		}

		/// <summary>
		/// Gets the last index of <paramref name="instruction"/> or -1 if it doesn't exist in the list
		/// </summary>
		/// <param name="instruction">Instruction</param>
		/// <returns></returns>
		public int LastIndexOf(in Instruction instruction) {
			for (int i = count - 1; i >= 0; i--) {
				if (elements[i] == instruction)
					return i;
			}
			return -1;
		}

		/// <summary>
		/// Gets the last index of <paramref name="instruction"/> or -1 if it doesn't exist in the list
		/// </summary>
		/// <param name="instruction">Instruction</param>
		/// <param name="index">Start index</param>
		/// <returns></returns>
		public int LastIndexOf(in Instruction instruction, int index) {
			int count = this.count;
			if ((uint)index > (uint)count)
				ThrowHelper.ThrowArgumentOutOfRangeException_index();
			var elements = this.elements;
			for (int i = count - 1; i >= index; i--) {
				if (elements[i] == instruction)
					return i;
			}
			return -1;
		}

		/// <summary>
		/// Gets the last index of <paramref name="instruction"/> or -1 if it doesn't exist in the list
		/// </summary>
		/// <param name="instruction">Instruction</param>
		/// <param name="index">Start index</param>
		/// <param name="count">Number of instructions to check</param>
		/// <returns></returns>
		public int LastIndexOf(in Instruction instruction, int index, int count) {
			if (index < 0)
				ThrowHelper.ThrowArgumentOutOfRangeException_index();
			if (count < 0)
				ThrowHelper.ThrowArgumentOutOfRangeException_count();
			int end = index + count;
			if ((uint)end > (uint)this.count)
				ThrowHelper.ThrowArgumentOutOfRangeException_count();
			var elements = this.elements;
			for (int i = end - 1; i >= index; i--) {
				if (elements[i] == instruction)
					return i;
			}
			return -1;
		}

		/// <summary>
		/// Removes the first copy of <paramref name="instruction"/> and returns <see langword="true"/> if it was removed
		/// </summary>
		/// <param name="instruction">Instruction</param>
		/// <returns></returns>
		public bool Remove(in Instruction instruction) {
			int index = IndexOf(instruction);
			if (index >= 0)
				RemoveAt(index);
			return index >= 0;
		}
		bool ICollection<Instruction>.Remove(Instruction instruction) => Remove(instruction);
		void IList.Remove(object? value) {
			if (value is Instruction instruction)
				Remove(instruction);
		}

		/// <summary>
		/// Copies this collection to <paramref name="array"/>
		/// </summary>
		/// <param name="array">Destination array</param>
		public void CopyTo(Instruction[] array) => CopyTo(array, 0);

		/// <summary>
		/// Copies this collection to <paramref name="array"/>
		/// </summary>
		/// <param name="array">Destination array</param>
		/// <param name="arrayIndex">Start index in <paramref name="array"/></param>
		public void CopyTo(Instruction[] array, int arrayIndex) =>
			Array.Copy(elements, 0, array, arrayIndex, count);
		void ICollection<Instruction>.CopyTo(Instruction[] array, int arrayIndex) => CopyTo(array, arrayIndex);
		void ICollection.CopyTo(Array array, int index) {
			if (array is null)
				ThrowHelper.ThrowArgumentNullException_array();
			else if (array is Instruction[] elemArray)
				CopyTo(elemArray, index);
			else
				ThrowHelper.ThrowArgumentException();
		}

		/// <summary>
		/// Copies this collection to <paramref name="array"/>
		/// </summary>
		/// <param name="index">Index in this collection</param>
		/// <param name="array">Destination array</param>
		/// <param name="arrayIndex">Destination index</param>
		/// <param name="count">Number of elements to copy</param>
		public void CopyTo(int index, Instruction[] array, int arrayIndex, int count) =>
			Array.Copy(elements, index, array, arrayIndex, count);

		/// <summary>
		/// Creates a new list that contains some of the instructions in this list
		/// </summary>
		/// <param name="index">Index of first instruction</param>
		/// <param name="count">Number of instructions</param>
		/// <returns></returns>
		public InstructionList GetRange(int index, int count) {
			if (index < 0)
				ThrowHelper.ThrowArgumentOutOfRangeException_index();
			if (count < 0)
				ThrowHelper.ThrowArgumentOutOfRangeException_count();
			if ((uint)index + (uint)count > (uint)this.count)
				ThrowHelper.ThrowArgumentOutOfRangeException_count();
			var list = new InstructionList(count);
			Array.Copy(elements, index, list.elements, 0, count);
			list.count = count;
			return list;
		}

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
		public struct Enumerator : IEnumerator<Instruction> {
			readonly InstructionList list;
			int index;

			public ref Instruction Current => ref list.elements[index];
			Instruction IEnumerator<Instruction>.Current => list.elements[index];
			object IEnumerator.Current => list.elements[index];

			internal Enumerator(InstructionList list) {
				// Only two fields, the jitter can put both fields in two registers and
				// won't allocate anything on the stack
				this.list = list;
				index = -1;
			}

			public bool MoveNext() {
				// Keep both statements, the jitter generates better code if it looks like this.
				// Both fields should already be in registers.
				index++;
				return index < list.count;
			}

			void IEnumerator.Reset() => throw new NotSupportedException();
			public void Dispose() { }
		}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

		/// <summary>
		/// Gets a ref iterator (use 'foreach ref')
		/// </summary>
		/// <returns></returns>
		public Enumerator GetEnumerator() => new Enumerator(this);
		IEnumerator<Instruction> IEnumerable<Instruction>.GetEnumerator() => new Enumerator(this);
		IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);

		/// <summary>
		/// Returns a read-only wrapper for this list
		/// </summary>
		/// <returns></returns>
		public ReadOnlyCollection<Instruction> AsReadOnly() => new ReadOnlyCollection<Instruction>(this);

		/// <summary>
		/// Creates a new array with all instructions and returns it
		/// </summary>
		/// <returns></returns>
		public Instruction[] ToArray() {
			int count = this.count;
			if (count == 0)
				return Array2.Empty<Instruction>();
			var res = new Instruction[count];
			Array.Copy(elements, 0, res, 0, res.Length);
			return res;
		}
	}
}
