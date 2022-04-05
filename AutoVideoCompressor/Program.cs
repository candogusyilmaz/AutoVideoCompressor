using AutoVideoCompressor;
using AutoVideoCompressor.Application;

Console.WriteLine("Press any key to exit the application.");

var fileManager = new FileManager(Conf.GetAppSettings(), new ConsoleLogger());
fileManager.Start();

Console.ReadKey();