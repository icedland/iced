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

#if FAST_FMT
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Iced.Intel.FastFormatterInternal;
using Iced.Intel.FormatterInternal;

namespace Iced.Intel {
	/// <summary>
	/// Fast formatter with less formatting options and with masm-like syntax.
	/// Use it if formatting speed is more important than being able to re-assemble formatted instructions.
	/// <br/>
	/// <br/>
	/// This formatter is 1.8-2.0x faster than the other formatters (the time includes decoding + formatting).
	/// </summary>
	public sealed class FastFormatter {
		readonly FastFormatterOptions options;
		readonly ISymbolResolver? symbolResolver;
		readonly FormatterString[] allRegisters;
		readonly string[] codeMnemonics;
		readonly FastFmtFlags[] codeFlags;
		readonly string[] allMemorySizes;
		readonly string[] rcStrings;
		readonly string[] scaleNumbers;

		const bool ShowUselessPrefixes = true;

		static readonly string[] s_rcStrings = new string[] {
			"{rn-sae}",
			"{rd-sae}",
			"{ru-sae}",
			"{rz-sae}",
		};
		static readonly string[] s_scaleNumbers = new string[4] {
			"*1", "*2", "*4", "*8",
		};

		/// <summary>
		/// Gets the formatter options
		/// </summary>
		public FastFormatterOptions Options => options;

		/// <summary>
		/// Constructor
		/// </summary>
		public FastFormatter() : this(null) { }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="symbolResolver">Symbol resolver or null</param>
		public FastFormatter(ISymbolResolver? symbolResolver) {
			options = new FastFormatterOptions();
			this.symbolResolver = symbolResolver;
			allRegisters = Registers.AllRegisters;
			codeMnemonics = FmtData.Mnemonics;
			codeFlags = FmtData.Flags;
			allMemorySizes = MemorySizes.AllMemorySizes;
			rcStrings = s_rcStrings;
			scaleNumbers = s_scaleNumbers;
		}

		/// <summary>
		/// Formats the whole instruction: prefixes, mnemonic, operands
		/// </summary>
		/// <param name="instruction">Instruction</param>
		/// <param name="output">Output</param>
		public void Format(in Instruction instruction, StringBuilder output) {
			if (output is null)
				ThrowHelper.ThrowArgumentNullException_output();

			var code = instruction.Code;
			var mnemonic = codeMnemonics[(int)code];
			var flags = codeFlags[(int)code];
			var pseudoOpsNum = (uint)flags >> (int)FastFmtFlags.PseudoOpsKindShift;
			var opCount = instruction.OpCount;
			if (pseudoOpsNum != 0 && options.UsePseudoOps && instruction.GetOpKind(opCount - 1) == OpKind.Immediate8) {
				int index = instruction.Immediate8;
				var pseudoOpKind = (PseudoOpsKind)(pseudoOpsNum - 1);
				var pseudoOps = FormatterConstants.GetPseudoOps(pseudoOpKind);
				if (pseudoOpKind == PseudoOpsKind.pclmulqdq || pseudoOpKind == PseudoOpsKind.vpclmulqdq) {
					if (index <= 1) {
						// nothing
					}
					else if (index == 0x10)
						index = 2;
					else if (index == 0x11)
						index = 3;
					else
						index = -1;
				}
				if ((uint)index < (uint)pseudoOps.Length) {
					mnemonic = pseudoOps[index].Lower;
					opCount--;
				}
			}

			var prefixSeg = instruction.SegmentPrefix;
			if (((uint)prefixSeg | instruction.HasAnyOf_Xacquire_Xrelease_Lock_Rep_Repne_Prefix) != 0) {
				bool hasNoTrackPrefix = prefixSeg == Register.DS && FormatterUtils.IsNotrackPrefixBranch(code);
				if (!hasNoTrackPrefix && prefixSeg != Register.None && ShowSegmentPrefix(instruction, opCount)) {
					FormatRegister(output, prefixSeg);
					output.Append(' ');
				}

				if (instruction.HasXacquirePrefix)
					output.Append("xacquire ");
				if (instruction.HasXreleasePrefix)
					output.Append("xrelease ");
				if (instruction.HasLockPrefix)
					output.Append("lock ");

				if (hasNoTrackPrefix)
					output.Append("notrack ");

				if (instruction.HasRepePrefix && FormatterUtils.ShowRepOrRepePrefix(code, ShowUselessPrefixes)) {
					if (FormatterUtils.IsRepeOrRepneInstruction(code))
						output.Append("repe ");
					else
						output.Append("rep ");
				}
				if (instruction.HasRepnePrefix) {
					if ((Code.Retnw_imm16 <= code && code <= Code.Retnq) ||
						(Code.Call_rel16 <= code && code <= Code.Jmp_rel32_64) ||
						(Code.Call_rm16 <= code && code <= Code.Call_rm64) ||
						(Code.Jmp_rm16 <= code && code <= Code.Jmp_rm64) ||
						code.IsJccShortOrNear()) {
						output.Append("bnd ");
					}
					else if (FormatterUtils.ShowRepnePrefix(code, ShowUselessPrefixes))
						output.Append("repne ");
				}
			}

			output.Append(mnemonic);

			bool isDeclareData;
			OpKind declareDataOpKind;
			if ((uint)code - 1 <= (uint)Code.DeclareQword - 1) {
				opCount = instruction.DeclareDataCount;
				isDeclareData = true;
				switch (code) {
				case Code.DeclareByte:
					declareDataOpKind = OpKind.Immediate8;
					break;
				case Code.DeclareWord:
					declareDataOpKind = OpKind.Immediate16;
					break;
				case Code.DeclareDword:
					declareDataOpKind = OpKind.Immediate32;
					break;
				default:
					Debug.Assert(code == Code.DeclareQword);
					declareDataOpKind = OpKind.Immediate64;
					break;
				}
			}
			else {
				isDeclareData = false;
				declareDataOpKind = OpKind.Register;
			}

			if (opCount > 0) {
				output.Append(' ');

				for (int operand = 0; operand < opCount; operand++) {
					if (operand > 0) {
						if (options.SpaceAfterOperandSeparator)
							output.Append(", ");
						else
							output.Append(',');
					}

					byte imm8;
					ushort imm16;
					uint imm32;
					ulong imm64;
					int immSize;
					SymbolResult symbol;
					ISymbolResolver? symbolResolver;
					var opKind = isDeclareData ? declareDataOpKind : instruction.GetOpKind(operand);
					switch (opKind) {
					case OpKind.Register:
						FormatRegister(output, instruction.GetOpRegister(operand));
						break;

					case OpKind.NearBranch16:
					case OpKind.NearBranch32:
					case OpKind.NearBranch64:
						if (opKind == OpKind.NearBranch64) {
							immSize = 8;
							imm64 = instruction.NearBranch64;
						}
						else if (opKind == OpKind.NearBranch32) {
							immSize = 4;
							imm64 = instruction.NearBranch32;
						}
						else {
							immSize = 2;
							imm64 = instruction.NearBranch16;
						}
						if (!((symbolResolver = this.symbolResolver) is null) && symbolResolver.TryGetSymbol(instruction, operand, operand, imm64, immSize, out symbol))
							WriteSymbol(output, imm64, symbol);
						else
							FormatNumber(output, imm64);
						break;

					case OpKind.FarBranch16:
					case OpKind.FarBranch32:
						if (opKind == OpKind.FarBranch32) {
							immSize = 4;
							imm64 = instruction.FarBranch32;
						}
						else {
							immSize = 2;
							imm64 = instruction.FarBranch16;
						}
						if (!((symbolResolver = this.symbolResolver) is null) && symbolResolver.TryGetSymbol(instruction, operand, operand, (uint)imm64, immSize, out symbol)) {
							Debug.Assert(operand + 1 == 1);
							if (!symbolResolver.TryGetSymbol(instruction, operand + 1, operand, instruction.FarBranchSelector, 2, out var selectorSymbol))
								FormatNumber(output, instruction.FarBranchSelector);
							else
								WriteSymbol(output, instruction.FarBranchSelector, selectorSymbol);
							output.Append(':');
							WriteSymbol(output, imm64, symbol);
						}
						else {
							FormatNumber(output, instruction.FarBranchSelector);
							output.Append(':');
							if (opKind == OpKind.FarBranch32)
								FormatNumber(output, instruction.FarBranch32);
							else
								FormatNumber(output, instruction.FarBranch16);
						}
						break;

					case OpKind.Immediate8:
					case OpKind.Immediate8_2nd:
						if (isDeclareData)
							imm8 = instruction.GetDeclareByteValue(operand);
						else if (opKind == OpKind.Immediate8)
							imm8 = instruction.Immediate8;
						else {
							Debug.Assert(opKind == OpKind.Immediate8_2nd);
							imm8 = instruction.Immediate8_2nd;
						}
						if (!((symbolResolver = this.symbolResolver) is null) && symbolResolver.TryGetSymbol(instruction, operand, operand, imm8, 1, out symbol)) {
							if ((symbol.Flags & SymbolFlags.Relative) == 0)
								output.Append("offset ");
							WriteSymbol(output, imm8, symbol);
						}
						else
							FormatNumber(output, imm8);
						break;

					case OpKind.Immediate16:
					case OpKind.Immediate8to16:
						if (isDeclareData)
							imm16 = instruction.GetDeclareWordValue(operand);
						else if (opKind == OpKind.Immediate16)
							imm16 = instruction.Immediate16;
						else {
							Debug.Assert(opKind == OpKind.Immediate8to16);
							imm16 = (ushort)instruction.Immediate8to16;
						}
						if (!((symbolResolver = this.symbolResolver) is null) && symbolResolver.TryGetSymbol(instruction, operand, operand, imm16, 2, out symbol)) {
							if ((symbol.Flags & SymbolFlags.Relative) == 0)
								output.Append("offset ");
							WriteSymbol(output, imm16, symbol);
						}
						else
							FormatNumber(output, imm16);
						break;

					case OpKind.Immediate32:
					case OpKind.Immediate8to32:
						if (isDeclareData)
							imm32 = instruction.GetDeclareDwordValue(operand);
						else if (opKind == OpKind.Immediate32)
							imm32 = instruction.Immediate32;
						else {
							Debug.Assert(opKind == OpKind.Immediate8to32);
							imm32 = (uint)instruction.Immediate8to32;
						}
						if (!((symbolResolver = this.symbolResolver) is null) && symbolResolver.TryGetSymbol(instruction, operand, operand, imm32, 4, out symbol)) {
							if ((symbol.Flags & SymbolFlags.Relative) == 0)
								output.Append("offset ");
							WriteSymbol(output, imm32, symbol);
						}
						else
							FormatNumber(output, imm32);
						break;

					case OpKind.Immediate64:
					case OpKind.Immediate8to64:
					case OpKind.Immediate32to64:
						if (isDeclareData)
							imm64 = instruction.GetDeclareQwordValue(operand);
						else if (opKind == OpKind.Immediate32to64)
							imm64 = (ulong)instruction.Immediate32to64;
						else if (opKind == OpKind.Immediate8to64)
							imm64 = (ulong)instruction.Immediate8to64;
						else {
							Debug.Assert(opKind == OpKind.Immediate64);
							imm64 = instruction.Immediate64;
						}
						if (!((symbolResolver = this.symbolResolver) is null) && symbolResolver.TryGetSymbol(instruction, operand, operand, imm64, 8, out symbol)) {
							if ((symbol.Flags & SymbolFlags.Relative) == 0)
								output.Append("offset ");
							WriteSymbol(output, imm64, symbol);
						}
						else
							FormatNumber(output, imm64);
						break;

					case OpKind.MemorySegSI:
						FormatMemory(output, instruction, operand, instruction.MemorySize, instruction.SegmentPrefix, instruction.MemorySegment, Register.SI, Register.None, 0, 0, 0, 2);
						break;

					case OpKind.MemorySegESI:
						FormatMemory(output, instruction, operand, instruction.MemorySize, instruction.SegmentPrefix, instruction.MemorySegment, Register.ESI, Register.None, 0, 0, 0, 4);
						break;

					case OpKind.MemorySegRSI:
						FormatMemory(output, instruction, operand, instruction.MemorySize, instruction.SegmentPrefix, instruction.MemorySegment, Register.RSI, Register.None, 0, 0, 0, 8);
						break;

					case OpKind.MemorySegDI:
						FormatMemory(output, instruction, operand, instruction.MemorySize, instruction.SegmentPrefix, instruction.MemorySegment, Register.DI, Register.None, 0, 0, 0, 2);
						break;

					case OpKind.MemorySegEDI:
						FormatMemory(output, instruction, operand, instruction.MemorySize, instruction.SegmentPrefix, instruction.MemorySegment, Register.EDI, Register.None, 0, 0, 0, 4);
						break;

					case OpKind.MemorySegRDI:
						FormatMemory(output, instruction, operand, instruction.MemorySize, instruction.SegmentPrefix, instruction.MemorySegment, Register.RDI, Register.None, 0, 0, 0, 8);
						break;

					case OpKind.MemoryESDI:
						FormatMemory(output, instruction, operand, instruction.MemorySize, instruction.SegmentPrefix, Register.ES, Register.DI, Register.None, 0, 0, 0, 2);
						break;

					case OpKind.MemoryESEDI:
						FormatMemory(output, instruction, operand, instruction.MemorySize, instruction.SegmentPrefix, Register.ES, Register.EDI, Register.None, 0, 0, 0, 4);
						break;

					case OpKind.MemoryESRDI:
						FormatMemory(output, instruction, operand, instruction.MemorySize, instruction.SegmentPrefix, Register.ES, Register.RDI, Register.None, 0, 0, 0, 8);
						break;

					case OpKind.Memory64:
						FormatMemory(output, instruction, operand, instruction.MemorySize, instruction.SegmentPrefix, instruction.MemorySegment, Register.None, Register.None, 0, 8, (long)instruction.MemoryAddress64, 8);
						break;

					case OpKind.Memory:
						int displSize = instruction.MemoryDisplSize;
						var baseReg = instruction.MemoryBase;
						var indexReg = instruction.MemoryIndex;
						int addrSize = InstructionUtils.GetAddressSizeInBytes(baseReg, indexReg, displSize, instruction.CodeSize);
						long displ;
						if (addrSize == 8)
							displ = (long)instruction.MemoryDisplacement64;
						else
							displ = instruction.MemoryDisplacement;
						if (code == Code.Xlat_m8)
							indexReg = Register.None;
						FormatMemory(output, instruction, operand, instruction.MemorySize, instruction.SegmentPrefix, instruction.MemorySegment, baseReg, indexReg, instruction.InternalMemoryIndexScale, displSize, displ, addrSize);
						break;

					default:
						throw new InvalidOperationException();
					}

					if (operand == 0 && instruction.HasOpMask) {
						output.Append('{');
						FormatRegister(output, instruction.OpMask);
						output.Append('}');
						if (instruction.ZeroingMasking)
							output.Append("{z}");
					}
				}
				if (instruction.HasRoundingControlOrSae) {
					var rc = instruction.RoundingControl;
					if (rc != RoundingControl.None) {
						Static.Assert((int)RoundingControl.None == 0 ? 0 : -1);
						Static.Assert((int)RoundingControl.RoundToNearest == 1 ? 0 : -1);
						Static.Assert((int)RoundingControl.RoundDown == 2 ? 0 : -1);
						Static.Assert((int)RoundingControl.RoundUp == 3 ? 0 : -1);
						Static.Assert((int)RoundingControl.RoundTowardZero == 4 ? 0 : -1);
						output.Append(rcStrings[(int)rc - 1]);
					}
					else {
						Debug.Assert(instruction.SuppressAllExceptions);
						output.Append("{sae}");
					}
				}
			}
		}

		// Only one caller
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		bool ShowSegmentPrefix(in Instruction instruction, int opCount) {
			for (int i = 0; i < opCount; i++) {
				switch (instruction.GetOpKind(i)) {
				case OpKind.Register:
				case OpKind.NearBranch16:
				case OpKind.NearBranch32:
				case OpKind.NearBranch64:
				case OpKind.FarBranch16:
				case OpKind.FarBranch32:
				case OpKind.Immediate8:
				case OpKind.Immediate8_2nd:
				case OpKind.Immediate16:
				case OpKind.Immediate32:
				case OpKind.Immediate64:
				case OpKind.Immediate8to16:
				case OpKind.Immediate8to32:
				case OpKind.Immediate8to64:
				case OpKind.Immediate32to64:
				case OpKind.MemoryESDI:
				case OpKind.MemoryESEDI:
				case OpKind.MemoryESRDI:
					break;

				case OpKind.MemorySegSI:
				case OpKind.MemorySegESI:
				case OpKind.MemorySegRSI:
				case OpKind.MemorySegDI:
				case OpKind.MemorySegEDI:
				case OpKind.MemorySegRDI:
				case OpKind.Memory64:
				case OpKind.Memory:
					return false;

				default:
					throw new InvalidOperationException();
				}
			}
			return ShowUselessPrefixes;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		void FormatRegister(StringBuilder output, Register register) =>
			output.Append(allRegisters[(int)register].Lower);

		void FormatNumber(StringBuilder output, ulong value) {
			bool useHexPrefix = options.UseHexPrefix;
			if (useHexPrefix)
				output.Append("0x");

			int digits = 1;
			for (ulong tmp = value; ;) {
				tmp >>= 4;
				if (tmp == 0)
					break;
				digits++;
			}

			int hexHigh = options.UppercaseHex ? 'A' - 10 : 'a' - 10;
			if (!useHexPrefix && digits < 17 && (int)((value >> ((digits - 1) << 2)) & 0xF) > 9)
				digits++;// Another 0
			for (int i = 0; i < digits; i++) {
				int index = digits - i - 1;
				int digit = index >= 16 ? 0 : (int)((value >> (index << 2)) & 0xF);
				if (digit > 9)
					output.Append((char)(digit + hexHigh));
				else
					output.Append((char)(digit + '0'));
			}

			if (!useHexPrefix)
				output.Append('h');
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		void WriteSymbol(StringBuilder output, ulong address, SymbolResult symbol) => WriteSymbol(output, address, symbol, true);

		void WriteSymbol(StringBuilder output, ulong address, SymbolResult symbol, bool writeMinusIfSigned) {
			long displ = (long)(address - symbol.Address);
			if ((symbol.Flags & SymbolFlags.Signed) != 0) {
				if (writeMinusIfSigned)
					output.Append('-');
				displ = -displ;
			}

			var text = symbol.Text;
			var array = text.TextArray;
			if (!(array is null)) {
				foreach (var part in array)
					output.Append(part.Text);
			}
			else if (text.Text.Text is string s)
				output.Append(s);

			if (displ != 0) {
				if (displ < 0) {
					output.Append('-');
					displ = -displ;
				}
				else
					output.Append('+');
				FormatNumber(output, (ulong)displ);
			}
			if (options.ShowSymbolAddress) {
				output.Append(" (");
				FormatNumber(output, address);
				output.Append(')');
			}
		}

		void FormatMemory(StringBuilder output, in Instruction instruction, int operand, MemorySize memSize, Register segOverride, Register segReg, Register baseReg, Register indexReg, int scale, int displSize, long displ, int addrSize) {
			Debug.Assert((uint)scale < (uint)scaleNumbers.Length);
			Debug.Assert(InstructionUtils.GetAddressSizeInBytes(baseReg, indexReg, displSize, instruction.CodeSize) == addrSize);

			ulong absAddr;
			if (baseReg == Register.RIP) {
				absAddr = (ulong)((long)instruction.NextIP + (int)displ);
				if (!options.RipRelativeAddresses) {
					Debug.Assert(indexReg == Register.None);
					baseReg = Register.None;
					displ = (long)absAddr;
					displSize = 8;
				}
			}
			else if (baseReg == Register.EIP) {
				absAddr = instruction.NextIP32 + (uint)displ;
				if (!options.RipRelativeAddresses) {
					Debug.Assert(indexReg == Register.None);
					baseReg = Register.None;
					displ = (long)absAddr;
					displSize = 4;
				}
			}
			else
				absAddr = (ulong)displ;

			SymbolResult symbol;
			bool useSymbol;
			var symbolResolver = this.symbolResolver;
			if (!(symbolResolver is null))
				useSymbol = symbolResolver.TryGetSymbol(instruction, operand, operand, absAddr, addrSize, out symbol);
			else {
				useSymbol = false;
				symbol = default;
			}

			bool useScale = scale != 0;
			if (!useScale) {
				// [rsi] = base reg, [rsi*1] = index reg
				if (baseReg == Register.None)
					useScale = true;
			}
			if (addrSize == 2)
				useScale = false;

			var flags = codeFlags[(int)instruction.Code];
			bool showMemSize = (flags & FastFmtFlags.ForceMemSize) != 0 || memSize.IsBroadcast() || options.AlwaysShowMemorySize;
			if (showMemSize) {
				Debug.Assert((uint)memSize < (uint)allMemorySizes.Length);
				var keywords = allMemorySizes[(int)memSize];
				output.Append(keywords);
			}

			var codeSize = instruction.CodeSize;
			bool noTrackPrefix = segOverride == Register.DS && FormatterUtils.IsNotrackPrefixBranch(instruction.Code) &&
				!((codeSize == CodeSize.Code16 || codeSize == CodeSize.Code32) && (baseReg == Register.BP || baseReg == Register.EBP || baseReg == Register.ESP));
			if (options.AlwaysShowSegmentRegister || (segOverride != Register.None && !noTrackPrefix && FormatterUtils.ShowSegmentPrefix(Register.None, instruction, ShowUselessPrefixes))) {
				FormatRegister(output, segReg);
				output.Append(':');
			}
			output.Append('[');

			bool needPlus = false;
			if (baseReg != Register.None) {
				FormatRegister(output, baseReg);
				needPlus = true;
			}

			if (indexReg != Register.None) {
				if (needPlus)
					output.Append('+');
				needPlus = true;

				FormatRegister(output, indexReg);
				if (useScale)
					output.Append(scaleNumbers[scale]);
			}

			if (useSymbol) {
				if (needPlus) {
					if ((symbol.Flags & SymbolFlags.Signed) != 0)
						output.Append('-');
					else
						output.Append('+');
				}
				else if ((symbol.Flags & SymbolFlags.Signed) != 0)
					output.Append('-');

				WriteSymbol(output, absAddr, symbol, false);
			}
			else if (!needPlus || (displSize != 0 && displ != 0)) {
				if (needPlus) {
					if (addrSize == 4) {
						if ((int)displ < 0) {
							output.Append('-');
							displ = (uint)-(int)displ;
						}
						else
							output.Append('+');
					}
					else if (addrSize == 8) {
						if (displ < 0) {
							output.Append('-');
							displ = -displ;
						}
						else
							output.Append('+');
					}
					else {
						Debug.Assert(addrSize == 2);
						if ((short)displ < 0) {
							output.Append('-');
							displ = (ushort)-(short)displ;
						}
						else
							output.Append('+');
					}
				}
				FormatNumber(output, (ulong)displ);
			}

			output.Append(']');
		}
	}
}
#endif
