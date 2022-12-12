(module
    ;; Callback for reporting back that a value has been found
    ;;(func $found_value_cb (import "cb" "found_value_cb") (param $val i32))
    (import "cb" "found_value_cb" (func $found_value_cb (param $val i32)))

    ;; Memory through which we receive the input
    (memory $m1 (export "memory") 1 1)

    (func $ping (param $in i32)
        (result i32)
        local.get $in
    )
    (export "ping" (func $ping))

    (func $get_max (param $len i32)
        (result i32)
        (local $total i32) ;; Sum of the current elf
        (local $max i32) ;; Overall maximum of calories
        (local $ix i32) ;; Current index in $m1
        (local $current i32) ;; Currently parsed number
        (loop $loop
            ;; Parse the next number
            (call $parse_num (local.get $ix) (local.get $len))
            local.set $ix ;; parse_num returns the index after the parsed number, this is the new $ix
            local.set $current ;; Store found number in current

            ;; Report back that we found a value
            (call $found_value_cb (local.get $current))

            ;; $total += $current
            (local.set $total (i32.add (local.get $current) (local.get $total)))

            ;; Check if we are at the end of an elf (end of input or \n\n)
            (i32.or
                (i32.eq (local.get $ix) (local.get $len))
                (i32.eq (i32.load8_u (i32.add (local.get $ix) (i32.const 1))) (i32.const 10))
            )
            (if
                (then
                    ;; We are at the end of an elf

                    ;; Check if $total is greater than $max; if so, set $max to $total
                    (i32.gt_u (local.get $total) (local.get $max))
                    (if
                        (then
                            (local.set $max (local.get $total))
                        )
                    )

                    ;; Reset $total
                    (local.set $total (i32.const 0))

                    ;; Check if we are at the end of the input; if not, jump over \n\n
                    ;; and continue with parsing the next number
                    (i32.ne (local.get $ix) (local.get $len))
                    (if 
                        (then
                            (local.set $ix (i32.add (local.get $ix) (i32.const 2)))
                            br $loop
                        )
                    )
                )
                (else
                    ;; We are not at the end of an elf

                    ;; Jump over \n at the end of number
                    (local.set $ix (i32.add (local.get $ix) (i32.const 1)))
                    br $loop
                )
            )
        )

        local.get $max
    )
    (export "get_max" (func $get_max))

    (func $parse_num (param $ix i32) (param $len i32)
        (result i32)
        (result i32)
        (local $num i32)
        (loop $loop
            ;; check if $m1[$ix] is not a \n and not the end of the input
            (i32.and
                (i32.ne (i32.load8_u (local.get $ix)) (i32.const 10))
                (i32.ne (local.get $ix) (local.get $len))
            )
            (if
                (then
                    ;; $num = $num * 10 + ($m1[$ix] - '0')
                    (local.set $num
                        (i32.add
                            (i32.mul
                                (local.get $num)
                                (i32.const 10)
                            )
                            (i32.sub
                                (i32.load8_u (local.get $ix))
                                (i32.const 48)
                            )
                        )
                    )

                    ;; Increment index
                    (local.set $ix (i32.add (local.get $ix) (i32.const 1)))

                    br $loop
                )
            )
        )

        local.get $num
        local.get $ix
    )
)
