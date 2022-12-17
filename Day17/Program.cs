const string Items = """
####

.#.
###
.#.

..#
..#
###

#
#
#
#

##
##
""";

//  +--- Add your data here
//  |
//  V
const string jetInput = """
    >>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>
    """;

var items = Items.Split("\n\n").Select(l => l.Split("\n").Select(l => l.ToArray()).ToArray()).ToArray();

// === Part one ===
var solution = Solve(2022);
Console.WriteLine(solution);

// === Part two ===
solution = Solve(1_000_000_000_000);
System.Console.WriteLine(solution);


// === Helper Functions ===
char[] NewLine() => Enumerable.Range(0, 7).Select(_ => '.').ToArray();

static bool CanGoTo(List<char[]> pixel, char[][] item, Pos targetPos)
{
    if (targetPos.X < 0 || (targetPos.X + item[0].Length) > 7 || targetPos.Y < 0)
    {
        return false;
    }

    for (int y = 0; y < item.Length; y++)
    {
        for (int x = 0; x < item[y].Length; x++)
        {
            if (item[y][x] != '.' && pixel[targetPos.Y - y][targetPos.X + x] != '.')
            {
                return false;
            }
        }
    }

    return true;
}

static void Draw(List<char[]> pixel, char[][] item, Pos targetPos)
{
    for (int y = 0; y < item.Length; y++)
    {
        for (int x = 0; x < item[y].Length; x++)
        {
            if (item[y][x] != '.')
            {
                pixel![targetPos.Y - y][targetPos.X + x] = item[y][x];
            }
        }
    }
}

long Solve(long desiredHeight)
{
    var tower = new List<char[]>();
    for (int i = 0; i < 3; i++) { tower.Add(NewLine()); }

    var states = new Dictionary<State, CountState>();
    State? dupl = null;

    var currentItemIx = 0;
    var currentJetIx = 0;
    var heightCorrection = 0L;
    for (long rocks = 0; rocks < desiredHeight; rocks++)
    {
        int firstEmptyLine;
        for (firstEmptyLine = 0; firstEmptyLine < tower.Count && tower[firstEmptyLine].Any(l => l != '.'); firstEmptyLine++) ;

        var state = GetState(tower, currentItemIx, currentJetIx, firstEmptyLine);
        if (dupl == null)
        {
            if (states.ContainsKey(state))
            {
                states[state] = new(rocks - states[state].Rocks, tower.Count - states[state].Height);
                dupl = state;
            }
            else
            {
                states.Add(state, new(rocks, tower.Count));
            }
        }
        else
        {
            if (dupl == state)
            {
                var repetations = (desiredHeight - rocks) / states[state].Rocks;
                rocks = rocks + repetations * states[state].Rocks;
                heightCorrection = repetations * states[state].Height;
            }
        }

        var currentItem = items[currentItemIx];
        currentItemIx = (currentItemIx + 1) % items.Length;

        while (tower.Count <= firstEmptyLine + 2 + currentItem.Length) { tower.Add(NewLine()); }

        var itemPosLeftUpper = new Pos(2, firstEmptyLine + 2 + currentItem.Length);

        var reachedEnd = false;
        while (!reachedEnd)
        {
            var dx = jetInput[currentJetIx] switch
            {
                '>' => 1,
                '<' => -1,
                _ => throw new NotImplementedException()
            };
            currentJetIx = (currentJetIx + 1) % jetInput.Length;

            var newPosLeftUpper = new Pos(itemPosLeftUpper.X + dx, itemPosLeftUpper.Y);
            if (CanGoTo(tower, currentItem, newPosLeftUpper)) { itemPosLeftUpper = newPosLeftUpper; }

            newPosLeftUpper = new Pos(itemPosLeftUpper.X, itemPosLeftUpper.Y - 1);
            if (CanGoTo(tower, currentItem, newPosLeftUpper)) { itemPosLeftUpper = newPosLeftUpper; }
            else { reachedEnd = true; }
        }

        Draw(tower, currentItem, itemPosLeftUpper);

    }

    for (var i = tower.Count - 1; i >= 0 && tower[i].All(l => l == '.'); i--)
    {
        tower.RemoveAt(i);
    }

    return tower.Count + heightCorrection;
}

State GetState(List<char[]> tower, int itemIx, int jetIx, int firstEmptyLine)
{
    var result = new State(0, 0, 0, 0, 0, 0, 0, itemIx, jetIx);
    if (firstEmptyLine == 0)
    {
        return result;
    }

    var found = 0;
    for (var i = 1; found < 7 && firstEmptyLine - i > 0; i++)
    {
        var line = tower[firstEmptyLine - i];
        if (line[0] != '.' && result.Dx1 == 0) { result.Dx1 = i; found++; }
        if (line[1] != '.' && result.Dx2 == 0) { result.Dx2 = i; found++; }
        if (line[2] != '.' && result.Dx3 == 0) { result.Dx3 = i; found++; }
        if (line[3] != '.' && result.Dx4 == 0) { result.Dx4 = i; found++; }
        if (line[4] != '.' && result.Dx5 == 0) { result.Dx5 = i; found++; }
        if (line[5] != '.' && result.Dx6 == 0) { result.Dx6 = i; found++; }
        if (line[6] != '.' && result.Dx7 == 0) { result.Dx7 = i; found++; }
    }

    return result;
}

record struct Pos(int X, int Y);

record struct State(int Dx1, int Dx2, int Dx3, int Dx4, int Dx5, int Dx6, int Dx7, int ItemIx, int JetIx);

record struct CountState(long Rocks, long Height);
