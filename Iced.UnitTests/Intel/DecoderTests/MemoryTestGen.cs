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
using System.Text;
using Iced.Intel;

namespace Iced.UnitTests.Intel.DecoderTests.MemoryTestGenImpl {
	readonly struct MemInfo : IEquatable<MemInfo> {
		public readonly string HexBytes;
		public readonly int ByteLength;
		public readonly string Code;
		public readonly Register Register;
		public readonly Register PrefixSeg;
		public readonly Register Segment;
		public readonly Register BaseReg;
		public readonly Register IndexReg;
		public readonly int Scale;
		public readonly int DisplSize;
		public readonly uint Displ;
		public readonly bool IsInvalid;

		public MemInfo(string hexBytes, int byteLength, string code, Register register, Register prefixSeg, Register baseReg, Register indexReg, int scale, int displSize, uint displ, bool isInvalid) {
			HexBytes = hexBytes;
			ByteLength = byteLength;
			Code = code;
			Register = register;
			PrefixSeg = prefixSeg;
			BaseReg = baseReg;
			IndexReg = indexReg;
			Scale = scale;
			DisplSize = displSize;
			Displ = displ;
			IsInvalid = isInvalid;
			if (prefixSeg != Register.None)
				Segment = prefixSeg;
			else if (baseReg == Register.BP || baseReg == Register.ESP || baseReg == Register.RSP || baseReg == Register.EBP || baseReg == Register.RBP)
				Segment = Register.SS;
			else
				Segment = Register.DS;
		}

		public bool Equals(MemInfo other) {
			if (HexBytes != other.HexBytes)
				return false;

			if (ByteLength != other.ByteLength ||
				Code != other.Code ||
				Register != other.Register ||
				PrefixSeg != other.PrefixSeg ||
				Segment != other.Segment ||
				BaseReg != other.BaseReg ||
				IndexReg != other.IndexReg ||
				Scale != other.Scale ||
				DisplSize != other.DisplSize ||
				Displ != other.Displ ||
				IsInvalid != other.IsInvalid)
				throw new InvalidOperationException();

			return true;
		}

		public override bool Equals(object obj) => obj is MemInfo other && Equals(other);

		public override int GetHashCode() => HexBytes.GetHashCode();
	}

	class Program {
		static void Main2(string[] args) => new Program().DoIt();

		const string Legacy_Code_16 = nameof(Code.Add_rm16_r16);
		const string Legacy_Code_32 = nameof(Code.Add_rm32_r32);
		const string Legacy_Code_64 = nameof(Code.Add_rm32_r32);
		const Register Legacy_Register_16 = Register.AX;
		const Register Legacy_Register_32 = Register.EAX;
		const Register Legacy_Register_64 = Register.EAX;
		const byte Legacy_OpCode = 0x01;

		const string EVEX_Code = nameof(Code.EVEX_Vpscatterdd_vm32x_k1_xmm);
		const int EVEX_LL = 0;
		const int EVEX_pp = 1;
		const int EVEX_mm = 2;
		const int EVEX_W = 0;
		const int EVEX_aaa = 1;
		const byte EVEX_OpCode = 0xA0;
		const int EVEX_Displ8N = 4;
		const Register EVEX_Register = Register.XMM0;

		readonly StringBuilder sb = new StringBuilder();
		int byteLength;

		string GetCodeValue(int codeSize, bool isVsib) {
			if (isVsib)
				return EVEX_Code;
			switch (codeSize) {
			case 16: return Legacy_Code_16;
			case 32: return Legacy_Code_32;
			case 64: return Legacy_Code_64;
			default: throw new InvalidOperationException();
			}
		}

		string GetHexBytes(out int byteLength) {
			byteLength = this.byteLength;
			this.byteLength = 0;
			var res = sb.ToString();
			sb.Clear();
			return res;
		}

		void AddSpace() => sb.Append(" ");

		void AddByteSpace(byte value) {
			AddByte(value);
			AddSpace();
		}

		void AddByte(byte value) {
			sb.Append(value.ToString("X2"));
			byteLength++;
		}

		void AddUInt16(ushort value) {
			AddByte((byte)value);
			AddByte((byte)(value >> 8));
		}

		void AddUInt32(uint value) {
			AddByte((byte)value);
			AddByte((byte)(value >> 8));
			AddByte((byte)(value >> 16));
			AddByte((byte)(value >> 24));
		}

		void EvexEncode(bool addrSizePrefix, byte modRM, int sib, int displSize, uint displ, int regNum, int baseRegNum, int indexRegNum) {
			if (addrSizePrefix)
				AddByteSpace(0x67);

			AddByteSpace(0x62);

			byte p0 = EVEX_mm;
			byte p1 = 4 | EVEX_pp | (EVEX_W << 7);
			byte p2 = EVEX_aaa | (EVEX_LL << 5);

			if (baseRegNum >= 0) {
				if ((uint)baseRegNum > 31)
					throw new InvalidOperationException();
				if ((baseRegNum & 8) != 0)
					p0 |= 0x20;
				if ((baseRegNum & 0x10) != 0)
					p0 |= 0x10;
			}

			if (indexRegNum >= 0) {
				if ((uint)indexRegNum > 31)
					throw new InvalidOperationException();
				if ((indexRegNum & 8) != 0)
					p0 |= 0x40;
				if ((indexRegNum & 0x10) != 0)
					p2 |= 8;
			}

			if ((uint)regNum > 15)
				throw new InvalidOperationException();
			if ((regNum & 8) != 0)
				p0 |= 0x80;

			p0 ^= 0xF0;
			p1 ^= 0x78;
			p2 ^= 8;
			AddByte(p0);
			AddByte(p1);
			AddByteSpace(p2);

			AddByteSpace(EVEX_OpCode);
			AddByte(modRM);
			if (sib >= 0) {
				AddSpace();
				AddByte((byte)sib);
			}
			EncodeDispl(displSize, displ);
		}

		void LegacyEncode(bool addrSizePrefix, int rex, byte modRM, int sib, int displSize, uint displ) {
			if (addrSizePrefix)
				AddByteSpace(0x67);
			if (rex != 0) {
				if (!(0x40 <= rex && rex <= 0x4F))
					throw new InvalidOperationException();
				AddByteSpace((byte)rex);
			}
			AddByteSpace(Legacy_OpCode);
			AddByte(modRM);
			if (sib >= 0) {
				AddSpace();
				AddByte((byte)sib);
			}
			EncodeDispl(displSize, displ);
		}

		void EncodeDispl(int displSize, uint displ) {
			switch (displSize) {
			case 0:
				break;

			case 1:
				AddSpace();
				AddByte((byte)displ);
				break;

			case 2:
				AddSpace();
				AddUInt16((ushort)displ);
				break;

			case 4:
			case 8:
				AddSpace();
				AddUInt32(displ);
				break;

			default:
				throw new InvalidOperationException();
			}
		}

		Register GetLegacyRegister(int codeSize) {
			switch (codeSize) {
			case 16: return Legacy_Register_16;
			case 32: return Legacy_Register_32;
			case 64: return Legacy_Register_64;
			default: throw new InvalidOperationException();
			}
		}

		uint GetDispl(int addrSize, int displSize, byte rand) {
			bool b = ((rand & 8) != 0);
			switch (displSize) {
			case 0:
				return 0;
			case 1:
				if (b)
					return 0x5A;
				else {
					if (addrSize == 16)
						return 0xFFA5;
					return 0xFFFFFFA5;
				}
			case 2:
				return b ? 0x5AA5U : 0xA55A;
			case 4:
			case 8:
				return b ? 0x5AA56789 : 0xA55A1234;
			default:
				throw new InvalidOperationException();
			}
		}

		void DoIt() {
			(int codeSize, int addrSize, bool isVsib)[] memInfos = new(int codeSize, int addrSize, bool isVsib)[] {
				(16, 16, false),
				(16, 32, false),
				(16, 32, true),

				(32, 16, false),
				(32, 32, false),
				(32, 32, true),

				(64, 32, false),
				(64, 64, false),
				(64, 32, true),
				(64, 64, true),
			};

			foreach (var memInfo in memInfos) {
				Console.WriteLine($"CodeSize: {memInfo.codeSize}, AddrSize: {memInfo.addrSize}, VSIB={memInfo.isVsib}");
				var filename = $@"C:\memtestgen\out_cs{memInfo.codeSize}_as{memInfo.addrSize}";
				if (memInfo.isVsib)
					filename += "-vsib";
				filename += ".bin";
				Console.WriteLine($"Filename: {filename}");
				if (File.Exists(filename))
					File.Delete(filename);
				using (var file = File.OpenWrite(filename)) {
					foreach (var info in GetMemInfo(memInfo.codeSize, memInfo.addrSize, memInfo.isVsib)) {
						string displString;
						switch (info.DisplSize) {
						case 0:
							displString = "0";
							break;
						case 1:
							if (memInfo.addrSize == 16)
								displString = $"0x{info.Displ:X4}";
							else
								displString = $"0x{info.Displ:X8}";
							break;
						case 2:
							displString = $"0x{info.Displ:X4}";
							break;
						case 4:
						case 8:
							displString = $"0x{info.Displ:X8}";
							break;
						default:
							throw new InvalidOperationException();
						}
						Console.WriteLine($"{info.HexBytes}, {info.ByteLength}, {info.Code}, {info.Register}, {info.PrefixSeg}, {info.Segment}, {info.BaseReg}, {info.IndexReg}, {info.Scale}, {displString}, {info.DisplSize}");
						var data = HexUtils.ToByteArray(info.HexBytes);
						file.Write(data, 0, data.Length);
					}
				}
				Console.WriteLine();
			}
		}

		IEnumerable<MemInfo> GetMemInfo(int codeSize, int addrSize, bool isVsib) {
			var hash = new HashSet<MemInfo>();
			if (addrSize == 16) {
				foreach (var info in GetMemInfo16(codeSize, addrSize, isVsib)) {
					if (!hash.Add(info))
						continue;
					yield return info;
				}
			}
			else {
				foreach (var info in GetMemInfo3264(codeSize, addrSize, isVsib)) {
					if (!hash.Add(info))
						continue;
					yield return info;
				}
			}
		}

		IEnumerable<MemInfo> GetMemInfo16(int codeSize, int addrSize, bool isVsib) {
			if (isVsib)
				throw new InvalidOperationException();
			for (uint i = 0; i < 0x100; i++) {
				byte modRM = (byte)i;

				var mod = (modRM >> 6) & 3;
				if (mod == 3)
					continue;
				int reg = (int)((modRM >> 3) & 7);
				int rm = (int)(modRM & 7);

				var prefixSeg = Register.None;
				int scale = 0;

				var register = GetLegacyRegister(codeSize) + reg;
				Register baseReg, indexReg;
				switch (rm) {
				case 0:
					baseReg = Register.BX;
					indexReg = Register.SI;
					break;
				case 1:
					baseReg = Register.BX;
					indexReg = Register.DI;
					break;
				case 2:
					baseReg = Register.BP;
					indexReg = Register.SI;
					break;
				case 3:
					baseReg = Register.BP;
					indexReg = Register.DI;
					break;
				case 4:
					baseReg = Register.SI;
					indexReg = Register.None;
					break;
				case 5:
					baseReg = Register.DI;
					indexReg = Register.None;
					break;
				case 6:
					baseReg = Register.BP;
					indexReg = Register.None;
					break;
				case 7:
					baseReg = Register.BX;
					indexReg = Register.None;
					break;
				default:
					throw new InvalidOperationException();
				}

				int displSize;
				switch (mod) {
				case 0:
					if (rm == 6) {
						displSize = 2;
						baseReg = Register.None;
						indexReg = Register.None;
					}
					else
						displSize = 0;
					break;
				case 1:
					displSize = 1;
					break;
				case 2:
					displSize = 2;
					break;
				default:
					throw new InvalidOperationException();
				}

				uint displ = GetDispl(addrSize, displSize, modRM);
				LegacyEncode(codeSize != addrSize, 0, modRM, -1, displSize, displ);
				var hexBytes = GetHexBytes(out int byteLength);
				yield return new MemInfo(hexBytes, byteLength, GetCodeValue(codeSize, isVsib), register, prefixSeg, baseReg, indexReg, scale, displSize, displ, isInvalid: false);
			}
		}

		IEnumerable<MemInfo> GetMemInfo3264(int codeSize, int addrSize, bool isVsib) {
			if (isVsib) {
				foreach (var info in GetMemInfo3264(codeSize, addrSize, isVsib, onlySib: true))
					yield return info;
			}
			else {
				foreach (var info in GetMemInfo3264(codeSize, addrSize, isVsib, onlySib: false))
					yield return info;
				foreach (var info in GetMemInfo3264(codeSize, addrSize, isVsib, onlySib: true))
					yield return info;
			}
		}

		IEnumerable<MemInfo> GetMemInfo3264(int codeSize, int addrSize, bool isVsib, bool onlySib) {
			Register defaultBaseReg;
			if (addrSize == 32)
				defaultBaseReg = Register.EAX;
			else
				defaultBaseReg = Register.RAX;
			var defaultIndexReg = isVsib ? Register.XMM0 : defaultBaseReg;

			uint max;
			// [7:0] = modRM
			// [8] = 'register' register bit 3 (bit 4 isn't used if it's XMM)
			// [9] = 'baseReg' register bit 3
			// [11:10] = 'indexReg' register bits [4:3]
			if (codeSize == 64) {
				if (isVsib)
					max = 0x100 * 16;
				else
					max = 0x100 * 8;
			}
			else
				max = 0x100;
			for (uint i = 0; i < max; i++) {
				byte modRM = (byte)i;

				var mod = (modRM >> 6) & 3;
				if (mod == 3)
					continue;
				int reg = (int)((modRM >> 3) & 7);
				int rm = (int)(modRM & 7);

				bool hasSib = rm == 4;
				if (hasSib) {
					if (!onlySib)
						continue;
				}
				else {
					if (onlySib)
						continue;
				}

				uint extraRegister = ((i >> 8) & 1) << 3;
				uint extraBaseReg = ((i >> 9) & 1) << 3;
				uint extraIndexReg = ((i >> 10) & 3) << 3;

				int regNum = (int)extraRegister + reg;
				var register = isVsib ? regNum + EVEX_Register : GetLegacyRegister(codeSize) + regNum;
				var baseReg = defaultBaseReg + (int)extraBaseReg + rm;

				// Only test the memory operand
				if (regNum != 6)
					continue;

				int displSize;
				switch (mod) {
				case 0:
					displSize = 0;
					if (rm == 5) {
						displSize = addrSize == 64 ? 8 : 4;
						if (codeSize == 64)
							baseReg = addrSize == 32 ? Register.EIP : Register.RIP;
						else
							baseReg = Register.None;
					}
					break;
				case 1:
					displSize = 1;
					break;
				case 2:
					displSize = addrSize == 64 ? 8 : 4;
					break;
				default:
					throw new InvalidOperationException();
				}

				if (!hasSib)
					yield return GetMemInfo3264(codeSize, addrSize, isVsib, modRM, -1, defaultBaseReg, defaultIndexReg, register, baseReg, extraBaseReg, extraIndexReg, displSize);
				else {
					for (int sib = 0; sib < 0x100; sib++)
						yield return GetMemInfo3264(codeSize, addrSize, isVsib, modRM, sib, defaultBaseReg, defaultIndexReg, register, baseReg, extraBaseReg, extraIndexReg, displSize);
				}
			}
		}

		MemInfo GetMemInfo3264(int codeSize, int addrSize, bool isVsib, byte modRM, int sib, Register defaultBaseReg, Register defaultIndexReg, Register register, Register baseReg, uint extraBaseReg, uint extraIndexReg, int displSize) {
			var mod = (modRM >> 6) & 3;
			Register indexReg;
			int scale;
			if (sib >= 0) {
				scale = (sib >> 6) & 3;
				int baseNum = (int)extraBaseReg + (sib & 7);
				baseReg = defaultBaseReg + baseNum;
				int indexNum = (int)extraIndexReg + ((sib >> 3) & 7);
				indexReg = defaultIndexReg + indexNum;
				if (!isVsib && indexNum == 4)
					indexReg = Register.None;
				if (mod == 0 && (baseNum & 7) == 5) {
					baseReg = Register.None;
					displSize = addrSize == 64 ? 8 : 4;
				}
			}
			else {
				scale = 0;
				indexReg = Register.None;
			}

			bool isInvalid = false;

			int baseRegNum = baseReg == Register.None || baseReg == Register.RIP || baseReg == Register.EIP ? -1 : baseReg - defaultBaseReg;
			int indexRegNum = indexReg == Register.None ? -1 : indexReg - defaultIndexReg;
			var regBase = isVsib ? EVEX_Register : GetLegacyRegister(codeSize);
			int regNum = register - regBase;

			var prefixSeg = Register.None;
			uint displ = GetDispl(addrSize, displSize, sib >= 0 ? (byte)sib : modRM);
			if (isVsib) {
				EvexEncode(codeSize != addrSize, modRM, sib, displSize, displ, regNum, baseRegNum, indexRegNum);
				if (displSize == 1)
					displ = (uint)((int)(sbyte)displ * EVEX_Displ8N);
			}
			else {
				int rex = 0;

				if (baseRegNum >= 0) {
					if ((uint)baseRegNum > 15)
						throw new InvalidOperationException();
					if (baseRegNum >= 8)
						rex |= 1;
				}

				if (indexRegNum >= 0) {
					if ((uint)indexRegNum > 15)
						throw new InvalidOperationException();
					if (indexRegNum >= 8)
						rex |= 2;
				}

				if ((uint)regNum > 15)
					throw new InvalidOperationException();
				if (regNum >= 8)
					rex |= 4;

				if (rex != 0)
					rex |= 0x40;
				LegacyEncode(codeSize != addrSize, rex, modRM, sib, displSize, displ);
			}
			var hexBytes = GetHexBytes(out int byteLength);
			return new MemInfo(hexBytes, byteLength, GetCodeValue(codeSize, isVsib), register, prefixSeg, baseReg, indexReg, scale, displSize, displ, isInvalid);
		}
	}
}
