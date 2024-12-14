using ConsoleApp2;
using MediaToolkit.Model;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Drawing;
namespace ConsoleApp2;

class Program {
    static Generator generator = new Generator(); 
    static VideoToImage videoCreator = new VideoToImage();
    private static string videoPath;
    private static void Main(string[] args)
    {
        videoPath = Path.Combine(Directory.GetCurrentDirectory(), "video.mp4");
        Console.WriteLine("Creating images from video file");
        PlayVideo();
        Console.ReadKey();
    }

    
    public static async void PlayVideo()
    {
        string path = Path.Combine(Path.GetDirectoryName(videoPath), "frames");
        //Checks if frames have already been created
        if (!Directory.Exists(path))
        {
            await Task.Run(() => videoCreator.CreateVideoAsync(videoPath));
        }
        
        string[] images = Directory.GetFiles(path);
        //Makes sure all the frames are in correct order.
        Array.Sort(images);
        int delay = 1000 / videoCreator.fps;
        foreach (string s in images)
        {
            Console.Clear();
            var sourceImage = await Image.LoadAsync(s);
            var imageRgba32 = sourceImage.CloneAs<Rgba32>();
            var image = new ImageSharpImageSource(imageRgba32);
            var asciiArt = generator.GenerateAsciiArtFromImage(image);
            Console.WriteLine(asciiArt.Art);
            await Task.Delay(delay);
        }
    }
}
