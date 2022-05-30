#!/bin/sh
set -e

# This file should only be called by luarocks, see the *.rockspec file

check_requirements() {
	if ! which cargo > /dev/null; then
		echo ""
		echo "***************************************************"
		echo "    Rust is required to build this library!"
		echo "    Install Rust:"
		echo "        https://www.rust-lang.org/tools/install"
		echo "***************************************************"
		exit 1
	fi
}

build() {
	lua_ver=$("$1" -v 2>&1)
	if echo "$lua_ver" | grep 'Lua 5\.1\.' > /dev/null; then
		lua_feat=lua5_1
	elif echo "$lua_ver" | grep 'Lua 5\.2\.' > /dev/null; then
		lua_feat=lua5_2
	elif echo "$lua_ver" | grep 'Lua 5\.3\.' > /dev/null; then
		lua_feat=lua5_3
	elif echo "$lua_ver" | grep 'Lua 5\.4\.' > /dev/null; then
		lua_feat=lua5_4
	else
		echo "Unsupported Lua version: $lua_ver"
		exit 1
	fi
	# Env var set by build-lua
	lua_feat="$lua_feat $ICED_LUA_EXTRA_FEATURES"
	cargo build --release -v --features "$lua_feat"
}

install() {
	package="$1"
	libname="$2"
	prefix="$3"
	libdir="$4"
	luadir="$5"

	libext=".so"
	if [ "$(uname)" = "Darwin" ]; then
		libext=".dylib"
	fi

	mkdir -p "$luadir/$package"
	cp -r lua/* "$luadir/$package"
	cp "target/release/lib$libname$libext" "$libdir/$libname$libext"
}

check_requirements
if [ "$1" = "build" ]; then
	if [ "$#" -ne 2 ]; then
		echo "Too many/few args: $@"
		exit 1
	fi
	shift
	build "$@"
elif [ "$1" = "install" ]; then
	if [ "$#" -ne 6 ]; then
		echo "Too many/few args: $@"
		exit 1
	fi
	shift
	install "$@"
else
	echo "Invalid args: $@"
	exit 1
fi
