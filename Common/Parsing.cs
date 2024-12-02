namespace AdventOfCode_2024.Common;
public class Parsing
{
    /// <summary>
    /// Parses a sequence of numbers from a string given a single delimiter
    /// </summary>
    /// <param name="line"></param>
    /// <param name="buffer"></param>
    /// <param name="delimiter"></param>
    /// <returns></returns>
    public static int ParseNumber(string line, Span<int> buffer, char delimiter)
    {
        ReadOnlySpan<char> span = line.AsSpan();

        int start = 0, count = 0;

        while (start < span.Length && count < buffer.Length)
        {
            int end = span[start..].IndexOf(delimiter);
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
