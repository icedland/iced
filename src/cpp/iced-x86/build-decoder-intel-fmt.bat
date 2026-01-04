@echo off
REM Build iced-x86 with decoder + Intel formatter only

cmake -B build-decoder-intel-fmt -S . ^
  -DICED_X86_ENCODER=OFF ^
  -DICED_X86_BLOCK_ENCODER=OFF ^
  -DICED_X86_GAS=OFF ^
  -DICED_X86_MASM=OFF ^
  -DICED_X86_NASM=OFF ^
  -DICED_X86_FAST_FMT=OFF ^
  -DICED_X86_BUILD_TESTS=OFF

cmake --build build-decoder-intel-fmt --config Release
