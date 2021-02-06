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

#[cfg(not(feature = "std"))]
use alloc::string::String;
use core::u32;
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
		for info in self.infos {
			if info.0 == section_name {
				return Some(info);
			}
		}
		None
	}
}
