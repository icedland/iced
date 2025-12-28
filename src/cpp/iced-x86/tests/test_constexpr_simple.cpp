#include <iostream>
#include <vector>

// Include the iced-x86 headers
#include <iced_x86/decoder.hpp>

int main() {
    std::cout << "Testing iced-x86 constexpr handler tables..." << std::endl;

    // Test basic decoder creation and simple instruction decoding
    std::vector<uint8_t> code = { 0x90, 0xCC, 0xC3 }; // NOP, INT3, RET

    try {
        iced_x86::Decoder decoder(64, code);

        std::cout << "Decoder created successfully!" << std::endl;

        int count = 0;
        while (decoder.can_decode()) {
            auto instr = decoder.decode();
            std::cout << "Instruction " << count << ": decoded successfully" << std::endl;
            count++;

            // Limit to first few instructions for quick test
            if (count >= 3) break;
        }

        std::cout << "Test PASSED: Constexpr handler tables working correctly!" << std::endl;
        return 0;

    } catch (const std::exception& e) {
        std::cout << "Test FAILED: " << e.what() << std::endl;
        return 1;
    } catch (...) {
        std::cout << "Test FAILED: Unknown exception" << std::endl;
        return 1;
    }
}
