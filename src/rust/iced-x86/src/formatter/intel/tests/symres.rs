// SPDX-License-Identifier: MIT
// Copyright wtfsck@protonmail.com
// Copyright iced contributors

use super::super::super::tests::sym_res::symbol_resolver_test;
use super::fmt_factory::create_resolver;

#[test]
fn symres() {
	symbol_resolver_test("Intel", "SymbolResolverTests", |symbol_resolver| create_resolver(symbol_resolver));
}
