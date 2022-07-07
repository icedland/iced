// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using Generator.IO;

namespace Generator.Decoder.Rust {
	sealed class RustDecoderTableSerializer : DecoderTableSerializer {
		public string TableName { get; }

		public RustDecoderTableSerializer(GenTypes genTypes, string tableName, DecoderTableSerializerInfo info)
			: base(genTypes, RustIdentifierConverter.Create(), info) => TableName = tableName;

		public void Serialize(FileWriter writer) {
			writer.WriteFileHeader();
			writer.WriteLine(RustConstants.AttributeNoRustFmt);
			writer.WriteLine($"pub(super) static TBL_DATA: &[u8] = &[");
			using (writer.Indent())
				SerializeCore(new TextFileByteTableWriter(writer));
			writer.WriteLine("];");

			writer.WriteLine($"pub(super) const MAX_ID_NAMES: usize = {info.TablesToSerialize.Length};");
			foreach (var name in info.TableIndexNames) {
				var constName = idConverter.Constant($"{name}Index");
				writer.WriteLine(RustConstants.AttributeAllowDeadCode);
				writer.WriteLine($"pub(super) const {constName}: usize = {GetInfo(name).Index};");
			}
		}
	}
}
