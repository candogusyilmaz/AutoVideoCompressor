using AutoVideoCompressor.Application;

namespace AutoVideoCompressor;
internal class ConsoleLogger : ILogger
{
    public void Info(string message)
    {
        Console.WriteLine($"INF: {message}");
    }

    public void Final(string message)
    {
        Console.WriteLine($"FNL: {message}");
    }
}
