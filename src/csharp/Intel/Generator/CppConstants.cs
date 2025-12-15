// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System.IO;
using System.Linq;

namespace Generator {
	static class CppConstants {
		public const string Namespace = "iced_x86";
		public const string InternalNamespace = "iced_x86::internal";
		public const string IncludeSubdir = "include";
		public const string SrcSubdir = "src";
		public const string TestsSubdir = "tests";
		public const string InternalSubdir = "internal";
		public const string HeaderExt = ".hpp";
		public const string SourceExt = ".cpp";
		public const string MainHeader = "iced_x86.hpp";

		public static string GetFilename( GenTypes genTypes, params string[] paths ) =>
			Path.Combine( new[] { genTypes.Dirs.CppDir }.Concat( paths ).ToArray() );

		public static string GetHeaderFilename( GenTypes genTypes, params string[] paths ) =>
			Path.Combine( new[] { genTypes.Dirs.CppDir, IncludeSubdir, Namespace }.Concat( paths ).ToArray() );

		public static string GetInternalHeaderFilename( GenTypes genTypes, params string[] paths ) =>
			Path.Combine( new[] { genTypes.Dirs.CppDir, IncludeSubdir, Namespace, InternalSubdir }.Concat( paths ).ToArray() );

		public static string GetSourceFilename( GenTypes genTypes, params string[] paths ) =>
			Path.Combine( new[] { genTypes.Dirs.CppDir, SrcSubdir }.Concat( paths ).ToArray() );

		public static string GetTestFilename( GenTypes genTypes, params string[] paths ) =>
			Path.Combine( new[] { genTypes.Dirs.CppDir, TestsSubdir }.Concat( paths ).ToArray() );

		public static string GetHeaderGuard( params string[] parts ) {
			var combined = string.Join( "_", new[] { "ICED_X86" }.Concat( parts ).Select( p => p.ToUpperInvariant() ) );
			return combined + "_HPP";
		}
	}
}
