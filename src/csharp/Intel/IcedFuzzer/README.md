# Generate valid and invalid instructions

With these generated instructions it's possible to verify that

- All invalid opcodes and invalid encodings of valid opcodes fail to be decoded
- All the rest are decoded successfully and the length of the decoded instruction is correct

It does not verify that the decoder decoded the correct instruction or the correct operands. To test that, disassemble all valid instructions. Use a high base address (RIP) value.

## How it works

It uses iced's instruction definitions that weren't filtered out by the user. For each of these defs, an instruction is created. If it has a modrm reg/mem op, it's split up into two: one with a reg op and the other with a mem op. These properties are unique to every instruction:

- encoding (legacy, 3DNow!, VEX, XOP, EVEX)
- table (eg. 0F, 0F38, etc)
- mandatory prefix (NP 66 F3 F2). In the case of legacy/3DNow! instructions it can also be None.
- opcode
- modrm mem op or not
- legacy: operand size and address size
- W, L, L'
- Group info:
  - none
  - `reg` group index
  - `rm` group index
  - modrm opcode (no operands and modrm >= C0h)

Unless you use the `--no-unused-tables` option, it will use all tables which includes EVEX table indexes 0-7, VEX table indexes 0-1Fh, XOP table indexes 0-1Fh (0-7: XOP.B=1).

For every unused slot that can be a reserved-nop, create a reserved-nop instruction. The remaining instructions are undefined instructions and an invalid instruction is created. Once this is finished, every slot is either a valid or an invalid instruction.

It generates the following instructions (see `FuzzerGen.cs`):

## Existing instructions

- Prefixes
  - Generates all prefixes, one at a time, before the mandatory prefix (if any)
  - Generates all prefixes, one at a time, after the mandatory prefix (if any)
  - Multi prefixes before the mandatory prefix:
    - 4F + any other prefix
    - same prefix twice
    - two prefixes from the same group, all combos, except same prefixes (already gen'd)
    - 67 + seg + seg (some combos)
    - 67 + F3/F2
    - xacquire/xrelease + lock
    - notrack + bnd
- For each instr, encode it with too few bytes (1..instr_len-1 total bytes)
- For each instruction, add enough valid/ignored prefixes to make the total length > 15 bytes
- For each (op1,op2) that use the same register class, gen op1==op2. If one of them
  is a modrm mem op, then the mem op must be a vsib op and the reg op must be a vec reg.
  It doesn't gen the same gpr in a reg op and a mem op, eg. `mov eax,[eax]`, but it
  does gen it if they're both reg ops, eg. `mov eax,eax`.
- Gens instructions with ignored bits set to 1 (W R X B R' L L')
- For each reg operand, gen each valid and invalid register. The other operands are set to valid operand values.
- If it's mov to/from CR/DR/TR, generate all mod=00..11 values
- If AMD and mov to/from CR/DR/TR, generate a LOCK prefix as an extra reg bit.
- For each modrm mem op, gen various kinds of mem ops with and without sib bytes and with and without 67 prefixes.
- For each mem offs op, gen the operand with and without a 67 prefix.
- For each instruction with an implied op, gen with and without a 67 prefix
- Every instruction with an immediate is generated with imm values. If it is a special pseudo op instruction, we generate all 256 imm values.
- Gens all instrs with no ops.
- Gens all combinations of EVEX.bcst/z/aaa bits, and if {er} is supported, all L'L bits
- Gens all possible values of V' vvvv bits, but only if it's an instruction that doesn't use them.
  If the instruction already uses the V' vvvv bits, the reg fuzzer gen has already generated all values.
- EVEX: Sets the reserved bits p0[3] to 1 and p1[2] to 0

If it's VEX, all the above are gen'd with a VEX2 prefix (if possible) and a VEX3 prefix.

## Unused instructions

All unused instructions' lengths are padded to 15 bytes.

- Gen an instruction with a modrm byte and reg and mem ops. If mem op, use [base+index] sib byte. Uses small reg values.
- For each invalid instruction with reg-only modrm, gen all possible values of reg, rm, or reg/rm bits.
- For each invalid instruction with mem-only modrm, gen all possible values of reg bits.
