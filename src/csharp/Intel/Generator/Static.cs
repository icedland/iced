// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

using System.Diagnostics;

namespace Generator {
	static class Static {
		/// <summary>
		/// Call it like so:<br/>
		/// <br/>
		/// <c>Static.Assert(SomeClass.SomeConstant == 123 ? 0 : -1);</c>
		/// </summary>
		[Conditional("E3967789CA584C48B3D02600CAB3C7B2")]
		public static void Assert(byte ignored) { }
	}
}
