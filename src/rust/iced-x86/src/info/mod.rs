// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

pub(crate) mod cpuid_table;
pub(crate) mod enums;
pub(crate) mod factory;
pub(crate) mod info_table;
pub(crate) mod rflags_table;
#[cfg(test)]
mod tests;

use crate::iced_constants::IcedConstants;
pub use crate::info::factory::*;
use crate::*;
use alloc::vec::Vec;
use core::fmt;

/// A register used by an instruction
#[derive(Default, Copy, Clone, Eq, PartialEq, Hash)]
pub struct UsedRegister {
	register: Register,
	access: OpAccess,
}

impl UsedRegister {
	/// Creates a new instance
	///
	/// # Arguments
	///
	/// * `register`: Register
	/// * `access`: Register access
	#[must_use]
	#[inline]
	pub const fn new(register: Register, access: OpAccess) -> Self {
		Self { register, access }
	}

	/// Gets the register
	#[must_use]
	#[inline]
	pub const fn register(&self) -> Register {
		self.register
	}

	/// Gets the register access
	#[must_use]
	#[inline]
	pub const fn access(&self) -> OpAccess {
		self.access
	}
}

impl fmt::Debug for UsedRegister {
	#[allow(clippy::missing_inline_in_public_items)]
	fn fmt(&self, f: &mut fmt::Formatter<'_>) -> fmt::Result {
		write!(f, "{:?}:{:?}", self.register(), self.access())?;
		Ok(())
	}
}

/// A memory location used by an instruction
#[derive(Default, Copy, Clone, Eq, PartialEq, Hash)]
pub struct UsedMemory {
	displacement: u64,
	segment: Register,
	base: Register,
	index: Register,
	scale: u8,
	memory_size: MemorySize,
	access: OpAccess,
	address_size: CodeSize,
	vsib_size: u8,
}

impl UsedMemory {
	/// Creates a new instance
	///
	/// # Arguments
	///
	/// * `segment`: Effective segment register or [`Register::None`] if the segment register is ignored
	/// * `base`: Base register
	/// * `index`: Index register
	/// * `scale`: 1, 2, 4 or 8
	/// * `displacement`: Displacement
	/// * `memory_size`: Memory size
	/// * `access`: Access
	///
	/// [`Register::None`]: enum.Register.html#variant.None
	#[must_use]
	#[inline]
	pub const fn new(
		segment: Register, base: Register, index: Register, scale: u32, displacement: u64, memory_size: MemorySize, access: OpAccess,
	) -> Self {
		Self { segment, base, index, scale: scale as u8, displacement, memory_size, access, address_size: CodeSize::Unknown, vsib_size: 0 }
	}

	/// Creates a new instance
	///
	/// # Arguments
	///
	/// * `segment`: Effective segment register or [`Register::None`] if the segment register is ignored
	/// * `base`: Base register
	/// * `index`: Index register
	/// * `scale`: 1, 2, 4 or 8
	/// * `displacement`: Displacement
	/// * `memory_size`: Memory size
	/// * `access`: Access
	/// * `address_size`: Address size
	/// * `vsib_size`: VSIB size (`0`, `4` or `8`)
	///
	/// [`Register::None`]: enum.Register.html#variant.None
	#[must_use]
	#[inline]
	pub fn new2(
		segment: Register, base: Register, index: Register, scale: u32, displacement: u64, memory_size: MemorySize, access: OpAccess,
		address_size: CodeSize, vsib_size: u32,
	) -> Self {
		debug_assert!(vsib_size == 0 || vsib_size == 4 || vsib_size == 8);
		Self { segment, base, index, scale: scale as u8, displacement, memory_size, access, address_size, vsib_size: vsib_size as u8 }
	}

	/// Effective segment register or [`Register::None`] if the segment register is ignored
	///
	/// [`Register::None`]: enum.Register.html#variant.None
	#[must_use]
	#[inline]
	pub const fn segment(&self) -> Register {
		self.segment
	}

	/// Base register or [`Register::None`] if none
	///
	/// [`Register::None`]: enum.Register.html#variant.None
	#[must_use]
	#[inline]
	pub const fn base(&self) -> Register {
		self.base
	}

	/// Index register or [`Register::None`] if none
	///
	/// [`Register::None`]: enum.Register.html#variant.None
	#[must_use]
	#[inline]
	pub const fn index(&self) -> Register {
		self.index
	}

	/// Index scale (1, 2, 4 or 8)
	#[must_use]
	#[inline]
	pub const fn scale(&self) -> u32 {
		self.scale as u32
	}

	/// Displacement
	#[must_use]
	#[inline]
	pub const fn displacement(&self) -> u64 {
		self.displacement
	}

	/// Size of location
	#[must_use]
	#[inline]
	pub const fn memory_size(&self) -> MemorySize {
		self.memory_size
	}

	/// Memory access
	#[must_use]
	#[inline]
	pub const fn access(&self) -> OpAccess {
		self.access
	}

	/// Address size
	#[must_use]
	#[inline]
	pub const fn address_size(&self) -> CodeSize {
		self.address_size
	}

	/// VSIB size (`0`, `4` or `8`)
	#[must_use]
	#[inline]
	pub const fn vsib_size(&self) -> u32 {
		self.vsib_size as u32
	}

	/// Gets the virtual address of a used memory location, or `None` if register resolution fails.
	///
	/// # Arguments
	///
	/// * `get_register_value`: Function that returns the value of a register or the base address of a segment register, or `None` on failure.
	///
	/// # Call-back function args
	///
	/// * Arg 1: `register`: Register. If it's a segment register, the call-back should return the segment's base address, not the segment's register value.
	/// * Arg 2: `element_index`: Only used if it's a vsib memory operand. This is the element index of the vector index register.
	/// * Arg 3: `element_size`: Only used if it's a vsib memory operand. Size in bytes of elements in vector index register (4 or 8).
	#[must_use]
	#[inline]
	pub fn virtual_address<F>(&self, element_index: usize, get_register_value: F) -> Option<u64>
	where
		F: FnMut(Register, usize, usize) -> Option<u64>,
	{
		self.try_virtual_address(element_index, get_register_value)
	}

	#[must_use]
	#[inline]
	#[doc(hidden)]
	pub fn try_virtual_address<F>(&self, element_index: usize, mut get_register_value: F) -> Option<u64>
	where
		F: FnMut(Register, usize, usize) -> Option<u64>,
	{
		let mut effective = self.displacement;

		match self.base {
			Register::None => {}
			_ => {
				let base = get_register_value(self.base, 0, 0)?;
				effective = effective.wrapping_add(base)
			}
		}

		match self.index {
			Register::None => {}
			_ => {
				let mut index = get_register_value(self.index, element_index, self.vsib_size as usize)?;
				if self.vsib_size == 4 {
					index = index as i32 as u64;
				}
				effective = effective.wrapping_add(index.wrapping_mul(self.scale as u64))
			}
		}

		match self.address_size {
			CodeSize::Code16 => effective = effective as u16 as u64,
			CodeSize::Code32 => effective = effective as u32 as u64,
			_ => {}
		}

		match self.segment {
			Register::None => {}
			_ => {
				let segment_base = get_register_value(self.segment, 0, 0)?;
				effective = effective.wrapping_add(segment_base)
			}
		}

		Some(effective)
	}
}

impl fmt::Debug for UsedMemory {
	#[allow(clippy::missing_inline_in_public_items)]
	fn fmt(&self, f: &mut fmt::Formatter<'_>) -> fmt::Result {
		write!(f, "[{:?}:", self.segment())?;
		let mut need_plus = if self.base() != Register::None {
			write!(f, "{:?}", self.base())?;
			true
		} else {
			false
		};
		if self.index() != Register::None {
			if need_plus {
				write!(f, "+")?;
			}
			need_plus = true;
			write!(f, "{:?}", self.index())?;
			if self.scale() != 1 {
				write!(f, "*{}", self.scale())?;
			}
		}
		if self.displacement() != 0 || !need_plus {
			if need_plus {
				write!(f, "+")?;
			}
			if self.displacement() <= 9 {
				write!(f, "{}", self.displacement())?;
			} else {
				write!(f, "0x{:X}", self.displacement())?;
			}
		}
		write!(f, ";{:?};{:?}]", self.memory_size(), self.access())?;
		Ok(())
	}
}

/// Contains information about an instruction, eg. read/written registers and memory locations, operand accesses.
/// Created by an [`InstructionInfoFactory`].
///
/// [`InstructionInfoFactory`]: struct.InstructionInfoFactory.html
#[derive(Debug, Clone)]
pub struct InstructionInfo {
	used_registers: Vec<UsedRegister>,
	used_memory_locations: Vec<UsedMemory>,
	op_accesses: [OpAccess; IcedConstants::MAX_OP_COUNT],
}

impl InstructionInfo {
	#[must_use]
	#[inline(always)]
	fn new(options: u32) -> Self {
		use crate::info::enums::InstrInfoConstants;
		Self {
			used_registers: if (options & InstructionInfoOptions::NO_REGISTER_USAGE) == 0 {
				Vec::with_capacity(InstrInfoConstants::DEFAULT_USED_REGISTER_COLL_CAPACITY)
			} else {
				Vec::new()
			},
			used_memory_locations: if (options & InstructionInfoOptions::NO_MEMORY_USAGE) == 0 {
				Vec::with_capacity(InstrInfoConstants::DEFAULT_USED_MEMORY_COLL_CAPACITY)
			} else {
				Vec::new()
			},
			op_accesses: [OpAccess::default(); IcedConstants::MAX_OP_COUNT],
		}
	}

	/// Gets all accessed registers. This method doesn't return all accessed registers if [`is_save_restore_instruction()`] is `true`.
	///
	/// Some instructions have a `r16`/`r32` operand but only use the low 8 bits of the register. In that case
	/// this method returns the 8-bit register even if it's `SPL`, `BPL`, `SIL`, `DIL` and the
	/// instruction was decoded in 16 or 32-bit mode. This is more accurate than returning the `r16`/`r32`
	/// register. Example instructions that do this: `PINSRB`, `ARPL`
	///
	/// [`is_save_restore_instruction()`]: #method.is_save_restore_instruction
	#[must_use]
	#[inline]
	pub fn used_registers(&self) -> &[UsedRegister] {
		self.used_registers.as_slice()
	}

	/// Gets all accessed memory locations
	#[must_use]
	#[inline]
	pub fn used_memory(&self) -> &[UsedMemory] {
		self.used_memory_locations.as_slice()
	}

	/// Operand #0 access
	#[must_use]
	#[inline]
	pub const fn op0_access(&self) -> OpAccess {
		self.op_accesses[0]
	}

	/// Operand #1 access
	#[must_use]
	#[inline]
	pub const fn op1_access(&self) -> OpAccess {
		self.op_accesses[1]
	}

	/// Operand #2 access
	#[must_use]
	#[inline]
	pub const fn op2_access(&self) -> OpAccess {
		self.op_accesses[2]
	}

	/// Operand #3 access
	#[must_use]
	#[inline]
	pub const fn op3_access(&self) -> OpAccess {
		self.op_accesses[3]
	}

	/// Operand #4 access
	#[must_use]
	#[inline]
	pub const fn op4_access(&self) -> OpAccess {
		self.op_accesses[4]
	}

	/// Gets operand access
	///
	/// # Arguments
	///
	/// * `operand`: Operand number, 0-4
	#[must_use]
	#[inline]
	pub fn op_access(&self, operand: u32) -> OpAccess {
		match self.op_accesses.get(operand as usize) {
			Some(&value) => value,
			None => {
				debug_assert!(false, "Invalid operand: {}", operand);
				OpAccess::default()
			}
		}
	}

	#[inline]
	#[doc(hidden)]
	pub fn try_op_access(&self, operand: u32) -> Result<OpAccess, IcedError> {
		self.op_accesses.get(operand as usize).map_or_else(|| Err(IcedError::new("Invalid operand")), |&op_access| Ok(op_access))
	}
}
