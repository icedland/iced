// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Linq;
using Generator.Enums;
using Generator.Tables;

namespace Generator.Encoder {
	[TypeGen(TypeGenOrders.CreatedInstructions)]
	sealed class EncoderTypes {
		public (EnumValue value, uint size)[] ImmSizes { get; }
		public (EnumValue opCodeOperandKind, OpHandlerKind opHandlerKind, object[] args)[] LegacyOpHandlers { get; }
		public (EnumValue opCodeOperandKind, OpHandlerKind opHandlerKind, object[] args)[] VexOpHandlers { get; }
		public (EnumValue opCodeOperandKind, OpHandlerKind opHandlerKind, object[] args)[] XopOpHandlers { get; }
		public (EnumValue opCodeOperandKind, OpHandlerKind opHandlerKind, object[] args)[] EvexOpHandlers { get; }
		public (EnumValue opCodeOperandKind, OpHandlerKind opHandlerKind, object[] args)[] MvexOpHandlers { get; }
		readonly Dictionary<OpCodeOperandKindDef, uint> toLegacy;
		readonly Dictionary<OpCodeOperandKindDef, uint> toVex;
		readonly Dictionary<OpCodeOperandKindDef, uint> toXop;
		readonly Dictionary<OpCodeOperandKindDef, uint> toEvex;
		readonly Dictionary<OpCodeOperandKindDef, uint> toMvex;

		EncoderTypes(GenTypes genTypes) {
			var gen = new EncoderTypesGen(genTypes);
			gen.Generate();
			ImmSizes = gen.ImmSizes ?? throw new InvalidOperationException();
			genTypes.Add(gen.EncFlags1 ?? throw new InvalidOperationException());

			var legacyOpKind = gen.LegacyOpKind ?? throw new InvalidOperationException();
			var vexOpKind = gen.VexOpKind ?? throw new InvalidOperationException();
			var xopOpKind = gen.XopOpKind ?? throw new InvalidOperationException();
			var evexOpKind = gen.EvexOpKind ?? throw new InvalidOperationException();
			var mvexOpKind = gen.MvexOpKind ?? throw new InvalidOperationException();

			LegacyOpHandlers = CreateOpHandlers(genTypes, EncodingKind.Legacy).ToArray();
			VexOpHandlers = CreateOpHandlers(genTypes, EncodingKind.VEX).ToArray();
			XopOpHandlers = CreateOpHandlers(genTypes, EncodingKind.XOP).ToArray();
			EvexOpHandlers = CreateOpHandlers(genTypes, EncodingKind.EVEX).ToArray();
			MvexOpHandlers = CreateOpHandlers(genTypes, EncodingKind.MVEX).ToArray();

			if (new HashSet<EnumValue>(LegacyOpHandlers.Select(a => a.opCodeOperandKind)).Count != legacyOpKind.Values.Length)
				throw new InvalidOperationException();
			if (new HashSet<EnumValue>(VexOpHandlers.Select(a => a.opCodeOperandKind)).Count != vexOpKind.Values.Length)
				throw new InvalidOperationException();
			if (new HashSet<EnumValue>(XopOpHandlers.Select(a => a.opCodeOperandKind)).Count != xopOpKind.Values.Length)
				throw new InvalidOperationException();
			if (new HashSet<EnumValue>(EvexOpHandlers.Select(a => a.opCodeOperandKind)).Count != evexOpKind.Values.Length)
				throw new InvalidOperationException();
			if (new HashSet<EnumValue>(MvexOpHandlers.Select(a => a.opCodeOperandKind)).Count != mvexOpKind.Values.Length)
				throw new InvalidOperationException();

			var opKindDefs = genTypes.GetObject<OpCodeOperandKindDefs>(TypeIds.OpCodeOperandKindDefs).Defs;
			toLegacy = LegacyOpHandlers.ToDictionary(a => opKindDefs[(int)a.opCodeOperandKind.Value], a => legacyOpKind[a.opCodeOperandKind.RawName].Value);
			toVex = VexOpHandlers.ToDictionary(a => opKindDefs[(int)a.opCodeOperandKind.Value], a => vexOpKind[a.opCodeOperandKind.RawName].Value);
			toXop = XopOpHandlers.ToDictionary(a => opKindDefs[(int)a.opCodeOperandKind.Value], a => xopOpKind[a.opCodeOperandKind.RawName].Value);
			toEvex = EvexOpHandlers.ToDictionary(a => opKindDefs[(int)a.opCodeOperandKind.Value], a => evexOpKind[a.opCodeOperandKind.RawName].Value);
			toMvex = MvexOpHandlers.ToDictionary(a => opKindDefs[(int)a.opCodeOperandKind.Value], a => mvexOpKind[a.opCodeOperandKind.RawName].Value);

			genTypes.AddObject(TypeIds.EncoderTypes, this);
		}

		static IEnumerable<(EnumValue opCodeOperandKind, OpHandlerKind opHandlerKind, object[] args)> CreateOpHandlers(GenTypes genTypes, EncodingKind encoding) {
			var defs = EncoderTypesGen.GetDefs(genTypes, encoding);

			var register = genTypes[TypeIds.Register];
			var opKind = genTypes[TypeIds.OpKind];

			foreach (var def in defs) {
				OpHandlerKind opHandlerKind;
				object[] args;
				EnumValue regLo, regHi;
				switch (def.OperandEncoding) {
				case OperandEncoding.None:
					opHandlerKind = OpHandlerKind.None;
					args = Array.Empty<object>();
					break;
				case OperandEncoding.NearBranch:
					opHandlerKind = OpHandlerKind.OpJ;
					args = def.NearBranchOpSize switch {
						16 => new object[] { opKind[nameof(OpKind.NearBranch16)], def.BranchOffsetSize / 8 },
						32 => new object[] { opKind[nameof(OpKind.NearBranch32)], def.BranchOffsetSize / 8 },
						64 => new object[] { opKind[nameof(OpKind.NearBranch64)], def.BranchOffsetSize / 8 },
						_ => throw new InvalidOperationException(),
					};
					break;
				case OperandEncoding.Xbegin:
					opHandlerKind = OpHandlerKind.OpJx;
					args = new object[] { def.BranchOffsetSize / 8 };
					break;
				case OperandEncoding.AbsNearBranch:
					opHandlerKind = OpHandlerKind.OpJdisp;
					args = new object[] { def.BranchOffsetSize / 8 };
					break;
				case OperandEncoding.FarBranch:
					opHandlerKind = OpHandlerKind.OpA;
					args = new object[] { def.BranchOffsetSize / 8 };
					break;
				case OperandEncoding.Immediate:
					switch (def.ImmediateSize) {
					case 4:
						if (!def.M2Z)
							throw new InvalidOperationException();
						opHandlerKind = OpHandlerKind.OpI4;
						args = Array.Empty<object>();
						break;
					case 8:
						opHandlerKind = OpHandlerKind.OpIb;
						args = def.ImmediateSignExtSize switch {
							8 => new object[] { opKind[nameof(OpKind.Immediate8)] },
							16 => new object[] { opKind[nameof(OpKind.Immediate8to16)] },
							32 => new object[] { opKind[nameof(OpKind.Immediate8to32)] },
							64 => new object[] { opKind[nameof(OpKind.Immediate8to64)] },
							_ => throw new InvalidOperationException(),
						};
						break;
					case 16:
						opHandlerKind = OpHandlerKind.OpIw;
						args = Array.Empty<object>();
						break;
					case 32:
						opHandlerKind = OpHandlerKind.OpId;
						args = def.ImmediateSignExtSize switch {
							32 => new object[] { opKind[nameof(OpKind.Immediate32)] },
							64 => new object[] { opKind[nameof(OpKind.Immediate32to64)] },
							_ => throw new InvalidOperationException(),
						};
						break;
					case 64:
						opHandlerKind = OpHandlerKind.OpIq;
						args = Array.Empty<object>();
						break;
					default:
						throw new InvalidOperationException();
					}
					break;
				case OperandEncoding.ImpliedConst:
					opHandlerKind = OpHandlerKind.OpImm;
					args = new object[] { def.ImpliedConst };
					break;
				case OperandEncoding.ImpliedRegister:
					opHandlerKind = OpHandlerKind.OpReg;
					args = new object[] { register[def.Register.ToString()] };
					break;
				case OperandEncoding.SegRBX:
					opHandlerKind = OpHandlerKind.OpMRBX;
					args = Array.Empty<object>();
					break;
				case OperandEncoding.SegRSI:
					opHandlerKind = OpHandlerKind.OpX;
					args = Array.Empty<object>();
					break;
				case OperandEncoding.SegRDI:
					opHandlerKind = OpHandlerKind.OprDI;
					args = Array.Empty<object>();
					break;
				case OperandEncoding.ESRDI:
					opHandlerKind = OpHandlerKind.OpY;
					args = Array.Empty<object>();
					break;
				case OperandEncoding.RegImm:
					opHandlerKind = OpHandlerKind.OpIsX;
					(regLo, regHi) = GetRegisterRange(encoding, register, def.Register);
					args = new object[] { regLo, regHi };
					break;
				case OperandEncoding.RegOpCode:
					if (def.Register == Register.ST0) {
						opHandlerKind = OpHandlerKind.OpRegSTi;
						args = Array.Empty<object>();
					}
					else {
						opHandlerKind = OpHandlerKind.OpRegEmbed8;
						(regLo, regHi) = GetRegisterRange(encoding, register, def.Register);
						args = new object[] { regLo, regHi };
					}
					break;
				case OperandEncoding.RegModrmReg:
					if (def.LockBit)
						opHandlerKind = OpHandlerKind.OpModRM_regF0;
					else if (def.Memory)
						opHandlerKind = OpHandlerKind.OpModRM_reg_mem;
					else
						opHandlerKind = OpHandlerKind.OpModRM_reg;
					(regLo, regHi) = GetRegisterRange(encoding, register, def.Register);
					args = new object[] { regLo, regHi };
					break;
				case OperandEncoding.RegModrmRm:
					opHandlerKind = OpHandlerKind.OpModRM_rm_reg_only;
					(regLo, regHi) = GetRegisterRange(encoding, register, def.Register);
					args = new object[] { regLo, regHi };
					break;
				case OperandEncoding.RegMemModrmRm:
					opHandlerKind = OpHandlerKind.OpModRM_rm;
					(regLo, regHi) = GetRegisterRange(encoding, register, def.Register);
					args = new object[] { regLo, regHi };
					break;
				case OperandEncoding.RegVvvvv:
					opHandlerKind = OpHandlerKind.OpHx;
					(regLo, regHi) = GetRegisterRange(encoding, register, def.Register);
					args = new object[] { regLo, regHi };
					break;
				case OperandEncoding.MemModrmRm:
					if (def.Vsib) {
						opHandlerKind = OpHandlerKind.OpVsib;
						(regLo, regHi) = GetRegisterRange(encoding, register, def.Register);
						args = new object[] { regLo, regHi };
					}
					else {
						opHandlerKind = OpHandlerKind.OpModRM_rm_mem_only;
						args = new object[] { def.SibRequired };
					}
					break;
				case OperandEncoding.MemOffset:
					opHandlerKind = OpHandlerKind.OpO;
					args = Array.Empty<object>();
					break;
				default:
					throw new InvalidOperationException();
				}
				yield return (def.EnumValue, opHandlerKind, args);
			}
		}

		static (EnumValue vecLo, EnumValue vecHi) GetRegisterRange(EncodingKind encoding, EnumType registerType, Register register) =>
			register switch {
				Register.AL => (registerType[nameof(Register.AL)], registerType[nameof(Register.R15L)]),
				Register.AX => (registerType[nameof(Register.AX)], registerType[nameof(Register.R15W)]),
				Register.EAX => (registerType[nameof(Register.EAX)], registerType[nameof(Register.R15D)]),
				Register.RAX => (registerType[nameof(Register.RAX)], registerType[nameof(Register.R15)]),
				Register.ES => (registerType[nameof(Register.ES)], registerType[nameof(Register.GS)]),
				Register.K0 => (registerType[nameof(Register.K0)], registerType[nameof(Register.K7)]),
				Register.BND0 => (registerType[nameof(Register.BND0)], registerType[nameof(Register.BND3)]),
				Register.CR0 => (registerType[nameof(Register.CR0)], registerType[nameof(Register.CR15)]),
				Register.DR0 => (registerType[nameof(Register.DR0)], registerType[nameof(Register.DR15)]),
				Register.TR0 => (registerType[nameof(Register.TR0)], registerType[nameof(Register.TR7)]),
				Register.ST0 => (registerType[nameof(Register.ST0)], registerType[nameof(Register.ST7)]),
				Register.MM0 => (registerType[nameof(Register.MM0)], registerType[nameof(Register.MM7)]),
				Register.TMM0 => (registerType[nameof(Register.TMM0)], registerType[nameof(Register.TMM7)]),
				_ => encoding switch {
					EncodingKind.Legacy or EncodingKind.D3NOW or EncodingKind.VEX or EncodingKind.XOP =>
						register switch {
							Register.XMM0 => (registerType[nameof(Register.XMM0)], registerType[nameof(Register.XMM15)]),
							Register.YMM0 => (registerType[nameof(Register.YMM0)], registerType[nameof(Register.YMM15)]),
							Register.ZMM0 => (registerType[nameof(Register.ZMM0)], registerType[nameof(Register.ZMM15)]),
							_ => throw new InvalidOperationException(),
						},
					EncodingKind.EVEX =>
						register switch {
							Register.XMM0 => (registerType[nameof(Register.XMM0)], registerType[nameof(Register.XMM31)]),
							Register.YMM0 => (registerType[nameof(Register.YMM0)], registerType[nameof(Register.YMM31)]),
							Register.ZMM0 => (registerType[nameof(Register.ZMM0)], registerType[nameof(Register.ZMM31)]),
							_ => throw new InvalidOperationException(),
						},
					EncodingKind.MVEX =>
						register switch {
							Register.ZMM0 => (registerType[nameof(Register.ZMM0)], registerType[nameof(Register.ZMM31)]),
							_ => throw new InvalidOperationException(),
						},
					_ => throw new InvalidOperationException(),
				},
			};

		public uint ToLegacy(OpCodeOperandKindDef opKind) => toLegacy[opKind];
		public uint ToVex(OpCodeOperandKindDef opKind) => toVex[opKind];
		public uint ToXop(OpCodeOperandKindDef opKind) => toXop[opKind];
		public uint ToEvex(OpCodeOperandKindDef opKind) => toEvex[opKind];
		public uint ToMvex(OpCodeOperandKindDef opKind) => toMvex[opKind];
	}
}
