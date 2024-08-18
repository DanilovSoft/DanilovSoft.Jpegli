using System.Runtime.InteropServices;

namespace DanilovSoft.Jpegli.Native;

[StructLayout(LayoutKind.Sequential)]
internal unsafe class jpeg_destination_mgr
{
    public byte* next_output_byte; // => next byte to write in buffer
    public int free_in_buffer; // # of byte spaces remaining in buffer

    public IntPtr init_destination;    
    public IntPtr empty_output_buffer;
    public IntPtr term_destination;



    // дополнительные поля из my_mem_destination_mgr
    // (!) не публичные

    /// <summary>target buffer</summary>
    public byte** outbuffer;    // unsigned char **outbuffer;
    public uint* outsize;
    /// <summary>newly allocated buffer</summary>
    public byte* newbuffer;

    /// <summary>start of buffer</summary>
    public byte* buffer;
    public long bufsize;
}
