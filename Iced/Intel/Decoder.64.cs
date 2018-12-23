/*
    Copyright (C) 2018 de4dot@gmail.com

    This file is part of Iced.

    Iced is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Iced is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Iced.  If not, see <https://www.gnu.org/licenses/>.
*/

#if !NO_DECODER64 && !NO_DECODER
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Iced.Intel {
	sealed partial class Decoder {
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
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
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void ReadOpMem_m64_FORCE64(ref Instruction instruction) {
			Debug.Assert(is64Mode);
			Debug.Assert(state.addressSize == OpSize.Size32 || state.addressSize == OpSize.Size64);
			Debug.Assert(state.Encoding != EncodingKind.EVEX);
			state.addressSize = OpSize.Size64;
			ReadOpMem32Or64(ref instruction, Register.RAX, Register.RAX, TupleType.None, false);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void ReadOpMem_m64(ref Instruction instruction, TupleType tupleType) {
			Debug.Assert(is64Mode);
			Debug.Assert(state.addressSize == OpSize.Size32 || state.addressSize == OpSize.Size64);
			if (state.addressSize == OpSize.Size64)
				ReadOpMem32Or64(ref instruction, Register.RAX, Register.RAX, tupleType, false);
			else
				ReadOpMem32Or64(ref instruction, Register.EAX, Register.EAX, tupleType, false);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
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
