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

#if !NO_INSTR_INFO
using System;
using System.Collections.Generic;
using Iced.Intel;
using Iced.UnitTests.Intel.DecoderTests;
using Xunit;

namespace Iced.UnitTests.Intel.InstructionInfoTests {
	public sealed class MiscTests {
		[Fact]
		void IsBranchCall() {
			var jccShort = new HashSet<Code> {
				Code.Jo_rel8_16,
				Code.Jo_rel8_32,
				Code.Jo_rel8_64,
				Code.Jno_rel8_16,
				Code.Jno_rel8_32,
				Code.Jno_rel8_64,
				Code.Jb_rel8_16,
				Code.Jb_rel8_32,
				Code.Jb_rel8_64,
				Code.Jae_rel8_16,
				Code.Jae_rel8_32,
				Code.Jae_rel8_64,
				Code.Je_rel8_16,
				Code.Je_rel8_32,
				Code.Je_rel8_64,
				Code.Jne_rel8_16,
				Code.Jne_rel8_32,
				Code.Jne_rel8_64,
				Code.Jbe_rel8_16,
				Code.Jbe_rel8_32,
				Code.Jbe_rel8_64,
				Code.Ja_rel8_16,
				Code.Ja_rel8_32,
				Code.Ja_rel8_64,

				Code.Js_rel8_16,
				Code.Js_rel8_32,
				Code.Js_rel8_64,
				Code.Jns_rel8_16,
				Code.Jns_rel8_32,
				Code.Jns_rel8_64,
				Code.Jp_rel8_16,
				Code.Jp_rel8_32,
				Code.Jp_rel8_64,
				Code.Jnp_rel8_16,
				Code.Jnp_rel8_32,
				Code.Jnp_rel8_64,
				Code.Jl_rel8_16,
				Code.Jl_rel8_32,
				Code.Jl_rel8_64,
				Code.Jge_rel8_16,
				Code.Jge_rel8_32,
				Code.Jge_rel8_64,
				Code.Jle_rel8_16,
				Code.Jle_rel8_32,
				Code.Jle_rel8_64,
				Code.Jg_rel8_16,
				Code.Jg_rel8_32,
				Code.Jg_rel8_64,
			};

			var jmpNear = new HashSet<Code> {
				Code.Jmp_rel16,
				Code.Jmp_rel32_32,
				Code.Jmp_rel32_64,
			};

			var jmpFar = new HashSet<Code> {
				Code.Jmp_ptr1616,
				Code.Jmp_ptr1632,
			};

			var jmpShort = new HashSet<Code> {
				Code.Jmp_rel8_16,
				Code.Jmp_rel8_32,
				Code.Jmp_rel8_64,
			};

			var jmpNearIndirect = new HashSet<Code> {
				Code.Jmp_rm16,
				Code.Jmp_rm32,
				Code.Jmp_rm64,
			};

			var jmpFarIndirect = new HashSet<Code> {
				Code.Jmp_m1616,
				Code.Jmp_m1632,
				Code.Jmp_m1664,
			};

			var jccNear = new HashSet<Code> {
				Code.Jo_rel16,
				Code.Jo_rel32_32,
				Code.Jo_rel32_64,
				Code.Jno_rel16,
				Code.Jno_rel32_32,
				Code.Jno_rel32_64,
				Code.Jb_rel16,
				Code.Jb_rel32_32,
				Code.Jb_rel32_64,
				Code.Jae_rel16,
				Code.Jae_rel32_32,
				Code.Jae_rel32_64,
				Code.Je_rel16,
				Code.Je_rel32_32,
				Code.Je_rel32_64,
				Code.Jne_rel16,
				Code.Jne_rel32_32,
				Code.Jne_rel32_64,
				Code.Jbe_rel16,
				Code.Jbe_rel32_32,
				Code.Jbe_rel32_64,
				Code.Ja_rel16,
				Code.Ja_rel32_32,
				Code.Ja_rel32_64,

				Code.Js_rel16,
				Code.Js_rel32_32,
				Code.Js_rel32_64,
				Code.Jns_rel16,
				Code.Jns_rel32_32,
				Code.Jns_rel32_64,
				Code.Jp_rel16,
				Code.Jp_rel32_32,
				Code.Jp_rel32_64,
				Code.Jnp_rel16,
				Code.Jnp_rel32_32,
				Code.Jnp_rel32_64,
				Code.Jl_rel16,
				Code.Jl_rel32_32,
				Code.Jl_rel32_64,
				Code.Jge_rel16,
				Code.Jge_rel32_32,
				Code.Jge_rel32_64,
				Code.Jle_rel16,
				Code.Jle_rel32_32,
				Code.Jle_rel32_64,
				Code.Jg_rel16,
				Code.Jg_rel32_32,
				Code.Jg_rel32_64,
			};

			var callFar = new HashSet<Code> {
				Code.Call_ptr1616,
				Code.Call_ptr1632,
			};

			var callNear = new HashSet<Code> {
				Code.Call_rel16,
				Code.Call_rel32_32,
				Code.Call_rel32_64,
			};

			var callNearIndirect = new HashSet<Code> {
				Code.Call_rm16,
				Code.Call_rm32,
				Code.Call_rm64,
			};

			var callFarIndirect = new HashSet<Code> {
				Code.Call_m1616,
				Code.Call_m1632,
				Code.Call_m1664,
			};

			for (int i = 0; i < Iced.Intel.DecoderConstants.NumberOfCodeValues; i++) {
				var code = (Code)i;
				Instruction instr = default;
				instr.Code = code;

				Assert.Equal(jccShort.Contains(code) || jccNear.Contains(code), code.IsJccShortOrNear());
				Assert.Equal(code.IsJccShortOrNear(), instr.IsJccShortOrNear);

				Assert.Equal(jccNear.Contains(code), code.IsJccNear());
				Assert.Equal(code.IsJccNear(), instr.IsJccNear);

				Assert.Equal(jccShort.Contains(code), code.IsJccShort());
				Assert.Equal(code.IsJccShort(), instr.IsJccShort);

				Assert.Equal(jmpShort.Contains(code), code.IsJmpShort());
				Assert.Equal(code.IsJmpShort(), instr.IsJmpShort);

				Assert.Equal(jmpNear.Contains(code), code.IsJmpNear());
				Assert.Equal(code.IsJmpNear(), instr.IsJmpNear);

				Assert.Equal(jmpShort.Contains(code) || jmpNear.Contains(code), code.IsJmpShortOrNear());
				Assert.Equal(code.IsJmpShortOrNear(), instr.IsJmpShortOrNear);

				Assert.Equal(jmpFar.Contains(code), code.IsJmpFar());
				Assert.Equal(code.IsJmpFar(), instr.IsJmpFar);

				Assert.Equal(callNear.Contains(code), code.IsCallNear());
				Assert.Equal(code.IsCallNear(), instr.IsCallNear);

				Assert.Equal(callFar.Contains(code), code.IsCallFar());
				Assert.Equal(code.IsCallFar(), instr.IsCallFar);

				Assert.Equal(jmpNearIndirect.Contains(code), code.IsJmpNearIndirect());
				Assert.Equal(code.IsJmpNearIndirect(), instr.IsJmpNearIndirect);

				Assert.Equal(jmpFarIndirect.Contains(code), code.IsJmpFarIndirect());
				Assert.Equal(code.IsJmpFarIndirect(), instr.IsJmpFarIndirect);

				Assert.Equal(callNearIndirect.Contains(code), code.IsCallNearIndirect());
				Assert.Equal(code.IsCallNearIndirect(), instr.IsCallNearIndirect);

				Assert.Equal(callFarIndirect.Contains(code), code.IsCallFarIndirect());
				Assert.Equal(code.IsCallFarIndirect(), instr.IsCallFarIndirect);
			}
		}

		[Fact]
		void Verify_ProtectedMode_is_true_if_VEX_XOP_EVEX() {
			foreach (var info in DecoderTestUtils.GetDecoderTests(includeOtherTests: false, includeInvalid: false)) {
				var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(info.HexBytes), info.Options);
				decoder.Decode(out var instr);
				switch (instr.Encoding) {
				case EncodingKind.Legacy:
				case EncodingKind.D3NOW:
					break;
				case EncodingKind.VEX:
				case EncodingKind.EVEX:
				case EncodingKind.XOP:
					Assert.True(instr.IsProtectedMode);
					Assert.True(info.Code.IsProtectedMode());
					break;
				default:
					throw new InvalidOperationException();
				}
			}
		}

		static readonly (Code jmpShort, Code jmpNear)[] jmpInfos = new (Code jmpShort, Code jmpNear)[] {
			(Code.Jmp_rel8_16, Code.Jmp_rel16),
			(Code.Jmp_rel8_32, Code.Jmp_rel32_32),
			(Code.Jmp_rel8_64, Code.Jmp_rel32_64),
		};
		static readonly (Code jcc, Code negated, Code jccNear, ConditionCode cc)[] jccShortInfos = new (Code jcc, Code negated, Code jccNear, ConditionCode cc)[] {
			(Code.Jo_rel8_16, Code.Jno_rel8_16, Code.Jo_rel16, ConditionCode.o),
			(Code.Jo_rel8_32, Code.Jno_rel8_32, Code.Jo_rel32_32, ConditionCode.o),
			(Code.Jo_rel8_64, Code.Jno_rel8_64, Code.Jo_rel32_64, ConditionCode.o),
			(Code.Jno_rel8_16, Code.Jo_rel8_16, Code.Jno_rel16, ConditionCode.no),
			(Code.Jno_rel8_32, Code.Jo_rel8_32, Code.Jno_rel32_32, ConditionCode.no),
			(Code.Jno_rel8_64, Code.Jo_rel8_64, Code.Jno_rel32_64, ConditionCode.no),
			(Code.Jb_rel8_16, Code.Jae_rel8_16, Code.Jb_rel16, ConditionCode.b),
			(Code.Jb_rel8_32, Code.Jae_rel8_32, Code.Jb_rel32_32, ConditionCode.b),
			(Code.Jb_rel8_64, Code.Jae_rel8_64, Code.Jb_rel32_64, ConditionCode.b),
			(Code.Jae_rel8_16, Code.Jb_rel8_16, Code.Jae_rel16, ConditionCode.ae),
			(Code.Jae_rel8_32, Code.Jb_rel8_32, Code.Jae_rel32_32, ConditionCode.ae),
			(Code.Jae_rel8_64, Code.Jb_rel8_64, Code.Jae_rel32_64, ConditionCode.ae),
			(Code.Je_rel8_16, Code.Jne_rel8_16, Code.Je_rel16, ConditionCode.e),
			(Code.Je_rel8_32, Code.Jne_rel8_32, Code.Je_rel32_32, ConditionCode.e),
			(Code.Je_rel8_64, Code.Jne_rel8_64, Code.Je_rel32_64, ConditionCode.e),
			(Code.Jne_rel8_16, Code.Je_rel8_16, Code.Jne_rel16, ConditionCode.ne),
			(Code.Jne_rel8_32, Code.Je_rel8_32, Code.Jne_rel32_32, ConditionCode.ne),
			(Code.Jne_rel8_64, Code.Je_rel8_64, Code.Jne_rel32_64, ConditionCode.ne),
			(Code.Jbe_rel8_16, Code.Ja_rel8_16, Code.Jbe_rel16, ConditionCode.be),
			(Code.Jbe_rel8_32, Code.Ja_rel8_32, Code.Jbe_rel32_32, ConditionCode.be),
			(Code.Jbe_rel8_64, Code.Ja_rel8_64, Code.Jbe_rel32_64, ConditionCode.be),
			(Code.Ja_rel8_16, Code.Jbe_rel8_16, Code.Ja_rel16, ConditionCode.a),
			(Code.Ja_rel8_32, Code.Jbe_rel8_32, Code.Ja_rel32_32, ConditionCode.a),
			(Code.Ja_rel8_64, Code.Jbe_rel8_64, Code.Ja_rel32_64, ConditionCode.a),
			(Code.Js_rel8_16, Code.Jns_rel8_16, Code.Js_rel16, ConditionCode.s),
			(Code.Js_rel8_32, Code.Jns_rel8_32, Code.Js_rel32_32, ConditionCode.s),
			(Code.Js_rel8_64, Code.Jns_rel8_64, Code.Js_rel32_64, ConditionCode.s),
			(Code.Jns_rel8_16, Code.Js_rel8_16, Code.Jns_rel16, ConditionCode.ns),
			(Code.Jns_rel8_32, Code.Js_rel8_32, Code.Jns_rel32_32, ConditionCode.ns),
			(Code.Jns_rel8_64, Code.Js_rel8_64, Code.Jns_rel32_64, ConditionCode.ns),
			(Code.Jp_rel8_16, Code.Jnp_rel8_16, Code.Jp_rel16, ConditionCode.p),
			(Code.Jp_rel8_32, Code.Jnp_rel8_32, Code.Jp_rel32_32, ConditionCode.p),
			(Code.Jp_rel8_64, Code.Jnp_rel8_64, Code.Jp_rel32_64, ConditionCode.p),
			(Code.Jnp_rel8_16, Code.Jp_rel8_16, Code.Jnp_rel16, ConditionCode.np),
			(Code.Jnp_rel8_32, Code.Jp_rel8_32, Code.Jnp_rel32_32, ConditionCode.np),
			(Code.Jnp_rel8_64, Code.Jp_rel8_64, Code.Jnp_rel32_64, ConditionCode.np),
			(Code.Jl_rel8_16, Code.Jge_rel8_16, Code.Jl_rel16, ConditionCode.l),
			(Code.Jl_rel8_32, Code.Jge_rel8_32, Code.Jl_rel32_32, ConditionCode.l),
			(Code.Jl_rel8_64, Code.Jge_rel8_64, Code.Jl_rel32_64, ConditionCode.l),
			(Code.Jge_rel8_16, Code.Jl_rel8_16, Code.Jge_rel16, ConditionCode.ge),
			(Code.Jge_rel8_32, Code.Jl_rel8_32, Code.Jge_rel32_32, ConditionCode.ge),
			(Code.Jge_rel8_64, Code.Jl_rel8_64, Code.Jge_rel32_64, ConditionCode.ge),
			(Code.Jle_rel8_16, Code.Jg_rel8_16, Code.Jle_rel16, ConditionCode.le),
			(Code.Jle_rel8_32, Code.Jg_rel8_32, Code.Jle_rel32_32, ConditionCode.le),
			(Code.Jle_rel8_64, Code.Jg_rel8_64, Code.Jle_rel32_64, ConditionCode.le),
			(Code.Jg_rel8_16, Code.Jle_rel8_16, Code.Jg_rel16, ConditionCode.g),
			(Code.Jg_rel8_32, Code.Jle_rel8_32, Code.Jg_rel32_32, ConditionCode.g),
			(Code.Jg_rel8_64, Code.Jle_rel8_64, Code.Jg_rel32_64, ConditionCode.g),
		};
		static readonly (Code jcc, Code negated, Code jccShort, ConditionCode cc)[] jccNearInfos = new (Code jcc, Code negated, Code jccShort, ConditionCode cc)[] {
			(Code.Jo_rel16, Code.Jno_rel16, Code.Jo_rel8_16, ConditionCode.o),
			(Code.Jo_rel32_32, Code.Jno_rel32_32, Code.Jo_rel8_32, ConditionCode.o),
			(Code.Jo_rel32_64, Code.Jno_rel32_64, Code.Jo_rel8_64, ConditionCode.o),
			(Code.Jno_rel16, Code.Jo_rel16, Code.Jno_rel8_16, ConditionCode.no),
			(Code.Jno_rel32_32, Code.Jo_rel32_32, Code.Jno_rel8_32, ConditionCode.no),
			(Code.Jno_rel32_64, Code.Jo_rel32_64, Code.Jno_rel8_64, ConditionCode.no),
			(Code.Jb_rel16, Code.Jae_rel16, Code.Jb_rel8_16, ConditionCode.b),
			(Code.Jb_rel32_32, Code.Jae_rel32_32, Code.Jb_rel8_32, ConditionCode.b),
			(Code.Jb_rel32_64, Code.Jae_rel32_64, Code.Jb_rel8_64, ConditionCode.b),
			(Code.Jae_rel16, Code.Jb_rel16, Code.Jae_rel8_16, ConditionCode.ae),
			(Code.Jae_rel32_32, Code.Jb_rel32_32, Code.Jae_rel8_32, ConditionCode.ae),
			(Code.Jae_rel32_64, Code.Jb_rel32_64, Code.Jae_rel8_64, ConditionCode.ae),
			(Code.Je_rel16, Code.Jne_rel16, Code.Je_rel8_16, ConditionCode.e),
			(Code.Je_rel32_32, Code.Jne_rel32_32, Code.Je_rel8_32, ConditionCode.e),
			(Code.Je_rel32_64, Code.Jne_rel32_64, Code.Je_rel8_64, ConditionCode.e),
			(Code.Jne_rel16, Code.Je_rel16, Code.Jne_rel8_16, ConditionCode.ne),
			(Code.Jne_rel32_32, Code.Je_rel32_32, Code.Jne_rel8_32, ConditionCode.ne),
			(Code.Jne_rel32_64, Code.Je_rel32_64, Code.Jne_rel8_64, ConditionCode.ne),
			(Code.Jbe_rel16, Code.Ja_rel16, Code.Jbe_rel8_16, ConditionCode.be),
			(Code.Jbe_rel32_32, Code.Ja_rel32_32, Code.Jbe_rel8_32, ConditionCode.be),
			(Code.Jbe_rel32_64, Code.Ja_rel32_64, Code.Jbe_rel8_64, ConditionCode.be),
			(Code.Ja_rel16, Code.Jbe_rel16, Code.Ja_rel8_16, ConditionCode.a),
			(Code.Ja_rel32_32, Code.Jbe_rel32_32, Code.Ja_rel8_32, ConditionCode.a),
			(Code.Ja_rel32_64, Code.Jbe_rel32_64, Code.Ja_rel8_64, ConditionCode.a),
			(Code.Js_rel16, Code.Jns_rel16, Code.Js_rel8_16, ConditionCode.s),
			(Code.Js_rel32_32, Code.Jns_rel32_32, Code.Js_rel8_32, ConditionCode.s),
			(Code.Js_rel32_64, Code.Jns_rel32_64, Code.Js_rel8_64, ConditionCode.s),
			(Code.Jns_rel16, Code.Js_rel16, Code.Jns_rel8_16, ConditionCode.ns),
			(Code.Jns_rel32_32, Code.Js_rel32_32, Code.Jns_rel8_32, ConditionCode.ns),
			(Code.Jns_rel32_64, Code.Js_rel32_64, Code.Jns_rel8_64, ConditionCode.ns),
			(Code.Jp_rel16, Code.Jnp_rel16, Code.Jp_rel8_16, ConditionCode.p),
			(Code.Jp_rel32_32, Code.Jnp_rel32_32, Code.Jp_rel8_32, ConditionCode.p),
			(Code.Jp_rel32_64, Code.Jnp_rel32_64, Code.Jp_rel8_64, ConditionCode.p),
			(Code.Jnp_rel16, Code.Jp_rel16, Code.Jnp_rel8_16, ConditionCode.np),
			(Code.Jnp_rel32_32, Code.Jp_rel32_32, Code.Jnp_rel8_32, ConditionCode.np),
			(Code.Jnp_rel32_64, Code.Jp_rel32_64, Code.Jnp_rel8_64, ConditionCode.np),
			(Code.Jl_rel16, Code.Jge_rel16, Code.Jl_rel8_16, ConditionCode.l),
			(Code.Jl_rel32_32, Code.Jge_rel32_32, Code.Jl_rel8_32, ConditionCode.l),
			(Code.Jl_rel32_64, Code.Jge_rel32_64, Code.Jl_rel8_64, ConditionCode.l),
			(Code.Jge_rel16, Code.Jl_rel16, Code.Jge_rel8_16, ConditionCode.ge),
			(Code.Jge_rel32_32, Code.Jl_rel32_32, Code.Jge_rel8_32, ConditionCode.ge),
			(Code.Jge_rel32_64, Code.Jl_rel32_64, Code.Jge_rel8_64, ConditionCode.ge),
			(Code.Jle_rel16, Code.Jg_rel16, Code.Jle_rel8_16, ConditionCode.le),
			(Code.Jle_rel32_32, Code.Jg_rel32_32, Code.Jle_rel8_32, ConditionCode.le),
			(Code.Jle_rel32_64, Code.Jg_rel32_64, Code.Jle_rel8_64, ConditionCode.le),
			(Code.Jg_rel16, Code.Jle_rel16, Code.Jg_rel8_16, ConditionCode.g),
			(Code.Jg_rel32_32, Code.Jle_rel32_32, Code.Jg_rel8_32, ConditionCode.g),
			(Code.Jg_rel32_64, Code.Jle_rel32_64, Code.Jg_rel8_64, ConditionCode.g),
		};
		static readonly (Code setcc, Code negated, ConditionCode cc)[] setccInfos = new (Code setcc, Code negated, ConditionCode cc)[] {
			(Code.Seto_rm8, Code.Setno_rm8, ConditionCode.o),
			(Code.Setno_rm8, Code.Seto_rm8, ConditionCode.no),
			(Code.Setb_rm8, Code.Setae_rm8, ConditionCode.b),
			(Code.Setae_rm8, Code.Setb_rm8, ConditionCode.ae),
			(Code.Sete_rm8, Code.Setne_rm8, ConditionCode.e),
			(Code.Setne_rm8, Code.Sete_rm8, ConditionCode.ne),
			(Code.Setbe_rm8, Code.Seta_rm8, ConditionCode.be),
			(Code.Seta_rm8, Code.Setbe_rm8, ConditionCode.a),
			(Code.Sets_rm8, Code.Setns_rm8, ConditionCode.s),
			(Code.Setns_rm8, Code.Sets_rm8, ConditionCode.ns),
			(Code.Setp_rm8, Code.Setnp_rm8, ConditionCode.p),
			(Code.Setnp_rm8, Code.Setp_rm8, ConditionCode.np),
			(Code.Setl_rm8, Code.Setge_rm8, ConditionCode.l),
			(Code.Setge_rm8, Code.Setl_rm8, ConditionCode.ge),
			(Code.Setle_rm8, Code.Setg_rm8, ConditionCode.le),
			(Code.Setg_rm8, Code.Setle_rm8, ConditionCode.g),
		};
		static readonly (Code cmovcc, Code negated, ConditionCode cc)[] cmovccInfos = new (Code cmovcc, Code negated, ConditionCode cc)[] {
			(Code.Cmovo_r16_rm16, Code.Cmovno_r16_rm16, ConditionCode.o),
			(Code.Cmovno_r16_rm16, Code.Cmovo_r16_rm16, ConditionCode.no),
			(Code.Cmovo_r32_rm32, Code.Cmovno_r32_rm32, ConditionCode.o),
			(Code.Cmovno_r32_rm32, Code.Cmovo_r32_rm32, ConditionCode.no),
			(Code.Cmovo_r64_rm64, Code.Cmovno_r64_rm64, ConditionCode.o),
			(Code.Cmovno_r64_rm64, Code.Cmovo_r64_rm64, ConditionCode.no),
			(Code.Cmovb_r16_rm16, Code.Cmovae_r16_rm16, ConditionCode.b),
			(Code.Cmovae_r16_rm16, Code.Cmovb_r16_rm16, ConditionCode.ae),
			(Code.Cmovb_r32_rm32, Code.Cmovae_r32_rm32, ConditionCode.b),
			(Code.Cmovae_r32_rm32, Code.Cmovb_r32_rm32, ConditionCode.ae),
			(Code.Cmovb_r64_rm64, Code.Cmovae_r64_rm64, ConditionCode.b),
			(Code.Cmovae_r64_rm64, Code.Cmovb_r64_rm64, ConditionCode.ae),
			(Code.Cmove_r16_rm16, Code.Cmovne_r16_rm16, ConditionCode.e),
			(Code.Cmovne_r16_rm16, Code.Cmove_r16_rm16, ConditionCode.ne),
			(Code.Cmove_r32_rm32, Code.Cmovne_r32_rm32, ConditionCode.e),
			(Code.Cmovne_r32_rm32, Code.Cmove_r32_rm32, ConditionCode.ne),
			(Code.Cmove_r64_rm64, Code.Cmovne_r64_rm64, ConditionCode.e),
			(Code.Cmovne_r64_rm64, Code.Cmove_r64_rm64, ConditionCode.ne),
			(Code.Cmovbe_r16_rm16, Code.Cmova_r16_rm16, ConditionCode.be),
			(Code.Cmova_r16_rm16, Code.Cmovbe_r16_rm16, ConditionCode.a),
			(Code.Cmovbe_r32_rm32, Code.Cmova_r32_rm32, ConditionCode.be),
			(Code.Cmova_r32_rm32, Code.Cmovbe_r32_rm32, ConditionCode.a),
			(Code.Cmovbe_r64_rm64, Code.Cmova_r64_rm64, ConditionCode.be),
			(Code.Cmova_r64_rm64, Code.Cmovbe_r64_rm64, ConditionCode.a),
			(Code.Cmovs_r16_rm16, Code.Cmovns_r16_rm16, ConditionCode.s),
			(Code.Cmovns_r16_rm16, Code.Cmovs_r16_rm16, ConditionCode.ns),
			(Code.Cmovs_r32_rm32, Code.Cmovns_r32_rm32, ConditionCode.s),
			(Code.Cmovns_r32_rm32, Code.Cmovs_r32_rm32, ConditionCode.ns),
			(Code.Cmovs_r64_rm64, Code.Cmovns_r64_rm64, ConditionCode.s),
			(Code.Cmovns_r64_rm64, Code.Cmovs_r64_rm64, ConditionCode.ns),
			(Code.Cmovp_r16_rm16, Code.Cmovnp_r16_rm16, ConditionCode.p),
			(Code.Cmovnp_r16_rm16, Code.Cmovp_r16_rm16, ConditionCode.np),
			(Code.Cmovp_r32_rm32, Code.Cmovnp_r32_rm32, ConditionCode.p),
			(Code.Cmovnp_r32_rm32, Code.Cmovp_r32_rm32, ConditionCode.np),
			(Code.Cmovp_r64_rm64, Code.Cmovnp_r64_rm64, ConditionCode.p),
			(Code.Cmovnp_r64_rm64, Code.Cmovp_r64_rm64, ConditionCode.np),
			(Code.Cmovl_r16_rm16, Code.Cmovge_r16_rm16, ConditionCode.l),
			(Code.Cmovge_r16_rm16, Code.Cmovl_r16_rm16, ConditionCode.ge),
			(Code.Cmovl_r32_rm32, Code.Cmovge_r32_rm32, ConditionCode.l),
			(Code.Cmovge_r32_rm32, Code.Cmovl_r32_rm32, ConditionCode.ge),
			(Code.Cmovl_r64_rm64, Code.Cmovge_r64_rm64, ConditionCode.l),
			(Code.Cmovge_r64_rm64, Code.Cmovl_r64_rm64, ConditionCode.ge),
			(Code.Cmovle_r16_rm16, Code.Cmovg_r16_rm16, ConditionCode.le),
			(Code.Cmovg_r16_rm16, Code.Cmovle_r16_rm16, ConditionCode.g),
			(Code.Cmovle_r32_rm32, Code.Cmovg_r32_rm32, ConditionCode.le),
			(Code.Cmovg_r32_rm32, Code.Cmovle_r32_rm32, ConditionCode.g),
			(Code.Cmovle_r64_rm64, Code.Cmovg_r64_rm64, ConditionCode.le),
			(Code.Cmovg_r64_rm64, Code.Cmovle_r64_rm64, ConditionCode.g),
		};

		[Fact]
		void Verify_NegateConditionCode() {
			var toNegatedCodeValue = new Dictionary<Code, Code>();
			foreach (var info in jccShortInfos)
				toNegatedCodeValue.Add(info.jcc, info.negated);
			foreach (var info in jccNearInfos)
				toNegatedCodeValue.Add(info.jcc, info.negated);
			foreach (var info in setccInfos)
				toNegatedCodeValue.Add(info.setcc, info.negated);
			foreach (var info in cmovccInfos)
				toNegatedCodeValue.Add(info.cmovcc, info.negated);

			for (int i = 0; i < Iced.Intel.DecoderConstants.NumberOfCodeValues; i++) {
				var code = (Code)i;
				Instruction instr = default;
				instr.Code = code;

				if (!toNegatedCodeValue.TryGetValue(code, out var negatedCodeValue))
					negatedCodeValue = code;

				Assert.Equal(negatedCodeValue, code.NegateConditionCode());
				instr.NegateConditionCode();
				Assert.Equal(negatedCodeValue, instr.Code);
			}
		}

		[Fact]
		void Verify_ToShortBranch() {
			var toShortBranch = new Dictionary<Code, Code>();
			foreach (var info in jccNearInfos)
				toShortBranch.Add(info.jcc, info.jccShort);
			foreach (var info in jmpInfos)
				toShortBranch.Add(info.jmpNear, info.jmpShort);

			for (int i = 0; i < Iced.Intel.DecoderConstants.NumberOfCodeValues; i++) {
				var code = (Code)i;
				Instruction instr = default;
				instr.Code = code;

				if (!toShortBranch.TryGetValue(code, out var shortCodeValue))
					shortCodeValue = code;

				Assert.Equal(shortCodeValue, code.ToShortBranch());
				instr.ToShortBranch();
				Assert.Equal(shortCodeValue, instr.Code);
			}
		}

		[Fact]
		void Verify_ToNearBranch() {
			var toNearBranch = new Dictionary<Code, Code>();
			foreach (var info in jccShortInfos)
				toNearBranch.Add(info.jcc, info.jccNear);
			foreach (var info in jmpInfos)
				toNearBranch.Add(info.jmpShort, info.jmpNear);

			for (int i = 0; i < Iced.Intel.DecoderConstants.NumberOfCodeValues; i++) {
				var code = (Code)i;
				Instruction instr = default;
				instr.Code = code;

				if (!toNearBranch.TryGetValue(code, out var nearCodeValue))
					nearCodeValue = code;

				Assert.Equal(nearCodeValue, code.ToNearBranch());
				instr.ToNearBranch();
				Assert.Equal(nearCodeValue, instr.Code);
			}
		}

		[Fact]
		void Verify_ConditionCode() {
			var toConditionCode = new Dictionary<Code, ConditionCode>();
			foreach (var info in jccShortInfos)
				toConditionCode.Add(info.jcc, info.cc);
			foreach (var info in jccNearInfos)
				toConditionCode.Add(info.jcc, info.cc);
			foreach (var info in setccInfos)
				toConditionCode.Add(info.setcc, info.cc);
			foreach (var info in cmovccInfos)
				toConditionCode.Add(info.cmovcc, info.cc);

			for (int i = 0; i < Iced.Intel.DecoderConstants.NumberOfCodeValues; i++) {
				var code = (Code)i;
				Instruction instr = default;
				instr.Code = code;

				if (!toConditionCode.TryGetValue(code, out var cc))
					cc = ConditionCode.None;

				Assert.Equal(cc, code.GetConditionCode());
				Assert.Equal(cc, instr.ConditionCode);
			}
		}

		[Fact]
		void Verify_ConditionCode_values_are_in_correct_order() {
			Assert.Equal(0, (int)ConditionCode.None);
			Assert.Equal(1, (int)ConditionCode.o);
			Assert.Equal(2, (int)ConditionCode.no);
			Assert.Equal(3, (int)ConditionCode.b);
			Assert.Equal(4, (int)ConditionCode.ae);
			Assert.Equal(5, (int)ConditionCode.e);
			Assert.Equal(6, (int)ConditionCode.ne);
			Assert.Equal(7, (int)ConditionCode.be);
			Assert.Equal(8, (int)ConditionCode.a);
			Assert.Equal(9, (int)ConditionCode.s);
			Assert.Equal(10, (int)ConditionCode.ns);
			Assert.Equal(11, (int)ConditionCode.p);
			Assert.Equal(12, (int)ConditionCode.np);
			Assert.Equal(13, (int)ConditionCode.l);
			Assert.Equal(14, (int)ConditionCode.ge);
			Assert.Equal(15, (int)ConditionCode.le);
			Assert.Equal(16, (int)ConditionCode.g);
		}

		[Fact]
		void InstructionInfoExtensions_Encoding_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)(-1)).Encoding());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)Iced.Intel.DecoderConstants.NumberOfCodeValues).Encoding());
		}

		[Fact]
		void InstructionInfoExtensions_CpuidFeatures_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)(-1)).CpuidFeatures());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)Iced.Intel.DecoderConstants.NumberOfCodeValues).CpuidFeatures());
		}

		[Fact]
		void InstructionInfoExtensions_FlowControl_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)(-1)).FlowControl());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)Iced.Intel.DecoderConstants.NumberOfCodeValues).FlowControl());
		}

		[Fact]
		void InstructionInfoExtensions_IsProtectedMode_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)(-1)).IsProtectedMode());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)Iced.Intel.DecoderConstants.NumberOfCodeValues).IsProtectedMode());
		}

		[Fact]
		void InstructionInfoExtensions_IsPrivileged_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)(-1)).IsPrivileged());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)Iced.Intel.DecoderConstants.NumberOfCodeValues).IsPrivileged());
		}

		[Fact]
		void InstructionInfoExtensions_IsStackInstruction_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)(-1)).IsStackInstruction());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)Iced.Intel.DecoderConstants.NumberOfCodeValues).IsStackInstruction());
		}

		[Fact]
		void InstructionInfoExtensions_IsSaveRestoreInstruction_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)(-1)).IsSaveRestoreInstruction());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)Iced.Intel.DecoderConstants.NumberOfCodeValues).IsSaveRestoreInstruction());
		}

		[Fact]
		void InstructionInfo_GetOpAccess_throws_if_invalid_input() {
			var info = Instruction.Create(Code.Nopd).GetInfo();
			Assert.Throws<ArgumentOutOfRangeException>(() => info.GetOpAccess(-1));
			Assert.Throws<ArgumentOutOfRangeException>(() => info.GetOpAccess(Iced.Intel.DecoderConstants.MaxOpCount));
		}

		[Fact]
		void MemorySizeExtensions_GetInfo_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)(-1)).GetInfo());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)Iced.Intel.DecoderConstants.NumberOfMemorySizes).GetInfo());
		}

		[Fact]
		void MemorySizeExtensions_GetSize_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)(-1)).GetSize());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)Iced.Intel.DecoderConstants.NumberOfMemorySizes).GetSize());
		}

		[Fact]
		void MemorySizeExtensions_GetElementSize_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)(-1)).GetElementSize());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)Iced.Intel.DecoderConstants.NumberOfMemorySizes).GetElementSize());
		}

		[Fact]
		void MemorySizeExtensions_GetElementType_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)(-1)).GetElementType());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)Iced.Intel.DecoderConstants.NumberOfMemorySizes).GetElementType());
		}

		[Fact]
		void MemorySizeExtensions_GetElementTypeInfo_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)(-1)).GetElementTypeInfo());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)Iced.Intel.DecoderConstants.NumberOfMemorySizes).GetElementTypeInfo());
		}

		[Fact]
		void MemorySizeExtensions_IsSigned_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)(-1)).IsSigned());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)Iced.Intel.DecoderConstants.NumberOfMemorySizes).IsSigned());
		}

		[Fact]
		void MemorySizeExtensions_IsPacked_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)(-1)).IsPacked());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)Iced.Intel.DecoderConstants.NumberOfMemorySizes).IsPacked());
		}

		[Fact]
		void MemorySizeExtensions_GetElementCount_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)(-1)).GetElementCount());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)Iced.Intel.DecoderConstants.NumberOfMemorySizes).GetElementCount());
		}

		[Fact]
		void MemorySizeInfo_ctor_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => new MemorySizeInfo(MemorySize.Unknown, -1, 0, MemorySize.Unknown, false, false));
			Assert.Throws<ArgumentOutOfRangeException>(() => new MemorySizeInfo(MemorySize.Unknown, 0, -1, MemorySize.Unknown, false, false));
			Assert.Throws<ArgumentOutOfRangeException>(() => new MemorySizeInfo(MemorySize.Unknown, 0, 1, MemorySize.Unknown, false, false));
		}

		[Fact]
		void RegisterExtensions_GetInfo_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Register)(-1)).GetInfo());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Register)Iced.Intel.DecoderConstants.NumberOfRegisters).GetInfo());
		}

		[Fact]
		void RegisterExtensions_GetBaseRegister_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Register)(-1)).GetBaseRegister());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Register)Iced.Intel.DecoderConstants.NumberOfRegisters).GetBaseRegister());
		}

		[Fact]
		void RegisterExtensions_GetNumber_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Register)(-1)).GetNumber());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Register)Iced.Intel.DecoderConstants.NumberOfRegisters).GetNumber());
		}

		[Fact]
		void RegisterExtensions_GetFullRegister_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Register)(-1)).GetFullRegister());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Register)Iced.Intel.DecoderConstants.NumberOfRegisters).GetFullRegister());
		}

		[Fact]
		void RegisterExtensions_GetFullRegister32_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Register)(-1)).GetFullRegister32());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Register)Iced.Intel.DecoderConstants.NumberOfRegisters).GetFullRegister32());
		}

		[Fact]
		void RegisterExtensions_GetSize_throws_if_invalid_input() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Register)(-1)).GetSize());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Register)Iced.Intel.DecoderConstants.NumberOfRegisters).GetSize());
		}
	}
}
#endif
