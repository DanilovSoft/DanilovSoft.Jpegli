using System.Buffers;
using DanilovSoft.Jpegli.Native;
using DanilovSoft.Jpegli.Native.PInvoke;

namespace DanilovSoft.Jpegli;

public static class Jpegli
{
    public static int JRoundUp(int a, int b)
    {
        return Jpeg62.jround_up(a, b);
    }

    public static void Compress(ReadOnlySpan<byte> inputImage, int width, int height, int stride, int channels, int quality, IBufferWriter<byte> output)
    {
        // будем вызывать API старой библиотеки на си "IJG JPEG LIBRARY"

        using var compressor = new LibJpegCompressor();
        compressor.Compress(inputImage, width, height, stride, channels, quality, output);
    }

    public static int JpegQualityScaling(int quality)
    {
        return Jpeg62.jpeg_quality_scaling(quality);
    }
}
