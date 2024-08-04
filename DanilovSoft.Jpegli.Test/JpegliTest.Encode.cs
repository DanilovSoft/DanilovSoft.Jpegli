using System.Buffers;
using System.Drawing;
using System.Drawing.Imaging;

namespace DanilovSoft.Jpegli.Test;

public sealed partial class JpegliTest
{
    private readonly Dictionary<Mode, RawImage> _rawImages = [];

    public JpegliTest()
    {
        // create test image on memory buffer
        using var png = (Bitmap)Image.FromFile(Path.Combine("TestImages", "1_webp_ll.png"));

        // png is Format32bppArgb
        // In .NET Argb is BGRA
        var width = png.Width;
        var height = png.Height;
        var format = png.PixelFormat;
        var bitmapData = png.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, format);
        var stride = bitmapData.Stride;
        var channel = Image.GetPixelFormatSize(format) / 8;
        var scanLine = bitmapData.Stride / channel;
        var hasAlpha = Image.IsAlphaPixelFormat(format);

        ReadOnlySpan<byte> scan0; // размер scan0 = Width * Height * num of Channels
        unsafe
        {
            scan0 = new ReadOnlySpan<byte>(bitmapData.Scan0.ToPointer(), stride * height * channel);
        }

        _rawImages[Mode.BGR] = ToRawImage(scan0, width, height, stride, scanLine, channel, hasAlpha, destChannel: 3, swapRgb: false, includeAlpha: false);
        _rawImages[Mode.RGB] = ToRawImage(scan0, width, height, stride, scanLine, channel, hasAlpha, destChannel: 3, swapRgb: true, includeAlpha: false);
        _rawImages[Mode.BGRA] = ToRawImage(scan0, width, height, stride, scanLine, channel, hasAlpha, destChannel: 4, swapRgb: false, includeAlpha: true);
        _rawImages[Mode.RGBA] = ToRawImage(scan0, width, height, stride, scanLine, channel, hasAlpha, destChannel: 4, swapRgb: true, includeAlpha: true);

        png.UnlockBits(bitmapData);
    }

    //[Fact]
    //public void WebPGetEncoderVersion()
    //{
    //    var version = Jpegli.WebPGetEncoderVersion();
    //    Assert.NotEqual(default, version);
    //}

    //[Fact]
    //public void WebPEncodeBGR()
    //{
    //    var raw = _rawImages[Mode.BGR];

    //    using (var encodedImage = Jpegli.WebPEncodeBGR(raw.Data, raw.Width, raw.Height, raw.Stride, 100))
    //    {
    //        Assert.True(encodedImage.Memory.Length > 0);
    //        SaveResultFile(encodedImage.Memory.Span.ToArray(), Jpegli.WebPEncodeBGR, "result.webp");
    //    }

    //    Assert.Throws<ArgumentOutOfRangeException>("width", () => { Jpegli.WebPEncodeBGR(raw.Data, 0, raw.Height, raw.Stride, 100); });
    //    Assert.Throws<ArgumentOutOfRangeException>("height", () => { Jpegli.WebPEncodeBGR(raw.Data, raw.Width, 0, raw.Stride, 100); });
    //    Assert.Throws<ArgumentOutOfRangeException>("stride", () => { Jpegli.WebPEncodeBGR(raw.Data, raw.Width, raw.Height, 0, 100); });
    //    Assert.Throws<ArgumentOutOfRangeException>("qualityFactor", () => { Jpegli.WebPEncodeBGR(raw.Data, raw.Width, raw.Height, raw.Stride, -1); });
    //    Assert.Throws<ArgumentOutOfRangeException>("qualityFactor", () => { Jpegli.WebPEncodeBGR(raw.Data, raw.Width, raw.Height, raw.Stride, 101); });
    //}

    [Theory]
    //[InlineData("C:\\Users\\Miles\\Desktop\\17_lossless.png", 85)]
    //[InlineData("C:\\Users\\Miles\\Desktop\\1_lossless.png", 85)]
    //[InlineData("C:\\Users\\Miles\\Desktop\\08.png", 80)]
    [InlineData("TestImages//1_webp_ll.png", 80)]
    public void WebPEncodeLossyBGR(string inputFile, int quality)
    {
        //var inputFileData = File.ReadAllBytes(inputFile);
        

        var inputFileInfo = new FileInfo(inputFile);
        var outputFile = Path.GetFileNameWithoutExtension(inputFileInfo.Name) + $"_Q{quality}.jpg";
        outputFile = Path.Combine(Path.GetDirectoryName(inputFile)!, outputFile);

        var raw = RawFromFile(inputFile);

        var output = new ArrayBufferWriter<byte>();
        Jpegli.Compress(raw.Data, raw.Width, raw.Height, raw.Stride, raw.Channel, quality, output);
        File.WriteAllBytes(outputFile, output.WrittenSpan.ToArray());

        //using (var encodedImage = Jpegli.Compress(raw.Data, raw.Width, raw.Height, raw.Stride, quality))
        //{
        //    //File.WriteAllBytes(outputFile, encodedImage.Memory.Span.ToArray());
        //}
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
            Channel = destChannel
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
        Debug.Assert(source.Length >= scanLine * channel * height);
        Debug.Assert(target.Length == scanLine * destChannel * height);

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

    private static void SaveResultFile(byte[] data, Delegate webpEncode, string fileName)
    {
        var directory = Path.Combine("Result", webpEncode.Method.Name);
        Directory.CreateDirectory(directory);
        var path = Path.Combine(directory, fileName);
        File.WriteAllBytes(path, data);
    }

    private static RawImage RawFromFile(string filePath)
    {
        // create test image on memory buffer
        using var image = (Bitmap)Image.FromFile(filePath);

        // jpg is Format24bppRgb
        // In .NET Argb is BGRA
        var width = image.Width;
        var height = image.Height;
        var format = image.PixelFormat;
        var bitmapData = image.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, format);
        var stride = bitmapData.Stride; // stride width (also called scan width)
        var hasAlpha = Image.IsAlphaPixelFormat(format);
        var channel = Image.GetPixelFormatSize(format) / 8;
        var scanLine = bitmapData.Stride / channel;

        ReadOnlySpan<byte> scan0; // размер scan0 = Width * Height * num of Channels
        unsafe
        {
            scan0 = new ReadOnlySpan<byte>(bitmapData.Scan0.ToPointer(), stride * height * channel);
        }

        var result = ToRawImage(scan0, width, height, stride, scanLine, channel, hasAlpha, destChannel: 3, swapRgb: false, includeAlpha: false);

        image.UnlockBits(bitmapData);

        return result;
    }

    private sealed class RawImage
    {
        public required byte[] Data { get; init; }
        public int Height { get; set; }
        public int Stride { get; set; }
        public int Width { get; set; }
        public int Channel { get; set; }
    }

    private enum Mode
    {
        RGB,
        BGR,
        RGBA,
        BGRA
    }
}
