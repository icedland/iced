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
				foreach (var cpuid in code.CpuidFeatures()) {
					if (ExcludeCpuid.Contains(cpuid))
						return false;
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

				var decOpt = opCode.DecoderOption;
				if (decOpt != DecoderOptions.None && decOpt != DecoderOptions.MPX)
					continue;
				// 8086 only
				if (code == Code.Popw_CS)
					continue;

				switch (options.CpuDecoder) {
				case CpuDecoder.Intel:
					switch (options.Bitness) {
					case 16:
						if (!opCode.IntelDecoder16)
							continue;
						break;
					case 32:
						if (!opCode.IntelDecoder32)
							continue;
						break;
					case 64:
						if (!opCode.IntelDecoder64)
							continue;
						break;
					default:
						throw ThrowHelpers.Unreachable;
					}
					break;
				case CpuDecoder.AMD:
					switch (options.Bitness) {
					case 16:
						if (!opCode.AmdDecoder16)
							continue;
						break;
					case 32:
						if (!opCode.AmdDecoder32)
							continue;
						break;
					case 64:
						if (!opCode.AmdDecoder64)
							continue;
						break;
					default:
						throw ThrowHelpers.Unreachable;
					}
					break;
				default:
					throw ThrowHelpers.Unreachable;
				}

				result.Add(opCode);
			}
			return result.ToArray();
		}
	}
}
