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

namespace Generator.Tables {
	interface ID3nowCodeValuesTableGenerator {
		void Generate((int index, EnumValue enumValue)[] infos);
	}

	sealed class D3nowCodeValuesTableGenerator {
		readonly ProjectDirs projectDirs;

		public D3nowCodeValuesTableGenerator(ProjectDirs projectDirs) {
			this.projectDirs = projectDirs;
		}

		public void Generate() {
			var generators = new ID3nowCodeValuesTableGenerator[(int)TargetLanguage.Last] {
				new CSharp.CSharpD3nowCodeValuesTableGenerator(projectDirs),
				new Rust.RustD3nowCodeValuesTableGenerator(projectDirs),
			};

			var infos = new (int index, EnumValue enumValue)[] {
				( 0x0C, CodeEnum.Instance["D3NOW_Pi2fw_mm_mmm64"] ),
				( 0x0D, CodeEnum.Instance["D3NOW_Pi2fd_mm_mmm64"] ),
				( 0x1C, CodeEnum.Instance["D3NOW_Pf2iw_mm_mmm64"] ),
				( 0x1D, CodeEnum.Instance["D3NOW_Pf2id_mm_mmm64"] ),
				( 0x86, CodeEnum.Instance["D3NOW_Pfrcpv_mm_mmm64"] ),
				( 0x87, CodeEnum.Instance["D3NOW_Pfrsqrtv_mm_mmm64"] ),
				( 0x8A, CodeEnum.Instance["D3NOW_Pfnacc_mm_mmm64"] ),
				( 0x8E, CodeEnum.Instance["D3NOW_Pfpnacc_mm_mmm64"] ),
				( 0x90, CodeEnum.Instance["D3NOW_Pfcmpge_mm_mmm64"] ),
				( 0x94, CodeEnum.Instance["D3NOW_Pfmin_mm_mmm64"] ),
				( 0x96, CodeEnum.Instance["D3NOW_Pfrcp_mm_mmm64"] ),
				( 0x97, CodeEnum.Instance["D3NOW_Pfrsqrt_mm_mmm64"] ),
				( 0x9A, CodeEnum.Instance["D3NOW_Pfsub_mm_mmm64"] ),
				( 0x9E, CodeEnum.Instance["D3NOW_Pfadd_mm_mmm64"] ),
				( 0xA0, CodeEnum.Instance["D3NOW_Pfcmpgt_mm_mmm64"] ),
				( 0xA4, CodeEnum.Instance["D3NOW_Pfmax_mm_mmm64"] ),
				( 0xA6, CodeEnum.Instance["D3NOW_Pfrcpit1_mm_mmm64"] ),
				( 0xA7, CodeEnum.Instance["D3NOW_Pfrsqit1_mm_mmm64"] ),
				( 0xAA, CodeEnum.Instance["D3NOW_Pfsubr_mm_mmm64"] ),
				( 0xAE, CodeEnum.Instance["D3NOW_Pfacc_mm_mmm64"] ),
				( 0xB0, CodeEnum.Instance["D3NOW_Pfcmpeq_mm_mmm64"] ),
				( 0xB4, CodeEnum.Instance["D3NOW_Pfmul_mm_mmm64"] ),
				( 0xB6, CodeEnum.Instance["D3NOW_Pfrcpit2_mm_mmm64"] ),
				( 0xB7, CodeEnum.Instance["D3NOW_Pmulhrw_mm_mmm64"] ),
				( 0xBB, CodeEnum.Instance["D3NOW_Pswapd_mm_mmm64"] ),
				( 0xBF, CodeEnum.Instance["D3NOW_Pavgusb_mm_mmm64"] ),
			};

			foreach (var generator in generators)
				generator.Generate(infos);
		}
	}
}
