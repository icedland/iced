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

use super::super::iced_error::IcedError;
use super::instr::*;
use super::*;
#[cfg(any(has_alloc, not(feature = "std")))]
use alloc::rc::Rc;
#[cfg(not(feature = "std"))]
use alloc::vec::Vec;
use core::cell::RefCell;
#[cfg(all(not(has_alloc), feature = "std"))]
use std::rc::Rc;

pub(super) struct Block {
	pub(super) encoder: Encoder,
	pub(super) rip: u64,
	reloc_infos: Option<Vec<RelocInfo>>,
	data_vec: Vec<Rc<RefCell<BlockData>>>,
	alignment: u64,
	valid_data: Vec<Rc<RefCell<BlockData>>>,
	valid_data_address: u64,
	valid_data_address_aligned: u64,
}

impl Block {
	pub(super) fn new(block_encoder: &BlockEncoder, rip: u64, reloc_infos: Option<Vec<RelocInfo>>) -> Self {
		Self {
			encoder: Encoder::new(block_encoder.bitness()),
			rip,
			reloc_infos,
			data_vec: Vec::new(),
			alignment: block_encoder.bitness() as u64 / 8,
			valid_data: Vec::new(),
			valid_data_address: 0,
			valid_data_address_aligned: 0,
		}
	}

	pub(super) fn alloc_pointer_location(&mut self) -> Rc<RefCell<BlockData>> {
		let data = Rc::new(RefCell::new(BlockData { data: 0, address: 0, address_initd: false, is_valid: true }));
		self.data_vec.push(Rc::clone(&data));
		data
	}

	pub(super) fn initialize_data(&mut self, instructions: &[Rc<RefCell<Instr>>]) {
		let base_addr = match instructions.last() {
			Some(instr) => instr.borrow().ip().wrapping_add(instr.borrow().size() as u64),
			None => self.rip,
		};
		self.valid_data_address = base_addr;

		let mut addr = base_addr.wrapping_add(self.alignment).wrapping_sub(1) & !self.alignment.wrapping_sub(1);
		self.valid_data_address_aligned = addr;
		for data in &mut self.data_vec {
			if !data.borrow().is_valid {
				continue;
			}
			data.borrow_mut().address = addr;
			data.borrow_mut().address_initd = true;
			self.valid_data.push(Rc::clone(data));
			addr = addr.wrapping_add(self.alignment);
		}
	}

	pub(super) fn write_data(&mut self) -> Result<(), IcedError> {
		if self.valid_data.is_empty() {
			return Ok(());
		}
		for _ in 0..self.valid_data_address_aligned - self.valid_data_address {
			self.encoder.write_byte_internal(0xCC);
		}
		match self.alignment {
			8 => {
				for data in &self.valid_data {
					let data = data.borrow();
					if let Some(ref mut reloc_infos) = self.reloc_infos {
						reloc_infos.push(RelocInfo::new(RelocKind::Offset64, data.address()?));
					}
					let d64 = data.data;
					let mut d = d64 as u32;
					self.encoder.write_byte_internal(d);
					self.encoder.write_byte_internal(d >> 8);
					self.encoder.write_byte_internal(d >> 16);
					self.encoder.write_byte_internal(d >> 24);
					d = (d64 >> 32) as u32;
					self.encoder.write_byte_internal(d);
					self.encoder.write_byte_internal(d >> 8);
					self.encoder.write_byte_internal(d >> 16);
					self.encoder.write_byte_internal(d >> 24);
				}
			}

			_ => unreachable!(),
		}

		Ok(())
	}

	pub(super) fn buffer_pos(&self) -> usize {
		self.encoder.position()
	}

	pub(super) fn write_byte(&mut self, value: u32) {
		self.encoder.write_byte_internal(value);
	}

	pub(super) fn take_buffer(&mut self) -> Vec<u8> {
		self.encoder.take_buffer()
	}

	pub(super) fn take_reloc_infos(&mut self) -> Vec<RelocInfo> {
		self.reloc_infos.take().unwrap_or_default()
	}

	pub(super) fn dispose(&mut self) {
		self.data_vec.clear();
		self.valid_data.clear();
	}

	pub(super) fn can_add_reloc_infos(&self) -> bool {
		self.reloc_infos.is_some()
	}

	pub(super) fn add_reloc_info(&mut self, reloc_info: RelocInfo) {
		if let Some(ref mut reloc_infos) = self.reloc_infos {
			reloc_infos.push(reloc_info);
		}
	}
}

pub(super) struct BlockData {
	pub(super) data: u64,
	address: u64,
	address_initd: bool,
	pub(super) is_valid: bool,
}

impl BlockData {
	pub(super) fn address(&self) -> Result<u64, IcedError> {
		if self.is_valid && self.address_initd {
			Ok(self.address)
		} else {
			Err(IcedError::new("Internal error"))
		}
	}
}
