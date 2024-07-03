// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

pub(super) mod enums;
mod fmt_data;
mod fmt_tbl;
mod mem_size_tbl;
mod options;
mod pseudo_ops_fast;
mod regs;
#[cfg(test)]
mod tests;
mod trait_options;
mod trait_options_fast_fmt;

use crate::formatter::fast::enums::*;
use crate::formatter::fast::fmt_tbl::FMT_DATA;
use crate::formatter::fast::mem_size_tbl::MEM_SIZE_TBL;
pub use crate::formatter::fast::options::*;
use crate::formatter::fast::pseudo_ops_fast::get_pseudo_ops;
use crate::formatter::fast::regs::REGS_TBL;
pub use crate::formatter::fast::trait_options::*;
pub use crate::formatter::fast::trait_options_fast_fmt::*;
use crate::formatter::fmt_utils_all::*;
use crate::formatter::instruction_internal::{self, get_address_size_in_bytes};
use crate::formatter::*;
use crate::iced_constants::IcedConstants;
use crate::*;
use alloc::boxed::Box;
use alloc::vec::Vec;
use core::marker::PhantomData;
use core::{mem, ptr, slice};

// full fmt'd str = "prefixes mnemonic op0<decorators1>, op1, op2, op3, op4<decorators2>"
// prefixes = "es xacquire xrelease lock notrack repe repne "
// mnemonic = "prefetch_exclusive"
// op sep = ", "
// op = "fpustate108 ptr fs:[rax+zmm31*8+0x12345678]"
//		- longest 'xxxx ptr' and longest memory operand
// op = "0x123456789ABCDEF0"
// op = "0x1234:0x12345678"
// op = "zmm31"
// op = "offset symbol (123456789ABCDEF0)"
//		- symbol can have any length
// <decorators1> = "{k3}{z}"
// <decorators2> = "{rn-sae}"
// symbol = any length + optional address " (123456789ABCDEF0)"
// full = "es xacquire xrelease lock notrack repe repne prefetch_exclusive fpustate108 ptr fs:[rax+zmm31*8+0x12345678]{k3}{z}, fpustate108 ptr fs:[rax+zmm31*8+0x12345678], fpustate108 ptr fs:[rax+zmm31*8+0x12345678], fpustate108 ptr fs:[rax+zmm31*8+0x12345678], fpustate108 ptr fs:[rax+zmm31*8+0x12345678]{rn-sae}"
//		- it's not possible to have 5 `fpustate108 ptr fs:[rax+zmm31*8+0x12345678]` operands
//		  so we'll never get a formatted string this long if there's no symbol resolver.
#[allow(dead_code)]
const MAX_FMT_INSTR_LEN: usize = {
	const MAX_PREFIXES_LEN: usize = "es xacquire xrelease lock notrack repe repne ".len();
	const MAX_OPERAND_LEN: usize = "fpustate108 ptr fs:[rax+zmm31*8+0x12345678]".len();
	const MAX_DECORATOR1_LEN: usize = "{k3}{z}{eh}".len();
	const MAX_DECORATOR2_LEN: usize = "{rn-sae}{float16}".len();

	MAX_PREFIXES_LEN
	+ crate::formatter::strings_data::MAX_STRING_LEN
	+ MAX_DECORATOR1_LEN
	+ (IcedConstants::MAX_OP_COUNT * (2/*", "*/ + MAX_OPERAND_LEN)) - 1/*','*/
	+ MAX_DECORATOR2_LEN
};
const _: () = assert!(
	MAX_FMT_INSTR_LEN ==
	// Max mnemonic len
	crate::formatter::strings_data::MAX_STRING_LEN
		+ "es xacquire xrelease lock notrack repe repne  \
			fpustate108 ptr fs:[rax+zmm31*8+0x12345678]{k3}{z}{eh}, \
			fpustate108 ptr fs:[rax+zmm31*8+0x12345678], \
			fpustate108 ptr fs:[rax+zmm31*8+0x12345678], \
			fpustate108 ptr fs:[rax+zmm31*8+0x12345678], \
			fpustate108 ptr fs:[rax+zmm31*8+0x12345678]{rn-sae}{float16}"
			.len()
);
// Make sure it doesn't grow too much without us knowing about it (eg. if more operands are added)
const _: () = assert!(MAX_FMT_INSTR_LEN < 350);

#[derive(Copy, Clone)]
#[repr(C)]
struct FastStringRepr<const SIZE: usize> {
	len: u8,
	// SIZE bytes can be read but only the first len bytes are part of the string
	data: [u8; SIZE],
}

impl<const SIZE: usize> FastStringRepr<SIZE> {
	const fn from_str(s: &str) -> Self {
		assert!(s.len() <= SIZE);
		let s = s.as_bytes();
		let len = s.len() as u8;
		let mut data = [0; SIZE];
		let mut i = 0;
		while i < SIZE {
			data[i] = if i < s.len() { s[i] } else { b' ' };
			i += 1;
		}
		Self { len, data }
	}
}

#[repr(transparent)]
#[derive(Copy, Clone)]
struct FastString<const SIZE: usize> {
	// offset 0: u8, length in bytes of utf8 string
	len_data: &'static FastStringRepr<SIZE>,
}

impl<const SIZE: usize> FastString<SIZE> {
	const SIZE: usize = SIZE;

	#[allow(dead_code)]
	#[allow(clippy::transmute_ptr_to_ref)] // Can't use pointer dereference in const fn yet
	const fn from_raw(len_data: &'static [u8]) -> Self {
		let required_size = 1 + SIZE;
		assert!(len_data.len() >= required_size);
		assert!(len_data[0] as usize <= SIZE);
		let len_data: &'static FastStringRepr<SIZE> = unsafe { mem::transmute(len_data.as_ptr()) };
		Self::new(len_data)
	}

	const fn new(len_data: &'static FastStringRepr<SIZE>) -> Self {
		Self { len_data }
	}

	const fn len(self) -> usize {
		self.len_data.len as usize
	}

	const fn utf8_data(self) -> *const u8 {
		self.len_data.data.as_ptr()
	}

	#[allow(dead_code)]
	fn get_slice(self) -> &'static [u8] {
		unsafe { slice::from_raw_parts(self.utf8_data(), self.len()) }
	}
}

// FastString2 isn't used since the code needs a 66h prefix (if target CPU is x86)
type FastString4 = FastString<4>; // ld 4
type FastString8 = FastString<8>; // ld 8
type FastString12 = FastString<12>; // ld 8 + ld 4
type FastString16 = FastString<16>; // ld 16
type FastString20 = FastString<20>; // ld 16 + ld 4

type FastStringMnemonic = FastString20;
type FastStringMemorySize = FastString16;
type FastStringRegister = FastString8;

macro_rules! mk_const_fast_str {
	($fast_ty:ty, $str:literal) => {{
		use $crate::formatter::fast::{FastString, FastStringRepr};

		// Eventually, inline-const will let us do:
		// const { FastString::new(&FastStringRepr::from_str($str)) }
		// to avoid having to pass a fast string type, and let it be inferred.
		const RES: $fast_ty = FastString::new(&FastStringRepr::from_str($str));
		RES
	}};
}
use mk_const_fast_str;

macro_rules! verify_output_has_enough_bytes_left {
	($dst:ident, $dst_next_p:ident, $num_bytes:expr) => {
		// SAFETY: This is an opt out feature so if this returns `false`, they know what they're doing.
		if unsafe { TraitOptions::verify_output_has_enough_bytes_left() } {
			// Verify that there's enough bytes left. This should never fail (because we've called
			// `$dst.reserve(MAX_FMT_INSTR_LEN)`).
			iced_assert!($dst.capacity() - ($dst_next_p as usize - $dst.as_ptr() as usize) >= $num_bytes);
		}
	};
}

macro_rules! write_fast_str {
	// $dst = dest vector (from output.as_mut_vec())
	// $dst_next_p = next ptr to write in $dst
	// $source_ty = source fast string type
	// $source = source fast string instance, must be the same type as $source_ty (compiler will give an error if it's not the same type)
	($dst:ident, $dst_next_p:ident, $source_ty:ty, $source:ident) => {{
		const DATA_LEN: usize = <$source_ty>::SIZE;
		verify_output_has_enough_bytes_left!($dst, $dst_next_p, DATA_LEN);
		// SAFETY:
		// - $source is a valid utf8 string and it points to DATA_LEN readable bytes
		//   ($source is never from user code)
		// - $source is not in $dst ($source is static)
		// - $dst is writable with at least DATA_LEN bytes left (see assert above)
		// - $dst is at a valid utf8 char boundary (we're appending bytes)
		unsafe {
			ptr::copy_nonoverlapping(<$source_ty>::utf8_data($source), $dst_next_p, DATA_LEN);
		}
		debug_assert!(<$source_ty>::len($source) <= DATA_LEN);
		// SAFETY:
		// - $source.len() <= DATA_LEN so the new ptr is valid
		$dst_next_p = unsafe { $dst_next_p.add(<$source_ty>::len($source)) };
	}};
}

static HEX_GROUP2_UPPER: &str = "\
	000102030405060708090A0B0C0D0E0F\
	101112131415161718191A1B1C1D1E1F\
	202122232425262728292A2B2C2D2E2F\
	303132333435363738393A3B3C3D3E3F\
	404142434445464748494A4B4C4D4E4F\
	505152535455565758595A5B5C5D5E5F\
	606162636465666768696A6B6C6D6E6F\
	707172737475767778797A7B7C7D7E7F\
	808182838485868788898A8B8C8D8E8F\
	909192939495969798999A9B9C9D9E9F\
	A0A1A2A3A4A5A6A7A8A9AAABACADAEAF\
	B0B1B2B3B4B5B6B7B8B9BABBBCBDBEBF\
	C0C1C2C3C4C5C6C7C8C9CACBCCCDCECF\
	D0D1D2D3D4D5D6D7D8D9DADBDCDDDEDF\
	E0E1E2E3E4E5E6E7E8E9EAEBECEDEEEF\
	F0F1F2F3F4F5F6F7F8F9FAFBFCFDFEFF\
	__"; // Padding so we can read 4 bytes at every index 0-0xFF inclusive

macro_rules! write_fast_hex2_rw_4bytes {
	($dst:ident, $dst_next_p:ident, $value:ident, $lower_or_value:ident, $check_limit:literal) => {{
		const DATA_LEN: usize = 4;
		const REAL_LEN: usize = 2;
		if $check_limit {
			verify_output_has_enough_bytes_left!($dst, $dst_next_p, DATA_LEN);
		}
		// We'll read DATA_LEN (4) bytes so we must be able to access up to and including offset 0x201
		debug_assert_eq!(HEX_GROUP2_UPPER.len(), 0xFF * REAL_LEN + DATA_LEN);
		debug_assert!($value < 0x100);
		// $lower_or_value == 0 if we should use uppercase hex digits or 0x2020_2020 to use lowercase hex digits.
		// If LE, we need xxxx2020 and if BE, we need 2020xxxx.
		debug_assert!($lower_or_value == 0 || $lower_or_value == 0x2020_2020);
		// SAFETY:
		// - HEX_GROUP2_UPPER is a valid utf8 string and every valid 2-digit hex number
		//	 0-0xFF can be used as an index * REAL_LEN (2) to read DATA_LEN (4) bytes.
		// - $dst is writable with at least DATA_LEN bytes left (see assert above)
		// - $dst is at a valid utf8 char boundary (we're appending bytes)
		#[allow(trivial_numeric_casts)]
		unsafe {
			let src_ptr = HEX_GROUP2_UPPER.as_ptr().add(($value as usize) * REAL_LEN) as *const u32;
			ptr::write_unaligned($dst_next_p as *mut u32, ptr::read_unaligned(src_ptr) | $lower_or_value);
		}
		const _: () = assert!(REAL_LEN <= DATA_LEN);
		// SAFETY:
		// - REAL_LEN <= DATA_LEN so the new ptr is valid since there's at least DATA_LEN bytes available in $dst
		$dst_next_p = unsafe { $dst_next_p.add(REAL_LEN) };
	}};
}

macro_rules! write_fast_ascii_char {
	// $dst = dest vector (from output.as_mut_vec())
	// $dst_next_p = next ptr to write in $dst
	// $ch = char to write (must be ASCII)
	($dst:ident, $dst_next_p:ident, $ch:expr, $check_limit:literal) => {{
		const DATA_LEN: usize = 1;
		if $check_limit {
			verify_output_has_enough_bytes_left!($dst, $dst_next_p, DATA_LEN);
		}
		#[allow(trivial_numeric_casts)]
		{
			debug_assert!($ch as u32 <= 0x7F);
		}
		// SAFETY:
		// - $ch is ASCII (valid 1-byte utf8 char)
		// - $dst is writable with at least DATA_LEN bytes left (see assert above)
		// - $dst is at a valid utf8 char boundary (we're appending bytes)
		#[allow(trivial_numeric_casts)]
		unsafe {
			*$dst_next_p = $ch as u8;
		}
		// SAFETY: There's at least one byte left so the new ptr is valid
		$dst_next_p = unsafe { $dst_next_p.add(1) };
	}};
}

macro_rules! write_fast_ascii_char_lit {
	// $dst = dest vector (from output.as_mut_vec())
	// $dst_next_p = next ptr to write in $dst
	// $ch = char to write (must be ASCII)
	($dst:ident, $dst_next_p:ident, $ch:tt, $check_limit:literal) => {{
		const _: () = assert!($ch as u32 <= 0x7F);
		write_fast_ascii_char!($dst, $dst_next_p, $ch, $check_limit);
	}};
}

macro_rules! update_vec_len {
	// $dst = dest vector (from output.as_mut_vec())
	// $dst_next_p = next ptr to write in $dst
	($dst:ident, $dst_next_p:ident) => {
		// SAFETY:
		// - we only write valid utf8 strings and ASCII chars to vec
		// - We've written all chars up to but not including $dst_next_p so all visible data have been initialized
		// - $dst_next_p points to a valid location inside the vec or at most 1 byte past the last valid byte
		unsafe {
			$dst.set_len($dst_next_p as usize - $dst.as_ptr() as usize);
		}
	};
}

macro_rules! use_dst_only_now {
	// $dst = dest vector (from output.as_mut_vec())
	// $dst_next_p = next ptr to write in $dst
	($dst:ident, $dst_next_p:ident) => {
		update_vec_len!($dst, $dst_next_p);
		// Make sure we don't use it accidentally
		#[allow(unused_variables)]
		let $dst_next_p: () = ();
	};
}
macro_rules! use_dst_next_p_now {
	// $dst = dest vector (from output.as_mut_vec())
	// $dst_next_p = next ptr to write in $dst
	($dst:ident, $dst_next_p:ident) => {
		// Need to make sure we have enough bytes available again because we could've
		// written a very long symbol name.
		$dst.reserve(MAX_FMT_INSTR_LEN);
		// Restore variable
		let mut $dst_next_p = unsafe { $dst.as_mut_ptr().add($dst.len()) };
	};
}

// Macros to safely call the methods (make sure the return value is stored back in dst_next_p)
macro_rules! call_format_register {
	($slf:ident, $dst:ident, $dst_next_p:ident, $reg:expr) => {{
		$dst_next_p = $slf.format_register($dst, $dst_next_p, $reg);
	}};
}
macro_rules! call_format_number {
	($slf:ident, $dst:ident, $dst_next_p:ident, $imm:expr) => {{
		$dst_next_p = $slf.format_number($dst, $dst_next_p, $imm);
	}};
}
macro_rules! call_write_symbol {
	($slf:ident, $dst:ident, $dst_next_p:ident, $imm:expr, $sym:expr) => {{
		$dst_next_p = $slf.write_symbol($dst, $dst_next_p, $imm, $sym);
	}};
}
macro_rules! call_write_symbol2 {
	($slf:ident, $dst:ident, $dst_next_p:ident, $imm:expr, $sym:expr, $write_minus_if_signed:literal) => {{
		$dst_next_p = $slf.write_symbol2($dst, $dst_next_p, $imm, $sym, $write_minus_if_signed);
	}};
}
macro_rules! format_memory_else_block {
	($slf:ident, $dst:ident, $dst_next_p:ident, $need_plus:ident, $displ_size:ident, $displ:ident, $addr_size:ident) => {
		if !$need_plus || ($displ_size != 0 && $displ != 0) {
			if $need_plus {
				let c = if $addr_size == 8 {
					if $displ < 0 {
						$displ = $displ.wrapping_neg();
						b'-'
					} else {
						b'+'
					}
				} else if $addr_size == 4 {
					if ($displ as i32) < 0 {
						$displ = ($displ as i32).wrapping_neg() as u32 as i64;
						b'-'
					} else {
						b'+'
					}
				} else {
					debug_assert_eq!($addr_size, 2);
					if ($displ as i16) < 0 {
						$displ = ($displ as i16).wrapping_neg() as u16 as i64;
						b'-'
					} else {
						b'+'
					}
				};
				write_fast_ascii_char!($dst, $dst_next_p, c, true);
			}
			call_format_number!($slf, $dst, $dst_next_p, $displ as u64);
		}
	};
}
// Only one caller has variable args starting from $seg_reg so this is a macro. The compiler is able
// to remove lots of code in all the other cases with literal macro args.
macro_rules! format_memory_code {
	($slf:ident, $dst:ident, $dst_next_p:ident, $instruction:ident, $operand:expr, $seg_reg:expr, $base_reg:expr, $index_reg:expr,
		$scale:expr, $displ_size:expr, $displ:expr, $addr_size:expr) => {
		#[allow(trivial_numeric_casts)]
		{
			let mut base_reg = $base_reg;
			let mut displ_size: u32 = $displ_size;
			let mut displ: i64 = $displ;
			debug_assert!(get_address_size_in_bytes(base_reg, $index_reg, displ_size, $instruction.code_size()) == $addr_size);

			let abs_addr;
			if base_reg == Register::RIP {
				abs_addr = displ as u64;
				if TraitOptions::rip_relative_addresses(&$slf.d.options) {
					displ = displ.wrapping_sub($instruction.next_ip() as i64);
				} else {
					debug_assert_eq!($index_reg, Register::None);
					base_reg = Register::None;
				}
				displ_size = 8;
			} else if base_reg == Register::EIP {
				abs_addr = displ as u32 as u64;
				if TraitOptions::rip_relative_addresses(&$slf.d.options) {
					displ = (displ as u32).wrapping_sub($instruction.next_ip32()) as i32 as i64;
				} else {
					debug_assert_eq!($index_reg, Register::None);
					base_reg = Register::None;
				}
				displ_size = 4;
			} else {
				abs_addr = displ as u64;
			}

			let show_mem_size = TraitOptions::always_show_memory_size(&$slf.d.options) || {
				let flags = $slf.d.code_flags[$instruction.code() as usize];
				(flags & (FastFmtFlags::FORCE_MEM_SIZE as u8)) != 0 || $instruction.is_broadcast()
			};
			if show_mem_size {
				let keywords = $slf.d.all_memory_sizes[$instruction.memory_size() as usize];
				write_fast_str!($dst, $dst_next_p, FastStringMemorySize, keywords);
			}

			let seg_override;
			if TraitOptions::always_show_segment_register(&$slf.d.options)
				|| ({
					seg_override = $instruction.segment_prefix();
					seg_override != Register::None
				} && !{
					let notrack_prefix = seg_override == Register::DS && is_notrack_prefix_branch($instruction.code()) && {
						let code_size = $instruction.code_size();
						!((code_size == CodeSize::Code16 || code_size == CodeSize::Code32)
							&& (base_reg == Register::BP || base_reg == Register::EBP || base_reg == Register::ESP))
					};
					notrack_prefix
				} && (SpecializedFormatter::<TraitOptions>::SHOW_USELESS_PREFIXES
					|| show_segment_prefix_bool(Register::None, $instruction, SpecializedFormatter::<TraitOptions>::SHOW_USELESS_PREFIXES)))
			{
				call_format_register!($slf, $dst, $dst_next_p, $seg_reg);
				write_fast_ascii_char_lit!($dst, $dst_next_p, b':', true);
			}
			write_fast_ascii_char_lit!($dst, $dst_next_p, b'[', true);

			let mut need_plus = if base_reg != Register::None {
				call_format_register!($slf, $dst, $dst_next_p, base_reg);
				true
			} else {
				false
			};

			if $index_reg != Register::None {
				if need_plus {
					write_fast_ascii_char_lit!($dst, $dst_next_p, b'+', true);
				}
				need_plus = true;

				call_format_register!($slf, $dst, $dst_next_p, $index_reg);

				// [rsi] = base reg, [rsi*1] = index reg
				if $addr_size != 2 && ($scale != 0 || base_reg == Register::None) {
					let scale_str = SCALE_NUMBERS[$scale as usize];
					write_fast_str!($dst, $dst_next_p, FastString4, scale_str);
				}
			}

			if TraitOptions::ENABLE_SYMBOL_RESOLVER {
				// See OpKind::NearBranch16 in format() for why we clone the symbols
				let mut vec: Vec<SymResTextPart<'_>> = Vec::new();
				if let Some(ref symbol) = if let Some(ref mut symbol_resolver) = $slf.symbol_resolver {
					to_owned(symbol_resolver.symbol($instruction, $operand, Some($operand), abs_addr, $addr_size), &mut vec)
				} else {
					None
				} {
					if need_plus {
						let c = if (symbol.flags & SymbolFlags::SIGNED) != 0 { b'-' } else { b'+' };
						write_fast_ascii_char!($dst, $dst_next_p, c, true);
					} else if (symbol.flags & SymbolFlags::SIGNED) != 0 {
						write_fast_ascii_char_lit!($dst, $dst_next_p, b'-', true);
					}

					call_write_symbol2!($slf, $dst, $dst_next_p, abs_addr, symbol, false);
				} else {
					let addr_size = $addr_size;
					format_memory_else_block!($slf, $dst, $dst_next_p, need_plus, displ_size, displ, addr_size);
				}
			} else {
				let addr_size = $addr_size;
				format_memory_else_block!($slf, $dst, $dst_next_p, need_plus, displ_size, displ, addr_size);
			}

			write_fast_ascii_char_lit!($dst, $dst_next_p, b']', true);
		}
	};
}
macro_rules! call_format_memory {
	($slf:ident, $dst:ident, $dst_next_p:ident, $instruction:ident, $operand:ident, $seg_reg:expr, $base_reg:tt,
		$index_reg:tt, $scale:tt, $displ_size:tt, $displ:tt, $addr_size:tt $(,)?) => {
		// This speeds up SpecializedFormatter but slows down FastFormatter so detect which
		// formatter it is. Both paths are tested (same tests).
		// This is fugly but the whole point of this formatter is to be fast which can result in ugly code.
		{
			if TraitOptions::__IS_FAST_FORMATTER {
				// Less code: call a method
				$dst_next_p = $slf.format_memory(
					$dst,
					$dst_next_p,
					$instruction,
					$operand,
					$seg_reg,
					$base_reg,
					$index_reg,
					$scale,
					$displ_size,
					$displ,
					$addr_size,
				)
			} else {
				// The options are all most likely hard coded so inline and specialize the 'method call'
				format_memory_code!(
					$slf,
					$dst,
					$dst_next_p,
					$instruction,
					$operand,
					$seg_reg,
					$base_reg,
					$index_reg,
					$scale,
					$displ_size,
					$displ,
					$addr_size
				)
			}
		}
	};
}

static SCALE_NUMBERS: [FastString4; 4] = [
	mk_const_fast_str!(FastString4, "*1"),
	mk_const_fast_str!(FastString4, "*2"),
	mk_const_fast_str!(FastString4, "*4"),
	mk_const_fast_str!(FastString4, "*8"),
];
const _: () = assert!(RoundingControl::None as u32 == 0);
const _: () = assert!(RoundingControl::RoundToNearest as u32 == 1);
const _: () = assert!(RoundingControl::RoundDown as u32 == 2);
const _: () = assert!(RoundingControl::RoundUp as u32 == 3);
const _: () = assert!(RoundingControl::RoundTowardZero as u32 == 4);
static RC_SAE_STRINGS: [FastString8; IcedConstants::ROUNDING_CONTROL_ENUM_COUNT] = [
	mk_const_fast_str!(FastString8, ""),
	mk_const_fast_str!(FastString8, "{rn-sae}"),
	mk_const_fast_str!(FastString8, "{rd-sae}"),
	mk_const_fast_str!(FastString8, "{ru-sae}"),
	mk_const_fast_str!(FastString8, "{rz-sae}"),
];
static RC_STRINGS: [FastString4; IcedConstants::ROUNDING_CONTROL_ENUM_COUNT] = [
	mk_const_fast_str!(FastString4, ""),
	mk_const_fast_str!(FastString4, "{rn}"),
	mk_const_fast_str!(FastString4, "{rd}"),
	mk_const_fast_str!(FastString4, "{ru}"),
	mk_const_fast_str!(FastString4, "{rz}"),
];
#[cfg(feature = "mvex")]
static MVEX_REG_MEM_CONSTS_32: [FastString12; IcedConstants::MVEX_REG_MEM_CONV_ENUM_COUNT] = [
	mk_const_fast_str!(FastString12, ""),
	mk_const_fast_str!(FastString12, ""),
	mk_const_fast_str!(FastString12, "{cdab}"),
	mk_const_fast_str!(FastString12, "{badc}"),
	mk_const_fast_str!(FastString12, "{dacb}"),
	mk_const_fast_str!(FastString12, "{aaaa}"),
	mk_const_fast_str!(FastString12, "{bbbb}"),
	mk_const_fast_str!(FastString12, "{cccc}"),
	mk_const_fast_str!(FastString12, "{dddd}"),
	mk_const_fast_str!(FastString12, ""),
	mk_const_fast_str!(FastString12, "{1to16}"),
	mk_const_fast_str!(FastString12, "{4to16}"),
	mk_const_fast_str!(FastString12, "{float16}"),
	mk_const_fast_str!(FastString12, "{uint8}"),
	mk_const_fast_str!(FastString12, "{sint8}"),
	mk_const_fast_str!(FastString12, "{uint16}"),
	mk_const_fast_str!(FastString12, "{sint16}"),
];
#[cfg(feature = "mvex")]
static MVEX_REG_MEM_CONSTS_64: [FastString12; IcedConstants::MVEX_REG_MEM_CONV_ENUM_COUNT] = [
	mk_const_fast_str!(FastString12, ""),
	mk_const_fast_str!(FastString12, ""),
	mk_const_fast_str!(FastString12, "{cdab}"),
	mk_const_fast_str!(FastString12, "{badc}"),
	mk_const_fast_str!(FastString12, "{dacb}"),
	mk_const_fast_str!(FastString12, "{aaaa}"),
	mk_const_fast_str!(FastString12, "{bbbb}"),
	mk_const_fast_str!(FastString12, "{cccc}"),
	mk_const_fast_str!(FastString12, "{dddd}"),
	mk_const_fast_str!(FastString12, ""),
	mk_const_fast_str!(FastString12, "{1to8}"),
	mk_const_fast_str!(FastString12, "{4to8}"),
	mk_const_fast_str!(FastString12, "{float16}"),
	mk_const_fast_str!(FastString12, "{uint8}"),
	mk_const_fast_str!(FastString12, "{sint8}"),
	mk_const_fast_str!(FastString12, "{uint16}"),
	mk_const_fast_str!(FastString12, "{sint16}"),
];

struct FmtTableData {
	mnemonics: Box<[FastStringMnemonic; IcedConstants::CODE_ENUM_COUNT]>,
	flags: Box<[u8; IcedConstants::CODE_ENUM_COUNT]>, // FastFmtFlags
}

/// Fast specialized formatter with less formatting options and with a masm-like syntax.
/// Use it if formatting speed is more important than being able to re-assemble formatted instructions.
///
/// The `TraitOptions` generic parameter is a [`SpecializedFormatterTraitOptions`] trait. It can
/// be used to hard code options so the compiler can create a smaller and faster formatter.
/// See also [`FastFormatter`] which allows changing the options at runtime at the cost of
/// being a little bit slower and using a little bit more code.
///
/// This formatter is ~3.3x faster than the gas/intel/masm/nasm formatters (the time includes decoding + formatting).
///
/// [`SpecializedFormatterTraitOptions`]: trait.SpecializedFormatterTraitOptions.html
/// [`FastFormatter`]: type.FastFormatter.html
///
/// # Examples
///
/// ```
/// use iced_x86::*;
///
/// let bytes = b"\x62\xF2\x4F\xDD\x72\x50\x01";
/// let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
/// let instr = decoder.decode();
///
/// // If you like the default options, you can also use DefaultSpecializedFormatterTraitOptions
/// // instead of impl the options trait.
/// struct MyTraitOptions;
/// impl SpecializedFormatterTraitOptions for MyTraitOptions {
///     fn space_after_operand_separator(_options: &FastFormatterOptions) -> bool {
///         // We hard code the value to `true` which means it's not possible to
///         // change this option at runtime, i.e., this will do nothing:
///         //      formatter.options_mut().set_space_after_operand_separator(false);
///         true
///     }
///     fn rip_relative_addresses(options: &FastFormatterOptions) -> bool {
///         // Since we return the input, we can change this value at runtime, i.e.,
///         // this works:
///         //      formatter.options_mut().set_rip_relative_addresses(false);
///         options.rip_relative_addresses()
///     }
/// }
/// type MyFormatter = SpecializedFormatter<MyTraitOptions>;
///
/// let mut output = String::new();
/// let mut formatter = MyFormatter::new();
/// formatter.format(&instr, &mut output);
/// assert_eq!(output, "vcvtne2ps2bf16 zmm2{k5}{z}, zmm6, dword bcst [rax+0x4]");
/// ```
///
/// # Fastest possible disassembly
///
/// For fastest possible disassembly, you should *not* enable the `db` feature (or you should set [`ENABLE_DB_DW_DD_DQ`] to `false`)
/// and you should also override the unsafe [`verify_output_has_enough_bytes_left()`] and return `false`.
///
/// [`ENABLE_DB_DW_DD_DQ`]: trait.SpecializedFormatterTraitOptions.html#associatedconstant.ENABLE_DB_DW_DD_DQ
/// [`verify_output_has_enough_bytes_left()`]: trait.SpecializedFormatterTraitOptions.html#method.verify_output_has_enough_bytes_left
///
/// ```
/// use iced_x86::*;
///
/// struct MyTraitOptions;
/// impl SpecializedFormatterTraitOptions for MyTraitOptions {
///     // If you never create a db/dw/dd/dq 'instruction', we don't need this feature.
///     const ENABLE_DB_DW_DD_DQ: bool = false;
///     // For a few percent faster code, you can also override `verify_output_has_enough_bytes_left()` and return `false`
///     // unsafe fn verify_output_has_enough_bytes_left() -> bool {
///     //     false
///     // }
/// }
/// type MyFormatter = SpecializedFormatter<MyTraitOptions>;
///
/// // Assume this is a big slice and not just one instruction
/// let bytes = b"\x62\xF2\x4F\xDD\x72\x50\x01";
/// let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
///
/// let mut output = String::new();
/// let mut instruction = Instruction::default();
/// let mut formatter = MyFormatter::new();
/// while decoder.can_decode() {
///     decoder.decode_out(&mut instruction);
///     output.clear();
///     formatter.format(&instruction, &mut output);
///     // do something with 'output' here, eg.:
///     //     println!("{}", output);
/// }
/// ```
///
/// Also add this to your `Cargo.toml` file:
///
/// ```toml
/// [profile.release]
/// codegen-units = 1
/// lto = true
/// opt-level = 3
/// ```
///
/// # Using a symbol resolver
///
/// The symbol resolver is disabled by default, but it's easy to enable it (or you can just use [`FastFormatter`])
///
/// ```
/// use iced_x86::*;
/// use std::collections::HashMap;
///
/// let bytes = b"\x48\x8B\x8A\xA5\x5A\xA5\x5A";
/// let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
/// let instr = decoder.decode();
///
/// struct MyTraitOptions;
/// impl SpecializedFormatterTraitOptions for MyTraitOptions {
///     const ENABLE_SYMBOL_RESOLVER: bool = true;
/// }
/// type MyFormatter = SpecializedFormatter<MyTraitOptions>;
///
/// struct MySymbolResolver { map: HashMap<u64, String> }
/// impl SymbolResolver for MySymbolResolver {
///     fn symbol(&mut self, _instruction: &Instruction, _operand: u32, _instruction_operand: Option<u32>,
///          address: u64, _address_size: u32) -> Option<SymbolResult> {
///         if let Some(symbol_string) = self.map.get(&address) {
///             // The 'address' arg is the address of the symbol and doesn't have to be identical
///             // to the 'address' arg passed to symbol(). If it's different from the input
///             // address, the formatter will add +N or -N, eg. '[rax+symbol+123]'
///             Some(SymbolResult::with_str(address, symbol_string.as_str()))
///         } else {
///             None
///         }
///     }
/// }
///
/// // Hard code the symbols, it's just an example!😄
/// let mut sym_map: HashMap<u64, String> = HashMap::new();
/// sym_map.insert(0x5AA55AA5, String::from("my_data"));
///
/// let mut output = String::new();
/// let resolver = Box::new(MySymbolResolver { map: sym_map });
/// let mut formatter = MyFormatter::try_with_options(Some(resolver)).unwrap();
/// formatter.format(&instr, &mut output);
/// assert_eq!("mov rcx,[rdx+my_data]", output);
/// ```
#[allow(missing_debug_implementations)]
pub struct SpecializedFormatter<TraitOptions: SpecializedFormatterTraitOptions> {
	d: SelfData,
	symbol_resolver: Option<Box<dyn SymbolResolver>>,
	_required_by_rustc: PhantomData<fn() -> TraitOptions>,
}

impl<TraitOptions: SpecializedFormatterTraitOptions> Default for SpecializedFormatter<TraitOptions> {
	#[must_use]
	#[inline]
	fn default() -> Self {
		SpecializedFormatter::<TraitOptions>::new()
	}
}

// Read-only data which is needed a couple of times due to borrow checker
struct SelfData {
	options: FastFormatterOptions,
	all_registers: &'static [FastStringRegister; IcedConstants::REGISTER_ENUM_COUNT],
	code_mnemonics: &'static [FastStringMnemonic; IcedConstants::CODE_ENUM_COUNT],
	code_flags: &'static [u8; IcedConstants::CODE_ENUM_COUNT],
	all_memory_sizes: &'static [FastStringMemorySize; IcedConstants::MEMORY_SIZE_ENUM_COUNT],
}

impl<TraitOptions: SpecializedFormatterTraitOptions> SpecializedFormatter<TraitOptions> {
	const SHOW_USELESS_PREFIXES: bool = true;

	/// Creates a new instance of this formatter
	#[must_use]
	#[inline]
	#[allow(clippy::unwrap_used)]
	pub fn new() -> Self {
		// This never panics
		SpecializedFormatter::<TraitOptions>::try_with_options(None).unwrap()
	}

	/// Creates a new instance of this formatter
	///
	/// # Errors
	///
	/// Fails if [`TraitOptions::ENABLE_SYMBOL_RESOLVER`] is `false` and `symbol_resolver.is_some()`
	///
	/// [`TraitOptions::ENABLE_SYMBOL_RESOLVER`]: trait.SpecializedFormatterTraitOptions.html#associatedconstant.ENABLE_SYMBOL_RESOLVER
	///
	/// # Arguments
	///
	/// - `symbol_resolver`: Symbol resolver or `None`
	#[allow(clippy::missing_inline_in_public_items)]
	pub fn try_with_options(symbol_resolver: Option<Box<dyn SymbolResolver>>) -> Result<Self, IcedError> {
		if !TraitOptions::ENABLE_SYMBOL_RESOLVER && symbol_resolver.is_some() {
			Err(IcedError::new(concat!(stringify!(TraitOptions::ENABLE_SYMBOL_RESOLVER), " is disabled so symbol resolvers aren't supported")))
		} else {
			Ok(Self {
				d: SelfData {
					options: FastFormatterOptions::new(),
					all_registers: &REGS_TBL,
					code_mnemonics: &FMT_DATA.mnemonics,
					code_flags: &FMT_DATA.flags,
					all_memory_sizes: &MEM_SIZE_TBL,
				},
				symbol_resolver,
				_required_by_rustc: PhantomData,
			})
		}
	}

	/// Gets the formatter options (immutable)
	///
	/// Note that the `TraitOptions` generic parameter can override any option and hard code them,
	/// see [`SpecializedFormatterTraitOptions`]
	///
	/// [`SpecializedFormatterTraitOptions`]: trait.SpecializedFormatterTraitOptions.html
	#[must_use]
	#[inline]
	pub fn options(&self) -> &FastFormatterOptions {
		&self.d.options
	}

	/// Gets the formatter options (mutable)
	///
	/// Note that the `TraitOptions` generic parameter can override any option and hard code them,
	/// see [`SpecializedFormatterTraitOptions`]
	///
	/// [`SpecializedFormatterTraitOptions`]: trait.SpecializedFormatterTraitOptions.html
	#[must_use]
	#[inline]
	pub fn options_mut(&mut self) -> &mut FastFormatterOptions {
		&mut self.d.options
	}

	/// Formats the whole instruction: prefixes, mnemonic, operands
	///
	/// # Arguments
	///
	/// - `instruction`: Instruction
	/// - `output`: Output
	#[allow(clippy::missing_inline_in_public_items)]
	#[allow(clippy::let_unit_value)]
	#[allow(clippy::useless_let_if_seq)]
	pub fn format(&mut self, instruction: &Instruction, output: &mut String) {
		// SAFETY: We only append data that come from a `&str`, a `String` or ASCII chars so the data is always valid utf8
		let dst = unsafe { output.as_mut_vec() };
		// The code assumes there's enough bytes (or it will panic) so reserve enough bytes here
		dst.reserve(MAX_FMT_INSTR_LEN);
		// SAFETY:
		// - ptr is in bounds (after last valid byte)
		// - it's reloaded when using 'dst' to write to the vector
		let mut dst_next_p = unsafe { dst.as_mut_ptr().add(dst.len()) };

		let code = instruction.code();
		let mut mnemonic = self.d.code_mnemonics[code as usize];
		let mut op_count = instruction.op_count();
		if TraitOptions::use_pseudo_ops(&self.d.options) {
			let flags = self.d.code_flags[code as usize];
			let pseudo_ops_num = flags >> FastFmtFlags::PSEUDO_OPS_KIND_SHIFT;
			if pseudo_ops_num != 0 && instruction.op_kind(op_count - 1) == OpKind::Immediate8 {
				let mut index = instruction.immediate8() as usize;
				// SAFETY: the generator generates only valid values (1-based)
				let pseudo_ops_kind: PseudoOpsKind = unsafe { mem::transmute(pseudo_ops_num - 1) };
				// Not enough bits to store all values so some are mapped to the same value. Fix that here.
				let pseudo_ops_kind = if pseudo_ops_kind == PseudoOpsKind::vpcmpd6 {
					match code {
						#[cfg(feature = "mvex")]
						Code::MVEX_Vpcmpud_kr_k1_zmm_zmmmt_imm8 => PseudoOpsKind::vpcmpud6,
						_ => pseudo_ops_kind,
					}
				} else {
					pseudo_ops_kind
				};
				let pseudo_ops = get_pseudo_ops(pseudo_ops_kind);
				if pseudo_ops_kind == PseudoOpsKind::pclmulqdq || pseudo_ops_kind == PseudoOpsKind::vpclmulqdq {
					if index <= 1 {
						// nothing
					} else if index == 0x10 {
						index = 2;
					} else if index == 0x11 {
						index = 3;
					} else {
						index = usize::MAX;
					}
				}
				if let Some(&pseudo_op_mnemonic) = pseudo_ops.get(index) {
					mnemonic = pseudo_op_mnemonic;
					op_count -= 1;
				}
			}
		}

		let prefix_seg = instruction_internal::internal_segment_prefix_raw(instruction);
		const _: () = assert!(Register::None as u32 == 0);
		if prefix_seg < 6 || instruction_internal::internal_has_any_of_lock_rep_repne_prefix(instruction) != 0 {
			const DS_REG: u32 = Register::DS as u32 - Register::ES as u32;
			let has_notrack_prefix = prefix_seg == DS_REG && is_notrack_prefix_branch(code);
			if !has_notrack_prefix && prefix_seg < 6 && SpecializedFormatter::<TraitOptions>::show_segment_prefix(instruction, op_count) {
				let prefix_seg = unsafe { mem::transmute((Register::ES as u32 + prefix_seg) as RegisterUnderlyingType) };
				call_format_register!(self, dst, dst_next_p, prefix_seg);
				write_fast_ascii_char_lit!(dst, dst_next_p, b' ', true);
			}

			let mut has_xacquire_xrelease = false;
			if instruction.has_xacquire_prefix() {
				const FAST_STR: FastString12 = mk_const_fast_str!(FastString12, "xacquire ");
				write_fast_str!(dst, dst_next_p, FastString12, FAST_STR);
				has_xacquire_xrelease = true;
			}
			if instruction.has_xrelease_prefix() {
				const FAST_STR: FastString12 = mk_const_fast_str!(FastString12, "xrelease ");
				write_fast_str!(dst, dst_next_p, FastString12, FAST_STR);
				has_xacquire_xrelease = true;
			}
			if instruction.has_lock_prefix() {
				const FAST_STR: FastString8 = mk_const_fast_str!(FastString8, "lock ");
				write_fast_str!(dst, dst_next_p, FastString8, FAST_STR);
			}
			if has_notrack_prefix {
				const FAST_STR: FastString8 = mk_const_fast_str!(FastString8, "notrack ");
				write_fast_str!(dst, dst_next_p, FastString8, FAST_STR);
			}
			if !has_xacquire_xrelease {
				if instruction.has_repe_prefix()
					&& (SpecializedFormatter::<TraitOptions>::SHOW_USELESS_PREFIXES
						|| show_rep_or_repe_prefix_bool(code, SpecializedFormatter::<TraitOptions>::SHOW_USELESS_PREFIXES))
				{
					if is_repe_or_repne_instruction(code) {
						const FAST_STR: FastString8 = mk_const_fast_str!(FastString8, "repe ");
						write_fast_str!(dst, dst_next_p, FastString8, FAST_STR);
					} else {
						const FAST_STR: FastString4 = mk_const_fast_str!(FastString4, "rep ");
						write_fast_str!(dst, dst_next_p, FastString4, FAST_STR);
					}
				}
				if instruction.has_repne_prefix() {
					if (Code::Retnw_imm16 <= code && code <= Code::Retnq)
						|| (Code::Call_rel16 <= code && code <= Code::Jmp_rel32_64)
						|| (Code::Call_rm16 <= code && code <= Code::Call_rm64)
						|| (Code::Jmp_rm16 <= code && code <= Code::Jmp_rm64)
						|| code.is_jcc_short_or_near()
					{
						const FAST_STR: FastString4 = mk_const_fast_str!(FastString4, "bnd ");
						write_fast_str!(dst, dst_next_p, FastString4, FAST_STR);
					} else if SpecializedFormatter::<TraitOptions>::SHOW_USELESS_PREFIXES
						|| show_repne_prefix_bool(code, SpecializedFormatter::<TraitOptions>::SHOW_USELESS_PREFIXES)
					{
						const FAST_STR: FastString8 = mk_const_fast_str!(FastString8, "repne ");
						write_fast_str!(dst, dst_next_p, FastString8, FAST_STR);
					}
				}
			}
		}

		write_fast_str!(dst, dst_next_p, FastStringMnemonic, mnemonic);

		let is_declare_data;
		let declare_data_kind = if !TraitOptions::ENABLE_DB_DW_DD_DQ {
			is_declare_data = false;
			OpKind::Register
		} else if (code as u32).wrapping_sub(Code::DeclareByte as u32) <= (Code::DeclareQword as u32 - Code::DeclareByte as u32) {
			op_count = instruction.declare_data_len() as u32;
			is_declare_data = true;
			match code {
				Code::DeclareByte => OpKind::Immediate8,
				Code::DeclareWord => OpKind::Immediate16,
				Code::DeclareDword => OpKind::Immediate32,
				_ => {
					debug_assert_eq!(code, Code::DeclareQword);
					OpKind::Immediate64
				}
			}
		} else {
			is_declare_data = false;
			OpKind::Register
		};

		if op_count > 0 {
			write_fast_ascii_char_lit!(dst, dst_next_p, b' ', true);

			#[cfg(feature = "mvex")]
			let mvex_rm_operand = {
				if IcedConstants::is_mvex(instruction.code()) {
					debug_assert_ne!(op_count, 0);
					if instruction.op_kind(op_count.wrapping_sub(1)) == OpKind::Immediate8 {
						op_count.wrapping_sub(2)
					} else {
						op_count.wrapping_sub(1)
					}
				} else {
					u32::MAX
				}
			};

			let mut operand = 0;
			loop {
				let imm8;
				let imm16;
				let imm32;
				let imm64;
				let imm_size;
				let op_kind = if TraitOptions::ENABLE_DB_DW_DD_DQ && is_declare_data { declare_data_kind } else { instruction.op_kind(operand) };

				// Share as much code as possible so put these in macros
				macro_rules! fmt_near_branch {
					($slf:ident, $dst:ident, $dst_next_p:ident, $instruction:ident, $imm_size:expr, $imm:ident) => {{
						if TraitOptions::ENABLE_SYMBOL_RESOLVER {
							// PERF: Symbols should be rare when using fast fmt with a symbol resolver so clone
							// the symbol (forced by borrowck).
							// This results in slightly faster code when we do NOT support a symbol resolver since
							// we don't need to pass in the options to various methods and can instead pass in &Self
							// (i.e., use a method instead of a func).
							let mut vec: Vec<SymResTextPart<'_>> = Vec::new();
							if let Some(ref symbol) = if let Some(ref mut symbol_resolver) = $slf.symbol_resolver {
								to_owned(symbol_resolver.symbol($instruction, operand, Some(operand), $imm, $imm_size), &mut vec)
							} else {
								None
							} {
								call_write_symbol!($slf, $dst, $dst_next_p, $imm, symbol);
							} else {
								call_format_number!($slf, $dst, $dst_next_p, $imm);
							}
						} else {
							call_format_number!($slf, $dst, $dst_next_p, $imm);
						}
					}};
				}
				macro_rules! fmt_far_branch {
					($slf:ident, $dst:ident, $dst_next_p:ident, $instruction:ident, $op_kind:ident, $imm_size_ident:ident, $imm64:ident) => {{
						if $op_kind == OpKind::FarBranch32 {
							$imm_size_ident = 4;
							$imm64 = $instruction.far_branch32() as u64;
						} else {
							$imm_size_ident = 2;
							$imm64 = $instruction.far_branch16() as u64;
						}
						if TraitOptions::ENABLE_SYMBOL_RESOLVER {
							// See fmt_near_branch!() above for why we clone the symbols
							let mut vec: Vec<SymResTextPart<'_>> = Vec::new();
							let mut vec2: Vec<SymResTextPart<'_>> = Vec::new();
							if let Some(ref symbol) = if let Some(ref mut symbol_resolver) = $slf.symbol_resolver {
								to_owned(
									symbol_resolver.symbol($instruction, operand, Some(operand), $imm64 as u32 as u64, $imm_size_ident),
									&mut vec,
								)
							} else {
								None
							} {
								debug_assert!(operand + 1 == 1);
								let selector_symbol = if let Some(ref mut symbol_resolver) = $slf.symbol_resolver {
									to_owned(
										symbol_resolver.symbol(
											$instruction,
											operand + 1,
											Some(operand),
											$instruction.far_branch_selector() as u64,
											2,
										),
										&mut vec2,
									)
								} else {
									None
								};
								if let Some(ref selector_symbol) = selector_symbol {
									call_write_symbol!($slf, $dst, $dst_next_p, $instruction.far_branch_selector() as u64, selector_symbol);
								} else {
									call_format_number!($slf, $dst, $dst_next_p, $instruction.far_branch_selector() as u64);
								}
								write_fast_ascii_char_lit!(dst, dst_next_p, b':', true);
								call_write_symbol!($slf, $dst, $dst_next_p, $imm64, symbol);
							} else {
								call_format_number!($slf, $dst, $dst_next_p, $instruction.far_branch_selector() as u64);
								write_fast_ascii_char_lit!(dst, dst_next_p, b':', true);
								call_format_number!($slf, $dst, $dst_next_p, $imm64);
							}
						} else {
							call_format_number!($slf, $dst, $dst_next_p, $instruction.far_branch_selector() as u64);
							write_fast_ascii_char_lit!(dst, dst_next_p, b':', true);
							call_format_number!($slf, $dst, $dst_next_p, $imm64);
						}
					}};
				}
				macro_rules! fmt_imm {
					($slf:ident, $dst:ident, $dst_next_p:ident, $instruction:ident, $imm:ident, $imm_size:literal) => {
						#[allow(trivial_numeric_casts)]
						{
							if TraitOptions::ENABLE_SYMBOL_RESOLVER {
								// See fmt_near_branch!() above for why we clone the symbols
								let mut vec: Vec<SymResTextPart<'_>> = Vec::new();
								if let Some(ref symbol) = if let Some(ref mut symbol_resolver) = $slf.symbol_resolver {
									to_owned(symbol_resolver.symbol($instruction, operand, Some(operand), $imm as u64, $imm_size), &mut vec)
								} else {
									None
								} {
									if (symbol.flags & SymbolFlags::RELATIVE) == 0 {
										const FAST_STR: FastString8 = mk_const_fast_str!(FastString8, "offset ");
										write_fast_str!($dst, $dst_next_p, FastString8, FAST_STR);
									}
									call_write_symbol!($slf, $dst, $dst_next_p, $imm as u64, symbol);
								} else {
									call_format_number!($slf, $dst, $dst_next_p, $imm as u64);
								}
							} else {
								call_format_number!($slf, $dst, $dst_next_p, $imm as u64);
							}
						}
					};
				}
				macro_rules! fmt_register {
					() => {{
						call_format_register!(self, dst, dst_next_p, instruction_internal::internal_op_register(instruction, operand))
					}};
				}
				macro_rules! fmt_far_br_16_32 {
					() => {{
						fmt_far_branch!(self, dst, dst_next_p, instruction, op_kind, imm_size, imm64)
					}};
				}
				macro_rules! fmt_memory_seg_si {
					() => {{
						call_format_memory!(
							self,
							dst,
							dst_next_p,
							instruction,
							operand,
							(instruction.memory_segment()),
							(Register::SI),
							(Register::None),
							0,
							0,
							0,
							2,
						)
					}};
				}
				macro_rules! fmt_memory_seg_esi {
					() => {{
						call_format_memory!(
							self,
							dst,
							dst_next_p,
							instruction,
							operand,
							(instruction.memory_segment()),
							(Register::ESI),
							(Register::None),
							0,
							0,
							0,
							4,
						)
					}};
				}
				macro_rules! fmt_memory_seg_rsi {
					() => {{
						call_format_memory!(
							self,
							dst,
							dst_next_p,
							instruction,
							operand,
							(instruction.memory_segment()),
							(Register::RSI),
							(Register::None),
							0,
							0,
							0,
							8,
						)
					}};
				}
				macro_rules! fmt_memory_seg_di {
					() => {{
						call_format_memory!(
							self,
							dst,
							dst_next_p,
							instruction,
							operand,
							(instruction.memory_segment()),
							(Register::DI),
							(Register::None),
							0,
							0,
							0,
							2,
						)
					}};
				}
				macro_rules! fmt_memory_seg_edi {
					() => {{
						call_format_memory!(
							self,
							dst,
							dst_next_p,
							instruction,
							operand,
							(instruction.memory_segment()),
							(Register::EDI),
							(Register::None),
							0,
							0,
							0,
							4,
						)
					}};
				}
				macro_rules! fmt_memory_seg_rdi {
					() => {{
						call_format_memory!(
							self,
							dst,
							dst_next_p,
							instruction,
							operand,
							(instruction.memory_segment()),
							(Register::RDI),
							(Register::None),
							0,
							0,
							0,
							8,
						)
					}};
				}
				macro_rules! fmt_memory_es_di {
					() => {{
						call_format_memory!(self, dst, dst_next_p, instruction, operand, (Register::ES), (Register::DI), (Register::None), 0, 0, 0, 2)
					}};
				}
				macro_rules! fmt_memory_es_edi {
					() => {{
						call_format_memory!(
							self,
							dst,
							dst_next_p,
							instruction,
							operand,
							(Register::ES),
							(Register::EDI),
							(Register::None),
							0,
							0,
							0,
							4
						)
					}};
				}
				macro_rules! fmt_memory_es_rdi {
					() => {{
						call_format_memory!(
							self,
							dst,
							dst_next_p,
							instruction,
							operand,
							(Register::ES),
							(Register::RDI),
							(Register::None),
							0,
							0,
							0,
							8
						)
					}};
				}
				macro_rules! fmt_memory {
					() => {{
						let displ_size = instruction.memory_displ_size();
						let base_reg = instruction.memory_base();
						let mut index_reg = instruction.memory_index();
						let addr_size = get_address_size_in_bytes(base_reg, index_reg, displ_size, instruction.code_size());
						let displ =
							if addr_size == 8 { instruction.memory_displacement64() as i64 } else { instruction.memory_displacement32() as i64 };
						if code == Code::Xlat_m8 {
							index_reg = Register::None;
						}
						call_format_memory!(
							self,
							dst,
							dst_next_p,
							instruction,
							operand,
							(instruction.memory_segment()),
							base_reg,
							index_reg,
							(instruction_internal::internal_get_memory_index_scale(instruction)),
							displ_size,
							displ,
							addr_size,
						);
						#[cfg(feature = "mvex")]
						if instruction.is_mvex_eviction_hint() {
							const FAST_STR: FastString4 = mk_const_fast_str!(FastString4, "{eh}");
							write_fast_str!(dst, dst_next_p, FastString4, FAST_STR);
						}
					}};
				}

				// This speeds up SpecializedFormatter since every option is hard coded, but makes FastFormatter
				// slower because every option is dynamic (more code). Detect FastFormatter and generate smaller
				// and faster code. Both paths are tested (same tests).
				// The whole point of this formatter is to be fast so unfortunately it can result in fugly code...
				if TraitOptions::__IS_FAST_FORMATTER {
					match op_kind {
						OpKind::Register => fmt_register!(),

						OpKind::NearBranch16 | OpKind::NearBranch32 | OpKind::NearBranch64 => {
							if op_kind == OpKind::NearBranch64 {
								imm_size = 8;
								imm64 = instruction.near_branch64();
							} else if op_kind == OpKind::NearBranch32 {
								imm_size = 4;
								imm64 = instruction.near_branch32() as u64;
							} else {
								imm_size = 2;
								imm64 = instruction.near_branch16() as u64;
							}
							fmt_near_branch!(self, dst, dst_next_p, instruction, imm_size, imm64);
						}

						OpKind::FarBranch16 | OpKind::FarBranch32 => fmt_far_br_16_32!(),

						OpKind::Immediate8 | OpKind::Immediate8_2nd => {
							if TraitOptions::ENABLE_DB_DW_DD_DQ && is_declare_data {
								imm8 = instruction.get_declare_byte_value(operand as usize);
							} else if op_kind == OpKind::Immediate8 {
								imm8 = instruction.immediate8();
							} else {
								debug_assert_eq!(op_kind, OpKind::Immediate8_2nd);
								imm8 = instruction.immediate8_2nd();
							}
							fmt_imm!(self, dst, dst_next_p, instruction, imm8, 1);
						}

						OpKind::Immediate16 | OpKind::Immediate8to16 => {
							if TraitOptions::ENABLE_DB_DW_DD_DQ && is_declare_data {
								imm16 = instruction.get_declare_word_value(operand as usize);
							} else if op_kind == OpKind::Immediate16 {
								imm16 = instruction.immediate16();
							} else {
								debug_assert_eq!(op_kind, OpKind::Immediate8to16);
								imm16 = instruction.immediate8to16() as u16;
							}
							fmt_imm!(self, dst, dst_next_p, instruction, imm16, 2)
						}

						OpKind::Immediate32 | OpKind::Immediate8to32 => {
							if TraitOptions::ENABLE_DB_DW_DD_DQ && is_declare_data {
								imm32 = instruction.get_declare_dword_value(operand as usize);
							} else if op_kind == OpKind::Immediate32 {
								imm32 = instruction.immediate32();
							} else {
								debug_assert_eq!(op_kind, OpKind::Immediate8to32);
								imm32 = instruction.immediate8to32() as u32;
							}
							fmt_imm!(self, dst, dst_next_p, instruction, imm32, 4)
						}

						OpKind::Immediate64 | OpKind::Immediate8to64 | OpKind::Immediate32to64 => {
							if TraitOptions::ENABLE_DB_DW_DD_DQ && is_declare_data {
								imm64 = instruction.get_declare_qword_value(operand as usize);
							} else if op_kind == OpKind::Immediate32to64 {
								imm64 = instruction.immediate32to64() as u64;
							} else if op_kind == OpKind::Immediate8to64 {
								imm64 = instruction.immediate8to64() as u64;
							} else {
								debug_assert_eq!(op_kind, OpKind::Immediate64);
								imm64 = instruction.immediate64();
							}
							fmt_imm!(self, dst, dst_next_p, instruction, imm64, 8)
						}

						OpKind::MemorySegSI => fmt_memory_seg_si!(),
						OpKind::MemorySegESI => fmt_memory_seg_esi!(),
						OpKind::MemorySegRSI => fmt_memory_seg_rsi!(),
						OpKind::MemorySegDI => fmt_memory_seg_di!(),
						OpKind::MemorySegEDI => fmt_memory_seg_edi!(),
						OpKind::MemorySegRDI => fmt_memory_seg_rdi!(),
						OpKind::MemoryESDI => fmt_memory_es_di!(),
						OpKind::MemoryESEDI => fmt_memory_es_edi!(),
						OpKind::MemoryESRDI => fmt_memory_es_rdi!(),
						OpKind::Memory => fmt_memory!(),
					}
				} else {
					match op_kind {
						OpKind::Register => fmt_register!(),

						OpKind::NearBranch16 => {
							imm64 = instruction.near_branch16() as u64;
							fmt_near_branch!(self, dst, dst_next_p, instruction, 2, imm64);
						}

						OpKind::NearBranch32 => {
							imm64 = instruction.near_branch32() as u64;
							fmt_near_branch!(self, dst, dst_next_p, instruction, 4, imm64);
						}

						OpKind::NearBranch64 => {
							imm64 = instruction.near_branch64();
							fmt_near_branch!(self, dst, dst_next_p, instruction, 8, imm64);
						}

						OpKind::FarBranch16 | OpKind::FarBranch32 => fmt_far_br_16_32!(),

						OpKind::Immediate8 => {
							imm8 = if TraitOptions::ENABLE_DB_DW_DD_DQ && is_declare_data {
								instruction.get_declare_byte_value(operand as usize)
							} else {
								instruction.immediate8()
							};
							fmt_imm!(self, dst, dst_next_p, instruction, imm8, 1);
						}

						OpKind::Immediate8_2nd => {
							imm8 = instruction.immediate8_2nd();
							fmt_imm!(self, dst, dst_next_p, instruction, imm8, 1);
						}

						OpKind::Immediate16 => {
							imm16 = if TraitOptions::ENABLE_DB_DW_DD_DQ && is_declare_data {
								instruction.get_declare_word_value(operand as usize)
							} else {
								instruction.immediate16()
							};
							fmt_imm!(self, dst, dst_next_p, instruction, imm16, 2)
						}

						OpKind::Immediate8to16 => {
							imm16 = instruction.immediate8to16() as u16;
							fmt_imm!(self, dst, dst_next_p, instruction, imm16, 2)
						}

						OpKind::Immediate32 => {
							imm32 = if TraitOptions::ENABLE_DB_DW_DD_DQ && is_declare_data {
								instruction.get_declare_dword_value(operand as usize)
							} else {
								instruction.immediate32()
							};
							fmt_imm!(self, dst, dst_next_p, instruction, imm32, 4)
						}

						OpKind::Immediate8to32 => {
							imm32 = instruction.immediate8to32() as u32;
							fmt_imm!(self, dst, dst_next_p, instruction, imm32, 4)
						}

						OpKind::Immediate64 => {
							imm64 = if TraitOptions::ENABLE_DB_DW_DD_DQ && is_declare_data {
								instruction.get_declare_qword_value(operand as usize)
							} else {
								instruction.immediate64()
							};
							fmt_imm!(self, dst, dst_next_p, instruction, imm64, 8)
						}

						OpKind::Immediate8to64 => {
							imm64 = instruction.immediate8to64() as u64;
							fmt_imm!(self, dst, dst_next_p, instruction, imm64, 8)
						}

						OpKind::Immediate32to64 => {
							imm64 = instruction.immediate32to64() as u64;
							fmt_imm!(self, dst, dst_next_p, instruction, imm64, 8)
						}

						OpKind::MemorySegSI => fmt_memory_seg_si!(),
						OpKind::MemorySegESI => fmt_memory_seg_esi!(),
						OpKind::MemorySegRSI => fmt_memory_seg_rsi!(),
						OpKind::MemorySegDI => fmt_memory_seg_di!(),
						OpKind::MemorySegEDI => fmt_memory_seg_edi!(),
						OpKind::MemorySegRDI => fmt_memory_seg_rdi!(),
						OpKind::MemoryESDI => fmt_memory_es_di!(),
						OpKind::MemoryESEDI => fmt_memory_es_edi!(),
						OpKind::MemoryESRDI => fmt_memory_es_rdi!(),
						OpKind::Memory => fmt_memory!(),
					}
				}

				if operand == 0 && instruction_internal::internal_has_op_mask_or_zeroing_masking(instruction) {
					if instruction.has_op_mask() {
						write_fast_ascii_char_lit!(dst, dst_next_p, b'{', true);
						call_format_register!(self, dst, dst_next_p, instruction.op_mask());
						write_fast_ascii_char_lit!(dst, dst_next_p, b'}', true);
					}
					if instruction.zeroing_masking() {
						const FAST_STR: FastString4 = mk_const_fast_str!(FastString4, "{z}");
						write_fast_str!(dst, dst_next_p, FastString4, FAST_STR);
					}
				}
				#[cfg(feature = "mvex")]
				if mvex_rm_operand == operand {
					let conv = instruction.mvex_reg_mem_conv();
					if conv != MvexRegMemConv::None {
						let mvex = crate::mvex::get_mvex_info(instruction.code());
						if mvex.conv_fn != MvexConvFn::None {
							let tbl = if mvex.is_conv_fn_32() { &MVEX_REG_MEM_CONSTS_32 } else { &MVEX_REG_MEM_CONSTS_64 };
							let s = tbl[conv as usize];
							write_fast_str!(dst, dst_next_p, FastString12, s);
						}
					}
				}

				operand += 1;
				if operand >= op_count {
					break;
				}

				if TraitOptions::space_after_operand_separator(&self.d.options) {
					const FAST_STR: FastString4 = mk_const_fast_str!(FastString4, ", ");
					write_fast_str!(dst, dst_next_p, FastString4, FAST_STR);
				} else {
					write_fast_ascii_char_lit!(dst, dst_next_p, b',', true);
				}
			}
			if instruction_internal::internal_has_rounding_control_or_sae(instruction) {
				let rc = instruction.rounding_control();
				if rc != RoundingControl::None {
					if IcedConstants::is_mvex(instruction.code()) && !instruction.suppress_all_exceptions() {
						let fast_str = RC_STRINGS[rc as usize];
						write_fast_str!(dst, dst_next_p, FastString4, fast_str);
					} else {
						let fast_str = RC_SAE_STRINGS[rc as usize];
						write_fast_str!(dst, dst_next_p, FastString8, fast_str);
					}
				} else {
					debug_assert!(instruction.suppress_all_exceptions());
					const FAST_STR: FastString8 = mk_const_fast_str!(FastString8, "{sae}");
					write_fast_str!(dst, dst_next_p, FastString8, FAST_STR);
				}
			}
		}

		update_vec_len!(dst, dst_next_p);
	}

	// Only one caller so inline it
	#[must_use]
	#[inline]
	fn show_segment_prefix(instruction: &Instruction, op_count: u32) -> bool {
		for i in 0..op_count {
			match instruction.op_kind(i) {
				OpKind::Register
				| OpKind::NearBranch16
				| OpKind::NearBranch32
				| OpKind::NearBranch64
				| OpKind::FarBranch16
				| OpKind::FarBranch32
				| OpKind::Immediate8
				| OpKind::Immediate8_2nd
				| OpKind::Immediate16
				| OpKind::Immediate32
				| OpKind::Immediate64
				| OpKind::Immediate8to16
				| OpKind::Immediate8to32
				| OpKind::Immediate8to64
				| OpKind::Immediate32to64
				| OpKind::MemoryESDI
				| OpKind::MemoryESEDI
				| OpKind::MemoryESRDI => {}

				OpKind::MemorySegSI
				| OpKind::MemorySegESI
				| OpKind::MemorySegRSI
				| OpKind::MemorySegDI
				| OpKind::MemorySegEDI
				| OpKind::MemorySegRDI
				| OpKind::Memory => return false,
			}
		}

		SpecializedFormatter::<TraitOptions>::SHOW_USELESS_PREFIXES
	}

	#[inline]
	#[must_use]
	fn format_register(&self, dst: &mut Vec<u8>, mut dst_next_p: *mut u8, register: Register) -> *mut u8 {
		let reg_str = self.d.all_registers[register as usize];
		write_fast_str!(dst, dst_next_p, FastStringRegister, reg_str);
		dst_next_p
	}

	#[must_use]
	fn format_number(&self, dst: &mut Vec<u8>, mut dst_next_p: *mut u8, value: u64) -> *mut u8 {
		macro_rules! format_number_impl {
			($dst:ident, $dst_next_p:ident, $value:ident, $uppercase_hex:literal, $use_hex_prefix:literal) => {{
				if $use_hex_prefix {
					const FAST_STR: FastString4 = mk_const_fast_str!(FastString4, "0x");
					write_fast_str!($dst, $dst_next_p, FastString4, FAST_STR);
				}

				if $value < 0x10 {
					if $use_hex_prefix {
						let hex_table = if $uppercase_hex { b"0123456789ABCDEF" } else { b"0123456789abcdef" };
						// SAFETY: 0<=$value<=0xF and hex_table.len() == 0x10
						let c = unsafe { *hex_table.get_unchecked($value as usize) };
						write_fast_ascii_char!($dst, $dst_next_p, c, true);

						$dst_next_p
					} else {
						// 1 (possible '0' prefix) + 1 (hex digit) + 1 ('h' suffix)
						verify_output_has_enough_bytes_left!($dst, $dst_next_p, 1 + 1 + 1);
						if $value > 9 {
							write_fast_ascii_char_lit!($dst, $dst_next_p, b'0', false);
						}

						let hex_table = if $uppercase_hex { b"0123456789ABCDEF" } else { b"0123456789abcdef" };
						// SAFETY: 0<=$value<=0xF and hex_table.len() == 0x10
						let c = unsafe { *hex_table.get_unchecked($value as usize) };
						write_fast_ascii_char!($dst, $dst_next_p, c, false);
						write_fast_ascii_char_lit!($dst, $dst_next_p, b'h', false);

						$dst_next_p
					}
				} else if $value < 0x100 {
					if $use_hex_prefix {
						let lower_or_value = if $uppercase_hex { 0 } else { 0x2020_2020 };
						write_fast_hex2_rw_4bytes!($dst, $dst_next_p, $value, lower_or_value, true);

						$dst_next_p
					} else {
						// 1 (possible '0' prefix) + 2 (hex digits) + 2 since
						// write_fast_hex2_rw_4bytes!() reads/writes 4 bytes and not 2.
						// '+2' also includes the 'h' suffix.
						verify_output_has_enough_bytes_left!($dst, $dst_next_p, 1 + 2 + 2);
						if $value > 0x9F {
							write_fast_ascii_char_lit!($dst, $dst_next_p, b'0', false);
						}

						let lower_or_value = if $uppercase_hex { 0 } else { 0x2020_2020 };
						write_fast_hex2_rw_4bytes!($dst, $dst_next_p, $value, lower_or_value, false);
						write_fast_ascii_char_lit!($dst, $dst_next_p, b'h', false);

						$dst_next_p
					}
				} else {
					let mut rshift = ((64 - u64::leading_zeros($value) + 3) & !3) as usize;

					// The first '1' is an optional '0' prefix.
					// `rshift / 4` == number of hex digits to copy. The last `+ 2` is the extra padding needed
					// since the write_fast_hex2_rw_4bytes!() macro reads and writes 4 bytes (2 hex digits + 2 bytes padding).
					// '+2' also includes the 'h' suffix.
					verify_output_has_enough_bytes_left!($dst, $dst_next_p, 1 + rshift / 4 + 2);

					if !$use_hex_prefix && (($value >> (rshift - 4)) & 0xF) > 9 {
						write_fast_ascii_char_lit!($dst, $dst_next_p, b'0', false);
					}

					// If odd number of hex digits
					if (rshift & 4) != 0 {
						rshift -= 4;
						let hex_table = if $uppercase_hex { b"0123456789ABCDEF" } else { b"0123456789abcdef" };
						let digit = (($value >> rshift) & 0xF) as usize;
						// SAFETY: 0<=digit<=0xF and hex_table.len() == 0x10
						let c = unsafe { *hex_table.get_unchecked(digit) };
						write_fast_ascii_char!($dst, $dst_next_p, c, false);
					}

					// If we're here, $value >= 0x100 so rshift >= 8
					debug_assert!(rshift >= 8);
					let lower_or_value = if $uppercase_hex { 0 } else { 0x2020_2020 };
					loop {
						rshift -= 8;
						let digits2 = (($value >> rshift) & 0xFF) as usize;
						write_fast_hex2_rw_4bytes!($dst, $dst_next_p, digits2, lower_or_value, false);

						if rshift == 0 {
							break;
						}
					}

					if !$use_hex_prefix {
						// We've verified that `dst` had `1 + rshift / 4 + 2` bytes left (see above).
						// The last `+2` is the padding that needed to be there. That's where
						// this 'h' gets written so we don't need to verify the vec len here
						// because it has at least 2 more bytes left.
						write_fast_ascii_char_lit!($dst, $dst_next_p, b'h', false);
					}

					$dst_next_p
				}
			}};
		}

		if TraitOptions::uppercase_hex(&self.d.options) {
			if TraitOptions::use_hex_prefix(&self.d.options) {
				// 0x12AB
				format_number_impl!(dst, dst_next_p, value, true, true)
			} else {
				// 12ABh
				format_number_impl!(dst, dst_next_p, value, true, false)
			}
		} else {
			if TraitOptions::use_hex_prefix(&self.d.options) {
				// 0x12ab
				format_number_impl!(dst, dst_next_p, value, false, true)
			} else {
				// 12abh
				format_number_impl!(dst, dst_next_p, value, false, false)
			}
		}
	}

	#[inline]
	#[must_use]
	fn write_symbol(&self, dst: &mut Vec<u8>, mut dst_next_p: *mut u8, address: u64, symbol: &SymbolResult<'_>) -> *mut u8 {
		call_write_symbol2!(self, dst, dst_next_p, address, symbol, true);
		dst_next_p
	}

	#[cold]
	#[must_use]
	fn write_symbol2(
		&self, dst: &mut Vec<u8>, mut dst_next_p: *mut u8, address: u64, symbol: &SymbolResult<'_>, write_minus_if_signed: bool,
	) -> *mut u8 {
		let mut displ = address.wrapping_sub(symbol.address) as i64;
		if (symbol.flags & SymbolFlags::SIGNED) != 0 {
			if write_minus_if_signed {
				write_fast_ascii_char_lit!(dst, dst_next_p, b'-', true);
			}
			displ = displ.wrapping_neg();
		}

		// Write the symbol. The symbol can be any length and is a `&'a str` so we must
		// write using `dst`. The macro will invalidate `dst_next_p` and will restore
		// it after the match statement.
		use_dst_only_now!(dst, dst_next_p);
		match symbol.text {
			SymResTextInfo::Text(ref part) => {
				let s = match &part.text {
					&SymResString::Str(s) => s,
					SymResString::String(s) => s.as_str(),
				};
				dst.extend_from_slice(s.as_bytes());
			}

			SymResTextInfo::TextVec(v) => {
				for part in v {
					let s = match &part.text {
						&SymResString::Str(s) => s,
						SymResString::String(s) => s.as_str(),
					};
					dst.extend_from_slice(s.as_bytes());
				}
			}
		}
		use_dst_next_p_now!(dst, dst_next_p);

		if displ != 0 {
			let c = if displ < 0 {
				displ = displ.wrapping_neg();
				b'-'
			} else {
				b'+'
			};
			write_fast_ascii_char!(dst, dst_next_p, c, true);
			call_format_number!(self, dst, dst_next_p, displ as u64);
		}
		if TraitOptions::show_symbol_address(&self.d.options) {
			const FAST_STR: FastString4 = mk_const_fast_str!(FastString4, " (");
			write_fast_str!(dst, dst_next_p, FastString4, FAST_STR);
			call_format_number!(self, dst, dst_next_p, address);
			write_fast_ascii_char_lit!(dst, dst_next_p, b')', true);
		}

		dst_next_p
	}

	#[must_use]
	fn format_memory(
		&mut self, dst: &mut Vec<u8>, mut dst_next_p: *mut u8, instruction: &Instruction, operand: u32, seg_reg: Register, base_reg: Register,
		index_reg: Register, scale: u32, displ_size: u32, displ: i64, addr_size: u32,
	) -> *mut u8 {
		format_memory_code!(self, dst, dst_next_p, instruction, operand, seg_reg, base_reg, index_reg, scale, displ_size, displ, addr_size);
		dst_next_p
	}
}

/// Fast formatter with less formatting options and with a masm-like syntax.
/// Use it if formatting speed is more important than being able to re-assemble formatted instructions.
///
/// This is a variant of [`SpecializedFormatter<TraitOptions>`] and allows changing the
/// formatter options at runtime and the use of a symbol resolver. For fastest possible
/// disassembly and smallest code, the options should be hard coded, so see [`SpecializedFormatter<TraitOptions>`].
///
/// This formatter is ~2.8x faster than the gas/intel/masm/nasm formatters (the time includes decoding + formatting).
///
/// [`SpecializedFormatter<TraitOptions>`]: struct.SpecializedFormatter.html
///
/// # Examples
///
/// ```
/// use iced_x86::*;
///
/// let bytes = b"\x62\xF2\x4F\xDD\x72\x50\x01";
/// let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
/// let instr = decoder.decode();
///
/// let mut output = String::new();
/// let mut formatter = FastFormatter::new();
/// formatter.options_mut().set_space_after_operand_separator(true);
/// formatter.format(&instr, &mut output);
/// assert_eq!(output, "vcvtne2ps2bf16 zmm2{k5}{z}, zmm6, dword bcst [rax+4h]");
/// ```
///
/// # Using a symbol resolver
///
/// ```
/// use iced_x86::*;
/// use std::collections::HashMap;
///
/// let bytes = b"\x48\x8B\x8A\xA5\x5A\xA5\x5A";
/// let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
/// let instr = decoder.decode();
///
/// struct MySymbolResolver { map: HashMap<u64, String> }
/// impl SymbolResolver for MySymbolResolver {
///     fn symbol(&mut self, _instruction: &Instruction, _operand: u32, _instruction_operand: Option<u32>,
///          address: u64, _address_size: u32) -> Option<SymbolResult> {
///         if let Some(symbol_string) = self.map.get(&address) {
///             // The 'address' arg is the address of the symbol and doesn't have to be identical
///             // to the 'address' arg passed to symbol(). If it's different from the input
///             // address, the formatter will add +N or -N, eg. '[rax+symbol+123]'
///             Some(SymbolResult::with_str(address, symbol_string.as_str()))
///         } else {
///             None
///         }
///     }
/// }
///
/// // Hard code the symbols, it's just an example!😄
/// let mut sym_map: HashMap<u64, String> = HashMap::new();
/// sym_map.insert(0x5AA55AA5, String::from("my_data"));
///
/// let mut output = String::new();
/// let resolver = Box::new(MySymbolResolver { map: sym_map });
/// let mut formatter = FastFormatter::try_with_options(Some(resolver)).unwrap();
/// formatter.format(&instr, &mut output);
/// assert_eq!("mov rcx,[rdx+my_data]", output);
/// ```
pub type FastFormatter = SpecializedFormatter<DefaultFastFormatterTraitOptions>;

/// Default [`SpecializedFormatter<TraitOptions>`] options. It doesn't override any `const` or `fn`
///
/// [`SpecializedFormatter<TraitOptions>`]: struct.SpecializedFormatter.html
#[allow(missing_copy_implementations)]
#[allow(missing_debug_implementations)]
pub struct DefaultSpecializedFormatterTraitOptions;

impl SpecializedFormatterTraitOptions for DefaultSpecializedFormatterTraitOptions {}
