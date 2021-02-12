// SPDX-License-Identifier: MIT
// Copyright wtfsck@protonmail.com
// Copyright iced contributors

using System;
using System.Reflection;

namespace Generator {
	[AttributeUsage(AttributeTargets.Field)]
	sealed class DeprecatedAttribute : Attribute {
		public string Version { get; }
		public string? NewName { get; }
		public string? Description { get; }
		public DeprecatedAttribute(string version, string? newName, string? description = null) {
			Version = version;
			NewName = newName;
			Description = description;
		}

		public static DeprecatedInfo GetDeprecatedInfo(MemberInfo member) {
			if (member.GetCustomAttribute(typeof(DeprecatedAttribute)) is DeprecatedAttribute ca)
				return new DeprecatedInfo(ca.Version, ca.NewName, ca.Description);
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
		public DeprecatedInfo(string version, string? newName, string? description) {
			Version = new Version(version);
			VersionStr = version;
			NewName = newName;
			Description = description;
		}
	}

	[AttributeUsage(AttributeTargets.All)]
	sealed class CommentAttribute : Attribute {
		public string Comment { get; }
		public CommentAttribute(string comment) => Comment = comment ?? throw new InvalidOperationException();

		public static string? GetDocumentation(MemberInfo member) =>
			((CommentAttribute?)member.GetCustomAttribute(typeof(CommentAttribute)))?.Comment;
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
