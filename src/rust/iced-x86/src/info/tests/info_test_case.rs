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

use super::super::super::*;
#[cfg(not(feature = "std"))]
use alloc::string::String;
#[cfg(not(feature = "std"))]
use alloc::vec::Vec;

#[derive(Default)]
pub(super) struct InstrInfoTestCase {
	pub(super) line_number: u32,
	pub(super) bitness: u32,
	pub(super) hex_bytes: String,
	pub(super) code: Code,
	pub(super) decoder_options: u32,
	pub(super) encoding: EncodingKind,
	pub(super) cpuid_features: Vec<CpuidFeature>,
	pub(super) rflags_read: u32,
	pub(super) rflags_undefined: u32,
	pub(super) rflags_written: u32,
	pub(super) rflags_cleared: u32,
	pub(super) rflags_set: u32,
	pub(super) stack_pointer_increment: i32,
	pub(super) is_privileged: bool,
	pub(super) is_stack_instruction: bool,
	pub(super) is_save_restore_instruction: bool,
	pub(super) is_special: bool,
	pub(super) used_registers: Vec<UsedRegister>,
	pub(super) used_memory: Vec<UsedMemory>,
	pub(super) flow_control: FlowControl,
	pub(super) op0_access: OpAccess,
	pub(super) op1_access: OpAccess,
	pub(super) op2_access: OpAccess,
	pub(super) op3_access: OpAccess,
	pub(super) op4_access: OpAccess,
}
