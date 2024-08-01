using System.Runtime.InteropServices;

[assembly: DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]

namespace DanilovSoft.Jpegli.Native.PInvoke;

[SuppressMessage("Security", "CA5393:Не используйте небезопасное значение DllImportSearchPath.", Justification = "Не знаю где ещё можно хранить")]
internal static unsafe partial class Jpeg62
{
    private const string NativeLibrary = "libjpeg\\jpeg62.dll";

    [LibraryImport(NativeLibrary, EntryPoint = "jpeg_create_compress")]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    public static partial void jpeg_create_compress(nint cinfo);

    [LibraryImport(NativeLibrary, EntryPoint = "jpeg_CreateCompress")]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    public static partial void jpeg_CreateCompress(nint cinfo, int version, nuint structsize);

    //[LibraryImport(NativeLibrary, EntryPoint = "jpeg_std_error")]
    [DllImport(NativeLibrary, CallingConvention = CallingConvention.Cdecl, EntryPoint = "jpeg_std_error")]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    //public static extern IntPtr jpeg_std_error(ref jpeg_error_mgr jpeg_error_mgr);
    //public static extern IntPtr jpeg_std_error(IntPtr jpeg_error_mgr);
    public static extern IntPtr jpeg_std_error([In, Out] jpeg_error_mgr jpeg_error_mgr);

    [LibraryImport(NativeLibrary, EntryPoint = "jpeg_std_error")]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    public static partial IntPtr jpeg_std_errorPtr(IntPtr jpeg_error_mgr);

    [DllImport(NativeLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern void jpeg_stdio_dest(nint cinfo, nint outfile);

    [DllImport(NativeLibrary, EntryPoint = "jpeg_set_defaults", CallingConvention = CallingConvention.Cdecl)]
    //[UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    public static extern void jpeg_set_defaults(IntPtr cinfo);
    //public static extern void jpeg_set_defaults([In, Out] jpeg_compress_struct cinfo);
    //public static extern void jpeg_set_defaults(ref jpeg_compress_struct cinfo);

    [DllImport(NativeLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern void jpeg_set_quality(nint cinfo, int quality, bool force_baseline);

    [DllImport(NativeLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern void jpeg_start_compress(nint cinfo, bool write_all_tables);

    [DllImport(NativeLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern nuint jpeg_write_scanlines(nint cinfo, nint scanlines, nuint num_lines);

    [DllImport(NativeLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern void jpeg_finish_compress(nint cinfo);

    [DllImport(NativeLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern void jpeg_destroy_compress(nint cinfo);

    [DllImport(NativeLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern void jpeg_destroy(nint j_common_ptr);

    /// <summary>
    /// Compute a rounded up to next multiple of b, ie, ceil(a/b)*b
    /// Assumes a >= 0, b > 0
    /// </summary>
    [LibraryImport(NativeLibrary, EntryPoint = "jround_up")]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    public static partial long jround_up(long a, long b);
}
