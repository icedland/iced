// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using Iced.Intel;

namespace IcedFuzzer.Core {
	sealed class CodeWriter {
		readonly byte[] data;
		int dataLen;

		public int Length => dataLen;
		public byte[] Data => data;
		public CodeWriter() => data = new byte[0x20];

		public void SetLength(int length) {
			Assert.True((uint)length <= (uint)data.Length);
			dataLen = length;
		}

		public void Clear() => dataLen = 0;
		public void WriteByte(byte b) {
			Assert.True(dataLen < data.Length);
			data[dataLen++] = b;
		}
	}

	public readonly struct FuzzerResult {
		public readonly FuzzerInstruction Instruction;
		public readonly byte[] EncodedData;
		public readonly int EncodedDataLength;
		public readonly bool Invalid;

		public FuzzerResult(FuzzerInstruction instruction, byte[] encodedData, int encodedDataLength, bool invalid) {
			Instruction = instruction;
			EncodedData = encodedData;
			EncodedDataLength = encodedDataLength;
			Invalid = invalid;
		}
	}

	public enum FuzzerOptions : uint {
		None					= 0,
		NoVerifyInstrs			= 0x00000001,
		NoPAUSE					= 0x00000002,
		NoWBNOINVD				= 0x00000004,
		NoTZCNT					= 0x00000008,
		NoLZCNT					= 0x00000010,
		HasMPX					= 0x00000020,
		UselessPrefixes			= 0x00000040,
	}

	public sealed class Fuzzer {
		internal CodeWriter Writer => writer;
		internal CpuDecoder CpuDecoder => cpuDecoder;
		internal int Bitness => bitness;
		readonly CodeWriter writer;
		readonly int bitness;
		readonly FuzzerOptions options;
		readonly CpuDecoder cpuDecoder;

		internal bool UselessPrefixes => (options & FuzzerOptions.UselessPrefixes) != 0;

		public Fuzzer(int bitness, FuzzerOptions options, CpuDecoder cpuDecoder) {
			writer = new CodeWriter();
			this.bitness = bitness;
			this.options = options;
			this.cpuDecoder = cpuDecoder;
		}

		sealed class CodeHash : IEqualityComparer<CodeHash.Key> {
			readonly struct Key {
				public readonly int Index;
				public readonly int Length;
				public Key(int index, int length) {
					Index = index;
					Length = length;
				}
			}

			readonly HashSet<Key> hash;
			byte[] data;
			int dataLength;
			public CodeHash() {
				data = new byte[0x100];
				hash = new HashSet<Key>(this);
			}

			public void Clear() {
				hash.Clear();
				dataLength = 0;
			}

			public bool Add(byte[] data, int index, int length) {
				int newDataLength = dataLength + length;
				Assert.True(newDataLength >= length);
				if (newDataLength > this.data.Length)
					Array.Resize(ref this.data, checked(this.data.Length * 2));
				Array.Copy(data, index, this.data, dataLength, length);
				var key = new Key(dataLength, length);
				if (hash.Add(key)) {
					dataLength = newDataLength;
					return true;
				}
				return false;
			}

			bool IEqualityComparer<Key>.Equals(Key x, Key y) {
				if (x.Length != y.Length)
					return false;
				var data = this.data;
				for (int i = 0; i < x.Length; i++) {
					if (data[x.Index + i] != data[y.Index + i])
						return false;
				}
				return true;
			}

			int IEqualityComparer<Key>.GetHashCode(Key obj) {
				var data = this.data;
				int hc = 0;
				for (int i = 0; i < obj.Length; i++)
					hc = (hc << 2) ^ data[obj.Index + i];
				return hc;
			}
		}

		sealed class FuzzerCodeReader : CodeReader {
			public bool CanReadByte => pos < length;

			byte[] data = Array.Empty<byte>();
			int length = 0;
			int pos = 0;

			public void Reset(byte[] data, int length) {
				this.data = data;
				this.length = length;
				pos = 0;
			}

			public override int ReadByte() {
				if (pos >= length)
					return -1;
				return data[pos++];
			}
		}

		public IEnumerable<FuzzerResult> GetInstructions(IEnumerable<FuzzerInstruction> instructions) {
			var reader = new FuzzerCodeReader();
			Decoder? decoder = null;
			foreach (var info in GetInstructionsUniqueBytes(instructions)) {
				if (!info.Instruction.IsValid)
					Assert.True(info.EncodedDataLength >= 15, "If it's invalid, it must add padding bytes");
				Assert.True(info.EncodedDataLength <= 0xFF, "An instruction must be at most 0xFF bytes in length");

				if (decoder is null) {
					var decoderOptions = DecoderOptions.None;
					if ((options & FuzzerOptions.HasMPX) != 0)
						decoderOptions |= DecoderOptions.MPX;
					if ((options & FuzzerOptions.NoPAUSE) != 0)
						decoderOptions |= DecoderOptions.NoPause;
					if ((options & FuzzerOptions.NoWBNOINVD) != 0)
						decoderOptions |= DecoderOptions.NoWbnoinvd;
					if ((options & FuzzerOptions.NoTZCNT) != 0)
						decoderOptions |= DecoderOptions.NoMPFX_0FBC;
					if ((options & FuzzerOptions.NoLZCNT) != 0)
						decoderOptions |= DecoderOptions.NoMPFX_0FBD;
					switch (cpuDecoder) {
					case CpuDecoder.Intel:
						break;
					case CpuDecoder.AMD:
						decoderOptions |= DecoderOptions.AMD;
						break;
					default:
						throw ThrowHelpers.Unreachable;
					}
					decoder = Decoder.Create(bitness, reader, decoderOptions);
				}
				reader.Reset(info.EncodedData, info.EncodedDataLength);
				decoder.IP = 0;
				decoder.Decode(out var instr);
				if (info.Invalid) {
					if ((options & FuzzerOptions.NoVerifyInstrs) == 0) {
						if (instr.Code != Code.INVALID)
							Assert.Fail($"Decoded an invalid instruction! {instr}");
						else if (decoder.LastError == DecoderError.None)
							Assert.Fail("Expected an error");
					}
				}
				else {
					if (instr.Code == Code.INVALID)
						Assert.Fail($"Couldn't decode a valid instruction: {info.Instruction.Code} {info.Instruction.Code.ToOpCode().ToOpCodeString()}");
					else if (decoder.LastError != DecoderError.None)
						Assert.Fail($"Got a decoder error: {decoder.LastError}");
					else if (reader.CanReadByte)
						Assert.Fail($"Didn't decode all bytes: {info.Instruction.Code} = {instr}");
					else if (info.Instruction.Code != instr.Code)
						Assert.Fail($"Decoded the wrong instruction. Expected {info.Instruction.Code} but got {instr.Code}");
				}

				yield return info;
			}
		}

		IEnumerable<FuzzerResult> GetInstructionsUniqueBytes(IEnumerable<FuzzerInstruction> instructions) {
			var codeHash = new CodeHash();
			foreach (var instruction in instructions) {
				codeHash.Clear();
				foreach (var tmp in GetInstructions(instruction)) {
					var isValid = tmp;
					Assert.True(!isValid || instruction.IsValid);
					var data = writer.Data;
					var dataLen = writer.Length;
					if (dataLen > 15)
						isValid = false;
					if (codeHash.Add(data, 0, dataLen))
						yield return new FuzzerResult(instruction, data, dataLen, !isValid);
					writer.Clear();
				}
			}
		}

		static readonly FuzzerGen[] validGens = new FuzzerGen[] {
			new PrefixesFuzzerGen(),
			new NotEnoughBytesLeftFuzzerGen(),
			new InvalidLengthFuzzerGen(),
			new SameRegsFuzzerGen(),
			new AllRegsFuzzerGen(),
			new AllModrmMemFuzzerGen(),
			new AllMemOffsFuzzerGen(),
			new AllImpliedMemFuzzerGen(),
			new AllImmediateMemFuzzerGen(),
			new AllNoOpFuzzerGen(),
			new EvexAaaZBcstErFuzzerGen(),
			new InvalidV2vvvvFuzzerGen(),
			new InvalidReservedEvexBitsFuzzerGen(),
		};
		static readonly FuzzerGen[] invalidGens = new FuzzerGen[] {
			new InvalidFuzzerGen(),
			new GroupInvalidFuzzerGen(),
		};
		static FuzzerGen[] GetFuzzerGens(bool isValid) => isValid ? validGens : invalidGens;

		IEnumerable<bool> GetInstructions(FuzzerInstruction instruction) {
			var gens = GetFuzzerGens(instruction.IsValid);
			var encodings = GetEncodings(instruction.Table.Encoding);
			foreach (var gen in gens) {
				foreach (var encoding in encodings) {
					if (encoding == FuzzerEncodingKind.VEX2) {
						// Check if it can't use VEX2 encoding
						if (instruction.Table.TableIndex != 1 || instruction.W != 0)
							continue;
					}

					var context = new FuzzerGenContext(this, instruction, encoding);
					foreach (var result in gen.Generate(context)) {
						yield return result.IsValid;
						context.UsedRegs.Clear();
					}
				}
			}
		}

		static readonly FuzzerEncodingKind[] legacyEncodings = new[] { FuzzerEncodingKind.Legacy };
		static readonly FuzzerEncodingKind[] vexEncodings = new[] { FuzzerEncodingKind.VEX2, FuzzerEncodingKind.VEX3 };
		static readonly FuzzerEncodingKind[] evexEncodings = new[] { FuzzerEncodingKind.EVEX };
		static readonly FuzzerEncodingKind[] xopEncodings = new[] { FuzzerEncodingKind.XOP };
		static readonly FuzzerEncodingKind[] d3nowEncodings = new[] { FuzzerEncodingKind.D3NOW };
		static FuzzerEncodingKind[] GetEncodings(EncodingKind encoding) =>
			encoding switch {
				EncodingKind.Legacy => legacyEncodings,
				EncodingKind.VEX => vexEncodings,
				EncodingKind.EVEX => evexEncodings,
				EncodingKind.XOP => xopEncodings,
				EncodingKind.D3NOW => d3nowEncodings,
				EncodingKind.MVEX => throw ThrowHelpers.Unreachable,
				_ => throw ThrowHelpers.Unreachable,
			};

		internal void Write(in InstructionInfo info) {
			switch (info.Encoding) {
			case FuzzerEncodingKind.Legacy:
				WriteLegacy(info);
				break;

			case FuzzerEncodingKind.D3NOW:
				WriteD3NOW(info);
				break;

			case FuzzerEncodingKind.VEX2:
			case FuzzerEncodingKind.VEX3:
				WriteVEX(info);
				break;

			case FuzzerEncodingKind.XOP:
				WriteXOP(info);
				break;

			case FuzzerEncodingKind.EVEX:
				WriteEVEX(info);
				break;

			default:
				throw ThrowHelpers.Unreachable;
			}

			if (!info.Instruction.IsValid) {
				while (writer.Length < 15)
					writer.WriteByte(0);
			}
		}

		void WritePrefixes(in InstructionInfo info) {
			bool wrote66 = false;
			bool wrote67 = false;
			bool wroteMP = false;
			foreach (var writePrefix in info.WritePrefixes) {
				switch (writePrefix.Kind) {
				case WritePrefixKind.RawBytes:
					foreach (var b in writePrefix.Prefixes)
						writer.WriteByte(b);
					break;
				case WritePrefixKind.AddressSize:
					Assert.False(wrote67);
					wrote67 = true;
					if (info.addressSizePrefix != 0)
						writer.WriteByte(info.addressSizePrefix);
					break;
				case WritePrefixKind.OperandSize:
					Assert.False(wrote66);
					wrote66 = true;
					if (info.operandSizePrefix != 0) {
						Assert.True(info.Encoding == FuzzerEncodingKind.Legacy || info.Encoding == FuzzerEncodingKind.D3NOW);
						writer.WriteByte(info.operandSizePrefix);
					}
					break;
				case WritePrefixKind.MandatoryPrefix:
					Assert.False(wroteMP);
					wroteMP = true;
					if (info.mandatoryPrefix != 0) {
						Assert.True(info.Encoding == FuzzerEncodingKind.Legacy || info.Encoding == FuzzerEncodingKind.D3NOW);
						Assert.True(info.mandatoryPrefix != info.operandSizePrefix);
						writer.WriteByte(info.mandatoryPrefix);
					}
					break;
				default:
					throw ThrowHelpers.Unreachable;
				}
			}
			Assert.True(info.operandSizePrefix == 0 || wrote66);
			Assert.True(info.addressSizePrefix == 0 || wrote67);
			Assert.True(info.mandatoryPrefix == 0 || wroteMP);

			switch (info.Encoding) {
			case FuzzerEncodingKind.Legacy:
			case FuzzerEncodingKind.D3NOW:
				Assert.True(info.w <= 1 && info.r <= 1 && info.x <= 1 && info.b <= 1);
				uint rex = (info.w << 3) | (info.r << 2) | (info.x << 1) | info.b;
				if (rex != 0 || (info.Flags & EncodedInfoFlags.HasREX) != 0)
					rex |= 0x40;
				if (rex != 0) {
					Assert.True(bitness == 64);
					writer.WriteByte((byte)rex);
				}
				break;

			case FuzzerEncodingKind.VEX2:
			case FuzzerEncodingKind.VEX3:
			case FuzzerEncodingKind.XOP:
			case FuzzerEncodingKind.EVEX:
				Assert.True((info.Flags & EncodedInfoFlags.HasREX) == 0);
				break;

			default:
				throw ThrowHelpers.Unreachable;
			}
		}

		void WriteOpCode(in InstructionInfo info) {
			if (info.OpCode.IsOneByte)
				writer.WriteByte((byte)(info.OpCode.Byte0 | info.opCodeBits));
			else if (info.OpCode.IsTwobyte) {
				writer.WriteByte(info.OpCode.Byte0);
				writer.WriteByte((byte)(info.OpCode.Byte1 | info.opCodeBits));
			}
			else
				throw ThrowHelpers.Unreachable;
		}

		void WriteOperands(in InstructionInfo info) {
			const UsedBits MODRM_FLAGS = UsedBits.modrm_mod | UsedBits.modrm_reg | UsedBits.modrm_rm;
			var modrmFlags = info.UsedBits & MODRM_FLAGS;
			Assert.True(modrmFlags == 0 || modrmFlags == MODRM_FLAGS);
			if ((info.Flags & EncodedInfoFlags.HasModrm) != 0) {
				Assert.True(modrmFlags == MODRM_FLAGS);
				Assert.True(info.modrm <= 0xFF);
				writer.WriteByte((byte)info.modrm);
				if ((info.Flags & EncodedInfoFlags.HasSib) != 0) {
					Assert.True(info.sib <= 0xFF);
					writer.WriteByte((byte)info.sib);
				}
			}
			else {
				Assert.True(modrmFlags == UsedBits.None);
				Assert.True((info.Flags & EncodedInfoFlags.HasSib) == 0);
			}

			Assert.True(info.imm1Size <= 4);
			Assert.False(info.imm1Size > 0 && info.imm0Size == 0);
			WriteImm(info.imm0Size, info.imm0, info.imm0Hi);
			WriteImm(info.imm1Size, info.imm1, 0);
		}

		void WriteImm(int size, uint lo, uint hi) {
			switch (size) {
			case 0:
				break;
			case 1:
				writer.WriteByte((byte)lo);
				break;
			case 2:
				writer.WriteByte((byte)lo);
				writer.WriteByte((byte)(lo >> 8));
				break;
			case 4:
				writer.WriteByte((byte)lo);
				writer.WriteByte((byte)(lo >> 8));
				writer.WriteByte((byte)(lo >> 16));
				writer.WriteByte((byte)(lo >> 24));
				break;
			case 8:
				writer.WriteByte((byte)lo);
				writer.WriteByte((byte)(lo >> 8));
				writer.WriteByte((byte)(lo >> 16));
				writer.WriteByte((byte)(lo >> 24));
				writer.WriteByte((byte)hi);
				writer.WriteByte((byte)(hi >> 8));
				writer.WriteByte((byte)(hi >> 16));
				writer.WriteByte((byte)(hi >> 24));
				break;
			default:
				throw ThrowHelpers.Unreachable;
			}
		}

		void WriteLegacy(in InstructionInfo info) {
			Assert.True(info.Encoding == FuzzerEncodingKind.Legacy);
			Assert.True(info.r2 == 0 && info.vvvv == 0 && info.z == 0 && info.bcst == 0 && info.v2 == 0 && info.aaa == 0);

			WritePrefixes(info);

			switch (info.mmmmm) {
			case OpCodeTableIndexes.LegacyTable_Normal:
				break;
			case OpCodeTableIndexes.LegacyTable_0F:
				writer.WriteByte(0x0F);
				break;
			case OpCodeTableIndexes.LegacyTable_0F38:
				writer.WriteByte(0x0F);
				writer.WriteByte(0x38);
				break;
			case OpCodeTableIndexes.LegacyTable_0F39:
				writer.WriteByte(0x0F);
				writer.WriteByte(0x39);
				break;
			case OpCodeTableIndexes.LegacyTable_0F3A:
				writer.WriteByte(0x0F);
				writer.WriteByte(0x3A);
				break;
			case OpCodeTableIndexes.LegacyTable_0F3B:
				writer.WriteByte(0x0F);
				writer.WriteByte(0x3B);
				break;
			case OpCodeTableIndexes.LegacyTable_0F3C:
				writer.WriteByte(0x0F);
				writer.WriteByte(0x3C);
				break;
			case OpCodeTableIndexes.LegacyTable_0F3D:
				writer.WriteByte(0x0F);
				writer.WriteByte(0x3D);
				break;
			case OpCodeTableIndexes.LegacyTable_0F3E:
				writer.WriteByte(0x0F);
				writer.WriteByte(0x3E);
				break;
			case OpCodeTableIndexes.LegacyTable_0F3F:
				writer.WriteByte(0x0F);
				writer.WriteByte(0x3F);
				break;
			default:
				throw ThrowHelpers.Unreachable;
			}
			WriteOpCode(info);
			WriteOperands(info);
		}

		void WriteD3NOW(in InstructionInfo info) {
			Assert.True(info.Encoding == FuzzerEncodingKind.D3NOW);
			Assert.True(info.mmmmm == OpCodeTableIndexes.D3nowTable);
			Assert.True(info.r2 == 0 && info.vvvv == 0 && info.z == 0 && info.bcst == 0 && info.v2 == 0 && info.aaa == 0);
			WritePrefixes(info);
			writer.WriteByte(0x0F);
			writer.WriteByte(0x0F);
			WriteOperands(info);
			WriteOpCode(info);
		}

		void WriteVEX(in InstructionInfo info) {
			Assert.True(info.Encoding == FuzzerEncodingKind.VEX2 || info.Encoding == FuzzerEncodingKind.VEX3);

			WritePrefixes(info);

			Assert.True(info.r <= 1 && info.x <= 1 && info.b <= 1 && info.w <= 1 && info.l <= 1 && info.vvvv <= 0xF && info.mmmmm <= 0x1F);
			Assert.True(info.r2 == 0 && info.z == 0 && info.bcst == 0 && info.v2 == 0 && info.aaa == 0);

			uint r = info.r;
			uint x = info.x;
			uint b = info.b;
			uint vvvv = info.vvvv;
			switch (info.Encoding) {
			case FuzzerEncodingKind.VEX2:
				Assert.True(x == 0 && b == 0 && info.mmmmm == 1 && info.w == 0);
				Assert.True(bitness == 64 || (r == 0 && vvvv <= 7));
				r ^= 1;
				vvvv ^= 0xF;
				writer.WriteByte(0xC5);
				writer.WriteByte((byte)((r << 7) | (vvvv << 3) | (info.l << 2) | info.pp));
				break;

			case FuzzerEncodingKind.VEX3:
				Assert.True(bitness == 64 || (r == 0 && x == 0));
				r ^= 1;
				x ^= 1;
				b ^= 1;
				vvvv ^= 0xF;
				writer.WriteByte(0xC4);
				writer.WriteByte((byte)((r << 7) | (x << 6) | (b << 5) | info.mmmmm));
				writer.WriteByte((byte)((info.w << 7) | (vvvv << 3) | (info.l << 2) | info.pp));
				break;

			default:
				throw ThrowHelpers.Unreachable;
			}
			WriteOpCode(info);
			WriteOperands(info);
		}

		void WriteXOP(in InstructionInfo info) {
			Assert.True(info.Encoding == FuzzerEncodingKind.XOP);

			WritePrefixes(info);

			Assert.True(info.r <= 1 && info.x <= 1 && info.b <= 1 && info.w <= 1 && info.l <= 1 && info.vvvv <= 0xF && info.mmmmm <= 0x1F);
			Assert.True(info.mmmmm >= 8 || info.b == 0);
			Assert.True(info.r2 == 0 && info.z == 0 && info.bcst == 0 && info.v2 == 0 && info.aaa == 0);

			uint r = info.r;
			uint x = info.x;
			uint b = info.b;
			uint vvvv = info.vvvv;
			r ^= 1;
			x ^= 1;
			b ^= 1;
			vvvv ^= 0xF;
			writer.WriteByte(0x8F);
			writer.WriteByte((byte)((r << 7) | (x << 6) | (b << 5) | info.mmmmm));
			writer.WriteByte((byte)((info.w << 7) | (vvvv << 3) | (info.l << 2) | info.pp));
			WriteOpCode(info);
			WriteOperands(info);
		}

		void WriteEVEX(in InstructionInfo info) {
			Assert.True(info.Encoding == FuzzerEncodingKind.EVEX);

			WritePrefixes(info);

			Assert.True(info.r <= 1 && info.x <= 1 && info.b <= 1 && info.r2 <= 1 && info.EVEX_res3 <= 1 && info.mmmmm <= 7);
			Assert.True(info.w <= 1 && info.vvvv <= 0xF && info.EVEX_res10 <= 1);
			Assert.True(info.z <= 1 && info.l <= 3 && info.bcst <= 1 && info.v2 <= 1 && info.aaa <= 7);

			uint r = info.r;
			uint x = info.x;
			uint b = info.b;
			uint r2 = info.r2;
			uint vvvv = info.vvvv;
			uint v2 = info.v2;
			r ^= 1;
			x ^= 1;
			b ^= 1;
			r2 ^= 1;
			vvvv ^= 0xF;
			v2 ^= 1;
			writer.WriteByte(0x62);
			writer.WriteByte((byte)((r << 7) | (x << 6) | (b << 5) | (r2 << 4) | (info.EVEX_res3 << 3) | info.mmmmm));
			writer.WriteByte((byte)((info.w << 7) | (vvvv << 3) | (info.EVEX_res10 << 2) | info.pp));
			writer.WriteByte((byte)((info.z << 7) | (info.l << 5) | (info.bcst << 4) | (v2 << 3) | info.aaa));
			WriteOpCode(info);
			WriteOperands(info);
		}
	}
}
