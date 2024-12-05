using System.Diagnostics.CodeAnalysis;
using AdventOfCode_2024.DayThree.Shared;

namespace AdventOfCode_2024.DayThree;
public class PartTwo
{
    private static readonly Dictionary<TokenType, char[]> _validSequences = new()
    {
        { TokenType.MulFunction, new []{'m','u','l','('} },
        { TokenType.DoFunction, new []{'d','o','(',')'} },
        { TokenType.DontFunction, new []{'d','o','n','\'','t','(',')'} }
    };

    public void Run()
    {
        List<IToken> tokens = new();

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
                switch (span[currentIndex])
                {
                    // Mul candidate
                    case 'm':
                        {
                            if (ConsumeMul(span[currentIndex..], out IToken? token, out int consumed))
                            {
                                tokens.Add(token);
                            }
                            currentIndex += consumed;
                            break;
                        }
                    // Do / dont candidate
                    case 'd':
                        {
                            if (ConsumeStateSwitch(span[currentIndex..], out IToken? token, out int consumed))
                            {
                                tokens.Add(token);
                            }
                            currentIndex += consumed;
                            break;
                        }
                    default:
                        currentIndex++;
                        break;
                }
            }
        }

        int accumulator = 0;
        int mulValue = 1;
        foreach (IToken token in tokens)
        {
            switch (token.Type)
            {
                case TokenType.DoFunction:
                    mulValue = 1;
                    break;
                case TokenType.DontFunction:
                    mulValue = 0;
                    break;
                case TokenType.MulFunction:
                    if (token is Token<int> casted)
                        accumulator += casted.GetResult() * mulValue;
                    break;
                default:
                    throw new Exception("Invalid Token Type");
            }
        }

        Console.WriteLine($"\nFinal Accumulated Value: {accumulator}");
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="candidate"></param>
    /// <param name="consumed"></param>
    /// <param name="validationState"></param>
    /// <returns>Null if no state change necessary</returns>
    private static bool ConsumeStateSwitch(ReadOnlySpan<char> candidate, [NotNullWhen(true)] out IToken? token, out int consumed)
    {
        consumed = 0;
        token = default;

        if (ConsumeDont(candidate, out token, out consumed))
        {
            return true;
        }

        if (ConsumeDo(candidate, out token, out consumed))
        {
            return true;
        }

        consumed++;
        return false;
    }

    private static bool ConsumeDo(ReadOnlySpan<char> candidate, [NotNullWhen(true)] out IToken? token, out int consumed)
    {
        consumed = 0;
        token = default;

        if (!candidate.StartsWith(_validSequences[TokenType.DoFunction]))
        {
            consumed++;
            return false;
        }

        Console.WriteLine(candidate[..4].ToString());
        consumed = _validSequences[TokenType.DoFunction].Length - 1;
        token = new DoToken();
        return true;
    }

    private static bool ConsumeDont(ReadOnlySpan<char> candidate, [NotNullWhen(true)] out IToken? token, out int consumed)
    {
        consumed = 0;
        token = default;

        if (!candidate.StartsWith(_validSequences[TokenType.DontFunction]))
        {
            consumed++;
            return false;
        }

        Console.WriteLine(candidate[..7].ToString());
        consumed = _validSequences[TokenType.DontFunction].Length - 1;
        token = new DontToken();
        return true;
    }

    private static bool ConsumeMul(ReadOnlySpan<char> candidate, out IToken token, out int consumed)
    {
        consumed = 0;
        token = default;

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
            Console.WriteLine("mul");
            token = new MulToken(valueA, valueB);
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

    public interface IToken
    {
        public abstract TokenType Type { get; }
    }

    public abstract record Token<T> : IToken
    {
        public abstract T GetResult();
        public abstract TokenType Type { get; }
    }

    public record MulToken : Token<int>
    {
        private int[] values;

        public MulToken(params int[] values)
        {
            this.values = values;
        }

        public override int GetResult()
        {
            int accumulator = values[0];
            for (var i = 1; i < values.Length; i++)
            {
                accumulator *= values[i];
            }

            return accumulator;
        }

        public override TokenType Type => TokenType.MulFunction;
    }

    public record DoToken : Token<bool>
    {
        public override bool GetResult() => true;
        public override TokenType Type => TokenType.DoFunction;

    }

    public record DontToken : Token<bool>
    {
        public override bool GetResult() => false;
        public override TokenType Type => TokenType.DontFunction;

    }
}

// Answer: 170068701