// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::block_enc::instr::*;
use crate::block_enc::*;
use crate::iced_error::IcedError;
use core::cell::RefCell;
use core::cmp;

#[derive(Debug, Copy, Clone, Eq, PartialEq, Ord, PartialOrd)]
enum InstrKind {
	Unchanged,
	Short,
	Near,
	Long,
	Uninitialized,
}

pub(super) struct JmpInstr {
	orig_ip: u64,
	size: u32,
	bitness: u32,
	instruction: Instruction,
	target_instr: TargetInstr,
	pointer_data: Option<Rc<RefCell<BlockData>>>,
	instr_kind: InstrKind,
	short_instruction_size: u32,
	near_instruction_size: u32,
}

impl JmpInstr {
	pub(super) fn new(block_encoder: &mut BlockEncInt, instruction: &Instruction) -> Self {
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
			instr_copy.set_code(instruction.code().as_short_branch());
			instr_copy.set_near_branch64(0);
			short_instruction_size = block_encoder.get_instruction_size(&instr_copy, 0);

			instr_copy = *instruction;
			instr_copy.set_code(instruction.code().as_near_branch());
			instr_copy.set_near_branch64(0);
			near_instruction_size = block_encoder.get_instruction_size(&instr_copy, 0);

			size = if block_encoder.bitness() == 64 {
				// Make sure it's not shorter than the real instruction. It can happen if there are extra prefixes.
				cmp::max(near_instruction_size, InstrUtils::CALL_OR_JMP_POINTER_DATA_INSTRUCTION_SIZE64)
			} else {
				near_instruction_size
			}
		}
		Self {
			orig_ip: instruction.ip(),
			size,
			bitness: block_encoder.bitness(),
			instruction: *instruction,
			target_instr: TargetInstr::default(),
			pointer_data: None,
			instr_kind,
			short_instruction_size,
			near_instruction_size,
		}
	}

	fn try_optimize<'a>(&mut self, ctx: &mut InstrContext<'a>, gained: u64) -> bool {
		if self.instr_kind == InstrKind::Unchanged || self.instr_kind == InstrKind::Short {
			return false;
		}

		let mut target_address = self.target_instr.address(ctx);
		let mut next_rip = ctx.ip.wrapping_add(self.short_instruction_size as u64);
		let mut diff = target_address.wrapping_sub(next_rip) as i64;
		diff = correct_diff(self.target_instr.is_in_block(ctx.block), diff, gained);
		if i8::MIN as i64 <= diff && diff <= i8::MAX as i64 {
			if let Some(ref pointer_data) = self.pointer_data {
				pointer_data.borrow_mut().is_valid = false;
			}
			self.instr_kind = InstrKind::Short;
			self.size = self.short_instruction_size;
			return true;
		}

		// If it's in the same block, we assume the target is at most 2GB away.
		let mut use_near = self.bitness != 64 || self.target_instr.is_in_block(ctx.block);
		if !use_near {
			target_address = self.target_instr.address(ctx);
			next_rip = ctx.ip.wrapping_add(self.near_instruction_size as u64);
			diff = target_address.wrapping_sub(next_rip) as i64;
			diff = correct_diff(self.target_instr.is_in_block(ctx.block), diff, gained);
			use_near = i32::MIN as i64 <= diff && diff <= i32::MAX as i64;
		}
		if use_near {
			if let Some(ref pointer_data) = self.pointer_data {
				pointer_data.borrow_mut().is_valid = false;
			}
			self.instr_kind = InstrKind::Near;
			self.size = self.near_instruction_size;
			return true;
		}

		if self.pointer_data.is_none() {
			self.pointer_data = Some(ctx.block.alloc_pointer_location());
		}
		self.instr_kind = InstrKind::Long;
		false
	}
}

impl Instr for JmpInstr {
	fn size(&self) -> u32 {
		self.size
	}

	fn orig_ip(&self) -> u64 {
		self.orig_ip
	}

	fn initialize<'a>(&mut self, block_encoder: &BlockEncInt, ctx: &mut InstrContext<'a>) {
		self.target_instr = block_encoder.get_target(self, self.instruction.near_branch_target());
		let _ = self.try_optimize(ctx, 0);
	}

	fn optimize<'a>(&mut self, ctx: &mut InstrContext<'a>, gained: u64) -> bool {
		self.try_optimize(ctx, gained)
	}

	fn encode<'a>(&mut self, ctx: &mut InstrContext<'a>) -> Result<(ConstantOffsets, bool), IcedError> {
		match self.instr_kind {
			InstrKind::Unchanged | InstrKind::Short | InstrKind::Near => {
				if self.instr_kind == InstrKind::Unchanged {
					// nothing
				} else if self.instr_kind == InstrKind::Short {
					self.instruction.set_code(self.instruction.code().as_short_branch());
				} else {
					debug_assert!(self.instr_kind == InstrKind::Near);
					self.instruction.set_code(self.instruction.code().as_near_branch());
				}
				self.instruction.set_near_branch64(self.target_instr.address(ctx));
				ctx.block.encoder.encode(&self.instruction, ctx.ip).map_or_else(
					|err| Err(IcedError::with_string(InstrUtils::create_error_message(&err, &self.instruction))),
					|_| Ok((ctx.block.encoder.get_constant_offsets(), true)),
				)
			}

			InstrKind::Long => {
				debug_assert!(self.pointer_data.is_some());
				let pointer_data = self.pointer_data.clone().ok_or_else(|| IcedError::new("Internal error"))?;
				pointer_data.borrow_mut().data = self.target_instr.address(ctx);
				InstrUtils::encode_branch_to_pointer_data(ctx.block, false, ctx.ip, pointer_data, self.size).map_or_else(
					|err| Err(IcedError::with_string(InstrUtils::create_error_message(&err, &self.instruction))),
					|_| Ok((ConstantOffsets::default(), false)),
				)
			}

			InstrKind::Uninitialized => unreachable!(),
		}
	}
}
