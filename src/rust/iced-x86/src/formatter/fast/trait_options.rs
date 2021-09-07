// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::formatter::fast::options::FastFormatterOptions;

/// A trait that allows you to hard code some formatter options which can make
/// the formatter faster and use less code.
///
/// The implementing struct can return hard coded values and/or return a value from the
/// passed in options. If it returns the value from the passed in options, that option can
/// be modified at runtime by calling `formatter.options_mut().set_<option>(new_value)`,
/// else it's ignored and calling that method has no effect (except wasting CPU cycles).
///
/// Every `fn` must be a pure function and must return a value from the `options` input or
/// a literal (`true` or `false`). Returning a literal is recommended since the compiler can
/// remove unused formatter code.
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
/// See [`SpecializedFormatter<TraitOptions>`] for more examples
///
/// [`SpecializedFormatter<TraitOptions>`]: struct.SpecializedFormatter.html
pub trait SpecializedFormatterTraitOptions {
	// Not a public API.
	// It's used by the formatter to detect FastFormatter so its speed doesn't regress
	// when we optimize SpecializedFormatter with hard coded options.
	#[doc(hidden)]
	const __IS_FAST_FORMATTER: bool = false;

	/// Enables support for a symbol resolver. This is disabled by default. If this
	/// is disabled, you must not pass in a symbol resolver to the constructor.
	///
	/// For fastest code, this should be *disabled*, not enabled.
	const ENABLE_SYMBOL_RESOLVER: bool = false;

	/// Enables support for formatting `db`, `dw`, `dd`, `dq`.
	///
	/// For fastest code, this should be *disabled*, not enabled.
	const ENABLE_DB_DW_DD_DQ: bool = false;

	/// The formatter makes sure that the `output` string has at least 300 bytes left at
	/// the start of `format()` and also after appending symbols to `output`. This is enough
	/// space for all formatted instructions.
	///
	/// *No formatted instruction will ever get close to being 300 bytes long!*
	///
	/// If this function returns `false`, the formatter won't verify that it has
	/// enough bytes left when writing to the `output` string. Note that it will
	/// always reserve at least 300 bytes at the start of `format()` and after
	/// appending symbols.
	///
	/// For fastest code, this method should return `false`. Default is `true`.
	///
	/// # Safety
	///
	/// See the above description.
	#[must_use]
	#[inline]
	unsafe fn verify_output_has_enough_bytes_left() -> bool {
		// It's not possible to create 'unsafe const' items so we use a fn here
		true
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
	/// * `options`: Current formatter options
	#[must_use]
	#[inline]
	fn space_after_operand_separator(_options: &FastFormatterOptions) -> bool {
		false
	}

	/// Show `RIP+displ` or the virtual address
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// 👍 | `true` | `mov eax,[rip+12345678h]`
	/// _ | `false` | `mov eax,[1029384756AFBECDh]`
	///
	/// # Arguments
	///
	/// * `options`: Current formatter options
	#[must_use]
	#[inline]
	fn rip_relative_addresses(_options: &FastFormatterOptions) -> bool {
		true
	}

	/// Use pseudo instructions
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// _ | `true` | `vcmpnltsd xmm2,xmm6,xmm3`
	/// 👍 | `false` | `vcmpsd xmm2,xmm6,xmm3,5h`
	///
	/// # Arguments
	///
	/// * `options`: Current formatter options
	#[must_use]
	#[inline]
	fn use_pseudo_ops(_options: &FastFormatterOptions) -> bool {
		false
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
	/// * `options`: Current formatter options
	#[must_use]
	#[inline]
	fn show_symbol_address(_options: &FastFormatterOptions) -> bool {
		false
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
	/// * `options`: Current formatter options
	#[must_use]
	#[inline]
	fn always_show_segment_register(_options: &FastFormatterOptions) -> bool {
		false
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
	/// * `options`: Current formatter options
	#[must_use]
	#[inline]
	fn always_show_memory_size(_options: &FastFormatterOptions) -> bool {
		false
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
	/// * `options`: Current formatter options
	#[must_use]
	#[inline]
	fn uppercase_hex(_options: &FastFormatterOptions) -> bool {
		true
	}

	/// Use a hex prefix (`0x`) or a hex suffix (`h`)
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// 👍 | `true` | `0x5A`
	/// _ | `false` | `5Ah`
	///
	/// # Arguments
	///
	/// * `options`: Current formatter options
	#[must_use]
	#[inline]
	fn use_hex_prefix(_options: &FastFormatterOptions) -> bool {
		true
	}
}
