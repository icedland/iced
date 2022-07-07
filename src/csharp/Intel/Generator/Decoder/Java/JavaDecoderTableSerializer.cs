// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using Generator.IO;

namespace Generator.Decoder.Java {
	sealed class JavaDecoderTableSerializer : DecoderTableSerializer {
		public string ClassName { get; }

		public JavaDecoderTableSerializer(GenTypes genTypes, string className, DecoderTableSerializerInfo info)
			: base(genTypes, JavaIdentifierConverter.Create(), info) => ClassName = className;

		public void Serialize(BinaryByteTableWriter binWriter, FileWriter srcWriter) {
			SerializeCore(binWriter);

			srcWriter.WriteLine($"static final int MAX_ID_NAMES = {info.TablesToSerialize.Length};");
			foreach (var name in info.TableIndexNames) {
				var constName = idConverter.Constant($"{name}Index");
				srcWriter.WriteLine($"static final int {constName} = {GetInfo(name).Index};");
			}
		}
	}
}
