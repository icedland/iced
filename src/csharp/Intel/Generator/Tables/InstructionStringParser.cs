// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Generator.Enums;

namespace Generator.Tables {
	struct InstructionStringParser {
		readonly Dictionary<string, EnumValue> toRegister;
		readonly string instrStr;
		readonly string mnemonic;
		readonly string[] operands;
		ParsedInstructionFlags instrFlags;

		public InstructionStringParser(Dictionary<string, EnumValue> toRegister, string instrStr) {
			this.toRegister = toRegister;
			this.instrStr = instrStr;
			int index = instrStr.IndexOf(' ', StringComparison.Ordinal);
			if (index < 0)
				index = instrStr.Length;
			mnemonic = instrStr[0..index];
			var opsStr = instrStr[index..].Trim();
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
					op = op[1..^1].Trim();
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
					firstPart = firstPart[0..(firstPart.Length - "+1".Length)];
					opFlags |= ParsedInstructionOperandFlags.RegPlus1;
				}
				else if (firstPart.EndsWith("+3", StringComparison.Ordinal)) {
					firstPart = firstPart[0..(firstPart.Length - "+3".Length)];
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
					sizeBits = int.Parse(firstPart.AsSpan()["disp".Length..]);
					opFlags |= ParsedInstructionOperandFlags.DispBranch;
					break;

				case "ptr16:16":
				case "ptr16:32":
					sizeBits = int.Parse(firstPart.AsSpan()["ptr16:".Length..]);
					opFlags |= ParsedInstructionOperandFlags.FarBranch;
					break;

				case "rel8":
				case "rel16":
				case "rel32":
					sizeBits = int.Parse(firstPart.AsSpan()["rel".Length..]);
					opFlags |= ParsedInstructionOperandFlags.RelBranch;
					break;

				case "imm4":
				case "imm8":
				case "imm16":
				case "imm32":
				case "imm64":
					sizeBits = int.Parse(firstPart.AsSpan()["imm".Length..]);
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
					sizeBits = int.Parse(firstPart.AsSpan()[2..4]);
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
					if (int.TryParse(firstPart, out _)) {
						opFlags |= ParsedInstructionOperandFlags.ConstImmediate;
						break;
					}
					else if (firstPart.StartsWith("m", StringComparison.Ordinal) && firstPart.Length >= 2 && char.IsDigit(firstPart[1])) {
						opFlags |= ParsedInstructionOperandFlags.Memory;
						break;
					}
					else if (firstPart.ToUpperInvariant() == firstPart) {
						if (firstPart == "ST(0)" || firstPart == "ST")
							firstPart = "ST0";
						if (toRegister.TryGetValue(firstPart, out var regEnum)) {
							register = (Register)regEnum.Value;
							opFlags |= ParsedInstructionOperandFlags.ImpliedRegister;
							break;
						}
					}
					error = $"Unknown value: `{firstPart}`";
					return false;
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
						error = $"Implied operands must be lowercase or uppercase: `{op}`";
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
