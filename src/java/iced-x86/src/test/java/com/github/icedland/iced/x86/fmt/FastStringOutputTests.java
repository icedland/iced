// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt;

import static org.junit.jupiter.api.Assertions.*;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.*;
import org.junit.jupiter.params.provider.*;

import com.github.icedland.iced.x86.fmt.fast.FastStringOutput;

final class FastStringOutputTests {
	private static String createString(char c, int count) {
		StringBuilder sb = new StringBuilder();
		for (int i = 0; i < count; i++)
			sb.append(c);
		return sb.toString();
	}

	@ParameterizedTest
	@ValueSource(ints = { -1, 0, 1, 1000 })
	void appendChars(int capacity) {
		FastStringOutput output = capacity < 0 ? new FastStringOutput() : new FastStringOutput(capacity);
		assertEquals("", output.toString());
		output.append('a');
		assertEquals("a", output.toString());
		output.append('b');
		assertEquals("ab", output.toString());
		for (int i = 0; i < 1000; i++)
			output.append('c');
		assertEquals("ab" + createString('c', 1000), output.toString());
	}

	@ParameterizedTest
	@ValueSource(strings = { "", "q", "qw", "qwerty" })
	void appendString(String s) {
		int[] capacities = new int[] { -1, 0, 1, 1000 };
		for (int capacity : capacities) {
			FastStringOutput output = capacity < 0 ? new FastStringOutput() : new FastStringOutput(capacity);
			assertEquals("", output.toString());
			output.append(s);
			assertEquals(s, output.toString());
			output.append("abc");
			assertEquals(s + "abc", output.toString());
			for (int i = 0; i < 200; i++)
				output.append("x");
			assertEquals(s + "abc" + createString('x', 200), output.toString());
			for (int i = 0; i < 200; i++)
				output.append("yy");
			assertEquals(s + "abc" + createString('x', 200) + createString('y', 400), output.toString());
		}
	}

	@Test
	void appendNullWorks() {
		FastStringOutput output = new FastStringOutput();
		assertEquals("", output.toString());
		output.append(null);
		assertEquals("", output.toString());
		output.append('a');
		assertEquals("a", output.toString());
		output.append(null);
		assertEquals("a", output.toString());
	}

	@Test
	void clearWorks() {
		FastStringOutput output = new FastStringOutput();
		assertEquals("", output.toString());
		output.append('a');
		assertEquals("a", output.toString());
		output.clear();
		assertEquals("", output.toString());
		output.append("abc");
		assertEquals("abc", output.toString());
		output.clear();
		assertEquals("", output.toString());
	}

	@Test
	void lengthWorks() {
		FastStringOutput output = new FastStringOutput();
		assertEquals(0, output.size());
		output.append('a');
		assertEquals(1, output.size());
		output.append('b');
		assertEquals(2, output.size());
		output.append("cde");
		assertEquals(5, output.size());
		output.clear();
		assertEquals(0, output.size());
		output.append("abc");
		assertEquals(3, output.size());
		output.clear();
		assertEquals(0, output.size());
	}
}
