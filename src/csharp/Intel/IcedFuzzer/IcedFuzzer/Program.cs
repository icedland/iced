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
using System.Diagnostics;
using System.IO;
using System.Linq;
using IcedFuzzer.Core;
using Iced.Intel;

namespace IcedFuzzer {
	sealed class Options {
		public readonly OpCodeInfoOptions OpCodeInfoOptions = new OpCodeInfoOptions();
		public bool PrintInstructions = false;
		public bool UnusedTables = true;
		public bool Quiet = false;
		public bool IncludeValidInstructions = true;
		public bool IncludeInvalidInstructions = true;
		public bool IncludeValidCodeValue = false;
		public readonly FilterOptions Filter = new FilterOptions();
		public string? InvalidFilename = null;
		public string? ValidFilename = null;
		public bool IncludeValidByteLength = false;
	}

	sealed class CommandLineParserException : Exception {
		public CommandLineParserException(string message) : base(message) { }
	}

	static class Program {
		static int Main(string[] args) {
			try {
				var options = ParseOptions(args);
				var infos = GetOpCodeInfos(options);
				if (options.PrintInstructions) {
					Array.Sort(infos, (a, b) => {
						int c = StringComparer.OrdinalIgnoreCase.Compare(a.Code.Mnemonic().ToString(), b.Code.Mnemonic().ToString());
						if (c != 0)
							return c;
						return a.Code.CompareTo(b.Code);
					});
					const int MaxCpuids = 2;
					Console.Write("Instruction\tOpCode\tCode");
					for (int i = 0; i < MaxCpuids; i++)
						Console.Write($"\tCPUID{i + 1}");
					Console.WriteLine();
					foreach (var opCode in infos) {
						var cpuidFeatures = opCode.Code.CpuidFeatures().Select(a => a.ToString()).OrderBy(a => a, StringComparer.OrdinalIgnoreCase).ToArray();
						if (cpuidFeatures.Length == 0 || cpuidFeatures.Length > MaxCpuids)
							throw new InvalidOperationException();
						Console.Write($"{opCode.ToInstructionString()}\t{opCode.ToOpCodeString()}\t{opCode.Code}");
						for (int i = 0; i < MaxCpuids; i++) {
							var cpuid = i < cpuidFeatures.Length ? cpuidFeatures[i] : string.Empty;
							Console.Write($"\t{cpuid}");
						}
						Console.WriteLine();
					}
					return 0;
				}
				else {
					if (!options.Quiet)
						Console.WriteLine($"Bitness: {options.OpCodeInfoOptions.Bitness}");
					var info = Gen(options, infos);
					if (!options.Quiet) {
						Console.WriteLine($"valid  : {info.totValid} instrs: {info.totBytesValid} bytes or {(double)info.totBytesValid / 1024:0.##} kB or {(double)info.totBytesValid / 1024 / 1024:0.##} MB");
						Console.WriteLine($"invalid: {info.totInvalid} instrs: {info.totBytesInvalid} bytes or {(double)info.totBytesInvalid / 1024:0.##} kB or {(double)info.totBytesInvalid / 1024 / 1024:0.##} MB");
					}
				}
				return 0;
			}
			catch (CommandLineParserException ex) {
				PrintHelp();
				if (ex.Message != string.Empty) {
					Console.WriteLine();
					Console.WriteLine($"*** {ex.Message}");
					return 1;
				}
				else
					return 0;
			}
			catch (Exception ex) {
				Console.WriteLine(ex);
				return 1;
			}
		}

		static OpCodeInfo[] GetOpCodeInfos(Options options) =>
			OpCodeInfoProvider.GetOpCodeInfos(options.OpCodeInfoOptions);

		static (uint totValid, ulong totBytesValid, uint totInvalid, ulong totBytesInvalid) Gen(Options options, OpCodeInfo[] infos) {
			var genFlags = InstrGenFlags.None;
			if (options.UnusedTables)
				genFlags |= InstrGenFlags.UnusedTables;
			if (!options.OpCodeInfoOptions.IncludeVEX)
				genFlags |= InstrGenFlags.NoVEX;
			if (!options.OpCodeInfoOptions.IncludeXOP)
				genFlags |= InstrGenFlags.NoXOP;
			if (!options.OpCodeInfoOptions.IncludeEVEX)
				genFlags |= InstrGenFlags.NoEVEX;
			if (!options.OpCodeInfoOptions.Include3DNow)
				genFlags |= InstrGenFlags.No3DNow;
			if (options.OpCodeInfoOptions.Filter.WasRemoved(CpuidFeature.AVX))
				genFlags |= InstrGenFlags.NoAVX;
			if (options.OpCodeInfoOptions.Filter.WasRemoved(CpuidFeature.AVX2))
				genFlags |= InstrGenFlags.NoAVX2;
			var encodingTables = InstrGen.Create(options.OpCodeInfoOptions.Bitness, infos, genFlags);

			var instructions = encodingTables.GetOpCodeGroups().SelectMany(a => a.opCodes).SelectMany(a => a.Instructions).Where(instr => {
				if (!options.Filter.ShouldInclude(instr.Code, instr.IsModrmMemory))
					return false;
				if (instr.Code == Code.INVALID)
					return options.IncludeInvalidInstructions;
				return options.IncludeValidInstructions;
			}).ToArray();

			FileStream? validStream = null;
			FileStream? invalidStream = null;
			BinaryWriter? validWriter = null;
			BinaryWriter? invalidWriter = null;
			var data2 = new byte[2];
			try {
				if (options.ValidFilename is object)
					CreateFile(ref validStream, ref validWriter, options.ValidFilename);
				if (options.InvalidFilename is object)
					CreateFile(ref invalidStream, ref invalidWriter, options.InvalidFilename);

				var fuzzerOptions = FuzzerOptions.NoPAUSE | FuzzerOptions.NoWBNOINVD |
					FuzzerOptions.NoTZCNT | FuzzerOptions.NoLZCNT;
				if (options.OpCodeInfoOptions.Filter.FilterEnabled || options.Filter.FilterEnabled)
					fuzzerOptions |= FuzzerOptions.NoVerifyInstrs;
				if (!options.OpCodeInfoOptions.Filter.WasRemoved(CpuidFeature.MPX))
					fuzzerOptions |= FuzzerOptions.HasMPX;
				foreach (var instr in instructions) {
					switch (instr.Code) {
					case Code.Pause:
						fuzzerOptions &= ~FuzzerOptions.NoPAUSE;
						break;
					case Code.Wbnoinvd:
						fuzzerOptions &= ~FuzzerOptions.NoWBNOINVD;
						break;
					case Code.Tzcnt_r16_rm16:
					case Code.Tzcnt_r32_rm32:
					case Code.Tzcnt_r64_rm64:
						fuzzerOptions &= ~FuzzerOptions.NoTZCNT;
						break;
					case Code.Lzcnt_r16_rm16:
					case Code.Lzcnt_r32_rm32:
					case Code.Lzcnt_r64_rm64:
						fuzzerOptions &= ~FuzzerOptions.NoLZCNT;
						break;
					}
				}

				var fuzzer = new Fuzzer(options.OpCodeInfoOptions.Bitness, fuzzerOptions, options.OpCodeInfoOptions.CpuDecoder);
				ulong totBytesValid = 0;
				ulong totBytesInvalid = 0;
				uint totValid = 0;
				uint totInvalid = 0;
				foreach (var info in fuzzer.GetInstructions(instructions)) {
					BinaryWriter? writer;
					bool writeLength;
					bool writeCodeValue;
					if (info.Invalid) {
						totBytesInvalid += (uint)info.EncodedDataLength;
						totInvalid++;
						writer = invalidWriter;
						writeLength = true;
						writeCodeValue = false;
					}
					else {
						totBytesValid += (uint)info.EncodedDataLength;
						totValid++;
						writer = validWriter;
						writeLength = options.IncludeValidByteLength;
						writeCodeValue = options.IncludeValidCodeValue;
					}
					if (writer is object) {
						if (writeCodeValue) {
							uint code = (uint)info.Instruction.Code;
							if (code > ushort.MaxValue)
								throw new InvalidOperationException();
							data2[0] = (byte)code;
							data2[1] = (byte)(code >> 8);
							writer.Write(data2, 0, 2);
						}
						if (writeLength) {
							data2[0] = (byte)info.EncodedDataLength;
							writer.Write(data2, 0, 1);
						}
						writer.Write(info.EncodedData, 0, info.EncodedDataLength);
					}
				}
				return (totValid, totBytesValid, totInvalid, totBytesInvalid);
			}
			finally {
				validWriter?.Dispose();
				invalidWriter?.Dispose();
				validStream?.Dispose();
				invalidStream?.Dispose();
			}
		}

		static void CreateFile(ref FileStream? stream, ref BinaryWriter? writer, string filename) {
			if (stream is object || writer is object)
				throw new InvalidOperationException();
			stream = File.Open(filename, FileMode.Create, FileAccess.Write, FileShare.Read | FileShare.Delete);
			writer = new BinaryWriter(stream);
		}

		static void PrintHelp() {
			var filename = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);
			var sep = seps[0];
			Console.WriteLine($"{filename} <-16|-32|-64> [other options]");
			Console.WriteLine();
			Console.WriteLine($"-h / --help               Show this text");
			Console.WriteLine($"--quiet                   Print less");
			Console.WriteLine($"--show-instructions       Show selected instructions, their Code/CPUID feature names and exit");
			Console.WriteLine();
			Console.WriteLine($"Bitness (required):");
			Console.WriteLine();
			Console.WriteLine($"-16                       Generate 16-bit code");
			Console.WriteLine($"-32                       Generate 32-bit code");
			Console.WriteLine($"-64                       Generate 64-bit code");
			Console.WriteLine();
			Console.WriteLine($"Decoder:");
			Console.WriteLine();
			Console.WriteLine($"--intel                   Intel decoder (default)");
			Console.WriteLine($"--amd                     AMD decoder");
			Console.WriteLine();
			Console.WriteLine($"Add/remove valid instructions:");
			Console.WriteLine();
			Console.WriteLine($"--no-vex                  No VEX instructions");
			Console.WriteLine($"--no-xop                  No XOP instructions");
			Console.WriteLine($"--no-evex                 No EVEX instructions");
			Console.WriteLine($"--no-3dnow                No 3DNow! instructions (implies --no-geode)");
			Console.WriteLine($"--no-geode                No AMD Geode LX/GX 3DNow! instructions");
			Console.WriteLine($"--no-via                  No VIA instructions");
			Console.WriteLine($"--no-unused-tables        Don't gen unused VEX/EVEX/XOP opcode tables (eg. EVEX.mm=00). Smaller output files.");
			Console.WriteLine($"--include-cpuid names     Only include instructions with these CPUID names, {sep}-separated");
			Console.WriteLine($"--exclude-cpuid names     Exclude instructions with these CPUID names, {sep}-separated");
			Console.WriteLine($"--include-code names      Only include instructions with these Code names, {sep}-separated");
			Console.WriteLine($"--exclude-code names      Exclude instructions with these Code names, {sep}-separated");
			Console.WriteLine();
			Console.WriteLine($"Only generate some of the instructions:");
			Console.WriteLine();
			Console.WriteLine($"--no-invalid-instr        Filter out all invalid instructions");
			Console.WriteLine($"--no-valid-instr          Filter out all valid instructions");
			Console.WriteLine($"--gen-include-cpuid names Gen: Only include instructions with these CPUID names, {sep}-separated");
			Console.WriteLine($"--gen-exclude-cpuid names Gen: Exclude instructions with these CPUID names, {sep}-separated");
			Console.WriteLine($"--gen-include-code names  Gen: Only include instructions with these Code names, {sep}-separated");
			Console.WriteLine($"--gen-exclude-code names  Gen: Exclude instructions with these Code names, {sep}-separated");
			Console.WriteLine();
			Console.WriteLine($"Output files:");
			Console.WriteLine();
			Console.WriteLine($"-oil filename             Invalid bytes filename with instr length bytes");
			Console.WriteLine($"-ovl filename             Valid bytes filename with instr length bytes");
			Console.WriteLine($"-ovlc filename            Valid bytes filename with instr length bytes and Code value");
			Console.WriteLine($"-ov filename              Valid bytes filename without instr length bytes");
			Console.WriteLine();
			Console.WriteLine($"At least one filename is required.");
			Console.WriteLine();
			Console.WriteLine($"-oil / -ovl format: <byte length> <length instruction bytes> ...");
			Console.WriteLine($"-ovlc format: <2-byte Code value LE> <byte length> <length instruction bytes> ...");
			Console.WriteLine($"-ov format: <all instruction bytes>");
			Console.WriteLine();
			Console.WriteLine($"--{{in,ex}}clude-{{cpuid,code}} vs --gen-{{in,ex}}clude-{{cpuid,code}}:");
			Console.WriteLine();
			Console.WriteLine($"--{{in,ex}}clude-{{cpuid,code}} tells the fuzzer which instructions exist.");
			Console.WriteLine($"--gen-{{in,ex}}clude-{{cpuid,code}} tells the fuzzer which instrs of those that exist that should be gen'd.");
			Console.WriteLine();
			Console.WriteLine($"CPUID: See CpuidFeatures enum or use --show-instructions");
			Console.WriteLine($"Code: See Code enum or use --show-instructions");
			Console.WriteLine();
			Console.WriteLine($"Examples:");
			Console.WriteLine();
			Console.WriteLine($@"{filename} -64 --show-instructions");
			Console.WriteLine($@"{filename} -32 --show-instructions --include-cpuid ""intel8086;intel186;intel286;intel386;intel486""");
			Console.WriteLine($@"{filename} -64 -oil invalid.bin -ov valid.bin");
			Console.WriteLine($@"{filename} -32 -oil invalid.bin -ov valid.bin --include-cpuid ""intel8086;intel186;intel286;intel386;intel486""");
			Console.WriteLine($@"{filename} -64 -oil invalid.bin -ov valid.bin --gen-include-cpuid ""avx;avx2""");
		}

		static Options ParseOptions(string[] args) {
			var options = new Options();
			var toCpuid = ((CpuidFeature[])Enum.GetValues(typeof(CpuidFeature))).ToDictionary(a => a.ToString(), a => a, StringComparer.OrdinalIgnoreCase);
			var toCode = ((Code[])Enum.GetValues(typeof(Code))).ToDictionary(a => a.ToString(), a => a, StringComparer.OrdinalIgnoreCase);

			for (int i = 0; i < args.Length; i++) {
				var arg = args[i];
				var next = i + 1 < args.Length ? args[i + 1] : null;
				switch (arg) {
				case "-h":
				case "--help":
					throw new CommandLineParserException(string.Empty);

				case "--quiet":
					options.Quiet = true;
					break;

				case "-16":
					SetBitness(options, 16);
					break;

				case "-32":
					SetBitness(options, 32);
					break;

				case "-64":
					SetBitness(options, 64);
					break;

				case "--show-instructions":
					options.PrintInstructions = true;
					break;

				case "--intel":
					options.OpCodeInfoOptions.CpuDecoder = CpuDecoder.Intel;
					break;

				case "--amd":
					options.OpCodeInfoOptions.CpuDecoder = CpuDecoder.AMD;
					break;

				case "--no-vex":
					options.OpCodeInfoOptions.IncludeVEX = false;
					break;

				case "--no-xop":
					options.OpCodeInfoOptions.IncludeXOP = false;
					break;

				case "--no-evex":
					options.OpCodeInfoOptions.IncludeEVEX = false;
					break;

				case "--no-3dnow":
					options.OpCodeInfoOptions.Include3DNow = false;
					options.OpCodeInfoOptions.Filter.ExcludeCode.Add(Code.Femms);
					options.OpCodeInfoOptions.Filter.ExcludeCpuid.Add(CpuidFeature.GEODE);
					break;

				case "--no-geode":
					options.OpCodeInfoOptions.Filter.ExcludeCpuid.Add(CpuidFeature.GEODE);
					break;

				case "--no-via":
					options.OpCodeInfoOptions.Filter.ExcludeCpuid.Add(CpuidFeature.PADLOCK_ACE);
					options.OpCodeInfoOptions.Filter.ExcludeCpuid.Add(CpuidFeature.PADLOCK_PHE);
					options.OpCodeInfoOptions.Filter.ExcludeCpuid.Add(CpuidFeature.PADLOCK_PMM);
					options.OpCodeInfoOptions.Filter.ExcludeCpuid.Add(CpuidFeature.PADLOCK_RNG);
					break;

				case "--no-unused-tables":
					options.UnusedTables = false;
					break;

				case "--include-cpuid":
					AddCpuid(options.OpCodeInfoOptions.Filter.IncludeCpuid, toCpuid, next);
					i++;
					break;

				case "--exclude-cpuid":
					AddCpuid(options.OpCodeInfoOptions.Filter.ExcludeCpuid, toCpuid, next);
					i++;
					break;

				case "--include-code":
					AddCode(options.OpCodeInfoOptions.Filter.IncludeCode, toCode, next);
					i++;
					break;

				case "--exclude-code":
					AddCode(options.OpCodeInfoOptions.Filter.ExcludeCode, toCode, next);
					i++;
					break;

				case "--gen-include-cpuid":
					AddCpuid(options.Filter.IncludeCpuid, toCpuid, next);
					i++;
					break;

				case "--gen-exclude-cpuid":
					AddCpuid(options.Filter.ExcludeCpuid, toCpuid, next);
					i++;
					break;

				case "--gen-include-code":
					AddCode(options.Filter.IncludeCode, toCode, next);
					i++;
					break;

				case "--gen-exclude-code":
					AddCode(options.Filter.ExcludeCode, toCode, next);
					i++;
					break;

				case "--no-invalid-instr":
					options.IncludeInvalidInstructions = false;
					break;

				case "--no-valid-instr":
					options.IncludeValidInstructions = false;
					break;

				case "-oil":
					if (next is null)
						throw new CommandLineParserException("Missing filename");
					if (options.ValidFilename is object)
						throw new CommandLineParserException("Can't use -oil twice");
					options.InvalidFilename = next;
					i++;
					break;

				case "-ov":
					AddValidFilename(options, next, false);
					i++;
					break;

				case "-ovl":
					AddValidFilename(options, next, true);
					i++;
					break;

				case "-ovlc":
					AddValidFilename(options, next, true);
					options.IncludeValidCodeValue = true;
					i++;
					break;

				default:
					throw new CommandLineParserException($"Unknown option {arg}");
				}
			}

			if (options.OpCodeInfoOptions.Bitness == 0)
				throw new CommandLineParserException("Missing bitness: -16 -32 or -64");
			if (!options.PrintInstructions) {
				if (options.ValidFilename is null && options.InvalidFilename is null)
					throw new CommandLineParserException("At least one of -oil, -ovl, -ovlc and -ov must be used");
			}
			return options;
		}

		static void AddValidFilename(Options options, string? filename, bool includeByteLength) {
			if (filename is null)
				throw new CommandLineParserException("Missing filename");
			if (options.ValidFilename is object)
				throw new CommandLineParserException("Can't use -ov, -ovl or -ovlc twice");
			options.ValidFilename = filename;
			options.IncludeValidByteLength = includeByteLength;
		}

		static void SetBitness(Options options, int bitness) {
			if (options.OpCodeInfoOptions.Bitness != 0)
				throw new CommandLineParserException("Only one of -16, -32 and -64 is allowed");
			options.OpCodeInfoOptions.Bitness = bitness;
		}

		static readonly char[] seps = new char[] { ';' };
		static void AddCpuid(HashSet<CpuidFeature> cpuidHash, Dictionary<string, CpuidFeature> toCpuid, string? arg) {
			if (arg is null)
				throw new CommandLineParserException("Missing cpuid feature(s)");
			foreach (var v in arg.Split(seps, StringSplitOptions.RemoveEmptyEntries)) {
				if (!toCpuid.TryGetValue(v, out var cpuidFeature))
					throw new CommandLineParserException($"Invalid CPUID feature name: {v}");
				cpuidHash.Add(cpuidFeature);
			}
		}

		static void AddCode(HashSet<Code> codeHash, Dictionary<string, Code> toCode, string? arg) {
			if (arg is null)
				throw new CommandLineParserException("Missing Code value");
			foreach (var v in arg.Split(seps, StringSplitOptions.RemoveEmptyEntries)) {
				if (!toCode.TryGetValue(v, out var code))
					throw new CommandLineParserException($"Invalid Code name: {v}");
				codeHash.Add(code);
			}
		}
	}
}
