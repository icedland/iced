#!/bin/bash
set -e

curl https://sh.rustup.rs | sh -s -- -y --profile=minimal
export PATH="$HOME/.cargo/bin:$PATH"

# Make sure crates.io isn't used, see build/build-python for more info
iced_x86_dir="$(pwd)/../iced-x86"
if [ ! -d "$iced_x86_dir" ]; then
	echo "Dir does not exist: $iced_x86_dir"
	exit 1
fi
if ! grep -E '^#pathci$' Cargo.toml 2>&1 > /dev/null; then
	echo "Cargo.toml is patched"
	exit 1
fi
sed -i -e "s&^#pathci$&path = \"$iced_x86_dir\"&" Cargo.toml

for PYBIN in /opt/python/cp36*/bin; do
	# Make sure the files don't get extra *.so files (should be 1 per file)
	rm -rf build/
	"$PYBIN/python" -m pip install -r requirements.txt
	"$PYBIN/python" setup.py bdist_wheel --py-limited-api=cp36
done
rm -rf build/

mv dist orig-dist
mkdir dist
for whl in orig-dist/*.whl; do
	auditwheel repair "$whl" -w dist/
done
rm -rf orig-dist

for PYBIN in /opt/python/cp{36,37,38,39}*/bin; do
	"$PYBIN/python" -m pip install -U pytest
	"$PYBIN/python" -m pip install iced-x86 --no-index -f dist/ --only-binary :all:
	"$PYBIN/python" -m pytest --color=yes --code-highlight=yes
	"$PYBIN/python" -m pip uninstall -y iced-x86
done
