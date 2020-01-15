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

mod block;
mod enums;
mod instr;
#[cfg(test)]
mod tests;

use self::block::*;
pub use self::enums::*;
use self::instr::*;
use super::iced_constants::IcedConstants;
use super::*;
#[cfg(any(has_alloc, not(feature = "std")))]
use alloc::rc::Rc;
#[cfg(not(feature = "std"))]
use alloc::string::String;
#[cfg(not(feature = "std"))]
use alloc::vec::Vec;
use core::cell::RefCell;
use core::{mem, u32};
#[cfg(not(feature = "std"))]
use hashbrown::HashMap;
#[cfg(feature = "std")]
use std::collections::HashMap;
#[cfg(all(not(has_alloc), feature = "std"))]
use std::rc::Rc;

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
	#[cfg_attr(has_must_use, must_use)]
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
	#[cfg_attr(has_must_use, must_use)]
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
	/// (eg. `JE SHORT` -> `JE NEAR`), the value `u32::MAX` is stored in that element.
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
	#[cfg_attr(feature = "cargo-clippy", allow(clippy::type_complexity))]
	blocks: Vec<(Rc<RefCell<Block>>, Vec<Rc<RefCell<Instr>>>)>,
	null_encoder: Encoder,
	to_instr: HashMap<u64, Rc<RefCell<Instr>>>,
	has_multiple_zero_ip_instrs: bool,
}

impl BlockEncoder {
	fn bitness(&self) -> u32 {
		self.bitness
	}

	fn fix_branches(&self) -> bool {
		(self.options & BlockEncoderOptions::DONT_FIX_BRANCHES) == 0
	}

	fn new<'a, 'b: 'a>(bitness: u32, instr_blocks: &'a [InstructionBlock<'b>], options: u32) -> Result<Self, String> {
		if bitness != 16 && bitness != 32 && bitness != 64 {
			panic!();
		}
		let mut this = Self {
			bitness,
			options,
			blocks: Vec::with_capacity(instr_blocks.len()),
			null_encoder: Encoder::new(bitness),
			to_instr: HashMap::new(),
			has_multiple_zero_ip_instrs: false,
		};

		let mut instr_count = 0;
		for instr_block in instr_blocks.iter() {
			let instructions = instr_block.instructions;
			let block = Rc::new(RefCell::new(Block::new(
				&this,
				instr_block.rip,
				if (options & BlockEncoderOptions::RETURN_RELOC_INFOS) != 0 { Some(Vec::new()) } else { None },
			)));
			let mut instrs = Vec::with_capacity(instructions.len());
			let mut ip = instr_block.rip;
			for instruction in instructions.iter() {
				let instr = InstrUtils::create(&mut this, block.clone(), instruction);
				instr.borrow_mut().set_ip(ip);
				instrs.push(instr.clone());
				instr_count += 1;
				debug_assert!(instr.borrow().size() != 0);
				ip = ip.wrapping_add(instr.borrow().size() as u64);
			}
			this.blocks.push((block.clone(), instrs));
		}
		// Optimize from low to high addresses
		this.blocks.sort_unstable_by(|a, b| a.0.borrow().rip.cmp(&b.0.borrow().rip));

		// There must not be any instructions with the same IP, except if IP = 0 (default value)
		this.to_instr = HashMap::with_capacity(instr_count);
		for info in this.blocks.iter() {
			for instr in info.1.iter() {
				let orig_ip = instr.borrow().orig_ip();
				if this.to_instr.get(&orig_ip).is_some() {
					if orig_ip != 0 {
						return Err(format!("Multiple instructions with the same IP: 0x{:X}", orig_ip));
					}
					this.has_multiple_zero_ip_instrs = true;
				} else {
					let _ = this.to_instr.insert(orig_ip, instr.clone());
				}
			}
		}
		if this.has_multiple_zero_ip_instrs {
			let _ = this.to_instr.remove(&0);
		}

		let mut tmp_blocks = mem::replace(&mut this.blocks, Vec::new());
		for info in tmp_blocks.iter_mut() {
			let mut ip = info.0.borrow().rip;
			for instr in info.1.iter_mut() {
				let mut instr = instr.borrow_mut();
				instr.set_ip(ip);
				let old_size = instr.size();
				instr.initialize(&this);
				if instr.size() > old_size {
					panic!();
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
	/// # Panics
	///
	/// Panics if `bitness` is not one of 16, 32, 64.
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
	/// let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
	/// decoder.set_ip(0x1234_5678_9ABC_DEF0);
	/// let instructions: Vec<_> = decoder.into_iter().collect();
	///
	/// // orig_rip + 8
	/// let block = InstructionBlock::new(&instructions, 0x1234_5678_9ABC_DEF8);
	/// let bytes = match BlockEncoder::encode(64, block, BlockEncoderOptions::NONE) {
	///     Err(err) => panic!(format!("Failed: {}", err)),
	///     Ok(result) => result.code_buffer,
	/// };
	/// assert_eq!(vec![0x75, 0xF4, 0x00, 0xCE, 0x41, 0x19, 0xD9], bytes);
	/// ```
	///
	/// [`BlockEncoderOptions`]: struct.BlockEncoderOptions.html
	/// [`BlockEncoderOptions::DONT_FIX_BRANCHES`]: struct.BlockEncoderOptions.html#associatedconstant.DONT_FIX_BRANCHES
	#[inline]
	pub fn encode(bitness: u32, block: InstructionBlock, options: u32) -> Result<BlockEncoderResult, String> {
		match Self::encode_slice(bitness, &[block], options) {
			Ok(ref mut result_vec) => {
				debug_assert_eq!(1, result_vec.len());
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
	/// # Panics
	///
	/// Panics if `bitness` is not one of 16, 32, 64.
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
	/// let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
	/// decoder.set_ip(0x1234_5678_9ABC_DEF0);
	/// let instructions1: Vec<_> = decoder.into_iter().collect();
	///
	/// // je short $
	/// let bytes = b"\x75\xFE";
	/// let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
	/// decoder.set_ip(0x1234_5678);
	/// let instructions2: Vec<_> = decoder.into_iter().collect();
	///
	/// // orig_rip + 8
	/// let block1 = InstructionBlock::new(&instructions1, 0x1234_5678_9ABC_DEF8);
	/// // a new ip
	/// let block2 = InstructionBlock::new(&instructions2, 0x8000_4000_2000_1000);
	/// let bytes = match BlockEncoder::encode_slice(64, &[block1, block2], BlockEncoderOptions::NONE) {
	///     Err(err) => panic!(format!("Failed: {}", err)),
	///     Ok(result) => {
	///         assert_eq!(2, result.len());
	///         assert_eq!(vec![0x75, 0xF4, 0x00, 0xCE, 0x41, 0x19, 0xD9], result[0].code_buffer);
	///         assert_eq!(vec![0x75, 0xFE], result[1].code_buffer);
	///     }
	/// };
	/// ```
	///
	/// [`BlockEncoderOptions`]: struct.BlockEncoderOptions.html
	/// [`BlockEncoderOptions::DONT_FIX_BRANCHES`]: struct.BlockEncoderOptions.html#associatedconstant.DONT_FIX_BRANCHES
	#[inline]
	pub fn encode_slice(bitness: u32, blocks: &[InstructionBlock], options: u32) -> Result<Vec<BlockEncoderResult>, String> {
		Self::new(bitness, blocks, options)?.encode2()
	}

	fn encode2(&mut self) -> Result<Vec<BlockEncoderResult>, String> {
		for _ in 0..1000 {
			let mut updated = false;
			for info in self.blocks.iter_mut() {
				let mut ip = info.0.borrow().rip;
				for instr in info.1.iter_mut() {
					let mut instr = instr.borrow_mut();
					instr.set_ip(ip);
					let old_size = instr.size();
					if instr.optimize() {
						let instr_size = instr.size();
						if instr_size > old_size {
							return Err(String::from("Internal error: new size > old size"));
						}
						if instr_size < old_size {
							updated = true;
						}
					} else if instr.size() != old_size {
						return Err(String::from("Internal error: new size != old size"));
					}
					ip = ip.wrapping_add(instr.size() as u64);
				}
			}
			if !updated {
				break;
			}
		}

		for info in self.blocks.iter_mut() {
			info.0.borrow_mut().initialize_data(&info.1);
		}

		let mut result_vec: Vec<BlockEncoderResult> = Vec::with_capacity(self.blocks.len());
		for info in self.blocks.iter_mut() {
			let mut block = info.0.borrow_mut();
			let mut ip = block.rip;
			let mut new_instruction_offsets: Vec<u32> =
				if (self.options & BlockEncoderOptions::RETURN_NEW_INSTRUCTION_OFFSETS) != 0 { Vec::with_capacity(info.1.len()) } else { Vec::new() };
			let mut constant_offsets: Vec<ConstantOffsets> =
				if (self.options & BlockEncoderOptions::RETURN_CONSTANT_OFFSETS) != 0 { Vec::with_capacity(info.1.len()) } else { Vec::new() };
			for instr in info.1.iter_mut() {
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
					return Err(String::from("Internal error: didn't write all bytes"));
				}
				if (self.options & BlockEncoderOptions::RETURN_NEW_INSTRUCTION_OFFSETS) != 0 {
					new_instruction_offsets.push(if is_original_instruction { ip.wrapping_sub(block.rip) as u32 } else { u32::MAX });
				}
				ip = ip.wrapping_add(size as u64);
			}
			block.write_data();
			result_vec.push(BlockEncoderResult {
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
			for info in self.blocks.iter() {
				// dispose() and other clear() calls should've removed all cyclic refs
				assert_eq!(1, Rc::strong_count(&info.0));
			}
		}

		Ok(result_vec)
	}

	fn get_target(&self, instr: &Instr, address: u64) -> TargetInstr {
		if (address != 0 || !self.has_multiple_zero_ip_instrs) && instr.orig_ip() == address {
			TargetInstr::new_owner()
		} else {
			match self.to_instr.get(&address) {
				Some(instr) => TargetInstr::new_instr(instr.clone()),
				None => TargetInstr::new_address(address),
			}
		}
	}

	fn get_instruction_size(&mut self, instruction: &Instruction, ip: u64) -> u32 {
		self.null_encoder.clear_buffer();
		match self.null_encoder.encode(instruction, ip) {
			Ok(len) => len as u32,
			Err(_) => IcedConstants::MAX_INSTRUCTION_LENGTH as u32,
		}
	}
}
