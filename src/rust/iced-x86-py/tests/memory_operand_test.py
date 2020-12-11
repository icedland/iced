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
	lambda mem: mem.clone(),
])
def test_copy_deepcopy_clone(copy_mem):
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
