/*
    Iced (Dis)Assembler
    C-Compatible Exports
  
    TetzkatLipHoka 2022-2024
*/

use iced_x86::{Instruction, InstructionBlock, ConstantOffsets, RelocInfo, BlockEncoder, BlockEncoderResult};
use std::{slice, ptr::null_mut};

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// BlockEncoder

// Encodes instructions. Any number of branches can be part of this block.
// You can use this function to move instructions from one location to another location.
// If the target of a branch is too far away, it'll be rewritten to a longer branch.
// You can disable this by passing in [`BlockEncoderOptions::DONT_FIX_BRANCHES`].
// If the block has any `RIP`-relative memory operands, make sure the data isn't too
// far away from the new location of the encoded instructions. Every OS should have
// some API to allocate memory close (+/-2GB) to the original code location.
//
// # Errors
// Returns 0-Data if it failed to encode one or more instructions.
//
// # Arguments 
// * `bitness`: 16, 32, or 64
// * `Intructions`: First Instruction to encode
// * `Count`: Instruction-Count
// * `Options`: Encoder options, see [`TBlockEncoderOptions`]
// * `Results`: Result-Structure
//
// # Result
// * Pointer to Result-Data. Musst be free'd using RustFreeMemory()
//#[repr(C)]
#[repr(packed)]
pub struct TBlockEncoderResult {
	/// Base IP of all encoded instructions
	pub rip: u64,

	/// The bytes of all encoded instructions
	pub code_buffer: *const u8,
    pub code_buffer_len: usize,

	/// If [`BlockEncoderOptions::RETURN_RELOC_INFOS`] option was enabled:
	///
	/// All [`RelocInfo`]s.
	///
	/// [`BlockEncoderOptions::RETURN_RELOC_INFOS`]: struct.BlockEncoderOptions.html#associatedconstant.RETURN_RELOC_INFOS
	/// [`RelocInfo`]: struct.RelocInfo.html
	pub reloc_infos: *const RelocInfo,
    pub reloc_infos_len: usize,

	/// If [`BlockEncoderOptions::RETURN_NEW_INSTRUCTION_OFFSETS`] option was enabled:
	///
	/// Offsets of the instructions relative to the base IP. If the instruction was rewritten to a new instruction
	/// (eg. `JE TARGET_TOO_FAR_AWAY` -> `JNE SHORT SKIP ;JMP QWORD PTR [MEM]`), the value `u32::MAX` is stored in that element.
	///
	/// [`BlockEncoderOptions::RETURN_NEW_INSTRUCTION_OFFSETS`]: struct.BlockEncoderOptions.html#associatedconstant.RETURN_NEW_INSTRUCTION_OFFSETS
	pub new_instruction_offsets: *const u32,
    pub new_instruction_offsets_len: usize,    

	/// If [`BlockEncoderOptions::RETURN_CONSTANT_OFFSETS`] option was enabled:
	///
	/// Offsets of all constants in the new encoded instructions. If the instruction was rewritten,
	/// the `default()` value is stored in the corresponding element.
	///
	/// [`BlockEncoderOptions::RETURN_CONSTANT_OFFSETS`]: struct.BlockEncoderOptions.html#associatedconstant.RETURN_CONSTANT_OFFSETS
	pub constant_offsets: *const ConstantOffsets,
    pub constant_offsets_len: usize,    
}

#[no_mangle]
pub unsafe extern "C" fn BlockEncoder( Bitness: u32, RIP : u64, Instructions: *mut Instruction, Count: usize, Result: *mut TBlockEncoderResult, Options: u32 ) -> *mut BlockEncoderResult { 
    if Instructions.is_null() {
        return null_mut();
    }
    if Count <= 0 {
        return null_mut();
    }
    if Result.is_null() {
        return null_mut();
    }

    let instructions = slice::from_raw_parts( Instructions, Count );
    let block = InstructionBlock::new( &instructions, RIP );
    match BlockEncoder::encode( Bitness, block, Options ) {
        Ok( value ) => {
            (*Result).rip = value.rip;

            if value.code_buffer.len() > 0 {
                (*Result).code_buffer = value.code_buffer.as_ptr();
            }else {
                (*Result).code_buffer = null_mut();
            }
            (*Result).code_buffer_len = value.code_buffer.len();

            if value.reloc_infos.len() > 0 {
                (*Result).reloc_infos = value.reloc_infos.as_ptr();
            }else {
                (*Result).reloc_infos = null_mut();
            }
            (*Result).reloc_infos_len = value.reloc_infos.len();

            if value.new_instruction_offsets.len() > 0 {
                (*Result).new_instruction_offsets = value.new_instruction_offsets.as_ptr();
            }else {
                (*Result).new_instruction_offsets = null_mut();
            }
            (*Result).new_instruction_offsets_len = value.new_instruction_offsets.len();

            if value.constant_offsets.len() > 0 {
                (*Result).constant_offsets = value.constant_offsets.as_ptr();
            }else {
                (*Result).constant_offsets = null_mut();
            }
            (*Result).constant_offsets_len = value.constant_offsets.len();
            return Box::into_raw( Box::new( value ) );
        },
        Err( _e ) => {
            (*Result).rip = 0 as u64;
            (*Result).code_buffer = null_mut();
            (*Result).code_buffer_len = 0;
            (*Result).reloc_infos = null_mut();
            (*Result).reloc_infos_len = 0;
            (*Result).new_instruction_offsets = null_mut();
            (*Result).new_instruction_offsets_len = 0;
            (*Result).constant_offsets = null_mut();
            (*Result).constant_offsets_len = 0;
            return null_mut();
        }
    }
}
