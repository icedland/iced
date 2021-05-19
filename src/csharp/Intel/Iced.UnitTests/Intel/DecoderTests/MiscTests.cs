// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Linq;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.DecoderTests {
	public sealed class MiscTests : DecoderTest {
		sealed class DecodeMultipleCodeReader : CodeReader {
			byte[] data;
			int offset;

			public void SetArray(byte[] data) {
				this.data = data;
				offset = 0;
			}

			public override int ReadByte() {
				if (offset >= data.Length)
					return -1;
				return data[offset++];
			}
		}

		[Fact]
		void Decode_multiple_instrs_with_one_instance() {
			var reader16 = new DecodeMultipleCodeReader();
			var reader32 = new DecodeMultipleCodeReader();
			var reader64 = new DecodeMultipleCodeReader();
			var decoderDict16 = new Dictionary<DecoderOptions, Decoder>();
			var decoderDict32 = new Dictionary<DecoderOptions, Decoder>();
			var decoderDict64 = new Dictionary<DecoderOptions, Decoder>();
			foreach (var info in DecoderTestUtils.GetDecoderTests(includeOtherTests: true, includeInvalid: true)) {
				var data = HexUtils.ToByteArray(info.HexBytes);
				var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(data), info.Options);
				decoder.IP = info.IP;
				Decoder decoderAll;
				switch (info.Bitness) {
				case 16:
					reader16.SetArray(data);
					if (!decoderDict16.TryGetValue(info.Options, out decoderAll))
						decoderDict16.Add(info.Options, decoderAll = Decoder.Create(info.Bitness, reader16, info.Options));
					break;
				case 32:
					reader32.SetArray(data);
					if (!decoderDict32.TryGetValue(info.Options, out decoderAll))
						decoderDict32.Add(info.Options, decoderAll = Decoder.Create(info.Bitness, reader32, info.Options));
					break;
				case 64:
					reader64.SetArray(data);
					if (!decoderDict64.TryGetValue(info.Options, out decoderAll))
						decoderDict64.Add(info.Options, decoderAll = Decoder.Create(info.Bitness, reader64, info.Options));
					break;
				default:
					throw new InvalidOperationException();
				}
				decoderAll.IP = decoder.IP;
				var instruction1 = decoder.Decode();
				var instruction2 = decoderAll.Decode();
				var co1 = decoder.GetConstantOffsets(instruction1);
				var co2 = decoderAll.GetConstantOffsets(instruction2);
				Assert.Equal(info.Code, instruction1.Code);
				Assert.True(Instruction.EqualsAllBits(instruction1, instruction2));
				VerifyConstantOffsets(co1, co2);
			}
		}

		[Theory]
		[MemberData(nameof(Test_ByteArrayCodeReader_ctor_Data))]
		void Test_ByteArrayCodeReader_ctor(ByteArrayCodeReader reader, byte[] expectedData) {
			int i = 0;
			Assert.Equal(0, reader.Position);
			while (reader.CanReadByte) {
				Assert.Equal(i, reader.Position);
				Assert.True(i < expectedData.Length);
				Assert.Equal(expectedData[i], reader.ReadByte());
				i++;
			}
			Assert.Equal(i, reader.Position);
			Assert.Equal(expectedData.Length, i);
			Assert.Equal(-1, reader.ReadByte());
			Assert.Equal(i, reader.Position);

			reader.Position = 0;
			Assert.Equal(0, reader.Position);
			i = 0;
			while (reader.CanReadByte) {
				Assert.Equal(i, reader.Position);
				Assert.True(i < expectedData.Length);
				Assert.Equal(expectedData[i], reader.ReadByte());
				i++;
			}
			Assert.Equal(i, reader.Position);
			Assert.Equal(expectedData.Length, i);
			Assert.Equal(-1, reader.ReadByte());
			Assert.Equal(i, reader.Position);

			reader.Position = reader.Count;
			Assert.Equal(reader.Count, reader.Position);
			Assert.False(reader.CanReadByte);
			Assert.Equal(-1, reader.ReadByte());

			for (i = expectedData.Length - 1; i >= 0; i--) {
				reader.Position = i;
				Assert.Equal(i, reader.Position);
				Assert.True(reader.CanReadByte);
				Assert.Equal(expectedData[i], reader.ReadByte());
				Assert.Equal(i + 1, reader.Position);
			}

			Assert.Throws<ArgumentOutOfRangeException>(() => reader.Position = int.MinValue);
			Assert.Throws<ArgumentOutOfRangeException>(() => reader.Position = -1);
			Assert.Throws<ArgumentOutOfRangeException>(() => reader.Position = expectedData.Length + 1);
			Assert.Throws<ArgumentOutOfRangeException>(() => reader.Position = int.MaxValue);
		}
		public static IEnumerable<object[]> Test_ByteArrayCodeReader_ctor_Data {
			get {
				yield return new object[] { new ByteArrayCodeReader(""), new byte[] { } };
				yield return new object[] { new ByteArrayCodeReader("12"), new byte[] { 0x12 } };
				yield return new object[] { new ByteArrayCodeReader("1234"), new byte[] { 0x12, 0x34 } };

				yield return new object[] { new ByteArrayCodeReader(new byte[] { }), new byte[] { } };
				yield return new object[] { new ByteArrayCodeReader(new byte[] { 0x23 }), new byte[] { 0x23 } };
				yield return new object[] { new ByteArrayCodeReader(new byte[] { 0x23, 0x45 }), new byte[] { 0x23, 0x45 } };

				yield return new object[] { new ByteArrayCodeReader(new byte[] { }, 0, 0), new byte[] { } };
				yield return new object[] { new ByteArrayCodeReader(new byte[] { 0x45 }, 0, 1), new byte[] { 0x45 } };
				yield return new object[] { new ByteArrayCodeReader(new byte[] { 0x45, 0x67 }, 0, 2), new byte[] { 0x45, 0x67 } };
				yield return new object[] { new ByteArrayCodeReader(new byte[] { 0x45, 0x67, 0x89 }, 0, 3), new byte[] { 0x45, 0x67, 0x89 } };
				yield return new object[] { new ByteArrayCodeReader(new byte[] { 0x45, 0x67, 0x89, 0xAB }, 0, 4), new byte[] { 0x45, 0x67, 0x89, 0xAB } };
				yield return new object[] { new ByteArrayCodeReader(new byte[] { 0x45, 0x67, 0x89, 0xAB }, 1, 3), new byte[] { 0x67, 0x89, 0xAB } };
				yield return new object[] { new ByteArrayCodeReader(new byte[] { 0x45, 0x67, 0x89, 0xAB }, 2, 2), new byte[] { 0x89, 0xAB } };
				yield return new object[] { new ByteArrayCodeReader(new byte[] { 0x45, 0x67, 0x89, 0xAB }, 3, 1), new byte[] { 0xAB } };
				yield return new object[] { new ByteArrayCodeReader(new byte[] { 0x45, 0x67, 0x89, 0xAB }, 4, 0), new byte[] { } };
				yield return new object[] { new ByteArrayCodeReader(new byte[] { 0x45, 0x67, 0x89, 0xAB }, 0, 3), new byte[] { 0x45, 0x67, 0x89 } };
				yield return new object[] { new ByteArrayCodeReader(new byte[] { 0x45, 0x67, 0x89, 0xAB }, 0, 2), new byte[] { 0x45, 0x67 } };
				yield return new object[] { new ByteArrayCodeReader(new byte[] { 0x45, 0x67, 0x89, 0xAB }, 0, 1), new byte[] { 0x45 } };
				yield return new object[] { new ByteArrayCodeReader(new byte[] { 0x45, 0x67, 0x89, 0xAB }, 0, 0), new byte[] { } };
				yield return new object[] { new ByteArrayCodeReader(new byte[] { 0x45, 0x67, 0x89, 0xAB }, 1, 2), new byte[] { 0x67, 0x89 } };
				yield return new object[] { new ByteArrayCodeReader(new byte[] { 0x45, 0x67, 0x89, 0xAB }, 1, 1), new byte[] { 0x67 } };
				yield return new object[] { new ByteArrayCodeReader(new byte[] { 0x45, 0x67, 0x89, 0xAB }, 2, 1), new byte[] { 0x89 } };

				yield return new object[] { new ByteArrayCodeReader(new ArraySegment<byte>(new byte[] { }, 0, 0)), new byte[] { } };
				yield return new object[] { new ByteArrayCodeReader(new ArraySegment<byte>(new byte[] { 0x45 }, 0, 1)), new byte[] { 0x45 } };
				yield return new object[] { new ByteArrayCodeReader(new ArraySegment<byte>(new byte[] { 0x45, 0x67 }, 0, 2)), new byte[] { 0x45, 0x67 } };
				yield return new object[] { new ByteArrayCodeReader(new ArraySegment<byte>(new byte[] { 0x45, 0x67, 0x89 }, 0, 3)), new byte[] { 0x45, 0x67, 0x89 } };
				yield return new object[] { new ByteArrayCodeReader(new ArraySegment<byte>(new byte[] { 0x45, 0x67, 0x89, 0xAB }, 0, 4)), new byte[] { 0x45, 0x67, 0x89, 0xAB } };
				yield return new object[] { new ByteArrayCodeReader(new ArraySegment<byte>(new byte[] { 0x45, 0x67, 0x89, 0xAB }, 1, 3)), new byte[] { 0x67, 0x89, 0xAB } };
				yield return new object[] { new ByteArrayCodeReader(new ArraySegment<byte>(new byte[] { 0x45, 0x67, 0x89, 0xAB }, 2, 2)), new byte[] { 0x89, 0xAB } };
				yield return new object[] { new ByteArrayCodeReader(new ArraySegment<byte>(new byte[] { 0x45, 0x67, 0x89, 0xAB }, 3, 1)), new byte[] { 0xAB } };
				yield return new object[] { new ByteArrayCodeReader(new ArraySegment<byte>(new byte[] { 0x45, 0x67, 0x89, 0xAB }, 4, 0)), new byte[] { } };
				yield return new object[] { new ByteArrayCodeReader(new ArraySegment<byte>(new byte[] { 0x45, 0x67, 0x89, 0xAB }, 0, 3)), new byte[] { 0x45, 0x67, 0x89 } };
				yield return new object[] { new ByteArrayCodeReader(new ArraySegment<byte>(new byte[] { 0x45, 0x67, 0x89, 0xAB }, 0, 2)), new byte[] { 0x45, 0x67 } };
				yield return new object[] { new ByteArrayCodeReader(new ArraySegment<byte>(new byte[] { 0x45, 0x67, 0x89, 0xAB }, 0, 1)), new byte[] { 0x45 } };
				yield return new object[] { new ByteArrayCodeReader(new ArraySegment<byte>(new byte[] { 0x45, 0x67, 0x89, 0xAB }, 0, 0)), new byte[] { } };
				yield return new object[] { new ByteArrayCodeReader(new ArraySegment<byte>(new byte[] { 0x45, 0x67, 0x89, 0xAB }, 1, 2)), new byte[] { 0x67, 0x89 } };
				yield return new object[] { new ByteArrayCodeReader(new ArraySegment<byte>(new byte[] { 0x45, 0x67, 0x89, 0xAB }, 1, 1)), new byte[] { 0x67 } };
				yield return new object[] { new ByteArrayCodeReader(new ArraySegment<byte>(new byte[] { 0x45, 0x67, 0x89, 0xAB }, 2, 1)), new byte[] { 0x89 } };
			}
		}

		[Fact]
		void Test_ByteArrayCodeReader_ctor_throws() {
			Assert.Throws<ArgumentNullException>(() => new ByteArrayCodeReader((string)null));
			Assert.Throws<ArgumentNullException>(() => new ByteArrayCodeReader((byte[])null));
			Assert.Throws<ArgumentNullException>(() => new ByteArrayCodeReader((byte[])null, 0, 0));
			Assert.Throws<ArgumentException>(() => new ByteArrayCodeReader(default(ArraySegment<byte>)));
			Assert.Throws<ArgumentOutOfRangeException>(() => new ByteArrayCodeReader(new byte[] { 1, 2, 3, 4 }, -1, 0));
			Assert.Throws<ArgumentOutOfRangeException>(() => new ByteArrayCodeReader(new byte[] { 1, 2, 3, 4 }, 0, -1));
			Assert.Throws<ArgumentOutOfRangeException>(() => new ByteArrayCodeReader(new byte[] { 1, 2, 3, 4 }, int.MinValue, 0));
			Assert.Throws<ArgumentOutOfRangeException>(() => new ByteArrayCodeReader(new byte[] { 1, 2, 3, 4 }, 0, 5));
			Assert.Throws<ArgumentOutOfRangeException>(() => new ByteArrayCodeReader(new byte[] { 1, 2, 3, 4 }, 0, int.MaxValue));
			Assert.Throws<ArgumentOutOfRangeException>(() => new ByteArrayCodeReader(new byte[] { 1, 2, 3, 4 }, int.MinValue, int.MaxValue));
			Assert.Throws<ArgumentOutOfRangeException>(() => new ByteArrayCodeReader(new byte[] { 1, 2, 3, 4 }, 4, 1));
			Assert.Throws<ArgumentOutOfRangeException>(() => new ByteArrayCodeReader(new byte[] { 1, 2, 3, 4 }, 4, int.MaxValue));
		}

		[Fact]
		void Test_Decoder_Create_throws() {
			foreach (var bitness in BitnessUtils.GetInvalidBitnessValues()) {
				Assert.Throws<ArgumentOutOfRangeException>(() => Decoder.Create(bitness, new ByteArrayCodeReader("90"), 0, DecoderOptions.None));
				Assert.Throws<ArgumentOutOfRangeException>(() => Decoder.Create(bitness, new byte[] { 0x90 }, 0, DecoderOptions.None));
				Assert.Throws<ArgumentOutOfRangeException>(() => Decoder.Create(bitness, new ByteArrayCodeReader("90"), DecoderOptions.None));
				Assert.Throws<ArgumentOutOfRangeException>(() => Decoder.Create(bitness, new byte[] { 0x90 }, DecoderOptions.None));
			}

			foreach (var bitness in new[] { 16, 32, 64 }) {
				Assert.Throws<ArgumentNullException>(() => Decoder.Create(bitness, (CodeReader)null, 0, DecoderOptions.None));
				Assert.Throws<ArgumentNullException>(() => Decoder.Create(bitness, (byte[])null, 0, DecoderOptions.None));
				Assert.Throws<ArgumentNullException>(() => Decoder.Create(bitness, (CodeReader)null, DecoderOptions.None));
				Assert.Throws<ArgumentNullException>(() => Decoder.Create(bitness, (byte[])null, DecoderOptions.None));
			}
		}

#if ENCODER
		[Fact]
		void Instruction_operator_eq_neq() {
			var instr1a = Instruction.Create(Code.Mov_r64_rm64, Register.RAX, Register.RCX);
			var instr1b = instr1a;
			var instruction2 = Instruction.Create(Code.Mov_r64_rm64, Register.RAX, Register.RDX);
			Assert.True(instr1a == instr1b);
			Assert.False(instr1a == instruction2);
			Assert.True(instr1a != instruction2);
			Assert.False(instr1a != instr1b);
		}
#endif

		[Fact]
		void Decode_with_too_few_bytes_left() {
			foreach (var tc in DecoderTestUtils.GetDecoderTests(includeOtherTests: true, includeInvalid: false)) {
				var bytes = HexUtils.ToByteArray(tc.HexBytes);
				for (int i = 0; i + 1 < bytes.Length; i++) {
					var decoder = Decoder.Create(tc.Bitness, new ByteArrayCodeReader(bytes, 0, i), tc.Options);
					decoder.IP = 0x1000;
					decoder.Decode(out var instr);
					Assert.Equal(0x1000UL + (ulong)i, decoder.IP);
					Assert.Equal(Code.INVALID, instr.Code);
					Assert.Equal(DecoderError.NoMoreBytes, decoder.LastError);
				}
			}
		}

		[Fact]
		void Decode_ctor_with_byte_array_arg() {
			var decoder = Decoder.Create(16, new byte[] { 0x01, 0xCE }, DecoderOptions.None);
			Assert.Equal(16, decoder.Bitness);
			decoder.Decode(out var instr);
			Assert.Equal(Code.Add_rm16_r16, instr.Code);

			decoder = Decoder.Create(32, new byte[] { 0x01, 0xCE }, DecoderOptions.None);
			Assert.Equal(32, decoder.Bitness);
			decoder.Decode(out instr);
			Assert.Equal(Code.Add_rm32_r32, instr.Code);

			decoder = Decoder.Create(64, new byte[] { 0x48, 0x01, 0xCE }, DecoderOptions.None);
			Assert.Equal(64, decoder.Bitness);
			decoder.Decode(out instr);
			Assert.Equal(Code.Add_rm64_r64, instr.Code);
		}

		InstructionList EnumeratorDecode(Decoder decoder) {
			var list = new InstructionList();
			foreach (var instr in decoder)
				list.Add(instr);
			return list;
		}

		[Fact]
		void Decode_enumerator_empty() {
			var data = Array.Empty<byte>();
			var decoder = Decoder.Create(64, data);
			var list = EnumeratorDecode(decoder);
			Assert.Equal(0, list.Count);

			decoder = Decoder.Create(64, data);
			var array = decoder.ToArray();
			Assert.Equal(list, array);
		}

		[Fact]
		void Decode_enumerator_one() {
			var data = new byte[] { 0x00, 0xCE };
			var decoder = Decoder.Create(64, data);
			var list = EnumeratorDecode(decoder);
			Assert.Equal(1, list.Count);
			Assert.Equal(Code.Add_rm8_r8, list[0].Code);

			decoder = Decoder.Create(64, data);
			var array = decoder.ToArray();
			Assert.Equal(list, array);
		}

		[Fact]
		void Decode_enumerator_two() {
			var data = new byte[] { 0x00, 0xCE, 0x66, 0x09, 0xCE };
			var decoder = Decoder.Create(64, data);
			var list = EnumeratorDecode(decoder);
			Assert.Equal(2, list.Count);
			Assert.Equal(Code.Add_rm8_r8, list[0].Code);
			Assert.Equal(Code.Or_rm16_r16, list[1].Code);

			decoder = Decoder.Create(64, data);
			var array = decoder.ToArray();
			Assert.Equal(list, array);
		}

		[Fact]
		void Decode_enumerator_incomplete_instruction_one() {
			var data = new byte[] { 0x66 };
			var decoder = Decoder.Create(64, data);
			var list = EnumeratorDecode(decoder);
			Assert.Equal(1, list.Count);
			Assert.Equal(Code.INVALID, list[0].Code);

			decoder = Decoder.Create(64, data);
			var array = decoder.ToArray();
			Assert.Equal(list, array);
		}

		[Fact]
		void Decode_enumerator_incomplete_instruction_two() {
			var data = new byte[] { 0x00, 0xCE, 0x66, 0x09 };
			var decoder = Decoder.Create(64, data);
			var list = EnumeratorDecode(decoder);
			Assert.Equal(2, list.Count);
			Assert.Equal(Code.Add_rm8_r8, list[0].Code);
			Assert.Equal(Code.INVALID, list[1].Code);

			decoder = Decoder.Create(64, data);
			var array = decoder.ToArray();
			Assert.Equal(list, array);
		}

		[Fact]
		void Decoder_without_ip() {
			{
				var decoder = Decoder.Create(64, new ByteArrayCodeReader(new byte[] { }), DecoderOptions.None);
				Assert.Equal(0UL, decoder.IP);
			}
			{
				var decoder = Decoder.Create(64, new byte[] { }, DecoderOptions.None);
				Assert.Equal(0UL, decoder.IP);
			}
		}

		[Fact]
		void Decoder_with_ip() {
			{
				var decoder = Decoder.Create(64, new ByteArrayCodeReader(new byte[] { }), 0x123456789ABCDEF1UL, DecoderOptions.None);
				Assert.Equal(0x123456789ABCDEF1UL, decoder.IP);
			}
			{
				var decoder = Decoder.Create(64, new byte[] { }, 0x123456789ABCDEF1UL, DecoderOptions.None);
				Assert.Equal(0x123456789ABCDEF1UL, decoder.IP);
			}
		}
	}
}
