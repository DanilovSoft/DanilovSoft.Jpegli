using System.Runtime.CompilerServices;

namespace DanilovSoft.Jpegli.Native;

internal sealed unsafe class LibJpegCompressor : IDisposable
{
#if DEBUG
    [ModuleInitializer]
    public static void InitializeModule()
    {
        Debug.Assert(Marshal.SizeOf<jpeg_common_struct>() == 40); // 40 for x64
        Debug.Assert(Marshal.SizeOf<jpeg_error_mgr>() == 168); // 164 абсолют; 168 для x64 (Pack 8)
        Debug.Assert(Marshal.SizeOf<jpeg_compress_struct>() == 504); // 504 for x64
    }
#endif

    private readonly jpeg_error_mgr _errMgr = new();
    private readonly jpeg_compress_struct _cinfo = new();

    public LibJpegCompressor()
    {

    }

    public void Dispose()
    {
    }

    public unsafe void Compress(int quality)
    {
        _cinfo.err = JpegStdError();
        JpegSetDefaults();
    }

    private unsafe jpeg_error_mgr* JpegStdError()
    {
        var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(_errMgr)); // (!) do not forget to free
        Marshal.StructureToPtr(_errMgr, ptr, true);

        IntPtr ptrRet = Jpeg62.jpeg_std_errorPtr(ptr); // (!) it's actually cals jpegli_std_error
        Debug.Assert(ptrRet == ptr);

        Marshal.PtrToStructure(ptr, _errMgr); // копируем значения взад.
        
        Debug.Assert(_errMgr.jpeg_message_table != default); // array of string messages
        Debug.Assert(_errMgr.last_jpeg_message == 129); // array size

#if DEBUG

        for (int i = 0; i < _errMgr.last_jpeg_message; i++)
        {
            // Получаем указатель на текущую строку
            IntPtr stringPtr = Marshal.ReadIntPtr(_errMgr.jpeg_message_table, i * IntPtr.Size);

            // Преобразуем указатель в строку
            string message = Marshal.PtrToStringAnsi(stringPtr);

            Debug.WriteLine($"message_table[{i}]: {message}");
        }
#endif

        SetErrCallbacks(_errMgr);

        Marshal.StructureToPtr(_errMgr, ptr, fDeleteOld: false);

        return (jpeg_error_mgr*)ptr.ToPointer();
    }

    public static IDisposable Compress(byte[] data, int width, int height, int stride, int quality)
    {
        var buf = IntPtr.Zero;

        NativeMethods.TjFree(buf);

        throw new NotImplementedException();

        //ulong bufSize = 0;
        //try
        //{
        //    var result = NativeMethods.TjCompress2(
        //        this.compressorHandle,
        //        srcPtr,
        //        width,
        //        stride,
        //        height,
        //        (int)pixelFormat,
        //        ref buf,
        //        ref bufSize,
        //        (int)subSamp,
        //        quality,
        //        (int)flags);

        //    if (result == -1)
        //    {
        //        TJUtils.GetErrorAndThrow();
        //    }

        //    var jpegBuf = new byte[bufSize];
        //    Marshal.Copy(buf, jpegBuf, 0, (int)bufSize);
        //    return jpegBuf;
        //}
        //finally
        //{
        //    NativeMethods.TjFree(buf);
        //}
    }

    private unsafe void SetErrCallbacks(jpeg_error_mgr err)
    {
        // колбеки можно переопределять после вызова jpeg_std_error
        err.error_exit = Marshal.GetFunctionPointerForDelegate<jpeg_common_ptr_delegate>(CustomErrorExit);
        err.emit_message = Marshal.GetFunctionPointerForDelegate<jpeg_emit_message_delegate>(CustomEmitMessage);
        err.output_message = Marshal.GetFunctionPointerForDelegate<jpeg_output_message_delegate>(CustomOutputMessage);
        err.format_message = Marshal.GetFunctionPointerForDelegate<jpeg_format_message_delegate>(CustomFormatMessage);
        err.reset_error_mgr = Marshal.GetFunctionPointerForDelegate<jpeg_reset_error_mgr_delegate>(CustomResetErrorMgr);
    }

    private unsafe void JpegSetDefaults()
    {
        _cinfo.global_state = GlobalState.CSTATE_START;
        _cinfo.in_color_space = J_COLOR_SPACE.JCS_RGB; // You must set in_color_space correctly before calling jpeg_set_defaults()
        _cinfo.master = DebugCreateUnmanaged<jpeg_comp_master>();
        _cinfo.comp_info = DebugCreateUnmanaged<jpeg_component_info>();
        _cinfo.dest = DebugCreateUnmanaged<jpeg_destination_mgr>();

        //_cinfo.quant_tbl_ptrs = [1,2,3,4];

        var cinfoPtr = Marshal.AllocHGlobal(Marshal.SizeOf(_cinfo));
        Marshal.StructureToPtr(_cinfo, cinfoPtr, false);

        //_cinfo.quant_tbl_ptrs = [];
        //Marshal.PtrToStructure(cinfoPtr, _cinfo);

        try
        {
            Jpeg62.jpeg_CreateCompress((j_compress_ptr)cinfoPtr.ToPointer(), 62, (nuint)Marshal.SizeOf(_cinfo));

            Marshal.PtrToStructure(cinfoPtr, _cinfo);


            Jpeg62.jpeg_set_defaults(cinfoPtr);
        }
        catch (Exception ex)
        {
        }
    }

    private static T* DebugCreateUnmanaged<T>() where T : new()
    {
        var structure = new T();

        var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<T>()); // (!) do not forget to free
        Marshal.StructureToPtr(structure, ptr, true);

        return (T*)ptr.ToPointer();
    }

    // does not return to caller
    private void CustomErrorExit(jpeg_common_struct* cinfo)
    {
        Marshal.PtrToStructure((nint)cinfo, _cinfo);
        Marshal.PtrToStructure((nint)_cinfo.err, _errMgr);

        // TODO можно извлечь сообщение из таблицы!
        throw new LibJpegException { MsgCode = _errMgr.msg_code };
    }

    private void CustomEmitMessage(IntPtr cinfo, int msg_level)
    {
        // Custom emit message handler
        Console.WriteLine($"Emit message: {msg_level}");
    }

    private void CustomOutputMessage(IntPtr cinfo)
    {
        // Custom output message handler
        Console.WriteLine("Output message");
    }

    private void CustomFormatMessage(IntPtr cinfo, IntPtr buffer)
    {
        // Custom format message handler
        Console.WriteLine("Format message");
    }

    private void CustomResetErrorMgr(IntPtr cinfo)
    {
        // Custom reset error manager handler
        Console.WriteLine("Reset error manager");
    }
}
