// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use super::super::super::tests::sym_res::symbol_resolver_test_fast;
use super::fmt_factory::create_resolver;

#[test]
fn symres() {
	symbol_resolver_test_fast("Fast", "SymbolResolverTests", |symbol_resolver| create_resolver(symbol_resolver));
}
