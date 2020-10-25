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

namespace Generator.Constants.InstructionInfo {
	static class MiscSectionNames {
		public const string JccShort = "jcc-short";
		public const string JccNear = "jcc-near";
		public const string JmpShort = "jmp-short";
		public const string JmpNear = "jmp-near";
		public const string JmpFar = "jmp-far";
		public const string JmpNearIndirect = "jmp-near-indirect";
		public const string JmpFarIndirect = "jmp-far-indirect";
		public const string CallNear = "call-near";
		public const string CallFar = "call-far";
		public const string CallNearIndirect = "call-near-indirect";
		public const string CallFarIndirect = "call-far-indirect";
		public const string JmpeNear = "jmpe-near";
		public const string JmpeNearIndirect = "jmpe-near-indirect";
		public const string Loop = "loop";
		public const string Jrcxz = "jrcxz";
		public const string Xbegin = "xbegin";
		public const string JmpInfo = "jmp-info";
		public const string JccShortInfo = "jcc-short-info";
		public const string JccNearInfo = "jcc-near-info";
		public const string SetccInfo = "setcc-info";
		public const string CmovccInfo = "cmovcc-info";
		public const string LoopccInfo = "loopcc-info";
	}
}
