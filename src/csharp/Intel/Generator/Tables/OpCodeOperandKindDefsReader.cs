// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Generator.Enums;
using Generator.Enums.Encoder;

namespace Generator.Tables {
	sealed class OpCodeOperandKindDefsReader {
		readonly string filename;
		readonly Dictionary<string, EnumValue> toRegister;
		readonly Dictionary<string, EnumValue> toOpCodeOperandKind;

		public OpCodeOperandKindDefsReader(GenTypes genTypes, string filename) {
			this.filename = filename;
			toRegister = CreateEnumDict(genTypes[TypeIds.Register], true);
			toOpCodeOperandKind = CreateEnumDict(genTypes[TypeIds.OpCodeOperandKind]);
		}

		static Dictionary<string, EnumValue> CreateEnumDict(EnumType enumType, bool ignoreCase = false) =>
			enumType.Values.ToDictionary(a => a.RawName, a => a, ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);

		public OpCodeOperandKindDef[] Read() {
			var defs = new List<OpCodeOperandKindDef>();

			var argCounts = new Dictionary<string, int>(StringComparer.Ordinal) {
				{ "br-near", 2 },
				{ "br-near-x", 1 },
				{ "br-disp", 1 },
				{ "br-far", 1 },
				{ "imm8-const", 1 },
				{ "imm", 2 },
				{ "imp-reg", 1 },
				{ "isx", 2 },
				{ "opcode", 1 },
				{ "reg", 1 },
				{ "rm-reg", 1 },
				{ "vvvv", 1 },
				{ "rm", 1 },
				{ "vsib", 2 },
			};

			var lines = File.ReadAllLines(filename);
			for (int i = 0; i < lines.Length; i++) {
				var line = lines[i];
				if (line.Length == 0 || line[0] == '#')
					continue;

				var parts = line.Split(',').Select(a => a.Trim()).ToArray();
				if (parts.Length != 3)
					throw new InvalidOperationException($"Line {i + 1}: Expected 2 commas");

				if (!toOpCodeOperandKind.TryGetValue(parts[0], out var enumValue))
					throw new InvalidOperationException($"Line {i + 1}: Invalid enum name or duplicate def: {parts[0]}");
				toOpCodeOperandKind.Remove(parts[0]);

				var flags = OpCodeOperandKindDefFlags.None;
				foreach (var flagStr in parts[2].Split(' ', StringSplitOptions.RemoveEmptyEntries)) {
					flags |= flagStr switch {
						"lock-bit" => OpCodeOperandKindDefFlags.LockBit,
						"p1" => OpCodeOperandKindDefFlags.RegPlus1,
						"p3" => OpCodeOperandKindDefFlags.RegPlus3,
						"mem" => OpCodeOperandKindDefFlags.Memory,
						"mpx" => OpCodeOperandKindDefFlags.MPX,
						"mib" => OpCodeOperandKindDefFlags.MIB,
						"sib" => OpCodeOperandKindDefFlags.SibRequired,
						_ => throw new InvalidOperationException($"Line {i + 1}: Invalid flag: {flagStr}"),
					};
				}

				var (key, value) = ParserUtils.GetKeyValue(parts[1]);
				argCounts.TryGetValue(key, out var argCount);
				var args = value == string.Empty ? Array.Empty<string>() : value.Split(';');
				if (args.Length != argCount)
					throw new InvalidOperationException($"Line {i + 1}: Expected {argCount} args but got {args.Length}: {value}");
				OpCodeOperandKindDef def;
				int arg1, arg2;
				Register register;
				switch (key) {
				case "none":
					def = new OpCodeOperandKindDef(enumValue, flags, OperandEncoding.None, 0, 0, Register.None);
					break;

				case "br-near":
					arg1 = int.Parse(args[0]);
					arg2 = int.Parse(args[1]);
					def = new OpCodeOperandKindDef(enumValue, flags, OperandEncoding.NearBranch, arg1, arg2, Register.None);
					break;

				case "br-near-x":
					arg1 = int.Parse(args[0]);
					def = new OpCodeOperandKindDef(enumValue, flags, OperandEncoding.Xbegin, arg1, 0, Register.None);
					break;

				case "br-disp":
					arg1 = int.Parse(args[0]);
					def = new OpCodeOperandKindDef(enumValue, flags, OperandEncoding.AbsNearBranch, arg1, 0, Register.None);
					break;

				case "br-far":
					arg1 = int.Parse(args[0]);
					def = new OpCodeOperandKindDef(enumValue, flags, OperandEncoding.FarBranch, arg1, 0, Register.None);
					break;

				case "imm4":
					flags |= OpCodeOperandKindDefFlags.M2Z;
					def = new OpCodeOperandKindDef(enumValue, flags, OperandEncoding.Immediate, 4, 4, Register.None);
					break;

				case "imm8-const":
					arg1 = int.Parse(args[0]);
					def = new OpCodeOperandKindDef(enumValue, flags, OperandEncoding.ImpliedConst, arg1, 0, Register.None);
					break;

				case "imm":
					arg1 = int.Parse(args[0]);
					arg2 = int.Parse(args[1]);
					def = new OpCodeOperandKindDef(enumValue, flags, OperandEncoding.Immediate, arg1, arg2, Register.None);
					break;

				case "imp-reg":
					register = (Register)toRegister[args[0]].Value;
					def = new OpCodeOperandKindDef(enumValue, flags, OperandEncoding.ImpliedRegister, 0, 0, register);
					break;

				case "seg-rbx-al":
					flags |= OpCodeOperandKindDefFlags.Memory;
					def = new OpCodeOperandKindDef(enumValue, flags, OperandEncoding.SegRBX, 0, 0, Register.None);
					break;

				case "seg-rsi":
					flags |= OpCodeOperandKindDefFlags.Memory;
					def = new OpCodeOperandKindDef(enumValue, flags, OperandEncoding.SegRSI, 0, 0, Register.None);
					break;

				case "seg-rdi":
					flags |= OpCodeOperandKindDefFlags.Memory;
					def = new OpCodeOperandKindDef(enumValue, flags, OperandEncoding.SegRDI, 0, 0, Register.None);
					break;

				case "es-rdi":
					flags |= OpCodeOperandKindDefFlags.Memory;
					def = new OpCodeOperandKindDef(enumValue, flags, OperandEncoding.ESRDI, 0, 0, Register.None);
					break;

				case "isx":
					register = (Register)toRegister[args[0]].Value;
					switch (int.Parse(args[1])) {
					case 4: break;
					case 5: flags |= OpCodeOperandKindDefFlags.Is5; break;
					default: throw new InvalidOperationException();
					}
					def = new OpCodeOperandKindDef(enumValue, flags, OperandEncoding.RegImm, 0, 0, register);
					break;

				case "opcode":
					register = (Register)toRegister[args[0]].Value;
					def = new OpCodeOperandKindDef(enumValue, flags, OperandEncoding.RegOpCode, 0, 0, register);
					break;

				case "reg":
					flags |= OpCodeOperandKindDefFlags.Modrm;
					register = (Register)toRegister[args[0]].Value;
					def = new OpCodeOperandKindDef(enumValue, flags, OperandEncoding.RegModrmReg, 0, 0, register);
					break;

				case "rm-reg":
					flags |= OpCodeOperandKindDefFlags.Modrm;
					register = (Register)toRegister[args[0]].Value;
					def = new OpCodeOperandKindDef(enumValue, flags, OperandEncoding.RegModrmRm, 0, 0, register);
					break;

				case "vvvv":
					register = (Register)toRegister[args[0]].Value;
					def = new OpCodeOperandKindDef(enumValue, flags, OperandEncoding.RegVvvvv, 0, 0, register);
					break;

				case "rm":
					flags |= OpCodeOperandKindDefFlags.Modrm;
					flags |= OpCodeOperandKindDefFlags.Memory;
					register = (Register)toRegister[args[0]].Value;
					def = new OpCodeOperandKindDef(enumValue, flags, OperandEncoding.RegMemModrmRm, 0, 0, register);
					break;

				case "rm-mem":
					flags |= OpCodeOperandKindDefFlags.Modrm;
					flags |= OpCodeOperandKindDefFlags.Memory;
					def = new OpCodeOperandKindDef(enumValue, flags, OperandEncoding.MemModrmRm, 0, 0, Register.None);
					break;

				case "vsib":
					flags |= OpCodeOperandKindDefFlags.Modrm;
					flags |= OpCodeOperandKindDefFlags.Memory;
					register = (Register)toRegister[args[0]].Value;
					flags |= (int.Parse(args[1])) switch {
						32 => OpCodeOperandKindDefFlags.Vsib32,
						64 => OpCodeOperandKindDefFlags.Vsib64,
						_ => throw new InvalidOperationException($"Line {i + 1}: Unknown vsib size: {args[1]}"),
					};
					def = new OpCodeOperandKindDef(enumValue, flags, OperandEncoding.MemModrmRm, 0, 0, register);
					break;

				case "moffs":
					flags |= OpCodeOperandKindDefFlags.Memory;
					def = new OpCodeOperandKindDef(enumValue, flags, OperandEncoding.MemOffset, 0, 0, Register.None);
					break;

				default:
					throw new InvalidOperationException($"Line {i + 1}: Unknown key: {key}");
				}
				defs.Add(def);
			}
			if (toOpCodeOperandKind.Count != 0)
				throw new InvalidOperationException($"Missing {nameof(OpCodeOperandKind)} definitions");

			return defs.OrderBy(a => a.EnumValue.Value).ToArray();
		}
	}
}
