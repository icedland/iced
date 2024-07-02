/*
    Iced (Dis)Assembler
    C-Compatible Exports
  
    TetzkatLipHoka 2022-2024
*/

use iced_x86_rust::{Instruction, Register};

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Virtual-Address Resolver
// Gets the virtual address of a memory operand
//
// # Arguments
// * `operand`: Operand number, 0-4, must be a memory operand
// * `element_index`: Only used if it's a vsib memory operand. This is the element index of the vector index register.
// * `get_register_value`: Function that returns the value of a register or the base address of a segment register, or `None` for unsupported
//    registers.
//
// # Call-back function args
// * Arg 1: `register`: Register (GPR8, GPR16, GPR32, GPR64, XMM, YMM, ZMM, seg). If it's a segment register, the call-back function should return the segment's base address, not the segment's register value.
// * Arg 2: `element_index`: Only used if it's a vsib memory operand. This is the element index of the vector index register.
// * Arg 3: `element_size`: Only used if it's a vsib memory operand. Size in bytes of elements in vector index register (4 or 8).
type
  TVirtualAddressResolverCallback = unsafe extern "C" fn( Register: u8/*Register*/, Index: usize, Size: usize, Address : *mut u64, UserData : *const usize ) -> bool;// FFI-Unsafe: Register

#[no_mangle]
pub unsafe extern "C" fn Instruction_VirtualAddress( Instruction: *mut Instruction, Callback : Option<TVirtualAddressResolverCallback>, Operand : u32, Index : usize, UserData : *const usize ) -> u64 {
    if Instruction.is_null() {
       return 0
    }
    if Callback.is_none() {
        return 0
    }

    let va = Instruction.as_mut().unwrap().virtual_address(Operand, Index, 
        |Register, Index, Size| {
            match Register {                
                Register::ES | Register::CS | Register::SS | Register::DS => Some( 0 ), // The base address of ES, CS, SS and DS is always 0 in 64-bit mode
                _ => {
                    let mut value : u64 = 0;
                    if Callback.unwrap()( Register as u8, Index, Size, &mut value as *mut u64, UserData ) {
                        Some( value )
                    }else {
                        None
                    }
                }
            }
        }
    );

    let res: u64;
    match va {
        None => res = 0x0,
        _ => res = va.unwrap()
    }
    return res;
}