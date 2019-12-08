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

#[derive(Default)]
pub(crate) struct InstrInfoTestCase {
	pub(crate) line_number: u32,
	pub(crate) bitness: u32,
	pub(crate) hex_bytes: String,
	pub(crate) code: Code,
	pub(crate) decoder_options: u32,
	pub(crate) encoding: EncodingKind,
	pub(crate) cpuid_features: Vec<CpuidFeature>,
	pub(crate) rflags_read: u32,
	pub(crate) rflags_undefined: u32,
	pub(crate) rflags_written: u32,
	pub(crate) rflags_cleared: u32,
	pub(crate) rflags_set: u32,
	pub(crate) stack_pointer_increment: i32,
	pub(crate) is_privileged: bool,
	pub(crate) is_protected_mode: bool,
	pub(crate) is_stack_instruction: bool,
	pub(crate) is_save_restore_instruction: bool,
	pub(crate) is_special: bool,
	pub(crate) used_registers: Vec<UsedRegister>,
	pub(crate) used_memory: Vec<UsedMemory>,
	pub(crate) flow_control: FlowControl,
	pub(crate) op0_access: OpAccess,
	pub(crate) op1_access: OpAccess,
	pub(crate) op2_access: OpAccess,
	pub(crate) op3_access: OpAccess,
	pub(crate) op4_access: OpAccess,
}
