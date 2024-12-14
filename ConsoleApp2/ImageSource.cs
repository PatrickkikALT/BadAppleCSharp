using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
namespace ConsoleApp2;


internal interface ImageSource : IDisposable
{
    int width { get; }
    int height { get; }
    float aspectRatio { get; }
    Rgb GetPixel(int x, int y);
}

internal record struct Rgb(
    byte Red,
    byte Green,
    byte Blue);


public record AsciiArt(
    string Art,
    int Width,
    int Height);

internal sealed class ImageSharpImageSource : ImageSource
{
    private readonly Image<Rgba32> _image;
    public ImageSharpImageSource(Image<Rgba32> image)
    {
        _image = image;
    }

    public int width => _image.Width;

    public int height => _image.Height;

    public float aspectRatio => _image.Width / (float)_image.Height;

    public Rgb GetPixel(int x, int y)
    {
        var pixel = _image[x, y];
        return new(
            pixel.R,
            pixel.G,
            pixel.B);
    }

    public void Dispose() => _image.Dispose();
}