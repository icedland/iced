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

#if ENCODER && OPCODE_INFO
using Iced.Intel;

namespace Iced.UnitTests.Intel.EncoderTests {
	sealed class OpCodeInfoTestCase {
		public int LineNumber = -1;
		public Code Code = Code.INVALID;
		public string OpCodeString = string.Empty;
		public string InstructionString = string.Empty;
		public EncodingKind Encoding = EncodingKind.Legacy;
		public bool IsInstruction;
		public bool Mode16;
		public bool Mode32;
		public bool Mode64;
		public bool Fwait;
		public int OperandSize;
		public int AddressSize;
		public uint L;
		public uint W;
		public bool IsLIG;
		public bool IsWIG;
		public bool IsWIG32;
		public TupleType TupleType = TupleType.N1;
		public bool CanBroadcast;
		public bool CanUseRoundingControl;
		public bool CanSuppressAllExceptions;
		public bool CanUseOpMaskRegister;
		public bool RequireOpMaskRegister;
		public bool CanUseZeroingMasking;
		public bool CanUseLockPrefix;
		public bool CanUseXacquirePrefix;
		public bool CanUseXreleasePrefix;
		public bool CanUseRepPrefix;
		public bool CanUseRepnePrefix;
		public bool CanUseBndPrefix;
		public bool CanUseHintTakenPrefix;
		public bool CanUseNotrackPrefix;
		public OpCodeTableKind Table = OpCodeTableKind.Normal;
		public MandatoryPrefix MandatoryPrefix = MandatoryPrefix.None;
		public uint OpCode = 0;
		public bool IsGroup = false;
		public int GroupIndex = -1;
		public bool IsRmGroup = false;
		public int RmGroupIndex = -1;
		public int OpCount = 0;
		public OpCodeOperandKind Op0Kind = OpCodeOperandKind.None;
		public OpCodeOperandKind Op1Kind = OpCodeOperandKind.None;
		public OpCodeOperandKind Op2Kind = OpCodeOperandKind.None;
		public OpCodeOperandKind Op3Kind = OpCodeOperandKind.None;
		public OpCodeOperandKind Op4Kind = OpCodeOperandKind.None;
	}
}
#endif
