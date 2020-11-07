#!/bin/sh
set -e

root_dir=$(dirname "$0")
root_dir=$(cd "$root_dir/.." && pwd)
if [ ! -f "$root_dir/LICENSE.txt" ]; then
	echo "Couldn't find the root dir"
	exit 1
fi

if [ "$OSTYPE" == "msys" ]; then
	is_windows=y
else
	is_windows=n
fi
net_tfm="netcoreapp3.1"
net_framework_tfm="net48"
xunit_version="2.4.1"
xunit_net_tfm_version="net472"
configuration=Release
no_gen_check=n
no_test=n
no_pack=n
no_coverage=n
quick_check=n

function new_func {
	echo
	echo "****************************************************************"
	echo "$1"
	echo "****************************************************************"
	echo
}

function clean_dotnet_build_output {
	dotnet clean -v:m -c $configuration "$root_dir/src/csharp/Iced.sln"
}

function generator_check {
	new_func "Run generator, verify no diff"

	dotnet run -c $configuration -p "$root_dir/src/csharp/Intel/Generator/Generator.csproj"
	git diff --exit-code
}

function build_features {
	new_func "Build one feature at a time"

	allFeatures=(
		"DECODER"
		"ENCODER"
		"ENCODER;BLOCK_ENCODER"
		"ENCODER;BLOCK_ENCODER;CODE_ASSEMBLER"
		"ENCODER;OPCODE_INFO"
		"INSTR_INFO"
		"GAS"
		"INTEL"
		"MASM"
		"NASM"
		"FAST_FMT"
		"DECODER;ENCODER;BLOCK_ENCODER;CODE_ASSEMBLER;OPCODE_INFO;INSTR_INFO;GAS;INTEL;MASM;NASM;FAST_FMT;NO_VEX"
		"DECODER;ENCODER;BLOCK_ENCODER;CODE_ASSEMBLER;OPCODE_INFO;INSTR_INFO;GAS;INTEL;MASM;NASM;FAST_FMT;NO_EVEX"
		"DECODER;ENCODER;BLOCK_ENCODER;CODE_ASSEMBLER;OPCODE_INFO;INSTR_INFO;GAS;INTEL;MASM;NASM;FAST_FMT;NO_XOP"
		"DECODER;ENCODER;BLOCK_ENCODER;CODE_ASSEMBLER;OPCODE_INFO;INSTR_INFO;GAS;INTEL;MASM;NASM;FAST_FMT;NO_D3NOW"
		"DECODER;ENCODER;BLOCK_ENCODER;CODE_ASSEMBLER;OPCODE_INFO;INSTR_INFO;GAS;INTEL;MASM;NASM;FAST_FMT;NO_VEX;NO_EVEX;NO_XOP;NO_D3NOW"
	)

	for features in "${allFeatures[@]}"; do
		clean_dotnet_build_output
		echo
		echo "==== $features ===="
		echo
		IcedFeatureFlags="$features" dotnet build -v:m -c $configuration "$root_dir/src/csharp/Intel/Iced/Iced.csproj"
	done

	allFeatures=(
		"DECODER"
		"DECODER;ENCODER"
		"DECODER;ENCODER;BLOCK_ENCODER"
		"DECODER;ENCODER;BLOCK_ENCODER;CODE_ASSEMBLER"
		"DECODER;ENCODER;OPCODE_INFO"
		"DECODER;INSTR_INFO"
		"DECODER;GAS"
		"DECODER;INTEL"
		"DECODER;MASM"
		"DECODER;NASM"
		"DECODER;FAST_FMT"
		"DECODER;ENCODER;BLOCK_ENCODER;CODE_ASSEMBLER;OPCODE_INFO;INSTR_INFO;GAS;INTEL;MASM;NASM;FAST_FMT;NO_VEX"
		"DECODER;ENCODER;BLOCK_ENCODER;CODE_ASSEMBLER;OPCODE_INFO;INSTR_INFO;GAS;INTEL;MASM;NASM;FAST_FMT;NO_EVEX"
		"DECODER;ENCODER;BLOCK_ENCODER;CODE_ASSEMBLER;OPCODE_INFO;INSTR_INFO;GAS;INTEL;MASM;NASM;FAST_FMT;NO_XOP"
		"DECODER;ENCODER;BLOCK_ENCODER;CODE_ASSEMBLER;OPCODE_INFO;INSTR_INFO;GAS;INTEL;MASM;NASM;FAST_FMT;NO_D3NOW"
		"DECODER;ENCODER;BLOCK_ENCODER;CODE_ASSEMBLER;OPCODE_INFO;INSTR_INFO;GAS;INTEL;MASM;NASM;FAST_FMT;NO_VEX;NO_EVEX;NO_XOP;NO_D3NOW"
	)
	for features in "${allFeatures[@]}"; do
		clean_dotnet_build_output
		echo
		echo "==== TEST $features ===="
		echo
		IcedFeatureFlags="$features" dotnet build -v:m -c $configuration "$root_dir/src/csharp/Intel/Iced.UnitTests/Iced.UnitTests.csproj"
	done

	clean_dotnet_build_output
}

function build_test {
	new_func "Build, test"

	dotnet build -v:m -c $configuration "$root_dir/src/csharp/Iced.sln"

	if [ "$no_test" != "y" ]; then
		if [ "$is_windows" == "y" ]; then
			echo
			echo "==== TEST (.NET Framework x64) ===="
			echo
			~/.nuget/packages/xunit.runner.console/$xunit_version/tools/$xunit_net_tfm_version/xunit.console.exe "$root_dir/src/csharp/Intel/Iced.UnitTests/bin/$configuration/$net_framework_tfm/Iced.UnitTests.dll" -noappdomain

			echo
			echo "==== TEST (.NET Framework x86) ===="
			echo
			~/.nuget/packages/xunit.runner.console/$xunit_version/tools/$xunit_net_tfm_version/xunit.console.x86.exe "$root_dir/src/csharp/Intel/Iced.UnitTests/bin/$configuration/$net_framework_tfm/Iced.UnitTests.dll" -noappdomain
		fi

		echo
		echo "==== TEST ===="
		echo
		if [ ! -d "$root_dir/src/csharp/Intel/Iced.UnitTests/bin/$configuration/$net_tfm" ]; then
			echo "Invalid tfm: $net_tfm"
			exit 1
		fi
		if [ "$no_coverage" != "y" ]; then
			collect_coverage=true
		else
			collect_coverage=
		fi
		# Full path needed so have to find the Windows path if this is Windows
		if [ "$is_windows" == "y" ]; then
			cov_dir=$(cygpath -wa "$root_dir")
		else
			cov_dir="$root_dir"
		fi
		dotnet test -c $configuration -f $net_tfm -p:Exclude='"[Iced]Iced.Intel.InstructionMemorySizes,[Iced]Iced.Intel.EncoderInternal.OpCodeHandlers,[Iced]Iced.Intel.InstructionInfoInternal.InfoHandlers,[Iced]Iced.Intel.MnemonicUtils,[Iced]Iced.Intel.InstructionOpCounts"' -p:ExcludeByFile="$cov_dir/src/csharp/Intel/Iced/**/*.g.cs" -p:ExcludeByAttribute='ObsoleteAttribute' -p:CollectCoverage=$collect_coverage -p:CoverletOutputFormat=lcov --no-build "$root_dir/src/csharp/Intel/Iced.UnitTests/Iced.UnitTests.csproj" -- RunConfiguration.NoAutoReporters=true RunConfiguration.TargetPlatform=X64
	fi

	if [ "$no_pack" != "y" ]; then
		echo
		echo "==== PACK ===="
		echo
		clean_dotnet_build_output
		# Don't include the IVT in the final binary
		IcedDefineConstants="IcedNoIVT" dotnet pack -v:m -c $configuration "$root_dir/src/csharp/Intel/Iced/Iced.csproj"
	fi
}

while [ "$#" -gt 0 ]; do
	case $1 in
	--no-gen-check) no_gen_check=y ;;
	--no-test) no_test=y ;;
	--no-pack) no_pack=y ;;
	--no-coverage) no_coverage=y ;;
	--quick-check) quick_check=y ;;
	*) echo "Unknown arg: $1"; exit 1 ;;
	esac
	shift
done

echo "dotnet version"
dotnet --version
if [ "$quick_check" != "y" ]; then
	if [ "$no_gen_check" != "y" ]; then
		generator_check
	fi
	build_features
fi
build_test
