// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Enums.Formatter {
	[Enum("FormatterTextKind", Documentation = "Formatter text kind", Public = true)]
	enum FormatterTextKind {
		[Comment("Normal text")]
		Text,
		[Comment("Assembler directive")]
		Directive,
		[Comment("Any prefix")]
		Prefix,
		[Comment("Any mnemonic")]
		Mnemonic,
		[Comment("Any keyword")]
		Keyword,
		[Comment("Any operator")]
		Operator,
		[Comment("Any punctuation")]
		Punctuation,
		[Comment("Number")]
		Number,
		[Comment("Any register")]
		Register,
		[Comment("A decorator, eg. #(c:sae)# in #(c:{sae})#")]
		Decorator,
		[Comment("Selector value (eg. far #(c:JMP)#/#(c:CALL)#)")]
		SelectorValue,
		[Comment("Label address (eg. #(c:JE XXXXXX)#)")]
		LabelAddress,
		[Comment("Function address (eg. #(c:CALL XXXXXX)#)")]
		FunctionAddress,
		[Comment("Data symbol")]
		Data,
		[Comment("Label symbol")]
		Label,
		[Comment("Function symbol")]
		Function,
	}
}
