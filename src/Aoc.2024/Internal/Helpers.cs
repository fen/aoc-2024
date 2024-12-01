namespace Aoc.Internal;

public static partial class Helpers
{
    public static void Deconstruct<T>(this T[] seq, out T first, out T second)
    {
        first = seq[0];
        second = seq.Length == 1 ? default! : seq[1];
    }

    public static void Deconstruct<T>(this T[] seq, out T first, out T second, out T third)
    {
        first = seq[0];
        second = seq.Length == 1 ? default! : seq[1];
        third = seq.Length == 2 ? default! : seq[2];
    }

    public static Task<string[]> ReadAllLinesAsync(this FileInfo file)
    {
        return File.ReadAllLinesAsync(file.FullName);
    }
}
