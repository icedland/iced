// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System.Runtime.CompilerServices;

[assembly: IgnoresAccessChecksTo("Iced")]

namespace System.Runtime.CompilerServices {
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	sealed class IgnoresAccessChecksToAttribute : Attribute {
		public IgnoresAccessChecksToAttribute(string assemblyName) => AssemblyName = assemblyName;
		public string AssemblyName { get; }
	}
}
