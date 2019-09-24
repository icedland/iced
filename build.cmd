@echo off
REM dotnet build isn't used because it can't build net35 tfms

msbuild -v:m -restore -t:Build -p:Configuration=Release || goto :error

"%ProgramFiles%\dotnet\dotnet.exe" test -c Release -p:Exclude=\"[Iced]Iced.Intel.InstructionMemorySizes,[Iced]Iced.Intel.EncoderInternal.OpCodeHandlers,[Iced]Iced.Intel.InstructionInfoInternal.InfoHandlers,[Iced]Iced.Intel.MnemonicUtils,[Iced]Iced.Intel.InstructionOpCounts\" -p:ExcludeByFile="Iced\**\*.g.cs" -p:ExcludeByAttribute="ObsoleteAttribute" -p:CollectCoverage=true -p:CoverletOutputFormat=json --no-build Iced.UnitTests\Iced.UnitTests.csproj -- RunConfiguration.NoAutoReporters=true RunConfiguration.TargetPlatform=X64 || goto :error
"%ProgramFiles(x86)%\dotnet\dotnet.exe" test -c Release --no-build Iced.UnitTests\Iced.UnitTests.csproj -- RunConfiguration.NoAutoReporters=true RunConfiguration.TargetPlatform=X86 || goto :error

set MoreDefineConstants=IcedNoIVT
msbuild -v:m -t:Clean -p:Configuration=Release || goto :error
msbuild -v:m -restore -t:Build -p:Configuration=Release Iced\Iced.csproj || goto :error
msbuild -v:m -t:Pack -p:Configuration=Release Iced\Iced.csproj || goto :error

goto :EOF

:error
exit /b %errorlevel%
