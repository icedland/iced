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

# Build with minimum supported Rust version since later versions require a later GLIBC
msrv="1.63.0"
curl https://sh.rustup.rs | sh -s -- -y --profile=minimal --default-toolchain=$msrv
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

if [ "$pypy" = "y" ]; then
	mkdir -p /tmp/pypy
	cd /tmp/pypy
	echo "Downloading PyPy"
	arch=$(uname -m)
	if [ "$arch" = "x86_64" ]; then
		# 7.3.8 requires GLIBC 2.14 but manylinux2010 only has 2.12
		curl https://downloads.python.org/pypy/pypy3.8-v7.3.7-linux64.tar.bz2 -o pypy3.8.tar.bz2
	elif [ "$arch" = "aarch64" ]; then
		curl https://downloads.python.org/pypy/pypy3.8-v7.3.7-aarch64.tar.bz2 -o pypy3.8.tar.bz2
	else
		echo "Non-supported arch: $arch"
		exit 1
	fi
	for f in pypy*.tar.*; do
		tar xf "$f"
	done
	cd "$THISDIR"
fi

for PYBIN in /opt/python/cp38*/bin/python; do
	# Make sure the files don't get extra *.so files (should be 1 per file)
	rm -rf build/
	"$PYBIN" -m pip install -r requirements.txt
	"$PYBIN" setup.py bdist_wheel --py-limited-api=cp38
done
for PYBIN in /tmp/pypy/pypy3.*/bin/pypy; do
	if [ -f "$PYBIN" ]; then
		rm -rf build/
		"$PYBIN" -m ensurepip
		"$PYBIN" -m pip install -r requirements.txt
		"$PYBIN" setup.py bdist_wheel
	fi
done
rm -rf build/

mv dist orig-dist
mkdir dist
for whl in orig-dist/*.whl; do
	auditwheel repair "$whl" -w dist/
done
rm -rf orig-dist

for PYBIN in /opt/python/cp{38,39,310,311}*/bin/python /tmp/pypy/pypy3.*/bin/pypy; do
	if [ -f "$PYBIN" ]; then
		"$PYBIN" -m pip install -U pytest
		"$PYBIN" -m pip install iced-x86 --no-index -f dist/ --only-binary iced-x86
		"$PYBIN" -m pytest --color=yes --code-highlight=yes
		"$PYBIN" -m pip uninstall -y iced-x86
	fi
done
