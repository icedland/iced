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
pub(crate) struct OpCodeInfoTestCase {
	pub(crate) line_number: u32,
	pub(crate) code: Code,
	pub(crate) op_code_string: String,
	pub(crate) instruction_string: String,
	pub(crate) encoding: EncodingKind,
	pub(crate) is_instruction: bool,
	pub(crate) mode16: bool,
	pub(crate) mode32: bool,
	pub(crate) mode64: bool,
	pub(crate) fwait: bool,
	pub(crate) operand_size: u32,
	pub(crate) address_size: u32,
	pub(crate) l: u32,
	pub(crate) w: u32,
	pub(crate) is_lig: bool,
	pub(crate) is_wig: bool,
	pub(crate) is_wig32: bool,
	pub(crate) tuple_type: TupleType,
	pub(crate) can_broadcast: bool,
	pub(crate) can_use_rounding_control: bool,
	pub(crate) can_suppress_all_exceptions: bool,
	pub(crate) can_use_op_mask_register: bool,
	pub(crate) require_non_zero_op_mask_register: bool,
	pub(crate) can_use_zeroing_masking: bool,
	pub(crate) can_use_lock_prefix: bool,
	pub(crate) can_use_xacquire_prefix: bool,
	pub(crate) can_use_xrelease_prefix: bool,
	pub(crate) can_use_rep_prefix: bool,
	pub(crate) can_use_repne_prefix: bool,
	pub(crate) can_use_bnd_prefix: bool,
	pub(crate) can_use_hint_taken_prefix: bool,
	pub(crate) can_use_notrack_prefix: bool,
	pub(crate) table: OpCodeTableKind,
	pub(crate) mandatory_prefix: MandatoryPrefix,
	pub(crate) op_code: u32,
	pub(crate) is_group: bool,
	pub(crate) group_index: i32,
	pub(crate) op_count: usize,
	pub(crate) op0_kind: OpCodeOperandKind,
	pub(crate) op1_kind: OpCodeOperandKind,
	pub(crate) op2_kind: OpCodeOperandKind,
	pub(crate) op3_kind: OpCodeOperandKind,
	pub(crate) op4_kind: OpCodeOperandKind,
}
