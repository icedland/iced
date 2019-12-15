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
using Generator.Enums;
using Generator.Enums.Encoder;

namespace Generator.Encoder {
	[Flags]
	enum OpCodeFlags : uint {
		None					= 0,
		Mode16					= 0x00000001,
		Mode32					= 0x00000002,
		Mode64					= 0x00000004,
		Fwait					= 0x00000008,
		LIG						= 0x00000010,
		WIG						= 0x00000020,
		WIG32					= 0x00000040,
		W						= 0x00000080,
		Broadcast				= 0x00000100,
		RoundingControl			= 0x00000200,
		SuppressAllExceptions	= 0x00000400,
		OpMaskRegister			= 0x00000800,
		ZeroingMasking			= 0x00001000,
		LockPrefix				= 0x00002000,
		XacquirePrefix			= 0x00004000,
		XreleasePrefix			= 0x00008000,
		RepPrefix				= 0x00010000,
		RepnePrefix				= 0x00020000,
		BndPrefix				= 0x00040000,
		HintTakenPrefix			= 0x00080000,
		NotrackPrefix			= 0x00100000,
		NoInstruction			= 0x00200000,
		NonZeroOpMaskRegister	= 0x00400000,
	}

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
	abstract class OpCodeInfo {
		public abstract EncodingKind Encoding { get; }
		public EnumValue Code { get; protected set; }
		public MandatoryPrefix MandatoryPrefix { get; protected set; }
		public OpCodeTableKind Table { get; protected set; }
		public uint OpCode { get; protected set; }
		public int GroupIndex { get; protected set; }
		public OpCodeFlags Flags { get; protected set; }
	}
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

	sealed class LegacyOpCodeInfo : OpCodeInfo {
		public override EncodingKind Encoding => EncodingKind.Legacy;
		public OperandSize OperandSize { get; }
		public AddressSize AddressSize { get; }
		public LegacyOpKind[] OpKinds { get; }
		public LegacyOpCodeInfo(EnumValue code, MandatoryPrefix mandatoryPrefix, OpCodeTableKind table, uint opCode, int groupIndex, OperandSize operandSize, AddressSize addressSize, OpCodeFlags flags, LegacyOpKind[] opKinds) {
			Code = code;
			MandatoryPrefix = mandatoryPrefix;
			Table = table;
			OpCode = opCode;
			GroupIndex = groupIndex;
			Flags = flags;
			OperandSize = operandSize;
			AddressSize = addressSize;
			OpKinds = opKinds;
		}
	}

	sealed class VexOpCodeInfo : OpCodeInfo {
		public override EncodingKind Encoding => EncodingKind.VEX;
		public VexVectorLength VectorLength { get; }
		public VexOpKind[] OpKinds { get; }
		public VexOpCodeInfo(EnumValue code, MandatoryPrefix mandatoryPrefix, OpCodeTableKind table, uint opCode, int groupIndex, VexVectorLength vecLen, OpCodeFlags flags, VexOpKind[] opKinds) {
			Code = code;
			MandatoryPrefix = mandatoryPrefix;
			Table = table;
			OpCode = opCode;
			GroupIndex = groupIndex;
			Flags = flags;
			VectorLength = vecLen;
			OpKinds = opKinds;
		}
	}

	sealed class XopOpCodeInfo : OpCodeInfo {
		public override EncodingKind Encoding => EncodingKind.XOP;
		public XopVectorLength VectorLength { get; }
		public XopOpKind[] OpKinds { get; }
		public XopOpCodeInfo(EnumValue code, MandatoryPrefix mandatoryPrefix, OpCodeTableKind table, uint opCode, int groupIndex, XopVectorLength vecLen, OpCodeFlags flags, XopOpKind[] opKinds) {
			Code = code;
			MandatoryPrefix = mandatoryPrefix;
			Table = table;
			OpCode = opCode;
			GroupIndex = groupIndex;
			Flags = flags;
			VectorLength = vecLen;
			OpKinds = opKinds;
		}
	}

	sealed class EvexOpCodeInfo : OpCodeInfo {
		public override EncodingKind Encoding => EncodingKind.EVEX;
		public EvexVectorLength VectorLength { get; }
		public TupleType TupleType { get; }
		public EvexOpKind[] OpKinds { get; }
		public EvexOpCodeInfo(EnumValue code, MandatoryPrefix mandatoryPrefix, OpCodeTableKind table, uint opCode, int groupIndex, EvexVectorLength vecLen, TupleType tupleType, OpCodeFlags flags, EvexOpKind[] opKinds) {
			Code = code;
			MandatoryPrefix = mandatoryPrefix;
			Table = table;
			OpCode = opCode;
			GroupIndex = groupIndex;
			Flags = flags;
			VectorLength = vecLen;
			OpKinds = opKinds;
			TupleType = tupleType;
		}
	}

	sealed class D3nowOpCodeInfo : OpCodeInfo {
		public override EncodingKind Encoding => EncodingKind.D3NOW;
		public uint Immediate8 { get; }
		public D3nowOpCodeInfo(EnumValue code, uint immediate8, OpCodeFlags flags) {
			Code = code;
			MandatoryPrefix = MandatoryPrefix.None;
			Table = OpCodeTableKind.T0F;
			OpCode = 0x0F;
			GroupIndex = -1;
			Flags = flags;
			Immediate8 = immediate8;
		}
	}
}
