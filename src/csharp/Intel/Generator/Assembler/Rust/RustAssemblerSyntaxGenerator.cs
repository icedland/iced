// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Linq;
using Generator.IO;
using Generator.Tables;

namespace Generator.Assembler.Rust {
	[Generator(TargetLanguage.Rust)]
	sealed class RustAssemblerSyntaxGenerator : AssemblerSyntaxGenerator {
		readonly IdentifierConverter idConverter;

		public RustAssemblerSyntaxGenerator(GeneratorContext generatorContext)
			: base(generatorContext.Types) {
			idConverter = RustIdentifierConverter.Create();
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

				var registerTypeName = genTypes[TypeIds.Register].Name(idConverter);
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
							writer.WriteLine($"pub const {asmRegName}: {structName} = {structName}::new({registerTypeName}::{regDef.Register.Name(idConverter)});");
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
			var structName = "__AsmRegister" + suffix;

			return (structName, modName, doc);
		}

		protected override void GenerateRegisterClasses(RegisterClassInfo[] infos) {
			var filename = genTypes.Dirs.GetRustFilename("code_asm", "reg.rs");
			using (var writer = new FileWriter(TargetLanguage.Rust, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();

				var registerTypeName = genTypes[TypeIds.Register].Name(idConverter);

				writer.WriteLine("use crate::code_asm::op_state::CodeAsmOpState;");
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
						writer.WriteLine("#[repr(transparent)]");
					writer.WriteLine($"pub struct {structName} {{");
					using (writer.Indent()) {
						writer.WriteLine($"register: {registerTypeName},");
						if (reg.NeedsState)
							writer.WriteLine("state: CodeAsmOpState,");
					}
					writer.WriteLine("}");
					writer.WriteLine();
					writer.WriteLine(RustConstants.AttributeNoRustFmt);
					writer.WriteLine($"impl {structName} {{");
					using (writer.Indent()) {
						writer.WriteLine($"pub(crate) const fn new(register: {registerTypeName}) -> Self {{");
						using (writer.Indent()) {
							if (reg.NeedsState)
								writer.WriteLine("Self { register, state: CodeAsmOpState::new() }");
							else
								writer.WriteLine("Self { register }");
						}
						writer.WriteLine("}");
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
			//TODO:
		}

		protected override void Generate(Dictionary<GroupKey, OpCodeInfoGroup> map, OpCodeInfoGroup[] groups) {
			//TODO:
		}
	}
}
