// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::register::{iced_to_register, register_to_iced, Register};
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
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Register, RegisterExt } = require("iced-x86");
	///
	/// assert.equal(RegisterExt.base(Register.GS), Register.ES);
	/// assert.equal(RegisterExt.base(Register.SIL), Register.AL);
	/// assert.equal(RegisterExt.base(Register.SP), Register.AX);
	/// assert.equal(RegisterExt.base(Register.R13D), Register.EAX);
	/// assert.equal(RegisterExt.base(Register.RBP), Register.RAX);
	/// assert.equal(RegisterExt.base(Register.MM6), Register.MM0);
	/// assert.equal(RegisterExt.base(Register.XMM28), Register.XMM0);
	/// assert.equal(RegisterExt.base(Register.YMM12), Register.YMM0);
	/// assert.equal(RegisterExt.base(Register.ZMM31), Register.ZMM0);
	/// assert.equal(RegisterExt.base(Register.K3), Register.K0);
	/// assert.equal(RegisterExt.base(Register.BND1), Register.BND0);
	/// assert.equal(RegisterExt.base(Register.ST7), Register.ST0);
	/// assert.equal(RegisterExt.base(Register.CR8), Register.CR0);
	/// assert.equal(RegisterExt.base(Register.DR6), Register.DR0);
	/// assert.equal(RegisterExt.base(Register.TR3), Register.TR0);
	/// assert.equal(RegisterExt.base(Register.RIP), Register.EIP);
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
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Register, RegisterExt } = require("iced-x86");
	///
	/// assert.equal(RegisterExt.number(Register.GS), 5);
	/// assert.equal(RegisterExt.number(Register.SIL), 10);
	/// assert.equal(RegisterExt.number(Register.SP), 4);
	/// assert.equal(RegisterExt.number(Register.R13D), 13);
	/// assert.equal(RegisterExt.number(Register.RBP), 5);
	/// assert.equal(RegisterExt.number(Register.MM6), 6);
	/// assert.equal(RegisterExt.number(Register.XMM28), 28);
	/// assert.equal(RegisterExt.number(Register.YMM12), 12);
	/// assert.equal(RegisterExt.number(Register.ZMM31), 31);
	/// assert.equal(RegisterExt.number(Register.K3), 3);
	/// assert.equal(RegisterExt.number(Register.BND1), 1);
	/// assert.equal(RegisterExt.number(Register.ST7), 7);
	/// assert.equal(RegisterExt.number(Register.CR8), 8);
	/// assert.equal(RegisterExt.number(Register.DR6), 6);
	/// assert.equal(RegisterExt.number(Register.TR3), 3);
	/// assert.equal(RegisterExt.number(Register.RIP), 1);
	/// ```
	pub fn number(value: Register) -> u32 {
		register_to_iced(value).number() as u32
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
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Register, RegisterExt } = require("iced-x86");
	///
	/// assert.equal(RegisterExt.fullRegister(Register.GS), Register.GS);
	/// assert.equal(RegisterExt.fullRegister(Register.SIL), Register.RSI);
	/// assert.equal(RegisterExt.fullRegister(Register.SP), Register.RSP);
	/// assert.equal(RegisterExt.fullRegister(Register.R13D), Register.R13);
	/// assert.equal(RegisterExt.fullRegister(Register.RBP), Register.RBP);
	/// assert.equal(RegisterExt.fullRegister(Register.MM6), Register.MM6);
	/// assert.equal(RegisterExt.fullRegister(Register.XMM10), Register.ZMM10);
	/// assert.equal(RegisterExt.fullRegister(Register.YMM10), Register.ZMM10);
	/// assert.equal(RegisterExt.fullRegister(Register.ZMM10), Register.ZMM10);
	/// assert.equal(RegisterExt.fullRegister(Register.K3), Register.K3);
	/// assert.equal(RegisterExt.fullRegister(Register.BND1), Register.BND1);
	/// assert.equal(RegisterExt.fullRegister(Register.ST7), Register.ST7);
	/// assert.equal(RegisterExt.fullRegister(Register.CR8), Register.CR8);
	/// assert.equal(RegisterExt.fullRegister(Register.DR6), Register.DR6);
	/// assert.equal(RegisterExt.fullRegister(Register.TR3), Register.TR3);
	/// assert.equal(RegisterExt.fullRegister(Register.RIP), Register.RIP);
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
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Register, RegisterExt } = require("iced-x86");
	///
	/// assert.equal(RegisterExt.fullRegister32(Register.GS), Register.GS);
	/// assert.equal(RegisterExt.fullRegister32(Register.SIL), Register.ESI);
	/// assert.equal(RegisterExt.fullRegister32(Register.SP), Register.ESP);
	/// assert.equal(RegisterExt.fullRegister32(Register.R13D), Register.R13D);
	/// assert.equal(RegisterExt.fullRegister32(Register.RBP), Register.EBP);
	/// assert.equal(RegisterExt.fullRegister32(Register.MM6), Register.MM6);
	/// assert.equal(RegisterExt.fullRegister32(Register.XMM10), Register.ZMM10);
	/// assert.equal(RegisterExt.fullRegister32(Register.YMM10), Register.ZMM10);
	/// assert.equal(RegisterExt.fullRegister32(Register.ZMM10), Register.ZMM10);
	/// assert.equal(RegisterExt.fullRegister32(Register.K3), Register.K3);
	/// assert.equal(RegisterExt.fullRegister32(Register.BND1), Register.BND1);
	/// assert.equal(RegisterExt.fullRegister32(Register.ST7), Register.ST7);
	/// assert.equal(RegisterExt.fullRegister32(Register.CR8), Register.CR8);
	/// assert.equal(RegisterExt.fullRegister32(Register.DR6), Register.DR6);
	/// assert.equal(RegisterExt.fullRegister32(Register.TR3), Register.TR3);
	/// assert.equal(RegisterExt.fullRegister32(Register.RIP), Register.RIP);
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
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Register, RegisterExt } = require("iced-x86");
	///
	/// assert.equal(RegisterExt.size(Register.GS), 2);
	/// assert.equal(RegisterExt.size(Register.SIL), 1);
	/// assert.equal(RegisterExt.size(Register.SP), 2);
	/// assert.equal(RegisterExt.size(Register.R13D), 4);
	/// assert.equal(RegisterExt.size(Register.RBP), 8);
	/// assert.equal(RegisterExt.size(Register.MM6), 8);
	/// assert.equal(RegisterExt.size(Register.XMM10), 16);
	/// assert.equal(RegisterExt.size(Register.YMM10), 32);
	/// assert.equal(RegisterExt.size(Register.ZMM10), 64);
	/// assert.equal(RegisterExt.size(Register.K3), 8);
	/// assert.equal(RegisterExt.size(Register.BND1), 16);
	/// assert.equal(RegisterExt.size(Register.ST7), 10);
	/// assert.equal(RegisterExt.size(Register.CR8), 8);
	/// assert.equal(RegisterExt.size(Register.DR6), 8);
	/// assert.equal(RegisterExt.size(Register.TR3), 4);
	/// assert.equal(RegisterExt.size(Register.RIP), 8);
	/// ```
	pub fn size(value: Register) -> u32 {
		register_to_iced(value).size() as u32
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
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Register, RegisterExt } = require("iced-x86");
	///
	/// assert.ok(RegisterExt.isSegmentRegister(Register.GS));
	/// assert.ok(!RegisterExt.isSegmentRegister(Register.RCX));
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
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Register, RegisterExt } = require("iced-x86");
	///
	/// assert.ok(!RegisterExt.isGPR(Register.GS));
	/// assert.ok(RegisterExt.isGPR(Register.CH));
	/// assert.ok(RegisterExt.isGPR(Register.DX));
	/// assert.ok(RegisterExt.isGPR(Register.R13D));
	/// assert.ok(RegisterExt.isGPR(Register.RSP));
	/// assert.ok(!RegisterExt.isGPR(Register.XMM0));
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
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Register, RegisterExt } = require("iced-x86");
	///
	/// assert.ok(!RegisterExt.isGPR8(Register.GS));
	/// assert.ok(RegisterExt.isGPR8(Register.CH));
	/// assert.ok(!RegisterExt.isGPR8(Register.DX));
	/// assert.ok(!RegisterExt.isGPR8(Register.R13D));
	/// assert.ok(!RegisterExt.isGPR8(Register.RSP));
	/// assert.ok(!RegisterExt.isGPR8(Register.XMM0));
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
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Register, RegisterExt } = require("iced-x86");
	///
	/// assert.ok(!RegisterExt.isGPR16(Register.GS));
	/// assert.ok(!RegisterExt.isGPR16(Register.CH));
	/// assert.ok(RegisterExt.isGPR16(Register.DX));
	/// assert.ok(!RegisterExt.isGPR16(Register.R13D));
	/// assert.ok(!RegisterExt.isGPR16(Register.RSP));
	/// assert.ok(!RegisterExt.isGPR16(Register.XMM0));
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
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Register, RegisterExt } = require("iced-x86");
	///
	/// assert.ok(!RegisterExt.isGPR32(Register.GS));
	/// assert.ok(!RegisterExt.isGPR32(Register.CH));
	/// assert.ok(!RegisterExt.isGPR32(Register.DX));
	/// assert.ok(RegisterExt.isGPR32(Register.R13D));
	/// assert.ok(!RegisterExt.isGPR32(Register.RSP));
	/// assert.ok(!RegisterExt.isGPR32(Register.XMM0));
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
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Register, RegisterExt } = require("iced-x86");
	///
	/// assert.ok(!RegisterExt.isGPR64(Register.GS));
	/// assert.ok(!RegisterExt.isGPR64(Register.CH));
	/// assert.ok(!RegisterExt.isGPR64(Register.DX));
	/// assert.ok(!RegisterExt.isGPR64(Register.R13D));
	/// assert.ok(RegisterExt.isGPR64(Register.RSP));
	/// assert.ok(!RegisterExt.isGPR64(Register.XMM0));
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
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Register, RegisterExt } = require("iced-x86");
	///
	/// assert.ok(!RegisterExt.isXMM(Register.R13D));
	/// assert.ok(!RegisterExt.isXMM(Register.RSP));
	/// assert.ok(RegisterExt.isXMM(Register.XMM0));
	/// assert.ok(!RegisterExt.isXMM(Register.YMM0));
	/// assert.ok(!RegisterExt.isXMM(Register.ZMM0));
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
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Register, RegisterExt } = require("iced-x86");
	///
	/// assert.ok(!RegisterExt.isYMM(Register.R13D));
	/// assert.ok(!RegisterExt.isYMM(Register.RSP));
	/// assert.ok(!RegisterExt.isYMM(Register.XMM0));
	/// assert.ok(RegisterExt.isYMM(Register.YMM0));
	/// assert.ok(!RegisterExt.isYMM(Register.ZMM0));
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
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Register, RegisterExt } = require("iced-x86");
	///
	/// assert.ok(!RegisterExt.isZMM(Register.R13D));
	/// assert.ok(!RegisterExt.isZMM(Register.RSP));
	/// assert.ok(!RegisterExt.isZMM(Register.XMM0));
	/// assert.ok(!RegisterExt.isZMM(Register.YMM0));
	/// assert.ok(RegisterExt.isZMM(Register.ZMM0));
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
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Register, RegisterExt } = require("iced-x86");
	///
	/// assert.ok(!RegisterExt.isVectorRegister(Register.R13D));
	/// assert.ok(!RegisterExt.isVectorRegister(Register.RSP));
	/// assert.ok(RegisterExt.isVectorRegister(Register.XMM0));
	/// assert.ok(RegisterExt.isVectorRegister(Register.YMM0));
	/// assert.ok(RegisterExt.isVectorRegister(Register.ZMM0));
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
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Register, RegisterExt } = require("iced-x86");
	///
	/// assert.ok(RegisterExt.isIP(Register.EIP));
	/// assert.ok(RegisterExt.isIP(Register.RIP));
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
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Register, RegisterExt } = require("iced-x86");
	///
	/// assert.ok(!RegisterExt.isK(Register.R13D));
	/// assert.ok(RegisterExt.isK(Register.K3));
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
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Register, RegisterExt } = require("iced-x86");
	///
	/// assert.ok(!RegisterExt.isCR(Register.R13D));
	/// assert.ok(RegisterExt.isCR(Register.CR3));
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
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Register, RegisterExt } = require("iced-x86");
	///
	/// assert.ok(!RegisterExt.isDR(Register.R13D));
	/// assert.ok(RegisterExt.isDR(Register.DR3));
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
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Register, RegisterExt } = require("iced-x86");
	///
	/// assert.ok(!RegisterExt.isTR(Register.R13D));
	/// assert.ok(RegisterExt.isTR(Register.TR3));
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
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Register, RegisterExt } = require("iced-x86");
	///
	/// assert.ok(!RegisterExt.isST(Register.R13D));
	/// assert.ok(RegisterExt.isST(Register.ST3));
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
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Register, RegisterExt } = require("iced-x86");
	///
	/// assert.ok(!RegisterExt.isBND(Register.R13D));
	/// assert.ok(RegisterExt.isBND(Register.BND3));
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
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Register, RegisterExt } = require("iced-x86");
	///
	/// assert.ok(!RegisterExt.isMM(Register.R13D));
	/// assert.ok(RegisterExt.isMM(Register.MM3));
	/// ```
	#[wasm_bindgen(js_name = "isMM")]
	pub fn is_mm(value: Register) -> bool {
		register_to_iced(value).is_mm()
	}

	/// Checks if it's a tile register (`TMM0`-`TMM7`)
	///
	/// # Arguments
	///
	/// - `value`: A [`Register`] enum value
	///
	/// [`Register`]: enum.Register.html
	///
	/// # Examples
	///
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Register, RegisterExt } = require("iced-x86");
	///
	/// assert.ok(!RegisterExt.isTMM(Register.R13D));
	/// assert.ok(RegisterExt.isTMM(Register.TMM3));
	/// ```
	#[wasm_bindgen(js_name = "isTMM")]
	pub fn is_tmm(value: Register) -> bool {
		register_to_iced(value).is_tmm()
	}
}
