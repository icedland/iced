// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if FAST_FMT
namespace Iced.Intel {
	/// <summary>
	/// Fast formatter options
	/// </summary>
	public sealed class FastFormatterOptions {
		Flags1 flags1;

		enum Flags1 : uint {
			None							= 0,
			SpaceAfterOperandSeparator		= 0x00000001,
			RipRelativeAddresses			= 0x00000002,
			UsePseudoOps					= 0x00000004,
			ShowSymbolAddress				= 0x00000008,
			AlwaysShowSegmentRegister		= 0x00000010,
			AlwaysShowMemorySize			= 0x00000020,
			UppercaseHex					= 0x00000040,
			UseHexPrefix					= 0x00000080,
		}

		internal FastFormatterOptions() =>
			flags1 = Flags1.UsePseudoOps | Flags1.UppercaseHex;

		/// <summary>
		/// Add a space after the operand separator
		/// <br/>
		/// Default: <see langword="false"/>
		/// <br/>
		/// <see langword="true"/>: <c>mov rax, rcx</c>
		/// <br/>
		/// <see langword="false"/>: <c>mov rax,rcx</c>
		/// </summary>
		public bool SpaceAfterOperandSeparator {
			get => (flags1 & Flags1.SpaceAfterOperandSeparator) != 0;
			set {
				if (value)
					flags1 |= Flags1.SpaceAfterOperandSeparator;
				else
					flags1 &= ~Flags1.SpaceAfterOperandSeparator;
			}
		}

		/// <summary>
		/// Show <c>RIP+displ</c> or the virtual address
		/// <br/>
		/// Default: <see langword="false"/>
		/// <br/>
		/// <see langword="true"/>: <c>mov eax,[rip+12345678h]</c>
		/// <br/>
		/// <see langword="false"/>: <c>mov eax,[1029384756AFBECDh]</c>
		/// </summary>
		public bool RipRelativeAddresses {
			get => (flags1 & Flags1.RipRelativeAddresses) != 0;
			set {
				if (value)
					flags1 |= Flags1.RipRelativeAddresses;
				else
					flags1 &= ~Flags1.RipRelativeAddresses;
			}
		}

		/// <summary>
		/// Use pseudo instructions
		/// <br/>
		/// Default: <see langword="true"/>
		/// <br/>
		/// <see langword="true"/>: <c>vcmpnltsd xmm2,xmm6,xmm3</c>
		/// <br/>
		/// <see langword="false"/>: <c>vcmpsd xmm2,xmm6,xmm3,5</c>
		/// </summary>
		public bool UsePseudoOps {
			get => (flags1 & Flags1.UsePseudoOps) != 0;
			set {
				if (value)
					flags1 |= Flags1.UsePseudoOps;
				else
					flags1 &= ~Flags1.UsePseudoOps;
			}
		}

		/// <summary>
		/// Show the original value after the symbol name
		/// <br/>
		/// Default: <see langword="false"/>
		/// <br/>
		/// <see langword="true"/>: <c>mov eax,[myfield (12345678)]</c>
		/// <br/>
		/// <see langword="false"/>: <c>mov eax,[myfield]</c>
		/// </summary>
		public bool ShowSymbolAddress {
			get => (flags1 & Flags1.ShowSymbolAddress) != 0;
			set {
				if (value)
					flags1 |= Flags1.ShowSymbolAddress;
				else
					flags1 &= ~Flags1.ShowSymbolAddress;
			}
		}

		/// <summary>
		/// Always show the effective segment register. If the option is <see langword="false"/>, only show the segment register if
		/// there's a segment override prefix.
		/// <br/>
		/// Default: <see langword="false"/>
		/// <br/>
		/// <see langword="true"/>: <c>mov eax,ds:[ecx]</c>
		/// <br/>
		/// <see langword="false"/>: <c>mov eax,[ecx]</c>
		/// </summary>
		public bool AlwaysShowSegmentRegister {
			get => (flags1 & Flags1.AlwaysShowSegmentRegister) != 0;
			set {
				if (value)
					flags1 |= Flags1.AlwaysShowSegmentRegister;
				else
					flags1 &= ~Flags1.AlwaysShowSegmentRegister;
			}
		}

		/// <summary>
		/// Always show memory operands' size
		/// <br/>
		/// Default: <see langword="false"/>
		/// <br/>
		/// <see langword="true"/>: <c>mov eax,dword ptr [ebx]</c> / <c>add byte ptr [eax],0x12</c>
		/// <br/>
		/// <see langword="false"/>: <c>mov eax,[ebx]</c> / <c>add byte ptr [eax],0x12</c>
		/// </summary>
		public bool AlwaysShowMemorySize {
			get => (flags1 & Flags1.AlwaysShowMemorySize) != 0;
			set {
				if (value)
					flags1 |= Flags1.AlwaysShowMemorySize;
				else
					flags1 &= ~Flags1.AlwaysShowMemorySize;
			}
		}

		/// <summary>
		/// Use uppercase hex digits
		/// <br/>
		/// Default: <see langword="true"/>
		/// <br/>
		/// <see langword="true"/>: <c>0xFF</c>
		/// <br/>
		/// <see langword="false"/>: <c>0xff</c>
		/// </summary>
		public bool UppercaseHex {
			get => (flags1 & Flags1.UppercaseHex) != 0;
			set {
				if (value)
					flags1 |= Flags1.UppercaseHex;
				else
					flags1 &= ~Flags1.UppercaseHex;
			}
		}

		/// <summary>
		/// Use a hex prefix (<c>0x</c>) or a hex suffix (<c>h</c>)
		/// <br/>
		/// Default: <see langword="false"/>
		/// <br/>
		/// <see langword="true"/>: <c>0x5A</c>
		/// <br/>
		/// <see langword="false"/>: <c>5Ah</c>
		/// </summary>
		public bool UseHexPrefix {
			get => (flags1 & Flags1.UseHexPrefix) != 0;
			set {
				if (value)
					flags1 |= Flags1.UseHexPrefix;
				else
					flags1 &= ~Flags1.UseHexPrefix;
			}
		}
	}
}
#endif
