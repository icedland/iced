#!/bin/bash
set -e

curl https://sh.rustup.rs | sh -s -- -y --profile=minimal
export PATH="$HOME/.cargo/bin:$PATH"

# Make sure crates.io isn't used
iced_x86_dir="$(pwd)/../iced-x86"
if [ ! -d "$iced_x86_dir" ]; then
	echo "Dir does not exist: $iced_x86_dir"
	exit 1
fi
echo "paths = [\"$iced_x86_dir\"]" > "$HOME/.cargo/config.toml"

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

mkdir dist1
mkdir dist2010
if [ -n "$(ls -A dist/*manylinux1* 2>/dev/null)" ]; then
	cp dist/*manylinux1* dist1/
fi
if [ -n "$(ls -A dist/*manylinux2010* 2>/dev/null)" ]; then
	cp dist/*manylinux2010* dist2010/
fi

for PYBIN in /opt/python/cp{36,37,38,39}*/bin; do
	"$PYBIN/python" -m pip install -U pytest
	for dir in dist dist1 dist2010; do
		if [ -n "$(ls -A "$dir/"* 2>/dev/null)" ]; then
			"$PYBIN/python" -m pip install iced-x86 --no-index -f "$dir/" --only-binary :all:
			"$PYBIN/python" -m pytest --color=yes --code-highlight=yes
			"$PYBIN/python" -m pip uninstall -y iced-x86
		fi
	done
done
