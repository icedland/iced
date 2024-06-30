/*
    Iced (Dis)Assembler
    C-Compatible Exports
  
    TetzkatLipHoka 2022-2024
*/

use iced_x86::Decoder;

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Free Memory
#[no_mangle]
pub unsafe extern "C" fn IcedFreeMemory( Pointer: *mut Decoder ) -> bool { 
    if Pointer.is_null() {
        return false;
    }

    drop( Box::from_raw( Pointer ) );
    return true;
}