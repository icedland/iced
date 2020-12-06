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

# pylint: disable=line-too-long
# pylint: disable=no-name-in-module

"""
iced-x86 is a high performance and correct x86/x64 disassembler, assembler and instruction decoder written in Rust with Python bindings
"""

from ._iced_x86_py import BlockEncoder, ConstantOffsets, Decoder, Encoder, FastFormatter, \
	Formatter, FpuStackIncrementInfo, Instruction, InstructionInfo, \
	InstructionInfoFactory, OpCodeInfo, UsedMemory, UsedRegister

from . import CC_a
from . import CC_ae
from . import CC_b
from . import CC_be
from . import CC_e
from . import CC_g
from . import CC_ge
from . import CC_l
from . import CC_le
from . import CC_ne
from . import CC_np
from . import CC_p
from . import Code
from . import CodeSize
from . import ConditionCode
from . import CpuidFeature
from . import DecoderError
from . import DecoderOptions
from . import EncodingKind
from . import FlowControl
from . import FormatMnemonicOptions
from . import FormatterSyntax
from . import MandatoryPrefix
from . import MemorySize
from . import MemorySizeOptions
from . import Mnemonic
from . import OpAccess
from . import OpCodeOperandKind
from . import OpCodeTableKind
from . import OpKind
from . import Register
from . import RflagsBits
from . import RoundingControl
from . import TupleType

__all__ = [
	"BlockEncoder",
	"ConstantOffsets",
	"Decoder",
	"Encoder",
	"FastFormatter",
	"Formatter",
	"FpuStackIncrementInfo",
	"Instruction",
	"InstructionInfo",
	"InstructionInfoFactory",
	"OpCodeInfo",
	"UsedMemory",
	"UsedRegister",

	"CC_a",
	"CC_ae",
	"CC_b",
	"CC_be",
	"CC_e",
	"CC_g",
	"CC_ge",
	"CC_l",
	"CC_le",
	"CC_ne",
	"CC_np",
	"CC_p",
	"Code",
	"CodeSize",
	"ConditionCode",
	"CpuidFeature",
	"DecoderError",
	"DecoderOptions",
	"EncodingKind",
	"FlowControl",
	"FormatMnemonicOptions",
	"FormatterSyntax",
	"MandatoryPrefix",
	"MemorySize",
	"MemorySizeOptions",
	"Mnemonic",
	"OpAccess",
	"OpCodeOperandKind",
	"OpCodeTableKind",
	"OpKind",
	"Register",
	"RflagsBits",
	"RoundingControl",
	"TupleType",
]
