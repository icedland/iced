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

using Generator.IO;

namespace Generator.Decoder.Rust {
	sealed class RustDecoderTableSerializer : DecoderTableSerializer {
		public string TableName { get; }

		public RustDecoderTableSerializer(string tableName, DecoderTableSerializerInfo info) : base(RustIdentifierConverter.Create(), info) {
			TableName = tableName;
		}

		public void Serialize(FileWriter writer) {
			writer.WriteFileHeader();
			writer.WriteLine(RustConstants.AttributeNoRustFmt);
			writer.WriteLine($"pub(super) static TBL_DATA: &[u8] = &[");
			using (writer.Indent())
				SerializeCore(writer);
			writer.WriteLine("];");

			foreach (var name in info.TableIndexNames) {
				var constName = idConverter.Constant($"{name}Index");
				writer.WriteLine($"pub(super) const {constName}: usize = {GetInfo(name).Index};");
			}
		}
	}
}
