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

def test_info():
	instr = Decoder(64, b"\xC4\xE3\x49\x48\x10\x41").decode()
	factory = InstructionInfoFactory()
	info = factory.info(instr)

	assert info.op0_access == OpAccess.WRITE
	assert info.op1_access == OpAccess.READ
	assert info.op2_access == OpAccess.READ
	assert info.op3_access == OpAccess.READ
	assert info.op4_access == OpAccess.READ
	assert info.op_access(0) == info.op0_access
	assert info.op_access(1) == info.op1_access
	assert info.op_access(2) == info.op2_access
	assert info.op_access(3) == info.op3_access
	assert info.op_access(4) == info.op4_access

	mem_list = info.used_memory()
	assert type(mem_list) == list
	assert len(mem_list) == 1
	mem = mem_list[0]
	assert mem.segment == Register.DS
	assert mem.base == Register.RAX
	assert mem.index == Register.NONE
	assert mem.scale == 1
	assert mem.displacement == 0
	assert mem.displacement_i64 == 0
	assert mem.memory_size == MemorySize.PACKED128_FLOAT32
	assert mem.access == OpAccess.READ
	assert mem.address_size == CodeSize.CODE64
	assert mem.vsib_size == 0

	reg_list = info.used_registers()
	assert type(reg_list) == list
	assert len(reg_list) == 4
	assert (reg_list[0].register, reg_list[0].access) == (Register.ZMM2, OpAccess.WRITE)
	assert (reg_list[1].register, reg_list[1].access) == (Register.XMM6, OpAccess.READ)
	assert (reg_list[2].register, reg_list[2].access) == (Register.RAX, OpAccess.READ)
	assert (reg_list[3].register, reg_list[3].access) == (Register.XMM4, OpAccess.READ)

@pytest.mark.parametrize("copy_value", [
	lambda instr: copy.copy(instr),
	lambda instr: copy.deepcopy(instr),
	lambda instr: instr.clone(),
])
def test_copy_deepcopy_clone(copy_value):
	instr = Decoder(64, b"\xC4\xE3\x49\x48\x10\x41").decode()
	factory = InstructionInfoFactory()
	info = factory.info(instr)

	mem = info.used_memory()[0]
	mem2 = copy_value(mem)
	assert mem is not mem2
	assert id(mem) != id(mem2)
	assert mem == mem2
	assert not (mem != mem2)

	reg = info.used_registers()[0]
	reg2 = copy_value(reg)
	assert reg is not reg2
	assert id(reg) != id(reg2)
	assert reg == reg2
	assert not (reg != reg2)

def test_op_access_raise():
	instr = Decoder(64, b"\xC4\xE3\x49\x48\x10\x41").decode()
	factory = InstructionInfoFactory()
	info = factory.info(instr)

	info.op_access(4)
	with pytest.raises(ValueError):
		info.op_access(123)
