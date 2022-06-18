// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Generator.Enums;
using Generator.Enums.Encoder;
using Generator.IO;

namespace Generator.Encoder {
	enum MethodArgType {
		Code,
		Register,
		RepPrefixKind,
		Memory,
		UInt8,
		UInt16,
		Int32,
		UInt32,
		Int64,
		UInt64,
		PreferredInt32,
		ArrayIndex,
		ArrayLength,
		ByteArray,
		WordArray,
		DwordArray,
		QwordArray,
		ByteSlice,
		WordSlice,
		DwordSlice,
		QwordSlice,
		BytePtr,
		WordPtr,
		DwordPtr,
		QwordPtr,
	}

	sealed class MethodArg {
		public string Doc { get; }
		public MethodArgType Type { get; }
		public string Name { get; }
		public object? DefaultValue { get; }
		public MethodArg(string doc, MethodArgType type, string name)
			: this(doc, type, name, null) {
		}
		public MethodArg(string doc, MethodArgType type, string name, object? defaultValue = null) {
			Doc = doc;
			Type = type;
			Name = name;
			DefaultValue = defaultValue;
		}
	}

	sealed class CreateMethod {
		public readonly List<string> Docs;
		public readonly List<MethodArg> Args = new();
		public readonly List<object?> DefaultArgs = new();
		public CreateMethod(params string[] docs) => Docs = docs.ToList();
		public CreateMethod Copy() {
			var method = new CreateMethod(Docs.ToArray());
			method.Args.AddRange(Args);
			method.DefaultArgs.AddRange(DefaultArgs);
			return method;
		}
	}

	abstract class InstrCreateGen {
		protected abstract (TargetLanguage language, string id, string filename) GetFileInfo();
		protected abstract void GenCreate(FileWriter writer, CreateMethod method, InstructionGroup group, int id);
		protected abstract void GenCreateBranch(FileWriter writer, CreateMethod method);
		protected abstract void GenCreateFarBranch(FileWriter writer, CreateMethod method);
		protected abstract void GenCreateXbegin(FileWriter writer, CreateMethod method);
		protected abstract void GenCreateString_Reg_SegRSI(FileWriter writer, CreateMethod method, StringMethodKind kind, string methodBaseName, EnumValue code, EnumValue register);
		protected abstract void GenCreateString_Reg_ESRDI(FileWriter writer, CreateMethod method, StringMethodKind kind, string methodBaseName, EnumValue code, EnumValue register);
		protected abstract void GenCreateString_ESRDI_Reg(FileWriter writer, CreateMethod method, StringMethodKind kind, string methodBaseName, EnumValue code, EnumValue register);
		protected abstract void GenCreateString_SegRSI_ESRDI(FileWriter writer, CreateMethod method, StringMethodKind kind, string methodBaseName, EnumValue code);
		protected abstract void GenCreateString_ESRDI_SegRSI(FileWriter writer, CreateMethod method, StringMethodKind kind, string methodBaseName, EnumValue code);
		protected abstract void GenCreateMaskmov(FileWriter writer, CreateMethod method, string methodBaseName, EnumValue code);
		protected abstract void GenCreateDeclareData(FileWriter writer, CreateMethod method, DeclareDataKind kind);
		protected abstract void GenCreateDeclareDataArray(FileWriter writer, CreateMethod method, DeclareDataKind kind, ArrayType arrayType);
		protected abstract void GenCreateDeclareDataArrayLength(FileWriter writer, CreateMethod method, DeclareDataKind kind, ArrayType arrayType);

		protected readonly GenTypes genTypes;
		protected readonly EnumType codeType;
		readonly Dictionary<string, EnumValue> toCode;
		bool writeItemSep;

		protected InstrCreateGen(GenTypes genTypes) {
			this.genTypes = genTypes;
			codeType = genTypes[TypeIds.Code];
			toCode = new Dictionary<string, EnumValue>(codeType.Values.Length);
			foreach (var value in codeType.Values)
				toCode.Add(value.RawName, value);
		}

		bool TryGetCode(string name, [NotNullWhen(true)] out EnumValue? code) => toCode.TryGetValue(name, out code);

		public void Generate() {
			// The code assumes it has value 0 so the field doesn't have to be initialized if we know that it's already 0
			if (genTypes[TypeIds.OpKind][nameof(OpKind.Register)].Value != 0)
				throw new InvalidOperationException();
			var (language, id, filename) = GetFileInfo();
			new FileUpdater(language, id, filename).Generate(Generate);
		}

		protected virtual void Generate(FileWriter writer) {
			GenCreateMethods(writer, 0);
			GenTheRest(writer);
		}

		protected void GenTheRest(FileWriter writer) {
			GenCreateBranch(writer);
			GenCreateFarBranch(writer);
			GenCreateXbegin(writer);
			GenCreateStringInstructions(writer);
			GenCreateDeclareXxx(writer);
		}

		protected void ResetItemSeparator() => writeItemSep = false;
		protected void WriteItemSeparator(FileWriter writer) {
			if (writeItemSep)
				writer.WriteLine();
			writeItemSep = true;
		}

		protected virtual bool SupportsUnsignedIntegers => true;
		protected void GenCreateMethods(FileWriter writer, int id) {
			var groups = new InstructionGroups(genTypes, true).GetGroups();
			const bool useReg = false; // r/m was split into two groups, r and m so this can be anything
			foreach (var info in GetCreateMethods(groups, useReg, SupportsUnsignedIntegers)) {
				foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(info.method)) {
					WriteItemSeparator(writer);
					GenCreate(writer, method, info.group, id);
				}
			}
		}

		protected virtual bool SupportsDefaultArguments => true;
		IEnumerable<CreateMethod> CreateMethodsWithoutDefaultArgsIfNeeded(CreateMethod method) {
			if (SupportsDefaultArguments || method.Args.Count == 0 || method.Args[^1].DefaultValue is null)
				yield return method;
			else {
				var methods = new List<CreateMethod>();
				methods.Add(method.Copy());
				while (method.Args.Count > 0 && method.Args[^1].DefaultValue is not null) {
					method.DefaultArgs.Insert(0, method.Args[^1].DefaultValue);
					method.Args.RemoveAt(method.Args.Count - 1);
					methods.Add(method.Copy());
				}
				methods.Reverse();
				if (methods.Count == 0)
					throw new InvalidOperationException();
				foreach (var m in methods)
					yield return m;
			}
		}

		static IEnumerable<(CreateMethod method, InstructionGroup group)> GetCreateMethods(InstructionGroup[] groups, bool useReg, bool supportsUnsignedIntegers) {
			foreach (var group in groups) {
				yield return (GetMethod(group, unsigned: false, useReg), group);
				if (supportsUnsignedIntegers && GetOpKindCount(group, useReg).immCount > 0)
					yield return (GetMethod(group, unsigned: true, useReg), group);
			}
		}

		protected static void AddCodeArg(CreateMethod method) => method.Args.Add(new MethodArg("Code value", MethodArgType.Code, "code"));
		static void AddAddressSizeArg(CreateMethod method) => method.Args.Add(new MethodArg("16, 32, or 64", MethodArgType.PreferredInt32, "addressSize"));
		static void AddTargetArg(CreateMethod method) => method.Args.Add(new MethodArg("Target address", MethodArgType.UInt64, "target"));
		static void AddBitnessArg(CreateMethod method) => method.Args.Add(new MethodArg("16, 32, or 64", MethodArgType.PreferredInt32, "bitness"));
		void AddSegmentPrefixArg(CreateMethod method) => method.Args.Add(new MethodArg("Segment override or #(e:Register.None)#", MethodArgType.Register, "segmentPrefix", genTypes[TypeIds.Register][nameof(Register.None)]));
		void AddRepPrefixArg(CreateMethod method) => method.Args.Add(new MethodArg("Rep prefix or #(e:RepPrefixKind.None)#", MethodArgType.RepPrefixKind, "repPrefix", genTypes[TypeIds.RepPrefixKind][nameof(RepPrefixKind.None)]));

		protected static CreateMethod GetMethod(InstructionGroup group, bool unsigned, bool useReg) {
			var (regCount, immCount, memCount) = GetOpKindCount(group, useReg);
			int regId = 1, immId = 1, memId = 1;
			string doc = group.Operands.Length switch {
				0 => "Creates an instruction with no operands",
				1 => "Creates an instruction with 1 operand",
				_ => $"Creates an instruction with {group.Operands.Length} operands",
			};
			var method = new CreateMethod(doc);
			AddCodeArg(method);
			int opNum = -1;
			foreach (var op in group.Operands) {
				opNum++;
				switch (op.Split(useReg)) {
				case InstructionOperand.RegisterMemory:
					throw new InvalidOperationException();
				case InstructionOperand.Register:
					method.Args.Add(new MethodArg($"op{opNum}: Register", MethodArgType.Register, GetArgName("register", regCount, regId++)));
					break;
				case InstructionOperand.Memory:
					method.Args.Add(new MethodArg($"op{opNum}: Memory operand", MethodArgType.Memory, GetArgName("memory", memCount, memId++)));
					break;
				case InstructionOperand.Imm32:
					method.Args.Add(new MethodArg($"op{opNum}: Immediate value", unsigned ? MethodArgType.UInt32 : MethodArgType.Int32, GetArgName("immediate", immCount, immId++)));
					break;
				case InstructionOperand.Imm64:
					method.Args.Add(new MethodArg($"op{opNum}: Immediate value", unsigned ? MethodArgType.UInt64 : MethodArgType.Int64, GetArgName("immediate", immCount, immId++)));
					break;
				default:
					throw new InvalidOperationException();
				}
			}
			return method;

			static string GetArgName(string name, int count, int id) {
				if (count <= 1)
					return name;
				return name + id.ToString();
			}
		}

		static (int regCount, int immCount, int memCount) GetOpKindCount(InstructionGroup group, bool useReg) {
			int regCount = 0;
			int immCount = 0;
			int memCount = 0;

			foreach (var op in group.Operands) {
				switch (op.Split(useReg)) {
				case InstructionOperand.RegisterMemory:
					throw new InvalidOperationException();
				case InstructionOperand.Register:
					regCount++;
					break;
				case InstructionOperand.Memory:
					memCount++;
					break;
				case InstructionOperand.Imm32:
				case InstructionOperand.Imm64:
					immCount++;
					break;
				default:
					throw new InvalidOperationException();
				}
			}

			return (regCount, immCount, memCount);
		}

		void GenCreateBranch(FileWriter writer) {
			var origMethod = new CreateMethod("Creates a new near/short branch instruction");
			AddCodeArg(origMethod);
			AddTargetArg(origMethod);
			foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod)) {
				WriteItemSeparator(writer);
				GenCreateBranch(writer, method);
			}
		}

		void GenCreateFarBranch(FileWriter writer) {
			var origMethod = new CreateMethod("Creates a new far branch instruction");
			AddCodeArg(origMethod);
			origMethod.Args.Add(new MethodArg("Selector/segment value", MethodArgType.UInt16, "selector"));
			origMethod.Args.Add(new MethodArg("Offset", MethodArgType.UInt32, "offset"));
			foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod)) {
				WriteItemSeparator(writer);
				GenCreateFarBranch(writer, method);
			}
		}

		void GenCreateXbegin(FileWriter writer) {
			if (TryGetCode(nameof(Code.Xbegin_rel16), out _) && TryGetCode(nameof(Code.Xbegin_rel32), out _)) {
				var origMethod = new CreateMethod("Creates a new #(c:XBEGIN)# instruction");
				AddBitnessArg(origMethod);
				AddTargetArg(origMethod);
				foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod)) {
					WriteItemSeparator(writer);
					GenCreateXbegin(writer, method);
				}
			}
		}

		void GenCreateStringInstructions(FileWriter writer) {
			GenCreateString_Reg_SegRSI(writer);
			GenCreateString_Reg_ESRDI(writer);
			GenCreateString_ESRDI_Reg(writer);
			GenCreateString_SegRSI_ESRDI(writer);
			GenCreateString_ESRDI_SegRSI(writer);
			GenCreateMaskmov(writer);
		}

		protected enum StringMethodKind {
			Full,
			Rep,
			Repe,
			Repne,
		}

		void GenCreateString_Reg_SegRSI(FileWriter writer) {
			Gen(writer, "outsb", nameof(Code.Outsb_DX_m8), genTypes[TypeIds.Register][nameof(Register.DX)]);
			Gen(writer, "outsw", nameof(Code.Outsw_DX_m16), genTypes[TypeIds.Register][nameof(Register.DX)]);
			Gen(writer, "outsd", nameof(Code.Outsd_DX_m32), genTypes[TypeIds.Register][nameof(Register.DX)]);
			Gen(writer, "lodsb", nameof(Code.Lodsb_AL_m8), genTypes[TypeIds.Register][nameof(Register.AL)]);
			Gen(writer, "lodsw", nameof(Code.Lodsw_AX_m16), genTypes[TypeIds.Register][nameof(Register.AX)]);
			Gen(writer, "lodsd", nameof(Code.Lodsd_EAX_m32), genTypes[TypeIds.Register][nameof(Register.EAX)]);
			Gen(writer, "lodsq", nameof(Code.Lodsq_RAX_m64), genTypes[TypeIds.Register][nameof(Register.RAX)]);

			void Gen(FileWriter writer, string mnemonic, string codeStr, EnumValue register) {
				if (!TryGetCode(codeStr, out var code))
					return;
				var mnemonicUpper = mnemonic.ToUpperInvariant();
				var baseName = mnemonicUpper[0..1] + mnemonicUpper[1..].ToLowerInvariant();

				{
					var origMethod = new CreateMethod($"Creates a #(c:{mnemonicUpper})# instruction");
					var name = baseName;
					AddAddressSizeArg(origMethod);
					AddSegmentPrefixArg(origMethod);
					AddRepPrefixArg(origMethod);
					foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod)) {
						WriteItemSeparator(writer);
						GenCreateString_Reg_SegRSI(writer, method, StringMethodKind.Full, name, code, register);
					}
				}
				{
					var origMethod = new CreateMethod($"Creates a #(c:REP {mnemonicUpper})# instruction");
					var name = "Rep" + baseName;
					AddAddressSizeArg(origMethod);
					foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod)) {
						WriteItemSeparator(writer);
						GenCreateString_Reg_SegRSI(writer, method, StringMethodKind.Rep, name, code, register);
					}
				}
			}
		}

		void GenCreateString_Reg_ESRDI(FileWriter writer) {
			Gen(writer, "scasb", nameof(Code.Scasb_AL_m8), genTypes[TypeIds.Register][nameof(Register.AL)]);
			Gen(writer, "scasw", nameof(Code.Scasw_AX_m16), genTypes[TypeIds.Register][nameof(Register.AX)]);
			Gen(writer, "scasd", nameof(Code.Scasd_EAX_m32), genTypes[TypeIds.Register][nameof(Register.EAX)]);
			Gen(writer, "scasq", nameof(Code.Scasq_RAX_m64), genTypes[TypeIds.Register][nameof(Register.RAX)]);

			void Gen(FileWriter writer, string mnemonic, string codeStr, EnumValue register) {
				if (!TryGetCode(codeStr, out var code))
					return;
				var mnemonicUpper = mnemonic.ToUpperInvariant();
				var baseName = mnemonicUpper[0..1] + mnemonicUpper[1..].ToLowerInvariant();

				{
					var origMethod = new CreateMethod($"Creates a #(c:{mnemonicUpper})# instruction");
					var name = baseName;
					AddAddressSizeArg(origMethod);
					AddRepPrefixArg(origMethod);
					foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod)) {
						WriteItemSeparator(writer);
						GenCreateString_Reg_ESRDI(writer, method, StringMethodKind.Full, name, code, register);
					}
				}
				{
					var origMethod = new CreateMethod($"Creates a #(c:REPE {mnemonicUpper})# instruction");
					var name = "Repe" + baseName;
					AddAddressSizeArg(origMethod);
					foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod)) {
						WriteItemSeparator(writer);
						GenCreateString_Reg_ESRDI(writer, method, StringMethodKind.Repe, name, code, register);
					}
				}
				{
					var origMethod = new CreateMethod($"Creates a #(c:REPNE {mnemonicUpper})# instruction");
					var name = "Repne" + baseName;
					AddAddressSizeArg(origMethod);
					foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod)) {
						WriteItemSeparator(writer);
						GenCreateString_Reg_ESRDI(writer, method, StringMethodKind.Repne, name, code, register);
					}
				}
			}
		}

		void GenCreateString_ESRDI_Reg(FileWriter writer) {
			Gen(writer, "insb", nameof(Code.Insb_m8_DX), genTypes[TypeIds.Register][nameof(Register.DX)]);
			Gen(writer, "insw", nameof(Code.Insw_m16_DX), genTypes[TypeIds.Register][nameof(Register.DX)]);
			Gen(writer, "insd", nameof(Code.Insd_m32_DX), genTypes[TypeIds.Register][nameof(Register.DX)]);
			Gen(writer, "stosb", nameof(Code.Stosb_m8_AL), genTypes[TypeIds.Register][nameof(Register.AL)]);
			Gen(writer, "stosw", nameof(Code.Stosw_m16_AX), genTypes[TypeIds.Register][nameof(Register.AX)]);
			Gen(writer, "stosd", nameof(Code.Stosd_m32_EAX), genTypes[TypeIds.Register][nameof(Register.EAX)]);
			Gen(writer, "stosq", nameof(Code.Stosq_m64_RAX), genTypes[TypeIds.Register][nameof(Register.RAX)]);

			void Gen(FileWriter writer, string mnemonic, string codeStr, EnumValue register) {
				if (!TryGetCode(codeStr, out var code))
					return;
				var mnemonicUpper = mnemonic.ToUpperInvariant();
				var baseName = mnemonicUpper[0..1] + mnemonicUpper[1..].ToLowerInvariant();

				{
					var origMethod = new CreateMethod($"Creates a #(c:{mnemonicUpper})# instruction");
					var name = baseName;
					AddAddressSizeArg(origMethod);
					AddRepPrefixArg(origMethod);
					foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod)) {
						WriteItemSeparator(writer);
						GenCreateString_ESRDI_Reg(writer, method, StringMethodKind.Full, name, code, register);
					}
				}
				{
					var origMethod = new CreateMethod($"Creates a #(c:REP {mnemonicUpper})# instruction");
					var name = "Rep" + baseName;
					AddAddressSizeArg(origMethod);
					foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod)) {
						WriteItemSeparator(writer);
						GenCreateString_ESRDI_Reg(writer, method, StringMethodKind.Rep, name, code, register);
					}
				}
			}
		}

		void GenCreateString_SegRSI_ESRDI(FileWriter writer) {
			Gen(writer, "cmpsb", nameof(Code.Cmpsb_m8_m8));
			Gen(writer, "cmpsw", nameof(Code.Cmpsw_m16_m16));
			Gen(writer, "cmpsd", nameof(Code.Cmpsd_m32_m32));
			Gen(writer, "cmpsq", nameof(Code.Cmpsq_m64_m64));

			void Gen(FileWriter writer, string mnemonic, string codeStr) {
				if (!TryGetCode(codeStr, out var code))
					return;
				var mnemonicUpper = mnemonic.ToUpperInvariant();
				var baseName = mnemonicUpper[0..1] + mnemonicUpper[1..].ToLowerInvariant();

				{
					var origMethod = new CreateMethod($"Creates a #(c:{mnemonicUpper})# instruction");
					var name = baseName;
					AddAddressSizeArg(origMethod);
					AddSegmentPrefixArg(origMethod);
					AddRepPrefixArg(origMethod);
					foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod)) {
						WriteItemSeparator(writer);
						GenCreateString_SegRSI_ESRDI(writer, method, StringMethodKind.Full, name, code);
					}
				}
				{
					var origMethod = new CreateMethod($"Creates a #(c:REPE {mnemonicUpper})# instruction");
					var name = "Repe" + baseName;
					AddAddressSizeArg(origMethod);
					foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod)) {
						WriteItemSeparator(writer);
						GenCreateString_SegRSI_ESRDI(writer, method, StringMethodKind.Repe, name, code);
					}
				}
				{
					var origMethod = new CreateMethod($"Creates a #(c:REPNE {mnemonicUpper})# instruction");
					var name = "Repne" + baseName;
					AddAddressSizeArg(origMethod);
					foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod)) {
						WriteItemSeparator(writer);
						GenCreateString_SegRSI_ESRDI(writer, method, StringMethodKind.Repne, name, code);
					}
				}
			}
		}

		void GenCreateString_ESRDI_SegRSI(FileWriter writer) {
			Gen(writer, "movsb", nameof(Code.Movsb_m8_m8));
			Gen(writer, "movsw", nameof(Code.Movsw_m16_m16));
			Gen(writer, "movsd", nameof(Code.Movsd_m32_m32));
			Gen(writer, "movsq", nameof(Code.Movsq_m64_m64));

			void Gen(FileWriter writer, string mnemonic, string codeStr) {
				if (!TryGetCode(codeStr, out var code))
					return;
				var mnemonicUpper = mnemonic.ToUpperInvariant();
				var baseName = mnemonicUpper[0..1] + mnemonicUpper[1..].ToLowerInvariant();

				{
					var origMethod = new CreateMethod($"Creates a #(c:{mnemonicUpper})# instruction");
					var name = baseName;
					AddAddressSizeArg(origMethod);
					AddSegmentPrefixArg(origMethod);
					AddRepPrefixArg(origMethod);
					foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod)) {
						WriteItemSeparator(writer);
						GenCreateString_ESRDI_SegRSI(writer, method, StringMethodKind.Full, name, code);
					}
				}
				{
					var origMethod = new CreateMethod($"Creates a #(c:REP {mnemonicUpper})# instruction");
					var name = "Rep" + baseName;
					AddAddressSizeArg(origMethod);
					foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod)) {
						WriteItemSeparator(writer);
						GenCreateString_ESRDI_SegRSI(writer, method, StringMethodKind.Rep, name, code);
					}
				}
			}
		}

		void GenCreateMaskmov(FileWriter writer) {
			Gen(writer, "maskmovq", nameof(Code.Maskmovq_rDI_mm_mm));
			Gen(writer, "maskmovdqu", nameof(Code.Maskmovdqu_rDI_xmm_xmm));
			Gen(writer, "vmaskmovdqu", nameof(Code.VEX_Vmaskmovdqu_rDI_xmm_xmm));

			void Gen(FileWriter writer, string mnemonic, string codeStr) {
				if (!TryGetCode(codeStr, out var code))
					return;
				var mnemonicUpper = mnemonic.ToUpperInvariant();
				var baseName = mnemonicUpper[0..1] + mnemonicUpper[1..].ToLowerInvariant();

				var origMethod = new CreateMethod($"Creates a #(c:{mnemonicUpper})# instruction");
				var name = baseName;
				AddAddressSizeArg(origMethod);
				origMethod.Args.Add(new MethodArg("Register", MethodArgType.Register, "register1"));
				origMethod.Args.Add(new MethodArg("Register", MethodArgType.Register, "register2"));
				AddSegmentPrefixArg(origMethod);
				foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod)) {
					WriteItemSeparator(writer);
					GenCreateMaskmov(writer, method, name, code);
				}
			}
		}

		protected enum DeclareDataKind {
			Byte,
			Word,
			Dword,
			Qword,
		}

		protected enum ArrayType {
			ByteArray,
			WordArray,
			DwordArray,
			QwordArray,

			ByteSlice,
			WordSlice,
			DwordSlice,
			QwordSlice,

			BytePtr,
			WordPtr,
			DwordPtr,
			QwordPtr,
		}

		void GenCreateDeclareXxx(FileWriter writer) {
			if (TryGetCode(nameof(Code.DeclareByte), out _))
				GenCreateDeclareByte(writer);
			if (TryGetCode(nameof(Code.DeclareWord), out _))
				GenCreateDeclareWord(writer);
			if (TryGetCode(nameof(Code.DeclareDword), out _))
				GenCreateDeclareDword(writer);
			if (TryGetCode(nameof(Code.DeclareQword), out _))
				GenCreateDeclareQword(writer);
		}

		static class DeclareConsts {
			public const string dbDoc = "Creates a #(c:db)#/#(c:.byte)# asm directive";
			public const string dwDoc = "Creates a #(c:dw)#/#(c:.word)# asm directive";
			public const string ddDoc = "Creates a #(c:dd)#/#(c:.int)# asm directive";
			public const string dqDoc = "Creates a #(c:dq)#/#(c:.quad)# asm directive";
			public const string dataArgName = "data";
			public const string dataArgDoc = "Data";
			public const string indexArgName = "index";
			public const string indexArgDoc = "Start index";
			public const string lengthArgName = "length";
			public const string lengthBytesDoc = "Number of bytes";
			public const string lengthElemsDoc = "Number of elements";
		}

		void GenCreateDeclareByte(FileWriter writer) {
			for (int args = 1; args <= 16; args++) {
				var origMethod = new CreateMethod(DeclareConsts.dbDoc);
				for (int i = 0; i < args; i++)
					origMethod.Args.Add(new MethodArg($"Byte {i}", MethodArgType.UInt8, $"b{i}"));
				foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod))
					GenCreateDeclareData(writer, method, DeclareDataKind.Byte);
			}
			{
				var origMethod = new CreateMethod(DeclareConsts.dbDoc);
				origMethod.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.BytePtr, DeclareConsts.dataArgName));
				origMethod.Args.Add(new MethodArg(DeclareConsts.lengthBytesDoc, MethodArgType.ArrayLength, DeclareConsts.lengthArgName));
				foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod))
					GenCreateDeclareDataArray(writer, method, DeclareDataKind.Byte, ArrayType.BytePtr);
			}
			{
				var origMethod = new CreateMethod(DeclareConsts.dbDoc);
				origMethod.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.ByteSlice, DeclareConsts.dataArgName));
				foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod))
					GenCreateDeclareDataArray(writer, method, DeclareDataKind.Byte, ArrayType.ByteSlice);
			}
			{
				var origMethod = new CreateMethod(DeclareConsts.dbDoc);
				origMethod.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.ByteArray, DeclareConsts.dataArgName));
				foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod))
					GenCreateDeclareDataArray(writer, method, DeclareDataKind.Byte, ArrayType.ByteArray);
			}
			{
				var origMethod = new CreateMethod(DeclareConsts.dbDoc);
				origMethod.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.ByteArray, DeclareConsts.dataArgName));
				origMethod.Args.Add(new MethodArg(DeclareConsts.indexArgDoc, MethodArgType.ArrayIndex, DeclareConsts.indexArgName));
				origMethod.Args.Add(new MethodArg(DeclareConsts.lengthBytesDoc, MethodArgType.ArrayLength, DeclareConsts.lengthArgName));
				foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod))
					GenCreateDeclareDataArrayLength(writer, method, DeclareDataKind.Byte, ArrayType.ByteArray);
			}
		}

		void GenCreateDeclareWord(FileWriter writer) {
			for (int args = 1; args <= 8; args++) {
				var origMethod = new CreateMethod(DeclareConsts.dwDoc);
				for (int i = 0; i < args; i++)
					origMethod.Args.Add(new MethodArg($"Word {i}", MethodArgType.UInt16, $"w{i}"));
				foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod))
					GenCreateDeclareData(writer, method, DeclareDataKind.Word);
			}
			{
				var origMethod = new CreateMethod(DeclareConsts.dwDoc);
				origMethod.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.BytePtr, DeclareConsts.dataArgName));
				origMethod.Args.Add(new MethodArg(DeclareConsts.lengthBytesDoc, MethodArgType.ArrayLength, DeclareConsts.lengthArgName));
				foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod))
					GenCreateDeclareDataArray(writer, method, DeclareDataKind.Word, ArrayType.BytePtr);
			}
			{
				var origMethod = new CreateMethod(DeclareConsts.dwDoc);
				origMethod.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.ByteSlice, DeclareConsts.dataArgName));
				foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod))
					GenCreateDeclareDataArray(writer, method, DeclareDataKind.Word, ArrayType.ByteSlice);
			}
			{
				var origMethod = new CreateMethod(DeclareConsts.dwDoc);
				origMethod.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.ByteArray, DeclareConsts.dataArgName));
				foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod))
					GenCreateDeclareDataArray(writer, method, DeclareDataKind.Word, ArrayType.ByteArray);
			}
			{
				var origMethod = new CreateMethod(DeclareConsts.dwDoc);
				origMethod.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.ByteArray, DeclareConsts.dataArgName));
				origMethod.Args.Add(new MethodArg(DeclareConsts.indexArgDoc, MethodArgType.ArrayIndex, DeclareConsts.indexArgName));
				origMethod.Args.Add(new MethodArg(DeclareConsts.lengthBytesDoc, MethodArgType.ArrayLength, DeclareConsts.lengthArgName));
				foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod))
					GenCreateDeclareDataArrayLength(writer, method, DeclareDataKind.Word, ArrayType.ByteArray);
			}
			{
				var origMethod = new CreateMethod(DeclareConsts.dwDoc);
				origMethod.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.WordPtr, DeclareConsts.dataArgName));
				origMethod.Args.Add(new MethodArg(DeclareConsts.lengthElemsDoc, MethodArgType.ArrayLength, DeclareConsts.lengthArgName));
				foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod))
					GenCreateDeclareDataArray(writer, method, DeclareDataKind.Word, ArrayType.WordPtr);
			}
			{
				var origMethod = new CreateMethod(DeclareConsts.dwDoc);
				origMethod.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.WordSlice, DeclareConsts.dataArgName));
				foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod))
					GenCreateDeclareDataArray(writer, method, DeclareDataKind.Word, ArrayType.WordSlice);
			}
			{
				var origMethod = new CreateMethod(DeclareConsts.dwDoc);
				origMethod.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.WordArray, DeclareConsts.dataArgName));
				foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod))
					GenCreateDeclareDataArray(writer, method, DeclareDataKind.Word, ArrayType.WordArray);
			}
			{
				var origMethod = new CreateMethod(DeclareConsts.dwDoc);
				origMethod.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.WordArray, DeclareConsts.dataArgName));
				origMethod.Args.Add(new MethodArg(DeclareConsts.indexArgDoc, MethodArgType.ArrayIndex, DeclareConsts.indexArgName));
				origMethod.Args.Add(new MethodArg(DeclareConsts.lengthElemsDoc, MethodArgType.ArrayLength, DeclareConsts.lengthArgName));
				foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod))
					GenCreateDeclareDataArrayLength(writer, method, DeclareDataKind.Word, ArrayType.WordArray);
			}
		}

		void GenCreateDeclareDword(FileWriter writer) {
			for (int args = 1; args <= 4; args++) {
				var origMethod = new CreateMethod(DeclareConsts.ddDoc);
				for (int i = 0; i < args; i++)
					origMethod.Args.Add(new MethodArg($"Dword {i}", MethodArgType.UInt32, $"d{i}"));
				foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod))
					GenCreateDeclareData(writer, method, DeclareDataKind.Dword);
			}
			{
				var origMethod = new CreateMethod(DeclareConsts.ddDoc);
				origMethod.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.BytePtr, DeclareConsts.dataArgName));
				origMethod.Args.Add(new MethodArg(DeclareConsts.lengthBytesDoc, MethodArgType.ArrayLength, DeclareConsts.lengthArgName));
				foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod))
					GenCreateDeclareDataArray(writer, method, DeclareDataKind.Dword, ArrayType.BytePtr);
			}
			{
				var origMethod = new CreateMethod(DeclareConsts.ddDoc);
				origMethod.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.ByteSlice, DeclareConsts.dataArgName));
				foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod))
					GenCreateDeclareDataArray(writer, method, DeclareDataKind.Dword, ArrayType.ByteSlice);
			}
			{
				var origMethod = new CreateMethod(DeclareConsts.ddDoc);
				origMethod.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.ByteArray, DeclareConsts.dataArgName));
				foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod))
					GenCreateDeclareDataArray(writer, method, DeclareDataKind.Dword, ArrayType.ByteArray);
			}
			{
				var origMethod = new CreateMethod(DeclareConsts.ddDoc);
				origMethod.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.ByteArray, DeclareConsts.dataArgName));
				origMethod.Args.Add(new MethodArg(DeclareConsts.indexArgDoc, MethodArgType.ArrayIndex, DeclareConsts.indexArgName));
				origMethod.Args.Add(new MethodArg(DeclareConsts.lengthBytesDoc, MethodArgType.ArrayLength, DeclareConsts.lengthArgName));
				foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod))
					GenCreateDeclareDataArrayLength(writer, method, DeclareDataKind.Dword, ArrayType.ByteArray);
			}
			{
				var origMethod = new CreateMethod(DeclareConsts.ddDoc);
				origMethod.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.DwordPtr, DeclareConsts.dataArgName));
				origMethod.Args.Add(new MethodArg(DeclareConsts.lengthElemsDoc, MethodArgType.ArrayLength, DeclareConsts.lengthArgName));
				foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod))
					GenCreateDeclareDataArray(writer, method, DeclareDataKind.Dword, ArrayType.DwordPtr);
			}
			{
				var origMethod = new CreateMethod(DeclareConsts.ddDoc);
				origMethod.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.DwordSlice, DeclareConsts.dataArgName));
				foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod))
					GenCreateDeclareDataArray(writer, method, DeclareDataKind.Dword, ArrayType.DwordSlice);
			}
			{
				var origMethod = new CreateMethod(DeclareConsts.ddDoc);
				origMethod.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.DwordArray, DeclareConsts.dataArgName));
				foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod))
					GenCreateDeclareDataArray(writer, method, DeclareDataKind.Dword, ArrayType.DwordArray);
			}
			{
				var origMethod = new CreateMethod(DeclareConsts.ddDoc);
				origMethod.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.DwordArray, DeclareConsts.dataArgName));
				origMethod.Args.Add(new MethodArg(DeclareConsts.indexArgDoc, MethodArgType.ArrayIndex, DeclareConsts.indexArgName));
				origMethod.Args.Add(new MethodArg(DeclareConsts.lengthElemsDoc, MethodArgType.ArrayLength, DeclareConsts.lengthArgName));
				foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod))
					GenCreateDeclareDataArrayLength(writer, method, DeclareDataKind.Dword, ArrayType.DwordArray);
			}
		}

		void GenCreateDeclareQword(FileWriter writer) {
			for (int args = 1; args <= 2; args++) {
				var origMethod = new CreateMethod(DeclareConsts.dqDoc);
				for (int i = 0; i < args; i++)
					origMethod.Args.Add(new MethodArg($"Qword {i}", MethodArgType.UInt64, $"q{i}"));
				foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod))
					GenCreateDeclareData(writer, method, DeclareDataKind.Qword);
			}
			{
				var origMethod = new CreateMethod(DeclareConsts.dqDoc);
				origMethod.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.BytePtr, DeclareConsts.dataArgName));
				origMethod.Args.Add(new MethodArg(DeclareConsts.lengthBytesDoc, MethodArgType.ArrayLength, DeclareConsts.lengthArgName));
				foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod))
					GenCreateDeclareDataArray(writer, method, DeclareDataKind.Qword, ArrayType.BytePtr);
			}
			{
				var origMethod = new CreateMethod(DeclareConsts.dqDoc);
				origMethod.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.ByteSlice, DeclareConsts.dataArgName));
				foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod))
					GenCreateDeclareDataArray(writer, method, DeclareDataKind.Qword, ArrayType.ByteSlice);
			}
			{
				var origMethod = new CreateMethod(DeclareConsts.dqDoc);
				origMethod.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.ByteArray, DeclareConsts.dataArgName));
				foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod))
					GenCreateDeclareDataArray(writer, method, DeclareDataKind.Qword, ArrayType.ByteArray);
			}
			{
				var origMethod = new CreateMethod(DeclareConsts.dqDoc);
				origMethod.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.ByteArray, DeclareConsts.dataArgName));
				origMethod.Args.Add(new MethodArg(DeclareConsts.indexArgDoc, MethodArgType.ArrayIndex, DeclareConsts.indexArgName));
				origMethod.Args.Add(new MethodArg(DeclareConsts.lengthBytesDoc, MethodArgType.ArrayLength, DeclareConsts.lengthArgName));
				foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod))
					GenCreateDeclareDataArrayLength(writer, method, DeclareDataKind.Qword, ArrayType.ByteArray);
			}
			{
				var origMethod = new CreateMethod(DeclareConsts.dqDoc);
				origMethod.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.QwordPtr, DeclareConsts.dataArgName));
				origMethod.Args.Add(new MethodArg(DeclareConsts.lengthElemsDoc, MethodArgType.ArrayLength, DeclareConsts.lengthArgName));
				foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod))
					GenCreateDeclareDataArray(writer, method, DeclareDataKind.Qword, ArrayType.QwordPtr);
			}
			{
				var origMethod = new CreateMethod(DeclareConsts.dqDoc);
				origMethod.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.QwordSlice, DeclareConsts.dataArgName));
				foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod))
					GenCreateDeclareDataArray(writer, method, DeclareDataKind.Qword, ArrayType.QwordSlice);
			}
			{
				var origMethod = new CreateMethod(DeclareConsts.dqDoc);
				origMethod.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.QwordArray, DeclareConsts.dataArgName));
				foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod))
					GenCreateDeclareDataArray(writer, method, DeclareDataKind.Qword, ArrayType.QwordArray);
			}
			{
				var origMethod = new CreateMethod(DeclareConsts.dqDoc);
				origMethod.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.QwordArray, DeclareConsts.dataArgName));
				origMethod.Args.Add(new MethodArg(DeclareConsts.indexArgDoc, MethodArgType.ArrayIndex, DeclareConsts.indexArgName));
				origMethod.Args.Add(new MethodArg(DeclareConsts.lengthElemsDoc, MethodArgType.ArrayLength, DeclareConsts.lengthArgName));
				foreach (var method in CreateMethodsWithoutDefaultArgsIfNeeded(origMethod))
					GenCreateDeclareDataArrayLength(writer, method, DeclareDataKind.Qword, ArrayType.QwordArray);
			}
		}

		protected readonly struct DynCreateMethodTable {
			/// Use the Code value as an index to get the group index into Groups below
			public readonly int[] CodeIndexes;
			/// If it's null (the first one), then it's an error (at runtime) since that Code
			/// value can't be passed in to Create() methods (a more specialized Create*() method
			/// should be used instead, eg. CreateBranch()).
			public readonly InstructionGroup?[] Groups;

			/// Original groups, sorted in the original order
			public readonly InstructionGroup[] OrigGroups;

			public DynCreateMethodTable(int[] codeIndexes, InstructionGroup?[] groups, InstructionGroup[] origGroups) {
				CodeIndexes = codeIndexes;
				Groups = groups;
				OrigGroups = origGroups;
			}
		}

		/// Can be used by dynamic languages for the Create() impl that can be passed any
		/// number of arguments and each arg can be any supported operand type.
		protected DynCreateMethodTable GetDynCreateMethodTable() {
			var groups = new InstructionGroups(genTypes, false).GetGroups();
			var newGroups = new InstructionGroup?[groups.Length + 1];
			// Invalid Code value is an error at runtime
			newGroups[0] = null;
			for (int i = 0; i < groups.Length; i++)
				newGroups[i + 1] = groups[i];

			var codeEnum = genTypes[TypeIds.Code];
			// The invalid index is `0` so we don't need to init it here
			var codeIndexes = new int[codeEnum.Values.Length];
			for (int newGroupIndex = 0; newGroupIndex < newGroups.Length; newGroupIndex++) {
				if (newGroups[newGroupIndex] is not InstructionGroup group)
					continue;
				foreach (var def in group.Defs) {
					// Make sure it isn't written twice
					if (codeIndexes[(int)def.Code.Value] != 0)
						throw new InvalidOperationException();
					codeIndexes[(int)def.Code.Value] = newGroupIndex;
				}
			}

			return new(codeIndexes, newGroups, groups);
		}
	}
}
