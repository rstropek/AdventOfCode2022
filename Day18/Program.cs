using System.Numerics;

//  +--- Add your data here
//  |
//  V
const string Cubes = """
    2,2,2
    1,2,2
    3,2,2
    2,1,2
    2,3,2
    2,2,1
    2,2,3
    2,2,4
    2,2,6
    1,2,5
    3,2,5
    2,1,5
    2,3,5
    """;

var Movements = new[]
{
    new Pos(1, 0, 0),
    new Pos(-1, 0, 0),
    new Pos(0, 1, 0),
    new Pos(0, -1, 0),
    new Pos(0, 0, 1),
    new Pos(0, 0, -1)
};

// === Parse ===
var cubes = new HashSet<Pos>(Cubes.Split("\n").Select(l =>
{
    var line = l.Split(',');
    return new Pos(int.Parse(line[0]), int.Parse(line[1]), int.Parse(line[2]));
}));

// === Part one ===
Console.WriteLine(cubes.Count * 6 - cubes.Select(c => Movements.Select(m => (c, m)).Count(t => cubes.Contains(t.c + t.m))).Sum());

// === Part two ===
var minX = cubes.Min(c => c.X) - 1;
var maxX = cubes.Max(c => c.X) + 1;
var minY = cubes.Min(c => c.Y) - 1;
var maxY = cubes.Max(c => c.Y) + 1;
var minZ = cubes.Min(c => c.Z) - 1;
var maxZ = cubes.Max(c => c.Z) + 1;

var invertedCubes = new HashSet<Pos>(Enumerable.Range(minX, maxX - minX + 1)
    .SelectMany(x => Enumerable.Range(minY, maxY - minY + 1)
        .SelectMany(y => Enumerable.Range(minZ, maxZ - minZ + 1)
            .Select(z => new Pos(x, y, z)))));

var reachableCubes = new HashSet<Pos>(invertedCubes.Count);
var previouslyAdded = new HashSet<Pos>(reachableCubes);
var added = new HashSet<Pos>(new[] { new Pos(0, 0, 0) });
do
{
    foreach (var c in added)
    {
        reachableCubes.Add(c);
        invertedCubes.Remove(c);
    }

    previouslyAdded = added;
    added = new HashSet<Pos>();

    foreach (var pa in previouslyAdded)
    {
        foreach (var m in Movements)
        {
            var check = new Pos(pa.X + m.X, pa.Y + m.Y, pa.Z + m.Z);
            if (invertedCubes.Contains(check) && !cubes.Contains(check)) added.Add(check);
        }
    }
} while (added.Count > 0);

Console.WriteLine(reachableCubes.Select(c => Movements.Select(m => (c, m)).Count(t => cubes.Contains(t.c + t.m))).Sum());

record struct Pos(int X, int Y, int Z) : IAdditionOperators<Pos, Pos, Pos>
{
    public static Pos operator +(Pos left, Pos right)
        => new Pos(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
}