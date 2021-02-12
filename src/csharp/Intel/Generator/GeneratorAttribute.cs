// SPDX-License-Identifier: MIT
// Copyright wtfsck@protonmail.com
// Copyright iced contributors

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
