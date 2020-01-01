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

using System;
using System.Collections.Generic;
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
		PreferedInt32,
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
		public MethodArg(string doc, MethodArgType type, string name, object? defaultValue = null) {
			Doc = doc;
			Type = type;
			Name = name;
			DefaultValue = defaultValue;
		}
	}

	sealed class CreateMethod {
		public readonly string Doc;
		public readonly List<MethodArg> Args = new List<MethodArg>();
		public CreateMethod(string doc) => Doc = doc;
	}

	abstract class InstrCreateGen {
		protected abstract (TargetLanguage language, string id, string filename) GetFileInfo();
		protected abstract void GenCreate(FileWriter writer, CreateMethod method, InstructionGroup group);
		protected abstract void GenCreateBranch(FileWriter writer, CreateMethod method);
		protected abstract void GenCreateFarBranch(FileWriter writer, CreateMethod method);
		protected abstract void GenCreateMemory64(FileWriter writer, CreateMethod method);
		protected abstract void GenCreateString_Reg_SegRSI(FileWriter writer, CreateMethod method, StringMethodKind kind, string methodBaseName, EnumValue code, EnumValue register);
		protected abstract void GenCreateString_Reg_ESRDI(FileWriter writer, CreateMethod method, StringMethodKind kind, string methodBaseName, EnumValue code, EnumValue register);
		protected abstract void GenCreateString_ESRDI_Reg(FileWriter writer, CreateMethod method, StringMethodKind kind, string methodBaseName, EnumValue code, EnumValue register);
		protected abstract void GenCreateString_SegRSI_ESRDI(FileWriter writer, CreateMethod method, StringMethodKind kind, string methodBaseName, EnumValue code);
		protected abstract void GenCreateString_ESRDI_SegRSI(FileWriter writer, CreateMethod method, StringMethodKind kind, string methodBaseName, EnumValue code);
		protected abstract void GenCreateMaskmov(FileWriter writer, CreateMethod method, string methodBaseName, EnumValue code);
		protected abstract void GenCreateDeclareData(FileWriter writer, CreateMethod method, DeclareDataKind kind);
		protected abstract void GenCreateDeclareDataArray(FileWriter writer, CreateMethod method, DeclareDataKind kind, ArrayType arrayType);
		protected abstract void GenCreateDeclareDataArrayLength(FileWriter writer, CreateMethod method, DeclareDataKind kind, ArrayType arrayType);

		public void Generate() {
			// The code assumes it has value 0 so the field doesn't have to be initialized if we know that it's already 0
			if (OpKindEnum.Instance[nameof(OpKind.Register)].Value != 0)
				throw new InvalidOperationException();
			var info = GetFileInfo();
			new FileUpdater(info.language, info.id, info.filename).Generate(writer => {
				GenerateCreateMethods(writer);
				GenerateCreateBranch(writer);
				GenerateCreateFarBranch(writer);
				GenCreateMemory64(writer);
				GenCreateStringInstructions(writer);
				GenCreateDeclareXxx(writer);
			});
		}

		void GenerateCreateMethods(FileWriter writer) {
			var groups = new InstructionGroups().GetGroups();
			bool writeLine = false;
			foreach (var info in GetCreateMethods(groups)) {
				if (writeLine)
					writer.WriteLine();
				writeLine = true;
				GenCreate(writer, info.method, info.group);
			}
		}

		IEnumerable<(CreateMethod method, InstructionGroup group)> GetCreateMethods(InstructionGroup[] groups) {
			foreach (var group in groups) {
				yield return (GetMethod(group, false), group);
				if (GetOpKindCount(group).immCount > 0)
					yield return (GetMethod(group, true), group);
			}
		}

		static void AddCodeArg(CreateMethod method) => method.Args.Add(new MethodArg("Code value", MethodArgType.Code, "code"));
		void AddAddressSizeArg(CreateMethod method) => method.Args.Add(new MethodArg("16, 32, or 64", MethodArgType.PreferedInt32, "addressSize"));
		void AddSegmentPrefixArg(CreateMethod method) => method.Args.Add(new MethodArg("Segment override or #(e:Register.None)#", MethodArgType.Register, "segmentPrefix", RegisterEnum.Instance[nameof(Register.None)]));
		void AddRepPrefixArg(CreateMethod method) => method.Args.Add(new MethodArg("Rep prefix or #(e:RepPrefixKind.None)#", MethodArgType.RepPrefixKind, "repPrefix", RepPrefixKindEnum.Instance[nameof(RepPrefixKind.None)]));

		CreateMethod GetMethod(InstructionGroup group, bool unsigned) {
			var (regCount, immCount, memCount) = GetOpKindCount(group);
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
				switch (op) {
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

		static (int regCount, int immCount, int memCount) GetOpKindCount(InstructionGroup group) {
			int regCount = 0;
			int immCount = 0;
			int memCount = 0;

			foreach (var op in group.Operands) {
				switch (op) {
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

		void GenerateCreateBranch(FileWriter writer) {
			writer.WriteLine();
			var method = new CreateMethod("Creates a new near/short branch instruction");
			AddCodeArg(method);
			method.Args.Add(new MethodArg("Target address", MethodArgType.UInt64, "target"));
			GenCreateBranch(writer, method);
		}

		void GenerateCreateFarBranch(FileWriter writer) {
			writer.WriteLine();
			var method = new CreateMethod("Creates a new far branch instruction");
			AddCodeArg(method);
			method.Args.Add(new MethodArg("Selector/segment value", MethodArgType.UInt16, "selector"));
			method.Args.Add(new MethodArg("Offset", MethodArgType.UInt32, "offset"));
			GenCreateFarBranch(writer, method);
		}

		void GenCreateMemory64(FileWriter writer) {
			writer.WriteLine();
			{
				var method = new CreateMethod("Creates an instruction with a 64-bit memory offset as the second operand, eg. #(c:mov al,[123456789ABCDEF0])#");
				AddCodeArg(method);
				method.Args.Add(new MethodArg("Register (#(c:AL)#, #(c:AX)#, #(c:EAX)#, #(c:RAX)#)", MethodArgType.Register, "register"));
				method.Args.Add(new MethodArg("64-bit address", MethodArgType.UInt64, "address"));
				AddSegmentPrefixArg(method);
				GenCreateMemory64(writer, method);
			}
			writer.WriteLine();
			{
				var method = new CreateMethod("Creates an instruction with a 64-bit memory offset as the first operand, eg. #(c:mov [123456789ABCDEF0],al)#");
				AddCodeArg(method);
				method.Args.Add(new MethodArg("64-bit address", MethodArgType.UInt64, "address"));
				method.Args.Add(new MethodArg("Register (#(c:AL)#, #(c:AX)#, #(c:EAX)#, #(c:RAX)#)", MethodArgType.Register, "register"));
				AddSegmentPrefixArg(method);
				GenCreateMemory64(writer, method);
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
			Gen(writer, "outsb", CodeEnum.Instance[nameof(Code.Outsb_DX_m8)], RegisterEnum.Instance[nameof(Register.DX)]);
			Gen(writer, "outsw", CodeEnum.Instance[nameof(Code.Outsw_DX_m16)], RegisterEnum.Instance[nameof(Register.DX)]);
			Gen(writer, "outsd", CodeEnum.Instance[nameof(Code.Outsd_DX_m32)], RegisterEnum.Instance[nameof(Register.DX)]);
			Gen(writer, "lodsb", CodeEnum.Instance[nameof(Code.Lodsb_AL_m8)], RegisterEnum.Instance[nameof(Register.AL)]);
			Gen(writer, "lodsw", CodeEnum.Instance[nameof(Code.Lodsw_AX_m16)], RegisterEnum.Instance[nameof(Register.AX)]);
			Gen(writer, "lodsd", CodeEnum.Instance[nameof(Code.Lodsd_EAX_m32)], RegisterEnum.Instance[nameof(Register.EAX)]);
			Gen(writer, "lodsq", CodeEnum.Instance[nameof(Code.Lodsq_RAX_m64)], RegisterEnum.Instance[nameof(Register.RAX)]);

			void Gen(FileWriter writer, string mnemonic, EnumValue code, EnumValue register) {
				var mnemonicUpper = mnemonic.ToUpperInvariant();
				var baseName = mnemonicUpper.Substring(0, 1) + mnemonicUpper.Substring(1).ToLowerInvariant();

				{
					writer.WriteLine();
					var method = new CreateMethod($"Creates a #(c:{mnemonicUpper})# instruction");
					var name = baseName;
					AddAddressSizeArg(method);
					AddSegmentPrefixArg(method);
					AddRepPrefixArg(method);
					GenCreateString_Reg_SegRSI(writer, method, StringMethodKind.Full, name, code, register);
				}
				{
					writer.WriteLine();
					var method = new CreateMethod($"Creates a #(c:REP {mnemonicUpper})# instruction");
					var name = "Rep" + baseName;
					AddAddressSizeArg(method);
					GenCreateString_Reg_SegRSI(writer, method, StringMethodKind.Rep, name, code, register);
				}
			}
		}

		void GenCreateString_Reg_ESRDI(FileWriter writer) {
			Gen(writer, "scasb", CodeEnum.Instance[nameof(Code.Scasb_AL_m8)], RegisterEnum.Instance[nameof(Register.AL)]);
			Gen(writer, "scasw", CodeEnum.Instance[nameof(Code.Scasw_AX_m16)], RegisterEnum.Instance[nameof(Register.AX)]);
			Gen(writer, "scasd", CodeEnum.Instance[nameof(Code.Scasd_EAX_m32)], RegisterEnum.Instance[nameof(Register.EAX)]);
			Gen(writer, "scasq", CodeEnum.Instance[nameof(Code.Scasq_RAX_m64)], RegisterEnum.Instance[nameof(Register.RAX)]);

			void Gen(FileWriter writer, string mnemonic, EnumValue code, EnumValue register) {
				var mnemonicUpper = mnemonic.ToUpperInvariant();
				var baseName = mnemonicUpper.Substring(0, 1) + mnemonicUpper.Substring(1).ToLowerInvariant();

				{
					writer.WriteLine();
					var method = new CreateMethod($"Creates a #(c:{mnemonicUpper})# instruction");
					var name = baseName;
					AddAddressSizeArg(method);
					AddRepPrefixArg(method);
					GenCreateString_Reg_ESRDI(writer, method, StringMethodKind.Full, name, code, register);
				}
				{
					writer.WriteLine();
					var method = new CreateMethod($"Creates a #(c:REPE {mnemonicUpper})# instruction");
					var name = "Repe" + baseName;
					AddAddressSizeArg(method);
					GenCreateString_Reg_ESRDI(writer, method, StringMethodKind.Repe, name, code, register);
				}
				{
					writer.WriteLine();
					var method = new CreateMethod($"Creates a #(c:REPNE {mnemonicUpper})# instruction");
					var name = "Repne" + baseName;
					AddAddressSizeArg(method);
					GenCreateString_Reg_ESRDI(writer, method, StringMethodKind.Repne, name, code, register);
				}
			}
		}

		void GenCreateString_ESRDI_Reg(FileWriter writer) {
			Gen(writer, "insb", CodeEnum.Instance[nameof(Code.Insb_m8_DX)], RegisterEnum.Instance[nameof(Register.DX)]);
			Gen(writer, "insw", CodeEnum.Instance[nameof(Code.Insw_m16_DX)], RegisterEnum.Instance[nameof(Register.DX)]);
			Gen(writer, "insd", CodeEnum.Instance[nameof(Code.Insd_m32_DX)], RegisterEnum.Instance[nameof(Register.DX)]);
			Gen(writer, "stosb", CodeEnum.Instance[nameof(Code.Stosb_m8_AL)], RegisterEnum.Instance[nameof(Register.AL)]);
			Gen(writer, "stosw", CodeEnum.Instance[nameof(Code.Stosw_m16_AX)], RegisterEnum.Instance[nameof(Register.AX)]);
			Gen(writer, "stosd", CodeEnum.Instance[nameof(Code.Stosd_m32_EAX)], RegisterEnum.Instance[nameof(Register.EAX)]);
			Gen(writer, "stosq", CodeEnum.Instance[nameof(Code.Stosq_m64_RAX)], RegisterEnum.Instance[nameof(Register.RAX)]);

			void Gen(FileWriter writer, string mnemonic, EnumValue code, EnumValue register) {
				var mnemonicUpper = mnemonic.ToUpperInvariant();
				var baseName = mnemonicUpper.Substring(0, 1) + mnemonicUpper.Substring(1).ToLowerInvariant();

				{
					writer.WriteLine();
					var method = new CreateMethod($"Creates a #(c:{mnemonicUpper})# instruction");
					var name = baseName;
					AddAddressSizeArg(method);
					AddRepPrefixArg(method);
					GenCreateString_ESRDI_Reg(writer, method, StringMethodKind.Full, name, code, register);
				}
				{
					writer.WriteLine();
					var method = new CreateMethod($"Creates a #(c:REP {mnemonicUpper})# instruction");
					var name = "Rep" + baseName;
					AddAddressSizeArg(method);
					GenCreateString_ESRDI_Reg(writer, method, StringMethodKind.Rep, name, code, register);
				}
			}
		}

		void GenCreateString_SegRSI_ESRDI(FileWriter writer) {
			Gen(writer, "cmpsb", CodeEnum.Instance[nameof(Code.Cmpsb_m8_m8)]);
			Gen(writer, "cmpsw", CodeEnum.Instance[nameof(Code.Cmpsw_m16_m16)]);
			Gen(writer, "cmpsd", CodeEnum.Instance[nameof(Code.Cmpsd_m32_m32)]);
			Gen(writer, "cmpsq", CodeEnum.Instance[nameof(Code.Cmpsq_m64_m64)]);

			void Gen(FileWriter writer, string mnemonic, EnumValue code) {
				var mnemonicUpper = mnemonic.ToUpperInvariant();
				var baseName = mnemonicUpper.Substring(0, 1) + mnemonicUpper.Substring(1).ToLowerInvariant();

				{
					writer.WriteLine();
					var method = new CreateMethod($"Creates a #(c:{mnemonicUpper})# instruction");
					var name = baseName;
					AddAddressSizeArg(method);
					AddSegmentPrefixArg(method);
					AddRepPrefixArg(method);
					GenCreateString_SegRSI_ESRDI(writer, method, StringMethodKind.Full, name, code);
				}
				{
					writer.WriteLine();
					var method = new CreateMethod($"Creates a #(c:REPE {mnemonicUpper})# instruction");
					var name = "Repe" + baseName;
					AddAddressSizeArg(method);
					GenCreateString_SegRSI_ESRDI(writer, method, StringMethodKind.Repe, name, code);
				}
				{
					writer.WriteLine();
					var method = new CreateMethod($"Creates a #(c:REPNE {mnemonicUpper})# instruction");
					var name = "Repne" + baseName;
					AddAddressSizeArg(method);
					GenCreateString_SegRSI_ESRDI(writer, method, StringMethodKind.Repne, name, code);
				}
			}
		}

		void GenCreateString_ESRDI_SegRSI(FileWriter writer) {
			Gen(writer, "movsb", CodeEnum.Instance[nameof(Code.Movsb_m8_m8)]);
			Gen(writer, "movsw", CodeEnum.Instance[nameof(Code.Movsw_m16_m16)]);
			Gen(writer, "movsd", CodeEnum.Instance[nameof(Code.Movsd_m32_m32)]);
			Gen(writer, "movsq", CodeEnum.Instance[nameof(Code.Movsq_m64_m64)]);

			void Gen(FileWriter writer, string mnemonic, EnumValue code) {
				var mnemonicUpper = mnemonic.ToUpperInvariant();
				var baseName = mnemonicUpper.Substring(0, 1) + mnemonicUpper.Substring(1).ToLowerInvariant();

				{
					writer.WriteLine();
					var method = new CreateMethod($"Creates a #(c:{mnemonicUpper})# instruction");
					var name = baseName;
					AddAddressSizeArg(method);
					AddSegmentPrefixArg(method);
					AddRepPrefixArg(method);
					GenCreateString_ESRDI_SegRSI(writer, method, StringMethodKind.Full, name, code);
				}
				{
					writer.WriteLine();
					var method = new CreateMethod($"Creates a #(c:REP {mnemonicUpper})# instruction");
					var name = "Rep" + baseName;
					AddAddressSizeArg(method);
					GenCreateString_ESRDI_SegRSI(writer, method, StringMethodKind.Rep, name, code);
				}
			}
		}

		void GenCreateMaskmov(FileWriter writer) {
			Gen(writer, "maskmovq", CodeEnum.Instance[nameof(Code.Maskmovq_rDI_mm_mm)]);
			Gen(writer, "maskmovdqu", CodeEnum.Instance[nameof(Code.Maskmovdqu_rDI_xmm_xmm)]);
			Gen(writer, "vmaskmovdqu", CodeEnum.Instance[nameof(Code.VEX_Vmaskmovdqu_rDI_xmm_xmm)]);

			void Gen(FileWriter writer, string mnemonic, EnumValue code) {
				var mnemonicUpper = mnemonic.ToUpperInvariant();
				var baseName = mnemonicUpper.Substring(0, 1) + mnemonicUpper.Substring(1).ToLowerInvariant();

				writer.WriteLine();
				var method = new CreateMethod($"Creates a #(c:{mnemonicUpper})# instruction");
				var name = baseName;
				AddAddressSizeArg(method);
				method.Args.Add(new MethodArg("Register", MethodArgType.Register, "register1"));
				method.Args.Add(new MethodArg("Register", MethodArgType.Register, "register2"));
				AddSegmentPrefixArg(method);
				GenCreateMaskmov(writer, method, name, code);
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
			GenCreateDeclareByte(writer);
			GenCreateDeclareWord(writer);
			GenCreateDeclareDword(writer);
			GenCreateDeclareQword(writer);
		}

		static class DeclareConsts {
			public const string dbDoc = "Create a #(c:db)#/#(c:.byte)# asm directive";
			public const string dwDoc = "Create a #(c:dw)#/#(c:.word)# asm directive";
			public const string ddDoc = "Create a #(c:dd)#/#(c:.int)# asm directive";
			public const string dqDoc = "Create a #(c:dq)#/#(c:.quad)# asm directive";
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
				var method = new CreateMethod(DeclareConsts.dbDoc);
				for (int i = 0; i < args; i++)
					method.Args.Add(new MethodArg($"Byte {i}", MethodArgType.UInt8, $"b{i}"));
				GenCreateDeclareData(writer, method, DeclareDataKind.Byte);
			}
			{
				var method = new CreateMethod(DeclareConsts.dbDoc);
				method.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.BytePtr, DeclareConsts.dataArgName));
				method.Args.Add(new MethodArg(DeclareConsts.lengthBytesDoc, MethodArgType.ArrayLength, DeclareConsts.lengthArgName));
				GenCreateDeclareDataArray(writer, method, DeclareDataKind.Byte, ArrayType.BytePtr);
			}
			{
				var method = new CreateMethod(DeclareConsts.dbDoc);
				method.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.ByteSlice, DeclareConsts.dataArgName));
				GenCreateDeclareDataArray(writer, method, DeclareDataKind.Byte, ArrayType.ByteSlice);
			}
			{
				var method = new CreateMethod(DeclareConsts.dbDoc);
				method.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.ByteArray, DeclareConsts.dataArgName));
				GenCreateDeclareDataArray(writer, method, DeclareDataKind.Byte, ArrayType.ByteArray);
			}
			{
				var method = new CreateMethod(DeclareConsts.dbDoc);
				method.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.ByteArray, DeclareConsts.dataArgName));
				method.Args.Add(new MethodArg(DeclareConsts.indexArgDoc, MethodArgType.ArrayIndex, DeclareConsts.indexArgName));
				method.Args.Add(new MethodArg(DeclareConsts.lengthBytesDoc, MethodArgType.ArrayLength, DeclareConsts.lengthArgName));
				GenCreateDeclareDataArrayLength(writer, method, DeclareDataKind.Byte, ArrayType.ByteArray);
			}
		}

		void GenCreateDeclareWord(FileWriter writer) {
			for (int args = 1; args <= 8; args++) {
				var method = new CreateMethod(DeclareConsts.dwDoc);
				for (int i = 0; i < args; i++)
					method.Args.Add(new MethodArg($"Word {i}", MethodArgType.UInt16, $"w{i}"));
				GenCreateDeclareData(writer, method, DeclareDataKind.Word);
			}
			{
				var method = new CreateMethod(DeclareConsts.dwDoc);
				method.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.BytePtr, DeclareConsts.dataArgName));
				method.Args.Add(new MethodArg(DeclareConsts.lengthBytesDoc, MethodArgType.ArrayLength, DeclareConsts.lengthArgName));
				GenCreateDeclareDataArray(writer, method, DeclareDataKind.Word, ArrayType.BytePtr);
			}
			{
				var method = new CreateMethod(DeclareConsts.dwDoc);
				method.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.ByteSlice, DeclareConsts.dataArgName));
				GenCreateDeclareDataArray(writer, method, DeclareDataKind.Word, ArrayType.ByteSlice);
			}
			{
				var method = new CreateMethod(DeclareConsts.dwDoc);
				method.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.ByteArray, DeclareConsts.dataArgName));
				GenCreateDeclareDataArray(writer, method, DeclareDataKind.Word, ArrayType.ByteArray);
			}
			{
				var method = new CreateMethod(DeclareConsts.dwDoc);
				method.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.ByteArray, DeclareConsts.dataArgName));
				method.Args.Add(new MethodArg(DeclareConsts.indexArgDoc, MethodArgType.ArrayIndex, DeclareConsts.indexArgName));
				method.Args.Add(new MethodArg(DeclareConsts.lengthBytesDoc, MethodArgType.ArrayLength, DeclareConsts.lengthArgName));
				GenCreateDeclareDataArrayLength(writer, method, DeclareDataKind.Word, ArrayType.ByteArray);
			}
			{
				var method = new CreateMethod(DeclareConsts.dwDoc);
				method.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.WordPtr, DeclareConsts.dataArgName));
				method.Args.Add(new MethodArg(DeclareConsts.lengthElemsDoc, MethodArgType.ArrayLength, DeclareConsts.lengthArgName));
				GenCreateDeclareDataArray(writer, method, DeclareDataKind.Word, ArrayType.WordPtr);
			}
			{
				var method = new CreateMethod(DeclareConsts.dwDoc);
				method.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.WordSlice, DeclareConsts.dataArgName));
				GenCreateDeclareDataArray(writer, method, DeclareDataKind.Word, ArrayType.WordSlice);
			}
			{
				var method = new CreateMethod(DeclareConsts.dwDoc);
				method.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.WordArray, DeclareConsts.dataArgName));
				GenCreateDeclareDataArray(writer, method, DeclareDataKind.Word, ArrayType.WordArray);
			}
			{
				var method = new CreateMethod(DeclareConsts.dwDoc);
				method.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.WordArray, DeclareConsts.dataArgName));
				method.Args.Add(new MethodArg(DeclareConsts.indexArgDoc, MethodArgType.ArrayIndex, DeclareConsts.indexArgName));
				method.Args.Add(new MethodArg(DeclareConsts.lengthElemsDoc, MethodArgType.ArrayLength, DeclareConsts.lengthArgName));
				GenCreateDeclareDataArrayLength(writer, method, DeclareDataKind.Word, ArrayType.WordArray);
			}
		}

		void GenCreateDeclareDword(FileWriter writer) {
			for (int args = 1; args <= 4; args++) {
				var method = new CreateMethod(DeclareConsts.ddDoc);
				for (int i = 0; i < args; i++)
					method.Args.Add(new MethodArg($"Dword {i}", MethodArgType.UInt32, $"d{i}"));
				GenCreateDeclareData(writer, method, DeclareDataKind.Dword);
			}
			{
				var method = new CreateMethod(DeclareConsts.ddDoc);
				method.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.BytePtr, DeclareConsts.dataArgName));
				method.Args.Add(new MethodArg(DeclareConsts.lengthBytesDoc, MethodArgType.ArrayLength, DeclareConsts.lengthArgName));
				GenCreateDeclareDataArray(writer, method, DeclareDataKind.Dword, ArrayType.BytePtr);
			}
			{
				var method = new CreateMethod(DeclareConsts.ddDoc);
				method.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.ByteSlice, DeclareConsts.dataArgName));
				GenCreateDeclareDataArray(writer, method, DeclareDataKind.Dword, ArrayType.ByteSlice);
			}
			{
				var method = new CreateMethod(DeclareConsts.ddDoc);
				method.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.ByteArray, DeclareConsts.dataArgName));
				GenCreateDeclareDataArray(writer, method, DeclareDataKind.Dword, ArrayType.ByteArray);
			}
			{
				var method = new CreateMethod(DeclareConsts.ddDoc);
				method.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.ByteArray, DeclareConsts.dataArgName));
				method.Args.Add(new MethodArg(DeclareConsts.indexArgDoc, MethodArgType.ArrayIndex, DeclareConsts.indexArgName));
				method.Args.Add(new MethodArg(DeclareConsts.lengthBytesDoc, MethodArgType.ArrayLength, DeclareConsts.lengthArgName));
				GenCreateDeclareDataArrayLength(writer, method, DeclareDataKind.Dword, ArrayType.ByteArray);
			}
			{
				var method = new CreateMethod(DeclareConsts.ddDoc);
				method.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.DwordPtr, DeclareConsts.dataArgName));
				method.Args.Add(new MethodArg(DeclareConsts.lengthElemsDoc, MethodArgType.ArrayLength, DeclareConsts.lengthArgName));
				GenCreateDeclareDataArray(writer, method, DeclareDataKind.Dword, ArrayType.DwordPtr);
			}
			{
				var method = new CreateMethod(DeclareConsts.ddDoc);
				method.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.DwordSlice, DeclareConsts.dataArgName));
				GenCreateDeclareDataArray(writer, method, DeclareDataKind.Dword, ArrayType.DwordSlice);
			}
			{
				var method = new CreateMethod(DeclareConsts.ddDoc);
				method.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.DwordArray, DeclareConsts.dataArgName));
				GenCreateDeclareDataArray(writer, method, DeclareDataKind.Dword, ArrayType.DwordArray);
			}
			{
				var method = new CreateMethod(DeclareConsts.ddDoc);
				method.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.DwordArray, DeclareConsts.dataArgName));
				method.Args.Add(new MethodArg(DeclareConsts.indexArgDoc, MethodArgType.ArrayIndex, DeclareConsts.indexArgName));
				method.Args.Add(new MethodArg(DeclareConsts.lengthElemsDoc, MethodArgType.ArrayLength, DeclareConsts.lengthArgName));
				GenCreateDeclareDataArrayLength(writer, method, DeclareDataKind.Dword, ArrayType.DwordArray);
			}
		}

		void GenCreateDeclareQword(FileWriter writer) {
			for (int args = 1; args <= 2; args++) {
				var method = new CreateMethod(DeclareConsts.dqDoc);
				for (int i = 0; i < args; i++)
					method.Args.Add(new MethodArg($"Qword {i}", MethodArgType.UInt64, $"q{i}"));
				GenCreateDeclareData(writer, method, DeclareDataKind.Qword);
			}
			{
				var method = new CreateMethod(DeclareConsts.dqDoc);
				method.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.BytePtr, DeclareConsts.dataArgName));
				method.Args.Add(new MethodArg(DeclareConsts.lengthBytesDoc, MethodArgType.ArrayLength, DeclareConsts.lengthArgName));
				GenCreateDeclareDataArray(writer, method, DeclareDataKind.Qword, ArrayType.BytePtr);
			}
			{
				var method = new CreateMethod(DeclareConsts.dqDoc);
				method.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.ByteSlice, DeclareConsts.dataArgName));
				GenCreateDeclareDataArray(writer, method, DeclareDataKind.Qword, ArrayType.ByteSlice);
			}
			{
				var method = new CreateMethod(DeclareConsts.dqDoc);
				method.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.ByteArray, DeclareConsts.dataArgName));
				GenCreateDeclareDataArray(writer, method, DeclareDataKind.Qword, ArrayType.ByteArray);
			}
			{
				var method = new CreateMethod(DeclareConsts.dqDoc);
				method.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.ByteArray, DeclareConsts.dataArgName));
				method.Args.Add(new MethodArg(DeclareConsts.indexArgDoc, MethodArgType.ArrayIndex, DeclareConsts.indexArgName));
				method.Args.Add(new MethodArg(DeclareConsts.lengthBytesDoc, MethodArgType.ArrayLength, DeclareConsts.lengthArgName));
				GenCreateDeclareDataArrayLength(writer, method, DeclareDataKind.Qword, ArrayType.ByteArray);
			}
			{
				var method = new CreateMethod(DeclareConsts.dqDoc);
				method.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.QwordPtr, DeclareConsts.dataArgName));
				method.Args.Add(new MethodArg(DeclareConsts.lengthElemsDoc, MethodArgType.ArrayLength, DeclareConsts.lengthArgName));
				GenCreateDeclareDataArray(writer, method, DeclareDataKind.Qword, ArrayType.QwordPtr);
			}
			{
				var method = new CreateMethod(DeclareConsts.dqDoc);
				method.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.QwordSlice, DeclareConsts.dataArgName));
				GenCreateDeclareDataArray(writer, method, DeclareDataKind.Qword, ArrayType.QwordSlice);
			}
			{
				var method = new CreateMethod(DeclareConsts.dqDoc);
				method.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.QwordArray, DeclareConsts.dataArgName));
				GenCreateDeclareDataArray(writer, method, DeclareDataKind.Qword, ArrayType.QwordArray);
			}
			{
				var method = new CreateMethod(DeclareConsts.dqDoc);
				method.Args.Add(new MethodArg(DeclareConsts.dataArgDoc, MethodArgType.QwordArray, DeclareConsts.dataArgName));
				method.Args.Add(new MethodArg(DeclareConsts.indexArgDoc, MethodArgType.ArrayIndex, DeclareConsts.indexArgName));
				method.Args.Add(new MethodArg(DeclareConsts.lengthElemsDoc, MethodArgType.ArrayLength, DeclareConsts.lengthArgName));
				GenCreateDeclareDataArrayLength(writer, method, DeclareDataKind.Qword, ArrayType.QwordArray);
			}
		}
	}
}
