using System.Runtime.InteropServices;

namespace DanilovSoft.Jpegli.Native;

[StructLayout(LayoutKind.Sequential)]
internal class jpeg_progress_mgr
{
    public IntPtr progress_monitor;     // void (*progress_monitor) (j_common_ptr cinfo);

    public int pass_counter;            /* work units completed in this pass */
    public int pass_limit;              /* total number of work units in this pass */
    public int completed_passes;         /* passes completed so far */
    public int total_passes;             /* total number of passes expected */
}
