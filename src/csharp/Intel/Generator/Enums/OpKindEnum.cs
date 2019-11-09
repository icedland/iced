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

namespace Generator.Enums {
	static class OpKindEnum {
		const string documentation = "Instruction operand kind";

		static EnumValue[] GetValues() =>
			new EnumValue[] {
				new EnumValue("Register", "A register (#(r:Iced.Intel.Register)#).#(p:)#This operand kind uses #(P:Instruction.Op0Register)#, #(P:Instruction.Op1Register)#, #(P:Instruction.Op2Register)#, #(P:Instruction.Op3Register)# or #(P:Instruction.Op4Register)# depending on operand number. See also #(M:Instruction.GetOpRegister)#."),
				new EnumValue("NearBranch16", "Near 16-bit branch. This operand kind uses #(P:Instruction.NearBranch16)#"),
				new EnumValue("NearBranch32", "Near 32-bit branch. This operand kind uses #(P:Instruction.NearBranch32)#"),
				new EnumValue("NearBranch64", "Near 64-bit branch. This operand kind uses #(P:Instruction.NearBranch64)#"),
				new EnumValue("FarBranch16", "Far 16-bit branch. This operand kind uses #(P:Instruction.FarBranch16)# and #(P:Instruction.FarBranchSelector)#"),
				new EnumValue("FarBranch32", "Far 32-bit branch. This operand kind uses #(P:Instruction.FarBranch32)# and #(P:Instruction.FarBranchSelector)#"),
				new EnumValue("Immediate8", "8-bit constant. This operand kind uses #(P:Instruction.Immediate8)#"),
				new EnumValue("Immediate8_2nd", "8-bit constant used by the #(c:enter)#, #(c:extrq)#, #(c:insertq)# instructions. This operand kind uses #(P:Instruction.Immediate8_2nd)#"),
				new EnumValue("Immediate16", "16-bit constant. This operand kind uses #(P:Instruction.Immediate16)#"),
				new EnumValue("Immediate32", "32-bit constant. This operand kind uses #(P:Instruction.Immediate32)#"),
				new EnumValue("Immediate64", "64-bit constant. This operand kind uses #(P:Instruction.Immediate64)#"),
				new EnumValue("Immediate8to16", "An 8-bit value sign extended to 16 bits. This operand kind uses #(P:Instruction.Immediate8to16)#"),
				new EnumValue("Immediate8to32", "An 8-bit value sign extended to 32 bits. This operand kind uses #(P:Instruction.Immediate8to32)#"),
				new EnumValue("Immediate8to64", "An 8-bit value sign extended to 64 bits. This operand kind uses #(P:Instruction.Immediate8to64)#"),
				new EnumValue("Immediate32to64", "A 32-bit value sign extended to 64 bits. This operand kind uses #(P:Instruction.Immediate32to64)#"),
				new EnumValue("MemorySegSI", "#(c:seg:[si])#. This operand kind uses #(P:Instruction.MemorySize)#, #(P:Instruction.MemorySegment)#, #(P:Instruction.SegmentPrefix)#"),
				new EnumValue("MemorySegESI", "#(c:seg:[esi])#. This operand kind uses #(P:Instruction.MemorySize)#, #(P:Instruction.MemorySegment)#, #(P:Instruction.SegmentPrefix)#"),
				new EnumValue("MemorySegRSI", "#(c:seg:[rsi])#. This operand kind uses #(P:Instruction.MemorySize)#, #(P:Instruction.MemorySegment)#, #(P:Instruction.SegmentPrefix)#"),
				new EnumValue("MemorySegDI", "#(c:seg:[di])#. This operand kind uses #(P:Instruction.MemorySize)#, #(P:Instruction.MemorySegment)#, #(P:Instruction.SegmentPrefix)#"),
				new EnumValue("MemorySegEDI", "#(c:seg:[edi])#. This operand kind uses #(P:Instruction.MemorySize)#, #(P:Instruction.MemorySegment)#, #(P:Instruction.SegmentPrefix)#"),
				new EnumValue("MemorySegRDI", "#(c:seg:[rdi])#. This operand kind uses #(P:Instruction.MemorySize)#, #(P:Instruction.MemorySegment)#, #(P:Instruction.SegmentPrefix)#"),
				new EnumValue("MemoryESDI", "#(c:es:[di])#. This operand kind uses #(P:Instruction.MemorySize)#"),
				new EnumValue("MemoryESEDI", "#(c:es:[edi])#. This operand kind uses #(P:Instruction.MemorySize)#"),
				new EnumValue("MemoryESRDI", "#(c:es:[rdi])#. This operand kind uses #(P:Instruction.MemorySize)#"),
				new EnumValue("Memory64", "64-bit offset #(c:[xxxxxxxxxxxxxxxx])#. This operand kind uses #(P:Instruction.MemoryAddress64)#, #(P:Instruction.MemorySegment)#, #(P:Instruction.SegmentPrefix)#, #(P:Instruction.MemorySize)#"),
				new EnumValue("Memory", "Memory operand.#(p:)#This operand kind uses #(P:Instruction.MemoryDisplSize)#, #(P:Instruction.MemorySize)#, #(P:Instruction.MemoryIndexScale)#, #(P:Instruction.MemoryDisplacement)#, #(P:Instruction.MemoryBase)#, #(P:Instruction.MemoryIndex)#, #(P:Instruction.MemorySegment)#, #(P:Instruction.SegmentPrefix)#"),
			};

		public static readonly EnumType Instance = new EnumType(EnumKind.OpKind, documentation, GetValues(), EnumTypeFlags.Public);
	}
}
