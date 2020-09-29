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
using System.Linq;
using Generator.Enums;
using Generator.Enums.Decoder;
using Generator.Enums.Encoder;
using Generator.IO;
using Generator.Tables;

namespace Generator.Encoder {
	abstract class EncoderGenerator {
		protected abstract void Generate(EnumType enumType);
		protected abstract void Generate((EnumValue opCodeOperandKind, EnumValue legacyOpKind, OpHandlerKind opHandlerKind, object[] args)[] legacy, (EnumValue opCodeOperandKind, EnumValue vexOpKind, OpHandlerKind opHandlerKind, object[] args)[] vex, (EnumValue opCodeOperandKind, EnumValue xopOpKind, OpHandlerKind opHandlerKind, object[] args)[] xop, (EnumValue opCodeOperandKind, EnumValue evexOpKind, OpHandlerKind opHandlerKind, object[] args)[] evex);
		protected abstract void GenerateOpCodeInfo(InstructionDef[] defs);
		protected abstract void Generate((EnumValue value, uint size)[] immSizes);
		protected abstract void Generate((EnumValue allowedPrefixes, InstructionDefFlags1 prefixes)[] infos, (EnumValue value, InstructionDefFlags1 flag)[] flagsInfos);
		protected abstract void GenerateInstructionFormatter((EnumValue code, string result)[] notInstrStrings, EnumValue[] opMaskIsK1, EnumValue[] incVecIndex, EnumValue[] noVecIndex, EnumValue[] swapVecIndex12, EnumValue[] fpuSkipOp0);
		protected abstract void GenerateOpCodeFormatter((EnumValue code, string result)[] notInstrStrings, EnumValue[] hasModRM, EnumValue[] hasVsib);
		protected abstract void GenerateCore();
		protected abstract void GenerateInstrSwitch(EnumValue[] jccInstr, EnumValue[] simpleBranchInstr, EnumValue[] callInstr, EnumValue[] jmpInstr, EnumValue[] xbeginInstr);
		protected abstract void GenerateVsib(EnumValue[] vsib32, EnumValue[] vsib64);

		protected readonly GenTypes genTypes;
		readonly EncoderTypes encoderTypes;

		protected EncoderGenerator(GenTypes genTypes) {
			this.genTypes = genTypes;
			encoderTypes = genTypes.GetObject<EncoderTypes>(TypeIds.EncoderTypes);
		}

		public void Generate() {
			var enumTypes = new EnumType[] {
				genTypes[TypeIds.EncFlags1],
				genTypes[TypeIds.LegacyFlags3],
				genTypes[TypeIds.VexFlags3],
				genTypes[TypeIds.XopFlags3],
				genTypes[TypeIds.EvexFlags3],
				genTypes[TypeIds.AllowedPrefixes],
				genTypes[TypeIds.LegacyFlags],
				genTypes[TypeIds.VexFlags],
				genTypes[TypeIds.XopFlags],
				genTypes[TypeIds.EvexFlags],
				genTypes[TypeIds.D3nowFlags],
			};
			foreach (var enumType in enumTypes)
				Generate(enumType);

			Generate(encoderTypes.LegacyOpHandlers, encoderTypes.VexOpHandlers, encoderTypes.XopOpHandlers, encoderTypes.EvexOpHandlers);
			var defs = genTypes.GetObject<InstructionDefs>(TypeIds.InstructionDefs).Defs;
			GenerateOpCodeInfo(defs);
			Generate(encoderTypes.ImmSizes);
			var opCodeFlags = genTypes[TypeIds.OpCodeFlags];
			var flagsInfos = new (EnumValue value, InstructionDefFlags1 flag)[] {
				(opCodeFlags[nameof(EncoderOpCodeFlags.LockPrefix)], InstructionDefFlags1.Lock),
				(opCodeFlags[nameof(EncoderOpCodeFlags.XacquirePrefix)], InstructionDefFlags1.Xacquire),
				(opCodeFlags[nameof(EncoderOpCodeFlags.XreleasePrefix)], InstructionDefFlags1.Xrelease),
				(opCodeFlags[nameof(EncoderOpCodeFlags.RepPrefix)], InstructionDefFlags1.Rep),
				(opCodeFlags[nameof(EncoderOpCodeFlags.RepnePrefix)], InstructionDefFlags1.Repne),
				(opCodeFlags[nameof(EncoderOpCodeFlags.BndPrefix)], InstructionDefFlags1.Bnd),
				(opCodeFlags[nameof(EncoderOpCodeFlags.HintTakenPrefix)], InstructionDefFlags1.HintTaken),
				(opCodeFlags[nameof(EncoderOpCodeFlags.NotrackPrefix)], InstructionDefFlags1.Notrack),
			};
			Generate(encoderTypes.AllowedPrefixesMap.Select(a => (a.Value, a.Key)).OrderBy(a => a.Value.Value).ToArray(), flagsInfos);
			var notInstrOpCodeStrs = defs.Where(a => (a.Flags1 & InstructionDefFlags1.NoInstruction) != 0).Select(a => (a.Code, a.OpCodeString)).ToArray();
			var notInstrInstrStrs = defs.Where(a => (a.Flags1 & InstructionDefFlags1.NoInstruction) != 0).Select(a => (a.Code, a.InstructionString)).ToArray();
			var opMaskIsK1 = defs.Where(a => (a.IStringFlags & InstructionStringFlags.OpMaskIsK1) != 0).Select(a => a.Code).ToArray();
			var incVecIndex = defs.Where(a => (a.IStringFlags & InstructionStringFlags.IncVecIndex) != 0).Select(a => a.Code).ToArray();
			var noVecIndex = defs.Where(a => (a.IStringFlags & InstructionStringFlags.NoVecIndex) != 0).Select(a => a.Code).ToArray();
			var swapVecIndex12 = defs.Where(a => (a.IStringFlags & InstructionStringFlags.SwapVecIndex12) != 0).Select(a => a.Code).ToArray();
			var fpuSkipOp0 = defs.Where(a => (a.IStringFlags & InstructionStringFlags.FpuSkipOp0) != 0).Select(a => a.Code).ToArray();
			GenerateInstructionFormatter(notInstrInstrStrs, opMaskIsK1, incVecIndex, noVecIndex, swapVecIndex12, fpuSkipOp0);
			var opCodeOperandKind = genTypes[TypeIds.OpCodeOperandKind];
			var hasModRM = new EnumValue[] {
				opCodeOperandKind[nameof(OpCodeOperandKind.mem)],
				opCodeOperandKind[nameof(OpCodeOperandKind.mem_mpx)],
				opCodeOperandKind[nameof(OpCodeOperandKind.mem_mib)],
				opCodeOperandKind[nameof(OpCodeOperandKind.mem_vsib32x)],
				opCodeOperandKind[nameof(OpCodeOperandKind.mem_vsib64x)],
				opCodeOperandKind[nameof(OpCodeOperandKind.mem_vsib32y)],
				opCodeOperandKind[nameof(OpCodeOperandKind.mem_vsib64y)],
				opCodeOperandKind[nameof(OpCodeOperandKind.mem_vsib32z)],
				opCodeOperandKind[nameof(OpCodeOperandKind.mem_vsib64z)],
				opCodeOperandKind[nameof(OpCodeOperandKind.r8_or_mem)],
				opCodeOperandKind[nameof(OpCodeOperandKind.r16_or_mem)],
				opCodeOperandKind[nameof(OpCodeOperandKind.r32_or_mem)],
				opCodeOperandKind[nameof(OpCodeOperandKind.r32_or_mem_mpx)],
				opCodeOperandKind[nameof(OpCodeOperandKind.r64_or_mem)],
				opCodeOperandKind[nameof(OpCodeOperandKind.r64_or_mem_mpx)],
				opCodeOperandKind[nameof(OpCodeOperandKind.mm_or_mem)],
				opCodeOperandKind[nameof(OpCodeOperandKind.xmm_or_mem)],
				opCodeOperandKind[nameof(OpCodeOperandKind.ymm_or_mem)],
				opCodeOperandKind[nameof(OpCodeOperandKind.zmm_or_mem)],
				opCodeOperandKind[nameof(OpCodeOperandKind.bnd_or_mem_mpx)],
				opCodeOperandKind[nameof(OpCodeOperandKind.k_or_mem)],
				opCodeOperandKind[nameof(OpCodeOperandKind.r8_reg)],
				opCodeOperandKind[nameof(OpCodeOperandKind.r16_reg)],
				opCodeOperandKind[nameof(OpCodeOperandKind.r16_reg_mem)],
				opCodeOperandKind[nameof(OpCodeOperandKind.r16_rm)],
				opCodeOperandKind[nameof(OpCodeOperandKind.r32_reg)],
				opCodeOperandKind[nameof(OpCodeOperandKind.r32_reg_mem)],
				opCodeOperandKind[nameof(OpCodeOperandKind.r32_rm)],
				opCodeOperandKind[nameof(OpCodeOperandKind.r64_reg)],
				opCodeOperandKind[nameof(OpCodeOperandKind.r64_reg_mem)],
				opCodeOperandKind[nameof(OpCodeOperandKind.r64_rm)],
				opCodeOperandKind[nameof(OpCodeOperandKind.seg_reg)],
				opCodeOperandKind[nameof(OpCodeOperandKind.k_reg)],
				opCodeOperandKind[nameof(OpCodeOperandKind.kp1_reg)],
				opCodeOperandKind[nameof(OpCodeOperandKind.k_rm)],
				opCodeOperandKind[nameof(OpCodeOperandKind.mm_reg)],
				opCodeOperandKind[nameof(OpCodeOperandKind.mm_rm)],
				opCodeOperandKind[nameof(OpCodeOperandKind.xmm_reg)],
				opCodeOperandKind[nameof(OpCodeOperandKind.xmm_rm)],
				opCodeOperandKind[nameof(OpCodeOperandKind.ymm_reg)],
				opCodeOperandKind[nameof(OpCodeOperandKind.ymm_rm)],
				opCodeOperandKind[nameof(OpCodeOperandKind.zmm_reg)],
				opCodeOperandKind[nameof(OpCodeOperandKind.zmm_rm)],
				opCodeOperandKind[nameof(OpCodeOperandKind.cr_reg)],
				opCodeOperandKind[nameof(OpCodeOperandKind.dr_reg)],
				opCodeOperandKind[nameof(OpCodeOperandKind.tr_reg)],
				opCodeOperandKind[nameof(OpCodeOperandKind.bnd_reg)],
				opCodeOperandKind[nameof(OpCodeOperandKind.sibmem)],
				opCodeOperandKind[nameof(OpCodeOperandKind.tmm_reg)],
				opCodeOperandKind[nameof(OpCodeOperandKind.tmm_rm)],
			};
			var hasVsib = new EnumValue[] {
				opCodeOperandKind[nameof(OpCodeOperandKind.mem_vsib32x)],
				opCodeOperandKind[nameof(OpCodeOperandKind.mem_vsib64x)],
				opCodeOperandKind[nameof(OpCodeOperandKind.mem_vsib32y)],
				opCodeOperandKind[nameof(OpCodeOperandKind.mem_vsib64y)],
				opCodeOperandKind[nameof(OpCodeOperandKind.mem_vsib32z)],
				opCodeOperandKind[nameof(OpCodeOperandKind.mem_vsib64z)],
			};
			GenerateOpCodeFormatter(notInstrOpCodeStrs, hasModRM, hasVsib);
			GenerateCore();
			var jccInstr = defs.Where(a => a.BranchKind == BranchKind.JccShort || a.BranchKind == BranchKind.JccNear).Select(a => a.Code).OrderBy(a => a.Value).ToArray();
			var simpleBranchInstr = defs.Where(a => a.BranchKind == BranchKind.Loop || a.BranchKind == BranchKind.Jrcxz).Select(a => a.Code).OrderBy(a => a.Value).ToArray();
			var callInstr = defs.Where(a => a.BranchKind == BranchKind.CallNear).Select(a => a.Code).OrderBy(a => a.Value).ToArray();
			var jmpInstr = defs.Where(a => a.BranchKind == BranchKind.JmpShort || a.BranchKind == BranchKind.JmpNear).Select(a => a.Code).OrderBy(a => a.Value).ToArray();
			var xbeginInstr = defs.Where(a => a.BranchKind == BranchKind.Xbegin).Select(a => a.Code).OrderBy(a => a.Value).ToArray();
			GenerateInstrSwitch(jccInstr, simpleBranchInstr, callInstr, jmpInstr, xbeginInstr);
			var vsib32 = defs.Where(a => a.OpKinds.Any(b =>
				b == OpCodeOperandKind.mem_vsib32x || b == OpCodeOperandKind.mem_vsib32y || b == OpCodeOperandKind.mem_vsib32z)).
				Select(a => a.Code).OrderBy(a => a.Value).ToArray();
			var vsib64 = defs.Where(a => a.OpKinds.Any(b =>
				b == OpCodeOperandKind.mem_vsib64x || b == OpCodeOperandKind.mem_vsib64y || b == OpCodeOperandKind.mem_vsib64z)).
				Select(a => a.Code).OrderBy(a => a.Value).ToArray();
			GenerateVsib(vsib32, vsib64);
		}

		protected IEnumerable<(InstructionDef def, uint dword1, uint dword2, uint dword3)> GetData(InstructionDef[] defs) {
			int encodingShift = (int)genTypes[TypeIds.EncFlags1]["EncodingShift"].Value;
			int opCodeShift = (int)genTypes[TypeIds.EncFlags1]["OpCodeShift"].Value;

			var legacyFlags3 = genTypes[TypeIds.LegacyFlags3];
			var vexFlags3 = genTypes[TypeIds.VexFlags3];
			var xopFlags3 = genTypes[TypeIds.XopFlags3];
			var evexFlags3 = genTypes[TypeIds.EvexFlags3];
			var legacyFlags = genTypes[TypeIds.LegacyFlags];
			var vexFlags = genTypes[TypeIds.VexFlags];
			var xopFlags = genTypes[TypeIds.XopFlags];
			var evexFlags = genTypes[TypeIds.EvexFlags];
			var d3nowFlags = genTypes[TypeIds.D3nowFlags];

			var legacyOpShifts = new[] {
				(int)legacyFlags3["Op0Shift"].Value,
				(int)legacyFlags3["Op1Shift"].Value,
				(int)legacyFlags3["Op2Shift"].Value,
				(int)legacyFlags3["Op3Shift"].Value,
			};
			var vexOpShifts = new[] {
				(int)vexFlags3["Op0Shift"].Value,
				(int)vexFlags3["Op1Shift"].Value,
				(int)vexFlags3["Op2Shift"].Value,
				(int)vexFlags3["Op3Shift"].Value,
				(int)vexFlags3["Op4Shift"].Value,
			};
			var xopOpShifts = new[] {
				(int)xopFlags3["Op0Shift"].Value,
				(int)xopFlags3["Op1Shift"].Value,
				(int)xopFlags3["Op2Shift"].Value,
				(int)xopFlags3["Op3Shift"].Value,
			};
			var evexOpShifts = new[] {
				(int)evexFlags3["Op0Shift"].Value,
				(int)evexFlags3["Op1Shift"].Value,
				(int)evexFlags3["Op2Shift"].Value,
				(int)evexFlags3["Op3Shift"].Value,
			};

			var legacyMandatoryPrefixShift = (int)legacyFlags["MandatoryPrefixByteShift"].Value;
			var legacyOpCodeTableShift = (int)legacyFlags["LegacyOpCodeTableShift"].Value;
			var legacyEncodableShift = (int)legacyFlags["EncodableShift"].Value;
			var legacyHasGroupIndex = legacyFlags["HasGroupIndex"].Value;
			var legacyGroupShift = (int)legacyFlags["GroupShift"].Value;
			var legacyAllowedPrefixesShift = (int)legacyFlags["AllowedPrefixesShift"].Value;
			var legacyFwait = legacyFlags["Fwait"].Value;
			var legacyHasMandatoryPrefix = legacyFlags["HasMandatoryPrefix"].Value;
			var legacyOperandSizeShift = (int)legacyFlags["OperandSizeShift"].Value;
			var legacyAddressSizeShift = (int)legacyFlags["AddressSizeShift"].Value;

			var vexMandatoryPrefixShift = (int)vexFlags["MandatoryPrefixByteShift"].Value;
			var vexOpCodeTableShift = (int)vexFlags["VexOpCodeTableShift"].Value;
			var vexEncodableShift = (int)vexFlags["EncodableShift"].Value;
			var vexHasGroupIndex = vexFlags["HasGroupIndex"].Value;
			var vexHasRmGroupIndex = vexFlags["HasRmGroupIndex"].Value;
			var vexGroupShift = (int)vexFlags["GroupShift"].Value;
			var vexLBitShift = (int)vexFlags["LBitShift"].Value;
			var vexWBitShift = (int)vexFlags["WBitShift"].Value;

			var xopMandatoryPrefixShift = (int)xopFlags["MandatoryPrefixByteShift"].Value;
			var xopOpCodeTableShift = (int)xopFlags["XopOpCodeTableShift"].Value;
			var xopEncodableShift = (int)xopFlags["EncodableShift"].Value;
			var xopHasGroupIndex = xopFlags["HasGroupIndex"].Value;
			var xopGroupShift = (int)xopFlags["GroupShift"].Value;
			var xopLBitShift = (int)xopFlags["LBitShift"].Value;
			var xopWBitShift = (int)xopFlags["WBitShift"].Value;

			var evexMandatoryPrefixShift = (int)evexFlags["MandatoryPrefixByteShift"].Value;
			var evexOpCodeTableShift = (int)evexFlags["EvexOpCodeTableShift"].Value;
			var evexEncodableShift = (int)evexFlags["EncodableShift"].Value;
			var evexHasGroupIndex = evexFlags["HasGroupIndex"].Value;
			var evexGroupShift = (int)evexFlags["GroupShift"].Value;
			var evexLBitShift = (int)evexFlags["LBitShift"].Value;
			var evexWBitShift = (int)evexFlags["WBitShift"].Value;
			var evexTupleTypeShift = (int)evexFlags["TupleTypeShift"].Value;
			var evex_b = evexFlags["b"].Value;
			var evex_er = evexFlags["er"].Value;
			var evex_sae = evexFlags["sae"].Value;
			var evex_k1 = evexFlags["k1"].Value;
			var evex_z = evexFlags["z"].Value;
			var evexRequireOpMaskRegister = evexFlags["RequireOpMaskRegister"].Value;

			var d3nowEncodableShift = (int)d3nowFlags["EncodableShift"].Value;

			foreach (var def in defs) {
				uint dword1, dword2, dword3;

				dword1 = (uint)def.Encoding << encodingShift;

				switch (def.Encoding) {
				case EncodingKind.Legacy:
					dword1 |= def.OpCode << opCodeShift;

					dword2 = 0;
					dword2 |= (uint)GetMandatoryPrefixByte(def.MandatoryPrefix) << legacyMandatoryPrefixShift;
					dword2 |= (uint)GetLegacyTable(def.Table) << legacyOpCodeTableShift;
					dword2 |= (uint)GetEncodable(def) << legacyEncodableShift;
					if (def.GroupIndex >= 0) {
						dword2 |= legacyHasGroupIndex;
						dword2 |= (uint)def.GroupIndex << legacyGroupShift;
					}
					dword2 |= GetAllowedPrefixes(def) << legacyAllowedPrefixesShift;
					if ((def.Flags1 & InstructionDefFlags1.Fwait) != 0)
						dword2 |= legacyFwait;
					if (def.MandatoryPrefix != MandatoryPrefix.None)
						dword2 |= legacyHasMandatoryPrefix;
					dword2 |= (uint)def.OperandSize << legacyOperandSizeShift;
					dword2 |= (uint)def.AddressSize << legacyAddressSizeShift;

					dword3 = 0;
					for (int i = 0; i < def.OpKinds.Length; i++)
						dword3 |= (uint)encoderTypes.ToLegacy(def.OpKinds[i]) << legacyOpShifts[i];
					break;

				case EncodingKind.VEX:
					dword1 |= def.OpCode << opCodeShift;

					dword2 = 0;
					dword2 |= (uint)GetMandatoryPrefixByte(def.MandatoryPrefix) << vexMandatoryPrefixShift;
					dword2 |= (uint)GetVexTable(def.Table) << vexOpCodeTableShift;
					dword2 |= (uint)GetEncodable(def) << vexEncodableShift;
					// They both use the same 3 bits
					if (def.GroupIndex >= 0 && def.RmGroupIndex >= 0)
						throw new InvalidOperationException();
					if (def.GroupIndex >= 0) {
						dword2 |= vexHasGroupIndex;
						dword2 |= (uint)def.GroupIndex << vexGroupShift;
					}
					if (def.RmGroupIndex >= 0) {
						dword2 |= vexHasRmGroupIndex;
						dword2 |= (uint)def.RmGroupIndex << vexGroupShift;
					}
					dword2 |= (uint)GetLBit(def) << vexLBitShift;
					dword2 |= (uint)GetWBit(def) << vexWBitShift;

					dword3 = 0;
					for (int i = 0; i < def.OpKinds.Length; i++)
						dword3 |= (uint)encoderTypes.ToVex(def.OpKinds[i]) << vexOpShifts[i];
					break;

				case EncodingKind.EVEX:
					dword1 |= def.OpCode << opCodeShift;

					dword2 = 0;
					dword2 |= (uint)GetMandatoryPrefixByte(def.MandatoryPrefix) << evexMandatoryPrefixShift;
					dword2 |= (uint)GetEvexTable(def.Table) << evexOpCodeTableShift;
					dword2 |= (uint)GetEncodable(def) << evexEncodableShift;
					if (def.GroupIndex >= 0) {
						dword2 |= evexHasGroupIndex;
						dword2 |= (uint)def.GroupIndex << evexGroupShift;
					}
					dword2 |= (uint)GetLBit(def) << evexLBitShift;
					dword2 |= (uint)GetWBit(def) << evexWBitShift;
					dword2 |= (uint)def.TupleType << evexTupleTypeShift;
					if ((def.Flags1 & InstructionDefFlags1.Broadcast) != 0)
						dword2 |= evex_b;
					if ((def.Flags1 & InstructionDefFlags1.RoundingControl) != 0)
						dword2 |= evex_er;
					if ((def.Flags1 & InstructionDefFlags1.SuppressAllExceptions) != 0)
						dword2 |= evex_sae;
					if ((def.Flags1 & InstructionDefFlags1.OpMaskRegister) != 0)
						dword2 |= evex_k1;
					if ((def.Flags1 & InstructionDefFlags1.ZeroingMasking) != 0)
						dword2 |= evex_z;
					if ((def.Flags1 & InstructionDefFlags1.RequireOpMaskRegister) != 0)
						dword2 |= evexRequireOpMaskRegister;

					dword3 = 0;
					for (int i = 0; i < def.OpKinds.Length; i++)
						dword3 |= (uint)encoderTypes.ToEvex(def.OpKinds[i]) << evexOpShifts[i];
					break;

				case EncodingKind.XOP:
					dword1 |= def.OpCode << opCodeShift;

					dword2 = 0;
					dword2 |= (uint)GetMandatoryPrefixByte(def.MandatoryPrefix) << xopMandatoryPrefixShift;
					dword2 |= (uint)GetXopTable(def.Table) << xopOpCodeTableShift;
					dword2 |= (uint)GetEncodable(def) << xopEncodableShift;
					if (def.GroupIndex >= 0) {
						dword2 |= xopHasGroupIndex;
						dword2 |= (uint)def.GroupIndex << xopGroupShift;
					}
					dword2 |= (uint)GetLBit(def) << xopLBitShift;
					dword2 |= (uint)GetWBit(def) << xopWBitShift;

					dword3 = 0;
					for (int i = 0; i < def.OpKinds.Length; i++)
						dword3 |= (uint)encoderTypes.ToXop(def.OpKinds[i]) << xopOpShifts[i];
					break;

				case EncodingKind.D3NOW:
					dword1 |= def.OpCode << opCodeShift;

					dword2 = 0;
					dword2 |= (uint)GetEncodable(def) << d3nowEncodableShift;

					dword3 = 0;
					break;

				default:
					throw new InvalidOperationException();
				}

				yield return (def, dword1, dword2, dword3);
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

		static Encodable GetEncodable(InstructionDef def) =>
			(def.Flags1 & (InstructionDefFlags1.Bit16 | InstructionDefFlags1.Bit32 | InstructionDefFlags1.Bit64)) switch {
				InstructionDefFlags1.Bit16 | InstructionDefFlags1.Bit32 | InstructionDefFlags1.Bit64 => Encodable.Any,
				InstructionDefFlags1.Bit16 => Encodable.Only1632,
				InstructionDefFlags1.Bit16 | InstructionDefFlags1.Bit32 => Encodable.Only1632,
				InstructionDefFlags1.Bit64 => Encodable.Only64,
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

		uint GetAllowedPrefixes(InstructionDef def) {
			var flags = def.Flags1 & EncoderTypesGen.PrefixesMask;
			return encoderTypes.AllowedPrefixesMap[flags].Value;
		}

		static LBit GetLBit(InstructionDef def) =>
			def.LBit switch {
				OpCodeL.L0 => LBit.L0,
				OpCodeL.L1 => LBit.L1,
				OpCodeL.LIG => LBit.LIG,
				OpCodeL.LZ => LBit.LZ,
				OpCodeL.L128 => LBit.L128,
				OpCodeL.L256 => LBit.L256,
				OpCodeL.L512 => LBit.L512,
				_ => throw new InvalidOperationException(),
			};

		static WBit GetWBit(InstructionDef def) {
			if ((def.Flags1 & InstructionDefFlags1.WIG32) != 0)
				return WBit.WIG32;
			return def.WBit switch {
				OpCodeW.W0 => WBit.W0,
				OpCodeW.W1 => WBit.W1,
				OpCodeW.WIG => WBit.WIG,
				OpCodeW.WIG32 => WBit.WIG32,
				_ => throw new InvalidOperationException(),
			};
		}

		protected void WriteFlags(FileWriter writer, IdentifierConverter idConverter, InstructionDefFlags1 prefixes, (EnumValue value, InstructionDefFlags1 flag)[] flagsInfos, string orSep, string enumItemSep, bool forceConstant) {
			bool printed = false;
			foreach (var info in flagsInfos) {
				if ((prefixes & info.flag) != 0) {
					prefixes &= ~info.flag;
					if (printed)
						writer.Write(orSep);
					printed = true;
					WriteEnum(writer, idConverter, info.value, enumItemSep, forceConstant);
				}
			}
			if (!printed) {
				var value = genTypes[TypeIds.OpCodeFlags][nameof(InstructionDefFlags1.None)];
				WriteEnum(writer, idConverter, value, enumItemSep, forceConstant);
			}
			if (prefixes != 0)
				throw new InvalidOperationException();

			static void WriteEnum(FileWriter writer, IdentifierConverter idConverter, EnumValue value, string enumItemSep, bool forceConstant) {
				var name = forceConstant ? idConverter.Constant(value.RawName) : value.Name(idConverter);
				writer.Write($"{value.DeclaringType.Name(idConverter)}{enumItemSep}{name}");
			}
		}
	}
}
