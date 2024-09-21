use super::enums::{
	ImpliedAccess, InfoFlags1 as InfoFlags1Consts, InfoFlags2 as InfoFlags2Consts, OpInfo0, OpInfo1, OpInfo2, OpInfo3, OpInfo4, OP_ACCESS_1,
	OP_ACCESS_2,
};
use super::{Code, EncodingKind, OpAccess};
use crate::info_table::TABLE;
use core::mem;

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

	pub const fn op0_access(&self) -> OpAccess {
		let op0_access = match self.op0_info() {
			OpInfo0::None => OpAccess::None,
			OpInfo0::Read => OpAccess::Read,
			OpInfo0::Write => OpAccess::Write,
			OpInfo0::WriteVmm => OpAccess::Write,
			OpInfo0::WriteForce | OpInfo0::WriteForceP1 => OpAccess::Write,
			OpInfo0::CondWrite => OpAccess::CondWrite,

			OpInfo0::CondWrite32_ReadWrite64 => {
				todo!()
				/*
				if (flags & Flags::IS_64BIT) != 0 {
					OpAccess::ReadWrite
				} else {
					OpAccess::CondWrite
				}
				*/
			}

			OpInfo0::ReadWrite => OpAccess::ReadWrite,

			OpInfo0::ReadWriteVmm => OpAccess::ReadWrite,
			OpInfo0::ReadCondWrite => OpAccess::ReadCondWrite,
			OpInfo0::NoMemAccess => OpAccess::NoMemAccess,

			OpInfo0::WriteMem_ReadWriteReg => {
				todo!()
				/*
				if instruction_internal::internal_op0_is_not_reg_or_op1_is_not_reg(instruction) {
					OpAccess::Write
				} else {
					OpAccess::ReadWrite
				}
				*/
			}
		};
		op0_access
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
