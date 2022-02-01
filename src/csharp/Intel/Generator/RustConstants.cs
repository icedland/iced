// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using Generator.Enums;

namespace Generator {
	static class RustConstants {
		public const string DocHidden = "#[doc(hidden)]";

		public const string AttrTransparent = "#[repr(transparent)]";
		public const string AttrReprU32 = "#[repr(u32)]";
		public const string AttributeNoRustFmt = "#[rustfmt::skip]";
		public const string AttributeCopyClone = "#[derive(Copy, Clone)]";
		public const string AttributeCopyEq = "#[derive(Copy, Clone, Eq, PartialEq)]";
		public const string AttributeCopyEqOrdHash = "#[derive(Copy, Clone, Eq, PartialEq, Ord, PartialOrd, Hash)]";
		public const string AttributeAllowNonCamelCaseTypes = "#[allow(non_camel_case_types)]";
		public const string InnerAttributeAllowNonCamelCaseTypes = "#![allow(non_camel_case_types)]";
		public const string AttributeMustUse = "#[must_use]";
		public const string AttributeNonExhaustive = "#[cfg_attr(not(feature = \"exhaustive_enums\"), non_exhaustive)]";
		public const string AttributeInline = "#[inline]";
		public const string InnerAttributeMissingErrorsDoc = "#![allow(clippy::missing_errors_doc)]";
		public const string InnerAttributeAllowMissingDocs = "#![allow(missing_docs)]";
		public const string AttributeAllowMissingDocs = "#[allow(missing_docs)]";
		public const string AttributeAllowMissingCopyImplementations = "#[allow(missing_copy_implementations)]";
		public const string AttributeAllowMissingDebugImplementations = "#[allow(missing_debug_implementations)]";
		public const string AttributeAllowMissingInlineInPublicItems = "#[allow(clippy::missing_inline_in_public_items)]";
		public const string AttributeAllowTrivialCasts = "#[allow(trivial_casts)]";
		public const string AttributeAllowDeadCode = "#[allow(dead_code)]";
		public const string AttributeWasmBindgen = "#[wasm_bindgen]";
		public const string AttributeWasmBindgenJsName = "#[wasm_bindgen(js_name = \"{0}\")]";
		public const string AttributeAllowNonSnakeCase = "#[allow(non_snake_case)]";
		public const string AttributeAllowUnwrapUsed = "#[allow(clippy::unwrap_used)]";

		public const string FeaturePrefix = "#[cfg(";
		public const string FeatureInstrInfo = "#[cfg(feature = \"instr_info\")]";
		public const string FeatureEncoder = "#[cfg(feature = \"encoder\")]";
		public const string FeatureOpCodeInfo = "#[cfg(all(feature = \"encoder\", feature = \"op_code_info\"))]";
		public const string Vex = "not(feature = \"no_vex\")";
		public const string Evex = "not(feature = \"no_evex\")";
		public const string Xop = "not(feature = \"no_xop\")";
		public const string Mvex = "feature = \"mvex\"";
		public const string FeatureVex = "#[cfg(not(feature = \"no_vex\"))]";
		public const string FeatureXop = "#[cfg(not(feature = \"no_xop\"))]";
		public const string FeatureVexOrXop = "#[cfg(any(not(feature = \"no_vex\"), not(feature = \"no_xop\")))]";
		public const string FeatureVexOrXopOrEvexOrMvex = "#[cfg(any(not(feature = \"no_vex\"), not(feature = \"no_xop\"), not(feature = \"no_evex\"), feature = \"mvex\"))]";
		public const string FeatureEvex = "#[cfg(not(feature = \"no_evex\"))]";
		public const string FeatureMvex = "#[cfg(feature = \"mvex\")]";
		public const string FeatureD3now = "#[cfg(not(feature = \"no_d3now\"))]";
		public const string FeatureEncodingOne = "#[cfg({0})]";
		public const string FeatureEncodingMany = "#[cfg(any({0}))]";
		public const string FeatureDecoder = "#[cfg(feature = \"decoder\")]";
		public const string FeatureDecoderOrEncoder = "#[cfg(any(feature = \"decoder\", feature = \"encoder\"))]";
		public const string FeatureDecoderOrEncoderOrOpCodeInfo = "#[cfg(any(feature = \"decoder\", feature = \"encoder\", feature = \"op_code_info\"))]";
		public const string FeatureDecoderOrEncoderOrInstrInfoOrOpCodeInfo = "#[cfg(any(feature = \"decoder\", feature = \"encoder\", feature = \"instr_info\", feature = \"op_code_info\"))]";
		public const string FeatureGasIntelNasm = "#[cfg(any(feature = \"gas\", feature = \"intel\", feature = \"nasm\"))]";

		public static string? GetFeature(EncodingKind encoding) =>
			encoding switch {
				EncodingKind.Legacy => null,
				EncodingKind.VEX => FeatureVex,
				EncodingKind.EVEX => FeatureEvex,
				EncodingKind.XOP => FeatureXop,
				EncodingKind.D3NOW => FeatureD3now,
				EncodingKind.MVEX => FeatureMvex,
				_ => throw new InvalidOperationException(),
			};
	}
}
