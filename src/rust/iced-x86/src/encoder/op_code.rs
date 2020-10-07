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
use super::instruction_fmt::*;
use super::op_code_fmt::*;
use super::op_kind_tables::*;
#[cfg(not(feature = "std"))]
use alloc::string::String;
use core::{fmt, mem};

struct Flags;
impl Flags {
	pub const NONE: u8 = 0;
	pub const IGNORES_ROUNDING_CONTROL: u8 = 0x01;
	pub const AMD_LOCK_REG_BIT: u8 = 0x02;
	pub const LIG: u8 = 0x04;
	pub const W: u8 = 0x08;
	pub const WIG: u8 = 0x10;
	pub const WIG32: u8 = 0x20;
}

/// Opcode info, returned by [`Code::op_code()`] and [`Instruction::op_code()`]
///
/// [`Code::op_code()`]: enum.Code.html#method.op_code
/// [`Instruction::op_code()`]: struct.Instruction.html#method.op_code
#[derive(Debug, Clone)]
pub struct OpCodeInfo {
	op_code_string: String,
	instruction_string: String,
	enc_flags2: u32,
	enc_flags3: u32,
	opc_flags1: u32,
	opc_flags2: u32,
	code: Code,
	op_code: u16,
	encoding: EncodingKind,
	operand_size: u8,
	address_size: u8,
	l: u8,
	tuple_type: TupleType,
	table: OpCodeTableKind,
	mandatory_prefix: MandatoryPrefix,
	group_index: i8,
	rm_group_index: i8,
	op0_kind: OpCodeOperandKind,
	op1_kind: OpCodeOperandKind,
	op2_kind: OpCodeOperandKind,
	op3_kind: OpCodeOperandKind,
	op4_kind: OpCodeOperandKind,
	flags: u8,
}

impl OpCodeInfo {
	#[allow(unused_mut)]
	pub(super) fn new(code: Code, enc_flags1: u32, enc_flags2: u32, enc_flags3: u32, opc_flags1: u32, opc_flags2: u32, sb: &mut String) -> Self {
		let mut flags = Flags::NONE;
		let op_code = (enc_flags2 >> EncFlags2::OP_CODE_SHIFT) as u16;

		if (enc_flags1 & EncFlags1::IGNORES_ROUNDING_CONTROL) != 0 {
			flags |= Flags::IGNORES_ROUNDING_CONTROL;
		}
		if (enc_flags1 & EncFlags1::AMD_LOCK_REG_BIT) != 0 {
			flags |= Flags::AMD_LOCK_REG_BIT;
		}

		let op0_kind;
		let op1_kind;
		let op2_kind;
		let op3_kind;
		let op4_kind;
		let mut l;
		let mandatory_prefix;
		let table;
		let group_index;
		let rm_group_index;
		let tuple_type;
		let operand_size;
		let address_size;
		let lkind;

		let encoding = unsafe { mem::transmute(((enc_flags3 >> EncFlags3::ENCODING_SHIFT) & EncFlags3::ENCODING_MASK) as u8) };
		mandatory_prefix =
			match unsafe { mem::transmute(((enc_flags2 >> EncFlags2::MANDATORY_PREFIX_SHIFT) & EncFlags2::MANDATORY_PREFIX_MASK) as u8) } {
				MandatoryPrefixByte::None => {
					if (enc_flags2 & EncFlags2::HAS_MANDATORY_PREFIX) != 0 {
						MandatoryPrefix::PNP
					} else {
						MandatoryPrefix::None
					}
				}
				MandatoryPrefixByte::P66 => MandatoryPrefix::P66,
				MandatoryPrefixByte::PF3 => MandatoryPrefix::PF3,
				MandatoryPrefixByte::PF2 => MandatoryPrefix::PF2,
			};
		operand_size = match unsafe { mem::transmute(((enc_flags3 >> EncFlags3::OPERAND_SIZE_SHIFT) & EncFlags3::OPERAND_SIZE_MASK) as u8) } {
			CodeSize::Unknown => 0,
			CodeSize::Code16 => 16,
			CodeSize::Code32 => 32,
			CodeSize::Code64 => 64,
		};
		address_size = match unsafe { mem::transmute(((enc_flags3 >> EncFlags3::ADDRESS_SIZE_SHIFT) & EncFlags3::ADDRESS_SIZE_MASK) as u8) } {
			CodeSize::Unknown => 0,
			CodeSize::Code16 => 16,
			CodeSize::Code32 => 32,
			CodeSize::Code64 => 64,
		};
		group_index = if (enc_flags2 & EncFlags2::HAS_GROUP_INDEX) == 0 { -1 } else { ((enc_flags2 >> EncFlags2::GROUP_INDEX_SHIFT) & 7) as i8 };
		rm_group_index =
			if (enc_flags2 & EncFlags2::HAS_RM_GROUP_INDEX) == 0 { -1 } else { ((enc_flags2 >> EncFlags2::GROUP_INDEX_SHIFT) & 7) as i8 };
		tuple_type = unsafe { mem::transmute(((enc_flags3 >> EncFlags3::TUPLE_TYPE_SHIFT) & EncFlags3::TUPLE_TYPE_MASK) as u8) };

		match unsafe { mem::transmute(((enc_flags2 >> EncFlags2::LBIT_SHIFT) & EncFlags2::LBIT_MASK) as u8) } {
			LBit::LZ => {
				lkind = LKind::LZ;
				l = 0;
			}
			LBit::L0 => {
				lkind = LKind::L0;
				l = 0;
			}
			LBit::L1 => {
				lkind = LKind::L0;
				l = 1;
			}
			LBit::L128 => {
				lkind = LKind::L128;
				l = 0;
			}
			LBit::L256 => {
				lkind = LKind::L128;
				l = 1;
			}
			LBit::L512 => {
				lkind = LKind::L128;
				l = 2;
			}
			LBit::LIG => {
				lkind = LKind::None;
				l = 0;
				flags |= Flags::LIG;
			}
		}

		match unsafe { mem::transmute(((enc_flags2 >> EncFlags2::WBIT_SHIFT) & EncFlags2::WBIT_MASK) as u8) } {
			WBit::W0 => {}
			WBit::W1 => flags |= Flags::W,
			WBit::WIG => flags |= Flags::WIG,
			WBit::WIG32 => flags |= Flags::WIG32,
		}

		let mut string_format = true;
		match encoding {
			EncodingKind::Legacy => {
				op0_kind = LEGACY_OP_KINDS[((enc_flags1 >> EncFlags1::LEGACY_OP0_SHIFT) & EncFlags1::LEGACY_OP_MASK) as usize];
				op1_kind = LEGACY_OP_KINDS[((enc_flags1 >> EncFlags1::LEGACY_OP1_SHIFT) & EncFlags1::LEGACY_OP_MASK) as usize];
				op2_kind = LEGACY_OP_KINDS[((enc_flags1 >> EncFlags1::LEGACY_OP2_SHIFT) & EncFlags1::LEGACY_OP_MASK) as usize];
				op3_kind = LEGACY_OP_KINDS[((enc_flags1 >> EncFlags1::LEGACY_OP3_SHIFT) & EncFlags1::LEGACY_OP_MASK) as usize];
				op4_kind = OpCodeOperandKind::None;

				table = match unsafe { mem::transmute(((enc_flags2 >> EncFlags2::TABLE_SHIFT) & EncFlags2::TABLE_MASK) as u8) } {
					LegacyOpCodeTable::Normal => OpCodeTableKind::Normal,
					LegacyOpCodeTable::Table0F => OpCodeTableKind::T0F,
					LegacyOpCodeTable::Table0F38 => OpCodeTableKind::T0F38,
					LegacyOpCodeTable::Table0F3A => OpCodeTableKind::T0F3A,
				};
			}

			#[cfg(feature = "no_vex")]
			EncodingKind::VEX => {
				op0_kind = OpCodeOperandKind::None;
				op1_kind = OpCodeOperandKind::None;
				op2_kind = OpCodeOperandKind::None;
				op3_kind = OpCodeOperandKind::None;
				op4_kind = OpCodeOperandKind::None;
				table = OpCodeTableKind::Normal;
				string_format = false;
			}

			#[cfg(not(feature = "no_vex"))]
			EncodingKind::VEX => {
				op0_kind = VEX_OP_KINDS[((enc_flags1 >> EncFlags1::VEX_OP0_SHIFT) & EncFlags1::VEX_OP_MASK) as usize];
				op1_kind = VEX_OP_KINDS[((enc_flags1 >> EncFlags1::VEX_OP1_SHIFT) & EncFlags1::VEX_OP_MASK) as usize];
				op2_kind = VEX_OP_KINDS[((enc_flags1 >> EncFlags1::VEX_OP2_SHIFT) & EncFlags1::VEX_OP_MASK) as usize];
				op3_kind = VEX_OP_KINDS[((enc_flags1 >> EncFlags1::VEX_OP3_SHIFT) & EncFlags1::VEX_OP_MASK) as usize];
				op4_kind = VEX_OP_KINDS[((enc_flags1 >> EncFlags1::VEX_OP4_SHIFT) & EncFlags1::VEX_OP_MASK) as usize];

				table = match unsafe { mem::transmute(((enc_flags2 >> EncFlags2::TABLE_SHIFT) & EncFlags2::TABLE_MASK) as u8) } {
					VexOpCodeTable::Table0F => OpCodeTableKind::T0F,
					VexOpCodeTable::Table0F38 => OpCodeTableKind::T0F38,
					VexOpCodeTable::Table0F3A => OpCodeTableKind::T0F3A,
				};
			}

			#[cfg(feature = "no_evex")]
			EncodingKind::EVEX => {
				op0_kind = OpCodeOperandKind::None;
				op1_kind = OpCodeOperandKind::None;
				op2_kind = OpCodeOperandKind::None;
				op3_kind = OpCodeOperandKind::None;
				op4_kind = OpCodeOperandKind::None;
				table = OpCodeTableKind::Normal;
				string_format = false;
			}

			#[cfg(not(feature = "no_evex"))]
			EncodingKind::EVEX => {
				op0_kind = EVEX_OP_KINDS[((enc_flags1 >> EncFlags1::EVEX_OP0_SHIFT) & EncFlags1::EVEX_OP_MASK) as usize];
				op1_kind = EVEX_OP_KINDS[((enc_flags1 >> EncFlags1::EVEX_OP1_SHIFT) & EncFlags1::EVEX_OP_MASK) as usize];
				op2_kind = EVEX_OP_KINDS[((enc_flags1 >> EncFlags1::EVEX_OP2_SHIFT) & EncFlags1::EVEX_OP_MASK) as usize];
				op3_kind = EVEX_OP_KINDS[((enc_flags1 >> EncFlags1::EVEX_OP3_SHIFT) & EncFlags1::EVEX_OP_MASK) as usize];
				op4_kind = OpCodeOperandKind::None;

				table = match unsafe { mem::transmute(((enc_flags2 >> EncFlags2::TABLE_SHIFT) & EncFlags2::TABLE_MASK) as u8) } {
					EvexOpCodeTable::Table0F => OpCodeTableKind::T0F,
					EvexOpCodeTable::Table0F38 => OpCodeTableKind::T0F38,
					EvexOpCodeTable::Table0F3A => OpCodeTableKind::T0F3A,
				};
			}

			#[cfg(feature = "no_xop")]
			EncodingKind::XOP => {
				op0_kind = OpCodeOperandKind::None;
				op1_kind = OpCodeOperandKind::None;
				op2_kind = OpCodeOperandKind::None;
				op3_kind = OpCodeOperandKind::None;
				op4_kind = OpCodeOperandKind::None;
				table = OpCodeTableKind::Normal;
				string_format = false;
			}

			#[cfg(not(feature = "no_xop"))]
			EncodingKind::XOP => {
				op0_kind = XOP_OP_KINDS[((enc_flags1 >> EncFlags1::XOP_OP0_SHIFT) & EncFlags1::XOP_OP_MASK) as usize];
				op1_kind = XOP_OP_KINDS[((enc_flags1 >> EncFlags1::XOP_OP1_SHIFT) & EncFlags1::XOP_OP_MASK) as usize];
				op2_kind = XOP_OP_KINDS[((enc_flags1 >> EncFlags1::XOP_OP2_SHIFT) & EncFlags1::XOP_OP_MASK) as usize];
				op3_kind = XOP_OP_KINDS[((enc_flags1 >> EncFlags1::XOP_OP3_SHIFT) & EncFlags1::XOP_OP_MASK) as usize];
				op4_kind = OpCodeOperandKind::None;

				table = match unsafe { mem::transmute(((enc_flags2 >> EncFlags2::TABLE_SHIFT) & EncFlags2::TABLE_MASK) as u8) } {
					XopOpCodeTable::XOP8 => OpCodeTableKind::XOP8,
					XopOpCodeTable::XOP9 => OpCodeTableKind::XOP9,
					XopOpCodeTable::XOPA => OpCodeTableKind::XOPA,
				};
			}

			#[cfg(feature = "no_d3now")]
			EncodingKind::D3NOW => {
				op0_kind = OpCodeOperandKind::None;
				op1_kind = OpCodeOperandKind::None;
				op2_kind = OpCodeOperandKind::None;
				op3_kind = OpCodeOperandKind::None;
				op4_kind = OpCodeOperandKind::None;
				table = OpCodeTableKind::Normal;
				string_format = false;
			}

			#[cfg(not(feature = "no_d3now"))]
			EncodingKind::D3NOW => {
				op0_kind = OpCodeOperandKind::mm_reg;
				op1_kind = OpCodeOperandKind::mm_or_mem;
				op2_kind = OpCodeOperandKind::None;
				op3_kind = OpCodeOperandKind::None;
				op4_kind = OpCodeOperandKind::None;
				table = OpCodeTableKind::T0F;
			}
		}

		let mut result = Self {
			op_code_string: String::new(),
			instruction_string: String::new(),
			enc_flags2,
			enc_flags3,
			opc_flags1,
			opc_flags2,
			code,
			op_code,
			encoding,
			operand_size,
			address_size,
			l,
			tuple_type,
			table,
			mandatory_prefix,
			group_index,
			rm_group_index,
			op0_kind,
			op1_kind,
			op2_kind,
			op3_kind,
			op4_kind,
			flags,
		};

		if string_format {
			let op_code_string = OpCodeFormatter::new(&result, sb, lkind, (opc_flags1 & OpCodeInfoFlags1::MOD_REG_RM_STRING) != 0).format();
			result.op_code_string = op_code_string;
			let fmt_opt = unsafe {
				mem::transmute(((opc_flags2 >> OpCodeInfoFlags2::INSTR_STR_FMT_OPTION_SHIFT) & OpCodeInfoFlags2::INSTR_STR_FMT_OPTION_MASK) as u8)
			};
			let instruction_string = InstructionFormatter::new(&result, fmt_opt, sb).format();
			result.instruction_string = instruction_string;
		}

		result
	}

	/// Gets the code
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	///
	/// let op_code = Code::EVEX_Vmovapd_ymm_k1z_ymmm256.op_code();
	/// assert_eq!(Code::EVEX_Vmovapd_ymm_k1z_ymmm256, op_code.code());
	/// ```
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn code(&self) -> Code {
		self.code
	}

	/// Gets the encoding
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	///
	/// let op_code = Code::EVEX_Vmovapd_ymm_k1z_ymmm256.op_code();
	/// assert_eq!(EncodingKind::EVEX, op_code.encoding());
	/// ```
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn encoding(&self) -> EncodingKind {
		self.encoding
	}

	/// `true` if it's an instruction, `false` if it's eg. [`Code::INVALID`], [`db`], [`dw`], [`dd`], [`dq`]
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	///
	/// assert!(Code::EVEX_Vmovapd_ymm_k1z_ymmm256.op_code().is_instruction());
	/// assert!(!Code::INVALID.op_code().is_instruction());
	/// assert!(!Code::DeclareByte.op_code().is_instruction());
	/// ```
	///
	/// [`Code::INVALID`]: enum.Code.html#variant.INVALID
	/// [`db`]: enum.Code.html#variant.DeclareByte
	/// [`dw`]: enum.Code.html#variant.DeclareWord
	/// [`dd`]: enum.Code.html#variant.DeclareDword
	/// [`dq`]: enum.Code.html#variant.DeclareQword
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn is_instruction(&self) -> bool {
		self.code > Code::DeclareQword
	}

	/// `true` if it's an instruction available in 16-bit mode
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn mode16(&self) -> bool {
		(self.enc_flags3 & EncFlags3::BIT16OR32) != 0
	}

	/// `true` if it's an instruction available in 32-bit mode
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn mode32(&self) -> bool {
		(self.enc_flags3 & EncFlags3::BIT16OR32) != 0
	}

	/// `true` if it's an instruction available in 64-bit mode
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn mode64(&self) -> bool {
		(self.enc_flags3 & EncFlags3::BIT64) != 0
	}

	/// `true` if an `FWAIT` (`9B`) instruction is added before the instruction
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn fwait(&self) -> bool {
		(self.enc_flags3 & EncFlags3::FWAIT) != 0
	}

	/// (Legacy encoding) Gets the required operand size (16,32,64) or 0 if no operand size prefix (`66`) or `REX.W` prefix is needed
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn operand_size(&self) -> u32 {
		self.operand_size as u32
	}

	/// (Legacy encoding) Gets the required address size (16,32,64) or 0 if no address size prefix (`67`) is needed
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn address_size(&self) -> u32 {
		self.address_size as u32
	}

	/// (VEX/XOP/EVEX) `L` / `L'L` value or default value if [`is_lig()`] is `true`
	///
	/// [`is_lig()`]: #method.is_lig
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn l(&self) -> u32 {
		self.l as u32
	}

	/// (VEX/XOP/EVEX) `W` value or default value if [`is_wig()`] or [`is_wig32()`] is `true`
	///
	/// [`is_wig()`]: #method.is_wig
	/// [`is_wig32()`]: #method.is_wig32
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn w(&self) -> u32 {
		if (self.flags & Flags::W) != 0 {
			1
		} else {
			0
		}
	}

	/// (VEX/XOP/EVEX) `true` if the `L` / `L'L` fields are ignored.
	///
	/// EVEX: if reg-only ops and `{er}` (`EVEX.b` is set), `L'L` is the rounding control and not ignored.
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn is_lig(&self) -> bool {
		(self.flags & Flags::LIG) != 0
	}

	/// (VEX/XOP/EVEX) `true` if the `W` field is ignored in 16/32/64-bit modes
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn is_wig(&self) -> bool {
		(self.flags & Flags::WIG) != 0
	}

	/// (VEX/XOP/EVEX) `true` if the `W` field is ignored in 16/32-bit modes (but not 64-bit mode)
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn is_wig32(&self) -> bool {
		(self.flags & Flags::WIG32) != 0
	}

	/// (EVEX) Gets the tuple type
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn tuple_type(&self) -> TupleType {
		self.tuple_type
	}

	/// (EVEX) `true` if the instruction supports broadcasting (`EVEX.b` bit) (if it has a memory operand)
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn can_broadcast(&self) -> bool {
		(self.enc_flags3 & EncFlags3::BROADCAST) != 0
	}

	/// (EVEX) `true` if the instruction supports rounding control
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn can_use_rounding_control(&self) -> bool {
		(self.enc_flags3 & EncFlags3::ROUNDING_CONTROL) != 0
	}

	/// (EVEX) `true` if the instruction supports suppress all exceptions
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn can_suppress_all_exceptions(&self) -> bool {
		(self.enc_flags3 & EncFlags3::SUPPRESS_ALL_EXCEPTIONS) != 0
	}

	/// (EVEX) `true` if an op mask register can be used
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn can_use_op_mask_register(&self) -> bool {
		(self.enc_flags3 & EncFlags3::OP_MASK_REGISTER) != 0
	}

	/// (EVEX) `true` if a non-zero op mask register must be used
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn require_op_mask_register(&self) -> bool {
		(self.enc_flags3 & EncFlags3::REQUIRE_OP_MASK_REGISTER) != 0
	}

	/// (EVEX) `true` if a non-zero op mask register must be used
	#[deprecated(since = "1.9.0", note = "Use require_op_mask_register() instead")]
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn require_non_zero_op_mask_register(&self) -> bool {
		self.require_op_mask_register()
	}

	/// (EVEX) `true` if the instruction supports zeroing masking (if one of the op mask registers `K1`-`K7` is used and destination operand is not a memory operand)
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn can_use_zeroing_masking(&self) -> bool {
		(self.enc_flags3 & EncFlags3::ZEROING_MASKING) != 0
	}

	/// `true` if the `LOCK` (`F0`) prefix can be used
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn can_use_lock_prefix(&self) -> bool {
		(self.enc_flags3 & EncFlags3::LOCK) != 0
	}

	/// `true` if the `XACQUIRE` (`F2`) prefix can be used
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn can_use_xacquire_prefix(&self) -> bool {
		(self.enc_flags3 & EncFlags3::XACQUIRE) != 0
	}

	/// `true` if the `XRELEASE` (`F3`) prefix can be used
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn can_use_xrelease_prefix(&self) -> bool {
		(self.enc_flags3 & EncFlags3::XRELEASE) != 0
	}

	/// `true` if the `REP` / `REPE` (`F3`) prefixes can be used
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn can_use_rep_prefix(&self) -> bool {
		(self.enc_flags3 & EncFlags3::REP) != 0
	}

	/// `true` if the `REPNE` (`F2`) prefix can be used
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn can_use_repne_prefix(&self) -> bool {
		(self.enc_flags3 & EncFlags3::REPNE) != 0
	}

	/// `true` if the `BND` (`F2`) prefix can be used
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn can_use_bnd_prefix(&self) -> bool {
		(self.enc_flags3 & EncFlags3::BND) != 0
	}

	/// `true` if the `HINT-TAKEN` (`3E`) and `HINT-NOT-TAKEN` (`2E`) prefixes can be used
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn can_use_hint_taken_prefix(&self) -> bool {
		(self.enc_flags3 & EncFlags3::HINT_TAKEN) != 0
	}

	/// `true` if the `NOTRACK` (`3E`) prefix can be used
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn can_use_notrack_prefix(&self) -> bool {
		(self.enc_flags3 & EncFlags3::NOTRACK) != 0
	}

	/// Gets the opcode table
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn table(&self) -> OpCodeTableKind {
		self.table
	}

	/// Gets the mandatory prefix
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn mandatory_prefix(&self) -> MandatoryPrefix {
		self.mandatory_prefix
	}

	/// Gets the opcode. `000000xxh` if it's 1-byte, `0000yyxxh` if it's 2-byte (`yy` != `00`, and `yy` is the first byte and `xx` the second byte).
	/// It doesn't include the table value, see [`table()`].
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	///
	/// assert_eq!(0xDFC0, Code::Ffreep_sti.op_code().op_code());
	/// assert_eq!(0x01D8, Code::Vmrunw.op_code().op_code());
	/// assert_eq!(0x2A, Code::Sub_r8_rm8.op_code().op_code());
	/// assert_eq!(0x2A, Code::Cvtpi2ps_xmm_mmm64.op_code().op_code());
	/// ```
	///
	/// [`table()`]: #method.table
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn op_code(&self) -> u32 {
		self.op_code as u32
	}

	/// `true` if it's part of a group
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn is_group(&self) -> bool {
		self.group_index >= 0
	}

	/// Group index (0-7) or -1. If it's 0-7, it's stored in the `reg` field of the `modrm` byte.
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn group_index(&self) -> i32 {
		self.group_index as i32
	}

	/// `true` if it's part of a modrm.rm group
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn is_rm_group(&self) -> bool {
		self.rm_group_index >= 0
	}

	/// Group index (0-7) or -1. If it's 0-7, it's stored in the `reg` field of the `modrm` byte.
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn rm_group_index(&self) -> i32 {
		self.rm_group_index as i32
	}

	/// Gets the number of operands
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn op_count(&self) -> u32 {
		unsafe { *instruction_op_counts::OP_COUNT.get_unchecked(self.code as usize) as u32 }
	}

	/// Gets operand #0's opkind
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn op0_kind(&self) -> OpCodeOperandKind {
		self.op0_kind
	}

	/// Gets operand #1's opkind
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn op1_kind(&self) -> OpCodeOperandKind {
		self.op1_kind
	}

	/// Gets operand #2's opkind
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn op2_kind(&self) -> OpCodeOperandKind {
		self.op2_kind
	}

	/// Gets operand #3's opkind
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn op3_kind(&self) -> OpCodeOperandKind {
		self.op3_kind
	}

	/// Gets operand #4's opkind
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn op4_kind(&self) -> OpCodeOperandKind {
		self.op4_kind
	}

	/// Gets an operand's opkind
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
	pub fn op_kind(&self, operand: u32) -> OpCodeOperandKind {
		match operand {
			0 => self.op0_kind(),
			1 => self.op1_kind(),
			2 => self.op2_kind(),
			3 => self.op3_kind(),
			4 => self.op4_kind(),
			_ => panic!(),
		}
	}

	/// Checks if the instruction is available in 16-bit mode, 32-bit mode or 64-bit mode
	///
	/// # Panics
	///
	/// Panics if `bitness` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `bitness`: 16, 32 or 64
	#[cfg_attr(has_must_use, must_use)]
	#[cfg_attr(feature = "cargo-clippy", allow(clippy::missing_inline_in_public_items))]
	pub fn is_available_in_mode(&self, bitness: u32) -> bool {
		match bitness {
			16 => self.mode16(),
			32 => self.mode32(),
			64 => self.mode64(),
			_ => panic!(),
		}
	}

	/// Gets the opcode string, eg. `VEX.128.66.0F38.W0 78 /r`, see also [`instruction_string()`]
	///
	/// [`instruction_string()`]: #method.instruction_string
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	///
	/// let op_code = Code::EVEX_Vmovapd_ymm_k1z_ymmm256.op_code();
	/// assert_eq!("EVEX.256.66.0F.W1 28 /r", op_code.op_code_string());
	/// ```
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn op_code_string(&self) -> &str {
		self.op_code_string.as_str()
	}

	/// Gets the instruction string, eg. `VPBROADCASTB xmm1, xmm2/m8`, see also [`op_code_string()`]
	///
	/// [`op_code_string()`]: #method.op_code_string
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	///
	/// let op_code = Code::EVEX_Vmovapd_ymm_k1z_ymmm256.op_code();
	/// assert_eq!("VMOVAPD ymm1 {k1}{z}, ymm2/m256", op_code.instruction_string());
	/// ```
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn instruction_string(&self) -> &str {
		self.instruction_string.as_str()
	}
}

impl fmt::Display for OpCodeInfo {
	#[inline]
	fn fmt<'a>(&self, f: &mut fmt::Formatter<'a>) -> fmt::Result {
		write!(f, "{}", self.instruction_string)
	}
}
