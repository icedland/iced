// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if INSTR_INFO
using System;
using System.Collections.Generic;
using Iced.Intel;

namespace Iced.UnitTests.Intel.InstructionInfoTests {
	public sealed class InstructionInfoTestCase {
		public InstructionInfoTestCase() => Static.Assert(IcedConstants.MaxOpCount == 5 ? 0 : -1);
		public ulong IP = 0;
		public EncodingKind Encoding = EncodingKind.Legacy;
		public CpuidFeature[] CpuidFeatures = Array.Empty<CpuidFeature>();
		public RflagsBits RflagsRead = RflagsBits.None;
		public RflagsBits RflagsUndefined = RflagsBits.None;
		public RflagsBits RflagsWritten = RflagsBits.None;
		public RflagsBits RflagsCleared = RflagsBits.None;
		public RflagsBits RflagsSet = RflagsBits.None;
		public int StackPointerIncrement = 0;
		public bool IsPrivileged = false;
		public bool IsStackInstruction = false;
		public bool IsSaveRestoreInstruction = false;
		public bool IsSpecial = false;
		public readonly List<UsedRegister> UsedRegisters = new List<UsedRegister>();
		public readonly List<UsedMemory> UsedMemory = new List<UsedMemory>();
		public FlowControl FlowControl = FlowControl.Next;
		public OpAccess Op0Access = OpAccess.None;
		public OpAccess Op1Access = OpAccess.None;
		public OpAccess Op2Access = OpAccess.None;
		public OpAccess Op3Access = OpAccess.None;
		public OpAccess Op4Access = OpAccess.None;
		public int FpuTopIncrement = 0;
		public bool FpuConditionalTop = false;
		public bool FpuWritesTop = false;
	}
}
#endif
