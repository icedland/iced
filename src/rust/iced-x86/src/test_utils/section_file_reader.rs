// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use alloc::string::String;
use std::fs::File;
use std::io::prelude::*;
use std::io::BufReader;
use std::path::Path;

pub(crate) trait SectionFileLineHandler {
	fn line(&mut self, id: u32, line: &str) -> Result<(), String>;
}

pub(crate) struct SectionFileReader<'a> {
	infos: &'a [(&'a str, u32)],
}

impl<'a> SectionFileReader<'a> {
	pub(crate) fn new(infos: &'a [(&'a str, u32)]) -> Self {
		Self { infos }
	}

	pub(crate) fn read<T: SectionFileLineHandler>(&mut self, filename: &Path, handler: &mut T) {
		let display_filename = filename.display().to_string();
		let file = File::open(filename).unwrap_or_else(|_| panic!("Couldn't open file {}", display_filename));
		let mut current_section_info: (&str, u32) = ("", u32::MAX);
		for info in BufReader::new(file).lines().zip(1..) {
			let line_number = info.1;
			let err_str = match info.0 {
				Err(err) => Some(err.to_string()),
				Ok(line) => {
					if line.is_empty() || line.starts_with('#') {
						None
					} else if line.starts_with('[') {
						if !line.ends_with(']') {
							Some("Missing ']'".to_string())
						} else {
							let section_name = &line[1..line.len() - 1];
							if let Some(new_info) = self.get_section(section_name) {
								current_section_info = *new_info;
								None
							} else {
								Some(format!("Unknown section name: {}", section_name))
							}
						}
					} else {
						if current_section_info.0.is_empty() {
							Some("Missing section".to_string())
						} else {
							match handler.line(current_section_info.1, &line) {
								Err(err) => Some(err.to_string()),
								Ok(_) => None,
							}
						}
					}
				}
			};
			if let Some(err) = err_str {
				panic!("Error parsing file '{}', line {}: {}", display_filename, line_number, err);
			}
		}
	}

	fn get_section(&self, section_name: &str) -> Option<&(&'a str, u32)> {
		self.infos.iter().find(|&info| info.0 == section_name)
	}
}
