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

use super::super::super::iced_constants::IcedConstants;
use super::super::super::*;
use super::super::FormatterString;
use super::enums::*;
use super::fmt_utils::show_segment_prefix;
use super::get_mnemonic_cc;
use super::regs::*;
#[cfg(not(feature = "std"))]
use alloc::string::String;
#[cfg(not(feature = "std"))]
use alloc::vec::Vec;
use core::{mem, u32};

#[derive(Debug)]
pub(super) struct InstrOpInfo<'a> {
	pub(super) mnemonic: &'a FormatterString,
	pub(super) flags: u16, // InstrOpInfoFlags
	pub(super) op_count: u8,
	op0_kind: InstrOpKind,
	op1_kind: InstrOpKind,
	op2_kind: InstrOpKind,
	op3_kind: InstrOpKind,
	op4_kind: InstrOpKind,
	op0_register: u8,
	op1_register: u8,
	op2_register: u8,
	op3_register: u8,
	op4_register: u8,
	op0_index: i8,
	op1_index: i8,
	op2_index: i8,
	op3_index: i8,
	op4_index: i8,
}

impl<'a> InstrOpInfo<'a> {
	pub(super) const TEST_REGISTER_BITS: u32 = IcedConstants::REGISTER_BITS;

	pub(super) fn op_register(&self, operand: u32) -> usize {
		match operand {
			0 => self.op0_register as usize,
			1 => self.op1_register as usize,
			2 => self.op2_register as usize,
			3 => self.op3_register as usize,
			4 => self.op4_register as usize,
			_ => panic!(),
		}
	}

	pub(super) fn op_kind(&self, operand: u32) -> InstrOpKind {
		match operand {
			0 => self.op0_kind,
			1 => self.op1_kind,
			2 => self.op2_kind,
			3 => self.op3_kind,
			4 => self.op4_kind,
			_ => {
				debug_assert!(
					self.op0_kind == InstrOpKind::DeclareByte
						|| self.op0_kind == InstrOpKind::DeclareWord
						|| self.op0_kind == InstrOpKind::DeclareDword
						|| self.op0_kind == InstrOpKind::DeclareQword
				);
				self.op0_kind
			}
		}
	}

	pub(super) fn instruction_index(&self, operand: u32) -> Option<u32> {
		let instruction_operand = match operand {
			0 => self.op0_index as i32,
			1 => self.op1_index as i32,
			2 => self.op2_index as i32,
			3 => self.op3_index as i32,
			4 => self.op4_index as i32,
			_ => {
				debug_assert!(
					self.op0_kind == InstrOpKind::DeclareByte
						|| self.op0_kind == InstrOpKind::DeclareWord
						|| self.op0_kind == InstrOpKind::DeclareDword
						|| self.op0_kind == InstrOpKind::DeclareQword
				);
				-1
			}
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
			0 => self.op0_index,
			1 => self.op1_index,
			2 => self.op2_index,
			3 => self.op3_index,
			4 => self.op4_index,
			_ => {
				debug_assert!(
					self.op0_kind == InstrOpKind::DeclareByte
						|| self.op0_kind == InstrOpKind::DeclareWord
						|| self.op0_kind == InstrOpKind::DeclareDword
						|| self.op0_kind == InstrOpKind::DeclareQword
				);
				self.op0_index
			}
		};
		if instruction_operand < OP_ACCESS_INVALID {
			Some(unsafe { mem::transmute(-instruction_operand - 2) })
		} else {
			None
		}
	}

	pub(super) fn operand_index(&self, instruction_operand: u32) -> Option<u32> {
		let index: i32 = if instruction_operand == self.op0_index as u32 {
			0
		} else if instruction_operand == self.op1_index as u32 {
			1
		} else if instruction_operand == self.op2_index as u32 {
			2
		} else if instruction_operand == self.op3_index as u32 {
			3
		} else if instruction_operand == self.op4_index as u32 {
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
			op0_kind: InstrOpKind::default(),
			op1_kind: InstrOpKind::default(),
			op2_kind: InstrOpKind::default(),
			op3_kind: InstrOpKind::default(),
			op4_kind: InstrOpKind::default(),
			op0_register: 0,
			op1_register: 0,
			op2_register: 0,
			op3_register: 0,
			op4_register: 0,
			op0_index: 0,
			op1_index: 0,
			op2_index: 0,
			op3_index: 0,
			op4_index: 0,
		}
	}

	pub(self) fn new(mnemonic: &'a FormatterString, instruction: &Instruction, flags: u32) -> Self {
		let mut res = InstrOpInfo::default(mnemonic);

		const_assert_eq!(5, IcedConstants::MAX_OP_COUNT);
		res.flags = flags as u16;
		res.op0_kind = unsafe { mem::transmute(instruction.op0_kind() as u8) };
		res.op1_kind = unsafe { mem::transmute(instruction.op1_kind() as u8) };
		res.op2_kind = unsafe { mem::transmute(instruction.op2_kind() as u8) };
		res.op3_kind = unsafe { mem::transmute(instruction.op3_kind() as u8) };
		res.op4_kind = unsafe { mem::transmute(instruction.op4_kind() as u8) };
		const_assert_eq!(8, InstrOpInfo::TEST_REGISTER_BITS);
		res.op0_register = instruction.op0_register() as u8;
		const_assert_eq!(8, InstrOpInfo::TEST_REGISTER_BITS);
		res.op1_register = instruction.op1_register() as u8;
		const_assert_eq!(8, InstrOpInfo::TEST_REGISTER_BITS);
		res.op2_register = instruction.op2_register() as u8;
		const_assert_eq!(8, InstrOpInfo::TEST_REGISTER_BITS);
		res.op3_register = instruction.op3_register() as u8;
		const_assert_eq!(8, InstrOpInfo::TEST_REGISTER_BITS);
		res.op4_register = instruction.op4_register() as u8;
		let op_count = instruction.op_count();
		res.op_count = op_count as u8;
		match op_count {
			0 => {
				res.op0_index = OP_ACCESS_INVALID;
				res.op1_index = OP_ACCESS_INVALID;
				res.op2_index = OP_ACCESS_INVALID;
				res.op3_index = OP_ACCESS_INVALID;
				res.op4_index = OP_ACCESS_INVALID;
			}

			1 => {
				res.op1_index = OP_ACCESS_INVALID;
				res.op2_index = OP_ACCESS_INVALID;
				res.op3_index = OP_ACCESS_INVALID;
				res.op4_index = OP_ACCESS_INVALID;
			}

			2 => {
				res.op1_index = 1;
				res.op2_index = OP_ACCESS_INVALID;
				res.op3_index = OP_ACCESS_INVALID;
				res.op4_index = OP_ACCESS_INVALID;
			}

			3 => {
				res.op1_index = 1;
				res.op2_index = 2;
				res.op3_index = OP_ACCESS_INVALID;
				res.op4_index = OP_ACCESS_INVALID;
			}

			4 => {
				res.op1_index = 1;
				res.op2_index = 2;
				res.op3_index = 3;
				res.op4_index = OP_ACCESS_INVALID;
			}

			5 => {
				res.op1_index = 1;
				res.op2_index = 2;
				res.op3_index = 3;
				res.op4_index = 4;
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
	pub const OP_ACCESS_COND_READ: i8 = -(OpAccess::CondRead as i8 + 2);
	pub const OP_ACCESS_READ_WRITE: i8 = -(OpAccess::ReadWrite as i8 + 2);
}
#[cfg(not(feature = "instr_info"))]
impl InstrInfoConstants {
	pub const OP_ACCESS_NONE: i8 = OP_ACCESS_INVALID;
	pub const OP_ACCESS_READ: i8 = OP_ACCESS_INVALID;
	pub const OP_ACCESS_COND_READ: i8 = OP_ACCESS_INVALID;
	pub const OP_ACCESS_READ_WRITE: i8 = OP_ACCESS_INVALID;
}

pub(super) trait InstrInfo {
	fn op_info<'a>(&'a self, options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a>;
}

fn get_bitness(code_size: CodeSize) -> u32 {
	match code_size {
		CodeSize::Code16 => 16,
		CodeSize::Code32 => 32,
		CodeSize::Code64 => 64,
		_ => 0,
	}
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
	flags: u32,
}

impl SimpleInstrInfo_cc {
	pub(super) fn new(cc_index: u32, mnemonics: Vec<String>) -> Self {
		Self { mnemonics: FormatterString::with_strings(mnemonics), cc_index, flags: InstrOpInfoFlags::NONE }
	}
	pub(super) fn with_flags(cc_index: u32, mnemonics: Vec<String>, flags: u32) -> Self {
		Self { mnemonics: FormatterString::with_strings(mnemonics), cc_index, flags }
	}
}

impl InstrInfo for SimpleInstrInfo_cc {
	fn op_info<'a>(&'a self, options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mnemonic = get_mnemonic_cc(options, self.cc_index, &self.mnemonics);
		InstrOpInfo::new(mnemonic, instruction, self.flags)
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_memsize {
	mnemonic: FormatterString,
	bitness: u32,
}

impl SimpleInstrInfo_memsize {
	pub(super) fn new(bitness: u32, mnemonic: String) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), bitness }
	}
}

impl InstrInfo for SimpleInstrInfo_memsize {
	fn op_info<'a>(&'a self, _options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let instr_bitness = get_bitness(instruction.code_size());
		let flags = if instr_bitness == 0 || (instr_bitness & self.bitness) != 0 {
			InstrOpInfoFlags::MEM_SIZE_NOTHING
		} else {
			InstrOpInfoFlags::MEM_SIZE_NORMAL | InstrOpInfoFlags::SHOW_NO_MEM_SIZE_FORCE_SIZE | InstrOpInfoFlags::SHOW_MIN_MEM_SIZE_FORCE_SIZE
		};
		InstrOpInfo::new(&self.mnemonic, instruction, flags)
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
pub(super) struct SimpleInstrInfo_Int3 {
	mnemonic: FormatterString,
}

impl SimpleInstrInfo_Int3 {
	pub(super) fn new(mnemonic: String) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic) }
	}
}

impl InstrInfo for SimpleInstrInfo_Int3 {
	fn op_info<'a>(&'a self, _options: &FormatterOptions, _instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut info = InstrOpInfo::default(&self.mnemonic);
		info.op_count = 1;
		info.op0_kind = InstrOpKind::ExtraImmediate8_Value3;
		info.op0_index = InstrInfoConstants::OP_ACCESS_READ;
		info
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_YD {
	mnemonic_args: FormatterString,
	mnemonic_no_args: FormatterString,
}

impl SimpleInstrInfo_YD {
	pub(super) fn new(mnemonic_args: String, mnemonic_no_args: String) -> Self {
		Self { mnemonic_args: FormatterString::new(mnemonic_args), mnemonic_no_args: FormatterString::new(mnemonic_no_args) }
	}
}

impl InstrInfo for SimpleInstrInfo_YD {
	fn op_info<'a>(&'a self, _options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		const FLAGS: u32 = InstrOpInfoFlags::SHOW_NO_MEM_SIZE_FORCE_SIZE | InstrOpInfoFlags::SHOW_MIN_MEM_SIZE_FORCE_SIZE;
		let short_form_op_kind = match instruction.code_size() {
			CodeSize::Unknown => instruction.op0_kind(),
			CodeSize::Code16 => OpKind::MemoryESDI,
			CodeSize::Code32 => OpKind::MemoryESEDI,
			CodeSize::Code64 => OpKind::MemoryESRDI,
		};
		let short_form = instruction.op0_kind() == short_form_op_kind;
		let mut info;
		if !short_form {
			info = InstrOpInfo::new(&self.mnemonic_args, instruction, FLAGS);
		} else {
			info = InstrOpInfo::default(&self.mnemonic_no_args);
			info.flags = FLAGS as u16;
		}
		info
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_DX {
	mnemonic_args: FormatterString,
	mnemonic_no_args: FormatterString,
}

impl SimpleInstrInfo_DX {
	pub(super) fn new(mnemonic_args: String, mnemonic_no_args: String) -> Self {
		Self { mnemonic_args: FormatterString::new(mnemonic_args), mnemonic_no_args: FormatterString::new(mnemonic_no_args) }
	}
}

impl InstrInfo for SimpleInstrInfo_DX {
	fn op_info<'a>(&'a self, options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		const FLAGS: u32 = InstrOpInfoFlags::SHOW_NO_MEM_SIZE_FORCE_SIZE | InstrOpInfoFlags::SHOW_MIN_MEM_SIZE_FORCE_SIZE;
		let short_form_op_kind = match instruction.code_size() {
			CodeSize::Unknown => instruction.op1_kind(),
			CodeSize::Code16 => OpKind::MemorySegSI,
			CodeSize::Code32 => OpKind::MemorySegESI,
			CodeSize::Code64 => OpKind::MemorySegRSI,
		};
		let mut info;
		let short_form = instruction.op1_kind() == short_form_op_kind
			&& (instruction.segment_prefix() == Register::None || !show_segment_prefix(Register::None, instruction, options));
		if !short_form {
			info = InstrOpInfo::new(&self.mnemonic_args, instruction, FLAGS);
		} else {
			info = InstrOpInfo::default(&self.mnemonic_no_args);
			info.flags = FLAGS as u16;
		}
		info
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_YX {
	mnemonic_args: FormatterString,
	mnemonic_no_args: FormatterString,
}

impl SimpleInstrInfo_YX {
	pub(super) fn new(mnemonic_args: String, mnemonic_no_args: String) -> Self {
		Self { mnemonic_args: FormatterString::new(mnemonic_args), mnemonic_no_args: FormatterString::new(mnemonic_no_args) }
	}
}

impl InstrInfo for SimpleInstrInfo_YX {
	fn op_info<'a>(&'a self, options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		const FLAGS: u32 = InstrOpInfoFlags::SHOW_NO_MEM_SIZE_FORCE_SIZE | InstrOpInfoFlags::SHOW_MIN_MEM_SIZE_FORCE_SIZE;
		let short_form_op_kind = match instruction.code_size() {
			CodeSize::Unknown => instruction.op0_kind(),
			CodeSize::Code16 => OpKind::MemoryESDI,
			CodeSize::Code32 => OpKind::MemoryESEDI,
			CodeSize::Code64 => OpKind::MemoryESRDI,
		};
		let mut info;
		let short_form = instruction.op0_kind() == short_form_op_kind
			&& (instruction.segment_prefix() == Register::None || !show_segment_prefix(Register::None, instruction, options));
		if !short_form {
			info = InstrOpInfo::new(&self.mnemonic_args, instruction, FLAGS);
		} else {
			info = InstrOpInfo::default(&self.mnemonic_no_args);
			info.flags = FLAGS as u16;
		}
		info
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_XY {
	mnemonic_args: FormatterString,
	mnemonic_no_args: FormatterString,
}

impl SimpleInstrInfo_XY {
	pub(super) fn new(mnemonic_args: String, mnemonic_no_args: String) -> Self {
		Self { mnemonic_args: FormatterString::new(mnemonic_args), mnemonic_no_args: FormatterString::new(mnemonic_no_args) }
	}
}

impl InstrInfo for SimpleInstrInfo_XY {
	fn op_info<'a>(&'a self, options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		const FLAGS: u32 = InstrOpInfoFlags::SHOW_NO_MEM_SIZE_FORCE_SIZE | InstrOpInfoFlags::SHOW_MIN_MEM_SIZE_FORCE_SIZE;
		let short_form_op_kind = match instruction.code_size() {
			CodeSize::Unknown => instruction.op1_kind(),
			CodeSize::Code16 => OpKind::MemoryESDI,
			CodeSize::Code32 => OpKind::MemoryESEDI,
			CodeSize::Code64 => OpKind::MemoryESRDI,
		};
		let mut info;
		let short_form = instruction.op1_kind() == short_form_op_kind
			&& (instruction.segment_prefix() == Register::None || !show_segment_prefix(Register::None, instruction, options));
		if !short_form {
			info = InstrOpInfo::new(&self.mnemonic_args, instruction, FLAGS);
		} else {
			info = InstrOpInfo::default(&self.mnemonic_no_args);
			info.flags = FLAGS as u16;
		}
		info
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_YA {
	mnemonic_args: FormatterString,
	mnemonic_no_args: FormatterString,
}

impl SimpleInstrInfo_YA {
	pub(super) fn new(mnemonic_args: String, mnemonic_no_args: String) -> Self {
		Self { mnemonic_args: FormatterString::new(mnemonic_args), mnemonic_no_args: FormatterString::new(mnemonic_no_args) }
	}
}

impl InstrInfo for SimpleInstrInfo_YA {
	fn op_info<'a>(&'a self, _options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		const FLAGS: u32 = InstrOpInfoFlags::SHOW_NO_MEM_SIZE_FORCE_SIZE | InstrOpInfoFlags::SHOW_MIN_MEM_SIZE_FORCE_SIZE;
		let short_form_op_kind = match instruction.code_size() {
			CodeSize::Unknown => instruction.op0_kind(),
			CodeSize::Code16 => OpKind::MemoryESDI,
			CodeSize::Code32 => OpKind::MemoryESEDI,
			CodeSize::Code64 => OpKind::MemoryESRDI,
		};
		let mut info;
		let short_form = instruction.op0_kind() == short_form_op_kind;
		if !short_form {
			info = InstrOpInfo::default(&self.mnemonic_args);
			info.flags = FLAGS as u16;
			info.op_count = 1;
			info.op0_kind = unsafe { mem::transmute(instruction.op0_kind() as u8) };
		} else {
			info = InstrOpInfo::default(&self.mnemonic_no_args);
			info.flags = FLAGS as u16;
		}
		info
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_AX {
	mnemonic_args: FormatterString,
	mnemonic_no_args: FormatterString,
}

impl SimpleInstrInfo_AX {
	pub(super) fn new(mnemonic_args: String, mnemonic_no_args: String) -> Self {
		Self { mnemonic_args: FormatterString::new(mnemonic_args), mnemonic_no_args: FormatterString::new(mnemonic_no_args) }
	}
}

impl InstrInfo for SimpleInstrInfo_AX {
	fn op_info<'a>(&'a self, options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		const FLAGS: u32 = InstrOpInfoFlags::SHOW_NO_MEM_SIZE_FORCE_SIZE | InstrOpInfoFlags::SHOW_MIN_MEM_SIZE_FORCE_SIZE;
		let short_form_op_kind = match instruction.code_size() {
			CodeSize::Unknown => instruction.op1_kind(),
			CodeSize::Code16 => OpKind::MemorySegSI,
			CodeSize::Code32 => OpKind::MemorySegESI,
			CodeSize::Code64 => OpKind::MemorySegRSI,
		};
		let mut info;
		let short_form = instruction.op1_kind() == short_form_op_kind
			&& (instruction.segment_prefix() == Register::None || !show_segment_prefix(Register::None, instruction, options));
		if !short_form {
			info = InstrOpInfo::default(&self.mnemonic_args);
			info.flags = FLAGS as u16;
			info.op_count = 1;
			info.op0_kind = unsafe { mem::transmute(instruction.op1_kind() as u8) };
			info.op0_index = 1;
		} else {
			info = InstrOpInfo::default(&self.mnemonic_no_args);
			info.flags = FLAGS as u16;
		}
		info
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_AY {
	mnemonic_args: FormatterString,
	mnemonic_no_args: FormatterString,
}

impl SimpleInstrInfo_AY {
	pub(super) fn new(mnemonic_args: String, mnemonic_no_args: String) -> Self {
		Self { mnemonic_args: FormatterString::new(mnemonic_args), mnemonic_no_args: FormatterString::new(mnemonic_no_args) }
	}
}

impl InstrInfo for SimpleInstrInfo_AY {
	fn op_info<'a>(&'a self, options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		const FLAGS: u32 = InstrOpInfoFlags::SHOW_NO_MEM_SIZE_FORCE_SIZE | InstrOpInfoFlags::SHOW_MIN_MEM_SIZE_FORCE_SIZE;
		let short_form_op_kind = match instruction.code_size() {
			CodeSize::Unknown => instruction.op1_kind(),
			CodeSize::Code16 => OpKind::MemoryESDI,
			CodeSize::Code32 => OpKind::MemoryESEDI,
			CodeSize::Code64 => OpKind::MemoryESRDI,
		};
		let mut info;
		let short_form = instruction.op1_kind() == short_form_op_kind
			&& (instruction.segment_prefix() == Register::None || !show_segment_prefix(Register::None, instruction, options));
		if !short_form {
			info = InstrOpInfo::default(&self.mnemonic_args);
			info.flags = FLAGS as u16;
			info.op_count = 1;
			info.op0_kind = unsafe { mem::transmute(instruction.op1_kind() as u8) };
			info.op0_index = 1;
		} else {
			info = InstrOpInfo::default(&self.mnemonic_no_args);
			info.flags = FLAGS as u16;
		}
		info
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_XLAT {
	mnemonic_args: FormatterString,
	mnemonic_no_args: FormatterString,
}

impl SimpleInstrInfo_XLAT {
	pub(super) fn new(mnemonic_args: String, mnemonic_no_args: String) -> Self {
		Self { mnemonic_args: FormatterString::new(mnemonic_args), mnemonic_no_args: FormatterString::new(mnemonic_no_args) }
	}
}

impl InstrInfo for SimpleInstrInfo_XLAT {
	fn op_info<'a>(&'a self, options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let base_reg = match instruction.code_size() {
			CodeSize::Unknown => instruction.memory_base(),
			CodeSize::Code16 => Register::BX,
			CodeSize::Code32 => Register::EBX,
			CodeSize::Code64 => Register::RBX,
		};
		let short_form = instruction.memory_base() == base_reg
			&& (instruction.segment_prefix() == Register::None || !show_segment_prefix(Register::None, instruction, options));
		if !short_form {
			InstrOpInfo::new(&self.mnemonic_args, instruction, InstrOpInfoFlags::SHOW_NO_MEM_SIZE_FORCE_SIZE | InstrOpInfoFlags::IGNORE_INDEX_REG)
		} else {
			InstrOpInfo::default(&self.mnemonic_no_args)
		}
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
			const_assert_eq!(0, InstrOpKind::Register as u32);
			//info.op0_kind = InstrOpKind::Register;
			//info.op1_kind = InstrOpKind::Register;
			const_assert_eq!(8, InstrOpInfo::TEST_REGISTER_BITS);
			info.op0_register = self.register as u8;
			const_assert_eq!(8, InstrOpInfo::TEST_REGISTER_BITS);
			info.op1_register = self.register as u8;
			info.op0_index = InstrInfoConstants::OP_ACCESS_NONE;
			info.op1_index = InstrInfoConstants::OP_ACCESS_NONE;
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
	#[cfg_attr(feature = "cargo-clippy", allow(clippy::nonminimal_bool))]
	fn op_info<'a>(&'a self, options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut info = InstrOpInfo::default(&self.mnemonic);
		debug_assert_eq!(2, instruction.op_count());
		debug_assert!(instruction.op0_kind() == OpKind::Register && instruction.op0_register() == Register::ST0);
		if !self.pseudo_op || !(options.use_pseudo_ops() && instruction.op1_register() == Register::ST1) {
			info.op_count = 1;
			const_assert_eq!(0, InstrOpKind::Register as u32);
			//info.op0_kind = InstrOpKind::Register;
			const_assert_eq!(8, InstrOpInfo::TEST_REGISTER_BITS);
			info.op0_register = instruction.op1_register() as u8;
			info.op0_index = 1;
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
		const FLAGS: u32 = 0;
		let mut info;
		if self.pseudo_op && options.use_pseudo_ops() && (instruction.op0_register() == Register::ST1 || instruction.op1_register() == Register::ST1)
		{
			info = InstrOpInfo::default(&self.mnemonic);
		} else {
			info = InstrOpInfo::new(&self.mnemonic, instruction, FLAGS);
			debug_assert_eq!(Register::ST0 as u8, info.op1_register);
			const_assert_eq!(8, InstrOpInfo::TEST_REGISTER_BITS);
			info.op1_register = Registers::REGISTER_ST as u8;
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
		debug_assert_eq!(Register::ST0 as u8, info.op0_register);
		const_assert_eq!(8, InstrOpInfo::TEST_REGISTER_BITS);
		info.op0_register = Registers::REGISTER_ST as u8;
		info
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_monitor {
	mnemonic: FormatterString,
	register1: Register,
	register2: Register,
	register3: Register,
}

impl SimpleInstrInfo_monitor {
	pub(super) fn new(mnemonic: String, register1: Register, register2: Register, register3: Register) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), register1, register2, register3 }
	}
}

impl InstrInfo for SimpleInstrInfo_monitor {
	fn op_info<'a>(&'a self, _options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut info = InstrOpInfo::default(&self.mnemonic);
		info.op_count = 3;
		info.op0_kind = InstrOpKind::Register;
		const_assert_eq!(8, InstrOpInfo::TEST_REGISTER_BITS);
		info.op0_register = self.register1 as u8;
		info.op1_kind = InstrOpKind::Register;
		const_assert_eq!(8, InstrOpInfo::TEST_REGISTER_BITS);
		info.op1_register = self.register2 as u8;
		info.op2_kind = InstrOpKind::Register;
		const_assert_eq!(8, InstrOpInfo::TEST_REGISTER_BITS);
		info.op2_register = self.register3 as u8;
		info.op0_index = InstrInfoConstants::OP_ACCESS_READ;
		info.op1_index = InstrInfoConstants::OP_ACCESS_READ;
		info.op2_index = InstrInfoConstants::OP_ACCESS_READ;
		if (instruction.code_size() == CodeSize::Code64 || instruction.code_size() == CodeSize::Unknown)
			&& (Register::EAX <= self.register2 && self.register2 <= Register::R15D)
		{
			info.op1_register += 0x10;
			info.op2_register += 0x10;
		}
		info
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_mwait {
	mnemonic: FormatterString,
}

impl SimpleInstrInfo_mwait {
	pub(super) fn new(mnemonic: String) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic) }
	}
}

impl InstrInfo for SimpleInstrInfo_mwait {
	fn op_info<'a>(&'a self, _options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut info = InstrOpInfo::default(&self.mnemonic);
		info.op_count = 2;
		info.op0_kind = InstrOpKind::Register;
		info.op1_kind = InstrOpKind::Register;
		info.op0_index = InstrInfoConstants::OP_ACCESS_READ;
		info.op1_index = InstrInfoConstants::OP_ACCESS_READ;

		match instruction.code_size() {
			CodeSize::Code16 => {
				const_assert_eq!(8, InstrOpInfo::TEST_REGISTER_BITS);
				info.op0_register = Register::AX as u8;
				const_assert_eq!(8, InstrOpInfo::TEST_REGISTER_BITS);
				info.op1_register = Register::ECX as u8;
			}
			CodeSize::Code32 => {
				const_assert_eq!(8, InstrOpInfo::TEST_REGISTER_BITS);
				info.op0_register = Register::EAX as u8;
				const_assert_eq!(8, InstrOpInfo::TEST_REGISTER_BITS);
				info.op1_register = Register::ECX as u8;
			}
			CodeSize::Unknown | CodeSize::Code64 => {
				const_assert_eq!(8, InstrOpInfo::TEST_REGISTER_BITS);
				info.op0_register = Register::RAX as u8;
				const_assert_eq!(8, InstrOpInfo::TEST_REGISTER_BITS);
				info.op1_register = Register::RCX as u8;
			}
		}
		info
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_mwaitx {
	mnemonic: FormatterString,
}

impl SimpleInstrInfo_mwaitx {
	pub(super) fn new(mnemonic: String) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic) }
	}
}

impl InstrInfo for SimpleInstrInfo_mwaitx {
	fn op_info<'a>(&'a self, _options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut info = InstrOpInfo::default(&self.mnemonic);
		info.op_count = 3;
		info.op0_kind = InstrOpKind::Register;
		info.op1_kind = InstrOpKind::Register;
		info.op2_kind = InstrOpKind::Register;
		info.op0_index = InstrInfoConstants::OP_ACCESS_READ;
		info.op1_index = InstrInfoConstants::OP_ACCESS_READ;
		info.op2_index = InstrInfoConstants::OP_ACCESS_COND_READ;

		match instruction.code_size() {
			CodeSize::Code16 => {
				const_assert_eq!(8, InstrOpInfo::TEST_REGISTER_BITS);
				info.op0_register = Register::AX as u8;
				const_assert_eq!(8, InstrOpInfo::TEST_REGISTER_BITS);
				info.op1_register = Register::ECX as u8;
				const_assert_eq!(8, InstrOpInfo::TEST_REGISTER_BITS);
				info.op2_register = Register::EBX as u8;
			}
			CodeSize::Code32 => {
				const_assert_eq!(8, InstrOpInfo::TEST_REGISTER_BITS);
				info.op0_register = Register::EAX as u8;
				const_assert_eq!(8, InstrOpInfo::TEST_REGISTER_BITS);
				info.op1_register = Register::ECX as u8;
				const_assert_eq!(8, InstrOpInfo::TEST_REGISTER_BITS);
				info.op2_register = Register::EBX as u8;
			}
			CodeSize::Unknown | CodeSize::Code64 => {
				const_assert_eq!(8, InstrOpInfo::TEST_REGISTER_BITS);
				info.op0_register = Register::RAX as u8;
				const_assert_eq!(8, InstrOpInfo::TEST_REGISTER_BITS);
				info.op1_register = Register::RCX as u8;
				const_assert_eq!(8, InstrOpInfo::TEST_REGISTER_BITS);
				info.op2_register = Register::RBX as u8;
			}
		}
		info
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_maskmovq {
	mnemonic: FormatterString,
	flags: u32,
}

impl SimpleInstrInfo_maskmovq {
	pub(super) fn new(mnemonic: String, flags: u32) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), flags }
	}
}

impl InstrInfo for SimpleInstrInfo_maskmovq {
	fn op_info<'a>(&'a self, options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		debug_assert_eq!(3, instruction.op_count());
		let short_form_op_kind = match instruction.code_size() {
			CodeSize::Unknown => instruction.op0_kind(),
			CodeSize::Code16 => OpKind::MemorySegDI,
			CodeSize::Code32 => OpKind::MemorySegEDI,
			CodeSize::Code64 => OpKind::MemorySegRDI,
		};
		let mut info;
		let short_form = instruction.op0_kind() == short_form_op_kind
			&& (instruction.segment_prefix() == Register::None || !show_segment_prefix(Register::None, instruction, options));
		if !short_form {
			info = InstrOpInfo::new(&self.mnemonic, instruction, self.flags);
		} else {
			info = InstrOpInfo::default(&self.mnemonic);
			info.flags = self.flags as u16;
			info.op_count = 2;
			info.op0_kind = unsafe { mem::transmute(instruction.op1_kind() as u8) };
			info.op0_index = 1;
			const_assert_eq!(8, InstrOpInfo::TEST_REGISTER_BITS);
			info.op0_register = instruction.op1_register() as u8;
			info.op1_kind = unsafe { mem::transmute(instruction.op2_kind() as u8) };
			info.op1_index = 2;
			const_assert_eq!(8, InstrOpInfo::TEST_REGISTER_BITS);
			info.op1_register = instruction.op2_register() as u8;
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
		debug_assert_eq!(2, instruction.op_count());
		info.op_count = 3;
		info.op0_kind = unsafe { mem::transmute(instruction.op0_kind() as u8) };
		const_assert_eq!(8, InstrOpInfo::TEST_REGISTER_BITS);
		info.op0_register = instruction.op0_register() as u8;
		info.op1_kind = unsafe { mem::transmute(instruction.op1_kind() as u8) };
		info.op1_index = 1;
		const_assert_eq!(8, InstrOpInfo::TEST_REGISTER_BITS);
		info.op1_register = instruction.op1_register() as u8;
		info.op2_kind = InstrOpKind::Register;
		const_assert_eq!(8, InstrOpInfo::TEST_REGISTER_BITS);
		info.op2_register = Register::XMM0 as u8;
		info.op2_index = InstrInfoConstants::OP_ACCESS_READ;
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
		debug_assert_eq!(2, instruction.op_count());
		info.op_count = 2;
		info.op0_kind = unsafe { mem::transmute(instruction.op1_kind() as u8) };
		info.op0_index = 1;
		const_assert_eq!(8, InstrOpInfo::TEST_REGISTER_BITS);
		info.op0_register = instruction.op1_register() as u8;
		info.op1_kind = unsafe { mem::transmute(instruction.op0_kind() as u8) };
		const_assert_eq!(8, InstrOpInfo::TEST_REGISTER_BITS);
		info.op1_register = instruction.op0_register() as u8;
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
		const_assert_eq!(0, CodeSize::Unknown as u32);
		const_assert_eq!(1, CodeSize::Code16 as u32);
		const_assert_eq!(2, CodeSize::Code32 as u32);
		const_assert_eq!(3, CodeSize::Code64 as u32);
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
pub(super) struct SimpleInstrInfo_OpSize_cc {
	mnemonics: Vec<FormatterString>,
	mnemonics_other: Vec<FormatterString>,
	cc_index: u32,
	code_size: CodeSize,
}

impl SimpleInstrInfo_OpSize_cc {
	pub(super) fn new(code_size: CodeSize, cc_index: u32, mnemonics: Vec<String>, mnemonics_other: Vec<String>) -> Self {
		Self {
			mnemonics: FormatterString::with_strings(mnemonics),
			mnemonics_other: FormatterString::with_strings(mnemonics_other),
			cc_index,
			code_size,
		}
	}
}

impl InstrInfo for SimpleInstrInfo_OpSize_cc {
	fn op_info<'a>(&'a self, options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mnemonics = if instruction.code_size() == self.code_size { &self.mnemonics } else { &self.mnemonics_other };
		let mnemonic = get_mnemonic_cc(options, self.cc_index, mnemonics);
		InstrOpInfo::new(mnemonic, instruction, InstrOpInfoFlags::NONE)
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_OpSize2 {
	mnemonics: [FormatterString; 4],
	can_use_bnd: bool,
}

impl SimpleInstrInfo_OpSize2 {
	pub(super) fn new(mnemonic: String, mnemonic16: String, mnemonic32: String, mnemonic64: String, can_use_bnd: bool) -> Self {
		const_assert_eq!(0, CodeSize::Unknown as u32);
		const_assert_eq!(1, CodeSize::Code16 as u32);
		const_assert_eq!(2, CodeSize::Code32 as u32);
		const_assert_eq!(3, CodeSize::Code64 as u32);
		Self {
			mnemonics: [
				FormatterString::new(mnemonic),
				FormatterString::new(mnemonic16),
				FormatterString::new(mnemonic32),
				FormatterString::new(mnemonic64),
			],
			can_use_bnd,
		}
	}
}

impl InstrInfo for SimpleInstrInfo_OpSize2 {
	fn op_info<'a>(&'a self, _options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut flags = InstrOpInfoFlags::NONE;
		if self.can_use_bnd && instruction.has_repne_prefix() {
			flags |= InstrOpInfoFlags::BND_PREFIX;
		}
		let mnemonic = &self.mnemonics[instruction.code_size() as usize];
		InstrOpInfo::new(mnemonic, instruction, flags)
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_fword {
	mnemonic: FormatterString,
	mnemonic2: FormatterString,
	code_size: CodeSize,
	flags: u32,
}

impl SimpleInstrInfo_fword {
	pub(super) fn new(code_size: CodeSize, flags: u32, mnemonic: String, mnemonic2: String) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), mnemonic2: FormatterString::new(mnemonic2), code_size, flags }
	}
}

impl InstrInfo for SimpleInstrInfo_fword {
	fn op_info<'a>(&'a self, _options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mnemonic =
			if instruction.code_size() == self.code_size || instruction.code_size() == CodeSize::Unknown { &self.mnemonic } else { &self.mnemonic2 };
		InstrOpInfo::new(mnemonic, instruction, self.flags)
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_jcc {
	mnemonics: Vec<FormatterString>,
	cc_index: u32,
}

impl SimpleInstrInfo_jcc {
	pub(super) fn new(cc_index: u32, mnemonics: Vec<String>) -> Self {
		Self { mnemonics: FormatterString::with_strings(mnemonics), cc_index }
	}
}

impl InstrInfo for SimpleInstrInfo_jcc {
	fn op_info<'a>(&'a self, options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut flags = InstrOpInfoFlags::NONE;
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
	flags: u32,
}

impl SimpleInstrInfo_pops {
	pub(super) fn with_mnemonic(mnemonic: String, pseudo_ops: &'static [FormatterString]) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), pseudo_ops, flags: InstrOpInfoFlags::NONE }
	}
	pub(super) fn new(mnemonic: String, pseudo_ops: &'static [FormatterString], flags: u32) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), pseudo_ops, flags }
	}

	fn remove_last_op(info: &mut InstrOpInfo) {
		match info.op_count {
			4 => info.op3_index = OP_ACCESS_INVALID,
			3 => info.op2_index = OP_ACCESS_INVALID,
			_ => unreachable!(),
		}
		info.op_count -= 1;
	}
}

impl InstrInfo for SimpleInstrInfo_pops {
	fn op_info<'a>(&'a self, options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut info = InstrOpInfo::new(&self.mnemonic, instruction, self.flags);
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
pub(super) struct SimpleInstrInfo_imul {
	mnemonic: FormatterString,
}

impl SimpleInstrInfo_imul {
	pub(super) fn new(mnemonic: String) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic) }
	}
}

impl InstrInfo for SimpleInstrInfo_imul {
	fn op_info<'a>(&'a self, options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut info = InstrOpInfo::new(&self.mnemonic, instruction, InstrOpInfoFlags::NONE);
		debug_assert_eq!(3, info.op_count);
		if options.use_pseudo_ops()
			&& info.op0_kind == InstrOpKind::Register
			&& info.op1_kind == InstrOpKind::Register
			&& info.op0_register == info.op1_register
		{
			info.op_count -= 1;
			info.op0_index = InstrInfoConstants::OP_ACCESS_READ_WRITE;
			info.op1_kind = info.op2_kind;
			info.op1_index = 2;
			info.op2_index = OP_ACCESS_INVALID;
		}
		info
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_Reg16 {
	mnemonic: FormatterString,
	flags: u32,
}

impl SimpleInstrInfo_Reg16 {
	pub(super) fn new(mnemonic: String, flags: u32) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), flags }
	}
}

impl InstrInfo for SimpleInstrInfo_Reg16 {
	fn op_info<'a>(&'a self, _options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut info = InstrOpInfo::new(&self.mnemonic, instruction, self.flags);
		if Register::EAX as u8 <= info.op0_register && info.op0_register <= Register::R15 as u8 {
			const_assert_eq!(8, InstrOpInfo::TEST_REGISTER_BITS);
			info.op0_register = (info.op0_register.wrapping_sub(Register::EAX as u8) & 0xF).wrapping_add(Register::AX as u8);
		}
		if Register::EAX as u8 <= info.op1_register && info.op1_register <= Register::R15 as u8 {
			const_assert_eq!(8, InstrOpInfo::TEST_REGISTER_BITS);
			info.op1_register = (info.op1_register.wrapping_sub(Register::EAX as u8) & 0xF).wrapping_add(Register::AX as u8);
		}
		info
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_Reg32 {
	mnemonic: FormatterString,
	flags: u32,
}

impl SimpleInstrInfo_Reg32 {
	pub(super) fn new(mnemonic: String, flags: u32) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), flags }
	}
}

impl InstrInfo for SimpleInstrInfo_Reg32 {
	fn op_info<'a>(&'a self, _options: &FormatterOptions, instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut info = InstrOpInfo::new(&self.mnemonic, instruction, self.flags);
		if Register::RAX as u8 <= info.op0_register && info.op0_register <= Register::R15 as u8 {
			const_assert_eq!(8, InstrOpInfo::TEST_REGISTER_BITS);
			info.op0_register = info.op0_register.wrapping_sub(Register::RAX as u8).wrapping_add(Register::EAX as u8);
		}
		if Register::RAX as u8 <= info.op2_register && info.op2_register <= Register::R15 as u8 {
			const_assert_eq!(8, InstrOpInfo::TEST_REGISTER_BITS);
			info.op2_register = info.op2_register.wrapping_sub(Register::RAX as u8).wrapping_add(Register::EAX as u8);
		}
		info
	}
}

#[allow(non_camel_case_types)]
pub(super) struct SimpleInstrInfo_reg {
	mnemonic: FormatterString,
	register: Register,
}

impl SimpleInstrInfo_reg {
	pub(super) fn new(mnemonic: String, register: Register) -> Self {
		Self { mnemonic: FormatterString::new(mnemonic), register }
	}
}

impl InstrInfo for SimpleInstrInfo_reg {
	fn op_info<'a>(&'a self, _options: &FormatterOptions, _instruction: &Instruction) -> InstrOpInfo<'a> {
		let mut info = InstrOpInfo::default(&self.mnemonic);
		info.op_count = 1;
		info.op0_kind = InstrOpKind::Register;
		const_assert_eq!(8, InstrOpInfo::TEST_REGISTER_BITS);
		info.op0_register = self.register as u8;
		info.op0_index = InstrInfoConstants::OP_ACCESS_READ;
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
		info.op0_kind = InstrOpKind::Register;
		info.op1_kind = InstrOpKind::Register;
		const_assert_eq!(8, InstrOpInfo::TEST_REGISTER_BITS);
		info.op1_register = Register::ECX as u8;
		info.op0_index = InstrInfoConstants::OP_ACCESS_READ;
		info.op1_index = InstrInfoConstants::OP_ACCESS_READ;

		match self.bitness {
			16 => {
				const_assert_eq!(8, InstrOpInfo::TEST_REGISTER_BITS);
				info.op0_register = Register::AX as u8;
			}

			32 => {
				const_assert_eq!(8, InstrOpInfo::TEST_REGISTER_BITS);
				info.op0_register = Register::EAX as u8;
			}

			64 => {
				const_assert_eq!(8, InstrOpInfo::TEST_REGISTER_BITS);
				info.op0_register = Register::RAX as u8;
			}

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
		info.op0_kind = self.op_kind;
		info.op1_kind = self.op_kind;
		info.op2_kind = self.op_kind;
		info.op3_kind = self.op_kind;
		info.op4_kind = self.op_kind;
		info.op0_index = InstrInfoConstants::OP_ACCESS_READ;
		info.op1_index = InstrInfoConstants::OP_ACCESS_READ;
		info.op2_index = InstrInfoConstants::OP_ACCESS_READ;
		info.op3_index = InstrInfoConstants::OP_ACCESS_READ;
		info.op4_index = InstrInfoConstants::OP_ACCESS_READ;
		info
	}
}
