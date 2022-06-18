// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Linq;
using Generator.IO;

namespace Generator.Tables.Java {
	[Generator(TargetLanguage.Java)]
	sealed class ICRegisterGen {
		const string AllRegistersClassName = "ICRegisters";
		readonly IdentifierConverter idConverter;
		readonly GenTypes genTypes;

		public ICRegisterGen(GeneratorContext generatorContext) {
			idConverter = JavaIdentifierConverter.Create();
			genTypes = generatorContext.Types;
		}

		public void Generate() {
			var registerType = genTypes[TypeIds.Register];
			var regGroups = genTypes.GetObject<RegisterDefs>(TypeIds.RegisterDefs).GetRegisterGroups(kind => kind != RegisterKind.None);

			var infos = new (string name, RegisterKind[] kinds)[] {
				(AllRegistersClassName, Array.Empty<RegisterKind>()),
				(AllRegistersClassName + "GPR", new[] { RegisterKind.GPR8, RegisterKind.GPR16, RegisterKind.GPR32, RegisterKind.GPR64 }),
				(AllRegistersClassName + "Vec", new[] { RegisterKind.XMM, RegisterKind.YMM, RegisterKind.ZMM }),

				(AllRegistersClassName + "GPR8", new[] { RegisterKind.GPR8 }),
				(AllRegistersClassName + "GPR16", new[] { RegisterKind.GPR16 }),
				(AllRegistersClassName + "GPR32", new[] { RegisterKind.GPR32 }),
				(AllRegistersClassName + "GPR64", new[] { RegisterKind.GPR64 }),
				(AllRegistersClassName + "IP", new[] { RegisterKind.IP }),
				(AllRegistersClassName + "Segment", new[] { RegisterKind.Segment }),
				(AllRegistersClassName + "ST", new[] { RegisterKind.ST }),
				(AllRegistersClassName + "CR", new[] { RegisterKind.CR }),
				(AllRegistersClassName + "DR", new[] { RegisterKind.DR }),
				(AllRegistersClassName + "TR", new[] { RegisterKind.TR }),
				(AllRegistersClassName + "BND", new[] { RegisterKind.BND }),
				(AllRegistersClassName + "K", new[] { RegisterKind.K }),
				(AllRegistersClassName + "MM", new[] { RegisterKind.MM }),
				(AllRegistersClassName + "XMM", new[] { RegisterKind.XMM }),
				(AllRegistersClassName + "YMM", new[] { RegisterKind.YMM }),
				(AllRegistersClassName + "ZMM", new[] { RegisterKind.ZMM }),
				(AllRegistersClassName + "TMM", new[] { RegisterKind.TMM }),
			};

			var allGroups = regGroups.ToDictionary(a => a.kind, a => true);
			allGroups.Remove(RegisterKind.None);
			foreach (var (_, kinds) in infos) {
				if (kinds.Length == 1)
					allGroups.Remove(kinds[0]);
			}
			if (allGroups.Count != 0)
				throw new InvalidOperationException($"New register kind: {allGroups.ToArray()[0].Key}");

			var toDefs = regGroups.ToDictionary(a => a.kind, a => a.regs);
			var allNames = infos.Select(a => a.name).ToArray();
			foreach (var (name, tmp) in infos) {
				var kinds = tmp;
				bool isAllRegs = kinds.Length == 0;
				if (isAllRegs)
					kinds = regGroups.Select(a => a.kind).ToArray();
				GenerateRegisters(toDefs, allNames, name, kinds, isAllRegs);
			}
		}

		void GenerateRegisters(Dictionary<RegisterKind, RegisterDef[]> toDefs, string[] allNames, string name,
				RegisterKind[] kinds, bool isAllRegs) {
			var filename = JavaConstants.GetFilename(genTypes, JavaConstants.IcedPackage, name + ".java");
			using (var writer = new FileWriter(TargetLanguage.Java, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();
				writer.WriteLine($"package {JavaConstants.IcedPackage};");
				writer.WriteLine();
				writer.WriteLine("/**");
				writer.WriteLine(" * Registers passed to <code>Instruction.create()</code> methods.");
				writer.WriteLine(" *");
				foreach (var otherName in allNames) {
					if (otherName != name)
						writer.WriteLine($" * @see {otherName}");
				}
				writer.WriteLine(" */");
				writer.WriteLine($"public final class {name} {{");
				using (writer.Indent()) {
					writer.WriteLine($"private {name}() {{");
					writer.WriteLine("}");
					writer.WriteLine();
					foreach (var kind in kinds) {
						var defs = toDefs[kind];
						foreach (var def in defs) {
							string regName;
							if (kind == RegisterKind.ST)
								regName = def.Register.RawName.ToLowerInvariant();
							else
								regName = def.Name.ToLowerInvariant();
							if (isAllRegs)
								writer.WriteLine($"public static final ICRegister {regName} = new ICRegister({idConverter.ToDeclTypeAndValue(def.Register)});");
							else
								writer.WriteLine($"public static final ICRegister {regName} = {AllRegistersClassName}.{regName};");
						}
					}
				}
				writer.WriteLine("}");
			}
		}
    }
}
