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

using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.InstructionTests {
	public sealed class GetVirtualAddressTests16 : GetVirtualAddressTests {
		const int bitness = 16;

		[Fact]
		void MemorySegSI() {
			var getRegValue = new VARegisterValueProviderImpl((Register.SI, 0x123456789ABCE5D1), (Register.DS, 0x7654321001234567));
			TestBase(bitness, "A5", 1, 0x7654321001242B38, getRegValue);
		}

		[Fact]
		void MemorySegSI_fs() {
			var getRegValue = new VARegisterValueProviderImpl((Register.SI, 0x123456789ABCE5D1), (Register.FS, 0x7654321001234567));
			TestBase(bitness, "64 A5", 1, 0x7654321001242B38, getRegValue);
		}

		[Fact]
		void MemorySegESI() {
			var getRegValue = new VARegisterValueProviderImpl((Register.ESI, 0x12345678FEDCBA8A), (Register.DS, 0x7654321001234567));
			TestBase(bitness, "67 A5", 1, 0x76543210FFFFFFF1, getRegValue);
		}

		[Fact]
		void MemorySegESI_fs() {
			var getRegValue = new VARegisterValueProviderImpl((Register.ESI, 0x12345678FEDCBA8A), (Register.FS, 0x7654321001234567));
			TestBase(bitness, "64 67 A5", 1, 0x76543210FFFFFFF1, getRegValue);
		}

		[Fact]
		void MemoryESDI() {
			var getRegValue = new VARegisterValueProviderImpl((Register.DI, 0x123456789ABCE5D1), (Register.ES, 0x7654321001234567));
			TestBase(bitness, "A5", 0, 0x7654321001242B38, getRegValue);
		}

		[Fact]
		void MemoryESEDI() {
			var getRegValue = new VARegisterValueProviderImpl((Register.EDI, 0x12345678FEDCBA8A), (Register.ES, 0x7654321001234567));
			TestBase(bitness, "67 A5", 0, 0x76543210FFFFFFF1, getRegValue);
		}

		[Fact]
		void MemorySegDI() {
			var getRegValue = new VARegisterValueProviderImpl((Register.DI, 0x123456789ABCE5D1), (Register.DS, 0x7654321001234567));
			TestBase(bitness, "C5F9 F7 D3", 0, 0x7654321001242B38, getRegValue);
		}

		[Fact]
		void MemorySegDI_fs() {
			var getRegValue = new VARegisterValueProviderImpl((Register.DI, 0x123456789ABCE5D1), (Register.FS, 0x7654321001234567));
			TestBase(bitness, "64 C5F9 F7 D3", 0, 0x7654321001242B38, getRegValue);
		}

		[Fact]
		void MemorySegEDI() {
			var getRegValue = new VARegisterValueProviderImpl((Register.EDI, 0x12345678FEDCBA8A), (Register.DS, 0x7654321001234567));
			TestBase(bitness, "67 C5F9 F7 D3", 0, 0x76543210FFFFFFF1, getRegValue);
		}

		[Fact]
		void MemorySegEDI_fs() {
			var getRegValue = new VARegisterValueProviderImpl((Register.EDI, 0x12345678FEDCBA8A), (Register.FS, 0x7654321001234567));
			TestBase(bitness, "64 67 C5F9 F7 D3", 0, 0x76543210FFFFFFF1, getRegValue);
		}

		[Fact]
		void Memory_1() {
			var getRegValue = new VARegisterValueProviderImpl((Register.DS, 0x7654321001234567));
			TestBase(bitness, "01 06 5AA5", 0, 0x765432100123EAC1, getRegValue);
		}

		[Fact]
		void Memory_1_gs() {
			var getRegValue = new VARegisterValueProviderImpl((Register.GS, 0x7654321001234567));
			TestBase(bitness, "65 01 06 5AA5", 0, 0x765432100123EAC1, getRegValue);
		}

		[Fact]
		void Memory_2() {
			var getRegValue = new VARegisterValueProviderImpl((Register.BX, 0x123456789ABCE5D1), (Register.DS, 0x7654321001234567));
			TestBase(bitness, "01 07", 0, 0x7654321001242B38, getRegValue);
		}

		[Fact]
		void Memory_2_es() {
			var getRegValue = new VARegisterValueProviderImpl((Register.BX, 0x123456789ABCE5D1), (Register.ES, 0x7654321001234567));
			TestBase(bitness, "26 01 07", 0, 0x7654321001242B38, getRegValue);
		}

		[Fact]
		void Memory_3() {
			var getRegValue = new VARegisterValueProviderImpl((Register.BP, 0x123456789ABCE5D1), (Register.DI, 0xBECD), (Register.SS, 0x7654321001234567));
			TestBase(bitness, "01 0B", 0, 0x765432100123EA05, getRegValue);
		}

		[Fact]
		void Memory_3_cs() {
			var getRegValue = new VARegisterValueProviderImpl((Register.BP, 0x123456789ABCE5D1), (Register.DI, 0xBECD), (Register.CS, 0x7654321001234567));
			TestBase(bitness, "2E 01 0B", 0, 0x765432100123EA05, getRegValue);
		}

		[Fact]
		void Memory_4() {
			var getRegValue = new VARegisterValueProviderImpl((Register.SI, 0x123456789ABCE5D1), (Register.DS, 0x7654321001234567));
			TestBase(bitness, "01 44 A5", 0, 0x7654321001242ADD, getRegValue);
		}

		[Fact]
		void Memory_4_ss() {
			var getRegValue = new VARegisterValueProviderImpl((Register.SI, 0x123456789ABCE5D1), (Register.SS, 0x7654321001234567));
			TestBase(bitness, "36 01 44 A5", 0, 0x7654321001242ADD, getRegValue);
		}

		[Fact]
		void Memory_5() {
			var getRegValue = new VARegisterValueProviderImpl((Register.BP, 0x123456789ABCFFEF), (Register.SS, 0x7654321001234567));
			TestBase(bitness, "01 46 5A", 0, 0x76543210012345B0, getRegValue);
		}

		[Fact]
		void Memory_5_ds() {
			var getRegValue = new VARegisterValueProviderImpl((Register.BP, 0x123456789ABCFFEF), (Register.DS, 0x7654321001234567));
			TestBase(bitness, "3E 01 46 5A", 0, 0x76543210012345B0, getRegValue);
		}

		[Fact]
		void Memory_6() {
			var getRegValue = new VARegisterValueProviderImpl((Register.BX, 0x123456789ABCE5D1), (Register.DS, 0x7654321001234567));
			TestBase(bitness, "01 87 5AA5", 0, 0x765432100123D092, getRegValue);
		}

		[Fact]
		void Memory_6_fs() {
			var getRegValue = new VARegisterValueProviderImpl((Register.BX, 0x123456789ABCE5D1), (Register.FS, 0x7654321001234567));
			TestBase(bitness, "64 01 87 5AA5", 0, 0x765432100123D092, getRegValue);
		}

		[Fact]
		void Memory_7() {
			var getRegValue = new VARegisterValueProviderImpl((Register.BX, 0x123456789ABCE5D1), (Register.DS, 0x7654321001234567), (Register.AL, 0x5A));
			TestBase(bitness, "D7", 0, 0x7654321001242B92, getRegValue);
		}
	}
}
