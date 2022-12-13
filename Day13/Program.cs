//  +--- Add your data here
//  |
//  V
const string Input = """
[1,1,3,1,1]
[1,1,5,1,1]

[[1],[2,3,4]]
[[1],4]

[9]
[[8,7,6]]

[[4,4],4,4]
[[4,4],4,4,4]

[7,7,7,7]
[7,7,7]

[]
[3]

[[[]]]
[[]]

[1,[2,[3,[4,[5,6,7]]]],8,9]
[1,[2,[3,[4,[5,6,0]]]],8,9]
""";

var data = Parse(Input);
var comparer = new ItemComparer();

// === Part one ===
Console.WriteLine(data.Select((d, ix) => (comparer.Compare(d.Item1, d.Item2), ix + 1))
    .Where(d => d.Item1 == -1)
    .Sum(d => d.Item2));

// === Part two ===
var div2 = new Item(null, new List<Item> { new Item(null, new List<Item> { new Item(2, null) }) });
var div6 = new Item(null, new List<Item> { new Item(null, new List<Item> { new Item(6, null) }) });
var ordered = data
    .SelectMany(d => new[] { d.Item1, d.Item2 })
    .Union(new[] { div2, div6 })
    .Order(comparer)
    .ToList();
Console.WriteLine((ordered.IndexOf(div2) + 1) * (ordered.IndexOf(div6) + 1));

IReadOnlyList<(Item, Item)> Parse(ReadOnlySpan<char> inputFull)
{
    /// <summary>
    /// Parses a pair of items separated by a newline.
    /// </summary>
    static (Item, Item) ParsePair(ReadOnlySpan<char> inputPair)
    {
        var itemSplitIx = inputPair.IndexOf('\n');
        var left = inputPair[..itemSplitIx];
        var right = inputPair[(itemSplitIx + 1)..];
        return (ParseItem(ref left), ParseItem(ref right));
    }

    /// <summary>
    /// Parses an item (number of array)
    /// </summary>
    static Item ParseItem(ref ReadOnlySpan<char> input)
    {
        if (char.IsAsciiDigit(input[0]))
        {
            // Found a number, parse it until we hit a non-digit.
            int val;
            for (val = 0; char.IsAsciiDigit(input[0]); input = input[1..])
            {
                val = val * 10 + (input[0] - '0');
            }

            return new Item(val, null);
        }

        if (input[0] == '[')
        {
            // Found an array, parse it as long as we hit commas.
            var result = new List<Item>();
            do
            {
                input = input[1..];
                if (input[0] != ']')
                {
                    // Recursively parse the next item.
                    var val = ParseItem(ref input);
                    result.Add(val);
                }
            } while (input[0] == ',');

            // Jump over the closing bracket.
            input = input[1..];
            return new Item(null, result);
        }

        // This should never happen
        throw new InvalidOperationException();
    }

    // Parse all pairs separated by two newlines.
    var result = new List<(Item, Item)>();
    for (var pairSplitIx = inputFull.IndexOf("\n\n"); pairSplitIx != -1; pairSplitIx = inputFull.IndexOf("\n\n"))
    {
        result.Add(ParsePair(inputFull));
        inputFull = inputFull[(pairSplitIx + 2)..];
    }

    result.Add(ParsePair(inputFull));

    return result;
}

record Item(int? Value, List<Item>? Values);

class ItemComparer : IComparer<Item>
{
    public int Compare(Item? item1, Item? item2)
    {
        ArgumentNullException.ThrowIfNull(item1);
        ArgumentNullException.ThrowIfNull(item2);
        
        if (item1.Value.HasValue && item2.Value.HasValue)
        {
            return item1.Value == item2.Value ? 0 : item1.Value > item2.Value ? 1 : -1;
        }
        else if (item1.Values != null && item2.Values != null)
        {
            int i = 0;
            for (; i < item1.Values.Count && i < item2.Values.Count; i++)
            {
                var compareResult = this.Compare(item1.Values[i], item2.Values[i]);
                if (compareResult != 0)
                {
                    return compareResult;
                }
            }

            if (i == item1.Values.Count && i == item2.Values.Count)
            {
                return 0;
            }

            return i == item1.Values.Count ? -1 : 1;
        }
        else
        {
            var i1 = item1.Value.HasValue ? new List<Item> { new Item(item1.Value, null) } : item1.Values;
            var i2 = item2.Value.HasValue ? new List<Item> { new Item(item2.Value, null) } : item2.Values;
            return this.Compare(new Item(null, i1), new Item(null, i2));
        }
    }
}
