/*
    Iced (Dis)Assembler
    C-Compatible Exports
  
    TetzkatLipHoka 2022-2024
*/

use iced_x86::{Instruction, NumberBase, NumberFormattingOptions, FormatterOperandOptions, FormatterOptionsProvider};
use std::ptr::eq;
use std::{slice, str};
use libc::{c_char, strlen};
use std::ffi::CString;

// Called by the formatter. The method can override any options before the formatter uses them.
//
// # Arguments
// - `instruction`: Instruction
// - `operand`: Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand.
// - `instruction_operand`: Instruction operand number, 0-based, or `None` if it's an operand created by the formatter.
// - `options`: Options. Only those options that will be used by the formatter are initialized.
// - `number_options`: Number formatting options
pub(crate)type
  TFormatterOptionsProviderCallback = unsafe extern "C" fn( Instruction: &Instruction, Operand: u32, InstructionOperand: u32, Options: &mut FormatterOperandOptions, NumberOptions: &mut TNumberFormattingOptions, UserData : *const usize );

  pub(crate) struct TFormatterOptionsProvider {
    pub(crate) userData: *const usize,
    pub(crate) callback: Option<TFormatterOptionsProviderCallback>
}

//#[repr(C)]
#[repr(packed)]
pub struct TNumberFormattingOptions {
	/// Number prefix or an empty string
	pub prefix: *const c_char,
	/// Number suffix or an empty string
	pub suffix: *const c_char,
	/// Digit separator or an empty string to not use a digit separator
	pub digit_separator: *const c_char,
	/// Size of a digit group or 0 to not use a digit separator
	pub digit_group_size: u8,
	/// Number base
	pub number_base: NumberBase,
	/// Use uppercase hex digits
	pub uppercase_hex: bool,
	/// Small hex numbers ( -9 .. 9 ) are shown in decimal
	pub small_hex_numbers_in_decimal: bool,
	/// Add a leading zero to hex numbers if there's no prefix and the number starts with hex digits `A-F`
	pub add_leading_zero_to_hex_numbers: bool,
	/// If `true`, add leading zeros to numbers, eg. `1h` vs `00000001h`
	pub leading_zeros: bool,
	/// If `true`, the number is signed, and if `false` it's an unsigned number
	pub signed_number: bool,
	/// Add leading zeros to displacements
	pub displacement_leading_zeros: bool,
}

impl FormatterOptionsProvider for TFormatterOptionsProvider {
    fn operand_options( &mut self, Instruction: &Instruction, Operand: u32, InstructionOperand: Option<u32>, Options: &mut FormatterOperandOptions, NumberOptions: &mut NumberFormattingOptions<'_> ) {
        unsafe {
            if !self.callback.is_none() {   
                let tprefix = CString::new( NumberOptions.prefix ).unwrap();
                let tsuffix = CString::new( NumberOptions.suffix ).unwrap();
                let tdigit_separator = CString::new( NumberOptions.digit_separator ).unwrap();

                let pprefix = tprefix.as_ptr() as *const c_char;
                let psuffix = tsuffix.as_ptr() as *const c_char;
                let pdigit_separator = tdigit_separator.as_ptr() as *const c_char;
              
                let mut numberoptions = TNumberFormattingOptions {                    
                    prefix: pprefix,
                    suffix: psuffix,
                    digit_separator: pdigit_separator,

                    digit_group_size : NumberOptions.digit_group_size,
                    number_base : NumberOptions.number_base,
                    uppercase_hex : NumberOptions.uppercase_hex,
                    small_hex_numbers_in_decimal : NumberOptions.small_hex_numbers_in_decimal,
                    add_leading_zero_to_hex_numbers : NumberOptions.add_leading_zero_to_hex_numbers,
                    leading_zeros : NumberOptions.leading_zeros,
                    signed_number : NumberOptions.signed_number,
                    displacement_leading_zeros : NumberOptions.displacement_leading_zeros
                };

                match InstructionOperand {
                    None => self.callback.unwrap()( Instruction, Operand, 0xFFFF_FFFF as u32, Options, &mut numberoptions, self.userData ),
                    _Some => self.callback.unwrap()( Instruction, Operand, InstructionOperand.unwrap(), Options, &mut numberoptions, self.userData )
                }

                if !eq(pprefix, numberoptions.prefix) {
                    NumberOptions.prefix = str::from_utf8_unchecked( slice::from_raw_parts( numberoptions.prefix as *const u8, strlen( numberoptions.prefix ) ) );
                }

                if !eq(psuffix, numberoptions.suffix) {
                    NumberOptions.suffix = str::from_utf8_unchecked( slice::from_raw_parts( numberoptions.suffix as *const u8, strlen( numberoptions.suffix ) ) );
                }

                if !eq(pdigit_separator, numberoptions.digit_separator) {
                    NumberOptions.digit_separator = str::from_utf8_unchecked( slice::from_raw_parts( numberoptions.digit_separator as *const u8, strlen( numberoptions.digit_separator ) ) );
                }

                NumberOptions.digit_group_size = numberoptions.digit_group_size;
                NumberOptions.number_base = numberoptions.number_base;
                NumberOptions.uppercase_hex = numberoptions.uppercase_hex;
                NumberOptions.small_hex_numbers_in_decimal = numberoptions.small_hex_numbers_in_decimal;
                NumberOptions.add_leading_zero_to_hex_numbers = numberoptions.add_leading_zero_to_hex_numbers;
                NumberOptions.leading_zeros = numberoptions.leading_zeros;
                NumberOptions.signed_number = numberoptions.signed_number;
                NumberOptions.displacement_leading_zeros = numberoptions.displacement_leading_zeros;
            }
        };
    }
}