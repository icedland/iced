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
using System.IO;
using Xunit;

namespace Iced.UnitTests.Intel.AssemblerTests {
	using Iced.Intel;
	
	public abstract class AssemblerTestsBase {
		int _bitness;
		
		protected AssemblerTestsBase(int bitness) {
			_bitness = bitness;
		}
		
		public int Bitness => _bitness;
		
		protected void TestAssembler(Action<Assembler> fAsm, Func<Instruction, bool> fIns, LocalOpCodeFlags flags = LocalOpCodeFlags.None) {
			var stream = new MemoryStream();
			var assembler = Assembler.Create(_bitness, new StreamCodeWriter(stream));
			
			// Encode the instruction
			if ((flags & LocalOpCodeFlags.PreferVex) != 0) {
				assembler.PreferVex = true;
			}
			else if ((flags & LocalOpCodeFlags.PreferEvex) != 0) {
				assembler.PreferVex = false;
			}
			if ((flags & LocalOpCodeFlags.PreferBranchShort) != 0) {
				assembler.PreferBranchShort = true;
			}
			else if ((flags & LocalOpCodeFlags.PreferBranchNear) != 0) {
				assembler.PreferBranchShort = false;
			}
			fAsm(assembler);
			
			// Expecting only one instruction
			Assert.Equal(1, assembler.Instructions.Count);

			// Check that the instruction is the one expected
			var inst = assembler.Instructions[0];
			Assert.True(fIns(inst));
			
			// Encode the instruction
			assembler.Encode(BlockEncoderOptions.DontFixBranches);

			// Special for decoding options
			DecoderOptions decoderOptions = DecoderOptions.None;
			switch (inst.Code) {

			case Code.Umov_rm8_r8:
			case Code.Umov_rm16_r16:
			case Code.Umov_rm32_r32:
			case Code.Umov_r8_rm8:
			case Code.Umov_r16_rm16:
			case Code.Umov_r32_rm32:
				decoderOptions = DecoderOptions.Umov;
				break;
			case Code.Xbts_r16_rm16:
			case Code.Xbts_r32_rm32:
			case Code.Ibts_rm16_r16:
			case Code.Ibts_rm32_r32:
				decoderOptions = DecoderOptions.Xbts;
				break;
			case Code.Frstpm:
			case Code.Fstdw_AX:
			case Code.Fstsg_AX:
				decoderOptions = DecoderOptions.OldFpu;
				break;
			case Code.Pcommit:
				decoderOptions = DecoderOptions.Pcommit;
				break;
			case Code.Loadall386:
				decoderOptions = DecoderOptions.Loadall386;
				break;
			case Code.Cl1invmb:
				decoderOptions = DecoderOptions.Cl1invmb;
				break;
			case Code.Mov_r32_tr:
			case Code.Mov_tr_r32:
				decoderOptions = DecoderOptions.MovTr;
				break;
			case Code.Jmpe_rm16:
			case Code.Jmpe_rm32:
			case Code.Jmpe_disp16:
			case Code.Jmpe_disp32:
				decoderOptions = DecoderOptions.Jmpe;
				break;
			}

			// Check decoding back against the original instruction
			stream.Position = 0;
			var decoder = Decoder.Create(_bitness, new StreamCodeReader(stream), decoderOptions);
			var againstInst = decoder.Decode();
			if ((flags & LocalOpCodeFlags.Fwait) != 0) {
				Assert.Equal(againstInst, Instruction.Create(Code.Wait));
				againstInst = decoder.Decode();

				switch (againstInst.Code)
				{
				case Code.Fnstenv_m14byte:
					againstInst.Code = Code.Fstenv_m14byte;
					break;
                case Code.Fnstenv_m28byte:
					againstInst.Code = Code.Fstenv_m28byte;
					break;
                case Code.Fnstcw_m2byte:
					againstInst.Code = Code.Fstcw_m2byte;
					break;
                case Code.Fneni:
					againstInst.Code = Code.Feni;
					break;
                case Code.Fndisi:
					againstInst.Code = Code.Fdisi;
					break;
                case Code.Fnclex:
					againstInst.Code = Code.Fclex;
					break;
                case Code.Fninit:
					againstInst.Code = Code.Finit;
					break;
                case Code.Fnsetpm:
					againstInst.Code = Code.Fsetpm;
					break;
                case Code.Fnsave_m94byte:
					againstInst.Code = Code.Fsave_m94byte;
					break;
                case Code.Fnsave_m108byte:	
					againstInst.Code = Code.Fsave_m108byte;
					break;
                case Code.Fnstsw_m2byte:
					againstInst.Code = Code.Fstsw_m2byte;
					break;
                case Code.Fnstsw_AX:
					againstInst.Code = Code.Fstsw_AX;
					break;
				}
			}

			// Reset IP to 0 when matching against decode
			if ((flags & LocalOpCodeFlags.Branch) != 0) {
				inst.NearBranch32 = 0;
			}

			Assert.Equal(inst , againstInst);
		}

		protected Label CreateAndEmitLabel(Assembler c) {
			var label = c.CreateLabel();
			c.Label(label);
			return label;
		}

		protected Instruction AssignLabel(Instruction instruction, ulong value) {
			instruction.IP = value;
			return instruction;
		}

		[Flags]
		protected enum LocalOpCodeFlags {
			None = 0,
			Fwait = 1 << 0,
			PreferVex = 1 << 1,
			PreferEvex = 1 << 2,			
			PreferBranchShort = 1 << 3,
			PreferBranchNear = 1 << 4,
			Branch = 1 << 5,
		}
		
		sealed class StreamCodeWriter : CodeWriter {
			readonly Stream _stream;

			public StreamCodeWriter(Stream stream) {
				_stream = stream;
			}

			public override void WriteByte(byte value) {
				_stream.WriteByte(value);
			}
		}
		
		sealed class StreamCodeReader : CodeReader {
			readonly Stream _stream;

			public StreamCodeReader(Stream stream) {
				_stream = stream;
			}

			public override int ReadByte() {
				return _stream.ReadByte();
			}
		}
	}
}
