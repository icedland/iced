@echo off
cls

::SET CARGO_TARGET_DIR=C:\tmp\targets\myapp64
::SET RELEASE_DIR=%CARGO_TARGET_DIR%\release

::cargo clean
::Pause

echo Cleaning x64, Release
cargo clean --release
echo Cleaning x64, Debug
cargo clean

echo Cleaning x86, Release
cargo clean --target i686-pc-windows-msvc --release
echo Cleaning x86, Debug
cargo clean --target i686-pc-windows-msvc

pause