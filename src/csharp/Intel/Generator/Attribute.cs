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

using System;
using System.Reflection;

namespace Generator {
	[AttributeUsage(AttributeTargets.All)]
	sealed class CommentAttribute : Attribute {
		public string Comment { get; }
		public CommentAttribute(string comment) => Comment = comment ?? throw new InvalidOperationException();

		public static string? GetDocumentation(MemberInfo member) =>
			((CommentAttribute?)member.GetCustomAttribute(typeof(CommentAttribute)))?.Comment;
	}

	[AttributeUsage(AttributeTargets.Enum)]
	sealed class EnumAttribute : Attribute {
		public string Name { get; }
		public new TypeId TypeId { get; }
		public string? Documentation { get; set; }
		public bool Public { get; set; }
		public bool NoInitialize { get; set; }
		public bool Flags { get; set; }

		public EnumAttribute(string typeId) {
			Name = typeId;
			TypeId = new TypeId(typeId);
		}

		public EnumAttribute(string name, string typeId) {
			Name = name;
			TypeId = new TypeId(typeId);
		}
	}

	static class TypeGenOrders {
		/// <summary>
		/// Only depends on exported enums (<see cref="EnumAttribute"/>), does not depend on other created enums/constants.
		/// Must not depend on Code.
		/// </summary>
		public const double NoDeps = 0;

		/// <summary>
		/// Depends on Code (before it's been filtered) and is called before instructions are filtered
		/// </summary>
		public const double PreCreateInstructions = 9000;

		/// <summary>
		/// Depends on Code, InstructionDefs and anything else that depends on them
		/// </summary>
		public const double CreatedInstructions = 10000;

		/// <summary>
		/// Depends on anything created earlier
		/// </summary>
		public const double Last = double.MaxValue;
	}

	[AttributeUsage(AttributeTargets.Class)]
	sealed class TypeGenAttribute : Attribute {
		public double Order { get; }
		public TypeGenAttribute(double order) {
			if (double.IsNaN(order))
				throw new ArgumentOutOfRangeException(nameof(order));
			Order = order;
		}
	}
}
