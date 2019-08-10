@echo off
REM dotnet build isn't used because it can't build net35 tfms

msbuild -v:m -restore -t:Build -p:Configuration=Release || goto :error

"C:\Program Files\dotnet\dotnet.exe" test -c Release --no-build Iced.UnitTests\Iced.UnitTests.csproj -- RunConfiguration.NoAutoReporters=true RunConfiguration.TargetPlatform=X64 || goto :error
"C:\Program Files (x86)\dotnet\dotnet.exe" test -c Release --no-build Iced.UnitTests\Iced.UnitTests.csproj -- RunConfiguration.NoAutoReporters=true RunConfiguration.TargetPlatform=X86 || goto :error

set MoreDefineConstants=IcedNoIVT
msbuild -v:m -t:Clean -p:Configuration=Release || goto :error
msbuild -v:m -restore -t:Build -p:Configuration=Release Iced\Iced.csproj || goto :error
msbuild -v:m -t:Pack -p:Configuration=Release Iced\Iced.csproj || goto :error

goto :EOF

:error
exit /b %errorlevel%
