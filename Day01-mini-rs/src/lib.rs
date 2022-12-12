#![no_std]
use no_std_compat::ptr;

#[panic_handler]
fn handle_panic(_: &core::panic::PanicInfo) -> ! {
    loop {}
}

#[link(wasm_import_module = "cb")]
extern { fn found_value_cb(val: i32); }

static mut BUFFER: [u8; 1024] = [1u8; 1024];

#[no_mangle]
pub fn ping(val: i32) -> i32 {
    val
}

#[no_mangle]
pub fn get_buffer(size: usize) -> *mut u8 {
    if size > unsafe { BUFFER.len() } {
        return ptr::null_mut()
    }

    unsafe { BUFFER.as_mut_ptr() }
}

#[no_mangle]
pub fn get_max(len: usize) -> i32 {
    if len > unsafe { BUFFER.len() } {
        return -1;
    }

    let mut max = 0;
    let mut total = 0;
    let mut ix = 0;

    loop {
        let current;
        (current, ix) = parse_num(ix, len);
        unsafe { found_value_cb(current); }
        total += current;

        if ix >= len || *unsafe { BUFFER.get_unchecked(ix + 1) } == b'\n' {
            if total > max {
                max = total;
            }

            total = 0;

            if ix != len {
                ix += 2;
            } else {
                break;
            }
        } else {
            ix += 1;
        }
    }

    max
}

fn parse_num(mut ix: usize, len: usize) -> (i32, usize) {
    let mut num = 0;
    while ix < len && *unsafe { BUFFER.get_unchecked(ix) } != b'\n' {
        num = num * 10 + (*unsafe { BUFFER.get_unchecked(ix) } - b'0') as i32;
        ix += 1;
    }
    
    (num, ix)
}