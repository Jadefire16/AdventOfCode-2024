using static AdventOfCode_2024.DayThree.Program;

namespace AdventOfCode_2024.DayThree;

public class Program
{
    private static readonly Dictionary<TokenType, char[]> _validSequences = new()
    {
        { TokenType.MulFunction, new []{'m','u','l','('} },
        { TokenType.DoFunction, new []{'d','o','(',')'} },
        { TokenType.DontFunction, new []{'d','o','n','t','(',')'} }
    };


    public static void Main(string[] args)
    {
        HashSet<Pair> pairs = new();

        string root = AppDomain.CurrentDomain.BaseDirectory;
        string resources = Path.Combine(root, "Resources", "input.txt");
        if (!File.Exists(resources))
            throw new Exception($"Missing Input File At Path: {resources}");

        using StreamReader reader = new(resources);

        while (reader.ReadLine() is { } line)
        {
            ReadOnlySpan<char> span = line.AsSpan();
            int currentIndex = 0;

            while (currentIndex < span.Length)
            {
                if (span[currentIndex] != 'm')
                {
                    currentIndex++;
                    continue;
                }

                if (Consume(span[currentIndex..], out Pair pair, out int consumed))
                {
                    pairs.Add(pair);
                    Console.WriteLine($"Pair: {pair.ValueA} | {pair.ValueB}");
                }
                currentIndex += consumed;
            }

        }

        int accumulator = 0;
        foreach (Pair pair in pairs)
        {
            accumulator += pair.ValueA * pair.ValueB;
        }

        Console.WriteLine($"\nFinal Accumulated Value: {accumulator}");
    }

    private static bool Consume(ReadOnlySpan<char> candidate, out Pair pair, out int consumed)
    {
        consumed = 0;
        pair = default;

        if (!candidate.StartsWith(_validSequences[TokenType.MulFunction]))
        {
            consumed++;
            return false;
        }

        consumed += _validSequences[TokenType.MulFunction].Length;
        candidate = candidate[(consumed - 1)..]; // Off by one so the indexof '(' doesnt fail

        int openParenIndex = candidate.IndexOf('(');
        int separatorIndex = candidate.IndexOf(',');
        int closeParenIndex = candidate.IndexOf(')');

        //Sanity check if the symbols are in the right order and if they exist
        if (openParenIndex == -1 || separatorIndex == -1 || closeParenIndex == -1 || openParenIndex > separatorIndex || separatorIndex > closeParenIndex)
        {
            return false;
        }

        if (TryParseNumber(candidate[1..separatorIndex], out int valueA) && TryParseNumber(candidate[(separatorIndex + 1)..(closeParenIndex)], out int valueB))
        {
            pair = new Pair(valueA, valueB);
            consumed += closeParenIndex;
            return true;
        }

        return false;
    }

    private static bool TryParseNumber(ReadOnlySpan<char> input, out int result)
    {
        result = 0;
        for (int i = 0; i < input.Length; i++)
        {
            if (!char.IsDigit(input[i]))
                return false;
            // Fancy hack I found to convert a sequence of characters into an int
            result = result * 10 + (input[i] - '0');
        }
        return true;
    }


    public readonly struct Pair
    {
        public int ValueA { get; }
        public int ValueB { get; }

        public Pair(int valueA, int valueB)
        {
            ValueA = valueA;
            ValueB = valueB;
        }
    }

    public enum TokenType
    {
        MulFunction,
        DoFunction,
        DontFunction
    }
}

// Answer: 170068701