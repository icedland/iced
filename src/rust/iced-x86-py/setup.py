# SPDX-License-Identifier: MIT
# Copyright (C) 2018-present iced project and contributors

from setuptools import setup

try:
	from setuptools_rust import RustExtension, Strip
except:
	print()
	print("=============== MISSING BUILD TOOLS ===============")
	print("Missing setuptools-rust")
	print("Building iced-x86 requires `setuptools-rust` and Rust")
	print("setuptools-rust: pip3 install setuptools-rust")
	print("Rust: https://www.rust-lang.org/tools/install")
	print("=============== MISSING BUILD TOOLS ===============")
	print()
	raise

with open("README.md", "r", encoding="utf-8") as file:
	readme_md = file.read()

setup(
	name="iced-x86",
	version="1.21.0",
	license="MIT",
	author_email="wtfsck@protonmail.com",
	author="wtfsck",
	description="iced-x86 is a blazing fast and correct x86/x64 disassembler, assembler and instruction decoder",
	long_description=readme_md,
	long_description_content_type="text/markdown",
	url="https://github.com/icedland/iced/tree/master/src/rust/iced-x86-py",
	platforms=["any"],
	python_requires="~=3.8",
	classifiers=[
		"Development Status :: 5 - Production/Stable",
		"Intended Audience :: Developers",
		"Operating System :: MacOS :: MacOS X",
		"Operating System :: Microsoft :: Windows",
		"Operating System :: OS Independent",
		"Operating System :: POSIX",
		"Programming Language :: Python",
		"Programming Language :: Python :: 3",
		"Programming Language :: Python :: 3.8",
		"Programming Language :: Python :: 3.9",
		"Programming Language :: Python :: 3.10",
		"Programming Language :: Python :: 3.11",
		"Programming Language :: Python :: 3.12",
		"Programming Language :: Python :: 3.13",
		"Programming Language :: Python :: Implementation :: CPython",
		"Programming Language :: Python :: Implementation :: PyPy",
		"Programming Language :: Rust",
		"Topic :: Software Development :: Disassemblers",
		"Topic :: Software Development :: Libraries",
	],
	package_dir={"": "src"},
	packages=["iced_x86"],
	rust_extensions=[RustExtension("iced_x86._iced_x86_py", path="Cargo.toml", strip=Strip.All)],
	include_package_data=True,
	zip_safe=False,
)
