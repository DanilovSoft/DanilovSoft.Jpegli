using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

[assembly: DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]

namespace DanilovSoft.Jpegli.Native.PInvoke;

[SuppressMessage("Security", "CA5393:Не используйте небезопасное значение DllImportSearchPath.", Justification = "Не знаю где ещё можно хранить")]
internal static unsafe partial class Jpeg62
{
    private const string NativeLibrary = "libjpeg\\jpeg62.dll";

    [LibraryImport(NativeLibrary, EntryPoint = "jpeg_create_compress")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void jpeg_create_compress(nint cinfo);

    [LibraryImport(NativeLibrary, EntryPoint = "jpeg_CreateCompress")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void jpeg_CreateCompress(j_compress_ptr cinfo, int version, nuint structsize);

    [LibraryImport(NativeLibrary, EntryPoint = "jpeg_std_error")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial IntPtr jpeg_std_error(IntPtr jpeg_error_mgr);

    [DllImport(NativeLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern void jpeg_stdio_dest(nint cinfo, nint outfile);

    [DllImport(NativeLibrary, EntryPoint = "jpeg_set_defaults", CallingConvention = CallingConvention.Cdecl)]
    //[UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    public static extern void jpeg_set_defaults(IntPtr cinfo);
    //public static extern void jpeg_set_defaults([In, Out] jpeg_compress_struct cinfo);
    //public static extern void jpeg_set_defaults(ref jpeg_compress_struct cinfo);

    [LibraryImport(NativeLibrary, EntryPoint = "jpeg_simple_progression")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void jpeg_simple_progression(IntPtr cinfo);

    [LibraryImport(NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void jpeg_mem_dest(IntPtr cinfo, ref IntPtr outbuffer, ref uint outsize);
    //public static partial void jpeg_mem_dest(IntPtr cinfo, ref IntPtr outbuffer, ref uint outsize);
    //public static partial void jpeg_mem_dest(IntPtr cinfo, [MarshalUsing(CountElementName = nameof(outsize))] ref Span<byte> outbuffer, ref uint outsize);

    [LibraryImport(NativeLibrary)]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvCdecl) })]
    public static partial void jpeg_set_quality(nint cinfo, int quality, [MarshalAs(UnmanagedType.Bool)] bool force_baseline);

    [LibraryImport(NativeLibrary, EntryPoint = "jpeg_quality_scaling")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int jpeg_quality_scaling(int quality);

    [DllImport(NativeLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern void jpeg_start_compress(nint cinfo, bool write_all_tables);

    /// <returns>number of scanlines actually written</returns>
    [LibraryImport(NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int jpeg_write_scanlines(nint cinfo, ReadOnlySpan<IntPtr> scanlines, int num_lines);
    //public static partial int jpeg_write_scanlines(nint cinfo, [In] byte*[] scanlines, int num_lines);

    [DllImport(NativeLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern void jpeg_finish_compress(nint cinfo);

    [LibraryImport(NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void jpeg_destroy_compress(nint cinfo);

    [DllImport(NativeLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern void jpeg_destroy(nint j_common_ptr);

    /// <summary>
    /// Compute a rounded up to next multiple of b, ie, ceil(a/b)*b
    /// Assumes a >= 0, b > 0
    /// </summary>
    [LibraryImport(NativeLibrary, EntryPoint = "jround_up")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int jround_up(int a, int b);
}
