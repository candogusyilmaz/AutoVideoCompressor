namespace AutoVideoCompressor.Application;
public interface ILogger
{
    void Info(string message);
    void Final(string message);
}
