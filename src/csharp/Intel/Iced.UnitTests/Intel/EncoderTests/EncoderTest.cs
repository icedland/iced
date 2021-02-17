// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if ENCODER
using System;
using System.Collections.Generic;
using Iced.Intel;
using Iced.UnitTests.Intel.DecoderTests;
using Xunit;

namespace Iced.UnitTests.Intel.EncoderTests {
	public abstract class EncoderTest {
		static string ToString(byte[] bytes) {
			if (bytes.Length == 0)
				return string.Empty;
			var builder = new System.Text.StringBuilder(bytes.Length * 3 - 1);
			for (int i = 0; i < bytes.Length; i++) {
				if (builder.Length > 0)
					builder.Append(' ');
				builder.Append(bytes[i].ToString("X2"));
			}
			return builder.ToString();
		}

		static bool ArrayEquals(byte[] a, byte[] b) {
			if (a.Length != b.Length)
				return false;
			for (int i = 0; i < a.Length; i++) {
				if (a[i] != b[i])
					return false;
			}
			return true;
		}

		protected void EncodeBase(uint id, int bitness, Code code, string hexBytes, ulong ip, string encodedHexBytes, DecoderOptions options) {
			var origBytes = HexUtils.ToByteArray(hexBytes);
			var decoder = CreateDecoder(bitness, origBytes, ip, options);
			var origRip = decoder.IP;
			var origInstr = decoder.Decode();
			var origConstantOffsets = decoder.GetConstantOffsets(origInstr);
			Assert.Equal(code, origInstr.Code);
			Assert.Equal(origBytes.Length, origInstr.Length);
			Assert.True(origInstr.Length <= IcedConstants.MaxInstructionLength);
			Assert.Equal((ushort)origRip, origInstr.IP16);
			Assert.Equal((uint)origRip, origInstr.IP32);
			Assert.Equal(origRip, origInstr.IP);
			var afterRip = decoder.IP;
			Assert.Equal((ushort)afterRip, origInstr.NextIP16);
			Assert.Equal((uint)afterRip, origInstr.NextIP32);
			Assert.Equal(afterRip, origInstr.NextIP);

			var writer = new CodeWriterImpl();
			var encoder = Encoder.Create(decoder.Bitness, writer);
			Assert.Equal(bitness, encoder.Bitness);
			var origInstrCopy = origInstr;
			bool result = encoder.TryEncode(origInstr, origRip, out uint encodedInstrLen, out string errorMessage);
			Assert.True(errorMessage is null, "Unexpected error message: " + errorMessage);
			Assert.True(result, "Error, result from Encoder.TryEncode must be true");
			var encodedConstantOffsets = encoder.GetConstantOffsets();
			FixConstantOffsets(ref encodedConstantOffsets, origInstr.Length, (int)encodedInstrLen);
			Assert.True(Equals(ref origConstantOffsets, ref encodedConstantOffsets));
			var encodedBytes = writer.ToArray();
			Assert.Equal(encodedBytes.Length, (int)encodedInstrLen);
			Assert.True(Instruction.EqualsAllBits(origInstr, origInstrCopy));

			var expectedBytes = HexUtils.ToByteArray(encodedHexBytes);
			if (!ArrayEquals(expectedBytes, encodedBytes)) {
#pragma warning disable xUnit2006 // Do not use invalid string equality check
				// Show the full string without ellipses by using Equal<string>() instead of Equal()
				Assert.Equal<string>(ToString(expectedBytes), ToString(encodedBytes));
				throw new InvalidOperationException();
#pragma warning restore xUnit2006 // Do not use invalid string equality check
			}

			var newInstr = CreateDecoder(bitness, encodedBytes, ip, options).Decode();
			Assert.Equal(code, newInstr.Code);
			Assert.Equal(encodedBytes.Length, newInstr.Length);
			newInstr.Length = origInstr.Length;
			newInstr.NextIP = origInstr.NextIP;
			Assert.True(Instruction.EqualsAllBits(origInstr, newInstr));
		}

		static void FixConstantOffsets(ref ConstantOffsets co, int origInstrLen, int newInstrLen) {
			byte diff = (byte)(origInstrLen - newInstrLen);
			if (co.HasDisplacement)
				co.DisplacementOffset += diff;
			if (co.HasImmediate)
				co.ImmediateOffset += diff;
			if (co.HasImmediate2)
				co.ImmediateOffset2 += diff;
		}

		static bool Equals(ref ConstantOffsets a, ref ConstantOffsets b) =>
			a.DisplacementOffset == b.DisplacementOffset &&
			a.ImmediateOffset == b.ImmediateOffset &&
			a.ImmediateOffset2 == b.ImmediateOffset2 &&
			a.DisplacementSize == b.DisplacementSize &&
			a.ImmediateSize == b.ImmediateSize &&
			a.ImmediateSize2 == b.ImmediateSize2;

		protected void NonDecodeEncodeBase(int bitness, ref Instruction instruction, string hexBytes, ulong rip) {
			var expectedBytes = HexUtils.ToByteArray(hexBytes);
			var writer = new CodeWriterImpl();
			var encoder = Encoder.Create(bitness, writer);
			Assert.Equal(bitness, encoder.Bitness);
			bool result = encoder.TryEncode(instruction, rip, out uint encodedInstrLen, out string errorMessage);
			Assert.True(errorMessage is null, "Unexpected error message: " + errorMessage);
			Assert.True(result, "Error, result from Encoder.TryEncode must be true");
			var encodedBytes = writer.ToArray();
			if (!ArrayEquals(expectedBytes, encodedBytes)) {
#pragma warning disable xUnit2006 // Do not use invalid string equality check
				// Show the full string without ellipses by using Equal<string>() instead of Equal()
				Assert.Equal<string>(ToString(expectedBytes), ToString(encodedBytes));
				throw new InvalidOperationException();
#pragma warning restore xUnit2006 // Do not use invalid string equality check
			}
			Assert.Equal(encodedBytes.Length, (int)encodedInstrLen);
		}

		protected void EncodeInvalidBase(uint id, int bitness, Code code, string hexBytes, ulong ip, DecoderOptions options, int invalidBitness) {
			var origBytes = HexUtils.ToByteArray(hexBytes);
			var decoder = CreateDecoder(bitness, origBytes, ip, options);
			var origRip = decoder.IP;
			var origInstr = decoder.Decode();
			Assert.Equal(code, origInstr.Code);
			Assert.Equal(origBytes.Length, origInstr.Length);
			Assert.True(origInstr.Length <= IcedConstants.MaxInstructionLength);
			Assert.Equal((ushort)origRip, origInstr.IP16);
			Assert.Equal((uint)origRip, origInstr.IP32);
			Assert.Equal(origRip, origInstr.IP);
			var afterRip = decoder.IP;
			Assert.Equal((ushort)afterRip, origInstr.NextIP16);
			Assert.Equal((uint)afterRip, origInstr.NextIP32);
			Assert.Equal(afterRip, origInstr.NextIP);

			var writer = new CodeWriterImpl();
			var encoder = CreateEncoder(invalidBitness, writer);
			bool result = encoder.TryEncode(origInstr, origRip, out uint encodedInstrLen, out string errorMessage);
			Assert.Equal(invalidBitness == 64 ? Encoder.ERROR_ONLY_1632_BIT_MODE : Encoder.ERROR_ONLY_64_BIT_MODE, errorMessage);
			Assert.False(result);
		}

		Encoder CreateEncoder(int bitness, CodeWriter writer) {
			var encoder = Encoder.Create(bitness, writer);
			Assert.Equal(bitness, encoder.Bitness);
			return encoder;
		}

		Decoder CreateDecoder(int bitness, byte[] hexBytes, ulong ip, DecoderOptions options) {
			var codeReader = new ByteArrayCodeReader(hexBytes);
			var decoder = Decoder.Create(bitness, codeReader, options);
			decoder.IP = ip;
			Assert.Equal(bitness, decoder.Bitness);
			return decoder;
		}

		protected static IEnumerable<object[]> GetEncodeData(int bitness) {
			foreach (var info in DecoderTestUtils.GetEncoderTests(includeOtherTests: true, includeInvalid: false)) {
				if (bitness != info.Bitness)
					continue;
				yield return new object[] { info.Id, info.Bitness, info.Code, info.HexBytes, info.IP, info.EncodedHexBytes, info.Options };
			}
		}

		protected static IEnumerable<object[]> GetNonDecodedEncodeData(int bitness) {
			foreach (var info in NonDecodedInstructions.GetTests()) {
				if (bitness != info.bitness)
					continue;
				ulong rip = 0;
				yield return new object[] { info.bitness, info.instruction, info.hexBytes, rip };
			}
		}
	}
}
#endif
