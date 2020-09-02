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

pub(super) mod enums;
mod fmt_data;
mod fmt_tbl;
mod mem_size_tbl;
#[cfg(test)]
mod tests;

use self::enums::*;
use self::fmt_tbl::FMT_DATA;
use self::mem_size_tbl::MEM_SIZE_TBL;
use super::super::*;
use super::fmt_utils_all::*;
use super::instruction_internal::get_address_size_in_bytes;
use super::pseudo_ops::get_pseudo_ops;
use super::regs_tbl::REGS_TBL;
use super::*;
#[cfg(not(feature = "std"))]
use alloc::boxed::Box;
#[cfg(not(feature = "std"))]
use alloc::vec::Vec;
use core::{mem, u16, u32, u8, usize};

static SCALE_NUMBERS: [&str; 4] = ["*1", "*2", "*4", "*8"];
static RC_STRINGS: [&str; 4] = ["{rn-sae}", "{rd-sae}", "{ru-sae}", "{rz-sae}"];

struct FmtTableData {
	mnemonics: Vec<&'static str>,
	flags: Vec<u8>, // FastFmtFlags
}

struct Flags1;
impl Flags1 {
	const SPACE_AFTER_OPERAND_SEPARATOR: u32 = 0x0000_0001;
	const RIP_RELATIVE_ADDRESSES: u32 = 0x0000_0002;
	const USE_PSEUDO_OPS: u32 = 0x0000_0004;
	const SHOW_SYMBOL_ADDRESS: u32 = 0x0000_0008;
	const ALWAYS_SHOW_SEGMENT_REGISTER: u32 = 0x0000_0010;
	const ALWAYS_SHOW_MEMORY_SIZE: u32 = 0x0000_0020;
	const UPPERCASE_HEX: u32 = 0x0000_0040;
	const USE_HEX_PREFIX: u32 = 0x0000_0080;
}

/// Fast formatter options
#[derive(Debug, Clone, Eq, PartialEq, Hash)]
#[allow(missing_copy_implementations)]
pub struct FastFormatterOptions {
	options1: u32,
}

impl FastFormatterOptions {
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	fn new() -> Self {
		Self { options1: Flags1::USE_PSEUDO_OPS | Flags1::UPPERCASE_HEX }
	}

	// NOTE: These tables must render correctly by `cargo doc` and inside of IDEs, eg. VSCode.
	// An extra `-` is needed for `cargo doc`.

	/// Add a space after the operand separator
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov rax, rcx`
	/// ✔️ | `false` | `mov rax,rcx`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn space_after_operand_separator(&self) -> bool {
		(self.options1 & Flags1::SPACE_AFTER_OPERAND_SEPARATOR) != 0
	}

	/// Add a space after the operand separator
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov rax, rcx`
	/// ✔️ | `false` | `mov rax,rcx`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_space_after_operand_separator(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::SPACE_AFTER_OPERAND_SEPARATOR;
		} else {
			self.options1 &= !Flags1::SPACE_AFTER_OPERAND_SEPARATOR;
		}
	}

	/// Show `RIP+displ` or the virtual address
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,[rip+12345678h]`
	/// ✔️ | `false` | `mov eax,[1029384756AFBECDh]`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn rip_relative_addresses(&self) -> bool {
		(self.options1 & Flags1::RIP_RELATIVE_ADDRESSES) != 0
	}

	/// Show `RIP+displ` or the virtual address
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,[rip+12345678h]`
	/// ✔️ | `false` | `mov eax,[1029384756AFBECDh]`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_rip_relative_addresses(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::RIP_RELATIVE_ADDRESSES;
		} else {
			self.options1 &= !Flags1::RIP_RELATIVE_ADDRESSES;
		}
	}

	/// Use pseudo instructions
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// ✔️ | `true` | `vcmpnltsd xmm2,xmm6,xmm3`
	/// - | `false` | `vcmpsd xmm2,xmm6,xmm3,5`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn use_pseudo_ops(&self) -> bool {
		(self.options1 & Flags1::USE_PSEUDO_OPS) != 0
	}

	/// Use pseudo instructions
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// ✔️ | `true` | `vcmpnltsd xmm2,xmm6,xmm3`
	/// - | `false` | `vcmpsd xmm2,xmm6,xmm3,5`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_use_pseudo_ops(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::USE_PSEUDO_OPS;
		} else {
			self.options1 &= !Flags1::USE_PSEUDO_OPS;
		}
	}

	/// Show the original value after the symbol name
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,[myfield (12345678)]`
	/// ✔️ | `false` | `mov eax,[myfield]`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn show_symbol_address(&self) -> bool {
		(self.options1 & Flags1::SHOW_SYMBOL_ADDRESS) != 0
	}

	/// Show the original value after the symbol name
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,[myfield (12345678)]`
	/// ✔️ | `false` | `mov eax,[myfield]`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_show_symbol_address(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::SHOW_SYMBOL_ADDRESS;
		} else {
			self.options1 &= !Flags1::SHOW_SYMBOL_ADDRESS;
		}
	}

	/// Always show the effective segment register. If the option is `false`, only show the segment register if
	/// there's a segment override prefix.
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,ds:[ecx]`
	/// ✔️ | `false` | `mov eax,[ecx]`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn always_show_segment_register(&self) -> bool {
		(self.options1 & Flags1::ALWAYS_SHOW_SEGMENT_REGISTER) != 0
	}

	/// Always show the effective segment register. If the option is `false`, only show the segment register if
	/// there's a segment override prefix.
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,ds:[ecx]`
	/// ✔️ | `false` | `mov eax,[ecx]`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_always_show_segment_register(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::ALWAYS_SHOW_SEGMENT_REGISTER;
		} else {
			self.options1 &= !Flags1::ALWAYS_SHOW_SEGMENT_REGISTER;
		}
	}

	/// Always show memory operands' size
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,dword ptr [ebx]` / `add byte ptr [eax],0x12`
	/// ✔️ | `false` | `mov eax,[ebx]` / `add byte ptr [eax],0x12`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn always_show_memory_size(&self) -> bool {
		(self.options1 & Flags1::ALWAYS_SHOW_MEMORY_SIZE) != 0
	}

	/// Always show memory operands' size
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,dword ptr [ebx]` / `add byte ptr [eax],0x12`
	/// ✔️ | `false` | `mov eax,[ebx]` / `add byte ptr [eax],0x12`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_always_show_memory_size(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::ALWAYS_SHOW_MEMORY_SIZE;
		} else {
			self.options1 &= !Flags1::ALWAYS_SHOW_MEMORY_SIZE;
		}
	}

	/// Use upper case hex digits
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// ✔️ | `true` | `0xFF`
	/// - | `false` | `0xff`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn uppercase_hex(&self) -> bool {
		(self.options1 & Flags1::UPPERCASE_HEX) != 0
	}

	/// Use upper case hex digits
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// ✔️ | `true` | `0xFF`
	/// - | `false` | `0xff`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_uppercase_hex(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::UPPERCASE_HEX;
		} else {
			self.options1 &= !Flags1::UPPERCASE_HEX;
		}
	}

	/// Use a hex prefix (`0x`) or a hex suffix (`h`)
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `0x5A`
	/// ✔️ | `false` | `5Ah`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn use_hex_prefix(&self) -> bool {
		(self.options1 & Flags1::USE_HEX_PREFIX) != 0
	}

	/// Always show memory operands' size
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,dword ptr [ebx]` / `add byte ptr [eax],0x12`
	/// ✔️ | `false` | `mov eax,[ebx]` / `add byte ptr [eax],0x12`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_use_hex_prefix(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::USE_HEX_PREFIX;
		} else {
			self.options1 &= !Flags1::USE_HEX_PREFIX;
		}
	}
}

/// Fast formatter with less formatting options and with masm-like syntax.
/// Use it if formatting speed is more important than being able to re-assemble formatted instructions.
///
/// This formatter is 1.6-1.8x faster than the other formatters (the time includes decoding + formatting).
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
/// assert_eq!("vcvtne2ps2bf16 zmm2{k5}{z}, zmm6, dword bcst [rax+4h]", output);
/// ```
///
/// Using a symbol resolver:
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
///     fn symbol(&mut self, instruction: &Instruction, operand: u32, instruction_operand: Option<u32>,
///          address: u64, address_size: u32) -> Option<SymbolResult> {
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
/// let mut formatter = FastFormatter::with_options(Some(resolver));
/// formatter.format(&instr, &mut output);
/// assert_eq!("mov rcx,[rdx+my_data]", output);
/// ```
#[allow(missing_debug_implementations)]
pub struct FastFormatter {
	d: SelfData,
	symbol_resolver: Option<Box<SymbolResolver>>,
}

impl Default for FastFormatter {
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	fn default() -> Self {
		FastFormatter::new()
	}
}

// Read-only data which is needed a couple of times due to borrow checker
struct SelfData {
	options: FastFormatterOptions,
	all_registers: &'static [FormatterString],
	code_mnemonics: &'static [&'static str],
	code_flags: &'static [u8],
	all_memory_sizes: &'static [&'static str],
}

impl FastFormatter {
	const SHOW_USELESS_PREFIXES: bool = true;

	/// Creates a fast formatter
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn new() -> Self {
		FastFormatter::with_options(None)
	}

	/// Creates a fast formatter
	///
	/// # Arguments
	///
	/// - `symbol_resolver`: Symbol resolver or `None`
	#[cfg_attr(has_must_use, must_use)]
	#[cfg_attr(feature = "cargo-clippy", allow(clippy::missing_inline_in_public_items))]
	pub fn with_options(symbol_resolver: Option<Box<SymbolResolver>>) -> Self {
		Self {
			d: SelfData {
				options: FastFormatterOptions::new(),
				all_registers: &*REGS_TBL,
				code_mnemonics: &FMT_DATA.mnemonics,
				code_flags: &FMT_DATA.flags,
				all_memory_sizes: &*MEM_SIZE_TBL,
			},
			symbol_resolver,
		}
	}

	/// Gets the formatter options (immutable)
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn options(&self) -> &FastFormatterOptions {
		&self.d.options
	}

	/// Gets the formatter options (mutable)
	#[cfg_attr(has_must_use, must_use)]
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
	#[cfg_attr(feature = "cargo-clippy", allow(clippy::missing_inline_in_public_items))]
	pub fn format(&mut self, instruction: &Instruction, output: &mut String) {
		let code = instruction.code();

		// Safe, all Code values are valid indexes
		let mut mnemonic = unsafe { *self.d.code_mnemonics.get_unchecked(code as usize) };
		// Safe, all Code values are valid indexes
		let flags = unsafe { *self.d.code_flags.get_unchecked(code as usize) };

		let mut op_count = instruction.op_count();
		let pseudo_ops_num = flags >> FastFmtFlags::PSEUDO_OPS_KIND_SHIFT;
		if pseudo_ops_num != 0 && self.d.options.use_pseudo_ops() && instruction.op_kind(op_count - 1) == OpKind::Immediate8 {
			let mut index = instruction.immediate8() as usize;
			// Safe, the generator generates only valid values (1-based)
			let pseudo_ops_kind: PseudoOpsKind = unsafe { mem::transmute(pseudo_ops_num - 1) };
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
			if let Some(pseudo_op_mnemonic) = pseudo_ops.get(index) {
				mnemonic = pseudo_op_mnemonic.lower();
				op_count -= 1;
			}
		}

		let prefix_seg = instruction.segment_prefix();
		const_assert_eq!(0, Register::None as u32);
		if ((prefix_seg as u32) | super::super::instruction_internal::internal_has_any_of_xacquire_xrelease_lock_rep_repne_prefix(instruction)) != 0 {
			let has_notrack_prefix = prefix_seg == Register::DS && is_notrack_prefix_branch(code);
			if !has_notrack_prefix && prefix_seg != Register::None && FastFormatter::show_segment_prefix(instruction, op_count) {
				FastFormatter::format_register(&self.d, output, prefix_seg);
				output.push(' ');
			}

			if instruction.has_xacquire_prefix() {
				output.push_str("xacquire ");
			}
			if instruction.has_xrelease_prefix() {
				output.push_str("xrelease ");
			}
			if instruction.has_lock_prefix() {
				output.push_str("lock ");
			}

			if has_notrack_prefix {
				output.push_str("notrack ");
			}

			if instruction.has_repe_prefix()
				&& (FastFormatter::SHOW_USELESS_PREFIXES || show_rep_or_repe_prefix_bool(code, FastFormatter::SHOW_USELESS_PREFIXES))
			{
				if is_repe_or_repne_instruction(code) {
					output.push_str("repe ");
				} else {
					output.push_str("rep ");
				}
			}
			if instruction.has_repne_prefix() {
				if (Code::Retnw_imm16 <= code && code <= Code::Retnq)
					|| (Code::Call_rel16 <= code && code <= Code::Jmp_rel32_64)
					|| (Code::Call_rm16 <= code && code <= Code::Call_rm64)
					|| (Code::Jmp_rm16 <= code && code <= Code::Jmp_rm64)
					|| code.is_jcc_short_or_near()
				{
					output.push_str("bnd ");
				} else if FastFormatter::SHOW_USELESS_PREFIXES || show_repne_prefix_bool(code, FastFormatter::SHOW_USELESS_PREFIXES) {
					output.push_str("repne ");
				}
			}
		}

		output.push_str(mnemonic);

		let is_declare_data;
		let declare_data_kind = if (code as u32).wrapping_sub(1) <= (Code::DeclareQword as u32 - 1) {
			op_count = instruction.declare_data_len() as u32;
			is_declare_data = true;
			match code {
				Code::DeclareByte => OpKind::Immediate8,
				Code::DeclareWord => OpKind::Immediate16,
				Code::DeclareDword => OpKind::Immediate32,
				_ => {
					debug_assert_eq!(Code::DeclareQword, code);
					OpKind::Immediate64
				}
			}
		} else {
			is_declare_data = false;
			OpKind::Register
		};

		if op_count > 0 {
			output.push(' ');

			for operand in 0..op_count {
				if operand > 0 {
					if self.d.options.space_after_operand_separator() {
						output.push_str(", ");
					} else {
						output.push(',');
					}
				}

				let imm8;
				let imm16;
				let imm32;
				let imm64;
				let imm_size;
				let op_kind = if is_declare_data { declare_data_kind } else { instruction.op_kind(operand) };
				match op_kind {
					OpKind::Register => FastFormatter::format_register(&self.d, output, instruction.op_register(operand)),

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
						if let Some(ref symbol) = if let Some(ref mut symbol_resolver) = self.symbol_resolver {
							symbol_resolver.symbol(instruction, operand, Some(operand), imm64, imm_size)
						} else {
							None
						} {
							FastFormatter::write_symbol(output, imm64, symbol, &self.d.options);
						} else {
							FastFormatter::format_number(output, imm64, &self.d.options);
						}
					}

					OpKind::FarBranch16 | OpKind::FarBranch32 => {
						if op_kind == OpKind::FarBranch32 {
							imm_size = 4;
							imm64 = instruction.far_branch32() as u64;
						} else {
							imm_size = 2;
							imm64 = instruction.far_branch16() as u64;
						}
						let mut vec: Vec<SymResTextPart> = Vec::new();
						if let Some(ref symbol) = if let Some(ref mut symbol_resolver) = self.symbol_resolver {
							to_owned(symbol_resolver.symbol(instruction, operand, Some(operand), imm64 as u32 as u64, imm_size), &mut vec)
						} else {
							None
						} {
							debug_assert!(operand + 1 == 1);
							let selector_symbol = if let Some(ref mut symbol_resolver) = self.symbol_resolver {
								symbol_resolver.symbol(instruction, operand + 1, Some(operand), instruction.far_branch_selector() as u64, 2)
							} else {
								None
							};
							if let Some(ref selector_symbol) = selector_symbol {
								FastFormatter::write_symbol(output, instruction.far_branch_selector() as u64, selector_symbol, &self.d.options);
							} else {
								FastFormatter::format_number(output, instruction.far_branch_selector() as u64, &self.d.options);
							}
							output.push(':');
							FastFormatter::write_symbol(output, imm64, symbol, &self.d.options);
						} else {
							FastFormatter::format_number(output, instruction.far_branch_selector() as u64, &self.d.options);
							output.push(':');
							FastFormatter::format_number(output, imm64, &self.d.options);
						}
					}

					OpKind::Immediate8 | OpKind::Immediate8_2nd => {
						if is_declare_data {
							imm8 = instruction.get_declare_byte_value(operand as usize);
						} else if op_kind == OpKind::Immediate8 {
							imm8 = instruction.immediate8();
						} else {
							debug_assert_eq!(OpKind::Immediate8_2nd, op_kind);
							imm8 = instruction.immediate8_2nd();
						}
						if let Some(ref symbol) = if let Some(ref mut symbol_resolver) = self.symbol_resolver {
							symbol_resolver.symbol(instruction, operand, Some(operand), imm8 as u64, 1)
						} else {
							None
						} {
							if (symbol.flags & SymbolFlags::RELATIVE) == 0 {
								output.push_str("offset ");
							}
							FastFormatter::write_symbol(output, imm8 as u64, symbol, &self.d.options);
						} else {
							FastFormatter::format_number(output, imm8 as u64, &self.d.options);
						}
					}

					OpKind::Immediate16 | OpKind::Immediate8to16 => {
						if is_declare_data {
							imm16 = instruction.get_declare_word_value(operand as usize);
						} else if op_kind == OpKind::Immediate16 {
							imm16 = instruction.immediate16();
						} else {
							debug_assert_eq!(OpKind::Immediate8to16, op_kind);
							imm16 = instruction.immediate8to16() as u16;
						}
						if let Some(ref symbol) = if let Some(ref mut symbol_resolver) = self.symbol_resolver {
							symbol_resolver.symbol(instruction, operand, Some(operand), imm16 as u64, 2)
						} else {
							None
						} {
							if (symbol.flags & SymbolFlags::RELATIVE) == 0 {
								output.push_str("offset ");
							}
							FastFormatter::write_symbol(output, imm16 as u64, symbol, &self.d.options);
						} else {
							FastFormatter::format_number(output, imm16 as u64, &self.d.options);
						}
					}

					OpKind::Immediate32 | OpKind::Immediate8to32 => {
						if is_declare_data {
							imm32 = instruction.get_declare_dword_value(operand as usize);
						} else if op_kind == OpKind::Immediate32 {
							imm32 = instruction.immediate32();
						} else {
							debug_assert_eq!(OpKind::Immediate8to32, op_kind);
							imm32 = instruction.immediate8to32() as u32;
						}
						if let Some(ref symbol) = if let Some(ref mut symbol_resolver) = self.symbol_resolver {
							symbol_resolver.symbol(instruction, operand, Some(operand), imm32 as u64, 4)
						} else {
							None
						} {
							if (symbol.flags & SymbolFlags::RELATIVE) == 0 {
								output.push_str("offset ");
							}
							FastFormatter::write_symbol(output, imm32 as u64, symbol, &self.d.options);
						} else {
							FastFormatter::format_number(output, imm32 as u64, &self.d.options);
						}
					}

					OpKind::Immediate64 | OpKind::Immediate8to64 | OpKind::Immediate32to64 => {
						if is_declare_data {
							imm64 = instruction.get_declare_qword_value(operand as usize);
						} else if op_kind == OpKind::Immediate32to64 {
							imm64 = instruction.immediate32to64() as u64;
						} else if op_kind == OpKind::Immediate8to64 {
							imm64 = instruction.immediate8to64() as u64;
						} else {
							debug_assert_eq!(OpKind::Immediate64, op_kind);
							imm64 = instruction.immediate64();
						}
						if let Some(ref symbol) = if let Some(ref mut symbol_resolver) = self.symbol_resolver {
							symbol_resolver.symbol(instruction, operand, Some(operand), imm64, 8)
						} else {
							None
						} {
							if (symbol.flags & SymbolFlags::RELATIVE) == 0 {
								output.push_str("offset ");
							}
							FastFormatter::write_symbol(output, imm64, symbol, &self.d.options);
						} else {
							FastFormatter::format_number(output, imm64, &self.d.options);
						}
					}

					OpKind::MemorySegSI => self.format_memory(
						output,
						instruction,
						operand,
						instruction.memory_size(),
						instruction.segment_prefix(),
						instruction.memory_segment(),
						Register::SI,
						Register::None,
						0,
						0,
						0,
						2,
					),
					OpKind::MemorySegESI => self.format_memory(
						output,
						instruction,
						operand,
						instruction.memory_size(),
						instruction.segment_prefix(),
						instruction.memory_segment(),
						Register::ESI,
						Register::None,
						0,
						0,
						0,
						4,
					),
					OpKind::MemorySegRSI => self.format_memory(
						output,
						instruction,
						operand,
						instruction.memory_size(),
						instruction.segment_prefix(),
						instruction.memory_segment(),
						Register::RSI,
						Register::None,
						0,
						0,
						0,
						8,
					),
					OpKind::MemorySegDI => self.format_memory(
						output,
						instruction,
						operand,
						instruction.memory_size(),
						instruction.segment_prefix(),
						instruction.memory_segment(),
						Register::DI,
						Register::None,
						0,
						0,
						0,
						2,
					),
					OpKind::MemorySegEDI => self.format_memory(
						output,
						instruction,
						operand,
						instruction.memory_size(),
						instruction.segment_prefix(),
						instruction.memory_segment(),
						Register::EDI,
						Register::None,
						0,
						0,
						0,
						4,
					),
					OpKind::MemorySegRDI => self.format_memory(
						output,
						instruction,
						operand,
						instruction.memory_size(),
						instruction.segment_prefix(),
						instruction.memory_segment(),
						Register::RDI,
						Register::None,
						0,
						0,
						0,
						8,
					),
					OpKind::MemoryESDI => self.format_memory(
						output,
						instruction,
						operand,
						instruction.memory_size(),
						instruction.segment_prefix(),
						Register::ES,
						Register::DI,
						Register::None,
						0,
						0,
						0,
						2,
					),
					OpKind::MemoryESEDI => self.format_memory(
						output,
						instruction,
						operand,
						instruction.memory_size(),
						instruction.segment_prefix(),
						Register::ES,
						Register::EDI,
						Register::None,
						0,
						0,
						0,
						4,
					),
					OpKind::MemoryESRDI => self.format_memory(
						output,
						instruction,
						operand,
						instruction.memory_size(),
						instruction.segment_prefix(),
						Register::ES,
						Register::RDI,
						Register::None,
						0,
						0,
						0,
						8,
					),
					OpKind::Memory64 => self.format_memory(
						output,
						instruction,
						operand,
						instruction.memory_size(),
						instruction.segment_prefix(),
						instruction.memory_segment(),
						Register::None,
						Register::None,
						0,
						8,
						instruction.memory_address64() as i64,
						8,
					),

					OpKind::Memory => {
						let displ_size = instruction.memory_displ_size();
						let base_reg = instruction.memory_base();
						let mut index_reg = instruction.memory_index();
						let addr_size = get_address_size_in_bytes(base_reg, index_reg, displ_size, instruction.code_size());
						let displ =
							if addr_size == 8 { instruction.memory_displacement64() as i64 } else { instruction.memory_displacement() as i64 };
						if code == Code::Xlat_m8 {
							index_reg = Register::None;
						}
						self.format_memory(
							output,
							instruction,
							operand,
							instruction.memory_size(),
							instruction.segment_prefix(),
							instruction.memory_segment(),
							base_reg,
							index_reg,
							super::super::instruction_internal::internal_get_memory_index_scale(instruction),
							displ_size,
							displ,
							addr_size,
						);
					}
				}

				if operand == 0 && instruction.has_op_mask() {
					output.push('{');
					FastFormatter::format_register(&self.d, output, instruction.op_mask());
					output.push('}');
					if instruction.zeroing_masking() {
						output.push_str("{z}");
					}
				}
			}
			if super::super::instruction_internal::internal_has_rounding_control_or_sae(instruction) {
				let rc = instruction.rounding_control();
				if rc != RoundingControl::None {
					const_assert_eq!(0, RoundingControl::None as u32);
					const_assert_eq!(1, RoundingControl::RoundToNearest as u32);
					const_assert_eq!(2, RoundingControl::RoundDown as u32);
					const_assert_eq!(3, RoundingControl::RoundUp as u32);
					const_assert_eq!(4, RoundingControl::RoundTowardZero as u32);
					output.push_str(RC_STRINGS[rc as usize - 1]);
				} else {
					debug_assert!(instruction.suppress_all_exceptions());
					output.push_str("{sae}");
				}
			}
		}
	}

	// Only one caller so inline it
	#[cfg_attr(has_must_use, must_use)]
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
				| OpKind::Memory64
				| OpKind::Memory => return false,
			}
		}

		FastFormatter::SHOW_USELESS_PREFIXES
	}

	#[inline]
	fn format_register(d: &SelfData, output: &mut String, register: Register) {
		// Safe, all Register values are valid indexes
		output.push_str(unsafe { d.all_registers.get_unchecked(register as usize) }.lower());
	}

	fn format_number(output: &mut String, value: u64, options: &FastFormatterOptions) {
		if options.use_hex_prefix() {
			output.push_str("0x");
		}

		let mut digits = 1;
		let mut tmp = value;
		loop {
			tmp >>= 4;
			if tmp == 0 {
				break;
			}
			digits += 1;
		}

		let hex_high = if options.uppercase_hex() { 'A' as u32 - 10 } else { 'a' as u32 - 10 };
		if !options.use_hex_prefix() && digits < 17 && ((value >> ((digits - 1) << 2)) & 0xF) > 9 {
			digits += 1; // Another 0
		}
		for i in 0..digits {
			let index = digits - i - 1;
			let digit = if index >= 16 { 0 } else { ((value >> (index << 2)) & 0xF) as u32 };
			if digit > 9 {
				output.push((digit + hex_high) as u8 as char);
			} else {
				output.push((digit + '0' as u32) as u8 as char);
			}
		}

		if !options.use_hex_prefix() {
			output.push('h');
		}
	}

	#[inline]
	fn write_symbol(output: &mut String, address: u64, symbol: &SymbolResult, options: &FastFormatterOptions) {
		FastFormatter::write_symbol2(output, address, symbol, options, true);
	}

	#[cold]
	fn write_symbol2(output: &mut String, address: u64, symbol: &SymbolResult, options: &FastFormatterOptions, write_minus_if_signed: bool) {
		let mut displ = address.wrapping_sub(symbol.address) as i64;
		if (symbol.flags & SymbolFlags::SIGNED) != 0 {
			if write_minus_if_signed {
				output.push('-');
			}
			displ = displ.wrapping_neg();
		}

		match symbol.text {
			SymResTextInfo::Text(ref part) => {
				let s = match &part.text {
					&SymResString::Str(s) => s,
					&SymResString::String(ref s) => s.as_str(),
				};
				output.push_str(s);
			}

			SymResTextInfo::TextVec(v) => {
				for part in v.iter() {
					let s = match &part.text {
						&SymResString::Str(s) => s,
						&SymResString::String(ref s) => s.as_str(),
					};
					output.push_str(s);
				}
			}
		}

		if displ != 0 {
			if displ < 0 {
				output.push('-');
				displ = displ.wrapping_neg();
			} else {
				output.push('+');
			}
			FastFormatter::format_number(output, displ as u64, options);
		}
		if options.show_symbol_address() {
			output.push_str(" (");
			FastFormatter::format_number(output, address, options);
			output.push(')');
		}
	}

	fn format_memory(
		&mut self, output: &mut String, instruction: &Instruction, operand: u32, mem_size: MemorySize, seg_override: Register, seg_reg: Register,
		mut base_reg: Register, index_reg: Register, scale: u32, mut displ_size: u32, mut displ: i64, addr_size: u32,
	) {
		debug_assert!((scale as usize) < SCALE_NUMBERS.len());
		debug_assert!(get_address_size_in_bytes(base_reg, index_reg, displ_size, instruction.code_size()) == addr_size);

		let abs_addr;
		if base_reg == Register::RIP {
			abs_addr = (instruction.next_ip() as i64).wrapping_add(displ as i32 as i64) as u64;
			if !self.d.options.rip_relative_addresses() {
				debug_assert_eq!(Register::None, index_reg);
				base_reg = Register::None;
				displ = abs_addr as i64;
				displ_size = 8;
			}
		} else if base_reg == Register::EIP {
			abs_addr = instruction.next_ip32().wrapping_add(displ as u32) as u64;
			if !self.d.options.rip_relative_addresses() {
				debug_assert_eq!(Register::None, index_reg);
				base_reg = Register::None;
				displ = abs_addr as i64;
				displ_size = 4;
			}
		} else {
			abs_addr = displ as u64;
		}

		let symbol = if let Some(ref mut symbol_resolver) = self.symbol_resolver {
			symbol_resolver.symbol(instruction, operand, Some(operand), abs_addr, addr_size)
		} else {
			None
		};

		let mut use_scale = scale != 0;
		if !use_scale {
			// [rsi] = base reg, [rsi*1] = index reg
			if base_reg == Register::None {
				use_scale = true;
			}
		}
		if addr_size == 2 {
			use_scale = false;
		}

		// Safe, all Code values are valid indexes
		let flags = unsafe { *self.d.code_flags.get_unchecked(instruction.code() as usize) };
		let show_mem_size =
			(flags & (FastFmtFlags::FORCE_MEM_SIZE as u8)) != 0 || mem_size.is_broadcast() || self.d.options.always_show_memory_size();
		if show_mem_size {
			// Safe, all MemorySize values are valid indexes
			let keywords = unsafe { *self.d.all_memory_sizes.get_unchecked(mem_size as usize) };
			output.push_str(keywords);
		}

		let code_size = instruction.code_size();
		let notrack_prefix = seg_override == Register::DS
			&& is_notrack_prefix_branch(instruction.code())
			&& !((code_size == CodeSize::Code16 || code_size == CodeSize::Code32)
				&& (base_reg == Register::BP || base_reg == Register::EBP || base_reg == Register::ESP));
		if self.d.options.always_show_segment_register()
			|| (seg_override != Register::None
				&& !notrack_prefix
				&& (FastFormatter::SHOW_USELESS_PREFIXES
					|| show_segment_prefix_bool(Register::None, instruction, FastFormatter::SHOW_USELESS_PREFIXES)))
		{
			FastFormatter::format_register(&self.d, output, seg_reg);
			output.push(':');
		}
		output.push('[');

		let mut need_plus = if base_reg != Register::None {
			FastFormatter::format_register(&self.d, output, base_reg);
			true
		} else {
			false
		};

		if index_reg != Register::None {
			if need_plus {
				output.push('+');
			}
			need_plus = true;

			FastFormatter::format_register(&self.d, output, index_reg);
			if use_scale {
				output.push_str(SCALE_NUMBERS[scale as usize]);
			}
		}

		if let Some(ref symbol) = symbol {
			if need_plus {
				if (symbol.flags & SymbolFlags::SIGNED) != 0 {
					output.push('-');
				} else {
					output.push('+');
				}
			} else if (symbol.flags & SymbolFlags::SIGNED) != 0 {
				output.push('-');
			}

			FastFormatter::write_symbol2(output, abs_addr, symbol, &self.d.options, false);
		} else if !need_plus || (displ_size != 0 && displ != 0) {
			if need_plus {
				if addr_size == 4 {
					if (displ as i32) < 0 {
						displ = (displ as i32).wrapping_neg() as u32 as i64;
						output.push('-');
					} else {
						output.push('+');
					}
				} else if addr_size == 8 {
					if displ < 0 {
						displ = displ.wrapping_neg();
						output.push('-');
					} else {
						output.push('+');
					}
				} else {
					debug_assert_eq!(2, addr_size);
					if (displ as i16) < 0 {
						displ = (displ as i16).wrapping_neg() as u16 as i64;
						output.push('-');
					} else {
						output.push('+');
					}
				}
			}
			FastFormatter::format_number(output, displ as u64, &self.d.options);
		}

		output.push(']');
	}
}
