using System.Text.RegularExpressions;

const int LMIN = 0, LMAX = 1, RMIN = 2, RMAX = 3;

var regex = Data.SectionsRegex();

// === Part one ===
// Use regex to find matches
Console.WriteLine(regex.Matches(Data.Input)
    // Parse capture groups into int array
    .Select(m => m.Groups.Cast<Group>().Skip(1).Select(g => int.Parse(g.Value)).ToArray())
    // Count pairs that fully contain each other
    .Count(m => (m[LMIN] >= m[RMIN] && m[LMAX] <= m[RMAX]) || (m[RMIN] >= m[LMIN] && m[RMAX] <= m[LMAX])));

// === Part two ===
// Use regex to find matches
Console.WriteLine(regex.Matches(Data.Input)
    // Parse capture groups into int array
    .Select(m => m.Groups.Cast<Group>().Skip(1).Select(g => int.Parse(g.Value)).ToArray())
    // Count pairs that overlap
    .Count(m => m[LMIN] <= m[RMAX] && m[RMIN] <= m[LMAX]));

public static partial class Data
{

    [GeneratedRegex(@"(\d+)-(\d+),(\d+)-(\d+)")]
    public static partial Regex SectionsRegex();

    //  +--- Add your data here
    //  |
    //  V
    public const string Input = """
        2-4,6-8
        2-3,4-5
        5-7,7-9
        2-8,3-7
        6-6,4-6
        2-6,4-8
        """;
}
