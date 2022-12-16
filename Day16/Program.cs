using System.Text.RegularExpressions;

//  +--- Add your data here
//  |
//  V
const string TestInput = """
Valve AA has flow rate=0; tunnels lead to valves DD, II, BB
Valve BB has flow rate=13; tunnels lead to valves CC, AA
Valve CC has flow rate=2; tunnels lead to valves DD, BB
Valve DD has flow rate=20; tunnels lead to valves CC, AA, EE
Valve EE has flow rate=3; tunnels lead to valves FF, DD
Valve FF has flow rate=0; tunnels lead to valves EE, GG
Valve GG has flow rate=0; tunnels lead to valves FF, HH
Valve HH has flow rate=22; tunnel leads to valve GG
Valve II has flow rate=0; tunnels lead to valves AA, JJ
Valve JJ has flow rate=21; tunnel leads to valve II
""";

const string Input = """

""";

var regex = Data.InputRegex();

var valves = regex.Matches(TestInput)
    .Select(m => m.Groups.Cast<Group>().Skip(1).ToArray())
    .Select(g => new Valve(g[0].Value, int.Parse(g[1].Value), g[2].Value.Split(", ")))
    .ToDictionary(g => g.ID, g => g);
foreach (var v in valves.Values)
{
    v.NextValves = v.NextValveIDs.Select(id => valves[id]).ToArray();
}

System.Console.WriteLine();

// === Part one ===
var maxPreassure = 0;
Release(valves["AA"], new(), new(), 0, 0);

// var analyze = preassureRelease
//     .Where(p => p.path.Count >= 4 && p.path[0] == "DD" && p.path[1] == "BB" && p.path[1] == "JJ" && p.path[1] == "HH" && p.path[1] == "EE" && p.path[1] == "CC")
//     .ToArray();

Console.WriteLine(maxPreassure);

void Release(Valve v, List<string> visited, List<string> opened, int elapsedMinutes, int releasedPreassure)
{
    if (elapsedMinutes >= 28)
    {
        if (maxPreassure < releasedPreassure)
        {
            System.Console.WriteLine(elapsedMinutes);
            maxPreassure = releasedPreassure;
        }
        return;
    }

    var newVisited = new List<string>();
    newVisited.AddRange(visited);
    newVisited.Add(v.ID);
    visited = newVisited;

    if (v.FlowRate > 0)
    {
        var newOpened = new List<string>();
        newOpened.AddRange(opened);
        newOpened.Add(v.ID);
        opened = newOpened;

        var newReleasedPreassure = releasedPreassure + v.FlowRate * (30 - 1 - elapsedMinutes);

        if ((newReleasedPreassure + valves.Where(v => !opened.Contains(v.Key)).Sum(v => v.Value.FlowRate * (30 - 2 - elapsedMinutes))) > maxPreassure)
        {
            //preassureRelease.Add((opened, newReleasedPreassure));
            foreach (var next in v.NextValves)
            {
                // if (!opened.Contains(next.ID))
                {
                    Release(next, visited, opened, elapsedMinutes + 2, newReleasedPreassure);
                }
            }
        }
    }

    //preassureRelease.Add((newPath, releasedPreassure));
    if ((releasedPreassure + valves.Where(v => !opened.Contains(v.Key)).Sum(v => v.Value.FlowRate * (30 - 2 - elapsedMinutes))) > maxPreassure)
    {
        foreach (var next in v.NextValves)
        {
            // if (!opened.Contains(next.ID))
            {
                Release(next, visited, opened, elapsedMinutes + 1, releasedPreassure);
            }
        }
    }
}

// === Part two ===


static partial class Data
{
    [GeneratedRegex(@"Valve ([A-Z]{2}) has flow rate=(\d+); tunnels? leads? to valves? ([A-Z\, ]+)")]
    public static partial Regex InputRegex();
}

record Valve(string ID, int FlowRate, string[] NextValveIDs)
{
    public Valve[] NextValves { get; set; }
}