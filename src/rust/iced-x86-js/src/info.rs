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

use iced_x86::{Instruction, InstructionInfo, InstructionInfoFactory};
use wasm_bindgen::prelude::*;

/// Instruction info options used by [`InstructionInfoFactory`]
///
/// [`InstructionInfoFactory`]: struct.InstructionInfoFactory.html
#[wasm_bindgen(js_name = "InstructionInfoOptions")]
#[derive(Debug, Copy, Clone, Eq, PartialEq, Ord, PartialOrd, Hash)]
pub enum InstructionInfoOptionsJS {
	/// No option is enabled
	None = 0,
	/// Don't include memory usage, i.e., [`InstructionInfo::used_memory()`] will return an empty vector. All
	/// registers that are used by memory operands are still returned by [`InstructionInfo::used_registers()`].
	///
	/// [`InstructionInfo::used_memory()`]: struct.InstructionInfo.html#method.used_memory
	/// [`InstructionInfo::used_registers()`]: struct.InstructionInfo.html#method.used_registers
	NoMemoryUsage = 0x0000_0001,
	/// Don't include register usage, i.e., [`InstructionInfo::used_registers()`] will return an empty vector
	///
	/// [`InstructionInfo::used_registers()`]: struct.InstructionInfo.html#method.used_registers
	NoRegisterUsage = 0x0000_0002,
}

/// Creates [`InstructionInfo`]s.
///
/// If you don't need to know register and memory usage, it's faster to call [`Instruction`] and
/// [`Code`] methods such as [`Instruction::flow_control()`] instead of getting that info from this struct.
///
/// [`InstructionInfo`]: struct.InstructionInfo.html
/// [`Instruction`]: struct.Instruction.html
/// [`Code`]: enum.Code.html
/// [`Instruction::flow_control()`]: struct.Instruction.html#method.flow_control
#[wasm_bindgen]
#[derive(Debug)]
pub struct InstructionInfoFactoryX86 {
	info_factory: InstructionInfoFactory,
}

#[wasm_bindgen]
impl InstructionInfoFactoryX86 {
	/// Creates a new instance.
	///
	/// If you don't need to know register and memory usage, it's faster to call [`Instruction`] and
	/// [`Code`] methods such as [`Instruction::flow_control()`] instead of getting that info from this struct.
	///
	/// [`Instruction`]: struct.Instruction.html
	/// [`Code`]: enum.Code.html
	/// [`Instruction::flow_control()`]: struct.Instruction.html#method.flow_control
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	///
	/// // add [rdi+r12*8-5AA5EDCCh],esi
	/// let bytes = b"\x42\x01\xB4\xE7\x34\x12\x5A\xA5";
	/// let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
	///
	/// // This allocates two vectors but they get re-used every time you call info() and info_options().
	/// let mut info_factory = InstructionInfoFactory::new();
	///
	/// for instr in &mut decoder {
	///     // There's also info_options() if you only need reg usage or only mem usage.
	///     // info() returns both.
	///     let info = info_factory.info(&instr);
	///     for mem_info in info.used_memory().iter() {
	///         println!("{:?}", mem_info);
	///     }
	///     for reg_info in info.used_registers().iter() {
	///         println!("{:?}", reg_info);
	///     }
	/// }
	/// ```
	#[wasm_bindgen(constructor)]
	#[allow(clippy::new_without_default)]
	#[must_use]
	pub fn new() -> Self {
		InstructionInfoFactoryX86 { info_factory: InstructionInfoFactory::new() }
	}

	/// Creates a new [`InstructionInfo`], see also [`info_options()`] if you only need register usage
	/// but not memory usage or vice versa.
	///
	/// If you don't need to know register and memory usage, it's faster to call [`Instruction`] and
	/// [`Code`] methods such as [`Instruction::flow_control()`] instead of getting that info from this struct.
	///
	/// [`InstructionInfo`]: struct.InstructionInfo.html
	/// [`info_options()`]: #method.info_options
	/// [`Instruction`]: struct.Instruction.html
	/// [`Code`]: enum.Code.html
	/// [`Instruction::flow_control()`]: struct.Instruction.html#method.flow_control
	///
	/// # Arguments
	///
	/// * `instruction`: The instruction that should be analyzed
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	///
	/// // add [rdi+r12*8-5AA5EDCCh],esi
	/// let bytes = b"\x42\x01\xB4\xE7\x34\x12\x5A\xA5";
	/// let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
	/// let mut info_factory = InstructionInfoFactory::new();
	///
	/// let instr = decoder.decode();
	/// let info = info_factory.info(&instr);
	///
	/// assert_eq!(1, info.used_memory().len());
	/// let mem = info.used_memory()[0];
	/// assert_eq!(Register::DS, mem.segment());
	/// assert_eq!(Register::RDI, mem.base());
	/// assert_eq!(Register::R12, mem.index());
	/// assert_eq!(8, mem.scale());
	/// assert_eq!(0xFFFFFFFFA55A1234, mem.displacement());
	/// assert_eq!(MemorySize::UInt32, mem.memory_size());
	/// assert_eq!(OpAccess::ReadWrite, mem.access());
	///
	/// let regs = info.used_registers();
	/// assert_eq!(3, regs.len());
	/// assert_eq!(Register::RDI, regs[0].register());
	/// assert_eq!(OpAccess::Read, regs[0].access());
	/// assert_eq!(Register::R12, regs[1].register());
	/// assert_eq!(OpAccess::Read, regs[1].access());
	/// assert_eq!(Register::ESI, regs[2].register());
	/// assert_eq!(OpAccess::Read, regs[2].access());
	/// ```
	pub fn info(&mut self, instruction: &Instruction) -> InstructionInfo {
		(*self.info_factory.info(instruction)).clone()
	}

	/// Creates a new [`InstructionInfo`], see also [`info()`].
	///
	/// If you don't need to know register and memory usage, it's faster to call [`Instruction`] and
	/// [`Code`] methods such as [`Instruction::flow_control()`] instead of getting that info from this struct.
	///
	/// [`InstructionInfo`]: struct.InstructionInfo.html
	/// [`info()`]: #method.info
	/// [`Instruction`]: struct.Instruction.html
	/// [`Code`]: enum.Code.html
	/// [`Instruction::flow_control()`]: struct.Instruction.html#method.flow_control
	///
	/// # Arguments
	///
	/// * `instruction`: The instruction that should be analyzed
	/// * `options`: Options, see [`InstructionInfoOptions`]
	///
	/// [`InstructionInfoOptions`]: struct.InstructionInfoOptions.html
	pub fn info_options(&mut self, instruction: &Instruction, options: InstructionInfoOptionsJS) -> InstructionInfo {
		(*self.info_factory.info_options(instruction, options as u32)).clone()
	}
}
