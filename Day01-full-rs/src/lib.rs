#[cfg(feature = "wasmbindgen")]
use wasm_bindgen::prelude::*;

#[cfg_attr(feature = "wasmbindgen", wasm_bindgen)]
#[cfg_attr(not(feature = "wasmbindgen"), no_mangle)]
pub fn day_1_part_1(input: &str) -> i32 {
    input
        .split("\n\n")
        .map(|s| s.lines().map(|l| l.parse::<i32>().unwrap()).sum())
        .max()
        .unwrap()
}

#[cfg_attr(not(feature = "wasmbindgen"), no_mangle)]
#[cfg(not(feature = "wasmbindgen"))]
pub fn day_1_part_1_raw(ptr: *mut u8, len: usize) -> i32 {
    let bytes = unsafe { std::slice::from_raw_parts(ptr, len) };
    let input = std::str::from_utf8(bytes).unwrap();
    let result = day_1_part_1(input);
    result
}

#[cfg_attr(not(feature = "wasmbindgen"), no_mangle)]
#[cfg(not(feature = "wasmbindgen"))]
pub fn alloc(len: usize) -> *mut u8 {
    let mut v = Vec::with_capacity(len);
    let ptr = v.as_mut_ptr();
    std::mem::forget(v);
    ptr
}

#[cfg_attr(not(feature = "wasmbindgen"), no_mangle)]
#[cfg(not(feature = "wasmbindgen"))]
pub fn free(ptr: *mut u8, len: usize) {
    let data = unsafe { Vec::from_raw_parts(ptr, len, len) };
    drop(data);
}
