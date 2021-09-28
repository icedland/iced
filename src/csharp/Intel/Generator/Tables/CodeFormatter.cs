// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Text;
using Generator.Enums;
using Generator.Enums.Encoder;

namespace Generator.Tables {
	readonly struct CodeFormatter {
		readonly StringBuilder sb;
		readonly RegisterDef[] regDefs;
		readonly MemorySizeDefs memSizeTbl;
		readonly string codeMnemonic;
		readonly string? codeSuffix;
		readonly string? codeMemorySize;
		readonly string? codeMemorySizeSuffix;
		readonly EnumValue memSize;
		readonly EnumValue memSizeBcst;
		readonly InstructionDefFlags1 flags1;
		readonly MvexInfoFlags1 mvexFlags;
		readonly EncodingKind encoding;
		readonly OpCodeOperandKindDef[] opKinds;
		readonly bool isKnc;

		public CodeFormatter(StringBuilder sb, RegisterDef[] regDefs, MemorySizeDefs memSizeTbl, string codeMnemonic, string? codeSuffix,
			string? codeMemorySize, string? codeMemorySizeSuffix, EnumValue memSize, EnumValue memSizeBcst, InstructionDefFlags1 flags1,
			MvexInfoFlags1 mvexFlags, EncodingKind encoding, OpCodeOperandKindDef[] opKinds, bool isKnc) {
			if (codeMnemonic == string.Empty)
				throw new ArgumentOutOfRangeException(nameof(codeMnemonic));
			this.sb = sb;
			this.regDefs = regDefs;
			this.memSizeTbl = memSizeTbl;
			this.codeMnemonic = codeMnemonic;
			this.codeSuffix = codeSuffix;
			this.codeMemorySize = codeMemorySize;
			this.codeMemorySizeSuffix = codeMemorySizeSuffix;
			this.memSize = memSize;
			this.memSizeBcst = memSizeBcst;
			this.flags1 = flags1;
			this.mvexFlags = mvexFlags;
			this.encoding = encoding;
			this.opKinds = opKinds;
			this.isKnc = isKnc;
		}

		MemorySize GetMemorySize(bool isBroadcast) => (MemorySize)(isBroadcast ? memSizeBcst.Value : memSize.Value);
		int GetSizeInBytes(MemorySize memSize) => (int)memSizeTbl.Defs[(int)memSize].Size;

		public string Format() {
			sb.Clear();

			switch (encoding) {
			case EncodingKind.Legacy:
				break;
			case EncodingKind.VEX:
				sb.Append("VEX_");
				if (isKnc)
					sb.Append("KNC_");
				break;
			case EncodingKind.EVEX:
				sb.Append("EVEX_");
				break;
			case EncodingKind.XOP:
				sb.Append("XOP_");
				break;
			case EncodingKind.D3NOW:
				sb.Append("D3NOW_");
				break;
			case EncodingKind.MVEX:
				sb.Append("MVEX_");
				break;
			default:
				throw new InvalidOperationException();
			}

			sb.Append(codeMnemonic);

			if (opKinds.Length > 0) {
				sb.Append('_');
				for (int i = 0; i < opKinds.Length; i++) {
					if (i > 0)
						sb.Append('_');
					int gprSizeBits;
					string regStr;
					var def = opKinds[i];
					switch (def.OperandEncoding) {
					case OperandEncoding.NearBranch:
					case OperandEncoding.Xbegin:
						sb.Append($"rel{def.BranchOffsetSize}");
						break;

					case OperandEncoding.AbsNearBranch:
						sb.Append($"disp{def.BranchOffsetSize}");
						break;

					case OperandEncoding.FarBranch:
						sb.Append($"ptr16{def.BranchOffsetSize}");
						break;

					case OperandEncoding.Immediate:
						sb.Append($"imm{def.ImmediateSize}");
						break;

					case OperandEncoding.ImpliedConst:
						sb.Append(def.ImpliedConst);
						break;

					case OperandEncoding.ImpliedRegister:
						if (def.Register == Register.ST0)
							sb.Append("st0");
						else
							WriteRegister(regDefs[(int)def.Register].Name);
						break;

					case OperandEncoding.SegRBX:
					case OperandEncoding.SegRSI:
					case OperandEncoding.ESRDI:
						WriteMemory();
						break;

					case OperandEncoding.SegRDI:
						sb.Append("rDI");
						break;

					case OperandEncoding.RegImm:
					case OperandEncoding.RegOpCode:
					case OperandEncoding.RegModrmReg:
					case OperandEncoding.RegModrmRm:
					case OperandEncoding.RegVvvvv:
						regStr = GetRegInfo(def, true).regStr;
						if (def.Register == Register.ES)
							sb.Append(regStr);
						else
							WriteRegOp(regStr);
						break;

					case OperandEncoding.RegMemModrmRm:
						(gprSizeBits, regStr) = GetRegInfo(def);
						if (gprSizeBits > 0)
							WriteGprMem(gprSizeBits);
						else
							WriteRegMem(regStr);
						break;

					case OperandEncoding.MemModrmRm:
						if (def.SibRequired)
							sb.Append("sibmem");
						else if (def.MIB)
							sb.Append("mib");
						else if (def.Vsib) {
							if (encoding == EncodingKind.MVEX)
								sb.Append("mvt");
							else {
								var sz = def.Vsib32 ? "32" : "64";
								// x, y, z
								var reg = regDefs[(int)def.Register].Name.ToLowerInvariant()[0..1];
								sb.Append($"vm{sz}{reg}");
							}
						}
						else
							WriteMemory();
						break;

					case OperandEncoding.MemOffset:
						sb.Append("moffs");
						WriteMemorySize(GetMemorySize(isBroadcast: false));
						break;

					case OperandEncoding.None:
					default:
						throw new InvalidOperationException();
					}

					if (i == 0) {
						if ((flags1 & InstructionDefFlags1.OpMaskRegister) != 0) {
							sb.Append("_k1");
							if ((flags1 & InstructionDefFlags1.ZeroingMasking) != 0)
								sb.Append('z');
						}
					}
					if (i == opKinds.Length - 1 && encoding != EncodingKind.MVEX) {
						if ((flags1 & InstructionDefFlags1.SuppressAllExceptions) != 0)
							sb.Append("_sae");
						if ((flags1 & InstructionDefFlags1.RoundingControl) != 0)
							sb.Append("_er");
					}
				}
			}

			if (codeSuffix is not null) {
				sb.Append('_');
				sb.Append(codeSuffix);
			}

			return sb.ToString();
		}

		static (int gprSizeBits, string regStr) GetRegInfo(OpCodeOperandKindDef def, bool kr = false) {
			string suffix;
			if (def.RegPlus1)
				suffix = "p1";
			else if (def.RegPlus3)
				suffix = "p3";
			else
				suffix = string.Empty;
			var (gprSizeBits, regStr) = def.Register switch {
				Register.AL => (8, "r8"),
				Register.AX => (16, "r16"),
				Register.EAX => (32, "r32"),
				Register.RAX => (64, "r64"),
				Register.ES => (0, "Sreg"),
				Register.MM0 => (0, "mm"),
				Register.XMM0 => (0, "xmm"),
				Register.YMM0 => (0, "ymm"),
				Register.ZMM0 => (0, "zmm"),
				Register.TMM0 => (0, "tmm"),
				Register.BND0 => (0, "bnd"),
				Register.K0 => (0, suffix == string.Empty && kr ? "kr" : "k"),
				Register.CR0 => (0, "cr"),
				Register.DR0 => (0, "dr"),
				Register.TR0 => (0, "tr"),
				Register.ST0 => (0, "sti"),
				_ => throw new InvalidOperationException(),
			};
			return (gprSizeBits, regStr + suffix);
		}

		void WriteGprMem(int regSize) {
			sb.Append('r');
			int memSize = GetSizeInBytes(GetMemorySize(isBroadcast: false)) * 8;
			if (memSize != regSize)
				sb.Append(regSize);
			WriteMemory();
		}

		void WriteRegMem(string register) {
			WriteRegOp(register);
			WriteMemory();
		}

		void WriteMemory() {
			WriteMemory(isBroadcast: false);
			if ((flags1 & InstructionDefFlags1.Broadcast) != 0)
				WriteMemory(isBroadcast: true);
		}

		void WriteMemory(bool isBroadcast) {
			var memorySize = GetMemorySize(isBroadcast);
			if (encoding == EncodingKind.MVEX)
				sb.Append((mvexFlags & MvexInfoFlags1.EvictionHint) != 0 ? "mt" : "m");
			else
				sb.Append(isBroadcast ? 'b' : 'm');
			WriteMemorySize(memorySize);
		}

		void WriteMemorySize(MemorySize memorySize) {
			if (codeMemorySize is not null)
				sb.Append(codeMemorySize);
			else {
				int memSize = GetSizeInBytes(memorySize);
				if (memSize != 0)
					sb.Append(memSize * 8);
			}

			if (codeMemorySizeSuffix is not null)
				sb.Append(codeMemorySizeSuffix);
		}

		void WriteRegister(string register) => sb.Append(register.ToUpperInvariant());
		void WriteRegOp(string register) => sb.Append(register.ToLowerInvariant());
	}
}
