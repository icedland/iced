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
		readonly List<Instruction> _instructions;

		private ExtendedEncoder(CodeWriter writer, int bitness) {
			Debug.Assert(bitness == 16 || bitness == 32 || bitness == 64);
			if (writer is null)
				ThrowHelper.ThrowArgumentNullException_writer();
			Bitness = bitness;
			_writer = writer;
			_labels = new List<Label>();
			_instructions = new List<Instruction>();
			_label = CreateLabel();
			PreferVex = true;
			PreferBranchNear = true;
		}
		
		public ulong BaseRip { get; set; }

		public int Bitness { get; }
		
		public bool PreferVex { get; set; }
		
		public bool PreferBranchNear { get; set; }

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
			_internalRip++;
			var label = new Label(name, _internalRip);
			_labels.Add(label);
			return label;
		}

		public Label CurrentLabel => _label;

		public void label(Label label) {
			if (label.IsEmpty) throw new ArgumentException($"Invalid label. Must be created via {nameof(CreateLabel)}");
			_label = label;
		}

		public void AddInstruction(Instruction instruction) {
			instruction.IP = _label.RIP;
			_instructions.Add(instruction);
			_label = default;
		}

		public BlockEncoderResult Encode(BlockEncoderOptions options = BlockEncoderOptions.None) {
			if (!TryEncode(out var errorMessage, out var blockResult, options)) {
				throw new InvalidOperationException(errorMessage);
			}
			return blockResult;
		}

		public bool TryEncode(out string? errorMessage, out BlockEncoderResult blockResult, BlockEncoderOptions options = BlockEncoderOptions.None) {
			var blocks = new InstructionBlock[1];
			var block = new InstructionBlock(this._writer, _instructions, BaseRip);
			blocks[0] = block;

			blockResult = default;
			var result = BlockEncoder.TryEncode(Bitness, blocks, out errorMessage, out var blockResults, options);
			if (result) {
				blockResult = blockResults[0];
			}

			return result;
		}

		InvalidOperationException NoOpCodeFoundFor(Mnemonic mnemonic, params object[] argNames) {
			var builder = new StringBuilder();
			builder.Append($"Unable to calculate an OpCode for `{mnemonic.ToString().ToLowerInvariant()}");
			for (int i = 0; i < argNames.Length; i++) {
				builder.Append(i == 0 ? " " : ", ");
				builder.Append(argNames[i]); // TODO: add pretty print for arguments (registers, memory...)
			}

			builder.Append($"`. Combination of arguments and/or current bitness {Bitness} is not compatible with any existing OpCode encoding.");
			return new InvalidOperationException(builder.ToString());
		}
	}
}
