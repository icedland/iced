// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::*;
use alloc::string::String;
use alloc::vec::Vec;

#[derive(Default)]
pub(super) struct InstrInfoTestCase {
	#[allow(dead_code)]
	pub(super) line_number: u32,
	pub(super) bitness: u32,
	pub(super) hex_bytes: String,
	pub(super) ip: u64,
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
	pub(super) fpu_top_increment: i32,
	pub(super) fpu_conditional_top: bool,
	pub(super) fpu_writes_top: bool,
}
