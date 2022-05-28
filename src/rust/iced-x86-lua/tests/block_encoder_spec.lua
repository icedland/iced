-- SPDX-License-Identifier: MIT
-- Copyright (C) 2018-present iced project and contributors

local from_hex = require("iced_test_utils").from_hex

describe("BlockEncoder", function()
	local BlockEncoder = require("iced_x86.BlockEncoder")
	local BlockEncoderOptions = require("iced_x86.BlockEncoderOptions")
	local Decoder = require("iced_x86.Decoder")

	it("encode: empty", function()
		local result = BlockEncoder.encode(64, {}, 0x12345678)
		assert.equals("", result.code_buffer)
	end)

	it("encode: invalid bitness", function()
		assert.has_error(function()
			local _ = BlockEncoder.encode(1, {}, 0x12345678)
		end)
	end)

	it("encode: with options", function()
		local decoder = Decoder.new(64, from_hex("72 00"), nil, 0x12345678)
		local instr = decoder:decode()
		local result = BlockEncoder.encode(64, { instr }, 0x12345578, BlockEncoderOptions.None)
		assert.equals(from_hex("0F82 FC000000"), result.code_buffer)
		assert.has_error(function()
			local _ = BlockEncoder.encode(64, { instr }, 0x12345578, BlockEncoderOptions.DontFixBranches)
		end)
	end)

	it("encode: 16", function()
		local decoder = Decoder.new(16, from_hex("72 00"), nil, 0x5678)
		local instr = decoder:decode()
		local result = BlockEncoder.encode(16, { instr }, 0x5578)
		assert.equals(from_hex("0F82 FE00"), result.code_buffer)
	end)

	it("encode: 32", function()
		local decoder = Decoder.new(32, from_hex("72 00"), nil, 0x12345678)
		local instr = decoder:decode()
		local result = BlockEncoder.encode(32, { instr }, 0x12345578)
		assert.equals(from_hex("0F82 FC000000"), result.code_buffer)
	end)

	it("encode: 64", function()
		local decoder = Decoder.new(64, from_hex("72 00"), nil, 0x12345678)
		local instr = decoder:decode()
		local result = BlockEncoder.encode(64, { instr }, 0x12345578)
		assert.equals(from_hex("0F82 FC000000"), result.code_buffer)
	end)
end)
