cmake_minimum_required(VERSION 3.25)
project(disassembler)

set(CMAKE_CXX_STANDARD 23)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

# Configure iced-x86 for decoder-only build
set(ICED_X86_DECODER ON)
set(ICED_X86_ENCODER OFF)
set(ICED_X86_BLOCK_ENCODER OFF)
set(ICED_X86_OP_CODE_INFO OFF)
set(ICED_X86_INSTR_INFO OFF)
set(ICED_X86_GAS OFF)
set(ICED_X86_INTEL OFF)
set(ICED_X86_MASM OFF)
set(ICED_X86_NASM OFF)
set(ICED_X86_FAST_FMT OFF)

# Include the iced-x86 source files directly
include_directories(src/cpp/iced-x86/include)

# Source files from iced-x86
set(ICED_SOURCES
    src/cpp/iced-x86/src/instruction.cpp
    src/cpp/iced-x86/src/instruction_create.cpp
    src/cpp/iced-x86/src/tables.cpp
    src/cpp/iced-x86/src/register_info.cpp
    src/cpp/iced-x86/src/memory_size_info.cpp
    src/cpp/iced-x86/src/mvex_info_data.cpp
    src/cpp/iced-x86/src/decoder.cpp
    src/cpp/iced-x86/src/handlers.cpp
    src/cpp/iced-x86/src/table_deserializer.cpp
)

# Create the disassembler executable with iced sources
add_executable(disassembler disassembler.cpp ${ICED_SOURCES})

# Set compile definitions for decoder-only
target_compile_definitions(disassembler PRIVATE
    ICED_X86_NO_ENCODER
    ICED_X86_NO_BLOCK_ENCODER
    ICED_X86_NO_OP_CODE_INFO
    ICED_X86_NO_INSTR_INFO
    ICED_X86_NO_GAS
    ICED_X86_NO_INTEL
    ICED_X86_NO_MASM
    ICED_X86_NO_NASM
    ICED_X86_NO_FAST_FMT
)