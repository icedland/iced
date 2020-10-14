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
use core::{mem, u16, u32, u64};

/// Instruction info options used by [`InstructionInfoFactory`]
///
/// [`InstructionInfoFactory`]: struct.InstructionInfoFactory.html
#[allow(missing_copy_implementations)]
#[allow(missing_debug_implementations)]
pub struct InstructionInfoOptions;
impl InstructionInfoOptions {
	/// No option is enabled
	pub const NONE: u32 = 0;
	/// Don't include memory usage, i.e., [`InstructionInfo::used_memory()`] will return an empty vector. All
	/// registers that are used by memory operands are still returned by [`InstructionInfo::used_registers()`].
	///
	/// [`InstructionInfo::used_memory()`]: struct.InstructionInfo.html#method.used_memory
	/// [`InstructionInfo::used_registers()`]: struct.InstructionInfo.html#method.used_registers
	pub const NO_MEMORY_USAGE: u32 = 0x0000_0001;
	/// Don't include register usage, i.e., [`InstructionInfo::used_registers()`] will return an empty vector
	///
	/// [`InstructionInfo::used_registers()`]: struct.InstructionInfo.html#method.used_registers
	pub const NO_REGISTER_USAGE: u32 = 0x0000_0002;
}

struct Flags;
impl Flags {
	pub const NO_MEMORY_USAGE: u32 = 0x0000_0001;
	pub const NO_REGISTER_USAGE: u32 = 0x0000_0002;
	pub const IS_64BIT: u32 = 0x0000_0004;
	pub const ZERO_EXT_VEC_REGS: u32 = 0x0000_0008;
}

/// Creates [`InstructionInfo`]s.
///
/// If you don't need to know register and memory usage, it's faster to call [`Instruction`] and
/// [`Code`] methods such as [`Instruction::flow_control()`] instead of getting that info from this struct.
///
/// [`InstructionInfo`]: struct.InstructionInfo.html
/// [`Instruction`]: struct.Instruction.html
/// [`Code`]: enum.Code.html
/// [`Instruction::flow_control()`]: struct.Instruction.html#method.flow_control
#[derive(Debug)]
pub struct InstructionInfoFactory {
	info: InstructionInfo,
}

#[cfg_attr(feature = "cargo-clippy", allow(clippy::new_without_default))]
impl InstructionInfoFactory {
	/// Creates a new instance.
	///
	/// If you don't need to know register and memory usage, it's faster to call [`Instruction`] and
	/// [`Code`] methods such as [`Instruction::flow_control()`] instead of getting that info from this struct.
	///
	/// [`Instruction`]: struct.Instruction.html
	/// [`Code`]: enum.Code.html
	/// [`Instruction::flow_control()`]: struct.Instruction.html#method.flow_control
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
		Self { info: InstructionInfo::new(0) }
	}

	/// Creates a new [`InstructionInfo`], see also [`info_options()`] if you only need register usage
	/// but not memory usage or vice versa.
	///
	/// If you don't need to know register and memory usage, it's faster to call [`Instruction`] and
	/// [`Code`] methods such as [`Instruction::flow_control()`] instead of getting that info from this struct.
	///
	/// [`InstructionInfo`]: struct.InstructionInfo.html
	/// [`info_options()`]: #method.info_options
	/// [`Instruction`]: struct.Instruction.html
	/// [`Code`]: enum.Code.html
	/// [`Instruction::flow_control()`]: struct.Instruction.html#method.flow_control
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
	pub fn info(&mut self, instruction: &Instruction) -> &InstructionInfo {
		Self::create(&mut self.info, instruction, InstructionInfoOptions::NONE)
	}

	/// Creates a new [`InstructionInfo`], see also [`info()`].
	///
	/// If you don't need to know register and memory usage, it's faster to call [`Instruction`] and
	/// [`Code`] methods such as [`Instruction::flow_control()`] instead of getting that info from this struct.
	///
	/// [`InstructionInfo`]: struct.InstructionInfo.html
	/// [`info()`]: #method.info
	/// [`Instruction`]: struct.Instruction.html
	/// [`Code`]: enum.Code.html
	/// [`Instruction::flow_control()`]: struct.Instruction.html#method.flow_control
	///
	/// # Arguments
	///
	/// * `instruction`: The instruction that should be analyzed
	/// * `options`: Options, see [`InstructionInfoOptions`]
	///
	/// [`InstructionInfoOptions`]: struct.InstructionInfoOptions.html
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn info_options(&mut self, instruction: &Instruction, options: u32) -> &InstructionInfo {
		Self::create(&mut self.info, instruction, options)
	}

	fn create<'a>(info: &'a mut InstructionInfo, instruction: &Instruction, options: u32) -> &'a InstructionInfo {
		info.used_registers.clear();
		info.used_memory_locations.clear();

		let index = (instruction.code() as usize) << 1;
		let flags1 = unsafe { *super::info_table::TABLE.get_unchecked(index) };
		let flags2 = unsafe { *super::info_table::TABLE.get_unchecked(index + 1) };

		info.cpuid_feature_internal = ((flags2 >> InfoFlags2::CPUID_FEATURE_INTERNAL_SHIFT) & InfoFlags2::CPUID_FEATURE_INTERNAL_MASK) as usize;
		info.flow_control = unsafe { mem::transmute(((flags2 >> InfoFlags2::FLOW_CONTROL_SHIFT) & InfoFlags2::FLOW_CONTROL_MASK) as u8) };
		info.encoding = unsafe { mem::transmute(((flags2 >> InfoFlags2::ENCODING_SHIFT) & InfoFlags2::ENCODING_MASK) as u8) };
		info.rflags_info = ((flags1 >> InfoFlags1::RFLAGS_INFO_SHIFT) & InfoFlags1::RFLAGS_INFO_MASK) as usize;

		const FLAGS_SHIFT: u32 = 12;
		const_assert_eq!(IIFlags::SAVE_RESTORE as u32, InfoFlags2::SAVE_RESTORE >> FLAGS_SHIFT);
		const_assert_eq!(IIFlags::STACK_INSTRUCTION as u32, InfoFlags2::STACK_INSTRUCTION >> FLAGS_SHIFT);
		const_assert_eq!(IIFlags::PRIVILEGED as u32, InfoFlags2::PRIVILEGED >> FLAGS_SHIFT);
		info.flags = (flags2 >> FLAGS_SHIFT) as u8;

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

		let op0_info = unsafe { mem::transmute(((flags1 >> InfoFlags1::OP_INFO0_SHIFT) & InfoFlags1::OP_INFO0_MASK) as u8) };
		let op0_access = match op0_info {
			OpInfo0::None => OpAccess::None,
			OpInfo0::Read => OpAccess::Read,

			OpInfo0::Write => {
				if instruction.has_op_mask() && instruction.merging_masking() {
					if instruction.op0_kind() != OpKind::Register {
						OpAccess::CondWrite
					} else {
						OpAccess::ReadWrite
					}
				} else {
					OpAccess::Write
				}
			}

			OpInfo0::WriteVmm => {
				// If it's opmask+merging ({k1}) and dest is xmm/ymm/zmm, then access is one of:
				//	k1			mem			xmm/ymm		zmm
				//	----------------------------------------
				//	all 1s		write		write		write		all bits are overwritten, upper bits in zmm (if xmm/ymm) are cleared
				//	all 0s		no access	read/write	no access	no elem is written, but xmm/ymm's upper bits (in zmm) are cleared so
				//													treat it as R lower bits + clear upper bits + W full reg
				//	else		cond-write	read/write	r-c-w		some elems are unchanged, the others are overwritten
				// If it's xmm/ymm, use RW, else use RCW. If it's mem, use CW
				if instruction.has_op_mask() && instruction.merging_masking() {
					if instruction.op0_kind() != OpKind::Register {
						OpAccess::CondWrite
					} else {
						OpAccess::ReadCondWrite
					}
				} else {
					OpAccess::Write
				}
			}

			OpInfo0::WriteForce | OpInfo0::WriteForceP1 => OpAccess::Write,
			OpInfo0::CondWrite => OpAccess::CondWrite,

			OpInfo0::CondWrite32_ReadWrite64 => {
				if (flags & Flags::IS_64BIT) != 0 {
					OpAccess::ReadWrite
				} else {
					OpAccess::CondWrite
				}
			}

			OpInfo0::ReadWrite => OpAccess::ReadWrite,

			OpInfo0::ReadWriteVmm => {
				// If it's opmask+merging ({k1}) and dest is xmm/ymm/zmm, then access is one of:
				//	k1			xmm/ymm		zmm
				//	-------------------------------
				//	all 1s		read/write	read/write	all bits are overwritten, upper bits in zmm (if xmm/ymm) are cleared
				//	all 0s		read/write	no access	no elem is written, but xmm/ymm's upper bits (in zmm) are cleared so
				//										treat it as R lower bits + clear upper bits + W full reg
				//	else		read/write	r-c-w		some elems are unchanged, the others are overwritten
				// If it's xmm/ymm, use RW, else use RCW
				if instruction.has_op_mask() && instruction.merging_masking() {
					OpAccess::ReadCondWrite
				} else {
					OpAccess::ReadWrite
				}
			}

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

		debug_assert!(instruction.op_count() as usize <= IcedConstants::MAX_OP_COUNT);
		info.op_accesses[0] = op0_access;
		let op1_info = ((flags1 >> InfoFlags1::OP_INFO1_SHIFT) & InfoFlags1::OP_INFO1_MASK) as usize;
		info.op_accesses[1] = unsafe { *OP_ACCESS_1.get_unchecked(op1_info) };
		info.op_accesses[2] = unsafe { *OP_ACCESS_2.get_unchecked(((flags1 >> InfoFlags1::OP_INFO2_SHIFT) & InfoFlags1::OP_INFO2_MASK) as usize) };
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
			let mut access = unsafe { *info.op_accesses.get_unchecked(i) };
			if access == OpAccess::None {
				continue;
			}

			match instruction.op_kind(i as u32) {
				OpKind::Register => {
					if access == OpAccess::NoMemAccess {
						access = OpAccess::Read;
						unsafe { *info.op_accesses.get_unchecked_mut(i) = OpAccess::Read };
					}
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						if i == 0 && op0_info == OpInfo0::WriteForceP1 {
							let reg = instruction.op0_register();
							debug_assert!(Register::K0 <= reg && reg <= Register::K7);
							Self::add_register(flags, info, reg, access);
							const_assert_eq!(1, (Register::K0 as u32 & 1));
							if (reg as u8 & 1) != 0 {
								Self::add_register(flags, info, unsafe { mem::transmute((reg as u32).wrapping_add(1) as u8) }, access);
							} else {
								Self::add_register(flags, info, unsafe { mem::transmute((reg as u32).wrapping_sub(1) as u8) }, access);
							}
						} else if i == 1 && op1_info == OpInfo1::ReadP3 as usize {
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
					const_assert_eq!(1 << 31, InfoFlags1::IGNORES_SEGMENT);
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
							let displ = if super::super::instruction_internal::get_address_size_in_bytes(
								base_register,
								index_register,
								instruction.memory_displ_size(),
								code_size,
							) == 8
							{
								instruction.memory_displacement64()
							} else {
								instruction.memory_displacement() as u64
							};
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

		let implied_access = unsafe { mem::transmute(((flags1 >> InfoFlags1::IMPLIED_ACCESS_SHIFT) & InfoFlags1::IMPLIED_ACCESS_MASK) as u8) };
		if implied_access != ImpliedAccess::None {
			Self::add_implied_accesses(implied_access, instruction, info, flags);
		}

		if instruction.has_op_mask() && (flags & Flags::NO_REGISTER_USAGE) == 0 {
			Self::add_register(
				flags,
				info,
				instruction.op_mask(),
				if (flags1 & InfoFlags1::OP_MASK_READ_WRITE) != 0 { OpAccess::ReadWrite } else { OpAccess::Read },
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

	#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
	fn add_implied_accesses(implied_access: ImpliedAccess, instruction: &Instruction, info: &mut InstructionInfo, flags: u32) {
		debug_assert_ne!(ImpliedAccess::None, implied_access);
		match implied_access {
			// GENERATOR-BEGIN: ImpliedAccessHandler
			// ⚠️This was generated by GENERATOR!🦹‍♂️
			ImpliedAccess::None => {
			}
			ImpliedAccess::Shift_Ib_MASK1FMOD9 => {
				Self::command_shift_mask_mod(instruction, info, 9);
			}
			ImpliedAccess::Shift_Ib_MASK1FMOD11 => {
				Self::command_shift_mask_mod(instruction, info, 17);
			}
			ImpliedAccess::Shift_Ib_MASK1F => {
				Self::command_shift_mask(instruction, info, 0x1F);
			}
			ImpliedAccess::Shift_Ib_MASK3F => {
				Self::command_shift_mask(instruction, info, 0x3F);
			}
			ImpliedAccess::Clear_rflags => {
				Self::command_clear_rflags(instruction, info, flags);
			}
			ImpliedAccess::t_push1x2 => {
				Self::command_push(instruction, info, flags, 1, 2);
			}
			ImpliedAccess::t_push1x4 => {
				Self::command_push(instruction, info, flags, 1, 4);
			}
			ImpliedAccess::t_pop1x2 => {
				Self::command_pop(instruction, info, flags, 1, 2);
			}
			ImpliedAccess::t_pop1x4 => {
				Self::command_pop(instruction, info, flags, 1, 4);
			}
			ImpliedAccess::t_RWal => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::AL, OpAccess::ReadWrite);
				}
			}
			ImpliedAccess::t_RWax => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::AX, OpAccess::ReadWrite);
				}
			}
			ImpliedAccess::t_push1x8 => {
				Self::command_push(instruction, info, flags, 1, 8);
			}
			ImpliedAccess::t_pop1x8 => {
				Self::command_pop(instruction, info, flags, 1, 8);
			}
			ImpliedAccess::t_pusha2 => {
				Self::command_pusha(instruction, info, flags, 2);
			}
			ImpliedAccess::t_pusha4 => {
				Self::command_pusha(instruction, info, flags, 4);
			}
			ImpliedAccess::t_popa2 => {
				Self::command_popa(instruction, info, flags, 2);
			}
			ImpliedAccess::t_popa4 => {
				Self::command_popa(instruction, info, flags, 4);
			}
			ImpliedAccess::t_arpl => {
				Self::command_arpl(instruction, info, flags);
			}
			ImpliedAccess::t_ins => {
				Self::command_ins(instruction, info, flags);
			}
			ImpliedAccess::t_outs => {
				Self::command_outs(instruction, info, flags);
			}
			ImpliedAccess::t_lea => {
				Self::command_lea(instruction, info, flags);
			}
			ImpliedAccess::t_gpr16 => {
				Self::command_last_gpr(instruction, info, flags, Register::AX);
			}
			ImpliedAccess::t_poprm2 => {
				Self::command_pop_rm(instruction, info, flags, 2);
			}
			ImpliedAccess::t_poprm4 => {
				Self::command_pop_rm(instruction, info, flags, 4);
			}
			ImpliedAccess::t_poprm8 => {
				Self::command_pop_rm(instruction, info, flags, 8);
			}
			ImpliedAccess::t_Ral_Wah => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::AL, OpAccess::Read);
					Self::add_register(flags, info, Register::AH, OpAccess::Write);
				}
			}
			ImpliedAccess::t_Rax_Weax => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::AX, OpAccess::Read);
					Self::add_register(flags, info, Register::EAX, OpAccess::Write);
				}
			}
			ImpliedAccess::t_RWeax => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::EAX, OpAccess::ReadWrite);
				}
			}
			ImpliedAccess::t_Rax_Wdx => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::AX, OpAccess::Read);
					Self::add_register(flags, info, Register::DX, OpAccess::Write);
				}
			}
			ImpliedAccess::t_Reax_Wedx => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::EAX, OpAccess::Read);
					Self::add_register(flags, info, Register::EDX, OpAccess::Write);
				}
			}
			ImpliedAccess::t_Rrax_Wrdx => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::RAX, OpAccess::Read);
					Self::add_register(flags, info, Register::RDX, OpAccess::Write);
				}
			}
			ImpliedAccess::t_push2x2 => {
				Self::command_push(instruction, info, flags, 2, 2);
			}
			ImpliedAccess::t_push2x4 => {
				Self::command_push(instruction, info, flags, 2, 4);
			}
			ImpliedAccess::t_Rah => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::AH, OpAccess::Read);
				}
			}
			ImpliedAccess::t_Wah => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::AH, OpAccess::Write);
				}
			}
			ImpliedAccess::t_movs => {
				Self::command_movs(instruction, info, flags);
			}
			ImpliedAccess::t_cmps => {
				Self::command_cmps(instruction, info, flags);
			}
			ImpliedAccess::t_stos => {
				Self::command_stos(instruction, info, flags);
			}
			ImpliedAccess::t_lods => {
				Self::command_lods(instruction, info, flags);
			}
			ImpliedAccess::t_scas => {
				Self::command_scas(instruction, info, flags);
			}
			ImpliedAccess::t_Wes => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::ES, OpAccess::Write);
				}
			}
			ImpliedAccess::t_Wds => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::DS, OpAccess::Write);
				}
			}
			ImpliedAccess::t_CWeax => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::EAX, OpAccess::CondWrite);
				}
			}
			ImpliedAccess::t_enter2 => {
				Self::command_enter(instruction, info, flags, 2);
			}
			ImpliedAccess::t_enter4 => {
				Self::command_enter(instruction, info, flags, 4);
			}
			ImpliedAccess::t_enter8 => {
				Self::command_enter(instruction, info, flags, 8);
			}
			ImpliedAccess::t_leave2 => {
				Self::command_leave(instruction, info, flags, 2);
			}
			ImpliedAccess::t_leave4 => {
				Self::command_leave(instruction, info, flags, 4);
			}
			ImpliedAccess::t_leave8 => {
				Self::command_leave(instruction, info, flags, 8);
			}
			ImpliedAccess::t_pop2x2 => {
				Self::command_pop(instruction, info, flags, 2, 2);
			}
			ImpliedAccess::t_pop2x4 => {
				Self::command_pop(instruction, info, flags, 2, 4);
			}
			ImpliedAccess::t_pop2x8 => {
				Self::command_pop(instruction, info, flags, 2, 8);
			}
			ImpliedAccess::b64_t_Wss_pop5x2_f_pop3x2 => {
				if (flags & Flags::IS_64BIT) != 0 {
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						Self::add_register(flags, info, Register::SS, OpAccess::Write);
					}
					Self::command_pop(instruction, info, flags, 5, 2);
				}
				else {
					Self::command_pop(instruction, info, flags, 3, 2);
				}
			}
			ImpliedAccess::b64_t_Wss_pop5x4_f_pop3x4 => {
				if (flags & Flags::IS_64BIT) != 0 {
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						Self::add_register(flags, info, Register::SS, OpAccess::Write);
					}
					Self::command_pop(instruction, info, flags, 5, 4);
				}
				else {
					Self::command_pop(instruction, info, flags, 3, 4);
				}
			}
			ImpliedAccess::t_Wss_pop5x8 => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::SS, OpAccess::Write);
				}
				Self::command_pop(instruction, info, flags, 5, 8);
			}
			ImpliedAccess::t_Ral_Wax => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::AL, OpAccess::Read);
					Self::add_register(flags, info, Register::AX, OpAccess::Write);
				}
			}
			ImpliedAccess::t_Wal => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::AL, OpAccess::Write);
				}
			}
			ImpliedAccess::t_RWst0 => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::ST0, OpAccess::ReadWrite);
				}
			}
			ImpliedAccess::t_Rst0 => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::ST0, OpAccess::Read);
				}
			}
			ImpliedAccess::t_Wst0 => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::ST0, OpAccess::Write);
				}
			}
			ImpliedAccess::t_Rst0_RWst1 => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::ST0, OpAccess::Read);
					Self::add_register(flags, info, Register::ST1, OpAccess::ReadWrite);
				}
			}
			ImpliedAccess::t_Rst1_RWst0 => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::ST1, OpAccess::Read);
					Self::add_register(flags, info, Register::ST0, OpAccess::ReadWrite);
				}
			}
			ImpliedAccess::t_Rst0_Rst1 => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::ST0, OpAccess::Read);
					Self::add_register(flags, info, Register::ST1, OpAccess::Read);
				}
			}
			ImpliedAccess::t_RWcx => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::CX, OpAccess::ReadWrite);
				}
			}
			ImpliedAccess::t_RWecx => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::ECX, OpAccess::ReadWrite);
				}
			}
			ImpliedAccess::t_RWrcx => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::RCX, OpAccess::ReadWrite);
				}
			}
			ImpliedAccess::t_Rcx => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::CX, OpAccess::Read);
				}
			}
			ImpliedAccess::t_Recx => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::ECX, OpAccess::Read);
				}
			}
			ImpliedAccess::t_Rrcx => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::RCX, OpAccess::Read);
				}
			}
			ImpliedAccess::t_Wdx_RWax => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::DX, OpAccess::Write);
					Self::add_register(flags, info, Register::AX, OpAccess::ReadWrite);
				}
			}
			ImpliedAccess::t_Wedx_RWeax => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::EDX, OpAccess::Write);
					Self::add_register(flags, info, Register::EAX, OpAccess::ReadWrite);
				}
			}
			ImpliedAccess::t_Wrdx_RWrax => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::RDX, OpAccess::Write);
					Self::add_register(flags, info, Register::RAX, OpAccess::ReadWrite);
				}
			}
			ImpliedAccess::t_RWax_RWdx => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::AX, OpAccess::ReadWrite);
					Self::add_register(flags, info, Register::DX, OpAccess::ReadWrite);
				}
			}
			ImpliedAccess::t_RWeax_RWedx => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::EAX, OpAccess::ReadWrite);
					Self::add_register(flags, info, Register::EDX, OpAccess::ReadWrite);
				}
			}
			ImpliedAccess::t_RWrax_RWrdx => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::RAX, OpAccess::ReadWrite);
					Self::add_register(flags, info, Register::RDX, OpAccess::ReadWrite);
				}
			}
			ImpliedAccess::t_push2x8 => {
				Self::command_push(instruction, info, flags, 2, 8);
			}
			ImpliedAccess::t_Rcr0 => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::CR0, OpAccess::Read);
				}
			}
			ImpliedAccess::t_RWcr0 => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::CR0, OpAccess::ReadWrite);
				}
			}
			ImpliedAccess::t_gpr16_RWcr0 => {
				Self::command_last_gpr(instruction, info, flags, Register::AX);
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::CR0, OpAccess::ReadWrite);
				}
			}
			ImpliedAccess::t_RCWeax_b64_t_CRrcx_CRrdx_CRrbx_CWrcx_CWrdx_CWrbx_f_CRecx_CRedx_CRebx_CRds_CWecx_CWedx_CWebx => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::EAX, OpAccess::ReadCondWrite);
				}
				if (flags & Flags::IS_64BIT) != 0 {
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						Self::add_register(flags, info, Register::RCX, OpAccess::CondRead);
						Self::add_register(flags, info, Register::RDX, OpAccess::CondRead);
						Self::add_register(flags, info, Register::RBX, OpAccess::CondRead);
						Self::add_register(flags, info, Register::RCX, OpAccess::CondWrite);
						Self::add_register(flags, info, Register::RDX, OpAccess::CondWrite);
						Self::add_register(flags, info, Register::RBX, OpAccess::CondWrite);
					}
				}
				else {
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						Self::add_register(flags, info, Register::ECX, OpAccess::CondRead);
						Self::add_register(flags, info, Register::EDX, OpAccess::CondRead);
						Self::add_register(flags, info, Register::EBX, OpAccess::CondRead);
						Self::add_register(flags, info, Register::DS, OpAccess::CondRead);
						Self::add_register(flags, info, Register::ECX, OpAccess::CondWrite);
						Self::add_register(flags, info, Register::EDX, OpAccess::CondWrite);
						Self::add_register(flags, info, Register::EBX, OpAccess::CondWrite);
					}
				}
			}
			ImpliedAccess::t_RWeax_b64_t_CRrcx_CRrdx_CRrbx_f_CRecx_CRedx_CRebx_CRds => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::EAX, OpAccess::ReadWrite);
				}
				if (flags & Flags::IS_64BIT) != 0 {
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						Self::add_register(flags, info, Register::RCX, OpAccess::CondRead);
						Self::add_register(flags, info, Register::RDX, OpAccess::CondRead);
						Self::add_register(flags, info, Register::RBX, OpAccess::CondRead);
					}
				}
				else {
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						Self::add_register(flags, info, Register::ECX, OpAccess::CondRead);
						Self::add_register(flags, info, Register::EDX, OpAccess::CondRead);
						Self::add_register(flags, info, Register::EBX, OpAccess::CondRead);
						Self::add_register(flags, info, Register::DS, OpAccess::CondRead);
					}
				}
			}
			ImpliedAccess::t_Rmem_Rax_Recx_Redx_Rseg => {
				if (flags & Flags::NO_MEMORY_USAGE) == 0 {
					Self::add_memory(info, Self::get_seg_default_ds(instruction), Register::AX, Register::None, 1, 0x0, MemorySize::Unknown, OpAccess::Read);
				}
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::AX, OpAccess::Read);
					Self::add_register(flags, info, Register::ECX, OpAccess::Read);
					Self::add_register(flags, info, Register::EDX, OpAccess::Read);
					Self::add_memory_segment_register(flags, info, Self::get_seg_default_ds(instruction), OpAccess::Read);
				}
			}
			ImpliedAccess::t_Rmem_Reax_Recx_Redx_Rseg => {
				if (flags & Flags::NO_MEMORY_USAGE) == 0 {
					Self::add_memory(info, Self::get_seg_default_ds(instruction), Register::EAX, Register::None, 1, 0x0, MemorySize::Unknown, OpAccess::Read);
				}
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::EAX, OpAccess::Read);
					Self::add_register(flags, info, Register::ECX, OpAccess::Read);
					Self::add_register(flags, info, Register::EDX, OpAccess::Read);
					Self::add_memory_segment_register(flags, info, Self::get_seg_default_ds(instruction), OpAccess::Read);
				}
			}
			ImpliedAccess::t_Rmem_Recx_Redx_Rrax_Rseg => {
				if (flags & Flags::NO_MEMORY_USAGE) == 0 {
					Self::add_memory(info, Self::get_seg_default_ds(instruction), Register::RAX, Register::None, 1, 0x0, MemorySize::Unknown, OpAccess::Read);
				}
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::ECX, OpAccess::Read);
					Self::add_register(flags, info, Register::EDX, OpAccess::Read);
					Self::add_register(flags, info, Register::RAX, OpAccess::Read);
					Self::add_memory_segment_register(flags, info, Self::get_seg_default_ds(instruction), OpAccess::Read);
				}
			}
			ImpliedAccess::t_Reax_Recx => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::EAX, OpAccess::Read);
					Self::add_register(flags, info, Register::ECX, OpAccess::Read);
				}
			}
			ImpliedAccess::t_Recx_Weax_Wedx => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::ECX, OpAccess::Read);
					Self::add_register(flags, info, Register::EAX, OpAccess::Write);
					Self::add_register(flags, info, Register::EDX, OpAccess::Write);
				}
			}
			ImpliedAccess::t_Reax_Recx_Redx => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::EAX, OpAccess::Read);
					Self::add_register(flags, info, Register::ECX, OpAccess::Read);
					Self::add_register(flags, info, Register::EDX, OpAccess::Read);
				}
			}
			ImpliedAccess::t_Rax => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::AX, OpAccess::Read);
				}
			}
			ImpliedAccess::t_Reax => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::EAX, OpAccess::Read);
				}
			}
			ImpliedAccess::t_Rrax => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::RAX, OpAccess::Read);
				}
			}
			ImpliedAccess::t_Rax_Recx => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::AX, OpAccess::Read);
					Self::add_register(flags, info, Register::ECX, OpAccess::Read);
				}
			}
			ImpliedAccess::t_Recx_Rrax => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::ECX, OpAccess::Read);
					Self::add_register(flags, info, Register::RAX, OpAccess::Read);
				}
			}
			ImpliedAccess::t_Weax_Wecx_Wedx => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::EAX, OpAccess::Write);
					Self::add_register(flags, info, Register::ECX, OpAccess::Write);
					Self::add_register(flags, info, Register::EDX, OpAccess::Write);
				}
			}
			ImpliedAccess::t_Reax_Recx_CRebx => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::EAX, OpAccess::Read);
					Self::add_register(flags, info, Register::ECX, OpAccess::Read);
					Self::add_register(flags, info, Register::EBX, OpAccess::CondRead);
				}
			}
			ImpliedAccess::t_Rax_Rseg => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::AX, OpAccess::Read);
					Self::add_memory_segment_register(flags, info, Self::get_seg_default_ds(instruction), OpAccess::Read);
				}
			}
			ImpliedAccess::t_Reax_Rseg => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::EAX, OpAccess::Read);
					Self::add_memory_segment_register(flags, info, Self::get_seg_default_ds(instruction), OpAccess::Read);
				}
			}
			ImpliedAccess::t_Rrax_Rseg => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::RAX, OpAccess::Read);
					Self::add_memory_segment_register(flags, info, Self::get_seg_default_ds(instruction), OpAccess::Read);
				}
			}
			ImpliedAccess::t_Wecx_b64_t_Wr11 => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::ECX, OpAccess::Write);
				}
				if (flags & Flags::IS_64BIT) != 0 {
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						Self::add_register(flags, info, Register::R11, OpAccess::Write);
					}
				}
			}
			ImpliedAccess::t_Redi_Res => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::EDI, OpAccess::Read);
					Self::add_register(flags, info, Register::ES, OpAccess::Read);
				}
			}
			ImpliedAccess::t_Recx_Wcs_Wss_b64_t_Rr11d => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::ECX, OpAccess::Read);
					Self::add_register(flags, info, Register::CS, OpAccess::Write);
					Self::add_register(flags, info, Register::SS, OpAccess::Write);
				}
				if (flags & Flags::IS_64BIT) != 0 {
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						Self::add_register(flags, info, Register::R11D, OpAccess::Read);
					}
				}
			}
			ImpliedAccess::t_Rr11d_Rrcx_Wcs_Wss => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::R11D, OpAccess::Read);
					Self::add_register(flags, info, Register::RCX, OpAccess::Read);
					Self::add_register(flags, info, Register::CS, OpAccess::Write);
					Self::add_register(flags, info, Register::SS, OpAccess::Write);
				}
			}
			ImpliedAccess::t_Weax_Wedx => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::EAX, OpAccess::Write);
					Self::add_register(flags, info, Register::EDX, OpAccess::Write);
				}
			}
			ImpliedAccess::t_Wesp => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::ESP, OpAccess::Write);
				}
			}
			ImpliedAccess::t_Recx_Redx_Wesp_Wcs_Wss => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::ECX, OpAccess::Read);
					Self::add_register(flags, info, Register::EDX, OpAccess::Read);
					Self::add_register(flags, info, Register::ESP, OpAccess::Write);
					Self::add_register(flags, info, Register::CS, OpAccess::Write);
					Self::add_register(flags, info, Register::SS, OpAccess::Write);
				}
			}
			ImpliedAccess::t_Rrcx_Rrdx_Wrsp_Wcs_Wss => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::RCX, OpAccess::Read);
					Self::add_register(flags, info, Register::RDX, OpAccess::Read);
					Self::add_register(flags, info, Register::RSP, OpAccess::Write);
					Self::add_register(flags, info, Register::CS, OpAccess::Write);
					Self::add_register(flags, info, Register::SS, OpAccess::Write);
				}
			}
			ImpliedAccess::t_zrrm => {
				Self::command_clear_reg_regmem(instruction, info, flags);
			}
			ImpliedAccess::t_zrrrm => {
				Self::command_clear_reg_reg_regmem(instruction, info, flags);
			}
			ImpliedAccess::b64_t_RWxmm0TOxmm15_f_RWxmm0TOxmm7 => {
				if (flags & Flags::IS_64BIT) != 0 {
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						for reg_num in (Register::XMM0 as u32)..((Register::XMM15 as u32) + 1) {
							Self::add_register(flags, info, unsafe { mem::transmute(reg_num as u8) }, OpAccess::ReadWrite);
						}
					}
				}
				else {
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						for reg_num in (Register::XMM0 as u32)..((Register::XMM7 as u32) + 1) {
							Self::add_register(flags, info, unsafe { mem::transmute(reg_num as u8) }, OpAccess::ReadWrite);
						}
					}
				}
			}
			ImpliedAccess::b64_t_Wzmm0TOzmm15_f_Wzmm0TOzmm7 => {
				if (flags & Flags::IS_64BIT) != 0 {
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						for reg_num in (Register::ZMM0 as u32)..((Register::ZMM15 as u32) + 1) {
							Self::add_register(flags, info, unsafe { mem::transmute(reg_num as u8) }, OpAccess::Write);
						}
					}
				}
				else {
					if (flags & Flags::NO_REGISTER_USAGE) == 0 {
						for reg_num in (Register::ZMM0 as u32)..((Register::ZMM7 as u32) + 1) {
							Self::add_register(flags, info, unsafe { mem::transmute(reg_num as u8) }, OpAccess::Write);
						}
					}
				}
			}
			ImpliedAccess::t_CRecx_Wecx_Wedx_Webx_RWeax => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::ECX, OpAccess::CondRead);
					Self::add_register(flags, info, Register::ECX, OpAccess::Write);
					Self::add_register(flags, info, Register::EDX, OpAccess::Write);
					Self::add_register(flags, info, Register::EBX, OpAccess::Write);
					Self::add_register(flags, info, Register::EAX, OpAccess::ReadWrite);
				}
			}
			ImpliedAccess::t_CRmem_CRsi_CReax_CRes_CWeax_CWedx_RCWecx => {
				if (flags & Flags::NO_MEMORY_USAGE) == 0 {
					Self::add_memory(info, Register::ES, Register::SI, Register::None, 1, 0x0, MemorySize::Unknown, OpAccess::CondRead);
				}
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::SI, OpAccess::CondRead);
					Self::add_register(flags, info, Register::EAX, OpAccess::CondRead);
					if (flags & Flags::IS_64BIT) == 0 {
						Self::add_register(flags, info, Register::ES, OpAccess::CondRead);
					}
					Self::add_register(flags, info, Register::EAX, OpAccess::CondWrite);
					Self::add_register(flags, info, Register::EDX, OpAccess::CondWrite);
					Self::add_register(flags, info, Register::ECX, OpAccess::ReadCondWrite);
				}
			}
			ImpliedAccess::t_CRmem_CReax_CResi_CRes_CWeax_CWedx_RCWecx => {
				if (flags & Flags::NO_MEMORY_USAGE) == 0 {
					Self::add_memory(info, Register::ES, Register::ESI, Register::None, 1, 0x0, MemorySize::Unknown, OpAccess::CondRead);
				}
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::EAX, OpAccess::CondRead);
					Self::add_register(flags, info, Register::ESI, OpAccess::CondRead);
					if (flags & Flags::IS_64BIT) == 0 {
						Self::add_register(flags, info, Register::ES, OpAccess::CondRead);
					}
					Self::add_register(flags, info, Register::EAX, OpAccess::CondWrite);
					Self::add_register(flags, info, Register::EDX, OpAccess::CondWrite);
					Self::add_register(flags, info, Register::ECX, OpAccess::ReadCondWrite);
				}
			}
			ImpliedAccess::t_CRmem_CReax_CRrsi_CRes_CWeax_CWedx_RCWrcx => {
				if (flags & Flags::NO_MEMORY_USAGE) == 0 {
					Self::add_memory(info, Register::ES, Register::RSI, Register::None, 1, 0x0, MemorySize::Unknown, OpAccess::CondRead);
				}
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::EAX, OpAccess::CondRead);
					Self::add_register(flags, info, Register::RSI, OpAccess::CondRead);
					if (flags & Flags::IS_64BIT) == 0 {
						Self::add_register(flags, info, Register::ES, OpAccess::CondRead);
					}
					Self::add_register(flags, info, Register::EAX, OpAccess::CondWrite);
					Self::add_register(flags, info, Register::EDX, OpAccess::CondWrite);
					Self::add_register(flags, info, Register::RCX, OpAccess::ReadCondWrite);
				}
			}
			ImpliedAccess::t_CRmem_CRmem_CWmem_CRax_CRsi_CRdi_CRes_CWax_CWsi_CWdi_RCWcx => {
				if (flags & Flags::NO_MEMORY_USAGE) == 0 {
					Self::add_memory(info, Register::ES, Register::SI, Register::None, 1, 0x0, MemorySize::Unknown, OpAccess::CondRead);
					Self::add_memory(info, Register::ES, Register::DI, Register::None, 1, 0x0, MemorySize::Unknown, OpAccess::CondRead);
					Self::add_memory(info, Register::ES, Register::DI, Register::None, 1, 0x0, MemorySize::Unknown, OpAccess::CondWrite);
				}
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::AX, OpAccess::CondRead);
					Self::add_register(flags, info, Register::SI, OpAccess::CondRead);
					Self::add_register(flags, info, Register::DI, OpAccess::CondRead);
					if (flags & Flags::IS_64BIT) == 0 {
						Self::add_register(flags, info, Register::ES, OpAccess::CondRead);
					}
					Self::add_register(flags, info, Register::AX, OpAccess::CondWrite);
					Self::add_register(flags, info, Register::SI, OpAccess::CondWrite);
					Self::add_register(flags, info, Register::DI, OpAccess::CondWrite);
					Self::add_register(flags, info, Register::CX, OpAccess::ReadCondWrite);
				}
			}
			ImpliedAccess::t_CRmem_CRmem_CWmem_CReax_CResi_CRedi_CRes_CWeax_CWesi_CWedi_RCWecx => {
				if (flags & Flags::NO_MEMORY_USAGE) == 0 {
					Self::add_memory(info, Register::ES, Register::ESI, Register::None, 1, 0x0, MemorySize::Unknown, OpAccess::CondRead);
					Self::add_memory(info, Register::ES, Register::EDI, Register::None, 1, 0x0, MemorySize::Unknown, OpAccess::CondRead);
					Self::add_memory(info, Register::ES, Register::EDI, Register::None, 1, 0x0, MemorySize::Unknown, OpAccess::CondWrite);
				}
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::EAX, OpAccess::CondRead);
					Self::add_register(flags, info, Register::ESI, OpAccess::CondRead);
					Self::add_register(flags, info, Register::EDI, OpAccess::CondRead);
					if (flags & Flags::IS_64BIT) == 0 {
						Self::add_register(flags, info, Register::ES, OpAccess::CondRead);
					}
					Self::add_register(flags, info, Register::EAX, OpAccess::CondWrite);
					Self::add_register(flags, info, Register::ESI, OpAccess::CondWrite);
					Self::add_register(flags, info, Register::EDI, OpAccess::CondWrite);
					Self::add_register(flags, info, Register::ECX, OpAccess::ReadCondWrite);
				}
			}
			ImpliedAccess::t_CRmem_CRmem_CWmem_CRrax_CRrsi_CRrdi_CRes_CWrax_CWrsi_CWrdi_RCWrcx => {
				if (flags & Flags::NO_MEMORY_USAGE) == 0 {
					Self::add_memory(info, Register::ES, Register::RSI, Register::None, 1, 0x0, MemorySize::Unknown, OpAccess::CondRead);
					Self::add_memory(info, Register::ES, Register::RDI, Register::None, 1, 0x0, MemorySize::Unknown, OpAccess::CondRead);
					Self::add_memory(info, Register::ES, Register::RDI, Register::None, 1, 0x0, MemorySize::Unknown, OpAccess::CondWrite);
				}
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::RAX, OpAccess::CondRead);
					Self::add_register(flags, info, Register::RSI, OpAccess::CondRead);
					Self::add_register(flags, info, Register::RDI, OpAccess::CondRead);
					if (flags & Flags::IS_64BIT) == 0 {
						Self::add_register(flags, info, Register::ES, OpAccess::CondRead);
					}
					Self::add_register(flags, info, Register::RAX, OpAccess::CondWrite);
					Self::add_register(flags, info, Register::RSI, OpAccess::CondWrite);
					Self::add_register(flags, info, Register::RDI, OpAccess::CondWrite);
					Self::add_register(flags, info, Register::RCX, OpAccess::ReadCondWrite);
				}
			}
			ImpliedAccess::t_Rcl_Rax => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::CL, OpAccess::Read);
					Self::add_register(flags, info, Register::AX, OpAccess::Read);
				}
			}
			ImpliedAccess::t_Rcl_Reax => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::CL, OpAccess::Read);
					Self::add_register(flags, info, Register::EAX, OpAccess::Read);
				}
			}
			ImpliedAccess::t_xstore => {
				Self::command_xstore(instruction, info, flags);
			}
			ImpliedAccess::t_CRmem_CRmem_CRmem_CWmem_CRdx_CRbx_CRsi_CRdi_CRes_CWsi_CWdi_RCWcx => {
				if (flags & Flags::NO_MEMORY_USAGE) == 0 {
					Self::add_memory(info, Register::ES, Register::DX, Register::None, 1, 0x0, MemorySize::Unknown, OpAccess::CondRead);
					Self::add_memory(info, Register::ES, Register::BX, Register::None, 1, 0x0, MemorySize::Unknown, OpAccess::CondRead);
					Self::add_memory(info, Register::ES, Register::SI, Register::None, 1, 0x0, MemorySize::Unknown, OpAccess::CondRead);
					Self::add_memory(info, Register::ES, Register::DI, Register::None, 1, 0x0, MemorySize::Unknown, OpAccess::CondWrite);
				}
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::DX, OpAccess::CondRead);
					Self::add_register(flags, info, Register::BX, OpAccess::CondRead);
					Self::add_register(flags, info, Register::SI, OpAccess::CondRead);
					Self::add_register(flags, info, Register::DI, OpAccess::CondRead);
					if (flags & Flags::IS_64BIT) == 0 {
						Self::add_register(flags, info, Register::ES, OpAccess::CondRead);
					}
					Self::add_register(flags, info, Register::SI, OpAccess::CondWrite);
					Self::add_register(flags, info, Register::DI, OpAccess::CondWrite);
					Self::add_register(flags, info, Register::CX, OpAccess::ReadCondWrite);
				}
			}
			ImpliedAccess::t_CRmem_CRmem_CRmem_CWmem_CRedx_CRebx_CResi_CRedi_CRes_CWesi_CWedi_RCWecx => {
				if (flags & Flags::NO_MEMORY_USAGE) == 0 {
					Self::add_memory(info, Register::ES, Register::EDX, Register::None, 1, 0x0, MemorySize::Unknown, OpAccess::CondRead);
					Self::add_memory(info, Register::ES, Register::EBX, Register::None, 1, 0x0, MemorySize::Unknown, OpAccess::CondRead);
					Self::add_memory(info, Register::ES, Register::ESI, Register::None, 1, 0x0, MemorySize::Unknown, OpAccess::CondRead);
					Self::add_memory(info, Register::ES, Register::EDI, Register::None, 1, 0x0, MemorySize::Unknown, OpAccess::CondWrite);
				}
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::EDX, OpAccess::CondRead);
					Self::add_register(flags, info, Register::EBX, OpAccess::CondRead);
					Self::add_register(flags, info, Register::ESI, OpAccess::CondRead);
					Self::add_register(flags, info, Register::EDI, OpAccess::CondRead);
					if (flags & Flags::IS_64BIT) == 0 {
						Self::add_register(flags, info, Register::ES, OpAccess::CondRead);
					}
					Self::add_register(flags, info, Register::ESI, OpAccess::CondWrite);
					Self::add_register(flags, info, Register::EDI, OpAccess::CondWrite);
					Self::add_register(flags, info, Register::ECX, OpAccess::ReadCondWrite);
				}
			}
			ImpliedAccess::t_CRmem_CRmem_CRmem_CWmem_CRrdx_CRrbx_CRrsi_CRrdi_CRes_CWrsi_CWrdi_RCWrcx => {
				if (flags & Flags::NO_MEMORY_USAGE) == 0 {
					Self::add_memory(info, Register::ES, Register::RDX, Register::None, 1, 0x0, MemorySize::Unknown, OpAccess::CondRead);
					Self::add_memory(info, Register::ES, Register::RBX, Register::None, 1, 0x0, MemorySize::Unknown, OpAccess::CondRead);
					Self::add_memory(info, Register::ES, Register::RSI, Register::None, 1, 0x0, MemorySize::Unknown, OpAccess::CondRead);
					Self::add_memory(info, Register::ES, Register::RDI, Register::None, 1, 0x0, MemorySize::Unknown, OpAccess::CondWrite);
				}
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::RDX, OpAccess::CondRead);
					Self::add_register(flags, info, Register::RBX, OpAccess::CondRead);
					Self::add_register(flags, info, Register::RSI, OpAccess::CondRead);
					Self::add_register(flags, info, Register::RDI, OpAccess::CondRead);
					if (flags & Flags::IS_64BIT) == 0 {
						Self::add_register(flags, info, Register::ES, OpAccess::CondRead);
					}
					Self::add_register(flags, info, Register::RSI, OpAccess::CondWrite);
					Self::add_register(flags, info, Register::RDI, OpAccess::CondWrite);
					Self::add_register(flags, info, Register::RCX, OpAccess::ReadCondWrite);
				}
			}
			ImpliedAccess::t_CRmem_CRmem_CRmem_CRmem_CWmem_CWmem_CRax_CRdx_CRbx_CRsi_CRdi_CRes_CWax_CWsi_CWdi_RCWcx => {
				if (flags & Flags::NO_MEMORY_USAGE) == 0 {
					Self::add_memory(info, Register::ES, Register::AX, Register::None, 1, 0x0, MemorySize::Unknown, OpAccess::CondRead);
					Self::add_memory(info, Register::ES, Register::DX, Register::None, 1, 0x0, MemorySize::Unknown, OpAccess::CondRead);
					Self::add_memory(info, Register::ES, Register::BX, Register::None, 1, 0x0, MemorySize::Unknown, OpAccess::CondRead);
					Self::add_memory(info, Register::ES, Register::SI, Register::None, 1, 0x0, MemorySize::Unknown, OpAccess::CondRead);
					Self::add_memory(info, Register::ES, Register::AX, Register::None, 1, 0x0, MemorySize::Unknown, OpAccess::CondWrite);
					Self::add_memory(info, Register::ES, Register::DI, Register::None, 1, 0x0, MemorySize::Unknown, OpAccess::CondWrite);
				}
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::AX, OpAccess::CondRead);
					Self::add_register(flags, info, Register::DX, OpAccess::CondRead);
					Self::add_register(flags, info, Register::BX, OpAccess::CondRead);
					Self::add_register(flags, info, Register::SI, OpAccess::CondRead);
					Self::add_register(flags, info, Register::DI, OpAccess::CondRead);
					if (flags & Flags::IS_64BIT) == 0 {
						Self::add_register(flags, info, Register::ES, OpAccess::CondRead);
					}
					Self::add_register(flags, info, Register::AX, OpAccess::CondWrite);
					Self::add_register(flags, info, Register::SI, OpAccess::CondWrite);
					Self::add_register(flags, info, Register::DI, OpAccess::CondWrite);
					Self::add_register(flags, info, Register::CX, OpAccess::ReadCondWrite);
				}
			}
			ImpliedAccess::t_CRmem_CRmem_CRmem_CRmem_CWmem_CWmem_CReax_CRedx_CRebx_CResi_CRedi_CRes_CWeax_CWesi_CWedi_RCWecx => {
				if (flags & Flags::NO_MEMORY_USAGE) == 0 {
					Self::add_memory(info, Register::ES, Register::EAX, Register::None, 1, 0x0, MemorySize::Unknown, OpAccess::CondRead);
					Self::add_memory(info, Register::ES, Register::EDX, Register::None, 1, 0x0, MemorySize::Unknown, OpAccess::CondRead);
					Self::add_memory(info, Register::ES, Register::EBX, Register::None, 1, 0x0, MemorySize::Unknown, OpAccess::CondRead);
					Self::add_memory(info, Register::ES, Register::ESI, Register::None, 1, 0x0, MemorySize::Unknown, OpAccess::CondRead);
					Self::add_memory(info, Register::ES, Register::EAX, Register::None, 1, 0x0, MemorySize::Unknown, OpAccess::CondWrite);
					Self::add_memory(info, Register::ES, Register::EDI, Register::None, 1, 0x0, MemorySize::Unknown, OpAccess::CondWrite);
				}
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::EAX, OpAccess::CondRead);
					Self::add_register(flags, info, Register::EDX, OpAccess::CondRead);
					Self::add_register(flags, info, Register::EBX, OpAccess::CondRead);
					Self::add_register(flags, info, Register::ESI, OpAccess::CondRead);
					Self::add_register(flags, info, Register::EDI, OpAccess::CondRead);
					if (flags & Flags::IS_64BIT) == 0 {
						Self::add_register(flags, info, Register::ES, OpAccess::CondRead);
					}
					Self::add_register(flags, info, Register::EAX, OpAccess::CondWrite);
					Self::add_register(flags, info, Register::ESI, OpAccess::CondWrite);
					Self::add_register(flags, info, Register::EDI, OpAccess::CondWrite);
					Self::add_register(flags, info, Register::ECX, OpAccess::ReadCondWrite);
				}
			}
			ImpliedAccess::t_CRmem_CRmem_CRmem_CRmem_CWmem_CWmem_CRrax_CRrdx_CRrbx_CRrsi_CRrdi_CRes_CWrax_CWrsi_CWrdi_RCWrcx => {
				if (flags & Flags::NO_MEMORY_USAGE) == 0 {
					Self::add_memory(info, Register::ES, Register::RAX, Register::None, 1, 0x0, MemorySize::Unknown, OpAccess::CondRead);
					Self::add_memory(info, Register::ES, Register::RDX, Register::None, 1, 0x0, MemorySize::Unknown, OpAccess::CondRead);
					Self::add_memory(info, Register::ES, Register::RBX, Register::None, 1, 0x0, MemorySize::Unknown, OpAccess::CondRead);
					Self::add_memory(info, Register::ES, Register::RSI, Register::None, 1, 0x0, MemorySize::Unknown, OpAccess::CondRead);
					Self::add_memory(info, Register::ES, Register::RAX, Register::None, 1, 0x0, MemorySize::Unknown, OpAccess::CondWrite);
					Self::add_memory(info, Register::ES, Register::RDI, Register::None, 1, 0x0, MemorySize::Unknown, OpAccess::CondWrite);
				}
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::RAX, OpAccess::CondRead);
					Self::add_register(flags, info, Register::RDX, OpAccess::CondRead);
					Self::add_register(flags, info, Register::RBX, OpAccess::CondRead);
					Self::add_register(flags, info, Register::RSI, OpAccess::CondRead);
					Self::add_register(flags, info, Register::RDI, OpAccess::CondRead);
					if (flags & Flags::IS_64BIT) == 0 {
						Self::add_register(flags, info, Register::ES, OpAccess::CondRead);
					}
					Self::add_register(flags, info, Register::RAX, OpAccess::CondWrite);
					Self::add_register(flags, info, Register::RSI, OpAccess::CondWrite);
					Self::add_register(flags, info, Register::RDI, OpAccess::CondWrite);
					Self::add_register(flags, info, Register::RCX, OpAccess::ReadCondWrite);
				}
			}
			ImpliedAccess::t_RCWal => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::AL, OpAccess::ReadCondWrite);
				}
			}
			ImpliedAccess::t_RCWax => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::AX, OpAccess::ReadCondWrite);
				}
			}
			ImpliedAccess::t_RCWeax => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::EAX, OpAccess::ReadCondWrite);
				}
			}
			ImpliedAccess::t_Reax_Redx => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::EAX, OpAccess::Read);
					Self::add_register(flags, info, Register::EDX, OpAccess::Read);
				}
			}
			ImpliedAccess::t_gpr8 => {
				Self::command_last_gpr(instruction, info, flags, Register::AL);
			}
			ImpliedAccess::t_gpr32_Reax_Redx => {
				Self::command_last_gpr(instruction, info, flags, Register::EAX);
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::EAX, OpAccess::Read);
					Self::add_register(flags, info, Register::EDX, OpAccess::Read);
				}
			}
			ImpliedAccess::t_Rmem_Rseg => {
				if (flags & Flags::NO_MEMORY_USAGE) == 0 {
					Self::add_memory(info, Self::get_seg_default_ds(instruction), instruction.op0_register(), Register::None, 1, 0x0, MemorySize::UInt8, OpAccess::Read);
				}
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_memory_segment_register(flags, info, Self::get_seg_default_ds(instruction), OpAccess::Read);
				}
			}
			ImpliedAccess::t_RCWrax => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::RAX, OpAccess::ReadCondWrite);
				}
			}
			ImpliedAccess::t_Wss => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::SS, OpAccess::Write);
				}
			}
			ImpliedAccess::t_Wfs => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::FS, OpAccess::Write);
				}
			}
			ImpliedAccess::t_Wgs => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::GS, OpAccess::Write);
				}
			}
			ImpliedAccess::t_CRecx_CRebx_RCWeax_RCWedx => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::ECX, OpAccess::CondRead);
					Self::add_register(flags, info, Register::EBX, OpAccess::CondRead);
					Self::add_register(flags, info, Register::EAX, OpAccess::ReadCondWrite);
					Self::add_register(flags, info, Register::EDX, OpAccess::ReadCondWrite);
				}
			}
			ImpliedAccess::t_CRrcx_CRrbx_RCWrax_RCWrdx => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::RCX, OpAccess::CondRead);
					Self::add_register(flags, info, Register::RBX, OpAccess::CondRead);
					Self::add_register(flags, info, Register::RAX, OpAccess::ReadCondWrite);
					Self::add_register(flags, info, Register::RDX, OpAccess::ReadCondWrite);
				}
			}
			ImpliedAccess::t_Wmem_RarDI_Rseg => {
				if (flags & Flags::NO_MEMORY_USAGE) == 0 {
					Self::add_memory(info, Self::get_seg_default_ds(instruction), Self::get_a_rdi(instruction), Register::None, 1, 0x0, instruction.memory_size(), OpAccess::Write);
				}
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Self::get_a_rdi(instruction), OpAccess::Read);
					Self::add_memory_segment_register(flags, info, Self::get_seg_default_ds(instruction), OpAccess::Read);
				}
			}
			ImpliedAccess::t_Rxmm0 => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::XMM0, OpAccess::Read);
				}
			}
			ImpliedAccess::t_Redx => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::EDX, OpAccess::Read);
				}
			}
			ImpliedAccess::t_Rrdx => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::RDX, OpAccess::Read);
				}
			}
			ImpliedAccess::t_Wmem_Res => {
				if (flags & Flags::NO_MEMORY_USAGE) == 0 {
					Self::add_memory(info, Register::ES, instruction.op0_register(), Register::None, 1, 0x0, instruction.memory_size(), OpAccess::Write);
				}
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					if (flags & Flags::IS_64BIT) == 0 {
						Self::add_register(flags, info, Register::ES, OpAccess::Read);
					}
				}
			}
			ImpliedAccess::t_Reax_Redx_Wxmm0 => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::EAX, OpAccess::Read);
					Self::add_register(flags, info, Register::EDX, OpAccess::Read);
					Self::add_register(flags, info, Register::XMM0, OpAccess::Write);
				}
			}
			ImpliedAccess::t_Rrax_Rrdx_Wxmm0 => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::RAX, OpAccess::Read);
					Self::add_register(flags, info, Register::RDX, OpAccess::Read);
					Self::add_register(flags, info, Register::XMM0, OpAccess::Write);
				}
			}
			ImpliedAccess::t_Reax_Redx_Wecx => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::EAX, OpAccess::Read);
					Self::add_register(flags, info, Register::EDX, OpAccess::Read);
					Self::add_register(flags, info, Register::ECX, OpAccess::Write);
				}
			}
			ImpliedAccess::t_Rrax_Rrdx_Wecx => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::RAX, OpAccess::Read);
					Self::add_register(flags, info, Register::RDX, OpAccess::Read);
					Self::add_register(flags, info, Register::ECX, OpAccess::Write);
				}
			}
			ImpliedAccess::t_Wxmm0 => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::XMM0, OpAccess::Write);
				}
			}
			ImpliedAccess::t_Wecx => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::ECX, OpAccess::Write);
				}
			}
			ImpliedAccess::t_Rmem_Rds => {
				if (flags & Flags::NO_MEMORY_USAGE) == 0 {
					Self::add_memory(info, Register::DS, instruction.op0_register(), Register::None, 1, 0x0, MemorySize::Unknown, OpAccess::Read);
				}
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					if (flags & Flags::IS_64BIT) == 0 {
						Self::add_register(flags, info, Register::DS, OpAccess::Read);
					}
				}
			}
			ImpliedAccess::t_Rrcx_Rrdx_RWrax => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::RCX, OpAccess::Read);
					Self::add_register(flags, info, Register::RDX, OpAccess::Read);
					Self::add_register(flags, info, Register::RAX, OpAccess::ReadWrite);
				}
			}
			ImpliedAccess::t_Rmem_Rrcx_Rseg_RWrax => {
				if (flags & Flags::NO_MEMORY_USAGE) == 0 {
					Self::add_memory(info, Self::get_seg_default_ds(instruction), Register::RCX, Register::None, 1, 0x0, MemorySize::UInt128, OpAccess::Read);
				}
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::RCX, OpAccess::Read);
					Self::add_memory_segment_register(flags, info, Self::get_seg_default_ds(instruction), OpAccess::Read);
					Self::add_register(flags, info, Register::RAX, OpAccess::ReadWrite);
				}
			}
			ImpliedAccess::t_RWrax => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::RAX, OpAccess::ReadWrite);
				}
			}
			ImpliedAccess::t_Rax_Recx_Redx_Weax => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::AX, OpAccess::Read);
					Self::add_register(flags, info, Register::ECX, OpAccess::Read);
					Self::add_register(flags, info, Register::EDX, OpAccess::Read);
					Self::add_register(flags, info, Register::EAX, OpAccess::Write);
				}
			}
			ImpliedAccess::t_Recx_Redx_RWeax => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::ECX, OpAccess::Read);
					Self::add_register(flags, info, Register::EDX, OpAccess::Read);
					Self::add_register(flags, info, Register::EAX, OpAccess::ReadWrite);
				}
			}
			ImpliedAccess::t_Recx_Redx_RWrax => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::ECX, OpAccess::Read);
					Self::add_register(flags, info, Register::EDX, OpAccess::Read);
					Self::add_register(flags, info, Register::RAX, OpAccess::ReadWrite);
				}
			}
			ImpliedAccess::t_Rax_Recx_Redx => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::AX, OpAccess::Read);
					Self::add_register(flags, info, Register::ECX, OpAccess::Read);
					Self::add_register(flags, info, Register::EDX, OpAccess::Read);
				}
			}
			ImpliedAccess::t_Recx_Redx_Rrax => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::ECX, OpAccess::Read);
					Self::add_register(flags, info, Register::EDX, OpAccess::Read);
					Self::add_register(flags, info, Register::RAX, OpAccess::Read);
				}
			}
			ImpliedAccess::t_Wtmm0TOtmm7 => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					for reg_num in (Register::TMM0 as u32)..((Register::TMM7 as u32) + 1) {
						Self::add_register(flags, info, unsafe { mem::transmute(reg_num as u8) }, OpAccess::Write);
					}
				}
			}
			ImpliedAccess::t_Reax_Rebx => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::EAX, OpAccess::Read);
					Self::add_register(flags, info, Register::EBX, OpAccess::Read);
				}
			}
			ImpliedAccess::t_Rebx_Weax => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::EBX, OpAccess::Read);
					Self::add_register(flags, info, Register::EAX, OpAccess::Write);
				}
			}
			ImpliedAccess::t_emmiW => {
				Self::command_emmi(instruction, info, flags, OpAccess::Write);
			}
			ImpliedAccess::t_emmiRW => {
				Self::command_emmi(instruction, info, flags, OpAccess::ReadWrite);
			}
			ImpliedAccess::t_emmiR => {
				Self::command_emmi(instruction, info, flags, OpAccess::Read);
			}
			ImpliedAccess::t_CRrcx_CRrdx_CRr8_CRr9_RWrax => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::RCX, OpAccess::CondRead);
					Self::add_register(flags, info, Register::RDX, OpAccess::CondRead);
					Self::add_register(flags, info, Register::R8, OpAccess::CondRead);
					Self::add_register(flags, info, Register::R9, OpAccess::CondRead);
					Self::add_register(flags, info, Register::RAX, OpAccess::ReadWrite);
				}
			}
			ImpliedAccess::t_RWxmm0TOxmm7 => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					for reg_num in (Register::XMM0 as u32)..((Register::XMM7 as u32) + 1) {
						Self::add_register(flags, info, unsafe { mem::transmute(reg_num as u8) }, OpAccess::ReadWrite);
					}
				}
			}
			ImpliedAccess::t_Reax_Rxmm0 => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::EAX, OpAccess::Read);
					Self::add_register(flags, info, Register::XMM0, OpAccess::Read);
				}
			}
			ImpliedAccess::t_Wxmm1_Wxmm2_RWxmm0_Wxmm4TOxmm6 => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::XMM1, OpAccess::Write);
					Self::add_register(flags, info, Register::XMM2, OpAccess::Write);
					Self::add_register(flags, info, Register::XMM0, OpAccess::ReadWrite);
					for reg_num in (Register::XMM4 as u32)..((Register::XMM6 as u32) + 1) {
						Self::add_register(flags, info, unsafe { mem::transmute(reg_num as u8) }, OpAccess::Write);
					}
				}
			}
			ImpliedAccess::t_RWxmm0_RWxmm1_Wxmm2TOxmm6 => {
				if (flags & Flags::NO_REGISTER_USAGE) == 0 {
					Self::add_register(flags, info, Register::XMM0, OpAccess::ReadWrite);
					Self::add_register(flags, info, Register::XMM1, OpAccess::ReadWrite);
					for reg_num in (Register::XMM2 as u32)..((Register::XMM6 as u32) + 1) {
						Self::add_register(flags, info, unsafe { mem::transmute(reg_num as u8) }, OpAccess::Write);
					}
				}
			}
			ImpliedAccess::t_pop3x8 => {
				Self::command_pop(instruction, info, flags, 3, 8);
			}
			// GENERATOR-END: ImpliedAccessHandler
		}
	}

	#[cfg_attr(has_must_use, must_use)]
	fn get_a_rdi(instruction: &Instruction) -> Register {
		match instruction.op0_kind() {
			OpKind::MemorySegDI => Register::DI,
			OpKind::MemorySegEDI => Register::EDI,
			_ => Register::RDI,
		}
	}

	#[cfg_attr(has_must_use, must_use)]
	fn get_seg_default_ds(instruction: &Instruction) -> Register {
		let seg = instruction.segment_prefix();
		if seg == Register::None {
			Register::DS
		} else {
			seg
		}
	}

	fn command_push(instruction: &Instruction, info: &mut InstructionInfo, flags: u32, count: u32, op_size: u32) {
		debug_assert!(count > 0);
		let mut xsp_mask = 0;
		let xsp = Self::get_xsp(instruction.code_size(), &mut xsp_mask);
		if (flags & Flags::NO_REGISTER_USAGE) == 0 {
			if (flags & Flags::IS_64BIT) == 0 {
				Self::add_register(flags, info, Register::SS, OpAccess::Read);
			}
			Self::add_register(flags, info, xsp, OpAccess::ReadWrite);
		}
		if (flags & Flags::NO_MEMORY_USAGE) == 0 {
			let mem_size = if op_size == 8 {
				MemorySize::UInt64
			} else if op_size == 4 {
				MemorySize::UInt32
			} else {
				debug_assert_eq!(2, op_size);
				MemorySize::UInt16
			};
			let mut offset = (op_size as u64).wrapping_neg();
			for _ in 0..count {
				Self::add_memory(info, Register::SS, xsp, Register::None, 1, offset & xsp_mask, mem_size, OpAccess::Write);
				offset -= op_size as u64;
			}
		}
	}

	fn command_pop(instruction: &Instruction, info: &mut InstructionInfo, flags: u32, count: u32, op_size: u32) {
		debug_assert!(count > 0);
		let mut xsp_mask = 0;
		let xsp = Self::get_xsp(instruction.code_size(), &mut xsp_mask);
		if (flags & Flags::NO_REGISTER_USAGE) == 0 {
			if (flags & Flags::IS_64BIT) == 0 {
				Self::add_register(flags, info, Register::SS, OpAccess::Read);
			}
			Self::add_register(flags, info, xsp, OpAccess::ReadWrite);
		}
		if (flags & Flags::NO_MEMORY_USAGE) == 0 {
			let mem_size = if op_size == 8 {
				MemorySize::UInt64
			} else if op_size == 4 {
				MemorySize::UInt32
			} else {
				debug_assert_eq!(2, op_size);
				MemorySize::UInt16
			};
			let mut offset = 0;
			for _ in 0..count {
				Self::add_memory(info, Register::SS, xsp, Register::None, 1, offset, mem_size, OpAccess::Read);
				offset += op_size as u64;
			}
		}
	}

	fn command_pop_rm(instruction: &Instruction, info: &mut InstructionInfo, flags: u32, op_size: u32) {
		let mut xsp_mask = 0;
		let xsp = Self::get_xsp(instruction.code_size(), &mut xsp_mask);
		if (flags & Flags::NO_REGISTER_USAGE) == 0 {
			if (flags & Flags::IS_64BIT) == 0 {
				Self::add_register(flags, info, Register::SS, OpAccess::Read);
			}
			Self::add_register(flags, info, xsp, OpAccess::ReadWrite);
		}
		if (flags & Flags::NO_MEMORY_USAGE) == 0 {
			let memory_size = if op_size == 8 {
				MemorySize::UInt64
			} else if op_size == 4 {
				MemorySize::UInt32
			} else {
				debug_assert_eq!(2, op_size);
				MemorySize::UInt16
			};
			if instruction.op0_kind() == OpKind::Memory {
				debug_assert_eq!(1, info.used_memory_locations.len());
				if instruction.memory_base() == Register::RSP || instruction.memory_base() == Register::ESP {
					let mem = info.used_memory_locations[0];
					let mut displ = mem.displacement().wrapping_add(op_size as u64);
					if instruction.memory_base() == Register::ESP {
						displ = displ as u32 as u64;
					}
					info.used_memory_locations[0] = UsedMemory { displacement: displ, ..mem };
				}
			}
			Self::add_memory(info, Register::SS, xsp, Register::None, 1, 0, memory_size, OpAccess::Read);
		}
	}

	fn command_pusha(instruction: &Instruction, info: &mut InstructionInfo, flags: u32, op_size: u32) {
		let mut xsp_mask = 0;
		let xsp = Self::get_xsp(instruction.code_size(), &mut xsp_mask);
		if (flags & Flags::NO_REGISTER_USAGE) == 0 {
			if (flags & Flags::IS_64BIT) == 0 {
				Self::add_register(flags, info, Register::SS, OpAccess::Read);
			}
			Self::add_register(flags, info, xsp, OpAccess::ReadWrite);
		}
		let (displ, memory_size, base_register) = if op_size == 4 {
			(-4i64, MemorySize::UInt32, Register::EAX)
		} else {
			debug_assert_eq!(2, op_size);
			(-2i64, MemorySize::UInt16, Register::AX)
		};
		for i in 0..8 {
			if (flags & Flags::NO_REGISTER_USAGE) == 0 {
				Self::add_register(flags, info, unsafe { mem::transmute((base_register as u32).wrapping_add(i) as u8) }, OpAccess::Read);
			}
			if (flags & Flags::NO_MEMORY_USAGE) == 0 {
				Self::add_memory(
					info,
					Register::SS,
					xsp,
					Register::None,
					1,
					displ.wrapping_mul((i + 1) as i64) as u64 & xsp_mask,
					memory_size,
					OpAccess::Write,
				);
			}
		}
	}

	fn command_popa(instruction: &Instruction, info: &mut InstructionInfo, flags: u32, op_size: u32) {
		let mut xsp_mask = 0;
		let xsp = Self::get_xsp(instruction.code_size(), &mut xsp_mask);
		if (flags & Flags::NO_REGISTER_USAGE) == 0 {
			if (flags & Flags::IS_64BIT) == 0 {
				Self::add_register(flags, info, Register::SS, OpAccess::Read);
			}
			Self::add_register(flags, info, xsp, OpAccess::ReadWrite);
		}
		let (memory_size, base_register) = if op_size == 4 {
			(MemorySize::UInt32, Register::EAX)
		} else {
			debug_assert_eq!(2, op_size);
			(MemorySize::UInt16, Register::AX)
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
						(op_size as u64).wrapping_mul(i as u64) & xsp_mask,
						memory_size,
						OpAccess::Read,
					);
				}
			}
		}
	}

	fn command_ins(instruction: &Instruction, info: &mut InstructionInfo, flags: u32) {
		let base_register;
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
				mem::transmute(((instruction.op0_kind() as u32).wrapping_sub(OpKind::MemoryESDI as u32) << 4).wrapping_add(Register::DI as u32) as u8)
			};
			if (flags & Flags::NO_MEMORY_USAGE) == 0 {
				Self::add_memory(info, Register::ES, base_register, Register::None, 1, 0, MemorySize::Unknown, OpAccess::CondWrite);
			}
			if (flags & Flags::NO_REGISTER_USAGE) == 0 {
				debug_assert_eq!(1, info.used_registers.len());
				info.used_registers[0] = UsedRegister { register: Register::DX, access: OpAccess::CondRead };
				Self::add_register(
					flags,
					info,
					unsafe {
						mem::transmute(
							((instruction.op0_kind() as u32).wrapping_sub(OpKind::MemoryESDI as u32) << 4).wrapping_add(Register::CX as u32) as u8,
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
				mem::transmute(((instruction.op0_kind() as u32).wrapping_sub(OpKind::MemoryESDI as u32) << 4).wrapping_add(Register::DI as u32) as u8)
			};
			if (flags & Flags::NO_MEMORY_USAGE) == 0 {
				Self::add_memory(info, Register::ES, base_register, Register::None, 1, 0, instruction.memory_size(), OpAccess::Write);
			}
			if (flags & Flags::NO_REGISTER_USAGE) == 0 {
				if (flags & Flags::IS_64BIT) == 0 {
					Self::add_register(flags, info, Register::ES, OpAccess::Read);
				}
				Self::add_register(flags, info, base_register, OpAccess::ReadWrite);
			}
		}
	}

	fn command_outs(instruction: &Instruction, info: &mut InstructionInfo, flags: u32) {
		let base_register;
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
				mem::transmute(((instruction.op1_kind() as u32).wrapping_sub(OpKind::MemorySegSI as u32) << 4).wrapping_add(Register::SI as u32) as u8)
			};
			if (flags & Flags::NO_MEMORY_USAGE) == 0 {
				Self::add_memory(info, instruction.memory_segment(), base_register, Register::None, 1, 0, MemorySize::Unknown, OpAccess::CondRead);
			}
			if (flags & Flags::NO_REGISTER_USAGE) == 0 {
				debug_assert_eq!(1, info.used_registers.len());
				info.used_registers[0] = UsedRegister { register: Register::DX, access: OpAccess::CondRead };
				Self::add_register(
					flags,
					info,
					unsafe {
						mem::transmute(
							((instruction.op1_kind() as u32).wrapping_sub(OpKind::MemorySegSI as u32) << 4).wrapping_add(Register::CX as u32) as u8,
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
				mem::transmute(((instruction.op1_kind() as u32).wrapping_sub(OpKind::MemorySegSI as u32) << 4).wrapping_add(Register::SI as u32) as u8)
			};
			if (flags & Flags::NO_MEMORY_USAGE) == 0 {
				Self::add_memory(info, instruction.memory_segment(), base_register, Register::None, 1, 0, instruction.memory_size(), OpAccess::Read);
			}
			if (flags & Flags::NO_REGISTER_USAGE) == 0 {
				Self::add_memory_segment_register(flags, info, instruction.memory_segment(), OpAccess::Read);
				Self::add_register(flags, info, base_register, OpAccess::ReadWrite);
			}
		}
	}

	fn command_movs(instruction: &Instruction, info: &mut InstructionInfo, flags: u32) {
		let mut base_register;
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
				mem::transmute(((instruction.op0_kind() as u32).wrapping_sub(OpKind::MemoryESDI as u32) << 4).wrapping_add(Register::DI as u32) as u8)
			};
			if (flags & Flags::NO_MEMORY_USAGE) == 0 {
				Self::add_memory(info, Register::ES, base_register, Register::None, 1, 0, MemorySize::Unknown, OpAccess::CondWrite);
			}
			if (flags & Flags::NO_REGISTER_USAGE) == 0 {
				Self::add_register(
					flags,
					info,
					unsafe {
						mem::transmute(
							((instruction.op0_kind() as u32).wrapping_sub(OpKind::MemoryESDI as u32) << 4).wrapping_add(Register::CX as u32) as u8,
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
				mem::transmute(((instruction.op1_kind() as u32).wrapping_sub(OpKind::MemorySegSI as u32) << 4).wrapping_add(Register::SI as u32) as u8)
			};
			if (flags & Flags::NO_MEMORY_USAGE) == 0 {
				Self::add_memory(info, instruction.memory_segment(), base_register, Register::None, 1, 0, MemorySize::Unknown, OpAccess::CondRead);
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
				mem::transmute(((instruction.op0_kind() as u32).wrapping_sub(OpKind::MemoryESDI as u32) << 4).wrapping_add(Register::DI as u32) as u8)
			};
			if (flags & Flags::NO_MEMORY_USAGE) == 0 {
				Self::add_memory(info, Register::ES, base_register, Register::None, 1, 0, instruction.memory_size(), OpAccess::Write);
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
				mem::transmute(((instruction.op1_kind() as u32).wrapping_sub(OpKind::MemorySegSI as u32) << 4).wrapping_add(Register::SI as u32) as u8)
			};
			if (flags & Flags::NO_MEMORY_USAGE) == 0 {
				Self::add_memory(info, instruction.memory_segment(), base_register, Register::None, 1, 0, instruction.memory_size(), OpAccess::Read);
			}
			if (flags & Flags::NO_REGISTER_USAGE) == 0 {
				Self::add_memory_segment_register(flags, info, instruction.memory_segment(), OpAccess::Read);
				Self::add_register(flags, info, base_register, OpAccess::ReadWrite);
			}
		}
	}

	fn command_cmps(instruction: &Instruction, info: &mut InstructionInfo, flags: u32) {
		let mut base_register;
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
				mem::transmute(((instruction.op0_kind() as u32).wrapping_sub(OpKind::MemorySegSI as u32) << 4).wrapping_add(Register::SI as u32) as u8)
			};
			if (flags & Flags::NO_MEMORY_USAGE) == 0 {
				Self::add_memory(info, instruction.memory_segment(), base_register, Register::None, 1, 0, MemorySize::Unknown, OpAccess::CondRead);
			}
			if (flags & Flags::NO_REGISTER_USAGE) == 0 {
				Self::add_register(
					flags,
					info,
					unsafe {
						mem::transmute(
							((instruction.op0_kind() as u32).wrapping_sub(OpKind::MemorySegSI as u32) << 4).wrapping_add(Register::CX as u32) as u8,
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
				mem::transmute(((instruction.op1_kind() as u32).wrapping_sub(OpKind::MemoryESDI as u32) << 4).wrapping_add(Register::DI as u32) as u8)
			};
			if (flags & Flags::NO_MEMORY_USAGE) == 0 {
				Self::add_memory(info, Register::ES, base_register, Register::None, 1, 0, MemorySize::Unknown, OpAccess::CondRead);
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
				mem::transmute(((instruction.op0_kind() as u32).wrapping_sub(OpKind::MemorySegSI as u32) << 4).wrapping_add(Register::SI as u32) as u8)
			};
			if (flags & Flags::NO_MEMORY_USAGE) == 0 {
				Self::add_memory(info, instruction.memory_segment(), base_register, Register::None, 1, 0, instruction.memory_size(), OpAccess::Read);
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
				mem::transmute(((instruction.op1_kind() as u32).wrapping_sub(OpKind::MemoryESDI as u32) << 4).wrapping_add(Register::DI as u32) as u8)
			};
			if (flags & Flags::NO_MEMORY_USAGE) == 0 {
				Self::add_memory(info, Register::ES, base_register, Register::None, 1, 0, instruction.memory_size(), OpAccess::Read);
			}
			if (flags & Flags::NO_REGISTER_USAGE) == 0 {
				if (flags & Flags::IS_64BIT) == 0 {
					Self::add_register(flags, info, Register::ES, OpAccess::Read);
				}
				Self::add_register(flags, info, base_register, OpAccess::ReadWrite);
			}
		}
	}

	fn command_stos(instruction: &Instruction, info: &mut InstructionInfo, flags: u32) {
		let base_register;
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
				mem::transmute(((instruction.op0_kind() as u32).wrapping_sub(OpKind::MemoryESDI as u32) << 4).wrapping_add(Register::DI as u32) as u8)
			};
			if (flags & Flags::NO_MEMORY_USAGE) == 0 {
				Self::add_memory(info, Register::ES, base_register, Register::None, 1, 0, MemorySize::Unknown, OpAccess::CondWrite);
			}
			if (flags & Flags::NO_REGISTER_USAGE) == 0 {
				debug_assert_eq!(1, info.used_registers.len());
				info.used_registers[0] = UsedRegister { register: info.used_registers[0].register, access: OpAccess::CondRead };
				Self::add_register(
					flags,
					info,
					unsafe {
						mem::transmute(
							((instruction.op0_kind() as u32).wrapping_sub(OpKind::MemoryESDI as u32) << 4).wrapping_add(Register::CX as u32) as u8,
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
				mem::transmute(((instruction.op0_kind() as u32).wrapping_sub(OpKind::MemoryESDI as u32) << 4).wrapping_add(Register::DI as u32) as u8)
			};
			if (flags & Flags::NO_MEMORY_USAGE) == 0 {
				Self::add_memory(info, Register::ES, base_register, Register::None, 1, 0, instruction.memory_size(), OpAccess::Write);
			}
			if (flags & Flags::NO_REGISTER_USAGE) == 0 {
				if (flags & Flags::IS_64BIT) == 0 {
					Self::add_register(flags, info, Register::ES, OpAccess::Read);
				}
				Self::add_register(flags, info, base_register, OpAccess::ReadWrite);
			}
		}
	}

	fn command_lods(instruction: &Instruction, info: &mut InstructionInfo, flags: u32) {
		let base_register;
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
				mem::transmute(((instruction.op1_kind() as u32).wrapping_sub(OpKind::MemorySegSI as u32) << 4).wrapping_add(Register::SI as u32) as u8)
			};
			if (flags & Flags::NO_MEMORY_USAGE) == 0 {
				Self::add_memory(info, instruction.memory_segment(), base_register, Register::None, 1, 0, MemorySize::Unknown, OpAccess::CondRead);
			}
			if (flags & Flags::NO_REGISTER_USAGE) == 0 {
				debug_assert_eq!(1, info.used_registers.len());
				info.used_registers[0] = UsedRegister { register: info.used_registers[0].register, access: OpAccess::CondWrite };
				Self::add_register(
					flags,
					info,
					unsafe {
						mem::transmute(
							((instruction.op1_kind() as u32).wrapping_sub(OpKind::MemorySegSI as u32) << 4).wrapping_add(Register::CX as u32) as u8,
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
				mem::transmute(((instruction.op1_kind() as u32).wrapping_sub(OpKind::MemorySegSI as u32) << 4).wrapping_add(Register::SI as u32) as u8)
			};
			if (flags & Flags::NO_MEMORY_USAGE) == 0 {
				Self::add_memory(info, instruction.memory_segment(), base_register, Register::None, 1, 0, instruction.memory_size(), OpAccess::Read);
			}
			if (flags & Flags::NO_REGISTER_USAGE) == 0 {
				Self::add_memory_segment_register(flags, info, instruction.memory_segment(), OpAccess::Read);
				Self::add_register(flags, info, base_register, OpAccess::ReadWrite);
			}
		}
	}

	fn command_scas(instruction: &Instruction, info: &mut InstructionInfo, flags: u32) {
		let base_register;
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
				mem::transmute(((instruction.op1_kind() as u32).wrapping_sub(OpKind::MemoryESDI as u32) << 4).wrapping_add(Register::DI as u32) as u8)
			};
			if (flags & Flags::NO_MEMORY_USAGE) == 0 {
				Self::add_memory(info, Register::ES, base_register, Register::None, 1, 0, MemorySize::Unknown, OpAccess::CondRead);
			}
			if (flags & Flags::NO_REGISTER_USAGE) == 0 {
				debug_assert_eq!(1, info.used_registers.len());
				info.used_registers[0] = UsedRegister { register: info.used_registers[0].register, access: OpAccess::CondRead };
				Self::add_register(
					flags,
					info,
					unsafe {
						mem::transmute(
							((instruction.op1_kind() as u32).wrapping_sub(OpKind::MemoryESDI as u32) << 4).wrapping_add(Register::CX as u32) as u8,
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
				mem::transmute(((instruction.op1_kind() as u32).wrapping_sub(OpKind::MemoryESDI as u32) << 4).wrapping_add(Register::DI as u32) as u8)
			};
			if (flags & Flags::NO_MEMORY_USAGE) == 0 {
				Self::add_memory(info, Register::ES, base_register, Register::None, 1, 0, instruction.memory_size(), OpAccess::Read);
			}
			if (flags & Flags::NO_REGISTER_USAGE) == 0 {
				if (flags & Flags::IS_64BIT) == 0 {
					Self::add_register(flags, info, Register::ES, OpAccess::Read);
				}
				Self::add_register(flags, info, base_register, OpAccess::ReadWrite);
			}
		}
	}

	fn command_xstore(instruction: &Instruction, info: &mut InstructionInfo, flags: u32) {
		let reg_index;
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
				Self::add_register(flags, info, unsafe { mem::transmute((Register::DI as u32).wrapping_add(reg_index) as u8) }, OpAccess::CondRead);
				Self::add_register(flags, info, unsafe { mem::transmute((Register::DI as u32).wrapping_add(reg_index) as u8) }, OpAccess::CondWrite);
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
				Self::add_register(flags, info, unsafe { mem::transmute((Register::DI as u32).wrapping_add(reg_index) as u8) }, OpAccess::ReadWrite);
				Self::add_register(flags, info, Register::EAX, OpAccess::Write);
				Self::add_register(flags, info, Register::EDX, OpAccess::Read);
			}
		}
	}

	fn command_enter(instruction: &Instruction, info: &mut InstructionInfo, flags: u32, op_size: u32) {
		let mut xsp_mask = 0;
		let xsp = Self::get_xsp(instruction.code_size(), &mut xsp_mask);
		if (flags & Flags::NO_REGISTER_USAGE) == 0 {
			if (flags & Flags::IS_64BIT) == 0 {
				Self::add_register(flags, info, Register::SS, OpAccess::Read);
			}
			Self::add_register(flags, info, xsp, OpAccess::ReadWrite);
		}

		let (memory_size, r_sp) = if op_size == 8 {
			(MemorySize::UInt64, Register::RSP)
		} else if op_size == 4 {
			(MemorySize::UInt32, Register::ESP)
		} else {
			debug_assert_eq!(2, op_size);
			(MemorySize::UInt16, Register::SP)
		};

		if r_sp != xsp && (flags & Flags::NO_REGISTER_USAGE) == 0 {
			Self::add_register(flags, info, r_sp, OpAccess::ReadWrite);
		}

		let nesting_level = (instruction.immediate8_2nd() & 0x1F) as u64;
		let mut xsp_offset = 0u64;
		// push rBP
		if (flags & Flags::NO_REGISTER_USAGE) == 0 {
			Self::add_register(flags, info, unsafe { mem::transmute((r_sp as u32).wrapping_add(1) as u8) }, OpAccess::ReadWrite);
		}
		if (flags & Flags::NO_MEMORY_USAGE) == 0 {
			xsp_offset = xsp_offset.wrapping_sub(op_size as u64);
			Self::add_memory(info, Register::SS, xsp, Register::None, 1, xsp_offset & xsp_mask, memory_size, OpAccess::Write);
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
					xbp_offset = xbp_offset.wrapping_sub(op_size as u64);
					Self::add_memory(info, Register::SS, xbp, Register::None, 1, xbp_offset & xsp_mask, memory_size, OpAccess::Read);
					xsp_offset = xsp_offset.wrapping_sub(op_size as u64);
					Self::add_memory(info, Register::SS, xsp, Register::None, 1, xsp_offset & xsp_mask, memory_size, OpAccess::Write);
				}
			}
			// push frameTemp
			if (flags & Flags::NO_MEMORY_USAGE) == 0 {
				xsp_offset = xsp_offset.wrapping_sub(op_size as u64);
				Self::add_memory(info, Register::SS, xsp, Register::None, 1, xsp_offset & xsp_mask, memory_size, OpAccess::Write);
			}
		}
	}

	fn command_leave(instruction: &Instruction, info: &mut InstructionInfo, flags: u32, op_size: u32) {
		let mut xsp_mask = 0;
		let xsp = Self::get_xsp(instruction.code_size(), &mut xsp_mask);
		if (flags & Flags::NO_REGISTER_USAGE) == 0 {
			if (flags & Flags::IS_64BIT) == 0 {
				Self::add_register(flags, info, Register::SS, OpAccess::Read);
			}
			Self::add_register(flags, info, xsp, OpAccess::Write);
		}

		if op_size == 8 {
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
		} else if op_size == 4 {
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
			debug_assert_eq!(2, op_size);
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

	fn command_shift_mask_mod(instruction: &Instruction, info: &mut InstructionInfo, modulus: u32) {
		match ((instruction.immediate8() as u32) & 0x1F) % modulus {
			0 => info.rflags_info = RflagsInfo::None as usize,
			1 => info.rflags_info = RflagsInfo::R_c_W_co as usize,
			_ => {}
		}
	}

	fn command_shift_mask(instruction: &Instruction, info: &mut InstructionInfo, mask: u32) {
		match (instruction.immediate8() as u32) & mask {
			0 => info.rflags_info = RflagsInfo::None as usize,
			1 => {
				if info.rflags_info == RflagsInfo::W_c_U_o as usize {
					info.rflags_info = RflagsInfo::W_co as usize;
				} else if info.rflags_info == RflagsInfo::R_c_W_c_U_o as usize {
					info.rflags_info = RflagsInfo::R_c_W_co as usize;
				} else {
					debug_assert_eq!(RflagsInfo::W_cpsz_U_ao as usize, info.rflags_info);
					info.rflags_info = RflagsInfo::W_copsz_U_a as usize;
				}
			}
			_ => {}
		}
	}

	fn command_clear_rflags(instruction: &Instruction, info: &mut InstructionInfo, flags: u32) {
		if instruction.op0_register() == instruction.op1_register()
			&& instruction.op0_kind() == OpKind::Register
			&& instruction.op1_kind() == OpKind::Register
		{
			info.op_accesses[0] = OpAccess::Write;
			info.op_accesses[1] = OpAccess::None;
			if instruction.code().mnemonic() == Mnemonic::Xor {
				info.rflags_info = RflagsInfo::C_cos_S_pz_U_a as usize;
			} else {
				info.rflags_info = RflagsInfo::C_acos_S_pz as usize;
			}
			if (flags & Flags::NO_REGISTER_USAGE) == 0 {
				debug_assert!(info.used_registers.len() == 2 || info.used_registers.len() == 3);
				info.used_registers.clear();
				Self::add_register(flags, info, instruction.op0_register(), OpAccess::Write);
			}
		}
	}

	fn command_clear_reg_regmem(instruction: &Instruction, info: &mut InstructionInfo, flags: u32) {
		if instruction.op0_register() == instruction.op1_register() && instruction.op1_kind() == OpKind::Register {
			info.op_accesses[0] = OpAccess::Write;
			info.op_accesses[1] = OpAccess::None;
			if (flags & Flags::NO_REGISTER_USAGE) == 0 {
				debug_assert!(info.used_registers.len() == 2 || info.used_registers.len() == 3);
				info.used_registers.clear();
				info.used_registers.push(UsedRegister { register: instruction.op0_register(), access: OpAccess::Write });
			}
		}
	}

	fn command_clear_reg_reg_regmem(instruction: &Instruction, info: &mut InstructionInfo, flags: u32) {
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

	fn command_arpl(instruction: &Instruction, info: &mut InstructionInfo, flags: u32) {
		if (flags & Flags::NO_REGISTER_USAGE) == 0 {
			debug_assert!(!info.used_registers.is_empty());
			// Skip memory operand, if any
			let start_index = if instruction.op0_kind() == OpKind::Register { 0 } else { info.used_registers.len() - 1 };
			for info in &mut info.used_registers[start_index..] {
				let mut index = Self::try_get_gpr_16_32_64_index(info.register);
				if index >= 4 {
					index += 4; // Skip AH, CH, DH, BH
				}
				if index >= 0 {
					info.register = unsafe { mem::transmute((Register::AL as u32).wrapping_add(index as u32) as u8) };
				}
			}
		}
	}

	fn command_last_gpr(instruction: &Instruction, info: &mut InstructionInfo, flags: u32, base_reg: Register) {
		if (flags & Flags::NO_REGISTER_USAGE) == 0 {
			const N: usize = 1;
			let op_count = instruction.op_count();
			let imm_count = if instruction.op_kind(op_count - 1) == OpKind::Immediate8 { 1 } else { 0 };
			let op_index = instruction.op_count() - N as u32 - imm_count;
			if instruction.op_kind(op_index) == OpKind::Register {
				debug_assert!(info.used_registers.len() >= N);
				debug_assert_eq!(instruction.op_register(op_index), info.used_registers[info.used_registers.len() - N].register);
				debug_assert_eq!(OpAccess::Read, info.used_registers[info.used_registers.len() - N].access);
				let mut index = Self::try_get_gpr_16_32_64_index(instruction.op_register(op_index));
				if index >= 4 && base_reg == Register::AL {
					index += 4; // Skip AH, CH, DH, BH
				}
				if index >= 0 {
					let regs_index = info.used_registers.len() - N;
					info.used_registers[regs_index] = UsedRegister {
						register: unsafe { mem::transmute((base_reg as u32).wrapping_add(index as u32) as u8) },
						access: OpAccess::Read,
					};
				}
			}
		}
	}

	fn command_lea(instruction: &Instruction, info: &mut InstructionInfo, flags: u32) {
		if (flags & Flags::NO_REGISTER_USAGE) == 0 {
			debug_assert!(!info.used_registers.is_empty());
			debug_assert_eq!(OpKind::Register, instruction.op0_kind());
			let reg = instruction.op0_register();
			for reg_info in info.used_registers.iter_mut().skip(1) {
				if reg >= Register::EAX && reg <= Register::R15D {
					if reg_info.register() >= Register::RAX && reg_info.register() <= Register::R15 {
						reg_info.register = unsafe {
							mem::transmute((reg_info.register as u32).wrapping_sub(Register::RAX as u32).wrapping_add(Register::EAX as u32) as u8)
						};
					}
				} else if reg >= Register::AX && reg <= Register::R15W {
					if reg_info.register() >= Register::EAX && reg_info.register() <= Register::R15 {
						reg_info.register = unsafe {
							mem::transmute(
								((reg_info.register as u32).wrapping_sub(Register::EAX as u32) & 0xF).wrapping_add(Register::AX as u32) as u8
							)
						};
					}
				} else {
					debug_assert!(reg >= Register::RAX && reg <= Register::R15);
					break;
				}
			}
		}
	}

	fn command_emmi(instruction: &Instruction, info: &mut InstructionInfo, flags: u32, op_access: OpAccess) {
		if (flags & Flags::NO_REGISTER_USAGE) == 0 {
			if instruction.op0_kind() == OpKind::Register {
				let mut reg = instruction.op0_register();
				if reg >= Register::MM0 && reg <= Register::MM7 {
					reg = unsafe { mem::transmute((((reg as u32 - Register::MM0 as u32) ^ 1) + Register::MM0 as u32) as u8) };
					Self::add_register(flags, info, reg, op_access);
				}
			}
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
			info.used_registers.push(UsedRegister { register: reg, access: OpAccess::Read });
			info.used_registers.push(UsedRegister {
				register: write_reg,
				access: if access == OpAccess::ReadWrite { OpAccess::Write } else { OpAccess::CondWrite },
			});
		}
	}
}
