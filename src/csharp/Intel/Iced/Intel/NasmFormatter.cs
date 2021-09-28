// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if NASM
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Iced.Intel.FormatterInternal;
using Iced.Intel.NasmFormatterInternal;

namespace Iced.Intel {
	/// <summary>
	/// Nasm formatter
	/// </summary>
	public sealed class NasmFormatter : Formatter {
		/// <summary>
		/// Gets the formatter options
		/// </summary>
		public override FormatterOptions Options => options;

		readonly FormatterOptions options;
		readonly ISymbolResolver? symbolResolver;
		readonly IFormatterOptionsProvider? optionsProvider;
		readonly FormatterString[] allRegisters;
		readonly InstrInfo[] instrInfos;
		readonly MemorySizes.Info[] allMemorySizes;
		readonly NumberFormatter numberFormatter;
		readonly FormatterString[] opSizeStrings;
		readonly FormatterString[] addrSizeStrings;
		readonly FormatterString[]?[] branchInfos;
		readonly string[] scaleNumbers;
#if MVEX
		readonly FormatterString[] mvexRegMemConsts32;
		readonly FormatterString[] mvexRegMemConsts64;
#endif
		readonly FormatterString[] memSizeInfos;
		readonly FormatterString[] farMemSizeInfos;

		/// <summary>
		/// Constructor
		/// </summary>
		public NasmFormatter() : this(null, null, null) { }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="symbolResolver">Symbol resolver or null</param>
		/// <param name="optionsProvider">Operand options provider or null</param>
		public NasmFormatter(ISymbolResolver? symbolResolver, IFormatterOptionsProvider? optionsProvider = null)
			: this(null, symbolResolver, optionsProvider) { }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="options">Formatter options or null</param>
		/// <param name="symbolResolver">Symbol resolver or null</param>
		/// <param name="optionsProvider">Operand options provider or null</param>
		public NasmFormatter(FormatterOptions? options, ISymbolResolver? symbolResolver = null, IFormatterOptionsProvider? optionsProvider = null) {
			this.options = options ?? FormatterOptions.CreateNasm();
			this.symbolResolver = symbolResolver;
			this.optionsProvider = optionsProvider;
			allRegisters = Registers.AllRegisters;
			instrInfos = InstrInfos.AllInfos;
			allMemorySizes = MemorySizes.AllMemorySizes;
			numberFormatter = new NumberFormatter(true);
			opSizeStrings = s_opSizeStrings;
			addrSizeStrings = s_addrSizeStrings;
			branchInfos = s_branchInfos;
			scaleNumbers = s_scaleNumbers;
#if MVEX
			mvexRegMemConsts32 = s_mvexRegMemConsts32;
			mvexRegMemConsts64 = s_mvexRegMemConsts64;
#endif
			memSizeInfos = s_memSizeInfos;
			farMemSizeInfos = s_farMemSizeInfos;
		}

		static readonly FormatterString str_bnd = new FormatterString("bnd");
		static readonly FormatterString str_byte = new FormatterString("byte");
		static readonly FormatterString str_dword = new FormatterString("dword");
		static readonly FormatterString str_lock = new FormatterString("lock");
		static readonly FormatterString str_notrack = new FormatterString("notrack");
		static readonly FormatterString str_qword = new FormatterString("qword");
		static readonly FormatterString str_rel = new FormatterString("rel");
		static readonly FormatterString str_rep = new FormatterString("rep");
		static readonly FormatterString[] str_repe = new FormatterString[2] {
			new FormatterString("repe"),
			new FormatterString("repz"),
		};
		static readonly FormatterString[] str_repne = new FormatterString[2] {
			new FormatterString("repne"),
			new FormatterString("repnz"),
		};
		static readonly FormatterString str_rn_sae = new FormatterString("rn-sae");
		static readonly FormatterString str_rd_sae = new FormatterString("rd-sae");
		static readonly FormatterString str_ru_sae = new FormatterString("ru-sae");
		static readonly FormatterString str_rz_sae = new FormatterString("rz-sae");
		static readonly FormatterString str_sae = new FormatterString("sae");
		static readonly FormatterString str_rn = new FormatterString("rn");
		static readonly FormatterString str_rd = new FormatterString("rd");
		static readonly FormatterString str_ru = new FormatterString("ru");
		static readonly FormatterString str_rz = new FormatterString("rz");
		static readonly FormatterString str_to = new FormatterString("to");
		static readonly FormatterString str_word = new FormatterString("word");
		static readonly FormatterString str_xacquire = new FormatterString("xacquire");
		static readonly FormatterString str_xrelease = new FormatterString("xrelease");
		static readonly FormatterString str_z = new FormatterString("z");
		static readonly FormatterString[] s_opSizeStrings = new FormatterString[(int)InstrOpInfoFlags.SizeOverrideMask + 1] {
			new FormatterString(""),
			new FormatterString("o16"),
			new FormatterString("o32"),
			new FormatterString("o64"),
		};
		static readonly FormatterString[] s_addrSizeStrings = new FormatterString[(int)InstrOpInfoFlags.SizeOverrideMask + 1] {
			new FormatterString(""),
			new FormatterString("a16"),
			new FormatterString("a32"),
			new FormatterString("a64"),
		};
		static readonly FormatterString[]?[] s_branchInfos = new FormatterString[]?[(int)InstrOpInfoFlags.BranchSizeInfoMask + 1] {
			null,
			new[] { new FormatterString("near") },
			new[] { new FormatterString("near"), new FormatterString("word") },
			new[] { new FormatterString("near"), new FormatterString("dword") },
			new[] { new FormatterString("word") },
			new[] { new FormatterString("dword") },
			new[] { new FormatterString("short") },
			null,
		};
		static readonly FormatterString[] s_memSizeInfos = new FormatterString[(int)InstrOpInfoFlags.MemorySizeInfoMask + 1] {
			new FormatterString(""),
			new FormatterString("word"),
			new FormatterString("dword"),
			new FormatterString("qword"),
		};
		static readonly FormatterString[] s_farMemSizeInfos = new FormatterString[(int)InstrOpInfoFlags.FarMemorySizeInfoMask + 1] {
			new FormatterString(""),
			new FormatterString("word"),
			new FormatterString("dword"),
			new FormatterString(""),
		};
		static readonly string[] s_scaleNumbers = new string[4] {
			"1", "2", "4", "8",
		};
#if MVEX
		static readonly FormatterString[] s_mvexRegMemConsts32 = new FormatterString[IcedConstants.MvexRegMemConvEnumCount] {
			new FormatterString(""),
			new FormatterString(""),
			new FormatterString("cdab"),
			new FormatterString("badc"),
			new FormatterString("dacb"),
			new FormatterString("aaaa"),
			new FormatterString("bbbb"),
			new FormatterString("cccc"),
			new FormatterString("dddd"),
			new FormatterString(""),
			new FormatterString("1to16"),
			new FormatterString("4to16"),
			new FormatterString("float16"),
			new FormatterString("uint8"),
			new FormatterString("sint8"),
			new FormatterString("uint16"),
			new FormatterString("sint16"),
		};
		static readonly FormatterString[] s_mvexRegMemConsts64 = new FormatterString[IcedConstants.MvexRegMemConvEnumCount] {
			new FormatterString(""),
			new FormatterString(""),
			new FormatterString("cdab"),
			new FormatterString("badc"),
			new FormatterString("dacb"),
			new FormatterString("aaaa"),
			new FormatterString("bbbb"),
			new FormatterString("cccc"),
			new FormatterString("dddd"),
			new FormatterString(""),
			new FormatterString("1to8"),
			new FormatterString("4to8"),
			new FormatterString("float16"),
			new FormatterString("uint8"),
			new FormatterString("sint8"),
			new FormatterString("uint16"),
			new FormatterString("sint16"),
		};
		static readonly FormatterString str_eh = new FormatterString("eh");
#endif

		/// <summary>
		/// Formats the mnemonic and/or any prefixes
		/// </summary>
		/// <param name="instruction">Instruction</param>
		/// <param name="output">Output</param>
		/// <param name="options">Options</param>
		public override void FormatMnemonic(in Instruction instruction, FormatterOutput output, FormatMnemonicOptions options) {
			Debug.Assert((uint)instruction.Code < (uint)instrInfos.Length);
			var instrInfo = instrInfos[(int)instruction.Code];
			instrInfo.GetOpInfo(this.options, instruction, out var opInfo);
			int column = 0;
			FormatMnemonic(instruction, output, opInfo, ref column, options);
		}

		/// <summary>
		/// Gets the number of operands that will be formatted. A formatter can add and remove operands
		/// </summary>
		/// <param name="instruction">Instruction</param>
		/// <returns></returns>
		public override int GetOperandCount(in Instruction instruction) {
			Debug.Assert((uint)instruction.Code < (uint)instrInfos.Length);
			var instrInfo = instrInfos[(int)instruction.Code];
			instrInfo.GetOpInfo(options, instruction, out var opInfo);
			return opInfo.OpCount;
		}

#if INSTR_INFO
		/// <summary>
		/// Returns the operand access but only if it's an operand added by the formatter. If it's an
		/// operand that is part of <see cref="Instruction"/>, you should call eg. <see cref="InstructionInfoFactory.GetInfo(in Instruction)"/>.
		/// </summary>
		/// <param name="instruction">Instruction</param>
		/// <param name="operand">Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand.
		/// See <see cref="GetOperandCount(in Instruction)"/></param>
		/// <param name="access">Updated with operand access if successful</param>
		/// <returns></returns>
		public override bool TryGetOpAccess(in Instruction instruction, int operand, out OpAccess access) {
			Debug.Assert((uint)instruction.Code < (uint)instrInfos.Length);
			var instrInfo = instrInfos[(int)instruction.Code];
			instrInfo.GetOpInfo(options, instruction, out var opInfo);
			// Although it's a TryXXX() method, it should only accept valid instruction operand indexes
			if ((uint)operand >= (uint)opInfo.OpCount)
				ThrowHelper.ThrowArgumentOutOfRangeException_operand();
			return opInfo.TryGetOpAccess(operand, out access);
		}
#endif

		/// <summary>
		/// Converts a formatter operand index to an instruction operand index. Returns -1 if it's an operand added by the formatter
		/// </summary>
		/// <param name="instruction">Instruction</param>
		/// <param name="operand">Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand.
		/// See <see cref="GetOperandCount(in Instruction)"/></param>
		/// <returns></returns>
		public override int GetInstructionOperand(in Instruction instruction, int operand) {
			Debug.Assert((uint)instruction.Code < (uint)instrInfos.Length);
			var instrInfo = instrInfos[(int)instruction.Code];
			instrInfo.GetOpInfo(options, instruction, out var opInfo);
			if ((uint)operand >= (uint)opInfo.OpCount)
				ThrowHelper.ThrowArgumentOutOfRangeException_operand();
			return opInfo.GetInstructionIndex(operand);
		}

		/// <summary>
		/// Converts an instruction operand index to a formatter operand index. Returns -1 if the instruction operand isn't used by the formatter
		/// </summary>
		/// <param name="instruction">Instruction</param>
		/// <param name="instructionOperand">Instruction operand</param>
		/// <returns></returns>
		public override int GetFormatterOperand(in Instruction instruction, int instructionOperand) {
			Debug.Assert((uint)instruction.Code < (uint)instrInfos.Length);
			var instrInfo = instrInfos[(int)instruction.Code];
			instrInfo.GetOpInfo(options, instruction, out var opInfo);
			if ((uint)instructionOperand >= (uint)instruction.OpCount)
				ThrowHelper.ThrowArgumentOutOfRangeException_instructionOperand();
			return opInfo.GetOperandIndex(instructionOperand);
		}

		/// <summary>
		/// Formats an operand. This is a formatter operand and not necessarily a real instruction operand.
		/// A formatter can add and remove operands.
		/// </summary>
		/// <param name="instruction">Instruction</param>
		/// <param name="output">Output</param>
		/// <param name="operand">Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand.
		/// See <see cref="GetOperandCount(in Instruction)"/></param>
		public override void FormatOperand(in Instruction instruction, FormatterOutput output, int operand) {
			Debug.Assert((uint)instruction.Code < (uint)instrInfos.Length);
			var instrInfo = instrInfos[(int)instruction.Code];
			instrInfo.GetOpInfo(options, instruction, out var opInfo);

			if ((uint)operand >= (uint)opInfo.OpCount)
				ThrowHelper.ThrowArgumentOutOfRangeException_operand();
			FormatOperand(instruction, output, opInfo, operand);
		}

		/// <summary>
		/// Formats an operand separator
		/// </summary>
		/// <param name="instruction">Instruction</param>
		/// <param name="output">Output</param>
		public override void FormatOperandSeparator(in Instruction instruction, FormatterOutput output) {
			if (output is null)
				ThrowHelper.ThrowArgumentNullException_output();
			output.Write(",", FormatterTextKind.Punctuation);
			if (options.SpaceAfterOperandSeparator)
				output.Write(" ", FormatterTextKind.Text);
		}

		/// <summary>
		/// Formats all operands
		/// </summary>
		/// <param name="instruction">Instruction</param>
		/// <param name="output">Output</param>
		public override void FormatAllOperands(in Instruction instruction, FormatterOutput output) {
			Debug.Assert((uint)instruction.Code < (uint)instrInfos.Length);
			var instrInfo = instrInfos[(int)instruction.Code];
			instrInfo.GetOpInfo(options, instruction, out var opInfo);
			FormatOperands(instruction, output, opInfo);
		}

		/// <summary>
		/// Formats the whole instruction: prefixes, mnemonic, operands
		/// </summary>
		/// <param name="instruction">Instruction</param>
		/// <param name="output">Output</param>
		public override void Format(in Instruction instruction, FormatterOutput output) {
			Debug.Assert((uint)instruction.Code < (uint)instrInfos.Length);
			var instrInfo = instrInfos[(int)instruction.Code];
			instrInfo.GetOpInfo(options, instruction, out var opInfo);

			int column = 0;
			FormatMnemonic(instruction, output, opInfo, ref column, FormatMnemonicOptions.None);

			if (opInfo.OpCount != 0) {
				FormatterUtils.AddTabs(output, column, options.FirstOperandCharIndex, options.TabSize);
				FormatOperands(instruction, output, opInfo);
			}
		}

		void FormatMnemonic(in Instruction instruction, FormatterOutput output, in InstrOpInfo opInfo, ref int column, FormatMnemonicOptions mnemonicOptions) {
			if (output is null)
				ThrowHelper.ThrowArgumentNullException_output();
			bool needSpace = false;
			if ((mnemonicOptions & FormatMnemonicOptions.NoPrefixes) == 0 && (opInfo.Flags & InstrOpInfoFlags.MnemonicIsDirective) == 0) {
				var prefixSeg = instruction.SegmentPrefix;

				const uint PrefixFlags =
					((uint)InstrOpInfoFlags.SizeOverrideMask << (int)InstrOpInfoFlags.OpSizeShift) |
					((uint)InstrOpInfoFlags.SizeOverrideMask << (int)InstrOpInfoFlags.AddrSizeShift) |
					(uint)InstrOpInfoFlags.BndPrefix;
				if (((uint)prefixSeg | instruction.HasAnyOf_Lock_Rep_Repne_Prefix | ((uint)opInfo.Flags & PrefixFlags)) != 0) {
					FormatterString prefix;

					prefix = opSizeStrings[((int)opInfo.Flags >> (int)InstrOpInfoFlags.OpSizeShift) & (int)InstrOpInfoFlags.SizeOverrideMask];
					if (prefix.Length != 0)
						FormatPrefix(output, instruction, ref column, prefix, PrefixKind.OperandSize, ref needSpace);

					prefix = addrSizeStrings[((int)opInfo.Flags >> (int)InstrOpInfoFlags.AddrSizeShift) & (int)InstrOpInfoFlags.SizeOverrideMask];
					if (prefix.Length != 0)
						FormatPrefix(output, instruction, ref column, prefix, PrefixKind.AddressSize, ref needSpace);

					bool hasNoTrackPrefix = prefixSeg == Register.DS && FormatterUtils.IsNotrackPrefixBranch(instruction.Code);
					if (!hasNoTrackPrefix && prefixSeg != Register.None && ShowSegmentPrefix(instruction, opInfo))
						FormatPrefix(output, instruction, ref column, allRegisters[(int)prefixSeg], FormatterUtils.GetSegmentRegisterPrefixKind(prefixSeg), ref needSpace);

					if (instruction.HasXacquirePrefix)
						FormatPrefix(output, instruction, ref column, str_xacquire, PrefixKind.Xacquire, ref needSpace);
					if (instruction.HasXreleasePrefix)
						FormatPrefix(output, instruction, ref column, str_xrelease, PrefixKind.Xrelease, ref needSpace);
					if (instruction.HasLockPrefix)
						FormatPrefix(output, instruction, ref column, str_lock, PrefixKind.Lock, ref needSpace);

					if (hasNoTrackPrefix)
						FormatPrefix(output, instruction, ref column, str_notrack, PrefixKind.Notrack, ref needSpace);
					bool hasBnd = (opInfo.Flags & InstrOpInfoFlags.BndPrefix) != 0;
					if (hasBnd)
						FormatPrefix(output, instruction, ref column, str_bnd, PrefixKind.Bnd, ref needSpace);

					if (instruction.HasRepePrefix && FormatterUtils.ShowRepOrRepePrefix(instruction.Code, options)) {
						if (FormatterUtils.IsRepeOrRepneInstruction(instruction.Code))
							FormatPrefix(output, instruction, ref column, MnemonicCC.GetMnemonicCC(options, 4, str_repe), PrefixKind.Repe, ref needSpace);
						else
							FormatPrefix(output, instruction, ref column, str_rep, PrefixKind.Rep, ref needSpace);
					}
					if (instruction.HasRepnePrefix && !hasBnd && FormatterUtils.ShowRepnePrefix(instruction.Code, options))
						FormatPrefix(output, instruction, ref column, MnemonicCC.GetMnemonicCC(options, 5, str_repne), PrefixKind.Repne, ref needSpace);
				}
			}

			if ((mnemonicOptions & FormatMnemonicOptions.NoMnemonic) == 0) {
				if (needSpace) {
					output.Write(" ", FormatterTextKind.Text);
					column++;
				}
				var mnemonic = opInfo.Mnemonic;
				if ((opInfo.Flags & InstrOpInfoFlags.MnemonicIsDirective) != 0) {
					output.Write(mnemonic.Get(options.UppercaseKeywords || options.UppercaseAll), FormatterTextKind.Directive);
				}
				else {
					output.WriteMnemonic(instruction, mnemonic.Get(options.UppercaseMnemonics || options.UppercaseAll));
				}
				column += mnemonic.Length;
			}
		}

		bool ShowSegmentPrefix(in Instruction instruction, in InstrOpInfo opInfo) {
			if ((opInfo.Flags & (InstrOpInfoFlags.JccNotTaken | InstrOpInfoFlags.JccTaken)) != 0)
				return true;

			switch (instruction.Code) {
			case Code.Monitorw:
			case Code.Monitord:
			case Code.Monitorq:
			case Code.Monitorxw:
			case Code.Monitorxd:
			case Code.Monitorxq:
			case Code.Clzerow:
			case Code.Clzerod:
			case Code.Clzeroq:
			case Code.Umonitor_r16:
			case Code.Umonitor_r32:
			case Code.Umonitor_r64:
			case Code.Maskmovq_rDI_mm_mm:
			case Code.Maskmovdqu_rDI_xmm_xmm:
#if !NO_VEX
			case Code.VEX_Vmaskmovdqu_rDI_xmm_xmm:
#endif
			case Code.Xlat_m8:
			case Code.Outsb_DX_m8:
			case Code.Outsw_DX_m16:
			case Code.Outsd_DX_m32:
			case Code.Movsb_m8_m8:
			case Code.Movsw_m16_m16:
			case Code.Movsd_m32_m32:
			case Code.Movsq_m64_m64:
			case Code.Cmpsb_m8_m8:
			case Code.Cmpsw_m16_m16:
			case Code.Cmpsd_m32_m32:
			case Code.Cmpsq_m64_m64:
			case Code.Lodsb_AL_m8:
			case Code.Lodsw_AX_m16:
			case Code.Lodsd_EAX_m32:
			case Code.Lodsq_RAX_m64:
				return FormatterUtils.ShowSegmentPrefix(Register.DS, instruction, options);

			default:
				break;
			}

			for (int i = 0; i < opInfo.OpCount; i++) {
				switch (opInfo.GetOpKind(i)) {
				case InstrOpKind.Register:
				case InstrOpKind.NearBranch16:
				case InstrOpKind.NearBranch32:
				case InstrOpKind.NearBranch64:
				case InstrOpKind.FarBranch16:
				case InstrOpKind.FarBranch32:
				case InstrOpKind.Immediate8:
				case InstrOpKind.Immediate8_2nd:
				case InstrOpKind.Immediate16:
				case InstrOpKind.Immediate32:
				case InstrOpKind.Immediate64:
				case InstrOpKind.Immediate8to16:
				case InstrOpKind.Immediate8to32:
				case InstrOpKind.Immediate8to64:
				case InstrOpKind.Immediate32to64:
				case InstrOpKind.MemoryESDI:
				case InstrOpKind.MemoryESEDI:
				case InstrOpKind.MemoryESRDI:
				case InstrOpKind.Sae:
				case InstrOpKind.RnSae:
				case InstrOpKind.RdSae:
				case InstrOpKind.RuSae:
				case InstrOpKind.RzSae:
				case InstrOpKind.Rn:
				case InstrOpKind.Rd:
				case InstrOpKind.Ru:
				case InstrOpKind.Rz:
				case InstrOpKind.DeclareByte:
				case InstrOpKind.DeclareWord:
				case InstrOpKind.DeclareDword:
				case InstrOpKind.DeclareQword:
					break;

				case InstrOpKind.MemorySegSI:
				case InstrOpKind.MemorySegESI:
				case InstrOpKind.MemorySegRSI:
				case InstrOpKind.MemorySegDI:
				case InstrOpKind.MemorySegEDI:
				case InstrOpKind.MemorySegRDI:
				case InstrOpKind.Memory:
					return false;

				default:
					throw new InvalidOperationException();
				}
			}
			return options.ShowUselessPrefixes;
		}

		void FormatPrefix(FormatterOutput output, in Instruction instruction, ref int column, FormatterString prefix, PrefixKind prefixKind, ref bool needSpace) {
			if (needSpace) {
				column++;
				output.Write(" ", FormatterTextKind.Text);
			}
			output.WritePrefix(instruction, prefix.Get(options.UppercasePrefixes || options.UppercaseAll), prefixKind);
			column += prefix.Length;
			needSpace = true;
		}

		void FormatOperands(in Instruction instruction, FormatterOutput output, in InstrOpInfo opInfo) {
			if (output is null)
				ThrowHelper.ThrowArgumentNullException_output();
			for (int i = 0; i < opInfo.OpCount; i++) {
				if (i > 0) {
					output.Write(",", FormatterTextKind.Punctuation);
					if (options.SpaceAfterOperandSeparator)
						output.Write(" ", FormatterTextKind.Text);
				}
				FormatOperand(instruction, output, opInfo, i);
			}
		}

		void FormatOperand(in Instruction instruction, FormatterOutput output, in InstrOpInfo opInfo, int operand) {
			Debug.Assert((uint)operand < (uint)opInfo.OpCount);
			if (output is null)
				ThrowHelper.ThrowArgumentNullException_output();

#if MVEX
			int mvexRmOperand;
			if (IcedConstants.IsMvex(instruction.Code)) {
				var opCount = instruction.OpCount;
				Debug.Assert(opCount != 0);
				mvexRmOperand = instruction.GetOpKind(opCount - 1) == OpKind.Immediate8 ? opCount - 2 : opCount - 1;
			}
			else
				mvexRmOperand = -1;
#endif
			int instructionOperand = opInfo.GetInstructionIndex(operand);

			string s;
			FormatterFlowControl flowControl;
			byte imm8;
			ushort imm16;
			uint imm32;
			ulong imm64, value64;
			int immSize;
			NumberFormattingOptions numberOptions;
			SymbolResult symbol;
			ISymbolResolver? symbolResolver;
			FormatterOperandOptions operandOptions;
			NumberKind numberKind;
			var opKind = opInfo.GetOpKind(operand);
			switch (opKind) {
			case InstrOpKind.Register:
				if ((opInfo.Flags & InstrOpInfoFlags.RegisterTo) != 0) {
					FormatKeyword(output, str_to);
					output.Write(" ", FormatterTextKind.Text);
				}
				FormatRegister(output, instruction, operand, instructionOperand, opInfo.GetOpRegister(operand));
				break;

			case InstrOpKind.NearBranch16:
			case InstrOpKind.NearBranch32:
			case InstrOpKind.NearBranch64:
				if (opKind == InstrOpKind.NearBranch64) {
					immSize = 8;
					imm64 = instruction.NearBranch64;
					numberKind = NumberKind.UInt64;
				}
				else if (opKind == InstrOpKind.NearBranch32) {
					immSize = 4;
					imm64 = instruction.NearBranch32;
					numberKind = NumberKind.UInt32;
				}
				else {
					immSize = 2;
					imm64 = instruction.NearBranch16;
					numberKind = NumberKind.UInt16;
				}
				numberOptions = NumberFormattingOptions.CreateBranchInternal(options);
				operandOptions = new FormatterOperandOptions(options.ShowBranchSize ? FormatterOperandOptions.Flags.None : FormatterOperandOptions.Flags.NoBranchSize);
				optionsProvider?.GetOperandOptions(instruction, operand, instructionOperand, ref operandOptions, ref numberOptions);
				if ((symbolResolver = this.symbolResolver) is not null && symbolResolver.TryGetSymbol(instruction, operand, instructionOperand, imm64, immSize, out symbol)) {
					FormatFlowControl(output, opInfo.Flags, operandOptions);
					output.Write(instruction, operand, instructionOperand, options, numberFormatter, numberOptions, imm64, symbol, options.ShowSymbolAddress);
				}
				else {
					operandOptions = new FormatterOperandOptions(options.ShowBranchSize ? FormatterOperandOptions.Flags.None : FormatterOperandOptions.Flags.NoBranchSize);
					optionsProvider?.GetOperandOptions(instruction, operand, instructionOperand, ref operandOptions, ref numberOptions);
					flowControl = FormatterUtils.GetFlowControl(instruction);
					FormatFlowControl(output, opInfo.Flags, operandOptions);
					if (opKind == InstrOpKind.NearBranch32)
						s = numberFormatter.FormatUInt32(options, numberOptions, instruction.NearBranch32, numberOptions.LeadingZeros);
					else if (opKind == InstrOpKind.NearBranch64)
						s = numberFormatter.FormatUInt64(options, numberOptions, instruction.NearBranch64, numberOptions.LeadingZeros);
					else
						s = numberFormatter.FormatUInt16(options, numberOptions, instruction.NearBranch16, numberOptions.LeadingZeros);
					output.WriteNumber(instruction, operand, instructionOperand, s, imm64, numberKind, FormatterUtils.IsCall(flowControl) ? FormatterTextKind.FunctionAddress : FormatterTextKind.LabelAddress);
				}
				break;

			case InstrOpKind.FarBranch16:
			case InstrOpKind.FarBranch32:
				if (opKind == InstrOpKind.FarBranch32) {
					immSize = 4;
					imm64 = instruction.FarBranch32;
					numberKind = NumberKind.UInt32;
				}
				else {
					immSize = 2;
					imm64 = instruction.FarBranch16;
					numberKind = NumberKind.UInt16;
				}
				numberOptions = NumberFormattingOptions.CreateBranchInternal(options);
				operandOptions = new FormatterOperandOptions(options.ShowBranchSize ? FormatterOperandOptions.Flags.None : FormatterOperandOptions.Flags.NoBranchSize);
				optionsProvider?.GetOperandOptions(instruction, operand, instructionOperand, ref operandOptions, ref numberOptions);
				if ((symbolResolver = this.symbolResolver) is not null && symbolResolver.TryGetSymbol(instruction, operand, instructionOperand, (uint)imm64, immSize, out symbol)) {
					FormatFlowControl(output, opInfo.Flags, operandOptions);
					Debug.Assert(operand + 1 == 1);
					if (!symbolResolver.TryGetSymbol(instruction, operand + 1, instructionOperand, instruction.FarBranchSelector, 2, out var selectorSymbol)) {
						s = numberFormatter.FormatUInt16(options, numberOptions, instruction.FarBranchSelector, numberOptions.LeadingZeros);
						output.WriteNumber(instruction, operand, instructionOperand, s, instruction.FarBranchSelector, NumberKind.UInt16, FormatterTextKind.SelectorValue);
					}
					else
						output.Write(instruction, operand, instructionOperand, options, numberFormatter, numberOptions, instruction.FarBranchSelector, selectorSymbol, options.ShowSymbolAddress);
					output.Write(":", FormatterTextKind.Punctuation);
					output.Write(instruction, operand, instructionOperand, options, numberFormatter, numberOptions, imm64, symbol, options.ShowSymbolAddress);
				}
				else {
					flowControl = FormatterUtils.GetFlowControl(instruction);
					FormatFlowControl(output, opInfo.Flags, operandOptions);
					s = numberFormatter.FormatUInt16(options, numberOptions, instruction.FarBranchSelector, numberOptions.LeadingZeros);
					output.WriteNumber(instruction, operand, instructionOperand, s, instruction.FarBranchSelector, NumberKind.UInt16, FormatterTextKind.SelectorValue);
					output.Write(":", FormatterTextKind.Punctuation);
					if (opKind == InstrOpKind.FarBranch32)
						s = numberFormatter.FormatUInt32(options, numberOptions, instruction.FarBranch32, numberOptions.LeadingZeros);
					else
						s = numberFormatter.FormatUInt16(options, numberOptions, instruction.FarBranch16, numberOptions.LeadingZeros);
					output.WriteNumber(instruction, operand, instructionOperand, s, imm64, numberKind, FormatterUtils.IsCall(flowControl) ? FormatterTextKind.FunctionAddress : FormatterTextKind.LabelAddress);
				}
				break;

			case InstrOpKind.Immediate8:
			case InstrOpKind.Immediate8_2nd:
			case InstrOpKind.DeclareByte:
				if (opKind == InstrOpKind.Immediate8)
					imm8 = instruction.Immediate8;
				else if (opKind == InstrOpKind.Immediate8_2nd)
					imm8 = instruction.Immediate8_2nd;
				else
					imm8 = instruction.GetDeclareByteValue(operand);
				numberOptions = NumberFormattingOptions.CreateImmediateInternal(options);
				operandOptions = default;
				optionsProvider?.GetOperandOptions(instruction, operand, instructionOperand, ref operandOptions, ref numberOptions);
				if ((symbolResolver = this.symbolResolver) is not null && symbolResolver.TryGetSymbol(instruction, operand, instructionOperand, imm8, 1, out symbol))
					output.Write(instruction, operand, instructionOperand, options, numberFormatter, numberOptions, imm8, symbol, options.ShowSymbolAddress);
				else {
					if (numberOptions.SignedNumber) {
						imm64 = (ulong)(sbyte)imm8;
						numberKind = NumberKind.Int8;
						if ((sbyte)imm8 < 0) {
							output.Write("-", FormatterTextKind.Operator);
							imm8 = (byte)-(sbyte)imm8;
						}
					}
					else {
						imm64 = imm8;
						numberKind = NumberKind.UInt8;
					}
					s = numberFormatter.FormatUInt8(options, numberOptions, imm8);
					output.WriteNumber(instruction, operand, instructionOperand, s, imm64, numberKind, FormatterTextKind.Number);
				}
				break;

			case InstrOpKind.Immediate16:
			case InstrOpKind.Immediate8to16:
			case InstrOpKind.DeclareWord:
				ShowSignExtendInfo(output, opInfo.Flags);
				if (opKind == InstrOpKind.Immediate16)
					imm16 = instruction.Immediate16;
				else if (opKind == InstrOpKind.Immediate8to16)
					imm16 = (ushort)instruction.Immediate8to16;
				else
					imm16 = instruction.GetDeclareWordValue(operand);
				numberOptions = NumberFormattingOptions.CreateImmediateInternal(options);
				operandOptions = default;
				optionsProvider?.GetOperandOptions(instruction, operand, instructionOperand, ref operandOptions, ref numberOptions);
				if ((symbolResolver = this.symbolResolver) is not null && symbolResolver.TryGetSymbol(instruction, operand, instructionOperand, imm16, 2, out symbol))
					output.Write(instruction, operand, instructionOperand, options, numberFormatter, numberOptions, imm16, symbol, options.ShowSymbolAddress);
				else {
					if (numberOptions.SignedNumber) {
						imm64 = (ulong)(short)imm16;
						numberKind = NumberKind.Int16;
						if ((short)imm16 < 0) {
							output.Write("-", FormatterTextKind.Operator);
							imm16 = (ushort)-(short)imm16;
						}
					}
					else {
						imm64 = imm16;
						numberKind = NumberKind.UInt16;
					}
					s = numberFormatter.FormatUInt16(options, numberOptions, imm16);
					output.WriteNumber(instruction, operand, instructionOperand, s, imm64, numberKind, FormatterTextKind.Number);
				}
				break;

			case InstrOpKind.Immediate32:
			case InstrOpKind.Immediate8to32:
			case InstrOpKind.DeclareDword:
				ShowSignExtendInfo(output, opInfo.Flags);
				if (opKind == InstrOpKind.Immediate32)
					imm32 = instruction.Immediate32;
				else if (opKind == InstrOpKind.Immediate8to32)
					imm32 = (uint)instruction.Immediate8to32;
				else
					imm32 = instruction.GetDeclareDwordValue(operand);
				numberOptions = NumberFormattingOptions.CreateImmediateInternal(options);
				operandOptions = default;
				optionsProvider?.GetOperandOptions(instruction, operand, instructionOperand, ref operandOptions, ref numberOptions);
				if ((symbolResolver = this.symbolResolver) is not null && symbolResolver.TryGetSymbol(instruction, operand, instructionOperand, imm32, 4, out symbol))
					output.Write(instruction, operand, instructionOperand, options, numberFormatter, numberOptions, imm32, symbol, options.ShowSymbolAddress);
				else {
					if (numberOptions.SignedNumber) {
						imm64 = (ulong)(int)imm32;
						numberKind = NumberKind.Int32;
						if ((int)imm32 < 0) {
							output.Write("-", FormatterTextKind.Operator);
							imm32 = (uint)-(int)imm32;
						}
					}
					else {
						imm64 = imm32;
						numberKind = NumberKind.UInt32;
					}
					s = numberFormatter.FormatUInt32(options, numberOptions, imm32);
					output.WriteNumber(instruction, operand, instructionOperand, s, imm64, numberKind, FormatterTextKind.Number);
				}
				break;

			case InstrOpKind.Immediate64:
			case InstrOpKind.Immediate8to64:
			case InstrOpKind.Immediate32to64:
			case InstrOpKind.DeclareQword:
				ShowSignExtendInfo(output, opInfo.Flags);
				if (opKind == InstrOpKind.Immediate32to64)
					imm64 = (ulong)instruction.Immediate32to64;
				else if (opKind == InstrOpKind.Immediate8to64)
					imm64 = (ulong)instruction.Immediate8to64;
				else if (opKind == InstrOpKind.Immediate64)
					imm64 = instruction.Immediate64;
				else
					imm64 = instruction.GetDeclareQwordValue(operand);
				numberOptions = NumberFormattingOptions.CreateImmediateInternal(options);
				operandOptions = default;
				optionsProvider?.GetOperandOptions(instruction, operand, instructionOperand, ref operandOptions, ref numberOptions);
				if ((symbolResolver = this.symbolResolver) is not null && symbolResolver.TryGetSymbol(instruction, operand, instructionOperand, imm64, 8, out symbol))
					output.Write(instruction, operand, instructionOperand, options, numberFormatter, numberOptions, imm64, symbol, options.ShowSymbolAddress);
				else {
					value64 = imm64;
					if (numberOptions.SignedNumber) {
						numberKind = NumberKind.Int64;
						if ((long)imm64 < 0) {
							output.Write("-", FormatterTextKind.Operator);
							imm64 = (ulong)-(long)imm64;
						}
					}
					else
						numberKind = NumberKind.UInt64;
					s = numberFormatter.FormatUInt64(options, numberOptions, imm64);
					output.WriteNumber(instruction, operand, instructionOperand, s, value64, numberKind, FormatterTextKind.Number);
				}
				break;

			case InstrOpKind.MemorySegSI:
				FormatMemory(output, instruction, operand, instructionOperand, opInfo.MemorySize, instruction.MemorySegment, Register.SI, Register.None, 0, 0, 0, 2, opInfo.Flags);
				break;

			case InstrOpKind.MemorySegESI:
				FormatMemory(output, instruction, operand, instructionOperand, opInfo.MemorySize, instruction.MemorySegment, Register.ESI, Register.None, 0, 0, 0, 4, opInfo.Flags);
				break;

			case InstrOpKind.MemorySegRSI:
				FormatMemory(output, instruction, operand, instructionOperand, opInfo.MemorySize, instruction.MemorySegment, Register.RSI, Register.None, 0, 0, 0, 8, opInfo.Flags);
				break;

			case InstrOpKind.MemorySegDI:
				FormatMemory(output, instruction, operand, instructionOperand, opInfo.MemorySize, instruction.MemorySegment, Register.DI, Register.None, 0, 0, 0, 2, opInfo.Flags);
				break;

			case InstrOpKind.MemorySegEDI:
				FormatMemory(output, instruction, operand, instructionOperand, opInfo.MemorySize, instruction.MemorySegment, Register.EDI, Register.None, 0, 0, 0, 4, opInfo.Flags);
				break;

			case InstrOpKind.MemorySegRDI:
				FormatMemory(output, instruction, operand, instructionOperand, opInfo.MemorySize, instruction.MemorySegment, Register.RDI, Register.None, 0, 0, 0, 8, opInfo.Flags);
				break;

			case InstrOpKind.MemoryESDI:
				FormatMemory(output, instruction, operand, instructionOperand, opInfo.MemorySize, Register.ES, Register.DI, Register.None, 0, 0, 0, 2, opInfo.Flags);
				break;

			case InstrOpKind.MemoryESEDI:
				FormatMemory(output, instruction, operand, instructionOperand, opInfo.MemorySize, Register.ES, Register.EDI, Register.None, 0, 0, 0, 4, opInfo.Flags);
				break;

			case InstrOpKind.MemoryESRDI:
				FormatMemory(output, instruction, operand, instructionOperand, opInfo.MemorySize, Register.ES, Register.RDI, Register.None, 0, 0, 0, 8, opInfo.Flags);
				break;

			case InstrOpKind.Memory:
				int displSize = instruction.MemoryDisplSize;
				var baseReg = instruction.MemoryBase;
				var indexReg = instruction.MemoryIndex;
				int addrSize = InstructionUtils.GetAddressSizeInBytes(baseReg, indexReg, displSize, instruction.CodeSize);
				long displ;
				if (addrSize == 8)
					displ = (long)instruction.MemoryDisplacement64;
				else
					displ = instruction.MemoryDisplacement32;
				FormatMemory(output, instruction, operand, instructionOperand, opInfo.MemorySize, instruction.MemorySegment, baseReg, indexReg, instruction.InternalMemoryIndexScale, displSize, displ, addrSize, opInfo.Flags);
				break;

			case InstrOpKind.Sae:
				FormatDecorator(output, instruction, operand, instructionOperand, str_sae, DecoratorKind.SuppressAllExceptions);
				break;

			case InstrOpKind.RnSae:
				FormatDecorator(output, instruction, operand, instructionOperand, str_rn_sae, DecoratorKind.RoundingControl);
				break;

			case InstrOpKind.RdSae:
				FormatDecorator(output, instruction, operand, instructionOperand, str_rd_sae, DecoratorKind.RoundingControl);
				break;

			case InstrOpKind.RuSae:
				FormatDecorator(output, instruction, operand, instructionOperand, str_ru_sae, DecoratorKind.RoundingControl);
				break;

			case InstrOpKind.RzSae:
				FormatDecorator(output, instruction, operand, instructionOperand, str_rz_sae, DecoratorKind.RoundingControl);
				break;

			case InstrOpKind.Rn:
				FormatDecorator(output, instruction, operand, instructionOperand, str_rn, DecoratorKind.RoundingControl);
				break;

			case InstrOpKind.Rd:
				FormatDecorator(output, instruction, operand, instructionOperand, str_rd, DecoratorKind.RoundingControl);
				break;

			case InstrOpKind.Ru:
				FormatDecorator(output, instruction, operand, instructionOperand, str_ru, DecoratorKind.RoundingControl);
				break;

			case InstrOpKind.Rz:
				FormatDecorator(output, instruction, operand, instructionOperand, str_rz, DecoratorKind.RoundingControl);
				break;

			default:
				throw new InvalidOperationException();
			}

			if (operand == 0 && instruction.HasOpMask_or_ZeroingMasking) {
				if (instruction.HasOpMask) {
					output.Write("{", FormatterTextKind.Punctuation);
					FormatRegister(output, instruction, operand, instructionOperand, instruction.OpMask);
					output.Write("}", FormatterTextKind.Punctuation);
				}
				if (instruction.ZeroingMasking)
					FormatDecorator(output, instruction, operand, instructionOperand, str_z, DecoratorKind.ZeroingMasking);
			}
#if MVEX
			if (mvexRmOperand == operand) {
				var conv = instruction.MvexRegMemConv;
				if (conv != MvexRegMemConv.None) {
					var mvex = new MvexInfo(instruction.Code);
					if (mvex.ConvFn != MvexConvFn.None) {
						var tbl = mvex.IsConvFn32 ? mvexRegMemConsts32 : mvexRegMemConsts64;
						var fs = tbl[(int)conv];
						if (fs.Length != 0)
							FormatDecorator(output, instruction, operand, instructionOperand, fs, DecoratorKind.SwizzleMemConv);
					}
				}
			}
#endif
		}

		void ShowSignExtendInfo(FormatterOutput output, InstrOpInfoFlags flags) {
			if (!options.NasmShowSignExtendedImmediateSize)
				return;

			FormatterString keyword;
			switch ((SignExtendInfo)(((int)flags >> (int)InstrOpInfoFlags.SignExtendInfoShift) & (int)InstrOpInfoFlags.SignExtendInfoMask)) {
			case SignExtendInfo.None:
				return;

			case SignExtendInfo.Sex1to2:
			case SignExtendInfo.Sex1to4:
			case SignExtendInfo.Sex1to8:
				keyword = str_byte;
				break;

			case SignExtendInfo.Sex2:
				keyword = str_word;
				break;

			case SignExtendInfo.Sex4:
				keyword = str_dword;
				break;

			case SignExtendInfo.Sex4to8:
				keyword = str_qword;
				break;

			default:
				throw new InvalidOperationException();
			}

			FormatKeyword(output, keyword);
			output.Write(" ", FormatterTextKind.Text);
		}

		void FormatFlowControl(FormatterOutput output, InstrOpInfoFlags flags, FormatterOperandOptions operandOptions) {
			if (!operandOptions.BranchSize)
				return;
			var keywords = branchInfos[((int)flags >> (int)InstrOpInfoFlags.BranchSizeInfoShift) & (int)InstrOpInfoFlags.BranchSizeInfoMask];
			if (keywords is null)
				return;
			foreach (var keyword in keywords) {
				FormatKeyword(output, keyword);
				output.Write(" ", FormatterTextKind.Text);
			}
		}

		void FormatDecorator(FormatterOutput output, in Instruction instruction, int operand, int instructionOperand, FormatterString text, DecoratorKind decorator) {
			output.Write("{", FormatterTextKind.Punctuation);
			output.WriteDecorator(instruction, operand, instructionOperand, text.Get(options.UppercaseDecorators || options.UppercaseAll), decorator);
			output.Write("}", FormatterTextKind.Punctuation);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		string ToRegisterString(Register reg) {
			Debug.Assert((uint)reg < (uint)allRegisters.Length);
			var regStr = allRegisters[(int)reg];
			return regStr.Get(options.UppercaseRegisters || options.UppercaseAll);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		void FormatRegister(FormatterOutput output, in Instruction instruction, int operand, int instructionOperand, Register reg) =>
			output.WriteRegister(instruction, operand, instructionOperand, ToRegisterString(reg), (Register)reg);

		void FormatMemory(FormatterOutput output, in Instruction instruction, int operand, int instructionOperand, MemorySize memSize, Register segReg, Register baseReg, Register indexReg, int scale, int displSize, long displ, int addrSize, InstrOpInfoFlags flags) {
			Debug.Assert((uint)scale < (uint)scaleNumbers.Length);
			Debug.Assert(InstructionUtils.GetAddressSizeInBytes(baseReg, indexReg, displSize, instruction.CodeSize) == addrSize);

			var numberOptions = NumberFormattingOptions.CreateDisplacementInternal(options);
			SymbolResult symbol;
			bool useSymbol;

			var operandOptions = new FormatterOperandOptions(options.MemorySizeOptions);
			operandOptions.RipRelativeAddresses = options.RipRelativeAddresses;
			optionsProvider?.GetOperandOptions(instruction, operand, instructionOperand, ref operandOptions, ref numberOptions);

			ulong absAddr;
			bool addRelKeyword = false;
			if (baseReg == Register.RIP) {
				absAddr = (ulong)displ;
				if (options.RipRelativeAddresses)
					displ -= (long)instruction.NextIP;
				else {
					Debug.Assert(indexReg == Register.None);
					baseReg = Register.None;
					flags &= ~(InstrOpInfoFlags)((uint)InstrOpInfoFlags.MemorySizeInfoMask << (int)InstrOpInfoFlags.MemorySizeInfoShift);
					addRelKeyword = true;
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
					flags = (flags & ~(InstrOpInfoFlags)((uint)InstrOpInfoFlags.MemorySizeInfoMask << (int)InstrOpInfoFlags.MemorySizeInfoShift)) | (InstrOpInfoFlags)((int)NasmFormatterInternal.MemorySizeInfo.Dword << (int)InstrOpInfoFlags.MemorySizeInfoShift);
					addRelKeyword = true;
				}
				displSize = 4;
			}
			else
				absAddr = (ulong)displ;

			if (this.symbolResolver is ISymbolResolver symbolResolver)
				useSymbol = symbolResolver.TryGetSymbol(instruction, operand, instructionOperand, absAddr, addrSize, out symbol);
			else {
				useSymbol = false;
				symbol = default;
			}

			bool useScale = scale != 0 || options.AlwaysShowScale;
			if (!useScale) {
				// [rsi] = base reg, [rsi*1] = index reg
				if (baseReg == Register.None)
					useScale = true;
			}
			if (addrSize == 2 || !FormatterUtils.ShowIndexScale(instruction, options))
				useScale = false;

			FormatMemorySize(output, memSize, flags, operandOptions);

			output.Write("[", FormatterTextKind.Punctuation);
			if (options.SpaceAfterMemoryBracket)
				output.Write(" ", FormatterTextKind.Text);

			var memSizeName = memSizeInfos[((int)flags >> (int)InstrOpInfoFlags.MemorySizeInfoShift) & (int)InstrOpInfoFlags.MemorySizeInfoMask];
			if (memSizeName.Length != 0) {
				FormatKeyword(output, memSizeName);
				output.Write(" ", FormatterTextKind.Text);
			}

			if (addRelKeyword) {
				FormatKeyword(output, str_rel);
				output.Write(" ", FormatterTextKind.Text);
			}

			var codeSize = instruction.CodeSize;
			var segOverride = instruction.SegmentPrefix;
			bool noTrackPrefix = segOverride == Register.DS && FormatterUtils.IsNotrackPrefixBranch(instruction.Code) &&
				!((codeSize == CodeSize.Code16 || codeSize == CodeSize.Code32) && (baseReg == Register.BP || baseReg == Register.EBP || baseReg == Register.ESP));
			if (options.AlwaysShowSegmentRegister || (segOverride != Register.None && !noTrackPrefix && FormatterUtils.ShowSegmentPrefix(Register.None, instruction, options))) {
				FormatRegister(output, instruction, operand, instructionOperand, segReg);
				output.Write(":", FormatterTextKind.Punctuation);
			}

			bool needPlus = false;
			if (baseReg != Register.None) {
				FormatRegister(output, instruction, operand, instructionOperand, baseReg);
				needPlus = true;
			}

			if (indexReg != Register.None) {
				if (needPlus) {
					if (options.SpaceBetweenMemoryAddOperators)
						output.Write(" ", FormatterTextKind.Text);
					output.Write("+", FormatterTextKind.Operator);
					if (options.SpaceBetweenMemoryAddOperators)
						output.Write(" ", FormatterTextKind.Text);
				}
				needPlus = true;

				if (!useScale)
					FormatRegister(output, instruction, operand, instructionOperand, indexReg);
				else if (options.ScaleBeforeIndex) {
					output.WriteNumber(instruction, operand, instructionOperand, scaleNumbers[scale], 1U << scale, NumberKind.Int32, FormatterTextKind.Number);
					if (options.SpaceBetweenMemoryMulOperators)
						output.Write(" ", FormatterTextKind.Text);
					output.Write("*", FormatterTextKind.Operator);
					if (options.SpaceBetweenMemoryMulOperators)
						output.Write(" ", FormatterTextKind.Text);
					FormatRegister(output, instruction, operand, instructionOperand, indexReg);
				}
				else {
					FormatRegister(output, instruction, operand, instructionOperand, indexReg);
					if (options.SpaceBetweenMemoryMulOperators)
						output.Write(" ", FormatterTextKind.Text);
					output.Write("*", FormatterTextKind.Operator);
					if (options.SpaceBetweenMemoryMulOperators)
						output.Write(" ", FormatterTextKind.Text);
					output.WriteNumber(instruction, operand, instructionOperand, scaleNumbers[scale], 1U << scale, NumberKind.Int32, FormatterTextKind.Number);
				}
			}

			if (useSymbol) {
				if (needPlus) {
					if (options.SpaceBetweenMemoryAddOperators)
						output.Write(" ", FormatterTextKind.Text);
					if ((symbol.Flags & SymbolFlags.Signed) != 0)
						output.Write("-", FormatterTextKind.Operator);
					else
						output.Write("+", FormatterTextKind.Operator);
					if (options.SpaceBetweenMemoryAddOperators)
						output.Write(" ", FormatterTextKind.Text);
				}
				else if ((symbol.Flags & SymbolFlags.Signed) != 0)
					output.Write("-", FormatterTextKind.Operator);

				output.Write(instruction, operand, instructionOperand, options, numberFormatter, numberOptions, absAddr, symbol, options.ShowSymbolAddress, false, options.SpaceBetweenMemoryAddOperators);
			}
			else if (!needPlus || (displSize != 0 && (options.ShowZeroDisplacements || displ != 0))) {
				ulong origDispl = (ulong)displ;
				bool isSigned;
				if (needPlus) {
					isSigned = numberOptions.SignedNumber;
					if (options.SpaceBetweenMemoryAddOperators)
						output.Write(" ", FormatterTextKind.Text);

					if (addrSize == 8) {
						if (!numberOptions.SignedNumber)
							output.Write("+", FormatterTextKind.Operator);
						else if (displ < 0) {
							displ = -displ;
							output.Write("-", FormatterTextKind.Operator);
						}
						else
							output.Write("+", FormatterTextKind.Operator);
						if (numberOptions.DisplacementLeadingZeros) {
							displSize = 4;
						}
					}
					else if (addrSize == 4) {
						if (!numberOptions.SignedNumber)
							output.Write("+", FormatterTextKind.Operator);
						else if ((int)displ < 0) {
							displ = (uint)-(int)displ;
							output.Write("-", FormatterTextKind.Operator);
						}
						else
							output.Write("+", FormatterTextKind.Operator);
						if (numberOptions.DisplacementLeadingZeros) {
							displSize = 4;
						}
					}
					else {
						Debug.Assert(addrSize == 2);
						if (!numberOptions.SignedNumber)
							output.Write("+", FormatterTextKind.Operator);
						else if ((short)displ < 0) {
							displ = (ushort)-(short)displ;
							output.Write("-", FormatterTextKind.Operator);
						}
						else
							output.Write("+", FormatterTextKind.Operator);
						if (numberOptions.DisplacementLeadingZeros) {
							displSize = 2;
						}
					}
					if (options.SpaceBetweenMemoryAddOperators)
						output.Write(" ", FormatterTextKind.Text);
				}
				else
					isSigned = false;

				NumberKind displKind;
				string s;
				if (displSize <= 1 && (ulong)displ <= byte.MaxValue) {
					s = numberFormatter.FormatDisplUInt8(options, numberOptions, (byte)displ);
					displKind = isSigned ? NumberKind.Int8 : NumberKind.UInt8;
				}
				else if (displSize <= 2 && (ulong)displ <= ushort.MaxValue) {
					s = numberFormatter.FormatDisplUInt16(options, numberOptions, (ushort)displ);
					displKind = isSigned ? NumberKind.Int16 : NumberKind.UInt16;
				}
				else if (displSize <= 4 && (ulong)displ <= uint.MaxValue) {
					s = numberFormatter.FormatDisplUInt32(options, numberOptions, (uint)displ);
					displKind = isSigned ? NumberKind.Int32 : NumberKind.UInt32;
				}
				else if (displSize <= 8) {
					s = numberFormatter.FormatDisplUInt64(options, numberOptions, (ulong)displ);
					displKind = isSigned ? NumberKind.Int64 : NumberKind.UInt64;
				}
				else
					throw new InvalidOperationException();
				output.WriteNumber(instruction, operand, instructionOperand, s, origDispl, displKind, FormatterTextKind.Number);
			}

			if (options.SpaceAfterMemoryBracket)
				output.Write(" ", FormatterTextKind.Text);
			output.Write("]", FormatterTextKind.Punctuation);

			Debug.Assert((uint)memSize < (uint)allMemorySizes.Length);
			var bcstTo = allMemorySizes[(int)memSize].bcstTo;
			if (bcstTo.Length != 0)
				FormatDecorator(output, instruction, operand, instructionOperand, bcstTo, DecoratorKind.Broadcast);
#if MVEX
			if (instruction.IsMvexEvictionHint)
				FormatDecorator(output, instruction, operand, instructionOperand, str_eh, DecoratorKind.EvictionHint);
#endif
		}

		void FormatMemorySize(FormatterOutput output, MemorySize memSize, InstrOpInfoFlags flags, FormatterOperandOptions operandOptions) {
			var memSizeOptions = operandOptions.MemorySizeOptions;
			if (memSizeOptions == MemorySizeOptions.Never)
				return;

			if ((flags & InstrOpInfoFlags.MemSize_Nothing) != 0)
				return;

			Debug.Assert((uint)memSize < (uint)allMemorySizes.Length);
			var memInfo = allMemorySizes[(int)memSize];
			var keyword = memInfo.keyword;
			if (keyword.Length == 0)
				return;

			if (memSizeOptions == MemorySizeOptions.Default) {
				if ((flags & InstrOpInfoFlags.ShowNoMemSize_ForceSize) == 0)
					return;
			}
			else if (memSizeOptions == MemorySizeOptions.Minimal) {
				if ((flags & InstrOpInfoFlags.ShowMinMemSize_ForceSize) == 0)
					return;
			}
			else
				Debug.Assert(memSizeOptions == MemorySizeOptions.Always);

			var farKind = farMemSizeInfos[((int)flags >> (int)InstrOpInfoFlags.FarMemorySizeInfoShift) & (int)InstrOpInfoFlags.FarMemorySizeInfoMask];
			if (farKind.Length != 0) {
				FormatKeyword(output, farKind);
				output.Write(" ", FormatterTextKind.Text);
			}
			FormatKeyword(output, keyword);
			output.Write(" ", FormatterTextKind.Text);
		}

		void FormatKeyword(FormatterOutput output, FormatterString keyword) =>
			output.Write(keyword.Get(options.UppercaseKeywords || options.UppercaseAll), FormatterTextKind.Keyword);

		/// <summary>
		/// Formats a register
		/// </summary>
		/// <param name="register">Register</param>
		/// <returns></returns>
		public override string Format(Register register) => ToRegisterString(register);

		/// <summary>
		/// Formats a <see cref="sbyte"/>
		/// </summary>
		/// <param name="value">Value</param>
		/// <param name="numberOptions">Options</param>
		/// <returns></returns>
		public override string FormatInt8(sbyte value, in NumberFormattingOptions numberOptions) =>
			numberFormatter.FormatInt8(options, numberOptions, value);

		/// <summary>
		/// Formats a <see cref="short"/>
		/// </summary>
		/// <param name="value">Value</param>
		/// <param name="numberOptions">Options</param>
		/// <returns></returns>
		public override string FormatInt16(short value, in NumberFormattingOptions numberOptions) =>
			numberFormatter.FormatInt16(options, numberOptions, value);

		/// <summary>
		/// Formats a <see cref="int"/>
		/// </summary>
		/// <param name="value">Value</param>
		/// <param name="numberOptions">Options</param>
		/// <returns></returns>
		public override string FormatInt32(int value, in NumberFormattingOptions numberOptions) =>
			numberFormatter.FormatInt32(options, numberOptions, value);

		/// <summary>
		/// Formats a <see cref="long"/>
		/// </summary>
		/// <param name="value">Value</param>
		/// <param name="numberOptions">Options</param>
		/// <returns></returns>
		public override string FormatInt64(long value, in NumberFormattingOptions numberOptions) =>
			numberFormatter.FormatInt64(options, numberOptions, value);

		/// <summary>
		/// Formats a <see cref="byte"/>
		/// </summary>
		/// <param name="value">Value</param>
		/// <param name="numberOptions">Options</param>
		/// <returns></returns>
		public override string FormatUInt8(byte value, in NumberFormattingOptions numberOptions) =>
			numberFormatter.FormatUInt8(options, numberOptions, value);

		/// <summary>
		/// Formats a <see cref="ushort"/>
		/// </summary>
		/// <param name="value">Value</param>
		/// <param name="numberOptions">Options</param>
		/// <returns></returns>
		public override string FormatUInt16(ushort value, in NumberFormattingOptions numberOptions) =>
			numberFormatter.FormatUInt16(options, numberOptions, value);

		/// <summary>
		/// Formats a <see cref="uint"/>
		/// </summary>
		/// <param name="value">Value</param>
		/// <param name="numberOptions">Options</param>
		/// <returns></returns>
		public override string FormatUInt32(uint value, in NumberFormattingOptions numberOptions) =>
			numberFormatter.FormatUInt32(options, numberOptions, value);

		/// <summary>
		/// Formats a <see cref="ulong"/>
		/// </summary>
		/// <param name="value">Value</param>
		/// <param name="numberOptions">Options</param>
		/// <returns></returns>
		public override string FormatUInt64(ulong value, in NumberFormattingOptions numberOptions) =>
			numberFormatter.FormatUInt64(options, numberOptions, value);
	}
}
#endif
