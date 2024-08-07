-- SPDX-License-Identifier: MIT
-- Copyright (C) 2018-present iced project and contributors

-- ⚠️This file was generated by GENERATOR!🦹‍♂️

---@meta
---@diagnostic disable unused-local

---Contains the FPU `TOP` increment, whether it's conditional and whether the instruction writes to `TOP`
---
---@class FpuStackIncrementInfo
local FpuStackIncrementInfo = {}

---Creates a new instance
---
---@param increment integer #(`i32`) Used if `writes_top` is `true`. Value added to `TOP`.
---@param conditional boolean #`true` if it's a conditional push/pop (eg. `FPTAN` or `FSINCOS`)
---@param writes_top boolean #`true` if `TOP` is written (it's a conditional/unconditional push/pop, `FNSAVE`, `FLDENV`, etc)
---@return FpuStackIncrementInfo
function FpuStackIncrementInfo.new(increment, conditional, writes_top) end

---Used if `FpuStackIncrementInfo:writes_top()` is `true`. Value added to `TOP`.
---
---This is negative if it pushes one or more values and positive if it pops one or more values
---and `0` if it writes to `TOP` (eg. `FLDENV`, etc) without pushing/popping anything.
---
---@return integer
function FpuStackIncrementInfo:increment() end

---`true` if it's a conditional push/pop (eg. `FPTAN` or `FSINCOS`)
---
---@return boolean
function FpuStackIncrementInfo:conditional() end

---`true` if `TOP` is written (it's a conditional/unconditional push/pop, `FNSAVE`, `FLDENV`, etc)
---
---@return boolean
function FpuStackIncrementInfo:writes_top() end

return FpuStackIncrementInfo
