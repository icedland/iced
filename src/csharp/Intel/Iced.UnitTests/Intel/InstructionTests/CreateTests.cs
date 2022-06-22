// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if ENCODER
using System;
using System.Collections.Generic;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.InstructionTests {
	public sealed class CreateTests {
		[Fact]
		void EncoderIgnoresPrefixesIfDeclareData() {
			Instruction instruction;

			instruction = Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08);
			Verify(ref instruction);

			instruction = Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xAA08);
			Verify(ref instruction);

			instruction = Instruction.CreateDeclareDword(0x77A9CE9D, 0x5505426C, 0x8632FE4F, 0x3427AA08);
			Verify(ref instruction);

			instruction = Instruction.CreateDeclareQword(0x77A9CE9D5505426C, 0x8632FE4F3427AA08);
			Verify(ref instruction);

			void Verify(ref Instruction instruction) {
				var origData = GetData(instruction);
				instruction.HasLockPrefix = true;
				instruction.HasRepePrefix = true;
				instruction.HasRepnePrefix = true;
				instruction.SegmentPrefix = Register.GS;
				instruction.HasXreleasePrefix = true;
				instruction.HasXacquirePrefix = true;
				instruction.SuppressAllExceptions = true;
				instruction.ZeroingMasking = true;
				foreach (var bitness in new int[] { 16, 32, 64 }) {
					var writer = new CodeWriterImpl();
					var encoder = Encoder.Create(bitness, writer);
					bool result = encoder.TryEncode(instruction, 0, out _, out var errorMessage);
					Assert.Null(errorMessage);
					Assert.True(result);
					Assert.Equal(origData, writer.ToArray());
				}
			}
		}

		static byte[] GetData(in Instruction instruction) {
			int length = instruction.DeclareDataCount;
			switch (instruction.Code) {
			case Code.DeclareByte:
				break;
			case Code.DeclareWord:
				length *= 2;
				break;
			case Code.DeclareDword:
				length *= 4;
				break;
			case Code.DeclareQword:
				length *= 8;
				break;
			default:
				throw new InvalidOperationException();
			}
			var res = new byte[length];
			for (int i = 0; i < res.Length; i++)
				res[i] = instruction.GetDeclareByteValue(i);
			return res;
		}

		[Fact]
		void DeclareDataByteOrderIsSame() {
			var data = new byte[] { 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08 };
			var db = Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08);
			var dw = Instruction.CreateDeclareWord(0xA977, 0x9DCE, 0x0555, 0x6C42, 0x3286, 0x4FFE, 0x2734, 0x08AA);
			var dd = Instruction.CreateDeclareDword(0x9DCEA977, 0x6C420555, 0x4FFE3286, 0x08AA2734);
			var dq = Instruction.CreateDeclareQword(0x6C4205559DCEA977, 0x08AA27344FFE3286);
			var data1 = GetData(db);
			var data2 = GetData(dw);
			var data4 = GetData(dd);
			var data8 = GetData(dq);
			Assert.Equal(data, data1);
			Assert.Equal(data, data2);
			Assert.Equal(data, data4);
			Assert.Equal(data, data8);
		}

		[Fact]
		void DeclareByteCanGetSet() {
			var db = Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08);
			db.SetDeclareByteValue(0, 0xE2);
			db.SetDeclareByteValue(1, 0xC5);
			db.SetDeclareByteValue(2, 0xFA);
			db.SetDeclareByteValue(3, 0xB4);
			db.SetDeclareByteValue(4, 0xCB);
			db.SetDeclareByteValue(5, 0xE3);
			db.SetDeclareByteValue(6, 0x4D);
			db.SetDeclareByteValue(7, 0xE4);
			db.SetDeclareByteValue(8, 0x96);
			db.SetDeclareByteValue(9, 0x98);
			db.SetDeclareByteValue(10, 0xFD);
			db.SetDeclareByteValue(11, 0x56);
			db.SetDeclareByteValue(12, 0x82);
			db.SetDeclareByteValue(13, 0x8D);
			db.SetDeclareByteValue(14, 0x06);
			db.SetDeclareByteValue(15, 0xC3);
			Assert.Equal((byte)0xE2, db.GetDeclareByteValue(0));
			Assert.Equal((byte)0xC5, db.GetDeclareByteValue(1));
			Assert.Equal((byte)0xFA, db.GetDeclareByteValue(2));
			Assert.Equal((byte)0xB4, db.GetDeclareByteValue(3));
			Assert.Equal((byte)0xCB, db.GetDeclareByteValue(4));
			Assert.Equal((byte)0xE3, db.GetDeclareByteValue(5));
			Assert.Equal((byte)0x4D, db.GetDeclareByteValue(6));
			Assert.Equal((byte)0xE4, db.GetDeclareByteValue(7));
			Assert.Equal((byte)0x96, db.GetDeclareByteValue(8));
			Assert.Equal((byte)0x98, db.GetDeclareByteValue(9));
			Assert.Equal((byte)0xFD, db.GetDeclareByteValue(10));
			Assert.Equal((byte)0x56, db.GetDeclareByteValue(11));
			Assert.Equal((byte)0x82, db.GetDeclareByteValue(12));
			Assert.Equal((byte)0x8D, db.GetDeclareByteValue(13));
			Assert.Equal((byte)0x06, db.GetDeclareByteValue(14));
			Assert.Equal((byte)0xC3, db.GetDeclareByteValue(15));
		}

		[Fact]
		void DeclareByteCanGetSetRev() {
			var db = Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08);
			db.SetDeclareByteValue(15, 0xC3);
			db.SetDeclareByteValue(14, 0x06);
			db.SetDeclareByteValue(13, 0x8D);
			db.SetDeclareByteValue(12, 0x82);
			db.SetDeclareByteValue(11, 0x56);
			db.SetDeclareByteValue(10, 0xFD);
			db.SetDeclareByteValue(9, 0x98);
			db.SetDeclareByteValue(8, 0x96);
			db.SetDeclareByteValue(7, 0xE4);
			db.SetDeclareByteValue(6, 0x4D);
			db.SetDeclareByteValue(5, 0xE3);
			db.SetDeclareByteValue(4, 0xCB);
			db.SetDeclareByteValue(3, 0xB4);
			db.SetDeclareByteValue(2, 0xFA);
			db.SetDeclareByteValue(1, 0xC5);
			db.SetDeclareByteValue(0, 0xE2);
			Assert.Equal((byte)0xE2, db.GetDeclareByteValue(0));
			Assert.Equal((byte)0xC5, db.GetDeclareByteValue(1));
			Assert.Equal((byte)0xFA, db.GetDeclareByteValue(2));
			Assert.Equal((byte)0xB4, db.GetDeclareByteValue(3));
			Assert.Equal((byte)0xCB, db.GetDeclareByteValue(4));
			Assert.Equal((byte)0xE3, db.GetDeclareByteValue(5));
			Assert.Equal((byte)0x4D, db.GetDeclareByteValue(6));
			Assert.Equal((byte)0xE4, db.GetDeclareByteValue(7));
			Assert.Equal((byte)0x96, db.GetDeclareByteValue(8));
			Assert.Equal((byte)0x98, db.GetDeclareByteValue(9));
			Assert.Equal((byte)0xFD, db.GetDeclareByteValue(10));
			Assert.Equal((byte)0x56, db.GetDeclareByteValue(11));
			Assert.Equal((byte)0x82, db.GetDeclareByteValue(12));
			Assert.Equal((byte)0x8D, db.GetDeclareByteValue(13));
			Assert.Equal((byte)0x06, db.GetDeclareByteValue(14));
			Assert.Equal((byte)0xC3, db.GetDeclareByteValue(15));
		}

		[Fact]
		void DeclareWordCanGetSet() {
			var dw = Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xAA08);
			dw.SetDeclareWordValue(0, 0xE2C5);
			dw.SetDeclareWordValue(1, 0xFAB4);
			dw.SetDeclareWordValue(2, 0xCBE3);
			dw.SetDeclareWordValue(3, 0x4DE4);
			dw.SetDeclareWordValue(4, 0x9698);
			dw.SetDeclareWordValue(5, 0xFD56);
			dw.SetDeclareWordValue(6, 0x828D);
			dw.SetDeclareWordValue(7, 0x06C3);
			Assert.Equal((ushort)0xE2C5, dw.GetDeclareWordValue(0));
			Assert.Equal((ushort)0xFAB4, dw.GetDeclareWordValue(1));
			Assert.Equal((ushort)0xCBE3, dw.GetDeclareWordValue(2));
			Assert.Equal((ushort)0x4DE4, dw.GetDeclareWordValue(3));
			Assert.Equal((ushort)0x9698, dw.GetDeclareWordValue(4));
			Assert.Equal((ushort)0xFD56, dw.GetDeclareWordValue(5));
			Assert.Equal((ushort)0x828D, dw.GetDeclareWordValue(6));
			Assert.Equal((ushort)0x06C3, dw.GetDeclareWordValue(7));
		}

		[Fact]
		void DeclareWordCanGetSetRev() {
			var dw = Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xAA08);
			dw.SetDeclareWordValue(7, 0x06C3);
			dw.SetDeclareWordValue(6, 0x828D);
			dw.SetDeclareWordValue(5, 0xFD56);
			dw.SetDeclareWordValue(4, 0x9698);
			dw.SetDeclareWordValue(3, 0x4DE4);
			dw.SetDeclareWordValue(2, 0xCBE3);
			dw.SetDeclareWordValue(1, 0xFAB4);
			dw.SetDeclareWordValue(0, 0xE2C5);
			Assert.Equal((ushort)0xE2C5, dw.GetDeclareWordValue(0));
			Assert.Equal((ushort)0xFAB4, dw.GetDeclareWordValue(1));
			Assert.Equal((ushort)0xCBE3, dw.GetDeclareWordValue(2));
			Assert.Equal((ushort)0x4DE4, dw.GetDeclareWordValue(3));
			Assert.Equal((ushort)0x9698, dw.GetDeclareWordValue(4));
			Assert.Equal((ushort)0xFD56, dw.GetDeclareWordValue(5));
			Assert.Equal((ushort)0x828D, dw.GetDeclareWordValue(6));
			Assert.Equal((ushort)0x06C3, dw.GetDeclareWordValue(7));
		}

		[Fact]
		void DeclareDwordCanGetSet() {
			var dd = Instruction.CreateDeclareDword(0x77A9CE9D, 0x5505426C, 0x8632FE4F, 0x3427AA08);
			dd.SetDeclareDwordValue(0, 0xE2C5FAB4);
			dd.SetDeclareDwordValue(1, 0xCBE34DE4);
			dd.SetDeclareDwordValue(2, 0x9698FD56);
			dd.SetDeclareDwordValue(3, 0x828D06C3);
			Assert.Equal((uint)0xE2C5FAB4, dd.GetDeclareDwordValue(0));
			Assert.Equal((uint)0xCBE34DE4, dd.GetDeclareDwordValue(1));
			Assert.Equal((uint)0x9698FD56, dd.GetDeclareDwordValue(2));
			Assert.Equal((uint)0x828D06C3, dd.GetDeclareDwordValue(3));
		}

		[Fact]
		void DeclareDwordCanGetSetRev() {
			var dd = Instruction.CreateDeclareDword(0x77A9CE9D, 0x5505426C, 0x8632FE4F, 0x3427AA08);
			dd.SetDeclareDwordValue(3, 0x828D06C3);
			dd.SetDeclareDwordValue(2, 0x9698FD56);
			dd.SetDeclareDwordValue(1, 0xCBE34DE4);
			dd.SetDeclareDwordValue(0, 0xE2C5FAB4);
			Assert.Equal((uint)0xE2C5FAB4, dd.GetDeclareDwordValue(0));
			Assert.Equal((uint)0xCBE34DE4, dd.GetDeclareDwordValue(1));
			Assert.Equal((uint)0x9698FD56, dd.GetDeclareDwordValue(2));
			Assert.Equal((uint)0x828D06C3, dd.GetDeclareDwordValue(3));
		}

		[Fact]
		void DeclareQwordCanGetSet() {
			var dq = Instruction.CreateDeclareQword(0x77A9CE9D5505426C, 0x8632FE4F3427AA08);
			dq.SetDeclareQwordValue(0, 0xE2C5FAB4CBE34DE4);
			dq.SetDeclareQwordValue(1, 0x9698FD56828D06C3);
			Assert.Equal(0xE2C5FAB4CBE34DE4, dq.GetDeclareQwordValue(0));
			Assert.Equal(0x9698FD56828D06C3, dq.GetDeclareQwordValue(1));
		}

		[Fact]
		void DeclareQwordCanGetSetRev() {
			var dq = Instruction.CreateDeclareQword(0x77A9CE9D5505426C, 0x8632FE4F3427AA08);
			dq.SetDeclareQwordValue(1, 0x9698FD56828D06C3);
			dq.SetDeclareQwordValue(0, 0xE2C5FAB4CBE34DE4);
			Assert.Equal(0xE2C5FAB4CBE34DE4, dq.GetDeclareQwordValue(0));
			Assert.Equal(0x9698FD56828D06C3, dq.GetDeclareQwordValue(1));
		}

		[Fact]
		void DeclareDataDoesNotUseOtherProperties() {
			Instruction instruction;

			var data = new byte[16];
			for (int i = 0; i < data.Length; i++)
				data[i] = 0xFF;

			instruction = Instruction.CreateDeclareByte(data);
			Verify(instruction);

			instruction = Instruction.CreateDeclareWord(data);
			Verify(instruction);

			instruction = Instruction.CreateDeclareDword(data);
			Verify(instruction);

			instruction = Instruction.CreateDeclareQword(data);
			Verify(instruction);

			static void Verify(in Instruction instruction) {
				Assert.Equal(Register.None, instruction.SegmentPrefix);
				Assert.Equal(CodeSize.Unknown, instruction.CodeSize);
				Assert.Equal(RoundingControl.None, instruction.RoundingControl);
				Assert.Equal(0UL, instruction.IP);
				Assert.False(instruction.IsBroadcast);
				Assert.False(instruction.HasOpMask);
				Assert.False(instruction.SuppressAllExceptions);
				Assert.False(instruction.ZeroingMasking);
				Assert.False(instruction.HasXacquirePrefix);
				Assert.False(instruction.HasXreleasePrefix);
				Assert.False(instruction.HasRepPrefix);
				Assert.False(instruction.HasRepePrefix);
				Assert.False(instruction.HasRepnePrefix);
				Assert.False(instruction.HasLockPrefix);
			}
		}

		[Fact]
		void CreateDeclareByte() {
			Instruction instruction;

			instruction = Instruction.CreateDeclareByte(0x77);
			Assert.Equal(Code.DeclareByte, instruction.Code);
			Assert.Equal(1, instruction.DeclareDataCount);
			Assert.Equal(new byte[] { 0x77 }, GetData(instruction));

			instruction = Instruction.CreateDeclareByte(0x77, 0xA9);
			Assert.Equal(Code.DeclareByte, instruction.Code);
			Assert.Equal(2, instruction.DeclareDataCount);
			Assert.Equal(new byte[] { 0x77, 0xA9 }, GetData(instruction));

			instruction = Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE);
			Assert.Equal(Code.DeclareByte, instruction.Code);
			Assert.Equal(3, instruction.DeclareDataCount);
			Assert.Equal(new byte[] { 0x77, 0xA9, 0xCE }, GetData(instruction));

			instruction = Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D);
			Assert.Equal(Code.DeclareByte, instruction.Code);
			Assert.Equal(4, instruction.DeclareDataCount);
			Assert.Equal(new byte[] { 0x77, 0xA9, 0xCE, 0x9D }, GetData(instruction));

			instruction = Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55);
			Assert.Equal(Code.DeclareByte, instruction.Code);
			Assert.Equal(5, instruction.DeclareDataCount);
			Assert.Equal(new byte[] { 0x77, 0xA9, 0xCE, 0x9D, 0x55 }, GetData(instruction));

			instruction = Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05);
			Assert.Equal(Code.DeclareByte, instruction.Code);
			Assert.Equal(6, instruction.DeclareDataCount);
			Assert.Equal(new byte[] { 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05 }, GetData(instruction));

			instruction = Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42);
			Assert.Equal(Code.DeclareByte, instruction.Code);
			Assert.Equal(7, instruction.DeclareDataCount);
			Assert.Equal(new byte[] { 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42 }, GetData(instruction));

			instruction = Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C);
			Assert.Equal(Code.DeclareByte, instruction.Code);
			Assert.Equal(8, instruction.DeclareDataCount);
			Assert.Equal(new byte[] { 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C }, GetData(instruction));

			instruction = Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86);
			Assert.Equal(Code.DeclareByte, instruction.Code);
			Assert.Equal(9, instruction.DeclareDataCount);
			Assert.Equal(new byte[] { 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86 }, GetData(instruction));

			instruction = Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32);
			Assert.Equal(Code.DeclareByte, instruction.Code);
			Assert.Equal(10, instruction.DeclareDataCount);
			Assert.Equal(new byte[] { 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32 }, GetData(instruction));

			instruction = Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE);
			Assert.Equal(Code.DeclareByte, instruction.Code);
			Assert.Equal(11, instruction.DeclareDataCount);
			Assert.Equal(new byte[] { 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE }, GetData(instruction));

			instruction = Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F);
			Assert.Equal(Code.DeclareByte, instruction.Code);
			Assert.Equal(12, instruction.DeclareDataCount);
			Assert.Equal(new byte[] { 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F }, GetData(instruction));

			instruction = Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34);
			Assert.Equal(Code.DeclareByte, instruction.Code);
			Assert.Equal(13, instruction.DeclareDataCount);
			Assert.Equal(new byte[] { 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34 }, GetData(instruction));

			instruction = Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27);
			Assert.Equal(Code.DeclareByte, instruction.Code);
			Assert.Equal(14, instruction.DeclareDataCount);
			Assert.Equal(new byte[] { 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27 }, GetData(instruction));

			instruction = Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA);
			Assert.Equal(Code.DeclareByte, instruction.Code);
			Assert.Equal(15, instruction.DeclareDataCount);
			Assert.Equal(new byte[] { 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA }, GetData(instruction));

			instruction = Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08);
			Assert.Equal(Code.DeclareByte, instruction.Code);
			Assert.Equal(16, instruction.DeclareDataCount);
			Assert.Equal(new byte[] { 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08 }, GetData(instruction));
		}

		[Fact]
		void CreateDeclareWord() {
			Instruction instruction;

			instruction = Instruction.CreateDeclareWord(0x77A9);
			Assert.Equal(Code.DeclareWord, instruction.Code);
			Assert.Equal(1, instruction.DeclareDataCount);
			Assert.Equal(new byte[] { 0xA9, 0x77 }, GetData(instruction));

			instruction = Instruction.CreateDeclareWord(0x77A9, 0xCE9D);
			Assert.Equal(Code.DeclareWord, instruction.Code);
			Assert.Equal(2, instruction.DeclareDataCount);
			Assert.Equal(new byte[] { 0xA9, 0x77, 0x9D, 0xCE }, GetData(instruction));

			instruction = Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505);
			Assert.Equal(Code.DeclareWord, instruction.Code);
			Assert.Equal(3, instruction.DeclareDataCount);
			Assert.Equal(new byte[] { 0xA9, 0x77, 0x9D, 0xCE, 0x05, 0x55 }, GetData(instruction));

			instruction = Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C);
			Assert.Equal(Code.DeclareWord, instruction.Code);
			Assert.Equal(4, instruction.DeclareDataCount);
			Assert.Equal(new byte[] { 0xA9, 0x77, 0x9D, 0xCE, 0x05, 0x55, 0x6C, 0x42 }, GetData(instruction));

			instruction = Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632);
			Assert.Equal(Code.DeclareWord, instruction.Code);
			Assert.Equal(5, instruction.DeclareDataCount);
			Assert.Equal(new byte[] { 0xA9, 0x77, 0x9D, 0xCE, 0x05, 0x55, 0x6C, 0x42, 0x32, 0x86 }, GetData(instruction));

			instruction = Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F);
			Assert.Equal(Code.DeclareWord, instruction.Code);
			Assert.Equal(6, instruction.DeclareDataCount);
			Assert.Equal(new byte[] { 0xA9, 0x77, 0x9D, 0xCE, 0x05, 0x55, 0x6C, 0x42, 0x32, 0x86, 0x4F, 0xFE }, GetData(instruction));

			instruction = Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427);
			Assert.Equal(Code.DeclareWord, instruction.Code);
			Assert.Equal(7, instruction.DeclareDataCount);
			Assert.Equal(new byte[] { 0xA9, 0x77, 0x9D, 0xCE, 0x05, 0x55, 0x6C, 0x42, 0x32, 0x86, 0x4F, 0xFE, 0x27, 0x34 }, GetData(instruction));

			instruction = Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xAA08);
			Assert.Equal(Code.DeclareWord, instruction.Code);
			Assert.Equal(8, instruction.DeclareDataCount);
			Assert.Equal(new byte[] { 0xA9, 0x77, 0x9D, 0xCE, 0x05, 0x55, 0x6C, 0x42, 0x32, 0x86, 0x4F, 0xFE, 0x27, 0x34, 0x08, 0xAA }, GetData(instruction));
		}

		[Fact]
		void CreateDeclareDword() {
			Instruction instruction;

			instruction = Instruction.CreateDeclareDword(0x77A9CE9D);
			Assert.Equal(Code.DeclareDword, instruction.Code);
			Assert.Equal(1, instruction.DeclareDataCount);
			Assert.Equal(new byte[] { 0x9D, 0xCE, 0xA9, 0x77 }, GetData(instruction));

			instruction = Instruction.CreateDeclareDword(0x77A9CE9D, 0x5505426C);
			Assert.Equal(Code.DeclareDword, instruction.Code);
			Assert.Equal(2, instruction.DeclareDataCount);
			Assert.Equal(new byte[] { 0x9D, 0xCE, 0xA9, 0x77, 0x6C, 0x42, 0x05, 0x55 }, GetData(instruction));

			instruction = Instruction.CreateDeclareDword(0x77A9CE9D, 0x5505426C, 0x8632FE4F);
			Assert.Equal(Code.DeclareDword, instruction.Code);
			Assert.Equal(3, instruction.DeclareDataCount);
			Assert.Equal(new byte[] { 0x9D, 0xCE, 0xA9, 0x77, 0x6C, 0x42, 0x05, 0x55, 0x4F, 0xFE, 0x32, 0x86 }, GetData(instruction));

			instruction = Instruction.CreateDeclareDword(0x77A9CE9D, 0x5505426C, 0x8632FE4F, 0x3427AA08);
			Assert.Equal(Code.DeclareDword, instruction.Code);
			Assert.Equal(4, instruction.DeclareDataCount);
			Assert.Equal(new byte[] { 0x9D, 0xCE, 0xA9, 0x77, 0x6C, 0x42, 0x05, 0x55, 0x4F, 0xFE, 0x32, 0x86, 0x08, 0xAA, 0x27, 0x34 }, GetData(instruction));
		}

		[Fact]
		void CreateDeclareQword() {
			Instruction instruction;

			instruction = Instruction.CreateDeclareQword(0x77A9CE9D5505426C);
			Assert.Equal(Code.DeclareQword, instruction.Code);
			Assert.Equal(1, instruction.DeclareDataCount);
			Assert.Equal(new byte[] { 0x6C, 0x42, 0x05, 0x55, 0x9D, 0xCE, 0xA9, 0x77 }, GetData(instruction));

			instruction = Instruction.CreateDeclareQword(0x77A9CE9D5505426C, 0x8632FE4F3427AA08);
			Assert.Equal(Code.DeclareQword, instruction.Code);
			Assert.Equal(2, instruction.DeclareDataCount);
			Assert.Equal(new byte[] { 0x6C, 0x42, 0x05, 0x55, 0x9D, 0xCE, 0xA9, 0x77, 0x08, 0xAA, 0x27, 0x34, 0x4F, 0xFE, 0x32, 0x86 }, GetData(instruction));
		}

		[Fact]
		void CreateDeclareByteArray() {
			var data = new (Instruction instruction, byte[] data)[] {
				(Instruction.CreateDeclareByte(0x77), new byte[] { 0x77 }),
				(Instruction.CreateDeclareByte(0x77, 0xA9), new byte[] { 0x77, 0xA9 }),
				(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE), new byte[] { 0x77, 0xA9, 0xCE }),
				(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D), new byte[] { 0x77, 0xA9, 0xCE, 0x9D }),
				(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55), new byte[] { 0x77, 0xA9, 0xCE, 0x9D, 0x55 }),
				(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05), new byte[] { 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05 }),
				(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42), new byte[] { 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42 }),
				(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C), new byte[] { 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C }),
				(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86), new byte[] { 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86 }),
				(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32), new byte[] { 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32 }),
				(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE), new byte[] { 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE }),
				(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F), new byte[] { 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F }),
				(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34), new byte[] { 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34 }),
				(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27), new byte[] { 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27 }),
				(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA), new byte[] { 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA }),
				(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08), new byte[] { 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08 }),
			};
			foreach (var info in data) {
				var instruction1 = info.instruction;
				var instruction2 = Instruction.CreateDeclareByte(info.data);
				Assert.True(Instruction.EqualsAllBits(instruction1, instruction2));
#if HAS_SPAN
				var instr3 = Instruction.CreateDeclareByte(info.data.AsSpan());
				Assert.True(Instruction.EqualsAllBits(instruction1, instr3));
#endif
			}
		}

		[Fact]
		void CreateDeclareWordArray() {
			var data = new (Instruction instruction, byte[] data)[] {
				(Instruction.CreateDeclareWord(0x77A9), new byte[] { 0xA9, 0x77 }),
				(Instruction.CreateDeclareWord(0x77A9, 0xCE9D), new byte[] { 0xA9, 0x77, 0x9D, 0xCE }),
				(Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505), new byte[] { 0xA9, 0x77, 0x9D, 0xCE, 0x05, 0x55 }),
				(Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C), new byte[] { 0xA9, 0x77, 0x9D, 0xCE, 0x05, 0x55, 0x6C, 0x42 }),
				(Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632), new byte[] { 0xA9, 0x77, 0x9D, 0xCE, 0x05, 0x55, 0x6C, 0x42, 0x32, 0x86 }),
				(Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F), new byte[] { 0xA9, 0x77, 0x9D, 0xCE, 0x05, 0x55, 0x6C, 0x42, 0x32, 0x86, 0x4F, 0xFE }),
				(Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427), new byte[] { 0xA9, 0x77, 0x9D, 0xCE, 0x05, 0x55, 0x6C, 0x42, 0x32, 0x86, 0x4F, 0xFE, 0x27, 0x34 }),
				(Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xAA08), new byte[] { 0xA9, 0x77, 0x9D, 0xCE, 0x05, 0x55, 0x6C, 0x42, 0x32, 0x86, 0x4F, 0xFE, 0x27, 0x34, 0x08, 0xAA }),
			};
			foreach (var info in data) {
				var instruction1 = info.instruction;
				var instruction2 = Instruction.CreateDeclareWord(info.data);
				Assert.True(Instruction.EqualsAllBits(instruction1, instruction2));
#if HAS_SPAN
				var instr3 = Instruction.CreateDeclareWord(info.data.AsSpan());
				Assert.True(Instruction.EqualsAllBits(instruction1, instr3));
#endif
			}
		}

		[Fact]
		void CreateDeclareDwordArray() {
			var data = new (Instruction instruction, byte[] data)[] {
				(Instruction.CreateDeclareDword(0x77A9CE9D), new byte[] { 0x9D, 0xCE, 0xA9, 0x77 }),
				(Instruction.CreateDeclareDword(0x77A9CE9D, 0x5505426C), new byte[] { 0x9D, 0xCE, 0xA9, 0x77, 0x6C, 0x42, 0x05, 0x55 }),
				(Instruction.CreateDeclareDword(0x77A9CE9D, 0x5505426C, 0x8632FE4F), new byte[] { 0x9D, 0xCE, 0xA9, 0x77, 0x6C, 0x42, 0x05, 0x55, 0x4F, 0xFE, 0x32, 0x86 }),
				(Instruction.CreateDeclareDword(0x77A9CE9D, 0x5505426C, 0x8632FE4F, 0x3427AA08), new byte[] { 0x9D, 0xCE, 0xA9, 0x77, 0x6C, 0x42, 0x05, 0x55, 0x4F, 0xFE, 0x32, 0x86, 0x08, 0xAA, 0x27, 0x34 }),
			};
			foreach (var info in data) {
				var instruction1 = info.instruction;
				var instruction2 = Instruction.CreateDeclareDword(info.data);
				Assert.True(Instruction.EqualsAllBits(instruction1, instruction2));
#if HAS_SPAN
				var instr3 = Instruction.CreateDeclareDword(info.data.AsSpan());
				Assert.True(Instruction.EqualsAllBits(instruction1, instr3));
#endif
			}
		}

		[Fact]
		void CreateDeclareQwordArray() {
			var data = new (Instruction instruction, byte[] data)[] {
				(Instruction.CreateDeclareQword(0x77A9CE9D5505426C), new byte[] { 0x6C, 0x42, 0x05, 0x55, 0x9D, 0xCE, 0xA9, 0x77 }),
				(Instruction.CreateDeclareQword(0x77A9CE9D5505426C, 0x8632FE4F3427AA08), new byte[] { 0x6C, 0x42, 0x05, 0x55, 0x9D, 0xCE, 0xA9, 0x77, 0x08, 0xAA, 0x27, 0x34, 0x4F, 0xFE, 0x32, 0x86 }),
			};
			foreach (var info in data) {
				var instruction1 = info.instruction;
				var instruction2 = Instruction.CreateDeclareQword(info.data);
				Assert.True(Instruction.EqualsAllBits(instruction1, instruction2));
#if HAS_SPAN
				var instr3 = Instruction.CreateDeclareQword(info.data.AsSpan());
				Assert.True(Instruction.EqualsAllBits(instruction1, instr3));
#endif
			}
		}

		[Fact]
		void CreateDeclareByteArray2() {
			var data = new (Instruction instruction, byte[] data)[] {
				(Instruction.CreateDeclareByte(0x77), new byte[] { 0xA5, 0x77, 0x5A }),
				(Instruction.CreateDeclareByte(0x77, 0xA9), new byte[] { 0xA5, 0x77, 0xA9, 0x5A }),
				(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE), new byte[] { 0xA5, 0x77, 0xA9, 0xCE, 0x5A }),
				(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D), new byte[] { 0xA5, 0x77, 0xA9, 0xCE, 0x9D, 0x5A }),
				(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55), new byte[] { 0xA5, 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x5A }),
				(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05), new byte[] { 0xA5, 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x5A }),
				(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42), new byte[] { 0xA5, 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x5A }),
				(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C), new byte[] { 0xA5, 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x5A }),
				(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86), new byte[] { 0xA5, 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x5A }),
				(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32), new byte[] { 0xA5, 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0x5A }),
				(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE), new byte[] { 0xA5, 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x5A }),
				(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F), new byte[] { 0xA5, 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x5A }),
				(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34), new byte[] { 0xA5, 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x5A }),
				(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27), new byte[] { 0xA5, 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0x5A }),
				(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA), new byte[] { 0xA5, 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x5A }),
				(Instruction.CreateDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08), new byte[] { 0xA5, 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08, 0x5A }),
			};
			foreach (var info in data) {
				var instruction1 = info.instruction;
				var instruction2 = Instruction.CreateDeclareByte(info.data, 1, info.data.Length - 2);
				Assert.True(Instruction.EqualsAllBits(instruction1, instruction2));
#if HAS_SPAN
				var instr3 = Instruction.CreateDeclareByte(info.data.AsSpan(1, info.data.Length - 2));
				Assert.True(Instruction.EqualsAllBits(instruction1, instr3));
#endif
			}
		}

		[Fact]
		void CreateDeclareWordArray2() {
			var data = new (Instruction instruction, byte[] data)[] {
				(Instruction.CreateDeclareWord(0x77A9), new byte[] { 0xA5, 0xA9, 0x77, 0x5A }),
				(Instruction.CreateDeclareWord(0x77A9, 0xCE9D), new byte[] { 0xA5, 0xA9, 0x77, 0x9D, 0xCE, 0x5A }),
				(Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505), new byte[] { 0xA5, 0xA9, 0x77, 0x9D, 0xCE, 0x05, 0x55, 0x5A }),
				(Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C), new byte[] { 0xA5, 0xA9, 0x77, 0x9D, 0xCE, 0x05, 0x55, 0x6C, 0x42, 0x5A }),
				(Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632), new byte[] { 0xA5, 0xA9, 0x77, 0x9D, 0xCE, 0x05, 0x55, 0x6C, 0x42, 0x32, 0x86, 0x5A }),
				(Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F), new byte[] { 0xA5, 0xA9, 0x77, 0x9D, 0xCE, 0x05, 0x55, 0x6C, 0x42, 0x32, 0x86, 0x4F, 0xFE, 0x5A }),
				(Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427), new byte[] { 0xA5, 0xA9, 0x77, 0x9D, 0xCE, 0x05, 0x55, 0x6C, 0x42, 0x32, 0x86, 0x4F, 0xFE, 0x27, 0x34, 0x5A }),
				(Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xAA08), new byte[] { 0xA5, 0xA9, 0x77, 0x9D, 0xCE, 0x05, 0x55, 0x6C, 0x42, 0x32, 0x86, 0x4F, 0xFE, 0x27, 0x34, 0x08, 0xAA, 0x5A }),
			};
			foreach (var info in data) {
				var instruction1 = info.instruction;
				var instruction2 = Instruction.CreateDeclareWord(info.data, 1, info.data.Length - 2);
				Assert.True(Instruction.EqualsAllBits(instruction1, instruction2));
#if HAS_SPAN
				var instr3 = Instruction.CreateDeclareWord(info.data.AsSpan(1, info.data.Length - 2));
				Assert.True(Instruction.EqualsAllBits(instruction1, instr3));
#endif
			}
		}

		[Fact]
		void CreateDeclareDwordArray2() {
			var data = new (Instruction instruction, byte[] data)[] {
				(Instruction.CreateDeclareDword(0x77A9CE9D), new byte[] { 0xA5, 0x9D, 0xCE, 0xA9, 0x77, 0x5A }),
				(Instruction.CreateDeclareDword(0x77A9CE9D, 0x5505426C), new byte[] { 0xA5, 0x9D, 0xCE, 0xA9, 0x77, 0x6C, 0x42, 0x05, 0x55, 0x5A }),
				(Instruction.CreateDeclareDword(0x77A9CE9D, 0x5505426C, 0x8632FE4F), new byte[] { 0xA5, 0x9D, 0xCE, 0xA9, 0x77, 0x6C, 0x42, 0x05, 0x55, 0x4F, 0xFE, 0x32, 0x86, 0x5A }),
				(Instruction.CreateDeclareDword(0x77A9CE9D, 0x5505426C, 0x8632FE4F, 0x3427AA08), new byte[] { 0xA5, 0x9D, 0xCE, 0xA9, 0x77, 0x6C, 0x42, 0x05, 0x55, 0x4F, 0xFE, 0x32, 0x86, 0x08, 0xAA, 0x27, 0x34, 0x5A }),
			};
			foreach (var info in data) {
				var instruction1 = info.instruction;
				var instruction2 = Instruction.CreateDeclareDword(info.data, 1, info.data.Length - 2);
				Assert.True(Instruction.EqualsAllBits(instruction1, instruction2));
#if HAS_SPAN
				var instr3 = Instruction.CreateDeclareDword(info.data.AsSpan(1, info.data.Length - 2));
				Assert.True(Instruction.EqualsAllBits(instruction1, instr3));
#endif
			}
		}

		[Fact]
		void CreateDeclareQwordArray2() {
			var data = new (Instruction instruction, byte[] data)[] {
				(Instruction.CreateDeclareQword(0x77A9CE9D5505426C), new byte[] { 0xA5, 0x6C, 0x42, 0x05, 0x55, 0x9D, 0xCE, 0xA9, 0x77, 0x5A }),
				(Instruction.CreateDeclareQword(0x77A9CE9D5505426C, 0x8632FE4F3427AA08), new byte[] { 0xA5, 0x6C, 0x42, 0x05, 0x55, 0x9D, 0xCE, 0xA9, 0x77, 0x08, 0xAA, 0x27, 0x34, 0x4F, 0xFE, 0x32, 0x86, 0x5A }),
			};
			foreach (var info in data) {
				var instruction1 = info.instruction;
				var instruction2 = Instruction.CreateDeclareQword(info.data, 1, info.data.Length - 2);
				Assert.True(Instruction.EqualsAllBits(instruction1, instruction2));
#if HAS_SPAN
				var instr3 = Instruction.CreateDeclareQword(info.data.AsSpan(1, info.data.Length - 2));
				Assert.True(Instruction.EqualsAllBits(instruction1, instr3));
#endif
			}
		}

		[Fact]
		void CreateDeclareWordArray3() {
			var data = new (Instruction instruction, ushort[] data)[] {
				(Instruction.CreateDeclareWord(0x77A9), new ushort[] { 0x77A9 }),
				(Instruction.CreateDeclareWord(0x77A9, 0xCE9D), new ushort[] { 0x77A9, 0xCE9D }),
				(Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505), new ushort[] { 0x77A9, 0xCE9D, 0x5505 }),
				(Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C), new ushort[] { 0x77A9, 0xCE9D, 0x5505, 0x426C }),
				(Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632), new ushort[] { 0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632 }),
				(Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F), new ushort[] { 0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F }),
				(Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427), new ushort[] { 0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427 }),
				(Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xAA08), new ushort[] { 0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xAA08 }),
			};
			foreach (var info in data) {
				var instruction1 = info.instruction;
				var instruction2 = Instruction.CreateDeclareWord(info.data);
				Assert.True(Instruction.EqualsAllBits(instruction1, instruction2));
#if HAS_SPAN
				var instr3 = Instruction.CreateDeclareWord(info.data.AsSpan());
				Assert.True(Instruction.EqualsAllBits(instruction1, instr3));
#endif
			}
		}

		[Fact]
		void CreateDeclareDwordArray3() {
			var data = new (Instruction instruction, uint[] data)[] {
				(Instruction.CreateDeclareDword(0x77A9CE9D), new uint[] { 0x77A9CE9D }),
				(Instruction.CreateDeclareDword(0x77A9CE9D, 0x5505426C), new uint[] { 0x77A9CE9D, 0x5505426C }),
				(Instruction.CreateDeclareDword(0x77A9CE9D, 0x5505426C, 0x8632FE4F), new uint[] { 0x77A9CE9D, 0x5505426C, 0x8632FE4F }),
				(Instruction.CreateDeclareDword(0x77A9CE9D, 0x5505426C, 0x8632FE4F, 0x3427AA08), new uint[] { 0x77A9CE9D, 0x5505426C, 0x8632FE4F, 0x3427AA08 }),
			};
			foreach (var info in data) {
				var instruction1 = info.instruction;
				var instruction2 = Instruction.CreateDeclareDword(info.data);
				Assert.True(Instruction.EqualsAllBits(instruction1, instruction2));
#if HAS_SPAN
				var instr3 = Instruction.CreateDeclareDword(info.data.AsSpan());
				Assert.True(Instruction.EqualsAllBits(instruction1, instr3));
#endif
			}
		}

		[Fact]
		void CreateDeclareQwordArray3() {
			var data = new (Instruction instruction, ulong[] data)[] {
				(Instruction.CreateDeclareQword(0x77A9CE9D5505426C), new ulong[] { 0x77A9CE9D5505426C }),
				(Instruction.CreateDeclareQword(0x77A9CE9D5505426C, 0x8632FE4F3427AA08), new ulong[] { 0x77A9CE9D5505426C, 0x8632FE4F3427AA08 }),
			};
			foreach (var info in data) {
				var instruction1 = info.instruction;
				var instruction2 = Instruction.CreateDeclareQword(info.data);
				Assert.True(Instruction.EqualsAllBits(instruction1, instruction2));
#if HAS_SPAN
				var instr3 = Instruction.CreateDeclareQword(info.data.AsSpan());
				Assert.True(Instruction.EqualsAllBits(instruction1, instr3));
#endif
			}
		}

		[Fact]
		void CreateDeclareWordArray4() {
			var data = new (Instruction instruction, ushort[] data)[] {
				(Instruction.CreateDeclareWord(0x77A9), new ushort[] { 0x5AA5, 0x77A9, 0xA55A }),
				(Instruction.CreateDeclareWord(0x77A9, 0xCE9D), new ushort[] { 0x5AA5, 0x77A9, 0xCE9D, 0xA55A }),
				(Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505), new ushort[] { 0x5AA5, 0x77A9, 0xCE9D, 0x5505, 0xA55A }),
				(Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C), new ushort[] { 0x5AA5, 0x77A9, 0xCE9D, 0x5505, 0x426C, 0xA55A }),
				(Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632), new ushort[] { 0x5AA5, 0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xA55A }),
				(Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F), new ushort[] { 0x5AA5, 0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0xA55A }),
				(Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427), new ushort[] { 0x5AA5, 0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xA55A }),
				(Instruction.CreateDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xAA08), new ushort[] { 0x5AA5, 0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xAA08, 0xA55A }),
			};
			foreach (var info in data) {
				var instruction1 = info.instruction;
				var instruction2 = Instruction.CreateDeclareWord(info.data, 1, info.data.Length - 2);
				Assert.True(Instruction.EqualsAllBits(instruction1, instruction2));
#if HAS_SPAN
				var instr3 = Instruction.CreateDeclareWord(info.data.AsSpan(1, info.data.Length - 2));
				Assert.True(Instruction.EqualsAllBits(instruction1, instr3));
#endif
			}
		}

		[Fact]
		void CreateDeclareDwordArray4() {
			var data = new (Instruction instruction, uint[] data)[] {
				(Instruction.CreateDeclareDword(0x77A9CE9D), new uint[] { 0x5AA5A55A, 0x77A9CE9D, 0xA55A5AA5 }),
				(Instruction.CreateDeclareDword(0x77A9CE9D, 0x5505426C), new uint[] { 0x5AA5A55A, 0x77A9CE9D, 0x5505426C, 0xA55A5AA5 }),
				(Instruction.CreateDeclareDword(0x77A9CE9D, 0x5505426C, 0x8632FE4F), new uint[] { 0x5AA5A55A, 0x77A9CE9D, 0x5505426C, 0x8632FE4F, 0xA55A5AA5 }),
				(Instruction.CreateDeclareDword(0x77A9CE9D, 0x5505426C, 0x8632FE4F, 0x3427AA08), new uint[] { 0x5AA5A55A, 0x77A9CE9D, 0x5505426C, 0x8632FE4F, 0x3427AA08, 0xA55A5AA5 }),
			};
			foreach (var info in data) {
				var instruction1 = info.instruction;
				var instruction2 = Instruction.CreateDeclareDword(info.data, 1, info.data.Length - 2);
				Assert.True(Instruction.EqualsAllBits(instruction1, instruction2));
#if HAS_SPAN
				var instr3 = Instruction.CreateDeclareDword(info.data.AsSpan(1, info.data.Length - 2));
				Assert.True(Instruction.EqualsAllBits(instruction1, instr3));
#endif
			}
		}

		[Fact]
		void CreateDeclareQwordArray4() {
			var data = new (Instruction instruction, ulong[] data)[] {
				(Instruction.CreateDeclareQword(0x77A9CE9D5505426C), new ulong[] { 0x5AA5A55A5AA5A55A, 0x77A9CE9D5505426C, 0xA55A5AA5A55A5AA5 }),
				(Instruction.CreateDeclareQword(0x77A9CE9D5505426C, 0x8632FE4F3427AA08), new ulong[] { 0x5AA5A55A5AA5A55A, 0x77A9CE9D5505426C, 0x8632FE4F3427AA08, 0xA55A5AA5A55A5AA5 }),
			};
			foreach (var info in data) {
				var instruction1 = info.instruction;
				var instruction2 = Instruction.CreateDeclareQword(info.data, 1, info.data.Length - 2);
				Assert.True(Instruction.EqualsAllBits(instruction1, instruction2));
#if HAS_SPAN
				var instr3 = Instruction.CreateDeclareQword(info.data.AsSpan(1, info.data.Length - 2));
				Assert.True(Instruction.EqualsAllBits(instruction1, instr3));
#endif
			}
		}

		[Theory]
		[MemberData(nameof(CreateTest_Data))]
		[MemberData(nameof(CreateTestVEX_Data))]
		[MemberData(nameof(CreateTestEVEX_Data))]
		void CreateTest(int bitness, string hexBytes, DecoderOptions options, Instruction createdInstr) {
			var bytes = HexUtils.ToByteArray(hexBytes);
			var decoder = Decoder.Create(bitness, new ByteArrayCodeReader(bytes), options);
			decoder.IP = bitness switch {
				16 => DecoderConstants.DEFAULT_IP16,
				32 => DecoderConstants.DEFAULT_IP32,
				64 => DecoderConstants.DEFAULT_IP64,
				_ => throw new InvalidOperationException(),
			};
			var origRip = decoder.IP;
			decoder.Decode(out var decodedInstr);
			decodedInstr.CodeSize = 0;
			decodedInstr.Length = 0;
			decodedInstr.NextIP = 0;

			Assert.True(Instruction.EqualsAllBits(decodedInstr, createdInstr));

			var writer = new CodeWriterImpl();
			var encoder = Encoder.Create(decoder.Bitness, writer);
			bool result = encoder.TryEncode(createdInstr, origRip, out _, out var errorMessage);
			Assert.Null(errorMessage);
			Assert.True(result);
			Assert.Equal(bytes, writer.ToArray());
		}
		public static IEnumerable<object[]> CreateTest_Data {
			get {
				yield return new object[] { 64, "90", DecoderOptions.None, Instruction.Create(Code.Nopd) };
				yield return new object[] { 64, "48B9FFFFFFFFFFFFFFFF", DecoderOptions.None, Instruction.Create(Code.Mov_r64_imm64, Register.RCX, -1L) };
				yield return new object[] { 64, "48B9FFFFFFFFFFFFFFFF", DecoderOptions.None, Instruction.Create(Code.Mov_r64_imm64, Register.RCX, -1) };
				yield return new object[] { 64, "48B9123456789ABCDE31", DecoderOptions.None, Instruction.Create(Code.Mov_r64_imm64, Register.RCX, 0x31DEBC9A78563412UL) };
				yield return new object[] { 64, "48B9FFFFFFFF00000000", DecoderOptions.None, Instruction.Create(Code.Mov_r64_imm64, Register.RCX, 0xFFFFFFFFU) };
				yield return new object[] { 64, "8FC1", DecoderOptions.None, Instruction.Create(Code.Pop_rm64, Register.RCX) };
				yield return new object[] { 64, "648F847501EFCDAB", DecoderOptions.None, Instruction.Create(Code.Pop_rm64, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS)) };
				yield return new object[] { 64, "C6F85A", DecoderOptions.None, Instruction.Create(Code.Xabort_imm8, 0x5AU) };
				yield return new object[] { 64, "66685AA5", DecoderOptions.None, Instruction.Create(Code.Push_imm16, 0xA55A) };
				yield return new object[] { 32, "685AA51234", DecoderOptions.None, Instruction.Create(Code.Pushd_imm32, 0x3412A55A) };
				yield return new object[] { 64, "666A5A", DecoderOptions.None, Instruction.Create(Code.Pushw_imm8, 0x5A) };
				yield return new object[] { 32, "6A5A", DecoderOptions.None, Instruction.Create(Code.Pushd_imm8, 0x5A) };
				yield return new object[] { 64, "6A5A", DecoderOptions.None, Instruction.Create(Code.Pushq_imm8, 0x5A) };
				yield return new object[] { 64, "685AA512A4", DecoderOptions.None, Instruction.Create(Code.Pushq_imm32, -0x5BED5AA6) };
				yield return new object[] { 32, "66705A", DecoderOptions.None, Instruction.CreateBranch(Code.Jo_rel8_16, 0x4D) };
				yield return new object[] { 32, "705A", DecoderOptions.None, Instruction.CreateBranch(Code.Jo_rel8_32, 0x8000004C) };
				yield return new object[] { 64, "705A", DecoderOptions.None, Instruction.CreateBranch(Code.Jo_rel8_64, 0x800000000000004C) };
				yield return new object[] { 32, "669A12345678", DecoderOptions.None, Instruction.CreateBranch(Code.Call_ptr1616, 0x7856, 0x3412) };
				yield return new object[] { 32, "9A123456789ABC", DecoderOptions.None, Instruction.CreateBranch(Code.Call_ptr1632, 0xBC9A, 0x78563412) };
				yield return new object[] { 16, "C7F85AA5", DecoderOptions.None, Instruction.CreateXbegin(16, 0x254E) };
				yield return new object[] { 32, "C7F85AA51234", DecoderOptions.None, Instruction.CreateXbegin(32, 0xB412A550) };
				yield return new object[] { 64, "C7F85AA51234", DecoderOptions.None, Instruction.CreateXbegin(64, 0x800000003412A550) };
				yield return new object[] { 64, "00D1", DecoderOptions.None, Instruction.Create(Code.Add_rm8_r8, Register.CL, Register.DL) };
				yield return new object[] { 64, "64028C7501EFCDAB", DecoderOptions.None, Instruction.Create(Code.Add_r8_rm8, Register.CL, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS)) };
				yield return new object[] { 64, "80C15A", DecoderOptions.None, Instruction.Create(Code.Add_rm8_imm8, Register.CL, 0x5A) };
				yield return new object[] { 64, "6681C15AA5", DecoderOptions.None, Instruction.Create(Code.Add_rm16_imm16, Register.CX, 0xA55A) };
				yield return new object[] { 64, "81C15AA51234", DecoderOptions.None, Instruction.Create(Code.Add_rm32_imm32, Register.ECX, 0x3412A55A) };
				yield return new object[] { 64, "48B904152637A55A5678", DecoderOptions.None, Instruction.Create(Code.Mov_r64_imm64, Register.RCX, 0x78565AA537261504UL) };
				yield return new object[] { 64, "6683C15A", DecoderOptions.None, Instruction.Create(Code.Add_rm16_imm8, Register.CX, 0x5A) };
				yield return new object[] { 64, "83C15A", DecoderOptions.None, Instruction.Create(Code.Add_rm32_imm8, Register.ECX, 0x5A) };
				yield return new object[] { 64, "4883C15A", DecoderOptions.None, Instruction.Create(Code.Add_rm64_imm8, Register.RCX, 0x5A) };
				yield return new object[] { 64, "4881C15AA51234", DecoderOptions.None, Instruction.Create(Code.Add_rm64_imm32, Register.RCX, 0x3412A55A) };
				yield return new object[] { 64, "64A0123456789ABCDEF0", DecoderOptions.None, Instruction.Create(Code.Mov_AL_moffs8, Register.AL, new MemoryOperand(Register.None, unchecked((long)0xF0DEBC9A78563412), 8, false, Register.FS)) };
				yield return new object[] { 64, "6400947501EFCDAB", DecoderOptions.None, Instruction.Create(Code.Add_rm8_r8, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), Register.DL) };
				yield return new object[] { 64, "6480847501EFCDAB5A", DecoderOptions.None, Instruction.Create(Code.Add_rm8_imm8, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x5A) };
				yield return new object[] { 64, "646681847501EFCDAB5AA5", DecoderOptions.None, Instruction.Create(Code.Add_rm16_imm16, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0xA55AU) };
				yield return new object[] { 64, "6481847501EFCDAB5AA51234", DecoderOptions.None, Instruction.Create(Code.Add_rm32_imm32, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x3412A55A) };
				yield return new object[] { 64, "646683847501EFCDAB5A", DecoderOptions.None, Instruction.Create(Code.Add_rm16_imm8, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x5A) };
				yield return new object[] { 64, "6483847501EFCDAB5A", DecoderOptions.None, Instruction.Create(Code.Add_rm32_imm8, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x5A) };
				yield return new object[] { 64, "644883847501EFCDAB5A", DecoderOptions.None, Instruction.Create(Code.Add_rm64_imm8, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x5A) };
				yield return new object[] { 64, "644881847501EFCDAB5AA51234", DecoderOptions.None, Instruction.Create(Code.Add_rm64_imm32, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x3412A55A) };
				yield return new object[] { 64, "E65A", DecoderOptions.None, Instruction.Create(Code.Out_imm8_AL, 0x5A, Register.AL) };
				yield return new object[] { 64, "E65A", DecoderOptions.None, Instruction.Create(Code.Out_imm8_AL, 0x5AU, Register.AL) };
				yield return new object[] { 64, "66C85AA5A6", DecoderOptions.None, Instruction.Create(Code.Enterw_imm16_imm8, 0xA55A, 0xA6) };
				yield return new object[] { 64, "66C85AA5A6", DecoderOptions.None, Instruction.Create(Code.Enterw_imm16_imm8, 0xA55AU, 0xA6U) };
				yield return new object[] { 64, "64A2123456789ABCDEF0", DecoderOptions.None, Instruction.Create(Code.Mov_moffs8_AL, new MemoryOperand(Register.None, unchecked((long)0xF0DEBC9A78563412), 8, false, Register.FS), Register.AL) };
				yield return new object[] { 64, "6669CAA55A", DecoderOptions.None, Instruction.Create(Code.Imul_r16_rm16_imm16, Register.CX, Register.DX, 0x5AA5U) };
				yield return new object[] { 64, "69CA5AA51234", DecoderOptions.None, Instruction.Create(Code.Imul_r32_rm32_imm32, Register.ECX, Register.EDX, 0x3412A55A) };
				yield return new object[] { 64, "666BCA5A", DecoderOptions.None, Instruction.Create(Code.Imul_r16_rm16_imm8, Register.CX, Register.DX, 0x5A) };
				yield return new object[] { 64, "6BCA5A", DecoderOptions.None, Instruction.Create(Code.Imul_r32_rm32_imm8, Register.ECX, Register.EDX, 0x5A) };
				yield return new object[] { 64, "486BCA5A", DecoderOptions.None, Instruction.Create(Code.Imul_r64_rm64_imm8, Register.RCX, Register.RDX, 0x5A) };
				yield return new object[] { 64, "4869CA5AA512A4", DecoderOptions.None, Instruction.Create(Code.Imul_r64_rm64_imm32, Register.RCX, Register.RDX, -0x5BED5AA6) };
				yield return new object[] { 64, "6466698C7501EFCDAB5AA5", DecoderOptions.None, Instruction.Create(Code.Imul_r16_rm16_imm16, Register.CX, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0xA55AU) };
				yield return new object[] { 64, "64698C7501EFCDAB5AA51234", DecoderOptions.None, Instruction.Create(Code.Imul_r32_rm32_imm32, Register.ECX, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x3412A55A) };
				yield return new object[] { 64, "64666B8C7501EFCDAB5A", DecoderOptions.None, Instruction.Create(Code.Imul_r16_rm16_imm8, Register.CX, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x5A) };
				yield return new object[] { 64, "646B8C7501EFCDAB5A", DecoderOptions.None, Instruction.Create(Code.Imul_r32_rm32_imm8, Register.ECX, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x5A) };
				yield return new object[] { 64, "64486B8C7501EFCDAB5A", DecoderOptions.None, Instruction.Create(Code.Imul_r64_rm64_imm8, Register.RCX, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x5A) };
				yield return new object[] { 64, "6448698C7501EFCDAB5AA512A4", DecoderOptions.None, Instruction.Create(Code.Imul_r64_rm64_imm32, Register.RCX, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), -0x5BED5AA6) };
				yield return new object[] { 64, "660F78C1A5FD", DecoderOptions.None, Instruction.Create(Code.Extrq_xmm_imm8_imm8, Register.XMM1, 0xA5, 0xFD) };
				yield return new object[] { 64, "660F78C1A5FD", DecoderOptions.None, Instruction.Create(Code.Extrq_xmm_imm8_imm8, Register.XMM1, 0xA5U, 0xFDU) };
				yield return new object[] { 64, "64660FA4947501EFCDAB5A", DecoderOptions.None, Instruction.Create(Code.Shld_rm16_r16_imm8, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), Register.DX, 0x5A) };
				yield return new object[] { 64, "64660FA4947501EFCDAB5A", DecoderOptions.None, Instruction.Create(Code.Shld_rm16_r16_imm8, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), Register.DX, 0x5AU) };
				yield return new object[] { 64, "F20F78CAA5FD", DecoderOptions.None, Instruction.Create(Code.Insertq_xmm_xmm_imm8_imm8, Register.XMM1, Register.XMM2, 0xA5, 0xFD) };
				yield return new object[] { 64, "F20F78CAA5FD", DecoderOptions.None, Instruction.Create(Code.Insertq_xmm_xmm_imm8_imm8, Register.XMM1, Register.XMM2, 0xA5U, 0xFDU) };
				yield return new object[] { 16, "0FB855AA", DecoderOptions.Jmpe, Instruction.CreateBranch(Code.Jmpe_disp16, 0xAA55) };
				yield return new object[] { 32, "0FB8123455AA", DecoderOptions.Jmpe, Instruction.CreateBranch(Code.Jmpe_disp32, 0xAA553412) };
				yield return new object[] { 32, "64676E", DecoderOptions.None, Instruction.CreateOutsb(16, Register.FS) };
				yield return new object[] { 64, "64676E", DecoderOptions.None, Instruction.CreateOutsb(32, Register.FS) };
				yield return new object[] { 64, "646E", DecoderOptions.None, Instruction.CreateOutsb(64, Register.FS) };
				yield return new object[] { 32, "6466676F", DecoderOptions.None, Instruction.CreateOutsw(16, Register.FS) };
				yield return new object[] { 64, "6466676F", DecoderOptions.None, Instruction.CreateOutsw(32, Register.FS) };
				yield return new object[] { 64, "64666F", DecoderOptions.None, Instruction.CreateOutsw(64, Register.FS) };
				yield return new object[] { 32, "64676F", DecoderOptions.None, Instruction.CreateOutsd(16, Register.FS) };
				yield return new object[] { 64, "64676F", DecoderOptions.None, Instruction.CreateOutsd(32, Register.FS) };
				yield return new object[] { 64, "646F", DecoderOptions.None, Instruction.CreateOutsd(64, Register.FS) };
				yield return new object[] { 32, "67AE", DecoderOptions.None, Instruction.CreateScasb(16) };
				yield return new object[] { 64, "67AE", DecoderOptions.None, Instruction.CreateScasb(32) };
				yield return new object[] { 64, "AE", DecoderOptions.None, Instruction.CreateScasb(64) };
				yield return new object[] { 32, "6667AF", DecoderOptions.None, Instruction.CreateScasw(16) };
				yield return new object[] { 64, "6667AF", DecoderOptions.None, Instruction.CreateScasw(32) };
				yield return new object[] { 64, "66AF", DecoderOptions.None, Instruction.CreateScasw(64) };
				yield return new object[] { 32, "67AF", DecoderOptions.None, Instruction.CreateScasd(16) };
				yield return new object[] { 64, "67AF", DecoderOptions.None, Instruction.CreateScasd(32) };
				yield return new object[] { 64, "AF", DecoderOptions.None, Instruction.CreateScasd(64) };
				yield return new object[] { 64, "6748AF", DecoderOptions.None, Instruction.CreateScasq(32) };
				yield return new object[] { 64, "48AF", DecoderOptions.None, Instruction.CreateScasq(64) };
				yield return new object[] { 32, "6467AC", DecoderOptions.None, Instruction.CreateLodsb(16, Register.FS) };
				yield return new object[] { 64, "6467AC", DecoderOptions.None, Instruction.CreateLodsb(32, Register.FS) };
				yield return new object[] { 64, "64AC", DecoderOptions.None, Instruction.CreateLodsb(64, Register.FS) };
				yield return new object[] { 32, "646667AD", DecoderOptions.None, Instruction.CreateLodsw(16, Register.FS) };
				yield return new object[] { 64, "646667AD", DecoderOptions.None, Instruction.CreateLodsw(32, Register.FS) };
				yield return new object[] { 64, "6466AD", DecoderOptions.None, Instruction.CreateLodsw(64, Register.FS) };
				yield return new object[] { 32, "6467AD", DecoderOptions.None, Instruction.CreateLodsd(16, Register.FS) };
				yield return new object[] { 64, "6467AD", DecoderOptions.None, Instruction.CreateLodsd(32, Register.FS) };
				yield return new object[] { 64, "64AD", DecoderOptions.None, Instruction.CreateLodsd(64, Register.FS) };
				yield return new object[] { 64, "646748AD", DecoderOptions.None, Instruction.CreateLodsq(32, Register.FS) };
				yield return new object[] { 64, "6448AD", DecoderOptions.None, Instruction.CreateLodsq(64, Register.FS) };
				yield return new object[] { 32, "676C", DecoderOptions.None, Instruction.CreateInsb(16) };
				yield return new object[] { 64, "676C", DecoderOptions.None, Instruction.CreateInsb(32) };
				yield return new object[] { 64, "6C", DecoderOptions.None, Instruction.CreateInsb(64) };
				yield return new object[] { 32, "66676D", DecoderOptions.None, Instruction.CreateInsw(16) };
				yield return new object[] { 64, "66676D", DecoderOptions.None, Instruction.CreateInsw(32) };
				yield return new object[] { 64, "666D", DecoderOptions.None, Instruction.CreateInsw(64) };
				yield return new object[] { 32, "676D", DecoderOptions.None, Instruction.CreateInsd(16) };
				yield return new object[] { 64, "676D", DecoderOptions.None, Instruction.CreateInsd(32) };
				yield return new object[] { 64, "6D", DecoderOptions.None, Instruction.CreateInsd(64) };
				yield return new object[] { 32, "67AA", DecoderOptions.None, Instruction.CreateStosb(16) };
				yield return new object[] { 64, "67AA", DecoderOptions.None, Instruction.CreateStosb(32) };
				yield return new object[] { 64, "AA", DecoderOptions.None, Instruction.CreateStosb(64) };
				yield return new object[] { 32, "6667AB", DecoderOptions.None, Instruction.CreateStosw(16) };
				yield return new object[] { 64, "6667AB", DecoderOptions.None, Instruction.CreateStosw(32) };
				yield return new object[] { 64, "66AB", DecoderOptions.None, Instruction.CreateStosw(64) };
				yield return new object[] { 32, "67AB", DecoderOptions.None, Instruction.CreateStosd(16) };
				yield return new object[] { 64, "67AB", DecoderOptions.None, Instruction.CreateStosd(32) };
				yield return new object[] { 64, "AB", DecoderOptions.None, Instruction.CreateStosd(64) };
				yield return new object[] { 64, "6748AB", DecoderOptions.None, Instruction.CreateStosq(32) };
				yield return new object[] { 64, "48AB", DecoderOptions.None, Instruction.CreateStosq(64) };
				yield return new object[] { 32, "6467A6", DecoderOptions.None, Instruction.CreateCmpsb(16, Register.FS) };
				yield return new object[] { 64, "6467A6", DecoderOptions.None, Instruction.CreateCmpsb(32, Register.FS) };
				yield return new object[] { 64, "64A6", DecoderOptions.None, Instruction.CreateCmpsb(64, Register.FS) };
				yield return new object[] { 32, "646667A7", DecoderOptions.None, Instruction.CreateCmpsw(16, Register.FS) };
				yield return new object[] { 64, "646667A7", DecoderOptions.None, Instruction.CreateCmpsw(32, Register.FS) };
				yield return new object[] { 64, "6466A7", DecoderOptions.None, Instruction.CreateCmpsw(64, Register.FS) };
				yield return new object[] { 32, "6467A7", DecoderOptions.None, Instruction.CreateCmpsd(16, Register.FS) };
				yield return new object[] { 64, "6467A7", DecoderOptions.None, Instruction.CreateCmpsd(32, Register.FS) };
				yield return new object[] { 64, "64A7", DecoderOptions.None, Instruction.CreateCmpsd(64, Register.FS) };
				yield return new object[] { 64, "646748A7", DecoderOptions.None, Instruction.CreateCmpsq(32, Register.FS) };
				yield return new object[] { 64, "6448A7", DecoderOptions.None, Instruction.CreateCmpsq(64, Register.FS) };
				yield return new object[] { 32, "6467A4", DecoderOptions.None, Instruction.CreateMovsb(16, Register.FS) };
				yield return new object[] { 64, "6467A4", DecoderOptions.None, Instruction.CreateMovsb(32, Register.FS) };
				yield return new object[] { 64, "64A4", DecoderOptions.None, Instruction.CreateMovsb(64, Register.FS) };
				yield return new object[] { 32, "646667A5", DecoderOptions.None, Instruction.CreateMovsw(16, Register.FS) };
				yield return new object[] { 64, "646667A5", DecoderOptions.None, Instruction.CreateMovsw(32, Register.FS) };
				yield return new object[] { 64, "6466A5", DecoderOptions.None, Instruction.CreateMovsw(64, Register.FS) };
				yield return new object[] { 32, "6467A5", DecoderOptions.None, Instruction.CreateMovsd(16, Register.FS) };
				yield return new object[] { 64, "6467A5", DecoderOptions.None, Instruction.CreateMovsd(32, Register.FS) };
				yield return new object[] { 64, "64A5", DecoderOptions.None, Instruction.CreateMovsd(64, Register.FS) };
				yield return new object[] { 64, "646748A5", DecoderOptions.None, Instruction.CreateMovsq(32, Register.FS) };
				yield return new object[] { 64, "6448A5", DecoderOptions.None, Instruction.CreateMovsq(64, Register.FS) };
				yield return new object[] { 32, "64670FF7D3", DecoderOptions.None, Instruction.CreateMaskmovq(16, Register.MM2, Register.MM3, Register.FS) };
				yield return new object[] { 64, "64670FF7D3", DecoderOptions.None, Instruction.CreateMaskmovq(32, Register.MM2, Register.MM3, Register.FS) };
				yield return new object[] { 64, "640FF7D3", DecoderOptions.None, Instruction.CreateMaskmovq(64, Register.MM2, Register.MM3, Register.FS) };
				yield return new object[] { 32, "6467660FF7D3", DecoderOptions.None, Instruction.CreateMaskmovdqu(16, Register.XMM2, Register.XMM3, Register.FS) };
				yield return new object[] { 64, "6467660FF7D3", DecoderOptions.None, Instruction.CreateMaskmovdqu(32, Register.XMM2, Register.XMM3, Register.FS) };
				yield return new object[] { 64, "64660FF7D3", DecoderOptions.None, Instruction.CreateMaskmovdqu(64, Register.XMM2, Register.XMM3, Register.FS) };

				yield return new object[] { 32, "6467F36E", DecoderOptions.None, Instruction.CreateOutsb(16, Register.FS, RepPrefixKind.Repe) };
				yield return new object[] { 64, "6467F36E", DecoderOptions.None, Instruction.CreateOutsb(32, Register.FS, RepPrefixKind.Repe) };
				yield return new object[] { 64, "64F36E", DecoderOptions.None, Instruction.CreateOutsb(64, Register.FS, RepPrefixKind.Repe) };
				yield return new object[] { 32, "646667F36F", DecoderOptions.None, Instruction.CreateOutsw(16, Register.FS, RepPrefixKind.Repe) };
				yield return new object[] { 64, "646667F36F", DecoderOptions.None, Instruction.CreateOutsw(32, Register.FS, RepPrefixKind.Repe) };
				yield return new object[] { 64, "6466F36F", DecoderOptions.None, Instruction.CreateOutsw(64, Register.FS, RepPrefixKind.Repe) };
				yield return new object[] { 32, "6467F36F", DecoderOptions.None, Instruction.CreateOutsd(16, Register.FS, RepPrefixKind.Repe) };
				yield return new object[] { 64, "6467F36F", DecoderOptions.None, Instruction.CreateOutsd(32, Register.FS, RepPrefixKind.Repe) };
				yield return new object[] { 64, "64F36F", DecoderOptions.None, Instruction.CreateOutsd(64, Register.FS, RepPrefixKind.Repe) };
				yield return new object[] { 32, "67F3AE", DecoderOptions.None, Instruction.CreateScasb(16, RepPrefixKind.Repe) };
				yield return new object[] { 64, "67F3AE", DecoderOptions.None, Instruction.CreateScasb(32, RepPrefixKind.Repe) };
				yield return new object[] { 64, "F3AE", DecoderOptions.None, Instruction.CreateScasb(64, RepPrefixKind.Repe) };
				yield return new object[] { 32, "6667F3AF", DecoderOptions.None, Instruction.CreateScasw(16, RepPrefixKind.Repe) };
				yield return new object[] { 64, "6667F3AF", DecoderOptions.None, Instruction.CreateScasw(32, RepPrefixKind.Repe) };
				yield return new object[] { 64, "66F3AF", DecoderOptions.None, Instruction.CreateScasw(64, RepPrefixKind.Repe) };
				yield return new object[] { 32, "67F3AF", DecoderOptions.None, Instruction.CreateScasd(16, RepPrefixKind.Repe) };
				yield return new object[] { 64, "67F3AF", DecoderOptions.None, Instruction.CreateScasd(32, RepPrefixKind.Repe) };
				yield return new object[] { 64, "F3AF", DecoderOptions.None, Instruction.CreateScasd(64, RepPrefixKind.Repe) };
				yield return new object[] { 64, "67F348AF", DecoderOptions.None, Instruction.CreateScasq(32, RepPrefixKind.Repe) };
				yield return new object[] { 64, "F348AF", DecoderOptions.None, Instruction.CreateScasq(64, RepPrefixKind.Repe) };
				yield return new object[] { 32, "6467F3AC", DecoderOptions.None, Instruction.CreateLodsb(16, Register.FS, RepPrefixKind.Repe) };
				yield return new object[] { 64, "6467F3AC", DecoderOptions.None, Instruction.CreateLodsb(32, Register.FS, RepPrefixKind.Repe) };
				yield return new object[] { 64, "64F3AC", DecoderOptions.None, Instruction.CreateLodsb(64, Register.FS, RepPrefixKind.Repe) };
				yield return new object[] { 32, "646667F3AD", DecoderOptions.None, Instruction.CreateLodsw(16, Register.FS, RepPrefixKind.Repe) };
				yield return new object[] { 64, "646667F3AD", DecoderOptions.None, Instruction.CreateLodsw(32, Register.FS, RepPrefixKind.Repe) };
				yield return new object[] { 64, "6466F3AD", DecoderOptions.None, Instruction.CreateLodsw(64, Register.FS, RepPrefixKind.Repe) };
				yield return new object[] { 32, "6467F3AD", DecoderOptions.None, Instruction.CreateLodsd(16, Register.FS, RepPrefixKind.Repe) };
				yield return new object[] { 64, "6467F3AD", DecoderOptions.None, Instruction.CreateLodsd(32, Register.FS, RepPrefixKind.Repe) };
				yield return new object[] { 64, "64F3AD", DecoderOptions.None, Instruction.CreateLodsd(64, Register.FS, RepPrefixKind.Repe) };
				yield return new object[] { 64, "6467F348AD", DecoderOptions.None, Instruction.CreateLodsq(32, Register.FS, RepPrefixKind.Repe) };
				yield return new object[] { 64, "64F348AD", DecoderOptions.None, Instruction.CreateLodsq(64, Register.FS, RepPrefixKind.Repe) };
				yield return new object[] { 32, "67F36C", DecoderOptions.None, Instruction.CreateInsb(16, RepPrefixKind.Repe) };
				yield return new object[] { 64, "67F36C", DecoderOptions.None, Instruction.CreateInsb(32, RepPrefixKind.Repe) };
				yield return new object[] { 64, "F36C", DecoderOptions.None, Instruction.CreateInsb(64, RepPrefixKind.Repe) };
				yield return new object[] { 32, "6667F36D", DecoderOptions.None, Instruction.CreateInsw(16, RepPrefixKind.Repe) };
				yield return new object[] { 64, "6667F36D", DecoderOptions.None, Instruction.CreateInsw(32, RepPrefixKind.Repe) };
				yield return new object[] { 64, "66F36D", DecoderOptions.None, Instruction.CreateInsw(64, RepPrefixKind.Repe) };
				yield return new object[] { 32, "67F36D", DecoderOptions.None, Instruction.CreateInsd(16, RepPrefixKind.Repe) };
				yield return new object[] { 64, "67F36D", DecoderOptions.None, Instruction.CreateInsd(32, RepPrefixKind.Repe) };
				yield return new object[] { 64, "F36D", DecoderOptions.None, Instruction.CreateInsd(64, RepPrefixKind.Repe) };
				yield return new object[] { 32, "67F3AA", DecoderOptions.None, Instruction.CreateStosb(16, RepPrefixKind.Repe) };
				yield return new object[] { 64, "67F3AA", DecoderOptions.None, Instruction.CreateStosb(32, RepPrefixKind.Repe) };
				yield return new object[] { 64, "F3AA", DecoderOptions.None, Instruction.CreateStosb(64, RepPrefixKind.Repe) };
				yield return new object[] { 32, "6667F3AB", DecoderOptions.None, Instruction.CreateStosw(16, RepPrefixKind.Repe) };
				yield return new object[] { 64, "6667F3AB", DecoderOptions.None, Instruction.CreateStosw(32, RepPrefixKind.Repe) };
				yield return new object[] { 64, "66F3AB", DecoderOptions.None, Instruction.CreateStosw(64, RepPrefixKind.Repe) };
				yield return new object[] { 32, "67F3AB", DecoderOptions.None, Instruction.CreateStosd(16, RepPrefixKind.Repe) };
				yield return new object[] { 64, "67F3AB", DecoderOptions.None, Instruction.CreateStosd(32, RepPrefixKind.Repe) };
				yield return new object[] { 64, "F3AB", DecoderOptions.None, Instruction.CreateStosd(64, RepPrefixKind.Repe) };
				yield return new object[] { 64, "67F348AB", DecoderOptions.None, Instruction.CreateStosq(32, RepPrefixKind.Repe) };
				yield return new object[] { 64, "F348AB", DecoderOptions.None, Instruction.CreateStosq(64, RepPrefixKind.Repe) };
				yield return new object[] { 32, "6467F3A6", DecoderOptions.None, Instruction.CreateCmpsb(16, Register.FS, RepPrefixKind.Repe) };
				yield return new object[] { 64, "6467F3A6", DecoderOptions.None, Instruction.CreateCmpsb(32, Register.FS, RepPrefixKind.Repe) };
				yield return new object[] { 64, "64F3A6", DecoderOptions.None, Instruction.CreateCmpsb(64, Register.FS, RepPrefixKind.Repe) };
				yield return new object[] { 32, "646667F3A7", DecoderOptions.None, Instruction.CreateCmpsw(16, Register.FS, RepPrefixKind.Repe) };
				yield return new object[] { 64, "646667F3A7", DecoderOptions.None, Instruction.CreateCmpsw(32, Register.FS, RepPrefixKind.Repe) };
				yield return new object[] { 64, "6466F3A7", DecoderOptions.None, Instruction.CreateCmpsw(64, Register.FS, RepPrefixKind.Repe) };
				yield return new object[] { 32, "6467F3A7", DecoderOptions.None, Instruction.CreateCmpsd(16, Register.FS, RepPrefixKind.Repe) };
				yield return new object[] { 64, "6467F3A7", DecoderOptions.None, Instruction.CreateCmpsd(32, Register.FS, RepPrefixKind.Repe) };
				yield return new object[] { 64, "64F3A7", DecoderOptions.None, Instruction.CreateCmpsd(64, Register.FS, RepPrefixKind.Repe) };
				yield return new object[] { 64, "6467F348A7", DecoderOptions.None, Instruction.CreateCmpsq(32, Register.FS, RepPrefixKind.Repe) };
				yield return new object[] { 64, "64F348A7", DecoderOptions.None, Instruction.CreateCmpsq(64, Register.FS, RepPrefixKind.Repe) };
				yield return new object[] { 32, "6467F3A4", DecoderOptions.None, Instruction.CreateMovsb(16, Register.FS, RepPrefixKind.Repe) };
				yield return new object[] { 64, "6467F3A4", DecoderOptions.None, Instruction.CreateMovsb(32, Register.FS, RepPrefixKind.Repe) };
				yield return new object[] { 64, "64F3A4", DecoderOptions.None, Instruction.CreateMovsb(64, Register.FS, RepPrefixKind.Repe) };
				yield return new object[] { 32, "646667F3A5", DecoderOptions.None, Instruction.CreateMovsw(16, Register.FS, RepPrefixKind.Repe) };
				yield return new object[] { 64, "646667F3A5", DecoderOptions.None, Instruction.CreateMovsw(32, Register.FS, RepPrefixKind.Repe) };
				yield return new object[] { 64, "6466F3A5", DecoderOptions.None, Instruction.CreateMovsw(64, Register.FS, RepPrefixKind.Repe) };
				yield return new object[] { 32, "6467F3A5", DecoderOptions.None, Instruction.CreateMovsd(16, Register.FS, RepPrefixKind.Repe) };
				yield return new object[] { 64, "6467F3A5", DecoderOptions.None, Instruction.CreateMovsd(32, Register.FS, RepPrefixKind.Repe) };
				yield return new object[] { 64, "64F3A5", DecoderOptions.None, Instruction.CreateMovsd(64, Register.FS, RepPrefixKind.Repe) };
				yield return new object[] { 64, "6467F348A5", DecoderOptions.None, Instruction.CreateMovsq(32, Register.FS, RepPrefixKind.Repe) };
				yield return new object[] { 64, "64F348A5", DecoderOptions.None, Instruction.CreateMovsq(64, Register.FS, RepPrefixKind.Repe) };

				yield return new object[] { 32, "6467F26E", DecoderOptions.None, Instruction.CreateOutsb(16, Register.FS, RepPrefixKind.Repne) };
				yield return new object[] { 64, "6467F26E", DecoderOptions.None, Instruction.CreateOutsb(32, Register.FS, RepPrefixKind.Repne) };
				yield return new object[] { 64, "64F26E", DecoderOptions.None, Instruction.CreateOutsb(64, Register.FS, RepPrefixKind.Repne) };
				yield return new object[] { 32, "646667F26F", DecoderOptions.None, Instruction.CreateOutsw(16, Register.FS, RepPrefixKind.Repne) };
				yield return new object[] { 64, "646667F26F", DecoderOptions.None, Instruction.CreateOutsw(32, Register.FS, RepPrefixKind.Repne) };
				yield return new object[] { 64, "6466F26F", DecoderOptions.None, Instruction.CreateOutsw(64, Register.FS, RepPrefixKind.Repne) };
				yield return new object[] { 32, "6467F26F", DecoderOptions.None, Instruction.CreateOutsd(16, Register.FS, RepPrefixKind.Repne) };
				yield return new object[] { 64, "6467F26F", DecoderOptions.None, Instruction.CreateOutsd(32, Register.FS, RepPrefixKind.Repne) };
				yield return new object[] { 64, "64F26F", DecoderOptions.None, Instruction.CreateOutsd(64, Register.FS, RepPrefixKind.Repne) };
				yield return new object[] { 32, "67F2AE", DecoderOptions.None, Instruction.CreateScasb(16, RepPrefixKind.Repne) };
				yield return new object[] { 64, "67F2AE", DecoderOptions.None, Instruction.CreateScasb(32, RepPrefixKind.Repne) };
				yield return new object[] { 64, "F2AE", DecoderOptions.None, Instruction.CreateScasb(64, RepPrefixKind.Repne) };
				yield return new object[] { 32, "6667F2AF", DecoderOptions.None, Instruction.CreateScasw(16, RepPrefixKind.Repne) };
				yield return new object[] { 64, "6667F2AF", DecoderOptions.None, Instruction.CreateScasw(32, RepPrefixKind.Repne) };
				yield return new object[] { 64, "66F2AF", DecoderOptions.None, Instruction.CreateScasw(64, RepPrefixKind.Repne) };
				yield return new object[] { 32, "67F2AF", DecoderOptions.None, Instruction.CreateScasd(16, RepPrefixKind.Repne) };
				yield return new object[] { 64, "67F2AF", DecoderOptions.None, Instruction.CreateScasd(32, RepPrefixKind.Repne) };
				yield return new object[] { 64, "F2AF", DecoderOptions.None, Instruction.CreateScasd(64, RepPrefixKind.Repne) };
				yield return new object[] { 64, "67F248AF", DecoderOptions.None, Instruction.CreateScasq(32, RepPrefixKind.Repne) };
				yield return new object[] { 64, "F248AF", DecoderOptions.None, Instruction.CreateScasq(64, RepPrefixKind.Repne) };
				yield return new object[] { 32, "6467F2AC", DecoderOptions.None, Instruction.CreateLodsb(16, Register.FS, RepPrefixKind.Repne) };
				yield return new object[] { 64, "6467F2AC", DecoderOptions.None, Instruction.CreateLodsb(32, Register.FS, RepPrefixKind.Repne) };
				yield return new object[] { 64, "64F2AC", DecoderOptions.None, Instruction.CreateLodsb(64, Register.FS, RepPrefixKind.Repne) };
				yield return new object[] { 32, "646667F2AD", DecoderOptions.None, Instruction.CreateLodsw(16, Register.FS, RepPrefixKind.Repne) };
				yield return new object[] { 64, "646667F2AD", DecoderOptions.None, Instruction.CreateLodsw(32, Register.FS, RepPrefixKind.Repne) };
				yield return new object[] { 64, "6466F2AD", DecoderOptions.None, Instruction.CreateLodsw(64, Register.FS, RepPrefixKind.Repne) };
				yield return new object[] { 32, "6467F2AD", DecoderOptions.None, Instruction.CreateLodsd(16, Register.FS, RepPrefixKind.Repne) };
				yield return new object[] { 64, "6467F2AD", DecoderOptions.None, Instruction.CreateLodsd(32, Register.FS, RepPrefixKind.Repne) };
				yield return new object[] { 64, "64F2AD", DecoderOptions.None, Instruction.CreateLodsd(64, Register.FS, RepPrefixKind.Repne) };
				yield return new object[] { 64, "6467F248AD", DecoderOptions.None, Instruction.CreateLodsq(32, Register.FS, RepPrefixKind.Repne) };
				yield return new object[] { 64, "64F248AD", DecoderOptions.None, Instruction.CreateLodsq(64, Register.FS, RepPrefixKind.Repne) };
				yield return new object[] { 32, "67F26C", DecoderOptions.None, Instruction.CreateInsb(16, RepPrefixKind.Repne) };
				yield return new object[] { 64, "67F26C", DecoderOptions.None, Instruction.CreateInsb(32, RepPrefixKind.Repne) };
				yield return new object[] { 64, "F26C", DecoderOptions.None, Instruction.CreateInsb(64, RepPrefixKind.Repne) };
				yield return new object[] { 32, "6667F26D", DecoderOptions.None, Instruction.CreateInsw(16, RepPrefixKind.Repne) };
				yield return new object[] { 64, "6667F26D", DecoderOptions.None, Instruction.CreateInsw(32, RepPrefixKind.Repne) };
				yield return new object[] { 64, "66F26D", DecoderOptions.None, Instruction.CreateInsw(64, RepPrefixKind.Repne) };
				yield return new object[] { 32, "67F26D", DecoderOptions.None, Instruction.CreateInsd(16, RepPrefixKind.Repne) };
				yield return new object[] { 64, "67F26D", DecoderOptions.None, Instruction.CreateInsd(32, RepPrefixKind.Repne) };
				yield return new object[] { 64, "F26D", DecoderOptions.None, Instruction.CreateInsd(64, RepPrefixKind.Repne) };
				yield return new object[] { 32, "67F2AA", DecoderOptions.None, Instruction.CreateStosb(16, RepPrefixKind.Repne) };
				yield return new object[] { 64, "67F2AA", DecoderOptions.None, Instruction.CreateStosb(32, RepPrefixKind.Repne) };
				yield return new object[] { 64, "F2AA", DecoderOptions.None, Instruction.CreateStosb(64, RepPrefixKind.Repne) };
				yield return new object[] { 32, "6667F2AB", DecoderOptions.None, Instruction.CreateStosw(16, RepPrefixKind.Repne) };
				yield return new object[] { 64, "6667F2AB", DecoderOptions.None, Instruction.CreateStosw(32, RepPrefixKind.Repne) };
				yield return new object[] { 64, "66F2AB", DecoderOptions.None, Instruction.CreateStosw(64, RepPrefixKind.Repne) };
				yield return new object[] { 32, "67F2AB", DecoderOptions.None, Instruction.CreateStosd(16, RepPrefixKind.Repne) };
				yield return new object[] { 64, "67F2AB", DecoderOptions.None, Instruction.CreateStosd(32, RepPrefixKind.Repne) };
				yield return new object[] { 64, "F2AB", DecoderOptions.None, Instruction.CreateStosd(64, RepPrefixKind.Repne) };
				yield return new object[] { 64, "67F248AB", DecoderOptions.None, Instruction.CreateStosq(32, RepPrefixKind.Repne) };
				yield return new object[] { 64, "F248AB", DecoderOptions.None, Instruction.CreateStosq(64, RepPrefixKind.Repne) };
				yield return new object[] { 32, "6467F2A6", DecoderOptions.None, Instruction.CreateCmpsb(16, Register.FS, RepPrefixKind.Repne) };
				yield return new object[] { 64, "6467F2A6", DecoderOptions.None, Instruction.CreateCmpsb(32, Register.FS, RepPrefixKind.Repne) };
				yield return new object[] { 64, "64F2A6", DecoderOptions.None, Instruction.CreateCmpsb(64, Register.FS, RepPrefixKind.Repne) };
				yield return new object[] { 32, "646667F2A7", DecoderOptions.None, Instruction.CreateCmpsw(16, Register.FS, RepPrefixKind.Repne) };
				yield return new object[] { 64, "646667F2A7", DecoderOptions.None, Instruction.CreateCmpsw(32, Register.FS, RepPrefixKind.Repne) };
				yield return new object[] { 64, "6466F2A7", DecoderOptions.None, Instruction.CreateCmpsw(64, Register.FS, RepPrefixKind.Repne) };
				yield return new object[] { 32, "6467F2A7", DecoderOptions.None, Instruction.CreateCmpsd(16, Register.FS, RepPrefixKind.Repne) };
				yield return new object[] { 64, "6467F2A7", DecoderOptions.None, Instruction.CreateCmpsd(32, Register.FS, RepPrefixKind.Repne) };
				yield return new object[] { 64, "64F2A7", DecoderOptions.None, Instruction.CreateCmpsd(64, Register.FS, RepPrefixKind.Repne) };
				yield return new object[] { 64, "6467F248A7", DecoderOptions.None, Instruction.CreateCmpsq(32, Register.FS, RepPrefixKind.Repne) };
				yield return new object[] { 64, "64F248A7", DecoderOptions.None, Instruction.CreateCmpsq(64, Register.FS, RepPrefixKind.Repne) };
				yield return new object[] { 32, "6467F2A4", DecoderOptions.None, Instruction.CreateMovsb(16, Register.FS, RepPrefixKind.Repne) };
				yield return new object[] { 64, "6467F2A4", DecoderOptions.None, Instruction.CreateMovsb(32, Register.FS, RepPrefixKind.Repne) };
				yield return new object[] { 64, "64F2A4", DecoderOptions.None, Instruction.CreateMovsb(64, Register.FS, RepPrefixKind.Repne) };
				yield return new object[] { 32, "646667F2A5", DecoderOptions.None, Instruction.CreateMovsw(16, Register.FS, RepPrefixKind.Repne) };
				yield return new object[] { 64, "646667F2A5", DecoderOptions.None, Instruction.CreateMovsw(32, Register.FS, RepPrefixKind.Repne) };
				yield return new object[] { 64, "6466F2A5", DecoderOptions.None, Instruction.CreateMovsw(64, Register.FS, RepPrefixKind.Repne) };
				yield return new object[] { 32, "6467F2A5", DecoderOptions.None, Instruction.CreateMovsd(16, Register.FS, RepPrefixKind.Repne) };
				yield return new object[] { 64, "6467F2A5", DecoderOptions.None, Instruction.CreateMovsd(32, Register.FS, RepPrefixKind.Repne) };
				yield return new object[] { 64, "64F2A5", DecoderOptions.None, Instruction.CreateMovsd(64, Register.FS, RepPrefixKind.Repne) };
				yield return new object[] { 64, "6467F248A5", DecoderOptions.None, Instruction.CreateMovsq(32, Register.FS, RepPrefixKind.Repne) };
				yield return new object[] { 64, "64F248A5", DecoderOptions.None, Instruction.CreateMovsq(64, Register.FS, RepPrefixKind.Repne) };

				yield return new object[] { 32, "67F36E", DecoderOptions.None, Instruction.CreateRepOutsb(16) };
				yield return new object[] { 64, "67F36E", DecoderOptions.None, Instruction.CreateRepOutsb(32) };
				yield return new object[] { 64, "F36E", DecoderOptions.None, Instruction.CreateRepOutsb(64) };
				yield return new object[] { 32, "6667F36F", DecoderOptions.None, Instruction.CreateRepOutsw(16) };
				yield return new object[] { 64, "6667F36F", DecoderOptions.None, Instruction.CreateRepOutsw(32) };
				yield return new object[] { 64, "66F36F", DecoderOptions.None, Instruction.CreateRepOutsw(64) };
				yield return new object[] { 32, "67F36F", DecoderOptions.None, Instruction.CreateRepOutsd(16) };
				yield return new object[] { 64, "67F36F", DecoderOptions.None, Instruction.CreateRepOutsd(32) };
				yield return new object[] { 64, "F36F", DecoderOptions.None, Instruction.CreateRepOutsd(64) };
				yield return new object[] { 32, "67F3AE", DecoderOptions.None, Instruction.CreateRepeScasb(16) };
				yield return new object[] { 64, "67F3AE", DecoderOptions.None, Instruction.CreateRepeScasb(32) };
				yield return new object[] { 64, "F3AE", DecoderOptions.None, Instruction.CreateRepeScasb(64) };
				yield return new object[] { 32, "6667F3AF", DecoderOptions.None, Instruction.CreateRepeScasw(16) };
				yield return new object[] { 64, "6667F3AF", DecoderOptions.None, Instruction.CreateRepeScasw(32) };
				yield return new object[] { 64, "66F3AF", DecoderOptions.None, Instruction.CreateRepeScasw(64) };
				yield return new object[] { 32, "67F3AF", DecoderOptions.None, Instruction.CreateRepeScasd(16) };
				yield return new object[] { 64, "67F3AF", DecoderOptions.None, Instruction.CreateRepeScasd(32) };
				yield return new object[] { 64, "F3AF", DecoderOptions.None, Instruction.CreateRepeScasd(64) };
				yield return new object[] { 64, "67F348AF", DecoderOptions.None, Instruction.CreateRepeScasq(32) };
				yield return new object[] { 64, "F348AF", DecoderOptions.None, Instruction.CreateRepeScasq(64) };
				yield return new object[] { 32, "67F2AE", DecoderOptions.None, Instruction.CreateRepneScasb(16) };
				yield return new object[] { 64, "67F2AE", DecoderOptions.None, Instruction.CreateRepneScasb(32) };
				yield return new object[] { 64, "F2AE", DecoderOptions.None, Instruction.CreateRepneScasb(64) };
				yield return new object[] { 32, "6667F2AF", DecoderOptions.None, Instruction.CreateRepneScasw(16) };
				yield return new object[] { 64, "6667F2AF", DecoderOptions.None, Instruction.CreateRepneScasw(32) };
				yield return new object[] { 64, "66F2AF", DecoderOptions.None, Instruction.CreateRepneScasw(64) };
				yield return new object[] { 32, "67F2AF", DecoderOptions.None, Instruction.CreateRepneScasd(16) };
				yield return new object[] { 64, "67F2AF", DecoderOptions.None, Instruction.CreateRepneScasd(32) };
				yield return new object[] { 64, "F2AF", DecoderOptions.None, Instruction.CreateRepneScasd(64) };
				yield return new object[] { 64, "67F248AF", DecoderOptions.None, Instruction.CreateRepneScasq(32) };
				yield return new object[] { 64, "F248AF", DecoderOptions.None, Instruction.CreateRepneScasq(64) };
				yield return new object[] { 32, "67F3AC", DecoderOptions.None, Instruction.CreateRepLodsb(16) };
				yield return new object[] { 64, "67F3AC", DecoderOptions.None, Instruction.CreateRepLodsb(32) };
				yield return new object[] { 64, "F3AC", DecoderOptions.None, Instruction.CreateRepLodsb(64) };
				yield return new object[] { 32, "6667F3AD", DecoderOptions.None, Instruction.CreateRepLodsw(16) };
				yield return new object[] { 64, "6667F3AD", DecoderOptions.None, Instruction.CreateRepLodsw(32) };
				yield return new object[] { 64, "66F3AD", DecoderOptions.None, Instruction.CreateRepLodsw(64) };
				yield return new object[] { 32, "67F3AD", DecoderOptions.None, Instruction.CreateRepLodsd(16) };
				yield return new object[] { 64, "67F3AD", DecoderOptions.None, Instruction.CreateRepLodsd(32) };
				yield return new object[] { 64, "F3AD", DecoderOptions.None, Instruction.CreateRepLodsd(64) };
				yield return new object[] { 64, "67F348AD", DecoderOptions.None, Instruction.CreateRepLodsq(32) };
				yield return new object[] { 64, "F348AD", DecoderOptions.None, Instruction.CreateRepLodsq(64) };
				yield return new object[] { 32, "67F36C", DecoderOptions.None, Instruction.CreateRepInsb(16) };
				yield return new object[] { 64, "67F36C", DecoderOptions.None, Instruction.CreateRepInsb(32) };
				yield return new object[] { 64, "F36C", DecoderOptions.None, Instruction.CreateRepInsb(64) };
				yield return new object[] { 32, "6667F36D", DecoderOptions.None, Instruction.CreateRepInsw(16) };
				yield return new object[] { 64, "6667F36D", DecoderOptions.None, Instruction.CreateRepInsw(32) };
				yield return new object[] { 64, "66F36D", DecoderOptions.None, Instruction.CreateRepInsw(64) };
				yield return new object[] { 32, "67F36D", DecoderOptions.None, Instruction.CreateRepInsd(16) };
				yield return new object[] { 64, "67F36D", DecoderOptions.None, Instruction.CreateRepInsd(32) };
				yield return new object[] { 64, "F36D", DecoderOptions.None, Instruction.CreateRepInsd(64) };
				yield return new object[] { 32, "67F3AA", DecoderOptions.None, Instruction.CreateRepStosb(16) };
				yield return new object[] { 64, "67F3AA", DecoderOptions.None, Instruction.CreateRepStosb(32) };
				yield return new object[] { 64, "F3AA", DecoderOptions.None, Instruction.CreateRepStosb(64) };
				yield return new object[] { 32, "6667F3AB", DecoderOptions.None, Instruction.CreateRepStosw(16) };
				yield return new object[] { 64, "6667F3AB", DecoderOptions.None, Instruction.CreateRepStosw(32) };
				yield return new object[] { 64, "66F3AB", DecoderOptions.None, Instruction.CreateRepStosw(64) };
				yield return new object[] { 32, "67F3AB", DecoderOptions.None, Instruction.CreateRepStosd(16) };
				yield return new object[] { 64, "67F3AB", DecoderOptions.None, Instruction.CreateRepStosd(32) };
				yield return new object[] { 64, "F3AB", DecoderOptions.None, Instruction.CreateRepStosd(64) };
				yield return new object[] { 64, "67F348AB", DecoderOptions.None, Instruction.CreateRepStosq(32) };
				yield return new object[] { 64, "F348AB", DecoderOptions.None, Instruction.CreateRepStosq(64) };
				yield return new object[] { 32, "67F3A6", DecoderOptions.None, Instruction.CreateRepeCmpsb(16) };
				yield return new object[] { 64, "67F3A6", DecoderOptions.None, Instruction.CreateRepeCmpsb(32) };
				yield return new object[] { 64, "F3A6", DecoderOptions.None, Instruction.CreateRepeCmpsb(64) };
				yield return new object[] { 32, "6667F3A7", DecoderOptions.None, Instruction.CreateRepeCmpsw(16) };
				yield return new object[] { 64, "6667F3A7", DecoderOptions.None, Instruction.CreateRepeCmpsw(32) };
				yield return new object[] { 64, "66F3A7", DecoderOptions.None, Instruction.CreateRepeCmpsw(64) };
				yield return new object[] { 32, "67F3A7", DecoderOptions.None, Instruction.CreateRepeCmpsd(16) };
				yield return new object[] { 64, "67F3A7", DecoderOptions.None, Instruction.CreateRepeCmpsd(32) };
				yield return new object[] { 64, "F3A7", DecoderOptions.None, Instruction.CreateRepeCmpsd(64) };
				yield return new object[] { 64, "67F348A7", DecoderOptions.None, Instruction.CreateRepeCmpsq(32) };
				yield return new object[] { 64, "F348A7", DecoderOptions.None, Instruction.CreateRepeCmpsq(64) };
				yield return new object[] { 32, "67F2A6", DecoderOptions.None, Instruction.CreateRepneCmpsb(16) };
				yield return new object[] { 64, "67F2A6", DecoderOptions.None, Instruction.CreateRepneCmpsb(32) };
				yield return new object[] { 64, "F2A6", DecoderOptions.None, Instruction.CreateRepneCmpsb(64) };
				yield return new object[] { 32, "6667F2A7", DecoderOptions.None, Instruction.CreateRepneCmpsw(16) };
				yield return new object[] { 64, "6667F2A7", DecoderOptions.None, Instruction.CreateRepneCmpsw(32) };
				yield return new object[] { 64, "66F2A7", DecoderOptions.None, Instruction.CreateRepneCmpsw(64) };
				yield return new object[] { 32, "67F2A7", DecoderOptions.None, Instruction.CreateRepneCmpsd(16) };
				yield return new object[] { 64, "67F2A7", DecoderOptions.None, Instruction.CreateRepneCmpsd(32) };
				yield return new object[] { 64, "F2A7", DecoderOptions.None, Instruction.CreateRepneCmpsd(64) };
				yield return new object[] { 64, "67F248A7", DecoderOptions.None, Instruction.CreateRepneCmpsq(32) };
				yield return new object[] { 64, "F248A7", DecoderOptions.None, Instruction.CreateRepneCmpsq(64) };
				yield return new object[] { 32, "67F3A4", DecoderOptions.None, Instruction.CreateRepMovsb(16) };
				yield return new object[] { 64, "67F3A4", DecoderOptions.None, Instruction.CreateRepMovsb(32) };
				yield return new object[] { 64, "F3A4", DecoderOptions.None, Instruction.CreateRepMovsb(64) };
				yield return new object[] { 32, "6667F3A5", DecoderOptions.None, Instruction.CreateRepMovsw(16) };
				yield return new object[] { 64, "6667F3A5", DecoderOptions.None, Instruction.CreateRepMovsw(32) };
				yield return new object[] { 64, "66F3A5", DecoderOptions.None, Instruction.CreateRepMovsw(64) };
				yield return new object[] { 32, "67F3A5", DecoderOptions.None, Instruction.CreateRepMovsd(16) };
				yield return new object[] { 64, "67F3A5", DecoderOptions.None, Instruction.CreateRepMovsd(32) };
				yield return new object[] { 64, "F3A5", DecoderOptions.None, Instruction.CreateRepMovsd(64) };
				yield return new object[] { 64, "67F348A5", DecoderOptions.None, Instruction.CreateRepMovsq(32) };
				yield return new object[] { 64, "F348A5", DecoderOptions.None, Instruction.CreateRepMovsq(64) };
			}
		}
		public static IEnumerable<object[]> CreateTestVEX_Data {
			get {
#if !NO_VEX
				yield return new object[] { 64, "C5E814CB", DecoderOptions.None, Instruction.Create(Code.VEX_Vunpcklps_xmm_xmm_xmmm128, Register.XMM1, Register.XMM2, Register.XMM3) };
				yield return new object[] { 64, "64C5E8148C7501EFCDAB", DecoderOptions.None, Instruction.Create(Code.VEX_Vunpcklps_xmm_xmm_xmmm128, Register.XMM1, Register.XMM2, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS)) };
				yield return new object[] { 64, "64C4E261908C7501EFCDAB", DecoderOptions.None, Instruction.Create(Code.VEX_Vpgatherdd_xmm_vm32x_xmm, Register.XMM1, new MemoryOperand(Register.RBP, Register.XMM6, 2, -0x543210FF, 8, false, Register.FS), Register.XMM3) };
				yield return new object[] { 64, "64C4E2692E9C7501EFCDAB", DecoderOptions.None, Instruction.Create(Code.VEX_Vmaskmovps_m128_xmm_xmm, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), Register.XMM2, Register.XMM3) };
				yield return new object[] { 64, "C4E3694ACB40", DecoderOptions.None, Instruction.Create(Code.VEX_Vblendvps_xmm_xmm_xmmm128_xmm, Register.XMM1, Register.XMM2, Register.XMM3, Register.XMM4) };
				yield return new object[] { 64, "64C4E3E95C8C7501EFCDAB30", DecoderOptions.None, Instruction.Create(Code.VEX_Vfmaddsubps_xmm_xmm_xmm_xmmm128, Register.XMM1, Register.XMM2, Register.XMM3, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS)) };
				yield return new object[] { 64, "64C4E3694A8C7501EFCDAB40", DecoderOptions.None, Instruction.Create(Code.VEX_Vblendvps_xmm_xmm_xmmm128_xmm, Register.XMM1, Register.XMM2, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), Register.XMM4) };
				yield return new object[] { 64, "C4E36948CB40", DecoderOptions.None, Instruction.Create(Code.VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm4, Register.XMM1, Register.XMM2, Register.XMM3, Register.XMM4, 0x0) };
				yield return new object[] { 64, "C4E36948CB40", DecoderOptions.None, Instruction.Create(Code.VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm4, Register.XMM1, Register.XMM2, Register.XMM3, Register.XMM4, 0x0U) };
				yield return new object[] { 64, "64C4E3E9488C7501EFCDAB31", DecoderOptions.None, Instruction.Create(Code.VEX_Vpermil2ps_xmm_xmm_xmm_xmmm128_imm4, Register.XMM1, Register.XMM2, Register.XMM3, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x1) };
				yield return new object[] { 64, "64C4E3E9488C7501EFCDAB31", DecoderOptions.None, Instruction.Create(Code.VEX_Vpermil2ps_xmm_xmm_xmm_xmmm128_imm4, Register.XMM1, Register.XMM2, Register.XMM3, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0x1U) };
				yield return new object[] { 64, "64C4E369488C7501EFCDAB41", DecoderOptions.None, Instruction.Create(Code.VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm4, Register.XMM1, Register.XMM2, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), Register.XMM4, 0x1) };
				yield return new object[] { 64, "64C4E369488C7501EFCDAB41", DecoderOptions.None, Instruction.Create(Code.VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm4, Register.XMM1, Register.XMM2, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), Register.XMM4, 0x1U) };
				yield return new object[] { 32, "6467C5F9F7D3", DecoderOptions.None, Instruction.CreateVmaskmovdqu(16, Register.XMM2, Register.XMM3, Register.FS) };
				yield return new object[] { 64, "6467C5F9F7D3", DecoderOptions.None, Instruction.CreateVmaskmovdqu(32, Register.XMM2, Register.XMM3, Register.FS) };
				yield return new object[] { 64, "64C5F9F7D3", DecoderOptions.None, Instruction.CreateVmaskmovdqu(64, Register.XMM2, Register.XMM3, Register.FS) };
#endif
				yield break;
			}
		}
		public static IEnumerable<object[]> CreateTestEVEX_Data {
			get {
#if !NO_EVEX
				yield return new object[] { 64, "62F1F50873D2A5", DecoderOptions.None, Instruction.Create(Code.EVEX_Vpsrlq_xmm_k1z_xmmm128b64_imm8, Register.XMM1, Register.XMM2, 0xA5) };
				yield return new object[] { 64, "6462F1F50873947501EFCDABA5", DecoderOptions.None, Instruction.Create(Code.EVEX_Vpsrlq_xmm_k1z_xmmm128b64_imm8, Register.XMM1, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0xA5) };
				yield return new object[] { 64, "62F16D08C4CBA5", DecoderOptions.None, Instruction.Create(Code.EVEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register.XMM1, Register.XMM2, Register.EBX, 0xA5) };
				yield return new object[] { 64, "62F16D08C4CBA5", DecoderOptions.None, Instruction.Create(Code.EVEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register.XMM1, Register.XMM2, Register.EBX, 0xA5U) };
				yield return new object[] { 64, "6462F16D08C48C7501EFCDABA5", DecoderOptions.None, Instruction.Create(Code.EVEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register.XMM1, Register.XMM2, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0xA5) };
				yield return new object[] { 64, "6462F16D08C48C7501EFCDABA5", DecoderOptions.None, Instruction.Create(Code.EVEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register.XMM1, Register.XMM2, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FF, 8, false, Register.FS), 0xA5U) };
#endif
				yield break;
			}
		}

		[Theory]
		[MemberData(nameof(CreateThrowsIfInvalidBitness_Data))]
		void CreateThrowsIfInvalidBitness(Action<int> create) {
			foreach (var bitness in BitnessUtils.GetInvalidBitnessValues())
				Assert.Throws<ArgumentOutOfRangeException>(() => create(bitness));
		}
		public static IEnumerable<object[]> CreateThrowsIfInvalidBitness_Data {
			get {
				yield return new object[] { new Action<int>(bitness => Instruction.CreateXbegin(bitness, 0x800000003412A550)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateOutsb(bitness, Register.FS)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateOutsw(bitness, Register.FS)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateOutsd(bitness, Register.FS)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateScasb(bitness)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateScasw(bitness)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateScasd(bitness)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateScasq(bitness)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateLodsb(bitness, Register.FS)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateLodsw(bitness, Register.FS)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateLodsd(bitness, Register.FS)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateLodsq(bitness, Register.FS)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateInsb(bitness)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateInsw(bitness)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateInsd(bitness)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateStosb(bitness)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateStosw(bitness)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateStosd(bitness)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateStosq(bitness)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateCmpsb(bitness, Register.FS)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateCmpsw(bitness, Register.FS)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateCmpsd(bitness, Register.FS)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateCmpsq(bitness, Register.FS)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateMovsb(bitness, Register.FS)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateMovsw(bitness, Register.FS)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateMovsd(bitness, Register.FS)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateMovsq(bitness, Register.FS)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateMaskmovq(bitness, Register.MM2, Register.MM3, Register.FS)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateMaskmovdqu(bitness, Register.XMM2, Register.XMM3, Register.FS)) };
#if !NO_VEX
				yield return new object[] { new Action<int>(bitness => Instruction.CreateVmaskmovdqu(bitness, Register.XMM2, Register.XMM3, Register.FS)) };
#endif
				yield return new object[] { new Action<int>(bitness => Instruction.CreateRepOutsb(bitness)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateRepOutsw(bitness)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateRepOutsd(bitness)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateRepeScasb(bitness)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateRepeScasw(bitness)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateRepeScasd(bitness)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateRepeScasq(bitness)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateRepneScasb(bitness)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateRepneScasw(bitness)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateRepneScasd(bitness)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateRepneScasq(bitness)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateRepLodsb(bitness)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateRepLodsw(bitness)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateRepLodsd(bitness)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateRepLodsq(bitness)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateRepInsb(bitness)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateRepInsw(bitness)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateRepInsd(bitness)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateRepStosb(bitness)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateRepStosw(bitness)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateRepStosd(bitness)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateRepStosq(bitness)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateRepeCmpsb(bitness)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateRepeCmpsw(bitness)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateRepeCmpsd(bitness)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateRepeCmpsq(bitness)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateRepneCmpsb(bitness)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateRepneCmpsw(bitness)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateRepneCmpsd(bitness)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateRepneCmpsq(bitness)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateRepMovsb(bitness)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateRepMovsw(bitness)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateRepMovsd(bitness)) };
				yield return new object[] { new Action<int>(bitness => Instruction.CreateRepMovsq(bitness)) };
			}
		}

#if !NO_EVEX
		[Fact]
		void Encoding_instruction_requiring_opmask_fails_if_no_opmask() {
			var instruction = Instruction.Create(Code.EVEX_Vpgatherdd_xmm_k1_vm32x, Register.XMM1, new MemoryOperand(Register.RDX, Register.XMM3));
			Assert.False(instruction.HasOpMask);
			var writer = new CodeWriterImpl();
			var encoder = Encoder.Create(64, writer);
			bool result = encoder.TryEncode(instruction, 0, out _, out var errorMessage);
			Assert.False(result);
			Assert.Equal("The instruction must use an opmask register", errorMessage);
		}
#endif

		[Fact]
		void CreateDeclareXXX_throws_if_null_array() {
			Assert.Throws<ArgumentNullException>(() => Instruction.CreateDeclareByte(null));
			Assert.Throws<ArgumentNullException>(() => Instruction.CreateDeclareByte(null, 0, 0));

			Assert.Throws<ArgumentNullException>(() => Instruction.CreateDeclareWord((byte[])null));
			Assert.Throws<ArgumentNullException>(() => Instruction.CreateDeclareWord((byte[])null, 0, 0));
			Assert.Throws<ArgumentNullException>(() => Instruction.CreateDeclareWord((ushort[])null));
			Assert.Throws<ArgumentNullException>(() => Instruction.CreateDeclareWord((ushort[])null, 0, 0));

			Assert.Throws<ArgumentNullException>(() => Instruction.CreateDeclareDword((byte[])null));
			Assert.Throws<ArgumentNullException>(() => Instruction.CreateDeclareDword((byte[])null, 0, 0));
			Assert.Throws<ArgumentNullException>(() => Instruction.CreateDeclareDword((uint[])null));
			Assert.Throws<ArgumentNullException>(() => Instruction.CreateDeclareDword((uint[])null, 0, 0));

			Assert.Throws<ArgumentNullException>(() => Instruction.CreateDeclareQword((byte[])null));
			Assert.Throws<ArgumentNullException>(() => Instruction.CreateDeclareQword((byte[])null, 0, 0));
			Assert.Throws<ArgumentNullException>(() => Instruction.CreateDeclareQword((ulong[])null));
			Assert.Throws<ArgumentNullException>(() => Instruction.CreateDeclareQword((ulong[])null, 0, 0));
		}

		[Fact]
		void CreateDeclareXXX_throws_if_invalid_index_length() {
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareByte(new byte[0]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareByte(new byte[4], -1, 0));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareByte(new byte[4], -1, 1));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareByte(new byte[4], int.MinValue, 1));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareByte(new byte[4], 0, 5));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareByte(new byte[4], 0, int.MaxValue));
			Instruction.CreateDeclareByte(new byte[16]);
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareByte(new byte[17]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareByte(new byte[17], 0, 17));
#if HAS_SPAN
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareByte(new byte[0].AsSpan()));
			Instruction.CreateDeclareByte(new byte[16].AsSpan());
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareByte(new byte[17].AsSpan()));
#endif

			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareWord(new byte[0]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareWord(new byte[1]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareWord(new byte[3]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareWord(new byte[5]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareWord(new byte[7]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareWord(new byte[9]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareWord(new byte[11]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareWord(new byte[13]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareWord(new byte[15]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareWord(new byte[17]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareWord(new byte[4], -1, 0));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareWord(new byte[4], -1, 1));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareWord(new byte[4], int.MinValue, 1));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareWord(new byte[4], 0, 5));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareWord(new byte[4], 0, int.MaxValue));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareWord(new byte[64], 63, 2));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareWord(new byte[64], 32, 1));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareWord(new byte[64]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareWord(new byte[64], 0, 64));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareWord(new ushort[0]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareWord(new ushort[4], -1, 1));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareWord(new ushort[4], int.MinValue, 1));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareWord(new ushort[4], 0, 5));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareWord(new ushort[4], 0, int.MaxValue));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareWord(new ushort[4], 3, 2));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareWord(new ushort[9]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareWord(new ushort[9], 0, 9));
			Instruction.CreateDeclareWord(new ushort[8]);
			Instruction.CreateDeclareWord(new byte[16]);
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareWord(new byte[17], 0, 17));
#if HAS_SPAN
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareWord(new byte[0].AsSpan()));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareWord(new byte[1].AsSpan()));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareWord(new byte[3].AsSpan()));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareWord(new byte[5].AsSpan()));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareWord(new byte[7].AsSpan()));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareWord(new byte[9].AsSpan()));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareWord(new byte[11].AsSpan()));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareWord(new byte[13].AsSpan()));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareWord(new byte[15].AsSpan()));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareWord(new byte[17].AsSpan()));
			Instruction.CreateDeclareWord(new byte[16].AsSpan());
			Instruction.CreateDeclareWord(new ushort[8].AsSpan());
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareWord(new ushort[0].AsSpan()));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareWord(new ushort[9].AsSpan()));
#endif

			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new byte[0]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new byte[1]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new byte[2]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new byte[3]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new byte[5]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new byte[6]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new byte[7]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new byte[9]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new byte[10]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new byte[11]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new byte[13]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new byte[14]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new byte[15]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new byte[17]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new byte[16], -1, 0));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new byte[16], -1, 1));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new byte[16], int.MinValue, 1));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new byte[16], 1, 16));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new byte[16], 0, int.MaxValue));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new byte[64], 0, 9));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new byte[64], 0, 10));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new byte[64], 0, 11));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new byte[64], 63, 1));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new byte[64], 62, 2));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new byte[64], 61, 3));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new uint[0]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new uint[4], -1, 1));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new uint[4], int.MinValue, 1));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new uint[4], 0, 5));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new uint[4], 0, int.MaxValue));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new uint[4], 3, 2));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new uint[5]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new uint[5], 0, 5));
			Instruction.CreateDeclareDword(new uint[4]);
			Instruction.CreateDeclareDword(new byte[16]);
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new byte[17], 0, 17));
#if HAS_SPAN
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new byte[0].AsSpan()));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new byte[1].AsSpan()));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new byte[2].AsSpan()));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new byte[3].AsSpan()));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new byte[5].AsSpan()));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new byte[6].AsSpan()));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new byte[7].AsSpan()));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new byte[9].AsSpan()));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new byte[10].AsSpan()));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new byte[11].AsSpan()));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new byte[13].AsSpan()));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new byte[14].AsSpan()));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new byte[15].AsSpan()));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new byte[17].AsSpan()));
			Instruction.CreateDeclareDword(new byte[16].AsSpan());
			Instruction.CreateDeclareDword(new uint[4].AsSpan());
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new uint[0].AsSpan()));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareDword(new uint[5].AsSpan()));
#endif

			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[0]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[1]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[2]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[3]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[4]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[5]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[6]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[7]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[9]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[10]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[11]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[12]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[13]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[14]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[15]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[17]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[16], -1, 0));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[16], -1, 1));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[16], int.MinValue, 1));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[16], 1, 16));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[16], 0, int.MaxValue));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[64], 0, 9));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[64], 0, 10));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[64], 0, 11));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[64], 0, 12));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[64], 0, 13));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[64], 0, 14));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[64], 0, 15));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[64], 63, 1));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[64], 62, 2));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[64], 61, 3));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[64], 60, 4));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[64], 59, 5));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[64], 58, 6));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[64], 57, 7));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new ulong[0]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new ulong[2], -1, 1));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new ulong[2], int.MinValue, 1));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new ulong[2], 0, 3));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new ulong[2], 0, int.MaxValue));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new ulong[2], 1, 2));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new ulong[3]));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new ulong[3], 0, 3));
			Instruction.CreateDeclareQword(new ulong[2]);
			Instruction.CreateDeclareQword(new byte[16]);
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[17], 0, 17));
#if HAS_SPAN
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[0].AsSpan()));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[1].AsSpan()));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[2].AsSpan()));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[3].AsSpan()));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[4].AsSpan()));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[5].AsSpan()));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[6].AsSpan()));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[7].AsSpan()));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[9].AsSpan()));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[10].AsSpan()));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[11].AsSpan()));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[12].AsSpan()));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[13].AsSpan()));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[14].AsSpan()));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[15].AsSpan()));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new byte[17].AsSpan()));
			Instruction.CreateDeclareQword(new byte[16].AsSpan());
			Instruction.CreateDeclareQword(new ulong[2].AsSpan());
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new ulong[0].AsSpan()));
			Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.CreateDeclareQword(new ulong[3].AsSpan()));
#endif
		}

		[Fact]
		void Equals_returns_false_if_null_input() {
			Assert.False(default(Instruction).Equals(null));
			Assert.False(Instruction.Create(Code.Nopd).Equals(null));
		}

		[Fact]
		void Code_prop_throws_if_invalid() {
			Instruction instruction = default;
			Assert.Throws<ArgumentOutOfRangeException>(() => instruction.Code = (Code)(-1));
			Assert.Throws<ArgumentOutOfRangeException>(() => instruction.Code = (Code)(IcedConstants.CodeEnumCount));
		}

		[Fact]
		void GetSetOpKind_throws_if_invalid_input() {
			var instruction = Instruction.Create(Code.Adc_EAX_imm32, Register.EAX, uint.MaxValue);

			Assert.Throws<ArgumentOutOfRangeException>(() => instruction.GetOpKind(-1));
			instruction.GetOpKind(0);
			instruction.GetOpKind(1);
			for (int i = 2; i < IcedConstants.MaxOpCount; i++)
				instruction.GetOpKind(i);
			Assert.Throws<ArgumentOutOfRangeException>(() => instruction.GetOpKind(IcedConstants.MaxOpCount));

			Assert.Throws<ArgumentOutOfRangeException>(() => instruction.SetOpKind(-1, OpKind.Register));
			instruction.SetOpKind(0, OpKind.Register);
			instruction.SetOpKind(1, OpKind.Immediate32);
			for (int i = 2; i < IcedConstants.MaxOpCount; i++)
				instruction.SetOpKind(i, OpKind.Immediate8);
			Assert.Throws<ArgumentOutOfRangeException>(() => instruction.SetOpKind(IcedConstants.MaxOpCount, OpKind.Register));
		}

		[Fact]
		void GetSetImmediate_throws_if_invalid_input() {
			var instruction = Instruction.Create(Code.Adc_EAX_imm32, Register.EAX, uint.MaxValue);

			Assert.Throws<ArgumentOutOfRangeException>(() => instruction.GetImmediate(-1));
			Assert.Throws<ArgumentException>(() => instruction.GetImmediate(0));
			instruction.GetImmediate(1);
			for (int i = 2; i < IcedConstants.MaxOpCount; i++) {
				if (i == 4 && instruction.Op4Kind == OpKind.Immediate8)
					continue;
				Assert.Throws<ArgumentException>(() => instruction.GetImmediate(i));
			}
			Assert.Throws<ArgumentOutOfRangeException>(() => instruction.GetImmediate(IcedConstants.MaxOpCount));

			Assert.Throws<ArgumentOutOfRangeException>(() => instruction.SetImmediate(-1, 0));
			Assert.Throws<ArgumentOutOfRangeException>(() => instruction.SetImmediate(-1, 0L));
			Assert.Throws<ArgumentOutOfRangeException>(() => instruction.SetImmediate(-1, 0U));
			Assert.Throws<ArgumentOutOfRangeException>(() => instruction.SetImmediate(-1, 0UL));

			Assert.Throws<ArgumentException>(() => instruction.SetImmediate(0, 0));
			Assert.Throws<ArgumentException>(() => instruction.SetImmediate(0, 0L));
			Assert.Throws<ArgumentException>(() => instruction.SetImmediate(0, 0U));
			Assert.Throws<ArgumentException>(() => instruction.SetImmediate(0, 0UL));

			instruction.SetImmediate(1, 0);
			instruction.SetImmediate(1, 0L);
			instruction.SetImmediate(1, 0U);
			instruction.SetImmediate(1, 0UL);

			for (int i = 2; i < IcedConstants.MaxOpCount; i++) {
				if (i == 4 && instruction.Op4Kind == OpKind.Immediate8)
					continue;
				Assert.Throws<ArgumentException>(() => instruction.SetImmediate(i, 0));
				Assert.Throws<ArgumentException>(() => instruction.SetImmediate(i, 0L));
				Assert.Throws<ArgumentException>(() => instruction.SetImmediate(i, 0U));
				Assert.Throws<ArgumentException>(() => instruction.SetImmediate(i, 0UL));
			}
			Assert.Throws<ArgumentOutOfRangeException>(() => instruction.SetImmediate(IcedConstants.MaxOpCount, 0));
			Assert.Throws<ArgumentOutOfRangeException>(() => instruction.SetImmediate(IcedConstants.MaxOpCount, 0L));
			Assert.Throws<ArgumentOutOfRangeException>(() => instruction.SetImmediate(IcedConstants.MaxOpCount, 0U));
			Assert.Throws<ArgumentOutOfRangeException>(() => instruction.SetImmediate(IcedConstants.MaxOpCount, 0UL));
		}

		[Fact]
		void GetSetRegister_throws_if_invalid_input() {
			var instruction = Instruction.Create(Code.Adc_EAX_imm32, Register.EAX, uint.MaxValue);

			Assert.Throws<ArgumentOutOfRangeException>(() => instruction.GetOpRegister(-1));
			for (int i = 0; i < IcedConstants.MaxOpCount; i++)
				instruction.GetOpRegister(i);
			Assert.Throws<ArgumentOutOfRangeException>(() => instruction.GetOpRegister(IcedConstants.MaxOpCount));

			Assert.Throws<ArgumentOutOfRangeException>(() => instruction.SetOpRegister(-1, Register.EAX));
			for (int i = 0; i < IcedConstants.MaxOpCount; i++) {
				if (i == 4 && instruction.Op4Kind == OpKind.Immediate8)
					continue;
				instruction.SetOpRegister(i, Register.EAX);
			}
			Assert.Throws<ArgumentOutOfRangeException>(() => instruction.SetOpRegister(IcedConstants.MaxOpCount, Register.EAX));
		}

		[Fact]
		void SetDeclareXXXValue_throws_if_invalid_input() {
			{
				var instruction = Instruction.CreateDeclareByte(new byte[1]);
				Assert.Throws<ArgumentOutOfRangeException>(() => instruction.SetDeclareByteValue(-1, (sbyte)0));
				Assert.Throws<ArgumentOutOfRangeException>(() => instruction.SetDeclareByteValue(-1, (byte)0));
				Assert.Throws<ArgumentOutOfRangeException>(() => instruction.SetDeclareByteValue(16, (sbyte)0));
				Assert.Throws<ArgumentOutOfRangeException>(() => instruction.SetDeclareByteValue(16, (byte)0));
				for (int i = 0; i < 16; i++) {
					instruction.SetDeclareByteValue(i, (sbyte)0);
					instruction.SetDeclareByteValue(i, (byte)0);
				}
			}
			{
				var instruction = Instruction.CreateDeclareWord(new ushort[1]);
				Assert.Throws<ArgumentOutOfRangeException>(() => instruction.SetDeclareWordValue(-1, (short)0));
				Assert.Throws<ArgumentOutOfRangeException>(() => instruction.SetDeclareWordValue(-1, (ushort)0));
				Assert.Throws<ArgumentOutOfRangeException>(() => instruction.SetDeclareWordValue(8, (short)0));
				Assert.Throws<ArgumentOutOfRangeException>(() => instruction.SetDeclareWordValue(8, (ushort)0));
				for (int i = 0; i < 8; i++) {
					instruction.SetDeclareWordValue(i, (short)0);
					instruction.SetDeclareWordValue(i, (ushort)0);
				}
			}
			{
				var instruction = Instruction.CreateDeclareDword(new uint[1]);
				Assert.Throws<ArgumentOutOfRangeException>(() => instruction.SetDeclareDwordValue(-1, (int)0));
				Assert.Throws<ArgumentOutOfRangeException>(() => instruction.SetDeclareDwordValue(-1, (uint)0));
				Assert.Throws<ArgumentOutOfRangeException>(() => instruction.SetDeclareDwordValue(4, (int)0));
				Assert.Throws<ArgumentOutOfRangeException>(() => instruction.SetDeclareDwordValue(4, (uint)0));
				for (int i = 0; i < 4; i++) {
					instruction.SetDeclareDwordValue(i, (int)0);
					instruction.SetDeclareDwordValue(i, (uint)0);
				}
			}
			{
				var instruction = Instruction.CreateDeclareQword(new ulong[1]);
				Assert.Throws<ArgumentOutOfRangeException>(() => instruction.SetDeclareQwordValue(-1, (long)0));
				Assert.Throws<ArgumentOutOfRangeException>(() => instruction.SetDeclareQwordValue(-1, (ulong)0));
				Assert.Throws<ArgumentOutOfRangeException>(() => instruction.SetDeclareQwordValue(2, (long)0));
				Assert.Throws<ArgumentOutOfRangeException>(() => instruction.SetDeclareQwordValue(2, (ulong)0));
				for (int i = 0; i < 2; i++) {
					instruction.SetDeclareQwordValue(i, (long)0);
					instruction.SetDeclareQwordValue(i, (ulong)0);
				}
			}
		}

		[Fact]
		void GetDeclareXXXValue_throws_if_invalid_input() {
			{
				var instruction = Instruction.CreateDeclareByte(new byte[1]);
				Assert.Throws<ArgumentOutOfRangeException>(() => instruction.GetDeclareByteValue(-1));
				Assert.Throws<ArgumentOutOfRangeException>(() => instruction.GetDeclareByteValue(16));
				for (int i = 0; i < 16; i++)
					instruction.GetDeclareByteValue(i);
			}
			{
				var instruction = Instruction.CreateDeclareWord(new ushort[1]);
				Assert.Throws<ArgumentOutOfRangeException>(() => instruction.GetDeclareWordValue(-1));
				Assert.Throws<ArgumentOutOfRangeException>(() => instruction.GetDeclareWordValue(8));
				for (int i = 0; i < 8; i++)
					instruction.GetDeclareWordValue(i);
			}
			{
				var instruction = Instruction.CreateDeclareDword(new uint[1]);
				Assert.Throws<ArgumentOutOfRangeException>(() => instruction.GetDeclareDwordValue(-1));
				Assert.Throws<ArgumentOutOfRangeException>(() => instruction.GetDeclareDwordValue(4));
				for (int i = 0; i < 4; i++)
					instruction.GetDeclareDwordValue(i);
			}
			{
				var instruction = Instruction.CreateDeclareQword(new ulong[1]);
				Assert.Throws<ArgumentOutOfRangeException>(() => instruction.GetDeclareQwordValue(-1));
				Assert.Throws<ArgumentOutOfRangeException>(() => instruction.GetDeclareQwordValue(2));
				for (int i = 0; i < 2; i++)
					instruction.GetDeclareQwordValue(i);
			}
		}

		[Fact]
		void GetVirtualAddress_throws_if_null_input() {
			var instruction = Instruction.Create(Code.Lea_r64_m, Register.RAX, new MemoryOperand(Register.RCX, Register.RDI, 8));
			Assert.Throws<ArgumentNullException>(() => instruction.GetVirtualAddress(1, 0, (VAGetRegisterValue)null));
			Assert.Throws<ArgumentNullException>(() => instruction.GetVirtualAddress(1, 0, (IVARegisterValueProvider)null));
		}

		[Fact]
		void Create_imm_works() {
			// OpKind.Immediate8
			foreach (var imm in new int[] { -0x80, 0xFF }) {
				var instruction = Instruction.Create(Code.Add_rm8_imm8, Register.CL, imm);
				Assert.Equal((byte)imm, instruction.Immediate8);
			}
			foreach (var imm in new int[] { -0x81, 0x100 })
				Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.Create(Code.Add_rm8_imm8, Register.CL, imm));
			foreach (var imm in new long[] { -0x80, 0xFF }) {
				var instruction = Instruction.Create(Code.Add_rm8_imm8, Register.CL, imm);
				Assert.Equal((byte)imm, instruction.Immediate8);
			}
			foreach (var imm in new long[] { -0x81, 0x100 })
				Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.Create(Code.Add_rm8_imm8, Register.CL, imm));
			foreach (var imm in new uint[] { 0, 0xFF }) {
				var instruction = Instruction.Create(Code.Add_rm8_imm8, Register.CL, imm);
				Assert.Equal(imm, instruction.Immediate8);
			}
			foreach (var imm in new uint[] { 0x100, 0xFFFF_FFFF })
				Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.Create(Code.Add_rm8_imm8, Register.CL, imm));
			foreach (var imm in new ulong[] { 0, 0xFF }) {
				var instruction = Instruction.Create(Code.Add_rm8_imm8, Register.CL, imm);
				Assert.Equal(imm, instruction.Immediate8);
			}
			foreach (var imm in new ulong[] { 0x100, 0xFFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF })
				Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.Create(Code.Add_rm8_imm8, Register.CL, imm));

			// OpKind.Immediate8_2nd
			foreach (var imm in new int[] { -0x80, 0xFF }) {
				var instruction = Instruction.Create(Code.Enterq_imm16_imm8, 0, imm);
				Assert.Equal((byte)imm, instruction.Immediate8_2nd);
			}
			foreach (var imm in new int[] { -0x81, 0x100 })
				Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.Create(Code.Enterq_imm16_imm8, 0, imm));
			foreach (var imm in new uint[] { 0, 0xFF }) {
				var instruction = Instruction.Create(Code.Enterq_imm16_imm8, 0U, imm);
				Assert.Equal(imm, instruction.Immediate8_2nd);
			}
			foreach (var imm in new uint[] { 0x100, 0xFFFF_FFFF })
				Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.Create(Code.Enterq_imm16_imm8, 0U, imm));

			// OpKind.Immediate8to16
			foreach (var imm in new int[] { -0x80, 0x7F }) {
				var instruction = Instruction.Create(Code.Add_rm16_imm8, Register.CX, imm);
				Assert.Equal(imm, instruction.Immediate8to16);
			}
			foreach (var imm in new int[] { -0x81, 0x80 })
				Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.Create(Code.Add_rm16_imm8, Register.CX, imm));
			foreach (var imm in new long[] { -0x80, 0x7F }) {
				var instruction = Instruction.Create(Code.Add_rm16_imm8, Register.CX, imm);
				Assert.Equal(imm, instruction.Immediate8to16);
			}
			foreach (var imm in new long[] { -0x81, 0x80 })
				Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.Create(Code.Add_rm16_imm8, Register.CX, imm));
			foreach (var imm in new uint[] { 0, 0x7F, 0xFF80, 0xFFFF }) {
				var instruction = Instruction.Create(Code.Add_rm16_imm8, Register.CX, imm);
				Assert.Equal(imm, (ushort)instruction.Immediate8to16);
			}
			foreach (var imm in new uint[] { 0x80, 0xFF7F, 0x0001_0000, 0xFFFF_FFFF, 0x0001_FF80, 0x0001_FFFF })
				Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.Create(Code.Add_rm16_imm8, Register.CX, imm));
			foreach (var imm in new ulong[] { 0, 0x7F, 0xFF80, 0xFFFF }) {
				var instruction = Instruction.Create(Code.Add_rm16_imm8, Register.CX, imm);
				Assert.Equal(imm, (ushort)instruction.Immediate8to16);
			}
			foreach (var imm in new ulong[] { 0x80, 0xFF7F, 0x0001_0000, 0xFFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0x0001_FF80, 0x0001_FFFF })
				Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.Create(Code.Add_rm16_imm8, Register.CX, imm));

			// OpKind.Immediate8to32
			foreach (var imm in new int[] { -0x80, 0x7F }) {
				var instruction = Instruction.Create(Code.Add_rm32_imm8, Register.ECX, imm);
				Assert.Equal(imm, instruction.Immediate8to32);
			}
			foreach (var imm in new int[] { -0x81, 0x80 })
				Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.Create(Code.Add_rm32_imm8, Register.ECX, imm));
			foreach (var imm in new long[] { -0x80, 0x7F }) {
				var instruction = Instruction.Create(Code.Add_rm32_imm8, Register.ECX, imm);
				Assert.Equal(imm, instruction.Immediate8to32);
			}
			foreach (var imm in new long[] { -0x81, 0x80 })
				Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.Create(Code.Add_rm32_imm8, Register.ECX, imm));
			foreach (var imm in new uint[] { 0, 0x7F, 0xFFFF_FF80, 0xFFFF_FFFF }) {
				var instruction = Instruction.Create(Code.Add_rm32_imm8, Register.ECX, imm);
				Assert.Equal(imm, (uint)instruction.Immediate8to32);
			}
			foreach (var imm in new uint[] { 0x80, 0xFFFF_FF7F })
				Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.Create(Code.Add_rm32_imm8, Register.ECX, imm));
			foreach (var imm in new ulong[] { 0, 0x7F, 0xFFFF_FF80, 0xFFFF_FFFF }) {
				var instruction = Instruction.Create(Code.Add_rm32_imm8, Register.ECX, imm);
				Assert.Equal(imm, (uint)instruction.Immediate8to32);
			}
			foreach (var imm in new ulong[] { 0x80, 0xFFFF_FF7F, 0x0001_0000_0000, 0xFFFF_FFFF_FFFF_FFFF, 0x0001_FFFF_FF80, 0x0001_FFFF_FFFF })
				Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.Create(Code.Add_rm32_imm8, Register.ECX, imm));

			// OpKind.Immediate8to64
			foreach (var imm in new int[] { -0x80, 0x7F }) {
				var instruction = Instruction.Create(Code.Add_rm64_imm8, Register.RCX, imm);
				Assert.Equal(imm, instruction.Immediate8to64);
			}
			foreach (var imm in new int[] { -0x81, 0x80 })
				Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.Create(Code.Add_rm64_imm8, Register.RCX, imm));
			foreach (var imm in new long[] { -0x80, 0x7F }) {
				var instruction = Instruction.Create(Code.Add_rm64_imm8, Register.RCX, imm);
				Assert.Equal(imm, instruction.Immediate8to64);
			}
			foreach (var imm in new long[] { -0x81, 0x80 })
				Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.Create(Code.Add_rm64_imm8, Register.RCX, imm));
			foreach (var imm in new uint[] { 0, 0x7F }) {
				var instruction = Instruction.Create(Code.Add_rm64_imm8, Register.RCX, imm);
				Assert.Equal(imm, instruction.Immediate8to64);
			}
			foreach (var imm in new uint[] { 0x80, 0xFFFF_FFFF })
				Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.Create(Code.Add_rm64_imm8, Register.RCX, imm));
			foreach (var imm in new ulong[] { 0, 0x7F, 0xFFFF_FFFF_FFFF_FF80, 0xFFFF_FFFF_FFFF_FFFF }) {
				var instruction = Instruction.Create(Code.Add_rm64_imm8, Register.RCX, imm);
				Assert.Equal(imm, (ulong)instruction.Immediate8to64);
			}
			foreach (var imm in new ulong[] { 0x80, 0xFFFF_FFFF_FFFF_FF7F })
				Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.Create(Code.Add_rm64_imm8, Register.RCX, imm));

			// OpKind.Immediate32to64
			foreach (var imm in new int[] { -0x8000_0000, 0x7FFF_FFFF }) {
				var instruction = Instruction.Create(Code.Add_rm64_imm32, Register.RCX, imm);
				Assert.Equal(imm, instruction.Immediate32to64);
			}
			foreach (var imm in new long[] { -0x8000_0000, 0x7FFF_FFFF }) {
				var instruction = Instruction.Create(Code.Add_rm64_imm32, Register.RCX, imm);
				Assert.Equal(imm, instruction.Immediate32to64);
			}
			foreach (var imm in new long[] { -0x8000_0001, 0x8000_0000 })
				Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.Create(Code.Add_rm64_imm32, Register.RCX, imm));
			foreach (var imm in new uint[] { 0, 0x7FFF_FFFF }) {
				var instruction = Instruction.Create(Code.Add_rm64_imm32, Register.RCX, imm);
				Assert.Equal(imm, instruction.Immediate32to64);
			}
			foreach (var imm in new uint[] { 0x8000_0000, 0xFFFF_FFFF })
				Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.Create(Code.Add_rm64_imm32, Register.RCX, imm));
			foreach (var imm in new ulong[] { 0, 0x7FFF_FFFF, 0xFFFF_FFFF_8000_0000, 0xFFFF_FFFF_FFFF_FFFF }) {
				var instruction = Instruction.Create(Code.Add_rm64_imm32, Register.RCX, imm);
				Assert.Equal(imm, (ulong)instruction.Immediate32to64);
			}
			foreach (var imm in new ulong[] { 0x8000_0000, 0x0001_0000_0000, 0xFFFF_FFFF_7FFF_FFFF })
				Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.Create(Code.Add_rm64_imm32, Register.RCX, imm));

			// OpKind.Immediate16
			foreach (var imm in new int[] { -0x8000, 0xFFFF }) {
				var instruction = Instruction.Create(Code.Add_rm16_imm16, Register.CX, imm);
				Assert.Equal((ushort)imm, instruction.Immediate16);
			}
			foreach (var imm in new int[] { -0x8001, 0x0001_0000 })
				Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.Create(Code.Add_rm16_imm16, Register.CX, imm));
			foreach (var imm in new long[] { -0x8000, 0xFFFF }) {
				var instruction = Instruction.Create(Code.Add_rm16_imm16, Register.CX, imm);
				Assert.Equal((ushort)imm, instruction.Immediate16);
			}
			foreach (var imm in new long[] { -0x8001, 0x0001_0000 })
				Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.Create(Code.Add_rm16_imm16, Register.CX, imm));
			foreach (var imm in new uint[] { 0, 0xFFFF }) {
				var instruction = Instruction.Create(Code.Add_rm16_imm16, Register.CX, imm);
				Assert.Equal(imm, instruction.Immediate16);
			}
			foreach (var imm in new uint[] { 0x0001_0000, 0xFFFF_FFFF })
				Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.Create(Code.Add_rm16_imm16, Register.CX, imm));
			foreach (var imm in new ulong[] { 0, 0xFFFF }) {
				var instruction = Instruction.Create(Code.Add_rm16_imm16, Register.CX, imm);
				Assert.Equal(imm, instruction.Immediate16);
			}
			foreach (var imm in new ulong[] { 0x0001_0000, 0xFFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF })
				Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.Create(Code.Add_rm16_imm16, Register.CX, imm));

			// OpKind.Immediate32
			foreach (var imm in new int[] { -0x8000_0000, 0x7FFF_FFFF }) {
				var instruction = Instruction.Create(Code.Add_rm32_imm32, Register.ECX, imm);
				Assert.Equal((uint)imm, instruction.Immediate32);
			}
			foreach (var imm in new long[] { -0x8000_0000, 0xFFFF_FFFF }) {
				var instruction = Instruction.Create(Code.Add_rm32_imm32, Register.ECX, imm);
				Assert.Equal((uint)imm, instruction.Immediate32);
			}
			foreach (var imm in new long[] { -0x8000_0001, 0x0001_0000_0000 })
				Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.Create(Code.Add_rm32_imm32, Register.ECX, imm));
			foreach (var imm in new uint[] { 0, 0xFFFF_FFFF }) {
				var instruction = Instruction.Create(Code.Add_rm32_imm32, Register.ECX, imm);
				Assert.Equal(imm, instruction.Immediate32);
			}
			foreach (var imm in new ulong[] { 0, 0xFFFF_FFFF }) {
				var instruction = Instruction.Create(Code.Add_rm32_imm32, Register.ECX, imm);
				Assert.Equal(imm, instruction.Immediate32);
			}
			foreach (var imm in new ulong[] { 0x0001_0000_0000, 0xFFFF_FFFF_FFFF_FFFF })
				Assert.Throws<ArgumentOutOfRangeException>(() => Instruction.Create(Code.Add_rm32_imm32, Register.ECX, imm));

			// OpKind.Immediate64
			foreach (var imm in new int[] { int.MinValue, int.MaxValue }) {
				var instruction = Instruction.Create(Code.Mov_r64_imm64, Register.RCX, imm);
				Assert.Equal((ulong)imm, instruction.Immediate64);
			}
			foreach (var imm in new long[] { long.MinValue, long.MaxValue }) {
				var instruction = Instruction.Create(Code.Mov_r64_imm64, Register.RCX, imm);
				Assert.Equal((ulong)imm, instruction.Immediate64);
			}
			foreach (var imm in new uint[] { uint.MinValue, uint.MaxValue }) {
				var instruction = Instruction.Create(Code.Mov_r64_imm64, Register.RCX, imm);
				Assert.Equal(imm, instruction.Immediate64);
			}
			foreach (var imm in new ulong[] { ulong.MinValue, ulong.MaxValue }) {
				var instruction = Instruction.Create(Code.Mov_r64_imm64, Register.RCX, imm);
				Assert.Equal(imm, instruction.Immediate64);
			}
		}

		[Fact]
		void EncodeInvalidLenDwDdDq() {
			var encoder = Encoder.Create(64, new CodeWriterImpl());

			var dw = Instruction.CreateDeclareWord(1);
			dw.DeclareDataCount = 8;
			Assert.True(encoder.TryEncode(dw, 0, out var len, out _));
			Assert.Equal(16U, len);
			dw.DeclareDataCount = 8 + 1;
			Assert.False(encoder.TryEncode(dw, 0, out _, out _));

			var dd = Instruction.CreateDeclareDword(1);
			dd.DeclareDataCount = 4;
			Assert.True(encoder.TryEncode(dd, 0, out len, out _));
			Assert.Equal(16U, len);
			dd.DeclareDataCount = 4 + 1;
			Assert.False(encoder.TryEncode(dd, 0, out _, out _));

			var dq = Instruction.CreateDeclareQword(1);
			dq.DeclareDataCount = 2;
			Assert.True(encoder.TryEncode(dq, 0, out len, out _));
			Assert.Equal(16U, len);
			dq.DeclareDataCount = 2 + 1;
			Assert.False(encoder.TryEncode(dq, 0, out _, out _));
		}
	}
}
#endif
