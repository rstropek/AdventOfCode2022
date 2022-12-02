// === Part one ===
Console.WriteLine(
    // Split data into blocks
    Data.Input.Split("\n\n")
        .Select(e => 
            // Split block into lines
            e.Split('\n')
                // Convert each line's string into number
                .Select(int.Parse)
                // Calculate sum of block
                .Sum())
        // Get maximum value across all blocks
        .Max());

// === Part two ===
Console.WriteLine(
    // Split data into blocks
    Data.Input.Split("\n\n")
        .Select(e => 
            // Split block into lines
            e.Split('\n')
                // Convert each line's string into number
                .Select(int.Parse)
                // Calculate sum of block
                .Sum())
        // Sort sum of blocks descending
        .OrderDescending()
        // Take first three blocks
        .Take(3)
        // Calculate sum of first three blocks
        .Sum());

public static class Data
{
    //  +--- Add your data here
    //  |
    //  V
    public const string Input = """
        1000
        2000
        3000

        4000

        5000
        6000

        7000
        8000
        9000

        10000
        """;
}