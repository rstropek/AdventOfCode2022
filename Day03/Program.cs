// === Part one ===
// Split lines
Console.WriteLine(Data.Input.Split('\n')
    // Split line into halfs
    .Select(line => (line[..(line.Length / 2)], line[((byte)(line.Length / 2))..]))
    // Find char that is in both halfs
    .Select(halfs => halfs.Item1.First(c => halfs.Item2.Contains(c)))
    // Calculate priority
    .Select(c => char.IsAsciiLetterUpper(c) ? (c - 'A' + 27) : (c - 'a' + 1))
    // Sum priorities
    .Sum());

// === Part two ===
// Split lines
Console.WriteLine(Data.Input.Split('\n')
    // Group into blocks of 3
    .Chunk(3)
    // Find char that is in all 3 lines
    .Select(groups => groups.First().Where(c => groups.All(group => group.Contains(c))).First())
    // Calculate priority
    .Select(c => char.IsAsciiLetterUpper(c) ? (c - 'A' + 27) : (c - 'a' + 1))
    // Sum priorities
    .Sum());

public static class Data
{
    //  +--- Add your data here
    //  |
    //  V
    public const string Input = """
        vJrwpWtwJgWrhcsFMMfFFhFp
        jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
        PmmdzqPrVvPwwTWBwg
        wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
        ttgJtRGJQctTZtZT
        CrZsJsPPZsGzwwsLwLmpwMDw
        """;
}
