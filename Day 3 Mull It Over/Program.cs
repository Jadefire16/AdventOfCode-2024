namespace AdventOfCode_2024.DayThree;

public class Program
{
    private static readonly Dictionary<TokenType, char[]> _validSequences = new()
    {
        { TokenType.MulFunction, new []{'m','u','l','('} },
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

                Pair? pair = Consume(span[currentIndex..], out int endValue);

                currentIndex += endValue;
                if (pair is null) 
                    continue;

                pairs.Add(pair);
                Console.WriteLine($"Pair: {pair.ValueA} | {pair.ValueB}");
            }
        }

        int accumulator = 0;
        foreach (Pair pair in pairs)
        {
            accumulator += pair.ValueA * pair.ValueB;
        }

        Console.WriteLine($"\nFinal Accumulated Value: {accumulator}");
    }

    private static Pair? Consume(ReadOnlySpan<char> candidate, out int endValue)
    {
        endValue = 0;

        int index = 0, valA = 0, valB = 0;
        Span<char> digitBuffer = stackalloc char[4]; //Assume 4 digits is max

        int mulSequenceLength = _validSequences[TokenType.MulFunction].Length;
        var sequenceBuffer = candidate[..mulSequenceLength];

        if (!SequenceMatches(sequenceBuffer, _validSequences[TokenType.MulFunction]))
        {
            endValue++; // check next character up ("mm" case)
            return null;
        }

        if(!(candidate.Contains('(') && candidate.Contains(',') && candidate.Contains(')')))
        {
            endValue++;
            return null;
        }

        if (candidate[mulSequenceLength - 1] != '(')
        {
            endValue += mulSequenceLength; // Only assume that the mulSequence was valid and nothing after
            return null;
        }

        endValue = mulSequenceLength;

        sequenceBuffer = candidate[endValue..];
        for (; index < sequenceBuffer.Length; index++)
        {
            if (char.IsDigit(sequenceBuffer[index]))
            {
                digitBuffer[index] = sequenceBuffer[index];
            }
            else if (sequenceBuffer[index] == ',')
            {
                valA = int.Parse(digitBuffer[..index]);
                digitBuffer.Clear();
                break;
            }
            else
                return null;
        }

        endValue += index + 1;

        sequenceBuffer = candidate[endValue..];
        for (index = 0; index < sequenceBuffer.Length; index++)
        {
            if (char.IsDigit(sequenceBuffer[index]))
            {
                digitBuffer[index] = sequenceBuffer[index];
            }
            else if (sequenceBuffer[index] == ')')
            {
                valB = int.Parse(digitBuffer[..index]);
                digitBuffer.Clear();
                break;
            }
            else
                return null;
        }

        endValue += index;
        return new Pair(valA, valB);
    }

    private static bool SequenceMatches(ReadOnlySpan<char> sequence, ReadOnlySpan<char> toMatch)
    {
        if (sequence.Length != toMatch.Length)
            return false;
        for (int i = 0; i < sequence.Length; i++)
        {
            if (sequence[i] != toMatch[i])
                return false;
        }

        return true;
    }

    public class Pair
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
        OpenParen,
        CloseParen,
        ArgSeparator
    }
}

// Answer: 170068701