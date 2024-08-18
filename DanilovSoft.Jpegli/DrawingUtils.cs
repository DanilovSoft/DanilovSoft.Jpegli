using System.Drawing.Imaging;
using DanilovSoft.Jpegli.Native;

namespace DanilovSoft.Jpegli;

internal static class DrawingUtils
{
    /// <summary>
    /// Converts pixel format from <see cref="PixelFormat"/> to <see cref="TJPixelFormat"/>.
    /// </summary>
    /// <param name="pixelFormat">Pixel format to convert.</param>
    /// <returns>Converted value of pixel format or exception if convertion is impossible.</returns>
    /// <exception cref="NotSupportedException">Convertion can not be performed.</exception>
    public static J_COLOR_SPACE ConvertPixelFormat(PixelFormat pixelFormat) => pixelFormat switch
    {
        PixelFormat.Format32bppArgb => J_COLOR_SPACE.JCS_EXT_BGRA,
        PixelFormat.Format24bppRgb => J_COLOR_SPACE.JCS_EXT_BGR,
        PixelFormat.Format8bppIndexed => J_COLOR_SPACE.JCS_GRAYSCALE,
        _ => ThrowNotSupported(pixelFormat)
    };

    private static J_COLOR_SPACE ThrowNotSupported(PixelFormat pixelFormat)
    {
        throw new NotSupportedException($"Provided pixel format \"{pixelFormat}\" is not supported");
    }
}
