/*
    Copyright (C) 2018-2019 de4dot@gmail.com

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

using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.InstructionTests {
	public sealed class GetVirtualAddressTests32 : GetVirtualAddressTests {
		const int bitness = 32;

		[Fact]
		void MemorySegSI() {
			var getRegValue = new VARegisterValueProviderImpl((Register.SI, 0x123456789ABCE5D1), (Register.DS, 0x7654321001234567));
			TestBase(bitness, "67 A5", 1, 0x7654321001242B38, getRegValue);
		}

		[Fact]
		void MemorySegSI_fs() {
			var getRegValue = new VARegisterValueProviderImpl((Register.SI, 0x123456789ABCE5D1), (Register.FS, 0x7654321001234567));
			TestBase(bitness, "64 67 A5", 1, 0x7654321001242B38, getRegValue);
		}

		[Fact]
		void MemorySegESI() {
			var getRegValue = new VARegisterValueProviderImpl((Register.ESI, 0x123456785879E5D1), (Register.DS, 0x7654321001234567));
			TestBase(bitness, "A5", 1, 0x76543210599D2B38, getRegValue);
		}

		[Fact]
		void MemorySegESI_fs() {
			var getRegValue = new VARegisterValueProviderImpl((Register.ESI, 0x123456785879E5D1), (Register.FS, 0x7654321001234567));
			TestBase(bitness, "64 A5", 1, 0x76543210599D2B38, getRegValue);
		}

		[Fact]
		void MemoryESDI() {
			var getRegValue = new VARegisterValueProviderImpl((Register.DI, 0x123456789ABCE5D1), (Register.ES, 0x7654321001234567));
			TestBase(bitness, "67 A5", 0, 0x7654321001242B38, getRegValue);
		}

		[Fact]
		void MemoryESEDI() {
			var getRegValue = new VARegisterValueProviderImpl((Register.EDI, 0x12345678FEDCBA8A), (Register.ES, 0x7654321001234567));
			TestBase(bitness, "A5", 0, 0x76543210FFFFFFF1, getRegValue);
		}

		[Fact]
		void MemorySegDI() {
			var getRegValue = new VARegisterValueProviderImpl((Register.DI, 0x123456789ABCE5D1), (Register.DS, 0x7654321001234567));
			TestBase(bitness, "67 C5F9 F7 D3", 0, 0x7654321001242B38, getRegValue);
		}

		[Fact]
		void MemorySegDI_fs() {
			var getRegValue = new VARegisterValueProviderImpl((Register.DI, 0x123456789ABCE5D1), (Register.FS, 0x7654321001234567));
			TestBase(bitness, "64 67 C5F9 F7 D3", 0, 0x7654321001242B38, getRegValue);
		}

		[Fact]
		void MemorySegEDI() {
			var getRegValue = new VARegisterValueProviderImpl((Register.EDI, 0x123456785879E5D1), (Register.DS, 0x7654321001234567));
			TestBase(bitness, "C5F9 F7 D3", 0, 0x76543210599D2B38, getRegValue);
		}

		[Fact]
		void MemorySegEDI_fs() {
			var getRegValue = new VARegisterValueProviderImpl((Register.EDI, 0x123456785879E5D1), (Register.FS, 0x7654321001234567));
			TestBase(bitness, "64 C5F9 F7 D3", 0, 0x76543210599D2B38, getRegValue);
		}

		[Fact]
		void Memory_1() {
			var getRegValue = new VARegisterValueProviderImpl((Register.EBX, 0x894DC70F5879E5D1), (Register.DS, 0x7654321001234567));
			TestBase(bitness, "01 33", 0, 0x76543210599D2B38, getRegValue);
		}

		[Fact]
		void Memory_1_es() {
			var getRegValue = new VARegisterValueProviderImpl((Register.EBX, 0x894DC70F5879E5D1), (Register.ES, 0x7654321001234567));
			TestBase(bitness, "26 01 33", 0, 0x76543210599D2B38, getRegValue);
		}

		[Fact]
		void Memory_2() {
			var getRegValue = new VARegisterValueProviderImpl((Register.EBP, 0x894DC70F5879E5D1), (Register.SS, 0x7654321001234567));
			TestBase(bitness, "01 75 A5", 0, 0x76543210599D2ADD, getRegValue);
		}

		[Fact]
		void Memory_2_cs() {
			var getRegValue = new VARegisterValueProviderImpl((Register.EBP, 0x894DC70F5879E5D1), (Register.CS, 0x7654321001234567));
			TestBase(bitness, "2E 01 75 A5", 0, 0x76543210599D2ADD, getRegValue);
		}

		[Fact]
		void Memory_2_ds() {
			var getRegValue = new VARegisterValueProviderImpl((Register.EBP, 0x894DC70F5879E5D1), (Register.DS, 0x7654321001234567));
			TestBase(bitness, "3E 01 75 A5", 0, 0x76543210599D2ADD, getRegValue);
		}

		[Fact]
		void Memory_3() {
			var getRegValue = new VARegisterValueProviderImpl((Register.EDX, 0x894DC70F5879E5D1), (Register.DS, 0x7654321001234567));
			TestBase(bitness, "01 72 5A", 0, 0x76543210599D2B92, getRegValue);
		}

		[Fact]
		void Memory_3_ss() {
			var getRegValue = new VARegisterValueProviderImpl((Register.EDX, 0x894DC70F5879E5D1), (Register.SS, 0x7654321001234567));
			TestBase(bitness, "36 01 72 5A", 0, 0x76543210599D2B92, getRegValue);
		}

		[Fact]
		void Memory_4() {
			var getRegValue = new VARegisterValueProviderImpl((Register.EBX, 0x894DC70F5879E5D1), (Register.ESI, 0x8ACE506BB562E861), (Register.DS, 0x7654321001234567));
			TestBase(bitness, "01 34 33", 0, 0x765432100F001399, getRegValue);
		}

		[Fact]
		void Memory_4_fs() {
			var getRegValue = new VARegisterValueProviderImpl((Register.EBX, 0x894DC70F5879E5D1), (Register.ESI, 0x8ACE506BB562E861), (Register.FS, 0x7654321001234567));
			TestBase(bitness, "64 01 34 33", 0, 0x765432100F001399, getRegValue);
		}

		[Fact]
		void Memory_5() {
			var getRegValue = new VARegisterValueProviderImpl((Register.EBX, 0x894DC70F5879E5D1), (Register.ESI, 0x8ACE506BB562E861), (Register.DS, 0x7654321001234567));
			TestBase(bitness, "01 74 33 A5", 0, 0x765432100F00133E, getRegValue);
		}

		[Fact]
		void Memory_5_gs() {
			var getRegValue = new VARegisterValueProviderImpl((Register.EBX, 0x894DC70F5879E5D1), (Register.ESI, 0x8ACE506BB562E861), (Register.GS, 0x7654321001234567));
			TestBase(bitness, "65 01 74 33 A5", 0, 0x765432100F00133E, getRegValue);
		}

		[Fact]
		void Memory_6() {
			var getRegValue = new VARegisterValueProviderImpl((Register.EBX, 0x894DC70F5879E5D1), (Register.ESI, 0x8ACE506BB562E861), (Register.DS, 0x7654321001234567));
			TestBase(bitness, "01 74 33 5A", 0, 0x765432100F0013F3, getRegValue);
		}

		[Fact]
		void Memory_7() {
			var getRegValue = new VARegisterValueProviderImpl((Register.EDX, 0x894DC70F5879E5D1), (Register.DS, 0x7654321001234567));
			TestBase(bitness, "01 34 15 34125AA5", 0, 0x76543210FEF73D6C, getRegValue);
		}

		[Fact]
		void Memory_8() {
			var getRegValue = new VARegisterValueProviderImpl((Register.EDX, 0x894DC70F5879E5D1), (Register.DS, 0x7654321001234567));
			TestBase(bitness, "01 34 15 34125A75", 0, 0x76543210CEF73D6C, getRegValue);
		}

		[Fact]
		void Memory_9() {
			var getRegValue = new VARegisterValueProviderImpl((Register.EBX, 0x894DC70F5879E5D1), (Register.ESI, 0x8ACE506BB562E861), (Register.DS, 0x7654321001234567));
			TestBase(bitness, "01 74 33 A5", 0, 0x765432100F00133E, getRegValue);
		}

		[Fact]
		void Memory_10() {
			var getRegValue = new VARegisterValueProviderImpl((Register.EBX, 0x894DC70F5879E5D1), (Register.ESI, 0x8ACE506BB562E861), (Register.DS, 0x7654321001234567));
			TestBase(bitness, "01 74 73 A5", 0, 0x76543210C462FB9F, getRegValue);
		}

		[Fact]
		void Memory_11() {
			var getRegValue = new VARegisterValueProviderImpl((Register.EBX, 0x894DC70F5879E5D1), (Register.ESI, 0x8ACE506BB562E861), (Register.DS, 0x7654321001234567));
			TestBase(bitness, "01 74 B3 A5", 0, 0x765432102F28CC61, getRegValue);
		}

		[Fact]
		void Memory_12() {
			var getRegValue = new VARegisterValueProviderImpl((Register.EBX, 0x894DC70F5879E5D1), (Register.ESI, 0x8ACE506BB562E861), (Register.DS, 0x7654321001234567));
			TestBase(bitness, "01 74 F3 A5", 0, 0x7654321004B46DE5, getRegValue);
		}

		[Fact]
		void Memory_13() {
			var getRegValue = new VARegisterValueProviderImpl((Register.XMM1, 1, 4, 0x8ACE506BB562E861), (Register.DS, 0, 0, 0x7654321001234567));
			TestBase(bitness, "62 F27D09 A0 34 0D 8967A55A", 0, 1, 0x76543210112B9551, getRegValue);
		}

		[Fact]
		void Memory_14() {
			var getRegValue = new VARegisterValueProviderImpl((Register.XMM1, 2, 4, 0x8ACE506BB562E861), (Register.DS, 0, 0, 0x7654321001234567));
			TestBase(bitness, "62 F27D09 A0 34 CD 8967A55A", 0, 2, 0x7654321006DFEFF8, getRegValue);
		}

		[Fact]
		void Memory_15() {
			var getRegValue = new VARegisterValueProviderImpl((Register.XMM1, 1, 8, 0x8ACE506BB562E861), (Register.DS, 0, 0, 0x7654321001234567));
			TestBase(bitness, "62 F27D09 A1 34 0D 8967A55A", 0, 1, 0x76543210112B9551, getRegValue);
		}

		[Fact]
		void Memory_16() {
			var getRegValue = new VARegisterValueProviderImpl((Register.XMM1, 2, 8, 0x8ACE506BB562E861), (Register.DS, 0, 0, 0x7654321001234567));
			TestBase(bitness, "62 F27D09 A1 34 CD 8967A55A", 0, 2, 0x7654321006DFEFF8, getRegValue);
		}

		[Fact]
		void Memory_17() {
			var getRegValue = new VARegisterValueProviderImpl((Register.EBX, 0x123456789ABCE5D1), (Register.DS, 0x7654321001234567), (Register.AL, 0x5A));
			TestBase(bitness, "D7", 0, 0x765432109BE02B92, getRegValue);
		}
	}
}
