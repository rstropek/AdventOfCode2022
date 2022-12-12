//  +--- Add your data here
//  |
//  V
const string Input = """
Sabqponm
abcryxxl
accszExk
acctuvwj
abdefghi
""";

var map = Parse(Input);
FillDistances(map, map.End, 0);

// === Part one ===
System.Console.WriteLine(map.Distance[map.Start.Row][map.Start.Col]);

// === Part two ===
var shortest = map.Elevation
    .Select((row, rix) => row.Select((cell, cix) => cell == 0 ? map.Distance[rix][cix] : int.MaxValue).Min())
    .Min();
System.Console.WriteLine(shortest);

void FillDistances(Map map, Pos current, int distance)
{
    // Already found a shorter path
    if (distance >= map.Distance[current.Row][current.Col]) { return; }

    // Store the distance
    map.Distance[current.Row][current.Col] = distance;

    // Search in all directions
    var moves = new (int dRow, int dCol)[] { (0, -1), (0, 1), (-1, 0), (1, 0) };
    foreach(var move in moves)
    {
        var (row, col) = (current.Row + move.dRow, current.Col + move.dCol);
        if (col >= 0 && col < map.Elevation[current.Row].Length && row >= 0 && row < map.Elevation.Length
            && map.Elevation[current.Row][current.Col] - map.Elevation[row][col] <= 1)
        {
            FillDistances(map, new Pos(row, col), distance + 1);
        }
    }
}

Map Parse(string input)
{
    var lines = input.Split('\n');
    var elevation = new int[lines.Length][];
    var distance = new int[lines.Length][];
    var start = new Pos();
    var end = new Pos();
    for (int row = 0; row < lines.Length; row++)
    {
        elevation[row] = new int[lines[row].Length];
        distance[row] = new int[lines[row].Length];
        for (int col = 0; col < lines[row].Length; col++)
        {
            distance[row][col] = int.MaxValue;
            if (lines[row][col] == 'S')
            {
                elevation[row][col] = 'a' - 'a';
                start = new Pos(row, col);

            }
            else if (lines[row][col] == 'E')
            {
                elevation[row][col] = 'z' - 'a';
                end = new Pos(row, col);
            }
            else
            {
                elevation[row][col] = lines[row][col] - 'a';
            }
        }
    }

    return new Map(elevation, distance, start, end);
}

record struct Pos(int Row, int Col);

record Map(int[][] Elevation, int[][] Distance, Pos Start, Pos End);