// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Linq;
using Generator.Encoder.Rust;
using Generator.Enums;
using Generator.Enums.Encoder;
using Generator.IO;
using Generator.Tables;

namespace Generator.Assembler.Rust {
	[Generator(TargetLanguage.Rust)]
	sealed class RustAssemblerSyntaxGenerator : AssemblerSyntaxGenerator {
		const string AsmRegisterPrefix = "__AsmRegister";
		const string AsmMemoryOperand = "__AsmMemoryOperand";
		const string CodeLabel = "CodeLabel";
		const string CodeAssembler = "CodeAssembler";
		const string CodeAsmOpState = "CodeAsmOpState";
		const string ErrorType = "IcedError";
		readonly IdentifierConverter idConverter;
		readonly EnumType registerType;
		readonly EnumType memoryOperandSizeType;

		public RustAssemblerSyntaxGenerator(GeneratorContext generatorContext)
			: base(generatorContext.Types) {
			idConverter = RustIdentifierConverter.Create();
			registerType = genTypes[TypeIds.Register];
			memoryOperandSizeType = genTypes[TypeIds.CodeAsmMemoryOperandSize];
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

					var (structName, modName, doc) = GetAsmRegisterInfo(kind);
					writer.WriteLine();
					writer.WriteLine(RustConstants.AttributeNoRustFmt);
					writer.WriteLine($"pub mod {modName} {{");
					using (writer.Indent()) {
						writer.WriteLine($"//! {doc}");
						writer.WriteLine("#![allow(non_upper_case_globals)]");
						writer.WriteLine("#![allow(missing_docs)]");
						writer.WriteLine($"use crate::code_asm::reg::{structName};");
						writer.WriteLine($"use crate::{registerTypeName};");
						foreach (var regDef in regs) {
							var asmRegName = GetAsmRegisterName(regDef);
							writer.WriteLine($"pub const {asmRegName}: {structName} = {structName}::new({idConverter.ToDeclTypeAndValue(regDef.Register)});");
						}
					}
					writer.WriteLine("}");
				}

				writer.WriteLine();
				foreach (var (kind, regs) in regGroups.OrderBy(a => GetAsmRegisterInfo(a.kind).modName, StringComparer.Ordinal)) {
					var modName = GetAsmRegisterInfo(kind).modName;
					writer.WriteLine($"pub use self::{modName}::*;");
				}
			}
		}

		static (string structName, string modName, string doc) GetAsmRegisterInfo(RegisterKind kind) {
			var (suffix, modName, doc) = kind switch {
				RegisterKind.None => throw new InvalidOperationException(),
				RegisterKind.GPR8 => ("8", "gpr8", "All 8-bit general purpose registers."),
				RegisterKind.GPR16 => ("16", "gpr16", "All 16-bit general purpose registers."),
				RegisterKind.GPR32 => ("32", "gpr32", "All 32-bit general purpose registers."),
				RegisterKind.GPR64 => ("64", "gpr64", "All 64-bit general purpose registers."),
				RegisterKind.IP => ("Ip", "ip", "All instruction pointer registers."),
				RegisterKind.Segment => ("Segment", "segment", "All segment registers."),
				RegisterKind.ST => ("St", "st", "All FPU registers."),
				RegisterKind.CR => ("Cr", "cr", "All control registers."),
				RegisterKind.DR => ("Dr", "dr", "All debug registers."),
				RegisterKind.TR => ("Tr", "tr", "All test registers."),
				RegisterKind.BND => ("Bnd", "bnd", "All bound registers."),
				RegisterKind.K => ("K", "k", "All opmask registers."),
				RegisterKind.MM => ("Mm", "mm", "All MMX registers."),
				RegisterKind.XMM => ("Xmm", "xmm", "All 128-bit vector registers (XMM)."),
				RegisterKind.YMM => ("Ymm", "ymm", "All 256-bit vector registers (YMM)."),
				RegisterKind.ZMM => ("Zmm", "zmm", "All 512-bit vector registers (ZMM)."),
				RegisterKind.TMM => ("Tmm", "tmm", "All tile registers."),
				_ => throw new InvalidOperationException(),
			};
			var structName = AsmRegisterPrefix + suffix;

			return (structName, modName, doc);
		}

		protected override void GenerateRegisterClasses(RegisterClassInfo[] infos) {
			var filename = genTypes.Dirs.GetRustFilename("code_asm", "reg.rs");
			using (var writer = new FileWriter(TargetLanguage.Rust, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();

				var registerTypeName = registerType.Name(idConverter);

				writer.WriteLine($"use crate::code_asm::op_state::{CodeAsmOpState};");
				writer.WriteLine($"use crate::{registerTypeName};");
				foreach (var reg in infos) {
					var (structName, modName, doc) = GetAsmRegisterInfo(reg.Kind);

					writer.WriteLine();
					writer.WriteLine($"/// {doc}");
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
					writer.WriteLine($"/// use iced_x86::code_asm::registers::{modName}::*;");
					writer.WriteLine("/// ```");
					writer.WriteLine("#[derive(Debug, Copy, Clone, Eq, PartialEq)]");
					writer.WriteLine(RustConstants.AttributeNoRustFmt);
					if (!reg.NeedsState)
						writer.WriteLine(RustConstants.AttrTransparent);
					writer.WriteLine($"pub struct {structName} {{");
					using (writer.Indent()) {
						writer.WriteLine($"register: {registerTypeName},");
						if (reg.NeedsState)
							writer.WriteLine($"state: {CodeAsmOpState},");
					}
					writer.WriteLine("}");
					writer.WriteLine();
					writer.WriteLine(RustConstants.AttributeNoRustFmt);
					writer.WriteLine($"impl {structName} {{");
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
					writer.WriteLine($"impl From<{structName}> for {registerTypeName} {{");
					using (writer.Indent()) {
						writer.WriteLine("#[inline]");
						writer.WriteLine($"fn from(reg: {structName}) -> Self {{");
						using (writer.Indent())
							writer.WriteLine("reg.register");
						writer.WriteLine("}");
					}
					writer.WriteLine("}");
				}
			}
		}

		protected override void GenerateMemorySizeFunctions(MemorySizeFuncInfo[] infos) {
			string GetFuncName(MemorySizeFuncInfo info) => info.Name.Replace(' ', '_');
			var filename = genTypes.Dirs.GetRustFilename("code_asm", "mem.rs");

			new FileUpdater(TargetLanguage.Rust, "AsmMemoryOperandPtrMethods", filename).Generate(writer => {
				for (int i = 0; i < infos.Length; i++) {
					var info = infos[i];
					var fnName = GetFuncName(info);
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
					var fnName = GetFuncName(info);
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
				name += group.Signature.ArgCount.ToString();
			return EscapeKeyword(name);
		}

		static string GetTraitFnName(TraitGroup traitGroup) => GetTraitFnName(traitGroup.Groups[0]);
		static string GetTraitFnName(OpCodeInfoGroup group) {
			if (group.AddNameSuffix)
				return EscapeKeyword(group.Name + "_" + group.Signature.ArgCount.ToString());
			return EscapeKeyword(group.Name);
		}

		static string GetPubFnName(TraitGroup traitGroup) => GetTraitFnName(traitGroup);
		static string GetFnArgName(int argIndex) => "op" + argIndex.ToString();
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
			var filename = genTypes.Dirs.GetRustFilename("code_asm", "fn_asm_traits.rs");
			using (var writer = new FileWriter(TargetLanguage.Rust, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();
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
				writer.WriteLine("use crate::code_asm::fn_asm_traits::*;");
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
								var desc = i != 0 ? string.Empty : " (eg. an integer (`i32`/`u32`/`i64`/`u64`), a register (`rdx`), memory (`dword_ptr(rcx+r13*4)`) or a label)";
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
				ArgKind.LabelUlong => "u64",
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

		void GenerateAsmImpl(TraitGroup[] traitGroups) {
			var filename = genTypes.Dirs.GetRustFilename("code_asm", "fn_asm_impl.rs");
			using (var writer = new FileWriter(TargetLanguage.Rust, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();
				writer.WriteLine("#![allow(clippy::if_same_then_else)]");
				writer.WriteLine();
				writer.WriteLine("use crate::code_asm::fn_asm_traits::*;");
				writer.WriteLine("use crate::code_asm::mem::*;");
				writer.WriteLine($"use crate::code_asm::op_state::{memoryOperandSizeType.Name(idConverter)};");
				writer.WriteLine("use crate::code_asm::reg::*;");
				writer.WriteLine($"use crate::code_asm::{{{CodeAssembler}, {CodeLabel}}};");
				var codeStr = genTypes[TypeIds.Code].Name(idConverter);
				var registerStr = registerType.Name(idConverter);
				var repPrefixKindStr = genTypes[TypeIds.RepPrefixKind].Name(idConverter);
				writer.WriteLine($"use crate::{{{codeStr}, {ErrorType}, Instruction, {registerStr}, {repPrefixKindStr}}};");
				writer.WriteLine("use core::i8;");

				void WriteArg(FileWriter writer, string argExpr, ArgKind kind) {
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
									bool noSeg =
										group.MnemonicName.StartsWith("Ins", StringComparison.Ordinal) ||
										group.MnemonicName.StartsWith("Scas", StringComparison.Ordinal) ||
										group.MnemonicName.StartsWith("Stos", StringComparison.Ordinal) ||
										group.MnemonicName.StartsWith("Xbegin", StringComparison.Ordinal);
									if (!noSeg) {
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
				OpCodeSelectorKind.ImmediateByteSigned8 or OpCodeSelectorKind.ImmediateByteSigned8To32 => argKind == ArgKind.ImmediateUnsigned ?
					$"{argName} <= i8::MAX as {argType} || 0xFFFF_FF80 <= {argName}" :
					$"{argName} >= i8::MIN as {argType} && {argName} <= i8::MAX as {argType}",
				OpCodeSelectorKind.ImmediateByteSigned8To16 => argKind == ArgKind.ImmediateUnsigned ?
					$"{argName} <= i8::MAX as {argType} || (0xFF80 <= {argName} && {argName} <= 0xFFFF)" :
					$"{argName} >= i8::MIN as {argType} && {argName} <= i8::MAX as {argType}",
				OpCodeSelectorKind.Vex => "self.prefer_vex()",
				OpCodeSelectorKind.EvexBroadcastX => $"{argName}.is_broadcast()",
				OpCodeSelectorKind.EvexBroadcastY => $"{argName}.is_broadcast()",
				OpCodeSelectorKind.EvexBroadcastZ => $"{argName}.is_broadcast()",
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
				OpCodeSelectorKind.MemoryIndex32Xmm => $"{argName}.index().is_xmm()",
				OpCodeSelectorKind.MemoryIndex64Xmm => $"{argName}.index().is_xmm()",
				OpCodeSelectorKind.MemoryIndex32Ymm => $"{argName}.index().is_ymm()",
				OpCodeSelectorKind.MemoryIndex64Ymm => $"{argName}.index().is_ymm()",
				OpCodeSelectorKind.MemoryIndex32Zmm => $"{argName}.index().is_zmm()",
				OpCodeSelectorKind.MemoryIndex64Zmm => $"{argName}.index().is_zmm()",
				_ => throw new InvalidOperationException(),
			};
		}

		string GetRegisterString(string fieldName) =>
			idConverter.ToDeclTypeAndValue(registerType[fieldName]);

		string GetMemOpSizeString(string fieldName) =>
			idConverter.ToDeclTypeAndValue(memoryOperandSizeType[fieldName]);

		void GenerateTests(TraitGroup[] traitGroups) {
			//TODO:
		}
	}
}
