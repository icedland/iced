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

import pytest
from iced_x86 import *

@pytest.mark.parametrize("bitness", [16, 32, 64, 0, 15, 128])
def test_invalid_bitness(bitness):
	if bitness == 16 or bitness == 32 or bitness == 64:
		BlockEncoder(bitness)
	else:
		with pytest.raises(ValueError):
			BlockEncoder(bitness)

@pytest.mark.parametrize("fix_branches", [False, True])
def test_fix_branches_arg(fix_branches):
	decoder = Decoder(64, b"\x72\x00")
	decoder.ip = 0x1234_5678_9ABC_DEF0
	new_rip = 0xFEDC_BA98_7654_3210
	instr = decoder.decode()
	encoder = BlockEncoder(64, fix_branches)
	encoder.add(instr)
	if fix_branches:
		encoder.encode(new_rip)
	else:
		with pytest.raises(ValueError):
			encoder.encode(new_rip)

def test_fix_branches_defaults_to_true():
	decoder = Decoder(64, b"\x72\x00")
	decoder.ip = 0x1234_5678_9ABC_DEF0
	new_rip = 0xFEDC_BA98_7654_3210
	instr = decoder.decode()
	encoder = BlockEncoder(64)
	encoder.add(instr)
	# No exception
	encoded_data = encoder.encode(new_rip)
	assert len(encoded_data) > 2

def test_encode():
	decoder = Decoder(64, b"\xF3\x90\x90\x48\x09\xCE\x48\x09\xCE\x90\xF3\x90\x48\x09\xCE")
	decoder.ip = 0x1234_5678_9ABC_DEF0
	new_rip = 0xFEDC_BA98_7654_3210
	instrs = [instr for instr in decoder]
	encoder = BlockEncoder(64)
	encoder.add(instrs[0])
	encoder.add(instrs[1])
	encoder.add(instrs[2])
	encoder.add_many(instrs[3:6])
	encoder.add(instrs[6])
	encoded_data = encoder.encode(new_rip)
	assert type(encoded_data) == bytes
	assert encoded_data == b"\xF3\x90\x90\x48\x09\xCE\x48\x09\xCE\x90\xF3\x90\x48\x09\xCE"

def test_encode_invalid():
	encoder = BlockEncoder(64)
	encoder.add(Instruction())
	with pytest.raises(ValueError):
		encoder.encode(0)

def test_encode_empty():
	encoder = BlockEncoder(64)
	encoded_data = encoder.encode(0xFEDC_BA98_7654_3210)
	assert type(encoded_data) == bytes
	assert encoded_data == b""
