using System.Numerics;

//  +--- Add your data here
//  |
//  V
const string Input = """
    R 4
    U 4
    L 3
    D 1
    R 4
    D 1
    L 5
    R 2
    """;

// Parse input into tuples of (direction, distance)
var movements = Input.Split("\n")
    .Select(l => l.Split(' '))
    .Select(lp => new Movement(lp[0][0], int.Parse(lp[1])))
    .ToArray();

// === Part one ===
Position head = new(0, 0), tail = new(0, 0);

var visited = new HashSet<Position>();
visited.Add(tail);

// Iterate over all movements
foreach (var m in movements)
{
    // Get the delta for a single step of this movement
    var delta = m.GetDelta();

    // Repeat delta for the distance of this movement
    for (int i = 0; i < m.Distance; i++)
    {
        // Move the head
        head += delta;

        // Calculate distance between head and tail. If it's far, move the tail in the direction of the head.
        var dist = head - tail;
        if (dist.IsFar) { tail += dist.GetDirection(); }

        // Add the tail to the set of visited positions
        visited.Add(tail);
    }
}

System.Console.WriteLine(visited.Count);

// === Part two ===
const int RopeLength = 10;
const int HeadIx = RopeLength - 1;
const int TailIx = 0;
var knots = new Position[RopeLength];

visited = new();
visited.Add(knots[TailIx]);

// Iterate over all movements
foreach (var m in movements)
{
    // Get the delta for a single step of this movement
    var delta = m.GetDelta();

    // Repeat delta for the distance of this movement
    for (int i = 0; i < m.Distance; i++)
    {
        // Move the head
        knots[HeadIx] += delta;

        // Move all other knots of the rope
        for (int knot = HeadIx; knot > 0; knot--)
        {
            // Calculate distance between current and follower. If it's far, move the follower in the direction of the current.
            var dist = knots[knot] - knots[knot - 1];
            if (dist.IsFar) { knots[knot - 1] += dist.GetDirection(); }
        }

        visited.Add(knots[TailIx]);
    }
}

System.Console.WriteLine(visited.Count);

// === Helper Records ===
readonly record struct Delta(int Dx, int Dy)
{
    /// <summary>
    /// Returns true if the delta is more than one step in any direction.
    /// </summary>
    public bool IsFar => Math.Abs(this.Dx) > 1 || Math.Abs(this.Dy) > 1;

    /// <summary>
    /// Returns a new delta with length of 1 (sign of delta).
    /// </summary>
    public Delta GetDirection() => new(Math.Sign(this.Dx), Math.Sign(this.Dy));
}

readonly record struct Movement(char Direction, int Distance)
{
    /// <summary>
    /// Returns the delta for a single step of this movement.
    /// </summary>
    public Delta GetDelta() => this.Direction switch
    {
        'U' => new(0, -1),
        'D' => new(0, 1),
        'L' => new(-1, 0),
        'R' => new(1, 0),
        _ => throw new Exception("Invalid movement, this should NEVER happen")
    };
}

readonly record struct Position(int X, int Y)
    : IAdditionOperators<Position, Delta, Position>,
        ISubtractionOperators<Position, Position, Delta>
{
    public static Position operator +(Position left, Delta delta)
        => new(left.X + delta.Dx, left.Y + delta.Dy);

    public static Delta operator -(Position left, Position right)
        => new(left.X - right.X, left.Y - right.Y);
}
