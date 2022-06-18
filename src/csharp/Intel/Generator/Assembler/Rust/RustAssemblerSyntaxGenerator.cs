// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Generator.Encoder.Rust;
using Generator.Enums;
using Generator.Enums.Decoder;
using Generator.Enums.Encoder;
using Generator.IO;
using Generator.Tables;

namespace Generator.Assembler.Rust {
	[Generator(TargetLanguage.Rust)]
	sealed class RustAssemblerSyntaxGenerator : AssemblerSyntaxGenerator {
		const string AsmRegisterPrefix = "AsmRegister";
		const string AsmMemoryOperand = "AsmMemoryOperand";
		const string CodeLabel = "CodeLabel";
		const string CodeAssembler = "CodeAssembler";
		const string CodeAsmOpState = "CodeAsmOpState";
		const string ErrorType = "IcedError";
		const string CreatedLabelName = "lbl";
		const string FirstLabelIdName = "FIRST_LABEL_ID";
		readonly IdentifierConverter idConverter;
		readonly EnumType registerType;
		readonly EnumType memoryOperandSizeType;
		bool useInt32Suffix;

		public RustAssemblerSyntaxGenerator(GeneratorContext generatorContext)
			: base(generatorContext.Types) {
			idConverter = RustIdentifierConverter.Create();
			registerType = genTypes[TypeIds.Register];
			memoryOperandSizeType = genTypes[TypeIds.CodeAsmMemoryOperandSize];
			useInt32Suffix = true;
		}

		protected override void GenerateRegisters((RegisterKind kind, RegisterDef[] regs)[] regGroups) {
			var filename = genTypes.Dirs.GetRustFilename("code_asm", "registers.rs");
			using (var writer = new FileWriter(TargetLanguage.Rust, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();
				writer.WriteLine("//! This module contains all registers that can be used.");
				writer.WriteLine("//!");
				writer.WriteLine("//! All register identifiers (eg. `eax`, `cr8`) are part of the public API but the");
				writer.WriteLine("//! register *types* are *not*! They're an implementation detail.");
				writer.WriteLine("//!");
				writer.WriteLine("//! To use the registers, you must import everything from the module:");
				writer.WriteLine("//!");
				writer.WriteLine("//! ```");
				writer.WriteLine("//! # #![allow(unused_imports)]");
				writer.WriteLine("//! use iced_x86::code_asm::*;");
				writer.WriteLine("//! ```");
				writer.WriteLine("//!");
				writer.WriteLine("//! or import them from this module:");
				writer.WriteLine("//!");
				writer.WriteLine("//! ```");
				writer.WriteLine("//! # #![allow(unused_imports)]");
				writer.WriteLine("//! use iced_x86::code_asm::registers::*;");
				writer.WriteLine("//! ```");
				writer.WriteLine("//!");
				writer.WriteLine("//! or only the registers you need:");
				writer.WriteLine("//!");
				writer.WriteLine("//! ```");
				writer.WriteLine("//! # #![allow(unused_imports)]");
				writer.WriteLine("//! use iced_x86::code_asm::registers::gpr32::*;");
				writer.WriteLine("//! use iced_x86::code_asm::registers::gpr64::*;");
				writer.WriteLine("//! use iced_x86::code_asm::registers::xmm::*;");
				writer.WriteLine("//! ```");

				var registerTypeName = registerType.Name(idConverter);
				foreach (var (kind, regs) in regGroups) {
					Array.Sort(regs, (a, b) => a.Register.Value.CompareTo(b.Register.Value));

					var asmInfo = GetAsmRegisterInfo(kind);
					writer.WriteLine();
					writer.WriteLine(RustConstants.AttributeNoRustFmt);
					writer.WriteLine($"pub mod {asmInfo.ModName} {{");
					using (writer.Indent()) {
						writer.WriteLine($"//! {asmInfo.Doc}");
						writer.WriteLine("#![allow(non_upper_case_globals)]");
						writer.WriteLine("#![allow(missing_docs)]");
						writer.WriteLine($"use crate::code_asm::reg::{asmInfo.StructName};");
						writer.WriteLine($"use crate::{registerTypeName};");
						foreach (var regDef in regs) {
							var asmRegName = regDef.GetAsmRegisterName();
							writer.WriteLine($"pub const {asmRegName}: {asmInfo.StructName} = {asmInfo.StructName}::new({idConverter.ToDeclTypeAndValue(regDef.Register)});");
						}
						var aOrAn = kind switch {
							RegisterKind.IP or RegisterKind.ST or RegisterKind.MM or RegisterKind.XMM => "an",
							RegisterKind.GPR8 or RegisterKind.GPR16 or RegisterKind.GPR32 or RegisterKind.GPR64 or
							RegisterKind.Segment or RegisterKind.CR or RegisterKind.DR or RegisterKind.TR or
							RegisterKind.BND or RegisterKind.K or RegisterKind.YMM or RegisterKind.ZMM or
							RegisterKind.TMM => "a",
							_ => throw new InvalidOperationException(),
						};
						writer.WriteLine($"/// Gets {aOrAn} `{asmInfo.ModName.ToUpperInvariant()}` register or `None` if input is invalid.");
						writer.WriteLine(RustConstants.AttributeMustUse);
						writer.WriteLine(RustConstants.AttributeInline);
						writer.WriteLine($"pub fn get_{asmInfo.ModName}(register: Register) -> Option<{asmInfo.StructName}> {{");
						using (writer.Indent()) {
							writer.WriteLine($"if register.{asmInfo.FnIsRegName}() {{");
							using (writer.Indent())
								writer.WriteLine($"Some({asmInfo.StructName}::new(register))");
							writer.WriteLine("} else {");
							using (writer.Indent())
								writer.WriteLine("None");
							writer.WriteLine("}");
						}
						writer.WriteLine("}");
					}
					writer.WriteLine("}");
				}

				writer.WriteLine();
				foreach (var (kind, regs) in regGroups.OrderBy(a => GetAsmRegisterInfo(a.kind).ModName, StringComparer.Ordinal)) {
					var modName = GetAsmRegisterInfo(kind).ModName;
					writer.WriteLine($"pub use self::{modName}::*;");
				}
			}
		}

		readonly struct AsmRegisterInfo {
			public readonly string StructName;
			public readonly string ModName;
			public readonly string FnIsRegName;
			public readonly string Doc;

			public AsmRegisterInfo(string structName, string modName, string fnIsRegName, string doc) {
				StructName = structName;
				ModName = modName;
				FnIsRegName = fnIsRegName;
				Doc = doc;
			}
		}

		static AsmRegisterInfo GetAsmRegisterInfo(RegisterKind kind) =>
			kind switch {
				RegisterKind.None => throw new InvalidOperationException(),
				RegisterKind.GPR8 => new(AsmRegisterPrefix + "8", "gpr8", "is_gpr8", "All 8-bit general purpose registers."),
				RegisterKind.GPR16 => new(AsmRegisterPrefix + "16", "gpr16", "is_gpr16", "All 16-bit general purpose registers."),
				RegisterKind.GPR32 => new(AsmRegisterPrefix + "32", "gpr32", "is_gpr32", "All 32-bit general purpose registers."),
				RegisterKind.GPR64 => new(AsmRegisterPrefix + "64", "gpr64", "is_gpr64", "All 64-bit general purpose registers."),
				RegisterKind.IP => new(AsmRegisterPrefix + "Ip", "ip", "is_ip", "All instruction pointer registers."),
				RegisterKind.Segment => new(AsmRegisterPrefix + "Segment", "segment", "is_segment_register", "All segment registers."),
				RegisterKind.ST => new(AsmRegisterPrefix + "St", "st", "is_st", "All FPU registers."),
				RegisterKind.CR => new(AsmRegisterPrefix + "Cr", "cr", "is_cr", "All control registers."),
				RegisterKind.DR => new(AsmRegisterPrefix + "Dr", "dr", "is_dr", "All debug registers."),
				RegisterKind.TR => new(AsmRegisterPrefix + "Tr", "tr", "is_tr", "All test registers."),
				RegisterKind.BND => new(AsmRegisterPrefix + "Bnd", "bnd", "is_bnd", "All bound registers."),
				RegisterKind.K => new(AsmRegisterPrefix + "K", "k", "is_k", "All opmask registers."),
				RegisterKind.MM => new(AsmRegisterPrefix + "Mm", "mm", "is_mm", "All MMX registers."),
				RegisterKind.XMM => new(AsmRegisterPrefix + "Xmm", "xmm", "is_xmm", "All 128-bit vector registers (XMM)."),
				RegisterKind.YMM => new(AsmRegisterPrefix + "Ymm", "ymm", "is_ymm", "All 256-bit vector registers (YMM)."),
				RegisterKind.ZMM => new(AsmRegisterPrefix + "Zmm", "zmm", "is_zmm", "All 512-bit vector registers (ZMM)."),
				RegisterKind.TMM => new(AsmRegisterPrefix + "Tmm", "tmm", "is_tmm", "All tile registers."),
				_ => throw new InvalidOperationException(),
			};

		protected override void GenerateRegisterClasses(RegisterClassInfo[] infos) {
			var filename = genTypes.Dirs.GetRustFilename("code_asm", "reg.rs");
			using (var writer = new FileWriter(TargetLanguage.Rust, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();

				var registerTypeName = registerType.Name(idConverter);

				writer.WriteLine($"use crate::code_asm::op_state::{CodeAsmOpState};");
				writer.WriteLine($"use crate::{registerTypeName};");
				foreach (var reg in infos) {
					var asmInfo = GetAsmRegisterInfo(reg.Kind);

					writer.WriteLine();
					writer.WriteLine($"/// {asmInfo.Doc}");
					writer.WriteLine("///");
					writer.WriteLine("/// This type is *not* part of the public API! It's an implementation detail.");
					writer.WriteLine("/// The register identifiers, however, *are* part of the public API.");
					writer.WriteLine("///");
					writer.WriteLine("/// To use the registers, you must import everything from the module:");
					writer.WriteLine("///");
					writer.WriteLine("/// ```");
					writer.WriteLine("/// # #![allow(unused_imports)]");
					writer.WriteLine("/// use iced_x86::code_asm::*;");
					writer.WriteLine("/// ```");
					writer.WriteLine("///");
					writer.WriteLine("/// or import them from this module:");
					writer.WriteLine("///");
					writer.WriteLine("/// ```");
					writer.WriteLine("/// # #![allow(unused_imports)]");
					writer.WriteLine("/// use iced_x86::code_asm::registers::*;");
					writer.WriteLine("/// ```");
					writer.WriteLine("///");
					writer.WriteLine("/// or import only these registers:");
					writer.WriteLine("///");
					writer.WriteLine("/// ```");
					writer.WriteLine("/// # #![allow(unused_imports)]");
					writer.WriteLine($"/// use iced_x86::code_asm::registers::{asmInfo.ModName}::*;");
					writer.WriteLine("/// ```");
					writer.WriteLine("#[derive(Debug, Copy, Clone, Eq, PartialEq)]");
					writer.WriteLine(RustConstants.AttributeNoRustFmt);
					if (!reg.NeedsState)
						writer.WriteLine(RustConstants.AttrTransparent);
					writer.WriteLine($"pub struct {asmInfo.StructName} {{");
					using (writer.Indent()) {
						writer.WriteLine($"register: {registerTypeName},");
						if (reg.NeedsState)
							writer.WriteLine($"state: {CodeAsmOpState},");
					}
					writer.WriteLine("}");
					writer.WriteLine();
					writer.WriteLine(RustConstants.AttributeNoRustFmt);
					writer.WriteLine($"impl {asmInfo.StructName} {{");
					using (writer.Indent()) {
						writer.WriteLine(RustConstants.AttributeMustUse);
						writer.WriteLine(RustConstants.AttributeInline);
						writer.WriteLine($"pub(crate) const fn new(register: {registerTypeName}) -> Self {{");
						using (writer.Indent()) {
							if (reg.NeedsState)
								writer.WriteLine($"Self {{ register, state: {CodeAsmOpState}::new() }}");
							else
								writer.WriteLine("Self { register }");
						}
						writer.WriteLine("}");
						writer.WriteLine();
						// This method is needed because we can't call into() to get a Register since we call a generic method (with*()).
						// Instead, we just call this one to get the register.
						writer.WriteLine(RustConstants.AttributeMustUse);
						writer.WriteLine(RustConstants.AttributeInline);
						writer.WriteLine($"pub(crate) fn register(&self) -> {registerTypeName} {{");
						using (writer.Indent())
							writer.WriteLine("self.register");
						writer.WriteLine("}");
						if (reg.NeedsState) {
							writer.WriteLine();
							writer.WriteLine(RustConstants.AttributeMustUse);
							writer.WriteLine(RustConstants.AttributeInline);
							writer.WriteLine($"pub(crate) fn state(&self) -> {CodeAsmOpState} {{");
							using (writer.Indent())
								writer.WriteLine("self.state");
							writer.WriteLine("}");
							for (int i = 1; i < 8; i++) {
								writer.WriteLine();
								writer.WriteLine($"/// Adds a `{{k{i}}}` opmask register");
								writer.WriteLine("#[must_use]");
								writer.WriteLine("#[inline]");
								writer.WriteLine($"pub fn k{i}(mut self) -> Self {{");
								using (writer.Indent()) {
									writer.WriteLine($"self.state.set_k{i}();");
									writer.WriteLine("self");
								}
								writer.WriteLine("}");
							}
							writer.WriteLine();
							writer.WriteLine("/// Enables zeroing masking `{z}`");
							writer.WriteLine("#[must_use]");
							writer.WriteLine("#[inline]");
							writer.WriteLine("pub fn z(mut self) -> Self {");
							using (writer.Indent()) {
								writer.WriteLine("self.state.set_zeroing_masking();");
								writer.WriteLine("self");
							}
							writer.WriteLine("}");
							writer.WriteLine();
							writer.WriteLine("/// Enables suppress all exceptions `{sae}`");
							writer.WriteLine("#[must_use]");
							writer.WriteLine("#[inline]");
							writer.WriteLine("pub fn sae(mut self) -> Self {");
							using (writer.Indent()) {
								writer.WriteLine("self.state.set_suppress_all_exceptions();");
								writer.WriteLine("self");
							}
							writer.WriteLine("}");
							var rcInfos = new[] {
								("rn_sae", "Round to nearest (even)"),
								("rd_sae", "Round down (toward -inf)"),
								("ru_sae", "Round up (toward +inf)"),
								("rz_sae", "Round toward zero (truncate)"),
							};
							foreach (var (name, desc) in rcInfos) {
								writer.WriteLine();
								writer.WriteLine($"/// {desc}");
								writer.WriteLine("#[must_use]");
								writer.WriteLine("#[inline]");
								writer.WriteLine($"pub fn {name}(mut self) -> Self {{");
								using (writer.Indent()) {
									writer.WriteLine($"self.state.{name}();");
									writer.WriteLine("self");
								}
								writer.WriteLine("}");
							}
						}
					}
					writer.WriteLine("}");
					writer.WriteLine();
					writer.WriteLine(RustConstants.AttributeNoRustFmt);
					writer.WriteLine($"impl From<{asmInfo.StructName}> for {registerTypeName} {{");
					using (writer.Indent()) {
						writer.WriteLine("#[inline]");
						writer.WriteLine($"fn from(reg: {asmInfo.StructName}) -> Self {{");
						using (writer.Indent())
							writer.WriteLine("reg.register");
						writer.WriteLine("}");
					}
					writer.WriteLine("}");
				}
			}
		}

		static string GetName(MemorySizeFuncInfo fnInfo) => fnInfo.Name.Replace(' ', '_');

		protected override void GenerateMemorySizeFunctions(MemorySizeFuncInfo[] infos) {
			var filename = genTypes.Dirs.GetRustFilename("code_asm", "mem.rs");

			new FileUpdater(TargetLanguage.Rust, "AsmMemoryOperandPtrMethods", filename).Generate(writer => {
				for (int i = 0; i < infos.Length; i++) {
					var info = infos[i];
					var fnName = GetName(info);
					var calledFnName = info.IsBroadcast ? "bcst" : "ptr";

					if (i != 0)
						writer.WriteLine();
					writer.WriteLine(RustConstants.AttributeMustUse);
					writer.WriteLine(RustConstants.AttributeInline);
					writer.WriteLine(RustConstants.AttributeNoRustFmt);
					writer.WriteLine($"pub(crate) fn {fnName}(mut self) -> Self {{");
					using (writer.Indent()) {
						var enumValue = memoryOperandSizeType[info.Size.ToString()];
						writer.WriteLine($"self.state.{calledFnName}({idConverter.ToDeclTypeAndValue(enumValue)});");
						writer.WriteLine("self");
					}
					writer.WriteLine("}");
				}
			});

			new FileUpdater(TargetLanguage.Rust, "GlobalPtrMethods", filename).Generate(writer => {
				for (int i = 0; i < infos.Length; i++) {
					var info = infos[i];
					var fnName = GetName(info);
					var doc = info.GetMethodDocs("Creates", s => $"`{s}`");

					if (i != 0)
						writer.WriteLine();
					writer.WriteLine($"/// {doc}");
					writer.WriteLine("///");
					writer.WriteLine("/// # Examples");
					writer.WriteLine("///");
					writer.WriteLine("/// ```");
					writer.WriteLine("/// use iced_x86::code_asm::*;");
					writer.WriteLine("///");
					writer.WriteLine($"/// let _ = {fnName}(rax);");
					writer.WriteLine($"/// let _ = {fnName}(0x1234_5678).fs();");
					writer.WriteLine($"/// let _ = {fnName}(rdx * 4 + rcx - 123);");
					writer.WriteLine("/// ```");
					writer.WriteLine(RustConstants.AttributeMustUse);
					writer.WriteLine(RustConstants.AttributeInline);
					writer.WriteLine(RustConstants.AttributeNoRustFmt);
					writer.WriteLine($"pub fn {fnName}<M: Into<{AsmMemoryOperand}>>(mem: M) -> {AsmMemoryOperand} {{");
					using (writer.Indent())
						writer.WriteLine($"mem.into().{fnName}()");
					writer.WriteLine("}");
				}
			});
		}

		static string EscapeKeyword(string s) {
			if (rustKeywords.Contains(s))
				return s + "_";
			return s;
		}

		// https://doc.rust-lang.org/reference/keywords.html
		static readonly HashSet<string> rustKeywords = new[] {
			"as", "break", "const", "continue", "crate", "else", "enum", "extern", "false",
			"fn", "for", "if", "impl", "in", "let", "loop", "match", "mod", "move", "mut",
			"pub", "ref", "return", "self", "Self", "static", "struct", "super", "trait",
			"true", "type", "unsafe", "use", "where", "while", "async", "await", "dyn",
			"abstract", "become", "box", "do", "final", "macro", "override", "priv", "typeof",
			"unsized", "virtual", "yield", "try",
		}.ToHashSet(StringComparer.Ordinal);

		sealed class TraitGroup {
			public readonly List<OpCodeInfoGroup> Groups = new();
			public string Name => Groups[0].Name;
			public int ArgCount => Groups[0].Signature.ArgCount;
		}

		static TraitGroup[] CreateTraitGroups(OpCodeInfoGroup[] groups) {
			var dict = new Dictionary<string, Dictionary<int, List<OpCodeInfoGroup>>>(StringComparer.Ordinal);

			foreach (var group in groups) {
				if (!dict.TryGetValue(group.Name, out var traitDict))
					dict.Add(group.Name, traitDict = new());
				if (!traitDict.TryGetValue(group.Signature.ArgCount, out var list))
					traitDict.Add(group.Signature.ArgCount, list = new());
				list.Add(group);
			}

			var result = new List<TraitGroup>(dict.Count);
			foreach (var kv in dict.OrderBy(x => x.Key, StringComparer.Ordinal)) {
				bool addSuffix = false;
				foreach (var list in kv.Value.Select(x => x.Value).OrderBy(x => x[0].Signature.ArgCount)) {
					var traitGroup = new TraitGroup();
					result.Add(traitGroup);
					foreach (var group in list) {
						group.AddNameSuffix = addSuffix;
						traitGroup.Groups.Add(group);
					}
					addSuffix = true;
				}
			}
			return result.ToArray();
		}

		protected override void Generate(Dictionary<GroupKey, OpCodeInfoGroup> map, OpCodeInfoGroup[] groups) {
			var traitGroups = CreateTraitGroups(groups);
			GenerateAsmTraits(traitGroups);
			GenerateAsmPub(traitGroups);
			GenerateAsmImpl(traitGroups);
			GenerateTests(traitGroups);
		}

		static string GetTraitName(TraitGroup traitGroup) => GetTraitName(traitGroup.Groups[0]);
		static string GetTraitName(OpCodeInfoGroup group) {
			var name = "CodeAsm" + char.ToUpperInvariant(group.Name[0]).ToString() + group.Name[1..];
			if (group.AddNameSuffix)
				name += group.Signature.ArgCount.ToString(CultureInfo.InvariantCulture);
			return EscapeKeyword(name);
		}

		static string GetTraitFnName(TraitGroup traitGroup) => GetTraitFnName(traitGroup.Groups[0]);
		static string GetTraitFnName(OpCodeInfoGroup group) {
			if (group.AddNameSuffix)
				return EscapeKeyword(group.Name + "_" + group.Signature.ArgCount.ToString(CultureInfo.InvariantCulture));
			return EscapeKeyword(group.Name);
		}

		static string GetPubFnName(TraitGroup traitGroup) => GetPubFnName(traitGroup.Groups[0]);
		static string GetPubFnName(OpCodeInfoGroup group) => GetTraitFnName(group);
		static string GetFnArgName(int argIndex) => "op" + argIndex.ToString(CultureInfo.InvariantCulture);
		static string GetGenericParameterTypeName(int gpIndex) => gpNames[gpIndex];
		static readonly string[] gpNames = new[] { "T", "U", "V", "W", "X" };

		static void WriteGenericParameterTypes(FileWriter writer, int count) {
			if (count > 0) {
				writer.Write("<");
				for (int i = 0; i < count; i++) {
					if (i != 0)
						writer.Write(", ");
					writer.Write(GetGenericParameterTypeName(i));
				}
				writer.Write(">");
			}
		}

		void GenerateAsmTraits(TraitGroup[] traitGroups) {
			var filename = genTypes.Dirs.GetRustFilename("code_asm", "asm_traits.rs");
			using (var writer = new FileWriter(TargetLanguage.Rust, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();
				writer.WriteLine(RustConstants.InnerAttributeMissingErrorsDoc);
				writer.WriteLine(RustConstants.InnerAttributeAllowMissingDocs);
				writer.WriteLine(RustConstants.InnerAttributeAllowNonCamelCaseTypes);
				writer.WriteLine();
				writer.WriteLine($"use crate::{ErrorType};");

				foreach (var traitGroup in traitGroups) {
					var opCount = traitGroup.ArgCount;
					writer.WriteLine();
					writer.WriteLine(RustConstants.AttributeNoRustFmt);
					var traitName = GetTraitName(traitGroup);
					writer.Write($"pub trait {traitName}");
					WriteGenericParameterTypes(writer, opCount);
					writer.WriteLine(" {");
					using (writer.Indent()) {
						var traitFnName = GetTraitFnName(traitGroup);
						writer.Write($"fn {traitFnName}(&mut self");
						for (int i = 0; i < opCount; i++)
							writer.Write($", {GetFnArgName(i)}: {GetGenericParameterTypeName(i)}");
						writer.WriteLine($") -> Result<(), {ErrorType}>;");
					}
					writer.WriteLine("}");
				}
			}
		}

		static List<InstructionDef> GetSortedDefs(HashSet<InstructionDef> defHash, List<InstructionDef> sortedDefs, TraitGroup traitGroup) {
			defHash.Clear();
			sortedDefs.Clear();

			foreach (var group in traitGroup.Groups) {
				foreach (var def in group.GetDefsAndParentDefs()) {
					if (defHash.Add(def))
						sortedDefs.Add(def);
				}
			}

			if (sortedDefs.Count == 0)
				throw new InvalidOperationException();
			sortedDefs.Sort(CompareInstructionDefs);
			return sortedDefs;
		}

		static int CompareInstructionDefs(InstructionDef? x, InstructionDef? y) {
			if ((object?)x == (object?)y) return 0;
			if (x is null) return -1;
			if (y is null) return 1;
			int c;
			c = x.Table.CompareTo(y.Table);
			if (c != 0) return c;
			c = x.OpCode.CompareTo(y.OpCode);
			if (c != 0) return c;
			c = x.MandatoryPrefix.CompareTo(y.MandatoryPrefix);
			if (c != 0) return c;
			c = x.GroupIndex.CompareTo(y.GroupIndex);
			if (c != 0) return c;
			c = x.RmGroupIndex.CompareTo(y.RmGroupIndex);
			if (c != 0) return c;
			c = x.Encoding.CompareTo(y.Encoding);
			if (c != 0) return c;
			c = x.WBit.CompareTo(y.WBit);
			if (c != 0) return c;
			c = x.LBit.CompareTo(y.LBit);
			if (c != 0) return c;
			c = x.OperandSize.CompareTo(y.OperandSize);
			if (c != 0) return c;
			c = x.AddressSize.CompareTo(y.AddressSize);
			if (c != 0) return c;
			c = GetBitnessCompareValue(x).CompareTo(GetBitnessCompareValue(y));
			if (c != 0) return c;
			c = CompareSignatures(x, y);
			if (c != 0) return c;
			throw new InvalidOperationException($"Couldn't sort: {x.Code.RawName} vs {y.Code.RawName}");

			static int GetBitnessCompareValue(InstructionDef def) {
				int result = 0;
				if ((def.Flags1 & InstructionDefFlags1.Bit16) != 0)
					result |= 1;
				if ((def.Flags1 & InstructionDefFlags1.Bit32) != 0)
					result |= 2;
				if ((def.Flags1 & InstructionDefFlags1.Bit64) != 0)
					result |= 4;
				return result;
			}

			static int CompareSignatures(InstructionDef x, InstructionDef y) {
				var xOps = x.OpKindDefs;
				var yOps = y.OpKindDefs;
				int c = xOps.Length.CompareTo(yOps.Length);
				if (c != 0) return c;
				for (int i = 0; i < xOps.Length; i++) {
					var xOp = xOps[i];
					var yOp = yOps[i];
					c = xOp.HasRegister.CompareTo(yOp.HasRegister);
					if (c != 0) return c;
					if (xOp.HasRegister) {
						c = xOp.Register.CompareTo(yOp.Register);
						if (c != 0) return c;
					}
					c = xOp.Memory.CompareTo(yOp.Memory);
					if (c != 0) return c;
				}

				return 0;
			}
		}

		void GenerateAsmPub(TraitGroup[] traitGroups) {
			var filename = genTypes.Dirs.GetRustFilename("code_asm", "fn_asm_pub.rs");
			using (var writer = new FileWriter(TargetLanguage.Rust, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();
				writer.WriteLine("use crate::code_asm::asm_traits::*;");
				writer.WriteLine($"use crate::code_asm::{CodeAssembler};");
				writer.WriteLine($"use crate::{ErrorType};");
				writer.WriteLine();
				writer.WriteLine(RustConstants.AttributeNoRustFmt);
				writer.WriteLine($"impl {CodeAssembler} {{");
				var defHash = new HashSet<InstructionDef>();
				var sortedDefs = new List<InstructionDef>();
				using (writer.Indent()) {
					var opDescs = new[] {
						"First operand",
						"Second operand",
						"Third operand",
						"Fourth operand",
						"Fifth operand",
					};
					bool lineSep = false;
					foreach (var traitGroup in traitGroups) {
						var opCount = traitGroup.ArgCount;
						var traitName = GetTraitName(traitGroup);
						var traitFnName = GetTraitFnName(traitGroup);
						var pubFnName = GetPubFnName(traitGroup);

						if (lineSep)
							writer.WriteLine();
						lineSep = true;
						writer.WriteLine($"/// `{traitGroup.Name.ToUpperInvariant()}` instruction");
						writer.WriteLine("///");
						writer.WriteLine("/// Instruction | Opcode | CPUID");
						writer.WriteLine("/// ------------|--------|------");
						foreach (var def in GetSortedDefs(defHash, sortedDefs, traitGroup)) {
							var cpuid = string.Join(" ", def.CpuidFeatureStrings);
							writer.WriteLine($"/// `{def.InstructionString}` | `{def.OpCodeString}` | `{cpuid}`");
						}
						writer.WriteLine("///");
						writer.WriteLine("/// # Errors");
						writer.WriteLine("///");
						writer.WriteLine("/// Fails if an operand is invalid (basic checks only)");
						if (opCount > 0) {
							writer.WriteLine("///");
							writer.WriteLine("/// # Arguments");
							writer.WriteLine("///");
							for (int i = 0; i < opCount; i++) {
								var desc = i != 0 ? string.Empty : " (eg. an integer (a `u32`/`i64`/`u64` number suffix is sometimes needed), a register (`rdx`), memory (`dword_ptr(rcx+r13*4)`) or a label)";
								writer.WriteLine($"/// * `{GetFnArgName(i)}`: {opDescs[i]}{desc}");
							}
						}
						writer.WriteLine(RustConstants.AttributeInline);
						writer.Write($"pub fn {pubFnName}");
						WriteGenericParameterTypes(writer, opCount);
						writer.Write($"(&mut self");
						for (int i = 0; i < opCount; i++)
							writer.Write($", {GetFnArgName(i)}: {GetGenericParameterTypeName(i)}");
						writer.WriteLine($") -> Result<(), {ErrorType}>");
						writer.WriteLine("where");
						using (writer.Indent()) {
							writer.Write($"Self: {traitName}");
							WriteGenericParameterTypes(writer, opCount);
							writer.WriteLine(",");
						}
						writer.WriteLine("{");
						using (writer.Indent()) {
							writer.Write($"<Self as {traitName}");
							WriteGenericParameterTypes(writer, opCount);
							writer.Write($">::{traitFnName}(self");
							for (int i = 0; i < opCount; i++)
								writer.Write($", {GetFnArgName(i)}");
							writer.WriteLine(")");
						}
						writer.WriteLine("}");
					}
				}
				writer.WriteLine("}");
			}
		}

		static string ToTypeString(ArgKind argKind, int maxArgSize) =>
			argKind switch {
				ArgKind.Register8 => AsmRegisterPrefix + "8",
				ArgKind.Register16 => AsmRegisterPrefix + "16",
				ArgKind.Register32 => AsmRegisterPrefix + "32",
				ArgKind.Register64 => AsmRegisterPrefix + "64",
				ArgKind.RegisterK => AsmRegisterPrefix + "K",
				ArgKind.RegisterSt => AsmRegisterPrefix + "St",
				ArgKind.RegisterSegment => AsmRegisterPrefix + "Segment",
				ArgKind.RegisterBnd => AsmRegisterPrefix + "Bnd",
				ArgKind.RegisterMm => AsmRegisterPrefix + "Mm",
				ArgKind.RegisterXmm => AsmRegisterPrefix + "Xmm",
				ArgKind.RegisterYmm => AsmRegisterPrefix + "Ymm",
				ArgKind.RegisterZmm => AsmRegisterPrefix + "Zmm",
				ArgKind.RegisterCr => AsmRegisterPrefix + "Cr",
				ArgKind.RegisterDr => AsmRegisterPrefix + "Dr",
				ArgKind.RegisterTr => AsmRegisterPrefix + "Tr",
				ArgKind.RegisterTmm => AsmRegisterPrefix + "Tmm",
				ArgKind.Memory => AsmMemoryOperand,
				ArgKind.Immediate => maxArgSize switch {
					1 or 2 or 4 => "i32",
					8 => "i64",
					_ => throw new InvalidOperationException(),
				},
				ArgKind.ImmediateUnsigned => maxArgSize switch {
					1 or 2 or 4 => "u32",
					8 => "u64",
					_ => throw new InvalidOperationException(),
				},
				ArgKind.Label => CodeLabel,
				ArgKind.LabelU64 => "u64",
				_ => throw new InvalidOperationException($"Invalid arg kind: {argKind}"),
			};

		static void WriteGenericArgumentTypes(FileWriter writer, OpCodeInfoGroup group, int count) {
			for (int i = 0; i < count; i++) {
				if (i != 0)
					writer.Write(", ");
				var maxArgSize = group.MaxArgSizes[i];
				writer.Write(ToTypeString(group.Signature.GetArgKind(i), maxArgSize));
			}
		}

		static bool SpecialInstructionHasSegmentArg(string mnemonicName) =>
			!(mnemonicName.StartsWith("Ins", StringComparison.Ordinal) ||
			mnemonicName.StartsWith("Scas", StringComparison.Ordinal) ||
			mnemonicName.StartsWith("Stos", StringComparison.Ordinal) ||
			mnemonicName.StartsWith("Xbegin", StringComparison.Ordinal));

		void GenerateAsmImpl(TraitGroup[] traitGroups) {
			var filename = genTypes.Dirs.GetRustFilename("code_asm", "fn_asm_impl.rs");
			using (var writer = new FileWriter(TargetLanguage.Rust, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();
				writer.WriteLine("#![allow(clippy::if_same_then_else)]");
				writer.WriteLine("#![allow(clippy::missing_inline_in_public_items)]");
				writer.WriteLine();
				writer.WriteLine("use crate::code_asm::asm_traits::*;");
				writer.WriteLine("use crate::code_asm::mem::*;");
				writer.WriteLine($"use crate::code_asm::op_state::{memoryOperandSizeType.Name(idConverter)};");
				writer.WriteLine("use crate::code_asm::reg::*;");
				writer.WriteLine($"use crate::code_asm::{{{CodeAssembler}, {CodeLabel}}};");
				var codeStr = genTypes[TypeIds.Code].Name(idConverter);
				var registerStr = registerType.Name(idConverter);
				var repPrefixKindStr = genTypes[TypeIds.RepPrefixKind].Name(idConverter);
				writer.WriteLine($"use crate::{{{codeStr}, {ErrorType}, Instruction, {registerStr}, {repPrefixKindStr}}};");

				static void WriteArg(FileWriter writer, string argExpr, ArgKind kind) {
					writer.Write(argExpr);
					if (IsRegister(kind))
						writer.Write(".register()");
					else if (kind == ArgKind.Label)
						writer.Write(".id()");
					else if (kind == ArgKind.Memory)
						writer.Write(".to_memory_operand(self.bitness())");
				}

				var repPrefixNoneStr = idConverter.ToDeclTypeAndValue(genTypes[TypeIds.RepPrefixKind][nameof(RepPrefixKind.None)]);
				var registerNoneStr = idConverter.ToDeclTypeAndValue(registerType[nameof(Register.None)]);

				var stateArgsList = new List<string>();
				foreach (var traitGroup in traitGroups) {
					var traitName = GetTraitName(traitGroup);
					var traitFnName = GetTraitFnName(traitGroup);

					foreach (var group in traitGroup.Groups) {
						writer.WriteLine();
						writer.WriteLine(RustConstants.AttributeNoRustFmt);
						writer.Write($"impl {traitName}");
						var opCount = group.Signature.ArgCount;
						if (opCount > 0) {
							writer.Write("<");
							WriteGenericArgumentTypes(writer, group, opCount);
							writer.Write(">");
						}
						writer.WriteLine($" for {CodeAssembler} {{");
						using (writer.Indent()) {
							// Add #[inline] if the body is short (1-2 lines)
							bool inline = group.ParentPseudoOpsKind is not null ||
								(group.Flags & OpCodeArgFlags.HasSpecialInstructionEncoding) != 0 ||
								group.RootOpCodeNode.Def is InstructionDef ||
								group.RootOpCodeNode.Selector?.IsConditionInlineable == true;
							if (inline)
								writer.WriteLine(RustConstants.AttributeInline);
							writer.Write($"fn {traitFnName}(&mut self");
							for (int i = 0; i < opCount; i++) {
								writer.Write(", ");
								writer.Write($"{GetFnArgName(i)}: ");
								var maxArgSize = group.MaxArgSizes[i];
								writer.Write(ToTypeString(group.Signature.GetArgKind(i), maxArgSize));
							}
							writer.WriteLine($") -> Result<(), {ErrorType}> {{");
							using (writer.Indent()) {
								if (group.ParentPseudoOpsKind is OpCodeInfoGroup parent) {
									writer.Write($"<Self as {GetTraitName(parent)}<");
									WriteGenericArgumentTypes(writer, group, opCount);
									var intType = ToTypeString(parent.Signature.GetArgKind(group.Signature.ArgCount), parent.MaxArgSizes[group.Signature.ArgCount]);
									writer.Write($", {intType}>>::");
									writer.Write(GetTraitFnName(parent));
									writer.Write("(self");
									for (int i = 0; i < group.Signature.ArgCount; i++)
										writer.Write($", {GetFnArgName(i)}");
									writer.WriteLine($", {group.PseudoOpsKindImmediateValue})");
								}
								else if ((group.Flags & OpCodeArgFlags.HasSpecialInstructionEncoding) != 0) {
									writer.Write($"self.add_instr(Instruction::with_{group.MnemonicName.ToLowerInvariant()}(self.bitness()");
									for (int i = 0; i < opCount; i++) {
										writer.Write(", ");
										WriteArg(writer, GetFnArgName(i), group.Signature.GetArgKind(i));
									}
									if (SpecialInstructionHasSegmentArg(group.MnemonicName)) {
										writer.Write(", ");
										writer.Write(registerNoneStr);
									}
									if ((group.Defs[0].Flags3 & InstructionDefFlags3.IsStringOp) != 0) {
										writer.Write(", ");
										writer.Write(repPrefixNoneStr);
									}
									writer.WriteLine(")?)");
								}
								else {
									string codeExpr;
									if (group.RootOpCodeNode.Def is InstructionDef def)
										codeExpr = idConverter.ToDeclTypeAndValue(def.Code);
									else {
										codeExpr = "code";
										writer.Write($"let {codeExpr} = ");
										GenerateCodeEnumSelectorCode(writer, group, group.RootOpCodeNode, ";", false);
									}
									string withFailStr, withFnName;
									if (group.HasLabel) {
										withFailStr = "?";
										withFnName = RustInstrCreateGenNames.with_branch;
									}
									else {
										withFailStr = group.Signature.ArgCount == 0 ? string.Empty : "?";
										withFnName = InstrCreateGenImpl.GetRustOverloadedCreateName(group.Signature.ArgCount);
									}

									stateArgsList.Clear();
									foreach (var index in GetStateArgIndexes(group))
										stateArgsList.Add($"{GetFnArgName(index)}.state()");

									var addInstrName = stateArgsList.Count == 0 ? "add_instr" : "add_instr_with_state";
									writer.Write($"self.{addInstrName}(Instruction::{withFnName}({codeExpr}");
									for (int i = 0; i < group.Signature.ArgCount; i++) {
										writer.Write(", ");
										WriteArg(writer, GetFnArgName(i), group.Signature.GetArgKind(i));
									}
									writer.Write($"){withFailStr}");
									if (stateArgsList.Count != 0) {
										writer.Write(", ");
										for (int i = 0; i < stateArgsList.Count; i++) {
											if (i != 0)
												writer.Write(".merge(");
											writer.Write(stateArgsList[i]);
											if (i != 0)
												writer.Write(")");
										}
									}
									writer.WriteLine($")");
								}
							}
							writer.WriteLine("}");
						}
						writer.WriteLine("}");
					}
				}
			}
		}

		static (ArgKind argKind, int maxArgSize) GetArgInfo(OpCodeInfoGroup group, int argIndex) {
			if (argIndex >= 0)
				return (group.Signature.GetArgKind(argIndex), group.MaxArgSizes[argIndex]);
			else
				return (ArgKind.Unknown, 0);
		}

		void GenerateCodeEnumSelectorCode(FileWriter writer, OpCodeInfoGroup group, OpCodeNode node, string semiColon, bool inElse) {
			if (node.Def is InstructionDef def) {
				if (inElse) {
					writer.WriteLine("{");
					using (writer.Indent())
						writer.WriteLine(idConverter.ToDeclTypeAndValue(def.Code));
					writer.WriteLine($"}}{semiColon}");
				}
				else
					writer.WriteLine(idConverter.ToDeclTypeAndValue(def.Code));
			}
			else if (node.Selector is OpCodeSelector selector) {
				var (argKind, maxArgSize) = GetArgInfo(group, selector.ArgIndex);
				var condition = GetArgConditionForOpCodeKind(selector.ArgIndex, argKind, maxArgSize, selector.Kind);
				if (!inElse && selector.IfTrue.Def is InstructionDef defTrue && selector.IfFalse.Def is InstructionDef defFalse) {
					var trueExpr = idConverter.ToDeclTypeAndValue(defTrue.Code);
					var falseExpr = idConverter.ToDeclTypeAndValue(defFalse.Code);
					writer.WriteLine($"if {condition} {{ {trueExpr} }} else {{ {falseExpr} }}{semiColon}");
				}
				else {
					writer.WriteLine($"if {condition} {{");
					using (writer.Indent())
						GenerateCodeEnumSelectorCode(writer, group, selector.IfTrue, string.Empty, false);
					if (selector.IfFalse.IsEmpty) {
						writer.WriteLine("} else {");
						using (writer.Indent())
							writer.WriteLine($"return Err({ErrorType}::new(\"{group.Name}: invalid operands\"));");
						writer.WriteLine($"}}{semiColon}");
					}
					else {
						writer.Write("} else ");
						GenerateCodeEnumSelectorCode(writer, group, selector.IfFalse, semiColon, true);
					}
				}
			}
			else
				throw new InvalidOperationException();
		}

		string GetArgConditionForOpCodeKind(int argIndex, ArgKind argKind, int maxArgSize, OpCodeSelectorKind selectorKind) {
			var argName = GetFnArgName(argIndex);
			var otherArgName = GetFnArgName(argIndex == 0 ? 1 : 0);
			var argType = argKind == ArgKind.Unknown ? string.Empty : ToTypeString(argKind, maxArgSize);
			return selectorKind switch {
				OpCodeSelectorKind.MemOffs64_RAX => $"{otherArgName}.register() == {GetRegisterString(nameof(Register.RAX))} && self.bitness() == 64 && {argName}.is_displacement_only()",
				OpCodeSelectorKind.MemOffs64_EAX => $"{otherArgName}.register() == {GetRegisterString(nameof(Register.EAX))} && self.bitness() == 64 && {argName}.is_displacement_only()",
				OpCodeSelectorKind.MemOffs64_AX => $"{otherArgName}.register() == {GetRegisterString(nameof(Register.AX))} && self.bitness() == 64 && {argName}.is_displacement_only()",
				OpCodeSelectorKind.MemOffs64_AL => $"{otherArgName}.register() == {GetRegisterString(nameof(Register.AL))} && self.bitness() == 64 && {argName}.is_displacement_only()",
				OpCodeSelectorKind.MemOffs_RAX => $"{otherArgName}.register() == {GetRegisterString(nameof(Register.RAX))} && self.bitness() < 64 && {argName}.is_displacement_only()",
				OpCodeSelectorKind.MemOffs_EAX => $"{otherArgName}.register() == {GetRegisterString(nameof(Register.EAX))} && self.bitness() < 64 && {argName}.is_displacement_only()",
				OpCodeSelectorKind.MemOffs_AX => $"{otherArgName}.register() == {GetRegisterString(nameof(Register.AX))} && self.bitness() < 64 && {argName}.is_displacement_only()",
				OpCodeSelectorKind.MemOffs_AL => $"{otherArgName}.register() == {GetRegisterString(nameof(Register.AL))} && self.bitness() < 64 && {argName}.is_displacement_only()",
				OpCodeSelectorKind.Bitness64 => "self.bitness() == 64",
				OpCodeSelectorKind.Bitness32 => "self.bitness() >= 32",
				OpCodeSelectorKind.Bitness16 => "self.bitness() >= 16",
				OpCodeSelectorKind.ShortBranch => "self.prefer_short_branch()",
				OpCodeSelectorKind.ImmediateByteEqual1 => $"{argName} == 1",
				OpCodeSelectorKind.ImmediateByteSigned8To32 or OpCodeSelectorKind.ImmediateByteSigned8To64 => argKind == ArgKind.ImmediateUnsigned ?
					$"{argName} <= i8::MAX as {argType} || 0xFFFF_FF80 <= {argName}" :
					$"{argName} >= i8::MIN as {argType} && {argName} <= i8::MAX as {argType}",
				OpCodeSelectorKind.ImmediateByteSigned8To16 => argKind == ArgKind.ImmediateUnsigned ?
					$"{argName} <= i8::MAX as {argType} || (0xFF80 <= {argName} && {argName} <= 0xFFFF)" :
					$"{argName} >= i8::MIN as {argType} && {argName} <= i8::MAX as {argType}",
				OpCodeSelectorKind.Vex => "self.instruction_prefer_vex()",
				OpCodeSelectorKind.EvexBroadcastX or OpCodeSelectorKind.EvexBroadcastY or OpCodeSelectorKind.EvexBroadcastZ =>
					$"{argName}.is_broadcast()",
				OpCodeSelectorKind.RegisterCL => $"{argName}.register() == {GetRegisterString(nameof(Register.CL))}",
				OpCodeSelectorKind.RegisterAL => $"{argName}.register() == {GetRegisterString(nameof(Register.AL))}",
				OpCodeSelectorKind.RegisterAX => $"{argName}.register() == {GetRegisterString(nameof(Register.AX))}",
				OpCodeSelectorKind.RegisterEAX => $"{argName}.register() == {GetRegisterString(nameof(Register.EAX))}",
				OpCodeSelectorKind.RegisterRAX => $"{argName}.register() == {GetRegisterString(nameof(Register.RAX))}",
				OpCodeSelectorKind.RegisterBND => $"{argName}.is_bnd()",
				OpCodeSelectorKind.RegisterES => $"{argName}.register() == {GetRegisterString(nameof(Register.ES))}",
				OpCodeSelectorKind.RegisterCS => $"{argName}.register() == {GetRegisterString(nameof(Register.CS))}",
				OpCodeSelectorKind.RegisterSS => $"{argName}.register() == {GetRegisterString(nameof(Register.SS))}",
				OpCodeSelectorKind.RegisterDS => $"{argName}.register() == {GetRegisterString(nameof(Register.DS))}",
				OpCodeSelectorKind.RegisterFS => $"{argName}.register() == {GetRegisterString(nameof(Register.FS))}",
				OpCodeSelectorKind.RegisterGS => $"{argName}.register() == {GetRegisterString(nameof(Register.GS))}",
				OpCodeSelectorKind.RegisterDX => $"{argName}.register() == {GetRegisterString(nameof(Register.DX))}",
				OpCodeSelectorKind.Register8 => $"{argName}.is_gpr8()",
				OpCodeSelectorKind.Register16 => $"{argName}.is_gpr16()",
				OpCodeSelectorKind.Register32 => $"{argName}.is_gpr32()",
				OpCodeSelectorKind.Register64 => $"{argName}.is_gpr64()",
				OpCodeSelectorKind.RegisterK => $"{argName}.is_k()",
				OpCodeSelectorKind.RegisterST0 => $"{argName}.register() == {GetRegisterString(nameof(Register.ST0))}",
				OpCodeSelectorKind.RegisterST => $"{argName}.is_st()",
				OpCodeSelectorKind.RegisterSegment => $"{argName}.is_segment_register()",
				OpCodeSelectorKind.RegisterCR => $"{argName}.is_cr()",
				OpCodeSelectorKind.RegisterDR => $"{argName}.is_dr()",
				OpCodeSelectorKind.RegisterTR => $"{argName}.is_tr()",
				OpCodeSelectorKind.RegisterMM => $"{argName}.is_mm()",
				OpCodeSelectorKind.RegisterXMM => $"{argName}.is_xmm()",
				OpCodeSelectorKind.RegisterYMM => $"{argName}.is_ymm()",
				OpCodeSelectorKind.RegisterZMM => $"{argName}.is_zmm()",
				OpCodeSelectorKind.RegisterTMM => $"{argName}.is_tmm()",
				OpCodeSelectorKind.Memory8 => $"{argName}.size() == {GetMemOpSizeString(nameof(MemoryOperandSize.Byte))}",
				OpCodeSelectorKind.Memory16 => $"{argName}.size() == {GetMemOpSizeString(nameof(MemoryOperandSize.Word))}",
				OpCodeSelectorKind.Memory32 => $"{argName}.size() == {GetMemOpSizeString(nameof(MemoryOperandSize.Dword))}",
				OpCodeSelectorKind.Memory48 => $"{argName}.size() == {GetMemOpSizeString(nameof(MemoryOperandSize.Fword))}",
				OpCodeSelectorKind.Memory64 => $"{argName}.size() == {GetMemOpSizeString(nameof(MemoryOperandSize.Qword))}",
				OpCodeSelectorKind.Memory80 => $"{argName}.size() == {GetMemOpSizeString(nameof(MemoryOperandSize.Tbyte))}",
				OpCodeSelectorKind.MemoryMM => $"{argName}.size() == {GetMemOpSizeString(nameof(MemoryOperandSize.Qword))}",
				OpCodeSelectorKind.MemoryXMM => $"{argName}.size() == {GetMemOpSizeString(nameof(MemoryOperandSize.Xword))}",
				OpCodeSelectorKind.MemoryYMM => $"{argName}.size() == {GetMemOpSizeString(nameof(MemoryOperandSize.Yword))}",
				OpCodeSelectorKind.MemoryZMM => $"{argName}.size() == {GetMemOpSizeString(nameof(MemoryOperandSize.Zword))}",
				OpCodeSelectorKind.MemoryIndex32Xmm or OpCodeSelectorKind.MemoryIndex64Xmm => $"{argName}.index().is_xmm()",
				OpCodeSelectorKind.MemoryIndex32Ymm or OpCodeSelectorKind.MemoryIndex64Ymm => $"{argName}.index().is_ymm()",
				OpCodeSelectorKind.MemoryIndex32Zmm or OpCodeSelectorKind.MemoryIndex64Zmm => $"{argName}.index().is_zmm()",
				_ => throw new InvalidOperationException(),
			};
		}

		string GetRegisterString(string fieldName) =>
			idConverter.ToDeclTypeAndValue(registerType[fieldName]);

		string GetMemOpSizeString(string fieldName) =>
			idConverter.ToDeclTypeAndValue(memoryOperandSizeType[fieldName]);

		void GenerateTests(TraitGroup[] traitGroups) {
			foreach (var bitness in new[] { 16, 32, 64 }) {
				var bitnessFlag = bitness switch {
					16 => InstructionDefFlags1.Bit16,
					32 => InstructionDefFlags1.Bit32,
					64 => InstructionDefFlags1.Bit64,
					_ => throw new InvalidOperationException(),
				};

				var filename = genTypes.Dirs.GetRustFilename("code_asm", "tests", $"instr{bitness}.rs");
				using (var writer = new FileWriter(TargetLanguage.Rust, FileUtils.OpenWrite(filename))) {
					writer.WriteFileHeader();
					writer.WriteLine("#![allow(clippy::unreadable_literal)]");
					writer.WriteLine();
					var codeStr = genTypes[TypeIds.Code].Name(idConverter);
					var registerStr = registerType.Name(idConverter);
					var decoderOptsStr = genTypes[TypeIds.DecoderOptions].Name(idConverter);
					var repPrefixKindStr = genTypes[TypeIds.RepPrefixKind].Name(idConverter);
					writer.WriteLine($"use crate::code_asm::tests::{{add_op_mask, assign_label, create_and_emit_label, test_instr, test_invalid_instr, TestInstrFlags, {FirstLabelIdName}}};");
					writer.WriteLine("use crate::code_asm::*;");
					writer.WriteLine($"use crate::{{{codeStr}, {decoderOptsStr}, Instruction, MemoryOperand, {registerStr}, {repPrefixKindStr}}};");
					var sb = new StringBuilder();
					foreach (var traitGroup in traitGroups) {
						foreach (var group in traitGroup.Groups) {
							if ((group.AllDefFlags & bitnessFlag) == 0)
								continue;
							if (group.Name == "xbegin")
								continue; // Implemented manually

							var testFnName = GetTestMethodName(sb, group);
							if (ignoredTestsPerBitness.TryGetValue(bitness, out var ignoredTests) && ignoredTests.Contains(testFnName))
								continue;
							writer.WriteLine();
							writer.WriteLine("#[test]");
							writer.WriteLine(RustConstants.AttributeNoRustFmt);
							writer.WriteLine($"fn {testFnName}() {{");
							var args = new TestArgValues(traitGroup.ArgCount);
							using (writer.Indent()) {
								// If it has i32 args, test without the i32 suffix since that's what user code will look like.
								// Need to make sure that also compiles. But first, always test with i32 suffixes.
								if (!useInt32Suffix)
									throw new InvalidOperationException();
								GenerateTests(writer, bitness, group, OpCodeArgFlags.None, args, useInt32Suffix: true);
								if (group.Signature.HasKind(ArgKind.Immediate))
									GenerateTests(writer, bitness, group, OpCodeArgFlags.None, args, useInt32Suffix: false);
								useInt32Suffix = true;
							}
							writer.WriteLine("}");
						}
					}
				}
			}
		}

		void GenerateTests(FileWriter writer, int bitness, OpCodeInfoGroup group, OpCodeArgFlags contextFlags, TestArgValues args,
			bool useInt32Suffix) {
			this.useInt32Suffix = useInt32Suffix;
			if (group.ParentPseudoOpsKind is OpCodeInfoGroup parent)
				GenerateTestsForInstr(writer, bitness, group, parent.Defs[0], contextFlags, args);
			else
				GenerateTests(writer, bitness, group, group.RootOpCodeNode, contextFlags, args, false);
		}

		static string GetTestMethodName(StringBuilder sb, OpCodeInfoGroup group) {
			sb.Clear();
			sb.Append(group.Name.ToLowerInvariant());
			for (int i = 0; i < group.Signature.ArgCount; i++) {
				sb.Append('_');
				sb.Append(GetTestMethodArgName(group.Signature.GetArgKind(i)));
			}
			return sb.ToString();
		}

		void GenerateTests(FileWriter writer, int bitness, OpCodeInfoGroup group, OpCodeNode node, OpCodeArgFlags contextFlags, TestArgValues args, bool inElse) {
			if (node.Def is InstructionDef def) {
				if (inElse) {
					writer.WriteLine("*/ {");
					using (writer.Indent())
						GenerateTestsForInstr(writer, bitness, group, def, contextFlags, args);
					writer.WriteLine("}");
				}
				else
					GenerateTestsForInstr(writer, bitness, group, def, contextFlags, args);
			}
			else if (node.Selector is OpCodeSelector selector) {
				var (argKind, maxArgSize) = GetArgInfo(group, selector.ArgIndex);
				var condition = GetArgConditionForOpCodeKind(selector.ArgIndex, argKind, maxArgSize, selector.Kind);
				var isSelectorSupportedByBitness = IsSelectorSupportedByBitness(bitness, selector.Kind, out var continueElse);
				var (contextIfFlags, contextElseFlags) = GetIfElseContextFlags(selector.Kind);
				if (!inElse)
					writer.Write("/* ");
				writer.WriteLine($"if {condition} */ {{");
				if (isSelectorSupportedByBitness) {
					using (writer.Indent()) {
						foreach (var argValue in GetArgValue(selector.Kind, false, selector.ArgIndex, group.Signature, maxArgSize * 8)) {
							var oldValue = args.Set(selector.ArgIndex, argValue);
							GenerateTests(writer, bitness, group, selector.IfTrue, contextFlags | contextIfFlags, args, false);
							args.Restore(selector.ArgIndex, oldValue);
						}
					}
				}
				else {
					using (writer.Indent())
						writer.WriteLine($"// skip `if {condition}` since it's not supported by the current test bitness");
				}
				if (selector.IfFalse.IsEmpty) {
					writer.WriteLine("} /* else */ {");
					if (isSelectorSupportedByBitness && selector.ArgIndex >= 0) {
						var newArg = GetInvalidArgValue(selector.Kind, selector.ArgIndex);
						if (newArg is not null) {
							using (writer.Indent()) {
								int testBitness = GetInvalidTestBitness(bitness, group);
								var oldValue = args.Set(selector.ArgIndex, newArg);
								GenerateTests(writer, testBitness, group, selector.IfTrue,
									contextFlags | contextIfFlags | OpCodeArgFlags.GenerateInvalidTest, args, false);
								args.Restore(selector.ArgIndex, oldValue);
							}
						}
					}
					writer.WriteLine("}");
				}
				else {
					if (continueElse) {
						writer.Write("} /* else ");
						foreach (var argValue in GetArgValue(selector.Kind, true, selector.ArgIndex, group.Signature, maxArgSize * 8)) {
							var oldValue = args.Set(selector.ArgIndex, argValue);
							GenerateTests(writer, bitness, group, selector.IfFalse, contextFlags | contextElseFlags, args, true);
							args.Restore(selector.ArgIndex, oldValue);
						}
					}
					else {
						writer.WriteLine($"}} /* else */ {{");
						using (writer.Indent())
							writer.WriteLine($"// skip `if !({condition})` since it's not supported by the current test bitness");
						writer.WriteLine("}");
					}
				}
			}
			else
				throw new InvalidOperationException();
		}

		void GenerateTestsForInstr(FileWriter writer, int bitness, OpCodeInfoGroup group, InstructionDef def, OpCodeArgFlags contextFlags,
			TestArgValues args) {
			if (!IsBitnessSupported(bitness, def.Flags1)) {
				writer.WriteLine($"// Skipping {def.Code.Name(idConverter)} - Not supported by current bitness");
				return;
			}
			writer.WriteLine($"// {def.Code.RawName}");

			var withFns = new List<(string pre, string post)>();
			var asmArgs = new List<string>();
			var withArgs = new List<string>();
			int argBitness = GetArgBitness(bitness, def);
			if ((group.Flags & OpCodeArgFlags.HasSpecialInstructionEncoding) != 0)
				withArgs.Add(bitness.ToString(CultureInfo.InvariantCulture));
			else
				withArgs.Add(idConverter.ToDeclTypeAndValue(def.Code));
			bool hasLabel = false;
			for (var i = 0; i < args.Args.Count; i++) {
				var argKind = group.Signature.GetArgKind(i);
				if (argKind == ArgKind.Label)
					hasLabel = true;
				var asmArg = args.GetArgValue(argBitness, i)?.AsmStr;
				var withArg = args.GetArgValue(argBitness, i)?.WithStr;

				if (asmArg is null || withArg is null) {
					var argValue = GetDefaultArgument(def.OpKindDefs[group.NumberOfLeadingArgsToDiscard + i], i, argKind, group.MaxArgSizes[i] * 8);
					asmArg = argValue.Get(argBitness).AsmStr;
					withArg = argValue.Get(argBitness).WithStr;
				}

				if ((def.Flags1 & InstructionDefFlags1.OpMaskRegister) != 0 && i == 0) {
					asmArg += ".k1()";
					var opMask = idConverter.ToDeclTypeAndValue(GetRegisterDef(Register.K1).Register);
					withFns.Add(("add_op_mask(", $", {opMask})"));
				}

				asmArgs.Add(asmArg);
				withArgs.Add(withArg);
			}
			int extraArgsCount = 0;
			if (group.ParentPseudoOpsKind is not null) {
				extraArgsCount++;
				withArgs.Add(SignedImmToTestArgValue(group.PseudoOpsKindImmediateValue, 8, 8, 8).WithStr);
			}
			if ((group.Flags & OpCodeArgFlags.HasSpecialInstructionEncoding) != 0) {
				if (SpecialInstructionHasSegmentArg(group.MnemonicName))
					withArgs.Add(idConverter.ToDeclTypeAndValue(GetRegisterDef(Register.None).Register));
				if ((group.Defs[0].Flags3 & InstructionDefFlags3.IsStringOp) != 0)
					withArgs.Add(idConverter.ToDeclTypeAndValue(genTypes[TypeIds.RepPrefixKind][nameof(RepPrefixKind.None)]));
			}
			if (group.HasLabel && (group.Flags & OpCodeArgFlags.HasLabelUlong) == 0)
				withFns.Add(("assign_label(", $", {withArgs[1]})"));

			var asmName = GetPubFnName(group);
			var asmArgsStr = string.Join(", ", asmArgs);
			var asmBody = $"a.{asmName}({asmArgsStr})";
			var instrFlags = GetInstrTestFlags(def, group, contextFlags);
			if (instrFlags.Count == 0)
				instrFlags.Add(testInstrFlags[nameof(TestInstrFlags.None)]);
			var testInstrFlagsStr = string.Join(" | ",
				instrFlags.Select(x => $"{x.DeclaringType.Name(idConverter)}::{idConverter.Constant(x.RawName)}"));
			if ((contextFlags & OpCodeArgFlags.GenerateInvalidTest) != 0)
				writer.WriteLine($"test_invalid_instr({bitness}, |a| assert!({asmBody}.is_err()), {testInstrFlagsStr});");
			else {
				asmBody = $"{asmBody}.unwrap()";
				if (hasLabel)
					asmBody = $"{{ let {CreatedLabelName} = create_and_emit_label(a); {asmBody} }}";
				writer.WriteLine($"test_instr({bitness}, |a| {asmBody},");
				using (writer.Indent()) {
					string withFailStr = ".unwrap()";
					string withFnName;
					if ((group.Flags & OpCodeArgFlags.HasSpecialInstructionEncoding) != 0)
						withFnName = $"with_{group.MnemonicName.ToLowerInvariant()}";
					else if (group.HasLabel)
						withFnName = RustInstrCreateGenNames.with_branch;
					else {
						if (group.Signature.ArgCount == 0)
							withFailStr = string.Empty;
						withFnName = InstrCreateGenImpl.GetRustOverloadedCreateName(group.Signature.ArgCount + extraArgsCount);
					}
					var withArgsStr = string.Join(", ", withArgs);
					var withFnsPreStr = string.Join(string.Empty, ((IEnumerable<(string pre, string post)>)withFns).Reverse().Select(x => x.pre));
					var withFnsPostStr = string.Join(string.Empty, withFns.Select(x => x.post));
					writer.WriteLine($"{withFnsPreStr}Instruction::{withFnName}({withArgsStr}){withFailStr}{withFnsPostStr},");

					var decOpts = GetDecoderOptions(bitness, def);
					if (decOpts.Count == 0)
						decOpts.Add(decoderOptions[nameof(DecoderOptions.None)]);
					var decOptsStr = string.Join(" | ",
						decOpts.Select(x => $"{x.DeclaringType.Name(idConverter)}::{idConverter.Constant(x.RawName)}"));
					writer.WriteLine($"{testInstrFlagsStr}, {decOptsStr});");
				}
			}
		}

		protected override TestArgValueBitness MemToTestArgValue(MemorySizeFuncInfo size, int bitness, ulong address) {
			var memName = GetName(size);
			var asmStr = $"{memName}(0x{address:X}u64)";
			var withStr = $"MemoryOperand::with_displ(0x{address:X}u64, {bitness / 8})";
			return new TestArgValueBitness(asmStr, withStr);
		}

		protected override TestArgValueBitness MemToTestArgValue(MemorySizeFuncInfo size, Register @base, Register index, int scale, int displ) {
			if (scale != 1 && scale != 2 && scale != 4 && scale != 8)
				throw new InvalidOperationException();
			var sb = new StringBuilder();
			sb.Append(GetName(size));
			sb.Append('(');
			bool plus = false;
			if (@base != Register.None) {
				plus = true;
				sb.Append(GetRegisterDef(@base).GetAsmRegisterName());
			}
			if (index != Register.None) {
				if (plus)
					sb.Append('+');
				plus = true;
				sb.Append(GetRegisterDef(index).GetAsmRegisterName());
				if (scale > 1) {
					sb.Append('*');
					sb.Append(scale);
				}
			}
			if (displ != 0) {
				bool isNeg = displ < 0;
				if (isNeg)
					displ = -displ;
				if (plus)
					sb.Append(isNeg ? '-' : '+');
				sb.Append("0x");
				sb.Append(displ.ToString("X", CultureInfo.InvariantCulture));
			}
			sb.Append(')');
			var asmStr = sb.ToString();

			var baseStr = idConverter.ToDeclTypeAndValue(GetRegisterDef(@base).Register);
			var indexStr = idConverter.ToDeclTypeAndValue(GetRegisterDef(index).Register);
			var displStr = displ < 0 ?
				"-0x" + (-displ).ToString("X", CultureInfo.InvariantCulture) :
				"0x" + displ.ToString("X", CultureInfo.InvariantCulture);
			displStr += "i64";
			var displSize = displ == 0 ? "0" : "1";
			var isBcstStr = size.IsBroadcast ? "true" : "false";
			var regNoneStr = idConverter.ToDeclTypeAndValue(GetRegisterDef(Register.None).Register);
			var withStr = $"MemoryOperand::new({baseStr}, {indexStr}, {scale}, {displStr}, {displSize}, {isBcstStr}, {regNoneStr})";

			return new(asmStr, withStr);
		}

		protected override TestArgValueBitness RegToTestArgValue(Register register) {
			var regDef = GetRegisterDef(register);
			var asmReg = regDef.GetAsmRegisterName();
			var withReg = idConverter.ToDeclTypeAndValue(regDef.Register);
			return new(asmReg, withReg);
		}

		protected override TestArgValueBitness UnsignedImmToTestArgValue(ulong immediate, int encImmSizeBits, int immSizeBits, int argSizeBits) {
			if (encImmSizeBits > immSizeBits)
				throw new InvalidOperationException();
			var (castType, mask) = immSizeBits switch {
				4 or 8 => ("u32", byte.MaxValue),
				16 => ("u32", ushort.MaxValue),
				32 => ("u32", uint.MaxValue),
				64 => ("u64", ulong.MaxValue),
				_ => throw new InvalidOperationException(),
			};
			immediate &= mask;
			string numStr;
			if (immediate <= 9)
				numStr = immediate.ToString(CultureInfo.InvariantCulture);
			else
				numStr = "0x" + immediate.ToString("X", CultureInfo.InvariantCulture);

			var asmStr = numStr + castType;
			var withStr = asmStr;

			return new(asmStr, withStr);
		}

		protected override TestArgValueBitness SignedImmToTestArgValue(long immediate, int encImmSizeBits, int immSizeBits, int argSizeBits) {
			if (encImmSizeBits > immSizeBits)
				throw new InvalidOperationException();
			var (castType, castTypeNoSuffix) = argSizeBits switch {
				4 or 8 or 16 or 32 => ("i32", string.Empty),
				64 => ("i64", "i64"),
				_ => throw new InvalidOperationException(),
			};
			bool isNeg = immediate < 0;
			if (isNeg)
				immediate = -immediate;
			string numStr;
			if ((ulong)immediate <= 9)
				numStr = immediate.ToString(CultureInfo.InvariantCulture);
			else
				numStr = "0x" + immediate.ToString("X", CultureInfo.InvariantCulture);
			if (isNeg)
				numStr = "-" + numStr;

			var asmStr = useInt32Suffix ? numStr + castType : numStr + castTypeNoSuffix;
			var withStr = numStr + castType;

			return new(asmStr, withStr);
		}

		protected override TestArgValueBitness LabelToTestArgValue() => new(CreatedLabelName, FirstLabelIdName);
	}
}
