namespace DanilovSoft.Jpegli.Native;

[StructLayout(LayoutKind.Sequential)]
internal unsafe class jpeg_common_struct
{
    public jpeg_error_mgr* err; // jpeg_error_mgr*
    public jpeg_memory_mgr* mem; // jpeg_memory_mgr*
    public jpeg_progress_mgr* progress; // jpeg_progress_mgr*
    public void* client_data; // void *client_data; Available for use by application.
    [MarshalAs(UnmanagedType.I1)]
    public bool is_decompressor; // So common code can tell which is which
    public GlobalState global_state; // For checking call sequence validity
}
