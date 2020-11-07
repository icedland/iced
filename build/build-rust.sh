#!/bin/sh
set -e

root_dir=$(dirname "$0")
root_dir=$(cd "$root_dir/.." && pwd)
if [ ! -f "$root_dir/LICENSE.txt" ]; then
	echo "Couldn't find the root dir"
	exit 1
fi

configuration=Release
no_gen_check=n
no_dotnet=n
no_msrv=n

# Minimum supported Rust version
msrv="1.20.0"

function new_func {
	echo
	echo "****************************************************************"
	echo "$1"
	echo "****************************************************************"
	echo
}

function clean_dotnet_build_output {
	dotnet clean -v:m -c $configuration "$root_dir/src/csharp/Iced.sln"
}

function generator_check {
	new_func "Run generator, verify no diff"

	dotnet run -c $configuration -p "$root_dir/src/csharp/Intel/Generator/Generator.csproj"
	git diff --exit-code
}

function test_valid_invalid_instructions {
	new_func "Decode valid and invalid instructions"

	valid_file=$(mktemp)
	invalid_file=$(mktemp)

	# Needs to be rebuilt if the wrong #defines were used
	clean_dotnet_build_output

	for bitness in 16 32 64; do
		echo "==== ${bitness}-bit: Generating valid/invalid files ===="
		dotnet run -c:$configuration -p "$root_dir/src/csharp/Intel/IcedFuzzer/IcedFuzzer/IcedFuzzer.csproj" -- -$bitness -oil "$invalid_file" -ovlc "$valid_file"
		echo "==== ${bitness}-bit: Testing valid instructions ===="
		cargo run --color always --release --manifest-path "$root_dir/src/rust/Cargo.toml" -p iced-x86-fzgt -- -b $bitness -f "$valid_file"
		echo "==== ${bitness}-bit: Testing invalid instructions ===="
		cargo run --color always --release --manifest-path "$root_dir/src/rust/Cargo.toml" -p iced-x86-fzgt -- -b $bitness -f "$invalid_file" --invalid
	done

	for bitness in 16 32 64; do
		echo "==== ${bitness}-bit (AMD): Generating valid/invalid files ===="
		dotnet run -c:$configuration -p "$root_dir/src/csharp/Intel/IcedFuzzer/IcedFuzzer/IcedFuzzer.csproj" -- -$bitness -oil "$invalid_file" -ovlc "$valid_file" --amd
		echo "==== ${bitness}-bit (AMD): Testing valid instructions ===="
		cargo run --color always --release --manifest-path "$root_dir/src/rust/Cargo.toml" -p iced-x86-fzgt -- -b $bitness -f "$valid_file" --amd
		echo "==== ${bitness}-bit (AMD): Testing invalid instructions ===="
		cargo run --color always --release --manifest-path "$root_dir/src/rust/Cargo.toml" -p iced-x86-fzgt -- -b $bitness -f "$invalid_file" --invalid --amd
	done

	rm "$valid_file"
	rm "$invalid_file"
}

function build_no_std {
	new_func "Build no_std"
	pushd "$root_dir/src/rust/iced-x86"

	echo "==== BUILD DEBUG ===="
	cargo check --color always --no-default-features --features "no_std decoder encoder block_encoder op_code_info instr_info gas intel masm nasm fast_fmt db"

	popd
}

function build_features {
	new_func "Build one feature at a time"
	pushd "$root_dir/src/rust/iced-x86"

	allFeatures=(
		"std decoder"
		"std encoder"
		"std encoder block_encoder"
		"std encoder op_code_info"
		"std instr_info"
		"std gas"
		"std intel"
		"std masm"
		"std nasm"
		"std fast_fmt"
	)
	for features in "${allFeatures[@]}"; do
		echo "==== $features ===="
		cargo check --color always --no-default-features --features "$features"
	done

	allFeatures=(
		"no_vex"
		"no_evex"
		"no_xop"
		"no_d3now"
		"no_vex no_evex no_xop no_d3now"
	)
	for features in "${allFeatures[@]}"; do
		echo "==== $features ===="
		cargo check --color always --features "$features"
	done

	allFeatures=(
		"no_std decoder"
		"no_std encoder"
		"no_std encoder block_encoder"
		"no_std encoder op_code_info"
		"no_std instr_info"
		"no_std gas"
		"no_std intel"
		"no_std masm"
		"no_std nasm"
		"no_std fast_fmt"
	)
	for features in "${allFeatures[@]}"; do
		echo "==== $features ===="
		cargo check --color always --no-default-features --features "$features"
	done

	allFeatures=(
		"std decoder"
		"std decoder encoder"
		"std decoder encoder block_encoder"
		"std decoder encoder op_code_info"
		"std decoder instr_info"
		"std decoder gas"
		"std decoder intel"
		"std decoder masm"
		"std decoder nasm"
		"std decoder fast_fmt"
	)
	for features in "${allFeatures[@]}"; do
		echo "==== TEST $features ===="
		cargo check --color always --tests --no-default-features --features "$features"
	done

	allFeatures=(
		"no_vex"
		"no_evex"
		"no_xop"
		"no_d3now"
		"no_vex no_evex no_xop no_d3now"
	)
	for features in "${allFeatures[@]}"; do
		echo "==== TEST $features ===="
		cargo check --color always --tests --features "$features"
	done

	popd
}

function build_test_default {
	new_func "Build, test (default)"
	pushd "$root_dir/src/rust/iced-x86"

	echo "Rust version"
	cargo -V

	echo "==== CLIPPY ===="
	cargo clippy --color always

	echo "==== CLIPPY --tests ===="
	cargo clippy --color always --tests

	echo "==== FORMAT CHECK ===="
	cargo fmt -- --color always --check

	echo "==== DOC ===="
	cargo doc --color always

	echo "==== BUILD RELEASE ===="
	cargo build --color always --features "db" --release

	echo "==== TEST RELEASE ===="
	cargo test --color always --features "db" --release

	echo "==== TEST DEBUG ===="
	cargo test --color always --features "db"

	echo "==== BUILD RELEASE wasm32-unknown-unknown ===="
	cargo check --color always --features "db" --target wasm32-unknown-unknown --release

	echo "==== PUBLISH DRY-RUN ===="
	# It fails on Windows (GitHub CI) without this, claiming that some random number of Rust files are dirty.
	git status
	git diff
	cargo publish --color always --dry-run

	popd
}

function build_test_msrv {
	new_func "Build minimum supported Rust version: $msrv"

	pushd "$root_dir/src/rust/iced-x86"

	# Some of these commands can be removed/updated when msrv is changed
	expected_msrv="1.20.0"
	if [ "$msrv" != "$expected_msrv" ]; then
		echo "MSRV != $expected_msrv, update this code now"
		exit 1
	fi

	echo "*** If this fails, install Rust $msrv or use --no-msrv"

	sed -i -e 's/"iced-x86-fzgt",/#"iced-x86-fzgt",/' ../Cargo.toml

	echo "==== UPDATE Cargo.lock ===="
	cargo +$msrv generate-lockfile
	cargo +$msrv update --package lazy_static --precise 1.1.1

	echo "==== BUILD RELEASE ===="
	cargo +$msrv build --color always --features "db" --release

	echo "==== TEST RELEASE ===="
	cargo +$msrv test --color always --features "db" --release -- --skip "lib.rs"

	git checkout ../Cargo.toml
	# Restore it
	cargo generate-lockfile

	popd
}

while [ "$#" -gt 0 ]; do
	case $1 in
	--no-gen-check) no_gen_check=y ;;
	--no-dotnet) no_dotnet=y ;;
	--no-msrv) no_msrv=y ;;
	*) echo "Unknown arg: $1"; exit 1 ;;
	esac
	shift
done

if [ "$no_dotnet" != "y" ]; then
	echo "dotnet version (if this fails, install .NET or use --no-dotnet)"
	dotnet --version

	if [ "$no_gen_check" != "y" ]; then
		generator_check
	fi
	test_valid_invalid_instructions
fi
build_no_std
build_features
build_test_default
if [ "$no_msrv" != "y" ]; then
	build_test_msrv
fi
