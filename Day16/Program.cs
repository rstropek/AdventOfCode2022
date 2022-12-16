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
Valve AA has flow rate=0; tunnels lead to valves RZ, QQ, FH, IM, VJ
Valve FE has flow rate=0; tunnels lead to valves TM, TR
Valve QZ has flow rate=19; tunnels lead to valves HH, OY
Valve TU has flow rate=17; tunnels lead to valves NJ, IN, WN
Valve RG has flow rate=0; tunnels lead to valves IK, SZ
Valve TM has flow rate=0; tunnels lead to valves FE, JH
Valve JH has flow rate=4; tunnels lead to valves NW, QQ, TM, VH, AZ
Valve NW has flow rate=0; tunnels lead to valves JH, OB
Valve BZ has flow rate=0; tunnels lead to valves XG, XF
Valve VS has flow rate=0; tunnels lead to valves FF, GC
Valve OI has flow rate=20; tunnel leads to valve SY
Valve IK has flow rate=0; tunnels lead to valves RG, TR
Valve RO has flow rate=0; tunnels lead to valves UZ, YL
Valve LQ has flow rate=0; tunnels lead to valves IZ, PA
Valve GG has flow rate=18; tunnels lead to valves GH, VI
Valve NJ has flow rate=0; tunnels lead to valves TU, UZ
Valve SY has flow rate=0; tunnels lead to valves OI, ZL
Valve HH has flow rate=0; tunnels lead to valves QZ, WN
Valve RZ has flow rate=0; tunnels lead to valves AA, UZ
Valve OF has flow rate=0; tunnels lead to valves YL, IZ
Valve IZ has flow rate=9; tunnels lead to valves OF, FH, VH, JZ, LQ
Valve OB has flow rate=0; tunnels lead to valves UZ, NW
Valve AH has flow rate=0; tunnels lead to valves FF, ZL
Valve ZL has flow rate=11; tunnels lead to valves SY, VI, AH
Valve BF has flow rate=0; tunnels lead to valves PA, YL
Valve OH has flow rate=0; tunnels lead to valves CU, JZ
Valve VH has flow rate=0; tunnels lead to valves IZ, JH
Valve AZ has flow rate=0; tunnels lead to valves JC, JH
Valve XG has flow rate=0; tunnels lead to valves BZ, PA
Valve OY has flow rate=0; tunnels lead to valves PZ, QZ
Valve IM has flow rate=0; tunnels lead to valves FM, AA
Valve GO has flow rate=0; tunnels lead to valves VJ, TR
Valve YL has flow rate=8; tunnels lead to valves JC, RO, OF, BF, FM
Valve TY has flow rate=0; tunnels lead to valves SZ, TS
Valve UZ has flow rate=5; tunnels lead to valves OB, NJ, RO, RZ, GC
Valve XF has flow rate=21; tunnel leads to valve BZ
Valve RY has flow rate=0; tunnels lead to valves TR, FF
Valve QQ has flow rate=0; tunnels lead to valves JH, AA
Valve TS has flow rate=0; tunnels lead to valves TY, FF
Valve GC has flow rate=0; tunnels lead to valves VS, UZ
Valve JC has flow rate=0; tunnels lead to valves AZ, YL
Valve JZ has flow rate=0; tunnels lead to valves IZ, OH
Valve IN has flow rate=0; tunnels lead to valves TH, TU
Valve FM has flow rate=0; tunnels lead to valves IM, YL
Valve FH has flow rate=0; tunnels lead to valves AA, IZ
Valve VJ has flow rate=0; tunnels lead to valves AA, GO
Valve TH has flow rate=0; tunnels lead to valves CU, IN
Valve TR has flow rate=7; tunnels lead to valves FE, IK, RY, GO
Valve GH has flow rate=0; tunnels lead to valves GG, FF
Valve SZ has flow rate=10; tunnels lead to valves RG, TY
Valve PA has flow rate=16; tunnels lead to valves XG, LQ, BF
Valve PZ has flow rate=0; tunnels lead to valves CU, OY
Valve VI has flow rate=0; tunnels lead to valves ZL, GG
Valve CU has flow rate=22; tunnels lead to valves PZ, OH, TH
Valve WN has flow rate=0; tunnels lead to valves TU, HH
Valve FF has flow rate=13; tunnels lead to valves VS, RY, AH, TS, GH
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
var maxPreassures = new Dictionary<MaxPreassureCache, int>();
var max = Release(valves["AA"], new(), 0);

System.Console.WriteLine(max);
//System.Console.WriteLine(maxPreassures[new("AA", 0)]);

int Release(Valve v, HashSet<string> opened, int elapsedMinutes)
{
    if (maxPreassures.TryGetValue(new(v.ID, string.Join("", opened.Order()), elapsedMinutes), out var cachedMax))
    {
        return cachedMax;
    }

    if (elapsedMinutes >= 29)
    {
        return 0;
    }

    int VisitNexts(Valve v, HashSet<string> opened, int elapsedMinutes)
    {
        var max = 0;
        foreach (var next in v.NextValves)
        {
            var r = Release(next, opened, elapsedMinutes);
            if (r > max) { max = r; }
        }

        return max;
    }

    var maxWithoutOpening = VisitNexts(v, opened, elapsedMinutes + 1);
    var maxWithOpening = 0;

    // Does it make sense to open the valve
    if (v.FlowRate > 0 && !opened.Contains(v.ID))
    {
        var newOpened = new HashSet<string>(opened);
        newOpened.Add(v.ID);
        opened = newOpened;

        var flowingTime = 30
            - 1 // Flow will start in one minute
            - elapsedMinutes; // Reduce flow time by time already over
        maxWithOpening = v.FlowRate * flowingTime;

        maxWithOpening += VisitNexts(v, opened, elapsedMinutes + 2);
    }

    var newMax = Math.Max(maxWithoutOpening, maxWithOpening);

    maxPreassures[new(v.ID, string.Join("", opened.Order()), elapsedMinutes)] = newMax;

    return newMax;
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

record struct MaxPreassureCache(string ID, string Opened, int ElapsedMinutes);
