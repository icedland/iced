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

#if !NO_DECODER32 && !NO_DECODER
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Iced.Intel {
	sealed partial class Decoder {
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void ReadOpMem_m32(ref Instruction instruction) {
			Debug.Assert(!is64Mode);
			Debug.Assert(state.addressSize == OpSize.Size16 || state.addressSize == OpSize.Size32);
			Debug.Assert(state.Encoding != EncodingKind.EVEX);
			if (state.addressSize == OpSize.Size32)
				ReadOpMem32Or64(ref instruction, Register.EAX, Register.EAX, TupleType.None, false);
			else
				ReadOpMem16(ref instruction, TupleType.None);
		}

		// All MPX instructions in 16/32-bit mode require 32-bit addressing (see SDM Vol 1, 17.5.1 Intel MPX and Operating Modes)
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void ReadOpMem_m32_ONLY32(ref Instruction instruction) {
			Debug.Assert(!is64Mode);
			Debug.Assert(state.addressSize == OpSize.Size16 || state.addressSize == OpSize.Size32);
			Debug.Assert(state.Encoding != EncodingKind.EVEX);
			if (state.addressSize == OpSize.Size32)
				ReadOpMem32Or64(ref instruction, Register.EAX, Register.EAX, TupleType.None, false);
			else
				SetInvalidInstruction();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void ReadOpMem_m32(ref Instruction instruction, TupleType tupleType) {
			Debug.Assert(!is64Mode);
			Debug.Assert(state.addressSize == OpSize.Size16 || state.addressSize == OpSize.Size32);
			if (state.addressSize == OpSize.Size32)
				ReadOpMem32Or64(ref instruction, Register.EAX, Register.EAX, tupleType, false);
			else
				ReadOpMem16(ref instruction, tupleType);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void ReadOpMem_VSIB_m32(ref Instruction instruction, Register vsibIndex) {
			Debug.Assert(!is64Mode);
			Debug.Assert(state.addressSize == OpSize.Size16 || state.addressSize == OpSize.Size32);
			Debug.Assert(state.Encoding != EncodingKind.EVEX);
			if (state.addressSize == OpSize.Size32) {
				if (!ReadOpMem32Or64(ref instruction, Register.EAX, vsibIndex, TupleType.None, true))
					SetInvalidInstruction();
			}
			else
				SetInvalidInstruction();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void ReadOpMem_VSIB_m32(ref Instruction instruction, Register vsibIndex, TupleType tupleType) {
			Debug.Assert(!is64Mode);
			Debug.Assert(state.addressSize == OpSize.Size16 || state.addressSize == OpSize.Size32);
			if (state.addressSize == OpSize.Size32) {
				if (!ReadOpMem32Or64(ref instruction, Register.EAX, vsibIndex, tupleType, true))
					SetInvalidInstruction();
			}
			else
				SetInvalidInstruction();
		}
	}
}
#endif
