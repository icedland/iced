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

#if !NO_ENCODER
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Iced.Intel.BlockEncoderInternal;

namespace Iced.Intel {
	/// <summary>
	/// Relocation kind
	/// </summary>
	public enum RelocKind {
		/// <summary>
		/// 64-bit offset. Only used if it's 64-bit code.
		/// </summary>
		Offset64,
	}

	/// <summary>
	/// Relocation info
	/// </summary>
	public readonly struct RelocInfo {
		/// <summary>
		/// Address
		/// </summary>
		public readonly ulong Address;

		/// <summary>
		/// Relocation kind
		/// </summary>
		public readonly RelocKind Kind;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="kind">Relocation kind</param>
		/// <param name="address">Address</param>
		public RelocInfo(RelocKind kind, ulong address) {
			Kind = kind;
			Address = address;
		}
	}

	/// <summary>
	/// Contains a list of instructions and a base IP
	/// </summary>
	public readonly struct InstructionBlock {
		/// <summary>
		/// Code writer
		/// </summary>
		public readonly CodeWriter CodeWriter;

		/// <summary>
		/// All instructions
		/// </summary>
		public readonly IList<Instruction> Instructions;

		/// <summary>
		/// Base IP of all encoded instructions
		/// </summary>
		public readonly ulong RIP;

		/// <summary>
		/// List that gets all reloc infos or null
		/// </summary>
		public readonly IList<RelocInfo> RelocInfos;

		/// <summary>
		/// Offsets of the new instructions relative to the base IP. If the instruction was rewritten to
		/// a completely different instruction, the value <see cref="uint.MaxValue"/> is stored in that array element.
		/// This array can be null.
		/// </summary>
		public readonly uint[] NewInstructionOffsets;

		/// <summary>
		/// Offsets of all constants in the new encoded instructions. If the instruction was rewritten,
		/// the 'default' value is stored in the corresponding array element. This array can be null.
		/// </summary>
		public readonly ConstantOffsets[] ConstantOffsets;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="codeWriter">Code writer</param>
		/// <param name="instructions">Instructions</param>
		/// <param name="rip">Base IP of all encoded instructions</param>
		/// <param name="relocInfos">List that gets all reloc infos or null</param>
		/// <param name="newInstructionOffsets">Offsets of the new instructions relative to the base IP. If the instruction was rewritten to
		/// a completely different instruction, the value <see cref="uint.MaxValue"/> is stored in that array element.
		/// This array can be null.</param>
		/// <param name="constantOffsets">Offsets of all constants in the new encoded instructions. If the instruction was rewritten,
		/// the 'default' value is stored in the corresponding array element. This array can be null.</param>
		public InstructionBlock(CodeWriter codeWriter, IList<Instruction> instructions, ulong rip, IList<RelocInfo> relocInfos = null, uint[] newInstructionOffsets = null, ConstantOffsets[] constantOffsets = null) {
			CodeWriter = codeWriter ?? throw new ArgumentNullException(nameof(codeWriter));
			Instructions = instructions ?? throw new ArgumentNullException(nameof(instructions));
			RIP = rip;
			RelocInfos = relocInfos;
			NewInstructionOffsets = newInstructionOffsets;
			ConstantOffsets = constantOffsets;
			if (newInstructionOffsets != null && newInstructionOffsets.Length != instructions.Count)
				throw new ArgumentException($"{nameof(newInstructionOffsets)} must have the same number of elements as {nameof(instructions)} or it must be null");
			if (constantOffsets != null && constantOffsets.Length != instructions.Count)
				throw new ArgumentException($"{nameof(constantOffsets)} must have the same number of elements as {nameof(instructions)} or it must be null");
		}
	}

	/// <summary>
	/// Encoder options
	/// </summary>
	[Flags]
	public enum BlockEncoderOptions : uint {
		/// <summary>
		/// No option is set
		/// </summary>
		None						= 0,

		/// <summary>
		/// By default, branches get updated if the target is too far away, eg. jcc short -> jcc near
		/// or if 64-bit mode, jcc + jmp [rip+mem]. If this option is enabled, no branches are fixed.
		/// </summary>
		DontFixBranches				= 0x00000001,
	}

	/// <summary>
	/// Encodes instructions
	/// </summary>
	public sealed class BlockEncoder {
		readonly int bitness;
		readonly BlockEncoderOptions options;
		readonly Block[] blocks;
		readonly Encoder nullEncoder;
		readonly Dictionary<ulong, Instr> toInstr;

		internal int Bitness => bitness;
		internal bool FixBranches => (options & BlockEncoderOptions.DontFixBranches) == 0;
		internal Encoder NullEncoder => nullEncoder;

		sealed class NullCodeWriter : CodeWriter {
			public static readonly NullCodeWriter Instance = new NullCodeWriter();
			NullCodeWriter() { }
			public override void WriteByte(byte value) { }
		}

		BlockEncoder(int bitness, InstructionBlock[] instrBlocks, BlockEncoderOptions options) {
			if (bitness != 16 && bitness != 32 && bitness != 64)
				throw new ArgumentOutOfRangeException(nameof(bitness));
			if (instrBlocks == null)
				throw new ArgumentNullException(nameof(instrBlocks));
			this.bitness = bitness;
			nullEncoder = Encoder.Create(bitness, NullCodeWriter.Instance);
			this.options = options;

			blocks = new Block[instrBlocks.Length];
			int instrCount = 0;
			for (int i = 0; i < instrBlocks.Length; i++) {
				var instructions = instrBlocks[i].Instructions;
				if (instructions == null)
					throw new ArgumentException();
				var instrs = new Instr[instructions.Count];
				ulong ip = instrBlocks[i].RIP;
				for (int j = 0; j < instrs.Length; j++) {
					var instruction = instructions[j];
					var instr = Instr.Create(this, ref instruction);
					instr.IP = ip;
					instrs[j] = instr;
					instrCount++;
					ip += instr.Size;
				}
				blocks[i] = new Block(this, instrBlocks[i].CodeWriter, instrBlocks[i].RIP, instrBlocks[i].RelocInfos, instrBlocks[i].NewInstructionOffsets, instrBlocks[i].ConstantOffsets, instrs);
			}
			// Optimize from low to high addresses
			Array.Sort(blocks, (a, b) => a.RIP.CompareTo(b.RIP));

			// There must not be any instructions with the same IP, except if IP = 0 (default value)
			var toInstr = new Dictionary<ulong, Instr>(instrCount);
			this.toInstr = toInstr;
			bool hasMultipleZeroIPInstrs = false;
			foreach (var block in blocks) {
				foreach (var instr in block.Instructions) {
					ulong origIP = instr.OrigIP;
					if (toInstr.TryGetValue(origIP, out var origInstr)) {
						if (origIP != 0)
							throw new ArgumentException($"Multiple instructions with the same IP: 0x{origIP:X}");
						hasMultipleZeroIPInstrs = true;
					}
					else
						toInstr[origIP] = instr;
				}
			}
			if (hasMultipleZeroIPInstrs)
				toInstr.Remove(0);

			foreach (var block in blocks) {
				ulong ip = block.RIP;
				foreach (var instr in block.Instructions) {
					instr.IP = ip;
					var oldSize = instr.Size;
					instr.Initialize();
					if (instr.Size > oldSize)
						throw new InvalidOperationException();
					ip += instr.Size;
				}
			}
		}

		/// <summary>
		/// Encodes instructions. Returns null or an error message
		/// </summary>
		/// <param name="bitness">16, 32 or 64</param>
		/// <param name="block">All instructions</param>
		/// <param name="errorMessage">Updated with an error message if the method failed</param>
		/// <param name="options">Encoder options</param>
		/// <returns></returns>
		public static bool TryEncode(int bitness, InstructionBlock block, out string errorMessage, BlockEncoderOptions options = BlockEncoderOptions.None) =>
			TryEncode(bitness, new[] { block }, out errorMessage, options);

		/// <summary>
		/// Encodes instructions. Returns null or an error message
		/// </summary>
		/// <param name="bitness">16, 32 or 64</param>
		/// <param name="blocks">All instructions</param>
		/// <param name="errorMessage">Updated with an error message if the method failed</param>
		/// <param name="options">Encoder options</param>
		/// <returns></returns>
		public static bool TryEncode(int bitness, InstructionBlock[] blocks, out string errorMessage, BlockEncoderOptions options = BlockEncoderOptions.None) =>
			new BlockEncoder(bitness, blocks, options).Encode(out errorMessage);

		bool Encode(out string errorMessage) {
			const int MAX_ITERS = 1000;
			for (int iter = 0; iter < MAX_ITERS; iter++) {
				bool updated = false;
				foreach (var block in blocks) {
					ulong ip = block.RIP;
					foreach (var instr in block.Instructions) {
						instr.IP = ip;
						var oldSize = instr.Size;
						if (instr.Optimize()) {
							if (instr.Size > oldSize) {
								errorMessage = "Internal error: new size > old size";
								return false;
							}
							if (instr.Size < oldSize)
								updated = true;
						}
						else if (instr.Size != oldSize) {
							errorMessage = "Internal error: new size != old size";
							return false;
						}
						ip += instr.Size;
					}
				}
				if (!updated)
					break;
			}

			foreach (var block in blocks)
				block.InitializeData();

			foreach (var block in blocks) {
				var encoder = Encoder.Create(bitness, block.CodeWriter);
				ulong ip = block.RIP;
				var newInstructionOffsets = block.NewInstructionOffsets;
				var constantOffsets = block.ConstantOffsets;
				var instructions = block.Instructions;
				for (int i = 0; i < instructions.Length; i++) {
					var instr = instructions[i];
					uint bytesWritten = block.CodeWriter.BytesWritten;
					bool isOriginalInstruction;
					if (constantOffsets != null)
						errorMessage = instr.TryEncode(encoder, out constantOffsets[i], out isOriginalInstruction);
					else
						errorMessage = instr.TryEncode(encoder, out _, out isOriginalInstruction);
					if (errorMessage != null)
						return false;
					uint size = block.CodeWriter.BytesWritten - bytesWritten;
					if (size != instr.Size) {
						errorMessage = "Internal error: didn't write all bytes";
						return false;
					}
					if (newInstructionOffsets != null) {
						if (isOriginalInstruction)
							newInstructionOffsets[i] = (uint)(ip - block.RIP);
						else
							newInstructionOffsets[i] = uint.MaxValue;
					}
					ip += size;
				}
				block.WriteData();
			}

			errorMessage = null;
			return true;
		}

		internal TargetInstr GetTarget(ulong address) {
			if (toInstr.TryGetValue(address, out var instr))
				return new TargetInstr(instr);
			return new TargetInstr(address);
		}
	}
}
#endif
