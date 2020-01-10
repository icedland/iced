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
			
			Code.VEX_Vmovss_xmm_xmm_xmm_0F11,
			Code.VEX_Vmovsd_xmm_xmm_xmm_0F11,
			Code.EVEX_Vmovss_xmm_k1z_xmm_xmm_0F11,
			Code.EVEX_Vmovsd_xmm_k1z_xmm_xmm_0F11
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
				var name = memoName.ToLowerInvariant();
				bool toAdd = true;
				var signature = new Signature();
				var regOnlySignature = new Signature();

				var opCodeArgFlags = OpCodeArgFlags.Default;

				if (code is VexOpCodeInfo) opCodeArgFlags |= OpCodeArgFlags.HasVex;
				if (code is EvexOpCodeInfo) opCodeArgFlags |= OpCodeArgFlags.HasEvex;

				var argSizes = new List<int>();
				bool isSkipOk = false;
				
				for(int i = 0; i < code.OpKindsLength; i++) {
					var opKind = code.OpKind(i);
					var argKind = ArgKind.Unknown;
					int argSize = 0;
					bool skipArg = false;
					switch (opKind) {
					case OpCodeOperandKind.dx:
					case OpCodeOperandKind.cl:
					case OpCodeOperandKind.al:
					case OpCodeOperandKind.ax:
					case OpCodeOperandKind.eax:
					case OpCodeOperandKind.rax:
					case OpCodeOperandKind.r8_opcode:
					case OpCodeOperandKind.r16_opcode:
					case OpCodeOperandKind.r32_opcode:
					case OpCodeOperandKind.r64_opcode:
					case OpCodeOperandKind.r8_reg:
					case OpCodeOperandKind.r16_reg:
					case OpCodeOperandKind.r32_vvvv:
					case OpCodeOperandKind.r64_vvvv:
					case OpCodeOperandKind.r32_reg:
					case OpCodeOperandKind.r64_reg:
					case OpCodeOperandKind.es:
					case OpCodeOperandKind.cs:
					case OpCodeOperandKind.ss:
					case OpCodeOperandKind.ds:
					case OpCodeOperandKind.fs:
					case OpCodeOperandKind.gs: 
					case OpCodeOperandKind.xmm_is4:
					case OpCodeOperandKind.xmm_is5:
					case OpCodeOperandKind.ymm_is4:
					case OpCodeOperandKind.ymm_is5:
					case OpCodeOperandKind.xmm_reg:
					case OpCodeOperandKind.ymm_reg:
					case OpCodeOperandKind.zmm_reg:
					case OpCodeOperandKind.xmm_vvvv:
					case OpCodeOperandKind.ymm_vvvv:
					case OpCodeOperandKind.zmm_vvvv:
					case OpCodeOperandKind.xmm_rm:
					case OpCodeOperandKind.ymm_rm:
					case OpCodeOperandKind.zmm_rm:
					case OpCodeOperandKind.mm_reg:
					case OpCodeOperandKind.mm_rm:
					case OpCodeOperandKind.r32_rm:
					case OpCodeOperandKind.r64_rm:
					case OpCodeOperandKind.k_reg:
					case OpCodeOperandKind.k_vvvv:
					case OpCodeOperandKind.k_rm:
					case OpCodeOperandKind.cr_reg:
					case OpCodeOperandKind.dr_reg:
					case OpCodeOperandKind.tr_reg:
					case OpCodeOperandKind.st0:
					case OpCodeOperandKind.sti_opcode:
					case OpCodeOperandKind.bnd_reg:
					case OpCodeOperandKind.seg_reg:
						argKind = ArgKind.Register;
						break;

					case OpCodeOperandKind.seg_rSI:
					case OpCodeOperandKind.es_rDI:
						argKind = ArgKind.HiddenMemory;
						break;
					
					case OpCodeOperandKind.k_or_mem:
					case OpCodeOperandKind.mm_or_mem:
					case OpCodeOperandKind.xmm_or_mem:
					case OpCodeOperandKind.ymm_or_mem:
					case OpCodeOperandKind.zmm_or_mem:
					case OpCodeOperandKind.r8_or_mem:
					case OpCodeOperandKind.r16_or_mem:
					case OpCodeOperandKind.r32_or_mem:
					case OpCodeOperandKind.r64_or_mem:  
					case OpCodeOperandKind.r64_or_mem_mpx:
					case OpCodeOperandKind.r32_or_mem_mpx:
					case OpCodeOperandKind.bnd_or_mem_mpx:
						argKind = ArgKind.RegisterMemory;
						break;
					
					case OpCodeOperandKind.mem8_offs:
					case OpCodeOperandKind.mem:
					case OpCodeOperandKind.mem8:
					case OpCodeOperandKind.mem16:
					case OpCodeOperandKind.mem32:
					case OpCodeOperandKind.mem64:
					case OpCodeOperandKind.mem80:
					case OpCodeOperandKind.mem128:
					case OpCodeOperandKind.mem_mpx:
					case OpCodeOperandKind.mem_mib:
					case OpCodeOperandKind.mem_vsib32x:
					case OpCodeOperandKind.mem_vsib64x:
					case OpCodeOperandKind.mem_vsib32y:
					case OpCodeOperandKind.mem_vsib64y:
					case OpCodeOperandKind.mem_vsib32z:
					case OpCodeOperandKind.mem_vsib64z:
						argKind = ArgKind.Memory;
						break;
					
					case OpCodeOperandKind.xbegin_4:
					case OpCodeOperandKind.brdisp_4:
						argKind = ArgKind.Branch;
						opCodeArgFlags |= OpCodeArgFlags.HasBranchFar;
						break;

					// Because We encode only relative byte encoding by default 
					case OpCodeOperandKind.br32_4: // NEAR
					case OpCodeOperandKind.br64_4: // NEAR
					case OpCodeOperandKind.br16_2: // SHORT
						isSkipOk = true;
						break;

					case OpCodeOperandKind.xbegin_2:
					case OpCodeOperandKind.brdisp_2:
					case OpCodeOperandKind.br16_1:
					case OpCodeOperandKind.br32_1:
					case OpCodeOperandKind.br64_1:
						argKind = ArgKind.Branch;
						opCodeArgFlags |= OpCodeArgFlags.HasBranchShort;
						break;
					
					case OpCodeOperandKind.imm8_const_1:
						opCodeArgFlags |= OpCodeArgFlags.HasImmediateByteEqual1;
						argSize = 1;
						argKind = ArgKind.ImmediateByte;
						break;
						
					case OpCodeOperandKind.imm2_m2z:
						opCodeArgFlags |= OpCodeArgFlags.HasImmediateByteLessThanBits;
						argKind = ArgKind.ImmediateByte;
						argSize = 1;
						break;

					case OpCodeOperandKind.imm8:			
						argKind = ArgKind.ImmediateByte;
						argSize = 1;
						break;

					case OpCodeOperandKind.imm8sex16:
					case OpCodeOperandKind.imm8sex32:
					case OpCodeOperandKind.imm8sex64:
						opCodeArgFlags |= OpCodeArgFlags.HasImmediateByteSigned;
						argKind = ArgKind.Immediate;
						argSize = 1;
						break;
					
					case OpCodeOperandKind.imm16:
						argKind = ArgKind.Immediate;
						argSize = 2;
						break;
					
					case OpCodeOperandKind.imm32: 
					case OpCodeOperandKind.imm32sex64:							
						argKind = ArgKind.Immediate;
						argSize = 4;
						break;
					
					case OpCodeOperandKind.imm64:
						argKind = ArgKind.Immediate;
						argSize = 8;
						break;
					}
					
					if (argKind == ArgKind.Unknown) {
						toAdd = false;
						break;
					}
					else {
						argSizes.Add(argSize);
						signature.AddArgKind(argKind == ArgKind.RegisterMemory ? ArgKind.Memory : argKind);
						regOnlySignature.AddArgKind(argKind == ArgKind.RegisterMemory ? ArgKind.Register : argKind);
					}
				}

				if (toAdd) {
					var group = AddOpCodeToGroup(memoName, signature, code, opCodeArgFlags);
					group.UpdateMaxArgSizes(argSizes);
					if (signature != regOnlySignature) {
						var regOnlyGroup = AddOpCodeToGroup(memoName, regOnlySignature, code, opCodeArgFlags | OpCodeArgFlags.HasRegisterMemoryMappedToRegister);
						regOnlyGroup.UpdateMaxArgSizes(argSizes);
					}
				}
				else {
					if (!isSkipOk) {
						Console.WriteLine($"TODO: {code.GetType().Name} {memoName.ToLowerInvariant()} => {code.Code.RawName} not supported yet");
					}
				}
			}

			var orderedGroups = _groups.OrderBy(x => x.Key).Select(x => x.Value).ToArray();
			var signatures = new HashSet<Signature>();
			var opcodes = new List<OpCodeInfo>();
			foreach (var group in orderedGroups) {
				if (group.HasRegisterMemoryMappedToRegister) {
					var inputOpCodes = group.Items; 
					opcodes.Clear();
					signatures.Clear();
					// First-pass to select only register versions
					FilterOpCodesRegister(@group, inputOpCodes, opcodes, signatures, false);

					// Second-pass to populate with RM versions
					FilterOpCodesRegister(@group, inputOpCodes, opcodes, signatures, true);

					inputOpCodes.Clear();
					inputOpCodes.AddRange(opcodes);
				}
				
				// Update the selector graph for this group of opcodes
				group.RootOpCodeNode = BuildSelectorGraph(group);
			}
			
			Generate(_groups, orderedGroups);
		}

		OpCodeNode BuildSelectorGraph(OpCodeInfoGroup group) {
			// In case of one opcode, we don't need to perform any disambiguation
			var opcodes = group.Items; 
			if (opcodes.Count == 1) {
				return new OpCodeNode(opcodes[0]);
			}
			
			// Sort opcodes by decreasing size
			opcodes.Sort(group.OrderOpCodesPerOpKindPriority);

			if ((group.Flags & OpCodeArgFlags.HasImmediateByteEqual1) != 0) {
				// handle imm8 == 1 
				var opcodesWithImmediateByteEqual1 = new List<OpCodeInfo>();
				var opcodesOthers = new List<OpCodeInfo>();
				var indices = CollectByOperandKindPredicate(opcodes, IsImmediateByteEqual1, opcodesWithImmediateByteEqual1, opcodesOthers);
				Debug.Assert(indices.Count == 1);
				var newFlags = group.Flags ^ OpCodeArgFlags.HasImmediateByteEqual1;
				return new OpCodeSelector(indices[0], OpCodeSelectorKind.ImmediateByteEqual1) {IfTrue = BuildSelectorGraph(group.Signature, newFlags, opcodesWithImmediateByteEqual1), IfFalse = BuildSelectorGraph(group.Signature, newFlags, opcodesOthers)};
			}
			else if ((group.Flags & OpCodeArgFlags.HasImmediateByteSigned) != 0) { 
				// handle imm >= sbyte.MinValue && imm <= byte.MaxValue 
				var opcodesWithImmediateByteSigned = new List<OpCodeInfo>();
				var opcodesOthers = new List<OpCodeInfo>();
				var indices = CollectByOperandKindPredicate(opcodes, IsImmediateByteSigned, opcodesWithImmediateByteSigned, opcodesOthers);
				Debug.Assert(indices.Count == 1);
				var newFlags = group.Flags ^ OpCodeArgFlags.HasImmediateByteSigned;
				return new OpCodeSelector(indices[0], OpCodeSelectorKind.ImmediateByteSigned) {IfTrue = BuildSelectorGraph(group.Signature, newFlags, opcodesWithImmediateByteSigned), IfFalse = BuildSelectorGraph(group.Signature, newFlags, opcodesOthers)};
			}
			else if (group.IsBranch) {
				var branchShort = new List<OpCodeInfo>();
				var branchFar = new List<OpCodeInfo>();
				CollectByOperandKindPredicate(opcodes, IsBranchShort, branchShort, branchFar);
				var newFlags = group.Flags & ~(OpCodeArgFlags.HasBranchShort | OpCodeArgFlags.HasBranchFar);
				return new OpCodeSelector(OpCodeSelectorKind.BranchShort) {IfTrue = BuildSelectorGraph(group.Signature, newFlags, branchShort), IfFalse = BuildSelectorGraph(group.Signature, newFlags, branchFar)};
			}

			return BuildSelectorGraph(group.Signature, group.Flags, opcodes);
		}

		int stackDepth;
		
		OpCodeNode BuildSelectorGraph(Signature signature, OpCodeArgFlags argFlags, List<OpCodeInfo> opcodes) {
			if (opcodes.Count == 0) return default;

			if (opcodes.Count == 1) {
				return new OpCodeNode(opcodes[0]);
			}
			stackDepth++;
			if (stackDepth >= 16) {
				Debug.Assert(false, "Potential StackOverflow");
			}
			try {

				var selectors = FindBestSelectorByRegisterOrMemory(signature, argFlags, opcodes, true);

				if (selectors.Count < opcodes.Count) {
					var memSelectors = FindBestSelectorByRegisterOrMemory(signature, argFlags, opcodes, false);
					if (memSelectors.Count > selectors.Count) {
						selectors = memSelectors;
					}
				}

				// If we have zero or one kind of selectors for all opcodes based on register and/or memory,
				// it means that disambiguation is not done by register or memory but by either:
				// - evex/vex
				// - bitness
				if (selectors.Count <= 1) {
					if ((argFlags & (OpCodeArgFlags.HasVex | OpCodeArgFlags.HasEvex)) == (OpCodeArgFlags.HasVex | OpCodeArgFlags.HasEvex)) {
						Debug.Assert(opcodes.Count == 2);
						var vex = (VexOpCodeInfo)opcodes.First(x => x is VexOpCodeInfo);
						var evex = (EvexOpCodeInfo)opcodes.First(x => x is EvexOpCodeInfo);
						return new OpCodeSelector(OpCodeSelectorKind.Vex) {IfTrue = vex, IfFalse = evex};
					}

					// bitness
					selectors ??= new OrdererSelectorList();
					selectors.Clear();
					bool has64 = false;
					foreach (var opCodeInfo in opcodes) {
						if (opCodeInfo is LegacyOpCodeInfo legacy) {
							int bitness = 64;
							var sizeFlags = legacy.Flags & (OpCodeFlags.Mode16 | OpCodeFlags.Mode32 | OpCodeFlags.Mode64);
							switch (sizeFlags) {
							case OpCodeFlags.Mode16:
								bitness = 16;
								break;
							case OpCodeFlags.Mode16 | OpCodeFlags.Mode32:
								bitness = legacy.OperandSize == OperandSize.None || legacy.OperandSize == OperandSize.Size16 ? 16 : 32;
								break;
							case OpCodeFlags.Mode16 | OpCodeFlags.Mode32 | OpCodeFlags.Mode64:
								bitness = legacy.OperandSize == OperandSize.Size16 ? 16 : 32;
								break;
							
							case OpCodeFlags.Mode64:
								bitness = legacy.OperandSize == OperandSize.Size16 ? 16 : legacy.OperandSize == OperandSize.Size32 ? 32 :  64;
								break;
							default:
								break;
							}

							if (bitness == 64) has64 = true;

							OpCodeSelectorKind selectorKind;
							switch (bitness) {

							case 16:
								selectorKind = OpCodeSelectorKind.Bitness16;
								break;
							case 32:
								selectorKind = OpCodeSelectorKind.Bitness32;
								break;
							default:
							case 64:
								selectorKind = OpCodeSelectorKind.Bitness64;
								break;
							}

							selectors.Add(selectorKind, opCodeInfo);
						}
						else {
							Console.WriteLine($"Unable to detect bitness for opcode {opCodeInfo.Code.RawName}");
							//selectors.Add(OpCodeSelectorKind.Invalid, opCodeInfo);
						}
					}

					if (selectors.Count != opcodes.Count) {
						for (var i = 0; i < opcodes.Count; i++) {
							var opCodeInfo = opcodes[i];
							Console.WriteLine($"Unable to detect bitness for opcode {opCodeInfo.Code.RawName} {(i == 0 ? "(selected by default)" : "")}");
						}
						return new OpCodeNode(opcodes[0]);
					}
				}

				Debug.Assert(selectors != null);

				OpCodeSelector previousSelector = null;
				OpCodeNode rootNode = default;
				foreach (var (kind, list) in selectors) {
					var newSelector = selectors.ArgIndex >= 0 ? new OpCodeSelector(selectors.ArgIndex, kind) : new OpCodeSelector(kind);
					var node = new OpCodeNode(newSelector);
					if (rootNode.IsEmpty) {
						rootNode = node;
					}

					newSelector.IfTrue = list.Count == 1 ? new OpCodeNode(list[0]) : BuildSelectorGraph(signature, argFlags, list);

					if (previousSelector != null) {
						previousSelector.IfFalse = node;
					}

					previousSelector = newSelector;
				}

				return rootNode;
			}
			finally {
				stackDepth--;
			}
		}

		static List<int> CollectByOperandKindPredicate(List<OpCodeInfo> opcodes, Func<OpCodeOperandKind, bool?> predicate, List<OpCodeInfo> opcodesMatchingPredicate, List<OpCodeInfo> opcodesNotMatchingPredicate) {
			var argIndices = new List<int>();
			foreach (var opCodeInfo in opcodes) {
				var selected = opcodesNotMatchingPredicate;
				for (int i = 0; i < opCodeInfo.OpKindsLength; i++)
				{
					var argOpKind = opCodeInfo.OpKind(i);
					var result = predicate(argOpKind);
					if (result.HasValue) {
						if (result.Value) {
							if (!argIndices.Contains(i)) {
								argIndices.Add(i);
							}
							selected = opcodesMatchingPredicate;
						}
						break;
					}
				}
				selected.Add(opCodeInfo);
			}

			return argIndices;
		}

		static bool? IsBranchShort(OpCodeOperandKind kind) {
			switch (kind) {
			case OpCodeOperandKind.br32_4:
			case OpCodeOperandKind.br64_4: 
			case OpCodeOperandKind.xbegin_4:
			case OpCodeOperandKind.brdisp_4:
				return false;
						
			case OpCodeOperandKind.br16_2:
			case OpCodeOperandKind.xbegin_2:
			case OpCodeOperandKind.brdisp_2:
			case OpCodeOperandKind.br16_1:
			case OpCodeOperandKind.br32_1:
			case OpCodeOperandKind.br64_1:
				return true;
			}

			return null;
		}

		static bool? IsImmediateByteSigned(OpCodeOperandKind kind) {
			switch (kind) {
			case OpCodeOperandKind.imm8sex16:
			case OpCodeOperandKind.imm8sex32:
			case OpCodeOperandKind.imm8sex64:
				return true;
			}

			return null;
		}

		static bool? IsImmediateByteEqual1(OpCodeOperandKind kind) {
			switch (kind) {
			case OpCodeOperandKind.imm8_const_1:
				return true;
			}

			return null;
		}

		static OrdererSelectorList FindBestSelectorByRegisterOrMemory(Signature signature, OpCodeArgFlags argFlags, List<OpCodeInfo> opcodes, bool isRegister) {
			List<OrdererSelectorList> selectorsList = null;
			for (int argIndex = 0; argIndex < signature.ArgCount; argIndex++)
			{
				var argKind = signature.GetArgKind(argIndex);
				if (isRegister && (argKind != ArgKind.Register) || !isRegister && (argKind != ArgKind.RegisterMemory && argKind != ArgKind.Memory)) {
					continue;
				}

				var selectors = new OrdererSelectorList() {ArgIndex = argIndex};
				foreach (var opCodeInfo in opcodes)
				{
					var argOpKind = opCodeInfo.OpKind(argIndex);
					var conditionKind = GetSelectorKindForRegisterOrMemory(argOpKind, (argFlags & OpCodeArgFlags.HasRegisterMemoryMappedToRegister) != 0);
					selectors.Add(conditionKind, opCodeInfo);
				}

				// If we have already found the best selector, we can return immediately
				if (selectors.Count == opcodes.Count) {
					return selectors;
				}

				if (selectorsList == null) selectorsList = new List<OrdererSelectorList>();
				selectorsList.Add(selectors);
			}

			if (selectorsList == null) return new OrdererSelectorList(); 

			// Select the biggest selector
			return selectorsList.First(x => x.Count == selectorsList.Max(x => x.Count));
		}

		[DebuggerDisplay("Count = {Count}")]
		private class OrdererSelectorList : List<(OpCodeSelectorKind, List<OpCodeInfo>)> {
			public OrdererSelectorList() {
				ArgIndex = -1;
			}

			public int ArgIndex { get; set; }
			
			public void Add(OpCodeSelectorKind kindToAdd, OpCodeInfo opCodeInfo) {
				foreach (var (kind, list) in this) {
					if (kind == kindToAdd) {
						list.Add(opCodeInfo);
						return;
					}
				}
				Add((kindToAdd, new List<OpCodeInfo>(1) { opCodeInfo }));
			}
		}
		
	
		static int GetPriorityFromKind(OpCodeOperandKind kind) {
			switch (kind) {

			case OpCodeOperandKind.zmm_reg:
			case OpCodeOperandKind.zmm_vvvv:
			case OpCodeOperandKind.zmm_rm:
			case OpCodeOperandKind.zmm_or_mem:
			case OpCodeOperandKind.mem_vsib32z:
			case OpCodeOperandKind.mem_vsib64z:
				return -30;

			case OpCodeOperandKind.ymm_reg:
			case OpCodeOperandKind.ymm_is4:
			case OpCodeOperandKind.ymm_is5:
			case OpCodeOperandKind.ymm_vvvv:
			case OpCodeOperandKind.ymm_rm:
			case OpCodeOperandKind.ymm_or_mem:
			case OpCodeOperandKind.mem128:
			case OpCodeOperandKind.mem_vsib32y:
			case OpCodeOperandKind.mem_vsib64y:
				return -20;
			
			case OpCodeOperandKind.xmm_is4:
			case OpCodeOperandKind.xmm_is5:
			case OpCodeOperandKind.xmm_reg:
			case OpCodeOperandKind.xmm_vvvv:
			case OpCodeOperandKind.xmm_rm:
			case OpCodeOperandKind.xmm_or_mem:
			case OpCodeOperandKind.mem_vsib32x:
			case OpCodeOperandKind.mem_vsib64x:
				return -10;

			case OpCodeOperandKind.rax:
			case OpCodeOperandKind.st0:
				return 0;
			
			case OpCodeOperandKind.r64_opcode:
			case OpCodeOperandKind.r64_vvvv:
			case OpCodeOperandKind.r64_reg:
			case OpCodeOperandKind.r64_or_mem:
			case OpCodeOperandKind.r64_or_mem_mpx:
			case OpCodeOperandKind.imm32sex64:							
			case OpCodeOperandKind.imm64:
			case OpCodeOperandKind.r64_rm:
			case OpCodeOperandKind.mem64:
				return 10;

			case OpCodeOperandKind.eax:
				return 15;
			
			case OpCodeOperandKind.sti_opcode:
			case OpCodeOperandKind.r32_opcode:
			case OpCodeOperandKind.r32_vvvv:
			case OpCodeOperandKind.r32_reg:
			case OpCodeOperandKind.r32_or_mem:
			case OpCodeOperandKind.r32_or_mem_mpx:
			case OpCodeOperandKind.imm32:
			case OpCodeOperandKind.r32_rm:
			case OpCodeOperandKind.mem32:
				return 20;

			case OpCodeOperandKind.ax:
			case OpCodeOperandKind.dx:
				return 25;
				
			case OpCodeOperandKind.r16_opcode:
			case OpCodeOperandKind.r16_reg:
			case OpCodeOperandKind.r16_or_mem:
			case OpCodeOperandKind.imm16: 
			case OpCodeOperandKind.mem16:
				return 30;

			case OpCodeOperandKind.imm8_const_1:
				return 40;

			case OpCodeOperandKind.al:
			case OpCodeOperandKind.cl:
				return 45;
				
			case OpCodeOperandKind.r8_opcode:
			case OpCodeOperandKind.r8_reg:
			case OpCodeOperandKind.r8_or_mem:
			case OpCodeOperandKind.mem:
			case OpCodeOperandKind.imm8:
			case OpCodeOperandKind.imm8sex16:
			case OpCodeOperandKind.imm8sex32:
			case OpCodeOperandKind.imm8sex64:
			case OpCodeOperandKind.mem8:
			case OpCodeOperandKind.mem8_offs:
				return 50;
			
			case OpCodeOperandKind.seg_reg:
				return 60;
			
			default:
				return int.MaxValue;
			}
		}

		[Flags]
		protected enum OpCodeArgFlags {
			Default = 0,
			HasImmediateByteEqual1 = 1,
			HasImmediateByteLessThanBits = 1 << 1,
			HasImmediateByteSigned = 1 << 2,
			HasBranchShort = 1 << 3,
			HasBranchFar = 1 << 4,
			HasVex = 1 << 5,
			HasEvex = 1 << 6,
			HasRegisterMemoryMappedToRegister = 1 << 7,
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

		private ArgKind GetFilterRegisterKindFromOpKind(OpCodeOperandKind opKind, bool allowMemory) {
			switch (opKind) {
			case OpCodeOperandKind.st0:
			case OpCodeOperandKind.sti_opcode:
				return ArgKind.FilterRegisterST;
			
			case OpCodeOperandKind.r8_opcode:
			case OpCodeOperandKind.r8_reg:
				return ArgKind.FilterRegister8;
			case OpCodeOperandKind.r16_opcode:
			case OpCodeOperandKind.r16_reg:
				return ArgKind.FilterRegister16;
			case OpCodeOperandKind.r32_opcode:
			case OpCodeOperandKind.r32_vvvv:
			case OpCodeOperandKind.r32_reg:
			case OpCodeOperandKind.r32_rm:
				return ArgKind.FilterRegister32;
			case OpCodeOperandKind.r64_opcode:
			case OpCodeOperandKind.r64_vvvv:
			case OpCodeOperandKind.r64_reg:
			case OpCodeOperandKind.r64_rm:
				return ArgKind.FilterRegister64;

			case OpCodeOperandKind.imm8_const_1:
				return ArgKind.FilterImmediate1;

			case OpCodeOperandKind.imm2_m2z:
				return ArgKind.FilterImmediate2;

			case OpCodeOperandKind.imm8:
			case OpCodeOperandKind.imm8sex16:
			case OpCodeOperandKind.imm8sex32:
			case OpCodeOperandKind.imm8sex64:
				return ArgKind.FilterImmediate8;
			
			case OpCodeOperandKind.imm16: 
			case OpCodeOperandKind.imm32:
			case OpCodeOperandKind.imm32sex64:							
			case OpCodeOperandKind.imm64:
				return ArgKind.Immediate;
			
			case OpCodeOperandKind.seg_reg:
				return ArgKind.FilterRegisterSegment;
				break;
			
			case OpCodeOperandKind.dx:
				return ArgKind.FilterRegisterDX;
			case OpCodeOperandKind.cl:
				return ArgKind.FilterRegisterCL;
			case OpCodeOperandKind.al:
				return ArgKind.FilterRegisterAL;
			case OpCodeOperandKind.ax:
				return ArgKind.FilterRegisterAX;
			case OpCodeOperandKind.eax:
				return ArgKind.FilterRegisterEAX;
			case OpCodeOperandKind.rax:
				return ArgKind.FilterRegisterRAX;

			case OpCodeOperandKind.es:
				return ArgKind.FilterRegisterES;
			case OpCodeOperandKind.cs:
				return ArgKind.FilterRegisterCS;
			case OpCodeOperandKind.ss:
				return ArgKind.FilterRegisterSS;
			case OpCodeOperandKind.ds:
				return ArgKind.FilterRegisterDS;
			case OpCodeOperandKind.fs:
				return ArgKind.FilterRegisterFS;
			case OpCodeOperandKind.gs:
				return ArgKind.FilterRegisterGS;
			
			case OpCodeOperandKind.bnd_reg:
				return ArgKind.FilterRegisterBND;
			
			case OpCodeOperandKind.cr_reg:
			case OpCodeOperandKind.dr_reg:
			case OpCodeOperandKind.tr_reg:
				return ArgKind.FilterRegisterCDTR;
			
			case OpCodeOperandKind.k_reg:
			case OpCodeOperandKind.k_rm:
			case OpCodeOperandKind.k_vvvv:
				return ArgKind.FilterRegisterK;
			case OpCodeOperandKind.xmm_is4:
			case OpCodeOperandKind.xmm_is5:
			case OpCodeOperandKind.xmm_reg:
			case OpCodeOperandKind.xmm_vvvv:
			case OpCodeOperandKind.xmm_rm:
				return ArgKind.FilterRegisterXmm;
			case OpCodeOperandKind.ymm_is4:
			case OpCodeOperandKind.ymm_is5:
			case OpCodeOperandKind.ymm_reg:
			case OpCodeOperandKind.ymm_vvvv:
			case OpCodeOperandKind.ymm_rm:
				return ArgKind.FilterRegisterYmm;
			case OpCodeOperandKind.zmm_reg:
			case OpCodeOperandKind.zmm_vvvv:
			case OpCodeOperandKind.zmm_rm:
				return ArgKind.FilterRegisterZmm;
			case OpCodeOperandKind.mm_reg:
			case OpCodeOperandKind.mm_rm:
				return ArgKind.FilterRegistermm;
			}

			if (allowMemory) {
				switch (opKind) {
				case OpCodeOperandKind.bnd_or_mem_mpx:
					return ArgKind.FilterRegisterBND;
				case OpCodeOperandKind.r8_or_mem:
					return ArgKind.FilterRegister8;
				case OpCodeOperandKind.r16_or_mem:
					return ArgKind.FilterRegister16;
				case OpCodeOperandKind.r32_or_mem:
				case OpCodeOperandKind.r32_or_mem_mpx:
					return ArgKind.FilterRegister32;
				case OpCodeOperandKind.r64_or_mem:
				case OpCodeOperandKind.r64_or_mem_mpx:
					return ArgKind.FilterRegister64;
			    case OpCodeOperandKind.mm_or_mem:
				    return ArgKind.FilterRegistermm;
				case OpCodeOperandKind.k_or_mem:
					return ArgKind.FilterRegisterK;
				case OpCodeOperandKind.xmm_or_mem:
				case OpCodeOperandKind.mem:
					return ArgKind.FilterRegisterXmm;
				case OpCodeOperandKind.ymm_or_mem:
					return ArgKind.FilterRegisterYmm;
				case OpCodeOperandKind.zmm_or_mem:
					return ArgKind.FilterRegisterZmm;
				}
			}
			
			return ArgKind.Unknown;
		}

		protected static bool IsSegmentRegister(OpCodeOperandKind kind) {
			switch (kind) {
			case OpCodeOperandKind.es:
			case OpCodeOperandKind.cs:
			case OpCodeOperandKind.fs:
			case OpCodeOperandKind.gs:
			case OpCodeOperandKind.ss:
			case OpCodeOperandKind.ds:
				return true;
			}

			return false;
		}
		
		protected static OpCodeSelectorKind GetSelectorKindForRegisterOrMemory(OpCodeOperandKind opKind, bool returnMemoryAsRegister) {

			switch (opKind) {
		
			case OpCodeOperandKind.mem:
			case OpCodeOperandKind.mem_mpx:
				return OpCodeSelectorKind.Memory;
			
			case OpCodeOperandKind.mem8:
			case OpCodeOperandKind.mem_mib:
			case OpCodeOperandKind.mem8_offs:
				return OpCodeSelectorKind.Memory8;
			case OpCodeOperandKind.mem16:
				return OpCodeSelectorKind.Memory16;
			case OpCodeOperandKind.mem32:
				return OpCodeSelectorKind.Memory32;
			case OpCodeOperandKind.mem64:
				return OpCodeSelectorKind.Memory64;
			case OpCodeOperandKind.mem80:
				return OpCodeSelectorKind.Memory80;
			case OpCodeOperandKind.memK:
				return OpCodeSelectorKind.MemoryK;
			
			case OpCodeOperandKind.mem128:
			case OpCodeOperandKind.mem_vsib32x:
			case OpCodeOperandKind.mem_vsib64x:
				return OpCodeSelectorKind.MemoryXMM;

			case OpCodeOperandKind.mem_vsib32y:
			case OpCodeOperandKind.mem_vsib64y:
				return OpCodeSelectorKind.MemoryYMM;

			case OpCodeOperandKind.mem_vsib32z:
			case OpCodeOperandKind.mem_vsib64z:
				return OpCodeSelectorKind.MemoryZMM;
			
			case OpCodeOperandKind.r8_or_mem:
				return returnMemoryAsRegister ? OpCodeSelectorKind.Register8 : OpCodeSelectorKind.Memory8;

			case OpCodeOperandKind.r16_or_mem:
				return returnMemoryAsRegister ? OpCodeSelectorKind.Register16 : OpCodeSelectorKind.Memory16;

			case OpCodeOperandKind.r32_or_mem:
			case OpCodeOperandKind.r32_or_mem_mpx:
				return returnMemoryAsRegister ? OpCodeSelectorKind.Register32 : OpCodeSelectorKind.Memory32;

			case OpCodeOperandKind.r64_or_mem:
			case OpCodeOperandKind.r64_or_mem_mpx:
				return returnMemoryAsRegister ? OpCodeSelectorKind.Register64 : OpCodeSelectorKind.Memory64;

			case OpCodeOperandKind.mm_or_mem:
				return returnMemoryAsRegister ? OpCodeSelectorKind.RegisterMM : OpCodeSelectorKind.MemoryMM;
			
			case OpCodeOperandKind.xmm_or_mem:
				return returnMemoryAsRegister ? OpCodeSelectorKind.RegisterXMM : OpCodeSelectorKind.MemoryXMM;

			case OpCodeOperandKind.ymm_or_mem:
				return returnMemoryAsRegister ? OpCodeSelectorKind.RegisterYMM : OpCodeSelectorKind.MemoryYMM;

			case OpCodeOperandKind.zmm_or_mem:
				return returnMemoryAsRegister ? OpCodeSelectorKind.RegisterZMM : OpCodeSelectorKind.MemoryZMM;

			case OpCodeOperandKind.bnd_or_mem_mpx:
				return returnMemoryAsRegister ? OpCodeSelectorKind.RegisterBND : OpCodeSelectorKind.Memory64;

			case OpCodeOperandKind.k_or_mem:
				return returnMemoryAsRegister ? OpCodeSelectorKind.RegisterK : OpCodeSelectorKind.Memory;
			
			case OpCodeOperandKind.r8_reg:
			case OpCodeOperandKind.r8_opcode:
				return OpCodeSelectorKind.Register8;

			case OpCodeOperandKind.r16_reg:
			case OpCodeOperandKind.r16_rm:
			case OpCodeOperandKind.r16_opcode:
				return OpCodeSelectorKind.Register16;

			case OpCodeOperandKind.r32_reg:
			case OpCodeOperandKind.r32_rm:
			case OpCodeOperandKind.r32_opcode:
			case OpCodeOperandKind.r32_vvvv:
				return OpCodeSelectorKind.Register32;

			case OpCodeOperandKind.r64_reg:
			case OpCodeOperandKind.r64_rm:
			case OpCodeOperandKind.r64_opcode:
			case OpCodeOperandKind.r64_vvvv:
				return OpCodeSelectorKind.Register64;
			
			case OpCodeOperandKind.seg_reg:
				return OpCodeSelectorKind.RegisterSegment;
			
			case OpCodeOperandKind.k_reg:
			case OpCodeOperandKind.kp1_reg:
			case OpCodeOperandKind.k_rm:
			case OpCodeOperandKind.k_vvvv:
				return OpCodeSelectorKind.RegisterK;

			case OpCodeOperandKind.mm_reg:
			case OpCodeOperandKind.mm_rm:
				return OpCodeSelectorKind.RegisterMM;

			case OpCodeOperandKind.xmm_reg:
			case OpCodeOperandKind.xmm_rm:
			case OpCodeOperandKind.xmm_vvvv:
			case OpCodeOperandKind.xmmp3_vvvv:
			case OpCodeOperandKind.xmm_is4:
			case OpCodeOperandKind.xmm_is5:
				return OpCodeSelectorKind.RegisterXMM;

			case OpCodeOperandKind.ymm_reg:
			case OpCodeOperandKind.ymm_rm:
			case OpCodeOperandKind.ymm_vvvv:
			case OpCodeOperandKind.ymm_is4:
			case OpCodeOperandKind.ymm_is5:
				return OpCodeSelectorKind.RegisterYMM;
			
			case OpCodeOperandKind.zmm_reg:
			case OpCodeOperandKind.zmm_rm:
			case OpCodeOperandKind.zmm_vvvv:
			case OpCodeOperandKind.zmmp3_vvvv:
				return OpCodeSelectorKind.RegisterZMM;
			
			case OpCodeOperandKind.cr_reg:
				return OpCodeSelectorKind.RegisterCR;

			case OpCodeOperandKind.dr_reg:
				return OpCodeSelectorKind.RegisterDR;

			case OpCodeOperandKind.tr_reg:
				return OpCodeSelectorKind.RegisterTR;

			case OpCodeOperandKind.bnd_reg:
				return OpCodeSelectorKind.RegisterBND;
			
			case OpCodeOperandKind.es:
				return OpCodeSelectorKind.RegisterES;

			case OpCodeOperandKind.cs:
				return OpCodeSelectorKind.RegisterCS;

			case OpCodeOperandKind.ss:
				return OpCodeSelectorKind.RegisterSS;

			case OpCodeOperandKind.ds:
				return OpCodeSelectorKind.RegisterDS;
			
			case OpCodeOperandKind.fs:
				return OpCodeSelectorKind.RegisterFS;

			case OpCodeOperandKind.gs:
				return OpCodeSelectorKind.RegisterGS;
			
			case OpCodeOperandKind.al:
				return OpCodeSelectorKind.RegisterAL;

			case OpCodeOperandKind.cl:
				return OpCodeSelectorKind.RegisterCL;

			case OpCodeOperandKind.ax:
				return OpCodeSelectorKind.RegisterAX;

			case OpCodeOperandKind.dx:
				return OpCodeSelectorKind.RegisterDX;

			case OpCodeOperandKind.eax:
				return OpCodeSelectorKind.RegisterEAX;
			
			case OpCodeOperandKind.rax:
				return OpCodeSelectorKind.RegisterRAX;
			
			case OpCodeOperandKind.st0:
				return OpCodeSelectorKind.RegisterST0;
			case OpCodeOperandKind.sti_opcode:
				return OpCodeSelectorKind.RegisterST;
			//
			// case OpCodeOperandKind.imm2_m2z:
			// 	return OpCodeConditionKind.ImmediateByteWith2Bits;
			//
			// case OpCodeOperandKind.imm8:
			// 	return OpCodeConditionKind.ImmediateByte;
			//
			// case OpCodeOperandKind.imm8_const_1:
			// 	return OpCodeConditionKind.ImmediateByteEqual1;
			//
			// case OpCodeOperandKind.imm8sex16:
			// case OpCodeOperandKind.imm8sex32:
			// case OpCodeOperandKind.imm8sex64:
			// 	return OpCodeConditionKind.ImmediateByteSigned;
			//
			// case OpCodeOperandKind.imm16:
			// case OpCodeOperandKind.imm32:
			// case OpCodeOperandKind.imm32sex64:
			// case OpCodeOperandKind.imm64:
			// 	return OpCodeConditionKind.ImmediateInt;
			
			case OpCodeOperandKind.seg_rSI:
			case OpCodeOperandKind.es_rDI:
			case OpCodeOperandKind.seg_rDI:
			case OpCodeOperandKind.seg_rBX_al:
				return OpCodeSelectorKind.Memory;
			
			default:
				throw new ArgumentOutOfRangeException(nameof(opKind), opKind, null);
			}
		}

		OpCodeInfoGroup AddOpCodeToGroup(string name, Signature signature, OpCodeInfo code, OpCodeArgFlags opCodeArgFlags) {
			var key = new GroupKey(name, signature);
			if (!_groups.TryGetValue(key, out var group)) {
				group = new OpCodeInfoGroup(name, signature);
				_groups.Add(key, group);
			}
			
			group.Items.Add(code);
			group.Flags |= opCodeArgFlags;

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
			ImmediateByte,
			Branch,
			
			FilterRegisterCDTR,
			
			FilterRegisterK,
			
			FilterRegisterST,

			FilterRegister8,
			FilterRegister16,
			FilterRegister32,
			FilterRegister64,
		
			FilterRegisterSegment,

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
			
			FilterRegisterBND,

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
				MaxArgSizes = new List<int>();
			}
			
			public string MemoName { get; }
			
			public string Name { get; }
			
			public OpCodeArgFlags Flags { get; set; }
			
			public bool IsBranch => (Flags & (OpCodeArgFlags.HasBranchShort | OpCodeArgFlags.HasBranchFar)) != 0;

			public bool HasRegisterMemoryMappedToRegister => (Flags & OpCodeArgFlags.HasRegisterMemoryMappedToRegister) != 0;
			
			public bool HasVexAndEvex => (Flags & (OpCodeArgFlags.HasVex | OpCodeArgFlags.HasEvex)) == (OpCodeArgFlags.HasVex | OpCodeArgFlags.HasEvex);

			public Signature Signature { get; }
			
			public OpCodeNode RootOpCodeNode { get; set; }
			
			public int MaxImmediateSize { get; set; }

			public List<OpCodeInfo> Items { get; }
			
			public List<int> MaxArgSizes { get; }

			public void UpdateMaxArgSizes(List<int> argSizes) {
				if (MaxArgSizes.Count == 0) {
					MaxArgSizes.AddRange(argSizes);
				}
				Debug.Assert(MaxArgSizes.Count == argSizes.Count);
				for (int i = 0; i < MaxArgSizes.Count; i++) {
					if (MaxArgSizes[i] < argSizes[i]) {
						MaxArgSizes[i] = argSizes[i];
					}
				}
			}
			
			public int OrderOpCodesPerOpKindPriority(OpCodeInfo x, OpCodeInfo y) {
				Debug.Assert(x.OpKindsLength == y.OpKindsLength);
				for (int i = 0; i < x.OpKindsLength; i++) {
					if (Signature.GetArgKind(i) != ArgKind.Register) continue;  
					var result = GetPriorityFromKind(x.OpKind(i)).CompareTo(GetPriorityFromKind(y.OpKind(i)));
					if (result != 0) return result;
				}

				for (int i = 0; i < x.OpKindsLength; i++) {
					if (Signature.GetArgKind(i) == ArgKind.Register) continue;  
					var result = GetPriorityFromKind(x.OpKind(i)).CompareTo(GetPriorityFromKind(y.OpKind(i)));
					if (result != 0) return result;
				}
				
				// Case for ordering by decreasing bitness
				if (x is LegacyOpCodeInfo x1 && y is LegacyOpCodeInfo y1) {
					var result = (x1.AddressSize == AddressSize.None ? AddressSize.Size64 : x1.AddressSize).CompareTo((y1.AddressSize == AddressSize.None ? AddressSize.Size64 : y1.AddressSize));
					if (result != 0) return -result;
				
					result = (x1.OperandSize == OperandSize.None ? OperandSize.Size64 : x1.OperandSize).CompareTo((y1.OperandSize == OperandSize.None ? OperandSize.Size64 : y1.OperandSize));
					if (result != 0) return -result;
				}
			
				return 0;
			}

			
		}

		protected readonly struct OpCodeNode {
			readonly object _value;

			public OpCodeNode(OpCodeInfo opCodeInfo) {
				_value = opCodeInfo;
			}

			public OpCodeNode(OpCodeSelector selector) {
				_value = selector;
			}

			public bool IsEmpty => _value == null;

			public OpCodeInfo OpCodeInfo => _value as OpCodeInfo;
			
			public OpCodeSelector Selector => _value as OpCodeSelector;

			public static implicit operator OpCodeNode(OpCodeInfo opCodeInfo) => new OpCodeNode(opCodeInfo);

			public static implicit operator OpCodeNode(OpCodeSelector selector) => new OpCodeNode(selector);
		}

		protected class OpCodeSelector {
			public OpCodeSelector(OpCodeSelectorKind kind) {
				ArgIndex = -1;
				Kind = kind;	
			}

			public OpCodeSelector(int argIndex, OpCodeSelectorKind kind) {
				ArgIndex = argIndex;
				Kind = kind;
			}

			public readonly int ArgIndex;

			public readonly OpCodeSelectorKind Kind;
			
			public OpCodeNode IfTrue;

			public OpCodeNode IfFalse;

			public bool IsConditionInlineable => IfTrue.OpCodeInfo != null && IfFalse.OpCodeInfo != null;
		}
		
		protected enum OpCodeSelectorKind {
			Invalid,
			
			Bitness64,
			Bitness32,
			Bitness16,
			
			BranchShort,
			
			ImmediateInt,
			ImmediateByte,
			ImmediateByteEqual1,
			ImmediateByteSigned,
			ImmediateByteWith2Bits,

			Vex,
			
			RegisterCL,
			RegisterAL,
			RegisterAX,
			RegisterEAX,
			RegisterRAX,

			RegisterBND,

			RegisterES,
			RegisterCS,
			RegisterSS,
			RegisterDS,
			RegisterFS,
			RegisterGS,
			RegisterDX,
			
			Register8,
			Register16,
			Register32,
			Register64,
			
			RegisterK,
			
			RegisterST0,
			RegisterST,
			
			RegisterSegment,

			RegisterCR,
			RegisterDR,
			RegisterTR,
			
			RegisterMM,
			RegisterXMM,
			RegisterYMM,
			RegisterZMM,

			Memory,

			Memory8,
			Memory16,
			Memory32,
			Memory64,
			Memory80,
		
			MemoryK,
			
			MemoryMM,
			MemoryXMM,
			MemoryYMM,
			MemoryZMM,
		}
	}
}
