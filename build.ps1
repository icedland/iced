param([switch]$NoTest, [switch]$NoCoverage, [string]$Configuration = 'Release', [switch]$NoPack)

$ErrorActionPreference = 'Stop'

$netcore_tfm = 'netcoreapp3.1'
$net_tfm = 'net48'
$xunitVersion = '2.4.1'
$xunitNetTfmVersion = 'net472'

$env:MoreDefineConstants = ''

if ($null -eq $IsWindows) {
	$IsWindows = $true
}

dotnet build -v:m -c $configuration src/csharp/Iced.sln
if ($LASTEXITCODE) { exit $LASTEXITCODE }

if (!$NoTest) {
	if ($IsWindows) {
		& $HOME/.nuget/packages/xunit.runner.console/$xunitVersion/tools/$xunitNetTfmVersion/xunit.console.exe src/csharp/Intel/Iced.UnitTests/bin/$configuration/$net_tfm/Iced.UnitTests.dll -noappdomain
		if ($LASTEXITCODE) { exit $LASTEXITCODE }
		& $HOME/.nuget/packages/xunit.runner.console/$xunitVersion/tools/$xunitNetTfmVersion/xunit.console.x86.exe src/csharp/Intel/Iced.UnitTests/bin/$configuration/$net_tfm/Iced.UnitTests.dll -noappdomain
		if ($LASTEXITCODE) { exit $LASTEXITCODE }
	}
	if (!(Test-Path src/csharp/Intel/Iced.UnitTests/bin/$configuration/$netcore_tfm)) { throw "Invalid tfm: $netcore_tfm" }
	$collectCoverage = if ($NoCoverage) { '' } else { 'true' }
	dotnet test -c $configuration -f $netcore_tfm -p:Exclude='\"[Iced]Iced.Intel.InstructionMemorySizes,[Iced]Iced.Intel.EncoderInternal.OpCodeHandlers,[Iced]Iced.Intel.InstructionInfoInternal.InfoHandlers,[Iced]Iced.Intel.MnemonicUtils,[Iced]Iced.Intel.InstructionOpCounts\"' -p:ExcludeByFile="$PWD\src\csharp\Intel\Iced\**\*.g.cs" -p:ExcludeByAttribute='ObsoleteAttribute' -p:CollectCoverage=$collectCoverage -p:CoverletOutputFormat=lcov --no-build src/csharp/Intel/Iced.UnitTests/Iced.UnitTests.csproj -- RunConfiguration.NoAutoReporters=true RunConfiguration.TargetPlatform=X64
	if ($LASTEXITCODE) { exit $LASTEXITCODE }
}

if (!$NoPack) {
	# Don't include the IVT in the final binary
	$env:MoreDefineConstants = 'IcedNoIVT'
	dotnet clean -v:m -c $configuration src/csharp/Iced.sln
	if ($LASTEXITCODE) { exit $LASTEXITCODE }
	dotnet pack -v:m -c $configuration src/csharp/Intel/Iced/Iced.csproj
	if ($LASTEXITCODE) { exit $LASTEXITCODE }
}

$env:MoreDefineConstants = ''
