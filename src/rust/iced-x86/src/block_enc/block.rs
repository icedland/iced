// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::block_enc::*;
use crate::iced_error::IcedError;
use alloc::rc::Rc;
use alloc::vec::Vec;
use core::cell::RefCell;

pub(super) struct Block {
	pub(super) encoder: Encoder,
	pub(super) rip: u64,
	reloc_infos: Option<Vec<RelocInfo>>,
	data_vec: Vec<Rc<RefCell<BlockData>>>,
	alignment: u64,
	valid_data: Vec<Rc<RefCell<BlockData>>>,
	valid_data_address: u64,
	valid_data_address_aligned: u64,
	// start and end indexes (exclusive) of its instructions, eg. all_instrs[x.0..x.1]
	instr_indexes: (usize, usize),
}

impl Block {
	pub(super) fn new(bitness: u32, rip: u64, reloc_infos: Option<Vec<RelocInfo>>, start_index: usize, end_index: usize) -> Result<Self, IcedError> {
		Ok(Self {
			encoder: Encoder::try_new(bitness)?,
			rip,
			reloc_infos,
			data_vec: Vec::new(),
			alignment: bitness as u64 / 8,
			valid_data: Vec::new(),
			valid_data_address: 0,
			valid_data_address_aligned: 0,
			instr_indexes: (start_index, end_index),
		})
	}

	pub(super) const fn is_in_block(&self, instr_index: usize) -> bool {
		self.instr_indexes.0 <= instr_index && instr_index < self.instr_indexes.1
	}

	pub(super) fn alloc_pointer_location(&mut self) -> Rc<RefCell<BlockData>> {
		let data = Rc::new(RefCell::new(BlockData { data: 0, address: 0, address_initd: false, is_valid: true }));
		self.data_vec.push(data.clone());
		data
	}

	pub(super) fn initialize_data(&mut self, base_addr: u64) {
		self.valid_data_address = base_addr;

		let mut addr = base_addr.wrapping_add(self.alignment).wrapping_sub(1) & !self.alignment.wrapping_sub(1);
		self.valid_data_address_aligned = addr;
		for data in &mut self.data_vec {
			if !data.borrow().is_valid {
				continue;
			}
			data.borrow_mut().address = addr;
			data.borrow_mut().address_initd = true;
			self.valid_data.push(data.clone());
			addr = addr.wrapping_add(self.alignment);
		}
	}

	pub(super) fn write_data(&mut self) -> Result<(), IcedError> {
		if self.valid_data.is_empty() {
			return Ok(());
		}
		for _ in 0..self.valid_data_address_aligned.wrapping_sub(self.valid_data_address) {
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

	pub(super) const fn can_add_reloc_infos(&self) -> bool {
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
	pub(super) const fn address(&self) -> Result<u64, IcedError> {
		if self.is_valid && self.address_initd {
			Ok(self.address)
		} else {
			Err(IcedError::new("Internal error"))
		}
	}
}
