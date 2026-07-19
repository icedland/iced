extern crate winres;

#[cfg(windows)]
fn main() {
  if cfg!(target_os = "windows") {
    let res = winres::WindowsResource::new();
//    res.set_icon("Iced.ico");
    res.compile().unwrap();
  }
}

#[cfg(unix)]
fn main() {
}