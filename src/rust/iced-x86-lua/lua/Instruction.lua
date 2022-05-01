-- SPDX-License-Identifier: MIT
-- Copyright (C) 2018-present iced project and contributors

local iced_instr = require("iced_x86_priv.instr")

---A 16/32/64-bit x86 instruction. Created by `Decoder` or by `Instruction:create*()` methods
---@class Instruction
local Instruction = {}

---Creates a new empty instruction
---@return Instruction
function Instruction:new()
	local raw_instr = iced_instr.instruction_new()
	return Instruction:_from_raw(raw_instr)
end

---Internal func, do not use
---@param raw_instr userdata
---@return Instruction
function Instruction:_from_raw(raw_instr)
	local instr = { _instr = raw_instr }
	return setmetatable(instr, { __index = self })
end

return Instruction
