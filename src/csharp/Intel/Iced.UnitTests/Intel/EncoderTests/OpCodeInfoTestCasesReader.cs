// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if ENCODER && OPCODE_INFO
using System;
using System.Collections.Generic;
using System.IO;
using Iced.Intel;

namespace Iced.UnitTests.Intel.EncoderTests {
	static class OpCodeInfoTestCasesReader {
		public static IEnumerable<OpCodeInfoTestCase> ReadFile(string filename) {
			int lineNo = 0;
			foreach (var line in File.ReadLines(filename)) {
				lineNo++;
				if (line.Length == 0 || line[0] == '#')
					continue;
				OpCodeInfoTestCase testCase;
				try {
					testCase = ReadTestCase(line, lineNo);
				}
				catch (Exception ex) {
					throw new InvalidOperationException($"Error parsing opcode test case file '{filename}', line {lineNo}: {ex.Message}");
				}
				if (testCase is not null)
					yield return testCase;
			}
		}

		static readonly char[] seps = new char[] { ',' };
		static readonly char[] optsseps = new char[] { ' ' };
		static readonly char[] opseps = new char[] { ';' };
		static OpCodeInfoTestCase ReadTestCase(string line, int lineNo) {
			var parts = line.Split(seps);
			if (parts.Length != 11)
				throw new InvalidOperationException($"Invalid number of commas ({parts.Length - 1} commas)");

			var tc = new OpCodeInfoTestCase();
			tc.LineNumber = lineNo;
			tc.IsInstruction = true;
			tc.GroupIndex = -1;

			var code = parts[0].Trim();
			if (CodeUtils.IsIgnored(code))
				return null;
			tc.Code = ToCode(code);
			tc.Mnemonic = ToMnemonic(parts[1].Trim());
			tc.MemorySize = ToMemorySize(parts[2].Trim());
			tc.BroadcastMemorySize = ToMemorySize(parts[3].Trim());
			tc.Encoding = ToEncoding(parts[4].Trim());
			tc.MandatoryPrefix = ToMandatoryPrefix(parts[5].Trim());
			tc.Table = ToTable(parts[6].Trim());
			tc.OpCode = ToOpCode(parts[7].Trim(), out tc.OpCodeLength);
			tc.OpCodeString = parts[8].Trim();
			tc.InstructionString = parts[9].Trim().Replace('|', ',');

			bool gotVectorLength = false;
			bool gotW = false;
			foreach (var part in parts[10].Split(optsseps)) {
				var key = part.Trim();
				if (key.Length == 0)
					continue;
				int index = key.IndexOf('=');
				if (index >= 0) {
					var value = key.Substring(index + 1);
					key = key.Substring(0, index);
					switch (key) {
					case OpCodeInfoKeys.GroupIndex:
						if (!uint.TryParse(value, out uint groupIndex) || groupIndex > 7)
							throw new InvalidOperationException($"Invalid group index: {value}");
						tc.GroupIndex = (int)groupIndex;
						tc.IsGroup = true;
						break;

					case OpCodeInfoKeys.RmGroupIndex:
						if (!uint.TryParse(value, out uint rmGroupIndex) || rmGroupIndex > 7)
							throw new InvalidOperationException($"Invalid group index: {value}");
						tc.RmGroupIndex = (int)rmGroupIndex;
						tc.IsRmGroup = true;
						break;

					case OpCodeInfoKeys.OpCodeOperandKind:
						var opParts = value.Split(opseps);
						tc.OpCount = opParts.Length;
						if (opParts.Length >= 1)
							tc.Op0Kind = ToOpCodeOperandKind(opParts[0]);
						if (opParts.Length >= 2)
							tc.Op1Kind = ToOpCodeOperandKind(opParts[1]);
						if (opParts.Length >= 3)
							tc.Op2Kind = ToOpCodeOperandKind(opParts[2]);
						if (opParts.Length >= 4)
							tc.Op3Kind = ToOpCodeOperandKind(opParts[3]);
						if (opParts.Length >= 5)
							tc.Op4Kind = ToOpCodeOperandKind(opParts[4]);
						Static.Assert(IcedConstants.MaxOpCount == 5 ? 0 : -1);
						if (opParts.Length >= 6)
							throw new InvalidOperationException($"Invalid number of operands: '{value}'");
						break;

					case OpCodeInfoKeys.TupleType:
						tc.TupleType = ToTupleType(value.Trim());
						break;

					case OpCodeInfoKeys.DecoderOption:
						tc.DecoderOption = ToDecoderOptions(value.Trim());
						break;

					case OpCodeInfoKeys.MVEX:
#if MVEX
						var mvexParts = value.Split(opseps);
						if (mvexParts.Length != 4)
							throw new InvalidOperationException($"Invalid number of semicolons. Expected 3, found {mvexParts.Length - 1}");
						tc.Mvex.TupleTypeLutKind = ToMvexTupleTypeLutKind(mvexParts[0].Trim());
						tc.Mvex.ConversionFunc = ToMvexConvFn(mvexParts[1].Trim());
						tc.Mvex.ValidConversionFuncsMask = NumberConverter.ToUInt8(mvexParts[2].Trim());
						tc.Mvex.ValidSwizzleFuncsMask = NumberConverter.ToUInt8(mvexParts[3].Trim());
						break;
#else
						throw new InvalidOperationException();
#endif

					default:
						throw new InvalidOperationException($"Invalid key: '{key}'");
					}
				}
				else {
					switch (key) {
					case OpCodeInfoFlags.NoInstruction:
						tc.IsInstruction = false;
						break;

					case OpCodeInfoFlags.Bit16:
						tc.Mode16 = true;
						break;

					case OpCodeInfoFlags.Bit32:
						tc.Mode32 = true;
						break;

					case OpCodeInfoFlags.Bit64:
						tc.Mode64 = true;
						break;

					case OpCodeInfoFlags.Fwait:
						tc.Fwait = true;
						break;

					case OpCodeInfoFlags.OperandSize16:
						tc.OperandSize = 16;
						break;

					case OpCodeInfoFlags.OperandSize32:
						tc.OperandSize = 32;
						break;

					case OpCodeInfoFlags.OperandSize64:
						tc.OperandSize = 64;
						break;

					case OpCodeInfoFlags.AddressSize16:
						tc.AddressSize = 16;
						break;

					case OpCodeInfoFlags.AddressSize32:
						tc.AddressSize = 32;
						break;

					case OpCodeInfoFlags.AddressSize64:
						tc.AddressSize = 64;
						break;

					case OpCodeInfoFlags.LIG:
						tc.IsLIG = true;
						gotVectorLength = true;
						break;

					case OpCodeInfoFlags.L0:
						tc.L = 0;
						gotVectorLength = true;
						break;

					case OpCodeInfoFlags.L1:
						tc.L = 1;
						gotVectorLength = true;
						break;

					case OpCodeInfoFlags.L128:
						tc.L = 0;
						gotVectorLength = true;
						break;

					case OpCodeInfoFlags.L256:
						tc.L = 1;
						gotVectorLength = true;
						break;

					case OpCodeInfoFlags.L512:
						tc.L = 2;
						gotVectorLength = true;
						break;

					case OpCodeInfoFlags.WIG:
						tc.IsWIG = true;
						gotW = true;
						break;

					case OpCodeInfoFlags.WIG32:
						tc.W = 0;
						tc.IsWIG32 = true;
						gotW = true;
						break;

					case OpCodeInfoFlags.W0:
						tc.W = 0;
						gotW = true;
						break;

					case OpCodeInfoFlags.W1:
						tc.W = 1;
						gotW = true;
						break;

					case OpCodeInfoFlags.Broadcast:
						tc.CanBroadcast = true;
						break;

					case OpCodeInfoFlags.RoundingControl:
						tc.CanUseRoundingControl = true;
						break;

					case OpCodeInfoFlags.SuppressAllExceptions:
						tc.CanSuppressAllExceptions = true;
						break;

					case OpCodeInfoFlags.OpMaskRegister:
						tc.CanUseOpMaskRegister = true;
						break;

					case OpCodeInfoFlags.RequireOpMaskRegister:
						tc.CanUseOpMaskRegister = true;
						tc.RequireOpMaskRegister = true;
						break;

					case OpCodeInfoFlags.ZeroingMasking:
						tc.CanUseZeroingMasking = true;
						break;

					case OpCodeInfoFlags.Lock:
						tc.CanUseLockPrefix = true;
						break;

					case OpCodeInfoFlags.Xacquire:
						tc.CanUseXacquirePrefix = true;
						break;

					case OpCodeInfoFlags.Xrelease:
						tc.CanUseXreleasePrefix = true;
						break;

					case OpCodeInfoFlags.Rep:
					case OpCodeInfoFlags.Repe:
						tc.CanUseRepPrefix = true;
						break;

					case OpCodeInfoFlags.Repne:
						tc.CanUseRepnePrefix = true;
						break;

					case OpCodeInfoFlags.Bnd:
						tc.CanUseBndPrefix = true;
						break;

					case OpCodeInfoFlags.HintTaken:
						tc.CanUseHintTakenPrefix = true;
						break;

					case OpCodeInfoFlags.Notrack:
						tc.CanUseNotrackPrefix = true;
						break;

					case OpCodeInfoFlags.IgnoresRoundingControl:
						tc.IgnoresRoundingControl = true;
						break;

					case OpCodeInfoFlags.AmdLockRegBit:
						tc.AmdLockRegBit = true;
						break;

					case OpCodeInfoFlags.DefaultOpSize64:
						tc.DefaultOpSize64 = true;
						break;

					case OpCodeInfoFlags.ForceOpSize64:
						tc.ForceOpSize64 = true;
						break;

					case OpCodeInfoFlags.IntelForceOpSize64:
						tc.IntelForceOpSize64 = true;
						break;

					case OpCodeInfoFlags.Cpl0:
						tc.Cpl0 = true;
						break;

					case OpCodeInfoFlags.Cpl1:
						tc.Cpl1 = true;
						break;

					case OpCodeInfoFlags.Cpl2:
						tc.Cpl2 = true;
						break;

					case OpCodeInfoFlags.Cpl3:
						tc.Cpl3 = true;
						break;

					case OpCodeInfoFlags.InputOutput:
						tc.IsInputOutput = true;
						break;

					case OpCodeInfoFlags.Nop:
						tc.IsNop = true;
						break;

					case OpCodeInfoFlags.ReservedNop:
						tc.IsReservedNop = true;
						break;

					case OpCodeInfoFlags.SerializingIntel:
						tc.IsSerializingIntel = true;
						break;

					case OpCodeInfoFlags.SerializingAmd:
						tc.IsSerializingAmd = true;
						break;

					case OpCodeInfoFlags.MayRequireCpl0:
						tc.MayRequireCpl0 = true;
						break;

					case OpCodeInfoFlags.CetTracked:
						tc.IsCetTracked = true;
						break;

					case OpCodeInfoFlags.NonTemporal:
						tc.IsNonTemporal = true;
						break;

					case OpCodeInfoFlags.FpuNoWait:
						tc.IsFpuNoWait = true;
						break;

					case OpCodeInfoFlags.IgnoresModBits:
						tc.IgnoresModBits = true;
						break;

					case OpCodeInfoFlags.No66:
						tc.No66 = true;
						break;

					case OpCodeInfoFlags.NFx:
						tc.NFx = true;
						break;

					case OpCodeInfoFlags.RequiresUniqueRegNums:
						tc.RequiresUniqueRegNums = true;
						break;

					case OpCodeInfoFlags.Privileged:
						tc.IsPrivileged = true;
						break;

					case OpCodeInfoFlags.SaveRestore:
						tc.IsSaveRestore = true;
						break;

					case OpCodeInfoFlags.StackInstruction:
						tc.IsStackInstruction = true;
						break;

					case OpCodeInfoFlags.IgnoresSegment:
						tc.IgnoresSegment = true;
						break;

					case OpCodeInfoFlags.OpMaskReadWrite:
						tc.IsOpMaskReadWrite = true;
						break;

					case OpCodeInfoFlags.RealMode:
						tc.RealMode = true;
						break;

					case OpCodeInfoFlags.ProtectedMode:
						tc.ProtectedMode = true;
						break;

					case OpCodeInfoFlags.Virtual8086Mode:
						tc.Virtual8086Mode = true;
						break;

					case OpCodeInfoFlags.CompatibilityMode:
						tc.CompatibilityMode = true;
						break;

					case OpCodeInfoFlags.LongMode:
						tc.LongMode = true;
						break;

					case OpCodeInfoFlags.UseOutsideSmm:
						tc.UseOutsideSmm = true;
						break;

					case OpCodeInfoFlags.UseInSmm:
						tc.UseInSmm = true;
						break;

					case OpCodeInfoFlags.UseOutsideEnclaveSgx:
						tc.UseOutsideEnclaveSgx = true;
						break;

					case OpCodeInfoFlags.UseInEnclaveSgx1:
						tc.UseInEnclaveSgx1 = true;
						break;

					case OpCodeInfoFlags.UseInEnclaveSgx2:
						tc.UseInEnclaveSgx2 = true;
						break;

					case OpCodeInfoFlags.UseOutsideVmxOp:
						tc.UseOutsideVmxOp = true;
						break;

					case OpCodeInfoFlags.UseInVmxRootOp:
						tc.UseInVmxRootOp = true;
						break;

					case OpCodeInfoFlags.UseInVmxNonRootOp:
						tc.UseInVmxNonRootOp = true;
						break;

					case OpCodeInfoFlags.UseOutsideSeam:
						tc.UseOutsideSeam = true;
						break;

					case OpCodeInfoFlags.UseInSeam:
						tc.UseInSeam = true;
						break;

					case OpCodeInfoFlags.TdxNonRootGenUd:
						tc.TdxNonRootGenUd = true;
						break;

					case OpCodeInfoFlags.TdxNonRootGenVe:
						tc.TdxNonRootGenVe = true;
						break;

					case OpCodeInfoFlags.TdxNonRootMayGenEx:
						tc.TdxNonRootMayGenEx = true;
						break;

					case OpCodeInfoFlags.IntelVmExit:
						tc.IntelVmExit = true;
						break;

					case OpCodeInfoFlags.IntelMayVmExit:
						tc.IntelMayVmExit = true;
						break;

					case OpCodeInfoFlags.IntelSmmVmExit:
						tc.IntelSmmVmExit = true;
						break;

					case OpCodeInfoFlags.AmdVmExit:
						tc.AmdVmExit = true;
						break;

					case OpCodeInfoFlags.AmdMayVmExit:
						tc.AmdMayVmExit = true;
						break;

					case OpCodeInfoFlags.TsxAbort:
						tc.TsxAbort = true;
						break;

					case OpCodeInfoFlags.TsxImplAbort:
						tc.TsxImplAbort = true;
						break;

					case OpCodeInfoFlags.TsxMayAbort:
						tc.TsxMayAbort = true;
						break;

					case OpCodeInfoFlags.IntelDecoder16:
						tc.IntelDecoder16 = true;
						break;

					case OpCodeInfoFlags.IntelDecoder32:
						tc.IntelDecoder32 = true;
						break;

					case OpCodeInfoFlags.IntelDecoder64:
						tc.IntelDecoder64 = true;
						break;

					case OpCodeInfoFlags.AmdDecoder16:
						tc.AmdDecoder16 = true;
						break;

					case OpCodeInfoFlags.AmdDecoder32:
						tc.AmdDecoder32 = true;
						break;

					case OpCodeInfoFlags.AmdDecoder64:
						tc.AmdDecoder64 = true;
						break;

					case OpCodeInfoFlags.RequiresUniqueDestRegNum:
						tc.RequiresUniqueDestRegNum = true;
						break;

					case OpCodeInfoFlags.EH0:
#if MVEX
						tc.Mvex.EHBit = MvexEHBit.EH0;
						break;
#else
						throw new InvalidOperationException();
#endif

					case OpCodeInfoFlags.EH1:
#if MVEX
						tc.Mvex.EHBit = MvexEHBit.EH1;
						break;
#else
						throw new InvalidOperationException();
#endif

					case OpCodeInfoFlags.EvictionHint:
#if MVEX
						tc.Mvex.CanUseEvictionHint = true;
						break;
#else
						throw new InvalidOperationException();
#endif

					case OpCodeInfoFlags.ImmRoundingControl:
#if MVEX
						tc.Mvex.CanUseImmRoundingControl = true;
						break;
#else
						throw new InvalidOperationException();
#endif

					case OpCodeInfoFlags.IgnoresOpMaskRegister:
#if MVEX
						tc.Mvex.IgnoresOpMaskRegister = true;
						break;
#else
						throw new InvalidOperationException();
#endif

					case OpCodeInfoFlags.NoSaeRoundingControl:
#if MVEX
						tc.Mvex.NoSaeRc = true;
						break;
#else
						throw new InvalidOperationException();
#endif

					default:
						throw new InvalidOperationException($"Invalid key: '{key}'");
					}
				}
			}
			switch (tc.Encoding) {
			case EncodingKind.Legacy:
			case EncodingKind.D3NOW:
				break;
			case EncodingKind.VEX:
			case EncodingKind.EVEX:
			case EncodingKind.XOP:
			case EncodingKind.MVEX:
				if (!gotVectorLength)
					throw new InvalidOperationException("Missing vector length: L0/L1/L128/L256/L512/LIG");
				if (!gotW)
					throw new InvalidOperationException("Missing W bit: W0/W1/WIG/WIG32");
				break;
			default:
				throw new InvalidOperationException();
			}

			return tc;
		}

		static Code ToCode(string value) {
			if (!ToEnumConverter.TryCode(value, out var code))
				throw new InvalidOperationException($"Invalid Code value: '{value}'");
			return code;
		}

		static Mnemonic ToMnemonic(string value) {
			if (!ToEnumConverter.TryMnemonic(value, out var mnemonic))
				throw new InvalidOperationException($"Invalid Mnemonic value: '{value}'");
			return mnemonic;
		}

		static MemorySize ToMemorySize(string value) {
			if (!ToEnumConverter.TryMemorySize(value, out var memorySize))
				throw new InvalidOperationException($"Invalid MemorySize value: '{value}'");
			return memorySize;
		}

		static TupleType ToTupleType(string value) {
			if (!ToEnumConverter.TryTupleType(value, out var code))
				throw new InvalidOperationException($"Invalid TupleType value: '{value}'");
			return code;
		}

		static DecoderOptions ToDecoderOptions(string value) {
			if (!ToEnumConverter.TryDecoderOptions(value, out var code))
				throw new InvalidOperationException($"Invalid DecoderOptions value: '{value}'");
			return code;
		}

		static OpCodeOperandKind ToOpCodeOperandKind(string value) {
			if (!ToEnumConverter.TryOpCodeOperandKind(value, out var code))
				throw new InvalidOperationException($"Invalid OpCodeOperandKind value: '{value}'");
			return code;
		}

#if MVEX
		static MvexTupleTypeLutKind ToMvexTupleTypeLutKind(string value) {
			if (!ToEnumConverter.TryMvexTupleTypeLutKind(value, out var result))
				throw new InvalidOperationException($"Invalid MvexTupleTypeLutKind value: '{value}'");
			return result;
		}
#endif

#if MVEX
		static MvexConvFn ToMvexConvFn(string value) {
			if (!ToEnumConverter.TryMvexConvFn(value, out var mvexConvFn))
				throw new InvalidOperationException($"Invalid MvexConvFn value: '{value}'");
			return mvexConvFn;
		}
#endif

		static EncodingKind ToEncoding(string value) {
			if (OpCodeInfoDicts.ToEncodingKind.TryGetValue(value, out var kind))
				return kind;
			throw new InvalidOperationException($"Invalid encoding value: '{value}'");
		}

		static MandatoryPrefix ToMandatoryPrefix(string value) {
			if (OpCodeInfoDicts.ToMandatoryPrefix.TryGetValue(value, out var prefix))
				return prefix;
			throw new InvalidOperationException($"Invalid mandatory prefix value: '{value}'");
		}

		static OpCodeTableKind ToTable(string value) {
			if (OpCodeInfoDicts.ToOpCodeTableKind.TryGetValue(value, out var kind))
				return kind;
			throw new InvalidOperationException($"Invalid opcode table value: '{value}'");
		}

		static uint ToOpCode(string value, out int opCodeLength) {
			if (value.Length == 2 || value.Length == 4) {
				opCodeLength = value.Length / 2;
				if (uint.TryParse(value, System.Globalization.NumberStyles.HexNumber, null, out uint result))
					return result;
			}
			throw new InvalidOperationException($"Invalid opcode: '{value}'");
		}
	}
}
#endif
