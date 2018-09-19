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
using System.Runtime.Serialization;

namespace Iced.Intel {
	/// <summary>
	/// Thrown if the encoder can't encode an instruction
	/// </summary>
	[Serializable]
	public class EncoderException : Exception {
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="message">Exception message</param>
		public EncoderException(string message) : base(message) { }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected EncoderException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}
#endif
