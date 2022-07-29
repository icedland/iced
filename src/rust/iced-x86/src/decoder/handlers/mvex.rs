// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::decoder::handlers::*;
use crate::decoder::*;
use crate::iced_constants::IcedConstants;
use crate::instruction_internal;
use crate::mvex::get_mvex_info;
use crate::mvex::mvex_tt_lut::MVEX_TUPLE_TYPE_LUT;
use crate::*;

// SAFETY:
//	code: let this = unsafe { &*(self_ptr as *const Self) };
// The first arg (`self_ptr`) to decode() is always the handler itself, cast to a `*const OpCodeHandler`.
// All handlers are `#[repr(C)]` structs so the OpCodeHandler fields are always at the same offsets.

macro_rules! write_eviction_hint {
	($decoder:ident, $instruction:ident) => {
		if ($decoder.state.flags & StateFlags::MVEX_EH) != 0 {
			$instruction.set_is_mvex_eviction_hint(true);
		}
	};
}

macro_rules! write_mem_conv {
	($decoder:ident, $instruction:ident, $mvex:ident, $sss:ident) => {
		if (($mvex.invalid_conv_fns as u32) & (1 << $sss) & $decoder.invalid_check_mask) != 0 {
			$decoder.set_invalid_instruction();
		}
		const _: () = assert!(MvexRegMemConv::MemConvNone as u32 + 1 == MvexRegMemConv::MemConvBroadcast1 as u32);
		const _: () = assert!(MvexRegMemConv::MemConvNone as u32 + 2 == MvexRegMemConv::MemConvBroadcast4 as u32);
		const _: () = assert!(MvexRegMemConv::MemConvNone as u32 + 3 == MvexRegMemConv::MemConvFloat16 as u32);
		const _: () = assert!(MvexRegMemConv::MemConvNone as u32 + 4 == MvexRegMemConv::MemConvUint8 as u32);
		const _: () = assert!(MvexRegMemConv::MemConvNone as u32 + 5 == MvexRegMemConv::MemConvSint8 as u32);
		const _: () = assert!(MvexRegMemConv::MemConvNone as u32 + 6 == MvexRegMemConv::MemConvUint16 as u32);
		const _: () = assert!(MvexRegMemConv::MemConvNone as u32 + 7 == MvexRegMemConv::MemConvSint16 as u32);
		const _: () = assert!(StateFlags::MVEX_SSS_MASK == 7);
		debug_assert!($sss <= 7);
		instruction_internal::internal_set_mvex_reg_mem_conv($instruction, MvexRegMemConv::MemConvNone as u32 + $sss);
	};
}

macro_rules! write_reg_conv_and_er_sae {
	($decoder:ident, $instruction:ident, $mvex:ident, $sss:ident) => {
		if ($decoder.state.flags & StateFlags::MVEX_EH) != 0 {
			if $mvex.can_use_suppress_all_exceptions() {
				if ($sss & 4) != 0 {
					$instruction.set_suppress_all_exceptions(true);
				}
				if $mvex.can_use_rounding_control() {
					const _: () = assert!(RoundingControl::None as u32 == 0);
					const _: () = assert!(RoundingControl::RoundToNearest as u32 == 1);
					const _: () = assert!(RoundingControl::RoundDown as u32 == 2);
					const _: () = assert!(RoundingControl::RoundUp as u32 == 3);
					const _: () = assert!(RoundingControl::RoundTowardZero as u32 == 4);
					instruction_internal::internal_set_rounding_control($instruction, ($sss & 3) + RoundingControl::RoundToNearest as u32);
				}
			} else if $mvex.no_sae_rc() && ($sss & $decoder.invalid_check_mask) != 0 {
				$decoder.set_invalid_instruction();
			}
		} else {
			if (($mvex.invalid_swizzle_fns as u32) & (1 << $sss) & $decoder.invalid_check_mask) != 0 {
				$decoder.set_invalid_instruction();
			}
			const _: () = assert!(MvexRegMemConv::RegSwizzleNone as u32 + 1 == MvexRegMemConv::RegSwizzleCdab as u32);
			const _: () = assert!(MvexRegMemConv::RegSwizzleNone as u32 + 2 == MvexRegMemConv::RegSwizzleBadc as u32);
			const _: () = assert!(MvexRegMemConv::RegSwizzleNone as u32 + 3 == MvexRegMemConv::RegSwizzleDacb as u32);
			const _: () = assert!(MvexRegMemConv::RegSwizzleNone as u32 + 4 == MvexRegMemConv::RegSwizzleAaaa as u32);
			const _: () = assert!(MvexRegMemConv::RegSwizzleNone as u32 + 5 == MvexRegMemConv::RegSwizzleBbbb as u32);
			const _: () = assert!(MvexRegMemConv::RegSwizzleNone as u32 + 6 == MvexRegMemConv::RegSwizzleCccc as u32);
			const _: () = assert!(MvexRegMemConv::RegSwizzleNone as u32 + 7 == MvexRegMemConv::RegSwizzleDddd as u32);
			const _: () = assert!(StateFlags::MVEX_SSS_MASK == 7);
			debug_assert!($sss <= 7);
			instruction_internal::internal_set_mvex_reg_mem_conv($instruction, MvexRegMemConv::RegSwizzleNone as u32 + $sss);
		}
	};
}

// Only called by the MVEX opcode handlers and they only pass in 0<=sss<=7
#[inline(always)]
fn get_tuple_type(lut: MvexTupleTypeLutKind, sss: u32) -> TupleType {
	const _: () = assert!(StateFlags::MVEX_SSS_MASK == 7);
	debug_assert!(sss <= 7);
	debug_assert_eq!(MVEX_TUPLE_TYPE_LUT.len(), IcedConstants::MVEX_TUPLE_TYPE_LUT_KIND_ENUM_COUNT * (StateFlags::MVEX_SSS_MASK as usize + 1));
	// SAFETY: valid index, see above
	unsafe { *MVEX_TUPLE_TYPE_LUT.get_unchecked((lut as usize) * (StateFlags::MVEX_SSS_MASK as usize + 1) + sss as usize) }
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_EH {
	has_modrm: bool,
	handlers: [(OpCodeHandlerDecodeFn, &'static OpCodeHandler); 2],
}

impl OpCodeHandler_EH {
	#[inline]
	pub(in crate::decoder) fn new(
		handler_eh0: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), handler_eh1: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
	) -> (OpCodeHandlerDecodeFn, Self) {
		debug_assert!(!is_null_instance_handler(handler_eh0.1));
		debug_assert!(!is_null_instance_handler(handler_eh1.1));
		(OpCodeHandler_EH::decode, Self { has_modrm: true, handlers: [handler_eh0, handler_eh1] })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::MVEX as u32);
		let (decode, handler) = this.handlers[((decoder.state.flags & StateFlags::MVEX_EH) != 0) as usize];
		(decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_MVEX_M {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_MVEX_M {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		debug_assert!(get_mvex_info(code).ignores_op_mask_register());
		debug_assert!(!get_mvex_info(code).can_use_eviction_hint());
		debug_assert!(get_mvex_info(code).ignores_eviction_hint());
		(OpCodeHandler_MVEX_M::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::MVEX as u32);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_op_mask(Register::None); // It's ignored (see ctor)
		instruction.set_code(this.code);
		let mvex = get_mvex_info(this.code);
		let sss = decoder.state.sss();
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			instruction.set_op0_kind(OpKind::Memory);
			write_mem_conv!(decoder, instruction, mvex, sss);
			decoder.read_op_mem_tuple_type(instruction, get_tuple_type(mvex.tuple_type_lut_kind, sss));
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_MVEX_MV {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_MVEX_MV {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		debug_assert!(!get_mvex_info(code).ignores_op_mask_register());
		(OpCodeHandler_MVEX_MV::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::MVEX as u32);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);
		write_op1_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + Register::ZMM0 as u32
		);
		let mvex = get_mvex_info(this.code);
		let sss = decoder.state.sss();
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			instruction.set_op0_kind(OpKind::Memory);
			if mvex.can_use_eviction_hint() {
				write_eviction_hint!(decoder, instruction);
			}
			write_mem_conv!(decoder, instruction, mvex, sss);
			decoder.read_op_mem_tuple_type(instruction, get_tuple_type(mvex.tuple_type_lut_kind, sss));
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_MVEX_VW {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_MVEX_VW {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		debug_assert!(!get_mvex_info(code).ignores_op_mask_register());
		debug_assert!(get_mvex_info(code).can_use_eviction_hint());
		debug_assert!(!get_mvex_info(code).ignores_eviction_hint());
		(OpCodeHandler_MVEX_VW::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::MVEX as u32);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);

		write_op0_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + Register::ZMM0 as u32
		);
		let mvex = get_mvex_info(this.code);
		let sss = decoder.state.sss();
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base_evex + Register::ZMM0 as u32);
			write_reg_conv_and_er_sae!(decoder, instruction, mvex, sss);
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			write_eviction_hint!(decoder, instruction);
			write_mem_conv!(decoder, instruction, mvex, sss);
			decoder.read_op_mem_tuple_type(instruction, get_tuple_type(mvex.tuple_type_lut_kind, sss));
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_MVEX_HWIb {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_MVEX_HWIb {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		debug_assert!(!get_mvex_info(code).ignores_op_mask_register());
		debug_assert!(get_mvex_info(code).can_use_eviction_hint());
		debug_assert!(!get_mvex_info(code).ignores_eviction_hint());
		(OpCodeHandler_MVEX_HWIb::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::MVEX as u32);
		instruction.set_code(this.code);

		write_op0_reg!(instruction, decoder.state.vvvv + Register::ZMM0 as u32);
		instruction.set_op2_kind(OpKind::Immediate8);
		let mvex = get_mvex_info(this.code);
		let sss = decoder.state.sss();
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base_evex + Register::ZMM0 as u32);
			write_reg_conv_and_er_sae!(decoder, instruction, mvex, sss);
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			write_eviction_hint!(decoder, instruction);
			write_mem_conv!(decoder, instruction, mvex, sss);
			decoder.read_op_mem_tuple_type(instruction, get_tuple_type(mvex.tuple_type_lut_kind, sss));
		}
		instruction.set_immediate8(decoder.read_u8() as u8);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_MVEX_VWIb {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_MVEX_VWIb {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		debug_assert!(!get_mvex_info(code).ignores_op_mask_register());
		debug_assert!(get_mvex_info(code).can_use_eviction_hint());
		debug_assert!(!get_mvex_info(code).ignores_eviction_hint());
		(OpCodeHandler_MVEX_VWIb::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::MVEX as u32);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);

		write_op0_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + Register::ZMM0 as u32
		);
		instruction.set_op2_kind(OpKind::Immediate8);
		let mvex = get_mvex_info(this.code);
		let sss = decoder.state.sss();
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base_evex + Register::ZMM0 as u32);
			write_reg_conv_and_er_sae!(decoder, instruction, mvex, sss);
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			write_eviction_hint!(decoder, instruction);
			write_mem_conv!(decoder, instruction, mvex, sss);
			decoder.read_op_mem_tuple_type(instruction, get_tuple_type(mvex.tuple_type_lut_kind, sss));
		}
		instruction.set_immediate8(decoder.read_u8() as u8);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_MVEX_VHW {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_MVEX_VHW {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		debug_assert!(!get_mvex_info(code).ignores_op_mask_register());
		debug_assert!(get_mvex_info(code).can_use_eviction_hint());
		debug_assert!(!get_mvex_info(code).ignores_eviction_hint());
		(OpCodeHandler_MVEX_VHW::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::MVEX as u32);
		instruction.set_code(this.code);

		write_op0_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + Register::ZMM0 as u32
		);
		write_op1_reg!(instruction, decoder.state.vvvv + Register::ZMM0 as u32);
		let mvex = get_mvex_info(this.code);
		if mvex.require_op_mask_register() && decoder.invalid_check_mask != 0 && decoder.state.aaa == 0 {
			decoder.set_invalid_instruction();
		}
		let sss = decoder.state.sss();
		if decoder.state.mod_ == 3 {
			write_op2_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base_evex + Register::ZMM0 as u32);
			write_reg_conv_and_er_sae!(decoder, instruction, mvex, sss);
		} else {
			instruction.set_op2_kind(OpKind::Memory);
			write_eviction_hint!(decoder, instruction);
			write_mem_conv!(decoder, instruction, mvex, sss);
			decoder.read_op_mem_tuple_type(instruction, get_tuple_type(mvex.tuple_type_lut_kind, sss));
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_MVEX_VHWIb {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_MVEX_VHWIb {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		debug_assert!(!get_mvex_info(code).ignores_op_mask_register());
		debug_assert!(get_mvex_info(code).can_use_eviction_hint());
		debug_assert!(!get_mvex_info(code).ignores_eviction_hint());
		(OpCodeHandler_MVEX_VHWIb::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::MVEX as u32);
		instruction.set_code(this.code);

		write_op0_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + Register::ZMM0 as u32
		);
		write_op1_reg!(instruction, decoder.state.vvvv + Register::ZMM0 as u32);
		instruction.set_op3_kind(OpKind::Immediate8);
		let mvex = get_mvex_info(this.code);
		let sss = decoder.state.sss();
		if decoder.state.mod_ == 3 {
			write_op2_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base_evex + Register::ZMM0 as u32);
			write_reg_conv_and_er_sae!(decoder, instruction, mvex, sss);
		} else {
			instruction.set_op2_kind(OpKind::Memory);
			write_eviction_hint!(decoder, instruction);
			write_mem_conv!(decoder, instruction, mvex, sss);
			decoder.read_op_mem_tuple_type(instruction, get_tuple_type(mvex.tuple_type_lut_kind, sss));
		}
		instruction.set_immediate8(decoder.read_u8() as u8);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_MVEX_VKW {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_MVEX_VKW {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		debug_assert!(!get_mvex_info(code).ignores_op_mask_register());
		debug_assert!(get_mvex_info(code).can_use_eviction_hint());
		debug_assert!(!get_mvex_info(code).ignores_eviction_hint());
		(OpCodeHandler_MVEX_VKW::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::MVEX as u32);
		if (decoder.state.vvvv & decoder.invalid_check_mask) > 7 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);

		write_op0_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + Register::ZMM0 as u32
		);
		write_op1_reg!(instruction, (decoder.state.vvvv & 7) + Register::K0 as u32);
		let mvex = get_mvex_info(this.code);
		let sss = decoder.state.sss();
		if decoder.state.mod_ == 3 {
			write_op2_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base_evex + Register::ZMM0 as u32);
			write_reg_conv_and_er_sae!(decoder, instruction, mvex, sss);
		} else {
			instruction.set_op2_kind(OpKind::Memory);
			write_eviction_hint!(decoder, instruction);
			write_mem_conv!(decoder, instruction, mvex, sss);
			decoder.read_op_mem_tuple_type(instruction, get_tuple_type(mvex.tuple_type_lut_kind, sss));
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_MVEX_KHW {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_MVEX_KHW {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		debug_assert!(!get_mvex_info(code).ignores_op_mask_register());
		debug_assert!(get_mvex_info(code).can_use_eviction_hint());
		debug_assert!(!get_mvex_info(code).ignores_eviction_hint());
		(OpCodeHandler_MVEX_KHW::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::MVEX as u32);
		instruction.set_code(this.code);

		write_op0_reg!(instruction, decoder.state.reg + Register::K0 as u32);
		write_op1_reg!(instruction, decoder.state.vvvv + Register::ZMM0 as u32);
		if ((decoder.state.extra_register_base | decoder.state.extra_register_base_evex) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		let mvex = get_mvex_info(this.code);
		let sss = decoder.state.sss();
		if decoder.state.mod_ == 3 {
			write_op2_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base_evex + Register::ZMM0 as u32);
			write_reg_conv_and_er_sae!(decoder, instruction, mvex, sss);
		} else {
			instruction.set_op2_kind(OpKind::Memory);
			write_eviction_hint!(decoder, instruction);
			write_mem_conv!(decoder, instruction, mvex, sss);
			decoder.read_op_mem_tuple_type(instruction, get_tuple_type(mvex.tuple_type_lut_kind, sss));
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_MVEX_KHWIb {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_MVEX_KHWIb {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		debug_assert!(!get_mvex_info(code).ignores_op_mask_register());
		debug_assert!(get_mvex_info(code).can_use_eviction_hint());
		debug_assert!(!get_mvex_info(code).ignores_eviction_hint());
		(OpCodeHandler_MVEX_KHWIb::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::MVEX as u32);
		instruction.set_code(this.code);

		write_op0_reg!(instruction, decoder.state.reg + Register::K0 as u32);
		write_op1_reg!(instruction, decoder.state.vvvv + Register::ZMM0 as u32);
		if ((decoder.state.extra_register_base | decoder.state.extra_register_base_evex) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_op3_kind(OpKind::Immediate8);
		let mvex = get_mvex_info(this.code);
		let sss = decoder.state.sss();
		if decoder.state.mod_ == 3 {
			write_op2_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base_evex + Register::ZMM0 as u32);
			write_reg_conv_and_er_sae!(decoder, instruction, mvex, sss);
		} else {
			instruction.set_op2_kind(OpKind::Memory);
			write_eviction_hint!(decoder, instruction);
			write_mem_conv!(decoder, instruction, mvex, sss);
			decoder.read_op_mem_tuple_type(instruction, get_tuple_type(mvex.tuple_type_lut_kind, sss));
		}
		instruction.set_immediate8(decoder.read_u8() as u8);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_MVEX_VSIB {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_MVEX_VSIB {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		debug_assert!(!get_mvex_info(code).ignores_op_mask_register());
		debug_assert!(get_mvex_info(code).can_use_eviction_hint());
		debug_assert!(!get_mvex_info(code).ignores_eviction_hint());
		(OpCodeHandler_MVEX_VSIB::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::MVEX as u32);
		if decoder.invalid_check_mask != 0 && ((decoder.state.vvvv_invalid_check & 0xF) != 0 || decoder.state.aaa == 0) {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);

		let mvex = get_mvex_info(this.code);
		let sss = decoder.state.sss();
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			instruction.set_op0_kind(OpKind::Memory);
			write_eviction_hint!(decoder, instruction);
			write_mem_conv!(decoder, instruction, mvex, sss);
			decoder.read_op_mem_vsib(instruction, Register::ZMM0, get_tuple_type(mvex.tuple_type_lut_kind, sss));
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_MVEX_VSIB_V {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_MVEX_VSIB_V {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		debug_assert!(!get_mvex_info(code).ignores_op_mask_register());
		debug_assert!(get_mvex_info(code).can_use_eviction_hint());
		debug_assert!(!get_mvex_info(code).ignores_eviction_hint());
		(OpCodeHandler_MVEX_VSIB_V::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::MVEX as u32);
		if decoder.invalid_check_mask != 0 && ((decoder.state.vvvv_invalid_check & 0xF) != 0 || decoder.state.aaa == 0) {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);

		write_op1_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + Register::ZMM0 as u32
		);
		let mvex = get_mvex_info(this.code);
		let sss = decoder.state.sss();
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			instruction.set_op0_kind(OpKind::Memory);
			write_eviction_hint!(decoder, instruction);
			write_mem_conv!(decoder, instruction, mvex, sss);
			decoder.read_op_mem_vsib(instruction, Register::ZMM0, get_tuple_type(mvex.tuple_type_lut_kind, sss));
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_MVEX_V_VSIB {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_MVEX_V_VSIB {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		debug_assert!(!get_mvex_info(code).ignores_op_mask_register());
		debug_assert!(get_mvex_info(code).can_use_eviction_hint());
		debug_assert!(!get_mvex_info(code).ignores_eviction_hint());
		(OpCodeHandler_MVEX_V_VSIB::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::MVEX as u32);
		if decoder.invalid_check_mask != 0 && ((decoder.state.vvvv_invalid_check & 0xF) != 0 || decoder.state.aaa == 0) {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);

		let reg_num = decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex;
		write_op0_reg!(instruction, reg_num + Register::ZMM0 as u32);
		let mvex = get_mvex_info(this.code);
		let sss = decoder.state.sss();
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			write_eviction_hint!(decoder, instruction);
			write_mem_conv!(decoder, instruction, mvex, sss);
			decoder.read_op_mem_vsib(instruction, Register::ZMM0, get_tuple_type(mvex.tuple_type_lut_kind, sss));
			if decoder.invalid_check_mask != 0 {
				if reg_num == ((instruction.memory_index() as u32).wrapping_sub(Register::XMM0 as u32) % IcedConstants::VMM_COUNT) {
					decoder.set_invalid_instruction();
				}
			}
		}
	}
}
