// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if ENCODER && MVEX
using System.Diagnostics;

namespace Iced.Intel.EncoderInternal {
	readonly struct MvexInfo {
		readonly int index;

		public int TupleTypeSize => MvexInfoData.Data[index * MvexInfoData.StructSize + MvexInfoData.TupleTypeSizeIndex];
		public int MemorySize => MvexInfoData.Data[index * MvexInfoData.StructSize + MvexInfoData.MemorySizeIndex];
		public int ElementSize => MvexInfoData.Data[index * MvexInfoData.StructSize + MvexInfoData.ElementSizeIndex];
		public MvexEHBit EHBit => (MvexEHBit)MvexInfoData.Data[index * MvexInfoData.StructSize + MvexInfoData.EHBitIndex];
		public MvexConvFn ConvFn => (MvexConvFn)MvexInfoData.Data[index * MvexInfoData.StructSize + MvexInfoData.ConvFnIndex];
		public byte ValidConvFns => MvexInfoData.Data[index * MvexInfoData.StructSize + MvexInfoData.ValidConvFnsIndex];
		public byte ValidSwizzleFns => MvexInfoData.Data[index * MvexInfoData.StructSize + MvexInfoData.ValidSwizzleFnsIndex];
		public bool IsNDD => ((MvexInfoFlags)MvexInfoData.Data[index * MvexInfoData.StructSize + MvexInfoData.FlagsIndex] & MvexInfoFlags.NDD) != 0;
		public bool IsNDS => ((MvexInfoFlags)MvexInfoData.Data[index * MvexInfoData.StructSize + MvexInfoData.FlagsIndex] & MvexInfoFlags.NDS) != 0;
		public bool CanUseEvictionHint => ((MvexInfoFlags)MvexInfoData.Data[index * MvexInfoData.StructSize + MvexInfoData.FlagsIndex] & MvexInfoFlags.EvictionHint) != 0;
		public bool CanUseImmRoundingControl => ((MvexInfoFlags)MvexInfoData.Data[index * MvexInfoData.StructSize + MvexInfoData.FlagsIndex] & MvexInfoFlags.ImmRoundingControl) != 0;

		public MvexInfo(Code code) {
			index = (int)code - (int)IcedConstants.MvexStart;
			Debug.Assert((uint)index < IcedConstants.MvexLength);
		}
	}
}
#endif
