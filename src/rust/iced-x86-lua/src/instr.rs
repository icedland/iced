// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::enum_utils::{to_code, to_code_size, to_mvex_reg_mem_conv, to_op_kind, to_register, to_rep_prefix_kind, to_rounding_control};
use crate::fpui::FpuStackIncrementInfo;
use crate::info::mem::UsedMemory;
use crate::info::regs::UsedRegister;
use crate::opci::OpCodeInfo;
use libc::c_int;
use loona::lua_api::{lua_CFunction, lua_GetIType, LUA_TNUMBER, LUA_TSTRING, LUA_TTABLE};
use loona::prelude::*;

lua_struct_module! { luaopen_iced_x86_Instruction : Instruction }
lua_impl_userdata! { Instruction }

/// A 16/32/64-bit x86 instruction. Created by `Decoder` or by `Instruction.create*()` methods
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

macro_rules! mk_dx_body {
	($lua:ident, $elem_size:literal, $lua_try_get_num:path, $str_arg_fn:path, $slice_arg_fn:path,
	$err_string_arg:literal, $err_int_values:literal, $err_count_int_values:literal,) => {
		const MAX_ELEMS: c_int = 16 / $elem_size;
		const IGNORED_ARGS: c_int = 0;
		const ERR_STRING_ARG: &str = $err_string_arg;
		const ERR_INT_VALUES: &str = $err_int_values;
		const ERR_COUNT_INT_VALUES: &str = $err_count_int_values;

		let num_args = $lua.get_top().wrapping_sub(IGNORED_ARGS);
		let type0 = $lua.type_(IGNORED_ARGS + 1);
		if type0 == LUA_TSTRING {
			if num_args != 1 {
				$lua.throw_error_msg("Invalid number of args: expected one string");
			}
			let bytes = if let Some(bytes) = $lua.try_get_byte_slice(IGNORED_ARGS + 1) {
				bytes
			} else {
				$lua.throw_error_msg("Expected one string");
			};
			let instr = if let Ok(instr) = $str_arg_fn(bytes) {
				Instruction { inner: instr }
			} else {
				$lua.throw_error_msg(ERR_STRING_ARG);
			};
			let _ = Instruction::init_and_push($lua, &instr);
		} else if type0 == LUA_TTABLE {
			if num_args != 1 {
				$lua.throw_error_msg("Invalid number of args: expected one array");
			}
			let table_len = $lua.raw_len(IGNORED_ARGS + 1);
			if table_len < 1 || table_len > MAX_ELEMS as usize {
				$lua.throw_error_msg("Array is empty or has too many elements");
			}
			let mut data = [0; MAX_ELEMS as usize];
			for i in 0..(table_len as lua_GetIType) {
				$lua.raw_get_i(IGNORED_ARGS + 1, i + 1);
				let idx = -1;
				if $lua.type_(idx) != LUA_TNUMBER {
					$lua.throw_error_msg(ERR_INT_VALUES);
				}
				if let Ok(value) = $lua_try_get_num($lua, idx) {
					data[i as usize] = value;
				} else {
					$lua.throw_error_msg(ERR_INT_VALUES);
				}
				$lua.pop(1);
			}
			let instr = if let Ok(instr) = $slice_arg_fn(&data[0..table_len]) {
				Instruction { inner: instr }
			} else {
				$lua.throw_error_msg(ERR_COUNT_INT_VALUES);
			};
			let _ = Instruction::init_and_push($lua, &instr);
		} else {
			if num_args < 1 || num_args > MAX_ELEMS {
				$lua.throw_error_msg(ERR_COUNT_INT_VALUES);
			}
			let mut data = [0; MAX_ELEMS as usize];
			for i in 0..num_args {
				let idx = IGNORED_ARGS + 1 + i;
				if let Ok(value) = $lua_try_get_num($lua, idx) {
					data[i as usize] = value;
				} else {
					$lua.throw_error_msg(ERR_INT_VALUES);
				}
			}
			let instr = if let Ok(instr) = $slice_arg_fn(&data[0..num_args as usize]) {
				Instruction { inner: instr }
			} else {
				$lua.throw_error_msg(ERR_COUNT_INT_VALUES);
			};
			let _ = Instruction::init_and_push($lua, &instr);
		}
	};
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
	unsafe fn copy(lua, this: &Instruction) -> 1 {
		unsafe {
			let _ = Instruction::init_and_push(lua, this);
		}
	}

	/// Checks if two instructions are equal, comparing all bits, not ignoring anything. `==` ignores some fields.
	/// @param other Instruction #Other instruction
	unsafe fn eq_all_bits(lua, this: &Instruction, other: &Instruction) -> 1 {
		unsafe {
			lua.push(this.inner.eq_all_bits(&other.inner))
		}
	}

	/// Gets the 16-bit IP of the instruction
	/// @return integer
	unsafe fn ip16(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.ip16()); }
	}

	unsafe fn set_ip16(lua, this: &mut Instruction, new_value: u16) -> 0 {
		this.inner.set_ip16(new_value);
	}

	/// Gets the 32-bit IP of the instruction
	/// @return integer
	unsafe fn ip32(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.ip32()); }
	}

	unsafe fn set_ip32(lua, this: &mut Instruction, new_value: u32) -> 0 {
		this.inner.set_ip32(new_value);
	}

	/// Gets the 64-bit IP of the instruction
	/// @return integer
	unsafe fn ip(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.ip()); }
	}

	unsafe fn set_ip(lua, this: &mut Instruction, new_value: u64) -> 0 {
		this.inner.set_ip(new_value);
	}

	/// Gets the 16-bit IP of the next instruction
	/// @return integer
	unsafe fn next_ip16(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.next_ip16()); }
	}

	unsafe fn set_next_ip16(lua, this: &mut Instruction, new_value: u16) -> 0 {
		this.inner.set_next_ip16(new_value);
	}

	/// Gets the 32-bit IP of the next instruction
	/// @return integer
	unsafe fn next_ip32(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.next_ip32()); }
	}

	unsafe fn set_next_ip32(lua, this: &mut Instruction, new_value: u32) -> 0 {
		this.inner.set_next_ip32(new_value);
	}

	/// Gets the 64-bit IP of the next instruction
	/// @return integer
	unsafe fn next_ip(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.next_ip()); }
	}

	unsafe fn set_next_ip(lua, this: &mut Instruction, new_value: u64) -> 0 {
		this.inner.set_next_ip(new_value);
	}

	/// Gets the code size (a `CodeSize` enum value) when the instruction was decoded.
	///
	/// # Note
	/// This value is informational and can be used by a formatter.
	///
	/// @return integer # A `CodeSize` enum value
	unsafe fn code_size(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.code_size() as u32); }
	}

	unsafe fn set_code_size(lua, this: &mut Instruction, new_value: u32) -> 0 {
		this.inner.set_code_size(unsafe { to_code_size(lua, new_value) });
	}

	/// Checks if it's an invalid instruction (`Instruction:code()` == `Code.INVALID`)
	/// @return boolean
	unsafe fn is_invalid(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.is_invalid()); }
	}

	/// Gets the instruction code (a `Code` enum value), see also `Instruction:mnemonic()`
	/// @return integer # A `Code` enum value
	unsafe fn code(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.code() as u32); }
	}

	unsafe fn set_code(lua, this: &mut Instruction, new_value: u32) -> 0 {
		this.inner.set_code(unsafe { to_code(lua, new_value) });
	}

	/// Gets the mnemonic (a `Mnemonic` enum value), see also `Instruction:code()`
	/// @return integer # A `Mnemonic` enum value
	unsafe fn mnemonic(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.mnemonic() as u32); }
	}

	/// Gets the operand count. An instruction can have 0-5 operands.
	/// @return integer
	///
	/// # Examples
	/// ```lua
	/// local Decoder = require("iced_x86.Decoder")
	///
	/// -- add [rax],ebx
	/// local data = "\001\024"
	/// local decoder = Decoder.new(64, data)
	/// local instr = decoder:decode()
	///
	/// assert(instr:op_count() == 2)
	/// ```
	unsafe fn op_count(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.op_count()); }
	}

	/// Gets the length of the instruction, 0-15 bytes.
	///
	/// You can also call `#instr` to get this value.
	///
	/// # Note
	/// This is just informational. If you modify the instruction or create a new one, this method could return the wrong value.
	///
	/// @return integer
	unsafe fn len(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.len()); }
	}

	unsafe fn set_len(lua, this: &mut Instruction, new_value: usize) -> 0 {
		this.inner.set_len(new_value);
	}

	/// `true` if the instruction has the `XACQUIRE` prefix (`F2`)
	/// @return boolean
	unsafe fn has_xacquire_prefix(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.has_xacquire_prefix()); }
	}

	unsafe fn set_has_xacquire_prefix(lua, this: &mut Instruction, new_value: bool) -> 0 {
		this.inner.set_has_xacquire_prefix(new_value);
	}

	/// `true` if the instruction has the `XRELEASE` prefix (`F3`)
	/// @return boolean
	unsafe fn has_xrelease_prefix(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.has_xrelease_prefix()); }
	}

	unsafe fn set_has_xrelease_prefix(lua, this: &mut Instruction, new_value: bool) -> 0 {
		this.inner.set_has_xrelease_prefix(new_value);
	}

	/// `true` if the instruction has the `REPE` or `REP` prefix (`F3`)
	/// @return boolean
	unsafe fn has_rep_prefix(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.has_rep_prefix()); }
	}

	unsafe fn set_has_rep_prefix(lua, this: &mut Instruction, new_value: bool) -> 0 {
		this.inner.set_has_rep_prefix(new_value);
	}

	/// `true` if the instruction has the `REPE` or `REP` prefix (`F3`)
	/// @return boolean
	unsafe fn has_repe_prefix(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.has_repe_prefix()); }
	}

	unsafe fn set_has_repe_prefix(lua, this: &mut Instruction, new_value: bool) -> 0 {
		this.inner.set_has_repe_prefix(new_value);
	}

	/// `true` if the instruction has the `REPNE` prefix (`F2`)
	/// @return boolean
	unsafe fn has_repne_prefix(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.has_repne_prefix()); }
	}

	unsafe fn set_has_repne_prefix(lua, this: &mut Instruction, new_value: bool) -> 0 {
		this.inner.set_has_repne_prefix(new_value);
	}

	/// `true` if the instruction has the `LOCK` prefix (`F0`)
	/// @return boolean
	unsafe fn has_lock_prefix(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.has_lock_prefix()); }
	}

	unsafe fn set_has_lock_prefix(lua, this: &mut Instruction, new_value: bool) -> 0 {
		this.inner.set_has_lock_prefix(new_value);
	}

	/// Gets operand #0's kind (an `OpKind` enum value) if the operand exists (see `Instruction:op_count()` and `Instruction:op_kind()`)
	/// @return integer # An `OpKind` enum value
	unsafe fn op0_kind(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.op0_kind() as u32); }
	}

	unsafe fn set_op0_kind(lua, this: &mut Instruction, new_value: u32) -> 0 {
		this.inner.set_op0_kind(unsafe { to_op_kind(lua, new_value) });
	}

	/// Gets operand #1's kind (an `OpKind` enum value) if the operand exists (see `Instruction:op_count()` and `Instruction:op_kind()`)
	/// @return integer # An `OpKind` enum value
	unsafe fn op1_kind(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.op1_kind() as u32); }
	}

	unsafe fn set_op1_kind(lua, this: &mut Instruction, new_value: u32) -> 0 {
		this.inner.set_op1_kind(unsafe { to_op_kind(lua, new_value) });
	}

	/// Gets operand #2's kind (an `OpKind` enum value) if the operand exists (see `Instruction:op_count()` and `Instruction:op_kind()`)
	/// @return integer # An `OpKind` enum value
	unsafe fn op2_kind(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.op2_kind() as u32); }
	}

	unsafe fn set_op2_kind(lua, this: &mut Instruction, new_value: u32) -> 0 {
		this.inner.set_op2_kind(unsafe { to_op_kind(lua, new_value) });
	}

	/// Gets operand #3's kind (an `OpKind` enum value) if the operand exists (see `Instruction:op_count()` and `Instruction:op_kind()`)
	/// @return integer # An `OpKind` enum value
	unsafe fn op3_kind(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.op3_kind() as u32); }
	}

	unsafe fn set_op3_kind(lua, this: &mut Instruction, new_value: u32) -> 0 {
		this.inner.set_op3_kind(unsafe { to_op_kind(lua, new_value) });
	}

	/// Gets operand #4's kind (an `OpKind` enum value) if the operand exists (see `Instruction:op_count()` and `Instruction:op_kind()`)
	/// @return integer # An `OpKind` enum value
	unsafe fn op4_kind(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.op4_kind() as u32); }
	}

	unsafe fn set_op4_kind(lua, this: &mut Instruction, new_value: u32) -> 0 {
		match this.inner.try_set_op4_kind(unsafe { to_op_kind(lua, new_value) }) {
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
	/// local Decoder = require("iced_x86.Decoder")
	/// local OpKind = require("iced_x86.OpKind")
	/// local Register = require("iced_x86.Register")
	///
	/// -- add [rax],ebx
	/// local data = "\001\024"
	/// local decoder = Decoder.new(64, data)
	/// local instr = decoder:decode()
	///
	/// assert(instr:op_count() == 2)
	/// assert(instr:op_kind(0) == OpKind.Memory)
	/// assert(instr:memory_base() == Register.RAX)
	/// assert(instr:memory_index() == Register.None)
	/// assert(instr:op_kind(1) == OpKind.Register)
	/// assert(instr:op_register(1) == Register.EBX)
	/// ```
	unsafe fn op_kind(lua, this: &Instruction, operand: u32) -> 1 {
		match this.inner.try_op_kind(operand) {
			Ok(op_kind) => unsafe { lua.push(op_kind as u32) },
			Err(e) => unsafe { lua.throw_error(e) },
		}
	}

	unsafe fn set_op_kind(lua, this: &mut Instruction, operand: u32, op_kind: u32) -> 0 {
		match this.inner.try_set_op_kind(operand, unsafe { to_op_kind(lua, op_kind) }) {
			Ok(()) => {},
			Err(e) => unsafe { lua.throw_error(e) },
		}
	}

	/// Checks if the instruction has a segment override prefix, see `Instruction:segment_prefix()`
	/// @return boolean
	unsafe fn has_segment_prefix(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.has_segment_prefix()); }
	}

	/// Gets the segment override prefix (a `Register` enum value) or `Register.None` if none.
	///
	/// See also `Instruction:memory_segment()`.
	///
	/// Use this method if the operand has kind `OpKind.Memory`,
	/// `OpKind.MemorySegSI`, `OpKind.MemorySegESI`, `OpKind.MemorySegRSI`
	///
	/// @return integer # A `Register` enum value
	unsafe fn segment_prefix(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.segment_prefix() as u32); }
	}

	unsafe fn set_segment_prefix(lua, this: &mut Instruction, new_value: u32) -> 0 {
		this.inner.set_segment_prefix(unsafe { to_register(lua, new_value) });
	}

	/// Gets the effective segment register used to reference the memory location (a `Register` enum value).
	///
	/// Use this method if the operand has kind `OpKind.Memory`, `OpKind.MemorySegSI`, `OpKind.MemorySegESI`, `OpKind.MemorySegRSI`
	///
	/// @return integer # A `Register` enum value
	unsafe fn memory_segment(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.memory_segment() as u32); }
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
	unsafe fn memory_displ_size(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.memory_displ_size()); }
	}

	unsafe fn set_memory_displ_size(lua, this: &mut Instruction, new_value: u32) -> 0 {
		this.inner.set_memory_displ_size(new_value);
	}

	/// `true` if the data is broadcast (EVEX instructions only)
	/// @return boolean
	unsafe fn is_broadcast(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.is_broadcast()); }
	}

	unsafe fn set_is_broadcast(lua, this: &mut Instruction, new_value: bool) -> 0 {
		this.inner.set_is_broadcast(new_value);
	}

	/// `true` if eviction hint bit is set (`{eh}`) (MVEX instructions only)
	/// @return boolean
	unsafe fn is_mvex_eviction_hint(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.is_mvex_eviction_hint()); }
	}

	unsafe fn set_is_mvex_eviction_hint(lua, this: &mut Instruction, new_value: bool) -> 0 {
		this.inner.set_is_mvex_eviction_hint(new_value);
	}

	/// (MVEX) Register/memory operand conversion function (an `MvexRegMemConv` enum value)
	/// @return integer # An `MvexRegMemConv` enum value
	unsafe fn mvex_reg_mem_conv(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.mvex_reg_mem_conv() as u32); }
	}

	unsafe fn set_mvex_reg_mem_conv(lua, this: &mut Instruction, new_value: u32) -> 0 {
		this.inner.set_mvex_reg_mem_conv(unsafe { to_mvex_reg_mem_conv(lua, new_value) });
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
	unsafe fn memory_size(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.memory_size() as u32); }
	}

	/// Gets the index register scale value, valid values are `*1`, `*2`, `*4`, `*8`.
	///
	/// Use this method if the operand has kind `OpKind.Memory`
	///
	/// @return integer
	unsafe fn memory_index_scale(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.memory_index_scale()); }
	}

	unsafe fn set_memory_index_scale(lua, this: &mut Instruction, new_value: u32) -> 0 {
		this.inner.set_memory_index_scale(new_value);
	}

	/// Gets the memory operand's displacement or the 64-bit absolute address if it's
	/// an `EIP` or `RIP` relative memory operand.
	///
	/// Use this method if the operand has kind `OpKind.Memory`
	///
	/// @return integer
	unsafe fn memory_displacement(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.memory_displacement64()); }
	}

	unsafe fn set_memory_displacement(lua, this: &mut Instruction, new_value: u64) -> 0 {
		this.inner.set_memory_displacement64(new_value);
	}

	/// Gets an operand's immediate value
	///
	/// @param operand integer # Operand number, 0-4
	/// @return integer # (`u64`) The immediate
	unsafe fn immediate(lua, this: &Instruction, operand: u32) -> 1 {
		let value = match this.inner.try_immediate(operand) {
			Ok(value) => value,
			Err(e) => unsafe { lua.throw_error(e) },
		};
		unsafe { lua.push(value); }
	}

	/// Sets an operand's immediate value
	///
	/// @param operand integer # Operand number, 0-4
	/// @param new_value integer # (`i32`) Immediate
	unsafe fn set_immediate_i32(lua, this: &mut Instruction, operand: u32, new_value: i32) -> 0 {
		match this.inner.try_set_immediate_i32(operand, new_value) {
			Ok(()) => {},
			Err(e) => unsafe { lua.throw_error(e) },
		}
	}

	/// Sets an operand's immediate value
	///
	/// @param operand integer # Operand number, 0-4
	/// @param new_value integer # (`u32`) Immediate
	unsafe fn set_immediate_u32(lua, this: &mut Instruction, operand: u32, new_value: u32) -> 0 {
		match this.inner.try_set_immediate_u32(operand, new_value) {
			Ok(()) => {},
			Err(e) => unsafe { lua.throw_error(e) },
		}
	}

	/// Sets an operand's immediate value
	///
	/// @param operand integer # Operand number, 0-4
	/// @param new_value integer # (`i64`) Immediate
	unsafe fn set_immediate_i64(lua, this: &mut Instruction, operand: u32, new_value: i64) -> 0 {
		match this.inner.try_set_immediate_i64(operand, new_value) {
			Ok(()) => {},
			Err(e) => unsafe { lua.throw_error(e) },
		}
	}

	/// Sets an operand's immediate value
	///
	/// @param operand integer # Operand number, 0-4
	/// @param new_value integer # (`u64`) Immediate
	unsafe fn set_immediate_u64(lua, this: &mut Instruction, operand: u32, new_value: u64) -> 0 {
		match this.inner.try_set_immediate_u64(operand, new_value) {
			Ok(()) => {},
			Err(e) => unsafe { lua.throw_error(e) },
		}
	}

	/// Gets the operand's immediate value.
	///
	/// Use this method if the operand has kind `OpKind.Immediate8`
	///
	/// @return integer
	unsafe fn immediate8(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.immediate8()); }
	}

	unsafe fn set_immediate8(lua, this: &mut Instruction, new_value: u8) -> 0 {
		this.inner.set_immediate8(new_value);
	}

	/// Gets the operand's immediate value.
	///
	/// Use this method if the operand has kind `OpKind.Immediate8_2nd`
	///
	/// @return integer
	unsafe fn immediate8_2nd(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.immediate8_2nd()); }
	}

	unsafe fn set_immediate8_2nd(lua, this: &mut Instruction, new_value: u8) -> 0 {
		this.inner.set_immediate8_2nd(new_value);
	}

	/// Gets the operand's immediate value.
	///
	/// Use this method if the operand has kind `OpKind.Immediate16`
	///
	/// @return integer
	unsafe fn immediate16(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.immediate16()); }
	}

	unsafe fn set_immediate16(lua, this: &mut Instruction, new_value: u16) -> 0 {
		this.inner.set_immediate16(new_value);
	}

	/// Gets the operand's immediate value.
	///
	/// Use this method if the operand has kind `OpKind.Immediate32`
	///
	/// @return integer
	unsafe fn immediate32(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.immediate32()); }
	}

	unsafe fn set_immediate32(lua, this: &mut Instruction, new_value: u32) -> 0 {
		this.inner.set_immediate32(new_value);
	}

	/// Gets the operand's immediate value.
	///
	/// Use this method if the operand has kind `OpKind.Immediate64`
	///
	/// @return integer
	unsafe fn immediate64(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.immediate64()); }
	}

	unsafe fn set_immediate64(lua, this: &mut Instruction, new_value: u64) -> 0 {
		this.inner.set_immediate64(new_value);
	}

	/// Gets the operand's immediate value.
	///
	/// Use this method if the operand has kind `OpKind.Immediate8To16`
	///
	/// @return integer
	unsafe fn immediate8to16(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.immediate8to16()); }
	}

	unsafe fn set_immediate8to16(lua, this: &mut Instruction, new_value: i16) -> 0 {
		this.inner.set_immediate8to16(new_value);
	}

	/// Gets the operand's immediate value.
	///
	/// Use this method if the operand has kind `OpKind.Immediate8To32`
	///
	/// @return integer
	unsafe fn immediate8to32(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.immediate8to32()); }
	}

	unsafe fn set_immediate8to32(lua, this: &mut Instruction, new_value: i32) -> 0 {
		this.inner.set_immediate8to32(new_value);
	}

	/// Gets the operand's immediate value.
	///
	/// Use this method if the operand has kind `OpKind.Immediate8To64`
	///
	/// @return integer
	unsafe fn immediate8to64(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.immediate8to64()); }
	}

	unsafe fn set_immediate8to64(lua, this: &mut Instruction, new_value: i64) -> 0 {
		this.inner.set_immediate8to64(new_value);
	}

	/// Gets the operand's immediate value.
	///
	/// Use this method if the operand has kind `OpKind.Immediate32To64`
	///
	/// @return integer
	unsafe fn immediate32to64(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.immediate32to64()); }
	}

	unsafe fn set_immediate32to64(lua, this: &mut Instruction, new_value: i64) -> 0 {
		this.inner.set_immediate32to64(new_value);
	}

	/// Gets the operand's branch target.
	///
	/// Use this method if the operand has kind `OpKind.NearBranch16`
	///
	/// @return integer
	unsafe fn near_branch16(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.near_branch16()); }
	}

	unsafe fn set_near_branch16(lua, this: &mut Instruction, new_value: u16) -> 0 {
		this.inner.set_near_branch16(new_value);
	}

	/// Gets the operand's branch target.
	///
	/// Use this method if the operand has kind `OpKind.NearBranch32`
	///
	/// @return integer
	unsafe fn near_branch32(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.near_branch32()); }
	}

	unsafe fn set_near_branch32(lua, this: &mut Instruction, new_value: u32) -> 0 {
		this.inner.set_near_branch32(new_value);
	}

	/// Gets the operand's branch target.
	///
	/// Use this method if the operand has kind `OpKind.NearBranch64`
	///
	/// @return integer
	unsafe fn near_branch64(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.near_branch64()); }
	}

	unsafe fn set_near_branch64(lua, this: &mut Instruction, new_value: u64) -> 0 {
		this.inner.set_near_branch64(new_value);
	}

	/// Gets the near branch target if it's a `CALL`/`JMP`/`Jcc` near branch instruction
	///
	/// (i.e., if `Instruction:op0_kind()` is `OpKind.NearBranch16`, `OpKind.NearBranch32` or `OpKind.NearBranch64`)
	///
	/// @return integer
	unsafe fn near_branch_target(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.near_branch_target()); }
	}

	/// Gets the operand's branch target.
	///
	/// Use this method if the operand has kind `OpKind.FarBranch16`
	///
	/// @return integer
	unsafe fn far_branch16(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.far_branch16()); }
	}

	unsafe fn set_far_branch16(lua, this: &mut Instruction, new_value: u16) -> 0 {
		this.inner.set_far_branch16(new_value);
	}

	/// Gets the operand's branch target.
	///
	/// Use this method if the operand has kind `OpKind.FarBranch32`
	///
	/// @return integer
	unsafe fn far_branch32(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.far_branch32()); }
	}

	unsafe fn set_far_branch32(lua, this: &mut Instruction, new_value: u32) -> 0 {
		this.inner.set_far_branch32(new_value);
	}

	/// Gets the operand's branch target selector.
	///
	/// Use this method if the operand has kind `OpKind.FarBranch16` or `OpKind.FarBranch32`
	///
	/// @return integer
	unsafe fn far_branch_selector(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.far_branch_selector()); }
	}

	unsafe fn set_far_branch_selector(lua, this: &mut Instruction, new_value: u16) -> 0 {
		this.inner.set_far_branch_selector(new_value);
	}

	/// Gets the memory operand's base register (a `Register` enum value) or `Register.None` if none.
	///
	/// Use this method if the operand has kind `OpKind.Memory`
	///
	/// @return integer # A `Register` enum value
	unsafe fn memory_base(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.memory_base() as u32); }
	}

	unsafe fn set_memory_base(lua, this: &mut Instruction, new_value: u32) -> 0 {
		this.inner.set_memory_base(unsafe { to_register(lua, new_value) });
	}

	/// Gets the memory operand's index register (a `Register` enum value) or `Register.None` if none.
	///
	/// Use this method if the operand has kind `OpKind.Memory`
	///
	/// @return integer # A `Register` enum value
	unsafe fn memory_index(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.memory_index() as u32); }
	}

	unsafe fn set_memory_index(lua, this: &mut Instruction, new_value: u32) -> 0 {
		this.inner.set_memory_index(unsafe { to_register(lua, new_value) });
	}

	/// Gets operand #0's register value (a `Register` enum value).
	///
	/// Use this method if operand #0 (`Instruction:op0_kind()`) has kind `OpKind.Register`, see `Instruction:op_count()` and `Instruction:op_register()`
	///
	/// @return integer # A `Register` enum value
	unsafe fn op0_register(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.op0_register() as u32); }
	}

	unsafe fn set_op0_register(lua, this: &mut Instruction, new_value: u32) -> 0 {
		this.inner.set_op0_register(unsafe { to_register(lua, new_value) });
	}

	/// Gets operand #1's register value (a `Register` enum value).
	///
	/// Use this method if operand #1 (`Instruction:op0_kind()`) has kind `OpKind.Register`, see `Instruction:op_count()` and `Instruction:op_register()`
	///
	/// @return integer # A `Register` enum value
	unsafe fn op1_register(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.op1_register() as u32); }
	}

	unsafe fn set_op1_register(lua, this: &mut Instruction, new_value: u32) -> 0 {
		this.inner.set_op1_register(unsafe { to_register(lua, new_value) });
	}

	/// Gets operand #2's register value (a `Register` enum value).
	///
	/// Use this method if operand #2 (`Instruction:op0_kind()`) has kind `OpKind.Register`, see `Instruction:op_count()` and `Instruction:op_register()`
	///
	/// @return integer # A `Register` enum value
	unsafe fn op2_register(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.op2_register() as u32); }
	}

	unsafe fn set_op2_register(lua, this: &mut Instruction, new_value: u32) -> 0 {
		this.inner.set_op2_register(unsafe { to_register(lua, new_value) });
	}

	/// Gets operand #3's register value (a `Register` enum value).
	///
	/// Use this method if operand #3 (`Instruction:op0_kind()`) has kind `OpKind.Register`, see `Instruction:op_count()` and `Instruction:op_register()`
	///
	/// @return integer # A `Register` enum value
	unsafe fn op3_register(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.op3_register() as u32); }
	}

	unsafe fn set_op3_register(lua, this: &mut Instruction, new_value: u32) -> 0 {
		this.inner.set_op3_register(unsafe { to_register(lua, new_value) });
	}

	/// Gets operand #4's register value (a `Register` enum value).
	///
	/// Use this method if operand #4 (`Instruction:op0_kind()`) has kind `OpKind.Register`, see `Instruction:op_count()` and `Instruction:op_register()`
	///
	/// @return integer # A `Register` enum value
	unsafe fn op4_register(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.op4_register() as u32); }
	}

	unsafe fn set_op4_register(lua, this: &mut Instruction, new_value: u32) -> 0 {
		match this.inner.try_set_op4_register(unsafe { to_register(lua, new_value) }) {
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
	/// local Decoder = require("iced_x86.Decoder")
	/// local OpKind = require("iced_x86.OpKind")
	/// local Register = require("iced_x86.Register")
	///
	/// -- add [rax],ebx
	/// local data = "\001\024"
	/// local decoder = Decoder.new(64, data)
	/// local instr = decoder:decode()
	///
	/// assert(instr:op_count() == 2)
	/// assert(instr:op_kind(0) == OpKind.Memory)
	/// assert(instr:op_kind(1) == OpKind.Register)
	/// assert(instr:op_register(1) == Register.EBX)
	/// ```
	unsafe fn op_register(lua, this: &Instruction, operand: u32) -> 1 {
		match this.inner.try_op_register(operand) {
			Ok(register) => unsafe { lua.push(register as u32) },
			Err(e) => unsafe { lua.throw_error(e) },
		}
	}

	unsafe fn set_op_register(lua, this: &mut Instruction, operand: u32, new_value: u32) -> 0 {
		match this.inner.try_set_op_register(operand, unsafe { to_register(lua, new_value) }) {
			Ok(()) => {},
			Err(e) => unsafe { lua.throw_error(e) },
		}
	}

	/// Gets the opmask register (`Register.K1` - `Register.K7`) or `Register.None` if none (a `Register` enum value)
	/// @return integer # A `Register` enum value
	unsafe fn op_mask(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.op_mask() as u32); }
	}

	unsafe fn set_op_mask(lua, this: &mut Instruction, new_value: u32) -> 0 {
		this.inner.set_op_mask(unsafe { to_register(lua, new_value) });
	}

	/// Checks if there's an opmask register (`Instruction:op_mask()`)
	/// @return boolean
	unsafe fn has_op_mask(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.has_op_mask()); }
	}

	/// `true` if zeroing-masking, `false` if merging-masking.
	///
	/// Only used by most EVEX encoded instructions that use opmask registers.
	///
	/// @return boolean
	unsafe fn zeroing_masking(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.zeroing_masking()); }
	}

	unsafe fn set_zeroing_masking(lua, this: &mut Instruction, new_value: bool) -> 0 {
		this.inner.set_zeroing_masking(new_value);
	}

	/// `true` if merging-masking, `false` if zeroing-masking.
	///
	/// Only used by most EVEX encoded instructions that use opmask registers.
	///
	/// @return boolean
	unsafe fn merging_masking(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.merging_masking()); }
	}

	unsafe fn set_merging_masking(lua, this: &mut Instruction, new_value: bool) -> 0 {
		this.inner.set_merging_masking(new_value);
	}

	/// Gets the rounding control (a `RoundingControl` enum value) or `RoundingControl.None` if the instruction doesn't use it.
	///
	/// # Note
	/// SAE is implied but `Instruction:suppress_all_exceptions()` still returns `false`.
	///
	/// @return integer # A `RoundingControl` enum value
	unsafe fn rounding_control(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.rounding_control() as u32); }
	}

	unsafe fn set_rounding_control(lua, this: &mut Instruction, new_value: u32) -> 0 {
		this.inner.set_rounding_control(unsafe { to_rounding_control(lua, new_value) });
	}

	/// Gets the number of elements in a `db`/`dw`/`dd`/`dq` directive.
	///
	/// Can only be called if `Instruction:code()` is `Code.DeclareByte`, `Code.DeclareWord`, `Code.DeclareDword`, `Code.DeclareQword`
	///
	/// @return integer
	unsafe fn declare_data_len(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.declare_data_len()); }
	}

	unsafe fn set_declare_data_len(lua, this: &mut Instruction, new_value: usize) -> 0 {
		this.inner.set_declare_data_len(new_value);
	}

	/// Sets a new `db` value, see also `Instruction:declare_data_len()`.
	///
	/// Can only be called if `Instruction:code()` is `Code.DeclareByte`
	///
	/// @param index integer # Index (0-15)
	/// @param new_value integer # (`u8`) New value
	unsafe fn set_declare_byte_value(lua, this: &mut Instruction, index: usize, new_value: u8) -> 0 {
		match this.inner.try_set_declare_byte_value(index, new_value) {
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
	unsafe fn get_declare_byte_value(lua, this: &Instruction, index: usize) -> 1 {
		let value = match this.inner.try_get_declare_byte_value(index) {
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
	unsafe fn get_declare_byte_value_i8(lua, this: &Instruction, index: usize) -> 1 {
		let value = match this.inner.try_get_declare_byte_value(index) {
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
	unsafe fn set_declare_word_value(lua, this: &mut Instruction, index: usize, new_value: u16) -> 0 {
		match this.inner.try_set_declare_word_value(index, new_value) {
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
	unsafe fn get_declare_word_value(lua, this: &Instruction, index: usize) -> 1 {
		let value = match this.inner.try_get_declare_word_value(index) {
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
	unsafe fn get_declare_word_value_i16(lua, this: &Instruction, index: usize) -> 1 {
		let value = match this.inner.try_get_declare_word_value(index) {
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
	unsafe fn set_declare_dword_value(lua, this: &mut Instruction, index: usize, new_value: u32) -> 0 {
		match this.inner.try_set_declare_dword_value(index, new_value) {
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
	unsafe fn get_declare_dword_value(lua, this: &Instruction, index: usize) -> 1 {
		let value = match this.inner.try_get_declare_dword_value(index) {
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
	unsafe fn get_declare_dword_value_i32(lua, this: &Instruction, index: usize) -> 1 {
		let value = match this.inner.try_get_declare_dword_value(index) {
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
	unsafe fn set_declare_qword_value(lua, this: &mut Instruction, index: usize, new_value: u64) -> 0 {
		match this.inner.try_set_declare_qword_value(index, new_value) {
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
	unsafe fn get_declare_qword_value(lua, this: &Instruction, index: usize) -> 1 {
		let value = match this.inner.try_get_declare_qword_value(index) {
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
	unsafe fn get_declare_qword_value_i64(lua, this: &Instruction, index: usize) -> 1 {
		let value = match this.inner.try_get_declare_qword_value(index) {
			Ok(value) => value,
			Err(e) => unsafe { lua.throw_error(e) },
		};
		unsafe { lua.push(value as i64); }
	}

	/// Checks if this is a VSIB instruction, see also `Instruction:is_vsib32()`, `Instruction:is_vsib64()`
	/// @return boolean
	unsafe fn is_vsib(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.is_vsib()); }
	}

	/// VSIB instructions only (`Instruction:is_vsib()`): `true` if it's using 32-bit indexes, `false` if it's using 64-bit indexes
	/// @return boolean
	unsafe fn is_vsib32(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.is_vsib32()); }
	}

	/// VSIB instructions only (`Instruction:is_vsib()`): `true` if it's using 64-bit indexes, `false` if it's using 32-bit indexes
	/// @return boolean
	unsafe fn is_vsib64(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.is_vsib64()); }
	}

	/// Checks if it's a vsib instruction.
	///
	/// - Returns `true` if it's a VSIB instruction with 64-bit indexes
	/// - Returns `false` if it's a VSIB instruction with 32-bit indexes
	/// - Returns `nil` if it's not a VSIB instruction.
	///
	/// @return boolean|nil
	unsafe fn vsib(lua, this: &Instruction) -> 1 {
		match this.inner.vsib() {
			Some(b) => unsafe { lua.push(b) },
			None => unsafe { lua.push(Nil) },
		}
	}

	/// Gets the suppress all exceptions flag (EVEX/MVEX encoded instructions). Note that if `Instruction:rounding_control()` is not `RoundingControl.None`, SAE is implied but this method will still return `false`.
	/// @return boolean
	unsafe fn suppress_all_exceptions(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.suppress_all_exceptions()); }
	}

	unsafe fn set_suppress_all_exceptions(lua, this: &mut Instruction, new_value: bool) -> 0 {
		this.inner.set_suppress_all_exceptions(new_value);
	}

	/// Checks if the memory operand is `RIP`/`EIP` relative
	/// @return boolean
	unsafe fn is_ip_rel_memory_operand(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.is_ip_rel_memory_operand()); }
	}

	/// Gets the `RIP`/`EIP` releative address (`Instruction:memory_displacement()`).
	///
	/// This method is only valid if there's a memory operand with `RIP`/`EIP` relative addressing, see `Instruction:is_ip_rel_memory_operand()`
	///
	/// @return integer
	unsafe fn ip_rel_memory_address(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.ip_rel_memory_address()); }
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
	/// local Decoder = require("iced_x86.Decoder")
	///
	/// -- pushfq
	/// local data = "\156"
	/// local decoder = Decoder.new(64, data)
	/// local instr = decoder:decode()
	///
	/// assert(instr:is_stack_instruction())
	/// assert(instr:stack_pointer_increment() == -8)
	/// ```
	unsafe fn stack_pointer_increment(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.stack_pointer_increment()); }
	}

	/// Gets the FPU status word's `TOP` increment value and whether it's a conditional or unconditional push/pop and whether `TOP` is written.
	///
	/// @return FpuStackIncrementInfo # FPU stack info
	///
	/// # Examples
	/// ```lua
	/// local Decoder = require("iced_x86.Decoder")
	///
	/// -- ficomp dword ptr [rax]
	/// local data = "\218\024"
	/// local decoder = Decoder.new(64, data)
	/// local instr = decoder:decode()
	///
	/// local info = instr:fpu_stack_increment_info()
	/// -- It pops the stack once
	/// assert(info:increment() == 1)
	/// assert(not info:conditional())
	/// assert(info:writes_top())
	/// ```
	unsafe fn fpu_stack_increment_info(lua, this: &Instruction) -> 1 {
		unsafe { let _ = FpuStackIncrementInfo::init_and_push_iced(lua, &this.inner.fpu_stack_increment_info()); }
	}

	/// Instruction encoding, eg. Legacy, 3DNow!, VEX, EVEX, XOP (an `EncodingKind` enum value)
	/// @return integer # An `EncodingKind` enum value
	///
	/// # Examples
	/// ```lua
	/// local Decoder = require("iced_x86.Decoder")
	/// local EncodingKind = require("iced_x86.EncodingKind")
	///
	/// -- vmovaps xmm1,xmm5
	/// local data = "\197\248\040\205"
	/// local decoder = Decoder.new(64, data)
	/// local instr = decoder:decode()
	///
	/// assert(instr:encoding() == EncodingKind.VEX)
	/// ```
	unsafe fn encoding(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.encoding() as u32); }
	}

	/// Gets the CPU or CPUID feature flags (an array of `CpuidFeature` enum values)
	///
	/// @return integer[] # (A `CpuidFeature` array) CPU or CPUID feature flags
	///
	/// # Examples
	/// ```lua
	/// local Decoder = require("iced_x86.Decoder")
	/// local CpuidFeature = require("iced_x86.CpuidFeature")
	///
	/// -- vmovaps xmm1,xmm5
	/// -- vmovaps xmm10{k3}{z},xmm19
	/// local data = "\197\248\040\205\098\049\124\139\040\211"
	/// local decoder = Decoder.new(64, data)
	///
	/// -- vmovaps xmm1,xmm5
	/// local instr = decoder:decode()
	/// local cpuid = instr:cpuid_features()
	/// assert(#cpuid == 1)
	/// assert(cpuid[1] == CpuidFeature.AVX)
	///
	/// -- vmovaps xmm10{k3}{z},xmm19
	/// local instr2 = decoder:decode()
	/// local cpuid2 = instr2:cpuid_features()
	/// assert(#cpuid2 == 2)
	/// assert(cpuid2[1] == CpuidFeature.AVX512VL)
	/// assert(cpuid2[2] == CpuidFeature.AVX512F)
	/// ```
	unsafe fn cpuid_features(lua, this: &Instruction) -> 1 {
		let cpuid_features = this.inner.cpuid_features();
		unsafe { lua.push_array(cpuid_features, |_, cpuid| *cpuid as u32); }
	}

	/// Control flow info (a `FlowControl` enum value)
	/// @return integer # A `FlowControl` enum value
	///
	/// # Examples
	/// ```lua
	/// local Decoder = require("iced_x86.Decoder")
	/// local FlowControl = require("iced_x86.FlowControl")
	///
	/// -- or ecx,esi
	/// -- ud0 rcx,rsi
	/// -- call rcx
	/// local data = "\011\206\072\015\255\206\255\209"
	/// local decoder = Decoder.new(64, data)
	///
	/// -- or ecx,esi
	/// local instr = decoder:decode()
	/// assert(instr:flow_control() == FlowControl.Next)
	///
	/// -- ud0 rcx,rsi
	/// local instr2 = decoder:decode()
	/// assert(instr2:flow_control() == FlowControl.Exception)
	///
	/// -- call rcx
	/// local instr3 = decoder:decode()
	/// assert(instr3:flow_control() == FlowControl.IndirectCall)
	/// ```
	unsafe fn flow_control(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.flow_control() as u32); }
	}

	/// `true` if it's a privileged instruction (all CPL=0 instructions (except `VMCALL`) and IOPL instructions `IN`, `INS`, `OUT`, `OUTS`, `CLI`, `STI`)
	/// @return boolean
	unsafe fn is_privileged(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.is_privileged()); }
	}

	/// `true` if this is an instruction that implicitly uses the stack pointer (`SP`/`ESP`/`RSP`), eg. `CALL`, `PUSH`, `POP`, `RET`, etc.
	///
	/// See also `Instruction:stack_pointer_increment()`
	///
	/// @return boolean
	///
	/// # Examples
	/// ```lua
	/// local Decoder = require("iced_x86.Decoder")
	///
	/// -- or ecx,esi
	/// -- push rax
	/// local data = "\011\206\080"
	/// local decoder = Decoder.new(64, data)
	///
	/// -- or ecx,esi
	/// local instr = decoder:decode()
	/// assert(not instr:is_stack_instruction())
	///
	/// -- push rax
	/// local instr2 = decoder:decode()
	/// assert(instr2:is_stack_instruction())
	/// assert(instr2:stack_pointer_increment() == -8)
	/// ```
	unsafe fn is_stack_instruction(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.is_stack_instruction()); }
	}

	/// `true` if it's an instruction that saves or restores too many registers (eg. `FXRSTOR`, `XSAVE`, etc).
	/// @return boolean
	unsafe fn is_save_restore_instruction(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.is_save_restore_instruction()); }
	}

	/// `true` if it's a "string" instruction, such as `MOVS`, `LODS`, `SCAS`, etc.
	/// @return boolean
	unsafe fn is_string_instruction(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.is_string_instruction()); }
	}

	/// All flags that are read by the CPU when executing the instruction.
	///
	/// This method returns an `RflagsBits` value. See also `Instruction:rflags_modified()`.
	///
	/// @return integer # An `RflagsBits` enum value
	///
	/// # Examples
	/// ```lua
	/// local Decoder = require("iced_x86.Decoder")
	/// local RflagsBits = require("iced_x86.RflagsBits")
	///
	/// -- adc rsi,rcx
	/// -- xor rdi,5Ah
	/// local data = "\072\017\206\072\131\247\090"
	/// local decoder = Decoder.new(64, data)
	///
	/// -- adc rsi,rcx
	/// local instr = decoder:decode()
	/// assert(instr:rflags_read() == RflagsBits.CF)
	/// assert(instr:rflags_written() == RflagsBits.OF + RflagsBits.SF + RflagsBits.ZF + RflagsBits.AF + RflagsBits.CF + RflagsBits.PF)
	/// assert(instr:rflags_cleared() == RflagsBits.None)
	/// assert(instr:rflags_set() == RflagsBits.None)
	/// assert(instr:rflags_undefined() == RflagsBits.None)
	/// assert(instr:rflags_modified() == RflagsBits.OF + RflagsBits.SF + RflagsBits.ZF + RflagsBits.AF + RflagsBits.CF + RflagsBits.PF)
	///
	/// -- xor rdi,5Ah
	/// local instr2 = decoder:decode()
	/// assert(instr2:rflags_read() == RflagsBits.None)
	/// assert(instr2:rflags_written() == RflagsBits.SF + RflagsBits.ZF + RflagsBits.PF)
	/// assert(instr2:rflags_cleared() == RflagsBits.OF + RflagsBits.CF)
	/// assert(instr2:rflags_set() == RflagsBits.None)
	/// assert(instr2:rflags_undefined() == RflagsBits.AF)
	/// assert(instr2:rflags_modified() == RflagsBits.OF + RflagsBits.SF + RflagsBits.ZF + RflagsBits.AF + RflagsBits.CF + RflagsBits.PF)
	/// ```
	unsafe fn rflags_read(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.rflags_read()); }
	}

	/// All flags that are written by the CPU, except those flags that are known to be undefined, always set or always cleared.
	///
	/// This method returns an `RflagsBits` value. See also `Instruction:rflags_modified()`.
	///
	/// @return integer # An `RflagsBits` enum value
	///
	/// # Examples
	/// ```lua
	/// local Decoder = require("iced_x86.Decoder")
	/// local RflagsBits = require("iced_x86.RflagsBits")
	///
	/// -- adc rsi,rcx
	/// -- xor rdi,5Ah
	/// local data = "\072\017\206\072\131\247\090"
	/// local decoder = Decoder.new(64, data)
	///
	/// -- adc rsi,rcx
	/// local instr = decoder:decode()
	/// assert(instr:rflags_read() == RflagsBits.CF)
	/// assert(instr:rflags_written() == RflagsBits.OF + RflagsBits.SF + RflagsBits.ZF + RflagsBits.AF + RflagsBits.CF + RflagsBits.PF)
	/// assert(instr:rflags_cleared() == RflagsBits.None)
	/// assert(instr:rflags_set() == RflagsBits.None)
	/// assert(instr:rflags_undefined() == RflagsBits.None)
	/// assert(instr:rflags_modified() == RflagsBits.OF + RflagsBits.SF + RflagsBits.ZF + RflagsBits.AF + RflagsBits.CF + RflagsBits.PF)
	///
	/// -- xor rdi,5Ah
	/// local instr2 = decoder:decode()
	/// assert(instr2:rflags_read() == RflagsBits.None)
	/// assert(instr2:rflags_written() == RflagsBits.SF + RflagsBits.ZF + RflagsBits.PF)
	/// assert(instr2:rflags_cleared() == RflagsBits.OF + RflagsBits.CF)
	/// assert(instr2:rflags_set() == RflagsBits.None)
	/// assert(instr2:rflags_undefined() == RflagsBits.AF)
	/// assert(instr2:rflags_modified() == RflagsBits.OF + RflagsBits.SF + RflagsBits.ZF + RflagsBits.AF + RflagsBits.CF + RflagsBits.PF)
	/// ```
	unsafe fn rflags_written(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.rflags_written()); }
	}

	/// All flags that are always cleared by the CPU.
	///
	/// This method returns an `RflagsBits` value. See also `Instruction:rflags_modified()`.
	///
	/// @return integer # An `RflagsBits` enum value
	///
	/// # Examples
	/// ```lua
	/// local Decoder = require("iced_x86.Decoder")
	/// local RflagsBits = require("iced_x86.RflagsBits")
	///
	/// -- adc rsi,rcx
	/// -- xor rdi,5Ah
	/// local data = "\072\017\206\072\131\247\090"
	/// local decoder = Decoder.new(64, data)
	///
	/// -- adc rsi,rcx
	/// local instr = decoder:decode()
	/// assert(instr:rflags_read() == RflagsBits.CF)
	/// assert(instr:rflags_written() == RflagsBits.OF + RflagsBits.SF + RflagsBits.ZF + RflagsBits.AF + RflagsBits.CF + RflagsBits.PF)
	/// assert(instr:rflags_cleared() == RflagsBits.None)
	/// assert(instr:rflags_set() == RflagsBits.None)
	/// assert(instr:rflags_undefined() == RflagsBits.None)
	/// assert(instr:rflags_modified() == RflagsBits.OF + RflagsBits.SF + RflagsBits.ZF + RflagsBits.AF + RflagsBits.CF + RflagsBits.PF)
	///
	/// -- xor rdi,5Ah
	/// local instr2 = decoder:decode()
	/// assert(instr2:rflags_read() == RflagsBits.None)
	/// assert(instr2:rflags_written() == RflagsBits.SF + RflagsBits.ZF + RflagsBits.PF)
	/// assert(instr2:rflags_cleared() == RflagsBits.OF + RflagsBits.CF)
	/// assert(instr2:rflags_set() == RflagsBits.None)
	/// assert(instr2:rflags_undefined() == RflagsBits.AF)
	/// assert(instr2:rflags_modified() == RflagsBits.OF + RflagsBits.SF + RflagsBits.ZF + RflagsBits.AF + RflagsBits.CF + RflagsBits.PF)
	/// ```
	unsafe fn rflags_cleared(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.rflags_cleared()); }
	}

	/// All flags that are always set by the CPU.
	///
	/// This method returns an `RflagsBits` value. See also `Instruction:rflags_modified()`.
	///
	/// @return integer # An `RflagsBits` enum value
	///
	/// # Examples
	/// ```lua
	/// local Decoder = require("iced_x86.Decoder")
	/// local RflagsBits = require("iced_x86.RflagsBits")
	///
	/// -- adc rsi,rcx
	/// -- xor rdi,5Ah
	/// local data = "\072\017\206\072\131\247\090"
	/// local decoder = Decoder.new(64, data)
	///
	/// -- adc rsi,rcx
	/// local instr = decoder:decode()
	/// assert(instr:rflags_read() == RflagsBits.CF)
	/// assert(instr:rflags_written() == RflagsBits.OF + RflagsBits.SF + RflagsBits.ZF + RflagsBits.AF + RflagsBits.CF + RflagsBits.PF)
	/// assert(instr:rflags_cleared() == RflagsBits.None)
	/// assert(instr:rflags_set() == RflagsBits.None)
	/// assert(instr:rflags_undefined() == RflagsBits.None)
	/// assert(instr:rflags_modified() == RflagsBits.OF + RflagsBits.SF + RflagsBits.ZF + RflagsBits.AF + RflagsBits.CF + RflagsBits.PF)
	///
	/// -- xor rdi,5Ah
	/// local instr2 = decoder:decode()
	/// assert(instr2:rflags_read() == RflagsBits.None)
	/// assert(instr2:rflags_written() == RflagsBits.SF + RflagsBits.ZF + RflagsBits.PF)
	/// assert(instr2:rflags_cleared() == RflagsBits.OF + RflagsBits.CF)
	/// assert(instr2:rflags_set() == RflagsBits.None)
	/// assert(instr2:rflags_undefined() == RflagsBits.AF)
	/// assert(instr2:rflags_modified() == RflagsBits.OF + RflagsBits.SF + RflagsBits.ZF + RflagsBits.AF + RflagsBits.CF + RflagsBits.PF)
	/// ```
	unsafe fn rflags_set(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.rflags_set()); }
	}

	/// All flags that are undefined after executing the instruction.
	///
	/// This method returns an `RflagsBits` value. See also `Instruction:rflags_modified()`.
	///
	/// @return integer # An `RflagsBits` enum value
	///
	/// # Examples
	/// ```lua
	/// local Decoder = require("iced_x86.Decoder")
	/// local RflagsBits = require("iced_x86.RflagsBits")
	///
	/// -- adc rsi,rcx
	/// -- xor rdi,5Ah
	/// local data = "\072\017\206\072\131\247\090"
	/// local decoder = Decoder.new(64, data)
	///
	/// -- adc rsi,rcx
	/// local instr = decoder:decode()
	/// assert(instr:rflags_read() == RflagsBits.CF)
	/// assert(instr:rflags_written() == RflagsBits.OF + RflagsBits.SF + RflagsBits.ZF + RflagsBits.AF + RflagsBits.CF + RflagsBits.PF)
	/// assert(instr:rflags_cleared() == RflagsBits.None)
	/// assert(instr:rflags_set() == RflagsBits.None)
	/// assert(instr:rflags_undefined() == RflagsBits.None)
	/// assert(instr:rflags_modified() == RflagsBits.OF + RflagsBits.SF + RflagsBits.ZF + RflagsBits.AF + RflagsBits.CF + RflagsBits.PF)
	///
	/// -- xor rdi,5Ah
	/// local instr2 = decoder:decode()
	/// assert(instr2:rflags_read() == RflagsBits.None)
	/// assert(instr2:rflags_written() == RflagsBits.SF + RflagsBits.ZF + RflagsBits.PF)
	/// assert(instr2:rflags_cleared() == RflagsBits.OF + RflagsBits.CF)
	/// assert(instr2:rflags_set() == RflagsBits.None)
	/// assert(instr2:rflags_undefined() == RflagsBits.AF)
	/// assert(instr2:rflags_modified() == RflagsBits.OF + RflagsBits.SF + RflagsBits.ZF + RflagsBits.AF + RflagsBits.CF + RflagsBits.PF)
	/// ```
	unsafe fn rflags_undefined(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.rflags_undefined()); }
	}

	/// All flags that are modified by the CPU. This is `rflags_written + rflags_cleared + rflags_set + rflags_undefined`.
	///
	/// This method returns an `RflagsBits` value.
	///
	/// @return integer # An `RflagsBits` enum value
	///
	/// # Examples
	/// ```lua
	/// local Decoder = require("iced_x86.Decoder")
	/// local RflagsBits = require("iced_x86.RflagsBits")
	///
	/// -- adc rsi,rcx
	/// -- xor rdi,5Ah
	/// local data = "\072\017\206\072\131\247\090"
	/// local decoder = Decoder.new(64, data)
	///
	/// -- adc rsi,rcx
	/// local instr = decoder:decode()
	/// assert(instr:rflags_read() == RflagsBits.CF)
	/// assert(instr:rflags_written() == RflagsBits.OF + RflagsBits.SF + RflagsBits.ZF + RflagsBits.AF + RflagsBits.CF + RflagsBits.PF)
	/// assert(instr:rflags_cleared() == RflagsBits.None)
	/// assert(instr:rflags_set() == RflagsBits.None)
	/// assert(instr:rflags_undefined() == RflagsBits.None)
	/// assert(instr:rflags_modified() == RflagsBits.OF + RflagsBits.SF + RflagsBits.ZF + RflagsBits.AF + RflagsBits.CF + RflagsBits.PF)
	///
	/// -- xor rdi,5Ah
	/// local instr2 = decoder:decode()
	/// assert(instr2:rflags_read() == RflagsBits.None)
	/// assert(instr2:rflags_written() == RflagsBits.SF + RflagsBits.ZF + RflagsBits.PF)
	/// assert(instr2:rflags_cleared() == RflagsBits.OF + RflagsBits.CF)
	/// assert(instr2:rflags_set() == RflagsBits.None)
	/// assert(instr2:rflags_undefined() == RflagsBits.AF)
	/// assert(instr2:rflags_modified() == RflagsBits.OF + RflagsBits.SF + RflagsBits.ZF + RflagsBits.AF + RflagsBits.CF + RflagsBits.PF)
	/// ```
	unsafe fn rflags_modified(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.rflags_modified()); }
	}

	/// Checks if it's a `Jcc SHORT` or `Jcc NEAR` instruction
	/// @return boolean
	unsafe fn is_jcc_short_or_near(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.is_jcc_short_or_near()); }
	}

	/// Checks if it's a `Jcc NEAR` instruction
	/// @return boolean
	unsafe fn is_jcc_near(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.is_jcc_near()); }
	}

	/// Checks if it's a `Jcc SHORT` instruction
	/// @return boolean
	unsafe fn is_jcc_short(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.is_jcc_short()); }
	}

	/// Checks if it's a `JMP SHORT` instruction
	/// @return boolean
	unsafe fn is_jmp_short(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.is_jmp_short()); }
	}

	/// Checks if it's a `JMP NEAR` instruction
	/// @return boolean
	unsafe fn is_jmp_near(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.is_jmp_near()); }
	}

	/// Checks if it's a `JMP SHORT` or a `JMP NEAR` instruction
	/// @return boolean
	unsafe fn is_jmp_short_or_near(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.is_jmp_short_or_near()); }
	}

	/// Checks if it's a `JMP FAR` instruction
	/// @return boolean
	unsafe fn is_jmp_far(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.is_jmp_far()); }
	}

	/// Checks if it's a `CALL NEAR` instruction
	/// @return boolean
	unsafe fn is_call_near(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.is_call_near()); }
	}

	/// Checks if it's a `CALL FAR` instruction
	/// @return boolean
	unsafe fn is_call_far(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.is_call_far()); }
	}

	/// Checks if it's a `JMP NEAR reg/[mem]` instruction
	/// @return boolean
	unsafe fn is_jmp_near_indirect(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.is_jmp_near_indirect()); }
	}

	/// Checks if it's a `JMP FAR [mem]` instruction
	/// @return boolean
	unsafe fn is_jmp_far_indirect(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.is_jmp_far_indirect()); }
	}

	/// Checks if it's a `CALL NEAR reg/[mem]` instruction
	/// @return boolean
	unsafe fn is_call_near_indirect(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.is_call_near_indirect()); }
	}

	/// Checks if it's a `CALL FAR [mem]` instruction
	/// @return boolean
	unsafe fn is_call_far_indirect(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.is_call_far_indirect()); }
	}

	/// Checks if it's a `JKccD SHORT` or `JKccD NEAR` instruction
	/// @return boolean
	unsafe fn is_jkcc_short_or_near(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.is_jkcc_short_or_near()); }
	}

	/// Checks if it's a `JKccD NEAR` instruction
	/// @return boolean
	unsafe fn is_jkcc_near(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.is_jkcc_near()); }
	}

	/// Checks if it's a `JKccD SHORT` instruction
	/// @return boolean
	unsafe fn is_jkcc_short(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.is_jkcc_short()); }
	}

	/// Checks if it's a `JCXZ SHORT`, `JECXZ SHORT` or `JRCXZ SHORT` instruction
	/// @return boolean
	unsafe fn is_jcx_short(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.code().is_jcx_short()); }
	}

	/// Checks if it's a `LOOPcc SHORT` instruction
	/// @return boolean
	unsafe fn is_loopcc(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.code().is_loopcc()); }
	}

	/// Checks if it's a `LOOP SHORT` instruction
	/// @return boolean
	unsafe fn is_loop(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.code().is_loop()); }
	}

	/// Negates the condition code, eg. `JE` -> `JNE`.
	///
	/// Can be used if it's `Jcc`, `SETcc`, `CMOVcc`, `CMPccXADD`, `LOOPcc` and does nothing if the instruction doesn't have a condition code.
	///
	/// # Examples
	/// ```lua
	/// local Code = require("iced_x86.Code")
	/// local ConditionCode = require("iced_x86.ConditionCode")
	/// local Decoder = require("iced_x86.Decoder")
	///
	/// -- setbe al
	/// local data = "\015\150\192"
	/// local decoder = Decoder.new(64, data)
	///
	/// local instr = decoder:decode()
	/// assert(instr:code() == Code.Setbe_rm8)
	/// assert(instr:condition_code() == ConditionCode.be)
	/// instr:negate_condition_code()
	/// assert(instr:code() == Code.Seta_rm8)
	/// assert(instr:condition_code() == ConditionCode.a)
	/// ```
	unsafe fn negate_condition_code(lua, this: &mut Instruction) -> 0 {
		this.inner.negate_condition_code();
	}

	/// Converts `Jcc/JMP NEAR` to `Jcc/JMP SHORT` and does nothing if it's not a `Jcc/JMP NEAR` instruction
	///
	/// # Examples
	/// ```lua
	/// local Code = require("iced_x86.Code")
	/// local Decoder = require("iced_x86.Decoder")
	///
	/// -- jbe near ptr label
	/// local data = "\015\134\090\165\018\052"
	/// local decoder = Decoder.new(64, data)
	///
	/// local instr = decoder:decode()
	/// assert(instr:code() == Code.Jbe_rel32_64)
	/// instr:as_short_branch()
	/// assert(instr:code() == Code.Jbe_rel8_64)
	/// instr:as_short_branch()
	/// assert(instr:code() == Code.Jbe_rel8_64)
	/// ```
	unsafe fn as_short_branch(lua, this: &mut Instruction) -> 0 {
		this.inner.as_short_branch();
	}

	/// Converts `Jcc/JMP SHORT` to `Jcc/JMP NEAR` and does nothing if it's not a `Jcc/JMP SHORT` instruction
	///
	/// # Examples
	/// ```lua
	/// local Code = require("iced_x86.Code")
	/// local Decoder = require("iced_x86.Decoder")
	///
	/// -- jbe short label
	/// local data = "\118\090"
	/// local decoder = Decoder.new(64, data)
	///
	/// local instr = decoder:decode()
	/// assert(instr:code() == Code.Jbe_rel8_64)
	/// instr:as_near_branch()
	/// assert(instr:code() == Code.Jbe_rel32_64)
	/// instr:as_near_branch()
	/// assert(instr:code() == Code.Jbe_rel32_64)
	/// ```
	unsafe fn as_near_branch(lua, this: &mut Instruction) -> 0 {
		this.inner.as_near_branch();
	}

	/// Gets the condition code (a `ConditionCode` enum value) if it's `Jcc`, `SETcc`, `CMOVcc`, `CMPccXADD`, `LOOPcc` else `ConditionCode.None` is returned
	/// @return integer # A `ConditionCode` enum value
	///
	/// # Examples
	/// ```lua
	/// local ConditionCode = require("iced_x86.ConditionCode")
	/// local Decoder = require("iced_x86.Decoder")
	///
	/// -- setbe al
	/// -- jl short label
	/// -- cmovne ecx,esi
	/// -- nop
	/// local data = "\015\150\192\124\090\015\069\206\144"
	/// local decoder = Decoder.new(64, data)
	///
	/// -- setbe al
	/// local instr = decoder:decode()
	/// assert(instr:condition_code() == ConditionCode.be)
	///
	/// -- jl short label
	/// local instr2 = decoder:decode()
	/// assert(instr2:condition_code() == ConditionCode.l)
	///
	/// -- cmovne ecx,esi
	/// local instr3 = decoder:decode()
	/// assert(instr3:condition_code() == ConditionCode.ne)
	///
	/// -- nop
	/// local instr4 = decoder:decode()
	/// assert(instr4:condition_code() == ConditionCode.None)
	/// ```
	unsafe fn condition_code(lua, this: &Instruction) -> 1 {
		unsafe { lua.push(this.inner.condition_code() as u32); }
	}

	/// Gets the `OpCodeInfo`
	///
	/// @return OpCodeInfo # Op code info
	unsafe fn op_code(lua, this: &Instruction) -> 1 {
		unsafe { let _ = OpCodeInfo::push_new(lua, this.inner.code()); }
	}

	/// Gets all used registers
	///
	/// See also `Instruction:used_memory()`, `Instruction:op_accesses()`, `Instruction:used_regs_mem()`, `Instruction:used_values()`
	///
	/// @return UsedRegister[] # All used registers
	///
	/// # Examples
	/// ```lua
	/// local Decoder = require("iced_x86.Decoder")
	/// local OpAccess = require("iced_x86.OpAccess")
	/// local Register = require("iced_x86.Register")
	///
	/// local decoder = Decoder.new(64, "\196\227\073\072\016\065")
	/// local instr = decoder:decode()
	///
	/// local used_registers = instr:used_registers()
	/// assert(#used_registers == 4)
	/// assert(used_registers[1]:register() == Register.ZMM2)
	/// assert(used_registers[1]:access() == OpAccess.Write)
	/// assert(used_registers[2]:register() == Register.XMM6)
	/// assert(used_registers[2]:access() == OpAccess.Read)
	/// assert(used_registers[3]:register() == Register.RAX)
	/// assert(used_registers[3]:access() == OpAccess.Read)
	/// assert(used_registers[4]:register() == Register.XMM4)
	/// assert(used_registers[4]:access() == OpAccess.Read)
	/// ```
	unsafe fn used_registers(lua, this: &Instruction) -> 1 {
		let mut factory = iced_x86::InstructionInfoFactory::new();
		let info = factory.info_options(&this.inner, iced_x86::InstructionInfoOptions::NO_MEMORY_USAGE);
		unsafe { push_used_registers(lua, info); }
	}

	/// Gets all used memory locations
	///
	/// See also `Instruction:used_registers()`, `Instruction:op_accesses()`, `Instruction:used_regs_mem()`, `Instruction:used_values()`
	///
	/// @return UsedMemory[] # All used memory locations
	///
	/// # Examples
	/// ```lua
	/// local CodeSize = require("iced_x86.CodeSize")
	/// local Decoder = require("iced_x86.Decoder")
	/// local MemorySize = require("iced_x86.MemorySize")
	/// local OpAccess = require("iced_x86.OpAccess")
	/// local Register = require("iced_x86.Register")
	///
	/// local decoder = Decoder.new(64, "\196\227\073\072\016\065")
	/// local instr = decoder:decode()
	///
	/// local used_memory = instr:used_memory()
	/// assert(#used_memory == 1)
	/// local mem = used_memory[1]
	/// assert(mem:segment() == Register.DS)
	/// assert(mem:base() == Register.RAX)
	/// assert(mem:index() == Register.None)
	/// assert(mem:scale() == 1)
	/// assert(mem:displacement() == 0)
	/// assert(mem:memory_size() == MemorySize.Packed128_Float32)
	/// assert(mem:access() == OpAccess.Read)
	/// assert(mem:address_size() == CodeSize.Code64)
	/// assert(mem:vsib_size() == 0)
	/// ```
	unsafe fn used_memory(lua, this: &Instruction) -> 1 {
		let mut factory = iced_x86::InstructionInfoFactory::new();
		let info = factory.info_options(&this.inner, iced_x86::InstructionInfoOptions::NO_REGISTER_USAGE);
		unsafe { push_used_memory(lua, info); }
	}

	/// Gets all operand accesses (Array of `OpAccess` values)
	///
	/// See also `Instruction:used_registers()`, `Instruction:used_memory()`, `Instruction:used_regs_mem()`, `Instruction:used_values()`
	///
	/// @return integer[] # Array of `OpAccess` values
	///
	/// # Examples
	/// ```lua
	/// local Decoder = require("iced_x86.Decoder")
	/// local OpAccess = require("iced_x86.OpAccess")
	///
	/// local decoder = Decoder.new(64, "\196\227\073\072\016\065")
	/// local instr = decoder:decode()
	///
	/// local op_accesses = instr:op_accesses()
	/// assert(#op_accesses == 5)
	/// assert(op_accesses[1] == OpAccess.Write)
	/// assert(op_accesses[2] == OpAccess.Read)
	/// assert(op_accesses[3] == OpAccess.Read)
	/// assert(op_accesses[4] == OpAccess.Read)
	/// assert(op_accesses[5] == OpAccess.Read)
	/// ```
	unsafe fn op_accesses(lua, this: &Instruction) -> 1 {
		let mut factory = iced_x86::InstructionInfoFactory::new();
		let info = factory.info_options(&this.inner, iced_x86::InstructionInfoOptions::NO_MEMORY_USAGE | iced_x86::InstructionInfoOptions::NO_REGISTER_USAGE);
		unsafe { push_op_accesses(lua, info, &this.inner); }
	}

	/// Gets all used registers and all used memory locations
	///
	/// See also `Instruction:used_registers()`, `Instruction:used_memory()`, `Instruction:op_accesses()`, `Instruction:used_values()`
	///
	/// @return UsedRegister[], UsedMemory[] # Used registers, used memory locations
	///
	/// # Examples
	/// ```lua
	/// local CodeSize = require("iced_x86.CodeSize")
	/// local Decoder = require("iced_x86.Decoder")
	/// local MemorySize = require("iced_x86.MemorySize")
	/// local OpAccess = require("iced_x86.OpAccess")
	/// local Register = require("iced_x86.Register")
	///
	/// local decoder = Decoder.new(64, "\196\227\073\072\016\065")
	/// local instr = decoder:decode()
	///
	/// local used_registers, used_memory = instr:used_regs_mem()
	///
	/// assert(#used_registers == 4)
	/// assert(used_registers[1]:register() == Register.ZMM2)
	/// assert(used_registers[1]:access() == OpAccess.Write)
	/// assert(used_registers[2]:register() == Register.XMM6)
	/// assert(used_registers[2]:access() == OpAccess.Read)
	/// assert(used_registers[3]:register() == Register.RAX)
	/// assert(used_registers[3]:access() == OpAccess.Read)
	/// assert(used_registers[4]:register() == Register.XMM4)
	/// assert(used_registers[4]:access() == OpAccess.Read)
	///
	/// assert(#used_memory == 1)
	/// local mem = used_memory[1]
	/// assert(mem:segment() == Register.DS)
	/// assert(mem:base() == Register.RAX)
	/// assert(mem:index() == Register.None)
	/// assert(mem:scale() == 1)
	/// assert(mem:displacement() == 0)
	/// assert(mem:memory_size() == MemorySize.Packed128_Float32)
	/// assert(mem:access() == OpAccess.Read)
	/// assert(mem:address_size() == CodeSize.Code64)
	/// assert(mem:vsib_size() == 0)
	/// ```
	unsafe fn used_regs_mem(lua, this: &Instruction) -> 2 {
		let mut factory = iced_x86::InstructionInfoFactory::new();
		let info = factory.info_options(&this.inner, iced_x86::InstructionInfoOptions::NONE);
		unsafe {
			push_used_registers(lua, info);
			push_used_memory(lua, info);
		}
	}

	/// Gets all used registers, all used memory locations and all operand accesses
	///
	/// See also `Instruction:used_registers()`, `Instruction:used_memory()`, `Instruction:op_accesses()`, `Instruction:used_regs_mem()`
	///
	/// @return UsedRegister[], UsedMemory[], integer[] # Used registers, used memory locations and `OpAccess`[]
	///
	/// # Examples
	/// ```lua
	/// local CodeSize = require("iced_x86.CodeSize")
	/// local Decoder = require("iced_x86.Decoder")
	/// local MemorySize = require("iced_x86.MemorySize")
	/// local OpAccess = require("iced_x86.OpAccess")
	/// local Register = require("iced_x86.Register")
	///
	/// local decoder = Decoder.new(64, "\196\227\073\072\016\065")
	/// local instr = decoder:decode()
	///
	/// local used_registers, used_memory, op_accesses = instr:used_values()
	///
	/// assert(#used_registers == 4)
	/// assert(used_registers[1]:register() == Register.ZMM2)
	/// assert(used_registers[1]:access() == OpAccess.Write)
	/// assert(used_registers[2]:register() == Register.XMM6)
	/// assert(used_registers[2]:access() == OpAccess.Read)
	/// assert(used_registers[3]:register() == Register.RAX)
	/// assert(used_registers[3]:access() == OpAccess.Read)
	/// assert(used_registers[4]:register() == Register.XMM4)
	/// assert(used_registers[4]:access() == OpAccess.Read)
	///
	/// assert(#used_memory == 1)
	/// local mem = used_memory[1]
	/// assert(mem:segment() == Register.DS)
	/// assert(mem:base() == Register.RAX)
	/// assert(mem:index() == Register.None)
	/// assert(mem:scale() == 1)
	/// assert(mem:displacement() == 0)
	/// assert(mem:memory_size() == MemorySize.Packed128_Float32)
	/// assert(mem:access() == OpAccess.Read)
	/// assert(mem:address_size() == CodeSize.Code64)
	/// assert(mem:vsib_size() == 0)
	///
	/// assert(#op_accesses == 5)
	/// assert(op_accesses[1] == OpAccess.Write)
	/// assert(op_accesses[2] == OpAccess.Read)
	/// assert(op_accesses[3] == OpAccess.Read)
	/// assert(op_accesses[4] == OpAccess.Read)
	/// assert(op_accesses[5] == OpAccess.Read)
	/// ```
	unsafe fn used_values(lua, this: &Instruction) -> 3 {
		let mut factory = iced_x86::InstructionInfoFactory::new();
		let info = factory.info_options(&this.inner, iced_x86::InstructionInfoOptions::NONE);
		unsafe {
			push_used_registers(lua, info);
			push_used_memory(lua, info);
			push_op_accesses(lua, info, &this.inner);
		}
	}

	/// Creates a `db`/`.byte` asm directive
	///
	/// - If the single arg is a string, the length must be 1-16 bytes
	/// - If the single arg is a table, it must be an array with 1-16 i8/u8 integer elements
	/// - Else it must be 1-16 i8/u8 integer args
	///
	/// # Examples
	/// ```lua
	/// local Instruction = require("iced_x86.Instruction")
	///
	/// local instr1 = Instruction.db("abc")
	/// local instr2 = Instruction.db({ 0x12, 0x34 })
	/// local instr3 = Instruction.db(0x12, 0x34, 0x56)
	/// ```
	///
	/// @return Instruction
	/// @overload fun(a1: integer, a2:integer, a3:integer, a4:integer, a5:integer, a6:integer, a7:integer, a8:integer, a9:integer, a10:integer, a11:integer, a12:integer, a13:integer, a14:integer, a15:integer, a16:integer): Instruction
	/// @overload fun(a1: integer, a2:integer, a3:integer, a4:integer, a5:integer, a6:integer, a7:integer, a8:integer, a9:integer, a10:integer, a11:integer, a12:integer, a13:integer, a14:integer, a15:integer): Instruction
	/// @overload fun(a1: integer, a2:integer, a3:integer, a4:integer, a5:integer, a6:integer, a7:integer, a8:integer, a9:integer, a10:integer, a11:integer, a12:integer, a13:integer, a14:integer): Instruction
	/// @overload fun(a1: integer, a2:integer, a3:integer, a4:integer, a5:integer, a6:integer, a7:integer, a8:integer, a9:integer, a10:integer, a11:integer, a12:integer, a13:integer): Instruction
	/// @overload fun(a1: integer, a2:integer, a3:integer, a4:integer, a5:integer, a6:integer, a7:integer, a8:integer, a9:integer, a10:integer, a11:integer, a12:integer): Instruction
	/// @overload fun(a1: integer, a2:integer, a3:integer, a4:integer, a5:integer, a6:integer, a7:integer, a8:integer, a9:integer, a10:integer, a11:integer): Instruction
	/// @overload fun(a1: integer, a2:integer, a3:integer, a4:integer, a5:integer, a6:integer, a7:integer, a8:integer, a9:integer, a10:integer): Instruction
	/// @overload fun(a1: integer, a2:integer, a3:integer, a4:integer, a5:integer, a6:integer, a7:integer, a8:integer, a9:integer): Instruction
	/// @overload fun(a1: integer, a2:integer, a3:integer, a4:integer, a5:integer, a6:integer, a7:integer, a8:integer): Instruction
	/// @overload fun(a1: integer, a2:integer, a3:integer, a4:integer, a5:integer, a6:integer, a7:integer): Instruction
	/// @overload fun(a1: integer, a2:integer, a3:integer, a4:integer, a5:integer, a6:integer): Instruction
	/// @overload fun(a1: integer, a2:integer, a3:integer, a4:integer, a5:integer): Instruction
	/// @overload fun(a1: integer, a2:integer, a3:integer, a4:integer): Instruction
	/// @overload fun(a1: integer, a2:integer, a3:integer): Instruction
	/// @overload fun(a1: integer, a2:integer): Instruction
	/// @overload fun(a1: integer): Instruction
	/// @overload fun(values: integer[]): Instruction
	/// @overload fun(bytes: string): Instruction
	unsafe fn db(lua) -> 1 {
		unsafe {
			mk_dx_body!(
				lua,
				1,
				Lua::try_get_u8,
				iced_x86::Instruction::with_declare_byte,
				iced_x86::Instruction::with_declare_byte,
				"Expected a string with 1-16 bytes",
				"Expected i8/u8 integer values",
				"Expected 1-16 i8/u8 integer values",
			);
		}
	}

	/// Creates a `dw`/`.word` asm directive
	///
	/// - If the single arg is a string, the length must be 2-16 bytes and a multiple of 2 bytes
	/// - If the single arg is a table, it must be an array with 1-8 i16/u16 integer elements
	/// - Else it must be 1-8 i16/u16 integer args
	///
	/// # Examples
	/// ```lua
	/// local Instruction = require("iced_x86.Instruction")
	///
	/// local instr1 = Instruction.dw("abcd")
	/// local instr2 = Instruction.dw({ 0x1234, 0x5678 })
	/// local instr3 = Instruction.dw(0x1234, 0x5678, 0x9ABC)
	/// ```
	///
	/// @return Instruction
	/// @overload fun(a1: integer, a2:integer, a3:integer, a4:integer, a5:integer, a6:integer, a7:integer, a8:integer): Instruction
	/// @overload fun(a1: integer, a2:integer, a3:integer, a4:integer, a5:integer, a6:integer, a7:integer): Instruction
	/// @overload fun(a1: integer, a2:integer, a3:integer, a4:integer, a5:integer, a6:integer): Instruction
	/// @overload fun(a1: integer, a2:integer, a3:integer, a4:integer, a5:integer): Instruction
	/// @overload fun(a1: integer, a2:integer, a3:integer, a4:integer): Instruction
	/// @overload fun(a1: integer, a2:integer, a3:integer): Instruction
	/// @overload fun(a1: integer, a2:integer): Instruction
	/// @overload fun(a1: integer): Instruction
	/// @overload fun(values: integer[]): Instruction
	/// @overload fun(bytes: string): Instruction
	unsafe fn dw(lua) -> 1 {
		unsafe {
			mk_dx_body!(
				lua,
				2,
				Lua::try_get_u16,
				iced_x86::Instruction::with_declare_word_slice_u8,
				iced_x86::Instruction::with_declare_word,
				"Expected a string with 2-16 bytes and a multiple of 2 bytes",
				"Expected i16/u16 integer values",
				"Expected 1-8 i16/u16 integer values",
			);
		}
	}

	/// Creates a `dd`/`.int` asm directive
	///
	/// - If the single arg is a string, the length must be 4-16 bytes and a multiple of 4 bytes
	/// - If the single arg is a table, it must be an array with 1-4 i32/u32 integer elements
	/// - Else it must be 1-4 i32/u32 integer args
	///
	/// # Examples
	/// ```lua
	/// local Instruction = require("iced_x86.Instruction")
	///
	/// local instr1 = Instruction.dd("abcdefgh")
	/// local instr2 = Instruction.dd({ 0x12345678, 0x9ABCDEF0 })
	/// local instr3 = Instruction.dd(1, 2, 3, 4)
	/// ```
	///
	/// @return Instruction
	/// @overload fun(a1: integer, a2:integer, a3:integer, a4:integer): Instruction
	/// @overload fun(a1: integer, a2:integer, a3:integer): Instruction
	/// @overload fun(a1: integer, a2:integer): Instruction
	/// @overload fun(a1: integer): Instruction
	/// @overload fun(values: integer[]): Instruction
	/// @overload fun(bytes: string): Instruction
	unsafe fn dd(lua) -> 1 {
		unsafe {
			mk_dx_body!(
				lua,
				4,
				Lua::try_get_u32,
				iced_x86::Instruction::with_declare_dword_slice_u8,
				iced_x86::Instruction::with_declare_dword,
				"Expected a string with 4-16 bytes and a multiple of 4 bytes",
				"Expected i32/u32 integer values",
				"Expected 1-4 i32/u32 integer values",
			);
		}
	}

	/// Creates a `dq`/`.quad` asm directive
	///
	/// - If the single arg is a string, the length must be 8-16 bytes and a multiple of 8 bytes
	/// - If the single arg is a table, it must be an array with 1-2 i64/u64 integer elements
	/// - Else it must be 1-2 i64/u64 integer args
	///
	/// # Examples
	/// ```lua
	/// local Instruction = require("iced_x86.Instruction")
	///
	/// local instr1 = Instruction.dq("abcdefgh")
	/// local instr2 = Instruction.dq({ 0x12345678, 0x9ABCDEF0 })
	/// local instr3 = Instruction.dq(1, 2)
	/// ```
	///
	/// @return Instruction
	/// @overload fun(a1: integer, a2:integer): Instruction
	/// @overload fun(a1: integer): Instruction
	/// @overload fun(values: integer[]): Instruction
	/// @overload fun(bytes: string): Instruction
	unsafe fn dq(lua) -> 1 {
		unsafe {
			mk_dx_body!(
				lua,
				8,
				Lua::try_get_u64,
				iced_x86::Instruction::with_declare_qword_slice_u8,
				iced_x86::Instruction::with_declare_qword,
				"Expected a string with 8-16 bytes and a multiple of 8 bytes",
				"Expected i64/u64 integer values",
				"Expected 1-2 i64/u64 integer values",
			);
		}
	}

	// GENERATOR-BEGIN: Create
	// ⚠️This was generated by GENERATOR!🦹‍♂️
	/// Creates an instruction. All immediate values are assumed to be signed
	///
	/// @param code integer #(A `Code` enum variant) Code value
	/// @return Instruction
	/// @overload fun(code: integer, register1: integer, register2: integer, register3: integer, memory: MemoryOperand, immediate: integer): Instruction
	/// @overload fun(code: integer, register1: integer, register2: integer, memory: MemoryOperand, register3: integer, immediate: integer): Instruction
	/// @overload fun(code: integer, register1: integer, register2: integer, register3: integer, register4: integer, immediate: integer): Instruction
	/// @overload fun(code: integer, register1: integer, register2: integer, immediate1: integer, immediate2: integer): Instruction
	/// @overload fun(code: integer, register1: integer, register2: integer, register3: integer, memory: MemoryOperand): Instruction
	/// @overload fun(code: integer, register1: integer, register2: integer, memory: MemoryOperand, immediate: integer): Instruction
	/// @overload fun(code: integer, register1: integer, register2: integer, register3: integer, immediate: integer): Instruction
	/// @overload fun(code: integer, register1: integer, register2: integer, memory: MemoryOperand, register3: integer): Instruction
	/// @overload fun(code: integer, register1: integer, register2: integer, register3: integer, register4: integer): Instruction
	/// @overload fun(code: integer, register: integer, immediate1: integer, immediate2: integer): Instruction
	/// @overload fun(code: integer, register1: integer, register2: integer, memory: MemoryOperand): Instruction
	/// @overload fun(code: integer, register: integer, memory: MemoryOperand, immediate: integer): Instruction
	/// @overload fun(code: integer, register1: integer, memory: MemoryOperand, register2: integer): Instruction
	/// @overload fun(code: integer, memory: MemoryOperand, register: integer, immediate: integer): Instruction
	/// @overload fun(code: integer, register1: integer, register2: integer, immediate: integer): Instruction
	/// @overload fun(code: integer, memory: MemoryOperand, register1: integer, register2: integer): Instruction
	/// @overload fun(code: integer, register1: integer, register2: integer, register3: integer): Instruction
	/// @overload fun(code: integer, immediate1: integer, immediate2: integer): Instruction
	/// @overload fun(code: integer, immediate: integer, register: integer): Instruction
	/// @overload fun(code: integer, register: integer, memory: MemoryOperand): Instruction
	/// @overload fun(code: integer, memory: MemoryOperand, immediate: integer): Instruction
	/// @overload fun(code: integer, register: integer, immediate: integer): Instruction
	/// @overload fun(code: integer, memory: MemoryOperand, register: integer): Instruction
	/// @overload fun(code: integer, register1: integer, register2: integer): Instruction
	/// @overload fun(code: integer, immediate: integer): Instruction
	/// @overload fun(code: integer, memory: MemoryOperand): Instruction
	/// @overload fun(code: integer, register: integer): Instruction
	/// @overload fun(code: integer): Instruction
	#[rustfmt::skip]
	unsafe fn create(lua, code: u32) -> 1 {
		let code = unsafe { to_code(lua, code) };
		let arg_count = unsafe { lua.get_top() };
		let instr: Instruction;
		match crate::grp_idx::GROUP_INDEXES[code as usize] {
			// Invalid Code value, call some other create method instead, eg. create_branch(), etc
			0 => unsafe { lua.throw_error_msg("Invalid Code value") },
			1 => {
				if arg_count != 1 {
					unsafe { lua.throw_error_msg("Invalid arg count") }
				}
				// @overload fun(code: integer): Instruction
				instr = Instruction { inner: iced_x86::Instruction::with(code) };
			}
			2 => {
				if arg_count != 2 {
					unsafe { lua.throw_error_msg("Invalid arg count") }
				}
				if unsafe { lua.type_(2) } == loona::lua_api::LUA_TUSERDATA {
					// @overload fun(code: integer, memory: MemoryOperand): Instruction
					let memory: <&crate::mem_op::MemoryOperand as loona::tofrom::FromLua<'_>>::RetType = unsafe { <&crate::mem_op::MemoryOperand as loona::tofrom::FromLua<'_>>::from_lua(lua, 2) };
					instr = Instruction { inner: match iced_x86::Instruction::with1(code, memory.inner) {
						Ok(instr) => instr,
						Err(e) => unsafe { lua.throw_error(e) },
					}};
				} else {
					// @overload fun(code: integer, register: integer): Instruction
					let register: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 2) };
					let register = unsafe { to_register(lua, register) };
					instr = Instruction { inner: match iced_x86::Instruction::with1(code, register) {
						Ok(instr) => instr,
						Err(e) => unsafe { lua.throw_error(e) },
					}};
				}
			}
			3 => {
				if arg_count != 2 {
					unsafe { lua.throw_error_msg("Invalid arg count") }
				}
				// @overload fun(code: integer, register: integer): Instruction
				let register: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 2) };
				let register = unsafe { to_register(lua, register) };
				instr = Instruction { inner: match iced_x86::Instruction::with1(code, register) {
					Ok(instr) => instr,
					Err(e) => unsafe { lua.throw_error(e) },
				}};
			}
			4 => {
				if arg_count != 2 {
					unsafe { lua.throw_error_msg("Invalid arg count") }
				}
				// @overload fun(code: integer, immediate: integer): Instruction
				let immediate: <i32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <i32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 2) };
				instr = Instruction { inner: match iced_x86::Instruction::with1(code, immediate) {
					Ok(instr) => instr,
					Err(e) => unsafe { lua.throw_error(e) },
				}};
			}
			5 => {
				if arg_count != 2 {
					unsafe { lua.throw_error_msg("Invalid arg count") }
				}
				// @overload fun(code: integer, memory: MemoryOperand): Instruction
				let memory: <&crate::mem_op::MemoryOperand as loona::tofrom::FromLua<'_>>::RetType = unsafe { <&crate::mem_op::MemoryOperand as loona::tofrom::FromLua<'_>>::from_lua(lua, 2) };
				instr = Instruction { inner: match iced_x86::Instruction::with1(code, memory.inner) {
					Ok(instr) => instr,
					Err(e) => unsafe { lua.throw_error(e) },
				}};
			}
			6 => {
				if arg_count != 3 {
					unsafe { lua.throw_error_msg("Invalid arg count") }
				}
				if unsafe { lua.type_(2) } == loona::lua_api::LUA_TUSERDATA {
					// @overload fun(code: integer, memory: MemoryOperand, register: integer): Instruction
					let memory: <&crate::mem_op::MemoryOperand as loona::tofrom::FromLua<'_>>::RetType = unsafe { <&crate::mem_op::MemoryOperand as loona::tofrom::FromLua<'_>>::from_lua(lua, 2) };
					let register: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 3) };
					let register = unsafe { to_register(lua, register) };
					instr = Instruction { inner: match iced_x86::Instruction::with2(code, memory.inner, register) {
						Ok(instr) => instr,
						Err(e) => unsafe { lua.throw_error(e) },
					}};
				} else {
					// @overload fun(code: integer, register1: integer, register2: integer): Instruction
					let register1: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 2) };
					let register2: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 3) };
					let register1 = unsafe { to_register(lua, register1) };
					let register2 = unsafe { to_register(lua, register2) };
					instr = Instruction { inner: match iced_x86::Instruction::with2(code, register1, register2) {
						Ok(instr) => instr,
						Err(e) => unsafe { lua.throw_error(e) },
					}};
				}
			}
			7 => {
				if arg_count != 3 {
					unsafe { lua.throw_error_msg("Invalid arg count") }
				}
				if unsafe { lua.type_(2) } == loona::lua_api::LUA_TUSERDATA {
					// @overload fun(code: integer, memory: MemoryOperand, immediate: integer): Instruction
					let memory: <&crate::mem_op::MemoryOperand as loona::tofrom::FromLua<'_>>::RetType = unsafe { <&crate::mem_op::MemoryOperand as loona::tofrom::FromLua<'_>>::from_lua(lua, 2) };
					let immediate: <i32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <i32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 3) };
					instr = Instruction { inner: match iced_x86::Instruction::with2(code, memory.inner, immediate) {
						Ok(instr) => instr,
						Err(e) => unsafe { lua.throw_error(e) },
					}};
				} else {
					// @overload fun(code: integer, register: integer, immediate: integer): Instruction
					let register: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 2) };
					let immediate: <i32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <i32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 3) };
					let register = unsafe { to_register(lua, register) };
					instr = Instruction { inner: match iced_x86::Instruction::with2(code, register, immediate) {
						Ok(instr) => instr,
						Err(e) => unsafe { lua.throw_error(e) },
					}};
				}
			}
			8 => {
				if arg_count != 3 {
					unsafe { lua.throw_error_msg("Invalid arg count") }
				}
				if unsafe { lua.type_(3) } == loona::lua_api::LUA_TUSERDATA {
					// @overload fun(code: integer, register: integer, memory: MemoryOperand): Instruction
					let register: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 2) };
					let memory: <&crate::mem_op::MemoryOperand as loona::tofrom::FromLua<'_>>::RetType = unsafe { <&crate::mem_op::MemoryOperand as loona::tofrom::FromLua<'_>>::from_lua(lua, 3) };
					let register = unsafe { to_register(lua, register) };
					instr = Instruction { inner: match iced_x86::Instruction::with2(code, register, memory.inner) {
						Ok(instr) => instr,
						Err(e) => unsafe { lua.throw_error(e) },
					}};
				} else {
					// @overload fun(code: integer, register1: integer, register2: integer): Instruction
					let register1: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 2) };
					let register2: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 3) };
					let register1 = unsafe { to_register(lua, register1) };
					let register2 = unsafe { to_register(lua, register2) };
					instr = Instruction { inner: match iced_x86::Instruction::with2(code, register1, register2) {
						Ok(instr) => instr,
						Err(e) => unsafe { lua.throw_error(e) },
					}};
				}
			}
			9 => {
				if arg_count != 3 {
					unsafe { lua.throw_error_msg("Invalid arg count") }
				}
				// @overload fun(code: integer, register1: integer, register2: integer): Instruction
				let register1: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 2) };
				let register2: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 3) };
				let register1 = unsafe { to_register(lua, register1) };
				let register2 = unsafe { to_register(lua, register2) };
				instr = Instruction { inner: match iced_x86::Instruction::with2(code, register1, register2) {
					Ok(instr) => instr,
					Err(e) => unsafe { lua.throw_error(e) },
				}};
			}
			10 => {
				if arg_count != 3 {
					unsafe { lua.throw_error_msg("Invalid arg count") }
				}
				// @overload fun(code: integer, register: integer, immediate: integer): Instruction
				let register: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 2) };
				let immediate: <i32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <i32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 3) };
				let register = unsafe { to_register(lua, register) };
				instr = Instruction { inner: match iced_x86::Instruction::with2(code, register, immediate) {
					Ok(instr) => instr,
					Err(e) => unsafe { lua.throw_error(e) },
				}};
			}
			11 => {
				if arg_count != 3 {
					unsafe { lua.throw_error_msg("Invalid arg count") }
				}
				// @overload fun(code: integer, register: integer, immediate: integer): Instruction
				let register: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 2) };
				let immediate: <i64 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <i64 as loona::tofrom::FromLua<'_>>::from_lua(lua, 3) };
				let register = unsafe { to_register(lua, register) };
				instr = Instruction { inner: match iced_x86::Instruction::with2(code, register, immediate) {
					Ok(instr) => instr,
					Err(e) => unsafe { lua.throw_error(e) },
				}};
			}
			12 => {
				if arg_count != 3 {
					unsafe { lua.throw_error_msg("Invalid arg count") }
				}
				// @overload fun(code: integer, register: integer, memory: MemoryOperand): Instruction
				let register: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 2) };
				let memory: <&crate::mem_op::MemoryOperand as loona::tofrom::FromLua<'_>>::RetType = unsafe { <&crate::mem_op::MemoryOperand as loona::tofrom::FromLua<'_>>::from_lua(lua, 3) };
				let register = unsafe { to_register(lua, register) };
				instr = Instruction { inner: match iced_x86::Instruction::with2(code, register, memory.inner) {
					Ok(instr) => instr,
					Err(e) => unsafe { lua.throw_error(e) },
				}};
			}
			13 => {
				if arg_count != 3 {
					unsafe { lua.throw_error_msg("Invalid arg count") }
				}
				// @overload fun(code: integer, immediate: integer, register: integer): Instruction
				let immediate: <i32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <i32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 2) };
				let register: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 3) };
				let register = unsafe { to_register(lua, register) };
				instr = Instruction { inner: match iced_x86::Instruction::with2(code, immediate, register) {
					Ok(instr) => instr,
					Err(e) => unsafe { lua.throw_error(e) },
				}};
			}
			14 => {
				if arg_count != 3 {
					unsafe { lua.throw_error_msg("Invalid arg count") }
				}
				// @overload fun(code: integer, immediate1: integer, immediate2: integer): Instruction
				let immediate1: <i32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <i32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 2) };
				let immediate2: <i32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <i32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 3) };
				instr = Instruction { inner: match iced_x86::Instruction::with2(code, immediate1, immediate2) {
					Ok(instr) => instr,
					Err(e) => unsafe { lua.throw_error(e) },
				}};
			}
			15 => {
				if arg_count != 3 {
					unsafe { lua.throw_error_msg("Invalid arg count") }
				}
				// @overload fun(code: integer, memory: MemoryOperand, register: integer): Instruction
				let memory: <&crate::mem_op::MemoryOperand as loona::tofrom::FromLua<'_>>::RetType = unsafe { <&crate::mem_op::MemoryOperand as loona::tofrom::FromLua<'_>>::from_lua(lua, 2) };
				let register: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 3) };
				let register = unsafe { to_register(lua, register) };
				instr = Instruction { inner: match iced_x86::Instruction::with2(code, memory.inner, register) {
					Ok(instr) => instr,
					Err(e) => unsafe { lua.throw_error(e) },
				}};
			}
			16 => {
				if arg_count != 4 {
					unsafe { lua.throw_error_msg("Invalid arg count") }
				}
				if unsafe { lua.type_(2) } == loona::lua_api::LUA_TUSERDATA {
					// @overload fun(code: integer, memory: MemoryOperand, register1: integer, register2: integer): Instruction
					let memory: <&crate::mem_op::MemoryOperand as loona::tofrom::FromLua<'_>>::RetType = unsafe { <&crate::mem_op::MemoryOperand as loona::tofrom::FromLua<'_>>::from_lua(lua, 2) };
					let register1: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 3) };
					let register2: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 4) };
					let register1 = unsafe { to_register(lua, register1) };
					let register2 = unsafe { to_register(lua, register2) };
					instr = Instruction { inner: match iced_x86::Instruction::with3(code, memory.inner, register1, register2) {
						Ok(instr) => instr,
						Err(e) => unsafe { lua.throw_error(e) },
					}};
				} else {
					// @overload fun(code: integer, register1: integer, register2: integer, register3: integer): Instruction
					let register1: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 2) };
					let register2: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 3) };
					let register3: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 4) };
					let register1 = unsafe { to_register(lua, register1) };
					let register2 = unsafe { to_register(lua, register2) };
					let register3 = unsafe { to_register(lua, register3) };
					instr = Instruction { inner: match iced_x86::Instruction::with3(code, register1, register2, register3) {
						Ok(instr) => instr,
						Err(e) => unsafe { lua.throw_error(e) },
					}};
				}
			}
			17 => {
				if arg_count != 4 {
					unsafe { lua.throw_error_msg("Invalid arg count") }
				}
				if unsafe { lua.type_(2) } == loona::lua_api::LUA_TUSERDATA {
					// @overload fun(code: integer, memory: MemoryOperand, register: integer, immediate: integer): Instruction
					let memory: <&crate::mem_op::MemoryOperand as loona::tofrom::FromLua<'_>>::RetType = unsafe { <&crate::mem_op::MemoryOperand as loona::tofrom::FromLua<'_>>::from_lua(lua, 2) };
					let register: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 3) };
					let immediate: <i32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <i32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 4) };
					let register = unsafe { to_register(lua, register) };
					instr = Instruction { inner: match iced_x86::Instruction::with3(code, memory.inner, register, immediate) {
						Ok(instr) => instr,
						Err(e) => unsafe { lua.throw_error(e) },
					}};
				} else {
					// @overload fun(code: integer, register1: integer, register2: integer, immediate: integer): Instruction
					let register1: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 2) };
					let register2: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 3) };
					let immediate: <i32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <i32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 4) };
					let register1 = unsafe { to_register(lua, register1) };
					let register2 = unsafe { to_register(lua, register2) };
					instr = Instruction { inner: match iced_x86::Instruction::with3(code, register1, register2, immediate) {
						Ok(instr) => instr,
						Err(e) => unsafe { lua.throw_error(e) },
					}};
				}
			}
			18 => {
				if arg_count != 4 {
					unsafe { lua.throw_error_msg("Invalid arg count") }
				}
				if unsafe { lua.type_(3) } == loona::lua_api::LUA_TUSERDATA {
					// @overload fun(code: integer, register1: integer, memory: MemoryOperand, register2: integer): Instruction
					let register1: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 2) };
					let memory: <&crate::mem_op::MemoryOperand as loona::tofrom::FromLua<'_>>::RetType = unsafe { <&crate::mem_op::MemoryOperand as loona::tofrom::FromLua<'_>>::from_lua(lua, 3) };
					let register2: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 4) };
					let register1 = unsafe { to_register(lua, register1) };
					let register2 = unsafe { to_register(lua, register2) };
					instr = Instruction { inner: match iced_x86::Instruction::with3(code, register1, memory.inner, register2) {
						Ok(instr) => instr,
						Err(e) => unsafe { lua.throw_error(e) },
					}};
				} else {
					// @overload fun(code: integer, register1: integer, register2: integer, register3: integer): Instruction
					let register1: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 2) };
					let register2: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 3) };
					let register3: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 4) };
					let register1 = unsafe { to_register(lua, register1) };
					let register2 = unsafe { to_register(lua, register2) };
					let register3 = unsafe { to_register(lua, register3) };
					instr = Instruction { inner: match iced_x86::Instruction::with3(code, register1, register2, register3) {
						Ok(instr) => instr,
						Err(e) => unsafe { lua.throw_error(e) },
					}};
				}
			}
			19 => {
				if arg_count != 4 {
					unsafe { lua.throw_error_msg("Invalid arg count") }
				}
				if unsafe { lua.type_(3) } == loona::lua_api::LUA_TUSERDATA {
					// @overload fun(code: integer, register: integer, memory: MemoryOperand, immediate: integer): Instruction
					let register: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 2) };
					let memory: <&crate::mem_op::MemoryOperand as loona::tofrom::FromLua<'_>>::RetType = unsafe { <&crate::mem_op::MemoryOperand as loona::tofrom::FromLua<'_>>::from_lua(lua, 3) };
					let immediate: <i32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <i32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 4) };
					let register = unsafe { to_register(lua, register) };
					instr = Instruction { inner: match iced_x86::Instruction::with3(code, register, memory.inner, immediate) {
						Ok(instr) => instr,
						Err(e) => unsafe { lua.throw_error(e) },
					}};
				} else {
					// @overload fun(code: integer, register1: integer, register2: integer, immediate: integer): Instruction
					let register1: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 2) };
					let register2: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 3) };
					let immediate: <i32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <i32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 4) };
					let register1 = unsafe { to_register(lua, register1) };
					let register2 = unsafe { to_register(lua, register2) };
					instr = Instruction { inner: match iced_x86::Instruction::with3(code, register1, register2, immediate) {
						Ok(instr) => instr,
						Err(e) => unsafe { lua.throw_error(e) },
					}};
				}
			}
			20 => {
				if arg_count != 4 {
					unsafe { lua.throw_error_msg("Invalid arg count") }
				}
				if unsafe { lua.type_(4) } == loona::lua_api::LUA_TUSERDATA {
					// @overload fun(code: integer, register1: integer, register2: integer, memory: MemoryOperand): Instruction
					let register1: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 2) };
					let register2: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 3) };
					let memory: <&crate::mem_op::MemoryOperand as loona::tofrom::FromLua<'_>>::RetType = unsafe { <&crate::mem_op::MemoryOperand as loona::tofrom::FromLua<'_>>::from_lua(lua, 4) };
					let register1 = unsafe { to_register(lua, register1) };
					let register2 = unsafe { to_register(lua, register2) };
					instr = Instruction { inner: match iced_x86::Instruction::with3(code, register1, register2, memory.inner) {
						Ok(instr) => instr,
						Err(e) => unsafe { lua.throw_error(e) },
					}};
				} else {
					// @overload fun(code: integer, register1: integer, register2: integer, register3: integer): Instruction
					let register1: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 2) };
					let register2: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 3) };
					let register3: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 4) };
					let register1 = unsafe { to_register(lua, register1) };
					let register2 = unsafe { to_register(lua, register2) };
					let register3 = unsafe { to_register(lua, register3) };
					instr = Instruction { inner: match iced_x86::Instruction::with3(code, register1, register2, register3) {
						Ok(instr) => instr,
						Err(e) => unsafe { lua.throw_error(e) },
					}};
				}
			}
			21 => {
				if arg_count != 4 {
					unsafe { lua.throw_error_msg("Invalid arg count") }
				}
				// @overload fun(code: integer, register1: integer, register2: integer, register3: integer): Instruction
				let register1: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 2) };
				let register2: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 3) };
				let register3: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 4) };
				let register1 = unsafe { to_register(lua, register1) };
				let register2 = unsafe { to_register(lua, register2) };
				let register3 = unsafe { to_register(lua, register3) };
				instr = Instruction { inner: match iced_x86::Instruction::with3(code, register1, register2, register3) {
					Ok(instr) => instr,
					Err(e) => unsafe { lua.throw_error(e) },
				}};
			}
			22 => {
				if arg_count != 4 {
					unsafe { lua.throw_error_msg("Invalid arg count") }
				}
				// @overload fun(code: integer, register1: integer, register2: integer, immediate: integer): Instruction
				let register1: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 2) };
				let register2: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 3) };
				let immediate: <i32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <i32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 4) };
				let register1 = unsafe { to_register(lua, register1) };
				let register2 = unsafe { to_register(lua, register2) };
				instr = Instruction { inner: match iced_x86::Instruction::with3(code, register1, register2, immediate) {
					Ok(instr) => instr,
					Err(e) => unsafe { lua.throw_error(e) },
				}};
			}
			23 => {
				if arg_count != 4 {
					unsafe { lua.throw_error_msg("Invalid arg count") }
				}
				// @overload fun(code: integer, register1: integer, register2: integer, memory: MemoryOperand): Instruction
				let register1: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 2) };
				let register2: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 3) };
				let memory: <&crate::mem_op::MemoryOperand as loona::tofrom::FromLua<'_>>::RetType = unsafe { <&crate::mem_op::MemoryOperand as loona::tofrom::FromLua<'_>>::from_lua(lua, 4) };
				let register1 = unsafe { to_register(lua, register1) };
				let register2 = unsafe { to_register(lua, register2) };
				instr = Instruction { inner: match iced_x86::Instruction::with3(code, register1, register2, memory.inner) {
					Ok(instr) => instr,
					Err(e) => unsafe { lua.throw_error(e) },
				}};
			}
			24 => {
				if arg_count != 4 {
					unsafe { lua.throw_error_msg("Invalid arg count") }
				}
				// @overload fun(code: integer, register: integer, immediate1: integer, immediate2: integer): Instruction
				let register: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 2) };
				let immediate1: <i32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <i32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 3) };
				let immediate2: <i32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <i32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 4) };
				let register = unsafe { to_register(lua, register) };
				instr = Instruction { inner: match iced_x86::Instruction::with3(code, register, immediate1, immediate2) {
					Ok(instr) => instr,
					Err(e) => unsafe { lua.throw_error(e) },
				}};
			}
			25 => {
				if arg_count != 4 {
					unsafe { lua.throw_error_msg("Invalid arg count") }
				}
				// @overload fun(code: integer, register1: integer, memory: MemoryOperand, register2: integer): Instruction
				let register1: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 2) };
				let memory: <&crate::mem_op::MemoryOperand as loona::tofrom::FromLua<'_>>::RetType = unsafe { <&crate::mem_op::MemoryOperand as loona::tofrom::FromLua<'_>>::from_lua(lua, 3) };
				let register2: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 4) };
				let register1 = unsafe { to_register(lua, register1) };
				let register2 = unsafe { to_register(lua, register2) };
				instr = Instruction { inner: match iced_x86::Instruction::with3(code, register1, memory.inner, register2) {
					Ok(instr) => instr,
					Err(e) => unsafe { lua.throw_error(e) },
				}};
			}
			26 => {
				if arg_count != 4 {
					unsafe { lua.throw_error_msg("Invalid arg count") }
				}
				// @overload fun(code: integer, memory: MemoryOperand, register1: integer, register2: integer): Instruction
				let memory: <&crate::mem_op::MemoryOperand as loona::tofrom::FromLua<'_>>::RetType = unsafe { <&crate::mem_op::MemoryOperand as loona::tofrom::FromLua<'_>>::from_lua(lua, 2) };
				let register1: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 3) };
				let register2: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 4) };
				let register1 = unsafe { to_register(lua, register1) };
				let register2 = unsafe { to_register(lua, register2) };
				instr = Instruction { inner: match iced_x86::Instruction::with3(code, memory.inner, register1, register2) {
					Ok(instr) => instr,
					Err(e) => unsafe { lua.throw_error(e) },
				}};
			}
			27 => {
				if arg_count != 5 {
					unsafe { lua.throw_error_msg("Invalid arg count") }
				}
				if unsafe { lua.type_(4) } == loona::lua_api::LUA_TUSERDATA {
					// @overload fun(code: integer, register1: integer, register2: integer, memory: MemoryOperand, register3: integer): Instruction
					let register1: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 2) };
					let register2: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 3) };
					let memory: <&crate::mem_op::MemoryOperand as loona::tofrom::FromLua<'_>>::RetType = unsafe { <&crate::mem_op::MemoryOperand as loona::tofrom::FromLua<'_>>::from_lua(lua, 4) };
					let register3: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 5) };
					let register1 = unsafe { to_register(lua, register1) };
					let register2 = unsafe { to_register(lua, register2) };
					let register3 = unsafe { to_register(lua, register3) };
					instr = Instruction { inner: match iced_x86::Instruction::with4(code, register1, register2, memory.inner, register3) {
						Ok(instr) => instr,
						Err(e) => unsafe { lua.throw_error(e) },
					}};
				} else {
					// @overload fun(code: integer, register1: integer, register2: integer, register3: integer, register4: integer): Instruction
					let register1: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 2) };
					let register2: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 3) };
					let register3: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 4) };
					let register4: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 5) };
					let register1 = unsafe { to_register(lua, register1) };
					let register2 = unsafe { to_register(lua, register2) };
					let register3 = unsafe { to_register(lua, register3) };
					let register4 = unsafe { to_register(lua, register4) };
					instr = Instruction { inner: match iced_x86::Instruction::with4(code, register1, register2, register3, register4) {
						Ok(instr) => instr,
						Err(e) => unsafe { lua.throw_error(e) },
					}};
				}
			}
			28 => {
				if arg_count != 5 {
					unsafe { lua.throw_error_msg("Invalid arg count") }
				}
				if unsafe { lua.type_(4) } == loona::lua_api::LUA_TUSERDATA {
					// @overload fun(code: integer, register1: integer, register2: integer, memory: MemoryOperand, immediate: integer): Instruction
					let register1: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 2) };
					let register2: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 3) };
					let memory: <&crate::mem_op::MemoryOperand as loona::tofrom::FromLua<'_>>::RetType = unsafe { <&crate::mem_op::MemoryOperand as loona::tofrom::FromLua<'_>>::from_lua(lua, 4) };
					let immediate: <i32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <i32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 5) };
					let register1 = unsafe { to_register(lua, register1) };
					let register2 = unsafe { to_register(lua, register2) };
					instr = Instruction { inner: match iced_x86::Instruction::with4(code, register1, register2, memory.inner, immediate) {
						Ok(instr) => instr,
						Err(e) => unsafe { lua.throw_error(e) },
					}};
				} else {
					// @overload fun(code: integer, register1: integer, register2: integer, register3: integer, immediate: integer): Instruction
					let register1: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 2) };
					let register2: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 3) };
					let register3: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 4) };
					let immediate: <i32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <i32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 5) };
					let register1 = unsafe { to_register(lua, register1) };
					let register2 = unsafe { to_register(lua, register2) };
					let register3 = unsafe { to_register(lua, register3) };
					instr = Instruction { inner: match iced_x86::Instruction::with4(code, register1, register2, register3, immediate) {
						Ok(instr) => instr,
						Err(e) => unsafe { lua.throw_error(e) },
					}};
				}
			}
			29 => {
				if arg_count != 5 {
					unsafe { lua.throw_error_msg("Invalid arg count") }
				}
				if unsafe { lua.type_(5) } == loona::lua_api::LUA_TUSERDATA {
					// @overload fun(code: integer, register1: integer, register2: integer, register3: integer, memory: MemoryOperand): Instruction
					let register1: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 2) };
					let register2: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 3) };
					let register3: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 4) };
					let memory: <&crate::mem_op::MemoryOperand as loona::tofrom::FromLua<'_>>::RetType = unsafe { <&crate::mem_op::MemoryOperand as loona::tofrom::FromLua<'_>>::from_lua(lua, 5) };
					let register1 = unsafe { to_register(lua, register1) };
					let register2 = unsafe { to_register(lua, register2) };
					let register3 = unsafe { to_register(lua, register3) };
					instr = Instruction { inner: match iced_x86::Instruction::with4(code, register1, register2, register3, memory.inner) {
						Ok(instr) => instr,
						Err(e) => unsafe { lua.throw_error(e) },
					}};
				} else {
					// @overload fun(code: integer, register1: integer, register2: integer, register3: integer, register4: integer): Instruction
					let register1: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 2) };
					let register2: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 3) };
					let register3: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 4) };
					let register4: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 5) };
					let register1 = unsafe { to_register(lua, register1) };
					let register2 = unsafe { to_register(lua, register2) };
					let register3 = unsafe { to_register(lua, register3) };
					let register4 = unsafe { to_register(lua, register4) };
					instr = Instruction { inner: match iced_x86::Instruction::with4(code, register1, register2, register3, register4) {
						Ok(instr) => instr,
						Err(e) => unsafe { lua.throw_error(e) },
					}};
				}
			}
			30 => {
				if arg_count != 5 {
					unsafe { lua.throw_error_msg("Invalid arg count") }
				}
				// @overload fun(code: integer, register1: integer, register2: integer, immediate1: integer, immediate2: integer): Instruction
				let register1: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 2) };
				let register2: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 3) };
				let immediate1: <i32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <i32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 4) };
				let immediate2: <i32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <i32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 5) };
				let register1 = unsafe { to_register(lua, register1) };
				let register2 = unsafe { to_register(lua, register2) };
				instr = Instruction { inner: match iced_x86::Instruction::with4(code, register1, register2, immediate1, immediate2) {
					Ok(instr) => instr,
					Err(e) => unsafe { lua.throw_error(e) },
				}};
			}
			31 => {
				if arg_count != 6 {
					unsafe { lua.throw_error_msg("Invalid arg count") }
				}
				if unsafe { lua.type_(4) } == loona::lua_api::LUA_TUSERDATA {
					// @overload fun(code: integer, register1: integer, register2: integer, memory: MemoryOperand, register3: integer, immediate: integer): Instruction
					let register1: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 2) };
					let register2: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 3) };
					let memory: <&crate::mem_op::MemoryOperand as loona::tofrom::FromLua<'_>>::RetType = unsafe { <&crate::mem_op::MemoryOperand as loona::tofrom::FromLua<'_>>::from_lua(lua, 4) };
					let register3: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 5) };
					let immediate: <i32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <i32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 6) };
					let register1 = unsafe { to_register(lua, register1) };
					let register2 = unsafe { to_register(lua, register2) };
					let register3 = unsafe { to_register(lua, register3) };
					instr = Instruction { inner: match iced_x86::Instruction::with5(code, register1, register2, memory.inner, register3, immediate) {
						Ok(instr) => instr,
						Err(e) => unsafe { lua.throw_error(e) },
					}};
				} else {
					// @overload fun(code: integer, register1: integer, register2: integer, register3: integer, register4: integer, immediate: integer): Instruction
					let register1: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 2) };
					let register2: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 3) };
					let register3: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 4) };
					let register4: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 5) };
					let immediate: <i32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <i32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 6) };
					let register1 = unsafe { to_register(lua, register1) };
					let register2 = unsafe { to_register(lua, register2) };
					let register3 = unsafe { to_register(lua, register3) };
					let register4 = unsafe { to_register(lua, register4) };
					instr = Instruction { inner: match iced_x86::Instruction::with5(code, register1, register2, register3, register4, immediate) {
						Ok(instr) => instr,
						Err(e) => unsafe { lua.throw_error(e) },
					}};
				}
			}
			32 => {
				if arg_count != 6 {
					unsafe { lua.throw_error_msg("Invalid arg count") }
				}
				if unsafe { lua.type_(5) } == loona::lua_api::LUA_TUSERDATA {
					// @overload fun(code: integer, register1: integer, register2: integer, register3: integer, memory: MemoryOperand, immediate: integer): Instruction
					let register1: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 2) };
					let register2: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 3) };
					let register3: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 4) };
					let memory: <&crate::mem_op::MemoryOperand as loona::tofrom::FromLua<'_>>::RetType = unsafe { <&crate::mem_op::MemoryOperand as loona::tofrom::FromLua<'_>>::from_lua(lua, 5) };
					let immediate: <i32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <i32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 6) };
					let register1 = unsafe { to_register(lua, register1) };
					let register2 = unsafe { to_register(lua, register2) };
					let register3 = unsafe { to_register(lua, register3) };
					instr = Instruction { inner: match iced_x86::Instruction::with5(code, register1, register2, register3, memory.inner, immediate) {
						Ok(instr) => instr,
						Err(e) => unsafe { lua.throw_error(e) },
					}};
				} else {
					// @overload fun(code: integer, register1: integer, register2: integer, register3: integer, register4: integer, immediate: integer): Instruction
					let register1: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 2) };
					let register2: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 3) };
					let register3: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 4) };
					let register4: <u32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <u32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 5) };
					let immediate: <i32 as loona::tofrom::FromLua<'_>>::RetType = unsafe { <i32 as loona::tofrom::FromLua<'_>>::from_lua(lua, 6) };
					let register1 = unsafe { to_register(lua, register1) };
					let register2 = unsafe { to_register(lua, register2) };
					let register3 = unsafe { to_register(lua, register3) };
					let register4 = unsafe { to_register(lua, register4) };
					instr = Instruction { inner: match iced_x86::Instruction::with5(code, register1, register2, register3, register4, immediate) {
						Ok(instr) => instr,
						Err(e) => unsafe { lua.throw_error(e) },
					}};
				}
			}
			// Unreachable
			_ => unsafe { lua.throw_error_msg("Invalid Code value") },
		}
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a new near/short branch instruction
	///
	/// @param code integer #(A `Code` enum variant) Code value
	/// @param target integer #(`u64`) Target address
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_branch(lua, code: u32, target: u64) -> 1 {
		let code = unsafe { to_code(lua, code) };
		let instr = Instruction { inner: match iced_x86::Instruction::with_branch(code, target) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a new far branch instruction
	///
	/// @param code integer #(A `Code` enum variant) Code value
	/// @param selector integer #(`u16`) Selector/segment value
	/// @param offset integer #(`u32`) Offset
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_far_branch(lua, code: u32, selector: u16, offset: u32) -> 1 {
		let code = unsafe { to_code(lua, code) };
		let instr = Instruction { inner: match iced_x86::Instruction::with_far_branch(code, selector, offset) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a new `XBEGIN` instruction
	///
	/// @param bitness integer #(`u32`) 16, 32, or 64
	/// @param target integer #(`u64`) Target address
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_xbegin(lua, bitness: u32, target: u64) -> 1 {
		let instr = Instruction { inner: match iced_x86::Instruction::with_xbegin(bitness, target) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `OUTSB` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @param segment_prefix? integer #(default = `None`) (A `Register` enum variant) Segment override or `Register.None`
	/// @param rep_prefix? integer #(default = `None`) (A `RepPrefixKind` enum variant) Rep prefix or `RepPrefixKind.None`
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_outsb(lua, address_size: u32, segment_prefix: LuaDefaultU32<{iced_x86::Register::None as u32}>, rep_prefix: LuaDefaultU32<{iced_x86::RepPrefixKind::None as u32}>) -> 1 {
		let segment_prefix = unsafe { to_register(lua, segment_prefix) };
		let rep_prefix = unsafe { to_rep_prefix_kind(lua, rep_prefix) };
		let instr = Instruction { inner: match iced_x86::Instruction::with_outsb(address_size, segment_prefix, rep_prefix) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `REP OUTSB` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_rep_outsb(lua, address_size: u32) -> 1 {
		let instr = Instruction { inner: match iced_x86::Instruction::with_rep_outsb(address_size) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `OUTSW` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @param segment_prefix? integer #(default = `None`) (A `Register` enum variant) Segment override or `Register.None`
	/// @param rep_prefix? integer #(default = `None`) (A `RepPrefixKind` enum variant) Rep prefix or `RepPrefixKind.None`
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_outsw(lua, address_size: u32, segment_prefix: LuaDefaultU32<{iced_x86::Register::None as u32}>, rep_prefix: LuaDefaultU32<{iced_x86::RepPrefixKind::None as u32}>) -> 1 {
		let segment_prefix = unsafe { to_register(lua, segment_prefix) };
		let rep_prefix = unsafe { to_rep_prefix_kind(lua, rep_prefix) };
		let instr = Instruction { inner: match iced_x86::Instruction::with_outsw(address_size, segment_prefix, rep_prefix) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `REP OUTSW` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_rep_outsw(lua, address_size: u32) -> 1 {
		let instr = Instruction { inner: match iced_x86::Instruction::with_rep_outsw(address_size) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `OUTSD` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @param segment_prefix? integer #(default = `None`) (A `Register` enum variant) Segment override or `Register.None`
	/// @param rep_prefix? integer #(default = `None`) (A `RepPrefixKind` enum variant) Rep prefix or `RepPrefixKind.None`
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_outsd(lua, address_size: u32, segment_prefix: LuaDefaultU32<{iced_x86::Register::None as u32}>, rep_prefix: LuaDefaultU32<{iced_x86::RepPrefixKind::None as u32}>) -> 1 {
		let segment_prefix = unsafe { to_register(lua, segment_prefix) };
		let rep_prefix = unsafe { to_rep_prefix_kind(lua, rep_prefix) };
		let instr = Instruction { inner: match iced_x86::Instruction::with_outsd(address_size, segment_prefix, rep_prefix) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `REP OUTSD` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_rep_outsd(lua, address_size: u32) -> 1 {
		let instr = Instruction { inner: match iced_x86::Instruction::with_rep_outsd(address_size) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `LODSB` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @param segment_prefix? integer #(default = `None`) (A `Register` enum variant) Segment override or `Register.None`
	/// @param rep_prefix? integer #(default = `None`) (A `RepPrefixKind` enum variant) Rep prefix or `RepPrefixKind.None`
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_lodsb(lua, address_size: u32, segment_prefix: LuaDefaultU32<{iced_x86::Register::None as u32}>, rep_prefix: LuaDefaultU32<{iced_x86::RepPrefixKind::None as u32}>) -> 1 {
		let segment_prefix = unsafe { to_register(lua, segment_prefix) };
		let rep_prefix = unsafe { to_rep_prefix_kind(lua, rep_prefix) };
		let instr = Instruction { inner: match iced_x86::Instruction::with_lodsb(address_size, segment_prefix, rep_prefix) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `REP LODSB` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_rep_lodsb(lua, address_size: u32) -> 1 {
		let instr = Instruction { inner: match iced_x86::Instruction::with_rep_lodsb(address_size) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `LODSW` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @param segment_prefix? integer #(default = `None`) (A `Register` enum variant) Segment override or `Register.None`
	/// @param rep_prefix? integer #(default = `None`) (A `RepPrefixKind` enum variant) Rep prefix or `RepPrefixKind.None`
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_lodsw(lua, address_size: u32, segment_prefix: LuaDefaultU32<{iced_x86::Register::None as u32}>, rep_prefix: LuaDefaultU32<{iced_x86::RepPrefixKind::None as u32}>) -> 1 {
		let segment_prefix = unsafe { to_register(lua, segment_prefix) };
		let rep_prefix = unsafe { to_rep_prefix_kind(lua, rep_prefix) };
		let instr = Instruction { inner: match iced_x86::Instruction::with_lodsw(address_size, segment_prefix, rep_prefix) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `REP LODSW` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_rep_lodsw(lua, address_size: u32) -> 1 {
		let instr = Instruction { inner: match iced_x86::Instruction::with_rep_lodsw(address_size) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `LODSD` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @param segment_prefix? integer #(default = `None`) (A `Register` enum variant) Segment override or `Register.None`
	/// @param rep_prefix? integer #(default = `None`) (A `RepPrefixKind` enum variant) Rep prefix or `RepPrefixKind.None`
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_lodsd(lua, address_size: u32, segment_prefix: LuaDefaultU32<{iced_x86::Register::None as u32}>, rep_prefix: LuaDefaultU32<{iced_x86::RepPrefixKind::None as u32}>) -> 1 {
		let segment_prefix = unsafe { to_register(lua, segment_prefix) };
		let rep_prefix = unsafe { to_rep_prefix_kind(lua, rep_prefix) };
		let instr = Instruction { inner: match iced_x86::Instruction::with_lodsd(address_size, segment_prefix, rep_prefix) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `REP LODSD` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_rep_lodsd(lua, address_size: u32) -> 1 {
		let instr = Instruction { inner: match iced_x86::Instruction::with_rep_lodsd(address_size) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `LODSQ` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @param segment_prefix? integer #(default = `None`) (A `Register` enum variant) Segment override or `Register.None`
	/// @param rep_prefix? integer #(default = `None`) (A `RepPrefixKind` enum variant) Rep prefix or `RepPrefixKind.None`
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_lodsq(lua, address_size: u32, segment_prefix: LuaDefaultU32<{iced_x86::Register::None as u32}>, rep_prefix: LuaDefaultU32<{iced_x86::RepPrefixKind::None as u32}>) -> 1 {
		let segment_prefix = unsafe { to_register(lua, segment_prefix) };
		let rep_prefix = unsafe { to_rep_prefix_kind(lua, rep_prefix) };
		let instr = Instruction { inner: match iced_x86::Instruction::with_lodsq(address_size, segment_prefix, rep_prefix) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `REP LODSQ` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_rep_lodsq(lua, address_size: u32) -> 1 {
		let instr = Instruction { inner: match iced_x86::Instruction::with_rep_lodsq(address_size) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `SCASB` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @param rep_prefix? integer #(default = `None`) (A `RepPrefixKind` enum variant) Rep prefix or `RepPrefixKind.None`
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_scasb(lua, address_size: u32, rep_prefix: LuaDefaultU32<{iced_x86::RepPrefixKind::None as u32}>) -> 1 {
		let rep_prefix = unsafe { to_rep_prefix_kind(lua, rep_prefix) };
		let instr = Instruction { inner: match iced_x86::Instruction::with_scasb(address_size, rep_prefix) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `REPE SCASB` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_repe_scasb(lua, address_size: u32) -> 1 {
		let instr = Instruction { inner: match iced_x86::Instruction::with_repe_scasb(address_size) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `REPNE SCASB` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_repne_scasb(lua, address_size: u32) -> 1 {
		let instr = Instruction { inner: match iced_x86::Instruction::with_repne_scasb(address_size) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `SCASW` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @param rep_prefix? integer #(default = `None`) (A `RepPrefixKind` enum variant) Rep prefix or `RepPrefixKind.None`
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_scasw(lua, address_size: u32, rep_prefix: LuaDefaultU32<{iced_x86::RepPrefixKind::None as u32}>) -> 1 {
		let rep_prefix = unsafe { to_rep_prefix_kind(lua, rep_prefix) };
		let instr = Instruction { inner: match iced_x86::Instruction::with_scasw(address_size, rep_prefix) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `REPE SCASW` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_repe_scasw(lua, address_size: u32) -> 1 {
		let instr = Instruction { inner: match iced_x86::Instruction::with_repe_scasw(address_size) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `REPNE SCASW` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_repne_scasw(lua, address_size: u32) -> 1 {
		let instr = Instruction { inner: match iced_x86::Instruction::with_repne_scasw(address_size) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `SCASD` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @param rep_prefix? integer #(default = `None`) (A `RepPrefixKind` enum variant) Rep prefix or `RepPrefixKind.None`
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_scasd(lua, address_size: u32, rep_prefix: LuaDefaultU32<{iced_x86::RepPrefixKind::None as u32}>) -> 1 {
		let rep_prefix = unsafe { to_rep_prefix_kind(lua, rep_prefix) };
		let instr = Instruction { inner: match iced_x86::Instruction::with_scasd(address_size, rep_prefix) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `REPE SCASD` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_repe_scasd(lua, address_size: u32) -> 1 {
		let instr = Instruction { inner: match iced_x86::Instruction::with_repe_scasd(address_size) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `REPNE SCASD` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_repne_scasd(lua, address_size: u32) -> 1 {
		let instr = Instruction { inner: match iced_x86::Instruction::with_repne_scasd(address_size) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `SCASQ` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @param rep_prefix? integer #(default = `None`) (A `RepPrefixKind` enum variant) Rep prefix or `RepPrefixKind.None`
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_scasq(lua, address_size: u32, rep_prefix: LuaDefaultU32<{iced_x86::RepPrefixKind::None as u32}>) -> 1 {
		let rep_prefix = unsafe { to_rep_prefix_kind(lua, rep_prefix) };
		let instr = Instruction { inner: match iced_x86::Instruction::with_scasq(address_size, rep_prefix) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `REPE SCASQ` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_repe_scasq(lua, address_size: u32) -> 1 {
		let instr = Instruction { inner: match iced_x86::Instruction::with_repe_scasq(address_size) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `REPNE SCASQ` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_repne_scasq(lua, address_size: u32) -> 1 {
		let instr = Instruction { inner: match iced_x86::Instruction::with_repne_scasq(address_size) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `INSB` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @param rep_prefix? integer #(default = `None`) (A `RepPrefixKind` enum variant) Rep prefix or `RepPrefixKind.None`
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_insb(lua, address_size: u32, rep_prefix: LuaDefaultU32<{iced_x86::RepPrefixKind::None as u32}>) -> 1 {
		let rep_prefix = unsafe { to_rep_prefix_kind(lua, rep_prefix) };
		let instr = Instruction { inner: match iced_x86::Instruction::with_insb(address_size, rep_prefix) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `REP INSB` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_rep_insb(lua, address_size: u32) -> 1 {
		let instr = Instruction { inner: match iced_x86::Instruction::with_rep_insb(address_size) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `INSW` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @param rep_prefix? integer #(default = `None`) (A `RepPrefixKind` enum variant) Rep prefix or `RepPrefixKind.None`
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_insw(lua, address_size: u32, rep_prefix: LuaDefaultU32<{iced_x86::RepPrefixKind::None as u32}>) -> 1 {
		let rep_prefix = unsafe { to_rep_prefix_kind(lua, rep_prefix) };
		let instr = Instruction { inner: match iced_x86::Instruction::with_insw(address_size, rep_prefix) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `REP INSW` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_rep_insw(lua, address_size: u32) -> 1 {
		let instr = Instruction { inner: match iced_x86::Instruction::with_rep_insw(address_size) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `INSD` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @param rep_prefix? integer #(default = `None`) (A `RepPrefixKind` enum variant) Rep prefix or `RepPrefixKind.None`
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_insd(lua, address_size: u32, rep_prefix: LuaDefaultU32<{iced_x86::RepPrefixKind::None as u32}>) -> 1 {
		let rep_prefix = unsafe { to_rep_prefix_kind(lua, rep_prefix) };
		let instr = Instruction { inner: match iced_x86::Instruction::with_insd(address_size, rep_prefix) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `REP INSD` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_rep_insd(lua, address_size: u32) -> 1 {
		let instr = Instruction { inner: match iced_x86::Instruction::with_rep_insd(address_size) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `STOSB` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @param rep_prefix? integer #(default = `None`) (A `RepPrefixKind` enum variant) Rep prefix or `RepPrefixKind.None`
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_stosb(lua, address_size: u32, rep_prefix: LuaDefaultU32<{iced_x86::RepPrefixKind::None as u32}>) -> 1 {
		let rep_prefix = unsafe { to_rep_prefix_kind(lua, rep_prefix) };
		let instr = Instruction { inner: match iced_x86::Instruction::with_stosb(address_size, rep_prefix) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `REP STOSB` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_rep_stosb(lua, address_size: u32) -> 1 {
		let instr = Instruction { inner: match iced_x86::Instruction::with_rep_stosb(address_size) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `STOSW` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @param rep_prefix? integer #(default = `None`) (A `RepPrefixKind` enum variant) Rep prefix or `RepPrefixKind.None`
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_stosw(lua, address_size: u32, rep_prefix: LuaDefaultU32<{iced_x86::RepPrefixKind::None as u32}>) -> 1 {
		let rep_prefix = unsafe { to_rep_prefix_kind(lua, rep_prefix) };
		let instr = Instruction { inner: match iced_x86::Instruction::with_stosw(address_size, rep_prefix) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `REP STOSW` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_rep_stosw(lua, address_size: u32) -> 1 {
		let instr = Instruction { inner: match iced_x86::Instruction::with_rep_stosw(address_size) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `STOSD` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @param rep_prefix? integer #(default = `None`) (A `RepPrefixKind` enum variant) Rep prefix or `RepPrefixKind.None`
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_stosd(lua, address_size: u32, rep_prefix: LuaDefaultU32<{iced_x86::RepPrefixKind::None as u32}>) -> 1 {
		let rep_prefix = unsafe { to_rep_prefix_kind(lua, rep_prefix) };
		let instr = Instruction { inner: match iced_x86::Instruction::with_stosd(address_size, rep_prefix) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `REP STOSD` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_rep_stosd(lua, address_size: u32) -> 1 {
		let instr = Instruction { inner: match iced_x86::Instruction::with_rep_stosd(address_size) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `STOSQ` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @param rep_prefix? integer #(default = `None`) (A `RepPrefixKind` enum variant) Rep prefix or `RepPrefixKind.None`
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_stosq(lua, address_size: u32, rep_prefix: LuaDefaultU32<{iced_x86::RepPrefixKind::None as u32}>) -> 1 {
		let rep_prefix = unsafe { to_rep_prefix_kind(lua, rep_prefix) };
		let instr = Instruction { inner: match iced_x86::Instruction::with_stosq(address_size, rep_prefix) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `REP STOSQ` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_rep_stosq(lua, address_size: u32) -> 1 {
		let instr = Instruction { inner: match iced_x86::Instruction::with_rep_stosq(address_size) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `CMPSB` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @param segment_prefix? integer #(default = `None`) (A `Register` enum variant) Segment override or `Register.None`
	/// @param rep_prefix? integer #(default = `None`) (A `RepPrefixKind` enum variant) Rep prefix or `RepPrefixKind.None`
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_cmpsb(lua, address_size: u32, segment_prefix: LuaDefaultU32<{iced_x86::Register::None as u32}>, rep_prefix: LuaDefaultU32<{iced_x86::RepPrefixKind::None as u32}>) -> 1 {
		let segment_prefix = unsafe { to_register(lua, segment_prefix) };
		let rep_prefix = unsafe { to_rep_prefix_kind(lua, rep_prefix) };
		let instr = Instruction { inner: match iced_x86::Instruction::with_cmpsb(address_size, segment_prefix, rep_prefix) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `REPE CMPSB` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_repe_cmpsb(lua, address_size: u32) -> 1 {
		let instr = Instruction { inner: match iced_x86::Instruction::with_repe_cmpsb(address_size) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `REPNE CMPSB` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_repne_cmpsb(lua, address_size: u32) -> 1 {
		let instr = Instruction { inner: match iced_x86::Instruction::with_repne_cmpsb(address_size) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `CMPSW` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @param segment_prefix? integer #(default = `None`) (A `Register` enum variant) Segment override or `Register.None`
	/// @param rep_prefix? integer #(default = `None`) (A `RepPrefixKind` enum variant) Rep prefix or `RepPrefixKind.None`
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_cmpsw(lua, address_size: u32, segment_prefix: LuaDefaultU32<{iced_x86::Register::None as u32}>, rep_prefix: LuaDefaultU32<{iced_x86::RepPrefixKind::None as u32}>) -> 1 {
		let segment_prefix = unsafe { to_register(lua, segment_prefix) };
		let rep_prefix = unsafe { to_rep_prefix_kind(lua, rep_prefix) };
		let instr = Instruction { inner: match iced_x86::Instruction::with_cmpsw(address_size, segment_prefix, rep_prefix) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `REPE CMPSW` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_repe_cmpsw(lua, address_size: u32) -> 1 {
		let instr = Instruction { inner: match iced_x86::Instruction::with_repe_cmpsw(address_size) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `REPNE CMPSW` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_repne_cmpsw(lua, address_size: u32) -> 1 {
		let instr = Instruction { inner: match iced_x86::Instruction::with_repne_cmpsw(address_size) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `CMPSD` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @param segment_prefix? integer #(default = `None`) (A `Register` enum variant) Segment override or `Register.None`
	/// @param rep_prefix? integer #(default = `None`) (A `RepPrefixKind` enum variant) Rep prefix or `RepPrefixKind.None`
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_cmpsd(lua, address_size: u32, segment_prefix: LuaDefaultU32<{iced_x86::Register::None as u32}>, rep_prefix: LuaDefaultU32<{iced_x86::RepPrefixKind::None as u32}>) -> 1 {
		let segment_prefix = unsafe { to_register(lua, segment_prefix) };
		let rep_prefix = unsafe { to_rep_prefix_kind(lua, rep_prefix) };
		let instr = Instruction { inner: match iced_x86::Instruction::with_cmpsd(address_size, segment_prefix, rep_prefix) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `REPE CMPSD` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_repe_cmpsd(lua, address_size: u32) -> 1 {
		let instr = Instruction { inner: match iced_x86::Instruction::with_repe_cmpsd(address_size) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `REPNE CMPSD` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_repne_cmpsd(lua, address_size: u32) -> 1 {
		let instr = Instruction { inner: match iced_x86::Instruction::with_repne_cmpsd(address_size) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `CMPSQ` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @param segment_prefix? integer #(default = `None`) (A `Register` enum variant) Segment override or `Register.None`
	/// @param rep_prefix? integer #(default = `None`) (A `RepPrefixKind` enum variant) Rep prefix or `RepPrefixKind.None`
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_cmpsq(lua, address_size: u32, segment_prefix: LuaDefaultU32<{iced_x86::Register::None as u32}>, rep_prefix: LuaDefaultU32<{iced_x86::RepPrefixKind::None as u32}>) -> 1 {
		let segment_prefix = unsafe { to_register(lua, segment_prefix) };
		let rep_prefix = unsafe { to_rep_prefix_kind(lua, rep_prefix) };
		let instr = Instruction { inner: match iced_x86::Instruction::with_cmpsq(address_size, segment_prefix, rep_prefix) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `REPE CMPSQ` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_repe_cmpsq(lua, address_size: u32) -> 1 {
		let instr = Instruction { inner: match iced_x86::Instruction::with_repe_cmpsq(address_size) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `REPNE CMPSQ` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_repne_cmpsq(lua, address_size: u32) -> 1 {
		let instr = Instruction { inner: match iced_x86::Instruction::with_repne_cmpsq(address_size) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `MOVSB` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @param segment_prefix? integer #(default = `None`) (A `Register` enum variant) Segment override or `Register.None`
	/// @param rep_prefix? integer #(default = `None`) (A `RepPrefixKind` enum variant) Rep prefix or `RepPrefixKind.None`
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_movsb(lua, address_size: u32, segment_prefix: LuaDefaultU32<{iced_x86::Register::None as u32}>, rep_prefix: LuaDefaultU32<{iced_x86::RepPrefixKind::None as u32}>) -> 1 {
		let segment_prefix = unsafe { to_register(lua, segment_prefix) };
		let rep_prefix = unsafe { to_rep_prefix_kind(lua, rep_prefix) };
		let instr = Instruction { inner: match iced_x86::Instruction::with_movsb(address_size, segment_prefix, rep_prefix) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `REP MOVSB` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_rep_movsb(lua, address_size: u32) -> 1 {
		let instr = Instruction { inner: match iced_x86::Instruction::with_rep_movsb(address_size) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `MOVSW` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @param segment_prefix? integer #(default = `None`) (A `Register` enum variant) Segment override or `Register.None`
	/// @param rep_prefix? integer #(default = `None`) (A `RepPrefixKind` enum variant) Rep prefix or `RepPrefixKind.None`
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_movsw(lua, address_size: u32, segment_prefix: LuaDefaultU32<{iced_x86::Register::None as u32}>, rep_prefix: LuaDefaultU32<{iced_x86::RepPrefixKind::None as u32}>) -> 1 {
		let segment_prefix = unsafe { to_register(lua, segment_prefix) };
		let rep_prefix = unsafe { to_rep_prefix_kind(lua, rep_prefix) };
		let instr = Instruction { inner: match iced_x86::Instruction::with_movsw(address_size, segment_prefix, rep_prefix) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `REP MOVSW` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_rep_movsw(lua, address_size: u32) -> 1 {
		let instr = Instruction { inner: match iced_x86::Instruction::with_rep_movsw(address_size) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `MOVSD` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @param segment_prefix? integer #(default = `None`) (A `Register` enum variant) Segment override or `Register.None`
	/// @param rep_prefix? integer #(default = `None`) (A `RepPrefixKind` enum variant) Rep prefix or `RepPrefixKind.None`
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_movsd(lua, address_size: u32, segment_prefix: LuaDefaultU32<{iced_x86::Register::None as u32}>, rep_prefix: LuaDefaultU32<{iced_x86::RepPrefixKind::None as u32}>) -> 1 {
		let segment_prefix = unsafe { to_register(lua, segment_prefix) };
		let rep_prefix = unsafe { to_rep_prefix_kind(lua, rep_prefix) };
		let instr = Instruction { inner: match iced_x86::Instruction::with_movsd(address_size, segment_prefix, rep_prefix) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `REP MOVSD` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_rep_movsd(lua, address_size: u32) -> 1 {
		let instr = Instruction { inner: match iced_x86::Instruction::with_rep_movsd(address_size) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `MOVSQ` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @param segment_prefix? integer #(default = `None`) (A `Register` enum variant) Segment override or `Register.None`
	/// @param rep_prefix? integer #(default = `None`) (A `RepPrefixKind` enum variant) Rep prefix or `RepPrefixKind.None`
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_movsq(lua, address_size: u32, segment_prefix: LuaDefaultU32<{iced_x86::Register::None as u32}>, rep_prefix: LuaDefaultU32<{iced_x86::RepPrefixKind::None as u32}>) -> 1 {
		let segment_prefix = unsafe { to_register(lua, segment_prefix) };
		let rep_prefix = unsafe { to_rep_prefix_kind(lua, rep_prefix) };
		let instr = Instruction { inner: match iced_x86::Instruction::with_movsq(address_size, segment_prefix, rep_prefix) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `REP MOVSQ` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_rep_movsq(lua, address_size: u32) -> 1 {
		let instr = Instruction { inner: match iced_x86::Instruction::with_rep_movsq(address_size) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `MASKMOVQ` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @param register1 integer #(A `Register` enum variant) Register
	/// @param register2 integer #(A `Register` enum variant) Register
	/// @param segment_prefix? integer #(default = `None`) (A `Register` enum variant) Segment override or `Register.None`
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_maskmovq(lua, address_size: u32, register1: u32, register2: u32, segment_prefix: LuaDefaultU32<{iced_x86::Register::None as u32}>) -> 1 {
		let register1 = unsafe { to_register(lua, register1) };
		let register2 = unsafe { to_register(lua, register2) };
		let segment_prefix = unsafe { to_register(lua, segment_prefix) };
		let instr = Instruction { inner: match iced_x86::Instruction::with_maskmovq(address_size, register1, register2, segment_prefix) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `MASKMOVDQU` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @param register1 integer #(A `Register` enum variant) Register
	/// @param register2 integer #(A `Register` enum variant) Register
	/// @param segment_prefix? integer #(default = `None`) (A `Register` enum variant) Segment override or `Register.None`
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_maskmovdqu(lua, address_size: u32, register1: u32, register2: u32, segment_prefix: LuaDefaultU32<{iced_x86::Register::None as u32}>) -> 1 {
		let register1 = unsafe { to_register(lua, register1) };
		let register2 = unsafe { to_register(lua, register2) };
		let segment_prefix = unsafe { to_register(lua, segment_prefix) };
		let instr = Instruction { inner: match iced_x86::Instruction::with_maskmovdqu(address_size, register1, register2, segment_prefix) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}

	/// Creates a `VMASKMOVDQU` instruction
	///
	/// @param address_size integer #(`u32`) 16, 32, or 64
	/// @param register1 integer #(A `Register` enum variant) Register
	/// @param register2 integer #(A `Register` enum variant) Register
	/// @param segment_prefix? integer #(default = `None`) (A `Register` enum variant) Segment override or `Register.None`
	/// @return Instruction
	#[rustfmt::skip]
	unsafe fn create_vmaskmovdqu(lua, address_size: u32, register1: u32, register2: u32, segment_prefix: LuaDefaultU32<{iced_x86::Register::None as u32}>) -> 1 {
		let register1 = unsafe { to_register(lua, register1) };
		let register2 = unsafe { to_register(lua, register2) };
		let segment_prefix = unsafe { to_register(lua, segment_prefix) };
		let instr = Instruction { inner: match iced_x86::Instruction::with_vmaskmovdqu(address_size, register1, register2, segment_prefix) {
			Ok(instr) => instr,
			Err(e) => unsafe { lua.throw_error(e) },
		}};
		let _ = unsafe { Instruction::init_and_push(lua, &instr) };
	}
	// GENERATOR-END: Create
}

unsafe fn push_used_registers(lua: &Lua<'_>, info: &iced_x86::InstructionInfo) {
	unsafe {
		lua.push_array2(info.used_registers(), |lua, used_reg| {
			let used_reg = UsedRegister { inner: *used_reg };
			let _ = UsedRegister::init_and_push(lua, &used_reg);
		})
	}
}

unsafe fn push_used_memory(lua: &Lua<'_>, info: &iced_x86::InstructionInfo) {
	unsafe {
		lua.push_array2(info.used_memory(), |lua, used_mem| {
			let used_mem = UsedMemory { inner: *used_mem };
			let _ = UsedMemory::init_and_push(lua, &used_mem);
		})
	}
}

unsafe fn push_op_accesses(lua: &Lua<'_>, info: &iced_x86::InstructionInfo, instr: &iced_x86::Instruction) {
	let op_count = instr.op_count();
	unsafe {
		lua.create_table(op_count as c_int, 0);
		for i in 0..op_count {
			lua.push(i + 1);
			lua.push(info.op_access(i) as u32);
			lua.raw_set(-3);
		}
	}
}

lua_methods! {
	unsafe fn instruction_tostring(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.to_string()) }
	}

	unsafe fn instruction_eq(lua, instr: &Instruction, instr2: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner == instr2.inner) }
	}

	unsafe fn instruction_len(lua, instr: &Instruction) -> 1 {
		unsafe { lua.push(instr.inner.len()) }
	}
}
