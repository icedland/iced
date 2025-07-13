#!/bin/bash
set -e

THISDIR=$(pwd)

manylinux_image="$1"
shift

pypy=n
while [ "$#" -gt 0 ]; do
	case $1 in
	--pypy) pypy=y ;;
	*) echo "Unknown arg: $1"; exit 1 ;;
	esac
	shift
done

curl https://sh.rustup.rs | sh -s -- -y --profile=minimal
export PATH="$HOME/.cargo/bin:$PATH"

# Make sure crates.io isn't used, see build/build-python for more info
iced_x86_dir="$THISDIR/../iced-x86"
if [ ! -d "$iced_x86_dir" ]; then
	echo "Dir does not exist: $iced_x86_dir"
	exit 1
fi
if ! grep -E '^#pathci$' Cargo.toml 2>&1 > /dev/null; then
	echo "Cargo.toml is patched"
	exit 1
fi
sed -i -e "s&^#pathci$&path = \"$iced_x86_dir\"&" Cargo.toml

echo "Python bins:"
for PYBIN in $(ls -v /opt/python/*/bin/python); do
	if [ -f "$PYBIN" ]; then
		echo "  $PYBIN"
	fi
done
CPYBINS="$(ls -v /opt/python/{cp38-cp38,cp39-cp39,cp310-cp310,cp311-cp311,cp312-cp312,cp313-cp313}/bin/python)"
cpython_found=
for PYBIN in $CPYBINS; do
	if [ -f "$PYBIN" ]; then
		cpython_found=1
		break
	fi
done
if [ "$cpython_found" == "" ]; then
	echo "Failed to execute python tests: no CPython bins found"
	exit 1
fi
echo "CPYBINS:"
for PYBIN in $CPYBINS; do
	echo "  $PYBIN"
done

PYPYBINS=
if [ "$pypy" = "y" ]; then
	PYPYBINS=$(ls -v /opt/python/pp*-pypy*/bin/python)
	pypy_found=
	for PYBIN in $CPYBINS; do
		if [ -f "$PYBIN" ]; then
			pypy_found=1
			break
		fi
	done
	if [ "$pypy_found" == "" ]; then
		echo "Failed to execute python tests: no PyPy bins found"
		exit 1
	fi
fi
echo "PYPYBINS:"
for PYBIN in $PYPYBINS; do
	echo "  $PYBIN"
done

for PYBIN in /opt/python/cp38*/bin/python; do
	# Make sure the files don't get extra *.so files (should be 1 per file)
	rm -rf build/
	"$PYBIN" -m pip install -r requirements.txt
	"$PYBIN" -m build --wheel --config-setting=--build-option=--py-limited-api=cp38
done
for PYBIN in $PYPYBINS; do
	if [ -f "$PYBIN" ]; then
		rm -rf build/
		"$PYBIN" -m ensurepip
		"$PYBIN" -m pip install -r requirements.txt
		"$PYBIN" -m build --wheel
	fi
done
rm -rf build/

mv dist orig-dist
mkdir dist
for whl in orig-dist/*.whl; do
	auditwheel repair "$whl" -w dist/
done
rm -rf orig-dist

for PYBIN in $CPYBINS $PYPYBINS; do
	if [ -f "$PYBIN" ]; then
		"$PYBIN" -m pip install -U pytest
		"$PYBIN" -m pip install iced-x86 --no-index -f dist/ --only-binary iced-x86
		"$PYBIN" -m pytest --color=yes --code-highlight=yes
		"$PYBIN" -m pip uninstall -y iced-x86
	fi
done
