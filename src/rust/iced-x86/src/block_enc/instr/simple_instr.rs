// SPDX-License-Identifier: MIT
// Copyright wtfsck@protonmail.com
// Copyright iced contributors

use super::super::super::iced_error::IcedError;
use super::super::*;
use super::*;
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

	fn initialize(&mut self, _block_encoder: &BlockEncoder) {}

	fn optimize(&mut self) -> bool {
		false
	}

	fn encode(&mut self, block: &mut Block) -> Result<(ConstantOffsets, bool), IcedError> {
		block.encoder.encode(&self.instruction, self.ip).map_or_else(
			|err| Err(IcedError::with_string(InstrUtils::create_error_message(&err, &self.instruction))),
			|_| Ok((block.encoder.get_constant_offsets(), true)),
		)
	}
}
