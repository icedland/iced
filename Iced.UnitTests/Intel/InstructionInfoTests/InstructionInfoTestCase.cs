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

#if !NO_INSTR_INFO
using System.Collections.Generic;
using System.Diagnostics;
using Iced.Intel;

namespace Iced.UnitTests.Intel.InstructionInfoTests {
	public sealed class InstructionInfoTestCase {
		public InstructionInfoTestCase() => Debug.Assert(Iced.Intel.DecoderConstants.MaxOpCount == 5);
		public EncodingKind Encoding = EncodingKind.Legacy;
		public CpuidFeature CpuidFeature = CpuidFeature.INTEL8086;
		public RflagsBits RflagsRead = RflagsBits.None;
		public RflagsBits RflagsUndefined = RflagsBits.None;
		public RflagsBits RflagsWritten = RflagsBits.None;
		public RflagsBits RflagsCleared = RflagsBits.None;
		public RflagsBits RflagsSet = RflagsBits.None;
		public int StackPointerIncrement = 0;
		public bool Privileged = false;
		public bool ProtectedMode = false;
		public bool StackInstruction = false;
		public bool SaveRestoreInstruction = false;
		public readonly List<UsedRegister> UsedRegisters = new List<UsedRegister>();
		public readonly List<UsedMemory> UsedMemory = new List<UsedMemory>();
		public FlowControl FlowControl = FlowControl.Next;
		public OpAccess Op0Access = OpAccess.None;
		public OpAccess Op1Access = OpAccess.None;
		public OpAccess Op2Access = OpAccess.None;
		public OpAccess Op3Access = OpAccess.None;
		public OpAccess Op4Access = OpAccess.None;
	}
}
#endif
