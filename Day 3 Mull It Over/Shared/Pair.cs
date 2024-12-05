namespace AdventOfCode_2024.DayThree.Shared;
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
