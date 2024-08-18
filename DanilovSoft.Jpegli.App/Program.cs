using System.Buffers;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

namespace DanilovSoft.Jpegli.App;

internal class Program
{
    static void Main()
    {
        const int Quality = 80;

        //var inputFile = File.ReadAllBytes("TestImages/1_webp_ll.png");
        using var bitmap = (Bitmap)Image.FromFile("TestImages/TJGF_title.jpg");

        var data = bitmap.LockBits(
            new Rectangle(0, 0, bitmap.Width, bitmap.Height), 
            ImageLockMode.ReadOnly,
            bitmap.PixelFormat);

        var output = new ArrayBufferWriter<byte>();
        try
        {
            Jpegli.Compress(data.Scan0, data.Stride, data.Width, data.Height, bitmap.PixelFormat, Quality, output);

            File.WriteAllBytes($"TestImages/output_MyJpegli_Q{Quality}.jpg", output.WrittenSpan.ToArray());
        }
        finally
        {
            bitmap.UnlockBits(data);
        }
    }

    private static RawImage PrepareRaw(Bitmap inputFile)
    {
        var width = inputFile.Width;
        var height = inputFile.Height;
        var pixelFormat = inputFile.PixelFormat; // png is Format32bppArgb. In .NET Argb is BGRA
        var bitmapData = inputFile.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, pixelFormat);
        var stride = bitmapData.Stride; // ширина в байтах
        var channels = Image.GetPixelFormatSize(pixelFormat) / 8;
        var scanLine = bitmapData.Stride / channels; // количество пикселей в строке (с заполнением)
        var hasAlpha = Image.IsAlphaPixelFormat(pixelFormat);

        ReadOnlySpan<byte> scan0; // размер scan0 = Width * Height * num of Channels
        unsafe
        {
            scan0 = new ReadOnlySpan<byte>((void*)bitmapData.Scan0, stride * height * channels);
        }

        //var absoluteSize = width * height * channels;

        //_rawImages[Mode.BGR] = ToRawImage(scan0, width, height, stride, scanLine, channel, hasAlpha, destChannel: 3, swapRgb: false, includeAlpha: false);
        var rawImage = ToRawImage(scan0, width, height, stride, scanLine, channels, hasAlpha, destChannels: 3, swapRgb: true, includeAlpha: false);
        //_rawImages[Mode.BGRA] = ToRawImage(scan0, width, height, stride, scanLine, channel, hasAlpha, destChannel: 4, swapRgb: false, includeAlpha: true);
        //_rawImages[Mode.RGBA] = ToRawImage(scan0, width, height, stride, scanLine, channel, hasAlpha, destChannel: 4, swapRgb: true, includeAlpha: true);

        inputFile.UnlockBits(bitmapData);

        return rawImage;
    }

    private static RawImage ToRawImage(ReadOnlySpan<byte> scan0, int width, int height, int stride, int scanLine,
        int channel, bool hasAlpha, int destChannels, bool swapRgb, bool includeAlpha)
    {
        var targetWidth = scanLine * destChannels;
        var targetData = new byte[targetWidth * height];

        ToRawImage(scan0, width, height, stride, scanLine, channel, hasAlpha, destChannels, swapRgb, includeAlpha, targetData);

        return new RawImage
        {
            Data = targetData,
            Width = width,
            Height = height,
            Stride = scanLine * destChannels,
            Channels = destChannels
        };
    }

    private static void ToRawImage(
        ReadOnlySpan<byte> source, int width, int height, int stride, int scanLine, int channel, bool hasAlpha, int destChannel, bool swapRgb, bool includeAlpha, Span<byte> target)
    {
        Debug.Assert(source.Length >= (scanLine * channel * height));
        Debug.Assert(target.Length == (scanLine * destChannel * height));

        var destStride = scanLine * destChannel;

        for (var _ = 0; _ < height; _++)
        {
            for (var __ = 0; __ < width; __++)
            {
                if (includeAlpha)
                {
                    target[3] = source[3];
                }

                if (hasAlpha && !includeAlpha)
                {
                    var alpha = source[3] / 255d;
                    if (swapRgb)
                    {
                        target[0] = (byte)(source[2] * alpha);
                        target[1] = (byte)(source[1] * alpha);
                        target[2] = (byte)(source[0] * alpha);
                    }
                    else
                    {
                        target[0] = (byte)(source[0] * alpha);
                        target[1] = (byte)(source[1] * alpha);
                        target[2] = (byte)(source[2] * alpha);
                    }
                }
                else
                {
                    if (swapRgb)
                    {
                        target[0] = source[2];
                        target[1] = source[1];
                        target[2] = source[0];
                    }
                    else
                    {
                        target[0] = source[0];
                        target[1] = source[1];
                        target[2] = source[2];
                    }
                }

                source = source[channel..];
                target = target[destChannel..];
            }

            source = source[(stride - width * channel)..];
            target = target[(destStride - width * destChannel)..];
        }
    }

    private sealed class RawImage
    {
        public required byte[] Data { get; init; }
        public int Height { get; set; }
        public int Stride { get; set; }
        public int Width { get; set; }
        public int Channels { get; set; }
    }
}
