/*
    Iced (Dis)Assembler
    C-Compatible Exports
  
    TetzkatLipHoka 2022-2024
*/

use iced_x86_rust::{SpecializedFormatter, SpecializedFormatterTraitOptions, FastFormatterOptions};

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Specialized-Formatter Trait Combos
pub struct SpecializedFormatterTraitOptions000;
impl SpecializedFormatterTraitOptions for SpecializedFormatterTraitOptions000 {
    const ENABLE_SYMBOL_RESOLVER: bool = false;
    const ENABLE_DB_DW_DD_DQ: bool = false;

    #[must_use]
    #[inline]
    unsafe fn verify_output_has_enough_bytes_left() -> bool {
        // It's not possible to create 'unsafe const' items so we use a fn here
        false
    }

    #[must_use]
    #[inline]
    fn space_after_operand_separator(_options: &FastFormatterOptions) -> bool {
        _options.space_after_operand_separator()
    }

    #[must_use]
    #[inline]
    fn rip_relative_addresses(_options: &FastFormatterOptions) -> bool {
        _options.rip_relative_addresses()
    }

    #[must_use]
    #[inline]
    fn use_pseudo_ops(_options: &FastFormatterOptions) -> bool {
        _options.use_pseudo_ops()
    }

    #[must_use]
    #[inline]
    fn show_symbol_address(_options: &FastFormatterOptions) -> bool {
        _options.show_symbol_address()
    }

    #[must_use]
    #[inline]
    fn always_show_segment_register(_options: &FastFormatterOptions) -> bool {
        _options.always_show_segment_register()
    }

    #[must_use]
    #[inline]
    fn always_show_memory_size(_options: &FastFormatterOptions) -> bool {
        _options.always_show_memory_size()
    }

    #[must_use]
    #[inline]
    fn uppercase_hex(_options: &FastFormatterOptions) -> bool {
        _options.uppercase_hex()
    }

    #[must_use]
    #[inline]
    fn use_hex_prefix(_options: &FastFormatterOptions) -> bool {
        _options.use_hex_prefix()
    }
}

pub struct SpecializedFormatterTraitOptions100;
impl SpecializedFormatterTraitOptions for SpecializedFormatterTraitOptions100 {
    const ENABLE_SYMBOL_RESOLVER: bool = true;
    const ENABLE_DB_DW_DD_DQ: bool = false;

    #[must_use]
    #[inline]
    unsafe fn verify_output_has_enough_bytes_left() -> bool {
        // It's not possible to create 'unsafe const' items so we use a fn here
        false
    }

    #[must_use]
    #[inline]
    fn space_after_operand_separator(_options: &FastFormatterOptions) -> bool {
        _options.space_after_operand_separator()
    }

    #[must_use]
    #[inline]
    fn rip_relative_addresses(_options: &FastFormatterOptions) -> bool {
        _options.rip_relative_addresses()
    }

    #[must_use]
    #[inline]
    fn use_pseudo_ops(_options: &FastFormatterOptions) -> bool {
        _options.use_pseudo_ops()
    }

    #[must_use]
    #[inline]
    fn show_symbol_address(_options: &FastFormatterOptions) -> bool {
        _options.show_symbol_address()
    }

    #[must_use]
    #[inline]
    fn always_show_segment_register(_options: &FastFormatterOptions) -> bool {
        _options.always_show_segment_register()
    }

    #[must_use]
    #[inline]
    fn always_show_memory_size(_options: &FastFormatterOptions) -> bool {
        _options.always_show_memory_size()
    }

    #[must_use]
    #[inline]
    fn uppercase_hex(_options: &FastFormatterOptions) -> bool {
        _options.uppercase_hex()
    }

    #[must_use]
    #[inline]
    fn use_hex_prefix(_options: &FastFormatterOptions) -> bool {
        _options.use_hex_prefix()
    }
}

pub struct SpecializedFormatterTraitOptions010;
impl SpecializedFormatterTraitOptions for SpecializedFormatterTraitOptions010 {
    const ENABLE_SYMBOL_RESOLVER: bool = false;
    const ENABLE_DB_DW_DD_DQ: bool = true;

    #[must_use]
    #[inline]
    unsafe fn verify_output_has_enough_bytes_left() -> bool {
        // It's not possible to create 'unsafe const' items so we use a fn here
        false
    }

    #[must_use]
    #[inline]
    fn space_after_operand_separator(_options: &FastFormatterOptions) -> bool {
        _options.space_after_operand_separator()
    }

    #[must_use]
    #[inline]
    fn rip_relative_addresses(_options: &FastFormatterOptions) -> bool {
        _options.rip_relative_addresses()
    }

    #[must_use]
    #[inline]
    fn use_pseudo_ops(_options: &FastFormatterOptions) -> bool {
        _options.use_pseudo_ops()
    }

    #[must_use]
    #[inline]
    fn show_symbol_address(_options: &FastFormatterOptions) -> bool {
        _options.show_symbol_address()
    }

    #[must_use]
    #[inline]
    fn always_show_segment_register(_options: &FastFormatterOptions) -> bool {
        _options.always_show_segment_register()
    }

    #[must_use]
    #[inline]
    fn always_show_memory_size(_options: &FastFormatterOptions) -> bool {
        _options.always_show_memory_size()
    }

    #[must_use]
    #[inline]
    fn uppercase_hex(_options: &FastFormatterOptions) -> bool {
        _options.uppercase_hex()
    }

    #[must_use]
    #[inline]
    fn use_hex_prefix(_options: &FastFormatterOptions) -> bool {
        _options.use_hex_prefix()
    }
}

pub struct SpecializedFormatterTraitOptions001;
impl SpecializedFormatterTraitOptions for SpecializedFormatterTraitOptions001 {
    const ENABLE_SYMBOL_RESOLVER: bool = false;
    const ENABLE_DB_DW_DD_DQ: bool = false;

    #[must_use]
    #[inline]
    unsafe fn verify_output_has_enough_bytes_left() -> bool {
        // It's not possible to create 'unsafe const' items so we use a fn here
        true
    }

    #[must_use]
    #[inline]
    fn space_after_operand_separator(_options: &FastFormatterOptions) -> bool {
        _options.space_after_operand_separator()
    }

    #[must_use]
    #[inline]
    fn rip_relative_addresses(_options: &FastFormatterOptions) -> bool {
        _options.rip_relative_addresses()
    }

    #[must_use]
    #[inline]
    fn use_pseudo_ops(_options: &FastFormatterOptions) -> bool {
        _options.use_pseudo_ops()
    }

    #[must_use]
    #[inline]
    fn show_symbol_address(_options: &FastFormatterOptions) -> bool {
        _options.show_symbol_address()
    }

    #[must_use]
    #[inline]
    fn always_show_segment_register(_options: &FastFormatterOptions) -> bool {
        _options.always_show_segment_register()
    }

    #[must_use]
    #[inline]
    fn always_show_memory_size(_options: &FastFormatterOptions) -> bool {
        _options.always_show_memory_size()
    }

    #[must_use]
    #[inline]
    fn uppercase_hex(_options: &FastFormatterOptions) -> bool {
        _options.uppercase_hex()
    }

    #[must_use]
    #[inline]
    fn use_hex_prefix(_options: &FastFormatterOptions) -> bool {
        _options.use_hex_prefix()
    }
}

pub struct SpecializedFormatterTraitOptions011;
impl SpecializedFormatterTraitOptions for SpecializedFormatterTraitOptions011 {
    const ENABLE_SYMBOL_RESOLVER: bool = false;
    const ENABLE_DB_DW_DD_DQ: bool = true;

    #[must_use]
    #[inline]
    unsafe fn verify_output_has_enough_bytes_left() -> bool {
        // It's not possible to create 'unsafe const' items so we use a fn here
        true
    }

    #[must_use]
    #[inline]
    fn space_after_operand_separator(_options: &FastFormatterOptions) -> bool {
        _options.space_after_operand_separator()
    }

    #[must_use]
    #[inline]
    fn rip_relative_addresses(_options: &FastFormatterOptions) -> bool {
        _options.rip_relative_addresses()
    }

    #[must_use]
    #[inline]
    fn use_pseudo_ops(_options: &FastFormatterOptions) -> bool {
        _options.use_pseudo_ops()
    }

    #[must_use]
    #[inline]
    fn show_symbol_address(_options: &FastFormatterOptions) -> bool {
        _options.show_symbol_address()
    }

    #[must_use]
    #[inline]
    fn always_show_segment_register(_options: &FastFormatterOptions) -> bool {
        _options.always_show_segment_register()
    }

    #[must_use]
    #[inline]
    fn always_show_memory_size(_options: &FastFormatterOptions) -> bool {
        _options.always_show_memory_size()
    }

    #[must_use]
    #[inline]
    fn uppercase_hex(_options: &FastFormatterOptions) -> bool {
        _options.uppercase_hex()
    }

    #[must_use]
    #[inline]
    fn use_hex_prefix(_options: &FastFormatterOptions) -> bool {
        _options.use_hex_prefix()
    }
}

pub struct SpecializedFormatterTraitOptions111;
impl SpecializedFormatterTraitOptions for SpecializedFormatterTraitOptions111 {
    const ENABLE_SYMBOL_RESOLVER: bool = true;
    const ENABLE_DB_DW_DD_DQ: bool = true;

    #[must_use]
    #[inline]
    unsafe fn verify_output_has_enough_bytes_left() -> bool {
        // It's not possible to create 'unsafe const' items so we use a fn here
        true
    }

    #[must_use]
    #[inline]
    fn space_after_operand_separator(_options: &FastFormatterOptions) -> bool {
        _options.space_after_operand_separator()
    }

    #[must_use]
    #[inline]
    fn rip_relative_addresses(_options: &FastFormatterOptions) -> bool {
        _options.rip_relative_addresses()
    }

    #[must_use]
    #[inline]
    fn use_pseudo_ops(_options: &FastFormatterOptions) -> bool {
        _options.use_pseudo_ops()
    }

    #[must_use]
    #[inline]
    fn show_symbol_address(_options: &FastFormatterOptions) -> bool {
        _options.show_symbol_address()
    }

    #[must_use]
    #[inline]
    fn always_show_segment_register(_options: &FastFormatterOptions) -> bool {
        _options.always_show_segment_register()
    }

    #[must_use]
    #[inline]
    fn always_show_memory_size(_options: &FastFormatterOptions) -> bool {
        _options.always_show_memory_size()
    }

    #[must_use]
    #[inline]
    fn uppercase_hex(_options: &FastFormatterOptions) -> bool {
        _options.uppercase_hex()
    }

    #[must_use]
    #[inline]
    fn use_hex_prefix(_options: &FastFormatterOptions) -> bool {
        _options.use_hex_prefix()
    }
}

pub struct SpecializedFormatterTraitOptions101;
impl SpecializedFormatterTraitOptions for SpecializedFormatterTraitOptions101 {
    const ENABLE_SYMBOL_RESOLVER: bool = true;
    const ENABLE_DB_DW_DD_DQ: bool = false;

    #[must_use]
    #[inline]
    unsafe fn verify_output_has_enough_bytes_left() -> bool {
        // It's not possible to create 'unsafe const' items so we use a fn here
        true
    }

    #[must_use]
    #[inline]
    fn space_after_operand_separator(_options: &FastFormatterOptions) -> bool {
        _options.space_after_operand_separator()
    }

    #[must_use]
    #[inline]
    fn rip_relative_addresses(_options: &FastFormatterOptions) -> bool {
        _options.rip_relative_addresses()
    }

    #[must_use]
    #[inline]
    fn use_pseudo_ops(_options: &FastFormatterOptions) -> bool {
        _options.use_pseudo_ops()
    }

    #[must_use]
    #[inline]
    fn show_symbol_address(_options: &FastFormatterOptions) -> bool {
        _options.show_symbol_address()
    }

    #[must_use]
    #[inline]
    fn always_show_segment_register(_options: &FastFormatterOptions) -> bool {
        _options.always_show_segment_register()
    }

    #[must_use]
    #[inline]
    fn always_show_memory_size(_options: &FastFormatterOptions) -> bool {
        _options.always_show_memory_size()
    }

    #[must_use]
    #[inline]
    fn uppercase_hex(_options: &FastFormatterOptions) -> bool {
        _options.uppercase_hex()
    }

    #[must_use]
    #[inline]
    fn use_hex_prefix(_options: &FastFormatterOptions) -> bool {
        _options.use_hex_prefix()
    }
}

pub struct SpecializedFormatterTraitOptions110;
impl SpecializedFormatterTraitOptions for SpecializedFormatterTraitOptions110 {
    const ENABLE_SYMBOL_RESOLVER: bool = true;
    const ENABLE_DB_DW_DD_DQ: bool = true;

    #[must_use]
    #[inline]
    unsafe fn verify_output_has_enough_bytes_left() -> bool {
        // It's not possible to create 'unsafe const' items so we use a fn here
        false
    }

    #[must_use]
    #[inline]
    fn space_after_operand_separator(_options: &FastFormatterOptions) -> bool {
        _options.space_after_operand_separator()
    }

    #[must_use]
    #[inline]
    fn rip_relative_addresses(_options: &FastFormatterOptions) -> bool {
        _options.rip_relative_addresses()
    }

    #[must_use]
    #[inline]
    fn use_pseudo_ops(_options: &FastFormatterOptions) -> bool {
        _options.use_pseudo_ops()
    }

    #[must_use]
    #[inline]
    fn show_symbol_address(_options: &FastFormatterOptions) -> bool {
        _options.show_symbol_address()
    }

    #[must_use]
    #[inline]
    fn always_show_segment_register(_options: &FastFormatterOptions) -> bool {
        _options.always_show_segment_register()
    }

    #[must_use]
    #[inline]
    fn always_show_memory_size(_options: &FastFormatterOptions) -> bool {
        _options.always_show_memory_size()
    }

    #[must_use]
    #[inline]
    fn uppercase_hex(_options: &FastFormatterOptions) -> bool {
        _options.uppercase_hex()
    }

    #[must_use]
    #[inline]
    fn use_hex_prefix(_options: &FastFormatterOptions) -> bool {
        _options.use_hex_prefix()
    }
}

pub(crate) struct TSpecializedFormatter000 {
    pub Formatter : SpecializedFormatter<SpecializedFormatterTraitOptions000>,
    pub Output : String,
}  
pub(crate) struct TSpecializedFormatter100 {
    pub Formatter : SpecializedFormatter<SpecializedFormatterTraitOptions100>,
    pub Output : String,
}  
pub(crate) struct TSpecializedFormatter010 {
    pub Formatter : SpecializedFormatter<SpecializedFormatterTraitOptions010>,
    pub Output : String,
}  
pub(crate) struct TSpecializedFormatter001 {
    pub Formatter : SpecializedFormatter<SpecializedFormatterTraitOptions001>,
    pub Output : String,
}  
pub(crate) struct TSpecializedFormatter011 {
    pub Formatter : SpecializedFormatter<SpecializedFormatterTraitOptions011>,
    pub Output : String,
}  
pub(crate) struct TSpecializedFormatter111 {
    pub Formatter : SpecializedFormatter<SpecializedFormatterTraitOptions111>,
    pub Output : String,
}  
pub(crate) struct TSpecializedFormatter101 {
    pub Formatter : SpecializedFormatter<SpecializedFormatterTraitOptions101>,
    pub Output : String,
}  
pub(crate) struct TSpecializedFormatter110 {
    pub Formatter : SpecializedFormatter<SpecializedFormatterTraitOptions110>,
    pub Output : String,
}  