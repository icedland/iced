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
using Generator.Enums.Decoder;

namespace Generator.Enums.Encoder {
	[Enum("EncFlags2", Flags = true, NoInitialize = true)]
	enum EncFlags2 : uint {
		None					= 0,
		OpCodeShift				= 0,
		// [15:0] = opcode (1 or 2 bytes)
		OpCodeIs2Bytes			= 0x00010000,
		/// <summary>
		/// <see cref="LegacyOpCodeTable"/>
		/// <see cref="VexOpCodeTable"/>
		/// <see cref="XopOpCodeTable"/>
		/// <see cref="EvexOpCodeTable"/>
		/// </summary>
		TableShift				= 17,
		TableMask				= 3,
		/// <summary><see cref="MandatoryPrefixByte"/></summary>
		MandatoryPrefixShift	= 19,
		MandatoryPrefixMask		= 3,
		/// <summary><see cref="WBit"/></summary>
		WBitShift				= 21,
		WBitMask				= 3,
		/// <summary><see cref="LBit"/></summary>
		LBitShift				= 23,
		LBitMask				= 7,
		GroupIndexShift			= 26,
		GroupIndexMask			= 7,
		HasMandatoryPrefix		= 0x20000000,
		HasGroupIndex			= 0x40000000,
		HasRmGroupIndex			= 0x80000000,
	}
}
