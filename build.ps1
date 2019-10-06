param([switch]$NoMsbuild, [switch]$NoTest, [switch]$NoCoverage)

$ErrorActionPreference = 'Stop'

$configuration = 'Release'
$netcore_tfm = 'netcoreapp3.0'

#
# dotnet build isn't used because it can't build net35 tfms
#

$env:MoreDefineConstants = ''
$env:NoTargetFrameworkNet35 = ''

if ($IsWindows) {
	$useMsbuild = $true
}
else {
	$useMsbuild = $false
}
if ($NoMsbuild) {
	$useMsbuild = $false
}

if (!$useMsbuild) {
	# There are currently no net35 reference assemblies on nuget
	$env:NoTargetFrameworkNet35 = 'true'
}

if ($useMsbuild) {
	msbuild -v:m -restore -t:Build -p:Configuration=$configuration
	if ($LASTEXITCODE) { exit $LASTEXITCODE }
}
else {
	dotnet build -v:m -c $configuration
	if ($LASTEXITCODE) { exit $LASTEXITCODE }
}

if (!$NoTest) {
	if (!(Test-Path Iced.UnitTests/bin/$configuration/$netcore_tfm)) { throw "Invalid tfm: $netcore_tfm" }
	$collectCoverage = if ($NoCoverage) { '' } else { 'true' }
	dotnet test -c $configuration -f $netcore_tfm -p:Exclude='\"[Iced]Iced.Intel.InstructionMemorySizes,[Iced]Iced.Intel.EncoderInternal.OpCodeHandlers,[Iced]Iced.Intel.InstructionInfoInternal.InfoHandlers,[Iced]Iced.Intel.MnemonicUtils,[Iced]Iced.Intel.InstructionOpCounts\"' -p:ExcludeByFile="$PWD\Iced\**\*.g.cs" -p:ExcludeByAttribute='ObsoleteAttribute' -p:CollectCoverage=$collectCoverage -p:CoverletOutputFormat=lcov --no-build Iced.UnitTests/Iced.UnitTests.csproj -- RunConfiguration.NoAutoReporters=true RunConfiguration.TargetPlatform=X64
	if ($LASTEXITCODE) { exit $LASTEXITCODE }
}

# Don't include the IVT in the final binary
$env:MoreDefineConstants = 'IcedNoIVT'
if ($useMsbuild) {
	msbuild -v:m -t:Clean -p:Configuration=$configuration
	if ($LASTEXITCODE) { exit $LASTEXITCODE }
	msbuild -v:m -restore -t:Build -p:Configuration=$configuration Iced/Iced.csproj
	if ($LASTEXITCODE) { exit $LASTEXITCODE }
	msbuild -v:m -t:Pack -p:Configuration=$configuration Iced/Iced.csproj
	if ($LASTEXITCODE) { exit $LASTEXITCODE }
}
else {
	dotnet clean -v:m -c $configuration
	if ($LASTEXITCODE) { exit $LASTEXITCODE }
	dotnet pack -v:m -c $configuration Iced/Iced.csproj
	if ($LASTEXITCODE) { exit $LASTEXITCODE }
}

$env:MoreDefineConstants = ''
$env:NoTargetFrameworkNet35 = ''
