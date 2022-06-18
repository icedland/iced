// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if ENCODER && BLOCK_ENCODER && CODE_ASSEMBLER
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Iced.Intel {
	/// <summary>
	/// High-Level Assembler.
	/// </summary>
	public partial class Assembler {
		readonly InstructionList instructions;
		ulong currentLabelId;
		Label currentLabel;
		Label currentAnonLabel;
		Label nextAnonLabel;
		bool definedAnonLabel;
		PrefixFlags prefixFlags;

		/// <summary>
		/// Creates a new instance of this assembler
		/// </summary>
		/// <param name="bitness">The assembler instruction set bitness, either 16, 32 or 64 bit.</param>
		public Assembler(int bitness) {
			switch (bitness) {
			case 16:
			case 32:
			case 64:
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(bitness));
			}
			Bitness = bitness;
			instructions = new InstructionList();
			currentLabelId = 0;
			currentLabel = default;
			currentAnonLabel = default;
			nextAnonLabel = default;
			definedAnonLabel = false;
			prefixFlags = PrefixFlags.None;
			PreferVex = true;
			PreferShortBranch = true;
		}

		/// <summary>
		/// Gets the bitness defined for this assembler.
		/// </summary>
		public int Bitness { get; }

		/// <summary>
		/// <c>true</c> to prefer VEX encoding over EVEX. This is the default. See also <see cref="vex"/> and <see cref="evex"/>.
		/// </summary>
		public bool PreferVex { get; set; }

		/// <summary>
		/// <c>true</c> to prefer short branch encoding. This is the default.
		/// </summary>
		public bool PreferShortBranch { get; set; }

		internal bool InstructionPreferVex {
			get {
				if ((prefixFlags & (PrefixFlags.PreferVex | PrefixFlags.PreferEvex)) != 0)
					return (prefixFlags & PrefixFlags.PreferVex) != 0;
				return PreferVex;
			}
		}

		/// <summary>
		/// Gets the instructions.
		/// </summary>
		public IReadOnlyList<Instruction> Instructions => instructions;

		/// <summary>
		/// Reset the current set of instructions and labels added to this instance.
		/// </summary>
		public void Reset() {
			instructions.Clear();
			currentLabelId = 0;
			currentLabel = default;
			currentAnonLabel = default;
			nextAnonLabel = default;
			definedAnonLabel = false;
			prefixFlags = PrefixFlags.None;
		}

		/// <summary>
		/// Creates a label.
		/// </summary>
		/// <param name="name">Optional name of the label.</param>
		/// <returns></returns>
		public Label CreateLabel(string? name = null) {
			currentLabelId++;
			var label = new Label(name, currentLabelId);
			return label;
		}

		/// <summary>
		/// Gets the current label used by this instance.
		/// </summary>
		public Label CurrentLabel => currentLabel;

		/// <summary>
		/// Use the specified label.
		/// </summary>
		/// <param name="label">Label to use</param>
		/// <exception cref="ArgumentException"></exception>
		public void Label(ref Label label) {
			if (label.IsEmpty)
				throw new ArgumentException($"Invalid label. Must be created via {nameof(CreateLabel)}", nameof(label));
			if (label.InstructionIndex >= 0)
				throw new ArgumentException($"Cannot reuse label. The specified label is already associated with an instruction at index {label.InstructionIndex}.", nameof(label));
			if (!currentLabel.IsEmpty)
				throw new ArgumentException("At most one label per instruction is allowed");
			label.InstructionIndex = instructions.Count;
			currentLabel = label;
		}

		/// <summary>
		/// Creates an anonymous label that can be referenced by using <see cref="B"/> (backward anonymous label)
		/// and <see cref="F"/> (forward anonymous label).
		/// </summary>
		public void AnonymousLabel() {
			if (definedAnonLabel)
				throw new InvalidOperationException("At most one anonymous label per instruction is allowed");
			if (nextAnonLabel.IsEmpty)
				currentAnonLabel = CreateLabel();
			else
				currentAnonLabel = nextAnonLabel;
			nextAnonLabel = default;
			definedAnonLabel = true;
		}

		/// <summary>
		/// References the previous anonymous label created by <see cref="AnonymousLabel"/>
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public Label @B {
			get {
				if (currentAnonLabel.IsEmpty)
					throw new InvalidOperationException("No anonymous label has been created yet");
				return currentAnonLabel;
			}
		}

		/// <summary>
		/// References the next anonymous label created by a future call to <see cref="AnonymousLabel"/>
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public Label @F {
			get {
				if (nextAnonLabel.IsEmpty)
					nextAnonLabel = CreateLabel();
				return nextAnonLabel;
			}
		}

		/// <summary>
		/// Add an instruction directly to the flow of instructions.
		/// </summary>
		/// <param name="instruction"></param>
		public void AddInstruction(Instruction instruction) =>
			AddInstruction(ref instruction);

		/// <summary>
		/// Add an instruction directly to the flow of instructions.
		/// </summary>
		/// <param name="instruction"></param>
		public void AddInstruction(ref Instruction instruction) {
			if (!currentLabel.IsEmpty && definedAnonLabel)
				throw new InvalidOperationException("You can't create both an anonymous label and a normal label");
			if (!currentLabel.IsEmpty)
				instruction.IP = currentLabel.Id;
			else if (definedAnonLabel)
				instruction.IP = currentAnonLabel.Id;

			// Setup prefixes
			if (prefixFlags != PrefixFlags.None) {
				if ((prefixFlags & PrefixFlags.Lock) != 0)
					instruction.HasLockPrefix = true;
				if ((prefixFlags & PrefixFlags.Repe) != 0)
					instruction.HasRepePrefix = true;
				else if ((prefixFlags & PrefixFlags.Repne) != 0)
					instruction.HasRepnePrefix = true;
				if ((prefixFlags & PrefixFlags.Notrack) != 0)
					instruction.SegmentPrefix = Register.DS;
			}

			instructions.Add(instruction);
			currentLabel = default;
			definedAnonLabel = false;
			prefixFlags = PrefixFlags.None;
		}

		/// <summary>
		/// Add an instruction directly to the flow of instructions.
		/// </summary>
		/// <param name="instruction"></param>
		/// <param name="flags">Operand flags passed.</param>
		void AddInstruction(Instruction instruction, AssemblerOperandFlags flags) {
			if (flags != AssemblerOperandFlags.None) {
				if ((flags & AssemblerOperandFlags.Broadcast) != 0)
					instruction.IsBroadcast = true;
				if ((flags & AssemblerOperandFlags.Zeroing) != 0)
					instruction.ZeroingMasking = true;
				if ((flags & AssemblerOperandFlags.RegisterMask) != 0) {
					// register mask is shift by 2 (starts at index 1 for K1)
					instruction.OpMask = (Register)((int)Register.K0 + (((int)(flags & AssemblerOperandFlags.RegisterMask)) >> 6));
				}
				if ((flags & AssemblerOperandFlags.SuppressAllExceptions) != 0)
					instruction.SuppressAllExceptions = true;
				if ((flags & AssemblerOperandFlags.RoundingControlMask) != 0)
					instruction.RoundingControl = (RoundingControl)((((int)(flags & AssemblerOperandFlags.RoundingControlMask)) >> 3));
			}
			AddInstruction(ref instruction);
		}

		/// <summary>
		/// Add lock prefix before the next instruction.
		/// </summary>
		/// <returns></returns>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public Assembler @lock {
			get {
				prefixFlags |= PrefixFlags.Lock;
				return this;
			}
		}

		/// <summary>
		/// Add xacquire prefix before the next instruction.
		/// </summary>
		/// <returns></returns>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public Assembler xacquire {
			get {
				prefixFlags |= PrefixFlags.Repne;
				return this;
			}
		}

		/// <summary>
		/// Add xrelease prefix before the next instruction.
		/// </summary>
		/// <returns></returns>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public Assembler xrelease {
			get {
				prefixFlags |= PrefixFlags.Repe;
				return this;
			}
		}

		/// <summary>
		/// Add rep prefix before the next instruction.
		/// </summary>
		/// <returns></returns>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public Assembler rep {
			get {
				prefixFlags |= PrefixFlags.Repe;
				return this;
			}
		}

		/// <summary>
		/// Add repe/repz prefix before the next instruction.
		/// </summary>
		/// <returns></returns>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public Assembler repe {
			get {
				prefixFlags |= PrefixFlags.Repe;
				return this;
			}
		}

		/// <summary>
		/// Add repe/repz prefix before the next instruction.
		/// </summary>
		/// <returns></returns>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public Assembler repz => repe;

		/// <summary>
		/// Add repne/repnz prefix before the next instruction.
		/// </summary>
		/// <returns></returns>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public Assembler repne {
			get {
				prefixFlags |= PrefixFlags.Repne;
				return this;
			}
		}

		/// <summary>
		/// Add repne/repnz prefix before the next instruction.
		/// </summary>
		/// <returns></returns>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public Assembler repnz => repne;

		/// <summary>
		/// Add bnd prefix before the next instruction.
		/// </summary>
		/// <returns></returns>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public Assembler bnd {
			get {
				prefixFlags |= PrefixFlags.Repne;
				return this;
			}
		}

		/// <summary>
		/// Add notrack prefix before the next instruction.
		/// </summary>
		/// <returns></returns>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public Assembler notrack {
			get {
				prefixFlags |= PrefixFlags.Notrack;
				return this;
			}
		}

		/// <summary>
		/// Prefer VEX encoding if the next instruction can be VEX and EVEX encoded
		/// </summary>
		/// <returns></returns>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public Assembler vex {
			get {
				prefixFlags |= PrefixFlags.PreferVex;
				return this;
			}
		}

		/// <summary>
		/// Prefer EVEX encoding if the next instruction can be VEX and EVEX encoded
		/// </summary>
		/// <returns></returns>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public Assembler evex {
			get {
				prefixFlags |= PrefixFlags.PreferEvex;
				return this;
			}
		}

		/// <summary>
		/// Adds data
		/// </summary>
		/// <param name="array">Data</param>
		public void db(byte[] array) {
			if (array is null)
				ThrowHelper.ThrowArgumentNullException_array();
			db(array, 0, array.Length);
		}

		/// <summary>
		/// Adds data
		/// </summary>
		/// <param name="array">Data</param>
		/// <param name="index">Start index</param>
		/// <param name="length">Length in bytes</param>
		public void db(byte[] array, int index, int length) {
			if (array is null)
				ThrowHelper.ThrowArgumentNullException_array();
			if (index < 0)
				ThrowHelper.ThrowArgumentOutOfRangeException_index();
			if (length < 0 || (uint)(index + length) > (uint)array.Length)
				ThrowHelper.ThrowArgumentOutOfRangeException_length();
			const int maxLength = 16;
			int cycles = Math.DivRem(length, maxLength, out int rest);
			int currentPosition = index;
			for (int i = 0; i < cycles; i++) {
				AddInstruction(Instruction.CreateDeclareByte(array, currentPosition, maxLength));
				currentPosition += maxLength;
			}
			if (rest > 0)
				AddInstruction(Instruction.CreateDeclareByte(array, currentPosition, rest));
		}

#if HAS_SPAN
		/// <summary>
		/// Adds data
		/// </summary>
		/// <param name="data">Data</param>
		public void db(ReadOnlySpan<byte> data) {
			const int maxLength = 16;
			int cycles = Math.DivRem(data.Length, maxLength, out int rest);
			int currentPosition = 0;
			for (int i = 0; i < cycles; i++) {
				AddInstruction(Instruction.CreateDeclareByte(data.Slice(currentPosition, maxLength)));
				currentPosition += maxLength;
			}
			if (rest > 0)
				AddInstruction(Instruction.CreateDeclareByte(data.Slice(currentPosition, rest)));
		}
#endif

		/// <summary>call selector:offset instruction.</summary>
		public void call(ushort selector, uint offset) =>
			AddInstruction(Instruction.CreateBranch(Bitness >= 32 ? Code.Call_ptr1632 : Code.Call_ptr1616, selector, offset));

		/// <summary>jmp selector:offset instruction.</summary>
		public void jmp(ushort selector, uint offset) =>
			AddInstruction(Instruction.CreateBranch(Bitness >= 32 ? Code.Jmp_ptr1632 : Code.Jmp_ptr1616, selector, offset));

		/// <summary>xlatb instruction.</summary>
		public void xlatb() {
			var baseReg = Bitness switch {
				64 => Register.RBX,
				32 => Register.EBX,
				_ => Register.BX,
			};
			AddInstruction(Instruction.Create(Code.Xlat_m8, new MemoryOperand(baseReg, Register.AL)));
		}

		/// <summary>
		/// Generates multibyte NOP instructions
		/// </summary>
		/// <param name="sizeInBytes">Size in bytes of all nops</param>
		public void nop(int sizeInBytes) {
			if (sizeInBytes < 0)
				throw new ArgumentOutOfRangeException(nameof(sizeInBytes));
			if (this.prefixFlags != PrefixFlags.None)
				throw new InvalidOperationException("No prefixes are allowed");
			if (sizeInBytes == 0)
				return;

			const int maxMultibyteNopInstructionLength = 9;

			int cycles = Math.DivRem(sizeInBytes, maxMultibyteNopInstructionLength, out int rest);

			for (int i = 0; i < cycles; i++)
				AppendNop(maxMultibyteNopInstructionLength);
			if (rest > 0)
				AppendNop(rest);

			void AppendNop(int amount) {
				switch (amount) {
				case 1:
					db(0x90); // NOP
					break;
				case 2:
					db(0x66, 0x90); // 66 NOP
					break;
				case 3:
					db(0x0F, 0x1F, 0x00); // NOP dword ptr [eax] or NOP word ptr [bx+si]
					break;
				case 4:
					db(0x0F, 0x1F, 0x40, 0x00); // NOP dword ptr [eax + 00] or NOP word ptr [bx+si]
					break;
				case 5:
					if (Bitness != 16)
						db(0x0F, 0x1F, 0x44, 0x00, 0x00); // NOP dword ptr [eax + eax*1 + 00]
					else
						db(0x0F, 0x1F, 0x80, 0x00, 0x00); // NOP word ptr[bx + si]
					break;
				case 6:
					if (Bitness != 16)
						db(0x66, 0x0F, 0x1F, 0x44, 0x00, 0x00); // 66 NOP dword ptr [eax + eax*1 + 00]
					else
						db(0x66, 0x0F, 0x1F, 0x80, 0x00, 0x00); // NOP dword ptr [bx+si]
					break;
				case 7:
					if (Bitness != 16)
						db(0x0F, 0x1F, 0x80, 0x00, 0x00, 0x00, 0x00); // NOP dword ptr [eax + 00000000]
					else
						db(0x67, 0x66, 0x0F, 0x1F, 0x44, 0x00, 0x00); // NOP dword ptr [eax+eax]
					break;
				case 8:
					if (Bitness != 16)
						db(0x0F, 0x1F, 0x84, 0x00, 0x00, 0x00, 0x00, 0x00); // NOP dword ptr [eax + eax*1 + 00000000]
					else
						db(0x67, 0x0F, 0x1F, 0x80, 0x00, 0x00, 0x00, 0x00); // NOP word ptr [eax]
					break;
				case 9:
					if (Bitness != 16)
						db(0x66, 0x0F, 0x1F, 0x84, 0x00, 0x00, 0x00, 0x00, 0x00); // 66 NOP dword ptr [eax + eax*1 + 00000000]
					else
						db(0x67, 0x0F, 0x1F, 0x84, 0x00, 0x00, 0x00, 0x00, 0x00); // NOP word ptr [eax+eax]
					break;
				default:
					throw new InvalidOperationException();
				}
			}
		}

		/// <summary>
		/// Assembles the instructions of this assembler with the specified options.
		/// </summary>
		/// <param name="writer">The code writer.</param>
		/// <param name="rip">Base address.</param>
		/// <param name="options">Encoding options.</param>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException"></exception>
		public AssemblerResult Assemble(CodeWriter writer, ulong rip, BlockEncoderOptions options = BlockEncoderOptions.None) {
			if (!TryAssemble(writer, rip, out var errorMessage, out var assemblerResult, options))
				throw new InvalidOperationException(errorMessage);
			return assemblerResult;
		}

		/// <summary>
		/// Tries to assemble the instructions of this assembler with the specified options.
		/// </summary>
		/// <param name="writer">The code writer.</param>
		/// <param name="rip">Base address.</param>
		/// <param name="errorMessage">Error messages.</param>
		/// <param name="assemblerResult">The assembler result if successful.</param>
		/// <param name="options">Encoding options.</param>
		/// <returns><c>true</c> if the encoding was successful; <c>false</c> otherwise.</returns>
		public bool TryAssemble(CodeWriter writer, ulong rip, [NotNullWhen(false)] out string? errorMessage, out AssemblerResult assemblerResult, BlockEncoderOptions options = BlockEncoderOptions.None) {
			if (writer is null)
				ThrowHelper.ThrowArgumentNullException_writer();

			assemblerResult = default;

			// Protect against using a prefix without actually using it
			if (prefixFlags != PrefixFlags.None) {
				errorMessage = $"Unused prefixes {prefixFlags}. You must emit an instruction after using an instruction prefix.";
				return false;
			}

			// Protect against a label emitted without being attached to an instruction
			if (!currentLabel.IsEmpty) {
				errorMessage = $"Unused label {currentLabel}. You must emit an instruction after emitting a label.";
				return false;
			}

			if (definedAnonLabel) {
				errorMessage = "Unused anonymous label. You must emit an instruction after emitting a label.";
				return false;
			}

			if (!nextAnonLabel.IsEmpty) {
				errorMessage = "Found an @F anonymous label reference but there was no call to " + nameof(AnonymousLabel);
				return false;
			}

			var blocks = new[] { new InstructionBlock(writer, instructions, rip) };
			if (BlockEncoder.TryEncode(Bitness, blocks, out errorMessage, out var blockResults, options)) {
				assemblerResult = new AssemblerResult(blockResults);
				return true;
			}
			else {
				assemblerResult = new AssemblerResult(Array2.Empty<BlockEncoderResult>());
				return false;
			}
		}

		/// <summary>
		/// Internal method used to throw an InvalidOperationException if it was not possible to encode an OpCode.
		/// </summary>
		/// <param name="mnemonic">The mnemonic of the instruction</param>
		/// <param name="argNames">The argument values.</param>
		/// <returns></returns>
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

		[Flags]
		enum PrefixFlags {
			None = 0,
			Lock = 0x01,
			Repe = 0x02,
			Repne = 0x04,
			Notrack = 0x08,
			PreferVex = 0x10,
			PreferEvex = 0x20,
		}
	}
}
#endif
