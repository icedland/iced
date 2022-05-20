// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

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
	#[must_use]
	#[inline]
	pub(super) const fn new() -> Self {
		Self { options1: Flags1::USE_PSEUDO_OPS | Flags1::UPPERCASE_HEX }
	}

	// NOTE: These tables must render correctly by `cargo doc` and inside of IDEs, eg. VSCode.

	/// Add a space after the operand separator
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// _ | `true` | `mov rax, rcx`
	/// 👍 | `false` | `mov rax,rcx`
	#[must_use]
	#[inline]
	pub const fn space_after_operand_separator(&self) -> bool {
		(self.options1 & Flags1::SPACE_AFTER_OPERAND_SEPARATOR) != 0
	}

	/// Add a space after the operand separator
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// _ | `true` | `mov rax, rcx`
	/// 👍 | `false` | `mov rax,rcx`
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
	/// _ | `true` | `mov eax,[rip+12345678h]`
	/// 👍 | `false` | `mov eax,[1029384756AFBECDh]`
	#[must_use]
	#[inline]
	pub const fn rip_relative_addresses(&self) -> bool {
		(self.options1 & Flags1::RIP_RELATIVE_ADDRESSES) != 0
	}

	/// Show `RIP+displ` or the virtual address
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// _ | `true` | `mov eax,[rip+12345678h]`
	/// 👍 | `false` | `mov eax,[1029384756AFBECDh]`
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
	/// 👍 | `true` | `vcmpnltsd xmm2,xmm6,xmm3`
	/// _ | `false` | `vcmpsd xmm2,xmm6,xmm3,5h`
	#[must_use]
	#[inline]
	pub const fn use_pseudo_ops(&self) -> bool {
		(self.options1 & Flags1::USE_PSEUDO_OPS) != 0
	}

	/// Use pseudo instructions
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// 👍 | `true` | `vcmpnltsd xmm2,xmm6,xmm3`
	/// _ | `false` | `vcmpsd xmm2,xmm6,xmm3,5h`
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
	/// _ | `true` | `mov eax,[myfield (12345678)]`
	/// 👍 | `false` | `mov eax,[myfield]`
	#[must_use]
	#[inline]
	pub const fn show_symbol_address(&self) -> bool {
		(self.options1 & Flags1::SHOW_SYMBOL_ADDRESS) != 0
	}

	/// Show the original value after the symbol name
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// _ | `true` | `mov eax,[myfield (12345678)]`
	/// 👍 | `false` | `mov eax,[myfield]`
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
	/// _ | `true` | `mov eax,ds:[ecx]`
	/// 👍 | `false` | `mov eax,[ecx]`
	#[must_use]
	#[inline]
	pub const fn always_show_segment_register(&self) -> bool {
		(self.options1 & Flags1::ALWAYS_SHOW_SEGMENT_REGISTER) != 0
	}

	/// Always show the effective segment register. If the option is `false`, only show the segment register if
	/// there's a segment override prefix.
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// _ | `true` | `mov eax,ds:[ecx]`
	/// 👍 | `false` | `mov eax,[ecx]`
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

	/// Always show the size of memory operands
	///
	/// Default | Value | Example | Example
	/// --------|-------|---------|--------
	/// _ | `true` | `mov eax,dword ptr [ebx]` | `add byte ptr [eax],0x12`
	/// 👍 | `false` | `mov eax,[ebx]` | `add byte ptr [eax],0x12`
	#[must_use]
	#[inline]
	pub const fn always_show_memory_size(&self) -> bool {
		(self.options1 & Flags1::ALWAYS_SHOW_MEMORY_SIZE) != 0
	}

	/// Always show the size of memory operands
	///
	/// Default | Value | Example | Example
	/// --------|-------|---------|--------
	/// _ | `true` | `mov eax,dword ptr [ebx]` | `add byte ptr [eax],0x12`
	/// 👍 | `false` | `mov eax,[ebx]` | `add byte ptr [eax],0x12`
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

	/// Use uppercase hex digits
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// 👍 | `true` | `0xFF`
	/// _ | `false` | `0xff`
	#[must_use]
	#[inline]
	pub const fn uppercase_hex(&self) -> bool {
		(self.options1 & Flags1::UPPERCASE_HEX) != 0
	}

	/// Use uppercase hex digits
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// 👍 | `true` | `0xFF`
	/// _ | `false` | `0xff`
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
	/// _ | `true` | `0x5A`
	/// 👍 | `false` | `5Ah`
	#[must_use]
	#[inline]
	pub const fn use_hex_prefix(&self) -> bool {
		(self.options1 & Flags1::USE_HEX_PREFIX) != 0
	}

	/// Use a hex prefix (`0x`) or a hex suffix (`h`)
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// _ | `true` | `0x5A`
	/// 👍 | `false` | `5Ah`
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
