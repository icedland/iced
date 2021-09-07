// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

// Use FuzzerGen (.NET) app to generate the valid and invalid files (--amd to use AMD decoder)
//		./IcedFuzzer.exe -16 -oil C:\path\invalid16.bin -ovlc C:\path\valid16.bin
//		./IcedFuzzer.exe -32 -oil C:\path\invalid32.bin -ovlc C:\path\valid32.bin
//		./IcedFuzzer.exe -64 -oil C:\path\invalid64.bin -ovlc C:\path\valid64.bin
// then (--amd to use AMD decoder)
//		cargo run --release -p iced-x86-fzgt -- -b 16 -f C:\path\valid16.bin
//		cargo run --release -p iced-x86-fzgt -- -b 16 -f C:\path\invalid16.bin --invalid
//		cargo run --release -p iced-x86-fzgt -- -b 32 -f C:\path\valid32.bin
//		cargo run --release -p iced-x86-fzgt -- -b 32 -f C:\path\invalid32.bin --invalid
//		cargo run --release -p iced-x86-fzgt -- -b 64 -f C:\path\valid64.bin
//		cargo run --release -p iced-x86-fzgt -- -b 64 -f C:\path\invalid64.bin --invalid

use iced_x86::*;
use std::env;
use std::error::Error;
use std::fmt;
use std::fs;
use std::mem;
use std::path::PathBuf;

struct Options {
	bitness: u32,
	filename: Option<PathBuf>,
	amd: bool,
	invalid: bool,
}

#[derive(Debug)]
struct ProgError(&'static str);
impl Error for ProgError {}
impl fmt::Display for ProgError {
	fn fmt(&self, f: &mut fmt::Formatter<'_>) -> fmt::Result {
		f.write_str(self.0)
	}
}

fn main() -> Result<(), Box<dyn Error>> {
	let mut options = Options { bitness: 0, filename: None, amd: false, invalid: false };
	let mut args = env::args().skip(1);
	while let Some(arg) = args.next() {
		match arg.as_str() {
			"-b" => options.bitness = str::parse(&args.next().ok_or(ProgError("Missing bitness"))?)?,
			"-f" => options.filename = Some(args.next().ok_or(ProgError("Missing filename"))?.into()),
			"--amd" => options.amd = true,
			"--invalid" => options.invalid = true,
			_ => return Err(ProgError("Invalid option").into()),
		}
	}
	let filename = options.filename.take().ok_or(ProgError("Missing filename"))?;
	match options.bitness {
		16 | 32 | 64 => {}
		_ => return Err(ProgError("Invalid bitness").into()),
	}
	let options = options;
	let has_code_value = !options.invalid;

	let bytes = fs::read(filename)?;
	let mut bytes = bytes.as_slice();
	let mut instr = Instruction::default();
	let decoder_options = DecoderOptions::MPX | if options.amd { DecoderOptions::AMD } else { DecoderOptions::NONE };
	while !bytes.is_empty() {
		let code = if has_code_value {
			let raw_value = (bytes[0] as u16) | ((bytes[1] as u16) << 8);
			bytes = &bytes[2..];
			unsafe { mem::transmute(raw_value) }
		} else {
			Code::INVALID
		};
		let len = bytes[0] as usize;
		bytes = &bytes[1..];
		let mut decoder = Decoder::try_new(options.bitness, &bytes[0..len], decoder_options)?;
		decoder.decode_out(&mut instr);
		if options.invalid {
			assert_ne!(decoder.last_error(), DecoderError::None);
			assert_eq!(instr.code(), Code::INVALID);
		} else {
			assert_eq!(decoder.last_error(), DecoderError::None);
			if has_code_value {
				assert_eq!(instr.code(), code);
			}
			assert_eq!(instr.len(), len);
		}
		bytes = &bytes[len..];
	}

	Ok(())
}
