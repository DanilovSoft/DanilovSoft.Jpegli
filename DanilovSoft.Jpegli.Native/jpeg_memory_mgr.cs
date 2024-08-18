using System.Runtime.InteropServices;

namespace DanilovSoft.Jpegli.Native;

[StructLayout(LayoutKind.Sequential)]
internal unsafe class jpeg_memory_mgr
{
    public IntPtr /*delegate* unmanaged[Cdecl]<int, IntPtr>*/ alloc_small;
    public IntPtr /*delegate* unmanaged[Cdecl]<int, int, IntPtr>*/ alloc_large;
    public IntPtr /*delegate* unmanaged[Cdecl]<int, int, IntPtr>*/ alloc_sarray;
    public IntPtr /*delegate* unmanaged[Cdecl]<int, int, int, IntPtr>*/ alloc_barray;
    public IntPtr /*delegate* unmanaged[Cdecl]<int, void>*/ request_virt_sarray;
    public IntPtr /*delegate* unmanaged[Cdecl]<int, int, int, void>*/ request_virt_barray;
    public IntPtr /*delegate* unmanaged[Cdecl]<int, void>*/ realize_virt_arrays;
    public IntPtr /*delegate* unmanaged[Cdecl]<int, void>*/ access_virt_sarray;
    public IntPtr /*delegate* unmanaged[Cdecl]<int, int, int, void>*/ access_virt_barray;
    public IntPtr /*delegate* unmanaged[Cdecl]<void>*/ free_pool;
    public IntPtr /*delegate* unmanaged[Cdecl]<void>*/ self_destruct;

    public int max_memory_to_use; // Maximum memory to use
    public int max_alloc_chunk; // Maximum allocation chunk size
}
