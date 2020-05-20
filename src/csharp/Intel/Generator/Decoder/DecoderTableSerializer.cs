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
using System.Text;
using Generator.Enums;
using Generator.Enums.Decoder;
using Generator.IO;

namespace Generator.Decoder {
	sealed class DecoderTableSerializerInfo {
		public readonly (string name, object?[] handlers)[] TablesToSerialize;
		public readonly string[] TableIndexNames;
		public object NullValue => nullValue ?? throw new InvalidOperationException();
		readonly object? nullValue;
		public readonly object HandlerReferenceValue;
		public readonly object ArrayReferenceValue;
		public readonly object Invalid2Value;
		public readonly object DupValue;

		DecoderTableSerializerInfo((string name, object?[] handlers)[] tablesToSerialize, string[] tableIndexNames, object? nullValue, object handlerReferenceValue, object arrayReferenceValue, object invalid2Value, object dupValue) {
			TablesToSerialize = tablesToSerialize;
			TableIndexNames = tableIndexNames;
			this.nullValue = nullValue;
			HandlerReferenceValue = handlerReferenceValue;
			ArrayReferenceValue = arrayReferenceValue;
			Invalid2Value = invalid2Value;
			DupValue = dupValue;
		}

		public static DecoderTableSerializerInfo Legacy(GenTypes genTypes) {
			var enumType = genTypes[TypeIds.OpCodeHandlerKind];
			return new DecoderTableSerializerInfo(genTypes.GetObject<DecoderTables>(TypeIds.DecoderTables).Legacy,
				new string[] { DecoderTable_Legacy.OneByteHandlers },
				enumType[nameof(OpCodeHandlerKind.Null)],
				enumType[nameof(OpCodeHandlerKind.HandlerReference)],
				enumType[nameof(OpCodeHandlerKind.ArrayReference)],
				enumType[nameof(OpCodeHandlerKind.Invalid2)],
				enumType[nameof(OpCodeHandlerKind.Dup)]);
		}

		public static DecoderTableSerializerInfo Vex(GenTypes genTypes) {
			var enumType = genTypes[TypeIds.VexOpCodeHandlerKind];
			return new DecoderTableSerializerInfo(genTypes.GetObject<DecoderTables>(TypeIds.DecoderTables).VEX,
				new string[] { DecoderTable_VEX.ThreeByteHandlers_0F38XX, DecoderTable_VEX.ThreeByteHandlers_0F3AXX, DecoderTable_VEX.TwoByteHandlers_0FXX },
				null,
				enumType[nameof(VexOpCodeHandlerKind.HandlerReference)],
				enumType[nameof(VexOpCodeHandlerKind.ArrayReference)],
				enumType[nameof(VexOpCodeHandlerKind.Invalid2)],
				enumType[nameof(VexOpCodeHandlerKind.Dup)]);
		}

		public static DecoderTableSerializerInfo Xop(GenTypes genTypes) {
			var enumType = genTypes[TypeIds.VexOpCodeHandlerKind];
			return new DecoderTableSerializerInfo(genTypes.GetObject<DecoderTables>(TypeIds.DecoderTables).XOP,
				new string[] { DecoderTable_XOP.XOP8, DecoderTable_XOP.XOP9, DecoderTable_XOP.XOPA },
				null,
				enumType[nameof(VexOpCodeHandlerKind.HandlerReference)],
				enumType[nameof(VexOpCodeHandlerKind.ArrayReference)],
				enumType[nameof(VexOpCodeHandlerKind.Invalid2)],
				enumType[nameof(VexOpCodeHandlerKind.Dup)]);
		}

		public static DecoderTableSerializerInfo Evex(GenTypes genTypes) {
			var enumType = genTypes[TypeIds.EvexOpCodeHandlerKind];
			return new DecoderTableSerializerInfo(genTypes.GetObject<DecoderTables>(TypeIds.DecoderTables).EVEX,
				new string[] { DecoderTable_EVEX.ThreeByteHandlers_0F38XX, DecoderTable_EVEX.ThreeByteHandlers_0F3AXX, DecoderTable_EVEX.TwoByteHandlers_0FXX },
				null,
				enumType[nameof(EvexOpCodeHandlerKind.HandlerReference)],
				enumType[nameof(EvexOpCodeHandlerKind.ArrayReference)],
				enumType[nameof(EvexOpCodeHandlerKind.Invalid2)],
				enumType[nameof(EvexOpCodeHandlerKind.Dup)]);
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
		readonly StringBuilder sb;

		protected DecoderTableSerializer(GenTypes genTypes, IdentifierConverter idConverter, DecoderTableSerializerInfo info) {
			this.genTypes = genTypes;
			this.idConverter = idConverter;
			this.info = info;
			enumValueInfo = CreateEnumValueInfo(genTypes);
			infos = new Dictionary<string, Info>(StringComparer.Ordinal);
			sb = new StringBuilder();
		}

		protected void SerializeCore(FileWriter writer) {
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

		void SerializeHandlers(FileWriter writer, object?[] handlers, bool writeKind = false) {
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

		void Write(FileWriter writer, EnumValue enumValue) {
			if ((uint)enumValue.Value > byte.MaxValue)
				throw new InvalidOperationException();
			writer.WriteByte((byte)enumValue.Value);
			writer.WriteCommentLine(enumValue.ToStringValue(idConverter));
		}

		static void Write(FileWriter writer, uint value) {
			writer.WriteCompressedUInt32(value);
			writer.WriteCommentLine("0x" + value.ToString("X"));
		}

		int SerializeHandler(FileWriter writer, object?[] handlers, int index) {
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
				if (!(handlers[i] is object?[] h) || !DecoderTableUtils.IsInvalid(genTypes, h))
					break;
				count++;
			}
			return count;
		}

		Dictionary<IEnumValue, (int codeIndex, int codeLen)> CreateEnumValueInfo(GenTypes genTypes) {
			var opCodeHandlerKind = genTypes[TypeIds.OpCodeHandlerKind];
			var vexOpCodeHandlerKind = genTypes[TypeIds.VexOpCodeHandlerKind];
			var evexOpCodeHandlerKind = genTypes[TypeIds.EvexOpCodeHandlerKind];
			return new Dictionary<IEnumValue, (int codeIndex, int codeLen)> {
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Ap)], (1, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.B_BM)], (1, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.B_Ev)], (1, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.BM_B)], (1, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.C_R_3a)], (1, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.DX_eAX)], (1, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.eAX_DX)], (1, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Ev_3b)], (1, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Ev_Gv_3b)], (1, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Ev_Gv_32_64)], (1, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Ev_Gv_REX)], (1, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Ev_P)], (1, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Ev_REXW)], (1, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Ev_VX)], (1, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Gv_Eb_REX)], (1, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Gv_Ev_3b)], (1, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Gv_Ev_32_64)], (1, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Gv_Ev_REX)], (1, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Gv_Ma)], (1, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Gv_Mp_2)], (1, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Gv_N)], (1, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Gv_N_Ib_REX)], (1, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.IbReg2)], (1, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Jdisp)], (1, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Jx)], (1, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.M_2)], (1, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.M_REXW_2)], (1, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.M_REXW_4)], (1, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Mf_2a)], (1, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Mv_Gv_REXW)], (1, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.P_Ev)], (1, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.P_Ev_Ib)], (1, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.PushOpSizeReg_4b)], (1, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.R_C_3a)], (1, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Reg_Ib2)], (1, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Reg_Xv2)], (1, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Rv_32_64)], (1, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.RvMw_Gw)], (1, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Simple4)], (1, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.VX_Ev)], (1, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Yv_Reg2)], (1, 2) },

				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Ed_V_Ib)], (2, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Gv_Ev_Ib_REX)], (2, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Gv_RX)], (2, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Gv_W)], (2, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.GvM_VX_Ib)], (2, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.V_Ev)], (2, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.VWIb_3)], (2, 2) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.VX_E_Ib)], (2, 2) },

				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.BranchIw)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.BranchSimple)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Ep)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Ev_3a)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Ev_4)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Ev_CL)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Ev_Gv_3a)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Ev_Gv_4)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Ev_Gv_CL)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Ev_Gv_Ib)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Ev_Ib_3)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Ev_Ib_4)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Ev_Ib2_3)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Ev_Ib2_4)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Ev_Iz_3)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Ev_Iz_4)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Ev_Sw)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Ev1)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Evj)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Evw)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Ew)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Gdq_Ev)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Gv_Eb)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Gv_Ev_3a)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Gv_Ev_Ib)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Gv_Ev_Iz)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Gv_Ev2)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Gv_Ev3)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Gv_Ew)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Gv_M)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Gv_M_as)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Gv_Mp_3)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Gv_Mv)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Iw_Ib)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Jb)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Jz)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Ms)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Mv_Gv)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Ov_Reg)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.PushEv)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.PushIb2)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.PushIz)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.PushOpSizeReg_4a)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.PushSimple2)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Reg_Iz)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Reg_Ov)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Reg_Xv)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Reg_Yv)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Rv)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Simple2_3a)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Simple2Iw)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Simple3)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Simple5)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Simple5_ModRM_as)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Sw_Ev)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Xv_Yv)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Yv_Reg)], (1, 3) },
				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.Yv_Xv)], (1, 3) },

				{ opCodeHandlerKind[nameof(OpCodeHandlerKind.PushSimpleReg)], (2, 3) },

				{ vexOpCodeHandlerKind[nameof(VexOpCodeHandlerKind.Ev_VX)], (1, 2) },
				{ vexOpCodeHandlerKind[nameof(VexOpCodeHandlerKind.Gv_Ev_Gv)], (1, 2) },
				{ vexOpCodeHandlerKind[nameof(VexOpCodeHandlerKind.Gv_Ev_Ib)], (1, 2) },
				{ vexOpCodeHandlerKind[nameof(VexOpCodeHandlerKind.Gv_Ev_Id)], (1, 2) },
				{ vexOpCodeHandlerKind[nameof(VexOpCodeHandlerKind.Gv_Gv_Ev)], (1, 2) },
				{ vexOpCodeHandlerKind[nameof(VexOpCodeHandlerKind.Hv_Ed_Id)], (1, 2) },
				{ vexOpCodeHandlerKind[nameof(VexOpCodeHandlerKind.Hv_Ev)], (1, 2) },
				{ vexOpCodeHandlerKind[nameof(VexOpCodeHandlerKind.RdRq)], (1, 2) },
				{ vexOpCodeHandlerKind[nameof(VexOpCodeHandlerKind.VX_Ev)], (1, 2) },

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

		void SerializeHandler(FileWriter writer, object?[] handler) {
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

		void SerializeData(FileWriter writer, object? data) {
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
				else if (typeId == TypeIds.OpCodeHandlerKind) {
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
				writer.WriteCommentLine("0x" + value.ToString());
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
