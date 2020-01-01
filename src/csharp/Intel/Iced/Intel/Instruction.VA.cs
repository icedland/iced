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
using System.Diagnostics;

namespace Iced.Intel {
	partial struct Instruction {
		sealed class VARegisterValueProviderImpl : IVARegisterValueProvider {
			readonly VAGetRegisterValue getRegisterValue;
			public VARegisterValueProviderImpl(VAGetRegisterValue getRegisterValue) =>
				this.getRegisterValue = getRegisterValue ?? throw new ArgumentNullException(nameof(getRegisterValue));
			public ulong GetRegisterValue(Register register, int elementIndex, int elementSize) =>
				getRegisterValue(register, elementIndex, elementSize);
		}

		/// <summary>
		/// Gets the virtual address of a memory operand
		/// </summary>
		/// <param name="operand">Operand number, must be a memory operand</param>
		/// <param name="elementIndex">Only used if it's a vsib memory operand. This is the element index of the vector index register.</param>
		/// <param name="getRegisterValue">Delegate that returns the value of a register or the base address of a segment register</param>
		/// <returns></returns>
		public readonly ulong GetVirtualAddress(int operand, int elementIndex, VAGetRegisterValue getRegisterValue) =>
			GetVirtualAddress(operand, elementIndex, new VARegisterValueProviderImpl(getRegisterValue));

		/// <summary>
		/// Gets the virtual address of a memory operand
		/// </summary>
		/// <param name="operand">Operand number, must be a memory operand</param>
		/// <param name="elementIndex">Only used if it's a vsib memory operand. This is the element index of the vector index register.</param>
		/// <param name="registerValueProvider">Returns values of registers and segment base addresses</param>
		/// <returns></returns>
		public readonly ulong GetVirtualAddress(int operand, int elementIndex, IVARegisterValueProvider registerValueProvider) {
			if (registerValueProvider is null)
				throw new ArgumentNullException(nameof(registerValueProvider));
			switch (GetOpKind(operand)) {
			case OpKind.Register:
			case OpKind.NearBranch16:
			case OpKind.NearBranch32:
			case OpKind.NearBranch64:
			case OpKind.FarBranch16:
			case OpKind.FarBranch32:
			case OpKind.Immediate8:
			case OpKind.Immediate8_2nd:
			case OpKind.Immediate16:
			case OpKind.Immediate32:
			case OpKind.Immediate64:
			case OpKind.Immediate8to16:
			case OpKind.Immediate8to32:
			case OpKind.Immediate8to64:
			case OpKind.Immediate32to64:
				return 0;

			case OpKind.MemorySegSI:
				return registerValueProvider.GetRegisterValue(MemorySegment, 0, 0) + (ushort)registerValueProvider.GetRegisterValue(Register.SI, 0, 0);

			case OpKind.MemorySegESI:
				return registerValueProvider.GetRegisterValue(MemorySegment, 0, 0) + (uint)registerValueProvider.GetRegisterValue(Register.ESI, 0, 0);

			case OpKind.MemorySegRSI:
				return registerValueProvider.GetRegisterValue(MemorySegment, 0, 0) + registerValueProvider.GetRegisterValue(Register.RSI, 0, 0);

			case OpKind.MemorySegDI:
				return registerValueProvider.GetRegisterValue(MemorySegment, 0, 0) + (ushort)registerValueProvider.GetRegisterValue(Register.DI, 0, 0);

			case OpKind.MemorySegEDI:
				return registerValueProvider.GetRegisterValue(MemorySegment, 0, 0) + (uint)registerValueProvider.GetRegisterValue(Register.EDI, 0, 0);

			case OpKind.MemorySegRDI:
				return registerValueProvider.GetRegisterValue(MemorySegment, 0, 0) + registerValueProvider.GetRegisterValue(Register.RDI, 0, 0);

			case OpKind.MemoryESDI:
				return registerValueProvider.GetRegisterValue(Register.ES, 0, 0) + (ushort)registerValueProvider.GetRegisterValue(Register.DI, 0, 0);

			case OpKind.MemoryESEDI:
				return registerValueProvider.GetRegisterValue(Register.ES, 0, 0) + (uint)registerValueProvider.GetRegisterValue(Register.EDI, 0, 0);

			case OpKind.MemoryESRDI:
				return registerValueProvider.GetRegisterValue(Register.ES, 0, 0) + registerValueProvider.GetRegisterValue(Register.RDI, 0, 0);

			case OpKind.Memory64:
				return registerValueProvider.GetRegisterValue(MemorySegment, 0, 0) + MemoryAddress64;

			case OpKind.Memory:
				var baseReg = MemoryBase;
				var indexReg = MemoryIndex;
				int addrSize = InstructionUtils.GetAddressSizeInBytes(baseReg, indexReg, MemoryDisplSize, CodeSize);
				ulong offset = MemoryDisplacement;
				ulong offsetMask;
				if (addrSize == 8) {
					offset = (ulong)(int)offset;
					offsetMask = ulong.MaxValue;
				}
				else if (addrSize == 4)
					offsetMask = uint.MaxValue;
				else {
					Debug.Assert(addrSize == 2);
					offsetMask = ushort.MaxValue;
				}
				if (baseReg != Register.None) {
					if (baseReg == Register.RIP)
						offset += NextIP;
					else if (baseReg == Register.EIP)
						offset += NextIP32;
					else
						offset += registerValueProvider.GetRegisterValue(baseReg, 0, 0);
				}
				if (indexReg != Register.None) {
					if (TryGetVsib64(out bool vsib64)) {
						if (vsib64)
							offset += registerValueProvider.GetRegisterValue(indexReg, elementIndex, 8) << InternalMemoryIndexScale;
						else
							offset += (ulong)(uint)registerValueProvider.GetRegisterValue(indexReg, elementIndex, 4) << InternalMemoryIndexScale;
					}
					else
						offset += registerValueProvider.GetRegisterValue(indexReg, elementIndex, 0) << InternalMemoryIndexScale;
				}
				offset &= offsetMask;
				return registerValueProvider.GetRegisterValue(MemorySegment, 0, 0) + offset;

			default:
				throw new InvalidOperationException();
			}
		}
	}

	/// <summary>
	/// Gets a register value. If <paramref name="register"/> is a segment register, this method should return the segment's base value,
	/// not the segment register value.
	/// </summary>
	/// <param name="register">Register (GPR8, GPR16, GPR32, GPR64, XMM, YMM, ZMM, seg)</param>
	/// <param name="elementIndex">Only used if it's a vsib memory operand. This is the element index of the vector index register.</param>
	/// <param name="elementSize">Only used if it's a vsib memory operand. Size in bytes of elements in vector index register (4 or 8).</param>
	/// <returns></returns>
	public delegate ulong VAGetRegisterValue(Register register, int elementIndex, int elementSize);

	/// <summary>
	/// Called when calculating the virtual address of a memory operand
	/// </summary>
	public interface IVARegisterValueProvider {
		/// <summary>
		/// Gets a register value. If <paramref name="register"/> is a segment register, this method should return the segment's base value,
		/// not the segment register value.
		/// </summary>
		/// <param name="register">Register (GPR8, GPR16, GPR32, GPR64, XMM, YMM, ZMM, seg)</param>
		/// <param name="elementIndex">Only used if it's a vsib memory operand. This is the element index of the vector index register.</param>
		/// <param name="elementSize">Only used if it's a vsib memory operand. Size in bytes of elements in vector index register (4 or 8).</param>
		/// <returns></returns>
		ulong GetRegisterValue(Register register, int elementIndex, int elementSize);
	}
}
