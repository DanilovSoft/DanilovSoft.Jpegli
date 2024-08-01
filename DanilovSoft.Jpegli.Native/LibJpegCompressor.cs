namespace DanilovSoft.Jpegli.Native;

internal sealed class LibJpegCompressor : IDisposable
{
    public LibJpegCompressor()
    {

    }

    public void Dispose()
    {
    }

    public void Compress(int quality)
    {
        IntPtr errPtr = JpegStdError();
        JpegSetDefaults(errPtr);
    }

    private unsafe IntPtr JpegStdError()
    {
        var err = new jpeg_error_mgr();
        
        Debug.Assert(Marshal.SizeOf(err) == 168); // 164 абсолют; 168 для x64 (Pack 8)

        var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(err));
        Marshal.StructureToPtr(err, ptr, true);

        IntPtr ptrRet = Jpeg62.jpeg_std_errorPtr(ptr); // (!) it's actually cals jpegli_std_error
        Debug.Assert(ptrRet == ptr);

        Marshal.PtrToStructure(ptr, err); // копируем значения взад.
        
        Debug.Assert(err.jpeg_message_table != default); // array of string messages
        Debug.Assert(err.last_jpeg_message == 129); // array size

#if DEBUG

        for (int i = 0; i < err.last_jpeg_message; i++)
        {
            // Получаем указатель на текущую строку
            IntPtr stringPtr = Marshal.ReadIntPtr(err.jpeg_message_table, i * IntPtr.Size);

            // Преобразуем указатель в строку
            string message = Marshal.PtrToStringAnsi(stringPtr);

            Console.WriteLine("Сообщение {0}: {1}", i, message);
        }
#endif

        SetErrCallbacks(err);

        Marshal.StructureToPtr(err, ptr, fDeleteOld: false);

        return ptr;
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

    private void SetErrCallbacks(jpeg_error_mgr err)
    {
        // колбеки можно переопределять после вызова jpeg_std_error
        err.error_exit = Marshal.GetFunctionPointerForDelegate<jpeg_common_ptr_delegate>(CustomErrorExit);
        err.emit_message = Marshal.GetFunctionPointerForDelegate<jpeg_emit_message_delegate>(CustomEmitMessage);
        err.output_message = Marshal.GetFunctionPointerForDelegate<jpeg_output_message_delegate>(CustomOutputMessage);
        err.format_message = Marshal.GetFunctionPointerForDelegate<jpeg_format_message_delegate>(CustomFormatMessage);
        err.reset_error_mgr = Marshal.GetFunctionPointerForDelegate<jpeg_reset_error_mgr_delegate>(CustomResetErrorMgr);
    }

    private unsafe void JpegSetDefaults(IntPtr errPtr)
    {
        //var err = new jpeg_error_mgr();
        //Jpeg62.jpeg_std_error(err);
        //Debug.Assert(err.last_jpeg_message == 129);
        //SetErrCallbacks(err);

        var cinfo = new jpeg_compress_struct
        {
            err = errPtr,
            //global_state = GlobalState.CSTATE_START,
            in_color_space = J_COLOR_SPACE.JCS_RGB
        };

        var cinfoPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cinfo));
        Marshal.StructureToPtr(cinfo, cinfoPtr, false);

        try
        {
            // You must set in_color_space correctly before calling jpeg_set_defaults()
            Jpeg62.jpeg_set_defaults(cinfoPtr);
        }
        catch (Exception ex)
        {
        }
    }

    // does not return to caller
    private void CustomErrorExit(IntPtr cinfo)
    {
        var cinfoStruct = Marshal.PtrToStructure<jpeg_compress_struct>(cinfo);
        var err = Marshal.PtrToStructure<jpeg_error_mgr>(cinfoStruct.err)!;

        throw new LibJpegException { MsgCode = err.msg_code };
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
