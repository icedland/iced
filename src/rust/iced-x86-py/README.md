TODO:

## Building the code

If on Windows, replace `python3` with `python`.

Prerequisites:

- Rust: https://www.rust-lang.org/tools/install
- Some C compiler, eg. `apt install gcc`
- Python >= 3.6: https://www.python.org/downloads/
- `python3 -m pip install -U setuptools wheel setuptools-rust`

```sh
# Create the wheel
python3 setup.py bdist_wheel
# Install the built wheel
python3 -m pip install iced-x86 --no-index -f dist
# Uninstall your built copy
python3 -m pip uninstall iced-x86
```

Tests:

- `python3 -m pip install -U pytest`

```sh
python3 setup.py bdist_wheel
python3 -m pip install iced-x86 --no-index -f dist
python3 -m pytest
python3 -m pip uninstall -y iced-x86
```

TODO:
