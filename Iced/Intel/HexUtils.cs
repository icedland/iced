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

#if (!NO_DECODER32 || !NO_DECODER64) && !NO_DECODER
using System;
using System.Text;

namespace Iced.Intel {
	static class HexUtils {
		public static byte[] ToByteArray(string hexData) {
			if (hexData == null)
				throw new ArgumentNullException(nameof(hexData));
			if (hexData.Length == 0)
				return Array.Empty<byte>();
			var data = new byte[hexData.Length / 2];
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
				Array.Resize(ref data, w);
			return data;
		}

		public static string ToString(byte[] hexData) {
			if (hexData == null)
				throw new ArgumentNullException(nameof(hexData));
			if (hexData.Length == 0)
				return string.Empty;

			var builder = new StringBuilder(hexData.Length * 3 - 1);
			for(int i = 0; i < hexData.Length; i++) {
				builder.Append(hexData[i].ToString("2X"));
				if (i + 1 < hexData.Length)
					builder.Append(' ');
			}
			return builder.ToString();
		}

		static int TryParseHexChar(char c) {
			if ('0' <= c && c <= '9')
				return c - '0';
			if ('a' <= c && c <= 'f')
				return c - 'a' + 10;
			if ('A' <= c && c <= 'F')
				return c - 'A' + 10;
			return -1;
		}
	}
}
#endif
