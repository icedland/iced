using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Generator.Decoder;
using Generator.Encoder;
using Generator.Enums;
using Generator.Enums.Encoder;
using Generator.IO;

namespace Generator.Extended {
	abstract class ExtendedSyntaxGenerator {
		Dictionary<GroupKey, OpCodeInfoGroup> _groups;

		static readonly HashSet<Code> DiscardOpCodes = new HashSet<Code>() {
			Code.Add_rm8_imm8_82,
			Code.Or_rm8_imm8_82,
			Code.Adc_rm8_imm8_82,
			Code.Sbb_rm8_imm8_82,
			Code.And_rm8_imm8_82,
			Code.Sub_rm8_imm8_82,
			Code.Xor_rm8_imm8_82,
			Code.Cmp_rm8_imm8_82,
			Code.Test_rm16_imm16_F7r1,
			Code.Test_rm32_imm32_F7r1,
			Code.Test_rm64_imm32_F7r1,
			Code.Test_rm8_imm8_F6r1,
			Code.Lfence_E9,
			Code.Lfence_EA,
			Code.Lfence_EB,
			Code.Lfence_EC,
			Code.Lfence_ED,
			Code.Lfence_EE,
			Code.Lfence_EF,
			Code.Mfence_F1,
			Code.Mfence_F2,
			Code.Mfence_F3,
			Code.Mfence_F4,
			Code.Mfence_F5,
			Code.Mfence_F6,
			Code.Mfence_F7,
			Code.Sfence_F9,
			Code.Sfence_FA,
			Code.Sfence_FB,
			Code.Sfence_FC,
			Code.Sfence_FD,
			Code.Sfence_FE,
			Code.Sfence_FF
		};
		
		protected abstract void GenerateRegisters(EnumType registers);

		protected abstract void Generate(Dictionary<GroupKey, OpCodeInfoGroup> map, OpCodeInfoGroup[] opCodes);

		protected IdentifierConverter Converter { get; set; }

		public void Generate() {
			GenerateRegisters(RegisterEnum.Instance);
			Generate(OpCodeInfoTable.Data);
		}

		void Generate(OpCodeInfo[] opCodes) {
			_groups = new Dictionary<GroupKey, OpCodeInfoGroup>();

			foreach(var code in opCodes) {
				if (DiscardOpCodes.Contains((Code)code.Code.Value)) continue;
				
				var name = MnemonicsTable.Table[(int)code.Code.Value].mnemonicEnum.RawName.ToLowerInvariant();
				bool toAdd = true;
				var signature = new Signature();
				var regOnlySignature = new Signature();

				int maxImmediateSize = 0;
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
						
						case LegacyOpKind.Ib:
							Debug.Assert(maxImmediateSize == 0);
							maxImmediateSize = 1;
							argKind = ArgKind.Immediate;
							break;
						case LegacyOpKind.Iw:
							Debug.Assert(maxImmediateSize == 0);
							maxImmediateSize = 2;
							argKind = ArgKind.Immediate;
							break;
						case LegacyOpKind.Id:
							Debug.Assert(maxImmediateSize == 0);
							maxImmediateSize = 4;
							argKind = ArgKind.Immediate;
							break;
						case LegacyOpKind.Iq:
							Debug.Assert(maxImmediateSize == 0);
							maxImmediateSize = 8;
							argKind = ArgKind.Immediate;
							break;
						}
						
						if (argKind == ArgKind.Unknown) {
							toAdd = false;
						}
						signature.AddArgKind(argKind);
						regOnlySignature.AddArgKind(argKind == ArgKind.RegisterMemory ? ArgKind.Register : argKind);
					}
				}
				else {
					toAdd = false;
				}

				if (toAdd) {
					var group = AddOpCodeToGroup(name, signature, code);
					if (maxImmediateSize > group.MaxImmediateSize) {
						group.MaxImmediateSize = maxImmediateSize;
					}
					if (signature != regOnlySignature) {
						var regOnlyGroup = AddOpCodeToGroup(name, regOnlySignature, code);
						regOnlyGroup.HasRegisterMemoryMappedToRegister = true;
						if (maxImmediateSize > regOnlyGroup.MaxImmediateSize) {
							regOnlyGroup.MaxImmediateSize = maxImmediateSize;
						}
					}
				}
			}

			var orderedGroups = _groups.OrderBy(x => x.Key).Select(x => x.Value).ToArray();
			var signatures = new HashSet<Signature>();
			var opcodes = new List<OpCodeInfo>();
			foreach (var group in orderedGroups) {
				if (!group.HasRegisterMemoryMappedToRegister) continue;

				opcodes.Clear();
				signatures.Clear();

				// First-pass to select only register versions
				FilterOpCodesRegister(@group, opcodes, signatures, false);

				// Second-pass to populate with RM versions
				FilterOpCodesRegister(@group, opcodes, signatures, true);

				group.Items.Clear();
				group.Items.AddRange(opcodes);
			}
			
			Generate(_groups, orderedGroups);
		}

		void FilterOpCodesRegister(OpCodeInfoGroup @group, List<OpCodeInfo> opcodes, HashSet<Signature> signatures, bool allowMemory)
		{
			foreach (var code in @group.Items)
			{
				var registerSignature = new Signature();
				if (code is LegacyOpCodeInfo legacy)
				{
					bool isValid = true;
					for (int i = 0; i < legacy.OpKinds.Length; i++)
					{
						var argKind = GetRegisterKind(legacy.OpKinds[i], allowMemory);
						if (argKind == ArgKind.Unknown)
						{
							isValid = false;
							break;
						}

						registerSignature.AddArgKind(argKind);
					}

					if (isValid && signatures.Add(registerSignature))
					{
						opcodes.Add(code);
					}
				}
			}
		}


		private ArgKind GetRegisterKind(LegacyOpKind opKind, bool allowMemory) {
			switch (opKind) {
			case LegacyOpKind.AL:
			case LegacyOpKind.r8_rb:
			case LegacyOpKind.Gb:
				return ArgKind.Register8;
			case LegacyOpKind.AX:
			case LegacyOpKind.r16_rw:
			case LegacyOpKind.Gw:
				return ArgKind.Register16;
			case LegacyOpKind.EAX:
			case LegacyOpKind.r32_rd:
			case LegacyOpKind.Gd:
				return ArgKind.Register32;
			case LegacyOpKind.RAX:
			case LegacyOpKind.r64_ro:
			case LegacyOpKind.Gq:
				return ArgKind.Register64;
			case LegacyOpKind.Ib:
			case LegacyOpKind.Iw:
			case LegacyOpKind.Id:
			case LegacyOpKind.Iq:
				return ArgKind.Immediate;
				break;			
			}

			if (allowMemory) {
				switch (opKind) {
				case LegacyOpKind.Eb:
					return ArgKind.Register8;
				case LegacyOpKind.Ew:
					return ArgKind.Register16;
				case LegacyOpKind.Ed:
					return ArgKind.Register32;
				case LegacyOpKind.Eq:
					return ArgKind.Register64;
				}
			}
			
			return ArgKind.Unknown;
		}

		OpCodeInfoGroup AddOpCodeToGroup(string name, Signature signature, OpCodeInfo code) {
			var key = new GroupKey(name, signature);
			if (!_groups.TryGetValue(key, out var group)) {
				group = new OpCodeInfoGroup(name, signature);
				_groups.Add(key, group);
			}
			group.Items.Add(code);
			return group;
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

			Register8,
			Register16,
			Register32,
			Register64,

			RegisterMemory,
			Immediate,
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

			public bool HasRegisterMemoryMappedToRegister { get; set; }
			
			public int MaxImmediateSize { get; set; }
		}
	}
}
