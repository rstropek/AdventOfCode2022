# Advent of Code 2022, Day 01 - Minimal Rust Wasm

This example implements Advent of Code, day 1, part 1 in Rust. However, it is not a regular Rust implementation. It implements the code in a way that the generated Wasm code is minimal. It can roughly be compared to a hand-crafted version (see [Day01-wat](../Day01-wat)) of the program.

In order to minimize the amount of generated code, the following measures were taken:

* *no_std*
* Empty panic handler
* No use of strings, just *u8*
* Only unchecked indexing to avoid bounds checking

Note that it is **not** recommended to write code like that in real-life. This sample was created for a meetup talk in which the basic principles of Wasm were the main topic.
