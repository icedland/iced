/*
    Copyright (C) 2018 de4dot@gmail.com

    This file is part of Iced.

    Iced is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Iced is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Iced.  If not, see <https://www.gnu.org/licenses/>.
*/

#if !NO_ENCODER
using System;
using System.Collections.Generic;

namespace Iced.Intel.BlockEncoderInternal {
	sealed class Block {
		public readonly CodeWriterImpl CodeWriter;
		public readonly ulong RIP;
		readonly IList<RelocInfo> relocInfos;
		public readonly uint[] NewInstructionOffsets;
		public readonly ConstantOffsets[] ConstantOffsets;
		public readonly Instr[] Instructions;
		readonly List<BlockData> dataList;
		readonly ulong alignment;
		readonly List<BlockData> validData;
		ulong validDataAddress;
		ulong validDataAddressAligned;

		public Block(BlockEncoder blockEncoder, CodeWriter codeWriter, ulong rip, IList<RelocInfo> relocInfos, uint[] newInstructionOffsets, ConstantOffsets[] constantOffsets, Instr[] instructions) {
			CodeWriter = new CodeWriterImpl(codeWriter);
			RIP = rip;
			this.relocInfos = relocInfos;
			NewInstructionOffsets = newInstructionOffsets;
			ConstantOffsets = constantOffsets;
			Instructions = instructions ?? throw new ArgumentNullException(nameof(instructions));
			dataList = new List<BlockData>();
			alignment = (uint)blockEncoder.Bitness / 8;
			validData = new List<BlockData>();
			foreach (var instr in instructions)
				instr.Block = this;
		}

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

		public bool CanAddRelocInfos => relocInfos != null;
		public void AddRelocInfo(RelocInfo relocInfo) => relocInfos?.Add(relocInfo);
	}

	sealed class BlockData {
		internal ulong __dont_use_address;
		internal bool __dont_use_address_initd;

		public bool IsValid;

		public ulong Address {
			get {
				if (!IsValid)
					throw new InvalidOperationException();
				if (!__dont_use_address_initd)
					throw new InvalidOperationException();
				return __dont_use_address;
			}
		}

		public ulong Data;
	}
}
#endif
