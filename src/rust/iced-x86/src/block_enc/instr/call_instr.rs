// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use super::super::super::iced_error::IcedError;
use super::super::*;
use super::*;
use core::cell::RefCell;
use core::{cmp, i32};

pub(super) struct CallInstr {
	orig_ip: u64,
	ip: u64,
	block: Rc<RefCell<Block>>,
	size: u32,
	bitness: u32,
	instruction: Instruction,
	target_instr: TargetInstr,
	orig_instruction_size: u32,
	pointer_data: Option<Rc<RefCell<BlockData>>>,
	use_orig_instruction: bool,
	done: bool,
}

impl CallInstr {
	pub(super) fn new(block_encoder: &mut BlockEncoder, block: Rc<RefCell<Block>>, instruction: &Instruction) -> Self {
		let mut instr_copy = *instruction;
		instr_copy.set_near_branch64(0);
		let orig_instruction_size = block_encoder.get_instruction_size(&instr_copy, 0);
		let mut done = false;
		let mut use_orig_instruction = false;
		let size = if !block_encoder.fix_branches() {
			use_orig_instruction = true;
			done = true;
			orig_instruction_size
		} else if block_encoder.bitness() == 64 {
			// Make sure it's not shorter than the real instruction. It can happen if there are extra prefixes.
			cmp::max(orig_instruction_size, InstrUtils::CALL_OR_JMP_POINTER_DATA_INSTRUCTION_SIZE64)
		} else {
			orig_instruction_size
		};
		Self {
			orig_ip: instruction.ip(),
			ip: 0,
			block,
			size,
			bitness: block_encoder.bitness(),
			instruction: *instruction,
			target_instr: TargetInstr::default(),
			orig_instruction_size,
			pointer_data: None,
			use_orig_instruction,
			done,
		}
	}

	fn try_optimize(&mut self) -> bool {
		if self.done {
			return false;
		}

		// If it's in the same block, we assume the target is at most 2GB away.
		let mut use_short = self.bitness != 64 || self.target_instr.is_in_block(Rc::clone(&self.block));
		if !use_short {
			let target_address = self.target_instr.address(self);
			let next_rip = self.ip.wrapping_add(self.orig_instruction_size as u64);
			let diff = target_address.wrapping_sub(next_rip) as i64;
			use_short = i32::MIN as i64 <= diff && diff <= i32::MAX as i64;
		}

		if use_short {
			if let Some(ref pointer_data) = self.pointer_data {
				pointer_data.borrow_mut().is_valid = false;
			}
			self.size = self.orig_instruction_size;
			self.use_orig_instruction = true;
			self.done = true;
			return true;
		}

		if self.pointer_data.is_none() {
			self.pointer_data = Some(Rc::clone(&self.block).borrow_mut().alloc_pointer_location());
		}
		false
	}
}

impl Instr for CallInstr {
	fn block(&self) -> Rc<RefCell<Block>> {
		Rc::clone(&self.block)
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
		let _ = self.try_optimize();
	}

	fn optimize(&mut self) -> bool {
		self.try_optimize()
	}

	fn encode(&mut self, block: &mut Block) -> Result<(ConstantOffsets, bool), IcedError> {
		if self.use_orig_instruction {
			self.instruction.set_near_branch64(self.target_instr.address(self));
			block.encoder.encode(&self.instruction, self.ip).map_or_else(
				|err| Err(IcedError::with_string(InstrUtils::create_error_message(&err, &self.instruction))),
				|_| Ok((block.encoder.get_constant_offsets(), true)),
			)
		} else {
			debug_assert!(self.pointer_data.is_some());
			let pointer_data = self.pointer_data.clone().ok_or_else(|| IcedError::new("Internal error"))?;
			pointer_data.borrow_mut().data = self.target_instr.address(self);
			InstrUtils::encode_branch_to_pointer_data(block, true, self.ip, pointer_data, self.size).map_or_else(
				|err| Err(IcedError::with_string(InstrUtils::create_error_message(&err, &self.instruction))),
				|_| Ok((ConstantOffsets::default(), false)),
			)
		}
	}
}
