// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::block_enc::instr::*;
use crate::block_enc::*;
use crate::iced_error::IcedError;

#[derive(Debug, Copy, Clone, Eq, PartialEq, Ord, PartialOrd)]
enum InstrKind {
	Unchanged,
	Rip,
	Eip,
	Long,
	Uninitialized,
}

pub(super) struct IpRelMemOpInstr {
	instruction: Instruction,
	instr_kind: InstrKind,
	eip_instruction_size: u8,
	rip_instruction_size: u8,
	target_instr: TargetInstr,
}

impl IpRelMemOpInstr {
	pub(super) fn new(block_encoder: &mut BlockEncInt, base: &mut InstrBase, instruction: &Instruction) -> Self {
		debug_assert!(instruction.is_ip_rel_memory_operand());

		let mut instr_copy = *instruction;
		instr_copy.set_memory_base(Register::RIP);
		instr_copy.set_memory_displacement64(0);
		let rip_instruction_size = block_encoder.get_instruction_size(&instr_copy, instr_copy.ip_rel_memory_address()) as u8;

		instr_copy.set_memory_base(Register::EIP);
		let eip_instruction_size = block_encoder.get_instruction_size(&instr_copy, instr_copy.ip_rel_memory_address()) as u8;

		base.size = eip_instruction_size as u32;
		debug_assert!(eip_instruction_size >= rip_instruction_size);
		Self {
			instruction: *instruction,
			instr_kind: InstrKind::Uninitialized,
			eip_instruction_size,
			rip_instruction_size,
			target_instr: TargetInstr::default(),
		}
	}

	fn try_optimize(&mut self, base: &mut InstrBase, ctx: &mut InstrContext<'_>, gained: u64) -> bool {
		if self.instr_kind == InstrKind::Unchanged || self.instr_kind == InstrKind::Rip || self.instr_kind == InstrKind::Eip {
			base.done = true;
			return false;
		}

		// If it's in the same block, we assume the target is at most 2GB away.
		let mut use_rip = self.target_instr.is_in_block(ctx.block);
		let target_address = self.target_instr.address(ctx);
		if !use_rip {
			let next_rip = ctx.ip.wrapping_add(self.rip_instruction_size as u64);
			let diff = target_address.wrapping_sub(next_rip) as i64;
			let diff = correct_diff(self.target_instr.is_in_block(ctx.block), diff, gained);
			use_rip = i32::MIN as i64 <= diff && diff <= i32::MAX as i64;
		}

		if use_rip {
			base.size = self.rip_instruction_size as u32;
			self.instr_kind = InstrKind::Rip;
			base.done = true;
			return true;
		}

		// If it's in the low 4GB we can use EIP relative addressing
		if target_address <= u32::MAX as u64 {
			base.size = self.eip_instruction_size as u32;
			self.instr_kind = InstrKind::Eip;
			base.done = true;
			return true;
		}

		self.instr_kind = InstrKind::Long;
		false
	}
}

impl Instr for IpRelMemOpInstr {
	fn get_target_instr(&mut self) -> (&mut TargetInstr, u64) {
		(&mut self.target_instr, self.instruction.ip_rel_memory_address())
	}

	fn optimize(&mut self, base: &mut InstrBase, ctx: &mut InstrContext<'_>, gained: u64) -> bool {
		self.try_optimize(base, ctx, gained)
	}

	fn encode(&mut self, _base: &mut InstrBase, ctx: &mut InstrContext<'_>) -> Result<(ConstantOffsets, bool), IcedError> {
		match self.instr_kind {
			InstrKind::Unchanged | InstrKind::Rip | InstrKind::Eip => {
				if self.instr_kind == InstrKind::Rip {
					self.instruction.set_memory_base(Register::RIP);
				} else if self.instr_kind == InstrKind::Eip {
					self.instruction.set_memory_base(Register::EIP);
				} else {
					debug_assert!(self.instr_kind == InstrKind::Unchanged);
				};

				let target_address = self.target_instr.address(ctx);
				self.instruction.set_memory_displacement64(target_address);
				match ctx.block.encoder.encode(&self.instruction, ctx.ip) {
					Ok(_) => {
						let expected_rip =
							if self.instruction.memory_base() == Register::EIP { target_address as u32 as u64 } else { target_address };
						if self.instruction.ip_rel_memory_address() != expected_rip {
							Err(IcedError::with_string(InstrUtils::create_error_message("Invalid IP relative address", &self.instruction)))
						} else {
							Ok((ctx.block.encoder.get_constant_offsets(), true))
						}
					}
					Err(err) => Err(IcedError::with_string(InstrUtils::create_error_message(err, &self.instruction))),
				}
			}

			InstrKind::Long => Err(IcedError::new(
				"IP relative memory operand is too far away and isn't currently supported. \
				 Try to allocate memory close to the original instruction (+/-2GB).",
			)),

			InstrKind::Uninitialized => unreachable!(),
		}
	}
}
