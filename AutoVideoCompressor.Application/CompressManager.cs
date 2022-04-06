using Microsoft.Extensions.Logging;

using System.Diagnostics;

namespace AutoVideoCompressor.Application;
public sealed class CompressManager
{
    private readonly CompressorConfiguration _config;
    private readonly ILogger _logger;
    private readonly List<VideoCompressInfo> _videos;
    private bool _compressing = false;

    public CompressManager(CompressorConfiguration config, ILogger logger)
    {
        _videos = new List<VideoCompressInfo>();
        _config = config;
        _logger = logger;
    }

    public async Task Add(VideoCompressInfo video)
    {
        _videos.Add(video);

        await Compress(video);
    }

    private async Task Compress(VideoCompressInfo video)
    {
        if (_compressing)
            return;

        await WaitForFile(video.InputFullPath);

        var process = Process.Start(GetProcessStartInfo(video));
        _logger.LogInformation("Process started. [{FileName}]", video.FileName);

        if (process != null)
        {
            _compressing = true;

            process.ErrorDataReceived += (s, e) =>
            {
                (s as Process)?.Kill();
                video.Failed();
                _logger.LogInformation("Process encountered an error. [{FileName}]", video.FileName);
            };

            await process.WaitForExitAsync();
            _logger.LogInformation("Process finished. [{FileName}] [{OutputDirectory}\\{OutputFileName}]"
                , video.FileName, Directory.GetParent(video.OutputFullPath)?.Name, video.OutputFileName);
        }

        await OnCompressEnd(video);
    }

    private async Task OnCompressEnd(VideoCompressInfo video)
    {
        _compressing = false;

        if (video.IsFailed)
        {
            if (video.CanRetry == false)
            {
                _videos.Remove(video);
                _logger.LogInformation("Video is not going to be compressed. [Too many fails] [{FileName}]", video.FileName);
            }
        }
        else
        {
            _videos.Remove(video);

            if (_config.DeleteOriginalFile)
                DeleteFile(video.InputFullPath);
        }

        await Next();
    }

    private ProcessStartInfo GetProcessStartInfo(VideoCompressInfo video)
    {
        var processStartInfo = new ProcessStartInfo()
        {
            FileName = _config.FfmpegLocation,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        processStartInfo.ArgumentList.Add("-loglevel");
        processStartInfo.ArgumentList.Add("warning");

        processStartInfo.ArgumentList.Add("-i");
        processStartInfo.ArgumentList.Add(video.InputFullPath);

        if (_config.UseDefaultCodecs == false && _config.Arguments != null && _config.Arguments.Length > 0)
        {
            foreach (var item in _config.Arguments.Split(' '))
            {
                processStartInfo.ArgumentList.Add(item);
            }
        }

        processStartInfo.ArgumentList.Add(video.OutputFullPath);

        return processStartInfo;
    }

    private static async Task WaitForFile(string fullpath)
    {
        while (true)
        {
            try
            {
                using var inputStream = File.Open(fullpath, FileMode.Open, FileAccess.Read, FileShare.None);
                inputStream.Close();
            }
            catch (IOException)
            {
                await Task.Delay(2000);
            }

            break;
        }
    }

    private void DeleteFile(string fullpath)
    {
        if (File.Exists(fullpath))
        {
            File.Delete(fullpath);
            _logger.LogInformation("File deleted. [{FileName}]", Path.GetFileName(fullpath));
        }
    }

    private async Task Next()
    {
        if (_videos.Count > 0)
            await Compress(_videos[0]);
    }
}
