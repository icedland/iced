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

namespace Generator.Assembler {
	abstract class AssemblerSyntaxGenerator {
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
			Code.Loadallreset286,
			
			Code.Fstp_sti_DFD0,
			Code.Fstp_sti_DFD8,
			Code.Fxch_st0_sti_DDC8,
			Code.Fxch_st0_sti_DFC8,
			Code.Fcom_st0_sti_DCD0,
			Code.Fcomp_st0_sti_DED0,
			Code.Fcomp_st0_sti_DCD8,
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
				
				var memoName = MnemonicsTable.Table[(int)code.Code.Value].mnemonicEnum.RawName;
				//var name = memoName.ToLowerInvariant();
				bool toAdd = true;
				var signature = new Signature();
				var regOnlySignature = new Signature();

				var groupKind = GroupKind.None;
				int immediateArgIndex = -1;
				
				int maxImmediateSize = 0;
				for(int i = 0; i < code.OpKindsLength; i++) {
					var opKind = code.OpKind(i);
					var argKind = ArgKind.Unknown;
					bool skipArg = false;
					switch (opKind) {
					case CommonOpKind.DX:
					case CommonOpKind.CL:
					case CommonOpKind.AL:
					case CommonOpKind.AX:
					case CommonOpKind.EAX:
					case CommonOpKind.RAX:
					case CommonOpKind.r8_rb:
					case CommonOpKind.r16_rw:
					case CommonOpKind.r32_rd:
					case CommonOpKind.r64_ro:
					case CommonOpKind.Gb:
					case CommonOpKind.Gw:
					case CommonOpKind.Hd:
					case CommonOpKind.Hq:
					case CommonOpKind.Gd:
					case CommonOpKind.Gq:
					case CommonOpKind.ES:
					case CommonOpKind.CS:
					case CommonOpKind.SS:
					case CommonOpKind.DS:
					case CommonOpKind.FS:
					case CommonOpKind.GS: 
					case CommonOpKind.Is4X:
					case CommonOpKind.Is5X:
					case CommonOpKind.Is4Y:
					case CommonOpKind.Is5Y:
					case CommonOpKind.VX:
					case CommonOpKind.VY:
					case CommonOpKind.VZ:
					case CommonOpKind.HX:
					case CommonOpKind.HY:
					case CommonOpKind.HZ:
					case CommonOpKind.RX:
					case CommonOpKind.RY:
					case CommonOpKind.RZ:
					case CommonOpKind.P:
					case CommonOpKind.N:
					case CommonOpKind.Rd:
					case CommonOpKind.Rq:
					case CommonOpKind.VK:
					case CommonOpKind.HK:
					case CommonOpKind.RK:
					case CommonOpKind.Cd:
					case CommonOpKind.Dd:
					case CommonOpKind.Td:
					case CommonOpKind.ST:
					case CommonOpKind.STi:
						argKind = ArgKind.Register;
						break;

					case CommonOpKind.Xb:
					case CommonOpKind.Xd:
					case CommonOpKind.Xw:
					case CommonOpKind.Xq:
					case CommonOpKind.Yb:
					case CommonOpKind.Yd:
					case CommonOpKind.Yw:
					case CommonOpKind.Yq:
						argKind = ArgKind.HiddenMemory;
						break;
					
					case CommonOpKind.WK:
					case CommonOpKind.WX:
					case CommonOpKind.WY:
					case CommonOpKind.WZ:
					case CommonOpKind.Eb:
					case CommonOpKind.Ew:
					case CommonOpKind.Ed:
					case CommonOpKind.Eq:  
					case CommonOpKind.Q:
					case CommonOpKind.M:
					case CommonOpKind.RqMb:
					case CommonOpKind.RqMw:
					case CommonOpKind.RdMb:
					case CommonOpKind.RdMw:
						argKind = ArgKind.RegisterMemory;
						break;
					
					case CommonOpKind.MK:
					case CommonOpKind.Mp:
					case CommonOpKind.Mb:
					case CommonOpKind.Mw:
					case CommonOpKind.Md:
					case CommonOpKind.Mq:
					case CommonOpKind.VM32X:
					case CommonOpKind.VM32Y:
					case CommonOpKind.VM32Z:
					case CommonOpKind.VM64X:
					case CommonOpKind.VM64Y:
					case CommonOpKind.VM64Z:
						argKind = ArgKind.Memory;
						break;
					
					case CommonOpKind.Jw:
					case CommonOpKind.wJd:
					case CommonOpKind.dJd:
					case CommonOpKind.qJd:
						argKind = ArgKind.Branch;
						groupKind = GroupKind.BranchFar;
						break;
						
					case CommonOpKind.wJb:
					case CommonOpKind.dJb:
					case CommonOpKind.qJb:
						argKind = ArgKind.Branch;
						groupKind = GroupKind.BranchShort;
						break;
					
					case CommonOpKind.Imm1:
						immediateArgIndex = i;
						groupKind = GroupKind.ImmediateBit1;
						Debug.Assert(maxImmediateSize == 0);
						maxImmediateSize = 1;
						argKind = ArgKind.Immediate;
						break;
						
					case CommonOpKind.I2:
						immediateArgIndex = i;
						groupKind = GroupKind.ImmediateBits2;
						Debug.Assert(maxImmediateSize == 0);
						maxImmediateSize = 1;
						argKind = ArgKind.Immediate;
						break;

					case CommonOpKind.Ib:			
					case CommonOpKind.Ib16:
					case CommonOpKind.Ib32:
					case CommonOpKind.Ib64:
						Debug.Assert(maxImmediateSize == 0);
						immediateArgIndex = i;
						groupKind = GroupKind.ImmediateByte;
						maxImmediateSize = 1;
						argKind = ArgKind.Immediate;
						break;
					
					case CommonOpKind.Iw:
						Debug.Assert(maxImmediateSize == 0);
						immediateArgIndex = i;
						groupKind = GroupKind.Standard;
						maxImmediateSize = 2;
						argKind = ArgKind.Immediate;
						break;
					case CommonOpKind.Id: 
					case CommonOpKind.Id64:							
						Debug.Assert(maxImmediateSize == 0);
						maxImmediateSize = 4;
						immediateArgIndex = i;
						groupKind = GroupKind.Standard;
						argKind = ArgKind.Immediate;
						break;
					case CommonOpKind.Iq:
						Debug.Assert(maxImmediateSize == 0);
						maxImmediateSize = 8;
						immediateArgIndex = i;
						groupKind = GroupKind.Standard;
						argKind = ArgKind.Immediate;
						break;
					}
					
					if (argKind == ArgKind.Unknown) {
						toAdd = false;
					}
					signature.AddArgKind(argKind);
					regOnlySignature.AddArgKind(argKind == ArgKind.RegisterMemory ? ArgKind.Register : argKind);
				}

				if (toAdd) {
					var group = AddOpCodeToGroup(memoName, signature, code, groupKind, immediateArgIndex);
					if (maxImmediateSize > group.MaxImmediateSize) {
						group.MaxImmediateSize = maxImmediateSize;
					}
					if (signature != regOnlySignature) {
						var regOnlyGroup = AddOpCodeToGroup(memoName, regOnlySignature, code, groupKind, immediateArgIndex);
						regOnlyGroup.HasRegisterMemoryMappedToRegister = true;
						if (maxImmediateSize > regOnlyGroup.MaxImmediateSize) {
							regOnlyGroup.MaxImmediateSize = maxImmediateSize;
						}
					}
				}
				else {
					Console.WriteLine($"TODO: {code.GetType().Name} {memoName.ToLowerInvariant()} => {code.Code.RawName} not supported yet");
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
				
				if (group.ItemsWithImmediateBit1 != null) {
					ProcessOpCodes(group.ItemsWithImmediateBit1);
				}
				
				if (group.ItemsWithImmediateBits2 != null) {
					ProcessOpCodes(group.ItemsWithImmediateBits2);
				}

				if (group.ItemsWithImmediateByte != null) {
					ProcessOpCodes(group.ItemsWithImmediateByte);
				}
			}
			
			Generate(_groups, orderedGroups);
		}

		protected enum GroupKind {
			None,
			Standard,
			ImmediateBit1,
			ImmediateBits2,
			ImmediateByte,
			BranchShort,
			BranchFar,
		}

		void FilterOpCodesRegister(OpCodeInfoGroup @group, List<OpCodeInfo> inputOpCodes, List<OpCodeInfo> opcodes, HashSet<Signature> signatures, bool allowMemory)
		{
			foreach (var code in inputOpCodes)
			{
				var registerSignature = new Signature();
				bool isValid = true;
				for (int i = 0; i < code.OpKindsLength; i++)
				{
					var argKind = GetFilterRegisterKindFromOpKind(code.OpKind(i), allowMemory);
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

		private ArgKind GetFilterRegisterKindFromOpKind(CommonOpKind opKind, bool allowMemory) {
			switch (opKind) {
			case CommonOpKind.ST:
			case CommonOpKind.STi:
				return ArgKind.FilterRegisterST;
			
			case CommonOpKind.r8_rb:
			case CommonOpKind.Gb:
				return ArgKind.FilterRegister8;
			case CommonOpKind.r16_rw:
			case CommonOpKind.Gw:
				return ArgKind.FilterRegister16;
			case CommonOpKind.r32_rd:
			case CommonOpKind.Hd:
			case CommonOpKind.Gd:
			case CommonOpKind.Rd:
				return ArgKind.FilterRegister32;
			case CommonOpKind.r64_ro:
			case CommonOpKind.Hq:
			case CommonOpKind.Gq:
			case CommonOpKind.Rq:
				return ArgKind.FilterRegister64;

			case CommonOpKind.Imm1:
				return ArgKind.FilterImmediate1;

			case CommonOpKind.I2:
				return ArgKind.FilterImmediate2;

			case CommonOpKind.Ib:
			case CommonOpKind.Ib16:
			case CommonOpKind.Ib32:
			case CommonOpKind.Ib64:
				return ArgKind.FilterImmediate8;
			
			case CommonOpKind.Iw: 
			case CommonOpKind.Id:
			case CommonOpKind.Id64:							
			case CommonOpKind.Iq:
				return ArgKind.Immediate;

			case CommonOpKind.DX:
				return ArgKind.FilterRegisterDX;
			case CommonOpKind.CL:
				return ArgKind.FilterRegisterCL;
			case CommonOpKind.AL:
				return ArgKind.FilterRegisterAL;
			case CommonOpKind.AX:
				return ArgKind.FilterRegisterAX;
			case CommonOpKind.EAX:
				return ArgKind.FilterRegisterEAX;
			case CommonOpKind.RAX:
				return ArgKind.FilterRegisterRAX;

			case CommonOpKind.ES:
				return ArgKind.FilterRegisterES;
			case CommonOpKind.CS:
				return ArgKind.FilterRegisterCS;
			case CommonOpKind.SS:
				return ArgKind.FilterRegisterSS;
			case CommonOpKind.DS:
				return ArgKind.FilterRegisterDS;
			case CommonOpKind.FS:
				return ArgKind.FilterRegisterFS;
			case CommonOpKind.GS:
				return ArgKind.FilterRegisterGS;
			
			case CommonOpKind.Cd:
			case CommonOpKind.Dd:
			case CommonOpKind.Td:
				return ArgKind.FilterRegisterCDTR;
			
			case CommonOpKind.VK:
			case CommonOpKind.RK:
			case CommonOpKind.HK:
				return ArgKind.FilterRegisterK;
			case CommonOpKind.Is4X:
			case CommonOpKind.Is5X:
			case CommonOpKind.VX:
			case CommonOpKind.HX:
			case CommonOpKind.RX:
				return ArgKind.FilterRegisterXmm;
			case CommonOpKind.Is4Y:
			case CommonOpKind.Is5Y:
			case CommonOpKind.VY:
			case CommonOpKind.HY:
			case CommonOpKind.RY:
				return ArgKind.FilterRegisterYmm;
			case CommonOpKind.VZ:
			case CommonOpKind.HZ:
			case CommonOpKind.RZ:
				return ArgKind.FilterRegisterZmm;
			case CommonOpKind.P:
			case CommonOpKind.N:
				return ArgKind.FilterRegistermm;
			}

			if (allowMemory) {
				switch (opKind) {
				case CommonOpKind.Eb:
				case CommonOpKind.RdMb:
				case CommonOpKind.RqMb:
					return ArgKind.FilterRegister8;
				case CommonOpKind.Ew:
				case CommonOpKind.RdMw:
				case CommonOpKind.RqMw:
					return ArgKind.FilterRegister16;
				case CommonOpKind.Ed:
					return ArgKind.FilterRegister32;
				case CommonOpKind.Eq:
					return ArgKind.FilterRegister64;
			    case CommonOpKind.Q:
				    return ArgKind.FilterRegistermm;
				case CommonOpKind.WK:
					return ArgKind.FilterRegisterK;
				case CommonOpKind.WX:
				case CommonOpKind.M:
					return ArgKind.FilterRegisterXmm;
				case CommonOpKind.WY:
					return ArgKind.FilterRegisterYmm;
				case CommonOpKind.WZ:
					return ArgKind.FilterRegisterZmm;
				}
			}
			
			return ArgKind.Unknown;
		}

		protected static bool IsSegmentRegister(CommonOpKind kind) {
			switch (kind) {
			case CommonOpKind.ES:
			case CommonOpKind.CS:
			case CommonOpKind.FS:
			case CommonOpKind.GS:
			case CommonOpKind.SS:
			case CommonOpKind.DS:
				return true;
			}

			return false;
		}
		
		protected static CommonOpKind GetContextualCommonOpKind(CommonOpKind opKind, bool returnMemoryAsRegister) {
			switch (opKind) {
			case CommonOpKind.Eb:
			case CommonOpKind.r8_rb:
				return CommonOpKind.Gb; 
			case CommonOpKind.Ew:
			case CommonOpKind.r16_rw:
				return CommonOpKind.Gw;
			case CommonOpKind.Ed:
			case CommonOpKind.r32_rd:
			case CommonOpKind.Hd:
				return CommonOpKind.Gd;
			case CommonOpKind.Eq:
			case CommonOpKind.r64_ro:
			case CommonOpKind.Hq:
				return CommonOpKind.Gq;
			case CommonOpKind.WK:
				return CommonOpKind.VK;
			case CommonOpKind.RX:
			case CommonOpKind.WX:
			case CommonOpKind.HX:
			case CommonOpKind.M:
			case CommonOpKind.Is4X:
			case CommonOpKind.Is5X:
				return CommonOpKind.VX;
			case CommonOpKind.RY:
			case CommonOpKind.WY:
			case CommonOpKind.HY:
			case CommonOpKind.Is4Y:
			case CommonOpKind.Is5Y:
				return CommonOpKind.VY;
			case CommonOpKind.RZ:
			case CommonOpKind.WZ:
			case CommonOpKind.HZ:
				return CommonOpKind.VZ;
			}
			return opKind;
		}

		OpCodeInfoGroup AddOpCodeToGroup(string name, Signature signature, OpCodeInfo code, GroupKind immKind, int immediateArgIndex) {
			var key = new GroupKey(name, signature);
			if (!_groups.TryGetValue(key, out var group)) {
				group = new OpCodeInfoGroup(name, signature);
				_groups.Add(key, group);
			}

			group.ImmediateArgIndex = immediateArgIndex;

			switch (immKind) {
			case GroupKind.ImmediateBit1:
				if (group.ItemsWithImmediateBit1 == null) group.ItemsWithImmediateBit1 = new List<OpCodeInfo>();
				group.ItemsWithImmediateBit1.Add(code);
				break;
			case GroupKind.ImmediateBits2:
				if (group.ItemsWithImmediateBits2 == null) group.ItemsWithImmediateBits2 = new List<OpCodeInfo>();
				group.ItemsWithImmediateBits2.Add(code);
				break;
			case GroupKind.ImmediateByte:
				if (group.ItemsWithImmediateByte == null) group.ItemsWithImmediateByte = new List<OpCodeInfo>();
				group.ItemsWithImmediateByte.Add(code);
				break;
			case GroupKind.BranchShort:
				group.IsBranch = true;
				if (group.ItemsWithBranchNear == null) group.ItemsWithBranchNear = new List<OpCodeInfo>();
				group.ItemsWithBranchNear.Add(code);
				break;
			case GroupKind.BranchFar:
				group.IsBranch = true;
				if (group.ItemsWithBranchFar == null) group.ItemsWithBranchFar = new List<OpCodeInfo>();
				group.ItemsWithBranchFar.Add(code);
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
			Memory,
			HiddenMemory,
			Immediate,
			Branch,
			
			FilterRegisterCDTR,
			
			FilterRegisterK,
			
			FilterRegisterST,

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
			
			FilterRegistermm,

			FilterRegisterXmm,
			FilterRegisterYmm,
			FilterRegisterZmm,

			FilterImmediate1,
			FilterImmediate2,
			FilterImmediate8,
		}

		protected class OpCodeInfoGroup {
			public OpCodeInfoGroup(string name, Signature signature) {
				MemoName = name;
				Name = name.ToLowerInvariant();
				Signature = signature;
				Items = new List<OpCodeInfo>();
				ImmediateArgIndex = -1;
			}
			
			public string MemoName { get; }
			
			public string Name { get; }
			
			public bool IsBranch {get; set;}

			public Signature Signature { get; }

			public List<OpCodeInfo> Items { get; }

			public List<OpCodeInfo> ItemsWithImmediateBit1 { get; set; }

			public List<OpCodeInfo> ItemsWithImmediateBits2 { get; set; }

			public List<OpCodeInfo> ItemsWithImmediateByte { get; set; }

			public List<OpCodeInfo> ItemsWithBranchNear { get; set; }

			public List<OpCodeInfo> ItemsWithBranchFar { get; set; }
			
			public bool HasRegisterMemoryMappedToRegister { get; set; }
			
			public int ImmediateArgIndex { get; set; }
			
			public int MaxImmediateSize { get; set; }

			public IEnumerable<KeyValuePair<GroupKind, List<OpCodeInfo>>> GroupedItems {
				get {
					if (ItemsWithImmediateBit1 != null) {
						yield return new KeyValuePair<GroupKind, List<OpCodeInfo>>(GroupKind.ImmediateBit1, ItemsWithImmediateBit1);
					}
					if (ItemsWithImmediateBits2 != null) {
						yield return new KeyValuePair<GroupKind, List<OpCodeInfo>>(GroupKind.ImmediateBits2, ItemsWithImmediateBits2);
					}
					if (ItemsWithImmediateByte != null) {
						yield return new KeyValuePair<GroupKind, List<OpCodeInfo>>(GroupKind.ImmediateByte, ItemsWithImmediateByte);
					}
					if (ItemsWithBranchNear != null) {
						yield return new KeyValuePair<GroupKind, List<OpCodeInfo>>(GroupKind.BranchShort, ItemsWithBranchNear);
					}
					if (ItemsWithBranchFar != null) {
						yield return new KeyValuePair<GroupKind, List<OpCodeInfo>>(GroupKind.BranchFar, ItemsWithBranchFar);
					}
					if (Items.Count > 0) {
						yield return new KeyValuePair<GroupKind, List<OpCodeInfo>>(GroupKind.None, Items);
					}
				}
			}
		}
	}
}
