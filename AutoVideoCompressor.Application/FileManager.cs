using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AutoVideoCompressor.Application;
public sealed class FileManager
{
    private readonly FileSystemWatcher _watcher;
    private readonly CompressorConfiguration _config;
    private readonly CompressManager _compressor;
    private readonly ILogger _logger;

    public FileManager(IConfiguration config, ILogger logger)
    {
        _config = config.Get<CompressorConfiguration>();
        _logger = logger;
        _compressor = new CompressManager(_config, logger);

        _watcher = new FileSystemWatcher(_config.VideosDirectory)
        {
            IncludeSubdirectories = false
        };
        _watcher.Created += OnFileCreated;

        CreateDirectoryIfNotExists();
    }

    public void Start()
    {
        _watcher.EnableRaisingEvents = true;
        _logger.LogInformation("File watching started. [{WatcherPath}]", _watcher.Path);
    }

    public void Stop()
    {
        _watcher.EnableRaisingEvents = false;
        _logger.LogInformation("Stopped file watching.");
    }

    private void OnFileCreated(object sender, FileSystemEventArgs e)
    {
        if (File.Exists(e.FullPath) == false)
            return;

        _ = _compressor.Add(new VideoCompressInfo(e.Name!, e.FullPath, _config.SaveDirectory));
    }

    private void CreateDirectoryIfNotExists()
    {
        if (Directory.Exists(_config.SaveDirectory) == false)
        {
            Directory.CreateDirectory(_config.SaveDirectory);
            _logger.LogInformation("Save directory does not exist. Folder created. [{SaveDirectory}]", _config.SaveDirectory);
        }
    }
}