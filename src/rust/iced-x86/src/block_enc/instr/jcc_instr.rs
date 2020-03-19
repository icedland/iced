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
use core::{cmp, i32, i8, u32};

#[derive(Debug, Copy, Clone, Eq, PartialEq, Ord, PartialOrd)]
enum InstrKind {
	Unchanged,
	Short,
	Near,
	Long,
	Uninitialized,
}

pub(super) struct JccInstr {
	orig_ip: u64,
	ip: u64,
	block: Rc<RefCell<Block>>,
	size: u32,
	bitness: u32,
	instruction: Instruction,
	target_instr: TargetInstr,
	pointer_data: Option<Rc<RefCell<BlockData>>>,
	instr_kind: InstrKind,
	short_instruction_size: u32,
	near_instruction_size: u32,
}

impl JccInstr {
	// Code:
	//		!jcc short skip		; negated jcc opcode
	//		jmp qword ptr [rip+mem]
	//	skip:
	const LONG_INSTRUCTION_SIZE64: u32 = 2 + InstrUtils::CALL_OR_JMP_POINTER_DATA_INSTRUCTION_SIZE64;

	pub(super) fn new(block_encoder: &mut BlockEncoder, block: Rc<RefCell<Block>>, instruction: &Instruction) -> Self {
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
				cmp::max(near_instruction_size, Self::LONG_INSTRUCTION_SIZE64)
			} else {
				near_instruction_size
			};
		}
		Self {
			orig_ip: instruction.ip(),
			ip: 0,
			block,
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

	fn try_optimize(&mut self) -> bool {
		if self.instr_kind == InstrKind::Unchanged || self.instr_kind == InstrKind::Short {
			return false;
		}

		let mut target_address = self.target_instr.address(self);
		let mut next_rip = self.ip.wrapping_add(self.short_instruction_size as u64);
		let mut diff = target_address.wrapping_sub(next_rip) as i64;
		if i8::MIN as i64 <= diff && diff <= i8::MAX as i64 {
			if let Some(ref pointer_data) = self.pointer_data {
				pointer_data.borrow_mut().is_valid = false;
			}
			self.instr_kind = InstrKind::Short;
			self.size = self.short_instruction_size;
			return true;
		}

		// If it's in the same block, we assume the target is at most 2GB away.
		let mut use_near = self.bitness != 64 || self.target_instr.is_in_block(Rc::clone(&self.block));
		if !use_near {
			target_address = self.target_instr.address(self);
			next_rip = self.ip.wrapping_add(self.near_instruction_size as u64);
			diff = target_address.wrapping_sub(next_rip) as i64;
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
			// Temp needed if rustc < 1.36.0 (2015 edition)
			let tmp = Rc::clone(&self.block);
			self.pointer_data = Some(tmp.borrow_mut().alloc_pointer_location());
		}
		self.instr_kind = InstrKind::Long;
		false
	}

	fn short_jcc_to_native_jcc(code: Code, bitness: u32) -> Code {
		let (c16, c32, c64) = match code {
			Code::Jo_rel8_16 | Code::Jo_rel8_32 | Code::Jo_rel8_64 => (Code::Jo_rel8_16, Code::Jo_rel8_32, Code::Jo_rel8_64),
			Code::Jno_rel8_16 | Code::Jno_rel8_32 | Code::Jno_rel8_64 => (Code::Jno_rel8_16, Code::Jno_rel8_32, Code::Jno_rel8_64),
			Code::Jb_rel8_16 | Code::Jb_rel8_32 | Code::Jb_rel8_64 => (Code::Jb_rel8_16, Code::Jb_rel8_32, Code::Jb_rel8_64),
			Code::Jae_rel8_16 | Code::Jae_rel8_32 | Code::Jae_rel8_64 => (Code::Jae_rel8_16, Code::Jae_rel8_32, Code::Jae_rel8_64),
			Code::Je_rel8_16 | Code::Je_rel8_32 | Code::Je_rel8_64 => (Code::Je_rel8_16, Code::Je_rel8_32, Code::Je_rel8_64),
			Code::Jne_rel8_16 | Code::Jne_rel8_32 | Code::Jne_rel8_64 => (Code::Jne_rel8_16, Code::Jne_rel8_32, Code::Jne_rel8_64),
			Code::Jbe_rel8_16 | Code::Jbe_rel8_32 | Code::Jbe_rel8_64 => (Code::Jbe_rel8_16, Code::Jbe_rel8_32, Code::Jbe_rel8_64),
			Code::Ja_rel8_16 | Code::Ja_rel8_32 | Code::Ja_rel8_64 => (Code::Ja_rel8_16, Code::Ja_rel8_32, Code::Ja_rel8_64),
			Code::Js_rel8_16 | Code::Js_rel8_32 | Code::Js_rel8_64 => (Code::Js_rel8_16, Code::Js_rel8_32, Code::Js_rel8_64),
			Code::Jns_rel8_16 | Code::Jns_rel8_32 | Code::Jns_rel8_64 => (Code::Jns_rel8_16, Code::Jns_rel8_32, Code::Jns_rel8_64),
			Code::Jp_rel8_16 | Code::Jp_rel8_32 | Code::Jp_rel8_64 => (Code::Jp_rel8_16, Code::Jp_rel8_32, Code::Jp_rel8_64),
			Code::Jnp_rel8_16 | Code::Jnp_rel8_32 | Code::Jnp_rel8_64 => (Code::Jnp_rel8_16, Code::Jnp_rel8_32, Code::Jnp_rel8_64),
			Code::Jl_rel8_16 | Code::Jl_rel8_32 | Code::Jl_rel8_64 => (Code::Jl_rel8_16, Code::Jl_rel8_32, Code::Jl_rel8_64),
			Code::Jge_rel8_16 | Code::Jge_rel8_32 | Code::Jge_rel8_64 => (Code::Jge_rel8_16, Code::Jge_rel8_32, Code::Jge_rel8_64),
			Code::Jle_rel8_16 | Code::Jle_rel8_32 | Code::Jle_rel8_64 => (Code::Jle_rel8_16, Code::Jle_rel8_32, Code::Jle_rel8_64),
			Code::Jg_rel8_16 | Code::Jg_rel8_32 | Code::Jg_rel8_64 => (Code::Jg_rel8_16, Code::Jg_rel8_32, Code::Jg_rel8_64),
			_ => unreachable!(),
		};

		match bitness {
			16 => c16,
			32 => c32,
			64 => c64,
			_ => unreachable!(),
		}
	}
}

impl Instr for JccInstr {
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

	fn encode(&mut self, block: &mut Block) -> Result<(ConstantOffsets, bool), String> {
		match self.instr_kind {
			InstrKind::Unchanged | InstrKind::Short | InstrKind::Near => {
				// Temp needed if rustc < 1.36.0 (2015 edition)
				let tmp;
				if self.instr_kind == InstrKind::Unchanged {
					// nothing
				} else if self.instr_kind == InstrKind::Short {
					tmp = self.instruction.code().as_short_branch();
					self.instruction.set_code(tmp);
				} else {
					debug_assert!(self.instr_kind == InstrKind::Near);
					tmp = self.instruction.code().as_near_branch();
					self.instruction.set_code(tmp);
				}
				// Temp needed if rustc < 1.36.0 (2015 edition)
				let tmp = self.target_instr.address(self);
				self.instruction.set_near_branch64(tmp);
				match block.encoder.encode(&self.instruction, self.ip) {
					Err(err) => Err(InstrUtils::create_error_message(&err, &self.instruction)),
					Ok(_) => Ok((block.encoder.get_constant_offsets(), true)),
				}
			}

			InstrKind::Long => {
				debug_assert!(self.pointer_data.is_some());
				let pointer_data = self.pointer_data.clone().unwrap();
				pointer_data.borrow_mut().data = self.target_instr.address(self);
				let mut instr = Instruction::default();
				instr.set_code(Self::short_jcc_to_native_jcc(
					self.instruction.code().negate_condition_code().as_short_branch(),
					block.encoder.bitness(),
				));
				instr.set_op0_kind(OpKind::NearBranch64);
				debug_assert!(block.encoder.bitness() == 64);
				debug_assert!(Self::LONG_INSTRUCTION_SIZE64 <= i8::MAX as u32);
				instr.set_near_branch64(self.ip.wrapping_add(Self::LONG_INSTRUCTION_SIZE64 as u64));
				let instr_len = match block.encoder.encode(&instr, self.ip) {
					Err(err) => return Err(InstrUtils::create_error_message(&err, &self.instruction)),
					Ok(len) => len,
				} as u32;
				match InstrUtils::encode_branch_to_pointer_data(
					block,
					false,
					self.ip.wrapping_add(instr_len as u64),
					pointer_data,
					self.size - instr_len,
				) {
					Ok(_) => Ok((ConstantOffsets::default(), false)),
					Err(err) => Err(InstrUtils::create_error_message(&err, &self.instruction)),
				}
			}

			InstrKind::Uninitialized => unreachable!(),
		}
	}
}
