// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::encoder::enums::*;
use crate::encoder::ops::*;
use crate::encoder::ops_tables::*;
use crate::encoder::*;
#[cfg(feature = "mvex")]
use crate::mvex::get_mvex_info;
#[cfg(any(not(feature = "no_evex"), feature = "mvex"))]
use crate::tuple_type_tbl::get_disp8n;
use crate::*;
use alloc::boxed::Box;
use alloc::vec::Vec;
use core::mem;

// SAFETY:
//	code: let this = unsafe { &*(self_ptr as *const Self) };
// The first arg (`self_ptr`) to encode() is always the handler itself, cast to a `*const OpCodeHandler`.
// All handlers are `#[repr(C)]` structs so the OpCodeHandler fields are always at the same offsets.

#[repr(C)]
pub(crate) struct OpCodeHandler {
	pub(super) encode: fn(self_ptr: *const OpCodeHandler, encoder: &mut Encoder, instruction: &Instruction),
	pub(super) try_convert_to_disp8n:
		Option<fn(self_ptr: *const OpCodeHandler, encoder: &mut Encoder, instruction: &Instruction, displ: i32) -> Option<i8>>,
	pub(crate) operands: Box<[&'static (dyn Op + Sync)]>,
	pub(super) op_code: u32,
	pub(super) group_index: i32,
	pub(super) rm_group_index: i32,
	pub(super) enc_flags3: u32, // EncFlags3
	pub(super) op_size: CodeSize,
	pub(super) addr_size: CodeSize,
	pub(super) is_2byte_opcode: bool,
	pub(super) is_special_instr: bool,
}

#[repr(C)]
pub(super) struct InvalidHandler {
	pub(super) base: OpCodeHandler,
}

impl InvalidHandler {
	pub(super) fn new() -> Self {
		Self {
			base: OpCodeHandler {
				encode: Self::encode,
				try_convert_to_disp8n: None,
				operands: Box::new([]),
				op_code: 0,
				group_index: -1,
				rm_group_index: -1,
				enc_flags3: EncFlags3::NONE,
				op_size: CodeSize::Unknown,
				addr_size: CodeSize::Unknown,
				is_2byte_opcode: false,
				is_special_instr: false,
			},
		}
	}

	pub(super) const ERROR_MESSAGE: &'static str = "Can't encode an invalid instruction";

	fn encode(_self_ptr: *const OpCodeHandler, encoder: &mut Encoder, _instruction: &Instruction) {
		encoder.set_error_message_str(Self::ERROR_MESSAGE);
	}
}

#[repr(C)]
pub(super) struct DeclareDataHandler {
	base: OpCodeHandler,
	elem_size: u32,
}

impl DeclareDataHandler {
	pub(super) fn new(code: Code) -> Self {
		Self {
			base: OpCodeHandler {
				encode: Self::encode,
				try_convert_to_disp8n: None,
				operands: Box::new([]),
				op_code: 0,
				group_index: -1,
				rm_group_index: -1,
				enc_flags3: EncFlags3::NONE,
				op_size: CodeSize::Unknown,
				addr_size: CodeSize::Unknown,
				is_2byte_opcode: false,
				is_special_instr: true,
			},
			elem_size: match code {
				Code::DeclareByte => 1,
				Code::DeclareWord => 2,
				Code::DeclareDword => 4,
				Code::DeclareQword => 8,
				_ => unreachable!(),
			},
		}
	}

	fn encode(self_ptr: *const OpCodeHandler, encoder: &mut Encoder, instruction: &Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		let length = instruction.declare_data_len() * this.elem_size as usize;
		for i in 0..length {
			match instruction.try_get_declare_byte_value(i) {
				Ok(value) => encoder.write_byte_internal(value as u32),
				Err(_) => {
					encoder.set_error_message_str("Invalid db/dw/dd/dq data length");
					return;
				}
			}
		}
	}
}

#[repr(C)]
pub(super) struct ZeroBytesHandler {
	base: OpCodeHandler,
}

impl ZeroBytesHandler {
	pub(super) fn new(_code: Code) -> Self {
		Self {
			base: OpCodeHandler {
				encode: Self::encode,
				try_convert_to_disp8n: None,
				operands: Box::new([]),
				op_code: 0,
				group_index: -1,
				rm_group_index: -1,
				enc_flags3: EncFlags3::NONE,
				op_size: CodeSize::Unknown,
				addr_size: CodeSize::Unknown,
				is_2byte_opcode: false,
				is_special_instr: true,
			},
		}
	}

	fn encode(_self_ptr: *const OpCodeHandler, _encoder: &mut Encoder, _instruction: &Instruction) {}
}

const fn get_op_code(enc_flags2: u32) -> u32 {
	(enc_flags2 >> EncFlags2::OP_CODE_SHIFT) as u16 as u32
}

#[repr(C)]
pub(super) struct LegacyHandler {
	base: OpCodeHandler,
	table_byte1: u32,
	table_byte2: u32,
	mandatory_prefix: u32,
}

impl LegacyHandler {
	pub(super) fn new(enc_flags1: u32, enc_flags2: u32, enc_flags3: u32) -> Self {
		let group_index = if (enc_flags2 & EncFlags2::HAS_GROUP_INDEX) == 0 { -1 } else { ((enc_flags2 >> EncFlags2::GROUP_INDEX_SHIFT) & 7) as i32 };
		let rm_group_index =
			if (enc_flags3 & EncFlags3::HAS_RM_GROUP_INDEX) == 0 { -1 } else { ((enc_flags2 >> EncFlags2::GROUP_INDEX_SHIFT) & 7) as i32 };
		// SAFETY: generated data is valid
		let table: LegacyOpCodeTable = unsafe { mem::transmute(((enc_flags2 >> EncFlags2::TABLE_SHIFT) & EncFlags2::TABLE_MASK) as u8) };
		let (table_byte1, table_byte2) = match table {
			LegacyOpCodeTable::MAP0 => (0, 0),
			LegacyOpCodeTable::MAP0F => (0x0F, 0),
			LegacyOpCodeTable::MAP0F38 => (0x0F, 0x38),
			LegacyOpCodeTable::MAP0F3A => (0x0F, 0x3A),
		};
		// SAFETY: generated data is valid
		let mpb: MandatoryPrefixByte =
			unsafe { mem::transmute(((enc_flags2 >> EncFlags2::MANDATORY_PREFIX_SHIFT) & EncFlags2::MANDATORY_PREFIX_MASK) as u8) };
		let mandatory_prefix = match mpb {
			MandatoryPrefixByte::None => 0,
			MandatoryPrefixByte::P66 => 0x66,
			MandatoryPrefixByte::PF3 => 0xF3,
			MandatoryPrefixByte::PF2 => 0xF2,
		};

		let op0 = ((enc_flags1 >> EncFlags1::LEGACY_OP0_SHIFT) & EncFlags1::LEGACY_OP_MASK) as usize;
		let op1 = ((enc_flags1 >> EncFlags1::LEGACY_OP1_SHIFT) & EncFlags1::LEGACY_OP_MASK) as usize;
		let op2 = ((enc_flags1 >> EncFlags1::LEGACY_OP2_SHIFT) & EncFlags1::LEGACY_OP_MASK) as usize;
		let op3 = ((enc_flags1 >> EncFlags1::LEGACY_OP3_SHIFT) & EncFlags1::LEGACY_OP_MASK) as usize;
		let operands = if op3 != 0 {
			vec![LEGACY_TABLE[op0], LEGACY_TABLE[op1], LEGACY_TABLE[op2], LEGACY_TABLE[op3]]
		} else if op2 != 0 {
			debug_assert_eq!(op3, 0);
			vec![LEGACY_TABLE[op0], LEGACY_TABLE[op1], LEGACY_TABLE[op2]]
		} else if op1 != 0 {
			debug_assert_eq!(op2, 0);
			debug_assert_eq!(op3, 0);
			vec![LEGACY_TABLE[op0], LEGACY_TABLE[op1]]
		} else if op0 != 0 {
			debug_assert_eq!(op1, 0);
			debug_assert_eq!(op2, 0);
			debug_assert_eq!(op3, 0);
			vec![LEGACY_TABLE[op0]]
		} else {
			debug_assert_eq!(op0, 0);
			debug_assert_eq!(op1, 0);
			debug_assert_eq!(op2, 0);
			debug_assert_eq!(op3, 0);
			Vec::new()
		};

		Self {
			base: OpCodeHandler {
				encode: Self::encode,
				try_convert_to_disp8n: None,
				operands: operands.into_boxed_slice(),
				op_code: get_op_code(enc_flags2),
				group_index,
				rm_group_index,
				enc_flags3,
				// SAFETY: generated data is valid
				op_size: unsafe {
					mem::transmute(((enc_flags3 >> EncFlags3::OPERAND_SIZE_SHIFT) & EncFlags3::OPERAND_SIZE_MASK) as CodeSizeUnderlyingType)
				},
				// SAFETY: generated data is valid
				addr_size: unsafe {
					mem::transmute(((enc_flags3 >> EncFlags3::ADDRESS_SIZE_SHIFT) & EncFlags3::ADDRESS_SIZE_MASK) as CodeSizeUnderlyingType)
				},
				is_2byte_opcode: (enc_flags2 & EncFlags2::OP_CODE_IS2_BYTES) != 0,
				is_special_instr: false,
			},
			table_byte1,
			table_byte2,
			mandatory_prefix,
		}
	}

	fn encode(self_ptr: *const OpCodeHandler, encoder: &mut Encoder, instruction: &Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		let mut b = this.mandatory_prefix;
		encoder.write_prefixes(instruction, b != 0xF3);
		if b != 0 {
			encoder.write_byte_internal(b);
		}

		const _: () = assert!(EncoderFlags::B == 0x01);
		const _: () = assert!(EncoderFlags::X == 0x02);
		const _: () = assert!(EncoderFlags::R == 0x04);
		const _: () = assert!(EncoderFlags::W == 0x08);
		const _: () = assert!(EncoderFlags::REX == 0x40);
		b = encoder.encoder_flags;
		b &= 0x4F;
		if b != 0 {
			if (encoder.encoder_flags & EncoderFlags::HIGH_LEGACY_8_BIT_REGS) != 0 {
				encoder.set_error_message_str(
					"Registers AH, CH, DH, BH can't be used if there's a REX prefix. Use AL, CL, DL, BL, SPL, BPL, SIL, DIL, R8L-R15L instead.",
				);
			}
			b |= 0x40;
			encoder.write_byte_internal(b);
		}

		b = this.table_byte1;
		if b != 0 {
			encoder.write_byte_internal(b);
			b = this.table_byte2;
			if b != 0 {
				encoder.write_byte_internal(b);
			}
		}
	}
}

#[cfg(not(feature = "no_vex"))]
#[repr(C)]
pub(super) struct VexHandler {
	base: OpCodeHandler,
	table: u32,
	last_byte: u32,
	mask_w_l: u32,
	mask_l: u32,
	w1: u32,
}

#[cfg(not(feature = "no_vex"))]
impl VexHandler {
	pub(super) fn new(enc_flags1: u32, enc_flags2: u32, enc_flags3: u32) -> Self {
		let group_index = if (enc_flags2 & EncFlags2::HAS_GROUP_INDEX) == 0 { -1 } else { ((enc_flags2 >> EncFlags2::GROUP_INDEX_SHIFT) & 7) as i32 };
		let rm_group_index =
			if (enc_flags3 & EncFlags3::HAS_RM_GROUP_INDEX) == 0 { -1 } else { ((enc_flags2 >> EncFlags2::GROUP_INDEX_SHIFT) & 7) as i32 };
		// SAFETY: generated data is valid
		let wbit: WBit = unsafe { mem::transmute(((enc_flags2 >> EncFlags2::WBIT_SHIFT) & EncFlags2::WBIT_MASK) as u8) };
		let w1 = if wbit == WBit::W1 { u32::MAX } else { 0 };
		// SAFETY: generated data is valid
		let lbit: LBit = unsafe { mem::transmute(((enc_flags2 >> EncFlags2::LBIT_SHIFT) & EncFlags2::LBIT_MASK) as u8) };
		let mut last_byte = if lbit == LBit::L1 || lbit == LBit::L256 { 4 } else { 0 };
		if w1 != 0 {
			last_byte |= 0x80;
		}
		last_byte |= (enc_flags2 >> EncFlags2::MANDATORY_PREFIX_SHIFT) & EncFlags2::MANDATORY_PREFIX_MASK;
		let mut mask_w_l = if wbit == WBit::WIG { 0x80 } else { 0 };
		let mask_l = if lbit == LBit::LIG {
			mask_w_l |= 4;
			4
		} else {
			0
		};

		let op0 = ((enc_flags1 >> EncFlags1::VEX_OP0_SHIFT) & EncFlags1::VEX_OP_MASK) as usize;
		let op1 = ((enc_flags1 >> EncFlags1::VEX_OP1_SHIFT) & EncFlags1::VEX_OP_MASK) as usize;
		let op2 = ((enc_flags1 >> EncFlags1::VEX_OP2_SHIFT) & EncFlags1::VEX_OP_MASK) as usize;
		let op3 = ((enc_flags1 >> EncFlags1::VEX_OP3_SHIFT) & EncFlags1::VEX_OP_MASK) as usize;
		let op4 = ((enc_flags1 >> EncFlags1::VEX_OP4_SHIFT) & EncFlags1::VEX_OP_MASK) as usize;
		let operands = if op4 != 0 {
			vec![VEX_TABLE[op0], VEX_TABLE[op1], VEX_TABLE[op2], VEX_TABLE[op3], VEX_TABLE[op4]]
		} else if op3 != 0 {
			debug_assert_eq!(op4, 0);
			vec![VEX_TABLE[op0], VEX_TABLE[op1], VEX_TABLE[op2], VEX_TABLE[op3]]
		} else if op2 != 0 {
			debug_assert_eq!(op3, 0);
			debug_assert_eq!(op4, 0);
			vec![VEX_TABLE[op0], VEX_TABLE[op1], VEX_TABLE[op2]]
		} else if op1 != 0 {
			debug_assert_eq!(op2, 0);
			debug_assert_eq!(op3, 0);
			debug_assert_eq!(op4, 0);
			vec![VEX_TABLE[op0], VEX_TABLE[op1]]
		} else if op0 != 0 {
			debug_assert_eq!(op1, 0);
			debug_assert_eq!(op2, 0);
			debug_assert_eq!(op3, 0);
			debug_assert_eq!(op4, 0);
			vec![VEX_TABLE[op0]]
		} else {
			debug_assert_eq!(op0, 0);
			debug_assert_eq!(op1, 0);
			debug_assert_eq!(op2, 0);
			debug_assert_eq!(op3, 0);
			debug_assert_eq!(op4, 0);
			Vec::new()
		};

		Self {
			base: OpCodeHandler {
				encode: Self::encode,
				try_convert_to_disp8n: None,
				operands: operands.into_boxed_slice(),
				op_code: get_op_code(enc_flags2),
				group_index,
				rm_group_index,
				enc_flags3,
				op_size: CodeSize::Unknown,
				addr_size: CodeSize::Unknown,
				is_2byte_opcode: (enc_flags2 & EncFlags2::OP_CODE_IS2_BYTES) != 0,
				is_special_instr: false,
			},
			table: (enc_flags2 >> EncFlags2::TABLE_SHIFT) & EncFlags2::TABLE_MASK,
			last_byte,
			mask_w_l,
			mask_l,
			w1,
		}
	}

	fn encode(self_ptr: *const OpCodeHandler, encoder: &mut Encoder, instruction: &Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		encoder.write_prefixes(instruction, true);
		let encoder_flags = encoder.encoder_flags;

		const _: () = assert!(MandatoryPrefixByte::None as u32 == 0);
		const _: () = assert!(MandatoryPrefixByte::P66 as u32 == 1);
		const _: () = assert!(MandatoryPrefixByte::PF3 as u32 == 2);
		const _: () = assert!(MandatoryPrefixByte::PF2 as u32 == 3);
		let mut b = this.last_byte;
		b |= (!encoder_flags >> (EncoderFlags::VVVVV_SHIFT - 3)) & 0x78;

		if (encoder.prevent_vex2
			| this.w1 | this.table.wrapping_sub(VexOpCodeTable::MAP0F as u32)
			| (encoder_flags & (EncoderFlags::X | EncoderFlags::B | EncoderFlags::W)))
			!= 0
		{
			encoder.write_byte_internal(0xC4);
			const _: () = assert!(VexOpCodeTable::MAP0F as u32 == 1);
			const _: () = assert!(VexOpCodeTable::MAP0F38 as u32 == 2);
			const _: () = assert!(VexOpCodeTable::MAP0F3A as u32 == 3);
			let mut b2 = this.table;
			const _: () = assert!(EncoderFlags::B == 1);
			const _: () = assert!(EncoderFlags::X == 2);
			const _: () = assert!(EncoderFlags::R == 4);
			b2 |= (!encoder_flags & 7) << 5;
			encoder.write_byte_internal(b2);
			b |= this.mask_w_l & encoder.internal_vex_wig_lig;
			encoder.write_byte_internal(b);
		} else {
			encoder.write_byte_internal(0xC5);
			const _: () = assert!(EncoderFlags::R == 4);
			b |= (!encoder_flags & 4) << 5;
			b |= this.mask_l & encoder.internal_vex_lig;
			encoder.write_byte_internal(b);
		}
	}
}

#[cfg(not(feature = "no_xop"))]
#[repr(C)]
pub(super) struct XopHandler {
	base: OpCodeHandler,
	table: u32,
	last_byte: u32,
}

#[cfg(not(feature = "no_xop"))]
impl XopHandler {
	pub(super) fn new(enc_flags1: u32, enc_flags2: u32, enc_flags3: u32) -> Self {
		const _: () = assert!(XopOpCodeTable::MAP8 as u32 == 0);
		const _: () = assert!(XopOpCodeTable::MAP9 as u32 == 1);
		const _: () = assert!(XopOpCodeTable::MAP10 as u32 == 2);
		let group_index = if (enc_flags2 & EncFlags2::HAS_GROUP_INDEX) == 0 { -1 } else { ((enc_flags2 >> EncFlags2::GROUP_INDEX_SHIFT) & 7) as i32 };
		let rm_group_index =
			if (enc_flags3 & EncFlags3::HAS_RM_GROUP_INDEX) == 0 { -1 } else { ((enc_flags2 >> EncFlags2::GROUP_INDEX_SHIFT) & 7) as i32 };
		// SAFETY: generated data is valid
		let lbit: LBit = unsafe { mem::transmute(((enc_flags2 >> EncFlags2::LBIT_SHIFT) & EncFlags2::LBIT_MASK) as u8) };
		let mut last_byte = match lbit {
			LBit::L1 | LBit::L256 => 4,
			_ => 0,
		};
		// SAFETY: generated data is valid
		let wbit: WBit = unsafe { mem::transmute(((enc_flags2 >> EncFlags2::WBIT_SHIFT) & EncFlags2::WBIT_MASK) as u8) };
		if wbit == WBit::W1 {
			last_byte |= 0x80;
		}
		last_byte |= (enc_flags2 >> EncFlags2::MANDATORY_PREFIX_SHIFT) & EncFlags2::MANDATORY_PREFIX_MASK;

		let op0 = ((enc_flags1 >> EncFlags1::XOP_OP0_SHIFT) & EncFlags1::XOP_OP_MASK) as usize;
		let op1 = ((enc_flags1 >> EncFlags1::XOP_OP1_SHIFT) & EncFlags1::XOP_OP_MASK) as usize;
		let op2 = ((enc_flags1 >> EncFlags1::XOP_OP2_SHIFT) & EncFlags1::XOP_OP_MASK) as usize;
		let op3 = ((enc_flags1 >> EncFlags1::XOP_OP3_SHIFT) & EncFlags1::XOP_OP_MASK) as usize;
		let operands = if op3 != 0 {
			vec![XOP_TABLE[op0], XOP_TABLE[op1], XOP_TABLE[op2], XOP_TABLE[op3]]
		} else if op2 != 0 {
			debug_assert_eq!(op3, 0);
			vec![XOP_TABLE[op0], XOP_TABLE[op1], XOP_TABLE[op2]]
		} else if op1 != 0 {
			debug_assert_eq!(op2, 0);
			debug_assert_eq!(op3, 0);
			vec![XOP_TABLE[op0], XOP_TABLE[op1]]
		} else if op0 != 0 {
			debug_assert_eq!(op1, 0);
			debug_assert_eq!(op2, 0);
			debug_assert_eq!(op3, 0);
			vec![XOP_TABLE[op0]]
		} else {
			debug_assert_eq!(op0, 0);
			debug_assert_eq!(op1, 0);
			debug_assert_eq!(op2, 0);
			debug_assert_eq!(op3, 0);
			Vec::new()
		};

		Self {
			base: OpCodeHandler {
				encode: Self::encode,
				try_convert_to_disp8n: None,
				operands: operands.into_boxed_slice(),
				op_code: get_op_code(enc_flags2),
				group_index,
				rm_group_index,
				enc_flags3,
				op_size: CodeSize::Unknown,
				addr_size: CodeSize::Unknown,
				is_2byte_opcode: (enc_flags2 & EncFlags2::OP_CODE_IS2_BYTES) != 0,
				is_special_instr: false,
			},
			table: 8 + ((enc_flags2 >> EncFlags2::TABLE_SHIFT) & EncFlags2::TABLE_MASK),
			last_byte,
		}
	}

	fn encode(self_ptr: *const OpCodeHandler, encoder: &mut Encoder, instruction: &Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		encoder.write_prefixes(instruction, true);
		encoder.write_byte_internal(0x8F);

		let encoder_flags = encoder.encoder_flags;
		const _: () = assert!(MandatoryPrefixByte::None as u32 == 0);
		const _: () = assert!(MandatoryPrefixByte::P66 as u32 == 1);
		const _: () = assert!(MandatoryPrefixByte::PF3 as u32 == 2);
		const _: () = assert!(MandatoryPrefixByte::PF2 as u32 == 3);

		let mut b = this.table;
		const _: () = assert!(EncoderFlags::B == 1);
		const _: () = assert!(EncoderFlags::X == 2);
		const _: () = assert!(EncoderFlags::R == 4);
		b |= (!encoder_flags & 7) << 5;
		encoder.write_byte_internal(b);
		b = this.last_byte;
		b |= (!encoder_flags >> (EncoderFlags::VVVVV_SHIFT - 3)) & 0x78;
		encoder.write_byte_internal(b);
	}
}

#[cfg(not(feature = "no_evex"))]
#[repr(C)]
pub(super) struct EvexHandler {
	base: OpCodeHandler,
	table: u32,
	p1_bits: u32,
	ll_bits: u32,
	mask_w: u32,
	mask_ll: u32,
	tuple_type: TupleType,
	wbit: WBit,
}

#[cfg(not(feature = "no_evex"))]
impl EvexHandler {
	pub(super) fn new(enc_flags1: u32, enc_flags2: u32, enc_flags3: u32) -> Self {
		let group_index = if (enc_flags2 & EncFlags2::HAS_GROUP_INDEX) == 0 { -1 } else { ((enc_flags2 >> EncFlags2::GROUP_INDEX_SHIFT) & 7) as i32 };
		let rm_group_index =
			if (enc_flags3 & EncFlags3::HAS_RM_GROUP_INDEX) == 0 { -1 } else { ((enc_flags2 >> EncFlags2::GROUP_INDEX_SHIFT) & 7) as i32 };
		const _: () = assert!(MandatoryPrefixByte::None as u32 == 0);
		const _: () = assert!(MandatoryPrefixByte::P66 as u32 == 1);
		const _: () = assert!(MandatoryPrefixByte::PF3 as u32 == 2);
		const _: () = assert!(MandatoryPrefixByte::PF2 as u32 == 3);
		let mut p1_bits = 4 | ((enc_flags2 >> EncFlags2::MANDATORY_PREFIX_SHIFT) & EncFlags2::MANDATORY_PREFIX_MASK);
		// SAFETY: generated data is valid
		let wbit: WBit = unsafe { mem::transmute(((enc_flags2 >> EncFlags2::WBIT_SHIFT) & EncFlags2::WBIT_MASK) as u8) };
		if wbit == WBit::W1 {
			p1_bits |= 0x80
		}
		// SAFETY: generated data is valid
		let lbit: LBit = unsafe { mem::transmute(((enc_flags2 >> EncFlags2::LBIT_SHIFT) & EncFlags2::LBIT_MASK) as u8) };
		let mut mask_ll = 0;
		let ll_bits = match lbit {
			LBit::LIG => {
				mask_ll = 3 << 5;
				0 << 5
			}
			LBit::L0 | LBit::LZ | LBit::L128 => 0 << 5,
			LBit::L1 | LBit::L256 => 1 << 5,
			LBit::L512 => 2 << 5,
		};
		let mask_w = if wbit == WBit::WIG { 0x80 } else { 0 };

		let op0 = ((enc_flags1 >> EncFlags1::EVEX_OP0_SHIFT) & EncFlags1::EVEX_OP_MASK) as usize;
		let op1 = ((enc_flags1 >> EncFlags1::EVEX_OP1_SHIFT) & EncFlags1::EVEX_OP_MASK) as usize;
		let op2 = ((enc_flags1 >> EncFlags1::EVEX_OP2_SHIFT) & EncFlags1::EVEX_OP_MASK) as usize;
		let op3 = ((enc_flags1 >> EncFlags1::EVEX_OP3_SHIFT) & EncFlags1::EVEX_OP_MASK) as usize;
		let operands = if op3 != 0 {
			vec![EVEX_TABLE[op0], EVEX_TABLE[op1], EVEX_TABLE[op2], EVEX_TABLE[op3]]
		} else if op2 != 0 {
			debug_assert_eq!(op3, 0);
			vec![EVEX_TABLE[op0], EVEX_TABLE[op1], EVEX_TABLE[op2]]
		} else if op1 != 0 {
			debug_assert_eq!(op2, 0);
			debug_assert_eq!(op3, 0);
			vec![EVEX_TABLE[op0], EVEX_TABLE[op1]]
		} else if op0 != 0 {
			debug_assert_eq!(op1, 0);
			debug_assert_eq!(op2, 0);
			debug_assert_eq!(op3, 0);
			vec![EVEX_TABLE[op0]]
		} else {
			debug_assert_eq!(op0, 0);
			debug_assert_eq!(op1, 0);
			debug_assert_eq!(op2, 0);
			debug_assert_eq!(op3, 0);
			Vec::new()
		};

		Self {
			base: OpCodeHandler {
				encode: Self::encode,
				try_convert_to_disp8n: Some(Self::try_convert_to_disp8n),
				operands: operands.into_boxed_slice(),
				op_code: get_op_code(enc_flags2),
				group_index,
				rm_group_index,
				enc_flags3,
				op_size: CodeSize::Unknown,
				addr_size: CodeSize::Unknown,
				is_2byte_opcode: (enc_flags2 & EncFlags2::OP_CODE_IS2_BYTES) != 0,
				is_special_instr: false,
			},
			table: (enc_flags2 >> EncFlags2::TABLE_SHIFT) & EncFlags2::TABLE_MASK,
			p1_bits,
			ll_bits,
			mask_w,
			mask_ll,
			// SAFETY: generated data is valid
			tuple_type: unsafe {
				mem::transmute(((enc_flags3 >> EncFlags3::TUPLE_TYPE_SHIFT) & EncFlags3::TUPLE_TYPE_MASK) as TupleTypeUnderlyingType)
			},
			wbit,
		}
	}

	fn try_convert_to_disp8n(self_ptr: *const OpCodeHandler, encoder: &mut Encoder, _instruction: &Instruction, displ: i32) -> Option<i8> {
		let this = unsafe { &*(self_ptr as *const Self) };
		let n = get_disp8n(this.tuple_type, (encoder.encoder_flags & EncoderFlags::BROADCAST) != 0) as i32;
		let res = displ / n;
		if res.wrapping_mul(n) == displ && i8::MIN as i32 <= res && res <= i8::MAX as i32 {
			Some(res as i8)
		} else {
			None
		}
	}

	fn encode(self_ptr: *const OpCodeHandler, encoder: &mut Encoder, instruction: &Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		encoder.write_prefixes(instruction, true);
		let encoder_flags = encoder.encoder_flags;

		encoder.write_byte_internal(0x62);

		const _: () = assert!(EvexOpCodeTable::MAP0F as u32 == 1);
		const _: () = assert!(EvexOpCodeTable::MAP0F38 as u32 == 2);
		const _: () = assert!(EvexOpCodeTable::MAP0F3A as u32 == 3);
		const _: () = assert!(EvexOpCodeTable::MAP5 as u32 == 5);
		const _: () = assert!(EvexOpCodeTable::MAP6 as u32 == 6);
		let mut b = this.table;
		const _: () = assert!(EncoderFlags::B == 1);
		const _: () = assert!(EncoderFlags::X == 2);
		const _: () = assert!(EncoderFlags::R == 4);
		b |= (encoder_flags & 7) << 5;
		const _: () = assert!(EncoderFlags::R2 == 0x0000_0200);
		b |= (encoder_flags >> (9 - 4)) & 0x10;
		b ^= !0xF;
		encoder.write_byte_internal(b);

		b = this.p1_bits;
		b |= (!encoder_flags >> (EncoderFlags::VVVVV_SHIFT - 3)) & 0x78;
		b |= this.mask_w & encoder.internal_evex_wig;
		encoder.write_byte_internal(b);

		b = crate::instruction_internal::internal_op_mask(instruction);
		if b != 0 {
			if (this.base.enc_flags3 & EncFlags3::OP_MASK_REGISTER) == 0 {
				encoder.set_error_message_str("The instruction doesn't support opmask registers");
			}
		} else {
			if (this.base.enc_flags3 & EncFlags3::REQUIRE_OP_MASK_REGISTER) != 0 {
				encoder.set_error_message_str("The instruction must use an opmask register");
			}
		}
		b |= (encoder_flags >> (EncoderFlags::VVVVV_SHIFT + 4 - 3)) & 8;
		if instruction.suppress_all_exceptions() {
			if (this.base.enc_flags3 & EncFlags3::SUPPRESS_ALL_EXCEPTIONS) == 0 {
				encoder.set_error_message_str("The instruction doesn't support suppress-all-exceptions");
			}
			b |= 0x10;
		}
		let rc = instruction.rounding_control();
		if rc != RoundingControl::None {
			if (this.base.enc_flags3 & EncFlags3::ROUNDING_CONTROL) == 0 {
				encoder.set_error_message_str("The instruction doesn't support rounding control");
			}
			b |= 0x10;
			const _: () = assert!(RoundingControl::RoundToNearest as u32 == 1);
			const _: () = assert!(RoundingControl::RoundDown as u32 == 2);
			const _: () = assert!(RoundingControl::RoundUp as u32 == 3);
			const _: () = assert!(RoundingControl::RoundTowardZero as u32 == 4);
			b |= (rc as u32 - RoundingControl::RoundToNearest as u32) << 5;
		} else if (this.base.enc_flags3 & EncFlags3::SUPPRESS_ALL_EXCEPTIONS) == 0 || !instruction.suppress_all_exceptions() {
			b |= this.ll_bits;
		}
		if (encoder_flags & EncoderFlags::BROADCAST) != 0 {
			b |= 0x10;
		} else if instruction.is_broadcast() {
			encoder.set_error_message_str("The instruction doesn't support broadcasting");
		}
		if instruction.zeroing_masking() {
			if (this.base.enc_flags3 & EncFlags3::ZEROING_MASKING) == 0 {
				encoder.set_error_message_str("The instruction doesn't support zeroing masking");
			}
			b |= 0x80;
		}
		b ^= 8;
		b |= this.mask_ll & encoder.internal_evex_lig;
		encoder.write_byte_internal(b);
	}
}

#[cfg(feature = "mvex")]
#[repr(C)]
pub(super) struct MvexHandler {
	base: OpCodeHandler,
	table: u32,
	p1_bits: u32,
	mask_w: u32,
	wbit: WBit,
}

#[cfg(feature = "mvex")]
impl MvexHandler {
	pub(super) fn new(enc_flags1: u32, enc_flags2: u32, enc_flags3: u32) -> Self {
		let group_index = if (enc_flags2 & EncFlags2::HAS_GROUP_INDEX) == 0 { -1 } else { ((enc_flags2 >> EncFlags2::GROUP_INDEX_SHIFT) & 7) as i32 };
		let rm_group_index =
			if (enc_flags3 & EncFlags3::HAS_RM_GROUP_INDEX) == 0 { -1 } else { ((enc_flags2 >> EncFlags2::GROUP_INDEX_SHIFT) & 7) as i32 };
		const _: () = assert!(MandatoryPrefixByte::None as u32 == 0);
		const _: () = assert!(MandatoryPrefixByte::P66 as u32 == 1);
		const _: () = assert!(MandatoryPrefixByte::PF3 as u32 == 2);
		const _: () = assert!(MandatoryPrefixByte::PF2 as u32 == 3);
		let mut p1_bits = (enc_flags2 >> EncFlags2::MANDATORY_PREFIX_SHIFT) & EncFlags2::MANDATORY_PREFIX_MASK;
		// SAFETY: generated data is valid
		let wbit: WBit = unsafe { mem::transmute(((enc_flags2 >> EncFlags2::WBIT_SHIFT) & EncFlags2::WBIT_MASK) as u8) };
		if wbit == WBit::W1 {
			p1_bits |= 0x80
		}
		let mask_w = if wbit == WBit::WIG { 0x80 } else { 0 };

		let op0 = ((enc_flags1 >> EncFlags1::MVEX_OP0_SHIFT) & EncFlags1::MVEX_OP_MASK) as usize;
		let op1 = ((enc_flags1 >> EncFlags1::MVEX_OP1_SHIFT) & EncFlags1::MVEX_OP_MASK) as usize;
		let op2 = ((enc_flags1 >> EncFlags1::MVEX_OP2_SHIFT) & EncFlags1::MVEX_OP_MASK) as usize;
		let op3 = ((enc_flags1 >> EncFlags1::MVEX_OP3_SHIFT) & EncFlags1::MVEX_OP_MASK) as usize;
		let operands = if op3 != 0 {
			vec![MVEX_TABLE[op0], MVEX_TABLE[op1], MVEX_TABLE[op2], MVEX_TABLE[op3]]
		} else if op2 != 0 {
			debug_assert_eq!(op3, 0);
			vec![MVEX_TABLE[op0], MVEX_TABLE[op1], MVEX_TABLE[op2]]
		} else if op1 != 0 {
			debug_assert_eq!(op2, 0);
			debug_assert_eq!(op3, 0);
			vec![MVEX_TABLE[op0], MVEX_TABLE[op1]]
		} else if op0 != 0 {
			debug_assert_eq!(op1, 0);
			debug_assert_eq!(op2, 0);
			debug_assert_eq!(op3, 0);
			vec![MVEX_TABLE[op0]]
		} else {
			debug_assert_eq!(op0, 0);
			debug_assert_eq!(op1, 0);
			debug_assert_eq!(op2, 0);
			debug_assert_eq!(op3, 0);
			Vec::new()
		};

		Self {
			base: OpCodeHandler {
				encode: Self::encode,
				try_convert_to_disp8n: Some(Self::try_convert_to_disp8n),
				operands: operands.into_boxed_slice(),
				op_code: get_op_code(enc_flags2),
				group_index,
				rm_group_index,
				enc_flags3,
				op_size: CodeSize::Unknown,
				addr_size: CodeSize::Unknown,
				is_2byte_opcode: (enc_flags2 & EncFlags2::OP_CODE_IS2_BYTES) != 0,
				is_special_instr: false,
			},
			table: (enc_flags2 >> EncFlags2::TABLE_SHIFT) & EncFlags2::TABLE_MASK,
			p1_bits,
			mask_w,
			wbit,
		}
	}

	fn try_convert_to_disp8n(_self_ptr: *const OpCodeHandler, _encoder: &mut Encoder, instruction: &Instruction, displ: i32) -> Option<i8> {
		let mvex = get_mvex_info(instruction.code());
		let conv = instruction.mvex_reg_mem_conv();
		let sss = (conv as usize).wrapping_sub(MvexRegMemConv::MemConvNone as usize) & 7;
		let tuple_type = crate::mvex::mvex_tt_lut::MVEX_TUPLE_TYPE_LUT[(mvex.tuple_type_lut_kind as usize) * 8 + sss];

		let n = get_disp8n(tuple_type, false) as i32;
		let res = displ / n;
		if res.wrapping_mul(n) == displ && i8::MIN as i32 <= res && res <= i8::MAX as i32 {
			Some(res as i8)
		} else {
			None
		}
	}

	fn encode(self_ptr: *const OpCodeHandler, encoder: &mut Encoder, instruction: &Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		encoder.write_prefixes(instruction, true);
		let encoder_flags = encoder.encoder_flags;

		encoder.write_byte_internal(0x62);

		const _: () = assert!(MvexOpCodeTable::MAP0F as u32 == 1);
		const _: () = assert!(MvexOpCodeTable::MAP0F38 as u32 == 2);
		const _: () = assert!(MvexOpCodeTable::MAP0F3A as u32 == 3);
		let mut b = this.table;
		const _: () = assert!(EncoderFlags::B == 1);
		const _: () = assert!(EncoderFlags::X == 2);
		const _: () = assert!(EncoderFlags::R == 4);
		b |= (encoder_flags & 7) << 5;
		const _: () = assert!(EncoderFlags::R2 == 0x0000_0200);
		b |= (encoder_flags >> (9 - 4)) & 0x10;
		b ^= !0xF;
		encoder.write_byte_internal(b);

		b = this.p1_bits;
		b |= (!encoder_flags >> (EncoderFlags::VVVVV_SHIFT - 3)) & 0x78;
		b |= this.mask_w & encoder.internal_mvex_wig;
		encoder.write_byte_internal(b);

		b = crate::instruction_internal::internal_op_mask(instruction);
		if b != 0 {
			if (this.base.enc_flags3 & EncFlags3::OP_MASK_REGISTER) == 0 {
				encoder.set_error_message_str("The instruction doesn't support opmask registers");
			}
		} else {
			if (this.base.enc_flags3 & EncFlags3::REQUIRE_OP_MASK_REGISTER) != 0 {
				encoder.set_error_message_str("The instruction must use an opmask register");
			}
		}
		b |= (encoder_flags >> (EncoderFlags::VVVVV_SHIFT + 4 - 3)) & 8;
		let mvex = get_mvex_info(instruction.code());
		let conv = instruction.mvex_reg_mem_conv();
		// Memory ops can only be op0-op2, never op3 (imm8)
		if instruction.op0_kind() == OpKind::Memory || instruction.op1_kind() == OpKind::Memory || instruction.op2_kind() == OpKind::Memory {
			const _: () = assert!(MvexRegMemConv::MemConvNone as u32 + 1 == MvexRegMemConv::MemConvBroadcast1 as u32);
			const _: () = assert!(MvexRegMemConv::MemConvNone as u32 + 2 == MvexRegMemConv::MemConvBroadcast4 as u32);
			const _: () = assert!(MvexRegMemConv::MemConvNone as u32 + 3 == MvexRegMemConv::MemConvFloat16 as u32);
			const _: () = assert!(MvexRegMemConv::MemConvNone as u32 + 4 == MvexRegMemConv::MemConvUint8 as u32);
			const _: () = assert!(MvexRegMemConv::MemConvNone as u32 + 5 == MvexRegMemConv::MemConvSint8 as u32);
			const _: () = assert!(MvexRegMemConv::MemConvNone as u32 + 6 == MvexRegMemConv::MemConvUint16 as u32);
			const _: () = assert!(MvexRegMemConv::MemConvNone as u32 + 7 == MvexRegMemConv::MemConvSint16 as u32);
			if conv >= MvexRegMemConv::MemConvNone && conv <= MvexRegMemConv::MemConvSint16 {
				b |= (conv as u32 - MvexRegMemConv::MemConvNone as u32) << 4;
			} else if conv == MvexRegMemConv::None {
				// Nothing, treat it as MvexRegMemConv::MemConvNone
			} else {
				encoder.set_error_message_str("Memory operands must use a valid MvexRegMemConv variant, eg. MvexRegMemConv::MemConvNone");
			}
			if instruction.is_mvex_eviction_hint() {
				if mvex.can_use_eviction_hint() {
					b |= 0x80;
				} else {
					encoder.set_error_message_str("This instruction doesn't support eviction hint (`{eh}`)");
				}
			}
		} else {
			if instruction.is_mvex_eviction_hint() {
				encoder.set_error_message_str("Only memory operands can enable eviction hint (`{eh}`)");
			}
			if conv == MvexRegMemConv::None {
				b |= 0x80;
				if instruction.suppress_all_exceptions() {
					b |= 0x40;
					if (this.base.enc_flags3 & EncFlags3::SUPPRESS_ALL_EXCEPTIONS) == 0 {
						encoder.set_error_message_str("The instruction doesn't support suppress-all-exceptions");
					}
				}
				let rc = instruction.rounding_control();
				if rc == RoundingControl::None {
					// Nothing
				} else {
					if (this.base.enc_flags3 & EncFlags3::ROUNDING_CONTROL) == 0 {
						encoder.set_error_message_str("The instruction doesn't support rounding control");
					} else {
						const _: () = assert!(RoundingControl::RoundToNearest as u32 == 1);
						const _: () = assert!(RoundingControl::RoundDown as u32 == 2);
						const _: () = assert!(RoundingControl::RoundUp as u32 == 3);
						const _: () = assert!(RoundingControl::RoundTowardZero as u32 == 4);
						b |= (rc as u32 - RoundingControl::RoundToNearest as u32) << 4;
					}
				}
			} else if conv >= MvexRegMemConv::RegSwizzleNone && conv <= MvexRegMemConv::RegSwizzleDddd {
				if instruction.suppress_all_exceptions() {
					encoder.set_error_message_str("Can't use {sae} with register swizzles");
				} else if instruction.rounding_control() != RoundingControl::None {
					encoder.set_error_message_str("Can't use rounding control with register swizzles");
				}
				b |= ((conv as u32).wrapping_sub(MvexRegMemConv::RegSwizzleNone as u32) & 7) << 4;
			} else {
				encoder.set_error_message_str("Register operands can't use memory up/down conversions");
			}
		}
		if mvex.eh_bit == MvexEHBit::EH1 {
			b |= 0x80;
		}
		b ^= 8;
		encoder.write_byte_internal(b);
	}
}

#[cfg(not(feature = "no_d3now"))]
#[repr(C)]
pub(super) struct D3nowHandler {
	base: OpCodeHandler,
	immediate: u32,
}

#[cfg(not(feature = "no_d3now"))]
impl D3nowHandler {
	pub(super) fn new(enc_flags2: u32, enc_flags3: u32) -> Self {
		let mut operands = Vec::with_capacity(2);
		static D3NOW_TABLE: [&(dyn Op + Sync); 2] =
			[&OpModRM_reg { reg_lo: Register::MM0, reg_hi: Register::MM7 }, &OpModRM_rm { reg_lo: Register::MM0, reg_hi: Register::MM7 }];
		operands.push(D3NOW_TABLE[0]);
		operands.push(D3NOW_TABLE[1]);

		Self {
			base: OpCodeHandler {
				encode: Self::encode,
				try_convert_to_disp8n: None,
				operands: operands.into_boxed_slice(),
				op_code: 0x0F,
				group_index: -1,
				rm_group_index: -1,
				enc_flags3,
				op_size: CodeSize::Unknown,
				addr_size: CodeSize::Unknown,
				is_2byte_opcode: (enc_flags2 & EncFlags2::OP_CODE_IS2_BYTES) != 0,
				is_special_instr: false,
			},
			immediate: get_op_code(enc_flags2),
		}
	}

	fn encode(self_ptr: *const OpCodeHandler, encoder: &mut Encoder, instruction: &Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		encoder.write_prefixes(instruction, true);
		encoder.write_byte_internal(0x0F);
		encoder.imm_size = ImmSize::Size1OpCode;
		encoder.immediate = this.immediate;
	}
}
