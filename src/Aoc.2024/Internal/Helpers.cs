namespace Aoc.Internal;

public static partial class Helpers
{
    public static void Deconstruct<T>(this T[] seq, out T first, out T second)
    {
        first = seq[0];
        second = seq.Length == 1 ? default! : seq[1];
    }

    public static void Deconstruct<T>(this IList<T> seq, out T first, out T second)
    {
        first = seq.First();
        second = seq.ElementAtOrDefault(1)!;
    }

    public static void Deconstruct<T>(this T[] seq, out T first, out T second, out T third)
    {
        first = seq[0];
        second = seq.Length == 1 ? default! : seq[1];
        third = seq.Length == 2 ? default! : seq[2];
    }

    public static void Deconstruct<T>(this IList<T> seq, out T first, out T second, out T third)
    {
        first = seq.First();
        second = seq.ElementAtOrDefault(1)!;
        third = seq.ElementAtOrDefault(2)!;
    }

    public static void Deconstruct<T>(this IList<T> seq, out T first, out T second, out T third, out T fourth)
    {
        first = seq.First();
        second = seq.ElementAtOrDefault(1)!;
        third = seq.ElementAtOrDefault(2)!;
        fourth = seq.ElementAtOrDefault(3)!;
    }

    public static async Task<char[][]> ReadCharMapAsync(this FileInfo file)
    {
        var lines =  await File.ReadAllLinesAsync(file.FullName);

        var charArray = new char[lines.Length][];
        for (var i = 0; i < lines.Length; i++)
        {
            charArray[i] = lines[i].ToCharArray();
        }

        return charArray;
    }

    public static Task<string[]> ReadAllLinesAsync(this FileInfo file)
    {
        return File.ReadAllLinesAsync(file.FullName);
    }

    public static Task<string> ReadAllTextAsync(this FileInfo file)
    {
        return File.ReadAllTextAsync(file.FullName);
    }

    /// <summary>
    /// Returns an enumerable sequence of tuples where each tuple contains two consecutive elements from the input sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="seq">The input sequence.</param>
    /// <returns>An enumerable sequence of tuples where each tuple contains two consecutive elements from the input sequence.</returns>
    /// <remarks>
    /// The method returns tuples of the form <c>(T First, T Second)</c> where <c>First</c> corresponds to the first element
    /// and <c>Second</c> corresponds to the second element.
    /// The method internally uses an enumerator to iterate through the input sequence, retrieving two consecutive elements
    /// at a time until there are no more elements left. If the input sequence is empty or has an odd number of elements,
    /// no tuple will be returned.
    /// </remarks>
    public static IEnumerable<(T first, T second)> ByTwo<T>(this IEnumerable<T> seq)
    {
        using var enumerator = seq.GetEnumerator();
        do
        {
            if (!enumerator.MoveNext())
            {
                yield break;
            }

            var first = enumerator.Current;
            enumerator.MoveNext();
            var second = enumerator.Current;
            yield return (first, second);
        } while (true);
    }

    /// <summary>
    /// Generates adjacent pairs of elements from a given sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="seq">The sequence of elements.</param>
    /// <returns>An enumerable of tuples representing adjacent pairs of elements.</returns>
    public static IEnumerable<(T first, T second)> AdjacentPairs<T>(this IEnumerable<T> seq) {
        using var enumerator = seq.GetEnumerator();
        enumerator.MoveNext();
        var first = enumerator.Current;
        do {
            if (!enumerator.MoveNext()) {
                yield break;
            }

            var second = enumerator.Current;
            yield return (first, second);
            first = second;
        } while (true);
    }
}
