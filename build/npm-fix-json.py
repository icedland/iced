import sys
import json

iced_version = "1.21.0"

if len(sys.argv) != 3:
	raise ValueError(f"usage {sys.argv[0]} npm_package_json test_package_json")

npm_package_json = sys.argv[1]
test_package_json = sys.argv[2]

with open(npm_package_json, "r", encoding="utf-8") as file:
	j = json.loads(file.read())
with open(test_package_json, "r", encoding="utf-8") as file:
	testj = json.loads(file.read())

j["version"] = iced_version
j["keywords"] = ["disassembler", "assembler", "decoder", "encoder", "asm", "disassembly",
				"x86", "amd64", "x64", "x86_64", "wasm", "webassembly", "rust"]
j["bugs"] = "https://github.com/icedland/iced/issues"
j["repository"]["directory"] = "src/rust/iced-x86-js"
j["files"].append("tests/*")
j["directories"] = {"test": "tests"}

for key in testj:
	if key == "dependencies":
		continue
	if key == "scripts" or key == "devDependencies":
		j[key] = testj[key]
	else:
		raise ValueError(f"Unknown key: `{key}`")

with open(npm_package_json, "w", encoding="utf-8") as file:
	json.dump(j, file, indent=2)
