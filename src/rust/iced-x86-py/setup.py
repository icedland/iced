# SPDX-License-Identifier: MIT
# Copyright wtfsckgh@gmail.com
# Copyright iced contributors

import sys
from setuptools import setup
from setuptools_rust import RustExtension, Strip

with open("README.md", "r", encoding="utf-8") as file:
	readme_md = file.read()

py_limited_api = any(arg == "--py-limited-api" or arg.startswith("--py-limited-api=") for arg in sys.argv)

setup(
	name="iced-x86",
	version="1.10.2",
	license="MIT",
	author_email="wtfsckgh@gmail.com",
	author="wtfsck",
	description="iced-x86 is a high performance and correct x86/x64 disassembler, assembler and instruction decoder",
	long_description=readme_md,
	long_description_content_type="text/markdown",
	url="https://github.com/icedland/iced/tree/master/src/rust/iced-x86-py",
	platforms=["any"],
	python_requires="~=3.6",
	classifiers=[
		"Development Status :: 5 - Production/Stable",
		"Intended Audience :: Developers",
		"License :: OSI Approved :: MIT License",
		"Operating System :: MacOS :: MacOS X",
		"Operating System :: Microsoft :: Windows",
		"Operating System :: OS Independent",
		"Operating System :: POSIX",
		"Programming Language :: Python",
		"Programming Language :: Python :: 3",
		"Programming Language :: Python :: 3.6",
		"Programming Language :: Python :: 3.7",
		"Programming Language :: Python :: 3.8",
		"Programming Language :: Python :: 3.9",
		"Programming Language :: Python :: Implementation :: CPython",
		"Programming Language :: Python :: Implementation :: PyPy",
		"Programming Language :: Rust",
		"Topic :: Software Development :: Disassemblers",
		"Topic :: Software Development :: Libraries",
	],
	package_dir={"": "src"},
	packages=["iced_x86"],
	rust_extensions=[RustExtension("iced_x86._iced_x86_py", path="Cargo.toml", strip=Strip.All, py_limited_api=py_limited_api)],
	include_package_data=True,
	zip_safe=False,
)
