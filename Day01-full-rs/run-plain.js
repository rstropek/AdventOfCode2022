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
  const wasmBuffer = fs.readFileSync("target/wasm32-unknown-unknown/release/full_rs.wasm");

  // Instantiate Wasm module
  const wasmModule = await WebAssembly.instantiate(wasmBuffer, {});

  // Get exports from Wasm module
  const {
    alloc,
    free,
    day_1_part_1,
    day_1_part_1_raw,
    memory,
  } = wasmModule.instance.exports;

  // Allocate memory for input
  const buffer = alloc(input.length);
  const bytes = new Uint8Array(memory.buffer, buffer, input.length);
  new TextEncoder().encodeInto(input, bytes);

  // Call Wasm function
  const max = day_1_part_1_raw(buffer, input.length);

  free(buffer);

  // Print result
  console.log(`Result: ${max}`);
})();
