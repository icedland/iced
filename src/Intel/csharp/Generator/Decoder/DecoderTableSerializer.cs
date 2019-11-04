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

#if !NO_DECODER
using System;
using System.Collections.Generic;
using System.Text;
using Generator.IO;
using Iced.Intel;
using Iced.Intel.DecoderInternal;

namespace Generator.Decoder {
	abstract class DecoderTableSerializer {
		public abstract string ClassName { get; }

		enum InfoKind {
			Handler,
			Handlers,
		}

		sealed class Info {
			public readonly uint Index;
			public readonly InfoKind Kind;
			public Info(uint index, InfoKind kind) {
				Index = index;
				Kind = kind;
			}
		}

		readonly Dictionary<string, Info> infos;
		readonly StringBuilder sb;

		protected DecoderTableSerializer() {
			infos = new Dictionary<string, Info>(StringComparer.Ordinal);
			sb = new StringBuilder();
		}

		protected abstract object[] GetTablesToSerialize();
		protected abstract string[] GetTableIndexNames();

		public void Serialize(FileWriter writer) {
			writer.WriteHeader();
			writer.WriteLine("#if !NO_DECODER");
			writer.WriteLine("namespace Iced.Intel.DecoderInternal {");
			writer.Indent();
			writer.WriteLine($"static partial class {ClassName} {{");
			writer.Indent();
			writer.WriteLine("static byte[] GetSerializedTables() =>");
			writer.Indent();
			writer.WriteLine("new byte[] {");
			writer.Indent();

			var tables = GetTablesToSerialize();
			if (tables.Length == 0)
				throw new InvalidOperationException();
			if ((tables.Length & 1) != 0)
				throw new InvalidOperationException();
			for (int i = 0; i < tables.Length; i += 2) {
				var name = (string)tables[i];
				var handlers = (object?[])tables[i + 1];
				bool isHandler = IsHandler(handlers);
				infos.Add(name, new Info((uint)i / 2, isHandler ? InfoKind.Handler : InfoKind.Handlers));

				if (i != 0)
					writer.WriteLine();
				writer.WriteCommentLine(name);

				SerializeHandlers(writer, handlers, writeKind: true);
			}

			writer.Unindent();
			writer.WriteLine("};");
			writer.Unindent();

			foreach (var name in GetTableIndexNames())
				writer.WriteLine($"const uint {name}Index = {GetInfo(name).Index};");

			writer.Unindent();
			writer.WriteLine("}");
			writer.Unindent();
			writer.WriteLine("}");
			writer.WriteLine("#endif");
		}

		static bool IsHandler(object?[] handlers) {
			var data = handlers[0];
			return data is OpCodeHandlerKind ||
				data is VexOpCodeHandlerKind ||
				data is EvexOpCodeHandlerKind;
		}

		static bool IsInvalid(object?[] handler) {
			var data = handler[0];
			bool isInvalid =
				(data is OpCodeHandlerKind kind && kind == OpCodeHandlerKind.Invalid) ||
				(data is VexOpCodeHandlerKind vexKind && vexKind == VexOpCodeHandlerKind.Invalid) ||
				(data is EvexOpCodeHandlerKind evexKind && evexKind == EvexOpCodeHandlerKind.Invalid);
			if (isInvalid && handler.Length != 1)
				throw new InvalidOperationException();
			return isInvalid;
		}

		void SerializeHandlers(FileWriter writer, object?[] handlers, bool writeKind = false) {
			if (IsHandler(handlers)) {
				if (writeKind)
					Write(writer, SerializedDataKind.HandlerReference);
				SerializeHandler(writer, handlers);
			}
			else {
				if (writeKind) {
					Write(writer, SerializedDataKind.ArrayReference);
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

		static void Write(FileWriter writer, SerializedDataKind kind) {
			if ((uint)kind > byte.MaxValue)
				throw new InvalidOperationException();
			writer.WriteByte((byte)kind);
			writer.WriteCommentLine(kind.ToString());
		}

		static void Write(FileWriter writer, uint value) {
			writer.WriteCompressedUInt32(value);
			writer.WriteCommentLine("0x" + value.ToString("X"));
		}

		int SerializeHandler(FileWriter writer, object?[] handlers, int index) {
			int invalidCount = CountInvalid(handlers, index);
			if (invalidCount == 2) {
				SerializeData(writer, new object[] { GetInvalid2Value() });
				return 2;
			}
			int count = CountSame(handlers, index);
			if (count > 1) {
				SerializeData(writer, new object[] { GetDupValue(), new DupInfo((uint)count, handlers[index]) });
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
			if (a == null || b == null)
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

		static int CountInvalid(object?[] handlers, int index) {
			int count = 0;
			for (int i = index; i < handlers.Length; i++) {
				if (!(handlers[i] is object?[] h) || !IsInvalid(h))
					break;
				count++;
			}
			return count;
		}

		protected abstract object GetNullValue();
		protected abstract object GetHandlerReferenceValue();
		protected abstract object GetArrayReferenceValue();
		protected abstract object GetInvalid2Value();
		protected abstract object GetDupValue();

		void SerializeHandler(FileWriter writer, object?[] handler) {
			int codeIndex = -1, codeLen = -1;
			switch (handler[0]) {
			case OpCodeHandlerKind kind:
				switch (kind) {
				case OpCodeHandlerKind.Ap:
				case OpCodeHandlerKind.B_BM:
				case OpCodeHandlerKind.B_Ev:
				case OpCodeHandlerKind.BM_B:
				case OpCodeHandlerKind.C_R_3a:
				case OpCodeHandlerKind.DX_eAX:
				case OpCodeHandlerKind.eAX_DX:
				case OpCodeHandlerKind.Ev_3b:
				case OpCodeHandlerKind.Ev_Gv_3b:
				case OpCodeHandlerKind.Ev_Gv_32_64:
				case OpCodeHandlerKind.Ev_Gv_REX:
				case OpCodeHandlerKind.Ev_P:
				case OpCodeHandlerKind.Ev_REXW:
				case OpCodeHandlerKind.Ev_VX:
				case OpCodeHandlerKind.Gv_Eb_REX:
				case OpCodeHandlerKind.Gv_Ev_3b:
				case OpCodeHandlerKind.Gv_Ev_32_64:
				case OpCodeHandlerKind.Gv_Ev_REX:
				case OpCodeHandlerKind.Gv_Ma:
				case OpCodeHandlerKind.Gv_Mp_2:
				case OpCodeHandlerKind.Gv_N:
				case OpCodeHandlerKind.Gv_N_Ib_REX:
				case OpCodeHandlerKind.IbReg2:
				case OpCodeHandlerKind.Jdisp:
				case OpCodeHandlerKind.Jx:
				case OpCodeHandlerKind.M_2:
				case OpCodeHandlerKind.M_REXW_2:
				case OpCodeHandlerKind.M_REXW_4:
				case OpCodeHandlerKind.Mf_2a:
				case OpCodeHandlerKind.Mv_Gv_REXW:
				case OpCodeHandlerKind.P_Ev:
				case OpCodeHandlerKind.P_Ev_Ib:
				case OpCodeHandlerKind.PushOpSizeReg_4b:
				case OpCodeHandlerKind.R_C_3a:
				case OpCodeHandlerKind.Reg_Ib2:
				case OpCodeHandlerKind.Reg_Xv2:
				case OpCodeHandlerKind.Rv_32_64:
				case OpCodeHandlerKind.RvMw_Gw:
				case OpCodeHandlerKind.Simple4:
				case OpCodeHandlerKind.VX_Ev:
				case OpCodeHandlerKind.Yv_Reg2:
					codeIndex = 1;
					codeLen = 2;
					break;

				case OpCodeHandlerKind.Ed_V_Ib:
				case OpCodeHandlerKind.Gv_Ev_Ib_REX:
				case OpCodeHandlerKind.Gv_RX:
				case OpCodeHandlerKind.Gv_W:
				case OpCodeHandlerKind.GvM_VX_Ib:
				case OpCodeHandlerKind.V_Ev:
				case OpCodeHandlerKind.VWIb_3:
				case OpCodeHandlerKind.VX_E_Ib:
					codeIndex = 2;
					codeLen = 2;
					break;

				case OpCodeHandlerKind.BranchIw:
				case OpCodeHandlerKind.BranchSimple:
				case OpCodeHandlerKind.Ep:
				case OpCodeHandlerKind.Ev_3a:
				case OpCodeHandlerKind.Ev_4:
				case OpCodeHandlerKind.Ev_CL:
				case OpCodeHandlerKind.Ev_Gv_3a:
				case OpCodeHandlerKind.Ev_Gv_4:
				case OpCodeHandlerKind.Ev_Gv_CL:
				case OpCodeHandlerKind.Ev_Gv_Ib:
				case OpCodeHandlerKind.Ev_Ib_3:
				case OpCodeHandlerKind.Ev_Ib_4:
				case OpCodeHandlerKind.Ev_Ib2_3:
				case OpCodeHandlerKind.Ev_Ib2_4:
				case OpCodeHandlerKind.Ev_Iz_3:
				case OpCodeHandlerKind.Ev_Iz_4:
				case OpCodeHandlerKind.Ev_Sw:
				case OpCodeHandlerKind.Ev1:
				case OpCodeHandlerKind.Evj:
				case OpCodeHandlerKind.Evw:
				case OpCodeHandlerKind.Ew:
				case OpCodeHandlerKind.Gdq_Ev:
				case OpCodeHandlerKind.Gv_Eb:
				case OpCodeHandlerKind.Gv_Ev_3a:
				case OpCodeHandlerKind.Gv_Ev_Ib:
				case OpCodeHandlerKind.Gv_Ev_Iz:
				case OpCodeHandlerKind.Gv_Ev2:
				case OpCodeHandlerKind.Gv_Ev3:
				case OpCodeHandlerKind.Gv_Ew:
				case OpCodeHandlerKind.Gv_M:
				case OpCodeHandlerKind.Gv_M_as:
				case OpCodeHandlerKind.Gv_Mp_3:
				case OpCodeHandlerKind.Gv_Mv:
				case OpCodeHandlerKind.Iw_Ib:
				case OpCodeHandlerKind.Jb:
				case OpCodeHandlerKind.Jz:
				case OpCodeHandlerKind.Ms:
				case OpCodeHandlerKind.Mv_Gv:
				case OpCodeHandlerKind.Ov_Reg:
				case OpCodeHandlerKind.PushEv:
				case OpCodeHandlerKind.PushIb2:
				case OpCodeHandlerKind.PushIz:
				case OpCodeHandlerKind.PushOpSizeReg_4a:
				case OpCodeHandlerKind.PushSimple2:
				case OpCodeHandlerKind.Reg_Iz:
				case OpCodeHandlerKind.Reg_Ov:
				case OpCodeHandlerKind.Reg_Xv:
				case OpCodeHandlerKind.Reg_Yv:
				case OpCodeHandlerKind.Rv:
				case OpCodeHandlerKind.Simple2_3a:
				case OpCodeHandlerKind.Simple2Iw:
				case OpCodeHandlerKind.Simple3:
				case OpCodeHandlerKind.Simple5:
				case OpCodeHandlerKind.Simple5_ModRM_as:
				case OpCodeHandlerKind.Sw_Ev:
				case OpCodeHandlerKind.Xv_Yv:
				case OpCodeHandlerKind.Yv_Reg:
				case OpCodeHandlerKind.Yv_Xv:
					codeIndex = 1;
					codeLen = 3;
					break;

				case OpCodeHandlerKind.PushSimpleReg:
					codeIndex = 2;
					codeLen = 3;
					break;
				}
				break;

			case VexOpCodeHandlerKind kind:
				switch (kind) {
				case VexOpCodeHandlerKind.Ev_VX:
				case VexOpCodeHandlerKind.Gv_Ev_Gv:
				case VexOpCodeHandlerKind.Gv_Ev_Ib:
				case VexOpCodeHandlerKind.Gv_Ev_Id:
				case VexOpCodeHandlerKind.Gv_Gv_Ev:
				case VexOpCodeHandlerKind.Hv_Ed_Id:
				case VexOpCodeHandlerKind.Hv_Ev:
				case VexOpCodeHandlerKind.RdRq:
				case VexOpCodeHandlerKind.VX_Ev:
					codeIndex = 1;
					codeLen = 2;
					break;

				case VexOpCodeHandlerKind.Ed_V_Ib:
				case VexOpCodeHandlerKind.Gv_GPR_Ib:
				case VexOpCodeHandlerKind.Gv_RX:
				case VexOpCodeHandlerKind.Gv_W:
				case VexOpCodeHandlerKind.GvM_VX_Ib:
				case VexOpCodeHandlerKind.VHEv:
				case VexOpCodeHandlerKind.VHEvIb:
				case VexOpCodeHandlerKind.VWIb_3:
					codeIndex = 2;
					codeLen = 2;
					break;
				}
				break;

			case EvexOpCodeHandlerKind kind:
				switch (kind) {
				case EvexOpCodeHandlerKind.Ev_VX:
				case EvexOpCodeHandlerKind.VX_Ev:
					codeIndex = 1;
					codeLen = 2;
					break;

				case EvexOpCodeHandlerKind.Ed_V_Ib:
				case EvexOpCodeHandlerKind.Ev_VX_Ib:
				case EvexOpCodeHandlerKind.Gv_W_er:
				case EvexOpCodeHandlerKind.GvM_VX_Ib:
				case EvexOpCodeHandlerKind.V_H_Ev_er:
				case EvexOpCodeHandlerKind.V_H_Ev_Ib:
					codeIndex = 2;
					codeLen = 2;
					break;
				}
				break;

			default:
				throw new InvalidOperationException();
			}

			SerializeData(writer, handler[0]);
			writer.Indent();
			for (int i = 1; i < handler.Length; i++) {
				if (codeIndex >= 0 && (codeIndex < i && i < codeIndex + codeLen)) {
					var code1 = (Code)handler[codeIndex]!;
					var code2 = (Code)handler[i]!;
					if (code1 + (i - codeIndex) != code2)
						throw new InvalidOperationException();
					continue;
				}
				SerializeData(writer, handler[i]);
			}
			writer.Unindent();
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
				data = GetNullValue();
			switch (data) {
			case object?[] moreHandlers:
				SerializeHandlers(writer, moreHandlers);
				break;

			case OpCodeHandlerKind kind:
				if ((uint)kind > byte.MaxValue)
					throw new InvalidOperationException();
				writer.WriteByte((byte)kind);
				writer.WriteCommentLine(kind.ToString());
				break;

			case VexOpCodeHandlerKind kind:
				if ((uint)kind > byte.MaxValue)
					throw new InvalidOperationException();
				writer.WriteByte((byte)kind);
				writer.WriteCommentLine(kind.ToString());
				break;

			case EvexOpCodeHandlerKind kind:
				if ((uint)kind > byte.MaxValue)
					throw new InvalidOperationException();
				writer.WriteByte((byte)kind);
				writer.WriteCommentLine(kind.ToString());
				break;

			case Code code:
				writer.WriteCompressedUInt32((uint)code);
				writer.WriteCommentLine(code.ToString());
				break;

			case Register register:
				if ((uint)register > byte.MaxValue)
					throw new InvalidOperationException();
				writer.WriteByte((byte)register);
				writer.WriteCommentLine(register.ToString());
				break;

			case DecoderOptions options:
				writer.WriteCompressedUInt32((uint)options);
				writer.WriteCommentLine(options.ToString());
				break;

			case HandlerFlags flags:
				writer.WriteCompressedUInt32((uint)flags);
				writer.WriteCommentLine(ToString(sb, flags));
				break;

			case LegacyHandlerFlags flags:
				writer.WriteCompressedUInt32((uint)flags);
				writer.WriteCommentLine(flags.ToString());
				break;

			case TupleType tupleType:
				if ((uint)tupleType > byte.MaxValue)
					throw new InvalidOperationException();
				writer.WriteByte((byte)tupleType);
				writer.WriteCommentLine(tupleType.ToString());
				break;

			case string name:
				var info = GetInfo(name);
				object kind2;
				switch (info.Kind) {
				case InfoKind.Handler:
					kind2 = GetHandlerReferenceValue();
					break;
				case InfoKind.Handlers:
					kind2 = GetArrayReferenceValue();
					break;
				default:
					throw new InvalidOperationException();
				}
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

		static string ToString(StringBuilder sb, HandlerFlags flags) {
			sb.Clear();

			if ((flags & HandlerFlags.Xacquire) != 0) {
				flags &= ~HandlerFlags.Xacquire;
				Append(sb, nameof(HandlerFlags.Xacquire));
			}

			if ((flags & HandlerFlags.Xrelease) != 0) {
				flags &= ~HandlerFlags.Xrelease;
				Append(sb, nameof(HandlerFlags.Xrelease));
			}

			if ((flags & HandlerFlags.XacquireReleaseNoLock) != 0) {
				flags &= ~HandlerFlags.XacquireReleaseNoLock;
				Append(sb, nameof(HandlerFlags.XacquireReleaseNoLock));
			}

			if ((flags & HandlerFlags.Lock) != 0) {
				flags &= ~HandlerFlags.Lock;
				Append(sb, nameof(HandlerFlags.Lock));
			}

			if (flags != 0)
				throw new InvalidOperationException();

			if (sb.Length == 0)
				Append(sb, nameof(HandlerFlags.None));

			return sb.ToString();
		}

		static void Append(StringBuilder sb, string name) {
			if (sb.Length > 0)
				sb.Append(", ");
			sb.Append(name);
		}

		Info GetInfo(string name) {
			if (!infos.TryGetValue(name, out var info))
				throw new ArgumentException($"Invalid table name: {name}");
			return info;
		}
	}
}
#endif
