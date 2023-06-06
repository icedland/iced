// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Generator.Misc.Python {
	enum AttributeKind {
		Ignored,
		PyClass,
		PyMethods,
		Derive,
		New,
		Getter,
		Setter,
		StaticMethod,
		ClassMethod,
		TextSignature,
		Signature,
	}

	[DebuggerDisplay("Count = {Attributes.Count}")]
	sealed class RustAttributes {
		public readonly List<RustAttribute> Attributes = new();

		public bool Any(params AttributeKind[] attributes) {
			foreach (var attr in Attributes) {
				foreach (var kind in attributes) {
					if (attr.Kind == kind)
						return true;
				}
			}
			return false;
		}
	}

	[DebuggerDisplay("{Text,nq}")]
	sealed class RustAttribute {
		public readonly AttributeKind Kind;
		public readonly string Text;

		public RustAttribute(AttributeKind kind, string text) {
			Kind = kind;
			Text = text;
		}
	}

	enum DocCommentKind {
		Text,
		Args,
		Raises,
		Returns,
		Note,
		Warning,
		TestCode,
		TestOutput,
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

	[DebuggerDisplay("{SphinxType,nq}: {Documentation,nq}")]
	readonly struct TypeAndDocs {
		public readonly string SphinxType;
		public readonly string Documentation;

		public TypeAndDocs(string sphinxType, string documentation) {
			SphinxType = sphinxType;
			Documentation = documentation;
		}
	}

	[DebuggerDisplay("{Name,nq}: {SphinxType}")]
	readonly struct DocCommentArg {
		public readonly string Name;
		public readonly string SphinxType;
		public readonly string Documentation;

		public DocCommentArg(string name, string sphinxType, string documentation) {
			Name = name;
			SphinxType = sphinxType;
			Documentation = documentation;
		}
	}

	sealed class ArgsDocCommentSection : DocCommentSection {
		public readonly DocCommentArg[] Args;
		public ArgsDocCommentSection(DocCommentArg[] args) => Args = args;
	}

	sealed class RaisesDocCommentSection : DocCommentSection {
		public readonly TypeAndDocs[] Raises;
		public RaisesDocCommentSection(TypeAndDocs[] raises) => Raises = raises;
	}

	sealed class ReturnsDocCommentSection : DocCommentSection {
		public readonly TypeAndDocs Returns;
		public ReturnsDocCommentSection(TypeAndDocs returns) => Returns = returns;
	}

	sealed class NoteDocCommentSection : DocCommentSection {
		public readonly string[] Lines;
		public NoteDocCommentSection(string[] lines) => Lines = lines;
	}

	sealed class WarningDocCommentSection : DocCommentSection {
		public readonly string[] Lines;
		public WarningDocCommentSection(string[] lines) => Lines = lines;
	}

	sealed class TestCodeDocCommentSection : DocCommentSection {
		public readonly string[] Lines;
		public TestCodeDocCommentSection(string[] lines) => Lines = lines;
	}

	sealed class TestOutputDocCommentSection : DocCommentSection {
		public readonly string[] Lines;
		public TestOutputDocCommentSection(string[] lines) => Lines = lines;
	}

	[DebuggerDisplay("Count = {Sections.Count}")]
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

	[DebuggerDisplay("{Name,nq}: {RustType,nq}")]
	readonly struct PyMethodArg {
		public readonly string Name;
		public readonly string RustType;
		public readonly bool IsSelf;

		public PyMethodArg(string name, string rustType, bool isSelf) {
			Name = name;
			RustType = rustType;
			IsSelf = isSelf;
		}
	}

	[DebuggerDisplay("{Name} Args={Arguments.Count}")]
	sealed class PyMethod {
		public readonly string Name;
		public readonly DocComments DocComments;
		public readonly RustAttributes Attributes;
		public readonly List<PyMethodArg> Arguments;
		public readonly string RustReturnType;
		public bool HasReturnType =>
			RustReturnType != string.Empty &&
			RustReturnType != "PyResult<()>";

		public PyMethod(string name, DocComments docComments, RustAttributes attributes, List<PyMethodArg> arguments, string rustReturnType) {
			Name = name;
			DocComments = docComments;
			Attributes = attributes;
			Arguments = arguments;
			RustReturnType = rustReturnType;
		}
	}

	[DebuggerDisplay("{Name,nq} Methods={Methods.Count}")]
	sealed class PyClass {
		public readonly string Name;
		public readonly DocComments DocComments;
		public readonly RustAttributes Attributes;
		public readonly List<PyMethod> Methods;

		public PyClass(string name, DocComments docComments, RustAttributes attributes) {
			Name = name;
			DocComments = docComments;
			Attributes = attributes;
			Methods = new List<PyMethod>();
		}
	}
}
