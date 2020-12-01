TODO:

## Building the code

Prerequisites:

- Rust: https://www.rust-lang.org/tools/install
- Some C compiler, eg. `apt install gcc`
- Python >= 3.6: https://www.python.org/downloads/
- `pip3 install -U setuptools wheel setuptools-rust`
	- `pip` if on Windows

If on Windows, replace `python3/pip3` with `python/pip`.

```sh
# Create the wheel
python3 setup.py bdist_wheel
# Install the built wheel
pip3 install --no-index -f dist iced-x86
# Uninstall your built copy
pip3 uninstall iced-x86
```

TODO:
