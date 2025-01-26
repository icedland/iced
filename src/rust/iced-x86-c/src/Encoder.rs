/*
    Iced (Dis)Assembler
    C-Compatible Exports
  
    TetzkatLipHoka 2022-2024
*/

use iced_x86_rust::{Instruction, Encoder, ConstantOffsets};
use std::ptr::null_mut;

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Encoder

// Creates an encoder
//
// Returns NULL if `bitness` is not one of 16, 32, 64.
//
// # Arguments
// * `bitness`: 16, 32 or 64
#[no_mangle]
pub extern "C" fn Encoder_Create( Bitness: u32, Capacity: usize ) -> *mut Encoder { 
    if Capacity > 0 {
        match Encoder::try_with_capacity( Bitness, Capacity ) {
            Ok( value ) => return Box::into_raw( Box::new( value ) ),
            Err( _e ) => return null_mut()
        }
    }else {
        match Encoder::try_new( Bitness ) {
            Ok( value ) => return Box::into_raw( Box::new( value ) ),
            Err( _e ) => return null_mut()
        }
   }
}

// Encodes an instruction and returns the size of the encoded instruction
//
// # Result
// * Returns written amount of encoded Bytes
//
// # Arguments
// * `instruction`: Instruction to encode
// * `rip`: `RIP` of the encoded instruction
#[no_mangle]
pub unsafe extern "C" fn Encoder_Encode( Encoder: *mut Encoder, Instruction: *mut Instruction, RIP: u64 ) -> usize {
    if Encoder.is_null() {
        return 0;
    }
    let mut obj = Box::from_raw( Encoder );

    let value = obj.encode( Instruction.as_mut().unwrap(), RIP );
    
    Box::into_raw( obj );

    match value {
        Ok( v ) => return v,
        Err( _e ) => return 0,
    }
}

// Writes a byte to the output buffer
//
// # Arguments
//
// `value`: Value to write
#[no_mangle]
pub unsafe extern "C" fn Encoder_WriteByte( Encoder: *mut Encoder, Value : u8 ) -> bool {
    if Encoder.is_null() {
        return false;
    }
    let mut obj = Box::from_raw( Encoder );

    obj.write_u8( Value );

    Box::into_raw( obj );

    return true;
}

// Returns the buffer and initializes the internal buffer to an empty vector. Should be called when
// you've encoded all instructions and need the raw instruction bytes. See also [ `Encoder_SetBuffer()` ].
#[no_mangle]
pub unsafe extern "C" fn Encoder_GetBuffer( Encoder: *mut Encoder, Buffer : *mut u8, Size : usize ) -> bool {     
    if Encoder.is_null() {
        return false;
    }

    if Buffer.is_null() {
        return false;
    }

    if Size <= 0 {
        return false;
    }

    let mut obj = Box::from_raw( Encoder );

    let value = obj.take_buffer();
    Box::into_raw( obj );

    let mut l = value.len();
    if l > Size {
        l = Size;
    }
    
    if l > 0 {
        for i in 0..l {
            *( Buffer.add( i ) ) = value[ i ];
        }
    }
    *( Buffer.add( l ) ) = 0;

    return true;
}

// Overwrites the buffer with a new vector. The old buffer is dropped. See also [ `Encoder_GetBuffer()` ].
// NOTE: Monitor the result of [`Encoder_Encode`] (Encoded Bytes).
// DO NOT Encode more Bytes than fitting your provided Buffer as this would cause a realloc - which will lead to an access violation.
// Disabled: Unsetting the Buffer seems impossible as Rust wants to deallocate the Vector .. 
/*
#[no_mangle]
pub unsafe extern "C" fn Encoder_SetBuffer( Encoder: *mut Encoder, Buffer : *mut u8, Size : usize ) -> bool {     
    if Encoder.is_null() {
        return false;
    }

    if !Buffer.is_null() && ( Size <= 0 ) {
        return false;
    }

    let mut obj = Box::from_raw( Encoder );

    if Buffer.is_null() { 
        obj.set_buffer( Vec::new() );
    }else {
        obj.set_buffer( Vec::from_raw_parts( Buffer, 0/*Used*/, Size/*TotalSize*/ ) );
    }
    
    Box::into_raw( obj );

    return true;
}
*/

// Gets the offsets of the constants ( memory displacement and immediate ) in the encoded instruction.
// The caller can use this information to add relocations if needed.
#[no_mangle]
pub unsafe extern "C" fn Encoder_GetConstantOffsets( Encoder: *mut Encoder, ConstantOffsets : *mut ConstantOffsets ) {
    if Encoder.is_null() {
        return;
    }
    let obj = Box::from_raw( Encoder );
    *ConstantOffsets = obj.get_constant_offsets();

    Box::into_raw( obj );
}

// Disables 2-byte VEX encoding and encodes all VEX instructions with the 3-byte VEX encoding
#[no_mangle]
pub unsafe extern "C" fn Encoder_GetPreventVex2( Encoder: *mut Encoder ) -> bool {
    if Encoder.is_null() {
        return false;
    }
    let obj = Box::from_raw( Encoder );

    let value = obj.prevent_vex2();

    Box::into_raw( obj );
 
    return value;
}

// Disables 2-byte VEX encoding and encodes all VEX instructions with the 3-byte VEX encoding
//
// # Arguments
// * `new_value`: new value
#[no_mangle]
pub unsafe extern "C" fn Encoder_SetPreventVex2( Encoder: *mut Encoder, Value : bool ) -> bool {
    if Encoder.is_null() {
        return false;
    }
    let mut obj = Box::from_raw( Encoder );

    obj.set_prevent_vex2( Value );

    Box::into_raw( obj );

    return true;
}

// Value of the `VEX.W` bit to use if it's an instruction that ignores the bit. Default is 0.
#[no_mangle]
pub unsafe extern "C" fn Encoder_GetVexWig( Encoder: *mut Encoder ) -> u32 {
    if Encoder.is_null() {
        return 0;
    }
    let obj = Box::from_raw( Encoder );

    let value = obj.vex_wig();

    Box::into_raw( obj );
 
    return value;
}

// Value of the `VEX.W` bit to use if it's an instruction that ignores the bit. Default is 0.
//
// # Arguments
// * `new_value`: new value ( 0 or 1 )
#[no_mangle]
pub unsafe extern "C" fn Encoder_SetVexWig( Encoder: *mut Encoder, Value : u32 ) -> bool {
    if Encoder.is_null() {
        return false;
    }
    let mut obj = Box::from_raw( Encoder );

    obj.set_vex_wig( Value );

    Box::into_raw( obj );

    return true;
}

// Value of the `VEX.L` bit to use if it's an instruction that ignores the bit. Default is 0.
#[no_mangle]
pub unsafe extern "C" fn Encoder_GetVexLig( Encoder: *mut Encoder ) -> u32 {
    if Encoder.is_null() {
        return 0;
    }
    let obj = Box::from_raw( Encoder );

    let value = obj.vex_lig();

    Box::into_raw( obj );
 
    return value;
}

// Value of the `VEX.L` bit to use if it's an instruction that ignores the bit. Default is 0.
//
// # Arguments
// * `new_value`: new value ( 0 or 1 )
#[no_mangle]
pub unsafe extern "C" fn Encoder_SetVexLig( Encoder: *mut Encoder, Value : u32 ) -> bool {
    if Encoder.is_null() {
        return false;
    }
    let mut obj = Box::from_raw( Encoder );

    obj.set_vex_lig( Value );

    Box::into_raw( obj );

    return true;
}

// Value of the `EVEX.W` bit to use if it's an instruction that ignores the bit. Default is 0.
#[no_mangle]
pub unsafe extern "C" fn Encoder_GetEvexWig( Encoder: *mut Encoder ) -> u32 {
    if Encoder.is_null() {
        return 0;
    }
    let obj = Box::from_raw( Encoder );

    let value = obj.evex_wig();

    Box::into_raw( obj );
 
    return value;
}

// Value of the `EVEX.W` bit to use if it's an instruction that ignores the bit. Default is 0.
//
// # Arguments
// * `new_value`: new value ( 0 or 1 )
#[no_mangle]
pub unsafe extern "C" fn Encoder_SetEvexWig( Encoder: *mut Encoder, Value : u32 ) -> bool {
    if Encoder.is_null() {
        return false;
    }
    let mut obj = Box::from_raw( Encoder );

    obj.set_evex_wig( Value );

    Box::into_raw( obj );

    return true;
}

// Value of the `EVEX.L'L` bits to use if it's an instruction that ignores the bits. Default is 0.
#[no_mangle]
pub unsafe extern "C" fn Encoder_GetEvexLig( Encoder: *mut Encoder ) -> u32 {
    if Encoder.is_null() {
        return 0;
    }
    let obj = Box::from_raw( Encoder );

    let value = obj.evex_lig();

    Box::into_raw( obj );
 
    return value;
}

// Value of the `EVEX.L'L` bits to use if it's an instruction that ignores the bits. Default is 0.
//
// # Arguments
// * `new_value`: new value ( 0 or 3 )
#[no_mangle]
pub unsafe extern "C" fn Encoder_SetEvexLig( Encoder: *mut Encoder, Value : u32 ) -> bool {
    if Encoder.is_null() {
        return false;
    }
    let mut obj = Box::from_raw( Encoder );

    obj.set_evex_lig( Value );

    Box::into_raw( obj );

    return true;
}

// Value of the `MVEX.W` bit to use if it's an instruction that ignores the bit. Default is 0.
#[cfg(feature = "mvex")]
#[no_mangle]
pub unsafe extern "C" fn Encoder_GetMvexWig( Encoder: *mut Encoder ) -> u32 {
    if Encoder.is_null() {
        return 0;
    }
    let obj = Box::from_raw( Encoder );

    let value = obj.mvex_wig();

    Box::into_raw( obj );
 
    return value;
}

// Value of the `MVEX.W` bit to use if it's an instruction that ignores the bit. Default is 0.
//
// # Arguments
// * `new_value`: new value ( 0 or 1 )
#[cfg(feature = "mvex")]
#[no_mangle]
pub unsafe extern "C" fn Encoder_SetMvexWig( Encoder: *mut Encoder, Value : u32 ) -> bool {
    if Encoder.is_null() {
        return false;
    }
    let mut obj = Box::from_raw( Encoder );

    obj.set_mvex_wig( Value );

    Box::into_raw( obj );

    return true;
}

// Gets the bitness ( 16, 32 or 64 )
#[no_mangle]
pub unsafe extern "C" fn Encoder_GetBitness( Encoder: *mut Encoder ) -> u32 {
    if Encoder.is_null() {
        return 0;
    }
    let obj = Box::from_raw( Encoder );

    let value = obj.bitness();

    Box::into_raw( obj );

    return value;
}