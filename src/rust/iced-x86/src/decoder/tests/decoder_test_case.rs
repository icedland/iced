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

pub(crate) struct DecoderTestCase {
	pub(crate) line_number: i32,
	pub(crate) decoder_options: u32,
	pub(crate) bitness: i32,
	pub(crate) hex_bytes: String,
	pub(crate) encoded_hex_bytes: String,
	pub(crate) code: Code,
	pub(crate) mnemonic: Mnemonic,
	pub(crate) op_count: i32,
	pub(crate) zeroing_masking: bool,
	pub(crate) suppress_all_exceptions: bool,
	pub(crate) is_broadcast: bool,
	pub(crate) has_xacquire_prefix: bool,
	pub(crate) has_xrelease_prefix: bool,
	pub(crate) has_repe_prefix: bool,
	pub(crate) has_repne_prefix: bool,
	pub(crate) has_lock_prefix: bool,
	pub(crate) vsib_bitness: i32,
	pub(crate) op_mask: Register,
	pub(crate) rounding_control: RoundingControl,
	pub(crate) op0_kind: OpKind,
	pub(crate) op1_kind: OpKind,
	pub(crate) op2_kind: OpKind,
	pub(crate) op3_kind: OpKind,
	pub(crate) op4_kind: OpKind,
	pub(crate) segment_prefix: Register,
	pub(crate) memory_segment: Register,
	pub(crate) memory_base: Register,
	pub(crate) memory_index: Register,
	pub(crate) memory_displ_size: i32,
	pub(crate) memory_size: MemorySize,
	pub(crate) memory_index_scale: i32,
	pub(crate) memory_displacement: u32,
	pub(crate) immediate: u64,
	pub(crate) immediate_2nd: u8,
	pub(crate) memory_address64: u64,
	pub(crate) near_branch: u64,
	pub(crate) far_branch: u32,
	pub(crate) far_branch_selector: u16,
	pub(crate) op0_register: Register,
	pub(crate) op1_register: Register,
	pub(crate) op2_register: Register,
	pub(crate) op3_register: Register,
	pub(crate) op4_register: Register,
	pub(crate) constant_offsets: ConstantOffsets,
}

impl Default for DecoderTestCase {
	fn default() -> Self {
		DecoderTestCase {
			line_number: 0,
			decoder_options: 0,
			bitness: 0,
			hex_bytes: Default::default(),
			encoded_hex_bytes: Default::default(),
			code: Code::INVALID,
			mnemonic: Mnemonic::INVALID,
			op_count: 0,
			zeroing_masking: false,
			suppress_all_exceptions: false,
			is_broadcast: false,
			has_xacquire_prefix: false,
			has_xrelease_prefix: false,
			has_repe_prefix: false,
			has_repne_prefix: false,
			has_lock_prefix: false,
			vsib_bitness: 0,
			op_mask: Register::None,
			rounding_control: RoundingControl::None,
			op0_kind: OpKind::Register,
			op1_kind: OpKind::Register,
			op2_kind: OpKind::Register,
			op3_kind: OpKind::Register,
			op4_kind: OpKind::Register,
			segment_prefix: Register::None,
			memory_segment: Register::None,
			memory_base: Register::None,
			memory_index: Register::None,
			memory_displ_size: 0,
			memory_size: MemorySize::Unknown,
			memory_index_scale: 0,
			memory_displacement: 0,
			immediate: 0,
			immediate_2nd: 0,
			memory_address64: 0,
			near_branch: 0,
			far_branch: 0,
			far_branch_selector: 0,
			op0_register: Register::None,
			op1_register: Register::None,
			op2_register: Register::None,
			op3_register: Register::None,
			op4_register: Register::None,
			constant_offsets: Default::default(),
		}
	}
}

impl DecoderTestCase {
	pub fn op_kind(&self, operand: i32) -> OpKind {
		match operand {
			0 => self.op0_kind,
			1 => self.op1_kind,
			2 => self.op2_kind,
			3 => self.op3_kind,
			4 => self.op4_kind,
			_ => panic!(),
		}
	}

	pub fn set_op_kind(&mut self, operand: i32, op_kind: OpKind) {
		match operand {
			0 => self.op0_kind = op_kind,
			1 => self.op1_kind = op_kind,
			2 => self.op2_kind = op_kind,
			3 => self.op3_kind = op_kind,
			4 => self.op4_kind = op_kind,
			_ => panic!(),
		}
	}

	pub fn op_register(&self, operand: i32) -> Register {
		match operand {
			0 => self.op0_register,
			1 => self.op1_register,
			2 => self.op2_register,
			3 => self.op3_register,
			4 => self.op4_register,
			_ => panic!(),
		}
	}

	pub fn set_op_register(&mut self, operand: i32, register: Register) {
		match operand {
			0 => self.op0_register = register,
			1 => self.op1_register = register,
			2 => self.op2_register = register,
			3 => self.op3_register = register,
			4 => self.op4_register = register,
			_ => panic!(),
		}
	}
}
