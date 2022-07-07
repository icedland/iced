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
			var regGroups = genTypes.GetObject<RegisterDefs>(TypeIds.RegisterDefs).GetRegisterGroups(kind => kind != RegisterKind.None);
			var infos = Tables.Java.RegisterUtils.GetRegisterInfos(AllRegistersClassName, regGroups);
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
				writer.WriteLine(" * Registers passed to {@code Instruction.create()} methods.");
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
							string regName = def.GetAsmRegisterName();
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
