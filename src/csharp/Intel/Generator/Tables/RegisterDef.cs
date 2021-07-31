// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using Generator.Enums;

namespace Generator.Tables {
	[Enum("RegisterKind")]
	enum RegisterKind {
		None,
		GPR8,
		GPR16,
		GPR32,
		GPR64,
		IP,
		Segment,
		ST,
		CR,
		DR,
		TR,
		BND,
		K,
		MM,
		XMM,
		YMM,
		ZMM,
		TMM,
	}

	[Enum("RegisterClass")]
	enum RegisterClass {
		None,
		GPR,
		IP,
		Segment,
		ST,
		CR,
		DR,
		TR,
		BND,
		K,
		MM,
		Vector,
		TMM,
	}

	sealed class RegisterDef {
		public readonly EnumValue Register;
		public readonly EnumValue BaseRegister;
		public readonly EnumValue FullRegister32;
		public readonly EnumValue FullRegister;
		public readonly EnumValue RegisterClass;
		public readonly EnumValue RegisterKind;
		public readonly uint Index;
		public readonly uint Size;
		public readonly string Name;

		public RegisterDef(EnumValue register, EnumValue baseRegister, EnumValue fullRegister32, EnumValue fullRegister, EnumValue registerClass, EnumValue registerKind, uint size, string name) {
			Register = register;
			BaseRegister = baseRegister;
			FullRegister32 = fullRegister32;
			FullRegister = fullRegister;
			RegisterClass = registerClass;
			RegisterKind = registerKind;
			Size = size;
			if (register.Value < baseRegister.Value)
				throw new InvalidOperationException();
			Index = register.Value - baseRegister.Value;
			Name = name;
		}

		public Tables.RegisterKind GetRegisterKind() => (Tables.RegisterKind)RegisterKind.Value;
		public Tables.RegisterClass GetRegisterClass() => (Tables.RegisterClass)RegisterClass.Value;
	}
}
