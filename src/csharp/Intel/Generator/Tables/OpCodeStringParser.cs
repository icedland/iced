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
using System.Diagnostics.CodeAnalysis;
using Generator.Enums;
using Generator.Enums.Encoder;

namespace Generator.Tables {
	[Flags]
	enum ParsedOpCodeFlags : byte {
		None			= 0,
		Fwait			= 0x01,
		ModRegRmString	= 0x02,
	}

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
					if (result.OperandSize != CodeSize.Unknown) {
						error = $"Duplicate operand size: `{part}`";
						return false;
					}
					result.OperandSize = CodeSize.Code16;
					continue;

				case "o32":
					if (result.OperandSize != CodeSize.Unknown) {
						error = $"Duplicate operand size: `{part}`";
						return false;
					}
					result.OperandSize = CodeSize.Code32;
					continue;

				case "o64":
					if (result.OperandSize != CodeSize.Unknown) {
						error = $"Duplicate operand size: `{part}`";
						return false;
					}
					result.OperandSize = CodeSize.Code64;
					continue;

				case "a16":
					if (result.AddressSize != CodeSize.Unknown) {
						error = $"Duplicate address size: `{part}`";
						return false;
					}
					result.AddressSize = CodeSize.Code16;
					continue;

				case "a32":
					if (result.AddressSize != CodeSize.Unknown) {
						error = $"Duplicate address size: `{part}`";
						return false;
					}
					result.AddressSize = CodeSize.Code32;
					continue;

				case "a64":
					if (result.AddressSize != CodeSize.Unknown) {
						error = $"Duplicate address size: `{part}`";
						return false;
					}
					result.AddressSize = CodeSize.Code64;
					continue;

				case "NP":
					if (result.MandatoryPrefix != MandatoryPrefix.None) {
						error = $"Duplicate mandatory prefix: `{part}`";
						return false;
					}
					result.MandatoryPrefix = MandatoryPrefix.PNP;
					continue;

				case "66":
					if (result.MandatoryPrefix != MandatoryPrefix.None) {
						error = $"Duplicate mandatory prefix: `{part}`";
						return false;
					}
					result.MandatoryPrefix = MandatoryPrefix.P66;
					continue;

				case "F3":
					if (result.MandatoryPrefix != MandatoryPrefix.None) {
						error = $"Duplicate mandatory prefix: `{part}`";
						return false;
					}
					result.MandatoryPrefix = MandatoryPrefix.PF3;
					continue;

				case "F2":
					if (result.MandatoryPrefix != MandatoryPrefix.None) {
						error = $"Duplicate mandatory prefix: `{part}`";
						return false;
					}
					result.MandatoryPrefix = MandatoryPrefix.PF2;
					continue;
				}
				break;
			}

			uint opCode = 0;
			var table = OpCodeTableKind.Normal;
			int opCodeByteCount = 0;
			for (; index < parts.Length; index++) {
				var part = parts[index];
				int plusIndex = part.IndexOf('+');
				if (plusIndex >= 0)
					part = part.Substring(0, plusIndex);
				if (!TryParseHexByte(part, out byte opCodeByte, out _))
					break;
				if (opCodeByteCount == 2) {
					if (table != OpCodeTableKind.Normal) {
						error = "Too many opcode bytes";
						return false;
					}
					var highByte = (byte)(opCode >> 8);
					if (highByte != 0x0F) {
						error = $"Unsupported opcode, expected first byte to be 0F";
						return false;
					}
					table = OpCodeTableKind.T0F;
					opCode = (byte)opCode;
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

			error = null;
			return true;
		}

		bool TryParse3DNow(out OpCodeDef result, [NotNullWhen(false)] out string? error) {
			result = OpCodeDef.CreateDefault(EncodingKind.D3NOW);
			var hexByteStr = opCodeStr.Substring(D3nowPrefix.Length);
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
				VecEncoding.MVEX => throw new InvalidOperationException(),
				_ => throw new InvalidOperationException(),
			};
			result = OpCodeDef.CreateDefault(encoding);

			var parts = opCodeStr.Split(' ');
			var encParts = parts[0].Split('.');
			for (int i = 1; i < encParts.Length; i++) {
				var encPart = encParts[i];
				switch (encPart) {
				case "0F":
					if (result.Table != OpCodeTableKind.Normal) {
						error = $"Duplicate table: `{encPart}`";
						return false;
					}
					result.Table = OpCodeTableKind.T0F;
					break;
				case "0F38":
					if (result.Table != OpCodeTableKind.Normal) {
						error = $"Duplicate table: `{encPart}`";
						return false;
					}
					result.Table = OpCodeTableKind.T0F38;
					break;
				case "0F3A":
					if (result.Table != OpCodeTableKind.Normal) {
						error = $"Duplicate table: `{encPart}`";
						return false;
					}
					result.Table = OpCodeTableKind.T0F3A;
					break;
				case "X8":
					if (result.Table != OpCodeTableKind.Normal) {
						error = $"Duplicate table: `{encPart}`";
						return false;
					}
					result.Table = OpCodeTableKind.XOP8;
					break;
				case "X9":
					if (result.Table != OpCodeTableKind.Normal) {
						error = $"Duplicate table: `{encPart}`";
						return false;
					}
					result.Table = OpCodeTableKind.XOP9;
					break;
				case "XA":
					if (result.Table != OpCodeTableKind.Normal) {
						error = $"Duplicate table: `{encPart}`";
						return false;
					}
					result.Table = OpCodeTableKind.XOPA;
					break;

				case "NP":
					if (result.MandatoryPrefix != MandatoryPrefix.None) {
						error = $"Duplicate mandatory prefix: `{encPart}`";
						return false;
					}
					result.MandatoryPrefix = MandatoryPrefix.PNP;
					break;
				case "66":
					if (result.MandatoryPrefix != MandatoryPrefix.None) {
						error = $"Duplicate mandatory prefix: `{encPart}`";
						return false;
					}
					result.MandatoryPrefix = MandatoryPrefix.P66;
					break;
				case "F3":
					if (result.MandatoryPrefix != MandatoryPrefix.None) {
						error = $"Duplicate mandatory prefix: `{encPart}`";
						return false;
					}
					result.MandatoryPrefix = MandatoryPrefix.PF3;
					break;
				case "F2":
					if (result.MandatoryPrefix != MandatoryPrefix.None) {
						error = $"Duplicate mandatory prefix: `{encPart}`";
						return false;
					}
					result.MandatoryPrefix = MandatoryPrefix.PF2;
					break;

				case "L0":
					if (result.LBit != OpCodeL.None) {
						error = $"Duplicate L bit: `{encPart}`";
						return false;
					}
					result.LBit = OpCodeL.L0;
					break;
				case "L1":
					if (result.LBit != OpCodeL.None) {
						error = $"Duplicate L bit: `{encPart}`";
						return false;
					}
					result.LBit = OpCodeL.L1;
					break;
				case "LIG":
					if (result.LBit != OpCodeL.None) {
						error = $"Duplicate L bit: `{encPart}`";
						return false;
					}
					result.LBit = OpCodeL.LIG;
					break;
				case "LZ":
					if (result.LBit != OpCodeL.None) {
						error = $"Duplicate L bit: `{encPart}`";
						return false;
					}
					result.LBit = OpCodeL.LZ;
					break;
				case "128":
					if (result.LBit != OpCodeL.None) {
						error = $"Duplicate L bit: `{encPart}`";
						return false;
					}
					result.LBit = OpCodeL.L128;
					break;
				case "256":
					if (result.LBit != OpCodeL.None) {
						error = $"Duplicate L bit: `{encPart}`";
						return false;
					}
					result.LBit = OpCodeL.L256;
					break;
				case "512":
					if (result.LBit != OpCodeL.None) {
						error = $"Duplicate L bit: `{encPart}`";
						return false;
					}
					result.LBit = OpCodeL.L512;
					break;

				case "W0":
					if (result.WBit != OpCodeW.None) {
						error = $"Duplicate W bit: `{encPart}`";
						return false;
					}
					result.WBit = OpCodeW.W0;
					break;
				case "W1":
					if (result.WBit != OpCodeW.None) {
						error = $"Duplicate W bit: `{encPart}`";
						return false;
					}
					result.WBit = OpCodeW.W1;
					break;
				case "WIG":
					if (result.WBit != OpCodeW.None) {
						error = $"Duplicate W bit: `{encPart}`";
						return false;
					}
					result.WBit = OpCodeW.WIG;
					break;

				default:
					error = $"Unknown opcode value `{encPart}`";
					return false;
				}
			}
			if (result.MandatoryPrefix == MandatoryPrefix.None)
				result.MandatoryPrefix = MandatoryPrefix.PNP;
			if (result.Table == OpCodeTableKind.Normal) {
				error = "Missing table, eg. 0F, 0F38, 0F3A";
				return false;
			}
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
				case OpCodeTableKind.T0F:
				case OpCodeTableKind.T0F38:
				case OpCodeTableKind.T0F3A:
					break;
				case OpCodeTableKind.Normal:
				case OpCodeTableKind.XOP8:
				case OpCodeTableKind.XOP9:
				case OpCodeTableKind.XOPA:
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
				case OpCodeTableKind.XOP8:
				case OpCodeTableKind.XOP9:
				case OpCodeTableKind.XOPA:
					break;
				case OpCodeTableKind.Normal:
				case OpCodeTableKind.T0F:
				case OpCodeTableKind.T0F38:
				case OpCodeTableKind.T0F3A:
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
					break;
				case OpCodeTableKind.Normal:
				case OpCodeTableKind.XOP8:
				case OpCodeTableKind.XOP9:
				case OpCodeTableKind.XOPA:
				default:
					error = $"Invalid table: {result.Table}";
					return false;
				}
				break;

			case VecEncoding.MVEX:
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
			if (value.ToUpperInvariant() != value || !byte.TryParse(value, System.Globalization.NumberStyles.AllowHexSpecifier, null, out parsedValue)) {
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
