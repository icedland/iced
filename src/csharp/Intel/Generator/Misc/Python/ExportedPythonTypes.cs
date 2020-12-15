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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Generator.Enums;

namespace Generator.Misc.Python {
	[TypeGen(TypeGenOrders.NoDeps)]
	sealed class InitExportedPythonTypes {
		InitExportedPythonTypes(GenTypes genTypes) =>
			genTypes.AddObject(TypeIds.ExportedPythonTypes, new ExportedPythonTypes());
	}

	sealed class ExportedPythonTypes {
		readonly Dictionary<string, EnumType> toEnumType = new Dictionary<string, EnumType>(StringComparer.Ordinal);
		public List<EnumType> IntEnums { get; } = new List<EnumType>();
		public List<EnumType> IntFlags { get; } = new List<EnumType>();

		public void AddIntFlag(EnumType enumType) {
			IntFlags.Add(enumType);
			toEnumType.Add(enumType.RawName, enumType);
		}

		public void AddIntEnum(EnumType enumType) {
			IntEnums.Add(enumType);
			toEnumType.Add(enumType.RawName, enumType);
		}

		public bool TryFindByName(string name, [NotNullWhen(true)] out EnumType? enumType) =>
			toEnumType.TryGetValue(name, out enumType);
	}
}
