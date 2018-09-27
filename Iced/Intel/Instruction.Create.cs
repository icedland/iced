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
using System.Diagnostics;

namespace Iced.Intel {
	partial struct Instruction {
		static OpKind GetImmediateOpKind(Code code, int operand) {
			var handlers = EncoderInternal.OpCodeHandlers.Handlers;
			if ((uint)code >= (uint)handlers.Length)
				throw new ArgumentOutOfRangeException(nameof(code));
			var operands = handlers[(int)code].Operands;
			if ((uint)operand >= (uint)operands.Length)
				throw new ArgumentOutOfRangeException(nameof(operand), $"{code} doesn't have at least {operand + 1} operands");
			var opKind = operands[operand].GetImmediateOpKind();
			if (opKind == (OpKind)(-1))
				throw new ArgumentException($"{code}'s op{operand} isn't an immediate operand");
			return opKind;
		}

		static OpKind GetNearBranchOpKind(Code code, int operand) {
			var handlers = EncoderInternal.OpCodeHandlers.Handlers;
			if ((uint)code >= (uint)handlers.Length)
				throw new ArgumentOutOfRangeException(nameof(code));
			var operands = handlers[(int)code].Operands;
			if ((uint)operand >= (uint)operands.Length)
				throw new ArgumentOutOfRangeException(nameof(operand), $"{code} doesn't have at least {operand + 1} operands");
			var opKind = operands[operand].GetNearBranchOpKind();
			if (opKind == (OpKind)(-1))
				throw new ArgumentException($"{code}'s op{operand} isn't a near branch operand");
			return opKind;
		}

		static OpKind GetFarBranchOpKind(Code code, int operand) {
			var handlers = EncoderInternal.OpCodeHandlers.Handlers;
			if ((uint)code >= (uint)handlers.Length)
				throw new ArgumentOutOfRangeException(nameof(code));
			var operands = handlers[(int)code].Operands;
			if ((uint)operand >= (uint)operands.Length)
				throw new ArgumentOutOfRangeException(nameof(operand), $"{code} doesn't have at least {operand + 1} operands");
			var opKind = operands[operand].GetFarBranchOpKind();
			if (opKind == (OpKind)(-1))
				throw new ArgumentException($"{code}'s op{operand} isn't a far branch operand");
			return opKind;
		}

		/// <summary>
		/// Creates a new <see cref="Instruction"/> with no operands
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		public static Instruction Create(Code code) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(instruction.OpCount == 0);
			return instruction;
		}

		/// <summary>
		/// Creates a new near/short branch <see cref="Instruction"/>
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="target">Target address</param>
		/// <returns></returns>
		public static Instruction CreateBranch(Code code, ulong target) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			instruction.Op0Kind = GetNearBranchOpKind(code, 0);
			instruction.NearBranch64 = target;

			Debug.Assert(instruction.OpCount == 1);
			return instruction;
		}

		/// <summary>
		/// Creates a new far branch <see cref="Instruction"/>
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="selector">Selector/segment value</param>
		/// <param name="offset">Offset</param>
		/// <returns></returns>
		public static Instruction CreateBranch(Code code, ushort selector, uint offset) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			instruction.Op0Kind = GetFarBranchOpKind(code, 0);
			instruction.FarBranchSelector = selector;
			instruction.FarBranch32 = offset;

			Debug.Assert(instruction.OpCount == 1);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction with a 64-bit memory offset as the second operand, eg. 'mov al,[123456789ABCDEF0]'
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register">Register (al, ax, eax, rax)</param>
		/// <param name="address">64-bit address</param>
		/// <param name="prefixSegment">Segment override or <see cref="Register.None"/></param>
		/// <returns></returns>
		public static Instruction CreateMemory64(Code code, Register register, ulong address, Register prefixSegment = Register.None) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register;

			instruction.InternalOp1Kind = OpKind.Memory64;
			instruction.MemoryAddress64 = address;
			instruction.InternalSetMemoryDisplSize(4);
			instruction.SegmentPrefix = prefixSegment;

			Debug.Assert(instruction.OpCount == 2);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction with a 64-bit memory offset as the first operand, eg. 'mov [123456789ABCDEF0],al'
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="address">64-bit address</param>
		/// <param name="register">Register (al, ax, eax, rax)</param>
		/// <param name="prefixSegment">Segment override or <see cref="Register.None"/></param>
		/// <returns></returns>
		public static Instruction CreateMemory64(Code code, ulong address, Register register, Register prefixSegment = Register.None) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			instruction.InternalOp0Kind = OpKind.Memory64;
			instruction.MemoryAddress64 = address;
			instruction.InternalSetMemoryDisplSize(4);
			instruction.SegmentPrefix = prefixSegment;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = register;

			Debug.Assert(instruction.OpCount == 2);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register">Register</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register;

			Debug.Assert(instruction.OpCount == 1);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="immediate">Immediate</param>
		/// <returns></returns>
		public static Instruction Create(Code code, int immediate) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			instruction.InternalOp0Kind = GetImmediateOpKind(code, 0);
			instruction.Immediate32 = (uint)immediate;

			Debug.Assert(instruction.OpCount == 1);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="immediate">Immediate</param>
		/// <returns></returns>
		public static Instruction Create(Code code, uint immediate) =>
			Create(code, (int)immediate);

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="memory">Memory operand</param>
		/// <returns></returns>
		public static Instruction Create(Code code, in MemoryOperand memory) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			instruction.InternalOp0Kind = OpKind.Memory;
			instruction.InternalMemoryBase = memory.Base;
			instruction.InternalMemoryIndex = memory.Index;
			instruction.MemoryIndexScale = memory.Scale;
			instruction.MemoryDisplSize = memory.DisplSize;
			instruction.MemoryDisplacement = (uint)memory.Displacement;
			instruction.IsBroadcast = memory.IsBroadcast;
			instruction.SegmentPrefix = memory.SegmentPrefix;

			Debug.Assert(instruction.OpCount == 1);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register1">Register</param>
		/// <param name="register2">Register</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register1, Register register2) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register1;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = register2;

			Debug.Assert(instruction.OpCount == 2);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register">Register</param>
		/// <param name="immediate">Immediate</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register, int immediate) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register;

			var opKind = GetImmediateOpKind(code, 1);
			instruction.InternalOp1Kind = opKind;
			if (opKind == OpKind.Immediate64)
				instruction.Immediate64 = (ulong)immediate;
			else
				instruction.Immediate32 = (uint)immediate;

			Debug.Assert(instruction.OpCount == 2);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register">Register</param>
		/// <param name="immediate">Immediate</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register, uint immediate) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register;

			var opKind = GetImmediateOpKind(code, 1);
			instruction.InternalOp1Kind = opKind;
			if (opKind == OpKind.Immediate64)
				instruction.Immediate64 = immediate;
			else
				instruction.Immediate32 = immediate;

			Debug.Assert(instruction.OpCount == 2);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction with a 64-bit immediate value
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register">Register</param>
		/// <param name="immediate">64-bit immediate</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register, long immediate) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register;

			instruction.InternalOp1Kind = OpKind.Immediate64;
			instruction.Immediate64 = (ulong)immediate;

			Debug.Assert(instruction.OpCount == 2);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction with a 64-bit immediate value
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register">Register</param>
		/// <param name="immediate">64-bit immediate</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register, ulong immediate) =>
			Create(code, register, (long)immediate);

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register">Register (eg. dx, al, ax, eax, rax)</param>
		/// <param name="rSI">si, esi, or rsi</param>
		/// <param name="prefixSegment">Segment override or <see cref="Register.None"/></param>
		/// <returns></returns>
		public static Instruction CreateString_Reg_SegRSI(Code code, Register register, Register rSI, Register prefixSegment = Register.None) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register;

			if (rSI == Register.RSI)
				instruction.InternalOp1Kind = OpKind.MemorySegRSI;
			else if (rSI == Register.ESI)
				instruction.InternalOp1Kind = OpKind.MemorySegESI;
			else if (rSI == Register.SI)
				instruction.InternalOp1Kind = OpKind.MemorySegSI;
			else
				throw new ArgumentOutOfRangeException(nameof(rSI));

			instruction.SegmentPrefix = prefixSegment;

			Debug.Assert(instruction.OpCount == 2);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register">Register (eg. al, ax, eax, rax)</param>
		/// <param name="rDI">di, edi, or rdi</param>
		/// <returns></returns>
		public static Instruction CreateString_Reg_ESRDI(Code code, Register register, Register rDI) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register;

			if (rDI == Register.RDI)
				instruction.InternalOp1Kind = OpKind.MemoryESRDI;
			else if (rDI == Register.EDI)
				instruction.InternalOp1Kind = OpKind.MemoryESEDI;
			else if (rDI == Register.DI)
				instruction.InternalOp1Kind = OpKind.MemoryESDI;
			else
				throw new ArgumentOutOfRangeException(nameof(rDI));

			Debug.Assert(instruction.OpCount == 2);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register">Register</param>
		/// <param name="memory">Memory operand</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register, in MemoryOperand memory) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register;

			instruction.InternalOp1Kind = OpKind.Memory;
			instruction.InternalMemoryBase = memory.Base;
			instruction.InternalMemoryIndex = memory.Index;
			instruction.MemoryIndexScale = memory.Scale;
			instruction.MemoryDisplSize = memory.DisplSize;
			instruction.MemoryDisplacement = (uint)memory.Displacement;
			instruction.IsBroadcast = memory.IsBroadcast;
			instruction.SegmentPrefix = memory.SegmentPrefix;

			Debug.Assert(instruction.OpCount == 2);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="immediate">Immediate</param>
		/// <param name="register">Register</param>
		/// <returns></returns>
		public static Instruction Create(Code code, int immediate, Register register) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			instruction.InternalOp0Kind = GetImmediateOpKind(code, 0);
			instruction.Immediate32 = (uint)immediate;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = register;

			Debug.Assert(instruction.OpCount == 2);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="immediate">Immediate</param>
		/// <param name="register">Register</param>
		/// <returns></returns>
		public static Instruction Create(Code code, uint immediate, Register register) =>
			Create(code, (int)immediate, register);

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="immediate">Immediate</param>
		/// <param name="immediate2">Second immediate</param>
		/// <returns></returns>
		public static Instruction Create(Code code, int immediate, byte immediate2) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			instruction.InternalOp0Kind = GetImmediateOpKind(code, 0);
			instruction.Immediate32 = (uint)immediate;

			instruction.InternalOp1Kind = OpKind.Immediate8_2nd;
			instruction.Immediate8_2nd = immediate2;

			Debug.Assert(instruction.OpCount == 2);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="immediate">Immediate</param>
		/// <param name="immediate2">Second immediate</param>
		/// <returns></returns>
		public static Instruction Create(Code code, uint immediate, byte immediate2) =>
			Create(code, (int)immediate, immediate2);

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="rSI">si, esi, or rsi</param>
		/// <param name="rDI">di, edi, or rdi</param>
		/// <param name="prefixSegment">Segment override or <see cref="Register.None"/></param>
		/// <returns></returns>
		public static Instruction CreateString_SegRSI_ESRDI(Code code, Register rSI, Register rDI, Register prefixSegment = Register.None) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			if (rSI == Register.RSI)
				instruction.InternalOp0Kind = OpKind.MemorySegRSI;
			else if (rSI == Register.ESI)
				instruction.InternalOp0Kind = OpKind.MemorySegESI;
			else if (rSI == Register.SI)
				instruction.InternalOp0Kind = OpKind.MemorySegSI;
			else
				throw new ArgumentOutOfRangeException(nameof(rSI));

			if (rDI == Register.RDI)
				instruction.InternalOp1Kind = OpKind.MemoryESRDI;
			else if (rDI == Register.EDI)
				instruction.InternalOp1Kind = OpKind.MemoryESEDI;
			else if (rDI == Register.DI)
				instruction.InternalOp1Kind = OpKind.MemoryESDI;
			else
				throw new ArgumentOutOfRangeException(nameof(rDI));

			instruction.SegmentPrefix = prefixSegment;

			Debug.Assert(instruction.OpCount == 2);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="rDI">di, edi, or rdi</param>
		/// <param name="register">Register (eg. dx, al, ax, eax, rax)</param>
		/// <returns></returns>
		public static Instruction CreateString_ESRDI_Reg(Code code, Register rDI, Register register) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			if (rDI == Register.RDI)
				instruction.InternalOp0Kind = OpKind.MemoryESRDI;
			else if (rDI == Register.EDI)
				instruction.InternalOp0Kind = OpKind.MemoryESEDI;
			else if (rDI == Register.DI)
				instruction.InternalOp0Kind = OpKind.MemoryESDI;
			else
				throw new ArgumentOutOfRangeException(nameof(rDI));

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = register;

			Debug.Assert(instruction.OpCount == 2);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="rDI">di, edi, or rdi</param>
		/// <param name="rSI">si, esi, or rsi</param>
		/// <param name="prefixSegment">Segment override or <see cref="Register.None"/></param>
		/// <returns></returns>
		public static Instruction CreateString_ESRDI_SegRSI(Code code, Register rDI, Register rSI, Register prefixSegment = Register.None) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			if (rDI == Register.RDI)
				instruction.InternalOp0Kind = OpKind.MemoryESRDI;
			else if (rDI == Register.EDI)
				instruction.InternalOp0Kind = OpKind.MemoryESEDI;
			else if (rDI == Register.DI)
				instruction.InternalOp0Kind = OpKind.MemoryESDI;
			else
				throw new ArgumentOutOfRangeException(nameof(rDI));

			if (rSI == Register.RSI)
				instruction.InternalOp1Kind = OpKind.MemorySegRSI;
			else if (rSI == Register.ESI)
				instruction.InternalOp1Kind = OpKind.MemorySegESI;
			else if (rSI == Register.SI)
				instruction.InternalOp1Kind = OpKind.MemorySegSI;
			else
				throw new ArgumentOutOfRangeException(nameof(rSI));

			instruction.SegmentPrefix = prefixSegment;

			Debug.Assert(instruction.OpCount == 2);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="memory">Memory operand</param>
		/// <param name="register">Register</param>
		/// <returns></returns>
		public static Instruction Create(Code code, in MemoryOperand memory, Register register) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			instruction.InternalOp0Kind = OpKind.Memory;
			instruction.InternalMemoryBase = memory.Base;
			instruction.InternalMemoryIndex = memory.Index;
			instruction.MemoryIndexScale = memory.Scale;
			instruction.MemoryDisplSize = memory.DisplSize;
			instruction.MemoryDisplacement = (uint)memory.Displacement;
			instruction.IsBroadcast = memory.IsBroadcast;
			instruction.SegmentPrefix = memory.SegmentPrefix;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = register;

			Debug.Assert(instruction.OpCount == 2);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="memory">Memory operand</param>
		/// <param name="immediate">Immediate</param>
		/// <returns></returns>
		public static Instruction Create(Code code, in MemoryOperand memory, int immediate) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			instruction.InternalOp0Kind = OpKind.Memory;
			instruction.InternalMemoryBase = memory.Base;
			instruction.InternalMemoryIndex = memory.Index;
			instruction.MemoryIndexScale = memory.Scale;
			instruction.MemoryDisplSize = memory.DisplSize;
			instruction.MemoryDisplacement = (uint)memory.Displacement;
			instruction.IsBroadcast = memory.IsBroadcast;
			instruction.SegmentPrefix = memory.SegmentPrefix;

			instruction.InternalOp1Kind = GetImmediateOpKind(code, 1);
			instruction.Immediate32 = (uint)immediate;

			Debug.Assert(instruction.OpCount == 2);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="memory">Memory operand</param>
		/// <param name="immediate">Immediate</param>
		/// <returns></returns>
		public static Instruction Create(Code code, in MemoryOperand memory, uint immediate) =>
			Create(code, memory, (int)immediate);

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register1">Register</param>
		/// <param name="register2">Register</param>
		/// <param name="register3">Register</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register1, Register register2, Register register3) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register1;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = register2;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp2Kind = OpKind.Register;
			instruction.InternalOp2Register = register3;

			Debug.Assert(instruction.OpCount == 3);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register1">Register</param>
		/// <param name="register2">Register</param>
		/// <param name="immediate">Immediate</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register1, Register register2, int immediate) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register1;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = register2;

			instruction.InternalOp2Kind = GetImmediateOpKind(code, 2);
			instruction.Immediate32 = (uint)immediate;

			Debug.Assert(instruction.OpCount == 3);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register1">Register</param>
		/// <param name="register2">Register</param>
		/// <param name="immediate">Immediate</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register1, Register register2, uint immediate) =>
			Create(code, register1, register2, (int)immediate);

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register1">Register</param>
		/// <param name="register2">Register</param>
		/// <param name="memory">Memory operand</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register1, Register register2, in MemoryOperand memory) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register1;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = register2;

			instruction.InternalOp2Kind = OpKind.Memory;
			instruction.InternalMemoryBase = memory.Base;
			instruction.InternalMemoryIndex = memory.Index;
			instruction.MemoryIndexScale = memory.Scale;
			instruction.MemoryDisplSize = memory.DisplSize;
			instruction.MemoryDisplacement = (uint)memory.Displacement;
			instruction.IsBroadcast = memory.IsBroadcast;
			instruction.SegmentPrefix = memory.SegmentPrefix;

			Debug.Assert(instruction.OpCount == 3);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register">Register</param>
		/// <param name="immediate">Immediate</param>
		/// <param name="immediate2">Second immediate</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register, int immediate, byte immediate2) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register;

			instruction.InternalOp1Kind = GetImmediateOpKind(code, 1);
			instruction.Immediate32 = (uint)immediate;

			instruction.InternalOp2Kind = OpKind.Immediate8_2nd;
			instruction.Immediate8_2nd = immediate2;

			Debug.Assert(instruction.OpCount == 3);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register">Register</param>
		/// <param name="immediate">Immediate</param>
		/// <param name="immediate2">Second immediate</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register, uint immediate, byte immediate2) =>
			Create(code, register, (int)immediate, immediate2);

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register1">Register</param>
		/// <param name="memory">Memory operand</param>
		/// <param name="register2">Register</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register1, in MemoryOperand memory, Register register2) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register1;

			instruction.InternalOp1Kind = OpKind.Memory;
			instruction.InternalMemoryBase = memory.Base;
			instruction.InternalMemoryIndex = memory.Index;
			instruction.MemoryIndexScale = memory.Scale;
			instruction.MemoryDisplSize = memory.DisplSize;
			instruction.MemoryDisplacement = (uint)memory.Displacement;
			instruction.IsBroadcast = memory.IsBroadcast;
			instruction.SegmentPrefix = memory.SegmentPrefix;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp2Kind = OpKind.Register;
			instruction.InternalOp2Register = register2;

			Debug.Assert(instruction.OpCount == 3);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register">Register</param>
		/// <param name="memory">Memory operand</param>
		/// <param name="immediate">Immediate</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register, in MemoryOperand memory, int immediate) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register;

			instruction.InternalOp1Kind = OpKind.Memory;
			instruction.InternalMemoryBase = memory.Base;
			instruction.InternalMemoryIndex = memory.Index;
			instruction.MemoryIndexScale = memory.Scale;
			instruction.MemoryDisplSize = memory.DisplSize;
			instruction.MemoryDisplacement = (uint)memory.Displacement;
			instruction.IsBroadcast = memory.IsBroadcast;
			instruction.SegmentPrefix = memory.SegmentPrefix;

			instruction.InternalOp2Kind = GetImmediateOpKind(code, 2);
			instruction.Immediate32 = (uint)immediate;

			Debug.Assert(instruction.OpCount == 3);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register">Register</param>
		/// <param name="memory">Memory operand</param>
		/// <param name="immediate">Immediate</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register, in MemoryOperand memory, uint immediate) =>
			Create(code, register, memory, (int)immediate);

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="rDI">di, edi, or rdi</param>
		/// <param name="register1">Register</param>
		/// <param name="register2">Register</param>
		/// <param name="prefixSegment">Segment override or <see cref="Register.None"/></param>
		/// <returns></returns>
		public static Instruction CreateMaskmov_SegRDI_Reg_Reg(Code code, Register rDI, Register register1, Register register2, Register prefixSegment = Register.None) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			if (rDI == Register.RDI)
				instruction.InternalOp0Kind = OpKind.MemorySegRDI;
			else if (rDI == Register.EDI)
				instruction.InternalOp0Kind = OpKind.MemorySegEDI;
			else if (rDI == Register.DI)
				instruction.InternalOp0Kind = OpKind.MemorySegDI;
			else
				throw new ArgumentOutOfRangeException(nameof(rDI));

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = register1;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp2Kind = OpKind.Register;
			instruction.InternalOp2Register = register2;

			instruction.SegmentPrefix = prefixSegment;

			Debug.Assert(instruction.OpCount == 3);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="memory">Memory operand</param>
		/// <param name="register1">Register</param>
		/// <param name="register2">Register</param>
		/// <returns></returns>
		public static Instruction Create(Code code, in MemoryOperand memory, Register register1, Register register2) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			instruction.InternalOp0Kind = OpKind.Memory;
			instruction.InternalMemoryBase = memory.Base;
			instruction.InternalMemoryIndex = memory.Index;
			instruction.MemoryIndexScale = memory.Scale;
			instruction.MemoryDisplSize = memory.DisplSize;
			instruction.MemoryDisplacement = (uint)memory.Displacement;
			instruction.IsBroadcast = memory.IsBroadcast;
			instruction.SegmentPrefix = memory.SegmentPrefix;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = register1;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp2Kind = OpKind.Register;
			instruction.InternalOp2Register = register2;

			Debug.Assert(instruction.OpCount == 3);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="memory">Memory operand</param>
		/// <param name="register">Register</param>
		/// <param name="immediate">Immediate</param>
		/// <returns></returns>
		public static Instruction Create(Code code, in MemoryOperand memory, Register register, int immediate) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			instruction.InternalOp0Kind = OpKind.Memory;
			instruction.InternalMemoryBase = memory.Base;
			instruction.InternalMemoryIndex = memory.Index;
			instruction.MemoryIndexScale = memory.Scale;
			instruction.MemoryDisplSize = memory.DisplSize;
			instruction.MemoryDisplacement = (uint)memory.Displacement;
			instruction.IsBroadcast = memory.IsBroadcast;
			instruction.SegmentPrefix = memory.SegmentPrefix;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = register;

			instruction.InternalOp2Kind = GetImmediateOpKind(code, 2);
			instruction.Immediate32 = (uint)immediate;

			Debug.Assert(instruction.OpCount == 3);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="memory">Memory operand</param>
		/// <param name="register">Register</param>
		/// <param name="immediate">Immediate</param>
		/// <returns></returns>
		public static Instruction Create(Code code, in MemoryOperand memory, Register register, uint immediate) =>
			Create(code, memory, register, (int)immediate);

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register1">Register</param>
		/// <param name="register2">Register</param>
		/// <param name="register3">Register</param>
		/// <param name="register4">Register</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register1, Register register2, Register register3, Register register4) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register1;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = register2;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp2Kind = OpKind.Register;
			instruction.InternalOp2Register = register3;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp3Kind = OpKind.Register;
			instruction.InternalOp3Register = register4;

			Debug.Assert(instruction.OpCount == 4);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register1">Register</param>
		/// <param name="register2">Register</param>
		/// <param name="register3">Register</param>
		/// <param name="immediate">Immediate</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register1, Register register2, Register register3, int immediate) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register1;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = register2;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp2Kind = OpKind.Register;
			instruction.InternalOp2Register = register3;

			instruction.InternalOp3Kind = GetImmediateOpKind(code, 3);
			instruction.Immediate32 = (uint)immediate;

			Debug.Assert(instruction.OpCount == 4);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register1">Register</param>
		/// <param name="register2">Register</param>
		/// <param name="register3">Register</param>
		/// <param name="immediate">Immediate</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register1, Register register2, Register register3, uint immediate) =>
			Create(code, register1, register2, register3, (int)immediate);

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register1">Register</param>
		/// <param name="register2">Register</param>
		/// <param name="register3">Register</param>
		/// <param name="memory">Memory operand</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register1, Register register2, Register register3, in MemoryOperand memory) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register1;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = register2;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp2Kind = OpKind.Register;
			instruction.InternalOp2Register = register3;

			instruction.InternalOp3Kind = OpKind.Memory;
			instruction.InternalMemoryBase = memory.Base;
			instruction.InternalMemoryIndex = memory.Index;
			instruction.MemoryIndexScale = memory.Scale;
			instruction.MemoryDisplSize = memory.DisplSize;
			instruction.MemoryDisplacement = (uint)memory.Displacement;
			instruction.IsBroadcast = memory.IsBroadcast;
			instruction.SegmentPrefix = memory.SegmentPrefix;

			Debug.Assert(instruction.OpCount == 4);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register1">Register</param>
		/// <param name="register2">Register</param>
		/// <param name="immediate">Immediate</param>
		/// <param name="immediate2">Second immediate</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register1, Register register2, int immediate, byte immediate2) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register1;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = register2;

			instruction.InternalOp2Kind = GetImmediateOpKind(code, 2);
			instruction.Immediate32 = (uint)immediate;

			instruction.InternalOp3Kind = OpKind.Immediate8_2nd;
			instruction.Immediate8_2nd = immediate2;

			Debug.Assert(instruction.OpCount == 4);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register1">Register</param>
		/// <param name="register2">Register</param>
		/// <param name="immediate">Immediate</param>
		/// <param name="immediate2">Second immediate</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register1, Register register2, uint immediate, byte immediate2) =>
			Create(code, register1, register2, (int)immediate, immediate2);

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register1">Register</param>
		/// <param name="register2">Register</param>
		/// <param name="memory">Memory operand</param>
		/// <param name="register3">Register</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register1, Register register2, in MemoryOperand memory, Register register3) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register1;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = register2;

			instruction.InternalOp2Kind = OpKind.Memory;
			instruction.InternalMemoryBase = memory.Base;
			instruction.InternalMemoryIndex = memory.Index;
			instruction.MemoryIndexScale = memory.Scale;
			instruction.MemoryDisplSize = memory.DisplSize;
			instruction.MemoryDisplacement = (uint)memory.Displacement;
			instruction.IsBroadcast = memory.IsBroadcast;
			instruction.SegmentPrefix = memory.SegmentPrefix;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp3Kind = OpKind.Register;
			instruction.InternalOp3Register = register3;

			Debug.Assert(instruction.OpCount == 4);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register1">Register</param>
		/// <param name="register2">Register</param>
		/// <param name="memory">Memory operand</param>
		/// <param name="immediate">Immediate</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register1, Register register2, in MemoryOperand memory, int immediate) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register1;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = register2;

			instruction.InternalOp2Kind = OpKind.Memory;
			instruction.InternalMemoryBase = memory.Base;
			instruction.InternalMemoryIndex = memory.Index;
			instruction.MemoryIndexScale = memory.Scale;
			instruction.MemoryDisplSize = memory.DisplSize;
			instruction.MemoryDisplacement = (uint)memory.Displacement;
			instruction.IsBroadcast = memory.IsBroadcast;
			instruction.SegmentPrefix = memory.SegmentPrefix;

			instruction.InternalOp3Kind = GetImmediateOpKind(code, 3);
			instruction.Immediate32 = (uint)immediate;

			Debug.Assert(instruction.OpCount == 4);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register1">Register</param>
		/// <param name="register2">Register</param>
		/// <param name="memory">Memory operand</param>
		/// <param name="immediate">Immediate</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register1, Register register2, in MemoryOperand memory, uint immediate) =>
			Create(code, register1, register2, memory, (int)immediate);

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register1">Register</param>
		/// <param name="register2">Register</param>
		/// <param name="register3">Register</param>
		/// <param name="register4">Register</param>
		/// <param name="immediate">Immediate</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register1, Register register2, Register register3, Register register4, int immediate) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register1;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = register2;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp2Kind = OpKind.Register;
			instruction.InternalOp2Register = register3;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp3Kind = OpKind.Register;
			instruction.InternalOp3Register = register4;

			instruction.InternalOp4Kind = GetImmediateOpKind(code, 4);
			instruction.Immediate32 = (uint)immediate;

			Debug.Assert(instruction.OpCount == 5);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register1">Register</param>
		/// <param name="register2">Register</param>
		/// <param name="register3">Register</param>
		/// <param name="register4">Register</param>
		/// <param name="immediate">Immediate</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register1, Register register2, Register register3, Register register4, uint immediate) =>
			Create(code, register1, register2, register3, register4, (int)immediate);

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register1">Register</param>
		/// <param name="register2">Register</param>
		/// <param name="register3">Register</param>
		/// <param name="memory">Memory operand</param>
		/// <param name="immediate">Immediate</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register1, Register register2, Register register3, in MemoryOperand memory, int immediate) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register1;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = register2;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp2Kind = OpKind.Register;
			instruction.InternalOp2Register = register3;

			instruction.InternalOp3Kind = OpKind.Memory;
			instruction.InternalMemoryBase = memory.Base;
			instruction.InternalMemoryIndex = memory.Index;
			instruction.MemoryIndexScale = memory.Scale;
			instruction.MemoryDisplSize = memory.DisplSize;
			instruction.MemoryDisplacement = (uint)memory.Displacement;
			instruction.IsBroadcast = memory.IsBroadcast;
			instruction.SegmentPrefix = memory.SegmentPrefix;

			instruction.InternalOp4Kind = GetImmediateOpKind(code, 4);
			instruction.Immediate32 = (uint)immediate;

			Debug.Assert(instruction.OpCount == 5);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register1">Register</param>
		/// <param name="register2">Register</param>
		/// <param name="register3">Register</param>
		/// <param name="memory">Memory operand</param>
		/// <param name="immediate">Immediate</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register1, Register register2, Register register3, in MemoryOperand memory, uint immediate) =>
			Create(code, register1, register2, register3, memory, (int)immediate);

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register1">Register</param>
		/// <param name="register2">Register</param>
		/// <param name="memory">Memory operand</param>
		/// <param name="register3">Register</param>
		/// <param name="immediate">Immediate</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register1, Register register2, in MemoryOperand memory, Register register3, int immediate) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register1;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = register2;

			instruction.InternalOp2Kind = OpKind.Memory;
			instruction.InternalMemoryBase = memory.Base;
			instruction.InternalMemoryIndex = memory.Index;
			instruction.MemoryIndexScale = memory.Scale;
			instruction.MemoryDisplSize = memory.DisplSize;
			instruction.MemoryDisplacement = (uint)memory.Displacement;
			instruction.IsBroadcast = memory.IsBroadcast;
			instruction.SegmentPrefix = memory.SegmentPrefix;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp3Kind = OpKind.Register;
			instruction.InternalOp3Register = register3;

			instruction.InternalOp4Kind = GetImmediateOpKind(code, 4);
			instruction.Immediate32 = (uint)immediate;

			Debug.Assert(instruction.OpCount == 5);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register1">Register</param>
		/// <param name="register2">Register</param>
		/// <param name="memory">Memory operand</param>
		/// <param name="register3">Register</param>
		/// <param name="immediate">Immediate</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register1, Register register2, in MemoryOperand memory, Register register3, uint immediate) =>
			Create(code, register1, register2, memory, register3, (int)immediate);
	}
}
#endif
