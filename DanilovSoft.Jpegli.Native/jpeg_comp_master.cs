namespace DanilovSoft.Jpegli.Native;

[StructLayout(LayoutKind.Explicit, Pack = 8)]
internal sealed class jpeg_comp_master
{
    //public IntPtr prepare_for_pass; // void (*prepare_for_pass) (j_compress_ptr cinfo);
    //public IntPtr pass_startup; // void (*pass_startup) (j_compress_ptr cinfo);
    //public IntPtr finish_pass; // void (*finish_pass) (j_compress_ptr cinfo);

    //[MarshalAs(UnmanagedType.I1)]
    //public bool call_pass_startup; // boolean call_pass_startup;

    //[FieldOffset() MarshalAs(UnmanagedType.I1)]
    //public bool is_last_pass; // boolean is_last_pass;

    [FieldOffset(452)]
    public float psnr_target;
    
    [FieldOffset(456)]
    public float psnr_tolerance;

    [FieldOffset(464)]
    public float max_distance;
}
