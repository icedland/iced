$ErrorActionPreference = 'Stop'

#
# dotnet build isn't used because it can't build net35 tfms
#

$env:MoreDefineConstants = ''

msbuild -v:m -restore -t:Build -p:Configuration=Release
if ($LASTEXITCODE) { exit $LASTEXITCODE }

dotnet test -c Release -f netcoreapp3.0 -p:Exclude='\"[Iced]Iced.Intel.InstructionMemorySizes,[Iced]Iced.Intel.EncoderInternal.OpCodeHandlers,[Iced]Iced.Intel.InstructionInfoInternal.InfoHandlers,[Iced]Iced.Intel.MnemonicUtils,[Iced]Iced.Intel.InstructionOpCounts\"' -p:ExcludeByFile="$PWD\Iced\**\*.g.cs" -p:ExcludeByAttribute='ObsoleteAttribute' -p:CollectCoverage=true -p:CoverletOutputFormat=lcov --no-build Iced.UnitTests\Iced.UnitTests.csproj -- RunConfiguration.NoAutoReporters=true RunConfiguration.TargetPlatform=X64
if ($LASTEXITCODE) { exit $LASTEXITCODE }

# Don't include the IVT in the final binary
$env:MoreDefineConstants = 'IcedNoIVT'
msbuild -v:m -t:Clean -p:Configuration=Release
if ($LASTEXITCODE) { exit $LASTEXITCODE }
msbuild -v:m -restore -t:Build -p:Configuration=Release Iced\Iced.csproj
if ($LASTEXITCODE) { exit $LASTEXITCODE }
msbuild -v:m -t:Pack -p:Configuration=Release Iced\Iced.csproj
if ($LASTEXITCODE) { exit $LASTEXITCODE }
$env:MoreDefineConstants = ''
