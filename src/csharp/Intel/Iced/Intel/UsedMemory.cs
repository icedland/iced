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

#if INSTR_INFO
using System.Diagnostics;
using System.Text;

namespace Iced.Intel {
	/// <summary>
	/// A memory location used by an instruction
	/// </summary>
	public readonly struct UsedMemory {
		readonly ulong displ;
		readonly byte segReg;
		readonly byte baseReg;
		readonly byte indexReg;
		readonly byte memorySize;
		readonly byte scale;
		readonly byte access;

		/// <summary>
		/// Effective segment register
		/// </summary>
		public Register Segment => (Register)segReg;

		/// <summary>
		/// Base register or <see cref="Register.None"/> if none
		/// </summary>
		public Register Base => (Register)baseReg;

		/// <summary>
		/// Index register or <see cref="Register.None"/> if none
		/// </summary>
		public Register Index => (Register)indexReg;

		/// <summary>
		/// Index scale (1, 2, 4 or 8)
		/// </summary>
		public int Scale => scale;

		/// <summary>
		/// Displacement
		/// </summary>
		public ulong Displacement => displ;

		/// <summary>
		/// Size of location
		/// </summary>
		public MemorySize MemorySize => (MemorySize)memorySize;

		/// <summary>
		/// Memory access
		/// </summary>
		public OpAccess Access => (OpAccess)access;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="segReg">Effective segment register</param>
		/// <param name="baseReg">Base register</param>
		/// <param name="indexReg">Index register</param>
		/// <param name="scale">1, 2, 4 or 8</param>
		/// <param name="displ">Displacement</param>
		/// <param name="memorySize">Memory size</param>
		/// <param name="access">Access</param>
		public UsedMemory(Register segReg, Register baseReg, Register indexReg, int scale, long displ, MemorySize memorySize, OpAccess access) {
			this.displ = (ulong)displ;
			Debug.Assert((uint)segReg <= byte.MaxValue);
			this.segReg = (byte)segReg;
			Debug.Assert((uint)baseReg <= byte.MaxValue);
			this.baseReg = (byte)baseReg;
			Debug.Assert((uint)indexReg <= byte.MaxValue);
			this.indexReg = (byte)indexReg;
			Debug.Assert((uint)memorySize <= byte.MaxValue);
			this.memorySize = (byte)memorySize;
			Debug.Assert(scale == 1 || scale == 2 || scale == 4 || scale == 8);
			this.scale = (byte)scale;
			Debug.Assert((uint)access <= byte.MaxValue);
			this.access = (byte)access;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="segReg">Effective segment register</param>
		/// <param name="baseReg">Base register</param>
		/// <param name="indexReg">Index register</param>
		/// <param name="scale">1, 2, 4 or 8</param>
		/// <param name="displ">Displacement</param>
		/// <param name="memorySize">Memory size</param>
		/// <param name="access">Access</param>
		public UsedMemory(Register segReg, Register baseReg, Register indexReg, int scale, ulong displ, MemorySize memorySize, OpAccess access) {
			this.displ = displ;
			Debug.Assert((uint)segReg <= byte.MaxValue);
			this.segReg = (byte)segReg;
			Debug.Assert((uint)baseReg <= byte.MaxValue);
			this.baseReg = (byte)baseReg;
			Debug.Assert((uint)indexReg <= byte.MaxValue);
			this.indexReg = (byte)indexReg;
			Debug.Assert((uint)memorySize <= byte.MaxValue);
			this.memorySize = (byte)memorySize;
			Debug.Assert(scale == 1 || scale == 2 || scale == 4 || scale == 8);
			this.scale = (byte)scale;
			Debug.Assert((uint)access <= byte.MaxValue);
			this.access = (byte)access;
		}

		/// <summary>
		/// ToString()
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			var sb = new StringBuilder();
			sb.Append('[');
			sb.Append(Segment.ToString());
			sb.Append(':');
			bool needPlus = false;
			if (Base != Register.None) {
				sb.Append(Base.ToString());
				needPlus = true;
			}
			if (Index != Register.None) {
				if (needPlus)
					sb.Append('+');
				needPlus = true;
				sb.Append(Index.ToString());
				if (Scale != 1) {
					sb.Append('*');
					sb.Append((char)('0' + Scale));
				}
			}
			if (Displacement != 0 || !needPlus) {
				if (needPlus)
					sb.Append('+');
				if (Displacement <= 9)
					sb.Append(Displacement.ToString());
				else {
					sb.Append("0x");
					sb.Append(Displacement.ToString("X"));
				}
			}
			sb.Append(';');
			sb.Append(MemorySize.ToString());
			sb.Append(';');
			sb.Append(Access.ToString());
			sb.Append(']');
			return sb.ToString();
		}
	}
}
#endif
