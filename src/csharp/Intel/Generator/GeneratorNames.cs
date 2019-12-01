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

namespace Generator {
	static class GeneratorNames {
		/// <summary>Generates the decoder tables</summary>
		public const string Decoder_Table = "decoder/table";

		/// <summary>Generates the Code -> MemorySize table</summary>
		public const string Code_MemorySize = "code/memsize";

		/// <summary>Generates the Code -> op count table</summary>
		public const string Code_OpCount = "code/opcount";

		/// <summary>Generates the Code -> Mnemonic table</summary>
		public const string Code_Mnemonic = "code/mnemonic";

		/// <summary>Generates the formatter tables</summary>
		public const string Formatter_Table = "formatter/table";

		/// <summary>Generates enums</summary>
		public const string Enums = "enum";

		/// <summary>Generates enum hash table (string -> enum value lookup used by unit tests)</summary>
		public const string Enums_Table = "enum/hash";

		/// <summary>Generates constants</summary>
		public const string Constants = "const";

		/// <summary>Generates the MemorySizeInfo table</summary>
		public const string MemorySizeInfo_Table = "memsize/info";

		/// <summary>Generates the RegisterInfo table</summary>
		public const string RegisterInfo_Table = "register/info";

		/// <summary>Generates the 3DNow! table</summary>
		public const string D3now_Table = "3dnow";

		/// <summary>Generates instr info enums, constants and tables</summary>
		public const string InstrInfo = "info";
	}
}
