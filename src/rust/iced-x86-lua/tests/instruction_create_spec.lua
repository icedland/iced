-- SPDX-License-Identifier: MIT
-- Copyright (C) 2018-present iced project and contributors

local from_hex = require("iced_test_utils").from_hex
local has_int64 = require("iced_test_utils").has_int64

describe("Instruction: create", function()
	local Code = require("iced_x86.Code")
	local CodeSize = require("iced_x86.CodeSize")
	local Decoder = require("iced_x86.Decoder")
	local DecoderOptions = require("iced_x86.DecoderOptions")
	local Encoder = require("iced_x86.Encoder")
	local Instruction = require("iced_x86.Instruction")
	local MemoryOperand = require("iced_x86.MemoryOperand")
	local Register = require("iced_x86.Register")
	local RepPrefixKind = require("iced_x86.RepPrefixKind")

	it("db", function()
		-- stylua: ignore
		local tests = {
			{ Instruction.db(from_hex("77")), from_hex("77") },
			{ Instruction.db(from_hex("77A9")), from_hex("77A9") },
			{ Instruction.db(from_hex("77A9CE")), from_hex("77A9CE") },
			{ Instruction.db(from_hex("77A9CE9D")), from_hex("77A9CE9D") },
			{ Instruction.db(from_hex("77A9CE9D55")), from_hex("77A9CE9D55") },
			{ Instruction.db(from_hex("77A9CE9D5505")), from_hex("77A9CE9D5505") },
			{ Instruction.db(from_hex("77A9CE9D550542")), from_hex("77A9CE9D550542") },
			{ Instruction.db(from_hex("77A9CE9D5505426C")), from_hex("77A9CE9D5505426C") },
			{ Instruction.db(from_hex("77A9CE9D5505426C86")), from_hex("77A9CE9D5505426C86") },
			{ Instruction.db(from_hex("77A9CE9D5505426C8632")), from_hex("77A9CE9D5505426C8632") },
			{ Instruction.db(from_hex("77A9CE9D5505426C8632FE")), from_hex("77A9CE9D5505426C8632FE") },
			{ Instruction.db(from_hex("77A9CE9D5505426C8632FE4F")), from_hex("77A9CE9D5505426C8632FE4F") },
			{ Instruction.db(from_hex("77A9CE9D5505426C8632FE4F34")), from_hex("77A9CE9D5505426C8632FE4F34") },
			{ Instruction.db(from_hex("77A9CE9D5505426C8632FE4F3427")), from_hex("77A9CE9D5505426C8632FE4F3427") },
			{ Instruction.db(from_hex("77A9CE9D5505426C8632FE4F3427AA")), from_hex("77A9CE9D5505426C8632FE4F3427AA") },
			{ Instruction.db(from_hex("77A9CE9D5505426C8632FE4F3427AA08")), from_hex("77A9CE9D5505426C8632FE4F3427AA08") },

			{ Instruction.db(0x77), from_hex("77") },
			{ Instruction.db(0x77, 0xA9), from_hex("77A9") },
			{ Instruction.db(0x77, 0xA9, 0xCE), from_hex("77A9CE") },
			{ Instruction.db(0x77, 0xA9, 0xCE, 0x9D), from_hex("77A9CE9D") },
			{ Instruction.db(0x77, 0xA9, 0xCE, 0x9D, 0x55), from_hex("77A9CE9D55") },
			{ Instruction.db(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05), from_hex("77A9CE9D5505") },
			{ Instruction.db(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42), from_hex("77A9CE9D550542") },
			{ Instruction.db(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C), from_hex("77A9CE9D5505426C") },
			{ Instruction.db(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86), from_hex("77A9CE9D5505426C86") },
			{ Instruction.db(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32), from_hex("77A9CE9D5505426C8632") },
			{ Instruction.db(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE), from_hex("77A9CE9D5505426C8632FE") },
			{ Instruction.db(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F), from_hex("77A9CE9D5505426C8632FE4F") },
			{ Instruction.db(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34), from_hex("77A9CE9D5505426C8632FE4F34") },
			{ Instruction.db(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27), from_hex("77A9CE9D5505426C8632FE4F3427") },
			{ Instruction.db(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA), from_hex("77A9CE9D5505426C8632FE4F3427AA") },
			{ Instruction.db(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08), from_hex("77A9CE9D5505426C8632FE4F3427AA08") },

			{ Instruction.db(-0x80), from_hex("80") },
			{ Instruction.db(0xFF, -0x80), from_hex("FF80") },
			{ Instruction.db(0xFE, 0xFF, -0x80), from_hex("FEFF80") },
			{ Instruction.db(0xFD, 0xFE, 0xFF, -0x80), from_hex("FDFEFF80") },
			{ Instruction.db(0xFC, 0xFD, 0xFE, 0xFF, -0x80), from_hex("FCFDFEFF80") },
			{ Instruction.db(0xFB, 0xFC, 0xFD, 0xFE, 0xFF, -0x80), from_hex("FBFCFDFEFF80") },
			{ Instruction.db(0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, -0x80), from_hex("FAFBFCFDFEFF80") },
			{ Instruction.db(0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, -0x80), from_hex("F9FAFBFCFDFEFF80") },
			{ Instruction.db(0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, -0x80), from_hex("F8F9FAFBFCFDFEFF80") },
			{ Instruction.db(0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, -0x80), from_hex("F7F8F9FAFBFCFDFEFF80") },
			{ Instruction.db(0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, -0x80), from_hex("F6F7F8F9FAFBFCFDFEFF80") },
			{ Instruction.db(0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, -0x80), from_hex("F5F6F7F8F9FAFBFCFDFEFF80") },
			{ Instruction.db(0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, -0x80), from_hex("F4F5F6F7F8F9FAFBFCFDFEFF80") },
			{ Instruction.db(0xF3, 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, -0x80), from_hex("F3F4F5F6F7F8F9FAFBFCFDFEFF80") },
			{ Instruction.db(0xF2, 0xF3, 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, -0x80), from_hex("F2F3F4F5F6F7F8F9FAFBFCFDFEFF80") },
			{ Instruction.db(0xF1, 0xF2, 0xF3, 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, -0x80), from_hex("F1F2F3F4F5F6F7F8F9FAFBFCFDFEFF80") },
			{ Instruction.db(0xF0, 0xF1, 0xF2, 0xF3, 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF), from_hex("F0F1F2F3F4F5F6F7F8F9FAFBFCFDFEFF") },

			{ Instruction.db(0xFF), from_hex("FF") },
			{ Instruction.db(0x7F, 0xFF), from_hex("7FFF") },
			{ Instruction.db(0xFE, 0x7F, 0xFF), from_hex("FE7FFF") },
			{ Instruction.db(0xFD, 0xFE, 0x7F, 0xFF), from_hex("FDFE7FFF") },
			{ Instruction.db(0xFC, 0xFD, 0xFE, 0x7F, 0xFF), from_hex("FCFDFE7FFF") },
			{ Instruction.db(0xFB, 0xFC, 0xFD, 0xFE, 0x7F, 0xFF), from_hex("FBFCFDFE7FFF") },
			{ Instruction.db(0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0x7F, 0xFF), from_hex("FAFBFCFDFE7FFF") },
			{ Instruction.db(0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0x7F, 0xFF), from_hex("F9FAFBFCFDFE7FFF") },
			{ Instruction.db(0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0x7F, 0xFF), from_hex("F8F9FAFBFCFDFE7FFF") },
			{ Instruction.db(0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0x7F, 0xFF), from_hex("F7F8F9FAFBFCFDFE7FFF") },
			{ Instruction.db(0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0x7F, 0xFF), from_hex("F6F7F8F9FAFBFCFDFE7FFF") },
			{ Instruction.db(0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0x7F, 0xFF), from_hex("F5F6F7F8F9FAFBFCFDFE7FFF") },
			{ Instruction.db(0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0x7F, 0xFF), from_hex("F4F5F6F7F8F9FAFBFCFDFE7FFF") },
			{ Instruction.db(0xF3, 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0x7F, 0xFF), from_hex("F3F4F5F6F7F8F9FAFBFCFDFE7FFF") },
			{ Instruction.db(0xF2, 0xF3, 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0x7F, 0xFF), from_hex("F2F3F4F5F6F7F8F9FAFBFCFDFE7FFF") },
			{ Instruction.db(0xF1, 0xF2, 0xF3, 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0x7F, 0xFF), from_hex("F1F2F3F4F5F6F7F8F9FAFBFCFDFE7FFF") },

			{ Instruction.db({ 0x77 }), from_hex("77") },
			{ Instruction.db({ 0x77, 0xA9 }), from_hex("77A9") },
			{ Instruction.db({ 0x77, 0xA9, 0xCE }), from_hex("77A9CE") },
			{ Instruction.db({ 0x77, 0xA9, 0xCE, 0x9D }), from_hex("77A9CE9D") },
			{ Instruction.db({ 0x77, 0xA9, 0xCE, 0x9D, 0x55 }), from_hex("77A9CE9D55") },
			{ Instruction.db({ 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05 }), from_hex("77A9CE9D5505") },
			{ Instruction.db({ 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42 }), from_hex("77A9CE9D550542") },
			{ Instruction.db({ 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C }), from_hex("77A9CE9D5505426C") },
			{ Instruction.db({ 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86 }), from_hex("77A9CE9D5505426C86") },
			{ Instruction.db({ 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32 }), from_hex("77A9CE9D5505426C8632") },
			{ Instruction.db({ 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE }), from_hex("77A9CE9D5505426C8632FE") },
			{ Instruction.db({ 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F }), from_hex("77A9CE9D5505426C8632FE4F") },
			{ Instruction.db({ 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34 }), from_hex("77A9CE9D5505426C8632FE4F34") },
			{ Instruction.db({ 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27 }), from_hex("77A9CE9D5505426C8632FE4F3427") },
			{ Instruction.db({ 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA }), from_hex("77A9CE9D5505426C8632FE4F3427AA") },
			{ Instruction.db({ 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08 }), from_hex("77A9CE9D5505426C8632FE4F3427AA08") },

			{ Instruction.db({ -0x80 }), from_hex("80") },
			{ Instruction.db({ 0xFF, -0x80 }), from_hex("FF80") },
			{ Instruction.db({ 0xFE, 0xFF, -0x80 }), from_hex("FEFF80") },
			{ Instruction.db({ 0xFD, 0xFE, 0xFF, -0x80 }), from_hex("FDFEFF80") },
			{ Instruction.db({ 0xFC, 0xFD, 0xFE, 0xFF, -0x80 }), from_hex("FCFDFEFF80") },
			{ Instruction.db({ 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, -0x80 }), from_hex("FBFCFDFEFF80") },
			{ Instruction.db({ 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, -0x80 }), from_hex("FAFBFCFDFEFF80") },
			{ Instruction.db({ 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, -0x80 }), from_hex("F9FAFBFCFDFEFF80") },
			{ Instruction.db({ 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, -0x80 }), from_hex("F8F9FAFBFCFDFEFF80") },
			{ Instruction.db({ 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, -0x80 }), from_hex("F7F8F9FAFBFCFDFEFF80") },
			{ Instruction.db({ 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, -0x80 }), from_hex("F6F7F8F9FAFBFCFDFEFF80") },
			{ Instruction.db({ 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, -0x80 }), from_hex("F5F6F7F8F9FAFBFCFDFEFF80") },
			{ Instruction.db({ 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, -0x80 }), from_hex("F4F5F6F7F8F9FAFBFCFDFEFF80") },
			{ Instruction.db({ 0xF3, 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, -0x80 }), from_hex("F3F4F5F6F7F8F9FAFBFCFDFEFF80") },
			{ Instruction.db({ 0xF2, 0xF3, 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, -0x80 }), from_hex("F2F3F4F5F6F7F8F9FAFBFCFDFEFF80") },
			{ Instruction.db({ 0xF1, 0xF2, 0xF3, 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, -0x80 }), from_hex("F1F2F3F4F5F6F7F8F9FAFBFCFDFEFF80") },
			{ Instruction.db({ 0xF0, 0xF1, 0xF2, 0xF3, 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF }), from_hex("F0F1F2F3F4F5F6F7F8F9FAFBFCFDFEFF") },

			{ Instruction.db({ 0xFF }), from_hex("FF") },
			{ Instruction.db({ 0x7F, 0xFF }), from_hex("7FFF") },
			{ Instruction.db({ 0xFE, 0x7F, 0xFF }), from_hex("FE7FFF") },
			{ Instruction.db({ 0xFD, 0xFE, 0x7F, 0xFF }), from_hex("FDFE7FFF") },
			{ Instruction.db({ 0xFC, 0xFD, 0xFE, 0x7F, 0xFF }), from_hex("FCFDFE7FFF") },
			{ Instruction.db({ 0xFB, 0xFC, 0xFD, 0xFE, 0x7F, 0xFF }), from_hex("FBFCFDFE7FFF") },
			{ Instruction.db({ 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0x7F, 0xFF }), from_hex("FAFBFCFDFE7FFF") },
			{ Instruction.db({ 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0x7F, 0xFF }), from_hex("F9FAFBFCFDFE7FFF") },
			{ Instruction.db({ 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0x7F, 0xFF }), from_hex("F8F9FAFBFCFDFE7FFF") },
			{ Instruction.db({ 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0x7F, 0xFF }), from_hex("F7F8F9FAFBFCFDFE7FFF") },
			{ Instruction.db({ 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0x7F, 0xFF }), from_hex("F6F7F8F9FAFBFCFDFE7FFF") },
			{ Instruction.db({ 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0x7F, 0xFF }), from_hex("F5F6F7F8F9FAFBFCFDFE7FFF") },
			{ Instruction.db({ 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0x7F, 0xFF }), from_hex("F4F5F6F7F8F9FAFBFCFDFE7FFF") },
			{ Instruction.db({ 0xF3, 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0x7F, 0xFF }), from_hex("F3F4F5F6F7F8F9FAFBFCFDFE7FFF") },
			{ Instruction.db({ 0xF2, 0xF3, 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0x7F, 0xFF }), from_hex("F2F3F4F5F6F7F8F9FAFBFCFDFE7FFF") },
			{ Instruction.db({ 0xF1, 0xF2, 0xF3, 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0x7F, 0xFF }), from_hex("F1F2F3F4F5F6F7F8F9FAFBFCFDFE7FFF") },
		}
		for _, tc in ipairs(tests) do
			local instr = tc[1]
			local expected = tc[2]
			assert.equals(Code.DeclareByte, instr:code())
			assert.equals(#expected, instr:declare_data_len())
			for i = 1, #expected do
				local value = expected:byte(i)
				assert.equals(value, instr:get_declare_byte_value(i - 1))
			end
		end
	end)

	-- stylua: ignore
	it("db: fail", function()
		assert.has_error(function() Instruction.db("") end)
									Instruction.db(from_hex("00"))
									Instruction.db(from_hex("0000"))
									Instruction.db(from_hex("000000"))
									Instruction.db(from_hex("00000000"))
									Instruction.db(from_hex("0000000000"))
									Instruction.db(from_hex("000000000000"))
									Instruction.db(from_hex("00000000000000"))
									Instruction.db(from_hex("0000000000000000"))
									Instruction.db(from_hex("000000000000000000"))
									Instruction.db(from_hex("00000000000000000000"))
									Instruction.db(from_hex("0000000000000000000000"))
									Instruction.db(from_hex("000000000000000000000000"))
									Instruction.db(from_hex("00000000000000000000000000"))
									Instruction.db(from_hex("0000000000000000000000000000"))
									Instruction.db(from_hex("000000000000000000000000000000"))
									Instruction.db(from_hex("00000000000000000000000000000000"))
		assert.has_error(function() Instruction.db(from_hex("0000000000000000000000000000000000")) end)

		assert.has_error(function() Instruction.db() end)
									Instruction.db(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08)
		assert.has_error(function() Instruction.db(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08, 0x00) end)

		assert.has_error(function() Instruction.db(-0x81) end)
		assert.has_error(function() Instruction.db(0xFF, -0x81) end)
		assert.has_error(function() Instruction.db(0xFE, 0xFF, -0x81) end)
		assert.has_error(function() Instruction.db(0xFD, 0xFE, 0xFF, -0x81) end)
		assert.has_error(function() Instruction.db(0xFC, 0xFD, 0xFE, 0xFF, -0x81) end)
		assert.has_error(function() Instruction.db(0xFB, 0xFC, 0xFD, 0xFE, 0xFF, -0x81) end)
		assert.has_error(function() Instruction.db(0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, -0x81) end)
		assert.has_error(function() Instruction.db(0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, -0x81) end)
		assert.has_error(function() Instruction.db(0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, -0x81) end)
		assert.has_error(function() Instruction.db(0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, -0x81) end)
		assert.has_error(function() Instruction.db(0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, -0x81) end)
		assert.has_error(function() Instruction.db(0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, -0x81) end)
		assert.has_error(function() Instruction.db(0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, -0x81) end)
		assert.has_error(function() Instruction.db(0xF3, 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, -0x81) end)
		assert.has_error(function() Instruction.db(0xF2, 0xF3, 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, -0x81) end)
		assert.has_error(function() Instruction.db(0xF1, 0xF2, 0xF3, 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, -0x81) end)

		assert.has_error(function() Instruction.db(0x100) end)
		assert.has_error(function() Instruction.db(0xFF, 0x100) end)
		assert.has_error(function() Instruction.db(0xFE, 0xFF, 0x100) end)
		assert.has_error(function() Instruction.db(0xFD, 0xFE, 0xFF, 0x100) end)
		assert.has_error(function() Instruction.db(0xFC, 0xFD, 0xFE, 0xFF, 0x100) end)
		assert.has_error(function() Instruction.db(0xFB, 0xFC, 0xFD, 0xFE, 0xFF, 0x100) end)
		assert.has_error(function() Instruction.db(0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, 0x100) end)
		assert.has_error(function() Instruction.db(0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, 0x100) end)
		assert.has_error(function() Instruction.db(0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, 0x100) end)
		assert.has_error(function() Instruction.db(0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, 0x100) end)
		assert.has_error(function() Instruction.db(0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, 0x100) end)
		assert.has_error(function() Instruction.db(0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, 0x100) end)
		assert.has_error(function() Instruction.db(0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, 0x100) end)
		assert.has_error(function() Instruction.db(0xF3, 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, 0x100) end)
		assert.has_error(function() Instruction.db(0xF2, 0xF3, 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, 0x100) end)
		assert.has_error(function() Instruction.db(0xF1, 0xF2, 0xF3, 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, 0x100) end)

		assert.has_error(function() Instruction.db({ }) end)
									Instruction.db({ 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08 })
		assert.has_error(function() Instruction.db({ 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08, 0x00 }) end)

		assert.has_error(function() Instruction.db({ -0x81 }) end)
		assert.has_error(function() Instruction.db({ 0xFF, -0x81 }) end)
		assert.has_error(function() Instruction.db({ 0xFE, 0xFF, -0x81 }) end)
		assert.has_error(function() Instruction.db({ 0xFD, 0xFE, 0xFF, -0x81 }) end)
		assert.has_error(function() Instruction.db({ 0xFC, 0xFD, 0xFE, 0xFF, -0x81 }) end)
		assert.has_error(function() Instruction.db({ 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, -0x81 }) end)
		assert.has_error(function() Instruction.db({ 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, -0x81 }) end)
		assert.has_error(function() Instruction.db({ 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, -0x81 }) end)
		assert.has_error(function() Instruction.db({ 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, -0x81 }) end)
		assert.has_error(function() Instruction.db({ 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, -0x81 }) end)
		assert.has_error(function() Instruction.db({ 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, -0x81 }) end)
		assert.has_error(function() Instruction.db({ 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, -0x81 }) end)
		assert.has_error(function() Instruction.db({ 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, -0x81 }) end)
		assert.has_error(function() Instruction.db({ 0xF3, 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, -0x81 }) end)
		assert.has_error(function() Instruction.db({ 0xF2, 0xF3, 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, -0x81 }) end)
		assert.has_error(function() Instruction.db({ 0xF1, 0xF2, 0xF3, 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, -0x81 }) end)

		assert.has_error(function() Instruction.db({ 0x100 }) end)
		assert.has_error(function() Instruction.db({ 0xFF, 0x100 }) end)
		assert.has_error(function() Instruction.db({ 0xFE, 0xFF, 0x100 }) end)
		assert.has_error(function() Instruction.db({ 0xFD, 0xFE, 0xFF, 0x100 }) end)
		assert.has_error(function() Instruction.db({ 0xFC, 0xFD, 0xFE, 0xFF, 0x100 }) end)
		assert.has_error(function() Instruction.db({ 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, 0x100 }) end)
		assert.has_error(function() Instruction.db({ 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, 0x100 }) end)
		assert.has_error(function() Instruction.db({ 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, 0x100 }) end)
		assert.has_error(function() Instruction.db({ 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, 0x100 }) end)
		assert.has_error(function() Instruction.db({ 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, 0x100 }) end)
		assert.has_error(function() Instruction.db({ 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, 0x100 }) end)
		assert.has_error(function() Instruction.db({ 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, 0x100 }) end)
		assert.has_error(function() Instruction.db({ 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, 0x100 }) end)
		assert.has_error(function() Instruction.db({ 0xF3, 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, 0x100 }) end)
		assert.has_error(function() Instruction.db({ 0xF2, 0xF3, 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, 0x100 }) end)
		assert.has_error(function() Instruction.db({ 0xF1, 0xF2, 0xF3, 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, 0x100 }) end)

		assert.has_error(function() Instruction.db(true) end)
		assert.has_error(function() Instruction.db(0xFF, true) end)
		assert.has_error(function() Instruction.db(0xFE, 0xFF, true) end)
		assert.has_error(function() Instruction.db(0xFD, 0xFE, 0xFF, true) end)
		assert.has_error(function() Instruction.db(0xFC, 0xFD, 0xFE, 0xFF, true) end)
		assert.has_error(function() Instruction.db(0xFB, 0xFC, 0xFD, 0xFE, 0xFF, true) end)
		assert.has_error(function() Instruction.db(0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, true) end)
		assert.has_error(function() Instruction.db(0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, true) end)
		assert.has_error(function() Instruction.db(0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, true) end)
		assert.has_error(function() Instruction.db(0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, true) end)
		assert.has_error(function() Instruction.db(0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, true) end)
		assert.has_error(function() Instruction.db(0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, true) end)
		assert.has_error(function() Instruction.db(0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, true) end)
		assert.has_error(function() Instruction.db(0xF3, 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, true) end)
		assert.has_error(function() Instruction.db(0xF2, 0xF3, 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, true) end)
		assert.has_error(function() Instruction.db(0xF1, 0xF2, 0xF3, 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, true) end)

		assert.has_error(function() Instruction.db({ true }) end)
		assert.has_error(function() Instruction.db({ 0xFF, true }) end)
		assert.has_error(function() Instruction.db({ 0xFE, 0xFF, true }) end)
		assert.has_error(function() Instruction.db({ 0xFD, 0xFE, 0xFF, true }) end)
		assert.has_error(function() Instruction.db({ 0xFC, 0xFD, 0xFE, 0xFF, true }) end)
		assert.has_error(function() Instruction.db({ 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, true }) end)
		assert.has_error(function() Instruction.db({ 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, true }) end)
		assert.has_error(function() Instruction.db({ 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, true }) end)
		assert.has_error(function() Instruction.db({ 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, true }) end)
		assert.has_error(function() Instruction.db({ 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, true }) end)
		assert.has_error(function() Instruction.db({ 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, true }) end)
		assert.has_error(function() Instruction.db({ 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, true }) end)
		assert.has_error(function() Instruction.db({ 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, true }) end)
		assert.has_error(function() Instruction.db({ 0xF3, 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, true }) end)
		assert.has_error(function() Instruction.db({ 0xF2, 0xF3, 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, true }) end)
		assert.has_error(function() Instruction.db({ 0xF1, 0xF2, 0xF3, 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF, true }) end)

									Instruction.db(from_hex("77"))
		assert.has_error(function() Instruction.db(from_hex("77"), "") end)

									Instruction.db({ 0x77 })
		assert.has_error(function() Instruction.db({ 0x77 }, {}) end)
	end)

	it("dw", function()
		-- stylua: ignore
		local tests = {
			{ Instruction.dw(from_hex("A977")), { 0x77A9 } },
			{ Instruction.dw(from_hex("A9779DCE")), { 0x77A9, 0xCE9D } },
			{ Instruction.dw(from_hex("A9779DCE0555")), { 0x77A9, 0xCE9D, 0x5505 } },
			{ Instruction.dw(from_hex("A9779DCE05556C42")), { 0x77A9, 0xCE9D, 0x5505, 0x426C } },
			{ Instruction.dw(from_hex("A9779DCE05556C423286")), { 0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632 } },
			{ Instruction.dw(from_hex("A9779DCE05556C4232864FFE")), { 0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F } },
			{ Instruction.dw(from_hex("A9779DCE05556C4232864FFE2734")), { 0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427 } },
			{ Instruction.dw(from_hex("A9779DCE05556C4232864FFE273408AA")), { 0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xAA08 } },

			{ Instruction.dw(0x77A9), { 0x77A9 } },
			{ Instruction.dw(0x77A9, 0xCE9D), { 0x77A9, 0xCE9D } },
			{ Instruction.dw(0x77A9, 0xCE9D, 0x5505), { 0x77A9, 0xCE9D, 0x5505 } },
			{ Instruction.dw(0x77A9, 0xCE9D, 0x5505, 0x426C), { 0x77A9, 0xCE9D, 0x5505, 0x426C } },
			{ Instruction.dw(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632), { 0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632 } },
			{ Instruction.dw(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F), { 0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F } },
			{ Instruction.dw(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427), { 0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427 } },
			{ Instruction.dw(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xAA08), { 0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xAA08 } },

			{ Instruction.dw(-0x8000), { 0x8000 } },
			{ Instruction.dw(0xFFFF, -0x8000), { 0xFFFF, 0x8000 } },
			{ Instruction.dw(0xFFFE, 0xFFFF, -0x8000), { 0xFFFE, 0xFFFF, 0x8000 } },
			{ Instruction.dw(0xFFFD, 0xFFFE, 0xFFFF, -0x8000), { 0xFFFD, 0xFFFE, 0xFFFF, 0x8000 } },
			{ Instruction.dw(0xFFFC, 0xFFFD, 0xFFFE, 0xFFFF, -0x8000), { 0xFFFC, 0xFFFD, 0xFFFE, 0xFFFF, 0x8000 } },
			{ Instruction.dw(0xFFFB, 0xFFFC, 0xFFFD, 0xFFFE, 0xFFFF, -0x8000), { 0xFFFB, 0xFFFC, 0xFFFD, 0xFFFE, 0xFFFF, 0x8000 } },
			{ Instruction.dw(0xFFFA, 0xFFFB, 0xFFFC, 0xFFFD, 0xFFFE, 0xFFFF, -0x8000), { 0xFFFA, 0xFFFB, 0xFFFC, 0xFFFD, 0xFFFE, 0xFFFF, 0x8000 } },
			{ Instruction.dw(0xFFF9, 0xFFFA, 0xFFFB, 0xFFFC, 0xFFFD, 0xFFFE, 0xFFFF, -0x8000), { 0xFFF9, 0xFFFA, 0xFFFB, 0xFFFC, 0xFFFD, 0xFFFE, 0xFFFF, 0x8000 } },
			{ Instruction.dw(0xFFF8, 0xFFF9, 0xFFFA, 0xFFFB, 0xFFFC, 0xFFFD, 0xFFFE, 0xFFFF), { 0xFFF8, 0xFFF9, 0xFFFA, 0xFFFB, 0xFFFC, 0xFFFD, 0xFFFE, 0xFFFF } },

			{ Instruction.dw(0xFFFF), { 0xFFFF } },
			{ Instruction.dw(0x7FFF, 0xFFFF), { 0x7FFF, 0xFFFF } },
			{ Instruction.dw(0xFFFE, 0x7FFF, 0xFFFF), { 0xFFFE, 0x7FFF, 0xFFFF } },
			{ Instruction.dw(0xFFFD, 0xFFFE, 0x7FFF, 0xFFFF), { 0xFFFD, 0xFFFE, 0x7FFF, 0xFFFF } },
			{ Instruction.dw(0xFFFC, 0xFFFD, 0xFFFE, 0x7FFF, 0xFFFF), { 0xFFFC, 0xFFFD, 0xFFFE, 0x7FFF, 0xFFFF } },
			{ Instruction.dw(0xFFFB, 0xFFFC, 0xFFFD, 0xFFFE, 0x7FFF, 0xFFFF), { 0xFFFB, 0xFFFC, 0xFFFD, 0xFFFE, 0x7FFF, 0xFFFF } },
			{ Instruction.dw(0xFFFA, 0xFFFB, 0xFFFC, 0xFFFD, 0xFFFE, 0x7FFF, 0xFFFF), { 0xFFFA, 0xFFFB, 0xFFFC, 0xFFFD, 0xFFFE, 0x7FFF, 0xFFFF } },
			{ Instruction.dw(0xFFF9, 0xFFFA, 0xFFFB, 0xFFFC, 0xFFFD, 0xFFFE, 0x7FFF, 0xFFFF), { 0xFFF9, 0xFFFA, 0xFFFB, 0xFFFC, 0xFFFD, 0xFFFE, 0x7FFF, 0xFFFF } },

			{ Instruction.dw({ 0x77A9 }), { 0x77A9 } },
			{ Instruction.dw({ 0x77A9, 0xCE9D }), { 0x77A9, 0xCE9D } },
			{ Instruction.dw({ 0x77A9, 0xCE9D, 0x5505 }), { 0x77A9, 0xCE9D, 0x5505 } },
			{ Instruction.dw({ 0x77A9, 0xCE9D, 0x5505, 0x426C }), { 0x77A9, 0xCE9D, 0x5505, 0x426C } },
			{ Instruction.dw({ 0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632 }), { 0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632 } },
			{ Instruction.dw({ 0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F }), { 0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F } },
			{ Instruction.dw({ 0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427 }), { 0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427 } },
			{ Instruction.dw({ 0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xAA08 }), { 0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xAA08 } },

			{ Instruction.dw({ -0x8000 }), { 0x8000 } },
			{ Instruction.dw({ 0xFFFF, -0x8000 }), { 0xFFFF, 0x8000 } },
			{ Instruction.dw({ 0xFFFE, 0xFFFF, -0x8000 }), { 0xFFFE, 0xFFFF, 0x8000 } },
			{ Instruction.dw({ 0xFFFD, 0xFFFE, 0xFFFF, -0x8000 }), { 0xFFFD, 0xFFFE, 0xFFFF, 0x8000 } },
			{ Instruction.dw({ 0xFFFC, 0xFFFD, 0xFFFE, 0xFFFF, -0x8000 }), { 0xFFFC, 0xFFFD, 0xFFFE, 0xFFFF, 0x8000 } },
			{ Instruction.dw({ 0xFFFB, 0xFFFC, 0xFFFD, 0xFFFE, 0xFFFF, -0x8000 }), { 0xFFFB, 0xFFFC, 0xFFFD, 0xFFFE, 0xFFFF, 0x8000 } },
			{ Instruction.dw({ 0xFFFA, 0xFFFB, 0xFFFC, 0xFFFD, 0xFFFE, 0xFFFF, -0x8000 }), { 0xFFFA, 0xFFFB, 0xFFFC, 0xFFFD, 0xFFFE, 0xFFFF, 0x8000 } },
			{ Instruction.dw({ 0xFFF9, 0xFFFA, 0xFFFB, 0xFFFC, 0xFFFD, 0xFFFE, 0xFFFF, -0x8000 }), { 0xFFF9, 0xFFFA, 0xFFFB, 0xFFFC, 0xFFFD, 0xFFFE, 0xFFFF, 0x8000 } },
			{ Instruction.dw({ 0xFFF8, 0xFFF9, 0xFFFA, 0xFFFB, 0xFFFC, 0xFFFD, 0xFFFE, 0xFFFF }), { 0xFFF8, 0xFFF9, 0xFFFA, 0xFFFB, 0xFFFC, 0xFFFD, 0xFFFE, 0xFFFF } },

			{ Instruction.dw({ 0xFFFF }), { 0xFFFF } },
			{ Instruction.dw({ 0x7FFF, 0xFFFF }), { 0x7FFF, 0xFFFF } },
			{ Instruction.dw({ 0xFFFE, 0x7FFF, 0xFFFF }), { 0xFFFE, 0x7FFF, 0xFFFF } },
			{ Instruction.dw({ 0xFFFD, 0xFFFE, 0x7FFF, 0xFFFF }), { 0xFFFD, 0xFFFE, 0x7FFF, 0xFFFF } },
			{ Instruction.dw({ 0xFFFC, 0xFFFD, 0xFFFE, 0x7FFF, 0xFFFF }), { 0xFFFC, 0xFFFD, 0xFFFE, 0x7FFF, 0xFFFF } },
			{ Instruction.dw({ 0xFFFB, 0xFFFC, 0xFFFD, 0xFFFE, 0x7FFF, 0xFFFF }), { 0xFFFB, 0xFFFC, 0xFFFD, 0xFFFE, 0x7FFF, 0xFFFF } },
			{ Instruction.dw({ 0xFFFA, 0xFFFB, 0xFFFC, 0xFFFD, 0xFFFE, 0x7FFF, 0xFFFF }), { 0xFFFA, 0xFFFB, 0xFFFC, 0xFFFD, 0xFFFE, 0x7FFF, 0xFFFF } },
			{ Instruction.dw({ 0xFFF9, 0xFFFA, 0xFFFB, 0xFFFC, 0xFFFD, 0xFFFE, 0x7FFF, 0xFFFF }), { 0xFFF9, 0xFFFA, 0xFFFB, 0xFFFC, 0xFFFD, 0xFFFE, 0x7FFF, 0xFFFF } },
		}
		for _, tc in ipairs(tests) do
			local instr = tc[1]
			local expected = tc[2]
			assert.equals(Code.DeclareWord, instr:code())
			assert.equals(#expected, instr:declare_data_len())
			for i, value in ipairs(expected) do
				assert.equals(value, instr:get_declare_word_value(i - 1))
			end
		end
	end)

	-- stylua: ignore
	it("dw: fail", function()
		assert.has_error(function() Instruction.dw("") end)
		assert.has_error(function() Instruction.dw(from_hex("00")) end)
									Instruction.dw(from_hex("0000"))
		assert.has_error(function() Instruction.dw(from_hex("000000")) end)
									Instruction.dw(from_hex("00000000"))
		assert.has_error(function() Instruction.dw(from_hex("0000000000")) end)
									Instruction.dw(from_hex("000000000000"))
		assert.has_error(function() Instruction.dw(from_hex("00000000000000")) end)
									Instruction.dw(from_hex("0000000000000000"))
		assert.has_error(function() Instruction.dw(from_hex("000000000000000000")) end)
									Instruction.dw(from_hex("00000000000000000000"))
		assert.has_error(function() Instruction.dw(from_hex("0000000000000000000000")) end)
									Instruction.dw(from_hex("000000000000000000000000"))
		assert.has_error(function() Instruction.dw(from_hex("00000000000000000000000000")) end)
									Instruction.dw(from_hex("0000000000000000000000000000"))
		assert.has_error(function() Instruction.dw(from_hex("000000000000000000000000000000")) end)
									Instruction.dw(from_hex("00000000000000000000000000000000"))
		assert.has_error(function() Instruction.dw(from_hex("0000000000000000000000000000000000")) end)
		assert.has_error(function() Instruction.dw(from_hex("000000000000000000000000000000000000")) end)

		assert.has_error(function() Instruction.dw(-0x8001) end)
		assert.has_error(function() Instruction.dw(0xFFFF, -0x8001) end)
		assert.has_error(function() Instruction.dw(0xFFFE, 0xFFFF, -0x8001) end)
		assert.has_error(function() Instruction.dw(0xFFFD, 0xFFFE, 0xFFFF, -0x8001) end)
		assert.has_error(function() Instruction.dw(0xFFFC, 0xFFFD, 0xFFFE, 0xFFFF, -0x8001) end)
		assert.has_error(function() Instruction.dw(0xFFFB, 0xFFFC, 0xFFFD, 0xFFFE, 0xFFFF, -0x8001) end)
		assert.has_error(function() Instruction.dw(0xFFFA, 0xFFFB, 0xFFFC, 0xFFFD, 0xFFFE, 0xFFFF, -0x8001) end)
		assert.has_error(function() Instruction.dw(0xFFF9, 0xFFFA, 0xFFFB, 0xFFFC, 0xFFFD, 0xFFFE, 0xFFFF, -0x8001) end)

		assert.has_error(function() Instruction.dw(0x10000) end)
		assert.has_error(function() Instruction.dw(0xFFFF, 0x10000) end)
		assert.has_error(function() Instruction.dw(0xFFFE, 0xFFFF, 0x10000) end)
		assert.has_error(function() Instruction.dw(0xFFFD, 0xFFFE, 0xFFFF, 0x10000) end)
		assert.has_error(function() Instruction.dw(0xFFFC, 0xFFFD, 0xFFFE, 0xFFFF, 0x10000) end)
		assert.has_error(function() Instruction.dw(0xFFFB, 0xFFFC, 0xFFFD, 0xFFFE, 0xFFFF, 0x10000) end)
		assert.has_error(function() Instruction.dw(0xFFFA, 0xFFFB, 0xFFFC, 0xFFFD, 0xFFFE, 0xFFFF, 0x10000) end)
		assert.has_error(function() Instruction.dw(0xFFF9, 0xFFFA, 0xFFFB, 0xFFFC, 0xFFFD, 0xFFFE, 0xFFFF, 0x10000) end)

		assert.has_error(function() Instruction.dw({ -0x8001 }) end)
		assert.has_error(function() Instruction.dw({ 0xFFFF, -0x8001 }) end)
		assert.has_error(function() Instruction.dw({ 0xFFFE, 0xFFFF, -0x8001 }) end)
		assert.has_error(function() Instruction.dw({ 0xFFFD, 0xFFFE, 0xFFFF, -0x8001 }) end)
		assert.has_error(function() Instruction.dw({ 0xFFFC, 0xFFFD, 0xFFFE, 0xFFFF, -0x8001 }) end)
		assert.has_error(function() Instruction.dw({ 0xFFFB, 0xFFFC, 0xFFFD, 0xFFFE, 0xFFFF, -0x8001 }) end)
		assert.has_error(function() Instruction.dw({ 0xFFFA, 0xFFFB, 0xFFFC, 0xFFFD, 0xFFFE, 0xFFFF, -0x8001 }) end)
		assert.has_error(function() Instruction.dw({ 0xFFF9, 0xFFFA, 0xFFFB, 0xFFFC, 0xFFFD, 0xFFFE, 0xFFFF, -0x8001 }) end)

		assert.has_error(function() Instruction.dw({ 0x10000 }) end)
		assert.has_error(function() Instruction.dw({ 0xFFFF, 0x10000 }) end)
		assert.has_error(function() Instruction.dw({ 0xFFFE, 0xFFFF, 0x10000 }) end)
		assert.has_error(function() Instruction.dw({ 0xFFFD, 0xFFFE, 0xFFFF, 0x10000 }) end)
		assert.has_error(function() Instruction.dw({ 0xFFFC, 0xFFFD, 0xFFFE, 0xFFFF, 0x10000 }) end)
		assert.has_error(function() Instruction.dw({ 0xFFFB, 0xFFFC, 0xFFFD, 0xFFFE, 0xFFFF, 0x10000 }) end)
		assert.has_error(function() Instruction.dw({ 0xFFFA, 0xFFFB, 0xFFFC, 0xFFFD, 0xFFFE, 0xFFFF, 0x10000 }) end)
		assert.has_error(function() Instruction.dw({ 0xFFF9, 0xFFFA, 0xFFFB, 0xFFFC, 0xFFFD, 0xFFFE, 0xFFFF, 0x10000 }) end)

		assert.has_error(function() Instruction.dw(true) end)
		assert.has_error(function() Instruction.dw(0xFFFF, true) end)
		assert.has_error(function() Instruction.dw(0xFFFE, 0xFFFF, true) end)
		assert.has_error(function() Instruction.dw(0xFFFD, 0xFFFE, 0xFFFF, true) end)
		assert.has_error(function() Instruction.dw(0xFFFC, 0xFFFD, 0xFFFE, 0xFFFF, true) end)
		assert.has_error(function() Instruction.dw(0xFFFB, 0xFFFC, 0xFFFD, 0xFFFE, 0xFFFF, true) end)
		assert.has_error(function() Instruction.dw(0xFFFA, 0xFFFB, 0xFFFC, 0xFFFD, 0xFFFE, 0xFFFF, true) end)
		assert.has_error(function() Instruction.dw(0xFFF9, 0xFFFA, 0xFFFB, 0xFFFC, 0xFFFD, 0xFFFE, 0xFFFF, true) end)

		assert.has_error(function() Instruction.dw({ true }) end)
		assert.has_error(function() Instruction.dw({ 0xFFFF, true }) end)
		assert.has_error(function() Instruction.dw({ 0xFFFE, 0xFFFF, true }) end)
		assert.has_error(function() Instruction.dw({ 0xFFFD, 0xFFFE, 0xFFFF, true }) end)
		assert.has_error(function() Instruction.dw({ 0xFFFC, 0xFFFD, 0xFFFE, 0xFFFF, true }) end)
		assert.has_error(function() Instruction.dw({ 0xFFFB, 0xFFFC, 0xFFFD, 0xFFFE, 0xFFFF, true }) end)
		assert.has_error(function() Instruction.dw({ 0xFFFA, 0xFFFB, 0xFFFC, 0xFFFD, 0xFFFE, 0xFFFF, true }) end)
		assert.has_error(function() Instruction.dw({ 0xFFF9, 0xFFFA, 0xFFFB, 0xFFFC, 0xFFFD, 0xFFFE, 0xFFFF, true }) end)

									Instruction.dw(from_hex("7788"))
		assert.has_error(function() Instruction.dw(from_hex("7788"), "") end)

									Instruction.dw({ 0x7788 })
		assert.has_error(function() Instruction.dw({ 0x7788 }, {}) end)
	end)

	it("dd", function()
		-- stylua: ignore
		local tests = {
			{ Instruction.dd(from_hex("9DCEA977")), { 0x77A9CE9D } },
			{ Instruction.dd(from_hex("9DCEA9776C420555")), { 0x77A9CE9D, 0x5505426C } },
			{ Instruction.dd(from_hex("9DCEA9776C4205554FFE3286")), { 0x77A9CE9D, 0x5505426C, 0x8632FE4F } },
			{ Instruction.dd(from_hex("9DCEA9776C4205554FFE328608AA2734")), { 0x77A9CE9D, 0x5505426C, 0x8632FE4F, 0x3427AA08 } },

			{ Instruction.dd(0x77A9CE9D), { 0x77A9CE9D } },
			{ Instruction.dd(0x77A9CE9D, 0x5505426C), { 0x77A9CE9D, 0x5505426C } },
			{ Instruction.dd(0x77A9CE9D, 0x5505426C, 0x8632FE4F), { 0x77A9CE9D, 0x5505426C, 0x8632FE4F } },
			{ Instruction.dd(0x77A9CE9D, 0x5505426C, 0x8632FE4F, 0x3427AA08), { 0x77A9CE9D, 0x5505426C, 0x8632FE4F, 0x3427AA08 } },

			{ Instruction.dd(-0x80000000), { 0x80000000 } },
			{ Instruction.dd(0xFFFFFFFF, -0x80000000), { 0xFFFFFFFF, 0x80000000 } },
			{ Instruction.dd(0xFFFFFFFE, 0xFFFFFFFF, -0x80000000), { 0xFFFFFFFE, 0xFFFFFFFF, 0x80000000 } },
			{ Instruction.dd(0xFFFFFFFD, 0xFFFFFFFE, 0xFFFFFFFF, -0x80000000), { 0xFFFFFFFD, 0xFFFFFFFE, 0xFFFFFFFF, 0x80000000 } },
			{ Instruction.dd(0xFFFFFFFC, 0xFFFFFFFD, 0xFFFFFFFE, 0xFFFFFFFF), { 0xFFFFFFFC, 0xFFFFFFFD, 0xFFFFFFFE, 0xFFFFFFFF } },

			{ Instruction.dd(0xFFFFFFFF), { 0xFFFFFFFF } },
			{ Instruction.dd(0x7FFFFFFF, 0xFFFFFFFF), { 0x7FFFFFFF, 0xFFFFFFFF } },
			{ Instruction.dd(0xFFFFFFFE, 0x7FFFFFFF, 0xFFFFFFFF), { 0xFFFFFFFE, 0x7FFFFFFF, 0xFFFFFFFF } },
			{ Instruction.dd(0xFFFFFFFD, 0xFFFFFFFE, 0x7FFFFFFF, 0xFFFFFFFF), { 0xFFFFFFFD, 0xFFFFFFFE, 0x7FFFFFFF, 0xFFFFFFFF } },

			{ Instruction.dd({ 0x77A9CE9D }), { 0x77A9CE9D } },
			{ Instruction.dd({ 0x77A9CE9D, 0x5505426C }), { 0x77A9CE9D, 0x5505426C } },
			{ Instruction.dd({ 0x77A9CE9D, 0x5505426C, 0x8632FE4F }), { 0x77A9CE9D, 0x5505426C, 0x8632FE4F } },
			{ Instruction.dd({ 0x77A9CE9D, 0x5505426C, 0x8632FE4F, 0x3427AA08 }), { 0x77A9CE9D, 0x5505426C, 0x8632FE4F, 0x3427AA08 } },

			{ Instruction.dd({ -0x80000000 }), { 0x80000000 } },
			{ Instruction.dd({ 0xFFFFFFFF, -0x80000000 }), { 0xFFFFFFFF, 0x80000000 } },
			{ Instruction.dd({ 0xFFFFFFFE, 0xFFFFFFFF, -0x80000000 }), { 0xFFFFFFFE, 0xFFFFFFFF, 0x80000000 } },
			{ Instruction.dd({ 0xFFFFFFFD, 0xFFFFFFFE, 0xFFFFFFFF, -0x80000000 }), { 0xFFFFFFFD, 0xFFFFFFFE, 0xFFFFFFFF, 0x80000000 } },
			{ Instruction.dd({ 0xFFFFFFFC, 0xFFFFFFFD, 0xFFFFFFFE, 0xFFFFFFFF }), { 0xFFFFFFFC, 0xFFFFFFFD, 0xFFFFFFFE, 0xFFFFFFFF } },

			{ Instruction.dd({ 0xFFFFFFFF }), { 0xFFFFFFFF } },
			{ Instruction.dd({ 0x7FFFFFFF, 0xFFFFFFFF }), { 0x7FFFFFFF, 0xFFFFFFFF } },
			{ Instruction.dd({ 0xFFFFFFFE, 0x7FFFFFFF, 0xFFFFFFFF }), { 0xFFFFFFFE, 0x7FFFFFFF, 0xFFFFFFFF } },
			{ Instruction.dd({ 0xFFFFFFFD, 0xFFFFFFFE, 0x7FFFFFFF, 0xFFFFFFFF }), { 0xFFFFFFFD, 0xFFFFFFFE, 0x7FFFFFFF, 0xFFFFFFFF } },
		}
		for _, tc in ipairs(tests) do
			local instr = tc[1]
			local expected = tc[2]
			assert.equals(Code.DeclareDword, instr:code())
			assert.equals(#expected, instr:declare_data_len())
			for i, value in ipairs(expected) do
				assert.equals(value, instr:get_declare_dword_value(i - 1))
			end
		end
	end)

	-- stylua: ignore
	it("dd: fail", function()
		assert.has_error(function() Instruction.dd("") end)
		assert.has_error(function() Instruction.dd(from_hex("00")) end)
		assert.has_error(function() Instruction.dd(from_hex("0000")) end)
		assert.has_error(function() Instruction.dd(from_hex("000000")) end)
									Instruction.dd(from_hex("00000000"))
		assert.has_error(function() Instruction.dd(from_hex("0000000000")) end)
		assert.has_error(function() Instruction.dd(from_hex("000000000000")) end)
		assert.has_error(function() Instruction.dd(from_hex("00000000000000")) end)
									Instruction.dd(from_hex("0000000000000000"))
		assert.has_error(function() Instruction.dd(from_hex("000000000000000000")) end)
		assert.has_error(function() Instruction.dd(from_hex("00000000000000000000")) end)
		assert.has_error(function() Instruction.dd(from_hex("0000000000000000000000")) end)
									Instruction.dd(from_hex("000000000000000000000000"))
		assert.has_error(function() Instruction.dd(from_hex("00000000000000000000000000")) end)
		assert.has_error(function() Instruction.dd(from_hex("0000000000000000000000000000")) end)
		assert.has_error(function() Instruction.dd(from_hex("000000000000000000000000000000")) end)
									Instruction.dd(from_hex("00000000000000000000000000000000"))
		assert.has_error(function() Instruction.dd(from_hex("0000000000000000000000000000000000")) end)
		assert.has_error(function() Instruction.dd(from_hex("000000000000000000000000000000000000")) end)
		assert.has_error(function() Instruction.dd(from_hex("00000000000000000000000000000000000000")) end)
		assert.has_error(function() Instruction.dd(from_hex("0000000000000000000000000000000000000000")) end)

		assert.has_error(function() Instruction.dd(-0x80000001) end)
		assert.has_error(function() Instruction.dd(0xFFFFFFFF, -0x80000001) end)
		assert.has_error(function() Instruction.dd(0xFFFFFFFE, 0xFFFFFFFF, -0x80000001) end)
		assert.has_error(function() Instruction.dd(0xFFFFFFFD, 0xFFFFFFFE, 0xFFFFFFFF, -0x80000001) end)

		assert.has_error(function() Instruction.dd(0x100000000) end)
		assert.has_error(function() Instruction.dd(0xFFFFFFFF, 0x100000000) end)
		assert.has_error(function() Instruction.dd(0xFFFFFFFE, 0xFFFFFFFF, 0x100000000) end)
		assert.has_error(function() Instruction.dd(0xFFFFFFFD, 0xFFFFFFFE, 0xFFFFFFFF, 0x100000000) end)

		assert.has_error(function() Instruction.dd({ -0x80000001 }) end)
		assert.has_error(function() Instruction.dd({ 0xFFFFFFFF, -0x80000001 }) end)
		assert.has_error(function() Instruction.dd({ 0xFFFFFFFE, 0xFFFFFFFF, -0x80000001 }) end)
		assert.has_error(function() Instruction.dd({ 0xFFFFFFFD, 0xFFFFFFFE, 0xFFFFFFFF, -0x80000001 }) end)

		assert.has_error(function() Instruction.dd({ 0x100000000 }) end)
		assert.has_error(function() Instruction.dd({ 0xFFFFFFFF, 0x100000000 }) end)
		assert.has_error(function() Instruction.dd({ 0xFFFFFFFE, 0xFFFFFFFF, 0x100000000 }) end)
		assert.has_error(function() Instruction.dd({ 0xFFFFFFFD, 0xFFFFFFFE, 0xFFFFFFFF, 0x100000000 }) end)

		assert.has_error(function() Instruction.dd(true) end)
		assert.has_error(function() Instruction.dd(0xFFFFFFFF, true) end)
		assert.has_error(function() Instruction.dd(0xFFFFFFFE, 0xFFFFFFFF, true) end)
		assert.has_error(function() Instruction.dd(0xFFFFFFFD, 0xFFFFFFFE, 0xFFFFFFFF, true) end)

		assert.has_error(function() Instruction.dd({ true }) end)
		assert.has_error(function() Instruction.dd({ 0xFFFFFFFF, true }) end)
		assert.has_error(function() Instruction.dd({ 0xFFFFFFFE, 0xFFFFFFFF, true }) end)
		assert.has_error(function() Instruction.dd({ 0xFFFFFFFD, 0xFFFFFFFE, 0xFFFFFFFF, true }) end)

									Instruction.dd(from_hex("778899AA"))
		assert.has_error(function() Instruction.dd(from_hex("778899AA"), "") end)

									Instruction.dd({ 0x778899AA })
		assert.has_error(function() Instruction.dd({ 0x778899AA }, {}) end)
	end)

	it("dq", function()
		-- stylua: ignore
		local tests = {
			{ Instruction.dq(from_hex("123456789A000000")), { 0x9A78563412 } },
			{ Instruction.dq(from_hex("123456789A000000BCDEF01234000000")), { 0x9A78563412, 0x3412F0DEBC } },

			{ Instruction.dq(0x9A78563412), { 0x9A78563412 } },
			{ Instruction.dq(0x9A78563412, 0x3412F0DEBC), { 0x9A78563412, 0x3412F0DEBC } },
			{ Instruction.dq(-0x12345678), { -0x12345678 } },

			{ Instruction.dq({ 0x9A78563412 }), { 0x9A78563412 } },
			{ Instruction.dq({ 0x9A78563412, 0x3412F0DEBC }), { 0x9A78563412, 0x3412F0DEBC } },
			{ Instruction.dq({ -0x12345678 }), { -0x12345678 } },
		}
		-- stylua: ignore
		if has_int64 then
			tests[#tests + 1] = { Instruction.dq(from_hex("6C4205559DCEA977")), { 0x77A9CE9D5505426C } }
			tests[#tests + 1] = { Instruction.dq(from_hex("6C4205559DCEA97708AA27344FFE3286")), { 0x77A9CE9D5505426C, 0x8632FE4F3427AA08 } }

			tests[#tests + 1] = { Instruction.dq(0x77A9CE9D5505426C), { 0x77A9CE9D5505426C } }
			tests[#tests + 1] = { Instruction.dq(0x77A9CE9D5505426C, 0x8632FE4F3427AA08), { 0x77A9CE9D5505426C, 0x8632FE4F3427AA08 } }

			tests[#tests + 1] = { Instruction.dq(-0x8000000000000000), { 0x8000000000000000 } }
			tests[#tests + 1] = { Instruction.dq(0xFFFFFFFFFFFFFFFF, -0x8000000000000000), { 0xFFFFFFFFFFFFFFFF, 0x8000000000000000 } }
			tests[#tests + 1] = { Instruction.dq(0xFFFFFFFFFFFFFFFE, 0xFFFFFFFFFFFFFFFF), { 0xFFFFFFFFFFFFFFFE, 0xFFFFFFFFFFFFFFFF } }

			tests[#tests + 1] = { Instruction.dq(0xFFFFFFFFFFFFFFFF), { 0xFFFFFFFFFFFFFFFF } }
			tests[#tests + 1] = { Instruction.dq(0x7FFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF), { 0x7FFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF } }

			tests[#tests + 1] = { Instruction.dq({ 0x77A9CE9D5505426C }), { 0x77A9CE9D5505426C } }
			tests[#tests + 1] = { Instruction.dq({ 0x77A9CE9D5505426C, 0x8632FE4F3427AA08 }), { 0x77A9CE9D5505426C, 0x8632FE4F3427AA08 } }

			tests[#tests + 1] = { Instruction.dq({ -0x8000000000000000 }), { 0x8000000000000000 } }
			tests[#tests + 1] = { Instruction.dq({ 0xFFFFFFFFFFFFFFFF, -0x8000000000000000 }), { 0xFFFFFFFFFFFFFFFF, 0x8000000000000000 } }
			tests[#tests + 1] = { Instruction.dq({ 0xFFFFFFFFFFFFFFFE, 0xFFFFFFFFFFFFFFFF }), { 0xFFFFFFFFFFFFFFFE, 0xFFFFFFFFFFFFFFFF } }

			tests[#tests + 1] = { Instruction.dq({ 0xFFFFFFFFFFFFFFFF }), { 0xFFFFFFFFFFFFFFFF } }
			tests[#tests + 1] = { Instruction.dq({ 0x7FFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF }), { 0x7FFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF } }
		end
		for _, tc in ipairs(tests) do
			local instr = tc[1]
			local expected = tc[2]
			assert.equals(Code.DeclareQword, instr:code())
			assert.equals(#expected, instr:declare_data_len())
			for i, value in ipairs(expected) do
				assert.equals(value, instr:get_declare_qword_value(i - 1))
			end
		end
	end)

	-- stylua: ignore
	it("dq: fail", function()
		assert.has_error(function() Instruction.dq("") end)
		assert.has_error(function() Instruction.dq(from_hex("00")) end)
		assert.has_error(function() Instruction.dq(from_hex("0000")) end)
		assert.has_error(function() Instruction.dq(from_hex("000000")) end)
		assert.has_error(function() Instruction.dq(from_hex("00000000")) end)
		assert.has_error(function() Instruction.dq(from_hex("0000000000")) end)
		assert.has_error(function() Instruction.dq(from_hex("000000000000")) end)
		assert.has_error(function() Instruction.dq(from_hex("00000000000000")) end)
									Instruction.dq(from_hex("0000000000000000"))
		assert.has_error(function() Instruction.dq(from_hex("000000000000000000")) end)
		assert.has_error(function() Instruction.dq(from_hex("00000000000000000000")) end)
		assert.has_error(function() Instruction.dq(from_hex("0000000000000000000000")) end)
		assert.has_error(function() Instruction.dq(from_hex("000000000000000000000000")) end)
		assert.has_error(function() Instruction.dq(from_hex("00000000000000000000000000")) end)
		assert.has_error(function() Instruction.dq(from_hex("0000000000000000000000000000")) end)
		assert.has_error(function() Instruction.dq(from_hex("000000000000000000000000000000")) end)
									Instruction.dq(from_hex("00000000000000000000000000000000"))
		assert.has_error(function() Instruction.dq(from_hex("0000000000000000000000000000000000")) end)
		assert.has_error(function() Instruction.dq(from_hex("000000000000000000000000000000000000")) end)
		assert.has_error(function() Instruction.dq(from_hex("00000000000000000000000000000000000000")) end)
		assert.has_error(function() Instruction.dq(from_hex("0000000000000000000000000000000000000000")) end)
		assert.has_error(function() Instruction.dq(from_hex("000000000000000000000000000000000000000000")) end)
		assert.has_error(function() Instruction.dq(from_hex("00000000000000000000000000000000000000000000")) end)
		assert.has_error(function() Instruction.dq(from_hex("0000000000000000000000000000000000000000000000")) end)
		assert.has_error(function() Instruction.dq(from_hex("000000000000000000000000000000000000000000000000")) end)

		assert.has_error(function() Instruction.dq(true) end)
		assert.has_error(function() Instruction.dq(0, true) end)

		assert.has_error(function() Instruction.dq({ true }) end)
		assert.has_error(function() Instruction.dq({ 0, true }) end)

									Instruction.dq(from_hex("778899AABBCCDDEE"))
		assert.has_error(function() Instruction.dq(from_hex("778899AABBCCDDEE"), "") end)

									Instruction.dq({ 0x778899AABBCCDDEE })
		assert.has_error(function() Instruction.dq({ 0x778899AABBCCDDEE }, {}) end)
	end)

	it("create", function()
		-- stylua: ignore
		local tests = {
			{ 64, "90", DecoderOptions.None, Instruction.create(Code.Nopd) },
			{ 64, "48B9FFFFFFFFFFFFFFFF", DecoderOptions.None, Instruction.create(Code.Mov_r64_imm64, Register.RCX, -1) },
			{ 64, "48B9003056789ABCDE31", DecoderOptions.None, Instruction.create(Code.Mov_r64_imm64, Register.RCX, 0x31DEBC9A78563000) },
			{ 64, "48B9FFFFFFFF00000000", DecoderOptions.None, Instruction.create(Code.Mov_r64_imm64, Register.RCX, 0xFFFFFFFF) },
			{ 64, "8FC1", DecoderOptions.None, Instruction.create(Code.Pop_rm64, Register.RCX) },
			{ 64, "648F847501EFCDAB", DecoderOptions.None, Instruction.create(Code.Pop_rm64, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS)) },
			{ 64, "C6F85A", DecoderOptions.None, Instruction.create(Code.Xabort_imm8, 0x5A) },
			{ 64, "66685AA5", DecoderOptions.None, Instruction.create(Code.Push_imm16, 0xA55A) },
			{ 32, "685AA51234", DecoderOptions.None, Instruction.create(Code.Pushd_imm32, 0x3412A55A) },
			{ 64, "666A5A", DecoderOptions.None, Instruction.create(Code.Pushw_imm8, 0x5A) },
			{ 32, "6A5A", DecoderOptions.None, Instruction.create(Code.Pushd_imm8, 0x5A) },
			{ 64, "6A5A", DecoderOptions.None, Instruction.create(Code.Pushq_imm8, 0x5A) },
			{ 64, "685AA512A4", DecoderOptions.None, Instruction.create(Code.Pushq_imm32, -0x5BED5AA6) },
			{ 32, "66705A", DecoderOptions.None, Instruction.create_branch(Code.Jo_rel8_16, 0x4D) },
			{ 32, "705A", DecoderOptions.None, Instruction.create_branch(Code.Jo_rel8_32, 0x8000004C) },
			{ 64, "705A", DecoderOptions.None, Instruction.create_branch(Code.Jo_rel8_64, 0x800000004C) },
			{ 32, "669A12345678", DecoderOptions.None, Instruction.create_far_branch(Code.Call_ptr1616, 0x7856, 0x3412) },
			{ 32, "9A123456789ABC", DecoderOptions.None, Instruction.create_far_branch(Code.Call_ptr1632, 0xBC9A, 0x78563412) },
			{ 16, "C7F85AA5", DecoderOptions.None, Instruction.create_xbegin(16, 0x254E) },
			{ 32, "C7F85AA51234", DecoderOptions.None, Instruction.create_xbegin(32, 0xB412A550) },
			{ 64, "C7F85AA51234", DecoderOptions.None, Instruction.create_xbegin(64, 0x803412A550) },
			{ 64, "00D1", DecoderOptions.None, Instruction.create(Code.Add_rm8_r8, Register.CL, Register.DL) },
			{ 64, "64028C7501EFCDAB", DecoderOptions.None, Instruction.create(Code.Add_r8_rm8, Register.CL, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS)) },
			{ 64, "80C15A", DecoderOptions.None, Instruction.create(Code.Add_rm8_imm8, Register.CL, 0x5A) },
			{ 64, "6681C15AA5", DecoderOptions.None, Instruction.create(Code.Add_rm16_imm16, Register.CX, 0xA55A) },
			{ 64, "81C15AA51234", DecoderOptions.None, Instruction.create(Code.Add_rm32_imm32, Register.ECX, 0x3412A55A) },
			{ 64, "48B900102637A55A5678", DecoderOptions.None, Instruction.create(Code.Mov_r64_imm64, Register.RCX, 0x78565AA537261000) },
			{ 64, "6683C15A", DecoderOptions.None, Instruction.create(Code.Add_rm16_imm8, Register.CX, 0x5A) },
			{ 64, "83C15A", DecoderOptions.None, Instruction.create(Code.Add_rm32_imm8, Register.ECX, 0x5A) },
			{ 64, "4883C15A", DecoderOptions.None, Instruction.create(Code.Add_rm64_imm8, Register.RCX, 0x5A) },
			{ 64, "4881C15AA51234", DecoderOptions.None, Instruction.create(Code.Add_rm64_imm32, Register.RCX, 0x3412A55A) },
			{ 64, "64A0004056789ABCDEF0", DecoderOptions.None, Instruction.create(Code.Mov_AL_moffs8, Register.AL, MemoryOperand.new(Register.None, Register.None, 1, -0x0F21436587A9C000, 8, false, Register.FS)) },
			{ 64, "6400947501EFCDAB", DecoderOptions.None, Instruction.create(Code.Add_rm8_r8, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), Register.DL) },
			{ 64, "6480847501EFCDAB5A", DecoderOptions.None, Instruction.create(Code.Add_rm8_imm8, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x5A) },
			{ 64, "646681847501EFCDAB5AA5", DecoderOptions.None, Instruction.create(Code.Add_rm16_imm16, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0xA55A) },
			{ 64, "6481847501EFCDAB5AA51234", DecoderOptions.None, Instruction.create(Code.Add_rm32_imm32, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x3412A55A) },
			{ 64, "646683847501EFCDAB5A", DecoderOptions.None, Instruction.create(Code.Add_rm16_imm8, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x5A) },
			{ 64, "6483847501EFCDAB5A", DecoderOptions.None, Instruction.create(Code.Add_rm32_imm8, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x5A) },
			{ 64, "644883847501EFCDAB5A", DecoderOptions.None, Instruction.create(Code.Add_rm64_imm8, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x5A) },
			{ 64, "644881847501EFCDAB5AA51234", DecoderOptions.None, Instruction.create(Code.Add_rm64_imm32, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x3412A55A) },
			{ 64, "E65A", DecoderOptions.None, Instruction.create(Code.Out_imm8_AL, 0x5A, Register.AL) },
			{ 64, "66C85AA5A6", DecoderOptions.None, Instruction.create(Code.Enterw_imm16_imm8, 0xA55A, 0xA6) },
			{ 64, "64A2004056789ABCDEF0", DecoderOptions.None, Instruction.create(Code.Mov_moffs8_AL, MemoryOperand.new(Register.None, Register.None, 1, -0x0F21436587A9C000, 8, false, Register.FS), Register.AL) },
			{ 64, "6669CAA55A", DecoderOptions.None, Instruction.create(Code.Imul_r16_rm16_imm16, Register.CX, Register.DX, 0x5AA5) },
			{ 64, "69CA5AA51234", DecoderOptions.None, Instruction.create(Code.Imul_r32_rm32_imm32, Register.ECX, Register.EDX, 0x3412A55A) },
			{ 64, "666BCA5A", DecoderOptions.None, Instruction.create(Code.Imul_r16_rm16_imm8, Register.CX, Register.DX, 0x5A) },
			{ 64, "6BCA5A", DecoderOptions.None, Instruction.create(Code.Imul_r32_rm32_imm8, Register.ECX, Register.EDX, 0x5A) },
			{ 64, "486BCA5A", DecoderOptions.None, Instruction.create(Code.Imul_r64_rm64_imm8, Register.RCX, Register.RDX, 0x5A) },
			{ 64, "4869CA5AA512A4", DecoderOptions.None, Instruction.create(Code.Imul_r64_rm64_imm32, Register.RCX, Register.RDX, -0x5BED5AA6) },
			{ 64, "6466698C7501EFCDAB5AA5", DecoderOptions.None, Instruction.create(Code.Imul_r16_rm16_imm16, Register.CX, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0xA55A) },
			{ 64, "64698C7501EFCDAB5AA51234", DecoderOptions.None, Instruction.create(Code.Imul_r32_rm32_imm32, Register.ECX, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x3412A55A) },
			{ 64, "64666B8C7501EFCDAB5A", DecoderOptions.None, Instruction.create(Code.Imul_r16_rm16_imm8, Register.CX, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x5A) },
			{ 64, "646B8C7501EFCDAB5A", DecoderOptions.None, Instruction.create(Code.Imul_r32_rm32_imm8, Register.ECX, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x5A) },
			{ 64, "64486B8C7501EFCDAB5A", DecoderOptions.None, Instruction.create(Code.Imul_r64_rm64_imm8, Register.RCX, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x5A) },
			{ 64, "6448698C7501EFCDAB5AA512A4", DecoderOptions.None, Instruction.create(Code.Imul_r64_rm64_imm32, Register.RCX, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), -0x5BED5AA6) },
			{ 64, "660F78C1A5FD", DecoderOptions.None, Instruction.create(Code.Extrq_xmm_imm8_imm8, Register.XMM1, 0xA5, 0xFD) },
			{ 64, "64660FA4947501EFCDAB5A", DecoderOptions.None, Instruction.create(Code.Shld_rm16_r16_imm8, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), Register.DX, 0x5A) },
			{ 64, "F20F78CAA5FD", DecoderOptions.None, Instruction.create(Code.Insertq_xmm_xmm_imm8_imm8, Register.XMM1, Register.XMM2, 0xA5, 0xFD) },
			{ 16, "0FB855AA", DecoderOptions.Jmpe, Instruction.create_branch(Code.Jmpe_disp16, 0xAA55) },
			{ 32, "0FB8123455AA", DecoderOptions.Jmpe, Instruction.create_branch(Code.Jmpe_disp32, 0xAA553412) },
			{ 32, "64676E", DecoderOptions.None, Instruction.create_outsb(16, Register.FS, RepPrefixKind.None) },
			{ 64, "64676E", DecoderOptions.None, Instruction.create_outsb(32, Register.FS, RepPrefixKind.None) },
			{ 64, "646E", DecoderOptions.None, Instruction.create_outsb(64, Register.FS, RepPrefixKind.None) },
			{ 32, "6466676F", DecoderOptions.None, Instruction.create_outsw(16, Register.FS, RepPrefixKind.None) },
			{ 64, "6466676F", DecoderOptions.None, Instruction.create_outsw(32, Register.FS, RepPrefixKind.None) },
			{ 64, "64666F", DecoderOptions.None, Instruction.create_outsw(64, Register.FS, RepPrefixKind.None) },
			{ 32, "64676F", DecoderOptions.None, Instruction.create_outsd(16, Register.FS, RepPrefixKind.None) },
			{ 64, "64676F", DecoderOptions.None, Instruction.create_outsd(32, Register.FS, RepPrefixKind.None) },
			{ 64, "646F", DecoderOptions.None, Instruction.create_outsd(64, Register.FS, RepPrefixKind.None) },
			{ 32, "67AE", DecoderOptions.None, Instruction.create_scasb(16, RepPrefixKind.None) },
			{ 64, "67AE", DecoderOptions.None, Instruction.create_scasb(32, RepPrefixKind.None) },
			{ 64, "AE", DecoderOptions.None, Instruction.create_scasb(64, RepPrefixKind.None) },
			{ 32, "6667AF", DecoderOptions.None, Instruction.create_scasw(16, RepPrefixKind.None) },
			{ 64, "6667AF", DecoderOptions.None, Instruction.create_scasw(32, RepPrefixKind.None) },
			{ 64, "66AF", DecoderOptions.None, Instruction.create_scasw(64, RepPrefixKind.None) },
			{ 32, "67AF", DecoderOptions.None, Instruction.create_scasd(16, RepPrefixKind.None) },
			{ 64, "67AF", DecoderOptions.None, Instruction.create_scasd(32, RepPrefixKind.None) },
			{ 64, "AF", DecoderOptions.None, Instruction.create_scasd(64, RepPrefixKind.None) },
			{ 64, "6748AF", DecoderOptions.None, Instruction.create_scasq(32, RepPrefixKind.None) },
			{ 64, "48AF", DecoderOptions.None, Instruction.create_scasq(64, RepPrefixKind.None) },
			{ 32, "6467AC", DecoderOptions.None, Instruction.create_lodsb(16, Register.FS, RepPrefixKind.None) },
			{ 64, "6467AC", DecoderOptions.None, Instruction.create_lodsb(32, Register.FS, RepPrefixKind.None) },
			{ 64, "64AC", DecoderOptions.None, Instruction.create_lodsb(64, Register.FS, RepPrefixKind.None) },
			{ 32, "646667AD", DecoderOptions.None, Instruction.create_lodsw(16, Register.FS, RepPrefixKind.None) },
			{ 64, "646667AD", DecoderOptions.None, Instruction.create_lodsw(32, Register.FS, RepPrefixKind.None) },
			{ 64, "6466AD", DecoderOptions.None, Instruction.create_lodsw(64, Register.FS, RepPrefixKind.None) },
			{ 32, "6467AD", DecoderOptions.None, Instruction.create_lodsd(16, Register.FS, RepPrefixKind.None) },
			{ 64, "6467AD", DecoderOptions.None, Instruction.create_lodsd(32, Register.FS, RepPrefixKind.None) },
			{ 64, "64AD", DecoderOptions.None, Instruction.create_lodsd(64, Register.FS, RepPrefixKind.None) },
			{ 64, "646748AD", DecoderOptions.None, Instruction.create_lodsq(32, Register.FS, RepPrefixKind.None) },
			{ 64, "6448AD", DecoderOptions.None, Instruction.create_lodsq(64, Register.FS, RepPrefixKind.None) },
			{ 32, "676C", DecoderOptions.None, Instruction.create_insb(16, RepPrefixKind.None) },
			{ 64, "676C", DecoderOptions.None, Instruction.create_insb(32, RepPrefixKind.None) },
			{ 64, "6C", DecoderOptions.None, Instruction.create_insb(64, RepPrefixKind.None) },
			{ 32, "66676D", DecoderOptions.None, Instruction.create_insw(16, RepPrefixKind.None) },
			{ 64, "66676D", DecoderOptions.None, Instruction.create_insw(32, RepPrefixKind.None) },
			{ 64, "666D", DecoderOptions.None, Instruction.create_insw(64, RepPrefixKind.None) },
			{ 32, "676D", DecoderOptions.None, Instruction.create_insd(16, RepPrefixKind.None) },
			{ 64, "676D", DecoderOptions.None, Instruction.create_insd(32, RepPrefixKind.None) },
			{ 64, "6D", DecoderOptions.None, Instruction.create_insd(64, RepPrefixKind.None) },
			{ 32, "67AA", DecoderOptions.None, Instruction.create_stosb(16, RepPrefixKind.None) },
			{ 64, "67AA", DecoderOptions.None, Instruction.create_stosb(32, RepPrefixKind.None) },
			{ 64, "AA", DecoderOptions.None, Instruction.create_stosb(64, RepPrefixKind.None) },
			{ 32, "6667AB", DecoderOptions.None, Instruction.create_stosw(16, RepPrefixKind.None) },
			{ 64, "6667AB", DecoderOptions.None, Instruction.create_stosw(32, RepPrefixKind.None) },
			{ 64, "66AB", DecoderOptions.None, Instruction.create_stosw(64, RepPrefixKind.None) },
			{ 32, "67AB", DecoderOptions.None, Instruction.create_stosd(16, RepPrefixKind.None) },
			{ 64, "67AB", DecoderOptions.None, Instruction.create_stosd(32, RepPrefixKind.None) },
			{ 64, "AB", DecoderOptions.None, Instruction.create_stosd(64, RepPrefixKind.None) },
			{ 64, "6748AB", DecoderOptions.None, Instruction.create_stosq(32, RepPrefixKind.None) },
			{ 64, "48AB", DecoderOptions.None, Instruction.create_stosq(64, RepPrefixKind.None) },
			{ 32, "6467A6", DecoderOptions.None, Instruction.create_cmpsb(16, Register.FS, RepPrefixKind.None) },
			{ 64, "6467A6", DecoderOptions.None, Instruction.create_cmpsb(32, Register.FS, RepPrefixKind.None) },
			{ 64, "64A6", DecoderOptions.None, Instruction.create_cmpsb(64, Register.FS, RepPrefixKind.None) },
			{ 32, "646667A7", DecoderOptions.None, Instruction.create_cmpsw(16, Register.FS, RepPrefixKind.None) },
			{ 64, "646667A7", DecoderOptions.None, Instruction.create_cmpsw(32, Register.FS, RepPrefixKind.None) },
			{ 64, "6466A7", DecoderOptions.None, Instruction.create_cmpsw(64, Register.FS, RepPrefixKind.None) },
			{ 32, "6467A7", DecoderOptions.None, Instruction.create_cmpsd(16, Register.FS, RepPrefixKind.None) },
			{ 64, "6467A7", DecoderOptions.None, Instruction.create_cmpsd(32, Register.FS, RepPrefixKind.None) },
			{ 64, "64A7", DecoderOptions.None, Instruction.create_cmpsd(64, Register.FS, RepPrefixKind.None) },
			{ 64, "646748A7", DecoderOptions.None, Instruction.create_cmpsq(32, Register.FS, RepPrefixKind.None) },
			{ 64, "6448A7", DecoderOptions.None, Instruction.create_cmpsq(64, Register.FS, RepPrefixKind.None) },
			{ 32, "6467A4", DecoderOptions.None, Instruction.create_movsb(16, Register.FS, RepPrefixKind.None) },
			{ 64, "6467A4", DecoderOptions.None, Instruction.create_movsb(32, Register.FS, RepPrefixKind.None) },
			{ 64, "64A4", DecoderOptions.None, Instruction.create_movsb(64, Register.FS, RepPrefixKind.None) },
			{ 32, "646667A5", DecoderOptions.None, Instruction.create_movsw(16, Register.FS, RepPrefixKind.None) },
			{ 64, "646667A5", DecoderOptions.None, Instruction.create_movsw(32, Register.FS, RepPrefixKind.None) },
			{ 64, "6466A5", DecoderOptions.None, Instruction.create_movsw(64, Register.FS, RepPrefixKind.None) },
			{ 32, "6467A5", DecoderOptions.None, Instruction.create_movsd(16, Register.FS, RepPrefixKind.None) },
			{ 64, "6467A5", DecoderOptions.None, Instruction.create_movsd(32, Register.FS, RepPrefixKind.None) },
			{ 64, "64A5", DecoderOptions.None, Instruction.create_movsd(64, Register.FS, RepPrefixKind.None) },
			{ 64, "646748A5", DecoderOptions.None, Instruction.create_movsq(32, Register.FS, RepPrefixKind.None) },
			{ 64, "6448A5", DecoderOptions.None, Instruction.create_movsq(64, Register.FS, RepPrefixKind.None) },
			{ 32, "64670FF7D3", DecoderOptions.None, Instruction.create_maskmovq(16, Register.MM2, Register.MM3, Register.FS) },
			{ 64, "64670FF7D3", DecoderOptions.None, Instruction.create_maskmovq(32, Register.MM2, Register.MM3, Register.FS) },
			{ 64, "640FF7D3", DecoderOptions.None, Instruction.create_maskmovq(64, Register.MM2, Register.MM3, Register.FS) },
			{ 32, "6467660FF7D3", DecoderOptions.None, Instruction.create_maskmovdqu(16, Register.XMM2, Register.XMM3, Register.FS) },
			{ 64, "6467660FF7D3", DecoderOptions.None, Instruction.create_maskmovdqu(32, Register.XMM2, Register.XMM3, Register.FS) },
			{ 64, "64660FF7D3", DecoderOptions.None, Instruction.create_maskmovdqu(64, Register.XMM2, Register.XMM3, Register.FS) },

			{ 32, "6467F36E", DecoderOptions.None, Instruction.create_outsb(16, Register.FS, RepPrefixKind.Repe) },
			{ 64, "6467F36E", DecoderOptions.None, Instruction.create_outsb(32, Register.FS, RepPrefixKind.Repe) },
			{ 64, "64F36E", DecoderOptions.None, Instruction.create_outsb(64, Register.FS, RepPrefixKind.Repe) },
			{ 32, "646667F36F", DecoderOptions.None, Instruction.create_outsw(16, Register.FS, RepPrefixKind.Repe) },
			{ 64, "646667F36F", DecoderOptions.None, Instruction.create_outsw(32, Register.FS, RepPrefixKind.Repe) },
			{ 64, "6466F36F", DecoderOptions.None, Instruction.create_outsw(64, Register.FS, RepPrefixKind.Repe) },
			{ 32, "6467F36F", DecoderOptions.None, Instruction.create_outsd(16, Register.FS, RepPrefixKind.Repe) },
			{ 64, "6467F36F", DecoderOptions.None, Instruction.create_outsd(32, Register.FS, RepPrefixKind.Repe) },
			{ 64, "64F36F", DecoderOptions.None, Instruction.create_outsd(64, Register.FS, RepPrefixKind.Repe) },
			{ 32, "67F3AE", DecoderOptions.None, Instruction.create_scasb(16, RepPrefixKind.Repe) },
			{ 64, "67F3AE", DecoderOptions.None, Instruction.create_scasb(32, RepPrefixKind.Repe) },
			{ 64, "F3AE", DecoderOptions.None, Instruction.create_scasb(64, RepPrefixKind.Repe) },
			{ 32, "6667F3AF", DecoderOptions.None, Instruction.create_scasw(16, RepPrefixKind.Repe) },
			{ 64, "6667F3AF", DecoderOptions.None, Instruction.create_scasw(32, RepPrefixKind.Repe) },
			{ 64, "66F3AF", DecoderOptions.None, Instruction.create_scasw(64, RepPrefixKind.Repe) },
			{ 32, "67F3AF", DecoderOptions.None, Instruction.create_scasd(16, RepPrefixKind.Repe) },
			{ 64, "67F3AF", DecoderOptions.None, Instruction.create_scasd(32, RepPrefixKind.Repe) },
			{ 64, "F3AF", DecoderOptions.None, Instruction.create_scasd(64, RepPrefixKind.Repe) },
			{ 64, "67F348AF", DecoderOptions.None, Instruction.create_scasq(32, RepPrefixKind.Repe) },
			{ 64, "F348AF", DecoderOptions.None, Instruction.create_scasq(64, RepPrefixKind.Repe) },
			{ 32, "6467F3AC", DecoderOptions.None, Instruction.create_lodsb(16, Register.FS, RepPrefixKind.Repe) },
			{ 64, "6467F3AC", DecoderOptions.None, Instruction.create_lodsb(32, Register.FS, RepPrefixKind.Repe) },
			{ 64, "64F3AC", DecoderOptions.None, Instruction.create_lodsb(64, Register.FS, RepPrefixKind.Repe) },
			{ 32, "646667F3AD", DecoderOptions.None, Instruction.create_lodsw(16, Register.FS, RepPrefixKind.Repe) },
			{ 64, "646667F3AD", DecoderOptions.None, Instruction.create_lodsw(32, Register.FS, RepPrefixKind.Repe) },
			{ 64, "6466F3AD", DecoderOptions.None, Instruction.create_lodsw(64, Register.FS, RepPrefixKind.Repe) },
			{ 32, "6467F3AD", DecoderOptions.None, Instruction.create_lodsd(16, Register.FS, RepPrefixKind.Repe) },
			{ 64, "6467F3AD", DecoderOptions.None, Instruction.create_lodsd(32, Register.FS, RepPrefixKind.Repe) },
			{ 64, "64F3AD", DecoderOptions.None, Instruction.create_lodsd(64, Register.FS, RepPrefixKind.Repe) },
			{ 64, "6467F348AD", DecoderOptions.None, Instruction.create_lodsq(32, Register.FS, RepPrefixKind.Repe) },
			{ 64, "64F348AD", DecoderOptions.None, Instruction.create_lodsq(64, Register.FS, RepPrefixKind.Repe) },
			{ 32, "67F36C", DecoderOptions.None, Instruction.create_insb(16, RepPrefixKind.Repe) },
			{ 64, "67F36C", DecoderOptions.None, Instruction.create_insb(32, RepPrefixKind.Repe) },
			{ 64, "F36C", DecoderOptions.None, Instruction.create_insb(64, RepPrefixKind.Repe) },
			{ 32, "6667F36D", DecoderOptions.None, Instruction.create_insw(16, RepPrefixKind.Repe) },
			{ 64, "6667F36D", DecoderOptions.None, Instruction.create_insw(32, RepPrefixKind.Repe) },
			{ 64, "66F36D", DecoderOptions.None, Instruction.create_insw(64, RepPrefixKind.Repe) },
			{ 32, "67F36D", DecoderOptions.None, Instruction.create_insd(16, RepPrefixKind.Repe) },
			{ 64, "67F36D", DecoderOptions.None, Instruction.create_insd(32, RepPrefixKind.Repe) },
			{ 64, "F36D", DecoderOptions.None, Instruction.create_insd(64, RepPrefixKind.Repe) },
			{ 32, "67F3AA", DecoderOptions.None, Instruction.create_stosb(16, RepPrefixKind.Repe) },
			{ 64, "67F3AA", DecoderOptions.None, Instruction.create_stosb(32, RepPrefixKind.Repe) },
			{ 64, "F3AA", DecoderOptions.None, Instruction.create_stosb(64, RepPrefixKind.Repe) },
			{ 32, "6667F3AB", DecoderOptions.None, Instruction.create_stosw(16, RepPrefixKind.Repe) },
			{ 64, "6667F3AB", DecoderOptions.None, Instruction.create_stosw(32, RepPrefixKind.Repe) },
			{ 64, "66F3AB", DecoderOptions.None, Instruction.create_stosw(64, RepPrefixKind.Repe) },
			{ 32, "67F3AB", DecoderOptions.None, Instruction.create_stosd(16, RepPrefixKind.Repe) },
			{ 64, "67F3AB", DecoderOptions.None, Instruction.create_stosd(32, RepPrefixKind.Repe) },
			{ 64, "F3AB", DecoderOptions.None, Instruction.create_stosd(64, RepPrefixKind.Repe) },
			{ 64, "67F348AB", DecoderOptions.None, Instruction.create_stosq(32, RepPrefixKind.Repe) },
			{ 64, "F348AB", DecoderOptions.None, Instruction.create_stosq(64, RepPrefixKind.Repe) },
			{ 32, "6467F3A6", DecoderOptions.None, Instruction.create_cmpsb(16, Register.FS, RepPrefixKind.Repe) },
			{ 64, "6467F3A6", DecoderOptions.None, Instruction.create_cmpsb(32, Register.FS, RepPrefixKind.Repe) },
			{ 64, "64F3A6", DecoderOptions.None, Instruction.create_cmpsb(64, Register.FS, RepPrefixKind.Repe) },
			{ 32, "646667F3A7", DecoderOptions.None, Instruction.create_cmpsw(16, Register.FS, RepPrefixKind.Repe) },
			{ 64, "646667F3A7", DecoderOptions.None, Instruction.create_cmpsw(32, Register.FS, RepPrefixKind.Repe) },
			{ 64, "6466F3A7", DecoderOptions.None, Instruction.create_cmpsw(64, Register.FS, RepPrefixKind.Repe) },
			{ 32, "6467F3A7", DecoderOptions.None, Instruction.create_cmpsd(16, Register.FS, RepPrefixKind.Repe) },
			{ 64, "6467F3A7", DecoderOptions.None, Instruction.create_cmpsd(32, Register.FS, RepPrefixKind.Repe) },
			{ 64, "64F3A7", DecoderOptions.None, Instruction.create_cmpsd(64, Register.FS, RepPrefixKind.Repe) },
			{ 64, "6467F348A7", DecoderOptions.None, Instruction.create_cmpsq(32, Register.FS, RepPrefixKind.Repe) },
			{ 64, "64F348A7", DecoderOptions.None, Instruction.create_cmpsq(64, Register.FS, RepPrefixKind.Repe) },
			{ 32, "6467F3A4", DecoderOptions.None, Instruction.create_movsb(16, Register.FS, RepPrefixKind.Repe) },
			{ 64, "6467F3A4", DecoderOptions.None, Instruction.create_movsb(32, Register.FS, RepPrefixKind.Repe) },
			{ 64, "64F3A4", DecoderOptions.None, Instruction.create_movsb(64, Register.FS, RepPrefixKind.Repe) },
			{ 32, "646667F3A5", DecoderOptions.None, Instruction.create_movsw(16, Register.FS, RepPrefixKind.Repe) },
			{ 64, "646667F3A5", DecoderOptions.None, Instruction.create_movsw(32, Register.FS, RepPrefixKind.Repe) },
			{ 64, "6466F3A5", DecoderOptions.None, Instruction.create_movsw(64, Register.FS, RepPrefixKind.Repe) },
			{ 32, "6467F3A5", DecoderOptions.None, Instruction.create_movsd(16, Register.FS, RepPrefixKind.Repe) },
			{ 64, "6467F3A5", DecoderOptions.None, Instruction.create_movsd(32, Register.FS, RepPrefixKind.Repe) },
			{ 64, "64F3A5", DecoderOptions.None, Instruction.create_movsd(64, Register.FS, RepPrefixKind.Repe) },
			{ 64, "6467F348A5", DecoderOptions.None, Instruction.create_movsq(32, Register.FS, RepPrefixKind.Repe) },
			{ 64, "64F348A5", DecoderOptions.None, Instruction.create_movsq(64, Register.FS, RepPrefixKind.Repe) },

			{ 32, "6467F26E", DecoderOptions.None, Instruction.create_outsb(16, Register.FS, RepPrefixKind.Repne) },
			{ 64, "6467F26E", DecoderOptions.None, Instruction.create_outsb(32, Register.FS, RepPrefixKind.Repne) },
			{ 64, "64F26E", DecoderOptions.None, Instruction.create_outsb(64, Register.FS, RepPrefixKind.Repne) },
			{ 32, "646667F26F", DecoderOptions.None, Instruction.create_outsw(16, Register.FS, RepPrefixKind.Repne) },
			{ 64, "646667F26F", DecoderOptions.None, Instruction.create_outsw(32, Register.FS, RepPrefixKind.Repne) },
			{ 64, "6466F26F", DecoderOptions.None, Instruction.create_outsw(64, Register.FS, RepPrefixKind.Repne) },
			{ 32, "6467F26F", DecoderOptions.None, Instruction.create_outsd(16, Register.FS, RepPrefixKind.Repne) },
			{ 64, "6467F26F", DecoderOptions.None, Instruction.create_outsd(32, Register.FS, RepPrefixKind.Repne) },
			{ 64, "64F26F", DecoderOptions.None, Instruction.create_outsd(64, Register.FS, RepPrefixKind.Repne) },
			{ 32, "67F2AE", DecoderOptions.None, Instruction.create_scasb(16, RepPrefixKind.Repne) },
			{ 64, "67F2AE", DecoderOptions.None, Instruction.create_scasb(32, RepPrefixKind.Repne) },
			{ 64, "F2AE", DecoderOptions.None, Instruction.create_scasb(64, RepPrefixKind.Repne) },
			{ 32, "6667F2AF", DecoderOptions.None, Instruction.create_scasw(16, RepPrefixKind.Repne) },
			{ 64, "6667F2AF", DecoderOptions.None, Instruction.create_scasw(32, RepPrefixKind.Repne) },
			{ 64, "66F2AF", DecoderOptions.None, Instruction.create_scasw(64, RepPrefixKind.Repne) },
			{ 32, "67F2AF", DecoderOptions.None, Instruction.create_scasd(16, RepPrefixKind.Repne) },
			{ 64, "67F2AF", DecoderOptions.None, Instruction.create_scasd(32, RepPrefixKind.Repne) },
			{ 64, "F2AF", DecoderOptions.None, Instruction.create_scasd(64, RepPrefixKind.Repne) },
			{ 64, "67F248AF", DecoderOptions.None, Instruction.create_scasq(32, RepPrefixKind.Repne) },
			{ 64, "F248AF", DecoderOptions.None, Instruction.create_scasq(64, RepPrefixKind.Repne) },
			{ 32, "6467F2AC", DecoderOptions.None, Instruction.create_lodsb(16, Register.FS, RepPrefixKind.Repne) },
			{ 64, "6467F2AC", DecoderOptions.None, Instruction.create_lodsb(32, Register.FS, RepPrefixKind.Repne) },
			{ 64, "64F2AC", DecoderOptions.None, Instruction.create_lodsb(64, Register.FS, RepPrefixKind.Repne) },
			{ 32, "646667F2AD", DecoderOptions.None, Instruction.create_lodsw(16, Register.FS, RepPrefixKind.Repne) },
			{ 64, "646667F2AD", DecoderOptions.None, Instruction.create_lodsw(32, Register.FS, RepPrefixKind.Repne) },
			{ 64, "6466F2AD", DecoderOptions.None, Instruction.create_lodsw(64, Register.FS, RepPrefixKind.Repne) },
			{ 32, "6467F2AD", DecoderOptions.None, Instruction.create_lodsd(16, Register.FS, RepPrefixKind.Repne) },
			{ 64, "6467F2AD", DecoderOptions.None, Instruction.create_lodsd(32, Register.FS, RepPrefixKind.Repne) },
			{ 64, "64F2AD", DecoderOptions.None, Instruction.create_lodsd(64, Register.FS, RepPrefixKind.Repne) },
			{ 64, "6467F248AD", DecoderOptions.None, Instruction.create_lodsq(32, Register.FS, RepPrefixKind.Repne) },
			{ 64, "64F248AD", DecoderOptions.None, Instruction.create_lodsq(64, Register.FS, RepPrefixKind.Repne) },
			{ 32, "67F26C", DecoderOptions.None, Instruction.create_insb(16, RepPrefixKind.Repne) },
			{ 64, "67F26C", DecoderOptions.None, Instruction.create_insb(32, RepPrefixKind.Repne) },
			{ 64, "F26C", DecoderOptions.None, Instruction.create_insb(64, RepPrefixKind.Repne) },
			{ 32, "6667F26D", DecoderOptions.None, Instruction.create_insw(16, RepPrefixKind.Repne) },
			{ 64, "6667F26D", DecoderOptions.None, Instruction.create_insw(32, RepPrefixKind.Repne) },
			{ 64, "66F26D", DecoderOptions.None, Instruction.create_insw(64, RepPrefixKind.Repne) },
			{ 32, "67F26D", DecoderOptions.None, Instruction.create_insd(16, RepPrefixKind.Repne) },
			{ 64, "67F26D", DecoderOptions.None, Instruction.create_insd(32, RepPrefixKind.Repne) },
			{ 64, "F26D", DecoderOptions.None, Instruction.create_insd(64, RepPrefixKind.Repne) },
			{ 32, "67F2AA", DecoderOptions.None, Instruction.create_stosb(16, RepPrefixKind.Repne) },
			{ 64, "67F2AA", DecoderOptions.None, Instruction.create_stosb(32, RepPrefixKind.Repne) },
			{ 64, "F2AA", DecoderOptions.None, Instruction.create_stosb(64, RepPrefixKind.Repne) },
			{ 32, "6667F2AB", DecoderOptions.None, Instruction.create_stosw(16, RepPrefixKind.Repne) },
			{ 64, "6667F2AB", DecoderOptions.None, Instruction.create_stosw(32, RepPrefixKind.Repne) },
			{ 64, "66F2AB", DecoderOptions.None, Instruction.create_stosw(64, RepPrefixKind.Repne) },
			{ 32, "67F2AB", DecoderOptions.None, Instruction.create_stosd(16, RepPrefixKind.Repne) },
			{ 64, "67F2AB", DecoderOptions.None, Instruction.create_stosd(32, RepPrefixKind.Repne) },
			{ 64, "F2AB", DecoderOptions.None, Instruction.create_stosd(64, RepPrefixKind.Repne) },
			{ 64, "67F248AB", DecoderOptions.None, Instruction.create_stosq(32, RepPrefixKind.Repne) },
			{ 64, "F248AB", DecoderOptions.None, Instruction.create_stosq(64, RepPrefixKind.Repne) },
			{ 32, "6467F2A6", DecoderOptions.None, Instruction.create_cmpsb(16, Register.FS, RepPrefixKind.Repne) },
			{ 64, "6467F2A6", DecoderOptions.None, Instruction.create_cmpsb(32, Register.FS, RepPrefixKind.Repne) },
			{ 64, "64F2A6", DecoderOptions.None, Instruction.create_cmpsb(64, Register.FS, RepPrefixKind.Repne) },
			{ 32, "646667F2A7", DecoderOptions.None, Instruction.create_cmpsw(16, Register.FS, RepPrefixKind.Repne) },
			{ 64, "646667F2A7", DecoderOptions.None, Instruction.create_cmpsw(32, Register.FS, RepPrefixKind.Repne) },
			{ 64, "6466F2A7", DecoderOptions.None, Instruction.create_cmpsw(64, Register.FS, RepPrefixKind.Repne) },
			{ 32, "6467F2A7", DecoderOptions.None, Instruction.create_cmpsd(16, Register.FS, RepPrefixKind.Repne) },
			{ 64, "6467F2A7", DecoderOptions.None, Instruction.create_cmpsd(32, Register.FS, RepPrefixKind.Repne) },
			{ 64, "64F2A7", DecoderOptions.None, Instruction.create_cmpsd(64, Register.FS, RepPrefixKind.Repne) },
			{ 64, "6467F248A7", DecoderOptions.None, Instruction.create_cmpsq(32, Register.FS, RepPrefixKind.Repne) },
			{ 64, "64F248A7", DecoderOptions.None, Instruction.create_cmpsq(64, Register.FS, RepPrefixKind.Repne) },
			{ 32, "6467F2A4", DecoderOptions.None, Instruction.create_movsb(16, Register.FS, RepPrefixKind.Repne) },
			{ 64, "6467F2A4", DecoderOptions.None, Instruction.create_movsb(32, Register.FS, RepPrefixKind.Repne) },
			{ 64, "64F2A4", DecoderOptions.None, Instruction.create_movsb(64, Register.FS, RepPrefixKind.Repne) },
			{ 32, "646667F2A5", DecoderOptions.None, Instruction.create_movsw(16, Register.FS, RepPrefixKind.Repne) },
			{ 64, "646667F2A5", DecoderOptions.None, Instruction.create_movsw(32, Register.FS, RepPrefixKind.Repne) },
			{ 64, "6466F2A5", DecoderOptions.None, Instruction.create_movsw(64, Register.FS, RepPrefixKind.Repne) },
			{ 32, "6467F2A5", DecoderOptions.None, Instruction.create_movsd(16, Register.FS, RepPrefixKind.Repne) },
			{ 64, "6467F2A5", DecoderOptions.None, Instruction.create_movsd(32, Register.FS, RepPrefixKind.Repne) },
			{ 64, "64F2A5", DecoderOptions.None, Instruction.create_movsd(64, Register.FS, RepPrefixKind.Repne) },
			{ 64, "6467F248A5", DecoderOptions.None, Instruction.create_movsq(32, Register.FS, RepPrefixKind.Repne) },
			{ 64, "64F248A5", DecoderOptions.None, Instruction.create_movsq(64, Register.FS, RepPrefixKind.Repne) },

			{ 32, "67F36E", DecoderOptions.None, Instruction.create_rep_outsb(16) },
			{ 64, "67F36E", DecoderOptions.None, Instruction.create_rep_outsb(32) },
			{ 64, "F36E", DecoderOptions.None, Instruction.create_rep_outsb(64) },
			{ 32, "6667F36F", DecoderOptions.None, Instruction.create_rep_outsw(16) },
			{ 64, "6667F36F", DecoderOptions.None, Instruction.create_rep_outsw(32) },
			{ 64, "66F36F", DecoderOptions.None, Instruction.create_rep_outsw(64) },
			{ 32, "67F36F", DecoderOptions.None, Instruction.create_rep_outsd(16) },
			{ 64, "67F36F", DecoderOptions.None, Instruction.create_rep_outsd(32) },
			{ 64, "F36F", DecoderOptions.None, Instruction.create_rep_outsd(64) },
			{ 32, "67F3AE", DecoderOptions.None, Instruction.create_repe_scasb(16) },
			{ 64, "67F3AE", DecoderOptions.None, Instruction.create_repe_scasb(32) },
			{ 64, "F3AE", DecoderOptions.None, Instruction.create_repe_scasb(64) },
			{ 32, "6667F3AF", DecoderOptions.None, Instruction.create_repe_scasw(16) },
			{ 64, "6667F3AF", DecoderOptions.None, Instruction.create_repe_scasw(32) },
			{ 64, "66F3AF", DecoderOptions.None, Instruction.create_repe_scasw(64) },
			{ 32, "67F3AF", DecoderOptions.None, Instruction.create_repe_scasd(16) },
			{ 64, "67F3AF", DecoderOptions.None, Instruction.create_repe_scasd(32) },
			{ 64, "F3AF", DecoderOptions.None, Instruction.create_repe_scasd(64) },
			{ 64, "67F348AF", DecoderOptions.None, Instruction.create_repe_scasq(32) },
			{ 64, "F348AF", DecoderOptions.None, Instruction.create_repe_scasq(64) },
			{ 32, "67F2AE", DecoderOptions.None, Instruction.create_repne_scasb(16) },
			{ 64, "67F2AE", DecoderOptions.None, Instruction.create_repne_scasb(32) },
			{ 64, "F2AE", DecoderOptions.None, Instruction.create_repne_scasb(64) },
			{ 32, "6667F2AF", DecoderOptions.None, Instruction.create_repne_scasw(16) },
			{ 64, "6667F2AF", DecoderOptions.None, Instruction.create_repne_scasw(32) },
			{ 64, "66F2AF", DecoderOptions.None, Instruction.create_repne_scasw(64) },
			{ 32, "67F2AF", DecoderOptions.None, Instruction.create_repne_scasd(16) },
			{ 64, "67F2AF", DecoderOptions.None, Instruction.create_repne_scasd(32) },
			{ 64, "F2AF", DecoderOptions.None, Instruction.create_repne_scasd(64) },
			{ 64, "67F248AF", DecoderOptions.None, Instruction.create_repne_scasq(32) },
			{ 64, "F248AF", DecoderOptions.None, Instruction.create_repne_scasq(64) },
			{ 32, "67F3AC", DecoderOptions.None, Instruction.create_rep_lodsb(16) },
			{ 64, "67F3AC", DecoderOptions.None, Instruction.create_rep_lodsb(32) },
			{ 64, "F3AC", DecoderOptions.None, Instruction.create_rep_lodsb(64) },
			{ 32, "6667F3AD", DecoderOptions.None, Instruction.create_rep_lodsw(16) },
			{ 64, "6667F3AD", DecoderOptions.None, Instruction.create_rep_lodsw(32) },
			{ 64, "66F3AD", DecoderOptions.None, Instruction.create_rep_lodsw(64) },
			{ 32, "67F3AD", DecoderOptions.None, Instruction.create_rep_lodsd(16) },
			{ 64, "67F3AD", DecoderOptions.None, Instruction.create_rep_lodsd(32) },
			{ 64, "F3AD", DecoderOptions.None, Instruction.create_rep_lodsd(64) },
			{ 64, "67F348AD", DecoderOptions.None, Instruction.create_rep_lodsq(32) },
			{ 64, "F348AD", DecoderOptions.None, Instruction.create_rep_lodsq(64) },
			{ 32, "67F36C", DecoderOptions.None, Instruction.create_rep_insb(16) },
			{ 64, "67F36C", DecoderOptions.None, Instruction.create_rep_insb(32) },
			{ 64, "F36C", DecoderOptions.None, Instruction.create_rep_insb(64) },
			{ 32, "6667F36D", DecoderOptions.None, Instruction.create_rep_insw(16) },
			{ 64, "6667F36D", DecoderOptions.None, Instruction.create_rep_insw(32) },
			{ 64, "66F36D", DecoderOptions.None, Instruction.create_rep_insw(64) },
			{ 32, "67F36D", DecoderOptions.None, Instruction.create_rep_insd(16) },
			{ 64, "67F36D", DecoderOptions.None, Instruction.create_rep_insd(32) },
			{ 64, "F36D", DecoderOptions.None, Instruction.create_rep_insd(64) },
			{ 32, "67F3AA", DecoderOptions.None, Instruction.create_rep_stosb(16) },
			{ 64, "67F3AA", DecoderOptions.None, Instruction.create_rep_stosb(32) },
			{ 64, "F3AA", DecoderOptions.None, Instruction.create_rep_stosb(64) },
			{ 32, "6667F3AB", DecoderOptions.None, Instruction.create_rep_stosw(16) },
			{ 64, "6667F3AB", DecoderOptions.None, Instruction.create_rep_stosw(32) },
			{ 64, "66F3AB", DecoderOptions.None, Instruction.create_rep_stosw(64) },
			{ 32, "67F3AB", DecoderOptions.None, Instruction.create_rep_stosd(16) },
			{ 64, "67F3AB", DecoderOptions.None, Instruction.create_rep_stosd(32) },
			{ 64, "F3AB", DecoderOptions.None, Instruction.create_rep_stosd(64) },
			{ 64, "67F348AB", DecoderOptions.None, Instruction.create_rep_stosq(32) },
			{ 64, "F348AB", DecoderOptions.None, Instruction.create_rep_stosq(64) },
			{ 32, "67F3A6", DecoderOptions.None, Instruction.create_repe_cmpsb(16) },
			{ 64, "67F3A6", DecoderOptions.None, Instruction.create_repe_cmpsb(32) },
			{ 64, "F3A6", DecoderOptions.None, Instruction.create_repe_cmpsb(64) },
			{ 32, "6667F3A7", DecoderOptions.None, Instruction.create_repe_cmpsw(16) },
			{ 64, "6667F3A7", DecoderOptions.None, Instruction.create_repe_cmpsw(32) },
			{ 64, "66F3A7", DecoderOptions.None, Instruction.create_repe_cmpsw(64) },
			{ 32, "67F3A7", DecoderOptions.None, Instruction.create_repe_cmpsd(16) },
			{ 64, "67F3A7", DecoderOptions.None, Instruction.create_repe_cmpsd(32) },
			{ 64, "F3A7", DecoderOptions.None, Instruction.create_repe_cmpsd(64) },
			{ 64, "67F348A7", DecoderOptions.None, Instruction.create_repe_cmpsq(32) },
			{ 64, "F348A7", DecoderOptions.None, Instruction.create_repe_cmpsq(64) },
			{ 32, "67F2A6", DecoderOptions.None, Instruction.create_repne_cmpsb(16) },
			{ 64, "67F2A6", DecoderOptions.None, Instruction.create_repne_cmpsb(32) },
			{ 64, "F2A6", DecoderOptions.None, Instruction.create_repne_cmpsb(64) },
			{ 32, "6667F2A7", DecoderOptions.None, Instruction.create_repne_cmpsw(16) },
			{ 64, "6667F2A7", DecoderOptions.None, Instruction.create_repne_cmpsw(32) },
			{ 64, "66F2A7", DecoderOptions.None, Instruction.create_repne_cmpsw(64) },
			{ 32, "67F2A7", DecoderOptions.None, Instruction.create_repne_cmpsd(16) },
			{ 64, "67F2A7", DecoderOptions.None, Instruction.create_repne_cmpsd(32) },
			{ 64, "F2A7", DecoderOptions.None, Instruction.create_repne_cmpsd(64) },
			{ 64, "67F248A7", DecoderOptions.None, Instruction.create_repne_cmpsq(32) },
			{ 64, "F248A7", DecoderOptions.None, Instruction.create_repne_cmpsq(64) },
			{ 32, "67F3A4", DecoderOptions.None, Instruction.create_rep_movsb(16) },
			{ 64, "67F3A4", DecoderOptions.None, Instruction.create_rep_movsb(32) },
			{ 64, "F3A4", DecoderOptions.None, Instruction.create_rep_movsb(64) },
			{ 32, "6667F3A5", DecoderOptions.None, Instruction.create_rep_movsw(16) },
			{ 64, "6667F3A5", DecoderOptions.None, Instruction.create_rep_movsw(32) },
			{ 64, "66F3A5", DecoderOptions.None, Instruction.create_rep_movsw(64) },
			{ 32, "67F3A5", DecoderOptions.None, Instruction.create_rep_movsd(16) },
			{ 64, "67F3A5", DecoderOptions.None, Instruction.create_rep_movsd(32) },
			{ 64, "F3A5", DecoderOptions.None, Instruction.create_rep_movsd(64) },
			{ 64, "67F348A5", DecoderOptions.None, Instruction.create_rep_movsq(32) },
			{ 64, "F348A5", DecoderOptions.None, Instruction.create_rep_movsq(64) },

			{ 64, "C5E814CB", DecoderOptions.None, Instruction.create(Code.VEX_Vunpcklps_xmm_xmm_xmmm128, Register.XMM1, Register.XMM2, Register.XMM3) },
			{ 64, "64C5E8148C7501EFCDAB", DecoderOptions.None, Instruction.create(Code.VEX_Vunpcklps_xmm_xmm_xmmm128, Register.XMM1, Register.XMM2, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS)) },
			{ 64, "64C4E261908C7501EFCDAB", DecoderOptions.None, Instruction.create(Code.VEX_Vpgatherdd_xmm_vm32x_xmm, Register.XMM1, MemoryOperand.new(Register.RBP, Register.XMM6, 2, -0x543210FF, 8, false, Register.FS), Register.XMM3) },
			{ 64, "64C4E2692E9C7501EFCDAB", DecoderOptions.None, Instruction.create(Code.VEX_Vmaskmovps_m128_xmm_xmm, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), Register.XMM2, Register.XMM3) },
			{ 64, "C4E3694ACB40", DecoderOptions.None, Instruction.create(Code.VEX_Vblendvps_xmm_xmm_xmmm128_xmm, Register.XMM1, Register.XMM2, Register.XMM3, Register.XMM4) },
			{ 64, "64C4E3E95C8C7501EFCDAB30", DecoderOptions.None, Instruction.create(Code.VEX_Vfmaddsubps_xmm_xmm_xmm_xmmm128, Register.XMM1, Register.XMM2, Register.XMM3, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS)) },
			{ 64, "64C4E3694A8C7501EFCDAB40", DecoderOptions.None, Instruction.create(Code.VEX_Vblendvps_xmm_xmm_xmmm128_xmm, Register.XMM1, Register.XMM2, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), Register.XMM4) },
			{ 64, "C4E36948CB40", DecoderOptions.None, Instruction.create(Code.VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm4, Register.XMM1, Register.XMM2, Register.XMM3, Register.XMM4, 0x0) },
			{ 64, "64C4E3E9488C7501EFCDAB31", DecoderOptions.None, Instruction.create(Code.VEX_Vpermil2ps_xmm_xmm_xmm_xmmm128_imm4, Register.XMM1, Register.XMM2, Register.XMM3, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x1) },
			{ 64, "64C4E369488C7501EFCDAB41", DecoderOptions.None, Instruction.create(Code.VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm4, Register.XMM1, Register.XMM2, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), Register.XMM4, 0x1) },
			{ 32, "6467C5F9F7D3", DecoderOptions.None, Instruction.create_vmaskmovdqu(16, Register.XMM2, Register.XMM3, Register.FS) },
			{ 64, "6467C5F9F7D3", DecoderOptions.None, Instruction.create_vmaskmovdqu(32, Register.XMM2, Register.XMM3, Register.FS) },
			{ 64, "64C5F9F7D3", DecoderOptions.None, Instruction.create_vmaskmovdqu(64, Register.XMM2, Register.XMM3, Register.FS) },

			{ 64, "62F1F50873D2A5", DecoderOptions.None, Instruction.create(Code.EVEX_Vpsrlq_xmm_k1z_xmmm128b64_imm8, Register.XMM1, Register.XMM2, 0xA5) },
			{ 64, "6462F1F50873947501EFCDABA5", DecoderOptions.None, Instruction.create(Code.EVEX_Vpsrlq_xmm_k1z_xmmm128b64_imm8, Register.XMM1, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0xA5) },
			{ 64, "62F16D08C4CBA5", DecoderOptions.None, Instruction.create(Code.EVEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register.XMM1, Register.XMM2, Register.EBX, 0xA5) },
			{ 64, "6462F16D08C48C7501EFCDABA5", DecoderOptions.None, Instruction.create(Code.EVEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register.XMM1, Register.XMM2, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0xA5) },
		}
		-- stylua: ignore
		if has_int64 then
			tests[#tests + 1] = { 64, "48B9123456789ABCDE31", DecoderOptions.None, Instruction.create(Code.Mov_r64_imm64, Register.RCX, 0x31DEBC9A78563412) }
			tests[#tests + 1] = { 64, "48B904152637A55A5678", DecoderOptions.None, Instruction.create(Code.Mov_r64_imm64, Register.RCX, 0x78565AA537261504) }
			tests[#tests + 1] = { 64, "64A0123456789ABCDEF0", DecoderOptions.None, Instruction.create(Code.Mov_AL_moffs8, Register.AL, MemoryOperand.new(Register.None, Register.None, 1, -0x0F21436587A9CBEE, 8, false, Register.FS)) }
			tests[#tests + 1] = { 64, "64A2123456789ABCDEF0", DecoderOptions.None, Instruction.create(Code.Mov_moffs8_AL, MemoryOperand.new(Register.None, Register.None, 1, -0x0F21436587A9CBEE, 8, false, Register.FS), Register.AL) }
		end
		for _, tc in ipairs(tests) do
			local bitness = tc[1]
			local data = from_hex(tc[2])
			local options = tc[3]
			local created_instr = tc[4]

			local ip
			if bitness == 64 then
				ip = 0x7FFFFFFFF0
			elseif bitness == 32 then
				ip = 0x7FFFFFF0
			elseif bitness == 16 then
				ip = 0x7FF0
			else
				error("Invalid bitness")
			end
			local decoder = Decoder.new(bitness, data, options, ip)
			local orig_rip = decoder:ip()
			local decoded_instr = decoder:decode()
			decoded_instr:set_code_size(CodeSize.Unknown)
			decoded_instr:set_len(0)
			decoded_instr:set_next_ip(0)
			assert.is_true(decoded_instr:eq_all_bits(created_instr))

			local encoder = Encoder.new(decoder:bitness())
			encoder:encode(created_instr, orig_rip)
			local encoded_bytes = encoder:take_buffer()
			assert.equals(data, encoded_bytes)
		end
	end)

	it("create: fail: bitness", function()
		-- stylua: ignore
		local tests = {
			function() Instruction.create(0x789A) end,
			function() Instruction.create(0x789A, Register.RCX, -1) end,
			function() Instruction.create(0x789A, Register.RCX, -1) end,
			function() Instruction.create(0x789A, Register.RCX, 0x31DEBC9A78563412) end,
			function() Instruction.create(0x789A, Register.RCX, 0xFFFFFFFF) end,
			function() Instruction.create(0x789A, Register.RCX) end,
			function() Instruction.create(0x789A, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS)) end,
			function() Instruction.create(0x789A, 0x5A) end,
			function() Instruction.create(0x789A, 0xA55A) end,
			function() Instruction.create(0x789A, 0x3412A55A) end,
			function() Instruction.create(0x789A, 0x5A) end,
			function() Instruction.create(0x789A, 0x5A) end,
			function() Instruction.create(0x789A, 0x5A) end,
			function() Instruction.create(0x789A, -0x5BED5AA6) end,
			function() Instruction.create_branch(0x789A, 0x4D) end,
			function() Instruction.create_branch(0x789A, 0x8000004C) end,
			function() Instruction.create_branch(0x789A, 0x800000000000004C) end,
			function() Instruction.create_far_branch(0x789A, 0x7856, 0x3412) end,
			function() Instruction.create_far_branch(0x789A, 0xBC9A, 0x78563412) end,
			function() Instruction.create(0x789A, Register.CL, Register.DL) end,
			function() Instruction.create(0x789A, Register.CL, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS)) end,
			function() Instruction.create(0x789A, Register.CL, 0x5A) end,
			function() Instruction.create(0x789A, Register.CX, 0xA55A) end,
			function() Instruction.create(0x789A, Register.ECX, 0x3412A55A) end,
			function() Instruction.create(0x789A, Register.RCX, 0x78565AA537261504) end,
			function() Instruction.create(0x789A, Register.CX, 0x5A) end,
			function() Instruction.create(0x789A, Register.ECX, 0x5A) end,
			function() Instruction.create(0x789A, Register.RCX, 0x5A) end,
			function() Instruction.create(0x789A, Register.RCX, 0x3412A55A) end,
			function() Instruction.create(0x789A, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), Register.DL) end,
			function() Instruction.create(0x789A, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x5A) end,
			function() Instruction.create(0x789A, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0xA55A) end,
			function() Instruction.create(0x789A, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x3412A55A) end,
			function() Instruction.create(0x789A, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x5A) end,
			function() Instruction.create(0x789A, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x5A) end,
			function() Instruction.create(0x789A, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x5A) end,
			function() Instruction.create(0x789A, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x3412A55A) end,
			function() Instruction.create(0x789A, 0x5A, Register.AL) end,
			function() Instruction.create(0x789A, 0x5A, Register.AL) end,
			function() Instruction.create(0x789A, 0xA55A, 0xA6) end,
			function() Instruction.create(0x789A, 0xA55A, 0xA6) end,
			function() Instruction.create(0x789A, Register.CX, Register.DX, 0x5AA5) end,
			function() Instruction.create(0x789A, Register.ECX, Register.EDX, 0x3412A55A) end,
			function() Instruction.create(0x789A, Register.CX, Register.DX, 0x5A) end,
			function() Instruction.create(0x789A, Register.ECX, Register.EDX, 0x5A) end,
			function() Instruction.create(0x789A, Register.RCX, Register.RDX, 0x5A) end,
			function() Instruction.create(0x789A, Register.RCX, Register.RDX, -0x5BED5AA6) end,
			function() Instruction.create(0x789A, Register.CX, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0xA55A) end,
			function() Instruction.create(0x789A, Register.ECX, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x3412A55A) end,
			function() Instruction.create(0x789A, Register.CX, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x5A) end,
			function() Instruction.create(0x789A, Register.ECX, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x5A) end,
			function() Instruction.create(0x789A, Register.RCX, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x5A) end,
			function() Instruction.create(0x789A, Register.RCX, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), -0x5BED5AA6) end,
			function() Instruction.create(0x789A, Register.XMM1, 0xA5, 0xFD) end,
			function() Instruction.create(0x789A, Register.XMM1, 0xA5, 0xFD) end,
			function() Instruction.create(0x789A, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), Register.DX, 0x5A) end,
			function() Instruction.create(0x789A, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), Register.DX, 0x5A) end,
			function() Instruction.create(0x789A, Register.XMM1, Register.XMM2, 0xA5, 0xFD) end,
			function() Instruction.create(0x789A, Register.XMM1, Register.XMM2, 0xA5, 0xFD) end,
			function() Instruction.create_branch(0x789A, 0xAA55) end,
			function() Instruction.create_branch(0x789A, 0xAA553412) end,
			function() Instruction.create(0x789A, Register.XMM1, Register.XMM2, Register.XMM3) end,
			function() Instruction.create(0x789A, Register.XMM1, Register.XMM2, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS)) end,
			function() Instruction.create(0x789A, Register.XMM1, MemoryOperand.new(Register.RBP, Register.XMM6, 2, -0x543210FF, 8, false, Register.FS), Register.XMM3) end,
			function() Instruction.create(0x789A, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), Register.XMM2, Register.XMM3) end,
			function() Instruction.create(0x789A, Register.XMM1, Register.XMM2, Register.XMM3, Register.XMM4) end,
			function() Instruction.create(0x789A, Register.XMM1, Register.XMM2, Register.XMM3, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS)) end,
			function() Instruction.create(0x789A, Register.XMM1, Register.XMM2, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), Register.XMM4) end,
			function() Instruction.create(0x789A, Register.XMM1, Register.XMM2, Register.XMM3, Register.XMM4, 0x0) end,
			function() Instruction.create(0x789A, Register.XMM1, Register.XMM2, Register.XMM3, Register.XMM4, 0x0) end,
			function() Instruction.create(0x789A, Register.XMM1, Register.XMM2, Register.XMM3, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x1) end,
			function() Instruction.create(0x789A, Register.XMM1, Register.XMM2, Register.XMM3, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x1) end,
			function() Instruction.create(0x789A, Register.XMM1, Register.XMM2, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), Register.XMM4, 0x1) end,
			function() Instruction.create(0x789A, Register.XMM1, Register.XMM2, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), Register.XMM4, 0x1) end,
			function() Instruction.create(0x789A, Register.XMM1, Register.XMM2, 0xA5) end,
			function() Instruction.create(0x789A, Register.XMM1, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0xA5) end,
			function() Instruction.create(0x789A, Register.XMM1, Register.XMM2, Register.EBX, 0xA5) end,
			function() Instruction.create(0x789A, Register.XMM1, Register.XMM2, Register.EBX, 0xA5) end,
			function() Instruction.create(0x789A, Register.XMM1, Register.XMM2, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0xA5) end,
			function() Instruction.create(0x789A, Register.XMM1, Register.XMM2, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0xA5) end,
		}
		for _, tc in ipairs(tests) do
			assert.has_error(tc)
		end
	end)

	it("create: fail: rep enum variant", function()
		-- stylua: ignore
		local tests = {
			function() Instruction.create_cmpsb(64, Register.FS, 0x789A) end,
			function() Instruction.create_cmpsd(64, Register.FS, 0x789A) end,
			function() Instruction.create_cmpsq(64, Register.FS, 0x789A) end,
			function() Instruction.create_cmpsw(64, Register.FS, 0x789A) end,
			function() Instruction.create_insb(64, 0x789A) end,
			function() Instruction.create_insd(64, 0x789A) end,
			function() Instruction.create_insw(64, 0x789A) end,
			function() Instruction.create_lodsb(64, Register.FS, 0x789A) end,
			function() Instruction.create_lodsd(64, Register.FS, 0x789A) end,
			function() Instruction.create_lodsq(64, Register.FS, 0x789A) end,
			function() Instruction.create_lodsw(64, Register.FS, 0x789A) end,
			function() Instruction.create_movsb(64, Register.FS, 0x789A) end,
			function() Instruction.create_movsd(64, Register.FS, 0x789A) end,
			function() Instruction.create_movsq(64, Register.FS, 0x789A) end,
			function() Instruction.create_movsw(64, Register.FS, 0x789A) end,
			function() Instruction.create_outsb(64, Register.FS, 0x789A) end,
			function() Instruction.create_outsd(64, Register.FS, 0x789A) end,
			function() Instruction.create_outsw(64, Register.FS, 0x789A) end,
			function() Instruction.create_scasb(64, 0x789A) end,
			function() Instruction.create_scasd(64, 0x789A) end,
			function() Instruction.create_scasq(64, 0x789A) end,
			function() Instruction.create_scasw(64, 0x789A) end,
			function() Instruction.create_stosb(64, 0x789A) end,
			function() Instruction.create_stosd(64, 0x789A) end,
			function() Instruction.create_stosq(64, 0x789A) end,
			function() Instruction.create_stosw(64, 0x789A) end,
		}
		for _, tc in ipairs(tests) do
			assert.has_error(tc)
		end
	end)

	it("create: fail: register enum variant", function()
		-- stylua: ignore
		local tests = {
			function() Instruction.create(Code.Mov_r64_imm64, 0x789A, -1) end,
			function() Instruction.create(Code.Mov_r64_imm64, 0x789A, -1) end,
			function() Instruction.create(Code.Mov_r64_imm64, 0x789A, 0x31DEBC9A78563412) end,
			function() Instruction.create(Code.Mov_r64_imm64, 0x789A, 0xFFFFFFFF) end,
			function() Instruction.create(Code.Pop_rm64, 0x789A) end,
			function() Instruction.create(Code.Add_rm8_r8, 0x789A, Register.DL) end,
			function() Instruction.create(Code.Add_rm8_r8, Register.CL, 0x789A) end,
			function() Instruction.create(Code.Add_r8_rm8, 0x789A, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS)) end,
			function() Instruction.create(Code.Add_rm8_imm8, 0x789A, 0x5A) end,
			function() Instruction.create(Code.Add_rm16_imm16, 0x789A, 0xA55A) end,
			function() Instruction.create(Code.Add_rm32_imm32, 0x789A, 0x3412A55A) end,
			function() Instruction.create(Code.Mov_r64_imm64, 0x789A, 0x78565AA537261504) end,
			function() Instruction.create(Code.Add_rm16_imm8, 0x789A, 0x5A) end,
			function() Instruction.create(Code.Add_rm32_imm8, 0x789A, 0x5A) end,
			function() Instruction.create(Code.Add_rm64_imm8, 0x789A, 0x5A) end,
			function() Instruction.create(Code.Add_rm64_imm32, 0x789A, 0x3412A55A) end,
			function() Instruction.create(Code.Add_rm8_r8, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x789A) end,
			function() Instruction.create(Code.Out_imm8_AL, 0x5A, 0x789A) end,
			function() Instruction.create(Code.Out_imm8_AL, 0x5A, 0x789A) end,
			function() Instruction.create(Code.Imul_r16_rm16_imm16, 0x789A, Register.DX, 0x5AA5) end,
			function() Instruction.create(Code.Imul_r16_rm16_imm16, Register.CX, 0x789A, 0x5AA5) end,
			function() Instruction.create(Code.Imul_r32_rm32_imm32, 0x789A, Register.EDX, 0x3412A55A) end,
			function() Instruction.create(Code.Imul_r32_rm32_imm32, Register.ECX, 0x789A, 0x3412A55A) end,
			function() Instruction.create(Code.Imul_r16_rm16_imm8, 0x789A, Register.DX, 0x5A) end,
			function() Instruction.create(Code.Imul_r16_rm16_imm8, Register.CX, 0x789A, 0x5A) end,
			function() Instruction.create(Code.Imul_r32_rm32_imm8, 0x789A, Register.EDX, 0x5A) end,
			function() Instruction.create(Code.Imul_r32_rm32_imm8, Register.ECX, 0x789A, 0x5A) end,
			function() Instruction.create(Code.Imul_r64_rm64_imm8, 0x789A, Register.RDX, 0x5A) end,
			function() Instruction.create(Code.Imul_r64_rm64_imm8, Register.RCX, 0x789A, 0x5A) end,
			function() Instruction.create(Code.Imul_r64_rm64_imm32, 0x789A, Register.RDX, -0x5BED5AA6) end,
			function() Instruction.create(Code.Imul_r64_rm64_imm32, Register.RCX, 0x789A, -0x5BED5AA6) end,
			function() Instruction.create(Code.Imul_r16_rm16_imm16, 0x789A, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0xA55A) end,
			function() Instruction.create(Code.Imul_r32_rm32_imm32, 0x789A, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x3412A55A) end,
			function() Instruction.create(Code.Imul_r16_rm16_imm8, 0x789A, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x5A) end,
			function() Instruction.create(Code.Imul_r32_rm32_imm8, 0x789A, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x5A) end,
			function() Instruction.create(Code.Imul_r64_rm64_imm8, 0x789A, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x5A) end,
			function() Instruction.create(Code.Imul_r64_rm64_imm32, 0x789A, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), -0x5BED5AA6) end,
			function() Instruction.create(Code.Extrq_xmm_imm8_imm8, 0x789A, 0xA5, 0xFD) end,
			function() Instruction.create(Code.Extrq_xmm_imm8_imm8, 0x789A, 0xA5, 0xFD) end,
			function() Instruction.create(Code.Shld_rm16_r16_imm8, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x789A, 0x5A) end,
			function() Instruction.create(Code.Shld_rm16_r16_imm8, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x789A, 0x5A) end,
			function() Instruction.create(Code.Insertq_xmm_xmm_imm8_imm8, 0x789A, Register.XMM2, 0xA5, 0xFD) end,
			function() Instruction.create(Code.Insertq_xmm_xmm_imm8_imm8, Register.XMM1, 0x789A, 0xA5, 0xFD) end,
			function() Instruction.create(Code.Insertq_xmm_xmm_imm8_imm8, 0x789A, Register.XMM2, 0xA5, 0xFD) end,
			function() Instruction.create(Code.Insertq_xmm_xmm_imm8_imm8, Register.XMM1, 0x789A, 0xA5, 0xFD) end,
			function() Instruction.create_outsb(64, 0x789A, RepPrefixKind.None) end,
			function() Instruction.create_outsw(64, 0x789A, RepPrefixKind.None) end,
			function() Instruction.create_outsd(64, 0x789A, RepPrefixKind.None) end,
			function() Instruction.create_lodsb(64, 0x789A, RepPrefixKind.None) end,
			function() Instruction.create_lodsw(64, 0x789A, RepPrefixKind.None) end,
			function() Instruction.create_lodsd(64, 0x789A, RepPrefixKind.None) end,
			function() Instruction.create_lodsq(64, 0x789A, RepPrefixKind.None) end,
			function() Instruction.create_cmpsb(64, 0x789A, RepPrefixKind.None) end,
			function() Instruction.create_cmpsw(64, 0x789A, RepPrefixKind.None) end,
			function() Instruction.create_cmpsd(64, 0x789A, RepPrefixKind.None) end,
			function() Instruction.create_cmpsq(64, 0x789A, RepPrefixKind.None) end,
			function() Instruction.create_movsb(64, 0x789A, RepPrefixKind.None) end,
			function() Instruction.create_movsw(64, 0x789A, RepPrefixKind.None) end,
			function() Instruction.create_movsd(64, 0x789A, RepPrefixKind.None) end,
			function() Instruction.create_movsq(64, 0x789A, RepPrefixKind.None) end,
			function() Instruction.create_maskmovq(64, 0x789A, Register.MM3, Register.FS) end,
			function() Instruction.create_maskmovq(64, Register.MM2, 0x789A, Register.FS) end,
			function() Instruction.create_maskmovq(64, Register.MM2, Register.MM3, 0x789A) end,
			function() Instruction.create_maskmovdqu(64, 0x789A, Register.XMM3, Register.FS) end,
			function() Instruction.create_maskmovdqu(64, Register.XMM2, 0x789A, Register.FS) end,
			function() Instruction.create_maskmovdqu(64, Register.XMM2, Register.XMM3, 0x789A) end,
			function() Instruction.create_outsb(64, 0x789A, RepPrefixKind.Repe) end,
			function() Instruction.create_outsw(64, 0x789A, RepPrefixKind.Repe) end,
			function() Instruction.create_outsd(64, 0x789A, RepPrefixKind.Repe) end,
			function() Instruction.create_lodsb(64, 0x789A, RepPrefixKind.Repe) end,
			function() Instruction.create_lodsw(64, 0x789A, RepPrefixKind.Repe) end,
			function() Instruction.create_lodsd(64, 0x789A, RepPrefixKind.Repe) end,
			function() Instruction.create_lodsq(64, 0x789A, RepPrefixKind.Repe) end,
			function() Instruction.create_cmpsb(64, 0x789A, RepPrefixKind.Repe) end,
			function() Instruction.create_cmpsw(64, 0x789A, RepPrefixKind.Repe) end,
			function() Instruction.create_cmpsd(64, 0x789A, RepPrefixKind.Repe) end,
			function() Instruction.create_cmpsq(64, 0x789A, RepPrefixKind.Repe) end,
			function() Instruction.create_movsb(64, 0x789A, RepPrefixKind.Repe) end,
			function() Instruction.create_movsw(64, 0x789A, RepPrefixKind.Repe) end,
			function() Instruction.create_movsd(64, 0x789A, RepPrefixKind.Repe) end,
			function() Instruction.create_movsq(64, 0x789A, RepPrefixKind.Repe) end,
			function() Instruction.create_outsb(64, 0x789A, RepPrefixKind.Repne) end,
			function() Instruction.create_outsw(64, 0x789A, RepPrefixKind.Repne) end,
			function() Instruction.create_outsd(64, 0x789A, RepPrefixKind.Repne) end,
			function() Instruction.create_lodsb(64, 0x789A, RepPrefixKind.Repne) end,
			function() Instruction.create_lodsw(64, 0x789A, RepPrefixKind.Repne) end,
			function() Instruction.create_lodsd(64, 0x789A, RepPrefixKind.Repne) end,
			function() Instruction.create_lodsq(64, 0x789A, RepPrefixKind.Repne) end,
			function() Instruction.create_cmpsb(64, 0x789A, RepPrefixKind.Repne) end,
			function() Instruction.create_cmpsw(64, 0x789A, RepPrefixKind.Repne) end,
			function() Instruction.create_cmpsd(64, 0x789A, RepPrefixKind.Repne) end,
			function() Instruction.create_cmpsq(64, 0x789A, RepPrefixKind.Repne) end,
			function() Instruction.create_movsb(64, 0x789A, RepPrefixKind.Repne) end,
			function() Instruction.create_movsw(64, 0x789A, RepPrefixKind.Repne) end,
			function() Instruction.create_movsd(64, 0x789A, RepPrefixKind.Repne) end,
			function() Instruction.create_movsq(64, 0x789A, RepPrefixKind.Repne) end,
			function() Instruction.create(Code.VEX_Vunpcklps_xmm_xmm_xmmm128, 0x789A, Register.XMM2, Register.XMM3) end,
			function() Instruction.create(Code.VEX_Vunpcklps_xmm_xmm_xmmm128, Register.XMM1, 0x789A, Register.XMM3) end,
			function() Instruction.create(Code.VEX_Vunpcklps_xmm_xmm_xmmm128, Register.XMM1, Register.XMM2, 0x789A) end,
			function() Instruction.create(Code.VEX_Vunpcklps_xmm_xmm_xmmm128, 0x789A, Register.XMM2, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS)) end,
			function() Instruction.create(Code.VEX_Vunpcklps_xmm_xmm_xmmm128, Register.XMM1, 0x789A, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS)) end,
			function() Instruction.create(Code.VEX_Vpgatherdd_xmm_vm32x_xmm, 0x789A, MemoryOperand.new(Register.RBP, Register.XMM6, 2, -0x543210FF, 8, false, Register.FS), Register.XMM3) end,
			function() Instruction.create(Code.VEX_Vpgatherdd_xmm_vm32x_xmm, Register.XMM1, MemoryOperand.new(Register.RBP, Register.XMM6, 2, -0x543210FF, 8, false, Register.FS), 0x789A) end,
			function() Instruction.create(Code.VEX_Vmaskmovps_m128_xmm_xmm, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x789A, Register.XMM3) end,
			function() Instruction.create(Code.VEX_Vmaskmovps_m128_xmm_xmm, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), Register.XMM2, 0x789A) end,
			function() Instruction.create(Code.VEX_Vblendvps_xmm_xmm_xmmm128_xmm, 0x789A, Register.XMM2, Register.XMM3, Register.XMM4) end,
			function() Instruction.create(Code.VEX_Vblendvps_xmm_xmm_xmmm128_xmm, Register.XMM1, 0x789A, Register.XMM3, Register.XMM4) end,
			function() Instruction.create(Code.VEX_Vblendvps_xmm_xmm_xmmm128_xmm, Register.XMM1, Register.XMM2, 0x789A, Register.XMM4) end,
			function() Instruction.create(Code.VEX_Vblendvps_xmm_xmm_xmmm128_xmm, Register.XMM1, Register.XMM2, Register.XMM3, 0x789A) end,
			function() Instruction.create(Code.VEX_Vfmaddsubps_xmm_xmm_xmm_xmmm128, 0x789A, Register.XMM2, Register.XMM3, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS)) end,
			function() Instruction.create(Code.VEX_Vfmaddsubps_xmm_xmm_xmm_xmmm128, Register.XMM1, 0x789A, Register.XMM3, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS)) end,
			function() Instruction.create(Code.VEX_Vfmaddsubps_xmm_xmm_xmm_xmmm128, Register.XMM1, Register.XMM2, 0x789A, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS)) end,
			function() Instruction.create(Code.VEX_Vblendvps_xmm_xmm_xmmm128_xmm, 0x789A, Register.XMM2, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), Register.XMM4) end,
			function() Instruction.create(Code.VEX_Vblendvps_xmm_xmm_xmmm128_xmm, Register.XMM1, 0x789A, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), Register.XMM4) end,
			function() Instruction.create(Code.VEX_Vblendvps_xmm_xmm_xmmm128_xmm, Register.XMM1, Register.XMM2, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x789A) end,
			function() Instruction.create(Code.VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm4, 0x789A, Register.XMM2, Register.XMM3, Register.XMM4, 0x0) end,
			function() Instruction.create(Code.VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm4, Register.XMM1, 0x789A, Register.XMM3, Register.XMM4, 0x0) end,
			function() Instruction.create(Code.VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm4, Register.XMM1, Register.XMM2, 0x789A, Register.XMM4, 0x0) end,
			function() Instruction.create(Code.VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm4, Register.XMM1, Register.XMM2, Register.XMM3, 0x789A, 0x0) end,
			function() Instruction.create(Code.VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm4, 0x789A, Register.XMM2, Register.XMM3, Register.XMM4, 0x0) end,
			function() Instruction.create(Code.VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm4, Register.XMM1, 0x789A, Register.XMM3, Register.XMM4, 0x0) end,
			function() Instruction.create(Code.VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm4, Register.XMM1, Register.XMM2, 0x789A, Register.XMM4, 0x0) end,
			function() Instruction.create(Code.VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm4, Register.XMM1, Register.XMM2, Register.XMM3, 0x789A, 0x0) end,
			function() Instruction.create(Code.VEX_Vpermil2ps_xmm_xmm_xmm_xmmm128_imm4, 0x789A, Register.XMM2, Register.XMM3, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x1) end,
			function() Instruction.create(Code.VEX_Vpermil2ps_xmm_xmm_xmm_xmmm128_imm4, Register.XMM1, 0x789A, Register.XMM3, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x1) end,
			function() Instruction.create(Code.VEX_Vpermil2ps_xmm_xmm_xmm_xmmm128_imm4, Register.XMM1, Register.XMM2, 0x789A, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x1) end,
			function() Instruction.create(Code.VEX_Vpermil2ps_xmm_xmm_xmm_xmmm128_imm4, 0x789A, Register.XMM2, Register.XMM3, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x1) end,
			function() Instruction.create(Code.VEX_Vpermil2ps_xmm_xmm_xmm_xmmm128_imm4, Register.XMM1, 0x789A, Register.XMM3, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x1) end,
			function() Instruction.create(Code.VEX_Vpermil2ps_xmm_xmm_xmm_xmmm128_imm4, Register.XMM1, Register.XMM2, 0x789A, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x1) end,
			function() Instruction.create(Code.VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm4, 0x789A, Register.XMM2, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), Register.XMM4, 0x1) end,
			function() Instruction.create(Code.VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm4, Register.XMM1, 0x789A, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), Register.XMM4, 0x1) end,
			function() Instruction.create(Code.VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm4, Register.XMM1, Register.XMM2, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x789A, 0x1) end,
			function() Instruction.create(Code.VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm4, 0x789A, Register.XMM2, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), Register.XMM4, 0x1) end,
			function() Instruction.create(Code.VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm4, Register.XMM1, 0x789A, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), Register.XMM4, 0x1) end,
			function() Instruction.create(Code.VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm4, Register.XMM1, Register.XMM2, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x789A, 0x1) end,
			function() Instruction.create_vmaskmovdqu(64, 0x789A, Register.XMM3, Register.FS) end,
			function() Instruction.create_vmaskmovdqu(64, Register.XMM2, 0x789A, Register.FS) end,
			function() Instruction.create_vmaskmovdqu(64, Register.XMM2, Register.XMM3, 0x789A) end,
			function() Instruction.create(Code.EVEX_Vpsrlq_xmm_k1z_xmmm128b64_imm8, 0x789A, Register.XMM2, 0xA5) end,
			function() Instruction.create(Code.EVEX_Vpsrlq_xmm_k1z_xmmm128b64_imm8, Register.XMM1, 0x789A, 0xA5) end,
			function() Instruction.create(Code.EVEX_Vpsrlq_xmm_k1z_xmmm128b64_imm8, 0x789A, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0xA5) end,
			function() Instruction.create(Code.EVEX_Vpinsrw_xmm_xmm_r32m16_imm8, 0x789A, Register.XMM2, Register.EBX, 0xA5) end,
			function() Instruction.create(Code.EVEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register.XMM1, 0x789A, Register.EBX, 0xA5) end,
			function() Instruction.create(Code.EVEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register.XMM1, Register.XMM2, 0x789A, 0xA5) end,
			function() Instruction.create(Code.EVEX_Vpinsrw_xmm_xmm_r32m16_imm8, 0x789A, Register.XMM2, Register.EBX, 0xA5) end,
			function() Instruction.create(Code.EVEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register.XMM1, 0x789A, Register.EBX, 0xA5) end,
			function() Instruction.create(Code.EVEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register.XMM1, Register.XMM2, 0x789A, 0xA5) end,
			function() Instruction.create(Code.EVEX_Vpinsrw_xmm_xmm_r32m16_imm8, 0x789A, Register.XMM2, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0xA5) end,
			function() Instruction.create(Code.EVEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register.XMM1, 0x789A, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0xA5) end,
			function() Instruction.create(Code.EVEX_Vpinsrw_xmm_xmm_r32m16_imm8, 0x789A, Register.XMM2, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0xA5) end,
			function() Instruction.create(Code.EVEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register.XMM1, 0x789A, MemoryOperand.new(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0xA5) end,
		}
		for _, tc in ipairs(tests) do
			assert.has_error(tc)
		end
	end)

	it("create: u64: Lua 5.1/5.2", function()
		-- stylua: ignore
		local tests = {
			{ "mov rax,0FEDCBA9876543000h", Instruction.create(Code.Mov_r64_imm64, Register.RAX, 0xFEDCBA9876543000) },
			{ "mov rax,0EDCBA9876543D000h", Instruction.create(Code.Mov_r64_imm64, Register.RAX, -0x123456789ABC3000) },
		}
		for _, tc in ipairs(tests) do
			local expected = tc[1]
			local instr = tc[2]
			local actual = tostring(instr)
			assert.equals(expected, actual)
		end
	end)
end)
