//  +--- Add your data here
//  |
//  V
const string Input = """
    498,4 -> 498,6 -> 496,6
    503,4 -> 502,4 -> 502,9 -> 494,9
    """;

// Parse input
var stoneNodes = Input.Split('\n')
    .Select(line =>
        line.Split(" -> ")
            .Select(s => s.Split(','))
            .Select(s => (x: int.Parse(s[0]), y: int.Parse(s[1])))
            .ToArray())
    .ToList();

// === Part one ===
{
    var minX = stoneNodes.Min(s => s.Min(p => p.x));
    var maxX = stoneNodes.Max(s => s.Max(p => p.x));
    var maxY = stoneNodes.Max(s => s.Max(p => p.y));
    var width = maxX - minX + 1;

    var canvas = FillCanvas(maxY, width);
    BuildStoneWalls(stoneNodes, minX, canvas);

    var sandCounter = 0;
    var fallen = false;
    while (!fallen)
    {
        var sand = (x: 500 - minX, y: 0);
        var still = false;
        while (!still && sand.y < maxY && sand.x >= 0 && sand.x < canvas[0].Length)
        {
            if (canvas[sand.y + 1][sand.x] == '.') { sand.y++; }
            else if (sand.x == 0) { break; }
            else if (canvas[sand.y + 1][sand.x - 1] == '.')
            {
                sand.x--;
                sand.y++;
            }
            else if (sand.x == canvas[0].Length - 1) { break;}
            else if (canvas[sand.y + 1][sand.x + 1] == '.')
            {
                sand.x++;
                sand.y++;
            }
            else
            {
                canvas[sand.y][sand.x] = 'o';
                still = true;
                sandCounter++;
            }
        }

        if (!still) { fallen = true; }
    }

    Console.WriteLine(sandCounter);
}

// === Part two ===
{
    var minX = 0;
    var maxX = 1000;
    var maxY = stoneNodes.Max(s => s.Max(p => p.y)) + 2;
    var width = maxX - minX + 1;

    stoneNodes.Add(new[] { (x: minX, y: maxY), (x: maxX, y: maxY) });

    var canvas = FillCanvas(maxY, width);
    BuildStoneWalls(stoneNodes, minX, canvas);

    var full = false;
    var sandCounter = 0;
    while (!full)
    {
        var sand = (x: 500 - minX, y: 0);

        var still = false;
        while (!still)
        {
            if (canvas[sand.y + 1][sand.x] == '.') { sand.y++; }
            else if (canvas[sand.y + 1][sand.x - 1] == '.')
            {
                sand.x--;
                sand.y++;
            }
            else if (canvas[sand.y + 1][sand.x + 1] == '.')
            {
                sand.x++;
                sand.y++;
            }
            else
            {
                if (sand.x == 500 && sand.y == 0)
                {
                    // Found result
                    full = true;
                    break;
                }

                canvas[sand.y][sand.x] = 'o';
                still = true;
                sandCounter++;
            }
        }
    }

    Console.WriteLine(sandCounter + 1);
}

static void BuildStoneWalls(IEnumerable<(int x, int y)[]> stoneNodes, int minX, char[][] canvas)
{
    foreach (var s in stoneNodes)
    {
        for (int i = 0; i < s.Length - 1; i++)
        {
            var p1 = s[i];
            var p2 = s[i + 1];
            if (p1.x == p2.x)
            {
                int y;
                for (y = p1.y; y != p2.y; y += Math.Sign(p2.y - p1.y))
                {
                    canvas[y][p1.x - minX] = '#';
                }
                canvas[y][p1.x - minX] = '#';
            }
            else
            {
                int x;
                for (x = p1.x; x != p2.x; x += Math.Sign(p2.x - p1.x))
                {
                    canvas[p1.y][x - minX] = '#';
                }
                canvas[p1.y][x - minX] = '#';
            }
        }
    }
}

static char[][] FillCanvas(int maxY, int width)
{
    var canvas = new char[maxY + 1][];
    for (int i = 0; i < canvas.Length; i++)
    {
        canvas[i] = new char[width];
        for (int j = 0; j < width; j++)
        {
            canvas[i][j] = '.';
        }
    }

    return canvas;
}
