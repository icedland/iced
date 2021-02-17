// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if INSTR_INFO
using System;
using System.Collections;
using System.Collections.Generic;
using Iced.Intel.InstructionInfoInternal;

namespace Iced.Intel {
	/// <summary>
	/// Contains information about an instruction, eg. read/written registers, read/written <c>RFLAGS</c> bits, <c>CPUID</c> feature bit, etc
	/// </summary>
	public struct InstructionInfo {
		internal SimpleList<UsedRegister> usedRegisters;
		internal SimpleList<UsedMemory> usedMemoryLocations;
		internal unsafe fixed byte opAccesses[IcedConstants.MaxOpCount];
		internal byte cpuidFeatureInternal;
		internal byte flowControl;
		internal byte encoding;
		internal byte rflagsInfo;
		internal byte flags;

		[Flags]
		internal enum Flags1 : byte {
			SaveRestore				= 0x20,
			StackInstruction		= 0x40,
			Privileged				= 0x80,
		}

		internal InstructionInfo(bool dummy) {
			usedRegisters = new SimpleList<UsedRegister>(new UsedRegister[InstrInfoConstants.DefaultUsedRegisterCollCapacity]);
			usedMemoryLocations = new SimpleList<UsedMemory>(new UsedMemory[InstrInfoConstants.DefaultUsedMemoryCollCapacity]);
			unsafe {
				opAccesses[0] = 0;
				opAccesses[1] = 0;
				opAccesses[2] = 0;
				opAccesses[3] = 0;
				opAccesses[4] = 0;
				Static.Assert(IcedConstants.MaxOpCount == 5 ? 0 : -1);
			}
			cpuidFeatureInternal = 0;
			flowControl = 0;
			encoding = 0;
			rflagsInfo = 0;
			flags = 0;
		}

		/// <summary>
		/// Gets a struct iterator that returns all accessed registers. This method doesn't return all accessed registers if <see cref="IsSaveRestoreInstruction"/> is <see langword="true"/>.
		/// <br/>
		/// <br/>
		/// Some instructions have a <c>r16</c>/<c>r32</c> operand but only use the low 8 bits of the register. In that case
		/// this method returns the 8-bit register even if it's <c>SPL</c>, <c>BPL</c>, <c>SIL</c>, <c>DIL</c> and the
		/// instruction was decoded in 16 or 32-bit mode. This is more accurate than returning the <c>r16</c>/<c>r32</c>
		/// register. Example instructions that do this: <c>PINSRB</c>, <c>ARPL</c>
		/// </summary>
		/// <returns></returns>
		public readonly UsedRegisterIterator GetUsedRegisters() => new UsedRegisterIterator(usedRegisters.Array, (uint)usedRegisters.ValidLength);

		/// <summary>
		/// Gets a struct iterator that returns all accessed memory locations
		/// </summary>
		/// <returns></returns>
		public readonly UsedMemoryIterator GetUsedMemory() => new UsedMemoryIterator(usedMemoryLocations.Array, (uint)usedMemoryLocations.ValidLength);

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
		public struct UsedRegisterIterator : IEnumerable<UsedRegister>, IEnumerator<UsedRegister> {
			readonly UsedRegister[] usedRegisters;
			readonly uint length;
			int index;

			internal UsedRegisterIterator(UsedRegister[] usedRegisters, uint length) {
				this.usedRegisters = usedRegisters;
				this.length = length;
				index = -1;
			}

			public UsedRegisterIterator GetEnumerator() => this;
			public UsedRegister Current => usedRegisters[index];

			public bool MoveNext() {
				index++;
				return (uint)index < length;
			}

			IEnumerator<UsedRegister> IEnumerable<UsedRegister>.GetEnumerator() => GetEnumerator();
			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
			UsedRegister IEnumerator<UsedRegister>.Current => Current;
			object IEnumerator.Current => Current;
			bool IEnumerator.MoveNext() => MoveNext();
			void IEnumerator.Reset() => throw new NotSupportedException();
			public void Dispose() { }
		}

		public struct UsedMemoryIterator : IEnumerable<UsedMemory>, IEnumerator<UsedMemory> {
			readonly UsedMemory[] usedMemoryLocations;
			readonly uint length;
			int index;

			internal UsedMemoryIterator(UsedMemory[] usedMemoryLocations, uint length) {
				this.usedMemoryLocations = usedMemoryLocations;
				this.length = length;
				index = -1;
			}

			public UsedMemoryIterator GetEnumerator() => this;
			public UsedMemory Current => usedMemoryLocations[index];

			public bool MoveNext() {
				index++;
				return (uint)index < length;
			}

			IEnumerator<UsedMemory> IEnumerable<UsedMemory>.GetEnumerator() => GetEnumerator();
			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
			UsedMemory IEnumerator<UsedMemory>.Current => Current;
			object IEnumerator.Current => Current;
			bool IEnumerator.MoveNext() => MoveNext();
			void IEnumerator.Reset() => throw new NotSupportedException();
			public void Dispose() { }
		}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

		/// <summary>
		/// <see langword="true"/> if it's a privileged instruction (all CPL=0 instructions (except <c>VMCALL</c>) and IOPL instructions <c>IN</c>, <c>INS</c>, <c>OUT</c>, <c>OUTS</c>, <c>CLI</c>, <c>STI</c>)
		/// </summary>
		[System.Obsolete("Use " + nameof(Instruction) + "." + nameof(Instruction.IsPrivileged) + " instead", false)]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public readonly bool IsPrivileged => (flags & (uint)Flags1.Privileged) != 0;

		/// <summary>
		/// <see langword="true"/> if this is an instruction that implicitly uses the stack pointer (<c>SP</c>/<c>ESP</c>/<c>RSP</c>), eg. <c>CALL</c>, <c>PUSH</c>, <c>POP</c>, <c>RET</c>, etc.
		/// See also <see cref="Instruction.StackPointerIncrement"/>
		/// </summary>
		[System.Obsolete("Use " + nameof(Instruction) + "." + nameof(Instruction.IsStackInstruction) + " instead", false)]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public readonly bool IsStackInstruction => (flags & (uint)Flags1.StackInstruction) != 0;

		/// <summary>
		/// <see langword="true"/> if it's an instruction that saves or restores too many registers (eg. <c>FXRSTOR</c>, <c>XSAVE</c>, etc).
		/// <see cref="GetUsedRegisters"/> won't return all accessed registers.
		/// </summary>
		[System.Obsolete("Use " + nameof(Instruction) + "." + nameof(Instruction.IsSaveRestoreInstruction) + " instead", false)]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public readonly bool IsSaveRestoreInstruction => (flags & (uint)Flags1.SaveRestore) != 0;

		/// <summary>
		/// Instruction encoding, eg. Legacy, 3DNow!, VEX, EVEX, XOP
		/// </summary>
		[System.Obsolete("Use " + nameof(Instruction) + "." + nameof(Instruction.Encoding) + " instead", false)]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public readonly EncodingKind Encoding => (EncodingKind)encoding;

		/// <summary>
		/// Gets the CPU or CPUID feature flags
		/// </summary>
		[System.Obsolete("Use " + nameof(Instruction) + "." + nameof(Instruction.CpuidFeatures) + " instead", false)]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public readonly CpuidFeature[] CpuidFeatures => CpuidFeatureInternalData.ToCpuidFeatures[cpuidFeatureInternal];

		/// <summary>
		/// Control flow info
		/// </summary>
		[System.Obsolete("Use " + nameof(Instruction) + "." + nameof(Instruction.FlowControl) + " instead", false)]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public readonly FlowControl FlowControl => (FlowControl)flowControl;

		/// <summary>
		/// Operand #0 access
		/// </summary>
		public readonly OpAccess Op0Access {
			get {
				unsafe {
					return (OpAccess)opAccesses[0];
				}
			}
		}

		/// <summary>
		/// Operand #1 access
		/// </summary>
		public readonly OpAccess Op1Access {
			get {
				unsafe {
					return (OpAccess)opAccesses[1];
				}
			}
		}

		/// <summary>
		/// Operand #2 access
		/// </summary>
		public readonly OpAccess Op2Access {
			get {
				unsafe {
					return (OpAccess)opAccesses[2];
				}
			}
		}

		/// <summary>
		/// Operand #3 access
		/// </summary>
		public readonly OpAccess Op3Access {
			get {
				unsafe {
					return (OpAccess)opAccesses[3];
				}
			}
		}

		/// <summary>
		/// Operand #4 access
		/// </summary>
		public readonly OpAccess Op4Access {
			get {
				unsafe {
					return (OpAccess)opAccesses[4];
				}
			}
		}

		/// <summary>
		/// Gets operand access
		/// </summary>
		/// <param name="operand">Operand number, 0-4</param>
		/// <returns></returns>
		public readonly OpAccess GetOpAccess(int operand) {
			switch (operand) {
			case 0: return Op0Access;
			case 1: return Op1Access;
			case 2: return Op2Access;
			case 3: return Op3Access;
			case 4: return Op4Access;
			default:
				ThrowHelper.ThrowArgumentOutOfRangeException_operand();
				return 0;
			}
		}

		/// <summary>
		/// All flags that are read by the CPU when executing the instruction. See also <see cref="RflagsModified"/>
		/// </summary>
		[System.Obsolete("Use " + nameof(Instruction) + "." + nameof(Instruction.RflagsRead) + " instead", false)]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public readonly RflagsBits RflagsRead => (RflagsBits)RflagsInfoConstants.flagsRead[rflagsInfo];

		/// <summary>
		/// All flags that are written by the CPU, except those flags that are known to be undefined, always set or always cleared. See also <see cref="RflagsModified"/>
		/// </summary>
		[System.Obsolete("Use " + nameof(Instruction) + "." + nameof(Instruction.RflagsWritten) + " instead", false)]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public readonly RflagsBits RflagsWritten => (RflagsBits)RflagsInfoConstants.flagsWritten[rflagsInfo];

		/// <summary>
		/// All flags that are always cleared by the CPU. See also <see cref="RflagsModified"/>
		/// </summary>
		[System.Obsolete("Use " + nameof(Instruction) + "." + nameof(Instruction.RflagsCleared) + " instead", false)]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public readonly RflagsBits RflagsCleared => (RflagsBits)RflagsInfoConstants.flagsCleared[rflagsInfo];

		/// <summary>
		/// All flags that are always set by the CPU. See also <see cref="RflagsModified"/>
		/// </summary>
		[System.Obsolete("Use " + nameof(Instruction) + "." + nameof(Instruction.RflagsSet) + " instead", false)]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public readonly RflagsBits RflagsSet => (RflagsBits)RflagsInfoConstants.flagsSet[rflagsInfo];

		/// <summary>
		/// All flags that are undefined after executing the instruction. See also <see cref="RflagsModified"/>
		/// </summary>
		[System.Obsolete("Use " + nameof(Instruction) + "." + nameof(Instruction.RflagsUndefined) + " instead", false)]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public readonly RflagsBits RflagsUndefined => (RflagsBits)RflagsInfoConstants.flagsUndefined[rflagsInfo];

		/// <summary>
		/// All flags that are modified by the CPU. This is <see cref="RflagsWritten"/> + <see cref="RflagsCleared"/> + <see cref="RflagsSet"/> + <see cref="RflagsUndefined"/>
		/// </summary>
		[System.Obsolete("Use " + nameof(Instruction) + "." + nameof(Instruction.RflagsModified) + " instead", false)]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public readonly RflagsBits RflagsModified => (RflagsBits)RflagsInfoConstants.flagsModified[rflagsInfo];
	}
}
#endif
