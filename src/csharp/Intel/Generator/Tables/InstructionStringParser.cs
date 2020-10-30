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
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Generator.Enums;

namespace Generator.Tables {
	struct InstructionStringParser {
		readonly string instrStr;
		readonly string mnemonic;
		readonly string[] operands;
		ParsedInstructionFlags instrFlags;

		public InstructionStringParser(string instrStr) {
			this.instrStr = instrStr;
			int index = instrStr.IndexOf(' ', StringComparison.Ordinal);
			if (index < 0)
				index = instrStr.Length;
			mnemonic = instrStr.Substring(0, index);
			var opsStr = instrStr.Substring(index).Trim();
			operands = opsStr == string.Empty ? Array.Empty<string>() : opsStr.Split(',').Select(a => a.Trim()).ToArray();
			instrFlags = ParsedInstructionFlags.None;
		}

		public bool TryParse(out ParsedInstructionResult result, [NotNullWhen(false)] out string? error) {
			result = default;

			// Check if it's INVALID, db, dw, dd, dq
			if (instrStr.StartsWith("<", StringComparison.Ordinal) && instrStr.EndsWith(">", StringComparison.Ordinal)) {
				result = new ParsedInstructionResult(instrStr, ParsedInstructionFlags.None, instrStr, Array.Empty<ParsedInstructionOperand>(), Array.Empty<InstrStrImpliedOp>());
				error = null;
				return true;
			}

			var sb = new StringBuilder();
			sb.Append(mnemonic);

			int opIndex;
			var parsedOps = new List<ParsedInstructionOperand>();
			int realOps = 0;
			for (opIndex = 0; opIndex < operands.Length; opIndex++) {
				var op = operands[opIndex];
				if (op.Length == 0) {
					error = "Empty instruction operand";
					return false;
				}
				if (op[0] == '<')
					break;

				var opFlags = ParsedInstructionOperandFlags.None;

				if (op[0] == '[' && op[^1] == ']') {
					op = op[1..^1];
					opFlags |= ParsedInstructionOperandFlags.HiddenOperand;
				}
				else {
					if (realOps == 0)
						sb.Append(' ');
					else
						sb.Append(", ");
					sb.Append(op);
					realOps++;
				}

				if (!TryParseDecorators(op, out var newOp, out error))
					return false;
				op = newOp;

				var opParts = op.Split('/').Select(a => a.Trim()).ToArray();

				var firstPart = opParts[0];
				if (firstPart.EndsWith("+1", StringComparison.Ordinal)) {
					firstPart = firstPart.Substring(0, firstPart.Length - "+1".Length);
					opFlags |= ParsedInstructionOperandFlags.RegPlus1;
				}
				else if (firstPart.EndsWith("+3", StringComparison.Ordinal)) {
					firstPart = firstPart.Substring(0, firstPart.Length - "+3".Length);
					opFlags |= ParsedInstructionOperandFlags.RegPlus3;
				}
				if (firstPart == "r") {
					if (opParts.Length < 2) {
						error = "Missing GPR size (missing mXX)";
						return false;
					}
					switch (opParts[1]) {
					case "m8": firstPart = "r8"; break;
					case "m16": firstPart = "r16"; break;
					case "m32": firstPart = "r32"; break;
					case "m64": firstPart = "r64"; break;
					default:
						error = $"Can't detect GPR size, memory op: `{opParts[1]}`";
						return false;
					}
				}
				var register = Register.None;
				int sizeBits = 0;
				switch (firstPart) {
				case "1":
					opFlags |= ParsedInstructionOperandFlags.ConstImmediate;
					break;

				case "AL": register = Register.AL; opFlags |= ParsedInstructionOperandFlags.ImpliedRegister; break;
				case "CL": register = Register.CL; opFlags |= ParsedInstructionOperandFlags.ImpliedRegister; break;
				case "AX": register = Register.AX; opFlags |= ParsedInstructionOperandFlags.ImpliedRegister; break;
				case "DX": register = Register.DX; opFlags |= ParsedInstructionOperandFlags.ImpliedRegister; break;
				case "EAX": register = Register.EAX; opFlags |= ParsedInstructionOperandFlags.ImpliedRegister; break;
				case "RAX": register = Register.RAX; opFlags |= ParsedInstructionOperandFlags.ImpliedRegister; break;
				case "ST": register = Register.ST0; opFlags |= ParsedInstructionOperandFlags.ImpliedRegister; break;
				case "ST(0)": register = Register.ST0; opFlags |= ParsedInstructionOperandFlags.ImpliedRegister; break;
				case "ES": register = Register.ES; opFlags |= ParsedInstructionOperandFlags.ImpliedRegister; break;
				case "CS": register = Register.CS; opFlags |= ParsedInstructionOperandFlags.ImpliedRegister; break;
				case "SS": register = Register.SS; opFlags |= ParsedInstructionOperandFlags.ImpliedRegister; break;
				case "DS": register = Register.DS; opFlags |= ParsedInstructionOperandFlags.ImpliedRegister; break;
				case "FS": register = Register.FS; opFlags |= ParsedInstructionOperandFlags.ImpliedRegister; break;
				case "GS": register = Register.GS; opFlags |= ParsedInstructionOperandFlags.ImpliedRegister; break;

				case "bnd":
				case "bnd1":
				case "bnd2":
					register = Register.BND0;
					opFlags |= ParsedInstructionOperandFlags.Register;
					break;

				case "cr":
					register = Register.CR0;
					opFlags |= ParsedInstructionOperandFlags.Register;
					break;

				case "dr":
					register = Register.DR0;
					opFlags |= ParsedInstructionOperandFlags.Register;
					break;

				case "k1":
				case "k2":
				case "k3":
					register = Register.K0;
					opFlags |= ParsedInstructionOperandFlags.Register;
					break;

				case "mm":
				case "mm1":
				case "mm2":
					register = Register.MM0;
					opFlags |= ParsedInstructionOperandFlags.Register;
					break;

				case "r8":
					register = Register.AL;
					opFlags |= ParsedInstructionOperandFlags.Register;
					break;

				case "r16":
					register = Register.AX;
					opFlags |= ParsedInstructionOperandFlags.Register;
					break;

				case "r32":
				case "r32a":
				case "r32b":
					register = Register.EAX;
					opFlags |= ParsedInstructionOperandFlags.Register;
					break;

				case "r64":
				case "r64a":
				case "r64b":
					register = Register.RAX;
					opFlags |= ParsedInstructionOperandFlags.Register;
					break;

				case "Sreg":
					register = Register.ES;
					opFlags |= ParsedInstructionOperandFlags.Register;
					break;

				case "ST(i)":
					register = Register.ST0;
					opFlags |= ParsedInstructionOperandFlags.Register;
					break;

				case "tmm1":
				case "tmm2":
				case "tmm3":
					register = Register.TMM0;
					opFlags |= ParsedInstructionOperandFlags.Register;
					break;

				case "tr":
					register = Register.TR0;
					opFlags |= ParsedInstructionOperandFlags.Register;
					break;

				case "xmm":
				case "xmm1":
				case "xmm2":
				case "xmm3":
				case "xmm4":
					register = Register.XMM0;
					opFlags |= ParsedInstructionOperandFlags.Register;
					break;

				case "ymm1":
				case "ymm2":
				case "ymm3":
				case "ymm4":
					register = Register.YMM0;
					opFlags |= ParsedInstructionOperandFlags.Register;
					break;

				case "zmm1":
				case "zmm2":
				case "zmm3":
					register = Register.ZMM0;
					opFlags |= ParsedInstructionOperandFlags.Register;
					break;

				case "disp16":
				case "disp32":
					sizeBits = int.Parse(firstPart.Substring("disp".Length));
					opFlags |= ParsedInstructionOperandFlags.DispBranch;
					break;

				case "ptr16:16":
				case "ptr16:32":
					sizeBits = int.Parse(firstPart.Substring("ptr16:".Length));
					opFlags |= ParsedInstructionOperandFlags.FarBranch;
					break;

				case "rel8":
				case "rel16":
				case "rel32":
					sizeBits = int.Parse(firstPart.Substring("rel".Length));
					opFlags |= ParsedInstructionOperandFlags.RelBranch;
					break;

				case "imm2":
				case "imm8":
				case "imm16":
				case "imm32":
				case "imm64":
					sizeBits = int.Parse(firstPart.Substring("imm".Length));
					opFlags |= ParsedInstructionOperandFlags.Immediate;
					break;

				case "moffs8":
				case "moffs16":
				case "moffs32":
				case "moffs64":
					opFlags |= ParsedInstructionOperandFlags.MemoryOffset | ParsedInstructionOperandFlags.Memory;
					break;

				case "vm32x":
				case "vm32y":
				case "vm32z":
				case "vm64x":
				case "vm64y":
				case "vm64z":
					opFlags |= ParsedInstructionOperandFlags.Memory | ParsedInstructionOperandFlags.Vsib;
					sizeBits = int.Parse(firstPart.Substring(2, 2));
					register = (firstPart[^1]) switch {
						'x' => Register.XMM0,
						'y' => Register.YMM0,
						'z' => Register.ZMM0,
						_ => throw new InvalidOperationException(),
					};
					break;

				case "m":
				case "mem":
					opFlags |= ParsedInstructionOperandFlags.Memory;
					break;
				case "mib":
					opFlags |= ParsedInstructionOperandFlags.Memory | ParsedInstructionOperandFlags.MIB;
					break;
				case "sibmem":
					opFlags |= ParsedInstructionOperandFlags.Memory | ParsedInstructionOperandFlags.Sibmem;
					break;

				default:
					if (firstPart.StartsWith("m", StringComparison.Ordinal) && firstPart.Length >= 2 && char.IsDigit(firstPart[1])) {
						opFlags |= ParsedInstructionOperandFlags.Memory;
						break;
					}
					else {
						error = $"Unknown value: `{firstPart}`";
						return false;
					}
				}

				if (opParts.Length >= 2) {
					var part = opParts[1];
					if (part.StartsWith("m", StringComparison.Ordinal) && part.Length >= 2 && char.IsDigit(part[1]))
						opFlags |= ParsedInstructionOperandFlags.Memory;
					else {
						error = $"Unknown value: `{part}`";
						return false;
					}
				}

				if (opParts.Length >= 3) {
					var part = opParts[2];
					if (part.StartsWith("m", StringComparison.Ordinal) && part.Length >= 2 && char.IsDigit(part[1]) && part.EndsWith("bcst", StringComparison.Ordinal))
						opFlags |= ParsedInstructionOperandFlags.Broadcast;
					else {
						error = $"Unknown value: `{part}`";
						return false;
					}
				}

				if (opParts.Length >= 4) {
					error = "Too many reg/mem parts";
					return false;
				}

				parsedOps.Add(new ParsedInstructionOperand(opFlags, register, sizeBits));
			}

			var impliedOps = new List<InstrStrImpliedOp>();
			for (; opIndex < operands.Length; opIndex++) {
				var op = operands[opIndex];
				if (op.Length == 0) {
					error = "Empty instruction operand";
					return false;
				}

				if (realOps == 0)
					sb.Append(' ');
				else
					sb.Append(", ");
				sb.Append(op);
				realOps++;

				if (op[0] == '<') {
					if (op[^1] != '>') {
						error = "Implied operands must be enclosed in < >";
						return false;
					}
					if (op.ToUpperInvariant() != op && op.ToLowerInvariant() != op) {
						error = $"Implied operands must be lower case or upper case: `{op}`";
						return false;
					}
					impliedOps.Add(new InstrStrImpliedOp(op));
				}
				else {
					error = "All implied operands must be the last operands";
					return false;
				}
			}

			result = new ParsedInstructionResult(sb.ToString(), instrFlags, mnemonic, parsedOps.ToArray(), impliedOps.ToArray());
			error = null;
			return true;
		}

		bool TryParseDecorators(string op, [NotNullWhen(true)] out string? newOp, [NotNullWhen(false)] out string? error) {
			newOp = null;

			var parts = op.Split('{').Select(a => a.Trim()).ToArray();
			for (int i = 1; i < parts.Length; i++) {
				var decorator = parts[i];
				switch (decorator) {
				case "k1}":
				case "k2}":
					instrFlags |= ParsedInstructionFlags.OpMask;
					break;
				case "z}":
					instrFlags |= ParsedInstructionFlags.ZeroingMasking;
					break;
				case "sae}":
					instrFlags |= ParsedInstructionFlags.SuppressAllExceptions;
					break;
				case "er}":
					instrFlags |= ParsedInstructionFlags.RoundingControl;
					break;
				default:
					error = $"Unknown decorator: `{{{decorator}`";
					return false;
				}
			}

			newOp = parts[0];
			error = null;
			return true;
		}
	}
}
