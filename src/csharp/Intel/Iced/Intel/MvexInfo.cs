// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if (DECODER || ENCODER || (ENCODER && OPCODE_INFO)) && MVEX
using System.Diagnostics;

namespace Iced.Intel {
	readonly struct MvexInfo {
		readonly int index;

		public MvexTupleTypeLutKind TupleTypeLutKind => (MvexTupleTypeLutKind)MvexInfoData.Data[index * MvexInfoData.StructSize + MvexInfoData.TupleTypeLutKindIndex];
		public MvexEHBit EHBit => (MvexEHBit)MvexInfoData.Data[index * MvexInfoData.StructSize + MvexInfoData.EHBitIndex];
		public MvexConvFn ConvFn => (MvexConvFn)MvexInfoData.Data[index * MvexInfoData.StructSize + MvexInfoData.ConvFnIndex];
		public uint InvalidConvFns => MvexInfoData.Data[index * MvexInfoData.StructSize + MvexInfoData.InvalidConvFnsIndex];
		public uint InvalidSwizzleFns => MvexInfoData.Data[index * MvexInfoData.StructSize + MvexInfoData.InvalidSwizzleFnsIndex];
		public bool IsNDD => ((MvexInfoFlags1)MvexInfoData.Data[index * MvexInfoData.StructSize + MvexInfoData.Flags1Index] & MvexInfoFlags1.NDD) != 0;
		public bool IsNDS => ((MvexInfoFlags1)MvexInfoData.Data[index * MvexInfoData.StructSize + MvexInfoData.Flags1Index] & MvexInfoFlags1.NDS) != 0;
		public bool CanUseEvictionHint => ((MvexInfoFlags1)MvexInfoData.Data[index * MvexInfoData.StructSize + MvexInfoData.Flags1Index] & MvexInfoFlags1.EvictionHint) != 0;
		public bool CanUseImmRoundingControl => ((MvexInfoFlags1)MvexInfoData.Data[index * MvexInfoData.StructSize + MvexInfoData.Flags1Index] & MvexInfoFlags1.ImmRoundingControl) != 0;
		public bool CanUseRoundingControl => ((MvexInfoFlags1)MvexInfoData.Data[index * MvexInfoData.StructSize + MvexInfoData.Flags1Index] & MvexInfoFlags1.RoundingControl) != 0;
		public bool CanUseSuppressAllExceptions => ((MvexInfoFlags1)MvexInfoData.Data[index * MvexInfoData.StructSize + MvexInfoData.Flags1Index] & MvexInfoFlags1.SuppressAllExceptions) != 0;
		public bool IgnoresOpMaskRegister => ((MvexInfoFlags1)MvexInfoData.Data[index * MvexInfoData.StructSize + MvexInfoData.Flags1Index] & MvexInfoFlags1.IgnoresOpMaskRegister) != 0;
		public bool RequireOpMaskRegister => ((MvexInfoFlags1)MvexInfoData.Data[index * MvexInfoData.StructSize + MvexInfoData.Flags1Index] & MvexInfoFlags1.RequireOpMaskRegister) != 0;
		public bool NoSaeRc => ((MvexInfoFlags2)MvexInfoData.Data[index * MvexInfoData.StructSize + MvexInfoData.Flags2Index] & MvexInfoFlags2.NoSaeRoundingControl) != 0;
		public bool IsConvFn32 => ((MvexInfoFlags2)MvexInfoData.Data[index * MvexInfoData.StructSize + MvexInfoData.Flags2Index] & MvexInfoFlags2.ConvFn32) != 0;
		public bool IgnoresEvictionHint => ((MvexInfoFlags2)MvexInfoData.Data[index * MvexInfoData.StructSize + MvexInfoData.Flags2Index] & MvexInfoFlags2.IgnoresEvictionHint) != 0;

		public MvexInfo(Code code) {
			index = (int)code - (int)IcedConstants.MvexStart;
			Debug.Assert((uint)index < IcedConstants.MvexLength);
		}
		
		public TupleType GetTupleType(int sss) => (TupleType)MvexTupleTypeLut.Data[(int)TupleTypeLutKind * 8 + sss];
	}
}
#endif
