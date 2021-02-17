# SPDX-License-Identifier: MIT
# Copyright (C) 2018-present iced project and contributors

import pytest
from iced_x86 import *

@pytest.mark.parametrize("bitness", [16, 32, 64, 0, 15, 128])
def test_invalid_bitness(bitness):
	if bitness == 16 or bitness == 32 or bitness == 64:
		Encoder(bitness)
	else:
		with pytest.raises(ValueError):
			Encoder(bitness)

@pytest.mark.parametrize("capacity", [0, 1, 0x1234])
def test_capacity_arg(capacity):
	Encoder(64, capacity)

@pytest.mark.parametrize("bitness", [16, 32, 64])
def test_bitness(bitness):
	encoder = Encoder(bitness)
	assert encoder.bitness == bitness

@pytest.mark.parametrize("bitness", [16, 32, 64])
def test_options(bitness):
	encoder = Encoder(bitness)
	assert not encoder.prevent_vex2
	assert encoder.vex_wig == 0
	assert encoder.vex_lig == 0
	assert encoder.evex_wig == 0
	assert encoder.evex_lig == 0

	encoder.prevent_vex2 = True
	encoder.vex_wig = 1
	encoder.vex_lig = 1
	encoder.evex_wig = 1
	encoder.evex_lig = 2

	assert encoder.prevent_vex2
	assert encoder.vex_wig == 1
	assert encoder.vex_lig == 1
	assert encoder.evex_wig == 1
	assert encoder.evex_lig == 2

@pytest.mark.parametrize("bitness, data", [
	(16, b"\x03\xCE\x90\xF3\x90"),
	(32, b"\x03\xCE\x90\xF3\x90"),
	(64, b"\x48\x09\xCE\x90\xF3\x90"),
])
def test_encode(bitness, data):
	encoder = Encoder(bitness)
	decoder = Decoder(bitness, data, ip=0x1234_5678_9ABC_DEF0)
	rip = decoder.ip
	for instr in decoder:
		instr_len = encoder.encode(instr, rip)
		assert instr.len == instr_len
		rip += instr_len
	encoded_data = encoder.take_buffer()
	assert type(encoded_data) == bytes
	assert data == encoded_data

def test_encode_rip_63_set():
	decoder = Decoder(64, b"\x48\x09\xCE\x90\xF3\x90")
	instr = decoder.decode()
	encoder = Encoder(64)
	assert encoder.encode(instr, 0x1234_5678_9ABC_DEF0) == instr.len
	assert encoder.encode(instr, 0xFEDC_BA98_7654_3210) == instr.len

def test_write_u8():
	decoder = Decoder(64, b"\x48\x09\xCE\x90\xF3\x90")
	instr = decoder.decode()
	encoder = Encoder(64)
	encoder.write_u8(0x12)
	encoder.write_u8(0x34)
	assert encoder.encode(instr, 0) == 3
	encoder.write_u8(0x56)
	encoder.write_u8(0x78)
	encoder.write_u8(0x9A)
	encoded_data = encoder.take_buffer()
	assert encoded_data == b"\x12\x34\x48\x09\xCE\x56\x78\x9A"

def test_encode_invalid_instruction():
	# Can't encode INVALID
	instr = Instruction()
	encoder = Encoder(64)
	with pytest.raises(ValueError):
		encoder.encode(instr, instr.ip)

	# Jcc SHORT with a target too far away
	decoder = Decoder(64, b"\x72\x00", ip=0x1234_5678_9ABC_DEF0)
	decoder.decode_out(instr)
	encoder.encode(instr, instr.ip)
	with pytest.raises(ValueError):
		encoder.encode(instr, 0xFEDC_BA98_7654_3210)

def test_offsets():
	decoder = Decoder(64, b"\x90\x83\xB3\x34\x12\x5A\xA5\x5A")
	encoder = Encoder(64)

	instr = decoder.decode()
	assert encoder.encode(instr, instr.ip) == 1
	co = encoder.get_constant_offsets()
	assert co.displacement_offset == 0
	assert co.displacement_size == 0
	assert co.immediate_offset == 0
	assert co.immediate_size == 0
	assert co.immediate_offset2 == 0
	assert co.immediate_size2 == 0
	assert not co.has_displacement
	assert not co.has_immediate
	assert not co.has_immediate2

	instr = decoder.decode()
	assert encoder.encode(instr, instr.ip) == 7
	co = encoder.get_constant_offsets()
	assert co.displacement_offset == 2
	assert co.displacement_size == 4
	assert co.immediate_offset == 6
	assert co.immediate_size == 1
	assert co.immediate_offset2 == 0
	assert co.immediate_size2 == 0
	assert co.has_displacement
	assert co.has_immediate
	assert not co.has_immediate2
