/*
Copyright (C) 2018-2019 de4dot@gmail.com

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System;
using System.Collections.Generic;
using Iced.Intel;

namespace IcedFuzzer.Core {
	public enum CpuDecoder {
		Intel,
		AMD,
	}

	public sealed class FilterOptions {
		public bool FilterEnabled => IncludeCpuid.Count != 0 || ExcludeCpuid.Count != 0 || IncludeCode.Count != 0 || ExcludeCode.Count != 0;
		public readonly HashSet<CpuidFeature> IncludeCpuid = new HashSet<CpuidFeature>();
		public readonly HashSet<CpuidFeature> ExcludeCpuid = new HashSet<CpuidFeature>();
		public readonly HashSet<Code> IncludeCode = new HashSet<Code>();
		public readonly HashSet<Code> ExcludeCode = new HashSet<Code>();

		public bool WasRemoved(CpuidFeature cpu) {
			if (IncludeCpuid.Count != 0)
				return !IncludeCpuid.Contains(cpu);
			return ExcludeCpuid.Contains(cpu);
		}

		public bool ShouldInclude(Code code, bool? isMemOp) {
			if (ExcludeCpuid.Count != 0) {
				if (CodeUtils.IsSpecialAvxAvx2(code)) {
					// AVX (reg,mem) or AVX2 (reg,reg).
					if (isMemOp is null) {
						// Remove the instruction if both AVX and AVX2 should be excluded.
						if (ExcludeCpuid.Contains(CpuidFeature.AVX) && ExcludeCpuid.Contains(CpuidFeature.AVX2))
							return false;
					}
					else if (isMemOp.GetValueOrDefault()) {
						if (ExcludeCpuid.Contains(CpuidFeature.AVX))
							return false;
					}
					else {
						if (ExcludeCpuid.Contains(CpuidFeature.AVX2))
							return false;
					}
				}
				else {
					foreach (var cpuid in code.CpuidFeatures()) {
						if (ExcludeCpuid.Contains(cpuid))
							return false;
					}
				}
			}
			if (IncludeCpuid.Count != 0) {
				bool include = false;
				foreach (var cpuid in code.CpuidFeatures()) {
					if (IncludeCpuid.Contains(cpuid)) {
						include = true;
						break;
					}
				}
				if (!include)
					return false;
			}
			if (IncludeCode.Count != 0 && !IncludeCode.Contains(code))
				return false;
			if (ExcludeCode.Contains(code))
				return false;

			return true;
		}
	}

	public sealed class OpCodeInfoOptions {
		public int Bitness;
		public CpuDecoder CpuDecoder = CpuDecoder.Intel;
		public bool IncludeVEX = true;
		public bool IncludeXOP = true;
		public bool IncludeEVEX = true;
		public bool Include3DNow = true;
		public readonly FilterOptions Filter = new FilterOptions();
	}

	public static class OpCodeInfoProvider {
		public static OpCodeInfo[] GetOpCodeInfos(OpCodeInfoOptions options) {
			Assert.True(options.Bitness == 16 || options.Bitness == 32 || options.Bitness == 64, $"Invalid bitness: {options.Bitness}");
			var result = new List<OpCodeInfo>();
			foreach (var code in (Code[])Enum.GetValues(typeof(Code))) {
				var opCode = code.ToOpCode();
				if (!opCode.IsAvailableInMode(options.Bitness))
					continue;
				// INVALID, db, dw, dd, dq
				if (!opCode.IsInstruction)
					continue;
				// These require a WAIT instruction, so ignore them. The no-wait instructions (eg. fnstenv) are tested
				if (opCode.Fwait)
					continue;

				if (!options.Filter.ShouldInclude(code, null))
					continue;

				switch (opCode.Encoding) {
				case EncodingKind.Legacy:
					break;
				case EncodingKind.VEX:
					if (!options.IncludeVEX)
						continue;
					break;
				case EncodingKind.EVEX:
					if (!options.IncludeEVEX)
						continue;
					break;
				case EncodingKind.XOP:
					if (!options.IncludeXOP)
						continue;
					break;
				case EncodingKind.D3NOW:
					if (!options.Include3DNow)
						continue;
					break;
				default:
					throw ThrowHelpers.Unreachable;
				}

				switch (code) {
				// DecoderOptions.OldFpu
				case Code.Frstpm:
				case Code.Fstdw_AX:
				case Code.Fstsg_AX:
					continue;
				// DecoderOptions.Jmpe
				case Code.Jmpe_rm16:
				case Code.Jmpe_rm32:
				case Code.Jmpe_disp16:
				case Code.Jmpe_disp32:
					continue;
				// DecoderOptions.Pcommit
				case Code.Pcommit:
					continue;
				// DecoderOptions.Loadall286
				case Code.Loadallreset286:
				case Code.Loadall286:
					continue;
				// DecoderOptions.Loadall386
				case Code.Loadall386:
					continue;
				// DecoderOptions.Cl1invmb
				case Code.Cl1invmb:
					continue;
				// DecoderOptions.Umov
				case Code.Umov_rm8_r8:
				case Code.Umov_rm16_r16:
				case Code.Umov_rm32_r32:
				case Code.Umov_r8_rm8:
				case Code.Umov_r16_rm16:
				case Code.Umov_r32_rm32:
					continue;
				// DecoderOptions.MovTr
				case Code.Mov_r32_tr:
				case Code.Mov_tr_r32:
					continue;
				// DecoderOptions.Xbts
				case Code.Xbts_r16_rm16:
				case Code.Xbts_r32_rm32:
				case Code.Ibts_rm16_r16:
				case Code.Ibts_rm32_r32:
					continue;
				// DecoderOptions.Cmpxchg486A
				case Code.Cmpxchg486_rm8_r8:
				case Code.Cmpxchg486_rm16_r16:
				case Code.Cmpxchg486_rm32_r32:
					continue;

				// 8086 only
				case Code.Popw_CS:
					continue;

				// AMD only
				case Code.Ud0:
					if (options.CpuDecoder != CpuDecoder.AMD)
						continue;
					break;
				// Intel only
				case Code.Ud0_r16_rm16:
				case Code.Ud0_r32_rm32:
				case Code.Ud0_r64_rm64:
					if (options.CpuDecoder == CpuDecoder.AMD)
						continue;
					break;

				case Code.Call_rm16:
				case Code.Jmp_rm16:
				case Code.Jo_rel8_16:
				case Code.Jno_rel8_16:
				case Code.Jb_rel8_16:
				case Code.Jae_rel8_16:
				case Code.Je_rel8_16:
				case Code.Jne_rel8_16:
				case Code.Jbe_rel8_16:
				case Code.Ja_rel8_16:
				case Code.Js_rel8_16:
				case Code.Jns_rel8_16:
				case Code.Jp_rel8_16:
				case Code.Jnp_rel8_16:
				case Code.Jl_rel8_16:
				case Code.Jge_rel8_16:
				case Code.Jle_rel8_16:
				case Code.Jg_rel8_16:
				case Code.Jmp_rel8_16:
				case Code.Jo_rel16:
				case Code.Jno_rel16:
				case Code.Jb_rel16:
				case Code.Jae_rel16:
				case Code.Je_rel16:
				case Code.Jne_rel16:
				case Code.Jbe_rel16:
				case Code.Ja_rel16:
				case Code.Js_rel16:
				case Code.Jns_rel16:
				case Code.Jp_rel16:
				case Code.Jnp_rel16:
				case Code.Jl_rel16:
				case Code.Jge_rel16:
				case Code.Jle_rel16:
				case Code.Jg_rel16:
				case Code.Call_rel16:
				case Code.Jmp_rel16:
				case Code.Loopne_rel8_16_ECX:
				case Code.Loopne_rel8_16_RCX:
				case Code.Loope_rel8_16_ECX:
				case Code.Loope_rel8_16_RCX:
				case Code.Loop_rel8_16_ECX:
				case Code.Loop_rel8_16_RCX:
				case Code.Jecxz_rel8_16:
				case Code.Jrcxz_rel8_16:
				case Code.Retnw_imm16:
				case Code.Retnw:
					if (options.Bitness == 64) {
						// Only supported by AMD in 64-bit mode
						if (options.CpuDecoder != CpuDecoder.AMD)
							continue;
					}
					break;

				case Code.Call_m1664:
				case Code.Jmp_m1664:
				case Code.Lss_r64_m1664:
				case Code.Lfs_r64_m1664:
				case Code.Lgs_r64_m1664:
					if (options.Bitness == 64) {
						// Only supported by Intel in 64-bit mode
						if (options.CpuDecoder != CpuDecoder.Intel)
							continue;
					}
					break;
				}

				result.Add(opCode);
			}
			return result.ToArray();
		}
	}
}
