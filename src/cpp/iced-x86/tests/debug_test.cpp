// Quick debug test
#include <cstdio>
#include "iced_x86/iced_x86.hpp"

using namespace iced_x86;

int main() {
    // Test MOVAPS XMM0, XMM1: 0F 28 C1
    const uint8_t bytes[] = {0x0F, 0x28, 0xC1};
    
    Decoder decoder(64, std::span<const uint8_t>(bytes, sizeof(bytes)), 0x1000);
    auto result = decoder.decode();
    
    if (!result.has_value()) {
        printf("Decode failed!\n");
        return 1;
    }
    
    auto instr = *result;
    printf("Decoded: code=%u, length=%u\n", static_cast<unsigned>(instr.code()), instr.length());
    printf("  op_count=%u\n", instr.op_count());
    printf("  op0_kind=%u, op0_register=%u\n", static_cast<unsigned>(instr.op0_kind()), static_cast<unsigned>(instr.op0_register()));
    printf("  op1_kind=%u, op1_register=%u\n", static_cast<unsigned>(instr.op1_kind()), static_cast<unsigned>(instr.op1_register()));
    
    Encoder encoder(64);
    auto encode_result = encoder.encode(instr, 0x1000);
    
    if (!encode_result.has_value()) {
        printf("Encode failed!\n");
        return 1;
    }
    
    printf("Encode succeeded, length=%zu\n", *encode_result);
    
    auto buffer = encoder.take_buffer();
    printf("Encoded bytes: ");
    for (auto b : buffer) {
        printf("%02X ", b);
    }
    printf("\n");
    
    printf("Original bytes: ");
    for (size_t i = 0; i < sizeof(bytes); i++) {
        printf("%02X ", bytes[i]);
    }
    printf("\n");
    
    return 0;
}
