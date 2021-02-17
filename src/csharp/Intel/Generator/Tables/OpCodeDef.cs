// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using Generator.Enums;
using Generator.Enums.Encoder;

namespace Generator.Tables {
	enum OpCodeW : byte {
		None,
		W0,
		W1,
		WIG,
		WIG32,
	}

	enum OpCodeL : byte {
		None,
		L0,
		L1,
		LIG,
		LZ,
		L128,
		L256,
		L512,
	}

	[Flags]
	enum ParsedOpCodeFlags : byte {
		None			= 0,
		Fwait			= 0x01,
		ModRegRmString	= 0x02,
		Is4				= 0x04,
		Is5				= 0x08,
		Vsib			= 0x10,
	}

	struct OpCodeDef {
		public EncodingKind Encoding;
		public MandatoryPrefix MandatoryPrefix;
		public OpCodeW WBit;
		public OpCodeL LBit;
		public OpCodeTableKind Table;
		public uint OpCode;
		public int OpCodeLength;
		public sbyte GroupIndex;
		public sbyte RmGroupIndex;
		public CodeSize OperandSize;
		public CodeSize AddressSize;
		public ParsedOpCodeFlags Flags;

		public static OpCodeDef CreateDefault(EncodingKind encoding) =>
			new OpCodeDef {
				Encoding = encoding,
				MandatoryPrefix = MandatoryPrefix.None,
				WBit = OpCodeW.None,
				LBit = OpCodeL.None,
				Table = OpCodeTableKind.Normal,
				OpCode = 0,
				GroupIndex = -1,
				RmGroupIndex = -1,
				OperandSize = 0,
				AddressSize = 0,
				Flags = ParsedOpCodeFlags.None,
			};
	}
}
