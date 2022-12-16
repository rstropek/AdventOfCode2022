//  +--- Add your data here
//  |
//  V
using System.Diagnostics;
using System.Text.RegularExpressions;

const string TestInput = """
Sensor at x=2, y=18: closest beacon is at x=-2, y=15
Sensor at x=9, y=16: closest beacon is at x=10, y=16
Sensor at x=13, y=2: closest beacon is at x=15, y=3
Sensor at x=12, y=14: closest beacon is at x=10, y=16
Sensor at x=10, y=20: closest beacon is at x=10, y=16
Sensor at x=14, y=17: closest beacon is at x=10, y=16
Sensor at x=8, y=7: closest beacon is at x=2, y=10
Sensor at x=2, y=0: closest beacon is at x=2, y=10
Sensor at x=0, y=11: closest beacon is at x=2, y=10
Sensor at x=20, y=14: closest beacon is at x=25, y=17
Sensor at x=17, y=20: closest beacon is at x=21, y=22
Sensor at x=16, y=7: closest beacon is at x=15, y=3
Sensor at x=14, y=3: closest beacon is at x=15, y=3
Sensor at x=20, y=1: closest beacon is at x=15, y=3
""";

const string Input = """
Sensor at x=3772068, y=2853720: closest beacon is at x=4068389, y=2345925
Sensor at x=78607, y=2544104: closest beacon is at x=-152196, y=4183739
Sensor at x=3239531, y=3939220: closest beacon is at x=3568548, y=4206192
Sensor at x=339124, y=989831: closest beacon is at x=570292, y=1048239
Sensor at x=3957534, y=2132743: closest beacon is at x=3897332, y=2000000
Sensor at x=1882965, y=3426126: closest beacon is at x=2580484, y=3654136
Sensor at x=1159443, y=3861139: closest beacon is at x=2580484, y=3654136
Sensor at x=2433461, y=287013: closest beacon is at x=2088099, y=-190228
Sensor at x=3004122, y=3483833: closest beacon is at x=2580484, y=3654136
Sensor at x=3571821, y=799602: closest beacon is at x=3897332, y=2000000
Sensor at x=2376562, y=1539540: closest beacon is at x=2700909, y=2519581
Sensor at x=785113, y=1273008: closest beacon is at x=570292, y=1048239
Sensor at x=1990787, y=38164: closest beacon is at x=2088099, y=-190228
Sensor at x=3993778, y=3482849: closest beacon is at x=4247709, y=3561264
Sensor at x=3821391, y=3986080: closest beacon is at x=3568548, y=4206192
Sensor at x=2703294, y=3999015: closest beacon is at x=2580484, y=3654136
Sensor at x=1448314, y=2210094: closest beacon is at x=2700909, y=2519581
Sensor at x=3351224, y=2364892: closest beacon is at x=4068389, y=2345925
Sensor at x=196419, y=3491556: closest beacon is at x=-152196, y=4183739
Sensor at x=175004, y=138614: closest beacon is at x=570292, y=1048239
Sensor at x=1618460, y=806488: closest beacon is at x=570292, y=1048239
Sensor at x=3974730, y=1940193: closest beacon is at x=3897332, y=2000000
Sensor at x=2995314, y=2961775: closest beacon is at x=2700909, y=2519581
Sensor at x=105378, y=1513086: closest beacon is at x=570292, y=1048239
Sensor at x=3576958, y=3665667: closest beacon is at x=3568548, y=4206192
Sensor at x=2712265, y=2155055: closest beacon is at x=2700909, y=2519581
""";

const int findY = 2000000;

var regex = Data.Sensors();
var positions = regex.Matches(Input)
    .Select(m => m.Groups.Cast<Group>().Skip(1).Select(g => int.Parse(g.Value)).ToArray())
    .Select(g => new Sensor(new Pos(g[0], g[1]), new Pos(g[2], g[3])))
    .ToArray();

System.Console.WriteLine();

// === Part one ===
// var cannot = new HashSet<int>();
// foreach(var s in positions)
// {
//     var d = Math.Abs(s.Position.Y - findY);
//     if (d <= findY)
//     {
//         for (var i = (s.Distance - d) * (-1); i < s.Distance - d; i++)
//         {
//             cannot.Add(s.Position.X + i);
//         }
//     }
// }

// positions.Where(s => s.Beacon.Y == 10).Select(s => s.Beacon.X).ToList().ForEach(b => cannot.Add(b));

// System.Console.WriteLine(cannot.Count);


// === Part two ===
const int MIN = 0;
//const int MAX = 20;
const int MAX = 4000000;



// for (int y = MIN; y <= MAX; y++)
// {
//     for (int x = MIN; x <= MAX; x++)
//     {
//         var covered = false;
//         foreach (var s in positions)
//         {
//             if (s.Distance >= Math.Abs(s.Position.X - x) + Math.Abs(s.Position.Y - y))
//             {
//                 covered = true;
//                 break;
//             }
//         }

//         if (!covered)
//         {
//             System.Console.WriteLine($"({x},{y}) = {x * 4000000 + y}");
//         }
//     }
//     System.Console.WriteLine(y);
// }


//const int MAX = 4000000;
var cannot = new HashSet<Pos>();
// // positions.Where(s => s.Beacon.X is >= MIN and <= MAX && s.Beacon.Y is >= MIN and <= MAX).ToList().ForEach(b => cannot.Add(b.Beacon));
// // positions.Where(s => s.Position.X is >= MIN and <= MAX && s.Position.Y is >= MIN and <= MAX).ToList().ForEach(b => cannot.Add(b.Position));
// foreach (var s in positions)
// {
//     for (int y = MIN; y <= MAX; y++)
//     {
//         var d = Math.Abs(s.Position.Y - y);
//         if (s.Distance >= d)
//         {
//             for (var i = (s.Distance - d) * (-1); i <= s.Distance - d; i++)
//             {
//                 var pos = new Pos(s.Position.X + i, y);
//                 if (pos.X is >= MIN and <= MAX && pos.Y is >= MIN and <= MAX)
//                 {
//                     cannot.Add(pos);
//                 }
//             }
//         }
//     }
//     Debugger.Break();
//     //Print();
//     Count();
//     Console.ReadKey();
// }

var cannot2 = new List<List<(int from, int to)>>();
for (int y = MIN; y <= MAX; y++)
{
    var currentY = new List<(int from, int to)>();
    cannot2.Add(currentY);
    foreach (var s in positions)
    {
        var d = Math.Abs(s.Position.Y - y);
        if (s.Distance >= d)
        {
            var newArea = (
                Math.Max(0, s.Position.X + (s.Distance - d) * (-1)),
                Math.Min(MAX, s.Position.X + s.Distance - d)
            );

            currentY.Add(newArea);
        }
    }
}

for (int y = MIN; y <= MAX; y++)
{
    cannot2[y].Sort((a, b) => a.from.CompareTo(b.from));
    bool collapsed;
    do
    {
        collapsed = false;
        for (int i = 0; i < cannot2[y].Count - 1;)
        {
            var current = cannot2[y][i];
            var next = cannot2[y][i + 1];
            if ((next.from >= current.from && next.from <= current.to) || (next.to >= current.from && next.to <= current.to))
            {
                collapsed = true;
                cannot2[y][i] = (
                    Math.Min(current.from, next.from),
                    Math.Max(current.to, next.to)
                );
                cannot2[y].RemoveAt(i + 1);
            }
            else
            {
                i++;
            }
        }
    }
    while (collapsed);
}

System.Console.WriteLine("DONE");

for (var y = MIN; y <= MAX; y++)
{
    if (cannot2[y].Count == 2)
    {
        long x = cannot2[y][1].from - 1;
        System.Console.WriteLine($"({x},{y}) = {x * 4000000 + y}");
    }
}

return;

var cnt = 0;
Parallel.For(MIN, MAX + 1, y =>
{
    for (int x = MIN; x <= MAX; x++)
    {
        var covered = false;
        foreach (var (min, max) in cannot2[y])
        {
            if (x >= min && x <= max)
            {
                covered = true;
                break;
            }
        }

        if (!covered)
        {
            System.Console.WriteLine($"({x},{y}) = {x * 4000000 + y}");
            return;
        }
    }
    cnt++;
    if (cnt % 1000 == 0) System.Console.WriteLine(cnt);

    //System.Console.WriteLine(y);
});

// for (int y = MIN; y <= MAX; y++)
// {
//     for (int x = MIN; x <= MAX; x++)
//     {
//         if (!cannot.Contains(new Pos(x, y)))
//         {
//             System.Console.WriteLine($"({x},{y}) = {x * 4000000 + y}");
//             return;
//         }
//     }
// }

void Count()
{
    var c = 0;
    for (int y = MIN; y <= MAX; y++)
    {
        for (int x = MIN; x <= MAX; x++)
        {
            if (!cannot.Contains(new Pos(x, y))) c++;
        }
    }

    System.Console.WriteLine(c);
}

void Print()
{
    for (int y = MIN; y <= MAX; y++)
    {
        for (int x = MIN; x <= MAX; x++)
        {
            if (positions.Any(s => s.Position.X == x && s.Position.Y == y))
            {
                System.Console.Write("S");
            }
            else
            if (cannot.Contains(new Pos(x, y)))
            {
                System.Console.Write("#");
            }
            else
            {
                System.Console.Write(".");
            }
        }
        System.Console.WriteLine();
    }
    System.Console.WriteLine("\n");
}

record struct Pos(int X, int Y);
record struct Sensor(Pos Position, Pos Beacon)
{
    public int Distance => Math.Abs(Position.X - Beacon.X) + Math.Abs(Position.Y - Beacon.Y);
}

static partial class Data
{
    [GeneratedRegex(@"Sensor at x=(\-?\d+), y=(\-?\d+): closest beacon is at x=(\-?\d+), y=(\-?\d+)")]
    public static partial Regex Sensors();
}
