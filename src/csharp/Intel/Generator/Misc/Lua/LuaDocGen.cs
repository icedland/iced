// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Generator.IO;

namespace Generator.Misc.Lua {
	[Generator(TargetLanguage.Lua, double.MaxValue)]
	sealed class LuaDocGen {
		readonly GenTypes genTypes;
		readonly IdentifierConverter idConverter;

		public LuaDocGen(GeneratorContext generatorContext) {
			genTypes = generatorContext.Types;
			idConverter = LuaIdentifierConverter.Create();
		}

		public void Generate() {
			var classes = new List<LuaClass>();
			foreach (var filename in Directory.GetFiles(genTypes.Dirs.GetLuaRustDir(), "*.rs", SearchOption.AllDirectories)) {
				var parser = new LuaClassParser(filename);
				classes.AddRange(parser.ParseFile());
			}
			if (classes.Count == 0)
				throw new InvalidOperationException();

			WriteEmmyLuaDocs(classes);
		}

		void WriteEmmyLuaDocs(List<LuaClass> classes) {
			var classNameToClass = classes.ToDictionary(c => c.Name, StringComparer.Ordinal);
			foreach (var cls in classes.OrderBy(c => c.Name, StringComparer.Ordinal)) {
				var filename = genTypes.Dirs.GetLuaTypesFilename(cls.ModulePath.Names) + ".lua";
				var sb = new StringBuilder();
				using (var writer = new FileWriter(TargetLanguage.Lua, FileUtils.OpenWrite(filename))) {
					writer.WriteFileHeader();

					var clsName = idConverter.Type(cls.Name);

					// sumneko_lua won't show references to these dummy fns we're creating if this is in the file.
					// User refs to these fns will be shown however.
					writer.WriteLine("---@meta");
					writer.WriteLine("---@diagnostic disable unused-local");
					writer.WriteLine();

					WriteEmmyLuaDocs(writer, sb, cls.DocComments);
					writer.WriteLine($"local {clsName} = {{}}");
					foreach (var method in cls.Methods) {
						writer.WriteLine();
						WriteEmmyLuaDocs(writer, sb, method.DocComments);
						sb.Clear();
						sb.Append("function ");
						sb.Append(clsName);
						var methodSep = method.Kind switch {
							LuaMethodKind.Method => ":",
							LuaMethodKind.Function or LuaMethodKind.Constructor => ".",
							_ => throw new InvalidOperationException(),
						};
						sb.Append(methodSep);
						sb.Append(method.Name);
						sb.Append('(');
						for (int i = 0; i < method.Args.Length; i++) {
							if (i > 0)
								sb.Append(", ");
							sb.Append(method.Args[i].Name);
						}
						sb.Append(") end");
						writer.WriteLine(sb.ToString());
					}
					writer.WriteLine();
					writer.WriteLine($"return {clsName}");
				}
			}
		}

		void WriteEmmyLuaDocs(FileWriter writer, StringBuilder sb, DocComments docComments) {
			bool needEmptyLine = false;
			foreach (var doc in docComments.Sections) {
				if (needEmptyLine)
					writer.WriteLine("---");
				needEmptyLine = true;
				switch (doc) {
				case TextDocCommentSection text:
					foreach (var line in text.Lines)
						writer.WriteLine($"---{line}");
					break;

				case LuaAnnotationDocCommentSection args:
					foreach (var arg in args.Params) {
						sb.Clear();
						sb.Append($"---@param {arg.Name}");
						if (arg.IsOptional)
							sb.Append('?');
						sb.Append(' ');
						sb.Append(string.Join('|', arg.Type.Types));
						sb.Append($" #{arg.Comment}");
						writer.WriteLine(sb.ToString());
					}
					if (args.Return is LuaReturnAnnot luaReturn) {
						sb.Clear();
						sb.Append("---@return ");
						for (int i = 0; i < luaReturn.Types.Length; i++) {
							var types = luaReturn.Types[i];
							if (i > 0)
								sb.Append(", ");
							sb.Append(string.Join('|', types.Types));
						}
						if (luaReturn.Comment is string comment)
							sb.Append($" #{comment}");
						writer.WriteLine(sb.ToString());
					}
					if (args.Class is LuaClassAnnot luaClass) {
						sb.Clear();
						sb.Append("---@class ");
						sb.Append(string.Join('|', luaClass.Type.Types));
						writer.WriteLine(sb.ToString());
					}
					foreach (var overload in args.Overloads)
						writer.WriteLine($"---@overload {overload.Function}");
					break;

				case TestCodeDocCommentSection code:
					writer.WriteLine("---```lua");
					foreach (var line in code.Lines)
						writer.WriteLine($"---{line}");
					writer.WriteLine("---```");
					break;

				default:
					throw new InvalidOperationException($"Unknown doc comment section: {doc.GetType()}");
				}
			}
		}
	}
}
