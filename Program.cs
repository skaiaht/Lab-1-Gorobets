using System.Diagnostics;

namespace Lab1;

internal static class Program
{
    private const int VectorCount = 30000;
    private const int VectorSize = 100;
    private const int MinValue = -1000;
    private const int MaxValue = 1000;
    private static int[][] _vectors = new int[VectorCount][];
    private static readonly int[] Sums = new int[VectorCount];
    private static readonly ThreadLocal<Random> ThreadLocalRandom = new(() => new Random());

    private static void Main()
    {
        MeasurePerformance(1);
        MeasurePerformance(2);
        MeasurePerformance(4);
    }

    private static void MeasurePerformance(int threadCount)
    {
        _vectors = new int[VectorCount][];
        for (var i = 0; i < VectorCount; i++) _vectors[i] = new int[VectorSize];
        
        Console.WriteLine($"Filling vectors using {threadCount} thread(s)...");
        var fillStopwatch = Stopwatch.StartNew();
        ParallelFillVectors(threadCount);
        fillStopwatch.Stop();
        Console.WriteLine($"Filling completed in {fillStopwatch.ElapsedMilliseconds} ms.");
        
        Console.WriteLine($"Calculating sums using {threadCount} thread(s)...");
        var sumStopwatch = Stopwatch.StartNew();
        ParallelSumVectors(threadCount);
        sumStopwatch.Stop();
        Console.WriteLine($"Sum calculation completed in {sumStopwatch.ElapsedMilliseconds} ms.\n");
    }

    private static void ParallelFillVectors(int threadCount)
    {
        var options = new ParallelOptions { MaxDegreeOfParallelism = threadCount };
        Parallel.For(0, VectorCount, options, i =>
        {
            var random = ThreadLocalRandom.Value;
            for (var j = 0; j < VectorSize; j++) _vectors[i][j] = random!.Next(MinValue, MaxValue + 1);
        });
    }

    private static void ParallelSumVectors(int threadCount)
    {
        var options = new ParallelOptions { MaxDegreeOfParallelism = threadCount };
        Parallel.For(0, VectorCount, options, i =>
        {
            var sum = 0;
            for (var j = 0; j < VectorSize; j++) sum += _vectors[i][j];
            Sums[i] = sum;
        });
    }
}