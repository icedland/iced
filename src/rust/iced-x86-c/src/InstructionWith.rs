/*
    Iced (Dis)Assembler
    C-Compatible Exports
  
    TetzkatLipHoka 2022-2024
*/

use iced_x86_rust::{Instruction, Register, MemoryOperand, RepPrefixKind};
use std::mem::transmute;// Enum<->Int

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Instruction 'WITH'
// Creates an instruction with no operands
#[no_mangle]
pub unsafe extern "C" fn Instruction_With( Instruction : *mut Instruction, Code : u16 ) -> bool { // FFI-Unsafe: Code
  if Instruction.is_null() {
      return false;
  }

  (*Instruction) = Instruction::with( transmute( Code as u16 ) );
  return true;
}

// Creates an instruction with 1 operand
//
// # Errors
// Fails if one of the operands is invalid (basic checks)
#[no_mangle]
pub unsafe extern "C" fn Instruction_With1_Register( Instruction : *mut Instruction, Code : u16, Register : u8 ) -> bool { // FFI-Unsafe: Code, Register
  if Instruction.is_null() {
      return false;
  }

  let register: Register = transmute( Register );
  match Instruction::with1( transmute( Code as u16 ), register ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With1_i32( Instruction : *mut Instruction, Code : u16, Immediate : i32 ) -> bool { // FFI-Unsafe: Code
  if Instruction.is_null() {
      return false;
  }

  match Instruction::with1( transmute( Code as u16 ), Immediate ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With1_u32( Instruction : *mut Instruction, Code : u16, Immediate : u32 ) -> bool { // FFI-Unsafe: Code
  if Instruction.is_null() {
      return false;
  }

  match Instruction::with1( transmute( Code as u16 ), Immediate ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

//#[repr(C)]
#[repr(packed)]
pub struct TMemoryOperand {
	/// Segment override or [`Register::None`]
	///
	/// [`Register::None`]: enum.Register.html#variant.None
	pub segment_prefix: u8, // FFI-Unsafe: Register

	/// Base register or [`Register::None`]
	///
	/// [`Register::None`]: enum.Register.html#variant.None
	pub base: u8, // FFI-Unsafe: Register

	/// Index register or [`Register::None`]
	///
	/// [`Register::None`]: enum.Register.html#variant.None
	pub index: u8, // FFI-Unsafe: Register

	/// Index register scale (1, 2, 4, or 8)
	pub scale: u32,

	/// Memory displacement
	pub displacement: i64,

	/// 0 (no displ), 1 (16/32/64-bit, but use 2/4/8 if it doesn't fit in a `i8`), 2 (16-bit), 4 (32-bit) or 8 (64-bit)
	pub displ_size: u32,

	/// `true` if it's broadcast memory (EVEX instructions)
	pub is_broadcast: bool,
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With1_Memory( Instruction : *mut Instruction, Code : u16, Memory : *mut TMemoryOperand ) -> bool { // FFI-Unsafe: Code
  if Instruction.is_null() {
      return false;
  }

  let memory = MemoryOperand { 
    segment_prefix: transmute( (*Memory).segment_prefix ),
    base: transmute( (*Memory).base ),
    index: transmute( (*Memory).index ),
    scale: (*Memory).scale,
    displacement: (*Memory).displacement,
    displ_size: (*Memory).displ_size,
    is_broadcast: (*Memory).is_broadcast
   };

  match Instruction::with1( transmute( Code as u16 ), memory ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With2_Register_Register( Instruction : *mut Instruction, Code : u16, Register1 : u8, Register2 : u8 ) -> bool { // FFI-Unsafe: Code, Register
  if Instruction.is_null() {
      return false;
  }

  let register1: Register = transmute( Register1 );
  let register2: Register = transmute( Register2 );
  match Instruction::with2( transmute( Code as u16 ), register1, register2 ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With2_Register_i32( Instruction : *mut Instruction, Code : u16, Register : u8, Immediate : i32 ) -> bool { // FFI-Unsafe: Code, Register
  if Instruction.is_null() {
      return false;
  }

  let register: Register = transmute( Register );
  match Instruction::with2( transmute( Code as u16 ), register, Immediate ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With2_Register_u32( Instruction : *mut Instruction, Code : u16, Register : u8, Immediate : u32 ) -> bool { // FFI-Unsafe: Code, Register
  if Instruction.is_null() {
      return false;
  }

  let register: Register = transmute( Register );
  match Instruction::with2( transmute( Code as u16 ), register, Immediate ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With2_Register_i64( Instruction : *mut Instruction, Code : u16, Register : u8, Immediate : i64 ) -> bool { // FFI-Unsafe: Code, Register
  if Instruction.is_null() {
      return false;
  }

  let register: Register = transmute( Register );
  match Instruction::with2( transmute( Code as u16 ), register, Immediate ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With2_Register_u64( Instruction : *mut Instruction, Code : u16, Register : u8, Immediate : u64 ) -> bool { // FFI-Unsafe: Code, Register
  if Instruction.is_null() {
      return false;
  }

  let register: Register = transmute( Register );
  match Instruction::with2( transmute( Code as u16 ), register, Immediate ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With2_Register_MemoryOperand( Instruction : *mut Instruction, Code : u16, Register : u8, Memory : *mut TMemoryOperand ) -> bool { // FFI-Unsafe: Code, Register
  if Instruction.is_null() {
      return false;
  }

  let register: Register = transmute( Register as u8 );
  let memory = MemoryOperand { 
    segment_prefix: transmute( (*Memory).segment_prefix ),
    base: transmute( (*Memory).base ),
    index: transmute( (*Memory).index ),
    scale: (*Memory).scale,
    displacement: (*Memory).displacement,
    displ_size: (*Memory).displ_size,
    is_broadcast: (*Memory).is_broadcast
   };

  match Instruction::with2( transmute( Code as u16 ), register, memory ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With2_i32_Register( Instruction : *mut Instruction, Code : u16, Immediate : i32, Register : u8 ) -> bool { // FFI-Unsafe: Code, Register
    if Instruction.is_null() {
        return false;
    }
  
    let register: Register = transmute( Register );
    match Instruction::with2( transmute( Code as u16 ), Immediate, register ) {
      Err( _e ) => return false,
      Ok( instruction ) => { 
          (*Instruction) = instruction;
          return true
      }
    };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With2_u32_Register( Instruction : *mut Instruction, Code : u16, Immediate : u32, Register : u8 ) -> bool { // FFI-Unsafe: Code, Register
    if Instruction.is_null() {
        return false;
    }
  
    let register: Register = transmute( Register );
    match Instruction::with2( transmute( Code as u16 ), Immediate, register ) {
      Err( _e ) => return false,
      Ok( instruction ) => { 
          (*Instruction) = instruction;
          return true
      }
    };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With2_i32_i32( Instruction : *mut Instruction, Code : u16, Immediate1 : i32, Immediate2 : i32 ) -> bool { // FFI-Unsafe: Code
    if Instruction.is_null() {
        return false;
    }
  
    match Instruction::with2( transmute( Code as u16 ), Immediate1, Immediate2 ) {
      Err( _e ) => return false,
      Ok( instruction ) => { 
          (*Instruction) = instruction;
          return true
      }
    };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With2_u32_u32( Instruction : *mut Instruction, Code : u16, Immediate1 : u32, Immediate2 : u32 ) -> bool { // FFI-Unsafe: Code
    if Instruction.is_null() {
        return false;
    }
  
    match Instruction::with2( transmute( Code as u16 ), Immediate1, Immediate2 ) {
      Err( _e ) => return false,
      Ok( instruction ) => { 
          (*Instruction) = instruction;
          return true
      }
    };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With2_MemoryOperand_Register( Instruction : *mut Instruction, Code : u16, Memory : *mut TMemoryOperand, Register : u8 ) -> bool { // FFI-Unsafe: Code, Register
  if Instruction.is_null() {
      return false;
  }

  let memory = MemoryOperand { 
    segment_prefix: transmute( (*Memory).segment_prefix ),
    base: transmute( (*Memory).base ),
    index: transmute( (*Memory).index ),
    scale: (*Memory).scale,
    displacement: (*Memory).displacement,
    displ_size: (*Memory).displ_size,
    is_broadcast: (*Memory).is_broadcast
   };
  let register: Register = transmute( Register as u8 );

  match Instruction::with2( transmute( Code as u16 ), memory, register ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With2_MemoryOperand_i32( Instruction : *mut Instruction, Code : u16, Memory : *mut TMemoryOperand, Immediate : i32 ) -> bool { // FFI-Unsafe: Code
  if Instruction.is_null() {
      return false;
  }

  let memory = MemoryOperand { 
    segment_prefix: transmute( (*Memory).segment_prefix ),
    base: transmute( (*Memory).base ),
    index: transmute( (*Memory).index ),
    scale: (*Memory).scale,
    displacement: (*Memory).displacement,
    displ_size: (*Memory).displ_size,
    is_broadcast: (*Memory).is_broadcast
   };

  match Instruction::with2( transmute( Code as u16 ), memory, Immediate ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With2_MemoryOperand_u32( Instruction : *mut Instruction, Code : u16, Memory : *mut TMemoryOperand, Immediate : u32 ) -> bool { // FFI-Unsafe: Code
  if Instruction.is_null() {
      return false;
  }

  let memory = MemoryOperand { 
    segment_prefix: transmute( (*Memory).segment_prefix ),
    base: transmute( (*Memory).base ),
    index: transmute( (*Memory).index ),
    scale: (*Memory).scale,
    displacement: (*Memory).displacement,
    displ_size: (*Memory).displ_size,
    is_broadcast: (*Memory).is_broadcast
   };

  match Instruction::with2( transmute( Code as u16 ), memory, Immediate ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With3_Register_Register_Register( Instruction : *mut Instruction, Code : u16, Register1 : u8, Register2 : u8, Register3 : u8 ) -> bool { // FFI-Unsafe: Code, Register
  if Instruction.is_null() {
      return false;
  }

  let register1: Register = transmute( Register1 );
  let register2: Register = transmute( Register2 );
  let register3: Register = transmute( Register3 );
  
  match Instruction::with3( transmute( Code as u16 ), register1, register2, register3 ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With3_Register_Register_i32( Instruction : *mut Instruction, Code : u16, Register1 : u8, Register2 : u8, Immediate : i32 ) -> bool { // FFI-Unsafe: Code, Register
  if Instruction.is_null() {
      return false;
  }

  let register1: Register = transmute( Register1 );
  let register2: Register = transmute( Register2 );
  
  match Instruction::with3( transmute( Code as u16 ), register1, register2, Immediate ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With3_Register_Register_u32( Instruction : *mut Instruction, Code : u16, Register1 : u8, Register2 : u8, Immediate : u32 ) -> bool { // FFI-Unsafe: Code, Register
  if Instruction.is_null() {
      return false;
  }

  let register1: Register = transmute( Register1 );
  let register2: Register = transmute( Register2 );
  
  match Instruction::with3( transmute( Code as u16 ), register1, register2, Immediate ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With3_Register_Register_MemoryOperand( Instruction : *mut Instruction, Code : u16, Register1 : u8, Register2 : u8, Memory : *mut TMemoryOperand ) -> bool { // FFI-Unsafe: Code, Register
  if Instruction.is_null() {
      return false;
  }

  let register1: Register = transmute( Register1 );
  let register2: Register = transmute( Register2 );
  let memory = MemoryOperand { 
    segment_prefix: transmute( (*Memory).segment_prefix ),
    base: transmute( (*Memory).base ),
    index: transmute( (*Memory).index ),
    scale: (*Memory).scale,
    displacement: (*Memory).displacement,
    displ_size: (*Memory).displ_size,
    is_broadcast: (*Memory).is_broadcast
   };
  
  match Instruction::with3( transmute( Code as u16 ), register1, register2, memory ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With3_Register_i32_i32( Instruction : *mut Instruction, Code : u16, Register : u8, Immediate1 : i32, Immediate2 : i32 ) -> bool { // FFI-Unsafe: Code, Register
  if Instruction.is_null() {
      return false;
  }

  let register: Register = transmute( Register );

  match Instruction::with3( transmute( Code as u16 ), register, Immediate1, Immediate2 ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With3_Register_u32_u32( Instruction : *mut Instruction, Code : u16, Register : u8, Immediate1 : u32, Immediate2 : u32 ) -> bool { // FFI-Unsafe: Code, Register
  if Instruction.is_null() {
      return false;
  }

  let register: Register = transmute( Register as u8 );
    
  match Instruction::with3( transmute( Code as u16 ), register, Immediate1, Immediate2 ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With3_Register_MemoryOperand_Register( Instruction : *mut Instruction, Code : u16, Register1 : u8, Memory : *mut TMemoryOperand, Register2 : u8 ) -> bool { // FFI-Unsafe: Code, Register
  if Instruction.is_null() {
      return false;
  }

  let register1: Register = transmute( Register1 );
  let memory = MemoryOperand { 
    segment_prefix: transmute( (*Memory).segment_prefix ),
    base: transmute( (*Memory).base ),
    index: transmute( (*Memory).index ),
    scale: (*Memory).scale,
    displacement: (*Memory).displacement,
    displ_size: (*Memory).displ_size,
    is_broadcast: (*Memory).is_broadcast
   };
  let register2: Register = transmute( Register2 );
    
  match Instruction::with3( transmute( Code as u16 ), register1, memory, register2 ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With3_Register_MemoryOperand_i32( Instruction : *mut Instruction, Code : u16, Register1 : u8, Memory : *mut TMemoryOperand, Immediate : i32 ) -> bool { // FFI-Unsafe: Code, Register
  if Instruction.is_null() {
      return false;
  }

  let register1: Register = transmute( Register1 );
  let memory = MemoryOperand { 
    segment_prefix: transmute( (*Memory).segment_prefix ),
    base: transmute( (*Memory).base ),
    index: transmute( (*Memory).index ),
    scale: (*Memory).scale,
    displacement: (*Memory).displacement,
    displ_size: (*Memory).displ_size,
    is_broadcast: (*Memory).is_broadcast
   };
    
  match Instruction::with3( transmute( Code as u16 ), register1, memory, Immediate ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With3_Register_MemoryOperand_u32( Instruction : *mut Instruction, Code : u16, Register : u8, Memory : *mut TMemoryOperand, Immediate : u32 ) -> bool { // FFI-Unsafe: Code, Register
  if Instruction.is_null() {
      return false;
  }

  let register: Register = transmute( Register as u8 );
  let memory = MemoryOperand { 
    segment_prefix: transmute( (*Memory).segment_prefix ),
    base: transmute( (*Memory).base ),
    index: transmute( (*Memory).index ),
    scale: (*Memory).scale,
    displacement: (*Memory).displacement,
    displ_size: (*Memory).displ_size,
    is_broadcast: (*Memory).is_broadcast
   };
    
  match Instruction::with3( transmute( Code as u16 ), register, memory, Immediate ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With3_MemoryOperand_Register_Register( Instruction : *mut Instruction, Code : u16, Memory : *mut TMemoryOperand, Register1 : u8, Register2 : u8 ) -> bool { // FFI-Unsafe: Code, Register
  if Instruction.is_null() {
      return false;
  }

  let memory = MemoryOperand { 
    segment_prefix: transmute( (*Memory).segment_prefix ),
    base: transmute( (*Memory).base ),
    index: transmute( (*Memory).index ),
    scale: (*Memory).scale,
    displacement: (*Memory).displacement,
    displ_size: (*Memory).displ_size,
    is_broadcast: (*Memory).is_broadcast
   };
   let register1: Register = transmute( Register1 );
   let register2: Register = transmute( Register2 );
    
  match Instruction::with3( transmute( Code as u16 ), memory, register1, register2 ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With3_MemoryOperand_Register_i32( Instruction : *mut Instruction, Code : u16, Memory : *mut TMemoryOperand, Register : u8, Immediate : i32 ) -> bool { // FFI-Unsafe: Code, Register
  if Instruction.is_null() {
      return false;
  }

  let memory = MemoryOperand { 
    segment_prefix: transmute( (*Memory).segment_prefix ),
    base: transmute( (*Memory).base ),
    index: transmute( (*Memory).index ),
    scale: (*Memory).scale,
    displacement: (*Memory).displacement,
    displ_size: (*Memory).displ_size,
    is_broadcast: (*Memory).is_broadcast
   };
   let register: Register = transmute( Register );
    
  match Instruction::with3( transmute( Code as u16 ), memory, register, Immediate ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With3_MemoryOperand_Register_u32( Instruction : *mut Instruction, Code : u16, Memory : *mut TMemoryOperand, Register : u8, Immediate : u32 ) -> bool { // FFI-Unsafe: Code, Register
  if Instruction.is_null() {
      return false;
  }

  let memory = MemoryOperand { 
    segment_prefix: transmute( (*Memory).segment_prefix ),
    base: transmute( (*Memory).base ),
    index: transmute( (*Memory).index ),
    scale: (*Memory).scale,
    displacement: (*Memory).displacement,
    displ_size: (*Memory).displ_size,
    is_broadcast: (*Memory).is_broadcast
   };
   let register: Register = transmute( Register );
    
  match Instruction::with3( transmute( Code as u16 ), memory, register, Immediate ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With4_Register_Register_Register_Register( Instruction : *mut Instruction, Code : u16, Register1 : u8, Register2 : u8, Register3 : u8, Register4 : u8 ) -> bool { // FFI-Unsafe: Code, Register
  if Instruction.is_null() {
      return false;
  }

  let register1: Register = transmute( Register1 );
  let register2: Register = transmute( Register2 );
  let register3: Register = transmute( Register3 );
  let register4: Register = transmute( Register4 );
    
  match Instruction::with4( transmute( Code as u16 ), register1, register2, register3, register4 ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With4_Register_Register_Register_i32( Instruction : *mut Instruction, Code : u16, Register1 : u8, Register2 : u8, Register3 : u8, Immediate : i32 ) -> bool { // FFI-Unsafe: Code, Register
  if Instruction.is_null() {
      return false;
  }

  let register1: Register = transmute( Register1 );
  let register2: Register = transmute( Register2 );
  let register3: Register = transmute( Register3 );
    
  match Instruction::with4( transmute( Code as u16 ), register1, register2, register3, Immediate ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With4_Register_Register_Register_u32( Instruction : *mut Instruction, Code : u16, Register1 : u8, Register2 : u8, Register3 : u8, Immediate : u32 ) -> bool { // FFI-Unsafe: Code, Register
  if Instruction.is_null() {
      return false;
  }

  let register1: Register = transmute( Register1 );
  let register2: Register = transmute( Register2 );
  let register3: Register = transmute( Register3 );
    
  match Instruction::with4( transmute( Code as u16 ), register1, register2, register3, Immediate ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With4_Register_Register_Register_MemoryOperand( Instruction : *mut Instruction, Code : u16, Register1 : u8, Register2 : u8, Register3 : u8, Memory : *mut TMemoryOperand ) -> bool { // FFI-Unsafe: Code, Register
  if Instruction.is_null() {
      return false;
  }

  let register1: Register = transmute( Register1 );
  let register2: Register = transmute( Register2 );
  let register3: Register = transmute( Register3 );
  let memory = MemoryOperand { 
    segment_prefix: transmute( (*Memory).segment_prefix ),
    base: transmute( (*Memory).base ),
    index: transmute( (*Memory).index ),
    scale: (*Memory).scale,
    displacement: (*Memory).displacement,
    displ_size: (*Memory).displ_size,
    is_broadcast: (*Memory).is_broadcast
   };
    
  match Instruction::with4( transmute( Code as u16 ), register1, register2, register3, memory ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With4_Register_Register_i32_i32( Instruction : *mut Instruction, Code : u16, Register1 : u8, Register2 : u8, Immediate1 : i32, Immediate2 : i32 ) -> bool { // FFI-Unsafe: Code, Register
  if Instruction.is_null() {
      return false;
  }

  let register1: Register = transmute( Register1 );
  let register2: Register = transmute( Register2 );
    
  match Instruction::with4( transmute( Code as u16 ), register1, register2, Immediate1, Immediate2 ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With4_Register_Register_u32_u32( Instruction : *mut Instruction, Code : u16, Register1 : u8, Register2 : u8, Immediate1 : u32, Immediate2 : u32 ) -> bool { // FFI-Unsafe: Code, Register
  if Instruction.is_null() {
      return false;
  }

  let register1: Register = transmute( Register1 );
  let register2: Register = transmute( Register2 );
    
  match Instruction::with4( transmute( Code as u16 ), register1, register2, Immediate1, Immediate2 ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With4_Register_Register_MemoryOperand_Register( Instruction : *mut Instruction, Code : u16, Register1 : u8, Register2 : u8, Memory : *mut TMemoryOperand, Register3 : u8 ) -> bool { // FFI-Unsafe: Code, Register
  if Instruction.is_null() {
      return false;
  }

  let register1: Register = transmute( Register1 );
  let register2: Register = transmute( Register2 );
  let memory = MemoryOperand { 
    segment_prefix: transmute( (*Memory).segment_prefix ),
    base: transmute( (*Memory).base ),
    index: transmute( (*Memory).index ),
    scale: (*Memory).scale,
    displacement: (*Memory).displacement,
    displ_size: (*Memory).displ_size,
    is_broadcast: (*Memory).is_broadcast
   };
   let register3: Register = transmute( Register3 );
    
  match Instruction::with4( transmute( Code as u16 ), register1, register2, memory, register3 ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With4_Register_Register_MemoryOperand_i32( Instruction : *mut Instruction, Code : u16, Register1 : u8, Register2 : u8, Memory : *mut TMemoryOperand, Immediate : i32 ) -> bool { // FFI-Unsafe: Code, Register
  if Instruction.is_null() {
      return false;
  }

  let register1: Register = transmute( Register1 );
  let register2: Register = transmute( Register2 );
  let memory = MemoryOperand { 
    segment_prefix: transmute( (*Memory).segment_prefix ),
    base: transmute( (*Memory).base ),
    index: transmute( (*Memory).index ),
    scale: (*Memory).scale,
    displacement: (*Memory).displacement,
    displ_size: (*Memory).displ_size,
    is_broadcast: (*Memory).is_broadcast
   };
    
  match Instruction::with4( transmute( Code as u16 ), register1, register2, memory, Immediate ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With4_Register_Register_MemoryOperand_u32( Instruction : *mut Instruction, Code : u16, Register1 : u8, Register2 : u8, Memory : *mut TMemoryOperand, Immediate : u32 ) -> bool { // FFI-Unsafe: Code, Register
  if Instruction.is_null() {
      return false;
  }

  let register1: Register = transmute( Register1 );
  let register2: Register = transmute( Register2 );
  let memory = MemoryOperand { 
    segment_prefix: transmute( (*Memory).segment_prefix ),
    base: transmute( (*Memory).base ),
    index: transmute( (*Memory).index ),
    scale: (*Memory).scale,
    displacement: (*Memory).displacement,
    displ_size: (*Memory).displ_size,
    is_broadcast: (*Memory).is_broadcast
   };
    
  match Instruction::with4( transmute( Code as u16 ), register1, register2, memory, Immediate ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With5_Register_Register_Register_Register_i32( Instruction : *mut Instruction, Code : u16, Register1 : u8, Register2 : u8, Register3 : u8, Register4 : u8, Immediate : i32 ) -> bool { // FFI-Unsafe: Code, Register
  if Instruction.is_null() {
      return false;
  }

  let register1: Register = transmute( Register1 );
  let register2: Register = transmute( Register2 );
  let register3: Register = transmute( Register3 );
  let register4: Register = transmute( Register4 );

  match Instruction::with5( transmute( Code as u16 ), register1, register2, register3, register4, Immediate ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With5_Register_Register_Register_Register_u32( Instruction : *mut Instruction, Code : u16, Register1 : u8, Register2 : u8, Register3 : u8, Register4 : u8, Immediate : u32 ) -> bool { // FFI-Unsafe: Code, Register
  if Instruction.is_null() {
      return false;
  }

  let register1: Register = transmute( Register1 );
  let register2: Register = transmute( Register2 );
  let register3: Register = transmute( Register3 );
  let register4: Register = transmute( Register4 );

  match Instruction::with5( transmute( Code as u16 ), register1, register2, register3, register4, Immediate ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With5_Register_Register_Register_MemoryOperand_i32( Instruction : *mut Instruction, Code : u16, Register1 : u8, Register2 : u8, Register3 : u8, Memory : *mut TMemoryOperand, Immediate : i32 ) -> bool { // FFI-Unsafe: Code, Register
  if Instruction.is_null() {
      return false;
  }

  let register1: Register = transmute( Register1 );
  let register2: Register = transmute( Register2 );
  let register3: Register = transmute( Register3 );
  let memory = MemoryOperand { 
    segment_prefix: transmute( (*Memory).segment_prefix ),
    base: transmute( (*Memory).base ),
    index: transmute( (*Memory).index ),
    scale: (*Memory).scale,
    displacement: (*Memory).displacement,
    displ_size: (*Memory).displ_size,
    is_broadcast: (*Memory).is_broadcast
   };

  match Instruction::with5( transmute( Code as u16 ), register1, register2, register3, memory, Immediate ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With5_Register_Register_Register_MemoryOperand_u32( Instruction : *mut Instruction, Code : u16, Register1 : u8, Register2 : u8, Register3 : u8, Memory : *mut TMemoryOperand, Immediate : u32 ) -> bool { // FFI-Unsafe: Code, Register
  if Instruction.is_null() {
      return false;
  }

  let register1: Register = transmute( Register1 );
  let register2: Register = transmute( Register2 );
  let register3: Register = transmute( Register3 );
  let memory = MemoryOperand { 
    segment_prefix: transmute( (*Memory).segment_prefix ),
    base: transmute( (*Memory).base ),
    index: transmute( (*Memory).index ),
    scale: (*Memory).scale,
    displacement: (*Memory).displacement,
    displ_size: (*Memory).displ_size,
    is_broadcast: (*Memory).is_broadcast
   };

  match Instruction::with5( transmute( Code as u16 ), register1, register2, register3, memory, Immediate ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With5_Register_Register_MemoryOperand_Register_i32( Instruction : *mut Instruction, Code : u16, Register1 : u8, Register2 : u8, Memory : *mut TMemoryOperand, Register3 : u8, Immediate : i32 ) -> bool { // FFI-Unsafe: Code, Register
  if Instruction.is_null() {
      return false;
  }

  let register1: Register = transmute( Register1 );
  let register2: Register = transmute( Register2 );
  let memory = MemoryOperand { 
    segment_prefix: transmute( (*Memory).segment_prefix ),
    base: transmute( (*Memory).base ),
    index: transmute( (*Memory).index ),
    scale: (*Memory).scale,
    displacement: (*Memory).displacement,
    displ_size: (*Memory).displ_size,
    is_broadcast: (*Memory).is_broadcast
   };
   let register3: Register = transmute( Register3 );

  match Instruction::with5( transmute( Code as u16 ), register1, register2, memory, register3, Immediate ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With5_Register_Register_MemoryOperand_Register_u32( Instruction : *mut Instruction, Code : u16, Register1 : u8, Register2 : u8, Memory : *mut TMemoryOperand, Register3 : u8, Immediate : u32 ) -> bool { // FFI-Unsafe: Code, Register
  if Instruction.is_null() {
      return false;
  }

  let register1: Register = transmute( Register1 );
  let register2: Register = transmute( Register2 );
  let memory = MemoryOperand { 
    segment_prefix: transmute( (*Memory).segment_prefix ),
    base: transmute( (*Memory).base ),
    index: transmute( (*Memory).index ),
    scale: (*Memory).scale,
    displacement: (*Memory).displacement,
    displ_size: (*Memory).displ_size,
    is_broadcast: (*Memory).is_broadcast
   };
   let register3: Register = transmute( Register3 );

  match Instruction::with5( transmute( Code as u16 ), register1, register2, memory, register3, Immediate ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Branch( Instruction : *mut Instruction, Code : u16, Target : u64 ) -> bool { // FFI-Unsafe: Code
  if Instruction.is_null() {
      return false;
  }

  match Instruction::with_branch( transmute( Code as u16 ), Target ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Far_Branch( Instruction : *mut Instruction, Code : u16, Selector : u16, Offset : u32 ) -> bool { // FFI-Unsafe: Code
  if Instruction.is_null() {
      return false;
  }

  match Instruction::with_far_branch( transmute( Code as u16 ), Selector, Offset ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_xbegin( Instruction : *mut Instruction, Bitness : u32, Target : u64 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::with_xbegin( Bitness, Target ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_outsb( Instruction : *mut Instruction, AddressSize: u32, SegmentPrefix: u32, RepPrefix: u32 ) -> bool { // FFI-Unsafe: Register, RepPrefixKind
  if Instruction.is_null() {
      return false;
  }

  let segmentprefix: Register = transmute( SegmentPrefix as u8 );
  let repprefix: RepPrefixKind = transmute( RepPrefix as u8 );

  match Instruction::with_outsb( AddressSize, segmentprefix, repprefix ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Rep_outsb( Instruction : *mut Instruction, AddressSize: u32 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::with_rep_outsb( AddressSize ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_outsw( Instruction : *mut Instruction, AddressSize: u32, SegmentPrefix: u32, RepPrefix: u32 ) -> bool { // FFI-Unsafe: Register, RepPrefixKind
  if Instruction.is_null() {
      return false;
  }

  let segmentprefix: Register = transmute( SegmentPrefix as u8 );
  let repprefix: RepPrefixKind = transmute( RepPrefix as u8 );

  match Instruction::with_outsw( AddressSize, segmentprefix, repprefix ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Rep_outsw( Instruction : *mut Instruction, AddressSize: u32 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::with_rep_outsw( AddressSize ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_outsd( Instruction : *mut Instruction, AddressSize: u32, SegmentPrefix: u32, RepPrefix: u32 ) -> bool { // FFI-Unsafe: Register, RepPrefixKind
  if Instruction.is_null() {
      return false;
  }

  let segmentprefix: Register = transmute( SegmentPrefix as u8 );
  let repprefix: RepPrefixKind = transmute( RepPrefix as u8 );

  match Instruction::with_outsd( AddressSize, segmentprefix, repprefix ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Rep_outsd( Instruction : *mut Instruction, AddressSize: u32 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::with_rep_outsd( AddressSize ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_lodsb( Instruction : *mut Instruction, AddressSize: u32, SegmentPrefix: u32, RepPrefix: u32 ) -> bool { // FFI-Unsafe: Register, RepPrefixKind
  if Instruction.is_null() {
      return false;
  }

  let segmentprefix: Register = transmute( SegmentPrefix as u8 );
  let repprefix: RepPrefixKind = transmute( RepPrefix as u8 );

  match Instruction::with_lodsb( AddressSize, segmentprefix, repprefix ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Rep_lodsb( Instruction : *mut Instruction, AddressSize: u32 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::with_rep_lodsb( AddressSize ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_lodsw( Instruction : *mut Instruction, AddressSize: u32, SegmentPrefix: u32, RepPrefix: u32 ) -> bool { // FFI-Unsafe: Register, RepPrefixKind
  if Instruction.is_null() {
      return false;
  }

  let segmentprefix: Register = transmute( SegmentPrefix as u8 );
  let repprefix: RepPrefixKind = transmute( RepPrefix as u8 );

  match Instruction::with_lodsw( AddressSize, segmentprefix, repprefix ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Rep_lodsw( Instruction : *mut Instruction, AddressSize: u32 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::with_rep_lodsw( AddressSize ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_lodsd( Instruction : *mut Instruction, AddressSize: u32, SegmentPrefix: u32, RepPrefix: u32 ) -> bool { // FFI-Unsafe: Register, RepPrefixKind
  if Instruction.is_null() {
      return false;
  }

  let segmentprefix: Register = transmute( SegmentPrefix as u8 );
  let repprefix: RepPrefixKind = transmute( RepPrefix as u8 );

  match Instruction::with_lodsd( AddressSize, segmentprefix, repprefix ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Rep_lodsd( Instruction : *mut Instruction, AddressSize: u32 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::with_rep_lodsd( AddressSize ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_lodsq( Instruction : *mut Instruction, AddressSize: u32, SegmentPrefix: u32, RepPrefix: u32 ) -> bool { // FFI-Unsafe: Register, RepPrefixKind
  if Instruction.is_null() {
      return false;
  }

  let segmentprefix: Register = transmute( SegmentPrefix as u8 );
  let repprefix: RepPrefixKind = transmute( RepPrefix as u8 );

  match Instruction::with_lodsq( AddressSize, segmentprefix, repprefix ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Rep_lodsq( Instruction : *mut Instruction, AddressSize: u32 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::with_rep_lodsq( AddressSize ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_scasb( Instruction : *mut Instruction, AddressSize: u32, RepPrefix: u32 ) -> bool { // FFI-Unsafe: RepPrefixKind
  if Instruction.is_null() {
      return false;
  }

  let repprefix: RepPrefixKind = transmute( RepPrefix as u8 );

  match Instruction::with_scasb( AddressSize, repprefix ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Repe_scasb( Instruction : *mut Instruction, AddressSize: u32 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::with_repe_scasb( AddressSize ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Repne_scasb( Instruction : *mut Instruction, AddressSize: u32 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::with_repne_scasb( AddressSize ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_scasw( Instruction : *mut Instruction, AddressSize: u32, RepPrefix: u32 ) -> bool { // FFI-Unsafe: RepPrefixKind
  if Instruction.is_null() {
      return false;
  }

  let repprefix: RepPrefixKind = transmute( RepPrefix as u8 );

  match Instruction::with_scasw( AddressSize, repprefix ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Repe_scasw( Instruction : *mut Instruction, AddressSize: u32 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::with_repe_scasw( AddressSize ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Repne_scasw( Instruction : *mut Instruction, AddressSize: u32 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::with_repne_scasw( AddressSize ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_scasd( Instruction : *mut Instruction, AddressSize: u32, RepPrefix: u32 ) -> bool { // FFI-Unsafe: RepPrefixKind
  if Instruction.is_null() {
      return false;
  }

  let repprefix: RepPrefixKind = transmute( RepPrefix as u8 );

  match Instruction::with_scasd( AddressSize, repprefix ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Repe_scasd( Instruction : *mut Instruction, AddressSize: u32 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::with_repe_scasd( AddressSize ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Repne_scasd( Instruction : *mut Instruction, AddressSize: u32 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::with_repne_scasd( AddressSize ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_scasq( Instruction : *mut Instruction, AddressSize: u32, RepPrefix: u32 ) -> bool { // FFI-Unsafe: RepPrefixKind
  if Instruction.is_null() {
      return false;
  }

  let repprefix: RepPrefixKind = transmute( RepPrefix as u8 );

  match Instruction::with_scasq( AddressSize, repprefix ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Repe_scasq( Instruction : *mut Instruction, AddressSize: u32 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::with_repe_scasq( AddressSize ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Repne_scasq( Instruction : *mut Instruction, AddressSize: u32 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::with_repne_scasq( AddressSize ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_insb( Instruction : *mut Instruction, AddressSize: u32, RepPrefix: u32 ) -> bool { // FFI-Unsafe: RepPrefixKind
  if Instruction.is_null() {
      return false;
  }

  let repprefix: RepPrefixKind = transmute( RepPrefix as u8 );

  match Instruction::with_insb( AddressSize, repprefix ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Rep_insb( Instruction : *mut Instruction, AddressSize: u32 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::with_rep_insb( AddressSize ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_insw( Instruction : *mut Instruction, AddressSize: u32, RepPrefix: u32 ) -> bool { // FFI-Unsafe: RepPrefixKind
  if Instruction.is_null() {
      return false;
  }

  let repprefix: RepPrefixKind = transmute( RepPrefix as u8 );

  match Instruction::with_insw( AddressSize, repprefix ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Rep_insw( Instruction : *mut Instruction, AddressSize: u32 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::with_rep_insw( AddressSize ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_insd( Instruction : *mut Instruction, AddressSize: u32, RepPrefix: u32 ) -> bool { // FFI-Unsafe: RepPrefixKind
  if Instruction.is_null() {
      return false;
  }

  let repprefix: RepPrefixKind = transmute( RepPrefix as u8 );

  match Instruction::with_insd( AddressSize, repprefix ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Rep_insd( Instruction : *mut Instruction, AddressSize: u32 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::with_rep_insd( AddressSize ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_stosb( Instruction : *mut Instruction, AddressSize: u32, RepPrefix: u32 ) -> bool { // FFI-Unsafe: RepPrefixKind
  if Instruction.is_null() {
      return false;
  }

  let repprefix: RepPrefixKind = transmute( RepPrefix as u8 );

  match Instruction::with_stosb( AddressSize, repprefix ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Rep_stosb( Instruction : *mut Instruction, AddressSize: u32 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::with_rep_stosb( AddressSize ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_stosw( Instruction : *mut Instruction, AddressSize: u32, RepPrefix: u32 ) -> bool { // FFI-Unsafe: RepPrefixKind
  if Instruction.is_null() {
      return false;
  }

  let repprefix: RepPrefixKind = transmute( RepPrefix as u8 );

  match Instruction::with_stosw( AddressSize, repprefix ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Rep_stosw( Instruction : *mut Instruction, AddressSize: u32 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::with_rep_stosw( AddressSize ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_stosd( Instruction : *mut Instruction, AddressSize: u32, RepPrefix: u32 ) -> bool { // FFI-Unsafe: RepPrefixKind
  if Instruction.is_null() {
      return false;
  }

  let repprefix: RepPrefixKind = transmute( RepPrefix as u8 );

  match Instruction::with_stosd( AddressSize, repprefix ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Rep_stosd( Instruction : *mut Instruction, AddressSize: u32 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::with_rep_stosd( AddressSize ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Rep_stosq( Instruction : *mut Instruction, AddressSize: u32 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::with_rep_stosq( AddressSize ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_cmpsb( Instruction : *mut Instruction, AddressSize: u32, SegmentPrefix : u32, RepPrefix: u32 ) -> bool { // FFI-Unsafe: Register, RepPrefixKind
  if Instruction.is_null() {
      return false;
  }

  let segmentprefix: Register = transmute( SegmentPrefix as u8 );
  let repprefix: RepPrefixKind = transmute( RepPrefix as u8 );

  match Instruction::with_cmpsb( AddressSize, segmentprefix, repprefix ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Repe_cmpsb( Instruction : *mut Instruction, AddressSize: u32 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::with_repe_cmpsb( AddressSize ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Repne_cmpsb( Instruction : *mut Instruction, AddressSize: u32 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::with_repne_cmpsb( AddressSize ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_cmpsw( Instruction : *mut Instruction, AddressSize: u32, SegmentPrefix : u32, RepPrefix: u32 ) -> bool { // FFI-Unsafe: Register, RepPrefixKind
  if Instruction.is_null() {
      return false;
  }

  let segmentprefix: Register = transmute( SegmentPrefix as u8 );
  let repprefix: RepPrefixKind = transmute( RepPrefix as u8 );

  match Instruction::with_cmpsw( AddressSize, segmentprefix, repprefix ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Repe_cmpsw( Instruction : *mut Instruction, AddressSize: u32 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::with_repe_cmpsw( AddressSize ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Repne_cmpsw( Instruction : *mut Instruction, AddressSize: u32 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::with_repne_cmpsw( AddressSize ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_cmpsd( Instruction : *mut Instruction, AddressSize: u32, SegmentPrefix : u32, RepPrefix: u32 ) -> bool { // FFI-Unsafe: Register, RepPrefixKind
  if Instruction.is_null() {
      return false;
  }

  let segmentprefix: Register = transmute( SegmentPrefix as u8 );
  let repprefix: RepPrefixKind = transmute( RepPrefix as u8 );

  match Instruction::with_cmpsd( AddressSize, segmentprefix, repprefix ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Repe_cmpsd( Instruction : *mut Instruction, AddressSize: u32 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::with_repe_cmpsd( AddressSize ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Repne_cmpsd( Instruction : *mut Instruction, AddressSize: u32 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::with_repne_cmpsd( AddressSize ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_cmpsq( Instruction : *mut Instruction, AddressSize: u32, SegmentPrefix : u32, RepPrefix: u32 ) -> bool { // FFI-Unsafe: Register, RepPrefixKind
  if Instruction.is_null() {
      return false;
  }

  let segmentprefix: Register = transmute( SegmentPrefix as u8 );
  let repprefix: RepPrefixKind = transmute( RepPrefix as u8 );

  match Instruction::with_cmpsq( AddressSize, segmentprefix, repprefix ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Repe_cmpsq( Instruction : *mut Instruction, AddressSize: u32 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::with_repe_cmpsq( AddressSize ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Repne_cmpsq( Instruction : *mut Instruction, AddressSize: u32 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::with_repne_cmpsq( AddressSize ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_movsb( Instruction : *mut Instruction, AddressSize: u32, SegmentPrefix : u32, RepPrefix: u32 ) -> bool { // FFI-Unsafe: Register, RepPrefixKind
  if Instruction.is_null() {
      return false;
  }

  let segmentprefix: Register = transmute( SegmentPrefix as u8 );
  let repprefix: RepPrefixKind = transmute( RepPrefix as u8 );

  match Instruction::with_movsb( AddressSize, segmentprefix, repprefix ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Rep_movsb( Instruction : *mut Instruction, AddressSize: u32 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::with_rep_movsb( AddressSize ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_movsw( Instruction : *mut Instruction, AddressSize: u32, SegmentPrefix : u32, RepPrefix: u32 ) -> bool { // FFI-Unsafe: Register, RepPrefixKind
  if Instruction.is_null() {
      return false;
  }

  let segmentprefix: Register = transmute( SegmentPrefix as u8 );
  let repprefix: RepPrefixKind = transmute( RepPrefix as u8 );

  match Instruction::with_movsw( AddressSize, segmentprefix, repprefix ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Rep_movsw( Instruction : *mut Instruction, AddressSize: u32 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::with_rep_movsw( AddressSize ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_movsd( Instruction : *mut Instruction, AddressSize: u32, SegmentPrefix : u32, RepPrefix: u32 ) -> bool { // FFI-Unsafe: Register, RepPrefixKind
  if Instruction.is_null() {
      return false;
  }

  let segmentprefix: Register = transmute( SegmentPrefix as u8 );
  let repprefix: RepPrefixKind = transmute( RepPrefix as u8 );

  match Instruction::with_movsd( AddressSize, segmentprefix, repprefix ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Rep_movsd( Instruction : *mut Instruction, AddressSize: u32 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::with_rep_movsd( AddressSize ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_movsq( Instruction : *mut Instruction, AddressSize: u32, SegmentPrefix : u32, RepPrefix: u32 ) -> bool { // FFI-Unsafe: Register, RepPrefixKind
  if Instruction.is_null() {
      return false;
  }

  let segmentprefix: Register = transmute( SegmentPrefix as u8 );
  let repprefix: RepPrefixKind = transmute( RepPrefix as u8 );

  match Instruction::with_movsq( AddressSize, segmentprefix, repprefix ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Rep_movsq( Instruction : *mut Instruction, AddressSize: u32 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::with_rep_movsq( AddressSize ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_maskmovq( Instruction : *mut Instruction, AddressSize: u32, Register1 : u8, Register2 : u8, SegmentPrefix : u32 ) -> bool { // FFI-Unsafe: Register
  if Instruction.is_null() {
      return false;
  }

  let register1: Register = transmute( Register1 );
  let register2: Register = transmute( Register2 );
  let segmentprefix: Register = transmute( SegmentPrefix as u8 );

  match Instruction::with_maskmovq( AddressSize, register1, register2, segmentprefix ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_maskmovdqu( Instruction : *mut Instruction, AddressSize: u32, Register1 : u8, Register2 : u8, SegmentPrefix : u32 ) -> bool { // FFI-Unsafe: Register
  if Instruction.is_null() {
      return false;
  }

  let register1: Register = transmute( Register1 );
  let register2: Register = transmute( Register2 );
  let segmentprefix: Register = transmute( SegmentPrefix as u8 );

  match Instruction::with_maskmovdqu( AddressSize, register1, register2, segmentprefix ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_vmaskmovdqu( Instruction : *mut Instruction, AddressSize: u32, Register1 : u8, Register2 : u8, SegmentPrefix : u32 ) -> bool { // FFI-Unsafe: Register
  if Instruction.is_null() {
      return false;
  }

  let register1: Register = transmute( Register1 );
  let register2: Register = transmute( Register2 );
  let segmentprefix: Register = transmute( SegmentPrefix as u8 );

  match Instruction::with_vmaskmovdqu( AddressSize, register1, register2, segmentprefix ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Declare_Byte_1( Instruction : *mut Instruction, B0 : u8 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::try_with_declare_byte_1( B0 ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Declare_Byte_2( Instruction : *mut Instruction, B0 : u8, B1 : u8 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::try_with_declare_byte_2( B0, B1 ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Declare_Byte_3( Instruction : *mut Instruction, B0 : u8, B1 : u8, B2 : u8 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::try_with_declare_byte_3( B0, B1, B2 ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Declare_Byte_4( Instruction : *mut Instruction, B0 : u8, B1 : u8, B2 : u8, B3 : u8 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::try_with_declare_byte_4( B0, B1, B2, B3 ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Declare_Byte_5( Instruction : *mut Instruction, B0 : u8, B1 : u8, B2 : u8, B3 : u8, B4 : u8 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::try_with_declare_byte_5( B0, B1, B2, B3, B4 ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Declare_Byte_6( Instruction : *mut Instruction, B0 : u8, B1 : u8, B2 : u8, B3 : u8, B4 : u8, B5 : u8 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::try_with_declare_byte_6( B0, B1, B2, B3, B4, B5 ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Declare_Byte_7( Instruction : *mut Instruction, B0 : u8, B1 : u8, B2 : u8, B3 : u8, B4 : u8, B5 : u8, B6 : u8 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::try_with_declare_byte_7( B0, B1, B2, B3, B4, B5, B6 ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Declare_Byte_8( Instruction : *mut Instruction, B0 : u8, B1 : u8, B2 : u8, B3 : u8, B4 : u8, B5 : u8, B6 : u8, B7 : u8 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::try_with_declare_byte_8( B0, B1, B2, B3, B4, B5, B6, B7 ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Declare_Byte_9( Instruction : *mut Instruction, B0 : u8, B1 : u8, B2 : u8, B3 : u8, B4 : u8, B5 : u8, B6 : u8, B7 : u8, B8 : u8 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::try_with_declare_byte_9( B0, B1, B2, B3, B4, B5, B6, B7, B8 ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Declare_Byte_10( Instruction : *mut Instruction, B0 : u8, B1 : u8, B2 : u8, B3 : u8, B4 : u8, B5 : u8, B6 : u8, B7 : u8, B8 : u8, B9 : u8 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::try_with_declare_byte_10( B0, B1, B2, B3, B4, B5, B6, B7, B8, B9 ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Declare_Byte_11( Instruction : *mut Instruction, B0 : u8, B1 : u8, B2 : u8, B3 : u8, B4 : u8, B5 : u8, B6 : u8, B7 : u8, B8 : u8, B9 : u8, B10 : u8 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::try_with_declare_byte_11( B0, B1, B2, B3, B4, B5, B6, B7, B8, B9, B10 ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Declare_Byte_12( Instruction : *mut Instruction, B0 : u8, B1 : u8, B2 : u8, B3 : u8, B4 : u8, B5 : u8, B6 : u8, B7 : u8, B8 : u8, B9 : u8, B10 : u8, B11 : u8 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::try_with_declare_byte_12( B0, B1, B2, B3, B4, B5, B6, B7, B8, B9, B10, B11 ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Declare_Byte_13( Instruction : *mut Instruction, B0 : u8, B1 : u8, B2 : u8, B3 : u8, B4 : u8, B5 : u8, B6 : u8, B7 : u8, B8 : u8, B9 : u8, B10 : u8, B11 : u8, B12 : u8 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::try_with_declare_byte_13( B0, B1, B2, B3, B4, B5, B6, B7, B8, B9, B10, B11, B12 ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Declare_Byte_14( Instruction : *mut Instruction, B0 : u8, B1 : u8, B2 : u8, B3 : u8, B4 : u8, B5 : u8, B6 : u8, B7 : u8, B8 : u8, B9 : u8, B10 : u8, B11 : u8, B12 : u8, B13 : u8 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::try_with_declare_byte_14( B0, B1, B2, B3, B4, B5, B6, B7, B8, B9, B10, B11, B12, B13 ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Declare_Byte_15( Instruction : *mut Instruction, B0 : u8, B1 : u8, B2 : u8, B3 : u8, B4 : u8, B5 : u8, B6 : u8, B7 : u8, B8 : u8, B9 : u8, B10 : u8, B11 : u8, B12 : u8, B13 : u8, B14 : u8 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::try_with_declare_byte_15( B0, B1, B2, B3, B4, B5, B6, B7, B8, B9, B10, B11, B12, B13, B14 ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Declare_Byte_16( Instruction : *mut Instruction, B0 : u8, B1 : u8, B2 : u8, B3 : u8, B4 : u8, B5 : u8, B6 : u8, B7 : u8, B8 : u8, B9 : u8, B10 : u8, B11 : u8, B12 : u8, B13 : u8, B14 : u8, B15 : u8 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::try_with_declare_byte_16( B0, B1, B2, B3, B4, B5, B6, B7, B8, B9, B10, B11, B12, B13, B14, B15 ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Declare_Word_1( Instruction : *mut Instruction, W0 : u16 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::try_with_declare_word_1( W0 ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Declare_Word_2( Instruction : *mut Instruction, W0 : u16, W1 : u16 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::try_with_declare_word_2( W0, W1 ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Declare_Word_3( Instruction : *mut Instruction, W0 : u16, W1 : u16, W2 : u16 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::try_with_declare_word_3( W0, W1, W2 ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Declare_Word_4( Instruction : *mut Instruction, W0 : u16, W1 : u16, W2 : u16, W3 : u16 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::try_with_declare_word_4( W0, W1, W2, W3 ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Declare_Word_5( Instruction : *mut Instruction, W0 : u16, W1 : u16, W2 : u16, W3 : u16, W4 : u16 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::try_with_declare_word_5( W0, W1, W2, W3, W4 ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Declare_Word_6( Instruction : *mut Instruction, W0 : u16, W1 : u16, W2 : u16, W3 : u16, W4 : u16, W5 : u16 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::try_with_declare_word_6( W0, W1, W2, W3, W4, W5 ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Declare_Word_7( Instruction : *mut Instruction, W0 : u16, W1 : u16, W2 : u16, W3 : u16, W4 : u16, W5 : u16, W6 : u16 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::try_with_declare_word_7( W0, W1, W2, W3, W4, W5, W6 ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Declare_Word_8( Instruction : *mut Instruction, W0 : u16, W1 : u16, W2 : u16, W3 : u16, W4 : u16, W5 : u16, W6 : u16, W7 : u16 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::try_with_declare_word_8( W0, W1, W2, W3, W4, W5, W6, W7 ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Declare_DWord_1( Instruction : *mut Instruction, D0 : u32 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::try_with_declare_dword_1( D0 ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Declare_DWord_2( Instruction : *mut Instruction, D0 : u32, D1 : u32 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::try_with_declare_dword_2( D0, D1 ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Declare_DWord_3( Instruction : *mut Instruction, D0 : u32, D1 : u32, D2 : u32 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::try_with_declare_dword_3( D0, D1, D2 ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Declare_DWord_4( Instruction : *mut Instruction, D0 : u32, D1 : u32, D2 : u32, D3 : u32 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::try_with_declare_dword_4( D0, D1, D2, D3 ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Declare_QWord_1( Instruction : *mut Instruction, Q0 : u64 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::try_with_declare_qword_1( Q0 ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_With_Declare_QWord_2( Instruction : *mut Instruction, Q0 : u64, Q1 : u64 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  match Instruction::try_with_declare_qword_2( Q0, Q1 ) {
    Err( _e ) => return false,
    Ok( instruction ) => { 
        (*Instruction) = instruction;
        return true
    }
  };
}