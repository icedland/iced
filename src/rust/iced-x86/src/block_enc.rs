// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

mod block;
mod enums;
mod instr;
#[cfg(test)]
mod tests;

use crate::block_enc::block::*;
pub use crate::block_enc::enums::*;
use crate::block_enc::instr::*;
use crate::iced_constants::IcedConstants;
use crate::iced_error::IcedError;
use crate::*;
use alloc::boxed::Box;
use alloc::rc::Rc;
use alloc::vec::Vec;

/// Relocation info
#[derive(Debug, Copy, Clone, Eq, PartialEq, Hash)]
pub struct RelocInfo {
	/// Address
	pub address: u64,

	/// Relocation kind
	pub kind: RelocKind,
}

impl RelocInfo {
	/// Constructor
	///
	/// # Arguments
	///
	/// * `kind`: Relocation kind
	/// * `address`: Address
	#[must_use]
	#[inline]
	pub const fn new(kind: RelocKind, address: u64) -> Self {
		Self { address, kind }
	}
}

/// Contains a slice of instructions that should be encoded by [`BlockEncoder`]
///
/// [`BlockEncoder`]: struct.BlockEncoder.html
#[derive(Debug)]
pub struct InstructionBlock<'a> {
	instructions: &'a [Instruction],
	rip: u64,
}

impl<'a> InstructionBlock<'a> {
	/// Constructor
	///
	/// # Arguments
	///
	/// * `instructions`: All instructions
	/// * `rip`: Base IP of all encoded instructions
	#[must_use]
	#[inline]
	pub const fn new(instructions: &'a [Instruction], rip: u64) -> Self {
		Self { instructions, rip }
	}
}

/// [`BlockEncoder`] result if it was successful
///
/// [`BlockEncoder`]: struct.BlockEncoder.html
#[derive(Debug)]
pub struct BlockEncoderResult {
	/// Base IP of all encoded instructions
	pub rip: u64,

	/// The bytes of all encoded instructions
	pub code_buffer: Vec<u8>,

	/// If [`BlockEncoderOptions::RETURN_RELOC_INFOS`] option was enabled:
	///
	/// All [`RelocInfo`]s.
	///
	/// [`BlockEncoderOptions::RETURN_RELOC_INFOS`]: struct.BlockEncoderOptions.html#associatedconstant.RETURN_RELOC_INFOS
	/// [`RelocInfo`]: struct.RelocInfo.html
	pub reloc_infos: Vec<RelocInfo>,

	/// If [`BlockEncoderOptions::RETURN_NEW_INSTRUCTION_OFFSETS`] option was enabled:
	///
	/// Offsets of the instructions relative to the base IP. If the instruction was rewritten to a new instruction
	/// (eg. `JE TARGET_TOO_FAR_AWAY` -> `JNE SHORT SKIP ; JMP QWORD PTR [MEM]`), the value `u32::MAX` is stored in that element.
	///
	/// [`BlockEncoderOptions::RETURN_NEW_INSTRUCTION_OFFSETS`]: struct.BlockEncoderOptions.html#associatedconstant.RETURN_NEW_INSTRUCTION_OFFSETS
	pub new_instruction_offsets: Vec<u32>,

	/// If [`BlockEncoderOptions::RETURN_CONSTANT_OFFSETS`] option was enabled:
	///
	/// Offsets of all constants in the new encoded instructions. If the instruction was rewritten,
	/// the `default()` value is stored in the corresponding element.
	///
	/// [`BlockEncoderOptions::RETURN_CONSTANT_OFFSETS`]: struct.BlockEncoderOptions.html#associatedconstant.RETURN_CONSTANT_OFFSETS
	pub constant_offsets: Vec<ConstantOffsets>,
}

/// Encodes instructions. It can be used to move instructions from one location to another location.
#[allow(missing_debug_implementations)]
pub struct BlockEncoder {
	// The usizes are the same as block.instr_indexes, but inlined here for a small perf increase.
	blocks: Vec<(Block, usize, usize)>,
	all_instrs: Vec<(InstrBase, Box<dyn Instr>)>,
	all_ips: Vec<u64>,
	benc: BlockEncInt,
}

struct BlockEncInt {
	bitness: u32,
	options: u32, // BlockEncoderOptions
	null_encoder: Encoder,
	to_instr_index: Vec<(u64, usize)>,
	has_multiple_zero_ip_instrs: bool,
}

impl BlockEncInt {
	const fn bitness(&self) -> u32 {
		self.bitness
	}

	const fn fix_branches(&self) -> bool {
		(self.options & BlockEncoderOptions::DONT_FIX_BRANCHES) == 0
	}

	fn get_target(&self, base: &InstrBase, address: u64) -> TargetInstr {
		if (address != 0 || !self.has_multiple_zero_ip_instrs) && base.orig_ip == address {
			TargetInstr::new_owner()
		} else {
			match self.to_instr_index.binary_search_by(|(ip, _)| address.cmp(ip)) {
				Ok(index) => TargetInstr::new_instr(self.to_instr_index[index].1),
				Err(_) => TargetInstr::new_address(address),
			}
		}
	}

	fn get_instruction_size(&mut self, instruction: &Instruction, ip: u64) -> u32 {
		self.null_encoder.clear_buffer();
		self.null_encoder.encode(instruction, ip).map_or_else(|_| IcedConstants::MAX_INSTRUCTION_LENGTH as u32, |len| len as u32)
	}
}

impl BlockEncoder {
	fn new(bitness: u32, instr_blocks: &[InstructionBlock<'_>], options: u32) -> Result<Self, IcedError> {
		if bitness != 16 && bitness != 32 && bitness != 64 {
			return Err(IcedError::new("Invalid bitness"));
		}
		let total_instr_len = instr_blocks.iter().map(|b| b.instructions.len()).sum();
		let mut this = Self {
			blocks: Vec::with_capacity(instr_blocks.len()),
			all_instrs: Vec::with_capacity(total_instr_len),
			all_ips: Vec::with_capacity(total_instr_len),
			benc: BlockEncInt {
				bitness,
				options,
				null_encoder: Encoder::try_new(bitness)?,
				to_instr_index: Vec::new(),
				has_multiple_zero_ip_instrs: false,
			},
		};

		let mut instr_count = 0;
		for instr_block in instr_blocks {
			let instructions = instr_block.instructions;
			instr_count += instructions.len();
			let mut ip = instr_block.rip;
			let start_index = this.all_instrs.len();
			for instruction in instructions {
				let mut base = InstrBase { orig_ip: instruction.ip(), size: 0, done: false };
				let instr = InstrUtils::create(&mut this.benc, &mut base, instruction);
				debug_assert!(base.size != 0 || instruction.code() == Code::Zero_bytes);
				ip = ip.wrapping_add(base.size as u64);
				this.all_ips.push(ip);
				this.all_instrs.push((base, instr));
			}
			let end_index = this.all_instrs.len();
			let block = Block::new(
				this.benc.bitness,
				instr_block.rip,
				if (options & BlockEncoderOptions::RETURN_RELOC_INFOS) != 0 { Some(Vec::new()) } else { None },
				start_index,
				end_index,
			)?;
			this.blocks.push((block, start_index, end_index));
		}
		// Optimize from low to high addresses
		this.blocks.sort_unstable_by(|a, b| a.0.rip.cmp(&b.0.rip));

		this.benc.to_instr_index = Vec::with_capacity(instr_count);
		// There must not be any instructions with the same IP, except if IP = 0 (default value)
		let mut num_ip_0 = 0;
		for info in &this.blocks {
			// Reverse here since we'll sort them in reverse order, see below
			for (i, (base, _)) in this.all_instrs[info.1..info.2].iter().enumerate().rev() {
				let orig_ip = base.orig_ip;
				let insert = if orig_ip == 0 {
					num_ip_0 += 1;
					num_ip_0 == 1
				} else {
					true
				};
				if insert {
					this.benc.to_instr_index.push((orig_ip, info.1 + i));
				}
			}
		}
		// We sort them in reverse order so that if we must remove the 'ip==0' entry, we just need to pop()
		this.benc.to_instr_index.sort_unstable_by(|a, b| b.0.cmp(&a.0));
		if num_ip_0 > 1 {
			this.benc.has_multiple_zero_ip_instrs = true;
			if let Some(kv) = this.benc.to_instr_index.last() {
				debug_assert_eq!(kv.0, 0);
				if kv.0 == 0 {
					let _ = this.benc.to_instr_index.pop();
				}
			}
		}
		let mut prev_ip = this.benc.to_instr_index.first().map(|(ip, _)| *ip).unwrap_or_default();
		for (ip, _) in this.benc.to_instr_index.iter().skip(1) {
			if *ip == prev_ip {
				return Err(IcedError::with_string(format!("Multiple instructions with the same IP: 0x{:X}", ip)));
			}
			prev_ip = *ip;
		}

		for info in &mut this.blocks {
			let mut ip = info.0.rip;
			for (i, (base, instr)) in this.all_instrs[info.1..info.2].iter_mut().enumerate() {
				this.all_ips[info.1 + i] = ip;
				if !base.done {
					let (target_instr, target_ip) = instr.get_target_instr();
					*target_instr = this.benc.get_target(base, target_ip);
				}
				ip = ip.wrapping_add(base.size as u64);
			}
		}

		Ok(this)
	}

	/// Encodes instructions. Any number of branches can be part of this block.
	/// You can use this function to move instructions from one location to another location.
	/// If the target of a branch is too far away, it'll be rewritten to a longer branch.
	/// You can disable this by passing in [`BlockEncoderOptions::DONT_FIX_BRANCHES`].
	/// If the block has any `RIP`-relative memory operands, make sure the data isn't too
	/// far away from the new location of the encoded instructions. Every OS should have
	/// some API to allocate memory close (+/-2GB) to the original code location.
	///
	/// # Errors
	///
	/// Returns an error if it failed to encode one or more instructions.
	///
	/// # Arguments
	///
	/// * `bitness`: 16, 32, or 64
	/// * `block`: All instructions
	/// * `options`: Encoder options, see [`BlockEncoderOptions`]
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	///
	/// // je short $-2
	/// // add dh,cl
	/// // sbb r9d,ebx
	/// let bytes = b"\x75\xFC\x00\xCE\x41\x19\xD9";
	/// let decoder = Decoder::with_ip(64, bytes, 0x1234_5678_9ABC_DEF0, DecoderOptions::NONE);
	/// let instructions: Vec<_> = decoder.into_iter().collect();
	///
	/// // orig_rip + 8
	/// let block = InstructionBlock::new(&instructions, 0x1234_5678_9ABC_DEF8);
	/// let bytes = match BlockEncoder::encode(64, block, BlockEncoderOptions::NONE) {
	///     Err(err) => panic!("Failed: {}", err),
	///     Ok(result) => result.code_buffer,
	/// };
	/// assert_eq!(bytes, vec![0x75, 0xF4, 0x00, 0xCE, 0x41, 0x19, 0xD9]);
	/// ```
	///
	/// [`BlockEncoderOptions`]: struct.BlockEncoderOptions.html
	/// [`BlockEncoderOptions::DONT_FIX_BRANCHES`]: struct.BlockEncoderOptions.html#associatedconstant.DONT_FIX_BRANCHES
	#[inline]
	pub fn encode(bitness: u32, block: InstructionBlock<'_>, options: u32) -> Result<BlockEncoderResult, IcedError> {
		match Self::encode_slice(bitness, &[block], options) {
			Ok(ref mut result_vec) => {
				debug_assert_eq!(result_vec.len(), 1);
				Ok(result_vec.remove(0))
			}
			Err(err) => Err(err),
		}
	}

	/// Encodes instructions. Any number of branches can be part of this block.
	/// You can use this function to move instructions from one location to another location.
	/// If the target of a branch is too far away, it'll be rewritten to a longer branch.
	/// You can disable this by passing in [`BlockEncoderOptions::DONT_FIX_BRANCHES`].
	/// If the block has any `RIP`-relative memory operands, make sure the data isn't too
	/// far away from the new location of the encoded instructions. Every OS should have
	/// some API to allocate memory close (+/-2GB) to the original code location.
	///
	/// # Errors
	///
	/// Returns an error if it failed to encode one or more instructions.
	///
	/// # Arguments
	///
	/// * `bitness`: 16, 32, or 64
	/// * `blocks`: All instructions
	/// * `options`: Encoder options, see [`BlockEncoderOptions`]
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	///
	/// // je short $-2
	/// // add dh,cl
	/// // sbb r9d,ebx
	/// let bytes = b"\x75\xFC\x00\xCE\x41\x19\xD9";
	/// let decoder = Decoder::with_ip(64, bytes, 0x1234_5678_9ABC_DEF0, DecoderOptions::NONE);
	/// let instructions1: Vec<_> = decoder.into_iter().collect();
	///
	/// // je short $
	/// let bytes = b"\x75\xFE";
	/// let decoder = Decoder::with_ip(64, bytes, 0x1234_5678, DecoderOptions::NONE);
	/// let instructions2: Vec<_> = decoder.into_iter().collect();
	///
	/// // orig_rip + 8
	/// let block1 = InstructionBlock::new(&instructions1, 0x1234_5678_9ABC_DEF8);
	/// // a new ip
	/// let block2 = InstructionBlock::new(&instructions2, 0x8000_4000_2000_1000);
	/// let bytes = match BlockEncoder::encode_slice(64, &[block1, block2], BlockEncoderOptions::NONE) {
	///     Err(err) => panic!("Failed: {}", err),
	///     Ok(result) => result,
	/// };
	/// assert_eq!(bytes.len(), 2);
	/// assert_eq!(bytes[0].code_buffer, vec![0x75, 0xF4, 0x00, 0xCE, 0x41, 0x19, 0xD9]);
	/// assert_eq!(bytes[1].code_buffer, vec![0x75, 0xFE]);
	/// ```
	///
	/// [`BlockEncoderOptions`]: struct.BlockEncoderOptions.html
	/// [`BlockEncoderOptions::DONT_FIX_BRANCHES`]: struct.BlockEncoderOptions.html#associatedconstant.DONT_FIX_BRANCHES
	#[inline]
	pub fn encode_slice(bitness: u32, blocks: &[InstructionBlock<'_>], options: u32) -> Result<Vec<BlockEncoderResult>, IcedError> {
		Self::new(bitness, blocks, options)?.encode2()
	}

	fn encode2(&mut self) -> Result<Vec<BlockEncoderResult>, IcedError> {
		// 5 iters is enough even if millions of instructions are encoded. < 10 instructions are optimized per loop
		// iteration after only a few loop iters. It's not worth optimizing the remaining few instructions.
		for _ in 0..5 {
			let mut updated = false;
			for info in &mut self.blocks {
				let mut gained = 0;
				let block_rip = info.0.rip;
				let mut ctx = InstrContext { block: &mut info.0, all_ips: &mut self.all_ips, ip: block_rip };
				for (i, (base, instr)) in self.all_instrs[info.1..info.2].iter_mut().enumerate() {
					ctx.all_ips[info.1 + i] = ctx.ip;
					// If it can't be optimized further, don't call its virtual optimize() fn for a nice speedup
					if !base.done {
						let old_size = base.size;
						if instr.optimize(base, &mut ctx, gained) {
							let instr_size = base.size;
							if instr_size > old_size {
								return Err(IcedError::new("Internal error"));
							}
							if instr_size < old_size {
								gained += (old_size - instr_size) as u64;
								updated = true;
							}
						} else if base.size != old_size {
							return Err(IcedError::new("Internal error"));
						}
					}
					ctx.ip = ctx.ip.wrapping_add(base.size as u64);
				}
			}
			if !updated {
				break;
			}
		}

		for info in &mut self.blocks {
			let index = info.2.wrapping_sub(1);
			if let Some((last_base, _)) = self.all_instrs.get(index) {
				let last_ip = self.all_ips[index];
				let after_addr = last_ip.wrapping_add(last_base.size as u64);
				info.0.initialize_data(after_addr);
			} else {
				debug_assert_eq!(info.2, 0);
			}
		}

		let mut result_vec: Vec<BlockEncoderResult> = Vec::with_capacity(self.blocks.len());
		for info in &mut self.blocks {
			let instr_count = info.2 - info.1;
			let mut new_instruction_offsets: Vec<u32> = if (self.benc.options & BlockEncoderOptions::RETURN_NEW_INSTRUCTION_OFFSETS) != 0 {
				Vec::with_capacity(instr_count)
			} else {
				Vec::new()
			};
			let mut constant_offsets: Vec<ConstantOffsets> =
				if (self.benc.options & BlockEncoderOptions::RETURN_CONSTANT_OFFSETS) != 0 { Vec::with_capacity(instr_count) } else { Vec::new() };
			let block_rip = info.0.rip;
			let mut ctx = InstrContext { block: &mut info.0, all_ips: &mut self.all_ips, ip: block_rip };
			for (base, instr) in &mut self.all_instrs[info.1..info.2] {
				let buffer_pos = ctx.block.buffer_pos();
				let is_original_instruction = if (self.benc.options & BlockEncoderOptions::RETURN_CONSTANT_OFFSETS) != 0 {
					let result = instr.encode(base, &mut ctx)?;
					constant_offsets.push(result.0);
					result.1
				} else {
					instr.encode(base, &mut ctx)?.1
				};
				let size = ctx.block.buffer_pos() - buffer_pos;
				if size != base.size as usize {
					return Err(IcedError::new("Internal error"));
				}
				if (self.benc.options & BlockEncoderOptions::RETURN_NEW_INSTRUCTION_OFFSETS) != 0 {
					new_instruction_offsets.push(if is_original_instruction { ctx.ip.wrapping_sub(ctx.block.rip) as u32 } else { u32::MAX });
				}
				ctx.ip = ctx.ip.wrapping_add(size as u64);
			}
			info.0.write_data()?;
			result_vec.push(BlockEncoderResult {
				rip: info.0.rip,
				code_buffer: info.0.take_buffer(),
				reloc_infos: info.0.take_reloc_infos(),
				new_instruction_offsets,
				constant_offsets,
			});
			info.0.dispose();
		}

		Ok(result_vec)
	}
}
