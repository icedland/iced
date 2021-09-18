// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Enums {
	enum MvexConvDecorator {
		// 000b = no decorator needed
		None,
		// 001b = {1to16}
		Broadcast_1to16,
		// 001b = {1to8}
		Broadcast_1to8,
		// 010b = {4to16}
		Broadcast_4to16,
		// 010b = {4to8}
		Broadcast_4to8,
		// 011b = {float16}
		Float16,
		// 100b = {uint8}
		Uint8,
		// 101b = {sint8}
		Sint8,
		// 110b = {uint16}
		Uint16,
		// 111b = {sint16}
		Sint16,
	}
}
