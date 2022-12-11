use anyhow::Result;
use wasmtime::*;

const INPUT: &[u8] = r#"1000
2000
3000

4000

5000
6000

7000
8000
9000

10000"#
    .as_bytes();

fn main() -> Result<()> {
    // Setup wasmtime
    let mut config = Config::default();
    let engine = Engine::new(config.debug_info(true))?;
    let module = Module::from_file(&engine, "../aoc1.wasm")?;
    let mut store = Store::new(&engine, ());

    // Register callback function
    let found_value_cb = Func::wrap(&mut store, |val: i32| {
        println!("Found value: {val}");
    });
    let imports = [found_value_cb.into()];

    // Instantiate module
    let instance = Instance::new(&mut store, &module, &imports)?;

    // Copy input as UTF8 bytes into wasm memory
    let memory = instance
        .get_memory(&mut store, "memory")
        .ok_or(anyhow::format_err!("no `memory` export"))?;
    let target = memory.data_mut(&mut store);
    for i in 0..INPUT.len() {
        target[i] = INPUT[i];
    }

    // Call wasm function
    let double = instance.get_typed_func::<i32, i32, _>(&mut store, "get_max")?;
    let result = double.call(&mut store, INPUT.len() as i32)?;

    // Print result
    println!("Result: {result}");

    Ok(())
}
