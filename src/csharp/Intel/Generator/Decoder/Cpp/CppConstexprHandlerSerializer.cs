// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Generator.IO;
using Generator.Enums;
using Generator.Enums.Encoder;
using Generator.Enums;
using Generator.Enums.Decoder;

namespace Generator.Decoder.Cpp {
	sealed class CppConstexprHandlerSerializer : DecoderTableSerializer {
		readonly string tableName;
		readonly Dictionary<object, string> generatedHandlers = new();
		readonly StringBuilder handlerDeclarations = new();
		readonly StringBuilder tableDeclarations = new();
		int handlerCounter = 0;

		public string TableName => tableName;

		public CppConstexprHandlerSerializer(GenTypes genTypes, string tableName, DecoderTableSerializerInfo info)
			: base(genTypes, CppIdentifierConverter.Create(), info) {
			this.tableName = tableName;
		}

		public void Serialize(FileWriter writer) {
			GenerateConstexprHandlers();
			GenerateConstexprTables();

			writer.WriteFileHeader();

			writer.WriteLine("#pragma once");
			writer.WriteLine($"#ifndef ICED_X86_INTERNAL_CONSTEXPR_{TableName.ToUpperInvariant()}_HPP");
			writer.WriteLine($"#define ICED_X86_INTERNAL_CONSTEXPR_{TableName.ToUpperInvariant()}_HPP");
			writer.WriteLine();
			writer.WriteLine("#include \"iced_x86/internal/handlers.hpp\"");
			writer.WriteLine("#include \"iced_x86/internal/handlers_table.hpp\"");
			writer.WriteLine("#include \"iced_x86/decoder_options.hpp\"");
			writer.WriteLine("#include <cstdint>");
			writer.WriteLine("#include <cstddef>");
			writer.WriteLine("#include <array>");
			writer.WriteLine();
			writer.WriteLine("namespace iced_x86 {");
			writer.WriteLine("namespace internal {");
			writer.WriteLine("namespace constexpr_handlers {");
			writer.WriteLine();
			writer.WriteLine("// Compile-time generated handler instances");
			writer.WriteLine("// These replace runtime deserialization with constexpr evaluation");
			writer.Write(handlerDeclarations.ToString());
			writer.WriteLine();
			writer.WriteLine("// Handler tables");
			writer.Write(tableDeclarations.ToString());
			writer.WriteLine();
			writer.WriteLine("} // namespace constexpr_handlers");
			writer.WriteLine("} // namespace internal");
			writer.WriteLine("} // namespace iced_x86");
			writer.WriteLine();
			writer.WriteLine($"#endif // ICED_X86_INTERNAL_CONSTEXPR_{TableName.ToUpperInvariant()}_HPP");
		}

		void GenerateConstexprHandlers() {
			handlerDeclarations.Clear();

			foreach (var table in info.TablesToSerialize) {
				foreach (var handler in table.handlers) {
					if (handler is not null) {
						GenerateHandlerRecursive(handler);
					}
				}
			}
		}

		void GenerateHandlerRecursive(object handler) {
			if (generatedHandlers.ContainsKey(handler))
				return; // Already generated

			// Check if this is a single EnumValue (special handler like Null, Invalid, etc.)
			// These don't get generated as separate handler structs
			if (handler is EnumValue enumValue) {
				// Special handler kinds are referenced directly, not as separate declarations
				// Mark them as "special" so table generation knows to use get_null_handler() etc.
				generatedHandlers[handler] = GetSpecialHandlerReference(enumValue);
				return;
			}

			// Only increment counter and generate for array-based handlers
			if (handler is object[] handlerArray && handlerArray.Length > 0) {
				// Use table prefix to avoid name collisions when multiple tables are included
				string prefix = tableName switch {
					"legacy" => "leg",
					"vex" => "vex",
					"evex" => "evx",
					"xop" => "xop",
					"mvex" => "mvx",
					_ => "h"
				};
				string handlerName = $"{prefix}_{handlerCounter++:D4}";
				generatedHandlers[handler] = handlerName;

				string declaration = GenerateHandlerDeclaration(handlerArray, handlerName);
				if (!string.IsNullOrEmpty(declaration)) {
					handlerDeclarations.AppendLine(declaration);
				}
			}
		}

		string GetSpecialHandlerReference(EnumValue enumValue) {
			// Map special handler enum values to their C++ equivalents
			string rawName = enumValue.RawName;
			return rawName switch {
				"Null" or "Invalid" => "@@null@@", // Special marker for null handler
				_ => $"@@unknown:{rawName}@@" // Unknown special handler
			};
		}

		string GenerateHandlerDeclaration(object[] handlerArray, string handlerName) {
			if (handlerArray.Length == 0)
				return "// Empty handler array";

			return GenerateSimpleHandlerDeclaration(handlerArray, handlerName);
		}

		string GenerateSimpleHandlerDeclaration(object[] handlerArray, string handlerName) {
			if (handlerArray.Length == 0)
				return "// Empty handler array";

			var kind = handlerArray[0];
			if (kind is not EnumValue enumValue)
				return $"// Invalid handler kind: {kind?.GetType().Name ?? "null"}";

			string cppType = GetCppHandlerType(enumValue.RawName);
			if (string.IsNullOrEmpty(cppType))
				return $"// Unsupported handler kind: \"{enumValue.RawName}\" (table: {tableName})";

			// Extract parameters (skip the kind at index 0)
			var parameters = new List<string>();
			bool hasNonConstexprParams = false;
			for (int i = 1; i < handlerArray.Length; i++) {
				// Check if this parameter requires non-constexpr (nested handlers or string refs)
				if (handlerArray[i] is object[] || handlerArray[i] is null || handlerArray[i] is string) {
					hasNonConstexprParams = true;
				}
				string param = ConvertToCppLiteral(handlerArray[i]);
				if (!string.IsNullOrEmpty(param)) {
					parameters.Add(param);
				}
			}

			// All handlers have has_modrm as first parameter
			string hasModrm = GetHasModRM(enumValue.RawName);

			// Special handling for certain handler types that reuse parameters
			string kindUpper = enumValue.RawName.ToUpperInvariant();
			if (cppType == "OpCodeHandler_Mf" && parameters.Count == 1) {
				// MF_1 uses the same code for both code16 and code32
				parameters.Add(parameters[0]);
			}
			else if (cppType == "OpCodeHandler_Ev_REXW" && (kindUpper == "EV_REXW_1A" || kindUpper.Contains("_1A"))) {
				// Ev_REXW_1a uses the same code for both 32 and 64-bit
				// Insert duplicate of first param (the Code) before the flags
				if (parameters.Count >= 1) {
					parameters.Insert(1, parameters[0]);
				}
			}
			else if ((cppType == "OpCodeHandler_C_R" || cppType == "OpCodeHandler_R_C") && 
			         (kindUpper.Contains("_3B") || kindUpper.EndsWith("3B"))) {
				// C_R_3b and R_C_3b use the same code for both 32 and 64-bit
				if (parameters.Count >= 1) {
					parameters.Insert(1, parameters[0]);
				}
			}
			else if (cppType == "OpCodeHandler_PushOpSizeReg" && (kindUpper.Contains("_4B") || kindUpper.EndsWith("4B"))) {
				// PushOpSizeReg_4b has 2 codes + register, need to add Code::INVALID for code64
				// Insert Code::INVALID before the register (which is the last parameter)
				if (parameters.Count >= 2) {
					parameters.Insert(2, "Code::INVALID");
				}
			}
			// VEX VHW handlers need parameter expansion based on variant
			else if (cppType == "OpCodeHandler_VEX_VHW") {
				if (kindUpper == "VHW_2") {
					// VHW_2: (reg, code) -> (reg, reg, reg, code, code)
					if (parameters.Count == 2) {
						var reg = parameters[0];
						var code = parameters[1];
						parameters.Clear();
						parameters.AddRange(new[] { reg, reg, reg, code, code });
					}
				}
				else if (kindUpper == "VHW_3") {
					// VHW_3: (reg, codeR, codeM) -> (reg, reg, reg, codeR, codeM)
					if (parameters.Count == 3) {
						var reg = parameters[0];
						var codeR = parameters[1];
						var codeM = parameters[2];
						parameters.Clear();
						parameters.AddRange(new[] { reg, reg, reg, codeR, codeM });
					}
				}
				else if (kindUpper == "VHW_4") {
					// VHW_4: (reg1, reg2, reg3, code) -> (reg1, reg2, reg3, code, code)
					if (parameters.Count == 4) {
						parameters.Add(parameters[3]); // Duplicate code for codeM
					}
				}
			}
			// VEX VHWIb handlers need similar parameter expansion
			else if (cppType == "OpCodeHandler_VEX_VHWIb") {
				if (kindUpper.Contains("_2") || kindUpper == "VHWIB_2") {
					// VHWIB_2: (reg, code) -> (reg, reg, reg, code)
					if (parameters.Count == 2) {
						var reg = parameters[0];
						var code = parameters[1];
						parameters.Clear();
						parameters.AddRange(new[] { reg, reg, reg, code });
					}
				}
				else if (kindUpper.Contains("_4") || kindUpper == "VHWIB_4") {
					// VHWIB_4: (reg1, reg2, reg3, code) - already correct
				}
			}
			// VEX VW handlers need parameter expansion
			else if (cppType == "OpCodeHandler_VEX_VW") {
				if (kindUpper == "VW_2") {
					// VW_2: (reg, code) -> (reg, reg, code)
					if (parameters.Count == 2) {
						var reg = parameters[0];
						var code = parameters[1];
						parameters.Clear();
						parameters.AddRange(new[] { reg, reg, code });
					}
				}
				// VW_3: (reg1, reg2, code) - already correct
			}
			// VEX VWIb handlers need parameter expansion
			else if (cppType == "OpCodeHandler_VEX_VWIb") {
				if (kindUpper == "VWIB_2") {
					// VWIB_2: (reg, code) -> (reg, reg, code, code)
					if (parameters.Count == 2) {
						var reg = parameters[0];
						var code = parameters[1];
						parameters.Clear();
						parameters.AddRange(new[] { reg, reg, code, code });
					}
				}
				else if (kindUpper == "VWIB_3") {
					// VWIB_3: Generator data has (reg, code1, code2) where code2=code1+1
					// Need to expand to (reg, reg, code1, code2) to match C++ struct
					if (parameters.Count == 3) {
						var reg = parameters[0];
						var code1 = parameters[1];
						var code2 = parameters[2];
						parameters.Clear();
						parameters.AddRange(new[] { reg, reg, code1, code2 });
					}
				}
			}
			// VEX WV handler needs parameter expansion
			else if (cppType == "OpCodeHandler_VEX_WV") {
				// (reg, code) -> (reg, reg, code)
				if (parameters.Count == 2) {
					var reg = parameters[0];
					var code = parameters[1];
					parameters.Clear();
					parameters.AddRange(new[] { reg, reg, code });
				}
			}
			// VEX VK_R_Ib, VK_R, and G_VK need Register before Code in C++
			else if (cppType == "OpCodeHandler_VEX_VK_R_Ib" || cppType == "OpCodeHandler_VEX_VK_R" ||
			         cppType == "OpCodeHandler_VEX_G_VK") {
				// C# serializes (code, register) but C++ struct is {has_modrm, register, code}
				// Need to swap order
				if (parameters.Count == 2) {
					var temp = parameters[0];
					parameters[0] = parameters[1];
					parameters[1] = temp;
				}
			}
			// GvM_VX_Ib and similar handlers - check C++ struct layout
			else if (cppType == "OpCodeHandler_GvM_VX_Ib" || cppType == "OpCodeHandler_VEX_GvM_VX_Ib") {
				// C# serializes (register, code32, code64) but C++ struct is {has_modrm, code32, code64}
				// The register is not stored in the struct (handled differently)
				if (parameters.Count == 3) {
					// Remove the register parameter, keep only codes
					parameters.RemoveAt(0);
				}
			}
			// EVEX VkHW handlers need parameter expansion
			else if (cppType == "OpCodeHandler_EVEX_VkHW") {
				if (kindUpper == "VKHW_3" || kindUpper == "VKHW_3B") {
					// VkHW_3/3b: (reg, code, tupleType) -> (reg, reg, reg, code, tupleType, canBroadcast)
					// 3 = no broadcast, 3b = broadcast
					bool canBroadcast = kindUpper.EndsWith("B");
					if (parameters.Count == 3) {
						var reg = parameters[0];
						var code = parameters[1];
						var tupleType = parameters[2];
						parameters.Clear();
						parameters.AddRange(new[] { reg, reg, reg, code, tupleType, canBroadcast ? "true" : "false" });
					}
				}
				else if (kindUpper == "VKHW_5") {
					// VkHW_5: (reg1, reg2, reg3, code, tupleType) -> (reg1, reg2, reg3, code, tupleType, false)
					if (parameters.Count == 5) {
						parameters.Add("false");
					}
				}
			}
			// EVEX VkHWIb handlers need parameter expansion
			else if (cppType == "OpCodeHandler_EVEX_VkHWIb") {
				if (kindUpper == "VKHWIB_3" || kindUpper == "VKHWIB_3B") {
					// VkHWIb_3/3b: (reg, code, tupleType) -> (reg, reg, reg, code, tupleType, canBroadcast)
					bool canBroadcast = kindUpper.EndsWith("B");
					if (parameters.Count == 3) {
						var reg = parameters[0];
						var code = parameters[1];
						var tupleType = parameters[2];
						parameters.Clear();
						parameters.AddRange(new[] { reg, reg, reg, code, tupleType, canBroadcast ? "true" : "false" });
					}
				}
				else if (kindUpper == "VKHWIB_5") {
					// VkHWIb_5: (reg1, reg2, reg3, code, tupleType) -> (reg1, reg2, reg3, code, tupleType, false)
					if (parameters.Count == 5) {
						parameters.Add("false");
					}
				}
			}
			// EVEX VkHWIb_er handlers need parameter expansion
			else if (cppType == "OpCodeHandler_EVEX_VkHWIb_er") {
				if (kindUpper.StartsWith("VKHWIB_ER_4")) {
					// VkHWIb_er_4/4b: (reg, code, tupleType) -> (reg, code, tupleType, canBroadcast)
					bool canBroadcast = kindUpper.EndsWith("B");
					if (parameters.Count == 3) {
						parameters.Add(canBroadcast ? "true" : "false");
					}
				}
			}
			// EVEX VkHW_er handlers - only need to add canBroadcast flag
			else if (cppType == "OpCodeHandler_EVEX_VkHW_er") {
				if (kindUpper.StartsWith("VKHW_ER_4")) {
					// VkHW_er_4/4b: (reg, code, tupleType, onlySAE) -> (reg, code, tupleType, onlySAE, canBroadcast)
					bool canBroadcast = kindUpper.EndsWith("B");
					if (parameters.Count == 4) {
						parameters.Add(canBroadcast ? "true" : "false");
					}
				}
			}
			// EVEX VkW handlers need parameter expansion
			else if (cppType == "OpCodeHandler_EVEX_VkW") {
				bool canBroadcast = kindUpper.EndsWith("B");
				if (kindUpper.StartsWith("VKW_3")) {
					// VkW_3/3b: (reg, code, tupleType) -> (reg, reg, code, tupleType, canBroadcast)
					if (parameters.Count == 3) {
						var reg = parameters[0];
						var code = parameters[1];
						var tupleType = parameters[2];
						parameters.Clear();
						parameters.AddRange(new[] { reg, reg, code, tupleType, canBroadcast ? "true" : "false" });
					}
				}
				else if (kindUpper.StartsWith("VKW_4")) {
					// VkW_4/4b: (reg1, reg2, code, tupleType) -> (reg1, reg2, code, tupleType, canBroadcast)
					if (parameters.Count == 4) {
						parameters.Add(canBroadcast ? "true" : "false");
					}
				}
			}
			// EVEX VkW_er handlers need parameter expansion
			else if (cppType == "OpCodeHandler_EVEX_VkW_er") {
				// VkW_er_4: (reg, code, tupleType, onlySAE) -> (reg, reg, code, tupleType, onlySAE, canBroadcast)
				// VkW_er_5: (reg, reg, code, tupleType, onlySAE) -> (reg, reg, code, tupleType, onlySAE, canBroadcast)
				// canBroadcast is always true for VkW_er
				if (parameters.Count == 4) {
					var reg = parameters[0];
					var code = parameters[1];
					var tupleType = parameters[2];
					var onlySAE = parameters[3];
					parameters.Clear();
					parameters.AddRange(new[] { reg, reg, code, tupleType, onlySAE, "true" });
				}
				else if (parameters.Count == 5) {
					// VkW_er_5 already has reg1, reg2, code, tupleType, onlySAE - just add canBroadcast
					parameters.Add("true");
				}
			}
			// EVEX VHW handlers need parameter expansion
			else if (cppType == "OpCodeHandler_EVEX_VHW") {
				if (kindUpper == "VHW_3") {
					// VHW_3: (reg, code, tupleType) -> (reg, code, code, tupleType)
					if (parameters.Count == 3) {
						var reg = parameters[0];
						var code = parameters[1];
						var tupleType = parameters[2];
						parameters.Clear();
						parameters.AddRange(new[] { reg, code, code, tupleType });
					}
				}
				// VHW_4: (reg, code_r, code_m, tupleType) - already correct
			}
			// EVEX VHWIb handlers need parameter expansion
			else if (cppType == "OpCodeHandler_EVEX_VHWIb") {
				// VHWIb: (reg, code, tupleType) -> (reg, code, tupleType) - struct should match
			}
			// EVEX WkV handlers need parameter expansion
			else if (cppType == "OpCodeHandler_EVEX_WkV") {
				if (kindUpper == "WKV_3") {
					// WkV_3: (reg, code, tupleType) -> (reg, reg, code, tupleType, false)
					if (parameters.Count == 3) {
						var reg = parameters[0];
						var code = parameters[1];
						var tupleType = parameters[2];
						parameters.Clear();
						parameters.AddRange(new[] { reg, reg, code, tupleType, "false" });
					}
				}
				else if (kindUpper == "WKV_4A") {
					// WkV_4a: (reg1, reg2, code, tupleType) -> (reg1, reg2, code, tupleType, false)
					if (parameters.Count == 4) {
						parameters.Add("false");
					}
				}
				else if (kindUpper == "WKV_4B") {
					// WkV_4b: (reg, code, tupleType, allowZeroing) -> (reg, reg, code, tupleType, false)
					// Note: allowZeroing is converted to false for can_broadcast (different semantics)
					if (parameters.Count == 4) {
						var reg = parameters[0];
						var code = parameters[1];
						var tupleType = parameters[2];
						// Ignore allowZeroing, use false for can_broadcast
						parameters.Clear();
						parameters.AddRange(new[] { reg, reg, code, tupleType, "false" });
					}
				}
			}

			string paramsStr = string.Join(", ", new[] { hasModrm }.Concat(parameters));

			// Use inline const for handlers with non-constexpr params (nested handlers, string refs)
			// Use constexpr for simple handlers (faster compile-time evaluation)
			// Note: We can't use constinit because make_handler_entry uses reinterpret_cast which is not constexpr
			string storageClass = hasNonConstexprParams ? "inline const" : "inline constexpr";

			return $"{storageClass} {cppType} {handlerName}{{ {paramsStr} }};";
		}

		string GetCppHandlerType(string kindName) {
			// Map handler kind names to C++ struct names based on table type
			string? result = null;
			switch (tableName) {
				case "legacy":
					result = GetLegacyCppHandlerType(kindName);
					if (result is null) result = GetLegacyCppHandlerType(kindName.ToUpperInvariant());
					break;
				case "vex":
					result = GetVexCppHandlerType(kindName);
					if (result is null) result = GetVexCppHandlerType(kindName.ToUpperInvariant());
					if (result is null) result = GetLegacyCppHandlerType(kindName);
					if (result is null) result = GetLegacyCppHandlerType(kindName.ToUpperInvariant());
					break;
				case "evex":
					result = GetEvexCppHandlerType(kindName);
					if (result is null) result = GetEvexCppHandlerType(kindName.ToUpperInvariant());
					if (result is null) result = GetVexCppHandlerType(kindName);
					if (result is null) result = GetVexCppHandlerType(kindName.ToUpperInvariant());
					if (result is null) result = GetLegacyCppHandlerType(kindName);
					if (result is null) result = GetLegacyCppHandlerType(kindName.ToUpperInvariant());
					break;
				case "xop":
					result = GetXopCppHandlerType(kindName);
					if (result is null) result = GetXopCppHandlerType(kindName.ToUpperInvariant());
					if (result is null) result = GetVexCppHandlerType(kindName);
					if (result is null) result = GetVexCppHandlerType(kindName.ToUpperInvariant());
					if (result is null) result = GetLegacyCppHandlerType(kindName);
					if (result is null) result = GetLegacyCppHandlerType(kindName.ToUpperInvariant());
					break;
				case "mvex":
					result = GetMvexCppHandlerType(kindName);
					if (result is null) result = GetMvexCppHandlerType(kindName.ToUpperInvariant());
					if (result is null) result = GetVexCppHandlerType(kindName);
					if (result is null) result = GetVexCppHandlerType(kindName.ToUpperInvariant());
					if (result is null) result = GetLegacyCppHandlerType(kindName);
					if (result is null) result = GetLegacyCppHandlerType(kindName.ToUpperInvariant());
					break;
			}
			return result ?? "";
		}

		string? GetLegacyCppHandlerType(string kindName) {
			// Map legacy handler kind names to C++ struct names
			return kindName switch {
				"AL_DX" => "OpCodeHandler_AL_DX",
				"ANOTHER_TABLE" => "OpCodeHandler_AnotherTable",
				"AP" => "OpCodeHandler_Ap",
				"B_BM" => "OpCodeHandler_B_BM",
				"B_EV" => "OpCodeHandler_B_Ev",
				"B_MIB" => "OpCodeHandler_B_MIB",
				"BITNESS" => "OpCodeHandler_Bitness",
				"BITNESS_DONT_READ_MOD_RM" => "OpCodeHandler_Bitness_DontReadModRM",
				"BM_B" => "OpCodeHandler_BM_B",
				"BRANCH_IW" => "OpCodeHandler_BranchIw",
				"BRANCH_SIMPLE" => "OpCodeHandler_BranchSimple",
				"C_R_3A" => "OpCodeHandler_C_R",
				"C_R_3B" => "OpCodeHandler_C_R",
				"D3NOW" => "OpCodeHandler_D3NOW",
				"DX_AL" => "OpCodeHandler_DX_AL",
				"DX_E_AX" => "OpCodeHandler_DX_eAX",
				"E_AX_DX" => "OpCodeHandler_eAX_DX",
				"EB_1" => "OpCodeHandler_Eb_1",
				"EB_2" => "OpCodeHandler_Eb",
				"EB_CL" => "OpCodeHandler_Eb_CL",
				"EB_GB_1" => "OpCodeHandler_Eb_Gb",
				"EB_GB_2" => "OpCodeHandler_Eb_Gb",
				"EB_IB_1" => "OpCodeHandler_Eb_Ib",
				"EB_IB_2" => "OpCodeHandler_Eb_Ib",
				"EB1" => "OpCodeHandler_Eb_1",
				"ED_V_IB" => "OpCodeHandler_Ed_V_Ib",
				"EP" => "OpCodeHandler_Ep",
				"EV_3A" => "OpCodeHandler_Ev",
				"EV_3B" => "OpCodeHandler_Ev",
				"EV_4" => "OpCodeHandler_Ev",
				"EV_CL" => "OpCodeHandler_Ev_CL",
				"EV_GV_32_64" => "OpCodeHandler_Ev_Gv_32_64",
				"EV_GV_3A" => "OpCodeHandler_Ev_Gv",
				"EV_GV_3B" => "OpCodeHandler_Ev_Gv",
				"EV_GV_4" => "OpCodeHandler_Ev_Gv_flags",
				"EV_GV_CL" => "OpCodeHandler_Ev_Gv_CL",
				"EV_GV_IB" => "OpCodeHandler_Ev_Gv_Ib",
				"EV_GV_REX" => "OpCodeHandler_Ev_Gv_REX",
				"EV_IB_3" => "OpCodeHandler_Ev_Ib",
				"EV_IB_4" => "OpCodeHandler_Ev_Ib",
				"EV_IB2_3" => "OpCodeHandler_Ev_Ib2",
				"EV_IB2_4" => "OpCodeHandler_Ev_Ib2",
				"EV_IZ_3" => "OpCodeHandler_Ev_Iz",
				"EV_IZ_4" => "OpCodeHandler_Ev_Iz",
				"EV_P" => "OpCodeHandler_Ev_P",
				"EV_REXW" => "OpCodeHandler_Ev_REXW",
				"EV_REXW_1A" => "OpCodeHandler_Ev_REXW",
				"EV_SW" => "OpCodeHandler_Ev_Sw",
				"EV_VX" => "OpCodeHandler_Ev_VX",
				"EV1" => "OpCodeHandler_Ev_1",
				"EVEX" => "OpCodeHandler_EVEX",
				"EVJ" => "OpCodeHandler_Evj",
				"EVW" => "OpCodeHandler_Evw",
				"EW" => "OpCodeHandler_Ew",
				"GB_EB" => "OpCodeHandler_Gb_Eb",
				"GDQ_EV" => "OpCodeHandler_Gdq_Ev",
				"GD_RD" => "OpCodeHandler_Gd_Rd",
				"GROUP" => "OpCodeHandler_Group",
				"GROUP8X64" => "OpCodeHandler_Group8x64",
				"GROUP8X8" => "OpCodeHandler_Group8x8",
				"GV_EB" => "OpCodeHandler_Gv_Eb",
				"GV_EB_REX" => "OpCodeHandler_Gv_Eb_REX",
				"GV_EV_32_64" => "OpCodeHandler_Gv_Ev_32_64",
				"GV_EV_3A" => "OpCodeHandler_Gv_Ev",
				"GV_EV_3B" => "OpCodeHandler_Gv_Ev",
				"GV_EV_IB" => "OpCodeHandler_Gv_Ev_Ib",
				"GV_EV_IB_REX" => "OpCodeHandler_Gv_Ev_Ib_REX",
				"GV_EV_IZ" => "OpCodeHandler_Gv_Ev_Iz",
				"GV_EV_REX" => "OpCodeHandler_Gv_Ev_REX",
				"GV_EV2" => "OpCodeHandler_Gv_Ev2",
				"GV_EV3" => "OpCodeHandler_Gv_Ev3",
				"GV_EW" => "OpCodeHandler_Gv_Ew",
				"GV_M" => "OpCodeHandler_Gv_M",
				"GV_M_AS" => "OpCodeHandler_Gv_M_as",
				"GV_MA" => "OpCodeHandler_Gv_Ma",
				"GV_MP_2" => "OpCodeHandler_Gv_Mp",
				"GV_MP_3" => "OpCodeHandler_Gv_Mp",
				"GV_MV" => "OpCodeHandler_Gv_Mv",
				"GV_M_VX_IB" => "OpCodeHandler_GvM_VX_Ib",
				"GV_N" => "OpCodeHandler_Gv_N",
				"GV_N_IB_REX" => "OpCodeHandler_Gv_N_Ib_REX",
				"GV_RX" => "OpCodeHandler_Gv_RX",
				"GV_W" => "OpCodeHandler_Gv_W",
				"IB" => "OpCodeHandler_Ib",
				"IB3" => "OpCodeHandler_Ib3",
				"IB_REG" => "OpCodeHandler_IbReg",
				"IB_REG2" => "OpCodeHandler_IbReg2",
				"IW_IB" => "OpCodeHandler_Iw_Ib",
				"JB" => "OpCodeHandler_Jb",
				"JB2" => "OpCodeHandler_Jb2",
				"JDISP" => "OpCodeHandler_Jdisp",
				"JX" => "OpCodeHandler_Jx",
				"JZ" => "OpCodeHandler_Jz",
				"M_1" => "OpCodeHandler_M",
				"M_2" => "OpCodeHandler_M",
				"M_REXW_2" => "OpCodeHandler_M_REXW",
				"M_REXW_4" => "OpCodeHandler_M_REXW",
				"MEM_BX" => "OpCodeHandler_MemBx",
				"MF_1" => "OpCodeHandler_Mf",
				"MF_2A" => "OpCodeHandler_Mf",
				"MF_2B" => "OpCodeHandler_Mf",
				"MIB_B" => "OpCodeHandler_MIB_B",
				"MP" => "OpCodeHandler_MP",
				"MS" => "OpCodeHandler_Ms",
				"MV" => "OpCodeHandler_MV",
				"MV_GV" => "OpCodeHandler_Mv_Gv",
				"MV_GV_REXW" => "OpCodeHandler_Mv_Gv_REXW",
				"M_SW" => "OpCodeHandler_M_Sw",
				"NIb" => "OpCodeHandler_NIb",
				"OB_REG" => "OpCodeHandler_Ob_Reg",
				"OPTIONS1632_1" => "OpCodeHandler_Options1632",
				"OPTIONS1632_2" => "OpCodeHandler_Options1632",
				"OPTIONS3" => "OpCodeHandler_Options",
				"OPTIONS5" => "OpCodeHandler_Options",
				"OPTIONS_DONT_READ_MOD_RM" => "OpCodeHandler_Options_DontReadModRM",
				"OV_REG" => "OpCodeHandler_Ov_Reg",
				"P_EV" => "OpCodeHandler_P_Ev",
				"P_EV_IB" => "OpCodeHandler_P_Ev_Ib",
				"P_Q" => "OpCodeHandler_P_Q",
				"P_Q_IB" => "OpCodeHandler_P_Q_Ib",
				"P_R" => "OpCodeHandler_P_R",
				"P_W" => "OpCodeHandler_P_W",
				"PREFIX_ES_CS_SS_DS" => "OpCodeHandler_PrefixEsCsSsDs",
				"PREFIX_FS_GS" => "OpCodeHandler_PrefixFsGs",
				"PREFIX66" => "OpCodeHandler_Prefix66",
				"PREFIX67" => "OpCodeHandler_Prefix67",
				"PREFIX_F0" => "OpCodeHandler_PrefixF0",
				"PREFIX_F2" => "OpCodeHandler_PrefixF2",
				"PREFIX_F3" => "OpCodeHandler_PrefixF3",
				"PREFIX_REX" => "OpCodeHandler_PrefixREX",
				"PUSH_EV" => "OpCodeHandler_PushEv",
				"PUSH_IB2" => "OpCodeHandler_PushIb2",
				"PUSH_IZ" => "OpCodeHandler_PushIz",
				"PUSH_OP_SIZE_REG_4A" => "OpCodeHandler_PushOpSizeReg",
				"PUSH_OP_SIZE_REG_4B" => "OpCodeHandler_PushOpSizeReg",
				"PUSH_SIMPLE2" => "OpCodeHandler_PushSimple2",
				"PUSH_SIMPLE_REG" => "OpCodeHandler_PushSimpleReg",
				"Q_P" => "OpCodeHandler_Q_P",
				"R_C_3A" => "OpCodeHandler_R_C",
				"R_C_3B" => "OpCodeHandler_R_C",
				"R_DI_P_N" => "OpCodeHandler_rDI_P_N",
				"R_DI_VX_RX" => "OpCodeHandler_rDI_VX_RX",
				"REG" => "OpCodeHandler_Reg",
				"REG_IB" => "OpCodeHandler_RegIb",
				"REG_IB2" => "OpCodeHandler_Reg_Ib2",
				"REG_IB3" => "OpCodeHandler_RegIb3",
				"REG_IZ" => "OpCodeHandler_Reg_Iz",
				"REG_IZ2" => "OpCodeHandler_RegIz2",
				"REG_OB" => "OpCodeHandler_Reg_Ob",
				"REG_OV" => "OpCodeHandler_Reg_Ov",
				"REG_XB" => "OpCodeHandler_Reg_Xb",
				"REG_XV" => "OpCodeHandler_Reg_Xv",
				"REG_XV2" => "OpCodeHandler_Reg_Xv2",
				"REG_YB" => "OpCodeHandler_Reg_Yb",
				"REG_YV" => "OpCodeHandler_Reg_Yv",
				"RESERVEDNOP" => "OpCodeHandler_Reservednop",
				"RIB" => "OpCodeHandler_RIb",
				"RIB_IB" => "OpCodeHandler_RIbIb",
				"RM" => "OpCodeHandler_RM",
				"RQ" => "OpCodeHandler_Rq",
				"RV" => "OpCodeHandler_Rv",
				"RV_32_64" => "OpCodeHandler_Rv_32_64",
				"RV_MW_GW" => "OpCodeHandler_RvMw_Gw",
				"SIMPLE" => "OpCodeHandler_Simple",
				"SIMPLE2_3A" => "OpCodeHandler_Simple2",
				"SIMPLE2_3B" => "OpCodeHandler_Simple2",
				"SIMPLE2_IW" => "OpCodeHandler_Simple2Iw",
				"SIMPLE3" => "OpCodeHandler_Simple3",
				"SIMPLE4" => "OpCodeHandler_Simple4",
				"SIMPLE4B" => "OpCodeHandler_Simple4",
				"SIMPLE5" => "OpCodeHandler_Simple5",
				"SIMPLE5_A32" => "OpCodeHandler_Simple5_a32",
				"SIMPLE5_MOD_RM_AS" => "OpCodeHandler_Simple5_ModRM_as",
				"SIMPLE_REG" => "OpCodeHandler_SimpleReg",
				"ST_STI" => "OpCodeHandler_ST_STi",
				"STI" => "OpCodeHandler_STi",
				"STI_ST" => "OpCodeHandler_STi_ST",
				"SW_EV" => "OpCodeHandler_Sw_Ev",
				"SW_M" => "OpCodeHandler_Sw_M",
				"V_EV" => "OpCodeHandler_V_Ev",
				"VEX2" => "OpCodeHandler_VEX2",
				"VEX3" => "OpCodeHandler_VEX3",
				"VM" => "OpCodeHandler_VM",
				"VN" => "OpCodeHandler_VN",
				"VQ" => "OpCodeHandler_VQ",
				"VRIbIb" => "OpCodeHandler_VRIbIb",
				"VW" => "OpCodeHandler_VW",
				"VWIb" => "OpCodeHandler_VWIb",
				"VX_E_Ib" => "OpCodeHandler_VX_E_Ib",
				"VX_Ev" => "OpCodeHandler_VX_Ev",
				"V_Ev" => "OpCodeHandler_V_Ev",
				"WV" => "OpCodeHandler_WV",
				"Wbinvd" => "OpCodeHandler_Wbinvd",
				"XOP" => "OpCodeHandler_XOP",
				"Xb_Yb" => "OpCodeHandler_Xb_Yb",
				"XCHG_REG_R_AX" => "OpCodeHandler_Xchg_Reg_rAX",
				"Xv_Yv" => "OpCodeHandler_Xv_Yv",
				"Yb_Reg" => "OpCodeHandler_Yb_Reg",
				"Yb_Xb" => "OpCodeHandler_Yb_Xb",
				"Yv_Reg" => "OpCodeHandler_Yv_Reg",
				"Yv_Reg2" => "OpCodeHandler_Yv_Reg2",
				"Yv_Xv" => "OpCodeHandler_Yv_Xv",
				"eAX_DX" => "OpCodeHandler_eAX_DX",
				"rDI_P_N" => "OpCodeHandler_rDI_P_N",
				"rDI_VX_RX" => "OpCodeHandler_rDI_VX_RX",
				"INVALID" => "OpCodeHandler_Invalid",
				"INVALID_NO_MOD_RM" or "Invalid_NoModRM" => "OpCodeHandler_Invalid",
				"INVALID2" => "OpCodeHandler_Invalid",
				"DUP" => "", // Special case - not a direct handler type
				"NULL_" => "", // Special case - not a direct handler type
				"HANDLER_REFERENCE" => "", // Special case - not a direct handler type
				"ARRAY_REFERENCE" => null, // Special case - not a direct handler type
				"MANDATORY_PREFIX" => "OpCodeHandler_MandatoryPrefix",
				"MANDATORY_PREFIX_NO_MOD_RM" => "OpCodeHandler_MandatoryPrefix",
				"MandatoryPrefix" => "OpCodeHandler_MandatoryPrefix",
				"MandatoryPrefix_NoModRM" => "OpCodeHandler_MandatoryPrefix",
				"MANDATORY_PREFIX3" => "OpCodeHandler_MandatoryPrefix3",
				"MANDATORY_PREFIX4" => "OpCodeHandler_MandatoryPrefix4",
				"MandatoryPrefix3" => "OpCodeHandler_MandatoryPrefix3",
				"MandatoryPrefix4" => "OpCodeHandler_MandatoryPrefix4",
				"PushEv" => "OpCodeHandler_PushEv",
				"PushOpSizeReg_4a" or "PushOpSizeReg_4b" => "OpCodeHandler_PushOpSizeReg",
				"PushSimpleReg" => "OpCodeHandler_PushSimpleReg",
				"PushIz" => "OpCodeHandler_PushIz",
				"PushIb2" => "OpCodeHandler_PushIb2",
				"PushSimple2" => "OpCodeHandler_PushSimple2",
				"SimpleReg" => "OpCodeHandler_SimpleReg",
				"Simple_ModRM" => "OpCodeHandler_Simple",
				"Simple5_ModRM_as" => "OpCodeHandler_Simple5_ModRM_as",
				"RegIb" => "OpCodeHandler_RegIb",
				"RegIb3" => "OpCodeHandler_RegIb3",
				"RegIz2" => "OpCodeHandler_RegIz2",
				"AnotherTable" => "OpCodeHandler_AnotherTable",
				"PrefixEsCsSsDs" => "OpCodeHandler_PrefixEsCsSsDs",
				"PrefixFsGs" => "OpCodeHandler_PrefixFsGs",
				"PrefixREX" => "OpCodeHandler_PrefixREX",
				"PrefixF0" => "OpCodeHandler_PrefixF0",
				"PrefixF2" => "OpCodeHandler_PrefixF2",
				"PrefixF3" => "OpCodeHandler_PrefixF3",
				"Xchg_Reg_rAX" => "OpCodeHandler_Xchg_Reg_rAX",
				"BranchIw" => "OpCodeHandler_BranchIw",
				"BranchSimple" => "OpCodeHandler_BranchSimple",
				"Simple2Iw" => "OpCodeHandler_Simple2Iw",
				"MemBx" => "OpCodeHandler_MemBx",
				"IbReg" => "OpCodeHandler_IbReg",
				"IbReg2" => "OpCodeHandler_IbReg2",
				"DX_eAX" => "OpCodeHandler_DX_eAX",
				"Options_DontReadModRM" => "OpCodeHandler_Options_DontReadModRM",
				"RIbIb" => "OpCodeHandler_RIbIb",
				"RvMw_Gw" => "OpCodeHandler_RvMw_Gw",
				"Bitness_DontReadModRM" => "OpCodeHandler_Bitness_DontReadModRM",
				"GvM_VX_Ib" => "OpCodeHandler_GvM_VX_Ib",
				"VW_2" or "VW_3" => "OpCodeHandler_VW",
				"VWIb_2" or "VWIb_3" => "OpCodeHandler_VWIb",
				_ => null // Unknown/unsupported
			};
		}

		string? GetVexCppHandlerType(string kindName) {
			// Map VEX handler kind names to C++ struct names
			return kindName switch {
				"BITNESS" => "OpCodeHandler_Bitness",
				"BITNESS_DONT_READ_MOD_RM" or "Bitness_DontReadModRM" => "OpCodeHandler_Bitness_DontReadModRM",
				"INVALID" => "OpCodeHandler_Invalid",
				"INVALID2" => "OpCodeHandler_Invalid",
				"INVALID_NO_MOD_RM" or "Invalid_NoModRM" => "OpCodeHandler_Invalid",
				"DUP" => null, // Special case - not a direct handler type
				"NULL_" => null, // Special case - not a direct handler type
				"HANDLER_REFERENCE" => null, // Special case - not a direct handler type
				"ARRAY_REFERENCE" => null, // Special case - not a direct handler type
				"RM" => "OpCodeHandler_RM",
				"GROUP" => "OpCodeHandler_Group",
				"GROUP8X64" => "OpCodeHandler_Group8x64",
				"W" => "OpCodeHandler_VEX_W",
				"MANDATORY_PREFIX2" => "OpCodeHandler_VEX_MandatoryPrefix2",
				"MANDATORY_PREFIX2_1" => "OpCodeHandler_VEX_MandatoryPrefix2",
				"MANDATORY_PREFIX2_4" => "OpCodeHandler_VEX_MandatoryPrefix2",
				"MANDATORY_PREFIX2_NO_MOD_RM" => "OpCodeHandler_VEX_MandatoryPrefix2",
				"MandatoryPrefix2" => "OpCodeHandler_VEX_MandatoryPrefix2",
				"MandatoryPrefix2_1" => "OpCodeHandler_VEX_MandatoryPrefix2",
				"MandatoryPrefix2_4" => "OpCodeHandler_VEX_MandatoryPrefix2",
				"MandatoryPrefix2_NoModRM" => "OpCodeHandler_VEX_MandatoryPrefix2",
				"VECTOR_LENGTH_NO_MOD_RM" or "VectorLength_NoModRM" => "OpCodeHandler_VEX_VectorLength_NoModRM",
				"VECTOR_LENGTH" or "VectorLength" => "OpCodeHandler_VEX_VectorLength",
				"OPTIONS_DONT_READ_MOD_RM" => "OpCodeHandler_Options_DontReadModRM",
				"SIMPLE" => "OpCodeHandler_Simple",
				"VHW_2" or "VHW_3" or "VHW_4" => "OpCodeHandler_VEX_VHW",
				"VHWIB_2" or "VHWIB_4" => "OpCodeHandler_VEX_VHWIb",
				"VW_2" or "VW_3" => "OpCodeHandler_VEX_VW",
				"VWIB_2" or "VWIB_3" => "OpCodeHandler_VEX_VWIb",
				"WV" => "OpCodeHandler_VEX_WV",
				"WVIB" => "OpCodeHandler_VEX_WVIb",
				"VM" => "OpCodeHandler_VEX_VM",
				"MV" => "OpCodeHandler_VEX_MV",
				"M" => "OpCodeHandler_VEX_M",
				"VHM" => "OpCodeHandler_VEX_VHM",
				"MHV" => "OpCodeHandler_VEX_MHV",
				"VWH" => "OpCodeHandler_VEX_VWH",
				"WHV" => "OpCodeHandler_VEX_WHV",
				"VHEV" => "OpCodeHandler_VEX_VHEv",
				"VHEV_IB" => "OpCodeHandler_VEX_VHEvIb",
				"EV_VX" => "OpCodeHandler_VEX_Ev_VX",
				"VX_EV" => "OpCodeHandler_VEX_VX_Ev",
				"ED_V_IB" => "OpCodeHandler_VEX_Ed_V_Ib",
				"GV_M_VX_IB" => "OpCodeHandler_VEX_GvM_VX_Ib",
				"GV_EV" => "OpCodeHandler_VEX_Gv_Ev",
				"EV" => "OpCodeHandler_VEX_Ev",
				"GV_EV_GV" => "OpCodeHandler_VEX_Gv_Ev_Gv",
				"EV_GV_GV" => "OpCodeHandler_VEX_Ev_Gv_Gv",
				"GV_GV_EV" => "OpCodeHandler_VEX_Gv_Gv_Ev",
				"GV_EV_IB" => "OpCodeHandler_VEX_Gv_Ev_Ib",
				"GV_EV_ID" => "OpCodeHandler_VEX_Gv_Ev_Id",
				"GV_GPR_IB" => "OpCodeHandler_VEX_Gv_GPR_Ib",
				"GV_RX" => "OpCodeHandler_VEX_Gv_RX",
				"GV_W" => "OpCodeHandler_VEX_Gv_W",
				"HV_EV" => "OpCodeHandler_VEX_Hv_Ev",
				"HV_ED_ID" => "OpCodeHandler_VEX_Hv_Ed_Id",
				"HRIB" => "OpCodeHandler_VEX_HRIb",
				"R_DI_VX_RX" or "rDI_VX_RX" => "OpCodeHandler_VEX_rDI_VX_RX",
				"RD_RQ" => "OpCodeHandler_VEX_RdRq",
				"VHWIS4" => "OpCodeHandler_VEX_VHWIs4",
				"VHIS4_W" or "VHIs4W" => "OpCodeHandler_VEX_VHIs4W",
				"VHWIS5" => "OpCodeHandler_VEX_VHWIs5",
				"VHIS5_W" or "VHIs5W" => "OpCodeHandler_VEX_VHIs5W",
				"VHEvIb" => "OpCodeHandler_VEX_VHEvIb",
				"VK_HK_RK" => "OpCodeHandler_VEX_VK_HK_RK",
				"VK_RK" => "OpCodeHandler_VEX_VK_RK",
				"VK_RK_IB" => "OpCodeHandler_VEX_VK_RK_Ib",
				"VK_WK" => "OpCodeHandler_VEX_VK_WK",
				"VK_R" => "OpCodeHandler_VEX_VK_R",
				"VK_R_IB" => "OpCodeHandler_VEX_VK_R_Ib",
				"G_VK" => "OpCodeHandler_VEX_G_VK",
				"M_VK" => "OpCodeHandler_VEX_M_VK",
				"GQ_HK_RK" => "OpCodeHandler_VEX_Gq_HK_RK",
				"VX_VSIB_HX" => "OpCodeHandler_VEX_VX_VSIB_HX",
				"VT_SIBMEM" => "OpCodeHandler_VEX_VT_SIBMEM",
				"SIBMEM_VT" => "OpCodeHandler_VEX_SIBMEM_VT",
				"VT" => "OpCodeHandler_VEX_VT",
				"VT_RT_HT" => "OpCodeHandler_VEX_VT_RT_HT",
				"K_JB" => "OpCodeHandler_VEX_K_Jb",
				"K_JZ" => "OpCodeHandler_VEX_K_Jz",
				_ => null // Unknown/unsupported
			};
		}

		string? GetEvexCppHandlerType(string kindName) {
			// EVEX handlers - similar to VEX but with EVEX_ prefix
			return kindName switch {
				"MANDATORY_PREFIX2" or "MandatoryPrefix2" => "OpCodeHandler_EVEX_MandatoryPrefix2",
				"RM" => "OpCodeHandler_EVEX_RM",
				"GROUP" => "OpCodeHandler_EVEX_Group",
				"W" => "OpCodeHandler_EVEX_W",
				"VECTOR_LENGTH" or "VectorLength" => "OpCodeHandler_EVEX_VectorLength",
				"VECTOR_LENGTH_ER" or "VectorLength_er" => "OpCodeHandler_EVEX_VectorLength_er",
				// VkHW variants
				"VkHW_3" or "VkHW_3b" or "VkHW_5" => "OpCodeHandler_EVEX_VkHW",
				"VkHW_er_4" or "VkHW_er_4b" => "OpCodeHandler_EVEX_VkHW_er",
				"VkHW_er_ur_3" or "VkHW_er_ur_3b" => "OpCodeHandler_EVEX_VkHW_er_ur",
				"VkHWIb_3" or "VkHWIb_3b" or "VkHWIb_5" => "OpCodeHandler_EVEX_VkHWIb",
				"VkHWIb_er_4" or "VkHWIb_er_4b" => "OpCodeHandler_EVEX_VkHWIb_er",
				// VkW variants
				"VkW_3" or "VkW_3b" or "VkW_4" or "VkW_4b" => "OpCodeHandler_EVEX_VkW",
				"VkW_er_4" or "VkW_er_5" or "VkW_er_6" => "OpCodeHandler_EVEX_VkW_er",
				"VkWIb_3" or "VkWIb_3b" => "OpCodeHandler_EVEX_VkWIb",
				"VkWIb_er" => "OpCodeHandler_EVEX_VkWIb_er",
				"VkM" or "VkHM" => "OpCodeHandler_EVEX_VkM",
				"VkEv_REXW_2" or "VkEv_REXW_3" => "OpCodeHandler_EVEX_VkEv_REXW",
				// WkV variants
				"WkV_3" or "WkV_4a" or "WkV_4b" => "OpCodeHandler_EVEX_WkV",
				"WkHV" => "OpCodeHandler_EVEX_WkHV",
				"WkVIb" => "OpCodeHandler_EVEX_WkVIb",
				"WkVIb_er" => "OpCodeHandler_EVEX_WkVIb_er",
				// Kk variants
				"KkHW_3" or "KkHW_3b" => "OpCodeHandler_EVEX_KkHW",
				"KkHWIb_3" or "KkHWIb_3b" => "OpCodeHandler_EVEX_KkHWIb",
				"KkHWIb_sae_3" or "KkHWIb_sae_3b" => "OpCodeHandler_EVEX_KkHWIb_sae",
				"KkWIb_3" or "KkWIb_3b" => "OpCodeHandler_EVEX_KkWIb",
				"KR" => "OpCodeHandler_EVEX_KR",
				"KP1HW" => "OpCodeHandler_EVEX_KP1HW",
				// Hk variants
				"HkWIb_3" or "HkWIb_3b" => "OpCodeHandler_EVEX_HkWIb",
				"HWIb" => "OpCodeHandler_EVEX_HWIb",
				// V variants
				"VK" => "OpCodeHandler_EVEX_VK",
				"VHWIb" => "OpCodeHandler_EVEX_VHWIb",
				"VW_er" => "OpCodeHandler_EVEX_VW_er",
				"V_H_Ev_Ib" => "OpCodeHandler_EVEX_V_H_Ev_Ib",
				"V_H_Ev_er" => "OpCodeHandler_EVEX_V_H_Ev_er",
				"Ev_VX_Ib" => "OpCodeHandler_EVEX_Ev_VX_Ib",
				// Gv variants
				"Gv_W_er" => "OpCodeHandler_EVEX_Gv_W_er",
				// VSIB variants
				"VSIB_k1" => "OpCodeHandler_EVEX_VSIB_k1",
				"VSIB_k1_VX" => "OpCodeHandler_EVEX_VSIB_k1_VX",
				"Vk_VSIB" => "OpCodeHandler_EVEX_Vk_VSIB",
				// Memory variants
				"VM" => "OpCodeHandler_EVEX_VM",
				"MV" => "OpCodeHandler_EVEX_MV",
				// Additional EVEX-specific handlers that differ from VEX
				"VW" => "OpCodeHandler_EVEX_VW",
				"WV" => "OpCodeHandler_EVEX_WV",
				"VHM" => "OpCodeHandler_EVEX_VHM",
				"VX_EV" or "VX_Ev" => "OpCodeHandler_EVEX_VX_Ev",
				"EV_VX" or "Ev_VX" => "OpCodeHandler_EVEX_Ev_VX",
				"ED_V_IB" or "Ed_V_Ib" => "OpCodeHandler_EVEX_Ed_V_Ib",
				"GV_M_VX_IB" or "GvM_VX_Ib" => "OpCodeHandler_EVEX_GvM_VX_Ib",
				// VHW variants for EVEX
				"VHW_3" or "VHW_4" => "OpCodeHandler_EVEX_VHW",
				_ => null // No fallback - handled in main method
			};
		}

		string? GetXopCppHandlerType(string kindName) {
			// XOP handlers - most are similar to VEX
			return kindName switch {
				"MANDATORY_PREFIX2_1" or "MandatoryPrefix2_1" => "OpCodeHandler_VEX_MandatoryPrefix2",
				"VECTOR_LENGTH" or "VectorLength" => "OpCodeHandler_VEX_VectorLength",
				"RdRq" => "OpCodeHandler_VEX_RdRq",
				_ => null // No fallback - handled in main method
			};
		}

		string? GetMvexCppHandlerType(string kindName) {
			// MVEX handlers - similar to VEX but may have different implementations
			return kindName switch {
				"MANDATORY_PREFIX2" or "MandatoryPrefix2" => "OpCodeHandler_VEX_MandatoryPrefix2", // MVEX uses VEX-style
				"VHW" => "OpCodeHandler_MVEX_VHW",
				"VHWIb" => "OpCodeHandler_MVEX_VHWIb",
				"HWIb" => "OpCodeHandler_MVEX_HWIb",
				"VW" => "OpCodeHandler_MVEX_VW",
				"VWIb" => "OpCodeHandler_MVEX_VWIb",
				"MV" => "OpCodeHandler_MVEX_MV",
				"VKW" => "OpCodeHandler_MVEX_VKW",
				"KHW" => "OpCodeHandler_MVEX_KHW",
				"KHWIb" => "OpCodeHandler_MVEX_KHWIb",
				"VSIB" => "OpCodeHandler_MVEX_VSIB",
				"VSIB_V" => "OpCodeHandler_MVEX_VSIB_V",
				"V_VSIB" => "OpCodeHandler_MVEX_V_VSIB",
				"M" => "OpCodeHandler_MVEX_M",
				"EH" => "OpCodeHandler_MVEX_EH",
				_ => null // No fallback - handled in main method
			};
		}

		string GetHasModRM(string kindName) {
			// Most handlers have modrm, but some don't
			return kindName switch {
				"BITNESS_DONT_READ_MOD_RM" or "OPTIONS_DONT_READ_MOD_RM" or "INVALID_NO_MOD_RM" or
				"VECTOR_LENGTH_NO_MOD_RM" or "MANDATORY_PREFIX2_NO_MOD_RM" or
				"SIMPLE" or "SIMPLE_MOD_RM" or "SIMPLE2_3A" or "SIMPLE2_3B" or "SIMPLE2_IW" or
				"SIMPLE3" or "SIMPLE4" or "SIMPLE4B" or "SIMPLE5" or "SIMPLE5_A32" or
				"SIMPLE5_MOD_RM_AS" or "SIMPLE_REG" or "MEM_BX" or "AP" or "JB" or "JB2" or
				"JDISP" or "JX" or "JZ" or "BRANCH_IW" or "BRANCH_SIMPLE" or "IW_IB" or
				"OB_REG" or "OV_REG" or "REG_OB" or "REG_OV" or "REG_XB" or "REG_XV" or
				"REG_XV2" or "REG_YB" or "REG_YV" or "XB_YB" or "XV_YV" or "YB_REG" or
				"YB_XB" or "YV_REG" or "YV_REG2" or "YV_XV" or "AL_DX" or "DX_AL" or
				"DX_E_AX" or "E_AX_DX" or "PREFIX_ES_CS_SS_DS" or "PREFIX_FS_GS" or
				"PREFIX66" or "PREFIX67" or "PREFIX_F0" or "PREFIX_F2" or "PREFIX_F3" or
				"PREFIX_REX" or "WBINVD" or "K_JB" or "K_JZ" => "false",
				_ => "true"
			};
		}

		string ConvertToCppLiteral(object value) {
			return value switch {
				null => "get_null_handler()",
				OrEnumValue orValue => ConvertOrEnumToCpp(orValue),
				EnumValue enumValue => ConvertEnumToCpp(enumValue),
				Code code => $"Code::{idConverter.EnumField(code.ToString())}",
				Register register => $"Register::{idConverter.EnumField(register.ToString())}",
				RepPrefixKind rep => $"RepPrefixKind::{idConverter.EnumField(rep.ToString())}",
				int intValue => intValue.ToString(),
				uint uintValue => $"{uintValue}U",
				bool boolValue => boolValue.ToString().ToLower(),
				string strValue => ConvertStringReference(strValue),
				object[] nestedHandler => ConvertNestedHandler(nestedHandler),
				_ => $"/* Unknown type: {value?.GetType()?.Name ?? "null"} */"
			};
		}

		string ConvertStringReference(string str) {
			// String values are typically table name references
			// These reference other handler tables by name
			// For constexpr generation, we use null_handler_entry() as a placeholder
			// The runtime code would need to resolve these properly
			return "null_handler_entry()";
		}

		string ConvertOrEnumToCpp(OrEnumValue orValue) {
			// For flag enums (like HandlerFlags), output the combined numeric value
			// The C++ handler structs use uint32_t for flags
			return $"0x{orValue.Value:X}U";
		}

		string ConvertNestedHandler(object[] handlerArray) {
			if (handlerArray.Length == 0)
				return "get_null_handler()";

			// First, ensure this nested handler is generated
			GenerateHandlerRecursive(handlerArray);

			// Return a reference to the generated handler
			if (generatedHandlers.TryGetValue(handlerArray, out string? handlerName)) {
				return $"make_handler_entry(&{handlerName})";
			}

			return "get_null_handler() /* failed to generate nested handler */";
		}

		string ConvertEnumToCpp(EnumValue enumValue) {
			var enumType = enumValue.DeclaringType;
			if (enumType is null)
				return idConverter.EnumField(enumValue.RawName);

			// Map enum type names to C++ enum names
			string enumName = enumType.RawName switch {
				"DecoderOptions" => "DecoderOptions",
				"HandlerFlags" => "", // Skip flags for now
				"LegacyHandlerFlags" => "", // Skip flags for now
				"Code" => "Code",
				"Register" => "Register",
				"RepPrefixKind" => "RepPrefixKind",
				_ => "" // Skip unknown enums
			};

			if (!string.IsNullOrEmpty(enumName)) {
				// Use the identifier converter to get the proper C++ enum field name (SCREAMING_SNAKE_CASE)
				return $"{enumName}::{idConverter.EnumField(enumValue.RawName)}";
			}

			// For raw values or unknown enums, just use the value
			return enumValue.Value.ToString();
		}

		void GenerateConstexprTables() {
			tableDeclarations.Clear();

			foreach (var tableInfo in info.TablesToSerialize) {
				string tableName = GetTableName(tableInfo);
				var entries = new List<string>();

				foreach (var handler in tableInfo.handlers) {
					if (handler is null) {
						entries.Add("get_null_handler()");
					} else if (generatedHandlers.TryGetValue(handler, out string? handlerName)) {
						// Check for special marker handlers
						if (handlerName.StartsWith("@@")) {
							if (handlerName == "@@null@@") {
								entries.Add("get_null_handler()");
							} else {
								// Unknown special handler - use null as fallback
								entries.Add($"get_null_handler() /* {handlerName} */");
							}
						} else {
							entries.Add($"make_handler_entry(&constexpr_handlers::{handlerName})");
						}
					} else {
						entries.Add("get_null_handler() /* missing handler */");
					}
				}

			string entriesStr = string.Join(",\n    ", entries);
			// Use 'inline const' instead of 'inline constexpr' because make_handler_entry()
			// uses reinterpret_cast which is not allowed in constant expressions
			tableDeclarations.AppendLine($"inline const std::array<HandlerEntry, {tableInfo.handlers.Length}> {tableName} = {{");
			tableDeclarations.AppendLine($"    {entriesStr}");
			tableDeclarations.AppendLine("};");
				tableDeclarations.AppendLine();
			}
		}

		string GetTableName((string name, object?[] handlers) tableInfo) {
			return $"{TableName}_{tableInfo.name.Replace(" ", "_").ToLower()}";
		}
	}
}