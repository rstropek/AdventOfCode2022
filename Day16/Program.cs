using System.Collections.Concurrent;
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
foreach (var v in valves.Values)
{
    v.Distances = FindAllDistances(v);
}

var numberOfPlayers = 1;

var startTravels = GetTravelOptions(valves["AA"], new HashSet<string>(), numberOfPlayers == 1 ? 30 : 26);
var forkedRealities = RealitiesFromTravelOptions(startTravels, new Reality(Array.Empty<Task>(), new HashSet<string>(), 0, numberOfPlayers == 1 ? 30 : 26)).ToList();

// var q = new ConcurrentQueue<Reality>();
// var consumers



var max = 0;
while (forkedRealities.Any())
{
    var result = Tick(forkedRealities.First());
    if (result > max)
    {
        max = result;
        Console.WriteLine(max);
    }

    forkedRealities.RemoveAt(0);
}

Console.WriteLine(max);

int Tick(Reality reality)
{
    while(reality.RemainingTime >= 1)
    {
        // All valves are open -> no need to continue
        if (valves.All(v => v.Value.FlowRate == 0 || reality.Opened.Contains(v.Key)))
        {
            return reality.Flow;
        }

        // Process whatever the players did in the last minute
        foreach(var player in reality.Players)
        {
            switch (player)
            {
                case Opening o:
                    // Player was opening a valve, now it is open
                    if (reality.Opened.Add(o.Valve.ID))
                    {
                        reality.OpenHistory.Add(o.Valve.ID);
                        reality.Flow += (reality.RemainingTime - 1) * o.Valve.FlowRate;
                    }
                    break;
                case Travelling t:
                    // Player was travelling, the remaining distance is now one less
                    t.RemainingDistance--;
                    break;
                default:
                    break;
            }
        }

        // Check if searching any further does not make sense. 
        var notOpened = valves!.Where(v => !reality.Opened.Contains(v.Key)).ToArray();
        var theoreticalMax = notOpened.Any() ? (reality.RemainingTime - 2) * notOpened.Sum(v => valves![v.Key].FlowRate) : 0;
        if (reality.Flow + theoreticalMax < max)
        {
            return max;
        }

        //reality.Flow += reality.Opened.Select(o => valves[o].FlowRate).Sum();

        reality.RemainingTime--;

        var newPlayers = new List<Task>();
        foreach(var player in reality.Players)
        {
            switch (player)
            {
                case Opening o:
                    // Player was opening a valve, now he can travel on
                    var travels = GetTravelOptions(o.Valve, reality.Opened, reality.RemainingTime - 1);
                    if (travels.Any())
                    {
                        newPlayers.Add(travels.First());
                        forkedRealities.AddRange(RealitiesFromTravelOptions(travels.Skip(1), reality));
                    }
                    break;
                case Travelling t when t.RemainingDistance == 0:
                    if (!reality.Opened.Contains(t.Valve.ID))
                    {
                        newPlayers.Add(new Opening(t.Valve));
                    }
                    var to = GetTravelOptions(t.Valve, reality.Opened, reality.RemainingTime);
                    forkedRealities.AddRange(RealitiesFromTravelOptions(to, reality));
                    break;
                case Travelling t:
                    newPlayers.Add(t);
                    break;
                default:
                    break;
            }
        }

        reality.Players = newPlayers.ToArray();
    }

    Console.WriteLine(string.Join(", ", reality.OpenHistory));

    return reality.Flow;
}

IEnumerable<Travelling> GetTravelOptions(Valve from, HashSet<string> opened, int maxDistance)
{
    return from.Distances
        .Where(d => d.Valve != from && d.Valve.FlowRate > 0 && d.Distance < maxDistance - 1 && !opened.Contains(d.Valve.ID))
        .Select(d => new Travelling(d.Valve) { RemainingDistance = d.Distance });
}

IEnumerable<Reality> RealitiesFromTravelOptions(IEnumerable<Travelling> t, Reality source)
{
    if (numberOfPlayers == 1)
    {
        return t.Select(no => new Reality(new [] { no }, new HashSet<string>(source.Opened), 
            source.Flow, source.RemainingTime) { OpenHistory = new List<string>(source.OpenHistory) });
    }
    else
    {
        return t.SelectMany(no => t.Select(no2 => new Reality(new [] { no, no2 }, new HashSet<string>(source.Opened), 
            source.Flow, source.RemainingTime) { OpenHistory = new List<string>(source.OpenHistory) }));
    }
}

List<(Valve Valve, int distance)> FindAllDistances(Valve from)
{
    var distances = new List<(Valve Valve, int Distance)> { (from, 0) };
    FindAllDistancesImpl(from, distances, 1);
    return distances;
}

void FindAllDistancesImpl(Valve from, List<(Valve Valve, int Distance)> distances, int distance)
{
    foreach (var next in from.NextValves)
    {
        var changed = false;
        if (!distances.Any(d => d.Valve == next))
        {
            distances.Add((next, distance));
            changed = true;
        }
        else
        {
            var existing = distances.First(d => d.Valve == next);
            if (existing.Distance > distance)
            {
                existing.Distance = distance;
                changed = true;
            }
        }

        if (changed) { FindAllDistancesImpl(next, distances, distance + 1); }
    }
}

// // === Part two ===


static partial class Data
{
    [GeneratedRegex(@"Valve ([A-Z]{2}) has flow rate=(\d+); tunnels? leads? to valves? ([A-Z\, ]+)")]
    public static partial Regex InputRegex();
}

record Valve(string ID, int FlowRate, string[] NextValveIDs)
{
    public Valve[] NextValves { get; set; }
    public List<(Valve Valve, int Distance)> Distances { get; set; }
}

abstract record Task(Valve Valve);

record Opening(Valve Valve) : Task(Valve);

record Travelling(Valve Valve) : Task(Valve)
{
    public int RemainingDistance { get; set; }
}

record Reality(HashSet<string> Opened)
{
    public Reality(Task[] Players, HashSet<string> Opened, int Flow, int RemainingTime) : this(Opened)
    {
        this.Flow = Flow;
        this.Players = Players;
        this.RemainingTime = RemainingTime;
    }

    public Task[] Players { get; set; }

    public int Flow { get; set; }

    public int RemainingTime { get; set; }

    public List<string> OpenHistory { get; set; } = new();
}
