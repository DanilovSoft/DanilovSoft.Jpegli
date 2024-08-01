using System.Runtime.InteropServices;

namespace DanilovSoft.Jpegli.Native.PInvoke;

// https://developers.google.com/speed/webp/docs/api?hl=ru
[SuppressMessage("Security", "CA5393:Не используйте небезопасное значение DllImportSearchPath.", Justification = "Don't know where it's better to store libs")]
internal static partial class NativeMethods
{
    //private const string NativeLibrary = "libwebp\\libwebp.dll";
    private const string NativeLibrary = "libjpeg\\jpeg62.dll";
    
    [LibraryImport(NativeLibrary)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    public static partial int WebPGetEncoderVersion();

    [LibraryImport(NativeLibrary)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    public static partial int WebPGetDecoderVersion();

    [LibraryImport(NativeLibrary)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    public static partial long WebPEncodeRGB(ReadOnlySpan<byte> rgb,
                                             int32_t width,
                                             int32_t height,
                                             int32_t stride,
                                             float qualityFactor,
                                             out IntPtr output); // uint8_t** output

    [LibraryImport(NativeLibrary)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    public static partial long WebPEncodeBGR(ReadOnlySpan<byte> bgr,
                                             int32_t width,
                                             int32_t height,
                                             int32_t stride,
                                             float qualityFactor,
                                             out IntPtr output); // uint8_t** output

    [LibraryImport(NativeLibrary)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    public static partial long WebPEncodeRGBA(ReadOnlySpan<byte> rgba,
                                             int32_t width,
                                             int32_t height,
                                             int32_t stride,
                                             float qualityFactor,
                                             out IntPtr output); // uint8_t** output

    [LibraryImport(NativeLibrary)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    public static partial long WebPEncodeBGRA(ReadOnlySpan<byte> bgra,
                                             int32_t width,
                                             int32_t height,
                                             int32_t stride,
                                             float qualityFactor,
                                             out IntPtr output); // uint8_t** output

    [LibraryImport(NativeLibrary)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    public static partial void WebPFree(IntPtr p);

    [LibraryImport(NativeLibrary, EntryPoint = "tjCompress2")]
    [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    public static partial int TjCompress2(IntPtr handle, IntPtr srcBuf, int width, int pitch, int height, int pixelFormat, ref IntPtr jpegBuf, ref ulong jpegSize, int jpegSubsamp, int jpegQual, int flags);

    /// <summary>
    /// Free an image buffer previously allocated by TurboJPEG.  You should always
    /// use this function to free JPEG destination buffer(s) that were automatically
    /// (re)allocated by <see cref="TjCompress2"/> or <see cref="TjTransform"/> or that were manually
    /// allocated using <see cref="TjAlloc"/>.
    /// </summary>
    /// <param name="buffer">Address of the buffer to free.</param>
    /// <seealso cref="TjAlloc"/>
    [LibraryImport(NativeLibrary, EntryPoint = "tjFree")]
    [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    public static partial void TjFree(IntPtr buffer);
}
