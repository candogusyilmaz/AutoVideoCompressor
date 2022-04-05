namespace AutoVideoCompressor.Application;
public sealed class CompressorConfiguration
{
    public string Arguments { get; init; } = "";
    public bool UseDefaultCodecs { get; init; } = true;
    public bool UseCurrentDateTimeAsFileName { get; set; } = false;
    public string DateTimeFormat { get; set; } = "yyyy_mm_dd.hh_mm";
    public bool DeleteOriginalFile { get; set; } = false;
    public string FfmpegLocation { get; init; } = Path.Combine(AppContext.BaseDirectory, "ffmpeg.exe");
    public string SaveDirectory { get; init; } = Path.Combine(AppContext.BaseDirectory, "Compressed");
    public string VideosDirectory { get; set; } = AppContext.BaseDirectory;
}
