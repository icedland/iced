// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if INSTR_INFO
using System;
using System.Diagnostics;
using System.Text;

namespace Iced.Intel {
	/// <summary>
	/// A memory location used by an instruction
	/// </summary>
	public readonly struct UsedMemory {
		readonly ulong displ;
		readonly byte segReg;
		readonly byte baseReg;
		readonly byte indexReg;
		readonly byte memorySize;
		readonly byte scale;
		readonly byte access;
		readonly byte addressSize;
		readonly byte vsibSize;

		/// <summary>
		/// Effective segment register or <see cref="Register.None"/> if the segment register is ignored
		/// </summary>
		public Register Segment => (Register)segReg;

		/// <summary>
		/// Base register or <see cref="Register.None"/> if none
		/// </summary>
		public Register Base => (Register)baseReg;

		/// <summary>
		/// Index register or <see cref="Register.None"/> if none
		/// </summary>
		public Register Index => (Register)indexReg;

		/// <summary>
		/// Index scale (1, 2, 4 or 8)
		/// </summary>
		public int Scale => scale;

		/// <summary>
		/// Displacement
		/// </summary>
		public ulong Displacement => displ;

		/// <summary>
		/// Size of location
		/// </summary>
		public MemorySize MemorySize => (MemorySize)memorySize;

		/// <summary>
		/// Memory access
		/// </summary>
		public OpAccess Access => (OpAccess)access;

		/// <summary>
		/// Address size
		/// </summary>
		public CodeSize AddressSize => (CodeSize)addressSize;

		/// <summary>
		/// VSIB size (<c>0</c>, <c>4</c> or <c>8</c>)
		/// </summary>
		public int VsibSize => vsibSize;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="segReg">Effective segment register or <see cref="Register.None"/> if the segment register is ignored</param>
		/// <param name="baseReg">Base register</param>
		/// <param name="indexReg">Index register</param>
		/// <param name="scale">1, 2, 4 or 8</param>
		/// <param name="displ">Displacement</param>
		/// <param name="memorySize">Memory size</param>
		/// <param name="access">Access</param>
		public UsedMemory(Register segReg, Register baseReg, Register indexReg, int scale, long displ, MemorySize memorySize, OpAccess access) {
			this.displ = (ulong)displ;
			Debug.Assert((uint)segReg <= byte.MaxValue);
			this.segReg = (byte)segReg;
			Debug.Assert((uint)baseReg <= byte.MaxValue);
			this.baseReg = (byte)baseReg;
			Debug.Assert((uint)indexReg <= byte.MaxValue);
			this.indexReg = (byte)indexReg;
			Debug.Assert((uint)memorySize <= byte.MaxValue);
			this.memorySize = (byte)memorySize;
			Debug.Assert(scale == 1 || scale == 2 || scale == 4 || scale == 8);
			this.scale = (byte)scale;
			Debug.Assert((uint)access <= byte.MaxValue);
			this.access = (byte)access;
			addressSize = (byte)CodeSize.Unknown;
			vsibSize = 0;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="segReg">Effective segment register or <see cref="Register.None"/> if the segment register is ignored</param>
		/// <param name="baseReg">Base register</param>
		/// <param name="indexReg">Index register</param>
		/// <param name="scale">1, 2, 4 or 8</param>
		/// <param name="displ">Displacement</param>
		/// <param name="memorySize">Memory size</param>
		/// <param name="access">Access</param>
		public UsedMemory(Register segReg, Register baseReg, Register indexReg, int scale, ulong displ, MemorySize memorySize, OpAccess access) {
			this.displ = displ;
			Debug.Assert((uint)segReg <= byte.MaxValue);
			this.segReg = (byte)segReg;
			Debug.Assert((uint)baseReg <= byte.MaxValue);
			this.baseReg = (byte)baseReg;
			Debug.Assert((uint)indexReg <= byte.MaxValue);
			this.indexReg = (byte)indexReg;
			Debug.Assert((uint)memorySize <= byte.MaxValue);
			this.memorySize = (byte)memorySize;
			Debug.Assert(scale == 1 || scale == 2 || scale == 4 || scale == 8);
			this.scale = (byte)scale;
			Debug.Assert((uint)access <= byte.MaxValue);
			this.access = (byte)access;
			addressSize = (byte)CodeSize.Unknown;
			vsibSize = 0;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="segReg">Effective segment register or <see cref="Register.None"/> if the segment register is ignored</param>
		/// <param name="baseReg">Base register</param>
		/// <param name="indexReg">Index register</param>
		/// <param name="scale">1, 2, 4 or 8</param>
		/// <param name="displ">Displacement</param>
		/// <param name="memorySize">Memory size</param>
		/// <param name="access">Access</param>
		/// <param name="addressSize">Address size</param>
		/// <param name="vsibSize">VSIB size (<c>0</c>, <c>4</c> or <c>8</c>)</param>
		public UsedMemory(Register segReg, Register baseReg, Register indexReg, int scale, long displ, MemorySize memorySize, OpAccess access, CodeSize addressSize, int vsibSize) {
			this.displ = (ulong)displ;
			Debug.Assert((uint)segReg <= byte.MaxValue);
			this.segReg = (byte)segReg;
			Debug.Assert((uint)baseReg <= byte.MaxValue);
			this.baseReg = (byte)baseReg;
			Debug.Assert((uint)indexReg <= byte.MaxValue);
			this.indexReg = (byte)indexReg;
			Debug.Assert((uint)memorySize <= byte.MaxValue);
			this.memorySize = (byte)memorySize;
			Debug.Assert(scale == 1 || scale == 2 || scale == 4 || scale == 8);
			this.scale = (byte)scale;
			Debug.Assert((uint)access <= byte.MaxValue);
			this.access = (byte)access;
			Debug.Assert((uint)addressSize <= byte.MaxValue);
			this.addressSize = (byte)addressSize;
			Debug.Assert(vsibSize == 0 || vsibSize == 4 || vsibSize == 8);
			this.vsibSize = (byte)vsibSize;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="segReg">Effective segment register or <see cref="Register.None"/> if the segment register is ignored</param>
		/// <param name="baseReg">Base register</param>
		/// <param name="indexReg">Index register</param>
		/// <param name="scale">1, 2, 4 or 8</param>
		/// <param name="displ">Displacement</param>
		/// <param name="memorySize">Memory size</param>
		/// <param name="access">Access</param>
		/// <param name="addressSize">Address size</param>
		/// <param name="vsibSize">VSIB size (<c>0</c>, <c>4</c> or <c>8</c>)</param>
		public UsedMemory(Register segReg, Register baseReg, Register indexReg, int scale, ulong displ, MemorySize memorySize, OpAccess access, CodeSize addressSize, int vsibSize) {
			this.displ = displ;
			Debug.Assert((uint)segReg <= byte.MaxValue);
			this.segReg = (byte)segReg;
			Debug.Assert((uint)baseReg <= byte.MaxValue);
			this.baseReg = (byte)baseReg;
			Debug.Assert((uint)indexReg <= byte.MaxValue);
			this.indexReg = (byte)indexReg;
			Debug.Assert((uint)memorySize <= byte.MaxValue);
			this.memorySize = (byte)memorySize;
			Debug.Assert(scale == 1 || scale == 2 || scale == 4 || scale == 8);
			this.scale = (byte)scale;
			Debug.Assert((uint)access <= byte.MaxValue);
			this.access = (byte)access;
			Debug.Assert((uint)addressSize <= byte.MaxValue);
			this.addressSize = (byte)addressSize;
			Debug.Assert(vsibSize == 0 || vsibSize == 4 || vsibSize == 8);
			this.vsibSize = (byte)vsibSize;
		}

		/// <summary>
		/// Gets the virtual address of a memory operand
		/// </summary>
		/// <param name="elementIndex">Only used if it's a vsib memory operand. This is the element index of the vector index register.</param>
		/// <param name="getRegisterValue">Delegate that returns the value of a register or the base address of a segment register</param>
		/// <returns></returns>
		public readonly ulong GetVirtualAddress(int elementIndex, VAGetRegisterValue getRegisterValue) {
			if (getRegisterValue is null)
				throw new ArgumentNullException(nameof(getRegisterValue));
			var provider = new VARegisterValueProviderDelegateImpl(getRegisterValue);
			if (TryGetVirtualAddress(elementIndex, provider, out var result))
				return result;
			return 0;
		}

		/// <summary>
		/// Gets the virtual address of a memory operand
		/// </summary>
		/// <param name="elementIndex">Only used if it's a vsib memory operand. This is the element index of the vector index register.</param>
		/// <param name="registerValueProvider">Returns values of registers and segment base addresses</param>
		/// <returns></returns>
		public readonly ulong GetVirtualAddress(int elementIndex, IVARegisterValueProvider registerValueProvider) {
			if (registerValueProvider is null)
				throw new ArgumentNullException(nameof(registerValueProvider));
			var provider = new VARegisterValueProviderAdapter(registerValueProvider);
			if (TryGetVirtualAddress(elementIndex, provider, out var result))
				return result;
			return 0;
		}

		/// <summary>
		/// Gets the virtual address of a memory operand
		/// </summary>
		/// <param name="elementIndex">Only used if it's a vsib memory operand. This is the element index of the vector index register.</param>
		/// <param name="result">Result if this method returns <see langword="true"/></param>
		/// <param name="getRegisterValue">Returns values of registers and segment base addresses</param>
		/// <returns></returns>
		public readonly bool TryGetVirtualAddress(int elementIndex, out ulong result, VATryGetRegisterValue getRegisterValue) {
			if (getRegisterValue is null)
				throw new ArgumentNullException(nameof(getRegisterValue));
			var provider = new VATryGetRegisterValueDelegateImpl(getRegisterValue);
			return TryGetVirtualAddress(elementIndex, provider, out result);
		}

		/// <summary>
		/// Gets the virtual address of a memory operand
		/// </summary>
		/// <param name="elementIndex">Only used if it's a vsib memory operand. This is the element index of the vector index register.</param>
		/// <param name="registerValueProvider">Returns values of registers and segment base addresses</param>
		/// <param name="result">Result if this method returns <see langword="true"/></param>
		/// <returns></returns>
		public readonly bool TryGetVirtualAddress(int elementIndex, IVATryGetRegisterValueProvider registerValueProvider, out ulong result) {
			if (registerValueProvider is null)
				throw new ArgumentNullException(nameof(registerValueProvider));

			result = 0;

			ulong tmpAddr = Displacement;
			ulong tmpValue;

			var reg = Base;
			if (reg != Register.None) {
				if (!registerValueProvider.TryGetRegisterValue(reg, 0, 0, out tmpValue))
					return false;
				tmpAddr += tmpValue;
			}

			reg = Index;
			if (reg != Register.None) {
				if (!registerValueProvider.TryGetRegisterValue(reg, elementIndex, vsibSize, out tmpValue))
					return false;
				if (vsibSize == 4)
					tmpValue = (ulong)(int)tmpValue;
				tmpAddr += tmpValue * (ulong)(uint)Scale;
			}

			switch (AddressSize) {
			case CodeSize.Code16:
				tmpAddr = (ushort)tmpAddr;
				break;
			case CodeSize.Code32:
				tmpAddr = (uint)tmpAddr;
				break;
			}

			reg = Segment;
			if (reg != Register.None) {
				if (!registerValueProvider.TryGetRegisterValue(reg, 0, 0, out tmpValue))
					return false;
				tmpAddr += tmpValue;
			}

			result = tmpAddr;
			return true;
		}

		/// <summary>
		/// ToString()
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			var sb = new StringBuilder();
			sb.Append('[');
			sb.Append(Segment.ToString());
			sb.Append(':');
			bool needPlus = false;
			if (Base != Register.None) {
				sb.Append(Base.ToString());
				needPlus = true;
			}
			if (Index != Register.None) {
				if (needPlus)
					sb.Append('+');
				needPlus = true;
				sb.Append(Index.ToString());
				if (Scale != 1) {
					sb.Append('*');
					sb.Append((char)('0' + Scale));
				}
			}
			if (Displacement != 0 || !needPlus) {
				if (needPlus)
					sb.Append('+');
				if (Displacement <= 9)
					sb.Append(Displacement);
				else {
					sb.Append("0x");
					sb.Append(Displacement.ToString("X"));
				}
			}
			sb.Append(';');
			sb.Append(MemorySize.ToString());
			sb.Append(';');
			sb.Append(Access.ToString());
			sb.Append(']');
			return sb.ToString();
		}
	}
}
#endif
