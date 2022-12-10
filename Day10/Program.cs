    //  +--- Add your data here
    //  |
    //  V
const string Input = """
    addx 2
    addx 15
    addx -11
    addx 6
    noop
    noop
    noop
    addx -1
    addx 5
    addx -1
    addx 5
    noop
    noop
    noop
    noop
    noop
    addx 7
    addx -1
    addx 3
    addx 1
    addx 5
    addx 1
    noop
    addx -38
    noop
    addx 1
    addx 6
    addx 3
    noop
    addx -8
    noop
    addx 13
    addx 2
    addx 3
    addx -2
    addx 2
    noop
    addx 3
    addx 9
    addx -2
    addx 2
    addx -10
    addx 11
    addx 2
    addx -14
    addx -21
    addx 2
    noop
    addx 5
    addx 29
    addx -2
    noop
    addx -19
    noop
    addx 2
    addx 11
    addx -10
    addx 2
    addx 5
    addx -9
    noop
    addx 14
    addx 2
    addx 3
    addx -2
    addx 3
    addx 1
    noop
    addx -37
    noop
    addx 13
    addx -8
    noop
    noop
    noop
    noop
    addx 13
    addx -5
    addx 3
    addx 3
    addx 3
    noop
    noop
    noop
    noop
    noop
    noop
    noop
    addx 6
    addx 3
    addx 1
    addx 5
    addx -15
    addx 5
    addx -27
    addx 30
    addx -23
    addx 33
    addx -32
    addx 2
    addx 5
    addx 2
    addx -16
    addx 17
    addx 2
    addx -10
    addx 17
    addx 10
    addx -9
    addx 2
    addx 2
    addx 5
    addx -29
    addx -8
    noop
    noop
    noop
    addx 19
    addx -11
    addx -1
    addx 6
    noop
    noop
    addx -1
    addx 3
    noop
    addx 3
    addx 2
    addx -3
    addx 11
    addx -1
    addx 5
    addx -2
    addx 5
    addx 2
    noop
    noop
    addx 1
    noop
    noop
    """;

var instructions = Input.Split("\n")
    .Select(line => line.Split(" "))
    .Select(parts => (Operator: parts[0], Operand: parts.Length == 2 ? int.Parse(parts[1]) : (int?)null))
    .ToArray();

// === Part one ===
int totalStrength = 0;
Execute(instructions, (cycle, x) =>
{
    if ((cycle - 19) % 40 == 0)
    {
        var signalStrength = (cycle + 1) * x;
        totalStrength += signalStrength;
    }
});
System.Console.WriteLine(totalStrength);

// === Part two ===
var screen = new char[6, 40];
Execute(instructions, (cycle, x) =>
{
    var posX = cycle % 40;
    var posY = cycle / 40;
    if (posX >= x - 1 && posX <= x + 1)
    {
        screen[posY, posX] = '#';
    } else {
        screen[posY, posX] = '.';
    }
});
for (var row = 0; row < 6; row++)
{
    for (var col = 0; col < 40; col++)
    {
        System.Console.Write(screen[row, col]);
    }

    System.Console.WriteLine();
}

void Execute((string Operator, int? Operand)[] instructions, Action<int, int> process)
{
    int cycle = 0, ip = 0, x = 1, beginCurrentOp = 0;
    while(true)
    {
        var newOp = false;
        if (instructions[ip].Operator == "noop" && cycle - beginCurrentOp == 1)
        {
            ip++;
            newOp = true;
        }
        else if (instructions[ip].Operator == "addx" && cycle - beginCurrentOp == 2)
        {
            x += instructions[ip].Operand!.Value;
            ip++;
            newOp = true;
        }

        if (ip == instructions.Length)
        {
            break;
        }

        process(cycle, x);

        if (newOp)
        {
            beginCurrentOp = cycle;
        }

        cycle++;
    }
}
