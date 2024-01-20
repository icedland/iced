local modver = "1.21.0"
local specrev = "-1"
local libname = "iced_x86"

package = "iced_x86"
version = modver .. specrev
source = {
	url = "git+https://git@github.com/icedland/iced.git",
	tag = "v1.21.0",
}
description = {
	summary = "x86/x64 disassembler, assembler and instruction decoder",
	detailed = [[
		x86/x64 disassembler, assembler and instruction decoder

		This Lua rock uses a library written in Rust so Rust must be installed
		to compile it: https://www.rust-lang.org/tools/install
	]],
	license = "MIT",
	homepage = "https://github.com/icedland/iced",
}
dependencies = {
	"lua >= 5.1, < 5.5",
}
build = {
	platforms = {
		unix = {
			type = "command",
			build_command = 'sh luarocks-build-unix.sh build "$(LUA)"',
			install_command = 'sh luarocks-build-unix.sh install "'
				.. package
				.. '" "'
				.. libname
				.. '" "$(PREFIX)" "$(LIBDIR)" "$(LUADIR)"',
		},
		windows = {
			type = "command",
			build_command = "Yeah, no! Send a PR to support Windows: https://github.com/icedland/iced",
			--TODO: call a luarocks-build-windows.cmd file
		},
	},
}
