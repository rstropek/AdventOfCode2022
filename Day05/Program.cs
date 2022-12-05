using System.Text.RegularExpressions;

var regex = Data.Movements();

// === Part one ===
var logistics = Parse();

foreach (var move in logistics.Movements)
{
    for (var i = 0; i < move.Count; i++)
    {
        // Move top item from one stack to another
        logistics.Stacks[move.To].Add(logistics.Stacks[move.From][^1]);
        logistics.Stacks[move.From].RemoveAt(logistics.Stacks[move.From].Count - 1);
    }
}

logistics.Stacks.ForEach(s => Console.Write(s[^1]));
System.Console.WriteLine();

// === Part two ===
logistics = Parse();

foreach (var move in logistics.Movements)
{
    // Move items from one stack to another
    var from = logistics.Stacks[move.From];
    logistics.Stacks[move.To].AddRange(from.GetRange(from.Count - move.Count, move.Count));
    logistics.Stacks[move.From].RemoveRange(from.Count - move.Count, move.Count);
}

logistics.Stacks.ForEach(s => Console.Write(s[^1]));
System.Console.WriteLine();

LogisticsData Parse()
{
    // Split input into stacks and movements
    var parts = Data.Input.Split("\n\n");

    // Get number of stacks and lines of stack data
    var stackLines = parts[0].Split('\n');
    var numberOfStacks = stackLines[^2].Split(' ').Length;

    var result = new LogisticsData(
        // Parse movements
        regex.Matches(parts[1])
            .Select(m => m.Groups.Cast<Group>().Skip(1).Select(g => int.Parse(g.Value)).ToArray())
            .Select(m => new Movement(m[0], m[1] - 1, m[2] - 1))
            .ToArray(),
        // Create lists representing empty stacks
        new(Enumerable.Range(0, numberOfStacks).Select(_ => new List<char>()))
    );

    // Fill stacks with data. Last line is ignored because it contains stack indexes
    foreach (var line in stackLines[..^1])
    {
        var currentStack = line.AsSpan();
        for (var i = 0; i < numberOfStacks; i++)
        {
            var content = currentStack[1];
            if (content != ' ')
            {
                result.Stacks[i].Insert(0, content);
            }

            if (currentStack.Length > 3)
            {
                currentStack = currentStack[4..];
            }
        }
    }

    return result;
}

record Movement(int Count, int From, int To);
record LogisticsData(Movement[] Movements, List<List<char>> Stacks);

public static partial class Data
{

    [GeneratedRegex(@"move (\d+) from (\d+) to (\d+)")]
    public static partial Regex Movements();

    //  +--- Add your data here
    //  |
    //  V
    public const string Input = """
            [D]    
        [N] [C]    
        [Z] [M] [P]
        1   2   3 

        move 1 from 2 to 1
        move 3 from 1 to 3
        move 2 from 2 to 1
        move 1 from 1 to 2
        """;
}
