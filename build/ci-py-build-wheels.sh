#!/bin/bash
set -e

if [ ! "$GITHUB_ACTIONS" ]; then
	echo "This file should only be executed from GitHub Actions"
	exit 1
fi

python --version
python -m pip --version

python -m pip install -U setuptools wheel setuptools-rust pytest

# Needed so the wheel files don't get extra *.{so,pyd} files (should have exactly one)
# from earlier builds
git clean -xdf

cd src/rust/iced-x86-py
cargo_toml=Cargo.toml

patchci_verify_not_patched() {
	# msys grep fails if we use $
	if ! grep -E '^#pathci' "$cargo_toml" 2>&1 > /dev/null; then
		echo "Cargo.toml is patched"
		exit 1
	fi
}

patchci_verify_patched() {
	# msys grep fails if we use $
	if grep -E '^#pathci' "$cargo_toml" 2>&1 > /dev/null; then
		echo "Cargo.toml is not patched"
		exit 1
	fi
}

# See build-python
patchci_patch() {
	patchci_verify_not_patched

	curr_dir=$(pwd)
	cd "$root_dir"

	if [ "$OSTYPE" = "msys" ]; then
		iced_x86_dir="$(pwd -W)/../iced-x86"
	else
		iced_x86_dir="$(pwd)/../iced-x86"
	fi
	if [ ! -d "$iced_x86_dir" ]; then
		echo "Dir does not exist: $iced_x86_dir"
		exit 1
	fi

	sed -i -e "s&^#pathci$&path = \"$iced_x86_dir\"&" "$cargo_toml"

	cd "$curr_dir"
}

patchci_undo_patch() {
	git checkout "$cargo_toml"
}

# Build the wheel with the minimum supported Python version only
if python --version 2>&1 | grep 'Python 3\.6'; then
	patchci_verify_not_patched
	patchci_patch
	patchci_verify_patched

	python setup.py bdist_wheel --py-limited-api=cp36
	mkdir -p /tmp/py-dist
	cp dist/* /tmp/py-dist

	patchci_undo_patch
fi

echo "Testing it"
python -m pip install iced-x86 --no-index -f /tmp/py-dist --only-binary iced-x86
python -m pytest --color=yes --code-highlight=yes
python -m pip uninstall -y iced-x86
