//  +--- Add your data here
//  |
//  V
const string Input = """
    30373
    25512
    65332
    33549
    35390
    """;

// Split input
var trees = Input.Split("\n").Select(l => l.ToArray().Select(c => int.Parse(c.ToString())).ToArray()).ToArray();
var sideLength = trees.Length;

// === Part one ===
var visibleTrees = new HashSet<(int, int)>();
for (var i = 0; i < sideLength; i++)
{
    for (int j = 0, side1Max = -1, side2Max = -1, side3Max = -1, side4Max = -1; j < sideLength; j++)
    {
        if (trees[i][j] > side1Max) // From left
        {
            side1Max = trees[i][j];
            visibleTrees.Add((i, j));
        }

        var colRight = sideLength - 1 - j; // From right
        if (trees[i][colRight] > side2Max)
        {
            side2Max = trees[i][colRight];
            visibleTrees.Add((i, colRight));
        }

        if (trees[j][i] > side3Max) // From top
        {
            side3Max = trees[j][i];
            visibleTrees.Add((j, i));
        }

        var rowBottom = sideLength - 1 - j; // From bottom
        if (trees[rowBottom][i] > side4Max)
        {
            side4Max = trees[rowBottom][i];
            visibleTrees.Add((rowBottom, i));
        }
    }
}

System.Console.WriteLine(visibleTrees.Count());

// === Part two ===
var scenicMax = 0;
for (var row = 0; row < sideLength; row++)
{
    for (var col = 0; col < sideLength; col++)
    {
        int viewUp = 0, viewDown = 0, viewLeft = 0, viewRight = 0;

        // Look in all directions
        for (var row2 = row - 1; row2 >= 0; row2--) { viewUp++; if (trees[row2][col] >= trees[row][col]) break; }
        for (var row2 = row + 1; row2 < sideLength; row2++) { viewDown++; if (trees[row2][col] >= trees[row][col]) break; }
        for (var col2 = col - 1; col2 >= 0; col2--) { viewLeft++; if (trees[row][col2] >= trees[row][col]) break; }
        for (var col2 = col + 1; col2 < sideLength; col2++) { viewRight++; if (trees[row][col2] >= trees[row][col]) break; }

        // Calculate scenic value
        scenicMax = Math.Max(scenicMax, viewUp * viewDown * viewLeft * viewRight);
    }
}

System.Console.WriteLine(scenicMax);
