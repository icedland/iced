using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Iced.Intel.BlockEncoderInternal;

namespace Iced.Intel {
	public sealed partial class ExtendedEncoder {

		readonly CodeWriter _writer;
		ulong _internalRip;
		Label _label;
		readonly List<Label> _labels;

		private ExtendedEncoder(CodeWriter writer, int bitness) {
			Debug.Assert(bitness == 16 || bitness == 32 || bitness == 64);
			if (writer is null)
				ThrowHelper.ThrowArgumentNullException_writer();
			Bitness = bitness;
			_writer = writer;
			_labels = new List<Label>();
			_label = CreateLabel();
			PreferVex = true;
		}

		public int Bitness { get; }
		
		public bool PreferVex { get; set; }

		public static ExtendedEncoder Create(int bitness, CodeWriter writer) {
			if (writer == null) throw new ArgumentNullException(nameof(writer));
			switch (bitness) {
			case 16:
			case 32:
			case 64:
				return new ExtendedEncoder(writer, bitness);
			default:
				throw new ArgumentOutOfRangeException(nameof(bitness));
			}
		}

		public Label CreateLabel(string name = null) {
			var label = new Label(name, new InstructionBlock(_writer, new List<Instruction>(), _internalRip));
			_internalRip++;
			_labels.Add(label);
			return label;
		}

		public Label CurrentLabel => _label;

		public void label(Label label) {
			if (label.IsEmpty) throw new ArgumentException($"Invalid label. Must be created via {nameof(CreateLabel)}");
			_label = label;
		}

		public void AddInstruction(Instruction instruction) {
			_label.Block.Instructions.Add(instruction);
		}

		public BlockEncoderResult[] Encode(BlockEncoderOptions options = BlockEncoderOptions.None) {
			if (!TryEncode(out var errorMessage, out var blockResults, options)) {
				throw new InvalidOperationException(errorMessage);
			}

			return blockResults;
		}

		public bool TryEncode(out string? errorMessage, out BlockEncoderResult[]? blockResults, BlockEncoderOptions options = BlockEncoderOptions.None) {
			var blocks = new InstructionBlock[_labels.Count];
			for(int i = 0; i < _labels.Count; i++) {
				blocks[i] = _labels[i].Block;
			}
			return BlockEncoder.TryEncode(Bitness, blocks, out errorMessage, out blockResults, options);
		}

		InvalidOperationException NoOpCodeFoundFor(string name, params object[] argNames) {
			var builder = new StringBuilder();
			builder.Append($"Unable to calculate an OpCode for `{name}");
			for (int i = 0; i < argNames.Length; i++) {
				builder.Append(i == 0 ? " " : ", ");
				builder.Append(argNames[i]); // TODO: add pretty print for arguments (registers, memory...)
			}

			builder.Append($"`. Combination of arguments and/or current bitness {Bitness} is not compatible with any existing OpCode encoding.");
			return new InvalidOperationException(builder.ToString());
		}
	}
}
