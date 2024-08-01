using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text;

namespace DanilovSoft.Jpegli.Native;



//[StructLayout(LayoutKind.Sequential)]
//public unsafe class jpeg_error_mgr
//{
//    // Function pointers for error handling
//    /// <summary>
//    /// Method to call if fatal error occurs.
//    /// </summary>
//    //public IntPtr error_exit;
//    public delegate*<IntPtr, void> error_exit;
//    public delegate*<IntPtr, int, void> emit_message;
//    public delegate*<IntPtr, void> output_message;
//    public delegate*<IntPtr, IntPtr, void> format_message;
//    public delegate*<IntPtr, void> reset_error_mgr;

//    // Fields for error handling state
//    public int msg_code;
//    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
//    public int[]? msg_parm_i;
//    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
//    public string? msg_parm_s;

//    public int trace_level;
//    /// <summary>
//    /// Number of corrupt-data warnings.
//    /// </summary>
//    public long num_warnings;

//    public IntPtr jpeg_message_table;
//    public int last_jpeg_message;
//    public IntPtr addon_message_table;
//    public int first_addon_message;
//    public int last_addon_message;
//};


[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public class jpeg_error_mgr
{
    public IntPtr error_exit; // void (*error_exit) (j_common_ptr cinfo)
    public IntPtr emit_message; // void (*emit_message) (j_common_ptr cinfo, int msg_level)
    public IntPtr output_message; // void (*output_message) (j_common_ptr cinfo)
    public IntPtr format_message; // void (*format_message) (j_common_ptr cinfo, char *buffer)
    public IntPtr reset_error_mgr; // void (*reset_error_mgr) (j_common_ptr cinfo)

    public int msg_code;

    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi)]
    public unsafe struct msg_parm_union
    {
        [FieldOffset(0)]
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        //public int[] i;
        public msg_param_int8 i;

        [FieldOffset(0)]
        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        //public string s;
        public msg_param_char80 s; // char*

        public readonly string AsText => Encoding.ASCII.GetString(s);
        public readonly ReadOnlySpan<int> AsIntegers => i;
    }

    [InlineArray(8)]
    public struct msg_param_int8
    {
        public int Value0;
    }

    [InlineArray(80)]
    public struct msg_param_char80
    {
        public byte Value0;
    }

    public msg_parm_union msg_parm;

    public int trace_level;
    public long num_warnings;

    public IntPtr jpeg_message_table; // const char * const *
    public int last_jpeg_message;

    public IntPtr addon_message_table; // const char * const *
    public int first_addon_message;
    public int last_addon_message;
}