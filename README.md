# Auto Video Compressor

This application watches a folder and compresses newly created files.

If you would like to run this application as a **Windows Service** download the latest release and follow the given instructions.

I made this application for compressing my game captures which NVIDIA ShadowPlay recorded. Reduces the video size to about %40 of the original file size without losing any quality in the process.

## I just want to use this. How?

Download the latest **[release](https://github.com/candogusyilmaz/AutoVideoCompressor/releases/latest)** and follow the instructions in the release details.

## Development

### Publish

If you would like to create a single executable file run the following command in the powershell:

```powershell
dotnet publish .\AutoVideoCompressor.Service\AutoVideoCompressor.Service.csproj -r win-x86 -c Release --self-contained true -p:PublishSingleFile=true -p:PublishReadyToRun=true
```

You need **FFmpeg library** in order to compress the videos. **FFmpeg** is already included in the releases but if you would like to download from the source you can find the link below.

**[FFmpeg Releases](https://www.gyan.dev/ffmpeg/builds/)**

### Modifications

Modifications can be done by editing the **`appsettings.json`** file.

```json
"UseDefaultCodecs": true,
"DeleteOriginalFile": true,
"VideosDirectory": "<which folder to track>",
"SaveDirectory": "<where to save the compressed files>",
```

Optional parameters:

```JSON5
"Arguments": "<ffmpeg arguments>", //set UseDefaultCodecs to false if arguments specified
"FfmpegLocation": "<path to the ffmpeg.exe>" //default value is AssemblyDirectory\\ffmpeg.exe
```

## License

This project is licensed under [MIT License](LICENSE).
