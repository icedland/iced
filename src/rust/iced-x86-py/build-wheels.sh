#!/bin/bash
set -e

curl https://sh.rustup.rs | sh -s -- -y --profile=minimal
export PATH="$HOME/.cargo/bin:$PATH"

# Make sure crates.io isn't used
echo "paths = [\"$(pwd)/../iced-x86\"]" > "$HOME/.cargo/config.toml"

for PYBIN in /opt/python/cp{36,37,38,39}*/bin; do
	# Make sure the files don't get extra *.so files (should be 1 per file)
	rm -rf build/
	"$PYBIN/python" -m pip install -r requirements.txt
	"$PYBIN/python" setup.py bdist_wheel
done
rm -rf build/

mv dist orig-dist
mkdir dist
for whl in orig-dist/*.whl; do
	auditwheel repair "$whl" -w dist/
done

for PYBIN in /opt/python/cp{36,37,38,39}*/bin; do
	"$PYBIN/python" -m pip install -U pytest
	"$PYBIN/python" -m pip install iced-x86 --no-index -f dist
	"$PYBIN/python" -m pytest --color=yes --code-highlight=yes
	"$PYBIN/python" -m pip uninstall -y iced-x86
done
