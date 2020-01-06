using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Generator.Encoder;
using Generator.Enums;
using Generator.Enums.Encoder;
using Generator.IO;

namespace Generator.Extended {
	abstract class ExtendedSyntaxGenerator {
		protected abstract void GenerateRegisters(EnumType registers);

		protected abstract void Generate(Dictionary<GroupKey, OpCodeInfoGroup> map, OpCodeInfoGroup[] opCodes);

		protected IdentifierConverter Converter { get; set; }

		public void Generate() {
			GenerateRegisters(RegisterEnum.Instance);
			Generate(OpCodeInfoTable.Data);
		}

		void Generate(OpCodeInfo[] opCodes) {
			var groups = new Dictionary<GroupKey, OpCodeInfoGroup>();

			foreach(var code in opCodes) {
				var name = code.Code.Name(Converter).ToLowerInvariant();
				var indexOfUnder = name.IndexOf('_');
				name = indexOfUnder > 0 ? name.Substring(0, indexOfUnder) : name;

				bool toAdd = true;
				var signature = new Signature();

				if (code is LegacyOpCodeInfo legacy) {
					for(int i = 0; i < legacy.OpKinds.Length; i++) {
						var opKind = legacy.OpKinds[i];
						var argKind = ArgKind.Unknown;
						switch (opKind) {
						case LegacyOpKind.AL:
						case LegacyOpKind.AX:
						case LegacyOpKind.EAX:
						case LegacyOpKind.RAX:
						case LegacyOpKind.r8_rb:
						case LegacyOpKind.r16_rw:
						case LegacyOpKind.r32_rd:
						case LegacyOpKind.r64_ro:
						case LegacyOpKind.Gb:
						case LegacyOpKind.Gw:
						case LegacyOpKind.Gd:
						case LegacyOpKind.Gq:
							argKind = ArgKind.Register;
							break;

						case LegacyOpKind.Eb:
						case LegacyOpKind.Ew:
						case LegacyOpKind.Ed:
						case LegacyOpKind.Eq:
							argKind = ArgKind.RegisterMemory;
							break;
						}

						if (argKind == ArgKind.Unknown) {
							toAdd = false;
						}
						signature.AddArgKind(argKind);
					}
				}

				if (toAdd) {
					var key = new GroupKey(name, signature);
					if (!groups.TryGetValue(key, out var group)) {
						group = new OpCodeInfoGroup(name, signature);
						groups.Add(key, group);
					}
					group.Items.Add(code);
				}
			}

			var orderedGroups = groups.OrderBy(x => x.Key).Select(x => x.Value).ToArray();
			Generate(groups, orderedGroups);
		}

		[DebuggerDisplay("{Name} {Kind}")]
		protected readonly struct GroupKey : IEquatable<GroupKey>, IComparable<GroupKey> {
			public GroupKey(string name, Signature signature) {
				Name = name;
				Signature = signature;
			}

			public readonly string Name;

			public readonly Signature Signature;

			public bool Equals(GroupKey other) => Name == other.Name && Signature == other.Signature;

			public override bool Equals(object? obj) => obj is GroupKey other && Equals(other);

			public override int GetHashCode() => HashCode.Combine(Name, Signature);

			public static bool operator ==(GroupKey left, GroupKey right) => left.Equals(right);

			public static bool operator !=(GroupKey left, GroupKey right) => !left.Equals(right);

			public int CompareTo(GroupKey other)
			{
				var nameComparison = string.Compare(Name, other.Name, StringComparison.Ordinal);
				if (nameComparison != 0) return nameComparison;
				return Signature.CompareTo(other.Signature);
			}
		}

		protected struct Signature : IEquatable<Signature>, IComparable<Signature> {

			public int ArgCount;

			ulong _argKinds;

			public ArgKind GetArgKind(int argIndex) => (ArgKind)((_argKinds >> (8 * argIndex)) & 0xFF);

			public void AddArgKind(ArgKind kind) {
				var shift = (8 * ArgCount);
				_argKinds = (_argKinds & ~((ulong)0xFF << shift)) | ((ulong)kind << shift);
				ArgCount++;
			}

			public override string ToString() {
				var builder = new StringBuilder();
				builder.Append('(');
				for(int i = 0; i < ArgCount; i++) {
					if (i > 0) builder.Append(", ");
					builder.Append(GetArgKind(i));
				}
				builder.Append(')');
				return builder.ToString();
			}

			public bool Equals(Signature other) => ArgCount == other.ArgCount && _argKinds == other._argKinds;

			public override bool Equals(object? obj) => obj is Signature other && Equals(other);

			public override int GetHashCode() => HashCode.Combine(ArgCount, _argKinds);

			public static bool operator ==(Signature left, Signature right) => left.Equals(right);

			public static bool operator !=(Signature left, Signature right) => !left.Equals(right);

			public int CompareTo(Signature other)
			{
				var argCountComparison = ArgCount.CompareTo(other.ArgCount);
				if (argCountComparison != 0) return argCountComparison;
				return _argKinds.CompareTo(other._argKinds);
			}
		}
		
		protected enum ArgKind : byte {
			Unknown,
			Register,
			RegisterMemory,
			Immediate,
			Immediate8,
		}

		protected class OpCodeInfoGroup {
			public OpCodeInfoGroup(string name, Signature signature) {
				Name = name;
				Signature = signature;
				Items = new List<OpCodeInfo>();
			}
			
			public string Name { get; }

			public Signature Signature { get; }

			public List<OpCodeInfo> Items { get; }
		}
	}
}
