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
using System.Text;
using Generator.Enums;

namespace Generator.Tables {
	readonly struct CodeFormatter {
		readonly StringBuilder sb;
		readonly MemorySizeInfoTable memSizeTbl;
		readonly string codeMnemonic;
		readonly string? codeSuffix;
		readonly string? codeMemorySize;
		readonly string? codeMemorySizeSuffix;
		readonly EnumValue memSize;
		readonly EnumValue memSizeBcst;
		readonly InstructionDefFlags1 flags;
		readonly EncodingKind encoding;
		readonly OpCodeOperandKindDef[] opKinds;

		public CodeFormatter(StringBuilder sb, MemorySizeInfoTable memSizeTbl, string codeMnemonic, string? codeSuffix, string? codeMemorySize, string? codeMemorySizeSuffix, EnumValue memSize, EnumValue memSizeBcst, InstructionDefFlags1 flags, EncodingKind encoding, OpCodeOperandKindDef[] opKinds) {
			if (codeMnemonic == string.Empty)
				throw new ArgumentOutOfRangeException(nameof(codeMnemonic));
			this.sb = sb;
			this.memSizeTbl = memSizeTbl;
			this.codeMnemonic = codeMnemonic;
			this.codeSuffix = codeSuffix;
			this.codeMemorySize = codeMemorySize;
			this.codeMemorySizeSuffix = codeMemorySizeSuffix;
			this.memSize = memSize;
			this.memSizeBcst = memSizeBcst;
			this.flags = flags;
			this.encoding = encoding;
			this.opKinds = opKinds;
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
						sb.Append($"rel{def.BranchOffsetSize * 8}");
						break;

					case OperandEncoding.AbsNearBranch:
						sb.Append($"disp{def.BranchOffsetSize * 8}");
						break;

					case OperandEncoding.FarBranch:
						sb.Append($"ptr16{def.BranchOffsetSize * 8}");
						break;

					case OperandEncoding.Immediate:
						sb.Append($"imm{def.ImmediateSize * 8}");
						break;

					case OperandEncoding.ImmediateM2z:
						sb.Append("imm2");
						break;

					case OperandEncoding.ImpliedConst:
						sb.Append(def.ImpliedConst.ToString());
						break;

					case OperandEncoding.ImpliedRegister:
						if (def.Register == Register.ST0)
							sb.Append("st0");
						else
							WriteRegister(def.Register.ToString());
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
					case OperandEncoding.RegVvvv:
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
							var sz = def.Vsib32 ? "32" : "64";
							// x, y, z
							var reg = def.Register.ToString().ToLowerInvariant().Substring(0, 1);
							sb.Append($"vm{sz}{reg}");
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
						if ((flags & InstructionDefFlags1.OpMaskRegister) != 0) {
							sb.Append("_k1");
							if ((flags & InstructionDefFlags1.ZeroingMasking) != 0)
								sb.Append("z");
						}
					}
					if (i == opKinds.Length - 1) {
						if ((flags & InstructionDefFlags1.SuppressAllExceptions) != 0)
							sb.Append("_sae");
						if ((flags & InstructionDefFlags1.RoundingControl) != 0)
							sb.Append("_er");
					}
				}
			}

			if (codeSuffix is object) {
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
			if ((flags & InstructionDefFlags1.Broadcast) != 0)
				WriteMemory(isBroadcast: true);
		}

		void WriteMemory(bool isBroadcast) {
			var memorySize = GetMemorySize(isBroadcast);
			sb.Append(isBroadcast ? 'b' : 'm');
			WriteMemorySize(memorySize);
		}

		void WriteMemorySize(MemorySize memorySize) {
			if (codeMemorySize is object)
				sb.Append(codeMemorySize);
			else {
				int memSize = GetSizeInBytes(memorySize);
				if (memSize != 0)
					sb.Append(memSize * 8);
			}

			if (codeMemorySizeSuffix is object)
				sb.Append(codeMemorySizeSuffix);
		}

		void WriteRegister(string register) => sb.Append(register.ToUpperInvariant());
		void WriteRegOp(string register) => sb.Append(register.ToLowerInvariant());
	}
}
