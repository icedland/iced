/*
    Copyright (C) 2018 de4dot@gmail.com

    This file is part of Iced.

    Iced is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Iced is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Iced.  If not, see <https://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.DecoderTests {
	public abstract class DecoderTest {
		protected Decoder CreateDecoder16(string hexBytes, [CallerMemberName] string callerName = null) =>
			CreateDecoder(16, callerName, hexBytes, DecoderOptions.None);
		protected Decoder CreateDecoder32(string hexBytes, [CallerMemberName] string callerName = null) =>
			CreateDecoder(32, callerName, hexBytes, DecoderOptions.None);
		protected Decoder CreateDecoder64(string hexBytes, [CallerMemberName] string callerName = null) =>
			CreateDecoder(64, callerName, hexBytes, DecoderOptions.None);

		protected Decoder CreateDecoder16(string hexBytes, DecoderOptions options, [CallerMemberName] string callerName = null) =>
			CreateDecoder(16, callerName, hexBytes, options);
		protected Decoder CreateDecoder32(string hexBytes, DecoderOptions options, [CallerMemberName] string callerName = null) =>
			CreateDecoder(32, callerName, hexBytes, options);
		protected Decoder CreateDecoder64(string hexBytes, DecoderOptions options, [CallerMemberName] string callerName = null) =>
			CreateDecoder(64, callerName, hexBytes, options);

		Decoder CreateDecoder(int codeSize, string callerName, string hexBytes, DecoderOptions options) {
			Assert.StartsWith("Test" + codeSize.ToString(), callerName);
			return CreateDecoder(codeSize, hexBytes, options);
		}

		Decoder CreateDecoder(int codeSize, string hexBytes, DecoderOptions options) {
			Decoder decoder;
			var codeReader = new ByteArrayCodeReader(hexBytes);
			switch (codeSize) {
			case 16:
				decoder = Decoder.Create16(codeReader, options);
				decoder.InstructionPointer = DecoderConstants.DEFAULT_IP16;
				break;

			case 32:
				decoder = Decoder.Create32(codeReader, options);
				decoder.InstructionPointer = DecoderConstants.DEFAULT_IP32;
				break;

			case 64:
				decoder = Decoder.Create64(codeReader, options);
				decoder.InstructionPointer = DecoderConstants.DEFAULT_IP64;
				break;

			default:
				throw new ArgumentOutOfRangeException(nameof(codeSize));
			}

			Assert.Equal(codeSize, decoder.Bitness);
			return decoder;
		}

		protected void DecodeMemOpsBase(int bitness, string hexBytes, int byteLength, Code code, Register register, Register prefixSeg, Register segReg, Register baseReg, Register indexReg, int scale, uint displ, int displSize) {
			var decoder = CreateDecoder(bitness, hexBytes, DecoderOptions.None);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(prefixSeg, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(segReg, instr.MemorySegment);
			Assert.Equal(baseReg, instr.MemoryBase);
			Assert.Equal(indexReg, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1 << scale, instr.MemoryIndexScale);
			Assert.Equal(displSize, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(register, instr.Op1Register);
		}

		static readonly Dictionary<string, Code> toCode = CreateToCode();
		static readonly Dictionary<string, Register> toRegister = CreateToRegister();
		static readonly char[] colSep = new char[] { ',' };

		static Dictionary<string, Code> CreateToCode() {
			var dict = new Dictionary<string, Code>(StringComparer.Ordinal);
			foreach (var f in typeof(Code).GetFields()) {
				if (!f.IsLiteral)
					continue;
				var code = (Code)f.GetValue(null);
				var name = f.Name;
				dict.Add(name, code);
			}
			return dict;
		}

		static Dictionary<string, Register> CreateToRegister() {
			var dict = new Dictionary<string, Register>(StringComparer.Ordinal);
			foreach (var f in typeof(Register).GetFields()) {
				if (!f.IsLiteral)
					continue;
				var code = (Register)f.GetValue(null);
				var name = f.Name;
				dict.Add(name, code);
			}
			return dict;
		}

		protected static IEnumerable<object[]> GetMemOpsData(string className) {
			var filename = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Intel", "DecoderTests", className + ".txt");
			Debug.Assert(File.Exists(filename));
			foreach (var line in File.ReadLines(filename)) {
				if (line.Length == 0 || line[0] == '#')
					continue;
				var parts = line.Split(colSep, StringSplitOptions.None);
				if (parts.Length != 11)
					throw new InvalidOperationException();
				string hexBytes = parts[0].Trim();
				int byteLength = (int)ParseUInt32(parts[1].Trim());
				var code = toCode[parts[2].Trim()];
				var register = toRegister[parts[3].Trim()];
				var prefixSeg = toRegister[parts[4].Trim()];
				var segReg = toRegister[parts[5].Trim()];
				var baseReg = toRegister[parts[6].Trim()];
				var indexReg = toRegister[parts[7].Trim()];
				int scale = (int)ParseUInt32(parts[8].Trim());
				uint displ = ParseUInt32(parts[9].Trim());
				int displSize = (int)ParseUInt32(parts[10].Trim());
				yield return new object[11] { hexBytes, byteLength, code, register, prefixSeg, segReg, baseReg, indexReg, scale, displ, displSize };
			}

			uint ParseUInt32(string s) {
				if (uint.TryParse(s, out uint value))
					return value;
				if (s.StartsWith("0x")) {
					s = s.Substring(2);
					if (uint.TryParse(s, NumberStyles.HexNumber, null, out value))
						return value;
				}

				throw new InvalidOperationException();
			}
		}
	}
}
