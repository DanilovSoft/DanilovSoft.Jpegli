namespace DanilovSoft.Jpegli.Native;

public enum J_COLOR_SPACE : int
{
    JCS_UNKNOWN,            // error/unspecified
    JCS_GRAYSCALE,          // monochrome
    JCS_RGB,                // red/green/blue
    JCS_YCbCr,              // Y/Cb/Cr (also known as YUV)
    JCS_CMYK,               // C/M/Y/K
    JCS_YCCK,               // Y/Cb/Cr/K
    JCS_EXT_RGB,            // red/green/blue
    JCS_EXT_RGBX,           // red/green/blue/x
    JCS_EXT_BGR,            // blue/green/red
    JCS_EXT_BGRX,           // blue/green/red/x
    JCS_EXT_XBGR,           // x/blue/green/red
    JCS_EXT_XRGB,           // x/red/green/blue
    JCS_EXT_RGBA,           // red/green/blue/alpha
    JCS_EXT_BGRA,           // blue/green/red/alpha
    JCS_EXT_ABGR,           // alpha/blue/green/red
    JCS_EXT_ARGB,           // alpha/red/green/blue
    JCS_RGB565              // 5-bit red/6-bit green/5-bit blue
}
