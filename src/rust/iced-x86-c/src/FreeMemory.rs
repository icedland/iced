/*
    Iced (Dis)Assembler
    C-Compatible Exports
  
    TetzkatLipHoka 2022-2024
*/

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Free Memory
#[no_mangle]
pub unsafe extern "C" fn IcedFreeMemory( Pointer: *mut u8 ) -> bool { 
    if Pointer.is_null() {
        return false;
    }

    drop( Box::from_raw( Pointer ) );
    return true;
}