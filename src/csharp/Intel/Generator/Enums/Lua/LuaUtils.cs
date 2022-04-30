// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Enums.Lua {
	static class LuaUtils {
		public static (string name, string value) GetEnumNameValue(IdentifierConverter idConverter, EnumValue value) {
			var numStr = value.DeclaringType.IsFlags ? "0x" + value.Value.ToString("X8") : value.Value.ToString();
			var valueName = value.Name(idConverter);
			return (valueName, numStr);
		}
	}
}
