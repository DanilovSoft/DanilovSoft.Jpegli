using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DanilovSoft.Jpegli.Native;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
internal unsafe class jpeg_error_mgr
{
    public IntPtr error_exit; // void (*error_exit) (j_common_ptr cinfo)
    public IntPtr emit_message; // void (*emit_message) (j_common_ptr cinfo, int msg_level)
    public IntPtr output_message; // void (*output_message) (j_common_ptr cinfo)
    
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public delegate* unmanaged[Cdecl]<IntPtr, byte*, void> format_message;

    public IntPtr reset_error_mgr; // void (*reset_error_mgr) (j_common_ptr cinfo)
    public int msg_code;
    public msg_parm_union msg_parm;
    public int trace_level;
    public int num_warnings;
    public IntPtr jpeg_message_table; // const char * const *
    public int last_jpeg_message;
    public IntPtr addon_message_table; // const char * const *
    public int first_addon_message;
    public int last_addon_message;
}
