const string filePath = "Romeo and Juliet by William Shakespeare.txt";

try
{
    var wordCount = Counter.CountWords(filePath);
    Console.WriteLine($"The file contains {wordCount} words.");
}
catch (Exception ex)
{
    Console.WriteLine($"Error processing the file: {ex.Message}");
}

public static class Counter
{
    private static readonly char[] Separator = [' ', '\r', '\n', '\t'];

    // I'm using File.ReadLines here, which is a better option than File.ReadAllLines
    // It reads the file lazily, line by line. Suitable for very large files and can handle files much larger than the available system memory.
    public static long CountWords(string filePath)
    {
        var lines = File.ReadLines(filePath);
        long wordsCount = 0;

        Parallel.ForEach(lines, () => 0L, (line, state, localCount) =>
            {
                localCount += CountWordsInLine(line);
                return localCount;
            },
            localCount => Interlocked.Add(ref wordsCount, localCount));

        return wordsCount;
    }

    private static long CountWordsInLine(string line)
    {
        return string.IsNullOrWhiteSpace(line) ? 0 : line.Split(Separator, StringSplitOptions.RemoveEmptyEntries).Length;
    }
}
