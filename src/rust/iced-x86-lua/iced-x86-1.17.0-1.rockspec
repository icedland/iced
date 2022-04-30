local modver = "1.17.0"
local specrev = "-1"
local libname = "iced_x86_priv"

package = "iced-x86"
version = modver .. specrev
source = {
	url = "git+https://git@github.com/icedland/iced.git",
	tag = "v1.17.0",
}
description = {
	summary = "x86/x64 disassembler, assembler and instruction decoder",
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
			--TODO: call a luarocks-build-windows.cmd file
		},
	},
}
