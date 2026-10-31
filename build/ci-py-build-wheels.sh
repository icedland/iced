#!/bin/bash
set -e

if [ ! "$GITHUB_ACTIONS" ]; then
	echo "This file should only be executed from GitHub Actions"
	exit 1
fi

python=python
py_kind=cpy
build=n
versions=

while [ "$#" -gt 0 ]; do
	case $1 in
	--python) shift; python=$1 ;;
	--py-kind) shift; py_kind=$1 ;;
	--build) build=y ;;
	# Space/newline separated CPython versions, eg. "3.11 3.12 3.13". They must all be in PATH
	# (eg. installed by one setup-python step). The wheel is built with the first one (if --build)
	# and tested with all of them. Overrides --python and --py-kind.
	--versions) shift; versions=$1 ;;
	*) echo "Unknown arg: $1"; exit 1 ;;
	esac
	shift
done

# Finds the CPython interpreter with version $1 (eg. 3.12) in PATH. Works on Windows too where
# all interpreters are called python.exe so we can't use eg. python3.12
find_cpython() {
	local wanted=$1
	local dir exe found
	local IFS=:
	for dir in $PATH; do
		case "$dir" in
		# Windows Store 'python.exe' stubs
		*WindowsApps*) continue ;;
		esac
		for exe in "$dir/python.exe" "$dir/python"; do
			if [ -f "$exe" ] && [ -x "$exe" ]; then
				found=$("$exe" -c 'import platform, sys; print("%s %d.%d" % (platform.python_implementation(), sys.version_info[0], sys.version_info[1]))' 2>/dev/null || true)
				if [ "$found" = "CPython $wanted" ]; then
					echo "$exe"
					return 0
				fi
			fi
		done
	done
	echo "Couldn't find CPython $wanted in PATH" 1>&2
	return 1
}

pythons=()
if [ -n "$versions" ]; then
	py_kind=cpy
	# Split on spaces and newlines
	for ver in $versions; do
		py=$(find_cpython "$ver")
		pythons+=("$py")
	done
	if [ "${#pythons[@]}" -eq 0 ]; then
		echo "--versions: no versions"
		exit 1
	fi
else
	pythons=("$python")
fi
build_python=${pythons[0]}

for py in "${pythons[@]}"; do
	echo "Python: $py"
	"$py" --version
	"$py" -m pip --version
	"$py" -m pip install -U build setuptools wheel setuptools-rust pytest
done

# Needed so the wheel files don't get extra *.{so,pyd} files (should have exactly one)
# from earlier builds
git clean -xdf

cd src/rust/iced-x86-py
cargo_toml=Cargo.toml

patchci_verify_not_patched() {
	# msys grep fails if we use $
	if ! grep -E '^#pathci' "$cargo_toml" 2>&1 > /dev/null; then
		echo "Cargo.toml is patched"
		exit 1
	fi
}

patchci_verify_patched() {
	# msys grep fails if we use $
	if grep -E '^#pathci' "$cargo_toml" 2>&1 > /dev/null; then
		echo "Cargo.toml is not patched"
		exit 1
	fi
}

# See build-python
patchci_patch() {
	patchci_verify_not_patched

	curr_dir=$(pwd)
	cd "$root_dir"

	if [ "$OSTYPE" = "msys" ]; then
		iced_x86_dir="$(pwd -W)/../iced-x86"
	else
		iced_x86_dir="$(pwd)/../iced-x86"
	fi
	if [ ! -d "$iced_x86_dir" ]; then
		echo "Dir does not exist: $iced_x86_dir"
		exit 1
	fi

	sed -i -e "s&^#pathci$&path = \"$iced_x86_dir\"&" "$cargo_toml"

	cd "$curr_dir"
}

patchci_undo_patch() {
	git checkout "$cargo_toml"
}

# Build the wheel with the minimum supported Python version only
if [ "$build" = "y" ]; then
	patchci_verify_not_patched
	patchci_patch
	patchci_verify_patched

	extra_args=()
	if [ "$py_kind" = "cpy" ]; then
		extra_args=(--config-setting=--build-option=--py-limited-api=cp311)
	fi
	echo "Building the wheel with $build_python"
	"$build_python" -m build --wheel "${extra_args[@]}"
	mkdir -p /tmp/py-dist
	cp dist/* /tmp/py-dist

	patchci_undo_patch
fi

for py in "${pythons[@]}"; do
	echo "Testing it with $py"
	"$py" -m pip install iced-x86 --no-index -f /tmp/py-dist --only-binary iced-x86
	"$py" -m pytest --color=yes --code-highlight=yes
	"$py" -m pip uninstall -y iced-x86
done
