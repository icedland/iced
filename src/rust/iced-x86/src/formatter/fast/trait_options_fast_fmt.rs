// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::formatter::fast::options::FastFormatterOptions;
use crate::formatter::fast::trait_options::SpecializedFormatterTraitOptions;

/// Default [`FastFormatter`] options
///
/// [`FastFormatter`]: type.FastFormatter.html
#[allow(missing_copy_implementations)]
#[allow(missing_debug_implementations)]
pub struct DefaultFastFormatterTraitOptions;

impl SpecializedFormatterTraitOptions for DefaultFastFormatterTraitOptions {
	/// Set to `true` so symbol resolvers can be used
	const ENABLE_SYMBOL_RESOLVER: bool = true;

	/// Add a space after the operand separator
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// &nbsp; | `true` | `mov rax, rcx`
	/// ✔️ | `false` | `mov rax,rcx`
	///
	/// # Arguments
	///
	/// * `options`: Current formatter options
	#[must_use]
	#[inline]
	fn space_after_operand_separator(options: &FastFormatterOptions) -> bool {
		options.space_after_operand_separator()
	}

	/// Show `RIP+displ` or the virtual address
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// &nbsp; | `true` | `mov eax,[rip+12345678h]`
	/// ✔️ | `false` | `mov eax,[1029384756AFBECDh]`
	///
	/// # Arguments
	///
	/// * `options`: Current formatter options
	#[must_use]
	#[inline]
	fn rip_relative_addresses(options: &FastFormatterOptions) -> bool {
		options.rip_relative_addresses()
	}

	/// Use pseudo instructions
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// ✔️ | `true` | `vcmpnltsd xmm2,xmm6,xmm3`
	/// &nbsp; | `false` | `vcmpsd xmm2,xmm6,xmm3,5h`
	///
	/// # Arguments
	///
	/// * `options`: Current formatter options
	#[must_use]
	#[inline]
	fn use_pseudo_ops(options: &FastFormatterOptions) -> bool {
		options.use_pseudo_ops()
	}

	/// Show the original value after the symbol name
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// &nbsp; | `true` | `mov eax,[myfield (12345678)]`
	/// ✔️ | `false` | `mov eax,[myfield]`
	///
	/// # Arguments
	///
	/// * `options`: Current formatter options
	#[must_use]
	#[inline]
	fn show_symbol_address(options: &FastFormatterOptions) -> bool {
		options.show_symbol_address()
	}

	/// Always show the effective segment register. If the option is `false`, only show the segment register if
	/// there's a segment override prefix.
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// &nbsp; | `true` | `mov eax,ds:[ecx]`
	/// ✔️ | `false` | `mov eax,[ecx]`
	///
	/// # Arguments
	///
	/// * `options`: Current formatter options
	#[must_use]
	#[inline]
	fn always_show_segment_register(options: &FastFormatterOptions) -> bool {
		options.always_show_segment_register()
	}

	/// Always show the size of memory operands
	///
	/// Default | Value | Example | Example
	/// --------|-------|---------|--------
	/// &nbsp; | `true` | `mov eax,dword ptr [ebx]` | `add byte ptr [eax],0x12`
	/// ✔️ | `false` | `mov eax,[ebx]` | `add byte ptr [eax],0x12`
	///
	/// # Arguments
	///
	/// * `options`: Current formatter options
	#[must_use]
	#[inline]
	fn always_show_memory_size(options: &FastFormatterOptions) -> bool {
		options.always_show_memory_size()
	}

	/// Use uppercase hex digits
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// ✔️ | `true` | `0xFF`
	/// &nbsp; | `false` | `0xff`
	///
	/// # Arguments
	///
	/// * `options`: Current formatter options
	#[must_use]
	#[inline]
	fn uppercase_hex(options: &FastFormatterOptions) -> bool {
		options.uppercase_hex()
	}

	/// Use a hex prefix (`0x`) or a hex suffix (`h`)
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// &nbsp; | `true` | `0x5A`
	/// ✔️ | `false` | `5Ah`
	///
	/// # Arguments
	///
	/// * `options`: Current formatter options
	#[must_use]
	#[inline]
	fn use_hex_prefix(options: &FastFormatterOptions) -> bool {
		options.use_hex_prefix()
	}
}
