// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::block_enc::instr::*;
use crate::block_enc::*;
use crate::iced_error::IcedError;

#[derive(Debug, Copy, Clone, Eq, PartialEq, Ord, PartialOrd)]
enum InstrKind {
	Unchanged,
	Rel16,
	Rel32,
	Uninitialized,
}

pub(super) struct XbeginInstr {
	instruction: Instruction,
	target_instr: TargetInstr,
	instr_kind: InstrKind,
	short_instruction_size: u8,
	near_instruction_size: u8,
}

impl XbeginInstr {
	pub(super) fn new(block_encoder: &mut BlockEncInt, base: &mut InstrBase, instruction: &Instruction) -> Self {
		let mut instr_kind = InstrKind::Uninitialized;
		let mut instr_copy: Instruction;
		let short_instruction_size;
		let near_instruction_size;
		if !block_encoder.fix_branches() {
			instr_kind = InstrKind::Unchanged;
			instr_copy = *instruction;
			instr_copy.set_near_branch64(0);
			base.size = block_encoder.get_instruction_size(&instr_copy, 0);
			short_instruction_size = 0;
			near_instruction_size = 0;
		} else {
			instr_copy = *instruction;
			instr_copy.set_code(Code::Xbegin_rel16);
			instr_copy.set_near_branch64(0);
			short_instruction_size = block_encoder.get_instruction_size(&instr_copy, 0) as u8;

			instr_copy = *instruction;
			instr_copy.set_code(Code::Xbegin_rel32);
			instr_copy.set_near_branch64(0);
			near_instruction_size = block_encoder.get_instruction_size(&instr_copy, 0) as u8;

			base.size = near_instruction_size as u32;
		}
		Self { instruction: *instruction, target_instr: TargetInstr::default(), instr_kind, short_instruction_size, near_instruction_size }
	}

	fn try_optimize(&mut self, base: &mut InstrBase, ctx: &mut InstrContext<'_>, gained: u64) -> bool {
		if self.instr_kind == InstrKind::Unchanged || self.instr_kind == InstrKind::Rel16 {
			base.done = true;
			return false;
		}

		let target_address = self.target_instr.address(ctx);
		let next_rip = ctx.ip.wrapping_add(self.short_instruction_size as u64);
		let diff = target_address.wrapping_sub(next_rip) as i64;
		let diff = correct_diff(self.target_instr.is_in_block(ctx.block), diff, gained);
		if i16::MIN as i64 <= diff && diff <= i16::MAX as i64 {
			self.instr_kind = InstrKind::Rel16;
			base.size = self.short_instruction_size as u32;
			true
		} else {
			self.instr_kind = InstrKind::Rel32;
			base.size = self.near_instruction_size as u32;
			false
		}
	}
}

impl Instr for XbeginInstr {
	fn get_target_instr(&mut self) -> (&mut TargetInstr, u64) {
		(&mut self.target_instr, self.instruction.near_branch_target())
	}

	fn optimize(&mut self, base: &mut InstrBase, ctx: &mut InstrContext<'_>, gained: u64) -> bool {
		self.try_optimize(base, ctx, gained)
	}

	fn encode(&mut self, _base: &mut InstrBase, ctx: &mut InstrContext<'_>) -> Result<(ConstantOffsets, bool), IcedError> {
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
				self.instruction.set_near_branch64(self.target_instr.address(ctx));
				ctx.block.encoder.encode(&self.instruction, ctx.ip).map_or_else(
					|err| Err(IcedError::with_string(InstrUtils::create_error_message(err, &self.instruction))),
					|_| Ok((ctx.block.encoder.get_constant_offsets(), true)),
				)
			}

			InstrKind::Uninitialized => unreachable!(),
		}
	}
}
