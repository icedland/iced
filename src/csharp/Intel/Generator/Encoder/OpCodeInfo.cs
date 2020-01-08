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
using System.Diagnostics;
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

		public abstract int OpKindsLength { get; }

		public abstract CommonOpKind OpKind(int arg); 

		public override string ToString() => $"{this.GetType().Name}: {Code.RawName}";
	}
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

	sealed class LegacyOpCodeInfo : OpCodeInfo {
		public override EncodingKind Encoding => EncodingKind.Legacy;
		public OperandSize OperandSize { get; }
		public AddressSize AddressSize { get; }
		public LegacyOpKind[] OpKinds { get; }

		public override int OpKindsLength => OpKinds.Length;

		public override CommonOpKind OpKind(int arg) {
			var kind = OpKinds[arg];
			switch (kind) {
			case LegacyOpKind.None: return CommonOpKind.None;
			case LegacyOpKind.Aww: return CommonOpKind.Aww;
			case LegacyOpKind.Adw: return CommonOpKind.Adw;
			case LegacyOpKind.M: return CommonOpKind.M;
			case LegacyOpKind.Mfbcd: return CommonOpKind.Mfbcd;
			case LegacyOpKind.Mf32: return CommonOpKind.Mf32;
			case LegacyOpKind.Mf64: return CommonOpKind.Mf64;
			case LegacyOpKind.Mf80: return CommonOpKind.Mf80;
			case LegacyOpKind.Mfi16: return CommonOpKind.Mfi16;
			case LegacyOpKind.Mfi32: return CommonOpKind.Mfi32;
			case LegacyOpKind.Mfi64: return CommonOpKind.Mfi64;
			case LegacyOpKind.M14: return CommonOpKind.M14;
			case LegacyOpKind.M28: return CommonOpKind.M28;
			case LegacyOpKind.M98: return CommonOpKind.M98;
			case LegacyOpKind.M108: return CommonOpKind.M108;
			case LegacyOpKind.Mp: return CommonOpKind.Mp;
			case LegacyOpKind.Ms: return CommonOpKind.Ms;
			case LegacyOpKind.Mo: return CommonOpKind.Mo;
			case LegacyOpKind.Mb: return CommonOpKind.Mb;
			case LegacyOpKind.Mw: return CommonOpKind.Mw;
			case LegacyOpKind.Md: return CommonOpKind.Md;
			case LegacyOpKind.Md_MPX: return CommonOpKind.Md_MPX;
			case LegacyOpKind.Mq: return CommonOpKind.Mq;
			case LegacyOpKind.Mq_MPX: return CommonOpKind.Mq_MPX;
			case LegacyOpKind.Mw2: return CommonOpKind.Mw2;
			case LegacyOpKind.Md2: return CommonOpKind.Md2;
			case LegacyOpKind.Eb: return CommonOpKind.Eb;
			case LegacyOpKind.Ew: return CommonOpKind.Ew;
			case LegacyOpKind.Ed: return CommonOpKind.Ed;
			case LegacyOpKind.Ed_MPX: return CommonOpKind.Ed_MPX;
			case LegacyOpKind.Ew_d: return CommonOpKind.Ew_d;
			case LegacyOpKind.Ew_q: return CommonOpKind.Ew_q;
			case LegacyOpKind.Eq: return CommonOpKind.Eq;
			case LegacyOpKind.Eq_MPX: return CommonOpKind.Eq_MPX;
			case LegacyOpKind.Eww: return CommonOpKind.Eww;
			case LegacyOpKind.Edw: return CommonOpKind.Edw;
			case LegacyOpKind.Eqw: return CommonOpKind.Eqw;
			case LegacyOpKind.RdMb: return CommonOpKind.RdMb;
			case LegacyOpKind.RqMb: return CommonOpKind.RqMb;
			case LegacyOpKind.RdMw: return CommonOpKind.RdMw;
			case LegacyOpKind.RqMw: return CommonOpKind.RqMw;
			case LegacyOpKind.Gb: return CommonOpKind.Gb;
			case LegacyOpKind.Gw: return CommonOpKind.Gw;
			case LegacyOpKind.Gd: return CommonOpKind.Gd;
			case LegacyOpKind.Gq: return CommonOpKind.Gq;
			case LegacyOpKind.Rw: return CommonOpKind.Rw;
			case LegacyOpKind.Rd: return CommonOpKind.Rd;
			case LegacyOpKind.Rq: return CommonOpKind.Rq;
			case LegacyOpKind.Sw: return CommonOpKind.Sw;
			case LegacyOpKind.Cd: return CommonOpKind.Cd;
			case LegacyOpKind.Cq: return CommonOpKind.Cq;
			case LegacyOpKind.Dd: return CommonOpKind.Dd;
			case LegacyOpKind.Dq: return CommonOpKind.Dq;
			case LegacyOpKind.Td: return CommonOpKind.Td;
			case LegacyOpKind.Ib: return CommonOpKind.Ib;
			case LegacyOpKind.Ib16: return CommonOpKind.Ib16;
			case LegacyOpKind.Ib32: return CommonOpKind.Ib32;
			case LegacyOpKind.Ib64: return CommonOpKind.Ib64;
			case LegacyOpKind.Iw: return CommonOpKind.Iw;
			case LegacyOpKind.Id: return CommonOpKind.Id;
			case LegacyOpKind.Id64: return CommonOpKind.Id64;
			case LegacyOpKind.Iq: return CommonOpKind.Iq;
			case LegacyOpKind.Ib21: return CommonOpKind.Ib21;
			case LegacyOpKind.Ib11: return CommonOpKind.Ib11;
			case LegacyOpKind.Xb: return CommonOpKind.Xb;
			case LegacyOpKind.Xw: return CommonOpKind.Xw;
			case LegacyOpKind.Xd: return CommonOpKind.Xd;
			case LegacyOpKind.Xq: return CommonOpKind.Xq;
			case LegacyOpKind.Yb: return CommonOpKind.Yb;
			case LegacyOpKind.Yw: return CommonOpKind.Yw;
			case LegacyOpKind.Yd: return CommonOpKind.Yd;
			case LegacyOpKind.Yq: return CommonOpKind.Yq;
			case LegacyOpKind.wJb: return CommonOpKind.wJb;
			case LegacyOpKind.dJb: return CommonOpKind.dJb;
			case LegacyOpKind.qJb: return CommonOpKind.qJb;
			case LegacyOpKind.Jw: return CommonOpKind.Jw;
			case LegacyOpKind.wJd: return CommonOpKind.wJd;
			case LegacyOpKind.dJd: return CommonOpKind.dJd;
			case LegacyOpKind.qJd: return CommonOpKind.qJd;
			case LegacyOpKind.Jxw: return CommonOpKind.Jxw;
			case LegacyOpKind.Jxd: return CommonOpKind.Jxd;
			case LegacyOpKind.Jdisp16: return CommonOpKind.Jdisp16;
			case LegacyOpKind.Jdisp32: return CommonOpKind.Jdisp32;
			case LegacyOpKind.Ob: return CommonOpKind.Ob;
			case LegacyOpKind.Ow: return CommonOpKind.Ow;
			case LegacyOpKind.Od: return CommonOpKind.Od;
			case LegacyOpKind.Oq: return CommonOpKind.Oq;
			case LegacyOpKind.Imm1: return CommonOpKind.Imm1;
			case LegacyOpKind.B: return CommonOpKind.B;
			case LegacyOpKind.BMq: return CommonOpKind.BMq;
			case LegacyOpKind.BMo: return CommonOpKind.BMo;
			case LegacyOpKind.MIB: return CommonOpKind.MIB;
			case LegacyOpKind.N: return CommonOpKind.N;
			case LegacyOpKind.P: return CommonOpKind.P;
			case LegacyOpKind.Q: return CommonOpKind.Q;
			case LegacyOpKind.RX: return CommonOpKind.RX;
			case LegacyOpKind.VX: return CommonOpKind.VX;
			case LegacyOpKind.WX: return CommonOpKind.WX;
			case LegacyOpKind.rDI: return CommonOpKind.rDI;
			case LegacyOpKind.MRBX: return CommonOpKind.MRBX;
			case LegacyOpKind.ES: return CommonOpKind.ES;
			case LegacyOpKind.CS: return CommonOpKind.CS;
			case LegacyOpKind.SS: return CommonOpKind.SS;
			case LegacyOpKind.DS: return CommonOpKind.DS;
			case LegacyOpKind.FS: return CommonOpKind.FS;
			case LegacyOpKind.GS: return CommonOpKind.GS;
			case LegacyOpKind.AL: return CommonOpKind.AL;
			case LegacyOpKind.CL: return CommonOpKind.CL;
			case LegacyOpKind.AX: return CommonOpKind.AX;
			case LegacyOpKind.DX: return CommonOpKind.DX;
			case LegacyOpKind.EAX: return CommonOpKind.EAX;
			case LegacyOpKind.RAX: return CommonOpKind.RAX;
			case LegacyOpKind.ST: return CommonOpKind.ST;
			case LegacyOpKind.STi: return CommonOpKind.STi;
			case LegacyOpKind.r8_rb: return CommonOpKind.r8_rb;
			case LegacyOpKind.r16_rw: return CommonOpKind.r16_rw;
			case LegacyOpKind.r32_rd: return CommonOpKind.r32_rd;
			case LegacyOpKind.r64_ro: return CommonOpKind.r64_ro;
			default:
				throw new ArgumentOutOfRangeException($"{kind}");
			}
		} 
		
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

		public override int OpKindsLength => OpKinds.Length;

		public override CommonOpKind OpKind(int arg) {
			var kind = OpKinds[arg];
			switch (kind) {
			case VexOpKind.None: return CommonOpKind.None;
			case VexOpKind.Ed: return CommonOpKind.Ed;
			case VexOpKind.Eq: return CommonOpKind.Eq;
			case VexOpKind.Gd: return CommonOpKind.Gd;
			case VexOpKind.Gq: return CommonOpKind.Gq;
			case VexOpKind.RdMb: return CommonOpKind.RdMb;
			case VexOpKind.RqMb: return CommonOpKind.RqMb;
			case VexOpKind.RdMw: return CommonOpKind.RdMw;
			case VexOpKind.RqMw: return CommonOpKind.RqMw;
			case VexOpKind.Rd: return CommonOpKind.Rd;
			case VexOpKind.Rq: return CommonOpKind.Rq;
			case VexOpKind.Hd: return CommonOpKind.Hd;
			case VexOpKind.Hq: return CommonOpKind.Hq;
			case VexOpKind.HK: return CommonOpKind.HK;
			case VexOpKind.HX: return CommonOpKind.HX;
			case VexOpKind.HY: return CommonOpKind.HY;
			case VexOpKind.Ib: return CommonOpKind.Ib;
			case VexOpKind.I2: return CommonOpKind.I2;
			case VexOpKind.Is4X: return CommonOpKind.Is4X;
			case VexOpKind.Is4Y: return CommonOpKind.Is4Y;
			case VexOpKind.Is5X: return CommonOpKind.Is5X;
			case VexOpKind.Is5Y: return CommonOpKind.Is5Y;
			case VexOpKind.M: return CommonOpKind.M;
			case VexOpKind.Md: return CommonOpKind.Md;
			case VexOpKind.MK: return CommonOpKind.MK;
			case VexOpKind.rDI: return CommonOpKind.rDI;
			case VexOpKind.RK: return CommonOpKind.RK;
			case VexOpKind.RX: return CommonOpKind.RX;
			case VexOpKind.RY: return CommonOpKind.RY;
			case VexOpKind.VK: return CommonOpKind.VK;
			case VexOpKind.VM32X: return CommonOpKind.VM32X;
			case VexOpKind.VM32Y: return CommonOpKind.VM32Y;
			case VexOpKind.VM64X: return CommonOpKind.VM64X;
			case VexOpKind.VM64Y: return CommonOpKind.VM64Y;
			case VexOpKind.VX: return CommonOpKind.VX;
			case VexOpKind.VY: return CommonOpKind.VY;
			case VexOpKind.WK: return CommonOpKind.WK;
			case VexOpKind.WX: return CommonOpKind.WX;
			case VexOpKind.WY: return CommonOpKind.WY;
			default:
				throw new ArgumentOutOfRangeException($"{kind}");
			}
		}
		
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
		
		public override int OpKindsLength => OpKinds.Length;

		public override CommonOpKind OpKind(int arg) {
			var kind = OpKinds[arg];
			switch (kind) {
			case XopOpKind.None: return CommonOpKind.None;
			case XopOpKind.Ed: return CommonOpKind.Ed;
			case XopOpKind.Eq: return CommonOpKind.Eq;
			case XopOpKind.Gd: return CommonOpKind.Gd;
			case XopOpKind.Gq: return CommonOpKind.Gq;
			case XopOpKind.Rd: return CommonOpKind.Rd;
			case XopOpKind.Rq: return CommonOpKind.Rq;
			case XopOpKind.Hd: return CommonOpKind.Hd;
			case XopOpKind.Hq: return CommonOpKind.Hq;
			case XopOpKind.HX: return CommonOpKind.HX;
			case XopOpKind.HY: return CommonOpKind.HY;
			case XopOpKind.Ib: return CommonOpKind.Ib;
			case XopOpKind.Id: return CommonOpKind.Id;
			case XopOpKind.Is4X: return CommonOpKind.Is4X;
			case XopOpKind.Is4Y: return CommonOpKind.Is4Y;
			case XopOpKind.VX: return CommonOpKind.VX;
			case XopOpKind.VY: return CommonOpKind.VY;
			case XopOpKind.WX: return CommonOpKind.WX;
			case XopOpKind.WY: return CommonOpKind.WY;
			default:
				throw new ArgumentOutOfRangeException($"{kind}");
			}
		} 
		
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
		
		public override int OpKindsLength => OpKinds.Length;

		public override CommonOpKind OpKind(int arg) {
			var kind = OpKinds[arg];
			switch (kind) {
			case EvexOpKind.None: return CommonOpKind.None;
			case EvexOpKind.Ed: return CommonOpKind.Ed;
			case EvexOpKind.Eq: return CommonOpKind.Eq;
			case EvexOpKind.Gd: return CommonOpKind.Gd;
			case EvexOpKind.Gq: return CommonOpKind.Gq;
			case EvexOpKind.RdMb: return CommonOpKind.RdMb;
			case EvexOpKind.RqMb: return CommonOpKind.RqMb;
			case EvexOpKind.RdMw: return CommonOpKind.RdMw;
			case EvexOpKind.RqMw: return CommonOpKind.RqMw;
			case EvexOpKind.HX: return CommonOpKind.HX;
			case EvexOpKind.HY: return CommonOpKind.HY;
			case EvexOpKind.HZ: return CommonOpKind.HZ;
			case EvexOpKind.HXP3: return CommonOpKind.HXP3;
			case EvexOpKind.HZP3: return CommonOpKind.HZP3;
			case EvexOpKind.Ib: return CommonOpKind.Ib;
			case EvexOpKind.M: return CommonOpKind.M;
			case EvexOpKind.Rd: return CommonOpKind.Rd;
			case EvexOpKind.Rq: return CommonOpKind.Rq;
			case EvexOpKind.RX: return CommonOpKind.RX;
			case EvexOpKind.RY: return CommonOpKind.RY;
			case EvexOpKind.RZ: return CommonOpKind.RZ;
			case EvexOpKind.RK: return CommonOpKind.RK;
			case EvexOpKind.VM32X: return CommonOpKind.VM32X;
			case EvexOpKind.VM32Y: return CommonOpKind.VM32Y;
			case EvexOpKind.VM32Z: return CommonOpKind.VM32Z;
			case EvexOpKind.VM64X: return CommonOpKind.VM64X;
			case EvexOpKind.VM64Y: return CommonOpKind.VM64Y;
			case EvexOpKind.VM64Z: return CommonOpKind.VM64Z;
			case EvexOpKind.VK: return CommonOpKind.VK;
			case EvexOpKind.VKP1: return CommonOpKind.VKP1;
			case EvexOpKind.VX: return CommonOpKind.VX;
			case EvexOpKind.VY: return CommonOpKind.VY;
			case EvexOpKind.VZ: return CommonOpKind.VZ;
			case EvexOpKind.WX: return CommonOpKind.WX;
			case EvexOpKind.WY: return CommonOpKind.WY;
			case EvexOpKind.WZ: return CommonOpKind.WZ;
			default:
				throw new ArgumentOutOfRangeException($"{kind}");
			}
		} 
		
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
		
		public override int OpKindsLength => 0;

		public override CommonOpKind OpKind(int arg) => throw new ArgumentOutOfRangeException(); 
		
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
