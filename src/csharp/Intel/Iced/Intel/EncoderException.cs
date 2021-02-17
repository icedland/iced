// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if ENCODER
using System;
using System.Runtime.Serialization;

namespace Iced.Intel {
	/// <summary>
	/// Thrown if the encoder can't encode an instruction
	/// </summary>
	[Serializable]
	public class EncoderException : Exception {
		/// <summary>
		/// The instruction that couldn't be encoded
		/// </summary>
		public Instruction Instruction { get; }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="message">Exception message</param>
		/// <param name="instruction">Instruction</param>
		public EncoderException(string message, in Instruction instruction) : base(message) => Instruction = instruction;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected EncoderException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}
#endif
