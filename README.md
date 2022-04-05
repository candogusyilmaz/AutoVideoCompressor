# Auto Video Compressor

This application watches a folder and compresses newly created files.

I made this application for compressing my game captures with NVIDIA ShadowPlay. Around 2-3x smaller size.

## I just want to use this. How?

Download the latest **release** and unzip to the folder you want to compress.

## Installation Instructions

If you would like to create a single exe file run this command:

`dotnet publish -r win-x64 -c Release /p:PublishSingleFile=true /p:PublishTrimmed=true --self-contained true`

You need `ffmpeg.exe` in order to compress the videos. You can the download links below:

`https://www.gyan.dev/ffmpeg/builds/`

## Modifications

Modifications can be done by editing the **`appsettings.json`** file.

`"Arguments": "<arguments for changing the default behaviour of the compressing style>"`

`"UseDefaultCodecs": <default is true | false if you want to specify arguments>`

`"UseDateTimeFormatAsFilename": <true or false>`

`"DateTimeFormat": "<example: yyyy_mm_dd_hh_mm_ss>"`

`"DeleteOriginalFile": <true or false>`

`"FfmpegLocation": "<path to the ffmpeg.exe file name included>"`

`"VideosDirectory": "<which folder to track>"`

`"SaveDirectory": "<where to save the compressed files>"`

## License

This project is licensed under [MIT License](LICENSE).
