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
using System.Collections.Generic;
using Generator.Enums;
using Generator.Enums.Decoder;
using Generator.Enums.Encoder;

namespace Generator.Encoder {
	abstract class EncoderGenerator {
		protected abstract void Generate(EnumType enumType);
		protected abstract void Generate((EnumValue opCodeOperandKind, EnumValue legacyOpKind, OpHandlerKind opHandlerKind, object[] args)[] legacy, (EnumValue opCodeOperandKind, EnumValue vexOpKind, OpHandlerKind opHandlerKind, object[] args)[] vex, (EnumValue opCodeOperandKind, EnumValue xopOpKind, OpHandlerKind opHandlerKind, object[] args)[] xop, (EnumValue opCodeOperandKind, EnumValue evexOpKind, OpHandlerKind opHandlerKind, object[] args)[] evex);
		protected abstract void Generate(OpCodeInfo[] opCodes);
		protected abstract void Generate((EnumValue value, uint size)[] immSizes);
		protected abstract void GenerateCore();

		public void Generate() {
			var enumTypes = new EnumType[] {
				EncoderTypes.EncFlags1,
				EncoderTypes.LegacyFlags3,
				EncoderTypes.VexFlags3,
				EncoderTypes.XopFlags3,
				EncoderTypes.EvexFlags3,
				EncoderTypes.AllowedPrefixes,
				EncoderTypes.LegacyFlags,
				EncoderTypes.VexFlags,
				EncoderTypes.XopFlags,
				EncoderTypes.EvexFlags,
				EncoderTypes.D3nowFlags,
			};
			foreach (var enumType in enumTypes)
				Generate(enumType);

			Generate(EncoderTypes.LegacyOpHandlers, EncoderTypes.VexOpHandlers, EncoderTypes.XopOpHandlers, EncoderTypes.EvexOpHandlers);
			Generate(OpCodeInfoTable.Data);
			Generate(EncoderTypes.ImmSizes);
			GenerateCore();
		}

		static protected IEnumerable<(OpCodeInfo opCode, uint dword1, uint dword2, uint dword3)> GetData(OpCodeInfo[] opCodes) {
			int encodingShift = (int)EncoderTypes.EncFlags1["EncodingShift"].Value;
			int opCodeShift = (int)EncoderTypes.EncFlags1["OpCodeShift"].Value;

			var legacyOpShifts = new[] {
				(int)EncoderTypes.LegacyFlags3["Op0Shift"].Value,
				(int)EncoderTypes.LegacyFlags3["Op1Shift"].Value,
				(int)EncoderTypes.LegacyFlags3["Op2Shift"].Value,
				(int)EncoderTypes.LegacyFlags3["Op3Shift"].Value,
			};
			var vexOpShifts = new[] {
				(int)EncoderTypes.VexFlags3["Op0Shift"].Value,
				(int)EncoderTypes.VexFlags3["Op1Shift"].Value,
				(int)EncoderTypes.VexFlags3["Op2Shift"].Value,
				(int)EncoderTypes.VexFlags3["Op3Shift"].Value,
				(int)EncoderTypes.VexFlags3["Op4Shift"].Value,
			};
			var xopOpShifts = new[] {
				(int)EncoderTypes.XopFlags3["Op0Shift"].Value,
				(int)EncoderTypes.XopFlags3["Op1Shift"].Value,
				(int)EncoderTypes.XopFlags3["Op2Shift"].Value,
				(int)EncoderTypes.XopFlags3["Op3Shift"].Value,
			};
			var evexOpShifts = new[] {
				(int)EncoderTypes.EvexFlags3["Op0Shift"].Value,
				(int)EncoderTypes.EvexFlags3["Op1Shift"].Value,
				(int)EncoderTypes.EvexFlags3["Op2Shift"].Value,
				(int)EncoderTypes.EvexFlags3["Op3Shift"].Value,
			};

			var legacyMandatoryPrefixShift = (int)EncoderTypes.LegacyFlags["MandatoryPrefixByteShift"].Value;
			var legacyOpCodeTableShift = (int)EncoderTypes.LegacyFlags["LegacyOpCodeTableShift"].Value;
			var legacyEncodableShift = (int)EncoderTypes.LegacyFlags["EncodableShift"].Value;
			var legacyHasGroupIndex = EncoderTypes.LegacyFlags["HasGroupIndex"].Value;
			var legacyGroupShift = (int)EncoderTypes.LegacyFlags["GroupShift"].Value;
			var legacyAllowedPrefixesShift = (int)EncoderTypes.LegacyFlags["AllowedPrefixesShift"].Value;
			var legacyFwait = EncoderTypes.LegacyFlags["Fwait"].Value;
			var legacyHasMandatoryPrefix = EncoderTypes.LegacyFlags["HasMandatoryPrefix"].Value;
			var legacyOperandSizeShift = (int)EncoderTypes.LegacyFlags["OperandSizeShift"].Value;
			var legacyAddressSizeShift = (int)EncoderTypes.LegacyFlags["AddressSizeShift"].Value;

			var vexMandatoryPrefixShift = (int)EncoderTypes.VexFlags["MandatoryPrefixByteShift"].Value;
			var vexOpCodeTableShift = (int)EncoderTypes.VexFlags["VexOpCodeTableShift"].Value;
			var vexEncodableShift = (int)EncoderTypes.VexFlags["EncodableShift"].Value;
			var vexHasGroupIndex = EncoderTypes.VexFlags["HasGroupIndex"].Value;
			var vexGroupShift = (int)EncoderTypes.VexFlags["GroupShift"].Value;
			var vexVectorLengthShift = (int)EncoderTypes.VexFlags["VexVectorLengthShift"].Value;
			var vexWBitShift = (int)EncoderTypes.VexFlags["WBitShift"].Value;

			var xopMandatoryPrefixShift = (int)EncoderTypes.XopFlags["MandatoryPrefixByteShift"].Value;
			var xopOpCodeTableShift = (int)EncoderTypes.XopFlags["XopOpCodeTableShift"].Value;
			var xopEncodableShift = (int)EncoderTypes.XopFlags["EncodableShift"].Value;
			var xopHasGroupIndex = EncoderTypes.XopFlags["HasGroupIndex"].Value;
			var xopGroupShift = (int)EncoderTypes.XopFlags["GroupShift"].Value;
			var xopVectorLengthShift = (int)EncoderTypes.XopFlags["XopVectorLengthShift"].Value;
			var xopWBitShift = (int)EncoderTypes.XopFlags["WBitShift"].Value;

			var evexMandatoryPrefixShift = (int)EncoderTypes.EvexFlags["MandatoryPrefixByteShift"].Value;
			var evexOpCodeTableShift = (int)EncoderTypes.EvexFlags["EvexOpCodeTableShift"].Value;
			var evexEncodableShift = (int)EncoderTypes.EvexFlags["EncodableShift"].Value;
			var evexHasGroupIndex = EncoderTypes.EvexFlags["HasGroupIndex"].Value;
			var evexGroupShift = (int)EncoderTypes.EvexFlags["GroupShift"].Value;
			var evexVectorLengthShift = (int)EncoderTypes.EvexFlags["EvexVectorLengthShift"].Value;
			var evexWBitShift = (int)EncoderTypes.EvexFlags["WBitShift"].Value;
			var evexTupleTypeShift = (int)EncoderTypes.EvexFlags["TupleTypeShift"].Value;
			var evex_LIG = EncoderTypes.EvexFlags["LIG"].Value;
			var evex_b = EncoderTypes.EvexFlags["b"].Value;
			var evex_er = EncoderTypes.EvexFlags["er"].Value;
			var evex_sae = EncoderTypes.EvexFlags["sae"].Value;
			var evex_k1 = EncoderTypes.EvexFlags["k1"].Value;
			var evex_z = EncoderTypes.EvexFlags["z"].Value;

			var d3nowEncodableShift = (int)EncoderTypes.D3nowFlags["EncodableShift"].Value;

			foreach (var opCode in opCodes) {
				uint dword1, dword2, dword3;

				dword1 = (uint)opCode.Encoding << encodingShift;

				switch (opCode.Encoding) {
				case EncodingKind.Legacy:
					var linfo = (LegacyOpCodeInfo)opCode;

					dword1 |= opCode.OpCode << opCodeShift;

					dword2 = 0;
					dword2 |= (uint)GetMandatoryPrefixByte(opCode.MandatoryPrefix) << legacyMandatoryPrefixShift;
					dword2 |= (uint)GetLegacyTable(linfo.Table) << legacyOpCodeTableShift;
					dword2 |= (uint)GetEncodable(opCode) << legacyEncodableShift;
					if (opCode.GroupIndex >= 0) {
						dword2 |= legacyHasGroupIndex;
						dword2 |= (uint)opCode.GroupIndex << legacyGroupShift;
					}
					dword2 |= (uint)GetAllowedPrefixes(opCode) << legacyAllowedPrefixesShift;
					if ((opCode.Flags & OpCodeFlags.Fwait) != 0)
						dword2 |= legacyFwait;
					if (opCode.MandatoryPrefix != MandatoryPrefix.None)
						dword2 |= legacyHasMandatoryPrefix;
					dword2 |= (uint)linfo.OperandSize << legacyOperandSizeShift;
					dword2 |= (uint)linfo.AddressSize << legacyAddressSizeShift;

					dword3 = 0;
					for (int i = 0; i < linfo.OpKinds.Length; i++)
						dword3 |= (uint)linfo.OpKinds[i] << legacyOpShifts[i];
					break;

				case EncodingKind.VEX:
					var vinfo = (VexOpCodeInfo)opCode;

					dword1 |= opCode.OpCode << opCodeShift;

					dword2 = 0;
					dword2 |= (uint)GetMandatoryPrefixByte(opCode.MandatoryPrefix) << vexMandatoryPrefixShift;
					dword2 |= (uint)GetVexTable(vinfo.Table) << vexOpCodeTableShift;
					dword2 |= (uint)GetEncodable(opCode) << vexEncodableShift;
					if (opCode.GroupIndex >= 0) {
						dword2 |= vexHasGroupIndex;
						dword2 |= (uint)opCode.GroupIndex << vexGroupShift;
					}
					dword2 |= (uint)vinfo.VectorLength << vexVectorLengthShift;
					dword2 |= (uint)GetWBit(opCode) << vexWBitShift;

					dword3 = 0;
					for (int i = 0; i < vinfo.OpKinds.Length; i++)
						dword3 |= (uint)vinfo.OpKinds[i] << vexOpShifts[i];
					break;

				case EncodingKind.EVEX:
					var einfo = (EvexOpCodeInfo)opCode;

					dword1 |= opCode.OpCode << opCodeShift;

					dword2 = 0;
					dword2 |= (uint)GetMandatoryPrefixByte(opCode.MandatoryPrefix) << evexMandatoryPrefixShift;
					dword2 |= (uint)GetEvexTable(einfo.Table) << evexOpCodeTableShift;
					dword2 |= (uint)GetEncodable(opCode) << evexEncodableShift;
					if (opCode.GroupIndex >= 0) {
						dword2 |= evexHasGroupIndex;
						dword2 |= (uint)opCode.GroupIndex << evexGroupShift;
					}
					dword2 |= (uint)einfo.VectorLength << evexVectorLengthShift;
					dword2 |= (uint)GetWBit(opCode) << evexWBitShift;
					dword2 |= (uint)einfo.TupleType << evexTupleTypeShift;
					if ((opCode.Flags & OpCodeFlags.LIG) != 0)
						dword2 |= evex_LIG;
					if ((opCode.Flags & OpCodeFlags.Broadcast) != 0)
						dword2 |= evex_b;
					if ((opCode.Flags & OpCodeFlags.RoundingControl) != 0)
						dword2 |= evex_er;
					if ((opCode.Flags & OpCodeFlags.SuppressAllExceptions) != 0)
						dword2 |= evex_sae;
					if ((opCode.Flags & OpCodeFlags.OpMaskRegister) != 0)
						dword2 |= evex_k1;
					if ((opCode.Flags & OpCodeFlags.ZeroingMasking) != 0)
						dword2 |= evex_z;

					dword3 = 0;
					for (int i = 0; i < einfo.OpKinds.Length; i++)
						dword3 |= (uint)einfo.OpKinds[i] << evexOpShifts[i];
					break;

				case EncodingKind.XOP:
					var xinfo = (XopOpCodeInfo)opCode;

					dword1 |= opCode.OpCode << opCodeShift;

					dword2 = 0;
					dword2 |= (uint)GetMandatoryPrefixByte(opCode.MandatoryPrefix) << xopMandatoryPrefixShift;
					dword2 |= (uint)GetXopTable(xinfo.Table) << xopOpCodeTableShift;
					dword2 |= (uint)GetEncodable(opCode) << xopEncodableShift;
					if (opCode.GroupIndex >= 0) {
						dword2 |= xopHasGroupIndex;
						dword2 |= (uint)opCode.GroupIndex << xopGroupShift;
					}
					dword2 |= (uint)xinfo.VectorLength << xopVectorLengthShift;
					dword2 |= (uint)GetWBit(opCode) << xopWBitShift;

					dword3 = 0;
					for (int i = 0; i < xinfo.OpKinds.Length; i++)
						dword3 |= (uint)xinfo.OpKinds[i] << xopOpShifts[i];
					break;

				case EncodingKind.D3NOW:
					var dinfo = (D3nowOpCodeInfo)opCode;

					dword1 |= dinfo.Immediate8 << opCodeShift;

					dword2 = 0;
					dword2 |= (uint)GetEncodable(opCode) << d3nowEncodableShift;

					dword3 = 0;
					break;

				default:
					throw new InvalidOperationException();
				}

				yield return (opCode, dword1, dword2, dword3);
			}
		}

		static MandatoryPrefixByte GetMandatoryPrefixByte(MandatoryPrefix mandatoryPrefix) =>
			mandatoryPrefix switch {
				MandatoryPrefix.None => MandatoryPrefixByte.None,
				MandatoryPrefix.PNP => MandatoryPrefixByte.None,
				MandatoryPrefix.P66 => MandatoryPrefixByte.P66,
				MandatoryPrefix.PF3 => MandatoryPrefixByte.PF3,
				MandatoryPrefix.PF2 => MandatoryPrefixByte.PF2,
				_ => throw new InvalidOperationException(),
			};

		static Encodable GetEncodable(OpCodeInfo opCode) =>
			(opCode.Flags & (OpCodeFlags.Mode16 | OpCodeFlags.Mode32 | OpCodeFlags.Mode64)) switch {
				OpCodeFlags.Mode16 | OpCodeFlags.Mode32 | OpCodeFlags.Mode64 => Encodable.Any,
				OpCodeFlags.Mode16 | OpCodeFlags.Mode32 => Encodable.Only1632,
				OpCodeFlags.Mode64 => Encodable.Only64,
				_ => throw new InvalidOperationException(),
			};

		static LegacyOpCodeTable GetLegacyTable(OpCodeTableKind table) =>
			table switch {
				OpCodeTableKind.Normal => LegacyOpCodeTable.Normal,
				OpCodeTableKind.T0F => LegacyOpCodeTable.Table0F,
				OpCodeTableKind.T0F38 => LegacyOpCodeTable.Table0F38,
				OpCodeTableKind.T0F3A => LegacyOpCodeTable.Table0F3A,
				_ => throw new InvalidOperationException(),
			};

		static VexOpCodeTable GetVexTable(OpCodeTableKind table) =>
			table switch {
				OpCodeTableKind.T0F => VexOpCodeTable.Table0F,
				OpCodeTableKind.T0F38 => VexOpCodeTable.Table0F38,
				OpCodeTableKind.T0F3A => VexOpCodeTable.Table0F3A,
				_ => throw new InvalidOperationException(),
			};

		static EvexOpCodeTable GetEvexTable(OpCodeTableKind table) =>
			table switch {
				OpCodeTableKind.T0F => EvexOpCodeTable.Table0F,
				OpCodeTableKind.T0F38 => EvexOpCodeTable.Table0F38,
				OpCodeTableKind.T0F3A => EvexOpCodeTable.Table0F3A,
				_ => throw new InvalidOperationException(),
			};

		static XopOpCodeTable GetXopTable(OpCodeTableKind table) =>
			table switch {
				OpCodeTableKind.XOP8 => XopOpCodeTable.XOP8,
				OpCodeTableKind.XOP9 => XopOpCodeTable.XOP9,
				OpCodeTableKind.XOPA => XopOpCodeTable.XOPA,
				_ => throw new InvalidOperationException(),
			};

		static uint GetAllowedPrefixes(OpCodeInfo opCode) {
			var flags = opCode.Flags & EncoderTypesGen.PrefixesMask;
			return EncoderTypes.AllowedPrefixesMap[flags].Value;
		}

		static WBit GetWBit(OpCodeInfo opCode) {
			if ((opCode.Flags & OpCodeFlags.WIG32) != 0)
				return WBit.WIG32;
			if ((opCode.Flags & OpCodeFlags.WIG) != 0)
				return WBit.WIG;
			if ((opCode.Flags & OpCodeFlags.W) != 0)
				return WBit.W1;
			return WBit.W0;
		}
	}
}
