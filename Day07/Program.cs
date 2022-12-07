//  +--- Add your data here
//  |
//  V
const string input = """
    $ cd /
    $ ls
    dir a
    14848514 b.txt
    8504156 c.dat
    dir d
    $ cd a
    $ ls
    dir e
    29116 f
    2557 g
    62596 h.lst
    $ cd e
    $ ls
    584 i
    $ cd ..
    $ cd ..
    $ cd d
    $ ls
    4060174 j
    8033020 d.log
    5626152 d.ext
    7214296 k
    """;

// Split lines and ignore the first line ("cd /)
var lines = input.Split("\n")[1..];

// Setup data structure to gather folder sizes
var folders = new Dictionary<string, int>();

// Start recursive function to gather folder sizes
var totalSize = GetSize("");

// === Part one ===
Console.WriteLine(folders.Where(f => f.Value <= 100000).Sum(f => f.Value));

// === Part two ===
var required = 30_000_000 - (70_000_000 - totalSize);
Console.WriteLine(folders.Where(f => f.Value >= required).OrderBy(f => f.Value).First().Value);

int GetSize(string cwd)
{
    var size = 0;
    while (lines.Length > 0)
    {
        var line = lines[0];

        // Handle change of directory
        if (line.StartsWith("$ cd "))
        {
            if (line.EndsWith(" .."))
            {
                // Reached the end of current directory -> add size of dictionary
                folders.Add(cwd, size);

                // Goto next command and exit loop to go back to parent directory
                lines = lines[1..];
                break;
            }
            else
            {
                // Found a new subdirectory
                var newDir = line[5..];

                // Goto next command and recursively call function to get size of subdirectory
                lines = lines[1..];
                size += GetSize($"{cwd}/{newDir}");
                continue;
            }
        }
        // If the current command is not cd and not ls and not dir, it must be a file size.
        else if (!line.StartsWith("$ ls") && !line.StartsWith("dir "))
        {
            // Extract file size and add it to the current directory size
            var lineSpan = line.AsSpan();
            var indexOfBlank = lineSpan.IndexOf(' ');
            size += int.Parse(lineSpan[..indexOfBlank]);
        }

        // Goto next command
        if (lines.Length > 0) { lines = lines[1..]; }
    }

    // We have reached the end of the list of commands. We need to add the
    // last directory listing to the list of directory sizes.
    if (lines.Length == 0 && size > 0)
    {
        folders.Add(string.IsNullOrEmpty(cwd) ? "/" : cwd, size);
    }

    return size;
}
