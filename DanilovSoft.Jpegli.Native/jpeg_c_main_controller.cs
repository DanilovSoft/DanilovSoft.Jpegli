using System.Runtime.InteropServices;

namespace DanilovSoft.Jpegli.Native;

[StructLayout(LayoutKind.Sequential)]
internal sealed class jpeg_c_main_controller
{
    public IntPtr start_pass;    // void (*start_pass) (j_compress_ptr cinfo, J_BUF_MODE pass_mode);
    public IntPtr process_data;  // void (*process_data) (j_compress_ptr cinfo, JSAMPARRAY input_buf, JDIMENSION* in_row_ctr, JDIMENSION in_rows_avail);
}
