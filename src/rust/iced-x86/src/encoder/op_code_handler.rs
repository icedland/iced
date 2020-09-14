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
	pub(crate) operands: Box<[&'static (Op + Sync)]>,
	pub(super) op_code: u32,
	pub(super) group_index: i32,
	pub(super) rm_group_index: i32,
	pub(super) flags: u32, // OpCodeHandlerFlags
	pub(super) encodable: Encodable,
	pub(super) op_size: OperandSize,
	pub(super) addr_size: AddressSize,
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
				flags: OpCodeHandlerFlags::NONE,
				encodable: Encodable::Any,
				op_size: OperandSize::None,
				addr_size: AddressSize::None,
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
				flags: OpCodeHandlerFlags::DECLARE_DATA,
				encodable: Encodable::Any,
				op_size: OperandSize::None,
				addr_size: AddressSize::None,
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
			encoder.write_byte_internal(instruction.get_declare_byte_value(i) as u32);
		}
	}
}

fn get_op_code(dword1: u32) -> u32 {
	dword1 >> EncFlags1::OP_CODE_SHIFT
}

#[repr(C)]
pub(super) struct LegacyHandler {
	base: OpCodeHandler,
	table_byte1: u32,
	table_byte2: u32,
	mandatory_prefix: u32,
}

impl LegacyHandler {
	pub(super) fn new(dword1: u32, dword2: u32, dword3: u32) -> Self {
		let group_index = if (dword2 & LegacyFlags::HAS_GROUP_INDEX) == 0 { -1 } else { ((dword2 >> LegacyFlags::GROUP_SHIFT) & 7) as i32 };
		let flags = if (dword2 & LegacyFlags::FWAIT) != 0 { OpCodeHandlerFlags::FWAIT } else { OpCodeHandlerFlags::NONE };
		let table: LegacyOpCodeTable =
			unsafe { mem::transmute(((dword2 >> LegacyFlags::LEGACY_OP_CODE_TABLE_SHIFT) & LegacyFlags::LEGACY_OP_CODE_TABLE_MASK) as u8) };
		let (table_byte1, table_byte2) = match table {
			LegacyOpCodeTable::Normal => (0, 0),
			LegacyOpCodeTable::Table0F => (0x0F, 0),
			LegacyOpCodeTable::Table0F38 => (0x0F, 0x38),
			LegacyOpCodeTable::Table0F3A => (0x0F, 0x3A),
		};
		let mpb: MandatoryPrefixByte =
			unsafe { mem::transmute(((dword2 >> LegacyFlags::MANDATORY_PREFIX_BYTE_SHIFT) & LegacyFlags::MANDATORY_PREFIX_BYTE_MASK) as u8) };
		let mandatory_prefix = match mpb {
			MandatoryPrefixByte::None => 0,
			MandatoryPrefixByte::P66 => 0x66,
			MandatoryPrefixByte::PF3 => 0xF3,
			MandatoryPrefixByte::PF2 => 0xF2,
		};

		let mut operands;
		let op0: LegacyOpKind = unsafe { mem::transmute(((dword3 >> LegacyFlags3::OP0_SHIFT) & LegacyFlags3::OP_MASK) as u8) };
		let op1: LegacyOpKind = unsafe { mem::transmute(((dword3 >> LegacyFlags3::OP1_SHIFT) & LegacyFlags3::OP_MASK) as u8) };
		let op2: LegacyOpKind = unsafe { mem::transmute(((dword3 >> LegacyFlags3::OP2_SHIFT) & LegacyFlags3::OP_MASK) as u8) };
		let op3: LegacyOpKind = unsafe { mem::transmute(((dword3 >> LegacyFlags3::OP3_SHIFT) & LegacyFlags3::OP_MASK) as u8) };
		if op3 != LegacyOpKind::None {
			operands = Vec::with_capacity(4);
			operands.push(LEGACY_TABLE[op0 as usize]);
			operands.push(LEGACY_TABLE[op1 as usize]);
			operands.push(LEGACY_TABLE[op2 as usize]);
			operands.push(LEGACY_TABLE[op3 as usize]);
		} else if op2 != LegacyOpKind::None {
			operands = Vec::with_capacity(3);
			operands.push(LEGACY_TABLE[op0 as usize]);
			operands.push(LEGACY_TABLE[op1 as usize]);
			operands.push(LEGACY_TABLE[op2 as usize]);
			debug_assert_eq!(LegacyOpKind::None, op3);
		} else if op1 != LegacyOpKind::None {
			operands = Vec::with_capacity(2);
			operands.push(LEGACY_TABLE[op0 as usize]);
			operands.push(LEGACY_TABLE[op1 as usize]);
			debug_assert_eq!(LegacyOpKind::None, op2);
			debug_assert_eq!(LegacyOpKind::None, op3);
		} else if op0 != LegacyOpKind::None {
			operands = Vec::with_capacity(1);
			operands.push(LEGACY_TABLE[op0 as usize]);
			debug_assert_eq!(LegacyOpKind::None, op1);
			debug_assert_eq!(LegacyOpKind::None, op2);
			debug_assert_eq!(LegacyOpKind::None, op3);
		} else {
			operands = Vec::new();
			debug_assert_eq!(LegacyOpKind::None, op0);
			debug_assert_eq!(LegacyOpKind::None, op1);
			debug_assert_eq!(LegacyOpKind::None, op2);
			debug_assert_eq!(LegacyOpKind::None, op3);
		}

		Self {
			base: OpCodeHandler {
				encode: Self::encode,
				try_convert_to_disp8n: None,
				operands: operands.into_boxed_slice(),
				op_code: get_op_code(dword1),
				group_index,
				rm_group_index: -1,
				flags,
				encodable: unsafe { mem::transmute(((dword2 >> LegacyFlags::ENCODABLE_SHIFT) & LegacyFlags::ENCODABLE_MASK) as u8) },
				op_size: unsafe { mem::transmute(((dword2 >> LegacyFlags::OPERAND_SIZE_SHIFT) & LegacyFlags::OPERAND_SIZE_MASK) as u8) },
				addr_size: unsafe { mem::transmute(((dword2 >> LegacyFlags::ADDRESS_SIZE_SHIFT) & LegacyFlags::ADDRESS_SIZE_MASK) as u8) },
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
	pub(super) fn new(dword1: u32, dword2: u32, dword3: u32) -> Self {
		let group_index = if (dword2 & VexFlags::HAS_GROUP_INDEX) == 0 { -1 } else { ((dword2 >> VexFlags::GROUP_SHIFT) & 7) as i32 };
		let rm_group_index = if (dword2 & VexFlags::HAS_RM_GROUP_INDEX) == 0 { -1 } else { ((dword2 >> VexFlags::GROUP_SHIFT) & 7) as i32 };
		let wbit: WBit = unsafe { mem::transmute(((dword2 >> VexFlags::WBIT_SHIFT) & VexFlags::WBIT_MASK) as u8) };
		let w1 = if wbit == WBit::W1 { u32::MAX } else { 0 };
		let vex_flags: VexVectorLength =
			unsafe { mem::transmute(((dword2 >> VexFlags::VEX_VECTOR_LENGTH_SHIFT) & VexFlags::VEX_VECTOR_LENGTH_MASK) as u8) };
		let mut last_byte = if vex_flags == VexVectorLength::L1 || vex_flags == VexVectorLength::L256 { 4 } else { 0 };
		if w1 != 0 {
			last_byte |= 0x80;
		}
		last_byte |= (dword2 >> VexFlags::MANDATORY_PREFIX_BYTE_SHIFT) & VexFlags::MANDATORY_PREFIX_BYTE_MASK;
		let mut mask_w_l = if wbit == WBit::WIG { 0x80 } else { 0 };
		let mask_l = if vex_flags == VexVectorLength::LIG {
			mask_w_l |= 4;
			4
		} else {
			0
		};

		let mut operands;
		let op0: VexOpKind = unsafe { mem::transmute(((dword3 >> VexFlags3::OP0_SHIFT) & VexFlags3::OP_MASK) as u8) };
		let op1: VexOpKind = unsafe { mem::transmute(((dword3 >> VexFlags3::OP1_SHIFT) & VexFlags3::OP_MASK) as u8) };
		let op2: VexOpKind = unsafe { mem::transmute(((dword3 >> VexFlags3::OP2_SHIFT) & VexFlags3::OP_MASK) as u8) };
		let op3: VexOpKind = unsafe { mem::transmute(((dword3 >> VexFlags3::OP3_SHIFT) & VexFlags3::OP_MASK) as u8) };
		let op4: VexOpKind = unsafe { mem::transmute(((dword3 >> VexFlags3::OP4_SHIFT) & VexFlags3::OP_MASK) as u8) };
		if op4 != VexOpKind::None {
			operands = Vec::with_capacity(5);
			operands.push(VEX_TABLE[op0 as usize]);
			operands.push(VEX_TABLE[op1 as usize]);
			operands.push(VEX_TABLE[op2 as usize]);
			operands.push(VEX_TABLE[op3 as usize]);
			operands.push(VEX_TABLE[op4 as usize]);
		} else if op3 != VexOpKind::None {
			operands = Vec::with_capacity(4);
			operands.push(VEX_TABLE[op0 as usize]);
			operands.push(VEX_TABLE[op1 as usize]);
			operands.push(VEX_TABLE[op2 as usize]);
			operands.push(VEX_TABLE[op3 as usize]);
			debug_assert_eq!(VexOpKind::None, op4);
		} else if op2 != VexOpKind::None {
			operands = Vec::with_capacity(3);
			operands.push(VEX_TABLE[op0 as usize]);
			operands.push(VEX_TABLE[op1 as usize]);
			operands.push(VEX_TABLE[op2 as usize]);
			debug_assert_eq!(VexOpKind::None, op3);
			debug_assert_eq!(VexOpKind::None, op4);
		} else if op1 != VexOpKind::None {
			operands = Vec::with_capacity(2);
			operands.push(VEX_TABLE[op0 as usize]);
			operands.push(VEX_TABLE[op1 as usize]);
			debug_assert_eq!(VexOpKind::None, op2);
			debug_assert_eq!(VexOpKind::None, op3);
			debug_assert_eq!(VexOpKind::None, op4);
		} else if op0 != VexOpKind::None {
			operands = Vec::with_capacity(1);
			operands.push(VEX_TABLE[op0 as usize]);
			debug_assert_eq!(VexOpKind::None, op1);
			debug_assert_eq!(VexOpKind::None, op2);
			debug_assert_eq!(VexOpKind::None, op3);
			debug_assert_eq!(VexOpKind::None, op4);
		} else {
			operands = Vec::new();
			debug_assert_eq!(VexOpKind::None, op0);
			debug_assert_eq!(VexOpKind::None, op1);
			debug_assert_eq!(VexOpKind::None, op2);
			debug_assert_eq!(VexOpKind::None, op3);
			debug_assert_eq!(VexOpKind::None, op4);
		}

		Self {
			base: OpCodeHandler {
				encode: Self::encode,
				try_convert_to_disp8n: None,
				operands: operands.into_boxed_slice(),
				op_code: get_op_code(dword1),
				group_index,
				rm_group_index,
				flags: OpCodeHandlerFlags::NONE,
				encodable: unsafe { mem::transmute(((dword2 >> VexFlags::ENCODABLE_SHIFT) & VexFlags::ENCODABLE_MASK) as u8) },
				op_size: OperandSize::None,
				addr_size: AddressSize::None,
			},
			table: (dword2 >> VexFlags::VEX_OP_CODE_TABLE_SHIFT) & VexFlags::VEX_OP_CODE_TABLE_MASK,
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
	pub(super) fn new(dword1: u32, dword2: u32, dword3: u32) -> Self {
		const_assert_eq!(0, XopOpCodeTable::XOP8 as u32);
		const_assert_eq!(1, XopOpCodeTable::XOP9 as u32);
		const_assert_eq!(2, XopOpCodeTable::XOPA as u32);
		let group_index = if (dword2 & XopFlags::HAS_GROUP_INDEX) == 0 { -1 } else { ((dword2 >> XopFlags::GROUP_SHIFT) & 7) as i32 };
		let mut last_byte = (dword2 >> (XopFlags::XOP_VECTOR_LENGTH_SHIFT - 2)) & 4;
		let wbit: WBit = unsafe { mem::transmute(((dword2 >> XopFlags::WBIT_SHIFT) & XopFlags::WBIT_MASK) as u8) };
		if wbit == WBit::W1 {
			last_byte |= 0x80;
		}
		last_byte |= (dword2 >> XopFlags::MANDATORY_PREFIX_BYTE_SHIFT) & XopFlags::MANDATORY_PREFIX_BYTE_MASK;

		let mut operands;
		let op0: XopOpKind = unsafe { mem::transmute(((dword3 >> XopFlags3::OP0_SHIFT) & XopFlags3::OP_MASK) as u8) };
		let op1: XopOpKind = unsafe { mem::transmute(((dword3 >> XopFlags3::OP1_SHIFT) & XopFlags3::OP_MASK) as u8) };
		let op2: XopOpKind = unsafe { mem::transmute(((dword3 >> XopFlags3::OP2_SHIFT) & XopFlags3::OP_MASK) as u8) };
		let op3: XopOpKind = unsafe { mem::transmute(((dword3 >> XopFlags3::OP3_SHIFT) & XopFlags3::OP_MASK) as u8) };
		if op3 != XopOpKind::None {
			operands = Vec::with_capacity(4);
			operands.push(XOP_TABLE[op0 as usize]);
			operands.push(XOP_TABLE[op1 as usize]);
			operands.push(XOP_TABLE[op2 as usize]);
			operands.push(XOP_TABLE[op3 as usize]);
		} else if op2 != XopOpKind::None {
			operands = Vec::with_capacity(3);
			operands.push(XOP_TABLE[op0 as usize]);
			operands.push(XOP_TABLE[op1 as usize]);
			operands.push(XOP_TABLE[op2 as usize]);
			debug_assert_eq!(XopOpKind::None, op3);
		} else if op1 != XopOpKind::None {
			operands = Vec::with_capacity(2);
			operands.push(XOP_TABLE[op0 as usize]);
			operands.push(XOP_TABLE[op1 as usize]);
			debug_assert_eq!(XopOpKind::None, op2);
			debug_assert_eq!(XopOpKind::None, op3);
		} else if op0 != XopOpKind::None {
			operands = Vec::with_capacity(1);
			operands.push(XOP_TABLE[op0 as usize]);
			debug_assert_eq!(XopOpKind::None, op1);
			debug_assert_eq!(XopOpKind::None, op2);
			debug_assert_eq!(XopOpKind::None, op3);
		} else {
			operands = Vec::new();
			debug_assert_eq!(XopOpKind::None, op0);
			debug_assert_eq!(XopOpKind::None, op1);
			debug_assert_eq!(XopOpKind::None, op2);
			debug_assert_eq!(XopOpKind::None, op3);
		}

		Self {
			base: OpCodeHandler {
				encode: Self::encode,
				try_convert_to_disp8n: None,
				operands: operands.into_boxed_slice(),
				op_code: get_op_code(dword1),
				group_index,
				rm_group_index: -1,
				flags: OpCodeHandlerFlags::NONE,
				encodable: unsafe { mem::transmute(((dword2 >> XopFlags::ENCODABLE_SHIFT) & XopFlags::ENCODABLE_MASK) as u8) },
				op_size: OperandSize::None,
				addr_size: AddressSize::None,
			},
			table: 8 + ((dword2 >> XopFlags::XOP_OP_CODE_TABLE_SHIFT) & XopFlags::XOP_OP_CODE_TABLE_MASK),
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
	flags: u32, // EvexFlags
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
	pub(super) fn new(dword1: u32, dword2: u32, dword3: u32) -> Self {
		let group_index = if (dword2 & EvexFlags::HAS_GROUP_INDEX) == 0 { -1 } else { ((dword2 >> EvexFlags::GROUP_SHIFT) & 7) as i32 };
		const_assert_eq!(0, MandatoryPrefixByte::None as u32);
		const_assert_eq!(1, MandatoryPrefixByte::P66 as u32);
		const_assert_eq!(2, MandatoryPrefixByte::PF3 as u32);
		const_assert_eq!(3, MandatoryPrefixByte::PF2 as u32);
		let mut p1_bits = 4 | ((dword2 >> EvexFlags::MANDATORY_PREFIX_BYTE_SHIFT) & EvexFlags::MANDATORY_PREFIX_BYTE_MASK);
		let wbit: WBit = unsafe { mem::transmute(((dword2 >> EvexFlags::WBIT_SHIFT) & EvexFlags::WBIT_MASK) as u8) };
		if wbit == WBit::W1 {
			p1_bits |= 0x80
		}
		const_assert_eq!(3, EvexFlags::EVEX_VECTOR_LENGTH_MASK);
		let ll_bits = (dword2 >> (EvexFlags::EVEX_VECTOR_LENGTH_SHIFT - 5)) & 0x60;
		let mask_w = if wbit == WBit::WIG { 0x80 } else { 0 };
		let mask_ll = if (dword2 & EvexFlags::LIG) != 0 { 0x60 } else { 0 };

		let mut operands;
		let op0: EvexOpKind = unsafe { mem::transmute(((dword3 >> EvexFlags3::OP0_SHIFT) & EvexFlags3::OP_MASK) as u8) };
		let op1: EvexOpKind = unsafe { mem::transmute(((dword3 >> EvexFlags3::OP1_SHIFT) & EvexFlags3::OP_MASK) as u8) };
		let op2: EvexOpKind = unsafe { mem::transmute(((dword3 >> EvexFlags3::OP2_SHIFT) & EvexFlags3::OP_MASK) as u8) };
		let op3: EvexOpKind = unsafe { mem::transmute(((dword3 >> EvexFlags3::OP3_SHIFT) & EvexFlags3::OP_MASK) as u8) };
		if op3 != EvexOpKind::None {
			operands = Vec::with_capacity(4);
			operands.push(EVEX_TABLE[op0 as usize]);
			operands.push(EVEX_TABLE[op1 as usize]);
			operands.push(EVEX_TABLE[op2 as usize]);
			operands.push(EVEX_TABLE[op3 as usize]);
		} else if op2 != EvexOpKind::None {
			operands = Vec::with_capacity(3);
			operands.push(EVEX_TABLE[op0 as usize]);
			operands.push(EVEX_TABLE[op1 as usize]);
			operands.push(EVEX_TABLE[op2 as usize]);
			debug_assert_eq!(EvexOpKind::None, op3);
		} else if op1 != EvexOpKind::None {
			operands = Vec::with_capacity(2);
			operands.push(EVEX_TABLE[op0 as usize]);
			operands.push(EVEX_TABLE[op1 as usize]);
			debug_assert_eq!(EvexOpKind::None, op2);
			debug_assert_eq!(EvexOpKind::None, op3);
		} else if op0 != EvexOpKind::None {
			operands = Vec::with_capacity(1);
			operands.push(EVEX_TABLE[op0 as usize]);
			debug_assert_eq!(EvexOpKind::None, op1);
			debug_assert_eq!(EvexOpKind::None, op2);
			debug_assert_eq!(EvexOpKind::None, op3);
		} else {
			operands = Vec::new();
			debug_assert_eq!(EvexOpKind::None, op0);
			debug_assert_eq!(EvexOpKind::None, op1);
			debug_assert_eq!(EvexOpKind::None, op2);
			debug_assert_eq!(EvexOpKind::None, op3);
		}

		Self {
			base: OpCodeHandler {
				encode: Self::encode,
				try_convert_to_disp8n: Some(Self::try_convert_to_disp8n),
				operands: operands.into_boxed_slice(),
				op_code: get_op_code(dword1),
				group_index,
				rm_group_index: -1,
				flags: OpCodeHandlerFlags::NONE,
				encodable: unsafe { mem::transmute(((dword2 >> EvexFlags::ENCODABLE_SHIFT) & EvexFlags::ENCODABLE_MASK) as u8) },
				op_size: OperandSize::None,
				addr_size: AddressSize::None,
			},
			flags: dword2,
			table: (dword2 >> EvexFlags::EVEX_OP_CODE_TABLE_SHIFT) & EvexFlags::EVEX_OP_CODE_TABLE_MASK,
			p1_bits,
			ll_bits,
			mask_w,
			mask_ll,
			tuple_type: unsafe { mem::transmute(((dword2 >> EvexFlags::TUPLE_TYPE_SHIFT) & EvexFlags::TUPLE_TYPE_MASK) as u8) },
			wbit,
		}
	}

	fn try_convert_to_disp8n(self_ptr: *const OpCodeHandler, encoder: &mut Encoder, displ: i32) -> Option<i8> {
		let this = unsafe { &*(self_ptr as *const Self) };
		let n = match this.tuple_type {
			TupleType::N1 => 1,
			TupleType::N2 => 2,
			TupleType::N4 => 4,
			TupleType::N8 => 8,
			TupleType::N16 => 16,
			TupleType::N32 => 32,
			TupleType::N64 => 64,
			TupleType::N8b4 => {
				if (encoder.encoder_flags & EncoderFlags::BROADCAST) != 0 {
					4
				} else {
					8
				}
			}
			TupleType::N16b4 => {
				if (encoder.encoder_flags & EncoderFlags::BROADCAST) != 0 {
					4
				} else {
					16
				}
			}
			TupleType::N32b4 => {
				if (encoder.encoder_flags & EncoderFlags::BROADCAST) != 0 {
					4
				} else {
					32
				}
			}
			TupleType::N64b4 => {
				if (encoder.encoder_flags & EncoderFlags::BROADCAST) != 0 {
					4
				} else {
					64
				}
			}
			TupleType::N16b8 => {
				if (encoder.encoder_flags & EncoderFlags::BROADCAST) != 0 {
					8
				} else {
					16
				}
			}
			TupleType::N32b8 => {
				if (encoder.encoder_flags & EncoderFlags::BROADCAST) != 0 {
					8
				} else {
					32
				}
			}
			TupleType::N64b8 => {
				if (encoder.encoder_flags & EncoderFlags::BROADCAST) != 0 {
					8
				} else {
					64
				}
			}
		};
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
			if (this.flags & EvexFlags::K1) == 0 {
				encoder.set_error_message_str("The instruction doesn't support opmask registers");
			}
		} else {
			if (this.flags & EvexFlags::NON_ZERO_OP_MASK_REGISTER) != 0 {
				encoder.set_error_message_str("The instruction must use an opmask register");
			}
		}
		b |= (encoder_flags >> (EncoderFlags::VVVVV_SHIFT + 4 - 3)) & 8;
		if instruction.suppress_all_exceptions() {
			if (this.flags & EvexFlags::SAE) == 0 {
				encoder.set_error_message_str("The instruction doesn't support suppress-all-exceptions");
			}
			b |= 0x10;
		}
		let rc = instruction.rounding_control();
		if rc != RoundingControl::None {
			if (this.flags & EvexFlags::ER) == 0 {
				encoder.set_error_message_str("The instruction doesn't support rounding control");
			}
			b |= 0x10;
			const_assert_eq!(1, RoundingControl::RoundToNearest as u32);
			const_assert_eq!(2, RoundingControl::RoundDown as u32);
			const_assert_eq!(3, RoundingControl::RoundUp as u32);
			const_assert_eq!(4, RoundingControl::RoundTowardZero as u32);
			b |= (rc as u32 - RoundingControl::RoundToNearest as u32) << 5;
		} else if (this.flags & EvexFlags::SAE) == 0 || !instruction.suppress_all_exceptions() {
			b |= this.ll_bits;
		}
		if (encoder_flags & EncoderFlags::BROADCAST) != 0 {
			if (this.flags & EvexFlags::B) == 0 {
				encoder.set_error_message_str("The instruction doesn't support broadcasting");
			}
			b |= 0x10;
		}
		if instruction.zeroing_masking() {
			if (this.flags & EvexFlags::Z) == 0 {
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
	pub(super) fn new(dword1: u32, dword2: u32, _dword3: u32) -> Self {
		let mut operands = Vec::with_capacity(2);
		static D3NOW_TABLE: [&(Op + Sync); 2] =
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
				flags: OpCodeHandlerFlags::NONE,
				encodable: unsafe { mem::transmute(((dword2 >> D3nowFlags::ENCODABLE_SHIFT) & D3nowFlags::ENCODABLE_MASK) as u8) },
				op_size: OperandSize::None,
				addr_size: AddressSize::None,
			},
			immediate: get_op_code(dword1),
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
