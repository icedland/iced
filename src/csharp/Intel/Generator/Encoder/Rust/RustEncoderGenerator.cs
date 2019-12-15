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

using Generator.Enums;
using Generator.Enums.Rust;

namespace Generator.Encoder.Rust {
	[Generator(TargetLanguage.Rust, GeneratorNames.Encoder)]
	sealed class RustEncoderGenerator : EncoderGenerator {
		readonly IdentifierConverter idConverter;
		readonly GeneratorOptions generatorOptions;
		readonly RustEnumsGenerator enumGenerator;

		public RustEncoderGenerator(GeneratorOptions generatorOptions) {
			idConverter = RustIdentifierConverter.Create();
			this.generatorOptions = generatorOptions;
			enumGenerator = new RustEnumsGenerator(generatorOptions);
		}

		protected override void Generate(EnumType enumType) => enumGenerator.Generate(enumType);

		protected override void Generate((EnumValue opCodeOperandKind, EnumValue legacyOpKind, OpHandlerKind opHandlerKind, object[] args)[] legacy, (EnumValue opCodeOperandKind, EnumValue vexOpKind, OpHandlerKind opHandlerKind, object[] args)[] vex, (EnumValue opCodeOperandKind, EnumValue xopOpKind, OpHandlerKind opHandlerKind, object[] args)[] xop, (EnumValue opCodeOperandKind, EnumValue evexOpKind, OpHandlerKind opHandlerKind, object[] args)[] evex) {
			//TODO:
		}

		protected override void Generate(OpCodeInfo[] opCodes) {
			//TODO:
		}

		protected override void Generate((EnumValue value, uint size)[] immSizes) {
			//TODO:
		}

		protected override void GenerateCore() {
			//TODO:
		}
	}
}
