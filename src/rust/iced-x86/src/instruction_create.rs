// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

mod private {
	use crate::{Code, IcedError, Instruction};

	pub trait With1<T> {
		fn with1(code: Code, op0: T) -> Result<Instruction, IcedError>;
	}

	pub trait With2<T, U> {
		fn with2(code: Code, op0: T, op1: U) -> Result<Instruction, IcedError>;
	}

	pub trait With3<T, U, V> {
		fn with3(code: Code, op0: T, op1: U, op2: V) -> Result<Instruction, IcedError>;
	}

	pub trait With4<T, U, V, W> {
		fn with4(code: Code, op0: T, op1: U, op2: V, op3: W) -> Result<Instruction, IcedError>;
	}

	pub trait With5<T, U, V, W, X> {
		fn with5(code: Code, op0: T, op1: U, op2: V, op3: W, op4: X) -> Result<Instruction, IcedError>;
	}
}

use self::private::{With1, With2, With3, With4, With5};
use crate::instruction_internal;
use crate::{Code, IcedError, Instruction, MemoryOperand, OpKind, Register, RepPrefixKind};

impl Instruction {
	fn init_memory_operand(instruction: &mut Instruction, memory: &MemoryOperand) {
		instruction.set_memory_base(memory.base);
		instruction.set_memory_index(memory.index);
		instruction.set_memory_index_scale(memory.scale);
		instruction.set_memory_displ_size(memory.displ_size);
		instruction.set_memory_displacement64(memory.displacement as u64);
		instruction.set_is_broadcast(memory.is_broadcast);
		instruction.set_segment_prefix(memory.segment_prefix);
	}

	/// Creates an instruction with no operands
	///
	/// # Arguments
	///
	/// * `code`: Code value
	#[must_use]
	#[inline]
	#[rustfmt::skip]
	pub fn with(code: Code) -> Self {
		let mut instruction = Self::default();
		instruction.set_code(code);

		debug_assert_eq!(instruction.op_count(), 0);
		instruction
	}

	/// Creates an instruction with 1 operand
	///
	/// # Errors
	///
	/// Fails if one of the operands is invalid (basic checks)
	///
	/// # Arguments
	///
	/// * `code`: Code value
	/// * `op0`: First operand (eg. a [`Register`], an integer (a `u32`/`i64`/`u64` number suffix is sometimes needed), or a [`MemoryOperand`])
	///
	/// [`Register`]: enum.Register.html
	/// [`MemoryOperand`]: struct.MemoryOperand.html
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	///
	/// # fn main() -> Result<(), IcedError> {
	/// let _ = Instruction::with1(Code::Pop_rm64, Register::RCX)?;
	/// let _ = Instruction::with1(Code::Pop_rm64, MemoryOperand::new(Register::RBP, Register::RSI, 2, -0x5432_10FF, 8, false, Register::FS))?;
	/// # Ok(())
	/// # }
	/// ```
	#[inline]
	pub fn with1<T>(code: Code, op0: T) -> Result<Instruction, IcedError>
	where
		Self: With1<T>,
	{
		<Self as With1<T>>::with1(code, op0)
	}

	/// Creates an instruction with 2 operands
	///
	/// # Errors
	///
	/// Fails if one of the operands is invalid (basic checks)
	///
	/// # Arguments
	///
	/// * `code`: Code value
	/// * `op0`: First operand (eg. a [`Register`], an integer (a `u32`/`i64`/`u64` number suffix is sometimes needed), or a [`MemoryOperand`])
	/// * `op1`: Second operand
	///
	/// [`Register`]: enum.Register.html
	/// [`MemoryOperand`]: struct.MemoryOperand.html
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	///
	/// # fn main() -> Result<(), IcedError> {
	/// let _ = Instruction::with2(Code::Add_rm8_r8, Register::CL, Register::DL)?;
	/// let _ = Instruction::with2(Code::Add_r8_rm8, Register::CL, MemoryOperand::new(Register::RBP, Register::RSI, 2, -0x5432_10FF, 8, false, Register::FS))?;
	/// # Ok(())
	/// # }
	/// ```
	#[inline]
	pub fn with2<T, U>(code: Code, op0: T, op1: U) -> Result<Instruction, IcedError>
	where
		Self: With2<T, U>,
	{
		<Self as With2<T, U>>::with2(code, op0, op1)
	}

	/// Creates an instruction with 3 operands
	///
	/// # Errors
	///
	/// Fails if one of the operands is invalid (basic checks)
	///
	/// # Arguments
	///
	/// * `code`: Code value
	/// * `op0`: First operand (eg. a [`Register`], an integer (a `u32`/`i64`/`u64` number suffix is sometimes needed), or a [`MemoryOperand`])
	/// * `op1`: Second operand
	/// * `op2`: Third operand
	///
	/// [`Register`]: enum.Register.html
	/// [`MemoryOperand`]: struct.MemoryOperand.html
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	///
	/// # fn main() -> Result<(), IcedError> {
	/// let _ = Instruction::with3(Code::Imul_r16_rm16_imm16, Register::CX, Register::DX, 0x5AA5)?;
	/// let _ = Instruction::with3(Code::Imul_r16_rm16_imm16, Register::CX, MemoryOperand::new(Register::RBP, Register::RSI, 2, -0x5432_10FF, 8, false, Register::FS), 0xA55A)?;
	/// # Ok(())
	/// # }
	/// ```
	#[inline]
	pub fn with3<T, U, V>(code: Code, op0: T, op1: U, op2: V) -> Result<Instruction, IcedError>
	where
		Self: With3<T, U, V>,
	{
		<Self as With3<T, U, V>>::with3(code, op0, op1, op2)
	}

	/// Creates an instruction with 4 operands
	///
	/// # Errors
	///
	/// Fails if one of the operands is invalid (basic checks)
	///
	/// # Arguments
	///
	/// * `code`: Code value
	/// * `op0`: First operand (eg. a [`Register`], an integer (a `u32`/`i64`/`u64` number suffix is sometimes needed), or a [`MemoryOperand`])
	/// * `op1`: Second operand
	/// * `op2`: Third operand
	/// * `op3`: Fourth operand
	///
	/// [`Register`]: enum.Register.html
	/// [`MemoryOperand`]: struct.MemoryOperand.html
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	///
	/// # fn main() -> Result<(), IcedError> {
	/// let _ = Instruction::with4(Code::Insertq_xmm_xmm_imm8_imm8, Register::XMM1, Register::XMM2, 0xA5, 0xFD)?;
	/// let _ = Instruction::with4(Code::VEX_Vfmaddsubps_xmm_xmm_xmm_xmmm128, Register::XMM1, Register::XMM2, Register::XMM3, MemoryOperand::new(Register::RBP, Register::RSI, 2, -0x5432_10FF, 8, false, Register::FS))?;
	/// # Ok(())
	/// # }
	/// ```
	#[inline]
	pub fn with4<T, U, V, W>(code: Code, op0: T, op1: U, op2: V, op3: W) -> Result<Instruction, IcedError>
	where
		Self: With4<T, U, V, W>,
	{
		<Self as With4<T, U, V, W>>::with4(code, op0, op1, op2, op3)
	}

	/// Creates an instruction with 5 operands
	///
	/// # Errors
	///
	/// Fails if one of the operands is invalid (basic checks)
	///
	/// # Arguments
	///
	/// * `code`: Code value
	/// * `op0`: First operand (eg. a [`Register`], an integer (a `u32`/`i64`/`u64` number suffix is sometimes needed), or a [`MemoryOperand`])
	/// * `op1`: Second operand
	/// * `op2`: Third operand
	/// * `op3`: Fourth operand
	/// * `op4`: Fifth operand
	///
	/// [`Register`]: enum.Register.html
	/// [`MemoryOperand`]: struct.MemoryOperand.html
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	///
	/// # fn main() -> Result<(), IcedError> {
	/// let _ = Instruction::with5(Code::VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm4, Register::XMM1, Register::XMM2, Register::XMM3, Register::XMM4, 0x0)?;
	/// let _ = Instruction::with5(Code::VEX_Vpermil2ps_xmm_xmm_xmm_xmmm128_imm4, Register::XMM1, Register::XMM2, Register::XMM3, MemoryOperand::new(Register::RBP, Register::RSI, 2, -0x5432_10FF, 8, false, Register::FS), 0x1)?;
	/// # Ok(())
	/// # }
	/// ```
	#[inline]
	pub fn with5<T, U, V, W, X>(code: Code, op0: T, op1: U, op2: V, op3: W, op4: X) -> Result<Instruction, IcedError>
	where
		Self: With5<T, U, V, W, X>,
	{
		<Self as With5<T, U, V, W, X>>::with5(code, op0, op1, op2, op3, op4)
	}
}

// GENERATOR-BEGIN: Create
// ⚠️This was generated by GENERATOR!🦹‍♂️

impl With1<Register> for Instruction {
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	fn with1(code: Code, register: Register) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(code);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction.set_op0_register(register);

		debug_assert_eq!(instruction.op_count(), 1);
		Ok(instruction)
	}
}

impl With1<i32> for Instruction {
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	fn with1(code: Code, immediate: i32) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(code);

		instruction_internal::initialize_signed_immediate(&mut instruction, 0, immediate as i64)?;

		debug_assert_eq!(instruction.op_count(), 1);
		Ok(instruction)
	}
}

impl With1<u32> for Instruction {
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	fn with1(code: Code, immediate: u32) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(code);

		instruction_internal::initialize_unsigned_immediate(&mut instruction, 0, immediate as u64)?;

		debug_assert_eq!(instruction.op_count(), 1);
		Ok(instruction)
	}
}

impl With1<MemoryOperand> for Instruction {
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	fn with1(code: Code, memory: MemoryOperand) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(code);

		instruction.set_op0_kind(OpKind::Memory);
		Instruction::init_memory_operand(&mut instruction, &memory);

		debug_assert_eq!(instruction.op_count(), 1);
		Ok(instruction)
	}
}

impl With2<Register, Register> for Instruction {
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	fn with2(code: Code, register1: Register, register2: Register) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(code);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction.set_op0_register(register1);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction.set_op1_register(register2);

		debug_assert_eq!(instruction.op_count(), 2);
		Ok(instruction)
	}
}

impl With2<Register, i32> for Instruction {
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	fn with2(code: Code, register: Register, immediate: i32) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(code);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction.set_op0_register(register);

		instruction_internal::initialize_signed_immediate(&mut instruction, 1, immediate as i64)?;

		debug_assert_eq!(instruction.op_count(), 2);
		Ok(instruction)
	}
}

impl With2<Register, u32> for Instruction {
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	fn with2(code: Code, register: Register, immediate: u32) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(code);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction.set_op0_register(register);

		instruction_internal::initialize_unsigned_immediate(&mut instruction, 1, immediate as u64)?;

		debug_assert_eq!(instruction.op_count(), 2);
		Ok(instruction)
	}
}

impl With2<Register, i64> for Instruction {
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	fn with2(code: Code, register: Register, immediate: i64) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(code);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction.set_op0_register(register);

		instruction_internal::initialize_signed_immediate(&mut instruction, 1, immediate)?;

		debug_assert_eq!(instruction.op_count(), 2);
		Ok(instruction)
	}
}

impl With2<Register, u64> for Instruction {
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	fn with2(code: Code, register: Register, immediate: u64) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(code);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction.set_op0_register(register);

		instruction_internal::initialize_unsigned_immediate(&mut instruction, 1, immediate)?;

		debug_assert_eq!(instruction.op_count(), 2);
		Ok(instruction)
	}
}

impl With2<Register, MemoryOperand> for Instruction {
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	fn with2(code: Code, register: Register, memory: MemoryOperand) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(code);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction.set_op0_register(register);

		instruction.set_op1_kind(OpKind::Memory);
		Instruction::init_memory_operand(&mut instruction, &memory);

		debug_assert_eq!(instruction.op_count(), 2);
		Ok(instruction)
	}
}

impl With2<i32, Register> for Instruction {
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	fn with2(code: Code, immediate: i32, register: Register) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(code);

		instruction_internal::initialize_signed_immediate(&mut instruction, 0, immediate as i64)?;

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction.set_op1_register(register);

		debug_assert_eq!(instruction.op_count(), 2);
		Ok(instruction)
	}
}

impl With2<u32, Register> for Instruction {
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	fn with2(code: Code, immediate: u32, register: Register) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(code);

		instruction_internal::initialize_unsigned_immediate(&mut instruction, 0, immediate as u64)?;

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction.set_op1_register(register);

		debug_assert_eq!(instruction.op_count(), 2);
		Ok(instruction)
	}
}

impl With2<i32, i32> for Instruction {
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	fn with2(code: Code, immediate1: i32, immediate2: i32) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(code);

		instruction_internal::initialize_signed_immediate(&mut instruction, 0, immediate1 as i64)?;

		instruction_internal::initialize_signed_immediate(&mut instruction, 1, immediate2 as i64)?;

		debug_assert_eq!(instruction.op_count(), 2);
		Ok(instruction)
	}
}

impl With2<u32, u32> for Instruction {
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	fn with2(code: Code, immediate1: u32, immediate2: u32) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(code);

		instruction_internal::initialize_unsigned_immediate(&mut instruction, 0, immediate1 as u64)?;

		instruction_internal::initialize_unsigned_immediate(&mut instruction, 1, immediate2 as u64)?;

		debug_assert_eq!(instruction.op_count(), 2);
		Ok(instruction)
	}
}

impl With2<MemoryOperand, Register> for Instruction {
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	fn with2(code: Code, memory: MemoryOperand, register: Register) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(code);

		instruction.set_op0_kind(OpKind::Memory);
		Instruction::init_memory_operand(&mut instruction, &memory);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction.set_op1_register(register);

		debug_assert_eq!(instruction.op_count(), 2);
		Ok(instruction)
	}
}

impl With2<MemoryOperand, i32> for Instruction {
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	fn with2(code: Code, memory: MemoryOperand, immediate: i32) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(code);

		instruction.set_op0_kind(OpKind::Memory);
		Instruction::init_memory_operand(&mut instruction, &memory);

		instruction_internal::initialize_signed_immediate(&mut instruction, 1, immediate as i64)?;

		debug_assert_eq!(instruction.op_count(), 2);
		Ok(instruction)
	}
}

impl With2<MemoryOperand, u32> for Instruction {
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	fn with2(code: Code, memory: MemoryOperand, immediate: u32) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(code);

		instruction.set_op0_kind(OpKind::Memory);
		Instruction::init_memory_operand(&mut instruction, &memory);

		instruction_internal::initialize_unsigned_immediate(&mut instruction, 1, immediate as u64)?;

		debug_assert_eq!(instruction.op_count(), 2);
		Ok(instruction)
	}
}

impl With3<Register, Register, Register> for Instruction {
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	fn with3(code: Code, register1: Register, register2: Register, register3: Register) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(code);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction.set_op0_register(register1);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction.set_op1_register(register2);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op2_kind(OpKind::Register);
		instruction.set_op2_register(register3);

		debug_assert_eq!(instruction.op_count(), 3);
		Ok(instruction)
	}
}

impl With3<Register, Register, i32> for Instruction {
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	fn with3(code: Code, register1: Register, register2: Register, immediate: i32) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(code);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction.set_op0_register(register1);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction.set_op1_register(register2);

		instruction_internal::initialize_signed_immediate(&mut instruction, 2, immediate as i64)?;

		debug_assert_eq!(instruction.op_count(), 3);
		Ok(instruction)
	}
}

impl With3<Register, Register, u32> for Instruction {
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	fn with3(code: Code, register1: Register, register2: Register, immediate: u32) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(code);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction.set_op0_register(register1);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction.set_op1_register(register2);

		instruction_internal::initialize_unsigned_immediate(&mut instruction, 2, immediate as u64)?;

		debug_assert_eq!(instruction.op_count(), 3);
		Ok(instruction)
	}
}

impl With3<Register, Register, MemoryOperand> for Instruction {
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	fn with3(code: Code, register1: Register, register2: Register, memory: MemoryOperand) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(code);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction.set_op0_register(register1);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction.set_op1_register(register2);

		instruction.set_op2_kind(OpKind::Memory);
		Instruction::init_memory_operand(&mut instruction, &memory);

		debug_assert_eq!(instruction.op_count(), 3);
		Ok(instruction)
	}
}

impl With3<Register, i32, i32> for Instruction {
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	fn with3(code: Code, register: Register, immediate1: i32, immediate2: i32) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(code);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction.set_op0_register(register);

		instruction_internal::initialize_signed_immediate(&mut instruction, 1, immediate1 as i64)?;

		instruction_internal::initialize_signed_immediate(&mut instruction, 2, immediate2 as i64)?;

		debug_assert_eq!(instruction.op_count(), 3);
		Ok(instruction)
	}
}

impl With3<Register, u32, u32> for Instruction {
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	fn with3(code: Code, register: Register, immediate1: u32, immediate2: u32) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(code);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction.set_op0_register(register);

		instruction_internal::initialize_unsigned_immediate(&mut instruction, 1, immediate1 as u64)?;

		instruction_internal::initialize_unsigned_immediate(&mut instruction, 2, immediate2 as u64)?;

		debug_assert_eq!(instruction.op_count(), 3);
		Ok(instruction)
	}
}

impl With3<Register, MemoryOperand, Register> for Instruction {
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	fn with3(code: Code, register1: Register, memory: MemoryOperand, register2: Register) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(code);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction.set_op0_register(register1);

		instruction.set_op1_kind(OpKind::Memory);
		Instruction::init_memory_operand(&mut instruction, &memory);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op2_kind(OpKind::Register);
		instruction.set_op2_register(register2);

		debug_assert_eq!(instruction.op_count(), 3);
		Ok(instruction)
	}
}

impl With3<Register, MemoryOperand, i32> for Instruction {
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	fn with3(code: Code, register: Register, memory: MemoryOperand, immediate: i32) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(code);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction.set_op0_register(register);

		instruction.set_op1_kind(OpKind::Memory);
		Instruction::init_memory_operand(&mut instruction, &memory);

		instruction_internal::initialize_signed_immediate(&mut instruction, 2, immediate as i64)?;

		debug_assert_eq!(instruction.op_count(), 3);
		Ok(instruction)
	}
}

impl With3<Register, MemoryOperand, u32> for Instruction {
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	fn with3(code: Code, register: Register, memory: MemoryOperand, immediate: u32) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(code);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction.set_op0_register(register);

		instruction.set_op1_kind(OpKind::Memory);
		Instruction::init_memory_operand(&mut instruction, &memory);

		instruction_internal::initialize_unsigned_immediate(&mut instruction, 2, immediate as u64)?;

		debug_assert_eq!(instruction.op_count(), 3);
		Ok(instruction)
	}
}

impl With3<MemoryOperand, Register, Register> for Instruction {
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	fn with3(code: Code, memory: MemoryOperand, register1: Register, register2: Register) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(code);

		instruction.set_op0_kind(OpKind::Memory);
		Instruction::init_memory_operand(&mut instruction, &memory);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction.set_op1_register(register1);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op2_kind(OpKind::Register);
		instruction.set_op2_register(register2);

		debug_assert_eq!(instruction.op_count(), 3);
		Ok(instruction)
	}
}

impl With3<MemoryOperand, Register, i32> for Instruction {
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	fn with3(code: Code, memory: MemoryOperand, register: Register, immediate: i32) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(code);

		instruction.set_op0_kind(OpKind::Memory);
		Instruction::init_memory_operand(&mut instruction, &memory);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction.set_op1_register(register);

		instruction_internal::initialize_signed_immediate(&mut instruction, 2, immediate as i64)?;

		debug_assert_eq!(instruction.op_count(), 3);
		Ok(instruction)
	}
}

impl With3<MemoryOperand, Register, u32> for Instruction {
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	fn with3(code: Code, memory: MemoryOperand, register: Register, immediate: u32) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(code);

		instruction.set_op0_kind(OpKind::Memory);
		Instruction::init_memory_operand(&mut instruction, &memory);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction.set_op1_register(register);

		instruction_internal::initialize_unsigned_immediate(&mut instruction, 2, immediate as u64)?;

		debug_assert_eq!(instruction.op_count(), 3);
		Ok(instruction)
	}
}

impl With4<Register, Register, Register, Register> for Instruction {
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	fn with4(code: Code, register1: Register, register2: Register, register3: Register, register4: Register) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(code);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction.set_op0_register(register1);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction.set_op1_register(register2);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op2_kind(OpKind::Register);
		instruction.set_op2_register(register3);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op3_kind(OpKind::Register);
		instruction.set_op3_register(register4);

		debug_assert_eq!(instruction.op_count(), 4);
		Ok(instruction)
	}
}

impl With4<Register, Register, Register, i32> for Instruction {
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	fn with4(code: Code, register1: Register, register2: Register, register3: Register, immediate: i32) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(code);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction.set_op0_register(register1);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction.set_op1_register(register2);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op2_kind(OpKind::Register);
		instruction.set_op2_register(register3);

		instruction_internal::initialize_signed_immediate(&mut instruction, 3, immediate as i64)?;

		debug_assert_eq!(instruction.op_count(), 4);
		Ok(instruction)
	}
}

impl With4<Register, Register, Register, u32> for Instruction {
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	fn with4(code: Code, register1: Register, register2: Register, register3: Register, immediate: u32) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(code);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction.set_op0_register(register1);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction.set_op1_register(register2);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op2_kind(OpKind::Register);
		instruction.set_op2_register(register3);

		instruction_internal::initialize_unsigned_immediate(&mut instruction, 3, immediate as u64)?;

		debug_assert_eq!(instruction.op_count(), 4);
		Ok(instruction)
	}
}

impl With4<Register, Register, Register, MemoryOperand> for Instruction {
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	fn with4(code: Code, register1: Register, register2: Register, register3: Register, memory: MemoryOperand) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(code);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction.set_op0_register(register1);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction.set_op1_register(register2);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op2_kind(OpKind::Register);
		instruction.set_op2_register(register3);

		instruction.set_op3_kind(OpKind::Memory);
		Instruction::init_memory_operand(&mut instruction, &memory);

		debug_assert_eq!(instruction.op_count(), 4);
		Ok(instruction)
	}
}

impl With4<Register, Register, i32, i32> for Instruction {
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	fn with4(code: Code, register1: Register, register2: Register, immediate1: i32, immediate2: i32) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(code);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction.set_op0_register(register1);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction.set_op1_register(register2);

		instruction_internal::initialize_signed_immediate(&mut instruction, 2, immediate1 as i64)?;

		instruction_internal::initialize_signed_immediate(&mut instruction, 3, immediate2 as i64)?;

		debug_assert_eq!(instruction.op_count(), 4);
		Ok(instruction)
	}
}

impl With4<Register, Register, u32, u32> for Instruction {
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	fn with4(code: Code, register1: Register, register2: Register, immediate1: u32, immediate2: u32) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(code);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction.set_op0_register(register1);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction.set_op1_register(register2);

		instruction_internal::initialize_unsigned_immediate(&mut instruction, 2, immediate1 as u64)?;

		instruction_internal::initialize_unsigned_immediate(&mut instruction, 3, immediate2 as u64)?;

		debug_assert_eq!(instruction.op_count(), 4);
		Ok(instruction)
	}
}

impl With4<Register, Register, MemoryOperand, Register> for Instruction {
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	fn with4(code: Code, register1: Register, register2: Register, memory: MemoryOperand, register3: Register) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(code);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction.set_op0_register(register1);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction.set_op1_register(register2);

		instruction.set_op2_kind(OpKind::Memory);
		Instruction::init_memory_operand(&mut instruction, &memory);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op3_kind(OpKind::Register);
		instruction.set_op3_register(register3);

		debug_assert_eq!(instruction.op_count(), 4);
		Ok(instruction)
	}
}

impl With4<Register, Register, MemoryOperand, i32> for Instruction {
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	fn with4(code: Code, register1: Register, register2: Register, memory: MemoryOperand, immediate: i32) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(code);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction.set_op0_register(register1);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction.set_op1_register(register2);

		instruction.set_op2_kind(OpKind::Memory);
		Instruction::init_memory_operand(&mut instruction, &memory);

		instruction_internal::initialize_signed_immediate(&mut instruction, 3, immediate as i64)?;

		debug_assert_eq!(instruction.op_count(), 4);
		Ok(instruction)
	}
}

impl With4<Register, Register, MemoryOperand, u32> for Instruction {
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	fn with4(code: Code, register1: Register, register2: Register, memory: MemoryOperand, immediate: u32) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(code);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction.set_op0_register(register1);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction.set_op1_register(register2);

		instruction.set_op2_kind(OpKind::Memory);
		Instruction::init_memory_operand(&mut instruction, &memory);

		instruction_internal::initialize_unsigned_immediate(&mut instruction, 3, immediate as u64)?;

		debug_assert_eq!(instruction.op_count(), 4);
		Ok(instruction)
	}
}

impl With5<Register, Register, Register, Register, i32> for Instruction {
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	fn with5(code: Code, register1: Register, register2: Register, register3: Register, register4: Register, immediate: i32) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(code);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction.set_op0_register(register1);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction.set_op1_register(register2);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op2_kind(OpKind::Register);
		instruction.set_op2_register(register3);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op3_kind(OpKind::Register);
		instruction.set_op3_register(register4);

		instruction_internal::initialize_signed_immediate(&mut instruction, 4, immediate as i64)?;

		debug_assert_eq!(instruction.op_count(), 5);
		Ok(instruction)
	}
}

impl With5<Register, Register, Register, Register, u32> for Instruction {
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	fn with5(code: Code, register1: Register, register2: Register, register3: Register, register4: Register, immediate: u32) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(code);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction.set_op0_register(register1);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction.set_op1_register(register2);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op2_kind(OpKind::Register);
		instruction.set_op2_register(register3);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op3_kind(OpKind::Register);
		instruction.set_op3_register(register4);

		instruction_internal::initialize_unsigned_immediate(&mut instruction, 4, immediate as u64)?;

		debug_assert_eq!(instruction.op_count(), 5);
		Ok(instruction)
	}
}

impl With5<Register, Register, Register, MemoryOperand, i32> for Instruction {
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	fn with5(code: Code, register1: Register, register2: Register, register3: Register, memory: MemoryOperand, immediate: i32) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(code);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction.set_op0_register(register1);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction.set_op1_register(register2);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op2_kind(OpKind::Register);
		instruction.set_op2_register(register3);

		instruction.set_op3_kind(OpKind::Memory);
		Instruction::init_memory_operand(&mut instruction, &memory);

		instruction_internal::initialize_signed_immediate(&mut instruction, 4, immediate as i64)?;

		debug_assert_eq!(instruction.op_count(), 5);
		Ok(instruction)
	}
}

impl With5<Register, Register, Register, MemoryOperand, u32> for Instruction {
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	fn with5(code: Code, register1: Register, register2: Register, register3: Register, memory: MemoryOperand, immediate: u32) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(code);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction.set_op0_register(register1);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction.set_op1_register(register2);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op2_kind(OpKind::Register);
		instruction.set_op2_register(register3);

		instruction.set_op3_kind(OpKind::Memory);
		Instruction::init_memory_operand(&mut instruction, &memory);

		instruction_internal::initialize_unsigned_immediate(&mut instruction, 4, immediate as u64)?;

		debug_assert_eq!(instruction.op_count(), 5);
		Ok(instruction)
	}
}

impl With5<Register, Register, MemoryOperand, Register, i32> for Instruction {
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	fn with5(code: Code, register1: Register, register2: Register, memory: MemoryOperand, register3: Register, immediate: i32) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(code);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction.set_op0_register(register1);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction.set_op1_register(register2);

		instruction.set_op2_kind(OpKind::Memory);
		Instruction::init_memory_operand(&mut instruction, &memory);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op3_kind(OpKind::Register);
		instruction.set_op3_register(register3);

		instruction_internal::initialize_signed_immediate(&mut instruction, 4, immediate as i64)?;

		debug_assert_eq!(instruction.op_count(), 5);
		Ok(instruction)
	}
}

impl With5<Register, Register, MemoryOperand, Register, u32> for Instruction {
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	fn with5(code: Code, register1: Register, register2: Register, memory: MemoryOperand, register3: Register, immediate: u32) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(code);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction.set_op0_register(register1);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction.set_op1_register(register2);

		instruction.set_op2_kind(OpKind::Memory);
		Instruction::init_memory_operand(&mut instruction, &memory);

		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op3_kind(OpKind::Register);
		instruction.set_op3_register(register3);

		instruction_internal::initialize_unsigned_immediate(&mut instruction, 4, immediate as u64)?;

		debug_assert_eq!(instruction.op_count(), 5);
		Ok(instruction)
	}
}

impl Instruction {
	/// Creates a new near/short branch instruction
	///
	/// # Errors
	///
	/// Fails if the created instruction doesn't have a near branch operand
	///
	/// # Arguments
	///
	/// * `code`: Code value
	/// * `target`: Target address
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	pub fn with_branch(code: Code, target: u64) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(code);

		instruction.set_op0_kind(instruction_internal::get_near_branch_op_kind(code, 0)?);
		instruction.set_near_branch64(target);

		debug_assert_eq!(instruction.op_count(), 1);
		Ok(instruction)
	}

	/// Creates a new far branch instruction
	///
	/// # Errors
	///
	/// Fails if the created instruction doesn't have a far branch operand
	///
	/// # Arguments
	///
	/// * `code`: Code value
	/// * `selector`: Selector/segment value
	/// * `offset`: Offset
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	pub fn with_far_branch(code: Code, selector: u16, offset: u32) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(code);

		instruction.set_op0_kind(instruction_internal::get_far_branch_op_kind(code, 0)?);
		instruction.set_far_branch_selector(selector);
		instruction.set_far_branch32(offset);

		debug_assert_eq!(instruction.op_count(), 1);
		Ok(instruction)
	}

	/// Creates a new `XBEGIN` instruction
	///
	/// # Errors
	///
	/// Fails if `bitness` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `bitness`: 16, 32, or 64
	/// * `target`: Target address
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	pub fn with_xbegin(bitness: u32, target: u64) -> Result<Self, IcedError> {
		let mut instruction = Self::default();

		match bitness {
			16 => {
				instruction.set_code(Code::Xbegin_rel16);
				instruction.set_op0_kind(OpKind::NearBranch32);
				instruction.set_near_branch32(target as u32);
			}

			32 => {
				instruction.set_code(Code::Xbegin_rel32);
				instruction.set_op0_kind(OpKind::NearBranch32);
				instruction.set_near_branch32(target as u32);
			}

			64 => {
				instruction.set_code(Code::Xbegin_rel32);
				instruction.set_op0_kind(OpKind::NearBranch64);
				instruction.set_near_branch64(target);
			}

			_ => return Err(IcedError::new("Invalid bitness")),
		}

		debug_assert_eq!(instruction.op_count(), 1);
		Ok(instruction)
	}

	/// Creates a `OUTSB` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	/// * `segment_prefix`: Segment override or [`Register::None`]
	/// * `rep_prefix`: Rep prefix or [`RepPrefixKind::None`]
	///
	/// [`Register::None`]: enum.Register.html#variant.None
	/// [`RepPrefixKind::None`]: enum.RepPrefixKind.html#variant.None
	#[inline]
	#[rustfmt::skip]
	pub fn with_outsb(address_size: u32, segment_prefix: Register, rep_prefix: RepPrefixKind) -> Result<Self, IcedError> {
		instruction_internal::with_string_reg_segrsi(Code::Outsb_DX_m8, address_size, Register::DX, segment_prefix, rep_prefix)
	}

	/// Creates a `REP OUTSB` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	#[inline]
	#[rustfmt::skip]
	pub fn with_rep_outsb(address_size: u32) -> Result<Self, IcedError> {
		instruction_internal::with_string_reg_segrsi(Code::Outsb_DX_m8, address_size, Register::DX, Register::None, RepPrefixKind::Repe)
	}

	/// Creates a `OUTSW` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	/// * `segment_prefix`: Segment override or [`Register::None`]
	/// * `rep_prefix`: Rep prefix or [`RepPrefixKind::None`]
	///
	/// [`Register::None`]: enum.Register.html#variant.None
	/// [`RepPrefixKind::None`]: enum.RepPrefixKind.html#variant.None
	#[inline]
	#[rustfmt::skip]
	pub fn with_outsw(address_size: u32, segment_prefix: Register, rep_prefix: RepPrefixKind) -> Result<Self, IcedError> {
		instruction_internal::with_string_reg_segrsi(Code::Outsw_DX_m16, address_size, Register::DX, segment_prefix, rep_prefix)
	}

	/// Creates a `REP OUTSW` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	#[inline]
	#[rustfmt::skip]
	pub fn with_rep_outsw(address_size: u32) -> Result<Self, IcedError> {
		instruction_internal::with_string_reg_segrsi(Code::Outsw_DX_m16, address_size, Register::DX, Register::None, RepPrefixKind::Repe)
	}

	/// Creates a `OUTSD` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	/// * `segment_prefix`: Segment override or [`Register::None`]
	/// * `rep_prefix`: Rep prefix or [`RepPrefixKind::None`]
	///
	/// [`Register::None`]: enum.Register.html#variant.None
	/// [`RepPrefixKind::None`]: enum.RepPrefixKind.html#variant.None
	#[inline]
	#[rustfmt::skip]
	pub fn with_outsd(address_size: u32, segment_prefix: Register, rep_prefix: RepPrefixKind) -> Result<Self, IcedError> {
		instruction_internal::with_string_reg_segrsi(Code::Outsd_DX_m32, address_size, Register::DX, segment_prefix, rep_prefix)
	}

	/// Creates a `REP OUTSD` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	#[inline]
	#[rustfmt::skip]
	pub fn with_rep_outsd(address_size: u32) -> Result<Self, IcedError> {
		instruction_internal::with_string_reg_segrsi(Code::Outsd_DX_m32, address_size, Register::DX, Register::None, RepPrefixKind::Repe)
	}

	/// Creates a `LODSB` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	/// * `segment_prefix`: Segment override or [`Register::None`]
	/// * `rep_prefix`: Rep prefix or [`RepPrefixKind::None`]
	///
	/// [`Register::None`]: enum.Register.html#variant.None
	/// [`RepPrefixKind::None`]: enum.RepPrefixKind.html#variant.None
	#[inline]
	#[rustfmt::skip]
	pub fn with_lodsb(address_size: u32, segment_prefix: Register, rep_prefix: RepPrefixKind) -> Result<Self, IcedError> {
		instruction_internal::with_string_reg_segrsi(Code::Lodsb_AL_m8, address_size, Register::AL, segment_prefix, rep_prefix)
	}

	/// Creates a `REP LODSB` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	#[inline]
	#[rustfmt::skip]
	pub fn with_rep_lodsb(address_size: u32) -> Result<Self, IcedError> {
		instruction_internal::with_string_reg_segrsi(Code::Lodsb_AL_m8, address_size, Register::AL, Register::None, RepPrefixKind::Repe)
	}

	/// Creates a `LODSW` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	/// * `segment_prefix`: Segment override or [`Register::None`]
	/// * `rep_prefix`: Rep prefix or [`RepPrefixKind::None`]
	///
	/// [`Register::None`]: enum.Register.html#variant.None
	/// [`RepPrefixKind::None`]: enum.RepPrefixKind.html#variant.None
	#[inline]
	#[rustfmt::skip]
	pub fn with_lodsw(address_size: u32, segment_prefix: Register, rep_prefix: RepPrefixKind) -> Result<Self, IcedError> {
		instruction_internal::with_string_reg_segrsi(Code::Lodsw_AX_m16, address_size, Register::AX, segment_prefix, rep_prefix)
	}

	/// Creates a `REP LODSW` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	#[inline]
	#[rustfmt::skip]
	pub fn with_rep_lodsw(address_size: u32) -> Result<Self, IcedError> {
		instruction_internal::with_string_reg_segrsi(Code::Lodsw_AX_m16, address_size, Register::AX, Register::None, RepPrefixKind::Repe)
	}

	/// Creates a `LODSD` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	/// * `segment_prefix`: Segment override or [`Register::None`]
	/// * `rep_prefix`: Rep prefix or [`RepPrefixKind::None`]
	///
	/// [`Register::None`]: enum.Register.html#variant.None
	/// [`RepPrefixKind::None`]: enum.RepPrefixKind.html#variant.None
	#[inline]
	#[rustfmt::skip]
	pub fn with_lodsd(address_size: u32, segment_prefix: Register, rep_prefix: RepPrefixKind) -> Result<Self, IcedError> {
		instruction_internal::with_string_reg_segrsi(Code::Lodsd_EAX_m32, address_size, Register::EAX, segment_prefix, rep_prefix)
	}

	/// Creates a `REP LODSD` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	#[inline]
	#[rustfmt::skip]
	pub fn with_rep_lodsd(address_size: u32) -> Result<Self, IcedError> {
		instruction_internal::with_string_reg_segrsi(Code::Lodsd_EAX_m32, address_size, Register::EAX, Register::None, RepPrefixKind::Repe)
	}

	/// Creates a `LODSQ` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	/// * `segment_prefix`: Segment override or [`Register::None`]
	/// * `rep_prefix`: Rep prefix or [`RepPrefixKind::None`]
	///
	/// [`Register::None`]: enum.Register.html#variant.None
	/// [`RepPrefixKind::None`]: enum.RepPrefixKind.html#variant.None
	#[inline]
	#[rustfmt::skip]
	pub fn with_lodsq(address_size: u32, segment_prefix: Register, rep_prefix: RepPrefixKind) -> Result<Self, IcedError> {
		instruction_internal::with_string_reg_segrsi(Code::Lodsq_RAX_m64, address_size, Register::RAX, segment_prefix, rep_prefix)
	}

	/// Creates a `REP LODSQ` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	#[inline]
	#[rustfmt::skip]
	pub fn with_rep_lodsq(address_size: u32) -> Result<Self, IcedError> {
		instruction_internal::with_string_reg_segrsi(Code::Lodsq_RAX_m64, address_size, Register::RAX, Register::None, RepPrefixKind::Repe)
	}

	/// Creates a `SCASB` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	/// * `rep_prefix`: Rep prefix or [`RepPrefixKind::None`]
	///
	/// [`RepPrefixKind::None`]: enum.RepPrefixKind.html#variant.None
	#[inline]
	#[rustfmt::skip]
	pub fn with_scasb(address_size: u32, rep_prefix: RepPrefixKind) -> Result<Self, IcedError> {
		instruction_internal::with_string_reg_esrdi(Code::Scasb_AL_m8, address_size, Register::AL, rep_prefix)
	}

	/// Creates a `REPE SCASB` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	#[inline]
	#[rustfmt::skip]
	pub fn with_repe_scasb(address_size: u32) -> Result<Self, IcedError> {
		instruction_internal::with_string_reg_esrdi(Code::Scasb_AL_m8, address_size, Register::AL, RepPrefixKind::Repe)
	}

	/// Creates a `REPNE SCASB` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	#[inline]
	#[rustfmt::skip]
	pub fn with_repne_scasb(address_size: u32) -> Result<Self, IcedError> {
		instruction_internal::with_string_reg_esrdi(Code::Scasb_AL_m8, address_size, Register::AL, RepPrefixKind::Repne)
	}

	/// Creates a `SCASW` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	/// * `rep_prefix`: Rep prefix or [`RepPrefixKind::None`]
	///
	/// [`RepPrefixKind::None`]: enum.RepPrefixKind.html#variant.None
	#[inline]
	#[rustfmt::skip]
	pub fn with_scasw(address_size: u32, rep_prefix: RepPrefixKind) -> Result<Self, IcedError> {
		instruction_internal::with_string_reg_esrdi(Code::Scasw_AX_m16, address_size, Register::AX, rep_prefix)
	}

	/// Creates a `REPE SCASW` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	#[inline]
	#[rustfmt::skip]
	pub fn with_repe_scasw(address_size: u32) -> Result<Self, IcedError> {
		instruction_internal::with_string_reg_esrdi(Code::Scasw_AX_m16, address_size, Register::AX, RepPrefixKind::Repe)
	}

	/// Creates a `REPNE SCASW` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	#[inline]
	#[rustfmt::skip]
	pub fn with_repne_scasw(address_size: u32) -> Result<Self, IcedError> {
		instruction_internal::with_string_reg_esrdi(Code::Scasw_AX_m16, address_size, Register::AX, RepPrefixKind::Repne)
	}

	/// Creates a `SCASD` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	/// * `rep_prefix`: Rep prefix or [`RepPrefixKind::None`]
	///
	/// [`RepPrefixKind::None`]: enum.RepPrefixKind.html#variant.None
	#[inline]
	#[rustfmt::skip]
	pub fn with_scasd(address_size: u32, rep_prefix: RepPrefixKind) -> Result<Self, IcedError> {
		instruction_internal::with_string_reg_esrdi(Code::Scasd_EAX_m32, address_size, Register::EAX, rep_prefix)
	}

	/// Creates a `REPE SCASD` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	#[inline]
	#[rustfmt::skip]
	pub fn with_repe_scasd(address_size: u32) -> Result<Self, IcedError> {
		instruction_internal::with_string_reg_esrdi(Code::Scasd_EAX_m32, address_size, Register::EAX, RepPrefixKind::Repe)
	}

	/// Creates a `REPNE SCASD` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	#[inline]
	#[rustfmt::skip]
	pub fn with_repne_scasd(address_size: u32) -> Result<Self, IcedError> {
		instruction_internal::with_string_reg_esrdi(Code::Scasd_EAX_m32, address_size, Register::EAX, RepPrefixKind::Repne)
	}

	/// Creates a `SCASQ` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	/// * `rep_prefix`: Rep prefix or [`RepPrefixKind::None`]
	///
	/// [`RepPrefixKind::None`]: enum.RepPrefixKind.html#variant.None
	#[inline]
	#[rustfmt::skip]
	pub fn with_scasq(address_size: u32, rep_prefix: RepPrefixKind) -> Result<Self, IcedError> {
		instruction_internal::with_string_reg_esrdi(Code::Scasq_RAX_m64, address_size, Register::RAX, rep_prefix)
	}

	/// Creates a `REPE SCASQ` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	#[inline]
	#[rustfmt::skip]
	pub fn with_repe_scasq(address_size: u32) -> Result<Self, IcedError> {
		instruction_internal::with_string_reg_esrdi(Code::Scasq_RAX_m64, address_size, Register::RAX, RepPrefixKind::Repe)
	}

	/// Creates a `REPNE SCASQ` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	#[inline]
	#[rustfmt::skip]
	pub fn with_repne_scasq(address_size: u32) -> Result<Self, IcedError> {
		instruction_internal::with_string_reg_esrdi(Code::Scasq_RAX_m64, address_size, Register::RAX, RepPrefixKind::Repne)
	}

	/// Creates a `INSB` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	/// * `rep_prefix`: Rep prefix or [`RepPrefixKind::None`]
	///
	/// [`RepPrefixKind::None`]: enum.RepPrefixKind.html#variant.None
	#[inline]
	#[rustfmt::skip]
	pub fn with_insb(address_size: u32, rep_prefix: RepPrefixKind) -> Result<Self, IcedError> {
		instruction_internal::with_string_esrdi_reg(Code::Insb_m8_DX, address_size, Register::DX, rep_prefix)
	}

	/// Creates a `REP INSB` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	#[inline]
	#[rustfmt::skip]
	pub fn with_rep_insb(address_size: u32) -> Result<Self, IcedError> {
		instruction_internal::with_string_esrdi_reg(Code::Insb_m8_DX, address_size, Register::DX, RepPrefixKind::Repe)
	}

	/// Creates a `INSW` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	/// * `rep_prefix`: Rep prefix or [`RepPrefixKind::None`]
	///
	/// [`RepPrefixKind::None`]: enum.RepPrefixKind.html#variant.None
	#[inline]
	#[rustfmt::skip]
	pub fn with_insw(address_size: u32, rep_prefix: RepPrefixKind) -> Result<Self, IcedError> {
		instruction_internal::with_string_esrdi_reg(Code::Insw_m16_DX, address_size, Register::DX, rep_prefix)
	}

	/// Creates a `REP INSW` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	#[inline]
	#[rustfmt::skip]
	pub fn with_rep_insw(address_size: u32) -> Result<Self, IcedError> {
		instruction_internal::with_string_esrdi_reg(Code::Insw_m16_DX, address_size, Register::DX, RepPrefixKind::Repe)
	}

	/// Creates a `INSD` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	/// * `rep_prefix`: Rep prefix or [`RepPrefixKind::None`]
	///
	/// [`RepPrefixKind::None`]: enum.RepPrefixKind.html#variant.None
	#[inline]
	#[rustfmt::skip]
	pub fn with_insd(address_size: u32, rep_prefix: RepPrefixKind) -> Result<Self, IcedError> {
		instruction_internal::with_string_esrdi_reg(Code::Insd_m32_DX, address_size, Register::DX, rep_prefix)
	}

	/// Creates a `REP INSD` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	#[inline]
	#[rustfmt::skip]
	pub fn with_rep_insd(address_size: u32) -> Result<Self, IcedError> {
		instruction_internal::with_string_esrdi_reg(Code::Insd_m32_DX, address_size, Register::DX, RepPrefixKind::Repe)
	}

	/// Creates a `STOSB` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	/// * `rep_prefix`: Rep prefix or [`RepPrefixKind::None`]
	///
	/// [`RepPrefixKind::None`]: enum.RepPrefixKind.html#variant.None
	#[inline]
	#[rustfmt::skip]
	pub fn with_stosb(address_size: u32, rep_prefix: RepPrefixKind) -> Result<Self, IcedError> {
		instruction_internal::with_string_esrdi_reg(Code::Stosb_m8_AL, address_size, Register::AL, rep_prefix)
	}

	/// Creates a `REP STOSB` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	#[inline]
	#[rustfmt::skip]
	pub fn with_rep_stosb(address_size: u32) -> Result<Self, IcedError> {
		instruction_internal::with_string_esrdi_reg(Code::Stosb_m8_AL, address_size, Register::AL, RepPrefixKind::Repe)
	}

	/// Creates a `STOSW` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	/// * `rep_prefix`: Rep prefix or [`RepPrefixKind::None`]
	///
	/// [`RepPrefixKind::None`]: enum.RepPrefixKind.html#variant.None
	#[inline]
	#[rustfmt::skip]
	pub fn with_stosw(address_size: u32, rep_prefix: RepPrefixKind) -> Result<Self, IcedError> {
		instruction_internal::with_string_esrdi_reg(Code::Stosw_m16_AX, address_size, Register::AX, rep_prefix)
	}

	/// Creates a `REP STOSW` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	#[inline]
	#[rustfmt::skip]
	pub fn with_rep_stosw(address_size: u32) -> Result<Self, IcedError> {
		instruction_internal::with_string_esrdi_reg(Code::Stosw_m16_AX, address_size, Register::AX, RepPrefixKind::Repe)
	}

	/// Creates a `STOSD` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	/// * `rep_prefix`: Rep prefix or [`RepPrefixKind::None`]
	///
	/// [`RepPrefixKind::None`]: enum.RepPrefixKind.html#variant.None
	#[inline]
	#[rustfmt::skip]
	pub fn with_stosd(address_size: u32, rep_prefix: RepPrefixKind) -> Result<Self, IcedError> {
		instruction_internal::with_string_esrdi_reg(Code::Stosd_m32_EAX, address_size, Register::EAX, rep_prefix)
	}

	/// Creates a `REP STOSD` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	#[inline]
	#[rustfmt::skip]
	pub fn with_rep_stosd(address_size: u32) -> Result<Self, IcedError> {
		instruction_internal::with_string_esrdi_reg(Code::Stosd_m32_EAX, address_size, Register::EAX, RepPrefixKind::Repe)
	}

	/// Creates a `STOSQ` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	/// * `rep_prefix`: Rep prefix or [`RepPrefixKind::None`]
	///
	/// [`RepPrefixKind::None`]: enum.RepPrefixKind.html#variant.None
	#[inline]
	#[rustfmt::skip]
	pub fn with_stosq(address_size: u32, rep_prefix: RepPrefixKind) -> Result<Self, IcedError> {
		instruction_internal::with_string_esrdi_reg(Code::Stosq_m64_RAX, address_size, Register::RAX, rep_prefix)
	}

	/// Creates a `REP STOSQ` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	#[inline]
	#[rustfmt::skip]
	pub fn with_rep_stosq(address_size: u32) -> Result<Self, IcedError> {
		instruction_internal::with_string_esrdi_reg(Code::Stosq_m64_RAX, address_size, Register::RAX, RepPrefixKind::Repe)
	}

	/// Creates a `CMPSB` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	/// * `segment_prefix`: Segment override or [`Register::None`]
	/// * `rep_prefix`: Rep prefix or [`RepPrefixKind::None`]
	///
	/// [`Register::None`]: enum.Register.html#variant.None
	/// [`RepPrefixKind::None`]: enum.RepPrefixKind.html#variant.None
	#[inline]
	#[rustfmt::skip]
	pub fn with_cmpsb(address_size: u32, segment_prefix: Register, rep_prefix: RepPrefixKind) -> Result<Self, IcedError> {
		instruction_internal::with_string_segrsi_esrdi(Code::Cmpsb_m8_m8, address_size, segment_prefix, rep_prefix)
	}

	/// Creates a `REPE CMPSB` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	#[inline]
	#[rustfmt::skip]
	pub fn with_repe_cmpsb(address_size: u32) -> Result<Self, IcedError> {
		instruction_internal::with_string_segrsi_esrdi(Code::Cmpsb_m8_m8, address_size, Register::None, RepPrefixKind::Repe)
	}

	/// Creates a `REPNE CMPSB` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	#[inline]
	#[rustfmt::skip]
	pub fn with_repne_cmpsb(address_size: u32) -> Result<Self, IcedError> {
		instruction_internal::with_string_segrsi_esrdi(Code::Cmpsb_m8_m8, address_size, Register::None, RepPrefixKind::Repne)
	}

	/// Creates a `CMPSW` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	/// * `segment_prefix`: Segment override or [`Register::None`]
	/// * `rep_prefix`: Rep prefix or [`RepPrefixKind::None`]
	///
	/// [`Register::None`]: enum.Register.html#variant.None
	/// [`RepPrefixKind::None`]: enum.RepPrefixKind.html#variant.None
	#[inline]
	#[rustfmt::skip]
	pub fn with_cmpsw(address_size: u32, segment_prefix: Register, rep_prefix: RepPrefixKind) -> Result<Self, IcedError> {
		instruction_internal::with_string_segrsi_esrdi(Code::Cmpsw_m16_m16, address_size, segment_prefix, rep_prefix)
	}

	/// Creates a `REPE CMPSW` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	#[inline]
	#[rustfmt::skip]
	pub fn with_repe_cmpsw(address_size: u32) -> Result<Self, IcedError> {
		instruction_internal::with_string_segrsi_esrdi(Code::Cmpsw_m16_m16, address_size, Register::None, RepPrefixKind::Repe)
	}

	/// Creates a `REPNE CMPSW` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	#[inline]
	#[rustfmt::skip]
	pub fn with_repne_cmpsw(address_size: u32) -> Result<Self, IcedError> {
		instruction_internal::with_string_segrsi_esrdi(Code::Cmpsw_m16_m16, address_size, Register::None, RepPrefixKind::Repne)
	}

	/// Creates a `CMPSD` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	/// * `segment_prefix`: Segment override or [`Register::None`]
	/// * `rep_prefix`: Rep prefix or [`RepPrefixKind::None`]
	///
	/// [`Register::None`]: enum.Register.html#variant.None
	/// [`RepPrefixKind::None`]: enum.RepPrefixKind.html#variant.None
	#[inline]
	#[rustfmt::skip]
	pub fn with_cmpsd(address_size: u32, segment_prefix: Register, rep_prefix: RepPrefixKind) -> Result<Self, IcedError> {
		instruction_internal::with_string_segrsi_esrdi(Code::Cmpsd_m32_m32, address_size, segment_prefix, rep_prefix)
	}

	/// Creates a `REPE CMPSD` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	#[inline]
	#[rustfmt::skip]
	pub fn with_repe_cmpsd(address_size: u32) -> Result<Self, IcedError> {
		instruction_internal::with_string_segrsi_esrdi(Code::Cmpsd_m32_m32, address_size, Register::None, RepPrefixKind::Repe)
	}

	/// Creates a `REPNE CMPSD` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	#[inline]
	#[rustfmt::skip]
	pub fn with_repne_cmpsd(address_size: u32) -> Result<Self, IcedError> {
		instruction_internal::with_string_segrsi_esrdi(Code::Cmpsd_m32_m32, address_size, Register::None, RepPrefixKind::Repne)
	}

	/// Creates a `CMPSQ` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	/// * `segment_prefix`: Segment override or [`Register::None`]
	/// * `rep_prefix`: Rep prefix or [`RepPrefixKind::None`]
	///
	/// [`Register::None`]: enum.Register.html#variant.None
	/// [`RepPrefixKind::None`]: enum.RepPrefixKind.html#variant.None
	#[inline]
	#[rustfmt::skip]
	pub fn with_cmpsq(address_size: u32, segment_prefix: Register, rep_prefix: RepPrefixKind) -> Result<Self, IcedError> {
		instruction_internal::with_string_segrsi_esrdi(Code::Cmpsq_m64_m64, address_size, segment_prefix, rep_prefix)
	}

	/// Creates a `REPE CMPSQ` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	#[inline]
	#[rustfmt::skip]
	pub fn with_repe_cmpsq(address_size: u32) -> Result<Self, IcedError> {
		instruction_internal::with_string_segrsi_esrdi(Code::Cmpsq_m64_m64, address_size, Register::None, RepPrefixKind::Repe)
	}

	/// Creates a `REPNE CMPSQ` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	#[inline]
	#[rustfmt::skip]
	pub fn with_repne_cmpsq(address_size: u32) -> Result<Self, IcedError> {
		instruction_internal::with_string_segrsi_esrdi(Code::Cmpsq_m64_m64, address_size, Register::None, RepPrefixKind::Repne)
	}

	/// Creates a `MOVSB` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	/// * `segment_prefix`: Segment override or [`Register::None`]
	/// * `rep_prefix`: Rep prefix or [`RepPrefixKind::None`]
	///
	/// [`Register::None`]: enum.Register.html#variant.None
	/// [`RepPrefixKind::None`]: enum.RepPrefixKind.html#variant.None
	#[inline]
	#[rustfmt::skip]
	pub fn with_movsb(address_size: u32, segment_prefix: Register, rep_prefix: RepPrefixKind) -> Result<Self, IcedError> {
		instruction_internal::with_string_esrdi_segrsi(Code::Movsb_m8_m8, address_size, segment_prefix, rep_prefix)
	}

	/// Creates a `REP MOVSB` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	#[inline]
	#[rustfmt::skip]
	pub fn with_rep_movsb(address_size: u32) -> Result<Self, IcedError> {
		instruction_internal::with_string_esrdi_segrsi(Code::Movsb_m8_m8, address_size, Register::None, RepPrefixKind::Repe)
	}

	/// Creates a `MOVSW` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	/// * `segment_prefix`: Segment override or [`Register::None`]
	/// * `rep_prefix`: Rep prefix or [`RepPrefixKind::None`]
	///
	/// [`Register::None`]: enum.Register.html#variant.None
	/// [`RepPrefixKind::None`]: enum.RepPrefixKind.html#variant.None
	#[inline]
	#[rustfmt::skip]
	pub fn with_movsw(address_size: u32, segment_prefix: Register, rep_prefix: RepPrefixKind) -> Result<Self, IcedError> {
		instruction_internal::with_string_esrdi_segrsi(Code::Movsw_m16_m16, address_size, segment_prefix, rep_prefix)
	}

	/// Creates a `REP MOVSW` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	#[inline]
	#[rustfmt::skip]
	pub fn with_rep_movsw(address_size: u32) -> Result<Self, IcedError> {
		instruction_internal::with_string_esrdi_segrsi(Code::Movsw_m16_m16, address_size, Register::None, RepPrefixKind::Repe)
	}

	/// Creates a `MOVSD` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	/// * `segment_prefix`: Segment override or [`Register::None`]
	/// * `rep_prefix`: Rep prefix or [`RepPrefixKind::None`]
	///
	/// [`Register::None`]: enum.Register.html#variant.None
	/// [`RepPrefixKind::None`]: enum.RepPrefixKind.html#variant.None
	#[inline]
	#[rustfmt::skip]
	pub fn with_movsd(address_size: u32, segment_prefix: Register, rep_prefix: RepPrefixKind) -> Result<Self, IcedError> {
		instruction_internal::with_string_esrdi_segrsi(Code::Movsd_m32_m32, address_size, segment_prefix, rep_prefix)
	}

	/// Creates a `REP MOVSD` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	#[inline]
	#[rustfmt::skip]
	pub fn with_rep_movsd(address_size: u32) -> Result<Self, IcedError> {
		instruction_internal::with_string_esrdi_segrsi(Code::Movsd_m32_m32, address_size, Register::None, RepPrefixKind::Repe)
	}

	/// Creates a `MOVSQ` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	/// * `segment_prefix`: Segment override or [`Register::None`]
	/// * `rep_prefix`: Rep prefix or [`RepPrefixKind::None`]
	///
	/// [`Register::None`]: enum.Register.html#variant.None
	/// [`RepPrefixKind::None`]: enum.RepPrefixKind.html#variant.None
	#[inline]
	#[rustfmt::skip]
	pub fn with_movsq(address_size: u32, segment_prefix: Register, rep_prefix: RepPrefixKind) -> Result<Self, IcedError> {
		instruction_internal::with_string_esrdi_segrsi(Code::Movsq_m64_m64, address_size, segment_prefix, rep_prefix)
	}

	/// Creates a `REP MOVSQ` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	#[inline]
	#[rustfmt::skip]
	pub fn with_rep_movsq(address_size: u32) -> Result<Self, IcedError> {
		instruction_internal::with_string_esrdi_segrsi(Code::Movsq_m64_m64, address_size, Register::None, RepPrefixKind::Repe)
	}

	/// Creates a `MASKMOVQ` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	/// * `register1`: Register
	/// * `register2`: Register
	/// * `segment_prefix`: Segment override or [`Register::None`]
	///
	/// [`Register::None`]: enum.Register.html#variant.None
	#[inline]
	#[rustfmt::skip]
	pub fn with_maskmovq(address_size: u32, register1: Register, register2: Register, segment_prefix: Register) -> Result<Self, IcedError> {
		instruction_internal::with_maskmov(Code::Maskmovq_rDI_mm_mm, address_size, register1, register2, segment_prefix)
	}

	/// Creates a `MASKMOVDQU` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	/// * `register1`: Register
	/// * `register2`: Register
	/// * `segment_prefix`: Segment override or [`Register::None`]
	///
	/// [`Register::None`]: enum.Register.html#variant.None
	#[inline]
	#[rustfmt::skip]
	pub fn with_maskmovdqu(address_size: u32, register1: Register, register2: Register, segment_prefix: Register) -> Result<Self, IcedError> {
		instruction_internal::with_maskmov(Code::Maskmovdqu_rDI_xmm_xmm, address_size, register1, register2, segment_prefix)
	}

	/// Creates a `VMASKMOVDQU` instruction
	///
	/// # Errors
	///
	/// Fails if `address_size` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `address_size`: 16, 32, or 64
	/// * `register1`: Register
	/// * `register2`: Register
	/// * `segment_prefix`: Segment override or [`Register::None`]
	///
	/// [`Register::None`]: enum.Register.html#variant.None
	#[inline]
	#[rustfmt::skip]
	pub fn with_vmaskmovdqu(address_size: u32, register1: Register, register2: Register, segment_prefix: Register) -> Result<Self, IcedError> {
		instruction_internal::with_maskmov(Code::VEX_Vmaskmovdqu_rDI_xmm_xmm, address_size, register1, register2, segment_prefix)
	}

	#[doc(hidden)]
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	pub fn try_with_declare_byte_1(b0: u8) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(Code::DeclareByte);
		instruction_internal::internal_set_declare_data_len(&mut instruction, 1);

		instruction.try_set_declare_byte_value(0, b0)?;

		debug_assert_eq!(instruction.op_count(), 0);
		Ok(instruction)
	}

	/// Creates a `db`/`.byte` asm directive
	///
	/// # Arguments
	///
	/// * `b0`: Byte 0
	#[allow(clippy::unwrap_used)]
	#[must_use]
	#[inline]
	#[rustfmt::skip]
	pub fn with_declare_byte_1(b0: u8) -> Self {
		Instruction::try_with_declare_byte_1(b0).unwrap()
	}

	#[doc(hidden)]
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	pub fn try_with_declare_byte_2(b0: u8, b1: u8) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(Code::DeclareByte);
		instruction_internal::internal_set_declare_data_len(&mut instruction, 2);

		instruction.try_set_declare_byte_value(0, b0)?;
		instruction.try_set_declare_byte_value(1, b1)?;

		debug_assert_eq!(instruction.op_count(), 0);
		Ok(instruction)
	}

	/// Creates a `db`/`.byte` asm directive
	///
	/// # Arguments
	///
	/// * `b0`: Byte 0
	/// * `b1`: Byte 1
	#[allow(clippy::unwrap_used)]
	#[must_use]
	#[inline]
	#[rustfmt::skip]
	pub fn with_declare_byte_2(b0: u8, b1: u8) -> Self {
		Instruction::try_with_declare_byte_2(b0, b1).unwrap()
	}

	#[doc(hidden)]
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	pub fn try_with_declare_byte_3(b0: u8, b1: u8, b2: u8) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(Code::DeclareByte);
		instruction_internal::internal_set_declare_data_len(&mut instruction, 3);

		instruction.try_set_declare_byte_value(0, b0)?;
		instruction.try_set_declare_byte_value(1, b1)?;
		instruction.try_set_declare_byte_value(2, b2)?;

		debug_assert_eq!(instruction.op_count(), 0);
		Ok(instruction)
	}

	/// Creates a `db`/`.byte` asm directive
	///
	/// # Arguments
	///
	/// * `b0`: Byte 0
	/// * `b1`: Byte 1
	/// * `b2`: Byte 2
	#[allow(clippy::unwrap_used)]
	#[must_use]
	#[inline]
	#[rustfmt::skip]
	pub fn with_declare_byte_3(b0: u8, b1: u8, b2: u8) -> Self {
		Instruction::try_with_declare_byte_3(b0, b1, b2).unwrap()
	}

	#[doc(hidden)]
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	pub fn try_with_declare_byte_4(b0: u8, b1: u8, b2: u8, b3: u8) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(Code::DeclareByte);
		instruction_internal::internal_set_declare_data_len(&mut instruction, 4);

		instruction.try_set_declare_byte_value(0, b0)?;
		instruction.try_set_declare_byte_value(1, b1)?;
		instruction.try_set_declare_byte_value(2, b2)?;
		instruction.try_set_declare_byte_value(3, b3)?;

		debug_assert_eq!(instruction.op_count(), 0);
		Ok(instruction)
	}

	/// Creates a `db`/`.byte` asm directive
	///
	/// # Arguments
	///
	/// * `b0`: Byte 0
	/// * `b1`: Byte 1
	/// * `b2`: Byte 2
	/// * `b3`: Byte 3
	#[allow(clippy::unwrap_used)]
	#[must_use]
	#[inline]
	#[rustfmt::skip]
	pub fn with_declare_byte_4(b0: u8, b1: u8, b2: u8, b3: u8) -> Self {
		Instruction::try_with_declare_byte_4(b0, b1, b2, b3).unwrap()
	}

	#[doc(hidden)]
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	pub fn try_with_declare_byte_5(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(Code::DeclareByte);
		instruction_internal::internal_set_declare_data_len(&mut instruction, 5);

		instruction.try_set_declare_byte_value(0, b0)?;
		instruction.try_set_declare_byte_value(1, b1)?;
		instruction.try_set_declare_byte_value(2, b2)?;
		instruction.try_set_declare_byte_value(3, b3)?;
		instruction.try_set_declare_byte_value(4, b4)?;

		debug_assert_eq!(instruction.op_count(), 0);
		Ok(instruction)
	}

	/// Creates a `db`/`.byte` asm directive
	///
	/// # Arguments
	///
	/// * `b0`: Byte 0
	/// * `b1`: Byte 1
	/// * `b2`: Byte 2
	/// * `b3`: Byte 3
	/// * `b4`: Byte 4
	#[allow(clippy::unwrap_used)]
	#[must_use]
	#[inline]
	#[rustfmt::skip]
	pub fn with_declare_byte_5(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8) -> Self {
		Instruction::try_with_declare_byte_5(b0, b1, b2, b3, b4).unwrap()
	}

	#[doc(hidden)]
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	pub fn try_with_declare_byte_6(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(Code::DeclareByte);
		instruction_internal::internal_set_declare_data_len(&mut instruction, 6);

		instruction.try_set_declare_byte_value(0, b0)?;
		instruction.try_set_declare_byte_value(1, b1)?;
		instruction.try_set_declare_byte_value(2, b2)?;
		instruction.try_set_declare_byte_value(3, b3)?;
		instruction.try_set_declare_byte_value(4, b4)?;
		instruction.try_set_declare_byte_value(5, b5)?;

		debug_assert_eq!(instruction.op_count(), 0);
		Ok(instruction)
	}

	/// Creates a `db`/`.byte` asm directive
	///
	/// # Arguments
	///
	/// * `b0`: Byte 0
	/// * `b1`: Byte 1
	/// * `b2`: Byte 2
	/// * `b3`: Byte 3
	/// * `b4`: Byte 4
	/// * `b5`: Byte 5
	#[allow(clippy::unwrap_used)]
	#[must_use]
	#[inline]
	#[rustfmt::skip]
	pub fn with_declare_byte_6(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8) -> Self {
		Instruction::try_with_declare_byte_6(b0, b1, b2, b3, b4, b5).unwrap()
	}

	#[doc(hidden)]
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	pub fn try_with_declare_byte_7(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(Code::DeclareByte);
		instruction_internal::internal_set_declare_data_len(&mut instruction, 7);

		instruction.try_set_declare_byte_value(0, b0)?;
		instruction.try_set_declare_byte_value(1, b1)?;
		instruction.try_set_declare_byte_value(2, b2)?;
		instruction.try_set_declare_byte_value(3, b3)?;
		instruction.try_set_declare_byte_value(4, b4)?;
		instruction.try_set_declare_byte_value(5, b5)?;
		instruction.try_set_declare_byte_value(6, b6)?;

		debug_assert_eq!(instruction.op_count(), 0);
		Ok(instruction)
	}

	/// Creates a `db`/`.byte` asm directive
	///
	/// # Arguments
	///
	/// * `b0`: Byte 0
	/// * `b1`: Byte 1
	/// * `b2`: Byte 2
	/// * `b3`: Byte 3
	/// * `b4`: Byte 4
	/// * `b5`: Byte 5
	/// * `b6`: Byte 6
	#[allow(clippy::unwrap_used)]
	#[must_use]
	#[inline]
	#[rustfmt::skip]
	pub fn with_declare_byte_7(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8) -> Self {
		Instruction::try_with_declare_byte_7(b0, b1, b2, b3, b4, b5, b6).unwrap()
	}

	#[doc(hidden)]
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	pub fn try_with_declare_byte_8(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8, b7: u8) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(Code::DeclareByte);
		instruction_internal::internal_set_declare_data_len(&mut instruction, 8);

		instruction.try_set_declare_byte_value(0, b0)?;
		instruction.try_set_declare_byte_value(1, b1)?;
		instruction.try_set_declare_byte_value(2, b2)?;
		instruction.try_set_declare_byte_value(3, b3)?;
		instruction.try_set_declare_byte_value(4, b4)?;
		instruction.try_set_declare_byte_value(5, b5)?;
		instruction.try_set_declare_byte_value(6, b6)?;
		instruction.try_set_declare_byte_value(7, b7)?;

		debug_assert_eq!(instruction.op_count(), 0);
		Ok(instruction)
	}

	/// Creates a `db`/`.byte` asm directive
	///
	/// # Arguments
	///
	/// * `b0`: Byte 0
	/// * `b1`: Byte 1
	/// * `b2`: Byte 2
	/// * `b3`: Byte 3
	/// * `b4`: Byte 4
	/// * `b5`: Byte 5
	/// * `b6`: Byte 6
	/// * `b7`: Byte 7
	#[allow(clippy::unwrap_used)]
	#[must_use]
	#[inline]
	#[rustfmt::skip]
	pub fn with_declare_byte_8(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8, b7: u8) -> Self {
		Instruction::try_with_declare_byte_8(b0, b1, b2, b3, b4, b5, b6, b7).unwrap()
	}

	#[doc(hidden)]
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	pub fn try_with_declare_byte_9(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8, b7: u8, b8: u8) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(Code::DeclareByte);
		instruction_internal::internal_set_declare_data_len(&mut instruction, 9);

		instruction.try_set_declare_byte_value(0, b0)?;
		instruction.try_set_declare_byte_value(1, b1)?;
		instruction.try_set_declare_byte_value(2, b2)?;
		instruction.try_set_declare_byte_value(3, b3)?;
		instruction.try_set_declare_byte_value(4, b4)?;
		instruction.try_set_declare_byte_value(5, b5)?;
		instruction.try_set_declare_byte_value(6, b6)?;
		instruction.try_set_declare_byte_value(7, b7)?;
		instruction.try_set_declare_byte_value(8, b8)?;

		debug_assert_eq!(instruction.op_count(), 0);
		Ok(instruction)
	}

	/// Creates a `db`/`.byte` asm directive
	///
	/// # Arguments
	///
	/// * `b0`: Byte 0
	/// * `b1`: Byte 1
	/// * `b2`: Byte 2
	/// * `b3`: Byte 3
	/// * `b4`: Byte 4
	/// * `b5`: Byte 5
	/// * `b6`: Byte 6
	/// * `b7`: Byte 7
	/// * `b8`: Byte 8
	#[allow(clippy::unwrap_used)]
	#[must_use]
	#[inline]
	#[rustfmt::skip]
	pub fn with_declare_byte_9(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8, b7: u8, b8: u8) -> Self {
		Instruction::try_with_declare_byte_9(b0, b1, b2, b3, b4, b5, b6, b7, b8).unwrap()
	}

	#[doc(hidden)]
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	pub fn try_with_declare_byte_10(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8, b7: u8, b8: u8, b9: u8) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(Code::DeclareByte);
		instruction_internal::internal_set_declare_data_len(&mut instruction, 10);

		instruction.try_set_declare_byte_value(0, b0)?;
		instruction.try_set_declare_byte_value(1, b1)?;
		instruction.try_set_declare_byte_value(2, b2)?;
		instruction.try_set_declare_byte_value(3, b3)?;
		instruction.try_set_declare_byte_value(4, b4)?;
		instruction.try_set_declare_byte_value(5, b5)?;
		instruction.try_set_declare_byte_value(6, b6)?;
		instruction.try_set_declare_byte_value(7, b7)?;
		instruction.try_set_declare_byte_value(8, b8)?;
		instruction.try_set_declare_byte_value(9, b9)?;

		debug_assert_eq!(instruction.op_count(), 0);
		Ok(instruction)
	}

	/// Creates a `db`/`.byte` asm directive
	///
	/// # Arguments
	///
	/// * `b0`: Byte 0
	/// * `b1`: Byte 1
	/// * `b2`: Byte 2
	/// * `b3`: Byte 3
	/// * `b4`: Byte 4
	/// * `b5`: Byte 5
	/// * `b6`: Byte 6
	/// * `b7`: Byte 7
	/// * `b8`: Byte 8
	/// * `b9`: Byte 9
	#[allow(clippy::unwrap_used)]
	#[must_use]
	#[inline]
	#[rustfmt::skip]
	pub fn with_declare_byte_10(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8, b7: u8, b8: u8, b9: u8) -> Self {
		Instruction::try_with_declare_byte_10(b0, b1, b2, b3, b4, b5, b6, b7, b8, b9).unwrap()
	}

	#[doc(hidden)]
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	pub fn try_with_declare_byte_11(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8, b7: u8, b8: u8, b9: u8, b10: u8) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(Code::DeclareByte);
		instruction_internal::internal_set_declare_data_len(&mut instruction, 11);

		instruction.try_set_declare_byte_value(0, b0)?;
		instruction.try_set_declare_byte_value(1, b1)?;
		instruction.try_set_declare_byte_value(2, b2)?;
		instruction.try_set_declare_byte_value(3, b3)?;
		instruction.try_set_declare_byte_value(4, b4)?;
		instruction.try_set_declare_byte_value(5, b5)?;
		instruction.try_set_declare_byte_value(6, b6)?;
		instruction.try_set_declare_byte_value(7, b7)?;
		instruction.try_set_declare_byte_value(8, b8)?;
		instruction.try_set_declare_byte_value(9, b9)?;
		instruction.try_set_declare_byte_value(10, b10)?;

		debug_assert_eq!(instruction.op_count(), 0);
		Ok(instruction)
	}

	/// Creates a `db`/`.byte` asm directive
	///
	/// # Arguments
	///
	/// * `b0`: Byte 0
	/// * `b1`: Byte 1
	/// * `b2`: Byte 2
	/// * `b3`: Byte 3
	/// * `b4`: Byte 4
	/// * `b5`: Byte 5
	/// * `b6`: Byte 6
	/// * `b7`: Byte 7
	/// * `b8`: Byte 8
	/// * `b9`: Byte 9
	/// * `b10`: Byte 10
	#[allow(clippy::unwrap_used)]
	#[must_use]
	#[inline]
	#[rustfmt::skip]
	pub fn with_declare_byte_11(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8, b7: u8, b8: u8, b9: u8, b10: u8) -> Self {
		Instruction::try_with_declare_byte_11(b0, b1, b2, b3, b4, b5, b6, b7, b8, b9, b10).unwrap()
	}

	#[doc(hidden)]
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	pub fn try_with_declare_byte_12(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8, b7: u8, b8: u8, b9: u8, b10: u8, b11: u8) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(Code::DeclareByte);
		instruction_internal::internal_set_declare_data_len(&mut instruction, 12);

		instruction.try_set_declare_byte_value(0, b0)?;
		instruction.try_set_declare_byte_value(1, b1)?;
		instruction.try_set_declare_byte_value(2, b2)?;
		instruction.try_set_declare_byte_value(3, b3)?;
		instruction.try_set_declare_byte_value(4, b4)?;
		instruction.try_set_declare_byte_value(5, b5)?;
		instruction.try_set_declare_byte_value(6, b6)?;
		instruction.try_set_declare_byte_value(7, b7)?;
		instruction.try_set_declare_byte_value(8, b8)?;
		instruction.try_set_declare_byte_value(9, b9)?;
		instruction.try_set_declare_byte_value(10, b10)?;
		instruction.try_set_declare_byte_value(11, b11)?;

		debug_assert_eq!(instruction.op_count(), 0);
		Ok(instruction)
	}

	/// Creates a `db`/`.byte` asm directive
	///
	/// # Arguments
	///
	/// * `b0`: Byte 0
	/// * `b1`: Byte 1
	/// * `b2`: Byte 2
	/// * `b3`: Byte 3
	/// * `b4`: Byte 4
	/// * `b5`: Byte 5
	/// * `b6`: Byte 6
	/// * `b7`: Byte 7
	/// * `b8`: Byte 8
	/// * `b9`: Byte 9
	/// * `b10`: Byte 10
	/// * `b11`: Byte 11
	#[allow(clippy::unwrap_used)]
	#[must_use]
	#[inline]
	#[rustfmt::skip]
	pub fn with_declare_byte_12(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8, b7: u8, b8: u8, b9: u8, b10: u8, b11: u8) -> Self {
		Instruction::try_with_declare_byte_12(b0, b1, b2, b3, b4, b5, b6, b7, b8, b9, b10, b11).unwrap()
	}

	#[doc(hidden)]
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	pub fn try_with_declare_byte_13(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8, b7: u8, b8: u8, b9: u8, b10: u8, b11: u8, b12: u8) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(Code::DeclareByte);
		instruction_internal::internal_set_declare_data_len(&mut instruction, 13);

		instruction.try_set_declare_byte_value(0, b0)?;
		instruction.try_set_declare_byte_value(1, b1)?;
		instruction.try_set_declare_byte_value(2, b2)?;
		instruction.try_set_declare_byte_value(3, b3)?;
		instruction.try_set_declare_byte_value(4, b4)?;
		instruction.try_set_declare_byte_value(5, b5)?;
		instruction.try_set_declare_byte_value(6, b6)?;
		instruction.try_set_declare_byte_value(7, b7)?;
		instruction.try_set_declare_byte_value(8, b8)?;
		instruction.try_set_declare_byte_value(9, b9)?;
		instruction.try_set_declare_byte_value(10, b10)?;
		instruction.try_set_declare_byte_value(11, b11)?;
		instruction.try_set_declare_byte_value(12, b12)?;

		debug_assert_eq!(instruction.op_count(), 0);
		Ok(instruction)
	}

	/// Creates a `db`/`.byte` asm directive
	///
	/// # Arguments
	///
	/// * `b0`: Byte 0
	/// * `b1`: Byte 1
	/// * `b2`: Byte 2
	/// * `b3`: Byte 3
	/// * `b4`: Byte 4
	/// * `b5`: Byte 5
	/// * `b6`: Byte 6
	/// * `b7`: Byte 7
	/// * `b8`: Byte 8
	/// * `b9`: Byte 9
	/// * `b10`: Byte 10
	/// * `b11`: Byte 11
	/// * `b12`: Byte 12
	#[allow(clippy::unwrap_used)]
	#[must_use]
	#[inline]
	#[rustfmt::skip]
	pub fn with_declare_byte_13(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8, b7: u8, b8: u8, b9: u8, b10: u8, b11: u8, b12: u8) -> Self {
		Instruction::try_with_declare_byte_13(b0, b1, b2, b3, b4, b5, b6, b7, b8, b9, b10, b11, b12).unwrap()
	}

	#[doc(hidden)]
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	pub fn try_with_declare_byte_14(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8, b7: u8, b8: u8, b9: u8, b10: u8, b11: u8, b12: u8, b13: u8) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(Code::DeclareByte);
		instruction_internal::internal_set_declare_data_len(&mut instruction, 14);

		instruction.try_set_declare_byte_value(0, b0)?;
		instruction.try_set_declare_byte_value(1, b1)?;
		instruction.try_set_declare_byte_value(2, b2)?;
		instruction.try_set_declare_byte_value(3, b3)?;
		instruction.try_set_declare_byte_value(4, b4)?;
		instruction.try_set_declare_byte_value(5, b5)?;
		instruction.try_set_declare_byte_value(6, b6)?;
		instruction.try_set_declare_byte_value(7, b7)?;
		instruction.try_set_declare_byte_value(8, b8)?;
		instruction.try_set_declare_byte_value(9, b9)?;
		instruction.try_set_declare_byte_value(10, b10)?;
		instruction.try_set_declare_byte_value(11, b11)?;
		instruction.try_set_declare_byte_value(12, b12)?;
		instruction.try_set_declare_byte_value(13, b13)?;

		debug_assert_eq!(instruction.op_count(), 0);
		Ok(instruction)
	}

	/// Creates a `db`/`.byte` asm directive
	///
	/// # Arguments
	///
	/// * `b0`: Byte 0
	/// * `b1`: Byte 1
	/// * `b2`: Byte 2
	/// * `b3`: Byte 3
	/// * `b4`: Byte 4
	/// * `b5`: Byte 5
	/// * `b6`: Byte 6
	/// * `b7`: Byte 7
	/// * `b8`: Byte 8
	/// * `b9`: Byte 9
	/// * `b10`: Byte 10
	/// * `b11`: Byte 11
	/// * `b12`: Byte 12
	/// * `b13`: Byte 13
	#[allow(clippy::unwrap_used)]
	#[must_use]
	#[inline]
	#[rustfmt::skip]
	pub fn with_declare_byte_14(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8, b7: u8, b8: u8, b9: u8, b10: u8, b11: u8, b12: u8, b13: u8) -> Self {
		Instruction::try_with_declare_byte_14(b0, b1, b2, b3, b4, b5, b6, b7, b8, b9, b10, b11, b12, b13).unwrap()
	}

	#[doc(hidden)]
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	pub fn try_with_declare_byte_15(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8, b7: u8, b8: u8, b9: u8, b10: u8, b11: u8, b12: u8, b13: u8, b14: u8) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(Code::DeclareByte);
		instruction_internal::internal_set_declare_data_len(&mut instruction, 15);

		instruction.try_set_declare_byte_value(0, b0)?;
		instruction.try_set_declare_byte_value(1, b1)?;
		instruction.try_set_declare_byte_value(2, b2)?;
		instruction.try_set_declare_byte_value(3, b3)?;
		instruction.try_set_declare_byte_value(4, b4)?;
		instruction.try_set_declare_byte_value(5, b5)?;
		instruction.try_set_declare_byte_value(6, b6)?;
		instruction.try_set_declare_byte_value(7, b7)?;
		instruction.try_set_declare_byte_value(8, b8)?;
		instruction.try_set_declare_byte_value(9, b9)?;
		instruction.try_set_declare_byte_value(10, b10)?;
		instruction.try_set_declare_byte_value(11, b11)?;
		instruction.try_set_declare_byte_value(12, b12)?;
		instruction.try_set_declare_byte_value(13, b13)?;
		instruction.try_set_declare_byte_value(14, b14)?;

		debug_assert_eq!(instruction.op_count(), 0);
		Ok(instruction)
	}

	/// Creates a `db`/`.byte` asm directive
	///
	/// # Arguments
	///
	/// * `b0`: Byte 0
	/// * `b1`: Byte 1
	/// * `b2`: Byte 2
	/// * `b3`: Byte 3
	/// * `b4`: Byte 4
	/// * `b5`: Byte 5
	/// * `b6`: Byte 6
	/// * `b7`: Byte 7
	/// * `b8`: Byte 8
	/// * `b9`: Byte 9
	/// * `b10`: Byte 10
	/// * `b11`: Byte 11
	/// * `b12`: Byte 12
	/// * `b13`: Byte 13
	/// * `b14`: Byte 14
	#[allow(clippy::unwrap_used)]
	#[must_use]
	#[inline]
	#[rustfmt::skip]
	pub fn with_declare_byte_15(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8, b7: u8, b8: u8, b9: u8, b10: u8, b11: u8, b12: u8, b13: u8, b14: u8) -> Self {
		Instruction::try_with_declare_byte_15(b0, b1, b2, b3, b4, b5, b6, b7, b8, b9, b10, b11, b12, b13, b14).unwrap()
	}

	#[doc(hidden)]
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	pub fn try_with_declare_byte_16(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8, b7: u8, b8: u8, b9: u8, b10: u8, b11: u8, b12: u8, b13: u8, b14: u8, b15: u8) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(Code::DeclareByte);
		instruction_internal::internal_set_declare_data_len(&mut instruction, 16);

		instruction.try_set_declare_byte_value(0, b0)?;
		instruction.try_set_declare_byte_value(1, b1)?;
		instruction.try_set_declare_byte_value(2, b2)?;
		instruction.try_set_declare_byte_value(3, b3)?;
		instruction.try_set_declare_byte_value(4, b4)?;
		instruction.try_set_declare_byte_value(5, b5)?;
		instruction.try_set_declare_byte_value(6, b6)?;
		instruction.try_set_declare_byte_value(7, b7)?;
		instruction.try_set_declare_byte_value(8, b8)?;
		instruction.try_set_declare_byte_value(9, b9)?;
		instruction.try_set_declare_byte_value(10, b10)?;
		instruction.try_set_declare_byte_value(11, b11)?;
		instruction.try_set_declare_byte_value(12, b12)?;
		instruction.try_set_declare_byte_value(13, b13)?;
		instruction.try_set_declare_byte_value(14, b14)?;
		instruction.try_set_declare_byte_value(15, b15)?;

		debug_assert_eq!(instruction.op_count(), 0);
		Ok(instruction)
	}

	/// Creates a `db`/`.byte` asm directive
	///
	/// # Arguments
	///
	/// * `b0`: Byte 0
	/// * `b1`: Byte 1
	/// * `b2`: Byte 2
	/// * `b3`: Byte 3
	/// * `b4`: Byte 4
	/// * `b5`: Byte 5
	/// * `b6`: Byte 6
	/// * `b7`: Byte 7
	/// * `b8`: Byte 8
	/// * `b9`: Byte 9
	/// * `b10`: Byte 10
	/// * `b11`: Byte 11
	/// * `b12`: Byte 12
	/// * `b13`: Byte 13
	/// * `b14`: Byte 14
	/// * `b15`: Byte 15
	#[allow(clippy::unwrap_used)]
	#[must_use]
	#[inline]
	#[rustfmt::skip]
	pub fn with_declare_byte_16(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8, b7: u8, b8: u8, b9: u8, b10: u8, b11: u8, b12: u8, b13: u8, b14: u8, b15: u8) -> Self {
		Instruction::try_with_declare_byte_16(b0, b1, b2, b3, b4, b5, b6, b7, b8, b9, b10, b11, b12, b13, b14, b15).unwrap()
	}

	/// Creates a `db`/`.byte` asm directive
	///
	/// # Errors
	///
	/// Fails if `data.len()` is not 1-16
	///
	/// # Arguments
	///
	/// * `data`: Data
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	pub fn with_declare_byte(data: &[u8]) -> Result<Self, IcedError> {
		if data.len().wrapping_sub(1) > 16 - 1 {
			return Err(IcedError::new("Invalid slice length"));
		}

		let mut instruction = Self::default();
		instruction.set_code(Code::DeclareByte);
		instruction_internal::internal_set_declare_data_len(&mut instruction, data.len() as u32);

		for i in data.iter().enumerate() {
			instruction.try_set_declare_byte_value(i.0, *i.1)?;
		}

		debug_assert_eq!(instruction.op_count(), 0);
		Ok(instruction)
	}

	#[doc(hidden)]
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	pub fn try_with_declare_word_1(w0: u16) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(Code::DeclareWord);
		instruction_internal::internal_set_declare_data_len(&mut instruction, 1);

		instruction.try_set_declare_word_value(0, w0)?;

		debug_assert_eq!(instruction.op_count(), 0);
		Ok(instruction)
	}

	/// Creates a `dw`/`.word` asm directive
	///
	/// # Arguments
	///
	/// * `w0`: Word 0
	#[allow(clippy::unwrap_used)]
	#[must_use]
	#[inline]
	#[rustfmt::skip]
	pub fn with_declare_word_1(w0: u16) -> Self {
		Instruction::try_with_declare_word_1(w0).unwrap()
	}

	#[doc(hidden)]
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	pub fn try_with_declare_word_2(w0: u16, w1: u16) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(Code::DeclareWord);
		instruction_internal::internal_set_declare_data_len(&mut instruction, 2);

		instruction.try_set_declare_word_value(0, w0)?;
		instruction.try_set_declare_word_value(1, w1)?;

		debug_assert_eq!(instruction.op_count(), 0);
		Ok(instruction)
	}

	/// Creates a `dw`/`.word` asm directive
	///
	/// # Arguments
	///
	/// * `w0`: Word 0
	/// * `w1`: Word 1
	#[allow(clippy::unwrap_used)]
	#[must_use]
	#[inline]
	#[rustfmt::skip]
	pub fn with_declare_word_2(w0: u16, w1: u16) -> Self {
		Instruction::try_with_declare_word_2(w0, w1).unwrap()
	}

	#[doc(hidden)]
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	pub fn try_with_declare_word_3(w0: u16, w1: u16, w2: u16) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(Code::DeclareWord);
		instruction_internal::internal_set_declare_data_len(&mut instruction, 3);

		instruction.try_set_declare_word_value(0, w0)?;
		instruction.try_set_declare_word_value(1, w1)?;
		instruction.try_set_declare_word_value(2, w2)?;

		debug_assert_eq!(instruction.op_count(), 0);
		Ok(instruction)
	}

	/// Creates a `dw`/`.word` asm directive
	///
	/// # Arguments
	///
	/// * `w0`: Word 0
	/// * `w1`: Word 1
	/// * `w2`: Word 2
	#[allow(clippy::unwrap_used)]
	#[must_use]
	#[inline]
	#[rustfmt::skip]
	pub fn with_declare_word_3(w0: u16, w1: u16, w2: u16) -> Self {
		Instruction::try_with_declare_word_3(w0, w1, w2).unwrap()
	}

	#[doc(hidden)]
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	pub fn try_with_declare_word_4(w0: u16, w1: u16, w2: u16, w3: u16) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(Code::DeclareWord);
		instruction_internal::internal_set_declare_data_len(&mut instruction, 4);

		instruction.try_set_declare_word_value(0, w0)?;
		instruction.try_set_declare_word_value(1, w1)?;
		instruction.try_set_declare_word_value(2, w2)?;
		instruction.try_set_declare_word_value(3, w3)?;

		debug_assert_eq!(instruction.op_count(), 0);
		Ok(instruction)
	}

	/// Creates a `dw`/`.word` asm directive
	///
	/// # Arguments
	///
	/// * `w0`: Word 0
	/// * `w1`: Word 1
	/// * `w2`: Word 2
	/// * `w3`: Word 3
	#[allow(clippy::unwrap_used)]
	#[must_use]
	#[inline]
	#[rustfmt::skip]
	pub fn with_declare_word_4(w0: u16, w1: u16, w2: u16, w3: u16) -> Self {
		Instruction::try_with_declare_word_4(w0, w1, w2, w3).unwrap()
	}

	#[doc(hidden)]
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	pub fn try_with_declare_word_5(w0: u16, w1: u16, w2: u16, w3: u16, w4: u16) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(Code::DeclareWord);
		instruction_internal::internal_set_declare_data_len(&mut instruction, 5);

		instruction.try_set_declare_word_value(0, w0)?;
		instruction.try_set_declare_word_value(1, w1)?;
		instruction.try_set_declare_word_value(2, w2)?;
		instruction.try_set_declare_word_value(3, w3)?;
		instruction.try_set_declare_word_value(4, w4)?;

		debug_assert_eq!(instruction.op_count(), 0);
		Ok(instruction)
	}

	/// Creates a `dw`/`.word` asm directive
	///
	/// # Arguments
	///
	/// * `w0`: Word 0
	/// * `w1`: Word 1
	/// * `w2`: Word 2
	/// * `w3`: Word 3
	/// * `w4`: Word 4
	#[allow(clippy::unwrap_used)]
	#[must_use]
	#[inline]
	#[rustfmt::skip]
	pub fn with_declare_word_5(w0: u16, w1: u16, w2: u16, w3: u16, w4: u16) -> Self {
		Instruction::try_with_declare_word_5(w0, w1, w2, w3, w4).unwrap()
	}

	#[doc(hidden)]
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	pub fn try_with_declare_word_6(w0: u16, w1: u16, w2: u16, w3: u16, w4: u16, w5: u16) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(Code::DeclareWord);
		instruction_internal::internal_set_declare_data_len(&mut instruction, 6);

		instruction.try_set_declare_word_value(0, w0)?;
		instruction.try_set_declare_word_value(1, w1)?;
		instruction.try_set_declare_word_value(2, w2)?;
		instruction.try_set_declare_word_value(3, w3)?;
		instruction.try_set_declare_word_value(4, w4)?;
		instruction.try_set_declare_word_value(5, w5)?;

		debug_assert_eq!(instruction.op_count(), 0);
		Ok(instruction)
	}

	/// Creates a `dw`/`.word` asm directive
	///
	/// # Arguments
	///
	/// * `w0`: Word 0
	/// * `w1`: Word 1
	/// * `w2`: Word 2
	/// * `w3`: Word 3
	/// * `w4`: Word 4
	/// * `w5`: Word 5
	#[allow(clippy::unwrap_used)]
	#[must_use]
	#[inline]
	#[rustfmt::skip]
	pub fn with_declare_word_6(w0: u16, w1: u16, w2: u16, w3: u16, w4: u16, w5: u16) -> Self {
		Instruction::try_with_declare_word_6(w0, w1, w2, w3, w4, w5).unwrap()
	}

	#[doc(hidden)]
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	pub fn try_with_declare_word_7(w0: u16, w1: u16, w2: u16, w3: u16, w4: u16, w5: u16, w6: u16) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(Code::DeclareWord);
		instruction_internal::internal_set_declare_data_len(&mut instruction, 7);

		instruction.try_set_declare_word_value(0, w0)?;
		instruction.try_set_declare_word_value(1, w1)?;
		instruction.try_set_declare_word_value(2, w2)?;
		instruction.try_set_declare_word_value(3, w3)?;
		instruction.try_set_declare_word_value(4, w4)?;
		instruction.try_set_declare_word_value(5, w5)?;
		instruction.try_set_declare_word_value(6, w6)?;

		debug_assert_eq!(instruction.op_count(), 0);
		Ok(instruction)
	}

	/// Creates a `dw`/`.word` asm directive
	///
	/// # Arguments
	///
	/// * `w0`: Word 0
	/// * `w1`: Word 1
	/// * `w2`: Word 2
	/// * `w3`: Word 3
	/// * `w4`: Word 4
	/// * `w5`: Word 5
	/// * `w6`: Word 6
	#[allow(clippy::unwrap_used)]
	#[must_use]
	#[inline]
	#[rustfmt::skip]
	pub fn with_declare_word_7(w0: u16, w1: u16, w2: u16, w3: u16, w4: u16, w5: u16, w6: u16) -> Self {
		Instruction::try_with_declare_word_7(w0, w1, w2, w3, w4, w5, w6).unwrap()
	}

	#[doc(hidden)]
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	pub fn try_with_declare_word_8(w0: u16, w1: u16, w2: u16, w3: u16, w4: u16, w5: u16, w6: u16, w7: u16) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(Code::DeclareWord);
		instruction_internal::internal_set_declare_data_len(&mut instruction, 8);

		instruction.try_set_declare_word_value(0, w0)?;
		instruction.try_set_declare_word_value(1, w1)?;
		instruction.try_set_declare_word_value(2, w2)?;
		instruction.try_set_declare_word_value(3, w3)?;
		instruction.try_set_declare_word_value(4, w4)?;
		instruction.try_set_declare_word_value(5, w5)?;
		instruction.try_set_declare_word_value(6, w6)?;
		instruction.try_set_declare_word_value(7, w7)?;

		debug_assert_eq!(instruction.op_count(), 0);
		Ok(instruction)
	}

	/// Creates a `dw`/`.word` asm directive
	///
	/// # Arguments
	///
	/// * `w0`: Word 0
	/// * `w1`: Word 1
	/// * `w2`: Word 2
	/// * `w3`: Word 3
	/// * `w4`: Word 4
	/// * `w5`: Word 5
	/// * `w6`: Word 6
	/// * `w7`: Word 7
	#[allow(clippy::unwrap_used)]
	#[must_use]
	#[inline]
	#[rustfmt::skip]
	pub fn with_declare_word_8(w0: u16, w1: u16, w2: u16, w3: u16, w4: u16, w5: u16, w6: u16, w7: u16) -> Self {
		Instruction::try_with_declare_word_8(w0, w1, w2, w3, w4, w5, w6, w7).unwrap()
	}

	/// Creates a `dw`/`.word` asm directive
	///
	/// # Errors
	///
	/// Fails if `data.len()` is not 2-16 or not a multiple of 2
	///
	/// # Arguments
	///
	/// * `data`: Data
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	#[allow(trivial_casts)]
	pub fn with_declare_word_slice_u8(data: &[u8]) -> Result<Self, IcedError> {
		if data.len().wrapping_sub(1) > 16 - 1 || (data.len() & 1) != 0 {
			return Err(IcedError::new("Invalid slice length"));
		}

		let mut instruction = Self::default();
		instruction.set_code(Code::DeclareWord);
		instruction_internal::internal_set_declare_data_len(&mut instruction, data.len() as u32 / 2);

		for i in 0..data.len() / 2 {
			let v = (data[i * 2] as u16) | ((data[i * 2 + 1] as u16) << 8);
			instruction.try_set_declare_word_value(i, v)?;
		}

		debug_assert_eq!(instruction.op_count(), 0);
		Ok(instruction)
	}

	/// Creates a `dw`/`.word` asm directive
	///
	/// # Errors
	///
	/// Fails if `data.len()` is not 1-8
	///
	/// # Arguments
	///
	/// * `data`: Data
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	pub fn with_declare_word(data: &[u16]) -> Result<Self, IcedError> {
		if data.len().wrapping_sub(1) > 8 - 1 {
			return Err(IcedError::new("Invalid slice length"));
		}

		let mut instruction = Self::default();
		instruction.set_code(Code::DeclareWord);
		instruction_internal::internal_set_declare_data_len(&mut instruction, data.len() as u32);

		for i in data.iter().enumerate() {
			instruction.try_set_declare_word_value(i.0, *i.1)?;
		}

		debug_assert_eq!(instruction.op_count(), 0);
		Ok(instruction)
	}

	#[doc(hidden)]
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	pub fn try_with_declare_dword_1(d0: u32) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(Code::DeclareDword);
		instruction_internal::internal_set_declare_data_len(&mut instruction, 1);

		instruction.try_set_declare_dword_value(0, d0)?;

		debug_assert_eq!(instruction.op_count(), 0);
		Ok(instruction)
	}

	/// Creates a `dd`/`.int` asm directive
	///
	/// # Arguments
	///
	/// * `d0`: Dword 0
	#[allow(clippy::unwrap_used)]
	#[must_use]
	#[inline]
	#[rustfmt::skip]
	pub fn with_declare_dword_1(d0: u32) -> Self {
		Instruction::try_with_declare_dword_1(d0).unwrap()
	}

	#[doc(hidden)]
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	pub fn try_with_declare_dword_2(d0: u32, d1: u32) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(Code::DeclareDword);
		instruction_internal::internal_set_declare_data_len(&mut instruction, 2);

		instruction.try_set_declare_dword_value(0, d0)?;
		instruction.try_set_declare_dword_value(1, d1)?;

		debug_assert_eq!(instruction.op_count(), 0);
		Ok(instruction)
	}

	/// Creates a `dd`/`.int` asm directive
	///
	/// # Arguments
	///
	/// * `d0`: Dword 0
	/// * `d1`: Dword 1
	#[allow(clippy::unwrap_used)]
	#[must_use]
	#[inline]
	#[rustfmt::skip]
	pub fn with_declare_dword_2(d0: u32, d1: u32) -> Self {
		Instruction::try_with_declare_dword_2(d0, d1).unwrap()
	}

	#[doc(hidden)]
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	pub fn try_with_declare_dword_3(d0: u32, d1: u32, d2: u32) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(Code::DeclareDword);
		instruction_internal::internal_set_declare_data_len(&mut instruction, 3);

		instruction.try_set_declare_dword_value(0, d0)?;
		instruction.try_set_declare_dword_value(1, d1)?;
		instruction.try_set_declare_dword_value(2, d2)?;

		debug_assert_eq!(instruction.op_count(), 0);
		Ok(instruction)
	}

	/// Creates a `dd`/`.int` asm directive
	///
	/// # Arguments
	///
	/// * `d0`: Dword 0
	/// * `d1`: Dword 1
	/// * `d2`: Dword 2
	#[allow(clippy::unwrap_used)]
	#[must_use]
	#[inline]
	#[rustfmt::skip]
	pub fn with_declare_dword_3(d0: u32, d1: u32, d2: u32) -> Self {
		Instruction::try_with_declare_dword_3(d0, d1, d2).unwrap()
	}

	#[doc(hidden)]
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	pub fn try_with_declare_dword_4(d0: u32, d1: u32, d2: u32, d3: u32) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(Code::DeclareDword);
		instruction_internal::internal_set_declare_data_len(&mut instruction, 4);

		instruction.try_set_declare_dword_value(0, d0)?;
		instruction.try_set_declare_dword_value(1, d1)?;
		instruction.try_set_declare_dword_value(2, d2)?;
		instruction.try_set_declare_dword_value(3, d3)?;

		debug_assert_eq!(instruction.op_count(), 0);
		Ok(instruction)
	}

	/// Creates a `dd`/`.int` asm directive
	///
	/// # Arguments
	///
	/// * `d0`: Dword 0
	/// * `d1`: Dword 1
	/// * `d2`: Dword 2
	/// * `d3`: Dword 3
	#[allow(clippy::unwrap_used)]
	#[must_use]
	#[inline]
	#[rustfmt::skip]
	pub fn with_declare_dword_4(d0: u32, d1: u32, d2: u32, d3: u32) -> Self {
		Instruction::try_with_declare_dword_4(d0, d1, d2, d3).unwrap()
	}

	/// Creates a `dd`/`.int` asm directive
	///
	/// # Errors
	///
	/// Fails if `data.len()` is not 4-16 or not a multiple of 4
	///
	/// # Arguments
	///
	/// * `data`: Data
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	#[allow(trivial_casts)]
	pub fn with_declare_dword_slice_u8(data: &[u8]) -> Result<Self, IcedError> {
		if data.len().wrapping_sub(1) > 16 - 1 || (data.len() & 3) != 0 {
			return Err(IcedError::new("Invalid slice length"));
		}

		let mut instruction = Self::default();
		instruction.set_code(Code::DeclareDword);
		instruction_internal::internal_set_declare_data_len(&mut instruction, data.len() as u32 / 4);

		for i in 0..data.len() / 4 {
			let v = (data[i * 4] as u32) | ((data[i * 4 + 1] as u32) << 8) | ((data[i * 4 + 2] as u32) << 16) | ((data[i * 4 + 3] as u32) << 24);
			instruction.try_set_declare_dword_value(i, v)?;
		}

		debug_assert_eq!(instruction.op_count(), 0);
		Ok(instruction)
	}

	/// Creates a `dd`/`.int` asm directive
	///
	/// # Errors
	///
	/// Fails if `data.len()` is not 1-4
	///
	/// # Arguments
	///
	/// * `data`: Data
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	pub fn with_declare_dword(data: &[u32]) -> Result<Self, IcedError> {
		if data.len().wrapping_sub(1) > 4 - 1 {
			return Err(IcedError::new("Invalid slice length"));
		}

		let mut instruction = Self::default();
		instruction.set_code(Code::DeclareDword);
		instruction_internal::internal_set_declare_data_len(&mut instruction, data.len() as u32);

		for i in data.iter().enumerate() {
			instruction.try_set_declare_dword_value(i.0, *i.1)?;
		}

		debug_assert_eq!(instruction.op_count(), 0);
		Ok(instruction)
	}

	#[doc(hidden)]
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	pub fn try_with_declare_qword_1(q0: u64) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(Code::DeclareQword);
		instruction_internal::internal_set_declare_data_len(&mut instruction, 1);

		instruction.try_set_declare_qword_value(0, q0)?;

		debug_assert_eq!(instruction.op_count(), 0);
		Ok(instruction)
	}

	/// Creates a `dq`/`.quad` asm directive
	///
	/// # Arguments
	///
	/// * `q0`: Qword 0
	#[allow(clippy::unwrap_used)]
	#[must_use]
	#[inline]
	#[rustfmt::skip]
	pub fn with_declare_qword_1(q0: u64) -> Self {
		Instruction::try_with_declare_qword_1(q0).unwrap()
	}

	#[doc(hidden)]
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	pub fn try_with_declare_qword_2(q0: u64, q1: u64) -> Result<Self, IcedError> {
		let mut instruction = Self::default();
		instruction.set_code(Code::DeclareQword);
		instruction_internal::internal_set_declare_data_len(&mut instruction, 2);

		instruction.try_set_declare_qword_value(0, q0)?;
		instruction.try_set_declare_qword_value(1, q1)?;

		debug_assert_eq!(instruction.op_count(), 0);
		Ok(instruction)
	}

	/// Creates a `dq`/`.quad` asm directive
	///
	/// # Arguments
	///
	/// * `q0`: Qword 0
	/// * `q1`: Qword 1
	#[allow(clippy::unwrap_used)]
	#[must_use]
	#[inline]
	#[rustfmt::skip]
	pub fn with_declare_qword_2(q0: u64, q1: u64) -> Self {
		Instruction::try_with_declare_qword_2(q0, q1).unwrap()
	}

	/// Creates a `dq`/`.quad` asm directive
	///
	/// # Errors
	///
	/// Fails if `data.len()` is not 8-16 or not a multiple of 8
	///
	/// # Arguments
	///
	/// * `data`: Data
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	#[allow(trivial_casts)]
	pub fn with_declare_qword_slice_u8(data: &[u8]) -> Result<Self, IcedError> {
		if data.len().wrapping_sub(1) > 16 - 1 || (data.len() & 7) != 0 {
			return Err(IcedError::new("Invalid slice length"));
		}

		let mut instruction = Self::default();
		instruction.set_code(Code::DeclareQword);
		instruction_internal::internal_set_declare_data_len(&mut instruction, data.len() as u32 / 8);

		for i in 0..data.len() / 8 {
			let v = (data[i * 8] as u64) | ((data[i * 8 + 1] as u64) << 8) | ((data[i * 8 + 2] as u64) << 16) | ((data[i * 8 + 3] as u64) << 24)
				| ((data[i * 8 + 4] as u64) << 32) | ((data[i * 8 + 5] as u64) << 40) | ((data[i * 8 + 6] as u64) << 48) | ((data[i * 8 + 7] as u64) << 56);
			instruction.try_set_declare_qword_value(i, v)?;
		}

		debug_assert_eq!(instruction.op_count(), 0);
		Ok(instruction)
	}

	/// Creates a `dq`/`.quad` asm directive
	///
	/// # Errors
	///
	/// Fails if `data.len()` is not 1-2
	///
	/// # Arguments
	///
	/// * `data`: Data
	#[allow(clippy::missing_inline_in_public_items)]
	#[rustfmt::skip]
	pub fn with_declare_qword(data: &[u64]) -> Result<Self, IcedError> {
		if data.len().wrapping_sub(1) > 2 - 1 {
			return Err(IcedError::new("Invalid slice length"));
		}

		let mut instruction = Self::default();
		instruction.set_code(Code::DeclareQword);
		instruction_internal::internal_set_declare_data_len(&mut instruction, data.len() as u32);

		for i in data.iter().enumerate() {
			instruction.try_set_declare_qword_value(i.0, *i.1)?;
		}

		debug_assert_eq!(instruction.op_count(), 0);
		Ok(instruction)
	}
}
// GENERATOR-END: Create
