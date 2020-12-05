TODO:

## Building the code

If on Windows, replace `python3` in all commands with `python` or `py`.

Prerequisites:

- Rust: https://www.rust-lang.org/tools/install
- Python >= 3.6: https://www.python.org/downloads/
- `python3 -m pip install -r requirements.txt`

```sh
# Create the wheel
python3 setup.py bdist_wheel
# Install the built wheel
python3 -m pip install iced-x86 --no-index -f dist
# Uninstall your built copy
python3 -m pip uninstall iced-x86
```

Tests:

- `python3 -m pip install -r requirements-dev.txt`

```sh
python3 setup.py bdist_wheel
python3 -m pip install iced-x86 --no-index -f dist
python3 -m pytest
python3 -m pip uninstall -y iced-x86
```

TODO:
