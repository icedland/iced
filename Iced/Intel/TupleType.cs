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

#if ((!NO_DECODER32 || !NO_DECODER64) && !NO_DECODER) || !NO_ENCODER
namespace Iced.Intel {
	enum TupleType : byte {
		None,
		Full_128,
		Full_256,
		Full_512,
		Half_128,
		Half_256,
		Half_512,
		Full_Mem_128,
		Full_Mem_256,
		Full_Mem_512,
		Tuple1_Scalar,
		Tuple1_Scalar_1,
		Tuple1_Scalar_2,
		Tuple1_Scalar_4,
		Tuple1_Scalar_8,
		Tuple1_Fixed,
		Tuple1_Fixed_4,
		Tuple1_Fixed_8,
		Tuple2,
		Tuple4,
		Tuple8,
		Tuple1_4X,
		Half_Mem_128,
		Half_Mem_256,
		Half_Mem_512,
		Quarter_Mem_128,
		Quarter_Mem_256,
		Quarter_Mem_512,
		Eighth_Mem_128,
		Eighth_Mem_256,
		Eighth_Mem_512,
		Mem128,
		MOVDDUP_128,
		MOVDDUP_256,
		MOVDDUP_512,
	}
}
#endif
