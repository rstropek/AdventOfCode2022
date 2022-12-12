const fs = require("fs");

const input = `1000
2000
3000

4000

5000
6000

7000
8000
9000

10000`;

(async () => {
  // Read Wasm module
  const wasmBuffer = fs.readFileSync("target/wasm32-unknown-unknown/release/hello_wasm.wasm");

  const callbacks = {
    cb: {
      found_value_cb(value) {
        console.log(`Found value: ${value}`);
      },
    },
  };

  // Instantiate Wasm module
  const wasmModule = await WebAssembly.instantiate(wasmBuffer, callbacks);

  // Get exports from Wasm module
  const {
    get_max,
    get_buffer,
    parse_num,
    memory,
  } = wasmModule.instance.exports;

  // Write input to Wasm memory
  const buffer = get_buffer();
  const bytes = new Uint8Array(memory.buffer, buffer, 1024);
  new TextEncoder().encodeInto(input, bytes);

  // Call Wasm function
  const max = get_max(input.length);

  // Print result
  console.log(`Result: ${max}`);
})();
