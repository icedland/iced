# SPDX-License-Identifier: MIT
# Copyright (C) 2018-present iced project and contributors

import copy
import pytest
from iced_x86 import *

def test_eq_ne_hash():
	mem1 = MemoryOperand()
	mem2 = MemoryOperand(Register.NONE, Register.NONE, 1, 0, 0, False, Register.NONE)
	mem3 = MemoryOperand(index=Register.RAX)

	assert mem1 == mem2
	assert not (mem1 != mem2)
	assert hash(mem1) == hash(mem2)

	assert mem1 != mem3
	assert not (mem1 == mem3)

	assert mem1 != 1
	assert mem1 != 1.23
	assert mem1 != None
	assert mem1 != []
	assert mem1 != {}
	assert mem1 != (1, 2)

	assert not (mem1 == 1)
	assert not (mem1 == 1.23)
	assert not (mem1 == None)
	assert not (mem1 == [])
	assert not (mem1 == {})
	assert not (mem1 == (1, 2))

@pytest.mark.parametrize("copy_mem", [
	lambda mem: copy.copy(mem),
	lambda mem: copy.deepcopy(mem),
	lambda mem: mem.copy(),
])
def test_copy_deepcopy_mcopy(copy_mem):
	mem = MemoryOperand(Register.RCX, Register.RDI, 8, 0x1234_5678, 8, True, Register.GS)
	mem2 = copy_mem(mem)
	assert mem is not mem2
	assert id(mem) != id(mem2)

	assert mem == mem2
	assert not (mem != mem2)
	assert hash(mem) == hash(mem2)

def test_ctor():
	mem1 = MemoryOperand(base=Register.RCX, index=Register.ZMM13, scale=2, displ=-0x1234_5678, displ_size=8, is_broadcast=True, seg=Register.FS)
	mem2 = MemoryOperand(seg=Register.FS, is_broadcast=True, displ_size=8, displ=-0x1234_5678, scale=2, index=Register.ZMM13, base=Register.RCX)
	assert mem1 == mem2
	assert not (mem1 != mem2)
	assert hash(mem1) == hash(mem2)

def test_displ_size():
	mem1 = MemoryOperand(Register.EBP, displ=0x1234_5678)

	mem2 = MemoryOperand(Register.EBP, displ=0x1234_5678, displ_size=0)
	assert mem1 == mem2
	assert not (mem1 != mem2)
	assert hash(mem1) == hash(mem2)

	mem2 = MemoryOperand(Register.EBP, displ=0x1234_5678, displ_size=1)
	assert mem1 == mem2
	assert not (mem1 != mem2)
	assert hash(mem1) == hash(mem2)

	mem2 = MemoryOperand(Register.EBP, displ=0x1234_5678, displ_size=4)
	assert mem1 != mem2
	assert not (mem1 == mem2)

@pytest.mark.parametrize("create", [
	lambda: MemoryOperand(base=1234),
	lambda: MemoryOperand(index=1234),
	lambda: MemoryOperand(seg=1234),
])
def test_invalid_reg_enum_arg(create):
	with pytest.raises(ValueError):
		create()
