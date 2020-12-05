#
# Copyright (C) 2018-2019 de4dot@gmail.com
#
# Permission is hereby granted, free of charge, to any person obtaining
# a copy of this software and associated documentation files (the
# "Software"), to deal in the Software without restriction, including
# without limitation the rights to use, copy, modify, merge, publish,
# distribute, sublicense, and/or sell copies of the Software, and to
# permit persons to whom the Software is furnished to do so, subject to
# the following conditions:
#
# The above copyright notice and this permission notice shall be
# included in all copies or substantial portions of the Software.
#
# THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
# EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
# MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
# IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
# CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
# TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
# SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#

from setuptools import setup
from setuptools_rust import RustExtension, Strip

with open("README.md", "r", encoding="utf-8") as file:
	readme_md = file.read()

setup(
	name="iced-x86",
	version="1.9.1",
	license="MIT",
	author_email="de4dot@gmail.com",
	author="0xd4d",
	description="iced-x86 is a high performance and correct x86/x64 disassembler, assembler and instruction decoder",
	long_description=readme_md,
	long_description_content_type="text/markdown",
	url="https://github.com/0xd4d/iced/tree/master/src/rust/iced-x86-py",
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
	rust_extensions=[RustExtension("iced_x86._iced_x86_py", path="Cargo.toml", strip=Strip.All)],
	include_package_data=True,
	zip_safe=False,
)
