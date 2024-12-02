using System.IO;
using System.Text;

namespace AdventOfCode_2024;

/// <summary>
/// Day 1: Historian Hysteria
/// Goal is to sort two lists of integer values by size and accumulate the absolute distances between them
/// </summary>
public class Program
{
    private static void Main(string[] args)
    {
        //PartOne();
        PartTwo();
    }

    private static void PartOne()
    {
        List<int> left = new();
        List<int> right = new();

        string root = AppDomain.CurrentDomain.BaseDirectory;
        string resources = Path.Combine(root, "Resources", "input.txt");
        if (!File.Exists(resources))
            throw new Exception($"Missing Input File At Path: {resources}");

        using StreamReader reader = new(resources);

        while (reader.ReadLine() is { } line)
        {
            ReadOnlySpan<char> span = line.AsSpan();
            int lastSpace = line.LastIndexOf(" ", StringComparison.Ordinal);

            try
            {
                left.Add(int.Parse(span[..(lastSpace - 2)]));
                right.Add(int.Parse(span[(lastSpace + 1)..]));
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to parse values", ex);
            }
        }

        if (left.Count != right.Count)
            throw new Exception("Input size is not the same");

        left.Sort();
        right.Sort();

        int finalValue = 0;
        for (int i = 0; i < left.Count; i++)
        {
            finalValue += Math.Abs(right[i] - left[i]);
        }
        Console.WriteLine($"Final Accumulated Value: {finalValue}");
    }

    private static void PartTwo()
    {

        Dictionary<int, Key> values = new();

        string root = AppDomain.CurrentDomain.BaseDirectory;
        string resources = Path.Combine(root, "Resources", "input.txt");
        if (!File.Exists(resources))
            throw new Exception($"Missing Input File At Path: {resources}");

        using StreamReader reader = new(resources);

        while (reader.ReadLine() is { } line)
        {
            ReadOnlySpan<char> span = line.AsSpan();
            int lastSpace = line.LastIndexOf(" ", StringComparison.Ordinal);

            try
            {
                int lValue = int.Parse(span[..(lastSpace - 2)]);
                int rValue = int.Parse(span[(lastSpace + 1)..]);
                
                if(!values.TryAdd(lValue, new Key(1, 0)))
                    values[lValue].KeyCount++;

                if(!values.TryGetValue(rValue, out Key? value))
                    values.Add(rValue, new Key(0, 1));
                else
                    value.ValueCount++;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to parse values", ex);
            }
        }

        int accumulator = 0;

        foreach (var value in values)
        {
            accumulator += ((value.Key * value.Value.KeyCount) * value.Value.ValueCount);
        }

        Console.WriteLine($"Final Accumulated Value: {accumulator}");
    }
    private record Key
    {
        public int KeyCount { get; set; }
        public int ValueCount { get; set; }

        public Key(int keyCount, int valueCount)
        {
            KeyCount = keyCount;
            ValueCount = valueCount;
        }
    }
}
// Final answer part one: 1646452
// Final answer part two: 23609874