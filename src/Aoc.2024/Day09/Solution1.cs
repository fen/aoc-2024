namespace Aoc.Day09;

public class Solution1 : ISolver
{
    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        var lines = await inputFile.ReadAllLinesAsync();

        return DiskCompactor.CompactDiskAndCalculateChecksum(lines[0]).ToString();
    }
}

sealed class DiskCompactor
{
    public static long CompactDiskAndCalculateChecksum(string diskMap)
    {
        var blocks = ParseDiskBlocks(diskMap);

        CompactBlocks(blocks);

        return CalculateChecksum(blocks);
    }

    private static List<char> ParseDiskBlocks(string diskMap)
    {
        var blocks = new List<char>();
        var fileId = 0;

        for (var i = 0; i < diskMap.Length; i += 2)
        {
            if (i < diskMap.Length)
            {
                var fileLength = diskMap[i] - '0';
                if (fileLength > 0)
                {
                    blocks.AddRange(new string((char)('0' + fileId), fileLength));
                }
            }

            if (i + 1 < diskMap.Length)
            {
                var freeSpace = diskMap[i + 1] - '0';
                blocks.AddRange(new string('.', freeSpace));
            }

            fileId++;
        }

        return blocks;
    }

    private static void CompactBlocks(List<char> blocks)
    {
        var lastIndex = blocks.Count - 1;

        // Move from rightmost file block to leftmost free space
        while (true)
        {
            // Find the rightmost file block
            while (lastIndex >= 0 && blocks[lastIndex] == '.')
            {
                lastIndex--;
            }

            // Find the leftmost free space
            var freeIndex = 0;
            while (freeIndex < blocks.Count && blocks[freeIndex] != '.')
            {
                freeIndex++;
            }

            if (lastIndex <= freeIndex || freeIndex == blocks.Count)
            {
                break; // No more moves possible
            }

            // Move the file block to the free space
            blocks[freeIndex] = blocks[lastIndex];
            blocks[lastIndex] = '.';
            lastIndex--;
        }
    }

    private static long CalculateChecksum(List<char> blocks)
    {
        long checksum = 0;

        for (var i = 0; i < blocks.Count; i++)
        {
            if (blocks[i] != '.')
            {
                var fileId = blocks[i] - '0';
                checksum += i * fileId;
            }
        }

        return checksum;
    }
}
