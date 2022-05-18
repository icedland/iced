// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::enum_utils::{to_code, to_code_size, to_mvex_reg_mem_conv, to_op_kind, to_register, to_rounding_control};
use crate::fpui::FpuStackIncrementInfo;
use crate::opci::OpCodeInfo;
use libc::c_int;
use loona::lua_api::lua_CFunction;
use loona::prelude::*;

lua_struct_module! { luaopen_iced_x86_Instruction : Instruction }
lua_impl_userdata! { Instruction }

/// A 16/32/64-bit x86 instruction. Created by `Decoder` or by `Instruction:create*()` methods
/// @class Instruction
#[derive(Clone, Copy)]
pub(crate) struct Instruction {
	pub(crate) inner: iced_x86::Instruction,
}

impl Instruction {
	pub(crate) unsafe fn push_new<'lua>(lua: &Lua<'lua>) -> &'lua mut Instruction {
		unsafe {
			let instr = Instruction { inner: iced_x86::Instruction::new() };
			Self::init_and_push(lua, &instr)
		}
	}

	pub(crate) unsafe fn init_and_push<'lua>(lua: &Lua<'lua>, instr: &Instruction) -> &'lua mut Instruction {
		unsafe {
			let instr = lua.push_user_data_copy(instr);

			lua_get_or_init_metatable!(Instruction: lua);
			let _ = lua.set_metatable(-2);
			instr
		}
	}

	unsafe fn init_metatable(lua: &Lua<'_>) {
		unsafe {
			lua.push("__index");
			lua.new_table();

			for &(name, method) in INSTRUCTION_EXPORTS {
				lua.push(name);
				lua.push(method);
				lua.raw_set(-3);
			}

			// Write to __index
			lua.raw_set(-3);

			#[rustfmt::skip]
			let special_methods: &[(&str, lua_CFunction)] = &[
				("__tostring", instruction_tostring),
				("__eq", instruction_eq),
				("__len", instruction_len),
			];
			for &(name, method) in special_methods {
				lua.push(name);
				lua.push(method);
				lua.raw_set(-3);
			}
		}
	}
}

lua_pub_methods! { static INSTRUCTION_EXPORTS =>
	/// Creates a new empty instruction
	/// @return Instruction
	unsafe fn new(lua) -> 1 {
		unsafe {
			let _ = Instruction::push_new(lua);
		}
	}

	/// Creates a new instruction that's exactly identical to this one
	/// @return Instruction
	unsafe fn copy(lua, instr: &Instruction) -> 1 {
		unsafe {
			let _ = Instruction::init_and_push(lua, instr);
		}
	}

	/// Checks if two instructions are equal, comparing all bits, not ignoring anything. `==` ignores some fields.
	unsafe fn eq_all_bits(lua, instr: &Instruction, instr2: &Instruction) -> 1 {
		unsafe {
			lua.push(instr.inner.eq_all_bits(&instr2.inner))
		}
	}

	/// Gets the 16-bit IP of the instruction
	/// @return integer
	unsafe fn ip16(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.ip16()); }
	}

	/// Gets the 16-bit IP of the instruction
	unsafe fn set_ip16(lua, instr: &mut Instruction, new_value: u16) -> 0 {
		instr.inner.set_ip16(new_value);
	}

	/// Gets the 32-bit IP of the instruction
	/// @return integer
	unsafe fn ip32(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.ip32()); }
	}

	/// Gets the 32-bit IP of the instruction
	unsafe fn set_ip32(lua, instr: &mut Instruction, new_value: u32) -> 0 {
		instr.inner.set_ip32(new_value);
	}

	/// Gets the 64-bit IP of the instruction
	/// @return integer
	unsafe fn ip(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.ip()); }
	}

	/// Gets the 64-bit IP of the instruction
	unsafe fn set_ip(lua, instr: &mut Instruction, new_value: u64) -> 0 {
		instr.inner.set_ip(new_value);
	}

	/// Gets the 16-bit IP of the next instruction
	/// @return integer
	unsafe fn next_ip16(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.next_ip16()); }
	}

	/// Gets the 16-bit IP of the next instruction
	unsafe fn set_next_ip16(lua, instr: &mut Instruction, new_value: u16) -> 0 {
		instr.inner.set_next_ip16(new_value);
	}

	/// Gets the 32-bit IP of the next instruction
	/// @return integer
	unsafe fn next_ip32(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.next_ip32()); }
	}

	/// Gets the 32-bit IP of the next instruction
	unsafe fn set_next_ip32(lua, instr: &mut Instruction, new_value: u32) -> 0 {
		instr.inner.set_next_ip32(new_value);
	}

	/// Gets the 64-bit IP of the next instruction
	/// @return integer
	unsafe fn next_ip(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.next_ip()); }
	}

	/// Gets the 64-bit IP of the next instruction
	unsafe fn set_next_ip(lua, instr: &mut Instruction, new_value: u64) -> 0 {
		instr.inner.set_next_ip(new_value);
	}

	/// Gets the code size (a `CodeSize` enum value) when the instruction was decoded.
	///
	/// # Note
	/// This value is informational and can be used by a formatter.
	///
	/// @return integer # A `CodeSize` enum value
	unsafe fn code_size(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.code_size() as u32); }
	}

	/// Gets the code size (a `CodeSize` enum value) when the instruction was decoded.
	unsafe fn set_code_size(lua, instr: &mut Instruction, new_value: u32) -> 0 {
		instr.inner.set_code_size(unsafe { to_code_size(lua, new_value) });
	}

	/// Checks if it's an invalid instruction (`Instruction:code()` == `Code.INVALID`)
	/// @return boolean
	unsafe fn is_invalid(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.is_invalid()); }
	}

	/// Gets the instruction code (a `Code` enum value), see also `Instruction:mnemonic()`
	/// @return integer # A `Code` enum value
	unsafe fn code(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.code() as u32); }
	}

	/// Gets the instruction code (a `Code` enum value), see also `Instruction:mnemonic()`
	unsafe fn set_code(lua, instr: &mut Instruction, new_value: u32) -> 0 {
		instr.inner.set_code(unsafe { to_code(lua, new_value) });
	}

	/// Gets the mnemonic (a `Mnemonic` enum value), see also `Instruction:code()`
	/// @return integer # A `Mnemonic` enum value
	unsafe fn mnemonic(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.mnemonic() as u32); }
	}

	/// Gets the operand count. An instruction can have 0-5 operands.
	/// @return int
	///
	/// # Examples
	/// ```lua
	/// from iced_x86 import *
	///
	/// # add [rax],ebx
	/// data = b"\x01\x18"
	/// decoder = Decoder(64, data)
	/// instr = decoder.decode()
	///
	/// assert instr.op_count == 2
	/// ```
	unsafe fn op_count(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.op_count()); }
	}

	/// Gets the length of the instruction, 0-15 bytes.
	///
	/// You can also call `#instr` to get this value.
	///
	/// # Note
	/// This is just informational. If you modify the instruction or create a new one, this method could return the wrong value.
	///
	/// @return integer
	unsafe fn len(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.len()); }
	}

	/// Gets the length of the instruction, 0-15 bytes.
	unsafe fn set_len(lua, instr: &mut Instruction, new_value: usize) -> 0 {
		instr.inner.set_len(new_value);
	}

	/// `true` if the instruction has the `XACQUIRE` prefix (`F2`)
	/// @return boolean
	unsafe fn has_xacquire_prefix(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.has_xacquire_prefix()); }
	}

	/// `true` if the instruction has the `XACQUIRE` prefix (`F2`)
	unsafe fn set_has_xacquire_prefix(lua, instr: &mut Instruction, new_value: bool) -> 0 {
		instr.inner.set_has_xacquire_prefix(new_value);
	}

	/// `true` if the instruction has the `XRELEASE` prefix (`F3`)
	/// @return boolean
	unsafe fn has_xrelease_prefix(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.has_xrelease_prefix()); }
	}

	/// `true` if the instruction has the `XRELEASE` prefix (`F3`)
	unsafe fn set_has_xrelease_prefix(lua, instr: &mut Instruction, new_value: bool) -> 0 {
		instr.inner.set_has_xrelease_prefix(new_value);
	}

	/// `true` if the instruction has the `REPE` or `REP` prefix (`F3`)
	/// @return boolean
	unsafe fn has_rep_prefix(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.has_rep_prefix()); }
	}

	/// `true` if the instruction has the `REPE` or `REP` prefix (`F3`)
	unsafe fn set_has_rep_prefix(lua, instr: &mut Instruction, new_value: bool) -> 0 {
		instr.inner.set_has_rep_prefix(new_value);
	}

	/// `true` if the instruction has the `REPE` or `REP` prefix (`F3`)
	/// @return boolean
	unsafe fn has_repe_prefix(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.has_repe_prefix()); }
	}

	/// `true` if the instruction has the `REPE` or `REP` prefix (`F3`)
	unsafe fn set_has_repe_prefix(lua, instr: &mut Instruction, new_value: bool) -> 0 {
		instr.inner.set_has_repe_prefix(new_value);
	}

	/// `true` if the instruction has the `REPNE` prefix (`F2`)
	/// @return boolean
	unsafe fn has_repne_prefix(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.has_repne_prefix()); }
	}

	/// `true` if the instruction has the `REPNE` prefix (`F2`)
	unsafe fn set_has_repne_prefix(lua, instr: &mut Instruction, new_value: bool) -> 0 {
		instr.inner.set_has_repne_prefix(new_value);
	}

	/// `true` if the instruction has the `LOCK` prefix (`F0`)
	/// @return boolean
	unsafe fn has_lock_prefix(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.has_lock_prefix()); }
	}

	/// `true` if the instruction has the `LOCK` prefix (`F0`)
	unsafe fn set_has_lock_prefix(lua, instr: &mut Instruction, new_value: bool) -> 0 {
		instr.inner.set_has_lock_prefix(new_value);
	}

	/// Gets operand #0's kind (an `OpKind` enum value) if the operand exists (see `Instruction:op_count()` and `Instruction:op_kind()`)
	/// @return integer # An `OpKind` enum value
	unsafe fn op0_kind(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.op0_kind() as u32); }
	}

	/// Gets operand #0's kind (an `OpKind` enum value) if the operand exists (see `Instruction:op_count()` and `Instruction:op_kind()`)
	unsafe fn set_op0_kind(lua, instr: &mut Instruction, new_value: u32) -> 0 {
		instr.inner.set_op0_kind(unsafe { to_op_kind(lua, new_value) });
	}

	/// Gets operand #1's kind (an `OpKind` enum value) if the operand exists (see `Instruction:op_count()` and `Instruction:op_kind()`)
	/// @return integer # An `OpKind` enum value
	unsafe fn op1_kind(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.op1_kind() as u32); }
	}

	/// Gets operand #1's kind (an `OpKind` enum value) if the operand exists (see `Instruction:op_count()` and `Instruction:op_kind()`)
	unsafe fn set_op1_kind(lua, instr: &mut Instruction, new_value: u32) -> 0 {
		instr.inner.set_op1_kind(unsafe { to_op_kind(lua, new_value) });
	}

	/// Gets operand #2's kind (an `OpKind` enum value) if the operand exists (see `Instruction:op_count()` and `Instruction:op_kind()`)
	/// @return integer # An `OpKind` enum value
	unsafe fn op2_kind(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.op2_kind() as u32); }
	}

	/// Gets operand #2's kind (an `OpKind` enum value) if the operand exists (see `Instruction:op_count()` and `Instruction:op_kind()`)
	unsafe fn set_op2_kind(lua, instr: &mut Instruction, new_value: u32) -> 0 {
		instr.inner.set_op2_kind(unsafe { to_op_kind(lua, new_value) });
	}

	/// Gets operand #3's kind (an `OpKind` enum value) if the operand exists (see `Instruction:op_count()` and `Instruction:op_kind()`)
	/// @return integer # An `OpKind` enum value
	unsafe fn op3_kind(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.op3_kind() as u32); }
	}

	/// Gets operand #3's kind (an `OpKind` enum value) if the operand exists (see `Instruction:op_count()` and `Instruction:op_kind()`)
	unsafe fn set_op3_kind(lua, instr: &mut Instruction, new_value: u32) -> 0 {
		instr.inner.set_op3_kind(unsafe { to_op_kind(lua, new_value) });
	}

	/// Gets operand #4's kind (an `OpKind` enum value) if the operand exists (see `Instruction:op_count()` and `Instruction:op_kind()`)
	/// @return integer # An `OpKind` enum value
	unsafe fn op4_kind(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.op4_kind() as u32); }
	}

	/// Gets operand #4's kind (an `OpKind` enum value) if the operand exists (see `Instruction:op_count()` and `Instruction:op_kind()`)
	unsafe fn set_op4_kind(lua, instr: &mut Instruction, new_value: u32) -> 0 {
		match instr.inner.try_set_op4_kind(unsafe { to_op_kind(lua, new_value) }) {
			Ok(()) => {},
			Err(e) => unsafe { lua.throw_error(e) },
		}
	}

	/// Gets an operand's kind (an `OpKind` enum value) if it exists (see `Instruction:op_count()`)
	///
	/// @param operand integer # Operand number, 0-4
	/// @return integer # An `OpKind` enum value
	///
	/// # Examples
	/// ```lua
	/// from iced_x86 import *
	///
	/// # add [rax],ebx
	/// data = b"\x01\x18"
	/// decoder = Decoder(64, data)
	/// instr = decoder.decode()
	///
	/// assert instr.op_count == 2
	/// assert instr.op_kind(0) == OpKind.Memory
	/// assert instr.memory_base == Register.RAX
	/// assert instr.memory_index == Register.NONE
	/// assert instr.op_kind(1) == OpKind.Register
	/// assert instr.op_register(1) == Register.EBX
	/// ```
	unsafe fn op_kind(lua, instr: &Instruction, operand: u32) -> 1 {
		match instr.inner.try_op_kind(operand) {
			Ok(op_kind) => unsafe { lua.push(op_kind as u32) },
			Err(e) => unsafe { lua.throw_error(e) },
		}
	}

	/// Sets an operand's kind
	///
	/// @param operand integer # Operand number, 0-4
	/// @param op_kind integer # Operand kind (An `OpKind` enum value)
	unsafe fn set_op_kind(lua, instr: &mut Instruction, operand: u32, op_kind: u32) -> 0 {
		match instr.inner.try_set_op_kind(operand, unsafe { to_op_kind(lua, op_kind) }) {
			Ok(()) => {},
			Err(e) => unsafe { lua.throw_error(e) },
		}
	}

	/// Checks if the instruction has a segment override prefix, see `Instruction:segment_prefix()`
	/// @return boolean
	unsafe fn has_segment_prefix(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.has_segment_prefix()); }
	}

	/// Gets the segment override prefix (a `Register` enum value) or `Register.None` if none.
	///
	/// See also `Instruction:memory_segment()`.
	///
	/// Use this method if the operand has kind `OpKind.Memory`,
	/// `OpKind.MemorySegSI`, `OpKind.MemorySegESI`, `OpKind.MemorySegRSI`
	///
	/// @return integer # A `Register` enum value
	unsafe fn segment_prefix(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.segment_prefix() as u32); }
	}

	/// Gets the segment override prefix (a `Register` enum value) or `Register.None` if none.
	unsafe fn set_segment_prefix(lua, instr: &mut Instruction, new_value: u32) -> 0 {
		instr.inner.set_segment_prefix(unsafe { to_register(lua, new_value) });
	}

	/// Gets the effective segment register used to reference the memory location (a `Register` enum value).
	///
	/// Use this method if the operand has kind `OpKind.Memory`, `OpKind.MemorySegSI`, `OpKind.MemorySegESI`, `OpKind.MemorySegRSI`
	///
	/// @return integer # A `Register` enum value
	unsafe fn memory_segment(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.memory_segment() as u32); }
	}

	/// Gets the size of the memory displacement in bytes.
	///
	/// Valid values are `0`, `1` (16/32/64-bit), `2` (16-bit), `4` (32-bit), `8` (64-bit).
	///
	/// Note that the return value can be 1 and `Instruction:memory_displacement()` may still not fit in
	/// a signed byte if it's an EVEX/MVEX encoded instruction.
	///
	/// Use this method if the operand has kind `OpKind.Memory`
	///
	/// @return integer
	unsafe fn memory_displ_size(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.memory_displ_size()); }
	}

	/// Gets the size of the memory displacement in bytes.
	unsafe fn set_memory_displ_size(lua, instr: &mut Instruction, new_value: u32) -> 0 {
		instr.inner.set_memory_displ_size(new_value);
	}

	/// `true` if the data is broadcast (EVEX instructions only)
	/// @return boolean
	unsafe fn is_broadcast(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.is_broadcast()); }
	}

	/// `true` if the data is broadcast (EVEX instructions only)
	unsafe fn set_is_broadcast(lua, instr: &mut Instruction, new_value: bool) -> 0 {
		instr.inner.set_is_broadcast(new_value);
	}

	/// `true` if eviction hint bit is set (`{eh}`) (MVEX instructions only)
	/// @return boolean
	unsafe fn is_mvex_eviction_hint(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.is_mvex_eviction_hint()); }
	}

	/// `true` if eviction hint bit is set (`{eh}`) (MVEX instructions only)
	unsafe fn set_is_mvex_eviction_hint(lua, instr: &mut Instruction, new_value: bool) -> 0 {
		instr.inner.set_is_mvex_eviction_hint(new_value);
	}

	/// (MVEX) Register/memory operand conversion function (an `MvexRegMemConv` enum value)
	/// @return integer # An `MvexRegMemConv` enum value
	unsafe fn mvex_reg_mem_conv(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.mvex_reg_mem_conv() as u32); }
	}

	/// (MVEX) Register/memory operand conversion function (an `MvexRegMemConv` enum value)
	unsafe fn set_mvex_reg_mem_conv(lua, instr: &mut Instruction, new_value: u32) -> 0 {
		instr.inner.set_mvex_reg_mem_conv(unsafe { to_mvex_reg_mem_conv(lua, new_value) });
	}

	/// Gets the size of the memory location (a `MemorySize` enum value) that is referenced by the operand.
	///
	/// See also `Instruction:is_broadcast()`.
	///
	/// Use this method if the operand has kind `OpKind.Memory`,
	/// `OpKind.MemorySegSI`, `OpKind.MemorySegESI`, `OpKind.MemorySegRSI`,
	/// `OpKind.MemoryESDI`, `OpKind.MemoryESEDI`, `OpKind.MemoryESRDI`
	///
	/// @return integer # A `MemorySize` enum value
	unsafe fn memory_size(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.memory_size() as u32); }
	}

	/// Gets the index register scale value, valid values are `*1`, `*2`, `*4`, `*8`.
	///
	/// Use this method if the operand has kind `OpKind.Memory`
	///
	/// @return integer
	unsafe fn memory_index_scale(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.memory_index_scale()); }
	}

	/// Gets the index register scale value, valid values are `*1`, `*2`, `*4`, `*8`.
	unsafe fn set_memory_index_scale(lua, instr: &mut Instruction, new_value: u32) -> 0 {
		instr.inner.set_memory_index_scale(new_value);
	}

	/// Gets the memory operand's displacement or the 64-bit absolute address if it's
	/// an `EIP` or `RIP` relative memory operand.
	///
	/// Use this method if the operand has kind `OpKind.Memory`
	///
	/// @return integer
	unsafe fn memory_displacement(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.memory_displacement64()); }
	}

	/// Gets the memory operand's displacement or the 64-bit absolute address if it's
	/// an `EIP` or `RIP` relative memory operand.
	unsafe fn set_memory_displacement(lua, instr: &mut Instruction, new_value: u64) -> 0 {
		instr.inner.set_memory_displacement64(new_value);
	}

	/// Gets an operand's immediate value
	///
	/// @param operand integer # Operand number, 0-4
	/// @return integer # (`u64`) The immediate
	unsafe fn immediate(lua, instr: &Instruction, operand: u32) -> 1 {
		let value = match instr.inner.try_immediate(operand) {
			Ok(value) => value,
			Err(e) => unsafe { lua.throw_error(e) },
		};
		unsafe { lua.push(value); }
	}

	/// Sets an operand's immediate value
	///
	/// @param operand integer # Operand number, 0-4
	/// @param new_value integer # (`i32`) Immediate
	unsafe fn set_immediate_i32(lua, instr: &mut Instruction, operand: u32, new_value: i32) -> 0 {
		match instr.inner.try_set_immediate_i32(operand, new_value) {
			Ok(()) => {},
			Err(e) => unsafe { lua.throw_error(e) },
		}
	}

	/// Sets an operand's immediate value
	///
	/// @param operand integer # Operand number, 0-4
	/// @param new_value integer # (`u32`) Immediate
	unsafe fn set_immediate_u32(lua, instr: &mut Instruction, operand: u32, new_value: u32) -> 0 {
		match instr.inner.try_set_immediate_u32(operand, new_value) {
			Ok(()) => {},
			Err(e) => unsafe { lua.throw_error(e) },
		}
	}

	/// Sets an operand's immediate value
	///
	/// @param operand integer # Operand number, 0-4
	/// @param new_value integer # (`i64`) Immediate
	unsafe fn set_immediate_i64(lua, instr: &mut Instruction, operand: u32, new_value: i64) -> 0 {
		match instr.inner.try_set_immediate_i64(operand, new_value) {
			Ok(()) => {},
			Err(e) => unsafe { lua.throw_error(e) },
		}
	}

	/// Sets an operand's immediate value
	///
	/// @param operand integer # Operand number, 0-4
	/// @param new_value integer # (`u64`) Immediate
	unsafe fn set_immediate_u64(lua, instr: &mut Instruction, operand: u32, new_value: u64) -> 0 {
		match instr.inner.try_set_immediate_u64(operand, new_value) {
			Ok(()) => {},
			Err(e) => unsafe { lua.throw_error(e) },
		}
	}

	/// Gets the operand's immediate value.
	///
	/// Use this method if the operand has kind `OpKind.Immediate8`
	///
	/// @return integer
	unsafe fn immediate8(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.immediate8()); }
	}

	/// Gets the operand's immediate value.
	unsafe fn set_immediate8(lua, instr: &mut Instruction, new_value: u8) -> 0 {
		instr.inner.set_immediate8(new_value);
	}

	/// Gets the operand's immediate value.
	///
	/// Use this method if the operand has kind `OpKind.Immediate8_2nd`
	///
	/// @return integer
	unsafe fn immediate8_2nd(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.immediate8_2nd()); }
	}

	/// Gets the operand's immediate value.
	unsafe fn set_immediate8_2nd(lua, instr: &mut Instruction, new_value: u8) -> 0 {
		instr.inner.set_immediate8_2nd(new_value);
	}

	/// Gets the operand's immediate value.
	///
	/// Use this method if the operand has kind `OpKind.Immediate16`
	///
	/// @return integer
	unsafe fn immediate16(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.immediate16()); }
	}

	/// Gets the operand's immediate value.
	unsafe fn set_immediate16(lua, instr: &mut Instruction, new_value: u16) -> 0 {
		instr.inner.set_immediate16(new_value);
	}

	/// Gets the operand's immediate value.
	///
	/// Use this method if the operand has kind `OpKind.Immediate32`
	///
	/// @return integer
	unsafe fn immediate32(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.immediate32()); }
	}

	/// Gets the operand's immediate value.
	unsafe fn set_immediate32(lua, instr: &mut Instruction, new_value: u32) -> 0 {
		instr.inner.set_immediate32(new_value);
	}

	/// Gets the operand's immediate value.
	///
	/// Use this method if the operand has kind `OpKind.Immediate64`
	///
	/// @return integer
	unsafe fn immediate64(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.immediate64()); }
	}

	/// Gets the operand's immediate value.
	unsafe fn set_immediate64(lua, instr: &mut Instruction, new_value: u64) -> 0 {
		instr.inner.set_immediate64(new_value);
	}

	/// Gets the operand's immediate value.
	///
	/// Use this method if the operand has kind `OpKind.Immediate8To16`
	///
	/// @return integer
	unsafe fn immediate8to16(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.immediate8to16()); }
	}

	/// Gets the operand's immediate value.
	unsafe fn set_immediate8to16(lua, instr: &mut Instruction, new_value: i16) -> 0 {
		instr.inner.set_immediate8to16(new_value);
	}

	/// Gets the operand's immediate value.
	///
	/// Use this method if the operand has kind `OpKind.Immediate8To32`
	///
	/// @return integer
	unsafe fn immediate8to32(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.immediate8to32()); }
	}

	/// Gets the operand's immediate value.
	unsafe fn set_immediate8to32(lua, instr: &mut Instruction, new_value: i32) -> 0 {
		instr.inner.set_immediate8to32(new_value);
	}

	/// Gets the operand's immediate value.
	///
	/// Use this method if the operand has kind `OpKind.Immediate8To64`
	///
	/// @return integer
	unsafe fn immediate8to64(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.immediate8to64()); }
	}

	/// Gets the operand's immediate value.
	unsafe fn set_immediate8to64(lua, instr: &mut Instruction, new_value: i64) -> 0 {
		instr.inner.set_immediate8to64(new_value);
	}

	/// Gets the operand's immediate value.
	///
	/// Use this method if the operand has kind `OpKind.Immediate32To64`
	///
	/// @return integer
	unsafe fn immediate32to64(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.immediate32to64()); }
	}

	/// Gets the operand's immediate value.
	unsafe fn set_immediate32to64(lua, instr: &mut Instruction, new_value: i64) -> 0 {
		instr.inner.set_immediate32to64(new_value);
	}

	/// Gets the operand's branch target.
	///
	/// Use this method if the operand has kind `OpKind.NearBranch16`
	///
	/// @return integer
	unsafe fn near_branch16(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.near_branch16()); }
	}

	/// Gets the operand's branch target.
	unsafe fn set_near_branch16(lua, instr: &mut Instruction, new_value: u16) -> 0 {
		instr.inner.set_near_branch16(new_value);
	}

	/// Gets the operand's branch target.
	///
	/// Use this method if the operand has kind `OpKind.NearBranch32`
	///
	/// @return integer
	unsafe fn near_branch32(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.near_branch32()); }
	}

	/// Gets the operand's branch target.
	unsafe fn set_near_branch32(lua, instr: &mut Instruction, new_value: u32) -> 0 {
		instr.inner.set_near_branch32(new_value);
	}

	/// Gets the operand's branch target.
	///
	/// Use this method if the operand has kind `OpKind.NearBranch64`
	///
	/// @return integer
	unsafe fn near_branch64(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.near_branch64()); }
	}

	/// Gets the operand's branch target.
	unsafe fn set_near_branch64(lua, instr: &mut Instruction, new_value: u64) -> 0 {
		instr.inner.set_near_branch64(new_value);
	}

	/// Gets the near branch target if it's a `CALL`/`JMP`/`Jcc` near branch instruction
	///
	/// (i.e., if `Instruction:op0_kind()` is `OpKind.NearBranch16`, `OpKind.NearBranch32` or `OpKind.NearBranch64`)
	///
	/// @return integer
	unsafe fn near_branch_target(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.near_branch_target()); }
	}

	/// Gets the operand's branch target.
	///
	/// Use this method if the operand has kind `OpKind.FarBranch16`
	///
	/// @return integer
	unsafe fn far_branch16(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.far_branch16()); }
	}

	/// Gets the operand's branch target.
	unsafe fn set_far_branch16(lua, instr: &mut Instruction, new_value: u16) -> 0 {
		instr.inner.set_far_branch16(new_value);
	}

	/// Gets the operand's branch target.
	///
	/// Use this method if the operand has kind `OpKind.FarBranch32`
	///
	/// @return integer
	unsafe fn far_branch32(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.far_branch32()); }
	}

	/// Gets the operand's branch target.
	unsafe fn set_far_branch32(lua, instr: &mut Instruction, new_value: u32) -> 0 {
		instr.inner.set_far_branch32(new_value);
	}

	/// Gets the operand's branch target selector.
	///
	/// Use this method if the operand has kind `OpKind.FarBranch16` or `OpKind.FarBranch32`
	///
	/// @return integer
	unsafe fn far_branch_selector(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.far_branch_selector()); }
	}

	/// Gets the operand's branch target selector.
	unsafe fn set_far_branch_selector(lua, instr: &mut Instruction, new_value: u16) -> 0 {
		instr.inner.set_far_branch_selector(new_value);
	}

	/// Gets the memory operand's base register (a `Register` enum value) or `Register.None` if none.
	///
	/// Use this method if the operand has kind `OpKind.Memory`
	///
	/// @return integer # A `Register` enum value
	unsafe fn memory_base(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.memory_base() as u32); }
	}

	/// Gets the memory operand's base register (a `Register` enum value) or `Register.None` if none.
	unsafe fn set_memory_base(lua, instr: &mut Instruction, new_value: u32) -> 0 {
		instr.inner.set_memory_base(unsafe { to_register(lua, new_value) });
	}

	/// Gets the memory operand's index register (a `Register` enum value) or `Register.None` if none.
	///
	/// Use this method if the operand has kind `OpKind.Memory`
	///
	/// @return integer # A `Register` enum value
	unsafe fn memory_index(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.memory_index() as u32); }
	}

	/// Gets the memory operand's index register (a `Register` enum value) or `Register.None` if none.
	unsafe fn set_memory_index(lua, instr: &mut Instruction, new_value: u32) -> 0 {
		instr.inner.set_memory_index(unsafe { to_register(lua, new_value) });
	}

	/// Gets operand #0's register value (a `Register` enum value).
	///
	/// Use this method if operand #0 (`Instruction:op0_kind()`) has kind `OpKind.Register`, see `Instruction:op_count()` and `Instruction:op_register()`
	///
	/// @return integer # A `Register` enum value
	unsafe fn op0_register(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.op0_register() as u32); }
	}

	/// Gets operand #0's register value (a `Register` enum value).
	unsafe fn set_op0_register(lua, instr: &mut Instruction, new_value: u32) -> 0 {
		instr.inner.set_op0_register(unsafe { to_register(lua, new_value) });
	}

	/// Gets operand #1's register value (a `Register` enum value).
	///
	/// Use this method if operand #1 (`Instruction:op0_kind()`) has kind `OpKind.Register`, see `Instruction:op_count()` and `Instruction:op_register()`
	///
	/// @return integer # A `Register` enum value
	unsafe fn op1_register(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.op1_register() as u32); }
	}

	/// Gets operand #1's register value (a `Register` enum value).
	unsafe fn set_op1_register(lua, instr: &mut Instruction, new_value: u32) -> 0 {
		instr.inner.set_op1_register(unsafe { to_register(lua, new_value) });
	}

	/// Gets operand #2's register value (a `Register` enum value).
	///
	/// Use this method if operand #2 (`Instruction:op0_kind()`) has kind `OpKind.Register`, see `Instruction:op_count()` and `Instruction:op_register()`
	///
	/// @return integer # A `Register` enum value
	unsafe fn op2_register(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.op2_register() as u32); }
	}

	/// Gets operand #2's register value (a `Register` enum value).
	unsafe fn set_op2_register(lua, instr: &mut Instruction, new_value: u32) -> 0 {
		instr.inner.set_op2_register(unsafe { to_register(lua, new_value) });
	}

	/// Gets operand #3's register value (a `Register` enum value).
	///
	/// Use this method if operand #3 (`Instruction:op0_kind()`) has kind `OpKind.Register`, see `Instruction:op_count()` and `Instruction:op_register()`
	///
	/// @return integer # A `Register` enum value
	unsafe fn op3_register(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.op3_register() as u32); }
	}

	/// Gets operand #3's register value (a `Register` enum value).
	unsafe fn set_op3_register(lua, instr: &mut Instruction, new_value: u32) -> 0 {
		instr.inner.set_op3_register(unsafe { to_register(lua, new_value) });
	}

	/// Gets operand #4's register value (a `Register` enum value).
	///
	/// Use this method if operand #4 (`Instruction:op0_kind()`) has kind `OpKind.Register`, see `Instruction:op_count()` and `Instruction:op_register()`
	///
	/// @return integer # A `Register` enum value
	unsafe fn op4_register(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.op4_register() as u32); }
	}

	/// Gets operand #4's register value (a `Register` enum value).
	unsafe fn set_op4_register(lua, instr: &mut Instruction, new_value: u32) -> 0 {
		match instr.inner.try_set_op4_register(unsafe { to_register(lua, new_value) }) {
			Ok(()) => {},
			Err(e) => unsafe { lua.throw_error(e) },
		}
	}

	/// Gets the operand's register value (a `Register` enum value).
	///
	/// Use this method if the operand has kind `OpKind.Register`
	///
	/// @param operand integer # Operand number, 0-4
	/// @return integer # (A `Register` enum value) The operand's register value
	///
	/// # Examples
	/// ```lua
	/// from iced_x86 import *
	///
	/// # add [rax],ebx
	/// data = b"\x01\x18"
	/// decoder = Decoder(64, data)
	/// instr = decoder.decode()
	///
	/// assert instr.op_count == 2
	/// assert instr.op_kind(0) == OpKind.Memory
	/// assert instr.op_kind(1) == OpKind.Register
	/// assert instr.op_register(1) == Register.EBX
	/// ```
	unsafe fn op_register(lua, instr: &Instruction, operand: u32) -> 1 {
		match instr.inner.try_op_register(operand) {
			Ok(register) => unsafe { lua.push(register as u32) },
			Err(e) => unsafe { lua.throw_error(e) },
		}
	}

	/// Sets the operand's register value.
	///
	/// Use this method if the operand has kind `OpKind.Register`
	///
	/// @param operand integer # Operand number, 0-4
	/// @param new_value integer # New value (A `Register` enum value)
	unsafe fn set_op_register(lua, instr: &mut Instruction, operand: u32, new_value: u32) -> 0 {
		match instr.inner.try_set_op_register(operand, unsafe { to_register(lua, new_value) }) {
			Ok(()) => {},
			Err(e) => unsafe { lua.throw_error(e) },
		}
	}

	/// Gets the opmask register (`Register.K1` - `Register.K7`) or `Register.None` if none (a `Register` enum value)
	/// @return integer # A `Register` enum value
	unsafe fn op_mask(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.op_mask() as u32); }
	}

	/// Gets the opmask register (`Register.K1` - `Register.K7`) or `Register.None` if none (a `Register` enum value)
	unsafe fn set_op_mask(lua, instr: &mut Instruction, new_value: u32) -> 0 {
		instr.inner.set_op_mask(unsafe { to_register(lua, new_value) });
	}

	/// Checks if there's an opmask register (`Instruction:op_mask()`)
	/// @return boolean
	unsafe fn has_op_mask(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.has_op_mask()); }
	}

	/// `true` if zeroing-masking, `false` if merging-masking.
	///
	/// Only used by most EVEX encoded instructions that use opmask registers.
	///
	/// @return boolean
	unsafe fn zeroing_masking(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.zeroing_masking()); }
	}

	/// `true` if zeroing-masking, `false` if merging-masking.
	unsafe fn set_zeroing_masking(lua, instr: &mut Instruction, new_value: bool) -> 0 {
		instr.inner.set_zeroing_masking(new_value);
	}

	/// `true` if merging-masking, `false` if zeroing-masking.
	///
	/// Only used by most EVEX encoded instructions that use opmask registers.
	///
	/// @return boolean
	unsafe fn merging_masking(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.merging_masking()); }
	}

	/// `true` if merging-masking, `false` if zeroing-masking.
	unsafe fn set_merging_masking(lua, instr: &mut Instruction, new_value: bool) -> 0 {
		instr.inner.set_merging_masking(new_value);
	}

	/// Gets the rounding control (a `RoundingControl` enum value) or `RoundingControl.None` if the instruction doesn't use it.
	///
	/// # Note
	/// SAE is implied but `Instruction:suppress_all_exceptions()` still returns `false`.
	///
	/// @return integer # A `RoundingControl` enum value
	unsafe fn rounding_control(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.rounding_control() as u32); }
	}

	/// Gets the rounding control (a `RoundingControl` enum value) or `RoundingControl.None` if the instruction doesn't use it.
	unsafe fn set_rounding_control(lua, instr: &mut Instruction, new_value: u32) -> 0 {
		instr.inner.set_rounding_control(unsafe { to_rounding_control(lua, new_value) });
	}

	/// Gets the number of elements in a `db`/`dw`/`dd`/`dq` directive.
	///
	/// Can only be called if `Instruction:code()` is `Code.DeclareByte`, `Code.DeclareWord`, `Code.DeclareDword`, `Code.DeclareQword`
	///
	/// @return integer
	unsafe fn declare_data_len(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.declare_data_len()); }
	}

	/// Gets the number of elements in a `db`/`dw`/`dd`/`dq` directive.
	unsafe fn set_declare_data_len(lua, instr: &mut Instruction, new_value: usize) -> 0 {
		instr.inner.set_declare_data_len(new_value);
	}

	/// Sets a new `db` value, see also `Instruction:declare_data_len()`.
	///
	/// Can only be called if `Instruction:code()` is `Code.DeclareByte`
	///
	/// @param index integer # Index (0-15)
	/// @param new_value integer # (`u8`) New value
	unsafe fn set_declare_byte_value(lua, instr: &mut Instruction, index: usize, new_value: u8) -> 0 {
		match instr.inner.try_set_declare_byte_value(index, new_value) {
			Ok(()) => {},
			Err(e) => unsafe { lua.throw_error(e) },
		}
	}

	/// Gets a `db` value, see also `Instruction:declare_data_len()`.
	///
	/// Can only be called if `Instruction:code()` is `Code.DeclareByte`
	///
	/// @param index integer # Index (0-15)
	/// @return integer # (`u8`) The value
	unsafe fn get_declare_byte_value(lua, instr: &Instruction, index: usize) -> 1 {
		let value = match instr.inner.try_get_declare_byte_value(index) {
			Ok(value) => value,
			Err(e) => unsafe { lua.throw_error(e) },
		};
		unsafe { lua.push(value); }
	}

	/// Gets a `db` value, see also `Instruction:declare_data_len()`.
	///
	/// Can only be called if `Instruction:code()` is `Code.DeclareByte`
	///
	/// @param index integer # Index (0-15)
	/// @return integer # (`i8`) The value
	unsafe fn get_declare_byte_value_i8(lua, instr: &Instruction, index: usize) -> 1 {
		let value = match instr.inner.try_get_declare_byte_value(index) {
			Ok(value) => value,
			Err(e) => unsafe { lua.throw_error(e) },
		};
		unsafe { lua.push(value as i8); }
	}

	/// Sets a new `dw` value, see also `Instruction:declare_data_len()`.
	///
	/// Can only be called if `Instruction:code()` is `Code.DeclareWord`
	///
	/// @param index integer # Index (0-7)
	/// @param new_value integer # (`u16`) New value
	unsafe fn set_declare_word_value(lua, instr: &mut Instruction, index: usize, new_value: u16) -> 0 {
		match instr.inner.try_set_declare_word_value(index, new_value) {
			Ok(()) => {},
			Err(e) => unsafe { lua.throw_error(e) },
		}
	}

	/// Gets a `dw` value, see also `Instruction:declare_data_len()`.
	///
	/// Can only be called if `Instruction:code()` is `Code.DeclareWord`
	///
	/// @param index integer # Index (0-7)
	/// @return integer # (`u16`) The value
	unsafe fn get_declare_word_value(lua, instr: &Instruction, index: usize) -> 1 {
		let value = match instr.inner.try_get_declare_word_value(index) {
			Ok(value) => value,
			Err(e) => unsafe { lua.throw_error(e) },
		};
		unsafe { lua.push(value); }
	}

	/// Gets a `dw` value, see also `Instruction:declare_data_len()`.
	///
	/// Can only be called if `Instruction:code()` is `Code.DeclareWord`
	///
	/// @param index integer # Index (0-7)
	/// @return integer # (`i16`) The value
	unsafe fn get_declare_word_value_i16(lua, instr: &Instruction, index: usize) -> 1 {
		let value = match instr.inner.try_get_declare_word_value(index) {
			Ok(value) => value,
			Err(e) => unsafe { lua.throw_error(e) },
		};
		unsafe { lua.push(value as i16); }
	}

	/// Sets a new `dd` value, see also `Instruction:declare_data_len()`.
	///
	/// Can only be called if `Instruction:code()` is `Code.DeclareDword`
	///
	/// @param index integer # Index (0-3)
	/// @param new_value integer # (`u32`) New value
	unsafe fn set_declare_dword_value(lua, instr: &mut Instruction, index: usize, new_value: u32) -> 0 {
		match instr.inner.try_set_declare_dword_value(index, new_value) {
			Ok(()) => {},
			Err(e) => unsafe { lua.throw_error(e) },
		}
	}

	/// Gets a `dd` value, see also `Instruction:declare_data_len()`.
	///
	/// Can only be called if `Instruction:code()` is `Code.DeclareDword`
	///
	/// @param index integer # Index (0-3)
	/// @return integer # (`u32`) The value
	unsafe fn get_declare_dword_value(lua, instr: &Instruction, index: usize) -> 1 {
		let value = match instr.inner.try_get_declare_dword_value(index) {
			Ok(value) => value,
			Err(e) => unsafe { lua.throw_error(e) },
		};
		unsafe { lua.push(value); }
	}

	/// Gets a `dd` value, see also `Instruction:declare_data_len()`.
	///
	/// Can only be called if `Instruction:code()` is `Code.DeclareDword`
	///
	/// @param index integer # Index (0-3)
	/// @return integer # (`i32`) The value
	unsafe fn get_declare_dword_value_i32(lua, instr: &Instruction, index: usize) -> 1 {
		let value = match instr.inner.try_get_declare_dword_value(index) {
			Ok(value) => value,
			Err(e) => unsafe { lua.throw_error(e) },
		};
		unsafe { lua.push(value as i32); }
	}

	/// Sets a new `dq` value, see also `Instruction:declare_data_len()`.
	///
	/// Can only be called if `Instruction:code()` is `Code.DeclareQword`
	///
	/// @param index integer # Index (0-1)
	/// @param new_value integer # (`u64`) New value
	unsafe fn set_declare_qword_value(lua, instr: &mut Instruction, index: usize, new_value: u64) -> 0 {
		match instr.inner.try_set_declare_qword_value(index, new_value) {
			Ok(()) => {},
			Err(e) => unsafe { lua.throw_error(e) },
		}
	}

	/// Gets a `dq` value, see also `Instruction:declare_data_len()`.
	///
	/// Can only be called if `Instruction:code()` is `Code.DeclareQword`
	///
	/// @param index integer # Index (0-1)
	/// @return integer # (`u64`) The value
	unsafe fn get_declare_qword_value(lua, instr: &Instruction, index: usize) -> 1 {
		let value = match instr.inner.try_get_declare_qword_value(index) {
			Ok(value) => value,
			Err(e) => unsafe { lua.throw_error(e) },
		};
		unsafe { lua.push(value); }
	}

	/// Gets a `dq` value, see also `Instruction:declare_data_len()`.
	///
	/// Can only be called if `Instruction:code()` is `Code.DeclareQword`
	///
	/// @param index integer # Index (0-1)
	/// @return integer # (`i64`) The value
	unsafe fn get_declare_qword_value_i64(lua, instr: &Instruction, index: usize) -> 1 {
		let value = match instr.inner.try_get_declare_qword_value(index) {
			Ok(value) => value,
			Err(e) => unsafe { lua.throw_error(e) },
		};
		unsafe { lua.push(value as i64); }
	}

	/// Checks if this is a VSIB instruction, see also `Instruction:is_vsib32()`, `Instruction:is_vsib64()`
	/// @return boolean
	unsafe fn is_vsib(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.is_vsib()); }
	}

	/// VSIB instructions only (`Instruction:is_vsib()`): `true` if it's using 32-bit indexes, `false` if it's using 64-bit indexes
	/// @return boolean
	unsafe fn is_vsib32(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.is_vsib32()); }
	}

	/// VSIB instructions only (`Instruction:is_vsib()`): `true` if it's using 64-bit indexes, `false` if it's using 32-bit indexes
	/// @return boolean
	unsafe fn is_vsib64(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.is_vsib64()); }
	}

	/// Checks if it's a vsib instruction.
	///
	/// - Returns `true` if it's a VSIB instruction with 64-bit indexes
	/// - Returns `false` if it's a VSIB instruction with 32-bit indexes
	/// - Returns `nil` if it's not a VSIB instruction.
	///
	/// @return boolean|nil
	unsafe fn vsib(lua, instr: &Instruction) -> 1 {
		match instr.inner.vsib() {
			Some(b) => unsafe { lua.push(b) },
			None => unsafe { lua.push(Nil) },
		}
	}

	/// Gets the suppress all exceptions flag (EVEX/MVEX encoded instructions). Note that if `Instruction:rounding_control()` is not `RoundingControl.None`, SAE is implied but this method will still return `false`.
	/// @return boolean
	unsafe fn suppress_all_exceptions(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.suppress_all_exceptions()); }
	}

	/// Gets the suppress all exceptions flag (EVEX/MVEX encoded instructions). Note that if `Instruction:rounding_control()` is not `RoundingControl.None`, SAE is implied but this method will still return `false`.
	unsafe fn set_suppress_all_exceptions(lua, instr: &mut Instruction, new_value: bool) -> 0 {
		instr.inner.set_suppress_all_exceptions(new_value);
	}

	/// Checks if the memory operand is `RIP`/`EIP` relative
	/// @return boolean
	unsafe fn is_ip_rel_memory_operand(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.is_ip_rel_memory_operand()); }
	}

	/// Gets the `RIP`/`EIP` releative address (`Instruction:memory_displacement()`).
	///
	/// This method is only valid if there's a memory operand with `RIP`/`EIP` relative addressing, see `Instruction:is_ip_rel_memory_operand()`
	///
	/// @return integer
	unsafe fn ip_rel_memory_address(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.ip_rel_memory_address()); }
	}

	/// Gets the number of bytes added to `SP`/`ESP`/`RSP` or 0 if it's not an instruction that pushes or pops data.
	///
	/// This method assumes the instruction doesn't change the privilege level (eg. `IRET/D/Q`). If it's the `LEAVE`
	/// instruction, this method returns 0.
	///
	/// @return integer
	///
	/// # Examples
	/// ```lua
	/// from iced_x86 import *
	///
	/// # pushfq
	/// data = b"\x9C"
	/// decoder = Decoder(64, data)
	/// instr = decoder.decode()
	///
	/// assert instr.is_stack_instruction
	/// assert instr.stack_pointer_increment == -8
	/// ```
	unsafe fn stack_pointer_increment(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.stack_pointer_increment()); }
	}

	/// Gets the FPU status word's `TOP` increment and whether it's a conditional or unconditional push/pop and whether `TOP` is written.
	///
	/// @return FpuStackIncrementInfo # FPU stack info
	///
	/// # Examples
	/// ```lua
	/// from iced_x86 import *
	///
	/// # ficomp dword ptr [rax]
	/// data = b"\xDA\x18"
	/// decoder = Decoder(64, data)
	/// instr = decoder.decode()
	///
	/// info = instr.fpu_stack_increment_info()
	/// # It pops the stack once
	/// assert info.increment == 1
	/// assert not info.conditional
	/// assert info.writes_top
	/// ```
	unsafe fn fpu_stack_increment_info(lua, instr: &Instruction) -> 1 {
		unsafe { let _ = FpuStackIncrementInfo::init_and_push_iced(lua, &instr.inner.fpu_stack_increment_info()); }
	}

	/// Instruction encoding, eg. Legacy, 3DNow!, VEX, EVEX, XOP (an `EncodingKind` enum value)
	/// @return integer # An `EncodingKind` enum value
	///
	/// # Examples
	/// ```lua
	/// from iced_x86 import *
	///
	/// # vmovaps xmm1,xmm5
	/// data = b"\xC5\xF8\x28\xCD"
	/// decoder = Decoder(64, data)
	/// instr = decoder.decode()
	///
	/// assert instr.encoding == EncodingKind.VEX
	/// ```
	unsafe fn encoding(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.encoding() as u32); }
	}

	/// Gets the CPU or CPUID feature flags (an array of `CpuidFeature` enum values)
	///
	/// @return integer[] # (A `CpuidFeature` array) CPU or CPUID feature flags
	///
	/// # Examples
	/// ```lua
	/// from iced_x86 import *
	///
	/// # vmovaps xmm1,xmm5
	/// # vmovaps xmm10{k3}{z},xmm19
	/// data = b"\xC5\xF8\x28\xCD\x62\x31\x7C\x8B\x28\xD3"
	/// decoder = Decoder(64, data)
	///
	/// # vmovaps xmm1,xmm5
	/// instr = decoder.decode()
	/// cpuid = instr.cpuid_features()
	/// assert len(cpuid) == 1
	/// assert cpuid[0] == CpuidFeature.AVX
	///
	/// # vmovaps xmm10{k3}{z},xmm19
	/// instr = decoder.decode()
	/// cpuid = instr.cpuid_features()
	/// assert len(cpuid) == 2
	/// assert cpuid[0] == CpuidFeature.AVX512VL
	/// assert cpuid[1] == CpuidFeature.AVX512F
	/// ```
	unsafe fn cpuid_features(lua, instr: &Instruction) -> 1 {
		let cpuid_features = instr.inner.cpuid_features();
		unsafe {
			lua.create_table(cpuid_features.len() as c_int, 0);
			for (i, &cpuid) in cpuid_features.iter().enumerate() {
				lua.push(i + 1);
				lua.push(cpuid as u32);
				lua.raw_set(-3);
			}
		}
	}

	/// Control flow info (a `FlowControl` enum value)
	/// @return integer # A `FlowControl` enum value
	///
	/// # Examples
	/// ```lua
	/// from iced_x86 import *
	///
	/// # or ecx,esi
	/// # ud0 rcx,rsi
	/// # call rcx
	/// data = b"\x0B\xCE\x48\x0F\xFF\xCE\xFF\xD1"
	/// decoder = Decoder(64, data)
	///
	/// # or ecx,esi
	/// instr = decoder.decode()
	/// assert instr.flow_control == FlowControl.NEXT
	///
	/// # ud0 rcx,rsi
	/// instr = decoder.decode()
	/// assert instr.flow_control == FlowControl.EXCEPTION
	///
	/// # call rcx
	/// instr = decoder.decode()
	/// assert instr.flow_control == FlowControl.INDIRECT_CALL
	/// ```
	unsafe fn flow_control(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.flow_control() as u32); }
	}

	/// `true` if it's a privileged instruction (all CPL=0 instructions (except `VMCALL`) and IOPL instructions `IN`, `INS`, `OUT`, `OUTS`, `CLI`, `STI`)
	/// @return boolean
	unsafe fn is_privileged(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.is_privileged()); }
	}

	/// `true` if this is an instruction that implicitly uses the stack pointer (`SP`/`ESP`/`RSP`), eg. `CALL`, `PUSH`, `POP`, `RET`, etc.
	///
	/// See also `Instruction:stack_pointer_increment()`
	///
	/// @return boolean
	///
	/// # Examples
	/// ```lua
	/// from iced_x86 import *
	///
	/// # or ecx,esi
	/// # push rax
	/// data = b"\x0B\xCE\x50"
	/// decoder = Decoder(64, data)
	///
	/// # or ecx,esi
	/// instr = decoder.decode()
	/// assert not instr.is_stack_instruction
	///
	/// # push rax
	/// instr = decoder.decode()
	/// assert instr.is_stack_instruction
	/// assert instr.stack_pointer_increment == -8
	/// ```
	unsafe fn is_stack_instruction(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.is_stack_instruction()); }
	}

	/// `true` if it's an instruction that saves or restores too many registers (eg. `FXRSTOR`, `XSAVE`, etc).
	/// @return boolean
	unsafe fn is_save_restore_instruction(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.is_save_restore_instruction()); }
	}

	/// `true` if it's a "string" instruction, such as `MOVS`, `LODS`, `SCAS`, etc.
	/// @return boolean
	unsafe fn is_string_instruction(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.is_string_instruction()); }
	}

	/// All flags that are read by the CPU when executing the instruction.
	///
	/// This method returns an `RflagsBits` value. See also `Instruction:rflags_modified()`.
	///
	/// @return integer # An `RflagsBits` enum value
	///
	/// # Examples
	/// ```lua
	/// from iced_x86 import *
	///
	/// # adc rsi,rcx
	/// # xor rdi,5Ah
	/// data = b"\x48\x11\xCE\x48\x83\xF7\x5A"
	/// decoder = Decoder(64, data)
	///
	/// # adc rsi,rcx
	/// instr = decoder.decode()
	/// assert instr.rflags_read == RflagsBits.CF
	/// assert instr.rflags_written == RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF
	/// assert instr.rflags_cleared == RflagsBits.NONE
	/// assert instr.rflags_set == RflagsBits.NONE
	/// assert instr.rflags_undefined == RflagsBits.NONE
	/// assert instr.rflags_modified == RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF
	///
	/// # xor rdi,5Ah
	/// instr = decoder.decode()
	/// assert instr.rflags_read == RflagsBits.NONE
	/// assert instr.rflags_written == RflagsBits.SF | RflagsBits.ZF | RflagsBits.PF
	/// assert instr.rflags_cleared == RflagsBits.OF | RflagsBits.CF
	/// assert instr.rflags_set == RflagsBits.NONE
	/// assert instr.rflags_undefined == RflagsBits.AF
	/// assert instr.rflags_modified == RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF
	/// ```
	unsafe fn rflags_read(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.rflags_read()); }
	}

	/// All flags that are written by the CPU, except those flags that are known to be undefined, always set or always cleared.
	///
	/// This method returns an `RflagsBits` value. See also `Instruction:rflags_modified()`.
	///
	/// @return integer # An `RflagsBits` enum value
	///
	/// # Examples
	/// ```lua
	/// from iced_x86 import *
	///
	/// # adc rsi,rcx
	/// # xor rdi,5Ah
	/// data = b"\x48\x11\xCE\x48\x83\xF7\x5A"
	/// decoder = Decoder(64, data)
	///
	/// # adc rsi,rcx
	/// instr = decoder.decode()
	/// assert instr.rflags_read == RflagsBits.CF
	/// assert instr.rflags_written == RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF
	/// assert instr.rflags_cleared == RflagsBits.NONE
	/// assert instr.rflags_set == RflagsBits.NONE
	/// assert instr.rflags_undefined == RflagsBits.NONE
	/// assert instr.rflags_modified == RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF
	///
	/// # xor rdi,5Ah
	/// instr = decoder.decode()
	/// assert instr.rflags_read == RflagsBits.NONE
	/// assert instr.rflags_written == RflagsBits.SF | RflagsBits.ZF | RflagsBits.PF
	/// assert instr.rflags_cleared == RflagsBits.OF | RflagsBits.CF
	/// assert instr.rflags_set == RflagsBits.NONE
	/// assert instr.rflags_undefined == RflagsBits.AF
	/// assert instr.rflags_modified == RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF
	/// ```
	unsafe fn rflags_written(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.rflags_written()); }
	}

	/// All flags that are always cleared by the CPU.
	///
	/// This method returns an `RflagsBits` value. See also `Instruction:rflags_modified()`.
	///
	/// @return integer # An `RflagsBits` enum value
	///
	/// # Examples
	/// ```lua
	/// from iced_x86 import *
	///
	/// # adc rsi,rcx
	/// # xor rdi,5Ah
	/// data = b"\x48\x11\xCE\x48\x83\xF7\x5A"
	/// decoder = Decoder(64, data)
	///
	/// # adc rsi,rcx
	/// instr = decoder.decode()
	/// assert instr.rflags_read == RflagsBits.CF
	/// assert instr.rflags_written == RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF
	/// assert instr.rflags_cleared == RflagsBits.NONE
	/// assert instr.rflags_set == RflagsBits.NONE
	/// assert instr.rflags_undefined == RflagsBits.NONE
	/// assert instr.rflags_modified == RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF
	///
	/// # xor rdi,5Ah
	/// instr = decoder.decode()
	/// assert instr.rflags_read == RflagsBits.NONE
	/// assert instr.rflags_written == RflagsBits.SF | RflagsBits.ZF | RflagsBits.PF
	/// assert instr.rflags_cleared == RflagsBits.OF | RflagsBits.CF
	/// assert instr.rflags_set == RflagsBits.NONE
	/// assert instr.rflags_undefined == RflagsBits.AF
	/// assert instr.rflags_modified == RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF
	/// ```
	unsafe fn rflags_cleared(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.rflags_cleared()); }
	}

	/// All flags that are always set by the CPU.
	///
	/// This method returns an `RflagsBits` value. See also `Instruction:rflags_modified()`.
	///
	/// @return integer # An `RflagsBits` enum value
	///
	/// # Examples
	/// ```lua
	/// from iced_x86 import *
	///
	/// # adc rsi,rcx
	/// # xor rdi,5Ah
	/// data = b"\x48\x11\xCE\x48\x83\xF7\x5A"
	/// decoder = Decoder(64, data)
	///
	/// # adc rsi,rcx
	/// instr = decoder.decode()
	/// assert instr.rflags_read == RflagsBits.CF
	/// assert instr.rflags_written == RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF
	/// assert instr.rflags_cleared == RflagsBits.NONE
	/// assert instr.rflags_set == RflagsBits.NONE
	/// assert instr.rflags_undefined == RflagsBits.NONE
	/// assert instr.rflags_modified == RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF
	///
	/// # xor rdi,5Ah
	/// instr = decoder.decode()
	/// assert instr.rflags_read == RflagsBits.NONE
	/// assert instr.rflags_written == RflagsBits.SF | RflagsBits.ZF | RflagsBits.PF
	/// assert instr.rflags_cleared == RflagsBits.OF | RflagsBits.CF
	/// assert instr.rflags_set == RflagsBits.NONE
	/// assert instr.rflags_undefined == RflagsBits.AF
	/// assert instr.rflags_modified == RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF
	/// ```
	unsafe fn rflags_set(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.rflags_set()); }
	}

	/// All flags that are undefined after executing the instruction.
	///
	/// This method returns an `RflagsBits` value. See also `Instruction:rflags_modified()`.
	///
	/// @return integer # An `RflagsBits` enum value
	///
	/// # Examples
	/// ```lua
	/// from iced_x86 import *
	///
	/// # adc rsi,rcx
	/// # xor rdi,5Ah
	/// data = b"\x48\x11\xCE\x48\x83\xF7\x5A"
	/// decoder = Decoder(64, data)
	///
	/// # adc rsi,rcx
	/// instr = decoder.decode()
	/// assert instr.rflags_read == RflagsBits.CF
	/// assert instr.rflags_written == RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF
	/// assert instr.rflags_cleared == RflagsBits.NONE
	/// assert instr.rflags_set == RflagsBits.NONE
	/// assert instr.rflags_undefined == RflagsBits.NONE
	/// assert instr.rflags_modified == RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF
	///
	/// # xor rdi,5Ah
	/// instr = decoder.decode()
	/// assert instr.rflags_read == RflagsBits.NONE
	/// assert instr.rflags_written == RflagsBits.SF | RflagsBits.ZF | RflagsBits.PF
	/// assert instr.rflags_cleared == RflagsBits.OF | RflagsBits.CF
	/// assert instr.rflags_set == RflagsBits.NONE
	/// assert instr.rflags_undefined == RflagsBits.AF
	/// assert instr.rflags_modified == RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF
	/// ```
	unsafe fn rflags_undefined(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.rflags_undefined()); }
	}

	/// All flags that are modified by the CPU. This is `rflags_written + rflags_cleared + rflags_set + rflags_undefined`.
	///
	/// This method returns an `RflagsBits` value.
	///
	/// @return integer # An `RflagsBits` enum value
	///
	/// # Examples
	/// ```lua
	/// from iced_x86 import *
	///
	/// # adc rsi,rcx
	/// # xor rdi,5Ah
	/// data = b"\x48\x11\xCE\x48\x83\xF7\x5A"
	/// decoder = Decoder(64, data)
	///
	/// # adc rsi,rcx
	/// instr = decoder.decode()
	/// assert instr.rflags_read == RflagsBits.CF
	/// assert instr.rflags_written == RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF
	/// assert instr.rflags_cleared == RflagsBits.NONE
	/// assert instr.rflags_set == RflagsBits.NONE
	/// assert instr.rflags_undefined == RflagsBits.NONE
	/// assert instr.rflags_modified == RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF
	///
	/// # xor rdi,5Ah
	/// instr = decoder.decode()
	/// assert instr.rflags_read == RflagsBits.NONE
	/// assert instr.rflags_written == RflagsBits.SF | RflagsBits.ZF | RflagsBits.PF
	/// assert instr.rflags_cleared == RflagsBits.OF | RflagsBits.CF
	/// assert instr.rflags_set == RflagsBits.NONE
	/// assert instr.rflags_undefined == RflagsBits.AF
	/// assert instr.rflags_modified == RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF
	/// ```
	unsafe fn rflags_modified(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.rflags_modified()); }
	}

	/// Checks if it's a `Jcc SHORT` or `Jcc NEAR` instruction
	/// @return boolean
	unsafe fn is_jcc_short_or_near(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.is_jcc_short_or_near()); }
	}

	/// Checks if it's a `Jcc NEAR` instruction
	/// @return boolean
	unsafe fn is_jcc_near(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.is_jcc_near()); }
	}

	/// Checks if it's a `Jcc SHORT` instruction
	/// @return boolean
	unsafe fn is_jcc_short(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.is_jcc_short()); }
	}

	/// Checks if it's a `JMP SHORT` instruction
	/// @return boolean
	unsafe fn is_jmp_short(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.is_jmp_short()); }
	}

	/// Checks if it's a `JMP NEAR` instruction
	/// @return boolean
	unsafe fn is_jmp_near(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.is_jmp_near()); }
	}

	/// Checks if it's a `JMP SHORT` or a `JMP NEAR` instruction
	/// @return boolean
	unsafe fn is_jmp_short_or_near(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.is_jmp_short_or_near()); }
	}

	/// Checks if it's a `JMP FAR` instruction
	/// @return boolean
	unsafe fn is_jmp_far(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.is_jmp_far()); }
	}

	/// Checks if it's a `CALL NEAR` instruction
	/// @return boolean
	unsafe fn is_call_near(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.is_call_near()); }
	}

	/// Checks if it's a `CALL FAR` instruction
	/// @return boolean
	unsafe fn is_call_far(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.is_call_far()); }
	}

	/// Checks if it's a `JMP NEAR reg/[mem]` instruction
	/// @return boolean
	unsafe fn is_jmp_near_indirect(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.is_jmp_near_indirect()); }
	}

	/// Checks if it's a `JMP FAR [mem]` instruction
	/// @return boolean
	unsafe fn is_jmp_far_indirect(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.is_jmp_far_indirect()); }
	}

	/// Checks if it's a `CALL NEAR reg/[mem]` instruction
	/// @return boolean
	unsafe fn is_call_near_indirect(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.is_call_near_indirect()); }
	}

	/// Checks if it's a `CALL FAR [mem]` instruction
	/// @return boolean
	unsafe fn is_call_far_indirect(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.is_call_far_indirect()); }
	}

	/// Checks if it's a `JKccD SHORT` or `JKccD NEAR` instruction
	/// @return boolean
	unsafe fn is_jkcc_short_or_near(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.is_jkcc_short_or_near()); }
	}

	/// Checks if it's a `JKccD NEAR` instruction
	/// @return boolean
	unsafe fn is_jkcc_near(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.is_jkcc_near()); }
	}

	/// Checks if it's a `JKccD SHORT` instruction
	/// @return boolean
	unsafe fn is_jkcc_short(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.is_jkcc_short()); }
	}

	/// Checks if it's a `JCXZ SHORT`, `JECXZ SHORT` or `JRCXZ SHORT` instruction
	/// @return boolean
	unsafe fn is_jcx_short(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.code().is_jcx_short()); }
	}

	/// Checks if it's a `LOOPcc SHORT` instruction
	/// @return boolean
	unsafe fn is_loopcc(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.code().is_loopcc()); }
	}

	/// Checks if it's a `LOOP SHORT` instruction
	/// @return boolean
	unsafe fn is_loop(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.code().is_loop()); }
	}

	/// Negates the condition code, eg. `JE` -> `JNE`.
	///
	/// Can be used if it's `Jcc`, `SETcc`, `CMOVcc`, `LOOPcc` and does nothing if the instruction doesn't have a condition code.
	///
	/// # Examples
	/// ```lua
	/// from iced_x86 import *
	///
	/// # setbe al
	/// data = b"\x0F\x96\xC0"
	/// decoder = Decoder(64, data)
	///
	/// instr = decoder.decode()
	/// assert instr.code == Code.SETBE_RM8
	/// assert instr.condition_code == ConditionCode.BE
	/// instr.negate_condition_code()
	/// assert instr.code == Code.SETA_RM8
	/// assert instr.condition_code == ConditionCode.A
	/// ```
	unsafe fn negate_condition_code(lua, instr: &mut Instruction) -> 0 {
		instr.inner.negate_condition_code();
	}

	/// Converts `Jcc/JMP NEAR` to `Jcc/JMP SHORT` and does nothing if it's not a `Jcc/JMP NEAR` instruction
	///
	/// # Examples
	/// ```lua
	/// from iced_x86 import *
	///
	/// # jbe near ptr label
	/// data = b"\x0F\x86\x5A\xA5\x12\x34"
	/// decoder = Decoder(64, data)
	///
	/// instr = decoder.decode()
	/// assert instr.code == Code.JBE_REL32_64
	/// instr.as_short_branch()
	/// assert instr.code == Code.JBE_REL8_64
	/// instr.as_short_branch()
	/// assert instr.code == Code.JBE_REL8_64
	/// ```
	unsafe fn as_short_branch(lua, instr: &mut Instruction) -> 0 {
		instr.inner.as_short_branch();
	}

	/// Converts `Jcc/JMP SHORT` to `Jcc/JMP NEAR` and does nothing if it's not a `Jcc/JMP SHORT` instruction
	///
	/// # Examples
	/// ```lua
	/// from iced_x86 import *
	///
	/// # jbe short label
	/// data = b"\x76\x5A"
	/// decoder = Decoder(64, data)
	///
	/// instr = decoder.decode()
	/// assert instr.code == Code.JBE_REL8_64
	/// instr.as_near_branch()
	/// assert instr.code == Code.JBE_REL32_64
	/// instr.as_near_branch()
	/// assert instr.code == Code.JBE_REL32_64
	/// ```
	unsafe fn as_near_branch(lua, instr: &mut Instruction) -> 0 {
		instr.inner.as_near_branch();
	}

	/// Gets the condition code (a `ConditionCode` enum value) if it's `Jcc`, `SETcc`, `CMOVcc`, `LOOPcc` else `ConditionCode.None` is returned
	/// @return integer # A `ConditionCode` enum value
	///
	/// # Examples
	/// ```lua
	/// from iced_x86 import *
	///
	/// # setbe al
	/// # jl short label
	/// # cmovne ecx,esi
	/// # nop
	/// data = b"\x0F\x96\xC0\x7C\x5A\x0F\x45\xCE\x90"
	/// decoder = Decoder(64, data)
	///
	/// # setbe al
	/// instr = decoder.decode()
	/// assert instr.condition_code == ConditionCode.BE
	///
	/// # jl short label
	/// instr = decoder.decode()
	/// assert instr.condition_code == ConditionCode.L
	///
	/// # cmovne ecx,esi
	/// instr = decoder.decode()
	/// assert instr.condition_code == ConditionCode.NE
	///
	/// # nop
	/// instr = decoder.decode()
	/// assert instr.condition_code == ConditionCode.NONE
	/// ```
	unsafe fn condition_code(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.condition_code() as u32); }
	}

	/// Gets the `OpCodeInfo`
	///
	/// @return OpCodeInfo # Op code info
	unsafe fn op_code(lua, instr: &Instruction) -> 1 {
		unsafe { let _ = OpCodeInfo::push_new(lua, instr.inner.code()); }
	}
}

lua_methods! {
	unsafe fn instruction_tostring(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(&instr.inner.to_string()) }
	}

	unsafe fn instruction_eq(lua, instr: &Instruction, instr2: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner == instr2.inner) }
	}

	unsafe fn instruction_len(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.len()) }
	}
}
