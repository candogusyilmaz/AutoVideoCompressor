using Microsoft.Extensions.Configuration;

using System.Diagnostics;

namespace AutoVideoCompressor.Application;
public sealed class CompressManager
{
    private readonly CompressorConfiguration _config;
    private readonly ILogger _logger;

    public CompressManager(IConfiguration configuration, ILogger logger)
    {
        _config = configuration.Get<CompressorConfiguration>();
        _logger = logger;
    }

    public async Task Compress(string filename)
    {
        using var ffmpegProcess = new Process();
        ffmpegProcess.StartInfo = GetProcessStartInfo(filename);

        ffmpegProcess.Start();
        _logger.Info($"File compressing started. [{Path.GetFileName(filename)}]");
        await ffmpegProcess.WaitForExitAsync();
        _logger.Final($"Compressing completed. [{Path.GetFileName(ffmpegProcess.StartInfo.ArgumentList.Last())}]");
    }

    private ProcessStartInfo GetProcessStartInfo(string filename)
    {
        var processStartInfo = new ProcessStartInfo()
        {
            FileName = _config.FfmpegLocation,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        processStartInfo.ArgumentList.Add("-i");
        processStartInfo.ArgumentList.Add(filename);

        if (_config.UseDefaultCodecs == false && _config.Arguments != null && _config.Arguments.Length > 0)
        {
            foreach (var item in _config.Arguments.Split(' '))
            {
                processStartInfo.ArgumentList.Add(item);
            }
        }


        string outputName = Path.GetFileNameWithoutExtension(filename) + ".mp4";

        if (_config.UseCurrentDateTimeAsFileName)
            outputName = DateTime.Now.ToString(_config.DateTimeFormat) + ".mp4";

        processStartInfo.ArgumentList.Add(Path.Combine(_config.SaveDirectory, outputName));

        return processStartInfo;
    }
}
