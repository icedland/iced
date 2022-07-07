// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if FAST_FMT
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Iced.Intel.FastFormatterInternal;
using Iced.Intel.FormatterInternal;

namespace Iced.Intel {
	/// <summary>
	/// Fast formatter with less formatting options and with a masm-like syntax.
	/// Use it if formatting speed is more important than being able to re-assemble formatted instructions.
	/// <br/>
	/// <br/>
	/// This formatter is ~2.3x faster than the other formatters (the time includes decoding + formatting).
	/// </summary>
	public sealed class FastFormatter {
		readonly FastFormatterOptions options;
		readonly ISymbolResolver? symbolResolver;
		readonly FormatterString[] allRegisters;
		readonly string[] codeMnemonics;
		readonly FastFmtFlags[] codeFlags;
		readonly string[] allMemorySizes;
		readonly string[] rcStrings;
		readonly string[] rcSaeStrings;
		readonly string[] scaleNumbers;
#if MVEX
		readonly string[] mvexRegMemConsts32;
		readonly string[] mvexRegMemConsts64;
#endif

		const bool ShowUselessPrefixes = true;

		static readonly string[] s_rcStrings = new string[] {
			"{rn}",
			"{rd}",
			"{ru}",
			"{rz}",
		};
		static readonly string[] s_rcSaeStrings = new string[] {
			"{rn-sae}",
			"{rd-sae}",
			"{ru-sae}",
			"{rz-sae}",
		};
		static readonly string[] s_scaleNumbers = new string[4] {
			"*1", "*2", "*4", "*8",
		};
#if MVEX
		static readonly string[] s_mvexRegMemConsts32 = new string[IcedConstants.MvexRegMemConvEnumCount] {
			string.Empty,
			string.Empty,
			"{cdab}",
			"{badc}",
			"{dacb}",
			"{aaaa}",
			"{bbbb}",
			"{cccc}",
			"{dddd}",
			string.Empty,
			"{1to16}",
			"{4to16}",
			"{float16}",
			"{uint8}",
			"{sint8}",
			"{uint16}",
			"{sint16}",
		};
		static readonly string[] s_mvexRegMemConsts64 = new string[IcedConstants.MvexRegMemConvEnumCount] {
			string.Empty,
			string.Empty,
			"{cdab}",
			"{badc}",
			"{dacb}",
			"{aaaa}",
			"{bbbb}",
			"{cccc}",
			"{dddd}",
			string.Empty,
			"{1to8}",
			"{4to8}",
			"{float16}",
			"{uint8}",
			"{sint8}",
			"{uint16}",
			"{sint16}",
		};
#endif

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
			rcSaeStrings = s_rcSaeStrings;
			scaleNumbers = s_scaleNumbers;
#if MVEX
			mvexRegMemConsts32 = s_mvexRegMemConsts32;
			mvexRegMemConsts64 = s_mvexRegMemConsts64;
#endif
		}

		/// <summary>
		/// Formats the whole instruction: prefixes, mnemonic, operands
		/// </summary>
		/// <param name="instruction">Instruction</param>
		/// <param name="output">Output</param>
		public void Format(in Instruction instruction, FastStringOutput output) {
			if (output is null)
				ThrowHelper.ThrowArgumentNullException_output();

			var code = instruction.Code;
			var mnemonic = codeMnemonics[(int)code];
			var flags = codeFlags[(int)code];
			var opCount = instruction.OpCount;
			var pseudoOpsNum = (uint)flags >> (int)FastFmtFlags.PseudoOpsKindShift;
			if (pseudoOpsNum != 0 && options.UsePseudoOps && instruction.GetOpKind(opCount - 1) == OpKind.Immediate8) {
				int index = instruction.Immediate8;
				var pseudoOpKind = (PseudoOpsKind)(pseudoOpsNum - 1);
				if (pseudoOpKind == PseudoOpsKind.vpcmpd6) {
					switch (code) {
#if MVEX
					case Code.MVEX_Vpcmpud_kr_k1_zmm_zmmmt_imm8:
						pseudoOpKind = PseudoOpsKind.vpcmpud6;
						break;
#endif
					default:
						break;
					}
				}
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
			Static.Assert(Register.None == 0 ? 0 : -1);
			if (((uint)prefixSeg | instruction.HasAnyOf_Lock_Rep_Repne_Prefix) != 0) {
				bool hasNoTrackPrefix = prefixSeg == Register.DS && FormatterUtils.IsNotrackPrefixBranch(code);
				if (!hasNoTrackPrefix && prefixSeg != Register.None && ShowSegmentPrefix(instruction, opCount)) {
					FormatRegister(output, prefixSeg);
					output.Append(' ');
				}

				bool hasXacquirePrefix = false;
				if (instruction.HasXacquirePrefix) {
					output.AppendNotNull("xacquire ");
					hasXacquirePrefix = true;
				}
				if (instruction.HasXreleasePrefix) {
					output.AppendNotNull("xrelease ");
					hasXacquirePrefix = true;
				}
				if (instruction.HasLockPrefix)
					output.AppendNotNull("lock ");
				if (hasNoTrackPrefix)
					output.AppendNotNull("notrack ");
				if (!hasXacquirePrefix) {
					if (instruction.HasRepePrefix && (ShowUselessPrefixes || FormatterUtils.ShowRepOrRepePrefix(code, ShowUselessPrefixes))) {
						if (FormatterUtils.IsRepeOrRepneInstruction(code))
							output.AppendNotNull("repe ");
						else
							output.AppendNotNull("rep ");
					}
					if (instruction.HasRepnePrefix) {
						if ((Code.Retnw_imm16 <= code && code <= Code.Retnq) ||
							(Code.Call_rel16 <= code && code <= Code.Jmp_rel32_64) ||
							(Code.Call_rm16 <= code && code <= Code.Call_rm64) ||
							(Code.Jmp_rm16 <= code && code <= Code.Jmp_rm64) ||
							code.IsJccShortOrNear()) {
							output.AppendNotNull("bnd ");
						}
						else if (ShowUselessPrefixes || FormatterUtils.ShowRepnePrefix(code, ShowUselessPrefixes))
							output.AppendNotNull("repne ");
					}
				}
			}

			output.AppendNotNull(mnemonic);

			bool isDeclareData;
			OpKind declareDataOpKind;
			if ((uint)code - (uint)Code.DeclareByte <= (uint)Code.DeclareQword - (uint)Code.DeclareByte) {
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

#if MVEX
				int mvexRmOperand;
				if (IcedConstants.IsMvex(instruction.Code)) {
					Debug.Assert(opCount != 0);
					mvexRmOperand = instruction.GetOpKind(opCount - 1) == OpKind.Immediate8 ? opCount - 2 : opCount - 1;
				}
				else
					mvexRmOperand = -1;
#endif

				for (int operand = 0; operand < opCount; operand++) {
					if (operand > 0) {
						if (options.SpaceAfterOperandSeparator)
							output.AppendNotNull(", ");
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
						if ((symbolResolver = this.symbolResolver) is not null && symbolResolver.TryGetSymbol(instruction, operand, operand, imm64, immSize, out symbol))
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
						if ((symbolResolver = this.symbolResolver) is not null && symbolResolver.TryGetSymbol(instruction, operand, operand, (uint)imm64, immSize, out symbol)) {
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
						if ((symbolResolver = this.symbolResolver) is not null && symbolResolver.TryGetSymbol(instruction, operand, operand, imm8, 1, out symbol)) {
							if ((symbol.Flags & SymbolFlags.Relative) == 0)
								output.AppendNotNull("offset ");
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
						if ((symbolResolver = this.symbolResolver) is not null && symbolResolver.TryGetSymbol(instruction, operand, operand, imm16, 2, out symbol)) {
							if ((symbol.Flags & SymbolFlags.Relative) == 0)
								output.AppendNotNull("offset ");
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
						if ((symbolResolver = this.symbolResolver) is not null && symbolResolver.TryGetSymbol(instruction, operand, operand, imm32, 4, out symbol)) {
							if ((symbol.Flags & SymbolFlags.Relative) == 0)
								output.AppendNotNull("offset ");
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
						if ((symbolResolver = this.symbolResolver) is not null && symbolResolver.TryGetSymbol(instruction, operand, operand, imm64, 8, out symbol)) {
							if ((symbol.Flags & SymbolFlags.Relative) == 0)
								output.AppendNotNull("offset ");
							WriteSymbol(output, imm64, symbol);
						}
						else
							FormatNumber(output, imm64);
						break;

					case OpKind.MemorySegSI:
						FormatMemory(output, instruction, operand, instruction.MemorySegment, Register.SI, Register.None, 0, 0, 0, 2);
						break;

					case OpKind.MemorySegESI:
						FormatMemory(output, instruction, operand, instruction.MemorySegment, Register.ESI, Register.None, 0, 0, 0, 4);
						break;

					case OpKind.MemorySegRSI:
						FormatMemory(output, instruction, operand, instruction.MemorySegment, Register.RSI, Register.None, 0, 0, 0, 8);
						break;

					case OpKind.MemorySegDI:
						FormatMemory(output, instruction, operand, instruction.MemorySegment, Register.DI, Register.None, 0, 0, 0, 2);
						break;

					case OpKind.MemorySegEDI:
						FormatMemory(output, instruction, operand, instruction.MemorySegment, Register.EDI, Register.None, 0, 0, 0, 4);
						break;

					case OpKind.MemorySegRDI:
						FormatMemory(output, instruction, operand, instruction.MemorySegment, Register.RDI, Register.None, 0, 0, 0, 8);
						break;

					case OpKind.MemoryESDI:
						FormatMemory(output, instruction, operand, Register.ES, Register.DI, Register.None, 0, 0, 0, 2);
						break;

					case OpKind.MemoryESEDI:
						FormatMemory(output, instruction, operand, Register.ES, Register.EDI, Register.None, 0, 0, 0, 4);
						break;

					case OpKind.MemoryESRDI:
						FormatMemory(output, instruction, operand, Register.ES, Register.RDI, Register.None, 0, 0, 0, 8);
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
							displ = instruction.MemoryDisplacement32;
						if (code == Code.Xlat_m8)
							indexReg = Register.None;
						FormatMemory(output, instruction, operand, instruction.MemorySegment, baseReg, indexReg, instruction.InternalMemoryIndexScale, displSize, displ, addrSize);
						break;

					default:
						throw new InvalidOperationException();
					}

					if (operand == 0 && instruction.HasOpMask_or_ZeroingMasking) {
						if (instruction.HasOpMask) {
							output.Append('{');
							FormatRegister(output, instruction.OpMask);
							output.Append('}');
						}
						if (instruction.ZeroingMasking)
							output.AppendNotNull("{z}");
					}
#if MVEX
					if (mvexRmOperand == operand) {
						var conv = instruction.MvexRegMemConv;
						if (conv != MvexRegMemConv.None) {
							var mvex = new MvexInfo(instruction.Code);
							if (mvex.ConvFn != MvexConvFn.None) {
								var tbl = mvex.IsConvFn32 ? mvexRegMemConsts32 : mvexRegMemConsts64;
								var s = tbl[(int)conv];
								if (s.Length != 0)
									output.AppendNotNull(s);
							}
						}
					}
#endif
				}
				if (instruction.HasRoundingControlOrSae) {
					var rc = instruction.RoundingControl;
					if (rc != RoundingControl.None) {
						Static.Assert((int)RoundingControl.None == 0 ? 0 : -1);
						Static.Assert((int)RoundingControl.RoundToNearest == 1 ? 0 : -1);
						Static.Assert((int)RoundingControl.RoundDown == 2 ? 0 : -1);
						Static.Assert((int)RoundingControl.RoundUp == 3 ? 0 : -1);
						Static.Assert((int)RoundingControl.RoundTowardZero == 4 ? 0 : -1);
						if (IcedConstants.IsMvex(instruction.Code) && !instruction.SuppressAllExceptions)
							output.AppendNotNull(rcStrings[(int)rc - 1]);
						else
							output.AppendNotNull(rcSaeStrings[(int)rc - 1]);
					}
					else {
						Debug.Assert(instruction.SuppressAllExceptions);
						output.AppendNotNull("{sae}");
					}
				}
			}
		}

		// Only one caller
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static bool ShowSegmentPrefix(in Instruction instruction, int opCount) {
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
				case OpKind.Memory:
					return false;

				default:
					throw new InvalidOperationException();
				}
			}
			return ShowUselessPrefixes;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		void FormatRegister(FastStringOutput output, Register register) =>
			output.AppendNotNull(allRegisters[(int)register].Lower);

		void FormatNumber(FastStringOutput output, ulong value) {
			bool useHexPrefix = options.UseHexPrefix;
			if (useHexPrefix)
				output.AppendNotNull("0x");

			int shift = 0;
			for (ulong tmp = value; ;) {
				shift += 4;
				tmp >>= 4;
				if (tmp == 0)
					break;
			}

			if (!useHexPrefix && (int)((value >> (shift - 4)) & 0xF) > 9)
				output.Append('0');
			var hexDigits = options.UppercaseHex ? "0123456789ABCDEF" : "0123456789abcdef";
			for (; ; ) {
				shift -= 4;
				int digit = (int)(value >> shift) & 0xF;
				output.Append(hexDigits[digit]);
				if (shift == 0)
					break;
			}

			if (!useHexPrefix)
				output.Append('h');
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		void WriteSymbol(FastStringOutput output, ulong address, in SymbolResult symbol) => WriteSymbol(output, address, symbol, true);

		void WriteSymbol(FastStringOutput output, ulong address, in SymbolResult symbol, bool writeMinusIfSigned) {
			long displ = (long)(address - symbol.Address);
			if ((symbol.Flags & SymbolFlags.Signed) != 0) {
				if (writeMinusIfSigned)
					output.Append('-');
				displ = -displ;
			}

			var text = symbol.Text;
			var array = text.TextArray;
			if (array is not null) {
				foreach (var part in array) {
					if (part.Text is string s)
						output.AppendNotNull(s);
				}
			}
			else if (text.Text.Text is string s)
				output.AppendNotNull(s);

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
				output.AppendNotNull(" (");
				FormatNumber(output, address);
				output.Append(')');
			}
		}

		void FormatMemory(FastStringOutput output, in Instruction instruction, int operand, Register segReg, Register baseReg, Register indexReg, int scale, int displSize, long displ, int addrSize) {
			Debug.Assert((uint)scale < (uint)scaleNumbers.Length);
			Debug.Assert(InstructionUtils.GetAddressSizeInBytes(baseReg, indexReg, displSize, instruction.CodeSize) == addrSize);

			ulong absAddr;
			if (baseReg == Register.RIP) {
				absAddr = (ulong)displ;
				if (options.RipRelativeAddresses)
					displ -= (long)instruction.NextIP;
				else {
					Debug.Assert(indexReg == Register.None);
					baseReg = Register.None;
				}
				displSize = 8;
			}
			else if (baseReg == Register.EIP) {
				absAddr = (uint)displ;
				if (options.RipRelativeAddresses)
					displ = (int)((uint)displ - instruction.NextIP32);
				else {
					Debug.Assert(indexReg == Register.None);
					baseReg = Register.None;
				}
				displSize = 4;
			}
			else
				absAddr = (ulong)displ;

			SymbolResult symbol;
			bool useSymbol;
			if (this.symbolResolver is ISymbolResolver symbolResolver)
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
			bool showMemSize = (flags & FastFmtFlags.ForceMemSize) != 0 || instruction.IsBroadcast || options.AlwaysShowMemorySize;
			if (showMemSize) {
				Debug.Assert((uint)instruction.MemorySize < (uint)allMemorySizes.Length);
				var keywords = allMemorySizes[(int)instruction.MemorySize];
				output.AppendNotNull(keywords);
			}

			var codeSize = instruction.CodeSize;
			var segOverride = instruction.SegmentPrefix;
			bool noTrackPrefix = segOverride == Register.DS && FormatterUtils.IsNotrackPrefixBranch(instruction.Code) &&
				!((codeSize == CodeSize.Code16 || codeSize == CodeSize.Code32) && (baseReg == Register.BP || baseReg == Register.EBP || baseReg == Register.ESP));
			if (options.AlwaysShowSegmentRegister || (segOverride != Register.None && !noTrackPrefix &&
				(ShowUselessPrefixes || FormatterUtils.ShowSegmentPrefix(Register.None, instruction, ShowUselessPrefixes)))) {
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
					output.AppendNotNull(scaleNumbers[scale]);
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
					if (addrSize == 8) {
						if (displ < 0) {
							displ = -displ;
							output.Append('-');
						}
						else
							output.Append('+');
					}
					else if (addrSize == 4) {
						if ((int)displ < 0) {
							displ = (uint)-(int)displ;
							output.Append('-');
						}
						else
							output.Append('+');
					}
					else {
						Debug.Assert(addrSize == 2);
						if ((short)displ < 0) {
							displ = (ushort)-(short)displ;
							output.Append('-');
						}
						else
							output.Append('+');
					}
				}
				FormatNumber(output, (ulong)displ);
			}

			output.Append(']');
#if MVEX
			if (instruction.IsMvexEvictionHint)
				output.AppendNotNull("{eh}");
#endif
		}
	}
}
#endif
