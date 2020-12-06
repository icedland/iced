#
# Copyright (C) 2018-2019 de4dot@gmail.com
#
# Permission is hereby granted, free of charge, to any person obtaining
# a copy of this software and associated documentation files (the
# "Software"), to deal in the Software without restriction, including
# without limitation the rights to use, copy, modify, merge, publish,
# distribute, sublicense, and/or sell copies of the Software, and to
# permit persons to whom the Software is furnished to do so, subject to
# the following conditions:
#
# The above copyright notice and this permission notice shall be
# included in all copies or substantial portions of the Software.
#
# THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
# EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
# MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
# IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
# CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
# TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
# SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#

from iced_x86 import *

def test_default_options():
	formatter = FastFormatter()
	assert not formatter.space_after_operand_separator
	assert not formatter.rip_relative_addresses
	assert formatter.use_pseudo_ops
	assert not formatter.show_symbol_address
	assert not formatter.always_show_segment_register
	assert not formatter.always_show_memory_size
	assert formatter.uppercase_hex
	assert not formatter.use_hex_prefix

	formatter.space_after_operand_separator = not formatter.space_after_operand_separator
	formatter.rip_relative_addresses = not formatter.rip_relative_addresses
	formatter.use_pseudo_ops = not formatter.use_pseudo_ops
	formatter.show_symbol_address = not formatter.show_symbol_address
	formatter.always_show_segment_register = not formatter.always_show_segment_register
	formatter.always_show_memory_size = not formatter.always_show_memory_size
	formatter.uppercase_hex = not formatter.uppercase_hex
	formatter.use_hex_prefix = not formatter.use_hex_prefix

	assert formatter.space_after_operand_separator
	assert formatter.rip_relative_addresses
	assert not formatter.use_pseudo_ops
	assert formatter.show_symbol_address
	assert formatter.always_show_segment_register
	assert formatter.always_show_memory_size
	assert not formatter.uppercase_hex
	assert formatter.use_hex_prefix

def test_format():
	decoder = Decoder(64, b"\x62\xF2\x4F\xDD\x72\x50\x03")
	instr = decoder.decode()
	formatter = FastFormatter()

	assert type(formatter.format(instr)) == str

	assert formatter.format(instr) == "vcvtne2ps2bf16 zmm2{k5}{z},zmm6,dword bcst [rax+0Ch]"
	formatter.space_after_operand_separator = True
	assert formatter.format(instr) == "vcvtne2ps2bf16 zmm2{k5}{z}, zmm6, dword bcst [rax+0Ch]"
	formatter.use_hex_prefix = True
	assert formatter.format(instr) == "vcvtne2ps2bf16 zmm2{k5}{z}, zmm6, dword bcst [rax+0xC]"
	formatter.always_show_segment_register = True
	assert formatter.format(instr) == "vcvtne2ps2bf16 zmm2{k5}{z}, zmm6, dword bcst ds:[rax+0xC]"
	formatter.uppercase_hex = False
	assert formatter.format(instr) == "vcvtne2ps2bf16 zmm2{k5}{z}, zmm6, dword bcst ds:[rax+0xc]"
