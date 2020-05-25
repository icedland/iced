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
		readonly InstructionList _instructions;
		ulong _currentLabelId;
		Label _label;
		Label _currentAnonLabel;
		Label _nextAnonLabel;
		bool _definedAnonLabel;
		PrefixFlags _nextPrefixFlags;

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
				throw new ArgumentOutOfRangeException("Only 16, 32 or 64 bitness are supported", nameof(bitness));
			}
			Bitness = bitness;
			_instructions = new InstructionList();
			_label = default;
			_currentAnonLabel = default;
			_nextAnonLabel = default;
			_definedAnonLabel = false;
			PreferVex = true;
			PreferBranchShort = true;
		}

		/// <summary>
		/// Gets the bitness defined for this assembler.
		/// </summary>
		public int Bitness { get; }

		/// <summary>
		/// <c>true</c> to prefer VEX encoding over EVEX. This is the default. 
		/// </summary>
		public bool PreferVex { get; set; }

		/// <summary>
		/// <c>true</c> to prefer short branch encoding. This is the default. 
		/// </summary>
		public bool PreferBranchShort { get; set; }

		/// <summary>
		/// Gets the instructions.
		/// </summary>
		public IReadOnlyList<Instruction> Instructions => _instructions;

		/// <summary>
		/// Reset the current set of instructions and labels added to this instance. 
		/// </summary>
		public void Reset() {
			_instructions.Clear();
			_currentLabelId = 0;
			_label = default;
			_currentAnonLabel = default;
			_nextAnonLabel = default;
			_definedAnonLabel = false;
			_nextPrefixFlags = PrefixFlags.None;
		}

		/// <summary>
		/// Creates a label.
		/// </summary>
		/// <param name="name">Optional name of the label.</param>
		/// <returns></returns>
		public Label CreateLabel(string? name = null) {
			_currentLabelId++;
			var label = new Label(name, _currentLabelId);
			return label;
		}

		/// <summary>
		/// Gets the current label used by this instance.
		/// </summary>
		public Label CurrentLabel => _label;

		/// <summary>
		/// Use the specified label.
		/// </summary>
		/// <param name="label">Label to use</param>
		/// <exception cref="ArgumentException"></exception>
		public void Label(ref Label label) {
			if (label.IsEmpty) throw new ArgumentException($"Invalid label. Must be created via {nameof(CreateLabel)}", nameof(label));
			if (label.InstructionIndex >= 0) throw new ArgumentException($"Cannot reuse label. The specified label is already associated with an instruction at index {label.InstructionIndex}.", nameof(label));
			if (!_label.IsEmpty) throw new ArgumentException("At most one label per instruction is allowed");
			label.InstructionIndex = _instructions.Count;
			_label = label;
		}

		/// <summary>
		/// Creates an anonymous label that can be referenced by using <see cref="B"/> (backward anonymous label)
		/// and <see cref="F"/> (forward anonymous label).
		/// </summary>
		public void AnonymousLabel() {
			if (_definedAnonLabel)
				throw new InvalidOperationException("At most one anonymous label per instruction is allowed");
			if (_nextAnonLabel.IsEmpty)
				_currentAnonLabel = CreateLabel();
			else
				_currentAnonLabel = _nextAnonLabel;
			_nextAnonLabel = default;
			_definedAnonLabel = true;
		}

		/// <summary>
		/// References the previous anonymous label created by <see cref="AnonymousLabel"/>
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public Label @B {
			get {
				if (_currentAnonLabel.IsEmpty)
					throw new InvalidOperationException("No anonymous label has been created yet");
				return _currentAnonLabel;
			}
		}

		/// <summary>
		/// References the next anonymous label created by a future call to <see cref="AnonymousLabel"/>
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public Label @F {
			get {
				if (_nextAnonLabel.IsEmpty)
					_nextAnonLabel = CreateLabel();
				return _nextAnonLabel;
			}
		}

		/// <summary>
		/// Add an instruction directly to the flow of instructions.
		/// </summary>
		/// <param name="instruction"></param>
		public void AddInstruction(Instruction instruction) {
			AddInstruction(ref instruction);
		}

		/// <summary>
		/// Add an instruction directly to the flow of instructions.
		/// </summary>
		/// <param name="instruction"></param>
		public void AddInstruction(ref Instruction instruction) {
			if (!_label.IsEmpty && _definedAnonLabel)
				throw new InvalidOperationException("You can't create both an anonymous label and a normal label");
			if (!_label.IsEmpty)
				instruction.IP = _label.Id;
			else if (_definedAnonLabel)
				instruction.IP = _currentAnonLabel.Id;

			// Setup prefixes
			if (_nextPrefixFlags != PrefixFlags.None) {
				if ((_nextPrefixFlags & PrefixFlags.Lock) != 0) {
					instruction.HasLockPrefix = true;
				}
				if ((_nextPrefixFlags & PrefixFlags.Xacquire) != 0) {
					instruction.HasXacquirePrefix = true;
				}
				if ((_nextPrefixFlags & PrefixFlags.Xrelease) != 0) {
					instruction.HasXreleasePrefix = true;
				}
				if ((_nextPrefixFlags & PrefixFlags.Rep) != 0) {
					instruction.HasRepPrefix = true;
				}
				else if ((_nextPrefixFlags & PrefixFlags.Repe) != 0) {
					instruction.HasRepePrefix = true;
				}
				else if ((_nextPrefixFlags & PrefixFlags.Repne) != 0) {
					instruction.HasRepnePrefix = true;
				}
				if ((_nextPrefixFlags & PrefixFlags.Bnd) != 0) {
					instruction.HasRepnePrefix = true;
				}
				if ((_nextPrefixFlags & PrefixFlags.Notrack) != 0) {
					instruction.SegmentPrefix = Register.DS;
				}
			}

			_instructions.Add(instruction);
			_label = default;
			_definedAnonLabel = false;
			_nextPrefixFlags = PrefixFlags.None;
		}

		/// <summary>
		/// Add an instruction directly to the flow of instructions.
		/// </summary>
		/// <param name="instruction"></param>
		/// <param name="flags">Operand flags passed.</param>
		void AddInstruction(Instruction instruction, AssemblerOperandFlags flags = AssemblerOperandFlags.None) {
			if (flags != AssemblerOperandFlags.None) {
				if ((flags & AssemblerOperandFlags.Broadcast) != 0) {
					instruction.IsBroadcast = true;
				}
				if ((flags & AssemblerOperandFlags.Zeroing) != 0) {
					instruction.ZeroingMasking = true;
				}
				if ((flags & AssemblerOperandFlags.RegisterMask) != 0) {
					// register mask is shift by 2 (starts at index 1 for K1)
					instruction.OpMask = (Register)((int)Register.K0 + (((int)(flags & AssemblerOperandFlags.RegisterMask)) >> 6));
				}
				if ((flags & AssemblerOperandFlags.SuppressAllExceptions) != 0) {
					instruction.SuppressAllExceptions = true;
				}
				if ((flags & AssemblerOperandFlags.RoundControlMask) != 0) {
					instruction.RoundingControl = (RoundingControl)((((int)(flags & AssemblerOperandFlags.RoundControlMask)) >> 3));
				}
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
				_nextPrefixFlags |= PrefixFlags.Lock;
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
				_nextPrefixFlags |= PrefixFlags.Xacquire;
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
				_nextPrefixFlags |= PrefixFlags.Xrelease;
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
				_nextPrefixFlags |= PrefixFlags.Rep;
				return this;
			}
		}

		/// <summary>
		/// Add repe prefix before the next instruction.
		/// </summary>
		/// <returns></returns>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public Assembler repe {
			get {
				_nextPrefixFlags |= PrefixFlags.Repe;
				return this;
			}
		}

		/// <summary>
		/// Add repne prefix before the next instruction.
		/// </summary>
		/// <returns></returns>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public Assembler repne {
			get {
				_nextPrefixFlags |= PrefixFlags.Repne;
				return this;
			}
		}

		/// <summary>
		/// Add bnd prefix before the next instruction.
		/// </summary>
		/// <returns></returns>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public Assembler bnd {
			get {
				_nextPrefixFlags |= PrefixFlags.Bnd;
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
				_nextPrefixFlags |= PrefixFlags.Notrack;
				return this;
			}
		}

		/// <summary>call selector:offset instruction.</summary>
		public void call(ushort selector, uint offset) {
			AddInstruction(Instruction.CreateBranch(Bitness >= 32 ? Code.Call_ptr1632 : Code.Call_ptr1616, selector, offset));
		}

		/// <summary>jmp selector:offset instruction.</summary>
		public void jmp(ushort selector, uint offset) {
			AddInstruction(Instruction.CreateBranch(Bitness >= 32 ? Code.Jmp_ptr1632 : Code.Jmp_ptr1616, selector, offset));
		}

		/// <summary>xlatb instruction.</summary>
		public void xlatb() {
			if (Bitness == 64)
				AddInstruction(Instruction.Create(Code.Xlat_m8, new MemoryOperand(Register.RBX, Register.AL, 1)));
			else if (Bitness == 32)
				AddInstruction(Instruction.Create(Code.Xlat_m8, new MemoryOperand(Register.EBX, Register.AL, 1)));
			else
				AddInstruction(Instruction.Create(Code.Xlat_m8, new MemoryOperand(Register.BX, Register.AL, 1)));
		}

		/// <summary>
		/// Generates multibyte NOP instructions
		/// </summary>
		/// <param name="amount">Number of bytes</param>
		public void nop(int amount) {
			if (amount < 0)
				throw new ArgumentOutOfRangeException(nameof(amount));
			if (amount == 0) return;

			const int maxMultibyteNopInstructionLength = 9;

			int cycles = Math.DivRem(amount, maxMultibyteNopInstructionLength, out int rest);

			for (int i = 0; i < cycles; i++) {
				AppendNop(maxMultibyteNopInstructionLength);
			}
			if (rest > 0)
				AppendNop(rest);

			void AppendNop(int amount) {
				switch (amount) {
				case 1:
					db(0x90); //NOP
					break;
				case 2:
					db(0x66, 0x90); //66 NOP
					break;
				case 3:
					db(0x0F, 0x1F, 0x00); //NOP dword ptr [eax] or NOP word ptr [bx+si]
					break;
				case 4:
					db(0x0F, 0x1F, 0x40, 0x00); //NOP dword ptr [eax + 00] or NOP word ptr [bx+si]
					break;
				case 5:
					if (Bitness >= 32)
						db(0x0F, 0x1F, 0x44, 0x00, 0x00); //NOP dword ptr [eax + eax*1 + 00]
					else
						db(0x0F, 0x1F, 0x80, 0x00, 0x00); //NOP word ptr[bx + si]
					break;
				case 6:
					if (Bitness >= 32)
						db(0x66, 0x0F, 0x1F, 0x44, 0x00, 0x00); //66 NOP dword ptr [eax + eax*1 + 00]
					else
						db(0x66, 0x0F, 0x1F, 0x80, 0x00, 0x00); //NOP dword ptr [bx+si]
					break;
				case 7:
					if (Bitness >= 32)
						db(0x0F, 0x1F, 0x80, 0x00, 0x00, 0x00, 0x00); //NOP dword ptr [eax + 00000000]
					else
						db(0x67, 0x66, 0x0F, 0x1F, 0x44, 0x00, 0x00); //NOP dword ptr [eax+eax]
					break;
				case 8:
					if (Bitness >= 32)
						db(0x0F, 0x1F, 0x84, 0x00, 0x00, 0x00, 0x00, 0x00); //NOP dword ptr [eax + eax*1 + 00000000]
					else
						db(0x67, 0x0F, 0x1F, 0x80, 0x00, 0x00, 0x00, 0x00); //NOP word ptr [eax]
					break;
				case 9:
					if (Bitness >= 32)
						db(0x66, 0x0F, 0x1F, 0x84, 0x00, 0x00, 0x00, 0x00, 0x00); //66 NOP dword ptr [eax + eax*1 + 00000000] 	
					else
						db(0x67, 0x0F, 0x1F, 0x84, 0x00, 0x00, 0x00, 0x00, 0x00); //NOP word ptr [eax+eax]	
					break;

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
			if (writer is null)
				ThrowHelper.ThrowArgumentNullException_writer();

			if (!TryAssemble(writer, rip, out var errorMessage, out var assemblerResult, options)) {
				throw new InvalidOperationException(errorMessage);
			}
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
			if (_nextPrefixFlags != PrefixFlags.None) {
				errorMessage = $"Unused prefixes {_nextPrefixFlags}. You must emit an instruction after using an instruction prefix.";
				return false;
			}

			// Protect against a label emitted without being attached to an instruction
			if (!_label.IsEmpty) {
				errorMessage = $"Unused label {_label}. You must emit an instruction after emitting a label.";
				return false;
			}

			if (_definedAnonLabel) {
				errorMessage = "Unused anonymous label. You must emit an instruction after emitting a label.";
				return false;
			}

			if (!_nextAnonLabel.IsEmpty) {
				errorMessage = "Found an @F anonymous label reference but there was no call to " + nameof(AnonymousLabel);
				return false;
			}

			var blocks = new[] { new InstructionBlock(writer, _instructions, rip) };
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
			Xacquire = 1 << 0,
			Xrelease = 1 << 1,
			Lock = 1 << 2,
			Rep = 1 << 3,
			Repe = 1 << 4,
			Repne = 1 << 5,
			Bnd = 1 << 6,
			Notrack = 1 << 7,
		}
	}
}
#endif
