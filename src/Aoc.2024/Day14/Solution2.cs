using System.Text;

namespace Aoc.Day14;

public class Solution2 : ISolver
{
    const int width = 101;
    const int height = 103;

    private readonly StringBuilder _sb = new();

    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        var input = await File.ReadAllLinesAsync(inputFile.FullName);

        return Simulation(input)
            .TakeWhile(robots => !Plot(robots).Contains("########"))
            .Count()
            .ToString();
    }

    private static IEnumerable<Robot[]> Simulation(string[] robotData)
    {
        var robots = ParseInput(robotData).ToArray();

        var time = 0;

        while (true)
        {
            yield return robots.Select(r => Step(r, time)).ToArray();
            time++;
        }
    }

    private static Robot Step(Robot robot, int time)
    {
        var (px, py) = robot.Position;
        var (vx, vy) = robot.Velocity;
        var x_t = (px + vx * time) % width;
        var y_t = (py + vy * time) % height;

        // Ensure positions are positive (modulo can result in negative values)
        if (x_t < 0) x_t += width;
        if (y_t < 0) y_t += height;

        return robot with { Position = new Vector(x_t, y_t) };
    }

    private string Plot(IEnumerable<Robot> robots)
    {
        var res = new char[height, width];
        foreach (var robot in robots)
        {
            res[robot.Position.Y, robot.Position.X] = '#';
        }

        _sb.Clear();

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                _sb.Append(res[y, x] == '#' ? "#" : " ");
            }

            _sb.AppendLine();
        }

        return _sb.ToString();
    }

    private static IEnumerable<Robot> ParseInput(string[] robotData)
    {
        foreach (var line in robotData)
        {
            var parts = line.Split(' ');
            var positionParts = parts[0][2..].Split(',');
            var velocityParts = parts[1][2..].Split(',');

            var px = int.Parse(positionParts[0]);
            var py = int.Parse(positionParts[1]);
            var vx = int.Parse(velocityParts[0]);
            var vy = int.Parse(velocityParts[1]);

            yield return new Robot(new Vector(px, py), new Vector(vx, vy));
        }
    }


    record struct Vector(int X, int Y);
    record struct Robot(Vector Position, Vector Velocity);
}
