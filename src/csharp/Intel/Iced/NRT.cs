// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if NETFRAMEWORK || NETSTANDARD2_0
namespace System.Diagnostics.CodeAnalysis {
	[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
	sealed class NotNullWhenAttribute : Attribute {
		public NotNullWhenAttribute(bool returnValue) => ReturnValue = returnValue;
		public bool ReturnValue { get; }
	}
	[AttributeUsage(AttributeTargets.Method, Inherited = false)]
	sealed class DoesNotReturnAttribute : Attribute {
		public DoesNotReturnAttribute() { }
	}
	[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
	sealed class DoesNotReturnIfAttribute : Attribute {
		public DoesNotReturnIfAttribute(bool parameterValue) => ParameterValue = parameterValue;
		public bool ParameterValue { get; }
	}
}
#endif
