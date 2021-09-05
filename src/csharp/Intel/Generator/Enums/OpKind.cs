// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Enums {
	[Enum("OpKind", Documentation = "Instruction operand kind", Public = true)]
	enum OpKind {
		[Comment("A register (#(r:Iced.Intel.Register)#).#(p:)#This operand kind uses #(P:Instruction.Op0Register)#, #(P:Instruction.Op1Register)#, #(P:Instruction.Op2Register)#, #(P:Instruction.Op3Register)# or #(P:Instruction.Op4Register)# depending on operand number. See also #(M:Instruction.GetOpRegister)#.")]
		Register,
		[Comment("Near 16-bit branch. This operand kind uses #(P:Instruction.NearBranch16)#")]
		NearBranch16,
		[Comment("Near 32-bit branch. This operand kind uses #(P:Instruction.NearBranch32)#")]
		NearBranch32,
		[Comment("Near 64-bit branch. This operand kind uses #(P:Instruction.NearBranch64)#")]
		NearBranch64,
		[Comment("Far 16-bit branch. This operand kind uses #(P:Instruction.FarBranch16)# and #(P:Instruction.FarBranchSelector)#")]
		FarBranch16,
		[Comment("Far 32-bit branch. This operand kind uses #(P:Instruction.FarBranch32)# and #(P:Instruction.FarBranchSelector)#")]
		FarBranch32,
		[Comment("8-bit constant. This operand kind uses #(P:Instruction.Immediate8)#")]
		Immediate8,
		[Comment("8-bit constant used by the #(c:ENTER)#, #(c:EXTRQ)#, #(c:INSERTQ)# instructions. This operand kind uses #(P:Instruction.Immediate8_2nd)#")]
		Immediate8_2nd,
		[Comment("16-bit constant. This operand kind uses #(P:Instruction.Immediate16)#")]
		Immediate16,
		[Comment("32-bit constant. This operand kind uses #(P:Instruction.Immediate32)#")]
		Immediate32,
		[Comment("64-bit constant. This operand kind uses #(P:Instruction.Immediate64)#")]
		Immediate64,
		[Comment("An 8-bit value sign extended to 16 bits. This operand kind uses #(P:Instruction.Immediate8to16)#")]
		Immediate8to16,
		[Comment("An 8-bit value sign extended to 32 bits. This operand kind uses #(P:Instruction.Immediate8to32)#")]
		Immediate8to32,
		[Comment("An 8-bit value sign extended to 64 bits. This operand kind uses #(P:Instruction.Immediate8to64)#")]
		Immediate8to64,
		[Comment("A 32-bit value sign extended to 64 bits. This operand kind uses #(P:Instruction.Immediate32to64)#")]
		Immediate32to64,
		[Comment("#(c:seg:[SI])#. This operand kind uses #(P:Instruction.MemorySize)#, #(P:Instruction.MemorySegment)#, #(P:Instruction.SegmentPrefix)#")]
		MemorySegSI,
		[Comment("#(c:seg:[ESI])#. This operand kind uses #(P:Instruction.MemorySize)#, #(P:Instruction.MemorySegment)#, #(P:Instruction.SegmentPrefix)#")]
		MemorySegESI,
		[Comment("#(c:seg:[RSI])#. This operand kind uses #(P:Instruction.MemorySize)#, #(P:Instruction.MemorySegment)#, #(P:Instruction.SegmentPrefix)#")]
		MemorySegRSI,
		[Comment("#(c:seg:[DI])#. This operand kind uses #(P:Instruction.MemorySize)#, #(P:Instruction.MemorySegment)#, #(P:Instruction.SegmentPrefix)#")]
		MemorySegDI,
		[Comment("#(c:seg:[EDI])#. This operand kind uses #(P:Instruction.MemorySize)#, #(P:Instruction.MemorySegment)#, #(P:Instruction.SegmentPrefix)#")]
		MemorySegEDI,
		[Comment("#(c:seg:[RDI])#. This operand kind uses #(P:Instruction.MemorySize)#, #(P:Instruction.MemorySegment)#, #(P:Instruction.SegmentPrefix)#")]
		MemorySegRDI,
		[Comment("#(c:ES:[DI])#. This operand kind uses #(P:Instruction.MemorySize)#")]
		MemoryESDI,
		[Comment("#(c:ES:[EDI])#. This operand kind uses #(P:Instruction.MemorySize)#")]
		MemoryESEDI,
		[Comment("#(c:ES:[RDI])#. This operand kind uses #(P:Instruction.MemorySize)#")]
		MemoryESRDI,
		[Comment("Memory operand.#(p:)#This operand kind uses #(P:Instruction.MemoryDisplSize)#, #(P:Instruction.MemorySize)#, #(P:Instruction.MemoryIndexScale)#, #(P:Instruction.MemoryDisplacement64)#, #(P:Instruction.MemoryBase)#, #(P:Instruction.MemoryIndex)#, #(P:Instruction.MemorySegment)#, #(P:Instruction.SegmentPrefix)#")]
		Memory,
	}
}
