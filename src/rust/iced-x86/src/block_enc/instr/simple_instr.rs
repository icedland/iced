// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::block_enc::instr::*;
use crate::block_enc::*;
use crate::iced_error::IcedError;
use core::cell::RefCell;

pub(super) struct SimpleInstr {
	orig_ip: u64,
	ip: u64,
	block: Rc<RefCell<Block>>,
	size: u32,
	instruction: Instruction,
}

impl SimpleInstr {
	pub(super) fn new(block_encoder: &mut BlockEncoder, block: Rc<RefCell<Block>>, instruction: &Instruction) -> Self {
		Self {
			orig_ip: instruction.ip(),
			ip: 0,
			block,
			size: block_encoder.get_instruction_size(instruction, instruction.ip()),
			instruction: *instruction,
		}
	}
}

impl Instr for SimpleInstr {
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

	fn initialize(&mut self, _block_encoder: &BlockEncoder) {}

	fn optimize(&mut self, _gained: u64) -> bool {
		false
	}

	fn encode(&mut self, block: &mut Block) -> Result<(ConstantOffsets, bool), IcedError> {
		block.encoder.encode(&self.instruction, self.ip).map_or_else(
			|err| Err(IcedError::with_string(InstrUtils::create_error_message(&err, &self.instruction))),
			|_| Ok((block.encoder.get_constant_offsets(), true)),
		)
	}
}
