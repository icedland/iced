// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::block_enc::instr::*;
use crate::block_enc::*;
use crate::iced_error::IcedError;

pub(super) struct SimpleInstr {
	orig_ip: u64,
	size: u32,
	instruction: Instruction,
}

impl SimpleInstr {
	pub(super) fn new(block_encoder: &mut BlockEncInt, instruction: &Instruction) -> Self {
		Self { orig_ip: instruction.ip(), size: block_encoder.get_instruction_size(instruction, instruction.ip()), instruction: *instruction }
	}
}

impl Instr for SimpleInstr {
	fn size(&self) -> u32 {
		self.size
	}

	fn orig_ip(&self) -> u64 {
		self.orig_ip
	}

	fn initialize<'a>(&mut self, _block_encoder: &BlockEncInt, _ctx: &mut InstrContext<'a>) {}

	fn optimize<'a>(&mut self, _ctx: &mut InstrContext<'a>, _gained: u64) -> bool {
		false
	}

	fn encode<'a>(&mut self, ctx: &mut InstrContext<'a>) -> Result<(ConstantOffsets, bool), IcedError> {
		ctx.block.encoder.encode(&self.instruction, ctx.ip).map_or_else(
			|err| Err(IcedError::with_string(InstrUtils::create_error_message(&err, &self.instruction))),
			|_| Ok((ctx.block.encoder.get_constant_offsets(), true)),
		)
	}
}
