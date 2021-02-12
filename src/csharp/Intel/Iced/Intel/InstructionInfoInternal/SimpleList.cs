// SPDX-License-Identifier: MIT
// Copyright wtfsck@protonmail.com
// Copyright iced contributors

#if INSTR_INFO
namespace Iced.Intel.InstructionInfoInternal {
	struct SimpleList<T> {
		public static readonly SimpleList<T> Empty = new SimpleList<T>(System.Array2.Empty<T>());
		public T[] Array;
		public int ValidLength;
		public SimpleList(T[] array) {
			Array = array;
			ValidLength = 0;
		}
	}
}
#endif
