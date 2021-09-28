// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Generator.Enums;
using Generator.Enums.Encoder;

namespace Generator.Tables {
	struct OpCodeStringParser {
		const string D3nowPrefix = "0F 0F /r ";
		readonly string opCodeStr;

		enum VecEncoding {
			VEX,
			XOP,
			EVEX,
			MVEX,
		}

		public OpCodeStringParser(string opCodeStr) =>
			this.opCodeStr = opCodeStr;

		public bool TryParse(out OpCodeDef result, [NotNullWhen(false)] out string? error) {
			// Check if it's INVALID, db, dw, dd, dq
			if (opCodeStr.StartsWith("<", StringComparison.Ordinal) && opCodeStr.EndsWith(">", StringComparison.Ordinal)) {
				result = OpCodeDef.CreateDefault(EncodingKind.Legacy);
				result.OpCodeLength = 1;
				error = null;
				return true;
			}
			if (opCodeStr.StartsWith(D3nowPrefix, StringComparison.Ordinal))
				return TryParse3DNow(out result, out error);
			if (opCodeStr.StartsWith("VEX.", StringComparison.Ordinal))
				return TryParseVec(VecEncoding.VEX, out result, out error);
			if (opCodeStr.StartsWith("XOP.", StringComparison.Ordinal))
				return TryParseVec(VecEncoding.XOP, out result, out error);
			if (opCodeStr.StartsWith("EVEX.", StringComparison.Ordinal))
				return TryParseVec(VecEncoding.EVEX, out result, out error);
			if (opCodeStr.StartsWith("MVEX.", StringComparison.Ordinal))
				return TryParseVec(VecEncoding.MVEX, out result, out error);
			return TryParseLegacy(out result, out error);
		}

		bool TryParseLegacy(out OpCodeDef result, [NotNullWhen(false)] out string? error) {
			result = OpCodeDef.CreateDefault(EncodingKind.Legacy);

			var parts = opCodeStr.Split(' ');
			int index = 0;
			for (; index < parts.Length; index++) {
				var part = parts[index];
				switch (part) {
				case "9B":
					// Check if it's the instruction and not the 'prefix'
					if (index + 1 == parts.Length)
						break;
					result.Flags |= ParsedOpCodeFlags.Fwait;
					continue;

				case "o16":
				case "o32":
				case "o64":
					if (result.OperandSize != CodeSize.Unknown) {
						error = $"Duplicate operand size: `{part}`";
						return false;
					}
					result.OperandSize = part switch {
						"o16" => CodeSize.Code16,
						"o32" => CodeSize.Code32,
						"o64" => CodeSize.Code64,
						_ => throw new InvalidOperationException(),
					};
					continue;

				case "REX.W":
					// We use o64 instead of REX.W because it doesn't make any sense to use REX.W if it's an instruction with
					// default op size == 64 (no REX.W needed). For consistency, we use o64 even if the default op size == 32.
					error = "Use o64 instead of REX.W";
					return false;

				case "a16":
				case "a32":
				case "a64":
					if (result.AddressSize != CodeSize.Unknown) {
						error = $"Duplicate address size: `{part}`";
						return false;
					}
					result.AddressSize = part switch {
						"a16" => CodeSize.Code16,
						"a32" => CodeSize.Code32,
						"a64" => CodeSize.Code64,
						_ => throw new InvalidOperationException(),
					};
					continue;

				case "NP":
				case "66":
				case "F3":
				case "F2":
					if (result.MandatoryPrefix != MandatoryPrefix.None) {
						error = $"Duplicate mandatory prefix: `{part}`";
						return false;
					}
					result.MandatoryPrefix = part switch {
						"NP" => MandatoryPrefix.PNP,
						"66" => MandatoryPrefix.P66,
						"F3" => MandatoryPrefix.PF3,
						"F2" => MandatoryPrefix.PF2,
						_ => throw new InvalidOperationException(),
					};
					continue;
				}
				break;
			}

			uint opCode = 0;
			var table = OpCodeTableKind.Normal;
			int opCodeByteCount = 0;
			for (; index < parts.Length; index++) {
				var part = parts[index];
				int plusIndex = part.IndexOf('+', StringComparison.Ordinal);
				if (plusIndex >= 0)
					part = part[0..plusIndex];
				if (!TryParseHexByte(part, out byte opCodeByte, out _))
					break;
				if (opCodeByteCount == 2) {
					if (table != OpCodeTableKind.Normal) {
						error = "Too many opcode bytes";
						return false;
					}
					if (opCode == 0x0F38) {
						table = OpCodeTableKind.T0F38;
						opCode = 0;
						opCodeByteCount = 1;
					}
					else if (opCode == 0x0F3A) {
						table = OpCodeTableKind.T0F3A;
						opCode = 0;
						opCodeByteCount = 1;
					}
					else {
						var highByte = (byte)(opCode >> 8);
						if (highByte != 0x0F) {
							error = $"Unsupported opcode, expected first byte to be 0F";
							return false;
						}
						table = OpCodeTableKind.T0F;
						opCode = (byte)opCode;
					}
				}
				else
					opCodeByteCount++;
				opCode = (opCode << 8) | opCodeByte;
			}
			if (opCodeByteCount == 0) {
				error = "Missing opcode byte";
				return false;
			}
			if (opCodeByteCount == 2) {
				byte hi = (byte)(opCode >> 8);
				switch (hi) {
				case 0x0F:
					if (table != OpCodeTableKind.Normal) {
						error = $"Invalid opcode, expected normal table but got `{table}`";
						return false;
					}
					opCode = (byte)opCode;
					table = OpCodeTableKind.T0F;
					opCodeByteCount--;
					break;

				case 0x38:
					if (table == OpCodeTableKind.T0F) {
						opCode = (byte)opCode;
						table = OpCodeTableKind.T0F38;
						opCodeByteCount--;
					}
					break;

				case 0x3A:
					if (table == OpCodeTableKind.T0F) {
						opCode = (byte)opCode;
						table = OpCodeTableKind.T0F3A;
						opCodeByteCount--;
					}
					break;

				case 0x39:
				case 0x3B:
				case 0x3C:
				case 0x3D:
				case 0x3E:
				case 0x3F:
					if (table == OpCodeTableKind.T0F) {
						error = $"Unsupported table 0F{hi:X2}";
						return false;
					}
					break;

				default:
					break;
				}
			}
			result.OpCode = opCode;
			result.OpCodeLength = opCodeByteCount;
			result.Table = table;

			for (; index < parts.Length; index++) {
				var part = parts[index];
				if (!TryParseArg(part, ref result, out error))
					return false;
			}

			error = null;
			return true;
		}

		static bool TryParseArg(string value, ref OpCodeDef result, [NotNullWhen(false)] out string? error) {
			if (value.Length == 2 && value[0] == '/' && '0' <= value[1] && value[1] <= '9') {
				result.GroupIndex = (sbyte)(value[1] - '0');
				if (result.GroupIndex > 7) {
					error = $"Invalid group index, must be 0-7: `{value}`";
					return false;
				}
			}
			else if (IsModRegRmString(value)) {
				if (!TryParseModRegRm(value, ref result.Flags, ref result.GroupIndex, ref result.RmGroupIndex, out error))
					return false;
			}
			else {
				switch (value) {
				case "/is4": result.Flags |= ParsedOpCodeFlags.Is4; break;
				case "/is5": result.Flags |= ParsedOpCodeFlags.Is5; break;
				case "/vsib": result.Flags |= ParsedOpCodeFlags.Vsib; break;

				case "/r":
				case "cb":
				case "cd":
				case "cp":
				case "cw":
				case "ib":
				case "id":
				case "io":
				case "iw":
				case "mo":
					break;

				default:
					throw new InvalidOperationException();
				}
			}

			error = null;
			return true;
		}

		bool TryParse3DNow(out OpCodeDef result, [NotNullWhen(false)] out string? error) {
			result = OpCodeDef.CreateDefault(EncodingKind.D3NOW);
			var hexByteStr = opCodeStr[D3nowPrefix.Length..];
			if (!TryParseHexByte(hexByteStr, out byte opCode, out error))
				return false;

			result.Table = OpCodeTableKind.T0F;
			result.OpCode = opCode;
			result.OpCodeLength = 1;
			return true;
		}

		bool TryParseVec(VecEncoding vecEnc, out OpCodeDef result, [NotNullWhen(false)] out string? error) {
			var encoding = vecEnc switch {
				VecEncoding.VEX => EncodingKind.VEX,
				VecEncoding.XOP => EncodingKind.XOP,
				VecEncoding.EVEX => EncodingKind.EVEX,
				VecEncoding.MVEX => EncodingKind.MVEX,
				_ => throw new InvalidOperationException(),
			};
			result = OpCodeDef.CreateDefault(encoding);

			var parts = opCodeStr.Split(' ');
			var encParts = parts[0].Split('.');
			OpCodeTableKind? table = null;
			for (int i = 1; i < encParts.Length; i++) {
				var encPart = encParts[i];
				switch (encPart) {
				case "0F":
				case "0F38":
				case "0F3A":
				case "MAP5":
				case "MAP6":
				case "X8":
				case "X9":
				case "XA":
					if (table is not null) {
						error = $"Duplicate table: `{encPart}`";
						return false;
					}
					table = encPart switch {
						"0F" => OpCodeTableKind.T0F,
						"0F38" => OpCodeTableKind.T0F38,
						"0F3A" => OpCodeTableKind.T0F3A,
						"MAP5" => OpCodeTableKind.MAP5,
						"MAP6" => OpCodeTableKind.MAP6,
						"X8" => OpCodeTableKind.MAP8,
						"X9" => OpCodeTableKind.MAP9,
						"XA" => OpCodeTableKind.MAP10,
						_ => throw new InvalidOperationException(),
					};
					break;

				case "NP":
				case "66":
				case "F3":
				case "F2":
					if (result.MandatoryPrefix != MandatoryPrefix.None) {
						error = $"Duplicate mandatory prefix: `{encPart}`";
						return false;
					}
					result.MandatoryPrefix = encPart switch {
						"NP" => MandatoryPrefix.PNP,
						"66" => MandatoryPrefix.P66,
						"F3" => MandatoryPrefix.PF3,
						"F2" => MandatoryPrefix.PF2,
						_ => throw new InvalidOperationException(),
					};
					break;

				case "L0":
				case "L1":
				case "LIG":
				case "LZ":
				case "128":
				case "256":
				case "512":
					if (result.LBit != OpCodeL.None) {
						error = $"Duplicate L bit: `{encPart}`";
						return false;
					}
					result.LBit = encPart switch {
						"L0" => OpCodeL.L0,
						"L1" => OpCodeL.L1,
						"LIG" => OpCodeL.LIG,
						"LZ" => OpCodeL.LZ,
						"128" => OpCodeL.L128,
						"256" => OpCodeL.L256,
						"512" => OpCodeL.L512,
						_ => throw new InvalidOperationException(),
					};
					break;

				case "W0":
				case "W1":
				case "WIG":
					if (result.WBit != OpCodeW.None) {
						error = $"Duplicate W bit: `{encPart}`";
						return false;
					}
					result.WBit = encPart switch {
						"W0" => OpCodeW.W0,
						"W1" => OpCodeW.W1,
						"WIG" => OpCodeW.WIG,
						_ => throw new InvalidOperationException(),
					};
					break;

				case "NDS":
				case "NDD":
					if (result.NDKind != NonDestructiveOpKind.None) {
						error = $"Duplicate NDD/NDS: `{encPart}`";
						return false;
					}
					result.NDKind = encPart switch {
						"NDS" => NonDestructiveOpKind.NDS,
						"NDD" => NonDestructiveOpKind.NDD,
						_ => throw new InvalidOperationException(),
					};
					break;

				case "EH0":
				case "EH1":
					if (result.MvexEHBit != MvexEHBit.None) {
						error = $"Duplicate EH bit: `{encPart}`";
						return false;
					}
					result.MvexEHBit = encPart switch {
						"EH0" => MvexEHBit.EH0,
						"EH1" => MvexEHBit.EH1,
						_ => throw new InvalidOperationException(),
					};
					break;

				default:
					error = $"Unknown opcode value `{encPart}`";
					return false;
				}
			}
			if (result.MandatoryPrefix == MandatoryPrefix.None)
				result.MandatoryPrefix = MandatoryPrefix.PNP;
			result.Table = table ?? OpCodeTableKind.Normal;
			if (result.LBit == OpCodeL.None) {
				error = "Missing L bit, eg. L128";
				return false;
			}
			if (result.WBit == OpCodeW.None) {
				error = "Missing W bit, eg. W0";
				return false;
			}

			if (parts.Length < 2) {
				error = "Missing opcode";
				return false;
			}
			if (!TryParseHexByte(parts[1], out byte opCode1, out error))
				return false;
			result.OpCode = opCode1;

			bool canParseOpCode = true;
			int opCodeByteCount = 1;
			for (int pi = 2; pi < parts.Length; pi++) {
				var part = parts[pi];
				if (canParseOpCode && TryParseHexByte(part, out byte opCode2, out _)) {
					opCodeByteCount++;
					if (opCodeByteCount > 2) {
						error = $"Found an extra opcode byte `{part}";
						return false;
					}
					result.OpCode = (result.OpCode << 8) | opCode2;
				}
				else {
					canParseOpCode = false;
					if (TryParseHexByte(part, out _, out _)) {
						error = $"Found an extra opcode byte `{part}";
						return false;
					}
					if (!TryParseArg(part, ref result, out error))
						return false;
				}
			}
			result.OpCodeLength = opCodeByteCount;

			if (result.NDKind != NonDestructiveOpKind.None && vecEnc != VecEncoding.MVEX) {
				error = "Can't use NDD/NDS";
				return false;
			}
			if (result.MvexEHBit != MvexEHBit.None && vecEnc != VecEncoding.MVEX) {
				error = "Can't use EH0/EH1";
				return false;
			}

			switch (vecEnc) {
			case VecEncoding.VEX:
				switch (result.LBit) {
				case OpCodeL.L0:
				case OpCodeL.L1:
				case OpCodeL.LIG:
				case OpCodeL.LZ:
				case OpCodeL.L128:
				case OpCodeL.L256:
					break;
				case OpCodeL.None:
				case OpCodeL.L512:
				default:
					error = $"Invalid L bit: {result.LBit}";
					return false;
				}
				switch (result.Table) {
				case OpCodeTableKind.Normal:
				case OpCodeTableKind.T0F:
				case OpCodeTableKind.T0F38:
				case OpCodeTableKind.T0F3A:
					break;
				case OpCodeTableKind.MAP5:
				case OpCodeTableKind.MAP6:
				case OpCodeTableKind.MAP8:
				case OpCodeTableKind.MAP9:
				case OpCodeTableKind.MAP10:
				default:
					error = $"Invalid table: {result.Table}";
					return false;
				}
				break;

			case VecEncoding.XOP:
				switch (result.LBit) {
				case OpCodeL.L0:
				case OpCodeL.L1:
				case OpCodeL.LIG:
				case OpCodeL.LZ:
				case OpCodeL.L128:
				case OpCodeL.L256:
					break;
				case OpCodeL.None:
				case OpCodeL.L512:
				default:
					error = $"Invalid L bit: {result.LBit}";
					return false;
				}
				switch (result.Table) {
				case OpCodeTableKind.MAP8:
				case OpCodeTableKind.MAP9:
				case OpCodeTableKind.MAP10:
					break;
				case OpCodeTableKind.Normal:
				case OpCodeTableKind.T0F:
				case OpCodeTableKind.T0F38:
				case OpCodeTableKind.T0F3A:
				case OpCodeTableKind.MAP5:
				case OpCodeTableKind.MAP6:
				default:
					error = $"Invalid table: {result.Table}";
					return false;
				}
				break;

			case VecEncoding.EVEX:
				switch (result.LBit) {
				case OpCodeL.L0:
				case OpCodeL.L1:
				case OpCodeL.LIG:
				case OpCodeL.LZ:
				case OpCodeL.L128:
				case OpCodeL.L256:
				case OpCodeL.L512:
					break;
				case OpCodeL.None:
				default:
					error = $"Invalid L bit: {result.LBit}";
					return false;
				}
				switch (result.Table) {
				case OpCodeTableKind.T0F:
				case OpCodeTableKind.T0F38:
				case OpCodeTableKind.T0F3A:
				case OpCodeTableKind.MAP5:
				case OpCodeTableKind.MAP6:
					break;
				case OpCodeTableKind.Normal:
				case OpCodeTableKind.MAP8:
				case OpCodeTableKind.MAP9:
				case OpCodeTableKind.MAP10:
				default:
					error = $"Invalid table: {result.Table}";
					return false;
				}
				break;

			case VecEncoding.MVEX:
				switch (result.LBit) {
				case OpCodeL.L512:
					break;
				case OpCodeL.None:
				case OpCodeL.L0:
				case OpCodeL.L1:
				case OpCodeL.LIG:
				case OpCodeL.LZ:
				case OpCodeL.L128:
				case OpCodeL.L256:
				default:
					error = $"Invalid L bit: {result.LBit}";
					return false;
				}
				switch (result.Table) {
				case OpCodeTableKind.T0F:
				case OpCodeTableKind.T0F38:
				case OpCodeTableKind.T0F3A:
					break;
				case OpCodeTableKind.Normal:
				case OpCodeTableKind.MAP5:
				case OpCodeTableKind.MAP6:
				case OpCodeTableKind.MAP8:
				case OpCodeTableKind.MAP9:
				case OpCodeTableKind.MAP10:
				default:
					error = $"Invalid table: {result.Table}";
					return false;
				}
				break;

			default:
				throw new InvalidOperationException();
			}

			return true;
		}

		static bool IsModRegRmString(string s) =>
			s.Contains(':', StringComparison.Ordinal);

		static bool TryParseModRegRm(string value, ref ParsedOpCodeFlags flags, ref sbyte groupIndex, ref sbyte rmGroupIndex, [NotNullWhen(false)] out string? error) {
			if (groupIndex >= 0 || rmGroupIndex >= 0) {
				error = $"Extra mod-reg-rm value `{value}`";
				return false;
			}

			var parts = value.Split(':');
			if (parts.Length != 3) {
				error = $"Expected 2 colons: `{value}`";
				return false;
			}

			bool isReg = parts[0] == "11";
			if (!isReg && parts[0] != "!(11)") {
				error = $"Expected first part to be `11` or `!(11)` but got `{parts[0]}`";
				return false;
			}

			if (!TryParseBits(parts[1], "rrr", out groupIndex, out error))
				return false;
			if (!TryParseBits(parts[2], "bbb", out rmGroupIndex, out error))
				return false;

			if (isReg) {
				if (groupIndex >= 0 && rmGroupIndex >= 0) {
					error = "Both group index and rm group index can't be used, use a 2-byte opcode instead, eg. `01 CF`";
					return false;
				}
			}
			else {
				if (rmGroupIndex == 4)
					rmGroupIndex = -1;
				else if (rmGroupIndex >= 0) {
					error = $"Invalid rm bits `{parts[2]}`";
					return false;
				}
				if (groupIndex >= 0 && rmGroupIndex >= 0) {
					error = "Both group index and rm group index can't be used with modrm mem instructions";
					return false;
				}
			}

			flags |= ParsedOpCodeFlags.ModRegRmString;
			return true;
		}

		static bool TryParseBits(string value, string defaultValue, out sbyte result, [NotNullWhen(false)] out string? error) {
			if (value == defaultValue) {
				result = -1;
				error = null;
				return true;
			}

			switch (value) {
			case "000": result = 0; break;
			case "001": result = 1; break;
			case "010": result = 2; break;
			case "011": result = 3; break;
			case "100": result = 4; break;
			case "101": result = 5; break;
			case "110": result = 6; break;
			case "111": result = 7; break;
			default:
				result = -1;
				error = $"Invalid binary value `{value}`. Expected a 3-digit binary value or `{defaultValue}`";
				return false;
			}

			error = null;
			return true;
		}

		static bool TryParseHexByte(string value, out byte parsedValue, [NotNullWhen(false)] out string? error) {
			if (value.ToUpperInvariant() != value || !byte.TryParse(value, NumberStyles.AllowHexSpecifier, null, out parsedValue)) {
				error = $"Invalid hex byte value `{value}`. It must be uppercase hex.";
				parsedValue = 0;
				return false;
			}
			else {
				error = null;
				return true;
			}
		}
	}
}
