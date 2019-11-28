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

using System.Linq;

namespace Generator.Enums {
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
		[Comment("8-bit constant used by the #(c:enter)#, #(c:extrq)#, #(c:insertq)# instructions. This operand kind uses #(P:Instruction.Immediate8_2nd)#")]
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
		[Comment("#(c:seg:[si])#. This operand kind uses #(P:Instruction.MemorySize)#, #(P:Instruction.MemorySegment)#, #(P:Instruction.SegmentPrefix)#")]
		MemorySegSI,
		[Comment("#(c:seg:[esi])#. This operand kind uses #(P:Instruction.MemorySize)#, #(P:Instruction.MemorySegment)#, #(P:Instruction.SegmentPrefix)#")]
		MemorySegESI,
		[Comment("#(c:seg:[rsi])#. This operand kind uses #(P:Instruction.MemorySize)#, #(P:Instruction.MemorySegment)#, #(P:Instruction.SegmentPrefix)#")]
		MemorySegRSI,
		[Comment("#(c:seg:[di])#. This operand kind uses #(P:Instruction.MemorySize)#, #(P:Instruction.MemorySegment)#, #(P:Instruction.SegmentPrefix)#")]
		MemorySegDI,
		[Comment("#(c:seg:[edi])#. This operand kind uses #(P:Instruction.MemorySize)#, #(P:Instruction.MemorySegment)#, #(P:Instruction.SegmentPrefix)#")]
		MemorySegEDI,
		[Comment("#(c:seg:[rdi])#. This operand kind uses #(P:Instruction.MemorySize)#, #(P:Instruction.MemorySegment)#, #(P:Instruction.SegmentPrefix)#")]
		MemorySegRDI,
		[Comment("#(c:es:[di])#. This operand kind uses #(P:Instruction.MemorySize)#")]
		MemoryESDI,
		[Comment("#(c:es:[edi])#. This operand kind uses #(P:Instruction.MemorySize)#")]
		MemoryESEDI,
		[Comment("#(c:es:[rdi])#. This operand kind uses #(P:Instruction.MemorySize)#")]
		MemoryESRDI,
		[Comment("64-bit offset #(c:[xxxxxxxxxxxxxxxx])#. This operand kind uses #(P:Instruction.MemoryAddress64)#, #(P:Instruction.MemorySegment)#, #(P:Instruction.SegmentPrefix)#, #(P:Instruction.MemorySize)#")]
		Memory64,
		[Comment("Memory operand.#(p:)#This operand kind uses #(P:Instruction.MemoryDisplSize)#, #(P:Instruction.MemorySize)#, #(P:Instruction.MemoryIndexScale)#, #(P:Instruction.MemoryDisplacement)#, #(P:Instruction.MemoryBase)#, #(P:Instruction.MemoryIndex)#, #(P:Instruction.MemorySegment)#, #(P:Instruction.SegmentPrefix)#")]
		Memory,
	}

	static class OpKindEnum {
		const string documentation = "Instruction operand kind";

		static EnumValue[] GetValues() =>
			typeof(OpKind).GetFields().Where(a => a.IsLiteral).Select(a => new EnumValue((uint)(OpKind)a.GetValue(null)!, a.Name, CommentAttribute.GetDocumentation(a))).ToArray();

		public static readonly EnumType Instance = new EnumType(TypeIds.OpKind, documentation, GetValues(), EnumTypeFlags.Public);
	}
}
