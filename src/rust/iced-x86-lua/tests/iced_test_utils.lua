-- SPDX-License-Identifier: MIT
-- Copyright (C) 2018-present iced project and contributors

local M = {}

local unpack = unpack or table.unpack

M.has_int64 = string.format("0x%16X", 0xFEDCBA987654321F) == "0xFEDCBA987654321F"

local function hex2bin(c, s)
	if c >= 0x30 and c <= 0x39 then
		return c - 0x30
	end
	if c >= 0x41 and c <= 0x46 then
		return c - 0x41 + 0x0A
	end
	if c >= 0x61 and c <= 0x66 then
		return c - 0x61 + 0x0A
	end
	error("Expected a hex string: " .. s)
end

function M.from_hex(s)
	local result = {}
	-- This ain't pretty...
	local skip = false
	for i = 1, #s do
		if not skip then
			local hi = s:byte(i)
			if not (hi == 0x20 or hi == 0x09) then
				local lo = s:byte(i + 1)
				result[#result + 1] = hex2bin(hi, s) * 0x10 + hex2bin(lo, s)
				skip = true
			end
		else
			skip = false
		end
	end
	return string.char(unpack(result))
end

return M
