// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using Generator.Enums;
using Generator.Enums.Decoder;
using Generator.IO;

namespace Generator.Decoder {
	sealed class DecoderTableSerializerInfo {
		public readonly (string name, object?[] handlers)[] TablesToSerialize;
		public readonly string[] TableIndexNames;
		public readonly string Define;
		public object NullValue => nullValue ?? throw new InvalidOperationException();
		readonly object? nullValue;
		public readonly object HandlerReferenceValue;
		public readonly object ArrayReferenceValue;
		public readonly object Invalid2Value;
		public readonly object DupValue;

		DecoderTableSerializerInfo((string name, object?[] handlers)[] tablesToSerialize, string[] tableIndexNames, string define, object? nullValue, object handlerReferenceValue, object arrayReferenceValue, object invalid2Value, object dupValue) {
			TablesToSerialize = tablesToSerialize;
			TableIndexNames = tableIndexNames;
			Define = define;
			this.nullValue = nullValue;
			HandlerReferenceValue = handlerReferenceValue;
			ArrayReferenceValue = arrayReferenceValue;
			Invalid2Value = invalid2Value;
			DupValue = dupValue;
		}

		public static DecoderTableSerializerInfo Legacy(GenTypes genTypes) {
			var enumType = genTypes[TypeIds.LegacyOpCodeHandlerKind];
			return new DecoderTableSerializerInfo(genTypes.GetObject<DecoderTables>(TypeIds.DecoderTables).Legacy,
				new string[] { DecoderTable_Legacy.Handlers_MAP0 },
				CSharpConstants.DecoderDefine,
				enumType[nameof(LegacyOpCodeHandlerKind.Null)],
				enumType[nameof(LegacyOpCodeHandlerKind.HandlerReference)],
				enumType[nameof(LegacyOpCodeHandlerKind.ArrayReference)],
				enumType[nameof(LegacyOpCodeHandlerKind.Invalid2)],
				enumType[nameof(LegacyOpCodeHandlerKind.Dup)]);
		}

		public static DecoderTableSerializerInfo Vex(GenTypes genTypes) {
			var enumType = genTypes[TypeIds.VexOpCodeHandlerKind];
			return new DecoderTableSerializerInfo(genTypes.GetObject<DecoderTables>(TypeIds.DecoderTables).VEX,
				new string[] { DecoderTable_VEX.Handlers_MAP0, DecoderTable_VEX.Handlers_0F, DecoderTable_VEX.Handlers_0F38, DecoderTable_VEX.Handlers_0F3A },
				CSharpConstants.DecoderVexDefine,
				enumType[nameof(VexOpCodeHandlerKind.Null)],
				enumType[nameof(VexOpCodeHandlerKind.HandlerReference)],
				enumType[nameof(VexOpCodeHandlerKind.ArrayReference)],
				enumType[nameof(VexOpCodeHandlerKind.Invalid2)],
				enumType[nameof(VexOpCodeHandlerKind.Dup)]);
		}

		public static DecoderTableSerializerInfo Xop(GenTypes genTypes) {
			var enumType = genTypes[TypeIds.VexOpCodeHandlerKind];
			return new DecoderTableSerializerInfo(genTypes.GetObject<DecoderTables>(TypeIds.DecoderTables).XOP,
				new string[] { DecoderTable_XOP.Handlers_MAP8, DecoderTable_XOP.Handlers_MAP9, DecoderTable_XOP.Handlers_MAP10 },
				CSharpConstants.DecoderXopDefine,
				null,
				enumType[nameof(VexOpCodeHandlerKind.HandlerReference)],
				enumType[nameof(VexOpCodeHandlerKind.ArrayReference)],
				enumType[nameof(VexOpCodeHandlerKind.Invalid2)],
				enumType[nameof(VexOpCodeHandlerKind.Dup)]);
		}

		public static DecoderTableSerializerInfo Evex(GenTypes genTypes) {
			var enumType = genTypes[TypeIds.EvexOpCodeHandlerKind];
			return new DecoderTableSerializerInfo(genTypes.GetObject<DecoderTables>(TypeIds.DecoderTables).EVEX,
				new string[] {
					DecoderTable_EVEX.Handlers_0F,
					DecoderTable_EVEX.Handlers_0F38,
					DecoderTable_EVEX.Handlers_0F3A,
					DecoderTable_EVEX.Handlers_MAP5,
					DecoderTable_EVEX.Handlers_MAP6,
				},
				CSharpConstants.DecoderEvexDefine,
				null,
				enumType[nameof(EvexOpCodeHandlerKind.HandlerReference)],
				enumType[nameof(EvexOpCodeHandlerKind.ArrayReference)],
				enumType[nameof(EvexOpCodeHandlerKind.Invalid2)],
				enumType[nameof(EvexOpCodeHandlerKind.Dup)]);
		}

		public static DecoderTableSerializerInfo Mvex(GenTypes genTypes) {
			var enumType = genTypes[TypeIds.MvexOpCodeHandlerKind];
			return new DecoderTableSerializerInfo(genTypes.GetObject<DecoderTables>(TypeIds.DecoderTables).MVEX,
				new string[] {
					DecoderTable_MVEX.Handlers_0F,
					DecoderTable_MVEX.Handlers_0F38,
					DecoderTable_MVEX.Handlers_0F3A,
				},
				CSharpConstants.DecoderMvexDefine,
				null,
				enumType[nameof(MvexOpCodeHandlerKind.HandlerReference)],
				enumType[nameof(MvexOpCodeHandlerKind.ArrayReference)],
				enumType[nameof(MvexOpCodeHandlerKind.Invalid2)],
				enumType[nameof(MvexOpCodeHandlerKind.Dup)]);
		}
	}

	abstract class DecoderTableSerializer {
		protected enum InfoKind {
			Handler,
			Handlers,
		}

		protected sealed class Info {
			public readonly uint Index;
			public readonly InfoKind Kind;
			public Info(uint index, InfoKind kind) {
				Index = index;
				Kind = kind;
			}
		}

		readonly GenTypes genTypes;
		protected readonly DecoderTableSerializerInfo info;
		protected readonly IdentifierConverter idConverter;
		readonly Dictionary<IEnumValue, (int codeIndex, int codeLen)> enumValueInfo;
		readonly Dictionary<string, Info> infos;

		protected DecoderTableSerializer(GenTypes genTypes, IdentifierConverter idConverter, DecoderTableSerializerInfo info) {
			this.genTypes = genTypes;
			this.idConverter = idConverter;
			this.info = info;
			enumValueInfo = CreateEnumValueInfo(genTypes);
			infos = new Dictionary<string, Info>(StringComparer.Ordinal);
		}

		protected void SerializeCore(ByteTableWriter writer) {
			var tables = info.TablesToSerialize;
			if (tables.Length == 0)
				throw new InvalidOperationException();
			for (int i = 0; i < tables.Length; i++) {
				var name = tables[i].name;
				var handlers = tables[i].handlers;
				bool isHandler = DecoderTableUtils.IsHandler(handlers);
				infos.Add(name, new Info((uint)i, isHandler ? InfoKind.Handler : InfoKind.Handlers));

				if (i != 0)
					writer.WriteLine();
				writer.WriteCommentLine(name);

				SerializeHandlers(writer, handlers, writeKind: true);
			}
		}

		void SerializeHandlers(ByteTableWriter writer, object?[] handlers, bool writeKind = false) {
			if (DecoderTableUtils.IsHandler(handlers)) {
				if (writeKind)
					Write(writer, genTypes[TypeIds.SerializedDataKind][nameof(SerializedDataKind.HandlerReference)]);
				SerializeHandler(writer, handlers);
			}
			else {
				if (writeKind) {
					Write(writer, genTypes[TypeIds.SerializedDataKind][nameof(SerializedDataKind.ArrayReference)]);
					Write(writer, (uint)handlers.Length);
				}
				bool writeIndexComment = handlers.Length > 1;
				int handlerIndex;
				for (handlerIndex = 0; handlerIndex < handlers.Length;) {
					if (writeIndexComment) {
						if (handlerIndex != 0)
							writer.WriteLine();
						writer.WriteCommentLine($"{handlerIndex} = 0x{handlerIndex:X2}");
					}
					int count = SerializeHandler(writer, handlers, handlerIndex);
					if (count <= 0 || (uint)handlerIndex + (uint)count > (uint)handlers.Length)
						throw new InvalidOperationException();
					handlerIndex += count;
				}
			}
		}

		void Write(ByteTableWriter writer, EnumValue enumValue) {
			if ((uint)enumValue.Value > byte.MaxValue)
				throw new InvalidOperationException();
			writer.WriteByte((byte)enumValue.Value);
			writer.WriteCommentLine(enumValue.ToStringValue(idConverter));
		}

		static void Write(ByteTableWriter writer, uint value) {
			writer.WriteCompressedUInt32(value);
			writer.WriteCommentLine("0x" + value.ToString("X"));
		}

		int SerializeHandler(ByteTableWriter writer, object?[] handlers, int index) {
			int invalidCount = CountInvalid(handlers, index);
			if (invalidCount == 2) {
				SerializeData(writer, new object[] { info.Invalid2Value });
				return 2;
			}
			int count = CountSame(handlers, index);
			if (count > 1) {
				SerializeData(writer, new object[] { info.DupValue, new DupInfo((uint)count, handlers[index]) });
				return count;
			}

			SerializeData(writer, handlers[index]);
			return 1;
		}

		static int CountSame(object?[] handlers, int index) {
			var orig = handlers[index];
			int count = 1;
			for (int i = index + 1; i < handlers.Length; i++) {
				if (!IsSame(orig, handlers[i]))
					break;
				count++;
			}
			return count;
		}

		static bool IsSame(object? a, object? b) {
			if (object.Equals(a, b))
				return true;
			if (a is null || b is null)
				return false;
			if (a is object?[] aa && b is object?[] ba) {
				if (aa.Length != ba.Length)
					return false;
				for (int i = 0; i < aa.Length; i++) {
					if (!IsSame(aa[i], ba[i]))
						return false;
				}
				return true;
			}
			return false;
		}

		int CountInvalid(object?[] handlers, int index) {
			int count = 0;
			for (int i = index; i < handlers.Length; i++) {
				if (handlers[i] is not object?[] h || !DecoderTableUtils.IsInvalid(genTypes, h))
					break;
				count++;
			}
			return count;
		}

		static Dictionary<IEnumValue, (int codeIndex, int codeLen)> CreateEnumValueInfo(GenTypes genTypes) {
			var opCodeHandlerKind = genTypes[TypeIds.LegacyOpCodeHandlerKind];
			var vexOpCodeHandlerKind = genTypes[TypeIds.VexOpCodeHandlerKind];
			var evexOpCodeHandlerKind = genTypes[TypeIds.EvexOpCodeHandlerKind];
			return new Dictionary<IEnumValue, (int codeIndex, int codeLen)> {
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Ap)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.B_BM)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.B_Ev)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.BM_B)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.C_R_3a)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.DX_eAX)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.eAX_DX)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Ev_3b)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Ev_Gv_3b)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Ev_Gv_32_64)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Ev_Gv_REX)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Ev_P)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Ev_REXW)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Ev_VX)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Gv_Eb_REX)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Gv_Ev_3b)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Gv_Ev_32_64)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Gv_Ev_REX)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Gv_Ma)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Gv_Mp_2)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Gv_N)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Gv_N_Ib_REX)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.IbReg2)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Jdisp)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Jx)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.M_2)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.M_REXW_2)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.M_REXW_4)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Mf_2a)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Mv_Gv_REXW)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.P_Ev)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.P_Ev_Ib)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.PushOpSizeReg_4b)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.R_C_3a)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Reg_Ib2)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Reg_Xv2)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Rv_32_64)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.RvMw_Gw)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Simple4)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.VX_Ev)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Yv_Reg2)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Ed_V_Ib)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Gv_Ev_Ib_REX)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Gv_RX)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Gv_W)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.GvM_VX_Ib)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.V_Ev)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.VWIb_3)], (1, 2) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.VX_E_Ib)], (1, 2) },

				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.BranchIw)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.BranchSimple)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Ep)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Ev_3a)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Ev_4)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Ev_CL)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Ev_Gv_3a)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Ev_Gv_4)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Ev_Gv_CL)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Ev_Gv_Ib)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Ev_Ib_3)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Ev_Ib_4)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Ev_Ib2_3)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Ev_Ib2_4)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Ev_Iz_3)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Ev_Iz_4)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Ev_Sw)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Ev1)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Evj)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Evw)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Ew)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Gdq_Ev)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Gv_Eb)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Gv_Ev_3a)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Gv_Ev_Ib)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Gv_Ev_Iz)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Gv_Ev2)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Gv_Ev3)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Gv_Ew)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Gv_M)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Gv_M_as)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Gv_Mp_3)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Gv_Mv)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Iw_Ib)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Jb)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Jz)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Ms)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Mv_Gv)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Ov_Reg)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.PushEv)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.PushIb2)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.PushIz)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.PushOpSizeReg_4a)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.PushSimple2)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Reg_Iz)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Reg_Ov)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Reg_Xv)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Reg_Yv)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Rv)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Simple2_3a)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Simple2Iw)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Simple3)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Simple5)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Simple5_a32)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Simple5_ModRM_as)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Sw_Ev)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Xv_Yv)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Yv_Reg)], (1, 3) },
				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.Yv_Xv)], (1, 3) },

				{ opCodeHandlerKind[nameof(LegacyOpCodeHandlerKind.PushSimpleReg)], (2, 3) },

				{ vexOpCodeHandlerKind[nameof(VexOpCodeHandlerKind.Ev_VX)], (1, 2) },
				{ vexOpCodeHandlerKind[nameof(VexOpCodeHandlerKind.Gv_Ev_Gv)], (1, 2) },
				{ vexOpCodeHandlerKind[nameof(VexOpCodeHandlerKind.Ev_Gv_Gv)], (1, 2) },
				{ vexOpCodeHandlerKind[nameof(VexOpCodeHandlerKind.Gv_Ev_Ib)], (1, 2) },
				{ vexOpCodeHandlerKind[nameof(VexOpCodeHandlerKind.Gv_Ev_Id)], (1, 2) },
				{ vexOpCodeHandlerKind[nameof(VexOpCodeHandlerKind.Gv_Gv_Ev)], (1, 2) },
				{ vexOpCodeHandlerKind[nameof(VexOpCodeHandlerKind.Hv_Ed_Id)], (1, 2) },
				{ vexOpCodeHandlerKind[nameof(VexOpCodeHandlerKind.Hv_Ev)], (1, 2) },
				{ vexOpCodeHandlerKind[nameof(VexOpCodeHandlerKind.RdRq)], (1, 2) },
				{ vexOpCodeHandlerKind[nameof(VexOpCodeHandlerKind.VX_Ev)], (1, 2) },
				{ vexOpCodeHandlerKind[nameof(VexOpCodeHandlerKind.Gv_Ev)], (1, 2) },
				{ vexOpCodeHandlerKind[nameof(VexOpCodeHandlerKind.Ev)], (1, 2) },

				{ vexOpCodeHandlerKind[nameof(VexOpCodeHandlerKind.Ed_V_Ib)], (2, 2) },
				{ vexOpCodeHandlerKind[nameof(VexOpCodeHandlerKind.Gv_GPR_Ib)], (2, 2) },
				{ vexOpCodeHandlerKind[nameof(VexOpCodeHandlerKind.Gv_RX)], (2, 2) },
				{ vexOpCodeHandlerKind[nameof(VexOpCodeHandlerKind.Gv_W)], (2, 2) },
				{ vexOpCodeHandlerKind[nameof(VexOpCodeHandlerKind.GvM_VX_Ib)], (2, 2) },
				{ vexOpCodeHandlerKind[nameof(VexOpCodeHandlerKind.VHEv)], (2, 2) },
				{ vexOpCodeHandlerKind[nameof(VexOpCodeHandlerKind.VHEvIb)], (2, 2) },
				{ vexOpCodeHandlerKind[nameof(VexOpCodeHandlerKind.VWIb_3)], (2, 2) },

				{ evexOpCodeHandlerKind[nameof(EvexOpCodeHandlerKind.Ev_VX)], (1, 2) },
				{ evexOpCodeHandlerKind[nameof(EvexOpCodeHandlerKind.VX_Ev)], (1, 2) },

				{ evexOpCodeHandlerKind[nameof(EvexOpCodeHandlerKind.Ed_V_Ib)], (2, 2) },
				{ evexOpCodeHandlerKind[nameof(EvexOpCodeHandlerKind.Ev_VX_Ib)], (2, 2) },
				{ evexOpCodeHandlerKind[nameof(EvexOpCodeHandlerKind.Gv_W_er)], (2, 2) },
				{ evexOpCodeHandlerKind[nameof(EvexOpCodeHandlerKind.GvM_VX_Ib)], (2, 2) },
				{ evexOpCodeHandlerKind[nameof(EvexOpCodeHandlerKind.V_H_Ev_er)], (2, 2) },
				{ evexOpCodeHandlerKind[nameof(EvexOpCodeHandlerKind.V_H_Ev_Ib)], (2, 2) },
			};
		}

		void SerializeHandler(ByteTableWriter writer, object?[] handler) {
			int codeIndex = -1, codeLen = -1;
			if (handler[0] is IEnumValue enumValue && enumValueInfo.TryGetValue(enumValue, out var info))
				(codeIndex, codeLen) = info;

			SerializeData(writer, handler[0]);
			using (writer.Indent()) {
				for (int i = 1; i < handler.Length; i++) {
					if (codeIndex >= 0 && (codeIndex < i && i < codeIndex + codeLen)) {
						var code1 = (EnumValue)handler[codeIndex]!;
						var code2 = (EnumValue)handler[i]!;
						if (code1.Value + (uint)(i - codeIndex) != code2.Value)
							throw new InvalidOperationException();
						continue;
					}
					SerializeData(writer, handler[i]);
				}
			}
		}

		sealed class InfoIndex {
			public readonly uint Index;
			public readonly string Name;
			public InfoIndex(uint index, string name) {
				Index = index;
				Name = name;
			}
		}

		sealed class DupInfo {
			public readonly uint Count;
			public readonly object? Data;
			public DupInfo(uint count, object? data) {
				Count = count;
				Data = data;
			}
		}

		void SerializeData(ByteTableWriter writer, object? data) {
			if (data is null)
				data = info.NullValue;
			switch (data) {
			case object?[] moreHandlers:
				SerializeHandlers(writer, moreHandlers);
				break;

			case IEnumValue enumValue:
				var typeId = enumValue.DeclaringType.TypeId;
				if (typeId == TypeIds.Code) {
					writer.WriteCompressedUInt32(enumValue.Value);
					writer.WriteCommentLine(enumValue.ToStringValue(idConverter));
				}
				else if (typeId == TypeIds.Register) {
					if ((uint)enumValue.Value > byte.MaxValue)
						throw new InvalidOperationException();
					writer.WriteByte((byte)enumValue.Value);
					writer.WriteCommentLine(enumValue.ToStringValue(idConverter));
				}
				else if (typeId == TypeIds.DecoderOptions) {
					writer.WriteCompressedUInt32((uint)enumValue.Value);
					writer.WriteCommentLine(enumValue.ToStringValue(idConverter));
				}
				else if (typeId == TypeIds.HandlerFlags) {
					writer.WriteCompressedUInt32((uint)enumValue.Value);
					writer.WriteCommentLine(enumValue.ToStringValue(idConverter));
				}
				else if (typeId == TypeIds.TupleType) {
					if ((uint)enumValue.Value > byte.MaxValue)
						throw new InvalidOperationException();
					writer.WriteByte((byte)enumValue.Value);
					writer.WriteCommentLine(enumValue.ToStringValue(idConverter));
				}
				else if (typeId == TypeIds.LegacyOpCodeHandlerKind) {
					if ((uint)enumValue.Value > byte.MaxValue)
						throw new InvalidOperationException();
					writer.WriteByte((byte)enumValue.Value);
					writer.WriteCommentLine(enumValue.ToStringValue(idConverter));
				}
				else if (typeId == TypeIds.VexOpCodeHandlerKind) {
					if ((uint)enumValue.Value > byte.MaxValue)
						throw new InvalidOperationException();
					writer.WriteByte((byte)enumValue.Value);
					writer.WriteCommentLine(enumValue.ToStringValue(idConverter));
				}
				else if (typeId == TypeIds.EvexOpCodeHandlerKind) {
					if ((uint)enumValue.Value > byte.MaxValue)
						throw new InvalidOperationException();
					writer.WriteByte((byte)enumValue.Value);
					writer.WriteCommentLine(enumValue.ToStringValue(idConverter));
				}
				else if (typeId == TypeIds.MvexOpCodeHandlerKind) {
					if ((uint)enumValue.Value > byte.MaxValue)
						throw new InvalidOperationException();
					writer.WriteByte((byte)enumValue.Value);
					writer.WriteCommentLine(enumValue.ToStringValue(idConverter));
				}
				else if (typeId == TypeIds.LegacyHandlerFlags) {
					writer.WriteCompressedUInt32((uint)enumValue.Value);
					writer.WriteCommentLine(enumValue.ToStringValue(idConverter));
				}
				else
					throw new InvalidOperationException();
				break;

			case string name:
				var info = GetInfo(name);
				object kind2 = info.Kind switch {
					InfoKind.Handler => this.info.HandlerReferenceValue,
					InfoKind.Handlers => this.info.ArrayReferenceValue,
					_ => throw new InvalidOperationException(),
				};
				SerializeHandlers(writer, new object[] { kind2, new InfoIndex(info.Index, name) });
				break;

			case InfoIndex infoIndex:
				if (infoIndex.Index > byte.MaxValue)
					throw new InvalidOperationException();
				writer.WriteByte((byte)infoIndex.Index);
				writer.WriteCommentLine($"0x{infoIndex.Index:X} = {infoIndex.Name}");
				break;

			case DupInfo dup:
				writer.WriteCompressedUInt32(dup.Count);
				writer.WriteCommentLine(dup.Count.ToString());
				SerializeData(writer, dup.Data);
				break;

			case bool value:
				writer.WriteByte((byte)(value ? 1 : 0));
				writer.WriteCommentLine(value ? "true" : "false");
				break;

			case int value:
				writer.WriteCompressedUInt32((uint)value);
				writer.WriteCommentLine("0x" + value.ToString("X"));
				break;

			default:
				throw new InvalidOperationException();
			}
		}

		protected Info GetInfo(string name) {
			if (!infos.TryGetValue(name, out var info))
				throw new ArgumentException($"Invalid table name: {name}");
			return info;
		}
	}
}
