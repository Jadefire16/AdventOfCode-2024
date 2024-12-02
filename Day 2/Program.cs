

namespace AdventOfCode_2024.DayTwo;

internal class Program
{
    private static void Main(string[] args)
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
            int count = Parse(line, buffer);

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
                else if(isDecrementing)
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

    private static int Parse(string line, Span<int> buffer)
    {
        ReadOnlySpan<char> span = line.AsSpan();

        int start = 0, count = 0;

        while (start < span.Length && count < buffer.Length)
        {
            int end = span[start..].IndexOf(' ');
            ReadOnlySpan<char> slice;

            if (end == -1)
            {
                slice = span[start..];
                start = span.Length;
            }
            else
            {
                slice = span[start..(start + end)];
                start += end + 1;
            }

            if (int.TryParse(slice, out int number))
                buffer[count++] = number;
        }

        return count;
    }
}
