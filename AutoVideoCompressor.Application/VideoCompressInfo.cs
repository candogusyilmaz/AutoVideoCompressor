namespace AutoVideoCompressor.Application;
public class VideoCompressInfo
{
    public string FileName { get; init; }
    public string FileNameWithoutExtension => Path.GetFileNameWithoutExtension(FileName);
    public string OutputFileName { get; private set; }
    public string OutputFullPath => Path.Combine(SavePath, OutputFileName);
    public string InputFullPath { get; init; }
    public string SavePath { get; init; }
    public bool IsFailed { get; private set; } = false;
    public int FailCount { get; private set; } = 0;
    public bool CanRetry => FailCount < 3;

    public VideoCompressInfo(string filename, string videoPath, string savePath)
    {
        FileName = filename;
        InputFullPath = videoPath;
        SavePath = savePath;
        OutputFileName = DetermineOutputFileName();
    }

    public void Failed()
    {
        FailCount++;
        IsFailed = true;
    }

    private string DetermineOutputFileName()
    {
        if (File.Exists(Path.Combine(SavePath, FileName)))
            return Path.GetRandomFileName() + ".mp4";
        else
            return FileName;
    }
}
