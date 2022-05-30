iced-x86 disassembler Lua bindings

iced-x86 is a blazing fast and correct x86 (16/32/64-bit) disassembler for Lua.

- 👍 Supports all Intel and AMD instructions
- 👍 Correct: All instructions are tested and iced has been tested against other disassemblers/assemblers (xed, gas, objdump, masm, dumpbin, nasm, ndisasm) and fuzzed
- 👍 The formatter supports masm, nasm, gas (AT&T), Intel (XED) and there are many options to customize the output
- 👍 The encoder can be used to re-encode decoded instructions at any address
- 👍 API to get instruction info, eg. read/written registers, memory and rflags bits; CPUID feature flag, control flow info, etc
- 👍 Rust + Lua
- 👍 License: MIT

# Install

Prerequisites:

- Rust: https://www.rust-lang.org/tools/install
- Lua 5.1 or later

```
cd this-dir
luarocks make *.rockspec
```

## How-tos

- [Disassemble (decode and format instructions)](#disassemble-decode-and-format-instructions)
- [Create and encode instructions](#create-and-encode-instructions)
- [Move code in memory (eg. hook a function)](#move-code-in-memory-eg-hook-a-function)
- [Get instruction info, eg. read/written regs/mem, control flow info, etc](#get-instruction-info-eg-readwritten-regsmem-control-flow-info-etc)
- [Disassemble old/deprecated CPU instructions](#disassemble-olddeprecated-cpu-instructions)

## Disassemble (decode and format instructions)

This example uses a `Decoder` and one of the `Formatter`s to decode and format the code.

```lua
local Decoder = require("iced_x86.Decoder")
local Formatter = require("iced_x86.Formatter")
local FormatterSyntax = require("iced_x86.FormatterSyntax")

-- This example produces the following output:
-- 00007FFAC46ACDA4 48895C2410           mov       [rsp+10h],rbx
-- 00007FFAC46ACDA9 4889742418           mov       [rsp+18h],rsi
-- 00007FFAC46ACDAE 55                   push      rbp
-- 00007FFAC46ACDAF 57                   push      rdi
-- 00007FFAC46ACDB0 4156                 push      r14
-- 00007FFAC46ACDB2 488DAC2400FFFFFF     lea       rbp,[rsp-100h]
-- 00007FFAC46ACDBA 4881EC00020000       sub       rsp,200h
-- 00007FFAC46ACDC1 488B0518570A00       mov       rax,[rel 7FFA`C475`24E0h]
-- 00007FFAC46ACDC8 4833C4               xor       rax,rsp
-- 00007FFAC46ACDCB 488985F0000000       mov       [rbp+0F0h],rax
-- 00007FFAC46ACDD2 4C8B052F240A00       mov       r8,[rel 7FFA`C474`F208h]
-- 00007FFAC46ACDD9 488D05787C0400       lea       rax,[rel 7FFA`C46F`4A58h]
-- 00007FFAC46ACDE0 33FF                 xor       edi,edi

local example_code_bitness = 64
local example_code_rip = 0x00007FFAC46ACDA4
local example_code = ""
    .. "\072\137\092\036\016\072\137\116\036\024\085\087\065\086\072"
    .. "\141\172\036\000\255\255\255\072\129\236\000\002\000\000\072"
    .. "\139\005\024\087\010\000\072\051\196\072\137\133\240\000\000"
    .. "\000\076\139\005\047\036\010\000\072\141\005\120\124\004\000"
    .. "\051\255"

-- Create the decoder and initialize RIP (the `nil` arg is a `DecoderOptions` enum value)
local decoder = Decoder.new(example_code_bitness, example_code, nil, example_code_rip)

-- Formatters: Masm, Nasm, Gas (AT&T) and Intel (XED).
local formatter = Formatter.new(FormatterSyntax.Nasm)

-- Change some options, there are many more
formatter:set_digit_separator("`")
formatter:set_first_operand_char_index(10)

local function bytes2hex(data, start, end_)
    local s = ""
    for i = start, end_ do
        s = s .. string.format("%02X", data:byte(i))
    end
    return s
end

-- You can also call decoder:can_decode() + decoder:decode()/decode_out(instr)
-- but the iterator is faster
for instr in decoder:iter_out() do
    local disasm = formatter:format(instr)
    local start_index = instr:ip() - example_code_rip + 1
    local bytes_str = bytes2hex(example_code, start_index, start_index + instr:len() - 1)
    -- Eg. "00007FFAC46ACDB2 488DAC2400FFFFFF     lea       rbp,[rsp-100h]"
    print(string.format("%016X %-20s %s", instr:ip(), bytes_str, disasm))
end
```

## Create and encode instructions

This example uses a `BlockEncoder` to encode created `Instruction`s.

```lua
local BlockEncoder = require("iced_x86.BlockEncoder")
local Code = require("iced_x86.Code")
local Decoder = require("iced_x86.Decoder")
local Formatter = require("iced_x86.Formatter")
local FormatterSyntax = require("iced_x86.FormatterSyntax")
local Instruction = require("iced_x86.Instruction")
local MemoryOperand = require("iced_x86.MemoryOperand")
local Register = require("iced_x86.Register")

local bitness = 64

-- All created instrs get an IP of 0. The label id is just an IP.
-- The branch instruction's *target* IP should be equal to the IP of the
-- target instruction.
local label_id = 1

local function create_label()
    local idd = label_id
    label_id = label_id + 1
    return idd
end

---@param id integer #id
---@param instr Instruction #Instruction
---@return Instruction
local function add_label(id, instr)
    instr:set_ip(id)
    return instr
end

local label1 = create_label()

local instrs = {}
instrs[#instrs + 1] = Instruction.create(Code.Push_r64, Register.RBP)
instrs[#instrs + 1] = Instruction.create(Code.Push_r64, Register.RDI)
instrs[#instrs + 1] = Instruction.create(Code.Push_r64, Register.RSI)
instrs[#instrs + 1] = Instruction.create(Code.Sub_rm64_imm32, Register.RSP, 0x50)
instrs[#instrs + 1] = Instruction.create(Code.VEX_Vzeroupper)
instrs[#instrs + 1] = Instruction.create(Code.Lea_r64_m, Register.RBP, MemoryOperand.with_base_displ(Register.RSP, 0x60))
instrs[#instrs + 1] = Instruction.create(Code.Mov_r64_rm64, Register.RSI, Register.RCX)
instrs[#instrs + 1] = Instruction.create(Code.Lea_r64_m, Register.RDI, MemoryOperand.with_base_displ(Register.RBP, -0x38))
instrs[#instrs + 1] = Instruction.create(Code.Mov_r32_imm32, Register.ECX, 0x0A)
instrs[#instrs + 1] = Instruction.create(Code.Xor_r32_rm32, Register.EAX, Register.EAX)
instrs[#instrs + 1] = Instruction.create_rep_stosd(bitness)
instrs[#instrs + 1] = Instruction.create(Code.Cmp_rm64_imm32, Register.RSI, 0x12345678)
-- Create a branch instruction that references label1
instrs[#instrs + 1] = Instruction.create_branch(Code.Jne_rel32_64, label1)
instrs[#instrs + 1] = Instruction.create(Code.Nopd)
-- Add the instruction that is the target of the branch
instrs[#instrs + 1] = add_label(label1, Instruction.create(Code.Xor_r32_rm32, Register.R15D, Register.R15D))

-- Create an instruction that accesses some data using an RIP relative memory operand
local data1 = create_label()
instrs[#instrs + 1] = Instruction.create(Code.Lea_r64_m, Register.R14, MemoryOperand.with_base_displ(Register.RIP, data1))
instrs[#instrs + 1] = Instruction.create(Code.Nopd)
local raw_data = "\018\052\086\120"
instrs[#instrs + 1] = add_label(data1, Instruction.db(raw_data))

-- Use BlockEncoder to encode a block of instrs. This block can contain any
-- number of branches and any number of instrs.
-- It uses Encoder to encode all instrs.
-- If the target of a branch is too far away, it can fix it to use a longer branch.
-- This can be disabled by passing in options (4th arg)
local target_rip = 0x00001248FC840000
local enc_result = BlockEncoder.encode(bitness, instrs, target_rip)
---@type string
local encoded_bytes = enc_result.code_buffer

-- Now disassemble the encoded instrs. Note that the 'jmp near'
-- instruction was turned into a 'jmp short' instruction because we
-- didn't disable branch optimizations.
local bytes_code = encoded_bytes:sub(1, #encoded_bytes - #raw_data)
local bytes_data = encoded_bytes:sub(#encoded_bytes - #raw_data + 1, #encoded_bytes)
local decoder = Decoder.new(bitness, bytes_code, nil, target_rip)
local formatter = Formatter.new(FormatterSyntax.Gas)
formatter:set_first_operand_char_index(8)
for instr in decoder:iter_out() do
    local disasm = formatter:format(instr)
    print(string.format("%016X %s", instr:ip(), disasm))
end

local db = Instruction.db(bytes_data)
print(string.format("%016X %s", decoder:ip(), formatter:format(db)))

-- Output:
-- 00001248FC840000 push    %rbp
-- 00001248FC840001 push    %rdi
-- 00001248FC840002 push    %rsi
-- 00001248FC840003 sub     $0x50,%rsp
-- 00001248FC84000A vzeroupper
-- 00001248FC84000D lea     0x60(%rsp),%rbp
-- 00001248FC840012 mov     %rcx,%rsi
-- 00001248FC840015 lea     -0x38(%rbp),%rdi
-- 00001248FC840019 mov     $0xA,%ecx
-- 00001248FC84001E xor     %eax,%eax
-- 00001248FC840020 rep stos %eax,(%rdi)
-- 00001248FC840022 cmp     $0x12345678,%rsi
-- 00001248FC840029 jne     0x00001248FC84002C
-- 00001248FC84002B nop
-- 00001248FC84002C xor     %r15d,%r15d
-- 00001248FC84002F lea     0x1248FC840037,%r14
-- 00001248FC840036 nop
-- 00001248FC840037 .byte   0x12,0x34,0x56,0x78
```

## Move code in memory (eg. hook a function)

Uses instruction info API and the encoder to patch a function to jump to the programmer's function.

```lua
local BlockEncoder = require("iced_x86.BlockEncoder")
local Code = require("iced_x86.Code")
local Decoder = require("iced_x86.Decoder")
local FlowControl = require("iced_x86.FlowControl")
local Formatter = require("iced_x86.Formatter")
local FormatterSyntax = require("iced_x86.FormatterSyntax")
local Instruction = require("iced_x86.Instruction")
local OpKind = require("iced_x86.OpKind")

local unpack = unpack or table.unpack

-- Decodes instructions from some address, then encodes them starting at some
-- other address. This can be used to hook a function. You decode enough instructions
-- until you have enough bytes to add a JMP instruction that jumps to your code.
-- Your code will then conditionally jump to the original code that you re-encoded.
--
-- This code uses the BlockEncoder which will help with some things, eg. converting
-- short branches to longer branches if the target is too far away.
--
-- 64-bit mode also supports RIP relative addressing, but the encoder can't rewrite
-- those to use a longer displacement. If any of the moved instructions have RIP
-- relative addressing and it tries to access data too far away, the encoder will fail.
-- The easiest solution is to use OS alloc functions that allocate memory close to the
-- original code (+/-2GB).

-- This example produces the following output:
-- Original code:
-- 00007FFAC46ACDA4 mov [rsp+10h],rbx
-- 00007FFAC46ACDA9 mov [rsp+18h],rsi
-- 00007FFAC46ACDAE push rbp
-- 00007FFAC46ACDAF push rdi
-- 00007FFAC46ACDB0 push r14
-- 00007FFAC46ACDB2 lea rbp,[rsp-100h]
-- 00007FFAC46ACDBA sub rsp,200h
-- 00007FFAC46ACDC1 mov rax,[rel 7FFAC47524E0h]
-- 00007FFAC46ACDC8 xor rax,rsp
-- 00007FFAC46ACDCB mov [rbp+0F0h],rax
-- 00007FFAC46ACDD2 mov r8,[rel 7FFAC474F208h]
-- 00007FFAC46ACDD9 lea rax,[rel 7FFAC46F4A58h]
-- 00007FFAC46ACDE0 xor edi,edi
--
-- Original + patched code:
-- 00007FFAC46ACDA4 mov rax,123456789ABCh
-- 00007FFAC46ACDAE jmp rax
-- 00007FFAC46ACDB0 push r14
-- 00007FFAC46ACDB2 lea rbp,[rsp-100h]
-- 00007FFAC46ACDBA sub rsp,200h
-- 00007FFAC46ACDC1 mov rax,[rel 7FFAC47524E0h]
-- 00007FFAC46ACDC8 xor rax,rsp
-- 00007FFAC46ACDCB mov [rbp+0F0h],rax
-- 00007FFAC46ACDD2 mov r8,[rel 7FFAC474F208h]
-- 00007FFAC46ACDD9 lea rax,[rel 7FFAC46F4A58h]
-- 00007FFAC46ACDE0 xor edi,edi
--
-- Moved code:
-- 00007FFAC48ACDA4 mov [rsp+10h],rbx
-- 00007FFAC48ACDA9 mov [rsp+18h],rsi
-- 00007FFAC48ACDAE push rbp
-- 00007FFAC48ACDAF push rdi
-- 00007FFAC48ACDB0 jmp 00007FFAC46ACDB0h

local example_code_bitness = 64
local example_code_rip = 0x00007FFAC46ACDA4
local example_code = ""
    .. "\072\137\092\036\016\072\137\116\036\024\085\087\065\086\072"
    .. "\141\172\036\000\255\255\255\072\129\236\000\002\000\000\072"
    .. "\139\005\024\087\010\000\072\051\196\072\137\133\240\000\000"
    .. "\000\076\139\005\047\036\010\000\072\141\005\120\124\004\000"
    .. "\051\255"

---@param data string #Bytes
---@param ip integer #IP
local function disassemble(data, ip)
    local formatter = Formatter.new(FormatterSyntax.Nasm)
    local decoder = Decoder.new(example_code_bitness, data, nil, ip)
    for instr in decoder:iter_out() do
        local disasm = formatter:format(instr)
        print(string.format("%016X %s", instr:ip(), disasm))
    end
    print()
end

local function how_to_move_code()
    print("Original code:")
    disassemble(example_code, example_code_rip)

    local decoder = Decoder.new(example_code_bitness, example_code, nil, example_code_rip)

    -- In 64-bit mode, we need 12 bytes to jump to any address:
    --      mov rax,imm64   -- 10
    --      jmp rax         -- 2
    -- We overwrite rax because it's probably not used by the called function.
    -- In 32-bit mode, a normal JMP is just 5 bytes
    local required_bytes = 10 + 2
    local total_bytes = 0
    local orig_instrs = {}
    -- iter_out() is faster but it re-uses the same Instruction instance. Use
    -- this other iterator instead to get a new instruction every iteration.
    for instr in decoder:iter_slow_copy() do
        orig_instrs[#orig_instrs + 1] = instr
        total_bytes = total_bytes + instr:len()
        if instr:is_invalid() then
            error("Found garbage")
        end
        if total_bytes >= required_bytes then
            break
        end

        local cflow = instr:flow_control()
        if cflow == FlowControl.Next then
            -- nothing
        elseif cflow == FlowControl.UnconditionalBranch then
            if instr:op0_kind() == OpKind.NearBranch64 then
                local _ = instr:near_branch_target()
                -- You could check if it's just jumping forward a few bytes and follow it
                -- but this is a simple example so we'll fail.
            end
            error("Not supported by this simple example")
        else
            error("Not supported by this simple example")
        end
    end
    if total_bytes < required_bytes then
        error("Not enough bytes!")
    end
    if #orig_instrs == 0 then
        error("Should not be empty here")
    end
    -- Create a JMP instruction that branches to the original code, except those instructions
    -- that we'll re-encode. We don't need to do it if it already ends in 'ret'
    local last_instr = orig_instrs[#orig_instrs]
    if last_instr.flow_control ~= FlowControl.Return then
        orig_instrs[#orig_instrs + 1] = Instruction.create_branch(Code.Jmp_rel32_64, last_instr:next_ip())
    end

    -- Relocate the code to some new location. It can fix short/near branches and
    -- convert them to short/near/long forms if needed. This also works even if it's a
    -- jrcxz/loop/loopcc instruction which only have short forms.
    --
    -- It can currently only fix RIP relative operands if the new location is within 2GB
    -- of the target data location.
    local relocated_base_address = example_code_rip + 0x200000
    local result = BlockEncoder.encode(decoder:bitness(), orig_instrs, relocated_base_address)
    local new_code = result.code_buffer

    -- Patch the original code. Pretend that we use some OS API to write to memory...
    -- We could use the BlockEncoder/Encoder for this but it's easy to do yourself too.
    -- This is 'mov rax,imm64; jmp rax'
    local your_func = 0x0000123456789ABC  -- Address of your code
    -- MOV RAX,imm64
    local code = { 0x48, 0xB8 }
    local v = your_func
    for _ = 1, 8 do
        code[#code + 1] = v % 0x100
        v = math.floor(v / 0x100)
    end
    -- JMP RAX
    code[#code + 1] = 0xFF
    code[#code + 1] = 0xE0
    code = string.char(unpack(code)) .. example_code:sub(#code + 1, #example_code)

    -- Disassemble it
    print("Original + patched code:")
    disassemble(code, example_code_rip)

    -- Disassemble the moved code
    print("Moved code:")
    disassemble(new_code, relocated_base_address)
end

how_to_move_code()
```

## Get instruction info, eg. read/written regs/mem, control flow info, etc

Shows how to get used registers/memory and other info. It uses `Instruction` methods to get this info.

```lua
local Code = require("iced_x86.Code")
local ConditionCode = require("iced_x86.ConditionCode")
local CpuidFeature = require("iced_x86.CpuidFeature")
local Decoder = require("iced_x86.Decoder")
local EncodingKind = require("iced_x86.EncodingKind")
local FlowControl = require("iced_x86.FlowControl")
local MemorySize = require("iced_x86.MemorySize")
local MemorySizeExt = require("iced_x86.MemorySizeExt")
local Mnemonic = require("iced_x86.Mnemonic")
local OpAccess = require("iced_x86.OpAccess")
local OpCodeOperandKind = require("iced_x86.OpCodeOperandKind")
local OpKind = require("iced_x86.OpKind")
local Register = require("iced_x86.Register")
local RflagsBits = require("iced_x86.RflagsBits")

-- This method produces the following output:
-- 00007FFAC46ACDA4 mov [rsp+10h],rbx
--     OpCode: o64 89 /r
--     Instruction: MOV r/m64, r64
--     Encoding: Legacy
--     Mnemonic: Mov
--     Code: Mov_rm64_r64
--     CpuidFeature: X64
--     FlowControl: Next
--     Displacement offset = 4, size = 1
--     Memory size: 8
--     Op0Access: Write
--     Op1Access: Read
--     Op0: r64_or_mem
--     Op1: r64_reg
--     Used reg: RSP:Read
--     Used reg: RBX:Read
--     Used mem: [SS:RSP+0x10;UInt64;Write]
-- 00007FFAC46ACDA9 mov [rsp+18h],rsi
--     OpCode: o64 89 /r
--     Instruction: MOV r/m64, r64
--     Encoding: Legacy
--     Mnemonic: Mov
--     Code: Mov_rm64_r64
--     CpuidFeature: X64
--     FlowControl: Next
--     Displacement offset = 4, size = 1
--     Memory size: 8
--     Op0Access: Write
--     Op1Access: Read
--     Op0: r64_or_mem
--     Op1: r64_reg
--     Used reg: RSP:Read
--     Used reg: RSI:Read
--     Used mem: [SS:RSP+0x18;UInt64;Write]
-- 00007FFAC46ACDAE push rbp
--     OpCode: o64 50+ro
--     Instruction: PUSH r64
--     Encoding: Legacy
--     Mnemonic: Push
--     Code: Push_r64
--     CpuidFeature: X64
--     FlowControl: Next
--     SP Increment: -8
--     Op0Access: Read
--     Op0: r64_opcode
--     Used reg: RBP:Read
--     Used reg: RSP:ReadWrite
--     Used mem: [SS:RSP-0x8;UInt64;Write]
-- 00007FFAC46ACDAF push rdi
--     OpCode: o64 50+ro
--     Instruction: PUSH r64
--     Encoding: Legacy
--     Mnemonic: Push
--     Code: Push_r64
--     CpuidFeature: X64
--     FlowControl: Next
--     SP Increment: -8
--     Op0Access: Read
--     Op0: r64_opcode
--     Used reg: RDI:Read
--     Used reg: RSP:ReadWrite
--     Used mem: [SS:RSP-0x8;UInt64;Write]
-- 00007FFAC46ACDB0 push r14
--     OpCode: o64 50+ro
--     Instruction: PUSH r64
--     Encoding: Legacy
--     Mnemonic: Push
--     Code: Push_r64
--     CpuidFeature: X64
--     FlowControl: Next
--     SP Increment: -8
--     Op0Access: Read
--     Op0: r64_opcode
--     Used reg: R14:Read
--     Used reg: RSP:ReadWrite
--     Used mem: [SS:RSP-0x8;UInt64;Write]
-- 00007FFAC46ACDB2 lea rbp,[rsp-100h]
--     OpCode: o64 8D /r
--     Instruction: LEA r64, m
--     Encoding: Legacy
--     Mnemonic: Lea
--     Code: Lea_r64_m
--     CpuidFeature: X64
--     FlowControl: Next
--     Displacement offset = 4, size = 4
--     Op0Access: Write
--     Op1Access: NoMemAccess
--     Op0: r64_reg
--     Op1: mem
--     Used reg: RBP:Write
--     Used reg: RSP:Read
-- 00007FFAC46ACDBA sub rsp,200h
--     OpCode: o64 81 /5 id
--     Instruction: SUB r/m64, imm32
--     Encoding: Legacy
--     Mnemonic: Sub
--     Code: Sub_rm64_imm32
--     CpuidFeature: X64
--     FlowControl: Next
--     Immediate offset = 3, size = 4
--     RFLAGS Written: OF, SF, ZF, AF, CF, PF
--     RFLAGS Modified: OF, SF, ZF, AF, CF, PF
--     Op0Access: ReadWrite
--     Op1Access: Read
--     Op0: r64_or_mem
--     Op1: imm32sex64
--     Used reg: RSP:ReadWrite
-- 00007FFAC46ACDC1 mov rax,[7FFAC47524E0h]
--     OpCode: o64 8B /r
--     Instruction: MOV r64, r/m64
--     Encoding: Legacy
--     Mnemonic: Mov
--     Code: Mov_r64_rm64
--     CpuidFeature: X64
--     FlowControl: Next
--     Displacement offset = 3, size = 4
--     Memory size: 8
--     Op0Access: Write
--     Op1Access: Read
--     Op0: r64_reg
--     Op1: r64_or_mem
--     Used reg: RAX:Write
--     Used mem: [DS:0x7FFAC47524E0;UInt64;Read]
-- 00007FFAC46ACDC8 xor rax,rsp
--     OpCode: o64 33 /r
--     Instruction: XOR r64, r/m64
--     Encoding: Legacy
--     Mnemonic: Xor
--     Code: Xor_r64_rm64
--     CpuidFeature: X64
--     FlowControl: Next
--     RFLAGS Written: SF, ZF, PF
--     RFLAGS Cleared: OF, CF
--     RFLAGS Undefined: AF
--     RFLAGS Modified: OF, SF, ZF, AF, CF, PF
--     Op0Access: ReadWrite
--     Op1Access: Read
--     Op0: r64_reg
--     Op1: r64_or_mem
--     Used reg: RAX:ReadWrite
--     Used reg: RSP:Read
-- 00007FFAC46ACDCB mov [rbp+0F0h],rax
--     OpCode: o64 89 /r
--     Instruction: MOV r/m64, r64
--     Encoding: Legacy
--     Mnemonic: Mov
--     Code: Mov_rm64_r64
--     CpuidFeature: X64
--     FlowControl: Next
--     Displacement offset = 3, size = 4
--     Memory size: 8
--     Op0Access: Write
--     Op1Access: Read
--     Op0: r64_or_mem
--     Op1: r64_reg
--     Used reg: RBP:Read
--     Used reg: RAX:Read
--     Used mem: [SS:RBP+0xF0;UInt64;Write]
-- 00007FFAC46ACDD2 mov r8,[7FFAC474F208h]
--     OpCode: o64 8B /r
--     Instruction: MOV r64, r/m64
--     Encoding: Legacy
--     Mnemonic: Mov
--     Code: Mov_r64_rm64
--     CpuidFeature: X64
--     FlowControl: Next
--     Displacement offset = 3, size = 4
--     Memory size: 8
--     Op0Access: Write
--     Op1Access: Read
--     Op0: r64_reg
--     Op1: r64_or_mem
--     Used reg: R8:Write
--     Used mem: [DS:0x7FFAC474F208;UInt64;Read]
-- 00007FFAC46ACDD9 lea rax,[7FFAC46F4A58h]
--     OpCode: o64 8D /r
--     Instruction: LEA r64, m
--     Encoding: Legacy
--     Mnemonic: Lea
--     Code: Lea_r64_m
--     CpuidFeature: X64
--     FlowControl: Next
--     Displacement offset = 3, size = 4
--     Op0Access: Write
--     Op1Access: NoMemAccess
--     Op0: r64_reg
--     Op1: mem
--     Used reg: RAX:Write
-- 00007FFAC46ACDE0 xor edi,edi
--     OpCode: o32 33 /r
--     Instruction: XOR r32, r/m32
--     Encoding: Legacy
--     Mnemonic: Xor
--     Code: Xor_r32_rm32
--     CpuidFeature: INTEL386
--     FlowControl: Next
--     RFLAGS Cleared: OF, SF, CF
--     RFLAGS Set: ZF, PF
--     RFLAGS Undefined: AF
--     RFLAGS Modified: OF, SF, ZF, AF, CF, PF
--     Op0Access: Write
--     Op1Access: None
--     Op0: r32_reg
--     Op1: r32_or_mem
--     Used reg: RDI:Write

local example_code_bitness = 64
local example_code_rip = 0x00007FFAC46ACDA4
local example_code = ""
    .. "\072\137\092\036\016\072\137\116\036\024\085\087\065\086\072"
    .. "\141\172\036\000\255\255\255\072\129\236\000\002\000\000\072"
    .. "\139\005\024\087\010\000\072\051\196\072\137\133\240\000\000"
    .. "\000\076\139\005\047\036\010\000\072\141\005\120\124\004\000"
    .. "\051\255"

---@param value integer #value
---@param flag integer #flag
---@return boolean
local function is_bit_set(value, flag)
    -- Lua 5.1 doesn't have bit ops or a bit32 lib
    -- assume integers
    return (value % (flag * 2)) >= flag
end

---@param rf integer #RflagsBits value
---@return string
local function rflags_bits_to_string(rf)
    ---@param sb string #sb
    ---@param s string #s
    local function append(sb, s)
        if #sb ~= 0 then
            sb = sb .. ", "
        end
        return sb .. s
    end

    local sb = ""
    if is_bit_set(rf, RflagsBits.OF) then
        sb = append(sb, "OF")
    end
    if is_bit_set(rf, RflagsBits.SF) then
        sb = append(sb, "SF")
    end
    if is_bit_set(rf, RflagsBits.ZF) then
        sb = append(sb, "ZF")
    end
    if is_bit_set(rf, RflagsBits.AF) then
        sb = append(sb, "AF")
    end
    if is_bit_set(rf, RflagsBits.CF) then
        sb = append(sb, "CF")
    end
    if is_bit_set(rf, RflagsBits.PF) then
        sb = append(sb, "PF")
    end
    if is_bit_set(rf, RflagsBits.DF) then
        sb = append(sb, "DF")
    end
    if is_bit_set(rf, RflagsBits.IF) then
        sb = append(sb, "IF")
    end
    if is_bit_set(rf, RflagsBits.AC) then
        sb = append(sb, "AC")
    end
    if is_bit_set(rf, RflagsBits.UIF) then
        sb = append(sb, "UIF")
    end
    if #sb == 0 then
        return "<empty>"
    end
    return sb
end

local function create_enum_dict(module)
    local result = {}
    for k, v in pairs(module) do
        result[v] = k
    end
    return result
end

REGISTER_TO_STRING = create_enum_dict(Register)
---@param value integer #Register
---@return string
local function register_to_string(value)
    return REGISTER_TO_STRING[value] or tostring(value) .. " /*Register enum*/"
end

OP_ACCESS_TO_STRING = create_enum_dict(OpAccess)
---@param value integer #OpAccess
---@return string
local function op_access_to_string(value)
    return OP_ACCESS_TO_STRING[value] or tostring(value) .. " /*OpAccess enum*/"
end

ENCODING_KIND_TO_STRING = create_enum_dict(EncodingKind)
---@param value integer #EncodingKind
---@return string
local function encoding_kind_to_string(value)
    return ENCODING_KIND_TO_STRING[value] or tostring(value) .. " /*EncodingKind enum*/"
end

MNEMONIC_TO_STRING = create_enum_dict(Mnemonic)
---@param value integer #Mnemonic
---@return string
local function mnemonic_to_string(value)
    return MNEMONIC_TO_STRING[value] or tostring(value) .. " /*Mnemonic enum*/"
end

CODE_TO_STRING = create_enum_dict(Code)
---@param value integer #Code
---@return string
local function code_to_string(value)
    return CODE_TO_STRING[value] or tostring(value) .. " /*Code enum*/"
end

FLOW_CONTROL_TO_STRING = create_enum_dict(FlowControl)
---@param value integer #FlowControl
---@return string
local function flow_control_to_string(value)
    return FLOW_CONTROL_TO_STRING[value] or tostring(value) .. " /*FlowControl enum*/"
end

OP_CODE_OPERAND_KIND_TO_STRING = create_enum_dict(OpCodeOperandKind)
---@param value integer #OpCodeOperandKind
---@return string
local function op_code_operand_kind_to_string(value)
    return OP_CODE_OPERAND_KIND_TO_STRING[value] or tostring(value) .. " /*OpCodeOperandKind enum*/"
end

CPUID_FEATURE_TO_STRING = create_enum_dict(CpuidFeature)
---@param value integer #CpuidFeature
---@return string
local function cpuid_feature_to_string(value)
    return CPUID_FEATURE_TO_STRING[value] or tostring(value) .. " /*CpuidFeature enum*/"
end

---@param cpuid_features integer[] #CPUID features
---@return string
local function cpuid_features_to_string(cpuid_features)
    local s = ""
    for i, f in ipairs(cpuid_features) do
        if i > 1 then
            s = s .. " and "
        end
        s = s .. cpuid_feature_to_string(f)
    end
    return s
end

MEMORY_SIZE_TO_STRING = create_enum_dict(MemorySize)
---@param value integer #MemorySize
---@return string
local function memory_size_to_string(value)
    return MEMORY_SIZE_TO_STRING[value] or tostring(value) .. " /*MemorySize enum*/"
end

CONDITION_CODE_TO_STRING = create_enum_dict(ConditionCode)
---@param value integer #ConditionCode
---@return string
local function condition_code_to_string(value)
    return CONDITION_CODE_TO_STRING[value] or tostring(value) .. " /*ConditionCode enum*/"
end

---@param reg_info UsedRegister #UsedRegister
---@return string
local function used_reg_to_string(reg_info)
    return register_to_string(reg_info:register()) .. ":" .. op_access_to_string(reg_info:access())
end

---@param mem_info UsedMemory #UsedMemory
---@return string
local function used_mem_to_string(mem_info)
    local sb = "[" .. register_to_string(mem_info:segment()) .. ":"
    local need_plus = mem_info:base() ~= Register.None
    if need_plus then
        sb = sb .. register_to_string(mem_info:base())
    end
    if mem_info:index() ~= Register.None then
        if need_plus then
            sb = sb .. "+"
        end
        need_plus = true
        sb = sb .. register_to_string(mem_info:index())
        if mem_info:scale() ~= 1 then
            sb = sb .. "*" .. tostring(mem_info:scale())
        end
    end
    if mem_info:displacement() ~= 0 or not need_plus then
        local displ = mem_info:displacement()
        if need_plus then
            if displ < 0 then
                displ = -displ
                sb = sb .. "-"
            else
                sb = sb .. "+"
            end
        end
        sb = sb .. string.format("0x%X", displ)
    end
    return sb
        .. ";"
        .. memory_size_to_string(mem_info:memory_size())
        .. ";"
        .. op_access_to_string(mem_info:access())
        .. "]"
end

local function how_to_get_instruction_info()
    local decoder = Decoder.new(example_code_bitness, example_code, nil, example_code_rip)

    for instr in decoder:iter_out() do
        -- Gets offsets in the instruction of the displacement and immediates and their sizes.
        -- This can be useful if there are relocations in the binary. The encoder has a similar
        -- method. This method must be called after decode() and you must pass in the last
        -- instruction decode() returned.
        local offsets = decoder:get_constant_offsets(instr)

        print(string.format("%016X %s", instr:ip(), tostring(instr)))

        local op_code = instr:op_code()
        local fpu_info = instr:fpu_stack_increment_info()
        print(string.format("    OpCode: %s", op_code:op_code_string()))
        print(string.format("    Instruction: %s", op_code:instruction_string()))
        print(string.format("    Encoding: %s", encoding_kind_to_string(instr:encoding())))
        print(string.format("    Mnemonic: %s", mnemonic_to_string(instr:mnemonic())))
        print(string.format("    Code: %s", code_to_string(instr:code())))
        print(string.format("    CpuidFeature: %s", cpuid_features_to_string(instr:cpuid_features())))
        print(string.format("    FlowControl: %s", flow_control_to_string(instr:flow_control())))
        if fpu_info:writes_top() then
            if fpu_info:increment() == 0 then
                print("    FPU TOP: the instruction overwrites TOP")
            else
                print(string.format("    FPU TOP inc: %s", fpu_info:increment()))
            end
            local cond_write = fpu_info:conditional() and "true" or "false"
            print(string.format("    FPU TOP cond write: %s", cond_write))
        end
        if offsets:has_displacement() then
            print(
                string.format(
                    "    Displacement offset = %d, size = %d",
                    offsets:displacement_offset(),
                    offsets:displacement_size()
                )
            )
        end
        if offsets:has_immediate() then
            print(
                string.format(
                    "    Immediate offset = %d, size = %d",
                    offsets:immediate_offset(),
                    offsets:immediate_size()
                )
            )
        end
        if offsets:has_immediate2() then
            print(
                string.format(
                    "    Immediate #2 offset = %d, size = %d",
                    offsets:immediate_offset2(),
                    offsets:immediate_size2()
                )
            )
        end
        if instr:is_stack_instruction() then
            print(string.format("    SP Increment: %s", instr:stack_pointer_increment()))
        end
        if instr:condition_code() ~= ConditionCode.None then
            print(string.format("    Condition code: %s", condition_code_to_string(instr:condition_code())))
        end
        if instr:rflags_read() ~= RflagsBits.None then
            print(string.format("    RFLAGS Read: %s", rflags_bits_to_string(instr:rflags_read())))
        end
        if instr:rflags_written() ~= RflagsBits.None then
            print(string.format("    RFLAGS Written: %s", rflags_bits_to_string(instr:rflags_written())))
        end
        if instr:rflags_cleared() ~= RflagsBits.None then
            print(string.format("    RFLAGS Cleared: %s", rflags_bits_to_string(instr:rflags_cleared())))
        end
        if instr:rflags_set() ~= RflagsBits.None then
            print(string.format("    RFLAGS Set: %s", rflags_bits_to_string(instr:rflags_set())))
        end
        if instr:rflags_undefined() ~= RflagsBits.None then
            print(string.format("    RFLAGS Undefined: %s", rflags_bits_to_string(instr:rflags_undefined())))
        end
        if instr:rflags_modified() ~= RflagsBits.None then
            print(string.format("    RFLAGS Modified: %s", rflags_bits_to_string(instr:rflags_modified())))
        end
        for i = 1, instr:op_count() do
            local op_kind = instr:op_kind(i - 1)
            if op_kind == OpKind.Memory then
                local size = MemorySizeExt.size(instr:memory_size())
                if size ~= 0 then
                    print(string.format("    Memory size: %s", size))
                end
                break
            end
        end
        local used_registers, used_memory, op_accesses = instr:used_values()
        for i = 1, instr:op_count() do
            print(string.format("    Op%dAccess: %s", i - 1, op_access_to_string(op_accesses[i])))
        end
        for i = 1, op_code:op_count() do
            print(string.format("    Op%d: %s", i - 1, op_code_operand_kind_to_string(op_code:op_kind(i - 1))))
        end
        for _, reg_info in ipairs(used_registers) do
            print(string.format("    Used reg: %s", used_reg_to_string(reg_info)))
        end
        for _, mem_info in ipairs(used_memory) do
            print(string.format("    Used mem: %s", used_mem_to_string(mem_info)))
        end
    end
end

how_to_get_instruction_info()
```

## Disassemble old/deprecated CPU instructions

```lua
local Decoder = require("iced_x86.Decoder")
local DecoderOptions = require("iced_x86.DecoderOptions")

-- This example produces the following output:
-- 731E0A03 bndmov bnd1,qword ptr [eax]
-- 731E0A07 mov tr3,esi
-- 731E0A0A rdshr [eax]
-- 731E0A0D dmint
-- 731E0A0F svdc [eax],cs
-- 731E0A12 cpu_read
-- 731E0A14 pmvzb mm1,[eax]
-- 731E0A17 frinear
-- 731E0A19 altinst

local test_code = ""
    .. "\102\015\026\008"
    .. "\015\038\222"
    .. "\015\054\000"
    .. "\015\057"
    .. "\015\120\008"
    .. "\015\061"
    .. "\015\088\008"
    .. "\223\252"
    .. "\015\063"

-- Enable decoding of Cyrix/Geode instructions, Centaur ALTINST,
-- MOV to/from TR and MPX instructions.
-- There are other options to enable other instructions such as UMOV, KNC, etc.
-- These are deprecated instructions or only used by old CPUs so they're not
-- enabled by default. Some newer instructions also use the same opcodes as
-- some of these old instructions.
local decoder_options = DecoderOptions.MPX
    + DecoderOptions.MovTr
    + DecoderOptions.Cyrix
    + DecoderOptions.Cyrix_DMI
    + DecoderOptions.ALTINST
local decoder = Decoder.new(32, test_code, decoder_options, 0x731E0A03)

for instr in decoder:iter_out() do
    print(string.format("%08X %s", instr:ip(), tostring(instr)))
end
```
