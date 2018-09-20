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

#if !NO_ENCODER
using Iced.Intel;
using Iced.Intel.EncoderInternal;
using Xunit;

namespace Iced.UnitTests.Intel.EncoderTests {
	public sealed class MiscTests : EncoderTest {
		[Fact]
		void Verify_Handlers_table_Code_values() {
			var handlers = OpCodeHandlers.Handlers;
			for (int i = 0; i < handlers.Length; i++)
				Assert.Equal((Code)i, handlers[i].TEST_Code);
		}

		[Fact]
		void Encode_INVALID_Code_value_is_an_error() {
			Encoder encoder;
			var instr = new Instruction { Code = Code.INVALID };
			string errorMessage;
			bool result;
			uint instrLen;

			encoder = Encoder.Create16(new CodeWriterImpl());
			result = encoder.TryEncode(ref instr, 0, out instrLen, out errorMessage);
			Assert.False(result);
			Assert.Equal(InvalidHandler.ERROR_MESSAGE, errorMessage);
			Assert.Equal(0U, instrLen);

			encoder = Encoder.Create32(new CodeWriterImpl());
			result = encoder.TryEncode(ref instr, 0, out instrLen, out errorMessage);
			Assert.False(result);
			Assert.Equal(InvalidHandler.ERROR_MESSAGE, errorMessage);
			Assert.Equal(0U, instrLen);

			encoder = Encoder.Create64(new CodeWriterImpl());
			result = encoder.TryEncode(ref instr, 0, out instrLen, out errorMessage);
			Assert.False(result);
			Assert.Equal(InvalidHandler.ERROR_MESSAGE, errorMessage);
			Assert.Equal(0U, instrLen);
		}

		[Fact]
		void Encode_throws() {
			var instr = new Instruction { Code = Code.INVALID };
			var encoder = Encoder.Create64(new CodeWriterImpl());
			Assert.Throws<EncoderException>(() => {
				var instrCopy = instr;
				encoder.Encode(ref instrCopy, 0);
			});
		}
	}
}
#endif
