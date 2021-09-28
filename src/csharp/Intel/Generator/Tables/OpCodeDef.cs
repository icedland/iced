// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using Generator.Enums;
using Generator.Enums.Encoder;

namespace Generator.Tables {
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
		public NonDestructiveOpKind NDKind;
		public MvexEHBit MvexEHBit;
		public uint OpCode;
		public int OpCodeLength;
		public sbyte GroupIndex;
		public sbyte RmGroupIndex;
		public CodeSize OperandSize;
		public CodeSize AddressSize;
		public ParsedOpCodeFlags Flags;

		public static OpCodeDef CreateDefault(EncodingKind encoding) =>
			new() {
				Encoding = encoding,
				MandatoryPrefix = MandatoryPrefix.None,
				WBit = OpCodeW.None,
				LBit = OpCodeL.None,
				Table = OpCodeTableKind.Normal,
				NDKind = NonDestructiveOpKind.None,
				MvexEHBit = MvexEHBit.None,
				OpCode = 0,
				GroupIndex = -1,
				RmGroupIndex = -1,
				OperandSize = 0,
				AddressSize = 0,
				Flags = ParsedOpCodeFlags.None,
			};
	}
}
