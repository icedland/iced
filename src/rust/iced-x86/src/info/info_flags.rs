use super::enums::{
	ImpliedAccess, InfoFlags1 as InfoFlags1Consts, InfoFlags2 as InfoFlags2Consts, OpInfo0, OpInfo1, OpInfo2, OpInfo3, OpInfo4, OP_ACCESS_1,
	OP_ACCESS_2,
};
use super::{Code, EncodingKind, OpAccess};
use crate::info_table::TABLE;
use core::{fmt, mem};

pub(crate) const fn code_info_flags(code: Code) -> &'static (InfoFlags1, InfoFlags2) {
	// SAFETY: info_table::TABLE has a generated entry for each Code
	let u32_tuple: &'static (u32, u32) = unsafe { &*TABLE.as_ptr().offset(code as isize) };
	// SAFETY: Creating InfoFlags1 and InfoFlags2 from the table elements is safe since the
	// generator only generates valid flags
	unsafe { mem::transmute(u32_tuple) }
}

#[repr(transparent)]
#[derive(Clone, Copy)]
pub(crate) struct InfoFlags2(u32);
impl InfoFlags2 {
	pub(crate) const fn encoding_kind(&self) -> EncodingKind {
		// SAFETY: safety must be guaranteed when calling the constructor
		unsafe { mem::transmute(((self.0 >> InfoFlags2Consts::ENCODING_SHIFT) & InfoFlags2Consts::ENCODING_MASK) as u8) }
	}
}
#[repr(transparent)]
#[derive(Clone, Copy)]
pub(crate) struct InfoFlags1(u32);
impl InfoFlags1 {
	pub(crate) const fn op_mask_read_write(&self) -> bool {
		(self.0 & InfoFlags1Consts::OP_MASK_READ_WRITE) != 0
	}

	pub(crate) const fn implied_access(&self) -> ImpliedAccess {
		// SAFETY: safety must be guaranteed when calling the constructor
		unsafe { mem::transmute(((self.0 >> InfoFlags1Consts::IMPLIED_ACCESS_SHIFT) & InfoFlags1Consts::IMPLIED_ACCESS_MASK) as u8) }
	}

	pub(crate) const fn ignores_index_va(&self) -> bool {
		self.0 & InfoFlags1Consts::IGNORES_INDEX_VA != 0
	}

	pub(crate) const fn sign_mask(&self) -> u32 {
		(self.0 as i32 >> 31) as u32
	}

	pub(crate) const fn op0_info(&self) -> OpInfo0 {
		// SAFETY: safety must be guaranteed when calling the constructor
		unsafe { mem::transmute(((self.0 >> InfoFlags1Consts::OP_INFO0_SHIFT) & InfoFlags1Consts::OP_INFO0_MASK) as u8) }
	}

	pub(crate) const fn op1_info(&self) -> OpInfo1 {
		// SAFETY: safety must be guaranteed when calling the constructor
		unsafe { mem::transmute(((self.0.wrapping_shr(InfoFlags1Consts::OP_INFO1_SHIFT)) & InfoFlags1Consts::OP_INFO1_MASK) as u8) }
	}

	pub(crate) const fn op2_info(&self) -> OpInfo2 {
		// SAFETY: safety must be guaranteed when calling the constructor
		unsafe { mem::transmute(((self.0.wrapping_shr(InfoFlags1Consts::OP_INFO2_SHIFT)) & InfoFlags1Consts::OP_INFO2_MASK) as u8) }
	}

	pub(crate) const fn op3_info(&self) -> OpInfo3 {
		// SAFETY: safety must be guaranteed when calling the constructor
		unsafe { mem::transmute((self.0 & ((InfoFlags1Consts::OP_INFO3_MASK) << InfoFlags1Consts::OP_INFO3_SHIFT)) != 0) }
	}

	pub(crate) const fn op4_info(&self) -> OpInfo4 {
		// SAFETY: safety must be guaranteed when calling the constructor
		unsafe { mem::transmute((self.0 & ((InfoFlags1Consts::OP_INFO4_MASK) << InfoFlags1Consts::OP_INFO4_SHIFT)) != 0) }
	}

	pub const fn op0_access(&self, config: OpAccessOptions) -> OpAccess {
		match self.op0_info() {
			OpInfo0::None => OpAccess::None,
			OpInfo0::Read => OpAccess::Read,
			OpInfo0::Write => OpAccess::Write,
			OpInfo0::WriteVmm => OpAccess::Write,
			OpInfo0::WriteForce | OpInfo0::WriteForceP1 => OpAccess::Write,
			OpInfo0::CondWrite => OpAccess::CondWrite,

			// Codes having this OpInfo0:
			// Cmovo_r32_rm32
			// Cmovno_r32_rm32
			// Cmovb_r32_rm32
			// Cmovae_r32_rm32
			// Cmove_r32_rm32
			// Cmovne_r32_rm32
			// Cmovbe_r32_rm32
			// Cmova_r32_rm32
			// Cmovs_r32_rm32
			// Cmovns_r32_rm32
			// Cmovp_r32_rm32
			// Cmovnp_r32_rm32
			// Cmovl_r32_rm32
			// Cmovge_r32_rm32
			// Cmovle_r32_rm32
			// Cmovg_r32_rm32
			OpInfo0::CondWrite32_ReadWrite64 => {
				if config.is_64_set() {
					OpAccess::ReadWrite
				} else {
					OpAccess::CondWrite
				}
			}
			OpInfo0::ReadWrite => OpAccess::ReadWrite,
			OpInfo0::ReadWriteVmm => OpAccess::ReadWrite,
			OpInfo0::ReadCondWrite => OpAccess::ReadCondWrite,
			OpInfo0::NoMemAccess => OpAccess::NoMemAccess,

			// Codes having this OpInfo0:
			// Movss_xmm_xmmm32
			// Movsd_xmm_xmmm64
			// Movss_xmmm32_xmm
			// Movsd_xmmm64_xmm
			//
			// Relevant part from the intel manual for movss
			// "Legacy version: When the source and destination operands are XMM registers, bits (MAXVL-1:32) of the corresponding destination register are unmodified. When the source operand is a memory location and destination operand is an XMM registers, Bits (127:32) of the destination operand is cleared to all 0s, bits MAXVL:128 of the destination operand remains unchanged."
			// When the operands are availale it is possible to decide whether this is
			// `OpAccess::Write` or `OpAccess::ReadWrite` here we return the more general one.
			OpInfo0::WriteMem_ReadWriteReg => {
				if config.has_memory_operand_set() {
					OpAccess::Write
				} else {
					OpAccess::ReadWrite
				}
			}
		}
	}

	pub const fn op1_access(&self) -> OpAccess {
		OP_ACCESS_1[self.op1_info() as usize]
	}

	pub const fn op2_access(&self) -> OpAccess {
		OP_ACCESS_2[self.op2_info() as usize]
	}

	pub const fn op3_access(&self) -> OpAccess {
		match self.op3_info() {
			OpInfo3::Read => OpAccess::Read,
			OpInfo3::None => OpAccess::None,
		}
	}

	pub const fn op4_access(&self) -> OpAccess {
		match self.op4_info() {
			OpInfo4::Read => OpAccess::Read,
			OpInfo4::None => OpAccess::None,
		}
	}
}

/// Controls behaviour of [`Code::op_access`] functions.
#[derive(Default, Clone, Copy)]
pub struct OpAccessOptions(u8);

impl fmt::Debug for OpAccessOptions {
	#[allow(clippy::missing_inline_in_public_items)]
	fn fmt(&self, f: &mut fmt::Formatter<'_>) -> fmt::Result {
		f.debug_struct("OpAccessOptions").field("has_memory_operand", &self.has_memory_operand_set()).field("is_64", &self.is_64_set()).finish()
	}
}

impl OpAccessOptions {
	const HAS_MEMORY_OPERAND_SHIFT: u8 = 0;
	const IS_64_SHIFT: u8 = 1;

	const HAS_MEMORY_OPERAND_MASK: u8 = 1u8 << Self::HAS_MEMORY_OPERAND_SHIFT;
	const IS_64_MASK: u8 = 1u8 << Self::IS_64_SHIFT;

	/// Create a new `OpAccessOptions` with options set to `false`
	#[must_use]
	#[inline]
	pub const fn new() -> Self {
		Self(0)
	}

	/// Set whether it should be assumed that there is at least one memory operand when calculating operand access. Currently this
	/// affects operand 0 of the following [`Code`]-s:
	///
	/// ```
	/// # use iced_x86::Code;
	/// # use iced_x86::Code::*;
	/// # use iced_x86::OpAccessOptions;
	/// #
	/// let affected_codes = [
	///     Movss_xmm_xmmm32, Movsd_xmm_xmmm64, Movss_xmmm32_xmm, Movsd_xmmm64_xmm,
	/// ];
	/// let config = OpAccessOptions::default();
	/// let config_memory = OpAccessOptions::default().has_memory_operand(true);
	/// for i in 0..=4 {
	///     for code in Code::values() {
	///         if affected_codes.contains(&code) && i == 0 {
	///             assert_ne!(code.op_access(i, config), code.op_access(i, config_memory));
	///         } else {
	///             assert_eq!(code.op_access(i, config), code.op_access(i, config_memory));
	///         }
	///     }
	/// }
	/// ```
	///
	/// Relevant part from the intel manual for `movss`:
	/// > Legacy version: When the source and destination operands are XMM registers, bits (MAXVL-1:32) of the corresponding destination register are unmodified. When the source operand is a memory location and destination operand is an XMM registers, Bits (127:32) of the destination operand is cleared to all 0s, bits MAXVL:128 of the destination operand remains unchanged.
	///
	/// When there are only register operands, operand 0 has `OpAccess::ReadWrite`. If there is a memory operand, the access is `OpAccess::Write`
	#[must_use]
	#[inline]
	pub const fn has_memory_operand(self, value: bool) -> Self {
		if value {
			Self(self.0 | Self::HAS_MEMORY_OPERAND_MASK)
		} else {
			Self(self.0 & !Self::HAS_MEMORY_OPERAND_MASK)
		}
	}

	/// Set whether the bitness should be 64 when calculating operand access. Currently this
	/// affects operand 0 the following [`Code`]-s:
	/// ```
	/// # use iced_x86::Code;
	/// # use iced_x86::Code::*;
	/// # use iced_x86::OpAccessOptions;
	/// #
	/// let affected_codes = [
	///     Cmovo_r32_rm32, Cmovno_r32_rm32, Cmovb_r32_rm32, Cmovae_r32_rm32,
	///     Cmove_r32_rm32, Cmovne_r32_rm32, Cmovbe_r32_rm32, Cmova_r32_rm32,
	///     Cmovs_r32_rm32, Cmovns_r32_rm32, Cmovp_r32_rm32, Cmovnp_r32_rm32,
	///     Cmovl_r32_rm32, Cmovge_r32_rm32, Cmovle_r32_rm32, Cmovg_r32_rm32,
	/// ];
	/// let config = OpAccessOptions::default();
	/// let config_64 = OpAccessOptions::default().is_64(true);
	/// for i in 0..=4 {
	///     for code in Code::values() {
	///         if affected_codes.contains(&code) && i == 0 {
	///             assert_ne!(code.op_access(i, config), code.op_access(i, config_64));
	///         } else {
	///             assert_eq!(code.op_access(i, config), code.op_access(i, config_64));
	///         }
	///     }
	/// }
	/// ```
	///
	/// In 64 bit mode [`OpAccess::ReadWrite`] is returned, since the upper 32 bits are always
	/// zeroed. In other modes [`OpAccess::CondWrite`] is returned.
	#[must_use]
	#[inline]
	pub const fn is_64(self, value: bool) -> Self {
		if value {
			Self(self.0 | Self::IS_64_MASK)
		} else {
			Self(self.0 & !Self::IS_64_MASK)
		}
	}

	const fn has_memory_operand_set(&self) -> bool {
		self.0 & Self::HAS_MEMORY_OPERAND_MASK != 0
	}

	const fn is_64_set(&self) -> bool {
		self.0 & Self::IS_64_MASK != 0
	}
}
