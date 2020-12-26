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

# Build the wheel with the minimum supported Python version only
if python --version 2>&1 | grep "Python 3\.6"; then
	python setup.py bdist_wheel --py-limited-api=cp36
	mkdir -p /tmp/py-dist
	cp dist/* /tmp/py-dist
fi

echo "Testing it"
python -m pip install iced-x86 --no-index -f /tmp/py-dist --only-binary :all:
python -m pytest --color=yes --code-highlight=yes
python -m pip uninstall -y iced-x86
