// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.enc;

import java.util.ArrayList;

import com.github.icedland.iced.x86.CodeWriter;

final class Block {
	final CodeWriterImpl codeWriter;
	final long rip;
	final ArrayList<RelocInfo> relocInfos;
	private Instr[] instructions;
	private final ArrayList<BlockData> dataList;
	private final long alignment;
	private final ArrayList<BlockData> validData;
	private long validDataAddress;
	private long validDataAddressAligned;

	Instr[] getInstructions() {
		return instructions;
	}

	Block(BlockEncoder blockEncoder, CodeWriter codeWriter, long rip, ArrayList<RelocInfo> relocInfos) {
		this.codeWriter = new CodeWriterImpl(codeWriter);
		this.rip = rip;
		this.relocInfos = relocInfos;
		instructions = new Instr[0];
		dataList = new ArrayList<BlockData>();
		alignment = blockEncoder.getBitness() / 8;
		validData = new ArrayList<BlockData>();
	}

	void setInstructions(Instr[] instructions) {
		this.instructions = instructions;
	}

	BlockData allocPointerLocation() {
		BlockData data = new BlockData(true);
		dataList.add(data);
		return data;
	}

	void initializeData() {
		long baseAddr;
		if (instructions.length > 0) {
			Instr instr = instructions[instructions.length - 1];
			baseAddr = instr.ip + instr.size;
		}
		else
			baseAddr = rip;
		validDataAddress = baseAddr;

		long addr = (baseAddr + alignment - 1) & ~(alignment - 1);
		validDataAddressAligned = addr;
		for (BlockData data : dataList) {
			if (!data.isValid)
				continue;
			data.__dont_use_address = addr;
			data.__dont_use_address_initd = true;
			validData.add(data);
			addr += alignment;
		}
	}

	void writeData() {
		if (validData.size() == 0)
			return;
		CodeWriterImpl codeWriter = this.codeWriter;
		int alignment = (int)(validDataAddressAligned - validDataAddress);
		for (int i = 0; i < alignment; i++)
			codeWriter.writeByte((byte)0xCC);
		ArrayList<RelocInfo> relocInfos = this.relocInfos;
		int d;
		switch ((int)this.alignment) {
		case 8:
			for (BlockData data : validData) {
				if (relocInfos != null)
					relocInfos.add(new RelocInfo(RelocKind.OFFSET64, data.getAddress()));
				d = (int)data.data;
				codeWriter.writeByte((byte)d);
				codeWriter.writeByte((byte)(d >>> 8));
				codeWriter.writeByte((byte)(d >>> 16));
				codeWriter.writeByte((byte)(d >>> 24));
				d = (int)(data.data >>> 32);
				codeWriter.writeByte((byte)d);
				codeWriter.writeByte((byte)(d >>> 8));
				codeWriter.writeByte((byte)(d >>> 16));
				codeWriter.writeByte((byte)(d >>> 24));
			}
			break;

		default:
			throw new UnsupportedOperationException();
		}
	}

	boolean canAddRelocInfos() {
		return relocInfos != null;
	}

	void addRelocInfo(RelocInfo relocInfo) {
		if (relocInfos != null)
			relocInfos.add(relocInfo);
	}
}
