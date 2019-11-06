using System.Runtime.CompilerServices;

[assembly: IgnoresAccessChecksTo("Iced")]

namespace System.Runtime.CompilerServices {
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	sealed class IgnoresAccessChecksToAttribute : Attribute {
		public IgnoresAccessChecksToAttribute(string assemblyName) => AssemblyName = assemblyName;
		public string AssemblyName { get; }
	}
}
