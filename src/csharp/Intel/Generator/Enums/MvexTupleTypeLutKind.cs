// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Enums {
	[Enum("MvexTupleTypeLutKind", Documentation = "MVEX tuple type lut kind used together with the #(c:MVEX.SSS)# bits to get the tuple type", Public = true)]
	enum MvexTupleTypeLutKind {
		[Comment("#(t:i32)# elements, eg. #(c:Si32)#/#(c:Di32)#/#(c:Ui32)#")]
		Int32,
		[Comment("#(t:i32)# elements, eg. #(c:Si32)#/#(c:Di32)#/#(c:Ui32)# with half memory size (32 bytes instead of 64 bytes, eg. #(c:VCVTUDQ2PD)#/#(c:VCVTDQ2PD)#)")]
		Int32_Half,
		[Comment("#(t:i32)# elements, eg. #(c:Si32)#/#(c:Di32)#/#(c:Ui32)# with built-in #(c:{4to16})# broadcast")]
		Int32_4to16,
		[Comment("#(t:i32)# elements, eg. #(c:Si32)#/#(c:Di32)#/#(c:Ui32)# with built-in #(c:{1to16})# broadcast or element level")]
		Int32_1to16_or_elem,
		[Comment("#(t:i64)# elements, eg. #(c:Si64)#/#(c:Di64)#/#(c:Ui64)#")]
		Int64,
		[Comment("#(t:i64)# elements, eg. #(c:Si64)#/#(c:Di64)#/#(c:Ui64)# with built-in #(c:{4to8})# broadcast")]
		Int64_4to8,
		[Comment("#(t:i64)# elements, eg. #(c:Si64)#/#(c:Di64)#/#(c:Ui64)# with built-in #(c:{1to8})# broadcast or element level")]
		Int64_1to8_or_elem,
		[Comment("#(t:f32)# elements, eg. #(c:Sf32)#/#(c:Df32)#/#(c:Uf32)#")]
		Float32,
		[Comment("#(t:f32)# elements, eg. #(c:Sf32)#/#(c:Df32)#/#(c:Uf32)# with half memory size (32 bytes instead of 64 bytes, eg. #(c:VCVTPS2PD)#")]
		Float32_Half,
		[Comment("#(t:f32)# elements, eg. #(c:Sf32)#/#(c:Df32)#/#(c:Uf32)# with built-in #(c:{4to16})# broadcast")]
		Float32_4to16,
		[Comment("#(t:f32)# elements, eg. #(c:Sf32)#/#(c:Df32)#/#(c:Uf32)# with built-in #(c:{1to16})# broadcast or element level")]
		Float32_1to16_or_elem,
		[Comment("#(t:f64)# elements, eg. #(c:Sf64)#/#(c:Df64)#/#(c:Uf64)#")]
		Float64,
		[Comment("#(t:f64)# elements, eg. #(c:Sf64)#/#(c:Df64)#/#(c:Uf64)# with built-in #(c:{4to8})# broadcast")]
		Float64_4to8,
		[Comment("#(t:f64)# elements, eg. #(c:Sf64)#/#(c:Df64)#/#(c:Uf64)# with built-in #(c:{1to8})# broadcast or element level")]
		Float64_1to8_or_elem,
	}
}
