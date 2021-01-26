// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

use super::super::super::iced_error::IcedError;
use super::super::*;
use super::*;
use core::cell::RefCell;
use core::{i32, u32};

#[derive(Debug, Copy, Clone, Eq, PartialEq, Ord, PartialOrd)]
enum InstrKind {
	Unchanged,
	Rip,
	Eip,
	Long,
	Uninitialized,
}

pub(super) struct IpRelMemOpInstr {
	orig_ip: u64,
	ip: u64,
	block: Rc<RefCell<Block>>,
	size: u32,
	instruction: Instruction,
	instr_kind: InstrKind,
	eip_instruction_size: u32,
	rip_instruction_size: u32,
	target_instr: TargetInstr,
}

impl IpRelMemOpInstr {
	pub(super) fn new(block_encoder: &mut BlockEncoder, block: Rc<RefCell<Block>>, instruction: &Instruction) -> Self {
		debug_assert!(instruction.is_ip_rel_memory_operand());

		let mut instr_copy = *instruction;
		instr_copy.set_memory_base(Register::RIP);
		instr_copy.set_memory_displacement64(0);
		let rip_instruction_size = block_encoder.get_instruction_size(&instr_copy, instr_copy.ip_rel_memory_address());

		instr_copy.set_memory_base(Register::EIP);
		let eip_instruction_size = block_encoder.get_instruction_size(&instr_copy, instr_copy.ip_rel_memory_address());

		debug_assert!(eip_instruction_size >= rip_instruction_size);
		Self {
			orig_ip: instruction.ip(),
			ip: 0,
			block,
			size: eip_instruction_size,
			instruction: *instruction,
			instr_kind: InstrKind::Uninitialized,
			eip_instruction_size,
			rip_instruction_size,
			target_instr: TargetInstr::default(),
		}
	}

	fn try_optimize(&mut self) -> bool {
		if self.instr_kind == InstrKind::Unchanged || self.instr_kind == InstrKind::Rip || self.instr_kind == InstrKind::Eip {
			return false;
		}

		// If it's in the same block, we assume the target is at most 2GB away.
		let mut use_rip = self.target_instr.is_in_block(Rc::clone(&self.block));
		let target_address = self.target_instr.address(self);
		if !use_rip {
			let next_rip = self.ip.wrapping_add(self.rip_instruction_size as u64);
			let diff = target_address.wrapping_sub(next_rip) as i64;
			use_rip = i32::MIN as i64 <= diff && diff <= i32::MAX as i64;
		}

		if use_rip {
			self.size = self.rip_instruction_size;
			self.instr_kind = InstrKind::Rip;
			return true;
		}

		// If it's in the lower 4GB we can use EIP relative addressing
		if target_address <= u32::MAX as u64 {
			self.size = self.eip_instruction_size;
			self.instr_kind = InstrKind::Eip;
			return true;
		}

		self.instr_kind = InstrKind::Long;
		false
	}
}

impl Instr for IpRelMemOpInstr {
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
		self.target_instr = block_encoder.get_target(self, self.instruction.ip_rel_memory_address());
		let _ = self.try_optimize();
	}

	fn optimize(&mut self) -> bool {
		self.try_optimize()
	}

	fn encode(&mut self, block: &mut Block) -> Result<(ConstantOffsets, bool), IcedError> {
		match self.instr_kind {
			InstrKind::Unchanged | InstrKind::Rip | InstrKind::Eip => {
				if self.instr_kind == InstrKind::Rip {
					self.instruction.set_memory_base(Register::RIP);
				} else if self.instr_kind == InstrKind::Eip {
					self.instruction.set_memory_base(Register::EIP);
				} else {
					debug_assert!(self.instr_kind == InstrKind::Unchanged);
				};

				let target_address = self.target_instr.address(self);
				self.instruction.set_memory_displacement64(target_address);
				match block.encoder.encode(&self.instruction, self.ip) {
					Ok(_) => {
						let expected_rip =
							if self.instruction.memory_base() == Register::EIP { target_address as u32 as u64 } else { target_address };
						if self.instruction.ip_rel_memory_address() != expected_rip {
							Err(IcedError::with_string(InstrUtils::create_error_message("Invalid IP relative address", &self.instruction)))
						} else {
							Ok((block.encoder.get_constant_offsets(), true))
						}
					}
					Err(err) => Err(IcedError::with_string(InstrUtils::create_error_message(&err, &self.instruction))),
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
