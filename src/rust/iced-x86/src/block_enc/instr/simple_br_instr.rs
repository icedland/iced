// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::block_enc::instr::*;
use crate::block_enc::*;
use crate::iced_error::IcedError;
use core::cell::RefCell;
use core::cmp;

#[derive(Debug, Copy, Clone, Eq, PartialEq, Ord, PartialOrd)]
enum InstrKind {
	Unchanged,
	Short,
	Near,
	Long,
	Uninitialized,
}

pub(super) struct SimpleBranchInstr {
	bitness: u8,
	instruction: Instruction,
	target_instr: TargetInstr,
	pointer_data: Option<Rc<RefCell<BlockData>>>,
	instr_kind: InstrKind,
	short_instruction_size: u8,
	near_instruction_size: u8,
	long_instruction_size: u8,
	native_instruction_size: u8,
	native_code: Code,
}

impl SimpleBranchInstr {
	pub(super) fn new(block_encoder: &mut BlockEncInt, base: &mut InstrBase, instruction: &Instruction) -> Self {
		let mut instr_kind = InstrKind::Uninitialized;
		let mut instr_copy;
		let native_code;
		let short_instruction_size;
		let near_instruction_size;
		let long_instruction_size;
		let native_instruction_size;
		if !block_encoder.fix_branches() {
			instr_kind = InstrKind::Unchanged;
			instr_copy = *instruction;
			instr_copy.set_near_branch64(0);
			base.size = block_encoder.get_instruction_size(&instr_copy, 0);
			native_code = Code::INVALID;
			short_instruction_size = 0;
			near_instruction_size = 0;
			long_instruction_size = 0;
			native_instruction_size = 0;
		} else {
			instr_copy = *instruction;
			instr_copy.set_near_branch64(0);
			short_instruction_size = block_encoder.get_instruction_size(&instr_copy, 0) as u8;

			native_code = Self::as_native_branch_code(instruction.code(), block_encoder.bitness());
			native_instruction_size = if native_code == instruction.code() {
				short_instruction_size
			} else {
				instr_copy = *instruction;
				instr_copy.set_code(native_code);
				instr_copy.set_near_branch64(0);
				block_encoder.get_instruction_size(&instr_copy, 0) as u8
			};

			near_instruction_size = match block_encoder.bitness() {
				16 => native_instruction_size + 2 + 3,
				32 | 64 => native_instruction_size + 2 + 5,
				_ => unreachable!(),
			};

			base.size = if block_encoder.bitness() == 64 {
				long_instruction_size = native_instruction_size + 2 + InstrUtils::CALL_OR_JMP_POINTER_DATA_INSTRUCTION_SIZE64 as u8;
				cmp::max(cmp::max(short_instruction_size, near_instruction_size), long_instruction_size)
			} else {
				long_instruction_size = 0;
				cmp::max(short_instruction_size, near_instruction_size)
			} as u32;
		}
		Self {
			bitness: block_encoder.bitness() as u8,
			instruction: *instruction,
			target_instr: TargetInstr::default(),
			pointer_data: None,
			instr_kind,
			short_instruction_size,
			near_instruction_size,
			long_instruction_size,
			native_instruction_size,
			native_code,
		}
	}

	fn try_optimize(&mut self, base: &mut InstrBase, ctx: &mut InstrContext<'_>, gained: u64) -> bool {
		if self.instr_kind == InstrKind::Unchanged || self.instr_kind == InstrKind::Short {
			base.done = true;
			return false;
		}

		let mut target_address = self.target_instr.address(ctx);
		let mut next_rip = ctx.ip.wrapping_add(self.short_instruction_size as u64);
		let mut diff = target_address.wrapping_sub(next_rip) as i64;
		diff = InstrUtils::convert_diff_to_bitness_diff(self.bitness, correct_diff(self.target_instr.is_in_block(ctx.block), diff, gained));
		if i8::MIN as i64 <= diff && diff <= i8::MAX as i64 {
			if let Some(ref pointer_data) = self.pointer_data {
				pointer_data.borrow_mut().is_valid = false;
			}
			self.instr_kind = InstrKind::Short;
			base.size = self.short_instruction_size as u32;
			base.done = true;
			return true;
		}

		// If it's in the same block, we assume the target is at most 2GB away.
		let mut use_near = self.bitness != 64 || self.target_instr.is_in_block(ctx.block);
		if !use_near {
			target_address = self.target_instr.address(ctx);
			next_rip = ctx.ip.wrapping_add(self.near_instruction_size as u64);
			diff = target_address.wrapping_sub(next_rip) as i64;
			diff = InstrUtils::convert_diff_to_bitness_diff(self.bitness, correct_diff(self.target_instr.is_in_block(ctx.block), diff, gained));
			use_near = i32::MIN as i64 <= diff && diff <= i32::MAX as i64;
		}
		if use_near {
			if let Some(ref pointer_data) = self.pointer_data {
				pointer_data.borrow_mut().is_valid = false;
			}
			if diff < (IcedConstants::MAX_INSTRUCTION_LENGTH as i64) * (i8::MIN as i64)
				|| diff > (IcedConstants::MAX_INSTRUCTION_LENGTH as i64) * (i8::MAX as i64)
			{
				base.done = true;
			}
			self.instr_kind = InstrKind::Near;
			base.size = self.near_instruction_size as u32;
			return true;
		}

		if self.pointer_data.is_none() {
			self.pointer_data = Some(ctx.block.alloc_pointer_location());
		}
		self.instr_kind = InstrKind::Long;
		false
	}

	fn as_native_branch_code(code: Code, bitness: u32) -> Code {
		let (c16, c32, c64) = match code {
			Code::Loopne_rel8_16_CX | Code::Loopne_rel8_32_CX => (Code::Loopne_rel8_16_CX, Code::Loopne_rel8_32_CX, Code::INVALID),
			Code::Loopne_rel8_16_ECX | Code::Loopne_rel8_32_ECX | Code::Loopne_rel8_64_ECX => {
				(Code::Loopne_rel8_16_ECX, Code::Loopne_rel8_32_ECX, Code::Loopne_rel8_64_ECX)
			}
			Code::Loopne_rel8_16_RCX | Code::Loopne_rel8_64_RCX => (Code::Loopne_rel8_16_RCX, Code::INVALID, Code::Loopne_rel8_64_RCX),
			Code::Loope_rel8_16_CX | Code::Loope_rel8_32_CX => (Code::Loope_rel8_16_CX, Code::Loope_rel8_32_CX, Code::INVALID),
			Code::Loope_rel8_16_ECX | Code::Loope_rel8_32_ECX | Code::Loope_rel8_64_ECX => {
				(Code::Loope_rel8_16_ECX, Code::Loope_rel8_32_ECX, Code::Loope_rel8_64_ECX)
			}
			Code::Loope_rel8_16_RCX | Code::Loope_rel8_64_RCX => (Code::Loope_rel8_16_RCX, Code::INVALID, Code::Loope_rel8_64_RCX),
			Code::Loop_rel8_16_CX | Code::Loop_rel8_32_CX => (Code::Loop_rel8_16_CX, Code::Loop_rel8_32_CX, Code::INVALID),
			Code::Loop_rel8_16_ECX | Code::Loop_rel8_32_ECX | Code::Loop_rel8_64_ECX => {
				(Code::Loop_rel8_16_ECX, Code::Loop_rel8_32_ECX, Code::Loop_rel8_64_ECX)
			}
			Code::Loop_rel8_16_RCX | Code::Loop_rel8_64_RCX => (Code::Loop_rel8_16_RCX, Code::INVALID, Code::Loop_rel8_64_RCX),
			Code::Jcxz_rel8_16 | Code::Jcxz_rel8_32 => (Code::Jcxz_rel8_16, Code::Jcxz_rel8_32, Code::INVALID),
			Code::Jecxz_rel8_16 | Code::Jecxz_rel8_32 | Code::Jecxz_rel8_64 => (Code::Jecxz_rel8_16, Code::Jecxz_rel8_32, Code::Jecxz_rel8_64),
			Code::Jrcxz_rel8_16 | Code::Jrcxz_rel8_64 => (Code::Jrcxz_rel8_16, Code::INVALID, Code::Jrcxz_rel8_64),
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

impl Instr for SimpleBranchInstr {
	fn get_target_instr(&mut self) -> (&mut TargetInstr, u64) {
		(&mut self.target_instr, self.instruction.near_branch_target())
	}

	fn optimize(&mut self, base: &mut InstrBase, ctx: &mut InstrContext<'_>, gained: u64) -> bool {
		self.try_optimize(base, ctx, gained)
	}

	fn encode(&mut self, base: &mut InstrBase, ctx: &mut InstrContext<'_>) -> Result<(ConstantOffsets, bool), IcedError> {
		let mut instr;
		let mut size;
		let instr_len;
		match self.instr_kind {
			InstrKind::Unchanged | InstrKind::Short => {
				self.instruction.set_near_branch64(self.target_instr.address(ctx));
				ctx.block.encoder.encode(&self.instruction, ctx.ip).map_or_else(
					|err| Err(IcedError::with_string(InstrUtils::create_error_message(err, &self.instruction))),
					|_| Ok((ctx.block.encoder.get_constant_offsets(), true)),
				)
			}

			InstrKind::Near => {
				// Code:
				//		brins tmp		; native_instruction_size
				//		jmp short skip	; 2
				//	tmp:
				//		jmp near target	; 3/5/5
				//	skip:

				instr = self.instruction;
				instr.set_code(self.native_code);
				instr.set_near_branch64(ctx.ip.wrapping_add(self.native_instruction_size as u64).wrapping_add(2));
				size = ctx
					.block
					.encoder
					.encode(&instr, ctx.ip)
					.map_err(|err| IcedError::with_string(InstrUtils::create_error_message(err, &self.instruction)))? as u32;

				instr = Instruction::default();
				instr.set_near_branch64(ctx.ip.wrapping_add(self.near_instruction_size as u64));
				let code_near = match ctx.block.encoder.bitness() {
					16 => {
						instr.set_code(Code::Jmp_rel8_16);
						instr.set_op0_kind(OpKind::NearBranch16);
						Code::Jmp_rel16
					}

					32 => {
						instr.set_code(Code::Jmp_rel8_32);
						instr.set_op0_kind(OpKind::NearBranch32);
						Code::Jmp_rel32_32
					}

					64 => {
						instr.set_code(Code::Jmp_rel8_64);
						instr.set_op0_kind(OpKind::NearBranch64);
						Code::Jmp_rel32_64
					}

					_ => unreachable!(),
				};
				instr_len = ctx
					.block
					.encoder
					.encode(&instr, ctx.ip.wrapping_add(size as u64))
					.map_err(|err| IcedError::with_string(InstrUtils::create_error_message(err, &self.instruction)))? as u32;
				size += instr_len;

				instr.set_code(code_near);
				instr.set_near_branch64(self.target_instr.address(ctx));
				ctx.block.encoder.encode(&instr, ctx.ip.wrapping_add(size as u64)).map_or_else(
					|err| Err(IcedError::with_string(InstrUtils::create_error_message(err, &self.instruction))),
					|_| Ok((ConstantOffsets::default(), false)),
				)
			}

			InstrKind::Long => {
				debug_assert_eq!(ctx.block.encoder.bitness(), 64);
				debug_assert!(self.pointer_data.is_some());
				let pointer_data = self.pointer_data.clone().ok_or_else(|| IcedError::new("Internal error"))?;
				pointer_data.borrow_mut().data = self.target_instr.address(ctx);

				// Code:
				//		brins tmp		; native_instruction_size
				//		jmp short skip	; 2
				//	tmp:
				//		jmp [mem_loc]	; 6
				//	skip:

				instr = self.instruction;
				instr.set_code(self.native_code);
				instr.set_near_branch64(ctx.ip.wrapping_add(self.native_instruction_size as u64).wrapping_add(2));
				size = ctx
					.block
					.encoder
					.encode(&instr, ctx.ip)
					.map_err(|err| IcedError::with_string(InstrUtils::create_error_message(err, &self.instruction)))? as u32;

				instr = Instruction::default();
				instr.set_near_branch64(ctx.ip.wrapping_add(self.long_instruction_size as u64));
				match ctx.block.encoder.bitness() {
					16 => {
						instr.set_code(Code::Jmp_rel8_16);
						instr.set_op0_kind(OpKind::NearBranch16);
					}

					32 => {
						instr.set_code(Code::Jmp_rel8_32);
						instr.set_op0_kind(OpKind::NearBranch32);
					}

					64 => {
						instr.set_code(Code::Jmp_rel8_64);
						instr.set_op0_kind(OpKind::NearBranch64);
					}

					_ => unreachable!(),
				}
				instr_len = ctx
					.block
					.encoder
					.encode(&instr, ctx.ip.wrapping_add(size as u64))
					.map_err(|err| IcedError::with_string(InstrUtils::create_error_message(err, &self.instruction)))? as u32;
				size += instr_len;

				InstrUtils::encode_branch_to_pointer_data(
					ctx.block,
					false,
					ctx.ip.wrapping_add(size as u64),
					pointer_data,
					base.size.wrapping_sub(size),
				)
				.map_or_else(
					|err| Err(IcedError::with_string(InstrUtils::create_error_message(err, &self.instruction))),
					|_| Ok((ConstantOffsets::default(), false)),
				)
			}

			InstrKind::Uninitialized => unreachable!(),
		}
	}
}
