// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

#if ENCODER && BLOCK_ENCODER
namespace Iced.Intel.BlockEncoderInternal {
	sealed class CodeWriterImpl : CodeWriter {
		public uint BytesWritten;
		readonly CodeWriter codeWriter;

		public CodeWriterImpl(CodeWriter codeWriter) {
			if (codeWriter is null)
				ThrowHelper.ThrowArgumentNullException_codeWriter();
			this.codeWriter = codeWriter;
		}

		public override void WriteByte(byte value) {
			BytesWritten++;
			codeWriter.WriteByte(value);
		}
	}
}
#endif
