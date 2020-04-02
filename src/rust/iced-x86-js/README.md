iced-x86 JavaScript bindings (Rust -> WebAssembly) ![License](https://img.shields.io/crates/l/iced-x86.svg)

x86/x64 disassembler for JavaScript (WebAssembly).

- ✔️Supports all Intel and AMD instructions
- ✔️The formatter supports masm, nasm, gas (AT&T), Intel (XED) and there are many options to customize the output
- ✔️The encoder can be used to re-encode decoded instructions at any address
- ✔️API to get instruction info, eg. read/written registers, memory and rflags bits; CPUID feature flag, flow control info, etc
- ✔️Rust + WebAssembly + JavaScript
- ✔️License: MIT

Rust crate: https://github.com/0xd4d/iced/blob/master/src/rust/iced-x86/README.md

# Required tools

- `Rust`: https://www.rust-lang.org/tools/install
- `wasm-pack`: https://rustwasm.github.io/wasm-pack/installer/

# Build

You can override which features to build to reduce the size of the wasm/ts/js files, see [Feature flags](#feature-flags).

This example assumes you need features `decoder` and `masm` and use a JavaScript bundler eg. webpack:

```sh
cd src/rust/iced-x86-js
wasm-pack build --target bundler -- --no-default-features --features "decoder masm"
```

`--target` docs [are here](https://rustwasm.github.io/docs/wasm-bindgen/reference/deployment.html).

The result is stored in the `pkg/` sub dir.

The js files aren't minified.

# Feature flags

Here's a list of all features you can enable when building the wasm file

- `instruction_api`: Enables `Instruction` methods and properties to get eg. mnemonic, operands, etc.
- `decoder`: (✔️Enabled by default) Enables the decoder. Required to disassemble code.
- `encoder`: Enables the encoder
- `block_encoder`: Enables the `BlockEncoder`. Requires `encoder`
- `op_code_info`: Get instruction metadata, see the `Instruction.opCode` property. Requires `encoder`
- `instr_info`: Enables instruction info code (read/written regs/mem, flags, control flow info etc)
- `gas`: Enables the GNU Assembler (AT&T) formatter
- `intel`: Enables the Intel (XED) formatter
- `masm`: (✔️Enabled by default) Enables the masm formatter
- `nasm`: Enables the nasm formatter

By default, `"decoder masm"` is enabled which is all you need to disassemble code.

`"decoder masm instruction_api instr_info"` if you want to analyze the code and disassemble it. Add `encoder` and optionally `block_encoder` if you want to re-encode the decoded instructions.

# Optimize wasm for speed

Edit `Cargo.toml` and change `opt-level = "z"` to [`opt-level = 3`](https://doc.rust-lang.org/nightly/cargo/reference/profiles.html#opt-level) (no double quotes).

```toml
[profile.release]
codegen-units = 1
lto = true
opt-level = 3
```

# Examples

TODO:
