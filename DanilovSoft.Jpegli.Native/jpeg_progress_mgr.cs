namespace DanilovSoft.Jpegli.Native;

[StructLayout(LayoutKind.Sequential)]
internal class jpeg_progress_mgr
{
    public IntPtr progress_monitor;     // void (*progress_monitor) (j_common_ptr cinfo);

    public int pass_counter;            /* work units completed in this pass */
    public int pass_limit;              /* total number of work units in this pass */
    public int completed_passes;         /* passes completed so far */
    public int total_passes;             /* total number of passes expected */
    //public byte Leftover0;
    //public byte Leftover1;
    //public byte Leftover2;
    //public byte Leftover3;
    //public byte Leftover4;
    //public byte Leftover5;
    //public byte Leftover6;
    //public byte Leftover7;
    //public byte Leftover8;
}
