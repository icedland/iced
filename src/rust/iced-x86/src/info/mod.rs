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

pub(crate) mod cpuid_table;
pub(crate) mod enums;
pub(crate) mod factory;
pub(crate) mod info_table;
pub(crate) mod rflags_table;
#[cfg(test)]
mod tests;

pub use self::factory::*;
use super::iced_constants::IcedConstants;
use super::*;
#[cfg(not(feature = "std"))]
use alloc::vec::Vec;
use core::fmt;

extern crate num_traits;
use self::num_traits::{AsPrimitive, WrappingAdd, WrappingMul};

/// A register used by an instruction
#[derive(Default, Copy, Clone, Eq, PartialEq, Hash)]
pub struct UsedRegister {
	register: Register,
	access: OpAccess,
}

#[cfg_attr(feature = "cargo-clippy", allow(clippy::trivially_copy_pass_by_ref))]
impl UsedRegister {
	/// Creates a new instance
	///
	/// # Arguments
	///
	/// * `register`: Register
	/// * `access`: Register access
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn new(register: Register, access: OpAccess) -> Self {
		Self { register, access }
	}

	/// Gets the register
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn register(&self) -> Register {
		self.register
	}

	/// Gets the register access
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn access(&self) -> OpAccess {
		self.access
	}
}

impl fmt::Debug for UsedRegister {
	#[cfg_attr(feature = "cargo-clippy", allow(clippy::missing_inline_in_public_items))]
	fn fmt<'a>(&self, f: &mut fmt::Formatter<'a>) -> fmt::Result {
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
	/// * `segment`: Effective segment register
	/// * `base`: Base register
	/// * `index`: Index register
	/// * `scale`: 1, 2, 4 or 8
	/// * `displacement`: Displacement
	/// * `memory_size`: Memory size
	/// * `access`: Access
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn new(segment: Register, base: Register, index: Register, scale: u32, displacement: u64, memory_size: MemorySize, access: OpAccess) -> Self {
		Self { segment, base, index, scale: scale as u8, displacement, memory_size, access, address_size: CodeSize::Unknown, vsib_size: 0 }
	}

	/// Creates a new instance
	///
	/// # Arguments
	///
	/// * `segment`: Effective segment register
	/// * `base`: Base register
	/// * `index`: Index register
	/// * `scale`: 1, 2, 4 or 8
	/// * `displacement`: Displacement
	/// * `memory_size`: Memory size
	/// * `access`: Access
	/// * `address_size`: Address size
	/// * `vsib_size`: VSIB size (`0`, `4` or `8`)
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn new2(
		segment: Register, base: Register, index: Register, scale: u32, displacement: u64, memory_size: MemorySize, access: OpAccess,
		address_size: CodeSize, vsib_size: u32,
	) -> Self {
		debug_assert!(vsib_size == 0 || vsib_size == 4 || vsib_size == 8);
		Self { segment, base, index, scale: scale as u8, displacement, memory_size, access, address_size, vsib_size: vsib_size as u8 }
	}

	/// Effective segment register
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn segment(&self) -> Register {
		self.segment
	}

	/// Base register or [`Register::None`] if none
	///
	/// [`Register::None`]: enum.Register.html#variant.None
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn base(&self) -> Register {
		self.base
	}

	/// Index register or [`Register::None`] if none
	///
	/// [`Register::None`]: enum.Register.html#variant.None
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn index(&self) -> Register {
		self.index
	}

	/// Index scale (1, 2, 4 or 8)
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn scale(&self) -> u32 {
		self.scale as u32
	}

	/// Displacement
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn displacement(&self) -> u64 {
		self.displacement
	}

	/// Size of location
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn memory_size(&self) -> MemorySize {
		self.memory_size
	}

	/// Memory access
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn access(&self) -> OpAccess {
		self.access
	}

	/// Address size
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn address_size(&self) -> CodeSize {
		self.address_size
	}

	/// VSIB size (`0`, `4` or `8`)
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn vsib_size(&self) -> u32 {
		self.vsib_size as u32
	}

	/// Gets the virtual address of a used memory location. See also [`try_virtual_address()`]
	///
	/// [`try_virtual_address()`]: #method.try_virtual_address
	///
	/// # Panics
	///
	/// Panics if virtual address computation fails.
	///
	/// # Arguments
	///
	/// * `get_register_value`: Function that returns the value of a register or the base address of a segment register.
	///
	/// # Call-back function args
	///
	/// * Arg 1: `register`: Register. If it's a segment register, the call-back should return the segment's base value, not the segment register value.
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn virtual_address<T, F>(&self, mut get_register_value: F) -> u64
	where
		T: Copy + WrappingAdd + WrappingMul + Into<u64> + 'static,
		u8: AsPrimitive<T>,
		u32: AsPrimitive<T>,
		u64: AsPrimitive<T>,
		F: FnMut(Register) -> u64,
	{
		self.try_virtual_address(|r| Some(get_register_value(r))).unwrap()
	}

	/// Gets the virtual address of a used memory location, or `None` if register resolution fails.
	///
	/// # Arguments
	///
	/// * `get_register_value`: Function that returns the value of a register or the base address of a segment register, or `None` on failure.
	///
	/// # Call-back function args
	///
	/// * Arg 1: `register`: Register. If it's a segment register, the call-back should return the segment's base value, not the segment register value.
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn try_virtual_address<T, F>(&self, mut get_register_value: F) -> Option<u64>
	where
		T: Copy + WrappingAdd + WrappingMul + Into<u64> + 'static,
		u8: AsPrimitive<T>,
		u32: AsPrimitive<T>,
		u64: AsPrimitive<T>,
		F: FnMut(Register) -> Option<u64>,
	{
		let segment_base = get_register_value(self.segment)?.as_();
		let base = get_register_value(self.base)?.as_();
		let index = get_register_value(self.index)?.as_();

		let effective = segment_base.wrapping_add(&base).wrapping_add(&index.wrapping_mul(&self.scale.as_())).wrapping_add(&self.displacement.as_());

		Some(effective.into())
	}
}

impl fmt::Debug for UsedMemory {
	#[cfg_attr(feature = "cargo-clippy", allow(clippy::missing_inline_in_public_items))]
	fn fmt<'a>(&self, f: &mut fmt::Formatter<'a>) -> fmt::Result {
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

struct IIFlags;
impl IIFlags {
	const SAVE_RESTORE: u8 = 0x20;
	const STACK_INSTRUCTION: u8 = 0x40;
	const PRIVILEGED: u8 = 0x80;
}

/// Contains information about an instruction, eg. read/written registers, read/written `RFLAGS` bits, `CPUID` feature bit, etc.
/// Created by an [`InstructionInfoFactory`].
///
/// [`InstructionInfoFactory`]: struct.InstructionInfoFactory.html
#[derive(Debug, Clone)]
pub struct InstructionInfo {
	used_registers: Vec<UsedRegister>,
	used_memory_locations: Vec<UsedMemory>,
	cpuid_feature_internal: usize,
	rflags_info: usize,
	flow_control: FlowControl,
	op_accesses: [OpAccess; IcedConstants::MAX_OP_COUNT],
	encoding: EncodingKind,
	flags: u8,
}

impl InstructionInfo {
	#[cfg_attr(has_must_use, must_use)]
	#[inline(always)]
	fn new(options: u32) -> Self {
		use self::enums::InstrInfoConstants;
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
			cpuid_feature_internal: 0,
			rflags_info: 0,
			flow_control: FlowControl::default(),
			op_accesses: [OpAccess::default(); IcedConstants::MAX_OP_COUNT],
			encoding: EncodingKind::default(),
			flags: 0,
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
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn used_registers(&self) -> &[UsedRegister] {
		self.used_registers.as_slice()
	}

	/// Gets all accessed memory locations
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn used_memory(&self) -> &[UsedMemory] {
		self.used_memory_locations.as_slice()
	}

	/// `true` if it's a privileged instruction (all CPL=0 instructions (except `VMCALL`) and IOPL instructions `IN`, `INS`, `OUT`, `OUTS`, `CLI`, `STI`)
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn is_privileged(&self) -> bool {
		(self.flags & IIFlags::PRIVILEGED) != 0
	}

	/// `true` if this is an instruction that implicitly uses the stack pointer (`SP`/`ESP`/`RSP`), eg. `CALL`, `PUSH`, `POP`, `RET`, etc.
	/// See also [`Instruction::stack_pointer_increment()`]
	///
	/// [`Instruction::stack_pointer_increment()`]: struct.Instruction.html#method.stack_pointer_increment
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn is_stack_instruction(&self) -> bool {
		(self.flags & IIFlags::STACK_INSTRUCTION) != 0
	}

	/// `true` if it's an instruction that saves or restores too many registers (eg. `FXRSTOR`, `XSAVE`, etc).
	/// [`used_registers()`] won't return all accessed registers.
	///
	/// [`used_registers()`]: #method.used_registers
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn is_save_restore_instruction(&self) -> bool {
		(self.flags & IIFlags::SAVE_RESTORE) != 0
	}

	/// Instruction encoding, eg. Legacy, 3DNow!, VEX, EVEX, XOP
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn encoding(&self) -> EncodingKind {
		self.encoding
	}

	/// Gets the CPU or CPUID feature flags
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn cpuid_features(&self) -> &'static [CpuidFeature] {
		unsafe { *self::cpuid_table::CPUID.get_unchecked(self.cpuid_feature_internal) }
	}

	/// Control flow info
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn flow_control(&self) -> FlowControl {
		self.flow_control
	}

	/// Operand #0 access
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn op0_access(&self) -> OpAccess {
		self.op_accesses[0]
	}

	/// Operand #1 access
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn op1_access(&self) -> OpAccess {
		self.op_accesses[1]
	}

	/// Operand #2 access
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn op2_access(&self) -> OpAccess {
		self.op_accesses[2]
	}

	/// Operand #3 access
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn op3_access(&self) -> OpAccess {
		self.op_accesses[3]
	}

	/// Operand #4 access
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn op4_access(&self) -> OpAccess {
		self.op_accesses[4]
	}

	/// Gets operand access
	///
	/// # Panics
	///
	/// Panics if `operand` is invalid
	///
	/// # Arguments
	///
	/// * `operand`: Operand number, 0-4
	#[cfg_attr(has_must_use, must_use)]
	#[cfg_attr(feature = "cargo-clippy", allow(clippy::missing_inline_in_public_items))]
	pub fn op_access(&self, operand: u32) -> OpAccess {
		self.op_accesses[operand as usize]
	}

	/// All flags that are read by the CPU when executing the instruction.
	/// This method returns a [`RflagsBits`] value. See also [`rflags_modified()`].
	///
	/// [`RflagsBits`]: struct.RflagsBits.html
	/// [`rflags_modified()`]: #method.rflags_modified
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn rflags_read(&self) -> u32 {
		unsafe { *super::info::rflags_table::FLAGS_READ.get_unchecked(self.rflags_info) as u32 }
	}

	/// All flags that are written by the CPU, except those flags that are known to be undefined, always set or always cleared.
	/// This method returns a [`RflagsBits`] value. See also [`rflags_modified()`].
	///
	/// [`RflagsBits`]: struct.RflagsBits.html
	/// [`rflags_modified()`]: #method.rflags_modified
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn rflags_written(&self) -> u32 {
		unsafe { *super::info::rflags_table::FLAGS_WRITTEN.get_unchecked(self.rflags_info) as u32 }
	}

	/// All flags that are always cleared by the CPU.
	/// This method returns a [`RflagsBits`] value. See also [`rflags_modified()`].
	///
	/// [`RflagsBits`]: struct.RflagsBits.html
	/// [`rflags_modified()`]: #method.rflags_modified
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn rflags_cleared(&self) -> u32 {
		unsafe { *super::info::rflags_table::FLAGS_CLEARED.get_unchecked(self.rflags_info) as u32 }
	}

	/// All flags that are always set by the CPU.
	/// This method returns a [`RflagsBits`] value. See also [`rflags_modified()`].
	///
	/// [`RflagsBits`]: struct.RflagsBits.html
	/// [`rflags_modified()`]: #method.rflags_modified
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn rflags_set(&self) -> u32 {
		unsafe { *super::info::rflags_table::FLAGS_SET.get_unchecked(self.rflags_info) as u32 }
	}

	/// All flags that are undefined after executing the instruction.
	/// This method returns a [`RflagsBits`] value. See also [`rflags_modified()`].
	///
	/// [`RflagsBits`]: struct.RflagsBits.html
	/// [`rflags_modified()`]: #method.rflags_modified
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn rflags_undefined(&self) -> u32 {
		unsafe { *super::info::rflags_table::FLAGS_UNDEFINED.get_unchecked(self.rflags_info) as u32 }
	}

	/// All flags that are modified by the CPU. This is `rflags_written() + rflags_cleared() + rflags_set() + rflags_undefined()`. This method returns a [`RflagsBits`] value.
	///
	/// [`RflagsBits`]: struct.RflagsBits.html
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn rflags_modified(&self) -> u32 {
		unsafe { *super::info::rflags_table::FLAGS_MODIFIED.get_unchecked(self.rflags_info) as u32 }
	}
}
