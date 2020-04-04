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

use super::code::{iced_to_code, Code};
use super::encoding_kind::{iced_to_encoding_kind, EncodingKind};
use super::mandatory_prefix::{iced_to_mandatory_prefix, MandatoryPrefix};
use super::op_code_operand_kind::{iced_to_op_code_operand_kind, OpCodeOperandKind};
use super::op_code_table_kind::{iced_to_op_code_table_kind, OpCodeTableKind};
use super::tuple_type::{iced_to_tuple_type, TupleType};
use wasm_bindgen::prelude::*;

/// Opcode info, returned by [`Instruction.opCode`]
///
/// [`Instruction.opCode`]: struct.Instruction.html#method.op_code
#[wasm_bindgen]
pub struct OpCodeInfo(pub(crate) &'static iced_x86::OpCodeInfo);

#[wasm_bindgen]
impl OpCodeInfo {
	/// Gets the code (a [`Code`] enum value)
	///
	/// [`Code`]: enum.Code.html
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	///
	/// let op_code = Code::EVEX_Vmovapd_ymm_k1z_ymmm256.op_code();
	/// assert_eq!(Code::EVEX_Vmovapd_ymm_k1z_ymmm256, op_code.code());
	/// ```
	#[wasm_bindgen(getter)]
	pub fn code(&self) -> Code {
		iced_to_code(self.0.code())
	}

	/// Gets the encoding (a [`EncodingKind`] enum value)
	///
	/// [`EncodingKind`]: enum.EncodingKind.html
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	///
	/// let op_code = Code::EVEX_Vmovapd_ymm_k1z_ymmm256.op_code();
	/// assert_eq!(EncodingKind::EVEX, op_code.encoding());
	/// ```
	#[wasm_bindgen(getter)]
	pub fn encoding(&self) -> EncodingKind {
		iced_to_encoding_kind(self.0.encoding())
	}

	/// `true` if it's an instruction, `false` if it's eg. [`Code.INVALID`], [`db`], [`dw`], [`dd`], [`dq`]
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
	/// [`Code.INVALID`]: enum.Code.html#variant.INVALID
	/// [`db`]: enum.Code.html#variant.DeclareByte
	/// [`dw`]: enum.Code.html#variant.DeclareWord
	/// [`dd`]: enum.Code.html#variant.DeclareDword
	/// [`dq`]: enum.Code.html#variant.DeclareQword
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "isInstruction")]
	pub fn is_instruction(&self) -> bool {
		self.0.is_instruction()
	}

	/// `true` if it's an instruction available in 16-bit mode
	#[wasm_bindgen(getter)]
	pub fn mode16(&self) -> bool {
		self.0.mode16()
	}

	/// `true` if it's an instruction available in 32-bit mode
	#[wasm_bindgen(getter)]
	pub fn mode32(&self) -> bool {
		self.0.mode32()
	}

	/// `true` if it's an instruction available in 64-bit mode
	#[wasm_bindgen(getter)]
	pub fn mode64(&self) -> bool {
		self.0.mode64()
	}

	/// `true` if an `FWAIT` (`9B`) instruction is added before the instruction
	#[wasm_bindgen(getter)]
	pub fn fwait(&self) -> bool {
		self.0.fwait()
	}

	/// (Legacy encoding) Gets the required operand size (16,32,64) or 0 if no operand size prefix (`66`) or `REX.W` prefix is needed
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "operandSize")]
	pub fn operand_size(&self) -> u32 {
		self.0.operand_size()
	}

	/// (Legacy encoding) Gets the required address size (16,32,64) or 0 if no address size prefix (`67`) is needed
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "addressSize")]
	pub fn address_size(&self) -> u32 {
		self.0.address_size()
	}

	/// (VEX/XOP/EVEX) `L` / `L'L` value or default value if [`isLIG`] is `true`
	///
	/// [`isLIG`]: #method.is_lig
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "L")]
	pub fn l(&self) -> u32 {
		self.0.l()
	}

	/// (VEX/XOP/EVEX) `W` value or default value if [`isWIG`] or [`isWIG32`] is `true`
	///
	/// [`isWIG`]: #method.is_wig
	/// [`isWIG32`]: #method.is_wig32
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "W")]
	pub fn w(&self) -> u32 {
		self.0.w()
	}

	/// (VEX/XOP/EVEX) `true` if the `L` / `L'L` fields are ignored.
	///
	/// EVEX: if reg-only ops and `{er}` (`EVEX.b` is set), `L'L` is the rounding control and not ignored.
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "isLIG")]
	pub fn is_lig(&self) -> bool {
		self.0.is_lig()
	}

	/// (VEX/XOP/EVEX) `true` if the `W` field is ignored in 16/32/64-bit modes
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "isWIG")]
	pub fn is_wig(&self) -> bool {
		self.0.is_wig()
	}

	/// (VEX/XOP/EVEX) `true` if the `W` field is ignored in 16/32-bit modes (but not 64-bit mode)
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "isWIG32")]
	pub fn is_wig32(&self) -> bool {
		self.0.is_wig32()
	}

	/// (EVEX) Gets the tuple type (a [`TupleType`] enum value)
	///
	/// [`TupleType`]: enum.TupleType.html
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "tupleType")]
	pub fn tuple_type(&self) -> TupleType {
		iced_to_tuple_type(self.0.tuple_type())
	}

	/// (EVEX) `true` if the instruction supports broadcasting (`EVEX.b` bit) (if it has a memory operand)
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "canBroadcast")]
	pub fn can_broadcast(&self) -> bool {
		self.0.can_broadcast()
	}

	/// (EVEX) `true` if the instruction supports rounding control
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "canUseRoundingControl")]
	pub fn can_use_rounding_control(&self) -> bool {
		self.0.can_use_rounding_control()
	}

	/// (EVEX) `true` if the instruction supports suppress all exceptions
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "canSuppressAllExceptions")]
	pub fn can_suppress_all_exceptions(&self) -> bool {
		self.0.can_suppress_all_exceptions()
	}

	/// (EVEX) `true` if an op mask register can be used
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "canUseOpMaskRegister")]
	pub fn can_use_op_mask_register(&self) -> bool {
		self.0.can_use_op_mask_register()
	}

	/// (EVEX) `true` if a non-zero op mask register must be used
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "requireNonZeroOpMaskRegister")]
	pub fn require_non_zero_op_mask_register(&self) -> bool {
		self.0.require_non_zero_op_mask_register()
	}

	/// (EVEX) `true` if the instruction supports zeroing masking (if one of the op mask registers `K1`-`K7` is used and destination operand is not a memory operand)
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "canUseZeroingMasking")]
	pub fn can_use_zeroing_masking(&self) -> bool {
		self.0.can_use_zeroing_masking()
	}

	/// `true` if the `LOCK` (`F0`) prefix can be used
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "canUseLockPrefix")]
	pub fn can_use_lock_prefix(&self) -> bool {
		self.0.can_use_lock_prefix()
	}

	/// `true` if the `XACQUIRE` (`F2`) prefix can be used
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "canUseXacquirePrefix")]
	pub fn can_use_xacquire_prefix(&self) -> bool {
		self.0.can_use_xacquire_prefix()
	}

	/// `true` if the `XRELEASE` (`F3`) prefix can be used
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "canUseXreleasePrefix")]
	pub fn can_use_xrelease_prefix(&self) -> bool {
		self.0.can_use_xrelease_prefix()
	}

	/// `true` if the `REP` / `REPE` (`F3`) prefixes can be used
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "canUseRepPrefix")]
	pub fn can_use_rep_prefix(&self) -> bool {
		self.0.can_use_rep_prefix()
	}

	/// `true` if the `REPNE` (`F2`) prefix can be used
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "canUseRepnePrefix")]
	pub fn can_use_repne_prefix(&self) -> bool {
		self.0.can_use_repne_prefix()
	}

	/// `true` if the `BND` (`F2`) prefix can be used
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "canUseBndPrefix")]
	pub fn can_use_bnd_prefix(&self) -> bool {
		self.0.can_use_bnd_prefix()
	}

	/// `true` if the `HINT-TAKEN` (`3E`) and `HINT-NOT-TAKEN` (`2E`) prefixes can be used
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "canUseHintTakenPrefix")]
	pub fn can_use_hint_taken_prefix(&self) -> bool {
		self.0.can_use_hint_taken_prefix()
	}

	/// `true` if the `NOTRACK` (`3E`) prefix can be used
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "canUseNotrackPrefix")]
	pub fn can_use_notrack_prefix(&self) -> bool {
		self.0.can_use_notrack_prefix()
	}

	/// Gets the opcode table (a [`OpCodeTableKind`] enum value)
	///
	/// [`OpCodeTableKind`]: enum.OpCodeTableKind.html
	#[wasm_bindgen(getter)]
	pub fn table(&self) -> OpCodeTableKind {
		iced_to_op_code_table_kind(self.0.table())
	}

	/// Gets the mandatory prefix (a [`MandatoryPrefix`] enum value)
	///
	/// [`MandatoryPrefix`]: enum.MandatoryPrefix.html
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "mandatoryPrefix")]
	pub fn mandatory_prefix(&self) -> MandatoryPrefix {
		iced_to_mandatory_prefix(self.0.mandatory_prefix())
	}

	/// Gets the opcode. `000000xxh` if it's 1-byte, `0000yyxxh` if it's 2-byte (`yy` != `00`, and `yy` is the first byte and `xx` the second byte).
	/// It doesn't include the table value, see [`table`].
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
	/// [`table`]: #method.table
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "opCode")]
	pub fn op_code(&self) -> u32 {
		self.0.op_code()
	}

	/// `true` if it's part of a group
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "isGroup")]
	pub fn is_group(&self) -> bool {
		self.0.is_group()
	}

	/// Group index (0-7) or -1. If it's 0-7, it's stored in the `reg` field of the `modrm` byte.
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "groupIndex")]
	pub fn group_index(&self) -> i32 {
		self.0.group_index()
	}

	/// Gets the number of operands
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "opCount")]
	pub fn op_count(&self) -> u32 {
		self.0.op_count()
	}

	/// Gets operand #0's opkind (a [`OpCodeOperandKind`] enum value)
	///
	/// [`OpCodeOperandKind`]: enum.OpCodeOperandKind.html
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "op0Kind")]
	pub fn op0_kind(&self) -> OpCodeOperandKind {
		iced_to_op_code_operand_kind(self.0.op0_kind())
	}

	/// Gets operand #1's opkind (a [`OpCodeOperandKind`] enum value)
	///
	/// [`OpCodeOperandKind`]: enum.OpCodeOperandKind.html
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "op1Kind")]
	pub fn op1_kind(&self) -> OpCodeOperandKind {
		iced_to_op_code_operand_kind(self.0.op1_kind())
	}

	/// Gets operand #2's opkind (a [`OpCodeOperandKind`] enum value)
	///
	/// [`OpCodeOperandKind`]: enum.OpCodeOperandKind.html
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "op2Kind")]
	pub fn op2_kind(&self) -> OpCodeOperandKind {
		iced_to_op_code_operand_kind(self.0.op2_kind())
	}

	/// Gets operand #3's opkind (a [`OpCodeOperandKind`] enum value)
	///
	/// [`OpCodeOperandKind`]: enum.OpCodeOperandKind.html
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "op3Kind")]
	pub fn op3_kind(&self) -> OpCodeOperandKind {
		iced_to_op_code_operand_kind(self.0.op3_kind())
	}

	/// Gets operand #4's opkind (a [`OpCodeOperandKind`] enum value)
	///
	/// [`OpCodeOperandKind`]: enum.OpCodeOperandKind.html
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "op4Kind")]
	pub fn op4_kind(&self) -> OpCodeOperandKind {
		iced_to_op_code_operand_kind(self.0.op4_kind())
	}

	/// Gets an operand's opkind (a [`OpCodeOperandKind`] enum value)
	///
	/// [`OpCodeOperandKind`]: enum.OpCodeOperandKind.html
	///
	/// # Panics
	///
	/// Panics if `operand` is invalid
	///
	/// # Arguments
	///
	/// * `operand`: Operand number, 0-4
	#[wasm_bindgen(js_name = "opKind")]
	pub fn op_kind(&self, operand: u32) -> OpCodeOperandKind {
		iced_to_op_code_operand_kind(self.0.op_kind(operand))
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
	#[wasm_bindgen(js_name = "isAvailableInMode")]
	pub fn is_available_in_mode(&self, bitness: u32) -> bool {
		self.0.is_available_in_mode(bitness)
	}

	/// Gets the opcode string, eg. `VEX.128.66.0F38.W0 78 /r`, see also [`instructionString`]
	///
	/// [`instructionString`]: #method.instruction_string
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	///
	/// let op_code = Code::EVEX_Vmovapd_ymm_k1z_ymmm256.op_code();
	/// assert_eq!("EVEX.256.66.0F.W1 28 /r", op_code.op_code_string());
	/// ```
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "opCodeString")]
	pub fn op_code_string(&self) -> String {
		self.0.op_code_string().to_owned()
	}

	/// Gets the instruction string, eg. `VPBROADCASTB xmm1, xmm2/m8`, see also [`opCodeString`]
	///
	/// [`opCodeString`]: #method.op_code_string
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	///
	/// let op_code = Code::EVEX_Vmovapd_ymm_k1z_ymmm256.op_code();
	/// assert_eq!("VMOVAPD ymm1 {k1}{z}, ymm2/m256", op_code.instruction_string());
	/// ```
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "instructionString")]
	pub fn instruction_string(&self) -> String {
		self.0.instruction_string().to_owned()
	}
}
