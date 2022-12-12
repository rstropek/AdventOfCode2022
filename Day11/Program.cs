//  +--- Add your data here
//  |
//  V
const string Input = """
    Monkey 0:
    Starting items: 79, 98
    Operation: new = old * 19
    Test: divisible by 23
        If true: throw to monkey 2
        If false: throw to monkey 3

    Monkey 1:
    Starting items: 54, 65, 75, 74
    Operation: new = old + 6
    Test: divisible by 19
        If true: throw to monkey 2
        If false: throw to monkey 0

    Monkey 2:
    Starting items: 79, 60, 97
    Operation: new = old * old
    Test: divisible by 13
        If true: throw to monkey 1
        If false: throw to monkey 3

    Monkey 3:
    Starting items: 74
    Operation: new = old + 3
    Test: divisible by 17
        If true: throw to monkey 0
        If false: throw to monkey 1
    """;

// === Part one ===
var data = Parse();
System.Console.WriteLine(Calculate(data, 20, 3));


// === Part two ===
data = Parse();
System.Console.WriteLine(Calculate(data, 10_000, 1));


long Calculate(IReadOnlyList<Monkey> data, int rounds, int divisor)
{
    var inspectionCount = new long[data.Count];
    var lcm = data.Select(d => d.Divisable).Aggregate(1L, (a, b) => a * b);
    for (var round = 0; round < rounds; round++)
    {
        for (var mix = 0; mix < data.Count; mix++)
        {
            var monkey = data[mix];

            while (monkey.WorryLevels.Any())
            {
                var item = monkey.WorryLevels.First();

                var level = 0L;
                if (monkey.Operation == '+')
                {
                    if (monkey.OperandOld ?? false)
                    {
                        level = item + item;
                    }
                    else
                    {
                        level = item + monkey.Operand!.Value;
                    }
                }
                else
                {
                    if (monkey.OperandOld ?? false)
                    {
                        level = item * item;
                    }
                    else
                    {
                        level = item * monkey.Operand!.Value;
                    }
                }

                level /= divisor;
                level %= lcm;

                if (level % monkey.Divisable == 0)
                {
                    data[monkey.TrueTarget].WorryLevels.Add(level);
                }
                else
                {
                    data[monkey.FalseTarget].WorryLevels.Add(level);
                }

                monkey.WorryLevels.RemoveAt(0);

                inspectionCount[mix]++;
            }
        }
    }

    var mostActive = inspectionCount.OrderDescending().Take(2).ToArray();
    return mostActive[0] * mostActive[1];
}


IReadOnlyList<Monkey> Parse()
{
    var monkeys = Input.Split("\n\n");
    var result = new List<Monkey>(monkeys.Length);

    foreach (var monkey in monkeys)
    {
        var lines = monkey.Split("\n")[1..];

        var worryLevels = lines[0].Split(": ")[1].Split(", ").Select(long.Parse).ToList();

        var operationData = lines[1].Split("old ")[1];
        var operation = operationData[0];
        operationData = operationData[2..];
        var operand = operationData == "old" ? (long?)null : long.Parse(operationData);
        var operandOld = operationData == "old";

        var divisable = long.Parse(lines[2].Split("by ")[1]);

        var trueTarget = int.Parse(lines[3].Split("to monkey ")[1]);

        var falseTarget = int.Parse(lines[4].Split("to monkey ")[1]);

        result.Add(new Monkey(worryLevels, operation, operand, operandOld, divisable, trueTarget, falseTarget));
    }

    return result;
}

record Monkey(List<long> WorryLevels, char Operation, long? Operand, bool? OperandOld, long Divisable, int TrueTarget, int FalseTarget);
