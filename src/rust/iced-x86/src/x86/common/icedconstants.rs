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

#![allow(dead_code)] //TODO: REMOVE

use super::{Code, MemorySize, Register};

// GENERATOR-BEGIN: IcedConstants
pub(crate) struct IcedConstants;
impl IcedConstants {
	pub(crate) const MAX_INSTRUCTION_LENGTH: i32 = 15;
	pub(crate) const MAX_OP_COUNT: i32 = 5;
	pub(crate) const NUMBER_OF_CODE_VALUES: i32 = Code::DeclareQword as i32 + 1;
	pub(crate) const NUMBER_OF_REGISTERS: i32 = Register::TR7 as i32 + 1;
	pub(crate) const NUMBER_OF_MEMORY_SIZES: i32 = MemorySize::Broadcast512_2xBFloat16 as i32 + 1;
	pub(crate) const FIRST_BROADCAST_MEMORY_SIZE: MemorySize = MemorySize::Broadcast64_UInt32;
}
// GENERATOR-END: IcedConstants
