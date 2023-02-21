// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::block_enc::instr::*;
use crate::block_enc::*;
use crate::iced_error::IcedError;

pub(super) struct SimpleInstr {
	instruction: Instruction,
}

impl SimpleInstr {
	pub(super) fn new(block_encoder: &mut BlockEncInt, base: &mut InstrBase, instruction: &Instruction) -> Self {
		base.done = true;
		base.size = block_encoder.get_instruction_size(instruction, instruction.ip());
		Self { instruction: *instruction }
	}
}

impl Instr for SimpleInstr {
	fn get_target_instr(&mut self) -> (&mut TargetInstr, u64) {
		// Never called since base.done == true
		unreachable!()
	}

	fn optimize(&mut self, _base: &mut InstrBase, _ctx: &mut InstrContext<'_>, _gained: u64) -> bool {
		false
	}

	fn encode(&mut self, _base: &mut InstrBase, ctx: &mut InstrContext<'_>) -> Result<(ConstantOffsets, bool), IcedError> {
		ctx.block.encoder.encode(&self.instruction, ctx.ip).map_or_else(
			|err| Err(IcedError::with_string(InstrUtils::create_error_message(err, &self.instruction))),
			|_| Ok((ctx.block.encoder.get_constant_offsets(), true)),
		)
	}
}
