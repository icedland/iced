-- SPDX-License-Identifier: MIT
-- Copyright (C) 2018-present iced project and contributors

describe("MemoryOperand", function()
	local MemoryOperand = require("iced_x86.MemoryOperand")
	local Register = require("iced_x86.Register")

	it("new", function()
		local mem
		local expected

		mem = MemoryOperand.new()
		expected = MemoryOperand.new(Register.None, Register.None, 1, 0, 0, false, Register.None)
		assert.is_true(mem == expected)

		mem = MemoryOperand.new(Register.ECX)
		expected = MemoryOperand.new(Register.ECX, Register.None, 1, 0, 0, false, Register.None)
		assert.is_true(mem == expected)

		mem = MemoryOperand.new(nil)
		expected = MemoryOperand.new(Register.None, Register.None, 1, 0, 0, false, Register.None)
		assert.is_true(mem == expected)

		mem = MemoryOperand.new(nil, Register.XMM3)
		expected = MemoryOperand.new(Register.None, Register.XMM3, 1, 0, 0, false, Register.None)
		assert.is_true(mem == expected)

		mem = MemoryOperand.new(nil, nil, 2)
		expected = MemoryOperand.new(Register.None, Register.None, 2, 0, 0, false, Register.None)
		assert.is_true(mem == expected)

		mem = MemoryOperand.new(nil, nil, nil, 123)
		expected = MemoryOperand.new(Register.None, Register.None, 1, 123, 0, false, Register.None)
		assert.is_true(mem == expected)

		mem = MemoryOperand.new(nil, nil, nil, nil, 8)
		expected = MemoryOperand.new(Register.None, Register.None, 1, 0, 8, false, Register.None)
		assert.is_true(mem == expected)

		mem = MemoryOperand.new(nil, nil, nil, nil, nil, true)
		expected = MemoryOperand.new(Register.None, Register.None, 1, 0, 0, true, Register.None)
		assert.is_true(mem == expected)

		mem = MemoryOperand.new(nil, nil, nil, nil, nil, nil, Register.FS)
		expected = MemoryOperand.new(Register.None, Register.None, 1, 0, 0, false, Register.FS)
		assert.is_true(mem == expected)

		mem = MemoryOperand.new(nil, nil, nil, nil, nil, nil, nil)
		expected = MemoryOperand.new(Register.None, Register.None, 1, 0, 0, false, Register.None)
		assert.is_true(mem == expected)

		mem = MemoryOperand.new(Register.None, Register.None, 1, 0, 1, false, Register.None)
		expected = MemoryOperand.new(Register.None, Register.None, 1, 0, 1, false, Register.None)
		assert.is_true(mem == expected)

		mem = MemoryOperand.new(Register.None, Register.None, 1, 0x1234, 0, false, Register.None)
		expected = MemoryOperand.new(Register.None, Register.None, 1, 0x1234, 1, false, Register.None)
		assert.is_true(mem == expected)

		mem = MemoryOperand.new(Register.None, Register.None, 1, 0x1234, 2, false, Register.None)
		expected = MemoryOperand.new(Register.None, Register.None, 1, 0x1234, 2, false, Register.None)
		assert.is_true(mem == expected)
	end)

	-- stylua: ignore
	it("new: fail", function()
		assert.has_error(function() MemoryOperand.new(0x789A, Register.None, 1, 0, 0, false, Register.None) end)
		assert.has_error(function() MemoryOperand.new(Register.None, 0x789A, 1, 0, 0, false, Register.None) end)
		assert.has_error(function() MemoryOperand.new(Register.None, Register.None, -0x80000001, 0, 0, false, Register.None) end)
		assert.has_error(function() MemoryOperand.new(Register.None, Register.None, 0x100000000, 0, 0, false, Register.None) end)
		assert.has_error(function() MemoryOperand.new(Register.None, Register.None, 1, {}, 0, false, Register.None) end)
		assert.has_error(function() MemoryOperand.new(Register.None, Register.None, 1, 0, -0x80000001, false, Register.None) end)
		assert.has_error(function() MemoryOperand.new(Register.None, Register.None, 1, 0, 0x100000000, false, Register.None) end)
		assert.has_error(function() MemoryOperand.new(Register.None, Register.None, 1, 0, 0, {}, Register.None) end)
		assert.has_error(function() MemoryOperand.new(Register.None, Register.None, 1, 0, 0, false, 0x789A) end)
	end)

	it("copy(), eq", function()
		local mem1 = MemoryOperand.new(Register.None, Register.None, 1, 0, 0, false, Register.None)
		local mem2 = mem1:copy()
		local mem3 = MemoryOperand.new(Register.None, Register.None, 1, 1, 0, false, Register.None)
		assert.is_true(mem1 == mem2)
		assert.is_true(mem2 == mem1)
		assert.is_false(mem1 == mem3)
		assert.is_false(mem3 == mem1)
	end)

	it("with_base_index_scale_displ_size", function()
		-- stylua: ignore
		local mem = MemoryOperand.with_base_index_scale_displ_size(Register.ECX, Register.XMM31, 2, 0x123456789ABCDE0F, 1)
		local expected = MemoryOperand.new(Register.ECX, Register.XMM31, 2, 0x123456789ABCDE0F, 1, false, Register.None)
		assert.is_true(mem == expected)

		mem = MemoryOperand.with_base_index_scale_displ_size(Register.R15, Register.RDI, 8, -0x123456789ABCDE0F, 8)
		expected = MemoryOperand.new(Register.R15, Register.RDI, 8, -0x123456789ABCDE0F, 8, false, Register.None)
		assert.is_true(mem == expected)
	end)

	-- stylua: ignore
	it("with_base_index_scale_displ_size: fail", function()
		MemoryOperand.with_base_index_scale_displ_size(Register.ECX, Register.XMM31, 2, 0x123456789ABCDE0F, 1)
		assert.has_error(function() MemoryOperand.with_base_index_scale_displ_size(0x789A, Register.XMM31, 2, 0x123456789ABCDE0F, 1) end)
		assert.has_error(function() MemoryOperand.with_base_index_scale_displ_size(Register.ECX, 0x789A, 2, 0x123456789ABCDE0F, 1) end)
		assert.has_error(function() MemoryOperand.with_base_index_scale_displ_size(Register.ECX, Register.XMM31, -0x80000001, 0x123456789ABCDE0F, 1) end)
		assert.has_error(function() MemoryOperand.with_base_index_scale_displ_size(Register.ECX, Register.XMM31, 0x100000000, 0x123456789ABCDE0F, 1) end)
		assert.has_error(function() MemoryOperand.with_base_index_scale_displ_size(Register.ECX, Register.XMM31, 2, {}, 1) end)
		assert.has_error(function() MemoryOperand.with_base_index_scale_displ_size(Register.ECX, Register.XMM31, 2, 0x123456789ABCDE0F, -0x80000001) end)
		assert.has_error(function() MemoryOperand.with_base_index_scale_displ_size(Register.ECX, Register.XMM31, 2, 0x123456789ABCDE0F, 0x100000000) end)
	end)

	it("with_base_index_scale", function()
		local mem = MemoryOperand.with_base_index_scale(Register.BX, Register.SI, 4)
		local expected = MemoryOperand.new(Register.BX, Register.SI, 4, 0, 0, false, Register.None)
		assert.is_true(mem == expected)

		mem = MemoryOperand.with_base_index_scale(Register.None, Register.BP, 2)
		expected = MemoryOperand.new(Register.None, Register.BP, 2, 0, 0, false, Register.None)
		assert.is_true(mem == expected)
	end)

	-- stylua: ignore
	it("with_base_index_scale: fail", function()
		MemoryOperand.with_base_index_scale(Register.BX, Register.SI, 4)
		assert.has_error(function() MemoryOperand.with_base_index_scale(0x789A, Register.SI, 4) end)
		assert.has_error(function() MemoryOperand.with_base_index_scale(Register.BX, 0x789A, 4) end)
		assert.has_error(function() MemoryOperand.with_base_index_scale(Register.BX, Register.SI, -0x80000001) end)
		assert.has_error(function() MemoryOperand.with_base_index_scale(Register.BX, Register.SI, 0x100000000) end)
	end)

	it("with_base_index", function()
		local mem = MemoryOperand.with_base_index(Register.RAX, Register.RCX)
		local expected = MemoryOperand.new(Register.RAX, Register.RCX, 1, 0, 0, false, Register.None)
		assert.is_true(mem == expected)

		mem = MemoryOperand.with_base_index(Register.RSP, Register.RBP)
		expected = MemoryOperand.new(Register.RSP, Register.RBP, 1, 0, 0, false, Register.None)
		assert.is_true(mem == expected)
	end)

	-- stylua: ignore
	it("with_base_index: fail", function()
		MemoryOperand.with_base_index(Register.RAX, Register.RCX)
		assert.has_error(function() MemoryOperand.with_base_index(0x789A, Register.RCX) end)
		assert.has_error(function() MemoryOperand.with_base_index(Register.RAX, 0x789A) end)
	end)

	it("with_base_displ_size", function()
		local mem = MemoryOperand.with_base_displ_size(Register.RSI, 0x123456789ABCDE0F, 1)
		local expected = MemoryOperand.new(Register.RSI, Register.None, 1, 0x123456789ABCDE0F, 1, false, Register.None)
		assert.is_true(mem == expected)

		mem = MemoryOperand.with_base_displ_size(Register.RBX, -0x123456789ABCDE0F, 8)
		expected = MemoryOperand.new(Register.RBX, Register.None, 1, -0x123456789ABCDE0F, 8, false, Register.None)
		assert.is_true(mem == expected)
	end)

	-- stylua: ignore
	it("with_base_displ_size: fail", function()
		MemoryOperand.with_base_displ_size(Register.RSI, 0x123456789ABCDE0F, 1)
		assert.has_error(function() MemoryOperand.with_base_displ_size(0x789A, 0x123456789ABCDE0F, 1) end)
		assert.has_error(function() MemoryOperand.with_base_displ_size(Register.RSI, {}, 1) end)
		assert.has_error(function() MemoryOperand.with_base_displ_size(Register.RSI, 0x123456789ABCDE0F, -0x80000001) end)
		assert.has_error(function() MemoryOperand.with_base_displ_size(Register.RSI, 0x123456789ABCDE0F, 0x100000000) end)
	end)

	it("with_index_scale_displ_size", function()
		local mem = MemoryOperand.with_index_scale_displ_size(Register.R8, 2, 0x123456789ABCDE0F, 8)
		local expected = MemoryOperand.new(Register.None, Register.R8, 2, 0x123456789ABCDE0F, 8, false, Register.None)
		assert.is_true(mem == expected)

		mem = MemoryOperand.with_index_scale_displ_size(Register.R10, 2, -0x123456789ABCDE0F, 1)
		expected = MemoryOperand.new(Register.None, Register.R10, 2, -0x123456789ABCDE0F, 1, false, Register.None)
		assert.is_true(mem == expected)
	end)

	-- stylua: ignore
	it("with_index_scale_displ_size: fail", function()
		MemoryOperand.with_index_scale_displ_size(Register.R8, 2, 0x123456789ABCDE0F, 8)
		assert.has_error(function() MemoryOperand.with_index_scale_displ_size(0x789A, 2, 0x123456789ABCDE0F, 8) end)
		assert.has_error(function() MemoryOperand.with_index_scale_displ_size(Register.R8, -0x80000001, 0x123456789ABCDE0F, 8) end)
		assert.has_error(function() MemoryOperand.with_index_scale_displ_size(Register.R8, 0x100000000, 0x123456789ABCDE0F, 8) end)
		assert.has_error(function() MemoryOperand.with_index_scale_displ_size(Register.R8, 2, {}, 8) end)
		assert.has_error(function() MemoryOperand.with_index_scale_displ_size(Register.R8, 2, 0x123456789ABCDE0F, -0x80000001) end)
		assert.has_error(function() MemoryOperand.with_index_scale_displ_size(Register.R8, 2, 0x123456789ABCDE0F, 0x100000000) end)
	end)

	it("with_base_displ", function()
		local mem = MemoryOperand.with_base_displ(Register.R9, 0x123456789ABCDE0F)
		local expected = MemoryOperand.new(Register.R9, Register.None, 1, 0x123456789ABCDE0F, 1, false, Register.None)
		assert.is_true(mem == expected)

		mem = MemoryOperand.with_base_displ(Register.R11, -0x123456789ABCDE0F)
		expected = MemoryOperand.new(Register.R11, Register.None, 1, -0x123456789ABCDE0F, 1, false, Register.None)
		assert.is_true(mem == expected)
	end)

	-- stylua: ignore
	it("with_base_displ: fail", function()
		MemoryOperand.with_base_displ(Register.R9, 0x123456789ABCDE0F)
		assert.has_error(function() MemoryOperand.with_base_displ(0x789A, 0x123456789ABCDE0F) end)
		assert.has_error(function() MemoryOperand.with_base_displ(Register.R9, {}) end)
	end)

	it("with_base", function()
		local mem = MemoryOperand.with_base(Register.R12)
		local expected = MemoryOperand.new(Register.R12, Register.None, 1, 0, 0, false, Register.None)
		assert.is_true(mem == expected)

		mem = MemoryOperand.with_base(Register.R13)
		expected = MemoryOperand.new(Register.R13, Register.None, 1, 0, 0, false, Register.None)
		assert.is_true(mem == expected)
	end)

	-- stylua: ignore
	it("with_base: fail", function()
		MemoryOperand.with_base(Register.R12)
		assert.has_error(function() MemoryOperand.with_base(0x789A) end)
	end)

	it("with_displ", function()
		local mem = MemoryOperand.with_displ(0x123456789ABCDE0F, 8)
		local expected = MemoryOperand.new(Register.None, Register.None, 1, 0x123456789ABCDE0F, 8, false, Register.None)
		assert.is_true(mem == expected)

		mem = MemoryOperand.with_displ(0x12345678, 4)
		expected = MemoryOperand.new(Register.None, Register.None, 1, 0x12345678, 4, false, Register.None)
		assert.is_true(mem == expected)

		mem = MemoryOperand.with_displ(0x1234, 2)
		expected = MemoryOperand.new(Register.None, Register.None, 1, 0x1234, 2, false, Register.None)
		assert.is_true(mem == expected)

		mem = MemoryOperand.with_displ(0x123456789ABCDE0F)
		expected = MemoryOperand.new(Register.None, Register.None, 1, 0x123456789ABCDE0F, 1, false, Register.None)
		assert.is_true(mem == expected)

		mem = MemoryOperand.with_displ(0x123456789ABCDE0F, nil)
		expected = MemoryOperand.new(Register.None, Register.None, 1, 0x123456789ABCDE0F, 1, false, Register.None)
		assert.is_true(mem == expected)
	end)

	-- stylua: ignore
	it("with_displ: fail", function()
		MemoryOperand.with_displ(0x123456789ABCDE0F, 8)
		assert.has_error(function() MemoryOperand.with_displ({}, 8) end)
		assert.has_error(function() MemoryOperand.with_displ(0x123456789ABCDE0F, -0x80000001) end)
		assert.has_error(function() MemoryOperand.with_displ(0x123456789ABCDE0F, 0x100000000) end)
	end)
end)
