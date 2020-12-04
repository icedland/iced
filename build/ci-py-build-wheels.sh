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
./build/build-python --wheel-only
mkdir -p /tmp/py-dist
cd src/rust/iced-x86-py
cp dist/* /tmp/py-dist

echo "Testing it"
python -m pip install iced-x86 --no-index -f dist
python -m pytest --color=yes --code-highlight=yes
python -m pip uninstall -y iced-x86
