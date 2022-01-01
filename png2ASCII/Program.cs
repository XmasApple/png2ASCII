using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace png2ASCII
{
    internal static class Program
    {
        public static string Alphabet = " .:=+*#%@";


        public const int Scaling = 2;
        public const float HeightMix = 0.7f;

        private static void Main(string[] args)
        {
            var width = Console.LargestWindowWidth;
            var height = Console.LargestWindowHeight;
            Console.SetWindowSize(width, height);
            using var image = Image.Load<Rgb24>("test.jpg");

            image.Mutate(x => x.Resize(width * Scaling, height * 2 * Scaling).Grayscale());

            for (var i = 0; i < height; i++)
            for (var j = 0; j < width; j++)
            {
                float index = 0;
                for (var k = 0; k < Scaling * Scaling; k++)
                {
                    index += image[j * Scaling + k % Scaling, i * 2 * Scaling + k / Scaling].R * HeightMix
                             + image[j * Scaling + k % Scaling, (i * 2 + 1) * Scaling + k / Scaling].R *
                             (1 - HeightMix);
                }

                var brightness = (int) (index / (Scaling * Scaling * 255) * Alphabet.Length);
                Console.Write(Alphabet[brightness]);
            }
        }
    }
}