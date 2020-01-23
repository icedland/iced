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

use super::handlers::OpCodeHandler;
use super::table_de::*;
#[cfg(not(feature = "std"))]
use alloc::vec::Vec;

pub(super) struct Tables {
	pub(super) handlers_xx: Vec<&'static OpCodeHandler>,
	pub(super) handlers_vex_0fxx: Vec<&'static OpCodeHandler>,
	pub(super) handlers_vex_0f38xx: Vec<&'static OpCodeHandler>,
	pub(super) handlers_vex_0f3axx: Vec<&'static OpCodeHandler>,
	pub(super) handlers_evex_0fxx: Vec<&'static OpCodeHandler>,
	pub(super) handlers_evex_0f38xx: Vec<&'static OpCodeHandler>,
	pub(super) handlers_evex_0f3axx: Vec<&'static OpCodeHandler>,
	pub(super) handlers_xop8: Vec<&'static OpCodeHandler>,
	pub(super) handlers_xop9: Vec<&'static OpCodeHandler>,
	pub(super) handlers_xopa: Vec<&'static OpCodeHandler>,
}

lazy_static! {
	pub(super) static ref TABLES: Tables = {
		let handlers_xx = read_legacy();
		let (handlers_vex_0fxx, handlers_vex_0f38xx, handlers_vex_0f3axx) = read_vex();
		let (handlers_evex_0fxx, handlers_evex_0f38xx, handlers_evex_0f3axx) = read_evex();
		let (handlers_xop8, handlers_xop9, handlers_xopa) = read_xop();
		Tables {
			handlers_xx,
			handlers_vex_0fxx,
			handlers_vex_0f38xx,
			handlers_vex_0f3axx,
			handlers_evex_0fxx,
			handlers_evex_0f38xx,
			handlers_evex_0f3axx,
			handlers_xop8,
			handlers_xop9,
			handlers_xopa,
		}
	};
}
