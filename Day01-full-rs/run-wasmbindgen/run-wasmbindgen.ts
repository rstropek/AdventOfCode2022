import * as we from 'full-rs';
import { memory } from 'full-rs/full_rs_bg.wasm';

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

const max = we.day_1_part_1(input);
console.log(max);
