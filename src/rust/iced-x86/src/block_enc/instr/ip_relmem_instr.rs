/*
Copyright (C) 2018-2019 de4dot@gmail.com

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

use super::super::*;
use super::*;
use std::cell::RefCell;
use std::{i32, u32};

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
	pub fn new(block_encoder: &mut BlockEncoder, block: Rc<RefCell<Block>>, instruction: &Instruction) -> Self {
		debug_assert!(instruction.is_ip_relative_memory_operand());

		let mut instr_copy = *instruction;
		instr_copy.set_memory_base(Register::RIP);
		let rip_instruction_size = block_encoder.get_instruction_size(&instr_copy, instr_copy.ip());

		instr_copy.set_memory_base(Register::EIP);
		let eip_instruction_size = block_encoder.get_instruction_size(&instr_copy, instr_copy.ip());

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
		let mut use_rip = self.target_instr.is_in_block(self.block.clone());
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

	fn initialize(&mut self, block_encoder: &BlockEncoder) {
		self.target_instr = block_encoder.get_target(self, self.instruction.ip_relative_memory_address());
		let _ = self.try_optimize();
	}

	fn optimize(&mut self) -> bool {
		self.try_optimize()
	}

	fn encode(&mut self, block: &mut Block) -> Result<(ConstantOffsets, bool), String> {
		match self.instr_kind {
			InstrKind::Unchanged | InstrKind::Rip | InstrKind::Eip => {
				let instr_size = if self.instr_kind == InstrKind::Rip {
					self.instruction.set_memory_base(Register::RIP);
					self.rip_instruction_size
				} else if self.instr_kind == InstrKind::Eip {
					self.instruction.set_memory_base(Register::EIP);
					self.eip_instruction_size
				} else {
					debug_assert!(self.instr_kind == InstrKind::Unchanged);
					if self.instruction.memory_base() == Register::EIP {
						self.eip_instruction_size
					} else {
						self.rip_instruction_size
					}
				};

				let target_address = self.target_instr.address(self);
				let next_rip = self.ip.wrapping_add(instr_size as u64);
				self.instruction.set_next_ip(next_rip);
				self.instruction.set_memory_displacement((target_address as u32).wrapping_sub(next_rip as u32));
				match block.encoder.encode(&self.instruction, self.ip) {
					Ok(_) => {
						let expected_rip =
							if self.instruction.memory_base() == Register::EIP { target_address as u32 as u64 } else { target_address };
						if self.instruction.ip_relative_memory_address() != expected_rip {
							Err(InstrUtils::create_error_message("Invalid IP relative address", &self.instruction))
						} else {
							Ok((block.encoder.get_constant_offsets(), true))
						}
					}
					Err(err) => Err(InstrUtils::create_error_message(&err, &self.instruction)),
				}
			}

			InstrKind::Long => Err("IP relative memory operand is too far away and isn't currently supported".to_owned()),
			InstrKind::Uninitialized => unreachable!(),
		}
	}
}
