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

#if !NO_INSTR_INFO
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Iced.Intel;

namespace Iced.UnitTests.Intel.InstructionInfoTests {
	public sealed class InstructionInfoTestCase {
		public InstructionInfoTestCase() => Debug.Assert(Iced.Intel.DecoderConstants.MaxOpCount == 5);
		public EncodingKind Encoding = EncodingKind.Legacy;
		public CpuidFeature[] CpuidFeatures = Array.Empty<CpuidFeature>();
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
