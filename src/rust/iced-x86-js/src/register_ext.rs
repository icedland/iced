/*
Copyright (C) 2018-2019 de4dot@gmail.com

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

use super::register::{iced_to_register, register_to_iced, Register};
use wasm_bindgen::prelude::*;

/// [`Register`] enum extension methods
///
/// [`Register`]: enum.Register.html
#[wasm_bindgen]
pub struct RegisterExt;

#[wasm_bindgen]
impl RegisterExt {
	/// Gets the base register, eg. `AL`, `AX`, `EAX`, `RAX`, `MM0`, `XMM0`, `YMM0`, `ZMM0`, `ES`
	///
	/// # Arguments
	///
	/// - `value`: A [`Register`] enum value
	///
	/// [`Register`]: enum.Register.html
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	/// assert_eq!(Register::ES, Register::GS.base());
	/// assert_eq!(Register::AL, Register::SIL.base());
	/// assert_eq!(Register::AX, Register::SP.base());
	/// assert_eq!(Register::EAX, Register::R13D.base());
	/// assert_eq!(Register::RAX, Register::RBP.base());
	/// assert_eq!(Register::MM0, Register::MM6.base());
	/// assert_eq!(Register::XMM0, Register::XMM28.base());
	/// assert_eq!(Register::YMM0, Register::YMM12.base());
	/// assert_eq!(Register::ZMM0, Register::ZMM31.base());
	/// assert_eq!(Register::K0, Register::K3.base());
	/// assert_eq!(Register::BND0, Register::BND1.base());
	/// assert_eq!(Register::ST0, Register::ST7.base());
	/// assert_eq!(Register::CR0, Register::CR8.base());
	/// assert_eq!(Register::DR0, Register::DR6.base());
	/// assert_eq!(Register::TR0, Register::TR3.base());
	/// assert_eq!(Register::EIP, Register::RIP.base());
	/// ```
	pub fn base(value: Register) -> Register {
		iced_to_register(register_to_iced(value).base())
	}

	/// The register number (index) relative to [`RegisterExt.base()`], eg. 0-15, or 0-31, or if 8-bit GPR, 0-19
	///
	/// # Arguments
	///
	/// - `value`: A [`Register`] enum value
	///
	/// [`Register`]: enum.Register.html
	/// [`RegisterExt.base()`]: #method.base
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	/// assert_eq!(5, Register::GS.number());
	/// assert_eq!(10, Register::SIL.number());
	/// assert_eq!(4, Register::SP.number());
	/// assert_eq!(13, Register::R13D.number());
	/// assert_eq!(5, Register::RBP.number());
	/// assert_eq!(6, Register::MM6.number());
	/// assert_eq!(28, Register::XMM28.number());
	/// assert_eq!(12, Register::YMM12.number());
	/// assert_eq!(31, Register::ZMM31.number());
	/// assert_eq!(3, Register::K3.number());
	/// assert_eq!(1, Register::BND1.number());
	/// assert_eq!(7, Register::ST7.number());
	/// assert_eq!(8, Register::CR8.number());
	/// assert_eq!(6, Register::DR6.number());
	/// assert_eq!(3, Register::TR3.number());
	/// assert_eq!(1, Register::RIP.number());
	/// ```
	pub fn number(value: Register) -> usize {
		register_to_iced(value).number()
	}

	/// Gets the full register that this one is a part of, eg. `CL`/`CH`/`CX`/`ECX`/`RCX` -> `RCX`, `XMM11`/`YMM11`/`ZMM11` -> `ZMM11`
	///
	/// # Arguments
	///
	/// - `value`: A [`Register`] enum value
	///
	/// [`Register`]: enum.Register.html
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	/// assert_eq!(Register::GS, Register::GS.full_register());
	/// assert_eq!(Register::RSI, Register::SIL.full_register());
	/// assert_eq!(Register::RSP, Register::SP.full_register());
	/// assert_eq!(Register::R13, Register::R13D.full_register());
	/// assert_eq!(Register::RBP, Register::RBP.full_register());
	/// assert_eq!(Register::MM6, Register::MM6.full_register());
	/// assert_eq!(Register::ZMM10, Register::XMM10.full_register());
	/// assert_eq!(Register::ZMM10, Register::YMM10.full_register());
	/// assert_eq!(Register::ZMM10, Register::ZMM10.full_register());
	/// assert_eq!(Register::K3, Register::K3.full_register());
	/// assert_eq!(Register::BND1, Register::BND1.full_register());
	/// assert_eq!(Register::ST7, Register::ST7.full_register());
	/// assert_eq!(Register::CR8, Register::CR8.full_register());
	/// assert_eq!(Register::DR6, Register::DR6.full_register());
	/// assert_eq!(Register::TR3, Register::TR3.full_register());
	/// assert_eq!(Register::RIP, Register::RIP.full_register());
	/// ```
	#[wasm_bindgen(js_name = "fullRegister")]
	pub fn full_register(value: Register) -> Register {
		iced_to_register(register_to_iced(value).full_register())
	}

	/// Gets the full register that this one is a part of, except if it's a GPR in which case the 32-bit register is returned,
	/// eg. `CL`/`CH`/`CX`/`ECX`/`RCX` -> `ECX`, `XMM11`/`YMM11`/`ZMM11` -> `ZMM11`
	///
	/// # Arguments
	///
	/// - `value`: A [`Register`] enum value
	///
	/// [`Register`]: enum.Register.html
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	/// assert_eq!(Register::GS, Register::GS.full_register32());
	/// assert_eq!(Register::ESI, Register::SIL.full_register32());
	/// assert_eq!(Register::ESP, Register::SP.full_register32());
	/// assert_eq!(Register::R13D, Register::R13D.full_register32());
	/// assert_eq!(Register::EBP, Register::RBP.full_register32());
	/// assert_eq!(Register::MM6, Register::MM6.full_register32());
	/// assert_eq!(Register::ZMM10, Register::XMM10.full_register32());
	/// assert_eq!(Register::ZMM10, Register::YMM10.full_register32());
	/// assert_eq!(Register::ZMM10, Register::ZMM10.full_register32());
	/// assert_eq!(Register::K3, Register::K3.full_register32());
	/// assert_eq!(Register::BND1, Register::BND1.full_register32());
	/// assert_eq!(Register::ST7, Register::ST7.full_register32());
	/// assert_eq!(Register::CR8, Register::CR8.full_register32());
	/// assert_eq!(Register::DR6, Register::DR6.full_register32());
	/// assert_eq!(Register::TR3, Register::TR3.full_register32());
	/// assert_eq!(Register::RIP, Register::RIP.full_register32());
	/// ```
	#[wasm_bindgen(js_name = "fullRegister32")]
	pub fn full_register32(value: Register) -> Register {
		iced_to_register(register_to_iced(value).full_register32())
	}

	/// Gets the size of the register in bytes
	///
	/// # Arguments
	///
	/// - `value`: A [`Register`] enum value
	///
	/// [`Register`]: enum.Register.html
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	/// assert_eq!(2, Register::GS.size());
	/// assert_eq!(1, Register::SIL.size());
	/// assert_eq!(2, Register::SP.size());
	/// assert_eq!(4, Register::R13D.size());
	/// assert_eq!(8, Register::RBP.size());
	/// assert_eq!(8, Register::MM6.size());
	/// assert_eq!(16, Register::XMM10.size());
	/// assert_eq!(32, Register::YMM10.size());
	/// assert_eq!(64, Register::ZMM10.size());
	/// assert_eq!(8, Register::K3.size());
	/// assert_eq!(16, Register::BND1.size());
	/// assert_eq!(10, Register::ST7.size());
	/// assert_eq!(8, Register::CR8.size());
	/// assert_eq!(8, Register::DR6.size());
	/// assert_eq!(4, Register::TR3.size());
	/// assert_eq!(8, Register::RIP.size());
	/// ```
	pub fn size(value: Register) -> usize {
		register_to_iced(value).size()
	}
}

#[wasm_bindgen]
impl RegisterExt {
	/// Checks if it's a segment register (`ES`, `CS`, `SS`, `DS`, `FS`, `GS`)
	///
	/// # Arguments
	///
	/// - `value`: A [`Register`] enum value
	///
	/// [`Register`]: enum.Register.html
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	/// assert!(Register::GS.is_segment_register());
	/// assert!(!Register::RCX.is_segment_register());
	/// ```
	#[wasm_bindgen(js_name = "isSegmentRegister")]
	pub fn is_segment_register(value: Register) -> bool {
		register_to_iced(value).is_segment_register()
	}

	/// Checks if it's a general purpose register (`AL`-`R15L`, `AX`-`R15W`, `EAX`-`R15D`, `RAX`-`R15`)
	///
	/// # Arguments
	///
	/// - `value`: A [`Register`] enum value
	///
	/// [`Register`]: enum.Register.html
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	/// assert!(!Register::GS.is_gpr());
	/// assert!(Register::CH.is_gpr());
	/// assert!(Register::DX.is_gpr());
	/// assert!(Register::R13D.is_gpr());
	/// assert!(Register::RSP.is_gpr());
	/// assert!(!Register::XMM0.is_gpr());
	/// ```
	#[wasm_bindgen(js_name = "isGPR")]
	pub fn is_gpr(value: Register) -> bool {
		register_to_iced(value).is_gpr()
	}

	/// Checks if it's an 8-bit general purpose register (`AL`-`R15L`)
	///
	/// # Arguments
	///
	/// - `value`: A [`Register`] enum value
	///
	/// [`Register`]: enum.Register.html
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	/// assert!(!Register::GS.is_gpr8());
	/// assert!(Register::CH.is_gpr8());
	/// assert!(!Register::DX.is_gpr8());
	/// assert!(!Register::R13D.is_gpr8());
	/// assert!(!Register::RSP.is_gpr8());
	/// assert!(!Register::XMM0.is_gpr8());
	/// ```
	#[wasm_bindgen(js_name = "isGPR8")]
	pub fn is_gpr8(value: Register) -> bool {
		register_to_iced(value).is_gpr8()
	}

	/// Checks if it's a 16-bit general purpose register (`AX`-`R15W`)
	///
	/// # Arguments
	///
	/// - `value`: A [`Register`] enum value
	///
	/// [`Register`]: enum.Register.html
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	/// assert!(!Register::GS.is_gpr16());
	/// assert!(!Register::CH.is_gpr16());
	/// assert!(Register::DX.is_gpr16());
	/// assert!(!Register::R13D.is_gpr16());
	/// assert!(!Register::RSP.is_gpr16());
	/// assert!(!Register::XMM0.is_gpr16());
	/// ```
	#[wasm_bindgen(js_name = "isGPR16")]
	pub fn is_gpr16(value: Register) -> bool {
		register_to_iced(value).is_gpr16()
	}

	/// Checks if it's a 32-bit general purpose register (`EAX`-`R15D`)
	///
	/// # Arguments
	///
	/// - `value`: A [`Register`] enum value
	///
	/// [`Register`]: enum.Register.html
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	/// assert!(!Register::GS.is_gpr32());
	/// assert!(!Register::CH.is_gpr32());
	/// assert!(!Register::DX.is_gpr32());
	/// assert!(Register::R13D.is_gpr32());
	/// assert!(!Register::RSP.is_gpr32());
	/// assert!(!Register::XMM0.is_gpr32());
	/// ```
	#[wasm_bindgen(js_name = "isGPR32")]
	pub fn is_gpr32(value: Register) -> bool {
		register_to_iced(value).is_gpr32()
	}

	/// Checks if it's a 64-bit general purpose register (`RAX`-`R15`)
	///
	/// # Arguments
	///
	/// - `value`: A [`Register`] enum value
	///
	/// [`Register`]: enum.Register.html
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	/// assert!(!Register::GS.is_gpr64());
	/// assert!(!Register::CH.is_gpr64());
	/// assert!(!Register::DX.is_gpr64());
	/// assert!(!Register::R13D.is_gpr64());
	/// assert!(Register::RSP.is_gpr64());
	/// assert!(!Register::XMM0.is_gpr64());
	/// ```
	#[wasm_bindgen(js_name = "isGPR64")]
	pub fn is_gpr64(value: Register) -> bool {
		register_to_iced(value).is_gpr64()
	}

	/// Checks if it's a 128-bit vector register (`XMM0`-`XMM31`)
	///
	/// # Arguments
	///
	/// - `value`: A [`Register`] enum value
	///
	/// [`Register`]: enum.Register.html
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	/// assert!(!Register::R13D.is_xmm());
	/// assert!(!Register::RSP.is_xmm());
	/// assert!(Register::XMM0.is_xmm());
	/// assert!(!Register::YMM0.is_xmm());
	/// assert!(!Register::ZMM0.is_xmm());
	/// ```
	#[wasm_bindgen(js_name = "isXMM")]
	pub fn is_xmm(value: Register) -> bool {
		register_to_iced(value).is_xmm()
	}

	/// Checks if it's a 256-bit vector register (`YMM0`-`YMM31`)
	///
	/// # Arguments
	///
	/// - `value`: A [`Register`] enum value
	///
	/// [`Register`]: enum.Register.html
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	/// assert!(!Register::R13D.is_ymm());
	/// assert!(!Register::RSP.is_ymm());
	/// assert!(!Register::XMM0.is_ymm());
	/// assert!(Register::YMM0.is_ymm());
	/// assert!(!Register::ZMM0.is_ymm());
	/// ```
	#[wasm_bindgen(js_name = "isYMM")]
	pub fn is_ymm(value: Register) -> bool {
		register_to_iced(value).is_ymm()
	}

	/// Checks if it's a 512-bit vector register (`ZMM0`-`ZMM31`)
	///
	/// # Arguments
	///
	/// - `value`: A [`Register`] enum value
	///
	/// [`Register`]: enum.Register.html
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	/// assert!(!Register::R13D.is_zmm());
	/// assert!(!Register::RSP.is_zmm());
	/// assert!(!Register::XMM0.is_zmm());
	/// assert!(!Register::YMM0.is_zmm());
	/// assert!(Register::ZMM0.is_zmm());
	/// ```
	#[wasm_bindgen(js_name = "isZMM")]
	pub fn is_zmm(value: Register) -> bool {
		register_to_iced(value).is_zmm()
	}

	/// Checks if it's an `XMM`, `YMM` or `ZMM` register
	///
	/// # Arguments
	///
	/// - `value`: A [`Register`] enum value
	///
	/// [`Register`]: enum.Register.html
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	/// assert!(!Register::R13D.is_vector_register());
	/// assert!(!Register::RSP.is_vector_register());
	/// assert!(Register::XMM0.is_vector_register());
	/// assert!(Register::YMM0.is_vector_register());
	/// assert!(Register::ZMM0.is_vector_register());
	/// ```
	#[wasm_bindgen(js_name = "isVectorRegister")]
	pub fn is_vector_register(value: Register) -> bool {
		register_to_iced(value).is_vector_register()
	}

	/// Checks if it's `EIP`/`RIP`
	///
	/// # Arguments
	///
	/// - `value`: A [`Register`] enum value
	///
	/// [`Register`]: enum.Register.html
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	/// assert!(Register::EIP.is_ip());
	/// assert!(Register::RIP.is_ip());
	/// ```
	#[wasm_bindgen(js_name = "isIP")]
	pub fn is_ip(value: Register) -> bool {
		register_to_iced(value).is_ip()
	}

	/// Checks if it's an opmask register (`K0`-`K7`)
	///
	/// # Arguments
	///
	/// - `value`: A [`Register`] enum value
	///
	/// [`Register`]: enum.Register.html
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	/// assert!(!Register::R13D.is_k());
	/// assert!(Register::K3.is_k());
	/// ```
	#[wasm_bindgen(js_name = "isK")]
	pub fn is_k(value: Register) -> bool {
		register_to_iced(value).is_k()
	}

	/// Checks if it's a control register (`CR0`-`CR15`)
	///
	/// # Arguments
	///
	/// - `value`: A [`Register`] enum value
	///
	/// [`Register`]: enum.Register.html
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	/// assert!(!Register::R13D.is_cr());
	/// assert!(Register::CR3.is_cr());
	/// ```
	#[wasm_bindgen(js_name = "isCR")]
	pub fn is_cr(value: Register) -> bool {
		register_to_iced(value).is_cr()
	}

	/// Checks if it's a debug register (`DR0`-`DR15`)
	///
	/// # Arguments
	///
	/// - `value`: A [`Register`] enum value
	///
	/// [`Register`]: enum.Register.html
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	/// assert!(!Register::R13D.is_dr());
	/// assert!(Register::DR3.is_dr());
	/// ```
	#[wasm_bindgen(js_name = "isDR")]
	pub fn is_dr(value: Register) -> bool {
		register_to_iced(value).is_dr()
	}

	/// Checks if it's a test register (`TR0`-`TR7`)
	///
	/// # Arguments
	///
	/// - `value`: A [`Register`] enum value
	///
	/// [`Register`]: enum.Register.html
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	/// assert!(!Register::R13D.is_tr());
	/// assert!(Register::TR3.is_tr());
	/// ```
	#[wasm_bindgen(js_name = "isTR")]
	pub fn is_tr(value: Register) -> bool {
		register_to_iced(value).is_tr()
	}

	/// Checks if it's an FPU stack register (`ST0`-`ST7`)
	///
	/// # Arguments
	///
	/// - `value`: A [`Register`] enum value
	///
	/// [`Register`]: enum.Register.html
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	/// assert!(!Register::R13D.is_st());
	/// assert!(Register::ST3.is_st());
	/// ```
	#[wasm_bindgen(js_name = "isST")]
	pub fn is_st(value: Register) -> bool {
		register_to_iced(value).is_st()
	}

	/// Checks if it's a bound register (`BND0`-`BND3`)
	///
	/// # Arguments
	///
	/// - `value`: A [`Register`] enum value
	///
	/// [`Register`]: enum.Register.html
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	/// assert!(!Register::R13D.is_bnd());
	/// assert!(Register::BND3.is_bnd());
	/// ```
	#[wasm_bindgen(js_name = "isBND")]
	pub fn is_bnd(value: Register) -> bool {
		register_to_iced(value).is_bnd()
	}

	/// Checks if it's an MMX register (`MM0`-`MM7`)
	///
	/// # Arguments
	///
	/// - `value`: A [`Register`] enum value
	///
	/// [`Register`]: enum.Register.html
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	/// assert!(!Register::R13D.is_mm());
	/// assert!(Register::MM3.is_mm());
	/// ```
	#[wasm_bindgen(js_name = "isMM")]
	pub fn is_mm(value: Register) -> bool {
		register_to_iced(value).is_mm()
	}
}
