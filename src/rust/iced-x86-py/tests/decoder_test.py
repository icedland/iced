# SPDX-License-Identifier: MIT
# Copyright (C) 2018-present iced project and contributors

import copy
import pytest
from iced_x86 import *

@pytest.mark.parametrize("bitness", [16, 32, 64, 0, 15, 128])
def test_invalid_bitness(bitness):
	data = b"\x90"
	if bitness == 16 or bitness == 32 or bitness == 64:
		Decoder(bitness, data)
	else:
		with pytest.raises(ValueError):
			Decoder(bitness, data)

@pytest.mark.parametrize("data, is_valid", [
	(b"", True),
	(b"\x90", True),
	(bytearray(b""), True),
	(bytearray(b"\x90"), True),
	(memoryview(b""), False),
	(memoryview(b"\x90"), False),
	(memoryview(bytearray(b"")), False),
	(memoryview(bytearray(b"\x90")), False),
	(None, False),
	(123, False),
	("Hello", False),
])
def test_different_data_types(data, is_valid):
	bitness = 64
	if is_valid:
		decoder = Decoder(bitness, data)
		if len(data) == 0:
			assert decoder.decode().code == Code.INVALID
			assert decoder.last_error == DecoderError.NO_MORE_BYTES
		else:
			assert decoder.decode().code != Code.INVALID
			assert decoder.last_error == DecoderError.NONE
	else:
		with pytest.raises(TypeError):
			Decoder(bitness, data)

@pytest.mark.parametrize("bitness, data, code", [
	(16, b"\x03\xCE", Code.ADD_R16_RM16),
	(32, b"\x03\xCE", Code.ADD_R32_RM32),
	(64, b"\x48\x03\xCE", Code.ADD_R64_RM64),
])
def test_bitness(bitness, data, code):
	decoder = Decoder(bitness, data)
	assert decoder.bitness == bitness
	instr = decoder.decode()
	assert instr.code == code

def test_without_decoder_options():
	decoder = Decoder(64, b"\x0F\x1A\x08")
	instr = decoder.decode()
	assert instr.code != Code.BNDLDX_BND_MIB

def test_with_decoder_options():
	decoder = Decoder(64, b"\x0F\x1A\x08", DecoderOptions.MPX)
	instr = decoder.decode()
	assert instr.code == Code.BNDLDX_BND_MIB

def test_iter():
	decoder = Decoder(64, b"\x48\x09\xCE\x90\xF3\x90")
	instrs = [instr for instr in decoder]
	assert len(instrs) == 3
	assert instrs[0].code == Code.OR_RM64_R64
	assert instrs[1].code == Code.NOPD
	assert instrs[2].code == Code.PAUSE

def test_ip():
	decoder = Decoder(64, b"\x48\x09\xCE\x90")
	assert decoder.ip == 0
	decoder = Decoder(64, b"\x48\x09\xCE\x90", ip=0xABCD_EF01_2345_6789)
	assert decoder.ip == 0xABCD_EF01_2345_6789
	decoder.ip = 0x1234_5678_9ABC_DEF0
	assert decoder.ip == 0x1234_5678_9ABC_DEF0
	decoder.decode()
	assert decoder.ip == 0x1234_5678_9ABC_DEF3
	decoder.decode()
	assert decoder.ip == 0x1234_5678_9ABC_DEF4
	decoder.decode()
	assert decoder.ip == 0x1234_5678_9ABC_DEF4
	decoder.ip = 0xFEDC_BA98_7654_3210
	assert decoder.ip == 0xFEDC_BA98_7654_3210

def test_position():
	decoder = Decoder(64, b"\x48\x09\xCE\x90\xF3\x90")

	for _ in range(2):
		assert decoder.can_decode
		assert decoder.max_position == 6
		assert decoder.position == 0

		assert decoder.decode().code == Code.OR_RM64_R64
		assert decoder.can_decode
		assert decoder.max_position == 6
		assert decoder.position == 3

		assert decoder.decode().code == Code.NOPD
		assert decoder.can_decode
		assert decoder.max_position == 6
		assert decoder.position == 4

		assert decoder.decode().code == Code.PAUSE
		assert not decoder.can_decode
		assert decoder.max_position == 6
		assert decoder.position == 6

		assert decoder.decode().code == Code.INVALID
		assert not decoder.can_decode
		assert decoder.max_position == 6
		assert decoder.position == 6

		decoder.position = 0

	decoder.position = 3
	assert decoder.decode().code == Code.NOPD
	assert decoder.can_decode
	assert decoder.max_position == 6
	assert decoder.position == 4

def test_invalid_position():
	decoder = Decoder(64, b"\x48\x09\xCE\x90\xF3\x90")
	decoder.position = 0
	decoder.position = 6
	with pytest.raises(ValueError):
		decoder.position = 7

def test_last_error():
	decoder = Decoder(64, b"\x90\x48")
	assert decoder.last_error == DecoderError.NONE

	assert decoder.decode().code == Code.NOPD
	assert decoder.last_error == DecoderError.NONE
	assert decoder.decode().code == Code.INVALID
	assert decoder.last_error == DecoderError.NO_MORE_BYTES

	decoder = Decoder(64, b"\xF0\x90")
	assert decoder.decode().code == Code.INVALID
	assert decoder.last_error == DecoderError.INVALID_INSTRUCTION

def test_decode():
	data = b"\x48\x09\xCE\x90\xF3\x90"
	decodera = Decoder(64, data)
	decoderb = Decoder(64, data)
	instr = Instruction()

	instr1 = decodera.decode()
	decoderb.decode_out(instr)
	assert instr.eq_all_bits(instr1)

	instr2 = decodera.decode()
	decoderb.decode_out(instr)
	assert instr.eq_all_bits(instr2)

def test_offsets():
	decoder = Decoder(64, b"\x90\x83\xB3\x34\x12\x5A\xA5\x5A")

	instr = decoder.decode()
	co = decoder.get_constant_offsets(instr)
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
	co = decoder.get_constant_offsets(instr)
	assert co.displacement_offset == 2
	assert co.displacement_size == 4
	assert co.immediate_offset == 6
	assert co.immediate_size == 1
	assert co.immediate_offset2 == 0
	assert co.immediate_size2 == 0
	assert co.has_displacement
	assert co.has_immediate
	assert not co.has_immediate2

def test_co_eq_ne_hash():
	decoder = Decoder(64, b"\x90\x90\x83\xB3\x34\x12\x5A\xA5\x5A\x83\xB3\x34\x12\x5A\xA5\x5A")
	instr = decoder.decode()
	co1 = decoder.get_constant_offsets(instr)
	instr = decoder.decode()
	co2 = decoder.get_constant_offsets(instr)
	instr = decoder.decode()
	co3 = decoder.get_constant_offsets(instr)
	instr = decoder.decode()
	co4 = decoder.get_constant_offsets(instr)

	assert id(co1) != id(co2)
	assert id(co3) != id(co4)

	assert hash(co1) == hash(co2)
	assert hash(co3) == hash(co4)

	assert co1 == co2
	assert not (co1 != co2)

	assert co3 == co4
	assert not (co3 != co4)

	assert co1 != co3
	assert not (co1 == co3)

	assert co1 != 1
	assert co1 != 1.23
	assert co1 != None
	assert co1 != []
	assert co1 != {}
	assert co1 != (1, 2)

	assert not (co1 == 1)
	assert not (co1 == 1.23)
	assert not (co1 == None)
	assert not (co1 == [])
	assert not (co1 == {})
	assert not (co1 == (1, 2))

@pytest.mark.parametrize("copy_co", [
	lambda instr: copy.copy(instr),
	lambda instr: copy.deepcopy(instr),
	lambda instr: instr.copy(),
])
def test_co_copy_deepcopy_mcopy(copy_co):
	decoder = Decoder(64, b"\x90\x83\xB3\x34\x12\x5A\xA5\x5A", ip=0x1234_5678_9ABC_DEF1)
	coa = decoder.get_constant_offsets(decoder.decode())
	cob = decoder.get_constant_offsets(decoder.decode())

	coa2 = copy_co(coa)
	assert coa is not coa2
	assert id(coa) != id(coa2)
	assert coa == coa2

	cob2 = copy_co(cob)
	assert cob is not cob2
	assert id(cob) != id(cob2)
	assert cob == cob2

def test_no_bytes():
	decoder = Decoder(64, b"")
	assert not decoder.can_decode
	assert decoder.position == 0
	assert decoder.max_position == 0

	instr = decoder.decode()
	assert decoder.last_error == DecoderError.NO_MORE_BYTES
	assert instr.is_invalid
	assert not instr
	assert not decoder.can_decode
	assert decoder.position == 0
	assert decoder.max_position == 0

def test_no_invalid_check_option():
	data = b"\xF0\x02\xCE"
	decodera = Decoder(64, data)
	decoderb = Decoder(64, data, DecoderOptions.NO_INVALID_CHECK)

	instra = decodera.decode()
	instrb = decoderb.decode()

	assert instra.code == Code.INVALID
	assert decodera.last_error == DecoderError.INVALID_INSTRUCTION
	assert instrb.code == Code.ADD_R8_RM8

def test_amd_option():
	data = b"\x66\x70\x5A"
	decodera = Decoder(64, data)
	decoderb = Decoder(64, data, DecoderOptions.AMD)

	instra = decodera.decode()
	instrb = decoderb.decode()

	assert instra.code == Code.JO_REL8_64
	assert instrb.code == Code.JO_REL8_16

def test_multiple_options():
	decoder = Decoder(64, b"\xF3\x90", DecoderOptions.AMD | DecoderOptions.NO_PAUSE)
	instr = decoder.decode()
	assert instr.code == Code.NOPD
