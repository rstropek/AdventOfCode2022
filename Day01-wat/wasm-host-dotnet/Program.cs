using Wasmtime;

using var engine = new Engine();
using var module = Module.FromFile(engine, "../aoc1.wasm");

using var store = new Store(engine); // Read more about stores at https://docs.wasmtime.dev/api/wasmtime/struct.Store.html

using var linker = new Linker(engine); // Read more about linker at https://docs.wasmtime.dev/api/wasmtime/struct.Linker.html
linker.Define(
    "cb",
    "found_value_cb",
    Function.FromCallback(store, (int value) => Console.WriteLine($"Found value: {value}"))
);

var instance = linker.Instantiate(store, module);

var input = """
    1000
    2000
    3000

    4000

    5000
    6000

    7000
    8000
    9000

    10000
    """u8; // Note u8 postfix here to convert string into byte array (UTF8)

// Copy inpot to address 0
var target = instance.GetMemory("memory")!.GetSpan(0, input.Length);
input.CopyTo(target);

// Call get_max function and print result
var run = instance.GetFunction<int, int>("get_max")!;
var result = run(input.Length);
Console.WriteLine($"Result: {result}");
