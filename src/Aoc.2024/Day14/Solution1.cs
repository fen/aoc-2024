namespace Aoc.Day14;

public class Solution1 : ISolver
{
    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        var input = await File.ReadAllLinesAsync(inputFile.FullName);
        var width = 101;
        var height = 103;
        var seconds = 100;

        return CalculateSafetyFactor(input, width, height, seconds).ToString();
    }

    static int CalculateSafetyFactor(string[] robotData, int width, int height, int time)
    {
        // Initialize quadrant counters
        int Q1 = 0, Q2 = 0, Q3 = 0, Q4 = 0;

        // Process each robot data line
        foreach (var line in robotData)
        {
            // Parse position and velocity
            var parts = line.Split(' ');
            var positionParts = parts[0][2..].Split(',');
            var velocityParts = parts[1][2..].Split(',');

            var px = int.Parse(positionParts[0]);
            var py = int.Parse(positionParts[1]);
            var vx = int.Parse(velocityParts[0]);
            var vy = int.Parse(velocityParts[1]);

            // Compute new position after time has elapsed
            var x_t = (px + vx * time) % width;
            var y_t = (py + vy * time) % height;

            // Ensure positions are positive (modulo can result in negative values)
            if (x_t < 0) x_t += width;
            if (y_t < 0) y_t += height;

            // Classify into quadrants
            if (x_t == 50 || y_t == 51)
            {
                // Skip robots at the exact middle
                continue;
            }

            switch (x_t)
            {
                case < 50 when y_t < 51:
                    Q1++;
                    break;
                case > 50 when y_t < 51:
                    Q2++;
                    break;
                case < 50 when y_t > 51:
                    Q3++;
                    break;
                case > 50 when y_t > 51:
                    Q4++;
                    break;
            }
        }

        var safetyFactor = Q1 * Q2 * Q3 * Q4;
        return safetyFactor;
    }
}
