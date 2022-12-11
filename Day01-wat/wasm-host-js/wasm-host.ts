import fs from "fs";

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
  const wasmBuffer = fs.readFileSync("../aoc1.wasm");

  // Object with callbacks exported from JS to Wasm
  const callbacks = {
    cb: {
      found_value_cb: function found_value_cb(value: number) {
        console.log(`Found value: ${value}`);
      },
    },
  };

  // Instantiate Wasm module
  const wasmModule = await WebAssembly.instantiate(wasmBuffer, callbacks);

  // Get exports from Wasm module
  const {
    get_max,
    memory,
  }: {
    get_max: (length: number) => number;
    memory: WebAssembly.Memory;
  } = wasmModule.instance.exports as any;

  // Write input to Wasm memory
  const bytes = new Uint8Array(memory.buffer, 0, 1024);
  new TextEncoder().encodeInto(input, bytes);

  // Call Wasm function
  const max = get_max(input.length);

  // Print result
  console.log(`Result: ${max}`);
})();
