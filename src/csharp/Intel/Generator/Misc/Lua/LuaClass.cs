// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System.Collections.Generic;
using System.Linq;

namespace Generator.Misc.Lua {
	sealed class DocComments {
		public readonly List<DocCommentSection> Sections = new();

		public void AddText(TextDocCommentSection text) {
			if (text.Lines.Length == 0)
				return;
			if (text.Lines.Length == 1 && text.Lines[0] == string.Empty)
				return;
			Sections.Add(text);
		}
	}

	abstract class DocCommentSection {
	}

	sealed class TextDocCommentSection : DocCommentSection {
		public readonly string[] Lines;

		public TextDocCommentSection(string[] lines) {
			int i;
			for (i = lines.Length - 1; i >= 0; i--) {
				if (!string.IsNullOrEmpty(lines[i]))
					break;
			}
			if (i + 1 != lines.Length)
				lines = lines.Take(i).ToArray();
			Lines = lines;
		}
	}

	sealed class LuaType {
		public readonly string[] Types;
		public LuaType(params string[] types) => Types = types;
    }

	sealed class LuaParamAnnot {
		public readonly string Name;
		public readonly LuaType Type;
		public readonly string Comment;
		public readonly bool IsOptional;
		public LuaParamAnnot(string name, LuaType type, string comment, bool isOptional) {
			Name = name;
			Type = type;
			Comment = comment;
			IsOptional = isOptional;
		}
    }

	sealed class LuaReturnAnnot {
		public readonly LuaType[] Types;
		public readonly string? Comment;
		public LuaReturnAnnot(string? comment, params LuaType[] types) {
			Types = types;
			Comment = comment;
		}
    }

	sealed class LuaClassAnnot {
		public readonly LuaType Type;
		public LuaClassAnnot(LuaType type) => Type = type;
    }

	sealed class LuaOverloadAnnot {
		public readonly string Function;
		public LuaOverloadAnnot(string function) => Function = function;
    }

	sealed class LuaAnnotationDocCommentSection : DocCommentSection {
		public readonly LuaParamAnnot[] Params;
		public readonly LuaReturnAnnot? Return;
		public readonly LuaOverloadAnnot[] Overloads;
		public readonly LuaClassAnnot? Class;
		public LuaAnnotationDocCommentSection(LuaClassAnnot? @class, LuaReturnAnnot? @return, LuaOverloadAnnot[] overloads, LuaParamAnnot[] @params) {
			Params = @params;
			Return = @return;
			Overloads = overloads;
			Class = @class;
		}
    }

	sealed class TestCodeDocCommentSection : DocCommentSection {
		public readonly string[] Lines;
		public TestCodeDocCommentSection(string[] lines) => Lines = lines;
    }

	readonly struct LuaMethodArg {
		public readonly string Name;
		public readonly string RustType;
		public readonly bool IsSelf;

		public LuaMethodArg(string name, string rustType, bool isSelf) {
			Name = name;
			RustType = rustType;
			IsSelf = isSelf;
		}
	}

	enum LuaMethodKind {
		Method,
		Function,
		Constructor,
	}

	sealed class LuaMethod {
		public readonly LuaMethodKind Kind;
		public readonly string Name;
		public readonly DocComments DocComments;
		public readonly uint ReturnValueCount;
		public readonly LuaMethodArg[] Args;

		public LuaMethod(LuaMethodKind kind, string name, DocComments docComments, uint returnValueCount, LuaMethodArg[] args) {
			Kind = kind;
			Name = name;
			DocComments = docComments;
			ReturnValueCount = returnValueCount;
			Args = args;
		}
	}

	readonly struct LuaModulePath {
		public readonly string[] Names;
		public LuaModulePath(string[] names) => Names = names;
	}

	sealed class LuaClass {
		public readonly string Name;
		public readonly LuaModulePath ModulePath;
		public readonly DocComments DocComments;
		public readonly LuaMethod[] Methods;
        public LuaClass(string name, LuaModulePath modulePath, DocComments docComments, LuaMethod[] methods) {
			Name = name;
			ModulePath = modulePath;
			DocComments = docComments;
			Methods = methods;
		}
    }
}
