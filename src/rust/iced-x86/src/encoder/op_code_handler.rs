// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

#[cfg(not(feature = "no_evex"))]
use super::super::tuple_type_tbl::get_disp8n;
use super::super::*;
use super::enums::*;
use super::ops::*;
use super::ops_tables::*;
use super::*;
#[cfg(not(feature = "std"))]
use alloc::boxed::Box;
#[cfg(not(feature = "std"))]
use alloc::vec::Vec;
use core::{i8, mem, u32};

#[repr(C)]
pub(crate) struct OpCodeHandler {
	pub(super) encode: fn(self_ptr: *const OpCodeHandler, encoder: &mut Encoder, instruction: &Instruction),
	pub(super) try_convert_to_disp8n: Option<fn(self_ptr: *const OpCodeHandler, encoder: &mut Encoder, displ: i32) -> Option<i8>>,
	pub(crate) operands: Box<[&'static (dyn Op + Sync)]>,
	pub(super) op_code: u32,
	pub(super) group_index: i32,
	pub(super) rm_group_index: i32,
	pub(super) enc_flags3: u32, // EncFlags3
	pub(super) op_size: CodeSize,
	pub(super) addr_size: CodeSize,
	pub(super) is_2byte_opcode: bool,
	pub(super) is_declare_data: bool,
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
				is_declare_data: false,
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
				is_declare_data: true,
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

fn get_op_code(enc_flags2: u32) -> u32 {
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
			if (enc_flags2 & EncFlags2::HAS_RM_GROUP_INDEX) == 0 { -1 } else { ((enc_flags2 >> EncFlags2::GROUP_INDEX_SHIFT) & 7) as i32 };
		let table: LegacyOpCodeTable = unsafe { mem::transmute(((enc_flags2 >> EncFlags2::TABLE_SHIFT) & EncFlags2::TABLE_MASK) as u8) };
		let (table_byte1, table_byte2) = match table {
			LegacyOpCodeTable::Normal => (0, 0),
			LegacyOpCodeTable::Table0F => (0x0F, 0),
			LegacyOpCodeTable::Table0F38 => (0x0F, 0x38),
			LegacyOpCodeTable::Table0F3A => (0x0F, 0x3A),
		};
		let mpb: MandatoryPrefixByte =
			unsafe { mem::transmute(((enc_flags2 >> EncFlags2::MANDATORY_PREFIX_SHIFT) & EncFlags2::MANDATORY_PREFIX_MASK) as u8) };
		let mandatory_prefix = match mpb {
			MandatoryPrefixByte::None => 0,
			MandatoryPrefixByte::P66 => 0x66,
			MandatoryPrefixByte::PF3 => 0xF3,
			MandatoryPrefixByte::PF2 => 0xF2,
		};

		let mut operands;
		let op0 = ((enc_flags1 >> EncFlags1::LEGACY_OP0_SHIFT) & EncFlags1::LEGACY_OP_MASK) as usize;
		let op1 = ((enc_flags1 >> EncFlags1::LEGACY_OP1_SHIFT) & EncFlags1::LEGACY_OP_MASK) as usize;
		let op2 = ((enc_flags1 >> EncFlags1::LEGACY_OP2_SHIFT) & EncFlags1::LEGACY_OP_MASK) as usize;
		let op3 = ((enc_flags1 >> EncFlags1::LEGACY_OP3_SHIFT) & EncFlags1::LEGACY_OP_MASK) as usize;
		if op3 != 0 {
			operands = Vec::with_capacity(4);
			operands.push(LEGACY_TABLE[op0]);
			operands.push(LEGACY_TABLE[op1]);
			operands.push(LEGACY_TABLE[op2]);
			operands.push(LEGACY_TABLE[op3]);
		} else if op2 != 0 {
			operands = Vec::with_capacity(3);
			operands.push(LEGACY_TABLE[op0]);
			operands.push(LEGACY_TABLE[op1]);
			operands.push(LEGACY_TABLE[op2]);
			debug_assert_eq!(0, op3);
		} else if op1 != 0 {
			operands = Vec::with_capacity(2);
			operands.push(LEGACY_TABLE[op0]);
			operands.push(LEGACY_TABLE[op1]);
			debug_assert_eq!(0, op2);
			debug_assert_eq!(0, op3);
		} else if op0 != 0 {
			operands = Vec::with_capacity(1);
			operands.push(LEGACY_TABLE[op0]);
			debug_assert_eq!(0, op1);
			debug_assert_eq!(0, op2);
			debug_assert_eq!(0, op3);
		} else {
			operands = Vec::new();
			debug_assert_eq!(0, op0);
			debug_assert_eq!(0, op1);
			debug_assert_eq!(0, op2);
			debug_assert_eq!(0, op3);
		}

		Self {
			base: OpCodeHandler {
				encode: Self::encode,
				try_convert_to_disp8n: None,
				operands: operands.into_boxed_slice(),
				op_code: get_op_code(enc_flags2),
				group_index,
				rm_group_index,
				enc_flags3,
				op_size: unsafe { mem::transmute(((enc_flags3 >> EncFlags3::OPERAND_SIZE_SHIFT) & EncFlags3::OPERAND_SIZE_MASK) as u8) },
				addr_size: unsafe { mem::transmute(((enc_flags3 >> EncFlags3::ADDRESS_SIZE_SHIFT) & EncFlags3::ADDRESS_SIZE_MASK) as u8) },
				is_2byte_opcode: (enc_flags2 & EncFlags2::OP_CODE_IS2_BYTES) != 0,
				is_declare_data: false,
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

		const_assert_eq!(0x01, EncoderFlags::B);
		const_assert_eq!(0x02, EncoderFlags::X);
		const_assert_eq!(0x04, EncoderFlags::R);
		const_assert_eq!(0x08, EncoderFlags::W);
		const_assert_eq!(0x40, EncoderFlags::REX);
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
			if (enc_flags2 & EncFlags2::HAS_RM_GROUP_INDEX) == 0 { -1 } else { ((enc_flags2 >> EncFlags2::GROUP_INDEX_SHIFT) & 7) as i32 };
		let wbit: WBit = unsafe { mem::transmute(((enc_flags2 >> EncFlags2::WBIT_SHIFT) & EncFlags2::WBIT_MASK) as u8) };
		let w1 = if wbit == WBit::W1 { u32::MAX } else { 0 };
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

		let mut operands;
		let op0 = ((enc_flags1 >> EncFlags1::VEX_OP0_SHIFT) & EncFlags1::VEX_OP_MASK) as usize;
		let op1 = ((enc_flags1 >> EncFlags1::VEX_OP1_SHIFT) & EncFlags1::VEX_OP_MASK) as usize;
		let op2 = ((enc_flags1 >> EncFlags1::VEX_OP2_SHIFT) & EncFlags1::VEX_OP_MASK) as usize;
		let op3 = ((enc_flags1 >> EncFlags1::VEX_OP3_SHIFT) & EncFlags1::VEX_OP_MASK) as usize;
		let op4 = ((enc_flags1 >> EncFlags1::VEX_OP4_SHIFT) & EncFlags1::VEX_OP_MASK) as usize;
		if op4 != 0 {
			operands = Vec::with_capacity(5);
			operands.push(VEX_TABLE[op0]);
			operands.push(VEX_TABLE[op1]);
			operands.push(VEX_TABLE[op2]);
			operands.push(VEX_TABLE[op3]);
			operands.push(VEX_TABLE[op4]);
		} else if op3 != 0 {
			operands = Vec::with_capacity(4);
			operands.push(VEX_TABLE[op0]);
			operands.push(VEX_TABLE[op1]);
			operands.push(VEX_TABLE[op2]);
			operands.push(VEX_TABLE[op3]);
			debug_assert_eq!(0, op4);
		} else if op2 != 0 {
			operands = Vec::with_capacity(3);
			operands.push(VEX_TABLE[op0]);
			operands.push(VEX_TABLE[op1]);
			operands.push(VEX_TABLE[op2]);
			debug_assert_eq!(0, op3);
			debug_assert_eq!(0, op4);
		} else if op1 != 0 {
			operands = Vec::with_capacity(2);
			operands.push(VEX_TABLE[op0]);
			operands.push(VEX_TABLE[op1]);
			debug_assert_eq!(0, op2);
			debug_assert_eq!(0, op3);
			debug_assert_eq!(0, op4);
		} else if op0 != 0 {
			operands = Vec::with_capacity(1);
			operands.push(VEX_TABLE[op0]);
			debug_assert_eq!(0, op1);
			debug_assert_eq!(0, op2);
			debug_assert_eq!(0, op3);
			debug_assert_eq!(0, op4);
		} else {
			operands = Vec::new();
			debug_assert_eq!(0, op0);
			debug_assert_eq!(0, op1);
			debug_assert_eq!(0, op2);
			debug_assert_eq!(0, op3);
			debug_assert_eq!(0, op4);
		}

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
				is_declare_data: false,
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

		const_assert_eq!(0, MandatoryPrefixByte::None as u32);
		const_assert_eq!(1, MandatoryPrefixByte::P66 as u32);
		const_assert_eq!(2, MandatoryPrefixByte::PF3 as u32);
		const_assert_eq!(3, MandatoryPrefixByte::PF2 as u32);
		let mut b = this.last_byte;
		b |= (!encoder_flags >> (EncoderFlags::VVVVV_SHIFT - 3)) & 0x78;

		if (encoder.prevent_vex2
			| this.w1 | this.table.wrapping_sub(VexOpCodeTable::Table0F as u32)
			| (encoder_flags & (EncoderFlags::X | EncoderFlags::B | EncoderFlags::W)))
			!= 0
		{
			encoder.write_byte_internal(0xC4);
			const_assert_eq!(1, VexOpCodeTable::Table0F as u32);
			const_assert_eq!(2, VexOpCodeTable::Table0F38 as u32);
			const_assert_eq!(3, VexOpCodeTable::Table0F3A as u32);
			let mut b2 = this.table;
			const_assert_eq!(1, EncoderFlags::B);
			const_assert_eq!(2, EncoderFlags::X);
			const_assert_eq!(4, EncoderFlags::R);
			b2 |= (!encoder_flags & 7) << 5;
			encoder.write_byte_internal(b2);
			b |= this.mask_w_l & encoder.internal_vex_wig_lig;
			encoder.write_byte_internal(b);
		} else {
			encoder.write_byte_internal(0xC5);
			const_assert_eq!(4, EncoderFlags::R);
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
		const_assert_eq!(0, XopOpCodeTable::XOP8 as u32);
		const_assert_eq!(1, XopOpCodeTable::XOP9 as u32);
		const_assert_eq!(2, XopOpCodeTable::XOPA as u32);
		let group_index = if (enc_flags2 & EncFlags2::HAS_GROUP_INDEX) == 0 { -1 } else { ((enc_flags2 >> EncFlags2::GROUP_INDEX_SHIFT) & 7) as i32 };
		let rm_group_index =
			if (enc_flags2 & EncFlags2::HAS_RM_GROUP_INDEX) == 0 { -1 } else { ((enc_flags2 >> EncFlags2::GROUP_INDEX_SHIFT) & 7) as i32 };
		let lbit: LBit = unsafe { mem::transmute(((enc_flags2 >> EncFlags2::LBIT_SHIFT) & EncFlags2::LBIT_MASK) as u8) };
		let mut last_byte = match lbit {
			LBit::L1 | LBit::L256 => 4,
			_ => 0,
		};
		let wbit: WBit = unsafe { mem::transmute(((enc_flags2 >> EncFlags2::WBIT_SHIFT) & EncFlags2::WBIT_MASK) as u8) };
		if wbit == WBit::W1 {
			last_byte |= 0x80;
		}
		last_byte |= (enc_flags2 >> EncFlags2::MANDATORY_PREFIX_SHIFT) & EncFlags2::MANDATORY_PREFIX_MASK;

		let mut operands;
		let op0 = ((enc_flags1 >> EncFlags1::XOP_OP0_SHIFT) & EncFlags1::XOP_OP_MASK) as usize;
		let op1 = ((enc_flags1 >> EncFlags1::XOP_OP1_SHIFT) & EncFlags1::XOP_OP_MASK) as usize;
		let op2 = ((enc_flags1 >> EncFlags1::XOP_OP2_SHIFT) & EncFlags1::XOP_OP_MASK) as usize;
		let op3 = ((enc_flags1 >> EncFlags1::XOP_OP3_SHIFT) & EncFlags1::XOP_OP_MASK) as usize;
		if op3 != 0 {
			operands = Vec::with_capacity(4);
			operands.push(XOP_TABLE[op0]);
			operands.push(XOP_TABLE[op1]);
			operands.push(XOP_TABLE[op2]);
			operands.push(XOP_TABLE[op3]);
		} else if op2 != 0 {
			operands = Vec::with_capacity(3);
			operands.push(XOP_TABLE[op0]);
			operands.push(XOP_TABLE[op1]);
			operands.push(XOP_TABLE[op2]);
			debug_assert_eq!(0, op3);
		} else if op1 != 0 {
			operands = Vec::with_capacity(2);
			operands.push(XOP_TABLE[op0]);
			operands.push(XOP_TABLE[op1]);
			debug_assert_eq!(0, op2);
			debug_assert_eq!(0, op3);
		} else if op0 != 0 {
			operands = Vec::with_capacity(1);
			operands.push(XOP_TABLE[op0]);
			debug_assert_eq!(0, op1);
			debug_assert_eq!(0, op2);
			debug_assert_eq!(0, op3);
		} else {
			operands = Vec::new();
			debug_assert_eq!(0, op0);
			debug_assert_eq!(0, op1);
			debug_assert_eq!(0, op2);
			debug_assert_eq!(0, op3);
		}

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
				is_declare_data: false,
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
		const_assert_eq!(0, MandatoryPrefixByte::None as u32);
		const_assert_eq!(1, MandatoryPrefixByte::P66 as u32);
		const_assert_eq!(2, MandatoryPrefixByte::PF3 as u32);
		const_assert_eq!(3, MandatoryPrefixByte::PF2 as u32);

		let mut b = this.table;
		const_assert_eq!(1, EncoderFlags::B);
		const_assert_eq!(2, EncoderFlags::X);
		const_assert_eq!(4, EncoderFlags::R);
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
			if (enc_flags2 & EncFlags2::HAS_RM_GROUP_INDEX) == 0 { -1 } else { ((enc_flags2 >> EncFlags2::GROUP_INDEX_SHIFT) & 7) as i32 };
		const_assert_eq!(0, MandatoryPrefixByte::None as u32);
		const_assert_eq!(1, MandatoryPrefixByte::P66 as u32);
		const_assert_eq!(2, MandatoryPrefixByte::PF3 as u32);
		const_assert_eq!(3, MandatoryPrefixByte::PF2 as u32);
		let mut p1_bits = 4 | ((enc_flags2 >> EncFlags2::MANDATORY_PREFIX_SHIFT) & EncFlags2::MANDATORY_PREFIX_MASK);
		let wbit: WBit = unsafe { mem::transmute(((enc_flags2 >> EncFlags2::WBIT_SHIFT) & EncFlags2::WBIT_MASK) as u8) };
		if wbit == WBit::W1 {
			p1_bits |= 0x80
		}
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

		let mut operands;
		let op0 = ((enc_flags1 >> EncFlags1::EVEX_OP0_SHIFT) & EncFlags1::EVEX_OP_MASK) as usize;
		let op1 = ((enc_flags1 >> EncFlags1::EVEX_OP1_SHIFT) & EncFlags1::EVEX_OP_MASK) as usize;
		let op2 = ((enc_flags1 >> EncFlags1::EVEX_OP2_SHIFT) & EncFlags1::EVEX_OP_MASK) as usize;
		let op3 = ((enc_flags1 >> EncFlags1::EVEX_OP3_SHIFT) & EncFlags1::EVEX_OP_MASK) as usize;
		if op3 != 0 {
			operands = Vec::with_capacity(4);
			operands.push(EVEX_TABLE[op0]);
			operands.push(EVEX_TABLE[op1]);
			operands.push(EVEX_TABLE[op2]);
			operands.push(EVEX_TABLE[op3]);
		} else if op2 != 0 {
			operands = Vec::with_capacity(3);
			operands.push(EVEX_TABLE[op0]);
			operands.push(EVEX_TABLE[op1]);
			operands.push(EVEX_TABLE[op2]);
			debug_assert_eq!(0, op3);
		} else if op1 != 0 {
			operands = Vec::with_capacity(2);
			operands.push(EVEX_TABLE[op0]);
			operands.push(EVEX_TABLE[op1]);
			debug_assert_eq!(0, op2);
			debug_assert_eq!(0, op3);
		} else if op0 != 0 {
			operands = Vec::with_capacity(1);
			operands.push(EVEX_TABLE[op0]);
			debug_assert_eq!(0, op1);
			debug_assert_eq!(0, op2);
			debug_assert_eq!(0, op3);
		} else {
			operands = Vec::new();
			debug_assert_eq!(0, op0);
			debug_assert_eq!(0, op1);
			debug_assert_eq!(0, op2);
			debug_assert_eq!(0, op3);
		}

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
				is_declare_data: false,
			},
			table: (enc_flags2 >> EncFlags2::TABLE_SHIFT) & EncFlags2::TABLE_MASK,
			p1_bits,
			ll_bits,
			mask_w,
			mask_ll,
			tuple_type: unsafe { mem::transmute(((enc_flags3 >> EncFlags3::TUPLE_TYPE_SHIFT) & EncFlags3::TUPLE_TYPE_MASK) as u8) },
			wbit,
		}
	}

	fn try_convert_to_disp8n(self_ptr: *const OpCodeHandler, encoder: &mut Encoder, displ: i32) -> Option<i8> {
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

		const_assert_eq!(1, EvexOpCodeTable::Table0F as u32);
		const_assert_eq!(2, EvexOpCodeTable::Table0F38 as u32);
		const_assert_eq!(3, EvexOpCodeTable::Table0F3A as u32);
		let mut b = this.table;
		const_assert_eq!(1, EncoderFlags::B);
		const_assert_eq!(2, EncoderFlags::X);
		const_assert_eq!(4, EncoderFlags::R);
		b |= (encoder_flags & 7) << 5;
		const_assert_eq!(0x0000_0200, EncoderFlags::R2);
		b |= (encoder_flags >> (9 - 4)) & 0x10;
		b ^= !0xF;
		encoder.write_byte_internal(b);

		b = this.p1_bits;
		b |= (!encoder_flags >> (EncoderFlags::VVVVV_SHIFT - 3)) & 0x78;
		b |= this.mask_w & encoder.internal_evex_wig;
		encoder.write_byte_internal(b);

		b = super::super::instruction_internal::internal_op_mask(instruction);
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
			const_assert_eq!(1, RoundingControl::RoundToNearest as u32);
			const_assert_eq!(2, RoundingControl::RoundDown as u32);
			const_assert_eq!(3, RoundingControl::RoundUp as u32);
			const_assert_eq!(4, RoundingControl::RoundTowardZero as u32);
			b |= (rc as u32 - RoundingControl::RoundToNearest as u32) << 5;
		} else if (this.base.enc_flags3 & EncFlags3::SUPPRESS_ALL_EXCEPTIONS) == 0 || !instruction.suppress_all_exceptions() {
			b |= this.ll_bits;
		}
		if (encoder_flags & EncoderFlags::BROADCAST) != 0 {
			if (this.base.enc_flags3 & EncFlags3::BROADCAST) == 0 {
				encoder.set_error_message_str("The instruction doesn't support broadcasting");
			}
			b |= 0x10;
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
				is_declare_data: false,
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
