using AdventOfCode_2024.Common;

namespace AdventOfCode_2024.DayTwo;

internal class Program
{
    private static void Main(string[] args)
    {
        // Part One();
        // PartTwo();
    }

    /// <summary>
    /// Go through each sequence of numbers and determine if they have an irregular sequence
    /// Rule 1: Sequences must always increment or decrement and cannot change mid-way through
    /// Rule 2: Difference must be greater than 0
    /// </summary>
    /// <exception cref="Exception"></exception>
    private static void PartOne()
    {
        string root = AppDomain.CurrentDomain.BaseDirectory;
        string resources = Path.Combine(root, "Resources", "input.txt");
        if (!File.Exists(resources))
            throw new Exception($"Missing Input File At Path: {resources}");

        using StreamReader reader = new(resources);

        Span<int> buffer = stackalloc int[16];
        buffer.Fill(0); // just sanity checking in case the allocated memory isn't cleared

        int safeReports = 0;
        int unsafeReports = 0;

        while (reader.ReadLine() is { } line)
        {
            int count = Parsing.ParseNumber(line, buffer, ' ');

            bool isDecrementing = buffer[0] > buffer[1];
            bool isValid = true;

            for (int i = 0; i < count - 1; i++)
            {
                Console.Write($"{buffer[i]},");
                int difference = buffer[i + 1] - buffer[i];
                if (difference == 0)
                {
                    isValid = false;
                    break;
                }
                else if (isDecrementing)
                {
                    if (buffer[i] >= buffer[i + 1] && difference >= -3)
                        continue;

                    isValid = false;
                    break;
                }
                else
                {
                    if (buffer[i] <= buffer[i + 1] && difference <= 3)
                        continue;

                    isValid = false;
                    break;
                }
            }
            Console.Write($"{buffer[count - 1]}");
            Console.WriteLine();

            if (isValid)
                safeReports++;
            else
                unsafeReports++;

            buffer.Fill(0);
        }

        Console.WriteLine($"Safe Reports: {safeReports}\nInvalid Reports: {unsafeReports}");
    }

    /// <summary>
    /// Essentially just go through all sequences, and accept any with only a single failure
    /// </summary>
    /// <exception cref="Exception"></exception>
    private static void PartTwo()
    {
        string root = AppDomain.CurrentDomain.BaseDirectory;
        string resources = Path.Combine(root, "Resources", "input.txt");
        if (!File.Exists(resources))
            throw new Exception($"Missing Input File At Path: {resources}");

        using StreamReader reader = new(resources);

        Span<int> buffer = stackalloc int[16];
        buffer.Fill(0); // just sanity checking in case the allocated memory isn't cleared

        int safeReports = 0;
        int unsafeReports = 0;

        while (reader.ReadLine() is { } line)
        {
            int count = Parsing.ParseNumber(line, buffer, ' ');

            bool isDecrementing = buffer[0] > buffer[1];
            int failures = 0;

            for (int i = 0; i < count - 1; i++)
            {
                int difference = buffer[i + 1] - buffer[i];
                if (difference == 0)
                {
                    failures++;
                }
                else if (isDecrementing)
                {
                    if (buffer[i] >= buffer[i + 1] && difference >= -3)
                        continue;

                    failures++;
                }
                else
                {
                    if (buffer[i] <= buffer[i + 1] && difference <= 3)
                        continue;

                    failures++;
                }
            }

            if (failures <= 1)
                safeReports++;
            else
                unsafeReports++;

            buffer.Fill(0);
        }

        Console.WriteLine($"Safe Reports: {safeReports}\nInvalid Reports: {unsafeReports}");
    }
}
