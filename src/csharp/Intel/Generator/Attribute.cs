// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Generator {
	[AttributeUsage(AttributeTargets.Field)]
	sealed class DeprecatedAttribute : Attribute {
		public string Version { get; }
		public string? NewName { get; }
		public string? Description { get; }
		public bool IsError { get; }
		public DeprecatedAttribute(string version, string? newName, string? description = null, bool isError = true) {
			Version = version;
			NewName = newName;
			Description = description;
			IsError = isError;
		}

		public static DeprecatedInfo GetDeprecatedInfo(MemberInfo member) {
			if (member.GetCustomAttribute(typeof(DeprecatedAttribute)) is DeprecatedAttribute ca)
				return new DeprecatedInfo(ca.Version, ca.NewName, ca.Description, ca.IsError);
			return default;
		}
	}

	readonly struct DeprecatedInfo {
		// Deprecated and a new renamed value was added
		public bool IsDeprecatedAndRenamed => NewName is not null;
		public bool IsDeprecated => VersionStr is not null;
		public readonly Version Version;
		public readonly string VersionStr;
		public readonly string? NewName;
		public readonly string? Description;
		public readonly bool IsError;
		public DeprecatedInfo(string version, string? newName, string? description, bool isError) {
			Version = new Version(version);
			VersionStr = version;
			NewName = newName;
			Description = description;
			IsError = isError;
		}
	}

	[AttributeUsage(AttributeTargets.All)]
	sealed class CommentAttribute : Attribute {
		public string Comment { get; }
		public string? CSharp { get; set; }
		public string? Rust { get; set; }
		public string? RustJS { get; set; }
		public string? Python { get; set; }
		public string? Lua { get; set; }
		public string? Java { get; set; }
		public CommentAttribute(string comment) => Comment = comment ?? throw new InvalidOperationException();

		public static LanguageDocumentation GetDocumentation(MemberInfo member) {
			if (member.GetCustomAttribute<CommentAttribute>() is not CommentAttribute attr)
				return default;
			var langComments = new List<(TargetLanguage language, string comment)>();
			if (attr.CSharp is string csharpComment)
				langComments.Add((TargetLanguage.CSharp, csharpComment));
			if (attr.Rust is string rustComment)
				langComments.Add((TargetLanguage.Rust, rustComment));
			if (attr.RustJS is string rustJSComment)
				langComments.Add((TargetLanguage.RustJS, rustJSComment));
			if (attr.Python is string pythonComment)
				langComments.Add((TargetLanguage.Python, pythonComment));
			if (attr.Lua is string luaComment)
				langComments.Add((TargetLanguage.Lua, luaComment));
			if (attr.Java is string javaComment)
				langComments.Add((TargetLanguage.Java, javaComment));
			return new(attr.Comment, langComments.Count == 0 ? Array.Empty<(TargetLanguage language, string comment)>() : langComments.ToArray());
		}
	}

	[AttributeUsage(AttributeTargets.Enum)]
	sealed class EnumAttribute : Attribute {
		public string Name { get; }
		public new TypeId TypeId { get; }
		public string? Documentation { get; set; }
		public bool Public { get; set; }
		public bool NoInitialize { get; set; }
		public bool Flags { get; set; }

		public EnumAttribute(string typeId) {
			Name = typeId;
			TypeId = new TypeId(typeId);
		}

		public EnumAttribute(string name, string typeId) {
			Name = name;
			TypeId = new TypeId(typeId);
		}

		public LanguageDocumentation GetDocumentation() => new(Documentation);
	}

	static class TypeGenOrders {
		/// <summary>
		/// Only depends on exported enums (<see cref="EnumAttribute"/>), does not depend on other created enums/constants.
		/// Must not depend on Code.
		/// </summary>
		public const double NoDeps = 0;

		/// <summary>
		/// Depends on Code (before it's been filtered) and is called before instructions are filtered
		/// </summary>
		public const double PreCreateInstructions = 9000;

		/// <summary>
		/// Depends on Code, InstructionDefs and anything else that depends on them
		/// </summary>
		public const double CreatedInstructions = 10000;

		/// <summary>
		/// Depends on anything created earlier
		/// </summary>
		public const double Last = double.MaxValue;
	}

	[AttributeUsage(AttributeTargets.Class)]
	sealed class TypeGenAttribute : Attribute {
		public double Order { get; }
		public TypeGenAttribute(double order) {
			if (double.IsNaN(order))
				throw new ArgumentOutOfRangeException(nameof(order));
			Order = order;
		}
	}
}
