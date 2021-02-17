// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if DECODER
using System;

namespace Iced.Intel {
	static class HexUtils {
		public static byte[] ToByteArray(string hexData) {
			if (hexData is null)
				throw new ArgumentNullException(nameof(hexData));
			if (hexData.Length == 0)
				return Array2.Empty<byte>();
			int len = 0;
			for (int i = 0; i < hexData.Length; i++) {
				if (hexData[i] != ' ')
					len++;
			}
			var data = new byte[len / 2];
			int w = 0;
			for (int i = 0; ;) {
				while (i < hexData.Length && char.IsWhiteSpace(hexData[i]))
					i++;
				if (i >= hexData.Length)
					break;
				int hi = TryParseHexChar(hexData[i++]);
				if (hi < 0)
					throw new ArgumentOutOfRangeException(nameof(hexData));

				while (i < hexData.Length && char.IsWhiteSpace(hexData[i]))
					i++;
				if (i >= hexData.Length)
					throw new ArgumentOutOfRangeException(nameof(hexData));
				int lo = TryParseHexChar(hexData[i++]);
				if (lo < 0)
					throw new ArgumentOutOfRangeException(nameof(hexData));
				data[w++] = (byte)((hi << 4) | lo);
			}
			if (w != data.Length)
				throw new InvalidOperationException();
			return data;
		}

		static int TryParseHexChar(char c) {
			if ('0' <= c && c <= '9')
				return c - '0';
			if ('A' <= c && c <= 'F')
				return c - 'A' + 10;
			if ('a' <= c && c <= 'f')
				return c - 'a' + 10;
			return -1;
		}
	}
}
#endif
