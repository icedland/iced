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

use super::super::super::FastFormatter;
use super::super::super::SymbolResolver;
#[cfg(not(feature = "std"))]
use alloc::boxed::Box;
#[cfg(not(feature = "std"))]
use alloc::string::String;

pub(super) fn create_default() -> Box<FastFormatter> {
	Box::new(FastFormatter::new())
}

pub(super) fn create_inverted() -> Box<FastFormatter> {
	let mut fmt = FastFormatter::new();

	let opt = fmt.options().space_after_operand_separator() ^ true;
	fmt.options_mut().set_space_after_operand_separator(opt);

	let opt = fmt.options().rip_relative_addresses() ^ true;
	fmt.options_mut().set_rip_relative_addresses(opt);

	let opt = fmt.options().use_pseudo_ops() ^ true;
	fmt.options_mut().set_use_pseudo_ops(opt);

	let opt = fmt.options().show_symbol_address() ^ true;
	fmt.options_mut().set_show_symbol_address(opt);

	let opt = fmt.options().always_show_segment_register() ^ true;
	fmt.options_mut().set_always_show_segment_register(opt);

	let opt = fmt.options().always_show_memory_size() ^ true;
	fmt.options_mut().set_always_show_memory_size(opt);

	let opt = fmt.options().uppercase_hex() ^ true;
	fmt.options_mut().set_uppercase_hex(opt);

	let opt = fmt.options().use_hex_prefix() ^ true;
	fmt.options_mut().set_use_hex_prefix(opt);

	Box::new(fmt)
}

pub(super) fn create_options() -> Box<FastFormatter> {
	let mut fmt = FastFormatter::new();
	fmt.options_mut().set_rip_relative_addresses(true);
	Box::new(fmt)
}

pub(super) fn create_resolver(symbol_resolver: Box<SymbolResolver>) -> Box<FastFormatter> {
	let mut fmt = FastFormatter::with_options(Some(symbol_resolver));
	fmt.options_mut().set_rip_relative_addresses(true);
	Box::new(fmt)
}
