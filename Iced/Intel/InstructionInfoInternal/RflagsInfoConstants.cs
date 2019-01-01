/*
    Copyright (C) 2018-2019 de4dot@gmail.com

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

#if !NO_INSTR_INFO
using System;

namespace Iced.Intel.InstructionInfoInternal {
	static class RflagsInfoConstants {
		public static readonly ushort[] flagsRead;
		public static readonly ushort[] flagsUndefined;
		public static readonly ushort[] flagsWritten;
		public static readonly ushort[] flagsCleared;
		public static readonly ushort[] flagsSet;
		public static readonly ushort[] flagsModified;

		static RflagsInfoConstants() {
			var flagsRead = new ushort[(int)RflagsInfo.Last];
			var flagsUndefined = new ushort[(int)RflagsInfo.Last];
			var flagsWritten = new ushort[(int)RflagsInfo.Last];
			var flagsCleared = new ushort[(int)RflagsInfo.Last];
			var flagsSet = new ushort[(int)RflagsInfo.Last];
			RflagsInfoConstants.flagsRead = flagsRead;
			RflagsInfoConstants.flagsUndefined = flagsUndefined;
			RflagsInfoConstants.flagsWritten = flagsWritten;
			RflagsInfoConstants.flagsCleared = flagsCleared;
			RflagsInfoConstants.flagsSet = flagsSet;

			for (int i = 0; i < flagsRead.Length; i++) {
				switch ((RflagsInfo)i) {
				case RflagsInfo.None:
					break;

				case RflagsInfo.C_AC:
					flagsCleared[i] = (ushort)RflagsBits.AC;
					break;

				case RflagsInfo.C_cos_S_pz_U_a:
					flagsCleared[i] = (ushort)(RflagsBits.CF | RflagsBits.OF | RflagsBits.SF);
					flagsSet[i] = (ushort)(RflagsBits.PF | RflagsBits.ZF);
					flagsUndefined[i] = (ushort)RflagsBits.AF;
					break;

				case RflagsInfo.C_c:
					flagsCleared[i] = (ushort)RflagsBits.CF;
					break;

				case RflagsInfo.C_d:
					flagsCleared[i] = (ushort)RflagsBits.DF;
					break;

				case RflagsInfo.C_i:
					flagsCleared[i] = (ushort)RflagsBits.IF;
					break;

				case RflagsInfo.R_a_W_ac_U_opsz:
					flagsRead[i] = (ushort)RflagsBits.AF;
					flagsUndefined[i] = (ushort)(RflagsBits.OF | RflagsBits.PF | RflagsBits.SF | RflagsBits.ZF);
					flagsWritten[i] = (ushort)(RflagsBits.AF | RflagsBits.CF);
					break;

				case RflagsInfo.R_ac_W_acpsz_U_o:
					flagsRead[i] = (ushort)(RflagsBits.AF | RflagsBits.CF);
					flagsUndefined[i] = (ushort)RflagsBits.OF;
					flagsWritten[i] = (ushort)(RflagsBits.AF | RflagsBits.CF | RflagsBits.PF | RflagsBits.SF | RflagsBits.ZF);
					break;

				case RflagsInfo.R_acopszid:
					flagsRead[i] = (ushort)(RflagsBits.AF | RflagsBits.CF | RflagsBits.OF | RflagsBits.PF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.IF | RflagsBits.DF);
					break;

				case RflagsInfo.R_acopszidAC:
					flagsRead[i] = (ushort)(RflagsBits.AF | RflagsBits.CF | RflagsBits.OF | RflagsBits.PF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.IF | RflagsBits.DF | RflagsBits.AC);
					break;

				case RflagsInfo.R_acpsz:
					flagsRead[i] = (ushort)(RflagsBits.AF | RflagsBits.CF | RflagsBits.PF | RflagsBits.SF | RflagsBits.ZF);
					break;

				case RflagsInfo.R_c:
					flagsRead[i] = (ushort)RflagsBits.CF;
					break;

				case RflagsInfo.R_c_W_acopsz:
					flagsRead[i] = (ushort)RflagsBits.CF;
					flagsWritten[i] = (ushort)(RflagsBits.AF | RflagsBits.CF | RflagsBits.OF | RflagsBits.PF | RflagsBits.SF | RflagsBits.ZF);
					break;

				case RflagsInfo.R_c_W_c:
					flagsRead[i] = (ushort)RflagsBits.CF;
					flagsWritten[i] = (ushort)RflagsBits.CF;
					break;

				case RflagsInfo.R_c_W_co:
					flagsRead[i] = (ushort)RflagsBits.CF;
					flagsWritten[i] = (ushort)(RflagsBits.CF | RflagsBits.OF);
					break;

				case RflagsInfo.R_cz:
					flagsRead[i] = (ushort)(RflagsBits.CF | RflagsBits.ZF);
					break;

				case RflagsInfo.R_d:
					flagsRead[i] = (ushort)RflagsBits.DF;
					break;

				case RflagsInfo.R_d_W_acopsz:
					flagsRead[i] = (ushort)RflagsBits.DF;
					flagsWritten[i] = (ushort)(RflagsBits.AF | RflagsBits.CF | RflagsBits.OF | RflagsBits.PF | RflagsBits.SF | RflagsBits.ZF);
					break;

				case RflagsInfo.R_o:
					flagsRead[i] = (ushort)RflagsBits.OF;
					break;

				case RflagsInfo.R_o_W_o:
					flagsRead[i] = (ushort)RflagsBits.OF;
					flagsWritten[i] = (ushort)RflagsBits.OF;
					break;

				case RflagsInfo.R_os:
					flagsRead[i] = (ushort)(RflagsBits.OF | RflagsBits.SF);
					break;

				case RflagsInfo.R_osz:
					flagsRead[i] = (ushort)(RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF);
					break;

				case RflagsInfo.R_p:
					flagsRead[i] = (ushort)RflagsBits.PF;
					break;

				case RflagsInfo.R_s:
					flagsRead[i] = (ushort)RflagsBits.SF;
					break;

				case RflagsInfo.R_z:
					flagsRead[i] = (ushort)RflagsBits.ZF;
					break;

				case RflagsInfo.S_AC:
					flagsSet[i] = (ushort)RflagsBits.AC;
					break;

				case RflagsInfo.S_c:
					flagsSet[i] = (ushort)RflagsBits.CF;
					break;

				case RflagsInfo.S_d:
					flagsSet[i] = (ushort)RflagsBits.DF;
					break;

				case RflagsInfo.S_i:
					flagsSet[i] = (ushort)RflagsBits.IF;
					break;

				case RflagsInfo.U_acopsz:
					flagsUndefined[i] = (ushort)(RflagsBits.AF | RflagsBits.CF | RflagsBits.OF | RflagsBits.PF | RflagsBits.SF | RflagsBits.ZF);
					break;

				case RflagsInfo.W_acopsz:
					flagsWritten[i] = (ushort)(RflagsBits.AF | RflagsBits.CF | RflagsBits.OF | RflagsBits.PF | RflagsBits.SF | RflagsBits.ZF);
					break;

				case RflagsInfo.W_acopszid:
					flagsWritten[i] = (ushort)(RflagsBits.AF | RflagsBits.CF | RflagsBits.OF | RflagsBits.PF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.IF | RflagsBits.DF);
					break;

				case RflagsInfo.W_acopszidAC:
					flagsWritten[i] = (ushort)(RflagsBits.AF | RflagsBits.CF | RflagsBits.OF | RflagsBits.PF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.IF | RflagsBits.DF | RflagsBits.AC);
					break;

				case RflagsInfo.W_acpsz:
					flagsWritten[i] = (ushort)(RflagsBits.AF | RflagsBits.CF | RflagsBits.PF | RflagsBits.SF | RflagsBits.ZF);
					break;

				case RflagsInfo.W_aopsz:
					flagsWritten[i] = (ushort)(RflagsBits.AF | RflagsBits.OF | RflagsBits.PF | RflagsBits.SF | RflagsBits.ZF);
					break;

				case RflagsInfo.W_c_C_aopsz:
					flagsWritten[i] = (ushort)RflagsBits.CF;
					flagsCleared[i] = (ushort)(RflagsBits.AF | RflagsBits.OF | RflagsBits.PF | RflagsBits.SF | RflagsBits.ZF);
					break;

				case RflagsInfo.W_c_U_aops:
					flagsUndefined[i] = (ushort)(RflagsBits.AF | RflagsBits.OF | RflagsBits.PF | RflagsBits.SF);
					flagsWritten[i] = (ushort)RflagsBits.CF;
					break;

				case RflagsInfo.W_c:
					flagsWritten[i] = (ushort)RflagsBits.CF;
					break;

				case RflagsInfo.W_co:
					flagsWritten[i] = (ushort)(RflagsBits.CF | RflagsBits.OF);
					break;

				case RflagsInfo.W_co_U_apsz:
					flagsUndefined[i] = (ushort)(RflagsBits.AF | RflagsBits.PF | RflagsBits.SF | RflagsBits.ZF);
					flagsWritten[i] = (ushort)(RflagsBits.CF | RflagsBits.OF);
					break;

				case RflagsInfo.W_copsz_U_a:
					flagsUndefined[i] = (ushort)RflagsBits.AF;
					flagsWritten[i] = (ushort)(RflagsBits.CF | RflagsBits.OF | RflagsBits.PF | RflagsBits.SF | RflagsBits.ZF);
					break;

				case RflagsInfo.W_cosz_C_ap:
					flagsWritten[i] = (ushort)(RflagsBits.CF | RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF);
					flagsCleared[i] = (ushort)(RflagsBits.AF | RflagsBits.PF);
					break;

				case RflagsInfo.W_cpz_C_aos:
					flagsWritten[i] = (ushort)(RflagsBits.CF | RflagsBits.PF | RflagsBits.ZF);
					flagsCleared[i] = (ushort)(RflagsBits.AF | RflagsBits.OF | RflagsBits.SF);
					break;

				case RflagsInfo.W_cs_C_oz_U_ap:
					flagsUndefined[i] = (ushort)(RflagsBits.AF | RflagsBits.PF);
					flagsWritten[i] = (ushort)(RflagsBits.CF | RflagsBits.SF);
					flagsCleared[i] = (ushort)(RflagsBits.OF | RflagsBits.ZF);
					break;

				case RflagsInfo.W_csz_C_o_U_ap:
					flagsUndefined[i] = (ushort)(RflagsBits.AF | RflagsBits.PF);
					flagsWritten[i] = (ushort)(RflagsBits.CF | RflagsBits.SF | RflagsBits.ZF);
					flagsCleared[i] = (ushort)RflagsBits.OF;
					break;

				case RflagsInfo.W_cz_C_aops:
					flagsWritten[i] = (ushort)(RflagsBits.CF | RflagsBits.ZF);
					flagsCleared[i] = (ushort)(RflagsBits.AF | RflagsBits.OF | RflagsBits.PF | RflagsBits.SF);
					break;

				case RflagsInfo.W_cz_U_aops:
					flagsUndefined[i] = (ushort)(RflagsBits.AF | RflagsBits.OF | RflagsBits.PF | RflagsBits.SF);
					flagsWritten[i] = (ushort)(RflagsBits.CF | RflagsBits.ZF);
					break;

				case RflagsInfo.W_psz_C_co_U_a:
					flagsUndefined[i] = (ushort)RflagsBits.AF;
					flagsWritten[i] = (ushort)(RflagsBits.PF | RflagsBits.SF | RflagsBits.ZF);
					flagsCleared[i] = (ushort)(RflagsBits.CF | RflagsBits.OF);
					break;

				case RflagsInfo.W_psz_U_aco:
					flagsUndefined[i] = (ushort)(RflagsBits.AF | RflagsBits.CF | RflagsBits.OF);
					flagsWritten[i] = (ushort)(RflagsBits.PF | RflagsBits.SF | RflagsBits.ZF);
					break;

				case RflagsInfo.W_sz_C_co_U_ap:
					flagsUndefined[i] = (ushort)(RflagsBits.AF | RflagsBits.PF);
					flagsWritten[i] = (ushort)(RflagsBits.SF | RflagsBits.ZF);
					flagsCleared[i] = (ushort)(RflagsBits.CF | RflagsBits.OF);
					break;

				case RflagsInfo.W_z:
					flagsWritten[i] = (ushort)RflagsBits.ZF;
					break;

				case RflagsInfo.W_z_C_acops:
					flagsWritten[i] = (ushort)RflagsBits.ZF;
					flagsCleared[i] = (ushort)(RflagsBits.AF | RflagsBits.CF | RflagsBits.OF | RflagsBits.PF | RflagsBits.SF);
					break;

				case RflagsInfo.W_z_C_co_U_aps:
					flagsUndefined[i] = (ushort)(RflagsBits.AF | RflagsBits.PF | RflagsBits.SF);
					flagsWritten[i] = (ushort)RflagsBits.ZF;
					flagsCleared[i] = (ushort)(RflagsBits.CF | RflagsBits.OF);
					break;

				case RflagsInfo.W_z_U_acops:
					flagsUndefined[i] = (ushort)(RflagsBits.AF | RflagsBits.CF | RflagsBits.OF | RflagsBits.PF | RflagsBits.SF);
					flagsWritten[i] = (ushort)RflagsBits.ZF;
					break;

				default:
					throw new InvalidOperationException();
				}
			}

			var flagsModified = new ushort[(int)RflagsInfo.Last];
			RflagsInfoConstants.flagsModified = flagsModified;
			for (int i = 0; i < flagsModified.Length; i++)
				flagsModified[i] = (ushort)(flagsUndefined[i] | flagsWritten[i] | flagsCleared[i] | flagsSet[i]);
		}
	}
}
#endif
