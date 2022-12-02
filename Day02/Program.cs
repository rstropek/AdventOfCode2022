const int Rock = 0, Paper = 1, Scissors = 2;
const int Win = 6, Draw = 3, Lose = 0;

// === Part one ===
Console.WriteLine(
    // Split input into lines
    Data.Input.Split('\n')
    // Turn A, B, and C into 0, 1, and 2 as well as X, Y, and Z into 0, 1, and 2
    .Select(l => (Opponent: l[0] - 'A', Me: l[2] - 'X'))
    .Select(l =>
            // Value of current move is move + 1 (rock = 1, paper = 2, scissors = 3)
            l.Me + 1 +
            // Calculate value of game results
            l.Me switch
            {
                Rock => l.Opponent switch { Paper => Lose, Scissors => Win, _ => Draw },
                Paper => l.Opponent switch { Rock => Win, Scissors => Lose, _ => Draw },
                _ => l.Opponent switch { Paper => Win, Rock => Lose, _ => Draw }
            })
        .Sum());


// === Part two ===
const int NeedToLose = 0, NeedToWin = 2;
Console.WriteLine(
    // Split input into lines
    Data.Input.Split('\n')
    // Turn A, B, and C into 0, 1, and 2 as well as X, Y, and Z into 0, 1, and 2
    .Select(l => (Opponent: l[0] - 'A', Result: l[2] - 'X'))
    .Select(l => {
            // Calculate our turn that leads to the wanted result
            var myTurn = l.Opponent switch {
                Rock => l.Result switch { NeedToLose => Scissors, NeedToWin => Paper, _ => l.Opponent },
                Paper => l.Result switch { NeedToLose => Rock, NeedToWin => Scissors, _ => l.Opponent },
                _ => l.Result switch { NeedToLose => Paper, NeedToWin => Rock, _ => l.Opponent },
            };
            // Value of current move is move + 1 (rock = 1, paper = 2, scissors = 3)
            return myTurn + 1 +
            // Calculate value of game results
            myTurn switch
            {
                Rock => l.Opponent switch { Paper => Lose, Scissors => Win, _ => Draw },
                Paper => l.Opponent switch { Rock => Win, Scissors => Lose, _ => Draw },
                _ => l.Opponent switch { Paper => Win, Rock => Lose, _ => Draw }
            };
            })
        .Sum());

public static class Data
{
    //  +--- Add your data here
    //  |
    //  V
    public const string Input = """
        A Y
        B X
        C Z
        """;
}
