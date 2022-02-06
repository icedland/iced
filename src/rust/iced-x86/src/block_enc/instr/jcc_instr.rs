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

pub(super) struct JccInstr {
	bitness: u32,
	instruction: Instruction,
	target_instr: TargetInstr,
	pointer_data: Option<Rc<RefCell<BlockData>>>,
	instr_kind: InstrKind,
	short_instruction_size: u32,
	near_instruction_size: u32,
	long_instruction_size64: u32,
}

impl JccInstr {
	#[inline]
	#[allow(unused_variables)]
	fn long_instruction_size64(instruction: &Instruction) -> u32 {
		#[cfg(feature = "mvex")]
		{
			// Check if JKZD/JKNZD
			if instruction.op_count() == 2 {
				return 5 + InstrUtils::CALL_OR_JMP_POINTER_DATA_INSTRUCTION_SIZE64;
			}
		}
		// Code:
		//		!jcc short skip		; negated jcc opcode
		//		jmp qword ptr [rip+mem]
		//	skip:
		2 + InstrUtils::CALL_OR_JMP_POINTER_DATA_INSTRUCTION_SIZE64
	}

	pub(super) fn new(block_encoder: &mut BlockEncInt, base: &mut InstrBase, instruction: &Instruction) -> Self {
		let mut instr_kind = InstrKind::Uninitialized;
		let mut instr_copy: Instruction;
		let short_instruction_size;
		let near_instruction_size;
		let long_instruction_size64 = Self::long_instruction_size64(instruction);
		if !block_encoder.fix_branches() {
			instr_kind = InstrKind::Unchanged;
			instr_copy = *instruction;
			instr_copy.set_near_branch64(0);
			base.size = block_encoder.get_instruction_size(&instr_copy, 0);
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

			base.size = if block_encoder.bitness() == 64 {
				// Make sure it's not shorter than the real instruction. It can happen if there are extra prefixes.
				cmp::max(near_instruction_size, long_instruction_size64)
			} else {
				near_instruction_size
			};
		}
		Self {
			bitness: block_encoder.bitness(),
			instruction: *instruction,
			target_instr: TargetInstr::default(),
			pointer_data: None,
			instr_kind,
			short_instruction_size,
			near_instruction_size,
			long_instruction_size64,
		}
	}

	fn try_optimize<'a>(&mut self, base: &mut InstrBase, ctx: &mut InstrContext<'a>, gained: u64) -> bool {
		if self.instr_kind == InstrKind::Unchanged || self.instr_kind == InstrKind::Short {
			return false;
		}

		let mut target_address = self.target_instr.address(ctx);
		let mut next_rip = ctx.ip.wrapping_add(self.short_instruction_size as u64);
		let mut diff = target_address.wrapping_sub(next_rip) as i64;
		diff = correct_diff(self.target_instr.is_in_block(ctx.block), diff, gained);
		if i8::MIN as i64 <= diff && diff <= i8::MAX as i64 {
			if let Some(ref pointer_data) = self.pointer_data {
				pointer_data.borrow_mut().is_valid = false;
			}
			self.instr_kind = InstrKind::Short;
			base.size = self.short_instruction_size;
			return true;
		}

		// If it's in the same block, we assume the target is at most 2GB away.
		let mut use_near = self.bitness != 64 || self.target_instr.is_in_block(ctx.block);
		if !use_near {
			target_address = self.target_instr.address(ctx);
			next_rip = ctx.ip.wrapping_add(self.near_instruction_size as u64);
			diff = target_address.wrapping_sub(next_rip) as i64;
			diff = correct_diff(self.target_instr.is_in_block(ctx.block), diff, gained);
			use_near = i32::MIN as i64 <= diff && diff <= i32::MAX as i64;
		}
		if use_near {
			if let Some(ref pointer_data) = self.pointer_data {
				pointer_data.borrow_mut().is_valid = false;
			}
			self.instr_kind = InstrKind::Near;
			base.size = self.near_instruction_size;
			return true;
		}

		if self.pointer_data.is_none() {
			self.pointer_data = Some(ctx.block.alloc_pointer_location());
		}
		self.instr_kind = InstrKind::Long;
		false
	}

	fn short_br_to_native_br(code: Code, bitness: u32) -> Code {
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
			_ => {
				#[cfg(feature = "mvex")]
				{
					if bitness == 64 {
						match code {
							Code::VEX_KNC_Jkzd_kr_rel8_64 | Code::VEX_KNC_Jknzd_kr_rel8_64 => return code,
							_ => {}
						}
					}
				}
				unreachable!()
			}
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
	fn initialize<'a>(&mut self, base: &mut InstrBase, block_encoder: &BlockEncInt, ctx: &mut InstrContext<'a>) {
		self.target_instr = block_encoder.get_target(base, self.instruction.near_branch_target());
		let _ = self.try_optimize(base, ctx, 0);
	}

	fn optimize<'a>(&mut self, base: &mut InstrBase, ctx: &mut InstrContext<'a>, gained: u64) -> bool {
		self.try_optimize(base, ctx, gained)
	}

	fn encode<'a>(&mut self, base: &mut InstrBase, ctx: &mut InstrContext<'a>) -> Result<(ConstantOffsets, bool), IcedError> {
		match self.instr_kind {
			InstrKind::Unchanged | InstrKind::Short | InstrKind::Near => {
				if self.instr_kind == InstrKind::Unchanged {
					// nothing
				} else if self.instr_kind == InstrKind::Short {
					self.instruction.set_code(self.instruction.code().as_short_branch());
				} else {
					debug_assert!(self.instr_kind == InstrKind::Near);
					self.instruction.set_code(self.instruction.code().as_near_branch());
				}
				self.instruction.set_near_branch64(self.target_instr.address(ctx));
				ctx.block.encoder.encode(&self.instruction, ctx.ip).map_or_else(
					|err| Err(IcedError::with_string(InstrUtils::create_error_message(&err, &self.instruction))),
					|_| Ok((ctx.block.encoder.get_constant_offsets(), true)),
				)
			}

			InstrKind::Long => {
				debug_assert!(self.pointer_data.is_some());
				let pointer_data = self.pointer_data.clone().ok_or_else(|| IcedError::new("Internal error"))?;
				pointer_data.borrow_mut().data = self.target_instr.address(ctx);
				let mut instr = Instruction::default();
				instr.set_code(Self::short_br_to_native_br(
					self.instruction.code().negate_condition_code().as_short_branch(),
					ctx.block.encoder.bitness(),
				));
				if self.instruction.op_count() == 1 {
					instr.set_op0_kind(OpKind::NearBranch64);
				} else {
					#[cfg(feature = "mvex")]
					{
						debug_assert_eq!(self.instruction.op_count(), 2);
						instr.set_op0_kind(OpKind::Register);
						instr.set_op0_register(self.instruction.op0_register());
						instr.set_op1_kind(OpKind::NearBranch64);
					}
					#[cfg(not(feature = "mvex"))]
					unreachable!();
				}
				debug_assert!(ctx.block.encoder.bitness() == 64);
				debug_assert!(self.long_instruction_size64 <= i8::MAX as u32);
				instr.set_near_branch64(ctx.ip.wrapping_add(self.long_instruction_size64 as u64));
				let instr_len = ctx
					.block
					.encoder
					.encode(&instr, ctx.ip)
					.map_err(|err| IcedError::with_string(InstrUtils::create_error_message(&err, &self.instruction)))? as u32;
				InstrUtils::encode_branch_to_pointer_data(
					ctx.block,
					false,
					ctx.ip.wrapping_add(instr_len as u64),
					pointer_data,
					base.size - instr_len,
				)
				.map_or_else(
					|err| Err(IcedError::with_string(InstrUtils::create_error_message(&err, &self.instruction))),
					|_| Ok((ConstantOffsets::default(), false)),
				)
			}

			InstrKind::Uninitialized => unreachable!(),
		}
	}
}
