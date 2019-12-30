/*
Copyright (C) 2018-2019 de4dot@gmail.com

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

namespace Generator {
	static class RustConstants {
		// "cargo-fmt" can be anything, rustfmt always sees the attribute
		const string attrNoRustFmt = "[cfg_attr(feature = \"cargo-fmt\", rustfmt::skip)]";

		public const string AttributeNoRustFmt = "#" + attrNoRustFmt;
		public const string AttributeNoRustFmtInner = "#!" + attrNoRustFmt;
		public const string AttributeCopyEq = "#[derive(Copy, Clone, Eq, PartialEq)]";
		public const string AttributeCopyEqOrdHash = "#[derive(Copy, Clone, Eq, PartialEq, Ord, PartialOrd, Hash)]";
		public const string AttributeAllowNonCamelCaseTypes = "#[allow(non_camel_case_types)]";
		public const string AttributeMustUse = "#[cfg_attr(has_must_use, must_use)]";
		public const string AttributeNonExhaustive = "#[cfg_attr(all(not(feature = \"exhaustive_enums\"), has_non_exhaustive), non_exhaustive)]";
		public const string AttributeInline = "#[inline]";
		public const string AttributeAllowMissingInlineInPublicItems = "#[cfg_attr(feature = \"cargo-clippy\", allow(clippy::missing_inline_in_public_items))]";
		public const string AttributeAllowMissingDocs = "#[allow(missing_docs)]";
		public const string AttributeAllowMissingCopyImplementations = "#[allow(missing_copy_implementations)]";

		public const string FeaturePrefix = "#[cfg(";
		public const string FeatureInstrInfo = "#[cfg(feature = \"instr_info\")]";
		public const string FeatureEncoder = "#[cfg(feature = \"encoder\")]";
		public const string FeatureDecoderOrEncoder = "#[cfg(any(feature = \"decoder\", feature = \"encoder\"))]";
		public const string FeatureDecoderOrEncoderOrInstrInfo = "#[cfg(any(feature = \"decoder\", feature = \"encoder\", feature = \"instr_info\"))]";
	}
}
