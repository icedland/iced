/*
    Iced (Dis)Assembler
    C-Compatible Exports
  
    TetzkatLipHoka 2022-2024
*/

use iced_x86::{Instruction, Decoder, ConstantOffsets};
use std::{slice, ptr::null_mut};

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Decoder

// Creates a decoder
//
// # Errors
// Fails if `bitness` is not one of 16, 32, 64.
//
// # Arguments
// * `bitness`: 16, 32 or 64
// * `data`: Data to decode
// * `data`: ByteSize of `Data`
// * `options`: Decoder options, `0` or eg. `DecoderOptions::NO_INVALID_CHECK | DecoderOptions::AMD`
#[no_mangle]
pub extern "C" fn Decoder_Create( Bitness: u32, Data: *const u8, DataSize : usize, IP: u64, Options: u32 ) -> *mut Decoder<'static> {
    let data2 = unsafe { slice::from_raw_parts( Data, DataSize ) };
    match Decoder::try_with_ip( Bitness, data2, IP, Options ) {
        Ok( value ) => return Box::into_raw( Box::new( value ) ),
        Err( _e ) => return null_mut()
    }
}

// Returns `true` if there's at least one more byte to decode. It doesn't verify that the
// next instruction is valid, it only checks if there's at least one more byte to read.
// See also [ `position()` ] and [ `max_position()` ]
//
// It's not required to call this method. If this method returns `false`, then [ `decode_out()` ]
// and [ `decode()` ] will return an instruction whose [ `code()` ] == [ `Code::INVALID` ].
#[no_mangle]
pub unsafe extern "C" fn Decoder_CanDecode( Decoder: *mut Decoder ) -> bool {
    if Decoder.is_null() {
        return false;
    }
    let obj = Box::from_raw( Decoder );

    let value = obj.can_decode();

    Box::into_raw( obj );

    return value;
}

// Gets the current `IP`/`EIP`/`RIP` value, see also [ `position()` ]
#[no_mangle]
pub unsafe extern "C" fn Decoder_GetIP( Decoder: *mut Decoder ) -> u64 {
    if Decoder.is_null() {
        return 0;
    }
    let obj = Box::from_raw( Decoder );

    let value = obj.ip();

    Box::into_raw( obj );

    return value;
}

// Sets the current `IP`/`EIP`/`RIP` value, see also [ `try_set_position()` ]
// This method only updates the IP value, it does not change the data position, use [ `try_set_position()` ] to change the position.
#[no_mangle]
pub unsafe extern "C" fn Decoder_SetIP( Decoder: *mut Decoder, Value : u64 ) -> bool {
    if Decoder.is_null() {
        return false;
    }
    let mut obj = Box::from_raw( Decoder );

    obj.set_ip( Value );

    Box::into_raw( obj );

    return true;
}

// Gets the bitness ( 16, 32 or 64 )
#[no_mangle]
pub unsafe extern "C" fn Decoder_GetBitness( Decoder: *mut Decoder ) -> u32 {
    if Decoder.is_null() {
        return 0;
    }
    let obj = Box::from_raw( Decoder );

    let value = obj.bitness();

    Box::into_raw( obj );

    return value;
}

// Gets the max value that can be passed to [ `try_set_position()` ]. This is the size of the data that gets
// decoded to instructions and it's the length of the slice that was passed to the constructor.
#[no_mangle]
pub unsafe extern "C" fn Decoder_GetMaxPosition( Decoder: *mut Decoder ) -> usize {
    if Decoder.is_null() {
        return 0;
    }
    let obj = Box::from_raw( Decoder );

    let value = obj.max_position();

    Box::into_raw( obj );
 
    return value;
}

// Gets the current data position. This value is always <= [ `max_position()` ].
// When [ `position()` ] == [ `max_position()` ], it's not possible to decode more
// instructions and [ `can_decode()` ] returns `false`.
#[no_mangle]
pub unsafe extern "C" fn Decoder_GetPosition( Decoder: *mut Decoder ) -> usize {
    if Decoder.is_null() {
        return 0;
    }
    let obj = Box::from_raw( Decoder );

    let value = obj.position();

    Box::into_raw( obj );
 
    return value;
}

// Sets the current data position, which is the index into the data passed to the constructor.
// This value is always <= [ `max_position()` ]
#[no_mangle]
pub unsafe extern "C" fn Decoder_SetPosition( Decoder: *mut Decoder, Value : usize ) -> bool {
    if Decoder.is_null() {
        return false;
    }
    let mut obj = Box::from_raw( Decoder );

    let value = obj.set_position( Value );

    Box::into_raw( obj );

    return value.is_ok();
}

// Gets the last decoder error. Unless you need to know the reason it failed,
// it's better to check [ `instruction.is_invalid()` ].
#[no_mangle]
pub unsafe extern "C" fn Decoder_GetLastError( Decoder: *mut Decoder ) -> u8 { // FFI-Unsafe: TDecoderError
    if Decoder.is_null() {
        return 0;// TDecoderError::None;
    }
    let obj = Box::from_raw( Decoder );

    let value: u8/*TDecoderError*/ = obj.last_error() as u8;

    Box::into_raw( obj );
   
    return value;
}

// Decodes and returns the next instruction, see also [ `decode_out( &mut Instruction )` ]
// which avoids copying the decoded instruction to the caller's return variable.
// See also [ `last_error()` ].
#[no_mangle]
pub unsafe extern "C" fn Decoder_Decode( Decoder: *mut Decoder, Instruction: *mut Instruction ) {
    if Decoder.is_null() {
        return;
    }
    let mut obj = Box::from_raw( Decoder );

    obj.decode_out( Instruction.as_mut().unwrap() );

    Box::into_raw( obj );
}

// Gets the offsets of the constants ( memory displacement and immediate ) in the decoded instruction.
// The caller can check if there are any relocations at those addresses.
//
// # Arguments
// * `instruction`: The latest instruction that was decoded by this decoder
#[no_mangle]
pub unsafe extern "C" fn Decoder_GetConstantOffsets( Decoder: *mut Decoder, Instruction: *mut Instruction, ConstantOffsets : *mut ConstantOffsets ) -> bool {
    if Decoder.is_null() {
        return false;
    }
    let obj = Box::from_raw( Decoder );
    *ConstantOffsets = obj.get_constant_offsets( Instruction.as_mut().unwrap() );

    Box::into_raw( obj );
    return true;
}