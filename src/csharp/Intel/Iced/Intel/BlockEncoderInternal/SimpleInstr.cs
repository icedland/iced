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

#if ENCODER && BLOCK_ENCODER
namespace Iced.Intel.BlockEncoderInternal {
	/// <summary>
	/// Simple instruction that doesn't need fixing, i.e., it's not IP relative (no branch instruction, no IP relative memory operand)
	/// </summary>
	sealed class SimpleInstr : Instr {
		Instruction instruction;

		public SimpleInstr(BlockEncoder blockEncoder, Block block, in Instruction instruction)
			: base(block, instruction.IP) {
			this.instruction = instruction;
			Size = blockEncoder.GetInstructionSize(instruction, instruction.IP);
		}

		public override void Initialize(BlockEncoder blockEncoder) { }
		public override bool Optimize() => false;

		public override string? TryEncode(Encoder encoder, out ConstantOffsets constantOffsets, out bool isOriginalInstruction) {
			isOriginalInstruction = true;
			if (!encoder.TryEncode(instruction, IP, out _, out var errorMessage)) {
				constantOffsets = default;
				return CreateErrorMessage(errorMessage, instruction);
			}
			constantOffsets = encoder.GetConstantOffsets();
			return null;
		}
	}
}
#endif
