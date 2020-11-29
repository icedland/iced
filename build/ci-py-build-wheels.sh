#!/bin/bash
set -e

if [ ! "$GITHUB_ACTIONS" ]; then
	echo "This file should only be executed from GitHub Actions"
	exit 1
fi

python --version
pip --version

pip install -U setuptools wheel setuptools-rust

# Needed so the wheel files don't get extra *.{so,pyd} files (should have exactly one)
# from earlier builds
git clean -xdf
./build/build-python --wheel-only
mkdir -p /tmp/py-dist
cp src/rust/iced-x86-py/dist/* /tmp/py-dist
