# Iced-x86 C++ Feature-based Compilation

This document describes how to selectively compile different parts of the iced-x86 C++ library.

## Available Features

### Core Features
- **ICED_X86_DECODER** (default: ON) - Enable decoder functionality
- **ICED_X86_ENCODER** (default: ON) - Enable encoder functionality  
- **ICED_X86_BLOCK_ENCODER** (default: ON) - Enable block encoder functionality

### Information Features
- **ICED_X86_OP_CODE_INFO** (default: ON) - Enable opcode information
- **ICED_X86_INSTR_INFO** (default: ON) - Enable instruction information

### Formatter Features
- **ICED_X86_GAS** (default: ON) - Enable GAS formatter
- **ICED_X86_INTEL** (default: ON) - Enable Intel formatter
- **ICED_X86_MASM** (default: ON) - Enable MASM formatter
- **ICED_X86_NASM** (default: ON) - Enable NASM formatter
- **ICED_X86_FAST_FMT** (default: ON) - Enable fast formatter

### Exclusion Features (matching Rust's no_* features)
- **ICED_X86_NO_VEX** (default: OFF) - Disable VEX instruction support
- **ICED_X86_NO_EVEX** (default: OFF) - Disable EVEX instruction support  
- **ICED_X86_NO_XOP** (default: OFF) - Disable XOP instruction support
- **ICED_X86_NO_D3NOW** (default: OFF) - Disable 3DNow instruction support



## Usage Examples

### Minimal Decoder-Only Build
```bash
cmake -B build -S . \
  -DICED_X86_ENCODER=OFF \
  -DICED_X86_BLOCK_ENCODER=OFF \
  -DICED_X86_OP_CODE_INFO=OFF \
  -DICED_X86_INSTR_INFO=OFF \
  -DICED_X86_GAS=OFF \
  -DICED_X86_INTEL=OFF \
  -DICED_X86_MASM=OFF \
  -DICED_X86_NASM=OFF \
  -DICED_X86_FAST_FMT=OFF
```

### Decoder + Intel Formatter Only
```bash
cmake -B build -S . \
  -DICED_X86_ENCODER=OFF \
  -DICED_X86_BLOCK_ENCODER=OFF \
  -DICED_X86_GAS=OFF \
  -DICED_X86_MASM=OFF \
  -DICED_X86_NASM=OFF \
  -DICED_X86_FAST_FMT=OFF
```

### Exclude AVX Instructions
```bash
cmake -B build -S . \
  -DICED_X86_NO_EVEX=ON
```

### Test Configuration (matches Rust default features)
```bash
# This includes all default features:
cmake -B build -S .
```

## Feature Dependencies

- `ICED_X86_BLOCK_ENCODER` requires `ICED_X86_ENCODER` to be enabled
- `ICED_X86_ENCODER` requires `ICED_X86_DECODER` to be enabled (encoder uses decoder tables)
- The build will fail with a clear error message if these dependencies are violated

## Build Size Reduction

When you disable features, both:
1. **Source files are excluded** from compilation (faster build times)
2. **Header content is conditionally excluded** via `#ifndef` guards

This means disabling a feature actually removes the code from the library, not just hides it.

## Preprocessor Definitions

When features are disabled, the following preprocessor definitions are added:
- `ICED_X86_NO_DECODER` (when decoder is disabled)
- `ICED_X86_NO_ENCODER` (when encoder is disabled)
- `ICED_X86_NO_BLOCK_ENCODER` (when block encoder is disabled)
- `ICED_X86_NO_OP_CODE_INFO` (when opcode info is disabled)
- `ICED_X86_NO_INSTR_INFO` (when instruction info is disabled)
- `ICED_X86_NO_GAS` (when GAS formatter is disabled)
- `ICED_X86_NO_INTEL` (when Intel formatter is disabled)
- `ICED_X86_NO_MASM` (when MASM formatter is disabled)
- `ICED_X86_NO_NASM` (when NASM formatter is disabled)
- `ICED_X86_NO_FAST_FMT` (when fast formatter is disabled)
- `ICED_X86_NO_VEX_INSTRUCTIONS` (when VEX is disabled)
- `ICED_X86_NO_EVEX_INSTRUCTIONS` (when EVEX is disabled)
- `ICED_X86_NO_XOP_INSTRUCTIONS` (when XOP is disabled)
- `ICED_X86_NO_D3NOW_INSTRUCTIONS` (when 3DNow is disabled)
These definitions can be used in your code with `#ifdef` guards:

```cpp
#ifndef ICED_X86_NO_ENCODER
// Encoder-specific code
#endif

#ifdef ICED_X86_NO_VEX_INSTRUCTIONS
// Code that runs when VEX is disabled
#endif
```

## Note

This feature system matches the Rust Cargo.toml features as closely as possible, allowing similar flexibility in binary size and compilation time.