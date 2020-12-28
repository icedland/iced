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

using Generator.Constants;
using Generator.Enums;
using Generator.IO;

namespace Generator.Documentation.CSharp {
	sealed class CSharpDeprecatedWriter : DeprecatedWriter {
		readonly IdentifierConverter idConverter;

		public CSharpDeprecatedWriter(IdentifierConverter idConverter) =>
			this.idConverter = idConverter;

		public override void WriteDeprecated(FileWriter writer, EnumValue value) {
			if (value.DeprecatedInfo.IsDeprecated) {
				if (value.DeprecatedInfo.NewName is not null) {
					var newValue = value.DeclaringType[value.DeprecatedInfo.NewName];
					WriteDeprecated(writer, newValue.Name(idConverter));
				}
				else
					WriteDeprecated(writer, null);
			}
		}

		public override void WriteDeprecated(FileWriter writer, Constant value) {
			if (value.DeprecatedInfo.IsDeprecated) {
				if (value.DeprecatedInfo.NewName is not null) {
					var newValue = value.DeclaringType[value.DeprecatedInfo.NewName];
					WriteDeprecated(writer, newValue.Name(idConverter));
				}
				else
					WriteDeprecated(writer, null);
			}
		}

		public void WriteDeprecated(FileWriter writer, string? newMember, bool isMember = true, bool isError = true) {
			if (newMember is not null) {
				var errStr = isError ? "true" : "false";
				if (isMember)
					writer.WriteLine($"[System.Obsolete(\"Use \" + nameof({newMember}) + \" instead\", {errStr})]");
				else
					writer.WriteLine($"[System.Obsolete(\"Use {newMember} instead\", {errStr})]");
			}
			else {
				// Our code still sometimes needs to reference the deprecated values so don't pass in 'true'
				writer.WriteLine($"[System.Obsolete(\"DEPRECATED. Don't use it!\", false)]");
			}
			writer.WriteLine("[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]");
		}
	}
}
