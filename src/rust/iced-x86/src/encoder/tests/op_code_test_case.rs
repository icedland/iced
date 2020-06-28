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

#[derive(Default)]
pub(super) struct OpCodeInfoTestCase {
	pub(super) line_number: u32,
	pub(super) code: Code,
	pub(super) op_code_string: String,
	pub(super) instruction_string: String,
	pub(super) encoding: EncodingKind,
	pub(super) is_instruction: bool,
	pub(super) mode16: bool,
	pub(super) mode32: bool,
	pub(super) mode64: bool,
	pub(super) fwait: bool,
	pub(super) operand_size: u32,
	pub(super) address_size: u32,
	pub(super) l: u32,
	pub(super) w: u32,
	pub(super) is_lig: bool,
	pub(super) is_wig: bool,
	pub(super) is_wig32: bool,
	pub(super) tuple_type: TupleType,
	pub(super) can_broadcast: bool,
	pub(super) can_use_rounding_control: bool,
	pub(super) can_suppress_all_exceptions: bool,
	pub(super) can_use_op_mask_register: bool,
	pub(super) require_non_zero_op_mask_register: bool,
	pub(super) can_use_zeroing_masking: bool,
	pub(super) can_use_lock_prefix: bool,
	pub(super) can_use_xacquire_prefix: bool,
	pub(super) can_use_xrelease_prefix: bool,
	pub(super) can_use_rep_prefix: bool,
	pub(super) can_use_repne_prefix: bool,
	pub(super) can_use_bnd_prefix: bool,
	pub(super) can_use_hint_taken_prefix: bool,
	pub(super) can_use_notrack_prefix: bool,
	pub(super) table: OpCodeTableKind,
	pub(super) mandatory_prefix: MandatoryPrefix,
	pub(super) op_code: u32,
	pub(super) is_group: bool,
	pub(super) group_index: i32,
	pub(super) is_rm_group: bool,
	pub(super) rm_group_index: i32,
	pub(super) op_count: u32,
	pub(super) op0_kind: OpCodeOperandKind,
	pub(super) op1_kind: OpCodeOperandKind,
	pub(super) op2_kind: OpCodeOperandKind,
	pub(super) op3_kind: OpCodeOperandKind,
	pub(super) op4_kind: OpCodeOperandKind,
}
