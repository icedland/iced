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
	}
}
#endif
