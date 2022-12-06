//          +--- Add your data here
//          |
//          V
var data = "mjqjpqmgbljsphdztnvjfqwrcgsmlb";

// === Part one ===
Console.WriteLine(FindDistinct(data, 4));

// === Part two ===
Console.WriteLine(FindDistinct(data, 14));


int FindDistinct(string data, int length)
{
    for (var i = length - 1; i < data.Length; i++)
    {
        var sub = data[(i - length + 1)..(i + 1)];
        if (sub.Distinct().Count() == length)
        {
            return i + 1;
        }
    }

    throw new InvalidOperationException();
}
