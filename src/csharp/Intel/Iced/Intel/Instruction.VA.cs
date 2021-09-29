// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Diagnostics;

namespace Iced.Intel {
	partial struct Instruction {
		/// <summary>
		/// Gets the virtual address of a memory operand
		/// </summary>
		/// <param name="operand">Operand number, must be a memory operand</param>
		/// <param name="elementIndex">Only used if it's a vsib memory operand. This is the element index of the vector index register.</param>
		/// <param name="getRegisterValue">Delegate that returns the value of a register or the base address of a segment register</param>
		/// <returns></returns>
		public readonly ulong GetVirtualAddress(int operand, int elementIndex, VAGetRegisterValue getRegisterValue) {
			if (getRegisterValue is null)
				throw new ArgumentNullException(nameof(getRegisterValue));
			var provider = new VARegisterValueProviderDelegateImpl(getRegisterValue);
			if (TryGetVirtualAddress(operand, elementIndex, provider, out var result))
				return result;
			return 0;
		}

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
			var provider = new VARegisterValueProviderAdapter(registerValueProvider);
			if (TryGetVirtualAddress(operand, elementIndex, provider, out var result))
				return result;
			return 0;
		}

		/// <summary>
		/// Gets the virtual address of a memory operand
		/// </summary>
		/// <param name="operand">Operand number, must be a memory operand</param>
		/// <param name="elementIndex">Only used if it's a vsib memory operand. This is the element index of the vector index register.</param>
		/// <param name="result">Result if this method returns <see langword="true"/></param>
		/// <param name="getRegisterValue">Returns values of registers and segment base addresses</param>
		/// <returns></returns>
		public readonly bool TryGetVirtualAddress(int operand, int elementIndex, out ulong result, VATryGetRegisterValue getRegisterValue) {
			if (getRegisterValue is null)
				throw new ArgumentNullException(nameof(getRegisterValue));
			var provider = new VATryGetRegisterValueDelegateImpl(getRegisterValue);
			return TryGetVirtualAddress(operand, elementIndex, provider, out result);
		}

		/// <summary>
		/// Gets the virtual address of a memory operand
		/// </summary>
		/// <param name="operand">Operand number, must be a memory operand</param>
		/// <param name="elementIndex">Only used if it's a vsib memory operand. This is the element index of the vector index register.</param>
		/// <param name="registerValueProvider">Returns values of registers and segment base addresses</param>
		/// <param name="result">Result if this method returns <see langword="true"/></param>
		/// <returns></returns>
		public readonly bool TryGetVirtualAddress(int operand, int elementIndex, IVATryGetRegisterValueProvider registerValueProvider, out ulong result) {
			if (registerValueProvider is null)
				throw new ArgumentNullException(nameof(registerValueProvider));
			ulong seg, @base;
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
				result = 0;
				return true;

			case OpKind.MemorySegSI:
				if (registerValueProvider.TryGetRegisterValue(MemorySegment, 0, 0, out seg) &&
					registerValueProvider.TryGetRegisterValue(Register.SI, 0, 0, out @base)) {
					result = seg + (ushort)@base;
					return true;
				}
				break;

			case OpKind.MemorySegESI:
				if (registerValueProvider.TryGetRegisterValue(MemorySegment, 0, 0, out seg) &&
					registerValueProvider.TryGetRegisterValue(Register.ESI, 0, 0, out @base)) {
					result = seg + (uint)@base;
					return true;
				}
				break;

			case OpKind.MemorySegRSI:
				if (registerValueProvider.TryGetRegisterValue(MemorySegment, 0, 0, out seg) &&
					registerValueProvider.TryGetRegisterValue(Register.RSI, 0, 0, out @base)) {
					result = seg + @base;
					return true;
				}
				break;

			case OpKind.MemorySegDI:
				if (registerValueProvider.TryGetRegisterValue(MemorySegment, 0, 0, out seg) &&
					registerValueProvider.TryGetRegisterValue(Register.DI, 0, 0, out @base)) {
					result = seg + (ushort)@base;
					return true;
				}
				break;

			case OpKind.MemorySegEDI:
				if (registerValueProvider.TryGetRegisterValue(MemorySegment, 0, 0, out seg) &&
					registerValueProvider.TryGetRegisterValue(Register.EDI, 0, 0, out @base)) {
					result = seg + (uint)@base;
					return true;
				}
				break;

			case OpKind.MemorySegRDI:
				if (registerValueProvider.TryGetRegisterValue(MemorySegment, 0, 0, out seg) &&
					registerValueProvider.TryGetRegisterValue(Register.RDI, 0, 0, out @base)) {
					result = seg + @base;
					return true;
				}
				break;

			case OpKind.MemoryESDI:
				if (registerValueProvider.TryGetRegisterValue(Register.ES, 0, 0, out seg) &&
					registerValueProvider.TryGetRegisterValue(Register.DI, 0, 0, out @base)) {
					result = seg + (ushort)@base;
					return true;
				}
				break;

			case OpKind.MemoryESEDI:
				if (registerValueProvider.TryGetRegisterValue(Register.ES, 0, 0, out seg) &&
					registerValueProvider.TryGetRegisterValue(Register.EDI, 0, 0, out @base)) {
					result = seg + (uint)@base;
					return true;
				}
				break;

			case OpKind.MemoryESRDI:
				if (registerValueProvider.TryGetRegisterValue(Register.ES, 0, 0, out seg) &&
					registerValueProvider.TryGetRegisterValue(Register.RDI, 0, 0, out @base)) {
					result = seg + @base;
					return true;
				}
				break;

			case OpKind.Memory:
				var baseReg = MemoryBase;
				var indexReg = MemoryIndex;
				int addrSize = InstructionUtils.GetAddressSizeInBytes(baseReg, indexReg, MemoryDisplSize, CodeSize);
				ulong offset = MemoryDisplacement64;
				ulong offsetMask;
				if (addrSize == 8)
					offsetMask = ulong.MaxValue;
				else if (addrSize == 4)
					offsetMask = uint.MaxValue;
				else {
					Debug.Assert(addrSize == 2);
					offsetMask = ushort.MaxValue;
				}
				if (baseReg != Register.None && baseReg != Register.RIP && baseReg != Register.EIP) {
					if (!registerValueProvider.TryGetRegisterValue(baseReg, 0, 0, out @base))
						break;
					offset += @base;
				}
				var code = Code;
				if (indexReg != Register.None && !code.IgnoresIndex() && !code.IsTileStrideIndex()) {
					if (TryGetVsib64(out bool vsib64)) {
						bool b;
						if (vsib64)
							b = registerValueProvider.TryGetRegisterValue(indexReg, elementIndex, 8, out @base);
						else {
							b = registerValueProvider.TryGetRegisterValue(indexReg, elementIndex, 4, out @base);
							@base = (ulong)(int)@base;
						}
						if (!b)
							break;
						offset += @base << InternalMemoryIndexScale;
					}
					else {
						if (!registerValueProvider.TryGetRegisterValue(indexReg, 0, 0, out @base))
							break;
						offset += @base << InternalMemoryIndexScale;
					}
				}
#if MVEX
				Static.Assert(Code.MVEX_Vloadunpackhd_zmm_k1_mt + 1 == Code.MVEX_Vloadunpackhq_zmm_k1_mt ? 0 : -1);
				Static.Assert(Code.MVEX_Vloadunpackhd_zmm_k1_mt + 2 == Code.MVEX_Vpackstorehd_mt_k1_zmm ? 0 : -1);
				Static.Assert(Code.MVEX_Vloadunpackhd_zmm_k1_mt + 3 == Code.MVEX_Vpackstorehq_mt_k1_zmm ? 0 : -1);
				Static.Assert(Code.MVEX_Vloadunpackhd_zmm_k1_mt + 4 == Code.MVEX_Vloadunpackhps_zmm_k1_mt ? 0 : -1);
				Static.Assert(Code.MVEX_Vloadunpackhd_zmm_k1_mt + 5 == Code.MVEX_Vloadunpackhpd_zmm_k1_mt ? 0 : -1);
				Static.Assert(Code.MVEX_Vloadunpackhd_zmm_k1_mt + 6 == Code.MVEX_Vpackstorehps_mt_k1_zmm ? 0 : -1);
				Static.Assert(Code.MVEX_Vloadunpackhd_zmm_k1_mt + 7 == Code.MVEX_Vpackstorehpd_mt_k1_zmm ? 0 : -1);
				if (code >= Code.MVEX_Vloadunpackhd_zmm_k1_mt && code <= Code.MVEX_Vpackstorehpd_mt_k1_zmm)
					offset -= 0x40;
#endif
				offset &= offsetMask;
				if (!code.IgnoresSegment()) {
					if (!registerValueProvider.TryGetRegisterValue(MemorySegment, 0, 0, out seg))
						break;
					offset += seg;
				}
				result = offset;
				return true;

			default:
				throw new InvalidOperationException();
			}

			result = 0;
			return false;
		}
	}

	/// <summary>
	/// Gets a register value. If <paramref name="register"/> is a segment register, this method should return the segment's base address,
	/// not the segment's register value.
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
		/// Gets a register value. If <paramref name="register"/> is a segment register, this method should return the segment's base address,
		/// not the segment's register value.
		/// </summary>
		/// <param name="register">Register (GPR8, GPR16, GPR32, GPR64, XMM, YMM, ZMM, seg)</param>
		/// <param name="elementIndex">Only used if it's a vsib memory operand. This is the element index of the vector index register.</param>
		/// <param name="elementSize">Only used if it's a vsib memory operand. Size in bytes of elements in vector index register (4 or 8).</param>
		/// <returns></returns>
		ulong GetRegisterValue(Register register, int elementIndex, int elementSize);
	}

	/// <summary>
	/// Gets a register value. If <paramref name="register"/> is a segment register, this method should return the segment's base address,
	/// not the segment's register value.
	/// </summary>
	/// <param name="register">Register (GPR8, GPR16, GPR32, GPR64, XMM, YMM, ZMM, seg)</param>
	/// <param name="elementIndex">Only used if it's a vsib memory operand. This is the element index of the vector index register.</param>
	/// <param name="elementSize">Only used if it's a vsib memory operand. Size in bytes of elements in vector index register (4 or 8).</param>
	/// <param name="value">Updated with the register value if successful</param>
	/// <returns></returns>
	public delegate bool VATryGetRegisterValue(Register register, int elementIndex, int elementSize, out ulong value);

	/// <summary>
	/// Called when calculating the virtual address of a memory operand
	/// </summary>
	public interface IVATryGetRegisterValueProvider {
		/// <summary>
		/// Gets a register value. If <paramref name="register"/> is a segment register, this method should return the segment's base address,
		/// not the segment's register value.
		/// </summary>
		/// <param name="register">Register (GPR8, GPR16, GPR32, GPR64, XMM, YMM, ZMM, seg)</param>
		/// <param name="elementIndex">Only used if it's a vsib memory operand. This is the element index of the vector index register.</param>
		/// <param name="elementSize">Only used if it's a vsib memory operand. Size in bytes of elements in vector index register (4 or 8).</param>
		/// <param name="value">Updated with the register value if successful</param>
		/// <returns></returns>
		bool TryGetRegisterValue(Register register, int elementIndex, int elementSize, out ulong value);
	}

	sealed class VARegisterValueProviderDelegateImpl : IVATryGetRegisterValueProvider {
		readonly VAGetRegisterValue getRegisterValue;

		public VARegisterValueProviderDelegateImpl(VAGetRegisterValue getRegisterValue) =>
			this.getRegisterValue = getRegisterValue ?? throw new ArgumentNullException(nameof(getRegisterValue));

		public bool TryGetRegisterValue(Register register, int elementIndex, int elementSize, out ulong value) {
			value = getRegisterValue(register, elementIndex, elementSize);
			return true;
		}
	}

	sealed class VARegisterValueProviderAdapter : IVATryGetRegisterValueProvider {
		readonly IVARegisterValueProvider provider;

		public VARegisterValueProviderAdapter(IVARegisterValueProvider provider) => this.provider = provider;

		public bool TryGetRegisterValue(Register register, int elementIndex, int elementSize, out ulong value) {
			value = provider.GetRegisterValue(register, elementIndex, elementSize);
			return true;
		}
	}

	sealed class VATryGetRegisterValueDelegateImpl : IVATryGetRegisterValueProvider {
		readonly VATryGetRegisterValue getRegisterValue;

		public VATryGetRegisterValueDelegateImpl(VATryGetRegisterValue getRegisterValue) => this.getRegisterValue = getRegisterValue;

		public bool TryGetRegisterValue(Register register, int elementIndex, int elementSize, out ulong value) =>
			getRegisterValue(register, elementIndex, elementSize, out value);
	}
}
