using System.Runtime.InteropServices;

namespace DanilovSoft.Jpegli.Native;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal delegate void jpeg_common_ptr_delegate(IntPtr cinfo);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal unsafe delegate void jpeg_emit_message_delegate(IntPtr cinfo, int msg_level);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal unsafe delegate void jpeg_output_message_delegate(IntPtr cinfo);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal unsafe delegate void jpeg_compress_ptr_delegate(jpeg_compress_struct* cinfo);