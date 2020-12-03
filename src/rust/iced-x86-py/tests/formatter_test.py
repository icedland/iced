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

FMT_SYNTAXES = [
	FormatterSyntax.GAS,
	FormatterSyntax.INTEL,
	FormatterSyntax.MASM,
	FormatterSyntax.NASM,
]

def test_invalid_syntax():
	with pytest.raises(ValueError):
		Formatter(0x12345)

@pytest.mark.parametrize("syntax", FMT_SYNTAXES)
def test_invalid_op_access_arg(syntax):
	instr = Decoder(64, b"\x62\xF2\x4F\xDD\x72\x50\x01").decode()
	formatter = Formatter(syntax)
	with pytest.raises(ValueError):
		formatter.op_access(instr, 100)

@pytest.mark.parametrize("syntax", FMT_SYNTAXES)
def test_invalid_get_instruction_operand_arg(syntax):
	instr = Decoder(64, b"\x62\xF2\x4F\xDD\x72\x50\x01").decode()
	formatter = Formatter(syntax)
	with pytest.raises(ValueError):
		formatter.get_instruction_operand(instr, 100)

@pytest.mark.parametrize("syntax", FMT_SYNTAXES)
def test_invalid_get_formatter_operand_arg(syntax):
	instr = Decoder(64, b"\x62\xF2\x4F\xDD\x72\x50\x01").decode()
	formatter = Formatter(syntax)
	with pytest.raises(ValueError):
		formatter.get_formatter_operand(instr, 100)

@pytest.mark.parametrize("syntax", FMT_SYNTAXES)
def test_invalid_format_operand_arg(syntax):
	instr = Decoder(64, b"\x62\xF2\x4F\xDD\x72\x50\x01").decode()
	formatter = Formatter(syntax)
	with pytest.raises(ValueError):
		formatter.format_operand(instr, 100)

@pytest.mark.parametrize("syntax", FMT_SYNTAXES)
def test_number_base(syntax):
	for base in range(0, 20):
		formatter = Formatter(syntax)
		if base == 2 or base == 8 or base == 10 or base == 16:
			assert formatter.number_base == 16
			formatter.number_base = base
			assert formatter.number_base == base
		else:
			assert formatter.number_base == 16
			with pytest.raises(ValueError):
				formatter.number_base = base
			assert formatter.number_base == 16
