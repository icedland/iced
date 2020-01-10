using System;

namespace Iced.Intel
{
	/// <summary>
	/// A label that can be created by <see cref="Assembler.CreateLabel"/>.
	/// </summary>
	public struct Label : IEquatable<Label> {
		internal Label(string? name, ulong id) {
			Name = name ?? "___label";
			Id = id;
		}
		
		/// <summary>
		/// Name of this label.
		/// </summary>
		public readonly string Name;

		/// <summary>
		/// Id of this label.
		/// </summary>
		internal readonly ulong Id;

		/// <summary>
		/// <c>true</c> if this label is empty and was not created by <see cref="Assembler.CreateLabel"/>.
		/// </summary>
		public bool IsEmpty => Id == 0;

		/// <inheritdoc />
		public override string ToString() => "{Name}@{Id}";

		/// <inheritdoc />
		public bool Equals(Label other) => Name == other.Name && Id == other.Id;

		/// <inheritdoc />
		public override bool Equals(object? obj) => obj is Label other && Equals(other);

		/// <inheritdoc />
		public override int GetHashCode()
		{
			unchecked
			{
				return (Name.GetHashCode() * 397) ^ Id.GetHashCode();
			}
		}

		/// <summary>
		/// Equality operator for <see cref="Label"/>
		/// </summary>
		/// <param name="left">Label</param>
		/// <param name="right">Label</param>
		/// <returns></returns>
		public static bool operator ==(Label left, Label right) => left.Equals(right);

		/// <summary>
		/// Inequality operator for <see cref="Label"/>
		/// </summary>
		/// <param name="left">Label</param>
		/// <param name="right">Label</param>
		/// <returns></returns>
		public static bool operator !=(Label left, Label right) => !left.Equals(right);
	}
}
