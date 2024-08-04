using System.Buffers;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

namespace DanilovSoft.Jpegli.App;

internal class Program
{
    static void Main()
    {
        //var inputFile = File.ReadAllBytes("TestImages/1_webp_ll.png");

        var bmp = (Bitmap)Image.FromFile("TestImages/gegegekman.jpeg");
        var rawImage = PrepareRaw(bmp);

        var output = new ArrayBufferWriter<byte>();
        Jpegli.Compress(rawImage.Data, rawImage.Width, rawImage.Height, rawImage.Stride, rawImage.Channels, 90, output);

        File.WriteAllBytes("test.jpg", output.WrittenSpan.ToArray());
    }

    private static unsafe RawImage PrepareRaw(Bitmap png)
    {
        // png is Format32bppArgb
        // In .NET Argb is BGRA
        var width = png.Width;
        var height = png.Height;
        var pixelFormat = png.PixelFormat;
        var bitmapData = png.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, pixelFormat);
        var stride = bitmapData.Stride;
        var channel = Image.GetPixelFormatSize(pixelFormat) / 8;
        var scanLine = bitmapData.Stride / channel;
        var hasAlpha = Image.IsAlphaPixelFormat(pixelFormat);

        ReadOnlySpan<byte> scan0; // размер scan0 = Width * Height * num of Channels
        unsafe
        {
            scan0 = new ReadOnlySpan<byte>(bitmapData.Scan0.ToPointer(), stride * height * channel);
        }

        //_rawImages[Mode.BGR] = ToRawImage(scan0, width, height, stride, scanLine, channel, hasAlpha, destChannel: 3, swapRgb: false, includeAlpha: false);
        var rawImage = ToRawImage(scan0, width, height, stride, scanLine, channel, hasAlpha, destChannel: 3, swapRgb: true, includeAlpha: false);
        //_rawImages[Mode.BGRA] = ToRawImage(scan0, width, height, stride, scanLine, channel, hasAlpha, destChannel: 4, swapRgb: false, includeAlpha: true);
        //_rawImages[Mode.RGBA] = ToRawImage(scan0, width, height, stride, scanLine, channel, hasAlpha, destChannel: 4, swapRgb: true, includeAlpha: true);

        png.UnlockBits(bitmapData);

        return rawImage;
    }

    private static RawImage ToRawImage(ReadOnlySpan<byte> scan0,
                                   int width,
                                   int height,
                                   int stride,
                                   int scanLine,
                                   int channel,
                                   bool hasAlpha,
                                   int destChannel,
                                   bool swapRgb,
                                   bool includeAlpha)
    {
        var targetData = new byte[scanLine * destChannel * height];

        ToRawImage(scan0, width, height, stride, scanLine, channel, hasAlpha, destChannel, swapRgb, includeAlpha, targetData);

        return new RawImage
        {
            Data = targetData,
            Width = width,
            Height = height,
            Stride = scanLine * destChannel,
            Channels = destChannel
        };
    }

    private static void ToRawImage(ReadOnlySpan<byte> source,
                                   int width,
                                   int height,
                                   int stride,
                                   int scanLine,
                                   int channel,
                                   bool hasAlpha,
                                   int destChannel,
                                   bool swapRgb,
                                   bool includeAlpha,
                                   Span<byte> target)
    {
        Debug.Assert(source.Length >= (scanLine * channel * height));
        Debug.Assert(target.Length == (scanLine * destChannel * height));

        var destStride = scanLine * destChannel;

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
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
