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
using System.IO;
using Iced.Intel;

namespace Iced.UnitTests.Intel.DecoderTests {
	static class DecoderTestParser {
		const string Broadcast = "bcst";
		const string Xacquire = "xacquire";
		const string Xrelease = "xrelease";
		const string Rep = "rep";
		const string Repe = "repe";
		const string Repne = "repne";
		const string Lock = "lock";
		const string ZeroingMasking = "zmsk";
		const string SuppressAllExceptions = "sae";
		const string Vsib32 = "vsib32";
		const string Vsib64 = "vsib64";
		const string RoundToNearest = "rc-rn";
		const string RoundDown = "rc-rd";
		const string RoundUp = "rc-ru";
		const string RoundTowardZero = "rc-rz";
		const string Op0Kind = "op0";
		const string Op1Kind = "op1";
		const string Op2Kind = "op2";
		const string Op3Kind = "op3";
		const string Op4Kind = "op4";
		const string EncodedHexBytes = "enc";
		const string DecoderOptions_AMD = "amd";
		const string DecoderOptions_ForceReservedNop = "resnop";
		const string DecoderOptions_Cflsh = "cflsh";
		const string DecoderOptions_Umov = "umov";
		const string DecoderOptions_Ecr = "ecr";
		const string DecoderOptions_Xbts = "xbts";
		const string DecoderOptions_Cmpxchg486A = "cmpxchg486a";
		const string DecoderOptions_Zalloc = "zalloc";
		const string DecoderOptions_OldFpu = "oldfpu";
		const string DecoderOptions_Pcommit = "pcommit";
		const string DecoderOptions_Loadall286 = "loadall286";
		const string DecoderOptions_Loadall386 = "loadall386";
		const string DecoderOptions_Cl1invmb = "cl1invmb";
		const string DecoderOptions_MovTr = "movtr";
		const string SegmentPrefix_ES = "es:";
		const string SegmentPrefix_CS = "cs:";
		const string SegmentPrefix_SS = "ss:";
		const string SegmentPrefix_DS = "ds:";
		const string SegmentPrefix_FS = "fs:";
		const string SegmentPrefix_GS = "gs:";
		const string OpMask_k1 = "k1";
		const string OpMask_k2 = "k2";
		const string OpMask_k3 = "k3";
		const string OpMask_k4 = "k4";
		const string OpMask_k5 = "k5";
		const string OpMask_k6 = "k6";
		const string OpMask_k7 = "k7";
		const string ConstantOffsets = "co";

		const string OpKind_Register = "r";
		const string OpKind_NearBranch16 = "nb16";
		const string OpKind_NearBranch32 = "nb32";
		const string OpKind_NearBranch64 = "nb64";
		const string OpKind_FarBranch16 = "fb16";
		const string OpKind_FarBranch32 = "fb32";
		const string OpKind_Immediate8 = "i8";
		const string OpKind_Immediate16 = "i16";
		const string OpKind_Immediate32 = "i32";
		const string OpKind_Immediate64 = "i64";
		const string OpKind_Immediate8to16 = "i8to16";
		const string OpKind_Immediate8to32 = "i8to32";
		const string OpKind_Immediate8to64 = "i8to64";
		const string OpKind_Immediate32to64 = "i32to64";
		const string OpKind_Immediate8_2nd = "i8_2nd";
		const string OpKind_MemorySegSI = "segsi";
		const string OpKind_MemorySegESI = "segesi";
		const string OpKind_MemorySegRSI = "segrsi";
		const string OpKind_MemorySegDI = "segdi";
		const string OpKind_MemorySegEDI = "segedi";
		const string OpKind_MemorySegRDI = "segrdi";
		const string OpKind_MemoryESDI = "esdi";
		const string OpKind_MemoryESEDI = "esedi";
		const string OpKind_MemoryESRDI = "esrdi";
		const string OpKind_Memory64 = "m64";
		const string OpKind_Memory = "m";

		static readonly Dictionary<string, Code> toCode = CreateToCode();
		static readonly Dictionary<string, Register> toRegister = CreateToRegister();
		static readonly Dictionary<string, MemorySize> toMemorySize = CreateToMemorySize();

		static Dictionary<string, Code> CreateToCode() {
			var dict = new Dictionary<string, Code>(StringComparer.Ordinal);
			var names = Enum.GetNames(typeof(Code));
			var values = (Code[])Enum.GetValues(typeof(Code));
			if (names.Length != values.Length)
				throw new InvalidOperationException();
			for (int i = 0; i < names.Length; i++)
				dict.Add(names[i], values[i]);
			return dict;
		}

		static Dictionary<string, Register> CreateToRegister() {
			var dict = new Dictionary<string, Register>(StringComparer.OrdinalIgnoreCase);
			var names = Enum.GetNames(typeof(Register));
			var values = (Register[])Enum.GetValues(typeof(Register));
			if (names.Length != values.Length)
				throw new InvalidOperationException();
			for (int i = 0; i < names.Length; i++)
				dict.Add(names[i], values[i]);
			return dict;
		}

		static Dictionary<string, MemorySize> CreateToMemorySize() {
			var dict = new Dictionary<string, MemorySize>(StringComparer.Ordinal);
			var names = Enum.GetNames(typeof(MemorySize));
			var values = (MemorySize[])Enum.GetValues(typeof(MemorySize));
			if (names.Length != values.Length)
				throw new InvalidOperationException();
			for (int i = 0; i < names.Length; i++)
				dict.Add(names[i], values[i]);
			return dict;
		}

		public static IEnumerable<DecoderTestCase> ReadFile(int bitness, string filename) {
			int lineNo = 0;
			foreach (var line in File.ReadLines(filename)) {
				lineNo++;
				if (line.Length == 0 || line[0] == '#')
					continue;
				DecoderTestCase testCase;
				try {
					testCase = ReadTestCase(bitness, line, lineNo);
				}
				catch (Exception ex) {
					throw new InvalidOperationException($"Error parsing decoder test case file '{filename}', line {lineNo}: {ex.Message}");
				}
				yield return testCase;
			}
		}

		static readonly char[] seps = new char[] { ',' };
		static readonly char[] extraSeps = new char[] { ' ' };
		static DecoderTestCase ReadTestCase(int bitness, string line, int lineNo) {
			var parts = line.Split(seps);
			if (parts.Length != 4)
				throw new InvalidOperationException();

			var tc = new DecoderTestCase();
			tc.LineNumber = lineNo;
			tc.Bitness = bitness;
			tc.HexBytes = ToHexBytes(parts[0].Trim());
			tc.EncodedHexBytes = tc.HexBytes;
			tc.Code = ToCode(parts[1].Trim());
			tc.OpCount = ToInt32(parts[2].Trim());

			var rest = parts[3].Split(extraSeps);
			foreach (var tmp in rest) {
				if (tmp == string.Empty)
					continue;
				var key = tmp;
				string value;
				int index = key.IndexOf('=');
				if (index >= 0) {
					value = key.Substring(index + 1);
					key = key.Substring(0, index);
				}
				else
					value = null;
				switch (key) {
				case Broadcast:
					tc.IsBroadcast = true;
					break;

				case Xacquire:
					tc.HasXacquirePrefix = true;
					break;

				case Xrelease:
					tc.HasXreleasePrefix = true;
					break;

				case Rep:
				case Repe:
					tc.HasRepePrefix = true;
					break;

				case Repne:
					tc.HasRepnePrefix = true;
					break;

				case Lock:
					tc.HasLockPrefix = true;
					break;

				case ZeroingMasking:
					tc.ZeroingMasking = true;
					break;

				case SuppressAllExceptions:
					tc.SuppressAllExceptions = true;
					break;

				case Vsib32:
					tc.VsibBitness = 32;
					break;

				case Vsib64:
					tc.VsibBitness = 64;
					break;

				case RoundToNearest:
					tc.RoundingControl = RoundingControl.RoundToNearest;
					break;

				case RoundDown:
					tc.RoundingControl = RoundingControl.RoundDown;
					break;

				case RoundUp:
					tc.RoundingControl = RoundingControl.RoundUp;
					break;

				case RoundTowardZero:
					tc.RoundingControl = RoundingControl.RoundTowardZero;
					break;

				case Op0Kind:
					if (tc.OpCount < 1)
						throw new InvalidOperationException($"Invalid OpCount: {tc.OpCount} < 1");
					ReadOpKind(tc, 0, value, out tc.Op0Kind, ref tc.Op0Register);
					break;

				case Op1Kind:
					if (tc.OpCount < 2)
						throw new InvalidOperationException($"Invalid OpCount: {tc.OpCount} < 2");
					ReadOpKind(tc, 1, value, out tc.Op1Kind, ref tc.Op1Register);
					break;

				case Op2Kind:
					if (tc.OpCount < 3)
						throw new InvalidOperationException($"Invalid OpCount: {tc.OpCount} < 3");
					ReadOpKind(tc, 2, value, out tc.Op2Kind, ref tc.Op2Register);
					break;

				case Op3Kind:
					if (tc.OpCount < 4)
						throw new InvalidOperationException($"Invalid OpCount: {tc.OpCount} < 4");
					ReadOpKind(tc, 3, value, out tc.Op3Kind, ref tc.Op3Register);
					break;

				case Op4Kind:
					if (tc.OpCount < 5)
						throw new InvalidOperationException($"Invalid OpCount: {tc.OpCount} < 5");
					ReadOpKind(tc, 4, value, out tc.Op4Kind, ref tc.Op4Register);
					break;

				case EncodedHexBytes:
					if (string.IsNullOrWhiteSpace(value))
						throw new InvalidOperationException($"Invalid encoded hex bytes: '{value}'");
					tc.EncodedHexBytes = value;
					break;

				case DecoderOptions_AMD:
					tc.DecoderOptions |= DecoderOptions.AMD;
					break;

				case DecoderOptions_ForceReservedNop:
					tc.DecoderOptions |= DecoderOptions.ForceReservedNop;
					break;

				case DecoderOptions_Cflsh:
					tc.DecoderOptions |= DecoderOptions.Cflsh;
					break;

				case DecoderOptions_Umov:
					tc.DecoderOptions |= DecoderOptions.Umov;
					break;

				case DecoderOptions_Ecr:
					tc.DecoderOptions |= DecoderOptions.Ecr;
					break;

				case DecoderOptions_Xbts:
					tc.DecoderOptions |= DecoderOptions.Xbts;
					break;

				case DecoderOptions_Cmpxchg486A:
					tc.DecoderOptions |= DecoderOptions.Cmpxchg486A;
					break;

				case DecoderOptions_Zalloc:
					tc.DecoderOptions |= DecoderOptions.Zalloc;
					break;

				case DecoderOptions_OldFpu:
					tc.DecoderOptions |= DecoderOptions.OldFpu;
					break;

				case DecoderOptions_Pcommit:
					tc.DecoderOptions |= DecoderOptions.Pcommit;
					break;

				case DecoderOptions_Loadall286:
					tc.DecoderOptions |= DecoderOptions.Loadall286;
					break;

				case DecoderOptions_Loadall386:
					tc.DecoderOptions |= DecoderOptions.Loadall386;
					break;

				case DecoderOptions_Cl1invmb:
					tc.DecoderOptions |= DecoderOptions.Cl1invmb;
					break;

				case DecoderOptions_MovTr:
					tc.DecoderOptions |= DecoderOptions.MovTr;
					break;

				case SegmentPrefix_ES:
					tc.SegmentPrefix = Register.ES;
					break;

				case SegmentPrefix_CS:
					tc.SegmentPrefix = Register.CS;
					break;

				case SegmentPrefix_SS:
					tc.SegmentPrefix = Register.SS;
					break;

				case SegmentPrefix_DS:
					tc.SegmentPrefix = Register.DS;
					break;

				case SegmentPrefix_FS:
					tc.SegmentPrefix = Register.FS;
					break;

				case SegmentPrefix_GS:
					tc.SegmentPrefix = Register.GS;
					break;

				case OpMask_k1:
					tc.OpMask = Register.K1;
					break;

				case OpMask_k2:
					tc.OpMask = Register.K2;
					break;

				case OpMask_k3:
					tc.OpMask = Register.K3;
					break;

				case OpMask_k4:
					tc.OpMask = Register.K4;
					break;

				case OpMask_k5:
					tc.OpMask = Register.K5;
					break;

				case OpMask_k6:
					tc.OpMask = Register.K6;
					break;

				case OpMask_k7:
					tc.OpMask = Register.K7;
					break;

				case ConstantOffsets:
					if (!TryParseConstantOffsets(value, out tc.ConstantOffsets))
						throw new InvalidOperationException($"Invalid ConstantOffsets: '{value}'");
					break;

				default:
					throw new InvalidOperationException($"Invalid key '{key}'");
				}
			}

			return tc;
		}

		static readonly char[] coSeps = new char[] { ';' };
		static bool TryParseConstantOffsets(string value, out ConstantOffsets constantOffsets) {
			constantOffsets = default;
			if (value == null)
				return false;

			var parts = value.Split(coSeps);
			if (parts.Length != 6)
				return false;
			constantOffsets.ImmediateOffset = ToUInt8(parts[0]);
			constantOffsets.ImmediateSize = ToUInt8(parts[1]);
			constantOffsets.ImmediateOffset2 = ToUInt8(parts[2]);
			constantOffsets.ImmediateSize2 = ToUInt8(parts[3]);
			constantOffsets.DisplacementOffset = ToUInt8(parts[4]);
			constantOffsets.DisplacementSize = ToUInt8(parts[5]);
			return true;
		}

		static readonly char[] opKindSeps = new char[] { ';' };
		static void ReadOpKind(DecoderTestCase tc, int operand, string value, out OpKind opKind, ref Register opRegister) {
			var parts = value.Split(opKindSeps);
			switch (parts[0]) {
			case OpKind_Register:
				if (parts.Length != 2)
					throw new InvalidOperationException($"Operand {operand}: expected 2 values, actual = {parts.Length}");
				opRegister = ToRegister(parts[1]);
				opKind = OpKind.Register;
				break;

			case OpKind_NearBranch16:
				if (parts.Length != 2)
					throw new InvalidOperationException($"Operand {operand}: expected 2 values, actual = {parts.Length}");
				opKind = OpKind.NearBranch16;
				tc.NearBranch = ToUInt16(parts[1]);
				break;

			case OpKind_NearBranch32:
				if (parts.Length != 2)
					throw new InvalidOperationException($"Operand {operand}: expected 2 values, actual = {parts.Length}");
				opKind = OpKind.NearBranch32;
				tc.NearBranch = ToUInt32(parts[1]);
				break;

			case OpKind_NearBranch64:
				if (parts.Length != 2)
					throw new InvalidOperationException($"Operand {operand}: expected 2 values, actual = {parts.Length}");
				opKind = OpKind.NearBranch64;
				tc.NearBranch = ToUInt64(parts[1]);
				break;

			case OpKind_FarBranch16:
				if (parts.Length != 3)
					throw new InvalidOperationException($"Operand {operand}: expected 3 values, actual = {parts.Length}");
				opKind = OpKind.FarBranch16;
				tc.FarBranchSelector = ToUInt16(parts[1]);
				tc.FarBranch = ToUInt16(parts[2]);
				break;

			case OpKind_FarBranch32:
				if (parts.Length != 3)
					throw new InvalidOperationException($"Operand {operand}: expected 3 values, actual = {parts.Length}");
				opKind = OpKind.FarBranch32;
				tc.FarBranchSelector = ToUInt16(parts[1]);
				tc.FarBranch = ToUInt32(parts[2]);
				break;

			case OpKind_Immediate8:
				if (parts.Length != 2)
					throw new InvalidOperationException($"Operand {operand}: expected 2 values, actual = {parts.Length}");
				opKind = OpKind.Immediate8;
				tc.Immediate = ToUInt8(parts[1]);
				break;

			case OpKind_Immediate16:
				if (parts.Length != 2)
					throw new InvalidOperationException($"Operand {operand}: expected 2 values, actual = {parts.Length}");
				opKind = OpKind.Immediate16;
				tc.Immediate = ToUInt16(parts[1]);
				break;

			case OpKind_Immediate32:
				if (parts.Length != 2)
					throw new InvalidOperationException($"Operand {operand}: expected 2 values, actual = {parts.Length}");
				opKind = OpKind.Immediate32;
				tc.Immediate = ToUInt32(parts[1]);
				break;

			case OpKind_Immediate64:
				if (parts.Length != 2)
					throw new InvalidOperationException($"Operand {operand}: expected 2 values, actual = {parts.Length}");
				opKind = OpKind.Immediate64;
				tc.Immediate = ToUInt64(parts[1]);
				break;

			case OpKind_Immediate8to16:
				if (parts.Length != 2)
					throw new InvalidOperationException($"Operand {operand}: expected 2 values, actual = {parts.Length}");
				opKind = OpKind.Immediate8to16;
				tc.Immediate = ToUInt16(parts[1]);
				break;

			case OpKind_Immediate8to32:
				if (parts.Length != 2)
					throw new InvalidOperationException($"Operand {operand}: expected 2 values, actual = {parts.Length}");
				opKind = OpKind.Immediate8to32;
				tc.Immediate = ToUInt32(parts[1]);
				break;

			case OpKind_Immediate8to64:
				if (parts.Length != 2)
					throw new InvalidOperationException($"Operand {operand}: expected 2 values, actual = {parts.Length}");
				opKind = OpKind.Immediate8to64;
				tc.Immediate = ToUInt64(parts[1]);
				break;

			case OpKind_Immediate32to64:
				if (parts.Length != 2)
					throw new InvalidOperationException($"Operand {operand}: expected 2 values, actual = {parts.Length}");
				opKind = OpKind.Immediate32to64;
				tc.Immediate = ToUInt64(parts[1]);
				break;

			case OpKind_Immediate8_2nd:
				if (parts.Length != 2)
					throw new InvalidOperationException($"Operand {operand}: expected 2 values, actual = {parts.Length}");
				opKind = OpKind.Immediate8_2nd;
				tc.Immediate_2nd = ToUInt8(parts[1]);
				break;

			case OpKind_MemorySegSI:
				if (parts.Length != 3)
					throw new InvalidOperationException($"Operand {operand}: expected 3 values, actual = {parts.Length}");
				opKind = OpKind.MemorySegSI;
				tc.MemorySegment = ToRegister(parts[1]);
				tc.MemorySize = ToMemorySize(parts[2]);
				break;

			case OpKind_MemorySegESI:
				if (parts.Length != 3)
					throw new InvalidOperationException($"Operand {operand}: expected 3 values, actual = {parts.Length}");
				opKind = OpKind.MemorySegESI;
				tc.MemorySegment = ToRegister(parts[1]);
				tc.MemorySize = ToMemorySize(parts[2]);
				break;

			case OpKind_MemorySegRSI:
				if (parts.Length != 3)
					throw new InvalidOperationException($"Operand {operand}: expected 3 values, actual = {parts.Length}");
				opKind = OpKind.MemorySegRSI;
				tc.MemorySegment = ToRegister(parts[1]);
				tc.MemorySize = ToMemorySize(parts[2]);
				break;

			case OpKind_MemorySegDI:
				if (parts.Length != 3)
					throw new InvalidOperationException($"Operand {operand}: expected 3 values, actual = {parts.Length}");
				opKind = OpKind.MemorySegDI;
				tc.MemorySegment = ToRegister(parts[1]);
				tc.MemorySize = ToMemorySize(parts[2]);
				break;

			case OpKind_MemorySegEDI:
				if (parts.Length != 3)
					throw new InvalidOperationException($"Operand {operand}: expected 3 values, actual = {parts.Length}");
				opKind = OpKind.MemorySegEDI;
				tc.MemorySegment = ToRegister(parts[1]);
				tc.MemorySize = ToMemorySize(parts[2]);
				break;

			case OpKind_MemorySegRDI:
				if (parts.Length != 3)
					throw new InvalidOperationException($"Operand {operand}: expected 3 values, actual = {parts.Length}");
				opKind = OpKind.MemorySegRDI;
				tc.MemorySegment = ToRegister(parts[1]);
				tc.MemorySize = ToMemorySize(parts[2]);
				break;

			case OpKind_MemoryESDI:
				if (parts.Length != 2)
					throw new InvalidOperationException($"Operand {operand}: expected 2 values, actual = {parts.Length}");
				opKind = OpKind.MemoryESDI;
				tc.MemorySize = ToMemorySize(parts[1]);
				break;

			case OpKind_MemoryESEDI:
				if (parts.Length != 2)
					throw new InvalidOperationException($"Operand {operand}: expected 2 values, actual = {parts.Length}");
				opKind = OpKind.MemoryESEDI;
				tc.MemorySize = ToMemorySize(parts[1]);
				break;

			case OpKind_MemoryESRDI:
				if (parts.Length != 2)
					throw new InvalidOperationException($"Operand {operand}: expected 2 values, actual = {parts.Length}");
				opKind = OpKind.MemoryESRDI;
				tc.MemorySize = ToMemorySize(parts[1]);
				break;

			case OpKind_Memory64:
				if (parts.Length != 4)
					throw new InvalidOperationException($"Operand {operand}: expected 4 values, actual = {parts.Length}");
				opKind = OpKind.Memory64;
				tc.MemorySegment = ToRegister(parts[1]);
				tc.MemoryAddress64 = ToUInt64(parts[2]);
				tc.MemorySize = ToMemorySize(parts[3]);
				break;

			case OpKind_Memory:
				if (parts.Length != 8)
					throw new InvalidOperationException($"Operand {operand}: expected 8 values, actual = {parts.Length}");
				opKind = OpKind.Memory;
				tc.MemorySegment = ToRegister(parts[1]);
				tc.MemoryBase = ToRegister(parts[2]);
				tc.MemoryIndex = ToRegister(parts[3]);
				tc.MemoryIndexScale = ToInt32(parts[4]);
				tc.MemoryDisplacement = ToUInt32(parts[5]);
				tc.MemoryDisplSize = ToInt32(parts[6]);
				tc.MemorySize = ToMemorySize(parts[7]);
				break;

			default:
				throw new InvalidOperationException($"Invalid opkind: '{parts[0]}'");
			}
		}

		static string ToHexBytes(string value) {
			try {
				HexUtils.ToByteArray(value);
			}
			catch {
				throw new InvalidOperationException($"Invalid hex bytes: '{value}'");
			}
			return value;
		}

		static ulong ToUInt64(string value) {
			if (value.StartsWith("0x")) {
				value = value.Substring(2);
				if (ulong.TryParse(value, System.Globalization.NumberStyles.HexNumber, null, out var number))
					return number;
			}
			else if (ulong.TryParse(value, out var number))
				return number;
			throw new InvalidOperationException($"Invalid number: '{value}'");
		}

		static long ToInt64(string value) {
			if (value.StartsWith("0x")) {
				value = value.Substring(2);
				if (long.TryParse(value, System.Globalization.NumberStyles.HexNumber, null, out var number))
					return number;
			}
			else if (long.TryParse(value, out var number))
				return number;
			throw new InvalidOperationException($"Invalid number: '{value}'");
		}

		static int ToInt32(string value) {
			long v = ToInt64(value);
			if (int.MinValue <= v && v <= int.MaxValue)
				return (int)v;
			throw new InvalidOperationException($"Invalid number: '{value}'");
		}

		static uint ToUInt32(string value) {
			ulong v = ToUInt64(value);
			if (v <= uint.MaxValue)
				return (uint)v;
			throw new InvalidOperationException($"Invalid number: '{value}'");
		}

		static ushort ToUInt16(string value) {
			ulong v = ToUInt64(value);
			if (v <= ushort.MaxValue)
				return (ushort)v;
			throw new InvalidOperationException($"Invalid number: '{value}'");
		}

		static byte ToUInt8(string value) {
			ulong v = ToUInt64(value);
			if (v <= byte.MaxValue)
				return (byte)v;
			throw new InvalidOperationException($"Invalid number: '{value}'");
		}

		static Code ToCode(string value) {
			if (!toCode.TryGetValue(value, out var code))
				throw new InvalidOperationException($"Invalid Code value: '{value}'");
			return code;
		}

		static Register ToRegister(string value) {
			if (value == string.Empty)
				return Register.None;
			if (!toRegister.TryGetValue(value, out var reg))
				throw new InvalidOperationException($"Invalid Register value: '{value}'");
			return reg;
		}

		static MemorySize ToMemorySize(string value) {
			if (!toMemorySize.TryGetValue(value, out var memSize))
				throw new InvalidOperationException($"Invalid MemorySize value: '{value}'");
			return memSize;
		}
	}
}
