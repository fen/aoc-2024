namespace Aoc.Day09;

using FilePositions = Dictionary<char, (int start, int end)>;

public class Solution2 : ISolver
{
    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        var lines = await inputFile.ReadAllLinesAsync();

        return CompactDiskAndCalculateChecksum(lines[0]).ToString();
    }

    const char FreeSpace = '.';

    private static long CompactDiskAndCalculateChecksum(string diskMap)
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
        var filePositions = GetFilePositions(blocks, FreeSpace);

        // Extract file IDs sorted in descending order
        var fileIds = new List<char>(filePositions.Keys);
        fileIds.Sort((x, y) => y.CompareTo(x));

        // Move files in decreasing order of file ID
        foreach (var fileId in fileIds)
        {
            var (fileStart, fileEnd) = filePositions[fileId];
            var fileLength = fileEnd - fileStart + 1;

            MoveFileToFreeSpace(blocks, fileLength, fileStart, fileId);
        }
    }

    private static FilePositions GetFilePositions(List<char> blocks, char freeSpace)
    {
        var filePositions = new FilePositions();
        for (var i = 0; i < blocks.Count; i++)
        {
            var current = blocks[i];
            if (current != freeSpace)
            {
                if (!filePositions.TryGetValue(current, out var position))
                {
                    filePositions[current] = (i, i);
                }
                else
                {
                    filePositions[current] = (position.start, i);
                }
            }
        }

        return filePositions;
    }

    private static void MoveFileToFreeSpace(List<char> blocks, int fileLength, int fileStart, char fileId)
    {
        var searchEndLimit = blocks.Count;

        for (var position = 0; position <= searchEndLimit - fileLength; position++)
        {
            if (IsSpaceAvailable(blocks, position, fileLength) && position < fileStart)
            {
                MoveFile(blocks, fileLength, fileStart, position, fileId);
                break;
            }
        }
    }

    private static void MoveFile(List<char> blocks, int fileLength, int fileStart, int newPosition, char fileId)
    {
        for (var offset = 0; offset < fileLength; offset++)
        {
            blocks[newPosition + offset] = fileId;
            blocks[fileStart + offset] = FreeSpace;
        }
    }

    private static bool IsSpaceAvailable(List<char> blocks, int start, int length)
    {
        for (var offset = 0; offset < length; offset++)
        {
            var currentBlock = blocks[start + offset];
            if (currentBlock != FreeSpace)
            {
                return false;
            }
        }

        return true;
    }

    private static long CalculateChecksum(List<char> blocks)
    {
        long checksum = 0;

        for (var i = 0; i < blocks.Count; i++)
        {
            if (blocks[i] != FreeSpace)
            {
                var fileId = blocks[i] - '0';
                checksum += (long)i * fileId;
            }
        }

        return checksum;
    }
}
