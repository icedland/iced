# SPDX-License-Identifier: MIT
# Copyright (C) 2018-present iced project and contributors

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
	decoder = Decoder(64, b"\x72\x00", ip=0x1234_5678_9ABC_DEF0)
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
	decoder = Decoder(64, b"\x72\x00", ip=0x1234_5678_9ABC_DEF0)
	new_rip = 0xFEDC_BA98_7654_3210
	instr = decoder.decode()
	encoder = BlockEncoder(64)
	encoder.add(instr)
	# No exception
	encoded_data = encoder.encode(new_rip)
	assert len(encoded_data) > 2

def test_encode():
	decoder = Decoder(64, b"\xF3\x90\x90\x48\x09\xCE\x48\x09\xCE\x90\xF3\x90\x48\x09\xCE", ip=0x1234_5678_9ABC_DEF0)
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
