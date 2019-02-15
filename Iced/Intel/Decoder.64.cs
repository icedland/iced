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

#if !NO_DECODER64 && !NO_DECODER
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Iced.Intel {
	sealed partial class Decoder {
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		internal void ReadOpMem_m64(ref Instruction instruction) {
			Debug.Assert(is64Mode);
			Debug.Assert(state.addressSize == OpSize.Size32 || state.addressSize == OpSize.Size64);
			Debug.Assert(state.Encoding != EncodingKind.EVEX);
			if (state.addressSize == OpSize.Size64)
				ReadOpMem32Or64(ref instruction, Register.RAX, Register.RAX, TupleType.None, false);
			else
				ReadOpMem32Or64(ref instruction, Register.EAX, Register.EAX, TupleType.None, false);
		}

		// All MPX instructions in 64-bit mode force 64-bit addressing (see SDM Vol 1, 17.5.1 Intel MPX and Operating Modes)
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		internal void ReadOpMem_m64_FORCE64(ref Instruction instruction) {
			Debug.Assert(is64Mode);
			Debug.Assert(state.addressSize == OpSize.Size32 || state.addressSize == OpSize.Size64);
			Debug.Assert(state.Encoding != EncodingKind.EVEX);
			state.addressSize = OpSize.Size64;
			ReadOpMem32Or64(ref instruction, Register.RAX, Register.RAX, TupleType.None, false);
		}

		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		internal void ReadOpMem_m64(ref Instruction instruction, TupleType tupleType) {
			Debug.Assert(is64Mode);
			Debug.Assert(state.addressSize == OpSize.Size32 || state.addressSize == OpSize.Size64);
			if (state.addressSize == OpSize.Size64)
				ReadOpMem32Or64(ref instruction, Register.RAX, Register.RAX, tupleType, false);
			else
				ReadOpMem32Or64(ref instruction, Register.EAX, Register.EAX, tupleType, false);
		}

		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		internal void ReadOpMem_VSIB_m64(ref Instruction instruction, Register vsibIndex) {
			Debug.Assert(is64Mode);
			Debug.Assert(state.addressSize == OpSize.Size32 || state.addressSize == OpSize.Size64);
			Debug.Assert(state.Encoding != EncodingKind.EVEX);
			if (state.addressSize == OpSize.Size64) {
				if (!ReadOpMem32Or64(ref instruction, Register.RAX, vsibIndex, TupleType.None, true))
					SetInvalidInstruction();
			}
			else {
				if (!ReadOpMem32Or64(ref instruction, Register.EAX, vsibIndex, TupleType.None, true))
					SetInvalidInstruction();
			}
		}

		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		internal void ReadOpMem_VSIB_m64(ref Instruction instruction, Register vsibIndex, TupleType tupleType) {
			Debug.Assert(is64Mode);
			Debug.Assert(state.addressSize == OpSize.Size32 || state.addressSize == OpSize.Size64);
			if (state.addressSize == OpSize.Size64) {
				if (!ReadOpMem32Or64(ref instruction, Register.RAX, vsibIndex, tupleType, true))
					SetInvalidInstruction();
			}
			else {
				if (!ReadOpMem32Or64(ref instruction, Register.EAX, vsibIndex, tupleType, true))
					SetInvalidInstruction();
			}
		}
	}
}
#endif
