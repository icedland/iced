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

use super::super::iced_constants::IcedConstants;
use super::enums::*;
use super::*;
use std::{mem, u16, u32, u64};

/// Instruction info options used by `InstructionInfoFactory`
#[allow(missing_copy_implementations)]
pub struct InstructionInfoOptions;
impl InstructionInfoOptions {
	/// No option is enabled
	pub const NONE: u32 = 0;
	/// Don't include memory usage, i.e., `InstructionInfo::used_memory()` will return an empty vector. All
	/// registers that are used by memory operands are still returned by `InstructionInfo::used_registers()`.
	pub const NO_MEMORY_USAGE: u32 = 0x0000_0001;
	/// Don't include register usage, i.e., `InstructionInfo::used_registers()` will return an empty vector
	pub const NO_REGISTER_USAGE: u32 = 0x0000_0002;
}

struct Flags;
impl Flags {
	pub const NONE: u32 = 0;
	pub const NO_MEMORY_USAGE: u32 = 0x0000_0001;
	pub const NO_REGISTER_USAGE: u32 = 0x0000_0002;
	pub const IS_64BIT: u32 = 0x0000_0004;
	pub const ZERO_EXT_VEC_REGS: u32 = 0x0000_0008;
}

/// Creates `InstructionInfo`s.
///
/// If you don't need to know register and memory usage, it's faster to call `Instruction` and
/// `Code` methods such as `Instruction::flow_control()` instead of getting that info from this struct.
pub struct InstructionInfoFactory {
	info: InstructionInfo,
}

#[cfg_attr(feature = "cargo-clippy", allow(clippy::new_without_default))]
impl InstructionInfoFactory {
	/// Creates a new instance.
	///
	/// If you don't need to know register and memory usage, it's faster to call `Instruction` and
	/// `Code` methods such as `Instruction::flow_control()` instead of getting that info from this method.
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	///
	/// // add [rdi+r12*8-5AA5EDCCh],esi
	/// let bytes = b"\x42\x01\xB4\xE7\x34\x12\x5A\xA5";
	/// let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
	///
	/// // This allocates two vectors but they get re-used every time you call info() and info_options().
	/// let mut info_factory = InstructionInfoFactory::new();
	///
	/// for instr in &mut decoder {
	///     // There's also info_options() if you only need reg usage or only mem usage.
	///     // info() returns both.
	///     let info = info_factory.info(&instr);
	///     for mem_info in info.used_memory().iter() {
	///         println!("{:?}", mem_info);
	///     }
	///     for reg_info in info.used_registers().iter() {
	///         println!("{:?}", reg_info);
	///     }
	/// }
	/// ```
	#[cfg_attr(has_must_use, must_use)]
	#[cfg_attr(feature = "cargo-clippy", allow(clippy::missing_inline_in_public_items))]
	pub fn new() -> Self {
		Self {
			info: InstructionInfo::new(0),
		}
	}

	/// Creates a new `InstructionInfo`, see also `info_options()` if you only need register usage
	/// but not memory usage or vice versa.
	///
	/// If you don't need to know register and memory usage, it's faster to call `Instruction` and
	/// `Code` methods such as `Instruction::flow_control()` instead of getting that info from this method.
	///
	/// # Arguments
	///
	/// * `instruction`: The instruction that should be analyzed
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	///
	/// // add [rdi+r12*8-5AA5EDCCh],esi
	/// let bytes = b"\x42\x01\xB4\xE7\x34\x12\x5A\xA5";
	/// let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
	/// let mut info_factory = InstructionInfoFactory::new();
	///
	/// let instr = decoder.decode();
	/// let info = info_factory.info(&instr);
	///
	/// assert_eq!(1, info.used_memory().len());
	/// let mem = info.used_memory()[0];
	/// assert_eq!(Register::DS, mem.segment());
	/// assert_eq!(Register::RDI, mem.base());
	/// assert_eq!(Register::R12, mem.index());
	/// assert_eq!(8, mem.scale());
	/// assert_eq!(0xFFFFFFFFA55A1234, mem.displacement());
	/// assert_eq!(MemorySize::UInt32, mem.memory_size());
	/// assert_eq!(OpAccess::ReadWrite, mem.access());
	///
	/// let regs = info.used_registers();
	/// assert_eq!(3, regs.len());
	/// assert_eq!(Register::RDI, regs[0].register());
	/// assert_eq!(OpAccess::Read, regs[0].access());
	/// assert_eq!(Register::R12, regs[1].register());
	/// assert_eq!(OpAccess::Read, regs[1].access());
	/// assert_eq!(Register::ESI, regs[2].register());
	/// assert_eq!(OpAccess::Read, regs[2].access());
	/// ```
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn info<'a, 'b>(&'a mut self, instruction: &'b Instruction) -> &'a InstructionInfo {
		Self::create(&mut self.info, instruction, InstructionInfoOptions::NONE)
	}

	/// Creates a new `InstructionInfo`, see also `info()`.
	///
	/// If you don't need to know register and memory usage, it's faster to call `Instruction` and
	/// `Code` methods such as `Instruction::flow_control()` instead of getting that info from this method.
	///
	/// # Arguments
	///
	/// * `instruction`: The instruction that should be analyzed
	/// * `options`: Options, see `InstructionInfoOptions`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn info_options<'a, 'b>(&'a mut self, instruction: &'b Instruction, options: u32) -> &'a InstructionInfo {
		Self::create(&mut self.info, instruction, options)
	}

	pub(crate) fn create<'a>(info: &'a mut InstructionInfo, instruction: &Instruction, options: u32) -> &'a InstructionInfo {
		info.used_registers.clear();
		info.used_memory_locations.clear();

		let index = (instruction.code() as usize) << 1;
		let flags1 = unsafe { *super::info_table::TABLE.as_ptr().offset(index as isize) };
		let mut flags2 = unsafe { *super::info_table::TABLE.as_ptr().offset((index + 1) as isize) };

		if (flags2 & InfoFlags2::AVX2_CHECK) != 0 && instruction.op1_kind() == OpKind::Register {
			flags2 = (flags2 & !(InfoFlags2::CPUID_FEATURE_INTERNAL_MASK << InfoFlags2::CPUID_FEATURE_INTERNAL_SHIFT))
				| ((CpuidFeatureInternal::AVX2 as u32) << InfoFlags2::CPUID_FEATURE_INTERNAL_SHIFT);
		}

		info.cpuid_feature_internal = ((flags2 >> InfoFlags2::CPUID_FEATURE_INTERNAL_SHIFT) & InfoFlags2::CPUID_FEATURE_INTERNAL_MASK) as usize;
		info.flow_control = unsafe { mem::transmute(((flags2 >> InfoFlags2::FLOW_CONTROL_SHIFT) & InfoFlags2::FLOW_CONTROL_MASK) as u8) };
		info.encoding = unsafe { mem::transmute(((flags2 >> InfoFlags2::ENCODING_SHIFT) & InfoFlags2::ENCODING_MASK) as u8) };
		info.rflags_info = ((flags1 >> InfoFlags1::RFLAGS_INFO_SHIFT) & InfoFlags1::RFLAGS_INFO_MASK) as usize;

		const_assert_eq!(0x0800_0000, InfoFlags1::SAVE_RESTORE);
		const_assert_eq!(0x1000_0000, InfoFlags1::STACK_INSTRUCTION);
		const_assert_eq!(0x2000_0000, InfoFlags1::PROTECTED_MODE);
		const_assert_eq!(0x4000_0000, InfoFlags1::PRIVILEGED);
		const_assert_eq!(0x01, IIFlags::SAVE_RESTORE);
		const_assert_eq!(0x02, IIFlags::STACK_INSTRUCTION);
		const_assert_eq!(0x04, IIFlags::PROTECTED_MODE);
		const_assert_eq!(0x08, IIFlags::PRIVILEGED);
		// Bit 4 could be set but we don't use it so we don't need to mask it out
		info.flags = (flags1 >> 27) as u8;

		let code_size = instruction.code_size();
		const_assert_eq!(Flags::NO_MEMORY_USAGE, InstructionInfoOptions::NO_MEMORY_USAGE);
		const_assert_eq!(Flags::NO_REGISTER_USAGE, InstructionInfoOptions::NO_REGISTER_USAGE);
		let mut flags = options & (Flags::NO_MEMORY_USAGE | Flags::NO_REGISTER_USAGE);
		if code_size == CodeSize::Code64 || code_size == CodeSize::Unknown {
			flags |= Flags::IS_64BIT;
		}
		if info.encoding != EncodingKind::Legacy {
			flags |= Flags::ZERO_EXT_VEC_REGS;
		}

		let op0_access = match unsafe { mem::transmute(((flags1 >> InfoFlags1::OP_INFO0_SHIFT) & InfoFlags1::OP_INFO0_MASK) as u8) } {
			OpInfo0::None => OpAccess::None,
			OpInfo0::Read => OpAccess::Read,
			OpInfo0::Write => {
				if instruction.has_op_mask() && instruction.merging_masking() {
					OpAccess::ReadWrite
				} else {
					OpAccess::Write
				}
			}

			OpInfo0::WriteForce => OpAccess::Write,
			OpInfo0::CondWrite => OpAccess::CondWrite,

			OpInfo0::CondWrite32_ReadWrite64 => {
				if (flags & Flags::IS_64BIT) != 0 {
					OpAccess::ReadWrite
				} else {
					OpAccess::CondWrite
				}
			}

			OpInfo0::ReadWrite => OpAccess::ReadWrite,
			OpInfo0::ReadCondWrite => OpAccess::ReadCondWrite,
			OpInfo0::NoMemAccess => OpAccess::NoMemAccess,

			OpInfo0::WriteMem_ReadWriteReg => {
				if super::super::instruction_internal::internal_op0_is_not_reg_or_op1_is_not_reg(instruction) {
					OpAccess::Write
				} else {
					OpAccess::ReadWrite
				}
			}
		};

		debug_assert!(instruction.op_count() <= IcedConstants::MAX_OP_COUNT);
		info.op_accesses[0] = op0_access;
		let op1_info = ((flags1 >> InfoFlags1::OP_INFO1_SHIFT) & InfoFlags1::OP_INFO1_MASK) as usize;
		info.op_accesses[1] = unsafe { *OP_ACCESS_1.as_ptr().offset(op1_info as isize) };
		info.op_accesses[2] = unsafe {
			*OP_ACCESS_2
				.as_ptr()
				.offset(((flags1 >> InfoFlags1::OP_INFO2_SHIFT) & InfoFlags1::OP_INFO2_MASK) as isize)
		};
		info.op_accesses[3] = if (flags1 & ((InfoFlags1::OP_INFO3_MASK) << InfoFlags1::OP_INFO3_SHIFT)) != 0 {
			const_assert_eq!(2, InstrInfoConstants::OP_INFO3_COUNT);
			OpAccess::Read
		} else {
			OpAccess::None
		};
		info.op_accesses[4] = if (flags1 & ((InfoFlags1::OP_INFO4_MASK) << InfoFlags1::OP_INFO4_SHIFT)) != 0 {
			const_assert_eq!(2, InstrInfoConstants::OP_INFO4_COUNT);
			OpAccess::Read
		} else {
			OpAccess::None
		};
		const_assert_eq!(5, IcedConstants::MAX_OP_COUNT);

		for i in 0..(instruction.op_count() as usize) {
			let mut access = unsafe { *info.op_accesses.as_ptr().offset(i as isize) };
			if access == OpAccess::None {
				continue;
			}

			match instruction.op_kind(i as u32) {
				OpKind::Register => {
					if access == OpAccess::NoMemAccess {
						access = OpAccess::Read;
						unsafe { *info.op_accesses.as_mut_ptr().offset(i as isize) = OpAccess::Read };
					}
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						if i == 1 && op1_info == OpInfo1::ReadP3 as usize {
							let mut reg = instruction.op1_register();
							debug_assert!(Register::XMM0 <= reg && reg <= IcedConstants::VMM_LAST);
							reg = unsafe {
								mem::transmute(
									(IcedConstants::VMM_FIRST as u32).wrapping_add((reg as u32).wrapping_sub(IcedConstants::VMM_FIRST as u32) & !3)
										as u8,
								)
							};
							for j in 0..4 {
								Self::add_register(flags, info, unsafe { mem::transmute((reg as u32).wrapping_add(j) as u8) }, access);
							}
						} else {
							Self::add_register(flags, info, instruction.op_register(i as u32), access);
						}
					}
				}

				OpKind::Memory64 => {
					if (flags & Flags::NO_MEMORY_USAGE) == 0 {
						Self::add_memory(
							info,
							instruction.memory_segment(),
							Register::None,
							Register::None,
							1,
							instruction.memory_address64(),
							instruction.memory_size(),
							access,
						);
					}
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						Self::add_memory_segment_register(flags, info, instruction.memory_segment(), OpAccess::Read);
					}
				}

				OpKind::Memory => {
					const_assert_eq!(1 << 31, InfoFlags1::NO_SEGMENT_READ);
					const_assert_eq!(0, Register::None as u32);
					let segment_register = unsafe { mem::transmute((instruction.memory_segment() as u32 & !((flags1 as i32 >> 31) as u32)) as u8) };
					let base_register = instruction.memory_base();
					if base_register == Register::RIP {
						if (flags & Flags::NO_MEMORY_USAGE) == 0 {
							Self::add_memory(
								info,
								segment_register,
								Register::None,
								Register::None,
								1,
								instruction.next_ip().wrapping_add(instruction.memory_displacement64()),
								instruction.memory_size(),
								access,
							);
						}
						if (flags & Flags::NO_REGISTER_USAGE) == 0 && segment_register != Register::None {
							Self::add_memory_segment_register(flags, info, segment_register, OpAccess::Read);
						}
					} else if base_register == Register::EIP {
						if (flags & Flags::NO_MEMORY_USAGE) == 0 {
							Self::add_memory(
								info,
								segment_register,
								Register::None,
								Register::None,
								1,
								instruction.next_ip32().wrapping_add(instruction.memory_displacement()) as u64,
								instruction.memory_size(),
								access,
							);
						}
						if (flags & Flags::NO_REGISTER_USAGE) == 0 && segment_register != Register::None {
							Self::add_memory_segment_register(flags, info, segment_register, OpAccess::Read);
						}
					} else {
						let index_register = instruction.memory_index();
						if (flags & Flags::NO_MEMORY_USAGE) == 0 {
							let displ;
							if super::super::instruction_internal::get_address_size_in_bytes(
								base_register,
								index_register,
								instruction.memory_displ_size(),
								code_size,
							) == 8
							{
								displ = instruction.memory_displacement64();
							} else {
								displ = instruction.memory_displacement() as u64;
							}
							Self::add_memory(
								info,
								segment_register,
								base_register,
								index_register,
								instruction.memory_index_scale(),
								displ,
								instruction.memory_size(),
								access,
							);
						}
						if (flags & Flags::NO_REGISTER_USAGE) == 0 {
							if segment_register != Register::None {
								Self::add_memory_segment_register(flags, info, segment_register, OpAccess::Read);
							}
							if base_register != Register::None {
								Self::add_register(flags, info, base_register, OpAccess::Read);
							}
							if index_register != Register::None {
								Self::add_register(flags, info, index_register, OpAccess::Read);
							}
						}
					}
				}
				_ => {}
			}
		}

		let code_info = unsafe { mem::transmute(((flags1 >> InfoFlags1::CODE_INFO_SHIFT) & InfoFlags1::CODE_INFO_MASK) as u8) };
		if code_info != CodeInfo::None {
			Self::code_info_handler(code_info, instruction, info, flags);
		}

		if instruction.has_op_mask() && (flags & Flags::NO_REGISTER_USAGE) == 0 {
			Self::add_register(
				flags,
				info,
				instruction.op_mask(),
				if (flags2 & InfoFlags2::OP_MASK_REG_READ_WRITE) != 0 {
					OpAccess::ReadWrite
				} else {
					OpAccess::Read
				},
			);
		}
		info
	}

	fn get_xsp(code_size: CodeSize, xsp_mask: &mut u64) -> Register {
		match code_size {
			CodeSize::Code64 | CodeSize::Unknown => {
				*xsp_mask = u64::MAX;
				Register::RSP
			}
			CodeSize::Code32 => {
				*xsp_mask = u32::MAX as u64;
				Register::ESP
			}
			CodeSize::Code16 => {
				*xsp_mask = u16::MAX as u64;
				Register::SP
			}
		}
	}

	#[cfg_attr(feature = "cargo-clippy", allow(clippy::erasing_op))]
	#[cfg_attr(feature = "cargo-clippy", allow(clippy::identity_op))]
	#[cfg_attr(feature = "cargo-clippy", allow(clippy::len_zero))]
	fn code_info_handler(code_info: CodeInfo, instruction: &Instruction, info: &mut InstructionInfo, flags: u32) {
		debug_assert_ne!(CodeInfo::None, code_info);
		let mut index;
		let reg_index;
		let mut xsp_mask = 0;
		let mut displ;
		let xsp;
		let mut base_register;
		let memory_size;
		let code;
		match code_info {
			CodeInfo::RW_AX => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::AX, OpAccess::ReadWrite);
				}
			}

			CodeInfo::RW_AL => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::AL, OpAccess::ReadWrite);
				}
			}

			CodeInfo::Salc => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::AL, OpAccess::Write);
				}
			}

			CodeInfo::R_AL_W_AH => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::AL, OpAccess::Read);
					Self::add_register(flags, info, Register::AH, OpAccess::Write);
				}
			}

			CodeInfo::R_AL_W_AX => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::AL, OpAccess::Read);
					Self::add_register(flags, info, Register::AX, OpAccess::Write);
				}
			}

			CodeInfo::Cwde => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::AX, OpAccess::Read);
					Self::add_register(flags, info, Register::EAX, OpAccess::Write);
				}
			}

			CodeInfo::Cdqe => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::EAX, OpAccess::Read);
					Self::add_register(flags, info, Register::RAX, OpAccess::Write);
				}
			}

			CodeInfo::Cwd => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::AX, OpAccess::Read);
					Self::add_register(flags, info, Register::DX, OpAccess::Write);
				}
			}

			CodeInfo::Cdq => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::EAX, OpAccess::Read);
					Self::add_register(flags, info, Register::EDX, OpAccess::Write);
				}
			}

			CodeInfo::Cqo => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::RAX, OpAccess::Read);
					Self::add_register(flags, info, Register::RDX, OpAccess::Write);
				}
			}

			CodeInfo::R_XMM0 => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::XMM0, OpAccess::Read);
				}
			}

			CodeInfo::Push_2 => {
				xsp = Self::get_xsp(instruction.code_size(), &mut xsp_mask);
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					if (flags & Flags::IS_64BIT) == 0 {
						Self::add_register(flags, info, Register::SS, OpAccess::Read);
					}
					Self::add_register(flags, info, xsp, OpAccess::ReadWrite);
				}
				if (flags & Flags::NO_MEMORY_USAGE) == 0 {
					Self::add_memory(
						info,
						Register::SS,
						xsp,
						Register::None,
						1,
						0xFFFF_FFFF_FFFF_FFFE & xsp_mask,
						MemorySize::UInt16,
						OpAccess::Write,
					);
				}
			}

			CodeInfo::Push_4 => {
				xsp = Self::get_xsp(instruction.code_size(), &mut xsp_mask);
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					if (flags & Flags::IS_64BIT) == 0 {
						Self::add_register(flags, info, Register::SS, OpAccess::Read);
					}
					Self::add_register(flags, info, xsp, OpAccess::ReadWrite);
				}
				if (flags & Flags::NO_MEMORY_USAGE) == 0 {
					Self::add_memory(
						info,
						Register::SS,
						xsp,
						Register::None,
						1,
						0xFFFF_FFFF_FFFF_FFFC & xsp_mask,
						MemorySize::UInt32,
						OpAccess::Write,
					);
				}
			}

			CodeInfo::Push_8 => {
				xsp = Self::get_xsp(instruction.code_size(), &mut xsp_mask);
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, xsp, OpAccess::ReadWrite);
				}
				if (flags & Flags::NO_MEMORY_USAGE) == 0 {
					Self::add_memory(
						info,
						Register::SS,
						xsp,
						Register::None,
						1,
						0xFFFF_FFFF_FFFF_FFF8 & xsp_mask,
						MemorySize::UInt64,
						OpAccess::Write,
					);
				}
			}

			CodeInfo::Push_2_2 => {
				xsp = Self::get_xsp(instruction.code_size(), &mut xsp_mask);
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					if (flags & Flags::IS_64BIT) == 0 {
						Self::add_register(flags, info, Register::SS, OpAccess::Read);
					}
					Self::add_register(flags, info, xsp, OpAccess::ReadWrite);
				}
				if (flags & Flags::NO_MEMORY_USAGE) == 0 {
					Self::add_memory(
						info,
						Register::SS,
						xsp,
						Register::None,
						1,
						0xFFFF_FFFF_FFFF_FFFE & xsp_mask,
						MemorySize::UInt16,
						OpAccess::Write,
					);
					Self::add_memory(
						info,
						Register::SS,
						xsp,
						Register::None,
						1,
						0xFFFF_FFFF_FFFF_FFFC & xsp_mask,
						MemorySize::UInt16,
						OpAccess::Write,
					);
				}
			}

			CodeInfo::Push_4_4 => {
				xsp = Self::get_xsp(instruction.code_size(), &mut xsp_mask);
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					if (flags & Flags::IS_64BIT) == 0 {
						Self::add_register(flags, info, Register::SS, OpAccess::Read);
					}
					Self::add_register(flags, info, xsp, OpAccess::ReadWrite);
				}
				if (flags & Flags::NO_MEMORY_USAGE) == 0 {
					Self::add_memory(
						info,
						Register::SS,
						xsp,
						Register::None,
						1,
						0xFFFF_FFFF_FFFF_FFFC & xsp_mask,
						MemorySize::UInt32,
						OpAccess::Write,
					);
					Self::add_memory(
						info,
						Register::SS,
						xsp,
						Register::None,
						1,
						0xFFFF_FFFF_FFFF_FFF8 & xsp_mask,
						MemorySize::UInt32,
						OpAccess::Write,
					);
				}
			}

			CodeInfo::Push_8_8 => {
				xsp = Self::get_xsp(instruction.code_size(), &mut xsp_mask);
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, xsp, OpAccess::ReadWrite);
				}
				if (flags & Flags::NO_MEMORY_USAGE) == 0 {
					Self::add_memory(
						info,
						Register::SS,
						xsp,
						Register::None,
						1,
						0xFFFF_FFFF_FFFF_FFF8 & xsp_mask,
						MemorySize::UInt64,
						OpAccess::Write,
					);
					Self::add_memory(
						info,
						Register::SS,
						xsp,
						Register::None,
						1,
						0xFFFF_FFFF_FFFF_FFF0 & xsp_mask,
						MemorySize::UInt64,
						OpAccess::Write,
					);
				}
			}

			CodeInfo::Pop_2 => {
				xsp = Self::get_xsp(instruction.code_size(), &mut xsp_mask);
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					if (flags & Flags::IS_64BIT) == 0 {
						Self::add_register(flags, info, Register::SS, OpAccess::Read);
					}
					Self::add_register(flags, info, xsp, OpAccess::ReadWrite);
				}
				if (flags & Flags::NO_MEMORY_USAGE) == 0 {
					Self::add_memory(info, Register::SS, xsp, Register::None, 1, 0, MemorySize::UInt16, OpAccess::Read);
				}
			}

			CodeInfo::Pop_4 => {
				xsp = Self::get_xsp(instruction.code_size(), &mut xsp_mask);
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					if (flags & Flags::IS_64BIT) == 0 {
						Self::add_register(flags, info, Register::SS, OpAccess::Read);
					}
					Self::add_register(flags, info, xsp, OpAccess::ReadWrite);
				}
				if (flags & Flags::NO_MEMORY_USAGE) == 0 {
					Self::add_memory(info, Register::SS, xsp, Register::None, 1, 0, MemorySize::UInt32, OpAccess::Read);
				}
			}

			CodeInfo::Pop_8 => {
				xsp = Self::get_xsp(instruction.code_size(), &mut xsp_mask);
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, xsp, OpAccess::ReadWrite);
				}
				if (flags & Flags::NO_MEMORY_USAGE) == 0 {
					Self::add_memory(info, Register::SS, xsp, Register::None, 1, 0, MemorySize::UInt64, OpAccess::Read);
				}
			}

			CodeInfo::Pop_2_2 => {
				xsp = Self::get_xsp(instruction.code_size(), &mut xsp_mask);
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					if (flags & Flags::IS_64BIT) == 0 {
						Self::add_register(flags, info, Register::SS, OpAccess::Read);
					}
					Self::add_register(flags, info, xsp, OpAccess::ReadWrite);
				}
				if (flags & Flags::NO_MEMORY_USAGE) == 0 {
					Self::add_memory(info, Register::SS, xsp, Register::None, 1, 0, MemorySize::UInt16, OpAccess::Read);
					Self::add_memory(info, Register::SS, xsp, Register::None, 1, 2, MemorySize::UInt16, OpAccess::Read);
				}
			}

			CodeInfo::Pop_4_4 => {
				xsp = Self::get_xsp(instruction.code_size(), &mut xsp_mask);
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					if (flags & Flags::IS_64BIT) == 0 {
						Self::add_register(flags, info, Register::SS, OpAccess::Read);
					}
					Self::add_register(flags, info, xsp, OpAccess::ReadWrite);
				}
				if (flags & Flags::NO_MEMORY_USAGE) == 0 {
					Self::add_memory(info, Register::SS, xsp, Register::None, 1, 0, MemorySize::UInt32, OpAccess::Read);
					Self::add_memory(info, Register::SS, xsp, Register::None, 1, 4, MemorySize::UInt32, OpAccess::Read);
				}
			}

			CodeInfo::Pop_8_8 => {
				xsp = Self::get_xsp(instruction.code_size(), &mut xsp_mask);
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, xsp, OpAccess::ReadWrite);
				}
				if (flags & Flags::NO_MEMORY_USAGE) == 0 {
					Self::add_memory(info, Register::SS, xsp, Register::None, 1, 0, MemorySize::UInt64, OpAccess::Read);
					Self::add_memory(info, Register::SS, xsp, Register::None, 1, 8, MemorySize::UInt64, OpAccess::Read);
				}
			}

			CodeInfo::Pop_Ev => {
				xsp = Self::get_xsp(instruction.code_size(), &mut xsp_mask);
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					if (flags & Flags::IS_64BIT) == 0 {
						Self::add_register(flags, info, Register::SS, OpAccess::Read);
					}
					Self::add_register(flags, info, xsp, OpAccess::ReadWrite);
				}
				if (flags & Flags::NO_MEMORY_USAGE) == 0 {
					code = instruction.code();
					let size = if code == Code::Pop_rm64 {
						memory_size = MemorySize::UInt64;
						8
					} else if code == Code::Pop_rm32 {
						memory_size = MemorySize::UInt32;
						4
					} else {
						debug_assert_eq!(Code::Pop_rm16, instruction.code());
						memory_size = MemorySize::UInt16;
						2
					};
					if instruction.op0_kind() == OpKind::Memory {
						debug_assert_eq!(1, info.used_memory_locations.len());
						if instruction.memory_base() == Register::RSP || instruction.memory_base() == Register::ESP {
							let mem = info.used_memory_locations[0];
							displ = mem.displacement().wrapping_add(size);
							if instruction.memory_base() == Register::ESP {
								displ = displ as u32 as u64;
							}
							info.used_memory_locations[0] = UsedMemory { displacement: displ, ..mem };
						}
					}
					Self::add_memory(info, Register::SS, xsp, Register::None, 1, 0, memory_size, OpAccess::Read);
				}
			}

			CodeInfo::Pusha => {
				xsp = Self::get_xsp(instruction.code_size(), &mut xsp_mask);
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					if (flags & Flags::IS_64BIT) == 0 {
						Self::add_register(flags, info, Register::SS, OpAccess::Read);
					}
					Self::add_register(flags, info, xsp, OpAccess::ReadWrite);
				}
				base_register = if instruction.code() == Code::Pushad {
					displ = 0xFFFF_FFFF_FFFF_FFFC;
					memory_size = MemorySize::UInt32;
					Register::EAX
				} else {
					debug_assert_eq!(Code::Pushaw, instruction.code());
					displ = 0xFFFF_FFFF_FFFF_FFFE;
					memory_size = MemorySize::UInt16;
					Register::AX
				};
				for i in 0..8 {
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						Self::add_register(
							flags,
							info,
							unsafe { mem::transmute((base_register as u32).wrapping_add(i) as u8) },
							OpAccess::Read,
						);
					}
					if (flags & Flags::NO_MEMORY_USAGE) == 0 {
						Self::add_memory(
							info,
							Register::SS,
							xsp,
							Register::None,
							1,
							(displ as i64).wrapping_mul((i + 1) as i64) as u64 & xsp_mask,
							memory_size,
							OpAccess::Write,
						);
					}
				}
			}

			CodeInfo::Popa => {
				xsp = Self::get_xsp(instruction.code_size(), &mut xsp_mask);
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					if (flags & Flags::IS_64BIT) == 0 {
						Self::add_register(flags, info, Register::SS, OpAccess::Read);
					}
					Self::add_register(flags, info, xsp, OpAccess::ReadWrite);
				}
				base_register = if instruction.code() == Code::Popad {
					displ = 4;
					memory_size = MemorySize::UInt32;
					Register::EAX
				} else {
					debug_assert_eq!(Code::Popaw, instruction.code());
					displ = 2;
					memory_size = MemorySize::UInt16;
					Register::AX
				};
				for i in 0..8 {
					// Ignore eSP
					if i != 3 {
						if (flags & Flags::NO_REGISTER_USAGE) == 0 {
							Self::add_register(
								flags,
								info,
								unsafe { mem::transmute((base_register as u32).wrapping_add(7).wrapping_sub(i) as u8) },
								OpAccess::Write,
							);
						}
						if (flags & Flags::NO_MEMORY_USAGE) == 0 {
							Self::add_memory(
								info,
								Register::SS,
								xsp,
								Register::None,
								1,
								displ.wrapping_mul(i as u64) & xsp_mask,
								memory_size,
								OpAccess::Read,
							);
						}
					}
				}
			}

			CodeInfo::Ins => {
				if super::super::instruction_internal::internal_has_repe_or_repne_prefix(instruction) {
					info.op_accesses[0] = OpAccess::CondWrite;
					info.op_accesses[1] = OpAccess::CondRead;
					const_assert_eq!(OpKind::MemoryESEDI as u32, OpKind::MemoryESDI as u32 + 1);
					const_assert_eq!(OpKind::MemoryESRDI as u32, OpKind::MemoryESDI as u32 + 2);
					const_assert_eq!(Register::EDI as u32, Register::DI as u32 + 16);
					const_assert_eq!(Register::RDI as u32, Register::DI as u32 + 32);
					const_assert_eq!(Register::ECX as u32, Register::CX as u32 + 16);
					const_assert_eq!(Register::RCX as u32, Register::CX as u32 + 32);
					base_register = unsafe {
						mem::transmute(
							((instruction.op0_kind() as u32).wrapping_sub(OpKind::MemoryESDI as u32) << 4).wrapping_add(Register::DI as u32) as u8,
						)
					};
					if (flags & Flags::NO_MEMORY_USAGE) == 0 {
						Self::add_memory(
							info,
							Register::ES,
							base_register,
							Register::None,
							1,
							0,
							MemorySize::Unknown,
							OpAccess::CondWrite,
						);
					}
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						debug_assert_eq!(1, info.used_registers.len());
						info.used_registers[0] = UsedRegister {
							register: Register::DX,
							access: OpAccess::CondRead,
						};
						Self::add_register(
							flags,
							info,
							unsafe {
								mem::transmute(
									((instruction.op0_kind() as u32).wrapping_sub(OpKind::MemoryESDI as u32) << 4).wrapping_add(Register::CX as u32)
										as u8,
								)
							},
							OpAccess::ReadCondWrite,
						);
						if (flags & Flags::IS_64BIT) == 0 {
							Self::add_register(flags, info, Register::ES, OpAccess::CondRead);
						}
						Self::add_register(flags, info, base_register, OpAccess::CondRead);
						Self::add_register(flags, info, base_register, OpAccess::CondWrite);
					}
				} else {
					const_assert_eq!(OpKind::MemoryESEDI as u32, OpKind::MemoryESDI as u32 + 1);
					const_assert_eq!(OpKind::MemoryESRDI as u32, OpKind::MemoryESDI as u32 + 2);
					const_assert_eq!(Register::EDI as u32, Register::DI as u32 + 16);
					const_assert_eq!(Register::RDI as u32, Register::DI as u32 + 32);
					base_register = unsafe {
						mem::transmute(
							((instruction.op0_kind() as u32).wrapping_sub(OpKind::MemoryESDI as u32) << 4).wrapping_add(Register::DI as u32) as u8,
						)
					};
					if (flags & Flags::NO_MEMORY_USAGE) == 0 {
						Self::add_memory(
							info,
							Register::ES,
							base_register,
							Register::None,
							1,
							0,
							instruction.memory_size(),
							OpAccess::Write,
						);
					}
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						if (flags & Flags::IS_64BIT) == 0 {
							Self::add_register(flags, info, Register::ES, OpAccess::Read);
						}
						Self::add_register(flags, info, base_register, OpAccess::ReadWrite);
					}
				}
			}

			CodeInfo::Outs => {
				if super::super::instruction_internal::internal_has_repe_or_repne_prefix(instruction) {
					info.op_accesses[0] = OpAccess::CondRead;
					info.op_accesses[1] = OpAccess::CondRead;
					const_assert_eq!(OpKind::MemorySegESI as u32, OpKind::MemorySegSI as u32 + 1);
					const_assert_eq!(OpKind::MemorySegRSI as u32, OpKind::MemorySegSI as u32 + 2);
					const_assert_eq!(Register::ESI as u32, Register::SI as u32 + 16);
					const_assert_eq!(Register::RSI as u32, Register::SI as u32 + 32);
					const_assert_eq!(Register::ECX as u32, Register::CX as u32 + 16);
					const_assert_eq!(Register::RCX as u32, Register::CX as u32 + 32);
					base_register = unsafe {
						mem::transmute(
							((instruction.op1_kind() as u32).wrapping_sub(OpKind::MemorySegSI as u32) << 4).wrapping_add(Register::SI as u32) as u8,
						)
					};
					if (flags & Flags::NO_MEMORY_USAGE) == 0 {
						Self::add_memory(
							info,
							instruction.memory_segment(),
							base_register,
							Register::None,
							1,
							0,
							MemorySize::Unknown,
							OpAccess::CondRead,
						);
					}
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						debug_assert_eq!(1, info.used_registers.len());
						info.used_registers[0] = UsedRegister {
							register: Register::DX,
							access: OpAccess::CondRead,
						};
						Self::add_register(
							flags,
							info,
							unsafe {
								mem::transmute(
									((instruction.op1_kind() as u32).wrapping_sub(OpKind::MemorySegSI as u32) << 4).wrapping_add(Register::CX as u32)
										as u8,
								)
							},
							OpAccess::ReadCondWrite,
						);
						Self::add_memory_segment_register(flags, info, instruction.memory_segment(), OpAccess::CondRead);
						Self::add_register(flags, info, base_register, OpAccess::CondRead);
						Self::add_register(flags, info, base_register, OpAccess::CondWrite);
					}
				} else {
					const_assert_eq!(OpKind::MemorySegESI as u32, OpKind::MemorySegSI as u32 + 1);
					const_assert_eq!(OpKind::MemorySegRSI as u32, OpKind::MemorySegSI as u32 + 2);
					const_assert_eq!(Register::ESI as u32, Register::SI as u32 + 16);
					const_assert_eq!(Register::RSI as u32, Register::SI as u32 + 32);
					base_register = unsafe {
						mem::transmute(
							((instruction.op1_kind() as u32).wrapping_sub(OpKind::MemorySegSI as u32) << 4).wrapping_add(Register::SI as u32) as u8,
						)
					};
					if (flags & Flags::NO_MEMORY_USAGE) == 0 {
						Self::add_memory(
							info,
							instruction.memory_segment(),
							base_register,
							Register::None,
							1,
							0,
							instruction.memory_size(),
							OpAccess::Read,
						);
					}
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						Self::add_memory_segment_register(flags, info, instruction.memory_segment(), OpAccess::Read);
						Self::add_register(flags, info, base_register, OpAccess::ReadWrite);
					}
				}
			}

			CodeInfo::Movs => {
				if super::super::instruction_internal::internal_has_repe_or_repne_prefix(instruction) {
					info.op_accesses[0] = OpAccess::CondWrite;
					info.op_accesses[1] = OpAccess::CondRead;
					const_assert_eq!(OpKind::MemoryESEDI as u32, OpKind::MemoryESDI as u32 + 1);
					const_assert_eq!(OpKind::MemoryESRDI as u32, OpKind::MemoryESDI as u32 + 2);
					const_assert_eq!(Register::EDI as u32, Register::DI as u32 + 16);
					const_assert_eq!(Register::RDI as u32, Register::DI as u32 + 32);
					const_assert_eq!(Register::ECX as u32, Register::CX as u32 + 16);
					const_assert_eq!(Register::RCX as u32, Register::CX as u32 + 32);
					base_register = unsafe {
						mem::transmute(
							((instruction.op0_kind() as u32).wrapping_sub(OpKind::MemoryESDI as u32) << 4).wrapping_add(Register::DI as u32) as u8,
						)
					};
					if (flags & Flags::NO_MEMORY_USAGE) == 0 {
						Self::add_memory(
							info,
							Register::ES,
							base_register,
							Register::None,
							1,
							0,
							MemorySize::Unknown,
							OpAccess::CondWrite,
						);
					}
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						Self::add_register(
							flags,
							info,
							unsafe {
								mem::transmute(
									((instruction.op0_kind() as u32).wrapping_sub(OpKind::MemoryESDI as u32) << 4).wrapping_add(Register::CX as u32)
										as u8,
								)
							},
							OpAccess::ReadCondWrite,
						);
						if (flags & Flags::IS_64BIT) == 0 {
							Self::add_register(flags, info, Register::ES, OpAccess::CondRead);
						}
						Self::add_register(flags, info, base_register, OpAccess::CondRead);
						Self::add_register(flags, info, base_register, OpAccess::CondWrite);
					}
					const_assert_eq!(OpKind::MemorySegESI as u32, OpKind::MemorySegSI as u32 + 1);
					const_assert_eq!(OpKind::MemorySegRSI as u32, OpKind::MemorySegSI as u32 + 2);
					const_assert_eq!(Register::ESI as u32, Register::SI as u32 + 16);
					const_assert_eq!(Register::RSI as u32, Register::SI as u32 + 32);
					base_register = unsafe {
						mem::transmute(
							((instruction.op1_kind() as u32).wrapping_sub(OpKind::MemorySegSI as u32) << 4).wrapping_add(Register::SI as u32) as u8,
						)
					};
					if (flags & Flags::NO_MEMORY_USAGE) == 0 {
						Self::add_memory(
							info,
							instruction.memory_segment(),
							base_register,
							Register::None,
							1,
							0,
							MemorySize::Unknown,
							OpAccess::CondRead,
						);
					}
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						Self::add_memory_segment_register(flags, info, instruction.memory_segment(), OpAccess::CondRead);
						Self::add_register(flags, info, base_register, OpAccess::CondRead);
						Self::add_register(flags, info, base_register, OpAccess::CondWrite);
					}
				} else {
					const_assert_eq!(OpKind::MemoryESEDI as u32, OpKind::MemoryESDI as u32 + 1);
					const_assert_eq!(OpKind::MemoryESRDI as u32, OpKind::MemoryESDI as u32 + 2);
					const_assert_eq!(Register::EDI as u32, Register::DI as u32 + 16);
					const_assert_eq!(Register::RDI as u32, Register::DI as u32 + 32);
					base_register = unsafe {
						mem::transmute(
							((instruction.op0_kind() as u32).wrapping_sub(OpKind::MemoryESDI as u32) << 4).wrapping_add(Register::DI as u32) as u8,
						)
					};
					if (flags & Flags::NO_MEMORY_USAGE) == 0 {
						Self::add_memory(
							info,
							Register::ES,
							base_register,
							Register::None,
							1,
							0,
							instruction.memory_size(),
							OpAccess::Write,
						);
					}
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						if (flags & Flags::IS_64BIT) == 0 {
							Self::add_register(flags, info, Register::ES, OpAccess::Read);
						}
						Self::add_register(flags, info, base_register, OpAccess::ReadWrite);
					}
					const_assert_eq!(OpKind::MemorySegESI as u32, OpKind::MemorySegSI as u32 + 1);
					const_assert_eq!(OpKind::MemorySegRSI as u32, OpKind::MemorySegSI as u32 + 2);
					const_assert_eq!(Register::ESI as u32, Register::SI as u32 + 16);
					const_assert_eq!(Register::RSI as u32, Register::SI as u32 + 32);
					base_register = unsafe {
						mem::transmute(
							((instruction.op1_kind() as u32).wrapping_sub(OpKind::MemorySegSI as u32) << 4).wrapping_add(Register::SI as u32) as u8,
						)
					};
					if (flags & Flags::NO_MEMORY_USAGE) == 0 {
						Self::add_memory(
							info,
							instruction.memory_segment(),
							base_register,
							Register::None,
							1,
							0,
							instruction.memory_size(),
							OpAccess::Read,
						);
					}
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						Self::add_memory_segment_register(flags, info, instruction.memory_segment(), OpAccess::Read);
						Self::add_register(flags, info, base_register, OpAccess::ReadWrite);
					}
				}
			}

			CodeInfo::Cmps => {
				if super::super::instruction_internal::internal_has_repe_or_repne_prefix(instruction) {
					info.op_accesses[0] = OpAccess::CondRead;
					info.op_accesses[1] = OpAccess::CondRead;
					const_assert_eq!(OpKind::MemorySegESI as u32, OpKind::MemorySegSI as u32 + 1);
					const_assert_eq!(OpKind::MemorySegRSI as u32, OpKind::MemorySegSI as u32 + 2);
					const_assert_eq!(Register::ESI as u32, Register::SI as u32 + 16);
					const_assert_eq!(Register::RSI as u32, Register::SI as u32 + 32);
					const_assert_eq!(Register::ECX as u32, Register::CX as u32 + 16);
					const_assert_eq!(Register::RCX as u32, Register::CX as u32 + 32);
					base_register = unsafe {
						mem::transmute(
							((instruction.op0_kind() as u32).wrapping_sub(OpKind::MemorySegSI as u32) << 4).wrapping_add(Register::SI as u32) as u8,
						)
					};
					if (flags & Flags::NO_MEMORY_USAGE) == 0 {
						Self::add_memory(
							info,
							instruction.memory_segment(),
							base_register,
							Register::None,
							1,
							0,
							MemorySize::Unknown,
							OpAccess::CondRead,
						);
					}
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						Self::add_register(
							flags,
							info,
							unsafe {
								mem::transmute(
									((instruction.op0_kind() as u32).wrapping_sub(OpKind::MemorySegSI as u32) << 4).wrapping_add(Register::CX as u32)
										as u8,
								)
							},
							OpAccess::ReadCondWrite,
						);
						Self::add_memory_segment_register(flags, info, instruction.memory_segment(), OpAccess::CondRead);
						Self::add_register(flags, info, base_register, OpAccess::CondRead);
						Self::add_register(flags, info, base_register, OpAccess::CondWrite);
					}
					const_assert_eq!(OpKind::MemoryESEDI as u32, OpKind::MemoryESDI as u32 + 1);
					const_assert_eq!(OpKind::MemoryESRDI as u32, OpKind::MemoryESDI as u32 + 2);
					const_assert_eq!(Register::EDI as u32, Register::DI as u32 + 16);
					const_assert_eq!(Register::RDI as u32, Register::DI as u32 + 32);
					base_register = unsafe {
						mem::transmute(
							((instruction.op1_kind() as u32).wrapping_sub(OpKind::MemoryESDI as u32) << 4).wrapping_add(Register::DI as u32) as u8,
						)
					};
					if (flags & Flags::NO_MEMORY_USAGE) == 0 {
						Self::add_memory(
							info,
							Register::ES,
							base_register,
							Register::None,
							1,
							0,
							MemorySize::Unknown,
							OpAccess::CondRead,
						);
					}
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						if (flags & Flags::IS_64BIT) == 0 {
							Self::add_register(flags, info, Register::ES, OpAccess::CondRead);
						}
						Self::add_register(flags, info, base_register, OpAccess::CondRead);
						Self::add_register(flags, info, base_register, OpAccess::CondWrite);
					}
				} else {
					const_assert_eq!(OpKind::MemorySegESI as u32, OpKind::MemorySegSI as u32 + 1);
					const_assert_eq!(OpKind::MemorySegRSI as u32, OpKind::MemorySegSI as u32 + 2);
					const_assert_eq!(Register::ESI as u32, Register::SI as u32 + 16);
					const_assert_eq!(Register::RSI as u32, Register::SI as u32 + 32);
					base_register = unsafe {
						mem::transmute(
							((instruction.op0_kind() as u32).wrapping_sub(OpKind::MemorySegSI as u32) << 4).wrapping_add(Register::SI as u32) as u8,
						)
					};
					if (flags & Flags::NO_MEMORY_USAGE) == 0 {
						Self::add_memory(
							info,
							instruction.memory_segment(),
							base_register,
							Register::None,
							1,
							0,
							instruction.memory_size(),
							OpAccess::Read,
						);
					}
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						Self::add_memory_segment_register(flags, info, instruction.memory_segment(), OpAccess::Read);
						Self::add_register(flags, info, base_register, OpAccess::ReadWrite);
					}
					const_assert_eq!(OpKind::MemoryESEDI as u32, OpKind::MemoryESDI as u32 + 1);
					const_assert_eq!(OpKind::MemoryESRDI as u32, OpKind::MemoryESDI as u32 + 2);
					const_assert_eq!(Register::EDI as u32, Register::DI as u32 + 16);
					const_assert_eq!(Register::RDI as u32, Register::DI as u32 + 32);
					base_register = unsafe {
						mem::transmute(
							((instruction.op1_kind() as u32).wrapping_sub(OpKind::MemoryESDI as u32) << 4).wrapping_add(Register::DI as u32) as u8,
						)
					};
					if (flags & Flags::NO_MEMORY_USAGE) == 0 {
						Self::add_memory(
							info,
							Register::ES,
							base_register,
							Register::None,
							1,
							0,
							instruction.memory_size(),
							OpAccess::Read,
						);
					}
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						if (flags & Flags::IS_64BIT) == 0 {
							Self::add_register(flags, info, Register::ES, OpAccess::Read);
						}
						Self::add_register(flags, info, base_register, OpAccess::ReadWrite);
					}
				}
			}

			CodeInfo::Stos => {
				if super::super::instruction_internal::internal_has_repe_or_repne_prefix(instruction) {
					info.op_accesses[0] = OpAccess::CondWrite;
					info.op_accesses[1] = OpAccess::CondRead;
					const_assert_eq!(OpKind::MemoryESEDI as u32, OpKind::MemoryESDI as u32 + 1);
					const_assert_eq!(OpKind::MemoryESRDI as u32, OpKind::MemoryESDI as u32 + 2);
					const_assert_eq!(Register::EDI as u32, Register::DI as u32 + 16);
					const_assert_eq!(Register::RDI as u32, Register::DI as u32 + 32);
					const_assert_eq!(Register::ECX as u32, Register::CX as u32 + 16);
					const_assert_eq!(Register::RCX as u32, Register::CX as u32 + 32);
					base_register = unsafe {
						mem::transmute(
							((instruction.op0_kind() as u32).wrapping_sub(OpKind::MemoryESDI as u32) << 4).wrapping_add(Register::DI as u32) as u8,
						)
					};
					if (flags & Flags::NO_MEMORY_USAGE) == 0 {
						Self::add_memory(
							info,
							Register::ES,
							base_register,
							Register::None,
							1,
							0,
							MemorySize::Unknown,
							OpAccess::CondWrite,
						);
					}
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						debug_assert_eq!(1, info.used_registers.len());
						info.used_registers[0] = UsedRegister {
							register: info.used_registers[0].register,
							access: OpAccess::CondRead,
						};
						Self::add_register(
							flags,
							info,
							unsafe {
								mem::transmute(
									((instruction.op0_kind() as u32).wrapping_sub(OpKind::MemoryESDI as u32) << 4).wrapping_add(Register::CX as u32)
										as u8,
								)
							},
							OpAccess::ReadCondWrite,
						);
						if (flags & Flags::IS_64BIT) == 0 {
							Self::add_register(flags, info, Register::ES, OpAccess::CondRead);
						}
						Self::add_register(flags, info, base_register, OpAccess::CondRead);
						Self::add_register(flags, info, base_register, OpAccess::CondWrite);
					}
				} else {
					const_assert_eq!(OpKind::MemoryESEDI as u32, OpKind::MemoryESDI as u32 + 1);
					const_assert_eq!(OpKind::MemoryESRDI as u32, OpKind::MemoryESDI as u32 + 2);
					const_assert_eq!(Register::EDI as u32, Register::DI as u32 + 16);
					const_assert_eq!(Register::RDI as u32, Register::DI as u32 + 32);
					base_register = unsafe {
						mem::transmute(
							((instruction.op0_kind() as u32).wrapping_sub(OpKind::MemoryESDI as u32) << 4).wrapping_add(Register::DI as u32) as u8,
						)
					};
					if (flags & Flags::NO_MEMORY_USAGE) == 0 {
						Self::add_memory(
							info,
							Register::ES,
							base_register,
							Register::None,
							1,
							0,
							instruction.memory_size(),
							OpAccess::Write,
						);
					}
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						if (flags & Flags::IS_64BIT) == 0 {
							Self::add_register(flags, info, Register::ES, OpAccess::Read);
						}
						Self::add_register(flags, info, base_register, OpAccess::ReadWrite);
					}
				}
			}

			CodeInfo::Lods => {
				if super::super::instruction_internal::internal_has_repe_or_repne_prefix(instruction) {
					info.op_accesses[0] = OpAccess::CondWrite;
					info.op_accesses[1] = OpAccess::CondRead;
					const_assert_eq!(OpKind::MemorySegESI as u32, OpKind::MemorySegSI as u32 + 1);
					const_assert_eq!(OpKind::MemorySegRSI as u32, OpKind::MemorySegSI as u32 + 2);
					const_assert_eq!(Register::ESI as u32, Register::SI as u32 + 16);
					const_assert_eq!(Register::RSI as u32, Register::SI as u32 + 32);
					const_assert_eq!(Register::ECX as u32, Register::CX as u32 + 16);
					const_assert_eq!(Register::RCX as u32, Register::CX as u32 + 32);
					base_register = unsafe {
						mem::transmute(
							((instruction.op1_kind() as u32).wrapping_sub(OpKind::MemorySegSI as u32) << 4).wrapping_add(Register::SI as u32) as u8,
						)
					};
					if (flags & Flags::NO_MEMORY_USAGE) == 0 {
						Self::add_memory(
							info,
							instruction.memory_segment(),
							base_register,
							Register::None,
							1,
							0,
							MemorySize::Unknown,
							OpAccess::CondRead,
						);
					}
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						debug_assert_eq!(1, info.used_registers.len());
						info.used_registers[0] = UsedRegister {
							register: info.used_registers[0].register,
							access: OpAccess::CondWrite,
						};
						Self::add_register(
							flags,
							info,
							unsafe {
								mem::transmute(
									((instruction.op1_kind() as u32).wrapping_sub(OpKind::MemorySegSI as u32) << 4).wrapping_add(Register::CX as u32)
										as u8,
								)
							},
							OpAccess::ReadCondWrite,
						);
						Self::add_memory_segment_register(flags, info, instruction.memory_segment(), OpAccess::CondRead);
						Self::add_register(flags, info, base_register, OpAccess::CondRead);
						Self::add_register(flags, info, base_register, OpAccess::CondWrite);
					}
				} else {
					const_assert_eq!(OpKind::MemorySegESI as u32, OpKind::MemorySegSI as u32 + 1);
					const_assert_eq!(OpKind::MemorySegRSI as u32, OpKind::MemorySegSI as u32 + 2);
					const_assert_eq!(Register::ESI as u32, Register::SI as u32 + 16);
					const_assert_eq!(Register::RSI as u32, Register::SI as u32 + 32);
					base_register = unsafe {
						mem::transmute(
							((instruction.op1_kind() as u32).wrapping_sub(OpKind::MemorySegSI as u32) << 4).wrapping_add(Register::SI as u32) as u8,
						)
					};
					if (flags & Flags::NO_MEMORY_USAGE) == 0 {
						Self::add_memory(
							info,
							instruction.memory_segment(),
							base_register,
							Register::None,
							1,
							0,
							instruction.memory_size(),
							OpAccess::Read,
						);
					}
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						Self::add_memory_segment_register(flags, info, instruction.memory_segment(), OpAccess::Read);
						Self::add_register(flags, info, base_register, OpAccess::ReadWrite);
					}
				}
			}

			CodeInfo::Scas => {
				if super::super::instruction_internal::internal_has_repe_or_repne_prefix(instruction) {
					info.op_accesses[0] = OpAccess::CondRead;
					info.op_accesses[1] = OpAccess::CondRead;
					const_assert_eq!(OpKind::MemoryESEDI as u32, OpKind::MemoryESDI as u32 + 1);
					const_assert_eq!(OpKind::MemoryESRDI as u32, OpKind::MemoryESDI as u32 + 2);
					const_assert_eq!(Register::EDI as u32, Register::DI as u32 + 16);
					const_assert_eq!(Register::RDI as u32, Register::DI as u32 + 32);
					const_assert_eq!(Register::ECX as u32, Register::CX as u32 + 16);
					const_assert_eq!(Register::RCX as u32, Register::CX as u32 + 32);
					base_register = unsafe {
						mem::transmute(
							((instruction.op1_kind() as u32).wrapping_sub(OpKind::MemoryESDI as u32) << 4).wrapping_add(Register::DI as u32) as u8,
						)
					};
					if (flags & Flags::NO_MEMORY_USAGE) == 0 {
						Self::add_memory(
							info,
							Register::ES,
							base_register,
							Register::None,
							1,
							0,
							MemorySize::Unknown,
							OpAccess::CondRead,
						);
					}
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						debug_assert_eq!(1, info.used_registers.len());
						info.used_registers[0] = UsedRegister {
							register: info.used_registers[0].register,
							access: OpAccess::CondRead,
						};
						Self::add_register(
							flags,
							info,
							unsafe {
								mem::transmute(
									((instruction.op1_kind() as u32).wrapping_sub(OpKind::MemoryESDI as u32) << 4).wrapping_add(Register::CX as u32)
										as u8,
								)
							},
							OpAccess::ReadCondWrite,
						);
						if (flags & Flags::IS_64BIT) == 0 {
							Self::add_register(flags, info, Register::ES, OpAccess::CondRead);
						}
						Self::add_register(flags, info, base_register, OpAccess::CondRead);
						Self::add_register(flags, info, base_register, OpAccess::CondWrite);
					}
				} else {
					const_assert_eq!(OpKind::MemoryESEDI as u32, OpKind::MemoryESDI as u32 + 1);
					const_assert_eq!(OpKind::MemoryESRDI as u32, OpKind::MemoryESDI as u32 + 2);
					const_assert_eq!(Register::EDI as u32, Register::DI as u32 + 16);
					const_assert_eq!(Register::RDI as u32, Register::DI as u32 + 32);
					base_register = unsafe {
						mem::transmute(
							((instruction.op1_kind() as u32).wrapping_sub(OpKind::MemoryESDI as u32) << 4).wrapping_add(Register::DI as u32) as u8,
						)
					};
					if (flags & Flags::NO_MEMORY_USAGE) == 0 {
						Self::add_memory(
							info,
							Register::ES,
							base_register,
							Register::None,
							1,
							0,
							instruction.memory_size(),
							OpAccess::Read,
						);
					}
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						if (flags & Flags::IS_64BIT) == 0 {
							Self::add_register(flags, info, Register::ES, OpAccess::Read);
						}
						Self::add_register(flags, info, base_register, OpAccess::ReadWrite);
					}
				}
			}

			CodeInfo::Cmpxchg => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					code = instruction.code();
					if code == Code::Cmpxchg_rm64_r64 {
						Self::add_register(flags, info, Register::RAX, OpAccess::ReadCondWrite);
					} else if code == Code::Cmpxchg_rm32_r32 || code == Code::Cmpxchg486_rm32_r32 {
						Self::add_register(flags, info, Register::EAX, OpAccess::ReadCondWrite);
					} else if code == Code::Cmpxchg_rm16_r16 || code == Code::Cmpxchg486_rm16_r16 {
						Self::add_register(flags, info, Register::AX, OpAccess::ReadCondWrite);
					} else {
						debug_assert!(code == Code::Cmpxchg_rm8_r8 || code == Code::Cmpxchg486_rm8_r8);
						Self::add_register(flags, info, Register::AL, OpAccess::ReadCondWrite);
					}
				}
			}

			CodeInfo::Cmpxchg8b => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					if instruction.code() == Code::Cmpxchg16b_m128 {
						Self::add_register(flags, info, Register::RDX, OpAccess::ReadCondWrite);
						Self::add_register(flags, info, Register::RAX, OpAccess::ReadCondWrite);
						Self::add_register(flags, info, Register::RCX, OpAccess::CondRead);
						Self::add_register(flags, info, Register::RBX, OpAccess::CondRead);
					} else {
						debug_assert_eq!(Code::Cmpxchg8b_m64, instruction.code());
						Self::add_register(flags, info, Register::EDX, OpAccess::ReadCondWrite);
						Self::add_register(flags, info, Register::EAX, OpAccess::ReadCondWrite);
						Self::add_register(flags, info, Register::ECX, OpAccess::CondRead);
						Self::add_register(flags, info, Register::EBX, OpAccess::CondRead);
					}
				}
			}

			CodeInfo::Cpuid => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::EAX, OpAccess::ReadWrite);
					Self::add_register(flags, info, Register::ECX, OpAccess::CondRead);
					Self::add_register(flags, info, Register::ECX, OpAccess::Write);
					Self::add_register(flags, info, Register::EDX, OpAccess::Write);
					Self::add_register(flags, info, Register::EBX, OpAccess::Write);
				}
			}

			CodeInfo::Div => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					code = instruction.code();
					if code == Code::Idiv_rm64 || code == Code::Div_rm64 {
						Self::add_register(flags, info, Register::RDX, OpAccess::ReadWrite);
						Self::add_register(flags, info, Register::RAX, OpAccess::ReadWrite);
					} else if code == Code::Idiv_rm32 || code == Code::Div_rm32 {
						Self::add_register(flags, info, Register::EDX, OpAccess::ReadWrite);
						Self::add_register(flags, info, Register::EAX, OpAccess::ReadWrite);
					} else if code == Code::Idiv_rm16 || code == Code::Div_rm16 {
						Self::add_register(flags, info, Register::DX, OpAccess::ReadWrite);
						Self::add_register(flags, info, Register::AX, OpAccess::ReadWrite);
					} else {
						debug_assert!(code == Code::Idiv_rm8 || code == Code::Div_rm8);
						Self::add_register(flags, info, Register::AX, OpAccess::ReadWrite);
					}
				}
			}

			CodeInfo::Mul => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					code = instruction.code();
					if code == Code::Imul_rm64 || code == Code::Mul_rm64 {
						Self::add_register(flags, info, Register::RAX, OpAccess::ReadWrite);
						Self::add_register(flags, info, Register::RDX, OpAccess::Write);
					} else if code == Code::Imul_rm32 || code == Code::Mul_rm32 {
						Self::add_register(flags, info, Register::EAX, OpAccess::ReadWrite);
						Self::add_register(flags, info, Register::EDX, OpAccess::Write);
					} else if code == Code::Imul_rm16 || code == Code::Mul_rm16 {
						Self::add_register(flags, info, Register::AX, OpAccess::ReadWrite);
						Self::add_register(flags, info, Register::DX, OpAccess::Write);
					} else {
						debug_assert!(code == Code::Imul_rm8 || code == Code::Mul_rm8);
						Self::add_register(flags, info, Register::AL, OpAccess::Read);
						Self::add_register(flags, info, Register::AX, OpAccess::Write);
					}
				}
			}

			CodeInfo::Enter => {
				xsp = Self::get_xsp(instruction.code_size(), &mut xsp_mask);
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					if (flags & Flags::IS_64BIT) == 0 {
						Self::add_register(flags, info, Register::SS, OpAccess::Read);
					}
					Self::add_register(flags, info, xsp, OpAccess::ReadWrite);
				}

				let op_size;
				code = instruction.code();
				let r_sp = if code == Code::Enterq_imm16_imm8 {
					op_size = 8;
					memory_size = MemorySize::UInt64;
					Register::RSP
				} else if code == Code::Enterd_imm16_imm8 {
					op_size = 4;
					memory_size = MemorySize::UInt32;
					Register::ESP
				} else {
					debug_assert_eq!(Code::Enterw_imm16_imm8, code);
					op_size = 2;
					memory_size = MemorySize::UInt16;
					Register::SP
				};

				if r_sp != xsp && (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, r_sp, OpAccess::ReadWrite);
				}

				let nesting_level = (instruction.immediate8_2nd() & 0x1F) as u64;
				let mut xsp_offset = 0u64;
				// push rBP
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(
						flags,
						info,
						unsafe { mem::transmute((r_sp as u32).wrapping_add(1) as u8) },
						OpAccess::ReadWrite,
					);
				}
				if (flags & Flags::NO_MEMORY_USAGE) == 0 {
					xsp_offset = xsp_offset.wrapping_sub(op_size);
					Self::add_memory(
						info,
						Register::SS,
						xsp,
						Register::None,
						1,
						xsp_offset & xsp_mask,
						memory_size,
						OpAccess::Write,
					);
				}

				if nesting_level != 0 {
					let xbp = unsafe { mem::transmute((xsp as u32).wrapping_add(1) as u8) }; // rBP immediately follows rSP
					let mut xbp_offset = 0u64;
					for i in 1..(nesting_level as u32) {
						if i == 1 && r_sp as u32 + 1 != xbp as u32 && (flags & Flags::NO_REGISTER_USAGE) == 0 {
							Self::add_register(flags, info, xbp, OpAccess::ReadWrite);
						}
						// push [xbp]
						if (flags & Flags::NO_MEMORY_USAGE) == 0 {
							xbp_offset = xbp_offset.wrapping_sub(op_size);
							Self::add_memory(
								info,
								Register::SS,
								xbp,
								Register::None,
								1,
								xbp_offset & xsp_mask,
								memory_size,
								OpAccess::Read,
							);
							xsp_offset = xsp_offset.wrapping_sub(op_size);
							Self::add_memory(
								info,
								Register::SS,
								xsp,
								Register::None,
								1,
								xsp_offset & xsp_mask,
								memory_size,
								OpAccess::Write,
							);
						}
					}
					// push frameTemp
					if (flags & Flags::NO_MEMORY_USAGE) == 0 {
						xsp_offset = xsp_offset.wrapping_sub(op_size);
						Self::add_memory(
							info,
							Register::SS,
							xsp,
							Register::None,
							1,
							xsp_offset & xsp_mask,
							memory_size,
							OpAccess::Write,
						);
					}
				}
			}

			CodeInfo::Leave => {
				xsp = Self::get_xsp(instruction.code_size(), &mut xsp_mask);
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					if (flags & Flags::IS_64BIT) == 0 {
						Self::add_register(flags, info, Register::SS, OpAccess::Read);
					}
					Self::add_register(flags, info, xsp, OpAccess::Write);
				}

				code = instruction.code();
				if code == Code::Leaveq {
					if (flags & Flags::NO_MEMORY_USAGE) == 0 {
						Self::add_memory(
							info,
							Register::SS,
							unsafe { mem::transmute((xsp as u32).wrapping_add(1) as u8) },
							Register::None,
							1,
							0,
							MemorySize::UInt64,
							OpAccess::Read,
						);
					}
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						if xsp as u32 + 1 == Register::RBP as u32 {
							Self::add_register(flags, info, Register::RBP, OpAccess::ReadWrite);
						} else {
							Self::add_register(flags, info, unsafe { mem::transmute((xsp as u32).wrapping_add(1) as u8) }, OpAccess::Read);
							Self::add_register(flags, info, Register::RBP, OpAccess::Write);
						}
					}
				} else if code == Code::Leaved {
					if (flags & Flags::NO_MEMORY_USAGE) == 0 {
						Self::add_memory(
							info,
							Register::SS,
							unsafe { mem::transmute((xsp as u32).wrapping_add(1) as u8) },
							Register::None,
							1,
							0,
							MemorySize::UInt32,
							OpAccess::Read,
						);
					}
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						if xsp as u32 + 1 == Register::EBP as u32 {
							Self::add_register(flags, info, Register::EBP, OpAccess::ReadWrite);
						} else {
							Self::add_register(flags, info, unsafe { mem::transmute((xsp as u32).wrapping_add(1) as u8) }, OpAccess::Read);
							Self::add_register(flags, info, Register::EBP, OpAccess::Write);
						}
					}
				} else {
					debug_assert_eq!(Code::Leavew, code);
					if (flags & Flags::NO_MEMORY_USAGE) == 0 {
						Self::add_memory(
							info,
							Register::SS,
							unsafe { mem::transmute((xsp as u32).wrapping_add(1) as u8) },
							Register::None,
							1,
							0,
							MemorySize::UInt16,
							OpAccess::Read,
						);
					}
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						if xsp as u32 + 1 == Register::BP as u32 {
							Self::add_register(flags, info, Register::BP, OpAccess::ReadWrite);
						} else {
							Self::add_register(flags, info, unsafe { mem::transmute((xsp as u32).wrapping_add(1) as u8) }, OpAccess::Read);
							Self::add_register(flags, info, Register::BP, OpAccess::Write);
						}
					}
				}
			}

			CodeInfo::Iret => {
				xsp = Self::get_xsp(instruction.code_size(), &mut xsp_mask);
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(
						flags,
						info,
						Register::SS,
						if (flags & Flags::IS_64BIT) != 0 {
							OpAccess::Write
						} else {
							OpAccess::Read
						},
					);
					Self::add_register(flags, info, xsp, OpAccess::ReadWrite);
				}
				if (flags & Flags::NO_MEMORY_USAGE) == 0 {
					code = instruction.code();
					if code == Code::Iretq {
						Self::add_memory(info, Register::SS, xsp, Register::None, 1, 0 * 8, MemorySize::UInt64, OpAccess::Read);
						Self::add_memory(info, Register::SS, xsp, Register::None, 1, 1 * 8, MemorySize::UInt64, OpAccess::Read);
						Self::add_memory(info, Register::SS, xsp, Register::None, 1, 2 * 8, MemorySize::UInt64, OpAccess::Read);
						Self::add_memory(info, Register::SS, xsp, Register::None, 1, 3 * 8, MemorySize::UInt64, OpAccess::Read);
						Self::add_memory(info, Register::SS, xsp, Register::None, 1, 4 * 8, MemorySize::UInt64, OpAccess::Read);
					} else if code == Code::Iretd {
						Self::add_memory(info, Register::SS, xsp, Register::None, 1, 0 * 4, MemorySize::UInt32, OpAccess::Read);
						Self::add_memory(info, Register::SS, xsp, Register::None, 1, 1 * 4, MemorySize::UInt32, OpAccess::Read);
						Self::add_memory(info, Register::SS, xsp, Register::None, 1, 2 * 4, MemorySize::UInt32, OpAccess::Read);
						if instruction.code_size() == CodeSize::Code64 {
							Self::add_memory(info, Register::SS, xsp, Register::None, 1, 3 * 4, MemorySize::UInt32, OpAccess::Read);
							Self::add_memory(info, Register::SS, xsp, Register::None, 1, 4 * 4, MemorySize::UInt32, OpAccess::Read);
						}
					} else {
						debug_assert_eq!(Code::Iretw, code);
						Self::add_memory(info, Register::SS, xsp, Register::None, 1, 0 * 2, MemorySize::UInt16, OpAccess::Read);
						Self::add_memory(info, Register::SS, xsp, Register::None, 1, 1 * 2, MemorySize::UInt16, OpAccess::Read);
						Self::add_memory(info, Register::SS, xsp, Register::None, 1, 2 * 2, MemorySize::UInt16, OpAccess::Read);
						if instruction.code_size() == CodeSize::Code64 {
							Self::add_memory(info, Register::SS, xsp, Register::None, 1, 3 * 2, MemorySize::UInt16, OpAccess::Read);
							Self::add_memory(info, Register::SS, xsp, Register::None, 1, 4 * 2, MemorySize::UInt16, OpAccess::Read);
						}
					}
				}
			}

			CodeInfo::Vzeroall => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					let access = if instruction.code() == Code::VEX_Vzeroupper {
						OpAccess::ReadWrite
					} else {
						debug_assert_eq!(Code::VEX_Vzeroall, instruction.code());
						OpAccess::Write
					};
					let max_vec_regs = if (flags & Flags::IS_64BIT) != 0 {
						16 // regs 16-31 are not modified
					} else {
						8
					};
					for i in 0..max_vec_regs {
						Self::add_register(
							flags,
							info,
							unsafe { mem::transmute((IcedConstants::VMM_FIRST as u32).wrapping_add(i) as u8) },
							access,
						);
					}
				}
			}

			CodeInfo::Jrcxz => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					code = instruction.code();
					if code == Code::Jrcxz_rel8_64 || code == Code::Jrcxz_rel8_16 {
						Self::add_register(flags, info, Register::RCX, OpAccess::Read);
					} else if code == Code::Jecxz_rel8_64 || code == Code::Jecxz_rel8_32 || code == Code::Jecxz_rel8_16 {
						Self::add_register(flags, info, Register::ECX, OpAccess::Read);
					} else {
						debug_assert!(code == Code::Jcxz_rel8_32 || code == Code::Jcxz_rel8_16);
						Self::add_register(flags, info, Register::CX, OpAccess::Read);
					}
				}
			}

			CodeInfo::Loop => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					code = instruction.code();
					if code == Code::Loopne_rel8_64_RCX
						|| code == Code::Loope_rel8_64_RCX
						|| code == Code::Loop_rel8_64_RCX
						|| code == Code::Loopne_rel8_16_RCX
						|| code == Code::Loope_rel8_16_RCX
						|| code == Code::Loop_rel8_16_RCX
					{
						Self::add_register(flags, info, Register::RCX, OpAccess::ReadWrite);
					} else if code == Code::Loopne_rel8_16_ECX
						|| code == Code::Loopne_rel8_32_ECX
						|| code == Code::Loopne_rel8_64_ECX
						|| code == Code::Loope_rel8_16_ECX
						|| code == Code::Loope_rel8_32_ECX
						|| code == Code::Loope_rel8_64_ECX
						|| code == Code::Loop_rel8_16_ECX
						|| code == Code::Loop_rel8_32_ECX
						|| code == Code::Loop_rel8_64_ECX
					{
						Self::add_register(flags, info, Register::ECX, OpAccess::ReadWrite);
					} else {
						debug_assert!(
							code == Code::Loopne_rel8_16_CX
								|| code == Code::Loopne_rel8_32_CX
								|| code == Code::Loope_rel8_16_CX
								|| code == Code::Loope_rel8_32_CX
								|| code == Code::Loop_rel8_16_CX || code == Code::Loop_rel8_32_CX
						);
						Self::add_register(flags, info, Register::CX, OpAccess::ReadWrite);
					}
				}
			}

			CodeInfo::Lahf => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(
						flags,
						info,
						Register::AH,
						if instruction.code() == Code::Sahf {
							OpAccess::Read
						} else {
							OpAccess::Write
						},
					);
				}
			}

			CodeInfo::Lds => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					code = instruction.code();
					if Code::Lfs_r16_m1616 <= code && code <= Code::Lfs_r64_m1664 {
						Self::add_register(flags, info, Register::FS, OpAccess::Write);
					} else if Code::Lgs_r16_m1616 <= code && code <= Code::Lgs_r64_m1664 {
						Self::add_register(flags, info, Register::GS, OpAccess::Write);
					} else if Code::Lss_r16_m1616 <= code && code <= Code::Lss_r64_m1664 {
						Self::add_register(flags, info, Register::SS, OpAccess::Write);
					} else if Code::Lds_r16_m1616 <= code && code <= Code::Lds_r32_m1632 {
						Self::add_register(flags, info, Register::DS, OpAccess::Write);
					} else {
						debug_assert!(Code::Les_r16_m1616 <= code && code <= Code::Les_r32_m1632);
						Self::add_register(flags, info, Register::ES, OpAccess::Write);
					}
				}
			}

			CodeInfo::Maskmovq => {
				base_register = match instruction.op0_kind() {
					OpKind::MemorySegDI => Register::DI,
					OpKind::MemorySegEDI => Register::EDI,
					_ => {
						debug_assert_eq!(OpKind::MemorySegRDI, instruction.op0_kind());
						Register::RDI
					}
				};
				if (flags & Flags::NO_MEMORY_USAGE) == 0 {
					Self::add_memory(
						info,
						instruction.memory_segment(),
						base_register,
						Register::None,
						1,
						0,
						instruction.memory_size(),
						OpAccess::Write,
					);
				}
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_memory_segment_register(flags, info, instruction.memory_segment(), OpAccess::Read);
					Self::add_register(flags, info, base_register, OpAccess::Read);
				}
			}

			CodeInfo::Monitor => {
				code = instruction.code();
				base_register = if code == Code::Monitorq || code == Code::Monitorxq {
					Register::RAX
				} else if code == Code::Monitord || code == Code::Monitorxd {
					Register::EAX
				} else {
					debug_assert!(code == Code::Monitorw || code == Code::Monitorxw);
					Register::AX
				};
				let mut seg = instruction.segment_prefix();
				if seg == Register::None {
					seg = Register::DS;
				}
				if (flags & Flags::NO_MEMORY_USAGE) == 0 {
					Self::add_memory(info, seg, base_register, Register::None, 1, 0, MemorySize::Unknown, OpAccess::Read);
				}
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_memory_segment_register(flags, info, seg, OpAccess::Read);
					Self::add_register(flags, info, base_register, OpAccess::Read);
					if (flags & Flags::IS_64BIT) != 0 {
						Self::add_register(flags, info, Register::RCX, OpAccess::Read);
						Self::add_register(flags, info, Register::RDX, OpAccess::Read);
					} else {
						Self::add_register(flags, info, Register::ECX, OpAccess::Read);
						Self::add_register(flags, info, Register::EDX, OpAccess::Read);
					}
				}
			}

			CodeInfo::Mwait => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					if (flags & Flags::IS_64BIT) != 0 {
						Self::add_register(flags, info, Register::RAX, OpAccess::Read);
						Self::add_register(flags, info, Register::RCX, OpAccess::Read);
					} else {
						Self::add_register(flags, info, Register::EAX, OpAccess::Read);
						Self::add_register(flags, info, Register::ECX, OpAccess::Read);
					}
				}
			}

			CodeInfo::Mwaitx => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					if (flags & Flags::IS_64BIT) != 0 {
						Self::add_register(flags, info, Register::RAX, OpAccess::Read);
						Self::add_register(flags, info, Register::RCX, OpAccess::Read);
						Self::add_register(flags, info, Register::RBX, OpAccess::CondRead);
					} else {
						Self::add_register(flags, info, Register::EAX, OpAccess::Read);
						Self::add_register(flags, info, Register::ECX, OpAccess::Read);
						Self::add_register(flags, info, Register::EBX, OpAccess::CondRead);
					}
				}
			}

			CodeInfo::Mulx => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					if instruction.code() == Code::VEX_Mulx_r32_r32_rm32 {
						Self::add_register(flags, info, Register::EDX, OpAccess::Read);
					} else {
						debug_assert_eq!(Code::VEX_Mulx_r64_r64_rm64, instruction.code());
						Self::add_register(flags, info, Register::RDX, OpAccess::Read);
					}
				}
			}

			CodeInfo::PcmpXstrY => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					code = instruction.code();
					if code == Code::Pcmpestrm_xmm_xmmm128_imm8
						|| code == Code::VEX_Vpcmpestrm_xmm_xmmm128_imm8
						|| code == Code::Pcmpestri_xmm_xmmm128_imm8
						|| code == Code::VEX_Vpcmpestri_xmm_xmmm128_imm8
					{
						Self::add_register(flags, info, Register::EAX, OpAccess::Read);
						Self::add_register(flags, info, Register::EDX, OpAccess::Read);
					} else if code == Code::Pcmpestrm64_xmm_xmmm128_imm8
						|| code == Code::VEX_Vpcmpestrm64_xmm_xmmm128_imm8
						|| code == Code::Pcmpestri64_xmm_xmmm128_imm8
						|| code == Code::VEX_Vpcmpestri64_xmm_xmmm128_imm8
					{
						Self::add_register(flags, info, Register::RAX, OpAccess::Read);
						Self::add_register(flags, info, Register::RDX, OpAccess::Read);
					}

					if code == Code::Pcmpestrm_xmm_xmmm128_imm8
						|| code == Code::VEX_Vpcmpestrm_xmm_xmmm128_imm8
						|| code == Code::Pcmpestrm64_xmm_xmmm128_imm8
						|| code == Code::VEX_Vpcmpestrm64_xmm_xmmm128_imm8
						|| code == Code::Pcmpistrm_xmm_xmmm128_imm8
						|| code == Code::VEX_Vpcmpistrm_xmm_xmmm128_imm8
					{
						Self::add_register(flags, info, Register::XMM0, OpAccess::Write);
					} else {
						debug_assert!(
							code == Code::Pcmpestri_xmm_xmmm128_imm8
								|| code == Code::VEX_Vpcmpestri_xmm_xmmm128_imm8
								|| code == Code::Pcmpestri64_xmm_xmmm128_imm8
								|| code == Code::VEX_Vpcmpestri64_xmm_xmmm128_imm8
								|| code == Code::Pcmpistri_xmm_xmmm128_imm8
								|| code == Code::VEX_Vpcmpistri_xmm_xmmm128_imm8
						);
						Self::add_register(flags, info, Register::ECX, OpAccess::Write);
					}
				}
			}

			CodeInfo::Shift_Ib_MASK1FMOD9 => {
				if (instruction.immediate8() & 0x1F) % 9 == 0 {
					info.rflags_info = RflagsInfo::None as usize;
				}
			}

			CodeInfo::Shift_Ib_MASK1FMOD11 => {
				if (instruction.immediate8() & 0x1F) % 17 == 0 {
					info.rflags_info = RflagsInfo::None as usize;
				}
			}

			CodeInfo::Shift_Ib_MASK1F => {
				if (instruction.immediate8() & 0x1F) == 0 {
					info.rflags_info = RflagsInfo::None as usize;
				}
			}

			CodeInfo::Shift_Ib_MASK3F => {
				if (instruction.immediate8() & 0x3F) == 0 {
					info.rflags_info = RflagsInfo::None as usize;
				}
			}

			CodeInfo::R_EAX_EDX => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::EAX, OpAccess::Read);
					Self::add_register(flags, info, Register::EDX, OpAccess::Read);
				}
			}

			CodeInfo::R_ECX_W_EAX_EDX => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::EAX, OpAccess::Write);
					Self::add_register(flags, info, Register::ECX, OpAccess::Read);
					Self::add_register(flags, info, Register::EDX, OpAccess::Write);
				}
			}

			CodeInfo::R_EAX_ECX_EDX => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::EAX, OpAccess::Read);
					Self::add_register(flags, info, Register::ECX, OpAccess::Read);
					Self::add_register(flags, info, Register::EDX, OpAccess::Read);
				}
			}

			CodeInfo::W_EAX_EDX => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::EAX, OpAccess::Write);
					Self::add_register(flags, info, Register::EDX, OpAccess::Write);
				}
			}

			CodeInfo::W_EAX_ECX_EDX => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::EAX, OpAccess::Write);
					Self::add_register(flags, info, Register::ECX, OpAccess::Write);
					Self::add_register(flags, info, Register::EDX, OpAccess::Write);
				}
			}

			CodeInfo::Pconfig => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::EAX, OpAccess::ReadWrite);
					base_register = if (flags & Flags::IS_64BIT) != 0 { Register::RAX } else { Register::EAX };
					Self::add_register(
						flags,
						info,
						unsafe { mem::transmute((base_register as u32).wrapping_add(1) as u8) },
						OpAccess::CondRead,
					);
					Self::add_register(
						flags,
						info,
						unsafe { mem::transmute((base_register as u32).wrapping_add(2) as u8) },
						OpAccess::CondRead,
					);
					Self::add_register(
						flags,
						info,
						unsafe { mem::transmute((base_register as u32).wrapping_add(3) as u8) },
						OpAccess::CondRead,
					);
				}
			}

			CodeInfo::Syscall => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					code = instruction.code();
					if code == Code::Syscall {
						Self::add_register(flags, info, Register::ECX, OpAccess::Write);
						if (flags & Flags::IS_64BIT) != 0 {
							Self::add_register(flags, info, Register::R11, OpAccess::Write);
						}
					} else if code == Code::Sysenter {
						Self::add_register(
							flags,
							info,
							if (flags & Flags::IS_64BIT) != 0 { Register::RSP } else { Register::ESP },
							OpAccess::Write,
						);
					} else if code == Code::Sysretq {
						Self::add_register(flags, info, Register::RCX, OpAccess::Read);
						Self::add_register(flags, info, Register::R11, OpAccess::Read);
					} else if code == Code::Sysexitq {
						Self::add_register(flags, info, Register::RCX, OpAccess::Read);
						Self::add_register(flags, info, Register::RDX, OpAccess::Read);
						Self::add_register(flags, info, Register::RSP, OpAccess::Write);
					} else if code == Code::Sysretd {
						Self::add_register(flags, info, Register::ECX, OpAccess::Read);
						if (flags & Flags::IS_64BIT) != 0 {
							Self::add_register(flags, info, Register::R11, OpAccess::Read);
						}
					} else {
						debug_assert_eq!(Code::Sysexitd, code);
						Self::add_register(flags, info, Register::ECX, OpAccess::Read);
						Self::add_register(flags, info, Register::EDX, OpAccess::Read);
						Self::add_register(
							flags,
							info,
							if (flags & Flags::IS_64BIT) != 0 { Register::RSP } else { Register::ESP },
							OpAccess::Write,
						);
					}
				}
			}

			CodeInfo::Encls => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					base_register = if (flags & Flags::IS_64BIT) != 0 { Register::RAX } else { Register::EAX };
					Self::add_register(flags, info, Register::EAX, OpAccess::Read);
					// rcx/ecx
					Self::add_register(
						flags,
						info,
						unsafe { mem::transmute((base_register as u32).wrapping_add(1) as u8) },
						OpAccess::CondRead,
					);
					Self::add_register(
						flags,
						info,
						unsafe { mem::transmute((base_register as u32).wrapping_add(1) as u8) },
						OpAccess::CondWrite,
					);
					// rdx/edx
					Self::add_register(
						flags,
						info,
						unsafe { mem::transmute((base_register as u32).wrapping_add(2) as u8) },
						OpAccess::CondRead,
					);
					Self::add_register(
						flags,
						info,
						unsafe { mem::transmute((base_register as u32).wrapping_add(2) as u8) },
						OpAccess::CondWrite,
					);
					// rbx/ebx
					Self::add_register(
						flags,
						info,
						unsafe { mem::transmute((base_register as u32).wrapping_add(3) as u8) },
						OpAccess::CondRead,
					);
					Self::add_register(
						flags,
						info,
						unsafe { mem::transmute((base_register as u32).wrapping_add(3) as u8) },
						OpAccess::CondWrite,
					);
				}
			}

			CodeInfo::Vmfunc => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::EAX, OpAccess::Read);
					Self::add_register(flags, info, Register::ECX, OpAccess::Read);
				}
			}

			CodeInfo::Vmload => {
				code = instruction.code();
				base_register = if code == Code::Vmloadq || code == Code::Vmsaveq || code == Code::Vmrunq {
					Register::RAX
				} else if code == Code::Vmloadd || code == Code::Vmsaved || code == Code::Vmrund {
					Register::EAX
				} else {
					debug_assert!(code == Code::Vmloadw || code == Code::Vmsavew || code == Code::Vmrunw);
					Register::AX
				};
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, base_register, OpAccess::Read);
				}
			}

			CodeInfo::R_CR0 => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::CR0, OpAccess::Read);
				}
			}

			CodeInfo::RW_CR0 => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::CR0, OpAccess::ReadWrite);
					if instruction.op0_kind() == OpKind::Register && instruction.op_count() > 0 {
						debug_assert!(info.used_registers.len() >= 1);
						debug_assert_eq!(instruction.op0_register(), info.used_registers[0].register);
						index = Self::try_get_gpr_16_32_64_index(instruction.op0_register());
						if index >= 0 {
							info.used_registers[0] = UsedRegister {
								register: unsafe { mem::transmute((Register::AX as u32).wrapping_add(index as u32) as u8) },
								access: OpAccess::Read,
							};
						}
					}
				}
			}

			CodeInfo::RW_ST0 => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::ST0, OpAccess::ReadWrite);
				}
			}

			CodeInfo::R_ST0 => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::ST0, OpAccess::Read);
				}
			}

			CodeInfo::W_ST0 => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::ST0, OpAccess::Write);
				}
			}

			CodeInfo::R_ST0_ST1 => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::ST0, OpAccess::Read);
					Self::add_register(flags, info, Register::ST1, OpAccess::Read);
				}
			}

			CodeInfo::R_ST0_R_ST1 => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::ST0, OpAccess::Read);
					Self::add_register(flags, info, Register::ST1, OpAccess::Read);
				}
			}

			CodeInfo::R_ST0_RW_ST1 => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::ST0, OpAccess::Read);
					Self::add_register(flags, info, Register::ST1, OpAccess::ReadWrite);
				}
			}

			CodeInfo::RW_ST0_R_ST1 => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::ST0, OpAccess::ReadWrite);
					Self::add_register(flags, info, Register::ST1, OpAccess::Read);
				}
			}

			CodeInfo::Clzero => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					code = instruction.code();
					base_register = if code == Code::Clzeroq {
						Register::RAX
					} else if code == Code::Clzerod {
						Register::EAX
					} else {
						debug_assert_eq!(Code::Clzerow, code);
						Register::AX
					};
					Self::add_register(flags, info, base_register, OpAccess::Read);
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						base_register = instruction.segment_prefix();
						if base_register == Register::None {
							base_register = Register::DS;
						}
						Self::add_memory_segment_register(flags, info, base_register, OpAccess::Read);
					}
				}
			}

			CodeInfo::Invlpga => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					code = instruction.code();
					base_register = if code == Code::Invlpgaq {
						Register::RAX
					} else if code == Code::Invlpgad {
						Register::EAX
					} else {
						debug_assert_eq!(Code::Invlpgaw, code);
						Register::AX
					};
					Self::add_register(flags, info, base_register, OpAccess::Read);
					Self::add_register(flags, info, Register::ECX, OpAccess::Read);
				}
			}

			CodeInfo::Llwpcb => {
				if (flags & (Flags::NO_REGISTER_USAGE | Flags::IS_64BIT)) == 0 {
					Self::add_register(flags, info, Register::DS, OpAccess::Read);
				}
				if (flags & Flags::NO_MEMORY_USAGE) == 0 {
					Self::add_memory(
						info,
						Register::DS,
						instruction.op0_register(),
						Register::None,
						1,
						0,
						MemorySize::Unknown,
						OpAccess::Read,
					);
				}
			}

			CodeInfo::Loadall386 => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::ES, OpAccess::Read);
					Self::add_register(flags, info, Register::EDI, OpAccess::Read);
				}
			}

			CodeInfo::Xbts => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					code = instruction.code();
					if code == Code::Xbts_r32_rm32 || code == Code::Ibts_rm32_r32 {
						Self::add_register(flags, info, Register::EAX, OpAccess::Read);
					} else {
						debug_assert!(code == Code::Xbts_r16_rm16 || code == Code::Ibts_rm16_r16);
						Self::add_register(flags, info, Register::AX, OpAccess::Read);
					}
					Self::add_register(flags, info, Register::CL, OpAccess::Read);
				}
			}

			CodeInfo::Umonitor => {
				base_register = instruction.segment_prefix();
				if base_register == Register::None {
					base_register = Register::DS;
				}
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_memory_segment_register(flags, info, base_register, OpAccess::Read);
				}
				if (flags & Flags::NO_MEMORY_USAGE) == 0 {
					Self::add_memory(
						info,
						base_register,
						instruction.op0_register(),
						Register::None,
						1,
						0,
						MemorySize::UInt8,
						OpAccess::Read,
					);
				}
			}

			CodeInfo::Movdir64b => {
				if (flags & Flags::IS_64BIT) == 0 && (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::ES, OpAccess::Read);
				}
				if (flags & Flags::NO_MEMORY_USAGE) == 0 {
					Self::add_memory(
						info,
						Register::ES,
						instruction.op0_register(),
						Register::None,
						1,
						0,
						MemorySize::UInt512,
						OpAccess::Write,
					);
				}
			}

			CodeInfo::Clear_rflags => {
				if instruction.op0_register() == instruction.op1_register()
					&& instruction.op0_kind() == OpKind::Register
					&& instruction.op1_kind() == OpKind::Register
				{
					info.op_accesses[0] = OpAccess::Write;
					info.op_accesses[1] = OpAccess::None;
					info.rflags_info = RflagsInfo::C_cos_S_pz_U_a as usize;
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						debug_assert!(info.used_registers.len() == 2 || info.used_registers.len() == 3);
						info.used_registers.clear();
						Self::add_register(flags, info, instruction.op0_register(), OpAccess::Write);
					}
				}
			}

			CodeInfo::Clear_reg_regmem => {
				if instruction.op0_register() == instruction.op1_register() && instruction.op1_kind() == OpKind::Register {
					info.op_accesses[0] = OpAccess::Write;
					info.op_accesses[1] = OpAccess::None;
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						debug_assert!(info.used_registers.len() == 2 || info.used_registers.len() == 3);
						info.used_registers.clear();
						info.used_registers.push(UsedRegister {
							register: instruction.op0_register(),
							access: OpAccess::Write,
						});
					}
				}
			}

			CodeInfo::Clear_reg_reg_regmem => {
				if instruction.op1_register() == instruction.op2_register() && instruction.op2_kind() == OpKind::Register {
					info.op_accesses[1] = OpAccess::None;
					info.op_accesses[2] = OpAccess::None;
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						debug_assert!(info.used_registers.len() == 3 || info.used_registers.len() == 4);
						debug_assert_eq!(instruction.op1_register(), info.used_registers[info.used_registers.len() - 2].register);
						debug_assert_eq!(instruction.op2_register(), info.used_registers[info.used_registers.len() - 1].register);
						let new_size_tmp = info.used_registers.len() - 2;
						info.used_registers.truncate(new_size_tmp);
					}
				}
			}

			CodeInfo::Montmul => {
				const_assert_eq!(Code::Montmul_32 as u32, Code::Montmul_16 as u32 + 1);
				const_assert_eq!(Code::Montmul_64 as u32, Code::Montmul_16 as u32 + 2);
				const_assert_eq!(Register::EAX as u32, Register::AX as u32 + 16);
				const_assert_eq!(Register::RAX as u32, Register::AX as u32 + 32);
				if super::super::instruction_internal::internal_has_repe_or_repne_prefix(instruction) {
					reg_index = (instruction.code() as u32).wrapping_sub(Code::Montmul_16 as u32) << 4;
					if (flags & Flags::NO_MEMORY_USAGE) == 0 {
						Self::add_memory(
							info,
							Register::ES,
							unsafe { mem::transmute((Register::SI as u32).wrapping_add(reg_index) as u8) },
							Register::None,
							1,
							0,
							MemorySize::Unknown,
							OpAccess::CondRead,
						);
					}
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						debug_assert_eq!(0, info.used_registers.len());
						Self::add_register(
							flags,
							info,
							if reg_index == 0 {
								Register::ECX
							} else {
								unsafe { mem::transmute((Register::CX as u32).wrapping_add(reg_index) as u8) }
							},
							OpAccess::ReadCondWrite,
						);
						if (flags & Flags::IS_64BIT) == 0 {
							Self::add_register(flags, info, Register::ES, OpAccess::CondRead);
						}
						Self::add_register(
							flags,
							info,
							unsafe { mem::transmute((Register::SI as u32).wrapping_add(reg_index) as u8) },
							OpAccess::CondRead,
						);
						Self::add_register(flags, info, Register::EAX, OpAccess::CondRead);
						Self::add_register(flags, info, Register::EAX, OpAccess::CondWrite);
						Self::add_register(flags, info, Register::EDX, OpAccess::CondWrite);
					}
				} else {
					reg_index = (instruction.code() as u32).wrapping_sub(Code::Montmul_16 as u32) << 4;
					if (flags & Flags::NO_MEMORY_USAGE) == 0 {
						Self::add_memory(
							info,
							Register::ES,
							unsafe { mem::transmute((Register::SI as u32).wrapping_add(reg_index) as u8) },
							Register::None,
							1,
							0,
							instruction.memory_size(),
							OpAccess::Read,
						);
					}
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						if (flags & Flags::IS_64BIT) == 0 {
							Self::add_register(flags, info, Register::ES, OpAccess::Read);
						}
						Self::add_register(
							flags,
							info,
							unsafe { mem::transmute((Register::SI as u32).wrapping_add(reg_index) as u8) },
							OpAccess::Read,
						);
						Self::add_register(flags, info, Register::EAX, OpAccess::ReadWrite);
						Self::add_register(flags, info, Register::EDX, OpAccess::Write);
					}
				}
			}

			CodeInfo::Xsha => {
				const_assert_eq!(Code::Xsha1_32 as u32, Code::Xsha1_16 as u32 + 1);
				const_assert_eq!(Code::Xsha1_64 as u32, Code::Xsha1_16 as u32 + 2);
				const_assert_eq!(Code::Xsha256_32 as u32, Code::Xsha256_16 as u32 + 1);
				const_assert_eq!(Code::Xsha256_64 as u32, Code::Xsha256_16 as u32 + 2);
				const_assert_eq!(0, (Code::Xsha256_16 as u32 - Code::Xsha1_16 as u32) % 3);
				const_assert_eq!(Register::EAX as u32, Register::AX as u32 + 16);
				const_assert_eq!(Register::RAX as u32, Register::AX as u32 + 32);
				if super::super::instruction_internal::internal_has_repe_or_repne_prefix(instruction) {
					reg_index = ((instruction.code() as u32).wrapping_sub(Code::Xsha1_16 as u32) % 3) << 4;
					if (flags & Flags::NO_MEMORY_USAGE) == 0 {
						Self::add_memory(
							info,
							Register::ES,
							unsafe { mem::transmute((Register::SI as u32).wrapping_add(reg_index) as u8) },
							Register::None,
							1,
							0,
							MemorySize::Unknown,
							OpAccess::CondRead,
						);
						Self::add_memory(
							info,
							Register::ES,
							unsafe { mem::transmute((Register::DI as u32).wrapping_add(reg_index) as u8) },
							Register::None,
							1,
							0,
							MemorySize::Unknown,
							OpAccess::CondRead,
						);
						Self::add_memory(
							info,
							Register::ES,
							unsafe { mem::transmute((Register::DI as u32).wrapping_add(reg_index) as u8) },
							Register::None,
							1,
							0,
							MemorySize::Unknown,
							OpAccess::CondWrite,
						);
					}
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						debug_assert_eq!(0, info.used_registers.len());
						Self::add_register(
							flags,
							info,
							unsafe { mem::transmute((Register::CX as u32).wrapping_add(reg_index) as u8) },
							OpAccess::ReadCondWrite,
						);
						if (flags & Flags::IS_64BIT) == 0 {
							Self::add_register(flags, info, Register::ES, OpAccess::CondRead);
						}
						Self::add_register(
							flags,
							info,
							unsafe { mem::transmute((Register::SI as u32).wrapping_add(reg_index) as u8) },
							OpAccess::CondRead,
						);
						Self::add_register(
							flags,
							info,
							unsafe { mem::transmute((Register::SI as u32).wrapping_add(reg_index) as u8) },
							OpAccess::CondWrite,
						);
						Self::add_register(
							flags,
							info,
							unsafe { mem::transmute((Register::DI as u32).wrapping_add(reg_index) as u8) },
							OpAccess::CondRead,
						);
						Self::add_register(
							flags,
							info,
							unsafe { mem::transmute((Register::DI as u32).wrapping_add(reg_index) as u8) },
							OpAccess::CondWrite,
						);
						Self::add_register(
							flags,
							info,
							unsafe { mem::transmute((Register::AX as u32).wrapping_add(reg_index) as u8) },
							OpAccess::CondRead,
						);
						Self::add_register(
							flags,
							info,
							unsafe { mem::transmute((Register::AX as u32).wrapping_add(reg_index) as u8) },
							OpAccess::CondWrite,
						);
					}
				} else {
					reg_index = ((instruction.code() as u32).wrapping_sub(Code::Xsha1_16 as u32) % 3) << 4;
					if (flags & Flags::NO_MEMORY_USAGE) == 0 {
						Self::add_memory(
							info,
							Register::ES,
							unsafe { mem::transmute((Register::SI as u32).wrapping_add(reg_index) as u8) },
							Register::None,
							1,
							0,
							instruction.memory_size(),
							OpAccess::Read,
						);
						Self::add_memory(
							info,
							Register::ES,
							unsafe { mem::transmute((Register::DI as u32).wrapping_add(reg_index) as u8) },
							Register::None,
							1,
							0,
							instruction.memory_size(),
							OpAccess::ReadWrite,
						);
					}
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						if (flags & Flags::IS_64BIT) == 0 {
							Self::add_register(flags, info, Register::ES, OpAccess::Read);
						}
						Self::add_register(
							flags,
							info,
							unsafe { mem::transmute((Register::SI as u32).wrapping_add(reg_index) as u8) },
							OpAccess::ReadWrite,
						);
						Self::add_register(
							flags,
							info,
							unsafe { mem::transmute((Register::DI as u32).wrapping_add(reg_index) as u8) },
							OpAccess::ReadWrite,
						);
					}
				}
			}

			CodeInfo::Xcrypt => {
				const_assert_eq!(Code::XcryptEcb_32 as u32, Code::XcryptEcb_16 as u32 + 1);
				const_assert_eq!(Code::XcryptEcb_64 as u32, Code::XcryptEcb_16 as u32 + 2);
				const_assert_eq!(Code::XcryptCbc_32 as u32, Code::XcryptCbc_16 as u32 + 1);
				const_assert_eq!(Code::XcryptCbc_64 as u32, Code::XcryptCbc_16 as u32 + 2);
				const_assert_eq!(Code::XcryptCtr_32 as u32, Code::XcryptCtr_16 as u32 + 1);
				const_assert_eq!(Code::XcryptCtr_64 as u32, Code::XcryptCtr_16 as u32 + 2);
				const_assert_eq!(Code::XcryptCfb_32 as u32, Code::XcryptCfb_16 as u32 + 1);
				const_assert_eq!(Code::XcryptCfb_64 as u32, Code::XcryptCfb_16 as u32 + 2);
				const_assert_eq!(Code::XcryptOfb_32 as u32, Code::XcryptOfb_16 as u32 + 1);
				const_assert_eq!(Code::XcryptOfb_64 as u32, Code::XcryptOfb_16 as u32 + 2);
				const_assert_eq!(0, (Code::XcryptCbc_16 as u32 - Code::XcryptEcb_16 as u32) % 3);
				const_assert_eq!(0, (Code::XcryptCtr_16 as u32 - Code::XcryptEcb_16 as u32) % 3);
				const_assert_eq!(0, (Code::XcryptCfb_16 as u32 - Code::XcryptEcb_16 as u32) % 3);
				const_assert_eq!(0, (Code::XcryptOfb_16 as u32 - Code::XcryptEcb_16 as u32) % 3);
				const_assert_eq!(Register::EAX as u32, Register::AX as u32 + 16);
				const_assert_eq!(Register::RAX as u32, Register::AX as u32 + 32);
				if super::super::instruction_internal::internal_has_repe_or_repne_prefix(instruction) {
					const_assert_eq!(OpKind::MemoryESEDI as u32, OpKind::MemoryESDI as u32 + 1);
					const_assert_eq!(OpKind::MemoryESRDI as u32, OpKind::MemoryESDI as u32 + 2);
					const_assert_eq!(Register::EDI as u32, Register::DI as u32 + 16);
					const_assert_eq!(Register::RDI as u32, Register::DI as u32 + 32);
					const_assert_eq!(Register::ECX as u32, Register::CX as u32 + 16);
					const_assert_eq!(Register::RCX as u32, Register::CX as u32 + 32);
					reg_index = ((instruction.code() as u32).wrapping_sub(Code::XcryptEcb_16 as u32) % 3) << 4;
					if (flags & Flags::NO_MEMORY_USAGE) == 0 {
						// Check if not XcryptEcb
						if instruction.code() >= Code::XcryptCbc_16 {
							Self::add_memory(
								info,
								Register::ES,
								unsafe { mem::transmute((Register::AX as u32).wrapping_add(reg_index) as u8) },
								Register::None,
								1,
								0,
								MemorySize::Unknown,
								OpAccess::CondRead,
							);
							Self::add_memory(
								info,
								Register::ES,
								unsafe { mem::transmute((Register::AX as u32).wrapping_add(reg_index) as u8) },
								Register::None,
								1,
								0,
								MemorySize::Unknown,
								OpAccess::CondWrite,
							);
						}
						Self::add_memory(
							info,
							Register::ES,
							unsafe { mem::transmute((Register::DX as u32).wrapping_add(reg_index) as u8) },
							Register::None,
							1,
							0,
							MemorySize::Unknown,
							OpAccess::CondRead,
						);
						Self::add_memory(
							info,
							Register::ES,
							unsafe { mem::transmute((Register::BX as u32).wrapping_add(reg_index) as u8) },
							Register::None,
							1,
							0,
							MemorySize::Unknown,
							OpAccess::CondRead,
						);
						Self::add_memory(
							info,
							Register::ES,
							unsafe { mem::transmute((Register::SI as u32).wrapping_add(reg_index) as u8) },
							Register::None,
							1,
							0,
							MemorySize::Unknown,
							OpAccess::CondRead,
						);
						Self::add_memory(
							info,
							Register::ES,
							unsafe { mem::transmute((Register::DI as u32).wrapping_add(reg_index) as u8) },
							Register::None,
							1,
							0,
							MemorySize::Unknown,
							OpAccess::CondWrite,
						);
					}
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						debug_assert_eq!(0, info.used_registers.len());
						Self::add_register(
							flags,
							info,
							unsafe { mem::transmute((Register::CX as u32).wrapping_add(reg_index) as u8) },
							OpAccess::ReadCondWrite,
						);
						if (flags & Flags::IS_64BIT) == 0 {
							Self::add_register(flags, info, Register::ES, OpAccess::CondRead);
						}
						// Check if not XcryptEcb
						if instruction.code() >= Code::XcryptCbc_16 {
							Self::add_register(
								flags,
								info,
								unsafe { mem::transmute((Register::AX as u32).wrapping_add(reg_index) as u8) },
								OpAccess::CondRead,
							);
							Self::add_register(
								flags,
								info,
								unsafe { mem::transmute((Register::AX as u32).wrapping_add(reg_index) as u8) },
								OpAccess::CondWrite,
							);
						}
						Self::add_register(
							flags,
							info,
							unsafe { mem::transmute((Register::DX as u32).wrapping_add(reg_index) as u8) },
							OpAccess::CondRead,
						);
						Self::add_register(
							flags,
							info,
							unsafe { mem::transmute((Register::BX as u32).wrapping_add(reg_index) as u8) },
							OpAccess::CondRead,
						);
						Self::add_register(
							flags,
							info,
							unsafe { mem::transmute((Register::SI as u32).wrapping_add(reg_index) as u8) },
							OpAccess::CondRead,
						);
						Self::add_register(
							flags,
							info,
							unsafe { mem::transmute((Register::SI as u32).wrapping_add(reg_index) as u8) },
							OpAccess::CondWrite,
						);
						Self::add_register(
							flags,
							info,
							unsafe { mem::transmute((Register::DI as u32).wrapping_add(reg_index) as u8) },
							OpAccess::CondRead,
						);
						Self::add_register(
							flags,
							info,
							unsafe { mem::transmute((Register::DI as u32).wrapping_add(reg_index) as u8) },
							OpAccess::CondWrite,
						);
					}
				} else {
					const_assert_eq!(OpKind::MemoryESEDI as u32, OpKind::MemoryESDI as u32 + 1);
					const_assert_eq!(OpKind::MemoryESRDI as u32, OpKind::MemoryESDI as u32 + 2);
					const_assert_eq!(Register::EDI as u32, Register::DI as u32 + 16);
					const_assert_eq!(Register::RDI as u32, Register::DI as u32 + 32);
					reg_index = ((instruction.code() as u32).wrapping_sub(Code::XcryptEcb_16 as u32) % 3) << 4;
					if (flags & Flags::NO_MEMORY_USAGE) == 0 {
						// Check if not XcryptEcb
						if instruction.code() >= Code::XcryptCbc_16 {
							Self::add_memory(
								info,
								Register::ES,
								unsafe { mem::transmute((Register::AX as u32).wrapping_add(reg_index) as u8) },
								Register::None,
								1,
								0,
								instruction.memory_size(),
								OpAccess::ReadWrite,
							);
						}
						Self::add_memory(
							info,
							Register::ES,
							unsafe { mem::transmute((Register::DX as u32).wrapping_add(reg_index) as u8) },
							Register::None,
							1,
							0,
							instruction.memory_size(),
							OpAccess::Read,
						);
						Self::add_memory(
							info,
							Register::ES,
							unsafe { mem::transmute((Register::BX as u32).wrapping_add(reg_index) as u8) },
							Register::None,
							1,
							0,
							instruction.memory_size(),
							OpAccess::Read,
						);
						Self::add_memory(
							info,
							Register::ES,
							unsafe { mem::transmute((Register::SI as u32).wrapping_add(reg_index) as u8) },
							Register::None,
							1,
							0,
							instruction.memory_size(),
							OpAccess::Read,
						);
						Self::add_memory(
							info,
							Register::ES,
							unsafe { mem::transmute((Register::DI as u32).wrapping_add(reg_index) as u8) },
							Register::None,
							1,
							0,
							instruction.memory_size(),
							OpAccess::Write,
						);
					}
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						if (flags & Flags::IS_64BIT) == 0 {
							Self::add_register(flags, info, Register::ES, OpAccess::Read);
						}
						// Check if not XcryptEcb
						if instruction.code() >= Code::XcryptCbc_16 {
							Self::add_register(
								flags,
								info,
								unsafe { mem::transmute((Register::AX as u32).wrapping_add(reg_index) as u8) },
								OpAccess::ReadWrite,
							);
						}
						Self::add_register(
							flags,
							info,
							unsafe { mem::transmute((Register::DX as u32).wrapping_add(reg_index) as u8) },
							OpAccess::Read,
						);
						Self::add_register(
							flags,
							info,
							unsafe { mem::transmute((Register::BX as u32).wrapping_add(reg_index) as u8) },
							OpAccess::Read,
						);
						Self::add_register(
							flags,
							info,
							unsafe { mem::transmute((Register::SI as u32).wrapping_add(reg_index) as u8) },
							OpAccess::ReadWrite,
						);
						Self::add_register(
							flags,
							info,
							unsafe { mem::transmute((Register::DI as u32).wrapping_add(reg_index) as u8) },
							OpAccess::ReadWrite,
						);
					}
				}
			}

			CodeInfo::Xstore => {
				const_assert_eq!(Code::Xstore_32 as u32, Code::Xstore_16 as u32 + 1);
				const_assert_eq!(Code::Xstore_64 as u32, Code::Xstore_16 as u32 + 2);
				const_assert_eq!(Register::EAX as u32, Register::AX as u32 + 16);
				const_assert_eq!(Register::RAX as u32, Register::AX as u32 + 32);
				if super::super::instruction_internal::internal_has_repe_or_repne_prefix(instruction) {
					reg_index = (instruction.code() as u32).wrapping_sub(Code::Xstore_16 as u32) << 4;
					if (flags & Flags::NO_MEMORY_USAGE) == 0 {
						Self::add_memory(
							info,
							Register::ES,
							unsafe { mem::transmute((Register::DI as u32).wrapping_add(reg_index) as u8) },
							Register::None,
							1,
							0,
							MemorySize::Unknown,
							OpAccess::CondWrite,
						);
					}
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						debug_assert_eq!(0, info.used_registers.len());
						Self::add_register(
							flags,
							info,
							unsafe { mem::transmute((Register::CX as u32).wrapping_add(reg_index) as u8) },
							OpAccess::ReadCondWrite,
						);
						if (flags & Flags::IS_64BIT) == 0 {
							Self::add_register(flags, info, Register::ES, OpAccess::CondRead);
						}
						Self::add_register(
							flags,
							info,
							unsafe { mem::transmute((Register::DI as u32).wrapping_add(reg_index) as u8) },
							OpAccess::CondRead,
						);
						Self::add_register(
							flags,
							info,
							unsafe { mem::transmute((Register::DI as u32).wrapping_add(reg_index) as u8) },
							OpAccess::CondWrite,
						);
						Self::add_register(flags, info, Register::EAX, OpAccess::CondWrite);
						Self::add_register(flags, info, Register::EDX, OpAccess::CondRead);
					}
				} else {
					reg_index = (instruction.code() as u32).wrapping_sub(Code::Xstore_16 as u32) << 4;
					if (flags & Flags::NO_MEMORY_USAGE) == 0 {
						Self::add_memory(
							info,
							Register::ES,
							unsafe { mem::transmute((Register::DI as u32).wrapping_add(reg_index) as u8) },
							Register::None,
							1,
							0,
							instruction.memory_size(),
							OpAccess::Write,
						);
					}
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						if (flags & Flags::IS_64BIT) == 0 {
							Self::add_register(flags, info, Register::ES, OpAccess::Read);
						}
						Self::add_register(
							flags,
							info,
							unsafe { mem::transmute((Register::DI as u32).wrapping_add(reg_index) as u8) },
							OpAccess::ReadWrite,
						);
						Self::add_register(flags, info, Register::EAX, OpAccess::Write);
						Self::add_register(flags, info, Register::EDX, OpAccess::Read);
					}
				}
			}

			CodeInfo::KP1 => {
				debug_assert!(Register::K0 <= instruction.op0_register() && instruction.op0_register() <= Register::K7);
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					const_assert_eq!(1, (Register::K0 as u32 & 1));
					if (instruction.op0_register() as u8 & 1) != 0 {
						Self::add_register(
							flags,
							info,
							unsafe { mem::transmute((instruction.op0_register() as u32).wrapping_add(1) as u8) },
							OpAccess::Write,
						);
					} else {
						Self::add_register(
							flags,
							info,
							unsafe { mem::transmute((instruction.op0_register() as u32).wrapping_sub(1) as u8) },
							OpAccess::Write,
						);
					}
				}
			}

			CodeInfo::Read_Reg8_Op0 => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					if instruction.op0_kind() == OpKind::Register {
						debug_assert!(info.used_registers.len() >= 1);
						debug_assert_eq!(instruction.op0_register(), info.used_registers[0].register);
						index = Self::try_get_gpr_16_32_64_index(instruction.op0_register());
						if index >= 4 {
							index += 4; // Skip AH, CH, DH, BH
						}
						if index >= 0 {
							info.used_registers[0] = UsedRegister {
								register: unsafe { mem::transmute((Register::AL as u32).wrapping_add(index as u32) as u8) },
								access: OpAccess::Read,
							};
						}
					}
				}
			}

			CodeInfo::Read_Reg8_Op1 => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					if instruction.op1_kind() == OpKind::Register {
						debug_assert!(info.used_registers.len() >= 2);
						debug_assert_eq!(instruction.op1_register(), info.used_registers[1].register);
						index = Self::try_get_gpr_16_32_64_index(instruction.op1_register());
						if index >= 4 {
							index += 4; // Skip AH, CH, DH, BH
						}
						if index >= 0 {
							info.used_registers[1] = UsedRegister {
								register: unsafe { mem::transmute((Register::AL as u32).wrapping_add(index as u32) as u8) },
								access: OpAccess::Read,
							};
						}
					}
				}
			}

			CodeInfo::Read_Reg8_Op2 => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					if instruction.op2_kind() == OpKind::Register {
						debug_assert!(info.used_registers.len() >= 3);
						debug_assert_eq!(instruction.op2_register(), info.used_registers[2].register);
						index = Self::try_get_gpr_16_32_64_index(instruction.op2_register());
						if index >= 4 {
							index += 4; // Skip AH, CH, DH, BH
						}
						if index >= 0 {
							info.used_registers[2] = UsedRegister {
								register: unsafe { mem::transmute((Register::AL as u32).wrapping_add(index as u32) as u8) },
								access: OpAccess::Read,
							};
						}
					}
				}
			}

			CodeInfo::Read_Reg16_Op0 => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					if instruction.op0_kind() == OpKind::Register {
						debug_assert!(info.used_registers.len() >= 1);
						debug_assert_eq!(instruction.op0_register(), info.used_registers[0].register);
						index = Self::try_get_gpr_16_32_64_index(instruction.op0_register());
						if index >= 0 {
							info.used_registers[0] = UsedRegister {
								register: unsafe { mem::transmute((Register::AX as u32).wrapping_add(index as u32) as u8) },
								access: OpAccess::Read,
							};
						}
					}
				}
			}

			CodeInfo::Read_Reg16_Op1 => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					if instruction.op1_kind() == OpKind::Register {
						debug_assert!(info.used_registers.len() >= 2);
						debug_assert_eq!(instruction.op1_register(), info.used_registers[1].register);
						index = Self::try_get_gpr_16_32_64_index(instruction.op1_register());
						if index >= 0 {
							info.used_registers[1] = UsedRegister {
								register: unsafe { mem::transmute((Register::AX as u32).wrapping_add(index as u32) as u8) },
								access: OpAccess::Read,
							};
						}
					}
				}
			}

			CodeInfo::Read_Reg16_Op2 => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					if instruction.op2_kind() == OpKind::Register {
						debug_assert!(info.used_registers.len() >= 3);
						debug_assert_eq!(instruction.op2_register(), info.used_registers[2].register);
						index = Self::try_get_gpr_16_32_64_index(instruction.op2_register());
						if index >= 0 {
							info.used_registers[2] = UsedRegister {
								register: unsafe { mem::transmute((Register::AX as u32).wrapping_add(index as u32) as u8) },
								access: OpAccess::Read,
							};
						}
					}
				}
			}

			CodeInfo::None => {}
		}
	}

	#[cfg_attr(has_must_use, must_use)]
	fn try_get_gpr_16_32_64_index(register: Register) -> i32 {
		let mut index;
		let reg = register as u32;
		index = reg.wrapping_sub(Register::EAX as u32);
		if index <= 15 {
			return index as i32;
		}
		index = reg.wrapping_sub(Register::RAX as u32);
		if index <= 15 {
			return index as i32;
		}
		index = reg.wrapping_sub(Register::AX as u32);
		if index <= 15 {
			index as i32
		} else {
			-1
		}
	}

	#[cfg_attr(feature = "cargo-clippy", allow(clippy::too_many_arguments))]
	#[inline(always)]
	fn add_memory(
		info: &mut InstructionInfo, segment_register: Register, base_register: Register, index_register: Register, scale: u32, displ: u64,
		memory_size: MemorySize, access: OpAccess,
	) {
		if access != OpAccess::NoMemAccess {
			info.used_memory_locations.push(UsedMemory {
				displacement: displ,
				segment: segment_register,
				base: base_register,
				index: index_register,
				scale: scale as u8,
				memory_size,
				access,
				_pad: 0,
			});
		}
	}

	#[inline(always)]
	fn add_memory_segment_register(flags: u32, info: &mut InstructionInfo, seg: Register, access: OpAccess) {
		debug_assert!(Register::ES <= seg && seg <= Register::GS);
		// Ignore es,cs,ss,ds memory operand segment registers in 64-bit mode
		if (flags & Flags::IS_64BIT) == 0 || seg >= Register::FS {
			Self::add_register(flags, info, seg, access);
		}
	}

	fn add_register(flags: u32, info: &mut InstructionInfo, mut reg: Register, access: OpAccess) {
		debug_assert!((flags & Flags::NO_REGISTER_USAGE) == 0);

		let mut write_reg = reg;
		if (flags & (Flags::IS_64BIT | Flags::ZERO_EXT_VEC_REGS)) != 0 {
			const_assert_eq!(OpAccess::CondWrite as u32, OpAccess::Write as u32 + 1);
			const_assert_eq!(OpAccess::ReadWrite as u32, OpAccess::Write as u32 + 2);
			const_assert_eq!(OpAccess::ReadCondWrite as u32, OpAccess::Write as u32 + 3);
			if (access as u32).wrapping_sub(OpAccess::Write as u32) <= 3 {
				const_assert_eq!(Register::ZMM0 as u32, IcedConstants::VMM_FIRST as u32);
				const_assert!((IcedConstants::VMM_COUNT & (IcedConstants::VMM_COUNT - 1)) == 0); // Verify that it's a power of 2
				let mut index = (reg as u32).wrapping_sub(Register::EAX as u32);
				if (flags & Flags::IS_64BIT) != 0 && index <= (Register::R15D as u32 - Register::EAX as u32) {
					write_reg = unsafe { mem::transmute((Register::RAX as u32).wrapping_add(index) as u8) };
				} else {
					index = (reg as u32).wrapping_sub(Register::XMM0 as u32);
					if (flags & Flags::ZERO_EXT_VEC_REGS) != 0 && index <= IcedConstants::VMM_LAST as u32 - Register::XMM0 as u32 {
						write_reg = unsafe { mem::transmute((Register::ZMM0 as u32).wrapping_add(index & (IcedConstants::VMM_COUNT - 1)) as u8) };
					}
				}
				if access != OpAccess::ReadWrite && access != OpAccess::ReadCondWrite {
					reg = write_reg;
				}
			}
		}

		if write_reg == reg {
			info.used_registers.push(UsedRegister { register: reg, access });
		} else {
			debug_assert!(access == OpAccess::ReadWrite || access == OpAccess::ReadCondWrite);
			info.used_registers.push(UsedRegister {
				register: reg,
				access: OpAccess::Read,
			});
			info.used_registers.push(UsedRegister {
				register: write_reg,
				access: if access == OpAccess::ReadWrite {
					OpAccess::Write
				} else {
					OpAccess::CondWrite
				},
			});
		}
	}
}
