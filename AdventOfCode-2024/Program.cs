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
        List<int> left = new();
        List<int> right = new();

        string root = AppDomain.CurrentDomain.BaseDirectory;
        string resources = Path.Combine(root, "Resources", "input.txt");
        if (!File.Exists(resources))
            throw new Exception($"Missing Input File At Path: {resources}");

        using StreamReader reader = new(resources);

        string? line = reader.ReadLine();
        if (line is null)
            throw new Exception($"Input file is empty: {resources}");

        while (line != null)
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
            line = reader.ReadLine();
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
}
// Final answer: 1646452