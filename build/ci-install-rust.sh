#!/bin/bash
set -e

if [ ! "$GITHUB_ACTIONS" ]; then
	echo "This file should only be executed from GitHub Actions"
	exit 1
fi

echo "$HOME/.cargo/bin" >> "$GITHUB_PATH"

if [[ "$OSTYPE" = "darwin"* ]]; then
	curl --proto '=https' --tlsv1.2 -sSf https://sh.rustup.rs | sh -s -- -y
	rustup install stable
	rustup component add rustfmt
	rustup component add clippy
fi

# It fails on Windows so disable auto self update
rustup toolchain install 1.63.0 --no-self-update
rustup target add wasm32-unknown-unknown
rustup update --no-self-update

rustc --version
cargo --version
