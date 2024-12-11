namespace Aoc.Day11;

public class Solution1 : ISolver
{
    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        var lines = await inputFile.ReadAllLinesAsync();
        var stones =lines[0].Split(' ').ToList();
        var blinks = 25;

        for (var blink = 0; blink < blinks; blink++)
        {
            List<string> newStones = [];

            foreach (var stone in stones)
            {
                if (stone == "0")
                {
                    newStones.Add("1");
                }
                else if (stone.Length % 2 == 0)
                {
                    var mid = stone.Length / 2;
                    var left = stone.Substring(0, mid).TrimStart('0');
                    var right = stone.Substring(mid).TrimStart('0');

                    if (string.IsNullOrEmpty(left)) left = "0";
                    if (string.IsNullOrEmpty(right)) right = "0";

                    newStones.Add(left);
                    newStones.Add(right);
                }
                else
                {
                    var num = long.Parse(stone);
                    newStones.Add((num * 2024).ToString());
                }
            }

            stones = newStones;
        }

        return stones.Count.ToString();
    }
}
