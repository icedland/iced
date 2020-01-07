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
			Code.INVALID,
			
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
			Code.Sfence_FF,
			
			Code.ReservedNop_rm16_r16_0F0D,
			Code.ReservedNop_rm32_r32_0F0D,
			Code.ReservedNop_rm64_r64_0F0D,
			Code.ReservedNop_rm16_r16_0F18,
			Code.ReservedNop_rm32_r32_0F18,
			Code.ReservedNop_rm64_r64_0F18,
			Code.ReservedNop_rm16_r16_0F19,
			Code.ReservedNop_rm32_r32_0F19,
			Code.ReservedNop_rm64_r64_0F19,
			Code.ReservedNop_rm16_r16_0F1A,
			Code.ReservedNop_rm32_r32_0F1A,
			Code.ReservedNop_rm64_r64_0F1A,
			Code.ReservedNop_rm16_r16_0F1B,
			Code.ReservedNop_rm32_r32_0F1B,
			Code.ReservedNop_rm64_r64_0F1B,
			Code.ReservedNop_rm16_r16_0F1C,
			Code.ReservedNop_rm32_r32_0F1C,
			Code.ReservedNop_rm64_r64_0F1C,
			Code.ReservedNop_rm16_r16_0F1D,
			Code.ReservedNop_rm32_r32_0F1D,
			Code.ReservedNop_rm64_r64_0F1D,
			Code.ReservedNop_rm16_r16_0F1E,
			Code.ReservedNop_rm32_r32_0F1E,
			Code.ReservedNop_rm64_r64_0F1E,
			Code.ReservedNop_rm16_r16_0F1F,
			Code.ReservedNop_rm32_r32_0F1F,
			Code.ReservedNop_rm64_r64_0F1F,
			
			Code.Cmpxchg486_rm8_r8,
			Code.Cmpxchg486_rm16_r16,
			Code.Cmpxchg486_rm32_r32,
			
			Code.Loadall286,
			Code.Loadallreset286
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

				var immKind = ImmediateKind.None;
				int immediateArgIndex = -1;
				
				int maxImmediateSize = 0;
				if (code is LegacyOpCodeInfo legacy) {
					for(int i = 0; i < legacy.OpKinds.Length; i++) {
						var opKind = legacy.OpKinds[i];
						var argKind = ArgKind.Unknown;
						bool skipArg = false;
						switch (opKind) {
						case LegacyOpKind.DX:
						case LegacyOpKind.CL:
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
						case LegacyOpKind.ES:
						case LegacyOpKind.CS:
						case LegacyOpKind.SS:
						case LegacyOpKind.DS:
						case LegacyOpKind.FS:
						case LegacyOpKind.GS:
							argKind = ArgKind.Register;
							break;

						case LegacyOpKind.Xb:
						case LegacyOpKind.Xd:
						case LegacyOpKind.Xw:
						case LegacyOpKind.Xq:
						case LegacyOpKind.Yb:
						case LegacyOpKind.Yd:
						case LegacyOpKind.Yw:
						case LegacyOpKind.Yq:
							argKind = ArgKind.HiddenMemory;
							break;
						
						case LegacyOpKind.Eb:
						case LegacyOpKind.Ew:
						case LegacyOpKind.Ed:
						case LegacyOpKind.Eq:
							argKind = ArgKind.RegisterMemory;
							break;
						
						case LegacyOpKind.Imm1:
							immediateArgIndex = i;
							immKind = ImmediateKind.Byte1;
							Debug.Assert(maxImmediateSize == 0);
							maxImmediateSize = 1;
							argKind = ArgKind.Immediate;
							break;
							
						case LegacyOpKind.Ib:			
						case LegacyOpKind.Ib16:
						case LegacyOpKind.Ib32:
						case LegacyOpKind.Ib64:
							Debug.Assert(maxImmediateSize == 0);
							immediateArgIndex = i;
							immKind = ImmediateKind.Byte;
							maxImmediateSize = 1;
							argKind = ArgKind.Immediate;
							break;
						
						case LegacyOpKind.Iw:
							Debug.Assert(maxImmediateSize == 0);
							immediateArgIndex = i;
							immKind = ImmediateKind.Standard;
							maxImmediateSize = 2;
							argKind = ArgKind.Immediate;
							break;
						case LegacyOpKind.Id: 
						case LegacyOpKind.Id64:							
							Debug.Assert(maxImmediateSize == 0);
							maxImmediateSize = 4;
							immediateArgIndex = i;
							immKind = ImmediateKind.Standard;
							argKind = ArgKind.Immediate;
							break;
						case LegacyOpKind.Iq:
							Debug.Assert(maxImmediateSize == 0);
							maxImmediateSize = 8;
							immediateArgIndex = i;
							immKind = ImmediateKind.Standard;
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
					var group = AddOpCodeToGroup(name, signature, code, immKind, immediateArgIndex);
					if (maxImmediateSize > group.MaxImmediateSize) {
						group.MaxImmediateSize = maxImmediateSize;
					}
					if (signature != regOnlySignature) {
						var regOnlyGroup = AddOpCodeToGroup(name, regOnlySignature, code, immKind, immediateArgIndex);
						regOnlyGroup.HasRegisterMemoryMappedToRegister = true;
						if (maxImmediateSize > regOnlyGroup.MaxImmediateSize) {
							regOnlyGroup.MaxImmediateSize = maxImmediateSize;
						}
					}
				}
				else {
					Console.WriteLine($"TODO: {code.GetType().Name} {name} => {code.Code.RawName} not supported yet");
				}
			}

			var orderedGroups = _groups.OrderBy(x => x.Key).Select(x => x.Value).ToArray();
			var signatures = new HashSet<Signature>();
			var opcodes = new List<OpCodeInfo>();
			foreach (var group in orderedGroups) {
				if (!group.HasRegisterMemoryMappedToRegister) continue;

				void ProcessOpCodes(List<OpCodeInfo> inputOpCodes)
				{
					opcodes.Clear();
					signatures.Clear();
					// First-pass to select only register versions
					FilterOpCodesRegister(@group, inputOpCodes, opcodes, signatures, false);

					// Second-pass to populate with RM versions
					FilterOpCodesRegister(@group, inputOpCodes, opcodes, signatures, true);

					inputOpCodes.Clear();
					inputOpCodes.AddRange(opcodes);
				}

				ProcessOpCodes(group.Items);
				
				if (group.ItemsWithImmediateByte1 != null) {
					ProcessOpCodes(group.ItemsWithImmediateByte1);
				}
				
				if (group.ItemsWithImmediateByte != null) {
					ProcessOpCodes(group.ItemsWithImmediateByte);
				}
			}
			
			Generate(_groups, orderedGroups);
		}

		private enum ImmediateKind {
			None,
			Standard,
			Byte1,
			Byte,
		}

		void FilterOpCodesRegister(OpCodeInfoGroup @group, List<OpCodeInfo> inputOpCodes, List<OpCodeInfo> opcodes, HashSet<Signature> signatures, bool allowMemory)
		{
			foreach (var code in inputOpCodes)
			{
				var registerSignature = new Signature();
				if (code is LegacyOpCodeInfo legacy)
				{
					bool isValid = true;
					for (int i = 0; i < legacy.OpKinds.Length; i++)
					{
						var argKind = GetFilterRegisterKindFromOpKind(legacy.OpKinds[i], allowMemory);
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

		private ArgKind GetFilterRegisterKindFromOpKind(LegacyOpKind opKind, bool allowMemory) {
			switch (opKind) {
			case LegacyOpKind.r8_rb:
			case LegacyOpKind.Gb:
				return ArgKind.FilterRegister8;
			case LegacyOpKind.r16_rw:
			case LegacyOpKind.Gw:
				return ArgKind.FilterRegister16;
			case LegacyOpKind.r32_rd:
			case LegacyOpKind.Gd:
				return ArgKind.FilterRegister32;
			case LegacyOpKind.r64_ro:
			case LegacyOpKind.Gq:
				return ArgKind.FilterRegister64;

			case LegacyOpKind.Imm1:
				return ArgKind.FilterImmediate1;

			case LegacyOpKind.Ib:
			case LegacyOpKind.Ib16:
			case LegacyOpKind.Ib32:
			case LegacyOpKind.Ib64:
				return ArgKind.FilterImmediate8;
			
			case LegacyOpKind.Iw: 
			case LegacyOpKind.Id:
			case LegacyOpKind.Id64:							
			case LegacyOpKind.Iq:
				return ArgKind.Immediate;

			case LegacyOpKind.DX:
				return ArgKind.FilterRegisterDX;
			case LegacyOpKind.CL:
				return ArgKind.FilterRegisterCL;
			case LegacyOpKind.AL:
				return ArgKind.FilterRegisterAL;
			case LegacyOpKind.AX:
				return ArgKind.FilterRegisterAX;
			case LegacyOpKind.EAX:
				return ArgKind.FilterRegisterEAX;
			case LegacyOpKind.RAX:
				return ArgKind.FilterRegisterRAX;

			case LegacyOpKind.ES:
				return ArgKind.FilterRegisterES;
			case LegacyOpKind.CS:
				return ArgKind.FilterRegisterCS;
			case LegacyOpKind.SS:
				return ArgKind.FilterRegisterSS;
			case LegacyOpKind.DS:
				return ArgKind.FilterRegisterDS;
			case LegacyOpKind.FS:
				return ArgKind.FilterRegisterFS;
			case LegacyOpKind.GS:
				return ArgKind.FilterRegisterGS;
			}

			if (allowMemory) {
				switch (opKind) {
				case LegacyOpKind.Eb:
					return ArgKind.FilterRegister8;
				case LegacyOpKind.Ew:
					return ArgKind.FilterRegister16;
				case LegacyOpKind.Ed:
					return ArgKind.FilterRegister32;
				case LegacyOpKind.Eq:
					return ArgKind.FilterRegister64;
				}
			}
			
			return ArgKind.Unknown;
		}

		protected static bool IsSegmentRegister(LegacyOpKind kind) {
			switch (kind) {
			case LegacyOpKind.ES:
			case LegacyOpKind.CS:
			case LegacyOpKind.FS:
			case LegacyOpKind.GS:
			case LegacyOpKind.SS:
			case LegacyOpKind.DS:
				return true;
			}

			return false;
		}
		
		protected static LegacyOpKind GetContextualLegacyOpKind(LegacyOpKind opKind, bool returnMemoryAsRegister) {
			switch (opKind) {
			case LegacyOpKind.Eb:
			case LegacyOpKind.r8_rb:
				return LegacyOpKind.Gb; 
			case LegacyOpKind.Ew:
			case LegacyOpKind.r16_rw:
				return LegacyOpKind.Gw;
			case LegacyOpKind.Ed:
			case LegacyOpKind.r32_rd:
				return LegacyOpKind.Gd;
			case LegacyOpKind.Eq:
			case LegacyOpKind.r64_ro:
				return LegacyOpKind.Gq;
			}
			return opKind;
		}

		OpCodeInfoGroup AddOpCodeToGroup(string name, Signature signature, OpCodeInfo code, ImmediateKind immKind, int immediateArgIndex) {
			var key = new GroupKey(name, signature);
			if (!_groups.TryGetValue(key, out var group)) {
				group = new OpCodeInfoGroup(name, signature);
				_groups.Add(key, group);
			}

			group.ImmediateArgIndex = immediateArgIndex;

			switch (immKind) {
			case ImmediateKind.Byte1:
				if (group.ItemsWithImmediateByte1 == null) group.ItemsWithImmediateByte1 = new List<OpCodeInfo>();
				group.ItemsWithImmediateByte1.Add(code);
				break;
			case ImmediateKind.Byte:
				if (group.ItemsWithImmediateByte == null) group.ItemsWithImmediateByte = new List<OpCodeInfo>();
				group.ItemsWithImmediateByte.Add(code);
				break;
			default:
				group.Items.Add(code);
				break;
			}

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
			RegisterMemory,
			HiddenMemory,
			Immediate,

			FilterRegister8,
			FilterRegister16,
			FilterRegister32,
			FilterRegister64,
		
			FilterRegisterDX,
			FilterRegisterCL,
			FilterRegisterAL,
			FilterRegisterAX,
			FilterRegisterEAX,
			FilterRegisterRAX,
			FilterRegisterES,
			FilterRegisterCS,
			FilterRegisterDS,
			FilterRegisterSS,
			FilterRegisterFS,
			FilterRegisterGS,

			FilterImmediate1,
			FilterImmediate8,
		}

		protected class OpCodeInfoGroup {
			public OpCodeInfoGroup(string name, Signature signature) {
				Name = name;
				Signature = signature;
				Items = new List<OpCodeInfo>();
				ImmediateArgIndex = -1;
			}
			
			public string Name { get; }

			public Signature Signature { get; }

			public List<OpCodeInfo> Items { get; }

			public List<OpCodeInfo> ItemsWithImmediateByte1 { get; set; }

			public List<OpCodeInfo> ItemsWithImmediateByte { get; set; }

			public bool HasRegisterMemoryMappedToRegister { get; set; }
			
			public int ImmediateArgIndex { get; set; }
			
			public int MaxImmediateSize { get; set; }
		}
	}
}
