// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator {
	readonly struct LanguageDocumentation {
		readonly string? defaultComment;
		readonly (TargetLanguage language, string comment)[]? langComments;

		public bool HasDefaultComment => defaultComment is not null;

		public LanguageDocumentation(string? comment) : this(comment, null) { }

		public LanguageDocumentation(string? comment, (TargetLanguage language, string comment)[]? langComments) {
			defaultComment = comment;
			this.langComments = langComments;
		}

		public string? GetComment(TargetLanguage language) {
			if (langComments is not null) {
				foreach (var (lang, comment) in langComments) {
					if (lang == language)
						return comment;
				}
			}
			return defaultComment;
		}
	}
}
