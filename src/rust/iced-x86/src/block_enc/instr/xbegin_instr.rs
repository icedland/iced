// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::block_enc::instr::*;
use crate::block_enc::*;
use crate::iced_error::IcedError;
use core::cell::RefCell;
use core::{i16, u32};

#[derive(Debug, Copy, Clone, Eq, PartialEq, Ord, PartialOrd)]
enum InstrKind {
	Unchanged,
	Rel16,
	Rel32,
	Uninitialized,
}

pub(super) struct XbeginInstr {
	orig_ip: u64,
	ip: u64,
	block: Rc<RefCell<Block>>,
	size: u32,
	instruction: Instruction,
	target_instr: TargetInstr,
	instr_kind: InstrKind,
	short_instruction_size: u32,
	near_instruction_size: u32,
}

impl XbeginInstr {
	pub(super) fn new(block_encoder: &mut BlockEncoder, block: Rc<RefCell<Block>>, instruction: &Instruction) -> Self {
		let mut instr_kind = InstrKind::Uninitialized;
		let mut instr_copy: Instruction;
		let size;
		let short_instruction_size;
		let near_instruction_size;
		if !block_encoder.fix_branches() {
			instr_kind = InstrKind::Unchanged;
			instr_copy = *instruction;
			instr_copy.set_near_branch64(0);
			size = block_encoder.get_instruction_size(&instr_copy, 0);
			short_instruction_size = 0;
			near_instruction_size = 0;
		} else {
			instr_copy = *instruction;
			instr_copy.set_code(Code::Xbegin_rel16);
			instr_copy.set_near_branch64(0);
			short_instruction_size = block_encoder.get_instruction_size(&instr_copy, 0);

			instr_copy = *instruction;
			instr_copy.set_code(Code::Xbegin_rel32);
			instr_copy.set_near_branch64(0);
			near_instruction_size = block_encoder.get_instruction_size(&instr_copy, 0);

			size = near_instruction_size;
		}
		Self {
			orig_ip: instruction.ip(),
			ip: 0,
			block,
			size,
			instruction: *instruction,
			target_instr: TargetInstr::default(),
			instr_kind,
			short_instruction_size,
			near_instruction_size,
		}
	}

	fn try_optimize(&mut self, gained: u64) -> bool {
		if self.instr_kind == InstrKind::Unchanged || self.instr_kind == InstrKind::Rel16 {
			return false;
		}

		let target_address = self.target_instr.address(self);
		let next_rip = self.ip.wrapping_add(self.short_instruction_size as u64);
		let diff = target_address.wrapping_sub(next_rip) as i64;
		let diff = correct_diff(self.target_instr.is_in_block(self.block()), diff, gained);
		if i16::MIN as i64 <= diff && diff <= i16::MAX as i64 {
			self.instr_kind = InstrKind::Rel16;
			self.size = self.short_instruction_size;
			true
		} else {
			self.instr_kind = InstrKind::Rel32;
			self.size = self.near_instruction_size;
			false
		}
	}
}

impl Instr for XbeginInstr {
	fn block(&self) -> Rc<RefCell<Block>> {
		self.block.clone()
	}

	fn size(&self) -> u32 {
		self.size
	}

	fn ip(&self) -> u64 {
		self.ip
	}

	fn set_ip(&mut self, new_ip: u64) {
		self.ip = new_ip
	}

	fn orig_ip(&self) -> u64 {
		self.orig_ip
	}

	fn initialize(&mut self, block_encoder: &BlockEncoder) {
		self.target_instr = block_encoder.get_target(self, self.instruction.near_branch_target());
		let _ = self.try_optimize(0);
	}

	fn optimize(&mut self, gained: u64) -> bool {
		self.try_optimize(gained)
	}

	fn encode(&mut self, block: &mut Block) -> Result<(ConstantOffsets, bool), IcedError> {
		match self.instr_kind {
			InstrKind::Unchanged | InstrKind::Rel16 | InstrKind::Rel32 => {
				if self.instr_kind == InstrKind::Unchanged {
					// nothing
				} else if self.instr_kind == InstrKind::Rel16 {
					self.instruction.set_code(Code::Xbegin_rel16);
				} else {
					debug_assert!(self.instr_kind == InstrKind::Rel32);
					self.instruction.set_code(Code::Xbegin_rel32);
				}
				self.instruction.set_near_branch64(self.target_instr.address(self));
				block.encoder.encode(&self.instruction, self.ip).map_or_else(
					|err| Err(IcedError::with_string(InstrUtils::create_error_message(&err, &self.instruction))),
					|_| Ok((block.encoder.get_constant_offsets(), true)),
				)
			}

			InstrKind::Uninitialized => unreachable!(),
		}
	}
}
