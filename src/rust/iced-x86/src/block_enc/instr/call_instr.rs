// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::block_enc::instr::*;
use crate::block_enc::*;
use crate::iced_error::IcedError;
use core::cell::RefCell;
use core::cmp;

pub(super) struct CallInstr {
	bitness: u8,
	instruction: Instruction,
	target_instr: TargetInstr,
	orig_instruction_size: u8,
	pointer_data: Option<Rc<RefCell<BlockData>>>,
	use_orig_instruction: bool,
}

impl CallInstr {
	pub(super) fn new(block_encoder: &mut BlockEncInt, base: &mut InstrBase, instruction: &Instruction) -> Self {
		let mut instr_copy = *instruction;
		instr_copy.set_near_branch64(0);
		let orig_instruction_size = block_encoder.get_instruction_size(&instr_copy, 0) as u8;
		let mut use_orig_instruction = false;
		base.size = if !block_encoder.fix_branches() {
			use_orig_instruction = true;
			orig_instruction_size
		} else if block_encoder.bitness() == 64 {
			// Make sure it's not shorter than the real instruction. It can happen if there are extra prefixes.
			cmp::max(orig_instruction_size, InstrUtils::CALL_OR_JMP_POINTER_DATA_INSTRUCTION_SIZE64 as u8)
		} else {
			orig_instruction_size
		} as u32;
		Self {
			bitness: block_encoder.bitness() as u8,
			instruction: *instruction,
			target_instr: TargetInstr::default(),
			orig_instruction_size,
			pointer_data: None,
			use_orig_instruction,
		}
	}

	fn try_optimize(&mut self, base: &mut InstrBase, ctx: &mut InstrContext<'_>, gained: u64) -> bool {
		if base.done || self.use_orig_instruction {
			base.done = true;
			return false;
		}

		// If it's in the same block, we assume the target is at most 2GB away.
		let mut use_short = self.bitness != 64 || self.target_instr.is_in_block(ctx.block);
		if !use_short {
			let target_address = self.target_instr.address(ctx);
			let next_rip = ctx.ip.wrapping_add(self.orig_instruction_size as u64);
			let diff = target_address.wrapping_sub(next_rip) as i64;
			let diff = correct_diff(self.target_instr.is_in_block(ctx.block), diff, gained);
			use_short = i32::MIN as i64 <= diff && diff <= i32::MAX as i64;
		}

		if use_short {
			if let Some(ref pointer_data) = self.pointer_data {
				pointer_data.borrow_mut().is_valid = false;
			}
			base.size = self.orig_instruction_size as u32;
			self.use_orig_instruction = true;
			base.done = true;
			return true;
		}

		if self.pointer_data.is_none() {
			self.pointer_data = Some(ctx.block.alloc_pointer_location());
		}
		false
	}
}

impl Instr for CallInstr {
	fn get_target_instr(&mut self) -> (&mut TargetInstr, u64) {
		(&mut self.target_instr, self.instruction.near_branch_target())
	}

	fn optimize(&mut self, base: &mut InstrBase, ctx: &mut InstrContext<'_>, gained: u64) -> bool {
		self.try_optimize(base, ctx, gained)
	}

	fn encode(&mut self, base: &mut InstrBase, ctx: &mut InstrContext<'_>) -> Result<(ConstantOffsets, bool), IcedError> {
		if self.use_orig_instruction {
			self.instruction.set_near_branch64(self.target_instr.address(ctx));
			ctx.block.encoder.encode(&self.instruction, ctx.ip).map_or_else(
				|err| Err(IcedError::with_string(InstrUtils::create_error_message(err, &self.instruction))),
				|_| Ok((ctx.block.encoder.get_constant_offsets(), true)),
			)
		} else {
			debug_assert!(self.pointer_data.is_some());
			let pointer_data = self.pointer_data.clone().ok_or_else(|| IcedError::new("Internal error"))?;
			pointer_data.borrow_mut().data = self.target_instr.address(ctx);
			InstrUtils::encode_branch_to_pointer_data(ctx.block, true, ctx.ip, pointer_data, base.size).map_or_else(
				|err| Err(IcedError::with_string(InstrUtils::create_error_message(err, &self.instruction))),
				|_| Ok((ConstantOffsets::default(), false)),
			)
		}
	}
}
