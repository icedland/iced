extern crate rustc_version;
use rustc_version::{version, Version};

fn main() {
	if version().unwrap() < Version::parse("1.36.0").unwrap() {
		println!("cargo:rustc-cfg=use_std_mem_uninitialized");
	}
}
