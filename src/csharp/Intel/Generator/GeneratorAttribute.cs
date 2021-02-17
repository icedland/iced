// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;

namespace Generator {
	[AttributeUsage(AttributeTargets.Class)]
	sealed class GeneratorAttribute : Attribute {
		public TargetLanguage Language { get; }
		public double Order { get; }

		public GeneratorAttribute(TargetLanguage language, double order = 0) {
			Language = language;
			Order = order;
		}
	}
}
