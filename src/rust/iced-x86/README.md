iced-x86
[![Latest version](https://img.shields.io/crates/v/iced-x86.svg)](https://crates.io/crates/iced-x86)
[![Documentation](https://docs.rs/iced-x86/badge.svg)](https://docs.rs/iced-x86)
[![Minimum rustc version](https://img.shields.io/badge/rustc-1.20.0+-yellow.svg)](https://github.com/0xd4d/iced/tree/master/src/rust/iced-x86#rust-version-requirements)
![License](https://img.shields.io/crates/l/iced-x86.svg)

TODO:

## Minimum supported `rustc` version

iced-x86 supports `rustc` 1.20.0 or later.
This is checked in CI builds where the minimum supported version and the latest stable version are used to build the source code and run tests.

If you use an older version of `rustc`, you may need to update the versions of some iced-x86 dependencies because `cargo` prefers to use the latest version which may not support your `rustc`.
Eg. iced-x86 needs `lazy_static` `1.1.0`, but `cargo` wants to use the latest version which is currently `1.4.0` and it doesn't support the minimum supported `rustc` version.
Here's how you can force a compatible version of any iced-x86 dependency without updating iced-x86's `Cargo.toml`:

```
cargo generate-lockfile
cargo update --package lazy_static --precise 1.1.0
```

## Breaking changes

The following are considered minor breaking changes. At least the minor version of iced-x86 will be incremented.

- Bumping the minimum supported version of `rustc`
- Adding a new enum item (eg. support new instructions)
- Sorting enum items
- Adding new APIs (can cause problems with `use iced_x86::*;`)

TODO:
