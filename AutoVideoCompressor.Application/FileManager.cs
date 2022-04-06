using Microsoft.Extensions.Configuration;

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
        _compressor = new CompressManager(config, _logger);

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
        _logger.Info($"File watching started. [{_watcher.Path}]");
    }

    public void Stop()
    {
        _watcher.EnableRaisingEvents = false;
        _logger.Info("Stopped file watching.");
    }

    private async void OnFileCreated(object sender, FileSystemEventArgs e)
    {
        while (IsFileInUse(e.FullPath)) { }

        // Just to make sure file is not in use
        await Task.Delay(2000);

        await _compressor.Compress(e.FullPath);

        DeleteOriginalFileIfEnabled(e.FullPath);
    }

    private void CreateDirectoryIfNotExists()
    {
        if (Directory.Exists(_config.SaveDirectory) == false)
        {
            Directory.CreateDirectory(_config.SaveDirectory);
            _logger.Info($"Save directory does not exist. Folder created. [{_config.SaveDirectory}]");
        }
    }

    private void DeleteOriginalFileIfEnabled(string filename)
    {
        if (_config.DeleteOriginalFile)
        {
            if (File.Exists(filename))
            {
                File.Delete(filename);
                _logger.Final($"Original file deleted. [{Path.GetFileName(filename)}]");
            }
        }
    }

    private bool IsFileInUse(string filename)
    {
        try
        {
            using (FileStream inputStream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                inputStream.Close();
            }
        }
        catch (IOException)
        {
            return true;
        }

        return false;
    }
}