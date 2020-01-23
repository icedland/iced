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
#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Iced.Intel.BlockEncoderInternal;
namespace Iced.Intel {
	/// <summary>
	/// High-Level Assembler.
	/// </summary>
	public sealed partial class Assembler {

		readonly CodeWriter _writer;
		readonly List<Instruction> _instructions;
		ulong _currentLabelId;
		Label _label;
		PrefixFlags _nextPrefixFlags;

		/// <summary>
		/// Creates a new instance of this assembler 
		/// </summary>
		/// <param name="writer">CodeWriter</param>
		/// <param name="bitness">Bitness</param>
		Assembler(CodeWriter writer, int bitness) {
			Debug.Assert(bitness == 16 || bitness == 32 || bitness == 64);
			if (writer is null)
				ThrowHelper.ThrowArgumentNullException_writer();
			Bitness = bitness;
			_writer = writer;
			_instructions = new List<Instruction>();
			_label = CreateLabel();
			PreferVex = true;
			PreferBranchShort = true;
		}
		
		/// <summary>
		/// Base RIP used when encoding.
		/// </summary>
		public ulong BaseRip { get; set; }

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
			_nextPrefixFlags = PrefixFlags.None;
		}

		/// <summary>
		/// Creates a new <see cref="Assembler"/>.
		/// </summary>
		/// <param name="bitness">Bitness of the assembler</param>
		/// <param name="writer">Code writer.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public static Assembler Create(int bitness, CodeWriter writer) {
			if (writer == null) throw new ArgumentNullException(nameof(writer));
			switch (bitness) {
			case 16:
			case 32:
			case 64:
				return new Assembler(writer, bitness);
			default:
				throw new ArgumentOutOfRangeException(nameof(bitness));
			}
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
		public void Label(Label label) {
			if (label.IsEmpty) throw new ArgumentException($"Invalid label. Must be created via {nameof(CreateLabel)}");
			_label = label;
		}

		/// <summary>
		/// Add an instruction directly to the flow of instructions.
		/// </summary>
		/// <param name="instruction"></param>
		/// <param name="flags">Operand flags passed.</param>
		public void AddInstruction(Instruction instruction, AssemblerOperandFlags flags = AssemblerOperandFlags.None) {
			instruction.IP = _label.Id;
			
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
			_instructions.Add(instruction);
			_label = default;
			_nextPrefixFlags = PrefixFlags.None;
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

		/// <summary>call ptr1632/ptr1616 instruction.</summary>
		public void call(ushort basePtr, uint address) {
			AddInstruction(Instruction.CreateBranch(Bitness >= 32 ? Code.Call_ptr1632 : Code.Call_ptr1616, basePtr, address));
		}
		
		/// <summary>jmp ptr1632/ptr1616 instruction.</summary>
		public void jmp(ushort basePtr, uint address) {
			AddInstruction(Instruction.CreateBranch(Bitness >= 32 ? Code.Jmp_ptr1632 : Code.Jmp_ptr1616, basePtr, address));
		}
		
		/// <summary>
		/// Generates multibyte NOP instructions
		/// </summary>
		/// <param name="amount">Amount of NOPs</param>
		public void nop(int amount) {
			if (amount <= 0)
				throw new ArgumentOutOfRangeException($"{nameof(amount)} must be within 1-{int.MaxValue}");

			int maxMultibyteNop = Bitness >= 32 ? 9 : 4;

			int cycles = Math.DivRem(amount, maxMultibyteNop, out int rest);

			for (int i = 0; i < cycles; i++) {
				AppendNop(maxMultibyteNop);
			}
			if (rest > 0)
				AppendNop(rest);

			void AppendNop(int numberOfNops) {
				switch (numberOfNops) {
				case 1:
					db(0x90); //NOP
					break;
				case 2:
					db(0x66, 0x90); //66 NOP
					break;
				case 3:
					db(0x0F, 0x1F, 0x00); //NOP DWORD ptr [EAX]
					break;
				case 4:
					db(0x0F, 0x1F, 0x40, 0x00); //NOP DWORD ptr [EAX + 00H]
					break;
				case 5:
					db(0x0F, 0x1F, 0x44, 0x00, 0x00); //NOP DWORD ptr [EAX + EAX*1 + 00H]
					break;
				case 6:
					db(0x66, 0x0F, 0x1F, 0x44, 0x00, 0x00); //66 NOP DWORD ptr [EAX + EAX*1 + 00H]
					break;
				case 7:
					db(0x0F, 0x1F, 0x80, 0x00, 0x00, 0x00, 0x00); //NOP DWORD ptr [EAX + 00000000H]
					break;
				case 8:
					db(0x0F, 0x1F, 0x84, 0x00, 0x00, 0x00, 0x00, 0x00); //NOP DWORD ptr [EAX + EAX*1 + 00000000H]
					break;
				case 9:
					db(0x66, 0x0F, 0x1F, 0x84, 0x00, 0x00, 0x00, 0x00, 0x00); //66 NOP DWORD ptr [EAX + EAX*1 + 00000000H] 	
					break;

				}
			}
		}
		
		/// <summary>xlatb instruction.</summary>
		public void xlatb() {
			if (Bitness == 64) {
				AddInstruction(Instruction.Create(Code.Xlat_m8, new MemoryOperand(Register.RBX, Register.AL, 1)));
			} else if (Bitness == 32) {
				AddInstruction(Instruction.Create(Code.Xlat_m8, new MemoryOperand(Register.EBX, Register.AL, 1)));
			}
			else {
				AddInstruction(Instruction.Create(Code.Xlat_m8, new MemoryOperand(Register.BX, Register.AL, 1)));
			}
		}

		/// <summary>
		/// Encode the instructions of this assembler with the specified options.
		/// </summary>
		/// <param name="options">Encoding options.</param>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException"></exception>
		public BlockEncoderResult Encode(BlockEncoderOptions options = BlockEncoderOptions.None) {
			if (!TryEncode(out var errorMessage, out var blockResult, options)) {
				throw new InvalidOperationException(errorMessage);
			}
			return blockResult;
		}

		/// <summary>
		/// Tries to encode the instructions of this assembler with the specified options.
		/// </summary>
		/// <param name="errorMessage">Error messages.</param>
		/// <param name="blockResult">Block result.</param>
		/// <param name="options">Encoding options.</param>
		/// <returns><c>true</c> if the encoding was successful; <c>false</c> otherwise.</returns>
		public bool TryEncode(out string? errorMessage, out BlockEncoderResult blockResult, BlockEncoderOptions options = BlockEncoderOptions.None) {
			blockResult = default;

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
			
			var blocks = new InstructionBlock[1];
			var block = new InstructionBlock(this._writer, _instructions, BaseRip);
			blocks[0] = block;

			blockResult = default;
			var result = BlockEncoder.TryEncode(Bitness, blocks, out errorMessage, out var blockResults, options);
			if (result && blockResults != null) {
				blockResult = blockResults[0];
			}

			return result;
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
