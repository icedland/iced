@echo off
REM Build iced-x86 with decoder only (minimal build)

cmake -B build-decoder-only -S . ^
  -DICED_X86_ENCODER=OFF ^
  -DICED_X86_BLOCK_ENCODER=OFF ^
  -DICED_X86_OP_CODE_INFO=OFF ^
  -DICED_X86_INSTR_INFO=OFF ^
  -DICED_X86_GAS=OFF ^
  -DICED_X86_INTEL=OFF ^
  -DICED_X86_MASM=OFF ^
  -DICED_X86_NASM=OFF ^
  -DICED_X86_FAST_FMT=OFF ^
  -DICED_X86_BUILD_TESTS=OFF

cmake --build build-decoder-only --config Release
