using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DanilovSoft.Jpegli.Native;

#if DEBUG
internal static class AssertNative
{
    [ModuleInitializer]
    public static void InitializeModule()
    {
        Debug.Assert(Marshal.SizeOf<jpeg_common_struct>() == 40); // 40 for x64
        Debug.Assert(Marshal.SizeOf<jpeg_error_mgr>() == 168); // 164 absolute; 168 for x64 (Pack 8)
        Debug.Assert(Marshal.SizeOf<jpeg_compress_struct>() == 504); // 504 for x64;   (!) у cjpegli.exe размер 520, почему??
        //Debug.Assert(Marshal.SizeOf<jpeg_destination_mgr>() == 40); 
        Debug.Assert(Marshal.SizeOf<jpeg_progress_mgr>() == 24);
        Debug.Assert(Marshal.SizeOf<jpeg_memory_mgr>() == 96);
        Debug.Assert(Marshal.SizeOf<jpeg_comp_master>() == 832); // версия jpegli содержит больше параметров чем jpeglib.
        Debug.Assert(Marshal.SizeOf<RowBuffer>() == 40);

        var force_baselineOffset = (int)Marshal.OffsetOf<jpeg_comp_master>(nameof(jpeg_comp_master.force_baseline));
        var xyb_modeOffset = (int)Marshal.OffsetOf<jpeg_comp_master>(nameof(jpeg_comp_master.xyb_mode));
        Debug.Assert(force_baselineOffset == 224);
        Debug.Assert(xyb_modeOffset == 225);

        var smooth_inputOffset = (int)Marshal.OffsetOf<jpeg_comp_master>(nameof(jpeg_comp_master.smooth_input));
        var next_scanlineOffset = (int)Marshal.OffsetOf<jpeg_compress_struct>(nameof(jpeg_compress_struct.next_scanline));
        var offset = (int)Marshal.OffsetOf<jpeg_compress_struct>(nameof(jpeg_compress_struct.master));
    }
}
#endif
