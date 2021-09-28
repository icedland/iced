// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Linq;
using Generator.Enums;
using Generator.Enums.Decoder;

namespace Generator.Decoder {
	sealed class DecoderTableFilter {
		readonly GenTypes genTypes;
		readonly HashSet<EnumValue> filteredCodeValues;
		readonly (string name, object?[] handlers)[] infos;
		readonly EnumType handlerKindType;
		readonly EnumValue invalid;
		readonly EnumValue invalid_NoModRM;
		readonly EnumValue? handlerKind_d3now;
		readonly EnumValue? handlerKind_wbinvd;
		readonly EnumValue groupKind;
		readonly EnumValue? group8x64Kind;
		readonly EnumValue? group8x8Kind;
		readonly EnumValue[] noModrmHandlers;
		readonly object[] invalidHandler;
		readonly object[] invalidNoModrmHandler;
		readonly List<string> removedNames;
		readonly EnumValue[] prefixes;

		public DecoderTableFilter(GenTypes genTypes, HashSet<EnumValue> filteredCodeValues, (string name, object?[] handlers)[] infos, EncodingKind encoding) {
			this.genTypes = genTypes;
			this.filteredCodeValues = filteredCodeValues;
			this.infos = infos;
			removedNames = new List<string>();

			switch (encoding) {
			case EncodingKind.Legacy:
				handlerKindType = genTypes[TypeIds.LegacyOpCodeHandlerKind];
				invalid = handlerKindType[nameof(LegacyOpCodeHandlerKind.Invalid)];
				invalid_NoModRM = handlerKindType[nameof(LegacyOpCodeHandlerKind.Invalid_NoModRM)];
				handlerKind_d3now = handlerKindType[nameof(LegacyOpCodeHandlerKind.D3NOW)];
				handlerKind_wbinvd = handlerKindType[nameof(LegacyOpCodeHandlerKind.Wbinvd)];
				groupKind = handlerKindType[nameof(LegacyOpCodeHandlerKind.Group)];
				group8x64Kind = handlerKindType[nameof(LegacyOpCodeHandlerKind.Group8x64)];
				group8x8Kind = handlerKindType[nameof(LegacyOpCodeHandlerKind.Group8x8)];
				noModrmHandlers = new[] { handlerKindType[nameof(LegacyOpCodeHandlerKind.MandatoryPrefix_NoModRM)] };
				prefixes = new[] {
					handlerKindType[nameof(LegacyOpCodeHandlerKind.PrefixEsCsSsDs)],
					handlerKindType[nameof(LegacyOpCodeHandlerKind.PrefixFsGs)],
					handlerKindType[nameof(LegacyOpCodeHandlerKind.Prefix66)],
					handlerKindType[nameof(LegacyOpCodeHandlerKind.Prefix67)],
					handlerKindType[nameof(LegacyOpCodeHandlerKind.PrefixF0)],
					handlerKindType[nameof(LegacyOpCodeHandlerKind.PrefixF2)],
					handlerKindType[nameof(LegacyOpCodeHandlerKind.PrefixF3)],
					handlerKindType[nameof(LegacyOpCodeHandlerKind.PrefixREX)],
				};
				break;

			case EncodingKind.VEX:
			case EncodingKind.XOP:
				handlerKindType = genTypes[TypeIds.VexOpCodeHandlerKind];
				invalid = handlerKindType[nameof(VexOpCodeHandlerKind.Invalid)];
				invalid_NoModRM = handlerKindType[nameof(VexOpCodeHandlerKind.Invalid_NoModRM)];
				handlerKind_d3now = null;
				handlerKind_wbinvd = null;
				groupKind = handlerKindType[nameof(VexOpCodeHandlerKind.Group)];
				group8x64Kind = null;
				group8x8Kind = null;
				noModrmHandlers = new[] {
					handlerKindType[nameof(VexOpCodeHandlerKind.MandatoryPrefix2_NoModRM)],
					handlerKindType[nameof(VexOpCodeHandlerKind.VectorLength_NoModRM)],
				};
				prefixes = Array.Empty<EnumValue>();
				break;

			case EncodingKind.EVEX:
				handlerKindType = genTypes[TypeIds.EvexOpCodeHandlerKind];
				invalid = handlerKindType[nameof(EvexOpCodeHandlerKind.Invalid)];
				invalid_NoModRM = invalid;
				handlerKind_d3now = null;
				handlerKind_wbinvd = null;
				groupKind = handlerKindType[nameof(EvexOpCodeHandlerKind.Group)];
				group8x64Kind = null;
				group8x8Kind = null;
				noModrmHandlers = Array.Empty<EnumValue>();
				prefixes = Array.Empty<EnumValue>();
				break;

			case EncodingKind.MVEX:
				handlerKindType = genTypes[TypeIds.MvexOpCodeHandlerKind];
				invalid = handlerKindType[nameof(MvexOpCodeHandlerKind.Invalid)];
				invalid_NoModRM = invalid;
				handlerKind_d3now = null;
				handlerKind_wbinvd = null;
				groupKind = handlerKindType[nameof(MvexOpCodeHandlerKind.Group)];
				group8x64Kind = null;
				group8x8Kind = null;
				noModrmHandlers = Array.Empty<EnumValue>();
				prefixes = Array.Empty<EnumValue>();
				break;

			case EncodingKind.D3NOW:
			default:
				throw new InvalidOperationException();
			}

			invalidHandler = new object[] { invalid };
			invalidNoModrmHandler = new object[] { invalid_NoModRM };
		}

		public (string name, object?[] handlers)[] Filter() {
			foreach (var (name, handlers) in infos) {
				bool canUseNull = handlers.Length == 0x40 && !name.Contains("_FPU_", StringComparison.Ordinal);
				if (canUseNull) {
					foreach (var h in handlers) {
						if (h is object?[] h2 && DecoderTableUtils.IsInvalid(genTypes, h2))
							throw new InvalidOperationException();
					}
				}
				else {
					foreach (var h in handlers) {
						if (h is null)
							throw new InvalidOperationException();
					}
				}

				for (int i = 0; i < handlers.Length; i++) {
					var handler = Filter(handlers[i], true);
					if (canUseNull && handler is object?[] handler2 && DecoderTableUtils.IsInvalid(genTypes, handler2))
						handler = null;
					handlers[i] = handler;
				}
			}

			return infos.Where(a => !removedNames.Contains(a.name)).ToArray();
		}

		int CountCodeValues(object?[] handler) {
			int keep = 0;
			int remove = 0;

			if (handler[0] == handlerKind_d3now) {
				var origCode = genTypes.GetObject<EnumValue[]>(TypeIds.OrigCodeValues);
				foreach (var code in origCode) {
					if (code.RawName.StartsWith("D3NOW_", StringComparison.Ordinal))
						CountValue(code);
				}
			}
			else if (handler[0] == handlerKind_wbinvd) {
				var origCode = genTypes.GetObject<EnumValue[]>(TypeIds.OrigCodeValues);
				var values = new[] {
					origCode[(int)Code.Wbinvd],
					origCode[(int)Code.Wbnoinvd],
				};
				foreach (var code in values)
					CountValue(code);
			}

			foreach (var obj in handler) {
				if (obj is EnumValue enumValue && enumValue.DeclaringType.TypeId == TypeIds.Code)
					CountValue(enumValue);
			}
			// All must be removed or all must be kept
			if (keep != 0 && remove != 0)
				throw new InvalidOperationException("A Code value was removed but it must be kept");
			if (keep != 0)
				return keep;
			return -remove;

			void CountValue(EnumValue code) {
				if (code.RawName == nameof(Code.INVALID))
					return;
				if (filteredCodeValues.Contains(code))
					keep++;
				else
					remove++;
			}
		}

		object GetInvalidHandler(bool useInvalidModrm) => useInvalidModrm ? invalidHandler : invalidNoModrmHandler;

		bool TryRemoveGroup(string[] groupNames) {
			foreach (var name in groupNames) {
				var handlers = infos.Single(a => a.name == name).handlers;
				foreach (var handler in handlers) {
					if (handler is null || (handler is object?[] h && (h[0] == invalid || h[0] == invalid_NoModRM)))
						continue;
					return false;
				}
			}

			removedNames.AddRange(groupNames);
			return true;
		}

		object? Filter(object? obj, bool useInvalidModrm) {
			if (obj is object?[] handler) {
				if (!DecoderTableUtils.IsHandler(handler, out var handlerKind))
					throw new InvalidOperationException();

				if (handler[0] == invalid || handler[0] == invalid_NoModRM)
					return handler;

				if (handlerKind == groupKind) {
					if (handler.Length != 2)
						throw new InvalidOperationException();
					if (handler[1] is not string name)
						throw new InvalidOperationException();
					if (TryRemoveGroup(new[] { name }))
						return GetInvalidHandler(true);
					return handler;
				}
				else if (handlerKind == group8x64Kind || handlerKind == group8x8Kind) {
					if (handler.Length != 3)
						throw new InvalidOperationException();
					if (handler[1] is not string name1)
						throw new InvalidOperationException();
					if (handler[2] is not string name2)
						throw new InvalidOperationException();
					if (TryRemoveGroup(new[] { name1, name2 }))
						return GetInvalidHandler(true);
					return handler;
				}

				bool childUseInvalidModrm = Array.IndexOf(noModrmHandlers, handlerKind) < 0;
				for (int i = 0; i < handler.Length; i++)
					handler[i] = Filter(handler[i], childUseInvalidModrm);

				var count = CountCodeValues(handler);
				if (count > 0) {
					// Keep it
					return handler;
				}
				else if (count < 0) {
					// They were removed, return invalid opcode handler
					return GetInvalidHandler(useInvalidModrm);
				}
				else {
					// If it's an opcode handler with only invalid opcode handlers as input, it's useless and we can
					// return an invalid opcode handler instead. Eg. W(invalid,invalid) becomes invalid
					if (CanReplaceHandlerWithInvalid(handler))
						return GetInvalidHandler(useInvalidModrm);
					return handler;
				}
			}
			else
				return obj;
		}

		bool CanReplaceHandlerWithInvalid(object?[] handler) {
			if (handler[0] == invalid || handler[0] == invalid_NoModRM)
				return false;
			foreach (var prefix in prefixes) {
				if (prefix == handler[0])
					return false;
			}
			bool foundInvalidHandler = false;
			bool otherArgs = false;
			for (int i = 1; i < handler.Length; i++) {
				switch (handler[i]) {
				case object?[] h:
					if (h[0] != invalid && h[0] != invalid_NoModRM)
						return false;
					foundInvalidHandler = true;
					break;

				case string:
					// Handler ref
					return false;

				default:
					otherArgs = true;
					break;
				}
			}
			if (!otherArgs)
				return true;
			return foundInvalidHandler;
		}
	}
}
