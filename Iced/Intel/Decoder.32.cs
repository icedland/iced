/*
    Copyright (C) 2018-2019 de4dot@gmail.com

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
