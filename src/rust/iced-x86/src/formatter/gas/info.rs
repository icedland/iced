// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::formatter::gas::enums::*;
use crate::formatter::gas::fmt_utils::can_show_rounding_control;
use crate::formatter::gas::get_mnemonic_cc;
use crate::formatter::gas::mem_size_tbl::MEM_SIZE_TBL;
use crate::formatter::FormatterString;
use crate::formatter::{r64_to_r32, r_to_r16, REGISTER_ST};
use crate::iced_constants::IcedConstants;
use crate::*;
use alloc::string::String;
use alloc::vec::Vec;
use core::mem;

#[derive(Debug)]
pub(super) struct InstrOpInfo<'a> {
	pub(super) mnemonic: &'a FormatterString,
	pub(super) flags: u16, // InstrOpInfoFlags
	pub(super) op_count: u8,
	op_kinds: [InstrOpKind; IcedConstants::MAX_OP_COUNT],
	op_registers: [Register; IcedConstants::MAX_OP_COUNT],
	op_indexes: [i8; IcedConstants::MAX_OP_COUNT],
}

impl<'a> InstrOpInfo<'a> {
	fn to_instr_op_kind(op_kind: OpKind) -> InstrOpKind {
		// SAFETY: All OpKind values are valid InstrOpKind values
		unsafe { mem::transmute(op_kind as u8) }
	}

	pub(super) const fn op_register(&self, operand: u32) -> Register {
		self.op_registers[operand as usize]
	}

	pub(super) fn op_kind(&self, operand: u32) -> InstrOpKind {
		if let Some(op_kind) = self.op_kinds.get(operand as usize) {
			*op_kind
		} else {
			debug_assert!(
				self.op_kinds[0] == InstrOpKind::DeclareByte
					|| self.op_kinds[0] == InstrOpKind::DeclareWord
					|| self.op_kinds[0] == InstrOpKind::DeclareDword
					|| self.op_kinds[0] == InstrOpKind::DeclareQword
			);
			self.op_kinds[0]
		}
	}

	pub(super) fn instruction_index(&self, operand: u32) -> Option<u32> {
		let instruction_operand = if let Some(operand) = self.op_indexes.get(operand as usize) {
			*operand
		} else {
			debug_assert!(
				self.op_kinds[0] == InstrOpKind::DeclareByte
					|| self.op_kinds[0] == InstrOpKind::DeclareWord
					|| self.op_kinds[0] == InstrOpKind::DeclareDword
					|| self.op_kinds[0] == InstrOpKind::DeclareQword
			);
			-1
		};
		if instruction_operand < 0 {
			None
		} else {
			Some(instruction_operand as u32)
		}
	}

	#[cfg(feature = "instr_info")]
	pub(super) fn op_access(&self, operand: u32) -> Option<OpAccess> {
		let instruction_operand = match operand {
			0 => self.op_indexes[0],
			1 => self.op_indexes[1],
			2 => self.op_indexes[2],
			3 => self.op_indexes[3],
			4 => self.op_indexes[4],
			_ => {
				debug_assert!(
					self.op_kinds[0] == InstrOpKind::DeclareByte
						|| self.op_kinds[0] == InstrOpKind::DeclareWord
						|| self.op_kinds[0] == InstrOpKind::DeclareDword
						|| self.op_kinds[0] == InstrOpKind::DeclareQword
				);
				self.op_indexes[0]
			}
		};
		if instruction_operand < OP_ACCESS_INVALID {
			Some(unsafe { mem::transmute(-instruction_operand - 2) })
		} else {
			None
		}
	}

	pub(super) const fn operand_index(&self, instruction_operand: u32) -> Option<u32> {
		let index: i32 = if instruction_operand == self.op_indexes[0] as u32 {
			0
		} else if instruction_operand == self.op_indexes[1] as u32 {
			1
		} else if instruction_operand == self.op_indexes[2] as u32 {
			2
		} else if instruction_operand == self.op_indexes[3] as u32 {
			3
		} else if instruction_operand == self.op_indexes[4] as u32 {
			4
		} else {
			-1
		};
		if (index as u32) < self.op_count as u32 {
			Some(index as u32)
		} else {
			None
		}
	}

	#[inline]
	fn default(mnemonic: &'a FormatterString) -> Self {
		Self {
			mnemonic,
			flags: 0,
			op_count: 0,
			op_kinds: [InstrOpKind::default(); IcedConstants::MAX_OP_COUNT],
			op_registers: [Register::None; IcedConstants::MAX_OP_COUNT],
			op_indexes: [0; IcedConstants::MAX_OP_COUNT],
		}
	}

	fn new(mnemonic: &'a FormatterString, instruction: &Instruction, flags: u32) -> Self {
		let mut res = InstrOpInfo::default(mnemonic);

		const _: () = assert!(IcedConstants::MAX_OP_COUNT == 5);
		res.flags = flags as u16;
		let op_count = instruction.op_count();
		res.op_count = op_count as u8;
		if (flags & InstrOpInfoFlags::KEEP_OPERAND_ORDER) != 0 {
			res.op_kinds[0] = InstrOpInfo::to_instr_op_kind(instruction.op0_kind());
			res.op_kinds[1] = InstrOpInfo::to_instr_op_kind(instruction.op1_kind());
			res.op_kinds[2] = InstrOpInfo::to_instr_op_kind(instruction.op2_kind());
			res.op_kinds[3] = InstrOpInfo::to_instr_op_kind(instruction.op3_kind());
			res.op_kinds[4] = InstrOpInfo::to_instr_op_kind(instruction.op4_kind());
			res.op_registers[0] = instruction.op0_register();
			res.op_registers[1] = instruction.op1_register();
			res.op_registers[2] = instruction.op2_register();
			res.op_registers[3] = instruction.op3_register();
			res.op_registers[4] = instruction.op4_register();
		} else {
			match op_count {
				0 => {}

				1 => {
					res.op_kinds[0] = InstrOpInfo::to_instr_op_kind(instruction.op0_kind());
					res.op_registers[0] = instruction.op0_register();
				}

				2 => {
					res.op_kinds[0] = InstrOpInfo::to_instr_op_kind(instruction.op1_kind());
					res.op_kinds[1] = InstrOpInfo::to_instr_op_kind(instruction.op0_kind());
					res.op_registers[0] = instruction.op1_register();
					res.op_registers[1] = instruction.op0_register();
				}

				3 => {
					res.op_kinds[0] = InstrOpInfo::to_instr_op_kind(instruction.op2_kind());
					res.op_kinds[1] = InstrOpInfo::to_instr_op_kind(instruction.op1_kind());
					res.op_kinds[2] = InstrOpInfo::to_instr_op_kind(instruction.op0_kind());
					res.op_registers[0] = instruction.op2_register();
					res.op_registers[1] = instruction.op1_register();
					res.op_registers[2] = instruction.op0_register();
				}

				4 => {
					res.op_kinds[0] = InstrOpInfo::to_instr_op_kind(instruction.op3_kind());
					res.op_kinds[1] = InstrOpInfo::to_instr_op_kind(instruction.op2_kind());
					res.op_kinds[2] = InstrOpInfo::to_instr_op_kind(instruction.op1_kind());
					res.op_kinds[3] = InstrOpInfo::to_instr_op_kind(instruction.op0_kind());
					res.op_registers[0] = instruction.op3_register();
					res.op_registers[1] = instruction.op2_register();
					res.op_registers[2] = instruction.op1_register();
					res.op_registers[3] = instruction.op0_register();
				}

				5 => {
					res.op_kinds[0] = InstrOpInfo::to_instr_op_kind(instruction.op4_kind());
					res.op_kinds[1] = InstrOpInfo::to_instr_op_kind(instruction.op3_kind());
					res.op_kinds[2] = InstrOpInfo::to_instr_op_kind(instruction.op2_kind());
					res.op_kinds[3] = InstrOpInfo::to_instr_op_kind(instruction.op1_kind());
					res.op_kinds[4] = InstrOpInfo::to_instr_op_kind(instruction.op0_kind());
					res.op_registers[0] = instruction.op4_register();
					res.op_registers[1] = instruction.op3_register();
					res.op_registers[2] = instruction.op2_register();
					res.op_registers[3] = instruction.op1_register();
					res.op_registers[4] = instruction.op0_register();
				}

				_ => unreachable!(),
			}
		}
		match res.op_count {
			0 => {
				res.op_indexes[0] = OP_ACCESS_INVALID;
				res.op_indexes[1] = OP_ACCESS_INVALID;
				res.op_indexes[2] = OP_ACCESS_INVALID;
				res.op_indexes[3] = OP_ACCESS_INVALID;
				res.op_indexes[4] = OP_ACCESS_INVALID;
			}

			1 => {
				res.op_indexes[1] = OP_ACCESS_INVALID;
				res.op_indexes[2] = OP_ACCESS_INVALID;
				res.op_indexes[3] = OP_ACCESS_INVALID;
				res.op_indexes[4] = OP_ACCESS_INVALID;
			}

			2 => {
				res.op_indexes[0] = 1;
				res.op_indexes[2] = OP_ACCESS_INVALID;
				res.op_indexes[3] = OP_ACCESS_INVALID;
				res.op_indexes[4] = OP_ACCESS_INVALID;
			}

			3 => {
				res.op_indexes[0] = 2;
				res.op_indexes[1] = 1;
				res.op_indexes[3] = OP_ACCESS_INVALID;
				res.op_indexes[4] = OP_ACCESS_INVALID;
			}

			4 => {
				res.op_indexes[0] = 3;
				res.op_indexes[1] = 2;
				res.op_indexes[2] = 1;
				res.op_indexes[4] = OP_ACCESS_INVALID;
			}

			5 => {
				res.op_indexes[0] = 4;
				res.op_indexes[1] = 3;
				res.op_indexes[2] = 2;
				res.op_indexes[3] = 1;
			}

			_ => unreachable!(),
		}

		res
	}
}

const OP_ACCESS_INVALID: i8 = -1;
struct InstrInfoConstants;
#[cfg(feature = "instr_info")]
impl InstrInfoConstants {
	pub const OP_ACCESS_NONE: i8 = -(OpAccess::None as i8 + 2);
	pub const OP_ACCESS_READ: i8 = -(OpAccess::Read as i8 + 2);
	pub const OP_ACCESS_READ_WRITE: i8 = -(OpAccess::ReadWrite as i8 + 2);
}
#[cfg(not(feature = "instr_info"))]
impl InstrInfoConstants {
	pub const OP_ACCESS_NONE: i8 = OP_ACCESS_INVALID;
	pub const OP_ACCESS_READ: i8 = OP_ACCESS_INVALID;
	pub const OP_ACCESS_READ_WRITE: i8 = OP_ACCESS_INVALID;
}

pub(super) trait InstrInfo {
	fn op_info<'a>(&'a self, options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a>;
}

fn get_bitness(code_size: CodeSize) -> u32 {
	static CODESIZE_TO_BITNESS: [u32; 4] = [0, 16, 32, 64];
	const _: () = assert!(CodeSize::Unknown as u32 == 0);
	const _: () = assert!(CodeSize::Code16 as u32 == 1);
	const _: () = assert!(CodeSize::Code32 as u32 == 2);
	const _: () = assert!(CodeSize::Code64 as u32 == 3);
	CODESIZE_TO_BITNESS[code_size as usize]
}

fn get_mnemonic<'a>(
	options: &FormatterOptions, instruction: &Instruction, mnemonic: &'a FormatterString, mnemonic_suffix: &'a FormatterString, flags: u32,
) -> &'a FormatterString {
	if options.gas_show_mnemonic_size_suffix() {
		return mnemonic_suffix;
	}
	if (flags & InstrOpInfoFlags::MNEMONIC_SUFFIX_IF_MEM) != 0 && (&*MEM_SIZE_TBL)[instruction.memory_size() as usize].is_default() {
		if instruction.op0_kind() == OpKind::Memory || instruction.op1_kind() == OpKind::Memory || instruction.op2_kind() == OpKind::Memory {
			return mnemonic_suffix;
		}
	}
	mnemonic
}

pub(super) struct SimpleInstrInfo {
	mnemonic: FormatterString,
	mnemonic_suffix: FormatterString,
	flags: u32,
}

impl SimpleInstrInfo {
	pub(super) fn with_mnemonic(mnemonic: String) -> Self {
		SimpleInstrInfo::new(mnemonic.clone(), mnemonic, InstrOpInfoFlags::NONE)
	}
	pub(super) fn with_mnemonic_flags(mnemonic: String, flags: u32) -> Self {
		SimpleInstrInfo::new(mnemonic.clone(), mnemonic, flags)
	}
	pub(super) fn with_mnemonic_suffix(mnemonic: String, mnemonic_suffix: String) -> Self {
		SimpleInstrInfo::new(mnemonic, mnemonic_suffix, InstrOpInfoFlags::NONE)
	}
	pub(super) fn new(mnemonic: String, mnemonic_suffix: String, flags: u32) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), mnemonic_suffix: FormatterString::new(mnemonic_suffix), flags }
	}
}

impl InstrInfo for SimpleInstrInfo {
	fn op_info<'a>(&'a self, options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		InstrOpInfo::new(get_mnemonic(options, instruction, &self.mnemonic, &self.mnemonic_suffix, self.flags), instruction, self.flags)
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_cc {
	mnemonics: Vec<FormatterString>,
	mnemonics_suffix: Vec<FormatterString>,
	cc_index: u32,
}

impl SimpleInstrInfo_cc {
	pub(super) fn new(cc_index: u32, mnemonics: Vec<String>, mnemonics_suffix: Vec<String>) -> Self {
		SimpleInstrInfo_cc {
			mnemonics: FormatterString::with_strings(mnemonics),
			mnemonics_suffix: FormatterString::with_strings(mnemonics_suffix),
			cc_index,
		}
	}
}

impl InstrInfo for SimpleInstrInfo_cc {
	fn op_info<'a>(&'a self, options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		const FLAGS: u32 = InstrOpInfoFlags::NONE;
		let mnemonic = get_mnemonic_cc(options, self.cc_index, &self.mnemonics);
		let mnemonic_suffix = get_mnemonic_cc(options, self.cc_index, &self.mnemonics_suffix);
		InstrOpInfo::new(get_mnemonic(options, instruction, mnemonic, mnemonic_suffix, FLAGS), instruction, FLAGS)
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_AamAad {
	mnemonic: FormatterString,
}

impl SimpleInstrInfo_AamAad {
	pub(super) fn new(mnemonic: String) -> Self {
		SimpleInstrInfo_AamAad { mnemonic: FormatterString::new(mnemonic) }
	}
}

impl InstrInfo for SimpleInstrInfo_AamAad {
	fn op_info<'a>(&'a self, _options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		if instruction.immediate8() == 10 {
			InstrOpInfo::default(&self.mnemonic)
		} else {
			InstrOpInfo::new(&self.mnemonic, instruction, InstrOpInfoFlags::NONE)
		}
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_nop {
	mnemonic: FormatterString,
	bitness: u32,
	register: Register,
	str_xchg: FormatterString,
	str_xchgw: FormatterString,
	str_xchgl: FormatterString,
	str_xchgq: FormatterString,
}

impl SimpleInstrInfo_nop {
	pub(super) fn new(bitness: u32, mnemonic: String, register: Register) -> Self {
		Self {
			mnemonic: FormatterString::new(mnemonic),
			bitness,
			register,
			str_xchg: FormatterString::new_str("xchg"),
			str_xchgw: FormatterString::new_str("xchgw"),
			str_xchgl: FormatterString::new_str("xchgl"),
			str_xchgq: FormatterString::new_str("xchgq"),
		}
	}
}

impl InstrInfo for SimpleInstrInfo_nop {
	fn op_info<'a>(&'a self, options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let instr_bitness = get_bitness(instruction.code_size());
		let mut info;
		if instr_bitness == 0 || (instr_bitness & self.bitness) != 0 {
			info = InstrOpInfo::new(&self.mnemonic, instruction, InstrOpInfoFlags::NONE);
		} else {
			info = InstrOpInfo::default(if !options.gas_show_mnemonic_size_suffix() {
				&self.str_xchg
			} else if self.register == Register::AX {
				&self.str_xchgw
			} else if self.register == Register::EAX {
				&self.str_xchgl
			} else if self.register == Register::RAX {
				&self.str_xchgq
			} else {
				unreachable!()
			});
			info.op_count = 2;
			const _: () = assert!(InstrOpKind::Register as u32 == 0);
			//info.op_kinds[0] = InstrOpKind::Register;
			//info.op_kinds[1] = InstrOpKind::Register;
			info.op_registers[0] = self.register;
			info.op_registers[1] = self.register;
			info.op_indexes[0] = InstrInfoConstants::OP_ACCESS_NONE;
			info.op_indexes[1] = InstrInfoConstants::OP_ACCESS_NONE;
		}
		info
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_STIG1 {
	mnemonic: FormatterString,
	pseudo_op: bool,
}

impl SimpleInstrInfo_STIG1 {
	pub(super) fn new(mnemonic: String, pseudo_op: bool) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), pseudo_op }
	}
}

impl InstrInfo for SimpleInstrInfo_STIG1 {
	#[allow(clippy::nonminimal_bool)]
	fn op_info<'a>(&'a self, options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut info = InstrOpInfo::default(&self.mnemonic);
		debug_assert_eq!(instruction.op_count(), 2);
		debug_assert!(instruction.op0_kind() == OpKind::Register && instruction.op0_register() == Register::ST0);
		if !self.pseudo_op || !(options.use_pseudo_ops() && instruction.op1_register() == Register::ST1) {
			info.op_count = 1;
			const _: () = assert!(InstrOpKind::Register as u32 == 0);
			//info.op_kinds[0] = InstrOpKind::Register;
			info.op_registers[0] = instruction.op1_register();
			info.op_indexes[0] = 1;
		}
		info
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_STi_ST {
	mnemonic: FormatterString,
	pseudo_op: bool,
}

impl SimpleInstrInfo_STi_ST {
	pub(super) fn new(mnemonic: String, pseudo_op: bool) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), pseudo_op }
	}
}

impl InstrInfo for SimpleInstrInfo_STi_ST {
	fn op_info<'a>(&'a self, options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		const FLAGS: u32 = InstrOpInfoFlags::NONE;
		let mut info;
		if self.pseudo_op && options.use_pseudo_ops() && (instruction.op0_register() == Register::ST1 || instruction.op1_register() == Register::ST1)
		{
			info = InstrOpInfo::default(&self.mnemonic);
		} else {
			info = InstrOpInfo::new(&self.mnemonic, instruction, FLAGS);
			debug_assert_eq!(info.op_registers[0] as u32, Register::ST0 as u32);
			info.op_registers[0] = REGISTER_ST;
		}
		info
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_ST_STi {
	mnemonic: FormatterString,
}

impl SimpleInstrInfo_ST_STi {
	pub(super) fn new(mnemonic: String) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic) }
	}
}

impl InstrInfo for SimpleInstrInfo_ST_STi {
	fn op_info<'a>(&'a self, _options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut info = InstrOpInfo::new(&self.mnemonic, instruction, InstrOpInfoFlags::NONE);
		debug_assert_eq!(info.op_registers[1] as u32, Register::ST0 as u32);
		info.op_registers[1] = REGISTER_ST;
		info
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_as {
	mnemonic: FormatterString,
	bitness: u32,
}

impl SimpleInstrInfo_as {
	pub(super) fn new(bitness: u32, mnemonic: String) -> Self {
		Self { bitness, mnemonic: FormatterString::new(mnemonic) }
	}
}

impl InstrInfo for SimpleInstrInfo_as {
	fn op_info<'a>(&'a self, _options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut flags = 0;
		let instr_bitness = get_bitness(instruction.code_size());
		if instr_bitness != 0 && instr_bitness != self.bitness {
			if self.bitness == 16 {
				flags |= InstrOpInfoFlags::ADDR_SIZE16;
			} else if self.bitness == 32 {
				flags |= InstrOpInfoFlags::ADDR_SIZE32;
			} else {
				flags |= InstrOpInfoFlags::ADDR_SIZE64;
			}
		}
		InstrOpInfo::new(&self.mnemonic, instruction, flags)
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_maskmovq {
	mnemonic: FormatterString,
}

impl SimpleInstrInfo_maskmovq {
	pub(super) fn new(mnemonic: String) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic) }
	}
}

impl InstrInfo for SimpleInstrInfo_maskmovq {
	fn op_info<'a>(&'a self, _options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		debug_assert_eq!(instruction.op_count(), 3);

		let instr_bitness = get_bitness(instruction.code_size());

		let bitness = match instruction.op0_kind() {
			OpKind::MemorySegDI => 16,
			OpKind::MemorySegEDI => 32,
			OpKind::MemorySegRDI => 64,
			_ => instr_bitness,
		};

		let mut info = InstrOpInfo::default(&self.mnemonic);
		info.op_count = 2;
		info.op_kinds[0] = InstrOpInfo::to_instr_op_kind(instruction.op2_kind());
		info.op_registers[0] = instruction.op2_register();
		info.op_indexes[0] = 2;
		info.op_kinds[1] = InstrOpInfo::to_instr_op_kind(instruction.op1_kind());
		info.op_registers[1] = instruction.op1_register();
		info.op_indexes[1] = 1;
		if instr_bitness != 0 && instr_bitness != bitness {
			if bitness == 16 {
				info.flags |= InstrOpInfoFlags::ADDR_SIZE16 as u16;
			} else if bitness == 32 {
				info.flags |= InstrOpInfoFlags::ADDR_SIZE32 as u16;
			} else {
				info.flags |= InstrOpInfoFlags::ADDR_SIZE64 as u16;
			}
		}
		info
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_pblendvb {
	mnemonic: FormatterString,
}

impl SimpleInstrInfo_pblendvb {
	pub(super) fn new(mnemonic: String) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic) }
	}
}

impl InstrInfo for SimpleInstrInfo_pblendvb {
	fn op_info<'a>(&'a self, _options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut info = InstrOpInfo::default(&self.mnemonic);
		debug_assert_eq!(instruction.op_count(), 2);
		info.op_count = 3;
		const _: () = assert!(InstrOpKind::Register as u32 == 0);
		//info.op_kinds[0] = InstrOpKind::Register;
		info.op_registers[0] = Register::XMM0;
		info.op_indexes[0] = InstrInfoConstants::OP_ACCESS_READ;
		info.op_kinds[1] = InstrOpInfo::to_instr_op_kind(instruction.op1_kind());
		info.op_indexes[1] = 1;
		info.op_registers[1] = instruction.op1_register();
		info.op_kinds[2] = InstrOpInfo::to_instr_op_kind(instruction.op0_kind());
		info.op_registers[2] = instruction.op0_register();
		info
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_OpSize {
	mnemonics: [FormatterString; 4],
	code_size: CodeSize,
}

impl SimpleInstrInfo_OpSize {
	pub(super) fn new(code_size: CodeSize, mnemonic: String, mnemonic16: String, mnemonic32: String, mnemonic64: String) -> Self {
		const _: () = assert!(CodeSize::Unknown as u32 == 0);
		const _: () = assert!(CodeSize::Code16 as u32 == 1);
		const _: () = assert!(CodeSize::Code32 as u32 == 2);
		const _: () = assert!(CodeSize::Code64 as u32 == 3);
		Self {
			mnemonics: [
				FormatterString::new(mnemonic),
				FormatterString::new(mnemonic16),
				FormatterString::new(mnemonic32),
				FormatterString::new(mnemonic64),
			],
			code_size,
		}
	}
}

impl InstrInfo for SimpleInstrInfo_OpSize {
	fn op_info<'a>(&'a self, options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mnemonic = if instruction.code_size() == self.code_size && !options.gas_show_mnemonic_size_suffix() {
			&self.mnemonics[CodeSize::Unknown as usize]
		} else {
			&self.mnemonics[self.code_size as usize]
		};
		InstrOpInfo::new(mnemonic, instruction, InstrOpInfoFlags::NONE)
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_OpSize2_bnd {
	mnemonics: [FormatterString; 4],
}

impl SimpleInstrInfo_OpSize2_bnd {
	pub(super) fn new(mnemonic: String, mnemonic16: String, mnemonic32: String, mnemonic64: String) -> Self {
		const _: () = assert!(CodeSize::Unknown as u32 == 0);
		const _: () = assert!(CodeSize::Code16 as u32 == 1);
		const _: () = assert!(CodeSize::Code32 as u32 == 2);
		const _: () = assert!(CodeSize::Code64 as u32 == 3);
		Self {
			mnemonics: [
				FormatterString::new(mnemonic),
				FormatterString::new(mnemonic16),
				FormatterString::new(mnemonic32),
				FormatterString::new(mnemonic64),
			],
		}
	}
}

impl InstrInfo for SimpleInstrInfo_OpSize2_bnd {
	fn op_info<'a>(&'a self, options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut flags = InstrOpInfoFlags::NONE;
		if instruction.has_repne_prefix() {
			flags |= InstrOpInfoFlags::BND_PREFIX;
		}
		let mnemonic = if options.gas_show_mnemonic_size_suffix() {
			&self.mnemonics[CodeSize::Code64 as usize]
		} else {
			&self.mnemonics[instruction.code_size() as usize]
		};
		InstrOpInfo::new(mnemonic, instruction, flags)
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_OpSize3 {
	mnemonic: FormatterString,
	mnemonic_suffix: FormatterString,
	bitness: u32,
}

impl SimpleInstrInfo_OpSize3 {
	pub(super) fn new(bitness: u32, mnemonic: String, mnemonic_suffix: String) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), mnemonic_suffix: FormatterString::new(mnemonic_suffix), bitness }
	}
}

impl InstrInfo for SimpleInstrInfo_OpSize3 {
	fn op_info<'a>(&'a self, options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let instr_bitness = get_bitness(instruction.code_size());
		let mnemonic = if !options.gas_show_mnemonic_size_suffix() && (instr_bitness == 0 || (instr_bitness & self.bitness) != 0) {
			&self.mnemonic
		} else {
			&self.mnemonic_suffix
		};
		InstrOpInfo::new(mnemonic, instruction, InstrOpInfoFlags::NONE)
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_os2 {
	mnemonic: FormatterString,
	mnemonic_suffix: FormatterString,
	bitness: u32,
	flags: u32,
	can_use_bnd: bool,
}

impl SimpleInstrInfo_os2 {
	pub(super) fn new(bitness: u32, mnemonic: String, mnemonic_suffix: String, can_use_bnd: bool, flags: u32) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), mnemonic_suffix: FormatterString::new(mnemonic_suffix), bitness, flags, can_use_bnd }
	}
}

impl InstrInfo for SimpleInstrInfo_os2 {
	fn op_info<'a>(&'a self, options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut flags = self.flags;
		if self.can_use_bnd && instruction.has_repne_prefix() {
			flags |= InstrOpInfoFlags::BND_PREFIX;
		}
		let instr_bitness = get_bitness(instruction.code_size());
		let mnemonic = if instr_bitness != 0 && instr_bitness != self.bitness {
			&self.mnemonic_suffix
		} else {
			get_mnemonic(options, instruction, &self.mnemonic, &self.mnemonic_suffix, flags)
		};
		InstrOpInfo::new(mnemonic, instruction, flags)
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_os {
	mnemonic: FormatterString,
	bitness: u32,
	flags: u32,
	can_use_bnd: bool,
}

impl SimpleInstrInfo_os {
	pub(super) fn new(bitness: u32, mnemonic: String, can_use_bnd: bool, flags: u32) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), bitness, can_use_bnd, flags }
	}
}

impl InstrInfo for SimpleInstrInfo_os {
	fn op_info<'a>(&'a self, options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut flags = self.flags;
		if self.can_use_bnd && instruction.has_repne_prefix() {
			flags |= InstrOpInfoFlags::BND_PREFIX;
		}
		let instr_bitness = get_bitness(instruction.code_size());
		if instr_bitness != 0 && instr_bitness != self.bitness {
			if self.bitness == 16 {
				flags |= InstrOpInfoFlags::OP_SIZE16;
			} else if self.bitness == 32 {
				flags |= InstrOpInfoFlags::OP_SIZE32;
			} else {
				flags |= InstrOpInfoFlags::OP_SIZE64;
			}
		}
		InstrOpInfo::new(get_mnemonic(options, instruction, &self.mnemonic, &self.mnemonic, flags), instruction, flags)
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_os_mem {
	mnemonic: FormatterString,
	mnemonic_suffix: FormatterString,
	bitness: u32,
}

impl SimpleInstrInfo_os_mem {
	pub(super) fn new(bitness: u32, mnemonic: String, mnemonic_suffix: String) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), mnemonic_suffix: FormatterString::new(mnemonic_suffix), bitness }
	}
}

impl InstrInfo for SimpleInstrInfo_os_mem {
	fn op_info<'a>(&'a self, options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut flags = InstrOpInfoFlags::NONE;
		let instr_bitness = get_bitness(instruction.code_size());
		let has_mem_op = instruction.op0_kind() == OpKind::Memory || instruction.op1_kind() == OpKind::Memory;
		if has_mem_op
			&& !(instr_bitness == 0 || (instr_bitness != 64 && instr_bitness == self.bitness) || (instr_bitness == 64 && self.bitness == 32))
		{
			if self.bitness == 16 {
				flags |= InstrOpInfoFlags::OP_SIZE16;
			} else if self.bitness == 32 {
				flags |= InstrOpInfoFlags::OP_SIZE32;
			} else {
				flags |= InstrOpInfoFlags::OP_SIZE64;
			}
		}
		let mnemonic = if has_mem_op { &self.mnemonic } else { get_mnemonic(options, instruction, &self.mnemonic, &self.mnemonic_suffix, flags) };
		InstrOpInfo::new(mnemonic, instruction, flags)
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_os_mem2 {
	mnemonic: FormatterString,
	mnemonic_suffix: FormatterString,
	bitness: u32,
}

impl SimpleInstrInfo_os_mem2 {
	pub(super) fn new(bitness: u32, mnemonic: String, mnemonic_suffix: String) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), mnemonic_suffix: FormatterString::new(mnemonic_suffix), bitness }
	}
}

impl InstrInfo for SimpleInstrInfo_os_mem2 {
	fn op_info<'a>(&'a self, options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		const FLAGS: u32 = InstrOpInfoFlags::NONE;
		let instr_bitness = get_bitness(instruction.code_size());
		let mnemonic = if instr_bitness != 0 && (instr_bitness & self.bitness) == 0 {
			&self.mnemonic_suffix
		} else {
			get_mnemonic(options, instruction, &self.mnemonic, &self.mnemonic_suffix, FLAGS)
		};
		InstrOpInfo::new(mnemonic, instruction, InstrOpInfoFlags::NONE)
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_Reg16 {
	mnemonic: FormatterString,
	mnemonic_suffix: FormatterString,
}

impl SimpleInstrInfo_Reg16 {
	pub(super) fn new(mnemonic: String, mnemonic_suffix: String) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), mnemonic_suffix: FormatterString::new(mnemonic_suffix) }
	}
}

impl InstrInfo for SimpleInstrInfo_Reg16 {
	fn op_info<'a>(&'a self, options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let flags = InstrOpInfoFlags::NONE;
		let mut info = InstrOpInfo::new(get_mnemonic(options, instruction, &self.mnemonic, &self.mnemonic_suffix, flags), instruction, flags);
		info.op_registers[0] = r_to_r16(info.op_registers[0]);
		info.op_registers[1] = r_to_r16(info.op_registers[1]);
		info.op_registers[2] = r_to_r16(info.op_registers[2]);
		info
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_mem16 {
	mnemonic: FormatterString,
	mnemonic_reg_suffix: FormatterString,
	mnemonic_mem_suffix: FormatterString,
}

impl SimpleInstrInfo_mem16 {
	pub(super) fn new(mnemonic: String, mnemonic_reg_suffix: String, mnemonic_mem_suffix: String) -> Self {
		Self {
			mnemonic: FormatterString::new(mnemonic),
			mnemonic_reg_suffix: FormatterString::new(mnemonic_reg_suffix),
			mnemonic_mem_suffix: FormatterString::new(mnemonic_mem_suffix),
		}
	}
}

impl InstrInfo for SimpleInstrInfo_mem16 {
	fn op_info<'a>(&'a self, options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		const FLAGS: u32 = InstrOpInfoFlags::NONE;
		let mnemonic_suffix = if instruction.op0_kind() == OpKind::Memory || instruction.op1_kind() == OpKind::Memory {
			&self.mnemonic_mem_suffix
		} else {
			&self.mnemonic_reg_suffix
		};
		InstrOpInfo::new(get_mnemonic(options, instruction, &self.mnemonic, mnemonic_suffix, FLAGS), instruction, FLAGS)
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_os_loop {
	mnemonics: Vec<FormatterString>,
	mnemonics_suffix: Vec<FormatterString>,
	bitness: u32,
	cc_index: u32,
	reg_size: u32,
}

impl SimpleInstrInfo_os_loop {
	pub(super) fn new(bitness: u32, reg_size: u32, cc_index: u32, mnemonics: Vec<String>, mnemonics_suffix: Vec<String>) -> Self {
		Self {
			mnemonics: FormatterString::with_strings(mnemonics),
			mnemonics_suffix: FormatterString::with_strings(mnemonics_suffix),
			bitness,
			cc_index,
			reg_size,
		}
	}
}

impl InstrInfo for SimpleInstrInfo_os_loop {
	fn op_info<'a>(&'a self, options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut flags = InstrOpInfoFlags::NONE;
		let instr_bitness = get_bitness(instruction.code_size());
		let mnemonics = if (instr_bitness != 0 && instr_bitness != self.reg_size) || options.gas_show_mnemonic_size_suffix() {
			&self.mnemonics_suffix
		} else {
			&self.mnemonics
		};
		if instr_bitness != 0 && instr_bitness != self.bitness {
			if self.bitness == 16 {
				flags |= InstrOpInfoFlags::OP_SIZE16 | InstrOpInfoFlags::OP_SIZE_IS_BYTE_DIRECTIVE;
			} else if self.bitness == 32 {
				flags |= InstrOpInfoFlags::OP_SIZE32 | InstrOpInfoFlags::OP_SIZE_IS_BYTE_DIRECTIVE;
			} else {
				flags |= InstrOpInfoFlags::OP_SIZE64;
			}
		}
		let mnemonic = if self.cc_index == u32::MAX { &mnemonics[0] } else { get_mnemonic_cc(options, self.cc_index, mnemonics) };
		InstrOpInfo::new(mnemonic, instruction, flags)
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_os_jcc {
	mnemonics: Vec<FormatterString>,
	bitness: u32,
	cc_index: u32,
}

impl SimpleInstrInfo_os_jcc {
	pub(super) fn new(bitness: u32, cc_index: u32, mnemonics: Vec<String>) -> Self {
		Self { mnemonics: FormatterString::with_strings(mnemonics), bitness, cc_index }
	}
}

impl InstrInfo for SimpleInstrInfo_os_jcc {
	fn op_info<'a>(&'a self, options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut flags = InstrOpInfoFlags::NONE;
		let instr_bitness = get_bitness(instruction.code_size());
		if instr_bitness != 0 && instr_bitness != self.bitness {
			if self.bitness == 16 {
				flags |= InstrOpInfoFlags::OP_SIZE16;
			} else if self.bitness == 32 {
				flags |= InstrOpInfoFlags::OP_SIZE32;
			} else {
				flags |= InstrOpInfoFlags::OP_SIZE64;
			}
		}
		let prefix_seg = instruction.segment_prefix();
		if prefix_seg == Register::CS {
			flags |= InstrOpInfoFlags::JCC_NOT_TAKEN;
		} else if prefix_seg == Register::DS {
			flags |= InstrOpInfoFlags::JCC_TAKEN;
		}
		if instruction.has_repne_prefix() {
			flags |= InstrOpInfoFlags::BND_PREFIX;
		}
		let mnemonic = get_mnemonic_cc(options, self.cc_index, &self.mnemonics);
		InstrOpInfo::new(get_mnemonic(options, instruction, mnemonic, mnemonic, flags), instruction, flags)
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_movabs {
	mnemonic: FormatterString,
	mnemonic_suffix: FormatterString,
	mnemonic64: FormatterString,
	mnemonic_suffix64: FormatterString,
}

impl SimpleInstrInfo_movabs {
	pub(super) fn new(mnemonic: String, mnemonic_suffix: String, mnemonic64: String, mnemonic_suffix64: String) -> Self {
		Self {
			mnemonic: FormatterString::new(mnemonic),
			mnemonic_suffix: FormatterString::new(mnemonic_suffix),
			mnemonic64: FormatterString::new(mnemonic64),
			mnemonic_suffix64: FormatterString::new(mnemonic_suffix64),
		}
	}
}

impl InstrInfo for SimpleInstrInfo_movabs {
	fn op_info<'a>(&'a self, options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut flags = InstrOpInfoFlags::NONE;
		let mut instr_bitness = get_bitness(instruction.code_size());
		let (mem_size, mnemonic, mnemonic_suffix): (u32, &FormatterString, &FormatterString) = match instruction.memory_displ_size() {
			2 => (16, &self.mnemonic, &self.mnemonic_suffix),
			4 => (32, &self.mnemonic, &self.mnemonic_suffix),
			_ => (64, &self.mnemonic64, &self.mnemonic_suffix64),
		};
		if instr_bitness == 0 {
			instr_bitness = mem_size;
		}
		if instr_bitness == 64 {
			if mem_size == 32 {
				flags |= InstrOpInfoFlags::ADDR_SIZE32;
			}
		} else if instr_bitness != mem_size {
			debug_assert!(mem_size == 16 || mem_size == 32);
			if mem_size == 16 {
				flags |= InstrOpInfoFlags::ADDR_SIZE16;
			} else {
				flags |= InstrOpInfoFlags::ADDR_SIZE32;
			}
		}
		InstrOpInfo::new(get_mnemonic(options, instruction, mnemonic, mnemonic_suffix, flags), instruction, flags)
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_er {
	mnemonic: FormatterString,
	mnemonic_suffix: FormatterString,
	er_index: u32,
	flags: u32,
}

impl SimpleInstrInfo_er {
	pub(super) fn with_mnemonic(er_index: u32, mnemonic: String) -> Self {
		Self {
			mnemonic: FormatterString::new(mnemonic.clone()),
			mnemonic_suffix: FormatterString::new(mnemonic),
			er_index,
			flags: InstrOpInfoFlags::NONE,
		}
	}
	pub(super) fn new(er_index: u32, mnemonic: String, mnemonic_suffix: String, flags: u32) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), mnemonic_suffix: FormatterString::new(mnemonic_suffix), er_index, flags }
	}

	fn move_operands(info: &mut InstrOpInfo<'_>, index: u32, new_op_kind: InstrOpKind) {
		debug_assert!(info.op_count <= 4);

		match index {
			0 => {
				info.op_kinds[4] = info.op_kinds[3];
				info.op_registers[4] = info.op_registers[3];
				info.op_kinds[3] = info.op_kinds[2];
				info.op_registers[3] = info.op_registers[2];
				info.op_kinds[2] = info.op_kinds[1];
				info.op_registers[2] = info.op_registers[1];
				info.op_kinds[1] = info.op_kinds[0];
				info.op_registers[1] = info.op_registers[0];
				info.op_kinds[0] = new_op_kind;
				info.op_indexes[4] = info.op_indexes[3];
				info.op_indexes[3] = info.op_indexes[2];
				info.op_indexes[2] = info.op_indexes[1];
				info.op_indexes[1] = info.op_indexes[0];
				info.op_indexes[0] = InstrInfoConstants::OP_ACCESS_NONE;
				info.op_count += 1;
			}

			1 => {
				info.op_kinds[4] = info.op_kinds[3];
				info.op_registers[4] = info.op_registers[3];
				info.op_kinds[3] = info.op_kinds[2];
				info.op_registers[3] = info.op_registers[2];
				info.op_kinds[2] = info.op_kinds[1];
				info.op_registers[2] = info.op_registers[1];
				info.op_kinds[1] = new_op_kind;
				info.op_indexes[4] = info.op_indexes[3];
				info.op_indexes[3] = info.op_indexes[2];
				info.op_indexes[2] = info.op_indexes[1];
				info.op_indexes[1] = InstrInfoConstants::OP_ACCESS_NONE;
				info.op_count += 1;
			}

			_ => unreachable!(),
		}
	}
}

impl InstrInfo for SimpleInstrInfo_er {
	fn op_info<'a>(&'a self, options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut info =
			InstrOpInfo::new(get_mnemonic(options, instruction, &self.mnemonic, &self.mnemonic_suffix, self.flags), instruction, self.flags);
		if IcedConstants::is_mvex(instruction.code()) {
			let rc = instruction.rounding_control();
			if rc != RoundingControl::None {
				let rc_op_kind = if instruction.suppress_all_exceptions() {
					match rc {
						RoundingControl::None => return info,
						RoundingControl::RoundToNearest => InstrOpKind::RnSae,
						RoundingControl::RoundDown => InstrOpKind::RdSae,
						RoundingControl::RoundUp => InstrOpKind::RuSae,
						RoundingControl::RoundTowardZero => InstrOpKind::RzSae,
					}
				} else {
					match rc {
						RoundingControl::None => return info,
						RoundingControl::RoundToNearest => InstrOpKind::Rn,
						RoundingControl::RoundDown => InstrOpKind::Rd,
						RoundingControl::RoundUp => InstrOpKind::Ru,
						RoundingControl::RoundTowardZero => InstrOpKind::Rz,
					}
				};
				SimpleInstrInfo_er::move_operands(&mut info, self.er_index, rc_op_kind);
			} else if instruction.suppress_all_exceptions() {
				SimpleInstrInfo_er::move_operands(&mut info, self.er_index, InstrOpKind::Sae);
			}
		} else {
			let rc = instruction.rounding_control();
			if rc != RoundingControl::None && can_show_rounding_control(instruction, options) {
				let rc_op_kind = match rc {
					RoundingControl::None => return info,
					RoundingControl::RoundToNearest => InstrOpKind::RnSae,
					RoundingControl::RoundDown => InstrOpKind::RdSae,
					RoundingControl::RoundUp => InstrOpKind::RuSae,
					RoundingControl::RoundTowardZero => InstrOpKind::RzSae,
				};
				SimpleInstrInfo_er::move_operands(&mut info, self.er_index, rc_op_kind);
			}
		}
		info
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_sae {
	mnemonic: FormatterString,
	sae_index: u32,
}

impl SimpleInstrInfo_sae {
	pub(super) fn new(sae_index: u32, mnemonic: String) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), sae_index }
	}
}

impl InstrInfo for SimpleInstrInfo_sae {
	fn op_info<'a>(&'a self, _options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut info = InstrOpInfo::new(&self.mnemonic, instruction, InstrOpInfoFlags::NONE);
		if instruction.suppress_all_exceptions() {
			SimpleInstrInfo_er::move_operands(&mut info, self.sae_index, InstrOpKind::Sae);
		}
		info
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_far {
	mnemonic: FormatterString,
	mnemonic_suffix: FormatterString,
	bitness: u32,
}

impl SimpleInstrInfo_far {
	pub(super) fn new(bitness: u32, mnemonic: String, mnemonic_suffix: String) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), mnemonic_suffix: FormatterString::new(mnemonic_suffix), bitness }
	}
}

impl InstrInfo for SimpleInstrInfo_far {
	fn op_info<'a>(&'a self, options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut flags = InstrOpInfoFlags::INDIRECT_OPERAND;
		let mut instr_bitness = get_bitness(instruction.code_size());
		if instr_bitness == 0 {
			instr_bitness = self.bitness;
		}
		let mnemonic = if self.bitness == 64 {
			flags |= InstrOpInfoFlags::OP_SIZE64;
			debug_assert_eq!(self.mnemonic.get(false), self.mnemonic_suffix.get(false));
			&self.mnemonic
		} else {
			if self.bitness != instr_bitness || options.gas_show_mnemonic_size_suffix() {
				&self.mnemonic_suffix
			} else {
				&self.mnemonic
			}
		};
		InstrOpInfo::new(mnemonic, instruction, flags)
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_bnd {
	mnemonic: FormatterString,
	mnemonic_suffix: FormatterString,
	flags: u32,
}

impl SimpleInstrInfo_bnd {
	pub(super) fn new(mnemonic: String, mnemonic_suffix: String, flags: u32) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), mnemonic_suffix: FormatterString::new(mnemonic_suffix), flags }
	}
}

impl InstrInfo for SimpleInstrInfo_bnd {
	fn op_info<'a>(&'a self, options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut flags = self.flags;
		if instruction.has_repne_prefix() {
			flags |= InstrOpInfoFlags::BND_PREFIX;
		}
		InstrOpInfo::new(get_mnemonic(options, instruction, &self.mnemonic, &self.mnemonic_suffix, flags), instruction, flags)
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_pops {
	mnemonic: FormatterString,
	pseudo_ops: &'static [FormatterString],
	can_use_sae: bool,
}

impl SimpleInstrInfo_pops {
	pub(super) fn new(mnemonic: String, pseudo_ops: &'static [FormatterString], can_use_sae: bool) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), pseudo_ops, can_use_sae }
	}

	fn remove_first_imm8_operand(info: &mut InstrOpInfo<'_>) {
		debug_assert_eq!(info.op_kinds[0], InstrOpKind::Immediate8);
		info.op_count -= 1;
		match info.op_count {
			0 => info.op_indexes[0] = OP_ACCESS_INVALID,

			1 => {
				info.op_kinds[0] = info.op_kinds[1];
				info.op_registers[0] = info.op_registers[1];
				info.op_indexes[0] = info.op_indexes[1];
				info.op_indexes[1] = OP_ACCESS_INVALID;
			}

			2 => {
				info.op_kinds[0] = info.op_kinds[1];
				info.op_registers[0] = info.op_registers[1];
				info.op_kinds[1] = info.op_kinds[2];
				info.op_registers[1] = info.op_registers[2];
				info.op_indexes[0] = info.op_indexes[1];
				info.op_indexes[1] = info.op_indexes[2];
				info.op_indexes[2] = OP_ACCESS_INVALID;
			}

			3 => {
				info.op_kinds[0] = info.op_kinds[1];
				info.op_registers[0] = info.op_registers[1];
				info.op_kinds[1] = info.op_kinds[2];
				info.op_registers[1] = info.op_registers[2];
				info.op_kinds[2] = info.op_kinds[3];
				info.op_registers[2] = info.op_registers[3];
				info.op_indexes[0] = info.op_indexes[1];
				info.op_indexes[1] = info.op_indexes[2];
				info.op_indexes[2] = info.op_indexes[3];
				info.op_indexes[3] = OP_ACCESS_INVALID;
			}

			4 => {
				info.op_kinds[0] = info.op_kinds[1];
				info.op_registers[0] = info.op_registers[1];
				info.op_kinds[1] = info.op_kinds[2];
				info.op_registers[1] = info.op_registers[2];
				info.op_kinds[2] = info.op_kinds[3];
				info.op_registers[2] = info.op_registers[3];
				info.op_kinds[3] = info.op_kinds[4];
				info.op_registers[3] = info.op_registers[4];
				info.op_indexes[0] = info.op_indexes[1];
				info.op_indexes[1] = info.op_indexes[2];
				info.op_indexes[2] = info.op_indexes[3];
				info.op_indexes[3] = info.op_indexes[4];
				info.op_indexes[4] = OP_ACCESS_INVALID;
			}

			_ => unreachable!(),
		}
	}
}

impl InstrInfo for SimpleInstrInfo_pops {
	fn op_info<'a>(&'a self, options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut info = InstrOpInfo::new(&self.mnemonic, instruction, InstrOpInfoFlags::NONE);
		if self.can_use_sae && instruction.suppress_all_exceptions() {
			SimpleInstrInfo_er::move_operands(&mut info, 1, InstrOpKind::Sae);
		}
		let imm = instruction.immediate8() as usize;
		if options.use_pseudo_ops() && imm < self.pseudo_ops.len() {
			SimpleInstrInfo_pops::remove_first_imm8_operand(&mut info);
			info.mnemonic = &self.pseudo_ops[imm];
		}
		info
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_pclmulqdq {
	mnemonic: FormatterString,
	pseudo_ops: &'static [FormatterString],
}

impl SimpleInstrInfo_pclmulqdq {
	pub(super) fn new(mnemonic: String, pseudo_ops: &'static [FormatterString]) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), pseudo_ops }
	}
}

impl InstrInfo for SimpleInstrInfo_pclmulqdq {
	fn op_info<'a>(&'a self, options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut info = InstrOpInfo::new(&self.mnemonic, instruction, InstrOpInfoFlags::NONE);
		if options.use_pseudo_ops() {
			let index: isize = match instruction.immediate8() {
				0 => 0,
				1 => 1,
				0x10 => 2,
				0x11 => 3,
				_ => -1,
			};
			if index >= 0 {
				SimpleInstrInfo_pops::remove_first_imm8_operand(&mut info);
				info.mnemonic = &self.pseudo_ops[index as usize];
			}
		}
		info
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_imul {
	mnemonic: FormatterString,
	mnemonic_suffix: FormatterString,
}

impl SimpleInstrInfo_imul {
	pub(super) fn new(mnemonic: String, mnemonic_suffix: String) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), mnemonic_suffix: FormatterString::new(mnemonic_suffix) }
	}
}

impl InstrInfo for SimpleInstrInfo_imul {
	fn op_info<'a>(&'a self, options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		const FLAGS: u32 = InstrOpInfoFlags::NONE;
		let mut info = InstrOpInfo::new(get_mnemonic(options, instruction, &self.mnemonic, &self.mnemonic_suffix, FLAGS), instruction, FLAGS);
		debug_assert_eq!(info.op_count, 3);
		if options.use_pseudo_ops()
			&& info.op_kinds[1] == InstrOpKind::Register
			&& info.op_kinds[2] == InstrOpKind::Register
			&& info.op_registers[1] == info.op_registers[2]
		{
			info.op_count -= 1;
			info.op_indexes[1] = InstrInfoConstants::OP_ACCESS_READ_WRITE;
			info.op_indexes[2] = OP_ACCESS_INVALID;
		}
		info
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_Reg32 {
	mnemonic: FormatterString,
}

impl SimpleInstrInfo_Reg32 {
	pub(super) fn new(mnemonic: String) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic) }
	}
}

impl InstrInfo for SimpleInstrInfo_Reg32 {
	fn op_info<'a>(&'a self, _options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		const FLAGS: u32 = InstrOpInfoFlags::NONE;
		let mut info = InstrOpInfo::new(&self.mnemonic, instruction, FLAGS);
		info.op_registers[0] = r64_to_r32(info.op_registers[0]);
		info.op_registers[1] = r64_to_r32(info.op_registers[1]);
		info.op_registers[2] = r64_to_r32(info.op_registers[2]);
		info
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_DeclareData {
	mnemonic: FormatterString,
	op_kind: InstrOpKind,
}

impl SimpleInstrInfo_DeclareData {
	pub(super) fn new(code: Code, mnemonic: String) -> Self {
		let op_kind = match code {
			Code::DeclareByte => InstrOpKind::DeclareByte,
			Code::DeclareWord => InstrOpKind::DeclareWord,
			Code::DeclareDword => InstrOpKind::DeclareDword,
			Code::DeclareQword => InstrOpKind::DeclareQword,
			_ => unreachable!(),
		};
		Self { mnemonic: FormatterString::new(mnemonic), op_kind }
	}
}

impl InstrInfo for SimpleInstrInfo_DeclareData {
	fn op_info<'a>(&'a self, _options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut info = InstrOpInfo::new(&self.mnemonic, instruction, InstrOpInfoFlags::KEEP_OPERAND_ORDER | InstrOpInfoFlags::MNEMONIC_IS_DIRECTIVE);
		info.op_count = instruction.declare_data_len() as u8;
		info.op_kinds[0] = self.op_kind;
		info.op_kinds[1] = self.op_kind;
		info.op_kinds[2] = self.op_kind;
		info.op_kinds[3] = self.op_kind;
		info.op_kinds[4] = self.op_kind;
		info.op_indexes[0] = InstrInfoConstants::OP_ACCESS_READ;
		info.op_indexes[1] = InstrInfoConstants::OP_ACCESS_READ;
		info.op_indexes[2] = InstrInfoConstants::OP_ACCESS_READ;
		info.op_indexes[3] = InstrInfoConstants::OP_ACCESS_READ;
		info.op_indexes[4] = InstrInfoConstants::OP_ACCESS_READ;
		info
	}
}
