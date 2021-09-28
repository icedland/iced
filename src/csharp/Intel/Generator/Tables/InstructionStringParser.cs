// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Generator.Enums;

namespace Generator.Tables {
	struct InstructionStringParser {
		readonly Dictionary<string, EnumValue> toRegister;
		readonly EncodingKind encoding;
		readonly string instrStr;
		readonly string mnemonic;
		readonly string[] operands;
		ParsedInstructionFlags instrFlags;

		public InstructionStringParser(Dictionary<string, EnumValue> toRegister, EncodingKind encoding, string instrStr) {
			this.toRegister = toRegister;
			this.encoding = encoding;
			this.instrStr = instrStr;
			int index = instrStr.IndexOf(' ', StringComparison.Ordinal);
			if (index < 0)
				index = instrStr.Length;
			mnemonic = instrStr[0..index];
			var opsStr = instrStr[index..].Trim();
			operands = opsStr == string.Empty ? Array.Empty<string>() : opsStr.Split(',').Select(a => a.Trim()).ToArray();
			instrFlags = ParsedInstructionFlags.None;
		}

		static bool TryParseMemorySize(string s, out int value) {
			Debug.Assert(s.StartsWith("m", StringComparison.Ordinal));
			var s2 = s.AsSpan()["m".Length..];
			if (s2.EndsWith("bcst", StringComparison.Ordinal))
				s2 = s2[..^"bcst".Length];
			if (s2.EndsWith("byte", StringComparison.Ordinal))
				s2 = s2[..^"byte".Length];
			if (s2.EndsWith("int", StringComparison.Ordinal))
				s2 = s2[..^"int".Length];
			if (s2.EndsWith("fp", StringComparison.Ordinal))
				s2 = s2[..^"fp".Length];
			if (s2.EndsWith("bcd", StringComparison.Ordinal))
				s2 = s2[..^"bcd".Length];

			switch (s2.ToString()) {
			case "16&16":
			case "16:16":
				value = 32;
				return true;
			case "16&32":
			case "16:32":
				value = 48;
				return true;
			case "16&64":
			case "16:64":
				value = 80;
				return true;
			case "32&32":
				value = 64;
				return true;
			}

			return int.TryParse(s2, out value);
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

				var mvexConvFn = MvexConvFn.None;
				if (encoding == EncodingKind.MVEX) {
					if (!TryParseMvedConvFn(op, out mvexConvFn, out var op2, out error))
						return false;
					op = op2;
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
						error = $"Couldn't detect GPR size, memory op: `{opParts[1]}`";
						return false;
					}
				}
				opParts[0] = firstPart;

				var register = Register.None;
				int sizeBits = 0;
				int memSizeBits = 0;
				int memSize2Bits = 0;
				foreach (var part in opParts) {
					switch (part) {
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
						sizeBits = int.Parse(part.AsSpan()["disp".Length..]);
						opFlags |= ParsedInstructionOperandFlags.DispBranch;
						break;

					case "ptr16:16":
					case "ptr16:32":
						sizeBits = int.Parse(part.AsSpan()["ptr16:".Length..]);
						opFlags |= ParsedInstructionOperandFlags.FarBranch;
						break;

					case "rel8":
					case "rel16":
					case "rel32":
						sizeBits = int.Parse(part.AsSpan()["rel".Length..]);
						opFlags |= ParsedInstructionOperandFlags.RelBranch;
						break;

					case "imm4":
					case "imm8":
					case "imm16":
					case "imm32":
					case "imm64":
						sizeBits = int.Parse(part.AsSpan()["imm".Length..]);
						opFlags |= ParsedInstructionOperandFlags.Immediate;
						break;

					case "moffs8":
					case "moffs16":
					case "moffs32":
					case "moffs64":
						opFlags |= ParsedInstructionOperandFlags.MemoryOffset | ParsedInstructionOperandFlags.Memory;
						memSizeBits = int.Parse(part.AsSpan()["moffs".Length..]);
						break;

					case "vm32x":
					case "vm32y":
					case "vm32z":
					case "vm64x":
					case "vm64y":
					case "vm64z":
						opFlags |= ParsedInstructionOperandFlags.Memory | ParsedInstructionOperandFlags.Vsib;
						sizeBits = int.Parse(part.AsSpan()[2..4]);
						register = part[^1] switch {
							'x' => Register.XMM0,
							'y' => Register.YMM0,
							'z' => Register.ZMM0,
							_ => throw new InvalidOperationException(),
						};
						break;

					case "mt":
						if (encoding != EncodingKind.MVEX) {
							error = $"Expected MVEX encoding: {part}";
							return false;
						}
						opFlags |= ParsedInstructionOperandFlags.Memory;
						instrFlags |= ParsedInstructionFlags.EvictionHint;
						break;
					case "mvt":
						if (encoding != EncodingKind.MVEX) {
							error = $"Expected MVEX encoding: {part}";
							return false;
						}
						opFlags |= ParsedInstructionOperandFlags.Memory | ParsedInstructionOperandFlags.Vsib;
						sizeBits = 32;
						instrFlags |= ParsedInstructionFlags.EvictionHint;
						register = Register.ZMM0;
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
						if (int.TryParse(part, out _))
							opFlags |= ParsedInstructionOperandFlags.ConstImmediate;
						else if (part.StartsWith("m", StringComparison.Ordinal) && part.Length >= 2 && char.IsDigit(part[1]) && part.EndsWith("bcst", StringComparison.Ordinal)) {
							opFlags |= ParsedInstructionOperandFlags.Broadcast;
							if (!TryParseMemorySize(part, out memSize2Bits)) {
								error = $"Invalid memory size: {part}";
								return false;
							}
						}
						else if (part.StartsWith("m", StringComparison.Ordinal) && part.Length >= 2 && char.IsDigit(part[1])) {
							opFlags |= ParsedInstructionOperandFlags.Memory;
							if (!TryParseMemorySize(part, out memSizeBits)) {
								error = $"Invalid memory size: {part}";
								return false;
							}
						}
						else if (part.ToUpperInvariant() == part) {
							var reg = part;
							if (reg == "ST(0)" || reg == "ST")
								reg = "ST0";
							if (toRegister.TryGetValue(reg, out var regEnum)) {
								register = (Register)regEnum.Value;
								opFlags |= ParsedInstructionOperandFlags.ImpliedRegister;
							}
						}
						else {
							error = $"Unknown value: `{part}`";
							return false;
						}
						break;
					}
				}

				parsedOps.Add(new ParsedInstructionOperand(opFlags, register, sizeBits, memSizeBits, memSize2Bits, mvexConvFn));
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

		static bool TryParseMvedConvFn(string op, out MvexConvFn mvexConvFn, [NotNullWhen(true)] out string? newOp, [NotNullWhen(false)] out string? error) {
			newOp = null;
			mvexConvFn = MvexConvFn.None;

			int index = op.IndexOf('(');
			if (index < 0) {
				newOp = op;
				error = null;
				return true;
			}
			int endIndex = op.IndexOf(')');
			if (endIndex <= index) {
				error = "Expected ')'";
				return false;
			}
			var convFnStr = op[0..index];
			// Some of them have an opmask register after ')'
			newOp = op[(index + 1)..endIndex] + op[(endIndex + 1)..];
			var convFn = convFnStr switch {
				"Sf32" => MvexConvFn.Sf32,
				"Sf64" => MvexConvFn.Sf64,
				"Si32" => MvexConvFn.Si32,
				"Si64" => MvexConvFn.Si64,
				"Uf32" => MvexConvFn.Uf32,
				"Uf64" => MvexConvFn.Uf64,
				"Ui32" => MvexConvFn.Ui32,
				"Ui64" => MvexConvFn.Ui64,
				"Df32" => MvexConvFn.Df32,
				"Df64" => MvexConvFn.Df64,
				"Di32" => MvexConvFn.Di32,
				"Di64" => MvexConvFn.Di64,
				_ => (MvexConvFn?)null,
			};
			if (convFn is MvexConvFn convFn2)
				mvexConvFn = convFn2;
			else {
				error = $"Unknown MVEX conv fn: {convFnStr}";
				return false;
			}

			error = null;
			return true;
		}
	}
}
