extern crate rustc_version;
use rustc_version::{version, Version};

fn main() {
	if version().unwrap() >= Version::parse("1.26.0").unwrap() {
		println!("cargo:rustc-cfg=has_fused_iterator");
	}
	if version().unwrap() >= Version::parse("1.27.0").unwrap() {
		println!("cargo:rustc-cfg=has_must_use");
	}
	if version().unwrap() < Version::parse("1.36.0").unwrap() {
		println!("cargo:rustc-cfg=use_std_mem_uninitialized");
	}
}
