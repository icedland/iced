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

#if !NO_ENCODER
using System;
using System.Collections.Generic;
using Iced.Intel;
using Iced.UnitTests.Intel.DecoderTests;
using Xunit;

namespace Iced.UnitTests.Intel.EncoderTests {
	public abstract class EncoderTest {
		static string ToString(byte[] hexData) {
			if (hexData.Length == 0)
				return string.Empty;
			var builder = new System.Text.StringBuilder(hexData.Length * 3 - 1);
			for (int i = 0; i < hexData.Length; i++) {
				if (i > 0)
					builder.Append(' ');
				builder.Append(hexData[i].ToString("X2"));
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

		protected void EncodeBase(int codeSize, Code code, string hexBytes, string encodedHexBytes, DecoderOptions options) {
			var origBytes = HexUtils.ToByteArray(hexBytes);
			var decoder = CreateDecoder(codeSize, origBytes, options);
			var origRip = decoder.IP;
			var origInstr = decoder.Decode();
			var origConstantOffsets = decoder.GetConstantOffsets(origInstr);
			Assert.Equal(code, origInstr.Code);
			Assert.Equal(origBytes.Length, origInstr.ByteLength);
			Assert.True(origInstr.ByteLength <= Iced.Intel.DecoderConstants.MaxInstructionLength);
			Assert.Equal((ushort)origRip, origInstr.IP16);
			Assert.Equal((uint)origRip, origInstr.IP32);
			Assert.Equal(origRip, origInstr.IP);
			var afterRip = decoder.IP;
			Assert.Equal((ushort)afterRip, origInstr.NextIP16);
			Assert.Equal((uint)afterRip, origInstr.NextIP32);
			Assert.Equal(afterRip, origInstr.NextIP);

			var writer = new CodeWriterImpl();
			var encoder = Encoder.Create(decoder.Bitness, writer);
			Assert.Equal(codeSize, encoder.Bitness);
			var origInstrCopy = origInstr;
			bool result = encoder.TryEncode(origInstr, origRip, out uint encodedInstrLen, out string errorMessage);
			Assert.True(errorMessage is null, "Unexpected ErrorMessage: " + errorMessage);
			Assert.True(result, "Error, result from Encoder.TryEncode must be true");
			var encodedConstantOffsets = encoder.GetConstantOffsets();
			FixConstantOffsets(ref encodedConstantOffsets, origInstr.ByteLength, (int)encodedInstrLen);
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

			var newInstr = CreateDecoder(codeSize, encodedBytes, options).Decode();
			Assert.Equal(code, newInstr.Code);
			Assert.Equal(encodedBytes.Length, newInstr.ByteLength);
			newInstr.ByteLength = origInstr.ByteLength;
			newInstr.NextIP = origInstr.NextIP;
			Assert.True(Instruction.EqualsAllBits(origInstr, newInstr));
			// Some tests use useless or extra prefixes, so we can't verify the exact length
			Assert.True(encodedBytes.Length <= origBytes.Length, "Unexpected encoded prefixes: " + ToString(encodedBytes));
		}

		static void FixConstantOffsets(ref ConstantOffsets ca, int origInstrLen, int newInstrLen) {
			byte diff = (byte)(origInstrLen - newInstrLen);
			if (ca.HasDisplacement)
				ca.DisplacementOffset += diff;
			if (ca.HasImmediate)
				ca.ImmediateOffset += diff;
			if (ca.HasImmediate2)
				ca.ImmediateOffset2 += diff;
		}

		static bool Equals(ref ConstantOffsets a, ref ConstantOffsets b) =>
			a.DisplacementOffset == b.DisplacementOffset &&
			a.ImmediateOffset == b.ImmediateOffset &&
			a.ImmediateOffset2 == b.ImmediateOffset2 &&
			a.DisplacementSize == b.DisplacementSize &&
			a.ImmediateSize == b.ImmediateSize &&
			a.ImmediateSize2 == b.ImmediateSize2;

		protected void NonDecodeEncodeBase(int codeSize, ref Instruction instr, string hexBytes, ulong rip) {
			var expectedBytes = HexUtils.ToByteArray(hexBytes);
			var writer = new CodeWriterImpl();
			var encoder = Encoder.Create(codeSize, writer);
			Assert.Equal(codeSize, encoder.Bitness);
			var origInstrCopy = instr;
			bool result = encoder.TryEncode(instr, rip, out uint encodedInstrLen, out string errorMessage);
			Assert.True(errorMessage is null, "Unexpected ErrorMessage: " + errorMessage);
			Assert.True(result, "Error, result from Encoder.TryEncode must be true");
			var encodedBytes = writer.ToArray();
			Assert.Equal(encodedBytes.Length, (int)encodedInstrLen);
			Assert.True(Instruction.EqualsAllBits(instr, origInstrCopy));
			if (!ArrayEquals(expectedBytes, encodedBytes)) {
#pragma warning disable xUnit2006 // Do not use invalid string equality check
				// Show the full string without ellipses by using Equal<string>() instead of Equal()
				Assert.Equal<string>(ToString(expectedBytes), ToString(encodedBytes));
				throw new InvalidOperationException();
#pragma warning restore xUnit2006 // Do not use invalid string equality check
			}
		}

		protected void EncodeInvalidBase(int codeSize, Code code, string hexBytes, DecoderOptions options, int invalidCodeSize) {
			var origBytes = HexUtils.ToByteArray(hexBytes);
			var decoder = CreateDecoder(codeSize, origBytes, options);
			var origRip = decoder.IP;
			var origInstr = decoder.Decode();
			Assert.Equal(code, origInstr.Code);
			Assert.Equal(origBytes.Length, origInstr.ByteLength);
			Assert.True(origInstr.ByteLength <= Iced.Intel.DecoderConstants.MaxInstructionLength);
			Assert.Equal((ushort)origRip, origInstr.IP16);
			Assert.Equal((uint)origRip, origInstr.IP32);
			Assert.Equal(origRip, origInstr.IP);
			var afterRip = decoder.IP;
			Assert.Equal((ushort)afterRip, origInstr.NextIP16);
			Assert.Equal((uint)afterRip, origInstr.NextIP32);
			Assert.Equal(afterRip, origInstr.NextIP);

			var writer = new CodeWriterImpl();
			var encoder = CreateEncoder(invalidCodeSize, writer);
			var origInstrCopy = origInstr;
			bool result = encoder.TryEncode(origInstr, origRip, out uint encodedInstrLen, out string errorMessage);
			Assert.Equal(invalidCodeSize == 64 ? Encoder.ERROR_ONLY_1632_BIT_MODE : Encoder.ERROR_ONLY_64_BIT_MODE, errorMessage);
			Assert.False(result);
			Assert.True(Instruction.EqualsAllBits(origInstr, origInstrCopy));
		}

		Encoder CreateEncoder(int codeSize, CodeWriter writer) {
			var encoder = Encoder.Create(codeSize, writer);
			Assert.Equal(codeSize, encoder.Bitness);
			return encoder;
		}

		Decoder CreateDecoder(int codeSize, byte[] hexBytes, DecoderOptions options) {
			var codeReader = new ByteArrayCodeReader(hexBytes);
			var decoder = Decoder.Create(codeSize, codeReader, options);
			switch (codeSize) {
			case 16:
				decoder.IP = DecoderConstants.DEFAULT_IP16;
				break;

			case 32:
				decoder.IP = DecoderConstants.DEFAULT_IP32;
				break;

			case 64:
				decoder.IP = DecoderConstants.DEFAULT_IP64;
				break;

			default:
				throw new ArgumentOutOfRangeException(nameof(codeSize));
			}

			Assert.Equal(codeSize, decoder.Bitness);
			return decoder;
		}

		protected static IEnumerable<object[]> GetEncodeData(int codeSize) {
			foreach (var info in DecoderTestUtils.GetDecoderTests(includeOtherTests: true, includeInvalid: false)) {
				if (codeSize != info.Bitness)
					continue;
				yield return new object[] { info.Bitness, info.Code, info.HexBytes, info.EncodedHexBytes, info.Options };
			}
		}

		protected static IEnumerable<object[]> GetNonDecodedEncodeData(int codeSize) {
			foreach (var info in NonDecodedInstructions.GetTests()) {
				if (codeSize != info.bitness)
					continue;
				ulong rip = 0;
				yield return new object[] { info.bitness, info.instruction, info.hexBytes, rip };
			}
		}
	}
}
#endif
