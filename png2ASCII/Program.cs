using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace png2ASCII
{
    internal static class Program
    {
        public static string Alphabet = "$@B%8&WM#*oahkbdpqwmZO0QLCJUYXzcvunxrjft/\\|()1{}[]?-_+~<>i!lI;:,\"^`'.";

        public const int Width = 240;
        public const int Height = 60;

        private static void Main(string[] args)
        {
            Console.SetWindowSize(Width, Height);
            var img = Image.FromFile("test.jpg");
            var resized = ResizeImage(img, Width, Height * 2);
            resized.Save("out1.png", ImageFormat.Png);
            var gray = GrayScaleFilter(resized);
            gray.Save("out2.png", ImageFormat.Png);

            for (var i = 0; i < Height; i++)
            for (var j = 0; j < Width; j++)
            {
                var index = (int)(gray.GetPixel(j, i * 2).R * (Alphabet.Length - 1) * 0.75f / 255f +
                             gray.GetPixel(j, i * 2 + 1).R * (Alphabet.Length - 1) * 0.25f / 255f);
                Console.Write(Alphabet[index]);
            }

            Console.ReadLine();
        }

        public static Bitmap GrayScaleFilter(Bitmap image)
        {
            var grayScale = new Bitmap(image.Width, image.Height);

            for (var y = 0; y < grayScale.Height; y++)
            for (var x = 0; x < grayScale.Width; x++)
            {
                var c = image.GetPixel(x, y);

                var gs = (int)(c.R * 0.3 + c.G * 0.59 + c.B * 0.11);

                grayScale.SetPixel(x, y, Color.FromArgb(gs, gs, gs));
            }

            return grayScale;
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}