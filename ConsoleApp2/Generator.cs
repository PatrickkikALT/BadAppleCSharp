﻿using System.Text;

namespace ConsoleApp2;

internal sealed class Generator
{
    public AsciiArt GenerateAsciiArtFromImage(ImageSource image)
    {
        var asciiChars = "@%#*+=-:,. ";

        var aspect = image.width / (double)image.height;
        var outputWidth = image.width / 16;
        var widthStep = image.width / outputWidth;
        var outputHeight = (int)(outputWidth / aspect);
        var heightStep = image.height / outputHeight;
        
        StringBuilder asciiBuilder = new(outputWidth * outputHeight);
        for (var h = 0; h < image.height; h += heightStep)
        {
            for (var w = 0; w < image.width; w += widthStep)
            {
                var pixelColor = image.GetPixel(w, h);
                var grayValue = (int)(pixelColor.Red * 0.3 + pixelColor.Green * 0.59 + pixelColor.Blue * 0.11);
                var asciiChar = asciiChars[grayValue * (asciiChars.Length - 1) / 255];
                asciiBuilder.Append(asciiChar);
                asciiBuilder.Append(asciiChar);
            }

            asciiBuilder.AppendLine();
        }

        AsciiArt art = new(
            asciiBuilder.ToString(),
            outputWidth,
            outputHeight);
        
        return art;
    }
}