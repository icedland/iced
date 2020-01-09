using System;

namespace Iced.Intel
{
	public struct Label : IEquatable<Label> {
		internal Label(string name, ulong id) {
			Name = name ?? "___label";
			Id = id;
		}
		
		public readonly string Name;

		internal readonly ulong Id;

		public bool IsEmpty => Id == 0;

		public override string ToString() => "{Name}@{Id}";

		public bool Equals(Label other) => Name == other.Name && Id == other.Id;

		public override bool Equals(object? obj) => obj is Label other && Equals(other);

		public override int GetHashCode()
		{
			unchecked
			{
				return (Name.GetHashCode() * 397) ^ Id.GetHashCode();
			}
		}

		public static bool operator ==(Label left, Label right) => left.Equals(right);

		public static bool operator !=(Label left, Label right) => !left.Equals(right);
	}
}
