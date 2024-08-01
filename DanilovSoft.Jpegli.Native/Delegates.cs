namespace DanilovSoft.Jpegli.Native;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public unsafe delegate void jpeg_common_ptr_delegate(IntPtr cinfo);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public unsafe delegate void jpeg_emit_message_delegate(IntPtr cinfo, int msg_level);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public unsafe delegate void jpeg_output_message_delegate(IntPtr cinfo);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public unsafe delegate void jpeg_format_message_delegate(IntPtr cinfo, IntPtr buffer);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public unsafe delegate void jpeg_reset_error_mgr_delegate(IntPtr cinfo);
