@echo off
cls

::SET CARGO_TARGET_DIR=C:\tmp\targets\myapp64
::SET RELEASE_DIR=%CARGO_TARGET_DIR%\release

::_Clean.bat
::Pause

echo Building x64, Release
cargo build --release
echo Building x64, Debug
cargo build

echo Building x86, Release
cargo build --target i686-pc-windows-msvc --release
echo Building x86, Debug
cargo build --target i686-pc-windows-msvc

pause