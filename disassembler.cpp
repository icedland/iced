#include <iostream>
#include <fstream>
#include <vector>
#include <iomanip>
#include <string>
#include "iced_x86/iced_x86.hpp"

using namespace iced_x86;

// Simple function to format a register
std::string format_register(Register reg) {
    switch (reg) {
        case Register::EAX: return "eax";
        case Register::EBX: return "ebx";
        case Register::ECX: return "ecx";
        case Register::EDX: return "edx";
        case Register::ESI: return "esi";
        case Register::EDI: return "edi";
        case Register::EBP: return "ebp";
        case Register::ESP: return "esp";
        case Register::RAX: return "rax";
        case Register::RBX: return "rbx";
        case Register::RCX: return "rcx";
        case Register::RDX: return "rdx";
        case Register::RSI: return "rsi";
        case Register::RDI: return "rdi";
        case Register::RBP: return "rbp";
        case Register::RSP: return "rsp";
        case Register::R8: return "r8";
        case Register::R9: return "r9";
        case Register::R10: return "r10";
        case Register::R11: return "r11";
        case Register::R12: return "r12";
        case Register::R13: return "r13";
        case Register::R14: return "r14";
        case Register::R15: return "r15";
        default: return "reg" + std::to_string(static_cast<int>(reg));
    }
}

// Simple function to get mnemonic string
std::string get_mnemonic_string(Mnemonic mnemonic) {
    switch (mnemonic) {
        case Mnemonic::NOP: return "nop";
        case Mnemonic::MOV: return "mov";
        case Mnemonic::ADD: return "add";
        case Mnemonic::SUB: return "sub";
        case Mnemonic::PUSH: return "push";
        case Mnemonic::POP: return "pop";
        case Mnemonic::CALL: return "call";
        case Mnemonic::RET: return "ret";
        case Mnemonic::JMP: return "jmp";
        case Mnemonic::JE: return "je";
        case Mnemonic::JNE: return "jne";
        case Mnemonic::CMP: return "cmp";
        case Mnemonic::TEST: return "test";
        case Mnemonic::LEA: return "lea";
        case Mnemonic::XOR: return "xor";
        case Mnemonic::AND: return "and";
        case Mnemonic::OR: return "or";
        case Mnemonic::SHL: return "shl";
        case Mnemonic::SHR: return "shr";
        case Mnemonic::SAR: return "sar";
        default: return "mnemonic" + std::to_string(static_cast<int>(mnemonic));
    }
}

// Simple function to format an operand
std::string format_operand(const Instruction& instr, uint32_t op_index) {
    OpKind kind = instr.op_kind(op_index);
    switch (kind) {
        case OpKind::REGISTER:
            return format_register(instr.op_register(op_index));
        case OpKind::IMMEDIATE8:
            return "0x" + std::to_string(static_cast<int>(instr.immediate8()));
        case OpKind::IMMEDIATE16:
            return "0x" + std::to_string(instr.immediate16());
        case OpKind::IMMEDIATE32:
            return "0x" + std::to_string(instr.immediate32());
        case OpKind::IMMEDIATE64:
            return "0x" + std::to_string(instr.immediate64());
        case OpKind::MEMORY: {
            std::string result = "[";
            if (instr.memory_base() != Register::NONE) {
                result += format_register(instr.memory_base());
            }
            if (instr.memory_index() != Register::NONE) {
                if (!result.empty() && result.back() != '[') result += "+";
                result += format_register(instr.memory_index());
                if (instr.memory_index_scale() > 1) {
                    result += "*" + std::to_string(instr.memory_index_scale());
                }
            }
            if (instr.memory_displacement64() != 0) {
                if (!result.empty() && result.back() != '[') result += "+";
                result += "0x" + std::to_string(instr.memory_displacement64());
            }
            result += "]";
            return result;
        }
        default:
            return "op" + std::to_string(op_index);
    }
}

// Simple disassembler function
void disassemble_file(const std::string& filename, uint32_t bitness = 64) {
    // Read the file
    std::ifstream file(filename, std::ios::binary);
    if (!file) {
        std::cerr << "Error: Cannot open file " << filename << std::endl;
        return;
    }

    std::vector<uint8_t> data((std::istreambuf_iterator<char>(file)),
                              std::istreambuf_iterator<char>());

    if (data.empty()) {
        std::cerr << "Error: File is empty or could not be read" << std::endl;
        return;
    }

    // Create decoder
    Decoder decoder(bitness, data, 0x1000); // Start at address 0x1000

    std::cout << "Disassembling " << filename << " (" << bitness << "-bit mode):" << std::endl;
    std::cout << std::endl;

    // Decode and print each instruction
    while (decoder.can_decode()) {
        auto result = decoder.decode();
        if (!result) {
            std::cerr << "Error: Failed to decode instruction at offset " << decoder.position() << std::endl;
            break;
        }

        const Instruction& instr = *result;

        // Print address
        std::cout << std::hex << std::setfill('0') << std::setw(8) << instr.ip() << ": ";

        // Print bytes
        size_t start_pos = decoder.position() - instr.length();
        for (uint32_t i = 0; i < instr.length(); ++i) {
            if (i > 0) std::cout << " ";
            std::cout << std::hex << std::setfill('0') << std::setw(2)
                      << static_cast<int>(data[start_pos + i]);
        }

        // Pad to align disassembly
        int padding = 20 - (instr.length() * 3);
        if (padding > 0) {
            std::cout << std::string(padding, ' ');
        }

        // Print mnemonic
        std::cout << get_mnemonic_string(instr.mnemonic());

        // Print operands
        uint32_t op_count = instr.op_count();
        for (uint32_t i = 0; i < op_count; ++i) {
            if (i == 0) std::cout << " ";
            else std::cout << ", ";
            std::cout << format_operand(instr, i);
        }

        std::cout << std::endl;
    }

    std::cout << std::endl << "Disassembly complete." << std::endl;
}

int main(int argc, char* argv[]) {
    if (argc < 2) {
        std::cerr << "Usage: " << argv[0] << " <filename> [bitness]" << std::endl;
        std::cerr << "  bitness: 16, 32, or 64 (default: 64)" << std::endl;
        return 1;
    }

    std::string filename = argv[1];
    uint32_t bitness = 64;

    if (argc >= 3) {
        bitness = std::stoi(argv[2]);
        if (bitness != 16 && bitness != 32 && bitness != 64) {
            std::cerr << "Error: Bitness must be 16, 32, or 64" << std::endl;
            return 1;
        }
    }

    disassemble_file(filename, bitness);
    return 0;
}