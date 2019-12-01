/*
Copyright (C) 2018-2019 de4dot@gmail.com

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using Generator.Constants;
using Generator.Constants.Rust;
using Generator.Enums;
using Generator.Enums.InstructionInfo;
using Generator.Enums.Rust;

namespace Generator.InstructionInfo.Rust {
	[Generator(TargetLanguage.Rust, GeneratorNames.InstrInfo)]
	sealed class RustInstrInfoGenerator : InstrInfoGenerator {
		readonly IdentifierConverter idConverter;
		readonly GeneratorOptions generatorOptions;
		readonly RustEnumsGenerator enumGenerator;
		readonly RustConstantsGenerator constantsGenerator;

		public RustInstrInfoGenerator(GeneratorOptions generatorOptions) {
			idConverter = RustIdentifierConverter.Create();
			this.generatorOptions = generatorOptions;
			enumGenerator = new RustEnumsGenerator(generatorOptions);
			constantsGenerator = new RustConstantsGenerator(generatorOptions);
		}

		protected override void Generate(EnumType enumType) => enumGenerator.Generate(enumType);
		protected override void Generate(ConstantsType constantsType) => constantsGenerator.Generate(constantsType);

		protected override void Generate((InstrInfo info, uint dword1, uint dword2)[] infos) {
			//TODO:
		}

		protected override void Generate(RflagsBits[] read, RflagsBits[] undefined, RflagsBits[] written, RflagsBits[] cleared, RflagsBits[] set, RflagsBits[] modified) {
			//TODO:
		}

		protected override void Generate((EnumValue cpuidInternal, EnumValue[] cpuidFeatures)[] cpuidFeatures) {
			//TODO:
		}

		protected override void GenerateCore() {
			//TODO:
		}
	}
}
