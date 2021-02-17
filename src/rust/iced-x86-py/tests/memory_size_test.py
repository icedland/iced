# SPDX-License-Identifier: MIT
# Copyright (C) 2018-present iced project and contributors

import pytest
from iced_x86 import *

def test_memory_size_ext():
	assert MemorySizeExt.size(MemorySize.UINT128) == 16
	assert MemorySizeExt.element_size(MemorySize.UINT128) == 16
	assert MemorySizeExt.element_type(MemorySize.UINT128) == MemorySize.UINT128
	assert not MemorySizeExt.is_signed(MemorySize.UINT128)
	assert MemorySizeExt.is_signed(MemorySize.PACKED256_INT16)
	assert not MemorySizeExt.is_packed(MemorySize.UINT128)
	assert MemorySizeExt.element_count(MemorySize.UINT128) == 1
	assert not MemorySizeExt.is_broadcast(MemorySize.UINT128)

	assert MemorySizeExt.size(MemorySize.PACKED256_INT16) == 32
	assert MemorySizeExt.element_size(MemorySize.PACKED256_INT16) == 2
	assert MemorySizeExt.element_type(MemorySize.PACKED256_INT16) == MemorySize.INT16
	assert MemorySizeExt.is_signed(MemorySize.PACKED256_INT16)
	assert MemorySizeExt.is_packed(MemorySize.PACKED256_INT16)
	assert MemorySizeExt.element_count(MemorySize.PACKED256_INT16) == 16
	assert not MemorySizeExt.is_broadcast(MemorySize.PACKED256_INT16)

	assert MemorySizeExt.is_broadcast(MemorySize.BROADCAST128_2X_INT32)

	assert MemorySizeExt.info(MemorySize.PACKED256_INT16).memory_size == MemorySize.PACKED256_INT16
	assert MemorySizeExt.info(MemorySize.PACKED256_INT16).size == 32
	assert MemorySizeExt.element_type_info(MemorySize.PACKED256_INT16).element_type == MemorySize.INT16

@pytest.mark.parametrize("create", [
	lambda memory_size: MemorySizeExt.info(memory_size),
	lambda memory_size: MemorySizeInfo(memory_size),
])
def test_memory_size_info(create):
	info = create(MemorySize.UINT128)
	assert info.memory_size == MemorySize.UINT128
	assert info.size == 16
	assert info.element_size == 16
	assert info.element_type == MemorySize.UINT128
	assert not info.is_signed
	assert create(MemorySize.PACKED256_INT16).is_signed
	assert not info.is_packed
	assert info.element_count == 1
	assert not info.is_broadcast

	info = create(MemorySize.PACKED256_INT16)
	assert info.memory_size == MemorySize.PACKED256_INT16
	assert info.size == 32
	assert info.element_size == 2
	assert info.element_type == MemorySize.INT16
	assert info.is_signed
	assert info.is_packed
	assert info.element_count == 16
	assert not info.is_broadcast

	assert create(MemorySize.BROADCAST128_2X_INT32).is_broadcast

@pytest.mark.parametrize("create", [
	lambda memory_size: MemorySizeExt.info(memory_size),
	lambda memory_size: MemorySizeInfo(memory_size),
])
def test_memory_size_info_invalid_arg(create):
	with pytest.raises(ValueError):
		create(1234)

def test_ext_size_invalid_arg():
	with pytest.raises(ValueError):
		MemorySizeExt.size(1234)

def test_ext_element_size_invalid_arg():
	with pytest.raises(ValueError):
		MemorySizeExt.element_size(1234)

def test_ext_element_type_invalid_arg():
	with pytest.raises(ValueError):
		MemorySizeExt.element_type(1234)

def test_ext_element_type_info_invalid_arg():
	with pytest.raises(ValueError):
		MemorySizeExt.element_type_info(1234)

def test_ext_is_signed_invalid_arg():
	with pytest.raises(ValueError):
		MemorySizeExt.is_signed(1234)

def test_ext_is_packed_invalid_arg():
	with pytest.raises(ValueError):
		MemorySizeExt.is_packed(1234)

def test_ext_element_count_invalid_arg():
	with pytest.raises(ValueError):
		MemorySizeExt.element_count(1234)

def test_ext_is_broadcast_invalid_arg():
	with pytest.raises(ValueError):
		MemorySizeExt.is_broadcast(1234)
