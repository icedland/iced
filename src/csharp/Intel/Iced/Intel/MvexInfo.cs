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
		public bool IsNDD => ((MvexInfoFlags)MvexInfoData.Data[index * MvexInfoData.StructSize + MvexInfoData.FlagsIndex] & MvexInfoFlags.NDD) != 0;
		public bool IsNDS => ((MvexInfoFlags)MvexInfoData.Data[index * MvexInfoData.StructSize + MvexInfoData.FlagsIndex] & MvexInfoFlags.NDS) != 0;
		public bool CanUseEvictionHint => ((MvexInfoFlags)MvexInfoData.Data[index * MvexInfoData.StructSize + MvexInfoData.FlagsIndex] & MvexInfoFlags.EvictionHint) != 0;
		public bool CanUseImmRoundingControl => ((MvexInfoFlags)MvexInfoData.Data[index * MvexInfoData.StructSize + MvexInfoData.FlagsIndex] & MvexInfoFlags.ImmRoundingControl) != 0;
		public bool CanUseRoundingControl => ((MvexInfoFlags)MvexInfoData.Data[index * MvexInfoData.StructSize + MvexInfoData.FlagsIndex] & MvexInfoFlags.RoundingControl) != 0;
		public bool CanUseSuppressAllExceptions => ((MvexInfoFlags)MvexInfoData.Data[index * MvexInfoData.StructSize + MvexInfoData.FlagsIndex] & MvexInfoFlags.SuppressAllExceptions) != 0;
		public bool CanUseOpMaskRegister => ((MvexInfoFlags)MvexInfoData.Data[index * MvexInfoData.StructSize + MvexInfoData.FlagsIndex] & MvexInfoFlags.OpMaskRegister) != 0;
		public bool RequireOpMaskRegister => ((MvexInfoFlags)MvexInfoData.Data[index * MvexInfoData.StructSize + MvexInfoData.FlagsIndex] & MvexInfoFlags.RequireOpMaskRegister) != 0;

		public MvexInfo(Code code) {
			index = (int)code - (int)IcedConstants.MvexStart;
			Debug.Assert((uint)index < IcedConstants.MvexLength);
		}
		
		public TupleType GetTupleType(int sss) => (TupleType)MvexTupleTypeLut.Data[(int)TupleTypeLutKind * 8 + sss];
	}
}
#endif
