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
use std::error::Error;
use std::fs;
use std::mem;
use std::path::PathBuf;
use structopt::StructOpt;

#[derive(StructOpt)]
struct Options {
	#[structopt(short = "b")]
	bitness: u32,
	#[structopt(short = "f", long, parse(from_os_str))]
	filename: PathBuf,
	#[structopt(long)]
	amd: bool,
	#[structopt(long)]
	invalid: bool,
}

fn main() -> Result<(), Box<dyn Error>> {
	let options = Options::from_args();
	let has_code_value = !options.invalid;

	let bytes = fs::read(options.filename)?;
	let mut bytes = bytes.as_slice();
	let mut instr = Instruction::default();
	let mut decoder_options = DecoderOptions::MPX | DecoderOptions::NO_LOCK_MOV_CR0;
	if options.amd {
		decoder_options = (decoder_options & !DecoderOptions::NO_LOCK_MOV_CR0) | DecoderOptions::AMD;
	}
	let decoder_options = decoder_options;
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
		let mut decoder = Decoder::new(options.bitness, &bytes[0..len], decoder_options);
		decoder.decode_out(&mut instr);
		if options.invalid {
			assert_ne!(DecoderError::None, decoder.last_error());
			assert_eq!(Code::INVALID, instr.code());
		} else {
			assert_eq!(DecoderError::None, decoder.last_error());
			if has_code_value {
				assert_eq!(code, instr.code());
			}
			assert_eq!(len, instr.len());
		}
		bytes = &bytes[len..];
	}

	Ok(())
}
