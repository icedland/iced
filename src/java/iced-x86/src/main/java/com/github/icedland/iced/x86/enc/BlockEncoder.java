// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.enc;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.List;

import com.github.icedland.iced.x86.Code;
import com.github.icedland.iced.x86.CodeWriter;
import com.github.icedland.iced.x86.ConstantOffsets;
import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.internal.IcedConstants;

/**
 * Encodes instructions. It can be used to move instructions from one location to another location.
 */
public final class BlockEncoder {
	private final int bitness;
	private final int options;
	private final Block[] blocks;
	private final Encoder nullEncoder;
	private final HashMap<Long, Instr> toInstr;

	int getBitness() {
		return bitness;
	}

	boolean fixBranches() {
		return (options & BlockEncoderOptions.DONT_FIX_BRANCHES) == 0;
	}

	private boolean returnRelocInfos() {
		return (options & BlockEncoderOptions.RETURN_RELOC_INFOS) != 0;
	}

	private boolean returnNewInstructionOffsets() {
		return (options & BlockEncoderOptions.RETURN_NEW_INSTRUCTION_OFFSETS) != 0;
	}

	private boolean returnConstantOffsets() {
		return (options & BlockEncoderOptions.RETURN_CONSTANT_OFFSETS) != 0;
	}

	private boolean returnAllConstantOffsets() {
		return (options & BlockEncoderOptions.RETURN_ALL_NEW_INSTRUCTION_OFFSETS) != 0;
	}

	private static final class NullCodeWriter implements CodeWriter {
		public static final NullCodeWriter instance = new NullCodeWriter();

		NullCodeWriter() {
		}

		@Override
		public void writeByte(byte value) {
		}
	}

	BlockEncoder(int bitness, InstructionBlock[] instrBlocks, int options) {
		if (bitness != 16 && bitness != 32 && bitness != 64)
			throw new IllegalArgumentException("bitness");
		if (instrBlocks == null)
			throw new NullPointerException("instrBlocks");
		this.bitness = bitness;
		nullEncoder = new Encoder(bitness, NullCodeWriter.instance);
		this.options = options;

		blocks = new Block[instrBlocks.length];
		int instrCount = 0;
		for (int i = 0; i < instrBlocks.length; i++) {
			List<Instruction> instructions = instrBlocks[i].instructions;
			if (instructions == null)
				throw new IllegalArgumentException();
			Block block = new Block(this, instrBlocks[i].codeWriter, instrBlocks[i].rip, returnRelocInfos() ? new ArrayList<RelocInfo>() : null);
			blocks[i] = block;
			Instr[] instrs = new Instr[instructions.size()];
			long ip = instrBlocks[i].rip;
			for (int j = 0; j < instrs.length; j++) {
				Instruction instruction = instructions.get(j);
				Instr instr = Instr.create(this, block, instruction);
				instr.ip = ip;
				instrs[j] = instr;
				instrCount++;
				assert instr.size != 0 || instruction.getCode() == Code.ZERO_BYTES;
				ip += instr.size;
			}
			block.setInstructions(instrs);
		}
		// Optimize from low to high addresses
		Arrays.sort(blocks, (a, b) -> Long.compareUnsigned(a.rip, b.rip));

		// There must not be any instructions with the same IP, except if IP = 0 (default value)
		HashMap<Long, Instr> toInstr = new HashMap<Long, Instr>(instrCount);
		this.toInstr = toInstr;
		boolean hasMultipleZeroIPInstrs = false;
		for (Block block : blocks) {
			for (Instr instr : block.getInstructions()) {
				long origIP = instr.origIP;
				if (toInstr.containsKey(origIP)) {
					if (origIP != 0)
						throw new IllegalArgumentException(String.format("Multiple instructions with the same IP: 0x%X", origIP));
					hasMultipleZeroIPInstrs = true;
				}
				else
					toInstr.put(origIP, instr);
			}
		}
		if (hasMultipleZeroIPInstrs)
			toInstr.remove(0L);

		for (Block block : blocks) {
			long ip = block.rip;
			for (Instr instr : block.getInstructions()) {
				instr.ip = ip;
				if (!instr.done)
					instr.initialize(this);
				ip += instr.size;
			}
		}
	}

	/**
	 * Encodes instructions. Any number of branches can be part of this block.
	 * <p>
	 * You can use this function to move instructions from one location to another location.
	 * <p>
	 * If the target of a branch is too far away, it'll be rewritten to a longer branch.
	 * You can disable this by passing in {@link BlockEncoderOptions#DONT_FIX_BRANCHES}.
	 * <p>
	 * If the block has any {@code RIP}-relative memory operands, make sure the data isn't too
	 * far away from the new location of the encoded instructions. Every OS should have
	 * some API to allocate memory close (+/-2GB) to the original code location.
	 *
	 * @param bitness 16, 32 or 64
	 * @param block   All instructions
	 * @return Object On success, it returns a {@link BlockEncoderResult}, and on failure, it returns a {@link String} with the error message.
	 */
	public static Object tryEncode(int bitness, InstructionBlock block) {
		return tryEncode(bitness, block, BlockEncoderOptions.NONE);
	}

	/**
	 * Encodes instructions. Any number of branches can be part of this block.
	 * <p>
	 * You can use this function to move instructions from one location to another location.
	 * <p>
	 * If the target of a branch is too far away, it'll be rewritten to a longer branch.
	 * You can disable this by passing in {@link BlockEncoderOptions#DONT_FIX_BRANCHES}.
	 * <p>
	 * If the block has any {@code RIP}-relative memory operands, make sure the data isn't too
	 * far away from the new location of the encoded instructions. Every OS should have
	 * some API to allocate memory close (+/-2GB) to the original code location.
	 *
	 * @param bitness 16, 32 or 64
	 * @param block   All instructions
	 * @param options Encoder options (a {@link BlockEncoderOptions} flags value)
	 * @return Object On success, it returns a {@link BlockEncoderResult}, and on failure, it returns a {@link String} with the error message.
	 */
	public static Object tryEncode(int bitness, InstructionBlock block, int options) {
		Object result = tryEncode(bitness, new InstructionBlock[] { block }, options);
		if (result instanceof BlockEncoderResult[]) {
			BlockEncoderResult[] resultArray = (BlockEncoderResult[])result;
			assert resultArray.length == 1 : resultArray.length;
			return resultArray[0];
		}
		if (result instanceof String)
			return (String)result;
		throw new UnsupportedOperationException();
	}

	/**
	 * Encodes instructions. Any number of branches can be part of this block.
	 * <p>
	 * You can use this function to move instructions from one location to another location.
	 * <p>
	 * If the target of a branch is too far away, it'll be rewritten to a longer branch.
	 * You can disable this by passing in {@link BlockEncoderOptions#DONT_FIX_BRANCHES}.
	 * <p>
	 * If the block has any {@code RIP}-relative memory operands, make sure the data isn't too
	 * far away from the new location of the encoded instructions. Every OS should have
	 * some API to allocate memory close (+/-2GB) to the original code location.
	 *
	 * @param bitness 16, 32 or 64
	 * @param blocks  All instructions
	 * @return Object On success, it returns an array of {@link BlockEncoderResult}, and on failure, it returns a {@link String} with the error
	 *         message.
	 */
	public static Object tryEncode(int bitness, InstructionBlock[] blocks) {
		return tryEncode(bitness, blocks, BlockEncoderOptions.NONE);
	}

	/**
	 * Encodes instructions. Any number of branches can be part of this block.
	 * <p>
	 * You can use this function to move instructions from one location to another location.
	 * <p>
	 * If the target of a branch is too far away, it'll be rewritten to a longer branch.
	 * You can disable this by passing in {@link BlockEncoderOptions#DONT_FIX_BRANCHES}.
	 * <p>
	 * If the block has any {@code RIP}-relative memory operands, make sure the data isn't too
	 * far away from the new location of the encoded instructions. Every OS should have
	 * some API to allocate memory close (+/-2GB) to the original code location.
	 *
	 * @param bitness 16, 32 or 64
	 * @param blocks  All instructions
	 * @param options Encoder options (a {@link BlockEncoderOptions} flags value)
	 * @return Object On success, it returns an array of {@link BlockEncoderResult}, and on failure, it returns a {@link String} with the error
	 *         message.
	 */
	public static Object tryEncode(int bitness, InstructionBlock[] blocks, int options) {
		return new BlockEncoder(bitness, blocks, options).encode();
	}

	private Object encode() {
		final int MAX_ITERS = 5;
		for (int iter = 0; iter < MAX_ITERS; iter++) {
			boolean updated = false;
			for (Block block : blocks) {
				long ip = block.rip;
				long gained = 0;
				for (Instr instr : block.getInstructions()) {
					instr.ip = ip;
					if (!instr.done) {
						int oldSize = instr.size;
						if (instr.optimize(gained)) {
							if (instr.size > oldSize)
								return "Internal error: new size > old size";
							if (instr.size < oldSize) {
								gained += oldSize - instr.size;
								updated = true;
							}
						}
						else if (instr.size != oldSize)
							return "Internal error: new size != old size";
					}
					ip += instr.size;
				}
			}
			if (!updated)
				break;
		}

		for (Block block : blocks)
			block.initializeData();

		BlockEncoderResult[] resultArray = new BlockEncoderResult[blocks.length];
		TryEncodeResult tryEncResult = new TryEncodeResult();
		boolean returnAllOffsets = returnAllConstantOffsets();
		for (int i = 0; i < blocks.length; i++) {
			Block block = blocks[i];
			Encoder encoder = new Encoder(bitness, block.codeWriter);
			long ip = block.rip;
			int[] newInstructionOffsets = returnNewInstructionOffsets() ? new int[block.getInstructions().length] : null;
			ConstantOffsets[] constantOffsets = returnConstantOffsets() ? new ConstantOffsets[block.getInstructions().length] : null;
			Instr[] instructions = block.getInstructions();
			for (int j = 0; j < instructions.length; j++) {
				Instr instr = instructions[j];
				int bytesWritten = block.codeWriter.bytesWritten;
				String errorMessage = instr.tryEncode(encoder, tryEncResult);
				if (errorMessage != null)
					return errorMessage;
				if (constantOffsets != null)
					constantOffsets[j] = tryEncResult.constantOffsets;
				int size = block.codeWriter.bytesWritten - bytesWritten;
				if (size != instr.size)
					return "Internal error: didn't write all bytes";
				if (newInstructionOffsets != null) {
					if (tryEncResult.isOriginalInstruction || returnAllOffsets)
						newInstructionOffsets[j] = (int)(ip - block.rip);
					else
						newInstructionOffsets[j] = 0xFFFF_FFFF;
				}
				ip += size;
			}
			resultArray[i] = new BlockEncoderResult(block.rip, block.relocInfos, newInstructionOffsets, constantOffsets);
			block.writeData();
		}

		return resultArray;
	}

	TargetInstr getTarget(long address) {
		Instr instr = toInstr.get(address);
		if (instr != null)
			return new TargetInstr(instr);
		return new TargetInstr(address);
	}

	int getInstructionSize(Instruction instruction, long ip) {
		Object result = nullEncoder.tryEncode(instruction, ip);
		if (result instanceof Integer)
			return ((Integer)result).intValue();
		return IcedConstants.MAX_INSTRUCTION_LENGTH;
	}
}
