// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using Generator.Enums;
using Generator.Enums.Decoder;

namespace Generator.Decoder {
	static class DecoderTable_XOP {
		public const string Handlers_MAP8 = nameof(Handlers_MAP8);
		public const string Handlers_MAP9 = nameof(Handlers_MAP9);
		public const string Handlers_MAP10 = nameof(Handlers_MAP10);

		public static (string name, object?[] handlers)[] CreateHandlers(GenTypes genTypes) {
			var kind = genTypes[TypeIds.VexOpCodeHandlerKind];
			var code = genTypes[TypeIds.Code];
			var reg = genTypes[TypeIds.Register];

			var Group = kind[nameof(VexOpCodeHandlerKind.Group)];
			var W = kind[nameof(VexOpCodeHandlerKind.W)];
			var MandatoryPrefix2_1 = kind[nameof(VexOpCodeHandlerKind.MandatoryPrefix2_1)];
			var VectorLength = kind[nameof(VexOpCodeHandlerKind.VectorLength)];
			var Gv_Ev_Id = kind[nameof(VexOpCodeHandlerKind.Gv_Ev_Id)];
			var Hv_Ed_Id = kind[nameof(VexOpCodeHandlerKind.Hv_Ed_Id)];
			var Hv_Ev = kind[nameof(VexOpCodeHandlerKind.Hv_Ev)];
			var RdRq = kind[nameof(VexOpCodeHandlerKind.RdRq)];
			var VHIs4W = kind[nameof(VexOpCodeHandlerKind.VHIs4W)];
			var VHW_2 = kind[nameof(VexOpCodeHandlerKind.VHW_2)];
			var VHWIb_2 = kind[nameof(VexOpCodeHandlerKind.VHWIb_2)];
			var VHWIs4 = kind[nameof(VexOpCodeHandlerKind.VHWIs4)];
			var VW_2 = kind[nameof(VexOpCodeHandlerKind.VW_2)];
			var VWH = kind[nameof(VexOpCodeHandlerKind.VWH)];
			var VWIb_2 = kind[nameof(VexOpCodeHandlerKind.VWIb_2)];

			var xmm0 = reg[nameof(Register.XMM0)];
			var ymm0 = reg[nameof(Register.YMM0)];

			var invalid = new object[] { kind[nameof(VexOpCodeHandlerKind.Invalid)] };
			var handlers = new (string name, object?[] handlers)[] {
				("grp_MAP9_01",
				new object[8] {
					invalid,
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { Hv_Ev, code[nameof(Code.XOP_Blcfill_r32_rm32)], code[nameof(Code.XOP_Blcfill_r64_rm64)] },
							invalid,
						}
					},
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { Hv_Ev, code[nameof(Code.XOP_Blsfill_r32_rm32)], code[nameof(Code.XOP_Blsfill_r64_rm64)] },
							invalid,
						}
					},
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { Hv_Ev, code[nameof(Code.XOP_Blcs_r32_rm32)], code[nameof(Code.XOP_Blcs_r64_rm64)] },
							invalid,
						}
					},
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { Hv_Ev, code[nameof(Code.XOP_Tzmsk_r32_rm32)], code[nameof(Code.XOP_Tzmsk_r64_rm64)] },
							invalid,
						}
					},
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { Hv_Ev, code[nameof(Code.XOP_Blcic_r32_rm32)], code[nameof(Code.XOP_Blcic_r64_rm64)] },
							invalid,
						}
					},
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { Hv_Ev, code[nameof(Code.XOP_Blsic_r32_rm32)], code[nameof(Code.XOP_Blsic_r64_rm64)] },
							invalid,
						}
					},
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { Hv_Ev, code[nameof(Code.XOP_T1mskc_r32_rm32)], code[nameof(Code.XOP_T1mskc_r64_rm64)] },
							invalid,
						}
					},
				}),

				("grp_MAP9_02",
				new object[8] {
					invalid,
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { Hv_Ev, code[nameof(Code.XOP_Blcmsk_r32_rm32)], code[nameof(Code.XOP_Blcmsk_r64_rm64)] },
							invalid,
						}
					},
					invalid,
					invalid,
					invalid,
					invalid,
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { Hv_Ev, code[nameof(Code.XOP_Blci_r32_rm32)], code[nameof(Code.XOP_Blci_r64_rm64)] },
							invalid,
						}
					},
					invalid,
				}),

				("grp_MAP9_12",
				new object[8] {
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { RdRq, code[nameof(Code.XOP_Llwpcb_r32)], code[nameof(Code.XOP_Llwpcb_r64)] },
							invalid,
						}
					},
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { RdRq, code[nameof(Code.XOP_Slwpcb_r32)], code[nameof(Code.XOP_Slwpcb_r64)] },
							invalid,
						}
					},
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
				}),

				("grp_MAP10_12",
				new object[8] {
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { Hv_Ed_Id, code[nameof(Code.XOP_Lwpins_r32_rm32_imm32)], code[nameof(Code.XOP_Lwpins_r64_rm32_imm32)] },
							invalid,
						}
					},
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { Hv_Ed_Id, code[nameof(Code.XOP_Lwpval_r32_rm32_imm32)], code[nameof(Code.XOP_Lwpval_r64_rm32_imm32)] },
							invalid,
						}
					},
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
				}),

				(Handlers_MAP8,
				new object[0x100] {
					// 00
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 08
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 10
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 18
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 20
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 28
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 30
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 38
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 40
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 48
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 50
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 58
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 60
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 68
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 70
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 78
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 80
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VHWIs4, xmm0, code[nameof(Code.XOP_Vpmacssww_xmm_xmm_xmmm128_xmm)] },
								invalid,
							},
							invalid,
						}
					},
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VHWIs4, xmm0, code[nameof(Code.XOP_Vpmacsswd_xmm_xmm_xmmm128_xmm)] },
								invalid,
							},
							invalid,
						}
					},
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VHWIs4, xmm0, code[nameof(Code.XOP_Vpmacssdql_xmm_xmm_xmmm128_xmm)] },
								invalid,
							},
							invalid,
						}
					},

					// 88
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VHWIs4, xmm0, code[nameof(Code.XOP_Vpmacssdd_xmm_xmm_xmmm128_xmm)] },
								invalid,
							},
							invalid,
						}
					},
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VHWIs4, xmm0, code[nameof(Code.XOP_Vpmacssdqh_xmm_xmm_xmmm128_xmm)] },
								invalid,
							},
							invalid,
						}
					},

					// 90
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VHWIs4, xmm0, code[nameof(Code.XOP_Vpmacsww_xmm_xmm_xmmm128_xmm)] },
								invalid,
							},
							invalid,
						}
					},
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VHWIs4, xmm0, code[nameof(Code.XOP_Vpmacswd_xmm_xmm_xmmm128_xmm)] },
								invalid,
							},
							invalid,
						}
					},
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VHWIs4, xmm0, code[nameof(Code.XOP_Vpmacsdql_xmm_xmm_xmmm128_xmm)] },
								invalid,
							},
							invalid,
						}
					},

					// 98
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VHWIs4, xmm0, code[nameof(Code.XOP_Vpmacsdd_xmm_xmm_xmmm128_xmm)] },
								invalid,
							},
							invalid,
						}
					},
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VHWIs4, xmm0, code[nameof(Code.XOP_Vpmacsdqh_xmm_xmm_xmmm128_xmm)] },
								invalid,
							},
							invalid,
						}
					},

					// A0
					invalid,
					invalid,
					new object[] { MandatoryPrefix2_1,
						new object[] { W,
							new object[] { VectorLength,
								new object[] { VHWIs4, xmm0, code[nameof(Code.XOP_Vpcmov_xmm_xmm_xmmm128_xmm)] },
								new object[] { VHWIs4, ymm0, code[nameof(Code.XOP_Vpcmov_ymm_ymm_ymmm256_ymm)] },
							},
							new object[] { VectorLength,
								new object[] { VHIs4W, xmm0, code[nameof(Code.XOP_Vpcmov_xmm_xmm_xmm_xmmm128)] },
								new object[] { VHIs4W, ymm0, code[nameof(Code.XOP_Vpcmov_ymm_ymm_ymm_ymmm256)] },
							}
						}
					},
					new object[] { MandatoryPrefix2_1,
						new object[] { W,
							new object[] { VectorLength,
								new object[] { VHWIs4, xmm0, code[nameof(Code.XOP_Vpperm_xmm_xmm_xmmm128_xmm)] },
								invalid,
							},
							new object[] { VectorLength,
								new object[] { VHIs4W, xmm0, code[nameof(Code.XOP_Vpperm_xmm_xmm_xmm_xmmm128)] },
								invalid,
							}
						}
					},
					invalid,
					invalid,
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VHWIs4, xmm0, code[nameof(Code.XOP_Vpmadcsswd_xmm_xmm_xmmm128_xmm)] },
								invalid,
							},
							invalid,
						}
					},
					invalid,

					// A8
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// B0
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VHWIs4, xmm0, code[nameof(Code.XOP_Vpmadcswd_xmm_xmm_xmmm128_xmm)] },
								invalid,
							},
							invalid,
						}
					},
					invalid,

					// B8
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// C0
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VWIb_2, xmm0, code[nameof(Code.XOP_Vprotb_xmm_xmmm128_imm8)] },
								invalid,
							},
							invalid,
						}
					},
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VWIb_2, xmm0, code[nameof(Code.XOP_Vprotw_xmm_xmmm128_imm8)] },
								invalid,
							},
							invalid,
						}
					},
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VWIb_2, xmm0, code[nameof(Code.XOP_Vprotd_xmm_xmmm128_imm8)] },
								invalid,
							},
							invalid,
						}
					},
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VWIb_2, xmm0, code[nameof(Code.XOP_Vprotq_xmm_xmmm128_imm8)] },
								invalid,
							},
							invalid,
						}
					},
					invalid,
					invalid,
					invalid,
					invalid,

					// C8
					invalid,
					invalid,
					invalid,
					invalid,
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VHWIb_2, xmm0, code[nameof(Code.XOP_Vpcomb_xmm_xmm_xmmm128_imm8)] },
								invalid,
							},
							invalid,
						}
					},
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VHWIb_2, xmm0, code[nameof(Code.XOP_Vpcomw_xmm_xmm_xmmm128_imm8)] },
								invalid,
							},
							invalid,
						}
					},
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VHWIb_2, xmm0, code[nameof(Code.XOP_Vpcomd_xmm_xmm_xmmm128_imm8)] },
								invalid,
							},
							invalid,
						}
					},
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VHWIb_2, xmm0, code[nameof(Code.XOP_Vpcomq_xmm_xmm_xmmm128_imm8)] },
								invalid,
							},
							invalid,
						}
					},

					// D0
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// D8
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// E0
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// E8
					invalid,
					invalid,
					invalid,
					invalid,
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VHWIb_2, xmm0, code[nameof(Code.XOP_Vpcomub_xmm_xmm_xmmm128_imm8)] },
								invalid,
							},
							invalid,
						}
					},
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VHWIb_2, xmm0, code[nameof(Code.XOP_Vpcomuw_xmm_xmm_xmmm128_imm8)] },
								invalid,
							},
							invalid,
						}
					},
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VHWIb_2, xmm0, code[nameof(Code.XOP_Vpcomud_xmm_xmm_xmmm128_imm8)] },
								invalid,
							},
							invalid,
						}
					},
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VHWIb_2, xmm0, code[nameof(Code.XOP_Vpcomuq_xmm_xmm_xmmm128_imm8)] },
								invalid,
							},
							invalid,
						}
					},

					// F0
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// F8
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
				}),

				(Handlers_MAP9,
				new object[0x100] {
					// 00
					invalid,
					new object[] { Group, "grp_MAP9_01" },
					new object[] { Group, "grp_MAP9_02" },
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 08
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 10
					invalid,
					invalid,
					new object[] { Group, "grp_MAP9_12" },
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 18
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 20
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 28
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 30
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 38
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 40
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 48
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 50
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 58
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 60
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 68
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 70
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 78
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 80
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VW_2, xmm0, code[nameof(Code.XOP_Vfrczps_xmm_xmmm128)] },
								invalid,
							},
							new object[] { W,
								new object[] { VW_2, ymm0, code[nameof(Code.XOP_Vfrczps_ymm_ymmm256)] },
								invalid,
							}
						}
					},
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VW_2, xmm0, code[nameof(Code.XOP_Vfrczpd_xmm_xmmm128)] },
								invalid,
							},
							new object[] { W,
								new object[] { VW_2, ymm0, code[nameof(Code.XOP_Vfrczpd_ymm_ymmm256)] },
								invalid,
							}
						}
					},
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VW_2, xmm0, code[nameof(Code.XOP_Vfrczss_xmm_xmmm32)] },
								invalid,
							},
							invalid,
						}
					},
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VW_2, xmm0, code[nameof(Code.XOP_Vfrczsd_xmm_xmmm64)] },
								invalid,
							},
							invalid,
						}
					},
					invalid,
					invalid,
					invalid,
					invalid,

					// 88
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 90
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VWH, xmm0, code[nameof(Code.XOP_Vprotb_xmm_xmmm128_xmm)] },
								new object[] { VHW_2, xmm0, code[nameof(Code.XOP_Vprotb_xmm_xmm_xmmm128)] },
							},
							invalid,
						}
					},
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VWH, xmm0, code[nameof(Code.XOP_Vprotw_xmm_xmmm128_xmm)] },
								new object[] { VHW_2, xmm0, code[nameof(Code.XOP_Vprotw_xmm_xmm_xmmm128)] },
							},
							invalid,
						}
					},
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VWH, xmm0, code[nameof(Code.XOP_Vprotd_xmm_xmmm128_xmm)] },
								new object[] { VHW_2, xmm0, code[nameof(Code.XOP_Vprotd_xmm_xmm_xmmm128)] },
							},
							invalid,
						}
					},
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VWH, xmm0, code[nameof(Code.XOP_Vprotq_xmm_xmmm128_xmm)] },
								new object[] { VHW_2, xmm0, code[nameof(Code.XOP_Vprotq_xmm_xmm_xmmm128)] },
							},
							invalid,
						}
					},
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VWH, xmm0, code[nameof(Code.XOP_Vpshlb_xmm_xmmm128_xmm)] },
								new object[] { VHW_2, xmm0, code[nameof(Code.XOP_Vpshlb_xmm_xmm_xmmm128)] },
							},
							invalid,
						}
					},
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VWH, xmm0, code[nameof(Code.XOP_Vpshlw_xmm_xmmm128_xmm)] },
								new object[] { VHW_2, xmm0, code[nameof(Code.XOP_Vpshlw_xmm_xmm_xmmm128)] },
							},
							invalid,
						}
					},
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VWH, xmm0, code[nameof(Code.XOP_Vpshld_xmm_xmmm128_xmm)] },
								new object[] { VHW_2, xmm0, code[nameof(Code.XOP_Vpshld_xmm_xmm_xmmm128)] },
							},
							invalid,
						}
					},
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VWH, xmm0, code[nameof(Code.XOP_Vpshlq_xmm_xmmm128_xmm)] },
								new object[] { VHW_2, xmm0, code[nameof(Code.XOP_Vpshlq_xmm_xmm_xmmm128)] },
							},
							invalid,
						}
					},

					// 98
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VWH, xmm0, code[nameof(Code.XOP_Vpshab_xmm_xmmm128_xmm)] },
								new object[] { VHW_2, xmm0, code[nameof(Code.XOP_Vpshab_xmm_xmm_xmmm128)] },
							},
							invalid,
						}
					},
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VWH, xmm0, code[nameof(Code.XOP_Vpshaw_xmm_xmmm128_xmm)] },
								new object[] { VHW_2, xmm0, code[nameof(Code.XOP_Vpshaw_xmm_xmm_xmmm128)] },
							},
							invalid,
						}
					},
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VWH, xmm0, code[nameof(Code.XOP_Vpshad_xmm_xmmm128_xmm)] },
								new object[] { VHW_2, xmm0, code[nameof(Code.XOP_Vpshad_xmm_xmm_xmmm128)] },
							},
							invalid,
						}
					},
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VWH, xmm0, code[nameof(Code.XOP_Vpshaq_xmm_xmmm128_xmm)] },
								new object[] { VHW_2, xmm0, code[nameof(Code.XOP_Vpshaq_xmm_xmm_xmmm128)] },
							},
							invalid,
						}
					},
					invalid,
					invalid,
					invalid,
					invalid,

					// A0
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// A8
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// B0
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// B8
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// C0
					invalid,
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VW_2, xmm0, code[nameof(Code.XOP_Vphaddbw_xmm_xmmm128)] },
								invalid,
							},
							invalid,
						}
					},
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VW_2, xmm0, code[nameof(Code.XOP_Vphaddbd_xmm_xmmm128)] },
								invalid,
							},
							invalid,
						}
					},
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VW_2, xmm0, code[nameof(Code.XOP_Vphaddbq_xmm_xmmm128)] },
								invalid,
							},
							invalid,
						}
					},
					invalid,
					invalid,
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VW_2, xmm0, code[nameof(Code.XOP_Vphaddwd_xmm_xmmm128)] },
								invalid,
							},
							invalid,
						}
					},
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VW_2, xmm0, code[nameof(Code.XOP_Vphaddwq_xmm_xmmm128)] },
								invalid,
							},
							invalid,
						}
					},

					// C8
					invalid,
					invalid,
					invalid,
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VW_2, xmm0, code[nameof(Code.XOP_Vphadddq_xmm_xmmm128)] },
								invalid,
							},
							invalid,
						}
					},
					invalid,
					invalid,
					invalid,
					invalid,

					// D0
					invalid,
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VW_2, xmm0, code[nameof(Code.XOP_Vphaddubw_xmm_xmmm128)] },
								invalid,
							},
							invalid,
						}
					},
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VW_2, xmm0, code[nameof(Code.XOP_Vphaddubd_xmm_xmmm128)] },
								invalid,
							},
							invalid,
						}
					},
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VW_2, xmm0, code[nameof(Code.XOP_Vphaddubq_xmm_xmmm128)] },
								invalid,
							},
							invalid,
						}
					},
					invalid,
					invalid,
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VW_2, xmm0, code[nameof(Code.XOP_Vphadduwd_xmm_xmmm128)] },
								invalid,
							},
							invalid,
						}
					},
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VW_2, xmm0, code[nameof(Code.XOP_Vphadduwq_xmm_xmmm128)] },
								invalid,
							},
							invalid,
						}
					},

					// D8
					invalid,
					invalid,
					invalid,
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VW_2, xmm0, code[nameof(Code.XOP_Vphaddudq_xmm_xmmm128)] },
								invalid,
							},
							invalid,
						}
					},
					invalid,
					invalid,
					invalid,
					invalid,

					// E0
					invalid,
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VW_2, xmm0, code[nameof(Code.XOP_Vphsubbw_xmm_xmmm128)] },
								invalid,
							},
							invalid,
						}
					},
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VW_2, xmm0, code[nameof(Code.XOP_Vphsubwd_xmm_xmmm128)] },
								invalid,
							},
							invalid,
						}
					},
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { W,
								new object[] { VW_2, xmm0, code[nameof(Code.XOP_Vphsubdq_xmm_xmmm128)] },
								invalid,
							},
							invalid,
						}
					},
					invalid,
					invalid,
					invalid,
					invalid,

					// E8
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// F0
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// F8
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
				}),

				(Handlers_MAP10,
				new object[0x100] {
					// 00
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 08
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 10
					new object[] { MandatoryPrefix2_1,
						new object[] { VectorLength,
							new object[] { Gv_Ev_Id, code[nameof(Code.XOP_Bextr_r32_rm32_imm32)], code[nameof(Code.XOP_Bextr_r64_rm64_imm32)] },
							invalid,
						}
					},
					invalid,
					new object[] { Group, "grp_MAP10_12" },
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 18
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 20
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 28
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 30
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 38
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 40
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 48
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 50
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 58
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 60
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 68
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 70
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 78
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 80
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 88
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 90
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// 98
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// A0
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// A8
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// B0
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// B8
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// C0
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// C8
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// D0
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// D8
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// E0
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// E8
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// F0
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,

					// F8
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
					invalid,
				}),
			};
			return handlers;
		}
	}
}
