@echo off
cls

cbindgen --config cbindgen.toml --lang c --crate Iced --output iCed.h

pause