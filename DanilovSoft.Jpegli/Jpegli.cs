using DanilovSoft.Jpegli.Native;
using DanilovSoft.Jpegli.Native.PInvoke;

namespace DanilovSoft.Jpegli;

public static class Jpegli
{
    public static long JRoundUp(long a, long b)
    {
        return Jpeg62.jround_up(a, b);
    }

    public static void Compress(int quality)
    {
        // будем вызывать API старой библиотеки на си "IJG JPEG LIBRARY"

        using var compressor = new LibJpegCompressor();
        compressor.Compress(quality);
    }
}
