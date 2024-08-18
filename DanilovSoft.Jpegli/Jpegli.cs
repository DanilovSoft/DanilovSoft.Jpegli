using System.Buffers;
using System.Drawing;
using System.Drawing.Imaging;
using DanilovSoft.Jpegli.Native;
using DanilovSoft.Jpegli.Native.PInvoke;

namespace DanilovSoft.Jpegli;

public static class Jpegli
{
    public static int JRoundUp(int a, int b)
    {
        return Jpeg62.jround_up(a, b);
    }

    public static void Compress(IntPtr scan0, int stride, int width, int height, PixelFormat pixelFormat, int quality, IBufferWriter<byte> output)
    {
        ArgumentNullException.ThrowIfNull(output);

        var channels = Image.GetPixelFormatSize(pixelFormat) / 8;

        using var compressor = new LibJpegCompressor();
        compressor.Compress(scan0, stride, width, height, channels, DrawingUtils.ConvertPixelFormat(pixelFormat), quality, output);
    }

    public static int JpegQualityScaling(int quality)
    {
        return Jpeg62.jpeg_quality_scaling(quality);
    }

    /// <summary>
    /// Converts pixel format from <see cref="PixelFormat"/> to <see cref="TJPixelFormat"/>.
    /// </summary>
    /// <param name="pixelFormat">Pixel format to convert.</param>
    /// <returns>Converted value of pixel format or exception if convertion is impossible.</returns>
    /// <exception cref="NotSupportedException">Convertion can not be performed.</exception>
    public static J_COLOR_SPACE ConvertPixelFormat(PixelFormat pixelFormat)
    {
        switch (pixelFormat)
        {
            case PixelFormat.Format32bppArgb:
                return J_COLOR_SPACE.JCS_EXT_BGRA;
            case PixelFormat.Format24bppRgb:
                return J_COLOR_SPACE.JCS_EXT_BGR;
            case PixelFormat.Format8bppIndexed:
                return J_COLOR_SPACE.JCS_GRAYSCALE;
            default:
                throw new NotSupportedException($"Provided pixel format \"{pixelFormat}\" is not supported");
        }
    }
}
