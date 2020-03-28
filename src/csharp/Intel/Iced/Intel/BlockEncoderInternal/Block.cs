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

#if ENCODER && BLOCK_ENCODER
using System;
using System.Collections.Generic;

namespace Iced.Intel.BlockEncoderInternal {
	sealed class Block {
		public readonly CodeWriterImpl CodeWriter;
		public readonly ulong RIP;
		public readonly List<RelocInfo>? relocInfos;
		public Instr[] Instructions => instructions;
		Instr[] instructions;
		readonly List<BlockData> dataList;
		readonly ulong alignment;
		readonly List<BlockData> validData;
		ulong validDataAddress;
		ulong validDataAddressAligned;

		public Block(BlockEncoder blockEncoder, CodeWriter codeWriter, ulong rip, List<RelocInfo>? relocInfos) {
			CodeWriter = new CodeWriterImpl(codeWriter);
			RIP = rip;
			this.relocInfos = relocInfos;
			instructions = Array2.Empty<Instr>();
			dataList = new List<BlockData>();
			alignment = (uint)blockEncoder.Bitness / 8;
			validData = new List<BlockData>();
		}

		internal void SetInstructions(Instr[] instructions) => this.instructions = instructions;

		public BlockData AllocPointerLocation() {
			var data = new BlockData { IsValid = true };
			dataList.Add(data);
			return data;
		}

		public void InitializeData() {
			ulong baseAddr;
			if (Instructions.Length > 0) {
				var instr = Instructions[Instructions.Length - 1];
				baseAddr = instr.IP + instr.Size;
			}
			else
				baseAddr = RIP;
			validDataAddress = baseAddr;

			ulong addr = (baseAddr + alignment - 1) & ~(alignment - 1);
			validDataAddressAligned = addr;
			foreach (var data in dataList) {
				if (!data.IsValid)
					continue;
				data.__dont_use_address = addr;
				data.__dont_use_address_initd = true;
				validData.Add(data);
				addr += alignment;
			}
		}

		public void WriteData() {
			if (validData.Count == 0)
				return;
			var codeWriter = CodeWriter;
			int alignment = (int)(validDataAddressAligned - validDataAddress);
			for (int i = 0; i < alignment; i++)
				codeWriter.WriteByte(0xCC);
			var relocInfos = this.relocInfos;
			uint d;
			switch ((int)this.alignment) {
			case 8:
				foreach (var data in validData) {
					relocInfos?.Add(new RelocInfo(RelocKind.Offset64, data.Address));
					d = (uint)data.Data;
					codeWriter.WriteByte((byte)d);
					codeWriter.WriteByte((byte)(d >> 8));
					codeWriter.WriteByte((byte)(d >> 16));
					codeWriter.WriteByte((byte)(d >> 24));
					d = (uint)(data.Data >> 32);
					codeWriter.WriteByte((byte)d);
					codeWriter.WriteByte((byte)(d >> 8));
					codeWriter.WriteByte((byte)(d >> 16));
					codeWriter.WriteByte((byte)(d >> 24));
				}
				break;

			default:
				throw new InvalidOperationException();
			}
		}

		public bool CanAddRelocInfos => !(relocInfos is null);
		public void AddRelocInfo(RelocInfo relocInfo) => relocInfos?.Add(relocInfo);
	}

	sealed class BlockData {
		internal ulong __dont_use_address;
		internal bool __dont_use_address_initd;

		public bool IsValid;

		public ulong Address {
			get {
				if (!IsValid)
					ThrowHelper.ThrowInvalidOperationException();
				if (!__dont_use_address_initd)
					ThrowHelper.ThrowInvalidOperationException();
				return __dont_use_address;
			}
		}

		public ulong Data;
	}
}
#endif
