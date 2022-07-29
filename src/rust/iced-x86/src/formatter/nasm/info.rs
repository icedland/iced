// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::formatter::nasm::enums::MemorySizeInfo as NasmMemorySizeInfo;
use crate::formatter::nasm::enums::*;
use crate::formatter::nasm::fmt_utils::can_show_rounding_control;
use crate::formatter::nasm::get_mnemonic_cc;
use crate::formatter::nasm::mem_size_tbl::MEM_SIZE_TBL;
use crate::formatter::FormatterString;
use crate::formatter::{r64_to_r32, r_to_r16};
use crate::iced_constants::IcedConstants;
use crate::*;
use alloc::string::String;
use alloc::vec::Vec;
use core::mem;

#[derive(Debug)]
pub(super) struct InstrOpInfo<'a> {
	pub(super) mnemonic: &'a FormatterString,
	pub(super) flags: u32, // InstrOpInfoFlags
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

	pub(super) fn memory_size(&self) -> MemorySize {
		unsafe {
			mem::transmute(((self.flags >> InstrOpInfoFlags::MEMORY_SIZE_SHIFT) & InstrOpInfoFlags::MEMORY_SIZE_MASK) as MemorySizeUnderlyingType)
		}
	}

	fn set_memory_size(&mut self, value: MemorySize) {
		self.flags = (self.flags & !(InstrOpInfoFlags::MEMORY_SIZE_MASK << InstrOpInfoFlags::MEMORY_SIZE_SHIFT))
			| (((value as u32) & InstrOpInfoFlags::MEMORY_SIZE_MASK) << InstrOpInfoFlags::MEMORY_SIZE_SHIFT);
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
		res.mnemonic = mnemonic;
		res.flags = flags | ((instruction.memory_size() as u32) << InstrOpInfoFlags::MEMORY_SIZE_SHIFT);
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
		let op_count = instruction.op_count();
		res.op_count = op_count as u8;
		match op_count {
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
				res.op_indexes[1] = 1;
				res.op_indexes[2] = OP_ACCESS_INVALID;
				res.op_indexes[3] = OP_ACCESS_INVALID;
				res.op_indexes[4] = OP_ACCESS_INVALID;
			}

			3 => {
				res.op_indexes[1] = 1;
				res.op_indexes[2] = 2;
				res.op_indexes[3] = OP_ACCESS_INVALID;
				res.op_indexes[4] = OP_ACCESS_INVALID;
			}

			4 => {
				res.op_indexes[1] = 1;
				res.op_indexes[2] = 2;
				res.op_indexes[3] = 3;
				res.op_indexes[4] = OP_ACCESS_INVALID;
			}

			5 => {
				res.op_indexes[1] = 1;
				res.op_indexes[2] = 2;
				res.op_indexes[3] = 3;
				res.op_indexes[4] = 4;
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

pub(super) struct SimpleInstrInfo {
	mnemonic: FormatterString,
	flags: u32,
}

impl SimpleInstrInfo {
	pub(super) fn with_mnemonic(mnemonic: String) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), flags: InstrOpInfoFlags::NONE }
	}
	pub(super) fn new(mnemonic: String, flags: u32) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), flags }
	}
}

impl InstrInfo for SimpleInstrInfo {
	fn op_info<'a>(&'a self, _options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		InstrOpInfo::new(&self.mnemonic, instruction, self.flags)
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_cc {
	mnemonics: Vec<FormatterString>,
	cc_index: u32,
}

impl SimpleInstrInfo_cc {
	pub(super) fn new(cc_index: u32, mnemonics: Vec<String>) -> Self {
		Self { mnemonics: FormatterString::with_strings(mnemonics), cc_index }
	}
}

impl InstrInfo for SimpleInstrInfo_cc {
	fn op_info<'a>(&'a self, options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		const FLAGS: u32 = InstrOpInfoFlags::NONE;
		let mnemonic = get_mnemonic_cc(options, self.cc_index, &self.mnemonics);
		InstrOpInfo::new(mnemonic, instruction, FLAGS)
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_push_imm8 {
	mnemonic: FormatterString,
	bitness: u32,
	sex_info: SignExtendInfo,
}

impl SimpleInstrInfo_push_imm8 {
	pub(super) fn new(bitness: u32, sex_info: SignExtendInfo, mnemonic: String) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), bitness, sex_info }
	}
}

impl InstrInfo for SimpleInstrInfo_push_imm8 {
	fn op_info<'a>(&'a self, _options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut flags = (self.sex_info as u32) << InstrOpInfoFlags::SIGN_EXTEND_INFO_SHIFT;

		let instr_bitness = get_bitness(instruction.code_size());
		if self.bitness != 0 && instr_bitness != 0 && instr_bitness != self.bitness {
			if self.bitness == 16 {
				flags |= InstrOpInfoFlags::OP_SIZE16;
			} else if self.bitness == 32 {
				flags |= InstrOpInfoFlags::OP_SIZE32;
			} else {
				flags |= InstrOpInfoFlags::OP_SIZE64;
			}
		}

		InstrOpInfo::new(&self.mnemonic, instruction, flags)
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_push_imm {
	mnemonic: FormatterString,
	bitness: u32,
	sex_info: SignExtendInfo,
}

impl SimpleInstrInfo_push_imm {
	pub(super) fn new(bitness: u32, sex_info: SignExtendInfo, mnemonic: String) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), bitness, sex_info }
	}
}

impl InstrInfo for SimpleInstrInfo_push_imm {
	fn op_info<'a>(&'a self, _options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut flags = InstrOpInfoFlags::NONE;

		let mut sign_extend = true;
		let instr_bitness = get_bitness(instruction.code_size());
		if self.bitness != 0 && instr_bitness != 0 && instr_bitness != self.bitness {
			if instr_bitness == 64 {
				flags |= InstrOpInfoFlags::OP_SIZE16;
			}
		} else if self.bitness == 16 && instr_bitness == 16 {
			sign_extend = false;
		}

		if sign_extend {
			flags |= (self.sex_info as u32) << InstrOpInfoFlags::SIGN_EXTEND_INFO_SHIFT;
		}

		InstrOpInfo::new(&self.mnemonic, instruction, flags)
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_SignExt {
	mnemonic: FormatterString,
	sex_info_reg: SignExtendInfo,
	sex_info_mem: SignExtendInfo,
	flags: u32,
}

impl SimpleInstrInfo_SignExt {
	pub(super) fn new3(sex_info: SignExtendInfo, mnemonic: String, flags: u32) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), sex_info_reg: sex_info, sex_info_mem: sex_info, flags }
	}
	pub(super) fn new(sex_info_reg: SignExtendInfo, sex_info_mem: SignExtendInfo, mnemonic: String, flags: u32) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), sex_info_reg, sex_info_mem, flags }
	}
}

impl InstrInfo for SimpleInstrInfo_SignExt {
	fn op_info<'a>(&'a self, _options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		debug_assert_eq!(instruction.op_count(), 2);
		let sex_info =
			if instruction.op0_kind() == OpKind::Memory || instruction.op1_kind() == OpKind::Memory { self.sex_info_mem } else { self.sex_info_reg };
		let flags = self.flags | ((sex_info as u32) << InstrOpInfoFlags::SIGN_EXTEND_INFO_SHIFT);
		InstrOpInfo::new(&self.mnemonic, instruction, flags)
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_imul {
	mnemonic: FormatterString,
	sex_info: SignExtendInfo,
}

impl SimpleInstrInfo_imul {
	pub(super) fn new(sex_info: SignExtendInfo, mnemonic: String) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), sex_info }
	}
}

impl InstrInfo for SimpleInstrInfo_imul {
	fn op_info<'a>(&'a self, options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let flags = (self.sex_info as u32) << InstrOpInfoFlags::SIGN_EXTEND_INFO_SHIFT;
		let mut info = InstrOpInfo::new(&self.mnemonic, instruction, flags);
		debug_assert_eq!(info.op_count, 3);
		if options.use_pseudo_ops()
			&& info.op_kinds[0] == InstrOpKind::Register
			&& info.op_kinds[1] == InstrOpKind::Register
			&& info.op_registers[0] == info.op_registers[1]
		{
			info.op_count -= 1;
			info.op_indexes[0] = InstrInfoConstants::OP_ACCESS_READ_WRITE;
			info.op_kinds[1] = info.op_kinds[2];
			info.op_indexes[1] = 2;
			info.op_indexes[2] = OP_ACCESS_INVALID;
		}
		info
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

const fn get_address_size_flags(op_kind: OpKind) -> u32 {
	match op_kind {
		OpKind::MemorySegSI | OpKind::MemorySegDI | OpKind::MemoryESDI => InstrOpInfoFlags::ADDR_SIZE16,
		OpKind::MemorySegESI | OpKind::MemorySegEDI | OpKind::MemoryESEDI => InstrOpInfoFlags::ADDR_SIZE32,
		OpKind::MemorySegRSI | OpKind::MemorySegRDI | OpKind::MemoryESRDI => InstrOpInfoFlags::ADDR_SIZE64,
		_ => 0,
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_String {
	mnemonic: FormatterString,
}

impl SimpleInstrInfo_String {
	pub(super) fn new(mnemonic: String) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic) }
	}
}

impl InstrInfo for SimpleInstrInfo_String {
	fn op_info<'a>(&'a self, _options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let op_kind = if instruction.op0_kind() != OpKind::Register { instruction.op0_kind() } else { instruction.op1_kind() };
		let op_kind_flags = get_address_size_flags(op_kind);
		let instr_flags = match instruction.code_size() {
			CodeSize::Unknown => op_kind_flags,
			CodeSize::Code16 => InstrOpInfoFlags::ADDR_SIZE16,
			CodeSize::Code32 => InstrOpInfoFlags::ADDR_SIZE32,
			CodeSize::Code64 => InstrOpInfoFlags::ADDR_SIZE64,
		};
		let flags = if op_kind_flags != instr_flags { op_kind_flags } else { 0 };
		let mut info = InstrOpInfo::default(&self.mnemonic);
		info.flags = flags;
		info
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_XLAT {
	mnemonic: FormatterString,
}

impl SimpleInstrInfo_XLAT {
	pub(super) fn new(mnemonic: String) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic) }
	}
}

impl InstrInfo for SimpleInstrInfo_XLAT {
	fn op_info<'a>(&'a self, _options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let base_reg = match instruction.code_size() {
			CodeSize::Unknown => instruction.memory_base(),
			CodeSize::Code16 => Register::BX,
			CodeSize::Code32 => Register::EBX,
			CodeSize::Code64 => Register::RBX,
		};
		let mut flags = 0;
		let mem_base_reg = instruction.memory_base();
		if mem_base_reg != base_reg {
			if mem_base_reg == Register::BX {
				flags |= InstrOpInfoFlags::ADDR_SIZE16;
			} else if mem_base_reg == Register::EBX {
				flags |= InstrOpInfoFlags::ADDR_SIZE32;
			} else if mem_base_reg == Register::RBX {
				flags |= InstrOpInfoFlags::ADDR_SIZE64;
			}
		}
		let mut info = InstrOpInfo::default(&self.mnemonic);
		info.flags = flags;
		info
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_nop {
	mnemonic: FormatterString,
	bitness: u32,
	register: Register,
	str_xchg: FormatterString,
}

impl SimpleInstrInfo_nop {
	pub(super) fn new(bitness: u32, mnemonic: String, register: Register) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), bitness, register, str_xchg: FormatterString::new_str("xchg") }
	}
}

impl InstrInfo for SimpleInstrInfo_nop {
	fn op_info<'a>(&'a self, _options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let instr_bitness = get_bitness(instruction.code_size());
		if instr_bitness == 0 || (instr_bitness & self.bitness) != 0 {
			InstrOpInfo::new(&self.mnemonic, instruction, InstrOpInfoFlags::NONE)
		} else {
			let mut info = InstrOpInfo::default(&self.str_xchg);
			info.op_count = 2;
			const _: () = assert!(InstrOpKind::Register as u32 == 0);
			//info.op_kinds[0] = InstrOpKind::Register;
			//info.op_kinds[1] = InstrOpKind::Register;
			info.op_registers[0] = self.register;
			info.op_registers[1] = self.register;
			info.op_indexes[0] = InstrInfoConstants::OP_ACCESS_NONE;
			info.op_indexes[1] = InstrInfoConstants::OP_ACCESS_NONE;
			info
		}
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
pub(super) struct SimpleInstrInfo_STIG2 {
	mnemonic: FormatterString,
	flags: u32,
	pseudo_op: bool,
}

impl SimpleInstrInfo_STIG2 {
	pub(super) fn with_pseudo_op(mnemonic: String, pseudo_op: bool) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), flags: InstrOpInfoFlags::NONE, pseudo_op }
	}
	pub(super) fn with_flags(mnemonic: String, flags: u32) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), flags, pseudo_op: false }
	}
}

impl InstrInfo for SimpleInstrInfo_STIG2 {
	#[allow(clippy::nonminimal_bool)]
	fn op_info<'a>(&'a self, options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut info = InstrOpInfo::default(&self.mnemonic);
		info.flags = self.flags;
		debug_assert_eq!(instruction.op_count(), 2);
		debug_assert!(instruction.op1_kind() == OpKind::Register && instruction.op1_register() == Register::ST0);
		if !self.pseudo_op || !(options.use_pseudo_ops() && instruction.op0_register() == Register::ST1) {
			info.op_count = 1;
			const _: () = assert!(InstrOpKind::Register as u32 == 0);
			//info.op_kinds[0] = InstrOpKind::Register;
			info.op_registers[0] = instruction.op0_register();
		}
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
		Self { mnemonic: FormatterString::new(mnemonic), bitness }
	}
}

impl InstrInfo for SimpleInstrInfo_as {
	fn op_info<'a>(&'a self, _options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut flags = InstrOpInfoFlags::NONE;
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
		info.op_kinds[0] = InstrOpInfo::to_instr_op_kind(instruction.op1_kind());
		info.op_indexes[0] = 1;
		info.op_registers[0] = instruction.op1_register();
		info.op_kinds[1] = InstrOpInfo::to_instr_op_kind(instruction.op2_kind());
		info.op_indexes[1] = 2;
		info.op_registers[1] = instruction.op2_register();
		if instr_bitness != 0 && instr_bitness != bitness {
			if bitness == 16 {
				info.flags |= InstrOpInfoFlags::ADDR_SIZE16;
			} else if bitness == 32 {
				info.flags |= InstrOpInfoFlags::ADDR_SIZE32;
			} else {
				info.flags |= InstrOpInfoFlags::ADDR_SIZE64;
			}
		}
		info
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_pblendvb {
	mnemonic: FormatterString,
	mem_size: MemorySize,
}

impl SimpleInstrInfo_pblendvb {
	pub(super) fn new(mnemonic: String, mem_size: MemorySize) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), mem_size }
	}
}

impl InstrInfo for SimpleInstrInfo_pblendvb {
	fn op_info<'a>(&'a self, _options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut info = InstrOpInfo::default(&self.mnemonic);
		debug_assert_eq!(instruction.op_count(), 2);
		info.op_count = 3;
		info.op_kinds[0] = InstrOpInfo::to_instr_op_kind(instruction.op0_kind());
		info.op_registers[0] = instruction.op0_register();
		info.op_kinds[1] = InstrOpInfo::to_instr_op_kind(instruction.op1_kind());
		info.op_indexes[1] = 1;
		info.op_registers[1] = instruction.op1_register();
		const _: () = assert!(InstrOpKind::Register as u32 == 0);
		//info.op_kinds[2] = InstrOpKind::Register;
		info.op_registers[2] = Register::XMM0;
		info.set_memory_size(self.mem_size);
		info.op_indexes[2] = InstrInfoConstants::OP_ACCESS_READ;
		info
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_reverse {
	mnemonic: FormatterString,
}

impl SimpleInstrInfo_reverse {
	pub(super) fn new(mnemonic: String) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic) }
	}
}

impl InstrInfo for SimpleInstrInfo_reverse {
	fn op_info<'a>(&'a self, _options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut info = InstrOpInfo::default(&self.mnemonic);
		debug_assert_eq!(instruction.op_count(), 2);
		info.op_count = 2;
		info.op_kinds[0] = InstrOpInfo::to_instr_op_kind(instruction.op1_kind());
		info.op_indexes[0] = 1;
		info.op_registers[0] = instruction.op1_register();
		info.op_kinds[1] = InstrOpInfo::to_instr_op_kind(instruction.op0_kind());
		info.op_registers[1] = instruction.op0_register();
		info.set_memory_size(instruction.memory_size());
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
	fn op_info<'a>(&'a self, _options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mnemonic = if instruction.code_size() == self.code_size {
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
	fn op_info<'a>(&'a self, _options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut flags = InstrOpInfoFlags::NONE;
		if instruction.has_repne_prefix() {
			flags |= InstrOpInfoFlags::BND_PREFIX;
		}
		let mnemonic = &self.mnemonics[instruction.code_size() as usize];
		InstrOpInfo::new(mnemonic, instruction, flags)
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_OpSize3 {
	mnemonic_default: FormatterString,
	mnemonic_full: FormatterString,
	bitness: u32,
}

impl SimpleInstrInfo_OpSize3 {
	pub(super) fn new(bitness: u32, mnemonic_default: String, mnemonic_full: String) -> Self {
		Self { mnemonic_full: FormatterString::new(mnemonic_full), mnemonic_default: FormatterString::new(mnemonic_default), bitness }
	}
}

impl InstrInfo for SimpleInstrInfo_OpSize3 {
	fn op_info<'a>(&'a self, _options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let instr_bitness = get_bitness(instruction.code_size());
		let mnemonic = if instr_bitness == 0 || (instr_bitness & self.bitness) != 0 { &self.mnemonic_default } else { &self.mnemonic_full };
		InstrOpInfo::new(mnemonic, instruction, InstrOpInfoFlags::NONE)
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_os {
	mnemonic: FormatterString,
	bitness: u32,
	flags: u32,
}

impl SimpleInstrInfo_os {
	pub(super) fn with_mnemonic(bitness: u32, mnemonic: String) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), bitness, flags: InstrOpInfoFlags::NONE }
	}
	pub(super) fn new(bitness: u32, mnemonic: String, flags: u32) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), bitness, flags }
	}
}

impl InstrInfo for SimpleInstrInfo_os {
	fn op_info<'a>(&'a self, _options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut flags = self.flags;
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
		InstrOpInfo::new(&self.mnemonic, instruction, flags)
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_os_mem {
	mnemonic: FormatterString,
	bitness: u32,
}

impl SimpleInstrInfo_os_mem {
	pub(super) fn new(bitness: u32, mnemonic: String) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), bitness }
	}
}

impl InstrInfo for SimpleInstrInfo_os_mem {
	fn op_info<'a>(&'a self, _options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
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
		InstrOpInfo::new(&self.mnemonic, instruction, flags)
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_os_mem2 {
	mnemonic: FormatterString,
	bitness: u32,
	flags: u32,
}

impl SimpleInstrInfo_os_mem2 {
	pub(super) fn new(bitness: u32, mnemonic: String, flags: u32) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), bitness, flags }
	}
}

impl InstrInfo for SimpleInstrInfo_os_mem2 {
	fn op_info<'a>(&'a self, _options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut flags = self.flags;
		let instr_bitness = get_bitness(instruction.code_size());
		if instr_bitness != 0 && (instr_bitness & self.bitness) == 0 {
			if instr_bitness != 16 {
				flags |= InstrOpInfoFlags::OP_SIZE16;
			} else {
				flags |= InstrOpInfoFlags::OP_SIZE32;
			}
		}
		InstrOpInfo::new(&self.mnemonic, instruction, flags)
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_os_mem_reg16 {
	mnemonic: FormatterString,
	bitness: u32,
}

impl SimpleInstrInfo_os_mem_reg16 {
	pub(super) fn new(bitness: u32, mnemonic: String) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), bitness }
	}
}

impl InstrInfo for SimpleInstrInfo_os_mem_reg16 {
	fn op_info<'a>(&'a self, _options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut flags = InstrOpInfoFlags::NONE;
		let instr_bitness = get_bitness(instruction.code_size());
		debug_assert_eq!(instruction.op_count(), 1);
		if instruction.op0_kind() == OpKind::Memory {
			if !(instr_bitness == 0 || (instr_bitness != 64 && instr_bitness == self.bitness) || (instr_bitness == 64 && self.bitness == 32)) {
				if self.bitness == 16 {
					flags |= InstrOpInfoFlags::OP_SIZE16;
				} else if self.bitness == 32 {
					flags |= InstrOpInfoFlags::OP_SIZE32;
				} else {
					flags |= InstrOpInfoFlags::OP_SIZE64;
				}
			}
		}
		let mut info = InstrOpInfo::new(&self.mnemonic, instruction, flags);
		if instruction.op0_kind() == OpKind::Register {
			let mut reg = info.op_registers[0];
			let reg_size = if Register::AX <= reg && reg <= Register::R15W {
				16
			} else if Register::EAX <= reg && reg <= Register::R15D {
				reg = r_to_r16(reg);
				32
			} else if Register::RAX <= reg && reg <= Register::R15 {
				reg = r_to_r16(reg);
				64
			} else {
				0
			};
			debug_assert_ne!(reg_size, 0);
			if reg_size != 0 {
				info.op_registers[0] = reg;
				if !((instr_bitness != 64 && instr_bitness == reg_size) || (instr_bitness == 64 && reg_size == 32)) {
					if self.bitness == 16 {
						info.flags |= InstrOpInfoFlags::OP_SIZE16;
					} else if self.bitness == 32 {
						info.flags |= InstrOpInfoFlags::OP_SIZE32;
					} else {
						info.flags |= InstrOpInfoFlags::OP_SIZE64;
					}
				}
			}
		}
		info
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_os_jcc {
	mnemonics: Vec<FormatterString>,
	bitness: u32,
	cc_index: u32,
	flags: u32,
}

impl SimpleInstrInfo_os_jcc {
	pub(super) fn with_mnemonic(bitness: u32, cc_index: u32, mnemonics: Vec<String>) -> Self {
		Self { mnemonics: FormatterString::with_strings(mnemonics), bitness, cc_index, flags: InstrOpInfoFlags::NONE }
	}
	pub(super) fn new(bitness: u32, cc_index: u32, mnemonics: Vec<String>, flags: u32) -> Self {
		Self { mnemonics: FormatterString::with_strings(mnemonics), bitness, cc_index, flags }
	}
}

impl InstrInfo for SimpleInstrInfo_os_jcc {
	fn op_info<'a>(&'a self, options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut flags = self.flags;
		let instr_bitness = get_bitness(instruction.code_size());
		if flags != InstrOpInfoFlags::NONE {
			if instr_bitness != 0 && instr_bitness != self.bitness {
				if self.bitness == 16 {
					flags |= InstrOpInfoFlags::OP_SIZE16;
				} else if self.bitness == 32 {
					flags |= InstrOpInfoFlags::OP_SIZE32;
				} else {
					flags |= InstrOpInfoFlags::OP_SIZE64;
				}
			}
		} else {
			let mut branch_info = BranchSizeInfo::Near;
			if instr_bitness != 0 && instr_bitness != self.bitness {
				if self.bitness == 16 {
					branch_info = BranchSizeInfo::NearWord;
				} else if self.bitness == 32 {
					branch_info = BranchSizeInfo::NearDword;
				}
			}
			flags |= (branch_info as u32) << InstrOpInfoFlags::BRANCH_SIZE_INFO_SHIFT;
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
		InstrOpInfo::new(mnemonic, instruction, flags)
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_os_loop {
	mnemonics: Vec<FormatterString>,
	bitness: u32,
	cc_index: u32,
	register: Register,
}

impl SimpleInstrInfo_os_loop {
	pub(super) fn new(bitness: u32, cc_index: u32, register: Register, mnemonics: Vec<String>) -> Self {
		Self { mnemonics: FormatterString::with_strings(mnemonics), bitness, cc_index, register }
	}
}

impl InstrInfo for SimpleInstrInfo_os_loop {
	fn op_info<'a>(&'a self, options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut flags = InstrOpInfoFlags::NONE;
		let instr_bitness = get_bitness(instruction.code_size());
		let expected_reg = match instr_bitness {
			0 => self.register,
			16 => Register::CX,
			32 => Register::ECX,
			64 => Register::RCX,
			_ => unreachable!(),
		};
		let add_reg = expected_reg != self.register;
		if instr_bitness != 0 && instr_bitness != self.bitness {
			if self.bitness == 16 {
				flags |= InstrOpInfoFlags::OP_SIZE16;
			} else if self.bitness == 32 {
				flags |= InstrOpInfoFlags::OP_SIZE32;
			} else {
				flags |= InstrOpInfoFlags::OP_SIZE64;
			}
		}
		let mnemonic = if self.cc_index == u32::MAX { &self.mnemonics[0] } else { get_mnemonic_cc(options, self.cc_index, &self.mnemonics) };
		let mut info = InstrOpInfo::new(mnemonic, instruction, flags);
		if add_reg {
			debug_assert_eq!(info.op_count, 1);
			info.op_count = 2;
			info.op_kinds[1] = InstrOpKind::Register;
			info.op_indexes[1] = InstrInfoConstants::OP_ACCESS_READ_WRITE;
			info.op_registers[1] = self.register;
		}
		info
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_os_call {
	mnemonic: FormatterString,
	bitness: u32,
	can_have_bnd_prefix: bool,
}

impl SimpleInstrInfo_os_call {
	pub(super) fn new(bitness: u32, mnemonic: String, can_have_bnd_prefix: bool) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), bitness, can_have_bnd_prefix }
	}
}

impl InstrInfo for SimpleInstrInfo_os_call {
	fn op_info<'a>(&'a self, _options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut flags = InstrOpInfoFlags::NONE;
		if self.can_have_bnd_prefix && instruction.has_repne_prefix() {
			flags |= InstrOpInfoFlags::BND_PREFIX;
		}
		let instr_bitness = get_bitness(instruction.code_size());
		let mut branch_info = BranchSizeInfo::None;
		if instr_bitness != 0 && instr_bitness != self.bitness {
			if self.bitness == 16 {
				branch_info = BranchSizeInfo::Word;
			} else if self.bitness == 32 {
				branch_info = BranchSizeInfo::Dword;
			}
		}
		flags |= (branch_info as u32) << InstrOpInfoFlags::BRANCH_SIZE_INFO_SHIFT;
		InstrOpInfo::new(&self.mnemonic, instruction, flags)
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_far {
	mnemonic: FormatterString,
	bitness: u32,
}

impl SimpleInstrInfo_far {
	pub(super) fn new(bitness: u32, mnemonic: String) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), bitness }
	}
}

impl InstrInfo for SimpleInstrInfo_far {
	fn op_info<'a>(&'a self, _options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut flags = InstrOpInfoFlags::NONE;
		let instr_bitness = get_bitness(instruction.code_size());
		let mut branch_info = BranchSizeInfo::None;
		if instr_bitness != 0 && instr_bitness != self.bitness {
			if self.bitness == 16 {
				branch_info = BranchSizeInfo::Word;
			} else {
				branch_info = BranchSizeInfo::Dword;
			}
		}
		flags |= (branch_info as u32) << InstrOpInfoFlags::BRANCH_SIZE_INFO_SHIFT;
		InstrOpInfo::new(&self.mnemonic, instruction, flags)
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_far_mem {
	mnemonic: FormatterString,
	bitness: u32,
}

impl SimpleInstrInfo_far_mem {
	pub(super) fn new(bitness: u32, mnemonic: String) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), bitness }
	}
}

impl InstrInfo for SimpleInstrInfo_far_mem {
	fn op_info<'a>(&'a self, _options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut flags = InstrOpInfoFlags::SHOW_NO_MEM_SIZE_FORCE_SIZE;
		let instr_bitness = get_bitness(instruction.code_size());
		let mut far_mem_size_info = FarMemorySizeInfo::None;
		if instr_bitness != 0 && instr_bitness != self.bitness {
			if self.bitness == 16 {
				far_mem_size_info = FarMemorySizeInfo::Word;
			} else {
				far_mem_size_info = FarMemorySizeInfo::Dword;
			}
		}
		flags |= (far_mem_size_info as u32) << InstrOpInfoFlags::FAR_MEMORY_SIZE_INFO_SHIFT;
		InstrOpInfo::new(&self.mnemonic, instruction, flags)
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_movabs {
	mnemonic: FormatterString,
}

impl SimpleInstrInfo_movabs {
	pub(super) fn new(mnemonic: String) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic) }
	}
}

impl InstrInfo for SimpleInstrInfo_movabs {
	fn op_info<'a>(&'a self, _options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut flags = InstrOpInfoFlags::NONE;
		let mut instr_bitness = get_bitness(instruction.code_size());
		let mem_size = match instruction.memory_displ_size() {
			2 => 16,
			4 => 32,
			_ => 64,
		};
		if instr_bitness == 0 {
			instr_bitness = mem_size;
		}
		let mut mem_size_info = NasmMemorySizeInfo::None;
		if instr_bitness == 64 {
			if mem_size == 32 {
				flags |= InstrOpInfoFlags::ADDR_SIZE32;
			} else {
				mem_size_info = NasmMemorySizeInfo::Qword;
			}
		} else if instr_bitness != mem_size {
			debug_assert!(mem_size == 16 || mem_size == 32);
			if mem_size == 16 {
				mem_size_info = NasmMemorySizeInfo::Word;
			} else {
				mem_size_info = NasmMemorySizeInfo::Dword;
			}
		}
		flags |= (mem_size_info as u32) << InstrOpInfoFlags::MEMORY_SIZE_INFO_SHIFT;
		InstrOpInfo::new(&self.mnemonic, instruction, flags)
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_er {
	mnemonic: FormatterString,
	er_index: u32,
	flags: u32,
}

impl SimpleInstrInfo_er {
	pub(super) fn with_mnemonic(er_index: u32, mnemonic: String) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), er_index, flags: InstrOpInfoFlags::NONE }
	}
	pub(super) fn new(er_index: u32, mnemonic: String, flags: u32) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), er_index, flags }
	}

	fn move_operands(info: &mut InstrOpInfo<'_>, index: u32, new_op_kind: InstrOpKind) {
		debug_assert!(info.op_count <= 4);

		match index {
			2 => {
				debug_assert!(info.op_count < 4 || info.op_kinds[3] != InstrOpKind::Register);
				info.op_kinds[4] = info.op_kinds[3];
				info.op_kinds[3] = info.op_kinds[2];
				info.op_registers[3] = info.op_registers[2];
				info.op_kinds[2] = new_op_kind;
				info.op_indexes[4] = info.op_indexes[3];
				info.op_indexes[3] = info.op_indexes[2];
				info.op_indexes[2] = InstrInfoConstants::OP_ACCESS_NONE;
				info.op_count += 1;
			}

			3 => {
				debug_assert!(info.op_count < 4 || info.op_kinds[3] != InstrOpKind::Register);
				info.op_kinds[4] = info.op_kinds[3];
				info.op_kinds[3] = new_op_kind;
				info.op_indexes[4] = info.op_indexes[3];
				info.op_indexes[3] = InstrInfoConstants::OP_ACCESS_NONE;
				info.op_count += 1;
			}

			_ => unreachable!(),
		}
	}
}

impl InstrInfo for SimpleInstrInfo_er {
	fn op_info<'a>(&'a self, options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut info = InstrOpInfo::new(&self.mnemonic, instruction, self.flags);
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
pub(super) struct SimpleInstrInfo_bcst {
	mnemonic: FormatterString,
	flags_no_broadcast: u32,
}

impl SimpleInstrInfo_bcst {
	pub(super) fn new(mnemonic: String, flags_no_broadcast: u32) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), flags_no_broadcast }
	}
}

impl InstrInfo for SimpleInstrInfo_bcst {
	fn op_info<'a>(&'a self, _options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let bcst_to = (&*MEM_SIZE_TBL)[instruction.memory_size() as usize].bcst_to;
		let flags = if !bcst_to.is_default() { InstrOpInfoFlags::NONE } else { self.flags_no_broadcast };
		InstrOpInfo::new(&self.mnemonic, instruction, flags)
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_bnd {
	mnemonic: FormatterString,
	flags: u32,
}

impl SimpleInstrInfo_bnd {
	pub(super) fn new(mnemonic: String, flags: u32) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), flags }
	}
}

impl InstrInfo for SimpleInstrInfo_bnd {
	fn op_info<'a>(&'a self, _options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut flags = self.flags;
		if instruction.has_repne_prefix() {
			flags |= InstrOpInfoFlags::BND_PREFIX;
		}
		InstrOpInfo::new(&self.mnemonic, instruction, flags)
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_pops {
	mnemonic: FormatterString,
	pseudo_ops: &'static [FormatterString],
}

impl SimpleInstrInfo_pops {
	pub(super) fn new(mnemonic: String, pseudo_ops: &'static [FormatterString]) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), pseudo_ops }
	}

	fn remove_last_op(info: &mut InstrOpInfo<'_>) {
		match info.op_count {
			5 => info.op_indexes[4] = OP_ACCESS_INVALID,
			4 => info.op_indexes[3] = OP_ACCESS_INVALID,
			3 => info.op_indexes[2] = OP_ACCESS_INVALID,
			_ => unreachable!(),
		}
		info.op_count -= 1;
	}
}

impl InstrInfo for SimpleInstrInfo_pops {
	fn op_info<'a>(&'a self, options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut info = InstrOpInfo::new(&self.mnemonic, instruction, InstrOpInfoFlags::NONE);
		if instruction.suppress_all_exceptions() {
			SimpleInstrInfo_er::move_operands(&mut info, instruction.op_count() - 1, InstrOpKind::Sae);
		}
		let imm = instruction.immediate8() as usize;
		if options.use_pseudo_ops() && imm < self.pseudo_ops.len() {
			info.mnemonic = &self.pseudo_ops[imm];
			SimpleInstrInfo_pops::remove_last_op(&mut info);
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
				info.mnemonic = &self.pseudo_ops[index as usize];
				SimpleInstrInfo_pops::remove_last_op(&mut info);
			}
		}
		info
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_Reg16 {
	mnemonic: FormatterString,
}

impl SimpleInstrInfo_Reg16 {
	pub(super) fn new(mnemonic: String) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic) }
	}
}

impl InstrInfo for SimpleInstrInfo_Reg16 {
	fn op_info<'a>(&'a self, _options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		const FLAGS: u32 = InstrOpInfoFlags::NONE;
		let mut info = InstrOpInfo::new(&self.mnemonic, instruction, FLAGS);
		info.op_registers[0] = r_to_r16(info.op_registers[0]);
		info.op_registers[1] = r_to_r16(info.op_registers[1]);
		info.op_registers[2] = r_to_r16(info.op_registers[2]);
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
pub(super) struct SimpleInstrInfo_invlpga {
	mnemonic: FormatterString,
	bitness: u32,
}

impl SimpleInstrInfo_invlpga {
	pub(super) fn new(bitness: u32, mnemonic: String) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), bitness }
	}
}

impl InstrInfo for SimpleInstrInfo_invlpga {
	fn op_info<'a>(&'a self, _options: &FormatterOptions, _instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut info = InstrOpInfo::default(&self.mnemonic);
		info.op_count = 2;
		info.op_kinds[0] = InstrOpKind::Register;
		info.op_kinds[1] = InstrOpKind::Register;
		info.op_registers[1] = Register::ECX;
		info.op_indexes[0] = InstrInfoConstants::OP_ACCESS_READ;
		info.op_indexes[1] = InstrInfoConstants::OP_ACCESS_READ;
		match self.bitness {
			16 => info.op_registers[0] = Register::AX,
			32 => info.op_registers[0] = Register::EAX,
			64 => info.op_registers[0] = Register::RAX,
			_ => unreachable!(),
		}
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
		Self {
			mnemonic: FormatterString::new(mnemonic),
			op_kind: match code {
				Code::DeclareByte => InstrOpKind::DeclareByte,
				Code::DeclareWord => InstrOpKind::DeclareWord,
				Code::DeclareDword => InstrOpKind::DeclareDword,
				Code::DeclareQword => InstrOpKind::DeclareQword,
				_ => unreachable!(),
			},
		}
	}
}

impl InstrInfo for SimpleInstrInfo_DeclareData {
	fn op_info<'a>(&'a self, _options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut info = InstrOpInfo::new(&self.mnemonic, instruction, InstrOpInfoFlags::MNEMONIC_IS_DIRECTIVE);
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
