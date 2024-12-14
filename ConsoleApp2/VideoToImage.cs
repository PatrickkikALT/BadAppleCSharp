using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

internal class VideoToImage
{
    private string outputDirectory;
    private Engine engine = new Engine();
    private MediaFile mp4 = new MediaFile();
    public int fps = 10;
    public async void CreateVideoAsync(string path)
    {
        mp4.Filename = path;
        outputDirectory = Path.Combine(Path.GetDirectoryName(path), "frames");
        if (!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }
        
        engine.GetMetadata(mp4);
        int totalSeconds = (int)mp4.Metadata.Duration.TotalSeconds;
        
        int maxConcurrentThreads = 4; 
        SemaphoreSlim semaphore = new SemaphoreSlim(maxConcurrentThreads);

        List<Task> tasks = new();
        for (double currentTime = 0; currentTime < totalSeconds; currentTime += 1.0 / fps)
        {
            double frameTime = currentTime; // Accurate timestamp for each frame
            await semaphore.WaitAsync();
            tasks.Add(Task.Run(() =>
            {
                try
                {
                    CreateThumbnail(frameTime);
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }

        await Task.WhenAll(tasks);
    }

    private void CreateThumbnail(double second)
    {
        Console.WriteLine("Converting Frame {0}", second);
        var options = new ConversionOptions { Seek = TimeSpan.FromSeconds(second) };
        var outputFile = new MediaFile
        {
            Filename = Path.Combine(outputDirectory, $"image-{((int)(second * fps)).ToString("D4")}.jpeg")
        };
        engine.GetThumbnail(mp4, outputFile, options);
    }
}