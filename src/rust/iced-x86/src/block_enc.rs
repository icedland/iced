// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

mod block;
mod enums;
mod instr;
#[cfg(test)]
mod tests;

use self::block::*;
pub use self::enums::*;
use self::instr::*;
use crate::iced_constants::IcedConstants;
use crate::iced_error::IcedError;
use crate::*;
use alloc::rc::Rc;
use alloc::vec::Vec;
use core::cell::RefCell;
use core::{mem, u32};
#[cfg(not(feature = "std"))]
use hashbrown::HashMap;
#[cfg(feature = "std")]
use std::collections::HashMap;

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
	pub fn new(kind: RelocKind, address: u64) -> Self {
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
	pub fn new(instructions: &'a [Instruction], rip: u64) -> Self {
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
	bitness: u32,
	options: u32, // BlockEncoderOptions
	// .1 is 'instructions' and is barely used by Block. Had to move
	// it here because of borrowck.
	blocks: Vec<(Rc<RefCell<Block>>, Vec<Rc<RefCell<dyn Instr>>>)>,
	null_encoder: Encoder,
	to_instr: HashMap<u64, Rc<RefCell<dyn Instr>>>,
	has_multiple_zero_ip_instrs: bool,
}

impl BlockEncoder {
	fn bitness(&self) -> u32 {
		self.bitness
	}

	fn fix_branches(&self) -> bool {
		(self.options & BlockEncoderOptions::DONT_FIX_BRANCHES) == 0
	}

	fn new(bitness: u32, instr_blocks: &[InstructionBlock<'_>], options: u32) -> Result<Self, IcedError> {
		if bitness != 16 && bitness != 32 && bitness != 64 {
			return Err(IcedError::new("Invalid bitness"));
		}
		let mut this = Self {
			bitness,
			options,
			blocks: Vec::with_capacity(instr_blocks.len()),
			null_encoder: Encoder::try_new(bitness)?,
			to_instr: HashMap::new(),
			has_multiple_zero_ip_instrs: false,
		};

		let mut instr_count = 0;
		for instr_block in instr_blocks {
			let instructions = instr_block.instructions;
			let block = Rc::new(RefCell::new(Block::new(
				&this,
				instr_block.rip,
				if (options & BlockEncoderOptions::RETURN_RELOC_INFOS) != 0 { Some(Vec::new()) } else { None },
			)?));
			let mut instrs = Vec::with_capacity(instructions.len());
			let mut ip = instr_block.rip;
			for instruction in instructions {
				let instr = InstrUtils::create(&mut this, Rc::clone(&block), instruction);
				instr.borrow_mut().set_ip(ip);
				instrs.push(Rc::clone(&instr));
				instr_count += 1;
				debug_assert!(instr.borrow().size() != 0);
				ip = ip.wrapping_add(instr.borrow().size() as u64);
			}
			this.blocks.push((Rc::clone(&block), instrs));
		}
		// Optimize from low to high addresses
		this.blocks.sort_unstable_by(|a, b| a.0.borrow().rip.cmp(&b.0.borrow().rip));

		// There must not be any instructions with the same IP, except if IP = 0 (default value)
		this.to_instr = HashMap::with_capacity(instr_count);
		for info in &this.blocks {
			for instr in &info.1 {
				let orig_ip = instr.borrow().orig_ip();
				if this.to_instr.get(&orig_ip).is_some() {
					if orig_ip != 0 {
						return Err(IcedError::with_string(format!("Multiple instructions with the same IP: 0x{:X}", orig_ip)));
					}
					this.has_multiple_zero_ip_instrs = true;
				} else {
					let _ = this.to_instr.insert(orig_ip, Rc::clone(instr));
				}
			}
		}
		if this.has_multiple_zero_ip_instrs {
			let _ = this.to_instr.remove(&0);
		}

		let mut tmp_blocks = mem::replace(&mut this.blocks, Vec::new());
		for info in &mut tmp_blocks {
			let mut ip = info.0.borrow().rip;
			for instr in &mut info.1 {
				let mut instr = instr.borrow_mut();
				instr.set_ip(ip);
				let old_size = instr.size();
				instr.initialize(&this);
				if instr.size() > old_size {
					return Err(IcedError::new("Internal error"));
				}
				ip = ip.wrapping_add(instr.size() as u64);
			}
		}
		this.blocks = tmp_blocks;

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
	/// let mut decoder = Decoder::with_ip(64, bytes, 0x1234_5678_9ABC_DEF0, DecoderOptions::NONE);
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
	/// let mut decoder = Decoder::with_ip(64, bytes, 0x1234_5678_9ABC_DEF0, DecoderOptions::NONE);
	/// let instructions1: Vec<_> = decoder.into_iter().collect();
	///
	/// // je short $
	/// let bytes = b"\x75\xFE";
	/// let mut decoder = Decoder::with_ip(64, bytes, 0x1234_5678, DecoderOptions::NONE);
	/// let instructions2: Vec<_> = decoder.into_iter().collect();
	///
	/// // orig_rip + 8
	/// let block1 = InstructionBlock::new(&instructions1, 0x1234_5678_9ABC_DEF8);
	/// // a new ip
	/// let block2 = InstructionBlock::new(&instructions2, 0x8000_4000_2000_1000);
	/// let bytes = match BlockEncoder::encode_slice(64, &[block1, block2], BlockEncoderOptions::NONE) {
	///     Err(err) => panic!("Failed: {}", err),
	///     Ok(result) => {
	///         assert_eq!(result.len(), 2);
	///         assert_eq!(result[0].code_buffer, vec![0x75, 0xF4, 0x00, 0xCE, 0x41, 0x19, 0xD9]);
	///         assert_eq!(result[1].code_buffer, vec![0x75, 0xFE]);
	///     }
	/// };
	/// ```
	///
	/// [`BlockEncoderOptions`]: struct.BlockEncoderOptions.html
	/// [`BlockEncoderOptions::DONT_FIX_BRANCHES`]: struct.BlockEncoderOptions.html#associatedconstant.DONT_FIX_BRANCHES
	#[inline]
	pub fn encode_slice(bitness: u32, blocks: &[InstructionBlock<'_>], options: u32) -> Result<Vec<BlockEncoderResult>, IcedError> {
		Self::new(bitness, blocks, options)?.encode2()
	}

	fn encode2(&mut self) -> Result<Vec<BlockEncoderResult>, IcedError> {
		for _ in 0..1000 {
			let mut updated = false;
			for info in &mut self.blocks {
				let mut ip = info.0.borrow().rip;
				for instr in &mut info.1 {
					let mut instr = instr.borrow_mut();
					instr.set_ip(ip);
					let old_size = instr.size();
					if instr.optimize() {
						let instr_size = instr.size();
						if instr_size > old_size {
							return Err(IcedError::new("Internal error"));
						}
						if instr_size < old_size {
							updated = true;
						}
					} else if instr.size() != old_size {
						return Err(IcedError::new("Internal error"));
					}
					ip = ip.wrapping_add(instr.size() as u64);
				}
			}
			if !updated {
				break;
			}
		}

		for info in &mut self.blocks {
			info.0.borrow_mut().initialize_data(&info.1);
		}

		let mut result_vec: Vec<BlockEncoderResult> = Vec::with_capacity(self.blocks.len());
		for info in &mut self.blocks {
			let mut block = info.0.borrow_mut();
			let mut ip = block.rip;
			let mut new_instruction_offsets: Vec<u32> =
				if (self.options & BlockEncoderOptions::RETURN_NEW_INSTRUCTION_OFFSETS) != 0 { Vec::with_capacity(info.1.len()) } else { Vec::new() };
			let mut constant_offsets: Vec<ConstantOffsets> =
				if (self.options & BlockEncoderOptions::RETURN_CONSTANT_OFFSETS) != 0 { Vec::with_capacity(info.1.len()) } else { Vec::new() };
			for instr in &mut info.1 {
				let mut instr = instr.borrow_mut();
				let buffer_pos = block.buffer_pos();
				let is_original_instruction = if (self.options & BlockEncoderOptions::RETURN_CONSTANT_OFFSETS) != 0 {
					let result = instr.encode(&mut block)?;
					constant_offsets.push(result.0);
					result.1
				} else {
					instr.encode(&mut block)?.1
				};
				let size = block.buffer_pos() - buffer_pos;
				if size != instr.size() as usize {
					return Err(IcedError::new("Internal error"));
				}
				if (self.options & BlockEncoderOptions::RETURN_NEW_INSTRUCTION_OFFSETS) != 0 {
					new_instruction_offsets.push(if is_original_instruction { ip.wrapping_sub(block.rip) as u32 } else { u32::MAX });
				}
				ip = ip.wrapping_add(size as u64);
			}
			block.write_data()?;
			result_vec.push(BlockEncoderResult {
				rip: block.rip,
				code_buffer: block.take_buffer(),
				reloc_infos: block.take_reloc_infos(),
				new_instruction_offsets,
				constant_offsets,
			});
			block.dispose();
			info.1.clear();
		}
		self.to_instr.clear();
		if cfg!(debug_assertions) {
			for info in &self.blocks {
				// dispose() and other clear() calls should've removed all cyclic refs
				if Rc::strong_count(&info.0) != 1 {
					return Err(IcedError::new("Internal error"));
				}
			}
		}

		Ok(result_vec)
	}

	fn get_target(&self, instr: &dyn Instr, address: u64) -> TargetInstr {
		if (address != 0 || !self.has_multiple_zero_ip_instrs) && instr.orig_ip() == address {
			TargetInstr::new_owner()
		} else {
			self.to_instr.get(&address).map_or_else(|| TargetInstr::new_address(address), |instr| TargetInstr::new_instr(Rc::clone(instr)))
		}
	}

	fn get_instruction_size(&mut self, instruction: &Instruction, ip: u64) -> u32 {
		self.null_encoder.clear_buffer();
		self.null_encoder.encode(instruction, ip).map_or_else(|_| IcedConstants::MAX_INSTRUCTION_LENGTH as u32, |len| len as u32)
	}
}
