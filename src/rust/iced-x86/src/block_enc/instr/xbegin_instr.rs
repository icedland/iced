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
#[cfg(not(feature = "std"))]
use alloc::string::String;
use core::cell::RefCell;

pub(super) struct XbeginInstr {
	orig_ip: u64,
	ip: u64,
	block: Rc<RefCell<Block>>,
	size: u32,
	target_addr: u64,
	instruction: Instruction,
	target_instr: TargetInstr,
}

impl XbeginInstr {
	pub(super) fn new(block_encoder: &mut BlockEncoder, block: Rc<RefCell<Block>>, instruction: &Instruction) -> Self {
		let mut instruction = *instruction;
		if block_encoder.fix_branches() {
			if block_encoder.bitness() == 16 {
				instruction.set_code(Code::Xbegin_rel16);
			} else {
				debug_assert!(block_encoder.bitness() == 32 || block_encoder.bitness() == 64);
				instruction.set_code(Code::Xbegin_rel32);
			}
		}
		let instruction = instruction;
		let mut instr_copy = instruction;
		instr_copy.set_near_branch64(0);
		Self {
			orig_ip: instruction.ip(),
			ip: 0,
			block,
			size: block_encoder.get_instruction_size(&instr_copy, 0),
			target_addr: instruction.near_branch_target(),
			instruction,
			target_instr: TargetInstr::default(),
		}
	}
}

impl Instr for XbeginInstr {
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
		self.target_instr = block_encoder.get_target(self, self.target_addr);
	}

	fn optimize(&mut self) -> bool {
		false
	}

	fn encode(&mut self, block: &mut Block) -> Result<(ConstantOffsets, bool), String> {
		// Temp needed if rustc < 1.36.0 (2015 edition)
		let tmp = self.target_instr.address(self);
		self.instruction.set_near_branch64(tmp);
		match block.encoder.encode(&self.instruction, self.ip) {
			Err(err) => Err(InstrUtils::create_error_message(&err, &self.instruction)),
			Ok(_) => Ok((block.encoder.get_constant_offsets(), true)),
		}
	}
}
